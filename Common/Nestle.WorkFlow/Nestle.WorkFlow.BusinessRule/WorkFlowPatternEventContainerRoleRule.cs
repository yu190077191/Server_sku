using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class WorkFlowPatternEventContainerRoleRule : System.Web.UI.Page
    {
        public static int InsertWorkFlowPatternEventContainerRole(WorkFlowPatternEventContainerRole model)
        {
            return WorkFlowPatternEventContainerRoleDao.InsertWorkFlowPatternEventContainerRole(model);
        }

        public static int UpdateWorkFlowPatternEventContainerRole(WorkFlowPatternEventContainerRole model)
        {
            return WorkFlowPatternEventContainerRoleDao.UpdateWorkFlowPatternEventContainerRole(model);
        }

        public static List<WorkFlowPatternEventContainerRole> GetWorkFlowPatternEventContainerRole(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowPatternEventContainerRoleDao.GetWorkFlowPatternEventContainerRole(procedureName, sqlParamters);
        }


        public static QueryResult<WorkFlowPatternEventContainerRole> GetWorkFlowPatternEventContainerRoleResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowPatternEventContainerRoleDao.GetWorkFlowPatternEventContainerRoleResult(procedureName, sqlParamters);
        }
    }
}
