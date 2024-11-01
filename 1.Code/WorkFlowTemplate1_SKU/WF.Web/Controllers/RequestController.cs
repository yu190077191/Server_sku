using MVC4Pager;
using System.Web.Mvc;
using WF.BusinessRule;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;
using System.Collections.Generic;
using System.Linq;

namespace WF.Web.Controllers
{
    public class RequestController : BaseController
    {
        WorkFlowAPIController api = new WorkFlowAPIController();

        public ActionResult Index()
        {
            return View();
        }

        #region ProjectRequirement
        public ActionResult Preview(int id)
        {
            ProjectRequirement model = GetData(id);
            return View(model);
        }

        public ActionResult ProjectRequirement(int? id = null)
        {
            if (id != null)
            {
                ProjectRequirement model = GetData((int)id);
            }

            return View();
        }

        public JsonResult SaveProjectRequirement(string json)
        {
            WF.Model.ProjectRequirement model = JsonHelper.DeserializeJson<WF.Model.ProjectRequirement>(json);
            int id = model.Id;
            if (model.Id > 0)
            {
                if (ProjectRequirementRule.UpdateProjectRequirement(model) <= 0)
                {
                    id = 0;
                }
            }
            else
            {
                id = ProjectRequirementRule.InsertProjectRequirement(model);
            }

            return Json(new { Success = id > 0, Id = id }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProjectRequirementList(int? pageIndex = null, string json = null)
        {
            Query query = new Query(pageIndex, json);
            QueryResult<WF.Model.ProjectRequirement> result = ProjectRequirementRule.GetProjectRequirementResult("GetProjectRequirementProc", query.GetParameters());
            var model = new PagedList<WF.Model.ProjectRequirement>(result.DataList, query.PageIndex, query.PageSize, result.Count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("ProjectRequirementListPartial", model);
            }
            else
            {
                if (result.Count == 0)
                {
                    return Redirect("~/Request/ProjectRequirement");
                }

                return View(model);
            }
        }
        #endregion ProjectRequirement

        public ActionResult Approvers(int id)
        {
            ProjectRequirement model = GetData(id);
            List<Nestle.WorkFlow.Model.ActionHistory> list = api.GetApprovers(id, id, (System.Guid)model.CreatedBy);
            list = list.Where(k => k.Sequence > 0).ToList();
            /*
            if (model.BusinessCaseOwner != null && list.Where(k => k.EmployeeId == model.BusinessCaseOwner).Count() == 0)
            {
                list.Insert(1, new Nestle.WorkFlow.Model.ActionHistory
                {
                    StepName = "Business Case Owner Review",
                    Title = "Business Case Owner",
                    IsManuallyAdded = true,
                    Sequence = 2,
                    Name = model.BusinessCaseOwnerName,
                    EmployeeId = model.BusinessCaseOwner
                });
            }
            */

            return View(list);
        }

        ProjectRequirement GetData(int id)
        {
            WF.Model.ProjectRequirement model = CommonRule.Get<WF.Model.ProjectRequirement>(id);
            if (model != null)
            {
                ViewBag.Json = CommonRule.GetJsonFromModel(model);
                ViewBag.commonAttachmentList = AttachmentRule.GetAttachment(model.Id, "Common");
            }

            List<Nestle.WorkFlow.Model.Button> buttonList = api.GetButtons(id, id);
            string url = Request.Url.ToString().ToLower();
            if (
                model.CreatedBy != Operation.OperationBy
                && !WorkFlowAPIController.IsAdmin
                && !WorkFlowAPIController.IsReadonlyAdmin
                && buttonList.Count() == 0
                //&& buttonList.Where(k => k.EventName.ToLower() == "submit").Count() == 0
                //&& buttonList.Where(k => k.EventName.ToLower() == "approve").Count() == 0
                //&& buttonList.Where(k => k.EventName.ToLower() == "view").Count() == 0
                //&& buttonList.Where(k => k.EventName.ToLower() == "save").Count() == 0
                )
            {
                Response.Write(LanguagesRule.Translate("Access denied!"));
                Response.End();
            }

            if (url.Contains("projectrequirement?") && buttonList.Where(k => k.EventName == "Submit").Count() == 0)
            {
                Response.Write(LanguagesRule.Translate("Invalid operation!"));
                Response.End();
            }

            ViewBag.ButtonList = buttonList;
            List<Nestle.WorkFlow.Model.ActionHistory> list = api.GetApprovers(id, id, (System.Guid)model.CreatedBy);
            list = list.Where(k => k.Sequence > 0).ToList();

            return model;
        }

        public ActionResult Approver(int? pageIndex = null, string json = null)
        {
            Query query = new Query(pageIndex, json);
            QueryResult<ApproverItem> result = api.GetApproverWorkListResult(query.GetParameters());
            var model = new PagedList<ApproverItem>(result.DataList, query.PageIndex, query.PageSize, result.Count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("PartialApproverItemList", model);
            }
            else
            {
                return View(model);
            }
        }

        public void Download(System.Guid id)
        {
            new AttachmentRule().Download(id);
        }

        public JsonResult OverWriteExcel(int id)
        {
            return Json(new { Message = CommonRule.OverWriteExcel(id) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDescription(System.Guid id, string description)
        {
            Attachment model = CommonRule.GetModel<Attachment>(guid: id);
            if (model.CreatedBy.Equals(Operation.OperationBy))
            {
                model.Description = description;
                model.ModifiedBy = Operation.OperationBy;
                model.ModifiedOn = System.DateTime.Now;
                int result = AttachmentRule.UpdateAttachment(model);
                if (result > 0)
                {
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(System.Guid id)
        {
            return Json(new { Success = AttachmentRule.Delete(id) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveNestoolInfo(string json)
        {
            ProjectRequirement newModel = JsonHelper.DeserializeJson<ProjectRequirement>(json);
            ProjectRequirement model = CommonRule.GetModel<ProjectRequirement>(newModel.Id);
            //if (WorkFlowAPIController.IsAdmin)
            //{
                model.Nestool_BR_ID = newModel.Nestool_BR_ID;
                model.NPDIIdentifier = newModel.NPDIIdentifier;
                model.ModifiedOn = System.DateTime.Now;
                model.ModifiedBy = Operation.OperationBy;
                int result = ProjectRequirementRule.UpdateProjectRequirement(model);
                if (result > 0)
                {
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
            //}

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeE2E(int id, int e2e)
        {
            return Json(new { Success = ProjectRequirementRule.ChangeE2E(id, e2e) > 0 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveInfo(string json)
        {
            ProjectRequirement_Info model = JsonHelper.DeserializeJson<ProjectRequirement_Info>(json);
            model.CreatedBy = Operation.OperationBy;
            int id = ProjectRequirement_InfoRule.InsertProjectRequirement_Info(model);
            if (id > 0)
            {
                return Json(new { Success = true, Id = id }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}
