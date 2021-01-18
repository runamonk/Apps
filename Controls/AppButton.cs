using Apps.Controls;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Utility;

namespace Apps
{

    public partial class AppButton : Button
    {
        private Config AppsConfig { get; set; }
        private bool IsMenuButton = false;
        public string FileName { get; set; }
        public Image FullImage { get; set; }
        public string FullText { get; set; }
        public delegate void AppButtonClickedHandler(AppButton Button);
        public event AppButtonClickedHandler OnAppButtonClicked;
        
        public AppButton(Config myConfig, bool isMenuButton = false)
        {
            FlatAppearance.BorderSize = 0;
            FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            TextAlign = ContentAlignment.TopLeft;
            UseCompatibleTextRendering = true; // keeps text from being wrapped prematurely.
            AutoEllipsis = false;
            UseMnemonic = false;
            AutoSize = false;
            IsMenuButton = isMenuButton;

            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += new EventHandler(ConfigChanged);
            SetColors();
        }

        private void ConfigChanged(object sender, EventArgs e)
        {
            SetColors();
        }

        // Stops the black default border from being displayed on button when the preview form is shown.
        public override void NotifyDefault(bool value)
        {
            base.NotifyDefault(false);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (OnAppButtonClicked != null)
                OnAppButtonClicked(this);
        }
               
        private void SetColors()
        {
            FlatAppearance.BorderColor = BackColor;
            if (IsMenuButton)
            {
                BackColor = AppsConfig.MenuButtonColor;
                ForeColor = AppsConfig.MenuFontColor;
            }
            else
            {
                BackColor = AppsConfig.AppsRowBackColor;
                ForeColor = AppsConfig.AppsFontColor;
            }
        }

        protected override void OnPaint(PaintEventArgs pea)
        {
            base.OnPaint(pea);

            if (!IsMenuButton)
            {
                // Defines pen 
                Pen pen = new Pen(ControlPaint.Dark(AppsConfig.AppsRowBackColor, 25));
                               
                PointF pt1 = new PointF(0F, Height-1);
                PointF pt2 = new PointF(0F, Height);
                pea.Graphics.DrawLine(pen, pt1, pt2);
            }
        }

        protected override bool ShowFocusCues
        {
            get {
                return false;
            }
        }

    } 
}
