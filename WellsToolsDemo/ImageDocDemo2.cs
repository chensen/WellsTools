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

namespace WellsToolsDemo
{
    public partial class ImageDocDemo2 : Form
    {
        HObject[] objList = null;
        public ImageDocDemo2()
        {
            InitializeComponent();
            var pcb = clsPCB.m_pPCB;
            clsPCB.m_pPCB.initialize(55000, 55000, 8000, 8000, 2456, 2056, Point.Empty, 0, 0, true);
        }

        private void ImageDocDemo2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            objList = new HObject[12];
            for (int igg = 0; igg < 12; igg++)
                HOperatorSet.ReadImage(out objList[igg], "D:\\" + (igg + 1).ToString("00") + ".bmp");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int row = int.Parse(textBox1.Text);
            int col = int.Parse(textBox2.Text);

            int width = (2456 - col * 2) * clsPCB.m_pPCB.m_xFovNum;
            int height = (2056 - row * 2) * clsPCB.m_pPCB.m_yFovNum;
            int _W = (2456 - col * 2);
            int _H = (2056 - row * 2);

            HTuple offsetRow = new HTuple();
            HTuple offsetCol = new HTuple();
            HTuple Row1 = new HTuple();
            HTuple Col1 = new HTuple();
            HTuple Row2 = new HTuple();
            HTuple Col2 = new HTuple();
            HObject imgs = new HObject();
            HOperatorSet.GenEmptyObj(out imgs);
            for (int igg = 0; igg < 12; igg++) HOperatorSet.ConcatObj(imgs, objList[igg], out imgs);
            for (int jgg = 0; jgg < clsPCB.m_pPCB.m_yFovNum; jgg++)
            {
                for (int igg = 0; igg < clsPCB.m_pPCB.m_xFovNum; igg++)
                {
                    offsetRow[jgg * clsPCB.m_pPCB.m_xFovNum + igg] = height - (jgg + 1) * _H;
                    offsetCol[jgg * clsPCB.m_pPCB.m_xFovNum + igg] = 0 + igg  * _W;
                    Row1[jgg * clsPCB.m_pPCB.m_xFovNum + igg] = row;
                    Col1[jgg * clsPCB.m_pPCB.m_xFovNum + igg] =col;
                    Row2[jgg * clsPCB.m_pPCB.m_xFovNum + igg] =2056-row;
                    Col2[jgg * clsPCB.m_pPCB.m_xFovNum + igg] = 2456 - col;
                }
            }
            HObject tiledImage = null;
            HOperatorSet.TileImagesOffset(imgs, out tiledImage, offsetRow, offsetCol, Row1, Col1, Row2, Col2, width, height);
            imageDoc1.Image = new HImage(tiledImage);
        }
    }
}
