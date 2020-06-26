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
    /// IniFiles����
    /// </summary>
    public class clsIni
    {
        public string FileName; //INI�ļ���
        //string path   =   System.IO.Path.Combine(Application.StartupPath,"pos.ini");

        //������дINI�ļ���API����
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        //��Ĺ��캯��������INI�ļ���
        public clsIni(string AFileName)
        {
            // �ж��ļ��Ƿ����
            FileInfo fileInfo = new FileInfo(AFileName);
            //Todo:����ö�ٵ��÷�
            if ((!fileInfo.Exists))
            { //|| (FileAttributes.Directory in fileInfo.Attributes))
              //�ļ������ڣ������ļ�
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
            //��������ȫ·�������������·��
            FileName = fileInfo.FullName;
        }

        //дINI�ļ�
        public void writeString(string Section, string Ident, string Value)
        {
            if (!WritePrivateProfileString(Section, Ident, Value, FileName))
            {
                Wells.FrmType.frm_Log.Log(string.Format(clsWellsLanguage.getString(117),Section,Ident,Value), 2, 0);
            }
        }

        //��ȡINI�ļ�ָ��
        public string readString(string Section, string Ident, string Default)
        {
            Byte[] Buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), FileName);
            //�����趨0��ϵͳĬ�ϵĴ���ҳ���ı��뷽ʽ�������޷�֧������
            string s = Encoding.GetEncoding(0).GetString(Buffer, 0, bufLen);
            //s = s.Substring(0, bufLen);
            return s.Trim();
        }

        //������
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

        //д����
        public void writeInteger(string Section, string Ident, int Value)
        {
            writeString(Section, Ident, Value.ToString());
        }
        
        //������
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
        
        //д����
        public void writeInteger64(string Section, string Ident, long Value)
        {
            writeString(Section, Ident, Value.ToString());
        }

        //д������
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

        //д������
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

        //������
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

        //дBool
        public void writeBool(string Section, string Ident, bool Value)
        {
            writeString(Section, Ident, Convert.ToString(Value));
        }

        //��Ini�ļ��У���ָ����Section�����е�����Ident��ӵ��б���
        public void readSection(string Section, StringCollection Idents)
        {
            Byte[] Buffer = new Byte[16384];
            //Idents.Clear();

            int bufLen = GetPrivateProfileString(Section, null, null, Buffer, Buffer.GetUpperBound(0), FileName);
            //��Section���н���
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

        //��Ini�ļ��У���ȡ���е�Sections������
        public void readSections(StringCollection SectionList)
        {
            //Note:�������Bytes��ʵ�֣�StringBuilderֻ��ȡ����һ��Section
            byte[] Buffer = new byte[65535];
            int bufLen = 0;
            bufLen = GetPrivateProfileString(null, null, null, Buffer,
            Buffer.GetUpperBound(0), FileName);
            getStringsFromBuffer(Buffer, bufLen, SectionList);
        }

        //��ȡָ����Section������Value���б���
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
        
        ////��ȡָ����Section������Value���б��У�
        //public void ReadSectionValues(string Section, NameValueCollection Values,char splitString)
        //{�� string sectionValue;
        //����string[] sectionValueSplit;
        //����StringCollection KeyList = new StringCollection();
        //����ReadSection(Section, KeyList);
        //����Values.Clear();
        //����foreach (string key in KeyList)
        //����{
        //��������sectionValue=ReadString(Section, key, "");
        //��������sectionValueSplit=sectionValue.Split(splitString);
        //��������Values.Add(key, sectionValueSplit[0].ToString(),sectionValueSplit[1].ToString());
        //����}
        //}

        //���ĳ��Section
        public void eraseSection(string Section)
        {
            if (!WritePrivateProfileString(Section, null, null, FileName))
            {
                Wells.FrmType.frm_Log.Log(clsWellsLanguage.getString(119) + Section + "��", 2, 0);
            }
        }

        //ɾ��ĳ��Section�µļ�
        public void deleteKey(string Section, string Ident)
        {
            WritePrivateProfileString(Section, Ident, null, FileName);
        }

        //Note:����Win9X����˵��Ҫʵ��UpdateFile�����������е�����д���ļ�
        //��Win NT, 2000��XP�ϣ�����ֱ��д�ļ���û�л��壬���ԣ�����ʵ��UpdateFile
        //ִ�����Ini�ļ����޸�֮��Ӧ�õ��ñ��������»�������
        public void updateFile()
        {
            WritePrivateProfileString(null, null, null, FileName);
        }

        //���ĳ��Section�µ�ĳ����ֵ�Ƿ����
        public bool existsValue(string Section, string Ident)
        {
            StringCollection Idents = new StringCollection();
            readSection(Section, Idents);
            return Idents.IndexOf(Ident) > -1;
        }

        //ȷ����Դ���ͷ�
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