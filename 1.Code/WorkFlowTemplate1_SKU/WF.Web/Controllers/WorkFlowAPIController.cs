using MVC4Pager;
using Nestle.WorkFlow.BusinessRule;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System;

namespace WF.Web.Controllers
{
    public class WorkFlowAPIController : BaseController
    {
        public static readonly string siteName = System.Configuration.ConfigurationManager.AppSettings["SiteName"];
        public static readonly System.Guid siteGuid = System.Guid.Parse(System.Configuration.ConfigurationManager.AppSettings["SiteGuid"]);

        public static string GetTypeCode(int id)
        {
            if(id == 0)
            {
                return "default";
            }

            WF.Model.RequestVersion model = WF.BusinessRule.CommonRule.Get<WF.Model.RequestVersion>(id);
            return model.WorkFlowTypeCode;
        }

        public static bool IsAdmin
        {
            get
            {
                List<DepartmentRole> list = APIRule.GetRoles(siteName, siteGuid, WF.Framework.Operation.UserName);
                var systemAdmin = list.Where(k => !string.IsNullOrEmpty(k.Department) && k.Department.ToLower() == "system" && k.RoleName.ToLower() == "admin");
                return systemAdmin != null && systemAdmin.Count() > 0;
            }
        }

        public static bool IsReadonlyAdmin
        {
            get
            {
                List<DepartmentRole> list = APIRule.GetRoles(siteName, siteGuid, WF.Framework.Operation.UserName);
                var readonlyadmin = list.Where(k => !string.IsNullOrEmpty(k.RoleName) && k.RoleName.ToLower() == "readonlyadmin");
                return readonlyadmin != null && readonlyadmin.Count() > 0;
            }
        }

        public static bool IsInRole(string roleName)
        {
            List<DepartmentRole> list = APIRule.GetRoles(siteName, siteGuid, WF.Framework.Operation.UserName);
            var systemAdmin = list.Where(k => k.RoleName != null && k.RoleName.ToLower().Contains(roleName.ToLower()));
            return systemAdmin != null && systemAdmin.Count() > 0;
        }

        public static List<DepartmentRole> MyRoles
        {
            get
            {
                List<DepartmentRole> list = APIRule.GetRoles(siteName, siteGuid, WF.Framework.Operation.UserName);
                if(list == null)
                {
                    list = new List<DepartmentRole>();
                }

                return list;
            }
        }

        public static bool IsDC
        {
            get
            {
                List<DepartmentRole> list = APIRule.GetRoles(siteName, siteGuid, WF.Framework.Operation.UserName);
                if (list != null && list.Count > 0)
                {
                    var obj = list.Where(k => k.Department != null && k.Department.ToUpper().IndexOf("DC") < 0);
                    if(obj.Count() > 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public JsonResult SaveApprovers(string json)
        {
            return Json(new { Success = APIRule.SaveManuallyAddedApprovers(json, siteName, siteGuid) > 0 }, JsonRequestBehavior.AllowGet);
        }

        public List<Nestle.WorkFlow.Model.ActionHistory> GetApprovers(int requestId, int id, System.Guid requestorEmployeeId, string typeCode = "default")
        {
            typeCode = GetTypeCode(id);
            return APIRule.GetActionHistory(requestId, id, siteName, siteGuid,requestorEmployeeId, typeCode);
        }
        public List<Nestle.WorkFlow.Model.ReturnStepResult> GetReturnStep(int requestId)
        {
            return APIRule.GetReturnStep(requestId);
        }

        public List<Nestle.WorkFlow.Model.Button> GetButtons(int requestId, int? id, string typeCode = "default")
        {
            if (id == null)
            {
                id = 0;
            }

            typeCode = GetTypeCode((int)id);
            return ActionRule.GetButtons(requestId, id != null ? (int)id : 0, siteName, siteGuid, typeCode);
        }

        public JsonResult ReviewRequest(string json)
        {
            try
            {
                ActionData model = JsonHelper.DeserializeJson<ActionData>(json);
                model.SiteName = siteName;
                model.SiteGuid = siteGuid;
                WF.Model.RequestVersion rv = WF.BusinessRule.CommonRule.Get<WF.Model.RequestVersion>(model.RequestVersionId);
                model.TypeCode = rv.WorkFlowTypeCode;
                model.DepartmentId = rv.DepartmentId;
                List<Nestle.WorkFlow.Model.ExecuteResult> list = APIRule.ReviewRequest(model);
                bool isSuccess = list.Where(k => k.IsSuccess).Count() > 0;
                string message = list != null && list.Count > 0 ? list[0].Message : string.Empty;
                if (isSuccess && list[0].Status > 0)
                {
                    List<Nestle.WorkFlow.Model.ActionHistory> historyList = GetApprovers(model.RequestVersionId, model.RequestVersionId, (System.Guid)Operation.OperationBy);
                    if (historyList != null && historyList.Where(k => k.Time == null).Count() == 1 && historyList.Where(k => k.Time == null).FirstOrDefault().StepName.ToLower().Contains("npdi"))
                    {
                        list[0].Status = 2;
                    }

                    int r = WF.BusinessRule.RequestVersionRule.UpdateStatus(model.RequestVersionId, list[0].Status);
                }

                return Json(new { Success = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message + " stack: " + ex.StackTrace }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CopyRequestForHK(string json)
        {
            try
            {
                ActionData model = JsonHelper.DeserializeJson<ActionData>(json);
                model.SiteName = siteName;
                model.SiteGuid = siteGuid;
                WF.Model.RequestVersion rv = WF.BusinessRule.CommonRule.Get<WF.Model.RequestVersion>(model.RequestVersionId);
                model.TypeCode = rv.WorkFlowTypeCode;
                model.DepartmentId = rv.DepartmentId;
                List<Nestle.WorkFlow.Model.ExecuteResult> list = APIRule.CopyRequestForHK(model);
                bool isSuccess = list.Where(k => k.IsSuccess).Count() > 0;
                string message = list != null && list.Count > 0 ? list[0].Message : string.Empty;
                if (isSuccess && list[0].Status > 0)
                {
                    List<Nestle.WorkFlow.Model.ActionHistory> historyList = GetApprovers(model.RequestVersionId, model.RequestVersionId, (System.Guid)Operation.OperationBy);
                    if (historyList != null && historyList.Where(k => k.Time == null).Count() == 1 && historyList.Where(k => k.Time == null).FirstOrDefault().StepName.ToLower().Contains("npdi"))
                    {
                        list[0].Status = 2;
                    }

                    int r = WF.BusinessRule.RequestVersionRule.UpdateStatus(model.RequestVersionId, list[0].Status);
                }

                return Json(new { Success = isSuccess, Message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message + " stack: " + ex.StackTrace }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Roles(int? pageIndex = null, string json = null)
        {
            if (!IsAdmin)
            {
                return Redirect("~/");
            }

            Query query = new Query();
            ViewBag.RoleList = RoleRule.GetRole("GetRole", query.GetParameters(siteName, siteGuid));
            List<Department> dptList = DepartmentRule.GetDepartment("GetDepartment", query.GetParameters(siteName, siteGuid));
            ViewBag.DepartmentList = dptList;
            ViewBag.BUList = dptList.Where(k=>k.Type == "BU").ToList();

            query = new Query(pageIndex, json);
            QueryResult<DepartmentRole> result = DepartmentRoleRule.GetDepartmentRoleResult("GetDepartmentRoleFullList", query.GetParameters(siteName, siteGuid));
            var model = new PagedList<DepartmentRole>(result.DataList, query.PageIndex, query.PageSize, result.Count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("PartialRolesList", model);
            }
            else
            {
                return View(model);
            }
        }

        public JsonResult AddDepartmentRole(System.Guid employeeId, int departmentId, int roleId, int buId)
        {
            if (!IsAdmin)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }

            int result = DepartmentRoleRule.AddDepartmentRole(new DepartmentRole
            {
                EmployeeId = employeeId,
                DepartmentId = departmentId,
                RoleId = roleId,
                BUDepartmentId = buId
            }, siteName, siteGuid);

            if (result > 0)
            {
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDepartmentRole(int id)
        {
            if (!IsAdmin)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }

            int result = DepartmentRoleRule.DeleteDepartmentRole(id, siteName, siteGuid);

            if (result > 0)
            {
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        public bool IsApprover(string typeCode = "default")
        {
            return APIRule.IsApprover(siteName, siteGuid, typeCode);
        }

        public bool IsHaveRole()
        {
            return APIRule.IsHaveRole(siteName, siteGuid);
        }

        public WF.Framework.QueryResult<WF.Model.ApproverItem> GetApproverWorkListResult(SqlParameter[] sqlParameters, int? id = null, string typeCode = "default")
        {
            if (id == null)
            {
                id = 0;
            }

            typeCode = GetTypeCode((int)id);
            //QueryResult<ApproverWorkList> r = APIRule.GetApproverWorkListResult(siteName, siteGuid, id, typeCode);

            WF.Framework.QueryResult<WF.Model.ApproverItem> result = new WF.Framework.QueryResult<WF.Model.ApproverItem>();
            //string xml = SerializationHelper.Serialize(r.DataList);
            //result.Count = r.Count;
            List<SqlParameter> li = sqlParameters.ToList();
            //li.Add(new SqlParameter("@xml", xml));
            li.Add(new SqlParameter("@OperationBy", Operation.OperationBy));
            result = WF.BusinessRule.CommonRule.GetQueryResult<WF.Model.ApproverItem>("GetApproverWorkList", li.ToArray());
            return result;
        }

        public string ExportToCSV(SqlParameter[] sqlParameters, int? id = null, string typeCode = "default")
        {
            if (id == null)
            {
                id = 0;
            }

            typeCode = GetTypeCode((int)id);
            QueryResult<ApproverWorkList> r = APIRule.GetApproverWorkListResult(siteName, siteGuid, id, typeCode);

            WF.Framework.QueryResult<WF.Model.ApproverItem> result = new WF.Framework.QueryResult<WF.Model.ApproverItem>();
            string xml = SerializationHelper.Serialize(r.DataList);
            result.Count = r.Count;
            List<SqlParameter> li = sqlParameters.ToList();
            li.Add(new SqlParameter("@xml", xml));
            li.Add(new SqlParameter("@OperationBy", Operation.OperationBy));
            result.DataList = WF.BusinessRule.CommonRule.Get<WF.Model.ApproverItem>("GetApproverWorkList", li.ToArray());

            return WF.BusinessRule.CommonRule.ExportToCSV("GetApproverWorkListReport", li.ToArray());
        }

        public int MyPendingCount
        {
            get
            {
                QueryResult<ApproverWorkList> r = APIRule.GetApproverWorkListResult(siteName, siteGuid);
                if (r != null && r.Count > 0)
                {
                    var pending = r.DataList.Where(k => k.IsPending);
                    return pending.Count();
                }

                return 0;
            }
        }

        public ActionResult DelegateRole(int? pageIndex = null, string json = null)
        {
            Query query = new Query();
            QueryResult<DepartmentRoleDeputy> result = DepartmentRoleDeputyRule.GetDepartmentRoleDeputyResult("GetMyDepartmentRoleToDeputy", query.GetParameters(siteName, siteGuid, pageIndex, json));
            var model = new PagedList<DepartmentRoleDeputy>(result.DataList, query.PageIndex, query.PageSize, result.Count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("PartialDelegateRoleList", model);
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult AddDelegateRole(int? id = null)
        {
            Query query2 = new Query();
            query2.Conditions.Add("@ShowMyRoles", "1");
            QueryResult<DepartmentRoleDeputy> result2 = DepartmentRoleDeputyRule.GetDepartmentRoleDeputyResult("GetMyDepartmentRoleToDeputy", query2.GetParameters(siteName, siteGuid));
            List<DepartmentRoleDeputy> myRoles = result2.DataList;
            ViewBag.myRoles = myRoles;

            DepartmentRoleDeputy model = null;
            if (id != null)
            {
                Query query = new Query();
                query.Conditions.Add("@Id", id.ToString());
                QueryResult<DepartmentRoleDeputy> result = DepartmentRoleDeputyRule.GetDepartmentRoleDeputyResult("GetMyDepartmentRoleToDeputy", query.GetParameters(siteName, siteGuid));
                if (result.Count > 0)
                {
                    model = result.DataList[0];
                    ViewBag.Json = CommonRule.GetJsonFromModel(model);
                }
            }

            return View(model);
        }

        public JsonResult SaveDelegateRoleSetting(string json)
        {
            DepartmentRoleDeputy model = JsonHelper.DeserializeJson<DepartmentRoleDeputy>(json);
            if (model.Id > 0)
            {
                Query query = new Query();
                query.Conditions.Add("@Id", model.Id.ToString());
                QueryResult<DepartmentRoleDeputy> result2 = DepartmentRoleDeputyRule.GetDepartmentRoleDeputyResult("GetMyDepartmentRoleToDeputy", query.GetParameters(siteName, siteGuid));
                if (result2.Count > 0)
                {
                    DepartmentRoleDeputy oldModel = result2.DataList[0];
                    oldModel.RecordStatus = 2;
                    oldModel.ModifiedBy = Operation.OperationBy;
                    oldModel.ModifiedOn = System.DateTime.Now;
                    DepartmentRoleDeputyRule.UpdateDepartmentRoleDeputy(oldModel);
                }
            }

            model.CreatedBy = (System.Guid)Operation.OperationBy;
            model.CreatedTime = System.DateTime.Now;
            int result = DepartmentRoleDeputyRule.InsertDepartmentRoleDeputy(model);
            if (result <= 0)
            {
                return Json(new { Success = result > 0, Message = "Sorry, failed!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = result > 0, Message = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDepartmentRoleDeputy(int id)
        {
            Query query = new Query();
            query.Conditions.Add("@Id", id.ToString());
            QueryResult<DepartmentRoleDeputy> result2 = DepartmentRoleDeputyRule.GetDepartmentRoleDeputyResult("GetMyDepartmentRoleToDeputy", query.GetParameters(siteName, siteGuid));
            int result = 0;
            if (result2.Count > 0)
            {
                DepartmentRoleDeputy oldModel = result2.DataList[0];
                oldModel.RecordStatus = 2;
                oldModel.ModifiedBy = Operation.OperationBy;
                oldModel.ModifiedOn = System.DateTime.Now;
                result = DepartmentRoleDeputyRule.UpdateDepartmentRoleDeputy(oldModel);
            }

            if (result <= 0)
            {
                return Json(new { Success = result > 0, Message = "Sorry, failed!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = result > 0, Message = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        public List<DepartmentRoleDeputy> MyDeputyRoles
        {
            get
            {
                List<DepartmentRoleDeputy> list = DepartmentRoleDeputyRule.GetDepartmentRoleDeputy(siteName, siteGuid);
                return list;
            }
        }

        public bool IsLargeOrderDeputy
        {
            get
            {
                if (MyDeputyRoles != null && MyDeputyRoles.Where(k => k.Role.ToLower() == "Factory Engineer".ToLower()).Count() > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public int PendingCount
        {
            get
            {
                return APIRule.GetPendingCount(siteName, siteGuid);
            }
        }

        public List<Errors> GetErrors(int requestId, int id, string typeCode = "default")
        {
            typeCode = GetTypeCode(id);
            return APIRule.GetErrors(requestId, id, siteName, siteGuid, typeCode);
        }

        public JsonResult DebugUser(string accountName)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["Debug"] != "true" || !IsAdmin)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }

            WF.Framework.Operation.Simulate(accountName);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        public List<Nestle.WorkFlow.Model.ActionHistory> GetApprovers(int requestId, int id, string typeCode = "default", bool autoFix = false)
        {
            typeCode = GetTypeCode(id);
            Guid requestorEmployeeId = (Guid)Operation.OperationBy;
            WF.Model.RequestVersion rv = WF.BusinessRule.CommonRule.Get<WF.Model.RequestVersion>(id);
            requestorEmployeeId = (Guid)rv.CreatedBy;
            string storedProcedureName = "GetActionHistory";
            int departmentid = rv.DepartmentId;

            if (IsAdmin)
            {
                List<ActionHistory> li = APIRule.GetActionHistory(requestId, id, siteName, siteGuid, requestorEmployeeId, typeCode, autoFix, "[FixApprover]", departmentid);
                if (li != null && li.Where(k => k.CheckResult != "OK").Count() > 0)
                {
                    return li;
                }
            }

            return APIRule.GetActionHistory(requestId, id, siteName, siteGuid, requestorEmployeeId, typeCode, false, storedProcedureName, departmentid);
        }

        public JsonResult AutoFixWorkFlow(int requestId, int id)
        {
            string typeCode = GetTypeCode(id);
            WF.Model.RequestVersion model = WF.BusinessRule.CommonRule.Get<WF.Model.RequestVersion>(id);
            Guid requestorEmployeeId = (Guid)model.CreatedBy;

            if (IsAdmin)
            {
                List<ActionHistory> li = APIRule.GetActionHistory(requestId, id, siteName, siteGuid, requestorEmployeeId, typeCode, true, "[FixApprover]");
                if (li != null && li.Where(k => k.CheckResult != "OK").Count() > 0)
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Rollback(int WorkFlowApproverId)
        {
            if (!IsAdmin)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }

            List<ExecuteResult> li = APIRule.Rollback(WorkFlowApproverId, siteName, siteGuid);
            return Json(new { Success = li != null && li.Count > 0 && li[0].IsSuccess }, JsonRequestBehavior.AllowGet);
        }
    }
}