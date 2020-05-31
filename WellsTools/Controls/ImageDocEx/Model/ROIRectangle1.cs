using System;
using hvppleDotNet;
using System.Xml.Serialization;
using System.Drawing;

namespace Wells.Controls.ImageDocEx
{
    public class ROIRectangle1 : ROI
    {
        public double Row1
        {
            get { return this.row1; }
            set
            {
                if (value == this.row1) return;
                this.row1 = value;
                base.NotifyPropertyChange("Row1");
            }
        }
        
        public double Column1
        {
            get { return this.col1; }
            set
            {
                if (value == this.col1) return;
                this.col1 = value;
                base.NotifyPropertyChange("Column1");
            }
        }
        
        public double Row2
        {
            get { return this.row2; }
            set
            {
                if (value == this.row2) return;
                this.row2 = value;
                base.NotifyPropertyChange("Row2");
            }
        }
        
        public double Column2
        {
            get { return this.col2; }
            set
            {
                if (value == this.col2) return;
                this.col2 = value;
                base.NotifyPropertyChange("Column2");
            }
        }

        private double row1;// upper left
        private double col1;   
        private double row2; // lower right 
        private double col2;
        private double midR;// midpoint 
        private double midC;

        public ROIRectangle1()
        {
            NumHandles = 9; // 0,mid;1,up left;2,up right;3,down right;4,down left;5,up mid;6,right mid;7,down mid;8,left mid;
            activeHandleIdx = 0;//default mid

            row1 = 0;
            row2 = 50;
            col1 = 0;
            col2 = 50;
            midR = 25;
            midC = 25;

            Type = "Rectangle1";
        }
        
        public ROIRectangle1(double midX, double midY, double width, double height)
        {
            NumHandles = 9;
            activeHandleIdx = 0;

            this.row1 = midY - height / 2.0;
            this.col1 = midX - width / 2.0;
            this.row2 = midY + height / 2.0;
            this.col2 = midX + width / 2.0;
            this.midR = midY;
            this.midC = midX;

            Type = "Rectangle1";
        }

        public override void createInitROI(double midX, double midY)
        {
            this.midR = midY;
            this.midC = midX;

            this.row1 = midR - 25;
            this.col1 = midC - 25;
            this.row2 = midR + 25;
            this.col2 = midC + 25;
        }
        
        public override void draw(HWindow window, int imageWidth, int imageHeight)
        {
            window.SetColor(Color);
            window.SetDraw(DrawMode);
            window.SetLineWidth(LineWidth);
            window.SetLineStyle(LineStyle == "dot" ? new HTuple(2, 2) : new HTuple());

            double littleRecSize = getHandleWidth(imageWidth, imageHeight);

            window.DispRectangle1(row1, col1, row2, col2);//body

            midR = (row1 + row2) / 2;
            midC = (col1 + col2) / 2;

            window.SetDraw("fill");
            window.DispRectangle2(midR, midC, 0, littleRecSize, littleRecSize);//0,mid

            if (Selected)
            {
                window.DispRectangle2(row1, col1, 0, littleRecSize, littleRecSize);//1,up left
                window.DispRectangle2(row1, col2, 0, littleRecSize, littleRecSize);//2,up right
                window.DispRectangle2(row2, col2, 0, littleRecSize, littleRecSize);//3,down right
                window.DispRectangle2(row2, col1, 0, littleRecSize, littleRecSize);//4,down left
                window.DispRectangle2(row1, (col1 + col2) / 2, 0, littleRecSize, littleRecSize);//5,up mid
                window.DispRectangle2((row1 + row2) / 2, col2, 0, littleRecSize, littleRecSize);//6,right mid
                window.DispRectangle2(row2, (col1 + col2) / 2, 0, littleRecSize, littleRecSize);//7,down mid
                window.DispRectangle2((row1 + row2) / 2, col1, 0, littleRecSize, littleRecSize);//8,left mid
            }
        }

        public override int ptLocation(double x, double y, int imageWidth, int imageHeight)
        {
            activeHandleIdx = -1;

            float centerX = (float)midC;
            float centerY = (float)midR;
            float width = (float)Math.Abs(col2 - col1);
            float height = (float)Math.Abs(row2 - row1);
            PointF ptF = new PointF((float)x, (float)y);

            float ffff = 0.6f;
            RectangleF rectInside = new RectangleF(centerX - ffff * width / 2.0f, centerY - ffff * height / 2.0f, ffff * width, ffff * height);
            bool bInside = rectInside.Contains(ptF);
            if (bInside)
            {
                activeHandleIdx = 0;
                return 0;//inside
            }
            
            ffff = 0.8f;
            RectangleF rectNone = new RectangleF(centerX - ffff * width / 2.0f, centerY - ffff * height / 2.0f, ffff * width, ffff * height);
            bool bNone = rectNone.Contains(ptF);
            if (bNone)
            {
                return 2;//no handle
            }
            
            ffff = 1.2f;
            RectangleF rectOnside = new RectangleF(centerX - ffff * width / 2.0f, centerY - ffff * height / 2.0f, ffff * width, ffff * height);
            bool bOnside = rectOnside.Contains(ptF);
            if (bOnside)
            {
                if (x > col1 - 0.2 * width / 2 && x < col1 + 0.2 * width / 2 && y > row1 - 0.2 * height / 2 && y < row1 + 0.2 * height / 2)//左上
                    activeHandleIdx = 1;
                else if (x > col2 - 0.2 * width / 2 && x < col2 + 0.2 * width / 2 && y > row1 - 0.2 * height / 2 && y < row1 + 0.2 * height / 2)//右上
                    activeHandleIdx = 2;
                else if (x > col2 - 0.2 * width / 2 && x < col2 + 0.2 * width / 2 && y > row2 - 0.2 * height / 2 && y < row2 + 0.2 * height / 2)//右下
                    activeHandleIdx = 3;
                else if (x > col1 - 0.2 * width / 2 && x < col1 + 0.2 * width / 2 && y > row2 - 0.2 * height / 2 && y < row2 + 0.2 * height / 2)//左下
                    activeHandleIdx = 4;
                else if (x > col1 + 0.8 * width / 2 && x < col2 - 0.8 * width / 2 && y > row1 - 0.2 * height / 2 && y < row1 + 0.2 * height / 2)//上中
                    activeHandleIdx = 5;
                else if (x > col2 - 0.2 * width / 2 && x < col2 + 0.2 * width / 2 && y > row1 + 0.8 * height / 2 && y < row2 - 0.8 * height / 2)//右中
                    activeHandleIdx = 6;
                else if (x > col1 + 0.8 * width / 2 && x < col2 - 0.8 * width / 2 && y > row2 - 0.2 * height / 2 && y < row2 + 0.2 * height / 2)//下中
                    activeHandleIdx = 7;
                else if (x > col1 - 0.2 * width / 2 && x < col1 + 0.2 * width / 2 && y > row1 + 0.8 * height / 2 && y < row2 - 0.8 * height / 2)//左中
                    activeHandleIdx = 8;
                return 1;//onside
            }
            else return -1;//outside
        }

        public override bool isInRect(RectangleF rect)
        {
            if (row1 > rect.Y && row2 < rect.Y + rect.Height && col1 > rect.X && col2 < rect.X + rect.Width)
                return true;

            return false;
        }

        public override double distToClosestHandle(double x, double y)
        {

            double max = 10000;
            double[] val = new double[NumHandles];

            midR = ((row2 - row1) / 2) + row1;
            midC = ((col2 - col1) / 2) + col1;

            val[0] = HMisc.DistancePp(y, x, midR, midC);//0,mid
            val[1] = HMisc.DistancePp(y, x, row1, col1);//1,up left
            val[2] = HMisc.DistancePp(y, x, row1, col2);//2,up right
            val[3] = HMisc.DistancePp(y, x, row2, col2);//3,down right
            val[4] = HMisc.DistancePp(y, x, row2, col1);//4,down left
            val[5] = HMisc.DistancePp(y, x, row1, (col1 + col2) / 2);//5,up mid
            val[6] = HMisc.DistancePp(y, x, (row1 + row2) / 2, col2);//6,down mid
            val[7] = HMisc.DistancePp(y, x, row2, (col1 + col2) / 2);//7,down mid
            val[8] = HMisc.DistancePp(y, x, (row1 + row2) / 2, col1);//8,left mid

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
            Row1 += dy;
            Column1 += dx;
            Row2 += dy;
            Column2 += dx;
        }

        public override void resize(double newX, double newY)
        {
            double len1, len2;
            double tmp;

            double dR = 0;
            double dC = 0;

            switch (activeHandleIdx)
            {
                case 0: //0,mid
                    {
                        len1 = ((row2 - row1) / 2);
                        len2 = ((col2 - col1) / 2);
                        Row1 = newY - len1;
                        Row2 = newY + len1;
                        Column1 = newX - len2;
                        Column2 = newX + len2;
                    }
                    break;
                case 1: //1,up left
                    {
                        dR = newY - row1;
                        dC = newX - col1;
                        Row1 += dR;
                        Column1 += dC;
                        Row2 -= dR;
                        Column2 -= dC;
                    }
                    break;
                case 2: //2,up right
                    {
                        dR = newY - row1;
                        dC = newX - col2;
                        Row1 += dR;
                        Column2 += dC;
                        Row2 -= dR;
                        Column1 -= dC;
                    }
                    break;
                case 3: //3,down right
                    {
                        dR = newY - row2;
                        dC = newX - col2;
                        Row2 += dR;
                        Column2 += dC;
                        Row1 -= dR;
                        Column1 -= dC;
                    }
                    break;
                case 4: //4,down left
                    {
                        dR = newY - row2;
                        dC = newX - col1;
                        Row2 += dR;
                        Column1 += dC;
                        Row1 -= dR;
                        Column2 -= dC;
                    }
                    break;
                case 5: //5,up mid
                    {
                        dR = newY - row1;
                        Row1 += dR;
                        Row2 -= dR;
                    }
                    break;
                case 6: //6,down mid
                    {
                        dC = newX - col2;
                        Column2 += dC;
                        Column1 -= dC;
                    }
                    break;
                case 7: //7,down mid
                    {
                        dR = newY - row2;
                        Row2 += dR;
                        Row1 -= dR;
                    }
                    break;
                case 8: //8,left mid
                    {
                        dC = newX - col1;
                        Column1 += dC;
                        Column2 -= dC;
                    }
                    break;
            }

            if (Row2 <= Row1)
            {
                tmp = Row1;
                Row1 = Row2;
                Row2 = tmp;
            }

            if (Column2 <= Column1)
            {
                tmp = Column1;
                Column1 = Column2;
                Column2 = tmp;
            }

            midR = ((Row2 - Row1) / 2) + Row1;
            midC = ((Column2 - Column1) / 2) + Column1;

        }
    }
}
