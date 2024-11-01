using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class InternetWorkFlowDao : BaseDao
    {
        public static int InsertInternetWorkFlow(Nestle.WorkFlow.Model.InternetWorkFlow model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@TypeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", model.RequestVersionId));
            sqlParameters.Add(new SqlParameter("@UserId_INT", model.UserId_INT));
            sqlParameters.Add(new SqlParameter("@UserId_GUID", model.UserId_GUID));
            sqlParameters.Add(new SqlParameter("@UserName", model.UserName));
            sqlParameters.Add(new SqlParameter("@StepName", model.StepName));
            sqlParameters.Add(new SqlParameter("@Token", model.Token));
            sqlParameters.Add(new SqlParameter("@Message", model.Message));
            sqlParameters.Add(new SqlParameter("@Actions", model.Actions));
            sqlParameters.Add(new SqlParameter("@Status", model.Status));
            sqlParameters.Add(new SqlParameter("@Note", model.Note));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertInternetWorkFlowProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateInternetWorkFlow(Nestle.WorkFlow.Model.InternetWorkFlow model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@TypeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", model.RequestVersionId));
            sqlParameters.Add(new SqlParameter("@UserId_INT", model.UserId_INT));
            sqlParameters.Add(new SqlParameter("@UserId_GUID", model.UserId_GUID));
            sqlParameters.Add(new SqlParameter("@UserName", model.UserName));
            sqlParameters.Add(new SqlParameter("@StepName", model.StepName));
            sqlParameters.Add(new SqlParameter("@Token", model.Token));
            sqlParameters.Add(new SqlParameter("@Message", model.Message));
            sqlParameters.Add(new SqlParameter("@Actions", model.Actions));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@Status", model.Status));
            sqlParameters.Add(new SqlParameter("@Note", model.Note));
            return ExecuteNonQuery("UpdateInternetWorkFlowProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.InternetWorkFlow> GetInternetWorkFlow(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.InternetWorkFlow> li = new List<Nestle.WorkFlow.Model.InternetWorkFlow>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//InternetWorkFlow");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.InternetWorkFlow>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.InternetWorkFlow> GetInternetWorkFlowResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.InternetWorkFlow> result = new QueryResult<Nestle.WorkFlow.Model.InternetWorkFlow>();
            List<Nestle.WorkFlow.Model.InternetWorkFlow> li = new List<Nestle.WorkFlow.Model.InternetWorkFlow>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//InternetWorkFlow");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.InternetWorkFlow>(node.OuterXml));
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
