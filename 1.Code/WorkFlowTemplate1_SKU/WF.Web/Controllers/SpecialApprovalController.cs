using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WF.DataAccess;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;
using WF.Model.SpecialApproval;

namespace WF.Web.Controllers
{
    public class SpecialApprovalController : Controller
    {
        public ActionResult Index(string id)
        {
            SpecialApprovalModel specialApproval = new SpecialApprovalModel();
            ViewBag.msg = "";
            ViewBag.id = id;
            ViewBag.requestStates = "-1";
            try
            {
                if (!string.IsNullOrEmpty(id) && id != "0")
                {
                    specialApproval.Id = Convert.ToInt32(id);
                    specialApproval.ListSpecialApprovalItemModel = new List<SpecialApprovalItemModel>();
                    specialApproval.ListSpecialApprovalItemModel = GetModelByRequestId(id);
                    if (specialApproval.ListSpecialApprovalItemModel.Count>0)
                    {
                        ViewBag.requestStates = specialApproval.ListSpecialApprovalItemModel.FirstOrDefault().requestStates;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex.Message;
            }
            ViewBag.SpecialApprovalModel =JsonConvert.SerializeObject(specialApproval); 
            return View( );
        }

        public ActionResult Edit(string id)
        {
            SpecialApprovalModel specialApproval = new SpecialApprovalModel();
            ViewBag.msg = "";
            ViewBag.id = id;
            ViewBag.requestStates = "-1";
            try
            {
                if (!string.IsNullOrEmpty(id) && id != "0")
                {
                    specialApproval.Id = Convert.ToInt32(id);
                    specialApproval.ListSpecialApprovalItemModel = new List<SpecialApprovalItemModel>();
                    specialApproval.ListSpecialApprovalItemModel = GetModelByRequestId(id);
                    if (specialApproval.ListSpecialApprovalItemModel.Count > 0)
                    {
                        ViewBag.requestStates = specialApproval.ListSpecialApprovalItemModel.FirstOrDefault().requestStates;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex.Message;
            }
            ViewBag.SpecialApprovalModel = JsonConvert.SerializeObject(specialApproval);
            return View();
        }

        public ActionResult List(string id)
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveSpecialApproval(string jsonmodel)
        {
            DataResult dr = new DataResult();
            try
            {
                dr.Status = 0;
                dr.Message = "未提交数据";
                int intRequestId = 0;
                if (!string.IsNullOrEmpty(jsonmodel))
                {
                    var model = JsonHelper.DeserializeJson<SpecialApprovalModel>(jsonmodel);
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
            return  Json(dr);  
        }

        public DataTable GetDateByRequestId(string Id)
        {
            DataTable dt = new DataTable();
             
            string sql = @"select s.*,r.State as requestStates from  Request  r  
                       join  SpecialApprovalItem s
                       on r.id = s.RequstId  and r.RecordStatus = 0  and  s.RecordStatus = 0
                       where  r.id  = " + Id + " order by s.number";

            dt = BaseDao.ExecuteDataSet(sql, null, CommandType.Text).Tables[0];

            return dt;
        }

        public List<SpecialApprovalItemModel> GetModelByRequestId(string Id)
        {
            var dt = GetDateByRequestId(Id);
            List<SpecialApprovalItemModel> listSpecialApprovalItem = new List<SpecialApprovalItemModel>();
            if (dt!=null && dt.Rows.Count>0)
            {
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    SpecialApprovalItemModel specialApprovalItem = new SpecialApprovalItemModel();
                    //specialApprovalItem.Id = dt.Rows[i]["Number"] + "";
                    specialApprovalItem.Id = Convert.ToInt32(dt.Rows[i]["Id"] + "");
                    specialApprovalItem.Number = Convert.ToInt32(dt.Rows[i]["Number"] + "");
                    specialApprovalItem.SKUCode =  dt.Rows[i]["SKUCode"] + "";
                    specialApprovalItem.SKUDescription =  dt.Rows[i]["SKUDescription"] + "" ;
                    specialApprovalItem.NetWeight =  dt.Rows[i]["NetWeight"] + "" ;
                    specialApprovalItem.Packing =  dt.Rows[i]["Packing"] + "" ;
                    specialApprovalItem.BarCode =  dt.Rows[i]["BarCode"] + "" ;
                    specialApprovalItem.Length =  dt.Rows[i]["Length"] + "" ;
                    specialApprovalItem.Width =  dt.Rows[i]["Width"] + "" ;
                    specialApprovalItem.Height =  dt.Rows[i]["Height"] + "" ;
                    specialApprovalItem.ShelfLife = dt.Rows[i]["ShelfLife"] + "";
                    specialApprovalItem.GrossWeight = dt.Rows[i]["GrossWeight"] + "";
                    specialApprovalItem.DataIsNew =  (dt.Rows[i]["DataIsNew"] + "").ToLower()=="true"?1:0 ;
                    specialApprovalItem.requestStates = dt.Rows[i]["requestStates"] + "";
                    listSpecialApprovalItem.Add(specialApprovalItem);
                }
            }
            return listSpecialApprovalItem;
        }

        public int InsertData(SpecialApprovalModel model)
        {
            //插入主表
            var sqlRequest = @"INSERT INTO [Request]([Type],[State],[CustomerNumber],[CustomerName],[RecordStatus],[CreatedBy],[CreatedTime])
                             VALUES ('BarCodeSpecialApp','" + model.State + "',NULL,NULL,'0','" + Operation.OperationBy + "','" + DateTime.Now + "')Select @@Identity";
            var requstId = BaseDao.ExecuteScalar(sqlRequest, null, CommandType.Text);
            int intRequestId = Convert.ToInt32(requstId);
            //插入详细表
            InsertDetailInfo(intRequestId, model.ListSpecialApprovalItemModel);
            return intRequestId;
            #region
            ////插入详细表
            //if (Convert.ToInt32(requstId) > 0 && model.ListSpecialApprovalItemModel != null)
            //{
            //    var sqlDetail = "";
            //    //插入对应子表
            //    foreach (var item in model.ListSpecialApprovalItemModel)
            //    {
            //        sqlDetail += @"  INSERT INTO SpecialApprovalItem (
            //                                             [RequstId]
            //                                             ,[Number]
            //                                             ,[SKUCode]
            //                                             ,[SKUDescription]
            //                                             ,[NetWeight]
            //                                             ,[Packing]
            //                                             ,[BarCode]
            //                                             ,[Length]
            //                                             ,[Width]
            //                                             ,[Height]
            //                                             ,[ShelfLife]
            //                                             ,[GrossWeight]
            //                                             ,[DataIsNew]
            //                                             ,[CreatedBy]
            //                                             ,[CreatedTime]
            //                                             ,[ModifiedBy]
            //                                             ,[ModifiedOn] 
            //                                           ) 
            //                    VALUES("
            //            + requstId
            //            + ",'" + item.Number + "'"
            //            + ",'" + item.SKUCode + "'"
            //            + ",N'" + item.SKUDescription + "'"
            //            + ",N'" + item.NetWeight + "'"
            //            + ",N'" + item.Packing + "'"
            //            + ",N'" + item.BarCode + "'"
            //            + ",N'" + item.Length + "'"
            //            + ",N'" + item.Width + "'"
            //            + ",N'" + item.Height + "'"
            //            + ",N'" + item.ShelfLife + "'"
            //            + ",N'" + item.GrossWeight + "'"
            //            + ",N'" + item.DataIsNew + "'"
            //            + ",'" + Operation.OperationBy + "'"
            //            + ",'" + DateTime.Now + "'"
            //            + ",null"
            //            + ",'" + DateTime.Now + "') ;";

            //    }

            //    BaseDao.ExecuteScalar(sqlDetail, null, CommandType.Text);
            //}
            #endregion
        }

        public void UpdateData(SpecialApprovalModel model)
        {
            //更新主表
            string updateRequest = @"UPDATE [dbo].[Request]
                                     SET
                                      [State] = '"+ model.State+ "'"
                                     + " ,[ModifiedBy] = '"+ Operation.OperationBy +"'"
                                     +",[ModifiedOn] = getdate() WHERE id = "+ model.Id;

            BaseDao.ExecuteScalar(updateRequest, null, CommandType.Text);

            //删除详细表 , 重新插入
            //string delItemsql = "delete from SpecialApprovalItem where RequstId = " + model.Id;
            string delItemsql = " update  SpecialApprovalItem set RecordStatus = 1  where RequstId = " + model.Id;
            BaseDao.ExecuteScalar(delItemsql, null, CommandType.Text);
            InsertDetailInfo(model.Id, model.ListSpecialApprovalItemModel);
        }

        public void InsertDetailInfo(int requstId, List<SpecialApprovalItemModel> model)
        {
            var sqlDetail = "";
            //插入对应子表
            foreach (var item in model)
            {
                sqlDetail += @"  INSERT INTO SpecialApprovalItem (
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
                    + ",'" + DateTime.Now + "') ;";
            }
            BaseDao.ExecuteScalar(sqlDetail, null, CommandType.Text);
        }
    }
}
