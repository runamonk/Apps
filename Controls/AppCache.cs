using System.Collections.Generic;
using System.Windows.Forms;

namespace Apps.Controls
{
    internal class AppCache : List<Control>
    {
        public AppCache(string id)
        {
            FolderId = id;
        }

        public string FolderId { get; set; }
    }
}