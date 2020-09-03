using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using hvppleDotNet;

namespace WellsToolsDemo
{
    public partial class frmHWindow : Form
    {
        private HWindow HWindow;
        private HObject image;
        private HObject region;
        public frmHWindow()
        {
            InitializeComponent();
            HWindow = this.hWindowControl1.hvppleWindow;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HOperatorSet.ReadImage(out image, "D:\\test.jpg");
            HOperatorSet.GetImageSize(image, out HTuple w, out HTuple h);
            hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, w.I, h.I);
            repaint();
        }

        public void repaint()
        {
            HSystem.SetSystem("flush_graphic", "false");
            HWindow.ClearWindow();

            try
            {
                if (image != null)
                    HWindow.DispObj(image);

                HWindow.SetDraw("margin");
                HWindow.SetColor("red");

                if (region != null)
                    HWindow.DispObj(region);
            }
            catch (System.Exception ex)
            {
            	
            }
            finally
            {
                HSystem.SetSystem("flush_graphic", "true");

                //注释了下面语句,会导致窗口无法实现缩放和拖动
                HWindow.SetColor("dim gray");
                HWindow.DispLine(-100.0, -100.0, -101.0, -101.0);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HOperatorSet.ReadImage(out image, "D:\\B18.jpg");
            HOperatorSet.GetImageSize(image, out HTuple w, out HTuple h);
            hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, w.I, h.I);
            repaint();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HOperatorSet.GenRectangle1(out region, 1500, 2000, 2500, 3000);
            repaint();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Wells.FrmType.frm_Log.Log("15555555555555555555555555555555555515555555555555", 2);
            Wells.FrmType.frm_Log.Log("我们都有一个家，名字叫中国，兄弟姐妹都很多，景色也不错！", 2);
            Wells.FrmType.frm_Log.Log("我们都有一个家，名字叫中国，兄弟姐妹都很多，景色也不错！", 0);
            Wells.FrmType.frm_Log.Log("我们都有一个家，名字叫中国，兄弟姐妹都很多，景色也不错！", 1);
            Wells.FrmType.frm_Log.Log("我们都有一个家，名字叫中国，兄弟姐妹都很多，景色也不错！", 2);
            Wells.FrmType.frm_Log.ShowDlg(true);
        }
    }
}
