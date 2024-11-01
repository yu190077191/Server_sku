using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("ApproverWorkList")]
    public class ApproverWorkList
    {
        [XmlElement("RequestVersionId")]
        public int RequestVersionId { get; set; }

        [XmlElement("IsPending")]
        public bool IsPending { get; set; }
    }
}