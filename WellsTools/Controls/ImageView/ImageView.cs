using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wells.Controls.ImageView
{
    public partial class ImageView : Panel
    {
        public AreaView m_AreaView = null;
        public LiveView m_LiveView = null;

        public int m_iEditType = ConstData.Mode_Edit;
        public int m_iViewType = ConstData.View_Area;
        public int m_iToolType = ConstData.Tool_None;
        public int m_iShowType = ConstData.Show_Normal;
        private Point ptStart, ptEnd;
        private bool m_bCapture = false;

        public delegate void syHandle(Point pt);
        public syHandle syHandleProcess = null;

        public ImageView()
        {
            InitializeComponent();
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ImageView_MouseWheel);

            base.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            base.UpdateStyles();
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
        }

        private void ImageView_MouseWheel(object sender, MouseEventArgs e)
        {
            #region 鼠标滚轮事件

            if (e.Delta <= -60)
            {
                ZoomSmall(); 
            }
            else if (e.Delta >= 60)
            {
                ZoomLarge();
            }

            #endregion
        }

        private void ImageView_MouseDown(object sender, MouseEventArgs e)
        {
            #region 鼠标按下操作

            if(e.Button == MouseButtons.Left)
            {
                if (m_iEditType == ConstData.Mode_Edit)
                {
                    if (m_iViewType == ConstData.View_Area)
                    {
                        if (m_iToolType == ConstData.Tool_Mesure)
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
                    else if (m_iViewType == ConstData.View_Camera)
                    {
                        if (m_iToolType == ConstData.Tool_Mesure)
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
                                
                                Point lpStart = m_LiveView.VpToLp(ptStart);
                                Point lpEnd = m_LiveView.VpToLp(ptEnd);

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

        private void ImageView_MouseUp(object sender, MouseEventArgs e)
        {
            #region 鼠标弹起操作

            if (e.Button == MouseButtons.Left)
            {
                if (m_iEditType == ConstData.Mode_Edit)
                {
                    if (m_iViewType == ConstData.View_Area)
                    {
                        if (m_iToolType == ConstData.Tool_Mesure)
                        {
                            //测量模式，图片不移动，不作响应
                        }
                        else
                        {
                            #region 视图中心跟随鼠标移动
                            Point pt = new Point(e.X, e.Y);
                            pt = m_AreaView.VpToLp(pt);
                            m_AreaView.Update(base.ClientRectangle, pt);
                            Invalidate();
                            #endregion
                        }
                    }
                    else if (m_iViewType == ConstData.View_Camera)
                    {
                        //do nothing
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (m_iEditType == ConstData.Mode_Edit)
                {
                    if (m_iViewType == ConstData.View_Area)
                    {
                        if (m_iToolType == ConstData.Tool_Mesure)
                        {
                            #region 测量功能
                            m_iToolType = ConstData.Tool_None;
                            tsbtnMesureDistance.Checked = false;
                            #endregion
                        }
                        else
                        {
                            tsbtnMesureDistance.Checked = m_iToolType == ConstData.Tool_Mesure;
                            tsbtnShowGrid.Checked = m_iShowType == ConstData.Show_Grid;
                            menuStrip.Show(this, new Point(e.X, e.Y), ToolStripDropDownDirection.BelowRight);
                        }
                    }
                    else if (m_iViewType == ConstData.View_Camera)
                    {
                        if (m_iToolType == ConstData.Tool_Mesure)
                        {
                            #region 测量功能
                            m_iToolType = ConstData.Tool_None;
                            tsbtnMesureDistance.Checked = false;
                            #endregion
                        }
                        else
                        {
                            tsbtnMesureDistance.Checked = m_iToolType == ConstData.Tool_Mesure;
                            tsbtnShowGrid.Checked = m_iShowType == ConstData.Show_Grid;
                            menuStrip.Show(this, new Point(e.X, e.Y), ToolStripDropDownDirection.BelowRight);
                        }
                    }
                }
            }

            #endregion
        }

        private void ImageView_MouseMove(object sender, MouseEventArgs e)
        {
            #region 鼠标移动事件

            Point pt = new Point(e.X, e.Y);
            pt = m_AreaView.VpToLp(pt);
            if (syHandleProcess != null)
                syHandleProcess(pt);

            if (m_iEditType == ConstData.Mode_Edit)
            {
                if (m_iViewType == ConstData.View_Area)
                {
                    if (m_iToolType == ConstData.Tool_Mesure)
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
                else if(m_iViewType == ConstData.View_Camera)
                {
                    if (m_iToolType == ConstData.Tool_Mesure)
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

        public void Initialize(int uSizeX, int uSizeY, int uResolutionX, int uResolutionY, int pWidth, int pHeight, Point ptOriginOffset, int type = ConstData.LeftDown)
        {
            #region 初始化参数

            PCB.m_pPCB.Initialize(uSizeX, uSizeY, uResolutionX, uResolutionY, pWidth, pHeight, ptOriginOffset, type);

            if (m_AreaView == null)
                m_AreaView = new AreaView();
            m_AreaView.LinkToView(this);
            m_AreaView.PrepareAreaView(base.ClientRectangle, Point.Empty, true);
            m_AreaView.PrepareCameraView();

            if (m_LiveView == null)
                m_LiveView = new LiveView();
            m_LiveView.LinkToView(this);
            m_LiveView.PrepareCameraView();
            m_LiveView.PrepareLiveView(base.ClientRectangle, true);
            
            Invalidate();

            #endregion
        }
        

        private void ImageView_Paint(object sender, PaintEventArgs e)
        {
            #region 绘图操作

            //Graphics g = this.CreateGraphics();
            Graphics g = e.Graphics;

            if (m_iViewType == ConstData.View_Area)
            {
                if (m_AreaView != null)
                {
                    m_AreaView.DrawArea(g);
                }
            }
            else if (m_iViewType == ConstData.View_Camera)
            {
                if (m_LiveView != null)
                {
                    m_LiveView.DrawLive(g);
                }
            }
            

            #endregion
        }
        
        public void ZoomLarge()
        {
            #region 放大操作

            if (1 == System.Threading.Interlocked.Read(ref LockedSign.l2ShowIsCreatingImage))
                return;

            if (m_iViewType == ConstData.View_Area)
            {
                decimal oldScale = m_AreaView.m_iScale;
                if (m_AreaView.m_iScaleIndex >= 1)
                {
                    m_AreaView.m_iScale = m_AreaView.m_iScaleList[--m_AreaView.m_iScaleIndex];
                    if(m_AreaView.m_iScale!=oldScale)
                    {
                        m_AreaView.Update(m_AreaView.m_rcScreenArea, m_AreaView.m_ptCenter);
                        Invalidate();
                    }
                }
            }
            else if (m_iViewType == ConstData.View_Camera)
            {
                //int index = -1;
                //decimal oldScale = m_LiveView.m_iScale;
                //for (int igg = 0; igg < 11; igg++)
                //{
                //    if (m_LiveView.m_iScaleList[igg] == m_LiveView.m_iScale)
                //    {
                //        index = igg;
                //        break;
                //    }
                //}
                //if (index >= 1)
                //{
                //    m_LiveView.m_iScale = m_LiveView.m_iScaleList[index - 1];
                //    if (m_LiveView.m_iScale != oldScale)
                //    {
                //        m_LiveView.Update(m_LiveView.m_rcScreenArea);
                //        Invalidate();
                //    }
                //}
            }

            #endregion
        }

        public void ZoomSmall()
        {
            #region 缩小操作

            if (1 == System.Threading.Interlocked.Read(ref LockedSign.l2ShowIsCreatingImage))
                return;

            if (m_iViewType == ConstData.View_Area)
            {
                decimal oldScale = m_AreaView.m_iScale;
                if (m_AreaView.m_iScaleIndex <= ConstData.Scale_Num-2)
                {
                    m_AreaView.m_iScale = m_AreaView.m_iScaleList[++m_AreaView.m_iScaleIndex];
                    if (m_AreaView.m_iScale != oldScale)
                    {
                        m_AreaView.Update(m_AreaView.m_rcScreenArea, m_AreaView.m_ptCenter);
                        Invalidate();
                    }
                }
            }
            else if(m_iViewType == ConstData.View_Camera)
            {
                //int index = -1;
                //decimal oldScale = m_LiveView.m_iScale;
                //for (int igg = 0; igg < 11; igg++)
                //{
                //    if (m_LiveView.m_iScaleList[igg] == m_LiveView.m_iScale)
                //    {
                //        index = igg;
                //        break;
                //    }
                //}
                //if (index <= 9)
                //{
                //    m_LiveView.m_iScale = m_LiveView.m_iScaleList[index + 1];
                //    if (m_LiveView.m_iScale != oldScale)
                //    {
                //        m_LiveView.Update(m_LiveView.m_rcScreenArea);
                //        Invalidate();
                //    }
                //}
            }

            #endregion
        }

        public void ZoomFit()
        {
            #region 最适屏幕

            if (1 == System.Threading.Interlocked.Read(ref LockedSign.l2ShowIsCreatingImage))
                return;

            if (m_iViewType == ConstData.View_Area)
            {
                m_AreaView.CalFitScale();
                m_AreaView.m_iScaleIndex = ConstData.Scale_Num / 2;
                m_AreaView.m_iScale = m_AreaView.m_iScaleList[m_AreaView.m_iScaleIndex];
                m_AreaView.m_ptCenter.X = PCB.m_pPCB.m_uSizeX / 2;
                m_AreaView.m_ptCenter.Y = PCB.m_pPCB.m_uSizeY / 2;
                m_AreaView.m_ptCenter.Offset(PCB.m_pPCB.m_ptOriginOffset);
                m_AreaView.Update(m_AreaView.m_rcScreenArea, m_AreaView.m_ptCenter);
                Invalidate();
            }
            else if (m_iViewType == ConstData.View_Camera)
            {
                //m_LiveView.Update(m_LiveView.m_rcScreenArea, true);
                //Invalidate();
            }

            #endregion
        }

        private void tsbtnFit_Click(object sender, EventArgs e)
        {
            ZoomFit();
        }

        private void tsbtnZoomLarge_Click(object sender, EventArgs e)
        {
            ZoomLarge();
        }

        private void tsbtnZoomSmall_Click(object sender, EventArgs e)
        {
            ZoomSmall();
        }

        private void ImageView_SizeChanged(object sender, EventArgs e)
        {
            if (m_AreaView != null)
            {
                m_AreaView.PrepareAreaView(base.ClientRectangle, m_AreaView.m_ptCenter);
                UpdateView();
            }
            if (m_LiveView != null)
            {
                //m_LiveView.PrepareLiveView(base.ClientRectangle);
                //m_LiveView.CreateBoardImage();
                //Invalidate();
            }
        }

        private void tsbtnShowGrid_Click(object sender, EventArgs e)
        {
            m_iShowType = tsbtnShowGrid.Checked ? ConstData.Show_Grid : ConstData.Show_Normal;
            Invalidate();
        }

        private void tsbtnSaveImage_Click(object sender, EventArgs e)
        {
            string path = Application.StartupPath + "\\SaveImages";
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            if (m_iViewType == ConstData.View_Area)
            {
                path += "\\" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss");
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                int nCount = m_AreaView.m_CameraViewList.Count;
                if (nCount > 0)
                {
                    Wells.FrmType.frmProgressBar frm = new Wells.FrmType.frmProgressBar();
                    frm.SetBarInfo(clsWellsLanguage.getString(109), nCount + 2);
                    System.Threading.ThreadPool.QueueUserWorkItem((s) => { frm.Start(); });
                    for (int igg = 0; igg < nCount; igg++)
                    {
                        CameraView pView = m_AreaView.m_CameraViewList[igg];
                        if (pView.m_bmp != null)
                            pView.m_bmp.Save(path + "\\" + pView.m_iIndex.ToString("000") + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                        frm.SetBarValue(igg);
                    }
                    Bitmap bmp = m_AreaView.CreateBoardImageLarge();
                    if (bmp != null)
                    {
                        bmp.Save(path + "\\FullLayout.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                        frm.SetBarValue(nCount);
                        bmp.Save(path + "\\FullLayout_thumbnail.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        frm.SetBarValue(nCount + 1);
                    }
                    frm.SetBarValue(nCount + 2);
                }
            }
            else if (m_iViewType == ConstData.View_Camera)
            {
                if (m_LiveView.m_bmpLive != null)
                    m_LiveView.m_bmpLive.Save(path + "\\LiveViewImage"+DateTime.Now.ToString("-yy-MM-dd-hh-mm-ss-fff")+".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            }
        }

        private void tsbtnMesureDistance_Click(object sender, EventArgs e)
        {
            m_iToolType = tsbtnMesureDistance.Checked ? ConstData.Tool_Mesure : ConstData.Tool_None;
        }

        public void UpdateView()
        {
            new System.Threading.Thread(delegate ()
            {
                if (m_iViewType == ConstData.View_Area)
                {
                    if (m_AreaView.CreateBoardImage())
                        this.Invalidate();
                }
                else if(m_iViewType == ConstData.View_Camera)
                {

                }
            }).Start();
        }
    }
}
