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

public partial class admin_master_effluent_otherunit_lst : System.Web.UI.Page
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
            string sCon = "";
            string sSearch = itemSearch.sSearch.Trims().ToLower();
            string sStatus = itemSearch.sStatus.Trims().ToLower();

            if (!string.IsNullOrEmpty(sSearch))
            {
                sCon += @" AND sName LIKE '%" + sSearch + "%' ";
            }
            if (!string.IsNullOrEmpty(sStatus))
            {
                sCon += @" AND (LOWER(cActive) = '" + sStatus + @"')  ";
            }

            var TB_Effluent_Product = db.TEffluent_OtherProduct_Point.ToList(); //IsUse

            string _SQL = @" select nUnitID
                              ,sName
                              ,CASE WHEN cActive = 'Y' THEN 'Active' ELSE 'Inactive' END as sStatus
                              ,dUpdate
                            from TM_Effluent_Unit
                            where cDel = 'N' " + sCon + @"
                            ORDER BY dUpdate DESC";
            DataTable dt = CommonFunction.Get_Data(SystemFunction.strConnect, _SQL);
            lstData = CommonFunction.ConvertDatableToList<TDataTable>(dt).ToList();

            foreach (var item in lstData)
            {
                item.sUpdate = item.dUpdate.DateString();
                item.sLink = "<a class='btn btn-warning' href='admin_master_effluent_otherunit_update.aspx?str=" + HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(item.nUnitID.ToString())) + "'><i class='fa fa-edit'></i>&nbsp;Edit</a>";
                item.nIsUse = TB_Effluent_Product.Any(a => a.nUnitID == item.nUnitID) ? TB_Effluent_Product.Where(a => a.nUnitID == item.nUnitID).Count() : 0;
            }

            #region//SORT
            int? nSortCol = SystemFunction.GetIntNull(itemSearch.sIndexCol);
            switch ((itemSearch.sOrderBy + "").ToLower())
            {
                case SystemFunction.ASC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderBy(o => o.sName).ToList(); break;
                            case 2: lstData = lstData.OrderBy(o => o.sStatus).ToList(); break;
                            case 3: lstData = lstData.OrderBy(o => o.sUpdate).ToList(); break;
                        }
                    }
                    break;
                case SystemFunction.DESC:
                    {
                        switch (nSortCol)
                        {
                            case 1: lstData = lstData.OrderByDescending(o => o.sName).ToList(); break;
                            case 2: lstData = lstData.OrderByDescending(o => o.sStatus).ToList(); break;
                            case 3: lstData = lstData.OrderByDescending(o => o.sUpdate).ToList(); break;
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
            db.TM_Effluent_Unit.Where(w => lstDelID.Contains(w.nUnitID)).ToList().ForEach(x =>
            {
                x.cDel = "Y";
                x.dUpdate = DateTime.Now;
                x.nUpdateBy = nUserID;
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
        public int nUnitID { get; set; }
        public int? nIsUse { get; set; }
        public string sName { get; set; }
        public string sStatus { get; set; }
        public DateTime? dUpdate { get; set; }
        public string sUpdate { get; set; }
        public string sLink { get; set; }
    }
    #endregion
}