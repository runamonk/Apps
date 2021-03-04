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
    public partial class Confirm : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public Confirm(Config myConfig)
        {
            InitializeComponent();
            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            ButtonOK.BackColor = BackColor;
            ButtonCancel.BackColor = BackColor;
        }

        public DialogResult ShowAsDialog(string Caption, string Message)
        {
            Text = Caption;
            ConfirmText.Text = Message;
            return ShowDialog();
        }

        private void Confirm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else
            if (e.KeyCode == Keys.Escape)
                ButtonCancel.PerformClick();
        }
    }
}
