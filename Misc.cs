using Apps.Forms;

namespace Apps
{
    class Misc
    {
        public static void ShowMessage(Config myConfig, string Caption, string MessageText)
        {
            Message f = new Message(myConfig);
            f.ShowAsDialog(Caption, MessageText);
        }

        public static System.Windows.Forms.DialogResult ConfirmDialog(Config myConfig, ConfirmButtons Buttons, string Caption, string MessageText)
        {
            Confirm f = new Confirm(myConfig);
            return f.ShowAsDialog(Buttons, Caption, MessageText);
        }
    }
}
