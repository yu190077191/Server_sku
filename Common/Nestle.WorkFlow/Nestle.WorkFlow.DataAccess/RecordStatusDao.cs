using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class RecordStatusDao : BaseDao
    {
        public static int InsertRecordStatus(Nestle.WorkFlow.Model.RecordStatus model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            return ExecuteNonQuery("InsertRecordStatusProc", sqlParameters.ToArray());
        }

        public static int UpdateRecordStatus(Nestle.WorkFlow.Model.RecordStatus model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            return ExecuteNonQuery("UpdateRecordStatusProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.RecordStatus> GetRecordStatus(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.RecordStatus> li = new List<Nestle.WorkFlow.Model.RecordStatus>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//RecordStatus");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.RecordStatus>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.RecordStatus> GetRecordStatusResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.RecordStatus> result = new QueryResult<Nestle.WorkFlow.Model.RecordStatus>();
            List<Nestle.WorkFlow.Model.RecordStatus> li = new List<Nestle.WorkFlow.Model.RecordStatus>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//RecordStatus");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.RecordStatus>(node.OuterXml));
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
