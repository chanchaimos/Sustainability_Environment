<%@ WebHandler Language="C#" Class="ShowDeviate" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;

public class ShowDeviate : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        int nIncID = int.Parse(context.Request["nIncID"] + "");
        int nOprtID = int.Parse(context.Request["nOprtID"] + "");
        int nFacID = int.Parse(context.Request["nFacID"] + "");
        string sYear = context.Request["sYear"] + "";

        context.Response.Expires = -1;
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;

        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        ResultDetail result = new ResultDetail();
        List<TDataDetail> lstData = new List<TDataDetail>();
        if (!UserAcc.UserExpired())
        {
            int nFormID = 0;
            int nYearPrevious = 0;

            var lstForms = db.TEPI_Forms.Where(w => w.IDIndicator == nIncID && w.OperationTypeID == nOprtID && w.FacilityID == nFacID).ToList();
            if (lstForms.Count > 0)
            {
                var qFormsNow = lstForms.FirstOrDefault(w => w.sYear == sYear);
                if (qFormsNow != null)
                {
                    nFormID = qFormsNow.FormID;
                }
                //else
                //{
                //    nYearPrevious = int.Parse(sYear) - 1;
                //    var q = lstForms.FirstOrDefault(w => w.sYear == nYearPrevious.ToString());
                //    if (q != null) nFormID = q.FormID;
                //}
            }

            string sql = @"select wfd.FormID
                                       ,CASE
		                                WHEN wfd.Month = 1 THEN 'Jan'WHEN wfd.Month = 2 THEN 'Feb'WHEN wfd.Month = 3 THEN 'Mar'WHEN wfd.Month = 4 THEN 'Apr'
		                                WHEN wfd.Month = 5 THEN 'May'WHEN wfd.Month = 6 THEN 'Jun'WHEN wfd.Month = 7 THEN 'Jul'WHEN wfd.Month = 8 THEN 'Aug'
		                                WHEN wfd.Month = 9 THEN 'Sep'WHEN wfd.Month = 10 THEN 'Oct'WHEN wfd.Month = 11 THEN 'Nov'WHEN wfd.Month = 12 THEN 'Dec'
		                                ELSE '-'
	                                   END	 as sMonth   
                                       ,wfd.Month as nMonth
                                      --,wfd.sRemark      
                                      --,wfd.dAction
                                      --,CONVERT(varchar,ISNULL(wfd.dAction,0),103) +' '+CONVERT(varchar,ISNULL(wfd.dAction,0),108) as sDate	                                 
	                                  --,ISNULL(u.Firstname,'') +' '+ ISNULL(u.Lastname,'-') as sActionBy
                       from TEPI_Forms_Deviate wfd
                       left join mTProductIndicator p on p.ProductID = wfd.nProductID
                       left join mTUser u on u.ID = wfd.nActionBy
                       where wfd.FormID = " + nFormID + @"
                       group by wfd.FormID,wfd.Month--,wfd.sRemark,wfd.dAction,u.Firstname,u.Lastname
                       order by wfd.Month";
            System.Data.DataTable dt = CommonFunction.Get_Data(SystemFunction.strConnect, sql);
            lstData = CommonFunction.ConvertDatableToList<TDataDetail>(dt).ToList();
            var lstDataDeviate = db.TEPI_Forms_Deviate.Where(w => w.FormID == nFormID).ToList();
            var lstUesr = db.mTUser.Where(w => w.cDel != "Y").ToList();
            foreach (var i in lstData)
            {
                var Data = lstDataDeviate.OrderByDescending(o => o.dAction).FirstOrDefault(w => w.Month == i.nMonth);
                var dataUser = lstUesr.FirstOrDefault(w => w.ID == Data.nActionBy);
                if (Data != null)
                {
                    i.sDate = Data.dAction.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    i.sRemark = Data.sRemark;
                    i.sActionBy = dataUser != null ? dataUser.Firstname + " " + dataUser.Lastname : "-";
                }
                else
                {
                    i.sDate = "";
                    i.sRemark = "";
                    i.sActionBy = "";
                }
            }

            result.lstData = lstData.OrderByDescending(o => o.dAction).ToList();
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

    public class ResultDetail : sysGlobalClass.CResutlWebMethod
    {
        public List<TDataDetail> lstData { get; set; }
    }

    public class TDataDetail
    {
        public int FormID { get; set; }
        public string sMonth { get; set; }
        public int nMonth { get; set; }
        public int nProductID { get; set; }
        public string sRemark { get; set; }
        public DateTime dAction { get; set; }
        public string ProductName { get; set; }
        public string sActionBy { get; set; }
        public string sDate { get; set; }
    }

}