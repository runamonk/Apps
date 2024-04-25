using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Apps.Controls;
using Icons;
using Utility;

namespace Apps
{
    public enum ButtonType
    {
        App,
        Back,
        Folder,
        FolderLink,
        Menu,
        Pin,
        Separator,
        Url
    }

    public class AppButton : Panel
    {
        public delegate void AppButtonClickedHandler(AppButton button);

        public delegate void AppButtonDropEventhandler(AppButton dropToButton, DragEventArgs e);

        private const string IconFolder = "\uE188";
        private const string IconFolderW7 = "\u25B7";
        private const string IconFolderLink = "\u25CF";
        private readonly Panel _borderPanel = new Panel();
        private readonly Timer _buttonHoldTimer = new Timer();
        private readonly Panel _buttonPanel = new Panel();
        private readonly AppButtonText _buttonText;
        private readonly AppButtonText _folderArrow;
        private readonly Timer _missingIconTimer = new Timer();

        private readonly PictureBox _pBox = new PictureBox();
        private Cursor _dragAndDropCursor;
        private string _fFavIcon;
        private string _fFileIconPath;
        private string _fFileName;
        private string _fUrl;

        public AppButton(Config myConfig, ButtonType buttonType)
        {
            DoubleBuffered = true;
            ButtonType = buttonType;
            _buttonText = new AppButtonText(myConfig);
            _folderArrow = new AppButtonText(myConfig);
            _borderPanel.Parent = this;
            _borderPanel.Dock = DockStyle.Fill;
            _borderPanel.Margin = new Padding(0, 0, 0, 0);
            _buttonPanel.Parent = _borderPanel;
            _buttonPanel.Dock = DockStyle.Fill;
            _buttonPanel.Margin = new Padding(0, 0, 0, 0);
            _buttonPanel.BorderStyle = BorderStyle.None;
            _pBox.Visible = false;
            _folderArrow.Visible = false;
            Height = 22;

            if (IsAppButton || IsUrlButton)
            {
                _pBox.Parent = _buttonPanel;
                _pBox.Dock = DockStyle.Left;
                _pBox.Width = 22;
                _pBox.BorderStyle = BorderStyle.None;
                _pBox.Padding = new Padding(0, 0, 0, 0);
                _pBox.Margin = new Padding(0,  0, 0, 0);
                _pBox.SizeMode = PictureBoxSizeMode.StretchImage;
                if (IsAppButton)
                {
                    _missingIconTimer.Enabled = false;
                    _missingIconTimer.Interval = 10000;
                    _missingIconTimer.Tick += CheckForMissingIcon;
                }
                else if (IsUrlButton)
                {
                    FavIconImage = GetFavIconImage();
                }

                _pBox.Visible = true;
                _pBox.MouseClick += TextOnClick;
            }
            else if (IsFolderButton)
            {
                _folderArrow.Parent = _buttonPanel;
                _folderArrow.AutoSize = false;
                _folderArrow.Dock = DockStyle.Left;
                _folderArrow.Font = new Font("Segoe UI Symbol", 10, FontStyle.Regular);
                _folderArrow.Width = 22;
                _folderArrow.BorderStyle = BorderStyle.None;
                _folderArrow.Padding = new Padding(0, 0, 0, 0);
                _folderArrow.Margin = new Padding(0,  0, 0, 0);
                _folderArrow.Text = Funcs.IsWindows7() ? IconFolderW7 : IconFolder;
                _folderArrow.Visible = true;
                _folderArrow.TextAlign = ContentAlignment.MiddleCenter;
                _folderArrow.MouseClick += TextOnClick;
            }
            else if (IsFolderLinkButton)
            {
                _folderArrow.Parent = _buttonPanel;
                _folderArrow.AutoSize = false;
                _folderArrow.Dock = DockStyle.Left;
                _folderArrow.Font = new Font("Segoe UI Symbol", 8, FontStyle.Regular);
                _folderArrow.Width = 22;
                _folderArrow.BorderStyle = BorderStyle.None;
                _folderArrow.Padding = new Padding(0, 0, 0, 0);
                _folderArrow.Margin = new Padding(0,  0, 0, 0);
                _folderArrow.Text = IconFolderLink;
                _folderArrow.Visible = true;
                _folderArrow.TextAlign = ContentAlignment.MiddleCenter;
                _folderArrow.MouseClick += TextOnClick;
            }
            else if (IsSeparatorButton)
            {
                _buttonText.IsSeparator = true;
                Height = 11;
            }

            _buttonText.AutoSize = false;
            _buttonText.Parent = _buttonPanel;
            _buttonText.Dock = DockStyle.Fill;
            _buttonText.UseCompatibleTextRendering = true;
            _buttonText.UseMnemonic = true;
            _buttonText.AutoEllipsis = true;
            _buttonText.MouseEnter += TextMouseEnter;
            _buttonText.MouseLeave += TextMouseLeave;

            if (IsHeaderButton)
            {
                _buttonText.Padding = new Padding(0, 0, 0, 0);
                _buttonText.TextAlign = ContentAlignment.MiddleCenter;
                _borderPanel.Padding = new Padding(1, 1, 1, 1);
            }
            else
            {
                if (!IsSeparatorButton)
                    _buttonText.Padding = new Padding(25, 0, 0, 0);
                _buttonText.TextAlign = ContentAlignment.MiddleLeft;
                _borderPanel.Padding = new Padding(0, 0, 0, 0);
                _buttonText.AllowDrop = true;
                _buttonText.DragOver += OnDragOver;
                _buttonText.DragDrop += OnDrop;
                _buttonText.MouseDown += TextMouseDown;
                _buttonText.MouseUp += TextMouseUp;
            }

            _buttonText.Margin = new Padding(0, 0, 0, 0);
            _buttonText.MouseClick += TextOnClick;

            AutoSize = false;

            if (IsPinButton)
            {
                ToolTip pinButtonToolTip = new ToolTip();
                pinButtonToolTip.SetToolTip(_buttonText, "Click to pin/unpin form (overrides autohide). [Press P to enable/disable]");
            }
            else if (IsBackButton)
            {
                ToolTip backButtonToolTip = new ToolTip();
                backButtonToolTip.SetToolTip(_buttonText, "Click to go back. [Backspace to go to parent. Control+Backspace/Click to go to the root.]");
            }

            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += ConfigChanged;
            SetColors();
        }

        public ButtonType ButtonType { get; }

        public string AppId { get; set; }

        public string AppName { get => _buttonText.Text; set => _buttonText.Text = value; }

        public string AsAdmin { get; set; }

        public Color BorderColor { get => _borderPanel.BackColor; set => _borderPanel.BackColor = value; }

        public new ContextMenuStrip ContextMenuStrip { get => _buttonText.ContextMenuStrip; set => _buttonText.ContextMenuStrip = value; }

        public string FavIcon
        {
            get => _fFavIcon;
            set
            {
                _fFavIcon = value;
                FavIconImage = GetFavIconImage();
            }
        }

        public Image FavIconImage { set => _pBox.Image = value; get => _pBox.Image; }

        public string FileName
        {
            get => _fFileName;
            set
            {
                _fFileName = value;
                FileIconPath = _fFileName;
            }
        }

        public string FileIconPath
        {
            get => _fFileIconPath;
            set
            {
                _fFileIconPath = value;
                if (!string.IsNullOrEmpty(_fFileIconPath))
                    FileIconImage = IconFuncs.GetIcon(_fFileIconPath, FileIconIndex);
            }
        }

        public string FileIconIndex { get; set; }

        public string FileArgs { get; set; }
        public string FileWorkingFolder { get; set; }

        public Image FileIconImage { set => _pBox.Image = value; get => _pBox.Image; }

        public bool IsAppButton => ButtonType == ButtonType.App;
        public bool IsBackButton => ButtonType == ButtonType.Back;
        public bool IsFolderButton => ButtonType == ButtonType.Folder;
        public bool IsFolderLinkButton => ButtonType == ButtonType.FolderLink;
        public bool IsHeaderButton => IsMenuButton || IsPinButton || IsBackButton;
        public bool IsMenuButton => ButtonType == ButtonType.Menu;
        public bool IsPinButton => ButtonType == ButtonType.Pin;
        public bool IsSeparatorButton => ButtonType == ButtonType.Separator;
        public bool IsUrlButton => ButtonType == ButtonType.Url;

        public string Url
        {
            get => _fUrl;
            set
            {
                _fUrl = value;
                FavIcon = Funcs.GetWebsiteFavIcon(value);
            }
        }

        public XmlNode Node { get; set; }

        public bool WatchForIconUpdate { get => _missingIconTimer.Enabled; set => _missingIconTimer.Enabled = value; }

        private Config AppsConfig { get; }

        protected override bool ShowFocusCues => false;

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            OnAppButtonClicked?.Invoke(this);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _buttonHoldTimer.Interval = 250;
            _buttonHoldTimer.Tick += ButtonHoldTimer_Tick;
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
        {
            base.OnGiveFeedback(gfbevent);
            if (MouseButtons == MouseButtons.Left && Cursor.Current != Cursors.No)
            {
                gfbevent.UseDefaultCursors = false;
                Cursor.Current = _dragAndDropCursor;
            }
        }

        private Image GetFavIconImage()
        {
            if (!string.IsNullOrEmpty(FavIcon))
            {
                MemoryStream s = new MemoryStream(Convert.FromBase64String(FavIcon));
                return new Bitmap(s);
            }

            return null;
        }

        public void ParseShortcut()
        {
            Misc.ParseShortcut(FileName, out string fileName, out string fileIcon, out string fileIconIdx, out string fileArgs, out string fileWf);
            _fFileName = fileName;
            FileIconPath = fileIcon;
            FileIconIndex = fileIconIdx;
            FileWorkingFolder = fileWf;
            FileArgs = fileArgs;
        }

        public void PerformClick() { TextOnClick(this, null); }

        private void SetColors()
        {
            if (IsHeaderButton)
            {
                BorderColor = AppsConfig.HeaderBackColor;
                _buttonPanel.BackColor = AppsConfig.HeaderButtonColor;
                _buttonText.ForeColor = AppsConfig.HeaderFontColor;
                _buttonText.BackColor = AppsConfig.HeaderButtonColor;
            }
            else
            {
                _folderArrow.ForeColor = AppsConfig.AppsFontColor;
                _folderArrow.BackColor = AppsConfig.AppsBackColor;
                _buttonPanel.BackColor = AppsConfig.AppsBackColor;
                _buttonText.ForeColor = AppsConfig.AppsFontColor;
                _buttonText.BackColor = AppsConfig.AppsBackColor;
            }
        }

        private void ButtonHoldTimer_Tick(object sender, EventArgs e)
        {
            _buttonHoldTimer.Stop();
            if (MouseButtons == MouseButtons.Left)
            {
                // copy the button and use it as the cursor while dragging and dropping.
                Bitmap bmpButtonCopy = new Bitmap(Width, Height);
                DrawToBitmap(bmpButtonCopy, new Rectangle(Point.Empty, bmpButtonCopy.Size));

                _dragAndDropCursor = new Cursor(bmpButtonCopy.GetHicon());
                Cursor.Current = _dragAndDropCursor;
                DoDragDrop(this, DragDropEffects.Move);
            }
        }

        private void CheckForMissingIcon(object sender, EventArgs e)
        {
            if (FileIconImage == null)
            {
                if (!string.IsNullOrEmpty(FileIconPath) || !string.IsNullOrEmpty(FileName))
                    if (File.Exists(FileIconPath) || File.Exists(FileName))
                    {
                        if (File.Exists(FileIconPath))
                            FileIconImage = IconFuncs.GetIcon(FileIconPath, FileIconIndex);
                        else
                            FileIconImage = IconFuncs.GetIcon(FileName, FileIconIndex);
                        WatchForIconUpdate = false;
                    }
            }
            else
            {
                _missingIconTimer.Enabled = false;
            }
        }

        private void ConfigChanged(object sender, EventArgs e) { SetColors(); }

        private void OnDragOver(object sender, DragEventArgs e) { e.Effect = DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move; }

        private void OnDrop(object sender, DragEventArgs e) { OnAppButtonDropped?.Invoke(this, e); }

        private void TextMouseDown(object sender, MouseEventArgs e)
        {
            if (IsAppButton | IsFolderButton | IsFolderLinkButton | IsSeparatorButton && e.Button == MouseButtons.Left)
                _buttonHoldTimer.Start();
        }

        private void TextMouseEnter(object sender, EventArgs e)
        {
            if (IsSeparatorButton)
                return;

            if (IsHeaderButton)
                _buttonText.BackColor = AppsConfig.HeaderButtonSelectedColor;
            else
                _buttonText.BackColor = AppsConfig.AppsSelectedBackColor;
        }

        private void TextMouseLeave(object sender, EventArgs e)
        {
            if (IsSeparatorButton)
                return;

            if (IsHeaderButton)
                _buttonText.BackColor = AppsConfig.HeaderButtonColor;
            else
                _buttonText.BackColor = AppsConfig.AppsBackColor;
        }

        private void TextMouseUp(object sender, MouseEventArgs e)
        {
            _buttonHoldTimer.Stop();
            base.OnMouseUp(e);
        }

        private void TextOnClick(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button == MouseButtons.Left)
            {
                if (IsAppButton || IsFolderLinkButton || IsUrlButton)
                    try
                    {
                        ProcessStartInfo procStartInfo;
                        if (IsUrlButton)
                            procStartInfo = new ProcessStartInfo(Url, FileArgs); // Let user specify a specific app to pass urls too?
                        else
                            procStartInfo = new ProcessStartInfo(FileName, FileArgs);

                        if (string.IsNullOrEmpty(FileWorkingFolder))
                            FileWorkingFolder = Path.GetDirectoryName(FileName);

                        procStartInfo.WorkingDirectory = FileWorkingFolder;

                        // run as administrator
                        if (ModifierKeys == Keys.Shift || AsAdmin == "Y")
                            procStartInfo.Verb = "runas";

                        Process process = Process.Start(procStartInfo);
                    }
                    catch (Exception error)
                    {
                        Misc.ShowMessage(AppsConfig, "Error", "Error while launching " + AppName + "\n\r" + error.Message);
                    }

                OnClick(e);
            }
        }

        public event AppButtonClickedHandler OnAppButtonClicked;

        public event AppButtonDropEventhandler OnAppButtonDropped;
    }
}