using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Wells.Controls.VisionInspect
{
    public class clsCameraView
    {
        /// <summary>
        /// 绘图号笔
        /// </summary>
        private Pen pen = new Pen(Color.White, 1);

        /// <summary>
        /// 绘图字体
        /// </summary>
        private Font font = new Font("Arial", 10);

        /// <summary>
        /// 视图索引
        /// </summary>
        public int m_iIndex;

        /// <summary>
        /// 视图物理坐标系坐标
        /// </summary>
        public Point m_lptCenter;

        /// <summary>
        /// mark偏移坐标
        /// </summary>
        public Point m_lptMarkOffset;

        /// <summary>
        /// 图片数据
        /// </summary>
        public clsImage m_image;

        /// <summary>
        /// 绑定的视图控件
        /// </summary>
        internal ImageDoc imageDoc;

        public clsCameraView()
        {
            #region 默认参数

            m_iIndex = 0;
            m_lptCenter = new Point(0, 0);
            m_lptMarkOffset = new Point(0, 0);
            m_image = new clsImage();
            imageDoc = null;

            #endregion
        }

        public void linkToView(ImageDoc imgdoc)
        {
            #region 绑定父view

            imageDoc = imgdoc;

            #endregion
        }

        #region ***** 坐标系转换功能 *****

        #region ***** 重点说明 *****

        //这里的视图坐标系要考虑到图片的存储方式，比如图片以文件方式写入时，保存数据的格式是从图片左下角开始存储的，但是相机可能的存储方式是从左上角开始的，所以要区分

        #endregion

        #region ***** 坐标系转换功能，cameraview以左上角为坐标原点 *****

        public int LpToVp(int s)
        {
            #region 物理坐标到视图坐标

            return (int)Math.Round((decimal)s * 2 * 1000 / (clsPCB.m_pPCB.m_uResolutionX + clsPCB.m_pPCB.m_uResolutionY));

            #endregion
        }

        public Point LpToVp(Point pt)
        {
            #region 物理坐标到视图坐标

            int X = 0, Y = 0;

            if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftDown)
            {
                if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftDown)
                {
                    X = (m_lptCenter.X + m_lptMarkOffset.X) - pt.X;
                    Y = (m_lptCenter.Y + m_lptMarkOffset.Y) - pt.Y;
                }
                else if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftUp)
                {
                    X = (m_lptCenter.X + m_lptMarkOffset.X) - pt.X;
                    Y = -(m_lptCenter.Y + m_lptMarkOffset.Y) + pt.Y;
                }
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftUp)
            {
                if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftDown)
                {
                    X = (m_lptCenter.X + m_lptMarkOffset.X) - pt.X;
                    Y = -(m_lptCenter.Y + m_lptMarkOffset.Y) + pt.Y;
                }
                else if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftUp)
                {
                    X = (m_lptCenter.X + m_lptMarkOffset.X) - pt.X;
                    Y = (m_lptCenter.Y + m_lptMarkOffset.Y) - pt.Y;
                }
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.RightDown)
            {
                if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftDown)
                {
                    X = -(m_lptCenter.X + m_lptMarkOffset.X) + pt.X;
                    Y = (m_lptCenter.Y + m_lptMarkOffset.Y) - pt.Y;
                }
                else if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftUp)
                {
                    X = -(m_lptCenter.X + m_lptMarkOffset.X) + pt.X;
                    Y = -(m_lptCenter.Y + m_lptMarkOffset.Y) + pt.Y;
                }
            }
            else
            {
                if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftDown)
                {
                    X = -(m_lptCenter.X + m_lptMarkOffset.X) + pt.X;
                    Y = -(m_lptCenter.Y + m_lptMarkOffset.Y) + pt.Y;
                }
                else if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftUp)
                {
                    X = -(m_lptCenter.X + m_lptMarkOffset.X) + pt.X;
                    Y = (m_lptCenter.Y + m_lptMarkOffset.Y) - pt.Y;
                }
            }

            X = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_uFovSizeX / 2 - X) * 1000 / clsPCB.m_pPCB.m_uResolutionX);
            Y = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_uFovSizeY / 2 - Y) * 1000 / clsPCB.m_pPCB.m_uResolutionY);

            return new Point(X, Y);

            #endregion
        }

        public Point VpToLp(Point pt)
        {
            #region 视图坐标到物理坐标

            int X = pt.X, Y = pt.Y;

            if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftDown)
            {
                if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftDown)
                {
                    X = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelWidth / 2 + X) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                    Y = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelHeight / 2 + Y) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
                }
                else if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftUp)
                {
                    X = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelWidth / 2 + X) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                    Y = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelHeight / 2 - Y) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
                }
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftUp)
            {
                if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftDown)
                {
                    X = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelWidth / 2 + X) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                    Y = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelHeight / 2 - Y) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
                }
                else if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftUp)
                {
                    X = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelWidth / 2 + X) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                    Y = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelHeight / 2 + Y) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
                }
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.RightDown)
            {
                if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftDown)
                {
                    X = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelWidth / 2 - X) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                    Y = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelHeight / 2 + Y) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
                }
                else if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftUp)
                {
                    X = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelWidth / 2 - X) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                    Y = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelHeight / 2 - Y) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
                }
            }
            else
            {
                if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftDown)
                {
                    X = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelWidth / 2 - X) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                    Y = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelHeight / 2 - Y) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
                }
                else if (clsPCB.m_pPCB.m_iImageCoordinateType == tagImageCoordinateType.LeftUp)
                {
                    X = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelWidth / 2 - X) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                    Y = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelHeight / 2 + Y) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
                }
            }

            X = (m_lptCenter.X + m_lptMarkOffset.X) + X;
            Y = (m_lptCenter.Y + m_lptMarkOffset.Y) + Y;

            return new Point(X, Y);

            #endregion
        }

        public Rectangle LpToVp(Rectangle rect)
        {
            #region 物理坐标到视图坐标

            int Left = 0, Right = 0, Top = 0, Bottom = 0;

            if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftDown)
            {
                Left = (m_lptCenter.X + m_lptMarkOffset.X) - rect.Left;
                Right = (m_lptCenter.X + m_lptMarkOffset.X) - rect.Right;
                Top = -(m_lptCenter.Y + m_lptMarkOffset.Y) + rect.Top;
                Bottom = -(m_lptCenter.Y + m_lptMarkOffset.Y) + rect.Bottom;
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftUp)
            {
                Left = (m_lptCenter.X + m_lptMarkOffset.X) - rect.Left;
                Right = (m_lptCenter.X + m_lptMarkOffset.X) - rect.Right;
                Top = (m_lptCenter.Y + m_lptMarkOffset.Y) - rect.Top;
                Bottom = (m_lptCenter.Y + m_lptMarkOffset.Y) - rect.Bottom;
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.RightDown)
            {
                Left = -(m_lptCenter.X + m_lptMarkOffset.X) + rect.Left;
                Right = -(m_lptCenter.X + m_lptMarkOffset.X) + rect.Right;
                Top = -(m_lptCenter.Y + m_lptMarkOffset.Y) + rect.Top;
                Bottom = -(m_lptCenter.Y + m_lptMarkOffset.Y) + rect.Bottom;
            }
            else
            {
                Left = -(m_lptCenter.X + m_lptMarkOffset.X) + rect.Left;
                Right = -(m_lptCenter.X + m_lptMarkOffset.X) + rect.Right;
                Top = (m_lptCenter.Y + m_lptMarkOffset.Y) - rect.Top;
                Bottom = (m_lptCenter.Y + m_lptMarkOffset.Y) - rect.Bottom;
            }

            Left = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_uFovSizeX / 2 - Left) * 1000 / clsPCB.m_pPCB.m_uResolutionX);
            Right = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_uFovSizeX / 2 - Right) * 1000 / clsPCB.m_pPCB.m_uResolutionX);
            Top = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_uFovSizeY / 2 - Top) * 1000 / clsPCB.m_pPCB.m_uResolutionY);
            Bottom = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_uFovSizeY / 2 - Bottom) * 1000 / clsPCB.m_pPCB.m_uResolutionY);

            if (Left > Right) { Left = Left + Right; Right = Left - Right; Left = Left - Right; }
            if (Top > Bottom) { Top = Top + Bottom; Bottom = Top - Bottom; Top = Top - Bottom; }

            return new Rectangle(Left, Top, Right - Left, Bottom - Top);

            #endregion
        }

        public Rectangle VpToLp(Rectangle rect)
        {
            #region 视图坐标到物理坐标

            int Left = rect.Left, Right = rect.Right, Top = rect.Top, Bottom = rect.Bottom;

            if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftDown)
            {
                Left = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelWidth / 2 + Left) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                Right = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelWidth / 2 + Right) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                Top = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelHeight / 2 - Top) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
                Bottom = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelHeight / 2 - Bottom) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftUp)
            {
                Left = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelWidth / 2 + Left) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                Right = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelWidth / 2 + Right) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                Top = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelHeight / 2 + Top) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
                Bottom = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelHeight / 2 + Bottom) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.RightDown)
            {
                Left = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelWidth / 2 - Left) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                Right = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelWidth / 2 - Right) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                Top = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelHeight / 2 - Top) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
                Bottom = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelHeight / 2 - Bottom) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
            }
            else
            {
                Left = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelWidth / 2 - Left) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                Right = (int)Math.Round((decimal)(clsPCB.m_pPCB.m_pFovPixelWidth / 2 - Right) / 1000 * clsPCB.m_pPCB.m_uResolutionX);
                Top = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelHeight / 2 + Top) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
                Bottom = (int)Math.Round((decimal)(-clsPCB.m_pPCB.m_pFovPixelHeight / 2 + Bottom) / 1000 * clsPCB.m_pPCB.m_uResolutionY);
            }

            Left = (m_lptCenter.X + m_lptMarkOffset.X) + Left;
            Right = (m_lptCenter.X + m_lptMarkOffset.X) + Right;
            Top = (m_lptCenter.Y + m_lptMarkOffset.Y) + Top;
            Bottom = (m_lptCenter.Y + m_lptMarkOffset.Y) + Bottom;

            if (Left > Right) { Left = Left + Right; Right = Left - Right; Left = Left - Right; }
            if (Top > Bottom) { Top = Top + Bottom; Bottom = Top - Bottom; Top = Top - Bottom; }

            return new Rectangle(Left, Top, Right - Left, Bottom - Top);

            #endregion
        }

        public Rectangle getAbsoluteRect()
        {
            #region 获取绝对物理坐标矩形

            return new Rectangle(m_lptCenter.X - clsPCB.m_pPCB.m_uFovSizeX / 2, m_lptCenter.Y - clsPCB.m_pPCB.m_uFovSizeY / 2, clsPCB.m_pPCB.m_uFovSizeX, clsPCB.m_pPCB.m_uFovSizeY);

            #endregion
        }

        #endregion

        #endregion

        public void setViewImage(clsImage img)
        {
            #region 创建视图图像

            m_image = img;

            #endregion
        }

        public void setViewImage(string path)
        {
            #region 创建视图图像

            //m_bmp = new Bitmap(Image.FromFile(path));

            #endregion
        }

        public bool getPixelViewImage(Point pt, out byte R, out byte G, out byte B)
        {
            #region 通过物理坐标获取图像像素数据

            bool ret = false;
            R = G = B = 100;

            if (m_image.ImgBuffer != null)
            {
                pt = LpToVp(pt);
                if (pt.X >= 0 && pt.Y >= 0 && pt.X < m_image.Width && pt.Y < m_image.Height)
                {
                    if (m_image.Color)
                    {
                        B = m_image.ImgBuffer[(pt.Y * m_image.Width + pt.X) * 3];
                        G = m_image.ImgBuffer[(pt.Y * m_image.Width + pt.X) * 3 + 1];
                        R = m_image.ImgBuffer[(pt.Y * m_image.Width + pt.X) * 3 + 2];
                    }
                    else
                    {
                        R = G = B = m_image.ImgBuffer[pt.Y * m_image.Width + pt.X];
                    }
                    ret = true;
                }
            }

            return ret;

            #endregion
        }

        public bool getPixelLiveImage(Point pt, out byte R, out byte G, out byte B)
        {
            #region 通过像素坐标获取图像像素数据

            bool ret = false;
            R = G = B = 100;

            return ret;

            #endregion
        }

        public void draw(Graphics g, Rectangle rect)
        {
            #region 绘图操作

            if (imageDoc != null)
            {
                if (m_image.isInitialized())
                {
                    g.drawImage(m_image, rect);
                }
                else
                {
                    g.FillRectangle((Brush)Brushes.DarkGray, rect);
                }
            }

            #endregion
        }

        public void drawScreenRect(Graphics g, Rectangle rect, Color color, System.Drawing.Drawing2D.DashStyle style)
        {
            #region ***** 绘制body *****

            pen.DashStyle = style;
            pen.Color = color;
            g.DrawRectangle(pen, rect);
            g.FillRectangle(Brushes.Aqua, rect.X, rect.Y, 20, 20);
            g.DrawString(m_iIndex.ToString("00"), font, Brushes.Blue, rect.X, rect.Y);

            #endregion
        }
    }
}
