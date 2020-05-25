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
    public partial class frmInput : frmBaseForm
    {
        public string strInputText = "";

        public frmInput()
        {
            InitializeComponent();
        }

        public void initForm(string strCaption,string strLabel,string strInputText,bool bPsd)
        {
            Text = strCaption;
            label1.Text = strLabel;
            textBox1.PasswordChar = bPsd ? '*' : '\0';
            textBox1.Text = strInputText;
            textBox1.SelectAll();
            textBox1.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            strInputText = this.textBox1.Text;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            strInputText = "";
            Close();
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            CCWin.SkinControl.PassKey passKey = new CCWin.SkinControl.PassKey(Left + textBox1.Left+5, Top + textBox1.Bottom + 30, textBox1);
            passKey.Show(this);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                btnOK_Click(null, null);
            }
        }

        private void frmInput_Load(object sender, EventArgs e)
        {
            TopMost = false;
            TopLevel = true;
            BringToFront();
            TopMost = true;
            strInputText = "";
        }

        private void frmInput_Activated(object sender, EventArgs e)
        {
            textBox1.Select();
        }
    }
}
