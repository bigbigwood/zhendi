using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.IO;
using System.Collections;
using System.Reflection;

namespace Rld.Acs.WpfApplication.Service
{
    public class ExcelOperation<T>
    {
        #region NPOI EXCEL2003
        #region NPOI 导出Excel DataTable
        /// <summary>
        /// 导出Excel DataTable
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="fileName">文件名</param>
        /// <param name="sheetName">工作簿名称</param>
        public static void ExportDataTableToExcel(DataTable sourceTable, string fileName, string sheetName,SortedList sortlist=null)
        {
            MemoryStream ms = null;
            if (sortlist == null)
                ms = GetDatasourceAsStream(sourceTable, sheetName) as MemoryStream;
            else
                ms = GetDatasourceAsStream(sourceTable, sheetName,sortlist) as MemoryStream;

            using (FileStream file = new FileStream(fileName, FileMode.Create, System.IO.FileAccess.Write))
            {
                ms.WriteTo(file);
            }

            //HttpContext.Current.Response.Charset = "GB2312";
            //HttpContext.Current.Response.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}.xls", fileName));
            //HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            //HttpContext.Current.Response.End();
            ms.Close();
            ms = null;
        }
        /// <summary>
        /// NPOI 导出Excel DataTable
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="sheetName">工作簿名称</param>
        private static Stream GetDatasourceAsStream(DataTable sourceTable, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();

            int rowIndex = 0;
            int sheetcount=1;
            ISheet sheet = workbook.CreateSheet(String.Format("{0}【{1}】", sheetName, sheetcount));
            CreateHeaderRow(sheet, sourceTable);

            foreach (DataRow row in sourceTable.Rows)
            {
                rowIndex++;
                if (rowIndex == 65536)//新建一个sheet
                {
                    sheetcount++;
                    sheet = workbook.CreateSheet(String.Format("{0}【{1}】", sheetName, sheetcount));
                    CreateHeaderRow(sheet, sourceTable);
                    rowIndex = 1;
                }
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in sourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            workbook = null;
            return ms;
        }
        /// <summary>
        /// 添加列头
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="sourceTable"></param>
        private static void CreateHeaderRow(ISheet sheet, DataTable sourceTable)
        {
            int cellIndex = 0;
            IRow headerRow = sheet.CreateRow(0);
            foreach (DataColumn column in sourceTable.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                cellIndex++;
            }
        }
        /// <summary>
        ///  NPOI 导出Excel DataTable
        /// </summary>
        /// <param name="sourceTable">导出数据源</param>
        /// <param name="sheetName">工作簿名称</param>
        /// <param name="sortlist">列头集合</param>
        /// <returns></returns>
        private static Stream GetDatasourceAsStream(DataTable sourceTable, string sheetName, SortedList sortlist)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet(sheetName);
            int rowIndex = 0;

            IRow headerRow = sheet.CreateRow(rowIndex);

            #region 数据
            bool flag = false;
            foreach (DataRow row in sourceTable.Rows)
            {
                rowIndex++;
                IRow dataRow = sheet.CreateRow(rowIndex);
                int i = 0;
                foreach (DictionaryEntry dic in sortlist)
                {
                    foreach (DataColumn column in sourceTable.Columns)
                    {
                        if (dic.Key.ToString().ToUpper() == column.ColumnName.ToUpper())
                        {
                            if (!flag)
                            {
                                headerRow.CreateCell(i).SetCellValue(dic.Value.ToString());
                            }
                            ICell cell = dataRow.CreateCell(i);
                            SetCellType(cell, row, column);
                        }
                    }
                    i++;
                }
                flag = true;
            }
            #endregion
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            headerRow = null;
            sheet = null;
            workbook = null;
            return ms;
        }
        #endregion

        #region NPOI 导出Excel List<T>
        /// <summary>
        /// 导出Excel List<T>
        /// </summary>
        /// <param name="sourceList"></param>
        /// <param name="ht">列名</param>
        /// <param name="sheetName">工作簿名称</param>
        /// <param name="fileName">文件名</param>
        public static void ExportListToExcel(List<T> sourceList, SortedList sortlist, string fileName, string sheetName)
        {
            //MemoryStream ms = getExcel(sourceList, sortlist, sheetName) as MemoryStream;
            //HttpContext.Current.Response.Charset = "GB2312";
            //HttpContext.Current.Response.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}.xls", System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8)));
            //HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            ////HttpContext.Current.Response.End();
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
            //ms.Close();
            //ms = null;
        }

        /// <summary>
        /// 导出Excel List<T>
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="head">列名</param>
        /// <param name="sheetName">工作簿名称</param>
        public static Stream getExcel(List<T> lists, SortedList sortlist, string sheetName)
        {

            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = workbook.CreateSheet(sheetName) as HSSFSheet;
            HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
            bool h = false;
            int j = 1;
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            foreach (T item in lists)
            {
                HSSFRow dataRow = sheet.CreateRow(j) as HSSFRow;
                int i = 0;
                foreach (DictionaryEntry de in sortlist)//让SortedList集合中的每一个列去匹配List实体中的值
                {
                    foreach (PropertyInfo column in properties)
                    {
                        if (!h)
                        {
                            if (de.Key.ToString() == column.Name)
                            {
                                headerRow.CreateCell(i).SetCellValue(de.Value.ToString());
                                //headerRow.CreateCell(i).SetCellValue(sortlist[column.Name] == null ? column.Name : sortlist[column.Name].ToString());
                                dataRow.CreateCell(i).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                            }
                        }
                        else
                        {
                            if (de.Key.ToString() == column.Name)
                            {
                                dataRow.CreateCell(i).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                            }
                        }
                    }
                    i++;
                }
                h = true;
                j++;
            }
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }
        #endregion
        
        #region 导出到Excel文件
        /// <summary>
        /// 导出到Excel文件
        /// </summary>
        public static void CreateExcel(DataTable dt, string FileName)
        {
            //HttpResponse resp = HttpContext.Current.Response;
            //resp.Clear();
            //resp.Charset = "GB2312";
            ////resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            ////resp.ContentEncoding = Encoding.UTF8;
            ////resp.AppendHeader("Content-Disposition", "attachment;filename=" + HttpContext.Current.Server.UrlEncode(FileName) + ".xls");
            //resp.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}.xls", FileName));
            //resp.ContentType = "application/ms-excel";

            //string colHeaders = "", ls_item = "";

            ////定义表对象与行对象，同时用DataSet对其值进行初始化 
            ////System.Data.DataTable dt = ds.Tables[0];
            //DataRow[] myRow = dt.Select();//可以类似dt.Select("id>10")之形式达到数据筛选目的

            //int i = 0;
            //int cl = dt.Columns.Count;

            ////取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符 
            //for (i = 0; i < cl; i++)
            //{
            //    if (i == (cl - 1))//最后一列，加\n
            //    {
            //        colHeaders += dt.Columns[i].Caption + "\n";
            //    }
            //    else
            //    {
            //        colHeaders += dt.Columns[i].Caption + "\t";
            //    }
            //}
            //resp.Write(colHeaders);
            ////向HTTP输出流中写入取得的数据信息 

            ////逐行处理数据   
            //foreach (DataRow row in myRow)
            //{
            //    //当前行数据写入HTTP输出流，并且置空ls_item以便下行数据     
            //    for (i = 0; i < cl; i++)
            //    {
            //        if (i == (cl - 1))//最后一列，加\n
            //        {
            //            ls_item += row[i] + "\n";
            //        }
            //        else
            //        {
            //            ls_item += row[i] + "\t";
            //        }
            //    }
            //    resp.Write(ls_item);
            //    ls_item = "";
            //}
            //resp.End();
        }
        #endregion
        #endregion

        #region NPOI EXCEL2007
        /// <summary>
        /// 导出Excel2007 DataTable
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="fileName">文件名</param>
        /// <param name="sheetName">工作簿名称</param>
        public static void ExportDataTableToExcel2007(DataTable sourceTable, string fileName, string sheetName, SortedList sortlist = null)
        {
            MemoryStream ms = null;
            if (sortlist == null)
                ms = GetDatasourceAsStream2007(sourceTable, sheetName) as MemoryStream;
            else
                ms = GetDatasourceAsStream2007(sourceTable, sheetName, sortlist) as MemoryStream;

            using (FileStream file = new FileStream(fileName, FileMode.Create, System.IO.FileAccess.Write))
            {
                ms.WriteTo(file);
            }

            //HttpContext.Current.Response.Charset = "GB2312";
            //HttpContext.Current.Response.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}.xlsx", fileName));
            //HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            //HttpContext.Current.Response.End();
            ms.Close();
            ms = null;
        }

        /// <summary>
        ///  NPOI 导出Excel2007 DataTable
        /// </summary>
        /// <param name="sourceTable">导出数据源</param>
        /// <param name="sheetName">工作簿名称</param>
        /// <param name="sortlist">列头集合</param>
        /// <returns></returns>
        private static Stream GetDatasourceAsStream2007(DataTable sourceTable, string sheetName, SortedList sortlist)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet(sheetName);
            int rowIndex = 0;

            IRow headerRow = sheet.CreateRow(rowIndex);

            #region 数据
            bool flag = false;
            foreach (DataRow row in sourceTable.Rows)
            {
                rowIndex++;
                IRow dataRow = sheet.CreateRow(rowIndex);
                int i = 0;
                foreach (DictionaryEntry dic in sortlist)
                {
                    foreach (DataColumn column in sourceTable.Columns)
                    {
                        if (dic.Key.ToString().ToUpper() == column.ColumnName.ToUpper())
                        {
                            if (!flag)
                            {
                                headerRow.CreateCell(i).SetCellValue(dic.Value.ToString());
                            }
                            ICell cell = dataRow.CreateCell(i);
                            SetCellType(cell, row, column);
                        }
                    }
                    i++;
                }
                flag = true;
            }
            #endregion
            ms.Position = 0;
            workbook.Write(ms);
            ms.Flush();
            headerRow = null;
            sheet = null;
            workbook = null;
            return ms;
        }

        /// <summary>
        /// NPOI 导出Excel2007 DataTable
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="sheetName">工作簿名称</param>
        private static Stream GetDatasourceAsStream2007(DataTable sourceTable, string sheetName)
        {
            XSSFWorkbook workbook = new XSSFWorkbook ();
            MemoryStream ms = new MemoryStream();

            int rowIndex = 0;
            int sheetcount = 1;
            ISheet sheet = workbook.CreateSheet(String.Format("{0}【{1}】", sheetName, sheetcount));
            CreateHeaderRow2007(sheet, sourceTable);

            foreach (DataRow row in sourceTable.Rows)
            {
                rowIndex++;
                if (rowIndex == 1000000)//新建一个sheet
                {
                    sheetcount++;
                    sheet = workbook.CreateSheet(String.Format("{0}【{1}】", sheetName, sheetcount));
                    CreateHeaderRow2007(sheet, sourceTable);
                    rowIndex = 1;
                }
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in sourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
            }
            ms.Position = 0;
            workbook.Write(ms);
            ms.Flush();
            sheet = null;
            workbook = null;
            return ms;
        }
        /// <summary>
        /// 添加列头
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="sourceTable"></param>
        private static void CreateHeaderRow2007(ISheet sheet, DataTable sourceTable)
        {
            int cellIndex = 0;
            IRow headerRow = sheet.CreateRow(0);
            foreach (DataColumn column in sourceTable.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                cellIndex++;
            }
        }
        #endregion

        #region  List转换成DataTable
        /// <summary>
        /// 将泛类型集合List类转换成DataTable
        /// </summary>
        /// <param name="list">泛类型集合</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {

            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }

            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }

            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);

                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
        #endregion

        /// <summary>
        /// 设置导出EXCEL单元格格式
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        private static void SetCellType(ICell cell, DataRow row,DataColumn column)
        {
            switch (column.DataType.ToString())
            {
                case "System.String":
                    cell.SetCellValue(row[column].ToString() ?? "");
                    break;
                case "System.DateTime":
                    DateTime dateV;
                    DateTime.TryParse(row[column].ToString(), out dateV);
                    cell.SetCellValue(dateV);
                    break;
                case "System.Boolean":
                    bool boolV = false;
                    bool.TryParse(row[column].ToString(), out boolV);
                    cell.SetCellValue(boolV);
                    break;
                case "System.Int16"://整型   
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(row[column].ToString(), out intV);
                    cell.SetCellValue(intV);
                    break;
                case "System.Decimal"://浮点型   
                case "System.Double":
                    double doubV = 0;
                    double.TryParse(row[column].ToString(), out doubV);
                    cell.SetCellValue(doubV);
                    break;
                case "System.DBNull"://空值处理   
                    cell.SetCellValue("");
                    break;
                default:
                    cell.SetCellValue("");
                    break;
            }
        }
    }
}