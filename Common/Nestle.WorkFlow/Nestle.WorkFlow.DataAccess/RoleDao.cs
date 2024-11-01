using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class RoleDao : BaseDao
    {
        public static int InsertRole(Nestle.WorkFlow.Model.Role model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@ParentId", model.ParentId));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@ChineseName", model.ChineseName));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertRoleProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateRole(Nestle.WorkFlow.Model.Role model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@ParentId", model.ParentId));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@ChineseName", model.ChineseName));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            return ExecuteNonQuery("UpdateRoleProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.Role> GetRole(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.Role> li = new List<Nestle.WorkFlow.Model.Role>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//Role");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.Role>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.Role> GetRoleResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.Role> result = new QueryResult<Nestle.WorkFlow.Model.Role>();
            List<Nestle.WorkFlow.Model.Role> li = new List<Nestle.WorkFlow.Model.Role>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//Role");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.Role>(node.OuterXml));
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
