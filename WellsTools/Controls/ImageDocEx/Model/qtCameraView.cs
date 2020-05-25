using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using hvppleDotNet;

namespace Wells.Controls.ImageDocEx
{
    public class qtCameraView
    {
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
        public qtImage m_image;

        /// <summary>
        /// 绑定的视图控件
        /// </summary>
        internal ImageDocEx imageDocEx;

        public qtCameraView()
        {
            #region 默认参数

            m_iIndex = 0;
            m_lptCenter = new Point(0, 0);
            m_lptMarkOffset = new Point(0, 0);
            m_image = new qtImage();
            imageDocEx = null;

            #endregion
        }
    }
}
