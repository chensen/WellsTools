using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Wells.Tools
{
    /// <summary>
    /// 图片格式转换类
    /// </summary>
    public class clsImageConvert
    {
        public static Bitmap IntPtr2Bitmap(IntPtr imagebuffer, int iwidth,int iheight)
        {
            #region 从指针转换成Bitmap图像
            PixelFormat format = PixelFormat.Format8bppIndexed;
            Bitmap temp = new Bitmap(iwidth, iheight, format);
            for (int i = 0; i < 256; i++)
                temp.Palette.Entries[i] = Color.FromArgb(i, i, i);
            try
            {
                byte[] m_byMonoBuffer = new byte[iwidth * iheight];
                Marshal.Copy(imagebuffer, m_byMonoBuffer, 0, iwidth * iheight);

                Rectangle rect = new Rectangle(0, 0, iwidth, iheight);
                BitmapData bitmapData = temp.LockBits(rect, ImageLockMode.ReadWrite, temp.PixelFormat);
                //得到一个指向Bitmap的buffer指针
                IntPtr ptrBmp = bitmapData.Scan0;
                int nImageStride = iwidth;
                //图像宽能够被4整除直接copy
                if (nImageStride == bitmapData.Stride)
                {
                    Marshal.Copy(m_byMonoBuffer, 0, ptrBmp, bitmapData.Stride * iheight);
                }
                else//图像宽不能够被4整除按照行copy
                {
                    for (int i = 0; i < iheight; ++i)
                    {
                        Marshal.Copy(m_byMonoBuffer, i * nImageStride, new IntPtr(ptrBmp.ToInt64() + i * bitmapData.Stride), iwidth);
                    }
                }
                temp.UnlockBits(bitmapData);
            }
            catch(Exception exc)
            {
                throw exc;
            }
            return temp;
            #endregion
        }
    }
}
