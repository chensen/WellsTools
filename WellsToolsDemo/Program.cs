using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WellsToolsDemo
{
    static class Program
    {
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
            Application.Run(new MetroControlDemo());
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
