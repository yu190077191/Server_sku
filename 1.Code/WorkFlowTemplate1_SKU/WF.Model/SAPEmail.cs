using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("SAPEmail")]
    public class SAPEmail
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("StepNumber")]
        public int StepNumber { get; set; }

        [XmlElement("StepName")]
        public string StepName { get; set; }

        [XmlElement("BU")]
        public string BU { get; set; }

        [XmlElement("Plant")]
        public string Plant { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlElement("DisplayName")]
        public string DisplayName { get; set; }

        [XmlElement("RowNumber")]
        public int RowNumber { get; set; }

        [XmlElement("Subject")]
        public string Subject { get; set; }

        [XmlElement("To")]
        public string To { get; set; }

        [XmlElement("Cc")]
        public string Cc { get; set; }

        [XmlElement("Body")]
        public string Body { get; set; }

        [XmlElement("Checked")]
        public bool Checked { get; set; }
    }
}