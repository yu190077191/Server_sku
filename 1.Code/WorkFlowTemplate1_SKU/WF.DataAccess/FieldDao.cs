using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.DataAccess
{
    public class FieldDao : BaseDao
    {
        public static int InsertField(WF.Model.Field model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Type", model.Type));
            sqlParameters.Add(new SqlParameter("@Category", model.Category));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@InputType", model.InputType));
            sqlParameters.Add(new SqlParameter("@Values", model.Values));
            sqlParameters.Add(new SqlParameter("@TypeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@Colspan", model.Colspan));
            sqlParameters.Add(new SqlParameter("@Priority", model.Priority));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertFieldProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateField(WF.Model.Field model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@Type", model.Type));
            sqlParameters.Add(new SqlParameter("@Category", model.Category));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@InputType", model.InputType));
            sqlParameters.Add(new SqlParameter("@Values", model.Values));
            sqlParameters.Add(new SqlParameter("@TypeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@Colspan", model.Colspan));
            sqlParameters.Add(new SqlParameter("@Priority", model.Priority));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateFieldProc", sqlParameters.ToArray());
        }

        public static List<WF.Model.Field> GetField(string procedureName, SqlParameter[] sqlParamters)
        {
            List<WF.Model.Field> li = new List<WF.Model.Field>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//Field");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.Field>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<WF.Model.Field> GetFieldResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<WF.Model.Field> result = new QueryResult<WF.Model.Field>();
            List<WF.Model.Field> li = new List<WF.Model.Field>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//Field");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.Field>(node.OuterXml));
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
