using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("ApproverItem")]
    public class ApproverItem
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("RequestVersionId")]
        public int RequestVersionId { get; set; }

        [XmlElement("IsPending")]
        public bool IsPending { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Requester")]
        public string Requester { get; set; }

        [XmlElement("CreatedTime")]
        public DateTime CreatedTime { get; set; }

        [XmlElement("BU")]
        public string BU { get; set; }

        [XmlElement("ReviewedBy")]
        public string ReviewedBy { get; set; }

        [XmlElement("StepName")]
        public string StepName { get; set; }

        [XmlElement("ReviewedResult")]
        public string ReviewedResult { get; set; }

        [XmlElement("ReviewedTime")]
        public DateTime? ReviewedTime { get; set; }

        [XmlElement("Status")]
        public string Status { get; set; }

        [XmlElement("Action")]
        public string Action { get; set; }

        [XmlElement("Comment")]
        public string Comment { get; set; }

        [XmlElement("MaterialNumber")]
        public string MaterialNumber { get; set; }
    }
}