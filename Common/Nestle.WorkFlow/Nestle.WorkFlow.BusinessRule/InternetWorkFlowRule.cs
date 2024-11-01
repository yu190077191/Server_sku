using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class InternetWorkFlowRule : System.Web.UI.Page
    {
        public static int InsertInternetWorkFlow(InternetWorkFlow model)
        {
            return InternetWorkFlowDao.InsertInternetWorkFlow(model);
        }

        public static int UpdateInternetWorkFlow(InternetWorkFlow model)
        {
            return InternetWorkFlowDao.UpdateInternetWorkFlow(model);
        }

        public static List<InternetWorkFlow> GetInternetWorkFlow(string procedureName, SqlParameter[] sqlParamters)
        {
            return InternetWorkFlowDao.GetInternetWorkFlow(procedureName, sqlParamters);
        }


        public static QueryResult<InternetWorkFlow> GetInternetWorkFlowResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return InternetWorkFlowDao.GetInternetWorkFlowResult(procedureName, sqlParamters);
        }
    }
}
