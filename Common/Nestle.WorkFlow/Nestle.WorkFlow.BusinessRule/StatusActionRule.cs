using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class StatusActionRule : System.Web.UI.Page
    {
        public static int InsertStatusAction(StatusAction model)
        {
            return StatusActionDao.InsertStatusAction(model);
        }

        public static int UpdateStatusAction(StatusAction model)
        {
            return StatusActionDao.UpdateStatusAction(model);
        }

        public static List<StatusAction> GetStatusAction(string procedureName, SqlParameter[] sqlParamters)
        {
            return StatusActionDao.GetStatusAction(procedureName, sqlParamters);
        }


        public static QueryResult<StatusAction> GetStatusActionResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return StatusActionDao.GetStatusActionResult(procedureName, sqlParamters);
        }
    }
}
