using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.DataAccess
{
    public class RequestVersionDao : BaseDao
    {
        public static int UpdateStatus(int requestVersionId, int status)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", requestVersionId));
            sqlParameters.Add(new SqlParameter("@Status", status));
            sqlParameters.Add(new SqlParameter("@OperationBy", Operation.OperationBy));
            return ExecuteNonQuery("UpdateStatus", sqlParameters.ToArray());
        }

        public static int InsertRequestVersion(WF.Model.RequestVersion model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@RequestId", model.RequestId));
            sqlParameters.Add(new SqlParameter("@IsCurrent", model.IsCurrent));
            sqlParameters.Add(new SqlParameter("@State", model.State));
            sqlParameters.Add(new SqlParameter("@Number", model.Number));
            sqlParameters.Add(new SqlParameter("@PreparedBy", model.PreparedBy));
            sqlParameters.Add(new SqlParameter("@ProjectName", model.ProjectName));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@Justification", model.Justification));
            sqlParameters.Add(new SqlParameter("@Comment", model.Comment));
            sqlParameters.Add(new SqlParameter("@ApprovedDate", model.ApprovedDate));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertRequestVersionProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateRequestVersion(WF.Model.RequestVersion model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@RequestId", model.RequestId));
            sqlParameters.Add(new SqlParameter("@IsCurrent", model.IsCurrent));
            sqlParameters.Add(new SqlParameter("@State", model.State));
            sqlParameters.Add(new SqlParameter("@BU", model.BU));
            sqlParameters.Add(new SqlParameter("@DepartmentId", model.DepartmentId));
            sqlParameters.Add(new SqlParameter("@DepartmentName", model.DepartmentName));
            sqlParameters.Add(new SqlParameter("@Number", model.Number));
            sqlParameters.Add(new SqlParameter("@PreparedBy", model.PreparedBy));
            sqlParameters.Add(new SqlParameter("@ProjectName", model.ProjectName));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@Justification", model.Justification));
            sqlParameters.Add(new SqlParameter("@Comment", model.Comment));
            sqlParameters.Add(new SqlParameter("@ApprovedDate", model.ApprovedDate));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
                                                      return ExecuteNonQuery("UpdateRequestVersionProc", sqlParameters.ToArray());
        }

        public static int UpdateField(WF.Model.UpdateField model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@RequestId", model.RequestId));
            sqlParameters.Add(new SqlParameter("@f26", model.f26));
            sqlParameters.Add(new SqlParameter("@f27", model.f27));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("UpdateUpdateFieldProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static List<WF.Model.RequestVersion> GetRequestVersion(string procedureName, SqlParameter[] sqlParamters)
        {
            List<WF.Model.RequestVersion> li = new List<WF.Model.RequestVersion>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//RequestVersion");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.RequestVersion>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<WF.Model.RequestVersion> GetRequestVersionResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<WF.Model.RequestVersion> result = new QueryResult<WF.Model.RequestVersion>();
            List<WF.Model.RequestVersion> li = new List<WF.Model.RequestVersion>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//RequestVersion");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.RequestVersion>(node.OuterXml));
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
