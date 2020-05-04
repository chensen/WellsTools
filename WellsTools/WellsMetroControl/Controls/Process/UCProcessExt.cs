using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wells.WellsMetroControl.Controls
{
    /// <summary>
    /// Class UCProcessExt.
    /// Implements the <see cref="Wells.WellsMetroControl.Controls.UCControlBase" />
    /// </summary>
    /// <seealso cref="Wells.WellsMetroControl.Controls.UCControlBase" />
    public partial class UCProcessExt : UCControlBase
    {
        /// <summary>
        /// The value
        /// </summary>
        private int _value = 0;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value
        {
            get { return this._value; }
            set
            {
                if (value < 0)
                    return;
                this._value = value;
                SetValue();
            }
        }

        /// <summary>
        /// The maximum value
        /// </summary>
        private int maxValue = 100;

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        public int MaxValue
        {
            get { return maxValue; }
            set
            {
                if (value <= 0)
                    return;
                maxValue = value;
                SetValue();
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        private void SetValue()
        {
            double dbl = (double)_value / (double)maxValue;
            this.panel1.Width = (int)(this.Width * dbl);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UCProcessExt" /> class.
        /// </summary>
        public UCProcessExt()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the SizeChanged event of the ProcessExt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ProcessExt_SizeChanged(object sender, EventArgs e)
        {
            SetValue();
        }

        /// <summary>
        /// Steps this instance.
        /// </summary>
        public void Step()
        {
            Value++;
        }
    }
}
