/**
 * WellsFramework - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2011 Sven Walter, http://github.com/viperneo
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

using Wells.WellsFramework.Components;
using Wells.WellsFramework.Drawing;
using Wells.WellsFramework.Interfaces;

namespace Wells.WellsFramework.Controls
{
    [Designer(typeof(Design.Controls.WellsMetroLinkDesigner), typeof(System.ComponentModel.Design.IRootDesigner))]
    //[Designer("Wells.WellsFramework.Design.Controls.WellsMetroLinkDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    [ToolboxBitmap(typeof(LinkLabel))]
    [DefaultEvent("Click")]
    public class WellsMetroLink : Button, IWellsMetroControl
    {
        #region Interface

        private bool displayFocusRectangle = false;
        [DefaultValue(false)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public bool DisplayFocus
        {
            get { return displayFocusRectangle; }
            set { displayFocusRectangle = value; }
        }

        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<WellsMetroPaintEventArgs> CustomPaintBackground;
        protected virtual void OnCustomPaintBackground(WellsMetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintBackground != null)
            {
                CustomPaintBackground(this, e);
            }
        }

        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<WellsMetroPaintEventArgs> CustomPaint;
        protected virtual void OnCustomPaint(WellsMetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaint != null)
            {
                CustomPaint(this, e);
            }
        }

        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<WellsMetroPaintEventArgs> CustomPaintForeground;
        protected virtual void OnCustomPaintForeground(WellsMetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintForeground != null)
            {
                CustomPaintForeground(this, e);
            }
        }

        private WellsMetroColorStyle metroStyle = WellsMetroColorStyle.Default;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
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
                    return WellsMetroDefaults.Style;
                }

                return metroStyle;
            }
            set { metroStyle = value; }
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
            set { metroTheme = value; }
        }

        private WellsMetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WellsMetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set { metroStyleManager = value; }
        }

        private bool useCustomBackColor = false;
        [DefaultValue(false)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomBackColor
        {
            get { return useCustomBackColor; }
            set { useCustomBackColor = value; }
        }

        private bool useCustomForeColor = false;
        [DefaultValue(false)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomForeColor
        {
            get { return useCustomForeColor; }
            set { useCustomForeColor = value; }
        }

        private bool useStyleColors = false;
        [DefaultValue(false)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public bool UseStyleColors
        {
            get { return useStyleColors; }
            set { useStyleColors = value; }
        }

        [Browsable(false)]
        [Category(WellsMetroDefaults.PropertyCategory.Behaviour)]
        [DefaultValue(false)]
        public bool UseSelectable
        {
            get { return GetStyle(ControlStyles.Selectable); }
            set { SetStyle(ControlStyles.Selectable, value); }
        }

        #endregion

        #region Fields

        private WellsMetroLinkSize metroLinkSize = WellsMetroLinkSize.Small;
        [DefaultValue(WellsMetroLinkSize.Small)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroLinkSize FontSize
        {
            get { return metroLinkSize; }
            set { metroLinkSize = value; }
        }

        private WellsMetroLinkWeight metroLinkWeight = WellsMetroLinkWeight.Bold;
        [DefaultValue(WellsMetroLinkWeight.Bold)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroLinkWeight FontWeight
        {
            get { return metroLinkWeight; }
            set { metroLinkWeight = value; }
        }

        [Browsable(false)]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        private bool isHovered = false;
        private bool isPressed = false;
        private bool isFocused = false;

        #endregion

        #region Constructor

        public WellsMetroLink()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
        }

        #endregion

        #region Paint Methods

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                Color backColor = BackColor;

                if (!useCustomBackColor)
                {
                    backColor = WellsMetroPaint.BackColor.Form(Theme);
                }

                if (backColor.A == 255 && BackgroundImage == null)
                {
                    e.Graphics.Clear(backColor);
                    return;
                }

                base.OnPaintBackground(e);

                OnCustomPaintBackground(new WellsMetroPaintEventArgs(backColor, Color.Empty, e.Graphics));
            }
            catch
            {
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (GetStyle(ControlStyles.AllPaintingInWmPaint))
                {
                    OnPaintBackground(e);
                }

                OnCustomPaint(new WellsMetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
                OnPaintForeground(e);
            }
            catch
            {
                Invalidate();
            }
        }

        protected virtual void OnPaintForeground(PaintEventArgs e)
        {
            Color foreColor;

            if (useCustomForeColor)
                foreColor = ForeColor;
            else
            {
                if (isHovered && !isPressed && Enabled)
                {
                    foreColor = WellsMetroPaint.ForeColor.Link.Hover(Theme);
                }
                else if (isHovered && isPressed && Enabled)
                {
                    foreColor = WellsMetroPaint.ForeColor.Link.Press(Theme);
                }
                else if (!Enabled)
                {
                    foreColor = WellsMetroPaint.ForeColor.Link.Disabled(Theme);
                }
                else
                {
                    foreColor = !useStyleColors ? WellsMetroPaint.ForeColor.Link.Normal(Theme) : WellsMetroPaint.GetStyleColor(Style);
                }
            }

            TextRenderer.DrawText(e.Graphics, Text, WellsMetroFonts.Link(metroLinkSize, metroLinkWeight), ClientRectangle, foreColor, WellsMetroPaint.GetTextFormatFlags(TextAlign));

            OnCustomPaintForeground(new WellsMetroPaintEventArgs(Color.Empty, foreColor, e.Graphics));

            if (displayFocusRectangle && isFocused)
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
        }

        #endregion

        #region Focus Methods

        protected override void OnGotFocus(EventArgs e)
        {
            isFocused = true;
            Invalidate();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLostFocus(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            isFocused = true;
            Invalidate();

            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLeave(e);
        }

        #endregion

        #region Keyboard Methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                isHovered = true;
                isPressed = true;
                Invalidate();
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnKeyUp(e);
        }

        #endregion

        #region Mouse Methods

        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isPressed = false;
            Invalidate();

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isHovered = false;
            Invalidate();

            base.OnMouseLeave(e);
        }

        #endregion

        #region Overridden Methods

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        #endregion
    }
}
