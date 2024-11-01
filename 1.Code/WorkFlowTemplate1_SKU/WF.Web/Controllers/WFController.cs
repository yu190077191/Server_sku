using WF.BusinessRule;
using WF.Framework;
using WF.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Linq;
using MVC4Pager;
using WF.Framework.Helper;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Text;
using WF.DataAccess;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;
using System.Web.UI.WebControls;

namespace WF.Web.Controllers
{
    public class WFController : WorkFlowAPIController
    {
        WorkFlowAPIController api = new WorkFlowAPIController();

        public ActionResult Index(int? id = null)
        {
            return View(GetData(id));
        }

        public int Create(string type)
        {
            int id = RequestRule.InsertRequest(new Request
            {
                Type = type
            });

            return id;
        }

        public int Save(int id, string json)
        {
            FieldNameValue[] array = JsonHelper.DeserializeJson<FieldNameValue[]>(json);
            string xml = SerializationHelper.Serialize(array);
            List<Request> li = CommonRule.Get<Request>("SaveRequest", new[]{
                new SqlParameter("@Id",id),
                new SqlParameter("@xml",xml),
                new SqlParameter("@CreatedBy", Operation.OperationBy)
            });

            return id;
        }

        public ActionResult Edit(int? id = null, string listType = null)
        {
            List<Field> list = null;
            //if Oversize is Yes Bonus Weight/Volume must have value bluefish 2019-10-10
            string Oversize = "";
            if (id != null)
            {
                string stepName = "BU Finance";
                list = CommonRule.Get<Field>("[dbo].[GetRequest]", new[] {
                new SqlParameter("@pagesize", 1000),
                new SqlParameter("@Id", id),
                new SqlParameter("@StepName", stepName)
                }).Where(m => m.Name == "Oversize").ToList<Field>();

                Oversize = list[0].Value;
            }
            ViewData["RequestID"] = id;
            ViewData["Oversize"] = Oversize;
            ViewData["ListType"] = (string.IsNullOrEmpty(listType) ? "" : listType);
            return View(GetData(id));
        }

        public ActionResult EditCopy(int? id = null, string listType = null)
        {
            List<Field> list = null;
            string Oversize = "";
            if (id != null)
            {
                string stepName = "BU Finance";
                list = CommonRule.Get<Field>("[dbo].[GetRequest]", new[] {
                new SqlParameter("@pagesize", 1000),
                new SqlParameter("@Id", id),
                new SqlParameter("@StepName", stepName)
                }).Where(m => m.Name == "Oversize").ToList<Field>();

                Oversize = list[0].Value;
            }
            ViewData["RequestID"] = id;
            ViewData["Oversize"] = Oversize;
            ViewData["ListType"] = listType;
            return View(GetData(id));
        }

        public ActionResult Preview(int? id = null)
        {
            string xml = null;
            List<SAPEmail> list = CommonRule.Get<SAPEmail>("GetSAPEmail", new[]{
                new SqlParameter("@Id",id),
                new SqlParameter("@xml",xml),
                new SqlParameter("@CreatedBy", Operation.OperationBy)
            });

            List<Nestle.WorkFlow.Model.ReturnStepResult> ReturnStep = new List<Nestle.WorkFlow.Model.ReturnStepResult>();
            ReturnStep = api.GetReturnStep((int)id);
            if (ReturnStep.Count() > 0)
            {
                ViewBag.ReturnStep = string.Join("|", ReturnStep.Select(x => string.Format("{0}&{1}", x.ReturnStepId, x.ReturnStepName)));
            }
            else
            {
                ViewBag.ReturnStep = "";
            }

            ViewBag.SAPEmail = list;

            return View(GetData(id));
        }

        public ActionResult Print(int id)
        {
            return View(GetData(id));
        }

        public ActionResult SaveImportedData(Guid id, string tableId, int requestVersionId, string name, string goodsType, string bu)
        {
            string storedPrecedureName = "[Import_WriteOff]";
            if (tableId == "StandardCost")
            {
                storedPrecedureName = "[Import_StandardCost]";
            }

            try
            {
                string xmlString = CommonRule.GetXmlString(storedPrecedureName, new[] {
                new SqlParameter("@OperationBy", Operation.OperationBy),
                new SqlParameter("@UniqueId", id),
                new SqlParameter("@TableNames", tableId),
                new SqlParameter("@RequestVersionId", requestVersionId),
                new SqlParameter("@name", name),
                new SqlParameter("@goodsType", goodsType),
                new SqlParameter("@BU", bu)
            });

                if (!string.IsNullOrEmpty(xmlString))
                {
                    if (xmlString.Contains("UploadResult"))
                    {
                        List<UploadResult> list = CommonRule.GetByXmlString<UploadResult>(xmlString);
                        return PartialView("UploadResult", list);
                    }
                    else if (xmlString.Contains("WriteOffItems"))
                    {
                        List<WriteOffItems> list = CommonRule.GetByXmlString<WriteOffItems>(xmlString);
                        ViewBag.IsEdit = true;
                        return PartialView("PartialWriteOffItems", list);
                    }
                }
            }
            catch (Exception ex)
            {
                List<UploadResult> list = new List<UploadResult>();
                list.Add(new UploadResult { Message = "Error: " + ex.Message + " Stack: " + ex.StackTrace });
                return PartialView("UploadResult", list);
            }

            return View();
        }

        public ActionResult LoadData(int id)
        {
            List<WriteOffItems> list = CommonRule.Get<WriteOffItems>("[GetWriteOffItemsByRequestVersionId]", new[] { new SqlParameter("@RequestVersionId", id) });
            return PartialView("PartialWriteOffItems", list);
        }

        public List<Field> GetData(int? id)
        {
            List<Field> list = null;
            int requestId = 0;
            if (id != null)
            {
                List<Nestle.WorkFlow.Model.ActionHistory> historyList = api.GetApprovers((int)id, (int)id);
                string stepName = string.Empty;
                ViewBag.historyList = historyList;
                if (historyList != null && historyList.Count > 0)
                {
                    var pending = historyList.Where(k => k.IsCurrent && k.EmployeeId == Operation.OperationBy);
                    if (pending != null && pending.Count() > 0)
                    {
                        ViewBag.Pending = pending.ToList();
                        stepName = pending.First().StepName;
                    }
                }

                list = CommonRule.Get<Field>("[dbo].[GetRequest]", new[] {
                new SqlParameter("@pagesize", 1000),
                new SqlParameter("@Id", id),
                new SqlParameter("@StepName", stepName)
                });
                requestId = list[0].RequestId;
            }

            List<Nestle.WorkFlow.Model.Button> buttonList = api.GetButtons(requestId, id);
            string url = Request.Url.ToString().ToLower();
            //if (url.Contains("edit?") && buttonList.Where(k => k.EventName == "Submit").Count() == 0)
            //{
            //    Response.Write(LanguagesRule.Translate("Invalid operation!"));
            //    Response.End();
            //}

            ViewBag.ButtonList = buttonList;
            //if (buttonList.Count == 0 && url.Contains("?id") && !WorkFlowAPIController.IsAdmin)
            //{
            //    Response.Write(LanguagesRule.Translate("Access denied!"));
            //    Response.End();
            //}
            List<FieldIdsResult> ChangeData = new List<FieldIdsResult>();
            ChangeData = CommonRule.Get<FieldIdsResult>("GetChangeData", new[] {
                    new System.Data.SqlClient.SqlParameter("@RequestId", requestId)
                });
            if (ChangeData.Count() > 0)
            {
                ViewBag.ChangeData = ChangeData[0].FieldIds;
            }
            else
            {
                ViewBag.ChangeData = "";
            }

            return list;
        }

        public ActionResult Approver(int? pageIndex = null, string json = null)
        {
            Query query = new Query(pageIndex, json);
            QueryResult<ApproverItem> result = api.GetApproverWorkListResult(query.GetParameters());
            var model = new PagedList<ApproverItem>(result.DataList, query.PageIndex, query.PageSize, result.Count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("PartialApproverItemList", model);
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Approvers(int id)
        {
            int requestId = 0;
            List<WriteOffItems> li = CommonRule.Get<WriteOffItems>("[GetWriteOffItemsByRequestVersionId]", new[] { new SqlParameter("@RequestVersionId", id) });
            if (li != null && li.Count > 0)
            {
                requestId = li[0].RequestId;
            }

            List<Nestle.WorkFlow.Model.Button> buttonList = api.GetButtons(requestId, id);
            string url = Request.Url.ToString().ToLower();
            if (buttonList.Where(k => k.EventName == "Submit").Count() == 0
                && buttonList.Where(k => k.EventName.ToLower() == "approve").Count() == 0
                )
            {
                Response.Write(LanguagesRule.Translate("Invalid operation!"));
                Response.End();
            }

            ViewBag.ButtonList = buttonList;
            List<Nestle.WorkFlow.Model.ActionHistory> list = api.GetApprovers(requestId, id);
            list = list.Where(k => k.Sequence > 0).ToList();
            return View(list);
        }

        #region RequestVersion

        public ActionResult RequestVersionList(int? pageIndex = null, string json = null)
        {
            Query query = new Query(pageIndex, json);
            QueryResult<WF.Model.RequestVersion> result = RequestVersionRule.GetRequestVersionResult("GetRequestVersionProc", query.GetParameters());
            var model = new PagedList<WF.Model.RequestVersion>(result.DataList, query.PageIndex, query.PageSize, result.Count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("RequestVersionListPartial", model);
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult RequestList(int? pageIndex = null, string json = null)
        {
            Query query = new Query(pageIndex, json);
            QueryResult<WF.Model.RequestVersion> result = RequestVersionRule.GetRequestVersionResult("GetRequestVersionProc", query.GetParameters());
            var model = new PagedList<WF.Model.RequestVersion>(result.DataList, query.PageIndex, query.PageSize, result.Count);
            if (Request.IsAjaxRequest())
            {
                return PartialView("RequestSelectList", model);
            }
            else
            {
                return View(model);
            }
        }
        public ActionResult RequestListData(string RequestID)
        {
            ViewBag.RequestID = RequestID;
            if (Request.IsAjaxRequest())
            {
                return PartialView("RequestSelectListData");
            }
            return View();
        }
        public ActionResult SearchRequestListData(string SKUCode, string RequestID)
        {
            JArray jaData = new JArray();
            string output = string.Empty;
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = "D:\\CURL\\curl.exe";
                process.StartInfo.Arguments = "-H \"Referer: https://wf.nestlechinese.com/\" -H \"Ocp-Apim-Subscription-Key: f124de52f407492e9ae34724273afb56\" https://api.nestlechinese.com/mdmceapi/pr/MDM/data/v1/getData?accessStr=b0ba2c7601da7f54cb0568264e9e965c&tableName=MDMAPI_Material_SKUWebForm&filterContent=MaterialNumber=" + SKUCode;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            ActionResult result;
            try
            {
                new List<RequestVersionData>();
                if (!string.IsNullOrEmpty(output))
                {
                    jaData = JArray.Parse(Encoding.UTF8.GetString(Encoding.Default.GetBytes(output)));
                    JArray jaDataTemp = new JArray();
                    int IsCN_HK_TW = Operation.Employee.Mail.ToLower().Contains("@cn.nestle.com") ? 0 : (Operation.Employee.Mail.ToLower().Contains("@hk.nestle.com") ? 1 : 2);
                    if (IsCN_HK_TW == 0)
                    {
                        JArray JaCN = jaData;
                        if (JaCN.Where(o => o["SalesOrg"].ToString().ToUpper().Contains("CN")) != null)
                        {
                            jaDataTemp.Add(JaCN.Where(o => o["SalesOrg"].ToString().ToUpper().Contains("CN")));
                        }
                        else 
                        { jaDataTemp = jaData; }
                    }
                    else if (IsCN_HK_TW == 1)
                    {
                        JArray JaHK = jaData;
                        if (JaHK.Where(o => o["SalesOrg"].ToString().ToUpper().Contains("HK")) != null)
                        {
                            jaDataTemp.Add(JaHK.Where(o => o["SalesOrg"].ToString().ToUpper().Contains("HK")));
                        }
                        else
                        { jaDataTemp = jaData; }
                    }
                    else if (IsCN_HK_TW == 2)
                    {
                        JArray JaTW = jaData;
                        if (JaTW.Where(o => o["SalesOrg"].ToString().ToUpper().Contains("TW")) != null)
                        {
                            jaDataTemp.Add(JaTW.Where(o => o["SalesOrg"].ToString().ToUpper().Contains("TW")));
                        }
                        else
                        { jaDataTemp = jaData; }
                    }
                    JArray arg_1A6_0 = jaDataTemp;
                    Func<JToken, string> arg_1A6_1;
                    //if ((arg_1A6_1 = WFController.<> c.<> 9__16_3) == null)
                    //{
                    //    arg_1A6_1 = (WFController.<> c.<> 9__16_3 = new Func<JToken, string>(WFController.<> c.<> 9.< SearchRequestListData > b__16_3));
                    //}
                    IEnumerable<IGrouping<string, JToken>> arg_41E_0 = arg_1A6_0.GroupBy(x => x["SalesOrg"].ToString());
                    RequestVersionData model = new RequestVersionData();
                    model.Id = 1;
                    model.Origin = jaDataTemp[0]["Origin"].ToString();
                    model.MaterialType = jaDataTemp[0]["MaterialType"].ToString();
                    model.UOMofBUN = jaDataTemp[0]["UOMofBUN"].ToString();
                    model.GlobalAttribute3 = jaDataTemp[0]["GlobalAttribute3"].ToString();
                    model.BaseProduct = jaDataTemp[0]["BaseProduct"].ToString();
                    model.BusinessGroup = jaDataTemp[0]["BusinessGroup"].ToString();
                    model.UOMOfVolumeInCS = jaDataTemp[0]["UOMOfVolumeInCS"].ToString();
                    model.ChineseDescription = jaDataTemp[0]["ChineseDescription"].ToString();
                    model.MaterialGroup = jaDataTemp[0]["MaterialGroup"].ToString();
                    model.ProductHierarchy = jaDataTemp[0]["ProductHierarchy"].ToString();
                    model.MaterialGroup1 = jaDataTemp[0]["MaterialGroup1"].ToString();
                    model.MaterialDescription = jaDataTemp[0]["MaterialDescription"].ToString();
                    model.RangeBrand = jaDataTemp[0]["RangeBrand"].ToString();
                    model.ParentCode = jaDataTemp[0]["ParentCode"].ToString();
                    model.SalesUnit = jaDataTemp[0]["SalesUnit"].ToString();
                    model.BAI = jaDataTemp[0]["BAI"].ToString();
                    model.CorporateBrand = jaDataTemp[0]["CorporateBrand"].ToString();
                    model.BrandDenomination = jaDataTemp[0]["BrandDenomination"].ToString();
                    model.MaterialNumber = jaDataTemp[0]["MaterialNumber"].ToString();
                    model.MaterialGrp4 = jaDataTemp[0]["MaterialGrp4"].ToString();
                    model.MaterialGrp3 = jaDataTemp[0]["MaterialGrp3"].ToString();
                    int index = 0;
                    using (IEnumerator<IGrouping<string, JToken>> enumerator = arg_41E_0.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            IGrouping<string, JToken> group = enumerator.Current;
                            if (index == 0)
                            {
                                model.SalesOrg1 = group.Key;
                            }
                            else if (index == 1)
                            {
                                model.SalesOrg2 = group.Key;
                            }
                            else if (index == 2)
                            {
                                model.SalesOrg3 = group.Key;
                            }
                            if (index >= 3)
                            {
                                break;
                            }
                            foreach (JToken item in (IEnumerable<JToken>)new JArray(
                                from p in jaDataTemp
                                where p["SalesOrg"].ToString() == @group.Key
                                select p))
                            {
                                if (index == 0)
                                {
                                    model.DeliveringPlant1 = item["DeliveringPlant"].ToString();
                                    RequestVersionData expr_4DD = model;
                                    expr_4DD.DistributionChannel1 = expr_4DD.DistributionChannel1 + item["DistributionChannel"].ToString() + ";";
                                }
                                else if (index == 1)
                                {
                                    model.DeliveringPlant2 = item["DeliveringPlant"].ToString();
                                    RequestVersionData expr_527 = model;
                                    expr_527.DistributionChannel2 = expr_527.DistributionChannel2 + item["DistributionChannel"].ToString() + ";";
                                }
                                else if (index == 2)
                                {
                                    model.DeliveringPlant3 = item["DeliveringPlant"].ToString();
                                    RequestVersionData expr_56E = model;
                                    expr_56E.DistributionChannel3 = expr_56E.DistributionChannel3 + item["DistributionChannel"].ToString() + ";";
                                }
                            }
                            index++;
                        }
                    }
                    model.dataInfo = new JObject
                    {
                        {
                            "data39",
                            model.Origin
                        },

                        {
                            "data10",
                            model.MaterialType
                        },

                        {
                            "data12",
                            model.UOMofBUN
                        },

                        {
                            "data3",
                            model.SalesOrg1
                        },

                        {
                            "data265",
                            model.SalesOrg2
                        },

                        {
                            "data284",
                            model.SalesOrg3
                        },

                        {
                            "data19",
                            model.GlobalAttribute3
                        },

                        {
                            "data38",
                            model.BusinessGroup
                        },

                        {
                            "data11",
                            model.MaterialGroup
                        },

                        {
                            "data35",
                            model.ProductHierarchy
                        },

                        {
                            "data5",
                            model.DistributionChannel1
                        },

                        {
                            "data278",
                            model.DistributionChannel2
                        },

                        {
                            "data292",
                            model.DistributionChannel3
                        },

                        {
                            "data29",
                            model.DeliveringPlant1
                        },

                        {
                            "data295",
                            model.DeliveringPlant2
                        },

                        {
                            "data298",
                            model.DeliveringPlant3
                        },

                        {
                            "data32",
                            model.MaterialGroup1
                        },

                        {
                            "data14",
                            model.RangeBrand
                        },

                        {
                            "data22",
                            model.ParentCode
                        },

                        {
                            "data28",
                            model.SalesUnit
                        },

                        {
                            "data16",
                            model.BAI
                        },

                        {
                            "data13",
                            model.CorporateBrand
                        },

                        {
                            "data15",
                            model.BrandDenomination
                        },

                        {
                            "data34",
                            model.MaterialGrp4
                        },

                        {
                            "data33",
                            model.MaterialGrp3
                        }
                    }.ToString();
                    CommonRule.GetDataSet("InsertCopyLog", new[]{
                        new SqlParameter("@RequestId", RequestID),
                        new SqlParameter("@CreatedBy", Operation.OperationBy)
                    });
                    List<RequestVersionData> modelList = new List<RequestVersionData>();
                    modelList.Add(model);
                    PagedList<RequestVersionData> returnModel = new PagedList<RequestVersionData>(modelList, 1, 20, modelList.Count);
                    result = this.PartialView("RequestSelectListData", returnModel);
                }
                else
                {
                    List<RequestVersionData> modelList2 = new List<RequestVersionData>();
                    PagedList<RequestVersionData> returnModel2 = new PagedList<RequestVersionData>(modelList2, 1, 20, modelList2.Count);
                    result = this.PartialView("RequestSelectListData", returnModel2);
                }
            }
            catch (Exception)
            {
                List<RequestVersionData> modelList3 = new List<RequestVersionData>();
                PagedList<RequestVersionData> returnModel3 = new PagedList<RequestVersionData>(modelList3, 1, 20, modelList3.Count);
                result = this.PartialView("RequestSelectListData", returnModel3);
            }
            return result;
        }
        public JsonResult GetCBRBData(string CBCode, string RBCode, string tType)
        {
            DataResult result = new DataResult();
            List<string> list = new List<string>();
            IDatabase db = DataFactory.Database();
            new DataTable();
            IEnumerator enumerator;
            if (tType == "CB")
            {
                if (string.IsNullOrEmpty(CBCode))
                {
                    return Json(new
                    {
                        Success = false
                    }, JsonRequestBehavior.AllowGet);
                }
                CBCode = (CBCode.Contains("99999") ? CBCode : CBCode.Substring(0, 6));
                enumerator = db.FindTableBySql("select * from CB_RB_BDMap WHERE CBCode='" + CBCode + "'").Rows.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        DataRow dr = (DataRow)enumerator.Current;
                        if (!list.Contains(dr[2].ToString()))
                        {
                            list.Add(dr[2].ToString());
                        }
                    }
                    goto IL_19F;
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
            if (string.IsNullOrEmpty(CBCode) && string.IsNullOrEmpty(RBCode))
            {
                return Json(new
                {
                    Success = false
                }, JsonRequestBehavior.AllowGet);
            }
            CBCode = (CBCode.Contains("99999") ? CBCode : CBCode.Substring(0, 6));
            RBCode = (RBCode.Contains("999999") ? RBCode : RBCode.Substring(0, 7));
            enumerator = db.FindTableBySql(string.Concat(new string[]
            {
                "select * from CB_RB_BDMap WHERE  CBCode='",
                CBCode,
                "' AND RBCode='",
                RBCode,
                "'"
            })).Rows.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DataRow dr2 = (DataRow)enumerator.Current;
                    list.Add(dr2[4].ToString());
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        IL_19F:
            return Json(result.Data = list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMG_MG1_PHData(string Province)
        {
            DataResult result = new DataResult();
            if (string.IsNullOrEmpty(Province))
            {
                return Json(new
                {
                    Success = false
                }, JsonRequestBehavior.AllowGet);
            }
            List<MG_MG1_PHMap> list = new List<MG_MG1_PHMap>();
            IDatabase arg_3D_0 = DataFactory.Database();
            DataTable PlantTable = new DataTable();
            PlantTable = arg_3D_0.FindTableBySql("select * from MG_MG1_PHMap WHERE [BUName]='" + Province + "'");
            DataRow[] array = PlantTable.Select("ControlType ='MG'");
            for (int i = 0; i < array.Length; i++)
            {
                DataRow dr = array[i];
                list.Add(new MG_MG1_PHMap
                {
                    ControlType = "MG",
                    ShowValue = dr[2].ToString(),
                    ShowDetails = dr[3].ToString()
                });
            }
            array = PlantTable.Select("ControlType ='MG1'");
            for (int i = 0; i < array.Length; i++)
            {
                DataRow dr2 = array[i];
                list.Add(new MG_MG1_PHMap
                {
                    ControlType = "MG1",
                    ShowValue = dr2[2].ToString(),
                    ShowDetails = dr2[3].ToString()
                });
            }
            array = PlantTable.Select("ControlType ='PHDetails'");
            for (int i = 0; i < array.Length; i++)
            {
                DataRow dr3 = array[i];
                list.Add(new MG_MG1_PHMap
                {
                    ControlType = "PHDetails",
                    ShowValue = dr3[2].ToString(),
                    ShowDetails = dr3[3].ToString()
                });
            }
            array = PlantTable.Select("ControlType ='Creation'");
            for (int i = 0; i < array.Length; i++)
            {
                DataRow dr4 = array[i];
                list.Add(new MG_MG1_PHMap
                {
                    ControlType = "Creation",
                    ShowValue = dr4[2].ToString(),
                    ShowDetails = dr4[3].ToString()
                });
            }
            array = PlantTable.Select("ControlType ='GPC'");
            for (int i = 0; i < array.Length; i++)
            {
                DataRow dr5 = array[i];
                list.Add(new MG_MG1_PHMap
                {
                    ControlType = "GPC",
                    ShowValue = dr5[2].ToString(),
                    ShowDetails = dr5[3].ToString()
                });
            }
            return Json(result.Data = list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPHCodeByPHDetails(string Province, string PHDetails)
        {
            if (string.IsNullOrEmpty(Province) || string.IsNullOrEmpty(PHDetails))
            {
                return Json(new
                {
                    Success = false
                }, JsonRequestBehavior.AllowGet);
            }
            DataTable PlantTable = DataFactory.Database().FindTableBySql(string.Concat(new string[]
            {
                "select top 1 * from MG_MG1_PHMap WHERE [BUName]='",
                Province,
                "' AND ControlType='PHDetails' AND ShowDetails='",
                PHDetails,
                "'"
            }));
            if (PlantTable == null)
            {
                return Json(new
                {
                    Success = false
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                Success = true,
                ShowValue = PlantTable.Rows[0]["ShowValue"].ToString()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMaterialDescriptionEN(string SKUCode)
        {
            string output = string.Empty;
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = "D:\\CURL\\curl.exe";
                process.StartInfo.Arguments = "-H \"Referer: https://wf.nestlechinese.com/\" -H \"Ocp-Apim-Subscription-Key: f124de52f407492e9ae34724273afb56\" https://api.nestlechinese.com/mdmceapi/pr/MDM/data/v1/getData?accessStr=b0ba2c7601da7f54cb0568264e9e965c&tableName=MDMAPI_Material_SKUWebForm&filterContent=MaterialNumber=" + SKUCode;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            JsonResult result;
            try
            {
                if (!string.IsNullOrEmpty(output))
                {
                    JArray jaData = JArray.Parse(Encoding.UTF8.GetString(Encoding.Default.GetBytes(output)));
                    if (jaData.Count > 0)
                    {
                        result = Json(new
                        {
                            Success = true,
                            MaterialDescriptionEN = jaData[0]["MaterialDescription"].ToString(),
                            CurrOperation = Operation.OperationBy.ToString()
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result = Json(new
                        {
                            Success = false,
                            MaterialDescriptionEN = "Value Null"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = Json(new
                    {
                        Success = false,
                        MaterialDescriptionEN = "Value Error1"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                result = Json(new
                {
                    Success = false,
                    MaterialDescriptionEN = "Value Error2：" + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
            return result;
        }
        //public ActionResult getRequestList(int? pageIndex = null, string json = null)
        //{
        //    Query query = new Query(pageIndex, json);
        //    QueryResult<WF.Model.RequestVersion> result =  RequestVersionRule.GetRequestVersionResult("GetRequestVersionProc", query.GetParameters());
        //    var model = new PagedList<WF.Model.RequestVersion>(result.DataList, query.PageIndex, query.PageSize, result.Count);

        //    return PartialView("RequestSelectList", model);
        //}
        #endregion RequestVersion

        public ActionResult Upload()
        {
            return View();
        }

        public JsonResult UpdateRequestVersion(string json)
        {
            RequestVersion rv = JsonHelper.DeserializeJson<RequestVersion>(json);
            int r = RequestVersionRule.UpdateRequestVersion(rv);
            return Json(new { Success = r > 0 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateField(string json)
        {
            UpdateField uf = JsonHelper.DeserializeJson<UpdateField>(json);
            int r = RequestVersionRule.UpdateField(uf);
            return Json(new { Success = r > 0 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateWord(int id)
        {
            try
            {
                List<Field> fList = GetData(id);
                string templateFolder = "templates/contract2018/";
                string targetPath = "Contracts/2018GT" + id.ToString().PadLeft(4, '0') + ".docx";
                List<WordParameters> list = new List<WordParameters>();
                list.Add(new WordParameters
                {
                    Path = "word/header2.xml",
                    Code = "2018GT0001",
                    Value = fList[0].ContractNumber
                });

                string path = WordRule.CreateWord(templateFolder, targetPath, list);
                return Json(new { Error = string.Empty, Path = path }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message + " stack: " + ex.StackTrace }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateRequest(string json)
        {
            Request rv = JsonHelper.DeserializeJson<Request>(json);
            Request old = CommonRule.Get<Request>(rv.Id);
            old.CustomerNumber = rv.CustomerNumber;
            //old.CustomerName = rv.CustomerName;
            int r = RequestRule.UpdateRequest(old);
            return Json(new { Success = r > 0 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSAPEmail(int id, string json)
        {
            List<SAPEmail> list = null;
            try
            {
                string xml = JsonHelper.GetXmlString(json);
                list = CommonRule.Get<SAPEmail>("GetSAPEmail", new[]{
                new SqlParameter("@Id",id),
                new SqlParameter("@xml",xml),
                new SqlParameter("@CreatedBy", Operation.OperationBy)
            });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return PartialView("PartialSAPEmail", list);
        }

        public ActionResult SetSAPEmail(int id, string json)
        {
            List<SAPEmail> list = null;
            try
            {
                string xml = JsonHelper.GetXmlString(json);
                list = CommonRule.Get<SAPEmail>("SetSAPEmail", new[]{
                new SqlParameter("@Id",id),
                new SqlParameter("@xml",xml),
                new SqlParameter("@CreatedBy", Operation.OperationBy)
            });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return PartialView("PartialSAPEmail", list);
        }


        public JsonResult GetPHDES(string L5Code)
        {
            if (string.IsNullOrEmpty(L5Code))
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string L5Description = CommonRule.GetPHDES(L5Code);
                if (string.IsNullOrEmpty(L5Description))
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = true, L5Des = L5Description }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        public JsonResult GetBusiness(string Bu, string Branch)
        {
            if (string.IsNullOrEmpty(Bu) || string.IsNullOrEmpty(Branch))
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string Businessstr = CommonRule.GetBusiness(Bu, Branch);
                if (string.IsNullOrEmpty(Businessstr))
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = true, Business = Businessstr }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        public ActionResult Packaging(int? id = null)
        {
            if (id != null)
            {

            }
            return View();
        }


        public JsonResult SavePackaging(string json)
        {
            PackagingJson model = JsonHelper.DeserializeJson<PackagingJson>(json);
            int row = CommonRule.AddPackagingJson(model);
            if (row > 0)
            {
                return Json(new { Success = row > 0, Id = row }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GePackaging(int RequestID)
        {
            DataResult result = new DataResult();
            List<PackagingDetails> list = new List<PackagingDetails>();
            if (RequestID == 0)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                list = CommonRule.Get<PackagingDetails>("GetPackaging", new[] {
                    new System.Data.SqlClient.SqlParameter("@RequestID", RequestID)

                });

            }
            return Json(result.Data = list);


        }

        public JsonResult GetCity(string Province)
        {
            DataResult result = new DataResult();
            if (string.IsNullOrEmpty(Province))
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<BranchResult> list = new List<BranchResult>();
                list = CommonRule.Get<BranchResult>("GetBranch", new[] {
                    new System.Data.SqlClient.SqlParameter("@BU", Province)
                });
                if (list != null)
                {
                    return Json(result.Data = list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetRequestVersionById(string strId)
        {
            List<Field> list = null;
            DataResult result = new DataResult();
            if (string.IsNullOrEmpty(strId))
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                list = CommonRule.Get<Field>("[dbo].[GetRequest]", new[] {
                new SqlParameter("@pagesize", 1000),
                new SqlParameter("@Id", strId),
                new SqlParameter("@StepName", "")
                });
                if (list != null)
                {

                    return Json(result.Data = list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetProductClassification(string Product1)
        {
            DataResult result = new DataResult();
            if (string.IsNullOrEmpty(Product1))
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Product1 = Product1.Substring(0, Product1.IndexOf(" "));
                List<ProductResult> list = new List<ProductResult>();
                list = CommonRule.Get<ProductResult>("GetProductClassification", new[] {
                    new System.Data.SqlClient.SqlParameter("@Code", Product1)
                });
                if (list != null)
                {

                    return Json(result.Data = list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }

            }


        }

        public JsonResult GetPlant(string CompanyCode)
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            if (string.IsNullOrEmpty(CompanyCode))
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string[] CompanyArr = CompanyCode.Split(',');
                WF.DataAccess.IDatabase db = WF.DataAccess.DataFactory.Database();
                string PlantStr = "[";
                for (int i = 0; i < CompanyArr.Length; i++)
                {
                    DataTable PlantTable = new DataTable();
                    int num = i + 1;
                    int level2 = 100;
                    string Companycode = CompanyArr[i].Substring(0, 4);
                    PlantTable = db.FindTableBySql("select * from PlantMap");
                    if (i == 0)
                    {
                        PlantStr += "{id:" + num + ", pId:0, name:\"" + Companycode + "\", open:true, nocheck:true}";
                    }
                    else
                    {
                        PlantStr += ",{id:" + num + ", pId:0, name:\"" + Companycode + "\", open:true, nocheck:true}";
                    }
                    foreach (DataRow dr in PlantTable.Select("companycode ='" + Companycode + "'"))
                    {
                        level2++;
                        PlantStr += ",{ id: " + level2 + " , pId: " + num + ", name: \"" + dr[1].ToString() + " " + dr[2].ToString() + "\"}";
                    }
                }
                PlantStr += "]";
                return Json(js.Deserialize(PlantStr, null), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetPlantByOrg(string CompanyCode)
        {
            DataResult result = new DataResult();
            if (string.IsNullOrEmpty(CompanyCode))
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<string> list = new List<string>();
                string[] CompanyArr = CompanyCode.Split(',');
                WF.DataAccess.IDatabase db = WF.DataAccess.DataFactory.Database();
                for (int i = 0; i < CompanyArr.Length; i++)
                {
                    DataTable PlantTable = new DataTable();
                    string Companycode = CompanyArr[i].Substring(0, 4);
                    PlantTable = db.FindTableBySql("select * from PlantMap");
                    foreach (DataRow dr in PlantTable.Select("companycode ='" + Companycode + "'"))
                    {
                        list.Add(dr[1].ToString() + " " + dr[2].ToString());
                    }
                }
                return Json(result.Data = list, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetReturnStep(string StrRequestVersionId)
        {
            DataResult result = new DataResult();
            if (string.IsNullOrEmpty(StrRequestVersionId))
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<ProductResult> list = new List<ProductResult>();
                list = CommonRule.Get<ProductResult>("GetReturnStep", new[] {
                    new System.Data.SqlClient.SqlParameter("@RequestVersionId", StrRequestVersionId)
                });
                if (list != null)
                {
                    return Json(result.Data = list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}