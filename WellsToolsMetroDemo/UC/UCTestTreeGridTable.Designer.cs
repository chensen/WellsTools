﻿namespace WellsToolsMetroDemo.UC
{
    partial class UCTestTreeGridTable
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ucDataGridView1 = new Wells.WellsMetroControl.Controls.UCDataGridView();
            this.SuspendLayout();
            // 
            // ucDataGridView1
            // 
            this.ucDataGridView1.BackColor = System.Drawing.Color.White;
            this.ucDataGridView1.Columns = null;
            this.ucDataGridView1.DataSource = null;
            this.ucDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDataGridView1.HeadFont = new System.Drawing.Font("微软雅黑", 12F);
            this.ucDataGridView1.HeadHeight = 40;
            this.ucDataGridView1.HeadPadingLeft = 24;
            this.ucDataGridView1.HeadTextColor = System.Drawing.Color.Black;
            this.ucDataGridView1.IsShowCheckBox = false;
            this.ucDataGridView1.IsShowHead = true;
            this.ucDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.ucDataGridView1.Name = "ucDataGridView1";
            this.ucDataGridView1.RowHeight = 41;
            this.ucDataGridView1.RowType = typeof(Wells.WellsMetroControl.Controls.UCDataGridViewRow);
            this.ucDataGridView1.Size = new System.Drawing.Size(825, 674);
            this.ucDataGridView1.TabIndex = 5;
            // 
            // UCTestTreeGridTable
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ucDataGridView1);
            this.Name = "UCTestTreeGridTable";
            this.Size = new System.Drawing.Size(825, 674);
            this.Load += new System.EventHandler(this.UCTestTreeGridTable_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Wells.WellsMetroControl.Controls.UCDataGridView ucDataGridView1;
    }
}
