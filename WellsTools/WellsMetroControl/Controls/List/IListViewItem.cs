using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wells.WellsMetroControl.Controls
{
    /// <summary>
    /// Interface IListViewItem
    /// </summary>
    public interface IListViewItem
    {
        /// <summary>
        /// 数据源
        /// </summary>
        /// <value>The data source.</value>
        object DataSource { get; set; }
        /// <summary>
        /// 选中项事件
        /// </summary>
        event EventHandler SelectedItemEvent;
        /// <summary>
        /// 选中处理，一般用以更改选中效果
        /// </summary>
        /// <param name="blnSelected">是否选中</param>
        void SetSelected(bool blnSelected);
    }
}
