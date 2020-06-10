using hvppleDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wells.Controls.ImageDocEx;

namespace WellsToolsDemo
{
    public partial class ImageDocExDemo : Form
    {
        public List<ROI> m_roiList = new List<ROI>();
        public ImageDocExDemo()
        {
            InitializeComponent();
            imgDoc.updateROI(m_roiList);
            imgDoc.qtHMouseMove = new ImageDocEx.HMouseMove(qtHMouseMove);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HObject obj;
            HOperatorSet.ReadImage(out obj, "D:\\test.jpg");
            imgDoc.setImage(obj);
            obj.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ROI rOI = new ROIRectangle1(15000, 4000, 3000, 3000);
            m_roiList.Add(rOI);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //imgDoc.StaticWnd = true;
            button3.Enabled = false;
            HTuple row1, row2, col1, col2;
            HOperatorSet.SetColor(imgDoc.HWindow, "red");
            HOperatorSet.DrawRectangle1(imgDoc.HWindow, out row1, out col1, out row2, out col2);
            //imgDoc.StaticWnd = false;
            button3.Enabled = true;
            //Wells.FrmType.frm_Log.ShowDlg(true);
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            
        }

        public void qtHMouseMove(string str)
        {
            label1.Text = str;
        }
    }
}
