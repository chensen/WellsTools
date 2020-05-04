using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wells.WellsMetroControl.Controls
{
    public class CrumbNavigationClickEventArgs : EventArgs
    {
        public int Index { get; set; }
        public CrumbNavigationItem Item { get; set; }
    }
}
