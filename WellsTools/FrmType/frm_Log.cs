using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Wells.FrmType
{
    public partial class frm_Log : frmBaseForm
    {
        public static frm_Log pCurrentForm;
        private static long m_lShowCount = 0;
        private static long m_IsWriting = 0;
        private static long m_lCacheCount = 0;
        private StringFormat strFormat = StringFormat.GenericTypographic;
        private Mutex muAddItem = new Mutex();
        private static Mutex muLog = new Mutex();
        private static Mutex muLogCache = new Mutex();
        private List<int> listDrawMode = new List<int>();
        private Font _font = new Font("宋体", 10f, FontStyle.Bold);
        private int iShowWidth = 300;
        private List<string> listObj = new List<string>();


        public frm_Log()
        {
            InitializeComponent();
            strFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            CheckForIllegalCrossThreadCalls = false;
        }

        private void libView_DrawItem(object sender, DrawItemEventArgs e)
        {
            #region 重写绘图函数
            try
            {
                if(e.Index>=0)
                {
                    string str = libView.Items[e.Index].ToString();
                    if (listDrawMode[e.Index] == 0)
                        e.Graphics.DrawString(str, libView.Font, new SolidBrush(Color.Aqua), e.Bounds);
                    else if(listDrawMode[e.Index] == 1)
                        e.Graphics.DrawString(str, libView.Font, new SolidBrush(Color.White), e.Bounds);
                    else if (listDrawMode[e.Index] == 2)
                        e.Graphics.DrawString(str, libView.Font, new SolidBrush(Color.Tomato), e.Bounds);
                    else
                        e.Graphics.DrawString(str, libView.Font, new SolidBrush(Color.White), e.Bounds);
                }
            }
            catch (Exception exc)
            {
            	
            }
            #endregion
        }

        private void AddItem(string item,int iDrawMode,string time = "")
        {
            #region 往listbox中添加一项数据
            muAddItem.WaitOne();
            try
            {
                if (m_lShowCount >= 500)
                    btnClearLog_Click(null, null);
                libView.BeginUpdate();
                listDrawMode.Add(iDrawMode);
                Graphics graphics = libView.CreateGraphics();
                graphics.PageUnit = GraphicsUnit.Pixel;
                string strAdd = time == "" ? ("[" + GetTimeNow(3) + "]" + item) : ("[" + time + "]" + item);
                SizeF size = graphics.MeasureString(strAdd, libView.Font, 2000, strFormat);
                //if (size.Width > (float)(iShowWidth - 120))
                //{
                //    iShowWidth = (int)(size.Width + 120);
                //    libView.HorizontalExtent = iShowWidth;
                //}
                if (size.Width > (float)(libView.Width))
                {
                    //iShowWidth = (int)(size.Width + 120);
                    libView.HorizontalExtent = (int)size.Width + 200;
                }
                libView.Items.Add(strAdd);
                m_lShowCount++;
                libView.SelectedIndex = libView.Items.Count - 1;
                libView.EndUpdate();
            }
            catch (Exception exc)
            {
                libView.EndUpdate();
            }
            finally
            {
                muAddItem.ReleaseMutex();
            }
            #endregion
        }

        public static string GetTimeNow(int k)
        {
            #region 获取当前时间
            string result;
            if (k == 1)
            {
                result = string.Concat(new string[]
                {
                    DateTime.Now.Year.ToString("0000"),
                    "-",
                    DateTime.Now.Month.ToString("00"),
                    "-",
                    DateTime.Now.Day.ToString("00")
                });
            }
            else if (k == 2)
            {
                result = string.Concat(new string[]
                {
                    DateTime.Now.Year.ToString("0000"),
                    "-",
                    DateTime.Now.Month.ToString("00"),
                    "-",
                    DateTime.Now.Day.ToString("00"),
                    "-",
                    DateTime.Now.Hour.ToString("00"),
                    "-",
                    DateTime.Now.Minute.ToString("00"),
                    "-",
                    DateTime.Now.Second.ToString("00"),
                    "-",
                    DateTime.Now.Millisecond.ToString("000"),
                });
            }
            else
            {
                result = string.Concat(new string[]
                {
                    DateTime.Now.Hour.ToString("00"),
                    "-",
                    DateTime.Now.Minute.ToString("00"),
                    "-",
                    DateTime.Now.Second.ToString("00"),
                    "-",
                    DateTime.Now.Millisecond.ToString("000")
                });
            }
            return result;
            #endregion
        }

        private void btnOpenLog_Click(object sender, EventArgs e)
        {
            #region 打开日志
            try
            {
                Process.Start(Application.StartupPath + "\\LOG");
            }
            catch (Exception exc)
            {
                WellsFramework.WellsMetroMessageBox.Show(null, exc.Message, clsWellsLanguage.getString(1));
            }
            #endregion
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            #region 清空界面显示
            m_lShowCount = 0;
            listDrawMode.Clear();
            libView.Items.Clear();
            #endregion
        }

        private void btnShowCache_Click(object sender, EventArgs e)
        {
            #region 显示缓存信息
            try
            {
                int count = listObj.Count / 3;
                if(count>0)
                {
                    for(int i=0;i<count;i++)
                    {
                        Log(listObj[i * 3], 0, 0, 1, listObj[i * 3 + 1], listObj[i * 3 + 2]);
                    }
                }
                m_lCacheCount = 0;
                listObj.Clear();
                btnShowCache.Text = clsWellsLanguage.getString(110)+"-0000";
            }
            catch (Exception exc)
            {
            	
            }
            #endregion
        }
        
        private void chkTopMost_CheckedChangeEvent(object sender, EventArgs e)
        {
            #region 窗口置顶
            base.TopMost = chkTopMost.Checked ? true : false;
            #endregion
        }

        public static void Log(string item, int iDrawMode = 0, int iShowMode = 0, int iWriteMode = 1, string timeAddItem = "", string timeWriteLog = "")
        {
            #region 全局记录函数
            muLog.WaitOne();
            try
            {
                pCurrentForm.AddItem(item, iDrawMode, timeAddItem);
                string dir = Application.StartupPath + "\\LOG";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                if(iWriteMode == 1)
                {
                    string path = dir + "\\";
                    if (iDrawMode == 0) path += GetTimeNow(1) + "-Default.txt";
                    else if (iDrawMode == 1) path += GetTimeNow(1) + "-Normal.txt";
                    else if (iDrawMode == 2) path += GetTimeNow(1) + "-Error.txt";
                    else path += GetTimeNow(1) + "-Default.txt";
                    LogFile(item, path, timeWriteLog);
                }
                MessageBoxIcon icon = MessageBoxIcon.None;
                if (iShowMode == 1) icon = MessageBoxIcon.Error;
                if (iShowMode == 2) icon = MessageBoxIcon.Question;
                if (iShowMode == 3) icon = MessageBoxIcon.Warning;
                if (iShowMode == 4) icon = MessageBoxIcon.Information;
                if (icon != MessageBoxIcon.None)
                    WellsFramework.WellsMetroMessageBox.Show(null, item, clsWellsLanguage.getString(111), MessageBoxButtons.OK, icon);
            }
            catch (Exception exc)
            {
            	
            }
            finally
            {
                muLog.ReleaseMutex();
            }
            #endregion
        }

        public static void LogFile(string item,string path, string time = "")
        {
            #region 写入文件
            try
            {
                if (Interlocked.Read(ref m_IsWriting) == 0L)
                {
                    Interlocked.Exchange(ref m_IsWriting, 1L);
                    StreamWriter streamWriter = new StreamWriter(path, true);
                    string str = time == "" ? (GetTimeNow(2) + "：") : (time + "：");
                    streamWriter.WriteLine(str + item);
                    streamWriter.Close();
                    Interlocked.Exchange(ref m_IsWriting, 0L);
                }
            }
            catch (Exception exc)
            {
            }
            #endregion
        }

        public static void LogCache(string item)
        {
            #region 全局记录函数，存放在缓存
            string dtWriteLog = GetTimeNow(2);
            string dtAddItem = GetTimeNow(3);
            muLogCache.WaitOne();
            try
            {
                pCurrentForm.listObj.Add(item);
                pCurrentForm.listObj.Add(dtAddItem);
                pCurrentForm.listObj.Add(dtWriteLog);
                m_lCacheCount++;
                if (m_lCacheCount >= 9999)
                {
                    m_lCacheCount = 0;
                    pCurrentForm.listObj.Clear();
                }
                //pCurrentForm.btnShowCache.Text = "缓存-" + m_lCacheCount.ToString("0000");
            }
            catch (Exception exc)
            {

            }
            finally
            {
                muLogCache.ReleaseMutex();
            }
            #endregion
        }

        private void frm_Log_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string str = clsWellsLanguage.getString(110) + "-" + m_lCacheCount.ToString("0000");
            if (btnShowCache.Text != str)
                btnShowCache.Text = str;
        }

        private void frm_Log_VisibleChanged(object sender, EventArgs e)
        {
            timer1.Enabled = this.Visible;
        }

        public static void InitDlg()
        {
            if (pCurrentForm == null)
                pCurrentForm = new frm_Log();
        }

        public static void ShowDlg(bool bShow)
        {
            if(pCurrentForm != null)
            {
                if (bShow)
                {
                    pCurrentForm.Show();
                    pCurrentForm.TopMost = true;
                    pCurrentForm.TopMost = false;
                }
                else
                    pCurrentForm.Close();
            }
        }

        public static bool GetDlgStatus()
        {
            return pCurrentForm.Visible;
        }
    }
}
