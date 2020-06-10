using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WellsToolsDemo
{
    public partial class clsSerialize : Form
    {
        public clsObj obj = new clsObj();
        public clsSerialize()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            string content = string.Empty;
            //serialize
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                content = writer.ToString();
            }
            //save to file
            using (StreamWriter stream_writer = new StreamWriter("D:\\test.xml"))
            {
                stream_writer.Write(content);
            }
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (StreamReader reader = new StreamReader("D:\\test.xml"))
            {
                obj = (clsObj) serializer.Deserialize(reader);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0).ToString("0.00") + " MB";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }
    }

    [Serializable]
    public class motion
    {
        public double vel;
        public double acc;
        public double dec;
        public double smooth;
        public double newadd;
        public motion()
        {
            vel = 100;
            acc = 100;
            dec = 100;
            smooth = 100;
            newadd = 100;
        }
    }

    [Serializable]
    public class clsObj
    {
        public List<string> newString;

        public List<string> strParam1;

        public string[] strParam2;

        public int iMarkNum;

        public string strMark;

        [XmlElement(ElementName ="用户名")]
        public List<motion> mo;

        public int newint;

        public bool[] bbb = new bool[100];

        public clsObj()
        {
            newString = new List<string>();newString.Add("1024");
            strParam1 = new List<string>();
            for (int igg = 0; igg < 100; igg++) strParam1.Add("0");
            strParam2 = new string[100];
            for (int igg = 0; igg < 100; igg++) strParam2[igg] = "0";
            iMarkNum = 100;
            strMark = "Mark";
            mo = new List<motion>();
            mo.Add(new motion()); mo.Add(new motion()); mo.Add(new motion()); mo.Add(new motion()); mo.Add(new motion()); mo.Add(new motion()); mo.Add(new motion()); mo.Add(new motion()); mo.Add(new motion()); mo.Add(new motion());
            newint = 1000;
        }
    }
}
