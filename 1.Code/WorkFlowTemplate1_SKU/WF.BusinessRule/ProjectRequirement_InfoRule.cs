using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WF.DataAccess;
using WF.Framework;
using WF.Model;

namespace WF.BusinessRule
{
    public class ProjectRequirement_InfoRule : System.Web.UI.Page
    {
        public static int InsertProjectRequirement_Info(ProjectRequirement_Info model)
        {
            return ProjectRequirement_InfoDao.InsertProjectRequirement_Info(model);
        }

        public static int UpdateProjectRequirement_Info(ProjectRequirement_Info model)
        {
            return ProjectRequirement_InfoDao.UpdateProjectRequirement_Info(model);
        }

        public static List<ProjectRequirement_Info> GetProjectRequirement_Info(string procedureName, SqlParameter[] sqlParamters)
        {
            return ProjectRequirement_InfoDao.GetProjectRequirement_Info(procedureName, sqlParamters);
        }


        public static QueryResult<ProjectRequirement_Info> GetProjectRequirement_InfoResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return ProjectRequirement_InfoDao.GetProjectRequirement_InfoResult(procedureName, sqlParamters);
        }
    }
}
