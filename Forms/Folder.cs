using System;
using System.Windows.Forms;

namespace Apps.Forms
{
    public partial class Folder : Form
    {
        private const int CpNocloseButton = 0x200;
        private readonly Config _appsConfig;
        private readonly AppButton _fAppButton;

        private bool _isCancelled;

        public Folder(Config myConfig, AppButton appButton)
        {
            InitializeComponent();
            _appsConfig = myConfig;
            _fAppButton = appButton;

            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            FolderNameEdit.BackColor = BackColor;
            FolderNameEdit.ForeColor = ForeColor;
            ButtonOK.BackColor = BackColor;
            ButtonCancel.BackColor = BackColor;
        }

        private string FolderName { get => FolderNameEdit.Text.Trim(); set => FolderNameEdit.Text = value; }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle |= CpNocloseButton;
                return myCp;
            }
        }

        private void AddFolder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isCancelled)
            {
                string errorStr = "";
                if (FolderName == "")
                    errorStr = "Please enter a folder name.";

                if (errorStr != "")
                {
                    Message f = new Message(_appsConfig);
                    f.ShowAsDialog("Error", errorStr);
                    e.Cancel = true;
                }
                else
                {
                    _fAppButton.AppName = FolderName;
                }
            }
        }

        private void AddFolder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else if (e.KeyCode == Keys.Escape)
                ButtonCancel.PerformClick();
        }

        private void AddFolder_Load(object sender, EventArgs e)
        {
            FolderName = _fAppButton.AppName;
            if (string.IsNullOrEmpty(FolderNameEdit.Text))
                Text = "Add Folder";
            else
                Text = "Edit Folder";
        }

        private void ButtonCancel_Click(object sender, EventArgs e) { _isCancelled = true; }

        private void ButtonOK_Click(object sender, EventArgs e) { _isCancelled = false; }
    }
}