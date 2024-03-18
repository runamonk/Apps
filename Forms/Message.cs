using System.Windows.Forms;

namespace Apps.Forms
{
    public partial class Message : Form
    {
        private const int CpNocloseButton = 0x200;

        public Message(Config myConfig)
        {
            InitializeComponent();
            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            ButtonOK.BackColor = BackColor;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var myCp = base.CreateParams;
                myCp.ClassStyle |= CpNocloseButton;
                return myCp;
            }
        }

        private void Message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else if (e.KeyCode == Keys.Escape)
                ButtonOK.PerformClick();
        }

        public DialogResult ShowAsDialog(string caption, string message)
        {
            Text = caption;
            MessageText.Text = message;
            return ShowDialog();
        }
    }
}