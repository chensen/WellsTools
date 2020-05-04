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
    [Designer(typeof(Design.Controls.WellsMetroTextBoxDesigner), typeof(System.ComponentModel.Design.IRootDesigner))]
    //[Designer("Wells.WellsFramework.Design.Controls.WellsMetroTextBoxDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    public class WellsMetroTextBox : Control, IWellsMetroControl
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

        private PromptedTextBox baseTextBox;

        private WellsMetroTextBoxSize metroTextBoxSize = WellsMetroTextBoxSize.Small;
        [DefaultValue(WellsMetroTextBoxSize.Small)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroTextBoxSize FontSize
        {
            get { return metroTextBoxSize; }
            set { metroTextBoxSize = value; UpdateBaseTextBox(); }
        }

        private WellsMetroTextBoxWeight metroTextBoxWeight = WellsMetroTextBoxWeight.Regular;
        [DefaultValue(WellsMetroTextBoxWeight.Regular)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroTextBoxWeight FontWeight
        {
            get { return metroTextBoxWeight; }
            set { metroTextBoxWeight = value; UpdateBaseTextBox(); }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue("")]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public string PromptText
        {
            get { return baseTextBox.PromptText; }
            set { baseTextBox.PromptText = value; }
        }

        private Image textBoxIcon = null;
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(null)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public Image Icon
        {
            get { return textBoxIcon; }
            set 
            { 
                textBoxIcon = value;
                Refresh();
            }
        }

        private bool textBoxIconRight = false;
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public bool IconRight
        {
            get { return textBoxIconRight; }
            set
            {
                textBoxIconRight = value;
                Refresh();
            }
        }

        private bool displayIcon = true;
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(true)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public bool DisplayIcon
        {
            get { return displayIcon; }
            set 
            { 
                displayIcon = value;
                Refresh();
            }
        }

        protected Size iconSize 
        {
            get 
            {
                if (displayIcon && textBoxIcon != null)
                {
                    Size originalSize = textBoxIcon.Size;
                    double resizeFactor = (ClientRectangle.Height - 2) / (double)originalSize.Height;

                    Point iconLocation = new Point(1, 1);
                    return new Size((int)(originalSize.Width * resizeFactor), (int)(originalSize.Height * resizeFactor));
                }

                return new Size(-1, -1);
            }   
        }

        #endregion

        #region Routing Fields

        public override ContextMenu ContextMenu
        {
            get { return baseTextBox.ContextMenu; }
            set 
            {
                ContextMenu = value;
                baseTextBox.ContextMenu = value; 
            }
        }

        public override ContextMenuStrip ContextMenuStrip
        {
            get { return baseTextBox.ContextMenuStrip; }
            set
            {
                ContextMenuStrip = value;
                baseTextBox.ContextMenuStrip = value;
            }
        }

        [DefaultValue(false)]
        public bool Multiline
        {
            get { return baseTextBox.Multiline; }
            set { baseTextBox.Multiline = value; }
        }

        public override string Text
        {
            get { return baseTextBox.Text; }
            set { baseTextBox.Text = value; }
        }
        
        public string[] Lines
        {
            get { return baseTextBox.Lines; }
            set { baseTextBox.Lines = value; }
        }

        [Browsable(false)]
        public string SelectedText
        {
            get { return baseTextBox.SelectedText;  }
            set { baseTextBox.Text = value; }
        }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return baseTextBox.ReadOnly; }
            set { baseTextBox.ReadOnly = value; }
        }

        public char PasswordChar
        {
            get { return baseTextBox.PasswordChar; }
            set { baseTextBox.PasswordChar = value; }
        }

        [DefaultValue(false)]
        public bool UseSystemPasswordChar
        {
            get { return baseTextBox.UseSystemPasswordChar; }
            set { baseTextBox.UseSystemPasswordChar = value; }
        }

        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign
        {
            get { return baseTextBox.TextAlign; }
            set { baseTextBox.TextAlign = value; }
        }

        [DefaultValue(true)]
        public new bool TabStop
        {
            get { return baseTextBox.TabStop; }
            set { baseTextBox.TabStop = value; }
        }

        public int MaxLength
        {
            get { return baseTextBox.MaxLength; }
            set { baseTextBox.MaxLength = value; }
        }

        public ScrollBars ScrollBars
        {
            get { return baseTextBox.ScrollBars; }
            set { baseTextBox.ScrollBars = value; }
        }

        #endregion

        #region Constructor

        public WellsMetroTextBox()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            base.TabStop = false;
            base.GotFocus += WellsMetroTextBox_GotFocus;
            CreateBaseTextBox();
            UpdateBaseTextBox();
            AddEventHandler();       
        }

        void WellsMetroTextBox_GotFocus(object sender, EventArgs e)
        {
            baseTextBox.Focus();
        }

        #endregion

        #region Routing Methods

        public event EventHandler AcceptsTabChanged;
        private void BaseTextBoxAcceptsTabChanged(object sender, EventArgs e)
        {
            if (AcceptsTabChanged != null)
                AcceptsTabChanged(this, e);
        }
        
        private void BaseTextBoxSizeChanged(object sender, EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        private void BaseTextBoxCursorChanged(object sender, EventArgs e)
        {
            base.OnCursorChanged(e);
        }

        private void BaseTextBoxContextMenuStripChanged(object sender, EventArgs e)
        {
            base.OnContextMenuStripChanged(e);
        }

        private void BaseTextBoxContextMenuChanged(object sender, EventArgs e)
        {
            base.OnContextMenuChanged(e);
        }

        private void BaseTextBoxClientSizeChanged(object sender, EventArgs e)
        {
            base.OnClientSizeChanged(e);
        }

        private void BaseTextBoxClick(object sender, EventArgs e)
        {
            base.OnClick(e);
        }

        private void BaseTextBoxChangeUiCues(object sender, UICuesEventArgs e)
        {
            base.OnChangeUICues(e);
        }

        private void BaseTextBoxCausesValidationChanged(object sender, EventArgs e)
        {
            base.OnCausesValidationChanged(e);
        }

        private void BaseTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        private void BaseTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void BaseTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        private void BaseTextBoxTextChanged(object sender, EventArgs e)
        {
            base.OnTextChanged(e);
        }

        public void Select(int start, int length)
        {
            baseTextBox.Select(start, length);
        }

        public void SelectAll()
        {
            baseTextBox.SelectAll();
        }

        public void Clear()
        {
            baseTextBox.Clear();
        }

        public void AppendText(string text)
        {
            baseTextBox.AppendText(text);
        }

        #endregion

        #region Paint Methods

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                Color backColor = BackColor;
                baseTextBox.BackColor = BackColor;

                if (!useCustomBackColor)
                {
                    backColor = WellsMetroPaint.BackColor.Button.Normal(Theme);
                    baseTextBox.BackColor = WellsMetroPaint.BackColor.Button.Normal(Theme);
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
            if (useCustomForeColor)
            {
                baseTextBox.ForeColor = ForeColor;
            }
            else
            {
                baseTextBox.ForeColor = WellsMetroPaint.ForeColor.Button.Normal(Theme);
            }

            Color borderColor = WellsMetroPaint.BorderColor.Button.Normal(Theme);

            if (useStyleColors)
                borderColor = WellsMetroPaint.GetStyleColor(Style);

            using (Pen p = new Pen(borderColor))
            {
                e.Graphics.DrawRectangle(p, new Rectangle(0, 0, Width - 1, Height - 1));
            }

            DrawIcon(e.Graphics);
        }

        private void DrawIcon(Graphics g)
        {
            if (displayIcon && textBoxIcon != null)
            {
                Point iconLocation = new Point(1, 1);
                if (textBoxIconRight)
                {
                    iconLocation = new Point(ClientRectangle.Width - iconSize.Width - 1, 1);
                }

                g.DrawImage(textBoxIcon, new Rectangle(iconLocation, iconSize));

                UpdateBaseTextBox();
            }

            OnCustomPaintForeground(new WellsMetroPaintEventArgs(Color.Empty, baseTextBox.ForeColor, g));
        }

        #endregion

        #region Overridden Methods

        public override void Refresh()
        {
            base.Refresh();
            UpdateBaseTextBox();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateBaseTextBox();
        }

        #endregion

        #region Private Methods

        private void CreateBaseTextBox()
        {
            if (baseTextBox != null) return;

            baseTextBox = new PromptedTextBox();

            baseTextBox.BorderStyle = BorderStyle.None;
            baseTextBox.Font = WellsMetroFonts.TextBox(metroTextBoxSize, metroTextBoxWeight);
            baseTextBox.Location = new Point(3, 3);
            baseTextBox.Size = new Size(Width - 6, Height - 6);

            Size = new Size(baseTextBox.Width + 6, baseTextBox.Height + 6);

            baseTextBox.TabStop = true;
            //baseTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            Controls.Add(baseTextBox);
        }

        private void AddEventHandler()
        {
            baseTextBox.AcceptsTabChanged += BaseTextBoxAcceptsTabChanged;

            baseTextBox.CausesValidationChanged += BaseTextBoxCausesValidationChanged;
            baseTextBox.ChangeUICues += BaseTextBoxChangeUiCues;
            baseTextBox.Click += BaseTextBoxClick;
            baseTextBox.ClientSizeChanged += BaseTextBoxClientSizeChanged;
            baseTextBox.ContextMenuChanged += BaseTextBoxContextMenuChanged;
            baseTextBox.ContextMenuStripChanged += BaseTextBoxContextMenuStripChanged;
            baseTextBox.CursorChanged += BaseTextBoxCursorChanged;

            baseTextBox.KeyDown += BaseTextBoxKeyDown;
            baseTextBox.KeyPress += BaseTextBoxKeyPress;
            baseTextBox.KeyUp += BaseTextBoxKeyUp;

            baseTextBox.SizeChanged += BaseTextBoxSizeChanged;

            baseTextBox.TextChanged += BaseTextBoxTextChanged;
        }

        private void UpdateBaseTextBox()
        {
            if (baseTextBox == null) return;

            baseTextBox.Font = WellsMetroFonts.TextBox(metroTextBoxSize, metroTextBoxWeight);

            if (displayIcon)
            {
                Point textBoxLocation = new Point(iconSize.Width + 4, 3);
                if (textBoxIconRight)
                {
                    textBoxLocation = new Point(3, 3);
                }

                baseTextBox.Location = textBoxLocation;
                baseTextBox.Size = new Size(Width - 7 - iconSize.Width, Height - 6);
            }
            else
            {
                baseTextBox.Location = new Point(3, 3);
                baseTextBox.Size = new Size(Width - 6, Height - 6);
            }
        }

        #endregion

        #region PromptedTextBox

        private class PromptedTextBox : TextBox
        {
            private const int OCM_COMMAND = 0x2111;
            private const int WM_PAINT = 15;

            private bool drawPrompt;

            private string promptText = "";
            [Browsable(true)]
            [EditorBrowsable(EditorBrowsableState.Always)]
            [DefaultValue("")]
            public string PromptText
            {
                get { return promptText; }
                set
                {
                    promptText = value.Trim();
                    Invalidate();
                }
            }

            public PromptedTextBox()
            {
                drawPrompt = (Text.Trim().Length == 0);
            }

            private void DrawTextPrompt()
            {
                using (Graphics graphics = CreateGraphics())
                {
                    DrawTextPrompt(graphics);
                }
            }

            private void DrawTextPrompt(Graphics g)
            {
                TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis;
                Rectangle clientRectangle = ClientRectangle;

                switch (TextAlign)
                {
                    case HorizontalAlignment.Left:
                        clientRectangle.Offset(1, 1);
                        break;

                    case HorizontalAlignment.Right:
                        flags |= TextFormatFlags.Right;
                        clientRectangle.Offset(0, 1);
                        break;

                    case HorizontalAlignment.Center:
                        flags |= TextFormatFlags.HorizontalCenter;
                        clientRectangle.Offset(0, 1);
                        break;
                }

                TextRenderer.DrawText(g, promptText, Font, clientRectangle, SystemColors.GrayText, BackColor, flags);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                if (drawPrompt)
                {
                    DrawTextPrompt(e.Graphics);
                }
            }

            protected override void OnTextAlignChanged(EventArgs e)
            {
                base.OnTextAlignChanged(e);
                Invalidate();
            }

            protected override void OnTextChanged(EventArgs e)
            {
                base.OnTextChanged(e);
                drawPrompt = (Text.Trim().Length == 0);
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (((m.Msg == WM_PAINT) || (m.Msg == OCM_COMMAND)) && (drawPrompt && !GetStyle(ControlStyles.UserPaint)))
                {
                    DrawTextPrompt();
                }
            }

        }

        #endregion
    }
}
