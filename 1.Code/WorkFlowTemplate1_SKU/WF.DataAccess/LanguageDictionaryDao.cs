using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.DataAccess
{
    public class LanguageDictionaryDao : BaseDao
    {
        public static int InsertOrUpdateDictionary(Translation model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Key", model.Key));
            sqlParameters.Add(new SqlParameter("@Chinese", model.Chinese));
            sqlParameters.Add(new SqlParameter("@English", model.English));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            return ExecuteNonQuery("InsertOrUpdateLanguageDictionary", sqlParameters.ToArray());
        }

        public static QueryResult<Translation> GetWordList(SqlParameter[] sqlParamters)
        {
            QueryResult<Translation> result = new QueryResult<Translation>();
            string procedureName = "GetWordList";
            List<Translation> li = new List<Translation>();
            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//Translation");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Translation>(node.OuterXml));
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

        public static int InsertLanguageDictionary(LanguageDictionary model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@LanguageId", model.LanguageId));
            sqlParameters.Add(new SqlParameter("@LanguageKey", model.LanguageKey));
            sqlParameters.Add(new SqlParameter("@Value", model.Value));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertLanguageDictionaryProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateLanguageDictionary(LanguageDictionary model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@LanguageId", model.LanguageId));
            sqlParameters.Add(new SqlParameter("@LanguageKey", model.LanguageKey));
            sqlParameters.Add(new SqlParameter("@Value", model.Value));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateLanguageDictionaryProc", sqlParameters.ToArray());
        }

        public static List<LanguageDictionary> GetLanguageDictionary(string procedureName, SqlParameter[] sqlParamters)
        {
            List<LanguageDictionary> li = new List<LanguageDictionary>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//LanguageDictionary");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<LanguageDictionary>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<LanguageDictionary> GetLanguageDictionaryResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<LanguageDictionary> result = new QueryResult<LanguageDictionary>();
            List<LanguageDictionary> li = new List<LanguageDictionary>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//LanguageDictionary");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<LanguageDictionary>(node.OuterXml));
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

    }
}
