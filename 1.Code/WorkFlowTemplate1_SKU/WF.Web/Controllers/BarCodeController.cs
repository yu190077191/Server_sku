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
using WF.Framework.Helper;
using WF.Framework;
using System.Reflection.Emit;
using System.Web.Helpers;
using System.Web.Http.OData.Query;
using WF.BusinessRule;
using System.Threading.Tasks;

namespace WF.Web.Controllers
{
    public class BarCodeController : Controller
    {
        WorkFlowAPIController api = new WorkFlowAPIController();
        //
        // GET: /BarCode/
        /// <summary>
        /// 条码首页
        /// </summary>
        /// <returns></returns>
        public ActionResult BarCodeInfo(int? pageIndex = 1,int? selectElement1 = null, int? selectElement2 = null)
        {
            string sqlInfo = "select a.Id,b.DisplayName,a.CreatedTime,a.State,a.type,a.CustomerName,a.CustomerNumber from Request a " +
                "inner join [Employee] b on a.CreatedBy = b.id where (a.type = 'BarCodeNew' or a.type = 'BarCodeSpecialApp' or a.type = 'BarCodeGeneralApp' ) and a.RecordStatus = 0";
           
            var db_Info = BaseDao.ExecuteDataSet(sqlInfo, null, CommandType.Text).Tables[0].Rows;
            List<BarCodeInfo> list_info = new List<BarCodeInfo>();
            int index = 1;
            foreach (DataRow item in db_Info)
            {
                list_info.Add(
                    new Model.BarCodeInfo() {
                        Id = index++,
                        CodeId = Convert.ToInt32(item["Id"]),
                        DisplayName = item["DisplayName"].ToString(),
                        BarName = item["type"].ToString().Equals("BarCodeNew") ? "条码申请" : (item["type"].ToString().Equals("BarCodeSpecialApp") ? "条码特批" : "条码变更"),
                        CreteTime =Convert.ToDateTime(item["CreatedTime"]).ToString("yyyy-MM-dd"),
                        State =Convert.ToInt32(item["State"]),
                        BarType = item["CustomerName"].ToString() }
                );
            }

            //if (selectElement1 > -1 )
            //{
            //    sqlInfo += " and a.State = "+ selectElement1 + "";
            //}
            if (selectElement2 > -1)
            {
                list_info = list_info.Where(o => o.State == selectElement2).ToList();
            }

            var model = new PagedList<BarCodeInfo>(list_info.OrderByDescending(o=>o.Id).ToList(), Convert.ToInt32(pageIndex), 10);
            if (Request.IsAjaxRequest())
            {
                return PartialView("BarCodeList", model);
            }
            else 
            {

                return View(model);
            }
        }


        /// <summary>
        /// 编辑或者修改
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public ActionResult BarCodeEdit(int? codeId,string barName)
        {
            ViewBag.BuInfoList = JsonConvert.SerializeObject(GetBuInfo(0));
            //添加
            if (codeId <= 0)
            {
                return View();
            }
            else //修改
            {
                if (!string.IsNullOrEmpty(barName) && barName.Equals("条码申请"))
                {
                    List<SKUBarCodeDetailsInfo> DetailsInfo = new List<SKUBarCodeDetailsInfo>();
                    //定义查询子数据
                    var select_sql = "select b.Id as DetailsId,* from Request a left " +
                        "join SKUBarCodeDetailsInfo b on a.id = b.RequestId " +
                        "where a.Type = 'BarCodeNew' and a.RecordStatus = 0 and b.RecordStatus = 0 and a.id = " + codeId + "";

                    var db_Infos = BaseDao.ExecuteDataSet(select_sql, null, CommandType.Text);

                    foreach (DataRow item in db_Infos.Tables[0].Rows)
                    {
                        DetailsInfo.Add(new SKUBarCodeDetailsInfo()
                        {
                            Id = Convert.ToInt32(item["DetailsId"]), //详情id
                            BuCode = item["CustomerNumber"].ToString(), // buID
                            BarName = item["CustomerName"].ToString(), //品牌
                            CodeNameEH = item["CodeNameEH"].ToString(), // 英文
                            CodeNameCH = item["CodeNameCH"].ToString(), // 英文
                            TargetId = item["TargetId"].ToString(), // 市场
                            CompanyId = Convert.ToInt32(item["CompanyId"]), // 工厂
                            BuCodeId = Convert.ToInt32(item["BuCodeId"]), // 分类
                            SpecsName = item["SpecsName"].ToString(), // 净含量
                            UnitId = Convert.ToInt32(item["UnitId"]), // 运输
                            PackingName = item["PackingName"].ToString(), // 包装形式
                            RatioName = item["RatioName"].ToString(), // 系数
                            RremarksName = item["RremarksName"].ToString(), // 原因
                            KeepType = Convert.ToInt32(item["KeepType"]), //备案状态
                            Length = !string.IsNullOrEmpty(item["Length"].ToString()) ? float.Parse(item["Length"].ToString()) : 0, //长
                            Width = !string.IsNullOrEmpty(item["Width"].ToString()) ? float.Parse(item["Width"].ToString()) : 0, //宽
                            Height = !string.IsNullOrEmpty(item["Height"].ToString()) ? float.Parse(item["Height"].ToString()) : 0, //高
                            BarCodeNum = item["BarCodeNum"].ToString(), //条形码
                            State = Convert.ToInt32(item["State"]),
                            RequestId = Convert.ToInt32(item["RequestId"])
                        });
                    }
                    return View(DetailsInfo);
                }
                else if (!string.IsNullOrEmpty(barName) && barName.Equals("条码特批"))
                {
                    return View();
                }
                return View();
            }
        }

        /// <summary>
        /// 备案方法
        /// </summary>
        /// <param name="codeId"></param>
        /// <returns></returns>
        public ActionResult KeepInfo(int codeId)
        {
            //var jsonInfo = JsonHelper.DeserializeJson<List<SKUBarCodeDetailsInfo>>(json);
            SKUFileUpload info = new SKUFileUpload();
            string sql = "select * from SKUBarCodeDetailsInfo where id = " + codeId + "";
            var db_Infos = BaseDao.ExecuteDataSet(sql, null, CommandType.Text).Tables[0];
            foreach (DataRow item in db_Infos.Rows)
            {
                info.CodeDetailsId = item.Field<int>("Id");
                info.RequestId = item.Field<int>("RequestId");
            }
            return View(info);
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(SKUFileUpload model)
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
        public int SKUBucodeCreateInfo(string json,int state)
        {
            try
            {
                var jsonInfo = JsonHelper.DeserializeJson<List<SKUBarCodeDetailsInfo>>(json);
                int index = 0;
                if (jsonInfo.Count > 0)
                {
                    //插入[Request]表中进行审批或暂存
                    var sqlRequest = @"INSERT INTO [Request]([Type],[State],[CustomerNumber],[CustomerName],[RecordStatus],[CreatedBy],[CreatedTime])
                        VALUES ('BarCodeNew'," + state + "," + jsonInfo.First().BuCode + ",N'" + jsonInfo.First().BarName + "'," +
                            "0,'" + Operation.OperationBy + "','" + DateTime.Now + "')Select @@Identity";
                    var requstId = BaseDao.ExecuteScalar(sqlRequest, null, CommandType.Text);
                    if (Convert.ToInt32(requstId) > 0)
                    {
                        //插入对应子表
                        foreach (var item in jsonInfo)
                        {
                            var sqlDetail = @"INSERT INTO SKUBarCodeDetailsInfo ([RequestId],[CodeNameEH],[CodeNameCH],[TargetId],[CompanyId],[BuCodeId],[SpecsName],[UnitId],[PackingName],[RatioName],[RremarksName] ,[KeepType],[RecordStatus]) 
                                VALUES(" + requstId + ",N'" + item.CodeNameEH + "',N'" + item.CodeNameCH + "'" +
                                ",N'" + item.TargetId + "'," + item.CompanyId + "," + item.BuCodeId + "" +
                                ",N'" + item.SpecsName + "'," + item.UnitId + ",N'" + item.PackingName + "'" +
                                ",N'" + item.RatioName + "','" + item.RremarksName + "',0,0)Select @@Identity";
                            index += Convert.ToInt32(BaseDao.ExecuteScalar(sqlDetail, null, CommandType.Text));
                        }
                    }
                }
                return index;
            }
            catch (Exception)
            {
                throw;
            }
   
        }

        /// <summary>
        /// 获取相关信息
        /// </summary>
        /// <param name="CodeId"></param>
        /// <returns></returns>
        public List<SKUBarCodeDetailsInfo> GetSKUBarCodeDetailsInfo(int CodeId)
        {
            try
            {
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="index">详情id</param>
        /// <param name="RequestId">RequestId</param>
        /// <returns></returns>
        public int DeleteDetailsInfo(int index,int requestId)
        {
            try
            {
                string sql = string.Empty;
                sql = "update SKUBarCodeDetailsInfo set RecordStatus = 1 where id = " + index + "";
                var vIndex =  BaseDao.ExecuteNonQuery(sql, null, CommandType.Text);
                if (vIndex > 0)
                {
                    sql = "select * from SKUBarCodeDetailsInfo where RequestId = "+ requestId + " and RecordStatus = 0";
                    var countInfo = Convert.ToInt32(BaseDao.ExecuteDataSet(sql, null, CommandType.Text).Tables[0].Rows.Count);

                    if (countInfo == 0)
                    {
                        sql = "update Request set RecordStatus = 1 where id = " + requestId + "";
                        BaseDao.ExecuteNonQuery(sql, null, CommandType.Text);
                    }
                }

                return vIndex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="buCodeId">buId</param>
        /// <param name="btType">逻辑类型</param>
        /// <returns></returns>
        public int UpdateStateInfo(int buCodeId,int btType)
        {
            try
            {
                string sql = string.Empty;
                switch (btType)
                {
                    case 5: //撤回
                        sql = "update Request set State = "+ btType+ "  where Id = " + buCodeId + "";
                        break;
                    case 7: //备案
                        sql = "update Request set State = " + btType + "  where Id = " + buCodeId + "";
                        break;
                    default:
                        break;
                }
                return BaseDao.ExecuteNonQuery(sql, null, CommandType.Text);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 生成条形码
        /// </summary>
        /// <param name="codeType"></param>
        public void SetBarCodeNumber(string codeType)
        {
            var str = CommonRule.SetBarCodeNumber(19585,codeType);
        }
    }
}
