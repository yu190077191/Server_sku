using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("InternetWorkFlow")]
    public class InternetWorkFlow
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("TypeCode")]
        public string TypeCode { get; set; }

        [XmlElement("InstanceId")]
        public int InstanceId { get; set; }

        [XmlElement("RequestVersionId")]
        public int RequestVersionId { get; set; }

        [XmlElement("UserId_INT")]
        public int? UserId_INT { get; set; }

        [XmlElement("UserId_GUID")]
        public Guid? UserId_GUID { get; set; }

        [XmlElement("UserName")]
        public string UserName { get; set; }

        [XmlElement("StepName")]
        public string StepName { get; set; }

        [XmlElement("Token")]
        public Guid Token { get; set; }

        [XmlElement("Message")]
        public string Message { get; set; }

        [XmlElement("Actions")]
        public string Actions { get; set; }

        [XmlElement("CreatedTime")]
        public DateTime CreatedTime { get; set; }

        [XmlElement("Status")]
        public int Status { get; set; }

        [XmlElement("Note")]
        public string Note { get; set; }

        [XmlElement("IsValid")]
        public bool IsValid { get; set; }

        [XmlElement("Feedback")]
        public string Feedback { get; set; }

        [XmlElement("FeedbackNote")]
        public string FeedbackNote { get; set; }

        [XmlElement("FeedbackStatus")]
        public int? FeedbackStatus { get; set; }

        [XmlElement("FeedbackCreatedTime")]
        public DateTime? FeedbackCreatedTime { get; set; }
    }
}