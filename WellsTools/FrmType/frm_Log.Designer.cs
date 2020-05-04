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
            this.chkTopMost = new System.Windows.Forms.CheckBox();
            this.libView = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnOpenLog
            // 
            this.btnOpenLog.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnOpenLog.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.btnOpenLog.FlatAppearance.BorderSize = 2;
            this.btnOpenLog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Aqua;
            this.btnOpenLog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            resources.ApplyResources(this.btnOpenLog, "btnOpenLog");
            this.btnOpenLog.ForeColor = System.Drawing.Color.Black;
            this.btnOpenLog.Name = "btnOpenLog";
            this.btnOpenLog.UseVisualStyleBackColor = false;
            this.btnOpenLog.Click += new System.EventHandler(this.btnOpenLog_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.BackColor = System.Drawing.Color.PaleVioletRed;
            this.btnClearLog.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnClearLog.FlatAppearance.BorderSize = 2;
            this.btnClearLog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.btnClearLog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateGray;
            resources.ApplyResources(this.btnClearLog, "btnClearLog");
            this.btnClearLog.ForeColor = System.Drawing.Color.Blue;
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.UseVisualStyleBackColor = false;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnShowCache
            // 
            this.btnShowCache.BackColor = System.Drawing.Color.LightCoral;
            this.btnShowCache.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.btnShowCache.FlatAppearance.BorderSize = 2;
            this.btnShowCache.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCoral;
            this.btnShowCache.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Aqua;
            resources.ApplyResources(this.btnShowCache, "btnShowCache");
            this.btnShowCache.ForeColor = System.Drawing.Color.Gold;
            this.btnShowCache.Name = "btnShowCache";
            this.btnShowCache.UseVisualStyleBackColor = false;
            this.btnShowCache.Click += new System.EventHandler(this.btnShowCache_Click);
            // 
            // chkTopMost
            // 
            resources.ApplyResources(this.chkTopMost, "chkTopMost");
            this.chkTopMost.ForeColor = System.Drawing.Color.Orange;
            this.chkTopMost.Name = "chkTopMost";
            this.chkTopMost.UseVisualStyleBackColor = true;
            this.chkTopMost.Click += new System.EventHandler(this.chkTopMost_Click);
            // 
            // libView
            // 
            this.libView.BackColor = System.Drawing.Color.DarkSlateGray;
            this.libView.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.libView, "libView");
            this.libView.ForeColor = System.Drawing.Color.DarkOrange;
            this.libView.FormattingEnabled = true;
            this.libView.Name = "libView";
            this.libView.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.libView_DrawItem);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frm_Log
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.libView);
            this.Controls.Add(this.chkTopMost);
            this.Controls.Add(this.btnShowCache);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.btnOpenLog);
            this.MaximizeBox = false;
            this.Name = "frm_Log";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Log_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.frm_Log_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnShowCache;
        private System.Windows.Forms.CheckBox chkTopMost;
        private System.Windows.Forms.ListBox libView;
        private System.Windows.Forms.Timer timer1;
    }
}