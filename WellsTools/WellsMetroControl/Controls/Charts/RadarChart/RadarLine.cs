using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Wells.WellsMetroControl.Controls
{
    /// <summary>
    /// Class RadarLine.
    /// </summary>
    public class RadarLine
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        public double[] Values { get; set; }
        /// <summary>
        /// Gets or sets the color of the line.
        /// </summary>
        /// <value>The color of the line.</value>
        public Color? LineColor { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [show value text].
        /// </summary>
        /// <value><c>true</c> if [show value text]; otherwise, <c>false</c>.</value>
        public bool ShowValueText { get; set; }
        /// <summary>
        /// Gets or sets the color of the fill.
        /// </summary>
        /// <value>The color of the fill.</value>
        public Color? FillColor { get; set; }
    }
}
