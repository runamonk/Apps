using System;
using System.Drawing;
using System.Windows.Forms;

namespace Apps.Controls
{
    public class AppMenu : ContextMenuStrip

    {
        public AppMenu(Config myConfig)
        {
            DoubleBuffered = true;
            AppsConfig = myConfig;
            AppsConfig.ConfigChanged += ConfigChanged;
            SetColors();
        }

        private Config AppsConfig { get; }

        private void SetColors()
        {
            BackColor = AppsConfig.MenuBackColor;
            ForeColor = AppsConfig.MenuFontColor;
            Renderer = null;
            Renderer = new CustomToolstripRenderer(AppsConfig);
        }

        private void ConfigChanged(object sender, EventArgs e)
        {
            SetColors();
        }
    }

    public class CustomToolstripRenderer : ToolStripProfessionalRenderer
    {
        public CustomToolstripRenderer(Config myConfig) : base(new CustomColors(myConfig))
        {
        }
    }

    public class CustomColors : ProfessionalColorTable
    {
        private readonly Config _config;

        public CustomColors(Config myConfig)
        {
            _config = myConfig;
        }

        public override Color ButtonSelectedBorder => Color.Transparent;

        public override Color ImageMarginGradientBegin => _config.MenuBackColor;

        public override Color ImageMarginGradientMiddle => _config.MenuBackColor;

        public override Color ImageMarginGradientEnd => _config.MenuBackColor;

        public override Color MenuItemSelected => _config.MenuSelectedColor;

        public override Color MenuItemBorder => _config.MenuSelectedColor;

        public override Color MenuBorder => _config.MenuBorderColor;

        public override Color CheckSelectedBackground => _config.MenuSelectedColor;

        public override Color CheckBackground => _config.MenuBackColor;

        public override Color CheckPressedBackground => _config.MenuBackColor;
    }
}