using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Wells.Tools;

namespace Wells.Controls.ImageView
{
    public class CameraView
    {
        public int m_iIndex;//index
        public Point m_ptCenter;//center//物理坐标系
        public Point m_ptMarkOffset;//mark偏移
        public Bitmap m_bmp;
        private clsPointBitmap _lockbmp;
        private bool m_bLock;
        private ImageView imageView;

        public CameraView()
        {
            #region 默认参数

            m_iIndex = 0;
            m_ptCenter = new Point(0, 0);
            m_ptMarkOffset = new Point(0, 0);
            m_bmp = null;
            imageView = null;
            _lockbmp = null;
            m_bLock = false;

            #endregion
        }

        public void LinkToView(ImageView imgview)
        {
            #region 绑定父view

            imageView = imgview;

            #endregion
        }

        #region 坐标系转换功能

        public int LpToVp(int s)
        {
            #region 物理坐标到视图坐标

            return (int)Math.Round((decimal)s * 2 * 1000 / (PCB.m_pPCB.m_uResolutionX + PCB.m_pPCB.m_uResolutionY));

            #endregion
        }

        public Point LpToVp(Point pt)
        {
            #region 物理坐标到视图坐标

            int X = 0, Y = 0;

            if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftDown)
            {
                X = (m_ptCenter.X + m_ptMarkOffset.X) - pt.X;
                Y = -(m_ptCenter.Y + m_ptMarkOffset.Y) + pt.Y;
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftUp)
            {
                X = (m_ptCenter.X + m_ptMarkOffset.X) + pt.X;
                Y = (m_ptCenter.Y + m_ptMarkOffset.Y) - pt.Y;
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.RightDown)
            {
                X = -(m_ptCenter.X + m_ptMarkOffset.X) + pt.X;
                Y = -(m_ptCenter.Y + m_ptMarkOffset.Y) + pt.Y;
            }
            else
            {
                X = -(m_ptCenter.X + m_ptMarkOffset.X) + pt.X;
                Y = (m_ptCenter.Y + m_ptMarkOffset.Y) - pt.Y;
            }
            
            X = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeX / 2 - X) * 1000 / PCB.m_pPCB.m_uResolutionX);
            Y = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeY / 2 - Y) * 1000 / PCB.m_pPCB.m_uResolutionY);

            return new Point(X, Y);

            #endregion
        }

        public Point VpToLp(Point pt)
        {
            #region 视图坐标到物理坐标

            int X = 0, Y = 0;

            if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftDown)
            {
                X = (int)Math.Round((decimal)(-PCB.m_pPCB.m_uFovSizeX / 2 + X) * 1000 / PCB.m_pPCB.m_uResolutionX);
                Y = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeY / 2 - Y) * 1000 / PCB.m_pPCB.m_uResolutionY);
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftUp)
            {
                X = (int)Math.Round((decimal)(-PCB.m_pPCB.m_uFovSizeX / 2 + X) * 1000 / PCB.m_pPCB.m_uResolutionX);
                Y = (int)Math.Round((decimal)(-PCB.m_pPCB.m_uFovSizeY / 2 + Y) * 1000 / PCB.m_pPCB.m_uResolutionY);
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.RightDown)
            {
                X = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeX / 2 - X) * 1000 / PCB.m_pPCB.m_uResolutionX);
                Y = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeY / 2 - Y) * 1000 / PCB.m_pPCB.m_uResolutionY);
            }
            else
            {
                X = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeX / 2 - X) * 1000 / PCB.m_pPCB.m_uResolutionX);
                Y = (int)Math.Round((decimal)(-PCB.m_pPCB.m_uFovSizeY / 2 + Y) * 1000 / PCB.m_pPCB.m_uResolutionY);
            }
            
            X = (m_ptCenter.X + m_ptMarkOffset.X) + X;
            Y = (m_ptCenter.Y + m_ptMarkOffset.Y) + Y;

            return new Point(X, Y);

            #endregion
        }

        public Rectangle LpToVp(Rectangle rect)
        {
            #region 物理坐标到视图坐标

            int Left = 0, Right = 0, Top = 0, Bottom = 0;

            if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftDown)
            {
                Left = (m_ptCenter.X + m_ptMarkOffset.X) - rect.Left;
                Right = (m_ptCenter.X + m_ptMarkOffset.X) - rect.Right;
                Top = -(m_ptCenter.Y + m_ptMarkOffset.Y) + rect.Top;
                Bottom = -(m_ptCenter.Y + m_ptMarkOffset.Y) + rect.Bottom;
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftUp)
            {
                Left = (m_ptCenter.X + m_ptMarkOffset.X) - rect.Left;
                Right =( m_ptCenter.X + m_ptMarkOffset.X) - rect.Right;
                Top = (m_ptCenter.Y + m_ptMarkOffset.Y) - rect.Top;
                Bottom = (m_ptCenter.Y + m_ptMarkOffset.Y) - rect.Bottom;
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.RightDown)
            {
                Left = -(m_ptCenter.X + m_ptMarkOffset.X) + rect.Left;
                Right = -(m_ptCenter.X + m_ptMarkOffset.X) + rect.Right;
                Top = -(m_ptCenter.Y + m_ptMarkOffset.Y) + rect.Top;
                Bottom = -(m_ptCenter.Y + m_ptMarkOffset.Y) + rect.Bottom;
            }
            else
            {
                Left = -(m_ptCenter.X + m_ptMarkOffset.X) + rect.Left;
                Right = -(m_ptCenter.X + m_ptMarkOffset.X) + rect.Right;
                Top = (m_ptCenter.Y + m_ptMarkOffset.Y) - rect.Top;
                Bottom = (m_ptCenter.Y + m_ptMarkOffset.Y) - rect.Bottom;
            }

            Left = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeX / 2 - Left) * 1000 / PCB.m_pPCB.m_uResolutionX);
            Right = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeX / 2 - Right) * 1000 / PCB.m_pPCB.m_uResolutionX);
            Top = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeY / 2 - Top) * 1000 / PCB.m_pPCB.m_uResolutionY);
            Bottom = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeY / 2 - Bottom) * 1000 / PCB.m_pPCB.m_uResolutionY);

            if (Left > Right) { Left = Left + Right; Right = Left - Right; Left = Left - Right; }
            if (Top > Bottom) { Top = Top + Bottom; Bottom = Top - Bottom; Top = Top - Bottom; }

            return new Rectangle(Left, Top, Right - Left, Bottom - Top);

            #endregion
        }

        public Rectangle VpToLp(Rectangle rect)
        {
            #region 视图坐标到物理坐标

            int Left = 0, Right = 0, Top = 0, Bottom = 0;

            if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftDown)
            {
                Left = (int)Math.Round((decimal)(-PCB.m_pPCB.m_uFovSizeX / 2 + Left) * 1000 / PCB.m_pPCB.m_uResolutionX);
                Right = (int)Math.Round((decimal)(-PCB.m_pPCB.m_uFovSizeX / 2 + Right) * 1000 / PCB.m_pPCB.m_uResolutionX);
                Top = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeY / 2 - Top) * 1000 / PCB.m_pPCB.m_uResolutionY);
                Bottom = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeY / 2 - Bottom) * 1000 / PCB.m_pPCB.m_uResolutionY);
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftUp)
            {
                Left = (int)Math.Round((decimal)(-PCB.m_pPCB.m_uFovSizeX / 2 + Left) * 1000 / PCB.m_pPCB.m_uResolutionX);
                Right = (int)Math.Round((decimal)(-PCB.m_pPCB.m_uFovSizeX / 2 + Right) * 1000 / PCB.m_pPCB.m_uResolutionX);
                Top = (int)Math.Round((decimal)(-PCB.m_pPCB.m_uFovSizeY / 2 + Top) * 1000 / PCB.m_pPCB.m_uResolutionY);
                Bottom = (int)Math.Round((decimal)(-PCB.m_pPCB.m_uFovSizeY / 2 + Bottom) * 1000 / PCB.m_pPCB.m_uResolutionY);
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.RightDown)
            {
                Left = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeX / 2 - Left) * 1000 / PCB.m_pPCB.m_uResolutionX);
                Right = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeX / 2 - Right) * 1000 / PCB.m_pPCB.m_uResolutionX);
                Top = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeY / 2 - Top) * 1000 / PCB.m_pPCB.m_uResolutionY);
                Bottom = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeY / 2 - Bottom) * 1000 / PCB.m_pPCB.m_uResolutionY);
            }
            else
            {
                Left = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeX / 2 - Left) * 1000 / PCB.m_pPCB.m_uResolutionX);
                Right = (int)Math.Round((decimal)(PCB.m_pPCB.m_uFovSizeX / 2 - Right) * 1000 / PCB.m_pPCB.m_uResolutionX);
                Top = (int)Math.Round((decimal)(-PCB.m_pPCB.m_uFovSizeY / 2 + Top) * 1000 / PCB.m_pPCB.m_uResolutionY);
                Bottom = (int)Math.Round((decimal)(-PCB.m_pPCB.m_uFovSizeY / 2 + Bottom) * 1000 / PCB.m_pPCB.m_uResolutionY);
            }

            Left = (m_ptCenter.X + m_ptMarkOffset.X) + Left;
            Right =(m_ptCenter.X + m_ptMarkOffset.X) + Right;
            Top = (m_ptCenter.Y + m_ptMarkOffset.Y) + Top;
            Bottom = (m_ptCenter.Y + m_ptMarkOffset.Y) + Bottom;

            if (Left > Right) { Left = Left + Right; Right = Left - Right; Left = Left - Right; }
            if (Top > Bottom) { Top = Top + Bottom; Bottom = Top - Bottom; Top = Top - Bottom; }

            return new Rectangle(Left, Top, Right - Left, Bottom - Top);

            #endregion
        }
        
        public Rectangle GetAbsoluteRect()
        {
            #region 获取绝对物理坐标矩形

            return new Rectangle(m_ptCenter.X - PCB.m_pPCB.m_uFovSizeX / 2, m_ptCenter.Y - PCB.m_pPCB.m_uFovSizeY / 2, PCB.m_pPCB.m_uFovSizeX, PCB.m_pPCB.m_uFovSizeY);

            #endregion
        }

        #endregion

        public void SetViewImage(Bitmap bmp)
        {
            #region 创建视图图像

            m_bmp = bmp;

            #endregion
        }

        public void SetViewImage(string path)
        {
            #region 创建视图图像

            m_bmp = new Bitmap(Image.FromFile(path));

            #endregion
        }

        public bool GetPixelViewImage(Point pt,out Color color)
        {
            #region 通过物理坐标获取图像像素数据

            bool ret = false;
            color = Color.FromArgb(100, 100, 100);

            if(m_bmp!=null)
            {
                if (_lockbmp==null)
                {
                    _lockbmp = new clsPointBitmap(m_bmp);
                    _lockbmp.lockBits();
                    m_bLock = true;
                }
                else if(!m_bLock)
                {
                    _lockbmp.lockBits();
                    m_bLock = true;
                }
                //PointBitmap _lockbmp = new PointBitmap(m_bmp);
                //_lockbmp.LockBits();
                pt = LpToVp(pt);
                if (pt.X >= 0 && pt.Y >= 0 && pt.X < _lockbmp.Width && pt.Y < _lockbmp.Height) 
                {
                    color = _lockbmp.getPixel(pt.X, pt.Y);
                    ret = true;
                }
                //_lockbmp.UnlockBits();
            }

            return ret;

            #endregion
        }

        public void UnLockBitmap()
        {
            #region 解锁图像内存

            if (m_bmp != null)
            {
                if (_lockbmp != null)
                {
                    if (m_bLock)
                    {
                        _lockbmp.unlockBits();
                        m_bLock = false;
                    }
                }
            }

            #endregion
        }

        public bool GetPixelLiveImage(Point pt, out Color color)
        {
            #region 通过像素坐标获取图像像素数据

            bool ret = false;
            color = Color.FromArgb(100, 100, 100);

            if (m_bmp != null)
            {
                clsPointBitmap _lockbmp = new clsPointBitmap(m_bmp);
                _lockbmp.lockBits();
                if (pt.X < _lockbmp.Width && pt.Y < _lockbmp.Height)
                {
                    color = _lockbmp.getPixel(pt.X, pt.Y);
                    ret = true;
                }
                _lockbmp.unlockBits();
            }

            return ret;

            #endregion
        }

        public void Draw(Graphics g,Rectangle rect)
        {
            #region 绘图操作

            if(imageView!=null)
            {
                if(imageView.m_iViewType == ConstData.View_Area)
                {
                    using (Pen pen = new Pen(Color.White, 1))
                    {
                        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        g.DrawRectangle(pen, rect);
                        g.FillRectangle((Brush)Brushes.Aqua, rect.X, rect.Y, 20, 20);
                        g.DrawString(m_iIndex.ToString(), new Font("Arial", 10), (Brush)Brushes.Blue, rect.X, rect.Y);
                    }
                }
                else if(imageView.m_iViewType == ConstData.View_Camera)
                {
                    
                }

                if (m_bmp != null)
                {
                    //g.DrawImage(m_bmp, rect);
                }
            }
            
            #endregion
        }

        public void Dispose()
        {
            #region 清空所占资源

            if(m_bmp!=null)
            {
                if(_lockbmp!=null)
                {
                    if (m_bLock)
                    {
                        _lockbmp.unlockBits();
                        m_bLock = false;
                    }
                    _lockbmp = null;
                }
                m_bmp.Dispose();
                m_bmp = null;
            }

            #endregion
        }
    }
}
