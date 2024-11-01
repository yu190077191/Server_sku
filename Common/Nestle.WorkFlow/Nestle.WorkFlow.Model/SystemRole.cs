using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("SystemRole")]
    public class SystemRole
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("ParentId")]
        public int? ParentId { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("ChineseName")]
        public string ChineseName { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("RecordStatus")]
        public int RecordStatus { get; set; }
    }
}
