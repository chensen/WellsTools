using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Wells.Controls.VisionInspect
{
    public static class clsPublic
    {
        private static ColorPalette colorPalette = null;

        public static ColorPalette Palette
        {
            get
            {
                if (colorPalette == null)
                {
                    using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
                    {
                        colorPalette = tempBmp.Palette;
                    }
                    for (int i = 0; i < 256; i++)
                    {
                        colorPalette.Entries[i] = Color.FromArgb(i, i, i);
                    }
                }
                return colorPalette;
            }
        }

        public static byte[] getImageData(string imagePath)
        {
            FileStream fs = new FileStream(imagePath, FileMode.Open);
            byte[] byteData = new byte[fs.Length];
            fs.Read(byteData, 0, byteData.Length);
            fs.Close();
            return byteData;
        }

        public static byte[] getImageData(System.Drawing.Image imgPhoto)
        {
            MemoryStream mstream = new MemoryStream();
            imgPhoto.Save(mstream, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] byData = new Byte[mstream.Length];
            mstream.Position = 0;
            mstream.Read(byData, 0, byData.Length); mstream.Close();
            return byData;
        }

        public static System.Drawing.Image getImage(byte[] streamByte)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            return img;
        }

        public static System.Drawing.Bitmap getGrayBitmap(clsImage image)
        {
            //// 申请目标位图的变量，并将其内存区域锁定  
            Bitmap bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            //// 获取图像参数  
            int stride = bmpData.Stride;  // 扫描线的宽度  
            int offset = stride - image.Width;  // 显示宽度与扫描线宽度的间隙  
            IntPtr iptr = bmpData.Scan0;  // 获取bmpData的内存起始位置  
            int scanBytes = stride * image.Height;// 用stride宽度，表示这是内存区域的大小  

            //// 下面把原始的显示大小字节数组转换为内存中实际存放的字节数组  
            int posScan = 0, posReal = 0;// 分别设置两个位置指针，指向源数组和目标数组  
            byte[] pixelValues = new byte[scanBytes];  //为目标数组分配内存  

            for (int x = 0; x < image.Height; x++)
            {
                //// 下面的循环节是模拟行扫描  
                for (int y = 0; y < image.Width; y++)
                {
                    pixelValues[posScan++] = image.ImgBuffer[posReal++];
                }
                posScan += offset;  //行扫描结束，要将目标位置指针移过那段“间隙”  
            }

            //// 用Marshal的Copy方法，将刚才得到的内存字节数组复制到BitmapData中  
            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, iptr, scanBytes);
            bmp.UnlockBits(bmpData);  // 解锁内存区域  

            //// 下面的代码是为了修改生成位图的索引表，从伪彩修改为灰度  
            bmp.Palette = clsPublic.Palette;

            //// 算法到此结束，返回结果  
            return bmp;

        }
    }

    public static class GraphicsEx
    {
        public static void drawImage(this Graphics g, byte[] data, int x, int y, int width, int height)
        {
            g.DrawImage(clsPublic.getImage(data), new Rectangle(x, y, width, height));
        }

        public static void drawImage(this Graphics g, byte[] data, Rectangle dstRect)
        {
            g.DrawImage(clsPublic.getImage(data), dstRect);
        }

        public static void drawImage(this Graphics g, clsImage img, int x, int y, int width, int height)
        {
            g.DrawImage(clsPublic.getGrayBitmap(img), new Rectangle(x, y, width, height));
        }

        public static void drawImage(this Graphics g, clsImage img, Rectangle dstRect)
        {
            g.DrawImage(clsPublic.getGrayBitmap(img), dstRect);
        }
    }
}
