using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class epi_input_spill : System.Web.UI.Page
{
    private const string sFolderInSharePahtTemp = "UploadFiles/Spill/Temp/";
    private const string sFolderInPathSave = "UploadFiles/Spill/File/{0}/";
    private const int nIndicator = 9;
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
                setCBL();
                string sQueryStrIndID = Request.QueryString["in"];
                if (!string.IsNullOrEmpty(sQueryStrIndID))
                {
                    hdfIndID.Value = STCrypt.Encrypt(sQueryStrIndID);

                    ((_MP_EPI_FORMS)this.Master).hdfPRMS = SystemFunction.GetPermissionMenu(13) + "";
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
    public void setCBL()
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstDataType = db.TData_Type.Where(w => w.cActive == "Y" && w.IndicatorID == nIndicator).Select(s => new
        {
            Value = s.nID,
            Text = s.sName,
            Type = s.sType,
            Order = s.nOrder,
        }).ToList();


        var querySPILLKEY = lstDataType.Where(w => w.Type == "SPILLKEY").OrderBy(o => o.Order).ToList();
        ddlCauseOfSPill.DataSource = querySPILLKEY;
        ddlCauseOfSPill.DataValueField = "Value";
        ddlCauseOfSPill.DataTextField = "Text";
        ddlCauseOfSPill.DataBind();
        ddlCauseOfSPill.Items.Insert(0, new ListItem("- Select -", ""));

        var querySPILLTYPE = lstDataType.Where(w => w.Type == "SPILLTYPE").OrderBy(o => o.Order).ToList();
        ddlSpillType.DataSource = querySPILLTYPE;
        ddlSpillType.DataValueField = "Value";
        ddlSpillType.DataTextField = "Text";
        ddlSpillType.DataBind();
        ddlSpillType.Items.Insert(0, new ListItem("- Select -", ""));

        ddlSpillOf.Items.Insert(0, new ListItem("- Select -", ""));

        var querySPILLTO = lstDataType.Where(w => w.Type == "SPILL2").OrderBy(o => o.Order).ToList();
        ddlSpillTo.DataSource = querySPILLTO;
        ddlSpillTo.DataValueField = "Value";
        ddlSpillTo.DataTextField = "Text";
        ddlSpillTo.DataBind();
        ddlSpillTo.Items.Insert(0, new ListItem("- Select -", ""));

        var querySPILLBY = lstDataType.Where(w => w.Type == "SPILLBY").OrderBy(o => o.Order).ToList();
        ddlSpillBy.DataSource = querySPILLBY;
        ddlSpillBy.DataValueField = "Value";
        ddlSpillBy.DataTextField = "Text";
        ddlSpillBy.DataBind();
        ddlSpillBy.Items.Insert(0, new ListItem("- Select -", ""));

        var queryVOLUME = db.mTUnit.Where(w => (w.UnitID == 63 || w.UnitID == 64 || w.UnitID == 65) && w.cDel != "Y" && w.cActive == "Y").Select(s => new
        {
            Value = s.UnitID,
            Text = s.UnitName,
        }).ToList().Concat(db.mTUnit.Where(w => w.UnitID == 2 && w.cDel != "Y" && w.cActive == "Y").Select(s => new { Value = s.UnitID, Text = s.UnitName }).ToList());
        ddlVolume.DataSource = queryVOLUME;
        ddlVolume.DataValueField = "Value";
        ddlVolume.DataTextField = "Text";
        ddlVolume.DataBind();

        var querySPILLTIER = lstDataType.Where(w => w.Type == "SPILLTIER").OrderBy(o => o.Order).ToList();
        ddlLOPCTier.DataSource = querySPILLTIER;
        ddlLOPCTier.DataValueField = "Value";
        ddlLOPCTier.DataTextField = "Text";
        ddlLOPCTier.DataBind();
        ddlLOPCTier.Items.Insert(0, new ListItem("- Select -", ""));
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static CReturnData LoadData(CParam param)
    {
        CReturnData result = new CReturnData();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        if (!UserAcc.UserExpired())
        {
            result.lstIncData = new List<TData_Spill_Product>();
            result.lstSpill = new List<TData_Spill>();
            result.lstStatus = new List<sysGlobalClass.T_TEPI_Workflow>();
            int nIndID = SystemFunction.GetIntNullToZero(param.sIndID);
            int nOprtID = SystemFunction.GetIntNullToZero(param.sOprtID);
            int nFacID = SystemFunction.GetIntNullToZero(param.sFacID);
            result.hdfPRMS = SystemFunction.GetPermission_EPI_FROMS(nIndID, nFacID) + "";
            string sYear = param.sYear;
            Func<string, string> GetValueNameMasterData = (sIDMaster) =>
            {
                string sMasterName = "";
                int nIDMaster = int.Parse(sIDMaster);
                var sDataMasterType = db.TData_Type.FirstOrDefault(w => w.cActive == "Y" && w.IndicatorID == nIndicator && w.nID == nIDMaster);
                if (sDataMasterType != null)
                {
                    sMasterName = sDataMasterType.sName;
                }
                return sMasterName;
            };
            Func<string, string> GetValueNameUnit = (sID) =>
            {
                string sUnitName = "";
                int nID = int.Parse(sID);
                var sDataUnit = db.mTUnit.FirstOrDefault(w => w.cActive == "Y" && w.UnitID == nID);
                if (sDataUnit != null)
                {
                    sUnitName = sDataUnit.UnitName;
                }
                return sUnitName;
            };
            db.mTProductIndicator.Where(w => w.IDIndicator == nIndicator).ToList().ForEach(f =>
            {
                TData_Spill_Product SpillPRD = new TData_Spill_Product();
                SpillPRD.ProductID = f.ProductID;
                SpillPRD.sUnit = f.sUnit;
                SpillPRD.ProductName = f.ProductName;
                SpillPRD.UnitID = f.ProductID == 209 ? 66 : (f.ProductID == 210 ? 21 : 1);
                SpillPRD.Target = "";
                result.lstIncData.Add(SpillPRD);
            });
            #region EPI_FORM
            var itemEPI_FORM = db.TEPI_Forms.FirstOrDefault(w => w.sYear == sYear && w.IDIndicator == nIndID && w.OperationTypeID == nOprtID && w.FacilityID == nFacID);
            if (itemEPI_FORM != null)
            {
                var lstDataSpillPRD = db.TSpill_Product.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
                #region Data Indicator
                foreach (var item in result.lstIncData)
                {
                    var itemSpillPRD = lstDataSpillPRD.FirstOrDefault(w => w.ProductID == item.ProductID);
                    if (itemSpillPRD != null)
                    {
                        var itemUnit = db.mTUnit.FirstOrDefault(w => w.UnitID == item.UnitID);
                        item.UnitID = item.UnitID;
                        item.sUnit = itemUnit != null ? itemUnit.UnitName : item.sUnit;
                        item.Target = itemSpillPRD.Target;
                        item.M1 = itemSpillPRD.M1 ?? "";
                        item.M2 = itemSpillPRD.M2 ?? "";
                        item.M3 = itemSpillPRD.M3 ?? "";
                        item.M4 = itemSpillPRD.M4 ?? "";
                        item.M5 = itemSpillPRD.M5 ?? "";
                        item.M6 = itemSpillPRD.M6 ?? "";
                        item.M7 = itemSpillPRD.M7 ?? "";
                        item.M8 = itemSpillPRD.M8 ?? "";
                        item.M9 = itemSpillPRD.M9 ?? "";
                        item.M10 = itemSpillPRD.M10 ?? "";
                        item.M11 = itemSpillPRD.M11 ?? "";
                        item.M12 = itemSpillPRD.M12 ?? "";
                        item.IsCheckM1 = itemSpillPRD.IsCheckM1 ?? "N";
                        item.IsCheckM2 = itemSpillPRD.IsCheckM2 ?? "N";
                        item.IsCheckM3 = itemSpillPRD.IsCheckM3 ?? "N";
                        item.IsCheckM4 = itemSpillPRD.IsCheckM4 ?? "N";
                        item.IsCheckM5 = itemSpillPRD.IsCheckM5 ?? "N";
                        item.IsCheckM6 = itemSpillPRD.IsCheckM6 ?? "N";
                        item.IsCheckM7 = itemSpillPRD.IsCheckM7 ?? "N";
                        item.IsCheckM8 = itemSpillPRD.IsCheckM8 ?? "N";
                        item.IsCheckM9 = itemSpillPRD.IsCheckM9 ?? "N";
                        item.IsCheckM10 = itemSpillPRD.IsCheckM10 ?? "N";
                        item.IsCheckM11 = itemSpillPRD.IsCheckM11 ?? "N";
                        item.IsCheckM12 = itemSpillPRD.IsCheckM12 ?? "N";
                    }
                    else
                    {
                        var itemUnit = db.mTUnit.FirstOrDefault(w => w.UnitID == item.UnitID);
                        item.UnitID = item.UnitID;
                        item.sUnit = itemUnit != null ? itemUnit.UnitName : item.sUnit;
                        item.Target = "";
                        item.M1 = "";
                        item.M2 = "";
                        item.M3 = "";
                        item.M4 = "";
                        item.M5 = "";
                        item.M6 = "";
                        item.M7 = "";
                        item.M8 = "";
                        item.M9 = "";
                        item.M10 = "";
                        item.M11 = "";
                        item.M12 = "";
                        item.IsCheckM1 = "N";
                        item.IsCheckM2 = "N";
                        item.IsCheckM3 = "N";
                        item.IsCheckM4 = "N";
                        item.IsCheckM5 = "N";
                        item.IsCheckM6 = "N";
                        item.IsCheckM7 = "N";
                        item.IsCheckM8 = "N";
                        item.IsCheckM9 = "N";
                        item.IsCheckM10 = "N";
                        item.IsCheckM11 = "N";
                        item.IsCheckM12 = "N";
                    }
                }
                #endregion

                #region Data Spill
                var lstDataTSpill = db.TSpill.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
                result.lstSpill = lstDataTSpill.Select(s => new TData_Spill
                {
                    nSpillID = s.nSpillID,
                    PrimaryReasonID = s.PrimaryReasonID.HasValue ? s.PrimaryReasonID.Value : 0,
                    sPrimaryReason = GetValueNameMasterData(s.PrimaryReasonID.Value + ""),
                    sOtherPrimary = s.sOtherPrimary,
                    SpillType = s.SpillType == "HC" ? "21" : s.SpillType == "NHC" ? "22" : "",
                    sSpillTypeName = GetValueNameMasterData(s.SpillType == "HC" ? "21" : s.SpillType == "NHC" ? "22" : "0"),
                    SpillOfID = s.SpillOfID.HasValue ? s.SpillOfID.Value : 0,
                    sSpillOfName = GetValueNameMasterData(s.SpillOfID + ""),
                    sOtherSpillOf = s.sOtherSpillOf,
                    Volume = s.Volume,
                    UnitVolumeID = s.UnitVolumeID.HasValue ? s.UnitVolumeID.Value : 0,
                    Density = s.Density ?? "",
                    sUnitName = GetValueNameUnit(s.UnitVolumeID + ""),
                    SpillToID = s.SpillToID.HasValue ? s.SpillToID.Value : 0,
                    sSpillToName = GetValueNameMasterData(s.SpillToID + ""),
                    sOtherSpillTo = s.sOtherSpillTo,
                    SpillByID = s.SpillByID.HasValue ? s.SpillByID.Value : 0,
                    sSpillByName = GetValueNameMasterData(s.SpillByID + ""),
                    sOtherSpillBy = s.sOtherSpillBy ?? "",
                    SpillDate = s.SpillDate,
                    sDescription = s.sDescription,
                    IncidentDescription = s.IncidentDescription,
                    RecoveryAction = s.RecoveryAction,
                    sIsSensitiveArea = s.sIsSensitiveArea,
                    nMonth = s.SpillDate.HasValue ? s.SpillDate.Value.Month : 0,
                    sIncidenceNo = s.sIncidenceNo ?? "",
                    sSubstanceName = s.sSubstanceName ?? "",
                    LOPCTierID = s.LOPCTierID.HasValue ? s.LOPCTierID.Value : 0,
                    IsDel = false,
                    IsNew = false,
                    IsSubmited = true,
                    IsShow = true,
                }).ToList();
                result.lstSpill.ForEach(f =>
                {
                    f.sSpillDate = f.SpillDate.HasValue ? f.SpillDate.Value.ToString("dd/MM/yyyy") : "";
                    f.SpillDate = null;
                });
                var lstFileSpill = db.TSpill_File.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
                foreach (var itemSpill in result.lstSpill)
                {

                    itemSpill.lstFile = new List<sysGlobalClass.FuncFileUpload.ItemData>();
                    itemSpill.lstFile = lstFileSpill.Where(w => w.nSpillID == itemSpill.nSpillID).Select(s => new sysGlobalClass.FuncFileUpload.ItemData
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
                foreach (var item in result.lstIncData)
                {
                    item.Target = "";
                    item.M1 = "";
                    item.M2 = "";
                    item.M3 = "";
                    item.M4 = "";
                    item.M5 = "";
                    item.M6 = "";
                    item.M7 = "";
                    item.M8 = "";
                    item.M9 = "";
                    item.M10 = "";
                    item.M11 = "";
                    item.M12 = "";
                    item.IsCheckM1 = "N";
                    item.IsCheckM2 = "N";
                    item.IsCheckM3 = "N";
                    item.IsCheckM4 = "N";
                    item.IsCheckM5 = "N";
                    item.IsCheckM6 = "N";
                    item.IsCheckM7 = "N";
                    item.IsCheckM8 = "N";
                    item.IsCheckM9 = "N";
                    item.IsCheckM10 = "N";
                    item.IsCheckM11 = "N";
                    item.IsCheckM12 = "N";
                }
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
    public static List<ListItem> ddlChange(int ID)
    {
        List<ListItem> result = new List<ListItem>();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstDataType = db.TData_Type.Where(w => w.cActive == "Y" && w.nReferenceID == ID).Select(s => new ListItem
        {
            Value = s.nID + "",
            Text = s.sName,
        }).ToList();
        return result = lstDataType;
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveToDB(cDataSave arrData)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        Func<List<string>, bool, string> Sum = (lstData, IsDec) =>
        {
            string sReusltData = "";
            int Total = 0;
            decimal TotalDecimal = 0;
            foreach (var item in lstData)
            {
                if (item != null)
                {
                    if (item.Trim() != "" && item.ToLower().Trim() != "n/a")
                    {
                        if (IsDec)
                        {
                            TotalDecimal += decimal.Parse(item);
                        }
                        else
                        {
                            Total += int.Parse(item);
                        }
                    }
                }
            }
            sReusltData = IsDec ? Total + "" : TotalDecimal + "";
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
                    db.TSpill_Product.RemoveRange(db.TSpill_Product.Where(w => w.FormID == FORM_ID));
                    db.TSpill.RemoveRange(db.TSpill.Where(w => w.FormID == FORM_ID));
                    db.SaveChanges();
                }

                #region TSpill_Product
                foreach (var incData in arrData.incData)
                {
                    TSpill_Product dataSpillPrd = new TSpill_Product();
                    dataSpillPrd.FormID = FORM_ID;
                    dataSpillPrd.ProductID = incData.ProductID;
                    dataSpillPrd.UnitID = incData.UnitID;
                    dataSpillPrd.Target = incData.Target;
                    dataSpillPrd.M1 = incData.M1;
                    dataSpillPrd.M2 = incData.M2;
                    dataSpillPrd.M3 = incData.M3;
                    dataSpillPrd.M4 = incData.M4;
                    dataSpillPrd.M5 = incData.M5;
                    dataSpillPrd.M6 = incData.M6;
                    dataSpillPrd.M7 = incData.M7;
                    dataSpillPrd.M8 = incData.M8;
                    dataSpillPrd.M9 = incData.M9;
                    dataSpillPrd.M10 = incData.M10;
                    dataSpillPrd.M11 = incData.M11;
                    dataSpillPrd.M12 = incData.M12;

                    dataSpillPrd.IsCheckM1 = incData.IsCheckM1;
                    dataSpillPrd.IsCheckM2 = incData.IsCheckM2;
                    dataSpillPrd.IsCheckM3 = incData.IsCheckM3;
                    dataSpillPrd.IsCheckM4 = incData.IsCheckM4;
                    dataSpillPrd.IsCheckM5 = incData.IsCheckM5;
                    dataSpillPrd.IsCheckM6 = incData.IsCheckM6;
                    dataSpillPrd.IsCheckM7 = incData.IsCheckM7;
                    dataSpillPrd.IsCheckM8 = incData.IsCheckM8;
                    dataSpillPrd.IsCheckM9 = incData.IsCheckM9;
                    dataSpillPrd.IsCheckM10 = incData.IsCheckM10;
                    dataSpillPrd.IsCheckM11 = incData.IsCheckM11;
                    dataSpillPrd.IsCheckM12 = incData.IsCheckM12;

                    List<string> lstForSum = new List<string> { incData.M1, incData.M2, incData.M3, incData.M4, incData.M5, 
                    incData.M6, incData.M7, incData.M8, incData.M9, incData.M10, incData.M11, incData.M12 };
                    dataSpillPrd.nTotal = Sum(lstForSum, incData.ProductID == 209 ? false : true);
                    db.TSpill_Product.Add(dataSpillPrd);
                }
                db.SaveChanges();
                #endregion

                #region TSpill
                int nSpill = 1;
                arrData.lstSpill.ForEach(f =>
                {
                    if (f.IsShow && f.IsSubmited)
                    {
                        TSpill data = new TSpill();
                        data.FormID = FORM_ID;
                        data.nSpillID = f.nSpillID;
                        data.PrimaryReasonID = f.PrimaryReasonID;
                        data.sOtherPrimary = f.sOtherPrimary;
                        data.SpillType = f.SpillType == "21" ? "HC" : f.SpillType == "22" ? "NHC" : "";
                        data.SpillOfID = f.SpillOfID;
                        data.sOtherSpillOf = f.sOtherSpillOf;
                        data.Volume = f.Volume;
                        data.UnitVolumeID = f.UnitVolumeID;
                        data.SpillToID = f.SpillToID;
                        data.sOtherSpillTo = f.sOtherSpillTo;
                        data.SpillByID = f.SpillByID;
                        data.SpillDate = DateTime.ParseExact(f.sSpillDate, "dd/MM/yyyy", null); ;
                        data.sDescription = f.sDescription;
                        data.IncidentDescription = f.IncidentDescription;
                        data.RecoveryAction = f.RecoveryAction;
                        data.sIsSensitiveArea = f.sIsSensitiveArea;
                        data.sIncidenceNo = f.sIncidenceNo;
                        data.sSubstanceName = f.sSubstanceName;
                        data.LOPCTierID = f.LOPCTierID;
                        data.Density = f.Density;
                        data.sOtherSpillBy = f.sOtherSpillBy;
                        data.dAdd = DateTime.Now;
                        data.AddBy = UserAcc.GetObjUser().nUserID;
                        db.TSpill.Add(data);
                        db.SaveChanges();

                        #region TSpill_File
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
                                    var query = db.TSpill_File.FirstOrDefault(w => w.FormID == FORM_ID && w.nSpillID == f.nSpillID && w.sSysFileName == qf.SaveToFileName);
                                    if (query != null)
                                    {
                                        new SystemFunction().DeleteFileInServer(query.sPath, query.sSysFileName);
                                        // new SystemFunction2().DeleteFileInServer(query.sSysPath, query.sSysFileName);
                                        db.TSpill_File.Remove(query);
                                        ///db.TAuditPlan_AttachFile.Remove(query);
                                    }
                                }
                                db.SaveChanges();
                            }
                            //Update Description
                            f.lstFile.Where(w => w.IsNewFile == false && w.sDelete == "N").ToList().ForEach(f2U =>
                            {
                                var data2Update = db.TSpill_File.FirstOrDefault(w => w.FormID == FORM_ID && w.nSpillID == f.nSpillID && w.nFileID == f2U.ID);
                                if (data2Update != null)
                                {
                                    data2Update.sDescription = f2U.sDescription;
                                }
                            });
                            //Save New File Only
                            var lstSave = f.lstFile.Where(w => w.IsNewFile == true && w.sDelete == "N").ToList();
                            if (lstSave.Any())
                            {
                                int nFileID = db.TSpill_File.Where(w => w.FormID == FORM_ID && w.nSpillID == f.nSpillID).Any() ? db.TSpill_File.Where(w => w.FormID == FORM_ID && w.nSpillID == f.nSpillID).Max(m => m.nFileID) + 1 : 1;

                                foreach (var s in lstSave)
                                {
                                    string sSystemFileName = FORM_ID + "_" + f.nSpillID + "_" + nFileID + "-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "." + SystemFunction.GetFileNameFromFileupload(s.SaveToFileName, ""); //+ "." + SystemFunction.GetFileNameFromFileupload(s.SaveToFileName, "") SystemFunction2.GetFileType(item.SaveToFileName);
                                    SystemFunction.UpFile2Server(s.SaveToPath, sPathSave, s.SaveToFileName, sSystemFileName);

                                    // SystemFunction2.UpFile2Server(item.SaveToPath, sPathSave, item.SaveToFileName, sSystemFileName);
                                    TSpill_File t = new TSpill_File();
                                    t.FormID = FORM_ID;
                                    t.nSpillID = f.nSpillID;
                                    t.nFileID = nFileID;
                                    t.sSysFileName = sSystemFileName;
                                    t.sFileName = s.FileName;
                                    t.sPath = sPathSave;
                                    t.sDescription = s.sDescription;
                                    db.TSpill_File.Add(t);
                                    nFileID++;
                                }
                            }
                            db.SaveChanges();
                        }
                        #endregion

                        nSpill++;
                    }
                });
                #endregion

                new EPIFunc().RecalculateSpill(arrData.nOperationID, arrData.nFacilityID, arrData.sYear);
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
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        var itemMaster = ((_MP_EPI_FORMS)this.Master);
        ExportData(itemMaster.Indicator + "", itemMaster.OperationType + "", itemMaster.Facility + "", itemMaster.Year);
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
        List<TData_Spill> lstProduct = new List<TData_Spill>();
        List<TSpill> lstDataTSpill = new List<TSpill>();
        Func<string, string> GetValueNameMasterData = (sIDMaster) =>
        {
            string sMasterName = "";
            int nIDMaster = int.Parse(sIDMaster);
            var sDataMasterType = db.TData_Type.FirstOrDefault(w => w.cActive == "Y" && w.IndicatorID == nIndicator && w.nID == nIDMaster);
            if (sDataMasterType != null)
            {
                sMasterName = sDataMasterType.sName;
            }
            return sMasterName;
        };
        Func<string, string> GetValueNameUnit = (sID) =>
        {
            string sUnitName = "";
            int nID = int.Parse(sID);
            var sDataUnit = db.mTUnit.FirstOrDefault(w => w.cActive == "Y" && w.UnitID == nID);
            if (sDataUnit != null)
            {
                sUnitName = sDataUnit.UnitName;
            }
            return sUnitName;
        };
        string[] arrShortMonth = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        int nIndID = SystemFunction.GetIntNullToZero(sIncID);
        int nOprtID = SystemFunction.GetIntNullToZero(sOprtID);
        int nFacID = SystemFunction.GetIntNullToZero(sFacID);
        string sIncName = db.mTIndicator.Any(w => w.ID == nIndID) ? db.mTIndicator.FirstOrDefault(w => w.ID == nIndID).Indicator : "";
        string sOprtName = db.mOperationType.Any(w => w.ID == nOprtID) ? db.mOperationType.FirstOrDefault(w => w.ID == nOprtID).Name : "";
        string sFacName = db.mTFacility.Any(w => w.ID == nFacID) ? db.mTFacility.FirstOrDefault(w => w.ID == nFacID).Name : "";
        List<int> lstOtherID = new List<int>() { 20, 27, 30, 75, 93 };
        bool IsNew = true;
        var itemEPI_FORM = db.TEPI_Forms.FirstOrDefault(w => w.sYear == sYear && w.IDIndicator == nIndID && w.OperationTypeID == nOprtID && w.FacilityID == nFacID);
        int EPI_FORMID = 0;
        if (itemEPI_FORM != null)
        {
            IsNew = false;
            EPI_FORMID = itemEPI_FORM.FormID;
            lstDataTSpill = db.TSpill.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
        }
        string sProductName = db.mTProductIndicator.FirstOrDefault(w => w.IDIndicator == 9) != null ? db.mTProductIndicator.FirstOrDefault(w => w.IDIndicator == 9).ProductName : "";
        lstProduct = lstDataTSpill.OrderBy(o => o.SpillDate.Value.Month).Select(s => new TData_Spill
        {
            nSpillID = s.nSpillID,
            PrimaryReasonID = s.PrimaryReasonID.HasValue ? s.PrimaryReasonID.Value : 0,
            sPrimaryReason = GetValueNameMasterData(s.PrimaryReasonID.Value + ""),
            sOtherPrimary = s.sOtherPrimary,
            SpillType = s.SpillType == "HC" ? "21" : s.SpillType == "NHC" ? "22" : "",
            sSpillTypeName = GetValueNameMasterData(s.SpillType == "HC" ? "21" : s.SpillType == "NHC" ? "22" : "0"),
            SpillOfID = s.SpillOfID.HasValue ? s.SpillOfID.Value : 0,
            sSpillOfName = GetValueNameMasterData(s.SpillOfID + ""),
            sOtherSpillOf = s.sOtherSpillOf,
            Volume = s.UnitVolumeID == 63 ? SystemFunction.GetDecimalNull(s.Volume) + "" //เป็น Liter อยู่แล้ว
                     : s.UnitVolumeID == 64 ? SystemFunction.ConvertBarrelToLiter(s.Volume) + "" //Convert Barrel To Liter
                     : s.UnitVolumeID == 65 ? SystemFunction.ConvertM3ToLiter(s.Volume) + ""
                     : s.UnitVolumeID == 2 ? (SystemFunction.GetDecimalNull(s.Volume) * SystemFunction.GetDecimalNull(s.Density)) + "" : "", //Convert M3 To Liter
            UnitVolumeID = s.UnitVolumeID.HasValue ? s.UnitVolumeID.Value : 0,
            sUnitName = "Liter",//GetValueNameUnit(s.UnitVolumeID + ""),
            SpillToID = s.SpillToID.HasValue ? s.SpillToID.Value : 0,
            sSpillToName = GetValueNameMasterData(s.SpillToID + ""),
            sOtherSpillTo = s.sOtherSpillTo,
            SpillByID = s.SpillByID.HasValue ? s.SpillByID.Value : 0,
            sSpillByName = GetValueNameMasterData(s.SpillByID + ""),
            sOtherSpillBy = s.sOtherSpillBy,
            SpillDate = s.SpillDate,
            sDescription = s.sDescription,
            IncidentDescription = s.IncidentDescription,
            RecoveryAction = s.RecoveryAction,
            sIsSensitiveArea = s.sIsSensitiveArea,
            nMonth = s.SpillDate.HasValue ? s.SpillDate.Value.Month : 0,
            IsDel = false,
            IsNew = false,
            IsSubmited = true,
            IsShow = true,
        }).ToList();
        lstProduct.ForEach(f =>
        {
            f.sSpillDate = f.SpillDate.HasValue ? f.SpillDate.Value.ToString("dd/MM/yyyy") : "";
            f.SpillDate = null;
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
        SetTbl(ws1, "Cause of Spill", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17);
        nCol++;
        SetTbl(ws1, "Spill Type", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17);
        nCol++;
        SetTbl(ws1, "Spill of", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 12);
        nCol++;
        SetTbl(ws1, "Volume", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 15);
        nCol++;
        SetTbl(ws1, "Unit", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 15);
        nCol++;
        SetTbl(ws1, "Spill to", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17);
        nCol++;
        SetTbl(ws1, "Spill by", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17);
        nCol++;
        SetTbl(ws1, "SensitiveArea", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 12);
        nCol++;
        SetTbl(ws1, "Data of Spill", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 15);
        nCol++;
        SetTbl(ws1, "Description", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 35);
        nCol++;
        SetTbl(ws1, "Intermediate Action", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 35);
        nCol++;
        SetTbl(ws1, "Recovery Action", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 35);
        nCol++;
        SetTbl(ws1, "Case of Spill(Other)", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 35);
        ws1.Range(nRow, 1, nRow, nCol).Style.Fill.BackgroundColor = XLColor.FromHtml("#9cb726");
        ws1.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        ws1.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        foreach (var item in lstProduct)
        {
            nRow++;
            nCol = 1;
            SetTbl(ws1, "'" + sProductName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + arrShortMonth[item.nMonth.Value - 1], nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + item.sPrimaryReason, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + item.sSpillTypeName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + (!lstOtherID.Contains(item.SpillOfID) ? item.sSpillOfName : item.sOtherSpillOf), nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, item.Volume, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            //SetTbl(ws1, "'" + item.sUnitName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            SetTbl(ws1, "'" + item.sUnitName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + (!lstOtherID.Contains(item.SpillToID) ? item.sSpillToName : item.sOtherSpillTo), nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + (!lstOtherID.Contains(item.SpillByID) ? item.sSpillByName : item.sOtherSpillBy), nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + (item.sIsSensitiveArea == "Y" ? "YES" : "NO"), nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + item.sSpillDate, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + item.sDescription, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + item.IncidentDescription, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + item.RecoveryAction, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + (item.sOtherPrimary ?? ""), nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
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

    #region Class
    public class CParam
    {
        public string sIndID { get; set; }
        public string sOprtID { get; set; }
        public string sFacID { get; set; }
        public string sYear { get; set; }
    }
    public class CReturnData : sysGlobalClass.CResutlWebMethod
    {
        public List<TData_Spill_Product> lstIncData { get; set; }
        public List<TData_Spill> lstSpill { get; set; }
        public List<sysGlobalClass.T_TEPI_Workflow> lstStatus { get; set; }
        public string hdfPRMS { get; set; }
    }
    public class TData_Spill_Product
    {
        public int ProductID { get; set; }
        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }

        public int FormID { get; set; }
        public int UnitID { get; set; }
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
        public string Target { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string nTotal { get; set; }
        public decimal? Factor { get; set; }

        public int? nStatusIDQ1 { get; set; }
        public int? nStatusIDQ2 { get; set; }
        public int? nStatusIDQ3 { get; set; }
        public int? nStatusIDQ4 { get; set; }

        public string IsEnableInputQ1 { get; set; }
        public string IsEnableInputQ2 { get; set; }
        public string IsEnableInputQ3 { get; set; }
        public string IsEnableInputQ4 { get; set; }

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
    public class TData_Spill
    {
        public int nSpillID { get; set; }//*
        public int PrimaryReasonID { get; set; } //*Primary reason for loss of containment แก้ชื่อเป็น Cause of Spill
        public string sPrimaryReason { get; set; }
        public string sOtherPrimary { get; set; }
        public string SpillType { get; set; }//*
        public string sSpillTypeName { get; set; }//HC = Hydrocarbon,NHC = Non-hydrocarbon,
        public int SpillOfID { get; set; }//*
        public string sSpillOfName { get; set; }
        public string sOtherSpillOf { get; set; }
        public string Volume { get; set; }//*
        public int UnitVolumeID { get; set; }//*
        public string Density { get; set; }
        public string sUnitName { get; set; }
        public int SpillToID { get; set; }//*
        public string sOtherSpillTo { get; set; }
        public string sSpillToName { get; set; }
        public int SpillByID { get; set; }//*
        public string sSpillByName { get; set; }
        public string sOtherSpillBy { get; set; }
        public int? Signification1ID { get; set; }//*ถ้าไม่เท่ากับ 0 หรือ Null จะนำค่าไปคำนวณ (ถ้าระบุว่า Spill to environment จะนำมาคำนวนทุกกรณี)
        public int? Signification2ID { get; set; }//*
        public DateTime? SpillDate { get; set; }//*
        public string sSpillDate { get; set; }
        public string sDescription { get; set; }
        public string IncidentDescription { get; set; }//>>แก้ไขชื่อคอลัมน์หน้าเว็บเป็น Intermediate Action >> 11/3/2558 by PTTEP
        public string RecoveryAction { get; set; }
        public string sIsSensitiveArea { get; set; }//Y = นำไปคำนวณเป็น Significant Spill
        public string IsPrmsEdit { get; set; }
        public string IsAddFile { get; set; }
        public string sIncidenceNo { get; set; }
        public string sSubstanceName { get; set; }
        public int? LOPCTierID { get; set; }

        //เพื่อให้ไปคำนวณ
        public decimal? nSpillVolume { get; set; }
        public List<sysGlobalClass.FuncFileUpload.ItemData> lstFile { get; set; }
        public int? nMonth { get; set; }
        public bool IsNew { get; set; }
        public bool IsDel { get; set; }
        public bool IsSubmited { get; set; }
        public bool IsShow { get; set; }
    }
    public class cDataSave
    {
        public int nIndicatorID { get; set; }
        public int nOperationID { get; set; }
        public int nFacilityID { get; set; }
        public string sYear { get; set; }
        public List<TData_Spill_Product> incData { get; set; }
        public List<TData_Spill> lstSpill { get; set; }
        public List<int> lstMonthSubmit { get; set; }
        public int nStatus { get; set; }
        public string sRemarkRequestEdit { get; set; }
    }
    #endregion
}