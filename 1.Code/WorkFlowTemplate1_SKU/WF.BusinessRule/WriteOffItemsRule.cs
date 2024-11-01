using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WF.DataAccess;
using WF.Framework;
using WF.Model;

namespace WF.BusinessRule
{
    public class WriteOffItemsRule : System.Web.UI.Page
    {
        public static int InsertWriteOffItems(WriteOffItems model)
        {
            return WriteOffItemsDao.InsertWriteOffItems(model);
        }

        public static int UpdateWriteOffItems(WriteOffItems model)
        {
            return WriteOffItemsDao.UpdateWriteOffItems(model);
        }

        public static List<WriteOffItems> GetWriteOffItems(string procedureName, SqlParameter[] sqlParamters)
        {
            return WriteOffItemsDao.GetWriteOffItems(procedureName, sqlParamters);
        }


        public static QueryResult<WriteOffItems> GetWriteOffItemsResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return WriteOffItemsDao.GetWriteOffItemsResult(procedureName, sqlParamters);
        }
    }
}
