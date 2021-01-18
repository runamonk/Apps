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

        private About AboutForm = new About();
        private AppButton MenuMainButton { get; set; }
        private AppMenu MenuMain { get; set; }
        private Config Config { get; set; }
        private AppPanel Apps { get; set; }

        private bool inAbout = false;
        private bool inClose = false;
        private bool inMenu = false;
        private bool inSettings = false;

        #region Todo

        #endregion

        #region Events
        private void ConfigChanged(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void ClipAdded(AppButton Clip, bool AppsavedToDisk)
        {
            if (AppsavedToDisk)
                AutoSizeForm(true);
        }

        private void ClipClicked(AppButton Clip)
        {
            if (Config.AutoHide)
                 ToggleShow(true, true);
        }

        private void ClipDeleted()
        {
            AutoSizeForm(false);
        }

        private void AppsLoaded()
        {
            AutoSizeForm(true);
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
            Button b = ((Button)sender);
            MenuMain.Show(b.Left + b.Width + Left, b.Top + b.Height + Top);
        }

        private void NotifyApps_DoubleClick(object sender, EventArgs e)
        {
            ToggleShow(false,false);
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
                MessageBox.Show("There is already a version of Apps running.");
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
                int c = 68;
                for (int i = 0; i <= Apps.Controls.Count - 1; i++)
                {
                    c = c + Apps.Controls[i].Height;
                }

                if (c < MaximumSize.Height)
                    Height = c;
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
                MenuMain = new AppMenu(Config);
                MenuMain.Opening += new System.ComponentModel.CancelEventHandler(MenuApps_Opening);
                MenuMain.Closed += new ToolStripDropDownClosedEventHandler(MenuApps_Closed);
                ToolStripMenuItem t;
                t = new ToolStripMenuItem("&About");
                t.Click += new EventHandler(MenuAbout_Click);
                MenuMain.Items.Add(t);
                MenuMain.Items.Add(new ToolStripSeparator());
                t = new ToolStripMenuItem("&Settings");
                t.Click += new EventHandler(MenuSettings_Click);
                MenuMain.Items.Add(t);
                t = new ToolStripMenuItem("&Close");
                t.Click += new EventHandler(MenuClose_Click);
                MenuMain.Items.Add(t);
                MenuMainButton = new AppButton(Config, true)
                {
                    Text = "...",
                    Width = 25,
                    Parent = pTop,
                    Dock = DockStyle.Left
                };
                MenuMainButton.Click += MainButton_Click;
                MenuMainButton.Padding = new Padding(0,0,0,3);
                MenuMainButton.TextAlign = ContentAlignment.MiddleCenter;
                notifyApps.ContextMenuStrip = MenuMain;

                Apps = new AppPanel(Config);
                Apps.AutoScroll = true;
                Apps.OnClipClicked += new AppPanel.ClipClickedHandler(ClipClicked);
                Apps.OnClipAdded += new AppPanel.ClipAddedHandler(ClipAdded);
                Apps.OnClipDeleted += new AppPanel.ClipDeletedHandler(ClipDeleted);
                Apps.OnAppsLoaded += new AppPanel.AppsLoadedHandler(AppsLoaded);
                Apps.Parent = pMain;
                Apps.Dock = DockStyle.Fill;
                SetFormPos();
            }
            Text = Funcs.GetName() + " v" + Funcs.GetVersion();
            pTop.BackColor = Config.AppsHeaderColor;
            BackColor = Config.AppsBackColor;
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

        private void ToggleShow(bool Override = false, bool IgnoreBounds = true)
        {
            if ((!Override) && (inClose || inAbout || Apps.InMenu || inMenu || inSettings))
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
                        Funcs.MoveFormToCursor(this, IgnoreBounds);
                    Opacity = 100;
                    Activate();
                }
            }
        }

        #endregion

    } // Main
}