using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Wells.WellsMetroControl.Controls
{
    /// <summary>
    /// Class DataGridViewColumnEntity.
    /// </summary>
    public class DataGridViewColumnEntity
    {
        /// <summary>
        /// Gets or sets the head text.
        /// </summary>
        /// <value>The head text.</value>
        public string HeadText { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the type of the width.
        /// </summary>
        /// <value>The type of the width.</value>
        public System.Windows.Forms.SizeType WidthType { get; set; }
        /// <summary>
        /// Gets or sets the data field.
        /// </summary>
        /// <value>The data field.</value>
        public string DataField { get; set; }
        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        public Func<object, string> Format { get; set; }
        /// <summary>
        /// The text align
        /// </summary>
        private ContentAlignment _TextAlign = ContentAlignment.MiddleCenter;
        /// <summary>
        /// Gets or sets the text align.
        /// </summary>
        /// <value>The text align.</value>
        public ContentAlignment TextAlign { get { return _TextAlign; } set { _TextAlign = value; } }
        /// <summary>
        /// 自定义的单元格控件，一个实现IDataGridViewCustomCell的Control
        /// </summary>
        /// <value>The custom cell.</value>
        private Type customCellType = null;
        public Type CustomCellType
        {
            get
            {
                return customCellType;
            }
            set
            {
                if (!typeof(IDataGridViewCustomCell).IsAssignableFrom(value) || !value.IsSubclassOf(typeof(System.Windows.Forms.Control)))
                    throw new Exception("行控件没有实现IDataGridViewCustomCell接口");
                customCellType = value;
            }
        }
    }
}
