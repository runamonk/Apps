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
        
        public IconPicker(List<Icon> IconList)
        {
            InitializeComponent();
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

        private void Icons_SelectedIndexChanged(object sender, EventArgs e)
        {
            Text = string.Format("Icon {0} of " + imageList.Images.Count.ToString(), (SelectedIconIndex + 1));
        }
    }
}
