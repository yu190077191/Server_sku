using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("LanguageDictionary")]
    public class LanguageDictionary
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("LanguageId")]
        public int LanguageId { get; set; }

        [XmlElement("LanguageKey")]
        public string LanguageKey { get; set; }

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
