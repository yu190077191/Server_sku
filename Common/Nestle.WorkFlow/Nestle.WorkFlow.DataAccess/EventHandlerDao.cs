using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class EventHandlerDao : BaseDao
    {
        public static int InsertEventHandler(Nestle.WorkFlow.Model.EventHandler model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@ChineseName", model.ChineseName));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@StoredProcedureName", model.StoredProcedureName));
            sqlParameters.Add(new SqlParameter("@DllFilePath", model.DllFilePath));
            sqlParameters.Add(new SqlParameter("@ConfigFilePath", model.ConfigFilePath));
            sqlParameters.Add(new SqlParameter("@ClassFullName", model.ClassFullName));
            sqlParameters.Add(new SqlParameter("@MethodName", model.MethodName));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertEventHandlerProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateEventHandler(Nestle.WorkFlow.Model.EventHandler model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@ChineseName", model.ChineseName));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@StoredProcedureName", model.StoredProcedureName));
            sqlParameters.Add(new SqlParameter("@DllFilePath", model.DllFilePath));
            sqlParameters.Add(new SqlParameter("@ConfigFilePath", model.ConfigFilePath));
            sqlParameters.Add(new SqlParameter("@ClassFullName", model.ClassFullName));
            sqlParameters.Add(new SqlParameter("@MethodName", model.MethodName));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateEventHandlerProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.EventHandler> GetEventHandler(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.EventHandler> li = new List<Nestle.WorkFlow.Model.EventHandler>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//EventHandler");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.EventHandler>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.EventHandler> GetEventHandlerResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.EventHandler> result = new QueryResult<Nestle.WorkFlow.Model.EventHandler>();
            List<Nestle.WorkFlow.Model.EventHandler> li = new List<Nestle.WorkFlow.Model.EventHandler>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//EventHandler");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.EventHandler>(node.OuterXml));
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
