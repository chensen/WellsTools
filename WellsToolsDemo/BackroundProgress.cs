using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WellsToolsDemo
{
    public partial class BackroundProgress : Form
    {
        private Form2 notifyForm = new Form2();

        public BackroundProgress()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            bkWorker.WorkerReportsProgress = true;
            bkWorker.WorkerSupportsCancellation = true;
            bkWorker.DoWork += new DoWorkEventHandler(DoWork);
            bkWorker.ProgressChanged += new ProgressChangedEventHandler(ProgessChanged);
            bkWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompleteWork);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            notifyForm.StartPosition = FormStartPosition.CenterParent;

            bkWorker.RunWorkerAsync();
            notifyForm.ShowDialog();
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            // 事件处理，指定处理函数  
            e.Result = ProcessProgress(bkWorker, e);
        }

        public void ProgessChanged(object sender, ProgressChangedEventArgs e)
        {
            // bkWorker.ReportProgress 会调用到这里，此处可以进行自定义报告方式  
            notifyForm.SetNotifyInfo(e.ProgressPercentage, "处理进度:" + Convert.ToString(e.ProgressPercentage) + "%");
        }

        public void CompleteWork(object sender, RunWorkerCompletedEventArgs e)
        {
            notifyForm.Close();
            MessageBox.Show("处理完毕!");
        }

        private int ProcessProgress(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 1000; i++)
            {
                if (bkWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return -1;
                }
                else
                {
                    // 状态报告  
                    bkWorker.ReportProgress(i / 10);

                    // 等待，用于UI刷新界面，很重要  
                    System.Threading.Thread.Sleep(10);
                }
            }

            return -1;
        }
    }
}
