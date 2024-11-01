using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class WorkFlowApproverDao : BaseDao
    {
        public static int InsertWorkFlowApprover(Nestle.WorkFlow.Model.WorkFlowApprover model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@WorkFlowId", model.WorkFlowId));
            sqlParameters.Add(new SqlParameter("@ApproverEmployeeId", model.ApproverEmployeeId));
            sqlParameters.Add(new SqlParameter("@Status", model.Status));
            sqlParameters.Add(new SqlParameter("@Sequence", model.Sequence));
            sqlParameters.Add(new SqlParameter("@NeedAllMemebersApproval", model.NeedAllMemebersApproval));
            sqlParameters.Add(new SqlParameter("@MinWeight", model.MinWeight));
            sqlParameters.Add(new SqlParameter("@Weight", model.Weight));
            sqlParameters.Add(new SqlParameter("@ReviewResult", model.ReviewResult));
            sqlParameters.Add(new SqlParameter("@ReviewTime", model.ReviewTime));
            sqlParameters.Add(new SqlParameter("@ReviewComment", model.ReviewComment));
            sqlParameters.Add(new SqlParameter("@IsManuallyAdded", model.IsManuallyAdded));
            sqlParameters.Add(new SqlParameter("@EscalationEventId", model.EscalationEventId));
            sqlParameters.Add(new SqlParameter("@EscalationTime", model.EscalationTime));
            sqlParameters.Add(new SqlParameter("@ApproveType", model.ApproveType));
            sqlParameters.Add(new SqlParameter("@ApprovalXml", model.ApprovalXml));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            return ExecuteNonQuery("InsertWorkFlowApproverProc", sqlParameters.ToArray());
        }

        public static int UpdateWorkFlowApprover(Nestle.WorkFlow.Model.WorkFlowApprover model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@WorkFlowId", model.WorkFlowId));
            sqlParameters.Add(new SqlParameter("@ApproverEmployeeId", model.ApproverEmployeeId));
            sqlParameters.Add(new SqlParameter("@Status", model.Status));
            sqlParameters.Add(new SqlParameter("@Sequence", model.Sequence));
            sqlParameters.Add(new SqlParameter("@NeedAllMemebersApproval", model.NeedAllMemebersApproval));
            sqlParameters.Add(new SqlParameter("@MinWeight", model.MinWeight));
            sqlParameters.Add(new SqlParameter("@Weight", model.Weight));
            sqlParameters.Add(new SqlParameter("@ReviewResult", model.ReviewResult));
            sqlParameters.Add(new SqlParameter("@ReviewTime", model.ReviewTime));
            sqlParameters.Add(new SqlParameter("@ReviewComment", model.ReviewComment));
            sqlParameters.Add(new SqlParameter("@IsManuallyAdded", model.IsManuallyAdded));
            sqlParameters.Add(new SqlParameter("@EscalationEventId", model.EscalationEventId));
            sqlParameters.Add(new SqlParameter("@EscalationTime", model.EscalationTime));
            sqlParameters.Add(new SqlParameter("@ApproveType", model.ApproveType));
            sqlParameters.Add(new SqlParameter("@ApprovalXml", model.ApprovalXml));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateWorkFlowApproverProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.WorkFlowApprover> GetWorkFlowApprover(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.WorkFlowApprover> li = new List<Nestle.WorkFlow.Model.WorkFlowApprover>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//WorkFlowApprover");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.WorkFlowApprover>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.WorkFlowApprover> GetWorkFlowApproverResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.WorkFlowApprover> result = new QueryResult<Nestle.WorkFlow.Model.WorkFlowApprover>();
            List<Nestle.WorkFlow.Model.WorkFlowApprover> li = new List<Nestle.WorkFlow.Model.WorkFlowApprover>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//WorkFlowApprover");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.WorkFlowApprover>(node.OuterXml));
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
