using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Model.BarCode
{
    /// <summary>
    /// 详情信息
    /// </summary>
    public class SKUBarCodeDetailsInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BuCode { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string BarName { get; set; }
        /// <summary>
        /// 条形码主键
        /// </summary>
        public int RequestId { get; set; }
        /// <summary>
        /// 产品英文
        /// </summary>
        public string CodeNameEH { get; set; }
        /// <summary>
        /// 产品中文
        /// </summary>
        public string CodeNameCH { get; set; }
        /// <summary>
        /// 目标市场
        /// </summary>
        public string TargetId { get; set; }
        /// <summary>
        /// 工厂Id
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 产品分类Id
        /// </summary>
        public int BuCodeId { get; set; }
        /// <summary>
        /// 净含量
        /// </summary>
        public string SpecsName { get; set; }
        /// <summary>
        /// 运输单元
        /// </summary>
        public int UnitId { get; set; }
        /// <summary>
        /// 包装形式
        /// </summary>
        public string PackingName { get; set; }
        /// <summary>
        /// 系数
        /// </summary>
        public string RatioName { get; set; }
        /// <summary>
        /// 备案状态
        /// </summary>
        public int KeepType { get; set; }
        /// <summary>
        /// 申请原因
        /// </summary>
        public string RremarksName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }

    }
}
