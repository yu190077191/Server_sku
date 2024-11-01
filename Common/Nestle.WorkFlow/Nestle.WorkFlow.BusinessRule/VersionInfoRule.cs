using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class VersionInfoRule : System.Web.UI.Page
    {
        public static int InsertVersionInfo(VersionInfo model)
        {
            return VersionInfoDao.InsertVersionInfo(model);
        }

        public static int UpdateVersionInfo(VersionInfo model)
        {
            return VersionInfoDao.UpdateVersionInfo(model);
        }

        public static List<VersionInfo> GetVersionInfo(string procedureName, SqlParameter[] sqlParamters)
        {
            return VersionInfoDao.GetVersionInfo(procedureName, sqlParamters);
        }


        public static QueryResult<VersionInfo> GetVersionInfoResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return VersionInfoDao.GetVersionInfoResult(procedureName, sqlParamters);
        }
    }
}
