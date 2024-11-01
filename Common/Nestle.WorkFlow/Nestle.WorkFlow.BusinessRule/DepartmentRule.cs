using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class DepartmentRule : System.Web.UI.Page
    {
        public static int InsertDepartment(Department model)
        {
            return DepartmentDao.InsertDepartment(model);
        }

        public static int UpdateDepartment(Department model)
        {
            return DepartmentDao.UpdateDepartment(model);
        }

        public static List<Department> GetDepartment(string procedureName, SqlParameter[] sqlParamters)
        {
            return DepartmentDao.GetDepartment(procedureName, sqlParamters);
        }


        public static QueryResult<Department> GetDepartmentResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return DepartmentDao.GetDepartmentResult(procedureName, sqlParamters);
        }
    }
}
