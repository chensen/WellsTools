namespace WellsToolsDemo
{
    partial class ImageDocDemo
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.imageDoc2 = new Wells.Controls.ImageDoc.ImageDoc();
            this.imageDoc1 = new Wells.Controls.ImageDoc.ImageDoc();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(127, 720);
            this.panel1.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "加载矩形";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "加载图片";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // imageDoc2
            // 
            this.imageDoc2.BackColor = System.Drawing.Color.Transparent;
            this.imageDoc2.EditMode = true;
            this.imageDoc2.Font = new System.Drawing.Font("思源黑体 CN Normal", 9F);
            this.imageDoc2.Image = null;
            this.imageDoc2.ImageMode = Wells.Controls.ImageDoc.ImageMode.Origin;
            this.imageDoc2.Location = new System.Drawing.Point(130, 463);
            this.imageDoc2.Margin = new System.Windows.Forms.Padding(0);
            this.imageDoc2.Name = "imageDoc2";
            this.imageDoc2.Padding = new System.Windows.Forms.Padding(3);
            this.imageDoc2.Size = new System.Drawing.Size(265, 254);
            this.imageDoc2.StaticWnd = false;
            this.imageDoc2.TabIndex = 4;
            // 
            // imageDoc1
            // 
            this.imageDoc1.BackColor = System.Drawing.Color.Transparent;
            this.imageDoc1.EditMode = true;
            this.imageDoc1.Font = new System.Drawing.Font("思源黑体 CN Normal", 9F);
            this.imageDoc1.Image = null;
            this.imageDoc1.ImageMode = Wells.Controls.ImageDoc.ImageMode.Origin;
            this.imageDoc1.Location = new System.Drawing.Point(127, 0);
            this.imageDoc1.Margin = new System.Windows.Forms.Padding(0);
            this.imageDoc1.Name = "imageDoc1";
            this.imageDoc1.Padding = new System.Windows.Forms.Padding(3);
            this.imageDoc1.Size = new System.Drawing.Size(731, 463);
            this.imageDoc1.StaticWnd = false;
            this.imageDoc1.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 71);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(91, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "显示初始矩形";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(13, 100);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(91, 23);
            this.button4.TabIndex = 2;
            this.button4.Text = "计算矩形";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(13, 129);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(91, 23);
            this.button5.TabIndex = 2;
            this.button5.Text = "显示计算矩形";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(13, 159);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(91, 23);
            this.button6.TabIndex = 3;
            this.button6.Text = "同时显示";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // ImageDocDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(861, 720);
            this.Controls.Add(this.imageDoc2);
            this.Controls.Add(this.imageDoc1);
            this.Controls.Add(this.panel1);
            this.Name = "ImageDocDemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Wells.Controls.ImageDoc.ImageDoc imageDoc1;
        private System.Windows.Forms.Panel panel1;
        private Wells.Controls.ImageDoc.ImageDoc imageDoc2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button6;
    }
}