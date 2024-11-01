<%@ WebHandler Language="C#" Class="UploadHandler" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.SessionState;

public class UploadHandler : System.Web.UI.Page, IHttpHandler, IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        new Nestle.Uploader.Helper.UploadHandler().ProcessRequest(context);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}