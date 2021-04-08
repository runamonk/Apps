using Apps.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace Apps.Forms
{
    public partial class IconPicker : Form
    {
        public IconPicker(Config myConfig, string fileName)
        {
            InitializeComponent();
            AppsConfig = myConfig;
            Icons.OwnerDraw = true;
            Icons.DrawItem += Icons_DrawItem;
            LoadIcons(fileName);
            SetColors();
        }

        #region Properties
        public int SelectedIconIndex
        {
            get 
            {
                if (Icons.SelectedItems.Count > 0)
                    return Icons.SelectedItems[0].Index;
                else
                    return 0;
            }
        }
        #endregion

        #region Privates
        private readonly Config AppsConfig;
        #endregion

        #region Methods
        private void LoadIcons(string fileName)
        {
            int idx = 0;
            while (true)
            {
                Icon f = Funcs.GetIconEx(fileName, idx);
                
                if (f != null)
                {
                    imageList.Images.Add(f);
                    ListViewItem item = new ListViewItem("", idx);
                    Icons.Items.Add(item);
                }
                else
                    break;
                idx++;
            }

            Icons.Items[0].Selected = true;
            Icons.Select();
        }
        private void MoveButtons()
        {
            ButtonOK.Left = (Width / 2 - ButtonOK.Width - 6);
            ButtonCancel.Left = (ButtonOK.Left + ButtonOK.Width) + 6;
        }
        private void SetColors()
        {
            BackColor = AppsConfig.AppsBackColor;
            ForeColor = AppsConfig.AppsFontColor;
            Icons.BackColor = AppsConfig.AppsBackColor;
            Icons.ForeColor = AppsConfig.AppsFontColor;
            ButtonOK.BackColor = AppsConfig.AppsBackColor;
            ButtonOK.ForeColor = AppsConfig.AppsFontColor;
            ButtonCancel.BackColor = AppsConfig.AppsBackColor;
            ButtonCancel.ForeColor = AppsConfig.AppsFontColor;
        }
        #endregion

        #region Events
        private void Icons_DoubleClick(object sender, EventArgs e)
        {
            if (Icons.SelectedItems.Count > 0)
                ButtonOK.PerformClick();
        }

        private void Icons_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (e.Item.Selected)
            {
                if (Icons.Focused)
                {
                    using (SolidBrush br = new SolidBrush(ControlPaint.Dark(AppsConfig.AppsFontColor, 50)))
                    {
                        e.Graphics.FillRectangle(br, e.Bounds);
                    }
                }
                else if (!Icons.HideSelection)
                {
                     using (SolidBrush br = new SolidBrush(Icons.BackColor))
                    {
                        e.Graphics.FillRectangle(br, e.Bounds);
                    } 
                }
            }
            else
            {
                using (SolidBrush br = new SolidBrush(Icons.BackColor))
                {
                    e.Graphics.FillRectangle(br, e.Bounds);
                }
            }
            Pen pen = new Pen(ControlPaint.Dark(AppsConfig.AppsFontColor, 50));
            e.Graphics.DrawRectangle(pen, e.Bounds);
            e.Graphics.DrawImage(e.Item.ImageList.Images[e.Item.ImageIndex], e.Bounds.X+5, e.Bounds.Y+5, e.Bounds.Width-10, e.Bounds.Height-10); // x y w h
        }
        private void IconPicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else
            if (e.KeyCode == Keys.Escape)
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
            Text = string.Format("Icon {0} of " + imageList.Images.Count.ToString(), (SelectedIconIndex + 1));
        }
        #endregion
    }
}
