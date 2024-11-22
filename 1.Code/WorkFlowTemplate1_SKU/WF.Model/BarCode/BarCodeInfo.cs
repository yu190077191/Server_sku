using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WF.Model
{
    [Serializable]
    [XmlType("BarCodeInfo")]
    public class BarCodeInfo
    {
        [XmlElement("Id")]
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [XmlElement("BaeName")]
        public string BaeName { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        [XmlElement("BaeType")]
        public string BaeType { get; set; }

    }
}
