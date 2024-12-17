using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("BarCodeEmail")]
    public class BarCodeEmail
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("StepNumber")]
        public int StepNumber { get; set; }

        [XmlElement("BuName")]
        public string BuName { get; set; }

        [XmlElement("EventContainerName")]
        public string EventContainerName { get; set; }

        [XmlElement("Category_Classification")]
        public string Category_Classification { get; set; }

        [XmlElement("mail")]
        public string mail { get; set; }

        [XmlElement("DisplayName")]
        public string DisplayName { get; set; }

        [XmlElement("Checked")]
        public bool Checked { get; set; }
    }
}