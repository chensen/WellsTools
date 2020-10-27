using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using hvppleDotNet;

namespace WellsToolsDemo
{
    [Serializable]
    public class clsProject
    {
        public HObject region;

        private static clsProject instance;
        public static clsProject Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new clsProject();

                }
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        private clsProject()
        {
            HOperatorSet.GenEmptyRegion(out region);
        }

        public static void writeProject(string path, bool bDual = false)
        {
            #region ***** 保存程式 *****

            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, instance);
                stream.Close();
            }
            catch (Exception exc)
            {
                Wells.FrmType.frm_Log.Log("保存程序失败：" + exc.Message, 2, 1);
            }

            #endregion
        }

        public static bool readProject(string path, bool bDual = false)
        {
            #region ***** 打开程式 *****

            bool ret = false;

            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                instance = (clsProject)formatter.Deserialize(stream);
                stream.Close();
                
                ret = true;
            }
            catch (Exception exc)
            {
                Wells.FrmType.frm_Log.Log("打开程序失败：" + exc.Message, 2, 1);
                ret = false;
            }

            return ret;

            #endregion
        }
    }

    public class clsPublic
    {
        /*函数功能：从一堆Rect中获得多个相邻的矩形块，并构建多个大矩形*/
        public static List<Rectangle> getRegionFromRects(List<Rectangle> rects)
        {
            //存放最终结果
            List<Rectangle> nRect = new List<Rectangle>();
            //cout << "cos相似性算法检测出的框个数=" << rects .size()<< endl;	
            while (rects.Count > 0)
            {
                //临时存放
                List<Rectangle> temp = new List<Rectangle>();
                //获得vector中的最后一个元素
                Rectangle last = rects[rects.Count - 1];
                //删除最后一个元素
                rects.RemoveAt(rects.Count - 1);
                //存入temp中
                temp.Add(last);

                //声明一个队列
                Queue<Rectangle> q = new Queue<Rectangle>();
                //此时temp中只有一个元素，入队
                q.Enqueue(temp[0]);

                //队列不空则出队
                while (q.Count > 0)
                {
                    //记录队头元素
                    Rectangle rect_head = q.Dequeue();
                    //根据rect_head在剩下的rects中找重叠的块,并从rects中删除这些块
                    List<Rectangle> overlapRegion_rects = getOverlapRegion(rect_head, rects);
                    //cout << "overlapRegion_rects大小为：" << overlapRegion_rects.size() << endl;//找到是该块的直接邻居，0,1,2,3,4，等值
                    //如果存在，就入队
                    if (overlapRegion_rects.Count > 0)
                    {
                        for (int i = 0; i < overlapRegion_rects.Count; i++)
                        {
                            //将这些重叠的块保存到temp中，每个大循环temp中就存放了一堆相邻的块，最后取这些块的最小x、y，最大x、y就获得一个框住这些块的大框
                            temp.Add(overlapRegion_rects[i]);
                            //重新入队
                            q.Enqueue(overlapRegion_rects[i]);
                        }
                    }
                }//while (q.size())
                 //全部出队完，temp中就保存了一堆彼此相邻的块，比较所有的Rect的x和y的最小值，最大值。
                int min_x = 100000, max_x = 0, min_y = 100000, max_y = 0;
                int width = 0, height = 0;//这里的16是小块的边长
                for (int i = 0; i < temp.Count; i++)
                {
                    //遍历Rect的x、y值
                    int x = temp[i].X;
                    int y = temp[i].Y;

                    if (x < min_x)
                        min_x = x;

                    if (x > max_x)
                    {
                        max_x = x;
                        //记住最大值的width
                        width = temp[i].Width;
                    }
                    if (y < min_y)
                        min_y = y;

                    if (y > max_y)
                    {
                        //记住最大值的height
                        height = temp[i].Height;
                        max_y = y;
                    }
                }
                //大框rect
                Rectangle big = new Rectangle();
                big.X = min_x;
                big.Y = min_y;
                big.Width = max_x + width - min_x;
                big.Height = max_y + height - min_y;
                //将最终结果存起来
                nRect.Add(big);
            }//while (rects.size())

            return nRect;
        }

        //判断矩形相邻代码如下
        //判断矩形重叠
        public static bool isOverlap(Rectangle rc1, Rectangle rc2)
        {
            int x1 = (rc1.Left + rc1.Right) / 2;
            int y1 = (rc1.Top + rc1.Bottom) / 2;
            int x2 = (rc2.Left + rc2.Right) / 2;
            int y2 = (rc2.Top + rc2.Bottom) / 2;
            double dis = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
            return dis < 1000;

            if (rc1.X + rc1.Width >= rc2.X && rc2.X + rc2.Width >= rc1.X && rc1.Y + rc1.Height >= rc2.Y && rc2.Y + rc2.Height >= rc1.Y)
                return true;
            else
                return false;
        }

        //根据当前的rect在rects中找到与该块重叠的块。
        public static List<Rectangle> getOverlapRegion(Rectangle r, List<Rectangle> rects)
        {
            List<Rectangle> res = new List<Rectangle>();

            for (int i = rects.Count - 1; i >= 0; i--)
            {
                if (isOverlap(r, rects[i]))
                {
                    //如果有重叠，就存入
                    res.Add(rects[i]);
                    //获取第i个迭代器
                    rects.RemoveAt(i);
                }
            }
            return res;
        }
    }
}
