using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Apps.Forms
{
    public partial class Folder : Form
    {
        public Folder(Config myConfig, AppButton appButton)
        {
            InitializeComponent();
            AppsConfig = myConfig;
            FAppButton = appButton;

            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            FolderNameEdit.BackColor = BackColor;
            FolderNameEdit.ForeColor = ForeColor;
            ButtonOK.BackColor = BackColor;
            ButtonCancel.BackColor = BackColor;
        }

        #region Properties
        private string FolderName
        {
            get { return FolderNameEdit.Text.Trim(); }
            set {
                FolderNameEdit.Text = value;
            }
        }
        #endregion

        #region Private
        private bool IsCancelled = false;
        private readonly Config AppsConfig;
        private AppButton FAppButton;
        private const int CP_NOCLOSE_BUTTON = 0x200;
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

        #region Events
        private void AddFolder_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FolderNameEdit.Text))
                Text = "Add Folder";
            else
                Text = "Edit Folder";

            FolderName = FAppButton.AppName;
        }

        private void AddFolder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCancelled)
            {
                {
                    string ErrorStr = "";
                    if (FolderName == "")
                        ErrorStr = "Please enter a folder name.";

                    if (ErrorStr != "")
                    {
                        Message f = new Message(AppsConfig);
                        f.ShowAsDialog("Error", ErrorStr);
                        e.Cancel = true;
                    }
                    else
                    {
                        FAppButton.AppName = FolderName;
                    }
                }
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            IsCancelled = true;
        }

        private void AddFolder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else
            if (e.KeyCode == Keys.Escape)
                ButtonCancel.PerformClick();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            IsCancelled = false;
        }
        #endregion
    }
}
