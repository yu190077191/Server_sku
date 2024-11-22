using MVC4Pager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using WF.Model;
using WF.DataAccess;
using WF.Model.BarCode;
using Newtonsoft.Json;

namespace WF.Web.Controllers
{
    public class BarCodeController : Controller
    {
        //
        // GET: /BarCode/
        /// <summary>
        /// 条码首页
        /// </summary>
        /// <returns></returns>
        public ActionResult BarCodeInfo()
        {
            List<BarCodeInfo> list_info = new List<BarCodeInfo>() 
            { 
                new Model.BarCodeInfo() { Id = 1, BaeName = "测试1", BaeType = "类型1" }
            };
            var model = new PagedList<BarCodeInfo>(list_info, 1,10);

            return View(model);
        }


        /// <summary>
        /// 编辑或者修改
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public ActionResult BarCodeEdit(int? codeId)
        {
            ViewBag.BuInfoList = JsonConvert.SerializeObject( GetBuInfo(0));
            //添加
            if (codeId <= 0)
            {
                return View();
            }
            else //修改
            {
                var barInfo = new Model.BarCodeInfo() { Id = 1, BaeName = "测试1", BaeType = "类型1" };
                return View(barInfo);
            }
        }

        /// <summary>
        /// 备案方法
        /// </summary>
        /// <param name="codeId"></param>
        /// <returns></returns>
        public ActionResult KeepInfo(int? codeId)
        {
            return View();
        }

        /// <summary>
        /// 获取基础信息
        /// </summary>
        public List<SKUBuInfo> GetBuInfo(int parenId)
        {
            List<SKUBuInfo> sku_buInfo = new List<SKUBuInfo>();
            var db_Infos = BaseDao.ExecuteDataSet("select * from SKUBuInfo where parenId = "+ parenId + "", null, CommandType.Text);
            foreach (DataRow item in db_Infos.Tables[0].Rows)
            {
                sku_buInfo.Add(new SKUBuInfo()
                {
                    BuCode = !string.IsNullOrEmpty(item["BuCode"].ToString()) ? Convert.ToInt32(item["BuCode"]) : 0,
                    BuName = item["BuName"].ToString().Trim(),
                    ParenId = Convert.ToInt32(item["parenId"]),
                    BuGPC = !string.IsNullOrEmpty(item["BuGPC"].ToString()) ? item["BuGPC"].ToString() : ""
                });
            }

            return sku_buInfo;

        }

        /// <summary>
        /// 请求产品分类
        /// </summary>
        /// <param name="parenId"></param>
        /// <returns></returns>
        public JsonResult GetListInfo(int parenId)
        {
            DataResult result = new DataResult();
            return Json(result.Data = GetBuInfo(parenId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取公司数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCompanyInfo()
        {
            List<SKUCompany> sku_buInfo = new List<SKUCompany>();
            var db_Info = BaseDao.ExecuteDataSet("select * from SKUCompany", null, CommandType.Text).Tables[0].Rows;
            foreach (DataRow item in db_Info)
            {
                sku_buInfo.Add(new SKUCompany() { 
                    Id = Convert.ToInt32(item["Id"]),
                    CompanyName = item["CompanyName"].ToString().Trim(),
                });
            }
            DataResult result = new DataResult();
            return Json(result.Data = sku_buInfo, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 插入申请数据
        /// </summary>
        /// <returns></returns>
        public int SKUBucodeCreateInfo()
        {
            return 0;
        }

    }
}
