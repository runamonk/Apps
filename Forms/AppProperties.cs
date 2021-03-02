using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Utility;


namespace Apps.Forms
{
    public partial class AppProperties : Form
    {
        public AppProperties(Config myConfig)
        {
            InitializeComponent();
            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            EditAppFilePath.BackColor = myConfig.AppsBackColor;
            EditAppFilePath.ForeColor = myConfig.AppsFontColor;
            EditAppName.BackColor = myConfig.AppsBackColor;
            EditAppName.ForeColor = myConfig.AppsFontColor;
            EditFileArgs.BackColor = myConfig.AppsBackColor;
            EditFileArgs.ForeColor = myConfig.AppsFontColor;
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

        public void SetFileProperties(string appName, string filePath, string fileIcon, string fileArgs)
        {
            EditAppName.Text = appName;
            EditAppFilePath.Text = filePath;
            EditFileArgs.Text = fileArgs;

            string s = fileIcon;

            if (string.IsNullOrEmpty(s))
                s = filePath;

            EditAppIcon.Image = Funcs.GetIcon(s);
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            string fileName = Funcs.BrowseForFile();   
            if (fileName != "")
            {
                FileVersionInfo f = Funcs.GetFileInfo(fileName);
                SetFileProperties(f.ProductName, f.FileName, null, null);
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
                // We'll use this for seperators.
                if (AppName == "-")
                {
                    
                }
                else
                {
                    string ErrorStr = "";
                    if (AppName == "")
                        ErrorStr = "Please enter a name.";
                    if (AppFileName == "")
                        ErrorStr = (ErrorStr != "" ? ErrorStr += "\r\n" : "") + "Please browse to a file to launch.";
                    if (ErrorStr != "")
                    {
                        MessageBox.Show(this, ErrorStr, "Error");
                        e.Cancel = true;
                    }
                }
            }
        }

        private void EditAppName_TextChanged(object sender, EventArgs e)
        {
            if (EditAppName.Text.Trim() == "-")
            {
                EditFileArgs.Text = "";
                EditFileArgs.ReadOnly = true;
                EditAppFilePath.Text = "";
                EditAppFilePath.ReadOnly = true;
                EditAppIcon.Image = null;
                EditAppIcon.Enabled = false;
            }
            else
            {
                EditFileArgs.ReadOnly = true;
                EditAppFilePath.ReadOnly = true;
                EditAppIcon.Enabled = true;
            }
        }
    }
}
