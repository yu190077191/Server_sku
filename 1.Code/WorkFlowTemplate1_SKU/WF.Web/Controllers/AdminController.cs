using MVC4Pager;
using System.Web.Mvc;
using WF.BusinessRule;
using WF.Framework;
using WF.Framework.Helper;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace WF.Web.Controllers
{
    public class AdminController : BaseController
    {
        public JsonResult StopDebugUser()
        {
            Session.Remove("debugAccountName");
            Session.Remove("EmployeeInfo");
            Session.Remove("OperationBy");
            Session.Remove(Constants.DelegatedBy);
            Session.Remove("Nestle.WorkFlow.Employee");
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public string ExportToCSV(string keyword, string status)
        {
            string procedureName = "GetReport";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@EmployeeId", Operation.OperationBy));
            list.Add(new SqlParameter("@Keyword", keyword));
            int i = 0;
            if(!string.IsNullOrEmpty(status) && int.TryParse(status, out i))
            {
                list.Add(new SqlParameter("@Status", i));
            }

            return CommonRule.ExportToCSV(procedureName, list.ToArray());
        }

        public string ExportToCSVForApprover(int? pageIndex = null, string json = null)
        {
            Query query = new Query(pageIndex, json);
            return new WorkFlowAPIController().ExportToCSV(query.GetParameters());
        }

        #region ProjectRequirement
        public ActionResult ProjectRequirement(int? id = null)
        {
            if (id != null)
            {
                WF.Model.ProjectRequirement model = CommonRule.Get<WF.Model.ProjectRequirement>(id);
                if (model != null)
                {
                    ViewBag.Json = CommonRule.GetJsonFromModel(model);
                }
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
                return View(model);
            }
        }
        #endregion ProjectRequirement


        #region Field
        public ActionResult Field(int? id = null)
        {
            if (id != null)
            {
                WF.Model.Field model = CommonRule.Get<WF.Model.Field>(id);
                if (model != null)
                {
                    ViewBag.Json = CommonRule.GetJsonFromModel(model);
                }
            }

            return View();
        }

        public JsonResult SaveField(string json)
        {
            WF.Model.Field model = JsonHelper.DeserializeJson<WF.Model.Field>(json);
            int id = model.Id;
            if (model.Id > 0)
            {
                if (FieldRule.UpdateField(model) <= 0)
                {
                    id = 0;
                }
            }
            else
            {
                id = FieldRule.InsertField(model);
            }

            return Json(new { Success = id > 0, Id = id }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldList(int? pageIndex = null, string json = null)
        {
            Query query = new Query(pageIndex, json);
            QueryResult<WF.Model.Field> result = FieldRule.GetFieldResult("GetFieldProc", query.GetParameters());
            var model = new PagedList<WF.Model.Field>(result.DataList, query.PageIndex, query.PageSize, result.Count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("FieldListPartial", model);
            }
            else
            {
                return View(model);
            }
        }
        #endregion Field


        #region Dictionary
        public ActionResult Dictionary(int? id = null)
        {
            if (id != null)
            {
                WF.Model.Dictionary model = CommonRule.Get<WF.Model.Dictionary>(id);
                if (model != null)
                {
                    ViewBag.Json = CommonRule.GetJsonFromModel(model);
                }
            }

            return View();
        }

        public JsonResult SaveDictionary(string json)
        {
            WF.Model.Dictionary model = JsonHelper.DeserializeJson<WF.Model.Dictionary>(json);
            int id = model.Id;
            if (model.Id > 0)
            {
                if (DictionaryRule.UpdateDictionary(model) <= 0)
                {
                    id = 0;
                }
            }
            else
            {
                id = DictionaryRule.InsertDictionary(model);
            }

            return Json(new { Success = id > 0, Id = id }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DictionaryList(int? pageIndex = null, string json = null)
        {
            Query query = new Query(pageIndex, json);
            QueryResult<WF.Model.Dictionary> result = DictionaryRule.GetDictionaryResult("GetDictionaryProc", query.GetParameters());
            var model = new PagedList<WF.Model.Dictionary>(result.DataList, query.PageIndex, query.PageSize, result.Count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("DictionaryListPartial", model);
            }
            else
            {
                return View(model);
            }
        }
        #endregion Dictionary


        #region MaterialGroup1
        public ActionResult MaterialGroup1(int? id = null)
        {
            if (id != null)
            {
                WF.Model.MaterialGroup1 model = CommonRule.Get<WF.Model.MaterialGroup1>(id);
                if (model != null)
                {
                    ViewBag.Json = CommonRule.GetJsonFromModel(model);
                }
            }

            return View();
        }

        public JsonResult SaveMaterialGroup1(string json)
        {
            WF.Model.MaterialGroup1 model = JsonHelper.DeserializeJson<WF.Model.MaterialGroup1>(json);
            int id = model.Id;
            try
            {
                if (model.Id > 0)
                {
                    if (MaterialGroup1Rule.UpdateMaterialGroup1(model) <= 0)
                    {
                        id = 0;
                    }
                }
                else
                {
                    id = MaterialGroup1Rule.InsertMaterialGroup1(model);
                }

                return Json(new { Success = id > 0, Id = id }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { Success = false, Message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MaterialGroup1List(int? pageIndex = null, string json = null)
        {
            Query query = new Query(pageIndex, json);
            QueryResult<WF.Model.MaterialGroup1> result = MaterialGroup1Rule.GetMaterialGroup1Result("GetMaterialGroup1Proc", query.GetParameters());
            var model = new PagedList<WF.Model.MaterialGroup1>(result.DataList, query.PageIndex, query.PageSize, result.Count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("MaterialGroup1ListPartial", model);
            }
            else
            {
                return View(model);
            }
        }
        #endregion MaterialGroup1

    }
}
