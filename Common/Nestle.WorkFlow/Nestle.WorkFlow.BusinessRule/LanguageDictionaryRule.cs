using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class LanguageDictionaryRule : System.Web.UI.Page
    {
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
