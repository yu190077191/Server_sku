using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("EmailTemplate")]
    public class EmailTemplate
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("InstanceId")]
        public int InstanceId { get; set; }

        [XmlElement("EmailTypeId")]
        public int? EmailTypeId { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Title")]
        public string Title { get; set; }

        [XmlElement("Body")]
        public string Body { get; set; }

        [XmlElement("FilePath")]
        public string FilePath { get; set; }

        [XmlElement("IsDefault")]
        public bool? IsDefault { get; set; }

        [XmlElement("RecordStatus")]
        public int RecordStatus { get; set; }

        [XmlElement("CreatedBy")]
        public Guid? CreatedBy { get; set; }

        [XmlElement("CreatedTime")]
        public DateTime? CreatedTime { get; set; }

        [XmlElement("ModifiedBy")]
        public Guid? ModifiedBy { get; set; }

        [XmlElement("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }
    }
}
