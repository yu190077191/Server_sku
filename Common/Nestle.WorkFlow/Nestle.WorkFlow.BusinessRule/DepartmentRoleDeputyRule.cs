using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class DepartmentRoleDeputyRule : System.Web.UI.Page
    {
        public static int InsertDepartmentRoleDeputy(DepartmentRoleDeputy model)
        {
            return DepartmentRoleDeputyDao.InsertDepartmentRoleDeputy(model);
        }

        public static int UpdateDepartmentRoleDeputy(DepartmentRoleDeputy model)
        {
            return DepartmentRoleDeputyDao.UpdateDepartmentRoleDeputy(model);
        }

        public static List<DepartmentRoleDeputy> GetDepartmentRoleDeputy(string procedureName, SqlParameter[] sqlParamters)
        {
            return DepartmentRoleDeputyDao.GetDepartmentRoleDeputy(procedureName, sqlParamters);
        }

        public static QueryResult<DepartmentRoleDeputy> GetDepartmentRoleDeputyResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return DepartmentRoleDeputyDao.GetDepartmentRoleDeputyResult(procedureName, sqlParamters);
        }

        public static List<DepartmentRoleDeputy> GetDepartmentRoleDeputy(string siteName, Guid siteGuid)
        {
            return DepartmentRoleDeputyDao.GetDepartmentRoleDeputy("GetMyDeputyRoles", new[]{
                new SqlParameter("@SiteName", siteName),
                new SqlParameter("@SiteGuid", siteGuid),
                new SqlParameter("@CreatedBy", Operation.OperationBy)
            });
        }
    }
}