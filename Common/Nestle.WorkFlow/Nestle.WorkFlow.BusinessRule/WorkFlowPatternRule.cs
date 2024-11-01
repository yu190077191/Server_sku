using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class WorkFlowPatternRule : System.Web.UI.Page
    {
        public static int InsertWorkFlowPattern(WorkFlowPatternJson model)
        {
            return WorkFlowPatternDao.InsertWorkFlowPattern(model);
        }

        public static int InsertWorkFlowPattern(WorkFlowPattern model)
        {
            return WorkFlowPatternDao.InsertWorkFlowPattern(model);
        }

        public static int UpdateWorkFlowPattern(WorkFlowPattern model)
        {
            return WorkFlowPatternDao.UpdateWorkFlowPattern(model);
        }

        public static List<WorkFlowPattern> GetWorkFlowPattern(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowPatternDao.GetWorkFlowPattern(procedureName, sqlParamters);
        }


        public static QueryResult<WorkFlowPattern> GetWorkFlowPatternResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowPatternDao.GetWorkFlowPatternResult(procedureName, sqlParamters);
        }

        public static QueryResult<Pattern> GetPatternResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowPatternDao.GetPatternResult(procedureName, sqlParamters);
        }
    }
}
