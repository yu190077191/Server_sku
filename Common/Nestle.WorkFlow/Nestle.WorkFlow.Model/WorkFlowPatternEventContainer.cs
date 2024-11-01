using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("WorkFlowPatternEventContainer")]
    public class WorkFlowPatternEventContainer
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("InstanceId")]
        public int InstanceId { get; set; }

        [XmlElement("WorkFlowPatternId")]
        public int WorkFlowPatternId { get; set; }

        [XmlElement("EventContainerId")]
        public int EventContainerId { get; set; }

        [XmlElement("Sequence")]
        public int Sequence { get; set; }

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
