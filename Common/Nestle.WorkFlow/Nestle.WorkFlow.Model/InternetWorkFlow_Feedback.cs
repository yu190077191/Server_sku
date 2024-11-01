using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("InternetWorkFlow_Feedback")]
    public class InternetWorkFlow_Feedback
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("InternetWorkFlowId")]
        public int InternetWorkFlowId { get; set; }

        [XmlElement("Feedback")]
        public string Feedback { get; set; }

        [XmlElement("Note")]
        public string Note { get; set; }

        [XmlElement("Status")]
        public int Status { get; set; }
    }
}
