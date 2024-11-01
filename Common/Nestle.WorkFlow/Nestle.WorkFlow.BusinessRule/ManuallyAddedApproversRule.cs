using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class ManuallyAddedApproversRule : System.Web.UI.Page
    {
        public static int InsertManuallyAddedApprovers(ManuallyAddedApprovers model)
        {
            return ManuallyAddedApproversDao.InsertManuallyAddedApprovers(model);
        }

        public static int UpdateManuallyAddedApprovers(ManuallyAddedApprovers model)
        {
            return ManuallyAddedApproversDao.UpdateManuallyAddedApprovers(model);
        }

        public static List<ManuallyAddedApprovers> GetManuallyAddedApprovers(string procedureName, SqlParameter[] sqlParamters)
        {
            return ManuallyAddedApproversDao.GetManuallyAddedApprovers(procedureName, sqlParamters);
        }


        public static QueryResult<ManuallyAddedApprovers> GetManuallyAddedApproversResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return ManuallyAddedApproversDao.GetManuallyAddedApproversResult(procedureName, sqlParamters);
        }
    }
}
