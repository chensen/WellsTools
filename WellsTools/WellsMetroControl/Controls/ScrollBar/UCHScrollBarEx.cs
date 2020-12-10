using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wells.WellsMetroControl.Controls
{
    [Designer(typeof(ScrollbarControlDesigner))]
    [DefaultEvent("Scroll")]
    public class UCHScrollbarEx : UCControlBase
    {
        #region 属性    English:attribute
        /// <summary>
        /// The mo minimum
        /// </summary>
        protected int moMinimum = 0;
        /// <summary>
        /// The mo maximum
        /// </summary>
        protected int moMaximum = 100;
        /// <summary>
        /// The mo low value
        /// </summary>
        protected int moValueLow = 0;
        /// <summary>
        /// The mo high value
        /// </summary>
        protected int moValueHigh = 100;
        /// <summary>
        /// The n click point low
        /// </summary>
        private int nClickPointLow;
        /// <summary>
        /// The n click point high
        /// </summary>
        private int nClickPointHigh;
        /// <summary>
        /// The mo low thumb top
        /// </summary>
        protected int moThumbLowLeft = 0;
        /// <summary>
        /// The mo high thumb top
        /// </summary>
        protected int moThumbHighLeft = 0;
        /// <summary>
        /// The mo automatic size
        /// </summary>
        protected bool moAutoSize = false;
        /// <summary>
        /// The mo thumb down
        /// </summary>
        private bool moThumbMouseDown = false;
        /// <summary>
        /// The mo thumb dragging
        /// </summary>
        private bool moThumbMouseDragging = false;
        /// <summary>
        /// The mo thumb index
        /// </summary>
        private int moThumbMouseIndex = 0;
        /// <summary>
        /// Occurs when [scroll].
        /// </summary>
        public new event EventHandler Scroll = null;
        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        public event EventHandler ValueChanged = null;
        
        /// <summary>
        /// The m int thumb minimum height
        /// </summary>
        private int m_intThumbMinWidth = 15;

        private decimal ratio = 1.0M;

        private int decLength = 0;

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("自定义"), Description("显示数值相乘系数")]
        public decimal Aratio
        {
            get { return ratio; }
            set
            {
                ratio = value;
                moMaximum = (int)(moMaximum / ratio);
                moMinimum = (int)(moMinimum / ratio);
                moValueLow = (int)(moValueLow / ratio);
                moValueHigh = (int)(moValueHigh / ratio);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("自定义"), Description("显示数值小数位数 ")]
        public int DecLength
        {
            get
            {
                return this.decLength;
            }
            set
            {
                this.decLength = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("自定义"), Description("Minimum")]
        public decimal Minimum
        {
            get { return Math.Round(moMinimum*ratio,decLength); }
            set
            {
                int tmp = (int)(value / ratio);

                if (tmp > moMaximum)
                    return;

                moMinimum = tmp;

                if (moValueLow < moMinimum)
                    moValueLow = moMinimum;
                if (moValueHigh < moValueLow)
                    moValueHigh = moValueLow;

                int nTrackWidth = this.Width;
                float fThumbWidth = nTrackWidth / (float)(moMaximum - moMinimum);
                int nThumbWidth = (int)fThumbWidth;

                if (nThumbWidth > nTrackWidth)
                {
                    nThumbWidth = nTrackWidth;
                    fThumbWidth = nTrackWidth;
                }
                if (nThumbWidth < m_intThumbMinWidth)
                {
                    nThumbWidth = m_intThumbMinWidth;
                    fThumbWidth = m_intThumbMinWidth;
                }

                //figure out value
                int nPixelRange = nTrackWidth - nThumbWidth;
                int nRealRange = (moMaximum - moMinimum);
                float fPerc = 0.0f;
                if (nRealRange != 0)
                {
                    fPerc = (float)(moValueLow - moMinimum) / (float)nRealRange;
                }

                float fLeft = fPerc * nPixelRange;
                moThumbLowLeft = (int)fLeft;

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("自定义"), Description("Maximum")]
        public decimal Maximum
        {
            get { return Math.Round(moMaximum * ratio, decLength); }
            set
            {
                int tmp = (int)(value / ratio);

                if (tmp < moMinimum)
                    return;

                moMaximum = tmp;

                if (moValueHigh > moMaximum)
                    moValueHigh = moMaximum;
                if (moValueLow > moValueHigh)
                    moValueLow = moValueHigh;

                int nTrackWidth = this.Width;
                float fThumbWidth = nTrackWidth / (float)(moMaximum - moMinimum);
                int nThumbWidth = (int)fThumbWidth;

                if (nThumbWidth > nTrackWidth)
                {
                    nThumbWidth = nTrackWidth;
                    fThumbWidth = nTrackWidth;
                }
                if (nThumbWidth < m_intThumbMinWidth)
                {
                    nThumbWidth = m_intThumbMinWidth;
                    fThumbWidth = m_intThumbMinWidth;
                }

                //figure out value
                int nPixelRange = nTrackWidth - nThumbWidth;
                int nRealRange = (moMaximum - moMinimum);
                float fPerc = 0.0f;
                if (nRealRange != 0)
                {
                    fPerc = (float)(moValueHigh - moMinimum) / (float)nRealRange;
                }

                float fLeft = fPerc * nPixelRange;
                moThumbHighLeft = (int)fLeft;

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the valuelow.
        /// </summary>
        /// <value>The value.</value>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("自定义"), Description("ValueLow")]
        public decimal ValueLow
        {
            get { return Math.Round(moValueLow * ratio, decLength); }
            set
            {
                int tmp = (int)(value / ratio);

                if (isShowDouble && (tmp > moValueHigh)) 
                    return;

                if (!isShowDouble && (tmp > moMaximum))
                    tmp = moMaximum;

                if (tmp < moMinimum)
                    tmp = moMinimum;

                if (tmp == moValueLow)
                    return;

                moValueLow = tmp;

                int nTrackWidth = this.Width;
                float fThumbWidth = nTrackWidth / (float)(moMaximum - moMinimum);
                int nThumbWidth = (int)fThumbWidth;

                if (nThumbWidth > nTrackWidth)
                {
                    nThumbWidth = nTrackWidth;
                    fThumbWidth = nTrackWidth;
                }
                if (nThumbWidth < m_intThumbMinWidth)
                {
                    nThumbWidth = m_intThumbMinWidth;
                    fThumbWidth = m_intThumbMinWidth;
                }

                //figure out value
                int nPixelRange = nTrackWidth - nThumbWidth;
                int nRealRange = (moMaximum - moMinimum);
                float fPerc = 0.0f;
                if (nRealRange != 0)
                {
                    fPerc = (float)(moValueLow - moMinimum) / (float)nRealRange;
                }

                float fLeft = fPerc * nPixelRange;
                moThumbLowLeft = (int)fLeft;

                if (ValueChanged != null)
                    ValueChanged(this, new EventArgs());

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the value high.
        /// </summary>
        /// <value>The value.</value>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("自定义"), Description("ValueHigh")]
        public decimal ValueHigh
        {
            get { return Math.Round(moValueHigh * ratio, decLength); }
            set
            {
                int tmp = (int)(value / ratio);

                if (isShowDouble && (tmp < moValueLow))
                    return;

                if (!isShowDouble)
                    tmp = moMaximum;

                if (tmp > moMaximum)
                    tmp = moMaximum;

                if (tmp == moValueHigh)
                    return;

                moValueHigh = tmp;

                int nTrackWidth = this.Width;
                float fThumbWidth = nTrackWidth / (float)(moMaximum - moMinimum);
                int nThumbWidth = (int)fThumbWidth;

                if (nThumbWidth > nTrackWidth)
                {
                    nThumbWidth = nTrackWidth;
                    fThumbWidth = nTrackWidth;
                }
                if (nThumbWidth < m_intThumbMinWidth)
                {
                    nThumbWidth = m_intThumbMinWidth;
                    fThumbWidth = m_intThumbMinWidth;
                }

                //figure out value
                int nPixelRange = nTrackWidth - nThumbWidth;
                int nRealRange = (moMaximum - moMinimum);
                float fPerc = 0.0f;
                if (nRealRange != 0)
                {
                    fPerc = (float)(moValueHigh - moMinimum) / (float)nRealRange;
                }

                float fLeft = fPerc * nPixelRange;
                moThumbHighLeft = (int)fLeft;

                if (ValueChanged != null)
                    ValueChanged(this, new EventArgs());

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic size].
        /// </summary>
        /// <value><c>true</c> if [automatic size]; otherwise, <c>false</c>.</value>
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
                if (base.AutoSize)
                {
                    this.Width = 15;
                }
            }
        }

        private bool isShowDouble = true;

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("自定义"), Description("是否显示双滑块")]
        public bool IsShowDouble
        {
            get { return isShowDouble; }
            set
            {
                isShowDouble = value;
                if (!isShowDouble)
                    moValueHigh = moMaximum;
                Invalidate();
            }
        }

        /// <summary>
        /// The low thumb color
        /// </summary>
        private Color lowThumbColor = Color.FromArgb(255, 77, 58);

        /// <summary>
        /// Gets or sets the color of the thumb.
        /// </summary>
        /// <value>The color of the thumb.</value>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("自定义"), Description("低值滑块颜色")]
        public Color LowThumbColor
        {
            get { return lowThumbColor; }
            set { lowThumbColor = value; }
        }

        /// <summary>
        /// The high thumb color
        /// </summary>
        private Color highThumbColor = Color.FromArgb(77, 255, 60);

        /// <summary>
        /// Gets or sets the color of the thumb.
        /// </summary>
        /// <value>The color of the thumb.</value>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("自定义"), Description("高值滑块颜色")]
        public Color HighThumbColor
        {
            get { return highThumbColor; }
            set { highThumbColor = value; }
        }

        private Color valueColor = Color.FromArgb(50, 50, 55);

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("自定义"), Description("滑块值段颜色")]
        public Color ValueColor
        {
            get { return valueColor; }
            set { valueColor = value; }
        }

        /// <summary>
        /// The is show tips
        /// </summary>
        private bool isShowTips = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is show tips.
        /// </summary>
        /// <value><c>true</c> if this instance is show tips; otherwise, <c>false</c>.</value>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("自定义"), Description("点击滑动时是否显示数值提示")]
        public bool IsShowTips
        {
            get { return isShowTips; }
            set { isShowTips = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private Color tipsBackColor = Color.FromArgb(255, 255, 255);

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("自定义"), Description("数值背景颜色")]
        public Color TipsBackColor
        {
            get { return tipsBackColor; }
            set { tipsBackColor = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private Color tipsForeColor = Color.FromArgb(0, 0, 0);

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("自定义"), Description("数值字体颜色")]
        public Color TipsForeColor
        {
            get { return tipsForeColor; }
            set { tipsForeColor = value; }
        }

        /// <summary>
        /// Gets or sets the tips format.
        /// </summary>
        /// <value>The tips format.</value>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("自定义"), Description("显示数值提示的格式化形式")]
        public string TipsFormat { get; set; }

        private TextBoxEx txtBoxLow = null;
        private TextBoxEx txtBoxHigh = null;

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("自定义"), Description("低值绑定TextboxEx")]
        public TextBoxEx TxtBoxLow
        {
            get { return txtBoxLow; }
            set
            {
                if (txtBoxLow != null)
                {
                    txtBoxLow.UcHScrollbarEx = null;
                    txtBoxLow.UCHNum = -1;
                }
                if (value == null)
                {
                    txtBoxLow = null;
                }
                else
                {
                    txtBoxLow = value;
                    txtBoxLow.UcHScrollbarEx = this;
                    txtBoxLow.UCHNum = 0;
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("自定义"), Description("高值绑定TextboxEx")]
        public TextBoxEx TxtBoxHigh
        {
            get { return txtBoxHigh; }
            set
            {
                if (txtBoxHigh != null)
                {
                    txtBoxHigh.UcHScrollbarEx = null;
                    txtBoxHigh.UCHNum = -1;
                }
                if (value == null)
                {
                    txtBoxHigh = null;
                }
                else
                {
                    txtBoxHigh = value;
                    txtBoxHigh.UcHScrollbarEx = this;
                    txtBoxHigh.UCHNum = 1;
                }
            }
        }

        #endregion

        public UCHScrollbarEx()
        {
            InitializeComponent();
            ConerRadius = 2;
            FillColor = Color.FromArgb(239, 239, 239);
            IsShowRect = false;
            IsRadius = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
        }
        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.MinimumSize = new System.Drawing.Size(0, 10);
            this.Name = "UCHScrollbarEx";
            this.Size = new System.Drawing.Size(150, 18);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CustomScrollbar_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CustomScrollbar_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CustomScrollbar_MouseUp);
            this.SizeChanged += new System.EventHandler(this.Custom_SizeChanged);
            this.ResumeLayout(false);

        }

        #region 鼠标事件    English:Mouse event
        /// <summary>
        /// Handles the MouseDown event of the CustomScrollbar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void CustomScrollbar_MouseDown(object sender, MouseEventArgs e)
        {
            //Point ptPoint = this.PointToClient(Cursor.Position);
            Point ptPoint = new Point(e.X, e.Y);
            int nTrackWidth = this.Width;
            float fThumbWidth = nTrackWidth / (float)(moMaximum - moMinimum);
            int nThumbWidth = (int)fThumbWidth;

            if (nThumbWidth > nTrackWidth)
            {
                nThumbWidth = nTrackWidth;
                fThumbWidth = nTrackWidth;
            }
            if (nThumbWidth < m_intThumbMinWidth)
            {
                nThumbWidth = m_intThumbMinWidth;
                fThumbWidth = m_intThumbMinWidth;
            }
            
            Rectangle thumbrectLow = new Rectangle(new Point(moThumbLowLeft, 1), new Size(nThumbWidth, this.Height - 2));
            Rectangle thumbrectHigh = new Rectangle(new Point(moThumbHighLeft, 1), new Size(nThumbWidth, this.Height - 2));

            bool bInLeft = thumbrectLow.Contains(ptPoint);
            bool bInRight = thumbrectHigh.Contains(ptPoint) && isShowDouble;

            if(bInLeft && bInRight)
            {
                if (e.X - moThumbLowLeft - nThumbWidth <= moThumbHighLeft - e.X) bInRight = false;
                else bInLeft = false;
            }

            //滑块
            if (bInLeft) 
            {
                //hit the thumb low
                nClickPointLow = (ptPoint.X - moThumbLowLeft);
                this.moThumbMouseIndex = -1;
                this.moThumbMouseDown = true;
                ShowTips();
            }

            if (bInRight) 
            {
                //hit the thumb high
                nClickPointHigh = (ptPoint.X - moThumbHighLeft);
                this.moThumbMouseIndex = 1;
                this.moThumbMouseDown = true;
                ShowTips();
            }

            
        }

        /// <summary>
        /// Handles the MouseUp event of the CustomScrollbar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void CustomScrollbar_MouseUp(object sender, MouseEventArgs e)
        {
            this.moThumbMouseDown = false;
            this.moThumbMouseDragging = false;
            this.moThumbMouseIndex = 0;

            if (frmTips != null && !frmTips.IsDisposed)
            {
                frmTips.Close();
                frmTips = null;
            }
        }

        /// <summary>
        /// Moves the thumb.
        /// </summary>
        /// <param name="x">The y.</param>
        private void MoveThumb(int x)
        {
            int nRealRange = moMaximum - moMinimum;
            int nTrackWidth = this.Width;
            float fThumbWidth = nTrackWidth / (float)(moMaximum - moMinimum);
            int nThumbWidth = (int)fThumbWidth;

            if (nThumbWidth > nTrackWidth)
            {
                nThumbWidth = nTrackWidth;
                fThumbWidth = nTrackWidth;
            }
            if (nThumbWidth < m_intThumbMinWidth)
            {
                nThumbWidth = m_intThumbMinWidth;
                fThumbWidth = m_intThumbMinWidth;
            }

            int nSpot = 0;

            if (moThumbMouseIndex == -1)
            {
                nSpot = nClickPointLow;

                int nPixelRange = (nTrackWidth - nThumbWidth);

                if (moThumbMouseDown && nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        int nNewThumbLeft = x - nSpot;

                        if (isShowDouble)
                        {
                            if (nNewThumbLeft < 0)
                            {
                                moThumbLowLeft = nNewThumbLeft = 0;
                            }
                            else if (nNewThumbLeft > moThumbHighLeft)
                            {
                                moThumbLowLeft = moThumbHighLeft;
                            }
                            else
                            {
                                moThumbLowLeft = x - nSpot;
                            }
                        }
                        else
                        {
                            if (nNewThumbLeft < 0)
                            {
                                moThumbLowLeft = nNewThumbLeft = 0;
                            }
                            else if (nNewThumbLeft > nPixelRange)
                            {
                                moThumbLowLeft = nPixelRange;
                            }
                            else
                            {
                                moThumbLowLeft = x - nSpot;
                            }
                        }
                        
                        float fPerc = (float)moThumbLowLeft / (float)nPixelRange;
                        float fValue = fPerc * (moMaximum - moMinimum) + moMinimum;
                        moValueLow = (int)fValue;

                        Application.DoEvents();

                        Invalidate();
                    }
                }
            }
            else if (moThumbMouseIndex == 1)
            {
                nSpot = nClickPointHigh;

                int nPixelRange = (nTrackWidth - nThumbWidth);

                if (moThumbMouseDown && nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        int nNewThumbLeft = x - nSpot;

                        if (nNewThumbLeft < moThumbLowLeft)
                        {
                            moThumbHighLeft = moThumbLowLeft;
                        }
                        else if (nNewThumbLeft > nPixelRange)
                        {
                            moThumbHighLeft = nNewThumbLeft = nPixelRange;
                        }
                        else
                        {
                            moThumbHighLeft = x - nSpot;
                        }


                        float fPerc = (float)moThumbHighLeft / (float)nPixelRange;
                        float fValue = fPerc * (moMaximum - moMinimum) + moMinimum;
                        moValueHigh = (int)fValue;

                        Application.DoEvents();

                        Invalidate();
                    }
                }
            }
        }

        /// <summary>
        /// Handles the MouseMove event of the CustomScrollbar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void CustomScrollbar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!moThumbMouseDown)
                return;

            if (moThumbMouseDown == true)
            {
                this.moThumbMouseDragging = true;
            }

            if (this.moThumbMouseDragging)
            {
                MoveThumb(e.X);
                ShowTips();
            }

            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());

            if (Scroll != null)
                Scroll(this, new EventArgs());

            if (txtBoxLow != null)
                txtBoxLow.Text = ValueLow.ToString();
            if (txtBoxHigh != null)
                txtBoxHigh.Text = ValueHigh.ToString();
        }
        #endregion

        private void Custom_SizeChanged(object sender,EventArgs e)
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SetGDIHigh();

            //draw thumb
            int nTrackWidth = this.Width;
            float fThumbWidth = nTrackWidth / (float)(moMaximum - moMinimum);
            int nThumbWidth = (int)fThumbWidth;

            if (nThumbWidth > nTrackWidth)
            {
                nThumbWidth = nTrackWidth;
                fThumbWidth = nTrackWidth;
            }
            if (nThumbWidth < m_intThumbMinWidth)
            {
                nThumbWidth = m_intThumbMinWidth;
                fThumbWidth = m_intThumbMinWidth;
            }

            e.Graphics.FillPath(new SolidBrush(lowThumbColor), new Rectangle(moThumbLowLeft, 1, nThumbWidth, this.Height - 3).CreateRoundedRectanglePath(this.ConerRadius));
            if (isShowDouble)
            {
                e.Graphics.FillPath(new SolidBrush(highThumbColor), new Rectangle(moThumbHighLeft, 1, nThumbWidth, this.Height - 3).CreateRoundedRectanglePath(this.ConerRadius));
                e.Graphics.FillRectangle(new SolidBrush(valueColor), new Rectangle(moThumbLowLeft + nThumbWidth, 5, moThumbHighLeft - moThumbLowLeft - nThumbWidth, this.Height - 10));
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(valueColor), new Rectangle(0, 5, moThumbLowLeft, this.Height - 10));
            }
        }

        Forms.FrmAnchorTips frmTips = null;

        private void ShowTips()
        {
            if (isShowTips)
            {
                string strValue = string.Empty;
                decimal Value = 0;
                Point ptScreen = Point.Empty;

                int nTrackWidth = this.Width;
                float fThumbWidth = nTrackWidth / (float)(moMaximum - moMinimum);
                int nThumbWidth = (int)fThumbWidth;

                if (nThumbWidth > nTrackWidth)
                {
                    nThumbWidth = nTrackWidth;
                    fThumbWidth = nTrackWidth;
                }
                if (nThumbWidth < m_intThumbMinWidth)
                {
                    nThumbWidth = m_intThumbMinWidth;
                    fThumbWidth = m_intThumbMinWidth;
                }

                if (moThumbMouseIndex == 0)
                {
                    return;
                }
                else if (moThumbMouseIndex == -1)
                {
                    Value = ValueLow;
                    ptScreen = this.PointToScreen(new Point(moThumbLowLeft - 3 * nThumbWidth / 2, 1));
                }
                else if (moThumbMouseIndex == 1)
                {
                    Value = ValueHigh;
                    ptScreen = this.PointToScreen(new Point(moThumbHighLeft - 3 * nThumbWidth / 2, 1));
                }

                if (!string.IsNullOrEmpty(TipsFormat))
                {
                    try
                    {
                        strValue = Value.ToString(TipsFormat);
                    }
                    catch { }
                }
                else
                {
                    strValue = Value.ToString();
                }

                if (frmTips == null || frmTips.IsDisposed || !frmTips.Visible)
                {
                    frmTips = Forms.FrmAnchorTips.ShowTips(new Rectangle(ptScreen.X, ptScreen.Y, 50, 20), strValue, Forms.AnchorTipsLocation.TOP, tipsBackColor,tipsForeColor, autoCloseTime: -1);
                }
                else
                {
                    frmTips.RectControl = new Rectangle(ptScreen.X, ptScreen.Y, 50, 20);
                    frmTips.StrMsg = strValue;
                }
            }
        }

        public void reset()
        {
            this.ValueLow = this.Minimum;
            this.ValueHigh = this.Maximum;
            if (this.txtBoxLow != null) txtBoxLow.Text = this.ValueLow.ToString();
            if (this.txtBoxHigh != null) txtBoxHigh.Text = this.ValueHigh.ToString();
        }
    }

}
