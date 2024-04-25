using System;
using System.Drawing;
using System.Windows.Forms;

namespace Apps.Controls
{
    public class AppButtonText : Label
    {
        private readonly Config _appsConfig;

        public AppButtonText(Config myConfig)
        {
            _appsConfig = myConfig;
            _appsConfig.ConfigChanged += ConfigChanged;
        }

        public bool IsSeparator { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!IsSeparator)
            {
                base.OnPaint(e);
            }
            else
            {
                //Pen pen = new Pen(ControlPaint.Dark(AppsConfig.AppsFontColor, 50)); 
                Pen pen = new Pen(_appsConfig.AppsFontColor);
                PointF pt1 = new PointF(0,     Height / 2);
                PointF pt2 = new PointF(Width, Height / 2);
                e.Graphics.DrawLine(pen, pt1, pt2);
            }
        }

        private void ConfigChanged(object sender, EventArgs e)
        {
            Invalidate(); // force a repaint.
        }
    }
}