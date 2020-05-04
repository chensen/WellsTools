using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Wells.Controls.VisionInspect
{
    public class clsPart
    {
        #region ***** 参数字段变量 *****

        /// <summary>
        /// 
        /// </summary>
        public Point m_lptCenter;

        /// <summary>
        /// 
        /// </summary>
        public int m_uSizeX;

        /// <summary>
        /// 
        /// </summary>
        public int m_uSizeY;

        /// <summary>
        /// 
        /// </summary>
        public bool m_bResult;

        public bool m_bSelected;


        public List<clsInspectObject> m_objList;

        #endregion

        public clsPart()
        {
            m_lptCenter = Point.Empty;
            m_uSizeX = 2000;
            m_uSizeY = 2000;
            m_objList = new List<clsInspectObject>();
            m_bSelected = false;
            m_bResult = true;
        }

        public Rectangle getAbsoluteRect()
        {
            return new Rectangle(m_lptCenter.X - m_uSizeX, m_lptCenter.Y - m_uSizeY, m_uSizeX, m_uSizeY);
        }

        public bool isPtInRect(Point pt)
        {
            return getAbsoluteRect().Contains(pt);
        }

        public void drawScreenRect(Graphics g, Rectangle rect)
        {
            Color bodyColor = Color.Lime;
            if (m_objList.Count == 0) bodyColor = Color.Blue;
            else if (!m_bResult) bodyColor = Color.Red;

            g.DrawRectangle(new Pen(bodyColor, 1), rect);

            if (m_bSelected)
            {
                int sizeRect = 5;
                g.FillRectangle(Brushes.DodgerBlue, new Rectangle(rect.X - sizeRect / 2, rect.Y - sizeRect / 2, sizeRect, sizeRect));
                g.FillRectangle(Brushes.DodgerBlue, new Rectangle(rect.X + rect.Width - sizeRect / 2, rect.Y - sizeRect / 2, sizeRect, sizeRect));
                g.FillRectangle(Brushes.DodgerBlue, new Rectangle(rect.X - sizeRect / 2, rect.Y + rect.Height - sizeRect / 2, sizeRect, sizeRect));
                g.FillRectangle(Brushes.DodgerBlue, new Rectangle(rect.X + rect.Width - sizeRect / 2, rect.Y + rect.Height - sizeRect / 2, sizeRect, sizeRect));
            }
        }
    }
}
