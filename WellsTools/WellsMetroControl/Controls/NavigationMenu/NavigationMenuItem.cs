using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Wells.WellsMetroControl.Controls
{
    /// <summary>
    /// Class NavigationMenuItem.
    /// </summary>
    public class NavigationMenuItem : NavigationMenuItemBase
    {
        /// <summary>
        /// The items
        /// </summary>
        private NavigationMenuItem[] items;
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        [Description("子项列表")]
        public NavigationMenuItem[] Items
        {
            get { return items; }
            set
            {
                items = value;
                if (value != null)
                {
                    foreach (var item in value)
                    {
                        item.ParentItem = this;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has split lint at top.
        /// </summary>
        /// <value><c>true</c> if this instance has split lint at top; otherwise, <c>false</c>.</value>
        [Description("是否在此项顶部显示一个分割线")]
        public bool HasSplitLintAtTop { get; set; }

        /// <summary>
        /// Gets the parent item.
        /// </summary>
        /// <value>The parent item.</value>
        [Description("父节点")]
        public NavigationMenuItem ParentItem { get; private set; }
    }


}
