using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Model.BarCode
{
    public class SKUFileUpload
    {
        /// <summary>
        /// SKUId
        /// </summary>
        public int RequestId { get; set; }
        /// <summary>
        /// 详情Id
        /// </summary>
        public int CodeDetailsId { get; set; }

        /// <summary>
        /// 业务标识
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get;set; }

        /// <summary>
        /// 上市时间
        /// </summary>
        public string SubCode { get; set; }

        /// <summary>
        /// 页面类型
        /// </summary>
        public string VType { get;set; }



    }
}
