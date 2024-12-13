﻿using Microsoft.Win32;
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
        public string BuCode { get; set; }
        public string BarName { get; set; }
        public int Id { get; set; }
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
        /// <summary>
        /// 条形码号码
        /// </summary>
        public string BarCodeNum { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public float Length { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public float Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public float Height { get; set; }
        /// <summary>
        /// number
        /// </summary>
        public int Number { get; set; }

    }
}
