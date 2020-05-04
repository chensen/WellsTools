using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;

namespace Wells.Tools
{
    public class clsExcelNPOI
    {
        private static IWorkbook workbook = null;
        private static FileStream fs = null;
        private static FileStream fout = null;
        private static IRow row = null;
        private static ISheet sheet = null;

        private static bool bOpen = false;

        public static bool open(string strFilePath,string strSheet)
        {
            bool ret = false;
            try
            {
                if (bOpen)
                    ret = true;
                else
                {
                    fs = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);//读取流
                    POIFSFileSystem ps = new POIFSFileSystem(fs);//需using NPOI.POIFS.FileSystem;
                    workbook = new HSSFWorkbook(ps);
                    sheet = workbook.GetSheet(strSheet);//获取工作表
                    row = sheet.GetRow(0); //得到表头
                    fout = new FileStream(strFilePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);//写入流
                    bOpen = true;
                    ret = true;
                }
            }
            catch (Exception exc)
            {
                bOpen = false;
            }
            return ret;
        }

        public static bool write()
        {
            if (bOpen)
            {
                fout.Flush();
                workbook.Write(fout);//写入文件
                workbook = null;
                return true;
            }
            return false;
        }

        public static bool close()
        {
            workbook = null;
            fout.Close();
            fs.Close();
            return true;
        }

        public static void setColValue(int colIndex,string value,bool bNewLine)
        {
            #region 往文件尾增加数据
            try
            {
                if (bOpen)
                {
                    if (bNewLine)
                        row = sheet.CreateRow(sheet.LastRowNum + 1);
                    else
                        row = sheet.GetRow(sheet.LastRowNum);
                    if (row != null)
                    {
                        ICell cell = row.CreateCell(colIndex);
                        cell.SetCellValue(value);
                    }
                }
            }
            catch (System.Exception exc)
            {
                
            }
            #endregion
        }

        public static void setColValue(int colIndex, double value, bool bNewLine)
        {
            #region 往文件尾增加数据
            try
            {
                if (bOpen)
                {
                    if (bNewLine)
                        row = sheet.CreateRow(sheet.LastRowNum + 1);
                    else
                        row = sheet.GetRow(sheet.LastRowNum);
                    if (row != null)
                    {
                        ICell cell = row.CreateCell(colIndex);
                        cell.SetCellValue(value);
                    }
                }
            }
            catch (System.Exception exc)
            {

            }
            #endregion
        }
    }
}
