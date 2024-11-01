using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("Attachment")]
    public class Attachment
    {
        [XmlElement("Id")]
        public Guid Id { get; set; }

        [XmlElement("TypeCode")]
        public string TypeCode { get; set; }

        [XmlElement("SubCode")]
        public string SubCode { get; set; }

        [XmlElement("RequestVersionId")]
        public int RequestVersionId { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("FilePath")]
        public string FilePath { get; set; }

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

        [XmlElement("IsPapperApproved")]
        public bool IsPapperApproved { get; set; }
    }
}
