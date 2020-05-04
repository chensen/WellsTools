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
    /// Class FrmWithOKCancel1.
    /// Implements the <see cref="Wells.WellsMetroControl.Forms.FrmWithTitle" />
    /// </summary>
    /// <seealso cref="Wells.WellsMetroControl.Forms.FrmWithTitle" />
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(System.ComponentModel.Design.IDesigner))]
    public partial class FrmWithOKCancel1 : FrmWithTitle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrmWithOKCancel1" /> class.
        /// </summary>
        public FrmWithOKCancel1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the BtnClick event of the btnOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnOK_BtnClick(object sender, EventArgs e)
        {
            DoEnter();
        }

        /// <summary>
        /// Handles the BtnClick event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnCancel_BtnClick(object sender, EventArgs e)
        {
            DoEsc();
        }

        /// <summary>
        /// Does the enter.
        /// </summary>
        protected override void DoEnter()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Handles the VisibleChanged event of the FrmWithOKCancel1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void FrmWithOKCancel1_VisibleChanged(object sender, EventArgs e)
        {
        }
    }
}
