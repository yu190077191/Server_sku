using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("EventHandler")]
    public class EventHandler
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("InstanceId")]
        public int InstanceId { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("ChineseName")]
        public string ChineseName { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("StoredProcedureName")]
        public string StoredProcedureName { get; set; }

        [XmlElement("DllFilePath")]
        public string DllFilePath { get; set; }

        [XmlElement("ConfigFilePath")]
        public string ConfigFilePath { get; set; }

        [XmlElement("ClassFullName")]
        public string ClassFullName { get; set; }

        [XmlElement("MethodName")]
        public string MethodName { get; set; }

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
