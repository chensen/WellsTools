using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wells.Tools
{
    public static class clsCRC16
    {
        /// <summary>
        /// CRC校验
        /// </summary>
        /// <param name="data">校验数据</param>
        /// <returns>高低8位</returns>
        public static string calCRC16(string data)
        {
            string[] datas = data.Split(' ');
            List<byte> bytedata = new List<byte>();

            foreach (string str in datas)
            {
                bytedata.Add(byte.Parse(str, System.Globalization.NumberStyles.AllowHexSpecifier));
            }
            byte[] crcbuf = bytedata.ToArray();
            //计算并填写CRC校验码
            int crc = 0xffff;
            int len = crcbuf.Length;
            for (int n = 0; n < len; n++)
            {
                byte i;
                crc = crc ^ crcbuf[n];
                for (i = 0; i < 8; i++)
                {
                    int TT;
                    TT = crc & 1;
                    crc = crc >> 1;
                    crc = crc & 0x7fff;
                    if (TT == 1)
                    {
                        crc = crc ^ 0xa001;
                    }
                    crc = crc & 0xffff;
                }

            }
            string[] redata = new string[2];
            redata[1] = Convert.ToString((byte)((crc >> 8) & 0xff), 16).PadLeft(2, '0').ToUpper();
            redata[0] = Convert.ToString((byte)((crc & 0xff)), 16).PadLeft(2, '0').ToUpper();
            return data + " " + redata[0] + " " + redata[1];
        }

        /// <summary>
        /// CRC校验
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] calCRC16(byte[] bytes)
        {
            //计算并填写CRC校验码
            int crc = 0xffff;
            int len = bytes.Length;
            for (int n = 0; n < len; n++)
            {
                byte i;
                crc = crc ^ bytes[n];
                for (i = 0; i < 8; i++)
                {
                    int TT;
                    TT = crc & 1;
                    crc = crc >> 1;
                    crc = crc & 0x7fff;
                    if (TT == 1)
                    {
                        crc = crc ^ 0xa001;
                    }
                    crc = crc & 0xffff;
                }

            }

            var nl = bytes.Length + 2;
            //生成的两位校验码
            byte[] redata = new byte[2];
            redata[0] = (byte)((crc & 0xff));
            redata[1] = (byte)((crc >> 8) & 0xff);

            //重新组织字节数组
            var newByte = new byte[nl];
            for (int i = 0; i < bytes.Length; i++)
            {
                newByte[i] = bytes[i];
            }
            newByte[nl - 2] = (byte)redata[0];
            newByte[nl - 1] = redata[1];

            return newByte;
        }
    }
}
