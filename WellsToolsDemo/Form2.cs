using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WellsToolsDemo
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void SetNotifyInfo(int percent, string message)
        {
            this.label1.Text = message;
            this.progressBar1.Value = percent;
        }

        private void skinTreeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            if (e.Node == null)
                return;
            
            treeView1.SelectedNode = e.Node;

            if (e.Node.Level == 0)
                contextMenuStrip1.Show(treeView1, e.X, e.Y);
        }

        private void 功能1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.LabelEdit = true;
        }

        private void 功能2ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {

        }
    }
}
