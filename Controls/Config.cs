using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility;
using Resolve.HotKeys;

namespace Apps
{
    public class Config
    {
        public const string ConfigFilename = "Apps.cfg";
        private List<string> _config;

        public Config()
        {
            _config = new List<string>();
            LoadConfiguration();
        }

        // properties
        public bool AutoHide
        {
            get
            {
                var s = FindKey("auto_hide");
                if (s == "")
                    s = SetKey("auto_hide", "false");
                return bool.Parse(s);
            }
            set => SetKey("auto_hide", value.ToString());
        }

        public bool AutoSizeHeight
        {
            get
            {
                var s = FindKey("auto_size_height");
                if (s == "")
                    s = SetKey("auto_size_height", "false");
                return bool.Parse(s);
            }
            set => SetKey("auto_size_height", value.ToString());
        }

        public Color AppsBackColor
        {
            get
            {
                var s = FindKey("Apps_back_color");
                if (s == "")
                    s = SetKey("Apps_back_color", Color.White.ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set => SetKey("Apps_back_color", value.ToArgb().ToString());
        }

        public Color AppsFontColor
        {
            get
            {
                var s = FindKey("Apps_font_color");
                if (s == "")
                    s = SetKey("Apps_font_color", Color.Black.ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set => SetKey("Apps_font_color", value.ToArgb().ToString());
        }

        public Color AppsHeaderColor
        {
            get
            {
                var s = FindKey("Apps_header_color");
                if (s == "")
                    s = SetKey("Apps_header_color", Color.White.ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set => SetKey("Apps_header_color", value.ToArgb().ToString());
        }

        public int AppsLinesPerRow
        {
            get
            {
                var s = FindKey("Apps_lines_per_row");
                if (s == "")
                    s = SetKey("Apps_lines_per_row", "1");
                return Convert.ToInt32(s);
            }
            set => SetKey("Apps_lines_per_row", value.ToString());
        }

        public Color AppsSelectedBackColor
        {
            get
            {
                var s = FindKey("Apps_row_back_color");
                if (s == "")
                    s = SetKey("Apps_row_back_color", Color.Gray.ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set => SetKey("Apps_row_back_color", value.ToArgb().ToString());
        }

        public int AppsMaxApps
        {
            get
            {
                var s = FindKey("Apps_max_Apps");
                if (s == "")
                    s = SetKey("Apps_max_Apps", "50");
                return Convert.ToInt32(s);
            }
            set => SetKey("Apps_max_Apps", value.ToString());
        }

        public int FormLeft
        {
            get
            {
                var s = FindKey("form_left");
                if (s == "")
                    s = SetKey("form_left", "0", true);
                return Convert.ToInt32(s);
            }
            set => SetKey("form_left", value.ToString(), true);
        }

        public int FormTop
        {
            get
            {
                var s = FindKey("form_top");
                if (s == "")
                    s = SetKey("form_top", "0", true);
                return Convert.ToInt32(s);
            }
            set => SetKey("form_top", value.ToString(), true);
        }

        public Size FormSize
        {
            get
            {
                var s = FindKey("form_size");
                Size sz;
                SizeConverter sc;

                if (s == "")
                {
                    sz = new Size(400, 300);
                    sc = new SizeConverter();
                    s = SetKey("form_size", sc.ConvertToString(sz), true);
                }

                sc = new SizeConverter();
                sz = (Size)sc.ConvertFromString(s);
                return sz;
            }
            set
            {
                var sc = new SizeConverter();
                SetKey("form_size", sc.ConvertToString(value), true);
            }
        }

        public Color HeaderBackColor
        {
            get
            {
                var s = FindKey("header_back_color");
                if (s == "")
                    s = SetKey("header_back_color", Color.White.ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set => SetKey("header_back_color", value.ToArgb().ToString());
        }

        public Color HeaderButtonColor
        {
            get
            {
                var s = FindKey("header_button_color");
                if (s == "")
                    s = SetKey("header_button_color", Color.White.ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set => SetKey("header_button_color", value.ToArgb().ToString());
        }

        public Color HeaderFontColor
        {
            get
            {
                var s = FindKey("header_font_color");
                if (s == "")
                    s = SetKey("header_font_color", Color.Black.ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set => SetKey("header_font_color", value.ToArgb().ToString());
        }

        public Color HeaderButtonSelectedColor
        {
            get
            {
                var s = FindKey("header_button_selected_color");
                if (s == "")
                    s = SetKey("header_button_selected_color", Color.Gray.ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set => SetKey("header_button_selected_color", value.ToArgb().ToString());
        }

        public string IgnoreWindows
        {
            get
            {
                var s = FindKey("ignorewindows");
                if (s == "")
                    s = SetKey("ignorewindows", "");
                return s;
            }
            set => SetKey("ignorewindows", value);
        }

        public Color MenuBackColor
        {
            get
            {
                var s = FindKey("menu_back_color");
                if (s == "")
                    s = SetKey("menu_back_color", Color.White.ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set => SetKey("menu_back_color", value.ToArgb().ToString());
        }

        public Color MenuBorderColor
        {
            get
            {
                var s = FindKey("menu_border_color");
                if (s == "")
                    s = SetKey("menu_border_color", Color.White.ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set => SetKey("menu_border_color", value.ToArgb().ToString());
        }

        public Color MenuButtonColor
        {
            get
            {
                var s = FindKey("menu_button_color");
                if (s == "")
                    s = SetKey("menu_button_color", Color.White.ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set => SetKey("menu_button_color", value.ToArgb().ToString());
        }

        public Color MenuFontColor
        {
            get
            {
                var s = FindKey("menu_font_color");
                if (s == "")
                    s = SetKey("menu_font_color", Color.Black.ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set => SetKey("menu_font_color", value.ToArgb().ToString());
        }

        public Color MenuSelectedColor
        {
            get
            {
                var s = FindKey("menu_selected_color");
                if (s == "")
                    s = SetKey("menu_selected_color", Color.Gray.ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set => SetKey("menu_selected_color", value.ToArgb().ToString());
        }

        public bool OpenFormAtCursor
        {
            get
            {
                var s = FindKey("open_form_at_cursor");
                if (s == "")
                    s = SetKey("open_form_at_cursor", "false");
                return bool.Parse(s);
            }
            set => SetKey("open_form_at_cursor", value.ToString());
        }

        public bool OpenAtRoot
        {
            get
            {
                var s = FindKey("open_at_root");
                if (s == "")
                    s = SetKey("open_at_root", "false");
                return bool.Parse(s);
            }
            set => SetKey("open_at_root", value.ToString());
        }

        public bool ParseShortcuts
        {
            get
            {
                var s = FindKey("parse_shortcuts");
                if (s == "")
                    s = SetKey("parse_shortcuts", "false");
                return bool.Parse(s);
            }
            set => SetKey("parse_shortcuts", value.ToString());
        }

        public Keys PopupHotkey
        {
            get
            {
                var s = FindKey("popup_hotkey");
                if (s == "")
                    s = SetKey("popup_hotkey", "None");
                return Funcs.StringToKey(s);
            }
            set => SetKey("popup_hotkey", value.ToString());
        }

        public Resolve.HotKeys.ModifierKey PopupHotkeyModifier
        {
            /* Modifier
               None = 0,
               Alt = 1,
               Control = 2,
               Shift = 4,
               WinKey = 8*/

            get
            {
                var s = FindKey("popup_hotkey_modifier");
                if (s == "")
                    s = SetKey("popup_hotkey_modifier", "0");
                return (Resolve.HotKeys.ModifierKey)Int32.Parse(s);
            }
            set => SetKey("popup_hotkey_modifier", ((int)value).ToString());
        }

        public event EventHandler ConfigChanged;

        ~Config()
        {
            _config.Clear();
            _config = null;
        }

        private int GetKeyIndex(string key)
        {
            for (var i = 0; i < _config.Count; i++)
                if (_config[i].IndexOf(key) > -1 && _config[i].Substring(0, key.Length) == key)
                    return i;
            return -1;
        }

        private string FindKey(string key)
        {
            foreach (var s in _config)
                if (s.Length > 0 && s.Substring(0, s.IndexOf('=')) == key)
                    return s.Substring(s.IndexOf('=') + 1, s.Length - (s.IndexOf('=') + 1));

            return "";
        }

        private string SetKey(string key, string value, bool saveNow = false)
        {
            var i = GetKeyIndex(key);
            if (i == -1)
                _config.Add(key + "=" + value);
            else
                _config[i] = key + "=" + value;

            if (saveNow) SaveConfiguration();
            return value;
        }

        private void LoadConfiguration()
        {
            try
            {
                if (!File.Exists(Funcs.AppPath(ConfigFilename)))
                {
                    var fs = File.Create(Funcs.AppPath(ConfigFilename));
                    fs.Close();
                }

                _config = File.ReadAllLines(Funcs.AppPath(ConfigFilename), Encoding.ASCII).ToList();
            }
            catch (Exception ee)
            {
                throw new Exception("Cannot LoadConfiguration()" + Environment.NewLine + ee.Message);
            }
        }

        private void SaveConfiguration()
        {
            if (File.Exists(Funcs.AppPath(ConfigFilename)))
                File.Delete(Funcs.AppPath(ConfigFilename));
            File.WriteAllLines(Funcs.AppPath(ConfigFilename), _config.ToArray());
        }

        public void ShowConfigForm(bool parentIsVisible)
        {
            var f = new Settings(this);
            if (!parentIsVisible)
            {
                f.StartPosition = FormStartPosition.Manual;
                Funcs.MoveFormToCursor(f);
            }
            else
            {
                f.StartPosition = FormStartPosition.CenterParent;
            }

            if (f.ShowDialog() == DialogResult.OK)
            {
                SaveConfiguration();
                ConfigChanged(this, null);
            }

            f.Close();
        }

        private class Settings : Apps.Settings
        {
            public Settings()
            {
            }

            public Settings(Config config)
            {
                if (config == null) throw new Exception("Config cannot be null.");

                Config = config;
                OK.Click += ButtonClick;
                Key.Text = Config.PopupHotkey.ToString();

                // fill out the hotkey modifiers
                /* Modifier
                       None = 0,
                       Alt = 1,
                       Control = 2,
                       Shift = 4,
                       WinKey = 8*/

                var m = (int)Config.PopupHotkeyModifier;
                Alt.Checked = m == 1 || m == 3 || m == 5 || m == 9;
                Control.Checked = m == 2 || m == 3 || m == 6 || m == 10;
                Shift.Checked = m == 4 || m == 5 || m == 6 || m == 12;
                Windows.Checked = m == 8 || m == 9 || m == 10 || m == 12;

                AutoHide.Checked = Config.AutoHide;
                Alt.BackColor = Config.AppsBackColor;
                Alt.ForeColor = Config.AppsFontColor;
                Control.BackColor = Config.AppsBackColor;
                Control.ForeColor = Config.AppsFontColor;
                Shift.BackColor = Config.AppsBackColor;
                Shift.ForeColor = Config.AppsFontColor;
                Windows.BackColor = Config.AppsBackColor;
                Windows.ForeColor = Config.AppsFontColor;
                Startup.Checked = Funcs.StartWithWindows;
                Startup.BackColor = Config.AppsBackColor;
                Startup.ForeColor = Config.AppsFontColor;
                OpenAtMouse.Checked = Config.OpenFormAtCursor;
                OpenAtMouse.BackColor = Config.AppsBackColor;
                OpenAtMouse.ForeColor = Config.AppsFontColor;
                ChkParseShortcuts.Checked = Config.ParseShortcuts;
                ChkParseShortcuts.BackColor = Config.AppsBackColor;
                ChkParseShortcuts.ForeColor = Config.AppsFontColor;
                ChkOpenRootFolder.Checked = Config.OpenAtRoot;
                ChkOpenRootFolder.BackColor = Config.AppsBackColor;
                ChkOpenRootFolder.ForeColor = Config.AppsFontColor;
                AutoSizeHeight.Checked = Config.AutoSizeHeight;
                AutoSizeHeight.BackColor = Config.AppsBackColor;
                AutoSizeHeight.ForeColor = Config.AppsFontColor;
                AppBackColor.BackColor = Config.AppsBackColor;
                AppFontColor.BackColor = Config.AppsFontColor;
                AppSelectedColor.BackColor = Config.AppsSelectedBackColor;
                GroupColorHeader.ForeColor = Config.AppsFontColor;
                HeaderBackColor.BackColor = Config.HeaderBackColor;
                HeaderButtonColor.BackColor = Config.HeaderButtonColor;
                HeaderButtonSelectedColor.BackColor = Config.HeaderButtonSelectedColor;
                HeaderFontColor.BackColor = Config.HeaderFontColor;
                MenuBackColor.BackColor = Config.MenuBackColor;
                MenuBorderColor.BackColor = Config.MenuBorderColor;
                MenuFontColor.BackColor = Config.MenuFontColor;
                MenuSelectedColor.BackColor = Config.MenuSelectedColor;
                IgnoreWindows.Text = Config.IgnoreWindows;
                IgnoreWindows.BackColor = Config.AppsBackColor;
                IgnoreWindows.ForeColor = Config.AppsFontColor;
                BackColor = config.AppsBackColor;
                ForeColor = config.AppsFontColor;
                OK.BackColor = BackColor;
                Cancel.BackColor = BackColor;
                Key.BackColor = BackColor;
                Key.ForeColor = ForeColor;
                GroupApps.ForeColor = ForeColor;
                GroupHotkey.ForeColor = ForeColor;
                GroupMenu.ForeColor = ForeColor;
                GroupOptions.ForeColor = ForeColor;
                GroupTheme.ForeColor = ForeColor;
            }

            public Config Config { get; }

            private void ButtonClick(object sender, EventArgs e)
            {
                Config.PopupHotkey = Funcs.StringToKey(Key.Text);
                /* Modifier
                   None = 0,
                   Alt = 1,
                   Control = 2,
                   Shift = 4,
                   WinKey = 8*/
                var i = 0;
                if (Alt.Checked) i++;
                if (Control.Checked) i += 2;
                if (Shift.Checked) i += 4;
                if (Windows.Checked) i += 8;

                Config.AutoHide = AutoHide.Checked;

                Config.AutoSizeHeight = AutoSizeHeight.Checked;
                Config.AppsBackColor = AppBackColor.BackColor;
                Config.AppsFontColor = AppFontColor.BackColor;
                Config.AppsSelectedBackColor = AppSelectedColor.BackColor;
                Config.HeaderBackColor = HeaderBackColor.BackColor;
                Config.HeaderButtonColor = HeaderButtonColor.BackColor;
                Config.HeaderButtonSelectedColor = HeaderButtonSelectedColor.BackColor;
                Config.HeaderFontColor = HeaderFontColor.BackColor;
                Config.IgnoreWindows = IgnoreWindows.Text;
                Config.MenuBackColor = MenuBackColor.BackColor;
                Config.MenuBorderColor = MenuBorderColor.BackColor;
                Config.MenuFontColor = MenuFontColor.BackColor;
                Config.MenuSelectedColor = MenuSelectedColor.BackColor;
                Config.OpenFormAtCursor = OpenAtMouse.Checked;
                Config.PopupHotkeyModifier = (ModifierKey)i;
                Funcs.StartWithWindows = Startup.Checked;
                Config.ParseShortcuts = ChkParseShortcuts.Checked;
                Config.OpenAtRoot = ChkOpenRootFolder.Checked;

                DialogResult = DialogResult.OK;
            }
        }
    }
}