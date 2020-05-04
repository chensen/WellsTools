using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Wells.Tools
{
    //������
    //�����ʱ������ �û�ʹ��Alt+Tab���л����򣬵���Key_Down�¼���ֻ�ܲ���Alt+F4��ϼ������˺ܶ����ϣ�������һ�£�����װ���������Hook���У�����ʵ������ϵͳ��ϼ�
    //ʹ��˵��
    //ʵ����Hook�ࣺHook hk = new Hook();
    //����Hook����:hk.Hook_Start();

    /// <summary>
    /// ���̹���(���μ��̰���)
    /// </summary>
    public class clsHook
    {
        //����Hook�ṹ���� 
        [StructLayout(LayoutKind.Sequential)]
        public class KeyBoardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        //ί�� 
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        private static int hHook = 0;
        public const int WH_KEYBOARD_LL = 13;

        //LowLevel���̽ػ������WH_KEYBOARD��2�������ܶ�ϵͳ���̽�ȡ��Acrobat Reader�������ȡ֮ǰ��ü��̡� 
        public HookProc KeyBoardHookProcedure;

        #region [DllImport("user32.dll")]

        //���ù��� 
        [DllImport("user32.dll")]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //������� 
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll")]
        //������һ������ 
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);
        #endregion

        #region ��װ���̹���

        /// <summary>
        /// ��װ���̹���
        /// </summary>
        public void start()
        {
            if (hHook == 0)
            {
                KeyBoardHookProcedure = new HookProc(keyBoardHookProc);
                hHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyBoardHookProcedure,GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
                //������ù���ʧ��. 
                if (hHook == 0)
                {
                    clear();
                }
            }
        }
        #endregion
        
        #region ȡ�������¼�
        /// <summary>
        /// ȡ�������¼�
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

        #region ���μ���
        /// <summary>
        /// ���μ���
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static int keyBoardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                //����Լ����ж���䣬�������Ҫ��İ������� return 1; 
                //û���ж�ֱ�� return 1;��ô���������а�������ctrl+alt+del
                KeyBoardHookStruct kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
                // ������"WIN"����"Win"
                if ((kbh.vkCode == (int)Keys.LWin) || (kbh.vkCode == (int)Keys.RWin))
                {
                    return 1;
                }
                //����Ctrl+Esc
                if (kbh.vkCode == (int)Keys.Escape && (int)Control.ModifierKeys == (int)Keys.Control)
                {
                    return 1;
                }
                //����Alt+f4 
                if (kbh.vkCode == (int)Keys.F4 && (int)Control.ModifierKeys == (int)Keys.Alt)
                {
                    return 1;
                }
                //����Alt+Esc
                if (kbh.vkCode == (int)Keys.Escape && (int)Control.ModifierKeys == (int)Keys.Alt)
                {
                    return 1;
                }
                //����Alt+Tab 
                if (kbh.vkCode == (int)Keys.Tab && (int)Control.ModifierKeys == (int)Keys.Alt)
                {
                    return 1;
                }
                //�ػ�Ctrl+Shift+Esc 
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
        private IntPtr pKeyboardHook = IntPtr.Zero;//���̹��Ӿ��
        //����ί������
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        private HookProc KeyboardHookProcedure;//���̹���ί��ʵ��������ʡ�Ա���
        public const int idHook = 13;//�ײ���̹���
        //��װ����
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr pInstance, int threadID);
        //ж�ع���
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(IntPtr pHookHandle);

        //���̹��Ӵ�����
        private int keyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            KeyMSG m = (KeyMSG)Marshal.PtrToStructure(lParam, typeof(KeyMSG));//������Ϣ����
            if (pKeyboardHook != IntPtr.Zero)//�жϹ��Ӿ���Ƿ�Ϊ��
            {
                switch (((Keys)m.vkCode))//�жϰ���
                {
                    case Keys.LWin://��������Win��
                    case Keys.RWin://�����Ҳ��Win��
                    case Keys.Delete://Delete��
                    case Keys.Alt://Alt��
                    case Keys.Escape: //Esc��
                    case Keys.F4: //F4��
                    case Keys.Control://Ctrl��
                    case Keys.Tab://Tab��
                        return 1;//��ִ���κβ���
                }
            }
            return 0;
        }
        //��װ���ӷ���
        public bool addHook()
        {
            IntPtr pIn = (IntPtr)4194304;//��4194304ת��Ϊ���
            if (this.pKeyboardHook == IntPtr.Zero)//�����ڹ���ʱ
            {
                //��������
                this.KeyboardHookProcedure = new HookProc(keyboardHookProc);
                //ʹ��SetWindowsHookEx������װ����
                this.pKeyboardHook = SetWindowsHookEx(idHook, KeyboardHookProcedure, pIn, 0);
                if (this.pKeyboardHook == IntPtr.Zero)//�����װ����ʧ��
                {
                    this.deleteHook();//ж�ع���
                    return false;
                }
            }
            return true;
        }
        //ж�ع��ӷ���
        public bool deleteHook()
        {
            bool result = true;
            if (this.pKeyboardHook != IntPtr.Zero)//������ڹ���
            {
                //ʹ��UnhookWindowsHookEx����ж�ع���
                result = (UnhookWindowsHookEx(this.pKeyboardHook) && result);
                this.pKeyboardHook = IntPtr.Zero;//���ָ��
            }
            return result;
        }
        //������Ϣ����ṹ
        [StructLayout(LayoutKind.Sequential)]
        public struct KeyMSG
        {
            public int vkCode;//���̰���
        }
    }
}