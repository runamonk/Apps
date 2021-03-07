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
    public enum ConfirmButtons
    {
        OKCancel,
        YesNo
    }

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
            ButtonOne.BackColor = BackColor;
            ButtonTwo.BackColor = BackColor;
        }

        public DialogResult ShowAsDialog(ConfirmButtons Buttons, string Caption, string Message)
        {
            if (Buttons == ConfirmButtons.OKCancel)
            {
                ButtonOne.Text = "OK";
                ButtonTwo.Text = "Cancel";
                ButtonOne.DialogResult = DialogResult.OK;
                ButtonTwo.DialogResult = DialogResult.Cancel;
            }
            else
            if (Buttons == ConfirmButtons.YesNo)
            {
                ButtonOne.Text = "Yes";
                ButtonTwo.Text = "No";
                ButtonOne.DialogResult = DialogResult.Yes;
                ButtonTwo.DialogResult = DialogResult.No;
            }
            Text = Caption;
            ConfirmText.Text = Message;
            return ShowDialog();
        }

        private void Confirm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOne.PerformClick();
            else
            if (e.KeyCode == Keys.Escape)
                ButtonTwo.PerformClick();
        }
    }
}
