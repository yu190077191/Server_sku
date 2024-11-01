using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("RequestVersion")]
    public class RequestVersion
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("RequestId")]
        public int RequestId { get; set; }

        [XmlElement("CustomerNumber")]
        public string CustomerNumber { get; set; }

        [XmlElement("CustomerName")]
        public string CustomerName { get; set; }

        [XmlElement("WorkFlowTypeCode")]
        public string WorkFlowTypeCode { get; set; }

        [XmlElement("IsCurrent")]
        public bool IsCurrent { get; set; }

        [XmlElement("State")]
        public int? State { get; set; }

        [XmlElement("BU")]
        public string BU { get; set; }

        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlElement("DepartmentName")]
        public string DepartmentName { get; set; }

        [XmlElement("Number")]
        public string Number { get; set; }

        [XmlElement("PreparedBy")]
        public string PreparedBy { get; set; }

        [XmlElement("ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("Justification")]
        public string Justification { get; set; }

        [XmlElement("Comment")]
        public string Comment { get; set; }

        [XmlElement("ApprovedDate")]
        public DateTime? ApprovedDate { get; set; }

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

        [XmlElement("ReviewedBy")]
        public string ReviewedBy { get; set; }

        [XmlElement("StepName")]
        public string StepName { get; set; }

        [XmlElement("ReviewedResult")]
        public string ReviewedResult { get; set; }

        [XmlElement("ReviewedTime")]
        public DateTime? ReviewedTime { get; set; }

        [XmlElement("Status")]
        public string Status { get; set; }

        [XmlElement("Action")]
        public string Action { get; set; }

        [XmlElement("AttachmentIds")]
        public string AttachmentIds { get; set; }

        [XmlElement("MappingIds")]
        public string MappingIds { get; set; }
    }
}