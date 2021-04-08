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
        Separator
    }

    public partial class AppButton : Panel
    {
        public AppButton(Config myConfig, ButtonType buttonType)
        {
            FButtonType = buttonType;
            ButtonText = new AppButtonText(myConfig);
            FolderArrow = new AppButtonText(myConfig);
            BorderPanel.Parent = this;
            BorderPanel.Dock = DockStyle.Fill;
            BorderPanel.Margin = new Padding(0, 0, 0, 0);
            ButtonPanel.Parent = BorderPanel;
            ButtonPanel.Dock = DockStyle.Fill;
            ButtonPanel.Margin = new Padding(0, 0, 0, 0);
            ButtonPanel.BorderStyle = BorderStyle.None;
            PBox.Visible = false;
            FolderArrow.Visible = false;
            Height = 22;

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
            else
            if (IsSeparatorButton)
            {
                ButtonText.IsSeparator = true;
                Height = 11;
            }

            ButtonText.AutoSize = false;
            ButtonText.Parent = ButtonPanel;
            ButtonText.Dock = DockStyle.Fill;
            ButtonText.UseCompatibleTextRendering = true;
            ButtonText.UseMnemonic = true;
            ButtonText.MouseEnter += new EventHandler(TextMouseEnter);
            ButtonText.MouseLeave += new EventHandler(TextMouseLeave);

            if (IsHeaderButton)
            {
                ButtonText.Padding = new Padding(0, 0, 0, 0);
                ButtonText.TextAlign = ContentAlignment.MiddleCenter;
                BorderPanel.Padding = new Padding(1, 1, 1, 1);
            }
            else
            {
                if (!IsSeparatorButton)
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
                PinButtonToolTip.SetToolTip(ButtonText, "Click to pin/unpin form (overrides autohide). [Press P to enable/disable]");
            }
            else
            if (IsBackButton)
            {
                ToolTip BackButtonToolTip = new ToolTip();
                BackButtonToolTip.SetToolTip(ButtonText, "Click to go back. [Backspace to go to parent. Control+Backspace/Click to go to the root.]");
            }

            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += new EventHandler(ConfigChanged);
            SetColors();
        }

        #region Properties
        public ButtonType ButtonType { get { return FButtonType; } }
        public string AppId { get; set; }
        public string AppName
        {
            get { return ButtonText.Text; }
            set { ButtonText.Text = value; }
        }
        public Color BorderColor
        {
            get { return BorderPanel.BackColor; }
            set { BorderPanel.BackColor = value; }
        }
        public new ContextMenuStrip ContextMenuStrip
        {
            get {
                return ButtonText.ContextMenuStrip;
            }
            set {
                ButtonText.ContextMenuStrip = value;
            }
        }
        public string FileName
        {
            get { return FFileName; }
            set {
                FFileName = value;
                FileIconImage = Funcs.GetIcon(FFileName,null);
                FFileIconPath = "";
            }
        }
        public string FileIconPath
        {
            get { return FFileIconPath; }
            set {
                FFileIconPath = value;
                if (!string.IsNullOrEmpty(FFileIconPath))
                    FileIconImage = Funcs.GetIcon(FFileIconPath, FFileIconIndex);
            }
        }
        public string FileIconIndex
        {
            get { return FFileIconIndex; }
            set { FFileIconIndex = value;
            }
        }
        public string FileArgs { get; set; }
        public string FileWorkingFolder { get; set; }
        public Image FileIconImage
        {
            set {
                PBox.Image = value;
            }
            get {
                return PBox.Image;
            }
        }
        public bool IsAppButton { get { return (FButtonType == ButtonType.App); } }
        public bool IsBackButton { get { return (FButtonType == ButtonType.Back); } }
        public bool IsFolderButton { get { return (FButtonType == ButtonType.Folder); } }
        public bool IsFolderLinkButton { get { return (FButtonType == ButtonType.FolderLink); } }
        public bool IsHeaderButton { get { return (IsMenuButton || IsPinButton || IsBackButton); } }
        public bool IsMenuButton { get { return (FButtonType == ButtonType.Menu); } }
        public bool IsPinButton { get { return (FButtonType == ButtonType.Pin); } }
        public bool IsSeparatorButton { get { return (FButtonType == ButtonType.Separator); } }
        public bool WatchForIconUpdate
        {
            get {
                return MissingIconTimer.Enabled;
            }
            set {
                MissingIconTimer.Enabled = value;
            }
        }
        #endregion

        #region Privates
        private Config AppsConfig { get; set; }
        private readonly ButtonType FButtonType;
        private string FFileName;
        private string FFileIconPath;
        private string FFileIconIndex;
        private readonly PictureBox PBox = new PictureBox();
        private readonly Panel BorderPanel = new Panel();
        private readonly Panel ButtonPanel = new Panel();
        private readonly AppButtonText ButtonText;
        private readonly AppButtonText FolderArrow;
        private const string ICON_FOLDER = "\uE188";
        private const string ICON_FOLDER_W7 = "\u25B7";
        private const string ICON_FOLDER_LINK = "\u25CF";
        private readonly Timer MissingIconTimer = new Timer();
        #endregion

        #region Events
        public delegate void AppButtonClickedHandler(AppButton Button);
        public event AppButtonClickedHandler OnAppButtonClicked;
        public delegate void AppButtonDropEventhandler(AppButton DropToButton, DragEventArgs e);
        public event AppButtonDropEventhandler OnAppButtonDropped;
        #endregion

        #region Methods
        private void CheckForMissingIcon(object sender, EventArgs e)
        {
            if (FileIconImage == null)
            {
                if ((!string.IsNullOrEmpty(FileIconPath)) || (!string.IsNullOrEmpty(FileName)))
                {
                    if (File.Exists(FileIconPath) || File.Exists(FileName))
                    {
                        if (File.Exists(FileIconPath))
                            FileIconImage = Funcs.GetIcon(FileIconPath, FileIconIndex);
                        else
                            FileIconImage = Funcs.GetIcon(FileName, FileIconIndex);
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
            if (IsHeaderButton)
            {              
                BorderColor = AppsConfig.HeaderBackColor;
                ButtonPanel.BackColor = AppsConfig.HeaderButtonColor;
                ButtonText.ForeColor = AppsConfig.HeaderFontColor;
                ButtonText.BackColor = AppsConfig.HeaderButtonColor;
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
                if (IsAppButton | IsFolderButton | IsFolderLinkButton | IsSeparatorButton  && (ModifierKeys == Keys.Control))
                {
                    DoDragDrop(this, DragDropEffects.Move);
                }
            }
        }
        private void TextMouseEnter(object sender, EventArgs e)
        {
            if (IsSeparatorButton)
                return;

            if (IsHeaderButton)
            {
                ButtonText.BackColor = AppsConfig.HeaderButtonSelectedColor;
            }
            else
            {
                ButtonText.BackColor = AppsConfig.AppsSelectedBackColor;
            }
        }
        private void TextMouseLeave(object sender, EventArgs e)
        {
            if (IsSeparatorButton)
                return;

            if (IsHeaderButton)
            {
                ButtonText.BackColor = AppsConfig.HeaderButtonColor;
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
        #endregion

        #region Overrides
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
        #endregion
    }
}
