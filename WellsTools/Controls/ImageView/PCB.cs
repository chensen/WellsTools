using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace Wells.Controls.ImageView
{
    public class PCB
    {
        private static PCB _PCB = null;
        public static PCB m_pPCB//唯一公共实例
        {
            get
            {
                if (_PCB == null)
                    _PCB = new PCB();
                return _PCB;
            }
        }
        public int m_uSizeX;//um
        public int m_uSizeY;//um
        public int m_uResolutionX;//1/1000um
        public int m_uResolutionY;//1/1000um
        public int m_pFovPixelWidth;
        public int m_pFovPixelHeight;
        public int m_uFovSizeX;
        public int m_uFovSizeY;
        public int m_iCoordinateType;

        public int m_xStep;
        public int m_yStep;
        public int m_xFovNum;
        public int m_yFovNum;

        public int m_xOldFovNum;
        public int m_yOldFovNum;

        public Point m_ptOriginOffset;

        public ColorPalette palette;

        public static PCB get_Instance()
        {
            if (_PCB == null)
                _PCB = new PCB();
            return _PCB;
        }

        private PCB()
        {
            #region 初始化，默认参数

            m_uSizeX = 350000;
            m_uSizeY = 250000;
            m_uResolutionX = 50000;
            m_uResolutionY = 50000;
            m_pFovPixelWidth = 1920;
            m_pFovPixelHeight = 1200;
            m_uFovSizeX = (int)Math.Round((decimal)m_pFovPixelWidth / 1000 * m_uResolutionX- ConstData.View_Gap);
            m_uFovSizeY = (int)Math.Round((decimal)m_pFovPixelHeight / 1000 * m_uResolutionY- ConstData.View_Gap);
            m_iCoordinateType = ConstData.LeftDown;

            m_xFovNum = m_uSizeX / m_uFovSizeX + 1;
            m_yFovNum = m_uSizeY / m_uFovSizeY + 1;
            m_xStep = m_uSizeX / m_xFovNum;
            m_yStep = m_uSizeY / m_yFovNum;

            m_xOldFovNum = 0;
            m_yOldFovNum = 0;

            m_ptOriginOffset = new Point(0, 0);

            try
            {
                PixelFormat format = PixelFormat.Format8bppIndexed;
                Bitmap bitmap = new Bitmap(m_pFovPixelWidth, m_pFovPixelHeight, format);
                palette = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    this.palette.Entries[i] = Color.FromArgb(i, i, i);
                }
            }
            catch
            {
                //MessageBox.Show("设置水星相机图板失败");
            }

            #endregion
        }

        public void Initialize(int uSizeX, int uSizeY, int uResolutionX, int uResolutionY, int pWidth, int pHeight, Point ptOriginOffset, int type = ConstData.LeftDown)
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

            m_xFovNum = m_uSizeX / m_uFovSizeX + 1;
            m_yFovNum = m_uSizeY / m_uFovSizeY + 1;
            m_xStep = m_uSizeX / m_xFovNum;
            m_yStep = m_uSizeY / m_yFovNum;

            m_xOldFovNum = 0;
            m_yOldFovNum = 0;

            m_ptOriginOffset = ptOriginOffset;

            try
            {
                PixelFormat format = PixelFormat.Format8bppIndexed;
                Bitmap bitmap = new Bitmap(m_pFovPixelWidth, m_pFovPixelHeight, format);
                palette = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    this.palette.Entries[i] = Color.FromArgb(i, i, i);
                }
            }
            catch
            {
                //MessageBox.Show("设置水星相机图板失败");
            }

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

        public bool IsNeedUpdateViewInfo()
        {
            #region 判断是否需要更新视图信息

            return ((m_xFovNum != m_xOldFovNum) || (m_yFovNum != m_yOldFovNum));

            #endregion
        }
    }
}
