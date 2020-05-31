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
            
        }
    }
}
