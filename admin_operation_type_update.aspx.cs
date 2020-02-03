using sysExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_operation_type_update : System.Web.UI.Page
{
    PTTGC_EPIEntities db = new PTTGC_EPIEntities();
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
                string OperationID = Request.QueryString["strid"];
                if (!string.IsNullOrEmpty(OperationID))
                {
                    int nID = int.Parse(STCrypt.Decrypt(OperationID));
                    hdfEncryptOperationID.Value = OperationID;
                    hdfOperationID.Value = STCrypt.Decrypt(hdfEncryptOperationID.Value);
                    SETDATA(nID);
                }
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData LoadData_Indicator(CSearch itemSearch)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        TRetunrLoadData result = new TRetunrLoadData();
        List<TDataIndicator> lstData = new List<TDataIndicator>();
        if (!UserAcc.UserExpired())
        {
            lstData = db.mTIndicator.Select(s => new TDataIndicator
            {
                nID = s.ID,
                sIndicator = s.Indicator,
                nOrder = s.nOrder,
                sPermission = "1",
            }).OrderBy(o => o.nOrder).ToList();

            //#region//SORT
            //int? nSortCol = SystemFunction.GetIntNull(itemSearch.sIndexCol);
            //switch ((itemSearch.sOrderBy + "").ToLower())
            //{
            //    case SystemFunction.ASC:
            //        {
            //            switch (nSortCol)
            //            {
            //                case 1: lstData = lstData.OrderBy(o => o.sCompName).ToList(); break;
            //                case 2: lstData = lstData.OrderBy(o => o.sStatus).ToList(); break;
            //                case 3: lstData = lstData.OrderBy(o => o.dUpdate).ToList(); break;
            //                case 4: lstData = lstData.OrderBy(o => o.nCountFacility).ToList(); break;
            //            }
            //        }
            //        break;
            //    case SystemFunction.DESC:
            //        {
            //            switch (nSortCol)
            //            {
            //                case 1: lstData = lstData.OrderByDescending(o => o.sCompName).ToList(); break;
            //                case 2: lstData = lstData.OrderByDescending(o => o.sStatus).ToList(); break;
            //                case 3: lstData = lstData.OrderByDescending(o => o.dUpdate).ToList(); break;
            //                case 4: lstData = lstData.OrderByDescending(o => o.nCountFacility).ToList(); break;
            //            }
            //        }
            //        break;
            //}
            //#endregion

            #region//Final Action >> Skip Take Data For Javasacript
            sysGlobalClass.Pagination dataPage = new sysGlobalClass.Pagination();
            dataPage = SystemFunction.GetPaginationSmall(SystemFunction.GetIntNullToZero(itemSearch.sPageSize), SystemFunction.GetIntNullToZero(itemSearch.sPageIndex), lstData.Count);
            lstData = lstData.Skip(dataPage.nSkipData).Take(dataPage.nTakeData).ToList();

            //foreach (var item in lstData)
            //{
            //    item.sUpdate = item.dUpdate.DateString();
            //    item.sLink = "<a class='btn btn-warning' href='admin_company_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nCompID.ToString())) + "'><i class='fa fa-edit'></i>&nbsp;Edit</a>";
            //}

            result.lstDataIndicator = lstData;
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
    public static TRetunrLoadData LoadData_Facility(CSearch itemSearch, string sID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        TRetunrLoadData result = new TRetunrLoadData();
        List<TDataFacility> lstData = new List<TDataFacility>();
        if (!UserAcc.UserExpired())
        {
            string sOperationID = sID.STCDecrypt();
            if (!string.IsNullOrEmpty(sOperationID))
            {
                int nOperationID = sOperationID.toIntNullToZero();
                var lst_Facility = db.mTFacility_Operationtype.Where(w => w.nOperationtypeID == nOperationID).ToList();
                if (lst_Facility != null)
                {
                    lst_Facility.ForEach(f =>
                    {
                        var lst = db.mTFacility.Where(w => w.ID == f.nFacID).Select(s => new TDataFacility
                        {
                            nID = s.ID,
                            sName = s.Name,
                        }).ToList();
                        lstData.AddRange(lst);
                    });
                }
            }



            //#region//SORT
            //int? nSortCol = SystemFunction.GetIntNull(itemSearch.sIndexCol);
            //switch ((itemSearch.sOrderBy + "").ToLower())
            //{
            //    case SystemFunction.ASC:
            //        {
            //            switch (nSortCol)
            //            {
            //                case 1: lstData = lstData.OrderBy(o => o.sCompName).ToList(); break;
            //                case 2: lstData = lstData.OrderBy(o => o.sStatus).ToList(); break;
            //                case 3: lstData = lstData.OrderBy(o => o.dUpdate).ToList(); break;
            //                case 4: lstData = lstData.OrderBy(o => o.nCountFacility).ToList(); break;
            //            }
            //        }
            //        break;
            //    case SystemFunction.DESC:
            //        {
            //            switch (nSortCol)
            //            {
            //                case 1: lstData = lstData.OrderByDescending(o => o.sCompName).ToList(); break;
            //                case 2: lstData = lstData.OrderByDescending(o => o.sStatus).ToList(); break;
            //                case 3: lstData = lstData.OrderByDescending(o => o.dUpdate).ToList(); break;
            //                case 4: lstData = lstData.OrderByDescending(o => o.nCountFacility).ToList(); break;
            //            }
            //        }
            //        break;
            //}
            //#endregion

            #region//Final Action >> Skip Take Data For Javasacript
            sysGlobalClass.Pagination dataPage = new sysGlobalClass.Pagination();
            dataPage = SystemFunction.GetPaginationSmall(SystemFunction.GetIntNullToZero(itemSearch.sPageSize), SystemFunction.GetIntNullToZero(itemSearch.sPageIndex), lstData.Count);
            lstData = lstData.Skip(dataPage.nSkipData).Take(dataPage.nTakeData).ToList();

            //foreach (var item in lstData)
            //{
            //    item.sUpdate = item.dUpdate.DateString();
            //    item.sLink = "<a class='btn btn-warning' href='admin_company_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nCompID.ToString())) + "'><i class='fa fa-edit'></i>&nbsp;Edit</a>";
            //}

            result.lstDataFacility = lstData;
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
    public static sysGlobalClass.CResutlWebMethod SaveData(DataSave itemSave)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        if (UserAcc.UserExpired())
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            Func<string, int?, bool> CheckDuplicateName = (name, id) =>
            {
                bool Isdup = false;
                var q = db.mOperationType.Where(w => (id.HasValue ? w.ID != id : true) && w.Name == name);
                Isdup = q.Any();
                return Isdup;
            };

            string sOperationID = itemSave.sID.STCDecrypt();
            if (!string.IsNullOrEmpty(sOperationID))
            {
                int nOperationID = sOperationID.toIntNullToZero();
                if (!CheckDuplicateName(itemSave.sOperationName, nOperationID))
                {
                    var query = db.mOperationType.FirstOrDefault(f => f.ID == nOperationID);
                    if (query != null)
                    {
                        query.Name = itemSave.sOperationName.Trims();
                        query.Description = itemSave.sDesc;
                        query.cActive = itemSave.sStatus;
                        query.nUpdateID = UserAcc.GetObjUser().nUserID;
                        query.dUpdate = DateTime.Now;
                        if (itemSave.sStatus == "N")
                        {
                            query.sRemark = itemSave.sRemark.Trims();
                        }
                        else
                        {
                            query.sRemark = "";
                        }
                        db.SaveChanges();
                        result.Status = SystemFunction.process_Success;
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Not found data !";
                    }
                }
                else
                {
                    result.Msg = "Duplicate Operation name !";
                    result.Status = SystemFunction.process_Failed;
                }
            }
            else
            {
                if (!CheckDuplicateName(itemSave.sOperationName, null))
                {
                    int nID = db.mOperationType.Any() ? db.mOperationType.Max(m => m.ID) + 1 : 1;
                    mOperationType t = new mOperationType();
                    t.ID = nID;
                    t.Name = itemSave.sOperationName.Trims();
                    t.Description = itemSave.sDesc;
                    t.cActive = itemSave.sStatus;
                    if (itemSave.sStatus == "N")
                    {
                        t.sRemark = itemSave.sRemark.Trims();
                    }
                    t.nCreateID = UserAcc.GetObjUser().nUserID;
                    t.dCreate = DateTime.Now;
                    t.nUpdateID = UserAcc.GetObjUser().nUserID;
                    t.dUpdate = DateTime.Now;
                    t.cDel = "N";
                    t.cManage = "Y";
                    db.mOperationType.Add(t);
                    db.SaveChanges();
                    result.Status = SystemFunction.process_Success;
                }
                else
                {
                    result.Msg = "Duplicate Operation name !";
                    result.Status = SystemFunction.process_Failed;
                }
            }

        }

        return result;
    }
    private void SETDATA(int nID)
    {
        if (!string.IsNullOrEmpty(nID + ""))
        {
            var Query = db.mOperationType.Where(w => w.cDel == "N" && w.ID == nID).ToList();
            if (Query != null && Query.Count > 0)
            {
                txtOperationName.Text = Query.Any() ? Query.First().Name : "";
                txtDesc.Text = Query.Any() ? Query.First().Description : "";
                txtRemark.Text = Query.Any() ? Query.First().sRemark : "";
                rblStatus.SelectedValue = Query.First().cActive;
                if (rblStatus.SelectedValue == "N")
                {
                    txtRemark.Text = Query.First().sRemark;
                    txtRemark.Enabled = true;
                }
            }
        }
    }


    #region class
    [Serializable]
    public class CSearch : sysGlobalClass.CommonLoadData
    {
        public string sSearch { get; set; }
        public string sStatus { get; set; }
        public string sPrms { get; set; }
    }

    [Serializable]
    public class TRetunrLoadData : sysGlobalClass.Pagination
    {
        public List<TDataIndicator> lstDataIndicator { get; set; }
        public List<TDataFacility> lstDataFacility { get; set; }
    }
    [Serializable]
    public class TDataIndicator
    {
        public int nID { get; set; }
        public string sIndicator { get; set; }
        public string sPermission { get; set; }
        public int? nOrder { get; set; }
    }

    [Serializable]
    public class TDataFacility
    {
        public int nID { get; set; }
        public string sName { get; set; }
    }
    [Serializable]
    public class DataSave
    {
        public string sID { get; set; }
        public string sOperationName { get; set; }
        public string sDesc { get; set; }
        public string sRemark { get; set; }
        public string sStatus { get; set; }
    }
    #endregion
}