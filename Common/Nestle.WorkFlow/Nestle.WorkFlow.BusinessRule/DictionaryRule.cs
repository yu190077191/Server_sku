using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class DictionaryRule : System.Web.UI.Page
    {
        public static int InsertDictionary(Dictionary model)
        {
            return DictionaryDao.InsertDictionary(model);
        }

        public static int UpdateDictionary(Dictionary model)
        {
            return DictionaryDao.UpdateDictionary(model);
        }

        public static List<Dictionary> GetDictionary(string procedureName, SqlParameter[] sqlParamters)
        {
            return DictionaryDao.GetDictionary(procedureName, sqlParamters);
        }


        public static QueryResult<Dictionary> GetDictionaryResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return DictionaryDao.GetDictionaryResult(procedureName, sqlParamters);
        }
    }
}
