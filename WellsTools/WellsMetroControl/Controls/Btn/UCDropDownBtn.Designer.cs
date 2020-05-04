﻿namespace Wells.WellsMetroControl.Controls
{
    /// <summary>
    /// Class UCDropDownBtn.
    /// Implements the <see cref="Wells.WellsMetroControl.Controls.UCBtnImg" />
    /// </summary>
    /// <seealso cref="Wells.WellsMetroControl.Controls.UCBtnImg" />
    partial class UCDropDownBtn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCDropDownBtn));
            this.SuspendLayout();
            // 
            // lbl
            // 
            this.lbl.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.lbl.ForeColor = System.Drawing.Color.White;
            this.lbl.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl.ImageList = null;
            this.lbl.Text = "自定义按钮";
            this.lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UCDropDownBtn
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BtnFont = new System.Drawing.Font("微软雅黑", 14F);
            this.BtnForeColor = System.Drawing.Color.White;
            this.ForeColor = System.Drawing.Color.White;
            this.Image = ((System.Drawing.Image)(resources.GetObject("$this.Image")));
            this.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UCDropDownBtn";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
