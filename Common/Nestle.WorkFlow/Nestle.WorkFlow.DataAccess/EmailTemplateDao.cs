using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class EmailTemplateDao : BaseDao
    {
        public static int InsertEmailTemplate(Nestle.WorkFlow.Model.EmailTemplate model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@EmailTypeId", model.EmailTypeId));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@Title", model.Title));
            sqlParameters.Add(new SqlParameter("@Body", model.Body));
            sqlParameters.Add(new SqlParameter("@FilePath", model.FilePath));
            sqlParameters.Add(new SqlParameter("@IsDefault", model.IsDefault));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertEmailTemplateProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateEmailTemplate(Nestle.WorkFlow.Model.EmailTemplate model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@InstanceId", model.InstanceId));
            sqlParameters.Add(new SqlParameter("@EmailTypeId", model.EmailTypeId));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            sqlParameters.Add(new SqlParameter("@Title", model.Title));
            sqlParameters.Add(new SqlParameter("@Body", model.Body));
            sqlParameters.Add(new SqlParameter("@FilePath", model.FilePath));
            sqlParameters.Add(new SqlParameter("@IsDefault", model.IsDefault));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateEmailTemplateProc", sqlParameters.ToArray());
        }

        public static List<Nestle.WorkFlow.Model.EmailTemplate> GetEmailTemplate(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Nestle.WorkFlow.Model.EmailTemplate> li = new List<Nestle.WorkFlow.Model.EmailTemplate>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//EmailTemplate");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.EmailTemplate>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Nestle.WorkFlow.Model.EmailTemplate> GetEmailTemplateResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Nestle.WorkFlow.Model.EmailTemplate> result = new QueryResult<Nestle.WorkFlow.Model.EmailTemplate>();
            List<Nestle.WorkFlow.Model.EmailTemplate> li = new List<Nestle.WorkFlow.Model.EmailTemplate>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//EmailTemplate");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Nestle.WorkFlow.Model.EmailTemplate>(node.OuterXml));
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
