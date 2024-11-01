using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.DataAccess
{
    public class UploadResultDao : BaseDao
    {
        public static List<UploadResult> GetUploadResultList(string procedureName, SqlParameter[] sqlParamters)
        {
            List<UploadResult> li = new List<UploadResult>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//UploadResult");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<UploadResult>(node.OuterXml));
                    }
                }
            }

            return li;
        }
    }
}
