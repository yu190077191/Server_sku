using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("WorkFlowPatternDebug")]
    public class WorkFlowPatternDebug
    {
        [XmlElement("Sequence")]
        public int Sequence { get; set; }

        [XmlElement("StepName")]
        public string StepName { get; set; }

        [XmlElement("RoleName")]
        public string RoleName { get; set; }

        [XmlElement("Department")]
        public string Department { get; set; }

        [XmlElement("UserId")]
        public string UserId { get; set; }

        [XmlElement("EmployeeName")]
        public string EmployeeName { get; set; }

        [XmlElement("EmployeeId")]
        public Guid? EmployeeId { get; set; }
    }
}