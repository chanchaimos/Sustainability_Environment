using sysExtension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_user_info_lst : System.Web.UI.Page
{
    public static bool IsView = false;
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnLoad(myFunc);
    }
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
                int Prms = SystemFunction.GetPermissionMenu(34);
                hdfPrmsMenu.Value = Prms + "";
                IsView = Prms == 1;
                if (IsView)
                {
                    ckbAll.Visible = false;
                    btnDel.Visible = false;
                    btnCreate.Visible = false;
                }

                SystemFunction.BindDropdownPageSize(ddlPageSize, null);
                BlindDDL();
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData LoadData(CSearch itemSearch)
    {
        TRetunrLoadData result = new TRetunrLoadData();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            List<TDataTable> lstData = new List<TDataTable>();
            int nRoleID = UserAcc.GetObjUser().nRoleID;
            int nUserID = UserAcc.GetObjUser().nUserID;

            string sCon = "";
            string sSearch = itemSearch.sSearch.Trims().ToLower();
            if (!string.IsNullOrEmpty(sSearch))
            {
                sCon += @" AND (tu.Firstname LIKE '%" + sSearch + "%' OR tu.Lastname LIKE '%" + sSearch + "%' OR tu.Username LIKE '%" + sSearch + "%') ";
            }
            if (!string.IsNullOrEmpty(itemSearch.sUserRole))
            {
                sCon += @" AND (tr.nRoleID = " + itemSearch.sUserRole + @") ";
            }
            if (!string.IsNullOrEmpty(itemSearch.sOperationSearch))
            {
                sCon += @" AND tf.OperationTypeID  = " + itemSearch.sOperationSearch + @" ";
            }
            if (!string.IsNullOrEmpty(itemSearch.sFacilitySearch))
            {
                sCon += @" AND tmf.nFacilityID  = " + itemSearch.sFacilitySearch + @" ";
            }
            if (!string.IsNullOrEmpty(itemSearch.sStatus))
            {
                sCon += @" AND tu.cActive = '" + itemSearch.sStatus + @"'  ";
            }

            if (nRoleID == 6)
            {
                var Get_Fac = db.mTUser_FacilityPermission.Where(w => w.nUserID == nUserID && w.nRoleID == nRoleID).Select(s => s.nFacilityID).Distinct().ToList(); // User And Role Get Facility
                if (Get_Fac.Any())
                {
                    string Format = "";
                    Get_Fac.ForEach(f =>
                    {
                        int nFac = f;
                        Format += "," + nFac + "";
                    });
                    Format = Format.Remove(0, 1);
                    sCon += @" AND tmf.nFacilityID IN (" + Format + ") ";
                }
                //var Get_Operation = db.mTFacility.Where(w => Get_Fac.Contains(w.ID) && itemSearch.sOperationSearch != "" ? w.ID == int.Parse(itemSearch.sOperationSearch) : true).Select(s => s.OperationTypeID).Distinct().ToList();
                var lst_User = db.mTUser_FacilityPermission.Where(w => Get_Fac.Contains(w.nFacilityID)).Select(s => s.nUserID).Distinct().ToList(); // User IN Facility Company_Role
                if (lst_User.Any())
                {
                    string Format = "";
                    lst_User.ForEach(f =>
                    {
                        int nUserId = f;
                        Format += "," + nUserId + "";
                    });
                    Format = Format.Remove(0, 1);
                    sCon += @" AND tu.ID IN (" + Format + ") ";
                }
            }

            string _SQL = @" SELECT tu.ID as nID
            ,ISNULL(tu.Username,'') as Username
            ,ISNULL(tu.Firstname,'') as Firstname
            ,ISNULL(tu.Lastname,'') as Lastname
            , ISNULL(tu.Company,'') as Company
            , ISNULL(tu.Email,'') as Email
            ,CASE WHEN  tu.cActive = 'Y' THEN 'Active' WHEN tu.cActive = 'N' THEN 'Inactive' ELSE '' END as sStatus
            ,tu.dUpdate as dUpdate
            ,ISNULL(tu.sUserCode,'') as sUserCode
            ,(SELECT COUNT(tr.nUID) FROM mTUserInRole tr WHERE tr.nUID = tu.ID) as nCountRole
            FROM mTUser tu
            LEFT JOIN mTUser_FacilityPermission tmf ON tmf.nUserID = tu.ID 
            LEFT JOIN TMenu_Permission tmu ON tmu.nUserID = tu.ID 
            LEFT JOIN mTFacility tf ON tf.ID = tmf.nFacilityID
            LEFT JOIN mTUserInRole tr ON tr.nUID = tu.ID
            WHERE tu.cDel = 'N' and tu.ID != 1  " + sCon + @"
            GROUP BY tu.ID,tu.Username,tu.Firstname,tu.Lastname,tu.Company,tu.Email,tu.cActive,tu.dUpdate,tu.sUserCode 
            ORDER BY tu.dUpdate DESC ";
            DataTable dt = CommonFunction.Get_Data(SystemFunction.strConnect, _SQL);
            lstData = CommonFunction.ConvertDatableToList<TDataTable>(dt).ToList();


            //lstData = db.mTUser.Where(w => w.cDel == "N" && (!string.IsNullOrEmpty(sSearch) ? w.Firstname.ToLower().Contains(sSearch) || w.Lastname.ToLower().Contains(sSearch) : true)).Select(s => new TDataTable
            //{
            //    //nCompID = s.ID,
            //    //sCompName = s.Name,
            //    //sStatus = s.cActive == "Y" ? "Active" : "Inactive",
            //    dUpdate = s.dUpdate,
            //    nID = s.ID,
            //    Firstname = (string.IsNullOrEmpty(s.Firstname)) ? "" : s.Firstname,
            //    Lastname = (string.IsNullOrEmpty(s.Lastname)) ? "" : s.Lastname,
            //    Username = (string.IsNullOrEmpty(s.Username)) ? "" : s.Username,
            //    Email = (string.IsNullOrEmpty(s.Email)) ? "" : s.Email,
            //    sUserCode = (string.IsNullOrEmpty(s.sUserCode)) ? "N/A" : s.sUserCode,
            //    Company = s.Company,
            //    sStatus = s.cActive == "Y" ? "Active" : "Inactive",
            //}).OrderByDescending(o => o.dUpdate).ToList();
            foreach (var item in lstData)
            {
                List<TDataRole> lstData1 = new List<TDataRole>();
                item.sUpdate = item.dUpdate.DateString();
                if (IsView)
                {
                    item.sLink = "<a class='btn btn-primary' href='admin_user_info_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nID.ToString())) + "'><i class='fa fa-search'></i>&nbsp;View</a>";
                }
                else
                {
                    item.sLink = "<a class='btn btn-warning' href='admin_user_info_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nID.ToString())) + "'><i class='fa fa-edit'></i>&nbsp;Edit</a>";
                }
            }

            #region//SORT
            int? nSortCol = SystemFunction.GetIntNull(itemSearch.sIndexCol);
            switch ((itemSearch.sOrderBy + "").ToLower())
            {
                case SystemFunction.ASC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderBy(o => o.Username).ToList(); break;
                            case 2: lstData = lstData.OrderBy(o => o.Firstname).ToList(); break;
                            case 3: lstData = lstData.OrderBy(o => o.Company).ToList(); break;
                            case 4: lstData = lstData.OrderBy(o => o.nCountRole).ToList(); break;
                            case 5: lstData = lstData.OrderBy(o => o.Email).ToList(); break;
                            case 6: lstData = lstData.OrderBy(o => o.sStatus).ToList(); break;
                        }
                    }
                    break;
                case SystemFunction.DESC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderByDescending(o => o.Username).ToList(); break;
                            case 2: lstData = lstData.OrderByDescending(o => o.Firstname).ToList(); break;
                            case 3: lstData = lstData.OrderByDescending(o => o.Company).ToList(); break;
                            case 4: lstData = lstData.OrderByDescending(o => o.nCountRole).ToList(); break;
                            case 5: lstData = lstData.OrderByDescending(o => o.Email).ToList(); break;
                            case 6: lstData = lstData.OrderByDescending(o => o.sStatus).ToList(); break;
                        }
                    }
                    break;
            }
            #endregion

            #region//Final Action >> Skip Take Data For Javasacript
            sysGlobalClass.Pagination dataPage = new sysGlobalClass.Pagination();
            dataPage = SystemFunction.GetPaginationSmall(SystemFunction.GetIntNullToZero(itemSearch.sPageSize), SystemFunction.GetIntNullToZero(itemSearch.sPageIndex), lstData.Count);
            lstData = lstData.Skip(dataPage.nSkipData).Take(dataPage.nTakeData).ToList();

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

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod DeleteData(string[] arrValue)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            List<int> lstDelID = new List<int>();
            if (arrValue != null)
                lstDelID = arrValue.Select(s => s.toIntNullToZero()).ToList();
            int nUserID = UserAcc.GetObjUser().nUserID;
            db.mTUser.Where(w => lstDelID.Contains(w.ID)).ToList().ForEach(x =>
             {
                 x.cDel = "Y";
                 x.dUpdate = DateTime.Now;
                 x.nUpdateID = nUserID;
             });
            db.SaveChanges();
            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData GetRole(string sUserID)
    {
        TRetunrLoadData result = new TRetunrLoadData();
        List<TDataRole> lstData = new List<TDataRole>();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        if (UserAcc.UserExpired())
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            int nUserID = 0;
            if (!string.IsNullOrEmpty(sUserID))
            {
                nUserID = int.Parse(sUserID);
            }
            var TBUser_InRow = db.mTUserInRole.Where(w => w.nUID == nUserID).ToList();
            // var lst_RoleAdmin = db.TMenu_Permission.Where(w => w.nUserID == nUserID).ToList();
            var lstRoleAdmin = db.TMenu_Permission.Where(w => w.nUserID == nUserID).Select(s => s.nRoleID).Distinct().ToList();
            var lst_RoleOther = db.mTUser_FacilityPermission.Where(w => w.nUserID == nUserID && w.nRoleID != 1).Select(s => s.nRoleID).Distinct().ToList();
            //var lst_RoleOther = db.mTUser_FacilityPermission.Where(w => w.nUserID == nUserID).ToList();
            var lstDataRole = db.mTUserRole.ToList();
            if (TBUser_InRow.Any())
            {
                TBUser_InRow.ForEach(f =>
                {
                    lstData.Add(new TDataRole
                    {
                        nRoleID = f.nRoleID,
                        sRoleName = lstDataRole.Any(a => a.ID == f.nRoleID) ? lstDataRole.First(a => a.ID == f.nRoleID).Name : "",
                    });
                });
            }
            //if (lstRoleAdmin.Any())
            //{
            //    foreach (var item in lstRoleAdmin)
            //    {
            //        int nRole = int.Parse(item + "");
            //        lstData.Add(new TDataRole
            //        {
            //            nRoleID = nRole,
            //            sRoleName = lstDataRole.Any(a => a.ID == item) ? lstDataRole.First(a => a.ID == item).Name : "",
            //        });
            //    }
            //}

            //if (lst_RoleOther.Any())
            //{
            //    foreach (var item in lst_RoleOther)
            //    {
            //        int nRole = int.Parse(item + "");
            //        lstData.Add(new TDataRole
            //        {
            //            nRoleID = nRole,
            //            sRoleName = lstDataRole.Any(a => a.ID == item) ? lstDataRole.First(a => a.ID == item).Name : "",
            //        });
            //    }
            //}
        }

        //        DataTable dt = new DataTable();
        //dt = SystemFunction
        //dt = SystemFunction2.GetPrmsRole(sUserID);
        //lstData = dt.AsEnumerable().Select(s => new TDataRole
        //    {
        //        nRoleID = s.Field<int>("nGroupID"),
        //        sRoleName = s.Field<string>("sGroupname")
        //    }).ToList();
        //lstDataRole = lstData.Distinct().ToList();
        result.lstDataRole = lstData.Distinct().ToList();
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData Get_Facility(string operationID)
    {
        //if (lst == null) lst = new List<string>();
        TRetunrLoadData result = new TRetunrLoadData();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        if (UserAcc.UserExpired())
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            int nRoleID = UserAcc.GetObjUser().nRoleID;
            int nUserID = UserAcc.GetObjUser().nUserID;

            List<sysGlobalClass.T_Facility> lstFacility = new List<sysGlobalClass.T_Facility>();
            if (!string.IsNullOrEmpty(operationID))
            {
                int nID = int.Parse(operationID);
                lstFacility = SystemFunction.Get_SubFacility(nID, nUserID, nRoleID);
            }
            result.lstData_Facility = lstFacility.Distinct().ToList();
        }
        return result;
    }
    #region BlindDDL
    private void BlindDDL()
    {
        int nRoleID = UserAcc.GetObjUser().nRoleID;
        int nUserID = UserAcc.GetObjUser().nUserID;
        DDL_OperationType(nRoleID, nUserID);
        //DDL_GroupIndicator();
        //DDL_Facility();
        DDL_Role();
    }
    private void DDL_OperationType(int nRoleID, int nUserID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstOperationType_Search = db.mOperationType.Where(w => w.cDel == "N" && w.cActive == "Y" && w.cManage == "N").Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        //if (nRoleID == 6)
        //{
        //    var db1 = (from a in db.mTUser_FacilityPermission select a).ToList();
        //    var db2 = (from a in db.mTFacility select a).ToList();
        //    var db3 = (from a in db.mOperationType select a).ToList();
        //    //var lstOperationType_1 = db.mOperationType.Where(w => w.cDel == "N" && w.cActive == "Y" && w.cManage == "N").Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        //    lstOperationType_Search = (from a in db1
        //                        join b in db2 on a.nFacilityID equals b.ID
        //                        join c in db3 on b.OperationTypeID equals c.ID
        //                        where a.nUserID == nUserID
        //                        select new { Value = c.ID, Text = c.Name }).Distinct().ToList();
        //}
        if (lstOperationType_Search.Count() > 0)
        {
            ddlOperationSearch.DataSource = lstOperationType_Search;
            ddlOperationSearch.DataValueField = "Value";
            ddlOperationSearch.DataTextField = "Text";
            ddlOperationSearch.DataBind();
            ddlOperationSearch.Items.Insert(0, new ListItem("- Operation type -", ""));
        }
    }
    private void DDL_Role()
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstUserRole = db.mTUserRole.Where(w => w.cDel == "N" && w.cActive == "Y").Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        if (lstUserRole.Count() > 0)
        {
            ddlUserRole.DataSource = lstUserRole;
            ddlUserRole.DataValueField = "Value";
            ddlUserRole.DataTextField = "Text";
            ddlUserRole.DataBind();
            ddlUserRole.Items.Insert(0, new ListItem("- User role -", ""));
        }
    }
    private void DDL_Facility()
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstFacility = db.mTFacility.Where(w => w.nLevel == 2 && w.cDel == "N" && w.cActive == "Y").Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        if (lstFacility.Count() > 0)
        {
            ddlFacilitySearch.DataSource = lstFacility;
            ddlFacilitySearch.DataValueField = "Value";
            ddlFacilitySearch.DataTextField = "Text";
            ddlFacilitySearch.DataBind();
            ddlFacilitySearch.Items.Insert(0, new ListItem("- Sub facility -", ""));
        }
    }
    #endregion

    #region class
    [Serializable]
    public class CSearch : sysGlobalClass.CommonLoadData
    {
        public string sSearch { get; set; }
        public string sStatus { get; set; }
        public string sPrms { get; set; }
        public string sUserRole { get; set; }
        public string sOperationSearch { get; set; }
        public string sFacilitySearch { get; set; }
    }

    [Serializable]
    public class TRetunrLoadData : sysGlobalClass.Pagination
    {
        public List<TDataTable> lstData { get; set; }
        public List<TDataRole> lstDataRole { get; set; }
        public List<sysGlobalClass.T_Facility> lstData_Facility { get; set; }
    }

    [Serializable]
    public class TDataTable
    {
        public int nID { get; set; }
        public string Username { get; set; }
        public string sUserCode { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Company { get; set; }
        public string BusinessUnit { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string cActive { get; set; }
        public string cDel { get; set; }
        public string sStatus { get; set; }
        public DateTime? dUpdate { get; set; }
        public string sUpdate { get; set; }
        public string sLink { get; set; }
        public int nCountRole { get; set; }
    }
    [Serializable]
    public class TDataRole
    {
        public int nRoleID { get; set; }
        public string sRoleName { get; set; }
    }
    [Serializable]
    public class T_Facility
    {
        public int nFacilityID { get; set; }
        public string sFacilityName { get; set; }
        public int? nHeaderID { get; set; }
        public int? nLevel { get; set; }
        public string sRelation { get; set; }
        public int nOperationTypeID { get; set; }
        public string sOperationName { get; set; }
    }

    #endregion
}