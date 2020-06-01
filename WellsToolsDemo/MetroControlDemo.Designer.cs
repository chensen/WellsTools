namespace WellsToolsDemo
{
    partial class MetroControlDemo
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.textBoxEx1 = new Wells.WellsMetroControl.Controls.TextBoxEx();
            this.uchScrollbarEx1 = new Wells.WellsMetroControl.Controls.UCHScrollbarEx();
            this.textBoxEx2 = new Wells.WellsMetroControl.Controls.TextBoxEx();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(453, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(516, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(474, 103);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Left;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(295, 450);
            this.propertyGrid1.TabIndex = 4;
            // 
            // textBoxEx1
            // 
            this.textBoxEx1.DecLength = 0;
            this.textBoxEx1.InputType = Wells.WellsMetroControl.TextInputType.Number;
            this.textBoxEx1.Location = new System.Drawing.Point(355, 150);
            this.textBoxEx1.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.textBoxEx1.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.textBoxEx1.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.textBoxEx1.Name = "textBoxEx1";
            this.textBoxEx1.OldText = null;
            this.textBoxEx1.PromptColor = System.Drawing.Color.Gray;
            this.textBoxEx1.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBoxEx1.PromptText = "";
            this.textBoxEx1.RegexPattern = "";
            this.textBoxEx1.Size = new System.Drawing.Size(100, 21);
            this.textBoxEx1.TabIndex = 6;
            this.textBoxEx1.UCHNum = 0;
            this.textBoxEx1.UcHScrollbarEx = this.uchScrollbarEx1;
            // 
            // uchScrollbarEx1
            // 
            this.uchScrollbarEx1.Aratio = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.uchScrollbarEx1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uchScrollbarEx1.ConerRadius = 2;
            this.uchScrollbarEx1.DecLength = 0;
            this.uchScrollbarEx1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.uchScrollbarEx1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.uchScrollbarEx1.HighThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(255)))), ((int)(((byte)(60)))));
            this.uchScrollbarEx1.IsRadius = true;
            this.uchScrollbarEx1.IsShowDouble = true;
            this.uchScrollbarEx1.IsShowRect = false;
            this.uchScrollbarEx1.IsShowTips = true;
            this.uchScrollbarEx1.Location = new System.Drawing.Point(426, 225);
            this.uchScrollbarEx1.LowThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.uchScrollbarEx1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uchScrollbarEx1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.uchScrollbarEx1.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.uchScrollbarEx1.MinimumSize = new System.Drawing.Size(0, 10);
            this.uchScrollbarEx1.Name = "uchScrollbarEx1";
            this.uchScrollbarEx1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.uchScrollbarEx1.RectWidth = 1;
            this.uchScrollbarEx1.Size = new System.Drawing.Size(150, 78);
            this.uchScrollbarEx1.TabIndex = 5;
            this.uchScrollbarEx1.TipsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.uchScrollbarEx1.TipsForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.uchScrollbarEx1.TipsFormat = null;
            this.uchScrollbarEx1.TxtBoxHigh = this.textBoxEx2;
            this.uchScrollbarEx1.TxtBoxLow = this.textBoxEx1;
            this.uchScrollbarEx1.ValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.uchScrollbarEx1.ValueHigh = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.uchScrollbarEx1.ValueLow = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.uchScrollbarEx1.ValueChanged += new System.EventHandler(this.uchScrollbarEx1_ValueChanged);
            // 
            // textBoxEx2
            // 
            this.textBoxEx2.DecLength = 0;
            this.textBoxEx2.InputType = Wells.WellsMetroControl.TextInputType.Number;
            this.textBoxEx2.Location = new System.Drawing.Point(532, 150);
            this.textBoxEx2.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.textBoxEx2.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.textBoxEx2.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.textBoxEx2.Name = "textBoxEx2";
            this.textBoxEx2.OldText = null;
            this.textBoxEx2.PromptColor = System.Drawing.Color.Gray;
            this.textBoxEx2.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBoxEx2.PromptText = "";
            this.textBoxEx2.RegexPattern = "";
            this.textBoxEx2.Size = new System.Drawing.Size(100, 21);
            this.textBoxEx2.TabIndex = 6;
            this.textBoxEx2.UCHNum = 1;
            this.textBoxEx2.UcHScrollbarEx = this.uchScrollbarEx1;
            // 
            // MetroControlDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxEx2);
            this.Controls.Add(this.textBoxEx1);
            this.Controls.Add(this.uchScrollbarEx1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MetroControlDemo";
            this.Text = "MetroControlDemo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private Wells.WellsMetroControl.Controls.UCHScrollbarEx uchScrollbarEx1;
        private Wells.WellsMetroControl.Controls.TextBoxEx textBoxEx1;
        private Wells.WellsMetroControl.Controls.TextBoxEx textBoxEx2;
    }
}