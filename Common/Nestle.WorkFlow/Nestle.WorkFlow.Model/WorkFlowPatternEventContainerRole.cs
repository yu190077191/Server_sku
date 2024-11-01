using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("WorkFlowPatternEventContainerRole")]
    public class WorkFlowPatternEventContainerRole
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("InstanceId")]
        public int InstanceId { get; set; }

        [XmlElement("WorkFlowPatternEventContainerId")]
        public int WorkFlowPatternEventContainerId { get; set; }

        [XmlElement("RoleId")]
        public int RoleId { get; set; }

        [XmlElement("Sequence")]
        public int Sequence { get; set; }

        [XmlElement("NeedAllMemebersApproval")]
        public bool NeedAllMemebersApproval { get; set; }

        [XmlElement("MinWeight")]
        public double MinWeight { get; set; }

        [XmlElement("Weight")]
        public double Weight { get; set; }

        [XmlElement("EscalationDays")]
        public int? EscalationDays { get; set; }

        [XmlElement("EscalationEventId")]
        public int? EscalationEventId { get; set; }

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
