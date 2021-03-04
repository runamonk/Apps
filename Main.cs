using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Utility;
using System.Diagnostics;
using System.Reflection;
using Apps.Controls;

namespace Apps
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // allow form to be dragged.
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
                
        private AppButton MenuMainButton { get; set; }
        private AppButton BackButton { get; set; }
        private Label SubfolderName { get; set; }
        private AppButton PinButton { get; set; }
        private AppMenu MenuMain { get; set; }
        private Config Config { get; set; }
        private AppPanel Apps { get; set; }

        private bool inAbout = false;
        private bool inClose = false;
        private bool inMenu = false;
        private bool inSettings = false;
        private bool pinned = false;

        private string ICON_PINNED_W7 = "\u25FC";
        private string ICON_UNPINNED_W7 = "\u25FB";
        private string ICON_PINNED = "\uE1F6";
        private string ICON_UNPINNED = "\uE1F7";
        private string ICON_MAINMENU = "\uE0C2";
        private string ICON_MAINMENU_W7 = "\u268A";

        #region Todo
        // seperators
        // link and open a literal folder
        // drag and drop to rearrange
        #endregion

        #region Events
        private void ConfigChanged(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void AppAdded()
        {
            AutoSizeForm(true);
        }

        private void AppClicked()
        {
            ToggleShow(true);
        }

        private void AppDeleted()
        {
            AutoSizeForm(false);
        }

        private void AppsLoaded()
        {
            SuspendLayout();
            SubfolderName.Text = Apps.CurrentFolderName;
            BackButton.Visible = (Apps.InAFolder);
            AutoSizeForm(true);
            ResumeLayout();
        }

        private void Main_Deactivate(object sender, EventArgs e)
        {
            if (Opacity > 0)
                ToggleShow();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            inClose = true;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void Main_ResizeEnd(object sender, EventArgs e)
        {
            Config.FormSize = Size;
            Config.FormTop = Top;
            Config.FormLeft = Left;
        }

        private void MenuAbout_Click(object sender, EventArgs e)
        {
            inAbout = true;
            About AboutForm = new About(Config);
            AboutForm.Show(this);
            inAbout = false;
        }

        private void MenuApps_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            inMenu = false;
        }

        private void MenuApps_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            inMenu = true;
        }

        private void MenuClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MenuSettings_Click(object sender, EventArgs e)
        {
            inSettings = true;
            Config.ShowConfigForm((Opacity > 1));
            inSettings = false;
        }

        private void MainButton_Click(object sender, EventArgs e)
        {
            AppButton b = ((AppButton)sender);
            MenuMain.Show(b.Left + b.Width + Left, b.Top + b.Height + Top);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Apps.GoBack();
        }

        private void PinButton_Click(object sender, EventArgs e)
        {
            AppButton b = ((AppButton)sender);
            if (!pinned)
            {
                pinned = true;
                b.AppName = (Funcs.IsWindows7() ? ICON_PINNED_W7 : ICON_PINNED);
            }
            else
            {
                pinned = false;
                b.AppName = (Funcs.IsWindows7() ? ICON_UNPINNED_W7 : ICON_UNPINNED);
            }
        }

        private void NotifyApps_DoubleClick(object sender, EventArgs e)
        {
            ToggleShow(false);
        }

        private void PTop_MouseDown(object sender, MouseEventArgs e)
        {
            // drag form.
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (RunningInstance() != null)
            {
                Misc.ShowMessage(Config, "", "There is already a version of Apps running.");
                Application.Exit();
            }
            else
                base.OnLoad(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312) //WM_HOTKEY
            {
                ToggleShow();
            }
            base.WndProc(ref m);
        }
        #endregion

        #region Methods
        
        private void AutoSizeForm(bool ScrollToTop)
        {
            if (Apps.InLoad) return;

            if (Config.AutoSizeHeight)
            {
                int c = 66;
                for (int i = 0; i <= Apps.Controls.Count - 1; i++)
                {
                    if (Apps.Controls[i].Visible)
                        c = c + Apps.Controls[i].Height;
                }

                if (c < MaximumSize.Height)
                    Height = c; //+ pMain.Padding.All;
                else
                    Height = MaximumSize.Height;
            }
            // select the first control.
            if (Apps.Controls.Count > 0)
                Apps.Controls[Apps.Controls.Count-1].Select();
        }

        private void LoadConfig()
        {
            if (Config == null)
            {
                Config = new Config();
                Config.ConfigChanged += new EventHandler(ConfigChanged);
                MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width,Screen.PrimaryScreen.WorkingArea.Height);
                MenuMain = new AppMenu(Config);
                MenuMain.Opening += new System.ComponentModel.CancelEventHandler(MenuApps_Opening);
                MenuMain.Closed += new ToolStripDropDownClosedEventHandler(MenuApps_Closed);
                MenuMain.ShowCheckMargin = false;
                MenuMain.ShowImageMargin = false;

                Funcs.AddMenuItem(MenuMain, "About", MenuAbout_Click);
                MenuMain.Items.Add(new ToolStripSeparator());
                Funcs.AddMenuItem(MenuMain, "Settings", MenuSettings_Click);
                Funcs.AddMenuItem(MenuMain, "Close", MenuClose_Click);

                MenuMainButton = new AppButton(Config, ButtonType.Menu)
                {
                    Width = 25,
                    Parent = pTop,
                    Dock = DockStyle.Left
                };
                MenuMainButton.Font = new Font("Segoe UI Symbol", 8, FontStyle.Regular);             
                MenuMainButton.AppName = (Funcs.IsWindows7() ? ICON_MAINMENU_W7 : ICON_MAINMENU);
                MenuMainButton.Click += MainButton_Click;
                MenuMainButton.Padding = new Padding(0,0,0,0);
                MenuMainButton.Margin = new Padding(0,0,0,0);

                BackButton = new AppButton(Config, ButtonType.Back)
                {
                    Width = 25,
                    Parent = pTop,
                    Dock = DockStyle.Left,
                    Visible = false
                };
                BackButton.Font = new Font("Segoe UI Symbol", 8, FontStyle.Regular);
                BackButton.AppName = "\u25C1";
                BackButton.Click += BackButton_Click;
                BackButton.Padding = new Padding(0, 0, 0, 0);
                BackButton.Margin = new Padding(0, 0, 0, 0);
                pTop.Controls.SetChildIndex(BackButton, 0);

                SubfolderName = new Label();
                SubfolderName.AutoSize = false;
                SubfolderName.UseMnemonic = false;
                SubfolderName.AutoEllipsis = true;
                SubfolderName.UseCompatibleTextRendering = true;
                SubfolderName.BorderStyle = BorderStyle.None;
                SubfolderName.TextAlign = ContentAlignment.MiddleCenter;
                SubfolderName.Parent = pTop;
                SubfolderName.Padding = new Padding(0, 3, 0, 3);
                SubfolderName.Margin = new Padding(0, 0, 0, 0);
                pTop.Controls.SetChildIndex(SubfolderName, 0);
                SubfolderName.Dock = DockStyle.Fill;

                PinButton = new AppButton(Config, ButtonType.Pin)
                {
                    Width = 25,
                    Parent = pTop,
                    Dock = DockStyle.Right
                };
                PinButton.Font = new Font("Segoe UI Symbol", 8, FontStyle.Regular);
                PinButton.AppName = (Funcs.IsWindows7() ? ICON_UNPINNED_W7 : ICON_UNPINNED);
                PinButton.Click += PinButton_Click;
                PinButton.Padding = new Padding(0,0,0,0);
                PinButton.Margin = new Padding(0,0,0,0);
                notifyApps.ContextMenuStrip = MenuMain;

                Apps = new AppPanel(Config);
                Apps.AutoScroll = true;
                Apps.OnAppClicked += new AppPanel.AppClickedHandler(AppClicked);
                Apps.OnAppAdded += new AppPanel.AppAddedHandler(AppAdded);
                Apps.OnAppDeleted += new AppPanel.AppDeletedHandler(AppDeleted);
                Apps.OnAppsLoaded += new AppPanel.AppsLoadedHandler(AppsLoaded);
                Apps.Parent = pMain;
                Apps.Dock = DockStyle.Fill;
                SetFormPos();
            }
            Text = Funcs.GetName() + " v" + Funcs.GetVersion();
            pTop.BackColor = Config.AppsHeaderColor;
            BackColor = Config.AppsBackColor;
            SubfolderName.ForeColor = Config.MenuFontColor;
            RegisterHotKey(this.Handle, 1, Config.PopupHotkeyModifier, ((Keys)Enum.Parse(typeof(Keys), Config.PopupHotkey)).GetHashCode());
        }

        private Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            foreach (Process process in processes)
                if (process.Id != current.Id)
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                        return process;
            return null;
        }

        private void SetFormPos()
        {
            Top = Config.FormTop;
            Left = Config.FormLeft;
            Size = Config.FormSize;
        }

        private void ToggleShow(bool Override = false)
        {
            if ((pinned) || (!Override) && (inClose || inAbout || Apps.InMenu || inMenu || inSettings))
                return;
            else
            {
                if (Opacity > 0)
                {
                    Opacity = 0;
                }
                else
                {
                    AutoSizeForm(true);
                    if (Config.OpenFormAtCursor)
                        Funcs.MoveFormToCursor(this, false);
                    Opacity = 100;
                    Activate();
                }
            }
        }

        #endregion

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                ToggleShow(true);
        }
    } // Main
}