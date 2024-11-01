using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class WorkFlowRoleRule : System.Web.UI.Page
    {
        public static int InsertWorkFlowRole(WorkFlowRole model)
        {
            return WorkFlowRoleDao.InsertWorkFlowRole(model);
        }

        public static int UpdateWorkFlowRole(WorkFlowRole model)
        {
            return WorkFlowRoleDao.UpdateWorkFlowRole(model);
        }

        public static List<WorkFlowRole> GetWorkFlowRole(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowRoleDao.GetWorkFlowRole(procedureName, sqlParamters);
        }


        public static QueryResult<WorkFlowRole> GetWorkFlowRoleResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowRoleDao.GetWorkFlowRoleResult(procedureName, sqlParamters);
        }
    }
}
