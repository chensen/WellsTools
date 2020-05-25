namespace WellsToolsDemo
{
    partial class ImageDocExDemo
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
            this.button3 = new System.Windows.Forms.Button();
            this.imageDocEx1 = new Wells.Controls.ImageDocEx.ImageDocEx();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(114, 450);
            this.panel1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 43);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "添加roi";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "加载整版图";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 73);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "显示一张图";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // imageDocEx1
            // 
            this.imageDocEx1.BackColor = System.Drawing.Color.Transparent;
            this.imageDocEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageDocEx1.ImageHeight = 2048;
            this.imageDocEx1.ImageMode = Wells.Controls.ImageDocEx.ImageMode.Origin;
            this.imageDocEx1.ImageWidth = 2448;
            this.imageDocEx1.Location = new System.Drawing.Point(114, 0);
            this.imageDocEx1.Margin = new System.Windows.Forms.Padding(0);
            this.imageDocEx1.Name = "imageDocEx1";
            this.imageDocEx1.Padding = new System.Windows.Forms.Padding(3);
            this.imageDocEx1.Size = new System.Drawing.Size(686, 450);
            this.imageDocEx1.StaticWnd = false;
            this.imageDocEx1.TabIndex = 0;
            // 
            // ImageDocExDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.imageDocEx1);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "ImageDocExDemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImageDocExDemo";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ImageDocExDemo_KeyPress);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Wells.Controls.ImageDocEx.ImageDocEx imageDocEx1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}