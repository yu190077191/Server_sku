using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class ActionRule : System.Web.UI.Page
    {
        public static int InsertAction(Nestle.WorkFlow.Model.Action model)
        {
            return ActionDao.InsertAction(model);
        }

        public static int UpdateAction(Nestle.WorkFlow.Model.Action model)
        {
            return ActionDao.UpdateAction(model);
        }

        public static List<Nestle.WorkFlow.Model.Action> GetAction(string procedureName, SqlParameter[] sqlParamters)
        {
            return ActionDao.GetAction(procedureName, sqlParamters);
        }


        public static QueryResult<Nestle.WorkFlow.Model.Action> GetActionResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return ActionDao.GetActionResult(procedureName, sqlParamters);
        }

        public static List<Button> GetButtons(int requestId, int requestVersionId, string siteName, Guid siteGuid, string typeCode)
        {
            return CommonRule.Get<Button>("GetButtons", new SqlParameter[] {
                new SqlParameter("@RequestId", requestId),
                new SqlParameter("@requestVersionId", requestVersionId),
                new SqlParameter("@typeCode", typeCode),
                new SqlParameter("@SiteName", siteName),
                new SqlParameter("@SiteGuid", siteGuid),
                new SqlParameter("@OperationBy", Operation.OperationBy)
            });
        }
    }
}