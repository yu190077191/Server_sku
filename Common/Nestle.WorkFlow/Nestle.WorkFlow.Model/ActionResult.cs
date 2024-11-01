using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("ExecuteResult")]
    public class ExecuteResult
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("IsSuccess")]
        public bool IsSuccess { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Message")]
        public string Message { get; set; }

        [XmlElement("Status")]
        public int Status { get; set; }
    }
}
