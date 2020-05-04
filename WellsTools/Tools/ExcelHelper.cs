using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.Office.Interop.Excel;//��Ҫ���ø�DLL����װoffice����
using System.Data;
using System.Reflection;
using System.Data.OleDb;
using Microsoft.CSharp;

namespace Wells.Tools.ExcelHelper
{
    /// <summary>
    /// C#��Excel������
    /// </summary>
    public class myExcelHelper
    {
        #region ������Excel
        #region ExportExcelForDataTable
        /// <summary>
        /// ��DataTable����Excel,ָ���б���,ָ��Ҫ�ų�����
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">��Excel���Ƶı���·�� ��pathType��1ʱ��Ч�������븳ֵ���ַ���</param>
        /// <param name="pathType">·�����͡�ֻ��ȡֵ��0�ͻ��Զ���·����1����˶���·������ʶ�ļ�����·���Ƿ����ָ�����ǿͻ��Զ���·�����ļ���</param>
        /// <param name="colName">���е�����List string </param>
        /// <param name="excludeColumn">Ҫ��ʾ/�ų�����</param>
        /// <param name="excludeType">��ʾ/�ų��з�ʽ 0Ϊ������ 1ָ����ΪҪ��ʾ���� 2ָ����ΪҪ�ų�����</param>
        /// <param name="sheetName">sheet1������ Ϊ���ַ���ʱ����Ĭ������</param>
        /// <param name="TemplatePath">ģ������Ŀ��������·�� ��:tp.xlsx Ϊ���ַ���ʱ��ʾ��ģ��</param>
        /// <param name="TemplateRow">ģ�����Ѵ������ݵ���������ģ��ʱ�봫����� 0</param>
        /// <param name="exDataTableList">��չ DataTable List ���ڵ���������������DataTable�������Ͳ�һ��,���ֶ���ͬһ��ʱʹ��,Ҫ���ʽ�������һ�� DataTable�������ֶ���һ��,���ֶ����Ϳɲ�ͬ</param>
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
                    workbook = workbooks.Add(TemplatePath); //����ģ��
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
                string exclStr = "";//Ҫ�ų�������ʱ��
                object exclType;//DataTable �е�����,������
                int colPosition = 0;//��λ��
                if (sheetName != null && sheetName != "")
                {
                    worksheet.Name = sheetName;
                }
                #region �б����ж�
                if (TemplatePath == "")
                {
                    if (colName != null && colName.Count > 0)
                    {
                        #region ָ�����б���
                        for (int i = 0; i < colName.Count; i++)
                        {
                            worksheet.Cells[1, i + 1] = colName[i];
                            range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                            range.Interior.ColorIndex = 15;
                            range.Font.Bold = true;
                            exclType = dt.Columns[i].DataType.Name;
                            if (exclType.ToString() != "DateTime")
                            {
                                //range.EntireColumn.AutoFit();//ȫ���Զ������п�,������ʹ�õ�������
                                ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.AutoFit();
                            }
                            else
                            {
                                //�涨�п�
                                ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.ColumnWidth = 20;
                            }
                            //((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.ColumnWidth = 20;
                        }
                        #endregion
                    }
                    else
                    {
                        #region δָ������
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                            range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                            range.Interior.ColorIndex = 15;
                            range.Font.Bold = true;
                            exclType = dt.Columns[i].DataType.Name;
                            if (exclType.ToString() != "DateTime")
                            {
                                //range.EntireColumn.AutoFit();//ȫ���Զ������п�,������ʹ�õ�������
                                ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.AutoFit();
                            }
                            else
                            {
                                //�涨�п�
                                ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.ColumnWidth = 20;
                            }
                            //((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1]).Columns.ColumnWidth = 20;
                        }
                        #endregion
                    }
                }
                else
                {
                    //����ģ�棬�����ر���
                }
                #endregion
                #region ��ʾ/�ų����ж�
                if (excludeColumn != null && excludeColumn.Count > 0)
                {
                    switch (excludeType)
                    {
                        case "0":
                            {
                                #region 0Ϊ��ʾ������
                                #region ������
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
                                #region ��չ��
                                if (exDataTableList != null && exDataTableList.Count > 0)
                                {
                                    foreach (System.Data.DataTable item in exDataTableList)
                                    {
                                        for (int k = 0; k < item.Rows.Count; r++, k++)
                                        {
                                            colPosition = 0;
                                            //������չ DataTable ÿ������
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
                                #region 1ָ����ΪҪ��ʾ����
                                #region ������
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
                                #region ��չ��
                                if (exDataTableList != null && exDataTableList.Count > 0)
                                {
                                    foreach (System.Data.DataTable item in exDataTableList)
                                    {
                                        for (int k = 0; k < item.Rows.Count; r++, k++)
                                        {
                                            colPosition = 0;
                                            //������չ DataTable ÿ������
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
                                #region 2ָ����ΪҪ�ų�����
                                #region ������
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
                                #region ��չ��
                                if (exDataTableList != null && exDataTableList.Count > 0)
                                {
                                    foreach (System.Data.DataTable item in exDataTableList)
                                    {
                                        for (int k = 0; k < item.Rows.Count; r++, k++)
                                        {
                                            colPosition = 0;
                                            //������չ DataTable ÿ������
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
                    //����ÿ������
                    int r = 0;
                    for (r = 0; r < dt.Rows.Count; r++)
                    {
                        //����ÿ������
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
                xlApp.Visible = false;//�Ƿ��ڷ�������
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
        /// ��DataTable����Excel,ָ���б���
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">��Excel���Ƶı���·�� ��pathType��1ʱ��Ч�������븳ֵ���ַ���</param>
        /// <param name="pathType">·�����͡�ֻ��ȡֵ��0�ͻ��Զ���·����1����˶���·������ʶ�ļ�����·���Ƿ����ָ�����ǿͻ��Զ���·�����ļ���</param>
        /// <param name="colName">���е�����List string </param>
        /// <param name="sheetName">sheet1������ Ϊ���ַ���ʱ����Ĭ������</param>
        /// <param name="TemplatePath">ģ������Ŀ��������·�� Ϊ���ַ���ʱ��ʾ��ģ��</param>
        /// <param name="TemplateRow">ģ�����Ѵ������ݵ���������ģ��ʱ�봫����� 0</param>
        /// <param name="exDataTableList">��չ DataTable List ���ڵ���������������DataTable�������Ͳ�һ��,���ֶ���ͬһ��ʱʹ��,Ҫ���ʽ�������һ�� DataTable�������ֶ���һ��,���ֶ����Ϳɲ�ͬ</param>
        /// <returns>bool</returns>
        public static bool ToExcelForDataTableC(System.Data.DataTable dt, string excelPathName, string pathType, List<string> colName, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> excludeColumn = new List<string>();
            string excludeType = "0";
            return ToExcelForDataTable(dt, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }

        /// <summary>
        /// ��DataTable����Excel,ָ��Ҫ�ų�����
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">��Excel���Ƶı���·�� ��pathType��1ʱ��Ч�������븳ֵ���ַ���</param>
        /// <param name="pathType">·�����͡�ֻ��ȡֵ��0�ͻ��Զ���·����1����˶���·������ʶ�ļ�����·���Ƿ����ָ�����ǿͻ��Զ���·�����ļ���</param>
        /// <param name="excludeColumn">Ҫ��ʾ/�ų�����</param>
        /// <param name="excludeType">��ʾ/�ų��з�ʽ 0Ϊ������ 1ָ����ΪҪ��ʾ���� 2ָ����ΪҪ�ų�����</param>
        /// <param name="sheetName">sheet1������ Ϊ���ַ���ʱ����Ĭ������</param>
        /// <param name="TemplatePath">ģ������Ŀ��������·�� Ϊ���ַ���ʱ��ʾ��ģ��</param>
        /// <param name="TemplateRow">ģ�����Ѵ������ݵ���������ģ��ʱ�봫����� 0</param>
        /// <param name="exDataTableList">��չ DataTable List ���ڵ���������������DataTable�������Ͳ�һ��,���ֶ���ͬһ��ʱʹ��,Ҫ���ʽ�������һ�� DataTable�������ֶ���һ��,���ֶ����Ϳɲ�ͬ</param>
        /// <returns>bool</returns>
        public static bool ToExcelForDataTableE(System.Data.DataTable dt, string excelPathName, string pathType, List<string> excludeColumn, string excludeType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> colName = new List<string>();
            return ToExcelForDataTable(dt, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }
        /// <summary>
        /// ��DataTable����Excel��ʹ��Ĭ�����������ų������κ���
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">��Excel���Ƶı���·�� ��pathType��1ʱ��Ч�������븳ֵ���ַ���</param>
        /// <param name="pathType">·�����͡�ֻ��ȡֵ��0�ͻ��Զ���·����1����˶���·������ʶ�ļ�����·���Ƿ����ָ�����ǿͻ��Զ���·�����ļ���</param>
        /// <param name="sheetName">sheet1������ Ϊ���ַ���ʱ����Ĭ������</param>
        /// <param name="TemplatePath">ģ������Ŀ��������·�� Ϊ���ַ���ʱ��ʾ��ģ��</param>
        /// <param name="TemplateRow">ģ�����Ѵ������ݵ���������ģ��ʱ�봫����� 0</param>
        /// <param name="exDataTableList">��չ DataTable List ���ڵ���������������DataTable�������Ͳ�һ��,���ֶ���ͬһ��ʱʹ��,Ҫ���ʽ�������һ�� DataTable�������ֶ���һ��,���ֶ����Ϳɲ�ͬ</param>
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
        /// ��DataTable����Excel,ָ���б���,ָ��Ҫ�ų�����
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">��Excel���Ƶı���·�� ��pathType��1ʱ��Ч�������븳ֵ���ַ���</param>
        /// <param name="pathType">·�����͡�ֻ��ȡֵ��0�ͻ��Զ���·����1����˶���·������ʶ�ļ�����·���Ƿ����ָ�����ǿͻ��Զ���·�����ļ���</param>
        /// <param name="colName">���е�����List string </param>
        /// <<param name="excludeColumn">Ҫ��ʾ/�ų�����</param>
        /// <param name="excludeType">��ʾ/�ų��з�ʽ 0Ϊ������ 1ָ����ΪҪ��ʾ���� 2ָ����ΪҪ�ų�����</param>
        /// <param name="sheetName">sheet1������ Ϊ���ַ���ʱ����Ĭ������</param>
        /// <param name="TemplatePath">ģ������Ŀ��������·�� Ϊ���ַ���ʱ��ʾ��ģ��</param>
        /// <param name="TemplateRow">ģ�����Ѵ������ݵ���������ģ��ʱ�봫����� 0</param>
        /// <param name="exDataTableList">��չ DataTable List ���ڵ���������������DataTable�������Ͳ�һ��,���ֶ���ͬһ��ʱʹ��,Ҫ���ʽ�������һ�� DataTable�������ֶ���һ��,���ֶ����Ϳɲ�ͬ</param>
        /// <returns>bool</returns>
        public static bool ToExcelForModelList<T>(List<T> md, string excelPathName, string pathType, List<string> colName, List<string> excludeColumn, string excludeType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            System.Data.DataTable dt = ModelListToDataTable(md);
            return ToExcelForDataTable(dt, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }

        /// <summary>
        /// ��DataTable����Excel,ָ���б���
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">��Excel���Ƶı���·�� ��pathType��1ʱ��Ч�������븳ֵ���ַ���</param>
        /// <param name="pathType">·�����͡�ֻ��ȡֵ��0�ͻ��Զ���·����1����˶���·������ʶ�ļ�����·���Ƿ����ָ�����ǿͻ��Զ���·�����ļ���</param>
        /// <param name="colName">���е�����List string </param>
        /// <param name="sheetName">sheet1������ Ϊ���ַ���ʱ����Ĭ������</param>
        /// <param name="TemplatePath">ģ������Ŀ��������·�� Ϊ���ַ���ʱ��ʾ��ģ��</param>
        /// <param name="TemplateRow">ģ�����Ѵ������ݵ���������ģ��ʱ�봫����� 0</param>
        /// <param name="exDataTableList">��չ DataTable List ���ڵ���������������DataTable�������Ͳ�һ��,���ֶ���ͬһ��ʱʹ��,Ҫ���ʽ�������һ�� DataTable�������ֶ���һ��,���ֶ����Ϳɲ�ͬ</param>
        /// <returns>bool</returns>
        public static bool ToExcelForModelListC<T>(List<T> md, string excelPathName, string pathType, List<string> colName, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> excludeColumn = new List<string>();
            string excludeType = "0";
            return ToExcelForModelList(md, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }

        /// <summary>
        /// ��DataTable����Excel,ָ��Ҫ�ų�����
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">��Excel���Ƶı���·�� ��pathType��1ʱ��Ч�������븳ֵ���ַ���</param>
        /// <param name="pathType">·�����͡�ֻ��ȡֵ��0�ͻ��Զ���·����1����˶���·������ʶ�ļ�����·���Ƿ����ָ�����ǿͻ��Զ���·�����ļ���</param>
        /// <param name="excludeColumn">Ҫ��ʾ/�ų�����</param>
        /// <param name="excludeType">��ʾ/�ų��з�ʽ 0Ϊ������ 1ָ����ΪҪ��ʾ���� 2ָ����ΪҪ�ų�����</param>
        /// <param name="sheetName">sheet1������ Ϊ���ַ���ʱ����Ĭ������</param>
        /// <param name="TemplatePath">ģ������Ŀ��������·�� Ϊ���ַ���ʱ��ʾ��ģ��</param>
        /// <param name="TemplateRow">ģ�����Ѵ������ݵ���������ģ��ʱ�봫����� 0</param>
        /// <param name="exDataTableList">��չ DataTable List ���ڵ���������������DataTable�������Ͳ�һ��,���ֶ���ͬһ��ʱʹ��,Ҫ���ʽ�������һ�� DataTable�������ֶ���һ��,���ֶ����Ϳɲ�ͬ</param>
        /// <returns>bool</returns>
        public static bool ToExcelForModelListE<T>(List<T> md, string excelPathName, string pathType, List<string> excludeColumn, string excludeType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> colName = new List<string>();
            return ToExcelForModelList(md, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }

        /// <summary>
        /// ��DataTable����Excel��ʹ��Ĭ�����������ų������κ���
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="excelPathName">��Excel���Ƶı���·�� ��pathType��1ʱ��Ч�������븳ֵ���ַ���</param>
        /// <param name="pathType">·�����͡�ֻ��ȡֵ��0�ͻ��Զ���·����1����˶���·������ʶ�ļ�����·���Ƿ����ָ�����ǿͻ��Զ���·�����ļ���</param>
        /// <param name="sheetName">sheet1������ Ϊ���ַ���ʱ����Ĭ������</param>
        /// <param name="TemplatePath">ģ������Ŀ��������·�� Ϊ���ַ���ʱ��ʾ��ģ��</param>
        /// <param name="TemplateRow">ģ�����Ѵ������ݵ���������ģ��ʱ�봫����� 0</param>
        /// <param name="exDataTableList">��չ DataTable List ���ڵ���������������DataTable�������Ͳ�һ��,���ֶ���ͬһ��ʱʹ��,Ҫ���ʽ�������һ�� DataTable�������ֶ���һ��,���ֶ����Ϳɲ�ͬ</param>
        /// <returns>bool</returns>
        public static bool ToExcelForModelListZ<T>(List<T> md, string excelPathName, string pathType, string sheetName, string TemplatePath, int TemplateRow, List<System.Data.DataTable> exDataTableList)
        {
            List<string> colName = new List<string>();
            List<string> excludeColumn = new List<string>();
            string excludeType = "0";
            return ToExcelForModelList(md, excelPathName, pathType, colName, excludeColumn, excludeType, sheetName, TemplatePath, TemplateRow, exDataTableList);
        }
        #endregion

        #region ��DataTable����Excel�� ToExcelModelʵ�崫��
        /// <summary>
        /// ��DataTable����Excel�� ToExcelModelʵ�崫��
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
        /// ʵ����ת����DataTable
        /// </summary>
        /// <param name="modelList">ʵ�����б�</param>
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

        #region ˵�� ���ʹ��
        /*
         * ���ܣ�
         *      1����System.Data.DataTable���ݵ�����Excel�ļ�
         *      2����Model(Entity)����ʵ�嵼����Excel�ļ�
         * �������ã�
         *      1��ExcelHelper.ToExcelForDataTable(DataTable,excelPathName,pathType,colName,excludeColumn,excludeType,sheetName,TemplatePath,TemplateRow,exDataTableList);
         *      2��ExcelHelper.ToExcelForModelList(Model,excelPathName,pathType,colName,excludeColumn,excludeType,sheetName,TemplatePath,TemplateRow,exDataTableList);
         * ����˵����
         *      1��DataTable��DataSet.DataTable[0];���ݱ�
         *      2��Model��Model.Users users = new Model.Users(){...};����ʵ��
         *      3��excelPathName����Excel���Ƶı���·�� ��pathType��1ʱ��Ч���û��Զ��屣��·��ʱ�븳ֵ���ַ��� ""����ʽ��"E://456.xlsx"
         *      4��pathType��·�����͡�ֻ��ȡֵ��0�û��Զ���·���������û�ѡ��·���Ի���1����˶���·������ʶ�ļ�����·���Ƿ����ָ�����ǿͻ��Զ���·�����ļ�������excelPathName��������
         *      5��colName�����е��б���List string�����磺�ֶ���ΪuserName���˴���ָ��Ϊ"�û���"�����Դ���ʾ
         *      6��excludeColumn��Ҫ��ʾ/�ų����У�ָ����Щ��������ʾ����ָ����Щ�����ڲ���ʾ��������Щ������ʾ���ǲ���ʾ����excludeType��������
         *      7��excludeType����ʾ/�ų��з�ʽ�� 0Ϊ��ʾ������ 1ָ������Ҫ��ʾ���� 2ָ������Ҫ�ų����У���excludeColumn����
         *      8��sheetName��sheet1�����ƣ�Ҫʹ�ڱ���Ĭ��������ָ��Ϊ���ַ��� ""
         *      9��TemplatePath��ģ������Ŀ��������·�� ��:tp.xlsx ����Ϊ���ַ��� "" ʱ��ʾ��ģ��
         *      10��TemplateRow��ģ�����Ѵ������ݵ���������TemplatePath���ã���ģ��ʱ�봫����� 0
         *      11��exDataTableList����չ DataTable List ���ڵ���������������DataTable�������Ͳ�һ��,���ֶ���ͬһ��ʱʹ��,Ҫ���ʽ�������һ�� DataTable�������ֶ���һ��,���ֶ����Ϳɲ�ͬ
         * ע�⣺
         *      1��exDataTableList����Ϊһ��List<System.Data.DataTable> ���ϣ�������Ϊ Model ʱ�����ȵ��� ExcelHelper.ModelListToDataTable(System.Data.DataTable dt)��ModelתΪSystem.Data.DataTable
         */
        #endregion
        #endregion

        #region ��Excel�������ݵ� Ms Sql
        /// <summary>
        /// ��Excel�������ݵ� Ms Sql
        /// </summary>
        /// <param name="excelFile">Excel�ļ�·��(���ļ���)</param>
        /// <param name="sheetName">sheet��</param>
        /// <param name="DbTableName">�洢�����ݿ��е����ݿ������</param>
        /// <param name="columnType">��Ӧ�����������ͣ����Ϊnull����ΪĬ�����ͣ�double,nvarchar(500),datetime</param>
        /// <param name="connectionString">�����ַ���</param>
        /// <returns></returns>
        public static bool FromExcel(string excelFile, string sheetName, string DbTableName, List<string> columnType, string connectionString)
        {
            DataSet ds = new DataSet();
            try
            {
                //��ȡȫ������   
                //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + excelFile + ";" + "Extended Properties=Excel 8.0;";
                string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + excelFile + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'"; //����

                #region ֪ʶ��չ
                //HDR=Yes�������һ���Ǳ��⣬����Ϊ����ʹ�á�HDR=NO�����ʾ��һ�в��Ǳ��⣬��Ϊ������ʹ�á�ϵͳĬ�ϵ���YES
                //IMEX=0 ֻ��ģʽ
                //IMEX=1 д��ģʽ
                //IMEX=2 �ɶ�дģʽ
                #endregion

                #region ����ִ��
                using (OleDbConnection conn = new OleDbConnection(strConn))
                {
                    conn.Open();
                    string strExcel = "";
                    OleDbDataAdapter myCommand = null;
                    strExcel = string.Format("select * from [{0}$]", sheetName);
                    myCommand = new OleDbDataAdapter(strExcel, strConn);
                    myCommand.Fill(ds, sheetName);

                    #region ���ݿ���Ƿ���ڵ� T-SQL ������׼��
                    //���Ŀ��������򴴽�   
                    string strSql = string.Format("if object_id('{0}') is null create table {0}(", DbTableName != "" ? DbTableName : sheetName);
                    if (columnType != null && columnType.Count > 0)
                    {
                        #region �ֶ�ָ����ÿ���ֶε���������
                        //ָ�����ݸ�ʽ,Ҫ��һһ��Ӧ
                        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                        {
                            System.Data.DataColumn c = ds.Tables[0].Columns[i];
                            strSql += string.Format("[{0}] {1},", c.ColumnName, columnType[i]);
                        }
                        #endregion
                    }
                    else
                    {
                        #region ʹ��Ĭ����������
                        foreach (System.Data.DataColumn c in ds.Tables[0].Columns)
                        {
                            //ʹ��Ĭ�ϸ�ʽ��ֻ��double,DateTime,String��������
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

                    #region ִ�� T-SQL ������ݿ���������½�������������½�
                    using (System.Data.SqlClient.SqlConnection sqlconn = new System.Data.SqlClient.SqlConnection(connectionString))
                    {
                        sqlconn.Open();
                        System.Data.SqlClient.SqlCommand command = sqlconn.CreateCommand();
                        command.CommandText = strSql;
                        command.ExecuteNonQuery();
                        sqlconn.Close();
                    }
                    #endregion

                    #region �����ݿ���������
                    using (System.Data.SqlClient.SqlBulkCopy sbc = new System.Data.SqlClient.SqlBulkCopy(connectionString))
                    {
                        sbc.SqlRowsCopied += new System.Data.SqlClient.SqlRowsCopiedEventHandler(bcp_SqlRowsCopied);
                        sbc.BatchSize = 100;//ÿ�δ��������   
                        sbc.NotifyAfter = 100;//������ʾ������   
                        sbc.DestinationTableName = DbTableName != "" ? DbTableName : sheetName;//���ݿ��������
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

        #region ������ʾ
        /// <summary>
        /// ������ʾ
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
        #region ToExcelModel�Զ�����
        /// <summary>
        /// ���ݱ�
        /// </summary>
        public System.Data.DataTable DataTable { get; set; }
        /// <summary>
        /// ��Excel���Ƶı���·�� ��pathType��1ʱ��Ч���û��Զ��屣��·��ʱ�븳ֵ���ַ��� ""����ʽ��"E://456.xlsx"
        /// </summary>
        public string excelPathName { get; set; }
        /// <summary>
        /// ·�����͡�ֻ��ȡֵ��0�û��Զ���·���������û�ѡ��·���Ի���1����˶���·������ʶ�ļ�����·���Ƿ����ָ�����ǿͻ��Զ���·�����ļ�������excelPathName��������
        /// </summary>
        public string pathType { get; set; }
        /// <summary>
        /// ���е��б���List string�����磺�ֶ���ΪuserName���˴���ָ��Ϊ"�û���"�����Դ���ʾ
        /// </summary>
        public List<string> colNameList { get; set; }
        /// <summary>
        /// Ҫ��ʾ/�ų����У�ָ����Щ��������ʾ����ָ����Щ�����ڲ���ʾ��������Щ������ʾ���ǲ���ʾ����excludeType��������
        /// </summary>
        public List<string> excludeColumn { get; set; }
        /// <summary>
        /// ��ʾ/�ų��з�ʽ�� 0Ϊ��ʾ������ 1ָ������Ҫ��ʾ���� 2ָ������Ҫ�ų����У���excludeColumn����
        /// </summary>
        public string excludeType { get; set; }
        /// <summary>
        /// sheet1�����ƣ�Ҫʹ�ڱ���Ĭ��������ָ��Ϊ���ַ��� ""
        /// </summary>
        public string sheetName { get; set; }
        /// <summary>
        /// ģ������Ŀ��������·�� ��:tp.xlsx ����Ϊ���ַ��� "" ʱ��ʾ��ģ��
        /// </summary>
        public string TemplatePath { get; set; }
        /// <summary>
        /// ģ�����Ѵ������ݵ���������TemplatePath���ã���ģ��ʱ�봫����� 0
        /// </summary>
        public int TemplateRow { get; set; }
        /// <summary>
        /// ��չ DataTable List ���ڵ���������������DataTable�������Ͳ�һ��,���ֶ���ͬһ��ʱʹ��,Ҫ���ʽ�������һ�� DataTable�������ֶ���һ��,���ֶ����Ϳɲ�ͬ
        /// </summary>
        public List<System.Data.DataTable> exDataTableList { get; set; }
        #endregion
    }
    public class FromExcelModel
    {
        /// <summary>
        /// Excel�ļ�·��(���ļ���)
        /// </summary>
        public string excelFile { get; set; }
        /// <summary>
        /// sheet��<
        /// </summary>
        public string sheetName { get; set; }
        /// <summary>
        /// �洢�����ݿ��е����ݿ������
        /// </summary>
        public string DbTableName { get; set; }
        /// <summary>
        /// ��Ӧ�����������ͣ����Ϊnull����ΪĬ�����ͣ�double,nvarchar(500),datetime
        /// </summary>
        public List<string> columnTypeList { get; set; }
        /// <summary>
        /// �����ַ��� server=serverip;database=databasename;uid=username;pwd=password;
        /// </summary>
        public string connectionString { get; set; }
    }
}
/*
        //�����ܹ���DataTableֱ�ӵ���ΪExcel�ļ�����List<Model> Model����ʵ�弯�ϵ�����Excel�ļ�,�����ṩ��ֱ�ӽ�Excel���ݵ��������ݿ�ķ��������Ƿǳ�ʵ�õģ���������������ʾ����ע��ʵ�ʹ�����Model���ֶ���Щ�����Լ�����ģ�������ֻ��һ���ֺ��ĵ��ô��룺
        /// <summary>
        /// ������Excel
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
                "�û�ID",
                "�û���",
                "��ע",
                "�û�IP",
                "����ʱ��",
                "����ʱ��1",
                "����ʱ��2"
                };
                List<string> tt = new List<string>() { "action_type" };
                DataSet dss = bll.GetList(10, "", "id");
                List<System.Data.DataTable> dtss = new List<System.Data.DataTable>();
                dtss.Add(dss.Tables[0]);
                dtss.Add(dss.Tables[0]);
                dtss.Add(dss.Tables[0]);
                ExcelHelper.ToExcelForDataTable(dt, Server.MapPath("~").ToString() + "456.xlsx", "0", colName, tt, "2", "", "", 0, dtss);//ָ�����б�����ָ����Ҫ�ų�����
                ToExcelModel tem = new ToExcelModel()
                {
                    DataTable = dt,
                    excelPathName = "",
                    pathType = "0",
                    colNameList = colName,
                    excludeColumn = tt,
                    excludeType = "0",
                    sheetName = "�ɹ�",
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
        /// �������ݵ����ݿ�
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