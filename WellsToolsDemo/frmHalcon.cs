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
    public partial class frmHalcon : Form
    {
        public frmHalcon()
        {
            InitializeComponent();
        }

        private void hWindowControl1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void hWindowControl1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            textBox1.Text += e.KeyCode + ",";
        }
    }
}
