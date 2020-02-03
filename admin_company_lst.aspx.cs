using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sysExtension;
using System.Web.Script.Services;
using System.Web.Services;

public partial class admin_company_lst : System.Web.UI.Page
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

            lstData = db.mTCompany.Where(w => w.cDel == "N" && (!string.IsNullOrEmpty(sSearch) ? w.Name.ToLower().Contains(sSearch) : true) && (!string.IsNullOrEmpty(sStatus) ? w.cActive.ToLower().Contains(sStatus) : true)).Select(s => new TDataTable
            {
                nCompID = s.ID,
                sCompName = s.Name,
                sStatus = s.cActive == "Y" ? "Active" : "Inactive",
                dUpdate = s.dUpdate,
                SearchBy = "",
            }).OrderByDescending(o => o.dUpdate).ToList();

            var lstDataFac = db.mTFacility.Where(w => w.cDel == "N").ToList();
            var lstCompanyID_SearchBySub = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 2 && (!string.IsNullOrEmpty(sSearch) ? w.Name.ToLower().Contains(sSearch) : false)).Select(s => new { s.CompanyID }).Distinct().ToList();
            var lstCompanyID_SearchBySubLve1 = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 1 && (!string.IsNullOrEmpty(sSearch) ? w.Name.ToLower().Contains(sSearch) : false)).Select(s => new { s.CompanyID }).Distinct().ToList();

            if (lstCompanyID_SearchBySub.Any())
            {
                List<int> lstComID = new List<int>();
                lstComID = lstCompanyID_SearchBySub.Select(s => s.CompanyID).ToList();
                var lstCompany = db.mTCompany.Where(w => w.cDel == "N" && lstComID.Contains(w.ID)).Select(s => s.ID).ToList();

                if (lstCompany.Any())
                {
                    foreach (var item in lstCompany)
                    {
                        ///// Check ว่ามีไอดีนี้ในบริษัทไหม ถ้าไม่มีค่อย แอด เพิ่ม
                        var q = lstData.FirstOrDefault(w => w.nCompID == item);
                        if (q == null)
                        {
                            List<TDataTable> lstDataSub2 = new List<TDataTable>();

                            lstDataSub2 = db.mTCompany.Where(w => w.cDel == "N" && w.ID == item).Select(s => new TDataTable
                            {
                                nCompID = s.ID,
                                sCompName = s.Name,
                                sStatus = s.cActive == "Y" ? "Active" : "Inactive",
                                dUpdate = s.dUpdate,
                                SearchBy = "Y",
                            }).OrderByDescending(o => o.dUpdate).ToList();
                            lstData.AddRange(lstDataSub2);
                        }
                    }
                }
            }

            if (lstCompanyID_SearchBySubLve1.Any())
            {
                List<int> lstComID = new List<int>();
                lstComID = lstCompanyID_SearchBySubLve1.Select(s => s.CompanyID).ToList();
                var lstCompany = db.mTCompany.Where(w => w.cDel == "N" && lstComID.Contains(w.ID)).Select(s => s.ID).ToList();

                if (lstCompany.Any())
                {
                    foreach (var item in lstCompany)
                    {
                        ///// Check ว่ามีไอดีนี้ในบริษัทไหม ถ้าไม่มีค่อย แอด เพิ่ม
                        var q = lstData.FirstOrDefault(w => w.nCompID == item);
                        if (q == null)
                        {
                            List<TDataTable> lstDataSub1 = new List<TDataTable>();

                            lstDataSub1 = db.mTCompany.Where(w => w.cDel == "N" && w.ID == item).Select(s => new TDataTable
                            {
                                nCompID = s.ID,
                                sCompName = s.Name,
                                sStatus = s.cActive == "Y" ? "Active" : "Inactive",
                                dUpdate = s.dUpdate,
                                SearchBy = "Y",
                            }).OrderByDescending(o => o.dUpdate).ToList();
                            lstData.AddRange(lstDataSub1);
                        }
                    }
                }
            }



            foreach (var item in lstData)
            {
                string sLinkFacility = "";
                item.nCountFacility = lstDataFac.Where(w => w.CompanyID == item.nCompID && w.cDel == "N" && (item.nCompID == 1 ? w.nLevel == 0 : w.nLevel == 1)).Count();
                sLinkFacility = "<div class='btn-group'>" +
                    "<a class='btn btn-sm btn-primary' href='admin_facility_lst.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nCompID.ToString())) + "'><span class='badge'>" + lstDataFac.Where(w => w.CompanyID == item.nCompID && w.cDel == "N" && (item.nCompID == 1 ? w.nLevel == 0 : w.nLevel == 1)).Count() + "</span></a>" +
                                "<a class='btn btn-sm btn-info' href='admin_facility_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nCompID.ToString())) + "'><i class='fa fa-plus'></i></a>" +
                                "</div>";
                item.sUpdate = item.dUpdate.DateString();
                item.sLink = "<a class='btn btn-warning' href='admin_company_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nCompID.ToString())) + "'><i class='fa fa-edit'></i>&nbsp;Edit</a>";
                //item.sLinkFacility = "<a class='btn btn-info' href='admin_facility_lst.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nCompID.ToString())) + "'><i class='fa fa-search'></i></a>";
                item.sLinkFacility = sLinkFacility;

            }

            #region//SORT
            int? nSortCol = SystemFunction.GetIntNull(itemSearch.sIndexCol);
            switch ((itemSearch.sOrderBy + "").ToLower())
            {
                case SystemFunction.ASC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderBy(o => o.sCompName).ToList(); break;
                            case 2: lstData = lstData.OrderBy(o => o.sStatus).ToList(); break;
                            case 3: lstData = lstData.OrderBy(o => o.dUpdate).ToList(); break;
                            case 4: lstData = lstData.OrderBy(o => o.nCountFacility).ToList(); break;
                        }
                    }
                    break;
                case SystemFunction.DESC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderByDescending(o => o.sCompName).ToList(); break;
                            case 2: lstData = lstData.OrderByDescending(o => o.sStatus).ToList(); break;
                            case 3: lstData = lstData.OrderByDescending(o => o.dUpdate).ToList(); break;
                            case 4: lstData = lstData.OrderByDescending(o => o.nCountFacility).ToList(); break;
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
            db.mTCompany.Where(w => w.ID != EPIFunc.DataType.Company.PTTGCID && lstDelID.Contains(w.ID)).ToList().ForEach(x =>
            {
                x.cDel = "Y";
                x.dUpdate = DateTime.Now;
                x.UpdateID = nUserID;
            });
            db.mTFacility.Where(w => lstDelID.Contains(w.CompanyID)).ToList().ForEach(x =>
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
        public int nCompID { get; set; }
        public string sCompName { get; set; }
        public string sStatus { get; set; }
        public int nCountFacility { get; set; }
        public DateTime? dUpdate { get; set; }
        public string sUpdate { get; set; }
        public string sLink { get; set; }
        public string sLinkFacility { get; set; }
        public string SearchBy { get; set; }
    }
    #endregion
}