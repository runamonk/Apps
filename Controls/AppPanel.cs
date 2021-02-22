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
        
        XmlDocument AppsXml;
        XmlNode AppsNode;
        
        private string AppsXmlFilePath = Funcs.AppPath() + "\\Apps.xml";
        private string New_AppsXml_file = "<XML VERSION=\"1.0\" ENCODING=\"utf-8\">\r\n<APPS>\r\n</APPS>\r\n</XML>";
        private string AppIdLookup = "//APPS//APP[@id='{0}']";

        public AppPanel(Config myConfig, bool isHeader = false)
        {
            IsHeader = isHeader;
            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += new EventHandler(ConfigChanged);
            ToolStripMenuItem t;
            MenuRC = new AppMenu(myConfig);

            MenuRC.Opening += new CancelEventHandler(Menu_Opening);

            t = new ToolStripMenuItem("&Add Application");
            t.Click += new EventHandler(MenuAddApp_Click);
            MenuRC.Items.Add(t);

            //t = new ToolStripMenuItem("&Add Sub Folder");
            //t.Click += new EventHandler(MenuAddFolder_Click);
            //MenuRC.Items.Add(t);

            EditMenuItem = new ToolStripMenuItem("&Edit Properties");
            EditMenuItem.Click += new EventHandler(MenuEdit_Click);
            MenuRC.Items.Add(EditMenuItem);

            DeleteMenuItem = new ToolStripMenuItem("&Delete");
            DeleteMenuItem.Click += new EventHandler(MenuDelete_Click);
            MenuRC.Items.Add(DeleteMenuItem);

            this.ContextMenuStrip = MenuRC;

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

        private AppButton AddAppButton()
        {
            AppButton b = new AppButton(AppsConfig);
            b.OnAppButtonClicked += new AppButton.AppButtonClickedHandler(ButtonClicked);
            b.ContextMenuStrip = MenuRC;
            b.Height = 22;
            b.Padding = new Padding(0, 0, 0, 0);
            b.Margin = new Padding(0, 0, 0, 0);
            b.TabStop = false;
            Controls.Add(b);
            b.Dock = DockStyle.Top;
            return b;
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
                        
            string siblingAppId = "";

            siblingAppId = ((AppButton)Controls[AddAtIndex]).AppId;
            Controls.SetChildIndex(b, AddAtIndex); // move button where we want it.

            XmlNode node = AppsXml.SelectSingleNode(string.Format(AppIdLookup, b.AppId));
            XmlNode nodeSib = null;

            if ((!string.IsNullOrEmpty(siblingAppId)) && (siblingAppId != b.AppId))
                nodeSib = AppsXml.SelectSingleNode(string.Format(AppIdLookup, siblingAppId));
            
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
                AppsXml.Save(AppsXmlFilePath);
            }

            if (OnAppAdded != null)
                OnAppAdded();
             ResumeLayout();
        }

        private void ButtonClicked(AppButton App)
        {

            SuspendLayout();

            if (OnAppClicked != null)
                OnAppClicked();

            ResumeLayout();
        }

        private void ConfigChanged(object sender, EventArgs e)
        {
            SetColors();
            LoadItems();         
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
                AppsXml.Save(AppsXmlFilePath);
            }
            else
            {
                AppsXml = new XmlDocument();
                AppsXml.Load(AppsXmlFilePath);
            }

            AppsNode = AppsXml.SelectSingleNode("//APPS");
            
            foreach (XmlNode xn in AppsNode)
            {
                AddItem(xn.Attributes["id"].Value, xn.Attributes["appname"].Value, xn.Attributes["filename"].Value, xn.Attributes["fileiconpath"].Value, xn.Attributes["fileargs"].Value, 0);
            }

            InLoad = false;
            if (OnAppsLoaded != null)
                OnAppsLoaded();
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
                    AppButton b = ((AppButton)(c.Parent.Parent.Parent)); // Label > Panel > Panel > AppButton
                    int i = Controls.GetChildIndex(b);
                    AddItem(null, f.AppName, f.AppFileName, f.AppIconPath, f.AppFileArgs, i);               
                }
            }

            GC.Collect();
            InMenu = false;
        }

        private void MenuDelete_Click(object sender, EventArgs e)
        {
            InMenu = true;
            var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
            AppButton b = ((AppButton)(c.Parent.Parent.Parent)); // Label > Panel > Panel > AppButton

            XmlNode node = AppsXml.SelectSingleNode(string.Format(AppIdLookup, b.AppId));
            AppsNode.RemoveChild(node);
            Controls.Remove(b);
            AppsXml.Save(AppsXmlFilePath);
            GC.Collect();

            if (OnAppDeleted != null)
                OnAppDeleted();
            InMenu = false;
        }

        private void MenuEdit_Click(object sender, EventArgs e)
        {
            InMenu = true;
            SuspendLayout();
            var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
            AppButton b = ((AppButton)(c.Parent.Parent.Parent)); // Label > Panel > Panel > AppButton
            
            AppProperties f = new AppProperties();
            f.SetFileProperties(b.AppName, b.FileName, b.FileIconPath, b.FileArgs);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                XmlNode node = AppsXml.SelectSingleNode(string.Format(AppIdLookup, b.AppId));
                int i = Controls.GetChildIndex(b);
                AppsNode.RemoveChild(node);
                Controls.Remove(b);
                AddItem(null, f.AppName, f.AppFileName, f.AppIconPath, f.AppFileArgs, i);
                AppsXml.Save(AppsXmlFilePath);
            }
            GC.Collect();
                        
            if (OnAppDeleted != null)
                OnAppDeleted();
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
        }

        private void SetColors()
        {
            if (IsHeader)
                BackColor = AppsConfig.AppsHeaderColor;
            else
                BackColor = AppsConfig.AppsBackColor;
        }

    }
}
