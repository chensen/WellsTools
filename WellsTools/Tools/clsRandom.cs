﻿using System;

namespace Wells.Tools
{
    /// <summary>
    /// BaseRandom
    /// 产生随机数
    /// 
    /// 随机数管理，最大值、最小值可以自己进行设定。
    /// </summary>
    public class clsRandomStatic
    {
        public static int Minimum = 100000;
        public static int Maximal = 999999;
        public static int RandomLength = 6;

        private static string RandomString = "0123456789ABCDEFGHIJKMLNOPQRSTUVWXYZ";
        private static Random Random = new Random(DateTime.Now.Second);

        #region public static string GetRandomString() 产生随机字符
        /// <summary>
        /// 产生随机字符
        /// </summary>
        /// <returns>字符串</returns>
        public static string getRandomString()
        {
            string returnValue = string.Empty;
            for (int i = 0; i < RandomLength; i++)
            {
                int r = Random.Next(0, RandomString.Length - 1);
                returnValue += RandomString[r];
            }
            return returnValue;
        }
        #endregion

        #region public static int GetRandom()
        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <returns>随机数</returns>
        public static int getRandom()
        {
            return Random.Next(Minimum, Maximal);
        }
        #endregion

        #region public static int GetRandom(int minimum, int maximal)
        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <param name="minimum">最小值</param>
        /// <param name="maximal">最大值</param>
        /// <returns>随机数</returns>
        public static int getRandom(int minimum, int maximal)
        {
            return Random.Next(minimum, maximal);
        }
        #endregion
    }

    /// <summary>
    /// 使用Random类生成伪随机数
    /// </summary>
    public class clsRandom
    {
        //随机数对象
        private Random _random;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public clsRandom()
        {
            //为随机数对象赋值
            this._random = new Random();
        }
        #endregion

        #region 生成一个指定范围的随机整数
        /// <summary>
        /// 生成一个指定范围的随机整数，该随机数范围包括最小值，但不包括最大值
        /// </summary>
        /// <param name="minNum">最小值</param>
        /// <param name="maxNum">最大值</param>
        public int getRandomInt(int minNum, int maxNum)
        {
            return this._random.Next(minNum, maxNum);
        }
        #endregion

        #region 生成一个0.0到1.0的随机小数
        /// <summary>
        /// 生成一个0.0到1.0的随机小数
        /// </summary>
        public double getRandomDouble()
        {
            return this._random.NextDouble();
        }
        #endregion

        #region 对一个数组进行随机排序
        /// <summary>
        /// 对一个数组进行随机排序
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="arr">需要随机排序的数组</param>
        public void getRandomArray<T>(T[] arr)
        {
            //对数组进行随机排序的算法:随机选择两个位置，将两个位置上的值交换

            //交换的次数,这里使用数组的长度作为交换次数
            int count = arr.Length;

            //开始交换
            for (int i = 0; i < count; i++)
            {
                //生成两个随机数位置
                int randomNum1 = getRandomInt(0, arr.Length);
                int randomNum2 = getRandomInt(0, arr.Length);

                //定义临时变量
                T temp;

                //交换两个随机数位置的值
                temp = arr[randomNum1];
                arr[randomNum1] = arr[randomNum2];
                arr[randomNum2] = temp;
            }
        }


        // 一：随机生成不重复数字字符串  
        private int rep = 0;
        public string generateCheckCodeNum(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }

        //方法二：随机生成字符串（数字和字母混和）
        public string generateCheckCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        #region

        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="allChar"></param>
        /// <param name="CodeCount"></param>
        /// <returns></returns>
        private string getRandomCode(string allChar, int CodeCount)
        {
            //string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z"; 
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }
            return RandomCode;
        }

        #endregion
        #endregion
    }
}