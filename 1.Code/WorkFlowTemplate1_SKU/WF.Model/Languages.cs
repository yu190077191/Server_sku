using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("Languages")]
    public class Languages
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Language")]
        public string Language { get; set; }

        [XmlElement("Priority")]
        public double? Priority { get; set; }

        [XmlElement("EnglishName")]
        public string EnglishName { get; set; }

        [XmlElement("RecordStatus")]
        public int? RecordStatus { get; set; }
    }
}
