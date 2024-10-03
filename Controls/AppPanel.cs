using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Apps.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Utility;
using static System.Windows.Forms.ListView;
using IDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;

namespace Apps.Controls
{
    public class AppPanel : Panel
    {
        public delegate void AppClickedHandler();

        public delegate void AppsChangedHandler();

        private const string NewAppsXmlFile = "<XML VERSION=\"1.0\" ENCODING=\"utf-8\">\r\n<APPS>\r\n</APPS>\r\n</XML>";
        private const int WmSetredraw = 11;
        private readonly string _appsXmlFilePath = Funcs.AppPath() + "\\Apps.xml";
        private readonly ToolStripMenuItem _deleteMenuItem;
        private readonly ToolStripMenuItem _downMenuItem;
        private readonly ToolStripMenuItem _editMenuItem;
        private readonly List<AppCache> _folderCache;
        private readonly AppMenu _menuRc;
        private readonly ToolStripMenuItem _moveToParentItem;

        private readonly ToolStripMenuItem _upMenuItem;

        private XmlNode _appsNode;
        private XmlDocument _appsXml;
        private int _beginUpdateCounter;
        private XmlNode _currentParentNode;
        private bool _inMenu;

        private Programs ProgramsForm;

        public AppPanel(Config myConfig)
        {
            DoubleBuffered = true;
            BeginUpdate();
            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += ConfigChanged;

            // Hide scrollbars and then enable AutoScroll.
            AutoScroll = false;
            HorizontalScroll.Maximum = 0;
            HorizontalScroll.Visible = false;
            VerticalScroll.Maximum = 0;
            VerticalScroll.Visible = false;
            AutoScroll = true;

            _folderCache = new List<AppCache>();
            _menuRc = new AppMenu(myConfig) { ShowCheckMargin = false, ShowImageMargin = false };
            _menuRc.Opening += Menu_Opening;
            //Funcs.AddMenuItem(_menuRc, "Add Application From List", MenuAddAppFromList_Click);
            Funcs.AddMenuItem(_menuRc, "Add Application",           MenuAddApp_Click);
            Funcs.AddMenuItem(_menuRc, "Add Folder",                MenuAddFolder_Click);
            Funcs.AddMenuItem(_menuRc, "Add Folder Link",           MenuAddFolderLink_Click);
            Funcs.AddMenuItem(_menuRc, "Add Separator",             MenuAddSeparator_Click);
            Funcs.AddMenuItem(_menuRc, "Add URL",                   MenuAddURL_Click);
            _editMenuItem = Funcs.AddMenuItem(_menuRc,   "Edit",   MenuEdit_Click);
            _deleteMenuItem = Funcs.AddMenuItem(_menuRc, "Delete", MenuDelete_Click);
            _menuRc.Items.Add(new ToolStripSeparator());
            _moveToParentItem = Funcs.AddMenuItem(_menuRc, "Move To Parent", MenuMoveToParent_Click);
            _upMenuItem = Funcs.AddMenuItem(_menuRc,       "Move Up",        MenuUp_Click);
            _downMenuItem = Funcs.AddMenuItem(_menuRc,     "Move Down",      MenuDown_Click);
            ContextMenuStrip = _menuRc;
            AllowDrop = true;
            DragOver += OnDragOver;
            DragDrop += DropToPanel;
            SetColors();
            LoadItems();
            EndUpdate();
        }

        private Config AppsConfig { get; }
        public string GetRootId { get; } = Guid.NewGuid().ToString();

        public bool InMenu { get => InProgramList() || _inMenu; set => _inMenu = value; }

        public bool InLoad { get; set; }

        public bool InAFolder => _currentParentNode != null;

        public string CurrentAppId => _currentParentNode != null ? _currentParentNode.Attributes["id"].Value : GetRootId;

        public string CurrentFolderName
        {
            get
            {
                if (_currentParentNode != null)
                    return _currentParentNode.Attributes["foldername"].Value;
                return "";
            }
        }

        public void AddApp(AppButton appButton, int addAtIndex)
        {
            if (string.IsNullOrEmpty(appButton.AppId))
                appButton.AppId = Guid.NewGuid().ToString();

            if (appButton.Node == null)
            {
                XmlNode nodeSib = ((AppButton)Controls[addAtIndex]).Node;

                if (AppsConfig.ParseShortcuts && Misc.IsShortcut(appButton.FileName))
                {
                    appButton.AppName = Path.GetFileNameWithoutExtension(appButton.FileName);
                    appButton.ParseShortcut();
                }

                appButton.Node = _appsXml.CreateNode(XmlNodeType.Element, "APP", null);
                SetAttrib(appButton.Node, "id",                appButton.AppId);
                SetAttrib(appButton.Node, "appname",           appButton.AppName);
                SetAttrib(appButton.Node, "filename",          appButton.FileName);
                SetAttrib(appButton.Node, "fileiconpath",      appButton.FileIconPath);
                SetAttrib(appButton.Node, "fileiconindex",     appButton.FileIconIndex);
                SetAttrib(appButton.Node, "fileargs",          appButton.FileArgs);
                SetAttrib(appButton.Node, "fileworkingfolder", appButton.FileWorkingFolder);
                XmlNode parentNode = _currentParentNode ?? _appsNode;
                if (nodeSib == null)
                    parentNode.AppendChild(appButton.Node);
                else
                    parentNode.InsertAfter(appButton.Node, nodeSib);
                SaveXml();
            }

            SetButtonDetails(appButton);
            Controls.SetChildIndex(appButton, addAtIndex); // move button where we want it.
            appButton.Visible = true;
            if (!InLoad)
                OnAppsChanged?.Invoke();
        }

        private AppButton AddAppButton(ButtonType buttonType, dynamic addTo)
        {
            AppButton b = new AppButton(AppsConfig, buttonType);
            b.OnAppButtonClicked += ButtonClicked;
            b.OnAppButtonDropped += DropToButton;
            b.ContextMenuStrip = _menuRc;
            b.Padding = new Padding(0, 0, 0, 0);
            b.Margin = new Padding(0,  0, 0, 0);
            b.TabStop = false;
            // hide the button when it's added intitially, this way the hidden scrollbars don't flash visible/invisible.
            // button is set back to visible in the AppAdded event in the main form. It's done this way because the form
            // is automatically resized based on the number of buttons.
            // Also we don't want to see a newly created (blank) button popup in the AppPanel.
            if (!InLoad)
                b.Visible = false;

            if (addTo is AppCache cache)
                cache.Insert(0, b);
            else
                Controls.Add(b);

            b.AutoSize = false;
            b.Dock = DockStyle.Top;
            return b;
        }

        public void AddFolder(AppButton appButton, int addAtIndex)
        {
            appButton.AppId = Guid.NewGuid().ToString();
            appButton.Node = _appsXml.CreateNode(XmlNodeType.Element, "APP", null);
            XmlNode nodeSib = ((AppButton)Controls[addAtIndex]).Node;
            SetAttrib(appButton.Node, "id",         appButton.AppId);
            SetAttrib(appButton.Node, "foldername", appButton.AppName);
            XmlNode parentNode = _currentParentNode ?? _appsNode;
            if (nodeSib == null)
                parentNode.AppendChild(appButton.Node);
            else
                parentNode.InsertAfter(appButton.Node, nodeSib);
            SaveXml();

            SetButtonDetails(appButton);
            AddToFolderCache(appButton.AppId);
            Controls.SetChildIndex(appButton, addAtIndex); // move button where we want it.
        }

        public void AddFolderLink(AppButton appButton, int addAtIndex)
        {
            appButton.AppId = Guid.NewGuid().ToString();
            appButton.Node = _appsXml.CreateNode(XmlNodeType.Element, "APP", null);

            XmlNode nodeSib = ((AppButton)Controls[addAtIndex]).Node;
            SetAttrib(appButton.Node, "id",             appButton.AppId);
            SetAttrib(appButton.Node, "folderlinkname", appButton.AppName);
            SetAttrib(appButton.Node, "folderlinkpath", appButton.FileName);
            XmlNode parentNode = _currentParentNode ?? _appsNode;
            if (nodeSib == null)
                parentNode.AppendChild(appButton.Node);
            else
                parentNode.InsertAfter(appButton.Node, nodeSib);

            SaveXml();
            SetButtonDetails(appButton);
            Controls.SetChildIndex(appButton, addAtIndex); // move button where we want it.
            appButton.Visible = true;
        }

        private void AddItems(XmlNode nodes)
        {
            string id = GetAttrib(nodes, "id");
            if (id == "")
                id = GetRootId;
            AppCache ac = _folderCache.Find(x => x.FolderId == id);
            Controls.AddRange(ac.ToArray());
            ac.Clear();
        }

        public void AddSeparator(int addAtIndex)
        {
            BeginUpdate();
            AppButton appButton = AddAppButton(ButtonType.Separator, Controls);
            appButton.AppId = Guid.NewGuid().ToString();
            appButton.Node = _appsXml.CreateNode(XmlNodeType.Element, "APP", null);
            XmlNode nodeSib = ((AppButton)Controls[addAtIndex]).Node;
            SetAttrib(appButton.Node, "id",        appButton.AppId);
            SetAttrib(appButton.Node, "separator", "Y");

            XmlNode parentNode = _currentParentNode ?? _appsNode;
            if (nodeSib == null)
                parentNode.AppendChild(appButton.Node);
            else
                parentNode.InsertAfter(appButton.Node, nodeSib);
            SaveXml();
            SetButtonDetails(appButton);
            Controls.SetChildIndex(appButton, addAtIndex); // move button where we want it.
            appButton.Visible = true;
            EndUpdate();
        }

        private AppCache AddToFolderCache(string id)
        {
            AppCache f = _folderCache.Find(x => x.FolderId == id);
            if (f == null)
            {
                _folderCache.Add(new AppCache(id));
                return _folderCache[_folderCache.Count - 1];
            }

            return f;
        }

        private void AddUrl(AppButton appButton, int addAtIndex)
        {
            appButton.AppId = Guid.NewGuid().ToString();
            appButton.Node = _appsXml.CreateNode(XmlNodeType.Element, "APP", null);
            XmlNode nodeSib = ((AppButton)Controls[addAtIndex]).Node;
            SetAttrib(appButton.Node, "id",      appButton.AppId);
            SetAttrib(appButton.Node, "appname", appButton.AppName);
            SetAttrib(appButton.Node, "url",     appButton.Url);
            SetAttrib(appButton.Node, "favicon", appButton.FavIcon);
            XmlNode parentNode = _currentParentNode ?? _appsNode;
            if (nodeSib == null)
                parentNode.AppendChild(appButton.Node);
            else
                parentNode.InsertAfter(appButton.Node, nodeSib);
            SaveXml();

            SetButtonDetails(appButton);
            AddToFolderCache(appButton.AppId);
            Controls.SetChildIndex(appButton, addAtIndex); // move button where we want it.
            appButton.Visible = true;
        }

        public void BeginUpdate()
        {
            _beginUpdateCounter++;
            Funcs.SendMessage(Handle, WmSetredraw, false, 0);
            AutoScroll = false;
        }

        private void DoExternalDropTo(AppButton toAppButton, DragEventArgs e)
        {
            string GetAppFileName(string filename)
            {
                string s = filename;
                // some apps are tacking on the shell:{} guid information when it's not needed. So check to see if it's just the filename, if not parse it out.
                if (s.IndexOf("\\") > -1)
                {
                    int ilen = filename.Length - 1;
                    for (int i = ilen; i > 0; i--)
                        if (filename[i] == '\\')
                        {
                            s = filename.Substring(i + 1, filename.Length - i - 1);
                            break;
                        }
                }

                return s;
            }

            object objRef = e.Data.GetData(typeof(SelectedListViewItemCollection));
            if (objRef != null && objRef is SelectedListViewItemCollection lv)
            {
                foreach (ListViewItem item in lv)
                {
                    Programs.InstalledApp app = (Programs.InstalledApp)item.Tag;
                    AppButton b = AddAppButton(Funcs.IsUrl(app.FileName) ? ButtonType.Url : ButtonType.App, Controls);

                    if (b.ButtonType == ButtonType.Url)
                    {
                        b.AppName = app.Caption;
                        b.Url = app.FileName;
                        AddUrl(b, toAppButton == null ? 0 : Controls.GetChildIndex(toAppButton));
                    }
                    else
                    {
                        b.AppName = app.Caption;
                        b.FileName = !string.IsNullOrEmpty(app.ShortcutPath) ? app.ShortcutPath : app.FileName;
                        b.FileArgs = app.Arguments;
                        AddApp(b, toAppButton == null ? 0 : Controls.GetChildIndex(toAppButton));
                    }
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string s = e.Data.GetData(DataFormats.Text).ToString();
                if (Funcs.IsUrl(s))
                {
                    AppButton b = AddAppButton(ButtonType.Url, Controls);
                    b.AppName = s;
                    b.Url = s;
                    AddUrl(b, toAppButton == null ? 0 : Controls.GetChildIndex(toAppButton));
                }
            }
            else
                // shell object?
            if (e.Data is IDataObject)
            {
                ShellObjectCollection shellObj = ShellObjectCollection.FromDataObject((IDataObject)e.Data);
                if (shellObj.Count > 0)
                {
                    BeginUpdate();
                    foreach (ShellObject shellFile in shellObj)
                        // File or Folder?
                        if (File.Exists(shellFile.ParsingName) || (!File.Exists(shellFile.ParsingName) && Directory.Exists(shellFile.ParsingName)))
                        {
                            if (File.Exists(shellFile.ParsingName))
                            {
                                AppButton b = AddAppButton(ButtonType.App, Controls);
                                b.AppName = string.IsNullOrEmpty(Funcs.GetFileInfo(shellFile.ParsingName).ProductName)
                                    ? Path.GetFileNameWithoutExtension(shellFile.ParsingName)
                                    : Funcs.GetFileInfo(shellFile.ParsingName).ProductName;
                                b.FileName = shellFile.ParsingName;
                                AddApp(b, toAppButton == null ? 0 : Controls.GetChildIndex(toAppButton));
                            }
                            else
                            {
                                AppButton b = AddAppButton(ButtonType.FolderLink, Controls);
                                b.AppName = Path.GetFileName(shellFile.ParsingName);
                                b.FileName = shellFile.ParsingName;
                                AddFolderLink(b, toAppButton == null ? 0 : Controls.GetChildIndex(toAppButton));
                            }
                        }
                        else
                        {
                            AppButton b = AddAppButton(ButtonType.App, Controls);
                            b.AppName = shellFile.Name;
                            b.FileName = "shell:AppsFolder\\" + GetAppFileName(shellFile.ParsingName);
                            AddApp(b, toAppButton == null ? 0 : Controls.GetChildIndex(toAppButton));
                        }

                    EndUpdate();
                }
                // some apps from the new windows 11 start menu come through blank.
            }
        }

        public void EndUpdate()
        {
            _beginUpdateCounter--;
            if (_beginUpdateCounter == 0)
            {
                ResumeLayout();
                Funcs.SendMessage(Handle, WmSetredraw, true, 0);
                if (!InLoad)
                    OnAppsChanged?.Invoke();
                Refresh();
                AutoScroll = true;
            }
        }

        private AppButton GetAppButton(object sender)
        {
            if (sender is AppMenu menu)
            {
                Control c = menu.SourceControl;
                return (AppButton)c.Parent.Parent.Parent; // Label > Panel > Panel > AppButton
            }
            else
            {
                Control c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                return (AppButton)c.Parent.Parent.Parent; // Label > Panel > Panel > AppButton
            }
        }

        private int GetAppButtonIndex(AppButton appButton) { return Controls.GetChildIndex(appButton); }

        private string GetAttrib(XmlNode xn, string attributeName)
        {
            if (xn.Attributes[attributeName] != null)
                return xn.Attributes[attributeName].Value;
            return "";
        }

        private XmlNode GetNextNode(XmlNode node) { return node.NextSibling; }

        private XmlNode GetPrevNode(XmlNode node) { return node.PreviousSibling; }

        public void GoBack()
        {
            if (_currentParentNode.ParentNode != null && _currentParentNode.ParentNode.Attributes["id"] != null && !ModifierKeys.HasFlag(Keys.Control))
                LoadFolder(LookupFolderButton(_currentParentNode.ParentNode.Attributes["id"].Value));
            else
                LoadItems();
        }

        public bool InProgramList() { return ProgramsForm != null && ProgramsForm.Visible; }

        private void LoadCache()
        {
            void AddToCache(XmlNode nodes)
            {
                string id = GetAttrib(nodes, "id");
                if (id == "")
                    id = GetRootId;

                AppCache ac = AddToFolderCache(id);

                foreach (XmlNode xn in nodes)
                    if (GetAttrib(xn, "folderlinkname") != "")
                    {
                        AppButton b = AddAppButton(ButtonType.FolderLink, ac);
                        b.Node = xn;
                        SetButtonDetails(b);
                    }
                    else if (GetAttrib(xn, "foldername") != "")
                    {
                        AppButton b = AddAppButton(ButtonType.Folder, ac);
                        b.Node = xn;
                        SetButtonDetails(b);
                        AddToCache(xn); // Recsurvise add contents of folder
                    }
                    else if (GetAttrib(xn, "separator") != "")
                    {
                        AppButton b = AddAppButton(ButtonType.Separator, ac);
                        b.Node = xn;
                        SetButtonDetails(b);
                    }
                    else if (GetAttrib(xn, "appname") != "")
                    {
                        AppButton b = AddAppButton(string.IsNullOrEmpty(GetAttrib(xn, "url")) ? ButtonType.App : ButtonType.Url, ac);
                        b.Node = xn;
                        SetButtonDetails(b);
                    }
            }

            if (_folderCache.Count == 0)
                AddToCache(_appsNode);
        }

        private void LoadFolder(AppButton appButton)
        {
            BeginUpdate();
            MoveToCache();
            _currentParentNode = appButton.Node;
            AddItems(_currentParentNode);
            EndUpdate();
        }

        public void LoadItems()
        {
            BeginUpdate();
            InLoad = true;
            bool doLoad = false;

            if (_currentParentNode != null)
            {
                doLoad = true;
                MoveToCache();
            }

            if (_appsNode == null)
            {
                doLoad = true;
                if (!File.Exists(_appsXmlFilePath))
                {
                    _appsXml = new XmlDocument();
                    _appsXml.LoadXml(NewAppsXmlFile);
                    SaveXml();
                }
                else
                {
                    _appsXml = new XmlDocument();
                    _appsXml.Load(_appsXmlFilePath);
                }

                _appsNode = _appsXml.SelectSingleNode("//APPS");
            }

            if (doLoad)
            {
                LoadCache();
                AddItems(_appsNode);
            }

            InLoad = false;
            EndUpdate();
        }

        private AppButton LookupFolderButton(string id)
        {
            foreach (AppCache a in _folderCache)
                foreach (AppButton b in a)
                    if (b.AppId == id)
                        return b;
            return null;
        }

        private void MoveButton(AppButton fromButton, AppButton toButton)
        {
            BeginUpdate();
            XmlNode parentNode = _currentParentNode ?? _appsNode;
            if (toButton == null)
            {
                parentNode.AppendChild(fromButton.Node);
                Controls.SetChildIndex(fromButton, 0);
            }
            else
            {
                int t = GetAppButtonIndex(toButton);
                int f = GetAppButtonIndex(fromButton);

                if (t < f)
                    parentNode.InsertAfter(fromButton.Node, toButton.Node);
                else
                    parentNode.InsertBefore(fromButton.Node, toButton.Node);

                Controls.SetChildIndex(fromButton, t);
            }

            SaveXml();
            EndUpdate();
        }

        private void MoveButtonInto(AppButton fromButton, AppButton toButton)
        {
            BeginUpdate();
            XmlNode parentNode = toButton.Node;
            parentNode.AppendChild(fromButton.Node);
            MoveToCache(fromButton, GetAttrib(parentNode, "id"));
            SaveXml();
            EndUpdate();
        }

        private void MoveToCache()
        {
            string id = CurrentAppId;
            _currentParentNode = null;
            AppCache f = AddToFolderCache(id);

            foreach (Control c in Controls) f.Add(c);
            Controls.Clear();
        }

        private void MoveToCache(AppButton appButton, string toId)
        {
            if (toId == "")
                toId = GetRootId;
            AppCache ac = AddToFolderCache(toId);
            if (ac != null)
                ac.Insert(0, appButton);
            Controls.Remove(appButton);
        }

        private void SaveXml() { _appsXml.Save(_appsXmlFilePath); }

        private void SetAttrib(XmlNode node, string attribName, string value)
        {
            XmlAttribute xmlAtt;
            if (node.Attributes[attribName] == null)
                xmlAtt = _appsXml.CreateAttribute(attribName);
            else
                xmlAtt = node.Attributes[attribName];

            xmlAtt.Value = value;
            node.Attributes.Append(xmlAtt);
        }

        private void SetButtonDetails(AppButton appButton)
        {
            if (appButton.ButtonType == ButtonType.App)
            {
                appButton.AppId = GetAttrib(appButton.Node,             "id");
                appButton.AppName = GetAttrib(appButton.Node,           "appname");
                appButton.AsAdmin = GetAttrib(appButton.Node,           "asadmin");
                appButton.FileName = GetAttrib(appButton.Node,          "filename");
                appButton.FileArgs = GetAttrib(appButton.Node,          "fileargs");
                appButton.FileIconIndex = GetAttrib(appButton.Node,     "fileiconindex");
                appButton.FileIconPath = GetAttrib(appButton.Node,      "fileiconpath");
                appButton.FileWorkingFolder = GetAttrib(appButton.Node, "fileworkingfolder");
                if (appButton.FileIconImage == null)
                    appButton.WatchForIconUpdate = true;
            }
            else if (appButton.ButtonType == ButtonType.Folder)
            {
                appButton.AppId = GetAttrib(appButton.Node,   "id");
                appButton.AppName = GetAttrib(appButton.Node, "foldername");
            }
            else if (appButton.ButtonType == ButtonType.FolderLink)
            {
                appButton.AppId = GetAttrib(appButton.Node,    "id");
                appButton.AppName = GetAttrib(appButton.Node,  "folderlinkname");
                appButton.FileName = GetAttrib(appButton.Node, "folderlinkpath");
            }
            else if (appButton.ButtonType == ButtonType.Separator)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
            }
            else if (appButton.ButtonType == ButtonType.Url)
            {
                appButton.AppId = GetAttrib(appButton.Node,   "id");
                appButton.AppName = GetAttrib(appButton.Node, "appname");
                appButton.Url = GetAttrib(appButton.Node,     "url");
                appButton.FavIcon = GetAttrib(appButton.Node, "favicon");
            }
        }

        private void SetColors() { BackColor = AppsConfig.AppsBackColor; }

        private void ButtonClicked(AppButton app)
        {
            if (!app.IsFolderButton)
                OnAppClicked?.Invoke();
            else
                LoadFolder(app);
        }

        private void ConfigChanged(object sender, EventArgs e) { SetColors(); }

        private void DropToButton(AppButton toAppButton, DragEventArgs e)
        {
            InMenu = true;
            object objRef = e.Data.GetData(typeof(AppButton));
            if (objRef != null && objRef is AppButton b)
            {
                if (toAppButton.IsFolderButton)
                {
                    Confirm c = new Confirm(AppsConfig);
                    DialogResult r = c.ShowAsDialog(ConfirmButtons.YesNo, "Move " + b.AppName + " into folder " + toAppButton.AppName + "?", "Move " + b.AppName + "?");
                    if (r == DialogResult.Yes)
                        MoveButtonInto(b, toAppButton);
                    else
                        MoveButton(b, toAppButton);
                }
                else
                {
                    MoveButton(b, toAppButton);
                }
            }
            else
            {
                DoExternalDropTo(toAppButton, e);
            }

            InMenu = false;
        }

        private void DropToPanel(object sender, DragEventArgs e)
        {
            object objRef = e.Data.GetData(typeof(AppButton));
            if (objRef != null && objRef is AppButton b)
                MoveButton(b, null);
            else
                DoExternalDropTo(null, e);
        }

        private void Menu_Opening(object sender, CancelEventArgs e)
        {
            Control c = ((AppMenu)sender).SourceControl;
            if (c is AppPanel)
            {
                _deleteMenuItem.Enabled = false;
                _editMenuItem.Enabled = false;
                _moveToParentItem.Enabled = false;
                _upMenuItem.Enabled = false;
                _downMenuItem.Enabled = false;
            }
            else if (c is AppButtonText)
            {
                _deleteMenuItem.Enabled = true;
                _editMenuItem.Enabled = GetAppButton(sender).IsSeparatorButton != true;
                _moveToParentItem.Enabled = GetAppButton(sender).Node.ParentNode.ParentNode.Name.ToLower() != "xml";
                _upMenuItem.Enabled = GetPrevNode(GetAppButton(sender).Node) != null;
                _downMenuItem.Enabled = GetNextNode(GetAppButton(sender).Node) != null;
            }
        }

        private void MenuAddApp_Click(object sender, EventArgs e)
        {
            InMenu = true;
            AppButton b = AddAppButton(ButtonType.App, Controls);
            Forms.Properties f = new Forms.Properties(AppsConfig, b);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                Control c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                if (c is AppPanel)
                    AddApp(b, 0);
                else
                    AddApp(b, Controls.GetChildIndex(GetAppButton(sender)));
            }
            else
            {
                Controls.Remove(b);
                b.Dispose();
            }

            b.Visible = true;
            InMenu = false;
        }

        private void MenuAddAppFromList_Click(object sender, EventArgs e)
        {
            InMenu = true;
            if (ProgramsForm == null || ProgramsForm.IsDisposed) ProgramsForm = new Programs(AppsConfig, Funcs.GetParentForm(this));
            ProgramsForm.Show();
            InMenu = false;
        }

        private void MenuAddFolder_Click(object sender, EventArgs e)
        {
            InMenu = true;
            BeginUpdate();
            AppButton appButton = AddAppButton(ButtonType.Folder, Controls);
            Folder f = new Folder(AppsConfig, appButton);

            if (f.ShowDialog(this) == DialogResult.OK)
            {
                Control c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                if (c is AppPanel)
                    AddFolder(appButton, 0);
                else
                    AddFolder(appButton, Controls.GetChildIndex(GetAppButton(sender)));
            }
            else
            {
                Controls.Remove(appButton);
                appButton.Dispose();
            }

            EndUpdate();
            appButton.Visible = true;
            InMenu = false;
        }

        private void MenuAddFolderLink_Click(object sender, EventArgs e)
        {
            InMenu = true;
            BeginUpdate();
            AppButton appButton = AddAppButton(ButtonType.FolderLink, Controls);
            FolderLink f = new FolderLink(AppsConfig, appButton);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                Control c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                if (c is AppPanel)
                    AddFolderLink(appButton, 0);
                else
                    AddFolderLink(appButton, Controls.GetChildIndex(GetAppButton(sender)));
            }
            else
            {
                Controls.Remove(appButton);
                appButton.Dispose();
            }

            EndUpdate();
            appButton.Visible = true;
            InMenu = false;
        }

        private void MenuAddSeparator_Click(object sender, EventArgs e)
        {
            InMenu = true;
            Control c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
            if (c is AppPanel)
                AddSeparator(0);
            else
                AddSeparator(Controls.GetChildIndex(GetAppButton(sender)));
            InMenu = false;
        }

        private void MenuAddURL_Click(object sender, EventArgs e)
        {
            InMenu = true;
            BeginUpdate();
            AppButton appButton = AddAppButton(ButtonType.Url, Controls);
            WebLink f = new WebLink(AppsConfig, appButton);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                Control c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                if (c is AppPanel)
                    AddUrl(appButton, 0);
                else
                    AddUrl(appButton, Controls.GetChildIndex(GetAppButton(sender)));
            }
            else
            {
                Controls.Remove(appButton);
                appButton.Dispose();
            }

            EndUpdate();
            InMenu = false;
        }

        private void MenuDelete_Click(object sender, EventArgs e)
        {
            InMenu = true;
            AppButton b = GetAppButton(sender);

            string s = "";
            if (b.IsAppButton)
                s = "application?";
            else if (b.IsFolderButton)
                s = "folder and all children?";
            else if (b.IsFolderLinkButton)
                s = "folder link?";
            else if (b.IsSeparatorButton)
                s = "Separator?";

            bool canDelete = Misc.ConfirmDialog(AppsConfig, ConfirmButtons.OkCancel, "Delete " + s, "Delete " + b.AppName + "?") == DialogResult.OK;

            if (canDelete)
            {
                BeginUpdate();
                XmlNode parentNode = b.Node.ParentNode;
                parentNode.RemoveChild(b.Node);
                Controls.Remove(b);
                // Cleanup cache?
                SaveXml();
                EndUpdate();
            }

            InMenu = false;
        }

        private void MenuDown_Click(object sender, EventArgs e)
        {
            InMenu = true;
            MoveButton(GetAppButton(sender), (AppButton)Controls[GetAppButtonIndex(GetAppButton(sender)) - 1]);
            InMenu = false;
        }

        private void MenuEdit_Click(object sender, EventArgs e)
        {
            InMenu = true;
            BeginUpdate();
            AppButton b = GetAppButton(sender);

            if (b.IsUrlButton)
            {
                WebLink f = new WebLink(AppsConfig, b);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    SetAttrib(b.Node, "appname", b.AppName);
                    SetAttrib(b.Node, "url",     b.Url);
                    SetAttrib(b.Node, "favicon", b.FavIcon);
                    SaveXml();
                }

                f.Dispose();
            }
            else if (b.IsFolderLinkButton)
            {
                FolderLink f = new FolderLink(AppsConfig, b);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    SetAttrib(b.Node, "folderlinkname", b.AppName);
                    SetAttrib(b.Node, "folderlinkpath", b.FileName);
                    SaveXml();
                }

                f.Dispose();
            }
            else if (b.IsFolderButton)
            {
                Folder f = new Folder(AppsConfig, b);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    SetAttrib(b.Node, "foldername", b.AppName);
                    SaveXml();
                }

                f.Dispose();
            }
            else
            {
                Forms.Properties f = new Forms.Properties(AppsConfig, b);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    SetAttrib(b.Node, "appname",           b.AppName);
                    SetAttrib(b.Node, "asadmin",           b.AsAdmin);
                    SetAttrib(b.Node, "filename",          b.FileName);
                    SetAttrib(b.Node, "fileiconpath",      b.FileIconPath);
                    SetAttrib(b.Node, "fileiconindex",     b.FileIconIndex);
                    SetAttrib(b.Node, "fileargs",          b.FileArgs);
                    SetAttrib(b.Node, "fileworkingfolder", b.FileWorkingFolder);
                    SaveXml();
                }

                f.Dispose();
            }

            EndUpdate();
            InMenu = false;
        }

        private void MenuMoveToParent_Click(object sender, EventArgs e)
        {
            BeginUpdate();
            AppButton b = GetAppButton(sender);
            XmlNode pn = b.Node.ParentNode.ParentNode;
            pn.AppendChild(b.Node);
            MoveToCache(b, GetAttrib(pn, "id"));
            SaveXml();
            EndUpdate();
        }

        private void MenuUp_Click(object sender, EventArgs e)
        {
            InMenu = true;
            MoveButton(GetAppButton(sender), (AppButton)Controls[GetAppButtonIndex(GetAppButton(sender)) + 1]);
            InMenu = false;
        }

        private void OnDragOver(object sender, DragEventArgs e) { e.Effect = DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move; }

        public event AppClickedHandler OnAppClicked;

        public event AppsChangedHandler OnAppsChanged;
    }
}