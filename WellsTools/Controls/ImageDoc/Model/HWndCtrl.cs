using System;
using System.Collections;
using hvppleDotNet;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Wells.Controls.ImageDoc
{
    /// <summary>
    /// This class works as a wrapper class for the HALCON window
    /// HWindow. HWndCtrl is in charge of the visualization.
    /// You can move and zoom the visible image part by using GUI component 
    /// inputs or with the mouse. The class HWndCtrl uses a graphics stack 
    /// to manage the iconic objects for the display. Each object is linked 
    /// to a graphical context, which determines how the object is to be drawn.
    /// The context can be changed by calling changeGraphicSettings().
    /// The graphical "modes" are defined by the class GraphicsContext and 
    /// map most of the dev_set_* operators provided in HDevelop.
    /// </summary>
    public class HWndCtrl
	{
		/// <summary>No action is performed on mouse events</summary>
		public const int MODE_VIEW_NONE       = 10;

		/// <summary>Zoom is performed on mouse events</summary>
		public const int MODE_VIEW_ZOOM       = 11;

		/// <summary>Move is performed on mouse events</summary>
		public const int MODE_VIEW_MOVE       = 12;

		/// <summary>Magnification is performed on mouse events</summary>
		public const int MODE_VIEW_ZOOMWINDOW	= 13;


		public const int MODE_INCLUDE_ROI     = 1;

		public const int MODE_EXCLUDE_ROI     = 2;


		/// <summary>
		/// Constant describes delegate message to signal new image
		/// </summary>
		public const int EVENT_UPDATE_IMAGE   = 31;
		/// <summary>
		/// Constant describes delegate message to signal error
		/// when reading an image from file
		/// </summary>
		public const int ERR_READING_IMG      = 32;
		/// <summary> 
		/// Constant describes delegate message to signal error
		/// when defining a graphical context
		/// </summary>
		public const int ERR_DEFINING_GC      = 33;

		/// <summary> 
		/// Maximum number of HALCON objects that can be put on the graphics 
		/// stack without loss. For each additional object, the first entry 
		/// is removed from the stack again.
		/// </summary>
		private const int MAXNUMOBJLIST       = 50;//原始值为50 实际上2都可以,因这里只是存储背景图片

        private double SHOW_DELTA = 0.4;//图片外额外显示比例


		private int    stateView;
		private bool   mousePressed = false;
		private double startX,startY;

		/// <summary>HALCON window</summary>
		public HWindowControl viewPort;

		/// <summary>
		/// Instance of ROIController, which manages ROI interaction
		/// </summary>
		public  ROIController roiManager;

		/* dispROI is a flag to know when to add the ROI models to the 
		   paint routine and whether or not to respond to mouse events for 
		   ROI objects */
		private int           dispROI;

        /// <summary>
        /// 是否做为一个静态窗口显示，不包含缩放编辑等功能
        /// </summary>
        public bool isStaticWnd = false;
        //开启编辑模式
        public bool EditModel = true;

		/* Basic parameters, like dimension of window and displayed image part */
		private int   windowWidth;
		private int   windowHeight;
		internal  int   imageWidth;
        internal int imageHeight;

		private int[] CompRangeX;
		private int[] CompRangeY;


		private int    prevCompX, prevCompY;
		private double stepSizeX, stepSizeY;


		/* Image coordinates, which describe the image part that is displayed  
		   in the HALCON window */
		public   double ImgRow1, ImgCol1, ImgRow2, ImgCol2;

        /// <summary>
        /// 记录图片当前显示的宽高，实际在控件中的值，单位跟随控件长度单位，并非图片像素值
        /// </summary>
        public int imageShowWidth, imageShowHeight;
        
		private HWindow ZoomWindow;

		private double  xScale;//控件一个长度代表多少像素
        private double yScale;
        private double originScale;

        private double  zoomAddOn;
		private int     zoomWndSize;

        /// <summary>
        /// 当前字体大小尺寸，文字创建时设定了字体，当图片放大缩小后，字体也要跟随变化
        /// </summary>
        private double currentTextSize = 0;

        /// <summary> 
        /// List of HALCON objects to be drawn into the HALCON window. 
        /// The list shouldn't contain more than MAXNUMOBJLIST objects, 
        /// otherwise the first entry is removed from the list.
        /// </summary>
        public ArrayList HObjImageList;

		/// <summary>
		/// Instance that describes the graphical context for the
		/// HALCON window. According on the graphical settings
		/// attached to each HALCON object, this graphical context list 
		/// is updated constantly.
		/// </summary>
		private GraphicsContext	mGC;

        /// <summary>
        /// 是否显示文字信息
        /// </summary>
        private bool isShowMessage = true;

        /// <summary>
        /// 是否显示中间十字线
        /// </summary>
        private bool isShowCross = false;

        /// <summary>
        /// 是否显示region区域
        /// </summary>
        private bool isShowHRegion = true;

        /// <summary>
        /// 是否选中，选中时画边界线
        /// </summary>
        private bool isSelected = false;

        public bool Selected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                repaint();
            }
        }


        private ImageMode imageMode = ImageMode.Origin;

        public ImageMode ImageMode
        {
            get { return imageMode; }
            set
            {
                imageMode = value;
            }
        }

        /// <summary>
        /// 右键菜单
        /// </summary>
        System.Windows.Forms.ContextMenuStrip hv_MenuStrip = null;

        public delegate void ShowZoomPercent(double per);
        public ShowZoomPercent showZoomPercent = null;
        
        /// <summary> 
        /// Initializes the image dimension, mouse delegation, and the 
        /// graphical context setup of the instance.
        /// </summary>
        /// <param name="view"> HALCON window </param>
        protected internal HWndCtrl(HWindowControl view)
		{
			viewPort = view;
			stateView = MODE_VIEW_NONE;
			windowWidth = viewPort.Size.Width;
			windowHeight = viewPort.Size.Height;
            
			zoomAddOn = Math.Pow(0.9, 5);
			zoomWndSize = 150;

			/*default*/
			CompRangeX = new int[] { 0, 100 };
			CompRangeY = new int[] { 0, 100 };

			prevCompX = prevCompY = 0;

			dispROI = MODE_INCLUDE_ROI;//1;

			viewPort.HMouseUp += new hvppleDotNet.HMouseEventHandler(this.mouseUp);
			viewPort.HMouseDown += new hvppleDotNet.HMouseEventHandler(this.mouseDown);
            viewPort.HMouseWheel += new hvppleDotNet.HMouseEventHandler(this.HMouseWheel);
			viewPort.HMouseMove += new hvppleDotNet.HMouseEventHandler(this.mouseMoved);
            
			// graphical stack 
			HObjImageList = new ArrayList(20);
			mGC = new GraphicsContext();
		}

        public void setContextMenuStrip(System.Windows.Forms.ContextMenuStrip strip)
        {
            hv_MenuStrip = strip;
        }

        public void funcMeasureLineDistance()
        {
            viewPort.Focus();

            HWindow window = viewPort.hvppleWindow;

            HWndMessage message = new HWndMessage(clsWellsLanguage.getString(126), 20, 20, 20, "green", "window");
            message.DispMessage(window, message.coordSystem, 1);

            isStaticWnd = true;

            viewPort.ContextMenuStrip = null;

            double r1, c1, r2, c2;
            HTuple dd;

            //获取当前显示信息
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            int hv_lineWidth;
            
            window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);

            hv_lineWidth = (int)window.GetLineWidth();
            string hv_Draw = window.GetDraw();
            window.SetLineWidth(1);//设置线宽
            window.SetLineStyle(new HTuple());
            window.SetColor("green");//画点的颜色

            window.DrawLine(out r1, out c1, out r2, out c2);
            window.DispLine(r1, c1, r2, c2);

            //恢复窗口显示信息
            window.SetRgb(hv_Red, hv_Green, hv_Blue);
            window.SetLineWidth(hv_lineWidth);
            window.SetDraw(hv_Draw);


            HOperatorSet.DistancePp(r1, c1, r2, c2, out dd);
            double dr = Math.Abs(r2 - r1);
            double dc = Math.Abs(c2 - c1);

            Wells.class_Public.Show(null, string.Format(clsWellsLanguage.getString(127), dd.D, dc, dr), clsWellsLanguage.getString(128), MessageBoxButtons.OK, MessageBoxIcon.Information);


            viewPort.ContextMenuStrip = hv_MenuStrip;

            isStaticWnd = false;

            repaint(window);
        }

        public void funcShowGrayHisto()
        {
            viewPort.Focus();

            HImage image = null;

            if (HObjImageList.Count > 0)
                image = new HImage(((HObjectEntry)HObjImageList[0]).HObj);

            if(image == null)
            {
                class_Public.Show(null, clsWellsLanguage.getString(122), clsWellsLanguage.getString(1), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            HWindow window = viewPort.hvppleWindow;

            HWndMessage message = new HWndMessage(clsWellsLanguage.getString(125), 20, 20, 20, "green", "window");
            message.DispMessage(window, message.coordSystem, 1);

            isStaticWnd = true;

            viewPort.ContextMenuStrip = null;

            double r1, c1, r2, c2;
            //HTuple dd;

            //获取当前显示信息
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            int hv_lineWidth;
            
            window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);
            hv_lineWidth = (int)window.GetLineWidth();
            string hv_Draw = window.GetDraw();

            window.SetLineWidth(1);//设置线宽
            window.SetLineStyle(new HTuple());
            window.SetColor("green");//画点的颜色


            window.DrawRectangle1(out r1, out c1, out r2, out c2);
            window.DispRectangle1(r1, c1, r2, c2);
            Form frm = new Form();
            frm.Text = "GrayHisto";
            frm.StartPosition = FormStartPosition.CenterParent;

            ThresholdUnit.ThresholdUnit thresUnit = new ThresholdUnit.ThresholdUnit();
            Size size = thresUnit.Size;
            size.Height = (int)(size.Height + 50);
            size.Width = (int)(size.Width + 50);
            frm.Size = size;
            frm.Controls.Add(thresUnit);
            thresUnit.Dock = DockStyle.Fill;
            HTuple grayVals;
            grayVals = class_Hvpple.getGrayHisto(image, new HTuple(r1, c1, r2, c2));
            thresUnit.setAxisAdaption(ThresholdUnit.ThresholdPlot.AXIS_RANGE_INCREASING);
            thresUnit.setLabel(clsWellsLanguage.getString(123), clsWellsLanguage.getString(124));
            thresUnit.setFunctionPlotValue(grayVals);

            thresUnit.computeStatistics(grayVals);

            frm.ShowDialog();


            //window.DrawLine(out r1, out c1, out r2, out c2);
            //window.DispLine(r1, c1, r2, c2);


            //恢复窗口显示信息
            window.SetRgb(hv_Red, hv_Green, hv_Blue);
            window.SetLineWidth(hv_lineWidth);
            window.SetDraw(hv_Draw);

            viewPort.ContextMenuStrip = hv_MenuStrip;

            isStaticWnd = false;

            repaint();
        }
        
        private void HMouseWheel(object sender, HMouseEventArgs e)
        {
            //关闭缩放事件
            if (isStaticWnd)
            {
                return;
            }

            double scale;

            if (e.Delta > 0)
                scale = 0.9;
            else
                scale = 1 / 0.9;

            zoomImage(e.X, e.Y, scale);
 
        }
		/// <summary>
		/// Read dimensions of the image to adjust own window settings
		/// </summary>
		/// <param name="image">HALCON image</param>
		private void setImagePart(HImage image)
		{
			string s;
			int w,h;

			image.GetImagePointer1(out s, out w, out h);
			setImagePart(0, 0, h, w);
		}


		/// <summary>
		/// Adjust window settings by the values supplied for the left 
		/// upper corner and the right lower corner
		/// </summary>
		/// <param name="r1">y coordinate of left upper corner</param>
		/// <param name="c1">x coordinate of left upper corner</param>
		/// <param name="r2">y coordinate of right lower corner</param>
		/// <param name="c2">x coordinate of right lower corner</param>
		private void setImagePart(int r1, int c1, int r2, int c2)//实际这里调用时，r1,r2必须为0，r2,c2必须为图片像素长宽
		{
			ImgRow1 = r1;
			ImgCol1 = c1;
			ImgRow2 = imageHeight = r2;
			ImgCol2 = imageWidth = c2;
            
            System.Drawing.Rectangle rect = viewPort.ImagePart;

            xScale = (double)imageWidth / viewPort.Width;
            yScale = (double)imageHeight / viewPort.Height;

            if (imageMode == ImageMode.Origin)
                originScale = xScale = yScale = Math.Max(xScale, yScale);
            else
                originScale = xScale;

            if (showZoomPercent != null)
                showZoomPercent(xScale == 0 ? 1 : 1 / xScale);

            ImgCol1 = -(viewPort.Width * xScale - imageWidth) / 2;
            ImgCol2 = (viewPort.Width * xScale - imageWidth) / 2 + imageWidth;
            ImgRow1 = -(viewPort.Height * yScale - imageHeight) / 2;
            ImgRow2 = (viewPort.Height * yScale - imageHeight) / 2 + imageHeight;

            imageShowWidth = (int)(imageWidth / xScale + viewPort.Width * SHOW_DELTA);
            imageShowHeight = (int)(imageHeight / yScale + viewPort.Height * SHOW_DELTA);


            rect.X = (int)ImgCol1;
			rect.Y = (int)ImgRow1;
            rect.Height = (int)(ImgRow2 - ImgRow1);
            rect.Width = (int)(ImgCol2 - ImgCol1);
			viewPort.ImagePart = rect;
		}


		/// <summary>
		/// Sets the view mode for mouse events in the HALCON window
		/// (zoom, move, magnify or none).
		/// </summary>
		/// <param name="mode">One of the MODE_VIEW_* constants</param>
        protected internal void setViewState(int mode)
		{
			stateView = mode;

			if (roiManager != null)
				roiManager.resetROI();
		}
        
		/// <summary>
		/// Paint or don't paint the ROIs into the HALCON window by 
		/// defining the parameter to be equal to 1 or not equal to 1.
		/// </summary>
		public void setDispLevel(int mode)
		{
			dispROI = mode;
		}

		/****************************************************************************/
		/*                          graphical element                               */
		/****************************************************************************/
		private void zoomImage(double x, double y, double scale)
		{
            //关闭缩放事件
            if (isStaticWnd)
            {
                return;
            }

            double tmpScale = 1;
            tmpScale = scale * xScale;

            if (xScale < 0.1 && tmpScale < xScale)
            {
                //超过一定缩放比例就不在缩放
                //resetWindow();
                return;
            }
            if (xScale > 100 && tmpScale > xScale)
            {
                //超过一定缩放比例就不在缩放
                //resetWindow();
                return;
            }
            xScale = tmpScale;
            yScale *= scale;

            if (showZoomPercent != null)
                showZoomPercent(xScale == 0 ? 1 : 1 / xScale);

            double lengthC, lengthR;
			double percentC, percentR;

			percentC = (x - ImgCol1) / (ImgCol2 - ImgCol1);
			percentR = (y - ImgRow1) / (ImgRow2 - ImgRow1);

			lengthC = (ImgCol2 - ImgCol1) * scale;
			lengthR = (ImgRow2 - ImgRow1) * scale;

			ImgCol1 = x - lengthC * percentC;
			ImgCol2 = x + lengthC * (1 - percentC);

			ImgRow1 = y - lengthR * percentR;
			ImgRow2 = y + lengthR * (1 - percentR);

            imageShowWidth = (int)(imageShowWidth / scale);
            imageShowHeight = (int)(imageShowHeight / scale);

            normalImagePart(); 

			System.Drawing.Rectangle rect = viewPort.ImagePart;
			rect.X = (int)Math.Round(ImgCol1);
			rect.Y = (int)Math.Round(ImgRow1);
            rect.Width = (int)Math.Round(ImgCol2 - ImgCol1);
            rect.Height = (int)Math.Round(ImgRow2 - ImgRow1);
            viewPort.ImagePart = rect;
            
			repaint();
		}

		/// <summary>
		/// Scales the image in the HALCON window according to the 
		/// value scaleFactor
		/// </summary>
		public void zoomImageByPercent(double percent)
		{
            double midX = 0, midY = 0;
            midX = (ImgCol2 - ImgCol1) / 2;
            midY = (ImgRow2 - ImgRow1) / 2;
            zoomImage(midX, midY, 1 / percent / xScale);
		}
        
		/// <summary>
		/// Recalculates the image-window-factor, which needs to be added to 
		/// the scale factor for zooming an image. This way the zoom gets 
		/// adjusted to the window-image relation, expressed by the equation 
		/// imageWidth/viewPort.Width.
		/// </summary>
		public void setZoomWndFactor()
		{
			//zoomWndFactor = ((double)imageWidth / viewPort.Width);
		}

		/// <summary>
		/// Sets the image-window-factor to the value zoomF
		/// </summary>
		public void setZoomWndFactor(double zoomF)
		{
			//zoomWndFactor = zoomF;
		}

		/*******************************************************************/
		private void moveImage(double motionX, double motionY)
		{
			ImgRow1 += -motionY;
			ImgRow2 += -motionY;

			ImgCol1 += -motionX;
			ImgCol2 += -motionX;

            normalImagePart();

            System.Drawing.Rectangle rect = viewPort.ImagePart;
			rect.X = (int)Math.Round(ImgCol1);
			rect.Y = (int)Math.Round(ImgRow1);
            rect.Width = (int)Math.Round(ImgCol2 - ImgCol1);
            rect.Height = (int)Math.Round(ImgRow2 - ImgRow1);
			viewPort.ImagePart = rect;

			repaint();
		}


		/// <summary>
		/// Resets all parameters that concern the HALCON window display 
		/// setup to their initial values and clears the ROI list.
		/// </summary>
        protected internal void resetAll()
		{
            setImagePart(0, 0, imageHeight, imageWidth);
            
            if (roiManager != null)
				roiManager.reset();
		}

        protected internal void resetWindow()
		{
            setImagePart(0, 0, imageHeight, imageWidth);
		}


		/*************************************************************************/
		/*      			 Event handling for mouse	   	                     */
		/*************************************************************************/
		private void mouseDown(object sender, hvppleDotNet.HMouseEventArgs e)
		{
            //关闭缩放事件
            if (isStaticWnd)
            {
                return;
            }

            stateView = MODE_VIEW_MOVE;

            if (e.Button == MouseButtons.Left)
                mousePressed = true;

			int activeROIidx = -1;

			if (roiManager != null && (dispROI == MODE_INCLUDE_ROI))
			{
				activeROIidx = roiManager.mouseDownAction(e.X, e.Y);
			}

			if (activeROIidx == -1)
			{
                switch (stateView)
				{
					case MODE_VIEW_MOVE:
						startX = e.X;
						startY = e.Y;
						break;

					case MODE_VIEW_NONE:
						break;
					case MODE_VIEW_ZOOMWINDOW:
						activateZoomWindow((int)e.X, (int)e.Y);
						break;
					default:
						break;
				}
			}
            //end of if
        }

		/*******************************************************************/
		private void activateZoomWindow(int X, int Y)
		{
			double posX, posY;
			int zoomZone;

			if (ZoomWindow != null)
				ZoomWindow.Dispose();

			HOperatorSet.SetSystem("border_width", 10);
			ZoomWindow = new HWindow();

			posX = ((X - ImgCol1) / (ImgCol2 - ImgCol1)) * viewPort.Width;
			posY = ((Y - ImgRow1) / (ImgRow2 - ImgRow1)) * viewPort.Height;

			zoomZone = (int)((zoomWndSize / 2) * xScale * zoomAddOn);
			ZoomWindow.OpenWindow((int)posY - (zoomWndSize / 2), (int)posX - (zoomWndSize / 2),
								   zoomWndSize, zoomWndSize,
								   viewPort.hvppleID, "visible", "");
			ZoomWindow.SetPart(Y - zoomZone, X - zoomZone, Y + zoomZone, X + zoomZone);
			repaint(ZoomWindow);
			ZoomWindow.SetColor("dim gray");
		}

        public void raiseMouseLeave()
        {
            mousePressed = false;

            if (roiManager != null
                && (roiManager.activeROIidx != -1)
                && (dispROI == MODE_INCLUDE_ROI))
            {
                
            }
            else if (stateView == MODE_VIEW_ZOOMWINDOW)
            {
                ZoomWindow.Dispose();
            }
        }
		/*******************************************************************/
		private void mouseUp(object sender, hvppleDotNet.HMouseEventArgs e)
		{
            //关闭缩放事件
            if (isStaticWnd)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
                mousePressed = false;

            viewPort.hvppleWindow.SetMshape("default");

            if (roiManager != null
				&& (roiManager.activeROIidx != -1)
				&& (dispROI == MODE_INCLUDE_ROI))
			{
				
			}
			else if (stateView == MODE_VIEW_ZOOMWINDOW)
			{
				ZoomWindow.Dispose();
			}
		}

		/*******************************************************************/
		private void mouseMoved(object sender, hvppleDotNet.HMouseEventArgs e)
		{
            //关闭缩放事件
            if (isStaticWnd)
            {
                return;
            }

            double motionX = 0, motionY = 0;
			double posX, posY;
			double zoomZone;


			if (!mousePressed)
				return;

			if (roiManager != null && (roiManager.activeROIidx != -1) && (dispROI == MODE_INCLUDE_ROI))
			{
				roiManager.mouseMoveAction(e.X, e.Y);
			}
			else if (stateView == MODE_VIEW_MOVE)
			{
                if (imageShowWidth > viewPort.Width || imageShowHeight > viewPort.Height)
                {
                    motionX = ((e.X - startX));
                    motionY = ((e.Y - startY));
                    if (((int)motionX != 0) || ((int)motionY != 0))
                    {
                        moveImage(motionX, motionY);
                        startX = e.X - motionX;
                        startY = e.Y - motionY;
                    }
                }
			}
			else if (stateView == MODE_VIEW_ZOOMWINDOW)
			{
				HSystem.SetSystem("flush_graphic", "false");
				ZoomWindow.ClearWindow();


				posX = ((e.X - ImgCol1) / (ImgCol2 - ImgCol1)) * viewPort.Width;
				posY = ((e.Y - ImgRow1) / (ImgRow2 - ImgRow1)) * viewPort.Height;
				zoomZone = (zoomWndSize / 2) * xScale * zoomAddOn;

				ZoomWindow.SetWindowExtents((int)posY - (zoomWndSize / 2),
											(int)posX - (zoomWndSize / 2),
											zoomWndSize, zoomWndSize);
				ZoomWindow.SetPart((int)(e.Y - zoomZone), (int)(e.X - zoomZone),
								   (int)(e.Y + zoomZone), (int)(e.X + zoomZone));
				repaint(ZoomWindow);

				HSystem.SetSystem("flush_graphic", "true");
				ZoomWindow.DispLine(-100.0, -100.0, -100.0, -100.0);
			}
		}

		/// <summary>
		/// To initialize the move function using a GUI component, the HWndCtrl
		/// first needs to know the range supplied by the GUI component. 
		/// For the x direction it is specified by xRange, which is 
		/// calculated as follows: GuiComponentX.Max()-GuiComponentX.Min().
		/// The starting value of the GUI component has to be supplied 
		/// by the parameter Init
		/// </summary>
		public void setGUICompRangeX(int[] xRange, int Init)
		{
			int cRangeX;

			CompRangeX = xRange;
			cRangeX = xRange[1] - xRange[0];
			prevCompX = Init;
			stepSizeX = ((double)imageWidth / cRangeX) * (imageWidth / windowWidth);

		}

		/// <summary>
		/// To initialize the move function using a GUI component, the HWndCtrl
		/// first needs to know the range supplied by the GUI component. 
		/// For the y direction it is specified by yRange, which is 
		/// calculated as follows: GuiComponentY.Max()-GuiComponentY.Min().
		/// The starting value of the GUI component has to be supplied 
		/// by the parameter Init
		/// </summary>
		public void setGUICompRangeY(int[] yRange, int Init)
		{
			int cRangeY;

			CompRangeY = yRange;
			cRangeY = yRange[1] - yRange[0];
			prevCompY = Init;
			stepSizeY = ((double)imageHeight / cRangeY) * (imageHeight / windowHeight);
		}


		/// <summary>
		/// Resets to the starting value of the GUI component.
		/// </summary>
		public void resetGUIInitValues(int xVal, int yVal)
		{
			prevCompX = xVal;
			prevCompY = yVal;
		}

		/// <summary>
		/// Moves the image by the value valX supplied by the GUI component
		/// </summary>
		public void moveXByGUIHandle(int valX)
		{
			double motionX;

			motionX = (valX - prevCompX) * stepSizeX;

			if (motionX == 0)
				return;

			moveImage(motionX, 0.0);
			prevCompX = valX;
		}


		/// <summary>
		/// Moves the image by the value valY supplied by the GUI component
		/// </summary>
		public void moveYByGUIHandle(int valY)
		{
			double motionY;

			motionY = (valY - prevCompY) * stepSizeY;

			if (motionY == 0)
				return;

			moveImage(0.0, motionY);
			prevCompY = valY;
		}

		/// <summary>
		/// Zooms the image by the value valF supplied by the GUI component
		/// </summary>
		public void zoomByGUIHandle(double valF)
		{
			double x, y, scale;
			double prevScaleC;



			x = (ImgCol1 + (ImgCol2 - ImgCol1) / 2);
			y = (ImgRow1 + (ImgRow2 - ImgRow1) / 2);

			prevScaleC = (double)((ImgCol2 - ImgCol1) / imageWidth);
			scale = ((double)1.0 / prevScaleC * (100.0 / valF));

			zoomImage(x, y, scale);
		}


        public bool showMessage()
        {
            isShowMessage = !isShowMessage;
            repaint();
            return isShowMessage;
        }

        public bool showCross()
        {
            isShowCross = !isShowCross;
            repaint();
            return isShowCross;
        }
        
        public bool showROI()
        {
            dispROI = dispROI == MODE_EXCLUDE_ROI ? MODE_INCLUDE_ROI : MODE_EXCLUDE_ROI;
            repaint();
            return dispROI == MODE_INCLUDE_ROI;
        }

        public bool showHRegion()
        {
            isShowHRegion = !isShowHRegion;
            repaint();
            return isShowHRegion;
        }

        /// <summary>
        /// Triggers a repaint of the HALCON window
        /// </summary>
        public void repaint()
		{
			repaint(viewPort.hvppleWindow);
		}

		/// <summary>
		/// Repaints the HALCON window 'window'
		/// </summary>
		public void repaint(hvppleDotNet.HWindow window)
		{
            try
            {


			    int count = HObjImageList.Count;
			    HObjectEntry entry;

			    HSystem.SetSystem("flush_graphic", "false");
			    window.ClearWindow();
			    mGC.stateOfSettings.Clear();

                //显示图片及文字
                showHObjectList();

                //显示region
                showHRegionList();

                //显示ROI
                showROIList();

                showHat(window);

                showBorder(window);

			    HSystem.SetSystem("flush_graphic", "true");

                //注释了下面语句,会导致窗口无法实现缩放和拖动
                window.SetColor("dim gray");
                window.DispLine(-100.0, -100.0, -101.0, -101.0);

            }
            catch (Exception exc)
            {

            }
		}


        /// <summary>
        /// 显示图片及文字信息
        /// </summary>
        private void showHObjectList()
        {
            HObjectEntry entry;
            try
            {
                for (int igg = 0; igg < HObjImageList.Count; igg++)
                {
                    entry = ((HObjectEntry)HObjImageList[igg]);
                    if (igg == 0)//第一个元素必须是图片
                    {
                        if (entry.HObj != null && entry.HObj.IsInitialized())
                        {
                            mGC.applyContext(viewPort.hvppleWindow, entry.gContext);
                            viewPort.hvppleWindow.DispObj(entry.HObj);
                        }
                    }
                    else//其余全存文字
                    {
                        if (entry.Message != null && isShowMessage)
                        {
                            //double zoom = 1.0 / xScale;
                            double zoom = entry.Message.coordSystem == "image" ? 1.0 / xScale : 1.0 / originScale;
                            double sizeTmp = entry.Message.changeDisplayFontSize(viewPort.hvppleWindow, zoom, currentTextSize);
                            currentTextSize = sizeTmp;
                            entry.Message.DispMessage(viewPort.hvppleWindow, entry.Message.coordSystem);
                        }
                    }
                }
            }
            catch(Exception exc)
            {

            }
        }

        /// <summary>
        /// 显示ROI区域
        /// </summary>
        private void showROIList()
        {
            if (roiManager != null && (dispROI == MODE_INCLUDE_ROI))
                roiManager.paintData(viewPort.hvppleWindow);
        }

        /// <summary>
        /// 显示中心十字线
        /// </summary>
        /// <param name="window"></param>
        private void showHat(HWindow window)
        {
            if (isShowCross)
            {
                //获取当前显示信息
                HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
                int hv_lineWidth;

                window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);

                hv_lineWidth = (int)window.GetLineWidth();
                string hv_Draw = window.GetDraw();
                window.SetLineWidth(1);//设置线宽
                window.SetLineStyle(new HTuple());
                window.SetColor("green");//十字架显示颜色
                double CrossCol = (double)imageWidth / 2.0, CrossRow = (double)imageHeight / 2.0;
                double borderWidth = (double)imageWidth / 50.0;
                CrossCol = (double)imageWidth / 2.0;
                CrossRow = (double)imageHeight / 2.0;
                //竖线
                //window.DispLine(0, CrossCol, CrossRow - 50, CrossCol);

                //window.DispLine(CrossRow + 50, CrossCol, imageHeight, CrossCol);

                window.DispPolygon(new HTuple(0, CrossRow - 50), new HTuple(CrossCol, CrossCol));
                window.DispPolygon(new HTuple(CrossRow + 50, imageHeight), new HTuple(CrossCol, CrossCol));


                //中心点
                window.DispPolygon(new HTuple(CrossRow - 2, CrossRow + 2), new HTuple(CrossCol, CrossCol));
                window.DispPolygon(new HTuple(CrossRow, CrossRow), new HTuple(CrossCol - 2, CrossCol + 2));

                //横线

                window.DispPolygon(new HTuple(CrossRow, CrossRow), new HTuple(0, CrossCol - 50));
                window.DispPolygon(new HTuple(CrossRow, CrossRow), new HTuple(CrossCol + 50, imageWidth));


                //window.DispLine(CrossRow, 0, CrossRow, CrossCol - 50);
                //window.DispLine(CrossRow, CrossCol + 50, CrossRow, imageWidth);

                //恢复窗口显示信息
                window.SetRgb(hv_Red, hv_Green, hv_Blue);
                window.SetLineWidth(hv_lineWidth);
                window.SetDraw(hv_Draw);
            }
            else
            {
                window.SetColor("dim gray");
                window.DispLine(-100.0, -100.0, -101.0, -101.0);
            }
        }


        /// <summary>
        /// 显示控件边框，默认黄色
        /// </summary>
        private void showBorder(HWindow window)
        {
            if (isSelected)
            {
                //获取当前显示信息
                HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
                int hv_lineWidth;

                window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);

                hv_lineWidth = (int)window.GetLineWidth();
                string hv_Draw = window.GetDraw();
                window.SetLineWidth(3);//设置线宽
                window.SetLineStyle(new HTuple());
                window.SetColor("green");//边框显示颜色

                window.DispLine(ImgRow1, ImgCol1, ImgRow2, ImgCol1);
                window.DispLine(ImgRow1, ImgCol1, ImgRow1, ImgCol2);
                window.DispLine(ImgRow2, ImgCol1, ImgRow2, ImgCol2);
                window.DispLine(ImgRow1, ImgCol2, ImgRow2, ImgCol2);

                //恢复窗口显示信息
                window.SetRgb(hv_Red, hv_Green, hv_Blue);
                window.SetLineWidth(hv_lineWidth);
                window.SetDraw(hv_Draw);
            }
        }

        /********************************************************************/
        /*                      GRAPHICSSTACK                               */
        /********************************************************************/

        /// <summary>
        /// Adds an iconic object to the graphics stack similar to the way
        /// it is defined for the HDevelop graphics stack.
        /// </summary>
        /// <param name="obj">Iconic object</param>
        public void addIconicVar(HObject img)
		{
            //先把HObjImageList给全部释放了,源代码 会出现内存泄漏问题
            for (int i = 0; i < HObjImageList.Count; i++)
            {
                if (HObjImageList[i] != null)
                    ((HObjectEntry)HObjImageList[i]).clear();
            }
            HObjImageList.Clear();



			HObjectEntry entry;

			if (img == null)
				return;
            
            HTuple classValue=null;
            HOperatorSet.GetObjClass(img, out classValue);
            if (!classValue.S.Equals("image"))
            {
                return;
            }
            
            HImage obj = new HImage(img);

			if (obj is HImage)
            {
				double r, c;
				int h, w, area;
				string s;

				area = ((HImage)obj).GetDomain().AreaCenter(out r, out c);
				((HImage)obj).GetImagePointer1(out s, out w, out h);

				if (area == (w * h))
				{
					clearList();

					if ((h != imageHeight) || (w != imageWidth))
					{
						imageHeight = h;
						imageWidth = w;
						setImagePart(0, 0, h, w);
					}
				}//if
			}//if

			entry = new HObjectEntry(obj, mGC.copyContextList());

			HObjImageList.Add(entry);

            //每当传入背景图的时候 都清空HObjectList
            clearHRegionList();

            if (HObjImageList.Count > MAXNUMOBJLIST)
            {
                //需要自己手动释放
                ((HObjectEntry)HObjImageList[1]).clear();
                HObjImageList.RemoveAt(1);
            }
				
		}
        
        /********************************************************************/
        /*                      GRAPHICSSTACK                               */
        /********************************************************************/
        public void addText(string message, int row, int colunm, int size, string color, string coordSystem = "image")
        {
            addIconicVar(new HWndMessage(message, row, colunm, size, color, coordSystem));
        }

        public void addText(string message, int row, int colunm)
        {
            addIconicVar(new HWndMessage(message, row, colunm));
        }

        public void addIconicVar(HWndMessage message)
        {
            HObjectEntry entry;
            if (message == null)
                return;

            entry = new HObjectEntry(message);

            if (HObjImageList.Count < 1)
                return;

            if (entry.Message != null)
            {
                //double zoom = 1.0 / xScale;
                double zoom = entry.Message.coordSystem == "image" ? 1.0 / xScale : 1.0 / originScale;
                double sizeTmp = entry.Message.changeDisplayFontSize(viewPort.hvppleWindow, zoom, currentTextSize);
                currentTextSize = sizeTmp;

                if (isShowMessage)
                    entry.Message.DispMessage(viewPort.hvppleWindow, entry.Message.coordSystem);

                HObjImageList.Add(entry);

                if (HObjImageList.Count > MAXNUMOBJLIST)
                {
                    ((HObjectEntry)HObjImageList[1]).clear();
                    HObjImageList.RemoveAt(1);
                }
            }
        }

        /// <summary>
        /// Clears all entries from the graphics stack 
        /// </summary>
        public void clearList(bool bClearImage = true)
		{
            if (HObjImageList != null && HObjImageList.Count > 0)
            {
                for (int igg = HObjImageList.Count - 1; igg >= 0; igg--)
                {
                    if (HObjImageList[igg] != null)
                    {
                        if (igg == 0 && !bClearImage)
                        {

                        }
                        else
                        {
                            ((HObjectEntry)HObjImageList[igg]).clear();
                            HObjImageList.RemoveAt(igg);
                        }
                    }
                }
            }
		}

		/// <summary>
		/// Returns the number of items on the graphics stack
		/// </summary>
		public int getListCount()
		{
			return HObjImageList.Count;
		}

		/// <summary>
		/// Changes the current graphical context by setting the specified mode
		/// (constant starting by GC_*) to the specified value.
		/// </summary>
		/// <param name="mode">
		/// Constant that is provided by the class GraphicsContext
		/// and describes the mode that has to be changed, 
		/// e.g., GraphicsContext.GC_COLOR
		/// </param>
		/// <param name="val">
		/// Value, provided as a string, 
		/// the mode is to be changed to, e.g., "blue" 
		/// </param>
		public void changeGraphicSettings(string mode, string val)
		{
			switch (mode)
			{
				case GraphicsContext.GC_COLOR:
					mGC.setColorAttribute(val);
					break;
				case GraphicsContext.GC_DRAWMODE:
					mGC.setDrawModeAttribute(val);
					break;
				case GraphicsContext.GC_LUT:
					mGC.setLutAttribute(val);
					break;
				case GraphicsContext.GC_PAINT:
					mGC.setPaintAttribute(val);
					break;
				case GraphicsContext.GC_SHAPE:
					mGC.setShapeAttribute(val);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Changes the current graphical context by setting the specified mode
		/// (constant starting by GC_*) to the specified value.
		/// </summary>
		/// <param name="mode">
		/// Constant that is provided by the class GraphicsContext
		/// and describes the mode that has to be changed, 
		/// e.g., GraphicsContext.GC_LINEWIDTH
		/// </param>
		/// <param name="val">
		/// Value, provided as an integer, the mode is to be changed to, 
		/// e.g., 5 
		/// </param>
		public void changeGraphicSettings(string mode, int val)
		{
			switch (mode)
			{
				case GraphicsContext.GC_COLORED:
					mGC.setColoredAttribute(val);
					break;
				case GraphicsContext.GC_LINEWIDTH:
					mGC.setLineWidthAttribute(val);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Changes the current graphical context by setting the specified mode
		/// (constant starting by GC_*) to the specified value.
		/// </summary>
		/// <param name="mode">
		/// Constant that is provided by the class GraphicsContext
		/// and describes the mode that has to be changed, 
		/// e.g.,  GraphicsContext.GC_LINESTYLE
		/// </param>
		/// <param name="val">
		/// Value, provided as an HTuple instance, the mode is 
		/// to be changed to, e.g., new HTuple(new int[]{2,2})
		/// </param>
		public void changeGraphicSettings(string mode, HTuple val)
		{
			switch (mode)
			{
				case GraphicsContext.GC_LINESTYLE:
					mGC.setLineStyleAttribute(val);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Clears all entries from the graphical context list
		/// </summary>
		public void clearGraphicContext()
		{
			mGC.clear();
		}

		/// <summary>
		/// Returns a clone of the graphical context list (hashtable)
		/// </summary>
		public Hashtable getGraphicContext()
		{
			return mGC.copyContextList();
		}

        /// <summary>
        /// Registers an instance of an ROIController with this window 
        /// controller (and vice versa).
        /// </summary>
        /// <param name="rC"> 
        /// Controller that manages interactive ROIs for the HALCON window 
        /// </param>
        protected internal void setROIController(ROIController rC)
        {
            roiManager = rC;
            rC.setViewController(this);
            this.setViewState(HWndCtrl.MODE_VIEW_NONE);
        }
        /// <summary>
        /// 添加设定显示的图像
        /// </summary>
        /// <param name="image"></param>
        protected internal void addImageShow(HObject image)
        {
            addIconicVar(image);
        }

        public double getOriginScale()
        {
            return originScale;
        }

        protected internal void normalImagePart()
        {
            if (imageShowWidth < viewPort.Width)
            {
                ImgCol1 = -(viewPort.Width * xScale - imageWidth) / 2;
                ImgCol2 = (viewPort.Width * xScale - imageWidth) / 2 + imageWidth;
            }
            else
            {
                double delta = viewPort.Width * SHOW_DELTA * xScale / 2;
                if (ImgCol1 < -delta) 
                {
                    ImgCol2 += -delta - ImgCol1;
                    ImgCol1 = -delta;
                }
                else if (ImgCol2 > imageWidth + delta) 
                {
                    ImgCol1 -= ImgCol2 - (imageWidth + delta);
                    ImgCol2 = imageWidth + delta;
                }
            }
            if (imageShowHeight < viewPort.Height)
            {
                ImgRow1 = -(viewPort.Height * yScale - imageHeight) / 2;
                ImgRow2 = (viewPort.Height * yScale - imageHeight) / 2 + imageHeight;
            }
            else
            {
                double delta = viewPort.Height * SHOW_DELTA * yScale / 2;
                if (ImgRow1 < -delta)
                {
                    ImgRow2 += -delta - ImgRow1;
                    ImgRow1 = -delta;
                }
                else if (ImgRow2 > imageHeight + delta)
                {
                    ImgRow1 -= ImgRow2 - (imageHeight + delta);
                    ImgRow2 = imageHeight + delta;
                }
            }
        }


        #region 再次显示region和 xld

        /// <summary>
        /// hRegionList用来存储存入的HRegion
        /// </summary>
        public List<HRegionEntry> hRegionList = new List<HRegionEntry>();

        /// <summary>
        /// 默认红颜色显示
        /// </summary>
        /// <param name="hObj">传入的region.xld,image</param>
        public void addRegion(HObject hObj)
        {
            addRegion(hObj, "green", "fill", 1);
        }

        public void addRegion(HObject hObj, string color, string drawmode)
        {
            addRegion(hObj, color, drawmode, 1);
        }

        /// <summary>
        /// 重新开辟内存保存 防止被传入的HObject在其他地方dispose后,不能重现
        /// </summary>
        /// <param name="hObj">传入的region.xld,image</param>
        /// <param name="color">颜色</param>
        public void addRegion(HObject hObj, string color, string drawmode, int lineWidth)
        {
            lock (this)
            {
                //显示指定的颜色
                if (color == null)
                    color = "red";

                //显示指定的绘图模式
                if (drawmode == null)
                    drawmode = "margin";

                viewPort.hvppleWindow.SetColor(color);
                viewPort.hvppleWindow.SetDraw(drawmode);
                viewPort.hvppleWindow.SetLineWidth(lineWidth);

                if (hObj != null && hObj.IsInitialized())
                {
                    //
                    HObject temp = new HObject(hObj);
                    //
                    hRegionList.Add(new HRegionEntry(temp, color, drawmode, lineWidth));

                    viewPort.hvppleWindow.DispObj(temp);

                }

                //恢复默认的红色
                //HOperatorSet.SetColor(viewPort.hvppleWindow, "red");
            }
        }

        /// <summary>
        /// 每次传入新的背景Image时,清空hRegionList,避免内存没有被释放
        /// </summary>
        public void clearHRegionList()
        {

            foreach (HRegionEntry hRegionEntry in hRegionList)
            {
                hRegionEntry.clear();
            }

            hRegionList.Clear();
        }

        /// <summary>
        /// 将hRegionList中的HObject,按照先后顺序显示出来
        /// </summary>
        private void showHRegionList()
        {
            if (!isShowHRegion)
                return;

            try
            {
                foreach (HRegionEntry hRegionEntry in hRegionList)
                {
                    viewPort.hvppleWindow.SetColor(hRegionEntry.Color);
                    viewPort.hvppleWindow.SetDraw(hRegionEntry.DrawMode);
                    viewPort.hvppleWindow.SetLineWidth(hRegionEntry.LineWidth);

                    if (hRegionEntry != null && hRegionEntry.HObject.IsInitialized())
                    {
                        viewPort.hvppleWindow.DispObj(hRegionEntry.HObject);

                        //恢复默认的红色
                        //HOperatorSet.SetColor(viewPort.hvppleWindow, "red");
                    }
                }
            }
            catch (Exception e)
            {
                //有时候hobj被dispose了,但是其本身不为null,此时则报错. 已经使用IsInitialized解决了 
            }
        }

        #endregion



    }//end of class
}//end of namespace
