using MVC4Pager;
using System.Web.Mvc;
using WF.BusinessRule;
using WF.Framework;
using WF.Model;

namespace WF.Web.Controllers
{
    public class EmployeeController : BaseController
    {
        public ActionResult EmployeeList(int? pageIndex = null, string json = null)
        {
            Query query = new Query(pageIndex, json);
            query.PageSize = 10;
            QueryResult<WF.Model.Employee> result = CommonRule.GetQueryResult<Employee>("GetEmployeeProc", query.GetParameters());
            var model = new PagedList<WF.Model.Employee>(result.DataList, query.PageIndex, query.PageSize, result.Count);
            if (Request.IsAjaxRequest())
            {
                string viewName = CommonRule.GetParameterValue(query.GetParameters(), "ViewName");
                if (string.IsNullOrEmpty(viewName))
                {
                    viewName = "EmployeeListPartial";
                }

                return PartialView(viewName, model);
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
