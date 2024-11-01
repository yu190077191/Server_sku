using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class SystemRoleRule : System.Web.UI.Page
    {
        public static int InsertSystemRole(SystemRole model)
        {
            return SystemRoleDao.InsertSystemRole(model);
        }

        public static int UpdateSystemRole(SystemRole model)
        {
            return SystemRoleDao.UpdateSystemRole(model);
        }

        public static List<SystemRole> GetSystemRole(string procedureName, SqlParameter[] sqlParamters)
        {
            return SystemRoleDao.GetSystemRole(procedureName, sqlParamters);
        }


        public static QueryResult<SystemRole> GetSystemRoleResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return SystemRoleDao.GetSystemRoleResult(procedureName, sqlParamters);
        }
    }
}
