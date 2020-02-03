using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using sysExtension;

public partial class admin_asset_lst : System.Web.UI.Page
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
                PTTGC_EPIEntities db = new PTTGC_EPIEntities();
                string str = Request.QueryString["strid"];
                if (!string.IsNullOrEmpty(str))
                {
                    int nFacID = SystemFunction.GetIntNullToZero(STCrypt.Decrypt(str));
                    hdfFacID.Value = str;
                    ltrCreate.Text = "<a class=\"btn btn-primary btn-sm btn-block\" href=\"admin_asset_update.aspx?strid=" + HttpUtility.UrlEncode(str) + "\"><i class=\"fa fa-plus\"></i>&nbsp;Create Sub-facility</a>";
                    var itemHeader = db.mTFacility.FirstOrDefault(w => w.ID == nFacID);
                    if (itemHeader != null)
                    {
                        var itemCompany = db.mTCompany.FirstOrDefault(w => w.ID == itemHeader.CompanyID);
                        ltrHeader.Text = "<a href='admin_company_lst.aspx' style='color:white'>Organization</a> >  <a style='color:white' href='admin_facility_lst.aspx?strid=" + HttpUtility.UrlEncode(STCrypt.Encrypt(itemCompany.ID + "")) + "'> " + itemCompany.Name + "</a> > " + itemHeader.Name;//กำหนด Header
                    }
                }
                SystemFunction.BindDropdownPageSize(ddlPageSize, null);
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
            int nFacID = SystemFunction.GetIntNullToZero(STCrypt.Decrypt(itemSearch.sFacID));
            lstData = db.mTFacility.Where(w => w.cDel == "N" && w.nLevel == 2 && w.nHeaderID == nFacID && (!string.IsNullOrEmpty(sSearch) ? w.Name.ToLower().Contains(sSearch) : true) && (!string.IsNullOrEmpty(sStatus) ? w.cActive.ToLower().Contains(sStatus) : true)).Select(s => new TDataTable
            {
                nFacID = s.nHeaderID,
                nAssetID = s.ID,
                sAssetName = s.Name,
                sStatus = s.cActive == "Y" ? "Active" : "Inactive",
                dUpdate = s.dUpdate
            }).OrderByDescending(o => o.dUpdate).ToList();
            #region//SORT
            int? nSortCol = SystemFunction.GetIntNull(itemSearch.sIndexCol);
            switch ((itemSearch.sOrderBy + "").ToLower())
            {
                case SystemFunction.ASC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderBy(o => o.sAssetName).ToList(); break;
                            case 2: lstData = lstData.OrderBy(o => o.sStatus).ToList(); break;
                            case 3: lstData = lstData.OrderBy(o => o.dUpdate).ToList(); break;
                        }
                    }
                    break;
                case SystemFunction.DESC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderByDescending(o => o.sAssetName).ToList(); break;
                            case 2: lstData = lstData.OrderByDescending(o => o.sStatus).ToList(); break;
                            case 3: lstData = lstData.OrderByDescending(o => o.dUpdate).ToList(); break;
                        }
                    }
                    break;
            }
            #endregion

            #region//Final Action >> Skip Take Data For Javasacript
            sysGlobalClass.Pagination dataPage = new sysGlobalClass.Pagination();
            dataPage = SystemFunction.GetPaginationSmall(SystemFunction.GetIntNullToZero(itemSearch.sPageSize), SystemFunction.GetIntNullToZero(itemSearch.sPageIndex), lstData.Count);
            lstData = lstData.Skip(dataPage.nSkipData).Take(dataPage.nTakeData).ToList();
            var lstDataFac = db.mTFacility.Where(w => w.cDel == "N").ToList();
            foreach (var item in lstData)
            {
                item.sUpdate = item.dUpdate.DateString();
                item.sLink = "<a class='btn btn-warning' href='admin_asset_update.aspx?strAssetID=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nAssetID.ToString())) + "&&strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nFacID.ToString())) + "'><i class='fa fa-edit'></i>&nbsp;Edit</a>";

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

    #region class
    [Serializable]
    public class CSearch : sysGlobalClass.CommonLoadData
    {
        public string sSearch { get; set; }
        public string sStatus { get; set; }
        public string sFacID { get; set; }
        public string sPrms { get; set; }
    }

    [Serializable]
    public class TRetunrLoadData : sysGlobalClass.Pagination
    {
        public List<TDataTable> lstData { get; set; }
    }

    [Serializable]
    public class TDataTable
    {
        public int? nFacID { get; set; }
        public int nAssetID { get; set; }
        public string sAssetName { get; set; }
        public string sStatus { get; set; }
        public DateTime? dUpdate { get; set; }
        public string sUpdate { get; set; }
        public string sLink { get; set; }
    }
    public class DataValue
    {
        public string sComID { get; set; }
        public string sFacilityID { get; set; }
        public string sFacilityName { get; set; }
        public string sFacilityPTT_ID { get; set; }
        public string sOperationTypePTT_ID { get; set; }
        public List<string> lstOperationTypeGC_ID { get; set; }
        public string sDescription { get; set; }
        public string sActive { get; set; }
        public string sRemark { get; set; }
        public string sComType { get; set; }
        public bool IsNew { get; set; }
    }
    #endregion
}