<%@ WebHandler Language="C#" Class="ShowDetailMonth" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;

public class ShowDetailMonth : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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

            string sql = @"select wf.nReportID
                              ,wf.FormID
	                          ,ISNULL(wfh.nLogID,0) as nLogID       
	                          ,CASE
			                        WHEN wf.nMonth = 1 THEN 'Jan'WHEN wf.nMonth = 2 THEN 'Feb'WHEN wf.nMonth = 3 THEN 'Mar'WHEN wf.nMonth = 4 THEN 'Apr'
			                        WHEN wf.nMonth = 5 THEN 'May'WHEN wf.nMonth = 6 THEN 'Jun'WHEN wf.nMonth = 7 THEN 'Jul'WHEN wf.nMonth = 8 THEN 'Aug'
			                        WHEN wf.nMonth = 9 THEN 'Sep'WHEN wf.nMonth = 10 THEN 'Oct'WHEN wf.nMonth = 11 THEN 'Nov'WHEN wf.nMonth = 12 THEN 'Dec'
			                        ELSE '-'
		                        END	 as sMonth
	                          ,wfh.nToStatusID
	                          ,ISNULL(wfh.sRemark,'-') as sRemark
	                          ,wfh.nActionBy
	                          ,wfh.dUpdate 
	                          ,ISNULL(s.sStatusName,'-') as sStatusName
	                          ,ISNULL(s.nLevelUse,-1) as nLevelUse
	                          ,ISNULL(s.cType,'-') as cType
	                          ,ISNULL(u.Firstname,'')+ ' ' + ISNULL(u.Lastname,'-') as sActionBy	  	
                        from TEPI_Workflow wf
                        LEFT join TEPI_Workflow_History wfh on wfh.nReportID = wf.nReportID
                        left join TStatus_Workflow s on s.nStatustID = wfh.nToStatusID
                        left join mTUser u on u.ID = wfh.nActionBy
                        where wf.FormID = " + nFormID + " and wfh.nLogID IS NOT NULL  ";
            System.Data.DataTable dt = CommonFunction.Get_Data(SystemFunction.strConnect, sql);
            lstData = CommonFunction.ConvertDatableToList<TDataDetail>(dt).ToList();

            foreach (var i in lstData)
            {
                i.sDate = i.dUpdate.ToShortDateString() == "1/1/0001" ? "-" : i.dUpdate.ToString("dd/MM/yyy hh:mm");
            }

            result.lstData = lstData.OrderByDescending(o => o.dUpdate).ToList();
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
        public int nReportID { get; set; }
        public int FormID { get; set; }
        public long nLogID { get; set; }
        public string sMonth { get; set; }
        public int nToStatusID { get; set; }
        public string sRemark { get; set; }
        public int nActionBy { get; set; }
        public DateTime dUpdate { get; set; }
        public string sStatusName { get; set; }
        public int nLevelUse { get; set; }
        public string cType { get; set; }
        public string sActionBy { get; set; }
        public string sDate { get; set; }
    }
}