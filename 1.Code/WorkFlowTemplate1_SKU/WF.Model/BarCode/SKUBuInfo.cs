using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Model.BarCode
{
    /// <summary>
    /// 产品分类基础信息
    /// </summary>
    public class SKUBuInfo
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public int BuCode { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string BuName { get; set; }
        /// <summary>
        /// 分类代码
        /// </summary>
        public string BuGPC { get; set; }
        /// <summary>
        /// 节点id
        /// </summary>
        public int ParenId { get; set; }

    }
}
