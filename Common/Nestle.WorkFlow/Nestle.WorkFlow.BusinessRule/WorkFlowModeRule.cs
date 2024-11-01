using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class WorkFlowModeRule : System.Web.UI.Page
    {
        public static int InsertWorkFlowMode(WorkFlowMode model)
        {
            return WorkFlowModeDao.InsertWorkFlowMode(model);
        }

        public static int UpdateWorkFlowMode(WorkFlowMode model)
        {
            return WorkFlowModeDao.UpdateWorkFlowMode(model);
        }

        public static List<WorkFlowMode> GetWorkFlowMode(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowModeDao.GetWorkFlowMode(procedureName, sqlParamters);
        }


        public static QueryResult<WorkFlowMode> GetWorkFlowModeResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowModeDao.GetWorkFlowModeResult(procedureName, sqlParamters);
        }
    }
}
