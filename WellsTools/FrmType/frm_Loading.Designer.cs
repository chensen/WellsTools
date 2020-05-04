namespace Wells.FrmType
{
    partial class frm_Loading
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.proBar_Process = new System.Windows.Forms.ProgressBar();
            this.label_percent = new System.Windows.Forms.Label();
            this.picBox_Wait = new System.Windows.Forms.PictureBox();
            this.tm_Open = new System.Windows.Forms.Timer(this.components);
            this.tm_Work = new System.Windows.Forms.Timer(this.components);
            this.tm_Close = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picBox_Wait)).BeginInit();
            this.SuspendLayout();
            // 
            // proBar_Process
            // 
            this.proBar_Process.Location = new System.Drawing.Point(13, 787);
            this.proBar_Process.Name = "proBar_Process";
            this.proBar_Process.Size = new System.Drawing.Size(840, 47);
            this.proBar_Process.Step = 1;
            this.proBar_Process.TabIndex = 0;
            // 
            // label_percent
            // 
            this.label_percent.AutoSize = true;
            this.label_percent.Font = new System.Drawing.Font("宋体", 16.125F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_percent.ForeColor = System.Drawing.Color.Lime;
            this.label_percent.Location = new System.Drawing.Point(896, 786);
            this.label_percent.Name = "label_percent";
            this.label_percent.Size = new System.Drawing.Size(65, 43);
            this.label_percent.TabIndex = 2;
            this.label_percent.Text = "0%";
            // 
            // picBox_Wait
            // 
            this.picBox_Wait.BackColor = System.Drawing.Color.Transparent;
            this.picBox_Wait.Image = global::Wells.Properties.Resources.waiting;
            this.picBox_Wait.Location = new System.Drawing.Point(1041, 632);
            this.picBox_Wait.Name = "picBox_Wait";
            this.picBox_Wait.Size = new System.Drawing.Size(251, 209);
            this.picBox_Wait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBox_Wait.TabIndex = 1;
            this.picBox_Wait.TabStop = false;
            // 
            // tm_Open
            // 
            this.tm_Open.Tick += new System.EventHandler(this.tm_Open_Tick);
            // 
            // tm_Work
            // 
            this.tm_Work.Tick += new System.EventHandler(this.tm_Work_Tick);
            // 
            // tm_Close
            // 
            this.tm_Close.Tick += new System.EventHandler(this.tm_Close_Tick);
            // 
            // frm_Loading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1296, 846);
            this.Controls.Add(this.label_percent);
            this.Controls.Add(this.picBox_Wait);
            this.Controls.Add(this.proBar_Process);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_Loading";
            this.Text = "frm_Loading";
            ((System.ComponentModel.ISupportInitialize)(this.picBox_Wait)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar proBar_Process;
        private System.Windows.Forms.PictureBox picBox_Wait;
        private System.Windows.Forms.Label label_percent;
        private System.Windows.Forms.Timer tm_Open;
        private System.Windows.Forms.Timer tm_Work;
        private System.Windows.Forms.Timer tm_Close;
    }
}