using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Apps.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Utility;
using IDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;

namespace Apps.Controls
{
    public class AppPanel : Panel
    {
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
            _menuRc = new AppMenu(myConfig)
            {
                ShowCheckMargin = false,
                ShowImageMargin = false
            };
            _menuRc.Opening += Menu_Opening;
            Funcs.AddMenuItem(_menuRc, "Add Application", MenuAddApp_Click);
            Funcs.AddMenuItem(_menuRc, "Add Folder", MenuAddFolder_Click);
            Funcs.AddMenuItem(_menuRc, "Add Folder Link", MenuAddFolderLink_Click);
            Funcs.AddMenuItem(_menuRc, "Add Separator", MenuAddSeparator_Click);
            Funcs.AddMenuItem(_menuRc, "Add URL", MenuAddURL_Click);
            _editMenuItem = Funcs.AddMenuItem(_menuRc, "Edit", MenuEdit_Click);
            _deleteMenuItem = Funcs.AddMenuItem(_menuRc, "Delete", MenuDelete_Click);
            _menuRc.Items.Add(new ToolStripSeparator());
            _moveToParentItem = Funcs.AddMenuItem(_menuRc, "Move To Parent", MenuMoveToParent_Click);
            _upMenuItem = Funcs.AddMenuItem(_menuRc, "Move Up", MenuUp_Click);
            _downMenuItem = Funcs.AddMenuItem(_menuRc, "Move Down", MenuDown_Click);
            ContextMenuStrip = _menuRc;
            AllowDrop = true;
            DragOver += OnDragOver;
            DragDrop += DropToPanel;
            SetColors();
            LoadItems();
            EndUpdate();
        }

        #region Properties

        private Config AppsConfig { get; }

        public string GetRootId { get; } = Guid.NewGuid().ToString();

        public bool InMenu { get; set; }
        public bool InLoad { get; set; }

        public bool InAFolder => _currentParentNode != null;

        public string CurrentAppId =>
            _currentParentNode != null ? _currentParentNode.Attributes["id"].Value : GetRootId;

        public string CurrentFolderName
        {
            get
            {
                if (_currentParentNode != null)
                    return _currentParentNode.Attributes["foldername"].Value;
                return "";
            }
        }

        #endregion

        #region Privates

        private XmlNode _appsNode;
        private XmlDocument _appsXml;
        private readonly string _appsXmlFilePath = Funcs.AppPath() + "\\Apps.xml";
        private int _beginUpdateCounter;
        private XmlNode _currentParentNode;
        private readonly ToolStripMenuItem _deleteMenuItem;
        private readonly ToolStripMenuItem _downMenuItem;
        private readonly ToolStripMenuItem _editMenuItem;
        private readonly List<AppCache> _folderCache;
        private readonly AppMenu _menuRc;
        private readonly ToolStripMenuItem _moveToParentItem;

        private const string NewAppsXmlFile = "<XML VERSION=\"1.0\" ENCODING=\"utf-8\">\r\n<APPS>\r\n</APPS>\r\n</XML>";

        private readonly ToolStripMenuItem _upMenuItem;
        private const int WmSetredraw = 11;

        #endregion

        #region EventHandlers

        public delegate void AppClickedHandler();

        public event AppClickedHandler OnAppClicked;

        public delegate void AppsChangedHandler();

        public event AppsChangedHandler OnAppsChanged;

        #endregion

        #region Events

        private void ButtonClicked(AppButton app)
        {
            if (!app.IsFolderButton)
                OnAppClicked?.Invoke();
            else
                LoadFolder(app);
        }

        private void ConfigChanged(object sender, EventArgs e)
        {
            SetColors();
        }

        private void DropToButton(AppButton toAppButton, DragEventArgs e)
        {
            InMenu = true;
            var objRef = e.Data.GetData(typeof(AppButton));
            if (objRef != null && objRef is AppButton b)
            {
                if (toAppButton.IsFolderButton)
                {
                    var c = new Confirm(AppsConfig);
                    var r = c.ShowAsDialog(ConfirmButtons.YesNo,
                        "Move " + b.AppName + " into folder " + toAppButton.AppName + "?", "Move " + b.AppName + "?");
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
            var objRef = e.Data.GetData(typeof(AppButton));
            if (objRef != null && objRef is AppButton b)
                MoveButton(b, null);
            else
                DoExternalDropTo(null, e);
        }

        private void MenuAddApp_Click(object sender, EventArgs e)
        {
            InMenu = true;
            var b = AddAppButton(ButtonType.App, Controls);
            var f = new Forms.Properties(AppsConfig, b);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
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

        private void MenuAddFolder_Click(object sender, EventArgs e)
        {
            InMenu = true;
            BeginUpdate();
            var appButton = AddAppButton(ButtonType.Folder, Controls);
            var f = new Folder(AppsConfig, appButton);

            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
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
            var appButton = AddAppButton(ButtonType.FolderLink, Controls);
            var f = new FolderLink(AppsConfig, appButton);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
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
            var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
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
            var appButton = AddAppButton(ButtonType.Url, Controls);
            var f = new WebLink(AppsConfig, appButton);
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
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
            var b = GetAppButton(sender);

            var s = "";
            if (b.IsAppButton)
                s = "application?";
            else if (b.IsFolderButton)
                s = "folder and all children?";
            else if (b.IsFolderLinkButton)
                s = "folder link?";
            else if (b.IsSeparatorButton)
                s = "Separator?";

            var canDelete =
                Misc.ConfirmDialog(AppsConfig, ConfirmButtons.OkCancel, "Delete " + s, "Delete " + b.AppName + "?") ==
                DialogResult.OK;

            if (canDelete)
            {
                BeginUpdate();
                var parentNode = b.Node.ParentNode;
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

        private void MenuUp_Click(object sender, EventArgs e)
        {
            InMenu = true;
            MoveButton(GetAppButton(sender), (AppButton)Controls[GetAppButtonIndex(GetAppButton(sender)) + 1]);
            InMenu = false;
        }

        private void MenuEdit_Click(object sender, EventArgs e)
        {
            InMenu = true;
            BeginUpdate();
            var b = GetAppButton(sender);

            if (b.IsUrlButton)
            {
                var f = new WebLink(AppsConfig, b);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    SetAttrib(b.Node, "appname", b.AppName);
                    SetAttrib(b.Node, "url", b.Url);
                    SetAttrib(b.Node, "favicon", b.FavIcon);
                    SaveXml();
                }

                f.Dispose();
            }
            else if (b.IsFolderLinkButton)
            {
                var f = new FolderLink(AppsConfig, b);
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
                var f = new Folder(AppsConfig, b);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    SetAttrib(b.Node, "foldername", b.AppName);
                    SaveXml();
                }

                f.Dispose();
            }
            else
            {
                var f = new Forms.Properties(AppsConfig, b);
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    SetAttrib(b.Node, "appname", b.AppName);
                    SetAttrib(b.Node, "asadmin", b.AsAdmin);
                    SetAttrib(b.Node, "filename", b.FileName);
                    SetAttrib(b.Node, "fileiconpath", b.FileIconPath);
                    SetAttrib(b.Node, "fileiconindex", b.FileIconIndex);
                    SetAttrib(b.Node, "fileargs", b.FileArgs);
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
            var b = GetAppButton(sender);
            var pn = b.Node.ParentNode.ParentNode;
            pn.AppendChild(b.Node);
            MoveToCache(b, GetAttrib(pn, "id"));
            SaveXml();
            EndUpdate();
        }

        private void Menu_Opening(object sender, CancelEventArgs e)
        {
            var c = ((AppMenu)sender).SourceControl;
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

        private void OnDragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move;
        }

        #endregion

        #region Methods

        private AppButton AddAppButton(ButtonType buttonType, dynamic addTo)
        {
            var b = new AppButton(AppsConfig, buttonType);
            b.OnAppButtonClicked += ButtonClicked;
            b.OnAppButtonDropped += DropToButton;
            b.ContextMenuStrip = _menuRc;
            b.Padding = new Padding(0, 0, 0, 0);
            b.Margin = new Padding(0, 0, 0, 0);
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

        public void AddApp(AppButton appButton, int addAtIndex)
        {
            if (string.IsNullOrEmpty(appButton.AppId))
                appButton.AppId = Guid.NewGuid().ToString();

            if (appButton.Node == null)
            {
                var nodeSib = ((AppButton)Controls[addAtIndex]).Node;

                if (AppsConfig.ParseShortcuts && Misc.IsShortcut(appButton.FileName))
                {
                    appButton.AppName = Path.GetFileNameWithoutExtension(appButton.FileName);
                    appButton.ParseShortcut();
                }

                appButton.Node = _appsXml.CreateNode(XmlNodeType.Element, "APP", null);
                SetAttrib(appButton.Node, "id", appButton.AppId);
                SetAttrib(appButton.Node, "appname", appButton.AppName);
                SetAttrib(appButton.Node, "filename", appButton.FileName);
                SetAttrib(appButton.Node, "fileiconpath", appButton.FileIconPath);
                SetAttrib(appButton.Node, "fileiconindex", appButton.FileIconIndex);
                SetAttrib(appButton.Node, "fileargs", appButton.FileArgs);
                SetAttrib(appButton.Node, "fileworkingfolder", appButton.FileWorkingFolder);
                var parentNode = _currentParentNode ?? _appsNode;
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

        public void AddFolder(AppButton appButton, int addAtIndex)
        {
            appButton.AppId = Guid.NewGuid().ToString();
            appButton.Node = _appsXml.CreateNode(XmlNodeType.Element, "APP", null);
            var nodeSib = ((AppButton)Controls[addAtIndex]).Node;
            SetAttrib(appButton.Node, "id", appButton.AppId);
            SetAttrib(appButton.Node, "foldername", appButton.AppName);
            var parentNode = _currentParentNode ?? _appsNode;
            if (nodeSib == null)
                parentNode.AppendChild(appButton.Node);
            else
                parentNode.InsertAfter(appButton.Node, nodeSib);
            SaveXml();

            SetButtonDetails(appButton);
            AddToFolderCache(appButton.AppId);
            Controls.SetChildIndex(appButton, addAtIndex); // move button where we want it.
        }

        private void AddItems(XmlNode nodes)
        {
            var id = GetAttrib(nodes, "id");
            if (id == "")
                id = GetRootId;
            var ac = _folderCache.Find(x => x.FolderId == id);
            Controls.AddRange(ac.ToArray());
            ac.Clear();
        }

        public void AddFolderLink(AppButton appButton, int addAtIndex)
        {
            appButton.AppId = Guid.NewGuid().ToString();
            appButton.Node = _appsXml.CreateNode(XmlNodeType.Element, "APP", null);

            var nodeSib = ((AppButton)Controls[addAtIndex]).Node;
            SetAttrib(appButton.Node, "id", appButton.AppId);
            SetAttrib(appButton.Node, "folderlinkname", appButton.AppName);
            SetAttrib(appButton.Node, "folderlinkpath", appButton.FileName);
            var parentNode = _currentParentNode ?? _appsNode;
            if (nodeSib == null)
                parentNode.AppendChild(appButton.Node);
            else
                parentNode.InsertAfter(appButton.Node, nodeSib);

            SaveXml();
            SetButtonDetails(appButton);
            Controls.SetChildIndex(appButton, addAtIndex); // move button where we want it.
            appButton.Visible = true;
        }

        public void AddSeparator(int addAtIndex)
        {
            BeginUpdate();
            var appButton = AddAppButton(ButtonType.Separator, Controls);
            appButton.AppId = Guid.NewGuid().ToString();
            appButton.Node = _appsXml.CreateNode(XmlNodeType.Element, "APP", null);
            var nodeSib = ((AppButton)Controls[addAtIndex]).Node;
            SetAttrib(appButton.Node, "id", appButton.AppId);
            SetAttrib(appButton.Node, "separator", "Y");

            var parentNode = _currentParentNode ?? _appsNode;
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
            var f = _folderCache.Find(x => x.FolderId == id);
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
            var nodeSib = ((AppButton)Controls[addAtIndex]).Node;
            SetAttrib(appButton.Node, "id", appButton.AppId);
            SetAttrib(appButton.Node, "appname", appButton.AppName);
            SetAttrib(appButton.Node, "url", appButton.Url);
            SetAttrib(appButton.Node, "favicon", appButton.FavIcon);
            var parentNode = _currentParentNode ?? _appsNode;
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
                var s = filename;
                // some apps are tacking on the shell:{} guid information when it's not needed. So check to see if it's just the filename, if not parse it out.
                if (s.IndexOf("\\") > -1)
                {
                    var ilen = filename.Length - 1;
                    for (var i = ilen; i > 0; i--)
                        if (filename[i] == '\\')
                        {
                            s = filename.Substring(i + 1, filename.Length - i - 1);
                            break;
                        }
                }

                return s;
            }

            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                var s = e.Data.GetData(DataFormats.Text).ToString();
                if (Funcs.IsUrl(s))
                {
                    var b = AddAppButton(ButtonType.Url, Controls);
                    b.AppName = s;
                    b.Url = s;
                    AddUrl(b, toAppButton == null ? 0 : Controls.GetChildIndex(toAppButton));
                }
            }
            else
                // shell object?
            if (e.Data is IDataObject)
            {
                var shellObj = ShellObjectCollection.FromDataObject((IDataObject)e.Data);
                if (shellObj.Count > 0)
                {
                    BeginUpdate();
                    foreach (ShellObject shellFile in shellObj)
                        // File or Folder?
                        if (File.Exists(shellFile.ParsingName) || (!File.Exists(shellFile.ParsingName) &&
                                                                   Directory.Exists(shellFile.ParsingName)))
                        {
                            if (File.Exists(shellFile.ParsingName))
                            {
                                var b = AddAppButton(ButtonType.App, Controls);
                                b.AppName = string.IsNullOrEmpty(Funcs.GetFileInfo(shellFile.ParsingName).ProductName)
                                    ? Path.GetFileNameWithoutExtension(shellFile.ParsingName)
                                    : Funcs.GetFileInfo(shellFile.ParsingName).ProductName;
                                b.FileName = shellFile.ParsingName;
                                AddApp(b, toAppButton == null ? 0 : Controls.GetChildIndex(toAppButton));
                            }
                            else
                            {
                                var b = AddAppButton(ButtonType.FolderLink, Controls);
                                b.AppName = Path.GetFileName(shellFile.ParsingName);
                                b.FileName = shellFile.ParsingName;
                                AddFolderLink(b, toAppButton == null ? 0 : Controls.GetChildIndex(toAppButton));
                            }
                        }
                        else
                        {
                            var b = AddAppButton(ButtonType.App, Controls);
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
                var c = menu.SourceControl;
                return (AppButton)c.Parent.Parent.Parent; // Label > Panel > Panel > AppButton
            }
            else
            {
                var c = ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl;
                return (AppButton)c.Parent.Parent.Parent; // Label > Panel > Panel > AppButton
            }
        }

        private int GetAppButtonIndex(AppButton appButton)
        {
            return Controls.GetChildIndex(appButton);
        }

        private string GetAttrib(XmlNode xn, string attributeName)
        {
            if (xn.Attributes[attributeName] != null)
                return xn.Attributes[attributeName].Value;
            return "";
        }

        private XmlNode GetPrevNode(XmlNode node)
        {
            return node.PreviousSibling;
        }

        private XmlNode GetNextNode(XmlNode node)
        {
            return node.NextSibling;
        }

        public void GoBack()
        {
            if (_currentParentNode.ParentNode != null && _currentParentNode.ParentNode.Attributes["id"] != null &&
                !ModifierKeys.HasFlag(Keys.Control))
                LoadFolder(LookupFolderButton(_currentParentNode.ParentNode.Attributes["id"].Value));
            else
                LoadItems();
        }

        private void LoadCache()
        {
            void AddToCache(XmlNode nodes)
            {
                var id = GetAttrib(nodes, "id");
                if (id == "")
                    id = GetRootId;

                var ac = AddToFolderCache(id);

                foreach (XmlNode xn in nodes)
                    if (GetAttrib(xn, "folderlinkname") != "")
                    {
                        var b = AddAppButton(ButtonType.FolderLink, ac);
                        b.Node = xn;
                        SetButtonDetails(b);
                    }
                    else if (GetAttrib(xn, "foldername") != "")
                    {
                        var b = AddAppButton(ButtonType.Folder, ac);
                        b.Node = xn;
                        SetButtonDetails(b);
                        AddToCache(xn); // Recsurvise add contents of folder
                    }
                    else if (GetAttrib(xn, "separator") != "")
                    {
                        var b = AddAppButton(ButtonType.Separator, ac);
                        b.Node = xn;
                        SetButtonDetails(b);
                    }
                    else if (GetAttrib(xn, "appname") != "")
                    {
                        var b = AddAppButton(
                            string.IsNullOrEmpty(GetAttrib(xn, "url")) ? ButtonType.App : ButtonType.Url, ac);
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
            var doLoad = false;

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
            foreach (var a in _folderCache)
            foreach (AppButton b in a)
                if (b.AppId == id)
                    return b;
            return null;
        }

        private void MoveButton(AppButton fromButton, AppButton toButton)
        {
            BeginUpdate();
            var parentNode = _currentParentNode ?? _appsNode;
            if (toButton == null)
            {
                parentNode.AppendChild(fromButton.Node);
                Controls.SetChildIndex(fromButton, 0);
            }
            else
            {
                var t = GetAppButtonIndex(toButton);
                var f = GetAppButtonIndex(fromButton);

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
            var parentNode = toButton.Node;
            parentNode.AppendChild(fromButton.Node);
            MoveToCache(fromButton, GetAttrib(parentNode, "id"));
            SaveXml();
            EndUpdate();
        }

        private void MoveToCache()
        {
            var id = CurrentAppId;
            _currentParentNode = null;
            var f = AddToFolderCache(id);

            foreach (Control c in Controls) f.Add(c);
            Controls.Clear();
        }

        private void MoveToCache(AppButton appButton, string toId)
        {
            if (toId == "")
                toId = GetRootId;
            var ac = AddToFolderCache(toId);
            if (ac != null)
                ac.Insert(0, appButton);
            Controls.Remove(appButton);
        }

        private void SaveXml()
        {
            _appsXml.Save(_appsXmlFilePath);
        }

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
                appButton.AppId = GetAttrib(appButton.Node, "id");
                appButton.AppName = GetAttrib(appButton.Node, "appname");
                appButton.AsAdmin = GetAttrib(appButton.Node, "asadmin");
                appButton.FileName = GetAttrib(appButton.Node, "filename");
                appButton.FileArgs = GetAttrib(appButton.Node, "fileargs");
                appButton.FileIconIndex = GetAttrib(appButton.Node, "fileiconindex");
                appButton.FileIconPath = GetAttrib(appButton.Node, "fileiconpath");
                appButton.FileWorkingFolder = GetAttrib(appButton.Node, "fileworkingfolder");
                if (appButton.FileIconImage == null)
                    appButton.WatchForIconUpdate = true;
            }
            else if (appButton.ButtonType == ButtonType.Folder)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
                appButton.AppName = GetAttrib(appButton.Node, "foldername");
            }
            else if (appButton.ButtonType == ButtonType.FolderLink)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
                appButton.AppName = GetAttrib(appButton.Node, "folderlinkname");
                appButton.FileName = GetAttrib(appButton.Node, "folderlinkpath");
            }
            else if (appButton.ButtonType == ButtonType.Separator)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
            }
            else if (appButton.ButtonType == ButtonType.Url)
            {
                appButton.AppId = GetAttrib(appButton.Node, "id");
                appButton.AppName = GetAttrib(appButton.Node, "appname");
                appButton.Url = GetAttrib(appButton.Node, "url");
                appButton.FavIcon = GetAttrib(appButton.Node, "favicon");
            }
        }

        private void SetColors()
        {
            BackColor = AppsConfig.AppsBackColor;
        }

        #endregion
    }
}