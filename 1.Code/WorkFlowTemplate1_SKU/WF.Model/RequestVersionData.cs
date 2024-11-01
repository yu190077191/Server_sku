using Newtonsoft.Json.Linq;

namespace WF.Model
{
    public class RequestVersionData
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public string MaterialType { get; set; }
        public string UOMofBUN { get; set; }
        public string GlobalAttribute3 { get; set; }
        public string BaseProduct { get; set; }
        public string BusinessGroup { get; set; }
        public string UOMOfVolumeInCS { get; set; }
        public string ChineseDescription { get; set; }
        public string MaterialGroup { get; set; }
        public string ProductHierarchy { get; set; }
        public string MaterialGroup1 { get; set; }
        public string MaterialDescription { get; set; }
        public string RangeBrand { get; set; }
        public string ParentCode { get; set; }
        public string SalesUnit { get; set; }
        public string BAI { get; set; }
        public string CorporateBrand { get; set; }
        public string BrandDenomination { get; set; }
        public string MaterialNumber { get; set; }
        public string MaterialGrp4 { get; set; }
        public string MaterialGrp3 { get; set; }
        public string SalesOrg1 { get; set; }
        public string DeliveringPlant1 { get; set; }
        public string DistributionChannel1 { get; set; }
        public string SalesOrg2 { get; set; }
        public string DeliveringPlant2 { get; set; }
        public string DistributionChannel2 { get; set; }
        public string SalesOrg3 { get; set; }
        public string DeliveringPlant3 { get; set; }
        public string DistributionChannel3 { get; set; }
        public string dataInfo { get; set; }
    }

    public class MG_MG1_PHMap 
    {
        public string ControlType { get; set; }
        public string ShowValue { get; set; }
        public string ShowDetails { get; set; }
    }
}
