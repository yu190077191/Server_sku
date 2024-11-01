using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.DataAccess
{
    public class ProjectRequirement_InfoDao : BaseDao
    {
        public static int InsertProjectRequirement_Info(WF.Model.ProjectRequirement_Info model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@ProjectRequirementId", model.ProjectRequirementId));
            sqlParameters.Add(new SqlParameter("@TypeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@Value", model.Value));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertProjectRequirement_InfoProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateProjectRequirement_Info(WF.Model.ProjectRequirement_Info model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@ProjectRequirementId", model.ProjectRequirementId));
            sqlParameters.Add(new SqlParameter("@TypeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@Value", model.Value));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateProjectRequirement_InfoProc", sqlParameters.ToArray());
        }

        public static List<WF.Model.ProjectRequirement_Info> GetProjectRequirement_Info(string procedureName, SqlParameter[] sqlParamters)
        {
            List<WF.Model.ProjectRequirement_Info> li = new List<WF.Model.ProjectRequirement_Info>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//ProjectRequirement_Info");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.ProjectRequirement_Info>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<WF.Model.ProjectRequirement_Info> GetProjectRequirement_InfoResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<WF.Model.ProjectRequirement_Info> result = new QueryResult<WF.Model.ProjectRequirement_Info>();
            List<WF.Model.ProjectRequirement_Info> li = new List<WF.Model.ProjectRequirement_Info>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//ProjectRequirement_Info");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.ProjectRequirement_Info>(node.OuterXml));
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
