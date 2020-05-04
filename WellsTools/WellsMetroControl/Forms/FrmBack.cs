using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wells.WellsMetroControl.Forms
{
    /// <summary>
    /// Class FrmBack.
    /// Implements the <see cref="Wells.WellsMetroControl.Forms.FrmBase" />
    /// </summary>
    /// <seealso cref="Wells.WellsMetroControl.Forms.FrmBase" />
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(System.ComponentModel.Design.IDesigner))]
    public partial class FrmBack : FrmBase
    {
        /// <summary>
        /// The FRM title
        /// </summary>
        private string _frmTitle = "自定义窗体";
        /// <summary>
        /// 窗体标题
        /// </summary>
        /// <value>The FRM title.</value>
        [Description("窗体标题"), Category("自定义")]
        public string FrmTitle
        {
            get { return _frmTitle; }
            set
            {
                _frmTitle = value;
                btnBack1.BtnText = "   " + value;
            }
        }
        /// <summary>
        /// Occurs when [BTN help click].
        /// </summary>
        [Description("帮助按钮点击事件"), Category("自定义")]
        public event EventHandler BtnHelpClick;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmBack" /> class.
        /// </summary>
        public FrmBack()
        {
            InitializeComponent();
            InitFormMove(this.panTop);
        }

        /// <summary>
        /// Handles the btnClick event of the btnBack1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnBack1_btnClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the MouseDown event of the label1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (BtnHelpClick != null)
                BtnHelpClick(sender, e);
        }
    }
}
