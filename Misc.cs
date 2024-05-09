using System;
using System.IO;
using System.Windows.Forms;
using Apps.Forms;
using Shell32;
using Utility;
using Folder = Shell32.Folder;
using Message = Apps.Forms.Message;

namespace Apps
{
    internal class Misc
    {
        public static DialogResult ConfirmDialog(Config myConfig, ConfirmButtons buttons, string caption, string messageText)
        {
            Confirm f = new Confirm(myConfig);
            return f.ShowAsDialog(buttons, caption, messageText);
        }

        public static bool IsShortcut(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            if (ext == ".lnk" || ext == ".url")
                return true;
            return false;
        }

        public static void ParseShortcut(string fileName, out string parsedFileName, out string parsedFileIcon, out string parsedFileIconIndex, out string parsedArgs, out string parsedWorkingFolder)
        {
            parsedFileName = "";
            parsedFileIcon = "";
            parsedFileIconIndex = "";
            parsedArgs = "";
            parsedWorkingFolder = "";

            if (!IsShortcut(fileName))
                throw new Exception("File must be a .lnk or .url file.");
            if (!File.Exists(fileName))
                throw new Exception(fileName + " not found.");

            if (Path.GetExtension(fileName).ToLower() == ".url")
            {
                string[] sFile = File.ReadAllLines(fileName);
                string iconFile = "";
                string url = "";
                string workingFolder = "";

                string urlString = "URL=";
                string iconFileString = "IconFile=";
                string wfString = "WorkingDirectory=";

                string GetValue(string s, string lookup) { return s.Substring(s.IndexOf(lookup) + lookup.Length, s.Length - lookup.Length); }

                foreach (string s in sFile)
                {
                    if (s.IndexOf(urlString) > -1)
                        url = GetValue(s, urlString);
                    else if (s.IndexOf(iconFileString) > -1)
                        iconFile = GetValue(s, iconFileString);
                    else if (s.IndexOf(wfString) > -1)
                        workingFolder = GetValue(s, wfString);

                    if (url != "" && iconFile != "")
                        break;
                }

                if (url != "")
                {
                    parsedFileName = url;
                    parsedFileIcon = iconFile;
                    parsedWorkingFolder = workingFolder;
                }
            }
            else
            {
                Shell shell = new Shell();
                Folder lnkPath = shell.NameSpace(Path.GetDirectoryName(fileName));
                FolderItem linkItem = lnkPath.Items().Item(Path.GetFileName(fileName));
                ShellLinkObject link = (ShellLinkObject)linkItem.GetLink;

                int i = link.GetIconLocation(out string linkIcon);
                // had some weird icon locations.
                if (i > 999 || i < -999) i = 0;
                parsedFileIconIndex = (i < 0 ? i * -1 : i).ToString();

                if (link.Target.Path != "")
                    parsedFileName = link.Target.Path.StartsWith("::") ? "shell:" + link.Target.Path : link.Target.Path.Contains("!") ? "shell:AppsFolder\\" + link.Target.Path : link.Target.Path;
                else
                    parsedFileName = "";

                if (!File.Exists(parsedFileName) && parsedFileName.Contains("Program Files (x86)"))
                {
                    string s = parsedFileName.Replace("Program Files (x86)", "Program Files");
                    if (File.Exists(s))
                        parsedFileName = s;
                }

                parsedArgs = link.Arguments;
                parsedFileIcon = Funcs.ParseEnvironmentVars(linkIcon == "" ? parsedFileName : linkIcon);
                parsedWorkingFolder = link.WorkingDirectory;
            }
        }

        public static void ShowMessage(Config myConfig, string caption, string messageText)
        {
            Message f = new Message(myConfig);
            f.ShowAsDialog(caption, messageText);
        }
    }
}