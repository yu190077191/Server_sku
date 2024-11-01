using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.DataAccess
{
    public class LanguagesDao : BaseDao
    {
        public static int AddLanguages(Languages model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Language", model.Language));
            sqlParameters.Add(new SqlParameter("@Priority", model.Priority));
            sqlParameters.Add(new SqlParameter("@EnglishName", model.EnglishName));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            return ExecuteNonQuery("InsertLanguagesProc", sqlParameters.ToArray());
        }

        public static List<Languages> GetLanguages(string userName = null)
        {
            string procedureName = "GetLanguages";
            List<Languages> li = new List<Languages>();
            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(userName))
            {
                sqlParameterList.Add(new SqlParameter("@EmployeeId", Operation.OperationBy));
                procedureName = "GetUserLanguage";
            }

            var xmlString = ExecuteXmlReader(procedureName, sqlParameterList.ToArray());
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodes = xmlDocument.FirstChild.SelectNodes("//Languages");
                    if (nodes != null)
                    {
                        foreach (XmlNode node in nodes)
                        {
                            li.Add(SerializationHelper.Deserialize<Languages>(node.OuterXml));
                        }
                    }
                }
            }

            return li;
        }

        public static int SetUserLanguage(string userName, string languageEnglishName)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@EmployeeId", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@languageEnglishName", languageEnglishName));
            return ExecuteNonQuery("SetUserLanguage", sqlParameters.ToArray());
        }

        public static string Translate(string languageKey, int? languageId, string languageEnglishName)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@languageKey", languageKey));
            if (languageId != null)
            {
                sqlParameters.Add(new SqlParameter("@languageId", languageId));
            }

            if (!string.IsNullOrEmpty(languageEnglishName))
            {
                sqlParameters.Add(new SqlParameter("@languageEnglishName", languageEnglishName));
            }

            DataSet ds = ExecuteDataSet("TranslateProc", sqlParameters.ToArray());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }

            return languageKey;
        }

        public static string GetLanguageKey(string languageValue)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@languageValue", languageValue));
            DataSet ds = ExecuteDataSet("GetLanguageKey", sqlParameters.ToArray());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }

            return languageValue;
        }

        public static List<LanguageKeyValue> TranslateArray(string LanguageKeyArray, int? languageId, string languageEnglishName)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@LanguageKeyArray", LanguageKeyArray));
            if (languageId != null)
            {
                sqlParameters.Add(new SqlParameter("@languageId", languageId));
            }

            if (!string.IsNullOrEmpty(languageEnglishName))
            {
                sqlParameters.Add(new SqlParameter("@languageEnglishName", languageEnglishName));
            }

            List<LanguageKeyValue> li = new List<LanguageKeyValue>();
            var xmlString = ExecuteXmlReader("TranslateArrayProc", sqlParameters.ToArray());
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodes = xmlDocument.FirstChild.SelectNodes("//LanguageKeyValue");
                    if (nodes != null)
                    {
                        foreach (XmlNode node in nodes)
                        {
                            li.Add(SerializationHelper.Deserialize<LanguageKeyValue>(node.OuterXml));
                        }
                    }
                }
            }

            return li;
        }

        public static int InsertLanguages(Languages model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Language", model.Language));
            sqlParameters.Add(new SqlParameter("@Priority", model.Priority));
            sqlParameters.Add(new SqlParameter("@EnglishName", model.EnglishName));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertLanguagesProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateLanguages(Languages model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@Language", model.Language));
            sqlParameters.Add(new SqlParameter("@Priority", model.Priority));
            sqlParameters.Add(new SqlParameter("@EnglishName", model.EnglishName));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            return ExecuteNonQuery("UpdateLanguagesProc", sqlParameters.ToArray());
        }

        public static List<Languages> GetLanguages(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Languages> li = new List<Languages>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//Languages");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Languages>(node.OuterXml));
                    }
                }
            }

            return li;
        }

    }
}