using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.DataAccess
{
    public class WriteOffItemsDao : BaseDao
    {
        public static int InsertWriteOffItems(WF.Model.WriteOffItems model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@RequestVersionId", model.RequestVersionId));
            sqlParameters.Add(new SqlParameter("@Code", model.Code));
            sqlParameters.Add(new SqlParameter("@MaterialNumber", model.MaterialNumber));
            sqlParameters.Add(new SqlParameter("@MaterialName", model.MaterialName));
            sqlParameters.Add(new SqlParameter("@Unit", model.Unit));
            sqlParameters.Add(new SqlParameter("@Quantity", model.Quantity));
            sqlParameters.Add(new SqlParameter("@TotalCost", model.TotalCost));
            sqlParameters.Add(new SqlParameter("@ExpiryDate", model.ExpiryDate));
            sqlParameters.Add(new SqlParameter("@BatchNo", model.BatchNo));
            sqlParameters.Add(new SqlParameter("@ReasonforWriteOff", model.ReasonforWriteOff));
            sqlParameters.Add(new SqlParameter("@ActionTakenToMinimizeWrittenoff", model.ActionTakenToMinimizeWrittenoff));
            sqlParameters.Add(new SqlParameter("@ActionTakenToAvoidSituationRecurring", model.ActionTakenToAvoidSituationRecurring));
            sqlParameters.Add(new SqlParameter("@AssetDescription", model.AssetDescription));
            sqlParameters.Add(new SqlParameter("@AssetNumber", model.AssetNumber));
            sqlParameters.Add(new SqlParameter("@CommissionDate", model.CommissionDate));
            sqlParameters.Add(new SqlParameter("@NestleOriginalGBV", model.NestleOriginalGBV));
            sqlParameters.Add(new SqlParameter("@AccumulatedDepreciation", model.AccumulatedDepreciation));
            sqlParameters.Add(new SqlParameter("@AccumulatedImpairmentLosses", model.AccumulatedImpairmentLosses));
            sqlParameters.Add(new SqlParameter("@NestleNBV", model.NestleNBV));
            sqlParameters.Add(new SqlParameter("@EstimatedDisposalValue", model.EstimatedDisposalValue));
            sqlParameters.Add(new SqlParameter("@EstimatedLossGainOnDisposal", model.EstimatedLossGainOnDisposal));
            sqlParameters.Add(new SqlParameter("@DisposalMethod", model.DisposalMethod));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertWriteOffItemsProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateWriteOffItems(WF.Model.WriteOffItems model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@RequestVersionId", model.RequestVersionId));
            sqlParameters.Add(new SqlParameter("@Code", model.Code));
            sqlParameters.Add(new SqlParameter("@MaterialNumber", model.MaterialNumber));
            sqlParameters.Add(new SqlParameter("@MaterialName", model.MaterialName));
            sqlParameters.Add(new SqlParameter("@Unit", model.Unit));
            sqlParameters.Add(new SqlParameter("@Quantity", model.Quantity));
            sqlParameters.Add(new SqlParameter("@TotalCost", model.TotalCost));
            sqlParameters.Add(new SqlParameter("@ExpiryDate", model.ExpiryDate));
            sqlParameters.Add(new SqlParameter("@BatchNo", model.BatchNo));
            sqlParameters.Add(new SqlParameter("@ReasonforWriteOff", model.ReasonforWriteOff));
            sqlParameters.Add(new SqlParameter("@ActionTakenToMinimizeWrittenoff", model.ActionTakenToMinimizeWrittenoff));
            sqlParameters.Add(new SqlParameter("@ActionTakenToAvoidSituationRecurring", model.ActionTakenToAvoidSituationRecurring));
            sqlParameters.Add(new SqlParameter("@AssetDescription", model.AssetDescription));
            sqlParameters.Add(new SqlParameter("@AssetNumber", model.AssetNumber));
            sqlParameters.Add(new SqlParameter("@CommissionDate", model.CommissionDate));
            sqlParameters.Add(new SqlParameter("@NestleOriginalGBV", model.NestleOriginalGBV));
            sqlParameters.Add(new SqlParameter("@AccumulatedDepreciation", model.AccumulatedDepreciation));
            sqlParameters.Add(new SqlParameter("@AccumulatedImpairmentLosses", model.AccumulatedImpairmentLosses));
            sqlParameters.Add(new SqlParameter("@NestleNBV", model.NestleNBV));
            sqlParameters.Add(new SqlParameter("@EstimatedDisposalValue", model.EstimatedDisposalValue));
            sqlParameters.Add(new SqlParameter("@EstimatedLossGainOnDisposal", model.EstimatedLossGainOnDisposal));
            sqlParameters.Add(new SqlParameter("@DisposalMethod", model.DisposalMethod));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateWriteOffItemsProc", sqlParameters.ToArray());
        }

        public static List<WF.Model.WriteOffItems> GetWriteOffItems(string procedureName, SqlParameter[] sqlParamters)
        {
            List<WF.Model.WriteOffItems> li = new List<WF.Model.WriteOffItems>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//WriteOffItems");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.WriteOffItems>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<WF.Model.WriteOffItems> GetWriteOffItemsResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<WF.Model.WriteOffItems> result = new QueryResult<WF.Model.WriteOffItems>();
            List<WF.Model.WriteOffItems> li = new List<WF.Model.WriteOffItems>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//WriteOffItems");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.WriteOffItems>(node.OuterXml));
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
