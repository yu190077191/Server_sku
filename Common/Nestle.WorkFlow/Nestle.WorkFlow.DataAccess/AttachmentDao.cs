using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.DataAccess
{
    public class AttachmentDao : BaseDao
    {
        public static string Upload(string fileFullPath, string tempTableName, string storedProcedureName, int instanceId)
        {
            string columnsSql = string.Empty;
            BulkCopyDao.WriteExcelToDataBase(fileFullPath, tempTableName, ref columnsSql);
            List<UploadResult> li = UploadResultDao.GetUploadResultList(storedProcedureName, new[] { new SqlParameter("@OperationBy", Operation.OperationBy), new SqlParameter("@InstanceId", instanceId) });
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
    }
}
