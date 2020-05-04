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

// Based on original work by 
// (c) Mick Doherty / Oscar Londono
// http://dotnetrix.co.uk/tabcontrol.htm
// http://www.pcreview.co.uk/forums/adding-custom-tabpages-design-time-t2904262.html
// http://www.codeproject.com/Articles/12185/A-NET-Flat-TabControl-CustomDraw
// http://www.codeproject.com/Articles/278/Fully-owner-drawn-tab-control

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;

using Wells.WellsFramework.Components;
using Wells.WellsFramework.Drawing;
using Wells.WellsFramework.Interfaces;
using Wells.WellsFramework.Native;

namespace Wells.WellsFramework.Controls
{
    #region WellsMetroTabPageCollection

    [ToolboxItem(false)]
    [Editor(typeof(Design.Controls.WellsMetroTabPageCollectionEditor), typeof(UITypeEditor))]
    //[Editor("Wells.WellsFramework.Design.WellsMetroTabPageCollectionEditor, " + AssemblyRef.MetroFrameworkDesignSN, typeof(UITypeEditor))]

    public class WellsMetroTabPageCollection : TabControl.TabPageCollection
    {
        public WellsMetroTabPageCollection(WellsMetroTabControl owner) : base(owner)
        { }
    }

    #endregion

    [Designer(typeof(Design.Controls.WellsMetroTabControlDesigner), typeof(System.ComponentModel.Design.IRootDesigner))]
    //[Designer("Wells.WellsFramework.Design.Controls.WellsMetroTabControlDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    [ToolboxBitmap(typeof(TabControl))]
    public class WellsMetroTabControl : TabControl, IWellsMetroControl
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

        #endregion

        #region Fields

        private SubClass scUpDown = null;
        private bool bUpDown = false;

        private const int TabBottomBorderHeight = 3;

        private WellsMetroTabControlSize metroLabelSize = WellsMetroTabControlSize.Medium;
        [DefaultValue(WellsMetroTabControlSize.Medium)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroTabControlSize FontSize
        {
            get { return metroLabelSize; }
            set { metroLabelSize = value; }
        }

        private WellsMetroTabControlWeight metroLabelWeight = WellsMetroTabControlWeight.Light;
        [DefaultValue(WellsMetroTabControlWeight.Light)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroTabControlWeight FontWeight
        {
            get { return metroLabelWeight; }
            set { metroLabelWeight = value; }
        }

        private ContentAlignment textAlign = ContentAlignment.MiddleLeft;
        [DefaultValue(ContentAlignment.MiddleLeft)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public ContentAlignment TextAlign
        {
            get
            {
                return textAlign;
            }
            set
            {
                textAlign = value;
            }
        }
        
        [Editor(typeof(Design.Controls.WellsMetroTabPageCollectionEditor), typeof(UITypeEditor))]
        //[Editor("MetroFramework.Design.MetroTabPageCollectionEditor, " + AssemblyRef.MetroFrameworkDesignSN, typeof(UITypeEditor))]
        //[Editor("Wells.WellsFramework.Design.WellsMetroTabPageCollectionEditor, " + AssemblyRef.MetroFrameworkDesignSN, typeof(UITypeEditor))]
        public new TabPageCollection TabPages
        {
            get
            {
                return base.TabPages;
            }
        }


        private bool isMirrored;
        [DefaultValue(false)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public new bool IsMirrored
        {
            get
            {
                return isMirrored;
            }
            set
            {
                if (isMirrored == value)
                {
                    return;
                }
                isMirrored = value;
                UpdateStyles();
            }
        }

        #endregion

        #region Constructor

        public WellsMetroTabControl()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            Padding = new Point(6, 8);
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
            for (var index = 0; index < TabPages.Count; index++)
            {
                if (index != SelectedIndex)
                {
                    DrawTab(index, e.Graphics);
                }
            }
            if (SelectedIndex <= -1)
            {
                return;
            }

            DrawTabBottomBorder(SelectedIndex, e.Graphics);
            DrawTab(SelectedIndex, e.Graphics);
            DrawTabSelected(SelectedIndex, e.Graphics);

            OnCustomPaintForeground(new WellsMetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
        }

        private void DrawTabBottomBorder(int index, Graphics graphics)
        {
            using (Brush bgBrush = new SolidBrush(WellsMetroPaint.BorderColor.TabControl.Normal(Theme)))
            {
                Rectangle borderRectangle = new Rectangle(DisplayRectangle.X, GetTabRect(index).Bottom + 2 - TabBottomBorderHeight, DisplayRectangle.Width, TabBottomBorderHeight);
                graphics.FillRectangle(bgBrush, borderRectangle);
            }
        }

        private void DrawTabSelected(int index, Graphics graphics)
        {
            using (Brush selectionBrush = new SolidBrush(WellsMetroPaint.GetStyleColor(Style)))
            {
                Rectangle selectedTabRect = GetTabRect(index);
                Rectangle borderRectangle = new Rectangle(selectedTabRect.X + ((index == 0) ? 2 : 0), GetTabRect(index).Bottom + 2 - TabBottomBorderHeight, selectedTabRect.Width + ((index == 0) ? 0 : 2), TabBottomBorderHeight);
                graphics.FillRectangle(selectionBrush, borderRectangle);
            }
        }

        private Size MeasureText(string text)
        {
            Size preferredSize;
            using (Graphics g = CreateGraphics())
            {
                Size proposedSize = new Size(int.MaxValue, int.MaxValue);
                preferredSize = TextRenderer.MeasureText(g, text, WellsMetroFonts.TabControl(metroLabelSize, metroLabelWeight),
                                                         proposedSize,
                                                         WellsMetroPaint.GetTextFormatFlags(TextAlign) |
                                                         TextFormatFlags.NoPadding);
            }
            return preferredSize;
        }

        private void DrawTab(int index, Graphics graphics)
        {
            Color foreColor;
            Color backColor = BackColor;

            if (!useCustomBackColor)
            {
                backColor = WellsMetroPaint.BackColor.Form(Theme);
            }

            TabPage tabPage = TabPages[index];
            Rectangle tabRect = GetTabRect(index);

            if (!Enabled)
            {
                foreColor = WellsMetroPaint.ForeColor.Label.Disabled(Theme);
            }
            else
            {
                if (useCustomForeColor)
                {
                    foreColor = DefaultForeColor;
                }
                else
                {
                    foreColor = !useStyleColors ? WellsMetroPaint.ForeColor.TabControl.Normal(Theme) : WellsMetroPaint.GetStyleColor(Style);
                }
            }

            if (index == 0)
            {
                tabRect.X = DisplayRectangle.X;
            }

            Rectangle bgRect = tabRect;

            tabRect.Width += 20;

            using (Brush bgBrush = new SolidBrush(backColor))
            {
                graphics.FillRectangle(bgBrush, bgRect);
            }

            TextRenderer.DrawText(graphics, tabPage.Text, WellsMetroFonts.TabControl(metroLabelSize, metroLabelWeight),
                                  tabRect, foreColor, backColor, WellsMetroPaint.GetTextFormatFlags(TextAlign));
        }

        [SecuritySafeCritical]
        private void DrawUpDown(Graphics graphics)
        {
            Color backColor = Parent != null ? Parent.BackColor : WellsMetroPaint.BackColor.Form(Theme);

            Rectangle borderRect = new Rectangle();
            WinApi.GetClientRect(scUpDown.Handle, ref borderRect);

            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            graphics.Clear(backColor);

            using (Brush b = new SolidBrush(WellsMetroPaint.BorderColor.TabControl.Normal(Theme)))
            {
                GraphicsPath gp = new GraphicsPath(FillMode.Winding);
                PointF[] pts = { new PointF(6, 6), new PointF(16, 0), new PointF(16, 12) };
                gp.AddLines(pts);

                graphics.FillPath(b, gp);

                gp.Reset();

                PointF[] pts2 = { new PointF(borderRect.Width - 15, 0), new PointF(borderRect.Width - 5, 6), new PointF(borderRect.Width - 15, 12) };
                gp.AddLines(pts2);

                graphics.FillPath(b, gp);

                gp.Dispose();
            }
        }

        #endregion

        #region Overridden Methods

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        [SecuritySafeCritical]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (!DesignMode)
            {
                WinApi.ShowScrollBar(Handle, (int)WinApi.ScrollBar.SB_BOTH, 0);
            }
        }

        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                const int WS_EX_LAYOUTRTL = 0x400000;
                const int WS_EX_NOINHERITLAYOUT = 0x100000;
                var cp = base.CreateParams;
                if (isMirrored)
                {
                    cp.ExStyle = cp.ExStyle | WS_EX_LAYOUTRTL | WS_EX_NOINHERITLAYOUT;
                }
                return cp;
            }
        }

        private new Rectangle GetTabRect(int index)
        {
            if (index < 0)
                return new Rectangle();

            Rectangle baseRect = base.GetTabRect(index);
            return baseRect;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (SelectedIndex != -1)
            {
                if (!TabPages[SelectedIndex].Focused)
                {
                    bool subControlFocused = false;
                    foreach (Control ctrl in TabPages[SelectedIndex].Controls)
                    {
                        if (ctrl.Focused)
                        {
                            subControlFocused = true;
                            return;
                        }
                    }

                    if (!subControlFocused)
                    {
                        TabPages[SelectedIndex].Select();
                        TabPages[SelectedIndex].Focus();
                    }
                }
            }
            
            base.OnMouseWheel(e);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.OnFontChanged(EventArgs.Empty);
            FindUpDown();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
 	         base.OnControlAdded(e);
             FindUpDown();
             UpdateUpDown();
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
 	        base.OnControlRemoved(e);
            FindUpDown();
            UpdateUpDown();
        }

        protected override void  OnSelectedIndexChanged(EventArgs e)
        {
 	        base.OnSelectedIndexChanged(e);
            UpdateUpDown();
            Invalidate();
        }

        //send font change to properly resize tab page header rects
        //http://www.codeproject.com/Articles/13305/Painting-Your-Own-Tabs?msg=2707590#xx2707590xx
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private const int WM_SETFONT = 0x30;
        private const int WM_FONTCHANGE = 0x1d;

        [SecuritySafeCritical]
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            IntPtr hFont = WellsMetroFonts.TabControl(metroLabelSize, metroLabelWeight).ToHfont();
            SendMessage(this.Handle, WM_SETFONT, hFont, (IntPtr)(-1));
            SendMessage(this.Handle, WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero);
            this.UpdateStyles();
        }
        #endregion

        #region Helper Methods

        [SecuritySafeCritical]
        private void FindUpDown()
        {
            if (!DesignMode)
            {
                bool bFound = false;

                IntPtr pWnd = WinApi.GetWindow(Handle, WinApi.GW_CHILD);

                while (pWnd != IntPtr.Zero)
                {
                    char[] className = new char[33];

                    int length = WinApi.GetClassName(pWnd, className, 32);

                    string s = new string(className, 0, length);

                    if (s == "msctls_updown32")
                    {
                        bFound = true;

                        if (!bUpDown)
                        {
                            this.scUpDown = new SubClass(pWnd, true);
                            this.scUpDown.SubClassedWndProc += new SubClass.SubClassWndProcEventHandler(scUpDown_SubClassedWndProc);

                            bUpDown = true;
                        }
                        break;
                    }

                    pWnd = WinApi.GetWindow(pWnd, WinApi.GW_HWNDNEXT);
                }

                if ((!bFound) && (bUpDown))
                    bUpDown = false;
            }
        }

        [SecuritySafeCritical]
        private void UpdateUpDown()
        {
            if (bUpDown && !DesignMode)
            {
                if (WinApi.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rect = new Rectangle();
                    WinApi.GetClientRect(scUpDown.Handle, ref rect);
                    WinApi.InvalidateRect(scUpDown.Handle, ref rect, true);
                }
            }
        }

        [SecuritySafeCritical]
        private int scUpDown_SubClassedWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WinApi.Messages.WM_PAINT:

                    IntPtr hDC = WinApi.GetWindowDC(scUpDown.Handle);

                    Graphics g = Graphics.FromHdc(hDC);

					DrawUpDown(g);

					g.Dispose();

                    WinApi.ReleaseDC(scUpDown.Handle, hDC);

                    m.Result = IntPtr.Zero;

                    Rectangle rect = new Rectangle();

                    WinApi.GetClientRect(scUpDown.Handle, ref rect);
                    WinApi.ValidateRect(scUpDown.Handle, ref rect);

                    return 1;
            }

            return 0;
        }

        #endregion

    }
}
