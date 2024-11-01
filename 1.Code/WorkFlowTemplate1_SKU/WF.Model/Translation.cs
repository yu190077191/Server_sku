using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("Translation")]
    public class Translation
    {
        [XmlElement("Key")]
        public string Key { get; set; }

        [XmlElement("Chinese")]
        public string Chinese { get; set; }

        [XmlElement("English")]
        public string English { get; set; }

        [XmlElement("CreatedBy")]
        public Guid? CreatedBy { get; set; }
    }
}
