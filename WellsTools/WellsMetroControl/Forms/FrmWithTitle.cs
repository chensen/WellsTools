﻿using System;
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
    /// Class FrmWithTitle.
    /// Implements the <see cref="Wells.WellsMetroControl.Forms.FrmBase" />
    /// </summary>
    /// <seealso cref="Wells.WellsMetroControl.Forms.FrmBase" />
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(System.ComponentModel.Design.IDesigner))]
    public partial class FrmWithTitle : FrmBase
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Description("窗体标题"), Category("自定义")]
        public string Title
        {
            get
            {
                return lblTitle.Text;
            }
            set
            {
                lblTitle.Text = value;
            }
        }
        /// <summary>
        /// The is show close BTN
        /// </summary>
        private bool _isShowCloseBtn = false;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is show close BTN.
        /// </summary>
        /// <value><c>true</c> if this instance is show close BTN; otherwise, <c>false</c>.</value>
        [Description("是否显示右上角关闭按钮"), Category("自定义")]
        public bool IsShowCloseBtn
        {
            get
            {
                return _isShowCloseBtn;
            }
            set
            {
                _isShowCloseBtn = value;
                btnClose.Visible = value;
                if (value)
                {
                    btnClose.Location = new Point(this.Width - btnClose.Width - 10, 0);
                    btnClose.BringToFront();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmWithTitle" /> class.
        /// </summary>
        public FrmWithTitle()
        {
            InitializeComponent();
            InitFormMove(this.lblTitle);
        }
       

        /// <summary>
        /// Handles the Shown event of the FrmWithTitle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void FrmWithTitle_Shown(object sender, EventArgs e)
        {
            if (IsShowCloseBtn)
            {
                btnClose.Location = new Point(this.Width - btnClose.Width - 10, 0);
                btnClose.BringToFront();
            }
        }


        /// <summary>
        /// Handles the VisibleChanged event of the FrmWithTitle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void FrmWithTitle_VisibleChanged(object sender, EventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmWithTitle_SizeChanged(object sender, EventArgs e)
        {
            btnClose.Location = new Point(this.Width - btnClose.Width, btnClose.Location.Y);
        }
    }
}
