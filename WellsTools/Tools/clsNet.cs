using System;
using System.Text;
using System.Net.Sockets;
using System.Net.Mail;
using System.Net;
using System.Runtime.InteropServices;

namespace Wells.Tools
{
    /// <summary>
    /// ���������ص���
    /// </summary>    
    public class clsNet
    {
        #region ��Ȿ���Ƿ�����
        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        /// <summary>
        /// ��Ȿ���Ƿ�����
        /// </summary>
        /// <returns></returns>
        public static bool isConnectedToInternet()
        {
            int i = 0;
            if (InternetGetConnectedState(out i, 0))
            {
                //������
                return true;
            }
            else
            {
                //δ����
                return false;
            }
        }
        #endregion

        #region ������õ�IP��ַ�Ƿ���ȷ��������ȷ��IP��ַ
        /// <summary>
        /// /// ������õ�IP��ַ�Ƿ���ȷ����������ȷ��IP��ַ,��ЧIP��ַ����"-1"��
        /// /// </summary>
        /// /// <param name="ip">���õ�IP��ַ</param>
        public static string getValidIP(string ip)
        {
            string[] tokens = ip.Split('.');
            if(tokens.Length==4)
            {
                return ip;
            }
            else
            {
                return "-1";
            }

        }
        #endregion

        #region ������õĶ˿ں��Ƿ���ȷ��������ȷ�Ķ˿ں�
        /// <summary>
        /// /// ������õĶ˿ں��Ƿ���ȷ����������ȷ�Ķ˿ں�,��Ч�˿ںŷ���-1��
        /// /// </summary>
        /// /// <param name="port">���õĶ˿ں�</param>        
        public static int getValidPort(string port)
        {
            //�������ص���ȷ�˿ں�
            int validPort = -1;
            //��С��Ч�˿ں�
            const int MINPORT = 0;
            //�����Ч�˿ں�
            const int MAXPORT = 65535;

            //���˿ں�
            try
            {
                //����Ķ˿ں�Ϊ�����׳��쳣
                if (port == "")
                {
                    throw new Exception("�˿ںŲ���Ϊ�գ�");
                }

                //���˿ڷ�Χ
                if ((Convert.ToInt32(port) < MINPORT) || (Convert.ToInt32(port) > MAXPORT))
                {
                    throw new Exception("�˿ںŷ�Χ��Ч��");
                }

                //Ϊ�˿ںŸ�ֵ
                validPort = Convert.ToInt32(port);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
            }
            return validPort;
        }
        #endregion

        #region ���ַ�����ʽ��IP��ַת����IPAddress����
        /// <summary>
        /// /// ���ַ�����ʽ��IP��ַת����IPAddress����
        /// /// </summary>
        /// /// <param name="ip">�ַ�����ʽ��IP��ַ</param>        
        public static IPAddress stringToIPAddress(string ip)
        {
            return IPAddress.Parse(ip);
        }
        #endregion

        #region ��ȡ�����ļ������
        /// <summary>
        /// /// ��ȡ�����ļ������
        /// /// </summary>
        public static string LocalHostName
        {
            get
            {
                return Dns.GetHostName();
            }
        }
        #endregion

        #region ��ȡ�����ľ�����IP
        /// <summary>
        /// /// ��ȡ�����ľ�����IP
        /// /// </summary>        
        public static string LANIP
        {
            get
            {
                //��ȡ������IP�б�,IP�б��еĵ�һ���Ǿ�����IP���ڶ����ǹ�����IP
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                //�������IP�б�Ϊ�գ��򷵻ؿ��ַ���
                if (addressList.Length < 1)
                {
                    return "";
                }

                //���ر����ľ�����IP
                return addressList[0].ToString();
            }
        }
        #endregion

        #region ��ȡ������Internet����Ĺ�����IP
        /// <summary>
        /// /// ��ȡ������Internet����Ĺ�����IP
        /// 
        /// /// </summary>        
        public static string WANIP
        {
            get
            {
                //��ȡ������IP�б�,IP�б��еĵ�һ���Ǿ�����IP���ڶ����ǹ�����IP
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                //�������IP�б�С��2���򷵻ؿ��ַ���
                if (addressList.Length < 2)
                {
                    return "";
                }

                //���ر����Ĺ�����IP
                return addressList[1].ToString();
            }
        }
        #endregion

        #region ��ȡԶ�̿ͻ�����IP��ַ
        /// <summary>
        /// /// ��ȡԶ�̿ͻ�����IP��ַ
        /// /// </summary>
        /// /// <param name="clientSocket">�ͻ��˵�socket����</param>        
        public static string getClientIP(Socket clientSocket)
        {
            IPEndPoint client = (IPEndPoint)clientSocket.RemoteEndPoint;
            return client.Address.ToString();
        }
        #endregion

        #region ����һ��IPEndPoint����
        /// <summary>
        /// /// ����һ��IPEndPoint����
        /// /// </summary>
        /// /// <param name="ip">IP��ַ</param>
        /// /// <param name="port">�˿ں�</param>        
        public static IPEndPoint createIPEndPoint(string ip, int port)
        {
            IPAddress ipAddress = stringToIPAddress(ip);
            return new IPEndPoint(ipAddress, port);
        }
        #endregion

        #region ����һ��TcpListener����
        /// <summary>
        /// /// ����һ���Զ�����IP�Ͷ˿ڵ�TcpListener����
        /// /// </summary>        
        public static TcpListener createTcpListener()
        {
            //����һ���Զ����������ڵ�
            IPAddress ipAddress = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 0);

            return new TcpListener(localEndPoint);
        }
        /// <summary>
        /// ����һ��TcpListener����
        /// </summary>
        /// <param name="ip">IP��ַ</param>
        /// <param name="port">�˿�</param>        
        public static TcpListener createTcpListener(string ip, int port)
        {
            //����һ������ڵ�
            IPAddress ipAddress = stringToIPAddress(ip);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            return new TcpListener(localEndPoint);
        }
        #endregion

        #region ����һ������TCPЭ���Socket����
        /// <summary>
        /// /// ����һ������TCPЭ���Socket����
        /// /// </summary>        
        public static Socket createTcpSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        #endregion

        #region ����һ������UDPЭ���Socket����
        /// <summary>
        /// /// ����һ������UDPЭ���Socket����
        /// /// </summary>        
        public static Socket createUdpSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
        #endregion

        #region ��ȡ�����ս��

        #region ��ȡTcpListener����ı����ս��
        /// <summary>
        /// /// ��ȡTcpListener����ı����ս��
        /// /// </summary>
        /// /// <param name="tcpListener">TcpListener����</param>        
        public static IPEndPoint getLocalPoint(TcpListener tcpListener)
        {
            return (IPEndPoint)tcpListener.LocalEndpoint;
        }
        /// <summary>
        /// ��ȡTcpListener����ı����ս���IP��ַ
        /// </summary>
        /// <param name="tcpListener">TcpListener����</param>        
        public static string getLocalPoint_IP(TcpListener tcpListener)
        {
            IPEndPoint localEndPoint = (IPEndPoint)tcpListener.LocalEndpoint;
            return localEndPoint.Address.ToString();
        }
        /// <summary>
        /// ��ȡTcpListener����ı����ս��Ķ˿ں�
        /// </summary>
        /// <param name="tcpListener">TcpListener����</param>        
        public static int getLocalPoint_Port(TcpListener tcpListener)
        {
            IPEndPoint localEndPoint = (IPEndPoint)tcpListener.LocalEndpoint;
            return localEndPoint.Port;
        }
        #endregion

        #region ��ȡSocket����ı����ս��
        /// <summary>
        /// /// ��ȡSocket����ı����ս��
        /// /// </summary>
        /// /// <param name="socket">Socket����</param>        
        public static IPEndPoint getLocalPoint(Socket socket)
        {
            return (IPEndPoint)socket.LocalEndPoint;
        }
        /// <summary>
        /// ��ȡSocket����ı����ս���IP��ַ
        /// </summary>
        /// <param name="socket">Socket����</param>        
        public static string getLocalPoint_IP(Socket socket)
        {
            IPEndPoint localEndPoint = (IPEndPoint)socket.LocalEndPoint;
            return localEndPoint.Address.ToString();
        }
        /// <summary>
        /// ��ȡSocket����ı����ս��Ķ˿ں�
        /// </summary>
        /// <param name="socket">Socket����</param>        
        public static int getLocalPoint_Port(Socket socket)
        {
            IPEndPoint localEndPoint = (IPEndPoint)socket.LocalEndPoint;
            return localEndPoint.Port;
        }
        #endregion

        #endregion

        #region ���ս��
        /// <summary>
        /// /// ���ս��
        /// /// </summary>
        /// /// <param name="socket">Socket����</param>
        /// /// <param name="endPoint">Ҫ�󶨵��ս��</param>
        public static void bindEndPoint(Socket socket, IPEndPoint endPoint)
        {
            if (!socket.IsBound)
            {
                socket.Bind(endPoint);
            }
        }
        /// <summary>
        /// ���ս��
        /// </summary>
        /// <param name="socket">Socket����</param>        
        /// <param name="ip">������IP��ַ</param>
        /// <param name="port">�������˿�</param>
        public static void bindEndPoint(Socket socket, string ip, int port)
        {
            //�����ս��
            IPEndPoint endPoint = createIPEndPoint(ip, port);

            //���ս��
            if (!socket.IsBound)
            {
                socket.Bind(endPoint);
            }
        }
        #endregion

        #region ָ��Socket����ִ�м���
        /// <summary>
        /// /// ָ��Socket����ִ�м�����Ĭ��������������������Ϊ100
        /// /// </summary>
        /// /// <param name="socket">ִ�м�����Socket����</param>
        /// /// <param name="port">�����Ķ˿ں�</param>
        public static void startListen(Socket socket, int port)
        {
            //���������ս��
            IPEndPoint localPoint = createIPEndPoint(clsNet.LocalHostName, port);

            //�󶨵������ս��
            bindEndPoint(socket, localPoint);

            //��ʼ����
            socket.Listen(100);
        }
        /// <summary>
        /// ָ��Socket����ִ�м���
        /// </summary>
        /// <param name="socket">ִ�м�����Socket����</param>
        /// <param name="port">�����Ķ˿ں�</param>
        /// <param name="maxConnection">������������������</param>
        public static void startListen(Socket socket, int port, int maxConnection)
        {
            //���������ս��
            IPEndPoint localPoint = createIPEndPoint(clsNet.LocalHostName, port);

            //�󶨵������ս��
            bindEndPoint(socket, localPoint);

            //��ʼ����
            socket.Listen(maxConnection);
        }
        /// <summary>
        /// ָ��Socket����ִ�м���
        /// </summary>
        /// <param name="socket">ִ�м�����Socket����</param>
        /// <param name="ip">������IP��ַ</param>
        /// <param name="port">�����Ķ˿ں�</param>
        /// <param name="maxConnection">������������������</param>
        public static void startListen(Socket socket, string ip, int port, int maxConnection)
        {
            //�󶨵������ս��
            bindEndPoint(socket, ip, port);

            //��ʼ����
            socket.Listen(maxConnection);
        }
        #endregion

        #region ���ӵ�����TCPЭ��ķ�����
        /// <summary>
        /// /// ���ӵ�����TCPЭ��ķ�����,���ӳɹ�����true�����򷵻�false
        /// /// </summary>
        /// /// <param name="socket">Socket����</param>
        /// /// <param name="ip">������IP��ַ</param>
        /// /// <param name="port">�������˿ں�</param>     
        public static bool connect(Socket socket, string ip, int port)
        {
            try
            {
                //���ӷ�����
                socket.Connect(ip, port);

                //�������״̬
                return socket.Poll(-1, SelectMode.SelectWrite);
            }
            catch (SocketException ex)
            {
                throw new Exception(ex.Message);
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
            }
        }
        #endregion

        #region ��ͬ����ʽ������Ϣ
        /// <summary>
        /// /// ��ͬ����ʽ��ָ����Socket��������Ϣ
        /// /// </summary>
        /// /// <param name="socket">socket����</param>
        /// /// <param name="msg">���͵���Ϣ</param>
        public static void sendMsg(Socket socket, byte[] msg)
        {
            //������Ϣ
            socket.Send(msg, msg.Length, SocketFlags.None);
        }
        /// <summary>
        /// ʹ��UTF8�����ʽ��ͬ����ʽ��ָ����Socket��������Ϣ
        /// </summary>
        /// <param name="socket">socket����</param>
        /// <param name="msg">���͵���Ϣ</param>
        public static void sendMsg(Socket socket, string msg)
        {
            //���ַ�����Ϣת�����ַ�����
            byte[] buffer = clsConvert.StringToBytes(msg, Encoding.Default);

            //������Ϣ
            socket.Send(buffer, buffer.Length, SocketFlags.None);
        }
        #endregion

        #region ��ͬ����ʽ������Ϣ
        /// <summary>
        /// /// ��ͬ����ʽ������Ϣ
        /// /// </summary>
        /// /// <param name="socket">socket����</param>
        /// /// <param name="buffer">������Ϣ�Ļ�����</param>
        public static void receiveMsg(Socket socket, byte[] buffer)
        {
            socket.Receive(buffer);
        }
        /// <summary>
        /// ��ͬ����ʽ������Ϣ����ת��ΪUTF8�����ʽ���ַ���,ʹ��5000�ֽڵ�Ĭ�ϻ��������ա�
        /// </summary>
        /// <param name="socket">socket����</param>        
        public static string receiveMsg(Socket socket)
        {
            //������ջ�����
            byte[] buffer = new byte[5000];
            //�������ݣ���ȡ���յ����ֽ���
            int receiveCount = socket.Receive(buffer);

            //������ʱ������
            byte[] tempBuffer = new byte[receiveCount];
            //�����յ�������д����ʱ������
            Buffer.BlockCopy(buffer, 0, tempBuffer, 0, receiveCount);
            //ת�����ַ����������䷵��
            return clsConvert.BytesToString(tempBuffer, Encoding.Default);
        }
        #endregion

        #region �رջ���TcpЭ���Socket����
        /// <summary>
        /// /// �رջ���TcpЭ���Socket����
        /// /// </summary>
        /// /// <param name="socket">Ҫ�رյ�Socket����</param>
        public static void close(Socket socket)
        {
            try
            {
                //��ֹSocket������պͷ�������
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException ex)
            {
                throw ex;
            }
            finally
            {
                //�ر�Socket����
                socket.Close();
            }
        }
        #endregion

        #region ���͵����ʼ�
        /// <summary>
        /// /// ���͵����ʼ�,����SMTP������Ϣ����config�����ļ���system.net������.
        /// /// </summary>
        /// /// <param name="receiveEmail">���յ����ʼ��ĵ�ַ</param>
        /// /// <param name="msgSubject">�����ʼ��ı���</param>
        /// /// <param name="msgBody">�����ʼ�������</param>
        /// /// <param name="IsEnableSSL">�Ƿ���SSL</param>
        public static bool sendEmail(string receiveEmail, string msgSubject, string msgBody, bool IsEnableSSL)
        {
            //���������ʼ�����
            MailMessage email = new MailMessage();
            //���ý����˵ĵ����ʼ���ַ
            email.To.Add(receiveEmail);
            //�����ʼ��ı���
            email.Subject = msgSubject;
            //�����ʼ�������
            email.Body = msgBody;
            //�����ʼ�ΪHTML��ʽ
            email.IsBodyHtml = true;

            //����SMTP�ͻ��ˣ����Զ��������ļ��л�ȡSMTP��������Ϣ
            SmtpClient smtp = new SmtpClient();
            //����SSL
            smtp.EnableSsl = IsEnableSSL;

            try
            {
                //���͵����ʼ�
                smtp.Send(email);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}