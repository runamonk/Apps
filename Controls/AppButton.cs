using Apps.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;
using Utility;
using System.Diagnostics;
using System.IO;

namespace Apps
{

    public partial class AppButton : Panel
    {
        private Config AppsConfig { get; set; }
        private bool IsMenuButton = false;
        private bool IsPinButton = false;

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

        public Color BorderColor
        {
            get { return BorderPanel.BackColor; }
            set { BorderPanel.BackColor = value;  }
        }      
        public string AppId { get; set; }
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
                PBox.Image = Funcs.GetIcon(FFileName);
                FFileIconPath = "";
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
                if (!string.IsNullOrEmpty(FFileIconPath))
                    PBox.Image = Funcs.GetIcon(FileIconPath); 
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
                return PBox.Image; 
            }
        }
                
        public delegate void AppButtonClickedHandler(AppButton Button);
        public event AppButtonClickedHandler OnAppButtonClicked;

        public AppButton(Config myConfig, bool isMenuButton = false, bool isPinButton = false)
        {
            IsMenuButton = isMenuButton;
            IsPinButton = isPinButton;

            BorderPanel.Parent = this;
            BorderPanel.Dock = DockStyle.Fill;
            BorderPanel.Margin = new Padding(0, 0, 0, 0);
            ButtonPanel.Parent = BorderPanel;
            ButtonPanel.Dock = DockStyle.Fill;
            ButtonPanel.Margin = new Padding(0, 0, 0, 0);
            ButtonPanel.BorderStyle = BorderStyle.None;
            
            if (!IsHeaderButton())
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

            if (IsHeaderButton())
                ButtonText.Padding = new Padding(0, 0, 0, 0);
            else
                ButtonText.Padding = new Padding(25, 0, 0, 0);

            ButtonText.Margin = new Padding(0, 0, 0, 0);

            if (IsHeaderButton())
                ButtonText.TextAlign = ContentAlignment.MiddleCenter;
            else
                ButtonText.TextAlign = ContentAlignment.MiddleLeft;

            ButtonText.MouseClick += new MouseEventHandler(TextOnClick);

            if (IsHeaderButton())
                BorderPanel.Padding = new Padding(1, 1, 1, 1);
            else
                BorderPanel.Padding = new Padding(0, 0, 0, 0);

            AutoSize = false;

            if (isPinButton)
            {
                ToolTip PinButtonToolTip = new ToolTip();
                PinButtonToolTip.SetToolTip(ButtonText, "Click to pin/unpin form (overrides autohide).");
            }

            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += new EventHandler(ConfigChanged);
            SetColors();
        }

        private void ConfigChanged(object sender, EventArgs e)
        {
            SetColors();
        }

        private bool IsHeaderButton()
        {
            return ((IsMenuButton) || (IsPinButton));
        }

        private void TextMouseEnter(object sender, EventArgs e)
        {
            if (IsHeaderButton())
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
            if (IsHeaderButton())
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
            {
                if (!IsHeaderButton())
                {
                    try
                    {
                        ProcessStartInfo procStartInfo = new ProcessStartInfo(FileName, FileArgs);
                        procStartInfo.WorkingDirectory = Path.GetDirectoryName(FileName);
                        Process.Start(procStartInfo);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Error while launching " + AppName + "\n\r" + error.Message.ToString());
                    }
                }
                OnClick(e);
            }               
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (OnAppButtonClicked != null)
                OnAppButtonClicked(this);
        }
               
        private void SetColors()
        {
            if (IsHeaderButton())
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
            Refresh();
        }

        protected override bool ShowFocusCues
        {
            get {
                return false;
            }
        }
    } 
}
