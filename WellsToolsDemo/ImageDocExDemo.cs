using hvppleDotNet;
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
    public partial class ImageDocExDemo : Form
    {
        public ImageDocExDemo()
        {
            InitializeComponent();
            imageDocEx1.Pcb.initialize(55000, 55000, 8000, 8000, 2456, 2056, Point.Empty, 0, 0, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int igg = 0; igg < imageDocEx1.Pcb.m_CameraViewList.Count; igg++)
            {
                HObject obj;
                HOperatorSet.ReadImage(out obj, "D:\\" + (igg + 1).ToString("00") + ".bmp");
                imageDocEx1.Pcb.m_CameraViewList[igg].m_image.hObj = obj;
            }

            imageDocEx1.setImageFromPcb();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Wells.Controls.ImageDocEx.ROI rOI = new Wells.Controls.ImageDocEx.ROIRectangle1(3500,3500,1000,1000);
            imageDocEx1.m_roiList.Add(rOI);

            rOI = new Wells.Controls.ImageDocEx.ROIRectangle1(1200, 1200,1000,1000);
            imageDocEx1.m_roiList.Add(rOI);

            //rOI = new Wells.Controls.ImageDocEx.ROIRectangle2(900, 900,0.7, 200,100);
           //imageDocEx1.m_roiList.Add(rOI);

            imageDocEx1.Invalidate();
        }
        HObject obj = null;
        private void button3_Click(object sender, EventArgs e)
        {
            Wells.Tools.clsRandom rd = new Wells.Tools.clsRandom();
            int igg = rd.getRandomInt(0, 17);
            if(obj == null)
            HOperatorSet.ReadImage(out obj, "D:\\" + (igg + 1).ToString("00") + ".bmp");
            imageDocEx1.setImage(obj);
            //obj.Dispose();
        }

        private void ImageDocExDemo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar=='D'||e.KeyChar == 'd')
            {
                System.Threading.ThreadPool.QueueUserWorkItem((s) =>
                {
                    for (int igg = 0; igg < 20; igg++)
                    {
                        button3_Click(null, null);
                        System.Threading.Thread.Sleep(50);
                    }
                }
                );
            }
        }
    }
}
