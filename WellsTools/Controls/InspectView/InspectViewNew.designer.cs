namespace Wells.Controls.InspectViewNew
{
    partial class InspectViewNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InspectViewNew));
            this.rBtnClickMenuStrip = new CCWin.SkinControl.SkinContextMenuStrip();
            this.btnTsFit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTsZoomIn = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTsZoomOut = new System.Windows.Forms.ToolStripMenuItem();
            this.rBtnClickMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // rBtnClickMenuStrip
            // 
            resources.ApplyResources(this.rBtnClickMenuStrip, "rBtnClickMenuStrip");
            this.rBtnClickMenuStrip.Arrow = System.Drawing.Color.Black;
            this.rBtnClickMenuStrip.Back = System.Drawing.Color.White;
            this.rBtnClickMenuStrip.BackRadius = 4;
            this.rBtnClickMenuStrip.Base = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.rBtnClickMenuStrip.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.rBtnClickMenuStrip.Fore = System.Drawing.Color.Black;
            this.rBtnClickMenuStrip.HoverFore = System.Drawing.Color.White;
            this.rBtnClickMenuStrip.ItemAnamorphosis = true;
            this.rBtnClickMenuStrip.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.rBtnClickMenuStrip.ItemBorderShow = true;
            this.rBtnClickMenuStrip.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.rBtnClickMenuStrip.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.rBtnClickMenuStrip.ItemRadius = 4;
            this.rBtnClickMenuStrip.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.rBtnClickMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTsFit,
            this.btnTsZoomIn,
            this.btnTsZoomOut});
            this.rBtnClickMenuStrip.ItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.rBtnClickMenuStrip.Name = "rBtnClickMenuStrip";
            this.rBtnClickMenuStrip.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.rBtnClickMenuStrip.SkinAllColor = true;
            this.rBtnClickMenuStrip.TitleAnamorphosis = true;
            this.rBtnClickMenuStrip.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.rBtnClickMenuStrip.TitleRadius = 4;
            this.rBtnClickMenuStrip.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.rBtnClickMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.rBtnClickMenuStrip_ItemClicked);
            // 
            // btnTsFit
            // 
            resources.ApplyResources(this.btnTsFit, "btnTsFit");
            this.btnTsFit.Name = "btnTsFit";
            // 
            // btnTsZoomIn
            // 
            resources.ApplyResources(this.btnTsZoomIn, "btnTsZoomIn");
            this.btnTsZoomIn.Name = "btnTsZoomIn";
            // 
            // btnTsZoomOut
            // 
            resources.ApplyResources(this.btnTsZoomOut, "btnTsZoomOut");
            this.btnTsZoomOut.Name = "btnTsZoomOut";
            // 
            // InspectViewNew
            // 
            resources.ApplyResources(this, "$this");
            this.ContextMenuStrip = this.rBtnClickMenuStrip;
            this.rBtnClickMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinContextMenuStrip rBtnClickMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem btnTsFit;
        private System.Windows.Forms.ToolStripMenuItem btnTsZoomIn;
        private System.Windows.Forms.ToolStripMenuItem btnTsZoomOut;
    }
}
