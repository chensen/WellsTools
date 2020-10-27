namespace WellsToolsDemo
{
    partial class Form2
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("节点3");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("节点4");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("节点5");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("节点0", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("节点6");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("节点7");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("节点1", new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6});
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("节点8");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("节点9");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("节点2", new System.Windows.Forms.TreeNode[] {
            treeNode8,
            treeNode9});
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.小功能1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.小功能2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.功能1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.功能2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.功能3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.小功能1ToolStripMenuItem,
            this.小功能2ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(120, 48);
            // 
            // 小功能1ToolStripMenuItem
            // 
            this.小功能1ToolStripMenuItem.Name = "小功能1ToolStripMenuItem";
            this.小功能1ToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.小功能1ToolStripMenuItem.Text = "小功能1";
            // 
            // 小功能2ToolStripMenuItem
            // 
            this.小功能2ToolStripMenuItem.Name = "小功能2ToolStripMenuItem";
            this.小功能2ToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.小功能2ToolStripMenuItem.Text = "小功能2";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.功能1ToolStripMenuItem,
            this.功能2ToolStripMenuItem,
            this.功能3ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 70);
            // 
            // 功能1ToolStripMenuItem
            // 
            this.功能1ToolStripMenuItem.Name = "功能1ToolStripMenuItem";
            this.功能1ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.功能1ToolStripMenuItem.Text = "重命名";
            this.功能1ToolStripMenuItem.Click += new System.EventHandler(this.功能1ToolStripMenuItem_Click);
            // 
            // 功能2ToolStripMenuItem
            // 
            this.功能2ToolStripMenuItem.Name = "功能2ToolStripMenuItem";
            this.功能2ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.功能2ToolStripMenuItem.Text = "删除";
            this.功能2ToolStripMenuItem.Click += new System.EventHandler(this.功能2ToolStripMenuItem_Click);
            // 
            // 功能3ToolStripMenuItem
            // 
            this.功能3ToolStripMenuItem.Name = "功能3ToolStripMenuItem";
            this.功能3ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.功能3ToolStripMenuItem.Text = "功能3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(59, 9);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(265, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.FullRowSelect = true;
            this.treeView1.Location = new System.Drawing.Point(24, 57);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "节点3";
            treeNode1.Text = "节点3";
            treeNode2.Name = "节点4";
            treeNode2.Text = "节点4";
            treeNode3.Name = "节点5";
            treeNode3.Text = "节点5";
            treeNode4.Name = "节点0";
            treeNode4.Text = "节点0";
            treeNode5.Name = "节点6";
            treeNode5.Text = "节点6";
            treeNode6.Name = "节点7";
            treeNode6.Text = "节点7";
            treeNode7.Name = "节点1";
            treeNode7.Text = "节点1";
            treeNode8.Name = "节点8";
            treeNode8.Text = "节点8";
            treeNode9.Name = "节点9";
            treeNode9.Text = "节点9";
            treeNode10.Name = "节点2";
            treeNode10.Text = "节点2";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode7,
            treeNode10});
            this.treeView1.ShowLines = false;
            this.treeView1.Size = new System.Drawing.Size(252, 445);
            this.treeView1.TabIndex = 2;
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 544);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 小功能1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 小功能2ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 功能1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 功能2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 功能3ToolStripMenuItem;
    }
}