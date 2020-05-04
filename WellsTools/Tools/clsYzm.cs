﻿using System;
using System.Web;
using System.Drawing;
using System.Security.Cryptography;

namespace Wells.Tools
{
    /// <summary>
    /// 验证码类
    /// </summary>
    public class clsRand
    {
        #region 生成随机数字
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        public static string genNumber(int Length)
        {
            return genNumber(Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string genNumber(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        #endregion

        #region 生成随机字母与数字
        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="IntStr">生成长度</param>
        public static string genStr(int Length)
        {
            return genStr(Length, false);
        }

        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string genStr(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
        #endregion

        #region 生成随机纯字母随机数
        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="IntStr">生成长度</param>
        public static string genStr_char(int Length)
        {
            return genStr_char(Length, false);
        }

        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string genStr_char(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
        #endregion
    }

    /// <summary>
    /// 验证图片类
    /// </summary>
    public class clsYzm
    {
        #region 私有字段
        private string text;
        private Bitmap image;
        private int letterCount = 4;   //验证码位数
        private int letterWidth = 16;  //单个字体的宽度范围
        private int letterHeight = 20; //单个字体的高度范围
        private static byte[] randb = new byte[4];
        private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
        private Font[] fonts = 
    {
       new Font(new FontFamily("Times New Roman"),10 +genNext(1),System.Drawing.FontStyle.Regular),
       new Font(new FontFamily("Georgia"), 10 + genNext(1),System.Drawing.FontStyle.Regular),
       new Font(new FontFamily("Arial"), 10 + genNext(1),System.Drawing.FontStyle.Regular),
       new Font(new FontFamily("Comic Sans MS"), 10 + genNext(1),System.Drawing.FontStyle.Regular)
    };
        #endregion

        #region 公有属性
        /// <summary>
        /// 验证码
        /// </summary>
        public string Text
        {
            get { return this.text; }
        }

        /// <summary>
        /// 验证码图片
        /// </summary>
        public Bitmap Image
        {
            get { return this.image; }
        }
        #endregion

        #region 构造函数
        public clsYzm()
        {
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.AddHeader("pragma", "no-cache");
            HttpContext.Current.Response.CacheControl = "no-cache";
            this.text = clsRand.genNumber(4);
            createImage();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
        private static int genNext(int max)
        {
            rand.GetBytes(randb);
            int value = BitConverter.ToInt32(randb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        private static int genNext(int min, int max)
        {
            int value = genNext(max - min) + min;
            return value;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 绘制验证码
        /// </summary>
        public void createImage()
        {
            int int_ImageWidth = this.text.Length * letterWidth;
            Bitmap image = new Bitmap(int_ImageWidth, letterHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            for (int i = 0; i < 2; i++)
            {
                int x1 = genNext(image.Width - 1);
                int x2 = genNext(image.Width - 1);
                int y1 = genNext(image.Height - 1);
                int y2 = genNext(image.Height - 1);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            int _x = -12, _y = 0;
            for (int int_index = 0; int_index < this.text.Length; int_index++)
            {
                _x += genNext(12, 16);
                _y = genNext(-2, 2);
                string str_char = this.text.Substring(int_index, 1);
                str_char = genNext(1) == 1 ? str_char.ToLower() : str_char.ToUpper();
                Brush newBrush = new SolidBrush(getRandomColor());
                Point thePos = new Point(_x, _y);
                g.DrawString(str_char, fonts[genNext(fonts.Length - 1)], newBrush, thePos);
            }
            for (int i = 0; i < 10; i++)
            {
                int x = genNext(image.Width - 1);
                int y = genNext(image.Height - 1);
                image.SetPixel(x, y, Color.FromArgb(genNext(0, 255), genNext(0, 255), genNext(0, 255)));
            }
            image = twistImage(image, true, genNext(1, 3), genNext(4, 6));
            g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, int_ImageWidth - 1, (letterHeight - 1));
            this.image = image;
        }

        /// <summary>
        /// 字体随机颜色
        /// </summary>
        public Color getRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            int int_Red = RandomNum_First.Next(180);
            int int_Green = RandomNum_Sencond.Next(180);
            int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高,一般为3</param>
        /// <param name="dPhase">波形的起始相位,取值区间[0-2*PI)</param>
        public System.Drawing.Bitmap twistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            double PI = 6.283185307179586476925286766559;
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI * (double)j) / dBaseAxisLen : (PI * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            srcBmp.Dispose();
            return destBmp;
        }
        #endregion
    }
}