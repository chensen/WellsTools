using hvppleDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Wells.Controls.VisionInspect;

namespace Wells.Controls.ImageDocEx
{
    public class qtPCB
    {
        #region ***** 参数变量 *****
        
        /// <summary>
        /// 基板长度，单位um
        /// </summary>
        public int m_uSizeX;//um

        /// <summary>
        /// 基板宽度，单位um
        /// </summary>
        public int m_uSizeY;//um

        /// <summary>
        /// X方向分辨率，单位um/1000
        /// </summary>
        public int m_uResolutionX;//1/1000um

        /// <summary>
        /// Y方向分辨率，单位um/1000
        /// </summary>
        public int m_uResolutionY;//1/1000um

        /// <summary>
        /// 相机像素长度，单位pixel
        /// </summary>
        public int m_pFovPixelWidth;

        /// <summary>
        /// 相机像素宽度，单位pixel
        /// </summary>
        public int m_pFovPixelHeight;

        /// <summary>
        /// 相机视野长度，单位um
        /// </summary>
        public int m_uFovSizeX;

        /// <summary>
        /// 相机视野宽度，单位um
        /// </summary>
        public int m_uFovSizeY;

        /// <summary>
        /// 指示原图是否为彩色
        /// </summary>
        public bool m_bColor;

        /// <summary>
        /// 基板选择坐标系，原点位置，0，左下，1，左上，2，右下，3，右上
        /// </summary>
        public int m_iCoordinateType;

        /// <summary>
        /// CameraView视图坐标系，原点位置，0，左上，1，左下
        /// </summary>
        public int m_iImageCoordinateType;

        /// <summary>
        /// 单张图片，实际显示长度，略小于图片实际长度，单位um
        /// </summary>
        public int m_xStep;

        /// <summary>
        /// 单张图片，实际显示宽度，略小于图片实际宽度，单位um
        /// </summary>
        public int m_yStep;

        /// <summary>
        /// X方向Fov个数
        /// </summary>
        public int m_xFovNum;

        /// <summary>
        /// Y方向Fov个数
        /// </summary>
        public int m_yFovNum;

        /// <summary>
        /// X方向Fov个数备份，当基板尺寸或者分辨率更改迫使需要重新分配fov时使用
        /// </summary>
        public int m_xOldFovNum;

        /// <summary>
        /// Y方向Fov个数备份，当基板尺寸或者分辨率更改迫使需要重新分配fov时使用
        /// </summary>
        public int m_yOldFovNum;

        /// <summary>
        /// 基板设定的原点偏移，软件偏移
        /// </summary>
        public Point m_ptOriginOffset;
        
        /// <summary>
        /// 视图列表
        /// </summary>
        public List<qtCameraView> m_CameraViewList;
        
        /// <summary>
        /// 绑定的控件
        /// </summary>
        internal ImageDocEx imageDocEx = null;

        #region ***** 图片拼接参数 *****

        public HTuple offsetRow, offsetCol, row1, row2, col1, col2;
        public int row, col;
        public int Width, Height;
        public int actualFovWidth, actualFovHeight;
        HObject imageList;

        #endregion

        #endregion

        public qtPCB()
        {

        }

        public void initialize(int uSizeX, int uSizeY, int uResolutionX, int uResolutionY, int pWidth, int pHeight, Point ptOriginOffset, int type = tagCoordinateType.LeftDown, int imagetype = tagImageCoordinateType.LeftUp, bool bColor = false)
        {
            #region 初始化各参数

            m_uSizeX = uSizeX;
            m_uSizeY = uSizeY;
            m_uResolutionX = uResolutionX;
            m_uResolutionY = uResolutionY;
            m_pFovPixelWidth = pWidth;
            m_pFovPixelHeight = pHeight;
            m_uFovSizeX = (int)Math.Round((decimal)m_pFovPixelWidth / 1000 * m_uResolutionX);
            m_uFovSizeY = (int)Math.Round((decimal)m_pFovPixelHeight / 1000 * m_uResolutionY);
            m_iCoordinateType = type;
            m_iImageCoordinateType = imagetype;
            m_bColor = bColor;

            m_xFovNum = m_uSizeX / m_uFovSizeX + 1;
            m_yFovNum = m_uSizeY / m_uFovSizeY + 1;
            m_xStep = m_uSizeX / m_xFovNum;
            m_yStep = m_uSizeY / m_yFovNum;

            m_xOldFovNum = 0;
            m_yOldFovNum = 0;

            m_ptOriginOffset = ptOriginOffset;

            preparePCBView();

            prepareTileParam();

            #endregion
        }

        public void setResolution(int uResolutionX, int uResolutionY)
        {
            #region 设置分辨率

            m_uResolutionX = uResolutionX;
            m_uResolutionY = uResolutionY;
            m_uFovSizeX = (int)Math.Round((decimal)m_pFovPixelWidth / 1000 * m_uResolutionX);
            m_uFovSizeY = (int)Math.Round((decimal)m_pFovPixelHeight / 1000 * m_uResolutionY);

            m_xOldFovNum = m_xFovNum;
            m_yOldFovNum = m_yFovNum;

            m_xFovNum = m_uSizeX / m_uFovSizeX + 1;
            m_yFovNum = m_uSizeY / m_uFovSizeY + 1;
            m_xStep = m_uSizeX / m_xFovNum;
            m_yStep = m_uSizeY / m_yFovNum;

            #endregion
        }

        public void preparePCBView()
        {
            #region **** 准备PCB的视图分配 *****

            if (m_CameraViewList == null)
                m_CameraViewList = new List<qtCameraView>();

            if ((m_xFovNum != m_xOldFovNum) || (m_yFovNum != m_yOldFovNum))
            {
                #region ***** 完全重新分配 *****

                foreach(var pView in m_CameraViewList)
                {
                    pView.m_image.Dispose();
                }
                m_CameraViewList.Clear();

                m_CameraViewList = new List<qtCameraView>();

                for (int y = 0; y < m_yFovNum; y++)
                {
                    Point pt = new Point();
                    pt.Y = m_yStep / 2 + y * m_yStep + m_ptOriginOffset.Y;
                    if (y % 2 == 0)
                    {
                        for (int x = 0; x < m_xFovNum; x++)
                        {
                            pt.X = m_xStep / 2 + x * m_xStep + m_ptOriginOffset.X;
                            qtCameraView pView = new qtCameraView();
                            pView.m_iIndex = m_CameraViewList.Count;
                            pView.m_lptCenter = pt;
                            pView.m_image.Width = m_pFovPixelWidth;
                            pView.m_image.Height = m_pFovPixelHeight;
                            pView.m_image.Color = m_bColor;
                            HOperatorSet.GenImageConst(out pView.m_image.hObj, "byte", m_pFovPixelWidth, m_pFovPixelHeight);
                            m_CameraViewList.Add(pView);
                        }
                    }
                    else
                    {
                        for (int x = m_xFovNum - 1; x >= 0; x--)
                        {
                            pt.X = m_xStep / 2 + x * m_xStep + m_ptOriginOffset.X;
                            qtCameraView pView = new qtCameraView();
                            pView.m_iIndex = m_CameraViewList.Count;
                            pView.m_lptCenter = pt;
                            pView.m_image.Width = m_pFovPixelWidth;
                            pView.m_image.Height = m_pFovPixelHeight;
                            pView.m_image.Color = m_bColor;
                            HOperatorSet.GenImageConst(out pView.m_image.hObj, "byte", m_pFovPixelWidth, m_pFovPixelHeight);
                            m_CameraViewList.Add(pView);
                        }
                    }
                }

                #endregion
            }
            else
            {
                if (m_CameraViewList.Count == m_xFovNum * m_yFovNum)
                {
                    #region ***** 更新视图分配 *****

                    for (int y = 0; y < m_yFovNum; y++)
                    {
                        Point pt = new Point();
                        pt.Y = m_yStep / 2 + y * m_yStep + m_ptOriginOffset.Y;
                        if (y % 2 == 0)
                        {
                            for (int x = 0; x < m_xFovNum; x++)
                            {
                                pt.X = m_xStep / 2 + x * m_xStep + m_ptOriginOffset.X;
                                int index = y * m_xFovNum + x;
                                qtCameraView pView = m_CameraViewList[index];
                                pView.m_lptCenter = pt;
                            }
                        }
                        else
                        {
                            for (int x = m_xFovNum - 1; x >= 0; x--)
                            {
                                pt.X = m_xStep / 2 + x * m_xStep + m_ptOriginOffset.X;
                                int index = y * m_xFovNum + (m_xFovNum - 1 - x);
                                qtCameraView pView = m_CameraViewList[index];
                                pView.m_lptCenter = pt;
                            }
                        }
                    }

                    #endregion
                }
            }

            #endregion
        }

        public void prepareTileParam()
        {
            #region ***** 准备图片拼接参数 *****

            if (imageList != null) imageList.Dispose();

            HOperatorSet.GenEmptyObj(out imageList);

            //贴磁砖方法进行拼接
            offsetRow = 0;//图片在目标图片的行坐标，即y
            offsetCol = 0;//图片在目标图片上的列坐标，即x
            row1 = 0;//图片自身裁剪左上角行坐标
            row2 = 0;//图片自身裁剪左上角列坐标
            col1 = 0;//图片自身裁剪右下角行坐标
            col2 = 0;//图片自身裁剪右下角列坐标

            actualFovWidth = (int)Math.Round((decimal)m_xStep / m_uResolutionX * 1000);
            actualFovHeight = (int)Math.Round((decimal)m_yStep / m_uResolutionY* 1000);
            row = (m_pFovPixelHeight - actualFovHeight) / 2;
            col = (m_pFovPixelWidth - actualFovWidth) / 2;
            Width = actualFovWidth * m_xFovNum;
            Height = actualFovHeight * m_yFovNum;

            for (int jgg = 0; jgg < m_yFovNum; jgg++) 
            {
                for (int igg = 0; igg < m_xFovNum; igg++)
                {
                    int index = jgg * m_xFovNum + igg;
                    HObject obj = m_CameraViewList[index].m_image.hObj;

                    if (obj == null)
                        HOperatorSet.GenImageConst(out obj, "byte", m_CameraViewList[index].m_image.Width, m_CameraViewList[index].m_image.Height);

                    HOperatorSet.ConcatObj(imageList, obj, out imageList);

                    if (m_iCoordinateType == tagCoordinateType.LeftDown)
                    {
                        offsetRow[index] = actualFovHeight * (m_yFovNum - jgg - 1);
                        offsetCol[index] = actualFovWidth * (jgg % 2 == 0 ? igg : m_xFovNum - igg - 1);
                    }
                    else if (m_iCoordinateType == tagCoordinateType.LeftUp)
                    {
                        offsetRow[index] = actualFovHeight * jgg;
                        offsetCol[index] = actualFovWidth * (jgg % 2 == 0 ? igg : m_xFovNum - igg - 1);
                    }
                    else if (m_iCoordinateType == tagCoordinateType.RightDown)
                    {
                        offsetRow[index] = actualFovHeight * (m_yFovNum - jgg - 1);
                        offsetCol[index] = actualFovWidth * (jgg % 2 == 0 ? m_xFovNum - igg - 1 : igg);
                    }
                    else
                    {
                        offsetRow[index] = actualFovHeight * jgg;
                        offsetCol[index] = actualFovWidth * (jgg % 2 == 0 ? m_xFovNum - igg - 1 : igg);
                    }

                    row1[index] = row;
                    row2[index] = m_pFovPixelHeight - row;
                    col1[index] = col;
                    col2[index] = m_pFovPixelWidth - col;
                }
            }

            #endregion
        }

        public HObject createPcbImage()
        {
            #region ***** 生成拼接大图 *****

            HObject tiledImage;
            HOperatorSet.GenEmptyObj(out tiledImage);

            HOperatorSet.TileImagesOffset(imageList, out tiledImage, offsetRow, offsetCol, row1, col1, row2, col2, Width, Height);

            return tiledImage;

            #endregion
        }

        public void showPcbInfo(HWindow window)
        {
            //获取当前显示信息
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null, hv_LineStype = null;
            int hv_lineWidth;
            string hv_Draw;

            window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);
            hv_lineWidth = (int)window.GetLineWidth();
            hv_Draw = window.GetDraw();
            hv_LineStype = window.GetLineStyle();

            window.SetLineWidth(1);//设置线宽
            window.SetLineStyle(new HTuple(20,7));
            window.SetColor("dim gray");//十字架显示颜色
            

            for (int igg = 1; igg <= m_xFovNum - 1; igg++) //col
            {
                for (int jgg = 1; jgg <= m_yFovNum - 1; jgg++)//row
                {
                    double col = igg * actualFovWidth;
                    double row = jgg * actualFovHeight;
                    window.DispLine(row,0,row,Width);
                    window.DispLine(0, col, Height, col);
                }
            }

            //恢复窗口显示信息
            window.SetRgb(hv_Red, hv_Green, hv_Blue);
            window.SetLineWidth(hv_lineWidth);
            window.SetDraw(hv_Draw);
            window.SetLineStyle(hv_LineStype);

            //window.SetColor("dim gray");
            //window.DispLine(-100.0, -100.0, -101.0, -101.0);
        }
    }
}
