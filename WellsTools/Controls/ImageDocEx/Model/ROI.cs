using System;
using hvppleDotNet;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace Wells.Controls.ImageDocEx
{
    [Serializable]
	public class ROI: Model
    {
        protected int   NumHandles;

        [NonSerialized]
        public int activeHandleIdx;

        [NonSerialized]
        public bool Selected = false;

        public object objBinding = null;

        public ROI()
        {
            
        }

		public virtual void createInitROI(double midX, double midY) { }
        
		public virtual void draw(HWindow window,int imageWidth,int imageHeight) { }

        public virtual int ptLocation(double x, double y, int imageWidth, int imageHeight) { return -1; }

        public virtual bool isInRect(RectangleF rect) { return false; }

        public virtual double distToClosestHandle(double x, double y) { return 0.0; }
        
        public virtual void move(double dx, double dy) { }

        public virtual void resize(double x, double y) { }
        
        public double getHandleWidth(int imageWidth, int imageHeight)
        {
            double littleRecSize = 0;
            if (imageHeight < 300) littleRecSize = 1;
            else if (imageHeight < 600) littleRecSize = 2;
            else if (imageHeight < 900) littleRecSize = 3;
            else if (imageHeight < 1200) littleRecSize = 4;
            else if (imageHeight < 1500) littleRecSize = 5;
            else if (imageHeight < 1800) littleRecSize = 6;
            else if (imageHeight < 2100) littleRecSize = 7;
            else if (imageHeight < 2400) littleRecSize = 8;
            else if (imageHeight < 2700) littleRecSize = 9;
            else if (imageHeight < 3000) littleRecSize = 10;
            else if (imageHeight < 3300) littleRecSize = 11;
            else if (imageHeight < 3600) littleRecSize = 12;
            else if (imageHeight < 3900) littleRecSize = 13;
            else if (imageHeight < 4200) littleRecSize = 14;
            else if (imageHeight < 4500) littleRecSize = 15;
            else if (imageHeight < 4800) littleRecSize = 16;
            else if (imageHeight < 5100) littleRecSize = 17;
            else littleRecSize = 18;
            //littleRecSize *= 3.0;
            return littleRecSize;
        }
	}
}
