﻿using System;
using System.Windows.Forms;
using Utility;

namespace Apps.Forms
{
    public partial class WebLink : Form
    {
        private const int CpNocloseButton = 0x200;

        private readonly Config _appsConfig;
        private readonly AppButton _fAppButton;
        private bool _isCancelled;

        public WebLink(Config myConfig, AppButton appButton)
        {
            InitializeComponent();
            _appsConfig = myConfig;
            _fAppButton = appButton;

            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            EditUrlName.BackColor = BackColor;
            EditUrlName.ForeColor = ForeColor;
            EditUrlPath.BackColor = BackColor;
            EditUrlPath.ForeColor = ForeColor;
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

        public string UrlName { get => EditUrlName.Text.Trim(); set => EditUrlName.Text = value; }

        public string UrlPath { get => EditUrlPath.Text.Trim(); set => EditUrlPath.Text = value; }

        private void ButtonCancel_Click(object sender, EventArgs e) { _isCancelled = true; }

        private void ButtonOK_Click(object sender, EventArgs e) { _isCancelled = false; }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else if (e.KeyCode == Keys.Escape)
                ButtonCancel.PerformClick();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            UrlName = _fAppButton.AppName;
            UrlPath = _fAppButton.Url;

            if (string.IsNullOrEmpty(EditUrlName.Text))
                Text = "Add URL";
            else
                Text = "Edit URL";
        }

        private void WebLink_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isCancelled)
            {
                string errorStr = "";

                if (UrlName == "")
                    errorStr = "Please enter a link name.";

                if (UrlPath == "")
                    errorStr = (errorStr != "" ? errorStr += "\r\n" : "") + "Please enter a url.";

                if (!Funcs.IsUrl(UrlPath))
                    errorStr = (errorStr != "" ? errorStr += "\r\n" : "") + "Please enter a properly formatted url. \r\n (https://www.website.com or ftp://ftp.website.com)";

                if (errorStr != "")
                {
                    Message f = new Message(_appsConfig);
                    f.ShowAsDialog("Error", errorStr);
                    e.Cancel = true;
                }
                else
                {
                    _fAppButton.AppName = UrlName;
                    _fAppButton.Url = UrlPath;
                }
            }
        }
    }
}