using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WF.DataAccess;
using WF.Framework;
using WF.Model;

namespace WF.BusinessRule
{
    public class RequestRule : System.Web.UI.Page
    {
        public static int InsertRequest(Request model)
        {
            return RequestDao.InsertRequest(model);
        }

        public static int UpdateRequest(Request model)
        {
            return RequestDao.UpdateRequest(model);
        }

        public static List<Request> GetRequest(string procedureName, SqlParameter[] sqlParamters)
        {
            return RequestDao.GetRequest(procedureName, sqlParamters);
        }


        public static QueryResult<Request> GetRequestResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return RequestDao.GetRequestResult(procedureName, sqlParamters);
        }
    }
}
