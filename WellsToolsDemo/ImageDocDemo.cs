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
        List<Rectangle> rois = new List<Rectangle>();
        List<Rectangle> rois2 = new List<Rectangle>();
        public ImageDocDemo()
        {
            InitializeComponent();
        }

        public void showRois(List<Rectangle> tmp, string color = "green", string mode = "margin")
        {
            imageDoc1.clearWindow(false);
            HOperatorSet.GenEmptyRegion(out HObject region);
            for (int igg = 0; igg < tmp.Count; igg++)
            {
                HOperatorSet.GenRectangle1(out HObject rectangle, tmp[igg].Y, tmp[igg].X, tmp[igg].Height - 1 + tmp[igg].Y, tmp[igg].Width - 1 + tmp[igg].X);
                //HOperatorSet.Union2(region, rectangle, out region);
                HOperatorSet.ConcatObj(region, rectangle, out region);
            }
            List<object> obj = new List<object>();
            obj.Add("region");
            obj.Add(new HRegionEntry(region, color, mode));
            imageDoc1.updateImage(obj);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //HOperatorSet.GenImageConst(out HObject img, "byte", 5000, 5000);
            //imageDoc1.Image = new HImage(img);
            imageDoc1.Image = new HImage("D:/F49.jpg");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string str = Wells.class_Public.getInputInfo("", "", "", false);
            string[] list = str.Split('#');
            imageDoc1.moveImageToPoint(double.Parse(list[0]), double.Parse(list[1]));
            return;
            rois.Add(new Rectangle(100, 100, 10, 10));
            rois.Add(new Rectangle(150, 100, 10, 10));
            rois.Add(new Rectangle(100, 120, 10, 10));
            rois.Add(new Rectangle(300, 400, 10, 10));
            rois.Add(new Rectangle(200, 150, 10, 10));
            rois.Add(new Rectangle(800, 500, 10, 10));
            rois.Add(new Rectangle(200, 100, 10, 10));
            rois.Add(new Rectangle(330, 190, 10, 10));
            rois.Add(new Rectangle(450, 900, 10, 10));
            rois.Add(new Rectangle(660, 80, 10, 10));
            rois.Add(new Rectangle(220, 350, 10, 10));
            rois.Add(new Rectangle(474, 222, 10, 10));
            rois.Add(new Rectangle(333, 50, 10, 10));
            rois.Add(new Rectangle(2111, 500, 10, 10));
            rois.Add(new Rectangle(1000, 1000, 10, 10));
            //rois.Add(new Rectangle(100, 100, 100, 100));
            //rois.Add(new Rectangle(150, 100, 100, 100));
            //rois.Add(new Rectangle(100, 120, 100, 100));
            //rois.Add(new Rectangle(300, 400, 100, 100));
            //rois.Add(new Rectangle(200, 150, 100, 100));
            //rois.Add(new Rectangle(800, 500, 100, 100));
            //rois.Add(new Rectangle(200, 100, 100, 100));
            //rois.Add(new Rectangle(330, 190, 100, 100));
            //rois.Add(new Rectangle(450, 900, 100, 100));
            //rois.Add(new Rectangle(660, 80, 100, 100));
            //rois.Add(new Rectangle(220, 350, 100, 100));
            //rois.Add(new Rectangle(474, 222, 100, 100));
            //rois.Add(new Rectangle(333, 50, 100, 100));
            //rois.Add(new Rectangle(2111, 500, 100, 100));
            //rois.Add(new Rectangle(1000, 1000, 100, 100));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            showRois(rois, "green", "margin");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<Rectangle> tmp = new List<Rectangle>();
            for(int igg=0;igg<rois.Count;igg++)
            {
                Rectangle rr = rois[igg];
                tmp.Add(rr);
            }
            rois2 = clsPublic.getRegionFromRects(tmp);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            showRois(rois2, "red", "margin");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            imageDoc1.clearWindow(false);

            HOperatorSet.GenEmptyRegion(out HObject region);
            for (int igg = 0; igg < rois.Count; igg++)
            {
                HOperatorSet.GenRectangle1(out HObject rectangle, rois[igg].Y, rois[igg].X, rois[igg].Height - 1 + rois[igg].Y, rois[igg].Width - 1 + rois[igg].X);
                //HOperatorSet.Union2(region, rectangle, out region);
                HOperatorSet.ConcatObj(region, rectangle, out region);
            }

            HOperatorSet.GenEmptyRegion(out HObject region2);
            for (int igg = 0; igg < rois2.Count; igg++)
            {
                HOperatorSet.GenRectangle1(out HObject rectangle, rois2[igg].Y, rois2[igg].X, rois2[igg].Height - 1 + rois2[igg].Y, rois2[igg].Width - 1 + rois2[igg].X);
                //HOperatorSet.Union2(region, rectangle, out region);
                HOperatorSet.ConcatObj(region2, rectangle, out region2);
            }

            List<object> obj = new List<object>();
            obj.Add("region");
            obj.Add(new HRegionEntry(region, "red", "margin"));
            obj.Add("region");
            obj.Add(new HRegionEntry(region2, "green", "margin"));
            imageDoc1.updateImage(obj);
        }
    }
}
