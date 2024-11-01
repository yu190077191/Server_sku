using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class RecordStatusRule : System.Web.UI.Page
    {
        public static int InsertRecordStatus(RecordStatus model)
        {
            return RecordStatusDao.InsertRecordStatus(model);
        }

        public static int UpdateRecordStatus(RecordStatus model)
        {
            return RecordStatusDao.UpdateRecordStatus(model);
        }

        public static List<RecordStatus> GetRecordStatus(string procedureName, SqlParameter[] sqlParamters)
        {
            return RecordStatusDao.GetRecordStatus(procedureName, sqlParamters);
        }


        public static QueryResult<RecordStatus> GetRecordStatusResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return RecordStatusDao.GetRecordStatusResult(procedureName, sqlParamters);
        }
    }
}
