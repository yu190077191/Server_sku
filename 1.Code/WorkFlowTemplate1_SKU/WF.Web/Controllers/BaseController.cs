using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Security;
using WF.BusinessRule;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.Web.Controllers
{
    public class BaseController : Controller
    {

        public static string WeChatSessionKey = "WeChatSessionKey";
        //public Nestle.WeChat.Model.WeChatUserInfoEntity WeChatUser
        //{
        //    get
        //    {
        //        return Nestle.WeChat.Helper.WeChatBase.GetUserSession(System.Web.HttpContext.Current);
        //    }
        //}

        #region SSO Controller

        string debugSession = "debugSession";
        public void DebugOnly()
        {
            Nestle.Internet.SSO.Client.Helper.UserInfo userInfo = new Nestle.Internet.SSO.Client.Helper.UserInfo
            { 
                //HKChanAv
                UserName = "CNSunJi1", 
                Email = "Jing.Sun1@CN.nestle.com",
                UserId = 0
            };

            if (userInfo != null)
            {
                Employee u = Operation.CreateByName(userInfo.UserName);
                Session[userInfoSessionName] = u;
                //Session[WeChatSessionKey] = new Nestle.WeChat.Model.WeChatUserInfoEntity
                //{
                //    userid = u.AccountName,
                //    name = u.DisplayName
                //};

                string returnUrl = "~/";
                if (Session["ReturnUrl"] != null)
                {
                    returnUrl = Session["ReturnUrl"].ToString();
                }

                Session[debugSession] = true;
                PropertyInfo Info = System.Web.HttpContext.Current.Request.QueryString.GetType().GetProperty("IsReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                if (Info != null) Info.SetValue(System.Web.HttpContext.Current.Request.QueryString, false, null);
                System.Web.HttpContext.Current.Request.QueryString["ReturnUrl"] = returnUrl;
                FormsAuthentication.RedirectFromLoginPage(userInfo.Email, userInfo.RememberMe);
            }
        }

        public Nestle.Internet.SSO.Client.Helper.UserInfo Me
        {
            get
            {
                if (Session[userInfoSessionName] != null)
                {
                    return (Nestle.Internet.SSO.Client.Helper.UserInfo)Session[userInfoSessionName];
                }
                else if (!string.IsNullOrEmpty(User.Identity.Name))
                {
                    CreateSessionFromSavedUserName();
                }

                return null;
            }
        }

        public int UserId
        {
            get
            {
                return (Me != null ? Me.UserId : 0);
            }
        }

        public string UserName
        {
            get
            {
                return (Me != null ? Me.UserName : string.Empty);
            }
        }

        const string userInfoSessionName = "SSOUserInfo";

        protected void CreateSessionFromSavedUserName()
        {
            if (Operation.Employee != null)
            {
                Session[userInfoSessionName] = new Nestle.Internet.SSO.Client.Helper.UserInfo
                {
                    UserId = 0,
                    UserName = Operation.Employee.AccountName
                };
            }
        }

        protected void CreateSessionFromSavedUserName(ActionExecutingContext filterContext)
        {
            CreateSessionFromSavedUserName();
        }

        public ActionResult ReturnFromSSOServer(System.Web.HttpSessionStateBase context, System.Web.HttpContext httpContext)
        {
            string url = System.Web.HttpContext.Current.Request.Url.ToString();
            if (!url.Contains(Nestle.Internet.SSO.Client.Helper.Constants.ChangeSessionIdStamp))
            {
                url += Nestle.Internet.SSO.Client.Helper.Constants.ChangeSessionIdStamp;
                if (context["ReturnUrl"] != null)
                {
                    string returnUrl = context["ReturnUrl"].ToString();
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        url += "&ReturnUrl=" + returnUrl;
                    }
                }

                System.Web.SessionState.SessionIDManager Manager = new System.Web.SessionState.SessionIDManager();
                string newID = Manager.CreateSessionID(httpContext);
                string oldID = httpContext.Session.SessionID;
                bool redirected = false;
                bool isAdded = false;
                Manager.SaveSessionID(httpContext, newID, out redirected, out isAdded);
                return Redirect(url);
            }
            else
            {
                Nestle.Internet.SSO.Client.Helper.UserInfo userInfo = Nestle.Internet.SSO.Client.Helper.SSORule.Login();
                if (userInfo != null)
                {
                    Employee u = Operation.CreateByName(userInfo.UserName);
                    context[userInfoSessionName] = u;
                    //context[WeChatSessionKey] = new Nestle.WeChat.Model.WeChatUserInfoEntity {
                    //    userid = u.AccountName,
                    //    name = u.DisplayName
                    //};

                    string returnUrl = "~/";
                    if (context["ReturnUrl"] != null)
                    {
                        returnUrl = context["ReturnUrl"].ToString();
                    }

                    string _returnURL = System.Web.HttpContext.Current.Request.QueryString["ReturnUrl"];
                    if(!string.IsNullOrEmpty(_returnURL))
                    {
                        returnUrl = _returnURL;
                    }

                    PropertyInfo Info = System.Web.HttpContext.Current.Request.QueryString.GetType().GetProperty("IsReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (Info != null) Info.SetValue(System.Web.HttpContext.Current.Request.QueryString, false, null);
                    System.Web.HttpContext.Current.Request.QueryString["ReturnUrl"] = returnUrl;
                    FormsAuthentication.RedirectFromLoginPage(userInfo.UserName, userInfo.RememberMe);
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult SSOLogOff(System.Web.HttpSessionStateBase context)
        {
            FormsAuthentication.SignOut();
            context.Remove(userInfoSessionName);
            return RedirectToAction("Index", "Home");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string absolutePath = Request.Url.AbsolutePath;
            if (Session[WeChatSessionKey] == null && string.IsNullOrEmpty(User.Identity.Name) && !absolutePath.ToLower().Contains("ssologin") && Session[debugSession] == null)
            {
                string url = Request.Url.ToString();
                string fromIntranet = "";
                if (url.Contains("fromintranet"))
                {
                    url = url.Replace("?fromintranet", "").Replace("&fromintranet", "");
                    fromIntranet = "&fromintranet";
                }

                Session["ReturnUrl"] = url;
#if DEBUG
                DebugOnly();
#endif
#if !DEBUG
                filterContext.Result = new RedirectResult(Nestle.Internet.SSO.Client.Helper.SSORule.SSOLoginUrl + fromIntranet);
#endif
                return;
            }
            else if (!string.IsNullOrEmpty(User.Identity.Name) && Session[userInfoSessionName] == null)
            {
                CreateSessionFromSavedUserName(filterContext);
            }

            if (Request.Url.AbsolutePath != "/")
            {
                string path = Request.Url.AbsolutePath.ToLower();
                string url = Request.Url.ToString().ToLower();

                if (!CheckAccess(Request.Url.AbsolutePath))
                {
                    filterContext.Result = new RedirectResult("~/Error/Error401");
                    return;
                }
            }

            //if (Operation.IsDelegated || Operation.IsInDebugMode())
            //{
            //    DelegateLogRule.RecordDelegateLog(Request.Url.ToString());
            //}

            base.OnActionExecuting(filterContext);
        }

        public static bool CheckAccess(string path)
        {
            return true;

            //if (path.ToLowerInvariant().IndexOf("/common") == 0 || path.ToLowerInvariant().IndexOf("/home") == 0 || IsAdmin)
            //{
            //    return true;
            //}

            //if (!string.IsNullOrEmpty(Operation.UserInfo.Title))
            //{
            //    string _title = Operation.UserInfo.Title;
            //    string roleSetting = System.Configuration.ConfigurationManager.AppSettings[_title + "_Roles"];
            //    if (!string.IsNullOrEmpty(roleSetting))
            //    {
            //        return PathContains(roleSetting, path);
            //    }
            //}

            //return false;
        }

        public static bool PathContains(string longPaths, string shortPath)
        {
            shortPath = shortPath.ToLowerInvariant();
            int a = shortPath.LastIndexOf("/");
            if (a > 0)
            {
                shortPath = shortPath.Substring(0, a);
            }

            a = shortPath.LastIndexOf("?");
            if (a > 0)
            {
                shortPath = shortPath.Substring(0, a);
            }

            string[] array = longPaths.ToLowerInvariant().Split(',');
            return array.Contains(shortPath);
        }

#endregion end of SSO Controller
    }
}