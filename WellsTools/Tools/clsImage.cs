using System;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
 
namespace Wells.Tools
{
    /// <summary>
    /// ö��,��������ͼģʽ
    /// </summary>
    public enum ThumbnailMod : byte
    {
        /// <summary>
        /// HW
        /// </summary>
        HW,
        /// <summary>
        /// W
        /// </summary>
        W,
        /// <summary>
        /// H
        /// </summary>
        H,
        /// <summary>
        /// Cut
        /// </summary>
        Cut
    };

    /// <summary>
    /// ͼ������
    /// </summary>
    public static class clsImage
    {
        #region ������תbitmap
        public static Bitmap ByteToImage(byte[] arr)
        {
            Bitmap bitmap = null;
            if (arr != null)
            {
                MemoryStream ms = new MemoryStream(arr);
                bitmap = (Bitmap)Bitmap.FromStream(ms);
                ms.Dispose();
            }
            return bitmap;
        }
        #endregion

        #region bitmapת������
        public static byte[] ImageToByte(Bitmap bitmap, ImageFormat format)
        {
            if (bitmap != null)
            {
                var newbitmap = new Bitmap(bitmap);
                MemoryStream ms = new MemoryStream();
                newbitmap.Save(ms, format);
                newbitmap.Dispose();
                return ms.ToArray();
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region imageת������
        public static byte[] ImageToByte(Image image, ImageFormat format)
        {
            if (image != null)
            {
                MemoryStream ms = new MemoryStream();
                image.Save(ms, format);
                return ms.ToArray();
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region ͼƬ�ϻ���
        public static Image DrawImage(Image bitmap, int x, int y, int with, int height, Color color , float penWidth = 2f)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                var rectangle = new Rectangle(x, y, with, height);
                g.DrawRectangle(new Pen(color, penWidth), rectangle);
            }
            return bitmap;
        }
        #endregion

        #region ͼƬ�ϻ���
        public static Image DrawImage(Image bitmap, Rectangle rect, Color color, float penWidth = 2f)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                var rectangle = rect;
                g.DrawRectangle(new Pen(color, penWidth), rectangle);
            }
            return bitmap;
        }
        #endregion

        #region ͼƬ��ˮӡ����
        public static Image DrawImage(Image bitmap,string strPrintInfo,Brush brush,float emSize,PointF location)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Font f = new Font("����", emSize, FontStyle.Bold, GraphicsUnit.Point);
                g.DrawString(strPrintInfo, f, brush, location);
            }
            return bitmap;
        }
        #endregion

        #region ͼƬ�ϻ����ˮӡ����
        public static Image DrawImage(Image bitmap, int x, int y, int with, int height, string strPrintInfo, Brush brush, float emSize, PointF location, Color color, float penWidth = 2f)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                var rectangle = new Rectangle(x, y, with, height);
                g.DrawRectangle(new Pen(color, penWidth), rectangle);
                Font f = new Font("����", emSize, FontStyle.Bold, GraphicsUnit.Point);
                g.DrawString(strPrintInfo, f, brush, location);
            }
            return bitmap;
        }
        #endregion

        #region ͼƬ�ϻ����ˮӡ����
        public static Image DrawImage(Image bitmap, Rectangle rect, string strPrintInfo, Brush brush, float emSize, PointF location, Color color, float penWidth = 2f)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                var rectangle = rect;
                g.DrawRectangle(new Pen(color, penWidth), rectangle);
                Font f = new Font("����", emSize, FontStyle.Bold, GraphicsUnit.Point);
                g.DrawString(strPrintInfo, f, brush, location);
            }
            return bitmap;
        }
        #endregion

        #region ����ͼ
        public static Image MakeThumbnail(Image originalImage, int width, int height, ThumbnailMod mode)
        {
            int towidth = width;
            int toheight = height;
 
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
 
            switch (mode)
            {
                case ThumbnailMod.HW:  //ָ���߿����ţ����ܱ��Σ�                
                    break;
                case ThumbnailMod.W:   //ָ�����߰�����                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumbnailMod.H:   //ָ���ߣ�������
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ThumbnailMod.Cut: //ָ���߿�ü��������Σ�                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
 
            //�½�һ��bmpͼƬ
            Image bitmap = new Bitmap(towidth, toheight);
 
            //�½�һ������
            Graphics g = Graphics.FromImage(bitmap);
 
            //���ø�������ֵ��
            g.InterpolationMode = InterpolationMode.High;
 
            //���ø�����,���ٶȳ���ƽ���̶�
            g.SmoothingMode = SmoothingMode.HighQuality;
 
            //��ջ�������͸������ɫ���
            g.Clear(Color.Transparent);
 
            //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

            g.Dispose();

            return bitmap;
        }
        #endregion

        #region ͼƬ�ϴ�ͼƬˮӡ
        /// <summary>
        /// ͼƬˮӡ������
        /// </summary>
        /// <param name="originalImage">��Ҫ����ˮӡ��ͼƬ</param>
        /// <param name="waterImage">ˮӡͼƬ</param>
        /// <param name="location">ˮӡλ�ã�������ȷ�Ĵ��룩</param>
        public static void  ImageWatermark(ref Image originalImage, Image waterImage, string location)
        {
            Graphics g = Graphics.FromImage(originalImage);
            ArrayList loca = GetLocation(location, originalImage, waterImage);
            g.DrawImage(waterImage, new Rectangle(int.Parse(loca[0].ToString()), int.Parse(loca[1].ToString()), waterImage.Width, waterImage.Height));
            g.Dispose();
        }
 
        /// <summary>
        /// ͼƬˮӡλ�ô�����
        /// </summary>
        /// <param name="location">ˮӡλ��</param>
        /// <param name="img">��Ҫ���ˮӡ��ͼƬ</param>
        /// <param name="waterimg">ˮӡͼƬ</param>
        private static ArrayList GetLocation(string location, Image img, Image waterimg)
        {
            ArrayList loca = new ArrayList();
            int x = 0;
            int y = 0;
 
            if (location == "LT")
            {
                x = 10;
                y = 10;
            }
            else if (location == "T")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else if (location == "RT")
            {
                x = img.Width - waterimg.Width;
                y = 10;
            }
            else if (location == "LC")
            {
                x = 10;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - waterimg.Width;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "LB")
            {
                x = 10;
                y = img.Height - waterimg.Height;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else
            {
                x = img.Width - waterimg.Width;
                y = img.Height - waterimg.Height;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;
        }
        #endregion
 
        #region ͼƬ������ˮӡ
        /// <summary>
        /// ͼƬˮӡ������
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="size"></param>
        /// <param name="letter"></param>
        /// <param name="color"></param>
        /// <param name="location"></param>
        public static void LetterWatermark(ref Image originalImage, int size, string letter, Color color, string location)
        {
            Graphics gs = Graphics.FromImage(originalImage);
            ArrayList loca = GetLocation(location, originalImage, size, letter.Length);
            Font font = new Font("����", size);
            Brush br = new SolidBrush(color);
            gs.DrawString(letter, font, br, float.Parse(loca[0].ToString()), float.Parse(loca[1].ToString()));
            gs.Dispose();
        }
 
        /// <summary>
        /// ����ˮӡλ�õķ���
        /// </summary>
        /// <param name="location">λ�ô���</param>
        /// <param name="img">ͼƬ����</param>
        /// <param name="width">��(��ˮӡ����Ϊ����ʱ,�������ľ�������Ĵ�С)</param>
        /// <param name="height">��(��ˮӡ����Ϊ����ʱ,�������ľ����ַ��ĳ���)</param>
        private static ArrayList GetLocation(string location, Image img, int width, int height)
        {
            #region
 
            ArrayList loca = new ArrayList();  //��������洢λ��
            float x = 10;
            float y = 10;
 
            if (location == "LT")
            {
                loca.Add(x);
                loca.Add(y);
            }
            else if (location == "T")
            {
                x = img.Width / 2 - (width * height) / 2;
                loca.Add(x);
                loca.Add(y);
            }
            else if (location == "RT")
            {
                x = img.Width - width * height;
            }
            else if (location == "LC")
            {
                y = img.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - (width * height) / 2;
                y = img.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - height;
                y = img.Height / 2;
            }
            else if (location == "LB")
            {
                y = img.Height - width - 5;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - (width * height) / 2;
                y = img.Height - width - 5;
            }
            else
            {
                x = img.Width - width * height;
                y = img.Height - width - 5;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;
 
            #endregion
        }
        #endregion
 
        #region �����ⰵ
        /// <summary>
        /// �����ⰵ
        /// </summary>
        /// <param name="mybm">ԭʼͼƬ</param>
        /// <param name="width">ԭʼͼƬ�ĳ���</param>
        /// <param name="height">ԭʼͼƬ�ĸ߶�</param>
        /// <param name="val">���ӻ���ٵĹⰵֵ</param>
        public static Bitmap LDPic(Bitmap mybm, int width, int height, int val)
        {
            Bitmap bm = new Bitmap(width, height);//��ʼ��һ����¼����������ͼƬ����
            int x, y, resultR, resultG, resultB;//x��y��ѭ�����������������Ǽ�¼����������ֵ��
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���ص�ֵ
                    resultR = pixel.R + val;//����ɫֵ�᲻�ᳬ��[0, 255]
                    resultG = pixel.G + val;//�����ɫֵ�᲻�ᳬ��[0, 255]
                    resultB = pixel.B + val;//�����ɫֵ�᲻�ᳬ��[0, 255]
                    bm.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB));//��ͼ
                }
            }
            return bm;
        }
        #endregion
 
        #region ��ɫ����
        /// <summary>
        /// ��ɫ����
        /// </summary>
        /// <param name="mybm">ԭʼͼƬ</param>
        /// <param name="width">ԭʼͼƬ�ĳ���</param>
        /// <param name="height">ԭʼͼƬ�ĸ߶�</param>
        public static Bitmap RePic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//��ʼ��һ����¼������ͼƬ�Ķ���
            int x, y, resultR, resultG, resultB;
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���������ֵ
                    resultR = 255 - pixel.R;//����
                    resultG = 255 - pixel.G;//����
                    resultB = 255 - pixel.B;//����
                    bm.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB));//��ͼ
                }
            }
            return bm;
        }
        #endregion
 
        #region ������
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="oldBitmap">ԭʼͼƬ</param>
        /// <param name="Width">ԭʼͼƬ�ĳ���</param>
        /// <param name="Height">ԭʼͼƬ�ĸ߶�</param>
        public static Bitmap FD(Bitmap oldBitmap, int Width, int Height)
        {
            Bitmap newBitmap = new Bitmap(Width, Height);
            Color color1, color2;
            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    color1 = oldBitmap.GetPixel(x, y);
                    color2 = oldBitmap.GetPixel(x + 1, y + 1);
                    r = Math.Abs(color1.R - color2.R + 128);
                    g = Math.Abs(color1.G - color2.G + 128);
                    b = Math.Abs(color1.B - color2.B + 128);
                    if (r > 255) r = 255;
                    if (r < 0) r = 0;
                    if (g > 255) g = 255;
                    if (g < 0) g = 0;
                    if (b > 255) b = 255;
                    if (b < 0) b = 0;
                    newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return newBitmap;
        }
        #endregion
 
        #region ����ͼƬ
        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="bmp">ԭʼͼƬ</param>
        /// <param name="newW">�µĿ��</param>
        /// <param name="newH">�µĸ߶�</param>
        public static Bitmap ResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap bap = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(bap);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bap, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bap.Width, bap.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return bap;
            }
            catch
            {
                return null;
            }
        }
        #endregion
 
        #region ��ɫ����
        /// <summary>
        /// ��ɫ����
        /// </summary>
        /// <param name="mybm">ԭʼͼƬ</param>
        /// <param name="width">ԭʼͼƬ�ĳ���</param>
        /// <param name="height">ԭʼͼƬ�ĸ߶�</param>
        public static Bitmap FilPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//��ʼ��һ����¼��ɫЧ����ͼƬ����
            int x, y;
            Color pixel;
 
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���������ֵ
                    bm.SetPixel(x, y, Color.FromArgb(0, pixel.G, pixel.B));//��ͼ
                }
            }
            return bm;
        }
        #endregion
 
        #region ���ҷ�ת
        /// <summary>
        /// ���ҷ�ת
        /// </summary>
        /// <param name="mybm">ԭʼͼƬ</param>
        /// <param name="width">ԭʼͼƬ�ĳ���</param>
        /// <param name="height">ԭʼͼƬ�ĸ߶�</param>
        public static Bitmap RevPicLR(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, z; //x,y��ѭ������,z��������¼���ص��x����ı仯��
            Color pixel;
            for (y = height - 1; y >= 0; y--)
            {
                for (x = width - 1, z = 0; x >= 0; x--)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���ص�ֵ
                    bm.SetPixel(z++, y, Color.FromArgb(pixel.R, pixel.G, pixel.B));//��ͼ
                }
            }
            return bm;
        }
        #endregion
 
        #region ���·�ת
        /// <summary>
        /// ���·�ת
        /// </summary>
        /// <param name="mybm">ԭʼͼƬ</param>
        /// <param name="width">ԭʼͼƬ�ĳ���</param>
        /// <param name="height">ԭʼͼƬ�ĸ߶�</param>
        public static Bitmap RevPicUD(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, z;
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = height - 1, z = 0; y >= 0; y--)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���ص�ֵ
                    bm.SetPixel(x, z++, Color.FromArgb(pixel.R, pixel.G, pixel.B));//��ͼ
                }
            }
            return bm;
        }
        #endregion
 
        #region ѹ��ͼƬ
        /// <summary>
        /// ѹ����ָ���ߴ�
        /// </summary>
        /// <param name="oldfile">ԭ�ļ�</param>
        /// <param name="newfile">���ļ�</param>
        public static bool Compress(string oldfile, string newfile)
        {
            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(oldfile);
                System.Drawing.Imaging.ImageFormat thisFormat = img.RawFormat;
                Size newSize = new Size(100, 125);
                Bitmap outBmp = new Bitmap(newSize.Width, newSize.Height);
                Graphics g = Graphics.FromImage(outBmp);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.Dispose();
                EncoderParameters encoderParams = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 100;
                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICI = null;
                for (int x = 0; x < arrayICI.Length; x++)
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICI = arrayICI[x]; //����JPEG����
                        break;
                    }
                img.Dispose();
                if (jpegICI != null) outBmp.Save(newfile, System.Drawing.Imaging.ImageFormat.Jpeg);
                outBmp.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
 
        #region ͼƬ�ҶȻ�
        public static Color Gray(Color c)
        {
            int rgb = Convert.ToInt32((double)(((0.3 * c.R) + (0.59 * c.G)) + (0.11 * c.B)));
            return Color.FromArgb(rgb, rgb, rgb);
        }
        #endregion
 
        #region ת��Ϊ�ڰ�ͼƬ
        /// <summary>
        /// ת��Ϊ�ڰ�ͼƬ
        /// </summary>
        /// <param name="mybt">Ҫ���д����ͼƬ</param>
        /// <param name="width">ͼƬ�ĳ���</param>
        /// <param name="height">ͼƬ�ĸ߶�</param>
        public static Bitmap BWPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, result; //x,y��ѭ��������result�Ǽ�¼����������ֵ
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//��ȡ��ǰ���������ֵ
                    result = (pixel.R + pixel.G + pixel.B) / 3;//ȡ��������ɫ��ƽ��ֵ
                    bm.SetPixel(x, y, Color.FromArgb(result, result, result));
                }
            }
            return bm;
        }
        #endregion
 
        #region ��ȡͼƬ�еĸ�֡
        /// <summary>
        /// ��ȡͼƬ�еĸ�֡
        /// </summary>
        /// <param name="pPath">ͼƬ·��</param>
        /// <param name="pSavePath">����·��</param>
        public static void GetFrames(string pPath, string pSavedPath)
        {
            Image gif = Image.FromFile(pPath);
            FrameDimension fd = new FrameDimension(gif.FrameDimensionsList[0]);
            int count = gif.GetFrameCount(fd); //��ȡ֡��(gifͼƬ���ܰ�����֡��������ʽͼƬһ���һ֡)
            for (int i = 0; i < count; i++)    //��Jpeg��ʽ�����֡
            {
                gif.SelectActiveFrame(fd, i);
                gif.Save(pSavedPath + "\\frame_" + i + ".jpg", ImageFormat.Jpeg);
            }
        }
        #endregion
    }
}