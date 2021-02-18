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
        

        
        private string BrowseForFile()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Multiselect = false;
            fd.Filter = "All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;

            DialogResult dr = fd.ShowDialog();

            if (dr == DialogResult.OK)
                return fd.FileName;
            else
                return "";           
        }

        private Image GetIcon(string fileName)
        {
            if (File.Exists(fileName))
            {
                string[] ImageTypes = { ".png", ".tif", ".jpg", ".gif", ".bmp", ".ico" };

                if (ImageTypes.Contains(Path.GetExtension(fileName)))
                {
                    return (Image)(Image)(new Bitmap(new Bitmap(fileName, false), AppIcon.Size));
                }
                    
                else
                    return (Image)(new Bitmap(Icon.ExtractAssociatedIcon(fileName).ToBitmap(), AppIcon.Size));
            }
            else
                return null;
        }

        public void SetFileProperties(string appName, string filePath, string fileIcon)
        {
            AppName.Text = appName;
            AppFilePath.Text = filePath;
            AppIcon.Image = GetIcon(fileIcon);
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            string fileName = BrowseForFile();   
            if (fileName != "")
            {
                FileVersionInfo f = FileVersionInfo.GetVersionInfo(fileName);
                SetFileProperties(f.ProductName, f.FileName, f.FileName);
            }
        }

        private void AppIcon_Click(object sender, EventArgs e)
        {
            string fileName = BrowseForFile();
            if (fileName != "")
            {
                AppIcon.Image = GetIcon(fileName);
            }
        }
    }
}
