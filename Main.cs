using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Apps.Controls;
using Resolve.HotKeys;
using Utility;
using zuulWindowTracker;

#region Todo

#endregion

namespace Apps
{
    public partial class Main : Form
    {
        private const int WmNclbuttondown = 0xA1;
        private const int HtCaption = 0x2;

        private const string IconPinnedW7 = "\u25FC";
        private const string IconUnpinnedW7 = "\u25FB";
        private const string IconPinned = "\uE1F6";
        private const string IconUnpinned = "\uE1F7";
        private const string IconMainmenu = "\uE0C2";
        private const string IconMainmenuW7 = "\u268A";
        private const string IconBack = "\uE197"; //"\uE08E";
        private const string IconBackW7 = "\u25C1";
        private bool _firstTime = true;

        private HotKey _hotkey1;

        private string[] _ignoreWindowsList;

        private bool _inAbout;
        private bool _inClose;
        private bool _inMenu;
        private bool _inSettings;
        private bool _pinned;
        private WindowTracker _windowTracker;

        public Main() { InitializeComponent(); }

        private AppButton MenuMainButton { get; set; }
        private AppButton BackButton { get; set; }
        private Label SubfolderName { get; set; }
        private AppButton PinButton { get; set; }
        private AppMenu MenuMain { get; set; }
        private Config Config { get; set; }
        private AppPanel Apps { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            if (Funcs.IsRunningDoShow()) Application.Exit();
            base.OnLoad(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312) //WM_HOTKEY
                ToggleShow();
            base.WndProc(ref m);
        }

        private void AutoSizeForm(bool scrollToTop, bool moveTopToCursor = false)
        {
            if (Config.AutoSizeHeight)
            {
                int c = 66;
                for (int i = 0; i <= Apps.Controls.Count - 1; i++) c += Apps.Controls[i].Height;

                if (c < MaximumSize.Height)
                    Height = c;
                else
                    Height = MaximumSize.Height;
            }

            // select the first control.
            if (scrollToTop && Apps.Controls.Count > 0)
                Apps.Controls[Apps.Controls.Count - 1].Select();

            Point p = new Point(Cursor.Position.X, Cursor.Position.Y);
            Rectangle workingArea = Screen.GetWorkingArea(p);

            //Height
            if (Top + Size.Height > workingArea.Bottom) Top -= Top + Size.Height - workingArea.Bottom;
        }

        private void DisableHotkey()
        {
            if (_hotkey1 != null)
            {
                _hotkey1.Unregister();
                _hotkey1.Dispose();
            }
        }

        private void EnableHotkey()
        {
            DisableHotkey();
            _hotkey1 = new HotKey(Config.PopupHotkey, Config.PopupHotkeyModifier);
            _hotkey1.Pressed += (sender, args) => ToggleShow();
            _hotkey1.Register();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private bool InWindowList(string title) { return title != "" && _ignoreWindowsList.Any(s => s.Trim() != "" && title.ToLower().Contains(s.ToLower())); }

        private bool IsVisible() { return Opacity >= 1; }

        private void LoadConfig()
        {
            if (Config == null)
            {
                Config = new Config();
                Config.ConfigChanged += ConfigChanged;
                MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                MenuMain = new AppMenu(Config);
                MenuMain.Opening += MenuApps_Opening;
                MenuMain.Closed += MenuApps_Closed;
                MenuMain.ShowCheckMargin = false;
                MenuMain.ShowImageMargin = false;

                Funcs.AddMenuItem(MenuMain, "About", MenuAbout_Click);
                MenuMain.Items.Add(new ToolStripSeparator());
                Funcs.AddMenuItem(MenuMain, "Settings", MenuSettings_Click);
                Funcs.AddMenuItem(MenuMain, "Close",    MenuClose_Click);

                MenuMainButton = new AppButton(Config, ButtonType.Menu) { Parent = pTop, Dock = DockStyle.Left };
                MenuMainButton.Width = MenuMainButton.Height;
                MenuMainButton.Font = new Font("Segoe UI Symbol", 8, FontStyle.Regular);
                MenuMainButton.AppName = Funcs.IsWindows7() ? IconMainmenuW7 : IconMainmenu;
                MenuMainButton.Click += MainButton_Click;
                MenuMainButton.Padding = new Padding(0, 0, 0, 0);
                MenuMainButton.Margin = new Padding(0,  0, 0, 0);

                BackButton = new AppButton(Config, ButtonType.Back) { Parent = pTop, Dock = DockStyle.Left, Visible = false };
                BackButton.Width = BackButton.Height;
                BackButton.Font = new Font("Segoe UI Symbol", 8, FontStyle.Regular);
                BackButton.AppName = Funcs.IsWindows7() ? IconBackW7 : IconBack;
                BackButton.Click += BackButton_Click;
                BackButton.Padding = new Padding(0, 0, 0, 0);
                BackButton.Margin = new Padding(0,  0, 0, 0);
                pTop.Controls.SetChildIndex(BackButton, 0);


                SubfolderName = new Label
                {
                    AutoSize = false,
                    UseMnemonic = false,
                    AutoEllipsis = true,
                    UseCompatibleTextRendering = true,
                    BorderStyle = BorderStyle.None,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Parent = pTop,
                    Padding = new Padding(0, 3, 0, 3),
                    Margin = new Padding(0,  0, 0, 0)
                };
                pTop.Controls.SetChildIndex(SubfolderName, 0);
                SubfolderName.Dock = DockStyle.Fill;

                PinButton = new AppButton(Config, ButtonType.Pin) { Parent = pTop, Dock = DockStyle.Right };
                PinButton.Width = PinButton.Height;
                PinButton.Font = new Font("Segoe UI Symbol", 8, FontStyle.Regular);
                PinButton.AppName = Funcs.IsWindows7() ? IconUnpinnedW7 : IconUnpinned;
                PinButton.Click += PinButton_Click;
                PinButton.Padding = new Padding(0, 0, 0, 0);
                PinButton.Margin = new Padding(0,  0, 0, 0);
                notifyApps.ContextMenuStrip = MenuMain;

                Apps = new AppPanel(Config);
                Apps.OnAppClicked += AppClicked;
                Apps.OnAppsChanged += AppsChanged;
                Apps.Parent = pMain;
                Apps.Dock = DockStyle.Fill;
                SetFormPos();
            }

            Text = Funcs.GetNameAndVersion();
            if (Config.AutoSizeHeight && IsVisible())
                AutoSizeForm(false);
            pTop.BackColor = Config.HeaderBackColor;
            BackColor = Config.AppsBackColor;
            _ignoreWindowsList = Config.IgnoreWindows.Split(',');
            SubfolderName.ForeColor = Config.MenuFontColor;

            if (Config.PopupHotkey == Keys.None)
                DisableHotkey();
            else
                EnableHotkey();

            MonitorWindowChanges();

            ShowInTaskbar = !Config.AutoHide;

            ToggleShow();
        }

        private void MonitorWindowChanges()
        {
            if (_windowTracker != null) return;
            _windowTracker = new WindowTracker();
            _windowTracker.WindowChanged += OnWindowChanged;
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private void SetFormPos()
        {
            Top = Config.FormTop;
            Left = Config.FormLeft;
            Size = Config.FormSize;
        }

        private void ToggleShow()
        {
            void SetVisible(bool setVis)
            {
                if (!setVis)
                {
                    KeyPreview = false;
                    Opacity = 0;
                }
                else
                {
                    if (Config.OpenFormAtCursor)
                        Funcs.MoveFormToCursor(this);

                    if (Config.OpenAtRoot && Apps.CurrentFolderName != "")
                        Apps.LoadItems();

                    AutoSizeForm(true);
                    Opacity = 100;
                    Activate();
                    KeyPreview = true;
                }
            }

            if (_pinned || _inClose || _inAbout || Apps.InMenu || _inMenu || _inSettings) return;

            if ((_firstTime && Config.AutoHide) || (Config.AutoHide && IsVisible()))
            {
                SetVisible(false);
                _firstTime = false;
            }
            else if ((_firstTime && !Config.AutoHide) || (!_firstTime && !IsVisible()))
            {
                SetVisible(true);
                _firstTime = false;
            }
        }

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private void AppClicked() { ToggleShow(); }

        private void AppsChanged()
        {
            SubfolderName.Text = Apps.CurrentFolderName;
            BackButton.Visible = Apps.InAFolder;
            AutoSizeForm(false, true);
        }

        private void BackButton_Click(object sender, EventArgs e) { Apps.GoBack(); }

        private void ConfigChanged(object sender, EventArgs e) { LoadConfig(); }

        private void Main_Deactivate(object sender, EventArgs e)
        {
            if (!_firstTime && Config.AutoHide && IsVisible() && !_pinned)
                ToggleShow();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            _inClose = true;
            if (_windowTracker == null) return;
            _windowTracker.WindowChanged -= OnWindowChanged;
            _windowTracker = null;
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && IsVisible())
                ToggleShow();
            else if (e.KeyCode == Keys.P)
                PinButton.PerformClick();
            else if (e.KeyCode == Keys.Back && BackButton.Visible)
                Apps.GoBack();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (_firstTime)
                LoadConfig();
        }

        private void Main_ResizeEnd(object sender, EventArgs e)
        {
            Config.FormSize = Size;
            Config.FormTop = Top;
            Config.FormLeft = Left;
        }

        private void MainButton_Click(object sender, EventArgs e)
        {
            AppButton b = (AppButton)sender;
            MenuMain.Show(b.Left + b.Width + Left, b.Top + b.Height + Top);
        }

        private void MenuAbout_Click(object sender, EventArgs e)
        {
            _inAbout = true;
            About aboutForm = new About(Config);
            aboutForm.Show(this);
            _inAbout = false;
        }

        private void MenuApps_Closed(object sender, ToolStripDropDownClosedEventArgs e) { _inMenu = false; }

        private void MenuApps_Opening(object sender, CancelEventArgs e) { _inMenu = true; }

        private void MenuClose_Click(object sender, EventArgs e) { Close(); }

        private void MenuSettings_Click(object sender, EventArgs e)
        {
            _inSettings = true;
            Config.ShowConfigForm(IsVisible());
            _inSettings = false;
        }

        private void notifyApps_MouseDoubleClick(object sender, MouseEventArgs e) { ToggleShow(); }

        private void OnWindowChanged(IntPtr handle)
        {
            if (_inClose || Config.IgnoreWindows == "") return;
            try
            {
                GetWindowThreadProcessId(handle, out uint pid);
                {
                    string t = Process.GetProcessById((int)pid).MainWindowTitle;

                    if (InWindowList(t))
                        DisableHotkey();
                    else
                        EnableHotkey();
                }
            }
            catch
            {
                EnableHotkey();
            }
        }

        private void PinButton_Click(object sender, EventArgs e)
        {
            AppButton b = (AppButton)sender;
            if (!_pinned)
            {
                _pinned = true;
                b.AppName = Funcs.IsWindows7() ? IconPinnedW7 : IconPinned;
            }
            else
            {
                _pinned = false;
                b.AppName = Funcs.IsWindows7() ? IconUnpinnedW7 : IconUnpinned;
            }
        }

        private void PTop_MouseDown(object sender, MouseEventArgs e)
        {
            // drag form.
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
            }
        }
    }
}