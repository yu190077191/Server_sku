using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("ProjectRequirement_Info")]
    public class ProjectRequirement_Info
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("ProjectRequirementId")]
        public int ProjectRequirementId { get; set; }

        [XmlElement("TypeCode")]
        public string TypeCode { get; set; }

        [XmlElement("Value")]
        public string Value { get; set; }

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
