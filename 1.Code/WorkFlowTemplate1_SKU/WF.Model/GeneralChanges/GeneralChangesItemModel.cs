using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WF.Model.GeneralChanges
{
    public class GeneralChangesItemModel
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Number")]
        public int Number { get; set; }

        [XmlElement("SKUCode")]
        public string SKUCode { get; set; }

        [XmlElement("SKUDescription")]
        public string SKUDescription { get; set; }

        [XmlElement("NetWeight")]
        public string NetWeight { get; set; }

        [XmlElement("Packing")]
        public string Packing { get; set; }

        [XmlElement("BarCode")]
        public string BarCode { get; set; }

        [XmlElement("Length")]
        public string Length { get; set; }

        [XmlElement("Width")]
        public string Width { get; set; }

        [XmlElement("Height")]
        public string Height { get; set; }
        [XmlElement("ShelfLife")]
        public string ShelfLife { get; set; }
        [XmlElement("GrossWeight")]
        public string GrossWeight { get; set; }

        [XmlElement("DataIsNew")]
        public int DataIsNew { get; set; }

        [XmlElement("CreatedBy")]
        public Guid? CreatedBy { get; set; }

        [XmlElement("CreatedTime")]
        public DateTime? CreatedTime { get; set; }

        [XmlElement("ModifiedBy")]
        public Guid? ModifiedBy { get; set; }

        [XmlElement("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [XmlElement("requestStates")]
        public string requestStates { get; set; }

        [XmlElement("Brand")]
        public string Brand { get; set; }
        [XmlElement("BUCode")]
        public string BUCode { get; set; }

        [XmlElement("BUName")]
        public string BUName { get; set; }

        [XmlElement("BusinessJustification")]
        public string BusinessJustification { get; set; }

        [XmlElement("BarCodeInfoType")]
        public int BarCodeInfoType { get; set; }
    }
}
