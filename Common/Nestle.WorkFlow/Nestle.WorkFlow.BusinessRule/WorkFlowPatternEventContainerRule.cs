using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class WorkFlowPatternEventContainerRule : System.Web.UI.Page
    {
        public static int InsertWorkFlowPatternEventContainer(WorkFlowPatternEventContainer model)
        {
            return WorkFlowPatternEventContainerDao.InsertWorkFlowPatternEventContainer(model);
        }

        public static int UpdateWorkFlowPatternEventContainer(WorkFlowPatternEventContainer model)
        {
            return WorkFlowPatternEventContainerDao.UpdateWorkFlowPatternEventContainer(model);
        }

        public static List<WorkFlowPatternEventContainer> GetWorkFlowPatternEventContainer(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowPatternEventContainerDao.GetWorkFlowPatternEventContainer(procedureName, sqlParamters);
        }


        public static QueryResult<WorkFlowPatternEventContainer> GetWorkFlowPatternEventContainerResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return WorkFlowPatternEventContainerDao.GetWorkFlowPatternEventContainerResult(procedureName, sqlParamters);
        }
    }
}
