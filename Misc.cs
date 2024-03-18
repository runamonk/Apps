using System;
using System.IO;
using System.Windows.Forms;
using Apps.Forms;
using Shell32;
using Message = Apps.Forms.Message;

namespace Apps
{
    internal class Misc
    {
        public static bool IsShortcut(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLower();
            if (ext == ".lnk" || ext == ".url")
                return true;
            return false;
        }

        public static void ParseShortcut(string fileName, out string parsedFileName, out string parsedFileIcon,
            out string parsedFileIconIndex, out string parsedArgs, out string parsedWorkingFolder)
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
                var sFile = File.ReadAllLines(fileName);
                var iconFile = "";
                var url = "";
                var workingFolder = "";

                var urlString = "URL=";
                var iconFileString = "IconFile=";
                var wfString = "WorkingDirectory=";

                string GetValue(string s, string lookup)
                {
                    return s.Substring(s.IndexOf(lookup) + lookup.Length, s.Length - lookup.Length);
                }

                foreach (var s in sFile)
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
                var shell = new Shell();
                var lnkPath = shell.NameSpace(Path.GetDirectoryName(fileName));
                var linkItem = lnkPath.Items().Item(Path.GetFileName(fileName));
                var link = (ShellLinkObject)linkItem.GetLink;
                string linkIcon;
                link.GetIconLocation(out linkIcon);

                if (link.Target.Path != "")
                    parsedFileName = link.Target.Path.Contains("!")
                        ? "shell:AppsFolder\\" + link.Target.Path
                        : link.Target.Path;
                else
                    parsedFileName = "";

                if (!File.Exists(parsedFileName) && parsedFileName.Contains("Program Files (x86)"))
                {
                    var s = parsedFileName.Replace("Program Files (x86)", "Program Files");
                    if (File.Exists(s))
                        parsedFileName = s;
                }

                parsedArgs = link.Arguments;
                parsedFileIcon = linkIcon;
                parsedFileIconIndex = "";
                parsedWorkingFolder = link.WorkingDirectory;
            }
        }

        public static void ShowMessage(Config myConfig, string caption, string messageText)
        {
            var f = new Message(myConfig);
            f.ShowAsDialog(caption, messageText);
        }

        public static DialogResult ConfirmDialog(Config myConfig, ConfirmButtons buttons, string caption,
            string messageText)
        {
            var f = new Confirm(myConfig);
            return f.ShowAsDialog(buttons, caption, messageText);
        }
    }
}