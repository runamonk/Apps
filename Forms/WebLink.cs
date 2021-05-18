using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace Apps.Forms
{
    public partial class WebLink : Form
    {
        public WebLink(Config myConfig, AppButton appButton)
        {
            InitializeComponent();
            AppsConfig = myConfig;
            FAppButton = appButton;

            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            EditUrlName.BackColor = BackColor;
            EditUrlName.ForeColor = ForeColor;
            EditUrlPath.BackColor = BackColor;
            EditUrlPath.ForeColor = ForeColor;
            ButtonOK.BackColor = BackColor;
            ButtonCancel.BackColor = BackColor;
        }

        #region  Properties
        public string UrlName
        {
            get { return EditUrlName.Text.Trim(); }
            set {
                EditUrlName.Text = value;
            }
        }
        public string UrlPath
        {
            get { return EditUrlPath.Text.Trim(); }
            set {
                EditUrlPath.Text = value;
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
            if (string.IsNullOrEmpty(EditUrlName.Text))
                Text = "Add URL";
            else
                Text = "Edit URL";
            UrlName = FAppButton.AppName;
            UrlPath = FAppButton.Url;
        }
        private void WebLink_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsCancelled)
            {
                {
                    string ErrorStr = "";

                    if (UrlName == "")
                        ErrorStr = "Please enter a link name.";

                    if (UrlPath == "")
                        ErrorStr = (ErrorStr != "" ? ErrorStr += "\r\n" : "") + "Please enter a url.";

                    if (!Funcs.IsUrl(UrlPath))
                        ErrorStr = (ErrorStr != "" ? ErrorStr += "\r\n" : "") + "Please enter a properly formatted url. \r\n (https://www.website.com or ftp://ftp.website.com)";

                    if (ErrorStr != "")
                    {
                        Message f = new Message(AppsConfig);
                        f.ShowAsDialog("Error", ErrorStr);
                        e.Cancel = true;
                    }
                    else
                    {
                        FAppButton.AppName = UrlName;
                        FAppButton.Url = UrlPath;
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
