using System;
using hvppleDotNet;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Drawing;

namespace Wells.Controls.ImageDocEx
{
    public class ROIRectangle2 : ROI
	{
        public double Row
        {
            get { return this.midR; }
            set
            {
                if (value == this.midR) return;
                this.midR = value;
                NotifyPropertyChange("Row");
            }
        }
        
        public double Column
        {
            get { return this.midC; }
            set
            {
                if (value == this.midC) return;
                this.midC = value;
                NotifyPropertyChange("Column");
            }
        }

        public double Phi
        {
            get { return this.phi; }
            set
            {
                if (value == this.phi) return;
                this.phi = value;
                NotifyPropertyChange("Phi");
            }
        }
        
        public double Lenth1
        {
            get { return this.length1; }
            set
            {
                if (value == this.length1) return;
                this.length1 = value;
                NotifyPropertyChange("Lenth1");
            }
        }
        
        public double Lenth2
        {
            get { return this.length2; }
            set
            {
                if (value == this.length2) return;
                this.length2 = value;
                NotifyPropertyChange("Lenth2");
            }
        }

		/// <summary>Half length of the rectangle side, perpendicular to phi</summary>
		private double length1;

		/// <summary>Half length of the rectangle side, in direction of phi</summary>
		private double length2;

		/// <summary>Row coordinate of midpoint of the rectangle</summary>
		private double midR;

		/// <summary>Column coordinate of midpoint of the rectangle</summary>
		private double midC;

		/// <summary>Orientation of rectangle defined in radians.</summary>
		private double phi;


		//auxiliary variables
		HTuple rowsInit;
		HTuple colsInit;
	    public 	HTuple rows;
        public HTuple cols;

		HHomMat2D hom2D, tmp;

		/// <summary>Constructor</summary>
		public ROIRectangle2()
		{
			NumHandles = 10; // 4 corners +  1 midpoint + 1 rotationpoint			
			activeHandleIdx = 4;

            midR = 25;
            midC = 25;
            length1 = 25;
            length2 = 15;
            phi = 0;

            rowsInit = new HTuple(new double[] { -1.0, -1.0, 1.0, 1.0, 0.0, 0.0, 0, -1, 0, 1 });
            colsInit = new HTuple(new double[] { -1.0, 1.0, 1.0, -1.0, 0.0, 1.2, -1, 0, 1, 0 });
            //order        ul ,  ur,   lr,  ll,   mp, arrowMidpoint
            hom2D = new HHomMat2D();
            tmp = new HHomMat2D();

            updateHandlePos();

            Type = "Rectangle2";
        }

        public ROIRectangle2(double row, double col, double phi, double length1, double length2)
        {
            NumHandles = 10; // 4 corners +  1 midpoint + 1 rotationpoint			
            activeHandleIdx = 4;

            this.midR = row;
            this.midC = col;
            this.length1 = length1;
            this.length2 = length2;
            this.phi = phi;

            rowsInit = new HTuple(new double[] { -1.0, -1.0, 1.0, 1.0, 0.0, 0.0, 0, -1, 0, 1 });
            colsInit = new HTuple(new double[] { -1.0, 1.0, 1.0, -1.0, 0.0, 1.2, -1, 0, 1, 0 });
            //order        ul ,  ur,   lr,  ll,   mp, arrowMidpoint
            hom2D = new HHomMat2D();
            tmp = new HHomMat2D();

            updateHandlePos();

            Type = "Rectangle2";
        }
        
		public override void createInitROI(double midX, double midY)
		{
			midR = midY;
			midC = midX;

			length1 = 25;
			length2 = 15;

			phi = 0.0;

            rowsInit = new HTuple(new double[] { -1.0, -1.0, 1.0, 1.0, 0.0, 0.0, 0, -1, 0, 1 });
            colsInit = new HTuple(new double[] { -1.0, 1.0, 1.0, -1.0, 0.0, 1.2, -1, 0, 1, 0 });
            //order        ul ,  ur,   lr,  ll,   mp, arrowMidpoint
            hom2D = new HHomMat2D();
            tmp = new HHomMat2D();

            updateHandlePos();
        }
        
        public override void draw(HWindow window, int imageWidth, int imageHeight)
		{
            window.SetColor(Color);
            window.SetDraw(DrawMode);
            window.SetLineWidth(LineWidth);
            window.SetLineStyle(LineStyle == "dot" ? new HTuple(2, 2) : new HTuple());

            double littleRecSize = getHandleWidth(imageWidth, imageHeight);

            window.DispRectangle2(midR, midC, -phi, length1, length2);//body

            window.SetDraw("fill");
            window.DispArrow(midR, midC, midR + (Math.Sin(phi) * length1 * 1.2), midC + (Math.Cos(phi) * length1 * 1.2), littleRecSize);

            if (Selected)
            {
                for (int i = 0; i < NumHandles; i++)
                {
                    window.DispRectangle2(rows[i].D, cols[i].D, -phi, littleRecSize, littleRecSize);
                    Application.DoEvents();
                }
            }
		}

        public override int ptLocation(double x, double y, int imageWidth, int imageHeight)
        {
            double width = 2 * getHandleWidth(imageWidth, imageHeight);

            return 0;
        }

        public override bool isInRect(RectangleF rect)
        {
            
            return false;
        }

        public override double distToClosestHandle(double x, double y)
		{
			double max = 10000;
			double [] val = new double[NumHandles];


			for (int i=0; i < NumHandles; i++)
				val[i] = HMisc.DistancePp(y, x, rows[i].D, cols[i].D);

			for (int i=0; i < NumHandles; i++)
			{
				if (val[i] < max)
				{
					max = val[i];
					activeHandleIdx = i;
				}
            }
            
            return val[activeHandleIdx];
		}

        public override void move(double dx, double dy)
        {
            midR += dy;
            midC += dx;
        }

        public override void resize(double newX, double newY)
		{
			double vX, vY, x=0, y=0;

			switch (activeHandleIdx)
			{
				case 0:
				case 1:
				case 2:
				case 3:
             
					tmp = hom2D.HomMat2dInvert();
					x = tmp.AffineTransPoint2d(newX, newY, out y);

					length2 = Math.Abs(y);
					length1 = Math.Abs(x);

					checkForRange(x, y);
					break;
				case 4:
					midC = newX;
					midR = newY;
					break;
				case 5:
					vY = newY - rows[4].D;
					vX = newX - cols[4].D;
					phi = Math.Atan2(vY, vX);
					break;
                case 7:
                case 9:
                          		tmp = hom2D.HomMat2dInvert();
					x = tmp.AffineTransPoint2d(newX, newY, out y);

				
				
                    length2 = Math.Abs(y);

					checkForRange(x, y);
					break;
                case 6:
                case 8:
                    		tmp = hom2D.HomMat2dInvert();
					x = tmp.AffineTransPoint2d(newX, newY, out y);


                    length1 = Math.Abs(x);
					checkForRange(x, y);
					break;
			}
			updateHandlePos();
		}//end of method


		/// <summary>
		/// Auxiliary method to recalculate the contour points of 
		/// the rectangle by transforming the initial row and 
		/// column coordinates (rowsInit, colsInit) by the updated
		/// homography hom2D
		/// </summary>
		private void updateHandlePos()
		{
			hom2D.HomMat2dIdentity();
			hom2D = hom2D.HomMat2dTranslate(midC, midR);
			hom2D = hom2D.HomMat2dRotateLocal(phi);
			tmp = hom2D.HomMat2dScaleLocal(length1, length2);
			cols = tmp.AffineTransPoint2d(colsInit, rowsInit, out rows);
		}


		/* This auxiliary method checks the half lengths 
		 * (length1, length2) using the coordinates (x,y) of the four 
		 * rectangle corners (handles 0 to 3) to avoid 'bending' of 
		 * the rectangular ROI at its midpoint, when it comes to a
		 * 'collapse' of the rectangle for length1=length2=0.
		 * */
		private void checkForRange(double x, double y)
		{
			switch (activeHandleIdx)
			{
				case 0:
					if ((x < 0) && (y < 0))
						return;
					if (x >= 0) length1 = 0.01;
					if (y >= 0) length2 = 0.01;
					break;
				case 1:
					if ((x > 0) && (y < 0))
						return;
					if (x <= 0) length1 = 0.01;
					if (y >= 0) length2 = 0.01;
					break;
				case 2:
					if ((x > 0) && (y > 0))
						return;
					if (x <= 0) length1 = 0.01;
					if (y <= 0) length2 = 0.01;
					break;
				case 3:
					if ((x < 0) && (y > 0))
						return;
					if (x >= 0) length1 = 0.01;
					if (y <= 0) length2 = 0.01;
					break;
				default:
					break;
			}
		}
	}//end of class
}//end of namespace
