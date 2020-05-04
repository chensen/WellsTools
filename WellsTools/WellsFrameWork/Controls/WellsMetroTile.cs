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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Wells.WellsFramework.Drawing;
using Wells.WellsFramework.Components;
using Wells.WellsFramework.Interfaces;

namespace Wells.WellsFramework.Controls
{
    [Designer(typeof(Design.Controls.WellsMetroTileDesigner), typeof(System.ComponentModel.Design.IRootDesigner))]
    //[Designer("Wells.WellsFramework.Design.Controls.WellsMetroTileDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    [ToolboxBitmap(typeof(Button))]
    public class WellsMetroTile : Button, IContainerControl, IWellsMetroControl
    {
        #region Interface

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

        private Control activeControl = null;
        [Browsable(false)]
        public Control ActiveControl
        {
            get { return activeControl; }
            set { activeControl = value; }
        }

        public bool ActivateControl(Control ctrl)
        {
            if (Controls.Contains(ctrl))
            {
                ctrl.Select();
                activeControl = ctrl;
                return true;
            }

            return false;
        }

        #endregion

        #region Fields

        private bool paintTileCount = true;
        [DefaultValue(true)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public bool PaintTileCount
        {
            get { return paintTileCount; }
            set { paintTileCount = value; }
        }

        private int tileCount = 0;
        [DefaultValue(0)]
        public int TileCount
        {
            get { return tileCount; }
            set { tileCount = value; }
        }

        [DefaultValue(ContentAlignment.BottomLeft)]
        public new ContentAlignment TextAlign
        {
            get { return base.TextAlign; }
            set { base.TextAlign = value; }
        }

        private Image tileImage = null;
        [DefaultValue(null)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public Image TileImage
        {
            get { return tileImage; }
            set { tileImage = value; }
        }

        private bool useTileImage = false;
        [DefaultValue(false)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public bool UseTileImage
        {
            get { return useTileImage; }
            set { useTileImage = value; }
        }

        private ContentAlignment tileImageAlign = ContentAlignment.TopLeft;
        [DefaultValue(ContentAlignment.TopLeft)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public ContentAlignment TileImageAlign
        {
            get { return tileImageAlign; }
            set { tileImageAlign = value; }
        }

        private WellsMetroTileTextSize tileTextFontSize = WellsMetroTileTextSize.Medium;
        [DefaultValue(WellsMetroTileTextSize.Medium)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroTileTextSize TileTextFontSize
        {
            get { return tileTextFontSize; }
            set { tileTextFontSize = value; Refresh(); }
        }

        private WellsMetroTileTextWeight tileTextFontWeight = WellsMetroTileTextWeight.Light;
        [DefaultValue(WellsMetroTileTextWeight.Light)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroTileTextWeight TileTextFontWeight
        {
            get { return tileTextFontWeight; }
            set { tileTextFontWeight = value; Refresh(); }
        }

        private bool isHovered = false;
        private bool isPressed = false;
        private bool isFocused = false;

        #endregion

        #region Constructor

        public WellsMetroTile()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            TextAlign = ContentAlignment.BottomLeft;
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
                    backColor = WellsMetroPaint.GetStyleColor(Style);
                }

                if (backColor.A == 255)
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

            if (isHovered && !isPressed && Enabled)
            {
                foreColor = WellsMetroPaint.ForeColor.Tile.Hover(Theme);
            }
            else if (isHovered && isPressed && Enabled)
            {
                foreColor = WellsMetroPaint.ForeColor.Tile.Press(Theme);
            }
            else if (!Enabled)
            {
                foreColor = WellsMetroPaint.ForeColor.Tile.Disabled(Theme);
            }
            else
            {
                foreColor = WellsMetroPaint.ForeColor.Tile.Normal(Theme);
            }

            if (useCustomForeColor)
            {
                foreColor = ForeColor;
            }

            if (isPressed)
            {

            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            if (useTileImage)
            {
                if (tileImage != null)
                {
                    Rectangle imageRectangle;
                    switch (tileImageAlign)
                    {
                        case ContentAlignment.BottomLeft:
                            imageRectangle = new Rectangle(new Point(0, Height - TileImage.Height), new System.Drawing.Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.BottomCenter:
                            imageRectangle = new Rectangle(new Point(Width / 2 - TileImage.Width / 2, Height - TileImage.Height), new System.Drawing.Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.BottomRight:
                            imageRectangle = new Rectangle(new Point(Width - TileImage.Width, Height - TileImage.Height), new System.Drawing.Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.MiddleLeft:
                            imageRectangle = new Rectangle(new Point(0, Height / 2 - TileImage.Height / 2), new System.Drawing.Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.MiddleCenter:
                            imageRectangle = new Rectangle(new Point(Width / 2 - TileImage.Width / 2, Height / 2 - TileImage.Height / 2), new System.Drawing.Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.MiddleRight:
                            imageRectangle = new Rectangle(new Point(Width - TileImage.Width, Height / 2 - TileImage.Height / 2), new System.Drawing.Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.TopLeft:
                            imageRectangle = new Rectangle(new Point(0, 0), new System.Drawing.Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.TopCenter:
                            imageRectangle = new Rectangle(new Point(Width / 2 - TileImage.Width / 2, 0), new System.Drawing.Size(TileImage.Width, TileImage.Height));
                            break;

                        case ContentAlignment.TopRight:
                            imageRectangle = new Rectangle(new Point(Width - TileImage.Width, 0), new System.Drawing.Size(TileImage.Width, TileImage.Height));
                            break;

                        default:
                            imageRectangle = new Rectangle(new Point(0, 0), new System.Drawing.Size(TileImage.Width, TileImage.Height));
                            break;
                    }

                    e.Graphics.DrawImage(TileImage, imageRectangle);
                }
            }

            if (TileCount > 0 && paintTileCount)
            {
                Size countSize = TextRenderer.MeasureText(TileCount.ToString(), WellsMetroFonts.TileCount);

                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                TextRenderer.DrawText(e.Graphics, TileCount.ToString(), WellsMetroFonts.TileCount, new Point(Width - countSize.Width, 0), foreColor);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            }

            Size textSize = TextRenderer.MeasureText(Text, WellsMetroFonts.Tile(tileTextFontSize, tileTextFontWeight));

            TextFormatFlags flags = WellsMetroPaint.GetTextFormatFlags(TextAlign) | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis;
            Rectangle textRectangle = ClientRectangle;
            if (isPressed)
            {
                textRectangle.Inflate(-2, -2);
            }

            TextRenderer.DrawText(e.Graphics, Text, WellsMetroFonts.Tile(tileTextFontSize, tileTextFontWeight), textRectangle, foreColor, flags);

            if (false && isFocused)
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
        }

        #endregion

        #region Focus Methods

        protected override void OnGotFocus(EventArgs e)
        {
            isFocused = true;
            isHovered = true;
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
            isHovered = true;
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
            //Remove this code cause this prevents the focus color
            //isHovered = false;
            //isPressed = false;
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
            //This will check if control got the focus
            //If not thats the only it will remove the focus color
            if (!isFocused)
            {
                isHovered = false;
            }
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
