namespace WellsToolsDemo
{
    partial class PropertyManageDemo
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
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.BackColor = System.Drawing.Color.AliceBlue;
            this.propertyGrid1.CategoryForeColor = System.Drawing.Color.Blue;
            this.propertyGrid1.CommandsBackColor = System.Drawing.Color.RoyalBlue;
            this.propertyGrid1.CommandsForeColor = System.Drawing.SystemColors.Control;
            this.propertyGrid1.HelpBackColor = System.Drawing.Color.DimGray;
            this.propertyGrid1.HelpForeColor = System.Drawing.Color.White;
            this.propertyGrid1.LineColor = System.Drawing.Color.SlateGray;
            this.propertyGrid1.Location = new System.Drawing.Point(13, 30);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid1.Size = new System.Drawing.Size(194, 315);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.Highlight;
            this.propertyGrid1.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.propertyGrid1_SelectedGridItemChanged);
            this.propertyGrid1.Validated += new System.EventHandler(this.propertyGrid1_Validated);
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Location = new System.Drawing.Point(327, 12);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(211, 426);
            this.propertyGrid2.TabIndex = 1;
            // 
            // PropertyManageDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.propertyGrid2);
            this.Controls.Add(this.propertyGrid1);
            this.Name = "PropertyManageDemo";
            this.Text = "PropertyManageDemo";
            this.Load += new System.EventHandler(this.PropertyManageDemo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
    }
}