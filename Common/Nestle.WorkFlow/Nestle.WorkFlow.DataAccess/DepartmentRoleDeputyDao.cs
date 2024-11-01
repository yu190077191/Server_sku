using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class DepartmentRoleDeputyDao : BaseDao
    {
        public static int InsertDepartmentRoleDeputy(Nestle.WorkFlow.Model.DepartmentRoleDeputy model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@DepartmentRoleId", model.DepartmentRoleId));
            sqlParameters.Add(new SqlParameter("@StartTime", model.StartTime));
            sqlParameters.Add(new SqlParameter("@EndTime", model.EndTime));
            sqlParameters.Add(new SqlParameter("@EmployeeId", model.EmployeeId));
            sqlParameters.Add(new SqlParameter("@Justification", model.Justification));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertDepartmentRoleDeputyProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateDepartmentRoleDeputy(Nestle.WorkFlow.Model.DepartmentRoleDeputy model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@DepartmentRoleId", model.DepartmentRoleId));
            sqlParameters.Add(new SqlParameter("@StartTime", model.StartTime));
            sqlParameters.Add(new SqlParameter("@EndTime", model.EndTime));
            sqlParameters.Add(new SqlParameter("@EmployeeId", model.EmployeeId));
            sqlParameters.Add(new SqlParameter("@Justification", model.Justification));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateDepartmentRoleDeputyProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.DepartmentRoleDeputy> GetDepartmentRoleDeputy(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.DepartmentRoleDeputy> li = new List<Nestle.WorkFlow.Model.DepartmentRoleDeputy>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//DepartmentRoleDeputy");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.DepartmentRoleDeputy>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.DepartmentRoleDeputy> GetDepartmentRoleDeputyResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.DepartmentRoleDeputy> result = new QueryResult<Nestle.WorkFlow.Model.DepartmentRoleDeputy>();
            List<Nestle.WorkFlow.Model.DepartmentRoleDeputy> li = new List<Nestle.WorkFlow.Model.DepartmentRoleDeputy>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//DepartmentRoleDeputy");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.DepartmentRoleDeputy>(node.OuterXml));
                    }

                    result.DataList = li;
                    XmlNode xmlNode = xmlDocument.FirstChild.SelectSingleNode("//TotalCount");
                    if (xmlNode != null)
                    {
                        result.Count = Convert.ToInt32(xmlNode.InnerText, Constants.CurrentCulture);
                    }
                }
            }

            return result;
        }

    }
}
