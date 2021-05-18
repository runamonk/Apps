using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Apps.Forms
{
    public partial class FolderLink : Form
    {
        public FolderLink(Config myConfig, AppButton appButton)
        {
            InitializeComponent();
            AppsConfig = myConfig;
            FAppButton = appButton;

            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            EditFolderName.BackColor = BackColor;
            EditFolderName.ForeColor = ForeColor;
            EditFolderPath.BackColor = BackColor;
            EditFolderPath.ForeColor = ForeColor;
            ButtonOK.BackColor = BackColor;
            ButtonCancel.BackColor = BackColor;
        }

        #region  Properties
        public string FolderName
        {
            get { return EditFolderName.Text.Trim(); }
            set {
                EditFolderName.Text = value;
            }
        }
        public string FolderPath
        {
            get { return EditFolderPath.Text.Trim(); }
            set {
                EditFolderPath.Text = value;
            }
        }
        #endregion

        #region Private
        readonly Config AppsConfig;
        bool IsCancelled = false;
        private readonly AppButton FAppButton;
        private const int CP_NOCLOSE_BUTTON = 0x200;
        #endregion

        #region Events
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else
            if (e.KeyCode == Keys.Escape)
                ButtonCancel.PerformClick();
        }
        private void Form_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EditFolderName.Text))
                Text = "Add Folder Link";
            else
                Text = "Edit Folder Link";
            FolderName = FAppButton.AppName;
            FolderPath = FAppButton.FileName;
        }
        private void FolderLink_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCancelled)
            {
                {
                    string ErrorStr = "";

                    if (FolderName == "")
                        ErrorStr = "Please enter a folder name.";

                    if (FolderPath == "")
                        ErrorStr = (ErrorStr != "" ? ErrorStr += "\r\n" : "") + "Please enter a folder path.";

                    if ((FolderPath != "") && (!Directory.Exists(FolderPath)))
                    {
                        e.Cancel = (Misc.ConfirmDialog(AppsConfig, ConfirmButtons.YesNo, "Are you sure?", "Folder: " + FolderPath + " cannot be found.") != DialogResult.Yes);
                    }

                    if (ErrorStr != "")
                    {
                        Message f = new Message(AppsConfig);
                        f.ShowAsDialog("Error", ErrorStr);
                        e.Cancel = true;
                    }
                    else
                    {
                        FAppButton.AppName = FolderName;
                        FAppButton.FileName = FolderPath;
                    }
                }
            }
        }
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            IsCancelled = true;
        }
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            IsCancelled = false;
        }
        private void BrowseWF_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if ((f.ShowDialog() == DialogResult.OK) && (Directory.Exists(f.SelectedPath)))
            {
                EditFolderName.Text = Path.GetFileName(f.SelectedPath);
                EditFolderPath.Text = f.SelectedPath;
            }
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
