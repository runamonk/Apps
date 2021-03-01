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

namespace Apps.Controls
{
    public partial class AppPanel : Panel
    {
        private Config AppsConfig { get; set; }
        private bool IsHeader = false;
        public bool InMenu { get; set; }
        public bool InLoad { get; set; }

        private AppMenu MenuRC;
        private ToolStripMenuItem DeleteMenuItem;
        private ToolStripMenuItem EditMenuItem;
        private ToolStripMenuItem UpMenuItem;
        private ToolStripMenuItem DownMenuItem;
        private ToolStripMenuItem AddSepMenuItem;

        XmlDocument AppsXml;
        XmlNode AppsNode;

        private string AppsXmlFilePath = Funcs.AppPath() + "\\Apps.xml";
        private string New_AppsXml_file = "<XML VERSION=\"1.0\" ENCODING=\"utf-8\">\r\n<APPS>\r\n</APPS>\r\n</XML>";
        private string AppIdLookup = "//APPS//APP[@id='{0}']";

        public AppPanel(Config myConfig)
        {
            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += new EventHandler(ConfigChanged);
            ToolStripMenuItem t;
            MenuRC = new AppMenu(myConfig);
            MenuRC.Opening += new CancelEventHandler(Menu_Opening);
            t = new ToolStripMenuItem("&Add Application");
            t.Click += new EventHandler(MenuAddApp_Click);
            MenuRC.Items.Add(t);
            EditMenuItem = new ToolStripMenuItem("&Edit Application");
            EditMenuItem.Click += new EventHandler(MenuEdit_Click);
            MenuRC.Items.Add(EditMenuItem);
            DeleteMenuItem = new ToolStripMenuItem("&Delete Application");
            DeleteMenuItem.Click += new EventHandler(MenuDelete_Click);
            MenuRC.Items.Add(DeleteMenuItem);
            MenuRC.Items.Add(new ToolStripSeparator());
            UpMenuItem = new ToolStripMenuItem("Move &Up");
            UpMenuItem.Click += new EventHandler(MenuUp_Click);
            MenuRC.Items.Add(UpMenuItem);
            DownMenuItem = new ToolStripMenuItem("&Move Down");
            DownMenuItem.Click += new EventHandler(MenuDown_Click);
            MenuRC.Items.Add(DownMenuItem);
            MenuRC.Items.Add(new ToolStripSeparator());
            t = new ToolStripMenuItem("Add &Folder");
            t.Click += new EventHandler(MenuAddFolder_Click);
            MenuRC.Items.Add(t);


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

        public void AddItem(string AppId, string FolderName, AppButton ParentButton)
        {
            SuspendLayout();
            XmlNode node = null;
            XmlNode nodeSib = null;
            string appId = "";
            int AddAtIndex = 0;
            
            if (string.IsNullOrEmpty(AppId))
                appId = Guid.NewGuid().ToString();
            else
                appId = AppId;

            if ((ParentButton == null) || (!ParentButton.IsFolderButton))
            {
                AppButton b = AddAppButton(false, false, true);
                b.IsFolderButton = true;
                b.AutoSize = false;
                b.AppName = FolderName;
                b.AppId = appId;
                node = GetNode(b.AppId);
                nodeSib = GetNode(((AppButton)Controls[AddAtIndex]).AppId);
                if (ParentButton != null)
                {
                    AddAtIndex = Controls.GetChildIndex(ParentButton);
                }
            }

            if (node == null)
            {
                node = AppsXml.CreateNode(XmlNodeType.Element, "APP", null);
                XmlAttribute XmlAtt;
                XmlAtt = AppsXml.CreateAttribute("id");
                XmlAtt.Value = appId;
                node.Attributes.Append(XmlAtt);
                XmlAtt = AppsXml.CreateAttribute("foldername");
                XmlAtt.Value = FolderName;
                node.Attributes.Append(XmlAtt);

                if ((ParentButton != null) && (ParentButton.IsFolderButton))
                {
                    XmlNode nodeParent = GetNode(ParentButton.AppId);
                    nodeParent.AppendChild(node);
                }
                else
                {
                    if (nodeSib == null)
                        AppsNode.AppendChild(node);
                    else
                        AppsNode.InsertAfter(node, nodeSib);
                }

                SaveXML();
            }

            OnAppAdded?.Invoke();
            ResumeLayout();
        }

        public void AddItem(string AppId, string AppName, string fileName, string fileIconPath, string fileArgs, int AddAtIndex)
        {
            SuspendLayout();

            AppButton b = AddAppButton();

            if (string.IsNullOrEmpty(AppId))
                b.AppId = Guid.NewGuid().ToString();
            else
                b.AppId = AppId;

            b.AutoSize = false;
            b.AppName = AppName;
            b.FileName = fileName;
            b.FileArgs = fileArgs;
            b.FileIconPath = fileIconPath;

            if (b.FileIconImage == null)
                b.WatchForIconUpdate = true;
            
            XmlNode node = GetNode(b.AppId);
            XmlNode nodeSib = GetNode(((AppButton)Controls[AddAtIndex]).AppId);

            Controls.SetChildIndex(b, AddAtIndex); // move button where we want it.
                       
            if (node == null)
            {
                node = AppsXml.CreateNode(XmlNodeType.Element, "APP", null);
                XmlAttribute XmlAtt;
                XmlAtt = AppsXml.CreateAttribute("id");
                XmlAtt.Value = b.AppId;
                node.Attributes.Append(XmlAtt);
                XmlAtt = AppsXml.CreateAttribute("appname");
                XmlAtt.Value = AppName;
                node.Attributes.Append(XmlAtt);
                XmlAtt = AppsXml.CreateAttribute("filename");
                XmlAtt.Value = fileName;
                node.Attributes.Append(XmlAtt);
                XmlAtt = AppsXml.CreateAttribute("fileiconpath");
                XmlAtt.Value = fileIconPath;
                node.Attributes.Append(XmlAtt);
                XmlAtt = AppsXml.CreateAttribute("fileargs");
                XmlAtt.Value = fileArgs;
                node.Attributes.Append(XmlAtt);

                if (nodeSib == null)
                    AppsNode.AppendChild(node);
                else
                    AppsNode.InsertAfter(node, nodeSib);

                SaveXML();
            }

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
                //MessageBox.Show(Funcs.GetNodePath(xn));
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
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filePath in files)
                {
                    AddItem(null, (string.IsNullOrEmpty(Funcs.GetFileInfo(filePath).ProductName) ? Path.GetFileNameWithoutExtension(filePath) : Funcs.GetFileInfo(filePath).ProductName), filePath, null, null, i);
                }
            }
            ResumeLayout();
        }

        private void DropToPanel(object sender, DragEventArgs e)
        {
            SuspendLayout();

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filePath in files)
                {
                    AddItem(null, (string.IsNullOrEmpty(Funcs.GetFileInfo(filePath).ProductName) ? Path.GetFileNameWithoutExtension(filePath) : Funcs.GetFileInfo(filePath).ProductName), filePath, null, null, 0);
                }
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

        public void LoadFolder(string AppId)
        {
            SuspendLayout();
            InLoad = true;
            
            Controls.Clear();
            // Add a .. button (Up level?)
            XmlNode SubNodes = GetNode(AppId);
            foreach (XmlNode xn in SubNodes)
            {
                if (xn.Attributes["foldername"] != null)
                    AddItem(xn.Attributes["id"].Value, xn.Attributes["foldername"].Value, null);
                else
                    AddItem(xn.Attributes["id"].Value, xn.Attributes["appname"].Value, xn.Attributes["filename"].Value, xn.Attributes["fileiconpath"].Value, xn.Attributes["fileargs"].Value, 0);
            }

            InLoad = false;
            OnAppsLoaded?.Invoke();
            ResumeLayout();
        }

        public void LoadItems()
        {
            SuspendLayout();
            Controls.Clear();
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

            foreach (XmlNode xn in AppsNode)
            {
                if (xn.Attributes["foldername"] != null)
                    AddItem(xn.Attributes["id"].Value, xn.Attributes["foldername"].Value, null);
                else
                    AddItem(xn.Attributes["id"].Value, xn.Attributes["appname"].Value, xn.Attributes["filename"].Value, xn.Attributes["fileiconpath"].Value, xn.Attributes["fileargs"].Value, 0);
            }

            InLoad = false;
            OnAppsLoaded?.Invoke();
            ResumeLayout();
        }

        private void MenuAddApp_Click(object sender, EventArgs e)
        {
            InMenu = true;
            AppProperties f = new AppProperties();
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                if (c is AppPanel)
                {
                    AddItem(null, f.AppName, f.AppFileName, f.AppIconPath, f.AppFileArgs, 0);
                }
                else
                {
                    AppButton b = GetAppButton(sender);
                    int i = Controls.GetChildIndex(b);
                    AddItem(null, f.AppName, f.AppFileName, f.AppIconPath, f.AppFileArgs, i);
                }
            }

            GC.Collect();
            InMenu = false;
        }

        private void MenuAddFolder_Click(object sender, EventArgs e)
        {
            InMenu = true;
            AddFolder f = new AddFolder();
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                if (c is AppPanel)
                {
                    AddItem(null, f.FolderName, null);
                }
                else
                {
                    AppButton b = GetAppButton(sender);
                    AddItem(null, f.FolderName, b);
                }
            }

            GC.Collect();
            InMenu = false;
        }

        private void MenuDelete_Click(object sender, EventArgs e)
        {
            InMenu = true;
            AppButton b = GetAppButton(sender);
            XmlNode node = GetNode(b.AppId);
            AppsNode.RemoveChild(node);
            Controls.Remove(b);
            SaveXML();
            GC.Collect();

            OnAppDeleted?.Invoke();
            InMenu = false;
        }

        private void MenuDown_Click(object sender, EventArgs e)
        {
            InMenu = true;
            SuspendLayout();
            AppButton b = GetAppButton(sender);
            AppsNode.InsertAfter(GetNode(b.AppId), GetNextNode(GetNode(b.AppId)));
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
            AppsNode.InsertBefore(GetNode(b.AppId), GetPrevNode(GetNode(b.AppId)));
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
            AppProperties f = new AppProperties();
            f.SetFileProperties(b.AppName, b.FileName, b.FileIconPath, b.FileArgs);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                XmlNode node = GetNode(b.AppId);
                int i = Controls.GetChildIndex(b);
                AppsNode.RemoveChild(node);
                Controls.Remove(b);
                AddItem(null, f.AppName, f.AppFileName, f.AppIconPath, f.AppFileArgs, i);
                SaveXML();
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
