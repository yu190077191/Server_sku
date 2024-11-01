using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("ManuallyAddedApprovers")]
    public class ManuallyAddedApprovers
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("InstanceId")]
        public int InstanceId { get; set; }

        [XmlElement("RequestVersionId")]
        public int RequestVersionId { get; set; }

        [XmlElement("WorkFlowPatternEventContainerId")]
        public int WorkFlowPatternEventContainerId { get; set; }

        [XmlElement("MinSequence")]
        public int MinSequence { get; set; }

        [XmlElement("EmployeeId")]
        public Guid EmployeeId { get; set; }

        [XmlElement("Comment")]
        public string Comment { get; set; }

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

        [XmlElement("ApproveType")]
        public int ApproveType { get; set; }
    }
}
