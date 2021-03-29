using Apps.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;
using Utility;
using System.Diagnostics;
using System.IO;

namespace Apps
{
    public enum ButtonType
    {
        App,
        Menu,
        Pin,
        Folder,
        FolderLink,
        Back,
        Seperator
    }

    public partial class AppButton : Panel
    {
        private Config AppsConfig { get; set; }
        private ButtonType FButtonType;

        public ButtonType ButtonType { get { return FButtonType; } }
        public bool IsMenuButton { get { return (FButtonType == ButtonType.Menu); } }
        public bool IsPinButton { get { return (FButtonType == ButtonType.Pin); } }
        public bool IsFolderButton { get { return (FButtonType == ButtonType.Folder); } }
        public bool IsBackButton { get { return (FButtonType == ButtonType.Back); } }
        public bool IsFolderLinkButton { get { return (FButtonType == ButtonType.FolderLink); } }
        public bool IsAppButton { get { return (FButtonType == ButtonType.App); } }

        private PictureBox PBox = new PictureBox();
        private Panel BorderPanel = new Panel();
        private Panel ButtonPanel = new Panel();
        private Label ButtonText = new Label();
        private Label FolderArrow = new Label();
        private Timer MissingIconTimer = new Timer();

        private string ICON_FOLDER = "\uE188";
        private string ICON_FOLDER_W7 = "\u25B7";
        private string ICON_FOLDER_LINK = "\u25CF";

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
        public string FileWorkingFolder { get; set; }
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
        public delegate void AppButtonDropEventhandler(AppButton DropToButton, DragEventArgs e);
        public event AppButtonDropEventhandler OnAppButtonDropped;

        public AppButton(Config myConfig, ButtonType buttonType)
        {
            FButtonType = buttonType;
            BorderPanel.Parent = this;
            BorderPanel.Dock = DockStyle.Fill;
            BorderPanel.Margin = new Padding(0, 0, 0, 0);
            ButtonPanel.Parent = BorderPanel;
            ButtonPanel.Dock = DockStyle.Fill;
            ButtonPanel.Margin = new Padding(0, 0, 0, 0);
            ButtonPanel.BorderStyle = BorderStyle.None;
            
            PBox.Visible = false;
            FolderArrow.Visible = false;

            if (IsAppButton)
            {
                PBox.Parent = ButtonPanel;
                PBox.Dock = DockStyle.Left;
                PBox.Width = 22;
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
            else
            if (IsFolderButton)
            {
                FolderArrow.Parent = ButtonPanel;
                FolderArrow.AutoSize = false;
                FolderArrow.Dock = DockStyle.Left;
                FolderArrow.Font = new Font("Segoe UI Symbol", 10, FontStyle.Regular);
                FolderArrow.Width = 22;
                FolderArrow.BorderStyle = BorderStyle.None;
                FolderArrow.Padding = new Padding(0, 0, 0, 0);
                FolderArrow.Margin = new Padding(0, 0, 0, 0);
                FolderArrow.Text = (Funcs.IsWindows7() ? ICON_FOLDER_W7 : ICON_FOLDER);
                FolderArrow.Visible = true;
                FolderArrow.TextAlign = ContentAlignment.MiddleCenter;
                FolderArrow.MouseClick += new MouseEventHandler(TextOnClick);
            }
            else
            if (IsFolderLinkButton)
            {
                FolderArrow.Parent = ButtonPanel;
                FolderArrow.AutoSize = false;
                FolderArrow.Dock = DockStyle.Left;
                FolderArrow.Font = new Font("Segoe UI Symbol", 8, FontStyle.Regular);
                FolderArrow.Width = 22;
                FolderArrow.BorderStyle = BorderStyle.None;
                FolderArrow.Padding = new Padding(0, 0, 0, 0);
                FolderArrow.Margin = new Padding(0, 0, 0, 0);
                FolderArrow.Text = ICON_FOLDER_LINK; 
                FolderArrow.Visible = true;
                FolderArrow.TextAlign = ContentAlignment.MiddleCenter;
                FolderArrow.MouseClick += new MouseEventHandler(TextOnClick);
            }
            
            ButtonText.AutoSize = false;
            ButtonText.Parent = ButtonPanel;
            ButtonText.Dock = DockStyle.Fill;
            ButtonText.UseCompatibleTextRendering = true;
            ButtonText.UseMnemonic = true;
            ButtonText.MouseEnter += new EventHandler(TextMouseEnter);
            ButtonText.MouseLeave += new EventHandler(TextMouseLeave);
            
                        
            if (IsHeaderButton())
            {
                ButtonText.Padding = new Padding(0, 0, 0, 0);
                ButtonText.TextAlign = ContentAlignment.MiddleCenter;
                BorderPanel.Padding = new Padding(1, 1, 1, 1);
            }
            else
            {
                ButtonText.Padding = new Padding(25, 0, 0, 0);
                ButtonText.TextAlign = ContentAlignment.MiddleLeft;
                BorderPanel.Padding = new Padding(0, 0, 0, 0);
                ButtonText.AllowDrop = true;
                ButtonText.DragOver += new DragEventHandler(OnDragOver);
                ButtonText.DragDrop += new DragEventHandler(OnDrop);
                ButtonText.MouseDown += new MouseEventHandler(TextMouseDown);
            }

            ButtonText.Margin = new Padding(0, 0, 0, 0);
            ButtonText.MouseClick += new MouseEventHandler(TextOnClick);

            AutoSize = false;

            if (IsPinButton)
            {
                ToolTip PinButtonToolTip = new ToolTip();
                PinButtonToolTip.SetToolTip(ButtonText, "Click to pin/unpin form (overrides autohide).");
            }
            else
            if (IsBackButton)
            {
                ToolTip BackButtonToolTip = new ToolTip();
                BackButtonToolTip.SetToolTip(ButtonText, "Click to go back. [Control + Click to go all the way back.]");
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
            return (IsMenuButton || IsPinButton || IsBackButton);
        }
        private void OnDragOver(object sender, DragEventArgs e)
        {
            e.Effect = (DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move);
        }
        private void OnDrop(object sender, DragEventArgs e)
        {       
            OnAppButtonDropped?.Invoke(this, e);
        }
        public void PerformClick()
        {
            TextOnClick(this, null);
        }
        private void SetColors()
        {
            if (IsHeaderButton())
            {
                //BorderColor = AppsConfig.MenuBorderColor;
                BorderColor = AppsConfig.MenuButtonColor;
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
        private void TextMouseDown(object sender, MouseEventArgs e)
        {
            if ((e == null) || (e.Button == MouseButtons.Left))
            {
                if (IsAppButton | IsFolderButton | IsFolderLinkButton && (ModifierKeys == Keys.Control))
                {
                    DoDragDrop(this, DragDropEffects.Move);
                }
            }
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
            if ((e == null) || (e.Button == MouseButtons.Left))
            {
                if (IsAppButton || IsFolderLinkButton)
                {
                    try
                    {
                        ProcessStartInfo procStartInfo = new ProcessStartInfo(FileName, FileArgs);
                        if (string.IsNullOrEmpty(FileWorkingFolder))
                            FileWorkingFolder = Path.GetDirectoryName(FileName);
                        procStartInfo.WorkingDirectory = FileWorkingFolder;
                        Process.Start(procStartInfo);
                    }
                    catch (Exception error)
                    {                      
                        Misc.ShowMessage(AppsConfig, "Error", "Error while launching " + AppName + "\n\r" + error.Message.ToString());
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
        protected override bool ShowFocusCues
        {
            get {
                return false;
            }
        }
    } 
}
