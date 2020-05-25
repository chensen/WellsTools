using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using hvppleDotNet;
using Wells.Controls.ImageDoc;

namespace WellsToolsDemo
{
    public partial class ImageDocDemo : Form
    {
        //Wells.Tools.clsXmlEx xml = new Wells.Tools.clsXmlEx("xml.xml");
        List<ROI> rois = new List<ROI>();
        public ImageDocDemo()
        {
            InitializeComponent();
            //imageDoc.setStaticWnd(true);
            //imageDoc1.ImageMode = ImageMode.Stretch;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //HImage img = new HImage("D:\\12.bmp");
            //imageDoc.HWindow.DispImage(img);
            imageDoc1.HWindow.SetTposition(100, 200);
            imageDoc1.HWindow.SetColor("red");
            imageDoc1.HWindow.WriteString("ABCfdaaaaa");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HImage img = new HImage("D:\\01.bmp");
            imageDoc1.displayImage(img);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            imageDoc1.genRect1(100, 100, 200, 500, ref rois);
            //imageDoc.viewWindow.genCircle(100, 100, 50, ref rois);
            //imageDoc1.genCircularArc(200, 200, 200, 0, 1.2, "", ref rois);
            imageDoc1.genRect2(300, 300, 0.6, 50, 80, ref rois);
            imageDoc1.genLine(200, 250, 400, 600, ref rois);
            //HObject re;
            //HOperatorSet.GenCircle(out re, 200, 100, 100);
            //imageDoc1.displayHRegion(re, "red", "fill", 10);
            //imageDoc.viewWindow.genNurbs(800, 800, ref rois);
            int delta = (int)(40 * imageDoc1.getScale());
            imageDoc1.displayMessage("小包子同学！", 100, 100, 40, "green", "window");
            imageDoc1.displayMessage("我们结婚吧！", 100 + delta + 10, 100, 40, "red", "window");
            imageDoc1.displayMessage("你是我的女王！", 300, 100, 55, "blue");
            imageDoc1.displayMessage("你的余生就交给我吧", 400, 100, 55, "yellow");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<int> data = new List<int>();
            data.AddRange(new int[] { 1, 5, 100, 4, 88, 87, 99, 2, 545, 9 });
            Wells.Tools.clsDataSort.insertSort(data);
            
            imageDoc1.saveROI(rois, "rois.xml");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rois = new List<ROI>();
            imageDoc1.loadROI("rois.xml", out rois);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            imageDoc1.clearWindow(false);
            //imageDoc1.setSelected(true);
            //imageDoc1.ImageMode = imageDoc1.ImageMode == ImageMode.Stretch ? ImageMode.Origin : ImageMode.Stretch;
        }
    }
}
