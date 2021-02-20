using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;
using Utility;

namespace Apps
{
    public class Config
    {
        public const string CONFIG_FILENAME = "Apps.cfg";

        private partial class _Settings : Settings
        {
            public _Settings()
            {

            }

            public _Settings(Config Config)
            {
                if (Config == null)
                    throw new Exception("Config cannot be null.");
                else
                {
                    _Config = Config;
                    OK.Click += new EventHandler(ButtonClick);
                    Key.Text = _Config.PopupHotkey;

                    // fill out the hotkey modifiers
                    /* Modifier
                       None = 0,
                       Alt = 1,
                       Control = 2,
                       Shift = 4,
                       WinKey = 8*/

                    int m = _Config.PopupHotkeyModifier;
                    Alt.Checked = (m == 1 || m == 3 || m == 5 || m == 9);
                    Control.Checked = (m == 2 || m == 3 || m == 6 || m == 10);
                    Shift.Checked = (m == 4 || m == 5 || m == 6 || m == 12);
                    Windows.Checked = (m == 8 || m == 9 || m == 10 || m == 12);
                    Startup.Checked = _Config.StartWithWindows;
                    AutoHide.Checked = _Config.AutoHide;
                    AutoSizeHeight.Checked = _Config.AutoSizeHeight;
                    AppBackColor.BackColor = _Config.AppsBackColor;
                    AppFontColor.BackColor = _Config.AppsFontColor;
                    AppHeaderColor.BackColor = _Config.AppsHeaderColor;
                    AppSelectedColor.BackColor = _Config.AppsSelectedBackColor;
                    MenuBackColor.BackColor = _Config.MenuBackColor;
                    MenuBorderColor.BackColor = _Config.MenuBorderColor;
                    MenuButtonColor.BackColor = _Config.MenuButtonColor;
                    MenuFontColor.BackColor = _Config.MenuFontColor;
                    MenuSelectedColor.BackColor = _Config.MenuSelectedColor;
                    OpenAtMouse.Checked = _Config.OpenFormAtCursor;
                }
            }

            private Config _Config;
            public Config Config
            {
                get { return _Config; }
                set { _Config = value; }
            }

            private void ButtonClick(object sender, EventArgs e)
            {
                _Config.PopupHotkey = Key.Text;
                /* Modifier
                   None = 0,
                   Alt = 1,
                   Control = 2,
                   Shift = 4,
                   WinKey = 8*/
                int i = 0;
                if (Alt.Checked) i = (i + 1);
                if (Control.Checked) i = (i + 2);
                if (Shift.Checked) i = (i + 4);
                if (Windows.Checked) i = (i + 8);
                _Config.AutoHide = AutoHide.Checked;
                _Config.AutoSizeHeight = AutoSizeHeight.Checked;
                _Config.AppsBackColor = AppBackColor.BackColor;
                _Config.AppsFontColor = AppFontColor.BackColor;
                _Config.AppsHeaderColor = AppHeaderColor.BackColor;
                _Config.AppsSelectedBackColor = AppSelectedColor.BackColor;
                _Config.MenuBackColor = MenuBackColor.BackColor;
                _Config.MenuBorderColor = MenuBorderColor.BackColor;
                _Config.MenuButtonColor = MenuButtonColor.BackColor;
                _Config.MenuFontColor = MenuFontColor.BackColor;
                _Config.MenuSelectedColor = MenuSelectedColor.BackColor;
                _Config.OpenFormAtCursor = OpenAtMouse.Checked;
                _Config.PopupHotkeyModifier = i;
                _Config.StartWithWindows = Startup.Checked;

                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        public event EventHandler ConfigChanged;
        List<string> _Config;

        public Config()
        {
            _Config = new List<string>();
            LoadConfiguration();
        }
        ~Config()
        {
            _Config.Clear();
            _Config = null;
        }

        private int GetKeyIndex(string Key)
        {
            for (int i = 0; i < _Config.Count; i++)
            {
                if (_Config[i].IndexOf(Key) > -1 && _Config[i].Substring(0, Key.Length) == Key)
                {
                    return i;
                }
            }
            return -1;
        }

        private string FindKey(string Key)
        {
            foreach (String s in _Config)
            {

                if ((s.Length > 0) && (s.Substring(0, s.IndexOf('=')) == Key))
                {
                    return s.Substring((s.IndexOf('=') + 1), (s.Length - (s.IndexOf('=') + 1)));
                }
            }

            return "";
        }

        private string SetKey(string Key, string Value, bool SaveNow = false)
        {
            int i = GetKeyIndex(Key);
            if (i == -1)
                _Config.Add(Key + "=" + Value);
            else
                _Config[i] = Key + "=" + Value;

            if (SaveNow) SaveConfiguration();
            return Value;
        }

        private void LoadConfiguration()
        {
            try
            {
                if (!(File.Exists(Funcs.AppPath(CONFIG_FILENAME))))
                {
                    FileStream fs = File.Create(Funcs.AppPath(CONFIG_FILENAME));
                    fs.Close();
                }

                _Config = File.ReadAllLines(Funcs.AppPath(CONFIG_FILENAME), Encoding.ASCII).ToList();
            }
            catch (Exception ee)
            {
                throw new Exception("Cannot LoadConfiguration()" + System.Environment.NewLine + ee.Message);
            }
        }

        private void SaveConfiguration()
        {
            if (File.Exists(Funcs.AppPath(CONFIG_FILENAME)))
                File.Delete(Funcs.AppPath(CONFIG_FILENAME));
            File.WriteAllLines(Funcs.AppPath(CONFIG_FILENAME), _Config.ToArray());
        }

        public void ShowConfigForm(bool ParentIsVisible)
        {
            _Settings f = new _Settings(this);
            if (!ParentIsVisible)
            {
                f.StartPosition = FormStartPosition.Manual;
                Funcs.MoveFormToCursor(f, false);
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

        // properties
        public Boolean AutoHide
        {
            get {
                string s = FindKey("auto_hide");
                if (s == "")
                    s = SetKey("auto_hide", "false");
                return bool.Parse(s);
            }
            set { SetKey("auto_hide", value.ToString()); }
        }

        public Boolean AutoSizeHeight
        {
            get {
                string s = FindKey("auto_size_height");
                if (s == "")
                    s = SetKey("auto_size_height", "false");
                return bool.Parse(s);
            }
            set { SetKey("auto_size_height", value.ToString()); }
        }

        public Color AppsBackColor
        {
            get {
                string s = FindKey("Apps_back_color");
                if (s == "")
                    s = SetKey("Apps_back_color", Color.FromName("Control").ToArgb().ToString());
                    return Color.FromArgb(Convert.ToInt32(s));
            }
            set { SetKey("Apps_back_color", value.ToArgb().ToString()); }
        }

        public Color AppsFontColor
        {
            get {
                string s = FindKey("Apps_font_color");
                if (s == "")
                    s = SetKey("Apps_font_color", Color.FromName("ControlText").ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set { SetKey("Apps_font_color", value.ToArgb().ToString()); }
        }

        public Color AppsHeaderColor
        {
            get {
                string s = FindKey("Apps_header_color");
                if (s == "")
                    s = SetKey("Apps_header_color", Color.FromName("Control").ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set { SetKey("Apps_header_color", value.ToArgb().ToString()); }
        }

        public int AppsLinesPerRow
        {
            get {
                string s = FindKey("Apps_lines_per_row");
                if (s == "")
                    s = SetKey("Apps_lines_per_row", "1");
                return Convert.ToInt32(s);
            }
            set { SetKey("Apps_lines_per_row", value.ToString()); }
        }

        public Color AppsSelectedBackColor
        {
            get {
                string s = FindKey("Apps_row_back_color");
                if (s == "")
                    s = SetKey("Apps_row_back_color", Color.FromName("Control").ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set { SetKey("Apps_row_back_color", value.ToArgb().ToString()); }
        }

        public int AppsMaxApps
        {
            get {
                string s = FindKey("Apps_max_Apps");
                if (s == "")
                    s = SetKey("Apps_max_Apps", "50");
                return Convert.ToInt32(s);
            }
            set { SetKey("Apps_max_Apps", value.ToString()); }
        }

        public int FormLeft
        {
            get {
                string s = FindKey("form_left");
                if (s == "")
                    s = SetKey("form_left", "0", true);
                return Convert.ToInt32(s);
            }
            set {SetKey("form_left", value.ToString(), true);}
        }

        public int FormTop
        {
            get {
                string s = FindKey("form_top");
                if (s == "")
                    s = SetKey("form_top", "0", true);
                return Convert.ToInt32(s);
            }
            set {SetKey("form_top", value.ToString(), true);}
        }

        public Size FormSize
        {
            get {
                string s = FindKey("form_size");
                Size sz = new Size(400, 300);
                SizeConverter sc = new SizeConverter();

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
            set {
                SizeConverter sc = new SizeConverter();
                SetKey("form_size", sc.ConvertToString(value), true);
            }
        }

        public Color MenuBackColor
        {
            get {
                string s = FindKey("menu_back_color");
                if (s == "")
                    s = SetKey("menu_back_color", Color.FromName("Control").ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set { SetKey("menu_back_color", value.ToArgb().ToString()); }
        }

        public Color MenuBorderColor
        {
            get {
                string s = FindKey("menu_border_color");
                if (s == "")
                    s = SetKey("menu_border_color", Color.FromName("Control").ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set { SetKey("menu_border_color", value.ToArgb().ToString()); }
        }

        public Color MenuButtonColor
        {
            get {
                string s = FindKey("menu_button_color");
                if (s == "")
                    s = SetKey("menu_button_color", Color.FromName("Control").ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set { SetKey("menu_button_color", value.ToArgb().ToString()); }
        }

        public Color MenuFontColor
        {
            get {
                string s = FindKey("menu_font_color");
                if (s == "")
                    s = SetKey("menu_font_color", Color.FromName("ControlText").ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set { SetKey("menu_font_color", value.ToArgb().ToString()); }
        }

        public Color MenuSelectedColor
        {
            get {
                string s = FindKey("menu_selected_color");
                if (s == "")
                    s = SetKey("menu_selected_color", Color.FromName("MenuHighlight").ToArgb().ToString());
                return Color.FromArgb(Convert.ToInt32(s));
            }
            set { SetKey("menu_selected_color", value.ToArgb().ToString()); }
        }

        public Boolean OpenFormAtCursor
        {
            get
            {
                string s = FindKey("open_form_at_cursor");
                if (s == "")
                    s = SetKey("open_form_at_cursor", "false");
                return bool.Parse(s);
            }
            set { SetKey("open_form_at_cursor", value.ToString()); }
        }

        public string PopupHotkey
        {
            get
            {
                string s = FindKey("popup_hotkey");
                if (s == "")
                    s = SetKey("popup_hotkey", "None");
                return s;
            }
            set { SetKey("popup_hotkey", value.ToString()); }
        }

        public int PopupHotkeyModifier
        {
             /* Modifier
                None = 0,
                Alt = 1,
                Control = 2,
                Shift = 4,
                WinKey = 8*/
            
            get
            {
                string s = FindKey("popup_hotkey_modifier");
                if (s == "")
                    s = SetKey("popup_hotkey_modifier", "0");
                return Convert.ToInt32(s);
            }
            set { SetKey("popup_hotkey_modifier", value.ToString()); }        
        }

        public Boolean StartWithWindows
        {
            get {
                RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);
                string subKey = (string)key.GetValue(Application.ProductName, "");
                key.Close();

                if (subKey != "")
                {
                    return true;
                }
                else
                    return false;
            }
            set {
                if (value == false)
                {
                    RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);
                    if (key != null)
                    {
                        key.DeleteValue(Application.ProductName, false);
                        key.Close();
                    }
                }
                else
                {
                    RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);
                    key.SetValue(Application.ProductName, '"' + Application.ExecutablePath.ToString() + '"');
                    key.Close();
                }
            }
        }
    }
}
