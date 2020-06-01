using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using hvppleDotNet;

namespace Wells.Controls.ImageDocEx
{
    public partial class ImageDocEx : UserControl
    {
        #region ***** 程序字段定义 *****

        /// <summary>
        /// 窗体控件的句柄
        /// </summary>
        public HWindow HWindow { get; }

        /// <summary>
        /// 缩放时操作的图片  此处千万不要使用hv_image = new HImage(),不然在生成控件dll的时候,会导致无法序列化
        /// </summary>
        private HImage Image = null;

        /// <summary>
        /// 图片宽
        /// </summary>
        public int ImageWidth { get; set; }

        /// <summary>
        /// 图片高
        /// </summary>
        public int ImageHeight { get; set; }

        /// <summary>
        /// 作为普通显示窗口,不允许缩放和鼠标右键菜单
        /// </summary>
        public bool StaticWnd { get; set; }
        
        /// <summary>
        /// 图片显示模式，Origin和Strech
        /// </summary>
        public ImageMode ImageMode { get; set; }

        /// <summary>
        /// 图片显示边界
        /// </summary>
        public double ImgRow1, ImgCol1, ImgRow2, ImgCol2;

        /// <summary>
        /// 控件一个长度代表多少像素
        /// </summary>
        private double xScale, yScale, originScale;

        /// <summary>
        /// 图片显示长宽
        /// </summary>
        public int imageShowWidth, imageShowHeight;

        /// <summary>
        /// 图片外额外显示比例
        /// </summary>
        private double SHOW_DELTA = 0.4;

        /// <summary>
        /// 左键按下状态
        /// </summary>
        private bool mouseLeftPressed = false;

        /// <summary>
        /// 右键按下状态
        /// </summary>
        private bool mouseRightPressed = false;

        /// <summary>
        /// 鼠标移动ROI状态
        /// </summary>
        private bool mouseMovingObject = false;

        /// <summary>
        /// 鼠标更新ROI大小状态
        /// </summary>
        private bool mouseResizingObject = false;

        /// <summary>
        /// 记录鼠标当前坐标
        /// </summary>
        private double startX, startY;

        /// <summary>
        /// 存储ROI列表
        /// </summary>
        public List<ROI> m_roiList = new List<ROI>();

        /// <summary>
        /// 即将要进行更新的ROI列表
        /// </summary>
        public List<ROI> m_l2updateList = new List<ROI>();
        
        /// <summary>
        /// 拖动轨迹
        /// </summary>
        public Tracker m_tracker = new Tracker();

        public List<HRegionEntry> m_regionList = new List<HRegionEntry>();

        private bool enteredSetup = false;

        /// <summary>
        /// 指示当前选中roi的序号，-2表示没有，-1表示多个，其他表示单一选择项的序号
        /// </summary>
        private int iMoveROINum = -2;

        public double handleWidth = 0;

        public delegate void HMouseDown(int index);
        public HMouseDown qtHMouseDown;

        public delegate void HMouseUp(int index);
        public HMouseUp qtHMouseUp;

        public delegate void HMouseMove(string strInfo);
        public HMouseMove qtHMouseMove;

        #endregion

        public ImageDocEx()
        {
            #region ***** 构造函数 *****
            
            InitializeComponent();

            x = this.Width;
            y = this.Height;
            
            try { HWindow = this.m_Ctrl_HWindow.hvppleWindow; } catch (Exception) { }//这里添加控件库时会报异常，运行不会，不知道为什么
            ImageWidth = 2448;
            ImageHeight = 2048;
            StaticWnd = false;
            //HObject obj;
            //HOperatorSet.GenImageConst(out obj, "byte", ImageWidth, ImageHeight);
            //Image = new HImage(obj);
            //obj.Dispose();

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

            if (HWindow != null)
                HWindow.SetWindowParam("background_color", "dim gray");

            m_Ctrl_HWindow.HMouseMove += m_Ctrl_HWindow_HMouseMove_2;

            #endregion
        }

        #region ***** 内置图片操作，设置、获取 *****

        public void setImage(HImage img)
        {
            #region ***** 设置显示图片 *****

            if (img == null)
                return;

            lock (this)
            {
                if (Image != null)
                {
                    Image.Dispose();
                    Image = null;
                }

                Image = img.Clone();

                setImagePart();
            }

            Invalidate();

            #endregion
        }

        public void setImage(HObject img)
        {
            #region ***** 设置显示图片 *****

            if (img == null)
                return;

            lock (this)
            {
                if (Image != null)
                {
                    Image.Dispose();
                    Image = null;
                }

                Image = new HImage(img);

                setImagePart();
            }

            Invalidate();

            #endregion
        }

        public HImage GetImage()
        {
            #region ***** 获取显示图片 *****

            return Image;

            #endregion
        }

        #endregion

        #region ***** 鼠标响应事件 *****

        /// 左键功能：1.单击空白区域，取消所有选中状态；2.单击元件，如果没有选中（可能其他选件被选中，一个或者多个），则变成选中状态，如果已经被选中（唯一），实行拖动或者尺寸改变功能，
        /// 如果已经被选中（且不唯一），可以实行拖动功能
        /// 
        /// 
        private void m_Ctrl_HWindow_HMouseDown(object sender, HMouseEventArgs e)
        {
            #region ***** 鼠标按下事件 *****

            //关闭缩放事件
            if (StaticWnd)
                return;

            if (e.Button == MouseButtons.Right)
                mouseRightPressed = true;
            if (e.Button == MouseButtons.Left)
                mouseLeftPressed = true;

            if (e.Button == MouseButtons.Left)
            {
                if (!enteredSetup) 
                {
                    bool bHasChooseOne = false;
                    int iChooseIndex = -1;

                    for (int igg = 0; igg < m_roiList.Count; igg++)
                    {
                        if (0 <= m_roiList[igg].ptLocation(e.X, e.Y, ImageWidth, ImageHeight))
                        {
                            bHasChooseOne = true;
                            iChooseIndex = igg;
                            break;
                        }
                    }

                    if (bHasChooseOne)//选中一个，判断下是本身有多个选中还是一个，也可能是0个
                    {
                        if (m_l2updateList.Contains(m_roiList[iChooseIndex]))//判断是否在选中列表中
                        {
                            if (m_l2updateList.Count == 1)
                                iMoveROINum = iChooseIndex;
                            else
                                iMoveROINum = -1;
                        }
                        else//不在列表中，直接选中当前项
                        {
                            for (int igg = 0; igg < m_l2updateList.Count; igg++)
                                m_l2updateList[igg].Selected = false;
                            m_l2updateList.Clear();
                            m_roiList[iChooseIndex].Selected = true;
                            m_l2updateList.Add(m_roiList[iChooseIndex]);

                            iMoveROINum = iChooseIndex;
                        }
                    }
                    else//没有选中任何roi，清除所有选中roi
                    {
                        for (int igg = 0; igg < m_l2updateList.Count; igg++)
                            m_l2updateList[igg].Selected = false;
                        m_l2updateList.Clear();

                        iMoveROINum = -2;
                    }
                }
            }
            
            qtHMouseDown?.Invoke(iMoveROINum);//用户自定义委托
            
            startX = e.X;
            startY = e.Y;
            m_tracker.Row1 = e.Y;
            m_tracker.Col1 = e.X;

            #endregion
        }

        private void m_Ctrl_HWindow_HMouseUp(object sender, HMouseEventArgs e)
        {
            #region ***** 鼠标松开事件 *****

            if (StaticWnd)
                return;

            if (e.Button == MouseButtons.Right)
                mouseRightPressed = false;
            if (e.Button == MouseButtons.Left)
                mouseLeftPressed = false;

            mouseMovingObject = false;
            mouseResizingObject = false;
            
            m_tracker.Actived = false;

            if(e.Button == MouseButtons.Left)
            {
                if (m_l2updateList.Count == 0)
                {
                    iMoveROINum = -2;
                }
                else if (m_l2updateList.Count == 1)
                {
                    iMoveROINum = m_roiList.IndexOf(m_l2updateList[0]);
                }
                else
                {
                    iMoveROINum = -1;
                }
            }

            qtHMouseUp?.Invoke(iMoveROINum);//用户自定义回调委托

            HWindow.SetMshape("default");

            Invalidate();

            #endregion
        }

        private void m_Ctrl_HWindow_HMouseMove(object sender, HMouseEventArgs e)
        {
            #region ***** 鼠标移动事件 *****

            if (StaticWnd)
                return;

            double motionX = 0, motionY = 0;
            double posX, posY;
            double zoomZone;
            int ptLocation = -2;
            double distance = 0;

            
            
            if (mouseRightPressed)
            {
                HWindow.SetMshape("crosshair");
                if (imageShowWidth > m_Ctrl_HWindow.Width || imageShowHeight > m_Ctrl_HWindow.Height)
                {
                    motionX = e.X - startX;
                    motionY = e.Y - startY;
                    if (((int)motionX != 0) || ((int)motionY != 0))
                    {
                        moveImage(motionX, motionY);
                        startX = e.X - motionX;//移动图片时，图片坐标系在变换，记录坐标需要加上偏移
                        startY = e.Y - motionY;
                    }
                }
            }
            else
            {
                if (iMoveROINum == -1)//多选
                {
                    bool bInROI = false;
                    for (int igg = 0; igg < m_l2updateList.Count; igg++)
                    {
                        ptLocation = m_l2updateList[igg].ptLocation(e.X, e.Y, ImageWidth, ImageHeight);
                        if (0 <= ptLocation)
                        {
                            bInROI = true;
                            break;
                        }
                    }
                    if (bInROI)
                        HWindow.SetMshape("Size All");
                    else
                        HWindow.SetMshape("default");
                }
                else if (iMoveROINum >= 0)//单选
                {
                    if (!mouseMovingObject && !mouseResizingObject)
                    {
                        ptLocation = m_roiList[iMoveROINum].ptLocation(e.X, e.Y, ImageWidth, ImageHeight);
                        //distance = m_roiList[iMoveROINum].distToClosestHandle(e.X, e.Y);

                        if (0 == ptLocation)//inside
                        {
                            HWindow.SetMshape("Size All");
                        }
                        else if (2 == ptLocation)//onside
                        {
                            string name = m_roiList[iMoveROINum].GetType().Name;

                            if (name == "ROIRectangle1")
                            {
                                switch (m_roiList[iMoveROINum].activeHandleIdx)
                                {
                                    case 0://中心
                                        HWindow.SetMshape("Size All");
                                        break;
                                    case 1://左上
                                        HWindow.SetMshape("Size NWSE");
                                        break;
                                    case 2://右上
                                        HWindow.SetMshape("Size NESW");
                                        break;
                                    case 3://右下
                                        HWindow.SetMshape("Size NWSE");
                                        break;
                                    case 4://左下
                                        HWindow.SetMshape("Size NESW");
                                        break;
                                    case 5://上中
                                        HWindow.SetMshape("Size S");
                                        break;
                                    case 6://右中
                                        HWindow.SetMshape("Size WE");
                                        break;
                                    case 7://下中
                                        HWindow.SetMshape("Size S");
                                        break;
                                    case 8://左中
                                        HWindow.SetMshape("Size WE");
                                        break;
                                    default://
                                        HWindow.SetMshape("default");
                                        break;
                                }
                            }
                            else if (name == "ROICircle")
                            {
                                switch (m_roiList[iMoveROINum].activeHandleIdx)
                                {
                                    case 0://中心
                                        HWindow.SetMshape("Size All");
                                        break;
                                    case 1:
                                        {
                                            double x = ((ROICircle)m_roiList[iMoveROINum]).Column;
                                            double y = ((ROICircle)m_roiList[iMoveROINum]).Row;
                                            double dx = Math.Abs(e.X - x);
                                            double dy = Math.Abs(e.Y - y);
                                            double delta = 2 * m_roiList[iMoveROINum].getHandleWidth(ImageWidth, ImageHeight);
                                            if (dx < delta)//上中//下中
                                                HWindow.SetMshape("Size S");
                                            else if (dy < delta)//右中//左中
                                                HWindow.SetMshape("Size WE");
                                            else if ((e.X < x && e.Y < y) || (e.X > x && e.Y > y)) //左上//右下
                                                HWindow.SetMshape("Size NWSE");
                                            else if ((e.X < x && e.Y > y) || (e.X > x && e.Y < y))//左下//右上
                                                HWindow.SetMshape("Size NESW");
                                            else
                                                HWindow.SetMshape("default");
                                        }
                                        break;
                                    default://
                                        HWindow.SetMshape("default");
                                        break;
                                }
                            }
                        }
                        else
                        {
                            HWindow.SetMshape("default");
                        }
                    }
                }
                else
                {
                    HWindow.SetMshape("default");
                }
            }

            if(mouseLeftPressed)
            {
                if (iMoveROINum == -2)//没有选中，显示轨迹
                {
                    m_tracker.Actived = true;
                    m_tracker.Row2 = e.Y;
                    m_tracker.Col2 = e.X;
                    RectangleF rect = m_tracker.getRect();
                    for (int igg = 0; igg < m_roiList.Count; igg++)
                    {
                        if (m_roiList[igg].isInRect(rect))
                        {
                            m_roiList[igg].Selected = true;
                            if (!m_l2updateList.Contains(m_roiList[igg]))
                                m_l2updateList.Add(m_roiList[igg]);
                        }
                        else
                        {
                            m_roiList[igg].Selected = false;
                            if (m_l2updateList.Contains(m_roiList[igg]))
                                m_l2updateList.Remove(m_roiList[igg]);
                        }
                    }
                }
                else if (iMoveROINum == -1) //多个一起被选中，一起执行move动作，无法执行resize动作
                {
                    mouseMovingObject = true;
                    motionX = e.X - startX;
                    motionY = e.Y - startY;
                    moveROI(motionX, motionY);
                    startX = e.X;//移动roi时，图片坐标系没有变更，只是roi坐标偏移，记录坐标记录当前鼠标坐标即可
                    startY = e.Y;
                }
                else if(iMoveROINum >= 0)//单一被选中，执行move或者resize动作
                {
                    if (!mouseMovingObject && !mouseResizingObject)
                        ptLocation = m_roiList[iMoveROINum].ptLocation(e.X, e.Y, ImageWidth, ImageHeight);
                    else
                    {
                        if (mouseMovingObject) ptLocation = 0;
                        if (mouseResizingObject) ptLocation = 2;
                    }

                    if (0 == ptLocation)
                    {
                        mouseMovingObject = true;
                        motionX = e.X - startX;
                        motionY = e.Y - startY;
                        moveROI(motionX, motionY);
                        startX = e.X;//移动roi时，图片坐标系没有变更，只是roi坐标偏移，记录坐标记录当前鼠标坐标即可
                        startY = e.Y;
                    }
                    else if (2 == ptLocation)
                    {
                        mouseResizingObject = true;
                        m_roiList[iMoveROINum].resize(e.X, e.Y);
                    }
                }
            }

            Invalidate();

            #endregion
        }
        
        private void m_Ctrl_HWindow_HMouseWheel(object sender, HMouseEventArgs e)
        {
            #region ***** 鼠标滚轮事件 *****

            //关闭缩放事件
            if (StaticWnd)
            {
                return;
            }

            double scale;

            if (e.Delta > 0)
                scale = 0.9;
            else
                scale = 1 / 0.9;

            zoomImage(e.X, e.Y, scale);

            Invalidate();

            #endregion
        }

        private Point pt = Point.Empty;

        private void m_Ctrl_HWindow_HMouseMove_2(object sender, HMouseEventArgs e)
        {
            #region ***** 鼠标移动事件2 *****

            if (Image != null)
            {
                try
                {
                    int button_state;
                    double positionX, positionY;
                    string str_value = "";
                    string str_position = "";
                    bool _isXOut = true, _isYOut = true;
                    HTuple channel_count;

                    HOperatorSet.CountChannels(Image, out channel_count);

                    HWindow.GetMpositionSubPix(out positionY, out positionX, out button_state);
                    
                    str_position = String.Format("{0}#{1}", (int)positionX, (int)positionY);

                    _isXOut = (positionX < 0 || positionX >= ImageWidth);
                    _isYOut = (positionY < 0 || positionY >= ImageHeight);

                    if (!_isXOut && !_isYOut)
                    {
                        if ((int)channel_count == 1)
                        {
                            double grayVal;
                            grayVal = Image.GetGrayval((int)positionY, (int)positionX);
                            str_value = String.Format("#{0}", (int)grayVal);
                        }
                        else if ((int)channel_count == 3)
                        {
                            double grayValRed, grayValGreen, grayValBlue;

                            HImage _RedChannel, _GreenChannel, _BlueChannel;

                            _RedChannel = Image.AccessChannel(1);
                            _GreenChannel = Image.AccessChannel(2);
                            _BlueChannel = Image.AccessChannel(3);

                            grayValRed = _RedChannel.GetGrayval((int)positionY, (int)positionX);
                            grayValGreen = _GreenChannel.GetGrayval((int)positionY, (int)positionX);
                            grayValBlue = _BlueChannel.GetGrayval((int)positionY, (int)positionX);

                            _RedChannel.Dispose();
                            _GreenChannel.Dispose();
                            _BlueChannel.Dispose();

                            str_value = String.Format("#{0}#{1}#{2}", (int)grayValRed, (int)grayValGreen, (int)grayValBlue);
                        }
                        else
                        {
                            str_value = "";
                        }
                    }

                    qtHMouseMove?.Invoke(str_position + str_value + "#"+xScale.ToString()+"#"+yScale.ToString());
                }
                catch (Exception ex)
                {
                    //不处理
                }
            }

            #endregion
        }

        private void ImageDocEx_Paint(object sender, PaintEventArgs e)
        {
            #region ***** 刷新界面，绘图 *****

            if (DesignMode) return;

            HSystem.SetSystem("flush_graphic", "false");
            HWindow.ClearWindow();

            try
            {
                if (Image != null)
                    HWindow.DispObj(Image);
                
                foreach (var obj in m_roiList)
                {
                    obj.draw(HWindow, ImageWidth, ImageHeight);
                }

                foreach (HRegionEntry reg in m_regionList)
                {
                    reg.draw(HWindow);
                }
                
                m_tracker.draw(HWindow);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                HSystem.SetSystem("flush_graphic", "true");

                //注释了下面语句,会导致窗口无法实现缩放和拖动
                HWindow.SetColor("dim gray");
                HWindow.DispLine(-100.0, -100.0, -101.0, -101.0);
            }

            #endregion
        }

        int x, y;
        
        private void ImageDocEx_SizeChanged(object sender, EventArgs e)
        {
            #region ***** 控件大小更改时，重新绘图 *****
            
            if (this.DesignMode)
                return;

            if (Image != null)
            {
                int dx = this.Width - x;
                int dy = this.Height - y;

                dx = (int)(dx * xScale);
                dy = (int)(dy * yScale);

                ImgCol1 -= dx / 2;
                ImgCol2 += dx / 2;
                ImgRow1 -= dy / 2;
                ImgRow2 += dy / 2;

                normalImagePart();

                System.Drawing.Rectangle rect = m_Ctrl_HWindow.ImagePart;
                rect.X = (int)Math.Round(ImgCol1);
                rect.Y = (int)Math.Round(ImgRow1);
                rect.Width = (int)Math.Round(ImgCol2 - ImgCol1);
                rect.Height = (int)Math.Round(ImgRow2 - ImgRow1);
                m_Ctrl_HWindow.ImagePart = rect;
            }

            x = this.Width;
            y = this.Height;

            #endregion
        }

        #endregion

        public void enterSetup(bool bOnOff)
        {
            enteredSetup = bOnOff;
        }

        private void moveImage(double motionX, double motionY)
        {
            #region ***** 移动图片 *****

            ImgRow1 += -motionY;
            ImgRow2 += -motionY;

            ImgCol1 += -motionX;
            ImgCol2 += -motionX;

            normalImagePart();

            System.Drawing.Rectangle rect = m_Ctrl_HWindow.ImagePart;
            rect.X = (int)Math.Round(ImgCol1);
            rect.Y = (int)Math.Round(ImgRow1);
            rect.Width = (int)Math.Round(ImgCol2 - ImgCol1);
            rect.Height = (int)Math.Round(ImgRow2 - ImgRow1);
            m_Ctrl_HWindow.ImagePart = rect;

            #endregion
        }

        private void zoomImage(double x, double y, double scale)
        {
            #region ***** 放大图片 *****
            
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

            System.Drawing.Rectangle rect = m_Ctrl_HWindow.ImagePart;
            rect.X = (int)Math.Round(ImgCol1);
            rect.Y = (int)Math.Round(ImgRow1);
            rect.Width = (int)Math.Round(ImgCol2 - ImgCol1);
            rect.Height = (int)Math.Round(ImgRow2 - ImgRow1);
            m_Ctrl_HWindow.ImagePart = rect;

            #endregion
        }
        
        private void moveROI(double dx, double dy)
        {
            #region ***** 移动ROI *****

            for (int igg = 0; igg < m_l2updateList.Count; igg++) 
            {
                m_l2updateList[igg].move(dx, dy);
            }

            #endregion
        }
        
        public void fitImage()
        {
            #region ***** 最适屏幕大小 *****

            setImagePart(true);
            Invalidate();

            #endregion
        }

        public void setImagePart(bool bFit = false)
        {
            #region ***** 设置图片显示区域 *****

            try
            {
                int width, height;
                string type;
                Image.GetImagePointer1(out type, out width, out height);
                HTuple chan;
                HOperatorSet.CountChannels(Image, out chan);

                if (ImageWidth == width && ImageHeight == height && !bFit)
                    return;

                ImageWidth = width;
                ImageHeight = height;

                handleWidth = ImageHeight / 300 + 1;
                if (handleWidth > 18) handleWidth = 18;

                ImgRow1 = 0;
                ImgCol1 = 0;
                ImgRow2 = height;
                ImgCol2 = width;

                System.Drawing.Rectangle rect = m_Ctrl_HWindow.ImagePart;

                xScale = (double)ImageWidth / m_Ctrl_HWindow.Width;
                yScale = (double)ImageHeight / m_Ctrl_HWindow.Height;

                if (ImageMode == ImageMode.Origin)
                    originScale = xScale = yScale = Math.Max(xScale, yScale);
                else
                    originScale = xScale;

                ImgCol1 = -(m_Ctrl_HWindow.Width * xScale - ImageWidth) / 2;
                ImgCol2 = (m_Ctrl_HWindow.Width * xScale - ImageWidth) / 2 + ImageWidth;
                ImgRow1 = -(m_Ctrl_HWindow.Height * yScale - ImageHeight) / 2;
                ImgRow2 = (m_Ctrl_HWindow.Height * yScale - ImageHeight) / 2 + ImageHeight;

                imageShowWidth = (int)(ImageWidth / xScale + m_Ctrl_HWindow.Width * SHOW_DELTA);
                imageShowHeight = (int)(ImageHeight / yScale + m_Ctrl_HWindow.Height * SHOW_DELTA);


                rect.X = (int)ImgCol1;
                rect.Y = (int)ImgRow1;
                rect.Height = (int)(ImgRow2 - ImgRow1);
                rect.Width = (int)(ImgCol2 - ImgCol1);
                m_Ctrl_HWindow.ImagePart = rect;
            }
            catch (System.Exception ex)
            {

            }

            #endregion
        }

        protected internal void normalImagePart()
        {
            #region ***** 归一化图片显示区域 *****

            if (imageShowWidth < m_Ctrl_HWindow.Width)
            {
                ImgCol1 = -(m_Ctrl_HWindow.Width * xScale - ImageWidth) / 2;
                ImgCol2 = (m_Ctrl_HWindow.Width * xScale - ImageWidth) / 2 + ImageWidth;
            }
            else
            {
                double delta = m_Ctrl_HWindow.Width * SHOW_DELTA * xScale / 2;
                if (ImgCol1 < -delta)
                {
                    ImgCol2 += -delta - ImgCol1;
                    ImgCol1 = -delta;
                }
                else if (ImgCol2 > ImageWidth + delta)
                {
                    ImgCol1 -= ImgCol2 - (ImageWidth + delta);
                    ImgCol2 = ImageWidth + delta;
                }
            }
            if (imageShowHeight < m_Ctrl_HWindow.Height)
            {
                ImgRow1 = -(m_Ctrl_HWindow.Height * yScale - ImageHeight) / 2;
                ImgRow2 = (m_Ctrl_HWindow.Height * yScale - ImageHeight) / 2 + ImageHeight;
            }
            else
            {
                double delta = m_Ctrl_HWindow.Height * SHOW_DELTA * yScale / 2;
                if (ImgRow1 < -delta)
                {
                    ImgRow2 += -delta - ImgRow1;
                    ImgRow1 = -delta;
                }
                else if (ImgRow2 > ImageHeight + delta)
                {
                    ImgRow1 -= ImgRow2 - (ImageHeight + delta);
                    ImgRow2 = ImageHeight + delta;
                }
            }

            #endregion
        }
        
        public void clearRegion()
        {
            #region ***** 清理region内存

            foreach (HRegionEntry reg in m_regionList)
                reg.Dispose();

            m_regionList.Clear();

            Invalidate();

            #endregion
        }

        public void updateRegion(List<HRegionEntry> list)
        {
            #region ***** 更新region显示 *****

            foreach (HRegionEntry reg in m_regionList)
                reg.Dispose();

            m_regionList.Clear();
            m_regionList = list;

            Invalidate();

            #endregion
        }

        public void updateROI(List<ROI> list)
        {
            #region ***** 更新region显示 *****

            m_roiList = list;
            m_l2updateList.Clear();
            iMoveROINum = -2;

            Invalidate();

            #endregion
        }
    }
}
