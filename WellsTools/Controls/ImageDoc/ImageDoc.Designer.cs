namespace Wells.Controls.ImageDoc
{
    partial class ImageDoc
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageDoc));
            this.mCtrl_HWindow = new hvppleDotNet.HWindowControl();
            this.hv_MenuStrip = new CCWin.SkinControl.SkinContextMenuStrip();
            this.tsbtn_fitWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtn_OriginSize = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtn_ZoomPercent = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtn_saveOriginImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtn_saveDumpWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtn_showInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtn_showMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtn_showROI = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtn_showRegion = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtn_showHat = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtn_showStatusBar = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtn_showFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtn_showHisto = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtn_MeasureDistance = new System.Windows.Forms.ToolStripMenuItem();
            this.m_CtrlHStatusLabelCtrl = new System.Windows.Forms.Label();
            this.hv_MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mCtrl_HWindow
            // 
            this.mCtrl_HWindow.BackColor = System.Drawing.Color.DimGray;
            this.mCtrl_HWindow.BorderColor = System.Drawing.Color.DimGray;
            this.mCtrl_HWindow.ContextMenuStrip = this.hv_MenuStrip;
            this.mCtrl_HWindow.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.mCtrl_HWindow, "mCtrl_HWindow");
            this.mCtrl_HWindow.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.mCtrl_HWindow.Name = "mCtrl_HWindow";
            this.mCtrl_HWindow.WindowSize = new System.Drawing.Size(367, 248);
            this.mCtrl_HWindow.HMouseMove += new hvppleDotNet.HMouseEventHandler(this.HWindowControl_HMouseMove);
            this.mCtrl_HWindow.SizeChanged += new System.EventHandler(this.mCtrl_HWindow_SizeChanged);
            this.mCtrl_HWindow.Click += new System.EventHandler(this.mCtrl_HWindow_Click);
            this.mCtrl_HWindow.DoubleClick += new System.EventHandler(this.mCtrl_HWindow_DoubleClick);
            this.mCtrl_HWindow.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mCtrl_HWindow_MouseDoubleClick);
            this.mCtrl_HWindow.MouseLeave += new System.EventHandler(this.mCtrl_HWindow_MouseLeave);
            // 
            // hv_MenuStrip
            // 
            this.hv_MenuStrip.Arrow = System.Drawing.Color.DeepPink;
            this.hv_MenuStrip.Back = System.Drawing.Color.LavenderBlush;
            this.hv_MenuStrip.BackColor = System.Drawing.Color.LavenderBlush;
            this.hv_MenuStrip.BackRadius = 4;
            this.hv_MenuStrip.Base = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.hv_MenuStrip.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            resources.ApplyResources(this.hv_MenuStrip, "hv_MenuStrip");
            this.hv_MenuStrip.Fore = System.Drawing.Color.Blue;
            this.hv_MenuStrip.HoverFore = System.Drawing.Color.White;
            this.hv_MenuStrip.ItemAnamorphosis = true;
            this.hv_MenuStrip.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.hv_MenuStrip.ItemBorderShow = true;
            this.hv_MenuStrip.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.hv_MenuStrip.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.hv_MenuStrip.ItemRadius = 4;
            this.hv_MenuStrip.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.hv_MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtn_fitWindow,
            this.tsbtn_OriginSize,
            this.tsbtn_ZoomPercent,
            this.toolStripSeparator1,
            this.tsbtn_saveOriginImage,
            this.tsbtn_saveDumpWindow,
            this.tsbtn_showInfo,
            this.tsbtn_showFunction});
            this.hv_MenuStrip.ItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.hv_MenuStrip.Name = "hv_MenuStrip";
            this.hv_MenuStrip.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.hv_MenuStrip.SkinAllColor = true;
            this.hv_MenuStrip.TitleAnamorphosis = true;
            this.hv_MenuStrip.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.hv_MenuStrip.TitleRadius = 4;
            this.hv_MenuStrip.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // tsbtn_fitWindow
            // 
            resources.ApplyResources(this.tsbtn_fitWindow, "tsbtn_fitWindow");
            this.tsbtn_fitWindow.Name = "tsbtn_fitWindow";
            this.tsbtn_fitWindow.Click += new System.EventHandler(this.tsbtn_fitWindow_Click);
            // 
            // tsbtn_OriginSize
            // 
            resources.ApplyResources(this.tsbtn_OriginSize, "tsbtn_OriginSize");
            this.tsbtn_OriginSize.Name = "tsbtn_OriginSize";
            this.tsbtn_OriginSize.Click += new System.EventHandler(this.tsbtn_OriginSize_Click);
            // 
            // tsbtn_ZoomPercent
            // 
            resources.ApplyResources(this.tsbtn_ZoomPercent, "tsbtn_ZoomPercent");
            this.tsbtn_ZoomPercent.Name = "tsbtn_ZoomPercent";
            this.tsbtn_ZoomPercent.Click += new System.EventHandler(this.tsbtn_ZoomPercent_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tsbtn_saveOriginImage
            // 
            resources.ApplyResources(this.tsbtn_saveOriginImage, "tsbtn_saveOriginImage");
            this.tsbtn_saveOriginImage.Name = "tsbtn_saveOriginImage";
            this.tsbtn_saveOriginImage.Click += new System.EventHandler(this.tsbtn_saveOriginImage_Click);
            // 
            // tsbtn_saveDumpWindow
            // 
            resources.ApplyResources(this.tsbtn_saveDumpWindow, "tsbtn_saveDumpWindow");
            this.tsbtn_saveDumpWindow.Name = "tsbtn_saveDumpWindow";
            this.tsbtn_saveDumpWindow.Click += new System.EventHandler(this.tsbtn_saveDumpWindow_Click);
            // 
            // tsbtn_showInfo
            // 
            this.tsbtn_showInfo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtn_showMessage,
            this.tsbtn_showROI,
            this.tsbtn_showRegion,
            this.tsbtn_showHat,
            this.tsbtn_showStatusBar});
            resources.ApplyResources(this.tsbtn_showInfo, "tsbtn_showInfo");
            this.tsbtn_showInfo.Name = "tsbtn_showInfo";
            // 
            // tsbtn_showMessage
            // 
            this.tsbtn_showMessage.Checked = true;
            this.tsbtn_showMessage.CheckOnClick = true;
            this.tsbtn_showMessage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbtn_showMessage.Name = "tsbtn_showMessage";
            resources.ApplyResources(this.tsbtn_showMessage, "tsbtn_showMessage");
            this.tsbtn_showMessage.Click += new System.EventHandler(this.tsbtn_showMessage_Click);
            // 
            // tsbtn_showROI
            // 
            this.tsbtn_showROI.Checked = true;
            this.tsbtn_showROI.CheckOnClick = true;
            this.tsbtn_showROI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbtn_showROI.Name = "tsbtn_showROI";
            resources.ApplyResources(this.tsbtn_showROI, "tsbtn_showROI");
            this.tsbtn_showROI.Click += new System.EventHandler(this.tsbtn_showROI_Click);
            // 
            // tsbtn_showRegion
            // 
            this.tsbtn_showRegion.Checked = true;
            this.tsbtn_showRegion.CheckOnClick = true;
            this.tsbtn_showRegion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbtn_showRegion.Name = "tsbtn_showRegion";
            resources.ApplyResources(this.tsbtn_showRegion, "tsbtn_showRegion");
            this.tsbtn_showRegion.Click += new System.EventHandler(this.tsbtn_showRegion_Click);
            // 
            // tsbtn_showHat
            // 
            this.tsbtn_showHat.CheckOnClick = true;
            this.tsbtn_showHat.Name = "tsbtn_showHat";
            resources.ApplyResources(this.tsbtn_showHat, "tsbtn_showHat");
            this.tsbtn_showHat.Click += new System.EventHandler(this.tsbtn_showHat_Click);
            // 
            // tsbtn_showStatusBar
            // 
            this.tsbtn_showStatusBar.CheckOnClick = true;
            this.tsbtn_showStatusBar.Name = "tsbtn_showStatusBar";
            resources.ApplyResources(this.tsbtn_showStatusBar, "tsbtn_showStatusBar");
            this.tsbtn_showStatusBar.Click += new System.EventHandler(this.tsbtn_showStatusBar_Click);
            // 
            // tsbtn_showFunction
            // 
            this.tsbtn_showFunction.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtn_showHisto,
            this.tsbtn_MeasureDistance});
            resources.ApplyResources(this.tsbtn_showFunction, "tsbtn_showFunction");
            this.tsbtn_showFunction.Name = "tsbtn_showFunction";
            // 
            // tsbtn_showHisto
            // 
            this.tsbtn_showHisto.Name = "tsbtn_showHisto";
            resources.ApplyResources(this.tsbtn_showHisto, "tsbtn_showHisto");
            this.tsbtn_showHisto.Click += new System.EventHandler(this.tsbtn_showHisto_Click);
            // 
            // tsbtn_MeasureDistance
            // 
            this.tsbtn_MeasureDistance.Name = "tsbtn_MeasureDistance";
            resources.ApplyResources(this.tsbtn_MeasureDistance, "tsbtn_MeasureDistance");
            this.tsbtn_MeasureDistance.Click += new System.EventHandler(this.tsbtn_MeasureDistance_Click);
            // 
            // m_CtrlHStatusLabelCtrl
            // 
            resources.ApplyResources(this.m_CtrlHStatusLabelCtrl, "m_CtrlHStatusLabelCtrl");
            this.m_CtrlHStatusLabelCtrl.BackColor = System.Drawing.Color.White;
            this.m_CtrlHStatusLabelCtrl.ForeColor = System.Drawing.SystemColors.WindowText;
            this.m_CtrlHStatusLabelCtrl.Name = "m_CtrlHStatusLabelCtrl";
            // 
            // ImageDoc
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.m_CtrlHStatusLabelCtrl);
            this.Controls.Add(this.mCtrl_HWindow);
            this.DoubleBuffered = true;
            this.Name = "ImageDoc";
            this.hv_MenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private hvppleDotNet.HWindowControl mCtrl_HWindow;
        private System.Windows.Forms.Label m_CtrlHStatusLabelCtrl;
        private CCWin.SkinControl.SkinContextMenuStrip hv_MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_fitWindow;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_showInfo;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_saveOriginImage;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_saveDumpWindow;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_showFunction;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_OriginSize;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_ZoomPercent;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_showMessage;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_showROI;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_showRegion;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_showHat;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_showStatusBar;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_showHisto;
        private System.Windows.Forms.ToolStripMenuItem tsbtn_MeasureDistance;
    }
}
