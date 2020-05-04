using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wells.WellsMetroControl.Controls
{
    /// <summary>
    /// Class FunelChartItem.
    /// </summary>
    public class FunelChartItem
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public float Value { get; set; }
        /// <summary>
        /// Gets or sets the color of the value.
        /// </summary>
        /// <value>The color of the value.</value>
        public System.Drawing.Color? ValueColor { get; set; }
        /// <summary>
        /// Gets or sets the color of the text fore.
        /// </summary>
        /// <value>The color of the text fore.</value>
        public System.Drawing.Color? TextForeColor { get; set; }
    }
}
