using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace Apps.Forms
{
    public partial class AppProperties : Form
    {
        public AppProperties()
        {
            InitializeComponent();
        }

        public void SetFileProperties(string appName, string filePath, string fileIcon)
        {
            AppName.Text = appName;
            AppFilePath.Text = filePath;

            if (File.Exists(fileIcon))
            {        
                AppIcon.Image = (Image)(new Bitmap(Icon.ExtractAssociatedIcon(fileIcon).ToBitmap(), AppIcon.Size));
            }
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Multiselect = false;
            fd.Filter = "All files (*.*)|*.*";
            fd.FilterIndex = 1;
            DialogResult dr = fd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                FileVersionInfo f = FileVersionInfo.GetVersionInfo(fd.FileName);
                SetFileProperties(f.ProductName, f.FileName, f.FileName);
            }
        }
    }
}
