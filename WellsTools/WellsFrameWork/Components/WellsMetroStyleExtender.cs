using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Wells.WellsFramework.Drawing;
using Wells.WellsFramework.Interfaces;

namespace Wells.WellsFramework.Components
{
	[ProvideProperty("ApplyMetroTheme", typeof(Control))]
    public sealed class WellsMetroStyleExtender : Component, IExtenderProvider, IWellsMetroComponent
    {
        #region Interface

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WellsMetroColorStyle Style
        {
            get { throw new NotSupportedException(); }
            set { }
        }

        private WellsMetroThemeStyle metroTheme = WellsMetroThemeStyle.Default;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
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
                    return WellsMetroDefaults.Theme;
                }

                return metroTheme;
            }
            set 
            { 
                metroTheme = value;
                UpdateTheme();
            }
        }

        private WellsMetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WellsMetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set 
            { 
                metroStyleManager = value;
                UpdateTheme();
            }
        }

        #endregion

        #region Fields

        private readonly List<Control> extendedControls = new List<Control>();

        #endregion

        #region Constructor

        public WellsMetroStyleExtender()
        {
        
        }

        public WellsMetroStyleExtender(IContainer parent)
            : this()
        {
            if (parent != null)
            {
                parent.Add(this);
            }
        }

        #endregion

        #region Management Methods

        private void UpdateTheme()
        {
            Color backColor = WellsMetroPaint.BackColor.Form(Theme);
            Color foreColor = WellsMetroPaint.ForeColor.Label.Normal(Theme);

            foreach (Control ctrl in extendedControls)
            {
                if (ctrl != null)
                {
                    try
                    {
                        ctrl.BackColor = backColor;
                    }
                    catch { }

                    try
                    {
                        ctrl.ForeColor = foreColor;
                    }
                    catch { }
                }
            }
        }

        #endregion

        #region IExtenderProvider

        bool IExtenderProvider.CanExtend(object target)
		{
		    return target is Control && !(target is IWellsMetroControl || target is IWellsMetroForm);
		}

        [DefaultValue(false)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        [Description("Apply WellsMetro Theme BackColor and ForeColor.")]
        public bool GetApplyMetroTheme(Control control)
		{
		    return control != null && extendedControls.Contains(control);
		}

        public void SetApplyMetroTheme(Control control, bool value)
        {
            if (control == null)
            {
                return;
            }

            if (extendedControls.Contains(control))
            {
                if (!value)
                {
                    extendedControls.Remove(control);
                }
            }
            else
            {
                if (value)
                {
                    extendedControls.Add(control);
                }
            }
        }

        #endregion
    }
}
