using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Wells.Tools
{
    /// <summary>
    /// 队列类（使用固定长度list）(加上线程安全)
    /// </summary>
    /// <typeparam name="T">队列中元素的类型</typeparam>
    public class clsList<T>
    {
        /// <summary>
        /// 固定长度
        /// </summary>
        private int size = 100;

        /// <summary>
        /// 通知的状态机
        /// </summary>
        AutoResetEvent notice = new AutoResetEvent(true);

        /// <summary>
        /// 内部实现list
        /// </summary>
        private List<T> _list = null;
        public clsList(int s)
        {
            size = s > 0 ? s : 100;
            _list = new List<T>();
        }

        /// <summary>
        /// 队列实际长度
        /// </summary>
        public int Count
        {
            get
            {
                Lock();
                int result = _list.Count;
                UnLock();
                return result;
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="data"></param>
        public void Add(T data)
        {
            Lock();
            if (_list.Count >= size)
                _list.RemoveAt(0);
            _list.Add(data);
            UnLock();
        }

        /// <summary>
        /// 获取指定序号数据，不删除数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T Peek(int index)//wells0059
        {
            Lock();
            T[] temp = _list.ToArray();
            UnLock();
            if (temp.Length > index + 1)
                return temp[index];
            else
                return default(T);
        }

        /// <summary>
        /// 是否包含指定数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Contains(T data)
        {
            Lock();
            bool ret = false;
            T[] temp = _list.ToArray();
            foreach (T t in temp)
            {
                if (t.Equals(data))
                {
                    ret = true;
                    break;
                }
            }
            UnLock();
            return ret;
        }

        /// <summary>
        /// 获取当前所有数据的字条串
        /// </summary>
        /// <returns></returns>
        public string ShowItems()
        {
            Lock();
            string ret = string.Empty;
            T[] temp = _list.ToArray();
            if (temp.Length > 0)
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    ret += temp[i].ToString() + ",";
                }
            }
            UnLock();
            return ret;
        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void Clear()
        {
            Lock();
            _list.Clear();
            _list = new List<T>();
            UnLock();
        }

        /// <summary>
        /// 加上锁定状态
        /// </summary>
        private void Lock()
        {
            notice.WaitOne();
        }

        /// <summary>
        /// 去除锁定状态
        /// </summary>
        private void UnLock()
        {
            notice.Set();
        }
    }
}
