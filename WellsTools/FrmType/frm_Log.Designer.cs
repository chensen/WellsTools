namespace Wells.FrmType
{
    partial class frm_Log
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Log));
            this.btnOpenLog = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnShowCache = new System.Windows.Forms.Button();
            this.libView = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.chkTopMost = new Wells.WellsMetroControl.Controls.UCCheckBox();
            this.SuspendLayout();
            // 
            // btnOpenLog
            // 
            this.btnOpenLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.btnOpenLog.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnOpenLog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.btnOpenLog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            resources.ApplyResources(this.btnOpenLog, "btnOpenLog");
            this.btnOpenLog.ForeColor = System.Drawing.Color.White;
            this.btnOpenLog.Name = "btnOpenLog";
            this.btnOpenLog.UseVisualStyleBackColor = false;
            this.btnOpenLog.Click += new System.EventHandler(this.btnOpenLog_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.btnClearLog.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnClearLog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.btnClearLog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            resources.ApplyResources(this.btnClearLog, "btnClearLog");
            this.btnClearLog.ForeColor = System.Drawing.Color.White;
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.UseVisualStyleBackColor = false;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnShowCache
            // 
            this.btnShowCache.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.btnShowCache.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnShowCache.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.btnShowCache.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            resources.ApplyResources(this.btnShowCache, "btnShowCache");
            this.btnShowCache.ForeColor = System.Drawing.Color.White;
            this.btnShowCache.Name = "btnShowCache";
            this.btnShowCache.UseVisualStyleBackColor = false;
            this.btnShowCache.Click += new System.EventHandler(this.btnShowCache_Click);
            // 
            // libView
            // 
            this.libView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.libView.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.libView, "libView");
            this.libView.ForeColor = System.Drawing.Color.White;
            this.libView.FormattingEnabled = true;
            this.libView.Name = "libView";
            this.libView.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.libView_DrawItem);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // chkTopMost
            // 
            resources.ApplyResources(this.chkTopMost, "chkTopMost");
            this.chkTopMost.BackColor = System.Drawing.Color.Transparent;
            this.chkTopMost.Checked = false;
            this.chkTopMost.ForeColor = System.Drawing.Color.White;
            this.chkTopMost.Name = "chkTopMost";
            this.chkTopMost.TextValue = "总是最前";
            this.chkTopMost.CheckedChangeEvent += new System.EventHandler(this.chkTopMost_CheckedChangeEvent);
            // 
            // frm_Log
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.Controls.Add(this.chkTopMost);
            this.Controls.Add(this.btnShowCache);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.btnOpenLog);
            this.Controls.Add(this.libView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frm_Log";
            this.ShowBorder = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Log_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.frm_Log_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnShowCache;
        private System.Windows.Forms.ListBox libView;
        private System.Windows.Forms.Timer timer1;
        private WellsMetroControl.Controls.UCCheckBox chkTopMost;
    }
}