using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("Employee")]
    public class Employee
    {
        [XmlElement("Id")]
        public Guid Id { get; set; }

        [XmlElement("Domain")]
        public string Domain { get; set; }

        [XmlElement("AccountName")]
        public string AccountName { get; set; }

        [XmlElement("WhenCreated")]
        public DateTime? WhenCreated { get; set; }

        [XmlElement("Mail")]
        public string Mail { get; set; }

        [XmlElement("WwwHomepage")]
        public string WwwHomepage { get; set; }

        [XmlElement("Company")]
        public string Company { get; set; }

        [XmlElement("Department")]
        public string Department { get; set; }

        [XmlElement("Title")]
        public string Title { get; set; }

        [XmlElement("Givenname")]
        public string Givenname { get; set; }

        [XmlElement("DisplayName")]
        public string DisplayName { get; set; }

        [XmlElement("SupervisorDisplayName")]
        public string SupervisorDisplayName { get; set; }

        [XmlElement("SupervisorEmployeeId")]
        public Guid? SupervisorEmployeeId { get; set; }

        [XmlElement("PersonnelNumber")]
        public string PersonnelNumber { get; set; }

        [XmlElement("OrganizationUnit")]
        public string OrganizationUnit { get; set; }

        [XmlElement("PersonnelAreaID")]
        public string PersonnelAreaID { get; set; }

        [XmlElement("PersonnelArea")]
        public string PersonnelArea { get; set; }

        [XmlElement("PersonnelSubArea")]
        public string PersonnelSubArea { get; set; }

        [XmlElement("EmployeeSubgroup")]
        public string EmployeeSubgroup { get; set; }

        [XmlElement("CostCenter")]
        public string CostCenter { get; set; }

        [XmlElement("Zone")]
        public string Zone { get; set; }

        [XmlElement("Country")]
        public string Country { get; set; }

        [XmlElement("Market")]
        public string Market { get; set; }

        [XmlElement("Business")]
        public string Business { get; set; }

        [XmlElement("Community")]
        public string Community { get; set; }

        [XmlElement("JobFamily")]
        public string JobFamily { get; set; }

        [XmlElement("JobSubFamily")]
        public string JobSubFamily { get; set; }

        [XmlElement("StreetAddress")]
        public string StreetAddress { get; set; }

        [XmlElement("Usncreated")]
        public int? Usncreated { get; set; }

        [XmlElement("UpdateTime")]
        public DateTime? UpdateTime { get; set; }

        [XmlElement("ProcessStatus")]
        public int ProcessStatus { get; set; }

        [XmlElement("RecordStatus")]
        public int RecordStatus { get; set; }

        [XmlElement("Note")]
        public string Note { get; set; }

        [XmlElement("SSOGeneratedLoginId")]
        public Guid? SSOGeneratedLoginId { get; set; }

        [XmlElement("TitleId")]
        public int TitleId { get; set; }

        [XmlElement("Sequence")]
        public double Sequence { get; set; }

        [XmlElement("IsManuallyAdded")]
        public bool IsManuallyAdded { get; set; }

        [XmlElement("FactoryName")]
        public string FactoryName { get; set; }
    }
}