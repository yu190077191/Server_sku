using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("WorkFlow")]
    public class WorkFlow
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("InstanceId")]
        public int InstanceId { get; set; }

        [XmlElement("RequestId")]
        public int RequestId { get; set; }

        [XmlElement("RequestVersionId")]
        public int RequestVersionId { get; set; }

        [XmlElement("WorkFlowPatternId")]
        public int WorkFlowPatternId { get; set; }

        [XmlElement("Originator")]
        public Guid Originator { get; set; }

        [XmlElement("WorkFlowStatusId")]
        public int WorkFlowStatusId { get; set; }

        [XmlElement("StatusId")]
        public int StatusId { get; set; }

        [XmlElement("Data")]
        public string Data { get; set; }

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
