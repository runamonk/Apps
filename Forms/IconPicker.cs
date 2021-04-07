﻿using Apps.Controls;
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
    public partial class IconPicker : Form
    {
        public IconPicker(Config myConfig, List<Icon> IconList)
        {
            InitializeComponent();
            AppsConfig = myConfig;
            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;
            Icons.BackColor = myConfig.AppsBackColor;
            Icons.ForeColor = myConfig.AppsFontColor;
            ButtonOK.BackColor = myConfig.AppsBackColor;
            ButtonOK.ForeColor = myConfig.AppsFontColor;
            ButtonCancel.BackColor = myConfig.AppsBackColor;
            ButtonCancel.ForeColor = myConfig.AppsFontColor;

            int i = 0;
            while (i < IconList.Count)
            {
                imageList.Images.Add((Icon)IconList[i]);
                ListViewItem item = new ListViewItem(i.ToString(), i);
                Icons.Items.Add(item);
                i++;
            }
            Icons.Items[0].Selected = true;
            Icons.Select();
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

        #region Events
        private void IconPicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ButtonOK.PerformClick();
            else
            if (e.KeyCode == Keys.Escape)
                ButtonCancel.PerformClick();
        }
        private void Icons_SelectedIndexChanged(object sender, EventArgs e)
        {
            Text = string.Format("Icon {0} of " + imageList.Images.Count.ToString(), (SelectedIconIndex + 1));
        }
        #endregion
    }
}
