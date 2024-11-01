using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.DataAccess
{
    public class DictionaryDao : BaseDao
    {
        public static int InsertDictionary(WF.Model.Dictionary model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@TypeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@Value", model.Value));
            sqlParameters.Add(new SqlParameter("@ValueString", model.ValueString));
            sqlParameters.Add(new SqlParameter("@Priority", model.Priority));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertDictionaryProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateDictionary(WF.Model.Dictionary model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@TypeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@Value", model.Value));
            sqlParameters.Add(new SqlParameter("@ValueString", model.ValueString));
            sqlParameters.Add(new SqlParameter("@Priority", model.Priority));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateDictionaryProc", sqlParameters.ToArray());
        }

        public static List<WF.Model.Dictionary> GetDictionary(string procedureName, SqlParameter[] sqlParamters)
        {
            List<WF.Model.Dictionary> li = new List<WF.Model.Dictionary>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//Dictionary");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.Dictionary>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<WF.Model.Dictionary> GetDictionaryResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<WF.Model.Dictionary> result = new QueryResult<WF.Model.Dictionary>();
            List<WF.Model.Dictionary> li = new List<WF.Model.Dictionary>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//Dictionary");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.Dictionary>(node.OuterXml));
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
