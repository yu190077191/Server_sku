using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("WordParameters")]
    public class WordParameters
    {
        [XmlElement("Path")]
        public string Path { get; set; }

        [XmlElement("Code")]
        public string Code { get; set; }

        [XmlElement("Value")]
        public string Value { get; set; }
    }
}
