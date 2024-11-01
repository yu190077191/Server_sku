using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class DepartmentRoleRule : System.Web.UI.Page
    {
        public static int AddDepartmentRole(Nestle.WorkFlow.Model.DepartmentRole model, string siteName, Guid siteGuid)
        {
            return DepartmentRoleDao.AddDepartmentRole(model, siteName, siteGuid);
        }

        public static int DeleteDepartmentRole(int id, string siteName, Guid siteGuid)
        {
            return DepartmentRoleDao.DeleteDepartmentRole(id, siteName, siteGuid);
        }

        public static int InsertDepartmentRole(DepartmentRole model)
        {
            return DepartmentRoleDao.InsertDepartmentRole(model);
        }

        public static int UpdateDepartmentRole(DepartmentRole model)
        {
            return DepartmentRoleDao.UpdateDepartmentRole(model);
        }

        public static List<DepartmentRole> GetDepartmentRole(string procedureName, SqlParameter[] sqlParamters)
        {
            return DepartmentRoleDao.GetDepartmentRole(procedureName, sqlParamters);
        }

        public static QueryResult<DepartmentRole> GetDepartmentRoleResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return DepartmentRoleDao.GetDepartmentRoleResult(procedureName, sqlParamters);
        }
    }
}
