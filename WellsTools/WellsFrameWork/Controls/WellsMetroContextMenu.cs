using Wells.WellsFramework;
using Wells.WellsFramework.Components;
using Wells.WellsFramework.Drawing;
using Wells.WellsFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Wells.WellsFramework.Controls
{
    public class WellsMetroContextMenu : ContextMenuStrip, IWellsMetroControl
    {
        #region Interface

        [Category("WellsMetro Appearance")]
        public event EventHandler<WellsMetroPaintEventArgs> CustomPaintBackground;
        protected virtual void OnCustomPaintBackground(WellsMetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintBackground != null)
            {
                CustomPaintBackground(this, e);
            }
        }

        [Category("WellsMetro Appearance")]
        public event EventHandler<WellsMetroPaintEventArgs> CustomPaint;
        protected virtual void OnCustomPaint(WellsMetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaint != null)
            {
                CustomPaint(this, e);
            }
        }

        [Category("WellsMetro Appearance")]
        public event EventHandler<WellsMetroPaintEventArgs> CustomPaintForeground;
        protected virtual void OnCustomPaintForeground(WellsMetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintForeground != null)
            {
                CustomPaintForeground(this, e);
            }
        }

        private WellsMetroColorStyle metroStyle = WellsMetroColorStyle.Default;
        [Category("WellsMetro Appearance")]
        [DefaultValue(WellsMetroColorStyle.Default)]
        public WellsMetroColorStyle Style
        {
            get
            {
                if (DesignMode || metroStyle != WellsMetroColorStyle.Default)
                {
                    return metroStyle;
                }

                if (StyleManager != null && metroStyle == WellsMetroColorStyle.Default)
                {
                    return StyleManager.Style;
                }
                if (StyleManager == null && metroStyle == WellsMetroColorStyle.Default)
                {
                    return WellsMetroColorStyle.Blue;
                }

                return metroStyle;
            }
            set { metroStyle = value; }
        }

        private WellsMetroThemeStyle metroTheme = WellsMetroThemeStyle.Default;
        [Category("WellsMetro Appearance")]
        [DefaultValue(WellsMetroThemeStyle.Default)]
        public WellsMetroThemeStyle Theme
        {
            get
            {
                if (DesignMode || metroTheme != WellsMetroThemeStyle.Default)
                {
                    return metroTheme;
                }

                if (StyleManager != null && metroTheme == WellsMetroThemeStyle.Default)
                {
                    return StyleManager.Theme;
                }
                if (StyleManager == null && metroTheme == WellsMetroThemeStyle.Default)
                {
                    return WellsMetroThemeStyle.Light;
                }

                return metroTheme;
            }
            set { metroTheme = value; }
        }

        private WellsMetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WellsMetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set { 
                metroStyleManager = value;
                settheme();
            }
        }

        private bool useCustomBackColor = false;
        [DefaultValue(false)]
        [Category("WellsMetro Appearance")]
        public bool UseCustomBackColor
        {
            get { return useCustomBackColor; }
            set { useCustomBackColor = value; }
        }

        private bool useCustomForeColor = false;
        [DefaultValue(false)]
        [Category("WellsMetro Appearance")]
        public bool UseCustomForeColor
        {
            get { return useCustomForeColor; }
            set { useCustomForeColor = value; }
        }

        private bool useStyleColors = false;
        [DefaultValue(false)]
        [Category("WellsMetro Appearance")]
        public bool UseStyleColors
        {
            get { return useStyleColors; }
            set { useStyleColors = value; }
        }

        [Browsable(false)]
        [Category("WellsMetro Behaviour")]
        [DefaultValue(false)]
        public bool UseSelectable
        {
            get { return GetStyle(ControlStyles.Selectable); }
            set { SetStyle(ControlStyles.Selectable, value); }
        }

        #endregion

        
        public WellsMetroContextMenu(IContainer Container) {
             if (Container != null)
            {
                Container.Add(this);
            }
        }

        private void settheme()
        {
            this.BackColor = WellsMetroPaint.BackColor.Form(Theme);
            this.ForeColor = WellsMetroPaint.ForeColor.Button.Normal(Theme);            
            this.Renderer = new WellsMetroCTXRenderer(Theme, Style);     
        }

        private class WellsMetroCTXRenderer : ToolStripProfessionalRenderer
        {
            public WellsMetroCTXRenderer(Wells.WellsFramework.WellsMetroThemeStyle Theme, WellsMetroColorStyle Style) : base(new contextcolors(Theme, Style)) { }
        }

        private class contextcolors : ProfessionalColorTable
        {
            WellsMetroThemeStyle _theme = WellsMetroThemeStyle.Light;
            WellsMetroColorStyle _style = WellsMetroColorStyle.Blue;

            public contextcolors(Wells.WellsFramework.WellsMetroThemeStyle Theme, WellsMetroColorStyle Style)
            {
                _theme = Theme;
                _style = Style;
            }

            public override Color MenuItemSelected
            {
                get { return WellsMetroPaint.GetStyleColor(_style); }
            }

            public override Color MenuBorder
            {
                get { return WellsMetroPaint.BackColor.Form(_theme); }
            }

            public override Color MenuItemBorder
            {
                get { return WellsMetroPaint.GetStyleColor(_style); }
            }

            public override Color ImageMarginGradientBegin
            {
                get { return WellsMetroPaint.BackColor.Form(_theme); }
            }

            public override Color ImageMarginGradientMiddle
            {
                get { return WellsMetroPaint.BackColor.Form(_theme); }
            }

            public override Color ImageMarginGradientEnd
            {
                get { return WellsMetroPaint.BackColor.Form(_theme); }
            }
        }
    }

    
}
