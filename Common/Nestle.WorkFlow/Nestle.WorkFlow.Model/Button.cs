using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("Button")]
    public class Button
    {
        [XmlElement("WorkFlowId")]
        public int WorkFlowId { get; set; }

        [XmlElement("EventId")]
        public int EventId { get; set; }

        [XmlElement("EventName")]
        public string EventName { get; set; }
    }
}