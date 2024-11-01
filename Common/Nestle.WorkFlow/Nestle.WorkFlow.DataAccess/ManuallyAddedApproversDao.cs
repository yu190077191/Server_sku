using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class ManuallyAddedApproversDao : BaseDao
    {
        public static int SaveManuallyAddedApprovers(string xml, string siteName, Guid siteGuid)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@xml", xml));

            sqlParameters.Add(new SqlParameter("@SiteName", siteName));
            sqlParameters.Add(new SqlParameter("@SiteGuid", siteGuid));
            sqlParameters.Add(new SqlParameter("@OperationBy", Operation.OperationBy));

            return ExecuteNonQuery("SaveManuallyAddedApprovers", sqlParameters.ToArray());
        }

        public static int InsertManuallyAddedApprovers(Nestle.WorkFlow.Model.ManuallyAddedApprovers model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", model.RequestVersionId));
            sqlParameters.Add(new SqlParameter("@MinSequence", model.MinSequence));
            sqlParameters.Add(new SqlParameter("@EmployeeId", model.EmployeeId));
            sqlParameters.Add(new SqlParameter("@Comment", model.Comment));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertManuallyAddedApproversProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateManuallyAddedApprovers(Nestle.WorkFlow.Model.ManuallyAddedApprovers model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", model.RequestVersionId));
            sqlParameters.Add(new SqlParameter("@MinSequence", model.MinSequence));
            sqlParameters.Add(new SqlParameter("@EmployeeId", model.EmployeeId));
            sqlParameters.Add(new SqlParameter("@Comment", model.Comment));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateManuallyAddedApproversProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.ManuallyAddedApprovers> GetManuallyAddedApprovers(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.ManuallyAddedApprovers> li = new List<Nestle.WorkFlow.Model.ManuallyAddedApprovers>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//ManuallyAddedApprovers");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.ManuallyAddedApprovers>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.ManuallyAddedApprovers> GetManuallyAddedApproversResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.ManuallyAddedApprovers> result = new QueryResult<Nestle.WorkFlow.Model.ManuallyAddedApprovers>();
            List<Nestle.WorkFlow.Model.ManuallyAddedApprovers> li = new List<Nestle.WorkFlow.Model.ManuallyAddedApprovers>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//ManuallyAddedApprovers");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.ManuallyAddedApprovers>(node.OuterXml));
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
