using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class WorkFlowApproverRule : System.Web.UI.Page
    {
        public static int InsertWorkFlowApprover(WorkFlowApprover model)
        {
            return WorkFlowApproverDao.InsertWorkFlowApprover(model);
        }

        public static int UpdateWorkFlowApprover(WorkFlowApprover model)
        {
            return WorkFlowApproverDao.UpdateWorkFlowApprover(model);
        }

        public static List<WorkFlowApprover> GetWorkFlowApprover(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowApproverDao.GetWorkFlowApprover(procedureName, sqlParamters);
        }


        public static QueryResult<WorkFlowApprover> GetWorkFlowApproverResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowApproverDao.GetWorkFlowApproverResult(procedureName, sqlParamters);
        }
    }
}
