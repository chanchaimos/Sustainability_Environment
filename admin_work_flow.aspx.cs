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

public partial class admin_work_flow : System.Web.UI.Page
{
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
                SystemFunction.BindDropdownPageSize(ddlPageSize, null);
                GETDDL();

                int Prms = SystemFunction.GetPermissionMenu(36);
                //hdfPrmsMenu.Value = Prms + "";
                //bool isView = Prms == 1;
                //if (isView)
                //{
                //    DivCreate_Workflow.Visible = false;
                //    ckbAll.Visible = false;
                //    btnDel.Visible = false;
                //}
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

            //var Indicator = db.mTIndicator.ToList();
            //var Facility = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N").ToList();
            //var Operation = db.mOperationType.Where(w => w.cActive == "Y" && w.cDel == "N").ToList();
            //var TbUser = db.mTUser.Where(w => w.cActive == "Y" && w.cDel == "N").ToList();
            int Operation_ID = 0;
            string sCon = "";

            int nRoleID = UserAcc.GetObjUser().nRoleID;
            int nUserID = UserAcc.GetObjUser().nUserID;

            if (nRoleID == 6)
            {
                var Get_Fac = db.mTUser_FacilityPermission.Where(w => w.nUserID == nUserID && w.nRoleID == nRoleID).ToList(); // User And Role Get Facility
                var lst_SubFac = Get_Fac.Select(id => id.nFacilityID).ToList(); //Facility ID in Company_admin 
                if (lst_SubFac.Any())
                {
                    string Format = "";
                    lst_SubFac.ForEach(f =>
                    {
                        int FacID = f;
                        Format += "," + FacID + "";
                    });
                    Format = Format.Remove(0, 1);
                    sCon += @" AND mtf.IDFac IN (" + Format + ") ";
                    //sCon += @" AND tu.ID IN (" + Format + ") ";
                }
            }


            string sSearch = itemSearch.sSearch.Trims().ToLower();
            if (!string.IsNullOrEmpty(sSearch))
            {
                sCon += @" AND (LOWER(tu.Firstname) LIKE '%" + sSearch + "%' COLLATE THAI_CI_AI OR LOWER(tu.Lastname) LIKE '%" + sSearch + "%' COLLATE THAI_CI_AI OR LOWER(tu1.Firstname) LIKE '%" + sSearch + "%' COLLATE THAI_CI_AI OR LOWER(tu1.Lastname) LIKE '%" + sSearch + "%' COLLATE THAI_CI_AI) ";
            }
            if (!string.IsNullOrEmpty(itemSearch.sOperationID))
            {
                sCon += @" AND mf.OperationTypeID = " + itemSearch.sOperationID + @" ";
            }
            if (!string.IsNullOrEmpty(itemSearch.sFacilityID))
            {
                sCon += @" AND mtf.IDFac = " + itemSearch.sFacilityID + @" ";
            }
            if (!string.IsNullOrEmpty(itemSearch.sIndicatorID))
            {
                sCon += @" AND mtf.IDIndicator = " + itemSearch.sIndicatorID + @" ";
            }
            //&& (!string.IsNullOrEmpty(sSearch) ? w.Name.ToLower().Contains(sSearch) : true)
            string _SQL = @" SELECT mtf.IDFac as IDFac
            ,mtf.IDIndicator as IDIndicator
            ,mtf.L1 as L1
            ,mtf.L2 as L2
            ,mtf.dUpdate as dUpdate
	        ,mti.Indicator as sGroupName
	        ,mf.OperationTypeID as IDOperation
	        ,ISNULL(mot.Name,'') as sOperationName
	        ,ISNULL(mf.Name,'') as sFacilityName
	        ,ISNULL(tu.Firstname,'') +' '+ ISNULL(tu.Lastname,'') AS sManagerName
	  	    ,ISNULL(tu1.Firstname,'') +' '+ ISNULL(tu1.Lastname,'') AS sEnvironName
            FROM mTWorkFlow mtf
            INNER JOIN mTIndicator mti ON mtf.IDIndicator = mti.ID
            INNER JOIN mTFacility mf ON mf.ID = mtf.IDFac AND mf.cActive = 'Y' AND mf.cDel = 'N'
            INNER JOIN mOperationType mot ON mot.ID = mf.OperationTypeID AND mot.cActive = 'Y' AND mot.cDel = 'N'
            LEFT JOIN mTUser tu ON tu.ID = mtf.L1 AND tu.cActive = 'Y' AND tu.cDel = 'N'
            LEFT JOIN mTUser tu1 ON tu1.ID = mtf.L2 AND tu1.cActive = 'Y' AND tu1.cDel = 'N'
            WHERE mtf.cDel ='N' " + sCon + @"
            ORDER BY mtf.dUpdate DESC ";
            DataTable dt = CommonFunction.Get_Data(SystemFunction.strConnect, _SQL);
            lstData = CommonFunction.ConvertDatableToList<TDataTable>(dt).ToList();

            //CommonFunction.
            //lstData = db.mTWorkFlow.Where(w => w.cDel == "N").AsEnumerable().Select(s => new TDataTable
            //{
            //    IDFac = s.IDFac,
            //    IDOperation = Operation_ID = Facility.Any(a => a.ID == s.IDFac && a.cDel == "N" && a.cActive == "Y") ? Facility.First(a => a.ID == s.IDFac && a.cDel == "N" && a.cActive == "Y").OperationTypeID : 0,
            //    IDIndicator = s.IDIndicator,
            //    sFacilityName = Facility.Any(a => a.ID == s.IDFac) ? Facility.First(a => a.ID == s.IDFac && a.cDel == "N" && a.cActive == "Y").Name : "",
            //    sGroupName = Indicator.Any(a => a.ID == s.IDIndicator) ? Indicator.First(a => a.ID == s.IDIndicator).Indicator : "",
            //    sOperationName = Operation.Any(a => a.ID == Operation_ID) ? Operation.First(a => a.ID == Operation_ID).Name : "",
            //    L1 = s.L1,
            //    L2 = s.L2,
            //    dUpdate = s.dUpdate,
            //    sManagerName = TbUser.Any(a => a.ID == s.L1) ? TbUser.First(a => a.ID == s.L1 && a.cActive == "Y" && a.cDel == "N").Firstname + ' ' + TbUser.First(a => a.ID == s.L1 && a.cActive == "Y" && a.cDel == "N").Lastname : "",
            //    sEnvironName = TbUser.Any(a => a.ID == s.L2) ? TbUser.First(a => a.ID == s.L2 && a.cActive == "Y" && a.cDel == "N").Firstname + ' ' + TbUser.First(a => a.ID == s.L2 && a.cActive == "Y" && a.cDel == "N").Lastname : "",
            //}).OrderByDescending(o => o.dUpdate).ToList();
            lstData = lstData.Where(w => w.sOperationName != "" && w.sFacilityName != "").ToList();
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
                            case 3: lstData = lstData.OrderBy(o => o.sGroupName).ToList(); break;
                            case 4: lstData = lstData.OrderBy(o => o.sManagerName).ToList(); break;
                            case 5: lstData = lstData.OrderBy(o => o.sEnvironName).ToList(); break;
                        }
                    }
                    break;
                case SystemFunction.DESC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderByDescending(o => o.sOperationName).ToList(); break;
                            case 2: lstData = lstData.OrderByDescending(o => o.sFacilityName).ToList(); break;
                            case 3: lstData = lstData.OrderByDescending(o => o.sGroupName).ToList(); break;
                            case 4: lstData = lstData.OrderByDescending(o => o.sManagerName).ToList(); break;
                            case 5: lstData = lstData.OrderByDescending(o => o.sEnvironName).ToList(); break;
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
            //    item.sLink = "<a class='btn btn-warning' href='admin_company_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.IDOperation.ToString())) + "'><i class='fa fa-edit'></i>&nbsp;Edit</a>";
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
                foreach (var item in arrValue)
                {
                    string[] strArr = null;
                    char[] splitchar = { ',' };
                    strArr = item.Split(splitchar);

                    int nFacID = int.Parse(strArr[0]);
                    int nGroupID = int.Parse(strArr[1]);
                    int nUserID = UserAcc.GetObjUser().nUserID;
                    db.mTWorkFlow.Where(w => w.IDFac == nFacID && w.IDIndicator == nGroupID).ToList().ForEach(x =>
                     {
                         db.mTWorkFlow.Remove(x);
                         //x.cDel = "Y";
                         //x.dUpdate = DateTime.Now;
                         //x.nUpdateID = nUserID;
                     });

                }
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
    public static TRetunrLoadData ConfirmData(List<TDataDupicate> lstData, List<TDataFacAndIndicator> lstData_Save)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        TRetunrLoadData result = new TRetunrLoadData();
        string Del_SQL = "";
        string Insert_SQL = "";
        DateTime now = DateTime.Now;
        int nUserID = UserAcc.GetObjUser().nUserID;
        if (UserAcc.UserExpired())
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            if (lstData.Count > 0)
            {
                foreach (var item in lstData)
                {
                    Del_SQL += @" DELETE FROM mTWorkFlow WHERE IDFac = " + item.nFacID + @" AND IDIndicator = " + item.nIndicatorID + @" ";
                    Insert_SQL += @" INSERT INTO mTWorkFlow (IDFac,IDIndicator,L1,L2,nUpdateID,dUpdate,cDel) VALUES ('" + item.nFacID + @"','" + item.nIndicatorID + @"','" + item.sManagerID + @"','" + item.sEnvironID + @"','" + nUserID + @"','" + now + @"','N') ";
                }
                SystemFunction.ExecuteSQL(SystemFunction.strConnect, Del_SQL);
                SystemFunction.ExecuteSQL(SystemFunction.strConnect, Insert_SQL);
                result.Status = SystemFunction.process_Success;
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Data not found";
            }

            if (lstData_Save.Count > 0)
            {
                foreach (var item in lstData_Save)
                {
                    Del_SQL += @" DELETE FROM mTWorkFlow WHERE IDFac = " + item.nFacID + @" AND IDIndicator = " + item.nIndicatorID + @" ";
                    Insert_SQL += @" INSERT INTO mTWorkFlow (IDFac,IDIndicator,L1,L2,nUpdateID,dUpdate,cDel) VALUES ('" + item.nFacID + @"','" + item.nIndicatorID + @"','" + item.sManagerID + @"','" + item.sEnvironID + @"','" + nUserID + @"','" + now + @"','N') ";
                }
                SystemFunction.ExecuteSQL(SystemFunction.strConnect, Del_SQL);
                SystemFunction.ExecuteSQL(SystemFunction.strConnect, Insert_SQL);
                result.Status = SystemFunction.process_Success;
            }
        }


        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData AddOperation(List<string> lst_Operation, List<string> lst_Facility, List<string> lst_GroupIndicator, string sManagerID, string sEnvironID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        TRetunrLoadData result = new TRetunrLoadData();
        List<TDataFacAndIndicator> lstData = new List<TDataFacAndIndicator>();
        List<TDataFacAndIndicator> lstDataToSave = new List<TDataFacAndIndicator>();
        List<TDataDupicate> lstDataDuplicate = new List<TDataDupicate>();
        if (!UserAcc.UserExpired())
        {
            Func<int?, int?, bool> CheckDuplicateID = (IDFac, IDIndicator) =>
            {
                bool Isdup = false;
                var q = db.mTWorkFlow.Where(w => w.IDFac == IDFac && w.IDIndicator == IDIndicator && w.cDel == "N");
                Isdup = q.Any();
                return Isdup;
            };

            if (lst_Facility != null)
            {
                int nFacID = 0;
                int nIndicatorID = 0;
                foreach (var Fac in lst_Facility)
                {
                    nFacID = int.Parse(Fac);
                    var lstFac = db.mTFacility.Where(w => w.ID == nFacID && w.cDel == "N" && w.cActive == "Y").ToList();
                    if (lst_GroupIndicator != null)
                    {
                        foreach (var Indicator in lst_GroupIndicator)
                        {
                            nIndicatorID = int.Parse(Indicator);
                            var lstIndicator = db.mTIndicator.Where(w => w.ID == nIndicatorID).ToList();
                            lstData.Add(new TDataFacAndIndicator
                            {
                                nFacID = nFacID,
                                sFacName = lstFac.Any() ? lstFac.First(a => a.ID == nFacID).Name : "",
                                nIndicatorID = nIndicatorID,
                                sIndicatorName = lstIndicator.Any() ? lstIndicator.First(a => a.ID == nIndicatorID).Indicator : "",
                            });
                        }
                        lstData.Distinct().ToList();
                    }
                }
                string _SQL = "";
                if (lstData.Any())
                {
                    DateTime now = DateTime.Now;
                    int nUserID = UserAcc.GetObjUser().nUserID;
                    bool Ispass = true;
                    foreach (var item in lstData)
                    {
                        if (!CheckDuplicateID(item.nFacID, item.nIndicatorID))
                        {
                            _SQL += @" INSERT INTO mTWorkFlow (IDFac,IDIndicator,L1,L2,nUpdateID,dUpdate,cDel) VALUES ('" + item.nFacID + @"','" + item.nIndicatorID + @"','" + sManagerID + @"','" + sEnvironID + @"','" + nUserID + @"','" + now + @"','N') ";
                            //result.Status = SystemFunction.process_Success;
                            lstDataToSave.Add(new TDataFacAndIndicator
                            {
                                nFacID = item.nFacID,
                                nIndicatorID = item.nIndicatorID,
                                sManagerID = sManagerID,
                                sEnvironID = sEnvironID,
                                sFacName = item.sFacName,
                                sIndicatorName = item.sIndicatorName,
                            });
                        }
                        else
                        {

                            //result.Msg += " (Facility : " + item.sFacName + ") And (Group Indicator : " + item.sIndicatorName + ") ! </br>";
                            result.Status = SystemFunction.process_Failed;
                            Ispass = false;
                            lstDataDuplicate.Add(new TDataDupicate
                            {
                                nFacID = item.nFacID,
                                nIndicatorID = item.nIndicatorID,
                                sManagerID = sManagerID,
                                sEnvironID = sEnvironID,
                                sFacName = item.sFacName,
                                sIndicatorName = item.sIndicatorName,
                            });
                            //return result;
                        }
                    }
                    if (Ispass)
                    {
                        SystemFunction.ExecuteSQL(SystemFunction.strConnect, _SQL);
                        result.Status = SystemFunction.process_Success;
                    }
                }
            }
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        if (lstDataDuplicate.Any())
        {
            var lstFacility = lstDataDuplicate.Select(s => s.nFacID).Distinct().ToList();
            if (lstFacility.Any())
            {
                result.Msg = " Facilities has been saved. Do you want to save again ?</br></br>";
                foreach (var i in lstFacility)
                {
                    int nFacID = i;
                    var lst = lstDataDuplicate.Where(w => w.nFacID == nFacID).ToList();
                    if (lst.Any())
                    {
                        result.Msg += " Facility : " + lst.First().sFacName + " </br>";
                        lst.ForEach(f =>
                        {
                            result.Msg += " - Group Indicator : " + f.sIndicatorName + " </br>";
                        });
                        result.Msg += "</br>";
                    }
                }
            }

        }
        result.lstToSave = lstDataToSave.Distinct().ToList();
        result.lstDuplicate = lstDataDuplicate.Distinct().ToList();
        result.lstMapping = lstData.Distinct().ToList();
        return result;
    }

    #region ByDropdown
    private void GETDDL()
    {
        int nRoleID = UserAcc.GetObjUser().nRoleID;
        int nUserID = UserAcc.GetObjUser().nUserID;
        DDL_Role_Manager();
        DDL_Role_Environ();
        DDL_OperationType(nRoleID, nUserID);
        DDL_GroupIndicator();
        // DDL_Facility();
    }
    private void DDL_Role_Manager()
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        //var lstManager = db.mTUser_FacilityPermission.Where(w => w.nRoleID == 3).Select(s => new { nUserID = s.nUserID }).Distinct().ToList();
        var lstManager = db.mTUserInRole.Where(w => w.nRoleID == 3).Select(s => new { nUserID = s.nUID }).Distinct().ToList();
        List<DropDownList> lstDropDown = new List<DropDownList>();
        if (lstManager.Any())
        {
            lstManager.ForEach(f =>
            {
                db.mTUser.Where(w => w.cDel == "N" && w.cActive == "Y" && w.ID == f.nUserID).ToList().ForEach(f2 =>
                {
                    lstDropDown.Add(new DropDownList()
                    {
                        Value = f2.ID,
                        Text = f2.Firstname + ' ' + f2.Lastname,
                    });
                });
            });
            if (lstDropDown.Any())
            {
                ddlManager.DataSource = lstDropDown;
                ddlManager.DataValueField = "Value";
                ddlManager.DataTextField = "Text";
                ddlManager.DataBind();
            }
        }
        ddlManager.Items.Insert(0, new ListItem("- Manager -", ""));
    }
    private void DDL_Role_Environ()
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<DropDownList> lstDropDown = new List<DropDownList>();
        //var lstEnviron = db.mTUser_FacilityPermission.Where(w => w.nRoleID == 4).Select(s => new { nUserID = s.nUserID }).Distinct().ToList();
        var lstEnviron = db.mTUserInRole.Where(w => w.nRoleID == 4).Select(s => new { nUserID = s.nUID }).Distinct().ToList();
        if (lstEnviron.Any())
        {
            lstEnviron.ForEach(f =>
            {
                db.mTUser.Where(w => w.cDel == "N" && w.cActive == "Y" && w.ID == f.nUserID).ToList().ForEach(f2 =>
                {
                    lstDropDown.Add(new DropDownList()
                    {
                        Value = f2.ID,
                        Text = f2.Firstname + ' ' + f2.Lastname,
                    });
                });
                if (lstDropDown.Any())
                {
                    ddlEnviron.DataSource = lstDropDown;
                    ddlEnviron.DataValueField = "Value";
                    ddlEnviron.DataTextField = "Text";
                    ddlEnviron.DataBind();
                }
            });
        }
        ddlEnviron.Items.Insert(0, new ListItem("- Environment -", ""));
    }
    private void DDL_OperationType(int nRoleID, int nUserID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstOperationType = db.mOperationType.Where(w => w.cDel == "N" && w.cActive == "Y" && w.cManage == "N").Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        if (lstOperationType.Count() > 0)
        {
            ddlOperationType.DataSource = lstOperationType;
            ddlOperationType.DataValueField = "Value";
            ddlOperationType.DataTextField = "Text";
            ddlOperationType.DataBind();
            //ddlOperationType.Items.Insert(0, new ListItem("- Operation Type -", ""));
        }
        var lstOperationType_Search = db.mOperationType.Where(w => w.cDel == "N" && w.cActive == "Y" && w.cManage == "N").Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        //if (nRoleID == 6)
        //{
        //    var db1 = (from a in db.mTUser_FacilityPermission select a).ToList();
        //    var db2 = (from a in db.mTFacility select a).ToList();
        //    var db3 = (from a in db.mOperationType select a).ToList();
        //    //var lstOperationType_1 = db.mOperationType.Where(w => w.cDel == "N" && w.cActive == "Y" && w.cManage == "N").Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        //    lstOperationType_Search = (from a in db1
        //                               join b in db2 on a.nFacilityID equals b.ID
        //                               join c in db3 on b.OperationTypeID equals c.ID
        //                               where a.nUserID == nUserID
        //                               select new { Value = c.ID, Text = c.Name }).Distinct().ToList();
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
    private void DDL_GroupIndicator()
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
        var lstGroup_Search = db.mTIndicator.OrderBy(o => o.nOrder).Select(s => new { Value = s.ID, Text = s.Indicator }).ToList();
        if (lstGroup_Search.Count() > 0)
        {
            ddlIndicatorSearch.DataSource = lstGroup_Search;
            ddlIndicatorSearch.DataValueField = "Value";
            ddlIndicatorSearch.DataTextField = "Text";
            ddlIndicatorSearch.DataBind();
            ddlIndicatorSearch.Items.Insert(0, new ListItem("- Indicator -", ""));
        }
    }
    private void DDL_Facility()
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstFacility = db.mTFacility.Where(w => w.cDel == "N" && w.cActive == "Y" && w.nLevel == 2).OrderBy(o => o.Name).Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        if (lstFacility.Count() > 0)
        {
            ddlFacilitySearch.DataSource = lstFacility;
            ddlFacilitySearch.DataValueField = "Value";
            ddlFacilitySearch.DataTextField = "Text";
            ddlFacilitySearch.DataBind();
            ddlFacilitySearch.Items.Insert(0, new ListItem("- Sub facility -", ""));
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData Get_Facility(List<int> lst)
    {
        //if (lst == null) lst = new List<string>();
        TRetunrLoadData result = new TRetunrLoadData();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int nRoleID = UserAcc.GetObjUser().nRoleID;
        int nUserID = UserAcc.GetObjUser().nUserID;
        if (UserAcc.UserExpired())
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            List<sysGlobalClass.T_Facility> lstFacility = new List<sysGlobalClass.T_Facility>();
            if (lst.Any())
            {
                lstFacility = SystemFunction.Get_SubFacility_ByMuti(lst, nUserID, nRoleID);
            }
            result.lstData_Facility = lstFacility.Distinct().ToList();
        }
        return result;
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData Get_Facility_Seach(string operationID)
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
    #endregion

    #region class
    [Serializable]
    public class CSearch : sysGlobalClass.CommonLoadData
    {
        public string sSearch { get; set; }
        public string sStatus { get; set; }
        public string sPrms { get; set; }

        public string sOperationID { get; set; }
        public string sIndicatorID { get; set; }
        public string sFacilityID { get; set; }
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
    public class TRetunrLoadData : sysGlobalClass.Pagination
    {
        public string Msg { get; set; }
        public string Status { get; set; }
        public List<TDataDupicate> lstDuplicate { get; set; }
        public List<TDataFacAndIndicator> lstMapping { get; set; }
        public List<TDataFacAndIndicator> lstToSave { get; set; }
        public List<TDataTable> lstData { get; set; }
        public List<sysGlobalClass.T_Facility> lstData_Facility { get; set; }
    }

    [Serializable]
    public class TDataTable
    {
        public int IDOperation { get; set; }
        public int IDFac { get; set; }
        public int IDIndicator { get; set; }
        public int? L1 { get; set; }
        public int? L2 { get; set; }
        public int? L3 { get; set; }
        public int? L4 { get; set; }
        public int? nUpdateID { get; set; }
        public string sManagerName { get; set; }
        public string sEnvironName { get; set; }

        public DateTime? dUpdate { get; set; }
        public string cDel { get; set; }
        public string sGroupName { get; set; }
        public string sFacilityName { get; set; }
        public string sOperationName { get; set; }
        public string sUpdate { get; set; }
        public string sStatus { get; set; }
        public string sLink { get; set; }
    }

    [Serializable]
    public class TDataFacAndIndicator
    {
        public int nFacID { get; set; }
        public string sFacName { get; set; }
        public int nIndicatorID { get; set; }
        public string sIndicatorName { get; set; }
        public string sManagerID { get; set; }
        public string sEnvironID { get; set; }
    }
    [Serializable]
    public class TDataDupicate
    {
        public int nFacID { get; set; }
        public string sFacName { get; set; }
        public int nIndicatorID { get; set; }
        public string sIndicatorName { get; set; }
        public string sManagerID { get; set; }
        public string sEnvironID { get; set; }
        public int nUserID { get; set; }
        public DateTime? dDate { get; set; }
    }
    [Serializable]
    public class DropDownList
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
    #endregion
}