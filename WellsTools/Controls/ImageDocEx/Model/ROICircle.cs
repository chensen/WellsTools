using System;
using hvppleDotNet;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing;

namespace Wells.Controls.ImageDocEx
{
    public class ROICircle : ROI
    {
        public double Row
        {
            get { return this.midR; }
            set
            {
                if (value == this.midR) return;
                this.midR = value;
                base.NotifyPropertyChange("Row");
            }
        }
        
        public double Column
        {
            get { return this.midC; }
            set
            {
                if (value == this.midC) return;
                this.midC = value;
                base.NotifyPropertyChange("Column");
            }
        }

        public double Radius
        {
            get { return this.radius; }
            set
            {
                if (value == this.radius) return;
                this.radius = value;
                base.NotifyPropertyChange("Radius");
            }
        }
        
        private double radius;
        private double row1;// first control handle
        private double col1;
        private double midR;// mid handle
        private double midC;  


        public ROICircle()
        {
            NumHandles = 2; //0,mid;1,circle;
            activeHandleIdx = 0;

            midR = 25;
            midC = 25;
            row1 = 25;
            col1 = 50;
            radius = 25;

            Type = "Circle";
        }

        public ROICircle(double midX, double midY, double radius)
        {
            NumHandles = 2;
            activeHandleIdx = 0;

            this.midR = midY;
            this.midC = midX;
            this.radius = radius;
            this.row1 = midY;
            this.col1 = midX + radius;

            Type = "Circle";
        }
        
        public override void createInitROI(double midX, double midY)
        {
            this.midR = midY;
            this.midC = midX;
            this.radius = 25;
            this.row1 = midY;
            this.col1 = midX + 25;

            NumHandles = 2;
            activeHandleIdx = 0;
        }
        
        public override void draw(HWindow window, int imageWidth, int imageHeight)
        {
            window.SetColor(Color);
            window.SetDraw(DrawMode);
            window.SetLineWidth(LineWidth);
            window.SetLineStyle(LineStyle == "dot" ? new HTuple(2, 2) : new HTuple());

            window.DispCircle(midR, midC, radius);//body

            double littleRecSize = getHandleWidth(imageWidth, imageHeight);

            window.SetDraw("fill");
            window.DispRectangle2(midR, midC, 0, littleRecSize, littleRecSize);//0,mid

            if (Selected)
            {
                window.DispRectangle2(row1, col1, 0, littleRecSize, littleRecSize);//1,circle
            }
        }

        public override int ptLocation(double x, double y, int imageWidth, int imageHeight)
        {
            double width = 2 * getHandleWidth(imageWidth, imageHeight);
            HTuple distance;
            HOperatorSet.DistancePp(new HTuple(y), new HTuple(x), new HTuple(midR), new HTuple(midC), out distance);
            if (distance.D > radius + width)//outside
                return -1;
            else if (distance.D < radius - width)//inside
                return 1;
            else//onside
                return 0;
        }

        public override bool isInRect(RectangleF rect)
        {
            if (midR - radius > rect.Y && midR + radius < rect.Y + rect.Height && midC - radius > rect.X && midC + radius < rect.X + rect.Width) 
                return true;

            return false;
        }

        public override double distToClosestHandle(double x, double y)
        {
            double max = 10000;
            double[] val = new double[NumHandles];

            val[0] = HMisc.DistancePp(y, x, midR, midC);//0,mid
            val[1] = HMisc.DistancePp(y, x, row1, col1);//1,circle
            

            for (int i = 0; i < NumHandles; i++)
            {
                if (val[i] < max)
                {
                    max = val[i];
                    activeHandleIdx = i;
                }
            }// end of for 
            
            return val[activeHandleIdx];
        }
        
        public override void move(double dx, double dy)
        {
            midR += dy;
            midC += dx;
            row1 += dy;
            col1 += dx;
        }

        public override void resize(double newX, double newY)
        {
            HTuple distance;
            double shiftX, shiftY;

            switch (activeHandleIdx)
            {
                case 0: //0,mid
                    {
                        shiftY = midR - newY;
                        shiftX = midC - newX;

                        midR = newY;
                        midC = newX;

                        row1 -= shiftY;
                        col1 -= shiftX;
                    }
                    break;
                case 1: //1,circle
                    {
                        row1 = newY;
                        col1 = newX;
                        HOperatorSet.DistancePp(new HTuple(row1), new HTuple(col1),new HTuple(midR), new HTuple(midC),out distance);
                        radius = distance[0].D;
                    }
                    break;
            }
        }
    }
}
