using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using hvppleDotNet;

namespace WellsToolsDemo
{
    static class Program
    {
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", CharSet = CharSet.Ansi)]
        public extern static long CopyMemory(int dest, int source, int size);
        private static void HObject2Bpp8(HObject image, out Bitmap res)
        {
            HTuple hpoint, type, width, height;

            const int Alpha = 255;
            int[] ptr = new int[2];
            HOperatorSet.GetImagePointer1(image, out hpoint, out type, out width, out height);

            res = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette pal = res.Palette;
            for (int i = 0; i <= 255; i++)
            {
                pal.Entries[i] = Color.FromArgb(Alpha, i, i, i);
            }
            res.Palette = pal;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = res.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            int PixelSize = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
            ptr[0] = bitmapData.Scan0.ToInt32();
            ptr[1] = hpoint.I;
            if (width % 4 == 0)
                CopyMemory(ptr[0], ptr[1], width * height * PixelSize);
            else
            {
                for (int i = 0; i < height - 1; i++)
                {
                    ptr[1] += width;
                    CopyMemory(ptr[0], ptr[1], width * PixelSize);
                    ptr[0] += bitmapData.Stride;
                }
            }
            res.UnlockBits(bitmapData);

        }

        private static void HObject2Bpp24(HObject image, out Bitmap res)
        {
            HTuple hred, hgreen, hblue, type, width, height;

            HOperatorSet.GetImagePointer3(image, out hred, out hgreen, out hblue, out type, out width, out height);

            res = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = res.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* bptr = (byte*)bitmapData.Scan0;
                byte* r = ((byte*)hred.I);
                byte* g = ((byte*)hgreen.I);
                byte* b = ((byte*)hblue.I);
                for (int i = 0; i < width * height; i++)
                {
                    bptr[i * 3] = (b)[i];
                    bptr[i * 3 + 1] = (g)[i];
                    bptr[i * 3 + 2] = (r)[i];
                }
            }

            res.UnlockBits(bitmapData);

        }

        public static Bitmap HObjectToBitmapEx(HObject obj)
        {
            #region ***** HObject转换成Bitmap *****

            Bitmap bmp = null;

            try
            {
                if (obj != null)
                {
                    HOperatorSet.CountChannels(obj, out HTuple channels);

                    if (channels.D == 1)
                    {
                        HTuple hpoint, type, width, height;

                        int[] ptr = new int[2];
                        HOperatorSet.GetImagePointer1(obj, out hpoint, out type, out width, out height);

                        bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                        ColorPalette pal = bmp.Palette;
                        for (int i = 0; i <= 255; i++)
                        {
                            pal.Entries[i] = Color.FromArgb(i, i, i);
                        }
                        bmp.Palette = pal;
                        Rectangle rect = new Rectangle(0, 0, width, height);
                        BitmapData bitmapData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                        int PixelSize = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
                        ptr[0] = bitmapData.Scan0.ToInt32();
                        ptr[1] = hpoint.I;
                        if (width % 4 == 0)
                        {
                            CopyMemory(ptr[0], ptr[1], width * height * PixelSize);
                        }
                        else
                        {
                            for (int i = 0; i < height - 1; i++)
                            {
                                ptr[1] += width;
                                CopyMemory(ptr[0], ptr[1], width * PixelSize);
                                ptr[0] += bitmapData.Stride;
                            }
                        }
                        bmp.UnlockBits(bitmapData);
                    }
                    else if (channels.D == 3)
                    {
                        HTuple hred, hgreen, hblue, type, width, height;

                        HOperatorSet.GetImagePointer3(obj, out hred, out hgreen, out hblue, out type, out width, out height);

                        bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                        Rectangle rect = new Rectangle(0, 0, width, height);
                        BitmapData bitmapData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                        unsafe
                        {
                            byte* bptr = (byte*)bitmapData.Scan0;
                            byte* r = ((byte*)hred.I);
                            byte* g = ((byte*)hgreen.I);
                            byte* b = ((byte*)hblue.I);
                            for (int i = 0; i < width * height; i++)
                            {
                                bptr[i * 3] = (b)[i];
                                bptr[i * 3 + 1] = (g)[i];
                                bptr[i * 3 + 2] = (r)[i];
                            }
                        }

                        bmp.UnlockBits(bitmapData);
                    }
                }
            }
            catch (System.Exception ex)
            {
                bmp = null;
            }

            return bmp;

            #endregion
        }

        private static void HObject2bmp(HObject image, out Bitmap res)
        {
            res = null;

            HOperatorSet.CountChannels(image, out HTuple channels);
            if(channels.D == 1)
            {
                HTuple hpoint, type, width, height;

                const int Alpha = 255;
                int[] ptr = new int[2];
                HOperatorSet.GetImagePointer1(image, out hpoint, out type, out width, out height);

                res = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                ColorPalette pal = res.Palette;
                for (int i = 0; i <= 255; i++)
                {
                    pal.Entries[i] = Color.FromArgb(Alpha, i, i, i);
                }
                res.Palette = pal;
                Rectangle rect = new Rectangle(0, 0, width, height);
                BitmapData bitmapData = res.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                int PixelSize = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
                ptr[0] = bitmapData.Scan0.ToInt32();
                ptr[1] = hpoint.I;
                if (width % 4 == 0)
                    CopyMemory(ptr[0], ptr[1], width * height * PixelSize);
                else
                {
                    for (int i = 0; i < height - 1; i++)
                    {
                        ptr[1] += width;
                        CopyMemory(ptr[0], ptr[1], width * PixelSize);
                        ptr[0] += bitmapData.Stride;
                    }
                }
                res.UnlockBits(bitmapData);
            }
            else if(channels.D == 3)
            {
                HTuple hred, hgreen, hblue, type, width, height;

                HOperatorSet.GetImagePointer3(image, out hred, out hgreen, out hblue, out type, out width, out height);

                res = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                Rectangle rect = new Rectangle(0, 0, width, height);
                BitmapData bitmapData = res.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
                unsafe
                {
                    byte* bptr = (byte*)bitmapData.Scan0;
                    byte* r = ((byte*)hred.I);
                    byte* g = ((byte*)hgreen.I);
                    byte* b = ((byte*)hblue.I);
                    for (int i = 0; i < width * height; i++)
                    {
                        bptr[i * 4] = (b)[i];
                        bptr[i * 4 + 1] = (g)[i];
                        bptr[i * 4 + 2] = (r)[i];
                        bptr[i * 4 + 3] = 255;
                    }
                }

                res.UnlockBits(bitmapData);
            }
        }

        private static void bmp2ho(Bitmap bmp,out HObject image)
        {
            image = null;
            if (bmp.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

                HOperatorSet.GenImage1(out image, "byte", bmp.Width, bmp.Height, srcBmpData.Scan0);
                bmp.UnlockBits(srcBmpData);
            }
            else if (bmp.PixelFormat == PixelFormat.Format24bppRgb)
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                HOperatorSet.GenImageInterleaved(out image, srcBmpData.Scan0, "rgb", bmp.Width, bmp.Height, 0, "byte", 0, 0, 0, 0, -1, 0);
                bmp.UnlockBits(srcBmpData);
            }
        }

        public static void Bitmap2HObjectBpp24(Bitmap bmp, out HObject image)
        {
            try
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                HOperatorSet.GenImageInterleaved(out image, srcBmpData.Scan0, "bgrx", bmp.Width, bmp.Height, 0, "byte", 0, 0, 0, 0, -1, 0);
                bmp.UnlockBits(srcBmpData);

            }
            catch (Exception ex)
            {
                image = null;
            }
        }

        public static void Bitmap2HObjectBpp8(Bitmap bmp, out HObject image)
        {
            try
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

                HOperatorSet.GenImage1(out image, "byte", bmp.Width, bmp.Height, srcBmpData.Scan0);
                bmp.UnlockBits(srcBmpData);
            }
            catch (Exception ex)
            {
                image = null;
            }
        }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(UIThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException +=
            new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Wells.clsWellsLanguage.setLanguageType(0);
            Wells.FrmType.frm_Log.InitDlg();
            //clsSerialize theapp = new clsSerialize();
            //Application.Run(theapp);

            //HOperatorSet.ReadImage(out HObject img, "D:\\0.jpg");
            //HOperatorSet.GetImagePointer3(img, out HTuple R, out HTuple G, out HTuple B, out HTuple type, out HTuple w, out HTuple h);

            //Bitmap bmp = new Bitmap("D:\\mul.bmp");
            //Bitmap2HObjectBpp24(bmp, out HObject img);
            //HOperatorSet.WriteImage(img, "bmp", 0, "d:\\1.bmp");

            Application.Run(new frmHalcon());
        }

        private static void UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            try
            {
                string errorMsg = "Windows窗体线程异常 : \n\n";
                MessageBox.Show(errorMsg + t.Exception.Message + Environment.NewLine + t.Exception.StackTrace);
            }
            catch
            {
                MessageBox.Show("不可恢复的Windows窗体异常，应用程序将退出！");
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                string errorMsg = "非窗体线程异常 : \n\n";
                MessageBox.Show(errorMsg + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            catch
            {
                MessageBox.Show("不可恢复的非Windows窗体线程异常，应用程序将退出！");
            }
        }
    }

    public class CustomExceptionHandler
    {
        public CustomExceptionHandler()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(this.OnThreadException);
        }

        private void OnThreadException(object sender, ThreadExceptionEventArgs args)
        {
            try {
                string errorMsg = "程序运行过程中发生错误,错误信息如下:\n";
                errorMsg += args.Exception.Message; errorMsg += "\n发生错误的程序集为:";
                errorMsg += args.Exception.Source; errorMsg += "\n发生错误的具体位置为:\n";
                errorMsg += args.Exception.StackTrace; errorMsg += "\n\n 请抓取此错误屏幕,并和地星伟业联系!";
                MessageBox.Show(errorMsg, "运行时错误--地星伟业", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("系统运行时发生致命错误!\n请保存好相关数据,重启系统。", "致命错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}
