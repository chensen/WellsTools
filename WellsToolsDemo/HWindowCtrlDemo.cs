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
    public partial class HWindowCtrlDemo : Form
    {
        public HWindowCtrlDemo()
        {
            InitializeComponent();
        }

        Keys keyPressed = Keys.None;
        
        private void hWindowControl1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //button1.Text = Control.ModifierKeys.ToString();
        }

        private void hWindowControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt) keyPressed |= Keys.Alt;
            if (e.Control) keyPressed |= Keys.Control;
        }

        private void hWindowControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Alt) keyPressed &= ~Keys.Alt;
            if (!e.Control) keyPressed &= ~Keys.Control;
        }
    }
}
