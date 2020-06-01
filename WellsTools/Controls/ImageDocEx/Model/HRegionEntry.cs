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
    public class HRegionEntry : Model
    {
        private HObject hObject;
        public HObject HObject
        {
            get { return hObject; }
            set { hObject = value; }
        }

        public HRegionEntry(HObject _hbj, string _color = "green", string _drawMode = "margin", int _lineWidth = 1, string lineStyle = "line", string _type = "")
        {
            hObject = _hbj;
            Color = _color;
            DrawMode = _drawMode;
            LineWidth = _lineWidth;
            Type = _type;
            LineStyle = lineStyle;
        }

        public override void draw(HWindow window)
        {
            window.SetColor(Color);
            window.SetDraw(DrawMode);
            window.SetLineWidth(LineWidth);
            window.SetLineStyle(LineStyle == "dot" ? new HTuple(10, 10) : new HTuple());

            window.DispObj(hObject);
        }
        
        public void Dispose()
        {
            if (hObject != null && hObject.IsInitialized())
                hObject.Dispose();
        }
    }
}
