using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Utility;

namespace Apps.Controls
{
    public partial class AppPanel : Panel
    {
        private Config AppsConfig { get; set; }
        private bool IsHeader = false;
        public bool InMenu { get; set; }
        public bool InLoad { get; set; }
 
        private AppMenu MenuRC;

        public AppPanel(Config myConfig, bool isHeader = false)
        {
            IsHeader = isHeader;
            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += new EventHandler(ConfigChanged);
            ToolStripMenuItem t;
            MenuRC = new AppMenu(myConfig);
            t = new ToolStripMenuItem("&Delete");
            t.Click += new EventHandler(MenuDelete_Click);
            MenuRC.Items.Add(t);
            SetColors();
            LoadItems();

        }
        
        #region EventHandlers
        public delegate void ClipAddedHandler(AppButton Clip, bool AppsavedToDisk);
        public event ClipAddedHandler OnClipAdded;

        public delegate void ClipClickedHandler(AppButton Clip);
        public event ClipClickedHandler OnClipClicked;

        public delegate void ClipDeletedHandler();
        public event ClipDeletedHandler OnClipDeleted;
        
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

            b.OnAppButtonClicked += new  AppButton.AppButtonClickedHandler(ButtonClicked);


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
            b.FullText = text;


            if (OnClipAdded != null)
                OnClipAdded(b, saveToDisk);

             ResumeLayout();
        }

        private void ButtonClicked(AppButton Clip)
        {

            SuspendLayout();

            if (OnClipClicked != null)
                OnClipClicked(Clip);

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
            //string[] files = Funcs.GetFiles(Funcs.AppPath() + "\\Cache", "*.xml");
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
            //InLoad = false;
            if (OnAppsLoaded != null)
                OnAppsLoaded();
            ResumeLayout();
        }

        private void MenuDelete_Click(object sender, EventArgs e)
        {
            InMenu = true;
            //AppButton b = ((AppButton)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl);
            //if (File.Exists(b.FileName))
            //    File.Delete(b.FileName);

            //if (Funcs.IsSame(b.FullImage, LastImage))
            //    LastImage = null;
            //else
            //if (b.FullText == LastText)
            //    LastText = null;

            //Controls.Remove(b);
            GC.Collect();

            if (OnClipDeleted != null)
                OnClipDeleted();
            InMenu = false;
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
