using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Utility;


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

        public string AppFileArgs
        {
            get { return this.EditFileArgs.Text.Trim(); }
        }

        public string AppIconPath
        {
            get { return FAppIconPath; }
        }

        //public Image AppIconImage
        //{
        //    get { return Funcs.GetIcon(FAppIconPath); }
        //}

        public void SetFileProperties(string appName, string filePath, string fileIcon)
        {
            EditAppName.Text = appName;
            EditAppFilePath.Text = filePath;

            string s = fileIcon;

            if (s == null)
                s = filePath;

            EditAppIcon.Image = Funcs.GetIcon(s);
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            string fileName = Funcs.BrowseForFile();   
            if (fileName != "")
            {
                FileVersionInfo f = FileVersionInfo.GetVersionInfo(fileName);
                SetFileProperties(f.ProductName, f.FileName, null);
            }
        }

        private void AppIcon_Click(object sender, EventArgs e)
        {
            string fileName = Funcs.BrowseForFile();
            if (fileName != "")
            {
                FAppIconPath = fileName;
                EditAppIcon.Image = Funcs.GetIcon(fileName);
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
