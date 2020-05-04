using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using Wells.Tools;

namespace Wells.Controls.ImageView
{
    public class LiveView1
    {
        public decimal m_iScale;
        public decimal m_iFitScale;
        public decimal[] m_iScaleList;
        public Point m_ptTopLeft;//不管什么坐标系，屏幕绝对左上角，与矩形结构的topleft不一样
        public Point m_ptBottomRight;//不管什么坐标系，屏蔽绝对右下角，与矩形结构的bottomright不一样
        public Rectangle m_rcScreenArea;//控件屏幕区域，物理坐标系
        public Rectangle m_rcPCBImageArea;//图片显示区域，视图坐标系
        public Point m_ptCenter;
        public Bitmap m_bmpLive;
        public bool m_bIsColor;
        private ImageView imageView = null;
        public CameraView m_pLiveView;

        public LiveView1()
        {
            #region 初始化
            m_iScale = 1.0M;
            m_iScaleList = new decimal[11];
            m_ptTopLeft = new Point(0, 0);
            m_ptBottomRight = new Point(1920, 1200);
            m_ptCenter = new Point(960, 600);
            m_rcScreenArea = new Rectangle(0, 0, 1920, 1200);
            m_rcPCBImageArea = new Rectangle(0, 0, 1920, 1200);
            imageView = null;
            m_pLiveView = null;
            m_bIsColor = false;
            #endregion
        }

        public void LinkToView(ImageView imgview)
        {
            #region 绑定父view

            imageView = imgview;

            #endregion
        }

        #region 坐标系转换功能

        public Point LpToVp(Point pt)
        {
            #region 物理坐标到视图坐标

            int X = 0, Y = 0;

            if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftDown)
            {
                X = -m_ptTopLeft.X + pt.X;
                Y = m_ptTopLeft.Y - pt.Y;
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftUp)
            {
                X = -m_ptTopLeft.X + pt.X;
                Y = -m_ptTopLeft.Y + pt.Y;
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.RightDown)
            {
                X = m_ptTopLeft.X - pt.X;
                Y = m_ptTopLeft.Y - pt.Y;
            }
            else
            {
                X = m_ptTopLeft.X - pt.X;
                Y = -m_ptTopLeft.Y + pt.Y;
            }

            X = (int)Math.Round((decimal)X * 1000 / PCB.m_pPCB.m_uResolutionX / m_iScale);
            Y = (int)Math.Round((decimal)Y * 1000 / PCB.m_pPCB.m_uResolutionY / m_iScale);

            return new Point(X, Y);

            #endregion
        }

        public Point VpToLp(Point pt)
        {
            #region 视图坐标到物理坐标

            int X = 0, Y = 0;

            X = (int)Math.Round((decimal)pt.X / 1000 * PCB.m_pPCB.m_uResolutionX * m_iScale);
            Y = (int)Math.Round((decimal)pt.Y / 1000 * PCB.m_pPCB.m_uResolutionY * m_iScale);

            if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftDown)
            {
                X = m_ptTopLeft.X + X;
                Y = m_ptTopLeft.Y - Y;
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftUp)
            {
                X = m_ptTopLeft.X + X;
                Y = m_ptTopLeft.Y + Y;
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.RightDown)
            {
                X = m_ptTopLeft.X - X;
                Y = m_ptTopLeft.Y - Y;
            }
            else
            {
                X = m_ptTopLeft.X - X;
                Y = m_ptTopLeft.Y + Y;
            }

            return new Point(X, Y);

            #endregion
        }

        public Rectangle LpToVp(Rectangle rect)
        {
            #region 物理坐标到视图坐标

            int Left = 0, Right = 0, Top = 0, Bottom = 0;

            if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftDown)
            {
                Left = -m_ptTopLeft.X + rect.Left;
                Right = -m_ptTopLeft.X + rect.Right;
                Top = m_ptTopLeft.Y - rect.Top;
                Bottom = m_ptTopLeft.Y - rect.Bottom;
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftUp)
            {
                Left = -m_ptTopLeft.X + rect.Left;
                Right = -m_ptTopLeft.X + rect.Right;
                Top = -m_ptTopLeft.Y + rect.Top;
                Bottom = -m_ptTopLeft.Y + rect.Bottom;
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.RightDown)
            {
                Left = m_ptTopLeft.X - rect.Left;
                Right = m_ptTopLeft.X - rect.Right;
                Top = m_ptTopLeft.Y - rect.Top;
                Bottom = m_ptTopLeft.Y - rect.Bottom;
            }
            else
            {
                Left = m_ptTopLeft.X - rect.Left;
                Right = m_ptTopLeft.X - rect.Right;
                Top = -m_ptTopLeft.Y + rect.Top;
                Bottom = -m_ptTopLeft.Y + rect.Bottom;
            }

            Left = (int)Math.Round((decimal)Left * 1000 / PCB.m_pPCB.m_uResolutionX / m_iScale);
            Right = (int)Math.Round((decimal)Right * 1000 / PCB.m_pPCB.m_uResolutionX / m_iScale);
            Top = (int)Math.Round((decimal)Top * 1000 / PCB.m_pPCB.m_uResolutionY / m_iScale);
            Bottom = (int)Math.Round((decimal)Bottom * 1000 / PCB.m_pPCB.m_uResolutionY / m_iScale);

            if (Left > Right) { Left = Left + Right; Right = Left - Right; Left = Left - Right; }
            if (Top > Bottom) { Top = Top + Bottom; Bottom = Top - Bottom; Top = Top - Bottom; }

            return new Rectangle(Left, Top, Math.Abs(Right - Left), Math.Abs(Bottom - Top));

            #endregion
        }

        public Rectangle VpToLp(Rectangle rect)
        {
            #region 视图坐标到物理坐标

            int Left = 0, Right = 0, Top = 0, Bottom = 0;

            Left = (int)Math.Round((decimal)rect.Left / 1000 * PCB.m_pPCB.m_uResolutionX * m_iScale);
            Right = (int)Math.Round((decimal)rect.Right / 1000 * PCB.m_pPCB.m_uResolutionX * m_iScale);
            Top = (int)Math.Round((decimal)rect.Top / 1000 * PCB.m_pPCB.m_uResolutionY * m_iScale);
            Bottom = (int)Math.Round((decimal)rect.Bottom / 1000 * PCB.m_pPCB.m_uResolutionY * m_iScale);

            if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftDown)
            {
                Left = m_ptTopLeft.X + Left;
                Right = m_ptTopLeft.X + Right;
                Top = m_ptTopLeft.Y - Top;
                Bottom = m_ptTopLeft.Y - Bottom;
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftUp)
            {
                Left = m_ptTopLeft.X + Left;
                Right = m_ptTopLeft.X + Right;
                Top = m_ptTopLeft.Y + Top;
                Bottom = m_ptTopLeft.Y + Bottom;
            }
            else if (PCB.m_pPCB.m_iCoordinateType == ConstData.RightDown)
            {
                Left = m_ptTopLeft.X - Left;
                Right = m_ptTopLeft.X - Right;
                Top = m_ptTopLeft.Y - Top;
                Bottom = m_ptTopLeft.Y - Bottom;
            }
            else
            {
                Left = m_ptTopLeft.X - Left;
                Right = m_ptTopLeft.X - Right;
                Top = m_ptTopLeft.Y + Top;
                Bottom = m_ptTopLeft.Y + Bottom;
            }

            if (Left > Right) { Left = Left + Right; Right = Left - Right; Left = Left - Right; }
            if (Top > Bottom) { Top = Top + Bottom; Bottom = Top - Bottom; Top = Top - Bottom; }

            return new Rectangle(Left, Top, Right - Left, Bottom - Top);

            #endregion
        }

        #endregion

        public void CalFitScale()
        {
            #region 计算最适屏幕的scale

            m_iFitScale = Math.Max((decimal)PCB.m_pPCB.m_pFovPixelWidth / m_rcScreenArea.Width, (decimal)PCB.m_pPCB.m_pFovPixelHeight / m_rcScreenArea.Height);
            m_iFitScale *= 1.1M;

            m_iScaleList[0] = m_iFitScale / 5.0M;
            m_iScaleList[1] = m_iFitScale / 4.2M;
            m_iScaleList[2] = m_iFitScale / 3.4M;
            m_iScaleList[3] = m_iFitScale / 2.6M;
            m_iScaleList[4] = m_iFitScale / 1.8M;
            m_iScaleList[5] = m_iFitScale;
            m_iScaleList[6] = m_iFitScale * 1.8M;
            m_iScaleList[7] = m_iFitScale * 2.6M;
            m_iScaleList[8] = m_iFitScale * 3.4M;
            m_iScaleList[9] = m_iFitScale * 4.2M;
            m_iScaleList[10] = m_iFitScale * 5.0M;

            #endregion
        }

        public bool PrepareLiveView(Rectangle rect, bool bFit = false)
        {
            #region 准备主区域视图显示

            bool ret = false;

            if (rect.Width > 0 && rect.Height > 0)
            {
                m_rcScreenArea = rect;

                CalFitScale();

                //m_ptCenter = new Point(PCB.m_pPCB.m_uFovSizeX / 2, PCB.m_pPCB.m_uFovSizeY / 2);
                m_ptCenter = m_pLiveView.m_ptCenter;

                if (bFit)
                    m_iScale = m_iFitScale;

                int X = (int)Math.Round((decimal)m_rcScreenArea.Width / 2 / 1000 * PCB.m_pPCB.m_uResolutionX * m_iScale);
                int Y = (int)Math.Round((decimal)m_rcScreenArea.Height / 2 / 1000 * PCB.m_pPCB.m_uResolutionY * m_iScale);

                if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftDown)
                {
                    m_ptTopLeft.X = m_ptCenter.X - X;
                    m_ptTopLeft.Y = m_ptCenter.Y + Y;
                    m_ptBottomRight.X = m_ptCenter.X + X;
                    m_ptBottomRight.Y = m_ptCenter.Y - Y;
                }
                else if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftUp)
                {
                    m_ptTopLeft.X = m_ptCenter.X - X;
                    m_ptTopLeft.Y = m_ptCenter.Y - Y;
                    m_ptBottomRight.X = m_ptCenter.X + X;
                    m_ptBottomRight.Y = m_ptCenter.Y + Y;
                }
                else if (PCB.m_pPCB.m_iCoordinateType == ConstData.RightDown)
                {
                    m_ptTopLeft.X = m_ptCenter.X + X;
                    m_ptTopLeft.Y = m_ptCenter.Y + Y;
                    m_ptBottomRight.X = m_ptCenter.X - X;
                    m_ptBottomRight.Y = m_ptCenter.Y - Y;
                }
                else
                {
                    m_ptTopLeft.X = m_ptCenter.X + X;
                    m_ptTopLeft.Y = m_ptCenter.Y - Y;
                    m_ptBottomRight.X = m_ptCenter.X - X;
                    m_ptBottomRight.Y = m_ptCenter.Y + Y;
                }

                Rectangle temp = new Rectangle(m_ptCenter.X - PCB.m_pPCB.m_uFovSizeX / 2, m_ptCenter.Y - PCB.m_pPCB.m_uFovSizeY / 2, PCB.m_pPCB.m_uFovSizeX, PCB.m_pPCB.m_uFovSizeY);

                m_rcPCBImageArea = LpToVp(temp);

                if (m_rcPCBImageArea.X < 0) { m_rcPCBImageArea.Width += m_rcPCBImageArea.X; m_rcPCBImageArea.X = 0; }
                if (m_rcPCBImageArea.Y < 0) { m_rcPCBImageArea.Height += m_rcPCBImageArea.Y; m_rcPCBImageArea.Y = 0; }
                if (m_rcPCBImageArea.Width + m_rcPCBImageArea.X > m_rcScreenArea.Width) m_rcPCBImageArea.Width = m_rcScreenArea.Width - m_rcPCBImageArea.X;
                if (m_rcPCBImageArea.Height + m_rcPCBImageArea.Y > m_rcScreenArea.Height) m_rcPCBImageArea.Height = m_rcScreenArea.Height - m_rcPCBImageArea.Y;

                ret = true;
            }

            return ret;

            #endregion
        }

        public void ClearCameraView()
        {
            #region 清空liveview

            if (m_pLiveView != null)
            {
                m_pLiveView.Dispose();
                m_pLiveView = null;
            }

            #endregion
        }

        public bool PrepareCameraView()
        {
            #region 准备主区域视图显示

            bool ret = false;

            ClearCameraView();

            m_pLiveView = new CameraView();
            m_pLiveView.LinkToView(imageView);
            m_pLiveView.m_ptCenter = new Point(PCB.m_pPCB.m_uSizeX / 2, PCB.m_pPCB.m_uSizeY / 2);
            m_pLiveView.m_ptCenter.Offset(PCB.m_pPCB.m_ptOriginOffset);
            m_pLiveView.m_bmp = null;

            return ret;

            #endregion
        }

        public bool CopyCameraView(CameraView pView)
        {
            #region 从view复制主区域视图显示

            bool ret = false;

            ClearCameraView();

            m_pLiveView = new CameraView();
            m_pLiveView.LinkToView(imageView);
            m_pLiveView.m_ptCenter = new Point(PCB.m_pPCB.m_uFovSizeX / 2, PCB.m_pPCB.m_uFovSizeY / 2);
            m_pLiveView.m_ptCenter.Offset(PCB.m_pPCB.m_ptOriginOffset);
            m_pLiveView.m_bmp = (Bitmap)pView.m_bmp.Clone();

            return ret;

            #endregion
        }

        public void Update(Rectangle rect, bool bFit = false)
        {
            #region 更新视图区

            PrepareLiveView(rect, bFit);
            CreateBoardImage();

            #endregion
        }

        public void CreateBoardImage()
        {
            #region 创建视图区图像

            if (1 == System.Threading.Interlocked.Read(ref LockedSign.l2ShowIsCreatingImage))
                return;

            if (m_rcPCBImageArea.Width > 0 && m_rcPCBImageArea.Height > 0)
            {
                System.Threading.Interlocked.Exchange(ref LockedSign.l2ShowIsCreatingImage, 1);

                Point pt = new Point(0, 0);
                Color rgb;
                PixelFormat format = PixelFormat.Format8bppIndexed;
                m_bmpLive = new Bitmap(m_rcPCBImageArea.Width, m_rcPCBImageArea.Height, format);
                clsPointBitmap _lockbmp = new clsPointBitmap(m_bmpLive);
                _lockbmp.lockBits();
                for (int j = 0; j < _lockbmp.Height; j++)
                {
                    for (int i = 0; i < _lockbmp.Width; i++)
                    {
                        pt.X = i;
                        pt.Y = j;
                        pt.Offset(m_rcPCBImageArea.X, m_rcPCBImageArea.Y);

                        pt = VpToLp(pt);

                        try
                        {
                            if (GetPixelLiveImage(pt, out rgb))
                            {
                                _lockbmp.setPixel(i, j, rgb);
                            }
                            else
                            {
                                _lockbmp.setPixel(i, j, Color.FromArgb(100, 100, 100));
                            }
                        }
                        catch (Exception exc)
                        {
                            //Wells.WellsFramework.WellsMetroMessageBox.Show(null, exc.Message);
                        }
                    }
                }
                _lockbmp.unlockBits();

                UnlockLiveImage();

                if (!m_bIsColor)
                    m_bmpLive.Palette = PCB.m_pPCB.palette;

                System.Threading.Interlocked.Exchange(ref LockedSign.l2ShowIsCreatingImage, 0);
            }

            #endregion
        }

        public bool GetPixelLiveImage(Point pt, out Color color)
        {
            #region 根据物理坐标获取区域像素值

            bool ret = false;
            color = Color.FromArgb(100, 100, 100);

            if (m_pLiveView != null)
            {
                return m_pLiveView.GetPixelViewImage(pt, out color);
            }

            return ret;

            #endregion
        }

        public void UnlockLiveImage()
        {
            #region 清除各相机视图的图像数据锁定

            if (m_pLiveView != null)
            {
                m_pLiveView.UnLockBitmap();
            }

            #endregion
        }

        public void DrawLive(Graphics g)
        {
            #region 主区域绘图

            g.FillRectangle((Brush)Brushes.Black, m_rcScreenArea);

            if (m_bmpLive != null)
            {
                if (0 == System.Threading.Interlocked.Read(ref LockedSign.l2ShowIsCreatingImage))
                    g.DrawImage(m_bmpLive, m_rcPCBImageArea);
                g.DrawRectangle(new Pen(Color.Aqua, 3), m_rcPCBImageArea);
            }
            else
                g.FillRectangle((Brush)Brushes.DimGray, m_rcPCBImageArea);

            g.DrawRectangle(new Pen(Color.RoyalBlue, 5), m_rcScreenArea);
            g.DrawLine(new Pen(Color.Lime, 1), m_rcScreenArea.Width / 2, 0, m_rcScreenArea.Width / 2, m_rcScreenArea.Height);
            g.DrawLine(new Pen(Color.Lime, 1), 0, m_rcScreenArea.Height / 2, m_rcScreenArea.Width, m_rcScreenArea.Height / 2);

            #endregion
        }
    }
}
