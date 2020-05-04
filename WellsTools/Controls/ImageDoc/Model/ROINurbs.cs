using System;
using hvppleDotNet;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Wells.Controls.ImageDoc
{
    /// <summary>
    /// This class demonstrates one of the possible implementations for a 
    /// (simple) rectangularly shaped ROI. ROIRectangle1 inherits 
    /// from the base class ROI and implements (besides other auxiliary
    /// methods) all virtual methods defined in ROI.cs.
    /// Since a simple rectangle is defined by two data points, by the upper 
    /// left corner and the lower right corner, we use four values (row1/col1) 
    /// and (row2/col2) as class members to hold these positions at 
    /// any time of the program. The four corners of the rectangle can be taken
    /// as handles, which the user can use to manipulate the size of the ROI. 
    /// Furthermore, we define a midpoint as an additional handle, with which
    /// the user can grab and drag the ROI. Therefore, we declare NumHandles
    /// to be 5 and set the activeHandle to be 0, which will be the upper left
    /// corner of our ROI.
    /// </summary>
    [Serializable]
    public class ROINurbs : ROI
    {





        private string color = "blue";


        private HTuple rows = new HTuple();
        private HTuple cols = new HTuple();

        /// <summary>Constructor</summary>
        public ROINurbs()
        {
            NumHandles = rows.Length ; // 4 corner points + midpoint
            activeHandleIdx = 0;
        }

        public ROINurbs(HTuple rows, HTuple cols)
        {
            createROINurbs(rows, cols);
        }

        public override void createROINurbs(HTuple  rows, HTuple  cols)
        {
            base.createROINurbs(rows, cols);
            this.rows = rows;
            this.cols = cols;
        }
        public override void createROINurbs(double imageHeight)
        {
            double size = 10.0 * getHandleWidth(0, (int)imageHeight);

            double length1 = size * 3;
            double length2 = size * 4;


            base.createROINurbs(rows, cols);
            this.rows = rows;
            this.cols = cols;
        }
        /// <summary>Creates a new ROI instance at the mouse position</summary>
        /// <param name="midX">
        /// x (=column) coordinate for interactive ROI
        /// </param>
        /// <param name="midY">
        /// y (=row) coordinate for interactive ROI
        /// </param>
        ////public override void createROI(List<double> rows, List<double> cols)
        ////{
        ////    //////midR = midY;
        ////    //////midC = midX;

        ////    //////row1 = midR - 25;
        ////    //////col1 = midC - 25;
        ////    //////row2 = midR + 25;
        ////    //////col2 = midC + 25;
        ////}

        /// <summary>Paints the ROI into the supplied window</summary>
        /// <param name="window">HALCON window</param>
        public override void draw(hvppleDotNet.HWindow window, int imageWidth, int imageHeight)
        {
            double littleRecSize = getHandleWidth(imageWidth, imageHeight);

            ////// window.DispObj (row1, col1, row2, col2);

            for (int i = 0; i < rows.Length ; i++)
            {
                window.SetDraw("fill"); 
                window.DispRectangle2(rows[i], cols[i], 0, littleRecSize, littleRecSize);
                window.SetDraw("margin");

                if (i < rows.Length  - 1)
                    window.DispLine((HTuple)rows[i], (HTuple)cols[i], (HTuple)rows[i + 1], (HTuple)cols[i + 1]);
                else
                    window.DispLine((HTuple)rows[i], (HTuple)cols[i], (HTuple)rows[0], (HTuple)cols[0]);
            }

        }

        /// <summary> 
        /// Returns the distance of the ROI handle being
        /// closest to the image point(x,y)
        /// </summary>
        /// <param name="x">x (=column) coordinate</param>
        /// <param name="y">y (=row) coordinate</param>
        /// <returns> 
        /// Distance of the closest ROI handle.
        /// </returns>
        public override double distToClosestHandle(double x, double y, out int iHandleIndex)
        {

            double max = 10000;
            double[] val = new double[rows.Length ];

            //midR = ((row2 - row1) / 2) + row1;
            //midC = ((col2 - col1) / 2) + col1;

            for (int i = 0; i < rows.Length ; i++)
            {
                val[i] = HMisc.DistancePp(y, x, rows[i], cols[i]); // upper left 
            }
            //////val[0] = HMisc.DistancePp(y, x, row1, col1); // upper left 
            //////val[1] = HMisc.DistancePp(y, x, row1, col2); // upper right 
            //////val[2] = HMisc.DistancePp(y, x, row2, col2); // lower right 
            //////val[3] = HMisc.DistancePp(y, x, row2, col1); // lower left 
            //////val[4] = HMisc.DistancePp(y, x, midR, midC); // midpoint 
            //////val[5] = HMisc.DistancePp(y, x, (row1 + row2) / 2, col1);
            //////val[6] = HMisc.DistancePp(y, x, (row1 + row2) / 2, col2);
            //////val[7] = HMisc.DistancePp(y, x, row1, (col1 + col2) / 2);
            //////val[8] = HMisc.DistancePp(y, x, row2, (col1 + col2) / 2);

            for (int i = 0; i < rows.Length  ; i++)
            {
                if (val[i] < max)
                {
                    max = val[i];
                    activeHandleIdx = i;
                }
            }// end of for 

            iHandleIndex = activeHandleIdx;


            return val[activeHandleIdx];
        }

        /// <summary> 
        /// Paints the active handle of the ROI object into the supplied window
        /// </summary>
        /// <param name="window">HALCON window</param>
        public override void displayActive(hvppleDotNet.HWindow window, int imageWidth, int imageHeight)
        {
            double littleRecSize = getHandleWidth(imageWidth, imageHeight);

            window.SetDraw("fill"); 
            window.DispRectangle2(rows[activeHandleIdx], cols[activeHandleIdx], 0, littleRecSize, littleRecSize);
            window.SetDraw("margin");
            ////switch (activeHandleIdx)
            ////{
            ////    case 0:
            ////        window.DispRectangle2(row1, col1, 0, littleRecSize, littleRecSize);
            ////        break;
            ////    case 1:
            ////        window.DispRectangle2(row1, col2, 0, littleRecSize, littleRecSize);
            ////        break;
            ////    case 2:
            ////        window.DispRectangle2(row2, col2, 0, littleRecSize, littleRecSize);
            ////        break;
            ////    case 3:
            ////        window.DispRectangle2(row2, col1, 0, littleRecSize, littleRecSize);
            ////        break;
            ////    case 4:
            ////        window.DispRectangle2(midR, midC, 0, littleRecSize, littleRecSize);
            ////        break;
            ////    case 5:
            ////        window.DispRectangle2((row1 + row2) / 2, col1, 0, littleRecSize, littleRecSize);
            ////        break;
            ////    case 6:
            ////        window.DispRectangle2((row1 + row2) / 2, col2, 0, littleRecSize, littleRecSize);
            ////        break;
            ////    case 7:
            ////        window.DispRectangle2(row1, (col1 + col2) / 2, 0, littleRecSize, littleRecSize);
            ////        break;
            ////    case 8:
            ////        window.DispRectangle2(row2, (col1 + col2) / 2, 0, littleRecSize, littleRecSize);
            ////        break;
            ////}
        }

        /// <summary>Gets the HALCON region described by the ROI</summary>
        public override HRegion  getRegion()
        {
            HRegion region = new HRegion();
            region.GenRegionPolygonFilled(new HTuple (rows ) , new HTuple (cols ) );
            return (HRegion )region;
        }

        /// <summary>
        /// Gets the model information described by 
        /// the interactive ROI
        /// </summary> 
        public override void  getModelData(out HTuple t1,out HTuple t2)
        {
            //return new HTuple();
            t1 = rows;
            t2 = cols;
        }


        /// <summary> 
        /// Recalculates the shape of the ROI instance. Translation is 
        /// performed at the active handle of the ROI object 
        /// for the image coordinate (x,y)
        /// </summary>
        /// <param name="newX">x mouse coordinate</param>
        /// <param name="newY">y mouse coordinate</param>
        public override void moveByHandle(double newX, double newY)
        {
            double len1, len2;
            double tmp;

            //switch (activeHandleIdx)
            //{
            //    case 0: // upper left 
            rows[activeHandleIdx] = newY;
            cols[activeHandleIdx] = newX;
            //        break;
            //    case 1: // upper right 
            //        row1 = newY;
            //        col2 = newX;
            //        break;
            //    case 2: // lower right 
            //        row2 = newY;
            //        col2 = newX;
            //        break;
            //    case 3: // lower left
            //        row2 = newY;
            //        col1 = newX;
            //        break;
            //    case 4: // midpoint 
            //        len1 = ((row2 - row1) / 2);
            //        len2 = ((col2 - col1) / 2);

            //        row1 = newY - len1;
            //        row2 = newY + len1;

            //        col1 = newX - len2;
            //        col2 = newX + len2;

            //        break;
            //    case 5: // upper right 
            //        col1 = newX;
            //        break;
            //    case 6: // lower right 
            //        col2 = newX;
            //        break;
            //    case 7: // lower left
            //        row1 = newY;
            //        break;
            //    case 8: // midpoint 
            //        row2 = newY;
            //        break;
            //}

            //if (row2 <= row1)
            //{
            //    tmp = row1;
            //    row1 = row2;
            //    row2 = tmp;
            //}

            //if (col2 <= col1)
            //{
            //    tmp = col1;
            //    col1 = col2;
            //    col2 = tmp;
            //}

            //midR = ((row2 - row1) / 2) + row1;
            //midC = ((col2 - col1) / 2) + col1;

        }//end of method
    }//end of class
}//end of namespace
