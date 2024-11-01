using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class WorkFlowPatternEventContainerRoleDao : BaseDao
    {
        public static int InsertWorkFlowPatternEventContainerRole(Nestle.WorkFlow.Model.WorkFlowPatternEventContainerRole model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@WorkFlowPatternEventContainerId", model.WorkFlowPatternEventContainerId));
            sqlParameters.Add(new SqlParameter("@RoleId", model.RoleId));
            sqlParameters.Add(new SqlParameter("@Sequence", model.Sequence));
            sqlParameters.Add(new SqlParameter("@NeedAllMemebersApproval", model.NeedAllMemebersApproval));
            sqlParameters.Add(new SqlParameter("@MinWeight", model.MinWeight));
            sqlParameters.Add(new SqlParameter("@Weight", model.Weight));
            sqlParameters.Add(new SqlParameter("@EscalationDays", model.EscalationDays));
            sqlParameters.Add(new SqlParameter("@EscalationEventId", model.EscalationEventId));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertWorkFlowPatternEventContainerRoleProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateWorkFlowPatternEventContainerRole(Nestle.WorkFlow.Model.WorkFlowPatternEventContainerRole model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@WorkFlowPatternEventContainerId", model.WorkFlowPatternEventContainerId));
            sqlParameters.Add(new SqlParameter("@RoleId", model.RoleId));
            sqlParameters.Add(new SqlParameter("@Sequence", model.Sequence));
            sqlParameters.Add(new SqlParameter("@NeedAllMemebersApproval", model.NeedAllMemebersApproval));
            sqlParameters.Add(new SqlParameter("@MinWeight", model.MinWeight));
            sqlParameters.Add(new SqlParameter("@Weight", model.Weight));
            sqlParameters.Add(new SqlParameter("@EscalationDays", model.EscalationDays));
            sqlParameters.Add(new SqlParameter("@EscalationEventId", model.EscalationEventId));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateWorkFlowPatternEventContainerRoleProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.WorkFlowPatternEventContainerRole> GetWorkFlowPatternEventContainerRole(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.WorkFlowPatternEventContainerRole> li = new List<Nestle.WorkFlow.Model.WorkFlowPatternEventContainerRole>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//WorkFlowPatternEventContainerRole");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.WorkFlowPatternEventContainerRole>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.WorkFlowPatternEventContainerRole> GetWorkFlowPatternEventContainerRoleResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.WorkFlowPatternEventContainerRole> result = new QueryResult<Nestle.WorkFlow.Model.WorkFlowPatternEventContainerRole>();
            List<Nestle.WorkFlow.Model.WorkFlowPatternEventContainerRole> li = new List<Nestle.WorkFlow.Model.WorkFlowPatternEventContainerRole>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//WorkFlowPatternEventContainerRole");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.WorkFlowPatternEventContainerRole>(node.OuterXml));
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
