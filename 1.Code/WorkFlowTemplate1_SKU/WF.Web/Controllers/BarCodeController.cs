using MVC4Pager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using WF.Model;

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
                new Model.BarCodeInfo() { Id = 1, BaeName = "测试1", BaeType = "类型1" }, 
                new Model.BarCodeInfo() { Id = 2, BaeName = "测试2", BaeType = "类型2" }, 
                new Model.BarCodeInfo() { Id = 3, BaeName = "测试3", BaeType = "类型3" }, 
                new Model.BarCodeInfo() { Id = 4, BaeName = "测试4", BaeType = "类型4" }
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

        //[ChildActionOnly]
        //public ActionResult BarCodeList() 
        //{
        //    return View();
        //}

    }
}
