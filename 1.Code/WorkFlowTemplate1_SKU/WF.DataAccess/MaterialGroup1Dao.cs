using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.DataAccess
{
    public class MaterialGroup1Dao : BaseDao
    {
        public static int InsertMaterialGroup1(WF.Model.MaterialGroup1 model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            return ExecuteNonQuery("InsertMaterialGroup1Proc", sqlParameters.ToArray());
        }

        public static int UpdateMaterialGroup1(WF.Model.MaterialGroup1 model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@Name", model.Name));
            return ExecuteNonQuery("UpdateMaterialGroup1Proc", sqlParameters.ToArray());
        }

        public static List<WF.Model.MaterialGroup1> GetMaterialGroup1(string procedureName, SqlParameter[] sqlParamters)
        {
            List<WF.Model.MaterialGroup1> li = new List<WF.Model.MaterialGroup1>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//MaterialGroup1");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.MaterialGroup1>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<WF.Model.MaterialGroup1> GetMaterialGroup1Result(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<WF.Model.MaterialGroup1> result = new QueryResult<WF.Model.MaterialGroup1>();
            List<WF.Model.MaterialGroup1> li = new List<WF.Model.MaterialGroup1>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//MaterialGroup1");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.MaterialGroup1>(node.OuterXml));
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
