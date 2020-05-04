namespace Wells.WellsFramework
{
    partial class WellsMetroMessageBoxControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WellsMetroMessageBoxControl));
            this.panelbody = new System.Windows.Forms.Panel();
            this.tlpBody = new System.Windows.Forms.TableLayoutPanel();
            this.messageLabel = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.metroButton3 = new System.Windows.Forms.Button();
            this.metroButton2 = new System.Windows.Forms.Button();
            this.metroButton1 = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.panelbody.SuspendLayout();
            this.tlpBody.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelbody
            // 
            resources.ApplyResources(this.panelbody, "panelbody");
            this.panelbody.BackColor = System.Drawing.Color.DarkGray;
            this.panelbody.Controls.Add(this.tlpBody);
            this.panelbody.Name = "panelbody";
            // 
            // tlpBody
            // 
            resources.ApplyResources(this.tlpBody, "tlpBody");
            this.tlpBody.Controls.Add(this.messageLabel, 1, 2);
            this.tlpBody.Controls.Add(this.pnlBottom, 1, 3);
            this.tlpBody.Controls.Add(this.titleLabel, 1, 1);
            this.tlpBody.Name = "tlpBody";
            // 
            // messageLabel
            // 
            resources.ApplyResources(this.messageLabel, "messageLabel");
            this.messageLabel.BackColor = System.Drawing.Color.Transparent;
            this.messageLabel.ForeColor = System.Drawing.Color.Yellow;
            this.messageLabel.Name = "messageLabel";
            // 
            // pnlBottom
            // 
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            this.pnlBottom.BackColor = System.Drawing.Color.Transparent;
            this.pnlBottom.Controls.Add(this.metroButton3);
            this.pnlBottom.Controls.Add(this.metroButton2);
            this.pnlBottom.Controls.Add(this.metroButton1);
            this.pnlBottom.Name = "pnlBottom";
            // 
            // metroButton3
            // 
            resources.ApplyResources(this.metroButton3, "metroButton3");
            this.metroButton3.Name = "metroButton3";
            this.metroButton3.UseVisualStyleBackColor = false;
            // 
            // metroButton2
            // 
            resources.ApplyResources(this.metroButton2, "metroButton2");
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.UseVisualStyleBackColor = false;
            // 
            // metroButton1
            // 
            resources.ApplyResources(this.metroButton1, "metroButton1");
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.UseVisualStyleBackColor = false;
            // 
            // titleLabel
            // 
            resources.ApplyResources(this.titleLabel, "titleLabel");
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.ForeColor = System.Drawing.Color.DarkViolet;
            this.titleLabel.Name = "titleLabel";
            // 
            // WellsMetroMessageBoxControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelbody);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WellsMetroMessageBoxControl";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.WellsMetroMessageBoxControl_Load);
            this.panelbody.ResumeLayout(false);
            this.tlpBody.ResumeLayout(false);
            this.tlpBody.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelbody;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.TableLayoutPanel tlpBody;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button metroButton3;
        private System.Windows.Forms.Button metroButton2;
        private System.Windows.Forms.Button metroButton1;
    }
}
