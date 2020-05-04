using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wells.Tools
{
    /// <summary>
    /// 自定义位数的数据类型算法
    /// </summary>
    public class clsWellsInt
    {
        /// <summary>
        /// 数据位数，小于等于30
        /// </summary>
        public static uint _bit = 27;

        /// <summary>
        /// 默认最大值
        /// </summary>
        public static int MaxValue = 134217727;

        /// <summary>
        /// 默认最小值
        /// </summary>
        public static int MinValue = -134217728;

        /// <summary>
        /// 设置自定义数据的位数信息
        /// </summary>
        /// <param name="bit"></param>
        public static void SetWellsIntBit(uint bit)
        {
            _bit = bit;
            MaxValue = (int)(Math.Pow(2, _bit ) - 1);
            MinValue = (int)(Math.Pow(2, _bit ) * (-1));
        }

        /// <summary>
        /// 存储数据值
        /// </summary>
        private int _value;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = Value;
            }
        }

        public clsWellsInt()
        {
            _value = 0;
        }

        /// <summary>
        /// 带参构造函数，传递实际数据
        /// </summary>
        /// <param name="value"></param>
        public clsWellsInt(int value)
        {
            while (value > MaxValue || value < MinValue)
            {
                value += value > 0 ? 2 * MinValue : (-2) * MinValue;//位数必须小于31
            }
            _value = value;
        }
        /// <summary>
        /// 重载加法运行符
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <returns></returns>
        public static clsWellsInt operator +(clsWellsInt w1, clsWellsInt w2)
        {
            int temp = w1.Value + w2.Value;
            if (temp > MaxValue)
            {
                temp = MinValue + temp - MaxValue - 1;
            }
            else if (temp < MinValue)
            {
                temp = MaxValue + temp - MinValue + 1;
            }
            return new clsWellsInt(temp);
        }

        public static clsWellsInt operator +(int w1, clsWellsInt w2)
        {
            return (new clsWellsInt(w1) + w2);
        }

        public static clsWellsInt operator +(clsWellsInt w1, int w2)
        {
            return (new clsWellsInt(w2) + w1);
        }

        /// <summary>
        /// 重载减法运算符
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <returns></returns>
        public static clsWellsInt operator -(clsWellsInt w1, clsWellsInt w2)
        {
            int temp = w1.Value - w2.Value;
            if (temp > MaxValue)
            {
                temp = MinValue + temp - MaxValue - 1;
            }
            else if (temp < MinValue)
            {
                temp = MaxValue + temp - MinValue + 1;
            }
            return new clsWellsInt(temp);
        }

        public static clsWellsInt operator -(int w1, clsWellsInt w2)
        {
            return (new clsWellsInt(w1) - w2);
        }

        public static clsWellsInt operator -(clsWellsInt w1, int w2)
        {
            return (w1 - new clsWellsInt(w2));
        }
    }
}
