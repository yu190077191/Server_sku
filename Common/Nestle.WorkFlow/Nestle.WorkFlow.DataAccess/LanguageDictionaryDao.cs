using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class LanguageDictionaryDao : BaseDao
    {
        public static int InsertLanguageDictionary(Nestle.WorkFlow.Model.LanguageDictionary model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
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

        public static int UpdateLanguageDictionary(Nestle.WorkFlow.Model.LanguageDictionary model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
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

        public static List<Nestle.WorkFlow.Model.LanguageDictionary> GetLanguageDictionary(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.LanguageDictionary> li = new List<Nestle.WorkFlow.Model.LanguageDictionary>();

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
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.LanguageDictionary>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.LanguageDictionary> GetLanguageDictionaryResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.LanguageDictionary> result = new QueryResult<Nestle.WorkFlow.Model.LanguageDictionary>();
            List<Nestle.WorkFlow.Model.LanguageDictionary> li = new List<Nestle.WorkFlow.Model.LanguageDictionary>();

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
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.LanguageDictionary>(node.OuterXml));
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
