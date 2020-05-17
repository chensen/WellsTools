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
}
