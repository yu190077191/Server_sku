using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("Field")]
    public class Field
    {
        [XmlElement("Editable")]
        public bool Editable { get; set; }

        [XmlElement("ContractNumber")]
        public string ContractNumber { get; set; }

        [XmlElement("RequestId")]
        public int RequestId { get; set; }

        [XmlElement("RequestType")]
        public string RequestType { get; set; }

        [XmlElement("RelatedLink")]
        public string RelatedLink { get; set; }

        [XmlElement("State")]
        public int State { get; set; }

        [XmlElement("RequiredIsEmpty")]
        public bool RequiredIsEmpty { get; set; }

        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("VendorName")]
        public string VendorName { get; set; }

        public bool IsCurrent = true;

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("Category")]
        public string Category { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Value")]
        public string Value { get; set; }

        [XmlElement("Files")]
        public string Files { get; set; }

        [XmlElement("InputType")]
        public string InputType { get; set; }

        [XmlElement("Specification")]
        public string Specification { get; set; }

        [XmlElement("Values")]
        public string Values { get; set; }

        [XmlElement("TypeCode")]
        public string TypeCode { get; set; }

        [XmlElement("Colspan")]
        public int Colspan { get; set; }

        [XmlElement("Priority")]
        public double Priority { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

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

        [XmlElement("Email")]
        public string Email { get; set; }
    }
}
