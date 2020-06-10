using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using hvppleDotNet;

namespace Wells.Controls.ImageDocEx
{
    public class Tracker : Model
    {
        public double Row1 { get; set; }
        public double Row2 { get; set; }
        public double Col1 { get; set; }
        public double Col2 { get; set; }

        public bool Actived = false;

        private TrackShape shape = TrackShape.Rect1;

        public TrackShape Shape
        {
            get { return shape; }
            set
            {
                shape = value;
                if(shape == TrackShape.Line)
                {
                    Color = "yellow";
                    LineStyle = "line";
                    LineWidth = 3;
                }
                else if(shape == TrackShape.Rect1)
                {
                    Color = "white";
                    LineStyle = "dot";
                    LineWidth = 1;
                }
                else if (shape == TrackShape.Rect2)
                {
                    Color = "green";
                    LineStyle = "line";
                    LineWidth = 1;
                }
            }
        }

        public Tracker()
        {
            Row1 = 0.0;
            Row2 = 0.0;
            Col1 = 0.0;
            Col2 = 0.0;
            Type = "Tracker";
            Color = "white";
            LineStyle = "dot";
        }

        public RectangleF getRect()
        {
            return new RectangleF(Col1 < Col2 ? (float)Col1 : (float)Col2, Row1 < Row2 ? (float)Row1 : (float)Row2, (float)Math.Abs(Col1 - Col2), (float)Math.Abs(Row1 - Row2));
        }

        public override void draw(HWindow window)
        {
            if (Actived)
            {
                window.SetColor(Color);
                window.SetDraw(DrawMode);
                window.SetLineWidth(LineWidth);
                window.SetLineStyle(LineStyle == "dot" ? new HTuple(2, 2) : new HTuple());

                if (Shape == TrackShape.Line)
                {
                    window.DispLine(Row1, Col1, Row2, Col2);
                    //window.DispArrow(Row2, Col2, Row1, Col1, LineWidth);
                }
                else
                {
                    window.DispRectangle1(Row1 < Row2 ? Row1 : Row2, Col1 < Col2 ? Col1 : Col2, Row1 < Row2 ? Row2 : Row1, Col1 < Col2 ? Col2 : Col1);
                }
            }
        }
    }
}
