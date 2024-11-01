using System;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

namespace WF.Framework
{
    public sealed class Constants
    {
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["ProjectManagementDatabase2"].ConnectionString;

        public static readonly string WorkFlowConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Nestle.WorkFlow.Database2"].ConnectionString;

        public static readonly string SiteName = System.Configuration.ConfigurationManager.AppSettings["SiteName"];

        public static readonly string SiteGuid = System.Configuration.ConfigurationManager.AppSettings["SiteGuid"];

        public static readonly string AdminEmail = System.Configuration.ConfigurationManager.AppSettings["AdminEmail"];

        public static readonly int PageSize = 10;

        public static readonly System.Globalization.CultureInfo CurrentCulture = new System.Globalization.CultureInfo("zh-CHS");

        public static readonly string EmployeeSessionName = "Nestle.WorkFlow.Employee";

        public static readonly string UploadFolder = ConfigurationManager.AppSettings["uploads"];

        public static readonly string DateString = DateTime.Now.ToString("yyyy-MM-dd");

        public static readonly string UploadFileType = ConfigurationManager.AppSettings["UploadFileType"];

        public static readonly string UploadResultHtml = "UploadResultHtml";

        public static readonly string DelegatedBy = "DelegatedBy";

        public static readonly string SelectedLanguage = "SelectedLanguage";

        public static string SiteType
        {
            get
            {
                string url = System.Web.HttpContext.Current.Request.ApplicationPath.ToUpper();
                return url;
            }
        }

        public static bool IsDigital
        {
            get
            {
                return System.Web.HttpContext.Current.Request.ApplicationPath.ToUpper().IndexOf("/digital".ToUpper()) == 0;
            }
        }
    }
}