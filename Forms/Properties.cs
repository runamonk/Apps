using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Utility;


namespace Apps.Forms
{
    public partial class Properties : Form
    {

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle |= CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        public Properties(Config myConfig)
        {
            InitializeComponent();
            AppsConfig = myConfig;
            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            EditAppFilePath.BackColor = myConfig.AppsBackColor;
            EditAppFilePath.ForeColor = myConfig.AppsFontColor;
            EditAppName.BackColor = myConfig.AppsBackColor;
            EditAppName.ForeColor = myConfig.AppsFontColor;
            EditFileArgs.BackColor = myConfig.AppsBackColor;
            EditFileArgs.ForeColor = myConfig.AppsFontColor;
            ButtonParseShortcut.BackColor = myConfig.AppsBackColor;
            ButtonParseShortcut.ForeColor = myConfig.AppsFontColor;
            EditWorkingFolder.BackColor = myConfig.AppsBackColor;
            EditWorkingFolder.ForeColor = myConfig.AppsFontColor;
            toolTip.SetToolTip(Browse, "Click to browse for file(s).");
            toolTip.SetToolTip(ButtonParseShortcut, "Click to replace file properties from the shortcut.");
        }

        private readonly Config AppsConfig;

        public string AppFileName
        {
            get {return EditAppFilePath.Text.Trim(); }
        }

        public string AppName
        {
            get { return EditAppName.Text.Trim(); }
        }

        private string FAppIconPath;
        private string FAppIconIndex;
        private bool IsCancelled = false;

        public string AppFileArgs
        {
            get { return this.EditFileArgs.Text.Trim(); }
        }

        public string AppFileWorkingFolder
        {
            get { return this.EditWorkingFolder.Text.Trim(); }
        }

        public string AppIconIndex
        {
            get { return FAppIconIndex; }
        }

        public string AppIconPath
        {
            get { return FAppIconPath; }
        }

        public void SetFileProperties(string appName, string filePath, string fileIcon, string fileIconIndex, string fileArgs, string fileWorkingFolder)
        {
            EditAppName.Text = appName;
            EditAppFilePath.Text = filePath;
            EditFileArgs.Text = fileArgs;
            EditWorkingFolder.Text = fileWorkingFolder;

            string s = fileIcon;

            if (string.IsNullOrEmpty(s))
                s = filePath;

            FAppIconPath = s;
            FAppIconIndex = fileIconIndex;
            EditAppIcon.Image = Funcs.GetIcon(s, fileIconIndex);
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            string fileName = Funcs.BrowseForFile();   
            if (fileName != "")
            {
                if (AppsConfig.ParseShortcuts && Funcs.IsShortcut(fileName))
                {
                    EditAppFilePath.Text = fileName;
                    ButtonParseShortcut.PerformClick();
                }
                else
                {
                    FileVersionInfo f = Funcs.GetFileInfo(fileName);
                    SetFileProperties(f.ProductName, fileName, null, null, null, null);
                }
            }
        }

        private void AppIcon_Click(object sender, EventArgs e)
        {
            string fileName = Funcs.BrowseForFile();
            if (fileName != "")
            {
                if ((Path.GetExtension(fileName) == ".dll") || (Path.GetExtension(fileName) == ".exe"))
                {
                    List<Icon> l = Funcs.GetIcons(fileName);
                    IconPicker frm = new IconPicker(l);
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        FAppIconIndex = frm.SelectedIconIndex.ToString();
                        FAppIconPath = fileName;
                        EditAppIcon.Image = Funcs.GetIcon(fileName, FAppIconIndex);
                    }
                }
                else
                {
                    FAppIconPath = fileName;
                    EditAppIcon.Image = Funcs.GetIcon(fileName, null);
                }
            }
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
                    ErrorStr = (ErrorStr != "" ? ErrorStr += "\r\n" : "") + "Please browse to a file to launch.";

                EditWorkingFolder.Text = EditWorkingFolder.Text.Trim();

                if ((EditWorkingFolder.Text != "") && (!Directory.Exists(EditWorkingFolder.Text)))
                {
                    e.Cancel = (Misc.ConfirmDialog(AppsConfig, ConfirmButtons.YesNo, "Are you sure?", "Working folder: " + EditWorkingFolder.Text + " cannot be found.") != DialogResult.Yes);
                }
  
                if (ErrorStr != "")
                {
                    Misc.ShowMessage(AppsConfig,"Error", ErrorStr);
                    e.Cancel = true;
                }
            }
        }

        private void EditAppFilePath_TextChanged(object sender, EventArgs e)
        {
            ButtonParseShortcut.Enabled = Funcs.IsShortcut(EditAppFilePath.Text);
        }

        private void ButtonParseShortcut_Click(object sender, EventArgs e)
        {
            string FileName = "";
            string FileIcon = "";
            string FileIconIndex = "";
            string FileArgs = "";
            string FileWF = "";
            
            Funcs.ParseShortcut(EditAppFilePath.Text, ref FileName, ref FileIcon, ref FileIconIndex, ref FileArgs, ref FileWF);
            SetFileProperties(Path.GetFileNameWithoutExtension(EditAppFilePath.Text), FileName, FileIcon, FileIconIndex, FileArgs, FileWF);
        }

        private void BrowseWF_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if ((f.ShowDialog() == DialogResult.OK) && (Directory.Exists(f.SelectedPath)))
                EditWorkingFolder.Text = f.SelectedPath;
        }

        private void AppProperties_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else
            if (e.KeyCode == Keys.Escape)
                ButtonCancel.PerformClick();
        }
    }
}
