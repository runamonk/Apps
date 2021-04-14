using Apps.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Utility;


namespace Apps.Forms
{
    public partial class Properties : Form
    {
        public Properties(Config myConfig, AppButton appButton)
        {
            InitializeComponent();
            AppsConfig = myConfig;
            FAppButton = appButton;
            MenuRC = new AppMenu(myConfig)
            {
                ShowCheckMargin = false,
                ShowImageMargin = false
            };
            Funcs.AddMenuItem(MenuRC, "Reset", MenuReset_Click);
            EditAppIcon.ContextMenuStrip = MenuRC;

            SetColors();
        }

        #region Properties
        public string AppFileArgs
        {
            get { return this.EditFileArgs.Text.Trim(); }
        }
        public string AppFileName
        {
            get { return EditAppFilePath.Text.Trim(); }
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
        public string AppName
        {
            get { return EditAppName.Text.Trim(); }
        }
        #endregion

        #region Privates
        private readonly Config AppsConfig;
        private const int CP_NOCLOSE_BUTTON = 0x200;
        private bool IsCancelled = false;
        private readonly AppMenu MenuRC; 
        private string FAppIconIndex;
        private string FAppIconPath;
        private readonly AppButton FAppButton;
        #endregion

        #region Events
        private void AppIcon_Click(object sender, EventArgs e)
        {
            void ShowIconPicker(string fileName)
            {
                IconPicker frm = new IconPicker(AppsConfig, fileName);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    FAppIconIndex = frm.SelectedIconIndex.ToString();
                    FAppIconPath = frm.SelectedFileName;
                    EditAppIcon.Image = Funcs.GetIcon(frm.SelectedFileName, FAppIconIndex);
                }
                frm.Dispose();
            }
            ShowIconPicker(FAppIconPath);
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
                    Misc.ShowMessage(AppsConfig, "Error", ErrorStr);
                    e.Cancel = true;
                }
                else
                {
                    FAppButton.AppName = EditAppName.Text;
                    FAppButton.FileName = EditAppFilePath.Text;
                    FAppButton.FileWorkingFolder = EditWorkingFolder.Text;
                    FAppButton.FileIconPath = FAppIconPath;
                    FAppButton.FileIconIndex = FAppIconIndex;
                    FAppButton.FileIconImage = EditAppIcon.Image;
                    FAppButton.FileArgs = EditFileArgs.Text;
                }
            }
        }
        private void AppProperties_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else
            if (e.KeyCode == Keys.Escape)
                ButtonCancel.PerformClick();
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
                    FAppButton.AppName = f.ProductName;
                    EditAppFilePath.Text = fileName;
                }
            }
        }
        private void BrowseWF_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if ((f.ShowDialog() == DialogResult.OK) && (Directory.Exists(f.SelectedPath)))
                EditWorkingFolder.Text = f.SelectedPath;
        }
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            IsCancelled = true;
        }
        private void ButtonParseShortcut_Click(object sender, EventArgs e)
        {
            Funcs.ParseShortcut(EditAppFilePath.Text, out string fileName, out string fileIcon, out string fileIconIdx, out string fileArgs, out string fileWF);
            if (EditAppName.Text == "")
                EditAppName.Text = Path.GetFileName(fileName);
            EditAppFilePath.Text = fileName;
            FAppIconIndex = fileIconIdx;
            FAppIconPath = (fileIcon != "" ? fileIcon : fileName);
            EditAppIcon.Image = Funcs.GetIcon(FAppIconPath, FAppIconIndex);
            EditFileArgs.Text =  fileArgs;
            EditWorkingFolder.Text = fileWF;
        }
        private void EditAppFilePath_TextChanged(object sender, EventArgs e)
        {
            ButtonParseShortcut.Enabled = Funcs.IsShortcut(EditAppFilePath.Text);
        }
        private void MenuReset_Click(object sender, EventArgs e)
        {
            if (File.Exists(AppFileName))
            {
                FAppIconPath = AppFileName;
                FAppIconIndex = "0";
                EditAppIcon.Image = Funcs.GetIcon(AppFileName, null);
            }
        }
        private void Properties_Load(object sender, EventArgs e)
        {
            EditAppName.Text = FAppButton.AppName;
            EditAppFilePath.Text = FAppButton.FileName;
            EditFileArgs.Text = FAppButton.FileArgs;
            EditWorkingFolder.Text = FAppButton.FileWorkingFolder;
            FAppIconPath = (FAppButton.FileIconPath == "" ? FAppButton.FileName : FAppButton.FileIconPath);
            FAppIconIndex = FAppButton.FileIconIndex;
            EditAppIcon.Image = Funcs.GetIcon(FAppIconPath, FAppIconIndex);
        }
        #endregion

        #region Methods
        private void SetColors()
        {
            BackColor = AppsConfig.AppsBackColor;
            ForeColor = AppsConfig.AppsFontColor;
            EditAppFilePath.BackColor = AppsConfig.AppsBackColor;
            EditAppFilePath.ForeColor = AppsConfig.AppsFontColor;
            EditAppName.BackColor = AppsConfig.AppsBackColor;
            EditAppName.ForeColor = AppsConfig.AppsFontColor;
            EditFileArgs.BackColor = AppsConfig.AppsBackColor;
            EditFileArgs.ForeColor = AppsConfig.AppsFontColor;
            ButtonParseShortcut.BackColor = AppsConfig.AppsBackColor;
            ButtonParseShortcut.ForeColor = AppsConfig.AppsFontColor;
            EditWorkingFolder.BackColor = AppsConfig.AppsBackColor;
            EditWorkingFolder.ForeColor = AppsConfig.AppsFontColor;
        }
        #endregion

        #region Overrides
        protected override CreateParams CreateParams
        {
            get {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle |= CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        #endregion
    }
}
