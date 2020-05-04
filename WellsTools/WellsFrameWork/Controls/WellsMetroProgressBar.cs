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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Wells.WellsFramework.Components;
using Wells.WellsFramework.Drawing;
using Wells.WellsFramework.Interfaces;

namespace Wells.WellsFramework.Controls
{
    [Designer(typeof(Design.Controls.WellsMetroProgressBarDesigner), typeof(System.ComponentModel.Design.IRootDesigner))]
    //[Designer("Wells.WellsFramework.Design.Controls.WellsMetroProgressBarDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    [ToolboxBitmap(typeof(ProgressBar))]
    public class WellsMetroProgressBar : ProgressBar, IWellsMetroControl
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
        public new WellsMetroColorStyle Style
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
        [Browsable(false)]
        [DefaultValue(false)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UseCustomForeColor
        {
            get { return useCustomForeColor; }
            set { useCustomForeColor = value; }
        }

        private bool useStyleColors = true;
        [Browsable(false)]
        [DefaultValue(true)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        private WellsMetroProgressBarSize metroLabelSize = WellsMetroProgressBarSize.Medium;
        [DefaultValue(WellsMetroProgressBarSize.Medium)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroProgressBarSize FontSize
        {
            get { return metroLabelSize; }
            set { metroLabelSize = value; }
        }

        private WellsMetroProgressBarWeight metroLabelWeight = WellsMetroProgressBarWeight.Light;
        [DefaultValue(WellsMetroProgressBarWeight.Light)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroProgressBarWeight FontWeight
        {
            get { return metroLabelWeight; }
            set { metroLabelWeight = value; }
        }

        private ContentAlignment textAlign = ContentAlignment.MiddleRight;
        [DefaultValue(ContentAlignment.MiddleRight)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public ContentAlignment TextAlign
        {
            get { return textAlign; }
            set { textAlign = value; }
        }

        private bool hideProgressText = true;
        [DefaultValue(true)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public bool HideProgressText
        {
            get { return hideProgressText; }
            set { hideProgressText = value; }
        }

        private ProgressBarStyle progressBarStyle = ProgressBarStyle.Continuous;
        [DefaultValue(ProgressBarStyle.Continuous)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public ProgressBarStyle ProgressBarStyle
        {
            get { return progressBarStyle; }
            set { progressBarStyle = value; }
        }

        public new int Value
        {
            get { return base.Value; }
            set { if (value > Maximum) return; base.Value = value; Invalidate(); }
        }

        [Browsable(false)]
        public double ProgressTotalPercent
        {
            get { return ((1 - (double)(Maximum - Value) / Maximum) * 100); }
        }

        [Browsable(false)]
        public double ProgressTotalValue
        {
            get { return (1 - (double)(Maximum - Value) / Maximum); }
        }

        [Browsable(false)]
        public string ProgressPercentText
        {
            get { return (string.Format("{0}%", Math.Round(ProgressTotalPercent))); }
        }

        private double ProgressBarWidth
        {
            get { return (((double)Value / Maximum) * ClientRectangle.Width); }
        }

        private int ProgressBarMarqueeWidth
        {
            get { return (ClientRectangle.Width / 3); }
        }

        #endregion

        #region Constructor

        public WellsMetroProgressBar()
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
                    if (!Enabled)
                    {
                        backColor = WellsMetroPaint.BackColor.ProgressBar.Bar.Disabled(Theme);
                    }
                    else
                    {
                        backColor = WellsMetroPaint.BackColor.ProgressBar.Bar.Normal(Theme);
                    }
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
            if (progressBarStyle == ProgressBarStyle.Continuous)
            {
                if (!DesignMode) StopTimer();

                DrawProgressContinuous(e.Graphics);
            }
            else if (progressBarStyle == ProgressBarStyle.Blocks)
            {
                if (!DesignMode) StopTimer();

                DrawProgressContinuous(e.Graphics);
            }
            else if (progressBarStyle == ProgressBarStyle.Marquee)
            {
                if (!DesignMode && Enabled) StartTimer();
                if (!Enabled) StopTimer();

                if (Value == Maximum)
                {
                    StopTimer();
                    DrawProgressContinuous(e.Graphics);
                }
                else
                {
                    DrawProgressMarquee(e.Graphics);
                }
            }

            DrawProgressText(e.Graphics);

            using (Pen p = new Pen(WellsMetroPaint.BorderColor.ProgressBar.Normal(Theme)))
            {
                Rectangle borderRect = new Rectangle(0, 0, Width - 1, Height - 1);
                e.Graphics.DrawRectangle(p, borderRect);
            }

            OnCustomPaintForeground(new WellsMetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
        }

        private void DrawProgressContinuous(Graphics graphics)
        {
            graphics.FillRectangle(WellsMetroPaint.GetStyleBrush(Style), 0, 0, (int)ProgressBarWidth, ClientRectangle.Height);
        }

        private int marqueeX = 0;

        private void DrawProgressMarquee(Graphics graphics)
        {
            graphics.FillRectangle(WellsMetroPaint.GetStyleBrush(Style), marqueeX, 0, ProgressBarMarqueeWidth, ClientRectangle.Height);
        }

        private void DrawProgressText(Graphics graphics)
        {
            if (HideProgressText) return;

            Color foreColor;

            if (!Enabled)
            {
                foreColor = WellsMetroPaint.ForeColor.ProgressBar.Disabled(Theme);
            }
            else
            {
                foreColor = WellsMetroPaint.ForeColor.ProgressBar.Normal(Theme);
            }
           
            TextRenderer.DrawText(graphics, ProgressPercentText, WellsMetroFonts.ProgressBar(metroLabelSize, metroLabelWeight), ClientRectangle, foreColor, WellsMetroPaint.GetTextFormatFlags(TextAlign));
        }

        #endregion

        #region Overridden Methods

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize;
            base.GetPreferredSize(proposedSize);

            using (var g = CreateGraphics())
            {
                proposedSize = new Size(int.MaxValue, int.MaxValue);
                preferredSize = TextRenderer.MeasureText(g, ProgressPercentText, WellsMetroFonts.ProgressBar(metroLabelSize, metroLabelWeight), proposedSize, WellsMetroPaint.GetTextFormatFlags(TextAlign));
            }

            return preferredSize;
        }

        #endregion

        #region Private Methods

        private Timer marqueeTimer;
        private bool marqueeTimerEnabled
        {
            get
            {
                return marqueeTimer != null && marqueeTimer.Enabled;
            }
        }

        private void StartTimer()
        {
            if (marqueeTimerEnabled) return;

            if (marqueeTimer == null)
            {
                marqueeTimer = new Timer {Interval = 10};
                marqueeTimer.Tick += marqueeTimer_Tick;
            }

            marqueeX = -ProgressBarMarqueeWidth;
    
            marqueeTimer.Stop();
            marqueeTimer.Start();

            marqueeTimer.Enabled = true;

            Invalidate();
        }
        private void StopTimer()
        {
            if (marqueeTimer == null) return;

            marqueeTimer.Stop();

            Invalidate();
        }

        private void marqueeTimer_Tick(object sender, EventArgs e)
        {
            marqueeX++;

            if (marqueeX > ClientRectangle.Width)
            {
                marqueeX = -ProgressBarMarqueeWidth;
            }

            Invalidate();
        }

        #endregion
    }
}
