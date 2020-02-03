using sysExtension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_user_info_update : System.Web.UI.Page
{
    public static bool IsView = false;
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnLoad(myFunc);
    }
    PTTGC_EPIEntities db = new PTTGC_EPIEntities();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (UserAcc.UserExpired())
            {
                SetBodyEventOnLoad(SystemFunction.PopupLogin());
            }
            else
            {
                string strID = Request.QueryString["strid"];
                if (!string.IsNullOrEmpty(strID))
                {
                    hidUserID.Value = STCrypt.Decrypt(strID);
                    SetData(hidUserID.Value.toIntNullToZero());
                    hidEncryptUserID.Value = STCrypt.Encrypt(hidUserID.Value);
                }
                else
                {
                    DivContract.Style.Add("display", "none");
                }

                txtPassword.Attributes.Add("type", "password");
                GETDDL();
                SystemFunction.BindDropdownPageSize(ddlPageSize, null);
                SystemFunction.BindDropdownPageSize(ddlPageSize_RoleAdmin, null);
                SystemFunction.BindDropdownPageSize(ddlPage_Main, null);

                int Prms = SystemFunction.GetPermissionMenu(73);
                hdfPrmsMenu.Value = Prms + "";
                IsView = Prms == 1;
                if (IsView)
                {
                    ckbAll.Visible = false;
                    btnDel.Visible = false;
                    btnResetmail.Visible = false;
                    btnAddOperation.Visible = false; //Add_Mutiselect
                    AddData_Role.Visible = false; // Add_Role
                    btnSave_Main.Visible = false; // Save_Data
                    // btnCreate.Visible = false;
                }
                // ListDBToDropDownList(SystemFunction.strConnect, ddlUserRole, "sele", "", "Value", "");
            }
        }
    }


    #region ByDropdown
    private void GETDDL()
    {
        int nRoleID = UserAcc.GetObjUser().nRoleID;
        int nUserID = UserAcc.GetObjUser().nUserID;
        DDL_Role(nRoleID);
        DDL_OperationType(nRoleID, nUserID);
        DDL_GroupIndicator(nRoleID, nUserID);
    }
    private void DDL_Role(int nRoleID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstUserRole = db.mTUserRole.Where(w => w.cDel == "N" && w.cActive == "Y").Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        if (lstUserRole.Count() > 0)
        {
            if (nRoleID == 6)
            {
                lstUserRole = lstUserRole.Where(w => w.Value == 2 || w.Value == 3 || w.Value == 5).ToList();
            }
            ddlUserRole.DataSource = lstUserRole;
            ddlUserRole.DataValueField = "Value";
            ddlUserRole.DataTextField = "Text";
            ddlUserRole.DataBind();
            ddlUserRole.Items.Insert(0, new ListItem("- User role -", ""));
        }
    }
    private void DDL_OperationType(int nRoleID, int nUserID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstOperationType = db.mOperationType.Where(w => w.cDel == "N" && w.cActive == "Y" && w.cManage == "N").Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        //if (nRoleID == 6)
        //{
        //    var db1 = (from a in db.mTUser_FacilityPermission select a).ToList();
        //    var db2 = (from a in db.mTFacility select a).ToList();
        //    var db3 = (from a in db.mOperationType select a).ToList();
        //    //var lstOperationType_1 = db.mOperationType.Where(w => w.cDel == "N" && w.cActive == "Y" && w.cManage == "N").Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        //    lstOperationType = (from a in db1
        //                        join b in db2 on a.nFacilityID equals b.ID
        //                        join c in db3 on b.OperationTypeID equals c.ID
        //                        where a.nUserID == nUserID
        //                        select new { Value = c.ID, Text = c.Name }).Distinct().ToList();
        //}
        if (lstOperationType.Count() > 0)
        {
            ddlOperationType.DataSource = lstOperationType;
            ddlOperationType.DataValueField = "Value";
            ddlOperationType.DataTextField = "Text";
            ddlOperationType.DataBind();
            //ddlOperationType.Items.Insert(0, new ListItem("- Operation Type -", ""));
        }
    }
    private void DDL_GroupIndicator(int nRoleID, int nUserID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstGroup = db.mTIndicator.OrderBy(o => o.nOrder).Select(s => new { Value = s.ID, Text = s.Indicator }).ToList();
        if (lstGroup.Count() > 0)
        {
            ddlGroupIndicator.DataSource = lstGroup;
            ddlGroupIndicator.DataValueField = "Value";
            ddlGroupIndicator.DataTextField = "Text";
            ddlGroupIndicator.DataBind();
            //ddlGroupIndicator.Items.Insert(0, new ListItem("- Operation Type -", ""));
        }
    }

    #endregion

    private void SetData(int nUserID)
    {
        var query = db.mTUser.FirstOrDefault(w => w.ID == nUserID);
        if (query != null)
        {
            if (query.cUserType == "0")
            {
                txtNameGc.Text = query.sUserCode + " - " + query.Firstname + " " + query.Lastname;
                txtNameGc.Attributes.Add("disabled", "true");
                txtOrgGc.Text = query.Company;
                txtOrgGc.Attributes.Add("disabled", "true");
                txtEmailGc.Text = query.Email;
                txtEmailGc.Attributes.Add("disabled", "true");
                DivContract.Style.Add("display", "none");
                hdfEmployeeID.Value = query.Username;
                hdfEmployeeName.Value = query.Firstname;
                hdfEmployeeLastName.Value = query.Lastname;
            }
            else
            {
                DivGc.Style.Add("display", "none");
            }
            txtName.Text = query.Firstname;
            txtSurname.Text = query.Lastname;
            txtOrg.Text = query.Company;
            txtEmail.Text = query.Email;
            txtUsername.Text = query.Username;
            txtPassword.Text = query.PasswordEncrypt == null ? "" : STCrypt.Decrypt(query.PasswordEncrypt);
            txtPassword.ReadOnly = true;
            rblStatus.SelectedValue = query.cActive;
            rblUsertype.SelectedValue = query.cUserType;


        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData Get_Facility(List<int> lst)
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
            if (lst.Any())
            {
                lstFacility = SystemFunction.Get_SubFacility_ByMuti(lst, nUserID, nRoleID);
            }
            result.lstData_Facility = lstFacility.Distinct().ToList();
        }
        return result;
    }

    #region By Data Table
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData Get_MenuAdmin(CSearch itemSearch, List<TDataMenu> lst)
    {
        TRetunrLoadData result = new TRetunrLoadData();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<TDataMenu> lst_Menu = new List<TDataMenu>();
        int nUserID = UserAcc.GetObjUser().nUserID;

        if (!string.IsNullOrEmpty(itemSearch.sEditdata)) // เป็นการแก้ไขข้อมูล
        {
            if (itemSearch.sEditdata == "Y")
            {
                foreach (var item in lst)
                {
                    var lst_Data = db.TMenu.Where(W => W.cActive == "Y" && W.sMenuHeadID == 6 && W.nMenuID == item.nMenuID && W.flage_Menu == "L").Select(s => new TDataMenu
                    {
                        nMenuID = item.nMenuID,
                        sMenuName = s.sMenuName,
                        sPermission = item.sPermission,
                    }).ToList();
                    lst_Menu.AddRange(lst_Data);
                }
            }
            else
            {
                if (itemSearch.sEditdata == "1")
                {
                    lst_Menu = db.TMenu.Where(W => W.cActive == "Y" && W.sMenuHeadID == 6 && W.flage_Menu == "L").Select(s => new TDataMenu
                    {
                        nMenuID = s.nMenuID,
                        sMenuName = s.sMenuName,
                        sPermission = "2",
                    }).ToList();
                }
                else if (itemSearch.sEditdata == "6")
                {
                    lst_Menu = db.TMenu.Where(W => W.cActive == "Y" && (W.nMenuID == 34 || W.nMenuID == 36)).Select(s => new TDataMenu
                    {
                        nMenuID = s.nMenuID,
                        sMenuName = s.sMenuName,
                        sPermission = "2",
                    }).ToList();
                }

            }

            //lst_Menu = lst;
        }

        result.lst_Menu = lst_Menu;
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData Add_Operation(List<string> lst_Operation, List<string> lst_Facility, List<string> lst_GroupIndicator, string sPermission, CSearch itemSearch, string[] arrDel, List<T_Data> lstOperation_Data, List<T_Data> lstOperation_DataAll)
    {
        TRetunrLoadData result = new TRetunrLoadData();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<T_Data> lstData = new List<T_Data>();
        List<T_Facility> lst_Fac = new List<T_Facility>();
        List<T_GroupIndicator> lst_Group = new List<T_GroupIndicator>();
        if (!string.IsNullOrEmpty(itemSearch.sEditdata))
        {
            List<int> lstDelGroup_ID = new List<int>();
            List<int> lstDelFac_ID = new List<int>();
            if (itemSearch.sEditdata == "Y")
            {
                lstOperation_DataAll.ForEach(f =>
                {
                    f.IsActive = true;
                });
                if (arrDel != null)
                    //lstDelFac_ID = arrDel.Select(s => s.toIntNullToZero()).ToList();
                    foreach (var i in arrDel)
                    {
                        string[] strArr = null;
                        char[] splitchar = { ',' };
                        strArr = i.Split(splitchar);

                        int nFacID = int.Parse(strArr[0]);
                        int nGroupID = int.Parse(strArr[1]);
                        lstDelFac_ID.Add(nFacID);
                        lstDelGroup_ID.Add(nGroupID);
                        lstOperation_DataAll.Where(w => w.nFacilityID == nFacID && w.nGroupIndicatorID == nGroupID).ToList().ForEach(f =>
                        {

                            f.IsActive = false;
                        });

                    }
                lstData = lstOperation_DataAll.Where(w => w.IsActive == true).Distinct().ToList();
            }
            else
            {
                if (itemSearch.sEditdata == "C")
                {
                    lstData = lstOperation_Data.Distinct().ToList();
                }
                else
                {
                    if (lstOperation_DataAll != null && lstOperation_DataAll.Count > 0)
                    {
                        lstData = lstOperation_DataAll.Distinct().ToList();
                    }
                    else
                    {
                        lstData = lstOperation_Data.Distinct().ToList();
                    }
                }

            }
        }
        else
        {
            if (lst_Facility != null)
            {
                foreach (var item_Fac in lst_Facility)
                {
                    int nID = int.Parse(item_Fac);
                    var fac = (from i in db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.ID == nID).AsEnumerable()
                               from k in db.mOperationType.Where(w => w.cActive == "Y" && w.cDel == "N" && w.ID == i.OperationTypeID)
                               select new T_Facility
                               {
                                   nOperationTypeID = k.ID,
                                   nFacilityID = i.ID,
                                   sOperationName = k.Name,
                                   sFacilityName = i.Name,
                               }).ToList();
                    lst_Fac.AddRange(fac);
                }
            }
            // List<T_Data> lst_Data = new List<T_Data>();
            if (lst_GroupIndicator != null)
            {
                foreach (var item_Group in lst_GroupIndicator)
                {
                    int nID = int.Parse(item_Group);
                    var group = db.mTIndicator.FirstOrDefault(w => w.ID == nID);
                    foreach (var item_Fac in lst_Fac)
                    {
                        lstData.Add(new T_Data
                        {
                            nOperationTypeID = item_Fac.nOperationTypeID,
                            sOperationName = item_Fac.sOperationName,
                            nFacilityID = item_Fac.nFacilityID,
                            sFacilityName = item_Fac.sFacilityName,
                            nGroupIndicatorID = int.Parse(item_Group),
                            sGroupIndicatorName = group.Indicator,
                            sPermission = sPermission,
                        });
                    }
                }
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
                        case 1: lstData = lstData.OrderBy(o => o.sOperationName).ToList(); break;
                        case 2: lstData = lstData.OrderBy(o => o.sFacilityName).ToList(); break;
                        case 3: lstData = lstData.OrderBy(o => o.sGroupIndicatorName).ToList(); break;
                        case 4: lstData = lstData.OrderBy(o => o.sPermission).ToList(); break;
                    }
                }
                break;
            case SystemFunction.DESC:
                {
                    switch (nSortCol)
                    {
                        case 1: lstData = lstData.OrderByDescending(o => o.sOperationName).ToList(); break;
                        case 2: lstData = lstData.OrderByDescending(o => o.sFacilityName).ToList(); break;
                        case 3: lstData = lstData.OrderByDescending(o => o.sGroupIndicatorName).ToList(); break;
                        case 4: lstData = lstData.OrderByDescending(o => o.sPermission).ToList(); break;
                    }
                }
                break;
        }
        #endregion
        result.lstData_All = lstData.Distinct().ToList();
        sysGlobalClass.Pagination dataPage = new sysGlobalClass.Pagination();
        dataPage = SystemFunction.GetPaginationSmall(SystemFunction.GetIntNullToZero(itemSearch.sPageSize), SystemFunction.GetIntNullToZero(itemSearch.sPageIndex), lstData.Count);
        lstData = lstData.Skip(dataPage.nSkipData).Take(dataPage.nTakeData).ToList();

        result.lstData = lstData.Distinct().ToList();

        result.nPageCount = dataPage.nPageCount;
        result.nPageIndex = dataPage.nPageIndex;
        result.sPageInfo = dataPage.sPageInfo;
        result.sContentPageIndex = dataPage.sContentPageIndex;
        result.nStartItemIndex = dataPage.nStartItemIndex;

        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData LoadData_Main(CSearch itemSearch, List<T_RoleUser> lstData_Role, string sUserID)
    {
        TRetunrLoadData result = new TRetunrLoadData();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<T_RoleUser> lstData = new List<T_RoleUser>();

        int nRoleID = UserAcc.GetObjUser().nRoleID;

        if (!string.IsNullOrEmpty(sUserID))
        {
            if (string.IsNullOrEmpty(itemSearch.sEditdata))
            {
                int nUserID = int.Parse(sUserID);

                var TBUser_InRow = db.mTUserInRole.Where(w => w.nUID == nUserID).ToList();

                //var Admin_menu = db.TMenu_Permission.Where(w => w.nUserID == nUserID).ToList();
                var Facility_Menu = db.mTUser_FacilityPermission.Where(w => w.nUserID == nUserID && w.nRoleID != 1).ToList();

                var lstRole = db.mTUserRole.ToList();
                var lst_Facility = db.mTUser_FacilityPermission.ToList();
                if (TBUser_InRow.Any())
                {
                    TBUser_InRow.ForEach(f =>
                    {
                        T_RoleUser g = new T_RoleUser();
                        int nRole = f.nRoleID;
                        g.nRoleID = nRole;
                        g.sRoleName = lstRole.Any(a => a.ID == nRole) ? lstRole.First(w => w.ID == nRole).Name : "";
                        lstData.Add(g);
                    });
                }

                if (lstData.Any())
                {
                    var lstMenu = db.TMenu.Where(w => w.cActive == "Y" && w.sMenuHeadID == 6).ToList();
                    lstData.ForEach(f =>
                    {
                        var gFacilityPerm = db.mTUser_FacilityPermission.Where(w => w.nUserID == nUserID && w.nRoleID == f.nRoleID).ToList();
                        var gFacility = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N").Select(s => new { s.ID, s.Name, s.OperationTypeID }).ToList();
                        var gGroupIndicator = db.mTIndicator.ToList();
                        var gOperation = db.mOperationType.Where(w => w.cActive == "Y" && w.cDel == "N" && w.cManage == "N").ToList();

                        f.lst_Menu = new List<TDataMenu>();
                        f.lstData_Operation = new List<T_Data>();

                        if (f.nRoleID == 1 || f.nRoleID == 6)
                        {
                            var Admin_menu = db.TMenu_Permission.Where(w => w.nUserID == nUserID && w.nRoleID == f.nRoleID).ToList();
                            f.lst_Menu = new List<TDataMenu>();
                            if (f.nRoleID == 1)
                            {
                                f.lst_Menu = lstMenu.Select(s => new TDataMenu
                                {
                                    nMenuID = s.nMenuID,
                                    sMenuName = lstMenu.Any(a => a.nMenuID == s.nMenuID) ? lstMenu.First(w => w.nMenuID == s.nMenuID).sMenuName : "",
                                    //sPermission = s.nPermission.HasValue ? s.nPermission.Value.ToString() : ""
                                    sPermission = Admin_menu.Any(a => a.nMenuID == s.nMenuID) ? Admin_menu.First(a => a.nMenuID == s.nMenuID).nPermission.Value.ToString() : "",
                                }).ToList();
                            }
                            else if (f.nRoleID == 6)
                            {
                                //var Company_Admin_menu = db.TMenu_Permission.Where(w => w.nUserID == nUserID).ToList();
                                f.lst_Menu = lstMenu.Select(s => new TDataMenu
                                {
                                    nMenuID = s.nMenuID,
                                    sMenuName = lstMenu.Any(a => a.nMenuID == s.nMenuID) ? lstMenu.First(w => w.nMenuID == s.nMenuID).sMenuName : "",
                                    sPermission = Admin_menu.Any(a => a.nMenuID == s.nMenuID) ? Admin_menu.First(a => a.nMenuID == s.nMenuID).nPermission.Value.ToString() : "",
                                    //sPermission = s.nPermission.HasValue ? s.nPermission.Value.ToString() : ""
                                }).ToList();
                            }

                            int nOperation = 0;
                            if (gFacilityPerm.Any())
                            {
                                f.lstData_Operation = gFacilityPerm.Select(s => new T_Data
                                {
                                    IsActive = false,
                                    nFacilityID = s.nFacilityID,
                                    nGroupIndicatorID = s.nGroupIndicatorID,
                                    nOperationTypeID = nOperation = gFacility.Any(a => a.ID == s.nFacilityID) ? gFacility.First(a => a.ID == s.nFacilityID).OperationTypeID : 0,
                                    sPermission = s.nPermission + "",
                                    sFacilityName = gFacility.Any(a => a.ID == s.nFacilityID) ? gFacility.First(a => a.ID == s.nFacilityID).Name : "",
                                    sGroupIndicatorName = gGroupIndicator.Any(a => a.ID == s.nGroupIndicatorID) ? gGroupIndicator.First(a => a.ID == s.nGroupIndicatorID).Indicator : "",
                                    sOperationName = gOperation.Any(a => a.ID == nOperation) ? gOperation.First(a => a.ID == nOperation).Name : ""
                                }).ToList();
                                f.lstData_Operation = f.lstData_Operation.Where(w => w.sOperationName != "" || w.sFacilityName != "").ToList();
                            }
                        }
                        else
                        {
                            int nOperation = 0;
                            if (gFacilityPerm.Any())
                            {
                                f.lstData_Operation = gFacilityPerm.Select(s => new T_Data
                                {
                                    IsActive = false,
                                    nFacilityID = s.nFacilityID,
                                    nGroupIndicatorID = s.nGroupIndicatorID,
                                    nOperationTypeID = nOperation = gFacility.Any(a => a.ID == s.nFacilityID) ? gFacility.First(a => a.ID == s.nFacilityID).OperationTypeID : 0,
                                    sPermission = s.nPermission + "",
                                    sFacilityName = gFacility.Any(a => a.ID == s.nFacilityID) ? gFacility.First(a => a.ID == s.nFacilityID).Name : "",
                                    sGroupIndicatorName = gGroupIndicator.Any(a => a.ID == s.nGroupIndicatorID) ? gGroupIndicator.First(a => a.ID == s.nGroupIndicatorID).Indicator : "",
                                    sOperationName = gOperation.Any(a => a.ID == nOperation) ? gOperation.First(a => a.ID == nOperation).Name : ""
                                }).ToList();
                                f.lstData_Operation = f.lstData_Operation.Where(w => w.sOperationName != "" || w.sFacilityName != "").ToList();
                            }
                        }
                    });
                }
                result.lstData_Role = lstData.Distinct().ToList();
            }
            else
            {
                result.lstData_Role = lstData_Role.Distinct().ToList();
            }
        }
        else
        {
            result.lstData_Role = lstData_Role.Distinct().ToList();
        }

        sysGlobalClass.Pagination dataPage = new sysGlobalClass.Pagination();
        dataPage = SystemFunction.GetPaginationSmall(SystemFunction.GetIntNullToZero(itemSearch.sPageSize), SystemFunction.GetIntNullToZero(itemSearch.sPageIndex), result.lstData_Role.Count);
        result.lstData_Role = result.lstData_Role.Skip(dataPage.nSkipData).Take(dataPage.nTakeData).ToList();


        result.lstData_Role = result.lstData_Role.Distinct().ToList();

        foreach (var item in result.lstData_Role)
        {
            if (nRoleID == 6)
            {
                if (item.nRoleID == 2 || item.nRoleID == 3 || item.nRoleID == 5)
                {
                    if (IsView)
                    {
                        item.sLink = "<button type='button' class='btn btn-primary btnInput' onclick='EditData(" + item.nRoleID + ")'><i class='fa fa-search'></i>&nbsp;View</button>";
                    }
                    else
                    {
                        item.sLink = "<button type='button' class='btn btn-warning btnInput' onclick='EditData(" + item.nRoleID + ")'><i class='fa fa-edit'></i>&nbsp;Edit</button>";
                        item.sLinkDel = "<button type='button' class='btn btn-danger btnInput' onclick='DelData(" + item.nRoleID + ")'><i class='fa fa-delete'></i>&nbsp;Delete</button>";
                    }
                }
                else
                {
                    item.sLink = "";
                    item.sLinkDel = "";
                }
            }
            else
            {
                if (IsView)
                {
                    item.sLink = "<button type='button' class='btn btn-primary btnInput' onclick='EditData(" + item.nRoleID + ")'><i class='fa fa-search'></i>&nbsp;View</button>";
                }
                else
                {
                    item.sLink = "<button type='button' class='btn btn-warning btnInput' onclick='EditData(" + item.nRoleID + ")'><i class='fa fa-edit'></i>&nbsp;Edit</button>";
                    item.sLinkDel = "<button type='button' class='btn btn-danger btnInput' onclick='DelData(" + item.nRoleID + ")'><i class='fa fa-delete'></i>&nbsp;Delete</button>";
                }
            }
        }
        result.lstData_Role = result.lstData_Role.OrderByDescending(o => o.sLink).ToList();

        result.nPageCount = dataPage.nPageCount;
        result.nPageIndex = dataPage.nPageIndex;
        result.sPageInfo = dataPage.sPageInfo;
        result.sContentPageIndex = dataPage.sContentPageIndex;
        result.nStartItemIndex = dataPage.nStartItemIndex;

        return result;
    }
    #endregion

    #region SaveData
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(CSaveData data)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();

            Func<string, int?, bool> CheckDuplicateName = (name, id) =>
              {
                  bool Isdup = false;
                  var q = db.mTUser.Where(w => (id.HasValue ? w.ID != id : true) && w.Username == name && w.cDel == "N");
                  Isdup = q.Any();
                  return Isdup;
              };
            //Func<string, int?, bool> CheckDuplicateEmployeeID = (sUserCode, id) =>
            //{
            //    bool Isdup = false;
            //    var q = db.mTUser.Where(w => (id.HasValue ? w.ID != id : true) && w.Username == sUserCode);
            //    Isdup = q.Any();
            //    return Isdup;
            //};


            //string sUserID = data.sUserID.STCDecrypt();
            if (!string.IsNullOrEmpty(data.sUserID))
            {
                int nUserID = data.sUserID.toIntNullToZero();

                var query = db.mTUser.FirstOrDefault(w => w.ID == nUserID);
                if (query != null)
                {
                    if (query.cUserType == "0") //Employee GC
                    {
                        if (!CheckDuplicateName(data.sUserCode, nUserID))
                        {
                            query.Firstname = data.sName.Trim();
                            query.Lastname = data.sSurName.Trim();
                            query.Email = data.sEmail.Trim();
                            query.Company = data.sOrg.Trim();
                            query.sUserCode = data.sUserCode.Trim();
                            query.Username = data.sUserCode.Trim();
                        }
                        else
                        {
                            result.Msg = "Duplicate Employee Code !";
                            result.Status = SystemFunction.process_Failed;
                            return result;
                        }

                    }
                    else
                    {
                        if (!CheckDuplicateName(data.sUsername, nUserID))
                        {
                            query.Username = data.sUsername.Trim();
                            query.Email = data.sEmail.Trim();
                            query.Company = data.sOrg.Trim();
                            query.Firstname = data.sName.Trim();
                            query.Lastname = data.sSurName.Trim();
                            query.Password = STCrypt.encryptMD5(data.sPassword.Trim());
                            query.PasswordEncrypt = STCrypt.Encrypt(data.sPassword.Trim());
                        }
                        else
                        {
                            result.Msg = "Duplicate Username !";
                            result.Status = SystemFunction.process_Failed;
                            return result;
                        }

                    }

                    query.cActive = data.sStatus.Trim();
                    query.cUserType = data.sUserType.Trim();
                    query.nUpdateID = UserAcc.GetObjUser().nUserID;
                    query.dUpdate = DateTime.Now;

                    bool Ispass = db.SaveChanges() > 0;
                    if (Ispass)
                    {
                        if (data.lstData_Role.Any())
                        {
                            //db.mTUser_FacilityPermission.Where(w => w.nUserID == nUserID).ToList().ForEach(d =>
                            //{
                            //    db.mTUser_FacilityPermission.Remove(d);
                            //});
                            //db.TMenu_Permission.Where(w => w.nUserID == nUserID).ToList().ForEach(d =>
                            //{
                            //    db.TMenu_Permission.Remove(d);
                            //});
                            string _SQL_DelMenu_Fac = @" DELETE FROM mTUser_FacilityPermission WHERE nUserID = " + nUserID + @" ";
                            string _SQL_DelMenu_Admin = @" DELETE FROM TMenu_Permission WHERE nUserID = " + nUserID + @" ";
                            string _SQL_DelUser_Role = @" DELETE FROM mTUserInRole WHERE nUID = " + nUserID + @" ";

                            CommonFunction.ExecuteSQL(SystemFunction.strConnect, _SQL_DelMenu_Fac);
                            CommonFunction.ExecuteSQL(SystemFunction.strConnect, _SQL_DelMenu_Admin);
                            CommonFunction.ExecuteSQL(SystemFunction.strConnect, _SQL_DelUser_Role);

                            var lst = data.lstData_Role.ToList();
                            foreach (var item in lst)
                            {
                                if (item.nRoleID == 1 || item.nRoleID == 6)
                                {
                                    item.lst_Menu.ForEach(f1 =>
                                    {
                                        TMenu_Permission t_Admin = new TMenu_Permission();

                                        t_Admin.nUserID = nUserID;
                                        t_Admin.nRoleID = int.Parse(item.nRoleID + "");
                                        t_Admin.nMenuID = int.Parse(f1.nMenuID + "");
                                        t_Admin.nPermission = int.Parse(f1.sPermission + "");
                                        db.TMenu_Permission.Add(t_Admin);

                                    });
                                }
                                item.lstData_Operation.ForEach(f3 =>
                                    {
                                        if (item.lstData_Operation.Any())
                                        {
                                            mTUser_FacilityPermission t_OtherMenu = new mTUser_FacilityPermission();

                                            t_OtherMenu.nUserID = nUserID;
                                            t_OtherMenu.nRoleID = int.Parse(item.nRoleID + "");
                                            t_OtherMenu.nFacilityID = int.Parse(f3.nFacilityID + "");
                                            t_OtherMenu.nGroupIndicatorID = int.Parse(f3.nGroupIndicatorID + "");
                                            t_OtherMenu.nPermission = int.Parse(f3.sPermission);
                                            db.mTUser_FacilityPermission.Add(t_OtherMenu);
                                        }
                                    });
                                mTUserInRole r = new mTUserInRole();
                                r.nUID = nUserID;
                                r.nRoleID = int.Parse(item.nRoleID + "");
                                db.mTUserInRole.Add(r);

                            }
                            db.SaveChanges();
                            result.Status = SystemFunction.process_Success;
                        }
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Not found data !";
                    return result;
                }
            }
            else
            {
                int nUserID = db.mTUser.Any() ? db.mTUser.Max(m => m.ID) + 1 : 1;
                mTUser t = new mTUser();
                if (data.sUserType == "0") //GC
                {
                    if (!CheckDuplicateName(data.sUserCode, null))
                    {
                        t.Firstname = data.sName.Trim();
                        t.Lastname = data.sSurName.Trim();
                        t.Email = data.sEmail.Trim();
                        t.Company = data.sOrg.Trim();
                        t.sUserCode = data.sUserCode.Trim();
                        t.Username = data.sUserCode.Trim();
                    }
                    else
                    {
                        result.Msg = "Duplicate Employee Code !";
                        result.Status = SystemFunction.process_Failed;
                        return result;
                    }
                }
                else
                {
                    if ((!CheckDuplicateName(data.sUsername, null)))
                    {
                        t.Username = data.sUsername.Trim();
                        t.Email = data.sEmail.Trim();
                        t.Company = data.sOrg.Trim();
                        t.Firstname = data.sName.Trim();
                        t.Lastname = data.sSurName.Trim();
                        t.Password = STCrypt.encryptMD5(data.sPassword.Trim());
                        t.PasswordEncrypt = STCrypt.Encrypt(data.sPassword.Trim());
                    }
                    else
                    {
                        result.Msg = "Duplicate User name !";
                        result.Status = SystemFunction.process_Failed;
                        return result;
                    }
                }
                t.ID = nUserID;
                t.cActive = data.sStatus.Trim();
                t.cUserType = data.sUserType.Trim();
                t.nCreateID = UserAcc.GetObjUser().nUserID;
                t.dCreate = DateTime.Now;
                t.nUpdateID = UserAcc.GetObjUser().nUserID;
                t.dUpdate = DateTime.Now;
                t.cDel = "N";
                db.mTUser.Add(t);

                bool Ispass = db.SaveChanges() > 0;
                if (Ispass)
                {
                    if (data.lstData_Role.Any())
                    {
                        string _SQL_DelMenu_Fac = @" DELETE FROM mTUser_FacilityPermission WHERE nUserID = " + nUserID + @" ";
                        string _SQL_DelMenu_Admin = @" DELETE FROM TMenu_Permission WHERE nUserID = " + nUserID + @" ";
                        string _SQL_DelUser_Role = @" DELETE FROM mTUserInRole WHERE nUID = " + nUserID + @" ";

                        CommonFunction.ExecuteSQL(SystemFunction.strConnect, _SQL_DelMenu_Fac);
                        CommonFunction.ExecuteSQL(SystemFunction.strConnect, _SQL_DelMenu_Admin);
                        CommonFunction.ExecuteSQL(SystemFunction.strConnect, _SQL_DelUser_Role);
                        var lst = data.lstData_Role.ToList();
                        foreach (var item in lst)
                        {
                            if (item.nRoleID == 1 || item.nRoleID == 6)
                            {
                                item.lst_Menu.ForEach(f1 =>
                                {
                                    TMenu_Permission t_Admin = new TMenu_Permission();

                                    t_Admin.nUserID = nUserID;
                                    t_Admin.nRoleID = int.Parse(item.nRoleID + "");
                                    t_Admin.nMenuID = int.Parse(f1.nMenuID + "");
                                    t_Admin.nPermission = int.Parse(f1.sPermission + "");
                                    db.TMenu_Permission.Add(t_Admin);
                                });

                            }
                            if (item.lstData_Operation.Any())
                            {
                                item.lstData_Operation.ForEach(f3 =>
                                {
                                    mTUser_FacilityPermission t_OtherMenu = new mTUser_FacilityPermission();

                                    t_OtherMenu.nUserID = nUserID;
                                    t_OtherMenu.nRoleID = int.Parse(item.nRoleID + "");
                                    t_OtherMenu.nFacilityID = int.Parse(f3.nFacilityID + "");
                                    t_OtherMenu.nGroupIndicatorID = int.Parse(f3.nGroupIndicatorID + "");
                                    t_OtherMenu.nPermission = int.Parse(f3.sPermission);
                                    db.mTUser_FacilityPermission.Add(t_OtherMenu);
                                });
                            }

                            mTUserInRole r = new mTUserInRole();
                            r.nUID = nUserID;
                            r.nRoleID = int.Parse(item.nRoleID + "");
                            db.mTUserInRole.Add(r);
                        }
                        db.SaveChanges();
                        result.Status = SystemFunction.process_Success;
                    }
                }
            }

        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }
    #endregion

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod ResetPass(string sUserID)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        if (UserAcc.UserExpired())
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            if (!string.IsNullOrEmpty(sUserID))
            {
                int nUserID = int.Parse(sUserID);
                bool Ispass = false;
                var query = db.mTUser.FirstOrDefault(w => w.ID == nUserID);
                if (query != null)
                {
                    try
                    {
                        query.Password = STCrypt.encryptMD5(WebConfigurationManager.AppSettings["DefaultPass"] + "");
                        query.PasswordEncrypt = STCrypt.Encrypt(WebConfigurationManager.AppSettings["DefaultPass"] + "");

                        db.SaveChanges();
                        Ispass = true;
                    }
                    catch (Exception e)
                    {
                        result.Msg = e.Message.ToString();
                        result.Status = SystemFunction.process_Failed;
                    }
                    if (Ispass)
                    {
                        string sTitle = "";
                        string sText = "";
                        string subject = "";
                        string message = "";
                        string sURL = "";
                        string sFoot = "";// "Should you have any questions about RD&T work process.";

                        // sURL = Applicationpath + "login_forget.aspx?str=" + SystemFunction.Encrypt_UrlEncrypt(nDocID + "");

                        subject = "New password | " + SystemFunction.SystemName + "";

                        sText += "<p>Your new password is :" + STCrypt.Decrypt(query.PasswordEncrypt) + "</p>";

                        string From = "phongsawat.p@softthai.com";
                        string To = "chotika.n@softthai.com,phongsawat.p@softthai.com";
                        message = string.Format(GET_TemplateEmail(),
                        "Dear " + query.Firstname + ' ' + query.Lastname,
                        sText,
                        sURL,
                        sFoot,
                        "",
                        "");
                        Workflow.DataMail_log log = new Workflow.DataMail_log();
                        log = SystemFunction.SendMailAll(WebConfigurationManager.AppSettings["SystemMail"] + "", WebConfigurationManager.AppSettings["RecieveDemoMail"] + "", "", "", subject, message, "");
                        log.nDataID = SystemFunction.GetIntNullToZero(sUserID);
                        log.sPageName = "admin_user_info_update.aspx";
                        new Workflow().SaveLogMail(log);
                        if (log.bStatus)
                        {
                            result.Status = SystemFunction.process_Success;
                        }
                    }
                }
                result.Content = "admin_user_info_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(sUserID + "")) + "";
            }
        }
        return result;
    }

    #region GET_Employee GC
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static GC_HCM_HR_RestServices.RootObject getEmployees(string sSearch)
    {
        GC_HCM_HR_RestServices.RootObject result = new GC_HCM_HR_RestServices.RootObject();
        result = GetEmployeeData(sSearch);
        return result;
    }

    public static GC_HCM_HR_RestServices.RootObject GetEmployeeData(string sSearch)
    {
        GC_HCM_HR_RestServices.RootObject _data = new GC_HCM_HR_RestServices.RootObject();
        string URI = "https://hr-webservices.pttgc.corp:4330/pttgc/hcm/hrwebservices/project/services/HR/services_conf/HR_WebServices_CONF.xsodata/";
        string URIUser = "ODATA_CONF";//odata
        string URIPass = "MoneyBag#0";//Hana#2017 Hana#1234
        string URLRestFormat = "json";
        string SyncType = "REST";//  SERVICE
        string SyncJobSkill = "Y";// 
                                  //UserID for access these services is  “odata” with password “Hana#1234” $select=EmployeeID,NameTH,MobilePhone,CompanyName,CompanyCode,CompanyShortTxt,EmailAddress&and (EmailAddress ne '') 
                                  // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls12
            | SecurityProtocolType.Ssl3;


        ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        var url = URI + "EmployeeService?$top={0}&$format={1}&$select=EmployeeID,Name,NameTH,ENTitle,ENFirstName,ENLastName,MobilePhone,CompanyName,CompanyCode,CompanyShortTxt,EmailAddress&$filter=" + ((sSearch != "") ? "substringof('" + sSearch + "', NameTH) or substringof('" + sSearch + "', Name) or substringof('" + sSearch + "', EmployeeID)" : "") + "and (PositionID ne '')";
        //+ ((sSearch != "" && sSearch != "") ? " and PositionID eq '" + sSearch + "'" : "");
        string _url = String.Format(url, "10", URLRestFormat);
        //_url += "&$filter=EmployeeID eq '26006044'";
        WebClient serviceRequest = new WebClient(); serviceRequest.Encoding = UTF8Encoding.UTF8;
        serviceRequest.Credentials = new System.Net.NetworkCredential(URIUser, URIPass);
        string response = serviceRequest.DownloadString(new Uri(_url));

        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GC_HCM_HR_RestServices.RootObject));
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response)))
        {
            _data = JsonHelper.JsonDeserialize<GC_HCM_HR_RestServices.RootObject>(response);
        }
        return _data;
    }
    #endregion


    #region Send Mail
    public static string GET_TemplateEmail()
    {
        return @"<div id=':km' class='ii gt adP adO'>
                <div id=':l9' class='a3s aXjCH m15f05c377e26ea4b'>
                    <u></u>
                    <div style='background: #f9f9f9'>
                        <div style='background-color: #f9f9f9'>

                            <div style='margin: 0px auto; /* max-width: 630px; */background: transparent;'>
                                <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: transparent;' align='center' border='0'>
                                    <tbody>
                                        <tr>
                                            <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; /* padding: 40px 0px */'>
                                                <div style='font-size: 1px; line-height: 12px'>&nbsp;{4}</div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div style='max-width: 640px; margin: 0 auto; border-radius: 4px; overflow: hidden'>
                                <div style='margin: 0px auto; max-width: 640px; background: #ffffff'>
                                    <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: #ffffff' align='center' border='0'>
                                        <tbody>
                                            <tr>
                                                <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; padding: 40px 70px'>
                                                    <div aria-labelledby='mj-column-per-100' class='m_5841562294398106085mj-column-per-100 m_5841562294398106085outlook-group-fix' style='vertical-align: top; display: inline-block; direction: ltr; font-size: 13px; text-align: left; width: 100%'>
                                                        <table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'>
                                                            <tbody>
                                                                <tr>
                                                                    <td style='word-break: break-word; font-size: 0px; padding: 0px' align='left'>
                                                                        <div style='color: #737f8d; font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-size: 16px; line-height: 24px; text-align: left'>

                                                                            <h2 style='font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-weight: 500; font-size: 20px; color: #4f545c; letter-spacing: 0.27px'>{0}</h2>
                                                                            {1}
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style='word-break: break-word; font-size: 0px; padding: 0px' align='left'>
                                                                        <div style='color: #737f8d; font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-size: 16px; line-height: 24px; text-align: left'>
                                                                            {5}
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style='word-break: break-word; font-size: 0px; padding-top: 15px;'>
                                                                        <p style='font-size: 1px; margin: 0px auto; border-top: 1px solid #dcddde; width: 100%'></p>
                                                                    </td>
                                                                </tr>
                                                                <tr style='display:none;'>
                                                                    <td style='word-break: break-word; font-size: 0px; padding: 0px' align='left'>
                                                                        <div style='color: #747f8d; font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-size: 13px; line-height: 16px; text-align: left'>
                                                                            <p>
                                                                                {3}
                                                                            </p>
                                                                            <p>
                                                                                Best regards,<br>
                                                                                Technology Management System Team
                                                                            </p>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div style='margin: 0px auto; max-width: 640px; background: transparent'>
                                <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: transparent' align='center' border='0'>
                                    <tbody>
                                        <tr>
                                            <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; padding: 0px'>
                                                <div aria-labelledby='mj-column-per-100' class='m_5841562294398106085mj-column-per-100 m_5841562294398106085outlook-group-fix' style='vertical-align: top; display: inline-block; direction: ltr; font-size: 13px; text-align: left; width: 100%'>
                                                    <table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'>
                                                        <tbody>
                                                            <tr>
                                                                <td style='word-break: break-word; font-size: 0px'>
                                                                    <div style='font-size: 1px; line-height: 12px'>&nbsp;</div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>";
    }
    #endregion

    #region class 
    [Serializable]
    public class CSearch : sysGlobalClass.CommonLoadData
    {
        public string sSearch { get; set; }
        public string sStatus { get; set; }
        public string sPrms { get; set; }
        public string cDel { get; set; }
        public string sEditdata { get; set; }
    }
    [Serializable]
    public class TRetunrLoadData : sysGlobalClass.Pagination
    {
        public List<TDataMenu> lst_Menu { get; set; }
        public List<sysGlobalClass.T_Facility> lstData_Facility { get; set; }
        public List<T_Data> lstData { get; set; }
        public List<T_Data> lstData_All { get; set; }
        public List<T_RoleUser> lstData_Role { get; set; }
    }

    [Serializable]
    public class TDataMenu
    {
        public int? nMenuID { get; set; }
        public int? sMenuOrder { get; set; }
        public string sMenuName { get; set; }
        public int? sMenuHeadID { get; set; }
        public int? nPermission { get; set; }
        public int? nRoleID { get; set; }
        public string cActive { get; set; }
        public string sPermission { get; set; }

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
    [Serializable]
    public class T_GroupIndicator
    {
        public int nGroupIndicatorID { get; set; }
        public string sGroupIndicatorName { get; set; }
    }
    [Serializable]
    public class T_Data
    {
        public int nID { get; set; }
        public int nFacilityID { get; set; }
        public string sFacilityName { get; set; }
        public int nOperationTypeID { get; set; }
        public string sOperationName { get; set; }
        public int nGroupIndicatorID { get; set; }
        public string sGroupIndicatorName { get; set; }
        public string sPermission { get; set; }
        public bool IsActive { get; set; }

    }
    [Serializable]
    public class T_RoleUser
    {
        public int? nRoleID { get; set; }
        public string sRoleName { get; set; }
        public List<TDataMenu> lst_Menu { get; set; }
        public List<T_Data> lstData_Operation { get; set; }
        public string sLink { get; set; }
        public string sLinkDel { get; set; }
    }
    [Serializable]
    public class T_UserInfo
    {
        public string sUsername { get; set; }
        public string sPassword { get; set; }
        public string sFirstname { get; set; }
        public string sCompany { get; set; }
        public string sBusinessUnit { get; set; }
        public string sDepartment { get; set; }
        public string sPosition { get; set; }
        public string sEmail { get; set; }
    }

    [Serializable]
    public class CSaveData
    {
        public string sUserID { get; set; }
        public string sName { get; set; }
        public string sNameGc { get; set; }
        public string sUserCode { get; set; }
        public string sOrgGc { get; set; }
        public string sEmailGc { get; set; }
        public string sSurName { get; set; }
        public string sOrg { get; set; }
        public string sEmail { get; set; }
        public string sUsername { get; set; }
        public string sPassword { get; set; }
        public string sStatus { get; set; }
        public string sUserType { get; set; }
        public List<T_RoleUser> lstData_Role { get; set; }
    }

    [Serializable]
    public class CAutucomplete
    {
        public string nID { get; set; }
        public string sName { get; set; }
        public string sEmail { get; set; }
        public string sUnitName { get; set; }
        public string sUnitID { get; set; }
        public string sPhone { get; set; }
        public string sFullName { get; set; }
    }
    #endregion

}