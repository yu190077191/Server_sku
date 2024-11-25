using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Model.BarCode
{
    /// <summary>
    /// 备案信息
    /// </summary>
    public class SKUKeepInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 详细信息主键
        /// </summary>
        public int BuDetailId { get; set; }
        /// <summary>
        /// 备案信息
        /// </summary>
        public string KeepText { get; set; }

    }
}
