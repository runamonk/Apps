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
                LoadFolder(App.AppId);
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
            Forms.Properties f = new Forms.Properties(AppsConfig);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                if (c is AppPanel)
                {
                    AddItem(null, f.AppName, f.AppFileName, f.AppIconPath, f.AppIconIndex, f.AppFileArgs, f.AppFileWorkingFolder, 0);
                }
                else
                {
                    AppButton b = GetAppButton(sender);
                    int i = Controls.GetChildIndex(b);
                    AddItem(null, f.AppName, f.AppFileName, f.AppIconPath, f.AppIconIndex, f.AppFileArgs, f.AppFileWorkingFolder, i);
                }
            }


            InMenu = false;
        }
        private void MenuAddFolder_Click(object sender, EventArgs e)
        {
            InMenu = true;
            Folder f = new Folder(AppsConfig);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                if (c is AppPanel)
                {
                    AddItem(null, f.FolderName, 0);
                }
                else
                {
                    AppButton b = GetAppButton(sender);
                    int i = Controls.GetChildIndex(b);
                    AddItem(null, f.FolderName, i);
                }
            }


            InMenu = false;
        }
        private void MenuAddFolderLink_Click(object sender, EventArgs e)
        {
            InMenu = true;
            FolderLink f = new FolderLink(AppsConfig);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                if (c is AppPanel)
                {
                    AddFolderLink(null, f.FolderName, f.FolderPath, 0);
                }
                else
                {
                    AppButton b = GetAppButton(sender);
                    int i = Controls.GetChildIndex(b);
                    AddFolderLink(null, f.FolderName, f.FolderPath, i);
                }
            }
            InMenu = false;
        }
        private void MenuAddSeparator_Click(object sender, EventArgs e)
        {
            InMenu = true;
            var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
            if (c is AppPanel)
            {
                AddSeparator(null, 0);
            }
            else
            {
                AppButton b = GetAppButton(sender);
                int i = Controls.GetChildIndex(b);
                AddSeparator(null, i);
            }
            InMenu = false;
        }
        private void MenuDelete_Click(object sender, EventArgs e)
        {
            InMenu = true;
            AppButton b = GetAppButton(sender);
            XmlNode node = GetNode(b.AppId);

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
                XmlNode parentNode = node.ParentNode;
                parentNode.RemoveChild(node);
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
            SuspendLayout();
            AppButton b = GetAppButton(sender);

            if (b.IsFolderLinkButton)
            {
                FolderLink f = new FolderLink(AppsConfig)
                {
                    FolderName = b.AppName,
                    FolderPath = b.FileName
                };

                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    XmlNode node = GetNode(b.AppId);
                    int i = Controls.GetChildIndex(b);
                    node.Attributes["folderlinkname"].Value = f.FolderName;
                    node.Attributes["folderlinkpath"].Value = f.FolderPath;
                    ((AppButton)Controls[i]).AppName = f.FolderName;
                    ((AppButton)Controls[i]).FileName = f.FolderPath;
                    SaveXML();
                }
                f.Dispose();
            }
            else
            if (b.IsFolderButton)
            {
                Folder f = new Folder(AppsConfig)
                {
                    FolderName = b.AppName
                };
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    XmlNode node = GetNode(b.AppId);
                    int i = Controls.GetChildIndex(b);
                    node.Attributes["foldername"].Value = f.FolderName;
                    ((AppButton)Controls[i]).AppName = f.FolderName;
                    SaveXML();
                }
                f.Dispose();
            }
            else
            {
                Forms.Properties f = new Forms.Properties(AppsConfig);
                f.SetFileProperties(b.AppName, b.FileName, b.FileIconPath, b.FileIconIndex, b.FileArgs, b.FileWorkingFolder);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    XmlNode node = GetNode(b.AppId);
                    int i = Controls.GetChildIndex(b);
                    XmlNode ParentNode = CurrentParentNode ?? AppsNode;
                    ParentNode.RemoveChild(node);
                    Controls.Remove(b);
                    AddItem(null, f.AppName, f.AppFileName, f.AppIconPath, f.AppIconIndex, f.AppFileArgs, f.AppFileWorkingFolder, i);
                    SaveXML();
                }
                f.Dispose();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            ResumeLayout();
            InMenu = false;
        }
        private void MenuMoveToParent_Click(object sender, EventArgs e)
        {
            SuspendLayout();
            AppButton b = GetAppButton(sender);
            XmlNode xn = GetNode(b.AppId);
            XmlNode pn = xn.ParentNode.ParentNode;
            pn.AppendChild(xn);
            MoveToCache(b, GetAttrib(pn, "id"));
            SaveXML();
            ResumeLayout();
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
                MoveToParentItem.Enabled = (GetNode(GetAppButton(sender).AppId).ParentNode.ParentNode.Name.ToLower() != "xml");
                UpMenuItem.Enabled = (GetPrevNode(GetNode(GetAppButton(sender).AppId)) != null);
                DownMenuItem.Enabled = (GetNextNode(GetNode(GetAppButton(sender).AppId)) != null);
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
            b.Height = 22;
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
        private void AddAttrib(XmlNode Node, string AttribName, string Value)
        {
            XmlAttribute XmlAtt;
            XmlAtt = AppsXml.CreateAttribute(AttribName);
            XmlAtt.Value = Value;
            Node.Attributes.Append(XmlAtt);
        }
        private void AddFiles(string[] Files, int AddAtIndex)
        {
            SuspendLayout();
            InLoad = true;
            foreach (string filePath in Files)
            {
                if (!File.Exists(filePath) && Directory.Exists(filePath))
                {
                    string AppName = Path.GetFileName(filePath);
                    AddFolderLink(null, AppName, filePath, AddAtIndex);
                }
                else
                if (File.Exists(filePath))
                {
                    string AppName = (string.IsNullOrEmpty(Funcs.GetFileInfo(filePath).ProductName) ? Path.GetFileNameWithoutExtension(filePath) : Funcs.GetFileInfo(filePath).ProductName);
                    AddItem(null, AppName, filePath, null, null, null, null, AddAtIndex);
                }
            }
            InLoad = false;
            ResumeLayout();
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
        public void AddItem(string AppId, string FolderName, int AddAtIndex)
        {
            AppButton b = AddAppButton(ButtonType.Folder, Controls);

            if (string.IsNullOrEmpty(AppId))
                AppId = Guid.NewGuid().ToString();

            XmlNode node = GetNode(b.AppId);
            if (node == null)
            {
                node = AppsXml.CreateNode(XmlNodeType.Element, "APP", null);
                XmlNode nodeSib = GetNode(((AppButton)Controls[AddAtIndex]).AppId);
                AddAttrib(node, "id", AppId);
                AddAttrib(node, "foldername", FolderName);
                XmlNode ParentNode = CurrentParentNode ?? AppsNode;
                if (nodeSib == null)
                    ParentNode.AppendChild(node);
                else
                    ParentNode.InsertAfter(node, nodeSib);
                SaveXML();
            }

            SetButtonDetails(b, node);
            AddToFolderCache(AppId);

            Controls.SetChildIndex(b, AddAtIndex); // move button where we want it.
            if (!InLoad)
                OnAppAdded?.Invoke();
        }
        public void AddItem(string AppId, string AppName, string fileName, string fileIconPath, string fileIconIndex, string fileArgs, string fileWorkingFolder, int AddAtIndex)
        {
            AppButton b = AddAppButton(ButtonType.App, Controls);
            if (string.IsNullOrEmpty(AppId))
                AppId = Guid.NewGuid().ToString();

            XmlNode node = GetNode(b.AppId);

            if (node == null)
            {
                XmlNode nodeSib = GetNode(((AppButton)Controls[AddAtIndex]).AppId);
                if ((AppsConfig.ParseShortcuts) && Funcs.IsShortcut(fileName))
                {
                    AppName = Path.GetFileNameWithoutExtension(fileName);
                    Funcs.ParseShortcut(fileName, ref fileName, ref fileIconPath, ref fileIconIndex, ref fileArgs, ref fileWorkingFolder);
                }

                node = AppsXml.CreateNode(XmlNodeType.Element, "APP", null);
                AddAttrib(node, "id", AppId);
                AddAttrib(node, "appname", AppName);
                AddAttrib(node, "filename", fileName);
                AddAttrib(node, "fileiconpath", fileIconPath);
                AddAttrib(node, "fileiconindex", fileIconIndex);
                AddAttrib(node, "fileargs", fileArgs);
                AddAttrib(node, "fileworkingfolder", fileWorkingFolder);
                XmlNode ParentNode = CurrentParentNode ?? AppsNode;
                if (nodeSib == null)
                    ParentNode.AppendChild(node);
                else
                    ParentNode.InsertAfter(node, nodeSib);
                SaveXML();
            }

            SetButtonDetails(b, node);
            Controls.SetChildIndex(b, AddAtIndex); // move button where we want it.
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
        public void AddFolderLink(string AppId, string FolderLinkName, string FolderPath, int AddAtIndex)
        {
            AppButton b = AddAppButton(ButtonType.FolderLink, Controls);
            if (string.IsNullOrEmpty(AppId))
                AppId = Guid.NewGuid().ToString();

            XmlNode node = GetNode(b.AppId);

            if (node == null)
            {
                node = AppsXml.CreateNode(XmlNodeType.Element, "APP", null);
                XmlNode nodeSib = GetNode(((AppButton)Controls[AddAtIndex]).AppId);
                AddAttrib(node, "id", AppId);
                AddAttrib(node, "folderlinkname", FolderLinkName);
                AddAttrib(node, "folderlinkpath", FolderPath);
                XmlNode ParentNode = CurrentParentNode ?? AppsNode;
                if (nodeSib == null)
                    ParentNode.AppendChild(node);
                else
                    ParentNode.InsertAfter(node, nodeSib);

                SaveXML();
            }
            SetButtonDetails(b, node);
            Controls.SetChildIndex(b, AddAtIndex); // move button where we want it.
            if (!InLoad)
                OnAppAdded?.Invoke();
        }
        public void AddSeparator(string AppId, int AddAtIndex)
        {
            AppButton b = AddAppButton(ButtonType.Separator, Controls);
            if (string.IsNullOrEmpty(AppId))
                AppId = Guid.NewGuid().ToString();

            XmlNode node = GetNode(b.AppId);

            if (node == null)
            {
                node = AppsXml.CreateNode(XmlNodeType.Element, "APP", null);
                XmlNode nodeSib = GetNode(((AppButton)Controls[AddAtIndex]).AppId);
                AddAttrib(node, "id", AppId);
                AddAttrib(node, "separator", "Y");
                
                XmlNode ParentNode = CurrentParentNode ?? AppsNode;
                if (nodeSib == null)
                    ParentNode.AppendChild(node);
                else
                    ParentNode.InsertAfter(node, nodeSib);

                SaveXML();
            }
            SetButtonDetails(b, node);
            Controls.SetChildIndex(b, AddAtIndex); // move button where we want it.
            if (!InLoad)
                OnAppAdded?.Invoke();
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
        private XmlNode GetNode(string AppId)
        {
            return AppsXml.SelectSingleNode(string.Format(AppIdLookup, AppId));
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
                LoadFolder(CurrentParentNode.ParentNode.Attributes["id"].Value);
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
                        SetButtonDetails(b, xn);
                    }
                    else
                    if (GetAttrib(xn, "foldername") != "")
                    {
                        AppButton b = AddAppButton(ButtonType.Folder, ac);
                        SetButtonDetails(b, xn);
                        AddToCache(xn); // Recsurvise add contents of folder
                    }
                    else
                    if (GetAttrib(xn, "separator") != "")
                    {
                        AppButton b = AddAppButton(ButtonType.Separator, ac);
                        SetButtonDetails(b, xn);
                    }
                    else
                    if (GetAttrib(xn, "appname") != "") 
                    {
                        AppButton b = AddAppButton(ButtonType.App, ac);
                        SetButtonDetails(b, xn);
                    }
                }
            }
            if (FolderCache.Count == 0)
                AddToCache(AppsNode);
        }
        private void LoadFolder(string AppId)
        {
            InLoad = true;
            SuspendLayout();
            MoveToCache();
            CurrentParentNode = GetNode(AppId);
            AddItems(CurrentParentNode);
            ResumeLayout();
            InLoad = false;
            OnAppsLoaded?.Invoke();
        }
        public void LoadItems()
        {
            SuspendLayout();
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
            ResumeLayout();
            OnAppsLoaded?.Invoke();
        }
        private void MoveButton(AppButton FromButton, AppButton ToButton)
        {
            SuspendLayout();
            XmlNode ParentNode = CurrentParentNode ?? AppsNode;
            if (ToButton == null)
            {
                ParentNode.AppendChild(GetNode(FromButton.AppId));
                Controls.SetChildIndex(FromButton, 0);
            }
            else
            {
                int t = GetAppButtonIndex(ToButton);
                int f = GetAppButtonIndex(FromButton);

                if (t < f)
                    ParentNode.InsertAfter(GetNode(FromButton.AppId), GetNode(ToButton.AppId));
                else
                    ParentNode.InsertBefore(GetNode(FromButton.AppId), GetNode(ToButton.AppId));

                Controls.SetChildIndex(FromButton, t);
            }

            SaveXML();

            OnAppAdded?.Invoke();
            ResumeLayout();
        }
        private void MoveButtonInto(AppButton FromButton, AppButton ToButton)
        {
            SuspendLayout();
            XmlNode ParentNode = GetNode(ToButton.AppId);
            ParentNode.AppendChild(GetNode(FromButton.AppId));
            MoveToCache(FromButton, GetAttrib(ParentNode, "id"));
            SaveXML();
            ResumeLayout();
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
        private void SetButtonDetails(AppButton appButton, XmlNode xn)
        {
            if (appButton.ButtonType == ButtonType.FolderLink)
            {
                appButton.AppId = GetAttrib(xn, "id");
                appButton.AppName = GetAttrib(xn, "folderlinkname");
                appButton.FileName = GetAttrib(xn, "folderlinkpath");
            }
            else
            if (appButton.ButtonType == ButtonType.Folder)
            {
                appButton.AppId = GetAttrib(xn, "id");
                appButton.AppName = GetAttrib(xn, "foldername");
            }
            else
            if (appButton.ButtonType == ButtonType.App)
            {
                appButton.AppId = GetAttrib(xn, "id");
                appButton.AppName = GetAttrib(xn, "appname");
                appButton.FileName = GetAttrib(xn, "filename");
                appButton.FileArgs = GetAttrib(xn, "fileargs");
                appButton.FileIconIndex = GetAttrib(xn, "fileiconindex");
                appButton.FileIconPath = GetAttrib(xn, "fileiconpath");
                appButton.FileWorkingFolder = GetAttrib(xn, "fileworkingfolder");
                if (appButton.FileIconImage == null)
                    appButton.WatchForIconUpdate = true;
            }
            else
            if (appButton.ButtonType == ButtonType.Separator)
            {
                appButton.AppId = GetAttrib(xn, "id");
            }
        }
        private void SetColors()
        {
            BackColor = AppsConfig.AppsBackColor;
        }

        #endregion
    }
}
