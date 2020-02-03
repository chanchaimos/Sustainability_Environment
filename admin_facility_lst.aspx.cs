using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using sysExtension;

public partial class admin_facility_lst : System.Web.UI.Page
{
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnLoad(myFunc);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SystemFunction.BindDropdownPageSize(ddlPageSize, null);
            SystemFunction.BindDropdownPageSize(ddlPageSizeGC, null);
            if (!UserAcc.UserExpired())
            {
                string str = Request.QueryString["strid"];
                if (!string.IsNullOrEmpty(str))
                {
                    if (SystemFunction.GetIntNullToZero(STCrypt.Decrypt(str)) != 1)
                    {
                        SetBodyEventOnLoad("$('div[id$=divContentGC]').show();$('div[id$=divContent]').hide()");
                    }
                    else
                    {
                        SetBodyEventOnLoad("$('div[id$=divContentGC]').hide();$('div[id$=divContent]').show()");
                    }
                    PTTGC_EPIEntities db = new PTTGC_EPIEntities();
                    int nComID = SystemFunction.GetIntNullToZero(STCrypt.Decrypt(str));
                    hdfComID.Value = STCrypt.Encrypt(nComID + "");
                    var itemCompany = db.mTCompany.FirstOrDefault(w => w.ID == nComID && w.cDel == "N");
                    if (itemCompany != null)
                    {
                        ltrHeader.Text = "<a href='admin_company_lst.aspx' style='color:white'>Organization</a> > " + itemCompany.Name;//กำหนด Header
                    }
                    ltrCreateGC.Text = "<a class=\"btn btn-primary btn-sm btn-block\" href=\"admin_facility_update.aspx?strid=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nComID + "")) + "\"><i class=\"fa fa-plus\"></i>&nbsp;Create Facility</a>";
                    ltrCreate.Text = "<a class=\"btn btn-primary btn-sm btn-block\" href=\"admin_facility_update.aspx?strid=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nComID + "")) + "\"><i class=\"fa fa-plus\"></i>&nbsp;Create Facility</a>";
                }
                else
                {
                    SetBodyEventOnLoad(SystemFunction.DialogWarningRedirect(SystemFunction.Msg_HeadWarning, "Invalid Data", "admin_company_lst.aspx"));// กรณีเข้ามาด้วย link ที่ไม่มี Querystring
                }
            }
            else
            {
                SetBodyEventOnLoad(SystemFunction.PopupLogin());
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
            string sSearch = itemSearch.sSearch.Trims().ToLower();
            string sStatus = itemSearch.sStatus.Trims().ToLower();
            int nComID = SystemFunction.GetIntNullToZero(STCrypt.Decrypt(itemSearch.sComID));
            lstData = db.mTFacility.Where(w => w.cDel == "N" && w.nLevel == 0 && w.CompanyID == nComID && (!string.IsNullOrEmpty(sSearch) ? w.Name.ToLower().Contains(sSearch) : true) && (!string.IsNullOrEmpty(sStatus) ? w.cActive.ToLower().Contains(sStatus) : true)).Select(s => new TDataTable
            {
                nCompanyID = s.CompanyID,
                nFacilityID = s.ID,
                sFacilityName = s.Name,
                sStatus = s.cActive == "Y" ? "Active" : "Inactive",
                dUpdate = s.dUpdate,
            }).OrderByDescending(o => o.dUpdate).ToList();

            #region//SORT
            int? nSortCol = SystemFunction.GetIntNull(itemSearch.sIndexCol);
            switch ((itemSearch.sOrderBy + "").ToLower())
            {
                case SystemFunction.ASC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderBy(o => o.sFacilityName).ToList(); break;
                            case 2: lstData = lstData.OrderBy(o => o.sStatus).ToList(); break;
                            case 3: lstData = lstData.OrderBy(o => o.dUpdate).ToList(); break;
                            case 4: lstData = lstData.OrderBy(o => o.dUpdate).ToList(); break;
                        }
                    }
                    break;
                case SystemFunction.DESC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderByDescending(o => o.sFacilityName).ToList(); break;
                            case 2: lstData = lstData.OrderByDescending(o => o.sStatus).ToList(); break;
                            case 3: lstData = lstData.OrderByDescending(o => o.dUpdate).ToList(); break;
                            case 4: lstData = lstData.OrderByDescending(o => o.dUpdate).ToList(); break;
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
                item.sUpdate = item.dUpdate.DateString();
                item.sLink = "<a class='btn btn-warning' href='admin_facility_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nCompanyID.ToString())) + "&&strFacID=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nFacilityID.ToString())) + "'><i class='fa fa-edit'></i>&nbsp;Edit</a>";
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
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static TRetunrLoadData LoadDataGC(CSearch itemSearch)
    {
        TRetunrLoadData result = new TRetunrLoadData();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            List<TDataTable> lstData = new List<TDataTable>();
            string sSearch = itemSearch.sSearch.Trims().ToLower();
            string sStatus = itemSearch.sStatus.Trims().ToLower();
            int nComID = SystemFunction.GetIntNullToZero(STCrypt.Decrypt(itemSearch.sComID));
            lstData = db.mTFacility.Where(w => w.cDel == "N" && w.nLevel == 1 && w.CompanyID == nComID && (!string.IsNullOrEmpty(sSearch) ? w.Name.ToLower().Contains(sSearch) : true) && (!string.IsNullOrEmpty(sStatus) ? w.cActive.ToLower().Contains(sStatus) : true)).Select(s => new TDataTable
            {
                nCompanyID = s.CompanyID,
                nFacilityID = s.ID,
                sFacilityName = s.Name,
                sStatus = s.cActive == "Y" ? "Active" : "Inactive",
                dUpdate = s.dUpdate,
            }).OrderByDescending(o => o.dUpdate).ToList();

            var lstDataFac = db.mTFacility.Where(w => w.cDel == "N" && w.nLevel == 2).ToList();
            foreach (var item in lstData)
            {
                string sLinkAsset = "";
                item.nCountAsset = lstDataFac.Where(w => w.CompanyID == item.nCompanyID && w.nHeaderID == item.nFacilityID && w.cDel == "N").Count();
                sLinkAsset = "<div class='btn-group'>" +
                                "<a class='btn btn-sm btn-primary' href='admin_asset_lst.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nFacilityID.ToString())) + "'><span class='badge'>" + lstDataFac.Where(w => w.CompanyID == item.nCompanyID && w.nHeaderID == item.nFacilityID && w.cDel == "N").Count() + "</span></a>" +
                                "<a class='btn btn-sm btn-info' href='admin_asset_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nFacilityID.ToString())) + "'><i class='fa fa-plus'></i></a>" +
                                "</div>";
                item.sUpdate = item.dUpdate.DateString();
                item.sLink = "<a class='btn btn-warning' href='admin_facility_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nCompanyID.ToString())) + "&&strFacID=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nFacilityID.ToString())) + "'><i class='fa fa-edit'></i>&nbsp;Edit</a>";
                //item.sLinkAsset = "<a class='btn btn-info' href='admin_asset_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nFacilityID.ToString())) + "'><i class='fa fa-search'></i></a>";
                item.sLinkAsset = sLinkAsset;
            }

            #region//SORT
            int? nSortCol = SystemFunction.GetIntNull(itemSearch.sIndexCol);
            switch ((itemSearch.sOrderBy + "").ToLower())
            {
                case SystemFunction.ASC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderBy(o => o.sFacilityName).ToList(); break;
                            case 2: lstData = lstData.OrderBy(o => o.sStatus).ToList(); break;
                            case 3: lstData = lstData.OrderBy(o => o.dUpdate).ToList(); break;
                            case 4: lstData = lstData.OrderBy(o => o.nCountAsset).ToList(); break;
                        }
                    }
                    break;
                case SystemFunction.DESC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderByDescending(o => o.sFacilityName).ToList(); break;
                            case 2: lstData = lstData.OrderByDescending(o => o.sStatus).ToList(); break;
                            case 3: lstData = lstData.OrderByDescending(o => o.dUpdate).ToList(); break;
                            case 4: lstData = lstData.OrderByDescending(o => o.nCountAsset).ToList(); break;
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
            db.mTFacility.Where(w => lstDelID.Contains(w.ID)).ToList().ForEach(x =>
            {
                x.cDel = "Y";
                x.dUpdate = DateTime.Now;
                x.UpdateID = nUserID;
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
    public static sysGlobalClass.CResutlWebMethod DeleteDataGC(string[] arrValue)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            List<int> lstDelID = new List<int>();
            if (arrValue != null)
                lstDelID = arrValue.Select(s => s.toIntNullToZero()).ToList();
            int nUserID = UserAcc.GetObjUser().nUserID;
            db.mTFacility.Where(w => lstDelID.Contains(w.ID)).ToList().ForEach(x =>
            {
                db.mTFacility_Operationtype.RemoveRange(db.mTFacility_Operationtype.Where(w => w.nFacID == x.ID));
                x.cDel = "Y";
                x.dUpdate = DateTime.Now;
                x.UpdateID = nUserID;
            });
            foreach (var item in lstDelID)
            {
                var qDelSubFac = db.mTFacility.Where(w => w.nLevel == 2 && w.nHeaderID == item);
                foreach (var itemS in qDelSubFac)
                {
                    itemS.cDel = "Y";
                    itemS.dUpdate = DateTime.Now;
                    itemS.UpdateID = nUserID;
                }
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

    #region class
    [Serializable]
    public class CSearch : sysGlobalClass.CommonLoadData
    {
        public string sSearch { get; set; }
        public string sStatus { get; set; }
        public string sPrms { get; set; }
        public string sComID { get; set; }
    }

    [Serializable]
    public class TRetunrLoadData : sysGlobalClass.Pagination
    {
        public List<TDataTable> lstData { get; set; }
    }

    [Serializable]
    public class TDataTable
    {
        public int nCompanyID { get; set; }
        public int nFacilityID { get; set; }
        public string sFacilityName { get; set; }
        public string sStatus { get; set; }
        public int nCountAsset { get; set; }
        public DateTime? dUpdate { get; set; }
        public string sUpdate { get; set; }
        public string sLink { get; set; }
        public string sLinkAsset { get; set; }
    }
    #endregion
}