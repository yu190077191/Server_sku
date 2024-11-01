using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("WorkFlowPatternStepDetails")]
    public class WorkFlowPatternStepDetails
    {
        [XmlElement("Sequence")]
        public int Sequence { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Role")]
        public string Role { get; set; }

        [XmlElement("Event")]
        public string Event { get; set; }
    }
}
