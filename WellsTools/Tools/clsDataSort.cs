using System.Collections.Generic;



namespace Wells.Tools
{
    public class clsDataSort
    {
        #region 交换数据
        /// <summary>
        /// 交换数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void swap(IList<int> data, int x, int y)
        {
            int temp = data[x];
            data[x] = data[y];
            data[y] = temp;
        }
        #endregion

        #region 选择排序
        /// <summary>
        /// 选择排序
        /// 原理：找出参与排序的数组最大值，放到末尾（或找到最小值放到开头）
        /// 过程解析：将剩余数组的最小数交换到开头。
        /// </summary>
        /// <param name="data"></param>
        public static void selectSort(IList<int> data)
        {
            for (int i = 0; i < data.Count - 1; i++)
            {
                int min = i;
                int temp = data[i];
                for (int j = i + 1; j < data.Count; j++)
                {
                    if (data[j] < temp)
                    {
                        min = j;
                        temp = data[j];
                    }
                }
                if (min != i)
                    swap(data, min, i);
            }
        }
        #endregion

        #region 冒泡排序
        /// <summary>
        /// 冒泡排序
        /// 原理：从头开始，每一个元素和它的下一个元素比较，如果它大，就将它与比较的元素交换，否则不动。这意味着，大的元素总是在向后慢慢移动直到遇到比它更大的元素。所以每一轮交换完成都能将最大值冒到最后。
        /// 过程解析：需要注意的是 j 小于 i，每轮冒完泡必然会将最大值排到数组末尾，所以需要排序的数应该是在减少的。很多网上版本每轮冒完泡后依然还是将所有的数进行第二轮冒泡即 j 小于 data.Count-1，这样会增加比较次数。
        /// </summary>
        /// <param name="data"></param>
        public static void bubbleSort(IList<int> data)
        {
            for (int i = data.Count - 1; i > 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    if (data[j] > data[j + 1])
                        swap(data, j, j + 1);
                }
            }
        }
        #endregion

        #region 标识符冒泡排序
        /// <summary>
        /// 标识符冒泡排序
        /// 通过添加标识来分辨剩余的数是否已经有序来减少比较次数，发现某轮冒泡没有任何数进行交换（即已经有序），就跳出排序。
        /// </summary>
        /// <param name="data"></param>
        public static void bubbleSortImprovedWithFlag(IList<int> data)
        {
            bool flag;
            for (int i = data.Count - 1; i > 0; i--)
            {
                flag = true;
                for (int j = 0; j < i; j++)
                {
                    if (data[j] > data[j + 1])
                    {
                        swap(data, j, j + 1);
                        flag = false;
                    }
                }
                if (flag) break;
            }
        }
        #endregion

        #region 鸡尾酒排序，双向冒泡排序
        /// <summary>
        /// 鸡尾酒排序，双向冒泡排序
        /// 原理：自左向右将大数冒到末尾，然后将剩余数列再自右向左将小数冒到开头，如此循环往复。
        /// 过程解析：分析第i轮冒泡，i是偶数则将剩余数列最大值向右冒泡至末尾，是奇数则将剩余数列最小值向左冒泡至开头。对于剩余数列，n为始，data.Count-1-m为末。来回冒泡比单向冒泡：对于随机数列，更容易得到有序的剩余数列。因此这里使用标识将会提升的更加明显。
        /// </summary>
        /// <param name="data"></param>
        public static void bubbleCocktailSort(IList<int> data)
        {
            bool flag;
            int m = 0, n = 0;
            for (int i = data.Count - 1; i > 0; i--)
            {
                flag = true;
                if (i % 2 == 0)
                {
                    for (int j = n; j < data.Count - 1 - m; j++)
                    {
                        if (data[j] > data[j + 1])
                        {
                            swap(data, j, j + 1);
                            flag = false;
                        }
                    }
                    if (flag) break;
                    m++;
                }
                else
                {
                    for (int k = data.Count - 1 - m; k > n; k--)
                    {
                        if (data[k] < data[k - 1])
                        {
                            swap(data, k, k - 1);
                            flag = false;
                        }
                    }
                    if (flag) break;
                    n++;
                }
            }
        }
        #endregion

        #region 插入排序
        /// <summary>
        /// 插入排序
        /// 原理：通过构建有序数列，将未排序的数从后向前比较，找到合适位置并插入。
        /// 过程解析：将要排序的数（索引为i）存储起来，向前查找合适位置j+1，将i-1到j+1的元素依次向后移动一位，空出j+1，然后将之前存储的值放在这个位置。
        /// </summary>
        /// <param name="data"></param>
        public static void insertSort(IList<int> data)
        {
            int temp;
            for (int i = 1; i < data.Count; i++)
            {
                temp = data[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    if (data[j] > temp)
                    {
                        data[j + 1] = data[j];
                        if (j == 0)
                        {
                            data[0] = temp;
                            break;
                        }
                    }
                    else
                    {
                        data[j + 1] = temp;
                        break;
                    }
                }
            }
        }
        #endregion

        #region 二分查找法优化插入排序
        /// <summary>
        /// 二分查找法优化插入排序
        /// 原理：通过二分查找法的方式找到一个位置索引。当要排序的数插入这个位置时，大于前一个数，小于后一个数。
        /// 过程解析：需要注意的是二分查找方法实现中 high-low==1 的时候 mid==low，所以需要33行mid-1 小于 0 即 mid==0 的判断，否则下行会索引越界。
        /// </summary>
        /// <param name="data"></param>
        public static void insertSortImprovedWithBinarySearch(IList<int> data)
        {
            int temp;
            int tempIndex;
            for (int i = 1; i < data.Count; i++)
            {
                temp = data[i];
                tempIndex = binarySearchForInsertSort(data, 0, i, i);
                for (int j = i - 1; j >= tempIndex; j--)
                {
                    data[j + 1] = data[j];
                }
                data[tempIndex] = temp;
            }
        }

        public static int binarySearchForInsertSort(IList<int> data, int low, int high, int key)
        {
            if (low >= data.Count - 1)
                return data.Count - 1;
            if (high <= 0)
                return 0;
            int mid = (low + high) / 2;
            if (mid == key) return mid;
            if (data[key] > data[mid])
            {
                if (data[key] < data[mid + 1])
                    return mid + 1;
                return binarySearchForInsertSort(data, mid + 1, high, key);
            }
            else  // data[key] <= data[mid]
            {
                if (mid - 1 < 0) return 0;
                if (data[key] > data[mid - 1])
                    return mid;
                return binarySearchForInsertSort(data, low, mid - 1, key);
            }
        }
        #endregion

        #region 快速排序
        /// <summary>
        /// 快速排序
        /// 原理：从数列中挑选一个数作为“哨兵”，使比它小的放在它的左侧，比它大的放在它的右侧。将要排序是数列递归地分割到最小数列，每次都让分割出的数列符合“哨兵”的规则，自然就将数列变得有序。
        /// 过程解析：取的哨兵是数列的第一个值，然后从第二个和末尾同时查找，左侧要显示的是小于哨兵的值，所以要找到不小于的i，右侧要显示的是大于哨兵的值，所以要找到不大于的j。将找到的i和j的数交换，这样可以减少交换次数。i>=j 时，数列全部查找了一遍，而不符合条件j必然是在小的那一边，而哨兵是第一个数，位置本应是小于自己的数。所以将哨兵与j交换，使符合“哨兵”的规则。这个版本的缺点在于如果是有序数列排序的话，递归次数会很可怕的。
        /// </summary>
        /// <param name="data"></param>
        public static void quickSortStrict(IList<int> data)
        {
            quickSortStrict(data, 0, data.Count - 1);
        }

        public static void quickSortStrict(IList<int> data, int low, int high)
        {
            if (low >= high) return;
            int temp = data[low];
            int i = low + 1, j = high;
            while (true)
            {
                while (data[j] > temp) j--;
                while (data[i] < temp && i < j) i++;
                if (i >= j) break;
                swap(data, i, j);
                i++; j--;
            }
            if (j != low)
                swap(data, low, j);
            quickSortStrict(data, j + 1, high);
            quickSortStrict(data, low, j - 1);
        }
        #endregion

        #region 快速排序2
        /// <summary>
        /// 快速排序2
        /// 过程解析：取的哨兵是数列中间的数。将数列分成两波，左侧小于等于哨兵，右侧大于等于哨兵。也就是说，哨兵不一定处于两波数的中间。虽然哨兵不在中间，但不妨碍“哨兵”的思想的实现。所以这个实现也可以达到快速排序的效果。但却造成了每次递归完成，要排序的数列数总和没有减少（除非i==j）。
        /// </summary>
        /// <param name="data"></param>
        public static void quickSortRelax(IList<int> data)
        {
            quickSortRelax(data, 0, data.Count - 1);
        }

        public static void quickSortRelax(IList<int> data, int low, int high)
        {
            if (low >= high) return;
            int temp = data[(low + high) / 2];
            int i = low - 1, j = high + 1;
            while (true)
            {
                while (data[++i] < temp) ;
                while (data[--j] > temp) ;
                if (i >= j) break;
                swap(data, i, j);
            }
            quickSortRelax(data, j + 1, high);
            quickSortRelax(data, low, i - 1);
        }
        #endregion

        #region 快速排序3
        /// <summary>
        /// 快速排序3
        /// 过程解析：定义了一个变量Index，来跟踪哨兵的位置。发现哨兵最后在小于自己的那堆，那就与j交换，否则与i交换。达到每次递归都能减少要排序的数列数总和的目的。
        /// </summary>
        /// <param name="data"></param>
        public static void quickSortRelaxImproved(IList<int> data)
        {
            quickSortRelaxImproved(data, 0, data.Count - 1);
        }

        public static void quickSortRelaxImproved(IList<int> data, int low, int high)
        {
            if (low >= high) return;
            int temp = data[(low + high) / 2];
            int i = low - 1, j = high + 1;
            int index = (low + high) / 2;
            while (true)
            {
                while (data[++i] < temp) ;
                while (data[--j] > temp) ;
                if (i >= j) break;
                swap(data, i, j);
                if (i == index) index = j;
                else if (j == index) index = i;
            }
            if (j == i)
            {
                quickSortRelaxImproved(data, j + 1, high);
                quickSortRelaxImproved(data, low, i - 1);
            }
            else //i-j==1
            {
                if (index >= i)
                {
                    if (index != i)
                        swap(data, index, i);
                    quickSortRelaxImproved(data, i + 1, high);
                    quickSortRelaxImproved(data, low, i - 1);
                }
                else //index < i
                {
                    if (index != j)
                        swap(data, index, j);
                    quickSortRelaxImproved(data, j + 1, high);
                    quickSortRelaxImproved(data, low, j - 1);
                }
            }
        }
        #endregion
    }
}