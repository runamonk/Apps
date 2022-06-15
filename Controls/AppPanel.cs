﻿using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Utility;
using Apps.Forms;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Shell;

namespace Apps.Controls
{
    public partial class AppPanel : Panel
    {
        public AppPanel(Config myConfig)
        {
            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += new EventHandler(ConfigChanged);
           
            // Hide scrollbars and then enable AutoScroll.
            AutoScroll = false;
            HorizontalScroll.Maximum = 0;
            HorizontalScroll.Visible = false;
            VerticalScroll.Maximum = 0;
            VerticalScroll.Visible = false;
            AutoScroll = true;

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
            Funcs.AddMenuItem(MenuRC, "Add URL", MenuAddURL_Click);
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
        private bool InLoad { get; set; }
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
        private XmlNode AppsNode;
        private XmlDocument AppsXml;
        private readonly string AppsXmlFilePath = Funcs.AppPath() + "\\Apps.xml";
        private int BeginUpdateCounter = 0;
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
        public delegate void AppClickedHandler();
        public event AppClickedHandler OnAppClicked;
        public delegate void AppsChangedHandler();
        public event AppsChangedHandler OnAppsChanged;
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
                DoExternalDropTo(ToAppButton, e);

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
                DoExternalDropTo(null, e);
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
            b.Visible = true;
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
            appButton.Visible = true;
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
            appButton.Visible = true;
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
        private void MenuAddURL_Click(object sender, EventArgs e)
        {
            InMenu = true;
            BeginUpdate();
            AppButton appButton = AddAppButton(ButtonType.Url, Controls);
            WebLink f = new WebLink(AppsConfig, appButton);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                if (c is AppPanel)
                {
                    AddUrl(appButton, 0);
                }
                else
                {
                    AddUrl(appButton, Controls.GetChildIndex(GetAppButton(sender)));
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
                BeginUpdate();
                XmlNode parentNode = b.Node.ParentNode;
                parentNode.RemoveChild(b.Node);
                Controls.Remove(b);
                // Cleanup cache?
                SaveXML();
                EndUpdate();
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

            if (b.IsUrlButton)
            {
                WebLink f = new WebLink(AppsConfig, b);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    SetAttrib(b.Node, "appname", b.AppName);
                    SetAttrib(b.Node, "url", b.Url);
                    SetAttrib(b.Node, "favicon", b.FavIcon);
                    SaveXML();
                }
                f.Dispose();
            }
            else
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
                    SetAttrib(b.Node, "asadmin", b.AsAdmin);
                    SetAttrib(b.Node, "filename", b.FileName);
                    SetAttrib(b.Node, "fileiconpath", b.FileIconPath);
                    SetAttrib(b.Node, "fileiconindex", b.FileIconIndex);
                    SetAttrib(b.Node, "fileargs", b.FileArgs);
                    SetAttrib(b.Node, "fileworkingfolder", b.FileWorkingFolder);
                    SaveXML();
                }
                f.Dispose();
            }
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
            // hide the button when it's added intitially, this way the hidden scrollbars don't flash visible/invisible.
            // button is set back to visible in the AppAdded event in the main form. It's done this way because the form
            // is automatically resized based on the number of buttons.
            // Also we don't want to see a newly created (blank) button popup in the AppPanel.
            if (!InLoad)
                b.Visible = false;

            if (AddTo is AppCache cache)
                cache.Insert(0, b);
            else
                Controls.Add(b);

            b.AutoSize = false;
            b.Dock = DockStyle.Top;
            return b;
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
                    appButton.ParseShortcut();
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
            appButton.Visible = true;
            if (!InLoad)
                OnAppsChanged?.Invoke();
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
        }
        private void AddItems(XmlNode Nodes)
        {
            string id = GetAttrib(Nodes, "id");
            if (id == "")
                id = GetRootId;
            AppCache ac = FolderCache.Find(x => x.FolderId == id);
            Controls.AddRange(ac.ToArray());
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
            appButton.Visible = true;
        }
        public void AddSeparator(int AddAtIndex)
        {
            BeginUpdate();
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
            appButton.Visible = true;
            EndUpdate();
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
        private void AddUrl(AppButton appButton, int AddAtIndex)
        {
            appButton.AppId = Guid.NewGuid().ToString();
            appButton.Node = AppsXml.CreateNode(XmlNodeType.Element, "APP", null);
            XmlNode nodeSib = ((AppButton)Controls[AddAtIndex]).Node;
            SetAttrib(appButton.Node, "id", appButton.AppId);
            SetAttrib(appButton.Node, "appname", appButton.AppName);
            SetAttrib(appButton.Node, "url", appButton.Url);
            SetAttrib(appButton.Node, "favicon", appButton.FavIcon); 
            XmlNode ParentNode = CurrentParentNode ?? AppsNode;
            if (nodeSib == null)
                ParentNode.AppendChild(appButton.Node);
            else
                ParentNode.InsertAfter(appButton.Node, nodeSib);
            SaveXML();

            SetButtonDetails(appButton);
            AddToFolderCache(appButton.AppId);
            Controls.SetChildIndex(appButton, AddAtIndex); // move button where we want it.
            appButton.Visible = true;
        }
        public void BeginUpdate()
        {
            BeginUpdateCounter++;
            Funcs.SendMessage(this.Handle, WM_SETREDRAW, false, 0);
            AutoScroll = false;
        }
        private void DoExternalDropTo(AppButton ToAppButton, DragEventArgs e)
        {           
            string getAppFileName(string filename)
            {  
                string s = filename;
                // some apps are tacking on the shell:{} guid information when it's not needed. So check to see if it's just the filename, if not parse it out.
                if (s.IndexOf("\\") > -1)
                {
                    int ilen = filename.Length - 1;
                    for (int i = ilen; i > 0; i--)
                    {
                        if (filename[i] == '\\')
                        {
                            s = filename.Substring((i + 1), (filename.Length - i - 1));
                            break;
                        }
                    }
                }
                return s;
            }
            // shell object?
            if (e.Data is System.Runtime.InteropServices.ComTypes.IDataObject)
            {
                var ShellObj = ShellObjectCollection.FromDataObject((System.Runtime.InteropServices.ComTypes.IDataObject)e.Data);
                if (ShellObj.Count > 0)
                {
                    BeginUpdate();
                    foreach (ShellObject shellFile in ShellObj)
                    {
                        // File or Folder?
                        if ((File.Exists(shellFile.ParsingName)) || (!File.Exists(shellFile.ParsingName) && Directory.Exists(shellFile.ParsingName)))
                        {
                            if (File.Exists(shellFile.ParsingName))
                            {
                                AppButton b = AddAppButton(ButtonType.App, Controls);
                                b.AppName = (string.IsNullOrEmpty(Funcs.GetFileInfo(shellFile.ParsingName).ProductName) ? Path.GetFileNameWithoutExtension(shellFile.ParsingName) : Funcs.GetFileInfo(shellFile.ParsingName).ProductName);
                                b.FileName = shellFile.ParsingName;
                                AddApp(b, (ToAppButton == null ? 0 : Controls.GetChildIndex(ToAppButton)));
                            }                               
                            else
                            {
                                AppButton b = AddAppButton(ButtonType.FolderLink, Controls);
                                b.AppName = Path.GetFileName(shellFile.ParsingName);
                                b.FileName = shellFile.ParsingName;
                                AddFolderLink(b, (ToAppButton == null ? 0 : Controls.GetChildIndex(ToAppButton)));
                            }
                        }
                        else
                        {
                            AppButton b = AddAppButton(ButtonType.App, Controls);
                            b.AppName = shellFile.Name;
                            b.FileName = "shell:AppsFolder\\" + getAppFileName(shellFile.ParsingName);
                            AddApp(b, (ToAppButton == null ? 0 : Controls.GetChildIndex(ToAppButton)));
                        }
                    }
                    EndUpdate();
                }
                else
                {
                    // some apps from the new windows 11 start menu come through blank.
                    
                }
            }
            else
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string s = e.Data.GetData(DataFormats.Text).ToString();
                if (Funcs.IsUrl(s))
                {
                    AppButton b = AddAppButton(ButtonType.Url, Controls);
                    b.AppName = s;
                    b.Url = s;
                    AddUrl(b, (ToAppButton == null ? 0 : Controls.GetChildIndex(ToAppButton)));
                }
            }
        }
        public void EndUpdate()
        {
            BeginUpdateCounter--;
            if (BeginUpdateCounter == 0)
            {
                ResumeLayout();
                Funcs.SendMessage(this.Handle, WM_SETREDRAW, true, 0);
                if (!InLoad)
                    OnAppsChanged?.Invoke();
                this.Refresh();
                AutoScroll = true;
            }
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
                        AppButton b = AddAppButton((string.IsNullOrEmpty(GetAttrib(xn, "url")) ? ButtonType.App : ButtonType.Url), ac);
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
            BeginUpdate();
            MoveToCache();
            CurrentParentNode = appButton.Node;
            AddItems(CurrentParentNode);
            EndUpdate();
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
            return null;
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
            if (appButton.ButtonType == ButtonType.App)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
                appButton.AppName = GetAttrib(appButton.Node, "appname");
                appButton.AsAdmin = GetAttrib(appButton.Node, "asadmin");
                appButton.FileName = GetAttrib(appButton.Node, "filename");
                appButton.FileArgs = GetAttrib(appButton.Node, "fileargs");
                appButton.FileIconIndex = GetAttrib(appButton.Node, "fileiconindex");
                appButton.FileIconPath = GetAttrib(appButton.Node, "fileiconpath");
                appButton.FileWorkingFolder = GetAttrib(appButton.Node, "fileworkingfolder");
                if (appButton.FileIconImage == null)
                    appButton.WatchForIconUpdate = true;
            }
            else
            if (appButton.ButtonType == ButtonType.Folder)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
                appButton.AppName = GetAttrib(appButton.Node, "foldername");
            }
            else
            if (appButton.ButtonType == ButtonType.FolderLink)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
                appButton.AppName = GetAttrib(appButton.Node, "folderlinkname");
                appButton.FileName = GetAttrib(appButton.Node, "folderlinkpath");
            }
            else
            if (appButton.ButtonType == ButtonType.Separator)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
            }
            else
            if (appButton.ButtonType == ButtonType.Url)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
                appButton.AppName = GetAttrib(appButton.Node, "appname");
                appButton.Url = GetAttrib(appButton.Node, "url");
                appButton.FavIcon = GetAttrib(appButton.Node, "favicon");
            }
        }
        private void SetColors()
        {
            BackColor = AppsConfig.AppsBackColor;
        }

        #endregion
    }
}
