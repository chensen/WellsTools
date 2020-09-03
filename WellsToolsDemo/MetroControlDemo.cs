using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WellsToolsDemo
{
    //[ComVisible(true)]
    //[Editor("System.Windows.Forms.Design.ShortcutKeysEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    [Flags]
    //[TypeConverter(typeof(KeysConverter))]
    public enum tagPartType
    {
        None = 0x1,
        Mark = 0x2,
        BadMark = 0x4,
        Regist = 0x8,
        AI = 0x10,
    }

    public partial class MetroControlDemo : Form
    {
        public MetroControlDemo()
        {
            InitializeComponent();
            List<KeyValuePair<string, string>> str = new List<KeyValuePair<string, string>>();
            str.Add(new KeyValuePair<string, string>("0", "1"));
            str.Add(new KeyValuePair<string, string>("1", "2"));
            propertyGrid1.SelectedObject = skinComboBox1;
            ucCombox1.Source = str;
            //uchScrollbarEx1.DataBindings.Add("ValueLow", textBoxEx1, "Text", true, DataSourceUpdateMode.OnValidation);
            //uchScrollbarEx1.ValueLow = 500.5M;
            textBoxEx1.Text = "125";
            textBoxEx2.Text = "255";
            comboBox1.Items.Clear();
            comboBox1.DataSource = System.Enum.GetNames(typeof(tagPartType));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Wells.FrmType.frm_Log.ShowDlg(true);
            return;
            int index = comboBox1.SelectedIndex;
            tagPartType type1 = (tagPartType)Enum.Parse(typeof(tagPartType), comboBox1.SelectedItem.ToString());
            wellsMetroComboBox1.SelectedIndex = -1;
            return;
            Wells.WellsMetroControl.Forms.FrmAnchorTips frmTips = Wells.WellsMetroControl.Forms.FrmAnchorTips.ShowTips(new Rectangle(100, 100, 50, 20), "ABC", Wells.WellsMetroControl.Forms.AnchorTipsLocation.TOP, Color.Lime, autoCloseTime: -1);
        }

        private void uchScrollbarEx1_ValueChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Write("value changed!\n");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Wells.FrmType.frm_Log.Log("GTN_SetDoBit出错，错误代码：-6，错误信息：打开控制器失11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111我的妈", 2);
        }

        private void ucCheckBox1_ClickEvent(object sender, EventArgs e)
        {
            Wells.class_Public.Show(null, ucCheckBox1.Checked.ToString());
        }

        private void textBoxEx3_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            textBoxEx3.Text = e.KeyData.ToString();
            e.IsInputKey = false;
        }

        private void textBoxEx3_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            e.SuppressKeyPress = false;
            textBox1.Text = e.KeyData.ToString() + "---" + e.KeyCode.ToString() + "---" + e.KeyValue.ToString();
        }

        private void textBoxEx3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = true;
        }

        private void textBoxEx3_PreviewKeyDown_1(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = false;
        }
    }
}
