using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class export_epi_input_materials : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string str = Request.QueryString["str"];
            if (!string.IsNullOrEmpty(str))
            {
                string sDataDecrypt = STCrypt.Decrypt(str);
                var arrDataDecrypt = sDataDecrypt.Split('|');
                string sIncID = arrDataDecrypt[0];
                string sOprtID = arrDataDecrypt[1];
                string sFacID = arrDataDecrypt[2];
                string sYear = arrDataDecrypt[3];
                ExportData(sIncID, sOprtID, sFacID, sYear);
            }
        }
    }
    public void ExportData(string sIncID, string sOprtID, string sFacID, string sYear)
    {

        HttpResponse httpResponse = Response;
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
        #region QUERY
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<CProduct> lstProduct = new List<CProduct>();
        List<CUnit> lstUnit = new List<CUnit>();
        string[] arrShortMonth = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        int nIndID = SystemFunction.GetIntNullToZero(sIncID);
        int nOprtID = SystemFunction.GetIntNullToZero(sOprtID);
        int nFacID = SystemFunction.GetIntNullToZero(sFacID);
        string sIncName = db.mTIndicator.Any(w => w.ID == nIndID) ? db.mTIndicator.FirstOrDefault(w => w.ID == nIndID).Indicator : "";
        string sOprtName = db.mOperationType.Any(w => w.ID == nOprtID) ? db.mOperationType.FirstOrDefault(w => w.ID == nOprtID).Name : "";
        string sFacName = db.mTFacility.Any(w => w.ID == nFacID) ? db.mTFacility.FirstOrDefault(w => w.ID == nFacID).Name : "";
        bool IsNew = true;
        var itemEPI_FORM = db.TEPI_Forms.FirstOrDefault(w => w.sYear == sYear && w.IDIndicator == nIndID && w.OperationTypeID == nOprtID && w.FacilityID == nFacID);
        int EPI_FORMID = 0;
        if (itemEPI_FORM != null)
        {
            IsNew = false;
            EPI_FORMID = itemEPI_FORM.FormID;
        }
        int[] lstUnitUse = new int[] { 1, 2, 3 };
        var lstRemark = db.TMaterial_Remark.Where(w => w.FormID == EPI_FORMID).ToList();
        lstUnit = db.mTUnit.Where(w => lstUnitUse.Contains(w.UnitID)).OrderByDescending(o => o.nFactor).ThenByDescending(o => o.UnitName).Select(s => new CUnit { nUnitID = s.UnitID, sUnitName = s.UnitName }).ToList();
        db.mTProductIndicator.Where(w => w.IDIndicator == 8).ToList().ForEach(f =>
        {
            CProduct itemH = new CProduct();
            switch (f.ProductID)
            {
                case 33:
                    itemH.nHeaderID = null;
                    itemH.nLevel = 1;
                    break;
                case 34:
                    itemH.nHeaderID = 33;
                    itemH.nLevel = 2;
                    break;
                case 35:
                    itemH.nHeaderID = 34;
                    itemH.nLevel = 3;
                    break;
                case 36:
                    itemH.nHeaderID = 34;
                    itemH.nLevel = 3;
                    break;
                case 37:
                    itemH.nHeaderID = 33;
                    itemH.nLevel = 2;
                    break;
                case 38:
                    itemH.nHeaderID = 37;
                    itemH.nLevel = 3;
                    break;
                case 39:
                    itemH.nHeaderID = 37;
                    itemH.nLevel = 3;
                    break;
                case 40:
                    itemH.nHeaderID = 37;
                    itemH.nLevel = 3;
                    break;
                case 41:
                    itemH.nHeaderID = 33;
                    itemH.nLevel = 2;
                    break;
            }
            itemH.ProductID = f.ProductID;
            itemH.ProductName = f.ProductName;
            itemH.sUnit = f.sUnit;
            lstProduct.Add(itemH);
            if (itemEPI_FORM != null)
            {
                var lstTMaterial_Product = db.TMaterial_Product.Where(w => w.FormID == EPI_FORMID).ToList();
                var lstMeterial_ProductData = db.TMaterial_ProductData.Where(w => w.FormID == EPI_FORMID).ToList();
                if (itemH.nLevel <= 3)
                {
                    var item = lstTMaterial_Product.FirstOrDefault(w => w.ProductID == f.ProductID);
                    if (item != null)
                    {
                        itemH.M1 = item.M1;
                        itemH.M2 = item.M2;
                        itemH.M3 = item.M3;
                        itemH.M4 = item.M4;
                        itemH.M5 = item.M5;
                        itemH.M6 = item.M6;
                        itemH.M7 = item.M7;
                        itemH.M8 = item.M8;
                        itemH.M9 = item.M9;
                        itemH.M10 = item.M10;
                        itemH.M11 = item.M11;
                        itemH.M12 = item.M12;
                        itemH.Target = item.Target ?? "";
                        itemH.Total = item.nTotal;
                    }
                    else
                    {
                        itemH.M1 = "";
                        itemH.M2 = "";
                        itemH.M3 = "";
                        itemH.M4 = "";
                        itemH.M5 = "";
                        itemH.M6 = "";
                        itemH.M7 = "";
                        itemH.M8 = "";
                        itemH.M9 = "";
                        itemH.M10 = "";
                        itemH.M11 = "";
                        itemH.M12 = "";
                        itemH.Target = "";
                    }
                    lstMeterial_ProductData.Where(w => w.nUnderbyProductID == f.ProductID).ToList().ForEach(data =>
                    {
                        CProduct prdData = new CProduct();
                        prdData.nLevel = 4;
                        prdData.ProductID = data.nSubProductID;
                        prdData.ProductName = data.sName;
                        prdData.nHeaderID = data.nUnderbyProductID;
                        prdData.sUnit = data.nUnitID == 1 ? "m3" : lstUnit.FirstOrDefault(w => w.nUnitID == data.nUnitID).sUnitName;
                        prdData.sRemark = "";
                        prdData.sDensity = data.Density;
                        prdData.Target = data.Target ?? "";
                        prdData.M1 = data.M1;
                        prdData.M2 = data.M2;
                        prdData.M3 = data.M3;
                        prdData.M4 = data.M4;
                        prdData.M5 = data.M5;
                        prdData.M6 = data.M6;
                        prdData.M7 = data.M7;
                        prdData.M8 = data.M8;
                        prdData.M9 = data.M9;
                        prdData.M10 = data.M10;
                        prdData.M11 = data.M11;
                        prdData.M12 = data.M12;
                        prdData.sDensity = data.Density;
                        prdData.Total = data.nTotal;
                        lstProduct.Add(prdData);

                    });
                }
                itemH.sRemark = lstRemark.Any(w => w.ProductID == f.ProductID) ? lstRemark.Where(w => w.ProductID == f.ProductID).OrderByDescending(o => o.nVersion).FirstOrDefault().sRemark : "";

            }
        });
        #endregion
        #region BIND DATA
        IXLWorksheet ws1 = wb.Worksheets.Add("Data");
        ws1.PageSetup.Margins.Top = 0.2;
        ws1.PageSetup.Margins.Bottom = 0.2;
        ws1.PageSetup.Margins.Left = 0.1;
        ws1.PageSetup.Margins.Right = 0;
        ws1.PageSetup.Margins.Footer = 0;
        ws1.PageSetup.Margins.Header = 0;
        ws1.Style.Font.FontName = "Cordia New";

        string[] arrColor = new string[4] { "#dbea97", "#e7bb5b", "#ffedc4", "#f9f9f9" };
        nRow = 1;
        nCol = 1;

        SetTbl(ws1, "Indicator : " + sIncName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        nRow++;
        SetTbl(ws1, "Operation : " + sOprtName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        nRow++;
        SetTbl(ws1, "Facility : " + sFacName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        nRow++;
        SetTbl(ws1, "Year : " + sYear, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        nRow++;

        SetTbl(ws1, "Indicator", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 45);
        nCol++;
        SetTbl(ws1, "Density", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17);
        nCol++;
        SetTbl(ws1, "Unit", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 10.5);
        nCol++;
        SetTbl(ws1, "Target", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17);
        nCol++;
        for (int i = 0; i < 12; i++)
        {
            SetTbl(ws1, arrShortMonth[i], nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17);
            nCol++;
        }
        SetTbl(ws1, "Total", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 15);
        nCol++;
        SetTbl(ws1, "Remark", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 20);
        ws1.Range(nRow, 1, nRow, nCol).Style.Fill.BackgroundColor = XLColor.FromHtml("#9cb726");
        ws1.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        ws1.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        foreach (var item in lstProduct)
        {
            nRow++;
            nCol = 1;
            string[] arrDataInMonth = new string[12] { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M2 };
            SetTbl(ws1, "'" + item.ProductName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, item.sDensity, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, item.sUnit, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, item.Target, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            for (int i = 0; i < 12; i++)
            {
                SetTbl(ws1, arrDataInMonth[i], nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
                nCol++;
            }
            SetTbl(ws1, item.Total, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "", nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null);
            ws1.Range(nRow, 1, nRow, nCol).Style.Fill.BackgroundColor = XLColor.FromHtml(arrColor[item.nLevel - 1]);
            ws1.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws1.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        #endregion
        #region CreateEXCEL

        httpResponse.Clear();
        httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        string sName = "Materials_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("en-US"));

        httpResponse.AddHeader("content-disposition", "attachment;filename=" + sName + ".xlsx");

        // Flush the workbook to the Response.OutputStream
        using (MemoryStream memoryStream = new MemoryStream())
        {
            wb.SaveAs(memoryStream);
            memoryStream.WriteTo(httpResponse.OutputStream);
            memoryStream.Close();
        }

        httpResponse.End();

        #endregion
    }
    public class CProduct
    {
        public int? nHeaderID { get; set; }
        public int nLevel { get; set; }
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
        public string sUnit { get; set; }
        public string Target { get; set; }
        public string sDensity { get; set; }
        public string M1 { get; set; }
        public string M2 { get; set; }
        public string M3 { get; set; }
        public string M4 { get; set; }
        public string M5 { get; set; }
        public string M6 { get; set; }
        public string M7 { get; set; }
        public string M8 { get; set; }
        public string M9 { get; set; }
        public string M10 { get; set; }
        public string M11 { get; set; }
        public string M12 { get; set; }
        public string Total { get; set; }
        public string sRemark { get; set; }
    }
    public class CUnit
    {
        public int nUnitID { get; set; }
        public string sUnitName { get; set; }
    }
    public class CStatus
    {
        public int nMonth { get; set; }
        public int? nStatus { get; set; }
    }
}