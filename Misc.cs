using Apps.Forms;
using System;
using System.IO;

namespace Apps
{
    class Misc
    {
        public static Boolean IsShortcut(string FileName)
        {
            string ext = Path.GetExtension(FileName).ToLower();
            if ((ext == ".lnk") || (ext == ".url"))
                return true;
            else
                return false;
        }

        public static void ParseShortcut(string FileName, out string ParsedFileName, out string ParsedFileIcon, out string ParsedFileIconIndex, out string ParsedArgs, out string ParsedWorkingFolder)
        {
            ParsedFileName = "";
            ParsedFileIcon = "";
            ParsedFileIconIndex = "";
            ParsedArgs = "";
            ParsedWorkingFolder = "";

            if (!IsShortcut(FileName))
                throw new Exception("File must be a .lnk or .url file.");
            if (!File.Exists(FileName))
                throw new Exception(FileName + " not found.");

            if (Path.GetExtension(FileName).ToLower() == ".url")
            {
                string[] sFile = File.ReadAllLines(FileName);
                string IconFile = "";
                string URL = "";
                string WorkingFolder = "";

                string urlString = "URL=";
                string IconFileString = "IconFile=";
                string wfString = "WorkingDirectory=";

                string GetValue(string s, string lookup)
                {
                    return s.Substring(s.IndexOf(lookup) + lookup.Length, s.Length - lookup.Length);
                }

                foreach (string s in sFile)
                {
                    if (s.IndexOf(urlString) > -1)
                        URL = GetValue(s, urlString);
                    else
                    if (s.IndexOf(IconFileString) > -1)
                        IconFile = GetValue(s, IconFileString);
                    else
                    if (s.IndexOf(wfString) > -1)
                        WorkingFolder = GetValue(s, wfString);

                    if ((URL != "") && (IconFile != ""))
                        break;
                }

                if (URL != "")
                {
                    ParsedFileName = URL;
                    ParsedFileIcon = IconFile;
                    ParsedWorkingFolder = WorkingFolder;
                }
            }
            else
            {
                var shell = new Shell32.Shell();
                var lnkPath = shell.NameSpace(System.IO.Path.GetDirectoryName(FileName));
                var linkItem = lnkPath.Items().Item(System.IO.Path.GetFileName(FileName));
                var link = (Shell32.ShellLinkObject)linkItem.GetLink;
                string linkIcon;
                link.GetIconLocation(out linkIcon);

                if (link.Target.Path != "")
                    ParsedFileName = link.Target.Path.Contains("!") ? "shell:AppsFolder\\" + link.Target.Path : link.Target.Path;
                else
                    ParsedFileName = "";

                if ((!File.Exists(ParsedFileName)) && (ParsedFileName.Contains("Program Files (x86)")))
                {
                    string s = ParsedFileName.Replace("Program Files (x86)", "Program Files");
                    if (File.Exists(s))
                        ParsedFileName = s;
                }

                ParsedArgs = link.Arguments;
                ParsedFileIcon = linkIcon;
                ParsedFileIconIndex = "";
                ParsedWorkingFolder = link.WorkingDirectory;
            }
        }

        public static void ShowMessage(Config myConfig, string Caption, string MessageText)
        {
            Forms.Message f = new Forms.Message(myConfig);
            f.ShowAsDialog(Caption, MessageText);
        }

        public static System.Windows.Forms.DialogResult ConfirmDialog(Config myConfig, ConfirmButtons Buttons, string Caption, string MessageText)
        {
            Confirm f = new Confirm(myConfig);
            return f.ShowAsDialog(Buttons, Caption, MessageText);
        }

    }
}
