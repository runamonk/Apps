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
    public partial class Message : Form
    {
        public Message(Config myConfig)
        {
            InitializeComponent();
            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            ButtonOK.BackColor = BackColor;
        }

        private void Message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else
            if (e.KeyCode == Keys.Escape)
                ButtonOK.PerformClick();
        }

        public DialogResult ShowAsDialog(string Caption, string Message)
        {
            Text = Caption;
            MessageText.Text = Message;
            return ShowDialog();
        }
    }
}
