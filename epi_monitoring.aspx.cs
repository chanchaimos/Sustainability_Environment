using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class epi_monitoring : System.Web.UI.Page
{
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnLoad(myFunc);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SetBodyEventOnLoad("");
        if (UserAcc.UserExpired())
        {
            SetBodyEventOnLoad(SystemFunction.PopupLogin());
        }
        else
        {
            if (!IsPostBack)
            {
                PTTGC_EPIEntities db = new PTTGC_EPIEntities();
                SystemFunction.BindDropdownPageSize(ddlPageSize, null);
                // SystemFunction.BindDropdownPageSize(ddlPageSizeDetail, null);

                SystemFunction.ListYearsDESC(ddlYear, "-select-", "en-US", "th-TH", short.Parse(ConfigurationSettings.AppSettings["startYear"].ToString()));
                // BindData();
            }
        }
    }

    public void BindData()
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        SetPageLoadMaster o = new SetPageLoadMaster();

        var lstIn = db.mTIndicator.ToList();
        o.lstIn = new List<T_mTIndicator>();
        bool IsSys = SystemFunction.IsSuperAdmin();
        int nUser = UserAcc.GetObjUser().nUserID;
        int nRoleID = UserAcc.GetObjUser().nRoleID;
        var lstUser = db.mTUser_FacilityPermission.Where(a => a.nUserID == nUser && a.nRoleID == nRoleID).ToList();
        var lstFlow = db.mTWorkFlow.Where(a => ((a.L1 == nUser && nRoleID == 3) || (a.L2 == nUser && nRoleID == 4))).ToList();
        var lstFacility = new List<T_mTFacility>();
        var lsInt = new List<T_mTIndicator>();
        //var lstOT = new T_mOperationType();
        if (lstIn.Count > 0)
        {
            var lb_1 = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 1).ToList();
            var lb_2 = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 2).ToList();

            var lstFacitltyTemp = (from a in lb_1
                                   from b in lb_2.Where(w => w.nHeaderID == a.ID)
                                   select new T_mTFacility
                                   {
                                       ID = b.ID,
                                       Name = b.Name,
                                       OperationTypeID = b.OperationTypeID,
                                   }).ToList();

            if (!IsSys)
            {
                var lstInUser = (from a in lstIn
                                 from b in lstUser.Where(w => w.nGroupIndicatorID == a.ID)
                                 select new T_mTIndicator
                                 {
                                     ID = a.ID,
                                     Indicator = a.Indicator,
                                     nOrder = a.nOrder,
                                     lstProIn = new List<T_mTProductIndicator>(),
                                 }).ToList();
                var lstInFlow = (from a in lstIn
                                 from b in lstFlow.Where(w => w.IDIndicator == a.ID)
                                 select new T_mTIndicator
                                 {
                                     ID = a.ID,
                                     Indicator = a.Indicator,
                                     nOrder = a.nOrder,
                                     lstProIn = new List<T_mTProductIndicator>(),
                                 }).ToList();
                var lstFaUser = (from a in lstFacitltyTemp.AsEnumerable()
                                 from b in lstUser.Where(w => w.nFacilityID == a.ID)
                                 select new T_mTFacility
                                 {
                                     ID = a.ID,
                                     Name = a.Name,
                                     OperationTypeID = a.OperationTypeID,
                                     nGroupIndicatorID = b.nGroupIndicatorID,
                                 }).ToList();
                var lstFaFlow = (from a in lstFacitltyTemp.AsEnumerable()
                                 from b in lstFlow.Where(w => w.IDFac == a.ID).AsEnumerable()
                                 select new T_mTFacility
                                 {
                                     ID = a.ID,
                                     Name = a.Name,
                                     OperationTypeID = a.OperationTypeID,
                                     nGroupIndicatorID = b.IDIndicator,
                                 }).ToList();
                var lstInGroup = lstInUser.Concat(lstInFlow).GroupBy(g => new { g.sPath, g.ID, g.Indicator, g.nOrder }).ToList();
                var lstFacGroup = lstFaUser.Concat(lstFaFlow).GroupBy(a => new { a.ID, a.Name, a.OperationTypeID, a.nGroupIndicatorID }).ToList();
                o.lstIn = lstInGroup.Select(g => new T_mTIndicator
                {

                    ID = g.Key.ID,
                    Indicator = g.Key.Indicator,
                    nOrder = g.Key.nOrder,
                    lstProIn = new List<T_mTProductIndicator>(),
                    sPath = SystemFunction.ReturnPath(g.Key.ID, 0, "", "", "")
                }).ToList();
                lstFacility = lstFacGroup.Select(a => new T_mTFacility
                {
                    ID = a.Key.ID,
                    Name = a.Key.Name,
                    OperationTypeID = a.Key.OperationTypeID,
                    nGroupIndicatorID = a.Key.nGroupIndicatorID
                }).ToList();
            }
            else
            {
                o.lstIn = lstIn.Select(s => new T_mTIndicator
                {
                    ID = s.ID,
                    Indicator = s.Indicator,
                    nOrder = s.nOrder,
                    lstProIn = new List<T_mTProductIndicator>(),
                    sPath = SystemFunction.ReturnPath(s.ID, 0, "", "", ""),
                }).ToList();


                lstFacility = lstFacitltyTemp.Select(s => new T_mTFacility
                {
                    ID = s.ID,
                    Name = s.Name,
                    OperationTypeID = s.OperationTypeID,
                    nGroupIndicatorID = 0,
                }).ToList();
            }

            var lstOperationType = db.mOperationType.Where(w => w.cManage == "N" && w.cDel == "N" && w.cActive == "Y").Select(s => new T_mOperationType
            {
                ID = s.ID,
                Name = s.Name,
                sCode = s.sCode
            }).ToList();

            var lstOTGroup = (from a in o.lstIn
                              from b in lstOperationType
                              from c in lstFacility.Where(w => w.OperationTypeID == b.ID)
                              select new T_mOperationType
                              {
                                  ID = b.ID,
                                  Name = b.Name,
                                  sCode = b.sCode,
                                  nIndicator = a.ID,
                                  sPath = SystemFunction.ReturnPath(a.ID, b.ID, "", "", "")
                              }).ToList();
            var lstOL = lstOTGroup.GroupBy(g => new { g.ID, g.Name, g.sCode, g.nIndicator, g.sPath }).ToList();
            var lstOT = lstOL.Select(s => new T_mOperationType
            {
                ID = s.Key.ID,
                Name = s.Key.Name,
                sCode = s.Key.sCode,
                nIndicator = s.Key.nIndicator,
                sPath = s.Key.sPath
            }).ToList();
            var lst = new List<T_mTFacility>();


            o.lstIn.ForEach(f =>
            {
                if (lstOperationType.Count > 0)
                {

                    lstOT.Where(w => w.nIndicator == f.ID).ToList().ForEach(f2 =>
                    {
                        f2.lstFacility = (lstFacility.Where(w => w.OperationTypeID == f2.ID && f.ID == w.nGroupIndicatorID).Count() > 0) || (IsSys == true && lstFacility.Where(w => w.OperationTypeID == f2.ID).Count() > 0) ? IsSys == true ? lstFacility.Where(w => w.OperationTypeID == f2.ID).ToList() : lstFacility.Where(w => w.OperationTypeID == f2.ID && f.ID == w.nGroupIndicatorID).ToList() : new List<T_mTFacility>();
                    });
                    f.lstOperationType = lstOT.Where(w => w.nIndicator == f.ID).ToList();
                }

            });

            //ddlIndicator.DataSource = o.lstIn;
            //ddlIndicator.DataValueField = "ID";
            //ddlIndicator.DataTextField = "Indicator";
            //ddlIndicator.DataBind();
            //ddlIndicator.Items.Insert(0, new ListItem("- All Indicator -", ""));

            //ddlFacility.DataSource = lstFacility;
            //ddlFacility.DataValueField = "ID";
            //ddlFacility.DataTextField = "Name";
            //ddlFacility.DataBind();
            //ddlFacility.Items.Insert(0, new ListItem("- All Facility -", ""));

            //ddlOperationType.DataSource = lstOT;
            //ddlOperationType.DataValueField = "ID";
            //ddlOperationType.DataTextField = "Name";
            //ddlOperationType.DataBind();
            //ddlOperationType.Items.Insert(0, new ListItem("- All OperationType -", ""));
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData LoadData(CSearch itemSearch, string sStatus)
    {
        TRetunrLoadData result = new TRetunrLoadData();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            List<TDataTable> lstData = new List<TDataTable>();
            int nUser = UserAcc.GetObjUser().nUserID;
            int nRoleID = UserAcc.GetObjUser().nRoleID;

            #region SQL
            string sCon = "";
            string sConAd = "";

            if (!string.IsNullOrEmpty(itemSearch.sFacilityID))
            {
                sCon += @" and it.nFacID = " + CommonFunction.ReplaceInjection(itemSearch.sFacilityID) + "";
                sConAd += @" and FacilityID = " + CommonFunction.ReplaceInjection(itemSearch.sFacilityID) + "";
            }

            if (!string.IsNullOrEmpty(itemSearch.sIDIndicator))
            {
                sCon += @" and it.nIndicator = " + CommonFunction.ReplaceInjection(itemSearch.sIDIndicator) + "";
                sConAd += @" and IDIndicator = " + CommonFunction.ReplaceInjection(itemSearch.sIDIndicator) + "";
            }

            if (!string.IsNullOrEmpty(itemSearch.sOperationTypeID))
            {
                sCon += @" and it.nOpID = " + CommonFunction.ReplaceInjection(itemSearch.sOperationTypeID) + "";
                sConAd += @" and OperationTypeID = " + CommonFunction.ReplaceInjection(itemSearch.sOperationTypeID) + "";
            }

            //if (!string.IsNullOrEmpty(itemSearch.sYear))
            //{
            //    sCon += @" and epi.sYear = " + CommonFunction.ReplaceInjection(itemSearch.sYear) + "";
            //}

            #region OLD
            //            string _SQL = @"select  epi.FormID
            //                                    ,epi.sYear
            //                                    ,epi.IDIndicator
            //                                    ,epi.OperationTypeID
            //                                    ,epi.FacilityID
            //                                    ,i.Indicator as sIndicator
            //                                    ,f.Name as sFacilityName
            //                                    ,o.Name as sOperationTypeName
            //                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 1),-1) as nM1
            //                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 2),-1) as nM2
            //                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 3),-1) as nM3
            //                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 4),-1) as nM4
            //                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 5),-1) as nM5
            //                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 6),-1) as nM6
            //                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 7),-1) as nM7
            //                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 8),-1) as nM8
            //                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 9),-1) as nM9
            //                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 10),-1) as nM10
            //                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 11),-1) as nM11
            //                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 12),-1) as nM12
            //                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 1),0) as nReportID1
            //                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 2),0) as nReportID2
            //                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 3),0) as nReportID3
            //                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 4),0) as nReportID4
            //                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 5),0) as nReportID5
            //                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 6),0) as nReportID6
            //                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 7),0) as nReportID7
            //                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 8),0) as nReportID8
            //                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 9),0) as nReportID9
            //                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 10),0) as nReportID10
            //                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 11),0) as nReportID11
            //                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 12),0) as nReportID12
            //                            from TEPI_Forms epi
            //                            INNER JOIN mTIndicator i on i.ID = epi.IDIndicator
            //                            INNER JOIN mTFacility f on f.ID = epi.FacilityID 
            //                            INNER JOIN mOperationType o on o.ID = epi.OperationTypeID 	
            //                            where o.cManage = 'N' and f.nLevel = 2 " + sCon + @"";
            #endregion

            string _SQL = "";

            if (SystemFunction.IsSuperAdmin())
            {
                #region SUPER ADMIN
                _SQL = @" select *
                          from  ( SELECT ot.ID as OperationTypeID,ot.Name as sOperationTypeName,f.ID as FacilityID,f.Name as sFacilityName, id.ID  as IDIndicator,id.Indicator as sIndicator FROM mTUser u
                                INNER JOIN mTUserInRole ur ON u.ID = ur.nUID
                                LEFT JOIN mTUser_FacilityPermission uf ON ur.nUID = uf.nUserID AND ur.nRoleID = uf.nRoleID
                                LEFT JOIN mTIndicator id ON id.ID = uf.nGroupIndicatorID 
                                INNER JOIN mOperationType AS ot ON ot.cManage = 'N' AND ot.cDel = 'N' AND ot.cActive = 'Y' 
                                INNER JOIN mTFacility AS f ON f.cActive = 'Y' AND f.cDel = 'N' AND f.nLevel = 2 AND ot.ID = f.OperationTypeID 
								where  id.ID IS NOT NULL 
                                UNION
                                SELECT ot.ID as OperationTypeID,ot.Name as sOperationTypeName,f.ID as FacilityID,f.Name as sFacilityName, id.ID  as IDIndicator,id.Indicator as sIndicator FROM mTUser u
                                INNER JOIN mTUserInRole ur ON u.ID = ur.nUID
                                LEFT JOIN mTWorkFlow wf ON ur.nUID = wf.L1 AND ur.nRoleID = 3
                                LEFT JOIN mTWorkFlow wf2 ON ur.nUID = wf2.L2 AND ur.nRoleID = 4
                                INNER JOIN mTIndicator id ON id.ID = wf.IDIndicator OR id.ID = wf2.IDIndicator
                                INNER JOIN mOperationType AS ot ON ot.cManage = 'N' AND ot.cDel = 'N' AND ot.cActive = 'Y' 
                                INNER JOIN mTFacility AS f ON f.cActive = 'Y' AND f.cDel = 'N' AND f.nLevel = 2 AND ot.ID = f.OperationTypeID 
								where  id.ID IS NOT NULL 
								group by ot.ID,ot.Name,f.ID,f.Name,id.ID,id.Indicator) it
                            where 1=1 " + sConAd + @"
                            order by sOperationTypeName,sFacilityName,sIndicator";
                //DataTable dt = CommonFunction.Get_Data(SystemFunction.strConnect, _SQL);
                //lstData = CommonFunction.ConvertDatableToList<TDataTable>(dt).ToList();
                //bool isYear = !string.IsNullOrEmpty(itemSearch.sYear) ? true : false;
                //var lstEPI = db.TEPI_Forms.ToList();
                //foreach (var i in lstData)
                //{
                //    var nFromID = 0;

                //    var q = lstEPI.FirstOrDefault(w => w.OperationTypeID == i.OperationTypeID && w.FacilityID == i.FacilityID && w.IDIndicator == i.IDIndicator && (isYear ? w.sYear == itemSearch.sYear : true));
                //    if (q != null)
                //    {
                //        nFromID = q.FormID;
                //    }

                //    if (nFromID != 0)
                //    {
                //        i.nM1 = CheckValMonth(nFromID, 1); i.nM2 = CheckValMonth(nFromID, 2); i.nM3 = CheckValMonth(nFromID, 3); i.nM4 = CheckValMonth(nFromID, 4);
                //        i.nM5 = CheckValMonth(nFromID, 5); i.nM6 = CheckValMonth(nFromID, 6);
                //        i.nM7 = CheckValMonth(nFromID, 7); i.nM8 = CheckValMonth(nFromID, 8); i.nM9 = CheckValMonth(nFromID, 9); i.nM10 = CheckValMonth(nFromID, 10);
                //        i.nM11 = CheckValMonth(nFromID, 11); i.nM12 = CheckValMonth(nFromID, 12);

                //        i.nReportID1 = CheckValReportID(nFromID, 1);
                //        i.nReportID2 = CheckValReportID(nFromID, 2); i.nReportID3 = CheckValReportID(nFromID, 3); i.nReportID4 = CheckValReportID(nFromID, 4);
                //        i.nReportID5 = CheckValReportID(nFromID, 5); i.nReportID6 = CheckValReportID(nFromID, 6);
                //        i.nReportID7 = CheckValReportID(nFromID, 7); i.nReportID8 = CheckValReportID(nFromID, 8); i.nReportID9 = CheckValReportID(nFromID, 9); i.nReportID10 = CheckValReportID(nFromID, 10);
                //        i.nReportID11 = CheckValReportID(nFromID, 11); i.nReportID12 = CheckValReportID(nFromID, 12);
                //    }
                //    else
                //    {
                //        i.nM1 = -1; i.nM2 = -1; i.nM3 = -1; i.nM4 = -1; i.nM5 = -1; i.nM6 = -1;
                //        i.nM7 = -1; i.nM8 = -1; i.nM9 = -1; i.nM10 = -1; i.nM11 = -1; i.nM12 = -1;
                //    }
                //}
                #endregion
            }
            else
            {
                #region NO Admin
                #region OLD
                //                _SQL = @"SELECT it.nOpID as OperationTypeID, it.nIndicator as IDIndicator,it.nFacID as FacilityID
                //                                ,it.nUserID, it.nRoleID, it.nOrder
                //                                ,it.sOpName as sOperationTypeName ,it.sFacName as sFacilityName,it.sIdName as sIndicator
                //                                ,epi.FormID
                //                                ,epi.sYear
                //                                ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 1),-1) as nM1
                //                                ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 2),-1) as nM2
                //                                                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 3),-1) as nM3
                //                                                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 4),-1) as nM4
                //                                                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 5),-1) as nM5
                //                                                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 6),-1) as nM6
                //                                                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 7),-1) as nM7
                //                                                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 8),-1) as nM8
                //                                                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 9),-1) as nM9
                //                                                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 10),-1) as nM10
                //                                                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 11),-1) as nM11
                //                                                                    ,ISNULL((select nStatusID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 12),-1) as nM12
                //                                                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 1),0) as nReportID1
                //                                                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 2),0) as nReportID2
                //                                                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 3),0) as nReportID3
                //                                                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 4),0) as nReportID4
                //                                                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 5),0) as nReportID5
                //                                                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 6),0) as nReportID6
                //                                                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 7),0) as nReportID7
                //                                                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 8),0) as nReportID8
                //                                                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 9),0) as nReportID9
                //                                                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 10),0) as nReportID10
                //                                                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 11),0) as nReportID11
                //                                                                    ,ISNULL((select nReportID  from TEPI_Workflow where FormID = epi.FormID and nMonth = 12),0) as nReportID12
                //                                 FROM (
                //                                SELECT ot.ID as nOpID,ot.Name AS sOpName,f.ID AS nFacID,f.Name AS sFacName, id.ID AS nIndicator,id.Indicator as sIdName, ur.nUID nUserID, ur.nRoleID, id.nOrder FROM mTUser u
                //                                INNER JOIN mTUserInRole ur ON u.ID = ur.nUID
                //                                LEFT JOIN mTUser_FacilityPermission uf ON ur.nUID = uf.nUserID AND ur.nRoleID = uf.nRoleID
                //                                LEFT JOIN mTIndicator id ON id.ID = uf.nGroupIndicatorID 
                //                                INNER JOIN mOperationType AS ot ON ot.cManage = 'N' AND ot.cDel = 'N' AND ot.cActive = 'Y' 
                //                                INNER JOIN mTFacility AS f ON f.cActive = 'Y' AND f.cDel = 'N' AND f.nLevel = 2 AND ot.ID = f.OperationTypeID 
                //                                UNION
                //                                SELECT ot.ID as nOpID,ot.Name AS sOpName,f.ID AS nFacID,f.Name AS sFacName, id.ID AS nIndicator,id.Indicator as sIdName, ur.nUID nUserID, ur.nRoleID, id.nOrder FROM mTUser u
                //                                INNER JOIN mTUserInRole ur ON u.ID = ur.nUID
                //                                LEFT JOIN mTWorkFlow wf ON ur.nUID = wf.L1 AND ur.nRoleID = 3
                //                                LEFT JOIN mTWorkFlow wf2 ON ur.nUID = wf2.L2 AND ur.nRoleID = 4
                //                                INNER JOIN mTIndicator id ON id.ID = wf.IDIndicator OR id.ID = wf2.IDIndicator
                //                                INNER JOIN mOperationType AS ot ON ot.cManage = 'N' AND ot.cDel = 'N' AND ot.cActive = 'Y' 
                //                                INNER JOIN mTFacility AS f ON f.cActive = 'Y' AND f.cDel = 'N' AND f.nLevel = 2 AND ot.ID = f.OperationTypeID 
                //                                )
                //                                it
                //                                Left JOIN TEPI_Forms epi on epi.IDIndicator = it.nIndicator and epi.OperationTypeID =it.nOpID and epi.FacilityID = it.nFacID
                //                                where it.nUserID = " + nUser + " and it.nRoleID = " + nRoleID + " " + sCon + @"
                //                                GROUP BY it.nOpID, it.nIndicator,it.nFacID, it.nUserID, it.nRoleID, it.nOrder ,it.sOpName,it.sFacName,it.sIdName,epi.FormID,epi.sYear";
                #endregion

                _SQL = @"SELECT it.nOpID as OperationTypeID, it.nIndicator as IDIndicator,it.nFacID as FacilityID
                                ,it.nUserID, it.nRoleID, it.nOrder
                                ,it.sOpName as sOperationTypeName ,it.sFacName as sFacilityName,it.sIdName as sIndicator              
                                 FROM (
                                SELECT ot.ID as nOpID,ot.Name AS sOpName,f.ID AS nFacID,f.Name AS sFacName, id.ID AS nIndicator,id.Indicator as sIdName, ur.nUID nUserID, ur.nRoleID, id.nOrder FROM mTUser u
                                INNER JOIN mTUserInRole ur ON u.ID = ur.nUID
                                LEFT JOIN mTUser_FacilityPermission uf ON ur.nUID = uf.nUserID AND ur.nRoleID = uf.nRoleID
                                LEFT JOIN mTIndicator id ON id.ID = uf.nGroupIndicatorID 
                                LEFT JOIN mOperationType AS ot ON ot.cManage = 'N' AND ot.cDel = 'N' AND ot.cActive = 'Y' 
								INNER JOIN mTFacility as f1 on f1.nLevel = 1 and f1.cDel = 'N' AND ot.ID = f1.OperationTypeID
                                INNER JOIN mTFacility AS f ON f.cActive = 'Y' AND f.cDel = 'N' AND f.nLevel = 2  and f.nHeaderID = f1.ID AND uf.nFacilityID = f.ID   
								--where ur.nUID = 12 and ur.nRoleID = 2
                                UNION
                                SELECT ot.ID as nOpID,ot.Name AS sOpName,f.ID AS nFacID,f.Name AS sFacName, id.ID AS nIndicator,id.Indicator as sIdName, ur.nUID nUserID, ur.nRoleID, id.nOrder FROM mTUser u
                                INNER JOIN mTUserInRole ur ON u.ID = ur.nUID
                                LEFT JOIN mTWorkFlow wf ON ur.nUID = wf.L1 AND ur.nRoleID = 3
                                LEFT JOIN mTWorkFlow wf2 ON ur.nUID = wf2.L2 AND ur.nRoleID = 4
                                INNER JOIN mTIndicator id ON id.ID = wf.IDIndicator OR id.ID = wf2.IDIndicator
                                INNER JOIN mOperationType AS ot ON ot.cManage = 'N' AND ot.cDel = 'N' AND ot.cActive = 'Y' 
								INNER JOIN mTFacility as f1 on f1.nLevel = 1 and f1.cDel = 'N' 
                                INNER JOIN mTFacility AS f ON f.cActive = 'Y' AND f.cDel = 'N' AND f.nLevel = 2  and f.nHeaderID = f1.ID AND ot.ID = f1.OperationTypeID
								--where ur.nUID = 12 and ur.nRoleID = 2
                                )
                                it                             
                                where it.nUserID = " + nUser + @" and it.nRoleID = " + nRoleID + @" " + sCon + @" 
                                GROUP BY it.nOpID, it.nIndicator,it.nFacID, it.nUserID, it.nRoleID, it.nOrder ,it.sOpName,it.sFacName,it.sIdName
								order by it.sOpName,it.sFacName,it.sIdName";
                #endregion
            }


            DataTable dt = CommonFunction.Get_Data(SystemFunction.strConnect, _SQL);
            lstData = CommonFunction.ConvertDatableToList<TDataTable>(dt).ToList();
            bool isYear = !string.IsNullOrEmpty(itemSearch.sYear) ? true : false;
            var lstEPI = db.TEPI_Forms.ToList();
            var lstAllWF = db.TEPI_Workflow.ToList();
            var lstStatus = db.TStatus_Workflow.ToList();
            foreach (var i in lstData)
            {
                var nFromID = 0;

                var q = lstEPI.FirstOrDefault(w => w.OperationTypeID == i.OperationTypeID && w.FacilityID == i.FacilityID && w.IDIndicator == i.IDIndicator && (isYear ? w.sYear == itemSearch.sYear : true));
                if (q != null)
                {
                    nFromID = q.FormID;
                }
                var lstWF = lstAllWF.Where(w => w.FormID == nFromID).ToList();
                if (nFromID != 0)
                {
                    i.nM1 = CheckValMonth(lstWF, 1); i.nM2 = CheckValMonth(lstWF, 2); i.nM3 = CheckValMonth(lstWF, 3); i.nM4 = CheckValMonth(lstWF, 4);
                    i.nM5 = CheckValMonth(lstWF, 5); i.nM6 = CheckValMonth(lstWF, 6);
                    i.nM7 = CheckValMonth(lstWF, 7); i.nM8 = CheckValMonth(lstWF, 8); i.nM9 = CheckValMonth(lstWF, 9); i.nM10 = CheckValMonth(lstWF, 10);
                    i.nM11 = CheckValMonth(lstWF, 11); i.nM12 = CheckValMonth(lstWF, 12);

                    i.nReportID1 = CheckValReportID(lstWF, 1);
                    i.nReportID2 = CheckValReportID(lstWF, 2); i.nReportID3 = CheckValReportID(lstWF, 3); i.nReportID4 = CheckValReportID(lstWF, 4);
                    i.nReportID5 = CheckValReportID(lstWF, 5); i.nReportID6 = CheckValReportID(lstWF, 6);
                    i.nReportID7 = CheckValReportID(lstWF, 7); i.nReportID8 = CheckValReportID(lstWF, 8); i.nReportID9 = CheckValReportID(lstWF, 9); i.nReportID10 = CheckValReportID(lstWF, 10);
                    i.nReportID11 = CheckValReportID(lstWF, 11); i.nReportID12 = CheckValReportID(lstWF, 12);
                }
                else
                {
                    i.nM1 = -1; i.nM2 = -1; i.nM3 = -1; i.nM4 = -1; i.nM5 = -1; i.nM6 = -1;
                    i.nM7 = -1; i.nM8 = -1; i.nM9 = -1; i.nM10 = -1; i.nM11 = -1; i.nM12 = -1;
                }
            }


            #region STATUS
            if (sStatus == "S")
            {
                foreach (var item in lstData)
                {
                    item.sM1 = CheckIcon(item.nM1, item.FormID, item.nReportID1, 1, item.FacilityID, item.IDIndicator, lstStatus);
                    item.sM2 = CheckIcon(item.nM2, item.FormID, item.nReportID2, 2, item.FacilityID, item.IDIndicator, lstStatus);
                    item.sM3 = CheckIcon(item.nM3, item.FormID, item.nReportID3, 3, item.FacilityID, item.IDIndicator, lstStatus);
                    item.sM4 = CheckIcon(item.nM4, item.FormID, item.nReportID4, 4, item.FacilityID, item.IDIndicator, lstStatus);
                    item.sM5 = CheckIcon(item.nM5, item.FormID, item.nReportID5, 5, item.FacilityID, item.IDIndicator, lstStatus);
                    item.sM6 = CheckIcon(item.nM6, item.FormID, item.nReportID6, 6, item.FacilityID, item.IDIndicator, lstStatus);
                    item.sM7 = CheckIcon(item.nM7, item.FormID, item.nReportID7, 7, item.FacilityID, item.IDIndicator, lstStatus);
                    item.sM8 = CheckIcon(item.nM8, item.FormID, item.nReportID8, 8, item.FacilityID, item.IDIndicator, lstStatus);
                    item.sM9 = CheckIcon(item.nM9, item.FormID, item.nReportID9, 9, item.FacilityID, item.IDIndicator, lstStatus);
                    item.sM10 = CheckIcon(item.nM10, item.FormID, item.nReportID10, 10, item.FacilityID, item.IDIndicator, lstStatus);
                    item.sM11 = CheckIcon(item.nM11, item.FormID, item.nReportID11, 11, item.FacilityID, item.IDIndicator, lstStatus);
                    item.sM12 = CheckIcon(item.nM12, item.FormID, item.nReportID12, 12, item.FacilityID, item.IDIndicator, lstStatus);
                }
            }
            else if (sStatus == "E")
            {
                foreach (var item in lstData)
                {
                    item.sM1 = CheckStatusName(item.nM1, lstStatus);
                    item.sM2 = CheckStatusName(item.nM2, lstStatus);
                    item.sM3 = CheckStatusName(item.nM3, lstStatus);
                    item.sM4 = CheckStatusName(item.nM4, lstStatus);
                    item.sM5 = CheckStatusName(item.nM5, lstStatus);
                    item.sM6 = CheckStatusName(item.nM6, lstStatus);
                    item.sM7 = CheckStatusName(item.nM7, lstStatus);
                    item.sM8 = CheckStatusName(item.nM8, lstStatus);
                    item.sM9 = CheckStatusName(item.nM9, lstStatus);
                    item.sM10 = CheckStatusName(item.nM10, lstStatus);
                    item.sM11 = CheckStatusName(item.nM11, lstStatus);
                    item.sM12 = CheckStatusName(item.nM12, lstStatus);
                }
            }
            #endregion

            #endregion

            result.lstDataAll = lstData;

            #region//SORT
            int? nSortCol = SystemFunction.GetIntNull(itemSearch.sIndexCol);
            switch ((itemSearch.sOrderBy + "").ToLower())
            {
                case SystemFunction.ASC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderBy(o => o.sOperationTypeName).ToList(); break;
                            case 2: lstData = lstData.OrderBy(o => o.sFacilityName).ToList(); break;
                            case 3: lstData = lstData.OrderBy(o => o.sIndicator).ToList(); break;
                        }
                    }
                    break;
                case SystemFunction.DESC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderByDescending(o => o.sOperationTypeName).ToList(); break;
                            case 2: lstData = lstData.OrderByDescending(o => o.sFacilityName).ToList(); break;
                            case 3: lstData = lstData.OrderByDescending(o => o.sIndicator).ToList(); break;
                        }
                    }
                    break;
            }
            #endregion

            #region//Final Action >> Skip Take Data For Javasacript
            sysGlobalClass.Pagination dataPage = new sysGlobalClass.Pagination();
            dataPage = SystemFunction.GetPaginationSmall(SystemFunction.GetIntNullToZero(itemSearch.sPageSize), SystemFunction.GetIntNullToZero(itemSearch.sPageIndex), lstData.Count);
            lstData = lstData.Skip(dataPage.nSkipData).Take(dataPage.nTakeData).ToList();

            //foreach (var item in lstData)
            //{
            //    item.sUpdate = item.dUpdate.DateString();
            //    item.sLink = "<a class='btn btn-warning' href='admin_user_info_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nID.ToString())) + "'><i class='fa fa-edit'></i>&nbsp;Edit</a>";
            //}

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
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }
    public static string CheckIcon(int nVal, int nFormID, int nReportID, int nMonth, int nFac, int nIDt, List<TStatus_Workflow> lstStatus)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        string sIcon = "";
        string sName = "";
        var q = lstStatus.Where(w => w.nStatustID == nVal).FirstOrDefault();
        if (q != null)
        {
            sName = q.sStatusName;
        }

        switch (nVal)
        {
            case -1:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");'"
                       + "class='btn-circle cbtnDefalt cPointer' title='No Action'><i class='fas '></i></a>"; // fa-circle
                break;
            case 0:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");' "
                    + "class='btn-circle btn-info cPointer'  title='" + (sName == "" ? "Save Draft" : sName) + "'><i class='fas'></i></a>"; //  fa-circle
                break;
            case 1:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");'"
                    + " class='btn-circle btn-info cPointer'  title='" + (sName == "" ? "Submitted by Operational User" : sName) + "'><i class='fas fa-hourglass'></i></a>";
                break;
            case 2:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");'"
                    + " class='btn-circle btn-warning cPointer'  title='" + (sName == "" ? "Edit Requested by Operational User" : sName) + "'><i class='fas fa-retweet'></i></a>";
                break;
            case 4:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");'"
                    + " class='btn-circle btn-success cPointer'  title='" + (sName == "" ? "Approved by Manager" : sName) + "'><i class='fas fa-user-check'></i></a>";
                break;
            case 5:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");'"
                    + " class='btn-circle btn-success cPointer'  title='" + (sName == "" ? "Approved by ENVI Corporate" : sName) + "'><i class='fas fa-check'></i></a>";
                break;
            case 8:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");'"
                    + " class='btn-circle btn-danger cPointer'  title='" + (sName == "" ? "Rejected by Manager" : sName) + "'><i class='fas fa-reply'></i></a>";
                break;
            case 9:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");'"
                    + " class='btn-circle btn-danger cPointer'  title='" + (sName == "" ? "Rejected by ENVI Corporate" : sName) + "'><i class='fas fa-reply-all'></i></a>";
                break;
            case 17:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");' "
                    + " class='btn-circle btn-success cPointer'  title='" + (sName == "" ? "Accept Edit Request by Manager" : sName) + "'><i class='fas fa-check-circle'></i></a>";
                break;
            case 18:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");'"
                    + " class='btn-circle btn-success cPointer'  title='" + (sName == "" ? "Accept Edit Request by ENVI Corporate" : sName) + "'><i class='fas fa-check-double'></i></a>";
                break;
            case 21:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");'"
                    + " class='btn-circle btn-info cPointer'  title='" + (sName == "" ? "Recalculated by QSHE Operation / QSHE Company" : sName) + "'><i class='fas fa-sync'></i></a>";
                break;
            case 24:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");'"
                    + " class='btn-circle btn-info cPointer'  title='" + (sName == "" ? "Recalled by Operational User" : sName) + "'><i class='fas fa-sync'></i></a>";
                break;
            case 25:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");'"
                    + " class='btn-circle btn-info cPointer'  title='" + (sName == "" ? "Recalculated by Manager" : sName) + "'><i class='fas fa-calculator'></i></a>";
                break;
            case 27:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");'"
                    + " class='btn-circle btn-success cPointer'  title='" + (sName == "" ? "Approve with edit content" : sName) + "'><i class='fas'></i></a>"; // user-check
                break;
            case 26:
                sIcon = "<a onclick='DetailData(" + nFormID + "," + nMonth + "," + nReportID + "," + nFac + "," + nIDt + ");'"
                    + " class='btn-circle btn-success cPointer'  title='" + (sName == "" ? "Complete" : sName) + "'><i class='fas'></i></a>"; // user-check
                break;
        }

        return sIcon;
    }
    public static string CheckStatusName(int nVal, List<TStatus_Workflow> lstWF)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        string sStatusName = "";
        var q = lstWF.Where(w => w.nStatustID == nVal).FirstOrDefault();
        if (q != null)
        {
            sStatusName = q.sStatusName;
        }
        else
        {
            switch (nVal)
            {
                case -1:
                    sStatusName = "No Action";
                    break;
                case 0:
                    sStatusName = "Save Draft"; //  fa-circle
                    break;
                case 1:
                    sStatusName = "Submit by Operational User";
                    break;
                case 2:
                    sStatusName = "Edit Requested by Operational User";
                    break;
                case 4:
                    sStatusName = "Approved by Manager";
                    break;
                case 5:
                    sStatusName = "Approved by ENVI Corporate";
                    break;
                case 8:
                    sStatusName = "Rejected by Manager";
                    break;
                case 9:
                    sStatusName = "Rejected by ENVI Corporate";
                    break;
                case 17:
                    sStatusName = "Accept Edit Request by Manager";
                    break;
                case 18:
                    sStatusName = "Accept Edit Request by ENVI Corporate";
                    break;
                case 21:
                    sStatusName = "Recalculated by QSHE ENVI Corporate";
                    break;
                case 24:
                    sStatusName = "Recalled by Operational User";
                    break;
                case 25:
                    sStatusName = "Recalculated by Manager";
                    break;
                case 27:
                    sStatusName = "Approve with edit content";
                    break;
                case 26:
                    sStatusName = "Completed";
                    break;
            }
        }

        return sStatusName;
    }
    public static int CheckValMonth(List<TEPI_Workflow> lstWF, int nMonth)
    {
        int nVal = 0;
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var q = lstWF.FirstOrDefault(w => w.nMonth == nMonth);
        if (q != null)
        {
            // nVal = q.nStatusID ?? 0;
            nVal = q.nHistoryStatusID ?? 0;
        }

        return nVal;
    }
    public static int CheckValReportID(List<TEPI_Workflow> lstWF, int nMonth)
    {
        int nVal = 0;
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var q = lstWF.FirstOrDefault(w => w.nMonth == nMonth);
        if (q != null)
        {
            nVal = q.nReportID;
        }

        return nVal;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadDetail ListDetail(ItemSearchDetail itemSearch)
    {
        TRetunrLoadDetail r = new TRetunrLoadDetail();
        List<TDataDetail> lstData = new List<TDataDetail>();
        if (UserAcc.UserExpired())
        {
            r.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            string sql = @"select h.nReportID
                              ,h.nToStatusID
	                          ,h.sRemark
	                          ,h.nActionBy
	                          ,h.dUpdate
                              ,CONVERT(varchar,ISNULL(h.dUpdate,0),103) +' '+CONVERT(varchar,ISNULL(h.dUpdate,0),108) as sDate	   
	                          ,s.sStatusName
	                          ,s.nLevelUse
	                          ,s.cType
	                          ,ISNULL(u.Firstname,'') + ' ' + ISNULL(u.Lastname,'') as sActionBy
                        from TEPI_Workflow_History h
                        left join TStatus_Workflow s on s.nStatustID = h.nToStatusID
                        left join mTUser u on u.ID = h.nActionBy
                        where h.nReportID = " + itemSearch.nReportID + @"
                        order by h.dUpdate DESC";
            DataTable dt = CommonFunction.Get_Data(SystemFunction.strConnect, sql);
            lstData = CommonFunction.ConvertDatableToList<TDataDetail>(dt).ToList();

            foreach (var item in lstData)
            {
                //item.sDate = item.dUpdate.ToShortDateString() == "1/1/0001" ? "-" : item.dUpdate.ToShortDateString();
                item.sNextTo = CheckStatus(item.nToStatusID, itemSearch.nFac, itemSearch.nIDt);
            }

            r.Status = SystemFunction.process_Success;
        }

        r.lstData = lstData;
        return r;
    }

    public static string CheckStatus(int nStatus, int nFac, int nIDt)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        string sNextTo = ""; string sFName = ""; string sLName = "";
        var qWF = db.mTWorkFlow.Where(w => w.IDFac == nFac && w.IDIndicator == nIDt && w.cDel == "N").FirstOrDefault();
        switch (nStatus)
        {
            case 0:
                sNextTo = "Operational User (L0)";
                break;
            case 1: // L1
                if (qWF != null)
                {
                    int nUser = qWF.L1 ?? 0;
                    var qUser = db.mTUser.FirstOrDefault(w => w.ID == nUser && w.cDel == "N");
                    if (qUser != null)
                    {
                        if (!string.IsNullOrEmpty(qUser.Firstname)) sFName = qUser.Firstname;
                        if (!string.IsNullOrEmpty(qUser.Lastname)) sLName = qUser.Lastname;
                        sNextTo = sFName + " " + sLName + " (L1)";
                    }
                }
                break;
            case 2: // L 2
                if (qWF != null)
                {
                    int nUser = qWF.L2 ?? 0;
                    var qUser = db.mTUser.FirstOrDefault(w => w.ID == nUser && w.cDel == "N");
                    if (qUser != null)
                    {
                        if (!string.IsNullOrEmpty(qUser.Firstname)) sFName = qUser.Firstname;
                        if (!string.IsNullOrEmpty(qUser.Lastname)) sLName = qUser.Lastname;
                        sNextTo = sFName + " " + sLName + " (L2)";
                    }
                }
                break;
            case 4: // L2
                if (qWF != null)
                {
                    int nUser = qWF.L2 ?? 0;
                    var qUser = db.mTUser.FirstOrDefault(w => w.ID == nUser && w.cDel == "N");
                    if (qUser != null)
                    {
                        if (!string.IsNullOrEmpty(qUser.Firstname)) sFName = qUser.Firstname;
                        if (!string.IsNullOrEmpty(qUser.Lastname)) sLName = qUser.Lastname;
                        sNextTo = sFName + " " + sLName + " (L2)";
                    }
                }
                break;
            case 5:
                sNextTo = "-";
                break;
            case 8:
                sNextTo = "Operational User (L0)";
                break;
            case 9:
                sNextTo = "Operational User (L0)";
                break;
            case 12:
                sNextTo = "-";
                break;
            case 13:
                sNextTo = "-";
                break;
            case 17:
                sNextTo = "Operational User (L0)";
                break;
            case 18:
                sNextTo = "Operational User (L0)";
                break;
            case 21:
                sNextTo = "-";
                break;
            case 24:
                sNextTo = "Operational User (L0)";
                break;
            case 25:
                sNextTo = "-";
                break;
        }

        return sNextTo;
    }


    #region EXPORT
    protected void btnEx_Click(object sender, EventArgs e)
    {
        ExportData_new();
    }
    public void ExportData_new()
    {

        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        if (!UserAcc.UserExpired())  //Check Session
        {
            int nOperationType = !string.IsNullOrEmpty(hidOperationType.Value) ? int.Parse(hidOperationType.Value) : 0;
            int nIndicator = !string.IsNullOrEmpty(hidIndicator.Value) ? int.Parse(hidIndicator.Value) : 0;
            int nFacility = !string.IsNullOrEmpty(hidFacility.Value) ? int.Parse(hidFacility.Value) : 0;

            CSearch item = new CSearch();
            item.sYear = hidYear.Value;
            item.sOperationTypeID = hidOperationType.Value;
            item.sIDIndicator = hidIndicator.Value;
            item.sFacilityID = hidFacility.Value;

            TRetunrLoadData dd = LoadData(item, "E");

            string sIndicatorName = nIndicator == 0 ? "- Select Indicator -" : db.mTIndicator.FirstOrDefault(w => w.ID == nIndicator).Indicator; //ddlIndicator.SelectedItem.Text;
            string sOName = nOperationType == 0 ? "- Select OperationType -" : db.mOperationType.FirstOrDefault(w => w.ID == nOperationType).Name; // ddlOperationType.SelectedItem.Text;
            string sFacility = nFacility == 0 ? "- Select Sub-facility -" : db.mTFacility.FirstOrDefault(w => w.ID == nFacility).Name; //ddlFacility.SelectedItem.Text;
            string sYear = hidYear.Value == "0" ? "- select Year -" : hidYear.Value; //ddlYear.SelectedItem.Text;

            string saveAsFileName = string.Format("EPIFROM_Monitoring" + "_" + "{0:ddMMyyyyHHmmss}.xlsx", DateTime.Now).Replace("/", "_");

            XLWorkbook workbook = GetWorkbookExcel(dd.lstDataAll, saveAsFileName
                , sIndicatorName, sOName, sFacility, sYear);

            #region Save the Excel spreadsheet to a MemoryStream and return it to the client
            using (MemoryStream exportData = new MemoryStream())
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("Downloaded", "True"));
                workbook.SaveAs(exportData);
                exportData.Position = 0;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.BinaryWrite(exportData.ToArray());
                HttpContext.Current.Response.End();
            }
            #endregion
        }
    }
    public static XLWorkbook GetWorkbookExcel(List<TDataTable> lstData, string sFileName, string sIndicatorName, string sOName, string sFacility, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        // Creating a new workbook
        XLWorkbook workbook = new XLWorkbook();
        workbook.Style.Font.FontName = "Cordia New";

        //Adding a worksheet
        string saveAsFileName = string.Format("Report_{0:ddMMyyyyHHmmss}", DateTime.Now).Replace("/", "_");

        //row number must be between 1 and 1048576
        int nRownumber = 1;
        //Column number must be between 1 and 16384
        int nColnumber = 1;
        IXLWorksheet worksheet = workbook.Worksheets.Add(sIndicatorName);
        worksheet.ShowGridLines = false;
        IXLCell icell;
        IXLRow irow;

        #region Set Column Width
        worksheet.Column(1).Width = 40;
        worksheet.Column(2).Width = 20;
        worksheet.Column(3).Width = 30;
        worksheet.Column(4).Width = 30;
        worksheet.Column(5).Width = 30;
        worksheet.Column(6).Width = 30;
        worksheet.Column(7).Width = 30;
        worksheet.Column(8).Width = 30;
        worksheet.Column(9).Width = 30;
        worksheet.Column(10).Width = 30;
        worksheet.Column(11).Width = 30;
        worksheet.Column(12).Width = 30;
        worksheet.Column(13).Width = 30;
        worksheet.Column(14).Width = 30;
        worksheet.Column(15).Width = 30;

        #endregion

        #region Report Header
        IXLStyle ReportStyle = (IXLStyle)workbook.Style;
        ReportStyle.Font.Bold = true;
        ReportStyle.Font.FontSize = 14;
        ReportStyle.Font.FontColor = XLColor.Black;
        ReportStyle.Fill.BackgroundColor = XLColor.FromHtml("#FFFFFF");
        ReportStyle.Fill.PatternType = XLFillPatternValues.Solid;
        ReportStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        ReportStyle.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        #region function
        Action<IXLRow, int, XLCellValues, XLAlignmentHorizontalValues, bool, object, string> SetBodyStyleHeader = (row, nColumn, CType, iHAlign, isBold, ObjValue, sFormat) =>
        {
            IXLCell cell = row.Cell(nColumn);
            cell.DataType = CType;
            cell.Value = ObjValue;
            //IXLStyle TBodyStyle = (IXLStyle)workbook.Style;
            cell.Style.Font.Bold = isBold;
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.Black;
            cell.Style.Border.TopBorderColor = XLColor.FromHtml("#FFFFFF");
            cell.Style.Border.LeftBorderColor = XLColor.FromHtml("#FFFFFF");
            cell.Style.Border.RightBorderColor = XLColor.FromHtml("#FFFFFF");
            cell.Style.Border.BottomBorderColor = XLColor.FromHtml("#FFFFFF");
            cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#ffffff");
            cell.Style.Fill.PatternType = XLFillPatternValues.Solid;
            cell.Style.Alignment.Horizontal = iHAlign;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.DateFormat.Format = "";
            cell.Style.NumberFormat.Format = "";

            if (CType == XLCellValues.DateTime) cell.Style.DateFormat.Format = sFormat;
            else if (CType == XLCellValues.Number) cell.Style.NumberFormat.Format = sFormat;
            //cell.Style = TBodyStyle;
        };
        #endregion

        worksheet.Row(nRownumber).Height = 30;
        irow = worksheet.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Indicator : " + sIndicatorName, "0");
        nRownumber++;

        worksheet.Row(nRownumber).Height = 30;
        irow = worksheet.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Operation Type : " + sOName, "0");
        nRownumber++;

        worksheet.Row(nRownumber).Height = 30;
        irow = worksheet.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Facility : " + sFacility, "0");
        nRownumber++;

        worksheet.Row(nRownumber).Height = 30;
        irow = worksheet.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Year : " + sYear, "0");
        nRownumber++;
        #endregion

        #region Merge Cell
        Action<string> SetMerge = (sRange) =>
        {
            IXLRange rangeMerge = worksheet.Range(sRange);
            rangeMerge.Merge();
            rangeMerge.Style.Border.TopBorderColor = XLColor.FromHtml("#FFFFFF");
            rangeMerge.Style.Border.LeftBorderColor = XLColor.FromHtml("#FFFFFF");
            rangeMerge.Style.Border.RightBorderColor = XLColor.FromHtml("#FFFFFF");
            rangeMerge.Style.Border.BottomBorderColor = XLColor.FromHtml("#FFFFFF");
            rangeMerge.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            rangeMerge.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            rangeMerge.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            rangeMerge.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        };
        //SetMerge("A1:P1");
        //SetMerge("A1:E1");
        //SetMerge("A2:E2");
        //SetMerge("A3:E3");
        #endregion

        #region Add Table Header

        Action<IXLRow, int, string> SetHeadStyle = (row, nColumn, sHeadText) =>
        {
            IXLCell cell = row.Cell(nColumn);
            cell.Value = sHeadText;
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.FromHtml("#31708F");
            cell.Style.Border.TopBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.LeftBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.RightBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.BottomBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EDF7");
            cell.Style.Fill.PatternType = XLFillPatternValues.Solid;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            //cell.Style = THeadStyle;
        };

        #endregion

        #region function BODY
        Action<IXLRow, int, XLCellValues, XLAlignmentHorizontalValues, bool, object, string> SetBodyStyle = (row, nColumn, CType, iHAlign, isBold, ObjValue, sFormat) =>
        {
            IXLCell cell = row.Cell(nColumn);
            cell.DataType = CType;
            cell.Value = ObjValue;
            //IXLStyle TBodyStyle = (IXLStyle)workbook.Style;
            cell.Style.Font.Bold = isBold;
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.Black;
            cell.Style.Border.TopBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.LeftBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.RightBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.BottomBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFFFFF");
            cell.Style.Fill.PatternType = XLFillPatternValues.Solid;
            cell.Style.Alignment.Horizontal = iHAlign;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.DateFormat.Format = "";
            cell.Style.NumberFormat.Format = "";

            if (CType == XLCellValues.DateTime) cell.Style.DateFormat.Format = sFormat;
            else if (CType == XLCellValues.Number) cell.Style.NumberFormat.Format = sFormat;
            //cell.Style = TBodyStyle;
        };
        Action<IXLRow, int, XLCellValues, XLAlignmentHorizontalValues, bool, object, string, string> SetBodyStyle2 = (row, nColumn, CType, iHAlign, isBold, ObjValue, sFormat, sBackgroundColor) =>
        {
            IXLCell cell = row.Cell(nColumn);
            cell.DataType = CType;
            cell.Value = ObjValue;
            //IXLStyle TBodyStyle = (IXLStyle)workbook.Style;
            cell.Style.Font.Bold = isBold;
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Border.TopBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.LeftBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.RightBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.BottomBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            if (sBackgroundColor == "#fabd4f") cell.Style.Font.FontColor = XLColor.Black;
            else if (sBackgroundColor == "#ffffff") cell.Style.Font.FontColor = XLColor.Black;
            else cell.Style.Font.FontColor = XLColor.Black;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml(sBackgroundColor);
            cell.Style.Fill.PatternType = XLFillPatternValues.Solid;
            cell.Style.Alignment.Horizontal = iHAlign;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.DateFormat.Format = "";
            cell.Style.NumberFormat.Format = "";

            if (CType == XLCellValues.DateTime) cell.Style.DateFormat.Format = sFormat;
            else if (CType == XLCellValues.Number) cell.Style.NumberFormat.Format = sFormat;
            //cell.Style = TBodyStyle;
        };
        #endregion

        #region BIND DATA

        #region HEAD
        nRownumber++;
        irow = worksheet.Row(nRownumber);
        irow.Height = 22.20;

        nColnumber = 1;

        SetHeadStyle(irow, nColnumber++, "Operation Type");
        SetHeadStyle(irow, nColnumber++, "Facility");
        SetHeadStyle(irow, nColnumber++, "Indicator");
        SetHeadStyle(irow, nColnumber++, "Jan");
        SetHeadStyle(irow, nColnumber++, "Feb");
        SetHeadStyle(irow, nColnumber++, "Mar");
        SetHeadStyle(irow, nColnumber++, "Apr");
        SetHeadStyle(irow, nColnumber++, "May");
        SetHeadStyle(irow, nColnumber++, "Jun");
        SetHeadStyle(irow, nColnumber++, "Jul");
        SetHeadStyle(irow, nColnumber++, "Aug");
        SetHeadStyle(irow, nColnumber++, "Sep");
        SetHeadStyle(irow, nColnumber++, "Oct");
        SetHeadStyle(irow, nColnumber++, "Nov");
        SetHeadStyle(irow, nColnumber++, "Dec");
        #endregion

        int index = 1;
        foreach (var item in lstData)
        {
            nColnumber = 1;
            nRownumber++;
            irow = worksheet.Row(nRownumber);
            irow.Height = 17.40;

            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sOperationTypeName, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sFacilityName, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sIndicator, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sM1, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sM2, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sM3, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sM4, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sM5, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sM6, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sM7, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sM8, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sM9, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sM10, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sM11, "");
            SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sM12, "");

            index++;
        }
        #endregion

        nRownumber++;

        return workbook;
    }

    #endregion

    #region CLASS

    [Serializable]
    public class CSearch : sysGlobalClass.CommonLoadData
    {
        public string sOperationTypeID { get; set; }
        public string sIDIndicator { get; set; }
        public string sFacilityID { get; set; }
        public string sYear { get; set; }
        public string sPms { get; set; }

        ///Detail
        public int nFormID { get; set; }
        public int nReportID { get; set; }

    }

    [Serializable]
    public class TRetunrLoadData : sysGlobalClass.Pagination
    {
        public List<TDataTable> lstData { get; set; }
        public List<TDataTable> lstDataAll { get; set; }
    }

    [Serializable]
    public class TDataTable
    {
        public int FormID { get; set; }
        public string sYear { get; set; }

        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public int FacilityID { get; set; }

        public string sIndicator { get; set; }
        public string sFacilityName { get; set; }
        public string sOperationTypeName { get; set; }

        public int nUserID { get; set; }
        public int nRoleID { get; set; }
        public int nOrder { get; set; }

        public int nM1 { get; set; }
        public int nM2 { get; set; }
        public int nM3 { get; set; }
        public int nM4 { get; set; }
        public int nM5 { get; set; }
        public int nM6 { get; set; }
        public int nM7 { get; set; }
        public int nM8 { get; set; }
        public int nM9 { get; set; }
        public int nM10 { get; set; }
        public int nM11 { get; set; }
        public int nM12 { get; set; }
        /// <summary>
        /// สัญลักษณ์ / btn
        /// </summary>
        public string sM1 { get; set; }
        public string sM2 { get; set; }
        public string sM3 { get; set; }
        public string sM4 { get; set; }
        public string sM5 { get; set; }
        public string sM6 { get; set; }
        public string sM7 { get; set; }
        public string sM8 { get; set; }
        public string sM9 { get; set; }
        public string sM10 { get; set; }
        public string sM11 { get; set; }
        public string sM12 { get; set; }

        public int nReportID1 { get; set; }
        public int nReportID2 { get; set; }
        public int nReportID3 { get; set; }
        public int nReportID4 { get; set; }
        public int nReportID5 { get; set; }
        public int nReportID6 { get; set; }
        public int nReportID7 { get; set; }
        public int nReportID8 { get; set; }
        public int nReportID9 { get; set; }
        public int nReportID10 { get; set; }
        public int nReportID11 { get; set; }
        public int nReportID12 { get; set; }
    }

    [Serializable]
    public class TRetunrLoadDetail : sysGlobalClass.Pagination
    {
        public List<TDataDetail> lstData { get; set; }
    }

    [Serializable]
    public class ItemSearchDetail
    {
        public int nFormID { get; set; }
        public int nReportID { get; set; }
        public int nMonth { get; set; }
        public int nFac { get; set; }
        public int nIDt { get; set; }
    }

    public class TDataDetail
    {
        public int nReportID { get; set; }
        public int nToStatusID { get; set; }
        public string sRemark { get; set; }
        public int nActionBy { get; set; }
        public string sDate { get; set; }
        public string sStatusName { get; set; }
        public int nLevelUse { get; set; }
        public string cType { get; set; }
        public string sActionBy { get; set; }
        public string sNextTo { get; set; }
        public string sColor { get; set; }
        public DateTime dUpdate { get; set; }
    }

    #endregion

    #region CLASS JAME
    public class SetPageLoadMaster
    {
        public List<T_mTIndicator> lstIn { get; set; }
        public string sMsg { get; set; }
        public string sStatus { get; set; }
    }
    public class T_mTIndicator
    {
        public int ID { get; set; }
        public int ID_OperationType { get; set; }
        public int ID_Facilitye { get; set; }
        public string Indicator { get; set; }
        public List<T_mTProductIndicator> lstProIn { get; set; }
        public List<T_mOperationType> lstOperationType { get; set; }
        public int? nOrder { get; set; }
        public string sPath { get; set; }
    }
    public class T_mTProductIndicator
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int? nOrder { get; set; }
        public int? IDIndicator { get; set; }
    }
    public class T_mOperationType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string sCode { get; set; }
        public string sPath { get; set; }
        public List<T_mTFacility> lstFacility { get; set; }
        public int nIndicator { get; set; }
    }
    public class T_mTFacility
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? OperationTypeID { get; set; }
        public bool IsActive { get; set; }
        public int nGroupIndicatorID { get; set; }

    }
    #endregion
}