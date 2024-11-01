using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("APIInfo")]
    public class APIInfo
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Step")]
        public string Step { get; set; }

        [XmlElement("Subject")]
        public string Subject { get; set; }

        [XmlElement("To")]
        public string To { get; set; }

        [XmlElement("Cc")]
        public string Cc { get; set; }

        [XmlElement("Body")]
        public string Body { get; set; }
    }
}