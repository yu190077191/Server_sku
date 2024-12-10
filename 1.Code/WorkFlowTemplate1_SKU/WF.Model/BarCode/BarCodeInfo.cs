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
        [XmlElement("BarName")]
        public string BarName { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        [XmlElement("BaeType")]
        public string BarType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreteTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [XmlElement("UserId")]
        public Guid UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string DisplayName{ get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 申请id
        /// </summary>
        public int CodeId { get; set; }
    }
}
