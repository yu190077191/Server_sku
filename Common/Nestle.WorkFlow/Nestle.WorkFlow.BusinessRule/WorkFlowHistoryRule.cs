using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class WorkFlowHistoryRule : System.Web.UI.Page
    {
        public static int InsertWorkFlowHistory(WorkFlowHistory model)
        {
            return WorkFlowHistoryDao.InsertWorkFlowHistory(model);
        }

        public static int UpdateWorkFlowHistory(WorkFlowHistory model)
        {
            return WorkFlowHistoryDao.UpdateWorkFlowHistory(model);
        }

        public static List<WorkFlowHistory> GetWorkFlowHistory(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowHistoryDao.GetWorkFlowHistory(procedureName, sqlParamters);
        }


        public static QueryResult<WorkFlowHistory> GetWorkFlowHistoryResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowHistoryDao.GetWorkFlowHistoryResult(procedureName, sqlParamters);
        }
    }
}
