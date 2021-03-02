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

            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            FolderNameEdit.BackColor = BackColor;
            FolderNameEdit.ForeColor = ForeColor;
            ButtonOK.BackColor = BackColor;
            ButtonCancel.BackColor = BackColor;
        }

        public string FolderName
        {
            get { return FolderNameEdit.Text.Trim(); }
            set {
                FolderNameEdit.Text = value;
            }
        }

        private void FolderName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
        }

        private void AddFolder_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FolderNameEdit.Text))
                Text = "Add Folder";
            else
                Text = "Edit Folder";
        }
    }
}
