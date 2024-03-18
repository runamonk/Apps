using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Icons;
using Utility;

namespace Apps.Forms
{
    public partial class IconPicker : Form
    {
        #region Privates

        private readonly Config _appsConfig;

        #endregion

        public IconPicker(Config myConfig, string fileName)
        {
            InitializeComponent();
            _appsConfig = myConfig;


            Icons.OwnerDraw = true;
            Icons.DrawItem += Icons_DrawItem;

            SelectedFileName = fileName;
            LoadIcons();
            SetColors();
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            //  Hide horizontal  scrollbar.
            ShowScrollBar(Icons.Handle, 0, false);
            base.WndProc(ref m);
        }

        #region Properties

        public string SelectedFileName { get; set; }

        public int SelectedIconIndex
        {
            get
            {
                if (Icons.SelectedItems.Count > 0)
                    return Icons.SelectedItems[0].Index;
                return 0;
            }
        }

        #endregion

        #region Methods

        private void LoadIcons()
        {
            labelFileName.Text = SelectedFileName;
            Icons.BeginUpdate();

            Icons.Items.Clear();
            imageList.Images.Clear();

            var idx = 0;
            while (true)
            {
                var f = IconFuncs.GetIcon(SelectedFileName, idx.ToString());
                if (f != null)
                {
                    imageList.Images.Add(f);
                    var item = new ListViewItem("", idx);
                    Icons.Items.Add(item);
                }
                else
                {
                    break;
                }

                if (!(Path.GetExtension(SelectedFileName) == ".dll" || Path.GetExtension(SelectedFileName) == ".exe"))
                    break;

                idx++;
            }

            if (Icons.Items.Count > 0)
            {
                Icons.EndUpdate();
                Icons.Items[0].Selected = true;
                Icons.Select();
            }
        }

        private void MoveButtons()
        {
            ButtonOK.Left = Width / 2 - (ButtonOK.Width + 10);
            ButtonCancel.Left = ButtonOK.Left + 6 + ButtonOK.Width;
        }

        private void SetColors()
        {
            BackColor = _appsConfig.AppsBackColor;
            ForeColor = _appsConfig.AppsFontColor;
            Browse.ForeColor = _appsConfig.AppsFontColor;
            Browse.BackColor = _appsConfig.AppsBackColor;
            Icons.BackColor = _appsConfig.AppsBackColor;
            Icons.ForeColor = _appsConfig.AppsFontColor;
            ButtonOK.BackColor = _appsConfig.AppsBackColor;
            ButtonOK.ForeColor = _appsConfig.AppsFontColor;
            ButtonCancel.BackColor = _appsConfig.AppsBackColor;
            ButtonCancel.ForeColor = _appsConfig.AppsFontColor;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

        #endregion

        #region Events

        private void Browse_Click(object sender, EventArgs e)
        {
            var fileName = Funcs.BrowseForFile("resources|*.dll;*.exe;*.png;*.tif;*.jpg;*.gif;*.bmp;*.ico");
            if (fileName != "")
            {
                SelectedFileName = fileName;
                LoadIcons();
            }
        }

        private void Icons_DoubleClick(object sender, EventArgs e)
        {
            if (Icons.SelectedItems.Count > 0)
                ButtonOK.PerformClick();
        }

        private void Icons_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if ((e.State & ListViewItemStates.Selected) != 0)
                e.DrawFocusRectangle();

            e.Graphics.DrawImage(e.Item.ImageList.Images[e.Item.ImageIndex], e.Bounds.X + 5, e.Bounds.Y + 5,
                e.Bounds.Width - 10, e.Bounds.Height - 10); // x y w h#
        }

        private void IconPicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else if (e.KeyCode == Keys.Escape)
                ButtonCancel.PerformClick();
        }

        private void IconPicker_Load(object sender, EventArgs e)
        {
            MoveButtons();
        }

        private void IconPicker_Resize(object sender, EventArgs e)
        {
            MoveButtons();
        }

        private void Icons_SelectedIndexChanged(object sender, EventArgs e)
        {
            Text = string.Format("Icon {0} of " + imageList.Images.Count, SelectedIconIndex + 1);
        }

        #endregion
    }
}