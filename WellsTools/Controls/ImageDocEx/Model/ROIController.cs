using System;
using hvppleDotNet;
using System.Collections;
using System.Collections.Generic;

namespace Wells.Controls.ImageDocEx
{
    public class ROIController
    {
        public bool EditModel = true;
        
        private double currX, currY;
        
        public ArrayList ROIList;
        
        private string activeCol = "green";
        private string activeHdlCol = "red";
        private string inactiveCol = "blue";
        
        protected internal ROIController()
        {
            stateROI = MODE_ROI_NONE;
            ROIList = new ArrayList();
            activeROIidx = -1;
            ModelROI = new HRegion();
            NotifyRCObserver = new IconicDelegate(dummyI);
            deletedIdx = -1;
            currX = currY = -1;
        }
        
        public ArrayList getROIList()
        {
            return ROIList;
        }
        
        public void setDrawColor(string aColor,
                                   string aHdlColor,
                                   string inaColor)
        {
            if (aColor != "")
                activeCol = aColor;
            if (aHdlColor != "")
                activeHdlCol = aHdlColor;
            if (inaColor != "")
                inactiveCol = inaColor;
        }
        
        public void paintData(hvppleDotNet.HWindow window)
        {
            window.SetDraw("margin");
            window.SetLineWidth(1);

            if (ROIList.Count > 0)
            {
                //
                //window.SetColor(inactiveCol);

                window.SetDraw("margin");

                for (int i = 0; i < ROIList.Count; i++)
                {
                    window.SetColor(((ROI)ROIList[i]).Color);
                    window.SetLineStyle(((ROI)ROIList[i]).flagLineStyle);
                    ((ROI)ROIList[i]).draw(window, Convert.ToInt32(viewController.ImgCol2 - viewController.ImgCol1), Convert.ToInt32(viewController.ImgRow2 - viewController.ImgRow1));
                }

                if (activeROIidx != -1)
                {
                    window.SetColor(activeCol);
                    window.SetLineStyle(((ROI)ROIList[activeROIidx]).flagLineStyle);
                    ((ROI)ROIList[activeROIidx]).draw(window, Convert.ToInt32(viewController.ImgCol2 - viewController.ImgCol1), Convert.ToInt32(viewController.ImgRow2 - viewController.ImgRow1));

                    window.SetColor(activeHdlCol);
                    ((ROI)ROIList[activeROIidx]).displayActive(window, Convert.ToInt32(viewController.ImgCol2 - viewController.ImgCol1), Convert.ToInt32(viewController.ImgRow2 - viewController.ImgRow1));
                }
            }
        }
        
        public int mouseDownAction(double imgX, double imgY)
        {
            int idxROI = -1;
            double max = 10000, dist = 0;
            double epsilon = 100.0;          //maximal shortest distance to one of
                                            //the handles

            if (roiMode != null)             //either a new ROI object is created
            {
                roiMode.createROI(imgX, imgY);
                ROIList.Add(roiMode);
                roiMode = null;
                activeROIidx = ROIList.Count - 1;
                viewController.repaint();

                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
            else if (ROIList.Count > 0)     // ... or an existing one is manipulated
            {
                activeROIidx = -1;

                int activeHandleIndex = -1;

                for (int i = 0; i < ROIList.Count; i++)
                {
                    dist = ((ROI)ROIList[i]).distToClosestHandle(imgX, imgY, out activeHandleIndex);
                    if ((dist < max) && (dist < epsilon))
                    {
                        max = dist;
                        idxROI = i;
                    }
                }//end of for

                if (idxROI >= 0)
                {
                    activeROIidx = idxROI;
                    NotifyRCObserver(ROIController.EVENT_ACTIVATED_ROI);

                    if (activeHandleIndex > -1)
                    {
                        string name = ROIList[idxROI].GetType().Name;
                        if(name == "ROIRectangle1")
                        {
                            switch(activeHandleIndex)
                            {
                                case 0://左上
                                    viewController.viewPort.hvppleWindow.SetMshape("Size NWSE");
                                    break;
                                case 1://右上
                                    viewController.viewPort.hvppleWindow.SetMshape("Size NESW");
                                    break;
                                case 2://右下
                                    viewController.viewPort.hvppleWindow.SetMshape("Size NWSE");
                                    break;
                                case 3://左下
                                    viewController.viewPort.hvppleWindow.SetMshape("Size NESW");
                                    break;
                                case 4://中心
                                    viewController.viewPort.hvppleWindow.SetMshape("Size All");
                                    break;
                                case 5://左中
                                    viewController.viewPort.hvppleWindow.SetMshape("Size WE");
                                    break;
                                case 6://右中
                                    viewController.viewPort.hvppleWindow.SetMshape("Size WE");
                                    break;
                                case 7://上中
                                    viewController.viewPort.hvppleWindow.SetMshape("Size S");
                                    break;
                                case 8://下中
                                    viewController.viewPort.hvppleWindow.SetMshape("Size S");
                                    break;
                                default://
                                    viewController.viewPort.hvppleWindow.SetMshape("default");
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    viewController.viewPort.hvppleWindow.SetMshape("default");
                }

                

                viewController.repaint();
            }
            return activeROIidx;
        }
        
        public void mouseMoveAction(double newX, double newY)
        {
            try
            {
                if (EditModel == false) return;
                if ((newX == currX) && (newY == currY))
                    return;

                ((ROI)ROIList[activeROIidx]).moveByHandle(newX, newY);
                viewController.repaint();
                currX = newX;
                currY = newY;
                NotifyRCObserver(ROIController.EVENT_MOVING_ROI);
            }
            catch (Exception)
            {
                //没有显示roi的时候 移动鼠标会报错
            }

        }
    }//end of class
}//end of namespace
