using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.DataAccess
{
    public class ProjectRequirementDao : BaseDao
    {
        public static int ChangeE2E(int id, int e2e)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@id", id));
            sqlParameters.Add(new SqlParameter("@e2e", e2e));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            return ExecuteNonQuery("[dbo].[ChangeE2E]", sqlParameters.ToArray());
        }

        public static int CreateId(Guid Temp_ImportedData_Id)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Temp_ImportedData_Id", Temp_ImportedData_Id));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("[dbo].[CreateId]", sqlParameters.ToArray());
            return (int)id.Value;
        }
        public static int UpdateStatus(int requestVersionId, int status)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", requestVersionId));
            sqlParameters.Add(new SqlParameter("@Status", status));
            sqlParameters.Add(new SqlParameter("@OperationBy", Operation.OperationBy));
            return ExecuteNonQuery("UpdateRequestStatus", sqlParameters.ToArray());
        }

        public static int InsertProjectRequirement(WF.Model.ProjectRequirement model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Status", model.Status));
            sqlParameters.Add(new SqlParameter("@InitiativeName", model.InitiativeName));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@Scope", model.Scope));
            sqlParameters.Add(new SqlParameter("@GeneralBenefitsAndRiskOfNotDoingThisChange", model.GeneralBenefitsAndRiskOfNotDoingThisChange));
            sqlParameters.Add(new SqlParameter("@BusinessCaseOwner", model.BusinessCaseOwner));
            sqlParameters.Add(new SqlParameter("@DesiredStartDate", model.DesiredStartDate));
            sqlParameters.Add(new SqlParameter("@DesiredClosureDate", model.DesiredClosureDate));
            sqlParameters.Add(new SqlParameter("@Market_OPCO", model.Market_OPCO));
            sqlParameters.Add(new SqlParameter("@DrivingBusiness", model.DrivingBusiness));
            sqlParameters.Add(new SqlParameter("@NBE_E2E_Flow", model.NBE_E2E_Flow));
            sqlParameters.Add(new SqlParameter("@AdditionalArea", model.AdditionalArea));
            sqlParameters.Add(new SqlParameter("@Currency", model.Currency));
            sqlParameters.Add(new SqlParameter("@InitialCostEstimation", model.InitialCostEstimation));
            sqlParameters.Add(new SqlParameter("@InitialCapexEstimation", model.InitialCapexEstimation));
            sqlParameters.Add(new SqlParameter("@InitialBenefitsEstimation", model.InitialBenefitsEstimation));
            sqlParameters.Add(new SqlParameter("@ContributionKPI", model.ContributionKPI));
            sqlParameters.Add(new SqlParameter("@E2ELead", model.E2ELead));
            sqlParameters.Add(new SqlParameter("@E2ELeadEndorsementDate", model.E2ELeadEndorsementDate));
            sqlParameters.Add(new SqlParameter("@ExecutiveSponsor", model.ExecutiveSponsor));
            sqlParameters.Add(new SqlParameter("@ExecutiveSponsorEndorsementDate", model.ExecutiveSponsorEndorsementDate));
            sqlParameters.Add(new SqlParameter("@NPDIIdentifier", model.NPDIIdentifier));
            sqlParameters.Add(new SqlParameter("@Nestool_BR_ID", model.Nestool_BR_ID));
            sqlParameters.Add(new SqlParameter("@ProgramProjectManager", model.ProgramProjectManager));
            sqlParameters.Add(new SqlParameter("@EstimationsGeneralNotes", model.EstimationsGeneralNotes));
            sqlParameters.Add(new SqlParameter("@Contact", model.Contact));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            SqlParameter id = new SqlParameter("@id", System.Data.SqlDbType.Int, 4);
            id.Direction = System.Data.ParameterDirection.Output;
            sqlParameters.Add(id);
            ExecuteNonQuery("InsertProjectRequirementProc", sqlParameters.ToArray());
            return (int)id.Value;
        }

        public static int UpdateProjectRequirement(WF.Model.ProjectRequirement model)
        {
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", model.Id));
            sqlParameters.Add(new SqlParameter("@Status", model.Status));
            sqlParameters.Add(new SqlParameter("@InitiativeName", model.InitiativeName));
            sqlParameters.Add(new SqlParameter("@Description", model.Description));
            sqlParameters.Add(new SqlParameter("@Scope", model.Scope));
            sqlParameters.Add(new SqlParameter("@GeneralBenefitsAndRiskOfNotDoingThisChange", model.GeneralBenefitsAndRiskOfNotDoingThisChange));
            sqlParameters.Add(new SqlParameter("@BusinessCaseOwner", model.BusinessCaseOwner));
            sqlParameters.Add(new SqlParameter("@DesiredStartDate", model.DesiredStartDate));
            sqlParameters.Add(new SqlParameter("@DesiredClosureDate", model.DesiredClosureDate));
            sqlParameters.Add(new SqlParameter("@Market_OPCO", model.Market_OPCO));
            sqlParameters.Add(new SqlParameter("@DrivingBusiness", model.DrivingBusiness));
            sqlParameters.Add(new SqlParameter("@NBE_E2E_Flow", model.NBE_E2E_Flow));
            sqlParameters.Add(new SqlParameter("@AdditionalArea", model.AdditionalArea));
            sqlParameters.Add(new SqlParameter("@Currency", model.Currency));
            sqlParameters.Add(new SqlParameter("@InitialCostEstimation", model.InitialCostEstimation));
            sqlParameters.Add(new SqlParameter("@InitialCapexEstimation", model.InitialCapexEstimation));
            sqlParameters.Add(new SqlParameter("@InitialBenefitsEstimation", model.InitialBenefitsEstimation));
            sqlParameters.Add(new SqlParameter("@ContributionKPI", model.ContributionKPI));
            sqlParameters.Add(new SqlParameter("@E2ELead", model.E2ELead));
            sqlParameters.Add(new SqlParameter("@E2ELeadEndorsementDate", model.E2ELeadEndorsementDate));
            sqlParameters.Add(new SqlParameter("@ExecutiveSponsor", model.ExecutiveSponsor));
            sqlParameters.Add(new SqlParameter("@ExecutiveSponsorEndorsementDate", model.ExecutiveSponsorEndorsementDate));
            sqlParameters.Add(new SqlParameter("@NPDIIdentifier", model.NPDIIdentifier));
            sqlParameters.Add(new SqlParameter("@Nestool_BR_ID", model.Nestool_BR_ID));
            sqlParameters.Add(new SqlParameter("@ProgramProjectManager", model.ProgramProjectManager));
            sqlParameters.Add(new SqlParameter("@EstimationsGeneralNotes", model.EstimationsGeneralNotes));
            sqlParameters.Add(new SqlParameter("@Contact", model.Contact));
            sqlParameters.Add(new SqlParameter("@RecordStatus", model.RecordStatus));
            sqlParameters.Add(new SqlParameter("@CreatedBy", model.CreatedBy));
            sqlParameters.Add(new SqlParameter("@CreatedTime", model.CreatedTime));
            sqlParameters.Add(new SqlParameter("@ModifiedBy", Operation.OperationBy));
            sqlParameters.Add(new SqlParameter("@ModifiedOn", model.ModifiedOn));
            return ExecuteNonQuery("UpdateProjectRequirementProc", sqlParameters.ToArray());
        }

        public static List<WF.Model.ProjectRequirement> GetProjectRequirement(string procedureName, SqlParameter[] sqlParamters)
        {
            List<WF.Model.ProjectRequirement> li = new List<WF.Model.ProjectRequirement>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//ProjectRequirement");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.ProjectRequirement>(node.OuterXml));
                    }
                }
            }

            return li;
        }


        public static QueryResult<WF.Model.ProjectRequirement> GetProjectRequirementResult(string procedureName, SqlParameter[] sqlParamters)
        {
            QueryResult<WF.Model.ProjectRequirement> result = new QueryResult<WF.Model.ProjectRequirement>();
            List<WF.Model.ProjectRequirement> li = new List<WF.Model.ProjectRequirement>();

            var xmlString = ExecuteXmlReader(procedureName, sqlParamters);
            if (!string.IsNullOrEmpty(xmlString))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNodeList nodeList = xmlDocument.FirstChild.SelectNodes("//ProjectRequirement");
                    foreach (XmlNode node in nodeList)
                    {
                        li.Add(SerializationHelper.Deserialize<WF.Model.ProjectRequirement>(node.OuterXml));
                    }

                    result.DataList = li;
                    XmlNode xmlNode = xmlDocument.FirstChild.SelectSingleNode("//TotalCount");
                    if (xmlNode != null)
                    {
                        result.Count = Convert.ToInt32(xmlNode.InnerText, Constants.CurrentCulture);
                    }
                }
            }

            return result;
        }

    }
}
