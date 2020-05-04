using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wells.FrmType
{
    public partial class frmProgressBar : Form
    {
        public frmProgressBar()
        {
            InitializeComponent();
        }

        public void Start()
        {
            progressBar1.Value = 0;
            this.Visible = false;
            this.timer1.Enabled = true;
            this.ShowDialog();
        }

        public void SetBarInfo(string title,int max)
        {
            this.Text = title;
            progressBar1.Maximum = max;
        }

        public void SetBarValue(int value)
        {
            progressBar1.Value = value;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            int value = progressBar1.Value;
            if(value>=progressBar1.Maximum)
            {
                this.Visible = false;
                this.Close();
                return;
            }
            timer1.Enabled = true;
        }
    }
}
