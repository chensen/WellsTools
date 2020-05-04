﻿/**
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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Windows.Forms;

using Wells.WellsFramework.Components;
using Wells.WellsFramework.Drawing;
using Wells.WellsFramework.Interfaces;
using Wells.WellsFramework.Native;

namespace Wells.WellsFramework.Forms
{
    #region Enums

    public enum WellsMetroFormTextAlign
    {
        Left,
        Center,
        Right
    }

    public enum WellsMetroFormShadowType
    {
        None,
        Flat,
        DropShadow,
        SystemShadow,
        AeroShadow
    }

    public enum WellsMetroFormBorderStyle
    {
        None,
        FixedSingle
    }

    public enum WellsBackLocation
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    #endregion

    public class WellsMetroForm : Form, IWellsMetroForm, IDisposable
    {
        #region Interface

        private WellsMetroColorStyle metroStyle = WellsMetroColorStyle.Blue;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroColorStyle Style
        {
            get
            {
                if (StyleManager != null)
                    return StyleManager.Style;

                return metroStyle;
            }
            set { metroStyle = value; }
        }

        private WellsMetroThemeStyle metroTheme = WellsMetroThemeStyle.Light;

        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroThemeStyle Theme
        {
            get
            {
                if (StyleManager != null)
                    return StyleManager.Theme;

                return metroTheme;
            }
            set { metroTheme = value; }
        }

        private WellsMetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        public WellsMetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set { metroStyleManager = value; }
        }

        #endregion

        #region Fields

        private WellsMetroFormTextAlign textAlign = WellsMetroFormTextAlign.Left;
        [Browsable(true)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroFormTextAlign TextAlign
        {
            get { return textAlign; }
            set { textAlign = value; }
        }

        [Browsable(false)]
        public override Color BackColor
        {
            get { return WellsMetroPaint.BackColor.Form(Theme); }
        }

        private WellsMetroFormBorderStyle formBorderStyle = WellsMetroFormBorderStyle.None;
        [DefaultValue(WellsMetroFormBorderStyle.None)]
        [Browsable(true)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroFormBorderStyle BorderStyle
        {
            get { return formBorderStyle; }
            set { formBorderStyle = value; }
        }

        private bool isMovable = true;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public bool Movable
        {
            get { return isMovable; }
            set { isMovable = value; }
        }

        public new Padding Padding
        {
            get { return base.Padding; }
            set
            {
                value.Top = Math.Max(value.Top, DisplayHeader ? 60 : 30);
                base.Padding = value;
            }
        }

        protected override Padding DefaultPadding
        {
            get { return new Padding(20, DisplayHeader ? 60 : 20, 20, 20); }
        }

        private bool displayHeader = true;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(true)]
        public bool DisplayHeader
        {
            get { return displayHeader; }
            set 
            {
                if (value != displayHeader)
                {
                    Padding p = base.Padding;
                    p.Top += value ? 30 : -30;
                    base.Padding = p;
                }
                displayHeader = value;
            }
        }

        private bool isResizable = true;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public bool Resizable
        {
            get { return isResizable; }
            set { isResizable = value; }
        }

        private WellsMetroFormShadowType shadowType = WellsMetroFormShadowType.Flat;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(WellsMetroFormShadowType.Flat)]
        public WellsMetroFormShadowType ShadowType
        {
            get { return IsMdiChild ? WellsMetroFormShadowType.None : shadowType; }
            set { shadowType = value; }
        }

        [Browsable(false)]
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            set { base.FormBorderStyle = value; }
        }

        public new Form MdiParent
        {
            get { return base.MdiParent; }
            set
            {
                if (value != null)
                {
                    RemoveShadow();
                    shadowType = WellsMetroFormShadowType.None;
                }

                base.MdiParent = value;
            }
        }

        private const int borderWidth = 5;

        private Bitmap _image = null;
        private Image backImage;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(null)]
        public Image BackImage
        {
            get { return backImage; }
            set
            {
                backImage = value;
                if(value != null) _image = ApplyInvert(new Bitmap(value));
                Refresh();
            }
        }

        private Padding backImagePadding;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public Padding BackImagePadding
        {
            get { return backImagePadding; }
            set
            {
                backImagePadding = value;
                Refresh();
            }
        }

        private int backMaxSize;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public int BackMaxSize
        {
            get { return backMaxSize; }
            set
            {
                backMaxSize = value;
                Refresh();
            }
        }

        private WellsBackLocation backLocation;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(WellsBackLocation.TopLeft)]
        public WellsBackLocation BackLocation
        {
            get { return backLocation; }
            set
            {
                backLocation = value;
                Refresh();
            }
        }

        private bool _imageinvert;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(true)]
        public bool ApplyImageInvert
        {
            get { return _imageinvert; }
            set
            {
                _imageinvert = value;
                Refresh();
            }
        }
        #endregion

        #region Constructor

        public WellsMetroForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            FormBorderStyle = FormBorderStyle.None;
            Name = "WellsMetroForm";
            StartPosition = FormStartPosition.CenterScreen;
            TransparencyKey = Color.Lavender;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveShadow();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Paint Methods

        public Bitmap ApplyInvert(Bitmap bitmapImage)
        {
            byte A, R, G, B;
            Color pixelColor;

            for (int y = 0; y < bitmapImage.Height; y++)
            {
                for (int x = 0; x < bitmapImage.Width; x++)
                {
                    pixelColor = bitmapImage.GetPixel(x, y);
                    A = pixelColor.A;
                    R = (byte)(255 - pixelColor.R);
                    G = (byte)(255 - pixelColor.G);
                    B = (byte)(255 - pixelColor.B);

                    if (R <= 0) R= 17;
                    if (G <= 0) G= 17;
                    if (B <= 0) B= 17;
                    //bitmapImage.SetPixel(x, y, Color.FromArgb((int)A, (int)R, (int)G, (int)B));
                    bitmapImage.SetPixel(x, y, Color.FromArgb((int)R, (int)G, (int)B));
                }
            }

            return bitmapImage;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Color backColor = WellsMetroPaint.BackColor.Form(Theme);
            Color foreColor = WellsMetroPaint.ForeColor.Title(Theme);

            e.Graphics.Clear(backColor);

            using (SolidBrush b = WellsMetroPaint.GetStyleBrush(Style))
            {
                Rectangle topRect = new Rectangle(0, 0, Width, borderWidth);
                e.Graphics.FillRectangle(b, topRect);
            }

            if (BorderStyle != WellsMetroFormBorderStyle.None)
            {
                Color c = WellsMetroPaint.BorderColor.Form(Theme);

                using (Pen pen = new Pen(c))
                {
                    e.Graphics.DrawLines(pen, new[]
                        {
                            new Point(0, borderWidth),
                            new Point(0, Height - 1),
                            new Point(Width - 1, Height - 1),
                            new Point(Width - 1, borderWidth)
                        });
                }
            }

            if (backImage != null && backMaxSize != 0)
            {
                Image img = WellsMetroImage.ResizeImage(backImage, new Rectangle(0, 0, backMaxSize, backMaxSize));
                if (_imageinvert)
                {
                    img = WellsMetroImage.ResizeImage((Theme == WellsMetroThemeStyle.Dark) ? _image : backImage, new Rectangle(0, 0, backMaxSize, backMaxSize));
                }

                switch (backLocation)
                {
                    case WellsBackLocation.TopLeft:
                        e.Graphics.DrawImage(img, 0 + backImagePadding.Left, 0 + backImagePadding.Top);
                        break;
                    case WellsBackLocation.TopRight:
                        e.Graphics.DrawImage(img, ClientRectangle.Right - (backImagePadding.Right + img.Width), 0 + backImagePadding.Top);
                        break;
                    case WellsBackLocation.BottomLeft:
                        e.Graphics.DrawImage(img, 0 + backImagePadding.Left, ClientRectangle.Bottom - (img.Height + backImagePadding.Bottom));
                        break;
                    case WellsBackLocation.BottomRight:
                        e.Graphics.DrawImage(img, ClientRectangle.Right - (backImagePadding.Right + img.Width),
                                             ClientRectangle.Bottom - (img.Height + backImagePadding.Bottom));
                        break;
                }
            }

            if (displayHeader)
            {
                Rectangle bounds = new Rectangle(20, 20, ClientRectangle.Width - 2 * 20, 40);
                TextFormatFlags flags = TextFormatFlags.EndEllipsis | GetTextFormatFlags();
                TextRenderer.DrawText(e.Graphics, Text, WellsMetroFonts.Title, bounds, foreColor, flags);
            }

            if (Resizable && (SizeGripStyle == SizeGripStyle.Auto || SizeGripStyle == SizeGripStyle.Show))
            {
                using (SolidBrush b = new SolidBrush(WellsMetroPaint.ForeColor.Button.Disabled(Theme)))
                {
                    Size resizeHandleSize = new Size(2, 2);
                    e.Graphics.FillRectangles(b, new Rectangle[] {
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-10,ClientRectangle.Height-10), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-10,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-10), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-14,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-14), resizeHandleSize)
                    });
                }
            }
        }

        private TextFormatFlags GetTextFormatFlags()
        {
            switch (TextAlign)
            {
                case WellsMetroFormTextAlign.Left: return TextFormatFlags.Left;
                case WellsMetroFormTextAlign.Center: return TextFormatFlags.HorizontalCenter;
                case WellsMetroFormTextAlign.Right: return TextFormatFlags.Right;
            }

            throw new InvalidOperationException();
        }

        #endregion

        #region Management Methods

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!(this is WellsMetroTaskWindow))
                WellsMetroTaskWindow.ForceClose();

            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            RemoveShadow();

            base.OnClosed(e);
        }

        [SecuritySafeCritical]
        public bool FocusMe()
        {
            return WinApi.SetForegroundWindow(Handle);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode) return;

            switch (StartPosition)
            {
                case FormStartPosition.CenterParent:
                    CenterToParent();
                    break;
                case FormStartPosition.CenterScreen:
                    if (IsMdiChild)
                    {
                        CenterToParent(); 
                    }
                    else 
                    {
                        CenterToScreen();
                    }
                    break;
            }

            RemoveCloseButton();
        
            if (ControlBox)
            {
                AddWindowButton(WindowButtons.Close);

                if (MaximizeBox)
                    AddWindowButton(WindowButtons.Maximize);

                if (MinimizeBox)
                    AddWindowButton(WindowButtons.Minimize);

                UpdateWindowButtonPosition();
            }

            CreateShadow();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (shadowType == WellsMetroFormShadowType.AeroShadow &&
                IsAeroThemeEnabled() && IsDropShadowSupported())
            {
                int val = 2;
                DwmApi.DwmSetWindowAttribute(Handle, 2, ref val, 4);
                var m = new DwmApi.MARGINS
                {
                    cyBottomHeight = 1,
                    cxLeftWidth = 0,
                    cxRightWidth = 0,
                    cyTopHeight = 0
                };

                DwmApi.DwmExtendFrameIntoClientArea(Handle, ref m);
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            Invalidate();
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            UpdateWindowButtonPosition();
        }

        protected override void WndProc(ref Message m)
        {
            if (DesignMode)
            {
                base.WndProc(ref m);
                return;
            }

            switch (m.Msg)
            {
                case (int)WinApi.Messages.WM_SYSCOMMAND:
                    int sc = m.WParam.ToInt32() & 0xFFF0;
                    switch (sc)
                    {
                        case (int)WinApi.Messages.SC_MOVE: 
                            if (!Movable) return; 
                            break;
                        case (int)WinApi.Messages.SC_MAXIMIZE: 
                            break;
                        case (int)WinApi.Messages.SC_RESTORE:
                            break;
                    }
                    break;

                case (int)WinApi.Messages.WM_NCLBUTTONDBLCLK:
                case (int)WinApi.Messages.WM_LBUTTONDBLCLK:
                    if (!MaximizeBox) return;
                    break;

                case (int)WinApi.Messages.WM_NCHITTEST:
                    WinApi.HitTest ht = HitTestNCA(m.HWnd, m.WParam, m.LParam);
                    if (ht != WinApi.HitTest.HTCLIENT)
                    {
                        m.Result = (IntPtr)ht;
                        return;
                    }
                    break;

                case (int)WinApi.Messages.WM_DWMCOMPOSITIONCHANGED:
                    break;
            }

            base.WndProc(ref m);

            switch (m.Msg)
            {
                case (int)WinApi.Messages.WM_GETMINMAXINFO:
                    OnGetMinMaxInfo(m.HWnd, m.LParam);
                    break;
            }
        }

        [SecuritySafeCritical]
        private unsafe void OnGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            WinApi.MINMAXINFO* pmmi = (WinApi.MINMAXINFO*)lParam;

            Screen s = Screen.FromHandle(hwnd);
            pmmi->ptMaxSize.x = s.WorkingArea.Width;
            pmmi->ptMaxSize.y = s.WorkingArea.Height;
            pmmi->ptMaxPosition.x = Math.Abs(s.WorkingArea.Left - s.Bounds.Left);
            pmmi->ptMaxPosition.y = Math.Abs(s.WorkingArea.Top - s.Bounds.Top);

            //if (MinimumSize.Width > 0) pmmi->ptMinTrackSize.x = MinimumSize.Width;
            //if (MinimumSize.Height > 0) pmmi->ptMinTrackSize.y = MinimumSize.Height;
            //if (MaximumSize.Width > 0) pmmi->ptMaxTrackSize.x = MaximumSize.Width;
            //if (MaximumSize.Height > 0) pmmi->ptMaxTrackSize.y = MaximumSize.Height;
        }

        private WinApi.HitTest HitTestNCA(IntPtr hwnd, IntPtr wparam, IntPtr lparam)
        {
            //Point vPoint = PointToClient(new Point((int)lparam & 0xFFFF, (int)lparam >> 16 & 0xFFFF));
            //Point vPoint = PointToClient(new Point((Int16)lparam, (Int16)((int)lparam >> 16)));
            Point vPoint = new Point((Int16)lparam, (Int16)((int)lparam >> 16));
            int vPadding = Math.Max(Padding.Right, Padding.Bottom);

            if (Resizable)
            {
                if (RectangleToScreen(new Rectangle(ClientRectangle.Width - vPadding, ClientRectangle.Height - vPadding, vPadding, vPadding)).Contains(vPoint))
                    return WinApi.HitTest.HTBOTTOMRIGHT;
            }

          if (RectangleToScreen(new Rectangle(borderWidth, borderWidth, ClientRectangle.Width - 2 * borderWidth, 50)).Contains(vPoint))
                return WinApi.HitTest.HTCAPTION;

            return WinApi.HitTest.HTCLIENT;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left && Movable)
            {
                if (WindowState == FormWindowState.Maximized) return;
                if (Width - borderWidth > e.Location.X && e.Location.X > borderWidth && e.Location.Y > borderWidth)
                {
                    MoveControl();                    
                }
            }
            
        }

        [SecuritySafeCritical]
        private void MoveControl()
        {
            WinApi.ReleaseCapture();
            WinApi.SendMessage(Handle, (int)WinApi.Messages.WM_NCLBUTTONDOWN, (int)WinApi.HitTest.HTCAPTION, 0);
        }

        [SecuritySafeCritical]
        private static bool IsAeroThemeEnabled()
        {
            if (Environment.OSVersion.Version.Major <= 5) return false;

            bool aeroEnabled;
            DwmApi.DwmIsCompositionEnabled(out aeroEnabled);
            return aeroEnabled;
        }

        private static bool IsDropShadowSupported()
        {
            return Environment.OSVersion.Version.Major > 5 && SystemInformation.IsDropShadowEnabled;
        }

        #endregion

        #region Window Buttons

        private enum WindowButtons
        {
            Minimize,
            Maximize,
            Close
        }

        private Dictionary<WindowButtons, WellsMetroFormButton> windowButtonList;

        private void AddWindowButton(WindowButtons button)
        {
            if (windowButtonList == null)
                windowButtonList = new Dictionary<WindowButtons, WellsMetroFormButton>();

            if (windowButtonList.ContainsKey(button))
                return;

            WellsMetroFormButton newButton = new WellsMetroFormButton();

            if (button == WindowButtons.Close)
            {
                newButton.Text = "r";
            }
            else if (button == WindowButtons.Minimize)
            {
                newButton.Text = "0";
            }
            else if (button == WindowButtons.Maximize)
            {
                if (WindowState == FormWindowState.Normal)
                    newButton.Text = "1";
                else
                    newButton.Text = "2";
            }

            newButton.Style = Style;
            newButton.Theme = Theme;
            newButton.Tag = button;
            newButton.Size = new Size(25, 20);
            newButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            newButton.TabStop = false; //remove the form controls from the tab stop
            newButton.Click += WindowButton_Click;
            Controls.Add(newButton);

            windowButtonList.Add(button, newButton);
        }

        private void WindowButton_Click(object sender, EventArgs e)
        {
            var btn = sender as WellsMetroFormButton;
            if (btn != null)
            {
                var btnFlag = (WindowButtons)btn.Tag;
                if (btnFlag == WindowButtons.Close)
                {
                    Close();
                }
                else if (btnFlag == WindowButtons.Minimize)
                {
                    WindowState = FormWindowState.Minimized;
                }
                else if (btnFlag == WindowButtons.Maximize)
                {
                    if (WindowState == FormWindowState.Normal)
                    {
                        WindowState = FormWindowState.Maximized;
                        btn.Text = "2";
                    }
                    else
                    {
                        WindowState = FormWindowState.Normal;
                        btn.Text = "1";
                    }
                }
            }
        }

        private void UpdateWindowButtonPosition()
        {
            if (!ControlBox) return;

            Dictionary<int, WindowButtons> priorityOrder = new Dictionary<int, WindowButtons>(3) { {0, WindowButtons.Close}, {1, WindowButtons.Maximize}, {2, WindowButtons.Minimize} };

            Point firstButtonLocation = new Point(ClientRectangle.Width - borderWidth - 25, borderWidth);
            int lastDrawedButtonPosition = firstButtonLocation.X - 25;

            WellsMetroFormButton firstButton = null;

            if (windowButtonList.Count == 1)
            {
                foreach (KeyValuePair<WindowButtons, WellsMetroFormButton> button in windowButtonList)
                {
                    button.Value.Location = firstButtonLocation;
                }
            }
            else
            {
                foreach (KeyValuePair<int, WindowButtons> button in priorityOrder)
                {
                    bool buttonExists = windowButtonList.ContainsKey(button.Value);

                    if (firstButton == null && buttonExists)
                    {
                        firstButton = windowButtonList[button.Value];
                        firstButton.Location = firstButtonLocation;
                        continue;
                    }

                    if (firstButton == null || !buttonExists) continue;

                    windowButtonList[button.Value].Location = new Point(lastDrawedButtonPosition, borderWidth);
                    lastDrawedButtonPosition = lastDrawedButtonPosition - 25;
                }
            }

            Refresh();
        }

        private class WellsMetroFormButton : Button, IWellsMetroControl
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

            private bool isHovered = false;
            private bool isPressed = false;

            #endregion

            #region Constructor

            public WellsMetroFormButton()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw |
                         ControlStyles.UserPaint, true);
            }

            #endregion

            #region Paint Methods

            protected override void OnPaint(PaintEventArgs e)
            {
                Color backColor, foreColor;

                WellsMetroThemeStyle _Theme = Theme;
                if (Parent != null)
                {
                    if (Parent is IWellsMetroForm)
                    {
                        _Theme = ((IWellsMetroForm)Parent).Theme;
                        backColor = WellsMetroPaint.BackColor.Form(_Theme);
                    }
                    else if (Parent is IWellsMetroControl)
                    {
                        backColor = WellsMetroPaint.GetStyleColor(Style);
                    }
                    else
                    {
                        backColor = Parent.BackColor;
                    }
                }
                else
                {
                    backColor = WellsMetroPaint.BackColor.Form(_Theme);
                }

                if (isHovered && !isPressed && Enabled)
                {
                    foreColor = WellsMetroPaint.ForeColor.Button.Normal(_Theme);
                    backColor = WellsMetroPaint.BackColor.Button.Normal(_Theme);
                }
                else if (isHovered && isPressed && Enabled)
                {
                    foreColor = WellsMetroPaint.ForeColor.Button.Press(_Theme);
                    backColor = WellsMetroPaint.GetStyleColor(Style);
                }
                else if (!Enabled)
                {
                    foreColor = WellsMetroPaint.ForeColor.Button.Disabled(_Theme);
                    backColor = WellsMetroPaint.BackColor.Button.Disabled(_Theme);
                }
                else
                {
                    foreColor = WellsMetroPaint.ForeColor.Button.Normal(_Theme);
                }

                e.Graphics.Clear(backColor);
                Font buttonFont = new Font("Webdings", 9.25f);
                TextRenderer.DrawText(e.Graphics, Text, buttonFont, ClientRectangle, foreColor, backColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
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
        }

        #endregion

        #region Shadows

        private const int CS_DROPSHADOW = 0x20000;
        const int WS_MINIMIZEBOX = 0x20000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                if (ShadowType == WellsMetroFormShadowType.SystemShadow)
                    cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private Form shadowForm;

        private void CreateShadow()
        {
            switch (ShadowType)
            {
                case WellsMetroFormShadowType.Flat:
                    shadowForm = new WellsMetroFlatDropShadow(this);
                    return;

                case WellsMetroFormShadowType.DropShadow:
                    shadowForm = new WellsMetroRealisticDropShadow(this);
                    return;
            }
        }

        private void RemoveShadow()
        {
            if (shadowForm == null || shadowForm.IsDisposed) return;

            shadowForm.Visible = false;
            Owner = shadowForm.Owner;
            shadowForm.Owner = null;
            shadowForm.Dispose();
            shadowForm = null;
        }

        #region MetroShadowBase

        protected abstract class WellsMetroShadowBase : Form
        {
            protected Form TargetForm { get; private set; }

            private readonly int shadowSize;
            private readonly int wsExStyle;

            protected WellsMetroShadowBase(Form targetForm, int shadowSize, int wsExStyle)
            {
                TargetForm = targetForm;
                this.shadowSize = shadowSize;
                this.wsExStyle = wsExStyle;

                TargetForm.Activated += OnTargetFormActivated;
                TargetForm.ResizeBegin += OnTargetFormResizeBegin;
                TargetForm.ResizeEnd += OnTargetFormResizeEnd;
                TargetForm.VisibleChanged += OnTargetFormVisibleChanged;
                TargetForm.SizeChanged += OnTargetFormSizeChanged;

                TargetForm.Move += OnTargetFormMove;
                TargetForm.Resize += OnTargetFormResize;

                if (TargetForm.Owner != null)
                    Owner = TargetForm.Owner;

                TargetForm.Owner = this;

                MaximizeBox = false;
                MinimizeBox = false;
                ShowInTaskbar = false;
                ShowIcon = false;
                FormBorderStyle = FormBorderStyle.None;

                Bounds = GetShadowBounds();
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= wsExStyle;
                    return cp;
                }
            }

            private Rectangle GetShadowBounds()
            {
                Rectangle r = TargetForm.Bounds;
                r.Inflate(shadowSize, shadowSize);
                return r;
            }

            protected abstract void PaintShadow();

            protected abstract void ClearShadow();

            #region Event Handlers

            private bool isBringingToFront;

            protected override void OnDeactivate(EventArgs e)
            {
                base.OnDeactivate(e);
                isBringingToFront = true;
            }

            private void OnTargetFormActivated(object sender, EventArgs e)
            {
                if (Visible) Update();
                if (isBringingToFront)
                {
                    Visible = true;
                    isBringingToFront = false;
                    return;
                }
                BringToFront();
            }

            private void OnTargetFormVisibleChanged(object sender, EventArgs e)
            {
                Visible = TargetForm.Visible && TargetForm.WindowState != FormWindowState.Minimized;
                Update();
            }

            private long lastResizedOn;

            private bool IsResizing { get { return lastResizedOn > 0; } }

            private void OnTargetFormResizeBegin(object sender, EventArgs e)
            {
                lastResizedOn = DateTime.Now.Ticks;
            }

            private void OnTargetFormMove(object sender, EventArgs e)
            {
                if (!TargetForm.Visible || TargetForm.WindowState != FormWindowState.Normal)
                {
                    Visible = false;
                }
                else
                {
                    Bounds = GetShadowBounds();
                }
            }

            private void OnTargetFormResize(object sender, EventArgs e)
            {
                ClearShadow();
            }

            private void OnTargetFormSizeChanged(object sender, EventArgs e)
            {
                Bounds = GetShadowBounds();

                if (IsResizing)
                {
                    return;
                }

                PaintShadowIfVisible();
            }

            private void OnTargetFormResizeEnd(object sender, EventArgs e)
            {
                lastResizedOn = 0;
                PaintShadowIfVisible();
            }

            private void PaintShadowIfVisible()
            {
                if (TargetForm.Visible && TargetForm.WindowState != FormWindowState.Minimized)
                    PaintShadow();
            }

            #endregion

            #region Constants

            protected const int WS_EX_TRANSPARENT = 0x20;
            protected const int WS_EX_LAYERED = 0x80000;
            protected const int WS_EX_NOACTIVATE = 0x8000000;

            private const int TICKS_PER_MS = 10000;
            private const long RESIZE_REDRAW_INTERVAL = 1000 * TICKS_PER_MS; 

            #endregion
        }

        #endregion

        #region Aero DropShadow

        protected class WellsMetroAeroDropShadow : WellsMetroShadowBase
        {
            public WellsMetroAeroDropShadow(Form targetForm)
                : base(targetForm, 0, WS_EX_TRANSPARENT | WS_EX_NOACTIVATE )
            {
                FormBorderStyle = FormBorderStyle.SizableToolWindow;
            }

            protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
            {
                if (specified == BoundsSpecified.Size) return;
                base.SetBoundsCore(x, y, width, height, specified);
            }

            protected override void PaintShadow() { Visible = true; }

            protected override void ClearShadow() { }

        }

        #endregion

        #region Flat DropShadow

        protected class WellsMetroFlatDropShadow : WellsMetroShadowBase
        {
            private Point Offset = new Point(-6, -6);

            public WellsMetroFlatDropShadow(Form targetForm) 
                : base(targetForm, 6, WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_NOACTIVATE)
            {
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);
                PaintShadow();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                Visible = true;
                PaintShadow();
            }

            protected override void PaintShadow()
            {
                using( Bitmap getShadow = DrawBlurBorder() )
                    SetBitmap(getShadow, 255);
            }

            protected override void ClearShadow()
            {
                Bitmap img = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(img);
                g.Clear(Color.Transparent);
                g.Flush();
                g.Dispose();
                SetBitmap(img, 255);
                img.Dispose();
            }

            #region Drawing methods

            [SecuritySafeCritical]
            private void SetBitmap(Bitmap bitmap, byte opacity)
            {
                if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                    throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

                IntPtr screenDc = WinApi.GetDC(IntPtr.Zero);
                IntPtr memDc = WinApi.CreateCompatibleDC(screenDc);
                IntPtr hBitmap = IntPtr.Zero;
                IntPtr oldBitmap = IntPtr.Zero;

                try
                {
                    hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                    oldBitmap = WinApi.SelectObject(memDc, hBitmap);

                    WinApi.SIZE size = new WinApi.SIZE(bitmap.Width, bitmap.Height);
                    WinApi.POINT pointSource = new WinApi.POINT(0, 0);
                    WinApi.POINT topPos = new WinApi.POINT(Left, Top);
                    WinApi.BLENDFUNCTION blend = new WinApi.BLENDFUNCTION();
                    blend.BlendOp = WinApi.AC_SRC_OVER;
                    blend.BlendFlags = 0;
                    blend.SourceConstantAlpha = opacity;
                    blend.AlphaFormat = WinApi.AC_SRC_ALPHA;

                    WinApi.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, WinApi.ULW_ALPHA);
                }
                finally
                {
                    WinApi.ReleaseDC(IntPtr.Zero, screenDc);
                    if (hBitmap != IntPtr.Zero)
                    {
                        WinApi.SelectObject(memDc, oldBitmap);
                        WinApi.DeleteObject(hBitmap);
                    }
                    WinApi.DeleteDC(memDc);
                }
            }

            private Bitmap DrawBlurBorder()
            {
                return (Bitmap)DrawOutsetShadow(Color.Black, new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height));
            }

            private Image DrawOutsetShadow(Color color, Rectangle shadowCanvasArea)
            {
                Rectangle rOuter = shadowCanvasArea;
                Rectangle rInner = new Rectangle(shadowCanvasArea.X + (-Offset.X - 1), shadowCanvasArea.Y + (-Offset.Y - 1), shadowCanvasArea.Width - (-Offset.X * 2 - 1), shadowCanvasArea.Height - (-Offset.Y * 2 - 1));

                Bitmap img = new Bitmap(rOuter.Width, rOuter.Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(img);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                using (Brush bgBrush = new SolidBrush(Color.FromArgb(30, Color.Black)))
                {
                    g.FillRectangle(bgBrush, rOuter);
                }
                using (Brush bgBrush = new SolidBrush(Color.FromArgb(60, Color.Black)))
                {
                    g.FillRectangle(bgBrush, rInner);
                }

                g.Flush();
                g.Dispose();

                return img;
            }

            #endregion
        }

        #endregion

        #region Realistic DropShadow

        protected class WellsMetroRealisticDropShadow : WellsMetroShadowBase
        {
            public WellsMetroRealisticDropShadow(Form targetForm) 
                : base(targetForm, 15, WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_NOACTIVATE)
            {
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);
                PaintShadow();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                Visible = true;
                PaintShadow();
            }

            protected override void PaintShadow()
            {
                using( Bitmap getShadow = DrawBlurBorder() )
                    SetBitmap(getShadow, 255);
            }

            protected override void ClearShadow()
            {
                Bitmap img = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(img);
                g.Clear(Color.Transparent);
                g.Flush();
                g.Dispose();
                SetBitmap(img, 255);
                img.Dispose();
            }

            #region Drawing methods

            [SecuritySafeCritical]
            private void SetBitmap(Bitmap bitmap, byte opacity)
            {
                if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                    throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

                IntPtr screenDc = WinApi.GetDC(IntPtr.Zero);
                IntPtr memDc = WinApi.CreateCompatibleDC(screenDc);
                IntPtr hBitmap = IntPtr.Zero;
                IntPtr oldBitmap = IntPtr.Zero;

                try
                {
                    hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                    oldBitmap = WinApi.SelectObject(memDc, hBitmap);

                    WinApi.SIZE size = new WinApi.SIZE(bitmap.Width, bitmap.Height);
                    WinApi.POINT pointSource = new WinApi.POINT(0, 0);
                    WinApi.POINT topPos = new WinApi.POINT(Left, Top);
                    WinApi.BLENDFUNCTION blend = new WinApi.BLENDFUNCTION
                        {
                            BlendOp = WinApi.AC_SRC_OVER,
                            BlendFlags = 0,
                            SourceConstantAlpha = opacity,
                            AlphaFormat = WinApi.AC_SRC_ALPHA
                        };

                    WinApi.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, WinApi.ULW_ALPHA);
                }
                finally
                {
                    WinApi.ReleaseDC(IntPtr.Zero, screenDc);
                    if (hBitmap != IntPtr.Zero)
                    {
                        WinApi.SelectObject(memDc, oldBitmap);
                        WinApi.DeleteObject(hBitmap);
                    }
                    WinApi.DeleteDC(memDc);
                }
            }

            private Bitmap DrawBlurBorder()
            {
                return (Bitmap)DrawOutsetShadow(0, 0, 40, 1, Color.Black, new Rectangle(1, 1, ClientRectangle.Width, ClientRectangle.Height));
            }

            private Image DrawOutsetShadow(int hShadow, int vShadow, int blur, int spread, Color color, Rectangle shadowCanvasArea)
            {
                Rectangle rOuter = shadowCanvasArea;
                Rectangle rInner = shadowCanvasArea;
                rInner.Offset(hShadow, vShadow);
                rInner.Inflate(-blur, -blur);
                rOuter.Inflate(spread, spread);
                rOuter.Offset(hShadow, vShadow);

                Rectangle originalOuter = rOuter;

                Bitmap img = new Bitmap(originalOuter.Width, originalOuter.Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(img);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                var currentBlur = 0;
                do
                {
                    var transparency = (rOuter.Height - rInner.Height) / (double)(blur * 2 + spread * 2);
                    var shadowColor = Color.FromArgb(((int)(200 * (transparency * transparency))), color);
                    var rOutput = rInner;
                    rOutput.Offset(-originalOuter.Left, -originalOuter.Top);

                    DrawRoundedRectangle(g, rOutput, currentBlur, Pens.Transparent, shadowColor);
                    rInner.Inflate(1, 1);
                    currentBlur = (int)((double)blur * (1 - (transparency * transparency)));

                } while (rOuter.Contains(rInner));

                g.Flush();
                g.Dispose();

                return img;
            }

            private void DrawRoundedRectangle(Graphics g, Rectangle bounds, int cornerRadius, Pen drawPen, Color fillColor)
            {
                int strokeOffset = Convert.ToInt32(Math.Ceiling(drawPen.Width));
                bounds = Rectangle.Inflate(bounds, -strokeOffset, -strokeOffset);

                var gfxPath = new GraphicsPath();

                if (cornerRadius > 0)
                {
                    gfxPath.AddArc(bounds.X, bounds.Y, cornerRadius, cornerRadius, 180, 90);
                    gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y, cornerRadius, cornerRadius, 270, 90);
                    gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
                    gfxPath.AddArc(bounds.X, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
                }
                else
                {
                    gfxPath.AddRectangle(bounds);
                }

                gfxPath.CloseAllFigures();

                if (cornerRadius > 5)
                {
                    using (SolidBrush b = new SolidBrush(fillColor))
                    {
                        g.FillPath(b, gfxPath);
                    }
                }
                if (drawPen != Pens.Transparent)
                {
                    using (Pen p = new Pen(drawPen.Color))
                    {
                        p.EndCap = p.StartCap = LineCap.Round;
                        g.DrawPath(p, gfxPath);
                    }
                }
            }

            #endregion
        }

        #endregion

        #endregion

        #region Helper Methods

        [SecuritySafeCritical]
        public void RemoveCloseButton()
        {
            IntPtr hMenu = WinApi.GetSystemMenu(Handle, false);
            if (hMenu == IntPtr.Zero) return;

            int n = WinApi.GetMenuItemCount(hMenu);
            if (n <= 0) return;

            WinApi.RemoveMenu(hMenu, (uint)(n - 1), WinApi.MfByposition | WinApi.MfRemove);
            WinApi.RemoveMenu(hMenu, (uint)(n - 2), WinApi.MfByposition | WinApi.MfRemove);
            WinApi.DrawMenuBar(Handle);
        }

        private Rectangle MeasureText(Graphics g, Rectangle clientRectangle, Font font, string text, TextFormatFlags flags)
        {
            var proposedSize = new Size(int.MaxValue, int.MinValue);
            var actualSize = TextRenderer.MeasureText(g, text, font, proposedSize, flags);
            return new Rectangle(clientRectangle.X, clientRectangle.Y, actualSize.Width, actualSize.Height);
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MetroForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "WellsMetroForm";
            this.ResumeLayout(false);

        }
    }
}
