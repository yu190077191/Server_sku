using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class InternetWorkFlow_FeedbackDao : BaseDao
    {
        public static int InsertInternetWorkFlow_Feedback(Nestle.WorkFlow.Model.InternetWorkFlow_Feedback model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@InternetWorkFlowId", model.InternetWorkFlowId));
            sqlParameters.Add(new SqlParameter("@Feedback", model.Feedback));
            sqlParameters.Add(new SqlParameter("@Note", model.Note));
            sqlParameters.Add(new SqlParameter("@Status", model.Status));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertInternetWorkFlow_FeedbackProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateInternetWorkFlow_Feedback(Nestle.WorkFlow.Model.InternetWorkFlow_Feedback model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InternetWorkFlowId", model.InternetWorkFlowId));
            sqlParameters.Add(new SqlParameter("@Feedback", model.Feedback));
            sqlParameters.Add(new SqlParameter("@Note", model.Note));
            sqlParameters.Add(new SqlParameter("@Status", model.Status));
            return ExecuteNonQuery("UpdateInternetWorkFlow_FeedbackProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.InternetWorkFlow_Feedback> GetInternetWorkFlow_Feedback(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.InternetWorkFlow_Feedback> li = new List<Nestle.WorkFlow.Model.InternetWorkFlow_Feedback>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//InternetWorkFlow_Feedback");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.InternetWorkFlow_Feedback>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.InternetWorkFlow_Feedback> GetInternetWorkFlow_FeedbackResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.InternetWorkFlow_Feedback> result = new QueryResult<Nestle.WorkFlow.Model.InternetWorkFlow_Feedback>();
            List<Nestle.WorkFlow.Model.InternetWorkFlow_Feedback> li = new List<Nestle.WorkFlow.Model.InternetWorkFlow_Feedback>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//InternetWorkFlow_Feedback");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.InternetWorkFlow_Feedback>(node.OuterXml));
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
