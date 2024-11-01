using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class SystemRoleDao : BaseDao
    {
        public static int InsertSystemRole(Nestle.WorkFlow.Model.SystemRole model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@ParentId", model.ParentId));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@ChineseName", model.ChineseName));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertSystemRoleProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateSystemRole(Nestle.WorkFlow.Model.SystemRole model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@ParentId", model.ParentId));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@ChineseName", model.ChineseName));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            return ExecuteNonQuery("UpdateSystemRoleProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.SystemRole> GetSystemRole(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.SystemRole> li = new List<Nestle.WorkFlow.Model.SystemRole>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//SystemRole");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.SystemRole>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.SystemRole> GetSystemRoleResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.SystemRole> result = new QueryResult<Nestle.WorkFlow.Model.SystemRole>();
            List<Nestle.WorkFlow.Model.SystemRole> li = new List<Nestle.WorkFlow.Model.SystemRole>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//SystemRole");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.SystemRole>(node.OuterXml));
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
