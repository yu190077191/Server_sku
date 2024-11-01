using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class WorkFlowStatusRule : System.Web.UI.Page
    {
        public static int InsertWorkFlowStatus(WorkFlowStatus model)
        {
            return WorkFlowStatusDao.InsertWorkFlowStatus(model);
        }

        public static int UpdateWorkFlowStatus(WorkFlowStatus model)
        {
            return WorkFlowStatusDao.UpdateWorkFlowStatus(model);
        }

        public static List<WorkFlowStatus> GetWorkFlowStatus(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowStatusDao.GetWorkFlowStatus(procedureName, sqlParamters);
        }


        public static QueryResult<WorkFlowStatus> GetWorkFlowStatusResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowStatusDao.GetWorkFlowStatusResult(procedureName, sqlParamters);
        }
    }
}
