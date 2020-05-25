namespace Wells.Controls.ImageDocEx
{
    partial class ImageDocEx
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageDocEx));
            this.m_Ctrl_HWindow = new hvppleDotNet.HWindowControl();
            this.m_Ctrl_HStatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_Ctrl_HWindow
            // 
            this.m_Ctrl_HWindow.BackColor = System.Drawing.Color.DimGray;
            this.m_Ctrl_HWindow.BorderColor = System.Drawing.Color.DimGray;
            this.m_Ctrl_HWindow.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.m_Ctrl_HWindow, "m_Ctrl_HWindow");
            this.m_Ctrl_HWindow.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.m_Ctrl_HWindow.Name = "m_Ctrl_HWindow";
            this.m_Ctrl_HWindow.WindowSize = new System.Drawing.Size(314, 234);
            this.m_Ctrl_HWindow.HMouseMove += new hvppleDotNet.HMouseEventHandler(this.m_Ctrl_HWindow_HMouseMove);
            this.m_Ctrl_HWindow.HMouseDown += new hvppleDotNet.HMouseEventHandler(this.m_Ctrl_HWindow_HMouseDown);
            this.m_Ctrl_HWindow.HMouseUp += new hvppleDotNet.HMouseEventHandler(this.m_Ctrl_HWindow_HMouseUp);
            this.m_Ctrl_HWindow.HMouseWheel += new hvppleDotNet.HMouseEventHandler(this.m_Ctrl_HWindow_HMouseWheel);
            // 
            // m_Ctrl_HStatusLabel
            // 
            resources.ApplyResources(this.m_Ctrl_HStatusLabel, "m_Ctrl_HStatusLabel");
            this.m_Ctrl_HStatusLabel.BackColor = System.Drawing.Color.DimGray;
            this.m_Ctrl_HStatusLabel.ForeColor = System.Drawing.SystemColors.Info;
            this.m_Ctrl_HStatusLabel.Name = "m_Ctrl_HStatusLabel";
            // 
            // ImageDocEx
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.m_Ctrl_HStatusLabel);
            this.Controls.Add(this.m_Ctrl_HWindow);
            this.DoubleBuffered = true;
            this.Name = "ImageDocEx";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageDocEx_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private hvppleDotNet.HWindowControl m_Ctrl_HWindow;
        private System.Windows.Forms.Label m_Ctrl_HStatusLabel;
    }
}
