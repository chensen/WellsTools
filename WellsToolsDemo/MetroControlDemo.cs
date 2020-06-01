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
    public partial class MetroControlDemo : Form
    {
        public MetroControlDemo()
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = uchScrollbarEx1;
            //uchScrollbarEx1.DataBindings.Add("ValueLow", textBoxEx1, "Text", true, DataSourceUpdateMode.OnValidation);
            //uchScrollbarEx1.ValueLow = 500.5M;
            textBoxEx1.Text = "125";
            textBoxEx2.Text = "255";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Wells.WellsMetroControl.Forms.FrmAnchorTips frmTips = Wells.WellsMetroControl.Forms.FrmAnchorTips.ShowTips(new Rectangle(100, 100, 50, 20), "ABC", Wells.WellsMetroControl.Forms.AnchorTipsLocation.TOP, Color.Lime, autoCloseTime: -1);
        }

        private void uchScrollbarEx1_ValueChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Write("value changed!\n");
        }
    }
}
