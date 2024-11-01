using System.Globalization;
using WF.BusinessRule;
using WF.Framework;

namespace System.Web.Mvc
{
    public static class GlobalLocalizationHelper
    {
        /// <summary>
        /// using in html ,output string to html
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Lang(this HtmlHelper htmlHelper, string key)
        {
            return GetLangString(htmlHelper.ViewContext.HttpContext, key);
        }

        /// <summary>
        /// using in html ,output string to javascript
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string LangOutJSVar(this HtmlHelper htmlHelper, string key)
        {
            string langstr = GetLangString(htmlHelper.ViewContext.HttpContext, key);
            return string.Format(Constants.CurrentCulture, "var {0} = '{1}'", key, langstr);
        }

        /// <summary>
        /// using in C# coding.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string InnerLang(HttpContextBase httpContext, string key)
        {
            return GetLangString(httpContext, key);
        }

        private static string GetLangString(HttpContextBase httpContext, string key)
        {
            return key;
            //return LanguagesRule.Translate(key);
        }
    }
}