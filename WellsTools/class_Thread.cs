using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wells
{
    public class class_Thread
    {
        /// <summary>
        /// 指示程序只能运行一个实例
        /// </summary>
        private static System.Threading.Mutex muSingleApp = null;

        public static bool isAppSingle(string name)
        {
            bool bSingle = true;
            muSingleApp = new System.Threading.Mutex(true, name, out bSingle);
            return bSingle;
        }

        public static void releaseApp()
        {
            if (muSingleApp != null)
                muSingleApp.ReleaseMutex();
        }

        public static void Sleep(double ms)
        {
            try
            {
                long ticks = DateTime.Now.Ticks;
                while ((double)(DateTime.Now.Ticks - ticks) / 10000.0 < ms)
                {
                    Application.DoEvents();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
