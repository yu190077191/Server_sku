using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class APIDao : BaseDao
    {
        public static List<ExecuteResult> SubmitRequest(int requestVersionId, int requestId = 0, string typeCode = "default", string xml = null)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@RequestId", requestId));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", requestVersionId));
            sqlParameters.Add(new SqlParameter("@typeCode", typeCode));
            sqlParameters.Add(new SqlParameter("@xml", xml));
            List<ExecuteResult> list = CommonDao.Get<ExecuteResult>("SubmitRequest", sqlParameters.ToArray());
            return list;
        }

        public static List<ExecuteResult> ReviewRequest(ActionData model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@RequestId", model.RequestId));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", model.RequestVersionId));
            sqlParameters.Add(new SqlParameter("@Comment", model.Comment));
            sqlParameters.Add(new SqlParameter("@EventId", model.EventId));
            sqlParameters.Add(new SqlParameter("@WorkFlowId", model.WorkFlowId));
            sqlParameters.Add(new SqlParameter("@xml", model.Xml));

            sqlParameters.Add(new SqlParameter("@typeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@SiteName", model.SiteName));
            sqlParameters.Add(new SqlParameter("@SiteGuid", model.SiteGuid));
            sqlParameters.Add(new SqlParameter("@OperationBy", Operation.OperationBy));

            if (model.DepartmentId > 0)
            {
                sqlParameters.Add(new SqlParameter("@DepartmentId", model.DepartmentId));
            }
            if (model.ReturnStep is null || !string.Empty.Equals(model.ReturnStep.ToString()))
            {
                sqlParameters.Add(new SqlParameter("@ReturnStep", model.ReturnStep));
            }
            else
            {
                sqlParameters.Add(new SqlParameter("@ReturnStep", ""));
            }

            List<ExecuteResult> list = CommonDao.Get<ExecuteResult>("ReviewRequest", sqlParameters.ToArray());

            return list;
        }

        public static List<ExecuteResult> CopyRequestForHK(ActionData model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@RequestId", model.RequestId));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", model.RequestVersionId));
            sqlParameters.Add(new SqlParameter("@Comment", model.Comment));
            sqlParameters.Add(new SqlParameter("@EventId", model.EventId));
            sqlParameters.Add(new SqlParameter("@WorkFlowId", model.WorkFlowId));
            sqlParameters.Add(new SqlParameter("@xml", model.Xml));

            sqlParameters.Add(new SqlParameter("@typeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@SiteName", model.SiteName));
            sqlParameters.Add(new SqlParameter("@SiteGuid", model.SiteGuid));
            sqlParameters.Add(new SqlParameter("@OperationBy", Operation.OperationBy));

            if (model.DepartmentId > 0)
            {
                sqlParameters.Add(new SqlParameter("@DepartmentId", model.DepartmentId));
            }

            List<ExecuteResult> list = CommonDao.Get<ExecuteResult>("CopyRequestForHK", sqlParameters.ToArray());

            return list;
        }

        public static List<ActionHistory> GetActionHistory(int requestId, int requestVersionId, string siteName, Guid siteGuid, Guid requestorEmployeeId, string typeCode = "default", bool autoFix = false, string storedProcedureName = "GetActionHistory", int? departmentId = null)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@RequestId", requestId));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", requestVersionId));
            sqlParameters.Add(new SqlParameter("@typeCode", typeCode));

            sqlParameters.Add(new SqlParameter("@SiteName", siteName));
            sqlParameters.Add(new SqlParameter("@SiteGuid", siteGuid));
            sqlParameters.Add(new SqlParameter("@OperationBy", requestorEmployeeId));
            sqlParameters.Add(new SqlParameter("@RunTimeMode", autoFix));
            if (departmentId > 0)
            {
                sqlParameters.Add(new SqlParameter("@DepartmentId", departmentId));
            }

            List<ActionHistory> list = CommonDao.Get<ActionHistory>(storedProcedureName, sqlParameters.ToArray());
            return list;
        }
        public static List<ReturnStepResult> GetReturnStep(int requestId, string storedProcedureName = "GetReturnStep")
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@RequestId", requestId));

            List<ReturnStepResult> list = CommonDao.Get<ReturnStepResult>(storedProcedureName, sqlParameters.ToArray());
            return list;
        }

        public static bool IsApprover(string siteName, Guid siteGuid, string typeCode = "default")
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@typeCode", typeCode));
            sqlParameters.Add(new SqlParameter("@SiteName", siteName));
            sqlParameters.Add(new SqlParameter("@SiteGuid", siteGuid));
            sqlParameters.Add(new SqlParameter("@OperationBy", Operation.OperationBy));
            return (bool)ExecuteScalar("IsApprover", sqlParameters.ToArray(), System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// 根据用户id,查找是否有权限 bluefish 2019.11.7
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="siteGuid"></param>
        /// <returns></returns>
        public static bool IsHaveRole(string siteName, Guid siteGuid)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@SiteName", siteName));
            sqlParameters.Add(new SqlParameter("@SiteGuid", siteGuid));
            sqlParameters.Add(new SqlParameter("@OperationBy", Operation.OperationBy));
            return (bool)ExecuteScalar("IsHaveRole", sqlParameters.ToArray(), System.Data.CommandType.StoredProcedure);
        }

        public static QueryResult<ApproverWorkList> GetApproverWorkListResult(string siteName, Guid siteGuid, int? id = null, string typeCode = "default")
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@typeCode", typeCode));
            sqlParameters.Add(new SqlParameter("@SiteName", siteName));
            sqlParameters.Add(new SqlParameter("@SiteGuid", siteGuid));
            sqlParameters.Add(new SqlParameter("@OperationBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@requestVersionId", id));

            QueryResult<ApproverWorkList> result = new QueryResult<ApproverWorkList>();
            List<ApproverWorkList> li = new List<ApproverWorkList>();

            var xmlString = ExecuteXmlReader("GetApproverWorkList", sqlParameters.ToArray());
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//ApproverWorkList");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<ApproverWorkList>(node.OuterXml));
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

        public static int GetPendingCount(string siteName, Guid siteGuid, int? id = null, string typeCode = "default")
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@typeCode", typeCode));
            sqlParameters.Add(new SqlParameter("@SiteName", siteName));
            sqlParameters.Add(new SqlParameter("@SiteGuid", siteGuid));
            sqlParameters.Add(new SqlParameter("@OperationBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@requestVersionId", id));
            return Convert.ToInt32(ExecuteScalar("[dbo].[GetPendingCount]", sqlParameters.ToArray(), CommandType.StoredProcedure).ToString());
        }

        public static List<Errors> GetErrors(int requestId, int requestVersionId, string siteName, Guid siteGuid, string typeCode = "default")
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@RequestId", requestId));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", requestVersionId));
            sqlParameters.Add(new SqlParameter("@typeCode", typeCode));

            sqlParameters.Add(new SqlParameter("@SiteName", siteName));
            sqlParameters.Add(new SqlParameter("@SiteGuid", siteGuid));
            sqlParameters.Add(new SqlParameter("@OperationBy", Operation.OperationBy));

            List<Errors> list = CommonDao.Get<Errors>("GetErrors", sqlParameters.ToArray());
            return list;
        }

        public static List<ExecuteResult> Rollback(int WorkFlowApproverId, string siteName, Guid siteGuid)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@WorkFlowApproverId", WorkFlowApproverId));
            sqlParameters.Add(new SqlParameter("@SiteName", siteName));
            sqlParameters.Add(new SqlParameter("@SiteGuid", siteGuid));
            sqlParameters.Add(new SqlParameter("@OperationBy", Operation.OperationBy));

            List<ExecuteResult> list = CommonDao.Get<ExecuteResult>("RollbackByWorkFlowApproverId", sqlParameters.ToArray());
            return list;
        }
    }
}