using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Wells.FrmType
{
    public partial class frm_Loading : Form
    {
        private static object obj = new object();
        private static frm_Loading m_instance = null;

        private int iCount = 0;
        private int iMaxTime = 10;
        private double delta = 0.1;
        private bool bIsShown = false;
        private bool bIsStop = false;
        
        private static frm_LoadingCallBack m_lpfnCallback;//备用回调
        private delegate void frm_LoadingCallBack(object obj);

        public static frm_Loading m_frm_Loading
        {
            get
            {
                if (m_instance == null)
                {
                    lock (obj)
                    {
                        if (m_instance == null)
                            m_instance = new frm_Loading();
                    }
                }
                return m_instance;
            }
            set
            {
                m_instance = null;
            }
        }
        public frm_Loading()
        {
            InitializeComponent();
        }

        public void SetMaxWaitTime(int iMaxTime)
        {
            this.iMaxTime = iMaxTime;
        }
        
        void RegisterCallback(frm_LoadingCallBack lpfnCallback)
        {
            m_lpfnCallback = lpfnCallback;
        }

        public void Start()
        {
            this.Opacity = 0.0;
            tm_Open.Start();
            tm_Work.Start();
            bIsShown = true;
            this.ShowDialog();
        }

        public void Wait()
        {
            do 
            {
                Thread.Sleep(1);
            } while (Interlocked.Equals(bIsShown,true));
        }

        public void Stop()
        {
            bIsStop = true;
        }

        private void tm_Open_Tick(object sender, EventArgs e)
        {
            this.Opacity += 0.05;
            if (this.Opacity >= 1.0)
                tm_Open.Stop();
        }

        private void tm_Work_Tick(object sender, EventArgs e)
        {
            tm_Work.Stop();
            iCount++;
            if(bIsStop)
            {
                int temp = (int)(95 * (iMaxTime * 1000 / 100) / 100.0);
                iCount = iCount < temp ? temp : iCount;
                bIsStop = false;
            }
            int value = (iCount * 100) / (iMaxTime * 1000 / 100);
            if (value < 0 || value > 100)
            {
                tm_Work.Stop();
                tm_Open.Stop();
                if (this.Opacity < 1.0)
                    this.Opacity = 1.0;
                tm_Close.Start();
            }
            else
            {
                proBar_Process.Value = value;
                label_percent.Text = string.Format("{0}%", value);
                tm_Work.Start();
            }
        }

        private void tm_Close_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.05;
            if(this.Opacity<=0.0)
            {
                tm_Close.Stop();
                bIsShown = false;
                bIsStop = false;
                this.Dispose();
            }
        }
    }
}
