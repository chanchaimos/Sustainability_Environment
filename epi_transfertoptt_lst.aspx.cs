using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sysExtension;
using System.Data;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;

public partial class epi_transfertoptt_lst : System.Web.UI.Page
{
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnLoad(myFunc);
    }

    PTTGC_EPIEntities db = new PTTGC_EPIEntities();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (UserAcc.UserExpired())
        {
            SetBodyEventOnLoad(SystemFunction.PopupLogin());
        }
        else
        {
            if (!IsPostBack)
            {
                if (UserAcc.GetObjUser().nRoleID == 4 || UserAcc.GetObjUser().nRoleID == 1)//ENVI Corporate(L2) & System Admin
                {
                    BindDDL();
                }
                else
                {
                    SetBodyEventOnLoad(SystemFunction.DialogWarningRedirect(SystemFunction.Msg_HeadWarning, "No permission !", "epi_mytask.aspx"));
                }
            }
        }
    }

    private void BindDDL()
    {
        SystemFunction.ListYearsDESC(ddlYear, "", "en-US", "th-TH", short.Parse(System.Configuration.ConfigurationSettings.AppSettings["startYear"].ToString()));
        SystemFunction.BindDropdownPageSize(ddlPageSize, null);

        ddlGroupIndicator.DataSource = db.mTIndicator.OrderBy(o => o.nOrder).ToList();
        ddlGroupIndicator.DataValueField = "ID";
        ddlGroupIndicator.DataTextField = "Indicator";
        ddlGroupIndicator.DataBind();
        ddlGroupIndicator.Items.Insert(0, new ListItem("- Select Indicator -", ""));

        int[] arrWFStatus = new int[] { 28, 29, 30, 32, 33, 34 };
        ddlStatus.DataSource = db.TStatus_Workflow.Where(w => arrWFStatus.Contains(w.nStatustID) && w.cTypeUse == "TNF" && w.cActive == "Y").OrderBy(o => o.sShorttStatus).ToList();
        ddlStatus.DataValueField = "nStatustID";
        ddlStatus.DataTextField = "sShorttStatus";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("- Select Status -", ""));
        ddlStatus.Items.Insert(1, new ListItem("Waiting Transfer", "0"));

        ddlFacility.DataSource = (from fptt in db.mTFacility.Where(w => w.cDel == "N" && w.cActive == "Y" && w.nLevel == 0 && w.CompanyID == 1)
                                  from fgc in db.mTFacility.Where(w => w.cDel == "N" && w.cActive == "Y" && w.nHeaderID == fptt.ID)
                                  select new
                                  {
                                      fgc.ID,
                                      fgc.Name
                                  }).OrderBy(o => o.Name).ToList();
        ddlFacility.DataTextField = "Name";
        ddlFacility.DataValueField = "ID";
        ddlFacility.DataBind();
        ddlFacility.Items.Insert(0, new ListItem("- Select Facility -", ""));
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static CReturnLoadData LoadData(CSearch itemSearch)
    {
        CReturnLoadData result = new CReturnLoadData();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            int nRoleID = UserAcc.GetObjUser().nRoleID;
            List<TDataTable> lstData = new List<TDataTable>();
            int nYear = itemSearch.sYear.toIntNullToZero();

            #region SQL Sub-facility
            string _sConditonSubFac = "";
            if (!string.IsNullOrEmpty(itemSearch.sIndicatorID))
            {
                _sConditonSubFac += " AND TEPI.IDIndicator='" + CommonFunction.ReplaceInjection(itemSearch.sIndicatorID) + "'";
            }
            string sqlSubFac = @"SELECT T1.FormID,T1.sYear, ISNULL(T1.IDIndicator,0) 'IDIndicator',ISNULL(T1.OperationtypeID,0) 'OperationtypeID',ISNULL(T1.FacilityID,0) 'FacilityID',T1.sFacName,ISNULL(T1.nHeaderID,0) 'nHeaderID', T1.nQuarter FROM
(
	--Q1
	SELECT TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.Name 'sFacName',TFAC.nHeaderID,1 'nQuarter',CASE WHEN (COUNT(TWF.nMonth)) = 3 THEN 1 ELSE 0 END AS nStatus
	FROM TEPI_Forms TEPI
	INNER JOIN TEPI_Workflow TWF ON TEPI.FormID=TWF.FormID
	INNER JOIN mTFacility TFAC ON TEPI.FacilityID=TFAC.ID AND TEPI.OperationtypeID=TFAC.OperationTypeID AND TFAC.cActive='Y' AND TFAC.cDel='N'
	WHERE TEPI.sYear='{0}' AND TWF.nStatusID=5 AND TWF.nMonth IN (1,2,3) " + _sConditonSubFac + @"
	GROUP BY TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.nHeaderID,TFAC.Name
	UNION
	--Q2
	SELECT TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.Name 'sFacName',TFAC.nHeaderID,2 'nQuarter',CASE WHEN (COUNT(TWF.nMonth)) = 3 THEN 1 ELSE 0 END AS nStatus
	FROM TEPI_Forms TEPI
	INNER JOIN TEPI_Workflow TWF ON TEPI.FormID=TWF.FormID
	INNER JOIN mTFacility TFAC ON TEPI.FacilityID=TFAC.ID AND TEPI.OperationtypeID=TFAC.OperationTypeID AND TFAC.cActive='Y' AND TFAC.cDel='N'
	WHERE TEPI.sYear='{0}' AND TWF.nStatusID=5 AND TWF.nMonth IN (4,5,6) " + _sConditonSubFac + @"
	GROUP BY TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.nHeaderID,TFAC.Name
	UNION
	--Q3
	SELECT TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.Name 'sFacName',TFAC.nHeaderID,3 'nQuarter',CASE WHEN (COUNT(TWF.nMonth)) = 3 THEN 1 ELSE 0 END AS nStatus
	FROM TEPI_Forms TEPI
	INNER JOIN TEPI_Workflow TWF ON TEPI.FormID=TWF.FormID
	INNER JOIN mTFacility TFAC ON TEPI.FacilityID=TFAC.ID AND TEPI.OperationtypeID=TFAC.OperationTypeID AND TFAC.cActive='Y' AND TFAC.cDel='N'
	WHERE TEPI.sYear='{0}' AND TWF.nStatusID=5 AND TWF.nMonth IN (7,8,9) " + _sConditonSubFac + @"
	GROUP BY TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.nHeaderID,TFAC.Name
	UNION
	--Q4
	SELECT TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.Name 'sFacName',TFAC.nHeaderID,4 'nQuarter',CASE WHEN (COUNT(TWF.nMonth)) = 3 THEN 1 ELSE 0 END AS nStatus
	FROM TEPI_Forms TEPI
	INNER JOIN TEPI_Workflow TWF ON TEPI.FormID=TWF.FormID
	INNER JOIN mTFacility TFAC ON TEPI.FacilityID=TFAC.ID AND TEPI.OperationtypeID=TFAC.OperationTypeID AND TFAC.cActive='Y' AND TFAC.cDel='N'
	WHERE TEPI.sYear='{0}' AND TWF.nStatusID=5 AND TWF.nMonth IN (10,11,12) " + _sConditonSubFac + @"
	GROUP BY TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.nHeaderID,TFAC.Name
) AS T1
WHERE T1.nStatus=1
";
            #endregion
            string _sqlSub = string.Format(sqlSubFac, nYear);
            var lstDataSubFac = db.Database.SqlQuery<TDataSubFac>(_sqlSub).ToList();

            #region SQL Main facility
            string sqlMainFac = @"SELECT TFPTT.ID 'nPTTFacID',TFPTT.Name 'sPTTFacName',TFPTT.sMappingCodePTT
,TFGC.ID 'nGCFacID',TFGC.Name 'sGCFacName'
FROM mTFacility TFPTT
INNER JOIN mTFacility TFGC ON TFPTT.ID=TFGC.nHeaderID AND TFGC.nLevel=1 AND TFGC.cActive='Y' AND TFGC.cDel='N'
WHERE TFPTT.nLevel=0 and TFPTT.cActive='Y' and TFPTT.cDel='N'";
            #endregion
            var lstDataMainFac = db.Database.SqlQuery<TDataMainFaciltiy>(sqlMainFac).ToList();

            var dataStausWF = db.TStatus_Workflow.Where(w => w.cTypeUse == "TNF" && w.cActive == "Y").ToList();

            int? nIndID = itemSearch.sIndicatorID.toIntNull();
            int? nStatusID = itemSearch.sStatus.toIntNull();
            int? nFacID = itemSearch.sFacID.toIntNull();

            lstData = (from mf in lstDataMainFac
                       from sf in lstDataSubFac.Where(w => w.nHeaderID == mf.nGCFacID)
                       from ind in db.mTIndicator.Where(w => w.ID == sf.IDIndicator)
                       from wf in db.TEPI_TransferPTT.Where(w => w.nYear == nYear && w.nFacilityID == sf.nHeaderID && w.nIndicatorID == sf.IDIndicator && w.nQuarter == sf.nQuarter).DefaultIfEmpty()
                       from st in dataStausWF.Where(w => (wf != null ? w.nStatustID == wf.nStatusID : false)).DefaultIfEmpty()
                       group new { mf, sf, st, ind } by new { mf.nGCFacID, mf.sGCFacName, mf.sPTTFacName, mf.sMappingCodePTT, sf.nQuarter, nIndID = ind.ID, ind.Indicator, nStatustID = (st != null ? st.nStatustID : 0), sStatusName = (st != null ? st.sShorttStatus : "Waiting Transfer"), dAction = wf != null ? wf.dAction : null } into grp
                       select new TDataTable
                       {
                           nYear = nYear,
                           nGCFacID = grp.Key.nGCFacID,
                           sGCFacName = grp.Key.sGCFacName,
                           sPTTFacilityCode = grp.Key.sMappingCodePTT,
                           sPTTFacilityName = grp.Key.sPTTFacName,
                           nQuarter = grp.Key.nQuarter,
                           nIndicatorID = grp.Key.nIndID,
                           sIndicatorName = grp.Key.Indicator,
                           nStatus = grp.Key.nStatustID,
                           sStatusName = grp.Key.sStatusName,
                           dAction = grp.Key.dAction
                       }).Where(w => (nIndID.HasValue ? w.nIndicatorID == nIndID : true) && (nStatusID.HasValue ? w.nStatus == nStatusID : true) && (nFacID.HasValue ? w.nGCFacID == nFacID : true))
                       .OrderByDescending(o => o.dAction).ThenBy(o => o.nYear).ThenBy(o => o.sGCFacName).ThenBy(o => o.nQuarter).ThenBy(o => o.sIndicatorName).ToList();

            #region//SORT
            int? nSortCol = SystemFunction.GetIntNull(itemSearch.sIndexCol);
            switch ((itemSearch.sOrderBy + "").ToLower())
            {
                case SystemFunction.ASC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderBy(o => o.nYear).ToList(); break;
                            case 2: lstData = lstData.OrderBy(o => o.nQuarter).ToList(); break;
                            case 3: lstData = lstData.OrderBy(o => o.sGCFacName).ToList(); break;
                            case 4: lstData = lstData.OrderBy(o => o.sIndicatorName).ToList(); break;
                            case 5: lstData = lstData.OrderBy(o => o.sStatusName).ToList(); break;
                        }
                    }
                    break;
                case SystemFunction.DESC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderByDescending(o => o.nYear).ToList(); break;
                            case 2: lstData = lstData.OrderByDescending(o => o.nQuarter).ToList(); break;
                            case 3: lstData = lstData.OrderByDescending(o => o.sGCFacName).ToList(); break;
                            case 4: lstData = lstData.OrderByDescending(o => o.sIndicatorName).ToList(); break;
                            case 5: lstData = lstData.OrderByDescending(o => o.sStatusName).ToList(); break;
                        }
                    }
                    break;
            }
            #endregion

            #region//Final Action >> Skip Take Data For Javasacript
            sysGlobalClass.Pagination dataPage = new sysGlobalClass.Pagination();
            dataPage = SystemFunction.GetPaginationSmall(SystemFunction.GetIntNullToZero(itemSearch.sPageSize), SystemFunction.GetIntNullToZero(itemSearch.sPageIndex), lstData.Count);
            lstData = lstData.Skip(dataPage.nSkipData).Take(dataPage.nTakeData).ToList();

            foreach (var item in lstData)
            {
                if (!string.IsNullOrEmpty(item.sPTTFacilityCode))
                {
                    item.sBtnClass = "btn btn-primary";
                    item.sBtnText = "View Detail";
                    item.sWarningPTTCode = "N";
                    if (nRoleID == 4)//ENVI Corporate(L2)
                    {
                        if (item.nStatus == 0 || item.nStatus == 29 || item.nStatus == 33)//Waiting Submit/ Reject by PTT / Accept Edit Request by PTT
                        {
                            item.sLink = "epi_transfertoptt.aspx?stryear=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nYear + "")) + "&strfacid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nGCFacID + "")) + "&strindid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nIndicatorID + "")) + "&strq=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nQuarter + "")) + "&strmode=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt("S"));
                        }
                        else if (item.nStatus == 28 || item.nStatus == 30)//Submitted to ptt / Approve by PTT
                        {
                            item.sBtnClass = "btn btn-warning";
                            item.sBtnText = "Request Edit";
                            item.sLink = "epi_transfertoptt.aspx?stryear=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nYear + "")) + "&strfacid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nGCFacID + "")) + "&strindid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nIndicatorID + "")) + "&strq=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nQuarter + "")) + "&strmode=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt("R"));
                        }
                        else
                        {
                            item.sLink = "epi_transfertoptt.aspx?stryear=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nYear + "")) + "&strfacid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nGCFacID + "")) + "&strindid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nIndicatorID + "")) + "&strq=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt("0")) + "&strmode=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt("V"));
                        }
                    }
                    else
                    {
                        item.sLink = "epi_transfertoptt.aspx?stryear=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nYear + "")) + "&strfacid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nGCFacID + "")) + "&strindid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nIndicatorID + "")) + "&strq=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt("0")) + "&strmode=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt("V"));
                    }
                }
                else
                {
                    item.sWarningPTTCode = "Y";
                }
            }

            result.lstData = lstData;
            result.nPageCount = dataPage.nPageCount;
            result.nPageIndex = dataPage.nPageIndex;
            result.sPageInfo = dataPage.sPageInfo;
            result.sContentPageIndex = dataPage.sContentPageIndex;
            result.nStartItemIndex = dataPage.nStartItemIndex;
            #endregion
        }
        else
        {
            result.Status = SystemFunction.process_Failed;
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static CResultHistory ViewHistory(int nFacID, int nYear, int nIndID, int nQuarter)
    {
        CResultHistory result = new CResultHistory();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            List<TDataHistory> lstData = new List<TDataHistory>();
            lstData = (from d in db.TEPI_TransferPTT_Log.Where(w => w.nFacilityID == nFacID && w.nYear == nYear && w.nIndicatorID == nIndID && w.nQuarter == nQuarter)
                       from u in db.mTUser.Where(w => w.ID == d.nActionBy).DefaultIfEmpty()
                       from st in db.TStatus_Workflow.Where(w => w.nStatustID == d.nStatusID && w.cActive == "Y" && w.cTypeUse == "TNF").DefaultIfEmpty()
                       orderby d.dAction descending
                       select new TDataHistory
                       {
                           dAction = d.dAction,
                           sStatus = st != null ? st.sStatusName : "-",
                           sActionBy = u != null ? u.Firstname + " " + u.Lastname : d.nActionBy == -1 ? "PTT" : "",
                           sComment = d.sRemark
                       }).ToList();
            foreach (var item in lstData)
            {
                item.sDate = item.dAction.DateString();
                item.sComment = (item.sComment + "").Replace("\n", "<br/>");
            }
            result.lstData = lstData;
            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    #region class
    [Serializable]
    public class CSearch : sysGlobalClass.CommonLoadData
    {
        public string sYear { get; set; }
        public string sIndicatorID { get; set; }
        public string sStatus { get; set; }
        public string sFacID { get; set; }
    }

    [Serializable]
    public class CReturnLoadData : sysGlobalClass.Pagination
    {
        public List<TDataTable> lstData { get; set; }
    }

    [Serializable]
    public class TDataTable
    {
        public int nYear { get; set; }
        public int nQuarter { get; set; }
        public int nGCFacID { get; set; }
        public string sGCFacName { get; set; }
        public int nIndicatorID { get; set; }
        public string sIndicatorName { get; set; }
        public int nStatus { get; set; }
        public string sStatusName { get; set; }
        public DateTime? dAction { get; set; }
        public string sLink { get; set; }
        public string sBtnClass { get; set; }
        public string sBtnText { get; set; }

        public string sMode { get; set; }
        public string sWarningPTTCode { get; set; }
        public string sPTTFacilityName { get; set; }
        public string sPTTFacilityCode { get; set; }
    }

    [Serializable]
    public class TDataSubFac
    {
        public int FormID { get; set; }
        public string sYear { get; set; }
        public int IDIndicator { get; set; }
        public int OperationtypeID { get; set; }
        public int FacilityID { get; set; }
        public string sFacName { get; set; }
        public int nHeaderID { get; set; }
        public int nQuarter { get; set; }
    }

    [Serializable]
    public class TDataMainFaciltiy
    {
        public int nPTTFacID { get; set; }
        public string sPTTFacName { get; set; }
        public string sMappingCodePTT { get; set; }
        public int nGCFacID { get; set; }
        public string sGCFacName { get; set; }
    }
    #endregion

    [Serializable]
    public class CResultHistory : sysGlobalClass.CResutlWebMethod
    {
        public List<TDataHistory> lstData { get; set; }
    }

    [Serializable]
    public class TDataHistory
    {
        public DateTime? dAction { get; set; }
        public string sDate { get; set; }
        public string sStatus { get; set; }
        public string sActionBy { get; set; }
        public string sComment { get; set; }
    }
}