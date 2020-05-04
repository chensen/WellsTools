using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Wells
{
    class class_Win32
    {

        #region Win32常量
        public const int SPI_SETDESKWALLPAPER = 20;//设置系统桌面背景
        public const uint SHGFI_ICON = 0x100;// 标准图标
        public const uint SHGFI_LARGEICON = 0x0; // 大图标
        public const uint SHGFI_SMALLICON = 0x1; // 小图标
        public const int WM_SYSCOMMAND = 0x0112;//该变量表示将向Windows发送的消息类型
        public const int SC_MOVE = 0xF010;//该变量表示发送消息的附加消息
        public const int HTCAPTION = 0x0002;//该变量表示发送消息的附加消息
        public const int LVM_SETICONAPACING = 0x1035;
        public static int AW_HIDE = 0x00010000; //该变量表示动画隐藏窗体
        public static int AW_SLIDE = 0x00040000;//该变量表示出现滑行效果的窗体
        public static int AW_VER_NEGATIVE = 0x00000008;//该变量表示从下向上开屏
        public static int AW_VER_POSITIVE = 0x00000004;//该变量表示从上向下开屏
        #endregion

        //用来释放被当前线程中某个窗口捕获的光标
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        //向指定的窗体发送Windows消息
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwdn, int wMsg, int mParam, int lParam);
        //从exe\dll\ico文件中获取指定索引或ID号的图标句柄
        [DllImport("shell32.dll", EntryPoint = "ExtractIcon")]
        public static extern int ExtractIcon(IntPtr hInst, string lpFileName, int nIndex);
        //获取文件图标的API函数
        [DllImport("shell32.dll", EntryPoint = "SHGetFileInfo")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttribute, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint Flags);
        //获取文件夹图标的API函数
        [DllImport("User32.dll", EntryPoint = "DestroyIcon")]
        public static extern int DestroyIcon(IntPtr hIcon);
        //从exe\dll\ico文件中生成图标句柄数组
        [DllImport("shell32.dll")]
        public static extern uint ExtractIconEx(string lpszFile, int nIconIndex, int[] phiconLarge, int[] phiconSmall, uint nIcons);
        //清空指定驱动器的回收站
        [DllImport("shell32.dll")]
        public static extern int SHEmptyRecycleBin(IntPtr hwnd, int pszRootPath, int dwFlags);
        //获取缓存路径的API函数
        [DllImport("kernel32")]
        public static extern int GetTempPath(int nBufferLength, ref StringBuilder lpBuffer);
        //查询或设置系统级参数
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfoA")]
        public static extern Int32 SystemParametersInfo(Int32 uAction, Int32 uParam, string lpvparam, Int32 fuwinIni);
        //定义系统API入口点，用来关闭、注销或者重启计算机
        [DllImport("user32.dll", EntryPoint = "ExitWindowsEx", CharSet = CharSet.Ansi)]
        public static extern int ExitWindowsEx(int uFlags, int dwReserved);
        //打开系统的命令窗口
        [DllImport("shell32.dll", EntryPoint = "ShellExecute")]
        public static extern int ShellExecute(int hwnd, String lpOperation, String lpFile, String lpParameters, String lpDirectory, int nShowCmd);
        //顶部隐藏
        [DllImport("User32.dll")]
        public static extern bool PtInRect(ref Rectangle r, Point p);
        //动画效果显示窗体
        [DllImportAttribute("user32.dll")]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        //文件信息结构
        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;//图标句柄
            public IntPtr iIcon;//系统图标列表的索引
            public uint dwAttributes;//文件属性
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;//文件的路径
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;//文件的类型名
        }
    }
}
