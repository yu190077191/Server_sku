using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("Department")]
    public class Department
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("InstanceId")]
        public int InstanceId { get; set; }

        [XmlElement("ParentId")]
        public int? ParentId { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("PlantCode")]
        public string PlantCode { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("ChineseName")]
        public string ChineseName { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("RecordStatus")]
        public int RecordStatus { get; set; }
    }
}
