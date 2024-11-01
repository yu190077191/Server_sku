using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class EventContainerEventsDao : BaseDao
    {
        public static int InsertEventContainerEvents(Nestle.WorkFlow.Model.EventContainerEvents model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@EventContainerId", model.EventContainerId));
            sqlParameters.Add(new SqlParameter("@EventId", model.EventId));
            sqlParameters.Add(new SqlParameter("@TargetEventContainerId", model.TargetEventContainerId));
            sqlParameters.Add(new SqlParameter("@Sequence", model.Sequence));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertEventContainerEventsProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateEventContainerEvents(Nestle.WorkFlow.Model.EventContainerEvents model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@EventContainerId", model.EventContainerId));
            sqlParameters.Add(new SqlParameter("@EventId", model.EventId));
            sqlParameters.Add(new SqlParameter("@TargetEventContainerId", model.TargetEventContainerId));
            sqlParameters.Add(new SqlParameter("@Sequence", model.Sequence));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateEventContainerEventsProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.EventContainerEvents> GetEventContainerEvents(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.EventContainerEvents> li = new List<Nestle.WorkFlow.Model.EventContainerEvents>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//EventContainerEvents");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.EventContainerEvents>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.EventContainerEvents> GetEventContainerEventsResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.EventContainerEvents> result = new QueryResult<Nestle.WorkFlow.Model.EventContainerEvents>();
            List<Nestle.WorkFlow.Model.EventContainerEvents> li = new List<Nestle.WorkFlow.Model.EventContainerEvents>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//EventContainerEvents");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.EventContainerEvents>(node.OuterXml));
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
