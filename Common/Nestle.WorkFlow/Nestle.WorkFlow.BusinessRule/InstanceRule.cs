using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class InstanceRule : System.Web.UI.Page
    {
        public static int InsertInstance(Instance model)
        {
            return InstanceDao.InsertInstance(model);
        }

        public static int UpdateInstance(Instance model)
        {
            return InstanceDao.UpdateInstance(model);
        }

        public static List<Instance> GetInstance(string procedureName, SqlParameter[] sqlParamters)
        {
            return InstanceDao.GetInstance(procedureName, sqlParamters);
        }


        public static QueryResult<Instance> GetInstanceResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return InstanceDao.GetInstanceResult(procedureName, sqlParamters);
        }
    }
}
