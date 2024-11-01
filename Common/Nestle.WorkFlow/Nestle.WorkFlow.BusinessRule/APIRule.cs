using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class APIRule : System.Web.UI.Page
    {
        public static List<DepartmentRole> GetRoles(string siteName, Guid siteGuid, string accountName)
        {
            List<DepartmentRole> list = CommonRule.Get<DepartmentRole>("[dbo].[GetUserRoles]", new SqlParameter[] {
                new SqlParameter("@siteName", siteName),
                new SqlParameter("@siteGuid", siteGuid),
                new SqlParameter("@accountName", accountName)
            });

            return list;
        }

        public static int SaveManuallyAddedApprovers(string json, string siteName, Guid siteGuid)
        {
            ManuallyAddedApprovers[] array = JsonHelper.DeserializeJson<ManuallyAddedApprovers[]>(json);
            string xml = SerializationHelper.Serialize(array);
            return ManuallyAddedApproversDao.SaveManuallyAddedApprovers(xml, siteName, siteGuid);
        }

        public static Instance GetInstance(string siteName, Guid siteGuid)
        {
            List<Instance> list = CommonRule.Get<Instance>("GetInstanceProc", new SqlParameter[] {
                new SqlParameter("@siteName", siteName),
                new SqlParameter("@siteGuid", siteGuid)
            });

            if (list != null && list.Count > 0)
            {
                return list[0];
            }

            return null;
        }

        public static List<ExecuteResult> SubmitRequest(int requestVersionId, int requestId = 0, string typeCode = "default", string xml = null)
        {
            return APIDao.SubmitRequest(requestVersionId, requestId, typeCode, xml);
        }

        public static List<ExecuteResult> ReviewRequest(ActionData model)
        {
            return APIDao.ReviewRequest(model);
        }

        public static List<ExecuteResult> CopyRequestForHK(ActionData model)
        {
            return APIDao.CopyRequestForHK(model);
        }
        public static List<ActionHistory> GetActionHistory(int requestId, int requestVersionId, string siteName, Guid siteGuid, Guid requestorEmployeeId, string typeCode = "default", bool autoFix = false, string storedProcedureName = "GetActionHistory", int? departmentid = null)
        {
            return APIDao.GetActionHistory(requestId, requestVersionId, siteName, siteGuid, requestorEmployeeId, typeCode, autoFix, storedProcedureName, departmentid);
        }
        public static List<ReturnStepResult> GetReturnStep(int requestId)
        {
            return APIDao.GetReturnStep(requestId);
        }

        public static string GetProcessDiv(List<Process> processList)
        {
            StringBuilder sb = new StringBuilder();
            if (processList != null && processList.Count > 0)
            {
                int i = 0;
                sb.Append("<div id=\"process\" class=\"section3\">\r\n");
                foreach (Process p in processList)
                {
                    if (i > 0)
                    {
                        string marginLeft = string.Empty;
                        if (i > 1 && (i - 1) % 3 == 0)
                        {
                            sb.Append("<div style='margin-top:55px;'>&nbsp;</div>\r\n");
                            marginLeft = " style='margin-left:13px;'";
                        }

                        sb.Append("   <div class=\"proce");
                        switch (p.ProcessState)
                        {
                            case (int)ProcessState.Completed:
                                sb.Append(" ready\"" + marginLeft + ">\r\n");
                                break;
                            case (int)ProcessState.Inprocess:
                                sb.Append(" doing\"" + marginLeft + ">\r\n");
                                break;
                            case (int)ProcessState.Wait:
                                sb.Append(" wait\"" + marginLeft + ">\r\n");
                                break;
                        }

                        sb.Append("       <ul>\r\n");
                        sb.Append("           <li class=\"tx1\">" + p.AboveLineText + "&nbsp;</li>\r\n");
                        sb.Append("       </ul>\r\n");
                        sb.Append("   </div>\r\n");
                    }

                    sb.Append("   <div class=\"node");
                    if (i == 0)
                    {
                        sb.Append(" fore");
                    }

                    switch (p.ProcessState)
                    {
                        case (int)ProcessState.Completed:
                            sb.Append(" ready\">\r\n");
                            break;
                        case (int)ProcessState.Inprocess:
                            sb.Append(" singular\">\r\n");
                            break;
                        case (int)ProcessState.Wait:
                            sb.Append(" wait\">\r\n");
                            break;
                    }

                    sb.Append("       <ul>\r\n");
                    sb.Append("           <li class=\"tx1\">" + p.Text1 + "&nbsp;</li>\r\n");
                    sb.Append("           <li class=\"tx2\">" + p.Name + "</li>\r\n");
                    sb.Append("           <li class=\"tx3\">" + p.Text3 + "</li>\r\n");
                    sb.Append("       </ul>\r\n");
                    sb.Append("   </div>\r\n");

                    i++;
                }

                sb.Append("</div>");
            }

            return sb.ToString();
        }

        public static bool IsApprover(string siteName, Guid siteGuid, string typeCode = "default")
        {
            return APIDao.IsApprover(siteName, siteGuid, typeCode);
        }

        public static bool IsHaveRole(string siteName, Guid siteGuid)
        {
            return APIDao.IsHaveRole(siteName, siteGuid);
        }

        public static QueryResult<ApproverWorkList> GetApproverWorkListResult(string siteName, Guid siteGuid, int? id = null, string typeCode = "default")
        {
            return APIDao.GetApproverWorkListResult(siteName, siteGuid, id, typeCode);
        }

        public static int GetPendingCount(string siteName, Guid siteGuid, int? id = null, string typeCode = "default")
        {
            return APIDao.GetPendingCount(siteName, siteGuid, id, typeCode);
        }

        public static List<Errors> GetErrors(int requestId, int requestVersionId, string siteName, Guid siteGuid, string typeCode = "default")
        {
            return APIDao.GetErrors(requestId, requestVersionId, siteName, siteGuid, typeCode);
        }

        public static List<ExecuteResult> Rollback(int WorkFlowApproverId, string siteName, Guid siteGuid)
        {
            return APIDao.Rollback(WorkFlowApproverId, siteName, siteGuid);
        }
    }
}
