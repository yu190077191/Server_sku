using System.Web.Mvc;
namespace WF.Web.Controllers
{
    public class HelpController : Controller
    {
        public ActionResult Index()
        {
            //return Redirect("~/Help/QRC - SKU Data Management Web-form.pdf?V" + System.DateTime.Now.ToString("yyyyMMddHHmmss"));
            return View();
        }
    }
}
