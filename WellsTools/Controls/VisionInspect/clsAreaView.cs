using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Wells.Controls.VisionInspect
{
    public class clsAreaView
    {
        #region ***** 基本参数 *****

        /// <summary>
        /// 保存图片缩放比例
        /// </summary>
        public int Board_Scale = 5;
        /// <summary>
        /// 视图缩放比例
        /// </summary>
        public decimal m_iScale;

        /// <summary>
        /// 不管什么方向坐标系，屏幕绝对左上角，与矩形结构的topleft不一样
        /// </summary>
        public Point m_lptTopLeft;

        /// <summary>
        /// 不管什么方向坐标系，屏蔽绝对右下角，与矩形结构的bottomright不一样
        /// </summary>
        public Point m_lptBottomRight;

        /// <summary>
        /// 控件屏幕区域，实际为控件的clientrectangle
        /// </summary>
        public Rectangle m_vrcScreenArea;

        /// <summary>
        /// 图片显示区域，视图坐标系
        /// </summary>
        public Rectangle m_vrcPCBImageArea;

        /// <summary>
        /// 视图中心坐标，物理坐标
        /// </summary>
        public Point m_lptCenter;

        /// <summary>
        /// 图片数据
        /// </summary>
        public clsImage m_image;

        /// <summary>
        /// 绑定的控件
        /// </summary>
        internal ImageDoc imageDoc = null;

        #endregion

        public clsAreaView()
        {
            #region 初始化，默认值

            m_iScale = 1.0M;
            m_lptTopLeft = new Point(0, 0);
            m_lptBottomRight = new Point(1920, 1200);
            m_lptCenter = new Point(960, 600);
            m_vrcScreenArea = new Rectangle(0, 0, 300, 200);
            m_vrcPCBImageArea = new Rectangle(0, 0, 300, 200);
            m_image = new clsImage();
            m_image.Color = clsPCB.m_pPCB.m_bColor;
            imageDoc = null;

            #endregion
        }

        public void linkToView(ImageDoc imgdoc)
        {
            #region 绑定父view

            imageDoc = imgdoc;

            #endregion
        }

        #region ***** 坐标系转换功能，areaview视图坐标以控件坐标原点为原点，即左上角 *****

        public Point LpToVp(Point pt)
        {
            #region 物理坐标到视图坐标

            int X = 0, Y = 0;

            if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftDown)
            {
                X = -m_lptTopLeft.X + pt.X;
                Y = m_lptTopLeft.Y - pt.Y;
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftUp)
            {
                X = -m_lptTopLeft.X + pt.X;
                Y = -m_lptTopLeft.Y + pt.Y;
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.RightDown)
            {
                X = m_lptTopLeft.X - pt.X;
                Y = m_lptTopLeft.Y - pt.Y;
            }
            else
            {
                X = m_lptTopLeft.X - pt.X;
                Y = -m_lptTopLeft.Y + pt.Y;
            }

            X = (int)Math.Round((decimal)X * 1000 / clsPCB.m_pPCB.m_uResolutionX / m_iScale);
            Y = (int)Math.Round((decimal)Y * 1000 / clsPCB.m_pPCB.m_uResolutionY / m_iScale);

            return new Point(X, Y);

            #endregion
        }

        public Point VpToLp(Point pt)
        {
            #region 视图坐标到物理坐标

            int X = 0, Y = 0;

            X = (int)Math.Round((decimal)pt.X / 1000 * clsPCB.m_pPCB.m_uResolutionX * m_iScale);
            Y = (int)Math.Round((decimal)pt.Y / 1000 * clsPCB.m_pPCB.m_uResolutionY * m_iScale);

            if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftDown)
            {
                X = m_lptTopLeft.X + X;
                Y = m_lptTopLeft.Y - Y;
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftUp)
            {
                X = m_lptTopLeft.X + X;
                Y = m_lptTopLeft.Y + Y;
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.RightDown)
            {
                X = m_lptTopLeft.X - X;
                Y = m_lptTopLeft.Y - Y;
            }
            else
            {
                X = m_lptTopLeft.X - X;
                Y = m_lptTopLeft.Y + Y;
            }

            return new Point(X, Y);

            #endregion
        }

        public Rectangle LpToVp(Rectangle rect)
        {
            #region 物理坐标到视图坐标

            int Left = 0, Right = 0, Top = 0, Bottom = 0;

            if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftDown)
            {
                Left = -m_lptTopLeft.X + rect.Left;
                Right = -m_lptTopLeft.X + rect.Right;
                Top = m_lptTopLeft.Y - rect.Top;
                Bottom = m_lptTopLeft.Y - rect.Bottom;
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftUp)
            {
                Left = -m_lptTopLeft.X + rect.Left;
                Right = -m_lptTopLeft.X + rect.Right;
                Top = -m_lptTopLeft.Y + rect.Top;
                Bottom = -m_lptTopLeft.Y + rect.Bottom;
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.RightDown)
            {
                Left = m_lptTopLeft.X - rect.Left;
                Right = m_lptTopLeft.X - rect.Right;
                Top = m_lptTopLeft.Y - rect.Top;
                Bottom = m_lptTopLeft.Y - rect.Bottom;
            }
            else
            {
                Left = m_lptTopLeft.X - rect.Left;
                Right = m_lptTopLeft.X - rect.Right;
                Top = -m_lptTopLeft.Y + rect.Top;
                Bottom = -m_lptTopLeft.Y + rect.Bottom;
            }

            Left = (int)Math.Round((decimal)Left * 1000 / clsPCB.m_pPCB.m_uResolutionX / m_iScale);
            Right = (int)Math.Round((decimal)Right * 1000 / clsPCB.m_pPCB.m_uResolutionX / m_iScale);
            Top = (int)Math.Round((decimal)Top * 1000 / clsPCB.m_pPCB.m_uResolutionY / m_iScale);
            Bottom = (int)Math.Round((decimal)Bottom * 1000 / clsPCB.m_pPCB.m_uResolutionY / m_iScale);

            if (Left > Right) { Left = Left + Right; Right = Left - Right; Left = Left - Right; }
            if (Top > Bottom) { Top = Top + Bottom; Bottom = Top - Bottom; Top = Top - Bottom; }

            return new Rectangle(Left, Top, Math.Abs(Right - Left), Math.Abs(Bottom - Top));

            #endregion
        }

        public Rectangle VpToLp(Rectangle rect)
        {
            #region 视图坐标到物理坐标

            int Left = 0, Right = 0, Top = 0, Bottom = 0;

            Left = (int)Math.Round((decimal)rect.Left / 1000 * clsPCB.m_pPCB.m_uResolutionX * m_iScale);
            Right = (int)Math.Round((decimal)rect.Right / 1000 * clsPCB.m_pPCB.m_uResolutionX * m_iScale);
            Top = (int)Math.Round((decimal)rect.Top / 1000 * clsPCB.m_pPCB.m_uResolutionY * m_iScale);
            Bottom = (int)Math.Round((decimal)rect.Bottom / 1000 * clsPCB.m_pPCB.m_uResolutionY * m_iScale);

            if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftDown)
            {
                Left = m_lptTopLeft.X + Left;
                Right = m_lptTopLeft.X + Right;
                Top = m_lptTopLeft.Y - Top;
                Bottom = m_lptTopLeft.Y - Bottom;
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftUp)
            {
                Left = m_lptTopLeft.X + Left;
                Right = m_lptTopLeft.X + Right;
                Top = m_lptTopLeft.Y + Top;
                Bottom = m_lptTopLeft.Y + Bottom;
            }
            else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.RightDown)
            {
                Left = m_lptTopLeft.X - Left;
                Right = m_lptTopLeft.X - Right;
                Top = m_lptTopLeft.Y - Top;
                Bottom = m_lptTopLeft.Y - Bottom;
            }
            else
            {
                Left = m_lptTopLeft.X - Left;
                Right = m_lptTopLeft.X - Right;
                Top = m_lptTopLeft.Y + Top;
                Bottom = m_lptTopLeft.Y + Bottom;
            }

            if (Left > Right) { Left = Left + Right; Right = Left - Right; Left = Left - Right; }
            if (Top > Bottom) { Top = Top + Bottom; Bottom = Top - Bottom; Top = Top - Bottom; }

            return new Rectangle(Left, Top, Right - Left, Bottom - Top);

            #endregion
        }

        #endregion

        public bool prepareAreaView(Rectangle rect, Point ptCenter, bool bFit = false)
        {
            #region 准备主区域视图显示

            bool ret = false;

            if (rect.Width > 0 && rect.Height > 0)
            {
                if (bFit)
                {
                    ptCenter = new Point(clsPCB.m_pPCB.m_uSizeX / 2, clsPCB.m_pPCB.m_uSizeY / 2);
                    ptCenter.Offset(clsPCB.m_pPCB.m_ptOriginOffset);

                    m_iScale = Math.Max((decimal)clsPCB.m_pPCB.m_uSizeX * 1000 / clsPCB.m_pPCB.m_uResolutionX / m_vrcScreenArea.Width, (decimal)clsPCB.m_pPCB.m_uSizeY * 1000 / clsPCB.m_pPCB.m_uResolutionY / m_vrcScreenArea.Height) / 0.9M;
                }

                m_vrcScreenArea = rect;

                m_lptCenter = ptCenter;

                int X = (int)Math.Round((decimal)m_vrcScreenArea.Width / 2 / 1000 * clsPCB.m_pPCB.m_uResolutionX * m_iScale);
                int Y = (int)Math.Round((decimal)m_vrcScreenArea.Height / 2 / 1000 * clsPCB.m_pPCB.m_uResolutionY * m_iScale);

                if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftDown)
                {
                    m_lptTopLeft.X = m_lptCenter.X - X;
                    m_lptTopLeft.Y = m_lptCenter.Y + Y;
                    m_lptBottomRight.X = m_lptCenter.X + X;
                    m_lptBottomRight.Y = m_lptCenter.Y - Y;
                }
                else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.LeftUp)
                {
                    m_lptTopLeft.X = m_lptCenter.X - X;
                    m_lptTopLeft.Y = m_lptCenter.Y - Y;
                    m_lptBottomRight.X = m_lptCenter.X + X;
                    m_lptBottomRight.Y = m_lptCenter.Y + Y;
                }
                else if (clsPCB.m_pPCB.m_iCoordinateType == tagCoordinateType.RightDown)
                {
                    m_lptTopLeft.X = m_lptCenter.X + X;
                    m_lptTopLeft.Y = m_lptCenter.Y + Y;
                    m_lptBottomRight.X = m_lptCenter.X - X;
                    m_lptBottomRight.Y = m_lptCenter.Y - Y;
                }
                else
                {
                    m_lptTopLeft.X = m_lptCenter.X + X;
                    m_lptTopLeft.Y = m_lptCenter.Y - Y;
                    m_lptBottomRight.X = m_lptCenter.X - X;
                    m_lptBottomRight.Y = m_lptCenter.Y + Y;
                }

                Rectangle temp = new Rectangle(0, 0, clsPCB.m_pPCB.m_uSizeX, clsPCB.m_pPCB.m_uSizeY);
                temp.Offset(clsPCB.m_pPCB.m_ptOriginOffset);

                m_vrcPCBImageArea = LpToVp(temp);

                if (m_vrcPCBImageArea.X < 0) { m_vrcPCBImageArea.Width += m_vrcPCBImageArea.X; m_vrcPCBImageArea.X = 0; }
                if (m_vrcPCBImageArea.Y < 0) { m_vrcPCBImageArea.Height += m_vrcPCBImageArea.Y; m_vrcPCBImageArea.Y = 0; }
                if (m_vrcPCBImageArea.Width + m_vrcPCBImageArea.X > m_vrcScreenArea.Width) m_vrcPCBImageArea.Width = m_vrcScreenArea.Width - m_vrcPCBImageArea.X;
                if (m_vrcPCBImageArea.Height + m_vrcPCBImageArea.Y > m_vrcScreenArea.Height) m_vrcPCBImageArea.Height = m_vrcScreenArea.Height - m_vrcPCBImageArea.Y;

                ret = createBoardImage();
            }

            return ret;

            #endregion
        }

        public void update(Rectangle rect, Point ptCenter)
        {
            #region 更新视图区

            if (imageDoc.m_iEditType == tagModeType.Mode_Normal)
            {
                prepareAreaView(rect, Point.Empty, true);
            }
            else if (imageDoc.m_iEditType == tagModeType.Mode_Edit)
            {
                prepareAreaView(rect, ptCenter);
            }

            #endregion
        }

        public bool createBoardImage()//速度太快，连续进入该函数会造成图片数据锁定冲突
        {
            #region 创建视图区图像

            m_image.clear();

            if (m_vrcPCBImageArea.Width > 0 && m_vrcPCBImageArea.Height > 0)
            {
                m_image.Width = m_vrcPCBImageArea.Width;
                m_image.Height = m_vrcPCBImageArea.Height;
                m_image.Color = clsPCB.m_pPCB.m_bColor;
                m_image.ImgBuffer = new byte[m_image.ImgBufferSize];

                Point pt = new Point(0, 0);
                byte R, G, B, tmp = 100;

                for (int j = 0; j < m_image.Height; j++)
                {
                    for (int i = 0; i < m_image.Width; i++)
                    {
                        pt.X = i;
                        pt.Y = j;
                        pt.Offset(m_vrcPCBImageArea.X, m_vrcPCBImageArea.Y);

                        pt = VpToLp(pt);

                        if (!clsPCB.m_pPCB.getPixelPCBImage(pt, out R, out G, out B))
                            R = G = B = tmp;

                        if (m_image.Color)
                        {
                            m_image.ImgBuffer[(j * m_image.Width + i )* 3] = B;
                            m_image.ImgBuffer[(j * m_image.Width + i )* 3 + 1] = G;
                            m_image.ImgBuffer[(j * m_image.Width + i )* 3 + 2] = R;
                        }
                        else
                        {
                            m_image.ImgBuffer[j * m_image.Width + i] = R;
                        }
                    }
                }
            }

            //Wells.FrmType.frm_Log.Log("拼接耗时：" + sw.ElapsedMilliseconds.ToString() + " ms", 0, 0);

            return true;

            #endregion
        }

        public void zoomIn()
        {
            #region 放大

            if (imageDoc.m_iEditType == tagModeType.Mode_Edit)
            {
                m_iScale *= 0.9M;
                prepareAreaView(m_vrcScreenArea, m_lptCenter);
            }

            #endregion
        }

        public void zoomOut()
        {
            #region 缩小

            if (imageDoc.m_iEditType == tagModeType.Mode_Edit)
            {
                m_iScale /= 0.9M;
                prepareAreaView(m_vrcScreenArea, m_lptCenter);
            }

            #endregion
        }

        public void zoomFit()
        {
            #region 最适屏幕

            if (imageDoc.m_iEditType == tagModeType.Mode_Edit)
            {
                prepareAreaView(m_vrcScreenArea, m_lptCenter, true);
            }

            #endregion
        }

        public void draw(Graphics g)
        {
            #region 主区域绘图

            if (imageDoc.m_iEditType == tagModeType.Mode_Normal)
            {
                g.FillRectangle(Brushes.DimGray, m_vrcScreenArea);

                if (m_image.isInitialized())
                {
                    g.drawImage(m_image, m_vrcPCBImageArea);
                }
                else
                {

                }
            }
            else if (imageDoc.m_iEditType == tagModeType.Mode_Edit)
            {
                g.FillRectangle(Brushes.Black, m_vrcScreenArea);

                if (m_image.isInitialized())
                {
                    g.drawImage(m_image, m_vrcPCBImageArea);
                }
                else
                {
                    g.FillRectangle(Brushes.DimGray, m_vrcPCBImageArea);
                }

                if (imageDoc.m_iShowType == tagShowType.Show_Grid)
                {
                    foreach (clsCameraView pView in clsPCB.m_pPCB.m_CameraViewList)
                    {
                        Rectangle rect = pView.getAbsoluteRect();
                        rect = LpToVp(rect);

                        pView.drawScreenRect(g, rect, Color.LightCoral, System.Drawing.Drawing2D.DashStyle.Dash);
                    }
                }
            }

            foreach (clsPart pPart in clsPCB.m_pPCB.m_PartList)
            {
                Rectangle rect = pPart.getAbsoluteRect();
                rect = LpToVp(rect);

                pPart.drawScreenRect(g, rect);
            }

            //g.DrawRectangle(new Pen(Color.RoyalBlue, 5), m_vrcScreenArea);
            g.DrawLine(new Pen(Color.DeepSkyBlue, 1), m_vrcScreenArea.Width / 2, 0, m_vrcScreenArea.Width / 2, m_vrcScreenArea.Height);
            g.DrawLine(new Pen(Color.DeepSkyBlue, 1), 0, m_vrcScreenArea.Height / 2, m_vrcScreenArea.Width, m_vrcScreenArea.Height / 2);

            #endregion
        }
    }
}
