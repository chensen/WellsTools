using hvppleDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wells.Controls.ImageDocEx
{
    /// <summary>
    /// 显示xld和region 带有颜色
    /// </summary>
    public class HRegionEntry
    {
        private HObject hObject;
        private string color = "green";
        private string drawMode = "margin";
        private int lineWidth = 1;
        private string type = "";

        public HRegionEntry(HObject _hbj)
        {
            hObject = _hbj;
        }

        public HRegionEntry(HObject _hbj, string _color, string _drawMode, string _type="")
        {
            hObject = _hbj;
            color = _color;
            drawMode = _drawMode;
            type = _type;
        }

        public HRegionEntry(HObject _hbj, string _color, string _drawMode, int _lineWidth, string _type = "")
        {
            hObject = _hbj;
            color = _color;
            drawMode = _drawMode;
            lineWidth = _lineWidth;
            type = _type;
        }
        
        public void clear()
        {
            if (hObject != null && hObject.IsInitialized())
                hObject.Dispose();
        }

        public HObject HObject
        {
            get { return hObject; }
            set { hObject = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public string DrawMode
        {
            get { return drawMode; }
            set { drawMode = value; }
        }

        public int LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
