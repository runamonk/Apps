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
    public partial class AddFolder : Form
    {
        public AddFolder(Config myConfig)
        {
            InitializeComponent();
            AppsConfig = myConfig;
            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            FolderNameEdit.BackColor = BackColor;
            FolderNameEdit.ForeColor = ForeColor;
            ButtonOK.BackColor = BackColor;
            ButtonCancel.BackColor = BackColor;
        }

        private bool IsCancelled = false;
        private Config AppsConfig;

        public string FolderName
        {
            get { return FolderNameEdit.Text.Trim(); }
            set {
                FolderNameEdit.Text = value;
            }
        }

        private void AddFolder_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FolderNameEdit.Text))
                Text = "Add Folder";
            else
                Text = "Edit Folder";
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
    }
}
