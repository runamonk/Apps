﻿using System;
using System.IO;
using System.Windows.Forms;

namespace Apps.Forms
{
    public partial class FolderLink : Form
    {
        private const int CpNocloseButton = 0x200;

        private readonly Config _appsConfig;
        private readonly AppButton _fAppButton;
        private bool _isCancelled;

        public FolderLink(Config myConfig, AppButton appButton)
        {
            InitializeComponent();
            _appsConfig = myConfig;
            _fAppButton = appButton;

            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            EditFolderName.BackColor = BackColor;
            EditFolderName.ForeColor = ForeColor;
            EditFolderPath.BackColor = BackColor;
            EditFolderPath.ForeColor = ForeColor;
            ButtonOK.BackColor = BackColor;
            ButtonCancel.BackColor = BackColor;
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

        public string FolderName { get => EditFolderName.Text.Trim(); set => EditFolderName.Text = value; }

        public string FolderPath { get => EditFolderPath.Text.Trim(); set => EditFolderPath.Text = value; }

        private void BrowseWF_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK && Directory.Exists(f.SelectedPath))
            {
                EditFolderName.Text = Path.GetFileName(f.SelectedPath);
                EditFolderPath.Text = f.SelectedPath;
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e) { _isCancelled = true; }

        private void ButtonOK_Click(object sender, EventArgs e) { _isCancelled = false; }

        private void FolderLink_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isCancelled)
            {
                string errorStr = "";

                if (FolderName == "")
                    errorStr = "Please enter a folder name.";

                if (FolderPath == "")
                    errorStr = (errorStr != "" ? errorStr += "\r\n" : "") + "Please enter a folder path.";

                if (FolderPath != "" && !Directory.Exists(FolderPath))
                    e.Cancel = Misc.ConfirmDialog(_appsConfig, ConfirmButtons.YesNo, "Are you sure?", "Folder: " + FolderPath + " cannot be found.") != DialogResult.Yes;

                if (errorStr != "")
                {
                    Message f = new Message(_appsConfig);
                    f.ShowAsDialog("Error", errorStr);
                    e.Cancel = true;
                }
                else
                {
                    _fAppButton.AppName = FolderName;
                    _fAppButton.FileName = FolderPath;
                }
            }
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else if (e.KeyCode == Keys.Escape)
                ButtonCancel.PerformClick();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            FolderName = _fAppButton.AppName;
            FolderPath = _fAppButton.FileName;
            if (string.IsNullOrEmpty(EditFolderName.Text))
                Text = "Add Folder Link";
            else
                Text = "Edit Folder Link";
        }
    }
}