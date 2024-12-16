using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WF.Model.SpecialApproval;

namespace WF.Model.GeneralChanges
{
    [Serializable]
    [XmlType("GeneralChangesModel")]
    public class GeneralChangesModel
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

        [XmlElement("BUCode")]
        public string BUCode { get; set; }

        [XmlElement("BUName")]
        public string BUName { get; set; }

        [XmlElement("BusinessJustification")]
        public string BusinessJustification { get; set; }

        [XmlElement("GeneralChangesItemModel")]
        public List<GeneralChangesItemModel> ListGeneralChangesItemModel { get; set; }
    }
}
