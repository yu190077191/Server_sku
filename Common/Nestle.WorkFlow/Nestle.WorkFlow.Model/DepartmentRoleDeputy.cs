using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("DepartmentRoleDeputy")]
    public class DepartmentRoleDeputy
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Department")]
        public string Department { get; set; }

        [XmlElement("Role")]
        public string Role { get; set; }

        [XmlElement("DeputyName")]
        public string DeputyName { get; set; }

        [XmlElement("DepartmentRoleId")]
        public int DepartmentRoleId { get; set; }

        [XmlElement("StartTime")]
        public DateTime StartTime { get; set; }

        [XmlElement("EndTime")]
        public DateTime EndTime { get; set; }

        [XmlElement("EmployeeId")]
        public Guid EmployeeId { get; set; }

        [XmlElement("Justification")]
        public string Justification { get; set; }

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
