using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("ActionData")]
    public class ActionData
    {
        [XmlElement("RequestId")]
        public int RequestId { get; set; }

        [XmlElement("RequestVersionId")]
        public int RequestVersionId { get; set; }

        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlElement("EventId")]
        public int EventId { get; set; }

        [XmlElement("WorkFlowId")]
        public int WorkFlowId { get; set; }

        [XmlElement("SiteName")]
        public string SiteName { get; set; }

        [XmlElement("SiteGuid")]
        public Guid SiteGuid { get; set; }

        [XmlElement("TypeCode")]
        public string TypeCode { get; set; }

        [XmlElement("Comment")]
        public string Comment { get; set; }

        [XmlElement("Xml")]
        public string Xml { get; set; }
         
        [XmlElement("Eauction")] 
         public string Eauction { get; set; }

        [XmlElement("ReturnStep")]
        public string ReturnStep { get; set; }


    }
}