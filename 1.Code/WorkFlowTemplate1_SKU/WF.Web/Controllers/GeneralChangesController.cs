using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WF.Model.SpecialApproval;
using WF.Model;
using WF.Model.GeneralChanges;
using System.Data;
 
using Newtonsoft.Json;
using WF.Framework.Helper;
using WF.DataAccess;
using WF.Framework;

namespace WF.Web.Controllers
{
    public class GeneralChangesController : Controller
    {
        //
        // GET: /GeneralChanges/

        public ActionResult Index(string id)
        {
            GeneralChangesModel generalChangesModel = new GeneralChangesModel();
            ViewBag.msg = "";
            ViewBag.id = id;
            ViewBag.requestStates = "-1";
            try
            {
                if (!string.IsNullOrEmpty(id) && id != "0")
                {
                    generalChangesModel.Id = Convert.ToInt32(id);
                    generalChangesModel.ListGeneralChangesItemModel = new List<GeneralChangesItemModel>();
                    generalChangesModel.ListGeneralChangesItemModel = GetModelByRequestId(id);
                    if (generalChangesModel.ListGeneralChangesItemModel.Count > 0)
                    {
                        generalChangesModel.BUCode = generalChangesModel.ListGeneralChangesItemModel.FirstOrDefault().BUCode;
                        generalChangesModel.BUName = generalChangesModel.ListGeneralChangesItemModel.FirstOrDefault().BUName;
                        generalChangesModel.BusinessJustification = generalChangesModel.ListGeneralChangesItemModel.FirstOrDefault().BusinessJustification;
                        ViewBag.requestStates = generalChangesModel.ListGeneralChangesItemModel.FirstOrDefault().requestStates;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex.Message;
            }
            ViewBag.SpecialApprovalModel = JsonConvert.SerializeObject(generalChangesModel);
            return View();
        }

        public ActionResult Edit(string id)
        {
            GeneralChangesModel generalChangesModel = new GeneralChangesModel();
            ViewBag.msg = "";
            ViewBag.id = id;
            ViewBag.requestStates = "-1";
            try
            {
                if (!string.IsNullOrEmpty(id) && id != "0")
                {
                    generalChangesModel.Id = Convert.ToInt32(id);
                    generalChangesModel.ListGeneralChangesItemModel = new List<GeneralChangesItemModel>();
                    generalChangesModel.ListGeneralChangesItemModel = GetModelByRequestId(id);
                    if (generalChangesModel.ListGeneralChangesItemModel.Count > 0)
                    {
                        generalChangesModel.BUCode = generalChangesModel.ListGeneralChangesItemModel.FirstOrDefault().BUCode;
                        generalChangesModel.BUName = generalChangesModel.ListGeneralChangesItemModel.FirstOrDefault().BUName;
                        generalChangesModel.BusinessJustification = generalChangesModel.ListGeneralChangesItemModel.FirstOrDefault().BusinessJustification;
                        ViewBag.requestStates = generalChangesModel.ListGeneralChangesItemModel.FirstOrDefault().requestStates;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex.Message;
            }
            ViewBag.SpecialApprovalModel = JsonConvert.SerializeObject(generalChangesModel);
            return View();
            
        }

        [HttpPost]
        public JsonResult SaveGeneralChanges(string jsonmodel)
        {
            DataResult dr = new DataResult();
            try
            {
                dr.Status = 0;
                dr.Message = "未提交数据";
                int intRequestId = 0;
                if (!string.IsNullOrEmpty(jsonmodel))
                {
                    var model = JsonHelper.DeserializeJson<GeneralChangesModel>(jsonmodel);
                    object requstId = model.Id;
                    //新数据--插入
                    if (model != null && model.Id == 0)
                    {
                        intRequestId = InsertData(model);
                    }
                    //更新主表 
                    else
                    {
                        UpdateData(model);
                        intRequestId = model.Id;
                    }

                    dr.Status = 1;
                    dr.Message = "提交成功!";
                    dr.Data = intRequestId;
                }
            }
            catch (Exception ex)
            {
                dr.Status = -1;
                dr.Message = ex.Message;
            }
            return Json(dr);
        }

        public int InsertData(GeneralChangesModel model)
        {
            //插入主表
            var sqlRequest = @"INSERT INTO [Request]([Type],[State],[CustomerNumber],[CustomerName],[RecordStatus],[CreatedBy],[CreatedTime])
                             VALUES ('BarCodeGeneralApp','" + model.State + "','" + model.BUCode + "','" + model.BUName + "','0','" + Operation.OperationBy + "','" + DateTime.Now + "')Select @@Identity";
            var requstId = BaseDao.ExecuteScalar(sqlRequest, null, CommandType.Text);
            int intRequestId = Convert.ToInt32(requstId);
            //插入详细表
            InsertDetailInfo(intRequestId, model.ListGeneralChangesItemModel);
            return intRequestId;
        }

        public void UpdateData(GeneralChangesModel model)
        {
            //更新主表
            string updateRequest = @"UPDATE [dbo].[Request]
                                     SET
                                      [State] = '" + model.State + "'"
                                     + " ,[ModifiedBy] = '" + Operation.OperationBy + "'"

                                     + " ,[CustomerNumber] = '" + model.BUCode + "'"
                                     + " ,[CustomerName] = '" + model.BUName + "'"

                                     + ",[ModifiedOn] = getdate() WHERE id = " + model.Id;

            BaseDao.ExecuteScalar(updateRequest, null, CommandType.Text);

            //删除详细表 , 重新插入
            //string delItemsql = "delete from SpecialApprovalItem where RequstId = " + model.Id;
            string delItemsql = " update  GeneralChangesModel set RecordStatus = 1  where RequstId = " + model.Id;
            BaseDao.ExecuteScalar(delItemsql, null, CommandType.Text);
            InsertDetailInfo(model.Id, model.ListGeneralChangesItemModel);
        }

        public void InsertDetailInfo(int requstId, List<GeneralChangesItemModel> model)
        {
            var sqlDetail = "";
            //插入对应子表
            foreach (var item in model)
            {
                sqlDetail += @"  INSERT INTO GeneralChangesItem (
                                  [RequstId]
                                 ,[Number]
                                 ,[SKUCode]
                                 ,[SKUDescription]
                                 ,[NetWeight]
                                 ,[Packing]
                                 ,[BarCode]
                                 ,[Length]
                                 ,[Width]
                                 ,[Height]
                                 ,[ShelfLife]
                                 ,[GrossWeight]
                                 ,[DataIsNew]
                                 ,[CreatedBy]
                                 ,[CreatedTime]
                                 ,[ModifiedBy]
                                 ,[ModifiedOn] 
                                 ,[Brand]
                                 ,[BUCode]
                                 ,[BUName] 
                                 ,[BusinessJustification] 
                                 ,[BarCodeInfoType] 
                                  ) 
                                VALUES("
                      + requstId
                      + ",'" + item.Number + "'"
                      + ",'" + item.SKUCode + "'"
                      + ",N'" + item.SKUDescription + "'"
                      + ",N'" + item.NetWeight + "'"
                      + ",N'" + item.Packing + "'"
                      + ",N'" + item.BarCode + "'"
                      + ",N'" + item.Length + "'"
                      + ",N'" + item.Width + "'"
                      + ",N'" + item.Height + "'"
                      + ",N'" + item.ShelfLife + "'"
                      + ",N'" + item.GrossWeight + "'"
                      + ",N'" + item.DataIsNew + "'"
                      + ",'" + Operation.OperationBy + "'"
                      + ",'" + DateTime.Now + "'"
                      + ",null"
                      + ",'" + DateTime.Now + "'"
                      + ",'" + item.Brand + "'"
                      + ",'" + item.BUCode + "'"
                      + ",'" + item.BUName + "'"
                      + ",N'" + item.BusinessJustification + "'"
                      + ",'" + item.BarCodeInfoType + "'" 
                      + ") ;";
            }
            BaseDao.ExecuteScalar(sqlDetail, null, CommandType.Text);
        }

        public List<GeneralChangesItemModel> GetModelByRequestId(string Id)
        {
            var dt = GetDateByRequestId(Id);
            List<GeneralChangesItemModel> listGeneralChangesItemModel = new List<GeneralChangesItemModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GeneralChangesItemModel generalChangesItemModel = new GeneralChangesItemModel();
                    //specialApprovalItem.Id = dt.Rows[i]["Number"] + "";
                    generalChangesItemModel.Id = Convert.ToInt32(dt.Rows[i]["Id"] + "");
                    generalChangesItemModel.Number = Convert.ToInt32(dt.Rows[i]["Number"] + "");
                    generalChangesItemModel.SKUCode = dt.Rows[i]["SKUCode"] + "";
                    generalChangesItemModel.SKUDescription = dt.Rows[i]["SKUDescription"] + "";
                    generalChangesItemModel.NetWeight = dt.Rows[i]["NetWeight"] + "";
                    generalChangesItemModel.Packing = dt.Rows[i]["Packing"] + "";
                    generalChangesItemModel.BarCode = dt.Rows[i]["BarCode"] + "";
                    generalChangesItemModel.Length = dt.Rows[i]["Length"] + "";
                    generalChangesItemModel.Width = dt.Rows[i]["Width"] + "";
                    generalChangesItemModel.Height = dt.Rows[i]["Height"] + "";
                    generalChangesItemModel.ShelfLife = dt.Rows[i]["ShelfLife"] + "";
                    generalChangesItemModel.GrossWeight = dt.Rows[i]["GrossWeight"] + "";
                    generalChangesItemModel.DataIsNew = (dt.Rows[i]["DataIsNew"] + "").ToLower() == "true" ? 1 : 0;
                    generalChangesItemModel.requestStates = dt.Rows[i]["requestStates"] + "";

                    generalChangesItemModel.Brand = dt.Rows[i]["Brand"] + "";
                    generalChangesItemModel.BUCode = dt.Rows[i]["BUCode"] + "";
                    generalChangesItemModel.BUName = dt.Rows[i]["BUName"] + "";
                    generalChangesItemModel.BusinessJustification = dt.Rows[i]["BusinessJustification"] + "";
                    generalChangesItemModel.BarCodeInfoType = dt.Rows[i]["BarCodeInfoType"] + "" ==""?-1:Convert.ToInt32(dt.Rows[i]["BarCodeInfoType"]);

                    listGeneralChangesItemModel.Add(generalChangesItemModel);
                }
            }
            return listGeneralChangesItemModel;
        }


        public DataTable GetDateByRequestId(string Id)
        {
            DataTable dt = new DataTable();

            string sql = @"select s.*,r.State as requestStates from  Request  r  
                       join  GeneralChangesItem s
                       on r.id = s.RequstId  and r.RecordStatus = 0  and  s.RecordStatus = 0
                       where  r.id  = " + Id + " order by s.number";

            dt = BaseDao.ExecuteDataSet(sql, null, CommandType.Text).Tables[0];

            return dt;
        }
    }
}
