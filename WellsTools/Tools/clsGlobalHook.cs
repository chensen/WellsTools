using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Wells.Tools
{
    public class clsGlobalHook
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

        //鼠标坐标Hook结构函数
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        //鼠标Hook结构函数 
        [StructLayout(LayoutKind.Sequential)]
        public class MouseLLHookStruct
        {
            public POINT pt;
            public int mouseData;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        //委托 
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);
        public clsGlobalHook()
        {
            start();
        }

        public clsGlobalHook(bool InstallMouseHook, bool InstallKeyboardHook)
        {
            start(InstallMouseHook, InstallKeyboardHook);
        }

        ~clsGlobalHook()
        {
            stop(true, true, false);
        }

        public event MouseEventHandler OnMouseActivity;
        public event KeyEventHandler KeyDown;
        public event KeyPressEventHandler KeyPress;
        public event KeyEventHandler KeyUp;

        private int hMouseHook = 0;
        private int hKeyboardHook = 0;

        public const int WH_KEYBOARD_LL = 13;
        public const int WH_MOUSE_LL = 14;

        public const int WM_KEYDOWN = 0x0100;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_KEYUP = 0x0101;
        public const int WM_SYSKEYUP = 0x0105;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_RBUTTONDBLCLK = 0x0206;
        public const int VK_SHIFT = 0x10;
        public const int VK_CAPITAL = 0x14;

        private static HookProc MouseHookProcedure;
        private static HookProc KeyboardHookProcedure;

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

        [DllImport("user32.dll")]
        public static extern int GetKeyState(int nVirtKey);// Long，欲测试的虚拟键键码。对字母、数字字符（A-Z、a-z、0-9），用它们实际的ASCII值

        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[]keyState);

        [DllImport("user32.dll")]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpKeyState, byte[] lpChar, int uFlags);
        #endregion

        public void start()
        {
            this.start(true, true);
        }

        public void start(bool InstallMouseHook, bool InstallKeyboardHook)
        {
            if (hMouseHook == 0 && InstallMouseHook)
            {
                MouseHookProcedure = new HookProc(mouseHookProc);
                hMouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                if (hMouseHook == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    stop(true, false, false);
                    throw new Win32Exception(errorCode);
                }
            }

            if (hKeyboardHook == 0 && InstallKeyboardHook)
            {
                KeyboardHookProcedure = new HookProc(keyboardHookProc);
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                if (hKeyboardHook == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    stop(false, true, false);
                    throw new Win32Exception(errorCode);
                }
            }
        }

        public void stop()
        {
            this.stop(true, true, true);
        }

        public void stop(bool UninstallMouseHook, bool UninstallKeyboardHook, bool ThrowExceptions)
        {
            if (hMouseHook != 0 && UninstallMouseHook)
            {
                bool retMouse = UnhookWindowsHookEx(hMouseHook);
                hMouseHook = 0;
                if (!retMouse && ThrowExceptions)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode);
                }
            }

            if (hKeyboardHook != 0 && UninstallKeyboardHook)
            {
                bool retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
                if (!retKeyboard && ThrowExceptions)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode);
                }
            }
        }

        private int mouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if ((nCode >= 0) && (OnMouseActivity != null))
            {
                MouseLLHookStruct mouseHookStruct = (MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));

                MouseButtons button = MouseButtons.None;
                short mouseDelta = 0;
                switch (wParam)
                {
                    case WM_LBUTTONDOWN:
                        button = MouseButtons.Left;
                        break;
                    case WM_RBUTTONDOWN:
                        button = MouseButtons.Right;
                        break;
                    case WM_MOUSEWHEEL:
                        mouseDelta = (short)((mouseHookStruct.mouseData >> 16) & 0xffff);
                        break;
                }

                int clickCount = 0;
                if (button != MouseButtons.None)
                    if (wParam == WM_LBUTTONDBLCLK || wParam == WM_RBUTTONDBLCLK) clickCount = 2;
                    else clickCount = 1;

                MouseEventArgs e = new MouseEventArgs(
                                                   button,
                                                   clickCount,
                                                   mouseHookStruct.pt.x,
                                                   mouseHookStruct.pt.y,
                                                   mouseDelta);
                OnMouseActivity(this, e);
            }
            return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }

        private int keyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            bool handled = false;
            if ((nCode >= 0) && (KeyDown != null || KeyUp != null || KeyPress != null))
            {
                KeyBoardHookStruct MyKeyboardHookStruct = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
                if (KeyDown != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyDown(this, e);
                    handled = handled || e.Handled;
                }

                if (KeyPress != null && wParam == WM_KEYDOWN)
                {
                    bool isDownShift = ((GetKeyState(VK_SHIFT) & 0x80) == 0x80 ? true : false);
                    bool isDownCapslock = (GetKeyState(VK_CAPITAL) != 0 ? true : false);

                    byte[] keyState = new byte[256];
                    GetKeyboardState(keyState);
                    byte[] inBuffer = new byte[2];
                    if (ToAscii(MyKeyboardHookStruct.vkCode,
                              MyKeyboardHookStruct.scanCode,
                              keyState,
                              inBuffer,
                              MyKeyboardHookStruct.flags) == 1)
                    {
                        char key = (char)inBuffer[0];
                        if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
                        KeyPressEventArgs e = new KeyPressEventArgs(key);
                        KeyPress(this, e);
                        handled = handled || e.Handled;
                    }
                }

                if (KeyUp != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyUp(this, e);
                    handled = handled || e.Handled;
                }

            }

            if (handled)
                return 1;
            else
                return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }
    }
}