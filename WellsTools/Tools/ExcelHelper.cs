using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.Office.Interop.Excel;//需要引用该DLL，安装office会有
using System.Data;
using System.Reflection;
using System.Data.OleDb;
using Microsoft.CSharp;

namespace Wells.Tools.ExcelHelper
{
    /// <summary>
    /// C#与Excel交互类
    /// </summary>
    public class myExcelHelper
    {
        #region 导出到Excel
        #region ExportExcelForDataTable
        /// <summary>
        /// 从DataTable导出Excel,指定列别名,指定要排除的列
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="colName">各列的列名List string </param>
        /// <param name="excludeColumn">要显示/排除的列</param>
        /// <param name="excludeType">显示/排除列方式 0为所有列 1指定的为要显示的列 2指定的为要排除的列</param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 例:tp.xlsx 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForDataTable(System.Data.DataTable dt, string excelPathName, string pathType, List<string> colName, List<string> excludeColumn, string excludeType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0) return false;
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    return false;
                }
                System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;

                Microsoft.Office.Interop.Excel.Workbook workbook = null;
                if (TemplatePath == "")
                {
                    workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                }
                else
                {
                    workbook = workbooks.Add(TemplatePath); //加载模板
                }
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
                Microsoft.Office.Interop.Excel.Range range;

                long totalCount = dt.Rows.Count;
                if (exDataTableList != null && exDataTableList.Count > 0)
                {
                    foreach (System.Data.DataTable item in exDataTableList)
                    {
                        totalCount += item.Rows.Count;
                    }
                }
                long rowRead = 0;
                float percent = 0;
                string exclStr = "";//要排除的列临时项
                object exclType;//DataTable 列的类型,用于做
                int colPosition = 0;//列位置
                if (sheetName != null && sheetName != "")
                {
                    worksheet.Name = sheetName;
                }
                #region 列别名判定
                if (TemplatePath == "")
                {
                    if (colName != null && colName.Count > 0)
                    {
                        #region 指定了列别名
                        for (int i = 0; i < colName.Count; i++)
                        {
                            worksheet.Cells[1, i + 1] = colName[i];
                            range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                            range.Interior.ColorIndex = 15;
                            range.Font.Bold = true;
                            exclType = dt.Columns[i].DataType.Name;
                            if (exclType.ToString() != "DateTime")
                            {
                                //range.EntireColumn.AutoFit();//全局自动调整列宽,不能再使用单独设置
                                ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.AutoFit();
                            }
                            else
                            {
                                //规定列宽
                                ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.ColumnWidth = 20;
                            }
                            //((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.ColumnWidth = 20;
                        }
                        #endregion
                    }
                    else
                    {
                        #region 未指定别名
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                            range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                            range.Interior.ColorIndex = 15;
                            range.Font.Bold = true;
                            exclType = dt.Columns[i].DataType.Name;
                            if (exclType.ToString() != "DateTime")
                            {
                                //range.EntireColumn.AutoFit();//全局自动调整列宽,不能再使用单独设置
                                ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.AutoFit();
                            }
                            else
                            {
                                //规定列宽
                                ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.ColumnWidth = 20;
                            }
                            //((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.ColumnWidth = 20;
                        }
                        #endregion
                    }
                }
                else
                {
                    //用了模版，不加载标题
                }
                #endregion
                #region 显示/排除列判定
                if (excludeColumn != null && excludeColumn.Count > 0)
                {
                    switch (excludeType)
                    {
                        case "0":
                            {
                                #region 0为显示所有列
                                #region 常规项
                                int r = 0;
                                for (r = 0; r < dt.Rows.Count; r++)
                                {
                                    colPosition = 0;
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        if (TemplatePath == "")
                                        {
                                            worksheet.Cells[r + 2, colPosition + 1] = dt.Rows[r][i].ToString();
                                        }
                                        else
                                        {
                                            worksheet.Cells[r + TemplateRow, colPosition + 1] = dt.Rows[r][i].ToString();
                                        }
                                        colPosition++;
                                    }
                                    rowRead++;
                                    percent = ((float)(100 * rowRead)) / totalCount;
                                }
                                #endregion
                                #region 扩展项
                                if (exDataTableList != null && exDataTableList.Count > 0)
                                {
                                    foreach (System.Data.DataTable item in exDataTableList)
                                    {
                                        for (int k = 0; k < item.Rows.Count; r++, k++)
                                        {
                                            colPosition = 0;
                                            //生成扩展 DataTable 每行数据
                                            for (int t = 0; t < item.Columns.Count; t++)
                                            {
                                                if (TemplatePath == "")
                                                {
                                                    worksheet.Cells[r + 2, colPosition + 1] = item.Rows[k][t].ToString();
                                                }
                                                else
                                                {
                                                    worksheet.Cells[r + TemplateRow, colPosition + 1] = item.Rows[k][t].ToString();
                                                }
                                                colPosition++;
                                            }
                                            rowRead++;
                                            percent = ((float)(100 * rowRead)) / totalCount;
                                        }
                                    }
                                }
                                #endregion
                                #endregion
                            }; break;
                        case "1":
                            {
                                #region 1指定的为要显示的列
                                #region 常规项
                                int r = 0;
                                for (r = 0; r < dt.Rows.Count; r++)
                                {
                                    colPosition = 0;
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        exclStr = dt.Columns[i].ColumnName;
                                        if (excludeColumn.Contains(exclStr))
                                        {
                                            if (TemplatePath == "")
                                            {
                                                worksheet.Cells[r + 2, colPosition + 1] = dt.Rows[r][i].ToString();
                                            }
                                            else
                                            {
                                                worksheet.Cells[r + TemplateRow, colPosition + 1] = dt.Rows[r][i].ToString();
                                            }
                                            colPosition++;
                                        }
                                        else
                                        {

                                        }
                                    }
                                    rowRead++;
                                    percent = ((float)(100 * rowRead)) / totalCount;
                                }
                                #endregion
                                #region 扩展项
                                if (exDataTableList != null && exDataTableList.Count > 0)
                                {
                                    foreach (System.Data.DataTable item in exDataTableList)
                                    {
                                        for (int k = 0; k < item.Rows.Count; r++, k++)
                                        {
                                            colPosition = 0;
                                            //生成扩展 DataTable 每行数据
                                            for (int t = 0; t < item.Columns.Count; t++)
                                            {
                                                exclStr = dt.Columns[t].ColumnName;
                                                if (excludeColumn.Contains(exclStr))
                                                {
                                                    if (TemplatePath == "")
                                                    {
                                                        worksheet.Cells[r + 2, colPosition + 1] = item.Rows[k][t].ToString();
                                                    }
                                                    else
                                                    {
                                                        worksheet.Cells[r + TemplateRow, colPosition + 1] = item.Rows[k][t].ToString();
                                                    }
                                                    colPosition++;
                                                }
                                                else
                                                {

                                                }
                                            }
                                            rowRead++;
                                            percent = ((float)(100 * rowRead)) / totalCount;
                                        }
                                    }
                                }
                                #endregion
                                #endregion
                            }; break;
                        case "2":
                            {
                                #region 2指定的为要排除的列
                                #region 常规项
                                int r = 0;
                                for (r = 0; r < dt.Rows.Count; r++)
                                {
                                    colPosition = 0;
                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        exclStr = dt.Columns[i].ColumnName;
                                        if (excludeColumn.Contains(exclStr))
                                        {

                                        }
                                        else
                                        {
                                            if (TemplatePath == "")
                                            {
                                                worksheet.Cells[r + 2, colPosition + 1] = dt.Rows[r][i].ToString();
                                            }
                                            else
                                            {
                                                worksheet.Cells[r + TemplateRow, colPosition + 1] = dt.Rows[r][i].ToString();
                                            }
                                            colPosition++;
                                        }
                                    }
                                    rowRead++;
                                    percent = ((float)(100 * rowRead)) / totalCount;
                                }
                                #endregion
                                #region 扩展项
                                if (exDataTableList != null && exDataTableList.Count > 0)
                                {
                                    foreach (System.Data.DataTable item in exDataTableList)
                                    {
                                        for (int k = 0; k < item.Rows.Count; r++, k++)
                                        {
                                            colPosition = 0;
                                            //生成扩展 DataTable 每行数据
                                            for (int t = 0; t < item.Columns.Count; t++)
                                            {
                                                exclStr = dt.Columns[t].ColumnName;
                                                if (excludeColumn.Contains(exclStr))
                                                {

                                                }
                                                else
                                                {
                                                    if (TemplatePath == "")
                                                    {
                                                        worksheet.Cells[r + 2, colPosition + 1] = item.Rows[k][t].ToString();
                                                    }
                                                    else
                                                    {
                                                        worksheet.Cells[r + TemplateRow, colPosition + 1] = item.Rows[k][t].ToString();
                                                    }
                                                    colPosition++;
                                                }
                                            }
                                            rowRead++;
                                            percent = ((float)(100 * rowRead)) / totalCount;
                                        }
                                    }
                                }
                                #endregion
                                #endregion
                            }; break;
                        default:
                            break;
                    }

                }
                else
                {
                    //生成每行数据
                    int r = 0;
                    for (r = 0; r < dt.Rows.Count; r++)
                    {
                        //生成每列数据
                        if (TemplatePath == "")
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                worksheet.Cells[r + 1 + TemplateRow, i + 1] = dt.Rows[r][i].ToString();
                            }
                        }
                        rowRead++;
                        percent = ((float)(100 * rowRead)) / totalCount;
                    }
                }
                #endregion
                switch (pathType)
                {
                    case "0": { workbook.Saved = false; }; break;
                    case "1": { workbook.Saved = true; workbook.SaveCopyAs(excelPathName); }; break;
                    default:
                        return false;
                }
                xlApp.Visible = false;//是否在服务器打开
                workbook.Close(true, Type.Missing, Type.Missing);
                workbook = null;
                xlApp.Quit();
                xlApp = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 从DataTable导出Excel,指定列别名
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="colName">各列的列名List string </param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForDataTableC(System.Data.DataTable dt, string excelPathName, string pathType, List<string> colName, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> excludeColumn = new List<string>();
            string excludeType = "0";
            return ToExcelForDataTable(dt, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }

        /// <summary>
        /// 从DataTable导出Excel,指定要排除的列
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="excludeColumn">要显示/排除的列</param>
        /// <param name="excludeType">显示/排除列方式 0为所有列 1指定的为要显示的列 2指定的为要排除的列</param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForDataTableE(System.Data.DataTable dt, string excelPathName, string pathType, List<string> excludeColumn, string excludeType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> colName = new List<string>();
            return ToExcelForDataTable(dt, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }
        /// <summary>
        /// 从DataTable导出Excel，使用默认列名，不排除导出任何列
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForDataTableZ(System.Data.DataTable dt, string excelPathName, string pathType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> colName = new List<string>();
            List<string> excludeColumn = new List<string>();
            string excludeType = "0";
            return ToExcelForDataTable(dt, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }
        #endregion

        #region ExportExcelForModelList
        /// <summary>
        /// 从DataTable导出Excel,指定列别名,指定要排除的列
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="colName">各列的列名List string </param>
        /// <<param name="excludeColumn">要显示/排除的列</param>
        /// <param name="excludeType">显示/排除列方式 0为所有列 1指定的为要显示的列 2指定的为要排除的列</param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForModelList<T>(List<T> md, string excelPathName, string pathType, List<string> colName, List<string> excludeColumn, string excludeType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            System.Data.DataTable dt = ModelListToDataTable(md);
            return ToExcelForDataTable(dt, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }

        /// <summary>
        /// 从DataTable导出Excel,指定列别名
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="colName">各列的列名List string </param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForModelListC<T>(List<T> md, string excelPathName, string pathType, List<string> colName, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> excludeColumn = new List<string>();
            string excludeType = "0";
            return ToExcelForModelList(md, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }

        /// <summary>
        /// 从DataTable导出Excel,指定要排除的列
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="excludeColumn">要显示/排除的列</param>
        /// <param name="excludeType">显示/排除列方式 0为所有列 1指定的为要显示的列 2指定的为要排除的列</param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForModelListE<T>(List<T> md, string excelPathName, string pathType, List<string> excludeColumn, string excludeType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> colName = new List<string>();
            return ToExcelForModelList(md, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }

        /// <summary>
        /// 从DataTable导出Excel，使用默认列名，不排除导出任何列
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">含Excel名称的保存路径 在pathType＝1时有效，其它请赋值空字符串</param>
        /// <param name="pathType">路径类型。只能取值：0客户自定义路径；1服务端定义路径，标识文件保存路径是服务端指定还是客户自定义路径及文件名</param>
        /// <param name="sheetName">sheet1的名称 为空字符串时保持默认名称</param>
        /// <param name="TemplatePath">模版在项目服务器中路径 为空字符串时表示无模版</param>
        /// <param name="TemplateRow">模版中已存在数据的行数，无模版时请传入参数 0</param>
        /// <param name="exDataTableList">扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同</param>
        /// <returns>bool</returns>
        public static bool ToExcelForModelListZ<T>(List<T> md, string excelPathName, string pathType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> colName = new List<string>();
            List<string> excludeColumn = new List<string>();
            string excludeType = "0";
            return ToExcelForModelList(md, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }
        #endregion

        #region 从DataTable导出Excel； ToExcelModel实体传参
        /// <summary>
        /// 从DataTable导出Excel； ToExcelModel实体传参
        /// </summary>
        /// <param name="tem">ExcelHelper.ToExcelModel</param>
        /// <returns></returns>
        public static bool ToExcelForDataTable(ToExcelModel tem)
        {
            if (tem != null)
            {
                return ToExcelForDataTable(tem.DataTable, tem.excelPathName, tem.pathType, tem.colNameList, tem.excludeColumn, tem.excludeType, tem.sheetName, tem.TemplatePath, tem.TemplateRow, tem.exDataTableList);
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Model To DataTable
        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static System.Data.DataTable ModelListToDataTable<T>(List<T> modelList)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            // column names
            PropertyInfo[] oProps = null;

            if (modelList == null) return dtReturn;

            foreach (T rec in modelList)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
        #endregion

        #region 说明 如何使用
        /*
         * 功能：
         *      1、将System.Data.DataTable数据导出到Excel文件
         *      2、将Model(Entity)数据实体导出到Excel文件
         * 完整调用：
         *      1、ExcelHelper.ToExcelForDataTable(DataTable,excelPathName,pathType,colName,excludeColumn,excludeType,sheetName,TemplatePath,TemplateRow,exDataTableList);
         *      2、ExcelHelper.ToExcelForModelList(Model,excelPathName,pathType,colName,excludeColumn,excludeType,sheetName,TemplatePath,TemplateRow,exDataTableList);
         * 参数说明：
         *      1、DataTable：DataSet.DataTable[0];数据表
         *      2、Model：Model.Users users = new Model.Users(){...};数据实体
         *      3、excelPathName：含Excel名称的保存路径 在pathType＝1时有效。用户自定义保存路径时请赋值空字符串 ""。格式："E://456.xlsx"
         *      4、pathType：路径类型。只能取值：0用户自定义路径，弹出用户选择路径对话框；1服务端定义路径。标识文件保存路径是服务端指定还是客户自定义路径及文件名，与excelPathName参数合用
         *      5、colName：各列的列别名List string，比如：字段名为userName，此处可指定为"用户名"，并以此显示
         *      6、excludeColumn：要显示/排除的列，指定这些列用于显示，或指定这些列用于不显示。倒低这些列是显示还是不显示，由excludeType参数决定
         *      7、excludeType：显示/排除列方式。 0为显示所有列 1指定的是要显示的列 2指定的是要排除的列，与excludeColumn合用
         *      8、sheetName：sheet1的名称，要使期保持默认名称请指定为空字符串 ""
         *      9、TemplatePath：模版在项目服务器中路径 例:tp.xlsx 。当为空字符串 "" 时表示无模版
         *      10、TemplateRow：模版中已存在数据的行数，与TemplatePath合用，无模版时请传入参数 0
         *      11、exDataTableList：扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同
         * 注意：
         *      1、exDataTableList参数为一个List<System.Data.DataTable> 集合，当数据为 Model 时，可先调用 ExcelHelper.ModelListToDataTable(System.Data.DataTable dt)将Model转为System.Data.DataTable
         */
        #endregion
        #endregion

        #region 从Excel导入数据到 Ms Sql
        /// <summary>
        /// 从Excel导入数据到 Ms Sql
        /// </summary>
        /// <param name="excelFile">Excel文件路径(含文件名)</param>
        /// <param name="sheetName">sheet名</param>
        /// <param name="DbTableName">存储到数据库中的数据库表名称</param>
        /// <param name="columnType">对应表格的数据类型，如果为null，则为默认类型：double,nvarchar(500),datetime</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static bool FromExcel(string excelFile, string sheetName, string DbTableName, List<string> columnType, string connectionString)
        {
            DataSet ds = new DataSet();
            try
            {
                //获取全部数据   
                //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + excelFile + ";" + "Extended Properties=Excel 8.0;";
                string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + excelFile + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'"; //此连

                #region 知识扩展
                //HDR=Yes，代表第一行是标题，不做为数据使用。HDR=NO，则表示第一行不是标题，做为数据来使用。系统默认的是YES
                //IMEX=0 只读模式
                //IMEX=1 写入模式
                //IMEX=2 可读写模式
                #endregion

                #region 命名执行
                using (OleDbConnection conn = new OleDbConnection(strConn))
                {
                    conn.Open();
                    string strExcel = "";
                    OleDbDataAdapter myCommand = null;
                    strExcel = string.Format("select * from [{0}$]", sheetName);
                    myCommand = new OleDbDataAdapter(strExcel, strConn);
                    myCommand.Fill(ds, sheetName);

                    #region 数据库表是否存在的 T-SQL 检测语句准备
                    //如果目标表不存在则创建   
                    string strSql = string.Format("if object_id('{0}') is null create table {0}(", DbTableName != "" ? DbTableName : sheetName);
                    if (columnType != null && columnType.Count > 0)
                    {
                        #region 手动指定定每个字段的数据类型
                        //指定数据格式,要求一一对应
                        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                        {
                            System.Data.DataColumn c = ds.Tables[0].Columns[i];
                            strSql += string.Format("[{0}] {1},", c.ColumnName, columnType[i]);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 使用默认数据类型
                        foreach (System.Data.DataColumn c in ds.Tables[0].Columns)
                        {
                            //使用默认格式：只有double,DateTime,String三种类型
                            switch (c.DataType.ToString())
                            {
                                case "DateTime":
                                    {
                                        strSql += string.Format("[{0}] DateTime,", c.ColumnName);
                                    }; break;
                                case "Double":
                                    {
                                        strSql += string.Format("[{0}] double,", c.ColumnName);
                                    }; break;
                                default:
                                    strSql += string.Format("[{0}] nvarchar(500),", c.ColumnName);
                                    break;
                            }
                        }
                        #endregion
                    }
                    strSql = strSql.Trim(',') + ")";
                    #endregion

                    #region 执行 T-SQL 如果数据库表不存在则新建表，如果存在则不新建
                    using (System.Data.SqlClient.SqlConnection sqlconn = new System.Data.SqlClient.SqlConnection(connectionString))
                    {
                        sqlconn.Open();
                        System.Data.SqlClient.SqlCommand command = sqlconn.CreateCommand();
                        command.CommandText = strSql;
                        command.ExecuteNonQuery();
                        sqlconn.Close();
                    }
                    #endregion

                    #region 向数据库表插入数据
                    using (System.Data.SqlClient.SqlBulkCopy sbc = new System.Data.SqlClient.SqlBulkCopy(connectionString))
                    {
                        sbc.SqlRowsCopied += new System.Data.SqlClient.SqlRowsCopiedEventHandler(bcp_SqlRowsCopied);
                        sbc.BatchSize = 100;//每次传输的行数   
                        sbc.NotifyAfter = 100;//进度提示的行数   
                        sbc.DestinationTableName = DbTableName != "" ? DbTableName : sheetName;//数据库表名表名
                        sbc.WriteToServer(ds.Tables[0]);
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        #region 进度显示
        /// <summary>
        /// 进度显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void bcp_SqlRowsCopied(object sender, System.Data.SqlClient.SqlRowsCopiedEventArgs e)
        {
            e.RowsCopied.ToString();
        }
        #endregion

        #endregion
    }
    public class ToExcelModel
    {
        #region ToExcelModel自动属性
        /// <summary>
        /// 数据表
        /// </summary>
        public System.Data.DataTable DataTable { get; set; }
        /// <summary>
        /// 含Excel名称的保存路径 在pathType＝1时有效。用户自定义保存路径时请赋值空字符串 ""。格式："E://456.xlsx"
        /// </summary>
        public string excelPathName { get; set; }
        /// <summary>
        /// 路径类型。只能取值：0用户自定义路径，弹出用户选择路径对话框；1服务端定义路径。标识文件保存路径是服务端指定还是客户自定义路径及文件名，与excelPathName参数合用
        /// </summary>
        public string pathType { get; set; }
        /// <summary>
        /// 各列的列别名List string，比如：字段名为userName，此处可指定为"用户名"，并以此显示
        /// </summary>
        public List<string> colNameList { get; set; }
        /// <summary>
        /// 要显示/排除的列，指定这些列用于显示，或指定这些列用于不显示。倒低这些列是显示还是不显示，由excludeType参数决定
        /// </summary>
        public List<string> excludeColumn { get; set; }
        /// <summary>
        /// 显示/排除列方式。 0为显示所有列 1指定的是要显示的列 2指定的是要排除的列，与excludeColumn合用
        /// </summary>
        public string excludeType { get; set; }
        /// <summary>
        /// sheet1的名称，要使期保持默认名称请指定为空字符串 ""
        /// </summary>
        public string sheetName { get; set; }
        /// <summary>
        /// 模版在项目服务器中路径 例:tp.xlsx 。当为空字符串 "" 时表示无模版
        /// </summary>
        public string TemplatePath { get; set; }
        /// <summary>
        /// 模版中已存在数据的行数，与TemplatePath合用，无模版时请传入参数 0
        /// </summary>
        public int TemplateRow { get; set; }
        /// <summary>
        /// 扩展 DataTable List 用于当上下两个及以上DataTable数据类型不一至,但又都在同一列时使用,要求格式与参数第一个 DataTable的列名字段名一至,仅字段类型可不同
        /// </summary>
        public List<System.Data.DataTable> exDataTableList { get; set; }
        #endregion
    }
    public class FromExcelModel
    {
        /// <summary>
        /// Excel文件路径(含文件名)
        /// </summary>
        public string excelFile { get; set; }
        /// <summary>
        /// sheet名<
        /// </summary>
        public string sheetName { get; set; }
        /// <summary>
        /// 存储到数据库中的数据库表名称
        /// </summary>
        public string DbTableName { get; set; }
        /// <summary>
        /// 对应表格的数据类型，如果为null，则为默认类型：double,nvarchar(500),datetime
        /// </summary>
        public List<string> columnTypeList { get; set; }
        /// <summary>
        /// 连接字符串 server=serverip;database=databasename;uid=username;pwd=password;
        /// </summary>
        public string connectionString { get; set; }
    }
}
/*
        //该类能够将DataTable直接导出为Excel文件，将List<Model> Model数据实体集合导出到Excel文件,并且提供了直接将Excel数据导出进数据库的方法，还是非常实用的，下面是两个调用示例，注意实际过程中Model和字段这些都是自己定义的，例子中只是一部分核心调用代码：
        /// <summary>
        /// 导出至Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                BLL.COMMON_UserInfo cu = new BLL.COMMON_UserInfo();
                List<Model.COMMON_UserInfo> cuiList = cu.GetModelList("");
                BLL.manager_log bll = new BLL.manager_log();
                DataSet ds = bll.GetList(10, "", "id");
                System.Data.DataTable dt = ds.Tables[0];
                List<string> colName = new List<string>() { 
                "用户ID",
                "用户名",
                "备注",
                "用户IP",
                "操作时间",
                "操作时间1",
                "操作时间2"
                };
                List<string> tt = new List<string>() { "action_type" };
                DataSet dss = bll.GetList(10, "", "id");
                List<System.Data.DataTable> dtss = new List<System.Data.DataTable>();
                dtss.Add(dss.Tables[0]);
                dtss.Add(dss.Tables[0]);
                dtss.Add(dss.Tables[0]);
                ExcelHelper.ToExcelForDataTable(dt, Server.MapPath("~").ToString() + "456.xlsx", "0", colName, tt, "2", "", "", 0, dtss);//指定了列别名，指定了要排除的列
                ToExcelModel tem = new ToExcelModel()
                {
                    DataTable = dt,
                    excelPathName = "",
                    pathType = "0",
                    colNameList = colName,
                    excludeColumn = tt,
                    excludeType = "0",
                    sheetName = "成功",
                    TemplatePath = "",
                    TemplateRow = 0,
                    exDataTableList = dtss
                };
                ExcelHelper.ToExcelForDataTable(tem);
            }
            catch (Exception ex)
            {
                FileLog.Log(ex.Message, "ExportExcelByDataTable");
            }
        }

        /// <summary>
        /// 导入数据到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button2_Click(object sender, EventArgs e)
        {
            string excelFile = "G://123.xls";
            string sheetName = "Sheet1";
            string DbTableName = "test_new_table";// "test_new_table";
            List<string> columnType = new List<string>() { 
                "int",
                "nvarchar(100)",
                "decimal(18,2)",
                "nvarchar(100)",
                "datetime"
            };
            string connectionString = "server=.;database=Test1;uid=sa;pwd=zhangquan;";
            ExcelHelper.FromExcel(excelFile, sheetName, DbTableName, columnType, connectionString);
        }
*/