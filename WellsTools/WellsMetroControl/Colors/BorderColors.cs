using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Wells.WellsMetroControl
{
    /// <summary>
    /// Class BorderColors.
    /// </summary>
    public class BorderColors
    {
        /// <summary>
        /// The green
        /// </summary>
        private static Color green = ColorTranslator.FromHtml("#f0f9ea");

        /// <summary>
        /// Gets the green.
        /// </summary>
        /// <value>The green.</value>
        public static Color Green
        {
            get { return green; }
            internal set { green = value; }
        }
        /// <summary>
        /// The blue
        /// </summary>
        private static Color blue = ColorTranslator.FromHtml("#ecf5ff");

        /// <summary>
        /// Gets the blue.
        /// </summary>
        /// <value>The blue.</value>
        public static Color Blue
        {
            get { return blue; }
            internal set { blue = value; }
        }
        /// <summary>
        /// The red
        /// </summary>
        private static Color red = ColorTranslator.FromHtml("#fef0f0");

        /// <summary>
        /// Gets the red.
        /// </summary>
        /// <value>The red.</value>
        public static Color Red
        {
            get { return red; }
            internal set { red = value; }
        }
        /// <summary>
        /// The yellow
        /// </summary>
        private static Color yellow = ColorTranslator.FromHtml("#fdf5e6");

        /// <summary>
        /// Gets the yellow.
        /// </summary>
        /// <value>The yellow.</value>
        public static Color Yellow
        {
            get { return yellow; }
            internal set { yellow = value; }
        }
    }
}
