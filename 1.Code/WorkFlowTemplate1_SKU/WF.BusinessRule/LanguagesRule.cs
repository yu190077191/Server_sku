using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using WF.DataAccess;
using WF.Framework;
using WF.Model;

namespace WF.BusinessRule
{
    public class LanguagesRule
    {
        public static List<Languages> GetLanguages()
        {
            return LanguagesDao.GetLanguages();
        }

        public static bool IsInEnglish()
        {
            return GetUserLanguage().ToUpper() == "ENGLISH";
        }

        public static bool IsInChinese()
        {
            return GetUserLanguage().ToUpper() == "CHINESE";
        }

        public static string GetUserLanguage()
        {
            if (System.Web.HttpContext.Current.Session[Constants.SelectedLanguage] != null)
            {
                return System.Web.HttpContext.Current.Session[Constants.SelectedLanguage].ToString();
            }

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name))
            {
                List<Languages> li = LanguagesDao.GetLanguages(System.Web.HttpContext.Current.User.Identity.Name);
                if (li != null && li.Count > 0)
                {
                    return li[0].EnglishName;
                }
            }

            return System.Configuration.ConfigurationManager.AppSettings["Language"];
        }

        public static void SetUserLanguage(string languageEnglishName)
        {
            System.Web.HttpContext.Current.Session[Constants.SelectedLanguage] = languageEnglishName;

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name))
            {
                LanguagesDao.SetUserLanguage(System.Web.HttpContext.Current.User.Identity.Name, languageEnglishName);
            }
        }

        public static string GetLanguageSelection()
        {
            List<Languages> li = LanguagesDao.GetLanguages();
            string myLanguage = GetUserLanguage();
            string alternativeLanguage = string.Empty;
            switch (myLanguage)
            {
                case "Chinese":
                    alternativeLanguage = "English";
                    break;
                case "English":
                    alternativeLanguage = "Chinese";
                    break;
            }

            return "<span onclick=\"setLanguage('" + alternativeLanguage + "')\" style=\"cursor:pointer;font-size:14px;color:black;\">" + li.Where(k => k.EnglishName == alternativeLanguage).FirstOrDefault().Language + "</span>";
        }

        public static string Translate(string languageKey, int? languageId, string languageEnglishName)
        {
            return LanguagesDao.Translate(languageKey, languageId, languageEnglishName);
        }

        public static string Translate(string languageKey)
        {
            return Translate(languageKey, null, GetUserLanguage());
        }

        public static string GetLanguageKey(string languageValue)
        {
            return LanguagesDao.GetLanguageKey(languageValue);
        }

        public static List<LanguageKeyValue> TranslateArray(string LanguageKeyArray, int? languageId, string languageEnglishName)
        {
            return LanguagesDao.TranslateArray(LanguageKeyArray, languageId, languageEnglishName);
        }

        public static List<LanguageKeyValue> TranslateArray(string LanguageKeyArray)
        {
            return LanguagesDao.TranslateArray(LanguageKeyArray, null, GetUserLanguage());
        }

        public static string Translate(string languageKey, List<LanguageKeyValue> languageKeyValueList)
        {
            var result = languageKeyValueList.Where(k => k.Key == languageKey).FirstOrDefault();
            if (result != null)
            {
                return result.Value;
            }

            return languageKey;
        }

        public static int InsertLanguages(Languages model)
        {
            return LanguagesDao.InsertLanguages(model);
        }


        public static int UpdateLanguages(Languages model)
        {
            return LanguagesDao.UpdateLanguages(model);
        }


        public static List<Languages> GetLanguages(string procedureName, SqlParameter[] sqlParamters)
        {
            return LanguagesDao.GetLanguages(procedureName, sqlParamters);
        }

    }
}
