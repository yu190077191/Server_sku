using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WF.Model;
using WF.Framework.Helper;
using WF.BusinessRule;

namespace WF.Web.Controllers
{
    public class HomeController : BaseController
    {
        public string heartBeat(int heartBeatTimes)
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                return User.Identity.Name + "|" + heartBeatTimes.ToString() + "|" + System.DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            else
            {
                return "Sorry, user info is null!|" + heartBeatTimes.ToString() + "|" + System.DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }

        public ActionResult Debug()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
            //WorkFlowAPIController api = new WorkFlowAPIController();
            //if(api.IsApprover())
            //{
            //    return Redirect("~/WriteOff/Approver");
            //}
            //else
            //{
            //    return Redirect("~/WriteOff/RequestVersionList");
            //}
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string SKU_Email_Tool_API(string typeCode, int id, string xml)
        {
            return WF.BusinessRule.CommonRule.SKU_Email_Tool_API(typeCode, id, xml);
        }

        public string SendEmail(string json)
        {
            APIInfo model = JsonHelper.DeserializeJson<APIInfo>(json);
            string xml = SerializationHelper.Serialize(model);
            return WF.BusinessRule.CommonRule.SKU_Email_Tool_API("SendEmail", model.Id, xml);
        }

        /// <summary>
        /// 请求消息通知
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMsgInfo()
        {
            var msg_Info = CommonRule.GetQueryResult<MsgInfo>("GetMsgInfo", null).DataList;
            var msgInfo = msg_Info != null ? msg_Info.FirstOrDefault() : null;
            //ViewBag.msgInfosd = msgInfo;
            return Json(new { DataInfo = msgInfo }, JsonRequestBehavior.AllowGet);
        }
    }
}
