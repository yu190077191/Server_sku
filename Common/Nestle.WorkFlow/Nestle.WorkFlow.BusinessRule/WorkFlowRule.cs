using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class WorkFlowRule : System.Web.UI.Page
    {
        public static int InsertWorkFlow(Nestle.WorkFlow.Model.WorkFlow model)
        {
            return WorkFlowDao.InsertWorkFlow(model);
        }

        public static int UpdateWorkFlow(Nestle.WorkFlow.Model.WorkFlow model)
        {
            return WorkFlowDao.UpdateWorkFlow(model);
        }

        public static List<Nestle.WorkFlow.Model.WorkFlow> GetWorkFlow(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowDao.GetWorkFlow(procedureName, sqlParamters);
        }

        public static QueryResult<Nestle.WorkFlow.Model.WorkFlow> GetWorkFlowResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowDao.GetWorkFlowResult(procedureName, sqlParamters);
        }
    }
}
