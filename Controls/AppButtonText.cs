using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Apps.Controls
{
    public partial class AppButtonText : Label
    {
        public AppButtonText (Config myConfig)
        {
            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += new EventHandler(ConfigChanged);
        }

        #region Properties       
        public bool IsSeparator { get; set; }
        #endregion

        #region Privates
        Config AppsConfig;
        #endregion

        #region Methods
        private void ConfigChanged(object sender, EventArgs e)
        {
            Invalidate(); // force a repaint.
        }
        #endregion

        #region Overrides
        protected override void OnPaint(PaintEventArgs e)
        {            
            if (!IsSeparator)
                base.OnPaint(e);
            else
            {
                //Pen pen = new Pen(ControlPaint.Dark(AppsConfig.AppsFontColor, 50)); 
                Pen pen = new Pen(AppsConfig.AppsFontColor); 
                PointF pt1 = new PointF(0, Height / 2);
                PointF pt2 = new PointF(Width, Height / 2);                
                e.Graphics.DrawLine(pen, pt1, pt2);
            }
        }
        #endregion
    }
}
