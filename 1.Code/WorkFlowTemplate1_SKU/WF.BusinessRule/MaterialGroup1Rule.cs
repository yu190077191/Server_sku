using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WF.DataAccess;
using WF.Framework;
using WF.Model;

namespace WF.BusinessRule
{
    public class MaterialGroup1Rule : System.Web.UI.Page
    {
        public static int InsertMaterialGroup1(MaterialGroup1 model)
        {
            return MaterialGroup1Dao.InsertMaterialGroup1(model);
        }

        public static int UpdateMaterialGroup1(MaterialGroup1 model)
        {
            return MaterialGroup1Dao.UpdateMaterialGroup1(model);
        }

        public static List<MaterialGroup1> GetMaterialGroup1(string procedureName, SqlParameter[] sqlParamters)
        {
            return MaterialGroup1Dao.GetMaterialGroup1(procedureName, sqlParamters);
        }


        public static QueryResult<MaterialGroup1> GetMaterialGroup1Result(string procedureName, SqlParameter[] sqlParamters)
        {
            return MaterialGroup1Dao.GetMaterialGroup1Result(procedureName, sqlParamters);
        }
    }
}
