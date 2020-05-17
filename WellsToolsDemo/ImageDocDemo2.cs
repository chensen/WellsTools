using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wells.Controls.ImageDoc;
using Wells.Controls.VisionInspect;
using hvppleDotNet;
using System.Collections;

namespace WellsToolsDemo
{
    public partial class ImageDocDemo2 : Form
    {
        HObject[] objList = null;
        public ImageDocDemo2()
        {
            InitializeComponent();
            imageDoc1.m_pcb.initialize(55000, 55000, 8000, 8000, 2456, 2056, Point.Empty, 0, 0, true);
            HObject img = imageDoc1.m_pcb.createPcbImage();
            imageDoc1.Image = new HImage(img);
            img.Dispose();
        }

        private void ImageDocDemo2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            for (int igg = 0; igg < imageDoc1.m_pcb.m_CameraViewList.Count; igg++)
            {
                if (imageDoc1.m_pcb.m_CameraViewList[igg].m_image.hObj != null)
                    imageDoc1.m_pcb.m_CameraViewList[igg].m_image.hObj.Dispose();
                HOperatorSet.ReadImage(out imageDoc1.m_pcb.m_CameraViewList[igg].m_image.hObj, "D:\\" + (igg + 1).ToString("00") + ".bmp");
            }
            imageDoc1.m_pcb.prepareTileParam();
            using (var obj = imageDoc1.m_pcb.createPcbImage())
            {
                imageDoc1.Image = new HImage(obj);
            }
        }
        List<Wells.Controls.ImageDocEx.ROI> roiList = new List<Wells.Controls.ImageDocEx.ROI>();
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            imageDoc1.setStaticWnd(true);
            double row1, row2, col1, col2;
            imageDoc1.HWindow.SetColor("green");
            imageDoc1.HWindow.DrawRectangle1(out row1, out col1, out row2, out col2);
            imageDoc1.genRect1(row1, col1, row2, col2, ref roiList);
            imageDoc1.setStaticWnd(false);
            button2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Wells.FrmType.frm_Log.Log("我们都有一个家，名字叫中国");
            Wells.FrmType.frm_Log.Log("我们都有一个家，名字叫中国", 1);
            Wells.FrmType.frm_Log.Log("我们都有一个家，名字叫中国", 2);
            Wells.FrmType.frm_Log.LogCache("我们都有一个家，名字叫中国cache1");
            Wells.FrmType.frm_Log.LogCache("我们都有一个家，名字叫中国cache2");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            long memo = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;
            label3.Text = (memo / 1024.0 / 1024.0).ToString("0.00") + "MB";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Wells.FrmType.frm_Log.ShowDlg(true);
        }
    }
}
