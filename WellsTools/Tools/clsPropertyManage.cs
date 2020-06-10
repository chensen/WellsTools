using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace Wells.Tools
{
    [Serializable]
    /// <summary>
    /// 属性表管理类
    /// </summary>
    public class clsPropertyManage : CollectionBase, ICustomTypeDescriptor
    {
        //private System.Windows.Forms.PropertyGrid _propertyGrid;

        //public System.Windows.Forms.PropertyGrid propertyGrid
        //{
        //    get
        //    {
        //        return _propertyGrid;
        //    }
        //    set
        //    {
        //        _propertyGrid = value;
        //    }
        //}

        [NonSerialized]
        public System.Windows.Forms.PropertyGrid propertyGrid = null;


        /// <summary>
        /// 添加一个属性表
        /// </summary>
        /// <param name="value"></param>
        public void Add(Property value)
        {
            #region ***** 添加一个属性表 *****

            int flag = -1;
            if (value != null)
            {
                if (base.List.Count > 0)
                {
                    IList<Property> mList = new List<Property>();
                    for (int i = 0; i < base.List.Count; i++)
                    {
                        Property p = base.List[i] as Property;
                        if (value.Name == p.Name)
                        {
                            flag = i;
                        }
                        mList.Add(p);
                    }
                    if (flag == -1)
                    {
                        mList.Add(value);
                    }
                    base.List.Clear();
                    foreach (Property p in mList)
                    {
                        base.List.Add(p);
                    }
                }
                else
                {
                    base.List.Add(value);
                }
            }

            #endregion
        }

        /// <summary>
        /// 移除一个属性表
        /// </summary>
        /// <param name="value"></param>
        public void Remove(Property value)
        {
            #region ***** 移除一个属性表 *****

            if (value != null && base.List.Count > 0)
            {
                base.List.Remove(value);
            }

            #endregion
        }

        /// <summary>
        /// 设置、获取属性表管理器指定索引处属性表
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Property this[int index]
        {
            #region ***** 设置、获取属性表管理器指定索引处属性表 *****

            get
            {
                return (Property)base.List[index];
            }
            set
            {
                base.List[index] = (Property)value;
            }

            #endregion
        }

        public Property this[string name]
        {
            get
            {
                foreach (var obj in base.List)
                {
                    if (((Property)obj).Name == name)
                        return (Property)obj;
                }
                return null;
            }
            set
            {
                Add(value);
            }
        }
        
        #region ***** ICustomTypeDescriptor 成员  *****

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptor[] newProps = new PropertyDescriptor[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                Property prop = (Property)this[i];
                newProps[i] = new CustomPropertyDescriptor(ref prop, attributes);
            }
            return new PropertyDescriptorCollection(newProps);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }
    
    [Serializable]
    /// <summary>
    /// 属性类
    /// </summary>
    public class Property
    {
        private string _name = string.Empty;
        private object _value = null;
        private bool _readonly = false;
        private bool _visible = true;
        private string _category = string.Empty;
        //TypeConverter _converter = null;
        object _editor = null;
        private string _displayname = string.Empty;
        private string _description = string.Empty;

        //visiable属性没用，不知道为啥

        public Property(string sName, object sValue, bool bReadOnly = false)
        {
            this._name = sName;
            this._value = sValue;
            this._readonly = bReadOnly;
        }
        
        /// <summary>
        /// 属性名，作为主键，不能重名
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// 属性显示名称
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _displayname;
            }
            set
            {
                _displayname = value;
            }
        }

        /// <summary>
        /// 属性表说明符
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        [NonSerialized]
        /// <summary>
        /// 类型转换器，我们在制作下拉列表时需要用到
        /// </summary>
        public TypeConverter Converter = null;
        //{
        //    get
        //    {
        //        return _converter;
        //    }
        //    set
        //    {
        //        _converter = value;
        //    }
        //}

        /// <summary>
        /// 属性所属类别
        /// </summary>
        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
            }
        }

        /// <summary>
        /// 属性值
        /// </summary>
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// 只读属性
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return _readonly;
            }
            set
            {
                _readonly = value;
            }
        }

        /// <summary>
        /// 可见属性
        /// </summary>
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
            }
        }

        /// <summary>
        /// 属性编辑器
        /// </summary>
        public virtual object Editor
        {
            get
            {
                return _editor;
            }
            set
            {
                _editor = value;
            }
        }
    }

    /// <summary>
    /// 自定义属性表类描述器
    /// </summary>
    public class CustomPropertyDescriptor : PropertyDescriptor
    {
        /// <summary>
        /// 绑定属性表
        /// </summary>
        Property m_Property;

        public CustomPropertyDescriptor(ref Property myProperty, Attribute[] attrs)
            : base(myProperty.Name, attrs)
        {
            m_Property = myProperty;
        }

        #region ***** PropertyDescriptor 重写方法  *****

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get
            {
                return null;
            }
        }

        public override object GetValue(object component)
        {
            return m_Property.Value;
        }

        public override string Description
        {
            get
            {
                return m_Property.Description;
            }
        }

        public override string Category
        {
            get
            {
                return m_Property.Category;
            }
        }

        public override string DisplayName
        {
            get
            {
                return m_Property.DisplayName != "" ? m_Property.DisplayName : m_Property.Name;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return m_Property.ReadOnly;
            }
        }

        public override bool IsBrowsable
        {
            get
            {
                return m_Property.Visible;
            }
        }

        public override void ResetValue(object component)
        {
            //Have to implement  
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override void SetValue(object component, object value)
        {
            m_Property.Value = value;
        }

        public override TypeConverter Converter
        {
            get
            {
                return m_Property.Converter;
            }
        }

        public override Type PropertyType
        {
            get { return m_Property.Value.GetType(); }
        }

        public override object GetEditor(Type editorBaseType)
        {
            return m_Property.Editor == null ? base.GetEditor(editorBaseType) : m_Property.Editor;
        }

        #endregion
    }
    
    /// <summary>
    /// 下拉框类型转换器
    /// </summary>
    public class DropDownListConverter : StringConverter
    {
        object[] m_Objects;

        public DropDownListConverter(object[] objects)
        {
            m_Objects = objects;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_Objects);//我们可以直接在内部定义一个数组，但并不建议这样做，这样对于下拉框的灵活性有很大影响  
        }
    }

    /// <summary>
    /// 文件路径选择
    /// </summary>
    public class PropertyGridFileItem : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (edSvc != null)
            {
                // 可以打开任何特定的对话框
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.AddExtension = false;
                if (dialog.ShowDialog().Equals(DialogResult.OK))
                {
                    return dialog.FileName;
                }
            }

            return value;
        }
    }
}
