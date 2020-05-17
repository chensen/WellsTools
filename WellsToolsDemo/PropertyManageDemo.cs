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
    public partial class PropertyManageDemo : Form
    {
        public PropertyManageDemo()
        {
            InitializeComponent();
        }

        private void PropertyManageDemo_Load(object sender, EventArgs e)
        {
            Wells.Tools.clsPropertyManage pmc = new Wells.Tools.clsPropertyManage();

            Wells.Tools.Property pp = new Wells.Tools.Property("Name", "陈森");
            pp.Category = "我的属性1";
            pp.DisplayName = "姓名";
            pp.Description = "姓名";
            pmc.Add(pp);

            pp = new Wells.Tools.Property("Sex", "男");
            pp.Category = "我的属性1";
            pp.DisplayName = "性别";
            pp.Description = "性别";
            pp.Converter = new Wells.Tools.DropDownListConverter(new string[] { "男", "女" });
            pmc.Add(pp);

            pp = new Wells.Tools.Property("Age", 30);
            pp.Category = "我的属性2";
            pp.DisplayName = "年龄";
            pp.Description = "年龄";
            pmc.Add(pp);

            pp = new Wells.Tools.Property("Path", "");
            pp.Category = "我的属性2";
            pp.DisplayName = "档案路径";
            pp.Description = "档案路径";
            pp.Editor = new Wells.Tools.PropertyGridFileItem();
            pmc.Add(pp);

            pp = new Wells.Tools.Property("Version", "1.0.0.1");
            pp.Category = "我的属性3";
            pp.DisplayName = "版本号";
            pp.Description = "版本号";
            pmc.Add(pp);

            propertyGrid3.SelectedObject = pmc;
            //propertyGrid3.


            propertyGrid2.SelectedObject = propertyGrid3;
        }

        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("item changed");
        }

        private void propertyGrid1_Validated(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("lose focus");
        }

        public class myp:PropertyGrid
        {
            public myp()
            {
                //base.disabled
            }
        }
    }
}
