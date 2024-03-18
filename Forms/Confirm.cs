using System.Windows.Forms;

namespace Apps.Forms
{
    public enum ConfirmButtons
    {
        OkCancel,
        YesNo
    }

    public partial class Confirm : Form
    {
        private const int CpNocloseButton = 0x200;

        public Confirm(Config myConfig)
        {
            InitializeComponent();
            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            ButtonOne.BackColor = BackColor;
            ButtonTwo.BackColor = BackColor;
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

        public DialogResult ShowAsDialog(ConfirmButtons buttons, string caption, string message)
        {
            if (buttons == ConfirmButtons.OkCancel)
            {
                ButtonOne.Text = "OK";
                ButtonTwo.Text = "Cancel";
                ButtonOne.DialogResult = DialogResult.OK;
                ButtonTwo.DialogResult = DialogResult.Cancel;
            }
            else if (buttons == ConfirmButtons.YesNo)
            {
                ButtonOne.Text = "Yes";
                ButtonTwo.Text = "No";
                ButtonOne.DialogResult = DialogResult.Yes;
                ButtonTwo.DialogResult = DialogResult.No;
            }

            Text = caption;
            ConfirmText.Text = message;
            return ShowDialog();
        }

        private void Confirm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOne.PerformClick();
            else if (e.KeyCode == Keys.Escape)
                ButtonTwo.PerformClick();
        }
    }
}