using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class RoleRule : System.Web.UI.Page
    {
        public static int InsertRole(Role model)
        {
            return RoleDao.InsertRole(model);
        }

        public static int UpdateRole(Role model)
        {
            return RoleDao.UpdateRole(model);
        }

        public static List<Role> GetRole(string procedureName, SqlParameter[] sqlParamters)
        {
            return RoleDao.GetRole(procedureName, sqlParamters);
        }


        public static QueryResult<Role> GetRoleResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return RoleDao.GetRoleResult(procedureName, sqlParamters);
        }
    }
}