using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Wells.WellsMetroControl
{
    /// <summary>
    /// Class BasisColors.
    /// </summary>
    public class BasisColors
    {
        /// <summary>
        /// The light
        /// </summary>
        private static Color light = ColorTranslator.FromHtml("#f5f7fa");

        /// <summary>
        /// Gets the light.
        /// </summary>
        /// <value>The light.</value>
        public static Color Light
        {
            get { return light; }
            internal set { light = value; }
        }
        /// <summary>
        /// The medium
        /// </summary>
        private static Color medium = ColorTranslator.FromHtml("#f0f2f5");

        /// <summary>
        /// Gets the medium.
        /// </summary>
        /// <value>The medium.</value>
        public static Color Medium
        {
            get { return medium; }
            internal set { medium = value; }
        }
        /// <summary>
        /// The dark
        /// </summary>
        private static Color dark = ColorTranslator.FromHtml("#000000");

        /// <summary>
        /// Gets the dark.
        /// </summary>
        /// <value>The dark.</value>
        public static Color Dark
        {
            get { return dark; }
            internal set { dark = value; }
        }
      
    }
}
