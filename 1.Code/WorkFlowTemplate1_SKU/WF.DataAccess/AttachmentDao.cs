using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.DataAccess
{
    public class AttachmentDao : BaseDao
    {
        public static string SaveData(string storedProcedureName, Guid UniqueId, int id)
        {
            List<UploadResult> li = UploadResultDao.GetUploadResultList(storedProcedureName, new[] {
                new SqlParameter("@OperationBy", Operation.OperationBy),
                new SqlParameter("@UniqueId", UniqueId),
                new SqlParameter("@Id", id)
            });

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<tr>\r\n");
            sb.Append("<th>Id</th>\r\n");
            sb.Append("<th>Name</th>\r\n");
            sb.Append("<th>Message</th>\r\n");
            sb.Append("</tr>\r\n");

            if (li != null && li.Count > 0)
            {
                foreach (UploadResult item in li)
                {
                    sb.Append("<tr onmouseover=\"this.className='trFocused'\" onmouseout=\"this.className='trCommon'\">\r\n");
                    sb.Append("<td class=\"listtd\" style=\"padding:5px;\">" + item.Id + "</td>\r\n");
                    sb.Append("<td class=\"listtd\" style=\"padding:5px;\">" + item.Name + "</td>\r\n");
                    sb.Append("<td class=\"listtd\" style=\"padding:5px;\">" + item.Message + "</td>\r\n");
                    sb.Append("</tr>\r\n");
                }

                return sb.ToString();
            }
            else
            {
                sb.Append("<tr onmouseover=\"this.className='trFocused'\" onmouseout=\"this.className='trCommon'\">\r\n");
                sb.Append("<td class=\"listtd\" style=\"padding:5px;\">1</td>\r\n");
                sb.Append("<td class=\"listtd\" style=\"padding:5px;\">OK</td>\r\n");
                sb.Append("<td class=\"listtd\" style=\"padding:5px;\">All data uploaded!</td>\r\n");
                sb.Append("</tr>\r\n");

                return sb.ToString();
            }
        }

        public static string Upload(string fileFullPath, string tempTableName, string storedProcedureName, Guid UniqueId, int? maxColumnCount = null, bool addUploadedTime = false)
        {
            string columnsSql = string.Empty;
            BulkCopyDao.WriteExcelToDataBase(fileFullPath, tempTableName, ref columnsSql, UniqueId, false, maxColumnCount, addUploadedTime);
            List<UploadResult> li = UploadResultDao.GetUploadResultList(storedProcedureName, new[] { 
                new SqlParameter("@OperationBy", Operation.OperationBy),
                new SqlParameter("@UniqueId", UniqueId),
                new SqlParameter("@TableName", tempTableName)
            });

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<tr>\r\n");
            sb.Append("<th>Id</th>\r\n");
            sb.Append("<th>Name</th>\r\n");
            sb.Append("<th>Message</th>\r\n");
            sb.Append("</tr>\r\n");

            if (li != null && li.Count > 0)
            {
                foreach (UploadResult item in li)
                {
                    sb.Append("<tr onmouseover=\"this.className='trFocused'\" onmouseout=\"this.className='trCommon'\">\r\n");
                    sb.Append("<td class=\"listtd\" style=\"padding:5px;\">" + item.Id + "</td>\r\n");
                    sb.Append("<td class=\"listtd\" style=\"padding:5px;\">" + item.Name + "</td>\r\n");
                    sb.Append("<td class=\"listtd\" style=\"padding:5px;\">" + item.Message + "</td>\r\n");
                    sb.Append("</tr>\r\n");
                }

                return sb.ToString();
            }
            else
            {
                sb.Append("<tr onmouseover=\"this.className='trFocused'\" onmouseout=\"this.className='trCommon'\">\r\n");
                sb.Append("<td class=\"listtd\" style=\"padding:5px;\">1</td>\r\n");
                sb.Append("<td class=\"listtd\" style=\"padding:5px;\">OK</td>\r\n");
                sb.Append("<td class=\"listtd\" style=\"padding:5px;\">All data uploaded!</td>\r\n");
                sb.Append("</tr>\r\n");

                return sb.ToString();
            }
        }

        public static int UpdateAttachmentPapperApprovedStatus(Guid attachmentId, int recordstatus)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@AttachmentId", attachmentId));
            sqlParameters.Add(new SqlParameter("@RecordStatus", recordstatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            return ExecuteNonQuery("UpdateAttachmentPapperApprovedStatus", sqlParameters.ToArray());
        }

        public static int InsertAttachment(Attachment model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@TypeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@SubCode", model.SubCode));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", model.RequestVersionId));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@FilePath", model.FilePath));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            return ExecuteNonQuery("InsertAttachmentProc", sqlParameters.ToArray());
        }

        public static int UpdateAttachment(Attachment model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@TypeCode", model.TypeCode));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", model.RequestVersionId));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@FilePath", model.FilePath));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateAttachmentProc", sqlParameters.ToArray());
        }

        public static List<Attachment> GetAttachment(string procedureName, SqlParameter[] sqlParamters)
        {
            List<Attachment> li = new List<Attachment>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//Attachment");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Attachment>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<Attachment> GetAttachmentResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<Attachment> result = new QueryResult<Attachment>();
            List<Attachment> li = new List<Attachment>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//Attachment");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<Attachment>(node.OuterXml));
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
