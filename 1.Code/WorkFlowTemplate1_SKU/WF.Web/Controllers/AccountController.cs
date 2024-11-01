using System.Web.Mvc;

namespace WF.Web.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult SSOLogin()
        {
            return new BaseController().ReturnFromSSOServer(Session, System.Web.HttpContext.Current);
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            return new BaseController().SSOLogOff(Session);
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}