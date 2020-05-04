using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wells.Controls.VisionInspect
{
    public partial class ImageDoc : Panel
    {
        #region ***** 参数字段变量 *****

        /// <summary>
        /// 整版视图
        /// </summary>
        public clsAreaView m_AreaView = null;

        /// <summary>
        /// FOV视图
        /// </summary>
        public clsCameraView m_CameraView = null;

        /// <summary>
        /// 编辑模式，指示是否可以进入参数设置页面
        /// </summary>
        public int m_iEditType = tagModeType.Mode_Edit;

        /// <summary>
        /// 视图模式，整版或者FOV
        /// </summary>
        public int m_iViewType = tagViewType.View_Area;
        
        /// <summary>
        /// 工具种类
        /// </summary>
        public int m_iToolType = tagToolType.Tool_None;

        /// <summary>
        /// 显示模式
        /// </summary>
        public int m_iShowType = tagShowType.Show_Normal;

        /// <summary>
        /// 记录鼠标起始位置
        /// </summary>
        private Point ptStart, ptEnd;

        /// <summary>
        /// 指示是否处于绘图模式，主要是测量工具
        /// </summary>
        private bool m_bCapture = false;

        /// <summary>
        /// 鼠标坐标数据委托
        /// </summary>
        /// <param name="pt"></param>
        public delegate void syHandle(Point pt);

        /// <summary>
        /// 鼠标坐标数据委托实例
        /// </summary>
        public syHandle syHandleProcess = null;

        #endregion

        public ImageDoc()
        {
            InitializeComponent();
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ImageDoc_MouseWheel);

            base.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            base.UpdateStyles();
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
            
            var obj = clsPCB.get_Instance();//创建唯一实例
            clsPCB.m_pPCB.imageDoc = this;

            initialize();
        }

        public void initialize()
        {
            #region ***** 初始化参数 *****
            
            if (m_AreaView == null)
                m_AreaView = new clsAreaView();
            m_AreaView.linkToView(this);
            m_AreaView.prepareAreaView(base.ClientRectangle, Point.Empty, true);

            if (m_CameraView == null)
                m_CameraView = new clsCameraView();
            m_CameraView.m_iIndex = -1;
            m_CameraView.linkToView(this);

            #endregion
        }

        #region **** 控件事件 *****

        private void ImageDoc_MouseWheel(object sender, MouseEventArgs e)
        {
            #region ***** 鼠标滚轮事件 *****

            if (e.Delta <= -60)
            {
                zoomOut(); 
            }
            else if (e.Delta >= 60)
            {
                zoomIn();
            }

            Invalidate();

            #endregion
        }
        clsPart pSelectedPart = null;
        private void ImageDoc_MouseDown(object sender, MouseEventArgs e)
        {
            #region ***** 鼠标按下操作 *****

            Point pt = new Point(e.X, e.Y);

            if (e.Button == MouseButtons.Left)
            {
                if (m_iEditType == tagModeType.Mode_Edit)
                {
                    if (m_iViewType == tagViewType.View_Area)
                    {
                        if (m_iToolType == tagToolType.Tool_None) 
                        {
                            Point lpt = m_AreaView.VpToLp(pt);
                            pSelectedPart = clsPCB.m_pPCB.findSelectedPart(lpt);
                            if (pSelectedPart != null)
                            {
                                pSelectedPart.m_bSelected = true;
                                ptStart = ptEnd = pt;
                            }
                        }
                        else if (m_iToolType == tagToolType.Tool_Mesure)
                        {
                            #region 测量功能
                            if (!m_bCapture)
                            {
                                ptStart = new Point(e.X, e.Y);
                                ptEnd = new Point(e.X, e.Y);
                                m_bCapture = true;
                            }
                            else
                            {
                                m_bCapture = false;
                                ptEnd.X = e.X;
                                ptEnd.Y = e.Y;

                                Point lpStart = m_AreaView.VpToLp(ptStart);
                                Point lpEnd = m_AreaView.VpToLp(ptEnd);

                                int fDistance = (int)Math.Sqrt(Math.Pow((lpStart.X - lpEnd.X), 2) + Math.Pow((lpStart.Y - lpEnd.Y), 2));
                                int fxDistance = Math.Abs(lpStart.X - lpEnd.X);
                                int fyDistance = Math.Abs(lpStart.Y - lpEnd.Y);

                                string str = "X:" + fxDistance.ToString() + " um;\n" + "Y:" + fyDistance.ToString() + " um;\n" + "XY:" + fDistance.ToString() + " um;\n";
                                Wells.WellsFramework.WellsMetroMessageBox.Show(null, str, clsWellsLanguage.getString(108), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            #endregion
                        }
                    }
                    else if (m_iViewType == tagViewType.View_Camera)
                    {
                        if (m_iToolType == tagToolType.Tool_Mesure)
                        {
                            #region 测量功能
                            if (!m_bCapture)
                            {
                                ptStart = new Point(e.X, e.Y);
                                ptEnd = new Point(e.X, e.Y);
                                m_bCapture = true;
                            }
                            else
                            {
                                m_bCapture = false;
                                ptEnd.X = e.X;
                                ptEnd.Y = e.Y;
                                
                                Point lpStart = m_CameraView.VpToLp(ptStart);
                                Point lpEnd = m_CameraView.VpToLp(ptEnd);

                                int fDistance = (int)Math.Sqrt(Math.Pow((lpStart.X - lpEnd.X), 2) + Math.Pow((lpStart.Y - lpEnd.Y), 2));
                                int fxDistance = Math.Abs(lpStart.X - lpEnd.X);
                                int fyDistance = Math.Abs(lpStart.Y - lpEnd.Y);

                                string str = "X:" + fxDistance.ToString() + " um;\n" + "Y:" + fyDistance.ToString() + " um;\n" + "XY:" + fDistance.ToString() + " um;\n";
                                Wells.WellsFramework.WellsMetroMessageBox.Show(null, str, clsWellsLanguage.getString(108), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            #endregion
                        }
                    }
                }
            }

            #endregion
        }

        private void ImageDoc_MouseUp(object sender, MouseEventArgs e)
        {
            #region ***** 鼠标弹起操作 *****

            if (e.Button == MouseButtons.Left)
            {
                if (m_iEditType == tagModeType.Mode_Edit)
                {
                    if (m_iViewType == tagViewType.View_Area)
                    {
                        if (m_iToolType == tagToolType.Tool_None)
                        {
                            if (pSelectedPart != null)
                            {
                                pSelectedPart.m_bSelected = false;
                                pSelectedPart = null;
                            }
                            else
                            {
                                #region 视图中心跟随鼠标移动

                                Point pt = new Point(e.X, e.Y);
                                pt = m_AreaView.VpToLp(pt);
                                m_AreaView.update(base.ClientRectangle, pt);
                                Invalidate();

                                #endregion
                            }
                        }
                        else if (m_iToolType == tagToolType.Tool_Mesure)
                        {
                            //测量模式，图片不移动，不作响应
                        }
                    }
                    else if (m_iViewType == tagViewType.View_Camera)
                    {
                        //do nothing
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (m_iEditType == tagModeType.Mode_Edit)
                {
                    if (m_iViewType == tagViewType.View_Area)
                    {
                        if (m_iToolType == tagToolType.Tool_Mesure)
                        {
                            #region 测量功能
                            m_iToolType = tagToolType.Tool_None;
                            tspbtnMesureDistance.Checked = false;
                            m_bCapture = false;
                            #endregion
                        }
                        else
                        {
                            tspbtnMesureDistance.Checked = m_iToolType == tagToolType.Tool_Mesure;
                            tspbtnShowGrid.Checked = m_iShowType == tagShowType.Show_Grid;
                            menuStrip.Show(this, new Point(e.X, e.Y), ToolStripDropDownDirection.BelowRight);
                        }
                    }
                    else if (m_iViewType == tagViewType.View_Camera)
                    {
                        if (m_iToolType == tagToolType.Tool_Mesure)
                        {
                            #region 测量功能
                            m_iToolType = tagToolType.Tool_None;
                            tspbtnMesureDistance.Checked = false;
                            #endregion
                        }
                        else
                        {
                            tspbtnMesureDistance.Checked = m_iToolType == tagToolType.Tool_Mesure;
                            tspbtnShowGrid.Checked = m_iShowType == tagShowType.Show_Grid;
                            menuStrip.Show(this, new Point(e.X, e.Y), ToolStripDropDownDirection.BelowRight);
                        }
                    }
                }
            }

            #endregion
        }

        private void ImageDoc_MouseMove1(object sender, MouseEventArgs e)
        {
            #region ***** 鼠标移动事件 *****

            Point pt = new Point(e.X, e.Y);
            pt = m_AreaView.VpToLp(pt);
            if (syHandleProcess != null)
                syHandleProcess(pt);

            if (m_iEditType == tagModeType.Mode_Edit)
            {
                if (m_iViewType == tagViewType.View_Area)
                {
                    if (m_iToolType == tagToolType.Tool_Mesure)
                    {
                        if (m_bCapture)
                        {
                            //ControlPaint.DrawReversibleLine(this.PointToScreen(ptStart), this.PointToScreen(ptEnd), Color.Yellow);
                            ptEnd.X = e.X;
                            ptEnd.Y = e.Y;
                            //Point _pts = PointToScreen(ptStart);
                            //Point _pte = PointToScreen(ptEnd);
                            //Size size = new Size(Math.Abs(_pts.X - _pte.X), Math.Abs(_pts.Y - _pte.Y));
                            //ControlPaint.FillReversibleRectangle(new Rectangle(_pts, size), Color.Yellow);
                            //ControlPaint.FillReversibleRectangle(new Rectangle(_pts, size), Color.Yellow);
                            ControlPaint.DrawReversibleLine(this.PointToScreen(ptStart), this.PointToScreen(ptEnd), Color.Yellow);
                            ControlPaint.DrawReversibleLine(this.PointToScreen(ptStart), this.PointToScreen(ptEnd), Color.Yellow);
                        }
                    }
                }
                else if(m_iViewType == tagViewType.View_Camera)
                {
                    if (m_iToolType == tagToolType.Tool_Mesure)
                    {
                        if (m_bCapture)
                        {
                            //ControlPaint.DrawReversibleLine(this.PointToScreen(ptStart), this.PointToScreen(ptEnd), Color.Yellow);
                            ptEnd.X = e.X;
                            ptEnd.Y = e.Y;
                            ControlPaint.DrawReversibleLine(this.PointToScreen(ptStart), this.PointToScreen(ptEnd), Color.Yellow);
                            ControlPaint.DrawReversibleLine(this.PointToScreen(ptStart), this.PointToScreen(ptEnd), Color.Yellow);
                        }
                    }
                }
            }

            #endregion
        }

        private void ImageDoc_MouseMove(object sender, MouseEventArgs e)
        {
            #region ***** 鼠标移动事件 *****

            Point pt = new Point(e.X, e.Y);
            if (syHandleProcess != null)
                syHandleProcess(m_AreaView.VpToLp(pt));

            if (m_iEditType == tagModeType.Mode_Edit)
            {
                if (m_iViewType == tagViewType.View_Area)
                {
                    if (m_iToolType == tagToolType.Tool_None)
                    {
                        if (pSelectedPart != null)
                        {
                            Point ptOld = m_AreaView.VpToLp(ptStart);
                            Point ptNew = m_AreaView.VpToLp(pt);
                            ptStart = pt;
                            pSelectedPart.m_lptCenter.X += ptNew.X - ptOld.X;
                            pSelectedPart.m_lptCenter.Y += ptNew.Y - ptOld.Y;
                        }
                    }
                    else if (m_iToolType == tagToolType.Tool_Mesure) 
                    {
                        if (m_bCapture)
                        {
                            //ControlPaint.DrawReversibleLine(this.PointToScreen(ptStart), this.PointToScreen(ptEnd), Color.Yellow);
                            ptEnd.X = e.X;
                            ptEnd.Y = e.Y;
                            //Point _pts = PointToScreen(ptStart);
                            //Point _pte = PointToScreen(ptEnd);
                            //Size size = new Size(Math.Abs(_pts.X - _pte.X), Math.Abs(_pts.Y - _pte.Y));
                            //ControlPaint.FillReversibleRectangle(new Rectangle(_pts, size), Color.Yellow);
                            //ControlPaint.FillReversibleRectangle(new Rectangle(_pts, size), Color.Yellow);
                            //ControlPaint.DrawReversibleLine(this.PointToScreen(ptStart), this.PointToScreen(ptEnd), Color.Yellow);
                            //ControlPaint.DrawReversibleLine(this.PointToScreen(ptStart), this.PointToScreen(ptEnd), Color.Yellow);
                        }
                    }
                }
                else if (m_iViewType == tagViewType.View_Camera)
                {
                    if (m_iToolType == tagToolType.Tool_Mesure)
                    {
                        if (m_bCapture)
                        {
                            //ControlPaint.DrawReversibleLine(this.PointToScreen(ptStart), this.PointToScreen(ptEnd), Color.Yellow);
                            ptEnd.X = e.X;
                            ptEnd.Y = e.Y;
                            //ControlPaint.DrawReversibleLine(this.PointToScreen(ptStart), this.PointToScreen(ptEnd), Color.Yellow);
                            //ControlPaint.DrawReversibleLine(this.PointToScreen(ptStart), this.PointToScreen(ptEnd), Color.Yellow);
                        }
                    }
                }
            }

            Invalidate();

            #endregion
        }

        private void ImageDoc_Paint(object sender, PaintEventArgs e)
        {
            #region ***** 绘图操作 *****

            Graphics g = e.Graphics;

            if (m_iViewType == tagViewType.View_Area)
            {
                m_AreaView.draw(g);
            }
            else if (m_iViewType == tagViewType.View_Camera)
            {
                m_CameraView.draw(g, base.ClientRectangle);
            }

            if(m_bCapture)
            {
                g.DrawLine(new Pen((Brush)Brushes.Yellow, 2), ptStart, ptEnd);
            }
            
            #endregion
        }

        private void ImageDoc_SizeChanged(object sender, EventArgs e)
        {
            #region ***** 尺寸改变事件 *****

            m_AreaView.prepareAreaView(base.ClientRectangle, m_AreaView.m_lptCenter);
            Invalidate();

            #endregion
        }

        #endregion

        #region ***** 右键菜单按钮事件 *****

        private void tspbtnZoomFit_Click(object sender, EventArgs e)
        {
            #region ***** 最适屏幕 *****

            zoomFit();
            Invalidate();

            #endregion
        }

        private void tspbtnZoomIn_Click(object sender, EventArgs e)
        {
            #region ***** 屏幕放大 *****

            zoomIn();
            Invalidate();

            #endregion
        }

        private void tspbtnZoomOut_Click(object sender, EventArgs e)
        {
            #region ***** 屏幕放小 *****

            zoomOut();
            Invalidate();

            #endregion
        }
        
        private void tspbtnShowGrid_Click(object sender, EventArgs e)
        {
            #region ***** 显示网格 *****

            m_iShowType = tspbtnShowGrid.Checked ? tagShowType.Show_Grid : tagShowType.Show_Normal;
            Invalidate();

            #endregion
        }

        private void tspbtnSaveImage_Click(object sender, EventArgs e)
        {
            #region ***** 保存图片 *****

            //string path = Application.StartupPath + "\\SaveImages";
            //if (!System.IO.Directory.Exists(path))
            //    System.IO.Directory.CreateDirectory(path);
            //if (m_iViewType == tagViewType.View_Area)
            //{
            //    path += "\\" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss");
            //    if (!System.IO.Directory.Exists(path))
            //        System.IO.Directory.CreateDirectory(path);
            //    int nCount = m_AreaView.m_CameraViewList.Count;
            //    if (nCount > 0)
            //    {
            //        Wells.FrmType.frmProgressBar frm = new Wells.FrmType.frmProgressBar();
            //        frm.SetBarInfo(clsWellsLanguage.getString(109), nCount + 2);
            //        System.Threading.ThreadPool.QueueUserWorkItem((s) => { frm.Start(); });
            //        for (int igg = 0; igg < nCount; igg++)
            //        {
            //            clsCameraView pView = m_AreaView.m_CameraViewList[igg];
            //            if (pView.m_bmp != null)
            //                pView.m_bmp.Save(path + "\\" + pView.m_iIndex.ToString("000") + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            //            frm.SetBarValue(igg);
            //        }
            //        Bitmap bmp = m_AreaView.CreateBoardImageLarge();
            //        if (bmp != null)
            //        {
            //            bmp.Save(path + "\\FullLayout.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            //            frm.SetBarValue(nCount);
            //            bmp.Save(path + "\\FullLayout_thumbnail.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //            frm.SetBarValue(nCount + 1);
            //        }
            //        frm.SetBarValue(nCount + 2);
            //    }
            //}
            //else if (m_iViewType == tagViewType.View_Camera)
            //{
            //    if (m_LiveView.m_bmpLive != null)
            //        m_LiveView.m_bmpLive.Save(path + "\\LiveViewImage"+DateTime.Now.ToString("-yy-MM-dd-hh-mm-ss-fff")+".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            //}

            #endregion
        }

        private void tspbtnMesureDistance_Click(object sender, EventArgs e)
        {
            #region ***** 测量工具开关 *****

            m_iToolType = tspbtnMesureDistance.Checked ? tagToolType.Tool_Mesure : tagToolType.Tool_None;

            #endregion
        }

        #endregion

        public void zoomIn()
        {
            #region 放大操作
            
            if (m_iViewType == tagViewType.View_Area)
            {
                m_AreaView.zoomIn();
            }
            else if (m_iViewType == tagViewType.View_Camera)
            {
                
            }

            #endregion
        }

        public void zoomOut()
        {
            #region 缩小操作
            
            if (m_iViewType == tagViewType.View_Area)
            {
                m_AreaView.zoomOut();
            }
            else if(m_iViewType == tagViewType.View_Camera)
            {

            }

            #endregion
        }

        public void zoomFit()
        {
            #region 最适屏幕
            
            if (m_iViewType == tagViewType.View_Area)
            {
                m_AreaView.zoomFit();
            }
            else if (m_iViewType == tagViewType.View_Camera)
            {
                
            }

            #endregion
        }
    }
}
