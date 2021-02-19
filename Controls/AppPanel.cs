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

        XmlDocument AppsXml;

        private string AppsXmlFilePath = Funcs.AppPath() + "\\Apps.xml";
        private string New_AppsXml_file = "<XML VERSION=\"1.0\" ENCODING=\"utf-8\">\r\n<DATA>\r\n</DATA>\r\n</XML>";
        private string New_AppXmlNode = "<APP>\r\n" +
                                          "<NAME>{1}</NAME>\r\n" +
                                          "<FILENAME>{2}</FILENAME>\r\n" +
                                          "<ARGS>{3}</ARGS>\r\n" +
                                          "<ICON>{4}</ICON>\r\n" +
                                        "</APP>";

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

            t = new ToolStripMenuItem("&Add Sub Folder");
            t.Click += new EventHandler(MenuAddFolder_Click);
            MenuRC.Items.Add(t);

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
            AppButton b = new AppButton(AppsConfig)
            {
                TabStop = false,
                Dock = DockStyle.Top,
                FlatStyle = FlatStyle.Flat
            };

            b.OnAppButtonClicked += new AppButton.AppButtonClickedHandler(ButtonClicked);
            b.ContextMenuStrip = MenuRC;
            b.ImageAlign = ContentAlignment.MiddleLeft;
            b.Parent = this;
            return b;
        }

        public void AddItem(string text, string fileName, bool saveToDisk = false)
        {
            SuspendLayout();

            AppButton b = AddAppButton();

            b.AutoSize = false;
            b.AutoEllipsis = false;
            //b.Text =

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

            //foreach (string file in files) 
            //{
            //    XmlDocument doc = new XmlDocument();
            //    doc.Load(file);
            //    XmlNode data = doc.DocumentElement.SelectSingleNode("/DATA");
            //    string type = data.Attributes["TYPE"]?.InnerText;

            //    if (type == "IMAGE")
            //    {
            //        MemoryStream ms = new MemoryStream(Convert.FromBase64String(data.InnerText));
            //        try
            //        {
            //            Image img = Image.FromStream(ms);
            //            AddItem(img, file, false);
            //        }
            //        finally
            //        {
            //            ms.Close();
            //        }
            //    }
            //    else
            //    {
            //        byte[] base64EncodedBytes = Convert.FromBase64String(data.InnerText);
            //        string decodedString = Encoding.UTF8.GetString(base64EncodedBytes);
            //        AddItem(decodedString, file, false);
            //    }
            //    doc = null;
            //}
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

                }
                else
                {
                    AppButton b = ((AppButton)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl);
                }
            }

            GC.Collect();
            InMenu = false;
        }

        private void MenuAddFolder_Click(object sender, EventArgs e)
        {
            InMenu = true;

            //AppButton b = ((AppButton)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl);
            //if (sender is AppPanel)

            AddFolder f = new AddFolder();

            if (f.ShowDialog(this) == DialogResult.OK)
            {

            }
            

            GC.Collect();

            if (OnAppAdded != null)
                OnAppAdded();

            InMenu = false;
        }

        private void MenuDelete_Click(object sender, EventArgs e)
        {
            InMenu = true;
            //AppButton b = ((AppButton)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl);

            //Controls.Remove(b);
            GC.Collect();

            if (OnAppDeleted != null)
                OnAppDeleted();
            InMenu = false;
        }

        private void Menu_Opening(object sender, CancelEventArgs e)
        {
            DeleteMenuItem.Enabled = (sender is AppPanel);
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
