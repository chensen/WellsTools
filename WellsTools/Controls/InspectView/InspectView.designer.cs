namespace Wells.Controls.InspectView
{
    partial class InspectView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InspectView));
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lbYpos = new CCWin.SkinControl.SkinLabel();
            this.lbImgInfo = new CCWin.SkinControl.SkinLabel();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.lbXpos = new CCWin.SkinControl.SkinLabel();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.pnlFov = new System.Windows.Forms.Panel();
            this.rBtnClickMenuStrip = new CCWin.SkinControl.SkinContextMenuStrip();
            this.btnTsFit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTsZoomIn = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTsZoomOut = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlStatus.SuspendLayout();
            this.rBtnClickMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlStatus
            // 
            resources.ApplyResources(this.pnlStatus, "pnlStatus");
            this.pnlStatus.BackColor = System.Drawing.Color.DimGray;
            this.pnlStatus.Controls.Add(this.lbYpos);
            this.pnlStatus.Controls.Add(this.lbImgInfo);
            this.pnlStatus.Controls.Add(this.skinLabel3);
            this.pnlStatus.Controls.Add(this.lbXpos);
            this.pnlStatus.Controls.Add(this.skinLabel1);
            this.pnlStatus.Name = "pnlStatus";
            // 
            // lbYpos
            // 
            resources.ApplyResources(this.lbYpos, "lbYpos");
            this.lbYpos.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.lbYpos.BackColor = System.Drawing.Color.Transparent;
            this.lbYpos.BorderColor = System.Drawing.Color.White;
            this.lbYpos.ForeColor = System.Drawing.Color.Yellow;
            this.lbYpos.Name = "lbYpos";
            // 
            // lbImgInfo
            // 
            resources.ApplyResources(this.lbImgInfo, "lbImgInfo");
            this.lbImgInfo.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.lbImgInfo.BackColor = System.Drawing.Color.Transparent;
            this.lbImgInfo.BorderColor = System.Drawing.Color.White;
            this.lbImgInfo.ForeColor = System.Drawing.Color.Blue;
            this.lbImgInfo.Name = "lbImgInfo";
            // 
            // skinLabel3
            // 
            resources.ApplyResources(this.skinLabel3, "skinLabel3");
            this.skinLabel3.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel3.BackColor = System.Drawing.Color.Lime;
            this.skinLabel3.BorderColor = System.Drawing.Color.White;
            this.skinLabel3.ForeColor = System.Drawing.Color.Fuchsia;
            this.skinLabel3.Name = "skinLabel3";
            // 
            // lbXpos
            // 
            resources.ApplyResources(this.lbXpos, "lbXpos");
            this.lbXpos.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.lbXpos.BackColor = System.Drawing.Color.Transparent;
            this.lbXpos.BorderColor = System.Drawing.Color.White;
            this.lbXpos.ForeColor = System.Drawing.Color.Yellow;
            this.lbXpos.Name = "lbXpos";
            // 
            // skinLabel1
            // 
            resources.ApplyResources(this.skinLabel1, "skinLabel1");
            this.skinLabel1.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.None;
            this.skinLabel1.BackColor = System.Drawing.Color.Lime;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.ForeColor = System.Drawing.Color.Fuchsia;
            this.skinLabel1.Name = "skinLabel1";
            // 
            // pnlFov
            // 
            resources.ApplyResources(this.pnlFov, "pnlFov");
            this.pnlFov.BackColor = System.Drawing.Color.Silver;
            this.pnlFov.ContextMenuStrip = this.rBtnClickMenuStrip;
            this.pnlFov.Name = "pnlFov";
            this.pnlFov.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlFov_Paint);
            this.pnlFov.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlFov_MouseMove);
            this.pnlFov.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlFov_MouseUp);
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
            // InspectView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlFov);
            this.Controls.Add(this.pnlStatus);
            this.Name = "InspectView";
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.rBtnClickMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlStatus;
        private CCWin.SkinControl.SkinLabel lbYpos;
        private CCWin.SkinControl.SkinLabel lbImgInfo;
        private CCWin.SkinControl.SkinLabel skinLabel3;
        private CCWin.SkinControl.SkinLabel lbXpos;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private System.Windows.Forms.Panel pnlFov;
        private CCWin.SkinControl.SkinContextMenuStrip rBtnClickMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem btnTsFit;
        private System.Windows.Forms.ToolStripMenuItem btnTsZoomIn;
        private System.Windows.Forms.ToolStripMenuItem btnTsZoomOut;
    }
}
