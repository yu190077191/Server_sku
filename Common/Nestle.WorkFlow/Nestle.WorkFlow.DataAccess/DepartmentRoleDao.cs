using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class DepartmentRoleDao : BaseDao
    {
        public static int AddDepartmentRole(Nestle.WorkFlow.Model.DepartmentRole model, string siteName, Guid siteGuid)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@EmployeeId", model.EmployeeId));
            sqlParameters.Add(new SqlParameter("@BUDepartmentId", model.BUDepartmentId));
            sqlParameters.Add(new SqlParameter("@DepartmentId", model.DepartmentId));
            sqlParameters.Add(new SqlParameter("@RoleId", model.RoleId));
            sqlParameters.Add(new SqlParameter("@SiteName", siteName));
            sqlParameters.Add(new SqlParameter("@SiteGuid", siteGuid));
            sqlParameters.Add(new SqlParameter("@OperationBy", Operation.OperationBy));

            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);

            ExecuteNonQuery("AddDepartmentRole", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int DeleteDepartmentRole(int id, string siteName, Guid siteGuid)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", id));
            sqlParameters.Add(new SqlParameter("@SiteName", siteName));
            sqlParameters.Add(new SqlParameter("@SiteGuid", siteGuid));
            sqlParameters.Add(new SqlParameter("@OperationBy", Operation.OperationBy));
            return ExecuteNonQuery("DeleteDepartmentRole", sqlParameters.ToArray());
        }

        public static int InsertDepartmentRole(Nestle.WorkFlow.Model.DepartmentRole model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@EmployeeId", model.EmployeeId));
            sqlParameters.Add(new SqlParameter("@RoleId", model.RoleId));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertDepartmentRoleProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateDepartmentRole(Nestle.WorkFlow.Model.DepartmentRole model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@EmployeeId", model.EmployeeId));
            sqlParameters.Add(new SqlParameter("@RoleId", model.RoleId));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateDepartmentRoleProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.DepartmentRole> GetDepartmentRole(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.DepartmentRole> li = new List<Nestle.WorkFlow.Model.DepartmentRole>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//DepartmentRole");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.DepartmentRole>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.DepartmentRole> GetDepartmentRoleResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.DepartmentRole> result = new QueryResult<Nestle.WorkFlow.Model.DepartmentRole>();
            List<Nestle.WorkFlow.Model.DepartmentRole> li = new List<Nestle.WorkFlow.Model.DepartmentRole>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//DepartmentRole");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.DepartmentRole>(node.OuterXml));
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
