<%@ WebHandler Language="C#" Class="GetMonthStatus" %>

using System;
using System.Web;

public class GetMonthStatus : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        int status = int.Parse(context.Request["status"] + "");
        int nIncID = int.Parse(context.Request["nIncID"] + "");
        int nOprtID = int.Parse(context.Request["nOprtID"] + "");
        int nFacID = int.Parse(context.Request["nFacID"] + "");
        string sYear = context.Request["sYear"] + "";
        context.Response.Expires = -1;
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        sysGlobalClass.CGetStatusMonth result = new sysGlobalClass.CGetStatusMonth();
        if (!UserAcc.UserExpired())
        {
            result.lstMonth = new SystemFunction().GetMontStatus(status, nIncID, nOprtID, nFacID, sYear);
            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = 2147483644 };
        string res = serializer.Serialize(result);//new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ob);

        context.Response.Write(res);
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}