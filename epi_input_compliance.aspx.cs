using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using sysExtension;
using ClosedXML.Excel;
using System.IO;
using System.Globalization;

public partial class epi_input_compliance : System.Web.UI.Page
{
    private const string sFolderInSharePahtTemp = "UploadFiles/Compliance/Temp/";
    private const string sFolderInPathSave = "UploadFiles/Compliance/File/{0}/";
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_EPI_FORMS)this.Master).SetBodyEventOnLoad(myFunc);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!UserAcc.UserExpired())
        {
            if (!IsPostBack)
            {
                string sQueryStrIndID = Request.QueryString["in"];
                if (!string.IsNullOrEmpty(sQueryStrIndID))
                {
                    hdfIndID.Value = STCrypt.Encrypt(sQueryStrIndID);

                    ((_MP_EPI_FORMS)this.Master).hdfPRMS = SystemFunction.GetPermissionMenu(14) + "";
                    ((_MP_EPI_FORMS)this.Master).hdfCheckRole = UserAcc.GetObjUser().nRoleID + "";
                }
                else
                {
                    SetBodyEventOnLoad(SystemFunction.DialogWarningRedirect(SystemFunction.Msg_HeadWarning, "Invalid Data", "Intensity_from.aspx"));// กรณีเข้ามาด้วย link ที่ไม่มี Querystring
                }
            }
        }
        else
        {
            SetBodyEventOnLoad(SystemFunction.PopupLogin());
        }
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static CReturnData LoadData(CParam param)
    {
        CReturnData result = new CReturnData();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        if (!UserAcc.UserExpired())
        {
            result.incData = new cDataInc();
            result.lstCompliance = new List<cCompliance>();
            result.lstStatus = new List<sysGlobalClass.T_TEPI_Workflow>();
            int nIndID = SystemFunction.GetIntNullToZero(param.sIndID);
            int nOprtID = SystemFunction.GetIntNullToZero(param.sOprtID);
            int nFacID = SystemFunction.GetIntNullToZero(param.sFacID);
            result.hdfPRMS = SystemFunction.GetPermission_EPI_FROMS(nIndID, nFacID) + "";
            string sYear = param.sYear;

            var itemPrdInc = db.mTProductIndicator.FirstOrDefault(w => w.IDIndicator == 2);
            result.incData.ProductID = itemPrdInc.ProductID;
            result.incData.sUnit = itemPrdInc.sUnit;
            result.incData.sProductName = itemPrdInc.ProductName;
            result.incData.nUnitID = 66;
            result.incData.sTarget = "";
            #region EPI_FORM
            var itemEPI_FORM = db.TEPI_Forms.FirstOrDefault(w => w.sYear == sYear && w.IDIndicator == nIndID && w.OperationTypeID == nOprtID && w.FacilityID == nFacID);
            if (itemEPI_FORM != null)
            {
                var itemDataCompliance = db.TCompliance_Product.FirstOrDefault(w => w.FormID == itemEPI_FORM.FormID);
                #region Data Indicator
                if (itemDataCompliance != null)
                {
                    var itemUnit = db.mTUnit.FirstOrDefault(w => w.UnitID == itemDataCompliance.UnitID);
                    result.incData.nUnitID = itemDataCompliance.UnitID;
                    result.incData.sUnit = itemUnit != null ? itemUnit.UnitName : result.incData.sUnit;
                    result.incData.sTarget = itemDataCompliance.Target;
                    result.incData.M1 = itemDataCompliance.M1;
                    result.incData.M2 = itemDataCompliance.M2;
                    result.incData.M3 = itemDataCompliance.M3;
                    result.incData.M4 = itemDataCompliance.M4;
                    result.incData.M5 = itemDataCompliance.M5;
                    result.incData.M6 = itemDataCompliance.M6;
                    result.incData.M7 = itemDataCompliance.M7;
                    result.incData.M8 = itemDataCompliance.M8;
                    result.incData.M9 = itemDataCompliance.M9;
                    result.incData.M10 = itemDataCompliance.M10;
                    result.incData.M11 = itemDataCompliance.M11;
                    result.incData.M12 = itemDataCompliance.M12;
                    result.incData.IsCheckM1 = itemDataCompliance.IsCheckM1;
                    result.incData.IsCheckM2 = itemDataCompliance.IsCheckM2;
                    result.incData.IsCheckM3 = itemDataCompliance.IsCheckM3;
                    result.incData.IsCheckM4 = itemDataCompliance.IsCheckM4;
                    result.incData.IsCheckM5 = itemDataCompliance.IsCheckM5;
                    result.incData.IsCheckM6 = itemDataCompliance.IsCheckM6;
                    result.incData.IsCheckM7 = itemDataCompliance.IsCheckM7;
                    result.incData.IsCheckM8 = itemDataCompliance.IsCheckM8;
                    result.incData.IsCheckM9 = itemDataCompliance.IsCheckM9;
                    result.incData.IsCheckM10 = itemDataCompliance.IsCheckM10;
                    result.incData.IsCheckM11 = itemDataCompliance.IsCheckM11;
                    result.incData.IsCheckM12 = itemDataCompliance.IsCheckM12;
                }
                #endregion

                #region Data Compliance
                result.lstCompliance = db.TCompliance.Where(w => w.FormID == itemEPI_FORM.FormID).Select(s => new cCompliance
                {
                    nComplianceID = s.nComplianceID,
                    ComplianceDate = s.ComplianceDate.HasValue ? s.ComplianceDate.Value : (DateTime?)null,
                    sDocNumber = s.sDocNumber,
                    sIssueBy = s.sIssueBy,
                    sSubject = s.sSubject,
                    sDetail = s.sDetail,
                    nMonth = s.ComplianceDate.HasValue ? s.ComplianceDate.Value.Month : 0,
                    IsDel = false,
                    IsNew = false,
                    IsSubmited = true,
                    IsShow = true,
                }).ToList();
                result.lstCompliance.ForEach(f =>
                {
                    f.sIssueDate = f.ComplianceDate.HasValue ? f.ComplianceDate.Value.ToString("dd/MM/yyyy") : "";
                    f.ComplianceDate = null;
                });
                var lstFileCompliance = db.TCompliance_File.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
                foreach (var itemCompliance in result.lstCompliance)
                {
                    itemCompliance.lstFile = new List<sysGlobalClass.FuncFileUpload.ItemData>();
                    itemCompliance.lstFile = lstFileCompliance.Where(w => w.nComplianceID == itemCompliance.nComplianceID).Select(s => new sysGlobalClass.FuncFileUpload.ItemData
                    {
                        ID = s.nFileID,
                        FileName = s.sFileName,
                        SaveToFileName = s.sSysFileName,
                        SaveToPath = s.sPath,
                        url = s.sPath + s.sSysFileName,
                        IsNewFile = false,
                        IsCompleted = true,
                        sDelete = "N",
                        sDescription = s.sDescription,
                    }).ToList();
                }
                #endregion

                #region Status Month
                result.lstStatus = db.TEPI_Workflow.Where(w => w.FormID == itemEPI_FORM.FormID).Select(s => new sysGlobalClass.T_TEPI_Workflow { nMonth = s.nMonth, nStatusID = s.nStatusID }).ToList();
                #endregion
            }
            else
            {
                for (int i = 1; i <= 12; i++)
                {
                    result.lstStatus.Add(new sysGlobalClass.T_TEPI_Workflow { nMonth = i, nStatusID = 0 });
                }
                result.incData.sTarget = "";
                result.incData.M1 = "";
                result.incData.M2 = "";
                result.incData.M3 = "";
                result.incData.M4 = "";
                result.incData.M5 = "";
                result.incData.M6 = "";
                result.incData.M7 = "";
                result.incData.M8 = "";
                result.incData.M9 = "";
                result.incData.M10 = "";
                result.incData.M11 = "";
                result.incData.M12 = "";
                result.incData.IsCheckM1 = "N";
                result.incData.IsCheckM2 = "N";
                result.incData.IsCheckM3 = "N";
                result.incData.IsCheckM4 = "N";
                result.incData.IsCheckM5 = "N";
                result.incData.IsCheckM6 = "N";
                result.incData.IsCheckM7 = "N";
                result.incData.IsCheckM8 = "N";
                result.incData.IsCheckM9 = "N";
                result.incData.IsCheckM10 = "N";
                result.incData.IsCheckM11 = "N";
                result.incData.IsCheckM12 = "N";
            }
            #endregion
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
    public static sysGlobalClass.CResutlWebMethod SaveToDB(cDataSave arrData)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        Func<List<string>, string> Sum = (lstData) =>
        {
            string sReusltData = "";
            int Total = 0;
            foreach (var item in lstData)
            {
                if (item.Trim() != "" && item.ToLower().Trim() != "n/a")
                {
                    Total += int.Parse(item);
                }
            }
            return sReusltData;
        };
        if (!UserAcc.UserExpired())
        {
            var itemEPI_FORM = db.TEPI_Forms.FirstOrDefault(w => w.sYear == arrData.sYear && w.IDIndicator == arrData.nIndicatorID && w.OperationTypeID == arrData.nOperationID && w.FacilityID == arrData.nFacilityID);
            bool IsNew = itemEPI_FORM != null ? false : true;
            int FORM_ID = itemEPI_FORM != null ? itemEPI_FORM.FormID : (db.TEPI_Forms.Any() ? db.TEPI_Forms.Max(m => m.FormID) + 1 : 1);

            #region EPI_FORM
            var EPI_FORM = db.TEPI_Forms.FirstOrDefault(w => w.FormID == FORM_ID);
            if (IsNew)
            {
                EPI_FORM = new TEPI_Forms();
                EPI_FORM.FormID = FORM_ID;
                EPI_FORM.sYear = arrData.sYear;
                EPI_FORM.IDIndicator = arrData.nIndicatorID;
                EPI_FORM.OperationTypeID = arrData.nOperationID;
                EPI_FORM.FacilityID = arrData.nFacilityID;
                EPI_FORM.sAddBy = UserAcc.GetObjUser().nUserID;
                EPI_FORM.dAddDate = DateTime.Now;
                EPI_FORM.ResponsiblePerson = UserAcc.GetObjUser().nUserID;
                EPI_FORM.sUpdateBy = UserAcc.GetObjUser().nUserID;
                EPI_FORM.dUpdateDate = DateTime.Now;
                db.TEPI_Forms.Add(EPI_FORM);
            }
            else
            {
                if (!SystemFunction.IsSuperAdmin())
                {
                    EPI_FORM.ResponsiblePerson = UserAcc.GetObjUser().nUserID;
                    EPI_FORM.sUpdateBy = UserAcc.GetObjUser().nUserID;
                    EPI_FORM.dUpdateDate = DateTime.Now;
                }
            }
            db.SaveChanges();
            #endregion

            #region EPI Workflow
            int nWkFlowID = db.TEPI_Workflow.Any() ? db.TEPI_Workflow.Max(m => m.nReportID) + 1 : 1;
            for (int i = 1; i <= 12; i++)
            {
                int nStatus = 0;
                if (arrData.nStatus != 0)
                {
                    var itemData = arrData.lstMonthSubmit.FirstOrDefault(a => a == i);
                    if (itemData != null && itemData != 0)
                    {
                        nStatus = arrData.nStatus;
                    }
                }
                var wkflow = db.TEPI_Workflow.FirstOrDefault(w => w.FormID == FORM_ID && w.nMonth == i);
                if (wkflow == null)
                {
                    wkflow = new TEPI_Workflow();
                    wkflow.nReportID = nWkFlowID;
                    wkflow.FormID = FORM_ID;
                    wkflow.nMonth = i;
                    wkflow.nStatusID = nStatus;
                    wkflow.nActionBy = UserAcc.GetObjUser().nUserID;
                    wkflow.dAction = DateTime.Now;
                    db.TEPI_Workflow.Add(wkflow);
                    nWkFlowID++;
                }
            }
            db.SaveChanges();
            #endregion

            #region Product
            if (arrData.nStatus != 24)
            {
                if (!IsNew)
                {
                    db.TCompliance_Product.RemoveRange(db.TCompliance_Product.Where(w => w.FormID == FORM_ID));
                    db.TCompliance.RemoveRange(db.TCompliance.Where(w => w.FormID == FORM_ID));
                    //db.TCompliance_File.RemoveRange(db.TCompliance_File.Where(w => w.FormID == FORM_ID));
                    db.SaveChanges();
                }

                #region TCompliance_Product
                TCompliance_Product dataCompliancePrd = new TCompliance_Product();
                dataCompliancePrd.FormID = FORM_ID;
                dataCompliancePrd.ProductID = arrData.incData.ProductID;
                dataCompliancePrd.UnitID = arrData.incData.nUnitID;
                dataCompliancePrd.Target = arrData.incData.sTarget;
                dataCompliancePrd.M1 = arrData.incData.M1;
                dataCompliancePrd.M2 = arrData.incData.M2;
                dataCompliancePrd.M3 = arrData.incData.M3;
                dataCompliancePrd.M4 = arrData.incData.M4;
                dataCompliancePrd.M5 = arrData.incData.M5;
                dataCompliancePrd.M6 = arrData.incData.M6;
                dataCompliancePrd.M7 = arrData.incData.M7;
                dataCompliancePrd.M8 = arrData.incData.M8;
                dataCompliancePrd.M9 = arrData.incData.M9;
                dataCompliancePrd.M10 = arrData.incData.M10;
                dataCompliancePrd.M11 = arrData.incData.M11;
                dataCompliancePrd.M12 = arrData.incData.M12;

                dataCompliancePrd.IsCheckM1 = arrData.incData.IsCheckM1;
                dataCompliancePrd.IsCheckM2 = arrData.incData.IsCheckM2;
                dataCompliancePrd.IsCheckM3 = arrData.incData.IsCheckM3;
                dataCompliancePrd.IsCheckM4 = arrData.incData.IsCheckM4;
                dataCompliancePrd.IsCheckM5 = arrData.incData.IsCheckM5;
                dataCompliancePrd.IsCheckM6 = arrData.incData.IsCheckM6;
                dataCompliancePrd.IsCheckM7 = arrData.incData.IsCheckM7;
                dataCompliancePrd.IsCheckM8 = arrData.incData.IsCheckM8;
                dataCompliancePrd.IsCheckM9 = arrData.incData.IsCheckM9;
                dataCompliancePrd.IsCheckM10 = arrData.incData.IsCheckM10;
                dataCompliancePrd.IsCheckM11 = arrData.incData.IsCheckM11;
                dataCompliancePrd.IsCheckM12 = arrData.incData.IsCheckM12;

                List<string> lstForSum = new List<string> { arrData.incData.M1, arrData.incData.M2, arrData.incData.M3, arrData.incData.M4, arrData.incData.M5, 
                    arrData.incData.M6, arrData.incData.M7, arrData.incData.M8, arrData.incData.M9, arrData.incData.M10, arrData.incData.M11, arrData.incData.M12 };
                dataCompliancePrd.nTotal = Sum(lstForSum);
                db.TCompliance_Product.Add(dataCompliancePrd);
                db.SaveChanges();
                #endregion

                #region TCompliance
                int nCompliance = 1;
                arrData.lstCompliance.ForEach(f =>
                {
                    if (f.IsShow && f.IsSubmited)
                    {
                        TCompliance data = new TCompliance();
                        data.FormID = FORM_ID;
                        data.nComplianceID = nCompliance;
                        data.ComplianceDate = DateTime.ParseExact(f.sIssueDate, "dd/MM/yyyy", null);
                        data.sDocNumber = f.sDocNumber;
                        data.sIssueBy = f.sIssueBy;
                        data.sSubject = f.sSubject;
                        data.sDetail = f.sDetail;
                        data.nAddBy = UserAcc.GetObjUser().nUserID;
                        data.dAdd = DateTime.Now;
                        db.TCompliance.Add(data);
                        db.SaveChanges();

                        #region TCompliance_File
                        if (f.lstFile.Any())
                        {
                            string sPathSave = string.Format(sFolderInPathSave, FORM_ID);
                            SystemFunction.CreateDirectory(sPathSave);

                            //ลบไฟล์เดิมที่เคยมีและกดลบจากหน้าเว็บ
                            var qDelFile = f.lstFile.Where(w => w.IsNewFile == false && w.sDelete == "Y").ToList();
                            if (qDelFile.Any())
                            {
                                foreach (var qf in qDelFile)
                                {
                                    var query = db.TCompliance_File.FirstOrDefault(w => w.FormID == FORM_ID && w.nComplianceID == f.nComplianceID && w.sSysFileName == qf.SaveToFileName);
                                    if (query != null)
                                    {
                                        new SystemFunction().DeleteFileInServer(query.sPath, query.sSysFileName);
                                        // new SystemFunction2().DeleteFileInServer(query.sSysPath, query.sSysFileName);
                                        db.TCompliance_File.Remove(query);
                                        ///db.TAuditPlan_AttachFile.Remove(query);
                                    }
                                }
                                db.SaveChanges();
                            }

                            //Update Description
                            f.lstFile.Where(w => w.IsNewFile == false && w.sDelete == "N").ToList().ForEach(f2U =>
                            {
                                var data2Update = db.TCompliance_File.FirstOrDefault(w => w.FormID == FORM_ID && w.nComplianceID == f.nComplianceID && w.nFileID == f2U.ID);
                                if (data2Update != null)
                                {
                                    data2Update.sDescription = f2U.sDescription;
                                }
                            });
                            //Save New File Only
                            var lstSave = f.lstFile.Where(w => w.IsNewFile == true && w.sDelete == "N").ToList();
                            if (lstSave.Any())
                            {
                                int nFileID = db.TCompliance_File.Where(w => w.FormID == FORM_ID && w.nComplianceID == f.nComplianceID).Any() ? db.TCompliance_File.Where(w => w.FormID == FORM_ID && w.nComplianceID == f.nComplianceID).Max(m => m.nFileID) + 1 : 1;

                                foreach (var s in lstSave)
                                {
                                    string sSystemFileName = FORM_ID + "_" + f.nComplianceID + "_" + nFileID + "-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "." + SystemFunction.GetFileNameFromFileupload(s.SaveToFileName, ""); //+ "." + SystemFunction.GetFileNameFromFileupload(s.SaveToFileName, "") SystemFunction2.GetFileType(item.SaveToFileName);
                                    SystemFunction.UpFile2Server(s.SaveToPath, sPathSave, s.SaveToFileName, sSystemFileName);

                                    // SystemFunction2.UpFile2Server(item.SaveToPath, sPathSave, item.SaveToFileName, sSystemFileName);
                                    TCompliance_File t = new TCompliance_File();
                                    t.FormID = FORM_ID;
                                    t.nComplianceID = f.nComplianceID;
                                    t.nFileID = nFileID;
                                    t.sSysFileName = sSystemFileName;
                                    t.sFileName = s.FileName;
                                    t.sPath = sPathSave;
                                    t.sDescription = s.sDescription;
                                    db.TCompliance_File.Add(t);
                                    nFileID++;
                                }
                                db.SaveChanges();
                            }
                        }
                        #endregion

                        nCompliance++;
                    }
                });
                #endregion

                new EPIFunc().RecalculateCompliance(arrData.nOperationID, arrData.nFacilityID, arrData.sYear);
            }
            #endregion

            if (arrData.nStatus != 27)
            {
                new Workflow().UpdateHistoryStatus(FORM_ID);
            }

            if (arrData.nStatus != 0 && arrData.nStatus != 9999)
            {
                string sMode = "";
                switch (arrData.nStatus)
                {
                    case 1:
                        sMode = "SM";
                        break;
                    case 2:
                        sMode = "RQ";
                        break;
                    case 24:
                        sMode = "RC";
                        break;
                    case 27:
                        sMode = "APC";
                        break;
                }
                result = new Workflow().WorkFlowAction(FORM_ID, arrData.lstMonthSubmit, sMode, UserAcc.GetObjUser().nUserID, UserAcc.GetObjUser().nRoleID, arrData.sRemarkRequestEdit);
            }
            else
            {
                if (arrData.nStatus != 0)
                {
                    if (UserAcc.GetObjUser().nRoleID == 4)//ENVI Corporate (L2) >> Req.09.04.2019 Send email to L0 on L2 Modified data.
                    {
                        new Workflow().SendEmailToL0onL2EditData(arrData.sYear, arrData.nIndicatorID, arrData.nFacilityID, arrData.nOperationID);
                    }
                }
                result.Status = SystemFunction.process_Success;
            }
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
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
        List<cReport> lstProduct = new List<cReport>();
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
        string sProductName = "";
        var dataCompliance = db.mTProductIndicator.FirstOrDefault(w => w.IDIndicator == 2);
        if (dataCompliance != null)
        {
            sProductName = dataCompliance.ProductName;
        }
        lstProduct = db.TCompliance.Where(w => w.FormID == EPI_FORMID).OrderBy(o => o.ComplianceDate).Select(s => new cReport
        {
            ComplianceDate = s.ComplianceDate.HasValue ? s.ComplianceDate.Value : (DateTime?)null,
            sProductName = sProductName,
            nMonth = s.ComplianceDate.HasValue ? s.ComplianceDate.Value.Month : 0,
            sDocNumber = s.sDocNumber,
            sIssueBy = s.sIssueBy,
            sSubject = s.sSubject,
            sDetail = s.sDetail,
        }).ToList();
        lstProduct.ForEach(f =>
        {
            f.sIssueDate = f.ComplianceDate.HasValue ? f.ComplianceDate.Value.ToString("dd/MM/yyyy") : "";
        });

        var lstDeviate = SystemFunction.GetDeviate(nIndID, nOprtID, nFacID, sYear);
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

        SetTbl(ws1, "Product", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 45);
        nCol++;
        SetTbl(ws1, "Month", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17);
        nCol++;
        SetTbl(ws1, "Issue date", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 12);
        nCol++;
        SetTbl(ws1, "Document number", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17);
        nCol++;
        SetTbl(ws1, "Issued by", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 15);
        nCol++;
        SetTbl(ws1, "Subject", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 35);
        nCol++;
        SetTbl(ws1, "Corrective action", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 35);
        ws1.Range(nRow, 1, nRow, nCol).Style.Fill.BackgroundColor = XLColor.FromHtml("#9cb726");
        ws1.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        ws1.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        foreach (var item in lstProduct)
        {
            nRow++;
            nCol = 1;
            SetTbl(ws1, "'" + item.sProductName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + arrShortMonth[item.nMonth.Value - 1], nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + item.sIssueDate, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + item.sDocNumber, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + item.sIssueBy, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + item.sSubject, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + item.sDetail, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            ws1.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws1.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        #endregion
        #region BIND DATA DEVIATE
        IXLWorksheet ws2 = wb.Worksheets.Add("Deviate");
        ws2.PageSetup.Margins.Top = 0.2;
        ws2.PageSetup.Margins.Bottom = 0.2;
        ws2.PageSetup.Margins.Left = 0.1;
        ws2.PageSetup.Margins.Right = 0;
        ws2.PageSetup.Margins.Footer = 0;
        ws2.PageSetup.Margins.Header = 0;
        ws2.Style.Font.FontName = "Cordia New";

        nRow = 1;
        nCol = 1;

        SetTbl(ws2, "Indicator : " + sIncName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        ws2.Range(nRow, nCol, nRow, nCol + 1).Merge();
        nCol++;
        SetTbl(ws2, sIncName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        nRow++;
        nCol = 1;
        SetTbl(ws2, "Operation : " + sOprtName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        ws2.Range(nRow, nCol, nRow, nCol + 1).Merge();
        nCol++;
        SetTbl(ws2, sOprtName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        nRow++;
        nCol = 1;
        SetTbl(ws2, "Facility : " + sFacName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        ws2.Range(nRow, nCol, nRow, nCol + 1).Merge();
        nCol++;
        SetTbl(ws2, sFacName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        nRow++;
        nCol = 1;
        SetTbl(ws2, "Year : " + sYear, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        ws2.Range(nRow, nCol, nRow, nCol + 1).Merge();
        nCol++;
        SetTbl(ws2, sYear, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        nRow++;

        nCol = 1;
        SetTbl(ws2, "No.", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 10);
        nCol++;
        SetTbl(ws2, "Month", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 13);
        nCol++;
        SetTbl(ws2, "Remark", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 50);
        nCol++;
        SetTbl(ws2, "Action By", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 20);
        nCol++;
        SetTbl(ws2, "Date", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 15);
        ws2.Range(nRow, 1, nRow, nCol).Style.Fill.BackgroundColor = XLColor.FromHtml("#9cb726");
        ws2.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        ws2.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        for (int i = 0; i < lstDeviate.Count(); i++)
        {
            nRow++;
            nCol = 1;
            SetTbl(ws2, "'" + (i + 1), nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws2, lstDeviate[i].sMonth, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws2, lstDeviate[i].sRemark, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws2, lstDeviate[i].sActionBy, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws2, lstDeviate[i].sDate, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null);
            ws2.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws2.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }


        #endregion
        #region CreateEXCEL

        httpResponse.Clear();
        httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        string sName = "Input_" + sIncName + "_" + sFacName + "_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("en-US"));

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
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        var itemMaster = ((_MP_EPI_FORMS)this.Master);
        ExportData(itemMaster.Indicator + "", itemMaster.OperationType + "", itemMaster.Facility + "", itemMaster.Year);
    }

    #region Class
    public class CParam
    {
        public string sIndID { get; set; }
        public string sOprtID { get; set; }
        public string sFacID { get; set; }
        public string sYear { get; set; }
    }
    public class cDataInc
    {
        public int ProductID { get; set; }
        public string sProductName { get; set; }
        public string sUnit { get; set; }
        public int? nUnitID { get; set; }
        public string sTarget { get; set; }
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
        public string IsCheckM1 { get; set; }
        public string IsCheckM2 { get; set; }
        public string IsCheckM3 { get; set; }
        public string IsCheckM4 { get; set; }
        public string IsCheckM5 { get; set; }
        public string IsCheckM6 { get; set; }
        public string IsCheckM7 { get; set; }
        public string IsCheckM8 { get; set; }
        public string IsCheckM9 { get; set; }
        public string IsCheckM10 { get; set; }
        public string IsCheckM11 { get; set; }
        public string IsCheckM12 { get; set; }

    }
    public class CReturnData : sysGlobalClass.CResutlWebMethod
    {
        public cDataInc incData { get; set; }
        public List<cCompliance> lstCompliance { get; set; }
        public List<sysGlobalClass.T_TEPI_Workflow> lstStatus { get; set; }
        public string hdfPRMS { get; set; }
    }
    public class cCompliance : TCompliance
    {
        public string sIssueDate { get; set; }
        public int? nMonth { get; set; }
        public bool IsNew { get; set; }
        public bool IsDel { get; set; }
        public bool IsSubmited { get; set; }
        public bool IsShow { get; set; }
        public List<sysGlobalClass.FuncFileUpload.ItemData> lstFile { get; set; }
    }
    public class cDataSave
    {
        public int nIndicatorID { get; set; }
        public int nOperationID { get; set; }
        public int nFacilityID { get; set; }
        public string sYear { get; set; }
        public cDataInc incData { get; set; }
        public List<cCompliance> lstCompliance { get; set; }
        public List<int> lstMonthSubmit { get; set; }
        public int nStatus { get; set; }
        public string sRemarkRequestEdit { get; set; }
    }
    public class cReport : cCompliance
    {
        public string sProductName { get; set; }
    }
    #endregion
}