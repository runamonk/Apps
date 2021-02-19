using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;


namespace Apps.Forms
{
    public partial class AppProperties : Form
    {
        public AppProperties()
        {
            InitializeComponent();
        }

        public string AppFileName
        {
            get {return EditAppFilePath.Text.Trim(); }
        }

        public string AppName
        {
            get { return EditAppName.Text.Trim(); }
        }

        private string FAppIconPath;
        private bool IsCancelled = false;
        public string AppIconPath
        {
            get { return FAppIconPath; }
        }

        public Image AppIconImage
        {
            get { return GetIcon(FAppIconPath); }
        }

        private string BrowseForFile()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Multiselect = false;
            fd.Filter = "All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;

            DialogResult dr = fd.ShowDialog();

            if (dr == DialogResult.OK)
                return fd.FileName;
            else
                return "";           
        }

        private Image GetIcon(string fileName)
        {
            if (File.Exists(fileName))
            {
                string[] ImageTypes = { ".png", ".tif", ".jpg", ".gif", ".bmp", ".ico" };

                if (ImageTypes.Contains(Path.GetExtension(fileName)))
                {
                    return (Image)(Image)(new Bitmap(new Bitmap(fileName, false), EditAppIcon.Size));
                }
                    
                else
                    return (Image)(new Bitmap(Icon.ExtractAssociatedIcon(fileName).ToBitmap(), EditAppIcon.Size));
            }
            else
                return null;
        }

        public void SetFileProperties(string appName, string filePath, string fileIcon)
        {
            EditAppName.Text = appName;
            EditAppFilePath.Text = filePath;
            FAppIcon = fileIcon;
            EditAppIcon.Image = GetIcon(FAppIcon);
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            string fileName = BrowseForFile();   
            if (fileName != "")
            {
                FileVersionInfo f = FileVersionInfo.GetVersionInfo(fileName);
                SetFileProperties(f.ProductName, f.FileName, f.FileName);
            }
        }

        private void AppIcon_Click(object sender, EventArgs e)
        {
            string fileName = BrowseForFile();
            if (fileName != "")
            {
                FAppIconPath = fileName;
                EditAppIcon.Image = GetIcon(fileName);
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            IsCancelled = false;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            IsCancelled = true;
        }

        private void AppProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCancelled)
            {
                string ErrorStr = "";
                if (AppName == "")
                    ErrorStr = "Please enter a name.";
                if (AppFileName == "")
                    ErrorStr = (ErrorStr != "" ? ErrorStr += "\r\n":"") + "Please browse to a file to launch.";
                if (ErrorStr != "")
                {
                    MessageBox.Show(this, ErrorStr, "Error");
                    e.Cancel = true;
                }
            }
        }
    }
}
