using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Wells.Tools
{
    /// <summary>
    /// IniFiles的类
    /// </summary>
    public class clsIni
    {
        public string FileName; //INI文件名
        //string path   =   System.IO.Path.Combine(Application.StartupPath,"pos.ini");

        //声明读写INI文件的API函数
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        //类的构造函数，传递INI文件名
        public clsIni(string AFileName)
        {
            // 判断文件是否存在
            FileInfo fileInfo = new FileInfo(AFileName);
            //Todo:搞清枚举的用法
            if ((!fileInfo.Exists))
            { //|| (FileAttributes.Directory in fileInfo.Attributes))
              //文件不存在，建立文件
                System.IO.StreamWriter sw = new System.IO.StreamWriter(AFileName, false, System.Text.Encoding.Default);
                try
                {
                    sw.Write(clsWellsLanguage.getString(115));
                    sw.Close();
                }
                catch
                {
                    throw (new ApplicationException(string.Format(clsWellsLanguage.getString(116), AFileName)));
                }
            }
            //必须是完全路径，不能是相对路径
            FileName = fileInfo.FullName;
        }

        //写INI文件
        public void writeString(string Section, string Ident, string Value)
        {
            if (!WritePrivateProfileString(Section, Ident, Value, FileName))
            {
                Wells.FrmType.frm_Log.Log(string.Format(clsWellsLanguage.getString(117),Section,Ident,Value), 2, 0);
            }
        }

        //读取INI文件指定
        public string readString(string Section, string Ident, string Default)
        {
            Byte[] Buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), FileName);
            //必须设定0（系统默认的代码页）的编码方式，否则无法支持中文
            string s = Encoding.GetEncoding(0).GetString(Buffer, 0, bufLen);
            //s = s.Substring(0, bufLen);
            return s.Trim();
        }

        //读整数
        public int readInteger(string Section, string Ident, int Default)
        {
            string intStr = readString(Section, Ident, Convert.ToString(Default));
            try
            {
                return Convert.ToInt32(intStr);
            }
            catch (Exception ex)
            {
                Wells.FrmType.frm_Log.Log(string.Format(clsWellsLanguage.getString(118),Section,Ident), 2, 0);
                return Default;
            }
        }

        //写整数
        public void writeInteger(string Section, string Ident, int Value)
        {
            writeString(Section, Ident, Value.ToString());
        }
        
        //读整数
        public long readInteger64(string Section, string Ident, long Default)
        {
            string intStr = readString(Section, Ident, Convert.ToString(Default));
            try
            {
                return Convert.ToInt64(intStr);
            }
            catch (Exception ex)
            {
                Wells.FrmType.frm_Log.Log(string.Format(clsWellsLanguage.getString(118), Section, Ident), 2, 0);
                return Default;
            }
        }
        
        //写整数
        public void writeInteger64(string Section, string Ident, long Value)
        {
            writeString(Section, Ident, Value.ToString());
        }

        //写浮点数
        public double readDouble(string Section, string Ident, double Default)
        {
            string intStr = readString(Section, Ident, Convert.ToString(Default));
            try
            {
                return Convert.ToDouble(intStr);
            }
            catch (Exception exc)
            {
                Wells.FrmType.frm_Log.Log(string.Format(clsWellsLanguage.getString(118), Section, Ident), 2, 0);
                return Default;
            }
        }

        public void writeDouble(string Section, string Ident, double Value)
        {
            writeString(Section, Ident, Value.ToString());
        }

        //写浮点数
        public float readFloat(string Section, string Ident, float Default)
        {
            string intStr = readString(Section, Ident, Convert.ToString(Default));
            try
            {
                return (float)Convert.ToDouble(intStr);
            }
            catch (Exception exc)
            {
                Wells.FrmType.frm_Log.Log(string.Format(clsWellsLanguage.getString(118), Section, Ident), 2, 0);
                return Default;
            }
        }

        public void writeFloat(string Section, string Ident, float Value)
        {
            writeString(Section, Ident, Value.ToString());
        }

        //读布尔
        public bool readBool(string Section, string Ident, bool Default)
        {
            try
            {
                return Convert.ToBoolean(readString(Section, Ident, Convert.ToString(Default)));
            }
            catch (Exception ex)
            {
                Wells.FrmType.frm_Log.Log(string.Format(clsWellsLanguage.getString(118), Section, Ident), 2, 0);
                return Default;
            }
        }

        //写Bool
        public void writeBool(string Section, string Ident, bool Value)
        {
            writeString(Section, Ident, Convert.ToString(Value));
        }

        //从Ini文件中，将指定的Section名称中的所有Ident添加到列表中
        public void readSection(string Section, StringCollection Idents)
        {
            Byte[] Buffer = new Byte[16384];
            //Idents.Clear();

            int bufLen = GetPrivateProfileString(Section, null, null, Buffer, Buffer.GetUpperBound(0), FileName);
            //对Section进行解析
            getStringsFromBuffer(Buffer, bufLen, Idents);
        }

        private void getStringsFromBuffer(Byte[] Buffer, int bufLen, StringCollection Strings)
        {
            Strings.Clear();
            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((Buffer[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding(0).GetString(Buffer, start, i - start);
                        Strings.Add(s);
                        start = i + 1;
                    }
                }
            }
        }

        //从Ini文件中，读取所有的Sections的名称
        public void readSections(StringCollection SectionList)
        {
            //Note:必须得用Bytes来实现，StringBuilder只能取到第一个Section
            byte[] Buffer = new byte[65535];
            int bufLen = 0;
            bufLen = GetPrivateProfileString(null, null, null, Buffer,
            Buffer.GetUpperBound(0), FileName);
            getStringsFromBuffer(Buffer, bufLen, SectionList);
        }

        //读取指定的Section的所有Value到列表中
        public void readSectionValues(string Section, NameValueCollection Values)
        {
            StringCollection KeyList = new StringCollection();
            readSection(Section, KeyList);
            Values.Clear();
            foreach (string key in KeyList)
            {
                Values.Add(key, readString(Section, key, ""));
            }
        }
        
        ////读取指定的Section的所有Value到列表中，
        //public void ReadSectionValues(string Section, NameValueCollection Values,char splitString)
        //{　 string sectionValue;
        //　　string[] sectionValueSplit;
        //　　StringCollection KeyList = new StringCollection();
        //　　ReadSection(Section, KeyList);
        //　　Values.Clear();
        //　　foreach (string key in KeyList)
        //　　{
        //　　　　sectionValue=ReadString(Section, key, "");
        //　　　　sectionValueSplit=sectionValue.Split(splitString);
        //　　　　Values.Add(key, sectionValueSplit[0].ToString(),sectionValueSplit[1].ToString());
        //　　}
        //}

        //清除某个Section
        public void eraseSection(string Section)
        {
            if (!WritePrivateProfileString(Section, null, null, FileName))
            {
                Wells.FrmType.frm_Log.Log(clsWellsLanguage.getString(119) + Section + "！", 2, 0);
            }
        }

        //删除某个Section下的键
        public void deleteKey(string Section, string Ident)
        {
            WritePrivateProfileString(Section, Ident, null, FileName);
        }

        //Note:对于Win9X，来说需要实现UpdateFile方法将缓冲中的数据写入文件
        //在Win NT, 2000和XP上，都是直接写文件，没有缓冲，所以，无须实现UpdateFile
        //执行完对Ini文件的修改之后，应该调用本方法更新缓冲区。
        public void updateFile()
        {
            WritePrivateProfileString(null, null, null, FileName);
        }

        //检查某个Section下的某个键值是否存在
        public bool existsValue(string Section, string Ident)
        {
            StringCollection Idents = new StringCollection();
            readSection(Section, Idents);
            return Idents.IndexOf(Ident) > -1;
        }

        //确保资源的释放
        ~clsIni()
        {
            updateFile();
        }
    }

    public class clsIniUnicode
    {
        public static string path = Application.StartupPath + "\\FQCTestSystem.ini";

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public void writeString(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, path);
        }

        public string readString(string Section, string Key)
        {
            StringBuilder stringBuilder = new StringBuilder(255);
            int privateProfileString = GetPrivateProfileString(Section, Key, "", stringBuilder, 255, path);
            string ret = stringBuilder.ToString();

            return myTrimString(ret);
        }

        public static string myTrimString(string ret)
        {
            ret = ret.Replace(" ", "");
            ret = ret.Replace("\0", "");
            ret = ret.Replace("\r", "");
            ret = ret.Replace("\n", "");
            ret = ret.Replace("\r\n", "");
            ret = ret.Replace("\n\r", "");
            return ret;
        }
    }
}