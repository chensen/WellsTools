using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Wells.Controls.VisionInspect
{
    public class clsPCB
    {
        #region ***** 参数变量 *****

        /// <summary>
        /// 私有实例
        /// </summary>
        private static clsPCB _PCB = null;

        /// <summary>
        /// 唯一公共实例，单例模式
        /// </summary>
        public static clsPCB m_pPCB
        {
            get
            {
                if (_PCB == null)
                    _PCB = new clsPCB();
                return _PCB;
            }
        }

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
        /// 搜索点位所在Fov索引，提高效率
        /// </summary>
        public int m_iCurpos = -1;

        /// <summary>
        /// 视图列表
        /// </summary>
        public List<clsCameraView> m_CameraViewList;


        public List<clsPart> m_PartList;

        /// <summary>
        /// 绑定的控件
        /// </summary>
        internal ImageDoc imageDoc = null;

        #endregion

        public static clsPCB get_Instance()
        {
            if (_PCB == null)
                _PCB = new clsPCB();
            return _PCB;
        }

        private clsPCB()
        {
            #region 初始化，默认参数

            initialize(350000, 250000, 8000, 8000, 1920, 1200, new Point(0, 0), tagCoordinateType.LeftDown, tagImageCoordinateType.LeftUp);

            #endregion
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

            m_PartList = new List<clsPart>();

            preparePCBView();

            #endregion
        }

        public void SetResolution(int uResolutionX, int uResolutionY)
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
                m_CameraViewList = new List<clsCameraView>();
            else
                m_CameraViewList.Clear();

            if ((m_xFovNum != m_xOldFovNum) || (m_yFovNum != m_yOldFovNum))
            {
                #region ***** 完全重新分配 *****

                m_CameraViewList = new List<clsCameraView>();

                for (int y = 0; y < m_yFovNum; y++)
                {
                    Point pt = new Point();
                    pt.Y = m_yStep / 2 + y * m_yStep + m_ptOriginOffset.Y;
                    if (y % 2 == 0)
                    {
                        for (int x = 0; x < m_xFovNum; x++)
                        {
                            pt.X = m_xStep / 2 + x * m_xStep + m_ptOriginOffset.X;
                            clsCameraView pView = new clsCameraView();
                            pView.m_iIndex = m_CameraViewList.Count;
                            pView.m_lptCenter = pt;
                            pView.m_image.Width = m_pFovPixelWidth;
                            pView.m_image.Height = m_pFovPixelHeight;
                            pView.m_image.Color = m_bColor;
                            pView.linkToView(imageDoc);
                            m_CameraViewList.Add(pView);
                        }
                    }
                    else
                    {
                        for (int x = m_xFovNum - 1; x >= 0; x--)
                        {
                            pt.X = m_xStep / 2 + x * m_xStep + m_ptOriginOffset.X;
                            clsCameraView pView = new clsCameraView();
                            pView.m_iIndex = m_CameraViewList.Count;
                            pView.m_lptCenter = pt;
                            pView.m_image.Width = m_pFovPixelWidth;
                            pView.m_image.Height = m_pFovPixelHeight;
                            pView.m_image.Color = m_bColor;
                            pView.linkToView(imageDoc);
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
                                clsCameraView pView = m_CameraViewList[index];
                                pView.m_lptCenter = pt;
                            }
                        }
                        else
                        {
                            for (int x = m_xFovNum - 1; x >= 0; x--)
                            {
                                pt.X = m_xStep / 2 + x * m_xStep + m_ptOriginOffset.X;
                                int index = y * m_xFovNum + (m_xFovNum - 1 - x);
                                clsCameraView pView = m_CameraViewList[index];
                                pView.m_lptCenter = pt;
                            }
                        }
                    }

                    #endregion
                }
            }

            #endregion
        }

        public bool getPixelPCBImage(Point pt, out byte R, out byte G, out byte B)
        {
            #region 根据物理坐标获取区域像素值

            bool ret = false;
            R = G = B = 100;

            if (m_CameraViewList != null)
            {
                if (m_CameraViewList.Count > 0)
                {
                    if (m_iCurpos >= 0 && m_iCurpos < m_CameraViewList.Count)
                    {
                        clsCameraView pView = m_CameraViewList[m_iCurpos];
                        if (pView != null)
                        {
                            int xDistance = Math.Abs(pView.m_lptCenter.X - pt.X);
                            int yDistance = Math.Abs(pView.m_lptCenter.Y - pt.Y);
                            if (((2 * xDistance) <= m_xStep) && ((2 * yDistance) <= m_yStep))
                            {
                                return pView.getPixelViewImage(pt, out R, out G, out B);
                            }
                        }
                    }
                }

                for (int igg = 0; igg < m_CameraViewList.Count; igg++)
                {
                    m_iCurpos = igg;
                    clsCameraView pView = m_CameraViewList[igg];
                    if (pView != null)
                    {
                        int xDistance = Math.Abs(pView.m_lptCenter.X - pt.X);
                        int yDistance = Math.Abs(pView.m_lptCenter.Y - pt.Y);
                        if (((2 * xDistance) <= m_xStep) && ((2 * yDistance) <= m_yStep))
                        {
                            return pView.getPixelViewImage(pt, out R, out G, out B);
                        }
                    }
                }

                m_iCurpos = -1;
            }

            return ret;

            #endregion
        }

        public clsPart findSelectedPart(Point pt)
        {
            clsPart retPart = null;

            foreach (clsPart pPart in m_PartList)
            {
                if (pPart.isPtInRect(pt))
                {
                    retPart = pPart;
                    break;
                }
            }

            return retPart;
        }
    }
}
