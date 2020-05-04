using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Sockets;

namespace Wells.Tools
{
    /// <summary>
    /// FTP帮助类
    /// </summary>
    public class clsFtp
    {
        #region 字段
        string ftpURI;
        string ftpUserID;
        string ftpServerIP;
        string ftpPassword;
        string ftpRemotePath;
        #endregion

        /// <summary>  
        /// 连接FTP服务器
        /// </summary>  
        /// <param name="FtpServerIP">FTP连接地址</param>  
        /// <param name="FtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>  
        /// <param name="FtpUserID">用户名</param>  
        /// <param name="FtpPassword">密码</param>  
        public clsFtp(string FtpServerIP, string FtpRemotePath, string FtpUserID, string FtpPassword)
        {
            ftpServerIP = FtpServerIP;
            ftpRemotePath = FtpRemotePath;
            ftpUserID = FtpUserID;
            ftpPassword = FtpPassword;
            ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
        }

        /// <summary>  
        /// 上传  
        /// </summary>   
        public void upload(string filename)
        {
            FileInfo fileInf = new FileInfo(filename);
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileInf.Name));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.KeepAlive = false;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>  
        /// 下载  
        /// </summary>   
        public void download(string filePath, string fileName)
        {
            try
            {
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>  
        /// 删除文件  
        /// </summary>  
        public void delete(string fileName)
        {
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFTP.KeepAlive = false;
                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>  
        /// 获取当前目录下明细(包含文件和文件夹)  
        /// </summary>  
        public string[] getFilesDetailList()
        {
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI));
                ftp.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                line = reader.ReadLine();
                line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf("\n"), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>  
        /// 获取FTP文件列表(包括文件夹)
        /// </summary>   
        private string[] getAllList(string url)
        {
            List<string> list = new List<string>();
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(new Uri(url));
            req.Credentials = new NetworkCredential(ftpPassword, ftpPassword);
            req.Method = WebRequestMethods.Ftp.ListDirectory;
            req.UseBinary = true;
            req.UsePassive = true;
            try
            {
                using (FtpWebResponse res = (FtpWebResponse)req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream()))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            list.Add(s);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return list.ToArray();
        }

        /// <summary>  
        /// 获取当前目录下文件列表(不包括文件夹)  
        /// </summary>  
        public string[] getFileList(string url)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpPassword, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {

                    if (line.IndexOf("<DIR>") == -1)
                    {
                        result.Append(Regex.Match(line, @"[\S]+ [\S]+", RegexOptions.IgnoreCase).Value.Split(' ')[1]);
                        result.Append("\n");
                    }
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return result.ToString().Split('\n');
        }

        /// <summary>  
        /// 判断当前目录下指定的文件是否存在  
        /// </summary>  
        /// <param name="RemoteFileName">远程文件名</param>  
        public bool fileExist(string RemoteFileName)
        {
            string[] fileList = getFileList("*.*");
            foreach (string str in fileList)
            {
                if (str.Trim() == RemoteFileName.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>  
        /// 创建文件夹  
        /// </summary>   
        public void makeDir(string dirName)
        {
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + dirName));
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            { }
        }

        /// <summary>  
        /// 获取指定文件大小  
        /// </summary>  
        public long getFileSize(string filename)
        {
            FtpWebRequest reqFTP;
            long fileSize = 0;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + filename));
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                fileSize = response.ContentLength;
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            { }
            return fileSize;
        }

        /// <summary>  
        /// 更改文件名  
        /// </summary> 
        public void reName(string currentFilename, string newFilename)
        {
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + currentFilename));
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            { }
        }

        /// <summary>  
        /// 移动文件  
        /// </summary>  
        public void movieFile(string currentFilename, string newDirectory)
        {
            reName(currentFilename, newDirectory);
        }

        /// <summary>  
        /// 切换当前目录  
        /// </summary>  
        /// <param name="IsRoot">true:绝对路径 false:相对路径</param>   
        public void gotoDirectory(string DirectoryName, bool IsRoot)
        {
            if (IsRoot)
            {
                ftpRemotePath = DirectoryName;
            }
            else
            {
                ftpRemotePath += DirectoryName + "/";
            }
            ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
        }
    }

    /// <summary>
    /// FTP操作类
    /// </summary>
    public class clsFtpOperater
    {
        #region 属性
        private clsFtpClient ftp;
        /// <summary>
        /// 全局FTP访问变量
        /// </summary>
        public clsFtpClient Ftp
        {
            get { return ftp; }
            set { ftp = value; }
        }

        private string _server;
        /// <summary>
        /// Ftp服务器
        /// </summary>
        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        private string _User;
        /// <summary>
        /// Ftp用户
        /// </summary>
        public string User
        {
            get { return _User; }
            set { _User = value; }
        }

        private string _Pass;
        /// <summary>
        /// Ftp密码
        /// </summary>
        public string Pass
        {
            get { return _Pass; }
            set { _Pass = value; }
        }

        private string _FolderZJ;
        /// <summary>
        /// Ftp密码
        /// </summary>
        public string FolderZJ
        {
            get { return _FolderZJ; }
            set { _FolderZJ = value; }
        }

        private string _FolderWX;
        /// <summary>
        /// Ftp密码
        /// </summary>
        public string FolderWX
        {
            get { return _FolderWX; }
            set { _FolderWX = value; }
        }
        #endregion

        /// <summary>
        /// 得到文件列表
        /// </summary>
        /// <returns></returns>
        public string[] GetList(string strPath)
        {
            if (ftp == null) ftp = this.getFtpClient();
            ftp.connect();
            ftp.changeDir(strPath);
            return ftp.dir("*");
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="ftpFolder">ftp目录</param>
        /// <param name="ftpFileName">ftp文件名</param>
        /// <param name="localFolder">本地目录</param>
        /// <param name="localFileName">本地文件名</param>
        public bool getFile(string ftpFolder, string ftpFileName, string localFolder, string localFileName)
        {
            try
            {
                if (ftp == null) ftp = this.getFtpClient();
                if (!ftp.Connected)
                {
                    ftp.connect();
                    ftp.changeDir(ftpFolder);
                }
                ftp.get(ftpFileName, localFolder, localFileName);

                return true;
            }
            catch
            {
                try
                {
                    ftp.disConnect();
                    ftp = null;
                }
                catch { ftp = null; }
                return false;
            }
        }

        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="ftpFolder">本地目录</param>
        /// <param name="ftpFileName">本地文件名temp</param>
        /// <param name="localFolder">本地目录</param>
        /// <param name="localFileName">本地文件名</param>
        public bool addMSCFile(string ftpFolder, string ftpFileName, string localFolder, string localFileName, string BscInfo)
        {
            string sLine = "";
            string sResult = "";
            string path = "获得应用程序所在的完整的路径";
            path = path.Substring(0, path.LastIndexOf("\\"));
            try
            {
                FileStream fsFile = new FileStream(ftpFolder + "\\" + ftpFileName, FileMode.Open);
                FileStream fsFileWrite = new FileStream(localFolder + "\\" + localFileName, FileMode.Create);
                StreamReader sr = new StreamReader(fsFile);
                StreamWriter sw = new StreamWriter(fsFileWrite);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                while (sr.Peek() > -1)
                {
                    sLine = sr.ReadToEnd();
                }
                string[] arStr = sLine.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < arStr.Length - 1; i++)
                {
                    sResult += BscInfo + "," + arStr[i].Trim() + "\n";
                }
                sr.Close();
                byte[] connect = new UTF8Encoding(true).GetBytes(sResult);
                fsFileWrite.Write(connect, 0, connect.Length);
                fsFileWrite.Flush();
                sw.Close();
                fsFile.Close();
                fsFileWrite.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="ftpFolder">ftp目录</param>
        /// <param name="ftpFileName">ftp文件名</param>
        public bool deleteFile(string ftpFolder, string ftpFileName)
        {
            try
            {
                if (ftp == null) ftp = this.getFtpClient();
                if (!ftp.Connected)
                {
                    ftp.connect();
                    ftp.changeDir(ftpFolder);
                }
                ftp.delete(ftpFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="ftpFolder">ftp目录</param>
        /// <param name="ftpFileName">ftp文件名</param>
        public bool putFile(string ftpFolder, string ftpFileName)
        {
            try
            {
                if (ftp == null) ftp = this.getFtpClient();
                if (!ftp.Connected)
                {
                    ftp.connect();
                    ftp.changeDir(ftpFolder);
                }
                ftp.put(ftpFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="ftpFolder">ftp目录</param>
        /// <param name="ftpFileName">ftp文件名</param>
        /// <param name="localFolder">本地目录</param>
        /// <param name="localFileName">本地文件名</param>
        public bool getFileNoBinary(string ftpFolder, string ftpFileName, string localFolder, string localFileName)
        {
            try
            {
                if (ftp == null) ftp = this.getFtpClient();
                if (!ftp.Connected)
                {
                    ftp.connect();
                    ftp.changeDir(ftpFolder);
                }
                ftp.getNoBinary(ftpFileName, localFolder, localFileName);
                return true;
            }
            catch
            {
                try
                {
                    ftp.disConnect();
                    ftp = null;
                }
                catch
                {
                    ftp = null;
                }
                return false;
            }
        }

        /// <summary>
        /// 得到FTP上文件信息
        /// </summary>
        /// <param name="ftpFolder">FTP目录</param>
        /// <param name="ftpFileName">ftp文件名</param>
        public string getFileInfo(string ftpFolder, string ftpFileName)
        {
            string strResult = "";
            try
            {
                if (ftp == null) ftp = this.getFtpClient();
                if (ftp.Connected) ftp.disConnect();
                ftp.connect();
                ftp.changeDir(ftpFolder);
                strResult = ftp.getFileInfo(ftpFileName);
                return strResult;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 测试FTP服务器是否可登陆
        /// </summary>
        public bool canConnect()
        {
            if (ftp == null) ftp = this.getFtpClient();
            try
            {
                ftp.connect();
                ftp.disConnect();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 得到FTP上文件信息
        /// </summary>
        /// <param name="ftpFolder">FTP目录</param>
        /// <param name="ftpFileName">ftp文件名</param>
        public string getFileInfoConnected(string ftpFolder, string ftpFileName)
        {
            string strResult = "";
            try
            {
                if (ftp == null) ftp = this.getFtpClient();
                if (!ftp.Connected)
                {
                    ftp.connect();
                    ftp.changeDir(ftpFolder);
                }
                strResult = ftp.getFileInfo(ftpFileName);
                return strResult;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 得到文件列表
        /// </summary>
        /// <param name="ftpFolder">FTP目录</param>
        /// <returns>FTP通配符号</returns>
        public string[] getFileList(string ftpFolder, string strMask)
        {
            string[] strResult;
            try
            {
                if (ftp == null) ftp = this.getFtpClient();
                if (!ftp.Connected)
                {
                    ftp.connect();
                    ftp.changeDir(ftpFolder);
                }
                strResult = ftp.dir(strMask);
                return strResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///得到FTP传输对象
        /// </summary>
        public clsFtpClient getFtpClient()
        {
            clsFtpClient ft = new clsFtpClient();
            ft.RemoteHost = this.Server;
            ft.RemoteUser = this.User;
            ft.RemotePass = this.Pass;
            return ft;
        }
    }

    /// <summary>
    /// FTP 操作类客户端
    /// </summary>
    public class clsFtpClient
    {
        public static object obj = new object();

        #region 构造函数
        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public clsFtpClient()
        {
            strRemoteHost = "";
            strRemotePath = "";
            strRemoteUser = "";
            strRemotePass = "";
            strRemotePort = 21;
            bConnected = false;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public clsFtpClient(string remoteHost, string remotePath, string remoteUser, string remotePass, int remotePort)
        {
            strRemoteHost = remoteHost;
            strRemotePath = remotePath;
            strRemoteUser = remoteUser;
            strRemotePass = remotePass;
            strRemotePort = remotePort;
            connect();
        }
        #endregion

        #region 字段
        private int strRemotePort;
        private Boolean bConnected;
        private string strRemoteHost;
        private string strRemotePass;
        private string strRemoteUser;
        private string strRemotePath;

        /// <summary>
        /// 服务器返回的应答信息(包含应答码)
        /// </summary>
        private string strMsg;
        /// <summary>
        /// 服务器返回的应答信息(包含应答码)
        /// </summary>
        private string strReply;
        /// <summary>
        /// 服务器返回的应答码
        /// </summary>
        private int iReplyCode;
        /// <summary>
        /// 进行控制连接的socket
        /// </summary>
        private Socket socketControl;
        /// <summary>
        /// 传输模式
        /// </summary>
        private TransferType trType;
        /// <summary>
        /// 接收和发送数据的缓冲区
        /// </summary>
        private static int BLOCK_SIZE = 512;
        /// <summary>
        /// 编码方式
        /// </summary>
        Encoding ASCII = Encoding.ASCII;
        /// <summary>
        /// 字节数组
        /// </summary>
        Byte[] buffer = new Byte[BLOCK_SIZE];
        #endregion

        #region 属性
        /// <summary>
        /// FTP服务器IP地址
        /// </summary>
        public string RemoteHost
        {
            get
            {
                return strRemoteHost;
            }
            set
            {
                strRemoteHost = value;
            }
        }

        /// <summary>
        /// FTP服务器端口
        /// </summary>
        public int RemotePort
        {
            get
            {
                return strRemotePort;
            }
            set
            {
                strRemotePort = value;
            }
        }

        /// <summary>
        /// 当前服务器目录
        /// </summary>
        public string RemotePath
        {
            get
            {
                return strRemotePath;
            }
            set
            {
                strRemotePath = value;
            }
        }

        /// <summary>
        /// 登录用户账号
        /// </summary>
        public string RemoteUser
        {
            set
            {
                strRemoteUser = value;
            }
        }

        /// <summary>
        /// 用户登录密码
        /// </summary>
        public string RemotePass
        {
            set
            {
                strRemotePass = value;
            }
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool Connected
        {
            get
            {
                return bConnected;
            }
        }
        #endregion

        #region 链接
        /// <summary>
        /// 建立连接 
        /// </summary>
        public void connect()
        {
            lock (obj)
            {
                socketControl = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(RemoteHost), strRemotePort);
                try
                {
                    socketControl.Connect(ep);
                }
                catch (Exception)
                {
                    throw new IOException("不能连接ftp服务器");
                }
            }
            readReply();
            if (iReplyCode != 220)
            {
                disConnect();
                throw new IOException(strReply.Substring(4));
            }
            sendCommand("USER " + strRemoteUser);
            if (!(iReplyCode == 331 || iReplyCode == 230))
            {
                closeSocketConnect();
                throw new IOException(strReply.Substring(4));
            }
            if (iReplyCode != 230)
            {
                sendCommand("PASS " + strRemotePass);
                if (!(iReplyCode == 230 || iReplyCode == 202))
                {
                    closeSocketConnect();
                    throw new IOException(strReply.Substring(4));
                }
            }
            bConnected = true;
            changeDir(strRemotePath);
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void disConnect()
        {
            if (socketControl != null)
            {
                sendCommand("QUIT");
            }
            closeSocketConnect();
        }
        #endregion

        #region 传输模式
        /// <summary>
        /// 传输模式:二进制类型、ASCII类型
        /// </summary>
        public enum TransferType { Binary, ASCII };

        /// <summary>
        /// 设置传输模式
        /// </summary>
        /// <param name="ttType">传输模式</param>
        public void setTransferType(TransferType ttType)
        {
            if (ttType == TransferType.Binary)
            {
                sendCommand("TYPE I");//binary类型传输
            }
            else
            {
                sendCommand("TYPE A");//ASCII类型传输
            }
            if (iReplyCode != 200)
            {
                throw new IOException(strReply.Substring(4));
            }
            else
            {
                trType = ttType;
            }
        }

        /// <summary>
        /// 获得传输模式
        /// </summary>
        /// <returns>传输模式</returns>
        public TransferType getTransferType()
        {
            return trType;
        }
        #endregion

        #region 文件操作
        /// <summary>
        /// 获得文件列表
        /// </summary>
        /// <param name="strMask">文件名的匹配字符串</param>
        public string[] dir(string strMask)
        {
            if (!bConnected)
            {
                connect();
            }
            Socket socketData = createDataSocket();
            sendCommand("NLST " + strMask);
            if (!(iReplyCode == 150 || iReplyCode == 125 || iReplyCode == 226))
            {
                throw new IOException(strReply.Substring(4));
            }
            strMsg = "";
            System.Threading.Thread.Sleep(2000);
            while (true)
            {
                int iBytes = socketData.Receive(buffer, buffer.Length, 0);
                strMsg += ASCII.GetString(buffer, 0, iBytes);
                if (iBytes < buffer.Length)
                {
                    break;
                }
            }
            char[] seperator = { '\n' };
            string[] strsFileList = strMsg.Split(seperator);
            socketData.Close(); //数据socket关闭时也会有返回码
            if (iReplyCode != 226)
            {
                readReply();
                if (iReplyCode != 226)
                {

                    throw new IOException(strReply.Substring(4));
                }
            }
            return strsFileList;
        }

        public void newPutByGuid(string strFileName, string strGuid)
        {
            if (!bConnected)
            {
                connect();
            }
            string str = strFileName.Substring(0, strFileName.LastIndexOf("\\"));
            string strTypeName = strFileName.Substring(strFileName.LastIndexOf("."));
            strGuid = str + "\\" + strGuid;
            Socket socketData = createDataSocket();
            sendCommand("STOR " + Path.GetFileName(strGuid));
            if (!(iReplyCode == 125 || iReplyCode == 150))
            {
                throw new IOException(strReply.Substring(4));
            }
            FileStream input = new FileStream(strGuid, FileMode.Open);
            input.Flush();
            int iBytes = 0;
            while ((iBytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                socketData.Send(buffer, iBytes, 0);
            }
            input.Close();
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(iReplyCode == 226 || iReplyCode == 250))
            {
                readReply();
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <returns>文件大小</returns>
        public long getFileSize(string strFileName)
        {
            if (!bConnected)
            {
                connect();
            }
            sendCommand("SIZE " + Path.GetFileName(strFileName));
            long lSize = 0;
            if (iReplyCode == 213)
            {
                lSize = Int64.Parse(strReply.Substring(4));
            }
            else
            {
                throw new IOException(strReply.Substring(4));
            }
            return lSize;
        }


        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <returns>文件大小</returns>
        public string getFileInfo(string strFileName)
        {
            if (!bConnected)
            {
                connect();
            }
            Socket socketData = createDataSocket();
            sendCommand("LIST " + strFileName);
            string strResult = "";
            if (!(iReplyCode == 150 || iReplyCode == 125
                || iReplyCode == 226 || iReplyCode == 250))
            {
                throw new IOException(strReply.Substring(4));
            }
            byte[] b = new byte[512];
            MemoryStream ms = new MemoryStream();

            while (true)
            {
                int iBytes = socketData.Receive(b, b.Length, 0);
                ms.Write(b, 0, iBytes);
                if (iBytes <= 0)
                {

                    break;
                }
            }
            byte[] bt = ms.GetBuffer();
            strResult = System.Text.Encoding.ASCII.GetString(bt);
            ms.Close();
            return strResult;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strFileName">待删除文件名</param>
        public void delete(string strFileName)
        {
            if (!bConnected)
            {
                connect();
            }
            sendCommand("DELE " + strFileName);
            if (iReplyCode != 250)
            {
                throw new IOException(strReply.Substring(4));
            }
        }

        /// <summary>
        /// 重命名(如果新文件名与已有文件重名,将覆盖已有文件)
        /// </summary>
        /// <param name="strOldFileName">旧文件名</param>
        /// <param name="strNewFileName">新文件名</param>
        public void rename(string strOldFileName, string strNewFileName)
        {
            if (!bConnected)
            {
                connect();
            }
            sendCommand("RNFR " + strOldFileName);
            if (iReplyCode != 350)
            {
                throw new IOException(strReply.Substring(4));
            }
            //  如果新文件名与原有文件重名,将覆盖原有文件
            sendCommand("RNTO " + strNewFileName);
            if (iReplyCode != 250)
            {
                throw new IOException(strReply.Substring(4));
            }
        }
        #endregion

        #region 上传和下载
        /// <summary>
        /// 下载一批文件
        /// </summary>
        /// <param name="strFileNameMask">文件名的匹配字符串</param>
        /// <param name="strFolder">本地目录(不得以\结束)</param>
        public void get(string strFileNameMask, string strFolder)
        {
            if (!bConnected)
            {
                connect();
            }
            string[] strFiles = dir(strFileNameMask);
            foreach (string strFile in strFiles)
            {
                if (!strFile.Equals(""))//一般来说strFiles的最后一个元素可能是空字符串
                {
                    get(strFile, strFolder, strFile);
                }
            }
        }

        /// <summary>
        /// 下载一个文件
        /// </summary>
        /// <param name="strRemoteFileName">要下载的文件名</param>
        /// <param name="strFolder">本地目录(不得以\结束)</param>
        /// <param name="strLocalFileName">保存在本地时的文件名</param>
        public void get(string strRemoteFileName, string strFolder, string strLocalFileName)
        {
            Socket socketData = createDataSocket();
            try
            {
                if (!bConnected)
                {
                    connect();
                }
                setTransferType(TransferType.Binary);
                if (strLocalFileName.Equals(""))
                {
                    strLocalFileName = strRemoteFileName;
                }
                sendCommand("RETR " + strRemoteFileName);
                if (!(iReplyCode == 150 || iReplyCode == 125 || iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
                FileStream output = new FileStream(strFolder + "\\" + strLocalFileName, FileMode.Create);
                while (true)
                {
                    int iBytes = socketData.Receive(buffer, buffer.Length, 0);
                    output.Write(buffer, 0, iBytes);
                    if (iBytes <= 0)
                    {
                        break;
                    }
                }
                output.Close();
                if (socketData.Connected)
                {
                    socketData.Close();
                }
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    readReply();
                    if (!(iReplyCode == 226 || iReplyCode == 250))
                    {
                        throw new IOException(strReply.Substring(4));
                    }
                }
            }
            catch
            {
                socketData.Close();
                socketData = null;
                socketControl.Close();
                bConnected = false;
                socketControl = null;
            }
        }

        /// <summary>
        /// 下载一个文件
        /// </summary>
        /// <param name="strRemoteFileName">要下载的文件名</param>
        /// <param name="strFolder">本地目录(不得以\结束)</param>
        /// <param name="strLocalFileName">保存在本地时的文件名</param>
        public void getNoBinary(string strRemoteFileName, string strFolder, string strLocalFileName)
        {
            if (!bConnected)
            {
                connect();
            }

            if (strLocalFileName.Equals(""))
            {
                strLocalFileName = strRemoteFileName;
            }
            Socket socketData = createDataSocket();
            sendCommand("RETR " + strRemoteFileName);
            if (!(iReplyCode == 150 || iReplyCode == 125 || iReplyCode == 226 || iReplyCode == 250))
            {
                throw new IOException(strReply.Substring(4));
            }
            FileStream output = new FileStream(strFolder + "\\" + strLocalFileName, FileMode.Create);
            while (true)
            {
                int iBytes = socketData.Receive(buffer, buffer.Length, 0);
                output.Write(buffer, 0, iBytes);
                if (iBytes <= 0)
                {
                    break;
                }
            }
            output.Close();
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(iReplyCode == 226 || iReplyCode == 250))
            {
                readReply();
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
            }
        }

        /// <summary>
        /// 上传一批文件
        /// </summary>
        /// <param name="strFolder">本地目录(不得以\结束)</param>
        /// <param name="strFileNameMask">文件名匹配字符(可以包含*和?)</param>
        public void put(string strFolder, string strFileNameMask)
        {
            string[] strFiles = Directory.GetFiles(strFolder, strFileNameMask);
            foreach (string strFile in strFiles)
            {
                put(strFile);
            }
        }

        /// <summary>
        /// 上传一个文件
        /// </summary>
        /// <param name="strFileName">本地文件名</param>
        public void put(string strFileName)
        {
            if (!bConnected)
            {
                connect();
            }
            Socket socketData = createDataSocket();
            if (Path.GetExtension(strFileName) == "")
                sendCommand("STOR " + Path.GetFileNameWithoutExtension(strFileName));
            else
                sendCommand("STOR " + Path.GetFileName(strFileName));

            if (!(iReplyCode == 125 || iReplyCode == 150))
            {
                throw new IOException(strReply.Substring(4));
            }

            FileStream input = new FileStream(strFileName, FileMode.Open);
            int iBytes = 0;
            while ((iBytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                socketData.Send(buffer, iBytes, 0);
            }
            input.Close();
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(iReplyCode == 226 || iReplyCode == 250))
            {
                readReply();
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
            }
        }


        /// <summary>
        /// 上传一个文件
        /// </summary>
        /// <param name="strFileName">本地文件名</param>
        public void putByGuid(string strFileName, string strGuid)
        {
            if (!bConnected)
            {
                connect();
            }
            string str = strFileName.Substring(0, strFileName.LastIndexOf("\\"));
            string strTypeName = strFileName.Substring(strFileName.LastIndexOf("."));
            strGuid = str + "\\" + strGuid;
            System.IO.File.Copy(strFileName, strGuid);
            System.IO.File.SetAttributes(strGuid, System.IO.FileAttributes.Normal);
            Socket socketData = createDataSocket();
            sendCommand("STOR " + Path.GetFileName(strGuid));
            if (!(iReplyCode == 125 || iReplyCode == 150))
            {
                throw new IOException(strReply.Substring(4));
            }
            FileStream input = new FileStream(strGuid, FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            int iBytes = 0;
            while ((iBytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                socketData.Send(buffer, iBytes, 0);
            }
            input.Close();
            File.Delete(strGuid);
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(iReplyCode == 226 || iReplyCode == 250))
            {
                readReply();
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
            }
        }
        #endregion

        #region 目录操作
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="strDirName">目录名</param>
        public void makeDir(string strDirName)
        {
            if (!bConnected)
            {
                connect();
            }
            sendCommand("MKD " + strDirName);
            if (iReplyCode != 257)
            {
                throw new IOException(strReply.Substring(4));
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="strDirName">目录名</param>
        public void RmDir(string strDirName)
        {
            if (!bConnected)
            {
                connect();
            }
            sendCommand("RMD " + strDirName);
            if (iReplyCode != 250)
            {
                throw new IOException(strReply.Substring(4));
            }
        }

        /// <summary>
        /// 改变目录
        /// </summary>
        /// <param name="strDirName">新的工作目录名</param>
        public void changeDir(string strDirName)
        {
            if (strDirName.Equals(".") || strDirName.Equals(""))
            {
                return;
            }
            if (!bConnected)
            {
                connect();
            }
            sendCommand("CWD " + strDirName);
            if (iReplyCode != 250)
            {
                throw new IOException(strReply.Substring(4));
            }
            this.strRemotePath = strDirName;
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 将一行应答字符串记录在strReply和strMsg,应答码记录在iReplyCode
        /// </summary>
        private void readReply()
        {
            strMsg = "";
            strReply = readLine();
            iReplyCode = Int32.Parse(strReply.Substring(0, 3));
        }

        /// <summary>
        /// 建立进行数据连接的socket
        /// </summary>
        /// <returns>数据连接socket</returns>
        private Socket createDataSocket()
        {
            sendCommand("PASV");
            if (iReplyCode != 227)
            {
                throw new IOException(strReply.Substring(4));
            }
            int index1 = strReply.IndexOf('(');
            int index2 = strReply.IndexOf(')');
            string ipData = strReply.Substring(index1 + 1, index2 - index1 - 1);
            int[] parts = new int[6];
            int len = ipData.Length;
            int partCount = 0;
            string buf = "";
            for (int i = 0; i < len && partCount <= 6; i++)
            {
                char ch = Char.Parse(ipData.Substring(i, 1));
                if (Char.IsDigit(ch))
                    buf += ch;
                else if (ch != ',')
                {
                    throw new IOException("Malformed PASV strReply: " + strReply);
                }
                if (ch == ',' || i + 1 == len)
                {
                    try
                    {
                        parts[partCount++] = Int32.Parse(buf);
                        buf = "";
                    }
                    catch (Exception)
                    {
                        throw new IOException("Malformed PASV strReply: " + strReply);
                    }
                }
            }
            string ipAddress = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];
            int port = (parts[4] << 8) + parts[5];
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            try
            {
                s.Connect(ep);
            }
            catch (Exception)
            {
                throw new IOException("无法连接ftp服务器");
            }
            return s;
        }

        /// <summary>
        /// 关闭socket连接(用于登录以前)
        /// </summary>
        private void closeSocketConnect()
        {
            lock (obj)
            {
                if (socketControl != null)
                {
                    socketControl.Close();
                    socketControl = null;
                }
                bConnected = false;
            }
        }

        /// <summary>
        /// 读取Socket返回的所有字符串
        /// </summary>
        /// <returns>包含应答码的字符串行</returns>
        private string readLine()
        {
            lock (obj)
            {
                while (true)
                {
                    int iBytes = socketControl.Receive(buffer, buffer.Length, 0);
                    strMsg += ASCII.GetString(buffer, 0, iBytes);
                    if (iBytes < buffer.Length)
                    {
                        break;
                    }
                }
            }
            char[] seperator = { '\n' };
            string[] mess = strMsg.Split(seperator);
            if (strMsg.Length > 2)
            {
                strMsg = mess[mess.Length - 2];
            }
            else
            {
                strMsg = mess[0];
            }
            if (!strMsg.Substring(3, 1).Equals(" ")) //返回字符串正确的是以应答码(如220开头,后面接一空格,再接问候字符串)
            {
                return readLine();
            }
            return strMsg;
        }

        /// <summary>
        /// 发送命令并获取应答码和最后一行应答字符串
        /// </summary>
        /// <param name="strCommand">命令</param>
        public void sendCommand(String strCommand)
        {
            lock (obj)
            {
                Byte[] cmdBytes = Encoding.ASCII.GetBytes((strCommand + "\r\n").ToCharArray());
                socketControl.Send(cmdBytes, cmdBytes.Length, 0);
                System.Threading.Thread.Sleep(500);
                readReply();
            }
        }
        #endregion
    }
}