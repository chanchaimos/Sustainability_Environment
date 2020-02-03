using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using sysExtension;
using System.Data;
using System.Globalization;

public partial class epi_mytask : System.Web.UI.Page
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
                SystemFunction.BindDropdownPageSize(ddlPageSize, null);

                int nRole = UserAcc.GetObjUser().nRoleID;
                if (nRole == 3 || nRole == 4)
                {
                    hdfPrmsMenu.Value = "2";
                }
                else
                {
                    hdfPrmsMenu.Value = "0";
                }

                BindDDL();
            }
        }
    }

    public void BindDDL()
    {
        SystemFunction.ListYearsDESC(ddlYear, "", "en-US", "th-TH", short.Parse(System.Configuration.ConfigurationSettings.AppSettings["startYear"].ToString()));

        int[] arrStatusID = new int[] { };
        string sqlFac = "";

        int nRole = UserAcc.GetObjUser().nRoleID;
        int nUserID = UserAcc.GetObjUser().nUserID;

        if (nRole == 3) // L1
        {
            arrStatusID = new int[] { 1 };
            sqlFac = @"SELECT B.ID 'nFacID',B.Name 'sFacName'
                        FROM mTWorkFlow A
                        INNER JOIN mTfacility B ON A.IDFac=B.ID
                        WHERE A.L1=" + nUserID + @"
                        AND B.cDel='N' AND B.cActive='Y' AND B.nLevel=2
                        GROUP BY B.ID,B.Name
                        ORDER BY B.Name";
        }
        else if (nRole == 4) // L2
        {
            arrStatusID = new int[] { 4, 2 }; //5, 9
            sqlFac = @"SELECT B.ID 'nFacID',B.Name 'sFacName'
                        FROM mTWorkFlow A
                        INNER JOIN mTfacility B ON A.IDFac=B.ID
                        WHERE A.L2=" + nUserID + @"
                        AND B.cDel='N' AND B.cActive='Y' AND B.nLevel=2
                        GROUP BY B.ID,B.Name
                        ORDER BY B.Name";
        }
        else if (nRole == 2) // L0
        {
            arrStatusID = new int[] { 0, 8, 9, 24, 18 }; //0, 8, 9, 24, 18 
            sqlFac = @"SELECT B.ID 'nFacID',B.Name 'sFacName'
                        FROM mTUser_FacilityPermission A
                        INNER JOIN mTfacility B ON A.nFacilityID=B.ID
                        WHERE A.nUserID=" + nUserID + @"AND A.nRoleID=2 AND A.nPermission IN (2)
                        AND B.cDel='N' AND B.cActive='Y' AND B.nLevel=2
                        GROUP BY B.ID,B.Name
                        ORDER BY B.Name";
        }


        ddlGroupIndicator.DataSource = db.mTIndicator.OrderBy(o => o.nOrder).ToList();
        ddlGroupIndicator.DataValueField = "ID";
        ddlGroupIndicator.DataTextField = "Indicator";
        ddlGroupIndicator.DataBind();
        ddlGroupIndicator.Items.Insert(0, new ListItem("- Select Indicator -", ""));

        ddlStatus.DataSource = db.TStatus_Workflow.Where(w => arrStatusID.Contains(w.nStatustID)).OrderBy(o => o.sStatusName).ToList();
        ddlStatus.DataValueField = "nStatustID";
        ddlStatus.DataTextField = "sStatusName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("- Select Status -", ""));

        if (nRole == 2)
        {
            ddlStatus.Items.Insert(1, new ListItem("No Action", "-1"));
        }


        SystemFunction.ListDBToDropDownList(ddlFacility, sqlFac, "- Select Sub-facility -", "nFacID", "sFacName");


    }

    #region class
    [Serializable]
    public class CSearch : sysGlobalClass.CommonLoadData
    {
        public string sSearch { get; set; }
        public int? nYear { get; set; }
        public int? nGroupIndID { get; set; }
        public string sStatus { get; set; }
        public string sPrms { get; set; }
        public int? nStatus { get; set; }
        public string sFacID { get; set; }
    }

    [Serializable]
    public class TRetunrLoadData : sysGlobalClass.Pagination
    {
        public List<Data_Perm> lstData { get; set; }
    }

    [Serializable]
    public class Data_Perm
    {
        public int nFacilities { get; set; }
        public int nIndicator { get; set; }
        public string sYear { get; set; }
        public int nL1 { get; set; }
        public int nL2 { get; set; }
        public int nFormID { get; set; }
        public int nMonth { get; set; }
        public string sMonth { get; set; }
        public int nReportID { get; set; }
        public string sNameFacilities { get; set; }
        public string sNameIndicator { get; set; }
        public int nStatus { get; set; }
        public string sStatus { get; set; }
        public string sBtn { get; set; }
        public int? nOperationtypeID { get; set; }
        public DateTime? dAction { get; set; }
    }
    #endregion

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData LoadData(CSearch itemSearch)
    {
        TRetunrLoadData result = new TRetunrLoadData();

        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities epi = new PTTGC_EPIEntities();
            List<Data_Perm> lstPerm = new List<Data_Perm>();
            int nRole = UserAcc.GetObjUser().nRoleID;
            int nUserID = UserAcc.GetObjUser().nUserID;
            string sSearch = CommonFunction.ReplaceInjection(itemSearch.sSearch.Trims().ToLower());
            int nFacID = 0;
            if (nRole == 3 || nRole == 4)
            {
                #region L1, L2
                string sCondition = "";
                if (itemSearch.nGroupIndID.HasValue)
                {
                    sCondition += " AND f.IDIndicator=" + itemSearch.nGroupIndID.Value;
                }

                if (!string.IsNullOrEmpty(sSearch))
                {
                    sCondition += " AND LOWER(mF.Name) LIKE '%" + sSearch + "%'";
                }

                if (itemSearch.nStatus.HasValue)
                {
                    sCondition += " AND d.nHistoryStatusID =" + itemSearch.nStatus;
                }

                if (!string.IsNullOrEmpty(itemSearch.sFacID))
                {
                    sCondition += " AND p.IDFac = " + itemSearch.sFacID;
                }

                #region SQL L1, L2
                string _Query = "SELECT" +
                                " p.IDFac AS nFacilities" +
                                " ,p.IDIndicator AS nIndicator" +
                                " ,p.L1 AS nL1" +
                                " ,p.L2 AS nL2 " +
                                " ,f.FormID AS nFormID" +
                                " ,f.sYear AS sYear" +
                                " ,d.nMonth AS nMonth" +
                                " ,d.nHistoryStatusID AS nStatus" +
                                " ,d.nReportID AS nReportID" +
                                " ,mF.Name AS sNameFacilities" +
                                " ,mI.Indicator AS sNameIndicator" +
                                " ,CASE " +
                                    " WHEN d.nMonth = 1 THEN 'Jan.'" +
                                    " WHEN d.nMonth = 2 THEN 'Feb.'" +
                                    " WHEN d.nMonth = 3 THEN 'Mar.'" +
                                    " WHEN d.nMonth = 4 THEN 'Apr.'" +
                                    " WHEN d.nMonth = 5 THEN 'May'" +
                                    " WHEN d.nMonth = 6 THEN 'Jun.'" +
                                    " WHEN d.nMonth = 7 THEN 'Jul.'" +
                                    " WHEN d.nMonth = 8 THEN 'Aug.'" +
                                    " WHEN d.nMonth = 9 THEN 'Sep.'" +
                                    " WHEN d.nMonth = 10 THEN 'Oct.'" +
                                    " WHEN d.nMonth = 11 THEN 'Nov.'" +
                                    " WHEN d.nMonth = 12 THEN 'Dec.'" +
                                    " ELSE 'Dec.'" +
                                " END sMonth" +
                                " ,d.nHistoryStatusID AS nStatus" +
                                " ,sf.sStatusName AS sStatus" +
                                " ,d.dAction" +
                                " FROM mTWorkFlow p" +
                                " INNER JOIN TEPI_Forms f ON f.FacilityID = p.IDFac AND f.IDIndicator = p.IDIndicator" +
                                " INNER JOIN TEPI_Workflow d ON d.FormID = f.FormID" +
                                " INNER JOIN mTFacility mF ON mF.ID = p.IDFac AND mF.cDel = 'N' AND mF.nLevel = 2" +
                                " INNER JOIN mTIndicator mI ON mI.ID = p.IDIndicator" +
                                " INNER JOIN TStatus_Workflow sf ON sf.nStatustID = d.nHistoryStatusID" +
                                " WHERE p.cDel = 'N' AND (p.L1 = " + nUserID + " OR p.L2 = " + nUserID + ") AND f.sYear=" + itemSearch.nYear + sCondition + " ORDER BY d.dAction DESC "; //f.sYear,d.nMonth,mF.Name,mI.Indicator
                #endregion

                lstPerm = epi.Database.SqlQuery<Data_Perm>(_Query).ToList();

                int[] lstStatusL1 = { 1 };
                int[] lstStatusL2 = { 4, 2 };
                int[] lstStatusL0 = { 0, 8, 9, 24, 18 };

                if (lstPerm.Any())
                {
                    if (nRole == 3)//L1
                    {
                        lstPerm = lstPerm.Where(w => w.nL1 == nUserID && lstStatusL1.Contains(w.nStatus)).OrderByDescending(o => o.dAction).ThenBy(o => o.sYear).ThenBy(o => o.nMonth).ThenBy(o => o.sNameFacilities).ThenBy(o => o.sNameIndicator).ToList();

                    }
                    else if (nRole == 4)//L2
                    {
                        lstPerm = lstPerm.Where(w => w.nL2 == nUserID && lstStatusL2.Contains(w.nStatus)).OrderByDescending(o => o.dAction).ThenBy(o => o.sYear).ThenBy(o => o.nMonth).ThenBy(o => o.sNameFacilities).ThenBy(o => o.sNameIndicator).ToList();

                    }
                }
                #endregion
            }
            else if (nRole == 2)
            {
                #region L0
                Func<int, string> GetMonthName = (month) =>
                {
                    string sMonthName = "";
                    if (month >= 1 && month <= 12)
                    {
                        sMonthName = new DateTime(DateTime.Now.Year, month, 1).ToString("MMM", new CultureInfo("en-US"));
                    }
                    else
                    {
                        sMonthName = month + "";
                    }
                    return sMonthName;
                };

                string sCondition = "";
                if (itemSearch.nGroupIndID.HasValue)
                {
                    sCondition += " AND TUF.nGroupIndicatorID=" + itemSearch.nGroupIndID.Value;
                }

                if (!string.IsNullOrEmpty(itemSearch.sFacID))
                {
                    nFacID = int.Parse(itemSearch.sFacID);
                }

                //if (itemSearch.nStatus.HasValue)
                //{
                //    sCondition += " AND TEPIWF.nHistoryStatusID =" + itemSearch.nStatus;
                //}

                #region sql
                string sqlL0 = @"SELECT TF.ID 'nFacilityID',TF.OperationtypeID,TF.Name 'sFacilityName',TIND.ID 'nIndID',TIND.Indicator 'sIndicator'
,TEPI.FormID,TEPIWF.nReportID,TEPIWF.nMonth,TEPIWF.nHistoryStatusID ,TSWF.sStatusName,TEPIWF.dAction
FROM mTUser_FacilityPermission  TUF
INNER JOIN mTFacility TF ON TUF.nFacilityID=TF.ID AND TF.nLevel=2
INNER JOIN mTFacility TFH ON TF.nHeaderID=TFH.ID AND TFH.nLevel=1
INNER JOIN mTIndicator TIND ON TUF.nGroupIndicatorID=TIND.ID
LEFT JOIN TEPI_Forms TEPI ON TUF.nGroupIndicatorID=TEPI.IDIndicator AND TF.ID=TEPI.FacilityID AND TF.OperationTypeID=TEPI.OperationTypeID AND TEPI.sYear='" + itemSearch.nYear + @"'
LEFT JOIN TEPI_Workflow TEPIWF ON TEPI.FormID=TEPIWF.FormID
LEFT JOIN TStatus_Workflow TSWF ON ISNULL(TEPIWF.nHistoryStatusID,0)=TSWF.nStatustID
WHERE TUF.nUserID=" + nUserID + " AND TUF.nRoleID=" + nRole + @" AND TUF.nPermission=2 AND TF.cDel='N' AND TF.cActive='Y' AND TFH.cDel='N' AND TFH.cActive='Y'
AND (TEPIWF.nHistoryStatusID IS NULL OR TEPIWF.nHistoryStatusID IN (0,8,9,24,18)) " + sCondition + @"
order by TEPIWF.dAction DESC";
                #endregion
                DataTable dt = new DataTable();
                dt = CommonFunction.Get_Data(SystemFunction.strConnect, sqlL0);
                var Query = dt.AsEnumerable().Select(s => new
                    {
                        nFacilityID = s.Field<int>("nFacilityID"),
                        OperationtypeID = s.Field<int>("OperationtypeID"),
                        sFacilityName = s.Field<string>("sFacilityName"),
                        nIndID = s.Field<int>("nIndID"),
                        sIndicator = s.Field<string>("sIndicator"),
                        FormID = s.Field<int?>("FormID"),
                        nReportID = s.Field<int?>("nReportID"),
                        nMonth = s.Field<int?>("nMonth"),
                        nStatusID = s.Field<int?>("nHistoryStatusID"),
                        sStatusName = s.Field<string>("sStatusName"),
                        dAction = s.Field<DateTime?>("dAction"),
                    });

                lstPerm = Query.Where(w => w.FormID.HasValue).Select(s => new Data_Perm
                {
                    nFacilities = s.nFacilityID,
                    nIndicator = s.nIndID,
                    sYear = itemSearch.nYear + "",
                    nFormID = s.FormID ?? 0,
                    nMonth = s.nMonth ?? 0,
                    sMonth = GetMonthName(s.nMonth ?? 0),
                    nReportID = s.nReportID ?? 0,
                    sNameFacilities = s.sFacilityName,
                    sNameIndicator = s.sIndicator,
                    nStatus = s.nStatusID ?? 0,
                    sStatus = s.sStatusName,
                    nOperationtypeID = s.OperationtypeID,
                    dAction = s.dAction
                }).ToList();

                var QueryNoAction = Query.Where(w => !w.FormID.HasValue).ToList();
                foreach (var item in QueryNoAction)
                {
                    for (var i = 1; i <= 12; i++)
                    {
                        lstPerm.Add(new Data_Perm
                            {
                                nFacilities = item.nFacilityID,
                                nOperationtypeID = item.OperationtypeID,
                                nIndicator = item.nIndID,
                                sYear = itemSearch.nYear + "",
                                nFormID = item.FormID ?? 0,
                                nMonth = i,
                                sMonth = GetMonthName(i),
                                nReportID = 0,
                                sNameFacilities = item.sFacilityName,
                                sNameIndicator = item.sIndicator,
                                nStatus = -1,
                                sStatus = "No Action",
                                dAction = null
                            });
                    }
                }

                lstPerm = lstPerm.Where(w => (itemSearch.nStatus != null ? w.nStatus == itemSearch.nStatus : true) && (!string.IsNullOrEmpty(itemSearch.sFacID) ? w.nFacilities == nFacID : true)).OrderByDescending(o => o.dAction).ThenBy(o => o.sYear).ThenBy(o => o.nMonth).ThenBy(o => o.sNameFacilities).ThenBy(o => o.sNameIndicator).ToList();
                #endregion
            }

            #region//SORT
            int? nSortCol = SystemFunction.GetIntNull(itemSearch.sIndexCol);
            switch ((itemSearch.sOrderBy + "").ToLower())
            {
                case SystemFunction.ASC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstPerm = lstPerm.OrderBy(o => int.Parse(o.sYear)).ToList(); break;
                            case 2: lstPerm = lstPerm.OrderBy(o => o.nMonth).ToList(); break;
                            case 3: lstPerm = lstPerm.OrderBy(o => o.sNameFacilities).ToList(); break;
                            case 4: lstPerm = lstPerm.OrderBy(o => o.sNameIndicator).ToList(); break;
                            //case 5: lstPerm = lstPerm.OrderBy(o => o.nStatus).ToList(); break;
                        }
                    }
                    break;
                case SystemFunction.DESC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstPerm = lstPerm.OrderByDescending(o => int.Parse(o.sYear)).ToList(); break;
                            case 2: lstPerm = lstPerm.OrderByDescending(o => o.nMonth).ToList(); break;
                            case 3: lstPerm = lstPerm.OrderByDescending(o => o.sNameFacilities).ToList(); break;
                            case 4: lstPerm = lstPerm.OrderByDescending(o => o.sNameIndicator).ToList(); break;
                            //case 5: lstPerm = lstPerm.OrderByDescending(o => o.nStatus).ToList(); break;
                        }
                    }
                    break;
            }
            #endregion

            #region//Final Action >> Skip Take Data For Javasacript
            sysGlobalClass.Pagination dataPage = new sysGlobalClass.Pagination();
            dataPage = SystemFunction.GetPaginationSmall(SystemFunction.GetIntNullToZero(itemSearch.sPageSize), SystemFunction.GetIntNullToZero(itemSearch.sPageIndex), lstPerm.Count);
            lstPerm = lstPerm.Skip(dataPage.nSkipData).Take(dataPage.nTakeData).ToList();
            foreach (var item in lstPerm)
            {
                if (nRole == 3 || nRole == 4)//L1,L2
                {
                    item.sBtn = "<a class='btn btn-primary' href='epi_mytask_view.aspx?strid=" +
                                    HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nFormID.ToString())) + "&strlevel=" +
                                    HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(nRole.ToString())) + "'>" +
                                    "<i class='fa fa-search'></i>&nbsp;View Detail</a>";
                }
                else if (nRole == 2)//L0
                {
                    string shref = SystemFunction.ReturnPath(item.nIndicator, item.nOperationtypeID ?? 0, item.nFacilities + "", item.sYear, "");
                    item.sBtn = "<a class='btn btn-warning btn-sm' title='Action' href='" + shref + "'><i class='fa fa-edit'></i></a>";
                }
            }

            result.lstData = lstPerm;
            result.nPageCount = dataPage.nPageCount;
            result.nPageIndex = dataPage.nPageIndex;
            result.sPageInfo = dataPage.sPageInfo;
            result.sContentPageIndex = dataPage.sContentPageIndex;
            result.nStartItemIndex = dataPage.nStartItemIndex;
            #endregion

        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod ApproveAll(List<DataApproveAll> arrValue)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities env = new PTTGC_EPIEntities();
            int nUserID = UserAcc.GetObjUser().nUserID;
            int nRoleID = UserAcc.GetObjUser().nRoleID;


            var gForm = arrValue.Select(s => s.nFormID).Distinct().ToList();
            int FormID = 0;
            if (gForm.Any())
            {
                gForm.ForEach(f =>
                {
                    FormID = f;
                    List<int> lstMonth = new List<int>();
                    arrValue.ForEach(f2 =>
                    {
                        if (f == f2.nFormID)
                        {
                            lstMonth.Add(f2.nMonth);
                        }
                    });

                    if (lstMonth.Any())
                    {
                        result = new Workflow().WorkFlowAction(FormID, lstMonth, "AP", nUserID, nRoleID, "");
                    }
                });
            }
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    [Serializable]
    public class DataApproveAll
    {
        public int nMonth { get; set; }
        public int nFormID { get; set; }
    }


}