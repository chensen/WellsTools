using System;
using System.Text;
using System.IO;

namespace Wells.Tools
{
    /// <summary>
    /// �ļ�������
    /// </summary>
    public static class clsFile
    {
        #region ���ָ��Ŀ¼�Ƿ����
        /// <summary>
        /// ���ָ��Ŀ¼�Ƿ����
        /// </summary>
        /// <param name="directoryPath">Ŀ¼�ľ���·��</param>
        /// <returns></returns>
        public static bool isExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        #endregion

        #region ���ָ���ļ��Ƿ����,������ڷ���true
        /// <summary>
        /// ���ָ���ļ��Ƿ����,��������򷵻�true��
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>        
        public static bool isExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion

        #region ��ȡָ��Ŀ¼�е��ļ��б�
        /// <summary>
        /// ��ȡָ��Ŀ¼�������ļ��б�
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>        
        public static string[] getFileNames(string directoryPath)
        {
            //���Ŀ¼�����ڣ����׳��쳣
            if (!isExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //��ȡ�ļ��б�
            return Directory.GetFiles(directoryPath);
        }
        #endregion

        #region ��ȡָ��Ŀ¼��������Ŀ¼�б�,��Ҫ����Ƕ�׵���Ŀ¼�б�,��ʹ�����ط���.
        /// <summary>
        /// ��ȡָ��Ŀ¼��������Ŀ¼�б�,��Ҫ����Ƕ�׵���Ŀ¼�б�,��ʹ�����ط���.
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>        
        public static string[] getDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ��ȡָ��Ŀ¼����Ŀ¼�������ļ��б�
        /// <summary>
        /// ��ȡָ��Ŀ¼����Ŀ¼�������ļ��б�
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        /// <param name="searchPattern">ģʽ�ַ�����"*"����0��N���ַ���"?"����1���ַ���
        /// ������"Log*.xml"��ʾ����������Log��ͷ��Xml�ļ���</param>
        /// <param name="isSearchChild">�Ƿ�������Ŀ¼</param>
        public static string[] getFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //���Ŀ¼�����ڣ����׳��쳣
            if (!isExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ���ָ��Ŀ¼�Ƿ�Ϊ��
        /// <summary>
        /// ���ָ��Ŀ¼�Ƿ�Ϊ��
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>        
        public static bool isEmptyDirectory(string directoryPath)
        {
            try
            {
                //�ж��Ƿ�����ļ�
                string[] fileNames = getFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                //�ж��Ƿ�����ļ���
                string[] directoryNames = getDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                //�����¼��־
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return true;
            }
        }
        #endregion

        #region ���ָ��Ŀ¼���Ƿ����ָ�����ļ�
        /// <summary>
        /// ���ָ��Ŀ¼���Ƿ����ָ�����ļ�
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        /// <param name="searchPattern">ģʽ�ַ�����"*"����0��N���ַ���"?"����1���ַ���
        /// ������"Log*.xml"��ʾ����������Log��ͷ��Xml�ļ���</param> 
        /// <param name="isSearchChild">�Ƿ�������Ŀ¼</param>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild = false)
        {
            try
            {
                //��ȡָ�����ļ��б�
                string[] fileNames = getFileNames(directoryPath, searchPattern, isSearchChild);

                //�ж�ָ���ļ��Ƿ����
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
            }
        }
        #endregion

        #region ������վĿ¼
        /// <summary>
        /// ������վĿ¼
        /// </summary>
        /// <param name="dir">Ҫ��������վĿ¼·������Ŀ¼��</param>
        public static void createWebDirectory(string dir)
        {
            if (dir.Length == 0) return;
            if (!Directory.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir))
                Directory.CreateDirectory(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir);
        }
        #endregion

        #region ɾ����վĿ¼
        /// <summary>
        /// ɾ����վĿ¼
        /// </summary>
        /// <param name="dir">Ҫɾ������վĿ¼·��������</param>
        public static void deleteWebDirectory(string dir)
        {
            if (dir.Length == 0) return;
            if (Directory.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir))
                Directory.Delete(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir);
        }
        #endregion

        #region ɾ����վ�ļ�
        /// <summary>
        /// ɾ����վ�ļ�
        /// </summary>
        /// <param name="file">Ҫɾ������վ�ļ�·��������</param>
        public static void deleteWebFile(string file)
        {
            if (File.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + file))
                File.Delete(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + file);
        }
        #endregion

        #region ������վ�ļ�
        /// <summary>
        /// ������վ�ļ�
        /// </summary>
        /// <param name="dir">����׺���ļ���</param>
        /// <param name="pagestr">�ļ�����</param>
        public static void createWebFile(string dir, string pagestr)
        {
            dir = dir.Replace("/", "\\");
            if (dir.IndexOf("\\") > -1)
                createWebDirectory(dir.Substring(0, dir.LastIndexOf("\\")));
            System.IO.StreamWriter sw = new System.IO.StreamWriter(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir, false, System.Text.Encoding.GetEncoding("GB2312"));
            sw.Write(pagestr);
            sw.Close();
        }
        #endregion

        #region �ƶ���վ�ļ�(����--ճ��)
        /// <summary>
        /// �ƶ��ļ�(����--ճ��)
        /// </summary>
        /// <param name="dir1">Ҫ�ƶ����ļ���·����ȫ��(������׺)</param>
        /// <param name="dir2">�ļ��ƶ����µ�λ��,��ָ���µ��ļ���</param>
        public static void moveWebFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1))
                File.Move(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1, System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir2);
        }
        #endregion

        #region ������վ�ļ�
        /// <summary>
        /// ������վ�ļ�
        /// </summary>
        /// <param name="dir1">Ҫ���Ƶ��ļ���·���Ѿ�ȫ��(������׺)</param>
        /// <param name="dir2">Ŀ��λ��,��ָ���µ��ļ���</param>
        public static void copyWebFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1))
            {
                File.Copy(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1, System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir2, true);
            }
        }
        #endregion

        #region ����ʱ��õ�Ŀ¼�� / ��ʽ:yyyyMMdd ���� HHmmssff
        /// <summary>
        /// ����ʱ��õ�Ŀ¼��yyyyMMdd
        /// </summary>
        /// <returns></returns>
        public static string getDateDirectory()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }
        
        /// <summary>
        /// ����ʱ��õ��ļ���HHmmssff
        /// </summary>
        /// <returns></returns>
        public static string getDateFile()
        {
            return DateTime.Now.ToString("HHmmssff");
        }
        #endregion

        #region �����ļ���
        /// <summary>
        /// �����ļ���(�ݹ�)
        /// </summary>
        /// <param name="varFromDirectory">Դ�ļ���·��</param>
        /// <param name="varToDirectory">Ŀ���ļ���·��</param>
        public static void copyFolder(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    copyFolder(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }
            string[] files = Directory.GetFiles(varFromDirectory);
            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Copy(s, varToDirectory + s.Substring(s.LastIndexOf("\\")), true);
                }
            }
        }
        #endregion

        #region ����ļ�,����ļ��������򴴽�
        /// <summary>
        /// ����ļ�,����ļ��������򴴽�  
        /// </summary>
        /// <param name="FilePath">·��,�����ļ���</param>
        public static void existsFile(string FilePath)
        {
            //if(!File.Exists(FilePath))    
            //File.Create(FilePath);    
            //����д���ᱨ��,��ϸ�����뿴����.........   
            if (!File.Exists(FilePath))
            {
                FileStream fs = File.Create(FilePath);
                fs.Close();
            }
        }
        #endregion

        #region ɾ��ָ���ļ��ж�Ӧ�����ļ�������ļ�
        /// <summary>
        /// ɾ��ָ���ļ��ж�Ӧ�����ļ�������ļ�
        /// </summary>
        /// <param name="varFromDirectory">ָ���ļ���·��</param>
        /// <param name="varToDirectory">��Ӧ�����ļ���·��</param>
        public static void deleteFolderFiles(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    deleteFolderFiles(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }


            string[] files = Directory.GetFiles(varFromDirectory);

            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Delete(varToDirectory + s.Substring(s.LastIndexOf("\\")));
                }
            }
        }
        #endregion

        #region ���ļ��ľ���·���л�ȡ�ļ���( ������չ�� )
        /// <summary>
        /// ���ļ��ľ���·���л�ȡ�ļ���( ������չ�� )
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>        
        public static string getFileName(string filePath)
        {
            //��ȡ�ļ�������
            FileInfo fi = new FileInfo(filePath);
            return fi.Name;
        }
        #endregion

        #region ���ļ��ľ���·���л�ȡ�ļ���( ��������չ�� )
        /// <summary>
        /// ���ļ��ľ���·���л�ȡ�ļ���( ��������չ�� )
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>        
        public static string getFileTitle(string filePath)
        {
            //��ȡ�ļ�������
            FileInfo fi = new FileInfo(filePath);
            return fi.Name.Substring(0, fi.Name.LastIndexOf('.'));
        }
        #endregion

        #region ���ļ��ľ���·���л�ȡ�ļ�������չ��
        /// <summary>
        /// ���ļ��ľ���·���л�ȡ�ļ�������չ��
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>        
        public static string getFileExtension(string filePath)
        {
            //��ȡ�ļ�������
            FileInfo fi = new FileInfo(filePath);
            //return fi.Name.Substring(fi.Name.LastIndexOf('.') + 1);
            return fi.Extension;
        }
        #endregion

        #region ���ļ��ľ���·���л�ȡ�ļ�·��
        /// <summary>
        /// ���ļ��ľ���·���л�ȡ�ļ�·��
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>        
        public static string getFilePath(string filePath)
        {
            return filePath.Substring(0, filePath.LastIndexOf('\\'));
        }
        #endregion

        #region �����ļ��ο�����,ҳ��������
        /// <summary>
        /// �����ļ��ο�����,ҳ��������
        /// </summary>
        /// <param name="cDir">��·��</param>
        /// <param name="TempId">ģ�������滻���</param>
        public static void copyFiles(string cDir, string TempId)
        {
            //if (Directory.Exists(Request.PhysicalApplicationPath + "\\Controls"))
            //{
            //    string TempStr = string.Empty;
            //    StreamWriter sw;
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Default.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\List.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\View.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\DissList.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Diss.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Review.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Search.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //}
        }
        #endregion

        #region ����һ��Ŀ¼
        /// <summary>
        /// ����һ��Ŀ¼
        /// </summary>
        /// <param name="directoryPath">Ŀ¼�ľ���·��</param>
        public static void createDirectory(string directoryPath)
        {
            //���Ŀ¼�������򴴽���Ŀ¼
            if (!isExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        #endregion

        #region ����һ���ļ�
        /// <summary>
        /// ����һ���ļ���
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        public static void createFile(string filePath)
        {
            try
            {
                //����ļ��������򴴽����ļ�
                if (!isExistFile(filePath))
                {
                    //����һ��FileInfo����
                    FileInfo file = new FileInfo(filePath);

                    //�����ļ�
                    FileStream fs = file.Create();

                    //�ر��ļ���
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }
        
        /// <summary>
        /// ����һ���ļ�,�����ֽ���д���ļ���
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        /// <param name="buffer">������������</param>
        public static void createFile(string filePath, byte[] buffer)
        {
            try
            {
                //����ļ��������򴴽����ļ�
                if (!isExistFile(filePath))
                {
                    //����һ��FileInfo����
                    FileInfo file = new FileInfo(filePath);

                    //�����ļ�
                    FileStream fs = file.Create();

                    //д���������
                    fs.Write(buffer, 0, buffer.Length);

                    //�ر��ļ���
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }
        #endregion

        #region ��ȡ�ı��ļ�������
        /// <summary>
        /// ��ȡ�ı��ļ�������
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>        
        public static int getLineCount(string filePath)
        {
            //���ı��ļ��ĸ��ж���һ���ַ���������
            string[] rows = File.ReadAllLines(filePath);

            //��������
            return rows.Length;
        }
        #endregion

        #region ��ȡһ���ļ��ĳ���
        /// <summary>
        /// ��ȡһ���ļ��ĳ���,��λΪByte
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>        
        public static int getFileSize(string filePath)
        {
            //����һ���ļ�����
            FileInfo fi = new FileInfo(filePath);

            //��ȡ�ļ��Ĵ�С
            return (int)fi.Length;
        }
        #endregion

        #region ��ȡָ��Ŀ¼�е���Ŀ¼�б�
        /// <summary>
        /// ��ȡָ��Ŀ¼����Ŀ¼��������Ŀ¼�б�
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        /// <param name="searchPattern">ģʽ�ַ�����"*"����0��N���ַ���"?"����1���ַ���
        /// ������"Log*.xml"��ʾ����������Log��ͷ��Xml�ļ���</param>
        /// <param name="isSearchChild">�Ƿ�������Ŀ¼</param>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ���ı��ļ�д������
        /// <summary>
        /// ���ı��ļ���д������
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        /// <param name="text">д�������</param>
        /// <param name="encoding">����</param>
        public static void writeText(string filePath, string text, Encoding encoding)
        {
            //���ļ�д������
            File.WriteAllText(filePath, text, encoding);
        }
        #endregion

        #region ���ı��ļ���β��׷������
        /// <summary>
        /// ���ı��ļ���β��׷������
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        /// <param name="content">д�������</param>
        public static void appendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }
        #endregion

        #region �������ļ������ݸ��Ƶ����ļ���
        /// <summary>
        /// ��Դ�ļ������ݸ��Ƶ�Ŀ���ļ���
        /// </summary>
        /// <param name="sourceFilePath">Դ�ļ��ľ���·��</param>
        /// <param name="destFilePath">Ŀ���ļ��ľ���·��</param>
        public static void copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }
        #endregion

        #region ���ļ��ƶ���ָ��Ŀ¼
        /// <summary>
        /// ���ļ��ƶ���ָ��Ŀ¼
        /// </summary>
        /// <param name="sourceFilePath">��Ҫ�ƶ���Դ�ļ��ľ���·��</param>
        /// <param name="descDirectoryPath">�ƶ�����Ŀ¼�ľ���·��</param>
        public static void move(string sourceFilePath, string descDirectoryPath)
        {
            //��ȡԴ�ļ�������
            string sourceFileName = getFileName(sourceFilePath);

            if (isExistDirectory(descDirectoryPath))
            {
                //���Ŀ���д���ͬ���ļ�,��ɾ��
                if (isExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    File.Delete(descDirectoryPath + "\\" + sourceFileName);
                }
                //���ļ��ƶ���ָ��Ŀ¼
                File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }
        #endregion

        #region ���ָ��Ŀ¼
        /// <summary>
        /// ���ָ��Ŀ¼�������ļ�����Ŀ¼,����Ŀ¼��Ȼ����.
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        public static void ClearDirectory(string directoryPath)
        {
            if (isExistDirectory(directoryPath))
            {
                //ɾ��Ŀ¼�����е��ļ�
                string[] fileNames = getFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    File.Delete(fileNames[i]);
                }

                //ɾ��Ŀ¼�����е���Ŀ¼
                string[] directoryNames = getDirectories(directoryPath);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    deleteDirectory(directoryNames[i]);
                }
            }
        }
        #endregion

        #region ����ļ�����
        /// <summary>
        /// ����ļ�����
        /// </summary>
        /// <param name="filePath">�ļ��ľ���·��</param>
        public static void clearFile(string filePath)
        {
            //ɾ���ļ�
            File.Delete(filePath);

            //���´������ļ�
            createFile(filePath);
        }
        #endregion

        #region ɾ��ָ��Ŀ¼
        /// <summary>
        /// ɾ��ָ��Ŀ¼����������Ŀ¼
        /// </summary>
        /// <param name="directoryPath">ָ��Ŀ¼�ľ���·��</param>
        public static void deleteDirectory(string directoryPath)
        {
            if (isExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
        #endregion
    }

    /// <summary>
    /// �ļ�������
    /// </summary>
    public class clsFileCN
    {
        #region д�ļ�
        protected void writeTxt(string FileName, string Content)
        {
            Encoding code = Encoding.GetEncoding("gb2312");
            string htmlfilename = System.Web.HttpContext.Current.Server.MapPath("Precious\\" + FileName + ".txt");��//�����ļ���·��
            string str = Content;
            StreamWriter sw = null;
            {
                try
                {
                    sw = new StreamWriter(htmlfilename, false, code);
                    sw.Write(str);
                    sw.Flush();
                }
                catch { }
            }
            sw.Close();
            sw.Dispose();

        }
        #endregion

        #region ���ļ�
        protected string readTxt(string filename)
        {

            Encoding code = Encoding.GetEncoding("gb2312");
            string temp = System.Web.HttpContext.Current.Server.MapPath("Precious\\" + filename + ".txt");
            string str = "";
            if (File.Exists(temp))
            {
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader(temp, code);
                    str = sr.ReadToEnd(); // ��ȡ�ļ�
                }
                catch { }
                sr.Close();
                sr.Dispose();
            }
            else
            {
                str = "";
            }


            return str;
        }
        #endregion

        #region ȡ���ļ���׺��
        /****************************************
         * �������ƣ�GetPostfixStr
         * ����˵����ȡ���ļ���׺��
         * ��    ����filename:�ļ�����
         * ����ʾ�У�
         *           string filename = "aaa.aspx";        
         *           string s = DotNet.Utilities.FileOperate.GetPostfixStr(filename);         
        *****************************************/
        /// <summary>
        /// ȡ��׺��
        /// </summary>
        /// <param name="filename">�ļ���</param>
        /// <returns>.gif|.html��ʽ</returns>
        public static string getPostfixStr(string filename)
        {
            int start = filename.LastIndexOf(".");
            int length = filename.Length;
            string postfix = filename.Substring(start, length - start);
            return postfix;
        }
        #endregion

        #region д�ļ�
        /****************************************
         * �������ƣ�WriteFile
         * ����˵�������ļ�����ʱ���򴴽��ļ�����׷���ļ�
         * ��    ����Path:�ļ�·��,Strings:�ı�����
         * ����ʾ�У�
         *           string Path = Server.MapPath("Default2.aspx");       
         *           string Strings = "������д�����ݰ�";
         *           DotNet.Utilities.FileOperate.WriteFile(Path,Strings);
        *****************************************/
        /// <summary>
        /// д�ļ�
        /// </summary>
        /// <param name="Path">�ļ�·��</param>
        /// <param name="Strings">�ļ�����</param>
        public static void writeFile(string Path, string Strings)
        {

            if (!System.IO.File.Exists(Path))
            {
                System.IO.FileStream f = System.IO.File.Create(Path);
                f.Close();
                f.Dispose();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(Path, true, System.Text.Encoding.UTF8);
            f2.WriteLine(Strings);
            f2.Close();
            f2.Dispose();


        }
        #endregion

        #region ���ļ�
        /****************************************
         * �������ƣ�ReadFile
         * ����˵������ȡ�ı�����
         * ��    ����Path:�ļ�·��
         * ����ʾ�У�
         *           string Path = Server.MapPath("Default2.aspx");       
         *           string s = DotNet.Utilities.FileOperate.ReadFile(Path);
        *****************************************/
        /// <summary>
        /// ���ļ�
        /// </summary>
        /// <param name="Path">�ļ�·��</param>
        /// <returns></returns>
        public static string readFile(string Path)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                s = "��������Ӧ��Ŀ¼";
            else
            {
                StreamReader f2 = new StreamReader(Path, System.Text.Encoding.GetEncoding("gb2312"));
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }
        #endregion

        #region ׷���ļ�
        /****************************************
         * �������ƣ�FileAdd
         * ����˵����׷���ļ�����
         * ��    ����Path:�ļ�·��,strings:����
         * ����ʾ�У�
         *           string Path = Server.MapPath("Default2.aspx");     
         *           string Strings = "��׷������";
         *           DotNet.Utilities.FileOperate.FileAdd(Path, Strings);
        *****************************************/
        /// <summary>
        /// ׷���ļ�
        /// </summary>
        /// <param name="Path">�ļ�·��</param>
        /// <param name="strings">����</param>
        public static void addFile(string Path, string strings)
        {
            StreamWriter sw = File.AppendText(Path);
            sw.Write(strings);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        #endregion

        #region �����ļ�
        /****************************************
         * �������ƣ�FileCoppy
         * ����˵���������ļ�
         * ��    ����OrignFile:ԭʼ�ļ�,NewFile:���ļ�·��
         * ����ʾ�У�
         *           string OrignFile = Server.MapPath("Default2.aspx");     
         *           string NewFile = Server.MapPath("Default3.aspx");
         *           DotNet.Utilities.FileOperate.FileCoppy(OrignFile, NewFile);
        *****************************************/
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="OrignFile">ԭʼ�ļ�</param>
        /// <param name="NewFile">���ļ�·��</param>
        public static void copyFile(string OrignFile, string NewFile)
        {
            File.Copy(OrignFile, NewFile, true);
        }

        #endregion

        #region ɾ���ļ�
        /****************************************
         * �������ƣ�FileDel
         * ����˵����ɾ���ļ�
         * ��    ����Path:�ļ�·��
         * ����ʾ�У�
         *           string Path = Server.MapPath("Default3.aspx");    
         *           DotNet.Utilities.FileOperate.FileDel(Path);
        *****************************************/
        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="Path">·��</param>
        public static void deleteFile(string Path)
        {
            File.Delete(Path);
        }
        #endregion

        #region �ƶ��ļ�
        /****************************************
         * �������ƣ�FileMove
         * ����˵�����ƶ��ļ�
         * ��    ����OrignFile:ԭʼ·��,NewFile:���ļ�·��
         * ����ʾ�У�
         *            string OrignFile = Server.MapPath("../˵��.txt");    
         *            string NewFile = Server.MapPath("../../˵��.txt");
         *            DotNet.Utilities.FileOperate.FileMove(OrignFile, NewFile);
        *****************************************/
        /// <summary>
        /// �ƶ��ļ�
        /// </summary>
        /// <param name="OrignFile">ԭʼ·��</param>
        /// <param name="NewFile">��·��</param>
        public static void moveFile(string OrignFile, string NewFile)
        {
            File.Move(OrignFile, NewFile);
        }
        #endregion

        #region �ڵ�ǰĿ¼�´���Ŀ¼
        /****************************************
         * �������ƣ�FolderCreate
         * ����˵�����ڵ�ǰĿ¼�´���Ŀ¼
         * ��    ����OrignFolder:��ǰĿ¼,NewFloder:��Ŀ¼
         * ����ʾ�У�
         *           string OrignFolder = Server.MapPath("test/");    
         *           string NewFloder = "new";
         *           DotNet.Utilities.FileOperate.FolderCreate(OrignFolder, NewFloder); 
        *****************************************/
        /// <summary>
        /// �ڵ�ǰĿ¼�´���Ŀ¼
        /// </summary>
        /// <param name="OrignFolder">��ǰĿ¼</param>
        /// <param name="NewFloder">��Ŀ¼</param>
        public static void createFolder(string OrignFolder, string NewFloder)
        {
            Directory.SetCurrentDirectory(OrignFolder);
            Directory.CreateDirectory(NewFloder);
        }

        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="Path"></param>
        public static void createFolder(string Path)
        {
            // �ж�Ŀ��Ŀ¼�Ƿ����������������½�֮
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        #endregion

        #region ����Ŀ¼
        public static void createFile(string Path)
        {
            FileInfo CreateFile = new FileInfo(Path); //�����ļ� 
            if (!CreateFile.Exists)
            {
                FileStream FS = CreateFile.Create();
                FS.Close();
            }
        }
        #endregion

        #region �ݹ�ɾ���ļ���Ŀ¼���ļ�
        /****************************************
         * �������ƣ�DeleteFolder
         * ����˵�����ݹ�ɾ���ļ���Ŀ¼���ļ�
         * ��    ����dir:�ļ���·��
         * ����ʾ�У�
         *           string dir = Server.MapPath("test/");  
         *           DotNet.Utilities.FileOperate.DeleteFolder(dir);       
        *****************************************/
        /// <summary>
        /// �ݹ�ɾ���ļ���Ŀ¼���ļ�
        /// </summary>
        /// <param name="dir"></param>  
        /// <returns></returns>
        public static void deleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //�����������ļ���ɾ��֮ 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //ֱ��ɾ�����е��ļ�                        
                    else
                        deleteFolder(d); //�ݹ�ɾ�����ļ��� 
                }
                Directory.Delete(dir, true); //ɾ���ѿ��ļ���                 
            }
        }

        #endregion

        #region ��ָ���ļ����������������copy��Ŀ���ļ������� ��Ŀ���ļ���Ϊֻ�����Ծͻᱨ��
        /****************************************
         * �������ƣ�CopyDir
         * ����˵������ָ���ļ����������������copy��Ŀ���ļ������� ��Ŀ���ļ���Ϊֻ�����Ծͻᱨ��
         * ��    ����srcPath:ԭʼ·��,aimPath:Ŀ���ļ���
         * ����ʾ�У�
         *           string srcPath = Server.MapPath("test/");  
         *           string aimPath = Server.MapPath("test1/");
         *           DotNet.Utilities.FileOperate.CopyDir(srcPath,aimPath);   
        *****************************************/
        /// <summary>
        /// ָ���ļ����������������copy��Ŀ���ļ�������
        /// </summary>
        /// <param name="srcPath">ԭʼ·��</param>
        /// <param name="aimPath">Ŀ���ļ���</param>
        public static void copyDir(string srcPath, string aimPath)
        {
            try
            {
                // ���Ŀ��Ŀ¼�Ƿ���Ŀ¼�ָ��ַ�����������������֮
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // �ж�Ŀ��Ŀ¼�Ƿ����������������½�֮
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // �õ�ԴĿ¼���ļ��б��������ǰ����ļ��Լ�Ŀ¼·����һ������
                //�����ָ��copyĿ���ļ�������ļ���������Ŀ¼��ʹ������ķ���
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                //�������е��ļ���Ŀ¼
                foreach (string file in fileList)
                {
                    //�ȵ���Ŀ¼��������������Ŀ¼�͵ݹ�Copy��Ŀ¼������ļ�

                    if (Directory.Exists(file))
                        copyDir(file, aimPath + Path.GetFileName(file));
                    //����ֱ��Copy�ļ�
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
        }
        #endregion

        #region ��ȡָ���ļ�����������Ŀ¼���ļ�(����)
        /****************************************
         * �������ƣ�GetFoldAll(string Path)
         * ����˵������ȡָ���ļ�����������Ŀ¼���ļ�(����)
         * ��    ����Path:��ϸ·��
         * ����ʾ�У�
         *           string strDirlist = Server.MapPath("templates");       
         *           this.Literal1.Text = DotNet.Utilities.FileOperate.GetFoldAll(strDirlist);  
        *****************************************/
        /// <summary>
        /// ��ȡָ���ļ�����������Ŀ¼���ļ�
        /// </summary>
        /// <param name="Path">��ϸ·��</param>
        public static string getFoldAll(string Path)
        {

            string str = "";
            DirectoryInfo thisOne = new DirectoryInfo(Path);
            str = listTreeShow(thisOne, 0, str);
            return str;

        }

        /// <summary>
        /// ��ȡָ���ļ�����������Ŀ¼���ļ�����
        /// </summary>
        /// <param name="theDir">ָ��Ŀ¼</param>
        /// <param name="nLevel">Ĭ����ʼֵ,����ʱ,һ��Ϊ0</param>
        /// <param name="Rn">���ڵ��ӵĴ���ֵ,һ��Ϊ��</param>
        /// <returns></returns>
        public static string listTreeShow(DirectoryInfo theDir, int nLevel, string Rn)//�ݹ�Ŀ¼ �ļ�
        {
            DirectoryInfo[] subDirectories = theDir.GetDirectories();//���Ŀ¼
            foreach (DirectoryInfo dirinfo in subDirectories)
            {

                if (nLevel == 0)
                {
                    Rn += "��";
                }
                else
                {
                    string _s = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        _s += "��&nbsp;";
                    }
                    Rn += _s + "��";
                }
                Rn += "<b>" + dirinfo.Name.ToString() + "</b><br />";
                FileInfo[] fileInfo = dirinfo.GetFiles();   //Ŀ¼�µ��ļ�
                foreach (FileInfo fInfo in fileInfo)
                {
                    if (nLevel == 0)
                    {
                        Rn += "��&nbsp;��";
                    }
                    else
                    {
                        string _f = "";
                        for (int i = 1; i <= nLevel; i++)
                        {
                            _f += "��&nbsp;";
                        }
                        Rn += _f + "��&nbsp;��";
                    }
                    Rn += fInfo.Name.ToString() + " <br />";
                }
                Rn = listTreeShow(dirinfo, nLevel + 1, Rn);


            }
            return Rn;
        }



        /****************************************
         * �������ƣ�GetFoldAll(string Path)
         * ����˵������ȡָ���ļ�����������Ŀ¼���ļ�(��������)
         * ��    ����Path:��ϸ·��
         * ����ʾ�У�
         *            string strDirlist = Server.MapPath("templates");      
         *            this.Literal2.Text = DotNet.Utilities.FileOperate.GetFoldAll(strDirlist,"tpl","");
        *****************************************/
        /// <summary>
        /// ��ȡָ���ļ�����������Ŀ¼���ļ�(��������)
        /// </summary>
        /// <param name="Path">��ϸ·��</param>
        ///<param name="DropName">�����б�����</param>
        ///<param name="tplPath">Ĭ��ѡ��ģ������</param>
        public static string getFoldAll(string Path, string DropName, string tplPath)
        {
            string strDrop = "<select name=\"" + DropName + "\" id=\"" + DropName + "\"><option value=\"\">--��ѡ����ϸģ��--</option>";
            string str = "";
            DirectoryInfo thisOne = new DirectoryInfo(Path);
            str = listTreeShow(thisOne, 0, str, tplPath);
            return strDrop + str + "</select>";

        }

        /// <summary>
        /// ��ȡָ���ļ�����������Ŀ¼���ļ�����
        /// </summary>
        /// <param name="theDir">ָ��Ŀ¼</param>
        /// <param name="nLevel">Ĭ����ʼֵ,����ʱ,һ��Ϊ0</param>
        /// <param name="Rn">���ڵ��ӵĴ���ֵ,һ��Ϊ��</param>
        /// <param name="tplPath">Ĭ��ѡ��ģ������</param>
        /// <returns></returns>
        public static string listTreeShow(DirectoryInfo theDir, int nLevel, string Rn, string tplPath)//�ݹ�Ŀ¼ �ļ�
        {
            DirectoryInfo[] subDirectories = theDir.GetDirectories();//���Ŀ¼

            foreach (DirectoryInfo dirinfo in subDirectories)
            {

                Rn += "<option value=\"" + dirinfo.Name.ToString() + "\"";
                if (tplPath.ToLower() == dirinfo.Name.ToString().ToLower())
                {
                    Rn += " selected ";
                }
                Rn += ">";

                if (nLevel == 0)
                {
                    Rn += "��";
                }
                else
                {
                    string _s = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        _s += "��&nbsp;";
                    }
                    Rn += _s + "��";
                }
                Rn += "" + dirinfo.Name.ToString() + "</option>";


                FileInfo[] fileInfo = dirinfo.GetFiles();   //Ŀ¼�µ��ļ�
                foreach (FileInfo fInfo in fileInfo)
                {
                    Rn += "<option value=\"" + dirinfo.Name.ToString() + "/" + fInfo.Name.ToString() + "\"";
                    if (tplPath.ToLower() == fInfo.Name.ToString().ToLower())
                    {
                        Rn += " selected ";
                    }
                    Rn += ">";

                    if (nLevel == 0)
                    {
                        Rn += "��&nbsp;��";
                    }
                    else
                    {
                        string _f = "";
                        for (int i = 1; i <= nLevel; i++)
                        {
                            _f += "��&nbsp;";
                        }
                        Rn += _f + "��&nbsp;��";
                    }
                    Rn += fInfo.Name.ToString() + "</option>";
                }
                Rn = listTreeShow(dirinfo, nLevel + 1, Rn, tplPath);


            }
            return Rn;
        }
        #endregion

        #region ��ȡ�ļ��д�С
        /****************************************
         * �������ƣ�GetDirectoryLength(string dirPath)
         * ����˵������ȡ�ļ��д�С
         * ��    ����dirPath:�ļ�����ϸ·��
         * ����ʾ�У�
         *           string Path = Server.MapPath("templates"); 
         *           Response.Write(DotNet.Utilities.FileOperate.GetDirectoryLength(Path));       
        *****************************************/
        /// <summary>
        /// ��ȡ�ļ��д�С
        /// </summary>
        /// <param name="dirPath">�ļ���·��</param>
        /// <returns></returns>
        public static long getDirectoryLength(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;
            DirectoryInfo di = new DirectoryInfo(dirPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += getDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }
        #endregion

        #region ��ȡָ���ļ���ϸ����
        /****************************************
         * �������ƣ�GetFileAttibe(string filePath)
         * ����˵������ȡָ���ļ���ϸ����
         * ��    ����filePath:�ļ���ϸ·��
         * ����ʾ�У�
         *           string file = Server.MapPath("robots.txt");  
         *            Response.Write(DotNet.Utilities.FileOperate.GetFileAttibe(file));         
        *****************************************/
        /// <summary>
        /// ��ȡָ���ļ���ϸ����
        /// </summary>
        /// <param name="filePath">�ļ���ϸ·��</param>
        /// <returns></returns>
        public static string getFileAttribute(string filePath)
        {
            string str = "";
            System.IO.FileInfo objFI = new System.IO.FileInfo(filePath);
            str += "��ϸ·��:" + objFI.FullName + "<br>�ļ�����:" + objFI.Name + "<br>�ļ�����:" + objFI.Length.ToString() + "�ֽ�<br>����ʱ��" + objFI.CreationTime.ToString() + "<br>������ʱ��:" + objFI.LastAccessTime.ToString() + "<br>�޸�ʱ��:" + objFI.LastWriteTime.ToString() + "<br>����Ŀ¼:" + objFI.DirectoryName + "<br>��չ��:" + objFI.Extension;
            return str;
        }
        #endregion
    }
}