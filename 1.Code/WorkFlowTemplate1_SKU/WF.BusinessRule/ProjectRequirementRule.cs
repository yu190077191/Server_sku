using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WF.DataAccess;
using WF.Framework;
using WF.Model;

namespace WF.BusinessRule
{
    public class ProjectRequirementRule : System.Web.UI.Page
    {
        public static int ChangeE2E(int id, int e2e)
        {
            return ProjectRequirementDao.ChangeE2E(id, e2e);
        }

        public static int CreateId(Guid Temp_ImportedData_Id)
        {
            return ProjectRequirementDao.CreateId(Temp_ImportedData_Id);
        }
        public static int UpdateStatus(int requestVersionId, int status)
        {
            return ProjectRequirementDao.UpdateStatus(requestVersionId, status);
        }

        public static int InsertProjectRequirement(ProjectRequirement model)
        {
            return ProjectRequirementDao.InsertProjectRequirement(model);
        }

        public static int UpdateProjectRequirement(ProjectRequirement model)
        {
            return ProjectRequirementDao.UpdateProjectRequirement(model);
        }

        public static List<ProjectRequirement> GetProjectRequirement(string procedureName, SqlParameter[] sqlParamters)
        {
            return ProjectRequirementDao.GetProjectRequirement(procedureName, sqlParamters);
        }


        public static QueryResult<ProjectRequirement> GetProjectRequirementResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return ProjectRequirementDao.GetProjectRequirementResult(procedureName, sqlParamters);
        }
    }
}
