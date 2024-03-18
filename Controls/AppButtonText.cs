using System;
using System.Drawing;
using System.Windows.Forms;

namespace Apps.Controls
{
    public class AppButtonText : Label
    {
        #region Privates

        private readonly Config _appsConfig;

        #endregion

        public AppButtonText(Config myConfig)
        {
            _appsConfig = myConfig;
            _appsConfig.ConfigChanged += ConfigChanged;
        }

        #region Properties

        public bool IsSeparator { get; set; }

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
            {
                base.OnPaint(e);
            }
            else
            {
                //Pen pen = new Pen(ControlPaint.Dark(AppsConfig.AppsFontColor, 50)); 
                var pen = new Pen(_appsConfig.AppsFontColor);
                var pt1 = new PointF(0, Height / 2);
                var pt2 = new PointF(Width, Height / 2);
                e.Graphics.DrawLine(pen, pt1, pt2);
            }
        }

        #endregion
    }
}