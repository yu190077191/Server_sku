using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class WorkFlowPatternDao : BaseDao
    {
        public static int InsertWorkFlowPattern(WorkFlowPatternJson model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@TypeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            string xml = SerializationHelper.Serialize(model.WorkFlowPatternStepDetailsList);
            sqlParameters.Add(new SqlParameter("@xml", xml));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertWorkFlowPatternByJson", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int InsertWorkFlowPattern(Nestle.WorkFlow.Model.WorkFlowPattern model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@TypeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@ChineseName", model.ChineseName));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertWorkFlowPatternProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateWorkFlowPattern(Nestle.WorkFlow.Model.WorkFlowPattern model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@TypeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@ChineseName", model.ChineseName));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateWorkFlowPatternProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.WorkFlowPattern> GetWorkFlowPattern(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.WorkFlowPattern> li = new List<Nestle.WorkFlow.Model.WorkFlowPattern>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//WorkFlowPattern");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.WorkFlowPattern>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.WorkFlowPattern> GetWorkFlowPatternResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.WorkFlowPattern> result = new QueryResult<Nestle.WorkFlow.Model.WorkFlowPattern>();
            List<Nestle.WorkFlow.Model.WorkFlowPattern> li = new List<Nestle.WorkFlow.Model.WorkFlowPattern>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//WorkFlowPattern");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.WorkFlowPattern>(node.OuterXml));
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

        public static QueryResult<Nestle.WorkFlow.Model.Pattern> GetPatternResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.Pattern> result = new QueryResult<Nestle.WorkFlow.Model.Pattern>();
            List<Nestle.WorkFlow.Model.Pattern> li = new List<Nestle.WorkFlow.Model.Pattern>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//Pattern");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.Pattern>(node.OuterXml));
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
