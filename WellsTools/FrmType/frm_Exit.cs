using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wells.FrmType
{
    public partial class frm_Exit : CCWin.Skin_Color
    {
        private FormCloseType type = FormCloseType.None;

        public frm_Exit()
        {
            InitializeComponent();
        }

        public FormCloseType GetCloseType()
        {
            return type;
        }

        private void btnCloseProgram_Click(object sender, EventArgs e)
        {
            type = FormCloseType.Close;
            this.Close();
        }

        private void btnRestartProgram_Click(object sender, EventArgs e)
        {
            type = FormCloseType.Restart;
            this.Close();
        }

        private void btnCloseComputer_Click(object sender, EventArgs e)
        {
            type = FormCloseType.CloseSystem;
            this.Close();
        }

        private void btnRestartComputer_Click(object sender, EventArgs e)
        {
            type = FormCloseType.RestartSystem;
            this.Close();
        }

        private void frm_Exit_Shown(object sender, EventArgs e)
        {
            type = FormCloseType.None;
        }
    }
}
