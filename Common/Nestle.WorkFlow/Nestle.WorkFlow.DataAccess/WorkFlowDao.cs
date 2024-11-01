using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class WorkFlowDao : BaseDao
    {
        public static int InsertWorkFlow(Nestle.WorkFlow.Model.WorkFlow model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@RequestId", model.RequestId));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", model.RequestVersionId));
            sqlParameters.Add(new SqlParameter("@WorkFlowPatternId", model.WorkFlowPatternId));
            sqlParameters.Add(new SqlParameter("@Originator", model.Originator));
            sqlParameters.Add(new SqlParameter("@WorkFlowStatusId", model.WorkFlowStatusId));
            sqlParameters.Add(new SqlParameter("@StatusId", model.StatusId));
            sqlParameters.Add(new SqlParameter("@Data", model.Data));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            return ExecuteNonQuery("InsertWorkFlowProc", sqlParameters.ToArray());
        }

        public static int UpdateWorkFlow(Nestle.WorkFlow.Model.WorkFlow model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@RequestId", model.RequestId));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", model.RequestVersionId));
            sqlParameters.Add(new SqlParameter("@WorkFlowPatternId", model.WorkFlowPatternId));
            sqlParameters.Add(new SqlParameter("@Originator", model.Originator));
            sqlParameters.Add(new SqlParameter("@WorkFlowStatusId", model.WorkFlowStatusId));
            sqlParameters.Add(new SqlParameter("@StatusId", model.StatusId));
            sqlParameters.Add(new SqlParameter("@Data", model.Data));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateWorkFlowProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.WorkFlow> GetWorkFlow(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.WorkFlow> li = new List<Nestle.WorkFlow.Model.WorkFlow>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//WorkFlow");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.WorkFlow>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.WorkFlow> GetWorkFlowResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.WorkFlow> result = new QueryResult<Nestle.WorkFlow.Model.WorkFlow>();
            List<Nestle.WorkFlow.Model.WorkFlow> li = new List<Nestle.WorkFlow.Model.WorkFlow>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//WorkFlow");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.WorkFlow>(node.OuterXml));
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
