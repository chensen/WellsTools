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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void SetNotifyInfo(int percent, string message)
        {
            this.label1.Text = message;
            this.progressBar1.Value = percent;
        }
    }
}
