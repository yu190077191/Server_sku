using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;

namespace Nestle.WorkFlow.DataAccess
{
    public class CommonDao : BaseDao
    {
        public static string GetSafeTableName2(string tableName)
        {
            tableName = tableName.Trim();
            string pattern = "[a-zA-Z0-9_]*";
            Match m = Regex.Match(tableName, pattern);
            if (m != null && m.Value == tableName)
            {
                return tableName;
            }

            return string.Empty;
        }

        public static List<T> Get<T>(string procedureName, SqlParameter[] sqlParamters, bool addSiteSettingAndOpertionBy = false)
        {
            List<T> li = new List<T>();
            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            if (addSiteSettingAndOpertionBy)
            {
                AddSiteSettingAndOpertionBy(ref sqlParameterList);
            }
            
            if (sqlParamters != null)
            {
                sqlParameterList.AddRange(sqlParamters);
            }

            DataSet ds = ExecuteDataSet(procedureName, sqlParameterList.ToArray());
            string xmlString = string.Empty;
            if (ds != null && ds.Tables.Count > 0)
            {
                if(ds.Tables[ds.Tables.Count - 1].Rows.Count > 0)
                {
                    for (int r = 0; r < ds.Tables[ds.Tables.Count - 1].Rows.Count; r++)
                    {
                        xmlString += ds.Tables[ds.Tables.Count - 1].Rows[r][0].ToString();
                    }
                }
            }
            
            //var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    string typeName = typeof(T).Name;
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//" + typeName);
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<T>(node.OuterXml));
                    }
                }
            }

            return li;
        }

        public static string GetFilters(string tableName, string filters)
        {
            StringBuilder sqlSb = new StringBuilder();
            tableName = GetSafeTableName(tableName);
            string[] filterArray = filters.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string filter in filterArray)
            {
                string columnName = GetSafeTableName(filter);
                sqlSb.Append("SELECT DISTINCT " + GetSafeTableName(filter) + " FROM " + tableName + " ORDER BY " + columnName + ";\r\n");
            }

            string sql = sqlSb.ToString().Trim();
            DataSet ds = ExecuteDataSet(sql, null, CommandType.Text);
            StringBuilder sb = new StringBuilder();
            if (ds != null && ds.Tables.Count == filterArray.Length)
            {
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Rows.Count > 0)
                    {
                        sb.Append("<select id=\"filter" + filterArray[i] + "\" class=\"inputfl\">\r\n");
                        sb.Append(" <option value=\"\">" + filterArray[i] + "</option>\r\n");
                        foreach (DataRow row in ds.Tables[i].Rows)
                        {
                            string value = Convert.ToString(row[0]).Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                sb.Append(" <option value=\"" + value + "\">" + value + "</option>\r\n");
                            }
                        }

                        sb.Append("</select>\r\n");
                    }
                }
            }

            return sb.ToString();
        }

        public static int Save<T>(string json)
        {
            T model = JsonHelper.DeserializeJson<T>(json);
            return SaveModel<T>(model);
        }

        public static int SaveModel<T>(Object obj)
        {
            T model = (T)obj;
            Type myType = typeof(T);
            FieldInfo[] myFields = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            string procedureName = "Insert" + myType.Name + "Proc";
            bool returnIntId = false;
            SqlParameter idPara = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            idPara.Direction = System.Data.ParameterDirection.Output;
            int oldId = 0;

            for (int i = 0; i < myFields.Length; i++)
            {
                object o = myFields[i].GetValue(model);
                string paraName = GetColumnName(myFields[i].Name);
                if (paraName.ToLower() == "createdby")
                {
                    sqlParameters.Add(new SqlParameter("@" + paraName, Operation.OperationBy));
                }
                else if (paraName.ToLower() == "id" && o != null)
                {
                    string value = o.ToString();
                    if (value.Length > 30 && value.Contains("-"))
                    {
                        Guid id = new System.Guid(value);
                        sqlParameters.Add(new SqlParameter("@Id", id));
                    }
                    else
                    {
                        int id = 0;
                        if (int.TryParse(value, out id))
                        {
                            if (id > 0)
                            {
                                oldId = id;
                                sqlParameters.Add(new SqlParameter("@Id", id));
                                procedureName = "Update" + myType.Name + "Proc";
                            }
                            else if (id == 0)
                            {
                                sqlParameters.Add(idPara);
                                returnIntId = true;
                            }
                        }
                    }
                }
                else if (o != null)
                {
                    sqlParameters.Add(new SqlParameter("@" + paraName, o));
                }
            }

            if (returnIntId)
            {
                ExecuteNonQuery(procedureName, sqlParameters.ToArray());
                return (int)idPara.Value;
            }
            else
            {
                int result = ExecuteNonQuery(procedureName, sqlParameters.ToArray());
                if (result > 0 && oldId > 0)
                {
                    return oldId;
                }

                return result;
            }
        }

        public static string GetColumnName(string columnName)
        {
            int a = columnName.IndexOf("<");
            if (a >= 0)
            {
                int b = columnName.IndexOf(">", a);
                if (b > a)
                {
                    return columnName.Substring(a + 1, b - a - 1);
                }
            }

            return columnName;
        }

        public static T GetModel<T>(int? id = null, Guid? guid = null)
        {
            string modelName = typeof(T).Name;
            var sqlParameters = new List<SqlParameter>();
            bool idIsValid = false;
            if (id != null)
            {
                sqlParameters.Add(new SqlParameter("@id", id));
                idIsValid = true;
            }
            else if (guid != null)
            {
                sqlParameters.Add(new SqlParameter("@id", guid));
                idIsValid = true;
            }

            if (idIsValid)
            {
                string sql = "SELECT * FROM [" + modelName + "] WHERE Id = @id FOR XML PATH('" + modelName + "'), ROOT('" + modelName + "List')";
                var xmlString = BaseDao.ExecuteXmlReader(sql, sqlParameters.ToArray(), CommandType.Text);
                if (!string.IsNullOrEmpty(xmlString))
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(xmlString);
                    if (xmlDocument.HasChildNodes)
                    {
                        XmlNode node = xmlDocument.FirstChild.SelectSingleNode("//" + modelName);
                        return SerializationHelper.Deserialize<T>(node.OuterXml);
                    }
                }
            }

            return default(T);
        }

        public static List<T> GetModel<T>(string ids)
        {
            string modelName = typeof(T).Name;
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Ids", ids));

            StringBuilder sql = new StringBuilder();
            sql
                .Append("	SELECT\r\n")
                .Append("	M.*\r\n")
                .Append("	FROM " + modelName + " M WITH (NOLOCK)\r\n")
                .Append("	JOIN [dbo].[GetTable](@Ids, ',') T ON T.Id = M.Id\r\n")
                .Append("	ORDER BY T.Sequence\r\n")
                .Append("	FOR XML PATH('" + modelName + "'), ROOT('" + modelName + "List')\r\n");

            List<T> list = new List<T>();

            var xmlString = BaseDao.ExecuteXmlReader(sql.ToString(), sqlParameters.ToArray(), CommandType.Text);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//" + modelName);
                    foreach (XmlNode node in nodeList)
                    {
                        list.Add(SerializationHelper.Deserialize<T>(node.OuterXml));
                    }
                }
            }

            return list;
        }

        public static QueryResult<T> GetQueryResult<T>(string procedureName, SqlParameter[] sqlParamters)
        {
            string modelName = typeof(T).Name;

            if (string.IsNullOrEmpty(procedureName))
            {
                procedureName = "Get" + modelName + "Proc";
            }

            QueryResult<T> result = new QueryResult<T>();
            List<T> li = new List<T>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//" + modelName);
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<T>(node.OuterXml));
                    }

                    result.DataList = li;
                    XmlNode xmlNode = xmlDocument.FirstChild.SelectSingleNode("//TotalCount");
                    if (xmlNode != null)
                    {
                        result.Count = Convert.ToInt32(xmlNode.InnerText, Constants.CurrentCulture);
                    }
                }
            }

            return result;
        }

        public static string GetSafeTableName(string tableName)
        {
            tableName = tableName.Trim();
            string pattern = "[a-zA-Z0-9_]*";
            Match m = Regex.Match(tableName, pattern);
            if (m != null && m.Value == tableName)
            {
                return "[" + tableName + "]";
            }

            return string.Empty;
        }

        public static int UpdateRecordStatus(string tableName, string ids, int recordStatus)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@tableName", GetSafeTableName(tableName)));
            sqlParameters.Add(new SqlParameter("@Ids", ids));
            sqlParameters.Add(new SqlParameter("@RecordStatus", recordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            return ExecuteNonQuery("UpdateRecordStatus", sqlParameters.ToArray());
        }

        public static int UpdateRecordStatus(SqlParameter[] sqlParamters)
        {
            return ExecuteNonQuery("UpdateRecordStatus", sqlParamters);
        }

        public static int SavePriority(SqlParameter[] sqlParamters)
        {
            return ExecuteNonQuery("SavePriority", sqlParamters);
        }

        public static DataTable GetDataTable(string procedureName, SqlParameter[] sqlParamters)
        {
            DataSet ds = ExecuteDataSet(procedureName, sqlParamters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
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
    }
}
