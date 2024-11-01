using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using WF.DataAccess;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.BusinessRule
{
    public class CommonRule
    {
        public static string OverWriteExcel(int id)
        {
            return CommonDao.OverWriteExcel(id);
        }

        public static string ExportToCSV(string procedureName, SqlParameter[] sqlParamters)
        {
            DataSet ds = GetDataSet(procedureName, sqlParamters);
            if (ds != null && ds.Tables.Count > 0)
            {
                string filePth = CommonRule.ExportToCSV("Report", ds);
                return filePth;
            }

            return "no records";
        }

        public static DataSet GetDataSet(string procedureName, SqlParameter[] sqlParamters)
        {
            return BaseDao.ExecuteDataSet(procedureName, sqlParamters);
        }

        public static string ExportToCSV(string savedFileNamePrefix, DataSet ds)
        {
            DataTable dt = new DataTable();
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                string savedFilePath = "Exported_Reports/" + savedFileNamePrefix + "_"
                    + DateTime.Now.ToString("yyyy-MM-dd") + "_"
                    + DateTime.Now.Hour.ToString().PadLeft(2, '0')
                    + DateTime.Now.Minute.ToString().PadLeft(2, '0')
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
                            sb.Append(dt.Rows[r][c].ToString().Replace("\"", string.Empty).Replace(columnSplitBy, string.Empty).Replace("\r\n", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty));
                        }
                    }
                }

                StreamWriter sw = new StreamWriter(path, true, Encoding.Unicode);
                sw.Write(sb.ToString());
                sw.Close();

                FileInfo fi = new FileInfo(path);
                double sizeInMB = Convert.ToDouble(fi.Length) / Convert.ToDouble(1024 * 1024);
                //if (sizeInMB > 3)
                //{
                //    DeleteOldExportedFiles();
                //    ZipHelper.ZipFile(path, path.Replace(".csv", ".zip"), 9, true);
                //    File.Delete(path);
                //    savedFilePath = savedFilePath.Replace(".csv", ".zip");
                //}

                return savedFilePath;
            }
            else
            {
                return "对不起，没有相关记录！";
            }
        }

        public static string GetSafeTableName(string tableName)
        {
            return CommonDao.GetSafeTableName(tableName);
        }

        public static string GetParameterValue(SqlParameter[] sqlParameters, string parameterName)
        {
            List<SqlParameter> li = new List<SqlParameter>(sqlParameters);
            return BaseDao.GetParameterValue(li, parameterName);
        }

        public static System.Web.Mvc.MvcHtmlString GetSelectOption(string typeCode, int? id = null, bool isDictionary = true, bool insertPleaseSelectOption = true, string selectedValue = null)
        {
            List<SelectOption> list = new List<SelectOption>();
            if (isDictionary)
            {
                list = GetSelectOptionFromDictionary(typeCode, id);
            }
            else
            {
                list = Get<SelectOption>("GetSelectOption", new[]{
                new SqlParameter("@TypeCode", typeCode),
                new SqlParameter("@id", id),
                new SqlParameter("@OperationBy", Operation.OperationBy)
            });
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (insertPleaseSelectOption)
            {
                sb.Append("<option value=\"\">" + LanguagesRule.Translate("Please select") + "</option>\r\n");
            }

            foreach (SelectOption item in list)
            {
                string checkedStr = (item.Checked ? " selected=\"selected\"" : string.Empty);
                if (!string.IsNullOrEmpty(selectedValue) && (selectedValue == item.Value || selectedValue == item.Text))
                {
                    checkedStr = " selected=\"selected\"";
                }

                sb.Append("<option value=\"" + item.Value + "\"" + checkedStr + ">" + item.Text + "</option>\r\n");
            }

            return System.Web.Mvc.MvcHtmlString.Create(sb.ToString());
        }

        public static List<SelectOption> GetSelectOptionFromDictionary(string typeCode, int? id = null)
        {
            return Get<SelectOption>("GetSelectOptionFromDictionary", new[]{
                new SqlParameter("@TypeCode", typeCode),
                new SqlParameter("@id", id),
                new SqlParameter("@OperationBy", Operation.OperationBy)
            });
        }

        public static int Save<T>(string json)
        {
            return CommonDao.Save<T>(json);
        }

        public static int SaveModel<T>(Object obj)
        {
            return CommonDao.SaveModel<T>(obj);
        }

        public static T GetModel<T>(int? id = null, Guid? guid = null)
        {
            return CommonDao.GetModel<T>(id, guid);
        }

        public static List<T> GetModel<T>(string ids)
        {
            return CommonDao.GetModel<T>(ids);
        }

        public static QueryResult<T> GetQueryResult<T>(string procedureName, SqlParameter[] sqlParamters)
        {
            return CommonDao.GetQueryResult<T>(procedureName, sqlParamters);
        }

        public static int UpdateRecordStatus(string tableName, string ids, int recordStatus)
        {
            return CommonDao.UpdateRecordStatus(tableName, ids, recordStatus);
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

        public static System.Web.Mvc.MvcHtmlString GetJsonFromModel(object obj)
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
            return System.Web.Mvc.MvcHtmlString.Create(sb.ToString());
        }

        public static string GetXmlString(string procedureName, SqlParameter[] sqlParamters)
        {
            return CommonDao.GetXmlString(procedureName, sqlParamters);
        }

        public static List<T> GetByXmlString<T>(string xmlString)
        {
            return CommonDao.GetByXmlString<T>(xmlString);
        }

        public static void ScanDir(string dir, ref List<string> files, string AllowedExtendedNames = null)
        {
            foreach (string d in System.IO.Directory.GetFileSystemEntries(dir))
            {
                if (System.IO.File.Exists(d))
                {
                    if (string.IsNullOrEmpty(AllowedExtendedNames)) files.Add(d);
                    else
                    {
                        string extendedNames = d;
                        if (extendedNames.IndexOf(".") > 0)
                        {
                            extendedNames = extendedNames.Substring(extendedNames.LastIndexOf("."));
                            if (("," + AllowedExtendedNames.ToLower() + ",").IndexOf("," + extendedNames.ToLower() + ",") >= 0)
                            {
                                files.Add(d);
                            }
                        }
                    }
                }
                else ScanDir(d, ref files, AllowedExtendedNames);
            }
        }

        public static string SKU_Email_Tool_API(string typeCode, int id, string xml)
        {
            return CommonDao.SKU_Email_Tool_API(typeCode, id, xml);
        }

        public static string GetPHDES(string L5Code)
        {
            return CommonDao.GetPHDES(L5Code);
        }

        public static string GetBusiness(string Bu, string Branch)
        {
            return CommonDao.GetBusiness(Bu, Branch);
        }


        /// <summary>
        /// 包装信息BLL
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int AddPackagingJson(PackagingJson model)
        {
            return PackagingDAL.AddPackagingJson(model);
        }

    }
}
