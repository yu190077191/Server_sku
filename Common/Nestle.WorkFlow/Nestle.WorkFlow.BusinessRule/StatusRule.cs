using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class StatusRule : System.Web.UI.Page
    {
        public static int InsertStatus(Status model)
        {
            return StatusDao.InsertStatus(model);
        }

        public static int UpdateStatus(Status model)
        {
            return StatusDao.UpdateStatus(model);
        }

        public static List<Status> GetStatus(string procedureName, SqlParameter[] sqlParamters)
        {
            return StatusDao.GetStatus(procedureName, sqlParamters);
        }


        public static QueryResult<Status> GetStatusResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return StatusDao.GetStatusResult(procedureName, sqlParamters);
        }
    }
}
