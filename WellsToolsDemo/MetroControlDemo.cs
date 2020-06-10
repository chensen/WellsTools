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
            propertyGrid1.SelectedObject = wellsMetroComboBox1;
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
            int index = comboBox1.SelectedIndex;
            tagPartType type1 = (tagPartType)Enum.Parse(typeof(tagPartType), comboBox1.SelectedItem.ToString());
            return;
            Wells.WellsMetroControl.Forms.FrmAnchorTips frmTips = Wells.WellsMetroControl.Forms.FrmAnchorTips.ShowTips(new Rectangle(100, 100, 50, 20), "ABC", Wells.WellsMetroControl.Forms.AnchorTipsLocation.TOP, Color.Lime, autoCloseTime: -1);
        }

        private void uchScrollbarEx1_ValueChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Write("value changed!\n");
        }
    }
}
