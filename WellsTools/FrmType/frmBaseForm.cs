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
    public partial class frmBaseForm : Form
    {
        [Category("Appearance")]
        [DefaultValue("Form")]
        public string Title
        {
            get
            {
                return lbTitle.Text;
            }
            set
            {
                Title = value;
                lbTitle.Text = value;
            }
        }
        public frmBaseForm()
        {
            InitializeComponent();
        }
    }
}
