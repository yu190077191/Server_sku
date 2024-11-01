using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("UploadResult")]
    public class UploadResult
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Message")]
        public string Message { get; set; }

        [XmlElement("TableName")]
        public string TableName { get; set; }

        [XmlElement("RowNumber")]
        public string RowNumber { get; set; }
    }
}