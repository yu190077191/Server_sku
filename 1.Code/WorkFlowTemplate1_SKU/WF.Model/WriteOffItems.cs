using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("WriteOffItems")]
    public class WriteOffItems
    {
        [XmlElement("BU")]
        public string BU { get; set; }

        [XmlElement("Number")]
        public string Number { get; set; }

        [XmlElement("ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("PreparedBy")]
        public string PreparedBy { get; set; }

        [XmlElement("RequestId")]
        public int RequestId { get; set; }

        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("RequestVersionId")]
        public int RequestVersionId { get; set; }

        [XmlElement("Code")]
        public string Code { get; set; }

        [XmlElement("MaterialNumber")]
        public string MaterialNumber { get; set; }

        [XmlElement("MaterialName")]
        public string MaterialName { get; set; }

        [XmlElement("Unit")]
        public string Unit { get; set; }

        [XmlElement("Quantity")]
        public decimal Quantity { get; set; }

        [XmlElement("TotalCost")]
        public decimal TotalCost { get; set; }

        [XmlElement("ExpiryDate")]
        public DateTime ExpiryDate { get; set; }

        [XmlElement("BatchNo")]
        public string BatchNo { get; set; }

        [XmlElement("ReasonforWriteOff")]
        public string ReasonforWriteOff { get; set; }

        [XmlElement("ActionTakenToMinimizeWrittenoff")]
        public string ActionTakenToMinimizeWrittenoff { get; set; }

        [XmlElement("ActionTakenToAvoidSituationRecurring")]
        public string ActionTakenToAvoidSituationRecurring { get; set; }

        [XmlElement("AssetDescription")]
        public string AssetDescription { get; set; }

        [XmlElement("AssetNumber")]
        public string AssetNumber { get; set; }

        [XmlElement("CommissionDate")]
        public DateTime CommissionDate { get; set; }

        [XmlElement("NestleOriginalGBV")]
        public decimal? NestleOriginalGBV { get; set; }

        [XmlElement("AccumulatedDepreciation")]
        public decimal? AccumulatedDepreciation { get; set; }

        [XmlElement("AccumulatedImpairmentLosses")]
        public decimal? AccumulatedImpairmentLosses { get; set; }

        [XmlElement("NestleNBV")]
        public decimal? NestleNBV { get; set; }

        [XmlElement("EstimatedDisposalValue")]
        public decimal? EstimatedDisposalValue { get; set; }

        [XmlElement("EstimatedLossGainOnDisposal")]
        public decimal? EstimatedLossGainOnDisposal { get; set; }

        [XmlElement("DisposalMethod")]
        public string DisposalMethod { get; set; }

        [XmlElement("HasSupportingDocuments")]
        public string HasSupportingDocuments { get; set; }

        [XmlElement("RecordStatus")]
        public int RecordStatus { get; set; }

        [XmlElement("CreatedBy")]
        public Guid? CreatedBy { get; set; }

        [XmlElement("CreatedTime")]
        public DateTime? CreatedTime { get; set; }

        [XmlElement("ModifiedBy")]
        public Guid? ModifiedBy { get; set; }

        [XmlElement("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }
    }
}
