using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using Wells.Tools;

namespace Wells.Controls.ImageView
{
    public class AreaView
    {
        public int Board_Scale = 5;//保存图片缩放比例
        public decimal m_iScale;
        public int m_iScaleIndex = 0;
        public decimal m_iFitScale;
        public decimal[] m_iScaleList;
        public Point m_ptTopLeft;//不管什么坐标系，屏幕绝对左上角，与矩形结构的topleft不一样
        public Point m_ptBottomRight;//不管什么坐标系，屏蔽绝对右下角，与矩形结构的bottomright不一样
        public Rectangle m_rcScreenArea;//控件屏幕区域，物理坐标系
        public Rectangle m_rcPCBImageArea;//图片显示区域，视图坐标系
        public Point m_ptCenter;
        public Bitmap m_bmpArea;
        public bool m_bIsColor;
        private ImageView imageView = null;
        public List<CameraView> m_CameraViewList = null;

        public AreaView()
        {
            #region 初始化，默认值

            m_iScale = 1.0M;
            m_iScaleIndex = ConstData.Scale_Num / 2;
            m_iScaleList = new decimal[ConstData.Scale_Num];
            m_ptTopLeft = new Point(0, 0);
            m_ptBottomRight = new Point(1920, 1200);
            m_ptCenter = new Point(960, 600);
            m_rcScreenArea = new Rectangle(0, 0, 1920, 1200);
            m_rcPCBImageArea = new Rectangle(0, 0, 1920, 1200);
            imageView = null;
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

            if(PCB.m_pPCB.m_iCoordinateType == ConstData.LeftDown)
            {
                X = -m_ptTopLeft.X + pt.X;
                Y = m_ptTopLeft.Y - pt.Y;
            }
            else if(PCB.m_pPCB.m_iCoordinateType == ConstData.LeftUp)
            {
                X = -m_ptTopLeft.X + pt.X;
                Y = -m_ptTopLeft.Y + pt.Y;
            }
            else if(PCB.m_pPCB.m_iCoordinateType == ConstData.RightDown)
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

            m_iFitScale = (int)Math.Max((decimal)PCB.m_pPCB.m_uSizeX * 1000 / PCB.m_pPCB.m_uResolutionX / m_rcScreenArea.Width, (decimal)PCB.m_pPCB.m_uSizeY * 1000 / PCB.m_pPCB.m_uResolutionY / m_rcScreenArea.Height) + 2;

            for (int igg = 0; igg < ConstData.Scale_Num; igg++)
                m_iScaleList[igg] = m_iFitScale * (decimal)Math.Pow(2, igg - ConstData.Scale_Num / 2);

            #endregion
        }

        public void ClearCameraView(bool bDispose=true)
        {
            #region 清空areaview的cameraview

            if (m_CameraViewList != null)
            {
                foreach (CameraView pView in m_CameraViewList)
                {
                    pView.Dispose();
                }
                if (bDispose)
                {
                    m_CameraViewList.Clear();
                    m_CameraViewList = null;
                }
            }

            #endregion
        }

        public void PrepareCameraView()
        {
            #region 分配视图

            if (PCB.m_pPCB.IsNeedUpdateViewInfo())
            {
                ClearCameraView();

                m_CameraViewList = new List<CameraView>();

                for (int y = 0; y < PCB.m_pPCB.m_yFovNum; y++)
                {
                    Point pt = new Point();
                    pt.Y = PCB.m_pPCB.m_yStep / 2 + y * PCB.m_pPCB.m_yStep + PCB.m_pPCB.m_ptOriginOffset.Y;
                    if (y % 2 == 0)
                    {
                        for (int x = 0; x < PCB.m_pPCB.m_xFovNum; x++)
                        {
                            pt.X = PCB.m_pPCB.m_xStep / 2 + x * PCB.m_pPCB.m_xStep + PCB.m_pPCB.m_ptOriginOffset.X;
                            CameraView pView = new CameraView();
                            pView.LinkToView(imageView);
                            pView.m_iIndex = m_CameraViewList.Count;
                            pView.m_ptCenter = pt;
                            m_CameraViewList.Add(pView);
                        }
                    }
                    else
                    {
                        for (int x = PCB.m_pPCB.m_xFovNum - 1; x >= 0; x--)
                        {
                            pt.X = PCB.m_pPCB.m_xStep / 2 + x * PCB.m_pPCB.m_xStep + PCB.m_pPCB.m_ptOriginOffset.X;
                            CameraView pView = new CameraView();
                            pView.LinkToView(imageView);
                            pView.m_iIndex = m_CameraViewList.Count;
                            pView.m_ptCenter = pt;
                            m_CameraViewList.Add(pView);
                        }
                    }
                }
            }
            else
            {
                if (m_CameraViewList != null && m_CameraViewList.Count == PCB.m_pPCB.m_xFovNum * PCB.m_pPCB.m_yFovNum)
                {
                    for (int y = 0; y < PCB.m_pPCB.m_yFovNum; y++)
                    {
                        Point pt = new Point();
                        pt.Y = PCB.m_pPCB.m_yStep / 2 + y * PCB.m_pPCB.m_yStep + PCB.m_pPCB.m_ptOriginOffset.Y;
                        if (y % 2 == 0)
                        {
                            for (int x = 0; x < PCB.m_pPCB.m_xFovNum; x++)
                            {
                                pt.X = PCB.m_pPCB.m_xStep / 2 + x * PCB.m_pPCB.m_xStep + PCB.m_pPCB.m_ptOriginOffset.X;
                                int index = y * PCB.m_pPCB.m_xFovNum + x;
                                CameraView pView = m_CameraViewList[index];
                                pView.m_ptCenter = pt;
                            }
                        }
                        else
                        {
                            for (int x = PCB.m_pPCB.m_xFovNum - 1; x >= 0; x--)
                            {
                                pt.X = PCB.m_pPCB.m_xStep / 2 + x * PCB.m_pPCB.m_xStep + PCB.m_pPCB.m_ptOriginOffset.X;
                                int index = y * PCB.m_pPCB.m_xFovNum + (PCB.m_pPCB.m_xFovNum - 1 - x);
                                CameraView pView = m_CameraViewList[index];
                                pView.m_ptCenter = pt;
                            }
                        }
                    }
                }
            }

            #endregion
        }
        
        public bool PrepareAreaView(Rectangle rect, Point ptCenter, bool bFit = false)
        {
            #region 准备主区域视图显示

            bool ret = false;

            if (rect.Width > 0 && rect.Height > 0)
            {
                m_rcScreenArea = rect;

                CalFitScale();

                if (bFit)
                {
                    m_ptCenter = new Point(PCB.m_pPCB.m_uSizeX / 2, PCB.m_pPCB.m_uSizeY / 2);
                    m_ptCenter.Offset(PCB.m_pPCB.m_ptOriginOffset);
                    m_iScaleIndex = ConstData.Scale_Num / 2;
                }
                else
                {
                    m_ptCenter = ptCenter;
                }

                m_iScale = m_iScaleList[m_iScaleIndex];
                
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

                Rectangle temp = new Rectangle(0, 0, PCB.m_pPCB.m_uSizeX, PCB.m_pPCB.m_uSizeY);
                temp.Offset(PCB.m_pPCB.m_ptOriginOffset);

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

        public void Update(Rectangle rect,Point ptCenter)
        {
            #region 更新视图区
            
            if(imageView.m_iEditType == ConstData.Mode_Normal)
            {
                PrepareAreaView(rect, Point.Empty, true);
                CreateBoardImage();
            }
            else if (imageView.m_iEditType == ConstData.Mode_Edit)
            {
                PrepareAreaView(rect, ptCenter);
                CreateBoardImage();
            }

            #endregion
        }

        public bool CreateBoardImage()//速度太快，连续进入该函数会造成图片数据锁定冲突
        {
            #region 创建视图区图像

            if (1 == System.Threading.Interlocked.Read(ref LockedSign.l2ShowIsCreatingImage))
                return false;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            if (m_rcPCBImageArea.Width>0&&m_rcPCBImageArea.Height>0)
            {
                System.Threading.Interlocked.Exchange(ref LockedSign.l2ShowIsCreatingImage, 1);

                Point pt = new Point(0, 0);
                Color rgb;
                PixelFormat format = PixelFormat.Format8bppIndexed;
                m_bmpArea = new Bitmap(m_rcPCBImageArea.Width, m_rcPCBImageArea.Height, format);
                clsPointBitmap _lockbmp = new clsPointBitmap(m_bmpArea);
                _lockbmp.lockBits();
                for(int j=0;j<_lockbmp.Height;j++)
                {
                    for(int i=0;i<_lockbmp.Width;i++)
                    {
                        pt.X = i;
                        pt.Y = j;
                        pt.Offset(m_rcPCBImageArea.X, m_rcPCBImageArea.Y);

                        pt = VpToLp(pt);

                        try
                        {
                            if (GetPixelAreaImage(pt, out rgb))
                            {
                                _lockbmp.setPixel(i, j, rgb);
                            }
                            else
                            {
                                _lockbmp.setPixel(i, j, Color.FromArgb(100, 100, 100));
                            }
                        }
                        catch(Exception exc)
                        {
                            //Wells.WellsFramework.WellsMetroMessageBox.Show(null, exc.Message);
                        }
                    }
                }
                _lockbmp.unlockBits();

                UnlockAreaImage();

                if (!m_bIsColor)
                    m_bmpArea.Palette = PCB.m_pPCB.palette;

                System.Threading.Interlocked.Exchange(ref LockedSign.l2ShowIsCreatingImage, 0);
            }
            sw.Stop();
            //Wells.FrmType.frm_Log.Log("拼接耗时：" + sw.ElapsedMilliseconds.ToString() + " ms", 0, 0);

            return true;

            #endregion
        }

        public Bitmap CreateBoardImageLarge()
        {
            #region 创建整版图
            
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            int nWidth = PCB.m_pPCB.m_uSizeX * 1000 / PCB.m_pPCB.m_uResolutionX / Board_Scale;
            int nHeight = PCB.m_pPCB.m_uSizeY * 1000 / PCB.m_pPCB.m_uResolutionY / Board_Scale;
            Point pt = new Point(0, 0);
            Color rgb;
            PixelFormat format = PixelFormat.Format8bppIndexed;
            Bitmap m_bmpTemp = new Bitmap(nWidth, nHeight, format);
            clsPointBitmap _lockbmp = new clsPointBitmap(m_bmpTemp);
            _lockbmp.lockBits();
            for (int j = 0; j < _lockbmp.Height; j++)
            {
                for (int i = 0; i < _lockbmp.Width; i++)
                {
                    if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftDown)
                    {
                        pt.X = i * Board_Scale * PCB.m_pPCB.m_uResolutionX / 1000;
                        pt.Y = -j * Board_Scale * PCB.m_pPCB.m_uResolutionY / 1000 + PCB.m_pPCB.m_uSizeY;
                    }
                    else if (PCB.m_pPCB.m_iCoordinateType == ConstData.LeftUp)
                    {
                        pt.X = i * Board_Scale * PCB.m_pPCB.m_uResolutionX / 1000;
                        pt.Y = j * Board_Scale * PCB.m_pPCB.m_uResolutionY / 1000;
                    }
                    else if (PCB.m_pPCB.m_iCoordinateType == ConstData.RightDown)
                    {
                        pt.X = -i * Board_Scale * PCB.m_pPCB.m_uResolutionX / 1000 + PCB.m_pPCB.m_uSizeX;
                        pt.Y = -j * Board_Scale * PCB.m_pPCB.m_uResolutionY / 1000 + PCB.m_pPCB.m_uSizeY;
                    }
                    else
                    {
                        pt.X = -i * Board_Scale * PCB.m_pPCB.m_uResolutionX / 1000 + PCB.m_pPCB.m_uSizeX;
                        pt.Y = j * Board_Scale * PCB.m_pPCB.m_uResolutionY / 1000;
                    }
                    pt.Offset(PCB.m_pPCB.m_ptOriginOffset);

                    try
                    {
                        if (GetPixelAreaImage(pt, out rgb))
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

            UnlockAreaImage();

            if (!m_bIsColor)
                m_bmpTemp.Palette = PCB.m_pPCB.palette;
            
            sw.Stop();
            //Wells.FrmType.frm_Log.Log("拼接耗时：" + sw.ElapsedMilliseconds.ToString() + " ms", 0, 0);

            return m_bmpTemp;

            #endregion
        }

        public int m_iCurpos = -1;

        public bool GetPixelAreaImage(Point pt,out Color color)
        {
            #region 根据物理坐标获取区域像素值

            bool ret = false;
            color = Color.FromArgb(100, 100, 100);

            if (m_CameraViewList != null)
            {
                if (m_CameraViewList.Count > 0)
                {
                    if (m_iCurpos >= 0 && m_iCurpos < m_CameraViewList.Count)
                    {
                        CameraView pView = m_CameraViewList[m_iCurpos];
                        if (pView != null)
                        {
                            int xDistance = Math.Abs(pView.m_ptCenter.X - pt.X);
                            int yDistance = Math.Abs(pView.m_ptCenter.Y - pt.Y);
                            if (((2 * xDistance) <= PCB.m_pPCB.m_xStep) && ((2 * yDistance) <= PCB.m_pPCB.m_yStep))
                            {
                                return pView.GetPixelViewImage(pt, out color);
                            }
                        }
                    }
                }

                for (int igg = 0; igg < m_CameraViewList.Count; igg++)
                {
                    m_iCurpos = igg;
                    CameraView pView = m_CameraViewList[igg];
                    if (pView != null)
                    {
                        int xDistance = Math.Abs(pView.m_ptCenter.X - pt.X);
                        int yDistance = Math.Abs(pView.m_ptCenter.Y - pt.Y);
                        if (((2 * xDistance) <= PCB.m_pPCB.m_xStep) && ((2 * yDistance) <= PCB.m_pPCB.m_yStep))
                        {
                            return pView.GetPixelViewImage(pt, out color);
                        }
                    }
                }

                m_iCurpos = -1;
            }

            return ret;

            #endregion
        }

        public void UnlockAreaImage()
        {
            #region 清除各相机视图的图像数据锁定

            if (m_CameraViewList != null)
            {
                if (m_CameraViewList.Count > 0)
                {
                    for (int igg = 0; igg < m_CameraViewList.Count; igg++)
                    {
                        CameraView pView = m_CameraViewList[igg];
                        pView.UnLockBitmap();
                    }
                }
            }

            #endregion
        }

        public void DrawArea(Graphics g)
        {
            #region 主区域绘图
            
            if(imageView.m_iEditType == ConstData.Mode_Normal)
            {
                g.FillRectangle((Brush)Brushes.DarkGray, m_rcScreenArea);
            }
            else if (imageView.m_iEditType == ConstData.Mode_Edit)
            {
                g.FillRectangle((Brush)Brushes.Black, m_rcScreenArea);

                if (m_bmpArea != null)
                {
                    if (0 == System.Threading.Interlocked.Read(ref LockedSign.l2ShowIsCreatingImage))
                        g.DrawImage(m_bmpArea, m_rcPCBImageArea);
                }
                else
                    g.FillRectangle((Brush)Brushes.DimGray, m_rcPCBImageArea);

                if (imageView.m_iShowType == ConstData.Show_Grid)
                {
                    if (m_CameraViewList != null)
                    {
                        foreach (CameraView pView in m_CameraViewList)
                        {
                            Rectangle rect = pView.GetAbsoluteRect();
                            rect = LpToVp(rect);
                            pView.Draw(g, rect);
                        }
                    }
                }
            }
            

            g.DrawRectangle(new Pen(Color.RoyalBlue, 5), m_rcScreenArea);
            g.DrawLine(new Pen(Color.Lime, 1), m_rcScreenArea.Width / 2, 0, m_rcScreenArea.Width / 2, m_rcScreenArea.Height);
            g.DrawLine(new Pen(Color.Lime, 1), 0, m_rcScreenArea.Height / 2, m_rcScreenArea.Width, m_rcScreenArea.Height / 2);

            #endregion
        }
    }
}
