using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("WorkFlowPatternJson")]
    public class WorkFlowPatternJson
    {
        [XmlElement("InstanceId")]
        public int InstanceId { get; set; }

        [XmlElement("TypeCode")]
        public string TypeCode { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        public WorkFlowPatternStepDetails[] WorkFlowPatternStepDetailsList { get; set; }
    }
}
