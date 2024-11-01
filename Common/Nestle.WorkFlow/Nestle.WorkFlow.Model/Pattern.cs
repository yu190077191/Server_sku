using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("Pattern")]
    public class Pattern
    {
        [XmlElement("WorkFlowPatternId")]
        public int WorkFlowPatternId { get; set; }

        [XmlElement("WorkFlowPatternEventContainerId")]
        public int WorkFlowPatternEventContainerId { get; set; }

        [XmlElement("InstanceId")]
        public int InstanceId { get; set; }

        [XmlElement("TypeCode")]
        public string TypeCode { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("WorkFlowPatternName")]
        public string WorkFlowPatternName { get; set; }

        [XmlElement("Sequence")]
        public int Sequence { get; set; }

        [XmlElement("Role")]
        public string Role { get; set; }

        [XmlElement("Event")]
        public string Event { get; set; }
    }
}