using hvppleDotNet;
/*********************************************************************************************************
 * 
 *    说明：
 * 
 *    halcon图像显示控件的再次封装 
 *   20180621
 *       1.对于图像显示及其他操作全部封装到c++代码中避免c#对于hobject对象释放导致显示异常问题
 *       2.c++ cli代理对于其他自定义算法也可以按照此模式添加
 *       3.roi操作参考的是halcon官方实例
 *       4.c++代码用到了qt5
 *       5.开发环境为vs2015+halcon13+qt5.9.1
 *       
 *   作者:林玉刚   有任何疑问或建议请联系 linyugang@foxmail.com
 * 
 *********************************************************************************************************/


namespace Wells.Controls.ImageDocEx
{
    public class HWndMessage
    {

        public string message;
        public int size = 16;
        public int row;
        public int colunm;
        public string color = "green";
        public double showSize = 16;
        public string coordSystem = "image";
        
        public HWndMessage(string message, int row, int colunm, int size, string color, string coord)
        {
            this.message = message;
            this.size = size;
            this.row = row;
            this.colunm = colunm;
            this.color = color;
            this.showSize = size;
            this.coordSystem = coord;
        }

        public HWndMessage(string message, int row, int colunm)
        {
            this.message = message;
            this.row = row;
            this.colunm = colunm;
        }

        public double changeDisplayFontSize(HTuple Window, double zoom, double sizeOld)
        {
            double currentSize = size * zoom;
            if (currentSize != sizeOld)
            {
                class_Hvpple.setDisplayFont(Window, currentSize, "serif", "true", "false");
            }
            showSize = currentSize;
            return currentSize;
        }

        public void DispMessage(HTuple Window, string coordSystem)
        {
            string[] msg = message.Split('#');

            for (int igg = 0; igg < msg.Length; igg++)
            {
                class_Hvpple.dispMessage(Window, msg[igg], coordSystem, row + igg * (showSize + 2), colunm, color, "false");
            }
        }

        public void DispMessage(HTuple Window, string coordSystem, double zoom)
        {
            class_Hvpple.setDisplayFont(Window, size * zoom, "serif", "true", "false");

            string[] msg = message.Split('#');

            for (int igg = 0; igg < msg.Length; igg++)
            {
                class_Hvpple.dispMessage(Window, msg[igg], coordSystem, row + igg * (showSize + 2), colunm, color, "false");
            }
        }
    }
}
