using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WF.BusinessRule;
using WF.DataAccess;
using WF.Framework;
using WF.Model;

namespace WF.Web.Controllers
{
    public class UploadController : BaseController
    {
        [AcceptVerbs(HttpVerbs.Post)]
        public string Index(HttpPostedFileBase fileData)
        {
            bool canAccess = true; // WF.Web.Controllers.WorkFlowAPIController.IsAdmin || EmployeeRoleRule.IsITAdmin() || EmployeeRoleRule.IsRequester();
            if (canAccess)
            {
                if (Request.Files.Count > 0 && fileData == null)
                {
                    fileData = Request.Files[0];
                }

                if (fileData != null)
                {
                    int id = 0;
                    try
                    {
                        string uploadFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.UploadFolder, Constants.DateString);
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        Guid guid = Guid.NewGuid();
                        string fileName = Path.GetFileName(fileData.FileName);
                        string fileExtension = Path.GetExtension(fileName);
                        string saveName = guid.ToString() + fileExtension;
                        string fileFullPath = Path.Combine(uploadFolder, saveName);
                        if (string.IsNullOrEmpty(fileExtension) || ("," + Constants.UploadFileType + ",").IndexOf("," + fileExtension.ToLower() + ",") < 0)
                        {
                            if (Request.Form["type"] == "IBRelease")
                            {
                                return "Please select an Excel 2003 file!";
                            }

                            return "File type is not allowed!";
                        }

                        fileData.SaveAs(fileFullPath);
                        

                        string error = string.Empty;
                        
                        string type = Request.Form["type"];

                        if (type == "MD5")
                        {
                            string md5 = MD5SHA1Rule.GetMd5Hash(fileFullPath);
                            string sha1 = MD5SHA1Rule.GetSHA1Hash(fileFullPath);
                            return md5 + "|" + sha1;
                        }

                        if (type == "ImportDataFromExcel")
                        {
                            string tableId = Request.Form["tableId"];
                            string result = BulkCopyRule.Upload(fileFullPath, guid, tableId);
                            Attachment model = new Attachment();
                            model.TypeCode = tableId;
                            model.SubCode = "Excel";
                            model.Description = fileName;
                            model.FilePath = AttachmentRule.GetVirtualPath(fileFullPath);
                            model.RequestVersionId = id;
                            model.Id = guid;
                            if (AttachmentRule.InsertAttachment(model) > 0)
                            {
                                return guid.ToString() + "|" + result;
                            }
                        }

                        if (type == "ImportDataFromExcel")
                        {
                            string tableId = Request.Form["tableId"];
                            string result = BulkCopyRule.Upload(fileFullPath, guid, tableId);
                            if (result.ToUpper() != "OK")
                            {
                                return result;
                            }

                            id = Convert.ToInt32(Request.Form["id"]);

                            if (id <= 0)
                            {
                                id = ProjectRequirementRule.CreateId(guid);
                            }

                            string storedProcedureName = "UploadPreliminaryBusinessCase";
                            string html = AttachmentRule.SaveData(storedProcedureName, guid, id);
                            Session[Constants.UploadResultHtml] = html;

                            if (html.ToLower().Contains("successfully"))
                            {
                                Attachment model = new Attachment();
                                model.TypeCode = Request.Form["typeCode"];
                                model.SubCode = Request.Form["subCode"];
                                model.Description = Request.Form["Description"];
                                model.FilePath = AttachmentRule.GetVirtualPath(fileFullPath);
                                model.RequestVersionId = id;
                                model.Id = Guid.NewGuid();
                                if (AttachmentRule.InsertAttachment(model) > 0)
                                {
                                    return id.ToString() + "|" + model.Id.ToString();
                                }
                            }

                            return id.ToString() + "|" + "Error!";
                        }
                        else if (Request.Form["isCommonUpload"] == "true")
                        {
                            int maxColumnCount = Convert.ToInt32(Request.Form["maxColumnCount"]);
                            type = CommonRule.GetSafeTableName(type);
                            string html = AttachmentRule.Upload(fileFullPath, "Temp_" + type, "UploadFile", guid, maxColumnCount, addUploadedTime: true);
                            Session[Constants.UploadResultHtml] = html;
                            return "ok";
                        }
                        else if (Request.Form["type"] == "CommonAttachment")
                        {
                            id = Convert.ToInt32(Request.Form["id"]);

                            if (id <= 0)
                            {
                                id = ProjectRequirementRule.CreateId(guid);
                            }

                            Attachment model = new Attachment();
                            model.TypeCode = Request.Form["typeCode"];
                            model.SubCode = Request.Form["subCode"];
                            model.Description = Request.Form["Description"];
                            model.FilePath = AttachmentRule.GetVirtualPath(fileFullPath);
                            model.RequestVersionId = id;
                            model.Id = Guid.NewGuid();
                            if (AttachmentRule.InsertAttachment(model) > 0)
                            {
                                return id.ToString() + "|" + model.Id.ToString();
                            }
                        }
                        else if (Request.Form["type"] == "BuCodeInfoFile")
                        {
                           
                            Attachment model = new Attachment();
                            model.Id = Guid.NewGuid();
                            //BarInfo：只有条码业务所用
                            model.TypeCode = "BarCodeNew";
                            //详情Id
                            model.RequestVersionId = Convert.ToInt32(Request.Form["Description123"]);
                            //文件名称
                            model.Description = fileData.FileName;
                            //文件路径
                            model.FilePath = AttachmentRule.GetVirtualPath(fileFullPath);
                            //上市时间
                            model.SubCode =Convert.ToDateTime(Request.Form["uploadDateTime"]).ToString("yyyy-MM-dd");

                            var info = AttachmentRule.GetAttachment(model.RequestVersionId, "BarInfo").Where(o=>o.TypeCode == "BarInfo").ToList();

                            if (info.Count > 0)
                            {
                                //更新
                                string UpSql = "update Attachment " +
                                    "set Id='"+ model.Id + "',SubCode='" + model.SubCode + "',Description='"+ model.Description + "'," +
                                    "FilePath = '"+ model.FilePath + "',ModifiedBy = '"+ Operation.OperationBy + "',ModifiedOn = '"+ System.DateTime.Now + "' " +
                                    "where RequestVersionId = "+ model.RequestVersionId + " and TypeCode ='BarInfo' and RecordStatus = 0";
                                if (BaseDao.ExecuteNonQuery(UpSql, null, CommandType.Text) > 0)
                                {
                                    return id.ToString() + "|" + model.Id.ToString();
                                }
                            }
                            else
                            {
                                if (AttachmentRule.InsertAttachment(model) > 0)
                                {
                                    //再次更新CodeDetails信息
                                    string sql = "update SKUBarCodeDetailsInfo set KeepType = 1 where id = " + model.RequestVersionId + "";
                                    if (BaseDao.ExecuteNonQuery(sql, null, CommandType.Text) > 0)
                                    {
                                        return id.ToString() + "|" + model.Id.ToString();
                                    }

                                }
                            }
                        }

                        return "Sorry, failed!";
                    }
                    catch (Exception ex)
                    {
                        return id.ToString() + "|" + ex.Message;
                    }
                }
                else
                {
                    return "Please select the file to upload!";
                }
            }
            else
            {
                return "Access denied!";
            }
        }

        public string GetUploadResultHtml()
        {
            if (Session[Constants.UploadResultHtml] != null)
            {
                return Session[Constants.UploadResultHtml].ToString();
            }

            return string.Empty;
        }

        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult SaveImportedData(Guid id, string tableId)
        {
            if (tableId.IndexOf("|") > 0)
            {
                tableId = tableId.Substring(0, tableId.IndexOf("|"));
            }

            List<UploadResult> list = BulkCopyRule.GetUploadResult(id, tableId);
            return PartialView("UploadResult", list);
        }
    }
}
