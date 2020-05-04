namespace Wells.FrmType
{
    partial class frmWaitingBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWaitingBox));
            this.panel1 = new System.Windows.Forms.Panel();
            this.labTimer = new System.Windows.Forms.Label();
            this.labMessage = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBoxCancel = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.labTimer);
            this.panel1.Controls.Add(this.pictureBoxCancel);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.labMessage);
            this.panel1.Name = "panel1";
            // 
            // labTimer
            // 
            resources.ApplyResources(this.labTimer, "labTimer");
            this.labTimer.ForeColor = System.Drawing.Color.Red;
            this.labTimer.Name = "labTimer";
            // 
            // labMessage
            // 
            resources.ApplyResources(this.labMessage, "labMessage");
            this.labMessage.Name = "labMessage";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBoxCancel
            // 
            resources.ApplyResources(this.pictureBoxCancel, "pictureBoxCancel");
            this.pictureBoxCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxCancel.Image = global::Wells.Properties.Resources._return;
            this.pictureBoxCancel.Name = "pictureBoxCancel";
            this.pictureBoxCancel.TabStop = false;
            this.pictureBoxCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Image = global::Wells.Properties.Resources.waiting;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // frmWaitingBox
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleTurquoise;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmWaitingBox";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWaitingBox_FormClosing);
            this.Shown += new System.EventHandler(this.frmWaitingBox_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labMessage;
        private System.Windows.Forms.PictureBox pictureBoxCancel;
        private System.Windows.Forms.Label labTimer;
        private System.Windows.Forms.Timer timer1;
    }
}