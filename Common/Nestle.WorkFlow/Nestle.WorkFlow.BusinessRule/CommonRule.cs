using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Nestle.WorkFlow.BusinessRule
{
    public class CommonRule
    {
        public static string GetXmlString(string sJson)
        {
            return JsonHelper.GetXmlString(sJson);
        }

        public static string GetSafeTableName(string tableName)
        {
            return CommonDao.GetSafeTableName2(tableName);
        }

        public static string GetSafeTableName2(string tableName)
        {
            return CommonDao.GetSafeTableName2(tableName);
        }

        public static System.Data.DataTable GetDataTable(string procedureName, SqlParameter[] sqlParamters)
        {
            return CommonDao.GetDataTable(procedureName, sqlParamters);
        }

        public static string ExportToExcel(DataTable dt, string templateFileName, string fileNamePrefix = "NR_Report")
        {
            if (dt == null)
            {
                return "对不起，没有符合条件的记录！";
            }
            else
            {
                string fileName = fileNamePrefix + DateTime.Now.ToString().Replace("/", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty).Replace(":", string.Empty);

                string folder = System.Web.HttpContext.Current.Server.MapPath("~/Export_Files/" + DateTime.Now.ToShortDateString().Replace("/", "-"));
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string filePath = Path.Combine(folder, fileName + ".xls");
                if (System.IO.File.Exists(filePath))
                {
                    for (int i = 1; i < 1000; i++)
                    {
                        filePath = Path.Combine(folder, fileName + "-" + i.ToString() + ".xls");
                        if (!System.IO.File.Exists(filePath))
                        {
                            fileName = fileName + "-" + i.ToString();
                            break;
                        }
                    }
                }

                System.IO.File.Copy(System.Web.HttpContext.Current.Server.MapPath("~/" + templateFileName), filePath);
                string result = ExportToExcel(dt, filePath);
                if (result == "ok")
                {
                    return "Export_Files/" + DateTime.Now.ToShortDateString().Replace("/", "-") + "/" + fileName + ".xls";
                }

                return result;
            }
        }

        public static string ExportToExcel(DataTable dt, string savedFileName)
        {
            string result = string.Empty;

            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Persist Security Info=True;Data Source=" + savedFileName + ";Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=0';";
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(strConn);
            OleDbCommand cmd = null;
            try
            {
                conn.Open();

                DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                string sheetName = schemaTable.Rows[0][2].ToString().Trim();
                string strExcel = "SELECT * FROM [" + sheetName + "]";
                DataSet ds = new DataSet();
                OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, strConn);
                adapter.Fill(ds, sheetName);
                DataTable templateTbl = ds.Tables[sheetName];
                string sql = string.Empty;
                string valuesPara = string.Empty;

                foreach (DataColumn col in templateTbl.Columns)
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        sql += ", ";
                        valuesPara += ", ";
                    }

                    sql += "[" + col.ColumnName + "]";
                    valuesPara += "?";
                }

                sql = "INSERT INTO [" + sheetName + "] (" + sql + ") VALUES (" + valuesPara + ")";
                cmd = new OleDbCommand(sql, conn);

                int minColumnCount = templateTbl.Columns.Count;
                if (dt.Columns.Count < minColumnCount)
                {
                    minColumnCount = dt.Columns.Count;
                }

                int count = 0;
                foreach (DataRow row in dt.Rows)
                {
                    cmd.Parameters.Clear();
                    for (int c = 0; c < minColumnCount; c++)
                    {
                        cmd.Parameters.Add(templateTbl.Columns[c].ColumnName, row[c]);
                    }

                    if (count == 0)
                    {
                        if (templateTbl.Rows.Count == 1)
                        {
                            string updateFirstRow = string.Empty;
                            for (int c = 0; c < minColumnCount; c++)
                            {
                                if (c > 0)
                                {
                                    updateFirstRow += ", ";
                                }

                                updateFirstRow += "[" + templateTbl.Columns[c].ColumnName + "] = ?";
                            }

                            OleDbCommand cmdUpdateFirstRow = new OleDbCommand("UPDATE [" + sheetName + "] SET " + updateFirstRow, conn);
                            cmdUpdateFirstRow.Parameters.Clear();
                            for (int c = 0; c < minColumnCount; c++)
                            {
                                cmdUpdateFirstRow.Parameters.Add(templateTbl.Columns[c].ColumnName, row[c]);
                            }

                            cmdUpdateFirstRow.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }

                    count++;
                }

                result = "ok";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }

                conn.Close();
            }

            return result;
        }

        public static string ExportToCSV(string savedFileNamePrefix, string procedureName, SqlParameter[] sqlParamters)
        {
            DataTable dt = GetDataTable(procedureName, sqlParamters);
            if (dt != null && dt.Rows.Count > 0)
            {
                string savedFilePath = "Exported_Reports/" + savedFileNamePrefix + "_"
                    + DateTime.Now.ToShortDateString().Replace("/", "-") + "_"
                    + DateTime.Now.Hour.ToString().PadLeft(2, '0') + "："
                    + DateTime.Now.Minute.ToString().PadLeft(2, '0') + "："
                    + DateTime.Now.Second.ToString().PadLeft(2, '0') + ".csv";

                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Exported_Reports")))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Exported_Reports"));
                }

                string path = System.Web.HttpContext.Current.Server.MapPath("~/" + savedFilePath);
                StringBuilder sb = new StringBuilder();
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    if (c > 0)
                    {
                        sb.Append("\t");
                    }

                    sb.Append(dt.Columns[c].ColumnName);
                }

                int columnCount = dt.Columns.Count;
                string columnSplitBy = "\t";
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    sb.Append("\r\n");
                    for (int c = 0; c < columnCount; c++)
                    {
                        if (c > 0)
                        {
                            sb.Append(columnSplitBy);
                        }

                        if (!Convert.IsDBNull(dt.Rows[r][c]))
                        {
                            sb.Append(dt.Rows[r][c].ToString().Replace(columnSplitBy, string.Empty));
                        }
                    }
                }

                StreamWriter sw = new StreamWriter(path, true, Encoding.Unicode);
                sw.Write(sb.ToString());
                sw.Close();

                return savedFilePath;
            }
            else
            {
                return "对不起，没有相关记录！";
            }
        }

        public static T Get<T>(int? id = null)
        {
            if (id != null)
            {
                List<T> list = Get<T>("Get" + typeof(T).Name + "Proc", new[] { new SqlParameter("@Id", id), new SqlParameter("@CreatedBy", Operation.OperationBy) });
                if (list != null && list.Count > 0)
                {
                    return list[0];
                }
            }

            return default(T);
        }

        public static List<T> Get<T>(string procedureName, SqlParameter[] sqlParamters)
        {
            return CommonDao.Get<T>(procedureName, sqlParamters);
        }

        public static int Save<T>(string json)
        {
            return CommonDao.Save<T>(json);
        }

        public static string GetJsonFromModel(object obj)
        {
            string json = JsonHelper.SerializeJson(obj);
            string ignorablePart = ",\"RecordStatus\":";
            if (json.Contains(ignorablePart))
            {
                json = json.Substring(0, json.IndexOf(ignorablePart)) + " }";
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script id=\"jsonModelJs\" type=\"text/javascript\">\r\n");
            sb.Append("    var jsonModel = " + json + ";\r\n");
            sb.Append("</script>\r\n");
            return sb.ToString();
        }

        public static List<SelectOption> GetSelectOptionFromDictionary(int instanceId, string typeCode, int? id = null)
        {
            return Get<SelectOption>("GetSelectOptionFromDictionary", new[]{
                new SqlParameter("@InstanceId", instanceId),
                new SqlParameter("@TypeCode", typeCode),
                new SqlParameter("@id", id)
            });
        }

        public static string GetSelectOption(int instanceId, string typeCode, int? id = null, bool isDictionary = true, bool insertPleaseSelectOption = true)
        {
            List<SelectOption> list = new List<SelectOption>();
            if (isDictionary)
            {
                list = GetSelectOptionFromDictionary(instanceId, typeCode, id);
            }
            else
            {
                list = Get<SelectOption>("GetSelectOption", new[]{
                new SqlParameter("@InstanceId", instanceId),
                new SqlParameter("@TypeCode", typeCode),
                new SqlParameter("@id", id)
            });
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (insertPleaseSelectOption)
            {
                sb.Append("<option value=\"\">Select</option>\r\n");
            }

            foreach (SelectOption item in list)
            {
                sb.Append("<option value=\"" + item.Value + "\"" + (item.Checked ? " selected=\"selected\"" : string.Empty) + ">" + item.Text + "</option>\r\n");
            }

            return sb.ToString();
        }

    }
}
