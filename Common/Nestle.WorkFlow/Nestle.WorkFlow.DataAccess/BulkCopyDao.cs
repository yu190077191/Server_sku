using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;

namespace Nestle.WorkFlow.DataAccess
{
    public class BulkCopyDao : BaseDao
    {
        #region 读取Excel

        public static List<T> ConvertToList<T>(string filePath, Object model, int minColumnCount, out string error, out string columnNames)
        {
            columnNames = string.Empty;
            Dictionary<string, int> columnDict = null;
            DataTable dataTable = GetFirstSheetFromExcel(filePath);
            string modelName = typeof(T).Name;
            string xmlString = string.Empty;
            StringBuilder sb = new StringBuilder();
            error = string.Empty;

            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (System.Reflection.PropertyInfo p in model.GetType().GetProperties())
            {
                dict.Add(p.Name, -1);
            }

            int startRowIndex = 1;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                columnDict = GetColumnIndex(dict, dataTable.Columns, dataTable.Columns.Count, minColumnCount, out columnNames);
                if (columnDict == null)
                {
                    dataTable = GetFirstSheetFromExcel(filePath, false);
                    startRowIndex = 0;
                }
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                int maxBlankRowCount = 50;
                int currentBlankRowCount = 0;
                for (int r = 0; r < dataTable.Rows.Count; r++)
                {
                    if (columnDict == null)
                    {
                        columnDict = GetColumnIndex(dict, dataTable.Rows[r], dataTable.Columns.Count, minColumnCount, out columnNames);
                    }
                    else
                    {
                        bool isBlankRow = true;
                        StringBuilder rowSb = new StringBuilder();
                        foreach (KeyValuePair<string, int> kv in columnDict)
                        {
                            string value = TranslateXmlString(Convert.ToString(dataTable.Rows[r][kv.Value]).Trim());
                            if (kv.Key == "Budget")
                            {
                            }

                            rowSb.Append("<" + kv.Key + ">" + value.Trim() + "</" + kv.Key + ">");
                            if (!string.IsNullOrEmpty(value))
                            {
                                isBlankRow = false;
                            }
                        }

                        if (isBlankRow)
                        {
                            currentBlankRowCount++;
                            if (currentBlankRowCount >= maxBlankRowCount)
                            {
                                break;
                            }
                        }
                        else
                        {
                            sb.Append("<" + modelName + ">");
                            sb.Append("<RowIndex>" + (r + 1 + startRowIndex).ToString() + "</RowIndex>");
                            sb.Append(rowSb.ToString());
                            sb.Append("</" + modelName + ">");
                        }
                    }
                }
            }

            xmlString = sb.ToString();
            if (!string.IsNullOrEmpty(xmlString))
            {
                xmlString = "<ArrayOf" + modelName + ">" + xmlString + "</ArrayOf" + modelName + ">";
                List<T> li = new List<T>();
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//" + modelName);
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<T>(node.OuterXml));
                    }
                }

                return li;
            }

            return null;
        }

        #region XmlCleaner
        public static string TranslateXmlString(string xmlStr)
        {
            xmlStr = xmlStr.Replace("&", "&amp;");
            xmlStr = xmlStr.Replace(">", "&gt;");
            xmlStr = xmlStr.Replace("<", "&lt;");
            xmlStr = xmlStr.Replace("\"", "&quot;");
            xmlStr = xmlStr.Replace("'", "&apos;");
            return xmlStr;
        }

        /// <summary>
        /// IsW3CCompliant
        /// http://w3c.org 
        /// The valid XML characters and character ranges (hex values) as defined by the W3C XML language specifications 1.0: 
        /// #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsW3CCompliant(char c)
        {
            int charInt = Convert.ToInt32(c);
            return charInt == 9 || charInt == 10 || charInt == 13 || (charInt >= 32 && charInt <= 55295) || (charInt >= 57344 && charInt <= 65533) || (charInt >= 65536 && charInt <= 1114111);
        }

        /// <summary>
        /// Scrub Xml ensures that each character is W3C compliant.
        /// This is a major performance hit. . .
        /// #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static string ScrubString(string xmlStr)
        {
            if (xmlStr == string.Empty || xmlStr == null)
            {
                return xmlStr;
            }

            string pattern = @"[^\w\.@-]";
            System.Text.StringBuilder strB = new System.Text.StringBuilder(xmlStr.Length);

            //-- If there are no special chars just return the original (99%)
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            if (!regex.Match(xmlStr).Success)
            {
                return xmlStr;
            }

            char[] charArray = xmlStr.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                if (IsW3CCompliant(charArray[i]))
                {
                    strB.Append(charArray[i]);
                }
            }

            return strB.ToString();
        }

        #endregion

        public static Dictionary<string, int> GetColumnIndex(Dictionary<string, int> dict, DataRow row, int columnCount, int minColumnCount, out string columnNames)
        {
            columnNames = string.Empty;
            Dictionary<string, int> outputDict = new Dictionary<string, int>();
            int matchedCount = 0;
            for (int c = 0; c < columnCount; c++)
            {
                string value = Convert.ToString(row[c]);

                foreach (KeyValuePair<string, int> kv in dict)
                {
                    if (IsMatched(kv.Key, value))
                    {
                        outputDict[kv.Key] = c;
                        matchedCount++;
                        if (!string.IsNullOrEmpty(columnNames))
                        {
                            columnNames += "|";
                        }

                        columnNames += value;
                    }
                }
            }

            if (matchedCount >= minColumnCount)
            {
                return outputDict;
            }

            return null;
        }

        public static Dictionary<string, int> GetColumnIndex(Dictionary<string, int> dict, DataColumnCollection columns, int columnCount, int minColumnCount, out string columnNames)
        {
            columnNames = string.Empty;
            Dictionary<string, int> outputDict = new Dictionary<string, int>();
            int matchedCount = 0;
            for (int c = 0; c < columns.Count; c++)
            {
                string value = columns[c].ColumnName;

                foreach (KeyValuePair<string, int> kv in dict)
                {
                    if (IsMatched(kv.Key, value))
                    {
                        outputDict[kv.Key] = c;
                        matchedCount++;
                        if (!string.IsNullOrEmpty(columnNames))
                        {
                            columnNames += "|";
                        }

                        columnNames += value;
                    }
                }
            }

            if (matchedCount >= minColumnCount)
            {
                return outputDict;
            }

            return null;
        }

        public static string[] SplitPropertyName(string propertyName)
        {
            if (!propertyName.Contains("_"))
            {
                StringBuilder sb = new StringBuilder();
                bool hasUnCapitalizedLetter = false;
                for (int i = 0; i < propertyName.Length; i++)
                {
                    char c = propertyName[i];
                    if (i > 0)
                    {
                        if (hasUnCapitalizedLetter && c >= 'A' && c <= 'Z')
                        {
                            hasUnCapitalizedLetter = false;
                            sb.Append('_');
                        }
                        else if (c >= 'a' && c <= 'z')
                        {
                            hasUnCapitalizedLetter = true;
                        }
                    }

                    sb.Append(c);
                }

                propertyName = sb.ToString();
            }

            if (propertyName.Contains("_"))
            {
                return propertyName.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            }

            return propertyName.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool IsMatched(string propertyName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            if (propertyName.Equals(value, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            else if (propertyName.ToLower() == "id")
            {
                return false;
            }

            string[] array = SplitPropertyName(propertyName);
            value = value.ToLower();
            foreach (string s in array)
            {
                if (!value.Contains(s.ToLower()))
                {
                    return false;
                }
            }

            return true;
        }

        public static DataTable GetFirstSheetFromExcel(string excelFileName, bool firstRowIsHeaderRow = true)
        {
            DataTable dt = new DataTable();
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source='" + excelFileName + "';Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";

            if (!firstRowIsHeaderRow)
            {
                strConn = strConn.Replace("HDR=Yes", "HDR=No");
            }

            OleDbConnection objConn = new System.Data.OleDb.OleDbConnection(strConn);
            objConn.Open();
            try
            {
                DataTable schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                string sheetName = schemaTable.Rows[0][2].ToString().Trim();
                string strExcel = "select * from   [" + sheetName + "]";
                DataSet ds = new DataSet();
                OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, strConn);
                adapter.Fill(ds, sheetName);
                dt = ds.Tables[sheetName];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                objConn.Close();
                objConn.Dispose();
            }

            return dt;
        }

        public static string WriteToExcel(string filepath, DataTable dt)
        {
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0;IMEX=0';";
            OleDbConnection objConn = new System.Data.OleDb.OleDbConnection(strConn);

            try
            {
                objConn.Open();
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
                cmd.Connection = objConn;

                DataTable schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                string sheetName = schemaTable.Rows[0][2].ToString().Trim();
                string sqlstr = "";

                DataRow[] myRow = dt.Select();
                int cl = dt.Columns.Count;

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < cl; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append("'" + dt.Columns[i].Caption.ToString() + "'");
                }

                //sqlstr = "insert into [" + sheetName + "] values(" + sb.ToString() + ")";
                //cmd.CommandText = sqlstr;
                //cmd.ExecuteNonQuery();

                foreach (DataRow row in myRow)
                {
                    sb = new StringBuilder();
                    for (int i = 0; i < cl; i++)
                    {
                        if (i > 0)
                        {
                            sb.Append(",");
                        }

                        sb.Append("'" + row[i].ToString() + "'");
                    }

                    sqlstr = "insert into [" + sheetName + "] values(" + sb.ToString() + ")";
                    cmd.CommandText = sqlstr;
                    cmd.ExecuteNonQuery();
                }

                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                objConn.Close();
            }
        }

        /// <summary>
        /// 将 Excel 文件转成 DataTable 后,再把 DataTable中的数据写入表Products
        /// </summary>
        /// <param name="serverMapPathExcelAndFileName"></param>
        /// <param name="excelFileRid"></param>
        /// <returns></returns>
        public static int WriteExcelToDataBase(string excelFileName, string destinationTableName, ref string columnsSql)
        {
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "data source=" + excelFileName + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'"; //此连接可以操作.xls与.xlsx文件 (支持Excel2003 和 Excel2007 的连接字符串)
            int rowsCount = 0;
            OleDbConnection objConn = new System.Data.OleDb.OleDbConnection(strConn);
            objConn.Open();
            try
            {
                DataTable schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                string sheetName = string.Empty;
                for (int j = 0; j < schemaTable.Rows.Count; j++)
                {
                    sheetName = schemaTable.Rows[j][2].ToString().Trim();//获取 Excel 的表名，默认值是sheet1 
                    if (sheetName.LastIndexOf("_") == (sheetName.Length - 1) || sheetName.Contains("FilterDatabase"))//Only copy tables with standard names
                    {
                        continue;
                    }

                    DataTable excelDataTable = ExcelToDataTable(excelFileName, sheetName, true);
                    if (excelDataTable.Columns.Count > 1)
                    {
                        StringBuilder columns = new StringBuilder();
                        int columnCount = 0;
                        foreach (DataColumn col in excelDataTable.Columns)
                        {
                            columnCount++;
                            if (columnCount > 1)
                            {
                                columns.Append(",\r\n");
                            }

                            columns.Append("		[" + col.ColumnName.Trim() + "] ");
                            columns.Append(SqlDbTypeConvert.CSharpSystemTypeToSqlTypeString(col.DataType.ToString()));
                        }

                        columnsSql = columns.ToString();
                        int createTempTableResult = ExecuteNonQuery("CreateBulkCopyTableFromExcel", new[] { new SqlParameter("@TableName", destinationTableName), new SqlParameter("@Columns", columnsSql), new SqlParameter("@DropIfExists", true), new SqlParameter("@OperationBy", Operation.OperationBy) });

                        SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.UseInternalTransaction);
                        sqlbulkcopy.DestinationTableName = destinationTableName;//数据库中的表名

                        sqlbulkcopy.WriteToServer(excelDataTable);
                        sqlbulkcopy.Close();

                        break;//only upload the first unempty table
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                objConn.Close();
                objConn.Dispose();
            }
            return rowsCount;
        }
        /// <summary>
        /// 读取Excel
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataSet ExcelToDS(string Path, string sheetName)
        {
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "data source=" + Path + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
            DataSet ds = null;
            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                OleDbDataAdapter myCommand = null;
                try
                {
                    conn.Open();
                    string strExcel = "";
                    strExcel = "select * from [" + sheetName + "$]";
                    myCommand = new OleDbDataAdapter(strExcel, strConn);
                    ds = new DataSet();
                    myCommand.Fill(ds, "table1");
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    myCommand.Dispose();
                    conn.Close();
                }
                return ds;
            }
        }

        /// <summary>
        /// 将 Excel 文件转成 DataTable
        /// </summary>
        /// <param name="serverMapPathExcel">Excel文件及其路径</param>
        /// <param name="strSheetName">工作表名,如:Sheet1</param>
        /// <param name="isTitleOrDataOfFirstRow">True 第一行是标题,False 第一行是数据</param>
        /// <returns>DataTable</returns>
        public static DataTable ExcelToDataTable(string serverMapPathExcel, string strSheetName, bool isTitleOrDataOfFirstRow)
        {

            string HDR = string.Empty;//如果第一行是数据而不是标题的话, 应该写: "HDR=No;"
            if (isTitleOrDataOfFirstRow)
            {
                HDR = "YES";//第一行是标题
            }
            else
            {
                HDR = "NO";//第一行是数据
            }
            //源的定义 
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "data source=" + serverMapPathExcel + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
            //Sql语句
            //string strExcel = string.Format("select * from [{0}$]", strSheetName); 这是一种方法
            string strExcel = "select * from   [" + strSheetName + "]";
            //定义存放的数据表
            DataSet ds = new DataSet();

            //连接数据源
            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                try
                {
                    conn.Open();
                    //适配到数据源
                    OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, strConn);

                    adapter.Fill(ds, strSheetName);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return ds.Tables[strSheetName];
        }
        #endregion
    }
}
