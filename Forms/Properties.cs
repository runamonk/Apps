using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Apps.Controls;
using Icons;
using Utility;

namespace Apps.Forms
{
    public partial class Properties : Form
    {
        private const int CpNocloseButton = 0x200;

        private readonly Config _appsConfig;
        private readonly AppButton _fAppButton;
        private readonly AppMenu _menuRc;
        private bool _isCancelled;

        public Properties(Config myConfig, AppButton appButton)
        {
            InitializeComponent();
            _appsConfig = myConfig;
            _fAppButton = appButton;
            _menuRc = new AppMenu(myConfig) { ShowCheckMargin = false, ShowImageMargin = false };
            Funcs.AddMenuItem(_menuRc, "Reset", MenuReset_Click);
            Funcs.AddMenuItem(_menuRc, "Save",  MenuSave_Click);

            EditAppIcon.ContextMenuStrip = _menuRc;

            SetColors();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle |= CpNocloseButton;
                return myCp;
            }
        }

        public string AppFileArgs => EditFileArgs.Text.Trim();

        public string AppFileName => EditAppFilePath.Text.Trim();

        public string AppFileWorkingFolder => EditWorkingFolder.Text.Trim();

        public string AppIconIndex { get; private set; }

        public string AppIconPath { get; private set; }

        public string AppName => EditAppName.Text.Trim();

        private void SetColors()
        {
            BackColor = _appsConfig.AppsBackColor;
            ForeColor = _appsConfig.AppsFontColor;
            EditAppFilePath.BackColor = _appsConfig.AppsBackColor;
            EditAppFilePath.ForeColor = _appsConfig.AppsFontColor;
            EditAppName.BackColor = _appsConfig.AppsBackColor;
            EditAppName.ForeColor = _appsConfig.AppsFontColor;
            EditFileArgs.BackColor = _appsConfig.AppsBackColor;
            EditFileArgs.ForeColor = _appsConfig.AppsFontColor;
            ButtonParseShortcut.BackColor = _appsConfig.AppsBackColor;
            ButtonParseShortcut.ForeColor = _appsConfig.AppsFontColor;
            EditWorkingFolder.BackColor = _appsConfig.AppsBackColor;
            EditWorkingFolder.ForeColor = _appsConfig.AppsFontColor;
        }

        private void AppIcon_Click(object sender, EventArgs e)
        {
            void ShowIconPicker(string fileName)
            {
                if (string.IsNullOrEmpty(fileName)) return;

                IconPicker frm = new IconPicker(_appsConfig, fileName);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    AppIconIndex = frm.SelectedIconIndex.ToString();
                    AppIconPath = frm.SelectedFileName;
                    EditAppIcon.Image = IconFuncs.GetIcon(frm.SelectedFileName, AppIconIndex);
                }

                frm.Dispose();
            }

            ShowIconPicker(AppIconPath != "" ? AppIconPath : AppFileName);
        }

        private void AppProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isCancelled)
            {
                string errorStr = "";
                if (AppName == "")
                    errorStr = "Please enter a name.";
                if (AppFileName == "")
                    errorStr = (errorStr != "" ? errorStr += "\r\n" : "") + "Please browse to a file to launch.";

                EditWorkingFolder.Text = EditWorkingFolder.Text.Trim();

                if (EditWorkingFolder.Text != "" && !Directory.Exists(EditWorkingFolder.Text) && !IconFuncs.IsShellApp(EditWorkingFolder.Text))
                    e.Cancel = Misc.ConfirmDialog(_appsConfig, ConfirmButtons.YesNo, "Are you sure?", "Working folder: " + EditWorkingFolder.Text + " cannot be found.") != DialogResult.Yes;

                if (errorStr != "")
                {
                    Misc.ShowMessage(_appsConfig, "Error", errorStr);
                    e.Cancel = true;
                }
                else
                {
                    _fAppButton.AppName = EditAppName.Text;
                    _fAppButton.FileName = EditAppFilePath.Text;
                    _fAppButton.FileWorkingFolder = EditWorkingFolder.Text;
                    _fAppButton.FileIconPath = AppIconPath;
                    _fAppButton.FileIconIndex = AppIconIndex;
                    _fAppButton.FileIconImage = EditAppIcon.Image;
                    _fAppButton.FileArgs = EditFileArgs.Text;
                    if (chkAsAdmin.Checked)
                        _fAppButton.AsAdmin = "Y";
                    else
                        _fAppButton.AsAdmin = "N";
                }
            }
        }

        private void AppProperties_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else if (e.KeyCode == Keys.Escape)
                ButtonCancel.PerformClick();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            string fileName = Funcs.BrowseForFile();
            if (fileName != "")
            {
                if (_appsConfig.ParseShortcuts && Misc.IsShortcut(fileName))
                {
                    EditAppFilePath.Text = fileName;
                    ButtonParseShortcut.PerformClick();
                }
                else
                {
                    FileVersionInfo f = Funcs.GetFileInfo(fileName);
                    _fAppButton.AppName = f.ProductName;
                    EditAppFilePath.Text = fileName;
                }
            }
        }

        private void BrowseWF_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK && Directory.Exists(f.SelectedPath))
                EditWorkingFolder.Text = f.SelectedPath;
        }

        private void ButtonCancel_Click(object sender, EventArgs e) { _isCancelled = true; }

        private void ButtonParseShortcut_Click(object sender, EventArgs e)
        {
            Misc.ParseShortcut(EditAppFilePath.Text, out string fileName, out string fileIcon, out string fileIconIdx, out string fileArgs, out string fileWf);
            if (EditAppName.Text == "")
                EditAppName.Text = Path.GetFileName(fileName);
            EditAppFilePath.Text = fileName;
            AppIconIndex = fileIconIdx;
            AppIconPath = fileIcon != "" ? fileIcon : fileName;
            EditAppIcon.Image = IconFuncs.GetIcon(AppIconPath, AppIconIndex);
            EditFileArgs.Text = fileArgs;
            EditWorkingFolder.Text = fileWf;
        }

        private void EditAppFilePath_TextChanged(object sender, EventArgs e)
        {
            ButtonParseShortcut.Enabled = Misc.IsShortcut(EditAppFilePath.Text);
            bool isShellApp = IconFuncs.IsShellApp(EditAppFilePath.Text);
            EditWorkingFolder.Enabled = !isShellApp;
            EditAppFilePath.Enabled = !isShellApp;
            Browse.Enabled = !isShellApp;
            BrowseWF.Enabled = !isShellApp;
            EditFileArgs.Enabled = !isShellApp;
        }

        private void MenuReset_Click(object sender, EventArgs e)
        {
            if (File.Exists(AppFileName) || IconFuncs.IsShellApp(AppFileName))
            {
                AppIconPath = AppFileName;
                AppIconIndex = "0";
                EditAppIcon.Image = IconFuncs.GetIcon(AppFileName, null);
            }
        }

        private void MenuSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog f = new SaveFileDialog();
            if (f.ShowDialog() == DialogResult.OK)
                EditAppIcon.Image.Save(f.FileName);
        }


        private void Properties_Load(object sender, EventArgs e)
        {
            EditAppName.Text = _fAppButton.AppName;
            EditAppFilePath.Text = _fAppButton.FileName;
            EditFileArgs.Text = _fAppButton.FileArgs;
            EditWorkingFolder.Text = _fAppButton.FileWorkingFolder;
            AppIconPath = _fAppButton.FileIconPath == "" ? _fAppButton.FileName : _fAppButton.FileIconPath;
            AppIconIndex = _fAppButton.FileIconIndex;
            EditAppIcon.Image = IconFuncs.GetIcon(AppIconPath, AppIconIndex);
            chkAsAdmin.Checked = _fAppButton.AsAdmin == "Y";

            if (string.IsNullOrEmpty(EditAppName.Text))
                Text = "Add Application";
            else
                Text = "Edit Application Properties";
        }
    }
}