using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("WorkFlowApprover")]
    public class WorkFlowApprover
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("InstanceId")]
        public int InstanceId { get; set; }

        [XmlElement("WorkFlowId")]
        public int WorkFlowId { get; set; }

        [XmlElement("ApproverEmployeeId")]
        public Guid ApproverEmployeeId { get; set; }

        [XmlElement("ApproverEmployeeName")]
        public string ApproverEmployeeName { get; set; }

        [XmlElement("Status")]
        public int Status { get; set; }

        [XmlElement("Sequence")]
        public double Sequence { get; set; }

        [XmlElement("NeedAllMemebersApproval")]
        public bool NeedAllMemebersApproval { get; set; }

        [XmlElement("MinWeight")]
        public double MinWeight { get; set; }

        [XmlElement("Weight")]
        public double Weight { get; set; }

        [XmlElement("ReviewResult")]
        public int ReviewResult { get; set; }

        [XmlElement("ReviewTime")]
        public DateTime? ReviewTime { get; set; }

        [XmlElement("ReviewComment")]
        public string ReviewComment { get; set; }

        [XmlElement("IsManuallyAdded")]
        public bool IsManuallyAdded { get; set; }

        [XmlElement("EscalationEventId")]
        public int? EscalationEventId { get; set; }

        [XmlElement("EscalationTime")]
        public DateTime? EscalationTime { get; set; }

        [XmlElement("ApproveType")]
        public int ApproveType { get; set; }

        [XmlElement("ApprovalXml")]
        public string ApprovalXml { get; set; }

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
