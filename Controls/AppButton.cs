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
        public bool IsMenuButton = false;
        public bool IsPinButton = false;
        public bool IsFolderButton = false;

        private PictureBox PBox = new PictureBox();
        private Panel BorderPanel = new Panel();
        private Panel ButtonPanel = new Panel();
        private Label ButtonText = new Label();
        private Label FolderArrow = new Label();
        private Timer MissingIconTimer = new Timer();

        
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
                FileIconImage = Funcs.GetIcon(FFileName);
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
                    FileIconImage = Funcs.GetIcon(FileIconPath); 
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
        public bool WatchForIconUpdate
        {
            get 
            {
                return MissingIconTimer.Enabled;
            }
            set 
            {
                MissingIconTimer.Enabled = value;
            }
        }

        public delegate void AppButtonClickedHandler(AppButton Button);
        public event AppButtonClickedHandler OnAppButtonClicked;

        public delegate void AppButtonDropEventhandler(AppButton Button, DragEventArgs e);
        public event AppButtonDropEventhandler OnAppButtonDropped;

        public AppButton(Config myConfig, bool isMenuButton = false, bool isPinButton = false, bool isFolderButton = false)
        {
            IsMenuButton = isMenuButton;
            IsPinButton = isPinButton;
            IsFolderButton = isFolderButton;

            BorderPanel.Parent = this;
            BorderPanel.Dock = DockStyle.Fill;
            BorderPanel.Margin = new Padding(0, 0, 0, 0);
            ButtonPanel.Parent = BorderPanel;
            ButtonPanel.Dock = DockStyle.Fill;
            ButtonPanel.Margin = new Padding(0, 0, 0, 0);
            ButtonPanel.BorderStyle = BorderStyle.None;

            PBox.Visible = false;
            FolderArrow.Visible = false;

            if (IsFolderButton)
            {
                FolderArrow.Parent = ButtonPanel;
                FolderArrow.AutoSize = false;
                FolderArrow.Dock = DockStyle.Left;
                FolderArrow.Font = new Font("Segoe UI Symbol", 10, FontStyle.Regular);
                FolderArrow.Width = 24;
                FolderArrow.BorderStyle = BorderStyle.None;
                FolderArrow.Padding = new Padding(0, 0, 0, 0);
                FolderArrow.Margin = new Padding(0, 0, 0, 0);                
                FolderArrow.Text = "";
                FolderArrow.Visible = true;
                FolderArrow.TextAlign = ContentAlignment.MiddleCenter;
                FolderArrow.MouseClick += new MouseEventHandler(TextOnClick);
            }
            if (!IsHeaderButton() && !IsFolderButton)
            {
                PBox.Parent = ButtonPanel;
                PBox.Dock = DockStyle.Left;
                PBox.Width = 24;
                PBox.BorderStyle = BorderStyle.None;
                PBox.Padding = new Padding(0, 0, 0, 0);
                PBox.Margin = new Padding(0, 0, 0, 0);
                PBox.SizeMode = PictureBoxSizeMode.StretchImage;
                MissingIconTimer.Enabled = false;
                MissingIconTimer.Interval = 10000;
                MissingIconTimer.Tick += new EventHandler(CheckForMissingIcon);
                PBox.Visible = true;
                PBox.MouseClick += new MouseEventHandler(TextOnClick);
            }                
            
            
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
            {
                ButtonText.Padding = new Padding(25, 0, 0, 0);
                ButtonText.AllowDrop = true;
                ButtonText.DragOver += new DragEventHandler(OnDragOver);
                ButtonText.DragDrop += new DragEventHandler(OnDrop);
            }

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

        private void CheckForMissingIcon(object sender, EventArgs e)
        {
            if (FileIconImage == null)
            {
                if ((!string.IsNullOrEmpty(FileIconPath)) || (!string.IsNullOrEmpty(FileName)))
                {
                    if (File.Exists(FileIconPath) || File.Exists(FileName))
                    {
                        if (File.Exists(FileIconPath))
                            FileIconImage = Funcs.GetIcon(FileIconPath);
                        else
                            FileIconImage = Funcs.GetIcon(FileName);
                        WatchForIconUpdate = false;
                    }                      
                }
            }
            else
            {
                MissingIconTimer.Enabled = false;
            }
        }

        private void ConfigChanged(object sender, EventArgs e)
        {
            SetColors();
        }

        private bool IsHeaderButton()
        {
            return ((IsMenuButton) || (IsPinButton));
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            e.Effect = (DragDropEffects.Copy | DragDropEffects.Link);
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            OnAppButtonDropped?.Invoke(this, e);
        }

        private void TextMouseEnter(object sender, EventArgs e)
        {
            if (IsHeaderButton())
            {
                ButtonText.BackColor = AppsConfig.MenuSelectedColor;
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
                ButtonText.BackColor = AppsConfig.MenuButtonColor;
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
                if (!IsHeaderButton() && !IsFolderButton)
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

            OnAppButtonClicked?.Invoke(this);
        }
               
        private void SetColors()
        {
            if (IsHeaderButton())
            {
                BorderColor = AppsConfig.MenuBorderColor;
                ButtonPanel.BackColor = AppsConfig.MenuButtonColor;
                ButtonText.ForeColor = AppsConfig.MenuFontColor;
                ButtonText.BackColor = AppsConfig.MenuButtonColor;
            }
            else
            {
                FolderArrow.ForeColor = AppsConfig.AppsFontColor;
                FolderArrow.BackColor = AppsConfig.AppsBackColor;
                ButtonPanel.BackColor = AppsConfig.AppsBackColor;
                ButtonText.ForeColor = AppsConfig.AppsFontColor;
                ButtonText.BackColor = AppsConfig.AppsBackColor;
            }
        }

        protected override bool ShowFocusCues
        {
            get {
                return false;
            }
        }
    } 
}
