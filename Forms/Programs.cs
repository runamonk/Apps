using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Icons;
using Utility;

namespace Apps.Forms
{
    public partial class Programs : Form
    {
        private const int CpNocloseButton = 0x200;
        private readonly Config _appsConfig;
        private readonly Timer _buttonHoldTimer = new Timer();

        private readonly List<InstalledApp> _installedApps = new List<InstalledApp>();
        //private Cursor _dragAndDropCursor;

        public Programs(Config myConfig, Form parentForm)
        {
            InitializeComponent();
            _appsConfig = myConfig;
            listPrograms.BackColor = myConfig.AppsBackColor;
            listPrograms.ForeColor = myConfig.AppsFontColor;
            listPrograms.HeaderStyle = ColumnHeaderStyle.None;

            panelSearch.BackColor = myConfig.AppsBackColor;
            panelButtons.BackColor = myConfig.AppsBackColor;
            panelPrograms.BackColor = myConfig.AppsBackColor;
            panelPrograms.ForeColor = myConfig.AppsFontColor;
            searchLabel.BackColor = myConfig.AppsBackColor;
            searchLabel.ForeColor = myConfig.AppsFontColor;
            ButtonOK.BackColor = myConfig.AppsBackColor;
            ButtonOK.ForeColor = myConfig.AppsFontColor;
            search.BackColor = myConfig.AppsBackColor;
            search.ForeColor = myConfig.AppsFontColor;
            BackColor = myConfig.AppsBackColor;
            ForeColor = myConfig.AppsFontColor;

            Left = parentForm.Left - Width;
            Top = parentForm.Top;

            _buttonHoldTimer.Interval = 250;
            _buttonHoldTimer.Tick += ButtonHoldTimer_Tick;

            Funcs.WaitThenDo(10, GetPrograms);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle |= CpNocloseButton;
                return myCp;
            }
        }

        //protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
        //{
        //    base.OnGiveFeedback(gfbevent);
        //    if (MouseButtons == MouseButtons.Left && Cursor.Current != Cursors.No)
        //    {
        //        gfbevent.UseDefaultCursors = false;
        //        Cursor.Current = _dragAndDropCursor;
        //    }
        //}

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape) Close();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            listPrograms.Columns[0].Width = Width - 50;
        }

        private void AddFilesToListView()
        {
            listPrograms.BeginUpdate();
            listPrograms.Items.Clear();
            imageList.Images.Clear();

            foreach (InstalledApp f in _installedApps.Where(f => { return Path.GetFileName(f.Caption).ToLower().Contains(search.Text.ToLower().Trim()) || search.Text.Trim() == ""; }))
                if (f.Icon != null)
                {
                    imageList.Images.Add(f.Icon);
                    ListViewItem i = new ListViewItem(f.Caption, imageList.Images.Count - 1);
                    listPrograms.Items.Add(i).Tag = f;
                }
                else
                {
                    listPrograms.Items.Add(new ListViewItem(f.Caption)).Tag = f;
                }

            listPrograms.EndUpdate();
        }

        private void GetPrograms()
        {
            string[] shortcuts;

            _installedApps.Clear();

            string userAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            shortcuts = Funcs.GetFiles("C:\\ProgramData\\Microsoft\\Windows\\Start Menu", "lnk,url");
            // remove duplicates inbetween each parse aka Where() and compare filenames.

            shortcuts = shortcuts.Concat(Funcs.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu", "lnk,url")
                                              .Where(f => { return !shortcuts.Any(a => { return Path.GetFileName(a) == Path.GetFileName(f); }); }))
                                 .ToArray();
            shortcuts = shortcuts.Concat(Funcs.GetFiles("C:\\Users\\Default\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu", "lnk,url")
                                              .Where(f => { return !shortcuts.Any(a => { return Path.GetFileName(a) == Path.GetFileName(f); }); }))
                                 .ToArray();
            shortcuts = shortcuts.Where(f => { return f.ToLower().IndexOf("uninstall") == -1; }).ToArray();

            foreach (string s in shortcuts)
            {
                InstalledApp app = new InstalledApp(s);
                if (File.Exists(app.FileName) || Funcs.IsUrl(app.FileName)) _installedApps.Add(app); // skip orphaned shortcuts/apps
            }

            _installedApps.Sort((file1, file2) => { return string.Compare(file1.Caption, file2.Caption, StringComparison.OrdinalIgnoreCase); });

            AddFilesToListView();
        }

        private void ButtonHoldTimer_Tick(object sender, EventArgs e)
        {
            _buttonHoldTimer.Stop();
            if (MouseButtons == MouseButtons.Left)
                // copy the button and use it as the cursor while dragging and dropping.
                //Bitmap bmpButtonCopy = new Bitmap(Width, Height);
                //DrawToBitmap(bmpButtonCopy, new Rectangle(Point.Empty, bmpButtonCopy.Size));
                //_dragAndDropCursor = new Cursor(bmpButtonCopy.GetHicon());
                //Cursor.Current = _dragAndDropCursor;
                DoDragDrop(listPrograms.SelectedItems, DragDropEffects.Move);
        }

        private void ButtonOK_Click(object sender, EventArgs e) { Close(); }

        private void listPrograms_MouseDown(object sender, MouseEventArgs e) { _buttonHoldTimer.Start(); }

        private void listPrograms_MouseUp(object sender, MouseEventArgs e) { _buttonHoldTimer.Stop(); }

        private void search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                search.Clear();
                searchLabel.Visible = true;
                listPrograms.Focus();
            }
        }

        private void search_TextChanged(object sender, EventArgs e) { AddFilesToListView(); }

        private void searchLabel_Click(object sender, EventArgs e)
        {
            searchLabel.Visible = false;
            search.Focus();
        }

        public class InstalledApp
        {
            public readonly string Caption;
            public readonly string FileName;
            public readonly Image Icon;
            public string Arguments;
            public string IconIndex;
            public string ShortcutPath;
            public string WorkingFolder;

            public InstalledApp(string fileName)
            {
                if (Misc.IsShortcut(fileName))
                {
                    Misc.ParseShortcut(fileName, out string parsedFileName, out string parsedFileIcon, out string parsedFileIconIndex, out string parsedArgs, out string parsedWorkingFolder);
                    Icon = IconFuncs.GetIcon(parsedFileIcon, parsedFileIconIndex);

                    if (Icon == null && Funcs.IsUrl(parsedFileName))
                        Icon = Funcs.GetWebsiteFavIconAsImage(parsedFileName);
                    else if (Icon == null)
                        Icon = IconFuncs.GetIcon(parsedFileName, "0");

                    Caption = Path.GetFileName(fileName).Replace(".url", "").Replace(".lnk", "");
                    FileName = parsedFileName;
                    ShortcutPath = fileName;
                    WorkingFolder = parsedWorkingFolder;
                    IconIndex = parsedFileIconIndex;
                    Arguments = parsedArgs;
                }
            }
        }
    }
}