using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Wells.Tools
{
    //描述：
    //软件有时候不允许 用户使用Alt+Tab键切换程序，但是Key_Down事件中只能捕获Alt+F4组合键，找了很多资料，整理了一下，最后分装在下面这个Hook类中，完美实现屏蔽系统组合键
    //使用说明
    //实例化Hook类：Hook hk = new Hook();
    //启动Hook钩子:hk.Hook_Start();

    /// <summary>
    /// 键盘钩子(屏蔽键盘按键)
    /// </summary>
    public class clsHook
    {
        //键盘Hook结构函数 
        [StructLayout(LayoutKind.Sequential)]
        public class KeyBoardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        //委托 
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        private static int hHook = 0;
        public const int WH_KEYBOARD_LL = 13;

        //LowLevel键盘截获，如果是WH_KEYBOARD＝2，并不能对系统键盘截取，Acrobat Reader会在你截取之前获得键盘。 
        public HookProc KeyBoardHookProcedure;

        #region [DllImport("user32.dll")]

        //设置钩子 
        [DllImport("user32.dll")]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //抽掉钩子 
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll")]
        //调用下一个钩子 
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);
        #endregion

        #region 安装键盘钩子

        /// <summary>
        /// 安装键盘钩子
        /// </summary>
        public void start()
        {
            if (hHook == 0)
            {
                KeyBoardHookProcedure = new HookProc(keyBoardHookProc);
                hHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyBoardHookProcedure,GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
                //如果设置钩子失败. 
                if (hHook == 0)
                {
                    clear();
                }
            }
        }
        #endregion
        
        #region 取消钩子事件
        /// <summary>
        /// 取消钩子事件
        /// </summary>
        public void clear()
        {
            bool retKeyboard = true;
            if (hHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hHook);
                hHook = 0;
            }
        }
        #endregion

        #region 屏蔽键盘
        /// <summary>
        /// 屏蔽键盘
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static int keyBoardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                //添加自己的判断语句，如果符合要求的按键，就 return 1; 
                //没有判断直接 return 1;那么就屏蔽所有按键除了ctrl+alt+del
                KeyBoardHookStruct kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
                // 屏蔽左"WIN"、右"Win"
                if ((kbh.vkCode == (int)Keys.LWin) || (kbh.vkCode == (int)Keys.RWin))
                {
                    return 1;
                }
                //屏蔽Ctrl+Esc
                if (kbh.vkCode == (int)Keys.Escape && (int)Control.ModifierKeys == (int)Keys.Control)
                {
                    return 1;
                }
                //屏蔽Alt+f4 
                if (kbh.vkCode == (int)Keys.F4 && (int)Control.ModifierKeys == (int)Keys.Alt)
                {
                    return 1;
                }
                //屏蔽Alt+Esc
                if (kbh.vkCode == (int)Keys.Escape && (int)Control.ModifierKeys == (int)Keys.Alt)
                {
                    return 1;
                }
                //屏蔽Alt+Tab 
                if (kbh.vkCode == (int)Keys.Tab && (int)Control.ModifierKeys == (int)Keys.Alt)
                {
                    return 1;
                }
                //截获Ctrl+Shift+Esc 
                if (kbh.vkCode == (int)Keys.Escape &&(int)Control.ModifierKeys == (int)Keys.Control + (int)Keys.Shift)
                {
                    return 1;
                }
            }
            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }
        #endregion
    }

    public class clsMyHook
    {
        private IntPtr pKeyboardHook = IntPtr.Zero;//键盘钩子句柄
        //钩子委托声明
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        private HookProc KeyboardHookProcedure;//键盘钩子委托实例，不能省略变量
        public const int idHook = 13;//底层键盘钩子
        //安装钩子
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr pInstance, int threadID);
        //卸载钩子
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(IntPtr pHookHandle);

        //键盘钩子处理函数
        private int keyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            KeyMSG m = (KeyMSG)Marshal.PtrToStructure(lParam, typeof(KeyMSG));//键盘消息处理
            if (pKeyboardHook != IntPtr.Zero)//判断钩子句柄是否为空
            {
                switch (((Keys)m.vkCode))//判断按键
                {
                    case Keys.LWin://键盘左侧的Win键
                    case Keys.RWin://键盘右侧的Win键
                    case Keys.Delete://Delete键
                    case Keys.Alt://Alt键
                    case Keys.Escape: //Esc键
                    case Keys.F4: //F4键
                    case Keys.Control://Ctrl键
                    case Keys.Tab://Tab键
                        return 1;//不执行任何操作
                }
            }
            return 0;
        }
        //安装钩子方法
        public bool addHook()
        {
            IntPtr pIn = (IntPtr)4194304;//将4194304转换为句柄
            if (this.pKeyboardHook == IntPtr.Zero)//不存在钩子时
            {
                //创建钩子
                this.KeyboardHookProcedure = new HookProc(keyboardHookProc);
                //使用SetWindowsHookEx函数安装钩子
                this.pKeyboardHook = SetWindowsHookEx(idHook, KeyboardHookProcedure, pIn, 0);
                if (this.pKeyboardHook == IntPtr.Zero)//如果安装钩子失败
                {
                    this.deleteHook();//卸载钩子
                    return false;
                }
            }
            return true;
        }
        //卸载钩子方法
        public bool deleteHook()
        {
            bool result = true;
            if (this.pKeyboardHook != IntPtr.Zero)//如果存在钩子
            {
                //使用UnhookWindowsHookEx函数卸载钩子
                result = (UnhookWindowsHookEx(this.pKeyboardHook) && result);
                this.pKeyboardHook = IntPtr.Zero;//清空指针
            }
            return result;
        }
        //键盘消息处理结构
        [StructLayout(LayoutKind.Sequential)]
        public struct KeyMSG
        {
            public int vkCode;//键盘按键
        }
    }
}