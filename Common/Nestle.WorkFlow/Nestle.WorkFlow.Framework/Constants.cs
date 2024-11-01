using System;
using System.Configuration;
using System.Globalization;
using System.Web;

namespace Nestle.WorkFlow.Framework
{
    public sealed class Constants
    {
        private Constants()
        {
        }

        public static readonly string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Nestle.WorkFlow.Database2"].ConnectionString;

        public static readonly string SiteName = System.Configuration.ConfigurationManager.AppSettings["SiteName"];

        public static readonly string SiteGuid = System.Configuration.ConfigurationManager.AppSettings["SiteGuid"];

        public static readonly int PageSize = 10;

        public static readonly System.Globalization.CultureInfo CurrentCulture = new System.Globalization.CultureInfo("zh-CHS");

        public static readonly string EmployeeSessionName = "Nestle.WorkFlow.Employee";

        public static readonly string UploadFolder = ConfigurationManager.AppSettings["uploads"];

        public static readonly string DateString = DateTime.Now.ToShortDateString().Replace("/", "-");

        public static readonly string UploadFileType = ConfigurationManager.AppSettings["UploadFileType"];

        public static readonly string UploadResultHtml = "UploadResultHtml";
    }
}