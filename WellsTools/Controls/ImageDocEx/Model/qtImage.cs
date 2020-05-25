using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hvppleDotNet;

namespace Wells.Controls.ImageDocEx
{
    public class qtImage:IDisposable
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Color { get; set; }

        public HObject hObj;

        public qtImage()
        {
            Width = 0;
            Height = 0;
            Color = false;
            hObj = null;
        }
        
        public void Dispose()
        {
            if (hObj == null) return;
            if (!hObj.IsInitialized()) return;
            hObj.Dispose();
        }
    }
}
