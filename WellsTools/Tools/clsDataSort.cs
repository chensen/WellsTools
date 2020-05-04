using System.Collections.Generic;



namespace Wells.Tools
{
    public class clsDataSort
    {
        #region ��������
        /// <summary>
        /// ��������
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

        #region ѡ������
        /// <summary>
        /// ѡ������
        /// ԭ���ҳ�����������������ֵ���ŵ�ĩβ�����ҵ���Сֵ�ŵ���ͷ��
        /// ���̽�������ʣ���������С����������ͷ��
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

        #region ð������
        /// <summary>
        /// ð������
        /// ԭ����ͷ��ʼ��ÿһ��Ԫ�غ�������һ��Ԫ�رȽϣ�������󣬾ͽ�����Ƚϵ�Ԫ�ؽ��������򲻶�������ζ�ţ����Ԫ����������������ƶ�ֱ���������������Ԫ�ء�����ÿһ�ֽ�����ɶ��ܽ����ֵð�����
        /// ���̽�������Ҫע����� j С�� i��ÿ��ð���ݱ�Ȼ�Ὣ���ֵ�ŵ�����ĩβ��������Ҫ�������Ӧ�����ڼ��ٵġ��ܶ����ϰ汾ÿ��ð���ݺ���Ȼ���ǽ����е������еڶ���ð�ݼ� j С�� data.Count-1�����������ӱȽϴ�����
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

        #region ��ʶ��ð������
        /// <summary>
        /// ��ʶ��ð������
        /// ͨ����ӱ�ʶ���ֱ�ʣ������Ƿ��Ѿ����������ٱȽϴ���������ĳ��ð��û���κ������н��������Ѿ����򣩣�����������
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

        #region ��β������˫��ð������
        /// <summary>
        /// ��β������˫��ð������
        /// ԭ���������ҽ�����ð��ĩβ��Ȼ��ʣ����������������С��ð����ͷ�����ѭ��������
        /// ���̽�����������i��ð�ݣ�i��ż����ʣ���������ֵ����ð����ĩβ����������ʣ��������Сֵ����ð������ͷ������ʣ�����У�nΪʼ��data.Count-1-mΪĩ������ð�ݱȵ���ð�ݣ�����������У������׵õ������ʣ�����С��������ʹ�ñ�ʶ���������ĸ������ԡ�
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

        #region ��������
        /// <summary>
        /// ��������
        /// ԭ��ͨ�������������У���δ��������Ӻ���ǰ�Ƚϣ��ҵ�����λ�ò����롣
        /// ���̽�������Ҫ�������������Ϊi���洢��������ǰ���Һ���λ��j+1����i-1��j+1��Ԫ����������ƶ�һλ���ճ�j+1��Ȼ��֮ǰ�洢��ֵ�������λ�á�
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

        #region ���ֲ��ҷ��Ż���������
        /// <summary>
        /// ���ֲ��ҷ��Ż���������
        /// ԭ��ͨ�����ֲ��ҷ��ķ�ʽ�ҵ�һ��λ����������Ҫ��������������λ��ʱ������ǰһ������С�ں�һ������
        /// ���̽�������Ҫע����Ƕ��ֲ��ҷ���ʵ���� high-low==1 ��ʱ�� mid==low��������Ҫ33��mid-1 С�� 0 �� mid==0 ���жϣ��������л�����Խ�硣
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

        #region ��������
        /// <summary>
        /// ��������
        /// ԭ������������ѡһ������Ϊ���ڱ�����ʹ����С�ķ���������࣬������ķ��������Ҳࡣ��Ҫ���������еݹ�طָ��С���У�ÿ�ζ��÷ָ�������з��ϡ��ڱ����Ĺ�����Ȼ�ͽ����б������
        /// ���̽�����ȡ���ڱ������еĵ�һ��ֵ��Ȼ��ӵڶ�����ĩβͬʱ���ң����Ҫ��ʾ����С���ڱ���ֵ������Ҫ�ҵ���С�ڵ�i���Ҳ�Ҫ��ʾ���Ǵ����ڱ���ֵ������Ҫ�ҵ������ڵ�j�����ҵ���i��j�����������������Լ��ٽ���������i>=j ʱ������ȫ��������һ�飬������������j��Ȼ����С����һ�ߣ����ڱ��ǵ�һ������λ�ñ�Ӧ��С���Լ����������Խ��ڱ���j������ʹ���ϡ��ڱ����Ĺ�������汾��ȱ�����������������������Ļ����ݹ������ܿ��µġ�
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

        #region ��������2
        /// <summary>
        /// ��������2
        /// ���̽�����ȡ���ڱ��������м�����������зֳ����������С�ڵ����ڱ����Ҳ���ڵ����ڱ���Ҳ����˵���ڱ���һ���������������м䡣��Ȼ�ڱ������м䣬�����������ڱ�����˼���ʵ�֡��������ʵ��Ҳ���Դﵽ���������Ч������ȴ�����ÿ�εݹ���ɣ�Ҫ������������ܺ�û�м��٣�����i==j����
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

        #region ��������3
        /// <summary>
        /// ��������3
        /// ���̽�����������һ������Index���������ڱ���λ�á������ڱ������С���Լ����Ƕѣ��Ǿ���j������������i�������ﵽÿ�εݹ鶼�ܼ���Ҫ������������ܺ͵�Ŀ�ġ�
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