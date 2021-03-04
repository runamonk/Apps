﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Utility;
using Apps.Forms;

namespace Apps.Controls
{
    public partial class AppPanel : Panel
    {
        private Config AppsConfig { get; set; }
        public bool InMenu { get; set; }
        public bool InLoad { get; set; }
        public bool InAFolder
        {
            get {
                return (CurrentParentNode != null);
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

        private AppMenu MenuRC;
        private ToolStripMenuItem DeleteMenuItem;
        private ToolStripMenuItem EditMenuItem;
        private ToolStripMenuItem UpMenuItem;
        private ToolStripMenuItem DownMenuItem;
        //private ToolStripMenuItem AddSepMenuItem;

        XmlDocument AppsXml;
        XmlNode AppsNode;
        private XmlNode CurrentParentNode = null;
        private string AppsXmlFilePath = Funcs.AppPath() + "\\Apps.xml";
        private string New_AppsXml_file = "<XML VERSION=\"1.0\" ENCODING=\"utf-8\">\r\n<APPS>\r\n</APPS>\r\n</XML>";
        private string AppIdLookup = "//APPS//APP[@id='{0}']";

        public AppPanel(Config myConfig)
        {
            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += new EventHandler(ConfigChanged);
            MenuRC = new AppMenu(myConfig);
            MenuRC.ShowCheckMargin = false;
            MenuRC.ShowImageMargin = false;
            MenuRC.Opening += new CancelEventHandler(Menu_Opening);

            Funcs.AddMenuItem(MenuRC, "Add Application", MenuAddApp_Click);
            Funcs.AddMenuItem(MenuRC, "Add Folder Link", MenuAddFolderLink_Click);
            Funcs.AddMenuItem(MenuRC, "Add Folder", MenuAddFolder_Click);
            EditMenuItem = Funcs.AddMenuItem(MenuRC, "Edit", MenuEdit_Click);
            DeleteMenuItem = Funcs.AddMenuItem(MenuRC, "Delete", MenuDelete_Click);
            MenuRC.Items.Add(new ToolStripSeparator());
            UpMenuItem = Funcs.AddMenuItem(MenuRC, "Move Up", MenuUp_Click);
            DownMenuItem = Funcs.AddMenuItem(MenuRC, "Move Down", MenuDown_Click);

            this.ContextMenuStrip = MenuRC;
            this.AllowDrop = true;
            this.DragOver += new DragEventHandler(OnDragOver);
            this.DragDrop += new DragEventHandler(DropToPanel);
            SetColors();
            LoadItems();
        }

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
        
        private AppButton AddAppButton(bool isMenuButton = false, bool isPinButton = false, bool isFolderButton = false)
        {
            AppButton b = new AppButton(AppsConfig,isMenuButton,isPinButton,isFolderButton);
            b.OnAppButtonClicked += new AppButton.AppButtonClickedHandler(ButtonClicked);
            b.OnAppButtonDropped += new AppButton.AppButtonDropEventhandler(DropToButton);
            b.ContextMenuStrip = MenuRC;
            b.Height = 22;
            b.Padding = new Padding(0, 0, 0, 0);
            b.Margin = new Padding(0, 0, 0, 0);
            b.TabStop = false;
            Controls.Add(b);
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
            foreach (string filePath in Files)
            {
                if (File.Exists(filePath))
                {
                    string AppName = (string.IsNullOrEmpty(Funcs.GetFileInfo(filePath).ProductName) ? Path.GetFileNameWithoutExtension(filePath) : Funcs.GetFileInfo(filePath).ProductName);
                    AddItem(null, AppName, filePath, null, null, null, AddAtIndex);
                }
            }
        }

        public void AddItem(string AppId, string FolderName, int AddAtIndex)
        {
            SuspendLayout();
            AppButton b = AddAppButton(false, false, true);
            if (string.IsNullOrEmpty(AppId))
                b.AppId = Guid.NewGuid().ToString();
            else
                b.AppId = AppId;
            
            b.AutoSize = false;
            b.AppName = FolderName;
            XmlNode node = GetNode(b.AppId);
            XmlNode nodeSib = null;
            nodeSib = GetNode(((AppButton)Controls[AddAtIndex]).AppId);
            Controls.SetChildIndex(b, AddAtIndex); // move button where we want it.
            if (node == null)
            {
                node = AppsXml.CreateNode(XmlNodeType.Element, "APP", null);
                AddAttrib(node, "id", b.AppId);
                AddAttrib(node, "foldername", FolderName);

                XmlNode ParentNode = (CurrentParentNode != null ? CurrentParentNode : AppsNode);
                if (nodeSib == null)
                    ParentNode.AppendChild(node);
                else
                    ParentNode.InsertAfter(node, nodeSib);

                SaveXML();
            }

            OnAppAdded?.Invoke();
            ResumeLayout();
        }

        public void AddItem(string AppId, string AppName, string fileName, string fileIconPath, string fileArgs, string fileWorkingFolder, int AddAtIndex)
        {
            SuspendLayout();
            AppButton b = AddAppButton();
            if (string.IsNullOrEmpty(AppId))
                b.AppId = Guid.NewGuid().ToString();
            else
                b.AppId = AppId;
            b.AutoSize = false;
            
            XmlNode node = GetNode(b.AppId);
            XmlNode nodeSib = null;
            nodeSib = GetNode(((AppButton)Controls[AddAtIndex]).AppId);
            Controls.SetChildIndex(b, AddAtIndex); // move button where we want it.
                       
            if (node == null)
            {
                if ((AppsConfig.ParseShortcuts) && Funcs.IsShortcut(fileName))
                {
                    AppName = Path.GetFileNameWithoutExtension(fileName);
                    Funcs.ParseShortcut(fileName, ref fileName, ref fileIconPath, ref fileArgs, ref fileWorkingFolder);
                }

                node = AppsXml.CreateNode(XmlNodeType.Element, "APP", null);
                AddAttrib(node, "id", b.AppId);
                AddAttrib(node, "appname", AppName);
                AddAttrib(node, "filename", fileName);
                AddAttrib(node, "fileiconpath", fileIconPath);
                AddAttrib(node, "fileargs", fileArgs);
                AddAttrib(node, "fileworkingfolder", fileWorkingFolder);
                XmlNode ParentNode = (CurrentParentNode != null ? CurrentParentNode : AppsNode);
                if (nodeSib == null)
                    ParentNode.AppendChild(node);
                else
                    ParentNode.InsertAfter(node, nodeSib);
                SaveXML();
            }

            b.AppName = AppName;
            b.FileName = fileName;
            b.FileArgs = fileArgs;
            b.FileIconPath = fileIconPath;
            b.FileWorkingFolder = fileWorkingFolder;
            if (b.FileIconImage == null)
                b.WatchForIconUpdate = true;

            OnAppAdded?.Invoke();
            ResumeLayout();
        }

        private void ButtonClicked(AppButton App)
        {
            SuspendLayout();

            if (!App.IsFolderButton)
                OnAppClicked?.Invoke();
            else
            {
                LoadFolder(App.AppId);
            }

            ResumeLayout();
        }

        private void ConfigChanged(object sender, EventArgs e)
        {
            SetColors();
        }

        private void DropToButton(AppButton App, DragEventArgs e)
        {
            SuspendLayout();
            int i = Controls.GetChildIndex(App);
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                AddFiles((string[])e.Data.GetData(DataFormats.FileDrop),i);
            }
            ResumeLayout();
        }

        private void DropToPanel(object sender, DragEventArgs e)
        {
            SuspendLayout();

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                AddFiles((string[])e.Data.GetData(DataFormats.FileDrop),0);
            }
            ResumeLayout();
        }

        private AppButton GetAppButton(object sender)
        {
            if (sender is AppMenu)
            {
                var c = ((AppMenu)sender).SourceControl;
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
            if ((CurrentParentNode.ParentNode != null) && (CurrentParentNode.ParentNode.Attributes["id"] != null))
                LoadFolder(CurrentParentNode.ParentNode.Attributes["id"].Value);
            else
                LoadItems();
        }

        private void AddFromNodes(XmlNode Nodes)
        {
            foreach (XmlNode xn in Nodes)
            {
                if (xn.Attributes["foldername"] != null)
                    AddItem(GetAttrib(xn, "id"), GetAttrib(xn, "foldername"), 0);
                else
                    AddItem(GetAttrib(xn, "id"), GetAttrib(xn, "appname"), GetAttrib(xn, "filename"), GetAttrib(xn, "fileiconpath"), GetAttrib(xn, "fileargs"), GetAttrib(xn, "fileworkingfolder"), 0);
            }
        }

        private void LoadFolder(string AppId)
        {
            SuspendLayout();
            InLoad = true;            
            Controls.Clear();

            CurrentParentNode = GetNode(AppId);
            AddFromNodes(CurrentParentNode);

            InLoad = false;
            OnAppsLoaded?.Invoke();
            ResumeLayout();
        }

        public void LoadItems()
        {
            SuspendLayout();
            Controls.Clear();
            CurrentParentNode = null;
            InLoad = true;

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
            AddFromNodes(AppsNode);

            InLoad = false;
            OnAppsLoaded?.Invoke();
            ResumeLayout();
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
                    AddItem(null, f.AppName, f.AppFileName, f.AppIconPath, f.AppFileArgs, f.AppFileWorkingFolder, 0);
                }
                else
                {
                    AppButton b = GetAppButton(sender);
                    int i = Controls.GetChildIndex(b);
                    AddItem(null, f.AppName, f.AppFileName, f.AppIconPath, f.AppFileArgs, f.AppFileWorkingFolder, i);
                }
            }

            GC.Collect();
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

            GC.Collect();
            InMenu = false;
        }

        private void MenuAddFolderLink_Click(object sender, EventArgs e)
        {
            InMenu = true;
            FolderBrowserDialog f = new FolderBrowserDialog();

            //AddFolder f = new AddFolder(AppsConfig);

            //if (f.ShowDialog(this) == DialogResult.OK)
            //{
            //    var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
            //    if (c is AppPanel)
            //    {
            //        AddItem(null, f.FolderName, 0);
            //    }
            //    else
            //    {
            //        AppButton b = GetAppButton(sender);
            //        int i = Controls.GetChildIndex(b);
            //        AddItem(null, f.FolderName, i);
            //    }
            //}
            GC.Collect();
            InMenu = false;
        }

        private void MenuDelete_Click(object sender, EventArgs e)
        {
            InMenu = true;
            AppButton b = GetAppButton(sender);
            XmlNode node = GetNode(b.AppId);
            bool CanDelete = true;
            CanDelete = (Misc.ConfirmDialog(AppsConfig, "Delete " + b.AppName + "?", (node.HasChildNodes || b.IsFolderButton ? "Delete folder, sub-folders and applications?" : "Delete application?")) == DialogResult.OK);

            if (CanDelete)
            {
                XmlNode parentNode = node.ParentNode;
                parentNode.RemoveChild(node);
                Controls.Remove(b);
                SaveXML();
                GC.Collect();
                OnAppDeleted?.Invoke();
            }
            InMenu = false;
        }

        private void MenuDown_Click(object sender, EventArgs e)
        {
            InMenu = true;
            SuspendLayout();
            AppButton b = GetAppButton(sender);
            XmlNode ParentNode = (CurrentParentNode != null ? CurrentParentNode : AppsNode);
            ParentNode.InsertAfter(GetNode(b.AppId), GetNextNode(GetNode(b.AppId)));
            Controls.SetChildIndex(b, GetAppButtonIndex(b) - 1); // indexes for panels are backwards last control is 0, first control is count. Dumb.
            SaveXML();
            GC.Collect();
            ResumeLayout();
            InMenu = false;
        }

        private void MenuUp_Click(object sender, EventArgs e)
        {
            InMenu = true;
            SuspendLayout();
            AppButton b = GetAppButton(sender);
            XmlNode ParentNode = (CurrentParentNode != null ? CurrentParentNode : AppsNode);
            ParentNode.InsertBefore(GetNode(b.AppId), GetPrevNode(GetNode(b.AppId)));
            Controls.SetChildIndex(b, GetAppButtonIndex(b)+1); // indexes for panels are backwards last control is 0, first control is count. Dumb.
            SaveXML();
            GC.Collect();
            ResumeLayout();
            InMenu = false;
        }

        private void MenuEdit_Click(object sender, EventArgs e)
        {
            InMenu = true;
            SuspendLayout();
            AppButton b = GetAppButton(sender);

            if (b.IsFolderButton)
            {
                Folder f = new Folder(AppsConfig);
                f.FolderName = b.AppName;
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    XmlNode node = GetNode(b.AppId);
                    int i = Controls.GetChildIndex(b);
                    node.Attributes["foldername"].Value = f.FolderName;
                    ((AppButton)Controls[i]).AppName = f.FolderName;
                    SaveXML();
                }
            }
            else
            {
                Forms.Properties f = new Forms.Properties(AppsConfig);
                f.SetFileProperties(b.AppName, b.FileName, b.FileIconPath, b.FileArgs, b.FileWorkingFolder);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    XmlNode node = GetNode(b.AppId);
                    int i = Controls.GetChildIndex(b);
                    XmlNode ParentNode = (CurrentParentNode != null ? CurrentParentNode : AppsNode);
                    ParentNode.RemoveChild(node);
                    Controls.Remove(b);
                    AddItem(null, f.AppName, f.AppFileName, f.AppIconPath, f.AppFileArgs, f.AppFileWorkingFolder, i);
                    SaveXML();
                }
            }

            GC.Collect();
            ResumeLayout();
            InMenu = false;
        }

        private void Menu_Opening(object sender, CancelEventArgs e)
        {
            bool b = false;
            var c = ((AppMenu)sender).SourceControl;

            if (c is AppPanel)
                b = false;
            else
            if (c is Label)
                b = true;
            DeleteMenuItem.Enabled = b;
            EditMenuItem.Enabled = b;
            if (b)
            {
                UpMenuItem.Enabled = (GetPrevNode(GetNode(GetAppButton(sender).AppId)) != null);
                DownMenuItem.Enabled = (GetNextNode(GetNode(GetAppButton(sender).AppId)) != null);
            }
            else
            {
                UpMenuItem.Enabled = false;
                DownMenuItem.Enabled = false;
            }
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            e.Effect = (DragDropEffects.Copy | DragDropEffects.Link);
        }

        private void SaveXML()
        {
            AppsXml.Save(AppsXmlFilePath);
        }

        private void SetColors()
        {
            BackColor = AppsConfig.AppsBackColor;
        }
    }
}
