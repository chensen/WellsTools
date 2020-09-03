using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Wells.Comm
{
    public class clsRS232
    {
        private SerialPort m_instance;
        public bool m_bIsOpen;
        EventWaitHandle waitHandle = new AutoResetEvent(false);
        Mutex muSend = new Mutex();
        public delegate void DataReceivedDelegate(string str);
        private DataReceivedDelegate m_pDataReceived;
        public string strReturn = string.Empty;
        public int timeOut = 500;

        private byte[] buffer = new byte[200];
        private int buffer_length = 0;
        private string strSendEnd = string.Empty;
        private string strReceivedEnd = string.Empty;
        private byte[] byteSendEnd = null;
        private byte[] byteReceivedEnd = null;
        private int lenSendEnd = 0;
        private int lenReceivedEnd = 0;

        public clsRS232()
        {
            m_bIsOpen = false;
            m_instance = new SerialPort();
            m_instance.DataReceived += new SerialDataReceivedEventHandler(Com_DataReceived);
        }

        public clsRS232(string PortName, int BaudRate, int DataBits, StopBits StopBits, Parity Parity)
        {
            #region 初始化
            m_bIsOpen = false;
            m_instance = new SerialPort();
            m_instance.PortName = PortName;
            m_instance.BaudRate = BaudRate;
            m_instance.DataBits = DataBits;
            m_instance.StopBits = StopBits;
            m_instance.Parity = Parity;
            m_instance.DataReceived += new SerialDataReceivedEventHandler(Com_DataReceived);
            #endregion
        }
        
        public bool openConnection()
        {
            #region 打开串口
            if (!m_bIsOpen)
            {
                try
                {
                    m_instance.Open();
                }
                catch (System.Exception exc)
                {
                    Wells.WellsFramework.WellsMetroMessageBox.Show(null, clsWellsLanguage.getString(101) + exc.Message, clsWellsLanguage.getString(6), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                m_bIsOpen = true;
            }
            return true;
            #endregion
        }

        public bool closeConnection()
        {
            #region 关闭串口
            if (m_bIsOpen)
            {
                try
                {
                    m_instance.Close();
                }
                catch (System.Exception exc)
                {
                    Wells.WellsFramework.WellsMetroMessageBox.Show(null, clsWellsLanguage.getString(102) + exc.Message, clsWellsLanguage.getString(6), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                m_bIsOpen = false;
            }
            return true;
            #endregion
        }

        public void updateConnSetting(int BaudRate, int DataBits, StopBits StopBits, Parity Parity)
        {
            #region 更新串口设置
            closeConnection();
            m_instance.BaudRate = BaudRate;
            m_instance.DataBits = DataBits;
            m_instance.StopBits = StopBits;
            m_instance.Parity = Parity;
            openConnection();
            #endregion
        }

        public void setTimeout(int iTimeout = 500)
        {
            #region 设置读取超时 *****

            timeOut = iTimeout;

            #endregion
        }

        public void setConnEx(string strSendEnd,string strReceivedEnd)
        {
            #region 设置串口通讯起始符和结束符

            this.strSendEnd = strSendEnd;
            this.strReceivedEnd = strReceivedEnd;
            if (strSendEnd != string.Empty)
            {
                this.byteSendEnd = Encoding.UTF8.GetBytes(strSendEnd);
                if (byteSendEnd != null)
                    lenSendEnd = byteSendEnd.Length;
            }
            if (strReceivedEnd != string.Empty)
            {
                this.byteReceivedEnd = Encoding.UTF8.GetBytes(strReceivedEnd);
                if (byteReceivedEnd != null)
                    lenReceivedEnd = byteReceivedEnd.Length;
            }

            #endregion
        }

        public void setConnEx(byte[] byteSendEnd, byte[] byteReceivedEnd)
        {
            #region 设置串口通讯起始符和结束符

            if(byteSendEnd==null)
            {
                this.strSendEnd = string.Empty;
                this.lenSendEnd = 0;
                this.byteSendEnd = null;
            }
            else
            {
                this.strSendEnd = Encoding.UTF8.GetString(byteSendEnd);
                this.lenSendEnd = byteSendEnd.Length;
                this.byteSendEnd = byteSendEnd;
            }

            if(byteReceivedEnd==null)
            {
                this.strReceivedEnd = string.Empty;
                this.lenReceivedEnd = 0;
                this.byteReceivedEnd = null;
            }
            else
            {
                this.strReceivedEnd = Encoding.UTF8.GetString(byteReceivedEnd);
                this.lenReceivedEnd = byteReceivedEnd.Length;
                this.byteReceivedEnd = byteReceivedEnd;
            }

            #endregion
        }

        public void registerReceivedDelegate(DataReceivedDelegate pDataReceived)
        {
            #region 数据处理异步委托
            m_pDataReceived = pDataReceived;
            #endregion
        }

        private void Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            #region 接收事件
            try
            {
                bool bGetSuccessful = true;
                int bytesToRead = m_instance.BytesToRead;
                byte[] array = new byte[bytesToRead];
                m_instance.Read(array, 0, bytesToRead);
                array.CopyTo(buffer, buffer_length);
                buffer_length += bytesToRead;
                if (lenReceivedEnd>0)
                {
                    if (buffer_length >= lenReceivedEnd)
                    {
                        for (int i = 0; i < byteReceivedEnd.Length; i++)
                        {
                            if (byteReceivedEnd[i] != buffer[buffer_length - byteReceivedEnd.Length + i]) { bGetSuccessful = false; break; }
                        }
                    }
                    else
                        bGetSuccessful = false;
                }
                else
                {
                    bGetSuccessful = true;
                }

                if (bGetSuccessful)
                {
                    strReturn = Encoding.UTF8.GetString(buffer);
                    MyTrim(ref strReturn);
                    if (m_pDataReceived != null)
                        m_pDataReceived.BeginInvoke(strReturn, null, null);
                    waitHandle.Set();
                }
            }
            catch (System.Exception exc)
            {
                Wells.WellsFramework.WellsMetroMessageBox.Show(null, clsWellsLanguage.getString(103) + exc.Message, clsWellsLanguage.getString(6), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                waitHandle.Set();
            }
            #endregion
        }

        public string sendCommand(string strCmd,bool bGetReturn)
        {
            #region 发送PLC指令

            muSend.WaitOne();

            string ret = string.Empty;

            try
            {
                
                if (m_bIsOpen)
                {
                    try
                    {
                        buffer = new byte[200];
                        buffer_length = 0;
                        strReturn = string.Empty;

                        if (bGetReturn)
                        {
                            waitHandle.Reset();
                        }

                        strCmd += strSendEnd;
                        byte[] byteCmd = Encoding.UTF8.GetBytes(strCmd);
                        m_instance.Write(byteCmd, 0, byteCmd.Length);
                        if (bGetReturn)
                            waitHandle.WaitOne(timeOut);
                        ret = strReturn;
                    }
                    catch (System.Exception exc)
                    {
                        ret = string.Empty;
                        Wells.FrmType.frm_Log.Log(string.Format(clsWellsLanguage.getString(104), strCmd) + exc.Message, 2);
                    }
                }
            }
            catch(Exception exc)
            {
                
            }
            finally
            {
                muSend.ReleaseMutex();
            }
            return ret;

            #endregion
        }

        public byte[] sendCommand(byte[] byteCmd, bool bGetReturn)
        {
            #region 发送PLC指令

            muSend.WaitOne();

            byte[] ret = null;

            try
            {

                if (m_bIsOpen)
                {
                    try
                    {
                        buffer = new byte[200];
                        buffer_length = 0;
                        strReturn = string.Empty;

                        if (bGetReturn)
                        {
                            waitHandle.Reset();
                        }

                        byte[] tmpCmd = null;
                        if (byteSendEnd != null && byteSendEnd.Length > 0)
                        {
                            tmpCmd = new byte[byteCmd.Length + byteSendEnd.Length];
                            Array.Copy(byteCmd, 0, tmpCmd, 0, byteCmd.Length);
                            Array.Copy(byteSendEnd, 0, tmpCmd, byteCmd.Length, byteSendEnd.Length);
                        }
                        else
                        {
                            tmpCmd = byteCmd;
                        }

                        m_instance.Write(byteCmd, 0, byteCmd.Length);

                        if (bGetReturn)
                            waitHandle.WaitOne(timeOut);

                        ret = new byte[buffer.Length];
                        Array.Copy(buffer, 0, ret, 0, buffer.Length);
                    }
                    catch (System.Exception exc)
                    {
                        ret = null;
                        Wells.FrmType.frm_Log.Log(string.Format(clsWellsLanguage.getString(104), BitConverter.ToString(byteCmd)) + exc.Message, 2);
                    }
                }
            }
            catch (Exception exc)
            {

            }
            finally
            {
                muSend.ReleaseMutex();
            }
            return ret;

            #endregion
        }

        #region 数据转换

        public static byte[] hexStringToByteArray(string s)//wells0045
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
                Wells.WellsFramework.WellsMetroMessageBox.Show(null, clsWellsLanguage.getString(105) + exc.Message, clsWellsLanguage.getString(6), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                result = array;
            }
            return result;
            #endregion
        }

        public static string byteArrayToHexString(byte[] data)//wells0045
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
                Wells.WellsFramework.WellsMetroMessageBox.Show(null, clsWellsLanguage.getString(106) + exc.Message, clsWellsLanguage.getString(6), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                result = data.ToString();
            }
            return result;
            #endregion
        }

        public static string hexToChar(string cmd)//wells0045
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
                Wells.WellsFramework.WellsMetroMessageBox.Show(null, clsWellsLanguage.getString(107) + exc.Message, clsWellsLanguage.getString(6), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                result = string.Empty;
            }
            return result;
            #endregion
        }

        public static void MyTrim(ref string str)
        {
            #region string去除无效字符
            str = str.Replace("\n", "");
            str = str.Replace("\r", "");
            str = str.Replace("\0", "");
            str = str.Replace(" ", "");
            #endregion
        }
        #endregion
    }
}
