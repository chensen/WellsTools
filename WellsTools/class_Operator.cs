using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Management;
using System.Diagnostics;
using System.Drawing;
using System.Data.OleDb;
using System.IO;

namespace Wells
{
    class class_Operator
    {
        public static List<Form> listForms = new List<Form>();//窗体集合，主要用来在锁屏及及解锁时使用

        /// <summary>
        /// 创建数据连接对象
        /// </summary>
        /// <returns>OleDbConnection对象</returns>
        public static OleDbConnection getCon()
        {
            OleDbConnection conn;
            try
            {
                conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=ConyData.accdb;");//连接Access2007及以上版本
            }
            catch
            {
                conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=ConyData.mdb;");//连接Access2000及2003版本
            }
            return conn;//返回OleDbConnection对象
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        public void executeSQL(string sql)
        {
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=ConyData.accdb;");//连接Access2007及以上版本
            conn.Open();//打开数据库连接
            OleDbCommand cmd = new OleDbCommand(sql, conn);//创建执行命令对象
            cmd.ExecuteNonQuery();//执行SQL命令
            conn.Close();//关闭数据库连接
        }

        /// <summary>
        /// 将英语转换为对应汉语
        /// </summary>
        /// <param name="str">要转换的英语字符串</param>
        /// <returns>转换后的汉语字符串</returns>
        private string EngtoCH(string str)
        {
            string strCH = "";//记录转换后的汉语字符串
            switch (str)
            {
                #region 记录转换后的汉语字符串
                case "AddressWidth":
                    strCH = "地址宽度";
                    break;
                case "Architecture":
                    strCH = "结构";
                    break;
                case "Availability":
                    strCH = "可用";
                    break;
                case "Caption":
                    strCH = "内部标记";
                    break;
                case "CpuStatus":
                    strCH = "处理器情况";
                    break;
                case "CreationClassName":
                    strCH = "创造类名称";
                    break;
                case "CurrentClockSpeed":
                    strCH = "当前时钟速度";
                    break;
                case "CurrentVoltage":
                    strCH = "当前电压";
                    break;
                case "DataWidth":
                    strCH = "数据宽度";
                    break;
                case "Description":
                    strCH = "描述";
                    break;
                case "DeviceID":
                    strCH = "版本";
                    break;
                case "ExtClock":
                    strCH = "外部时钟";
                    break;
                case "L2CacheSize":
                    strCH = "二级缓存";
                    break;
                case "L2CacheSpeed":
                    strCH = "二级缓存速度";
                    break;
                case "Level":
                    strCH = "级别";
                    break;
                case "LoadPercentage":
                    strCH = "符合百分比";
                    break;
                case "Manufacturer":
                    strCH = "制造商";
                    break;
                case "MaxClockSpeed":
                    strCH = "最大时钟速度";
                    break;
                case "Name":
                    strCH = "名称";
                    break;
                case "PowerManagementSupported":
                    strCH = "电源管理支持";
                    break;
                case "ProcessorId":
                    strCH = "处理器号码";
                    break;
                case "ProcessorType":
                    strCH = "处理器类型";
                    break;
                case "Role":
                    strCH = "类型";
                    break;
                case "SocketDesignation":
                    strCH = "插槽名称";
                    break;
                case "Status":
                    strCH = "状态";
                    break;
                case "StatusInfo":
                    strCH = "状态信息";
                    break;
                case "Stepping":
                    strCH = "分级";
                    break;
                case "SystemCreationClassName":
                    strCH = "系统创造类名称";
                    break;
                case "SystemName":
                    strCH = "系统名称";
                    break;
                case "UpgradeMethod":
                    strCH = "升级方法";
                    break;
                case "Version":
                    strCH = "型号";
                    break;
                case "Family":
                    strCH = "家族";
                    break;
                case "Revision":
                    strCH = "修订版本号";
                    break;
                case "PoweredOn":
                    strCH = "电源开关";
                    break;
                case "Product":
                    strCH = "产品";
                    break;
                    #endregion
            }
            if (strCH == "")//如果汉语字符串为空
                strCH = str;//直接显示英语字符串
            return strCH;//返回汉语字符串
        }

        /// <summary>
        /// 获取硬件相关的一些信息
        /// </summary>
        /// <param name="Key">要查找的</param>
        /// <param name="lst">显示信息的ListView组件</param>
        /// <param name="DontInsertNull">标识是否有信息</param>
        public void insertInfo(string Key, ref ListView lst, bool DontInsertNull)
        {
            lst.Items.Clear();//清空列表
            //创建ManagementObjectSearcher对象，使其查找参数Key的内容
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + Key);
            try
            {
                //遍历ManagementObjectSearcher对象查找的内容
                foreach (ManagementObject share in searcher.Get())
                {
                    ListViewGroup grp;//创建ListViewGroup对象
                    try
                    {
                        //设置组标题
                        grp = lst.Groups.Add(share["Name"].ToString(), share["Name"].ToString());
                    }
                    catch
                    {
                        grp = lst.Groups.Add(share.ToString(), share.ToString());
                    }
                    //如果没有查找到信息，则弹出提示
                    if (share.Properties.Count <= 0)
                    {
                        MessageBox.Show("No Information Available", "No Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    foreach (PropertyData PC in share.Properties)//遍历获取到的数据
                    {
                        ListViewItem item = new ListViewItem(grp);//将组添加到ListViewItem中
                        item.BackColor = Color.FromArgb(21, 49, 63);//设置每行的背景颜色
                        item.Text = EngtoCH(PC.Name); //设置项目标题
                        if (PC.Value != null && PC.Value.ToString() != "")
                        {
                            switch (PC.Value.GetType().ToString())//判断值的类型
                            {
                                case "System.String[]"://如果是字符串数组
                                    string[] str = (string[])PC.Value;//记录属性值
                                    string str2 = "";//定义变量，用来记录数组中存储的所有属性值
                                    foreach (string st in str)//遍历数组
                                        str2 += st + " ";//中间用空格分隔，记录所有值
                                    item.SubItems.Add(str2);//添加到列表项
                                    break;
                                case "System.UInt16[]"://如果是整型数组
                                    ushort[] shortData = (ushort[])PC.Value;
                                    string tstr2 = "";
                                    foreach (ushort st in shortData)
                                        tstr2 += st.ToString() + " ";
                                    item.SubItems.Add(tstr2);
                                    break;
                                default:
                                    item.SubItems.Add(PC.Value.ToString());//直接添加到列表项中
                                    break;
                            }
                        }
                        else
                        {
                            if (!DontInsertNull)//如果没有信息，则添加“没有信息”的提示
                                item.SubItems.Add("没有信息");
                            else
                                continue;
                        }
                        lst.Items.Add(item); //将内容添加到ListView控件中
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 在ListView组件中显示数据
        /// </summary>
        /// <param name="info">要显示的数据</param>
        /// <param name="remark">列表项名称</param>
        /// <param name="lv">ListView组件</param>
        private void showInfo(string[] info, string remark, ListView lv)
        {
            ListViewItem item = new ListViewItem(info, remark);//添加到列表项
            lv.Items.Add(item);//显示列表
        }

        /// <summary>
        /// 获取Widnows信息
        /// </summary>
        /// <param name="lv">显示Windows信息的ListView组件</param>
        public void getInfo(ListView lv)
        {
            string[] info = new string[2];//定义一个字符串数组，用来存储Windows相关的信息
            info[0] = "操作系统";//项名称
            info[1] = Environment.OSVersion.VersionString;//操作系统版本
            showInfo(info, "操作系统", lv);//调用自定义方法显示数据
            string strUser = "";
            try
            {
                RegistryKey mykey = Registry.LocalMachine;//获取注册表中的本地机器项
                mykey = mykey.CreateSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");//定位注册表子项
                strUser = (string)mykey.GetValue("RegisteredOrganization");//获取指定注册表项的值
                mykey.Close();//关闭注册表
            }
            catch
            { }
            info[0] = "注册用户";
            info[1] = strUser;//注册用户
            showInfo(info, "注册用户", lv);

            info[0] = "Windows文件夹";
            info[1] = Environment.GetEnvironmentVariable("WinDir");//Windows文件夹
            showInfo(info, "Windows文件夹", lv);

            info[0] = "系统文件夹";
            info[1] = Environment.SystemDirectory.ToString();//系统文件夹
            showInfo(info, "系统文件夹", lv);

            info[0] = "计算机名称";
            info[1] = Environment.MachineName.ToString();//计算机名称
            showInfo(info, "计算机名称", lv);

            info[0] = "本地日期时间";
            info[1] = DateTime.Now.ToString();//本地日期时间
            showInfo(info, "本地日期时间", lv);

            string strIDate = "";//定义变量，记录系统安装日期
            string strTime = "";//定义变量，记录系统启动时间
            //从WMI中查询操作系统相关信息
            ManagementObjectSearcher MySearcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject MyObject in MySearcher.Get())//遍历查询结果
            {
                strIDate += MyObject["InstallDate"].ToString().Substring(0, 8);//获取系统安装日期
                strTime += MyObject["LastBootUpTime"].ToString().Substring(0, 8);//获取最后启动时间
            }
            strIDate = strIDate.Insert(4, "-");//对日期格式进行处理
            strIDate = strIDate.Insert(7, "-");
            info[0] = "系统安装日期";
            info[1] = strIDate;//系统安装日期
            showInfo(info, "系统安装日期", lv);

            strTime = strTime.Insert(4, "-");//对时间格式进行处理
            strTime = strTime.Insert(7, "-");
            info[0] = "系统启动时间";
            info[1] = strTime;//系统启动时间
            showInfo(info, "系统启动时间", lv);

            Microsoft.VisualBasic.Devices.Computer My = new Microsoft.VisualBasic.Devices.Computer();
            info[0] = "物理内存总量(M)";
            info[1] = (My.Info.TotalPhysicalMemory / 1024 / 1024).ToString();//物理内存总量(M)
            showInfo(info, "物理内存总量(M)", lv);

            info[0] = "虚拟内存总量(M)";
            info[1] = (My.Info.TotalVirtualMemory / 1024 / 1024).ToString();//虚拟内存总量(M)
            showInfo(info, "虚拟内存总量(M)", lv);

            info[0] = "可用物理内存总量(M)";
            info[1] = (My.Info.AvailablePhysicalMemory / 1024 / 1024).ToString();//可用物理内存总量(M)
            showInfo(info, "可用物理内存总量(M)", lv);

            info[0] = "可用虚拟内存总量(M)";
            info[1] = (My.Info.AvailableVirtualMemory / 1024 / 1024).ToString();//可用虚拟内存总量(M)
            showInfo(info, "可用虚拟内存总量(M)", lv);

            info[0] = "系统驱动器";
            info[1] = Environment.GetEnvironmentVariable("SystemDrive");//系统驱动器
            showInfo(info, "系统驱动器", lv);

            info[0] = "桌面目录";
            info[1] = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//桌面目录
            showInfo(info, "桌面目录", lv);

            info[0] = "用户程序组目录";
            info[1] = Environment.GetFolderPath(Environment.SpecialFolder.Programs);//用户程序组目录
            showInfo(info, "用户程序组目录", lv);

            info[0] = "收藏夹目录";
            info[1] = Environment.GetFolderPath(Environment.SpecialFolder.Favorites);//收藏夹目录
            showInfo(info, "收藏夹目录", lv);

            info[0] = "Internet历史记录";
            info[1] = Environment.GetFolderPath(Environment.SpecialFolder.History);//Internet历史记录
            showInfo(info, "Internet历史记录", lv);

            info[0] = "Internet临时文件";
            info[1] = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);//Internet临时文件
            showInfo(info, "Internet临时文件", lv);
        }

        /// <summary>
        /// 获取指定路径下所有文件及其图标
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="imglist">存储文件及其图标</param>
        /// <param name="lv">要显示的ListView控件</param>
        public void getListViewItem(string path, ImageList imglist, ListView lv)
        {
            lv.Items.Clear();//清空ListView控件
            class_Win32.SHFILEINFO shfi = new class_Win32.SHFILEINFO();
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);
                for (int i = 0; i < dirs.Length; i++)
                {
                    string[] info = new string[4];
                    DirectoryInfo dir = new DirectoryInfo(dirs[i]);
                    if (dir.Name == "RECYCLER" || dir.Name == "RECYCLED" || dir.Name == "Recycled" || dir.Name == "System Volume Information")
                    { }
                    else
                    {
                        //获得图标
                        class_Win32.SHGetFileInfo(dirs[i],
                                            (uint)0x80,
                                            ref shfi,
                                            (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
                                            (uint)(0x100 | 0x400)); //取得Icon和TypeName
                        //添加图标
                        imglist.Images.Add(dir.Name, (Icon)Icon.FromHandle(shfi.hIcon).Clone());
                        info[0] = dir.Name.Remove(dir.Name.LastIndexOf("."));
                        info[1] = "";
                        info[2] = "文件夹";
                        info[3] = dir.LastWriteTime.ToString();
                        ListViewItem item = new ListViewItem(info, dir.Name);
                        lv.Items.Add(item);
                        //销毁图标
                        class_Win32.DestroyIcon(shfi.hIcon);
                    }
                }
                for (int i = 0; i < files.Length; i++)
                {
                    string[] info = new string[4];
                    FileInfo fi = new FileInfo(files[i]);
                    string Filetype = fi.Name.Substring(fi.Name.LastIndexOf(".") + 1, fi.Name.Length - fi.Name.LastIndexOf(".") - 1);
                    string newtype = Filetype.ToLower();
                    if (newtype == "sys" || newtype == "ini" || newtype == "bin" || newtype == "log" || newtype == "com" || newtype == "bat" || newtype == "db")
                    { }
                    else
                    {
                        //获得图标
                        class_Win32.SHGetFileInfo(files[i],
                                            (uint)0x80,
                                            ref shfi,
                                            (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
                                            (uint)(0x100 | 0x400)); //取得Icon和TypeName
                        //添加图标
                        imglist.Images.Add(fi.Name, (Icon)Icon.FromHandle(shfi.hIcon).Clone());
                        info[0] = fi.Name.Remove(fi.Name.LastIndexOf("."));
                        info[1] = fi.Length.ToString();
                        info[2] = fi.Extension.ToString();
                        info[3] = fi.LastWriteTime.ToString();
                        ListViewItem item = new ListViewItem(info, fi.Name);
                        lv.Items.Add(item);
                        //销毁图标
                        class_Win32.DestroyIcon(shfi.hIcon);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 获取所有进程信息
        /// </summary>
        /// <param name="lv">要显示进程信息的ListView组件</param>
        public void getProcessInfo(ListView lv)
        {
            try
            {
                lv.Items.Clear();//清空ListView列表
                Process[] MyProcesses = Process.GetProcesses();//获取所有进程
                string[] Minfo = new string[6];//定义字符串数组，用来存储进程的详细信息
                foreach (Process MyProcess in MyProcesses)//遍历所有进程
                {
                    Minfo[0] = MyProcess.ProcessName;//进程名称
                    Minfo[1] = MyProcess.Id.ToString();//进程ID
                    Minfo[2] = MyProcess.Threads.Count.ToString();//线程数
                    Minfo[3] = MyProcess.BasePriority.ToString();//优先级
                    Minfo[4] = Convert.ToString(MyProcess.WorkingSet64 / 1024) + "K";//物理内存
                    Minfo[5] = Convert.ToString(MyProcess.VirtualMemorySize64 / 1024) + "K";//虚拟内存
                    ListViewItem lvItem = new ListViewItem(Minfo, "process");//将进程信息数组添加到列表项中
                    lv.Items.Add(lvItem);//显示列表
                }
            }
            catch { }
        }

        /// <summary>
        /// 获取正在运行的所有程序信息
        /// </summary>
        /// <param name="lv">要显示程序信息的ListView组件</param>
        public void getWindowsInfo(ListView lv)
        {
            try
            {
                lv.Items.Clear();//清空ListView列表
                Process[] MyProcesses = Process.GetProcesses();//获取所有进程
                string[] Minfo = new string[4];//定义字符串数组，用来存储程序的详细信息
                foreach (Process MyProcess in MyProcesses)//遍历所有进程
                {
                    if (MyProcess.MainWindowTitle.Length > 0)//判断程序是否具有主窗口标题
                    {
                        Minfo[0] = MyProcess.MainWindowTitle;//窗口标题
                        Minfo[1] = MyProcess.Id.ToString();//进程ID
                        Minfo[2] = MyProcess.ProcessName;//进程名称
                        Minfo[3] = MyProcess.StartTime.ToString();//启动时间
                        ListViewItem lvItem = new ListViewItem(Minfo, "process");//将程序信息数组添加到列表项中
                        lv.Items.Add(lvItem);//显示列表
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 清空指定文件夹
        /// </summary>
        /// <param name="path">要清空的文件夹路径</param>
        private void clearFolder(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);//根据指定路径创建文件夹对象
            if (dir.Exists)//判断文件夹是否存在
            {
                dir.Delete(true);//删除文件夹及其子文件夹
                dir.Create();//重新创建文件夹
            }
        }

        /// <summary>
        /// 系统垃圾清理
        /// </summary>
        /// <param name="handle">窗口句柄，在清空回收站时使用</param>
        /// <param name="str">要清理的项</param>
        public void clearSystem(IntPtr handle, string str)
        {
            string dir = "";//定义一个变量，用来存储要清空的文件夹路径
            RegistryKey currentReg = Registry.CurrentUser;//获取注册表中的当前用户项
            try
            {
                switch (str)
                {
                    case "清空回收站":
                        class_Win32.SHEmptyRecycleBin(handle, 0, 7);//调用API函数清空回收站
                        break;
                    case "清空IE缓存区":
                        dir = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);//获取IE缓存区路径
                        clearFolder(dir);//调用方法清空IE缓存区
                        break;
                    case "清空IE　Cookies":
                        dir = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);//获取IE Cookies存储路径
                        clearFolder(dir);//调用方法清空IE　Cookies
                        break;
                    case "清空Windows临时文件夹":
                        dir = Environment.GetEnvironmentVariable("WinDir") + "\\Temp";//获取系统目录下的临时文件夹路径
                        clearFolder(dir);//调用方法清空Windows临时文件夹
                        dir = Environment.GetEnvironmentVariable("TEMP");//获取环境变量文件夹路径
                        clearFolder(dir);//调用方法清空环境变量文件夹
                        break;
                    case "清空打开的文件记录":
                        dir = Environment.GetFolderPath(Environment.SpecialFolder.Recent);//获取最近打开的文件记录存储路径
                        clearFolder(dir);//调用方法清空打开的文件记录
                        break;
                    case "清除IE地址栏中的历史网址":
                        RegistryKey software = currentReg.OpenSubKey(@"Software\Microsoft\Internet Explorer", true);//获取IE注册表项
                        software.DeleteSubKeyTree("TypedURLs");//清除IE地址栏中的历史网址
                        break;
                    case "清除运行对话框":
                        currentReg = currentReg.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\RunMRU");//获取运行对话框注册表项
                        string MyMRU = (String)currentReg.GetValue("MRULIST");//获取运行对话框的记录
                        for (int i = 0; i < MyMRU.Length; i++)//遍历
                        {
                            currentReg.DeleteValue(MyMRU[i].ToString());//删除注册表项
                        }
                        currentReg.SetValue("MRUList", "");//设置运行对话框的记录为空
                        break;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 以命令窗口形势执行系统操作
        /// </summary>
        /// <param name="cmd">字符串，表示要执行的命令</param>
        public void cmdOperator(string cmd)
        {
            Process myProcess = new Process();//创建进程对象
            myProcess.StartInfo.FileName = "cmd.exe";//设置打开cmd命令窗口
            myProcess.StartInfo.UseShellExecute = false;//不使用操作系统shell启动进程的值
            myProcess.StartInfo.RedirectStandardInput = true;//设置可以从标准输入流读取值
            myProcess.StartInfo.RedirectStandardOutput = true;//设置可以向标准输出流写入值
            myProcess.StartInfo.RedirectStandardError = true;//设置可以显示输入输出流中出现的错误
            myProcess.StartInfo.CreateNoWindow = true;//设置在新窗口中启动进程
            myProcess.Start();//启动进程
            myProcess.StandardInput.WriteLine(cmd);//传入要执行的命令
        }

        /// <summary>
        /// 通过在注册表的当前用户节点下创建子键，优化系统
        /// </summary>
        /// <param name="regkey">子键名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="flag">标识，用来确定是否有第3个参数</param>
        public void currentRegOptimize(string regkey, string key, object value, int flag)
        {
            try
            {
                RegistryKey reg = Registry.CurrentUser;//获取注册表中的当前用户节点
                reg = reg.CreateSubKey(regkey);//创建子键
                if (flag == 0)//判断标识参数是否为0
                    reg.SetValue(key, value);//设置注册表项值
                else
                    reg.SetValue(key, value, RegistryValueKind.DWord);//设置注册表项值
                reg.Close();
            }
            catch { }
        }

        /// <summary>
        /// 通过在注册表的本地机器节点下创建子键，优化系统
        /// </summary>
        /// <param name="regkey">子键名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="flag">标识，用来确定是否有第3个参数</param>
        public void localRegOptimize(string regkey, string key, object value, int flag)
        {
            try
            {
                RegistryKey reg = Registry.LocalMachine; ;//获取注册表中的本地机器节点
                reg = reg.CreateSubKey(regkey);//创建子键
                if (flag == 0)//判断标识参数是否为0
                    reg.SetValue(key, value);//设置注册表项值
                else
                    reg.SetValue(key, value, RegistryValueKind.DWord);//设置注册表项值
                reg.Close();
            }
            catch { }
        }
    }

}
