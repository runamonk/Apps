using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Utility;
using Apps.Forms;
using System.Collections.Generic;
using System.Windows;


namespace Apps.Controls
{
    public partial class AppPanel : Panel
    {
        public AppPanel(Config myConfig)
        {
            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += new EventHandler(ConfigChanged);
            AutoScroll = false;
            FolderCache = new List<AppCache>();
            MenuRC = new AppMenu(myConfig)
            {
                ShowCheckMargin = false,
                ShowImageMargin = false
            };
            MenuRC.Opening += new CancelEventHandler(Menu_Opening);
            Funcs.AddMenuItem(MenuRC, "Add Application", MenuAddApp_Click);
            Funcs.AddMenuItem(MenuRC, "Add Folder", MenuAddFolder_Click);
            Funcs.AddMenuItem(MenuRC, "Add Folder Link", MenuAddFolderLink_Click);
            Funcs.AddMenuItem(MenuRC, "Add Separator", MenuAddSeparator_Click);
            EditMenuItem = Funcs.AddMenuItem(MenuRC, "Edit", MenuEdit_Click);
            DeleteMenuItem = Funcs.AddMenuItem(MenuRC, "Delete", MenuDelete_Click);
            MenuRC.Items.Add(new ToolStripSeparator());
            MoveToParentItem = Funcs.AddMenuItem(MenuRC, "Move To Parent", MenuMoveToParent_Click);
            UpMenuItem = Funcs.AddMenuItem(MenuRC, "Move Up", MenuUp_Click);
            DownMenuItem = Funcs.AddMenuItem(MenuRC, "Move Down", MenuDown_Click);
            this.ContextMenuStrip = MenuRC;
            this.AllowDrop = true;
            this.DragOver += new DragEventHandler(OnDragOver);
            this.DragDrop += new DragEventHandler(DropToPanel);
            SetColors();
            LoadItems();
        }

        #region Properties
        private Config AppsConfig { get; set; }
        public string GetRootId
        {
            get {
                return FRootId;
            }
        }
        public bool InMenu { get; set; }
        public bool InLoad { get; set; }
        public bool InAFolder
        {
            get {
                return (CurrentParentNode != null);
            }
        }
        public string CurrentAppId
        {
            get {
                if (CurrentParentNode != null)
                    return CurrentParentNode.Attributes["id"].Value;
                else
                    return GetRootId;
            }
        }
        public string CurrentFolderName
        {
            get {
                if (CurrentParentNode != null)
                    return CurrentParentNode.Attributes["foldername"].Value;
                else
                    return "";
            }
        }
        #endregion

        #region Privates
        private const string AppIdLookup = "//APPS//APP[@id='{0}']";
        private XmlNode AppsNode;
        private XmlDocument AppsXml;
        private readonly string AppsXmlFilePath = Funcs.AppPath() + "\\Apps.xml";
        private XmlNode CurrentParentNode = null;
        private readonly ToolStripMenuItem DeleteMenuItem;
        private readonly ToolStripMenuItem DownMenuItem;
        private readonly ToolStripMenuItem EditMenuItem;
        private readonly List<AppCache> FolderCache;
        private readonly string FRootId = Guid.NewGuid().ToString();
        private readonly AppMenu MenuRC;
        private readonly ToolStripMenuItem MoveToParentItem;
        private const string New_AppsXml_file = "<XML VERSION=\"1.0\" ENCODING=\"utf-8\">\r\n<APPS>\r\n</APPS>\r\n</XML>";
        private readonly ToolStripMenuItem UpMenuItem;
        private const int WM_SETREDRAW = 11;
        #endregion

        #region EventHandlers
        public delegate void AppAddedHandler();
        public event AppAddedHandler OnAppAdded;

        public delegate void AppClickedHandler();
        public event AppClickedHandler OnAppClicked;

        public delegate void AppDeletedHandler();
        public event AppDeletedHandler OnAppDeleted;

        public delegate void AppsLoadedHandler();
        public event AppsLoadedHandler OnAppsLoaded;
        #endregion

        #region Events
        private void ButtonClicked(AppButton App)
        {
            if (!App.IsFolderButton)
                OnAppClicked?.Invoke();
            else
            {
                LoadFolder(App);
            }
        }
        private void ConfigChanged(object sender, EventArgs e)
        {
            SetColors();
        }
        private void DropToButton(AppButton ToAppButton, DragEventArgs e)
        {
            InMenu = true;
            object objRef = (object)e.Data.GetData(typeof(AppButton));
            if ((objRef != null) && (objRef is AppButton b))
            {
                if (ToAppButton.IsFolderButton)
                {
                    Confirm c = new Confirm(AppsConfig);
                    DialogResult r = c.ShowAsDialog(ConfirmButtons.YesNo, "Move " + b.AppName + " into folder " + ToAppButton.AppName + "?", "Move " + b.AppName + "?");
                    if (r == DialogResult.Yes)
                    {
                        MoveButtonInto(b, ToAppButton);
                    }
                    else
                        MoveButton(b, ToAppButton);
                }
                else
                    MoveButton(b, ToAppButton);
            }
            else
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);

                if ((fileNames != null) && (fileNames.Count() > 0))
                    AddFiles(fileNames, Controls.GetChildIndex(ToAppButton));
                else
                {
                    //IEnumerable<string> s = Funcs.GetPathsFromShellIDListArray(e.Data);
                    //MemoryStream ms;
                    //FileStream file;
                    //ms = ((MemoryStream)e.Data.GetData("Shell IDList Array"));
                    //file = new FileStream("c:\\file.bin", FileMode.Create, FileAccess.Write);
                    //ms.WriteTo(file);
                    //file.Close();
                    //ms.Close();
                }
            }
            InMenu = false;
        }
        private void DropToPanel(object sender, DragEventArgs e)
        {
            object objRef = (object)e.Data.GetData(typeof(AppButton));
            if ((objRef != null) && (objRef is AppButton b))
            {
                MoveButton(b, null);
            }
            else
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                AddFiles((string[])e.Data.GetData(DataFormats.FileDrop), 0);
            }
        }
        private void MenuAddApp_Click(object sender, EventArgs e)
        {
            InMenu = true;
            AppButton b = AddAppButton(ButtonType.App, Controls);
            Forms.Properties f = new Forms.Properties(AppsConfig, b);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                if (c is AppPanel)
                {
                    AddApp(b, 0);
                }
                else
                {
                    AddApp(b, Controls.GetChildIndex(GetAppButton(sender)));
                }
            }
            else
            {
                Controls.Remove(b);
                b.Dispose();
            }

            InMenu = false;
        }
        private void MenuAddFolder_Click(object sender, EventArgs e)
        {
            InMenu = true;
            BeginUpdate();
            AppButton appButton = AddAppButton(ButtonType.Folder, Controls);
            Folder f = new Folder(AppsConfig, appButton);

            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                if (c is AppPanel)
                {
                    AddFolder(appButton, 0);
                }
                else
                {
                    AddFolder(appButton, Controls.GetChildIndex(GetAppButton(sender)));
                }
            }
            else
            {
                Controls.Remove(appButton);
                appButton.Dispose();
            }
            EndUpdate();
            InMenu = false;
        }
        private void MenuAddFolderLink_Click(object sender, EventArgs e)
        {
            InMenu = true;
            BeginUpdate();
            AppButton appButton = AddAppButton(ButtonType.FolderLink, Controls);
            FolderLink f = new FolderLink(AppsConfig, appButton);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                if (c is AppPanel)
                {
                    AddFolderLink(appButton, 0);
                }
                else
                {
                    AddFolderLink(appButton, Controls.GetChildIndex(GetAppButton(sender)));
                }
            }
            else
            {
                Controls.Remove(appButton);
                appButton.Dispose();
            }

            EndUpdate();
            InMenu = false;
        }
        private void MenuAddSeparator_Click(object sender, EventArgs e)
        {
            InMenu = true;
            var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
            if (c is AppPanel)
            {
                AddSeparator(0);
            }
            else
            {
                AddSeparator(Controls.GetChildIndex(GetAppButton(sender)));
            }
            InMenu = false;
        }
        private void MenuDelete_Click(object sender, EventArgs e)
        {
            InMenu = true;
            AppButton b = GetAppButton(sender);

            string s = "";
            if (b.IsAppButton)
                s = "application?";
            else
            if (b.IsFolderButton)
                s = "folder and all children?";
            else
            if (b.IsFolderLinkButton)
                s = "folder link?";
            else
            if (b.IsSeparatorButton)
                s = "Separator?";

            bool CanDelete = (Misc.ConfirmDialog(AppsConfig, ConfirmButtons.OKCancel, "Delete " + s, "Delete " + b.AppName + "?") == DialogResult.OK);

            if (CanDelete)
            {
                XmlNode parentNode = b.Node.ParentNode;
                parentNode.RemoveChild(b.Node);
                Controls.Remove(b);
                // Cleanup cache?
                SaveXML();
                OnAppDeleted?.Invoke();
            }
            InMenu = false;
        }
        private void MenuDown_Click(object sender, EventArgs e)
        {
            InMenu = true;
            MoveButton(GetAppButton(sender), (AppButton)Controls[GetAppButtonIndex(GetAppButton(sender)) - 1]);
            InMenu = false;
        }
        private void MenuUp_Click(object sender, EventArgs e)
        {
            InMenu = true;
            MoveButton(GetAppButton(sender), (AppButton)Controls[GetAppButtonIndex(GetAppButton(sender)) + 1]);
            InMenu = false;
        }
        private void MenuEdit_Click(object sender, EventArgs e)
        {
            InMenu = true;
            BeginUpdate();
            AppButton b = GetAppButton(sender);

            if (b.IsFolderLinkButton)
            {
                FolderLink f = new FolderLink(AppsConfig, b);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    SetAttrib(b.Node, "folderlinkname", b.AppName);
                    SetAttrib(b.Node, "folderlinkpath", b.FileName);
                    SaveXML();
                }
                f.Dispose();
            }
            else
            if (b.IsFolderButton)
            {
                Folder f = new Folder(AppsConfig, b);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    SetAttrib(b.Node, "foldername", b.AppName);
                    SaveXML();
                }
                f.Dispose();
            }
            else
            {
                Forms.Properties f = new Forms.Properties(AppsConfig, b);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    SetAttrib(b.Node, "appname", b.AppName);
                    SetAttrib(b.Node, "filename", b.FileName);
                    SetAttrib(b.Node, "fileiconpath", b.FileIconPath);
                    SetAttrib(b.Node, "fileiconindex", b.FileIconIndex);
                    SetAttrib(b.Node, "fileargs", b.FileArgs);
                    SetAttrib(b.Node, "fileworkingfolder", b.FileWorkingFolder);
                    SaveXML();
                }
                f.Dispose();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            EndUpdate();
            InMenu = false;
        }
        private void MenuMoveToParent_Click(object sender, EventArgs e)
        {
            BeginUpdate();
            AppButton b = GetAppButton(sender);
            XmlNode pn = b.Node.ParentNode.ParentNode;
            pn.AppendChild(b.Node);
            MoveToCache(b, GetAttrib(pn, "id"));
            SaveXML();
            EndUpdate();
            OnAppDeleted?.Invoke();
        }
        private void Menu_Opening(object sender, CancelEventArgs e)
        {
            var c = ((AppMenu)sender).SourceControl;
            if (c is AppPanel)
            {
                DeleteMenuItem.Enabled = false;
                EditMenuItem.Enabled = false;
                MoveToParentItem.Enabled = false;
                UpMenuItem.Enabled = false;
                DownMenuItem.Enabled = false;
            }
            else
            if (c is AppButtonText)
            {
                DeleteMenuItem.Enabled = true;
                EditMenuItem.Enabled = (GetAppButton(sender).IsSeparatorButton != true);
                MoveToParentItem.Enabled = (GetAppButton(sender).Node.ParentNode.ParentNode.Name.ToLower() != "xml");
                UpMenuItem.Enabled = (GetPrevNode(GetAppButton(sender).Node) != null);
                DownMenuItem.Enabled = (GetNextNode(GetAppButton(sender).Node) != null);
            }
        }
        private void OnDragOver(object sender, DragEventArgs e)
        {
            e.Effect = (DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move);
        }
        #endregion

        #region Methods
        private AppButton AddAppButton(ButtonType buttonType, dynamic AddTo)
        {
            AppButton b = new AppButton(AppsConfig, buttonType);
            b.OnAppButtonClicked += new AppButton.AppButtonClickedHandler(ButtonClicked);
            b.OnAppButtonDropped += new AppButton.AppButtonDropEventhandler(DropToButton);
            b.ContextMenuStrip = MenuRC;
            b.Padding = new Padding(0, 0, 0, 0);
            b.Margin = new Padding(0, 0, 0, 0);
            b.TabStop = false;
            if (AddTo is AppCache cache)
                cache.Insert(0, b);
            else
                Controls.Add(b);

            b.AutoSize = false;
            b.Dock = DockStyle.Top;
            return b;
        }
        private void AddFiles(string[] Files, int AddAtIndex)
        {
            BeginUpdate();
            InLoad = true;
            foreach (string filePath in Files)
            {
                if (!File.Exists(filePath) && Directory.Exists(filePath))
                {
                    string AppName = Path.GetFileName(filePath);
                    AppButton b = AddAppButton(ButtonType.FolderLink, Controls);
                    b.AppName = AppName;
                    b.FileName = filePath;
                    AddFolderLink(b, AddAtIndex);
                }
                else
                if (File.Exists(filePath))
                {
                    string AppName = (string.IsNullOrEmpty(Funcs.GetFileInfo(filePath).ProductName) ? Path.GetFileNameWithoutExtension(filePath) : Funcs.GetFileInfo(filePath).ProductName);
                    AppButton b = AddAppButton(ButtonType.App, Controls);
                    b.AppName = AppName;
                    b.FileName = filePath;
                    AddApp(b, AddAtIndex);
                }
            }
            InLoad = false;
            EndUpdate();
            OnAppAdded?.Invoke();
        }
        private AppCache AddToFolderCache(string id)
        {
            var f = FolderCache.Find(x => x.FolderId == id);
            if (f == null)
            {
                FolderCache.Add(new AppCache(id));
                return FolderCache[FolderCache.Count - 1];
            }
            else
                return f;
        }
        public void AddFolder(AppButton appButton, int AddAtIndex)
        {
            appButton.AppId = Guid.NewGuid().ToString();
            appButton.Node = AppsXml.CreateNode(XmlNodeType.Element, "APP", null);
            XmlNode nodeSib = ((AppButton)Controls[AddAtIndex]).Node;
            SetAttrib(appButton.Node, "id", appButton.AppId);
            SetAttrib(appButton.Node, "foldername", appButton.AppName);
            XmlNode ParentNode = CurrentParentNode ?? AppsNode;
            if (nodeSib == null)
                ParentNode.AppendChild(appButton.Node);
            else
                ParentNode.InsertAfter(appButton.Node, nodeSib);
            SaveXML();

            SetButtonDetails(appButton);
            AddToFolderCache(appButton.AppId);
            Controls.SetChildIndex(appButton, AddAtIndex); // move button where we want it.
            if (!InLoad)
                OnAppAdded?.Invoke();
        }
        public void AddApp(AppButton appButton, int AddAtIndex)
        {
            if (string.IsNullOrEmpty(appButton.AppId))
                appButton.AppId = Guid.NewGuid().ToString();

            if (appButton.Node == null)
            {
                XmlNode nodeSib = ((AppButton)Controls[AddAtIndex]).Node;

                if ((AppsConfig.ParseShortcuts) && Funcs.IsShortcut(appButton.FileName))
                {
                    appButton.AppName = Path.GetFileNameWithoutExtension(appButton.FileName);
                    Funcs.ParseShortcut(appButton.FileName, appButton.FileName, appButton.FileIconPath, appButton.FileIconIndex, appButton.FileArgs, appButton.FileWorkingFolder);
                }

                appButton.Node = AppsXml.CreateNode(XmlNodeType.Element, "APP", null);
                SetAttrib(appButton.Node, "id", appButton.AppId);
                SetAttrib(appButton.Node, "appname", appButton.AppName);
                SetAttrib(appButton.Node, "filename", appButton.FileName);
                SetAttrib(appButton.Node, "fileiconpath", appButton.FileIconPath);
                SetAttrib(appButton.Node, "fileiconindex", appButton.FileIconIndex);
                SetAttrib(appButton.Node, "fileargs", appButton.FileArgs);
                SetAttrib(appButton.Node, "fileworkingfolder", appButton.FileWorkingFolder);
                XmlNode ParentNode = CurrentParentNode ?? AppsNode;
                if (nodeSib == null)
                    ParentNode.AppendChild(appButton.Node);
                else
                    ParentNode.InsertAfter(appButton.Node, nodeSib);
                SaveXML();
            }

            SetButtonDetails(appButton);
            Controls.SetChildIndex(appButton, AddAtIndex); // move button where we want it.
            if (!InLoad)
                OnAppAdded?.Invoke();
        }
        private void AddItems(XmlNode Nodes)
        {
            string id = GetAttrib(Nodes, "id");
            if (id == "")
                id = GetRootId;

            AppCache ac = FolderCache.Find(x => x.FolderId == id);
            foreach (Control c in ac)
            {
                Controls.Add(c);
            }
            ac.Clear();
        }
        public void AddFolderLink(AppButton appButton, int AddAtIndex)
        {
            appButton.AppId = Guid.NewGuid().ToString();
            appButton.Node = AppsXml.CreateNode(XmlNodeType.Element, "APP", null);

            XmlNode nodeSib = ((AppButton)Controls[AddAtIndex]).Node;
            SetAttrib(appButton.Node, "id", appButton.AppId);
            SetAttrib(appButton.Node, "folderlinkname", appButton.AppName);
            SetAttrib(appButton.Node, "folderlinkpath", appButton.FileName);
            XmlNode ParentNode = CurrentParentNode ?? AppsNode;
            if (nodeSib == null)
                ParentNode.AppendChild(appButton.Node);
            else
                ParentNode.InsertAfter(appButton.Node, nodeSib);

            SaveXML();
            SetButtonDetails(appButton);
            Controls.SetChildIndex(appButton, AddAtIndex); // move button where we want it.
            if (!InLoad)
                OnAppAdded?.Invoke();
        }
        public void AddSeparator(int AddAtIndex)
        {
            AppButton appButton = AddAppButton(ButtonType.Separator, Controls);
            appButton.AppId = Guid.NewGuid().ToString();
            appButton.Node = AppsXml.CreateNode(XmlNodeType.Element, "APP", null);
            XmlNode nodeSib = ((AppButton)Controls[AddAtIndex]).Node;
            SetAttrib(appButton.Node, "id", appButton.AppId);
            SetAttrib(appButton.Node, "separator", "Y");

            XmlNode ParentNode = CurrentParentNode ?? AppsNode;
            if (nodeSib == null)
                ParentNode.AppendChild(appButton.Node);
            else
                ParentNode.InsertAfter(appButton.Node, nodeSib);
            SaveXML();
            SetButtonDetails(appButton);
            Controls.SetChildIndex(appButton, AddAtIndex); // move button where we want it.
            if (!InLoad)
                OnAppAdded?.Invoke();
        }
        public void BeginUpdate()
        {
            Funcs.SendMessage(this.Handle, WM_SETREDRAW, false, 0);
        }
        public void EndUpdate()
        {
            Funcs.SendMessage(this.Handle, WM_SETREDRAW, true, 0);
            this.Refresh();
        }
        private AppButton GetAppButton(object sender)
        {
            if (sender is AppMenu menu)
            {
                var c = menu.SourceControl;
                return ((AppButton)(c.Parent.Parent.Parent)); // Label > Panel > Panel > AppButton
            }
            else
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                return ((AppButton)(c.Parent.Parent.Parent)); // Label > Panel > Panel > AppButton
            }
        }
        private int GetAppButtonIndex(AppButton appButton)
        {
            return Controls.GetChildIndex(appButton);
        }
        private string GetAttrib(XmlNode xn, string AttributeName)
        {
            if (xn.Attributes[AttributeName] != null)
                return xn.Attributes[AttributeName].Value;
            else
                return "";
        }
        private XmlNode GetPrevNode(XmlNode node)
        {
            return node.PreviousSibling;
        }
        private XmlNode GetNextNode(XmlNode node)
        {
            return node.NextSibling;
        }
        public void GoBack()
        {
            if ((CurrentParentNode.ParentNode != null) && (CurrentParentNode.ParentNode.Attributes["id"] != null) && !ModifierKeys.HasFlag(Keys.Control))
            {
                LoadFolder(LookupFolderButton(CurrentParentNode.ParentNode.Attributes["id"].Value));
            }
               
            else
                LoadItems();
        }
        private void LoadCache()
        {
            void AddToCache(XmlNode nodes)
            {
                string id = GetAttrib(nodes, "id");
                if (id == "")
                    id = GetRootId;

                AppCache ac = AddToFolderCache(id);

                foreach (XmlNode xn in nodes)
                {
                    if (GetAttrib(xn, "folderlinkname") != "")
                    {
                        AppButton b = AddAppButton(ButtonType.FolderLink, ac);
                        b.Node = xn;
                        SetButtonDetails(b);
                    }
                    else
                    if (GetAttrib(xn, "foldername") != "")
                    {
                        AppButton b = AddAppButton(ButtonType.Folder, ac);
                        b.Node = xn;
                        SetButtonDetails(b);
                        AddToCache(xn); // Recsurvise add contents of folder
                    }
                    else
                    if (GetAttrib(xn, "separator") != "")
                    {
                        AppButton b = AddAppButton(ButtonType.Separator, ac);
                        b.Node = xn;
                        SetButtonDetails(b);
                    }
                    else
                    if (GetAttrib(xn, "appname") != "") 
                    {
                        AppButton b = AddAppButton(ButtonType.App, ac);
                        b.Node = xn;
                        SetButtonDetails(b);
                    }
                }
            }
            if (FolderCache.Count == 0)
                AddToCache(AppsNode);
        }
        private void LoadFolder(AppButton appButton)
        {
            InLoad = true;
            BeginUpdate();
            MoveToCache();
            CurrentParentNode = appButton.Node;
            AddItems(CurrentParentNode);
            EndUpdate();
            InLoad = false;
            OnAppsLoaded?.Invoke();
        }
        public void LoadItems()
        {
            BeginUpdate();
            InLoad = true;
            bool doLoad = false;

            if (CurrentParentNode != null)
            {
                doLoad = true;
                MoveToCache();
            }

            if (AppsNode == null)
            {
                doLoad = true;
                if (!File.Exists(AppsXmlFilePath))
                {
                    AppsXml = new XmlDocument();
                    AppsXml.LoadXml(New_AppsXml_file);
                    SaveXML();
                }
                else
                {
                    AppsXml = new XmlDocument();
                    AppsXml.Load(AppsXmlFilePath);
                }
                AppsNode = AppsXml.SelectSingleNode("//APPS");
            }

            if (doLoad)
            {
                LoadCache();
                AddItems(AppsNode);
            }

            InLoad = false;
            EndUpdate();
            OnAppsLoaded?.Invoke();
        }
        private AppButton LookupFolderButton(string id)
        {
            foreach (AppCache a in FolderCache)
            {
                foreach (AppButton b in a)
                {
                    if (b.AppId == id)
                        return b;
                }
            }
            return  null;
        }
        private void MoveButton(AppButton FromButton, AppButton ToButton)
        {
            BeginUpdate();
            XmlNode ParentNode = CurrentParentNode ?? AppsNode;
            if (ToButton == null)
            {
                ParentNode.AppendChild(FromButton.Node);
                Controls.SetChildIndex(FromButton, 0);
            }
            else
            {
                int t = GetAppButtonIndex(ToButton);
                int f = GetAppButtonIndex(FromButton);

                if (t < f)
                    ParentNode.InsertAfter(FromButton.Node, ToButton.Node);
                else
                    ParentNode.InsertBefore(FromButton.Node, ToButton.Node);

                Controls.SetChildIndex(FromButton, t);
            }

            SaveXML();

            OnAppAdded?.Invoke();
            EndUpdate();
        }
        private void MoveButtonInto(AppButton FromButton, AppButton ToButton)
        {
            BeginUpdate();
            XmlNode ParentNode = ToButton.Node;
            ParentNode.AppendChild(FromButton.Node);
            MoveToCache(FromButton, GetAttrib(ParentNode, "id"));
            SaveXML();
            EndUpdate();
            OnAppDeleted?.Invoke();
        }
        private void MoveToCache()
        {
            string id = CurrentAppId;
            CurrentParentNode = null;
            AppCache f = AddToFolderCache(id);

            foreach (Control c in Controls)
            {
                f.Add(c);
            }
            Controls.Clear();
        }
        private void MoveToCache(AppButton appButton, String toId)
        {
            if (toId == "")
                toId = GetRootId;
            AppCache ac = AddToFolderCache(toId);
            if (ac != null)
                ac.Insert(0, appButton);
            Controls.Remove(appButton);
        }
        private void SaveXML()
        {
            AppsXml.Save(AppsXmlFilePath);
        }
        private void SetAttrib(XmlNode Node, string AttribName, string Value)
        {
            XmlAttribute XmlAtt;
            if (Node.Attributes[AttribName] == null)
            {
                XmlAtt = AppsXml.CreateAttribute(AttribName);
            }
            else
                XmlAtt = Node.Attributes[AttribName];

            XmlAtt.Value = Value;
            Node.Attributes.Append(XmlAtt);
        }
        private void SetButtonDetails(AppButton appButton)
        {
            if (appButton.ButtonType == ButtonType.FolderLink)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
                appButton.AppName = GetAttrib(appButton.Node, "folderlinkname");
                appButton.FileName = GetAttrib(appButton.Node, "folderlinkpath");
            }
            else
            if (appButton.ButtonType == ButtonType.Folder)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
                appButton.AppName = GetAttrib(appButton.Node, "foldername");
            }
            else
            if (appButton.ButtonType == ButtonType.App)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
                appButton.AppName = GetAttrib(appButton.Node, "appname");
                appButton.FileName = GetAttrib(appButton.Node, "filename");
                appButton.FileArgs = GetAttrib(appButton.Node, "fileargs");
                appButton.FileIconIndex = GetAttrib(appButton.Node, "fileiconindex");
                appButton.FileIconPath = GetAttrib(appButton.Node, "fileiconpath");
                appButton.FileWorkingFolder = GetAttrib(appButton.Node, "fileworkingfolder");
                if (appButton.FileIconImage == null)
                    appButton.WatchForIconUpdate = true;
            }
            else
            if (appButton.ButtonType == ButtonType.Separator)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
            }
        }
        private void SetColors()
        {
            BackColor = AppsConfig.AppsBackColor;
        }

        #endregion
    }
}
