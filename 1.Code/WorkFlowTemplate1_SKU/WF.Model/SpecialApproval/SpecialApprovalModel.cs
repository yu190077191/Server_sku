using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WF.Model.SpecialApproval
{
    [Serializable]
    [XmlType("SpecialApprovalModel")]
    public class SpecialApprovalModel
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("State")]
        public int State { get; set; }

        [XmlElement("CustomerNumber")]
        public string CustomerNumber { get; set; }

        [XmlElement("CustomerName")]
        public string CustomerName { get; set; }

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

        [XmlElement("ListSpecialApprovalItemModel")]
        public List<SpecialApprovalItemModel> ListSpecialApprovalItemModel { get; set; }
    }
}
