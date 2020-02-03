using sysExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_operation_type_lst : System.Web.UI.Page
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
                SystemFunction.BindDropdownPageSize(ddlPageSize, null);
                int Prms = SystemFunction.GetPermissionMenu(38);
                hdfPrmsMenu.Value = Prms + "";
                if (Prms == 1)
                {
                    IsView = true;
                }
                if (IsView)
                {
                    btnCreate.Visible = false;
                    ckbAll.Visible = false;
                    btnDel.Visible = false;
                }
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
            //lstData = db.mTCompany.Where(w => w.cDel == "N" && (!string.IsNullOrEmpty(sSearch) ? w.Name.ToLower().Contains(sSearch) : true)).Select(s => new TDataTable
            //{
            //    nCompID = s.ID,
            //    sCompName = s.Name,
            //    sStatus = s.cActive == "Y" ? "Active" : "Inactive",
            //    dUpdate = s.dUpdate
            //}).OrderByDescending(o => o.dUpdate).ToList();
            var query = from oType in db.mOperationType.Where(w => w.cDel == "N" && w.cManage == "Y").OrderBy(o => o.ID)
                        from f in db.mTFacility.Where(w => w.OperationTypeID == oType.ID && w.cDel == "N" && w.cActive == "Y").DefaultIfEmpty()
                        from g in db.mTFacility_Operationtype.Where(w => w.nOperationtypeID == w.nOperationtypeID).DefaultIfEmpty()
                        where (!string.IsNullOrEmpty(sSearch) ? oType.Name.ToLower().Contains(sSearch) : true) && (!string.IsNullOrEmpty(sStatus) ? oType.cActive.ToLower().Contains(sStatus) : true)
                        group new { oType, g } by new { oType.ID, oType.Name, oType.Description, oType.sCode, oType.sRemark, oType.cActive, oType.dUpdate } into grp
                        select new
                        {
                            nID = grp.Key.ID,
                            sName = grp.Key.Name,
                            sRemark = grp.Key.sRemark,
                            sCode = grp.Key.sCode,
                            sStatus = grp.Key.cActive,
                            sDescription = grp.Key.Description,
                            dUpdate = grp.Key.dUpdate,
                            nFacility = grp.Where(w => w.g.nOperationtypeID == grp.Key.ID).Count(),
                        };

            lstData = query.ToList().Select(s => new TDataTable
            {
                nID = s.nID,
                sName = s.sName,
                sDescription = s.sDescription,
                nFacility = s.nFacility,
                sCode = s.sCode,
                sStatus = s.sStatus,
                sRemark = s.sRemark,
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
                            case 1: lstData = lstData.OrderBy(o => o.sName).ToList(); break;
                            case 2: lstData = lstData.OrderBy(o => o.sDescription).ToList(); break;
                            case 3: lstData = lstData.OrderBy(o => o.nFacility).ToList(); break;
                            case 4: lstData = lstData.OrderBy(o => o.sStatus).ToList(); break;
                        }
                    }
                    break;
                case SystemFunction.DESC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderByDescending(o => o.sName).ToList(); break;
                            case 2: lstData = lstData.OrderByDescending(o => o.sDescription).ToList(); break;
                            case 3: lstData = lstData.OrderByDescending(o => o.nFacility).ToList(); break;
                            case 4: lstData = lstData.OrderByDescending(o => o.sStatus).ToList(); break;
                        }
                    }
                    break;
            }
            #endregion

            #region//Final Action >> Skip Take Data For Javasacript
            sysGlobalClass.Pagination dataPage = new sysGlobalClass.Pagination();
            dataPage = SystemFunction.GetPaginationSmall(SystemFunction.GetIntNullToZero(itemSearch.sPageSize), SystemFunction.GetIntNullToZero(itemSearch.sPageIndex), lstData.Count);
            lstData = lstData.Skip(dataPage.nSkipData).Take(dataPage.nTakeData).ToList();
            // bool isView = hdfPrmsMenu.Value == 1;
            foreach (var item in lstData)
            {
                if (IsView)
                {
                    item.sLink = "<a class='btn btn-primary' href='admin_operation_type_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nID.ToString())) + "'><i class='fa fa-search'></i>&nbsp;View</a>";
                }
                else
                {
                    item.sLink = "<a class='btn btn-warning' href='admin_operation_type_update.aspx?strid=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nID.ToString())) + "'><i class='fa fa-edit'></i>&nbsp;Edit</a>";
                }
                //item.sUpdate = item.dUpdate.DateString();

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
            db.mOperationType.Where(w => lstDelID.Contains(w.ID)).ToList().ForEach(x =>
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
    public class TDataTable
    {
        public int nID { get; set; }
        public string sName { get; set; }
        public string sDescription { get; set; }
        public int nFacility { get; set; }
        public string sRemark { get; set; }
        public string sCode { get; set; }
        public string sStatus { get; set; }
        public string sLink { get; set; }
        public DateTime? dUpdate { get; set; }
    }
    #endregion
}