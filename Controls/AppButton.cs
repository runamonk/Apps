using Apps.Controls;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Utility;
using System.Xml;

namespace Apps
{

    public partial class AppButton : Panel
    {
        private Config AppsConfig { get; set; }
        private bool IsMenuButton = false;
        private bool IsFolderButton = false;

        private PictureBox PBox = new PictureBox();
        private Panel BorderPanel = new Panel();
        private Panel ButtonPanel = new Panel();
        private Label ButtonText = new Label();

        public new ContextMenuStrip ContextMenuStrip
        {
            get 
            {
                return ButtonText.ContextMenuStrip;
            }
            set 
            {
                ButtonText.ContextMenuStrip = value;
            }
        }

        private new string Text; // get rid of the Text Property.

        public Color BorderColor
        {
            get { return BorderPanel.BackColor; }
            set { BorderPanel.BackColor = value;  }
        }      

        public string AppName
        {
            get { return ButtonText.Text; }
            set { ButtonText.Text = value; }
        }
        private string FFileName;
        
        public string FileName
        {
            get { return FFileName; }
            set 
            {
                FFileName = value;
                PBox.Image = FileIconImage;
            }
        }
        private string FFileIconPath;
        public string FileIconPath
        {
            get
            { return FFileIconPath; }
            set 
            {
                FFileIconPath = value;
            }
        }
        public string FileArgs { get; set; }

        public Image FileIconImage
        {
            set 
            {
                PBox.Image = value;
            }
            get 
            {
                return Funcs.GetIcon((!string.IsNullOrEmpty(FileIconPath) ? FileIconPath : FileName)); 
            }
        }
        
        public XmlNode MyNode { get; set; }

        public delegate void AppButtonClickedHandler(AppButton Button);
        public event AppButtonClickedHandler OnAppButtonClicked;
        
        public AppButton(Config myConfig, bool isMenuButton = false)
        {
            BorderPanel.Parent = this;
            BorderPanel.Dock = DockStyle.Fill;
            BorderPanel.Margin = new Padding(0, 0, 0, 0);
            ButtonPanel.Parent = BorderPanel;
            ButtonPanel.Dock = DockStyle.Fill;
            ButtonPanel.Margin = new Padding(0, 0, 0, 0);
            ButtonPanel.BorderStyle = BorderStyle.None;
            
            if (!isMenuButton)
            {
                PBox.Parent = ButtonPanel;
                PBox.Dock = DockStyle.Left;
                PBox.Width = 23;
                PBox.BorderStyle = BorderStyle.None;
                PBox.Padding = new Padding(3, 0, 0, 0);
                PBox.Margin = new Padding(0, 0, 0, 0);
                PBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
                PBox.Visible = false;
            
            PBox.MouseClick += new MouseEventHandler(TextOnClick);

            ButtonText.AutoSize = false;
            ButtonText.Parent = ButtonPanel;
            ButtonText.Dock = DockStyle.Fill;
            ButtonText.UseCompatibleTextRendering = true;
            ButtonText.UseMnemonic = true;
            ButtonText.MouseEnter += new EventHandler(TextMouseEnter);
            ButtonText.MouseLeave += new EventHandler(TextMouseLeave);

            if (isMenuButton)
                ButtonText.Padding = new Padding(0, 0, 0, 0);
            else
                ButtonText.Padding = new Padding(25, 0, 0, 0);

            ButtonText.Margin = new Padding(0, 0, 0, 0);

            if (isMenuButton)
                ButtonText.TextAlign = ContentAlignment.MiddleCenter;
            else
                ButtonText.TextAlign = ContentAlignment.MiddleLeft;

            ButtonText.MouseClick += new MouseEventHandler(TextOnClick);

            if (isMenuButton)
                BorderPanel.Padding = new Padding(1, 1, 1, 1);
            else
                BorderPanel.Padding = new Padding(0, 0, 0, 0);

            AutoSize = false;
            IsMenuButton = isMenuButton;

            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += new EventHandler(ConfigChanged);
            SetColors();
        }

        private void ConfigChanged(object sender, EventArgs e)
        {
            SetColors();
        }

        private void TextMouseEnter(object sender, EventArgs e)
        {
            if (IsMenuButton)
            {
                ButtonPanel.BackColor = AppsConfig.MenuSelectedColor;
            }
            else
            {
                ButtonText.BackColor = AppsConfig.AppsSelectedBackColor;
            }
        }

        private void TextMouseLeave(object sender, EventArgs e)
        {
            if (IsMenuButton)
            {
                ButtonPanel.BackColor = AppsConfig.MenuButtonColor;
            }
            else
            {
                ButtonText.BackColor = AppsConfig.AppsBackColor;
            }
        }

        private void TextOnClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) 
                OnClick(e);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (OnAppButtonClicked != null)
                OnAppButtonClicked(this);
        }
               
        private void SetColors()
        {
            if (IsMenuButton)
            {
                BorderColor = AppsConfig.MenuBorderColor;
                ButtonPanel.BackColor = AppsConfig.MenuButtonColor;
                ButtonText.ForeColor = AppsConfig.MenuFontColor;
            }
            else
            {
                ButtonPanel.BackColor = AppsConfig.AppsBackColor;
                ButtonText.ForeColor = AppsConfig.AppsFontColor;
            }
        }

        //protected override void OnPaint(PaintEventArgs pea)
        //{
        //    base.OnPaint(pea);

        //    if (!IsMenuButton)
        //    {
        //        // Defines pen 
        //        Pen pen = new Pen(ControlPaint.Dark(AppsConfig.AppsSelectedBackColor, 25));
                               
        //        PointF pt1 = new PointF(0F, Height-1);
        //        PointF pt2 = new PointF(0F, Height);
        //        pea.Graphics.DrawLine(pen, pt1, pt2);
        //    }
        //}

        protected override bool ShowFocusCues
        {
            get {
                return false;
            }
        }

    } 
}
