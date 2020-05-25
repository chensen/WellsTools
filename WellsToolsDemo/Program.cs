using System;
using System.Collections.Generic;
using System.Linq;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Wells.clsWellsLanguage.setLanguageType(0);
            Wells.FrmType.frm_Log.InitDlg();
            Application.Run(new ImageDocExDemo());
        }
    }
}
