using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WellsToolsDemo
{
    public partial class MetroControlDemo : Form
    {
        public MetroControlDemo()
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = skinComboBox1;
            textBoxEx1.Text = "125";
            textBoxEx2.Text = "255";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Wells.FrmType.frm_Log.ShowDlg(true);
        }

        private void uchScrollbarEx1_ValueChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Write("value changed!\n");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            //textBoxEx1.Text = "200";
            uchScrollbarEx1.ValueLow = 210;
        }
    }
}
