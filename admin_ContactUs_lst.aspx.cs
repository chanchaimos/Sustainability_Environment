using sysExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_ContactUs_lst : System.Web.UI.Page
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
            if (!string.IsNullOrEmpty(sStatus))
            {
                sStatus = sStatus == "w" ? null : sStatus;
            }
            lstData = db.TContactUs.Where(w => w.cDel == "N" && (!string.IsNullOrEmpty(sSearch) ? w.sContactName.ToLower().Contains(sSearch) : true) && (sStatus != "" ? w.cStatusAns == sStatus : true)).Select(s => new TDataTable
            {
                nContactID = s.nContactID,
                sContactName = s.sContactName,
                cStatusAns = s.cStatusAns == null ? "Wait" : s.cStatusAns == "1" ? "Read" : "Success",
                cStatus = s.cStatusAns,
                dCreate = s.dCreate,
                dUpdate = s.dUpdate,
            }).OrderByDescending(o => o.dUpdate).ToList();
            //lstData = db.mTCompany.Where(w => w.cDel == "N" && (!string.IsNullOrEmpty(sSearch) ? w.Name.ToLower().Contains(sSearch) : true)).Select(s => new TDataTable
            //{
            //    nCompID = s.ID,
            //    sCompName = s.Name,
            //    sStatus = s.cActive == "Y" ? "Active" : "Inactive",
            //    dUpdate = s.dUpdate
            //}).OrderByDescending(o => o.dUpdate).ToList();

            #region//SORT
            int? nSortCol = SystemFunction.GetIntNull(itemSearch.sIndexCol);
            switch ((itemSearch.sOrderBy + "").ToLower())
            {
                case SystemFunction.ASC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderBy(o => o.sContactName).ToList(); break;
                            case 2: lstData = lstData.OrderBy(o => o.dCreate).ToList(); break;
                            case 3: lstData = lstData.OrderBy(o => o.cStatus).ToList(); break;
                                //case 4: lstData = lstData.OrderBy(o => o.nCountFacility).ToList(); break;
                        }
                    }
                    break;
                case SystemFunction.DESC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderByDescending(o => o.sContactName).ToList(); break;
                            case 2: lstData = lstData.OrderByDescending(o => o.dCreate).ToList(); break;
                            case 3: lstData = lstData.OrderByDescending(o => o.cStatus).ToList(); break;
                                //case 4: lstData = lstData.OrderByDescending(o => o.nCountFacility).ToList(); break;
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
                var color = item.cStatus == null ? "warning" : item.cStatus == "1" ? "primary" : "success";
                var Wordding = item.cStatus == null ? "Reply" : item.cStatus == "1" ? "Read" : "View";
                var icon = item.cStatus == null ? "edit" : item.cStatus == "1" ? "search" : "search";
                item.sUpdate = item.dUpdate.DateString();
                item.sCreate = item.dCreate.DateString();
                item.sLink = "<a class='btn btn-" + color + "' href='admin_ContactUs_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nContactID.ToString())) + "'><i class='fa fa-" + icon + "'></i>&nbsp;" + Wordding + "</a>";
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
        public List<TDataTable> lstData { get; set; }
    }

    [Serializable]
    public class TDataTable
    {
        public int nContactID { get; set; }
        public string sContactName { get; set; }
        public string sContactEmail { get; set; }
        public string sContactTel { get; set; }
        public string sSubject { get; set; }
        public string sDetail { get; set; }
        public string sContactFile { get; set; }
        public string sContactPath { get; set; }
        public DateTime? dUpdate { get; set; }
        public DateTime? dCreate { get; set; }
        public string sUpdate { get; set; }
        public string sCreate { get; set; }
        public string cStatusAns { get; set; }
        public string cStatus { get; set; }
        public string sLink { get; set; }
    }
    #endregion
}