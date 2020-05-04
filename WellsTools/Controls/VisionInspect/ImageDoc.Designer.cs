namespace Wells.Controls.VisionInspect
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageDoc));
            this.menuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tspbtnZoomIn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tspbtnZoomOut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tspbtnZoomFit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tspbtnShowGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tspbtnMesureDistance = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tspbtnSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspbtnZoomIn,
            this.toolStripSeparator1,
            this.tspbtnZoomOut,
            this.toolStripSeparator2,
            this.tspbtnZoomFit,
            this.toolStripSeparator3,
            this.tspbtnShowGrid,
            this.toolStripSeparator4,
            this.tspbtnMesureDistance,
            this.toolStripSeparator5,
            this.tspbtnSaveImage,
            this.toolStripSeparator6});
            this.menuStrip.Name = "menuStrip";
            resources.ApplyResources(this.menuStrip, "menuStrip");
            // 
            // tspbtnZoomIn
            // 
            this.tspbtnZoomIn.Name = "tspbtnZoomIn";
            resources.ApplyResources(this.tspbtnZoomIn, "tspbtnZoomIn");
            this.tspbtnZoomIn.Click += new System.EventHandler(this.tspbtnZoomIn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tspbtnZoomOut
            // 
            this.tspbtnZoomOut.Name = "tspbtnZoomOut";
            resources.ApplyResources(this.tspbtnZoomOut, "tspbtnZoomOut");
            this.tspbtnZoomOut.Click += new System.EventHandler(this.tspbtnZoomOut_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // tspbtnZoomFit
            // 
            this.tspbtnZoomFit.Name = "tspbtnZoomFit";
            resources.ApplyResources(this.tspbtnZoomFit, "tspbtnZoomFit");
            this.tspbtnZoomFit.Click += new System.EventHandler(this.tspbtnZoomFit_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // tspbtnShowGrid
            // 
            this.tspbtnShowGrid.CheckOnClick = true;
            this.tspbtnShowGrid.Name = "tspbtnShowGrid";
            resources.ApplyResources(this.tspbtnShowGrid, "tspbtnShowGrid");
            this.tspbtnShowGrid.Click += new System.EventHandler(this.tspbtnShowGrid_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // tspbtnMesureDistance
            // 
            this.tspbtnMesureDistance.CheckOnClick = true;
            this.tspbtnMesureDistance.Name = "tspbtnMesureDistance";
            resources.ApplyResources(this.tspbtnMesureDistance, "tspbtnMesureDistance");
            this.tspbtnMesureDistance.Click += new System.EventHandler(this.tspbtnMesureDistance_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // tspbtnSaveImage
            // 
            this.tspbtnSaveImage.Name = "tspbtnSaveImage";
            resources.ApplyResources(this.tspbtnSaveImage, "tspbtnSaveImage");
            this.tspbtnSaveImage.Click += new System.EventHandler(this.tspbtnSaveImage_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // ImageDoc
            // 
            this.SizeChanged += new System.EventHandler(this.ImageDoc_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageDoc_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageDoc_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageDoc_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageDoc_MouseUp);
            this.menuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem tspbtnZoomIn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tspbtnZoomOut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tspbtnZoomFit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tspbtnMesureDistance;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem tspbtnShowGrid;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tspbtnSaveImage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    }
}
