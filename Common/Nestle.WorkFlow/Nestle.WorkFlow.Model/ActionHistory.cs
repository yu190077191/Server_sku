using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nestle.WorkFlow.Model
{
    [Serializable]
    [XmlType("ActionHistory")]
    public class ActionHistory
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("WorkFlowPatternEventContainerId")]
        public int WorkFlowPatternEventContainerId { get; set; }

        [XmlElement("Sequence")]
        public int Sequence { get; set; }

        [XmlElement("StepName")]
        public string StepName { get; set; }

        [XmlElement("EmployeeId")]
        public Guid? EmployeeId { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Title")]
        public string Title { get; set; }

        [XmlElement("Action")]
        public string Action { get; set; }

        [XmlElement("Time")]
        public DateTime? Time { get; set; }

        [XmlElement("Remark")]
        public string Remark { get; set; }

        [XmlElement("IsCurrent")]
        public bool IsCurrent { get; set; }

        [XmlElement("IsManuallyAdded")]
        public bool IsManuallyAdded { get; set; }

        [XmlElement("AddedByWho")]
        public string AddedByWho { get; set; }

        [XmlElement("ApprovalXml")]
        public string ApprovalXml { get; set; }

        [XmlElement("Note")]
        public string Note { get; set; }

        [XmlElement("RowNumber")]
        public int RowNumber { get; set; }

        [XmlElement("IsTeamReview")]
        public bool IsTeamReview { get; set; }

        [XmlElement("CheckResult")]
        public string CheckResult { get; set; }
    }
}
