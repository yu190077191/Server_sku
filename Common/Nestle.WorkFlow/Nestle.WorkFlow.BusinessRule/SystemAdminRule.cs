using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class SystemAdminRule : System.Web.UI.Page
    {
        public static int InsertSystemAdmin(SystemAdmin model)
        {
            return SystemAdminDao.InsertSystemAdmin(model);
        }

        public static int UpdateSystemAdmin(SystemAdmin model)
        {
            return SystemAdminDao.UpdateSystemAdmin(model);
        }

        public static List<SystemAdmin> GetSystemAdmin(string procedureName, SqlParameter[] sqlParamters)
        {
            return SystemAdminDao.GetSystemAdmin(procedureName, sqlParamters);
        }


        public static QueryResult<SystemAdmin> GetSystemAdminResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return SystemAdminDao.GetSystemAdminResult(procedureName, sqlParamters);
        }
    }
}
