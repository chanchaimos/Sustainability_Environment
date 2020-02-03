using ClosedXML.Excel;
using sysExtension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_reportHistory : System.Web.UI.Page
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
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();

        var lstHistory = db.TViewMenu_History.ToList();
        DateTime dStartValue = DateTime.ParseExact(itemSearch.sStartDate, "dd/MM/yyyy", new CultureInfo("en-US"));
        DateTime dEndValue = DateTime.ParseExact(itemSearch.sEndDate, "dd/MM/yyyy", new CultureInfo("en-US"));
        var lstMenu = db.TMenu.Where(w => w.cActive == "Y").ToList();
        var lstUser = db.mTUser.ToList();
        sslstData = new List<TDataTable>();
        lstHistory = lstHistory.Where(w => (w.dAction.Value.Date >= dStartValue.Date && w.dAction.Value.Date <= dEndValue)).ToList();
        var lstData = (from a in lstHistory
                       from b in lstMenu.Where(w => w.nMenuID == a.nMenuID)
                       from c in lstUser.Where(w => w.ID == a.nUserID)
                       select new TDataTable
                       {
                           nLogID = a.nLogID,
                           nMenuID = a.nMenuID,
                           nUserID = a.nUserID,
                           dActionDate = a.dAction,
                           Firstname = c.Firstname,
                           Username = c.Username,
                           Lastname = c.Lastname,
                           sMenu = b.sMenuName,
                           nLevel = b.nLevel,
                           nHead = b.sMenuHeadID
                       }).ToList();

        lstData.ForEach(f =>
        {
            f.sAction = SystemFunction.CovertDateEn2Th(f.dActionDate.Value + "", "EN4");
            if (f.nLevel == 2)
            {
                var mu = lstMenu.FirstOrDefault(w => w.nMenuID == f.nHead);
                if (mu != null)
                {
                    f.sMenu = mu.sMenuName;
                    var mu2 = lstMenu.FirstOrDefault(w => w.nMenuID == mu.sMenuHeadID);
                    if (mu != null)
                    {
                        f.sMenuHead = mu2.sMenuName;
                    }
                }
            }
            else if (f.nLevel == 1)
            {
                var mu = lstMenu.FirstOrDefault(w => w.nMenuID == f.nHead);
                if (mu != null)
                {
                    f.sMenuHead = mu.sMenuName;
                }
            }
            else
            {
                f.sMenuHead = f.sMenu;
                f.sMenu = "";
            }
        });
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
                        case 3: lstData = lstData.OrderBy(o => o.sMenuHead).ToList(); break;
                        case 4: lstData = lstData.OrderBy(o => o.sMenu).ToList(); break;
                        case 5: lstData = lstData.OrderBy(o => o.dActionDate).ToList(); break;
                    }
                }
                break;
            case SystemFunction.DESC:
                {
                    switch (nSortCol)
                    {
                        case 1: lstData = lstData.OrderByDescending(o => o.Username).ToList(); break;
                        case 2: lstData = lstData.OrderByDescending(o => o.Firstname).ToList(); break;
                        case 3: lstData = lstData.OrderByDescending(o => o.sMenuHead).ToList(); break;
                        case 4: lstData = lstData.OrderByDescending(o => o.sMenu).ToList(); break;
                        case 5: lstData = lstData.OrderByDescending(o => o.dActionDate).ToList(); break;
                    }
                }
                break;

        }
        #endregion
        sslstData = lstData;
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

        return result;
    }
    #region Export Excel
    protected void ExportExcel_Click(object sender, EventArgs e)
    {
        XLWorkbook wb = new XLWorkbook();
        int nRow = 1;
        int nCol = 1;

        #region Action
        Action<IXLWorksheet, string, int, int, int, bool, XLAlignmentHorizontalValues, XLAlignmentVerticalValues, bool, int?, double?> SetTbl = (sWorkSheet, sTxt, row, col, FontSize, Bold, Horizontal, Vertical, wraptext, dec, width) =>
        {
            if (sTxt == null) sTxt = "";
            sWorkSheet.Cell(row, col).Value = sTxt;
            sWorkSheet.Cell(row, col).Style.Font.FontSize = FontSize;
            sWorkSheet.Cell(row, col).Style.Font.Bold = Bold;
            sWorkSheet.Cell(row, col).Style.Alignment.WrapText = true;
            sWorkSheet.Cell(row, col).Style.Alignment.Horizontal = Horizontal;
            sWorkSheet.Cell(row, col).Style.Alignment.Vertical = Vertical;
            sWorkSheet.Cell(row, col).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            sWorkSheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            sWorkSheet.Cell(row, col).Style.Border.InsideBorderColor = XLColor.FromHtml("#343a40");
            sWorkSheet.Cell(row, col).Style.Border.OutsideBorderColor = XLColor.FromHtml("#343a40");
            if (width != null)
                sWorkSheet.Column(col).Width = width.Value;
            if (dec != null && sTxt != "0")
            {

                string sformate = "#,##";
                sWorkSheet.Cell(row, col).Style.NumberFormat.Format = sformate;

            }
            var nIndex = sTxt.Split('/').Length;
            if (nIndex == 3)
            {
                sWorkSheet.Cell(row, col).Style.DateFormat.Format = "dd/MM/yyyy";
            }
        };
        #endregion

        #region Query
        IXLWorksheet ws1 = wb.Worksheets.Add("History");
        ws1.PageSetup.Margins.Top = 0.2;
        ws1.PageSetup.Margins.Bottom = 0.2;
        ws1.PageSetup.Margins.Left = 0.1;
        ws1.PageSetup.Margins.Right = 0;
        ws1.PageSetup.Margins.Footer = 0;
        ws1.PageSetup.Margins.Header = 0;
        ws1.Style.Font.FontName = "Cordia New";

        SetTbl(ws1, "No.", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 10);
        nCol++;
        SetTbl(ws1, "Username", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 20);
        nCol++;
        SetTbl(ws1, "Full Name", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 20);
        nCol++;
        SetTbl(ws1, "Head Menu Name", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 25);
        nCol++;
        SetTbl(ws1, "Menu Name", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 25);
        nCol++;
        SetTbl(ws1, "Action Date", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 20);

        ws1.Range(nRow, 1, nRow, nCol).Style.Fill.BackgroundColor = XLColor.FromHtml("#e3e3f1");
        ws1.Range(nRow, 1, nRow, nCol).Style.Font.FontColor = XLColor.Black;
        ws1.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        ws1.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        ws1.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorderColor = XLColor.Black;
        ws1.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorderColor = XLColor.Black;

        nRow++;
        string sEmp = "";
        int i = 1;
        sslstData.ForEach(f =>
        {
            nCol = 1;
            SetTbl(ws1, "'" + i, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Top, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + f.Username, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Top, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + f.Firstname + " " + f.Lastname, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Top, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + f.sMenuHead, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Top, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + f.sMenu, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Top, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + f.sAction, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Top, true, null, null);
            nRow++;
            i++;

        });

        #endregion



        #region CreateEXCEL

        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        string sName = "History_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("en-US"));

        Response.AddHeader("content-disposition", "attachment;filename=" + sName + ".xlsx");

        // Flush the workbook to the Response.OutputStream
        using (MemoryStream memoryStream = new MemoryStream())
        {
            wb.SaveAs(memoryStream);
            memoryStream.WriteTo(Response.OutputStream);
            memoryStream.Close();
        }

        Response.End();

        #endregion

    }
    #endregion
    [Serializable]
    public class CSearch : sysGlobalClass.CommonLoadData
    {
        public string sSearch { get; set; }
        public string sStatus { get; set; }
        public string sPrms { get; set; }
        public string sUserRole { get; set; }
        public string sOperationSearch { get; set; }
        public string sFacilitySearch { get; set; }
        public string sStartDate { get; set; }
        public string sEndDate { get; set; }
    }

    [Serializable]
    public class TRetunrLoadData : sysGlobalClass.Pagination
    {
        public List<TDataTable> lstData { get; set; }
        public List<sysGlobalClass.T_Facility> lstData_Facility { get; set; }
    }
    [Serializable]
    public class TDataTable
    {
        public long nLogID { get; set; }
        public int? nUserID { get; set; }
        public DateTime? dActionDate { get; set; }
        public int? nMenuID { get; set; }
        public int nID { get; set; }
        public int? nLevel { get; set; }
        public int? nHead { get; set; }
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
        public string sMenu { get; set; }
        public string sMenuHead { get; set; }
        public string sAction { get; set; }
        public string sStatus { get; set; }
        public DateTime? dUpdate { get; set; }
        public string sUpdate { get; set; }
        public string sLink { get; set; }
        public int nCountRole { get; set; }
    }
    public static List<TDataTable> sslstData
    {
        get
        {
            return HttpContext.Current.Session["sslstData"] is List<TDataTable> ?
                (List<TDataTable>)HttpContext.Current.Session["sslstData"] : new List<TDataTable>();
        }
        set { HttpContext.Current.Session["sslstData"] = value; }
    }
}