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
    public partial class FolderLink : Form
    {
        public FolderLink(Config myConfig)
        {
            InitializeComponent();
            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            EditFolderName.BackColor = BackColor;
            EditFolderName.ForeColor = ForeColor;
            EditFolderPath.BackColor = BackColor;
            EditFolderPath.ForeColor = ForeColor;
            ButtonOK.BackColor = BackColor;
            ButtonCancel.BackColor = BackColor;
        }

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
        }
    }
}
