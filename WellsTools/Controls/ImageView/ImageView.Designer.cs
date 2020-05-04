namespace Wells.Controls.ImageView
{
    partial class ImageView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageView));
            this.menuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsbtnZoomLarge = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnZoomSmall = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnFit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnShowGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnMesureDistance = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnZoomLarge,
            this.toolStripSeparator1,
            this.tsbtnZoomSmall,
            this.toolStripSeparator2,
            this.tsbtnFit,
            this.toolStripSeparator3,
            this.tsbtnShowGrid,
            this.toolStripSeparator4,
            this.tsbtnMesureDistance,
            this.toolStripSeparator5,
            this.tsbtnSaveImage,
            this.toolStripSeparator6});
            this.menuStrip.Name = "menuStrip";
            // 
            // tsbtnZoomLarge
            // 
            resources.ApplyResources(this.tsbtnZoomLarge, "tsbtnZoomLarge");
            this.tsbtnZoomLarge.Name = "tsbtnZoomLarge";
            this.tsbtnZoomLarge.Click += new System.EventHandler(this.tsbtnZoomLarge_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // tsbtnZoomSmall
            // 
            resources.ApplyResources(this.tsbtnZoomSmall, "tsbtnZoomSmall");
            this.tsbtnZoomSmall.Name = "tsbtnZoomSmall";
            this.tsbtnZoomSmall.Click += new System.EventHandler(this.tsbtnZoomSmall_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // tsbtnFit
            // 
            resources.ApplyResources(this.tsbtnFit, "tsbtnFit");
            this.tsbtnFit.Name = "tsbtnFit";
            this.tsbtnFit.Click += new System.EventHandler(this.tsbtnFit_Click);
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // tsbtnShowGrid
            // 
            resources.ApplyResources(this.tsbtnShowGrid, "tsbtnShowGrid");
            this.tsbtnShowGrid.CheckOnClick = true;
            this.tsbtnShowGrid.Name = "tsbtnShowGrid";
            this.tsbtnShowGrid.Click += new System.EventHandler(this.tsbtnShowGrid_Click);
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // tsbtnMesureDistance
            // 
            resources.ApplyResources(this.tsbtnMesureDistance, "tsbtnMesureDistance");
            this.tsbtnMesureDistance.CheckOnClick = true;
            this.tsbtnMesureDistance.Name = "tsbtnMesureDistance";
            this.tsbtnMesureDistance.Click += new System.EventHandler(this.tsbtnMesureDistance_Click);
            // 
            // toolStripSeparator5
            // 
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // tsbtnSaveImage
            // 
            resources.ApplyResources(this.tsbtnSaveImage, "tsbtnSaveImage");
            this.tsbtnSaveImage.Name = "tsbtnSaveImage";
            this.tsbtnSaveImage.Click += new System.EventHandler(this.tsbtnSaveImage_Click);
            // 
            // toolStripSeparator6
            // 
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            // 
            // ImageView
            // 
            resources.ApplyResources(this, "$this");
            this.SizeChanged += new System.EventHandler(this.ImageView_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageView_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageView_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageView_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageView_MouseUp);
            this.menuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsbtnZoomLarge;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsbtnZoomSmall;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsbtnFit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tsbtnMesureDistance;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem tsbtnShowGrid;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsbtnSaveImage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    }
}
