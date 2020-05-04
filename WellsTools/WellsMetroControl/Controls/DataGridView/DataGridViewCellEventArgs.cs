using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wells.WellsMetroControl.Controls
{
    /// <summary>
    /// Class DataGridViewEventArgs.
    /// Implements the <see cref="System.EventArgs" />
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class DataGridViewEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the cell control.
        /// </summary>
        /// <value>The cell control.</value>
        public Control CellControl { get; set; }
        /// <summary>
        /// Gets or sets the index of the cell.
        /// </summary>
        /// <value>The index of the cell.</value>
        public int CellIndex { get; set; }
        /// <summary>
        /// Gets or sets the index of the row.
        /// </summary>
        /// <value>The index of the row.</value>
        public int RowIndex { get; set; }

    }
}
