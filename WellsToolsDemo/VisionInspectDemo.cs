using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wells.Controls.VisionInspect;

namespace WellsToolsDemo
{
    public partial class VisionInspectDemo : Form
    {
        public VisionInspectDemo()
        {
            InitializeComponent();
        }

        private void VisionInspectDemo_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                string path = @"D:\";
                for (int igg = 0; igg < 12; igg++)
                {
                    clsPCB.m_pPCB.m_CameraViewList[igg].m_image.ImgBuffer = Wells.Controls.VisionInspect.clsPublic.getImageData(path + (igg + 1).ToString("00") + ".bmp");
                    imageDoc1.m_AreaView.update(imageDoc1.ClientRectangle, imageDoc1.m_AreaView.m_lptCenter);
                    Wells.class_Thread.Sleep(50);
                }
            }
            );
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clsPCB.m_pPCB.initialize(int.Parse(textBox1.Text), int.Parse(textBox2.Text), int.Parse(textBox3.Text), int.Parse(textBox4.Text), int.Parse(textBox5.Text), int.Parse(textBox6.Text), Point.Empty, 0, 0, true);
            imageDoc1.zoomFit();
        }
    }
}
