using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WF.DataAccess;
using WF.Framework;
using WF.Model;

namespace WF.BusinessRule
{
    public class RequestVersionRule : System.Web.UI.Page
    {
        public static int InsertRequestVersion(RequestVersion model)
        {
            return RequestVersionDao.InsertRequestVersion(model);
        }

        public static int UpdateRequestVersion(RequestVersion model)
        {
            return RequestVersionDao.UpdateRequestVersion(model);
        }

        public static int UpdateField(UpdateField model)
        {
            return RequestVersionDao.UpdateField(model);
        }

        public static List<RequestVersion> GetRequestVersion(string procedureName, SqlParameter[] sqlParamters)
        {
            return RequestVersionDao.GetRequestVersion(procedureName, sqlParamters);
        }


        public static QueryResult<RequestVersion> GetRequestVersionResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return RequestVersionDao.GetRequestVersionResult(procedureName, sqlParamters);
        }

        public static int UpdateStatus(int requestVersionId, int status)
        {
            return RequestVersionDao.UpdateStatus(requestVersionId, status);
        }
    }
}
