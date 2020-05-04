using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Wells.Comm
{
    public class clsTCP
    {
        private TcpClient _TcpClient = null;
        private NetworkStream objNetworkStream;
        public bool m_bConnected = false;

        public bool openConnection(string ip, string port)
        {
            #region 打开连接

            bool ret = true;
            try
            {
                _TcpClient = new TcpClient();
                _TcpClient.SendTimeout = 300;
                _TcpClient.ReceiveTimeout = 300;
                _TcpClient.Connect(IPAddress.Parse(ip), int.Parse(port));
                objNetworkStream = _TcpClient.GetStream();
                m_bConnected = true;
            }
            catch (SocketException exc)
            {
                m_bConnected = false;
                ret = false;
            }
            return ret;

            #endregion
        }

        public string read(string strCmd, int iTimeOut = 1000)
        {
            #region 从PLC读取数据
            string ret = string.Empty;
            try
            {
                string strAddr = strCmd + Environment.NewLine;
                byte[] byteAddr = Encoding.UTF8.GetBytes(strAddr);
                writeData(byteAddr);
                byte[] result = readData(iTimeOut);
                ret = Encoding.UTF8.GetString(result);
            }
            catch (Exception exc)
            {
                ret = string.Empty;
            }
            return ret;
            #endregion
        }

        public string read(int iTimeOut = 1000)
        {
            #region 从PLC读取数据
            string ret = string.Empty;
            try
            {
                byte[] result = readData(iTimeOut);
                ret = Encoding.UTF8.GetString(result);
            }
            catch (Exception exc)
            {
                ret = string.Empty;
            }
            return ret;
            #endregion
        }

        private byte[] readData(int iTimeOut = 1000)
        {
            #region  从接收流读取数据 
            int iTimeout = 0;
            int count = _TcpClient.Available;
            byte[] buffer = null;
            try
            {
                while (count == 0)
                {
                    iTimeout++;
                    if (iTimeout > iTimeOut) break;
                    count = _TcpClient.Available;
                    Thread.Sleep(1);
                }
                buffer = new byte[count];
                if (count > 0)
                {
                    objNetworkStream.Read(buffer, 0, count);
                    objNetworkStream.Flush();
                }
            }
            catch(Exception exc)
            {
                buffer = null;
            }
            return buffer;

            #endregion
        }

        public void write(string strCmd)
        {
            #region  往PLC写str数据
            try
            {
                string strAddr = "";
                strAddr = strCmd + Environment.NewLine;
                byte[] byteAddr = Encoding.UTF8.GetBytes(strAddr);
                writeData(byteAddr);
            }
            catch (Exception exc)
            {
                
            }
            #endregion
        }

        private void writeData(byte[] byteCmd)
        {
            #region 往PLC写byte数据
            try
            {
                byte[] _bCmd = byteCmd;
                objNetworkStream.Write(_bCmd, 0, _bCmd.Length);
                objNetworkStream.Flush();
            }
            catch (Exception exc)
            {

            }
            #endregion
        }
    }
}
