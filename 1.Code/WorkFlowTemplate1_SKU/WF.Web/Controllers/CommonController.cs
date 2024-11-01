using MVC4Pager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;
using WF.BusinessRule;
using WF.DataAccess;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.Web.Controllers
{
    public class CommonController : BaseController
    {
        public static readonly string siteName = System.Configuration.ConfigurationManager.AppSettings["SiteName"];
        public static readonly System.Guid siteGuid = System.Guid.Parse(System.Configuration.ConfigurationManager.AppSettings["SiteGuid"]);

        public static bool IsAdmin
        {
            get
            {
                List<Nestle.WorkFlow.Model.DepartmentRole> list = Nestle.WorkFlow.BusinessRule.APIRule.GetRoles(siteName, siteGuid, WF.Framework.Operation.UserName);
                var systemAdmin = list.Where(k => !string.IsNullOrEmpty(k.Department) && k.Department.ToLower() == "system" && k.RoleName.ToLower() == "admin");
                return systemAdmin != null && systemAdmin.Count() > 0;
            }
        }

        public ActionResult Roles(int? pageIndex = null, string json = null)
        {
            if (Request.IsAjaxRequest())
            {
                Nestle.WorkFlow.Framework.Query query = new Nestle.WorkFlow.Framework.Query();
                //ViewBag.RoleList = Nestle.WorkFlow.BusinessRule.RoleRule.GetRole("GetRole", query.GetParameters(siteName, siteGuid));
                //List<Nestle.WorkFlow.Model.Department> dptList = Nestle.WorkFlow.BusinessRule.DepartmentRule.GetDepartment("GetDepartment", query.GetParameters(siteName, siteGuid));
                //ViewBag.DepartmentList = dptList;
                //ViewBag.BUList = dptList.Where(k => k.Type == "BU").ToList();

                query = new Nestle.WorkFlow.Framework.Query(pageIndex, json);
                QueryResult<DepartmentRole> result = CommonRule.GetQueryResult<DepartmentRole>("GetDepartmentRole", query.GetParameters(siteName, siteGuid));
                var model = new PagedList<DepartmentRole>(result.DataList, query.PageIndex, query.PageSize, result.Count);

                return PartialView("PartialRolesList", model);
            }
            else
            {
                return View();
            }
        }

        public ActionResult RolesPreview(int id)
        {
            List<DepartmentRole> list = CommonRule.Get<DepartmentRole>("GetRoleChangeList", new[] {
                    new SqlParameter("@EmployeeId", Operation.OperationBy),
                    new System.Data.SqlClient.SqlParameter("@id", id)
                });
            return View(list);
        }

        public string ApplyRoleChange(string checkedIds, string mode, string email, string reason)
        {
            List<CommonSearchResult> list = CommonRule.Get<CommonSearchResult>("ApplyRoleChange", new[] {
                    new SqlParameter("@EmployeeId", Operation.OperationBy),
                    new System.Data.SqlClient.SqlParameter("@checkedIds", checkedIds),
                    new System.Data.SqlClient.SqlParameter("@mode", mode),
                    new System.Data.SqlClient.SqlParameter("@email", email),
                    new System.Data.SqlClient.SqlParameter("@reason",reason)
                });

            return list[0].Description;
        }

        public string ExportToCSV(string type, string keyword, int status)
        {
            string procedureName = "GetReport";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@EmployeeId", Operation.OperationBy));
            list.Add(new SqlParameter("@Keyword", keyword));
            list.Add(new SqlParameter("@Status", status));
            list.Add(new SqlParameter("@TypeCode", type));
            return CommonRule.ExportToCSV(procedureName, list.ToArray());
        }

        public string AutoComplete()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string type = Request.QueryString["type"];
            string subType = Request.Params["subType"];
            if (string.IsNullOrEmpty(type))
            {
                return null;
            }

            string q = Request.Params["q"];
            List<CommonSearchResult> list = new List<CommonSearchResult>();
            if (!string.IsNullOrEmpty(q))
            {
                q = q.Trim();

                list = CommonRule.Get<CommonSearchResult>("GetAutoComplete", new[] { 
                    new System.Data.SqlClient.SqlParameter("@type", type), 
                    new System.Data.SqlClient.SqlParameter("@subType", subType),
                    new System.Data.SqlClient.SqlParameter("@Keyword", q)
                });

            }

            string result = JsonHelper.SerializeJson(list);
            return result;
        }

        public FileContentResult GetFileFromBitmap(Bitmap image)
        {
            var sm = new MemoryStream();
            image.Save(sm, ImageFormat.Gif);
            return File(sm.ToArray(), "image/gif");
        }

        public ActionResult QRcode(string data, int scale = 4, int version = 7)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();

            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;

            qrCodeEncoder.QRCodeScale = scale;

            qrCodeEncoder.QRCodeVersion = version;

            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

            return GetFileFromBitmap(qrCodeEncoder.Encode(data, System.Text.Encoding.Default));
        }

        public ActionResult Search(int? pageIndex = null, string json = null)
        {
            return View();
        }

        public static System.Web.Mvc.MvcHtmlString GetSelectOption(string typeCode, string selectedValue)
        {
            return CommonRule.GetSelectOption(typeCode, null, true, true, selectedValue);
        }

        public JsonResult UpdateRecordStatus(string tableName, int? id = null, Guid? guid = null, Guid? modifiedBy = null, Guid? createdBy = null, int? recordStatus = null)
        {
            int result = CommonDao.UpdateRecordStatus(tableName, id.ToString(), (int)recordStatus);
            if (result > 0)
            {
                return Json(new { Success = true });
            }

            return Json(new { Success = false });
        }

        public JsonResult Delete(string tableName, int id)
        {
            return UpdateRecordStatus(tableName, id, null, Operation.OperationBy, Operation.OperationBy, 2);
        }

        public JsonResult Restore(string tableName, int id)
        {
            return UpdateRecordStatus(tableName, id, null, Operation.OperationBy, Operation.OperationBy, 0);
        }

        public JsonResult DeleteGuid(string tableName, Guid id)
        {
            return UpdateRecordStatus(tableName, null, id, Operation.OperationBy, Operation.OperationBy, 2);
        }

        public JsonResult RestoreGuid(string tableName, Guid id)
        {
            return UpdateRecordStatus(tableName, null, id, Operation.OperationBy, Operation.OperationBy, 0);
        }
    }
}
