using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using hvppleDotNet;

namespace Wells.Controls.ImageDoc
{
    public partial class ImageDoc : UserControl
    {
        #region 私有变量定义.

        private HWindow /**/                 hv_window;                                       //halcon窗体控件的句柄 this.mCtrl_HWindow.hvppleWindow;
        private HImage  /**/                 hv_image;                                        //缩放时操作的图片  此处千万不要使用hv_image = new HImage(),不然在生成控件dll的时候,会导致无法序列化,去你妈隔壁,还好老子有版本控制,不然都找不到这种恶心问题
        private int /**/                     hv_imageWidth, hv_imageHeight;                   //图片宽,高
        private string /**/                  str_imgSize;                                     //图片尺寸大小 5120X3840
        private bool    /**/                 staticWnd = false;                                //作为普通显示窗口,不允许缩放和鼠标右键菜单
        
        public HWindowControl hWindowControl;   /**/                                           // 当前halcon窗口

        public HWndCtrl _hWndControl;

        public ROIController _roiController;

        #endregion
        

        public ImageDoc()
        {
            InitializeComponent();

            _hWndControl = new HWndCtrl(mCtrl_HWindow);
            _hWndControl.setContextMenuStrip(hv_MenuStrip);
            _roiController = new ROIController();
            _hWndControl.setROIController(_roiController);
            _hWndControl.setViewState(HWndCtrl.MODE_VIEW_NONE);
            _hWndControl.showZoomPercent = new HWndCtrl.ShowZoomPercent(showZoomPercent);
            hWindowControl = this.mCtrl_HWindow;
            try { hv_window = this.mCtrl_HWindow.hvppleWindow; } catch (Exception) { }//这里添加控件库时会报异常，运行不会，不知道为什么

            //设定鼠标按下时图标的形状
            //'arrow'箭头
            //'default'
            //'crosshair'十字
            //'text I-beam'输入光标
            //'Slashed circle'禁止
            //'Size All'拖动光标，十字方向
            //'Size NESW'拖动光标，左下右上
            //'Size S'上下拖动
            //'Size NWSE'左上右下
            //'Size WE'水平方向
            //'Vertical Arrow'竖直向中箭头
            //'Hourglass'等待
            //设定鼠标按下时图标的形状

            //hv_window.SetMshape("hand");

            if (hv_window != null)
                hv_window.SetWindowParam("background_color", "dim gray");

            m_CtrlHStatusLabelCtrl.Visible = false;
        }

        /// <summary>
        /// 绘制模式下,不允许缩放和鼠标右键菜单
        /// </summary>
        public bool StaticWnd
        {
            get { return staticWnd; }
            set
            {
                //缩放控制
                setStaticWnd(value);
                staticWnd = value;
            }
        }
        private bool editMode = true;//绘制的图形是否可以编辑
        public bool EditMode
        {
            get
            {
                return editMode;
            }
            set
            {
                setEditModel(value);
                editMode = value;
            }
        }
        
        /// <summary>
        /// 设置image,初始化控件参数
        /// </summary>
        public HImage Image
        {
            get
            {
                return this.hv_image;
            }
            set
            {
                lock (this)
                {
                    if (value != null)
                    {
                        if (this.hv_image != null)
                        {
                            this.hv_image.Dispose();
                            this.hv_image = null;
                        }

                        this.hv_image = value;
                        hv_image.GetImageSize(out hv_imageWidth, out hv_imageHeight);
                        str_imgSize = String.Format("{0}X{1}", hv_imageWidth, hv_imageHeight);

                        setImage(hv_image);
                    }
                }
            }
        }

        private ImageMode imageMode = ImageMode.Origin;

        /// <summary>
        /// 图像显示模式
        /// </summary>
        public ImageMode ImageMode
        {
            get
            {
                return this.imageMode;
            }
            set
            {
                imageMode = value;
                _hWndControl.ImageMode = value;
                dispImageFit();
            }
        }

        /// <summary>
        /// 获得halcon窗体控件的句柄
        /// </summary>
        public IntPtr HWindowHandle
        {
            get { return this.mCtrl_HWindow.hvppleID; }
        }

        public HWindow HWindow
        {
            get { return this.mCtrl_HWindow.hvppleWindow; }
        }

        public HWindowControl getHWindowControl()
        {
            return this.mCtrl_HWindow;
        }

        public void useContextMenuStrip(bool value)
        {
            mCtrl_HWindow.ContextMenuStrip = value ? hv_MenuStrip : null;
        }

        /// <summary>
        /// 状态条 显示/隐藏
        /// </summary>
        public void showStatusBar(bool bShow)
        {
            this.SuspendLayout();

            if (bShow)
            {
                m_CtrlHStatusLabelCtrl.Visible = true;
                mCtrl_HWindow.HMouseMove += HWindowControl_HMouseMove;
            }
            else
            {
                m_CtrlHStatusLabelCtrl.Visible = false;
                mCtrl_HWindow.HMouseMove -= HWindowControl_HMouseMove;
            }

            tsbtn_showStatusBar.Checked = bShow;
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// 保存窗体截图到本地
        /// </summary>
        private void saveWindowDump()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG Image|*.png|All Files|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (String.IsNullOrEmpty(sfd.FileName))
                    return;

                try
                {
                    //截取窗口图
                    HOperatorSet.DumpWindow(HWindowHandle, "png best", sfd.FileName);
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// 保存原始图片到本地
        /// </summary>
        private void saveImage()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "BMP Image|*.bmp|All Files|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (String.IsNullOrEmpty(sfd.FileName))
                {
                    return;
                }
                lock (this)
                {
                    try
                    {
                        HOperatorSet.WriteImage(hv_image, "bmp", 0, sfd.FileName);
                    }
                    catch (Exception) { }
                }
            }
        }

        /// <summary>
        /// 图片适应大小显示在窗体
        /// </summary>
        /// <param name="hw_Ctrl">halcon窗体控件</param>
        public void dispImageFit()
        {

            try
            {
                resetWindowImage();
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 鼠标在空间窗体里滑动,显示鼠标所在位置的灰度值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HWindowControl_HMouseMove(object sender, HMouseEventArgs e)
        {

            if (hv_image != null)
            {
                try
                {
                    int button_state;
                    double positionX, positionY;
                    string str_value = "";
                    string str_position = "";
                    bool _isXOut = true, _isYOut = true;
                    HTuple channel_count;

                    HOperatorSet.CountChannels(hv_image, out channel_count);

                    hv_window.GetMpositionSubPix(out positionY, out positionX, out button_state);
                    str_position = String.Format("X: {0:0000.0}, Y: {1:0000.0}", positionX, positionY);

                    _isXOut = (positionX < 0 || positionX >= hv_imageWidth);
                    _isYOut = (positionY < 0 || positionY >= hv_imageHeight);

                    if (!_isXOut && !_isYOut)
                    {
                        if ((int)channel_count == 1)
                        {
                            double grayVal;
                            grayVal = hv_image.GetGrayval((int)positionY, (int)positionX);
                            str_value = String.Format("Gray: {0:000.0}", grayVal);
                        }
                        else if ((int)channel_count == 3)
                        {
                            double grayValRed, grayValGreen, grayValBlue;

                            HImage _RedChannel, _GreenChannel, _BlueChannel;

                            _RedChannel = hv_image.AccessChannel(1);
                            _GreenChannel = hv_image.AccessChannel(2);
                            _BlueChannel = hv_image.AccessChannel(3);

                            grayValRed = _RedChannel.GetGrayval((int)positionY, (int)positionX);
                            grayValGreen = _GreenChannel.GetGrayval((int)positionY, (int)positionX);
                            grayValBlue = _BlueChannel.GetGrayval((int)positionY, (int)positionX);

                            _RedChannel.Dispose();
                            _GreenChannel.Dispose();
                            _BlueChannel.Dispose();

                            str_value = String.Format("R:{0:000.0}, G:{1:000.0},B: {2:000.0}", grayValRed, grayValGreen, grayValBlue);
                        }
                        else
                        {
                            str_value = "";
                        }
                    }
                    m_CtrlHStatusLabelCtrl.Text = str_imgSize + "    " + str_position + "    " + str_value;
                }
                catch (Exception ex)
                {
                    //不处理
                }
            }

        }
        public void resetWindow()
        {
            try
            {
                this.Invoke(new Action(
                        () =>
                        {
                            m_CtrlHStatusLabelCtrl.Visible = false;
                            mCtrl_HWindow.hvppleWindow.ClearWindow();
                            clearWindow();
                        }
                    ));
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }


        }


        /// <summary>
        /// Hobject转换为的临时Himage,显示背景图
        /// </summary>
        /// <param name="hobject">传递Hobject,必须为图像</param>
        public void HobjectToHimage(HObject hobject)
        {
            if (hobject == null || !hobject.IsInitialized())
            {
                resetWindow();
                return;
            }

            this.Image = new HImage(hobject);

        }
        

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mCtrl_HWindow_MouseLeave(object sender, EventArgs e)
        {
            //避免鼠标离开窗口,再返回的时候,图表随着鼠标移动
            mouseleave();
        }

        private void mCtrl_HWindow_DoubleClick(object sender, EventArgs e)
        {

        }

        private void mCtrl_HWindow_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void mCtrl_HWindow_Click(object sender, EventArgs e)
        {

        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////                                                                                                  ///////////////////
        //////////////////           把viewwindow的内容移到这里............................                 ///////////////////
        //////////////////                                                                                                  ////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //清空所有显示内容
        public void clearWindow(bool bClearImage = true)
        {
            //清空显示image
            _hWndControl.clearList(bClearImage);
            //清空hobjectList
            _hWndControl.clearHRegionList();

            _hWndControl.roiManager.reset();

            _hWndControl.repaint();
        }

        public void clearHRegion()
        {
            //清空hobjectList
            _hWndControl.clearHRegionList();
            _hWndControl.repaint();
        }

        public void clearROI()
        {
            _hWndControl.roiManager.reset();

            _hWndControl.repaint();
        }

        public void displayImage(HObject img)
        {
            Image = new HImage(img);
        }
        
        protected void setImage(HObject img)
        {
            //添加背景图片
            this._hWndControl.addImageShow(img);
            //清空roi容器,不让roi显示
            this._roiController.reset();
            //显示图片
            this._roiController.resetWindowImage();
        }

        public void setSelected(bool value)
        {
            //_hWndControl.Selected = value;
            this.BackColor = value ? Color.Lime : Color.Transparent;
        }

        public void displayMessage(string message, int row, int colunm)
        {
            this._hWndControl.addText(message, row, colunm);
        }

        public void displayMessage(string message, int row, int colunm, int size, string color)
        {
            this._hWndControl.addText(message, row, colunm, size, color);
        }

        public void displayMessage(string message, int row, int colunm, int size, string color, string coordSystem)
        {
            this._hWndControl.addText(message, row, colunm, size, color, coordSystem);
        }

        public void notDisplayRoi()
        {
            this._roiController.reset();
            //显示图片
            this._roiController.resetWindowImage();
        }

        //获取当前窗口显示的roi数量
        public int getRoiCount()
        {
            return _roiController.ROIList.Count;
        }

        public double getScale()
        {
            double tmp = _hWndControl.getOriginScale();
            return tmp == 0 ? 1 : 1 / tmp;
        }

        //是否开启缩放事件
        public void setStaticWnd(bool flag)
        {
            _hWndControl.isStaticWnd = flag;
            useContextMenuStrip(!flag);
        }
        //是否开启编辑事件
        public void setEditModel(bool flag)
        {
            _roiController.EditModel = flag;
        }
        public void resetWindowImage()
        {
            this._hWndControl.resetWindow();
            this._roiController.resetWindowImage();
        }

        public void mouseleave()
        {
            _hWndControl.raiseMouseLeave();
        }

        public void zoomWindowImage()
        {
            this._roiController.zoomWindowImage();
        }

        public void zoomImage(bool bZoomIn)
        {
            this._hWndControl.zoomImage(bZoomIn);
        }

        public void moveWindowImage()
        {
            this._roiController.moveWindowImage();
        }

        public void moveImageToPoint(double x, double y)
        {
            this._hWndControl.moveImageToPoint(x, y);
        }

        public void noneWindowImage()
        {
            this._roiController.noneWindowImage();
        }

        public void genRect1(double row1, double col1, double row2, double col2, ref List<ROI> list)
        {
            this._roiController.genRect1(row1, col1, row2, col2, ref list);
        }
        public void genNurbs(HTuple rows, HTuple cols, ref List<ROI> list)
        {
            this._roiController.genNurbs(rows, cols, ref list);
        }
        public void genInitRect1(ref List<ROI> list)
        {
            this._roiController.genInitRect1(_roiController.viewController.imageHeight, ref list);
        }

        public void genRect2(double row, double col, double phi, double length1, double length2, ref List<ROI> list)
        {
            this._roiController.genRect2(row, col, phi, length1, length2, ref list);
        }
        public void genInitRect2(ref List<ROI> list)
        {
            this._roiController.genInitRect2(_roiController.viewController.imageHeight, ref list);
        }
        public void genCircle(double row, double col, double radius, ref List<ROI> list)
        {
            this._roiController.genCircle(row, col, radius, ref list);
        }
        public void genCircularArc(double row, double col, double radius, double startPhi, double extentPhi, string direct, ref List<ROI> list)
        {
            this._roiController.genCircularArc(row, col, radius, startPhi, extentPhi, direct, ref list);
        }
        public void genLine(double beginRow, double beginCol, double endRow, double endCol, ref List<ROI> list)
        {
            this._roiController.genLine(beginRow, beginCol, endRow, endCol, ref list);
        }

        public List<double> smallestActiveROI(out string name, out int index)
        {
            List<double> resual = this._roiController.smallestActiveROI(out name, out index);
            return resual;
        }

        public ROI smallestActiveROI(out List<double> data, out int index)
        {
            ROI roi = this._roiController.smallestActiveROI(out data, out index);
            return roi;
        }

        public void selectROI(int index)
        {
            this._roiController.selectROI(index);
        }

        public void selectROI(List<ROI> rois, int index)
        {
            //this._roiController.selectROI(index);
            if ((rois.Count > index) && (index >= 0))
            {
                this._hWndControl.resetAll();
                this._hWndControl.repaint();

                HTuple m_roiData = null;
                m_roiData = rois[index].getModelData();

                switch (rois[index].Type)
                {
                    case "ROIRectangle1":

                        if (m_roiData != null)
                        {
                            this._roiController.displayRect1(rois[index].Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D);
                        }
                        break;
                    case "ROIRectangle2":

                        if (m_roiData != null)
                        {
                            this._roiController.displayRect2(rois[index].Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D, m_roiData[4].D);
                        }
                        break;
                    case "ROICircle":

                        if (m_roiData != null)
                        {
                            this._roiController.displayCircle(rois[index].Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D);
                        }
                        break;
                    case "ROICircularArc":

                        if (m_roiData != null)
                        {
                            this._roiController.displayCircularArc(rois[index].Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D, m_roiData[4].D);
                        }
                        break;
                    case "ROILine":

                        if (m_roiData != null)
                        {
                            this._roiController.displayLine(rois[index].Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void displayROI(List<ROI> rois)
        {
            if (rois == null)
            {
                return;
            }
            //this._hWndControl.resetAll();
            //this._hWndControl.repaint();

            foreach (var roi in rois)
            {


                switch (roi.Type)
                {
                    case "ROIRectangle1":
                        HTuple m_roiData = null;
                        m_roiData = roi.getModelData();
                        if (m_roiData != null)
                        {
                            this._roiController.displayRect1(roi.Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D);
                        }
                        break;
                    case "ROIRectangle2":
                        m_roiData = null;
                        m_roiData = roi.getModelData();
                        if (m_roiData != null)
                        {
                            this._roiController.displayRect2(roi.Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D, m_roiData[4].D);

                        }
                        break;
                    case "ROICircle":
                        m_roiData = roi.getModelData();
                        if (m_roiData != null)
                        {
                            this._roiController.displayCircle(roi.Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D);
                        }
                        break;
                    case "ROILine":
                        m_roiData = roi.getModelData();
                        if (m_roiData != null)
                        {
                            this._roiController.displayLine(roi.Color, m_roiData[0].D, m_roiData[1].D, m_roiData[2].D, m_roiData[3].D);
                        }
                        break;
                    case "ROINurbs":
                        HTuple rows, cols;
                        roi.getModelData(out rows, out cols);
                        if (rows != null)
                        {
                            this._roiController.displayNurbs(roi.Color, rows, cols);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void removeActiveROI(ref List<ROI> rois)
        {
            this._roiController.removeActiveROI(ref rois);
        }

        public void setActiveRoi(int index)
        {
            this._roiController.activeROIidx = index;
        }
        public void saveROI(List<ROI> rois, string fileNmae)
        {
            List<RoiData> m_RoiData = new List<RoiData>();
            for (int i = 0; i < rois.Count; i++)
            {
                m_RoiData.Add(new RoiData(i, rois[i]));
            }

            Wells.Tools.clsSerialize.Save(m_RoiData, fileNmae);
        }

        public void loadROI(string fileName, out List<ROI> rois)
        {
            this._hWndControl.resetAll();

            rois = new List<ROI>();
            List<RoiData> m_RoiData = new List<RoiData>();
            m_RoiData = (List<RoiData>)Wells.Tools.clsSerialize.Load(m_RoiData.GetType(), fileName);
            for (int i = 0; i < m_RoiData.Count; i++)
            {
                switch (m_RoiData[i].Name)
                {
                    case "Rectangle1":
                        this._roiController.genRect1(m_RoiData[i].Rectangle1.Row1, m_RoiData[i].Rectangle1.Column1,
                            m_RoiData[i].Rectangle1.Row2, m_RoiData[i].Rectangle1.Column2, ref rois);
                        rois.Last().Color = m_RoiData[i].Rectangle1.Color;
                        break;
                    case "Rectangle2":
                        this._roiController.genRect2(m_RoiData[i].Rectangle2.Row, m_RoiData[i].Rectangle2.Column,
                            m_RoiData[i].Rectangle2.Phi, m_RoiData[i].Rectangle2.Lenth1, m_RoiData[i].Rectangle2.Lenth2, ref rois);
                        rois.Last().Color = m_RoiData[i].Rectangle2.Color;
                        break;
                    case "Circle":
                        this._roiController.genCircle(m_RoiData[i].Circle.Row, m_RoiData[i].Circle.Column, m_RoiData[i].Circle.Radius, ref rois);
                        rois.Last().Color = m_RoiData[i].Circle.Color;
                        break;
                    case "Line":
                        this._roiController.genLine(m_RoiData[i].Line.RowBegin, m_RoiData[i].Line.ColumnBegin,
                            m_RoiData[i].Line.RowEnd, m_RoiData[i].Line.ColumnEnd, ref rois);
                        rois.Last().Color = m_RoiData[i].Line.Color;
                        break;
                    default:
                        break;
                }
            }


            this._hWndControl.repaint();
        }

        #region 专门用于 显示region 和xld的方法
        public void displayHRegion(HObject obj, string color, string drawmode)
        {
            _hWndControl.addRegion(obj, color, drawmode);

        }

        public void displayHRegion(HObject obj, string color, string drawmode, int lineWidth)
        {
            _hWndControl.addRegion(obj, color, drawmode, lineWidth);

        }

        public void displayHRegion(HObject obj)
        {
            _hWndControl.addRegion(obj);
        }

        #endregion

        private void showZoomPercent(double per)
        {
            int percent = (int)Math.Round(per * 100);
            tsbtn_ZoomPercent.Text = clsWellsLanguage.getString(131) + "<" + percent.ToString() + "%>";
        }

        private void tsbtn_fitWindow_Click(object sender, EventArgs e)
        {
            dispImageFit();
        }

        private void tsbtn_saveOriginImage_Click(object sender, EventArgs e)
        {
            saveImage();
        }

        private void tsbtn_saveDumpWindow_Click(object sender, EventArgs e)
        {
            saveWindowDump();
        }

        private void tsbtn_showMessage_Click(object sender, EventArgs e)
        {
            tsbtn_showInfo.Checked = _hWndControl.showMessage();
        }

        private void tsbtn_showROI_Click(object sender, EventArgs e)
        {
            tsbtn_showROI.Checked = _hWndControl.showROI();
        }

        private void tsbtn_showRegion_Click(object sender, EventArgs e)
        {
            tsbtn_showRegion.Checked = _hWndControl.showHRegion();
        }

        private void tsbtn_showHisto_Click(object sender, EventArgs e)
        {
            _hWndControl.funcShowGrayHisto();
        }

        private void tsbtn_showHat_Click(object sender, EventArgs e)
        {
            tsbtn_showHat.Checked = _hWndControl.showCross();
        }

        private void tsbtn_showStatusBar_Click(object sender, EventArgs e)
        {
            showStatusBar(tsbtn_showStatusBar.Checked);
        }

        private void tsbtn_MeasureDistance_Click(object sender, EventArgs e)
        {
            _hWndControl.funcMeasureLineDistance();
        }

        private void tsbtn_OriginSize_Click(object sender, EventArgs e)
        {
            _hWndControl.zoomImageByPercent(1);
        }

        private void tsbtn_ZoomPercent_Click(object sender, EventArgs e)
        {
            string str = Wells.class_Public.getInputInfo(clsWellsLanguage.getString(130), clsWellsLanguage.getString(131), "", false);
            double per;
            double.TryParse(str, out per);
            if (per > 0)
                _hWndControl.zoomImageByPercent(per/100.0);
        }

        private int bacWidth = 0;
        private int bacHeight = 0;

        private void mCtrl_HWindow_SizeChanged(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;

            if (mCtrl_HWindow.Width == 0 || mCtrl_HWindow.Height == 0)
                return;

            if (mCtrl_HWindow.Width == bacWidth && mCtrl_HWindow.Height == bacHeight)
                return;

            bacWidth = mCtrl_HWindow.Width;
            bacHeight = mCtrl_HWindow.Height;
            dispImageFit();
        }

        public void reFresh()
        {
            _hWndControl.repaint();
        }

        public void updateImage(List<object> list)
        {
            //清空显示image
            _hWndControl.clearList(false);
            //清空hobjectList
            _hWndControl.clearHRegionList();
            //清空roi
            _hWndControl.roiManager.reset();
            
            for (int igg = 0; igg < list.Count / 2; igg++)
            {
                string strDrawType = list[igg * 2] as string;
                if(strDrawType == "message")
                {
                    HWndMessage msg = list[igg * 2 + 1] as HWndMessage;
                    if (msg != null)
                        _hWndControl.HObjImageList.Add(new HObjectEntry(msg));
                }
                if(strDrawType == "region")
                {
                    HRegionEntry entry = list[igg * 2 + 1] as HRegionEntry;
                    if (entry != null)
                        _hWndControl.hRegionList.Add(entry);
                }
                if(strDrawType == "roi")
                {
                    ROI roi = list[igg * 2 + 1] as ROI;
                    _hWndControl.roiManager.ROIList.Add(roi);
                }
            }

            _hWndControl.repaint();
        }
    }
}
