using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("DepartmentRole")]
    public class DepartmentRole
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("InstanceId")]
        public int InstanceId { get; set; }

        [XmlElement("EmployeeId")]
        public Guid EmployeeId { get; set; }

        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlElement("BUDepartmentId")]
        public int BUDepartmentId { get; set; }

        [XmlElement("BUName")]
        public string BUName { get; set; }

        [XmlElement("RoleId")]
        public int RoleId { get; set; }

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

        [XmlElement("AccountName")]
        public string AccountName { get; set; }

        [XmlElement("DisplayName")]
        public string DisplayName { get; set; }

        [XmlElement("CostCenter")]
        public string CostCenter { get; set; }

        [XmlElement("RoleName")]
        public string RoleName { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("PARTITIONRowNumber")]
        public int PARTITIONRowNumber { get; set; }

        [XmlElement("WwwHomepage")]
        public string WwwHomepage { get; set; }

        [XmlElement("Title")]
        public string Title { get; set; }

        [XmlElement("OriginalTitle")]
        public string OriginalTitle { get; set; }

        [XmlElement("Department")]
        public string Department { get; set; }

        [XmlElement("OriginalDepartment")]
        public string OriginalDepartment { get; set; }
    }
}
