using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WF.DataAccess;
using WF.Framework;
using WF.Model;

namespace WF.BusinessRule
{
    public class LanguageDictionaryRule : System.Web.UI.Page
    {
        public static int InsertOrUpdateDictionary(Translation model)
        {
            return LanguageDictionaryDao.InsertOrUpdateDictionary(model);
        }

        public static QueryResult<Translation> GetWordList(SqlParameter[] sqlParamters)
        {
            return LanguageDictionaryDao.GetWordList(sqlParamters);
        }

        public static int InsertLanguageDictionary(LanguageDictionary model)
        {
            return LanguageDictionaryDao.InsertLanguageDictionary(model);
        }

        public static int UpdateLanguageDictionary(LanguageDictionary model)
        {
            return LanguageDictionaryDao.UpdateLanguageDictionary(model);
        }

        public static List<LanguageDictionary> GetLanguageDictionary(string procedureName, SqlParameter[] sqlParamters)
        {
            return LanguageDictionaryDao.GetLanguageDictionary(procedureName, sqlParamters);
        }


        public static QueryResult<LanguageDictionary> GetLanguageDictionaryResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return LanguageDictionaryDao.GetLanguageDictionaryResult(procedureName, sqlParamters);
        }
    }
}
