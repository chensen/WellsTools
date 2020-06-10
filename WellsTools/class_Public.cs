using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Wells
{
    public class class_Public
    {
        /// <summary>
        /// 判断是否处于管理员运行
        /// </summary>
        /// <returns></returns>
        public static bool isRunAdmin()        {            WindowsIdentity current = WindowsIdentity.GetCurrent();            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
            //WindowsBuiltInRole可以枚举出很多权限，例如系统用户、User、Guest等等
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);        }

        public static DialogResult Show(IWin32Window owner, String message)
        { return Show(owner, message, "Notification"); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title)
        { return Show(owner, message, title, MessageBoxButtons.OK); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons)
        { return Show(owner, message, title, buttons, MessageBoxIcon.None); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons, MessageBoxIcon icon)
        { return Show(owner, message, title, buttons, icon, MessageBoxDefaultButton.Button1); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <param name="defaultbutton"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultbutton)
        {
            return Wells.WellsFramework.WellsMetroMessageBox.Show(owner, message, title, buttons, icon, defaultbutton);
        }
        
        public static void closeMsgForm()
        {
            Wells.WellsFramework.WellsMetroMessageBox.CloseMsgForm();
        }

        public static void showTips(Form form, string strMsg, MessageBoxIcon icon = MessageBoxIcon.Information, int intAutoColseTime = 3000)
        {
            Wells.WellsMetroControl.Forms.TipsState status = WellsMetroControl.Forms.TipsState.Default;
            if (icon == MessageBoxIcon.Error) status = WellsMetroControl.Forms.TipsState.Error;
            else if (icon == MessageBoxIcon.Warning) status = WellsMetroControl.Forms.TipsState.Warning;
            else if (icon == MessageBoxIcon.None) status = WellsMetroControl.Forms.TipsState.Success;
            else if (icon == MessageBoxIcon.Information) status = WellsMetroControl.Forms.TipsState.Info;

            Wells.WellsMetroControl.Forms.FrmTips.ShowTips(form, strMsg, 
                intAutoColseTime, false, 
                System.Drawing.ContentAlignment.BottomCenter, null, 
                Wells.WellsMetroControl.Forms.TipsSizeMode.Large, null, 
                status);
        }

        public static void clearTips()
        {
            Wells.WellsMetroControl.Forms.FrmTips.ClearTips();
        }

        public static string[] openFiles(bool bMultiselect, bool isBmp, string initDir = "")
        {
            string[] ret = null;

            OpenFileDialog dlg = new OpenFileDialog();
            
            try
            {
                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;
                dlg.Multiselect = bMultiselect;
                if (initDir != string.Empty) dlg.InitialDirectory = initDir;
                if (isBmp) dlg.Filter = "Picture Files|*.jpg;*.jpeg;*.png;*.bmp;*.tif";
                else dlg.Filter = "All files（*.*）|*.*|All files(*.*)|*.* ";

                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    string[] filenames = dlg.FileNames;
                    if (filenames != null && filenames.Length > 0)
                        return filenames;
                }

            }
            catch (System.Exception exc)
            {
                ret = null;
                Wells.FrmType.frm_Log.Log(clsWellsLanguage.getString(120) + exc.Message, 2);
            }

            return ret;
        }

        public static string getInputInfo(string strCaption,string strLabel,string strInput,bool bPsd)
        {
            Wells.FrmType.frmInput dlg = new Wells.FrmType.frmInput();
            dlg.initForm(strCaption, strLabel, strInput, bPsd);
            dlg.ShowDialog();
            return dlg.strInputText;
        }

        public static List<string> readTxt2StrinList(string strFilePath, Encoding coding = null)
        {
            List<string> list = new List<string>();
            try
            {
                if (File.Exists(strFilePath))
                {
                    FileStream stream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
                    StreamReader streamReader = new StreamReader(stream, coding == null ? Encoding.Default : coding);
                    while (streamReader.Peek() != -1)
                    {
                        string item = streamReader.ReadLine();
                        list.Add(item);
                    }
                    streamReader.Close();
                }
            }
            catch (Exception exc)
            {
                list = new List<string>();
                Wells.FrmType.frm_Log.Log("readTxt2StrinList:" + exc.Message, 2);
            }
            return list;
        }

        public static void writeTxtFile(string strPath, FileMode filemode, List<string> list, Encoding coding = null)
        {
            new Thread(delegate ()
            {
                try
                {
                    FileStream stream = new FileStream(strPath, filemode, FileAccess.Write);
                    StreamWriter streamWriter = new StreamWriter(stream, coding == null ? Encoding.Default : coding);
                    for (int i = 0; i < list.Count; i++)
                    {
                        streamWriter.WriteLine(list[i]);
                    }
                    streamWriter.Close();
                }
                catch (System.Exception exc)
                {
                    Wells.FrmType.frm_Log.Log("writeTxtFile:" + exc.Message, 2);
                }
            }).Start();
        }

        public static byte[] fnHexStringToByteArray(string s)
        {
            #region hex转换byteArray
            byte[] array = new byte[1];
            byte[] result = null;
            try
            {
                s = s.Replace(" ", "");
                array = new byte[s.Length / 2];
                for (int i = 0; i < s.Length; i += 2)
                {
                    array[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
                }
                result = array;
            }
            catch (Exception exc)
            {
                Wells.FrmType.frm_Log.Log("fnHexStringToByteArray:" + exc.Message, 2, 0);
                result = array;
            }
            return result;
            #endregion
        }

        public static string fnByteArrayToHexString(byte[] data)//wells0045
        {
            #region byteArray转换hex
            string result = string.Empty;
            try
            {
                StringBuilder stringBuilder = new StringBuilder(data.Length * 3);
                for (int i = 0; i < data.Length; i++)
                {
                    byte value = data[i];
                    stringBuilder.Append(Convert.ToString(value, 16).PadLeft(2, '0').PadRight(3, ' '));
                }
                result = stringBuilder.ToString().ToUpper();
            }
            catch (Exception exc)
            {
                Wells.FrmType.frm_Log.Log("fnByteArrayToHexString:" + exc.Message, 2, 0);
                result = data.ToString();
            }
            return result;
            #endregion
        }

        public static string fnHexToChar(string cmd)//wells0045
        {
            #region hex转换char
            string result = string.Empty;
            try
            {
                string[] array = cmd.Split(' ');
                cmd = "";
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string value = array2[i];
                    int utf = Convert.ToInt32(value, 16);
                    string str = char.ConvertFromUtf32(utf);
                    cmd += str;
                }
                result = cmd;
            }
            catch (Exception exc)
            {
                Wells.FrmType.frm_Log.Log("fnHexToChar:" + exc.Message, 2, 0);
                result = string.Empty;
            }
            return result;
            #endregion
        }

        public static string transArray2String(string[] list, string strSpilt = "#", string strHead1 = "", bool useIndex = false, string strHead2 = "", string strTail = "")
        {
            string text = "";
            for (int i = 0; i < list.Length; i++)
            {
                if (useIndex)
                    text += strHead1 + i.ToString() + strHead2 + list[i] + strTail;
                else
                    text += strHead1 + strHead2 + list[i] + strTail;
                if (i < list.Length - 1)
                {
                    text += strSpilt;
                }
            }
            return text;
        }

        public static string transArray2String(long[] list, string strSpilt = "#", string strHead1 = "", bool useIndex = false, string strHead2 = "", string strTail = "")
        {
            string text = "";
            for (int i = 0; i < list.Length; i++)
            {
                if (useIndex)
                    text += strHead1 + i.ToString() + strHead2 + list[i].ToString() + strTail;
                else
                    text += strHead1 + strHead2 + list[i].ToString() + strTail;
                if (i < list.Length - 1)
                {
                    text += strSpilt;
                }
            }
            return text;
        }

        public static string transArray2String(List<string> list, string strSpilt = "#", string strHead1 = "", bool useIndex = false, string strHead2 = "", string strTail = "")
        {
            string text = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (useIndex)
                    text += strHead1 + i.ToString() + strHead2 + list[i] + strTail;
                else
                    text += strHead1 + strHead2 + list[i] + strTail;
                if (i < list.Count - 1)
                {
                    text += strSpilt;
                }
            }
            return text;
        }
    }
}
