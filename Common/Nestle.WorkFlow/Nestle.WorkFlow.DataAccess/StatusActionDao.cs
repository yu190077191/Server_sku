using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class StatusActionDao : BaseDao
    {
        public static int InsertStatusAction(Nestle.WorkFlow.Model.StatusAction model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@WorkFlowRoleId", model.WorkFlowRoleId));
            sqlParameters.Add(new SqlParameter("@StatusId", model.StatusId));
            sqlParameters.Add(new SqlParameter("@ActionId", model.ActionId));
            sqlParameters.Add(new SqlParameter("@Priority", model.Priority));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertStatusActionProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateStatusAction(Nestle.WorkFlow.Model.StatusAction model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@WorkFlowRoleId", model.WorkFlowRoleId));
            sqlParameters.Add(new SqlParameter("@StatusId", model.StatusId));
            sqlParameters.Add(new SqlParameter("@ActionId", model.ActionId));
            sqlParameters.Add(new SqlParameter("@Priority", model.Priority));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateStatusActionProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.StatusAction> GetStatusAction(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.StatusAction> li = new List<Nestle.WorkFlow.Model.StatusAction>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//StatusAction");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.StatusAction>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.StatusAction> GetStatusActionResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.StatusAction> result = new QueryResult<Nestle.WorkFlow.Model.StatusAction>();
            List<Nestle.WorkFlow.Model.StatusAction> li = new List<Nestle.WorkFlow.Model.StatusAction>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//StatusAction");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.StatusAction>(node.OuterXml));
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
