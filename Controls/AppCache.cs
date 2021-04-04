using System.Collections.Generic;
using System.Windows.Forms;

namespace Apps.Controls
{
    partial class AppCache : List<Control>
    {
        public string FolderId { get; set; }
        public AppCache(string id)
        {
            FolderId = id;
        }
    }

}
