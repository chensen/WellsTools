﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WellsToolsMetroDemo.UC
{
    public partial class UCTestGridTable_CustomCellIcon : UserControl, Wells.WellsMetroControl.Controls.IDataGridViewCustomCell
    {
        public UCTestGridTable_CustomCellIcon()
        {
            InitializeComponent();
        }

        public void SetBindSource(object obj)
        {
            if (obj is TestGridModel)
            {
                this.BackgroundImage = Properties.Resources.rowicon;
            }
        }
    }
}
