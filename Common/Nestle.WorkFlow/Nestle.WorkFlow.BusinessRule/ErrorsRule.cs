using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class ErrorsRule : System.Web.UI.Page
    {
        public static int InsertErrors(Errors model)
        {
            return ErrorsDao.InsertErrors(model);
        }

        public static int UpdateErrors(Errors model)
        {
            return ErrorsDao.UpdateErrors(model);
        }

        public static List<Errors> GetErrors(string procedureName, SqlParameter[] sqlParamters)
        {
            return ErrorsDao.GetErrors(procedureName, sqlParamters);
        }


        public static QueryResult<Errors> GetErrorsResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return ErrorsDao.GetErrorsResult(procedureName, sqlParamters);
        }
    }
}
