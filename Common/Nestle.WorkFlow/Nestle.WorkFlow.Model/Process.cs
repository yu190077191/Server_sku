using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("Process")]
    public class Process
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Text1")]
        public string Text1 { get; set; }

        [XmlElement("Text3")]
        public string Text3 { get; set; }

        [XmlElement("AboveLineText")]
        public string AboveLineText { get; set; }

        [XmlElement("RecordStatus")]
        public int RecordStatus { get; set; }

        [XmlElement("ProcessState")]
        public int ProcessState { get; set; }
    }
}
