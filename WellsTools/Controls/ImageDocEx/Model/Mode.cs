using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wells.Controls.ImageDocEx
{
    public enum ImageMode
    {
        /// <summary>
        /// 原始比率，不进行拉伸
        /// </summary>
        Origin,

        /// <summary>
        /// 填充屏幕，图片被拉伸处理
        /// </summary>
        Stretch,
    }

    public enum TrackShape
    {
        /// <summary>
        /// 直线
        /// </summary>
        Line = 0,

        /// <summary>
        /// 虚线矩形，显示轨迹
        /// </summary>
        Rect1=1,

        /// <summary>
        /// 实线矩形，显示新增元件轮廓
        /// </summary>
        Rect2=2,
    }
    
    public enum StatusMode
    {
        /// <summary>
        /// 默认值，标准模式
        /// </summary>
        Normal=0,

        /// <summary>
        /// 编辑模式，可以选择元件，拖动元件，改变大小
        /// </summary>
        Edit=1,
    }

    public enum ToolMode
    {
        /// <summary>
        /// 默认值，无操作
        /// </summary>
        None=0,

        /// <summary>
        /// 增加元件，一次性生效，添加成功后自动清除状态
        /// </summary>
        Add = 1,

        /// <summary>
        /// 复制元件，一次生效，添加成功后自动清除状态
        /// </summary>
        Copy = 2,

        /// <summary>
        /// 测量模式，需要手动切换
        /// </summary>
        Measure = 3,
    }
}
