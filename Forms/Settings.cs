﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Apps
{
    public partial class Settings : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle |= CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public Settings()
        {
            InitializeComponent();
        }

        private void FormConfig_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OK.PerformClick();
            else
            if (e.KeyCode == Keys.Escape)
                Cancel.PerformClick();
        }

        private void ColorControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                ((Panel)sender).BackColor = dlgColor.Color;
            }
        }

        private void LightTheme_Click(object sender, EventArgs e)
        {
            DarkTheme.Checked = false;
            AppBackColor.BackColor = Color.White;
            AppFontColor.BackColor = Color.Black;
            AppSelectedColor.BackColor = Color.Gray;
            HeaderBackColor.BackColor = Color.White;
            HeaderButtonColor.BackColor = Color.White;
            HeaderFontColor.BackColor = Color.Black;
            HeaderButtonSelectedColor.BackColor = Color.Gray;
            MenuBackColor.BackColor = Color.White;
            MenuBorderColor.BackColor = Color.White;
            MenuFontColor.BackColor = Color.Black;
            MenuSelectedColor.BackColor = Color.Gray;
        }

        private void DarkTheme_Click(object sender, EventArgs e)
        {
            LightTheme.Checked = false;
            AppBackColor.BackColor = Color.FromArgb(56, 56, 56);
            AppFontColor.BackColor = Color.White;
            AppSelectedColor.BackColor = Color.DarkGray;
            HeaderBackColor.BackColor = Color.FromArgb(56, 56, 56);
            HeaderButtonColor.BackColor = Color.FromArgb(56, 56, 56);
            HeaderFontColor.BackColor = Color.White;
            HeaderButtonSelectedColor.BackColor = Color.DarkGray;
            MenuBackColor.BackColor = Color.FromArgb(56, 56, 56);
            MenuBorderColor.BackColor = Color.FromArgb(56, 56, 56);
            MenuSelectedColor.BackColor = Color.DarkGray;
            MenuFontColor.BackColor = Color.White;
            MenuSelectedColor.BackColor = Color.DarkGray;
        }

        private void Key_KeyDown(object sender, KeyEventArgs e)
        {
            Keys k = (Keys)e.KeyCode;
            Key.Text = k.ToString();
        }

        private void Key_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; // this will stop the key pressed from actually entering into the text box.
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            Key.Clear();
        }
    }
}