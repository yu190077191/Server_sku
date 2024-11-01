using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WF.DataAccess;
using WF.Framework;
using WF.Model;

namespace WF.BusinessRule
{
    public class FieldRule : System.Web.UI.Page
    {
        public static int InsertField(Field model)
        {
            return FieldDao.InsertField(model);
        }

        public static int UpdateField(Field model)
        {
            return FieldDao.UpdateField(model);
        }

        public static List<Field> GetField(string procedureName, SqlParameter[] sqlParamters)
        {
            return FieldDao.GetField(procedureName, sqlParamters);
        }


        public static QueryResult<Field> GetFieldResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return FieldDao.GetFieldResult(procedureName, sqlParamters);
        }
    }
}
