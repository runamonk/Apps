namespace Apps
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Key = new System.Windows.Forms.TextBox();
            this.ChkParseShortcuts = new System.Windows.Forms.CheckBox();
            this.AutoSizeHeight = new System.Windows.Forms.CheckBox();
            this.OpenAtMouse = new System.Windows.Forms.CheckBox();
            this.Startup = new System.Windows.Forms.CheckBox();
            this.MenuButtonColor = new System.Windows.Forms.Panel();
            this.MenuSelectedColor = new System.Windows.Forms.Panel();
            this.MenuBorderColor = new System.Windows.Forms.Panel();
            this.MenuFontColor = new System.Windows.Forms.Panel();
            this.MenuBackColor = new System.Windows.Forms.Panel();
            this.AppHeaderColor = new System.Windows.Forms.Panel();
            this.AppSelectedColor = new System.Windows.Forms.Panel();
            this.AppBackColor = new System.Windows.Forms.Panel();
            this.AppFontColor = new System.Windows.Forms.Panel();
            this.dlgColor = new System.Windows.Forms.ColorDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.GroupOptions = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.GroupHotkey = new System.Windows.Forms.GroupBox();
            this.Clear = new System.Windows.Forms.Button();
            this.Windows = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Shift = new System.Windows.Forms.CheckBox();
            this.Alt = new System.Windows.Forms.CheckBox();
            this.Control = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PanelBottom = new System.Windows.Forms.Panel();
            this.Cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.GroupTheme = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.GroupMenu = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.GroupApps = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.DarkTheme = new System.Windows.Forms.RadioButton();
            this.LightTheme = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.GroupOptions.SuspendLayout();
            this.GroupHotkey.SuspendLayout();
            this.PanelBottom.SuspendLayout();
            this.GroupTheme.SuspendLayout();
            this.panel4.SuspendLayout();
            this.GroupMenu.SuspendLayout();
            this.GroupApps.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // Key
            // 
            this.Key.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Key.Location = new System.Drawing.Point(63, 19);
            this.Key.Name = "Key";
            this.Key.Size = new System.Drawing.Size(80, 20);
            this.Key.TabIndex = 0;
            this.toolTip1.SetToolTip(this.Key, "Press key to define as a hotkey.");
            this.Key.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Key_KeyDown);
            this.Key.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Key_KeyPress);
            // 
            // ChkParseShortcuts
            // 
            this.ChkParseShortcuts.AutoSize = true;
            this.ChkParseShortcuts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChkParseShortcuts.Location = new System.Drawing.Point(6, 65);
            this.ChkParseShortcuts.Name = "ChkParseShortcuts";
            this.ChkParseShortcuts.Size = new System.Drawing.Size(117, 17);
            this.ChkParseShortcuts.TabIndex = 3;
            this.ChkParseShortcuts.Text = "Parse .lnk && .url files";
            this.toolTip1.SetToolTip(this.ChkParseShortcuts, "Check to automatically parse .lnk and .url files when adding a new application.");
            this.ChkParseShortcuts.UseVisualStyleBackColor = true;
            // 
            // AutoSizeHeight
            // 
            this.AutoSizeHeight.AutoSize = true;
            this.AutoSizeHeight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AutoSizeHeight.Location = new System.Drawing.Point(6, 48);
            this.AutoSizeHeight.Name = "AutoSizeHeight";
            this.AutoSizeHeight.Size = new System.Drawing.Size(98, 17);
            this.AutoSizeHeight.TabIndex = 2;
            this.AutoSizeHeight.Text = "Auto size height";
            this.toolTip1.SetToolTip(this.AutoSizeHeight, "Check to automatically size Apps height to the number of Apps.");
            this.AutoSizeHeight.UseVisualStyleBackColor = true;
            // 
            // OpenAtMouse
            // 
            this.OpenAtMouse.AutoSize = true;
            this.OpenAtMouse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenAtMouse.Location = new System.Drawing.Point(6, 31);
            this.OpenAtMouse.Name = "OpenAtMouse";
            this.OpenAtMouse.Size = new System.Drawing.Size(95, 17);
            this.OpenAtMouse.TabIndex = 1;
            this.OpenAtMouse.Text = "Open at mouse";
            this.toolTip1.SetToolTip(this.OpenAtMouse, "Check to automatically display Apps at mouse.");
            this.OpenAtMouse.UseVisualStyleBackColor = true;
            // 
            // Startup
            // 
            this.Startup.AutoSize = true;
            this.Startup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Startup.Location = new System.Drawing.Point(6, 14);
            this.Startup.Name = "Startup";
            this.Startup.Size = new System.Drawing.Size(109, 17);
            this.Startup.TabIndex = 0;
            this.Startup.Text = "Start automatically";
            this.toolTip1.SetToolTip(this.Startup, "Check to automatically start Apps.");
            this.Startup.UseVisualStyleBackColor = true;
            // 
            // MenuButtonColor
            // 
            this.MenuButtonColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MenuButtonColor.Location = new System.Drawing.Point(119, 18);
            this.MenuButtonColor.Name = "MenuButtonColor";
            this.MenuButtonColor.Size = new System.Drawing.Size(26, 27);
            this.MenuButtonColor.TabIndex = 20;
            this.toolTip1.SetToolTip(this.MenuButtonColor, "Click to set color.");
            this.MenuButtonColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ColorControl_MouseClick);
            // 
            // MenuSelectedColor
            // 
            this.MenuSelectedColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MenuSelectedColor.Location = new System.Drawing.Point(6, 117);
            this.MenuSelectedColor.Name = "MenuSelectedColor";
            this.MenuSelectedColor.Size = new System.Drawing.Size(26, 27);
            this.MenuSelectedColor.TabIndex = 3;
            this.toolTip1.SetToolTip(this.MenuSelectedColor, "Click to set color.");
            this.MenuSelectedColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ColorControl_MouseClick);
            // 
            // MenuBorderColor
            // 
            this.MenuBorderColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MenuBorderColor.Location = new System.Drawing.Point(6, 84);
            this.MenuBorderColor.Name = "MenuBorderColor";
            this.MenuBorderColor.Size = new System.Drawing.Size(26, 27);
            this.MenuBorderColor.TabIndex = 2;
            this.toolTip1.SetToolTip(this.MenuBorderColor, "Click to set color.");
            this.MenuBorderColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ColorControl_MouseClick);
            // 
            // MenuFontColor
            // 
            this.MenuFontColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MenuFontColor.Location = new System.Drawing.Point(6, 18);
            this.MenuFontColor.Name = "MenuFontColor";
            this.MenuFontColor.Size = new System.Drawing.Size(26, 27);
            this.MenuFontColor.TabIndex = 0;
            this.toolTip1.SetToolTip(this.MenuFontColor, "Click to set color.");
            this.MenuFontColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ColorControl_MouseClick);
            // 
            // MenuBackColor
            // 
            this.MenuBackColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MenuBackColor.Location = new System.Drawing.Point(6, 51);
            this.MenuBackColor.Name = "MenuBackColor";
            this.MenuBackColor.Size = new System.Drawing.Size(26, 27);
            this.MenuBackColor.TabIndex = 1;
            this.toolTip1.SetToolTip(this.MenuBackColor, "Click to set color.");
            this.MenuBackColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ColorControl_MouseClick);
            // 
            // AppHeaderColor
            // 
            this.AppHeaderColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AppHeaderColor.Location = new System.Drawing.Point(10, 18);
            this.AppHeaderColor.Name = "AppHeaderColor";
            this.AppHeaderColor.Size = new System.Drawing.Size(26, 27);
            this.AppHeaderColor.TabIndex = 0;
            this.toolTip1.SetToolTip(this.AppHeaderColor, "Click to set color.");
            this.AppHeaderColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ColorControl_MouseClick);
            // 
            // AppSelectedColor
            // 
            this.AppSelectedColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AppSelectedColor.Location = new System.Drawing.Point(10, 117);
            this.AppSelectedColor.Name = "AppSelectedColor";
            this.AppSelectedColor.Size = new System.Drawing.Size(26, 27);
            this.AppSelectedColor.TabIndex = 3;
            this.toolTip1.SetToolTip(this.AppSelectedColor, "Click to set color.");
            this.AppSelectedColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ColorControl_MouseClick);
            // 
            // AppBackColor
            // 
            this.AppBackColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AppBackColor.Location = new System.Drawing.Point(10, 84);
            this.AppBackColor.Name = "AppBackColor";
            this.AppBackColor.Size = new System.Drawing.Size(26, 27);
            this.AppBackColor.TabIndex = 2;
            this.toolTip1.SetToolTip(this.AppBackColor, "Click to set color.");
            this.AppBackColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ColorControl_MouseClick);
            // 
            // AppFontColor
            // 
            this.AppFontColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AppFontColor.Location = new System.Drawing.Point(10, 51);
            this.AppFontColor.Name = "AppFontColor";
            this.AppFontColor.Size = new System.Drawing.Size(26, 27);
            this.AppFontColor.TabIndex = 1;
            this.toolTip1.SetToolTip(this.AppFontColor, "Click to set color.");
            this.AppFontColor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ColorControl_MouseClick);
            // 
            // dlgColor
            // 
            this.dlgColor.AnyColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.GroupOptions);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.GroupHotkey);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(418, 87);
            this.panel1.TabIndex = 6;
            // 
            // GroupOptions
            // 
            this.GroupOptions.Controls.Add(this.ChkParseShortcuts);
            this.GroupOptions.Controls.Add(this.AutoSizeHeight);
            this.GroupOptions.Controls.Add(this.OpenAtMouse);
            this.GroupOptions.Controls.Add(this.Startup);
            this.GroupOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupOptions.Location = new System.Drawing.Point(287, 0);
            this.GroupOptions.Margin = new System.Windows.Forms.Padding(0);
            this.GroupOptions.Name = "GroupOptions";
            this.GroupOptions.Size = new System.Drawing.Size(131, 87);
            this.GroupOptions.TabIndex = 12;
            this.GroupOptions.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(282, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(5, 87);
            this.panel3.TabIndex = 11;
            // 
            // GroupHotkey
            // 
            this.GroupHotkey.Controls.Add(this.Clear);
            this.GroupHotkey.Controls.Add(this.Key);
            this.GroupHotkey.Controls.Add(this.Windows);
            this.GroupHotkey.Controls.Add(this.label2);
            this.GroupHotkey.Controls.Add(this.Shift);
            this.GroupHotkey.Controls.Add(this.Alt);
            this.GroupHotkey.Controls.Add(this.Control);
            this.GroupHotkey.Controls.Add(this.label1);
            this.GroupHotkey.Dock = System.Windows.Forms.DockStyle.Left;
            this.GroupHotkey.Location = new System.Drawing.Point(0, 0);
            this.GroupHotkey.Name = "GroupHotkey";
            this.GroupHotkey.Size = new System.Drawing.Size(282, 87);
            this.GroupHotkey.TabIndex = 6;
            this.GroupHotkey.TabStop = false;
            this.GroupHotkey.Text = "Popup Hotkey";
            // 
            // Clear
            // 
            this.Clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Clear.Location = new System.Drawing.Point(156, 19);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(46, 20);
            this.Clear.TabIndex = 7;
            this.Clear.Text = "Clear";
            this.Clear.UseCompatibleTextRendering = true;
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // Windows
            // 
            this.Windows.AutoSize = true;
            this.Windows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Windows.Location = new System.Drawing.Point(209, 45);
            this.Windows.Name = "Windows";
            this.Windows.Size = new System.Drawing.Size(67, 17);
            this.Windows.TabIndex = 4;
            this.Windows.Text = "Windows";
            this.Windows.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 45);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Modifier";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Shift
            // 
            this.Shift.AutoSize = true;
            this.Shift.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Shift.Location = new System.Drawing.Point(162, 45);
            this.Shift.Name = "Shift";
            this.Shift.Size = new System.Drawing.Size(44, 17);
            this.Shift.TabIndex = 3;
            this.Shift.Text = "Shift";
            this.Shift.UseVisualStyleBackColor = true;
            // 
            // Alt
            // 
            this.Alt.AutoSize = true;
            this.Alt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Alt.Location = new System.Drawing.Point(123, 45);
            this.Alt.Name = "Alt";
            this.Alt.Size = new System.Drawing.Size(35, 17);
            this.Alt.TabIndex = 2;
            this.Alt.Text = "Alt";
            this.Alt.UseVisualStyleBackColor = true;
            // 
            // Control
            // 
            this.Control.AutoSize = true;
            this.Control.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Control.Location = new System.Drawing.Point(63, 45);
            this.Control.Name = "Control";
            this.Control.Size = new System.Drawing.Size(56, 17);
            this.Control.TabIndex = 1;
            this.Control.Text = "Control";
            this.Control.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Key";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PanelBottom
            // 
            this.PanelBottom.Controls.Add(this.Cancel);
            this.PanelBottom.Controls.Add(this.OK);
            this.PanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelBottom.Location = new System.Drawing.Point(5, 312);
            this.PanelBottom.Name = "PanelBottom";
            this.PanelBottom.Size = new System.Drawing.Size(418, 38);
            this.PanelBottom.TabIndex = 8;
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel.Location = new System.Drawing.Point(213, 7);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 25);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OK.Location = new System.Drawing.Point(130, 7);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 25);
            this.OK.TabIndex = 2;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            // 
            // GroupTheme
            // 
            this.GroupTheme.Controls.Add(this.panel4);
            this.GroupTheme.Controls.Add(this.panel5);
            this.GroupTheme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupTheme.Location = new System.Drawing.Point(5, 92);
            this.GroupTheme.Name = "GroupTheme";
            this.GroupTheme.Size = new System.Drawing.Size(418, 220);
            this.GroupTheme.TabIndex = 9;
            this.GroupTheme.TabStop = false;
            this.GroupTheme.Text = "Colors";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.GroupMenu);
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Controls.Add(this.GroupApps);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 49);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(412, 168);
            this.panel4.TabIndex = 1;
            // 
            // GroupMenu
            // 
            this.GroupMenu.Controls.Add(this.label15);
            this.GroupMenu.Controls.Add(this.MenuButtonColor);
            this.GroupMenu.Controls.Add(this.label13);
            this.GroupMenu.Controls.Add(this.MenuSelectedColor);
            this.GroupMenu.Controls.Add(this.label12);
            this.GroupMenu.Controls.Add(this.MenuBorderColor);
            this.GroupMenu.Controls.Add(this.label11);
            this.GroupMenu.Controls.Add(this.MenuFontColor);
            this.GroupMenu.Controls.Add(this.label10);
            this.GroupMenu.Controls.Add(this.MenuBackColor);
            this.GroupMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupMenu.Location = new System.Drawing.Point(209, 0);
            this.GroupMenu.Name = "GroupMenu";
            this.GroupMenu.Size = new System.Drawing.Size(203, 168);
            this.GroupMenu.TabIndex = 24;
            this.GroupMenu.TabStop = false;
            this.GroupMenu.Text = "Menu";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(151, 25);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(38, 13);
            this.label15.TabIndex = 21;
            this.label15.Text = "Button";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(38, 124);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Selected";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(38, 91);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Border";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(38, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(28, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "Font";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(38, 58);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Background";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(203, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(6, 168);
            this.panel2.TabIndex = 23;
            // 
            // GroupApps
            // 
            this.GroupApps.Controls.Add(this.label9);
            this.GroupApps.Controls.Add(this.AppHeaderColor);
            this.GroupApps.Controls.Add(this.label8);
            this.GroupApps.Controls.Add(this.AppSelectedColor);
            this.GroupApps.Controls.Add(this.label3);
            this.GroupApps.Controls.Add(this.label5);
            this.GroupApps.Controls.Add(this.AppBackColor);
            this.GroupApps.Controls.Add(this.AppFontColor);
            this.GroupApps.Dock = System.Windows.Forms.DockStyle.Left;
            this.GroupApps.Location = new System.Drawing.Point(0, 0);
            this.GroupApps.Name = "GroupApps";
            this.GroupApps.Size = new System.Drawing.Size(203, 168);
            this.GroupApps.TabIndex = 0;
            this.GroupApps.TabStop = false;
            this.GroupApps.Text = "Apps";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(42, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Header";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(42, 124);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Selected";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Background";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Font";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label14);
            this.panel5.Controls.Add(this.DarkTheme);
            this.panel5.Controls.Add(this.LightTheme);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(3, 16);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(412, 33);
            this.panel5.TabIndex = 0;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(68, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Preset colors";
            // 
            // DarkTheme
            // 
            this.DarkTheme.AutoSize = true;
            this.DarkTheme.Location = new System.Drawing.Point(143, 7);
            this.DarkTheme.Name = "DarkTheme";
            this.DarkTheme.Size = new System.Drawing.Size(48, 17);
            this.DarkTheme.TabIndex = 1;
            this.DarkTheme.TabStop = true;
            this.DarkTheme.Text = "Dark";
            this.DarkTheme.UseVisualStyleBackColor = true;
            this.DarkTheme.Click += new System.EventHandler(this.DarkTheme_Click);
            // 
            // LightTheme
            // 
            this.LightTheme.AutoSize = true;
            this.LightTheme.Location = new System.Drawing.Point(88, 7);
            this.LightTheme.Name = "LightTheme";
            this.LightTheme.Size = new System.Drawing.Size(48, 17);
            this.LightTheme.TabIndex = 0;
            this.LightTheme.TabStop = true;
            this.LightTheme.Text = "Light";
            this.LightTheme.UseVisualStyleBackColor = true;
            this.LightTheme.Click += new System.EventHandler(this.LightTheme_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 355);
            this.Controls.Add(this.GroupTheme);
            this.Controls.Add(this.PanelBottom);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Apps Configuration";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormConfig_KeyDown);
            this.panel1.ResumeLayout(false);
            this.GroupOptions.ResumeLayout(false);
            this.GroupOptions.PerformLayout();
            this.GroupHotkey.ResumeLayout(false);
            this.GroupHotkey.PerformLayout();
            this.PanelBottom.ResumeLayout(false);
            this.GroupTheme.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.GroupMenu.ResumeLayout(false);
            this.GroupMenu.PerformLayout();
            this.GroupApps.ResumeLayout(false);
            this.GroupApps.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ColorDialog dlgColor;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.GroupBox GroupOptions;
        public System.Windows.Forms.CheckBox ChkParseShortcuts;
        public System.Windows.Forms.CheckBox AutoSizeHeight;
        public System.Windows.Forms.CheckBox OpenAtMouse;
        public System.Windows.Forms.CheckBox Startup;
        private System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.GroupBox GroupHotkey;
        private System.Windows.Forms.Button Clear;
        public System.Windows.Forms.TextBox Key;
        public System.Windows.Forms.CheckBox Windows;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.CheckBox Shift;
        public System.Windows.Forms.CheckBox Alt;
        public System.Windows.Forms.CheckBox Control;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel PanelBottom;
        public System.Windows.Forms.Button Cancel;
        public System.Windows.Forms.Button OK;
        public System.Windows.Forms.GroupBox GroupTheme;
        private System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.GroupBox GroupMenu;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.Panel MenuButtonColor;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.Panel MenuSelectedColor;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.Panel MenuBorderColor;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.Panel MenuFontColor;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.Panel MenuBackColor;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.GroupBox GroupApps;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.Panel AppHeaderColor;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.Panel AppSelectedColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Panel AppBackColor;
        public System.Windows.Forms.Panel AppFontColor;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RadioButton DarkTheme;
        private System.Windows.Forms.RadioButton LightTheme;
    }
}