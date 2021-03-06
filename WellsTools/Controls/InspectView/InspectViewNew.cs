﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wells.Controls.InspectViewNew
{
    public partial class InspectViewNew : Panel
    {
        private decimal _Scale = 1.0M;
        private decimal _Scale_origin = 1.0M;
        private Image _Image = null;
        private Point _pCenter = new Point(160, 120);
        private int _PnlWidth = 320;
        private int _PnlHeight = 240;
        private int _ImgWidth = 1920;
        private int _ImgHeight = 1200;
        private bool _StatusBar = true;
        private string _strImageInfo = string.Empty;
        private int _xPos = 0;
        private int _yPos = 0;

        public InspectViewNew()
        {
            InitializeComponent();
            MouseWheel += new System.Windows.Forms.MouseEventHandler(pnlFov_MouseWheel);
            Paint += new System.Windows.Forms.PaintEventHandler(pnlFov_Paint);
            MouseMove += new System.Windows.Forms.MouseEventHandler(pnlFov_MouseMove);
            MouseUp += new System.Windows.Forms.MouseEventHandler(pnlFov_MouseUp);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
        }

        public InspectViewNew(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void InitialInspectView(Bitmap bmp)
        {
            Image img = bmp;
            InitialInspectView(img);
        }

        public void InitialInspectView(Image img = null)
        {
            if (img == null)
            {
                _Image = new Bitmap(1920, 1200);
                Graphics g = Graphics.FromImage(_Image);
                g.Clear(Color.Black);
                g.Dispose();
            }
            else
            {
                _Image = (Image)img.Clone();
            }

            _PnlWidth = Width;
            _PnlHeight = Height;
            _pCenter = new Point(_PnlWidth / 2, _PnlHeight / 2);
            _ImgWidth = _Image.Width;
            _ImgHeight = _Image.Height;
            _Scale = Math.Min(_PnlWidth * 1.0M / _ImgWidth, _PnlHeight * 1.0M / _ImgHeight);
            _Scale_origin = _Scale;
            _strImageInfo = _ImgWidth.ToString() + "*" + _ImgHeight.ToString();
            Invalidate();
        }

        private void pnlFov_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int iWidth = e.X - _PnlWidth / 2;
                int iHeight = e.Y - _PnlHeight / 2;
                _pCenter.X = _pCenter.X - iWidth;
                _pCenter.Y = _pCenter.Y - iHeight;
                Invalidate(ClientRectangle);
            }
            else if (e.Button == MouseButtons.Right)
            {
                rBtnClickMenuStrip.Show(this, new Point(e.X, e.Y), ToolStripDropDownDirection.BelowRight);
            }
        }

        private void pnlFov_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = Pp2Ip(e.X, e.Y);
            _xPos = pt.X;
            _yPos = pt.Y;
            Invalidate();
        }

        private void pnlFov_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                ZoomView(1);
            else
                ZoomView(-1);
        }

        private void pnlFov_Paint(object sender, PaintEventArgs e)
        {
            if (_Image != null)
            {
                Rectangle rectImg = Ir2Pr(0, 0, _ImgWidth, _ImgHeight);
                Rectangle rectPanel = new Rectangle(0, 0, _PnlWidth, _PnlHeight);
                Rectangle rectDst = Rectangle.Intersect(rectImg, rectPanel);
                Rectangle rectSrc = Pr2Ir(rectDst);
                Graphics gc = e.Graphics;
                gc.DrawImage(_Image, rectDst, rectSrc, GraphicsUnit.Pixel);
                Pen pen = new Pen(Color.Lime);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                gc.DrawLine(pen, _PnlWidth / 2 - 1, 0, _PnlWidth / 2 - 1, _PnlHeight - 1);
                gc.DrawLine(pen, 0, _PnlHeight / 2 - 1, _PnlWidth - 1, _PnlHeight / 2 - 1);
                pen.Color = Color.Blue;
                pen.Width = 5;
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                gc.DrawRectangle(pen, 0, 0, _PnlWidth, _PnlHeight);
                gc.DrawString("X:"+_xPos.ToString()+",  Y:"+_yPos.ToString(), new Font("Verdana", 12), new SolidBrush(Color.Red), new PointF(20, _PnlHeight-40));
                gc.DrawString(_strImageInfo, new Font("Verdana", 12), new SolidBrush(Color.Red), new PointF(_PnlWidth - 100, _PnlHeight - 40));
                //gc.Dispose();
            }
        }

        private decimal CalScale()
        {
            decimal ret = 0;
            if(_Scale<_Scale_origin)
            {
                ret = 0.05M;
            }
            else if(_Scale >= _Scale_origin && _Scale < 2*_Scale_origin)
            {
                ret = 0.2M;
            }
            else
            {
                ret = 0.4M;
            }
            return ret;
        }

        private void ZoomView(int iMode)
        {
            decimal delta = CalScale();
            if (iMode == 0)
            {
                _Scale = _Scale_origin;
                _pCenter = new Point(_PnlWidth / 2, _PnlHeight / 2);
            }
            else if (iMode == -1)
            {
                _Scale -= delta;
                if (_Scale < 0.2M * _Scale_origin)
                    _Scale += delta;
                else
                {
                    _pCenter.X -= (int)((_pCenter.X - _PnlWidth / 2) * delta / (_Scale + delta));
                    _pCenter.Y -= (int)((_pCenter.Y - _PnlHeight / 2) * delta / (_Scale + delta));
                }
            }
            else if (iMode == 1)
            {
                _Scale += delta;
                if (_Scale > 4.0M * _Scale_origin)
                    _Scale -= delta;
                else
                {
                    _pCenter.X += (int)((_pCenter.X - _PnlWidth / 2) * delta / (_Scale - delta));
                    _pCenter.Y += (int)((_pCenter.Y - _PnlHeight / 2) * delta / (_Scale - delta));
                }
            }
            Invalidate(ClientRectangle);
        }

        private void ZoomView1(int iMode)
        {
            if (iMode == 0)
            {
                _Scale = _Scale_origin;
                _pCenter = new Point(_PnlWidth / 2, _PnlHeight / 2);
            }
            else if (iMode == -1)
            {
                _Scale -= 0.05M;
                if (_Scale < 0.2M * _Scale_origin)
                    _Scale += 0.05M;
                else
                {
                    _pCenter.X -= (int)((_pCenter.X - _PnlWidth / 2) * 0.05M / (_Scale + 0.05M));
                    _pCenter.Y -= (int)((_pCenter.Y - _PnlHeight / 2) * 0.05M / (_Scale + 0.05M));
                }
            }
            else if (iMode == 1)
            {
                _Scale += 0.05M;
                if (_Scale > 4.0M * _Scale_origin)
                    _Scale -= 0.05M;
                else
                {
                    _pCenter.X += (int)((_pCenter.X - _PnlWidth / 2) * 0.05M / (_Scale - 0.05M));
                    _pCenter.Y += (int)((_pCenter.Y - _PnlHeight / 2) * 0.05M / (_Scale - 0.05M));
                }
            }
            Invalidate(ClientRectangle);
        }

        public Point Ip2Pp(Point pt)
        {
            Point ret = new Point(0, 0);
            ret.X = (int)((pt.X - _ImgWidth / 2) * _Scale) + _pCenter.X;
            ret.Y = (int)((pt.Y - _ImgHeight / 2) * _Scale) + _pCenter.Y;
            return ret;
        }

        public Point Ip2Pp(int x, int y)
        {
            return Ip2Pp(new Point(x, y));
        }

        public Rectangle Ir2Pr(Rectangle rect)
        {
            Point pt = Ip2Pp(rect.X, rect.Y);
            int width = (int)(rect.Width * _Scale);
            int height = (int)(rect.Height * _Scale);
            return new Rectangle(pt.X, pt.Y, width, height);
        }

        public Rectangle Ir2Pr(int x, int y, int width, int height)
        {
            return Ir2Pr(new Rectangle(x, y, width, height));
        }

        public Point Pp2Ip(Point pt)
        {
            Point ret = new Point(0, 0);
            ret.X = (int)((pt.X - _pCenter.X) / _Scale) + _ImgWidth / 2;
            ret.Y = (int)((pt.Y - _pCenter.Y) / _Scale) + _ImgHeight / 2;
            return ret;
        }

        public Point Pp2Ip(int x, int y)
        {
            return Pp2Ip(new Point(x, y));
        }

        public Rectangle Pr2Ir(Rectangle rect)
        {
            Point pt = Pp2Ip(rect.X, rect.Y);
            int width = (int)(rect.Width / _Scale);
            int height = (int)(rect.Height / _Scale);
            return new Rectangle(pt.X, pt.Y, width, height);
        }

        public Rectangle Pr2Ir(int x, int y, int width, int height)
        {
            return Pr2Ir(new Rectangle(x, y, width, height));
        }

        private void rBtnClickMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == btnTsFit)
                ZoomView(0);
            else if (e.ClickedItem == btnTsZoomIn)
                ZoomView(-1);
            else if (e.ClickedItem == btnTsZoomOut)
                ZoomView(1);
        }
    }
}
