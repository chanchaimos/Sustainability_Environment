using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using ClosedXML.Excel;
using System.IO;
public partial class export_epi_input_intensity_2 : System.Web.UI.Page
{
    private const int nIndicator = 6;
    private const int nOperationType = 11;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!UserAcc.UserExpired())
        {
            string str1 = Request.QueryString["str1"] + "";
            string str2 = Request.QueryString["str2"] + "";
            if(ssTRetunrLoadData_2 != null)
            {
                if (ssTRetunrLoadData_2.lstIn.Count > 0)
                {
                    string sFacility = "";
                    string sYear = "";
                    if (!string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2))
                    {
                        sFacility = STCrypt.Decrypt(str1);
                        sYear = STCrypt.Decrypt(str2);

                    }
                    Export_EXCEL(sFacility, sYear);
                }
            }

        }
    }
    private void Export_EXCEL(string sFacility, string sYear)
    {
        int nFacility = SystemFunction.GetIntNullToZero(sFacility);
        string[] alphabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        MemoryStream fsExport = new MemoryStream();

        PTTGC_EPIEntities db = new PTTGC_EPIEntities();

        string[] Arr_Column = { "Indicator", "Unit", "Target", "Q1 : Jan", "Q1 : Feb", "Q1 : Mar", "Q2 : Apr", "Q2 : May", "Q2 : Jun", "Q3 : Jul", "Q3 : Aug", "Q3 : Sep", "Q4 : Oct", "Q4 : Nov", "Q4 : Dec", "Remark" };
        double[] Arr_ColumnWidth = { 50, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 50 };
        using (XLWorkbook wb = new XLWorkbook())
        {
            #region LG
            var ws = wb.Worksheets.Add("EPIFROM_Intensity");
            //สร้างหัวข้อ
            string sCol = alphabet[Arr_Column.Length - 1].ToUpper();
            #region CreatHeadReport
            ws.Cell("A1").Value = "Indicator : Intensity Denominator ";
            ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Cell("A1").Style.Font.FontName = "Cordia New";
            ws.Cell("A1").Style.Font.FontSize = 14.00;

            ws.Cell("A2").Value = "Operation Type : Petrochemicals";
            ws.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Cell("A2").Style.Font.FontName = "Cordia New";
            ws.Cell("A2").Style.Font.FontSize = 14.00;
            var Facility = db.mTFacility.FirstOrDefault(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 2 && w.ID + "" == sFacility);
            if (Facility != null)
            {
                sFacility = Facility.Name;
            }
            ws.Cell("A3").Value = "Sub-facility : " + sFacility;
            ws.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Cell("A3").Style.Font.FontName = "Cordia New";
            ws.Cell("A3").Style.Font.FontSize = 14.00;

            ws.Cell("A4").Value = "Year : " + sYear;
            ws.Cell("A4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Cell("A4").Style.Font.FontName = "Cordia New";
            ws.Cell("A4").Style.Font.FontSize = 14.00;
            #endregion


            //สร้างหัวตาราง
            int nRow = 5;
            var RowsHead = ws;
            string sCC = "#dbea97";



            int nRowna = 1;
            #region สร้างตาราง
            var lst = ssTRetunrLoadData_2;
            lst.lstIn.ForEach(f =>
            {
                sCC = "#dbea97";
      
                for (int i = 0; i < Arr_Column.Length; i++)
                {
                    sCol = alphabet[i].ToUpper();
                    GetCellHead(RowsHead, Arr_ColumnWidth[i], sCol, nRow + "", Arr_Column[i] + "", sCC);
                }
                nRow++;
                string sText = "";
                XLAlignmentHorizontalValues align = XLAlignmentHorizontalValues.Center;
                for (int j = 0; j < Arr_Column.Length; j++)
                {
                    sCol = alphabet[j].ToUpper();
                    sCC = "#dbea97";
                    if (f.cTotalAll != "Y")
                    {
                        sCC = "#ffedc4";
                    }
                    #region CaseRow
                    sText = SetText(j, f);
                    align = SetAlign(j);
                    #endregion
                    GetCellText(RowsHead, Arr_ColumnWidth[j], sCol, nRow + "", sText, align, j + "", sCC);
                }
                nRow++;
                f.lstarrDetail.ForEach(f2 =>
                {
                    for (int k = 0; k < Arr_Column.Length; k++)
                    {
                        sCC = "#FFFFFF";
                        sCol = alphabet[k].ToUpper();
                        sText = SetTextDetail(k, f2);
                        align = SetAlign(k);
                        GetCellText(RowsHead, Arr_ColumnWidth[k], sCol, nRow + "", sText, align, k + "", sCC);
                    }
                    nRow++;
                });

            });

            #endregion
            #endregion

            #region Deviate
            var dv = wb.Worksheets.Add("Deviate");
            //สร้างหัวข้อ
            sCol = alphabet[Arr_Column.Length - 1].ToUpper();
            #region CreatHeadReport
            dv.Cell("A1").Value = "Indicator : Intensity Denominator ";
            dv.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            dv.Cell("A1").Style.Font.FontName = "Cordia New";
            dv.Cell("A1").Style.Font.FontSize = 14.00;

            dv.Cell("A2").Value = "Operation Type : Refinery";
            dv.Cell("A2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            dv.Cell("A2").Style.Font.FontName = "Cordia New";
            dv.Cell("A2").Style.Font.FontSize = 14.00;

            dv.Cell("A3").Value = "Sub-facility : " + sFacility;
            dv.Cell("A3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            dv.Cell("A3").Style.Font.FontName = "Cordia New";
            dv.Cell("A3").Style.Font.FontSize = 14.00;

            dv.Cell("A4").Value = "Year : " + sYear;
            dv.Cell("A4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            dv.Cell("A4").Style.Font.FontName = "Cordia New";
            dv.Cell("A4").Style.Font.FontSize = 14.00;
            #endregion


            //สร้างหัวตาราง
            nRow = 5;
            RowsHead = dv;
            sCC = "#9cb726";


            nRowna = 1;
            #region สร้างตาราง
            var lstdv = SystemFunction.GetDeviate(nIndicator, nOperationType, nFacility, sYear);
            GetCellHead(RowsHead, 10, "A", nRow + "", "No." + "", sCC);
            GetCellHead(RowsHead, 20, "B", nRow + "", "Month" + "", sCC);
            GetCellHead(RowsHead, 20, "C", nRow + "", "Remark" + "", sCC);
            GetCellHead(RowsHead, 20, "D", nRow + "", "Action By" + "", sCC);
            GetCellHead(RowsHead, 20, "E", nRow + "", "Date" + "", sCC);
            nRow++;
            sCC = "#ffffff";
            lstdv.ForEach(f =>
            {
                XLAlignmentHorizontalValues align = XLAlignmentHorizontalValues.Center;
                GetCellText(RowsHead, 10, "A", nRow + "", nRowna + "", align, 0 + "", sCC);
                GetCellText(RowsHead, 20, "B", nRow + "", f.sMonth + "", align, 0 + "", sCC);
                GetCellText(RowsHead, 20, "C", nRow + "", f.sRemark + "", XLAlignmentHorizontalValues.Left, 0 + "", sCC);
                GetCellText(RowsHead, 20, "D", nRow + "", f.sActionBy + "", align, 0 + "", sCC);
                GetCellText(RowsHead, 20, "E", nRow + "", f.sDate + "", align, 0 + "", sCC);
                nRow++;
                nRowna++;

            });

            #endregion
            #endregion
            wb.SaveAs(fsExport);
        }
        string saveAsFileName = "Input_Intensity_" + sFacility + "_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("en-US")) + ".xlsx";
        fsExport.Position = 0;
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment; filename=" + saveAsFileName);
        Response.ContentType = "application/vnd.ms-excel";

        try
        {
            Response.BinaryWrite(fsExport.ToArray());
        }
        catch (Exception ee)
        { }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "SessionExpire", "UnblockUI();", true);
        Response.End();
    }
    public static sysGlobalClass.TRetunrLoadData ssTRetunrLoadData_2
    {
        get
        {
            return HttpContext.Current.Session["ssTRetunrLoadData_2"] is sysGlobalClass.TRetunrLoadData ?
                (sysGlobalClass.TRetunrLoadData)HttpContext.Current.Session["ssTRetunrLoadData_2"] : new sysGlobalClass.TRetunrLoadData();
        }
        set { HttpContext.Current.Session["ssTRetunrLoadData_2"] = value; }
    }
    public static double CalHeight(double HeightFix, int LenghtTitle, double MaxLenght)
    {
        return (HeightFix * ((LenghtTitle / MaxLenght) > 1.0 ? LenghtTitle / MaxLenght : 1));
    }
    public string SetText(int i, sysGlobalClass.TData_Intensity f)
    {
        string sText = "";
        switch (i)
        {
            //ชื่อ
            //string[] Arr_Column = { "No.", "PG", "Subject", "Employee ID", "Name", "Type", "Indicator", "Status" };
            case 0:
                sText = f.ProductName + "";
                break;
            case 1:
                sText = "Tonnes Product";
                break;
            //ชื่อเมนู
            case 2:
                sText = f.Target + "";
                break;
            //คำอธิบาย
            case 3:
                sText = f.M1 + "";
                break;
            //วันที่สร้าง
            case 4:
                sText = f.M2 + "";
                break;
            case 5:
                sText = f.M3 + "";
                break;
            case 6:
                sText = f.M4 + "";
                break;
            case 7:
                sText = f.M5 + "";
                break;
            case 8:
                sText = f.M6 + "";
                break;
            case 9:
                sText = f.M7 + "";
                break;
            case 10:
                sText = f.M8 + "";
                break;
            case 11:
                sText = f.M9 + "";
                break;
            case 12:
                sText = f.M10 + "";
                break;
            case 13:
                sText = f.M11 + "";
                break;
            case 14:
                sText = f.M12 + "";
                break;
            case 15:
                sText = f.sRemark + "";
                break;
                //UnSuccess//onProcess
        }

        return sText;
    }
    public string SetTextDetail(int i, sysGlobalClass.T_TIntensity_Other f)
    {
        string sText = "";
        switch (i)
        {
            //ชื่อ
            //string[] Arr_Column = { "No.", "PG", "Subject", "Employee ID", "Name", "Type", "Indicator", "Status" };
            case 0:
                sText = f.sIndicator + "";
                break;
            case 1:
                sText = "Tonnes Product";
                break;
            //ชื่อเมนู
            case 2:
                sText = f.sTarget + "";
                break;
            //คำอธิบาย
            case 3:
                sText = f.M1 + "";
                break;
            //วันที่สร้าง
            case 4:
                sText = f.M2 + "";
                break;
            case 5:
                sText = f.M3 + "";
                break;
            case 6:
                sText = f.M4 + "";
                break;
            case 7:
                sText = f.M5 + "";
                break;
            case 8:
                sText = f.M6 + "";
                break;
            case 9:
                sText = f.M7 + "";
                break;
            case 10:
                sText = f.M8 + "";
                break;
            case 11:
                sText = f.M9 + "";
                break;
            case 12:
                sText = f.M10 + "";
                break;
            case 13:
                sText = f.M11 + "";
                break;
            case 14:
                sText = f.M12 + "";
                break;
            case 15:
                sText = "";
                break;
        }

        return sText;
    }
    public XLAlignmentHorizontalValues SetAlign(int i)
    {
        XLAlignmentHorizontalValues align = XLAlignmentHorizontalValues.Right;
        switch (i)
        {
            case 0:
                align = XLAlignmentHorizontalValues.Left;
                break;
            case 1:
                align = XLAlignmentHorizontalValues.Center;
                break;
            case 15:
                align = XLAlignmentHorizontalValues.Left;
                break;
        }
        return align;
    }
    public void GetCellHead(IXLWorksheet RowsHead, double nWidthColumn, string StringCell, string NumCell, string sValue, string sCC)
    {
        var temColumn = RowsHead.Column(StringCell);
        //temColumn.Style.Fill.BackgroundColor = XLColor.DarkOrange;
        temColumn.Width = nWidthColumn;
        var itemCell = RowsHead.Cell(StringCell + NumCell);
        itemCell.Value = sValue;
        itemCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        itemCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
        //itemCell.Style.Font.Bold = true;
        itemCell.Style.Font.FontName = "Cordia New";
        itemCell.Style.Font.FontSize = 14.00;
        itemCell.Style.Font.FontColor = XLColor.FromHtml("#31708F");
        itemCell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        itemCell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        itemCell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        itemCell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        itemCell.Style.Fill.BackgroundColor = XLColor.FromHtml(sCC);
        itemCell.Style.Border.LeftBorderColor = XLColor.Black;
        itemCell.Style.Border.RightBorderColor = XLColor.Black;
        itemCell.Style.Border.TopBorderColor = XLColor.Black;
        itemCell.Style.Border.BottomBorderColor = XLColor.Black;

        itemCell.Style.Alignment.WrapText = true;
    }
    public void GetCellText(IXLWorksheet RowsHead, double nWidthColumn, string StringCell, string NumCell, string sValue, XLAlignmentHorizontalValues Align, string index,string sCC)
    {
        //RowsHead.Range(StringCell + NumCell+":")

        var temColumn = RowsHead.Column(StringCell);
        //temColumn.Style.Fill.BackgroundColor = XLColor.DarkOrange;

        temColumn.Width = nWidthColumn;

        var itemCell = RowsHead.Cell(StringCell + NumCell);

        itemCell.Value = sValue;
        itemCell.Style.Alignment.Horizontal = Align;
        itemCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
        //itemCell.Style.Font.Bold = true;
        itemCell.Style.Font.FontName = "Cordia New";
        itemCell.Style.Font.FontSize = 12;

        //itemCell.Style.Font.Underline = true;
        itemCell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
        itemCell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        itemCell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        itemCell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        itemCell.Style.Fill.BackgroundColor = XLColor.FromHtml(sCC);
        itemCell.Style.Border.LeftBorderColor = XLColor.Black;
        itemCell.Style.Border.RightBorderColor = XLColor.Black;
        itemCell.Style.Border.TopBorderColor = XLColor.Black;
        itemCell.Style.Border.BottomBorderColor = XLColor.Black;
        itemCell.Style.Alignment.WrapText = true;
    }
}