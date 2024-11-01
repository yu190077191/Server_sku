using System;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("ProjectRequirement")]
    public class ProjectRequirement
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("BusinessCaseOwnerName")]
        public string BusinessCaseOwnerName { get; set; }

        [XmlElement("E2ELeadName")]
        public string E2ELeadName { get; set; }

        [XmlElement("ProgramProjectManagerName")]
        public string ProgramProjectManagerName { get; set; }

        [XmlElement("ExecutiveSponsorName")]
        public string ExecutiveSponsorName { get; set; }

        [XmlElement("CurrencyName")]
        public string CurrencyName { get; set; }

        [XmlElement("AdditionalAreaName")]
        public string AdditionalAreaName { get; set; }

        [XmlElement("Market_OPCOName")]
        public string Market_OPCOName { get; set; }

        [XmlElement("DrivingBusinessName")]
        public string DrivingBusinessName { get; set; }

        [XmlElement("NBE_E2E_FlowName")]
        public string NBE_E2E_FlowName { get; set; }

        [XmlElement("WorkFlowTypeCode")]
        public string WorkFlowTypeCode { get; set; }

        [XmlElement("UserName")]
        public string UserName { get; set; }

        [XmlElement("Status")]
        public int Status { get; set; }

        [XmlElement("StatusName")]
        public string StatusName { get; set; }

        [XmlElement("InitiativeName")]
        public string InitiativeName { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("Scope")]
        public string Scope { get; set; }

        [XmlElement("GeneralBenefitsAndRiskOfNotDoingThisChange")]
        public string GeneralBenefitsAndRiskOfNotDoingThisChange { get; set; }

        [XmlElement("BusinessCaseOwner")]
        public Guid? BusinessCaseOwner { get; set; }

        [XmlElement("DesiredStartDate")]
        public DateTime? DesiredStartDate { get; set; }

        [XmlElement("DesiredClosureDate")]
        public DateTime? DesiredClosureDate { get; set; }

        [XmlElement("Market_OPCO")]
        public int? Market_OPCO { get; set; }

        [XmlElement("DrivingBusiness")]
        public int? DrivingBusiness { get; set; }

        [XmlElement("NBE_E2E_Flow")]
        public int? NBE_E2E_Flow { get; set; }

        [XmlElement("AdditionalArea")]
        public int? AdditionalArea { get; set; }

        [XmlElement("Currency")]
        public int? Currency { get; set; }

        [XmlElement("InitialCostEstimation")]
        public double? InitialCostEstimation { get; set; }

        public string Cost
        {
            get
            {
                if (InitialCostEstimation != null)
                {
                    return ((double)InitialCostEstimation).ToString("N0") + " (" + CurrencyName + ")";
                }

                return string.Empty;
            }
        }

        [XmlElement("InitialCapexEstimation")]
        public double? InitialCapexEstimation { get; set; }

        [XmlElement("InitialBenefitsEstimation")]
        public double? InitialBenefitsEstimation { get; set; }

        [XmlElement("ContributionKPI")]
        public string ContributionKPI { get; set; }

        [XmlElement("E2ELead")]
        public Guid? E2ELead { get; set; }

        [XmlElement("E2ELeadEndorsementDate")]
        public DateTime? E2ELeadEndorsementDate { get; set; }

        [XmlElement("ExecutiveSponsor")]
        public Guid? ExecutiveSponsor { get; set; }

        [XmlElement("ExecutiveSponsorEndorsementDate")]
        public DateTime? ExecutiveSponsorEndorsementDate { get; set; }

        [XmlElement("NPDIIdentifier")]
        public string NPDIIdentifier { get; set; }

        [XmlElement("Nestool_BR_ID")]
        public string Nestool_BR_ID { get; set; }

        [XmlElement("ProgramProjectManager")]
        public Guid? ProgramProjectManager { get; set; }

        [XmlElement("EstimationsGeneralNotes")]
        public string EstimationsGeneralNotes { get; set; }

        [XmlElement("Contact")]
        public string Contact { get; set; }

        [XmlElement("MarketLevel")]
        public string MarketLevel { get; set; }

        [XmlElement("Nestool_BR_ID_Status")]
        public string Nestool_BR_ID_Status { get; set; }

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
