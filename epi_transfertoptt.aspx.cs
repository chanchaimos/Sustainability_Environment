using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sysExtension;
using System.Data;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Configuration;

public partial class epi_transfertoptt : System.Web.UI.Page
{
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnLoad(myFunc);
    }

    PTTGC_EPIEntities db = new PTTGC_EPIEntities();
    private static int[] arrStatusRecalcuate = new int[] { 29, 33 };

    #region Web Service Property
    private const string sNAFormat = "N/A";
    private static string PTTEPIServicePTTGCComCode = ConfigurationManager.AppSettings["PTTEPIServicePTTGCComCode"] + "";
    private const string UserEPIService = "EPIUser";
    private const string PasswordEPIService = "EPIUser@1234";
    public class PTTEPIServiceInidcatorCode
    {
        public const string Complaint = "001";
        public const string Compliance = "002";
        public const string Effluent = "003";
        public const string Emission = "004";
        public const string IntensityDenominator = "006";
        public const string Material = "008";
        public const string Spill = "009";
        public const string Waste = "010";
        public const string Water = "011";
    }

    public class DataType
    {
        public class Indicator
        {
            public const int IntensityDenonimator = 6;
            public const int Water = 11;
            public const int Material = 8;
            public const int Waste = 10;
            public const int Emission = 4;
            public const int Effluent = 3;
            public const int Spill = 9;
            public const int Compliance = 2;
            public const int Complaint = 1;
        }
    }
    #endregion

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
                if (UserAcc.GetObjUser().nRoleID == 4 || UserAcc.GetObjUser().nRoleID == 1)
                {
                    string stryear = Request.QueryString["stryear"] + "";
                    string strfacid = Request.QueryString["strfacid"] + "";
                    string strindid = Request.QueryString["strindid"] + "";
                    string strquarter = Request.QueryString["strq"] + "";
                    string strmode = Request.QueryString["strmode"] + "";
                    if (!string.IsNullOrEmpty(stryear) && !string.IsNullOrEmpty(strfacid) && !string.IsNullOrEmpty(strindid) && !string.IsNullOrEmpty(strquarter) && !string.IsNullOrEmpty(strmode))
                    {
                        hdfYearEncrypt.Value = STCrypt.Decrypt(stryear);
                        hdfFacIDEncrypt.Value = STCrypt.Decrypt(strfacid);
                        hdfIndIDEncrypt.Value = STCrypt.Decrypt(strindid);
                        hdfQuareterEncrypt.Value = STCrypt.Decrypt(strquarter);

                        SetData(hdfIndIDEncrypt.Value.toIntNullToZero(), hdfFacIDEncrypt.Value.toIntNullToZero(), hdfYearEncrypt.Value.toIntNullToZero(), hdfQuareterEncrypt.Value.toIntNullToZero(), STCrypt.Decrypt(strmode));

                        hdfYearEncrypt.Value = STCrypt.Encrypt(hdfYearEncrypt.Value);
                        hdfFacIDEncrypt.Value = STCrypt.Encrypt(hdfFacIDEncrypt.Value);
                        hdfIndIDEncrypt.Value = STCrypt.Encrypt(hdfIndIDEncrypt.Value);
                        hdfQuareterEncrypt.Value = STCrypt.Encrypt(hdfQuareterEncrypt.Value);
                    }
                    else
                    {
                        SetBodyEventOnLoad(SystemFunction.DialogWarningRedirect(SystemFunction.Msg_HeadWarning, "Invalid !", "epi_transfertoptt_lst.aspx"));
                    }
                }
                else
                {
                    SetBodyEventOnLoad(SystemFunction.DialogWarningRedirect(SystemFunction.Msg_HeadWarning, "No permission !", "epi_mytask.aspx"));
                }
            }
        }
    }

    private void SetData(int nIndID, int nFacID, int nYear, int nQuarter, string sMode)
    {
        var qInd = db.mTIndicator.FirstOrDefault(w => w.ID == nIndID);
        var qFac = db.mTFacility.FirstOrDefault(w => w.ID == nFacID);
        if (qInd != null && qFac != null)
        {
            lblGroupIndicator.Text = qInd.Indicator;
            lblFacility.Text = qFac.Name;
            lblYear.Text = nYear + "";
            var qOperationtype = db.mOperationType.FirstOrDefault(w => w.ID == qFac.OperationTypeID);
            lblOperationtype.Text = qOperationtype != null ? qOperationtype.Name : "";

            int nIndex = 0;
            foreach (int item in GetAllowQuarter(nIndID, nFacID, nYear, sMode))
            {
                cklQuarter.Items.Insert(nIndex, new ListItem("Q" + item, item + ""));
                nIndex++;
            }

            foreach (ListItem item in cklQuarter.Items)
            {
                item.Attributes.Add("class", "flat-green checkbox checkbox-inline");
                if (item.Value == nQuarter + "")
                {
                    item.Selected = true;
                }
            }
        }
    }

    private List<int> GetAllowQuarter(int nIndID, int nFacID, int nYear, string sMode)
    {
        List<int> lstQuarter = new List<int>();

        var lstDataSubFac = GetDataSubFacilityAllowTransfer(nIndID, nYear);

        #region SQL Main facility
        string sqlMainFac = @"SELECT TFPTT.ID 'nPTTFacID',TFPTT.Name 'sPTTFacName',TFPTT.sMappingCodePTT
,TFGC.ID 'nGCFacID',TFGC.Name 'sGCFacName'
FROM mTFacility TFPTT
INNER JOIN mTFacility TFGC ON TFPTT.ID=TFGC.nHeaderID AND TFGC.nLevel=1 AND TFGC.cActive='Y' AND TFGC.cDel='N'
WHERE TFPTT.nLevel=0 and TFPTT.cActive='Y' and TFPTT.cDel='N'";
        #endregion
        var lstDataMainFac = db.Database.SqlQuery<TDataMainFaciltiy>(sqlMainFac).ToList();

        var lstData = (from mf in lstDataMainFac
                       from sf in lstDataSubFac.Where(w => w.nHeaderID == nFacID && w.nHeaderID == mf.nGCFacID)
                       from ind in db.mTIndicator.Where(w => w.ID == sf.IDIndicator && w.ID == nIndID)
                       from wf in db.TEPI_TransferPTT.Where(w => w.nYear == nYear && w.nFacilityID == sf.nHeaderID && w.nIndicatorID == sf.IDIndicator && w.nQuarter == sf.nQuarter).DefaultIfEmpty()
                       select new
                       {
                           nYear = nYear,
                           nGCFacID = mf.nGCFacID,
                           sGCFacName = mf.sGCFacName,
                           nQuarter = sf.nQuarter,
                           nIndicatorID = sf.IDIndicator,
                           sIndicatorName = ind.Indicator,
                           nStatus = wf != null ? wf.nStatusID : 0,
                       }).ToList();

        List<int> lstAllowStatus = new List<int>();
        if (sMode == "S")//Allow Submition
        {
            lstAllowStatus = new List<int>() { 0, 29, 33 };//Reference epi_transfertoptt_lst.aspx
            btnRequestEdit.Visible = false;
        }
        else if (sMode == "R")//Allow Request Edit
        {
            lstAllowStatus = new List<int>() { 28, 30 };//Reference epi_transfertoptt_lst.aspx
            btnSubmit.Visible = false;
        }
        else
        {
            btnRequestEdit.Visible = false;
            btnSubmit.Visible = false;
        }
        hdfModeWF.Value = sMode;
        lstQuarter = lstData.Where(w => lstAllowStatus.Contains(w.nStatus ?? 0)).GroupBy(g => g.nQuarter).Select(s => s.Key).OrderBy(o => o).ToList();

        var dataTransfer = db.TEPI_TransferPTT.Where(w => w.nYear == nYear && w.nIndicatorID == nIndID && lstQuarter.Contains(w.nQuarter) && w.nFacilityID == nFacID).ToList();
        if (dataTransfer.Any(x => arrStatusRecalcuate.Contains(x.nStatusID ?? 0)))
        {
            hdfAllowRecalcualte.Value = "Y";
            btnRecalcuate.Visible = true;
        }
        else
        {
            btnRecalcuate.Visible = false;
        }

        return lstQuarter;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static CResultGetDataPageLoad GetDataOnPageLoad(string sYear, string sFacID, string sIndID)
    {
        CResultGetDataPageLoad result = new CResultGetDataPageLoad();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            epi_transfertoptt thisPage = new epi_transfertoptt();
            int nYear = STCrypt.Decrypt(sYear).toIntNullToZero();
            int nFacID = STCrypt.Decrypt(sFacID).toIntNullToZero();
            int nIndID = STCrypt.Decrypt(sIndID).toIntNullToZero();
            List<TDataSubFac> lstSubFac = new List<TDataSubFac>();
            lstSubFac = thisPage.GetDataSubFacilityAllowTransfer(nIndID, nYear).Where(w => w.nHeaderID == nFacID).ToList();
            var qMainFac = db.mTFacility.FirstOrDefault(w => w.ID == nFacID);
            result.nOperationtypeID = qMainFac != null ? qMainFac.OperationTypeID : 0;
            switch (nIndID)
            {
                case DataType.Indicator.IntensityDenonimator:
                    {
                        var data = thisPage.GetDataIntensity(nYear, nFacID, lstSubFac);
                        result.lstDataInten = data.lstDataInten;
                        result.lstSubFac = lstSubFac;
                    }
                    break;
                case DataType.Indicator.Water:
                    {
                        var data = thisPage.GetDataWater(nYear, nFacID, lstSubFac);
                        result.lstDataWater = data.lstDataWater;
                        result.lstSubFac = lstSubFac;
                    }
                    break;
                case DataType.Indicator.Material:
                    {
                        var data = thisPage.GetDataMaterial(nYear, nFacID, lstSubFac);
                        result.lstDataMaterial = data.lstDataMaterial;
                        result.lstSubFac = lstSubFac;
                    }
                    break;
                case DataType.Indicator.Waste:
                    {
                        var data = thisPage.GetDataWaste(nYear, nFacID, lstSubFac);
                        result.lstDataWaste = data.lstDataWaste;
                        result.lstSubFac = lstSubFac;
                    }
                    break;
                case DataType.Indicator.Emission:
                    {
                        var data = thisPage.GetDataEmission(nYear, nFacID, lstSubFac);
                        result.lstDataEmissionCombusion = data.lstDataEmissionCombusion;
                        result.lstDataEmissionStack = data.lstDataEmissionStack;
                        result.lstDataEmissionVOC = data.lstDataEmissionVOC;
                        result.lstSubFac = lstSubFac;
                    }
                    break;
                case DataType.Indicator.Effluent:
                    {
                        var data = thisPage.GetDataEffluent(nYear, nFacID, lstSubFac);
                        result.lstDataEffluentSumamry = data.lstDataEffluentSumamry;
                        result.lstDataEffluentOutput = data.lstDataEffluentOutput;
                        result.lstDataEffluentPoint = data.lstDataEffluentPoint;
                        result.lstSubFac = lstSubFac;
                    }
                    break;
                case DataType.Indicator.Spill:
                    {
                        var data = thisPage.GetDataSpill(nYear, nFacID, lstSubFac);
                        result.lstDataSpillProduct = data.lstDataSpillProduct;
                        result.lstDataSpill = data.lstDataSpill;
                        result.lstSubFac = lstSubFac;
                    }
                    break;
                case DataType.Indicator.Compliance:
                    {
                        var data = thisPage.GetDataCompliance(nYear, nFacID, lstSubFac);
                        result.lstDataComplianceProduct = data.lstDataComplianceProduct;
                        result.lstDataCompliance = data.lstDataCompliance;
                        result.lstSubFac = lstSubFac;
                    }
                    break;
                case DataType.Indicator.Complaint:
                    {
                        var data = thisPage.GetDataComplaint(nYear, nFacID, lstSubFac);
                        result.lstDataComplaintProduct = data.lstDataComplaintProduct;
                        result.lstDataComplaint = data.lstDataComplaint;
                        result.lstSubFac = lstSubFac;
                    }
                    break;
            }

            result.nIndID = nIndID;
            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    private CResultGetDataPageLoad GetDataIntensity(int nYear, int nMainFacID, List<TDataSubFac> lstDataSubFac)
    {
        CResultGetDataPageLoad result = new CResultGetDataPageLoad();
        List<TDatIntesity> lstInten = new List<TDatIntesity>();
        string sYear = nYear + "";
        var lstSubFacID = lstDataSubFac.GroupBy(g => g.FacilityID).Select(s => s.Key).ToList();
        var dataDisplayType = (from f in db.mTFacility.Where(w => w.ID == nMainFacID)
                               from d in db.TIntensity_DisplayInput.Where(w => w.OperationTypeID == f.OperationTypeID)
                               select new
                               {
                                   d.DisplayType
                               }).FirstOrDefault();
        var dataEPIFrom = db.TEPI_Forms.Where(w => w.sYear == sYear && w.IDIndicator == DataType.Indicator.IntensityDenonimator && lstSubFacID.Contains(w.FacilityID ?? 0)).ToList();
        if (dataDisplayType != null)
        {
            if (dataDisplayType.DisplayType == 3)
            {
                #region Type 4 >> Chemical Transportation & Storage
                var queryAll = (from epi in dataEPIFrom
                                from d in db.TIntensityDominator.Where(w => w.FormID == epi.FormID && w.ProductID == 79).AsEnumerable()
                                from i in db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.IntensityDenonimator && w.ProductID == d.ProductID)
                                select new TDatIntesity
                                {
                                    nProductID = i.ProductID,
                                    sProductName = i.ProductName,
                                    cTotal = i.cTotal,
                                    cTotalAll = i.cTotalAll,
                                    sUnit = i.sUnit,

                                    sM1 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M1 : "",
                                    sM2 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M2 : "",
                                    sM3 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M3 : "",

                                    sM4 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M4 : "",
                                    sM5 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M5 : "",
                                    sM6 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M6 : "",

                                    sM7 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M7 : "",
                                    sM8 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M8 : "",
                                    sM9 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M9 : "",

                                    sM10 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M10 : "",
                                    sM11 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M11 : "",
                                    sM12 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M12 : "",
                                }).ToList();

                lstInten = queryAll.GroupBy(g => new { g.nProductID, g.sProductName, g.cTotalAll, g.cTotal, g.sUnit }).Select(s => new TDatIntesity
                {
                    nProductID = s.Key.nProductID,
                    sProductName = s.Key.sProductName,
                    cTotalAll = s.Key.cTotalAll,
                    cTotal = s.Key.cTotal,
                    sUnit = s.Key.sUnit,
                    sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
                    sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
                    sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
                    sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
                    sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
                    sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
                    sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
                    sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
                    sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
                    sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
                    sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
                    sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
                }).ToList();

                foreach (var item in lstInten)
                {
                    item.sTotal = SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";
                    item.sShowM1 = SystemFunction.ConvertFormatDecimal4(item.sM1);
                    item.sShowM2 = SystemFunction.ConvertFormatDecimal4(item.sM2);
                    item.sShowM3 = SystemFunction.ConvertFormatDecimal4(item.sM3);
                    item.sShowM4 = SystemFunction.ConvertFormatDecimal4(item.sM4);
                    item.sShowM5 = SystemFunction.ConvertFormatDecimal4(item.sM5);
                    item.sShowM6 = SystemFunction.ConvertFormatDecimal4(item.sM6);
                    item.sShowM7 = SystemFunction.ConvertFormatDecimal4(item.sM7);
                    item.sShowM8 = SystemFunction.ConvertFormatDecimal4(item.sM8);
                    item.sShowM9 = SystemFunction.ConvertFormatDecimal4(item.sM9);
                    item.sShowM10 = SystemFunction.ConvertFormatDecimal4(item.sM10);
                    item.sShowM11 = SystemFunction.ConvertFormatDecimal4(item.sM11);
                    item.sShowM12 = SystemFunction.ConvertFormatDecimal4(item.sM12);
                    item.sShowTotal = SystemFunction.ConvertFormatDecimal4(item.sTotal);
                }
                #endregion
            }
            else if (dataDisplayType.DisplayType == 4)
            {
                #region Type 4 >> Petrochemicals
                #region Query
                var queryAll = (from epi in dataEPIFrom
                                from d in db.TIntensity_Other.Where(w => w.FormID == epi.FormID).AsEnumerable()
                                select new
                                {
                                    epi.FacilityID,
                                    d.ProductID,
                                    d.ProductName,
                                    d.UnderProductID,
                                    sM1 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M1 : "",
                                    sM2 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M2 : "",
                                    sM3 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M3 : "",

                                    sM4 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M4 : "",
                                    sM5 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M5 : "",
                                    sM6 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M6 : "",

                                    sM7 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M7 : "",
                                    sM8 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M8 : "",
                                    sM9 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M9 : "",

                                    sM10 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M10 : "",
                                    sM11 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M11 : "",
                                    sM12 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M12 : "",
                                }).ToList();
                var qGroupSubProduct = queryAll.GroupBy(g => new { g.UnderProductID, g.ProductName }).Select(s => new
                    {
                        s.Key.UnderProductID,
                        s.Key.ProductName,
                        sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
                        sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
                        sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
                        sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
                        sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
                        sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
                        sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
                        sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
                        sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
                        sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
                        sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
                        sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
                    }).ToList();

                var qTotalProduct = qGroupSubProduct.Where(w => w.UnderProductID == 84).ToList();
                var qTotalByProduct = qGroupSubProduct.Where(w => w.UnderProductID == 85).ToList();
                #endregion

                #region Total Product and By-product
                lstInten.Add(new TDatIntesity
                    {
                        nProductID = 83,
                        sProductName = "Total Product and By-product",
                        cTotal = "Y",
                        cTotalAll = "Y",
                        sUnit = "Tonnes Product",
                        sM1 = qGroupSubProduct.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? qGroupSubProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : "",
                        sM2 = qGroupSubProduct.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? qGroupSubProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : "",
                        sM3 = qGroupSubProduct.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? qGroupSubProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : "",
                        sM4 = qGroupSubProduct.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? qGroupSubProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : "",
                        sM5 = qGroupSubProduct.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? qGroupSubProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : "",
                        sM6 = qGroupSubProduct.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? qGroupSubProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : "",
                        sM7 = qGroupSubProduct.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? qGroupSubProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : "",
                        sM8 = qGroupSubProduct.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? qGroupSubProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : "",
                        sM9 = qGroupSubProduct.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? qGroupSubProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : "",
                        sM10 = qGroupSubProduct.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? qGroupSubProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : "",
                        sM11 = qGroupSubProduct.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? qGroupSubProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : "",
                        sM12 = qGroupSubProduct.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? qGroupSubProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : "",
                    });
                #endregion

                #region Total Product
                lstInten.Add(new TDatIntesity
                {
                    nProductID = 84,
                    sProductName = "Total Product",
                    cTotal = "Y",
                    cTotalAll = "N",
                    sUnit = "Tonnes Product",
                    sM1 = qTotalProduct.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? qTotalProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : "",
                    sM2 = qTotalProduct.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? qTotalProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : "",
                    sM3 = qTotalProduct.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? qTotalProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : "",
                    sM4 = qTotalProduct.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? qTotalProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : "",
                    sM5 = qTotalProduct.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? qTotalProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : "",
                    sM6 = qTotalProduct.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? qTotalProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : "",
                    sM7 = qTotalProduct.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? qTotalProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : "",
                    sM8 = qTotalProduct.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? qTotalProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : "",
                    sM9 = qTotalProduct.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? qTotalProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : "",
                    sM10 = qTotalProduct.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? qTotalProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : "",
                    sM11 = qTotalProduct.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? qTotalProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : "",
                    sM12 = qTotalProduct.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? qTotalProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : "",
                });

                int nProductID = 85;
                foreach (var item in qTotalProduct)//Sub Product > Total Product
                {
                    lstInten.Add(new TDatIntesity
                        {
                            nProductID = nProductID,
                            sProductName = item.ProductName,
                            nHeaderID = 84,
                            cTotal = "N",
                            cTotalAll = "N",
                            sUnit = "Tonnes Product",
                            sM1 = item.sM1,
                            sM2 = item.sM2,
                            sM3 = item.sM3,
                            sM4 = item.sM4,
                            sM5 = item.sM5,
                            sM6 = item.sM6,
                            sM7 = item.sM7,
                            sM8 = item.sM8,
                            sM9 = item.sM9,
                            sM10 = item.sM10,
                            sM11 = item.sM11,
                            sM12 = item.sM12,
                        });
                    nProductID++;
                }
                #endregion

                #region Total By-product
                lstInten.Add(new TDatIntesity
                {
                    nProductID = 85,
                    sProductName = "Total By-product",
                    cTotal = "Y",
                    cTotalAll = "N",
                    sUnit = "Tonnes Product",
                    sM1 = qTotalByProduct.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? qTotalByProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : "",
                    sM2 = qTotalByProduct.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? qTotalByProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : "",
                    sM3 = qTotalByProduct.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? qTotalByProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : "",
                    sM4 = qTotalByProduct.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? qTotalByProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : "",
                    sM5 = qTotalByProduct.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? qTotalByProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : "",
                    sM6 = qTotalByProduct.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? qTotalByProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : "",
                    sM7 = qTotalByProduct.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? qTotalByProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : "",
                    sM8 = qTotalByProduct.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? qTotalByProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : "",
                    sM9 = qTotalByProduct.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? qTotalByProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : "",
                    sM10 = qTotalByProduct.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? qTotalByProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : "",
                    sM11 = qTotalByProduct.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? qTotalByProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : "",
                    sM12 = qTotalByProduct.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? qTotalByProduct.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : "",
                });

                nProductID = 86;
                foreach (var item in qTotalByProduct)//Sub Product > Total Product
                {
                    lstInten.Add(new TDatIntesity
                    {
                        nProductID = nProductID,
                        sProductName = item.ProductName,
                        nHeaderID = 85,
                        cTotal = "N",
                        cTotalAll = "N",
                        sUnit = "Tonnes Product",
                        sM1 = item.sM1,
                        sM2 = item.sM2,
                        sM3 = item.sM3,
                        sM4 = item.sM4,
                        sM5 = item.sM5,
                        sM6 = item.sM6,
                        sM7 = item.sM7,
                        sM8 = item.sM8,
                        sM9 = item.sM9,
                        sM10 = item.sM10,
                        sM11 = item.sM11,
                        sM12 = item.sM12,
                    });
                    nProductID++;
                }
                #endregion

                foreach (var item in lstInten)
                {
                    item.sTotal = SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";
                    item.sShowM1 = SystemFunction.ConvertFormatDecimal4(item.sM1);
                    item.sShowM2 = SystemFunction.ConvertFormatDecimal4(item.sM2);
                    item.sShowM3 = SystemFunction.ConvertFormatDecimal4(item.sM3);
                    item.sShowM4 = SystemFunction.ConvertFormatDecimal4(item.sM4);
                    item.sShowM5 = SystemFunction.ConvertFormatDecimal4(item.sM5);
                    item.sShowM6 = SystemFunction.ConvertFormatDecimal4(item.sM6);
                    item.sShowM7 = SystemFunction.ConvertFormatDecimal4(item.sM7);
                    item.sShowM8 = SystemFunction.ConvertFormatDecimal4(item.sM8);
                    item.sShowM9 = SystemFunction.ConvertFormatDecimal4(item.sM9);
                    item.sShowM10 = SystemFunction.ConvertFormatDecimal4(item.sM10);
                    item.sShowM11 = SystemFunction.ConvertFormatDecimal4(item.sM11);
                    item.sShowM12 = SystemFunction.ConvertFormatDecimal4(item.sM12);
                    item.sShowTotal = SystemFunction.ConvertFormatDecimal4(item.sTotal);
                }
                #endregion
            }
            else if (dataDisplayType.DisplayType == 5)
            {
                #region Type 4 >> PTT Group Building & PTT Research & Technology Institute
                int[] arrProductID = new int[] { 86, 87 };
                var queryAll = (from epi in dataEPIFrom
                                from d in db.TIntensityDominator.Where(w => w.FormID == epi.FormID && arrProductID.Contains(w.ProductID)).AsEnumerable()
                                from i in db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.IntensityDenonimator && w.ProductID == d.ProductID)
                                select new TDatIntesity
                                {
                                    nProductID = i.ProductID,
                                    sProductName = i.ProductName,
                                    cTotal = i.cTotal,
                                    cTotalAll = i.cTotalAll,
                                    sUnit = i.sUnit,

                                    sM1 = i.ProductID == 86 ? d.nTotal : lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M1 : "",
                                    sM2 = i.ProductID == 86 ? d.nTotal : lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M2 : "",
                                    sM3 = i.ProductID == 86 ? d.nTotal : lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M3 : "",

                                    sM4 = i.ProductID == 86 ? d.nTotal : lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M4 : "",
                                    sM5 = i.ProductID == 86 ? d.nTotal : lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M5 : "",
                                    sM6 = i.ProductID == 86 ? d.nTotal : lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M6 : "",

                                    sM7 = i.ProductID == 86 ? d.nTotal : lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M7 : "",
                                    sM8 = i.ProductID == 86 ? d.nTotal : lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M8 : "",
                                    sM9 = i.ProductID == 86 ? d.nTotal : lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M9 : "",

                                    sM10 = i.ProductID == 86 ? d.nTotal : lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M10 : "",
                                    sM11 = i.ProductID == 86 ? d.nTotal : lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M11 : "",
                                    sM12 = i.ProductID == 86 ? d.nTotal : lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M12 : "",
                                }).ToList();

                lstInten = queryAll.GroupBy(g => new { g.nProductID, g.sProductName, g.cTotalAll, g.cTotal, g.sUnit }).Select(s => new TDatIntesity
                {
                    nProductID = s.Key.nProductID,
                    sProductName = s.Key.sProductName,
                    cTotalAll = s.Key.cTotalAll,
                    cTotal = s.Key.cTotal,
                    sUnit = s.Key.sUnit,
                    sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
                    sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
                    sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
                    sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
                    sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
                    sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
                    sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
                    sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
                    sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
                    sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
                    sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
                    sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
                }).ToList();

                foreach (var item in lstInten)
                {
                    item.sTotal = item.nProductID == 86 ? SystemFunction.ConvertFormatDecimal4(item.sM1) : SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";
                    item.sShowM1 = SystemFunction.ConvertFormatDecimal4(item.sM1);
                    item.sShowM2 = SystemFunction.ConvertFormatDecimal4(item.sM2);
                    item.sShowM3 = SystemFunction.ConvertFormatDecimal4(item.sM3);
                    item.sShowM4 = SystemFunction.ConvertFormatDecimal4(item.sM4);
                    item.sShowM5 = SystemFunction.ConvertFormatDecimal4(item.sM5);
                    item.sShowM6 = SystemFunction.ConvertFormatDecimal4(item.sM6);
                    item.sShowM7 = SystemFunction.ConvertFormatDecimal4(item.sM7);
                    item.sShowM8 = SystemFunction.ConvertFormatDecimal4(item.sM8);
                    item.sShowM9 = SystemFunction.ConvertFormatDecimal4(item.sM9);
                    item.sShowM10 = SystemFunction.ConvertFormatDecimal4(item.sM10);
                    item.sShowM11 = SystemFunction.ConvertFormatDecimal4(item.sM11);
                    item.sShowM12 = SystemFunction.ConvertFormatDecimal4(item.sM12);
                    item.sShowTotal = SystemFunction.ConvertFormatDecimal4(item.sTotal);
                }
                #endregion
            }
            else if (dataDisplayType.DisplayType == 6)
            {
                #region Type 4 >> Refinery
                int[] arrProductID = new int[] { 88, 89, 90 };
                var queryAll = (from epi in dataEPIFrom
                                from d in db.TIntensityDominator.Where(w => w.FormID == epi.FormID && arrProductID.Contains(w.ProductID)).AsEnumerable()
                                from i in db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.IntensityDenonimator && w.ProductID == d.ProductID)
                                select new TDatIntesity
                                {
                                    nProductID = i.ProductID,
                                    sProductName = i.ProductName,
                                    cTotal = i.cTotal,
                                    cTotalAll = i.cTotalAll,
                                    sUnit = i.sUnit,

                                    sM1 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M1 : "",
                                    sM2 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M2 : "",
                                    sM3 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M3 : "",

                                    sM4 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M4 : "",
                                    sM5 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M5 : "",
                                    sM6 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M6 : "",

                                    sM7 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M7 : "",
                                    sM8 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M8 : "",
                                    sM9 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M9 : "",

                                    sM10 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M10 : "",
                                    sM11 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M11 : "",
                                    sM12 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M12 : "",
                                }).ToList();

                lstInten = queryAll.GroupBy(g => new { g.nProductID, g.sProductName, g.cTotalAll, g.cTotal, g.sUnit }).Select(s => new TDatIntesity
                {
                    nProductID = s.Key.nProductID,
                    sProductName = s.Key.sProductName,
                    cTotalAll = s.Key.cTotalAll,
                    cTotal = s.Key.cTotal,
                    sUnit = s.Key.sUnit,
                    sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
                    sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
                    sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
                    sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
                    sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
                    sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
                    sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
                    sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
                    sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
                    sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
                    sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
                    sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
                }).ToList();

                foreach (var item in lstInten)
                {
                    item.sTotal = SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";
                    item.sShowM1 = SystemFunction.ConvertFormatDecimal4(item.sM1);
                    item.sShowM2 = SystemFunction.ConvertFormatDecimal4(item.sM2);
                    item.sShowM3 = SystemFunction.ConvertFormatDecimal4(item.sM3);
                    item.sShowM4 = SystemFunction.ConvertFormatDecimal4(item.sM4);
                    item.sShowM5 = SystemFunction.ConvertFormatDecimal4(item.sM5);
                    item.sShowM6 = SystemFunction.ConvertFormatDecimal4(item.sM6);
                    item.sShowM7 = SystemFunction.ConvertFormatDecimal4(item.sM7);
                    item.sShowM8 = SystemFunction.ConvertFormatDecimal4(item.sM8);
                    item.sShowM9 = SystemFunction.ConvertFormatDecimal4(item.sM9);
                    item.sShowM10 = SystemFunction.ConvertFormatDecimal4(item.sM10);
                    item.sShowM11 = SystemFunction.ConvertFormatDecimal4(item.sM11);
                    item.sShowM12 = SystemFunction.ConvertFormatDecimal4(item.sM12);
                    item.sShowTotal = SystemFunction.ConvertFormatDecimal4(item.sTotal);
                }
                #endregion
            }
        }
        result.lstDataInten = lstInten;
        return result;
    }

    private CResultGetDataPageLoad GetDataWater(int nYear, int nMainFacID, List<TDataSubFac> lstDataSubFac)
    {
        CResultGetDataPageLoad result = new CResultGetDataPageLoad();
        List<TDataWater> lstWater = new List<TDataWater>();
        string sYear = nYear + "";
        var lstSubFacID = lstDataSubFac.GroupBy(g => g.FacilityID).Select(s => s.Key).ToList();
        var dataEPIFrom = db.TEPI_Forms.Where(w => w.sYear == sYear && w.IDIndicator == DataType.Indicator.Water && lstSubFacID.Contains(w.FacilityID ?? 0)).ToList();
        #region Query
        var queryAll = (from epi in dataEPIFrom
                        from d in db.TWater_Product.Where(w => w.FormID == epi.FormID).AsEnumerable()
                        from i in db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.Water && w.ProductID == d.ProductID)
                        orderby i.nOrder
                        select new TDataWater
                        {
                            nProductID = i.ProductID,
                            sProductName = i.ProductName,
                            cTotal = i.cTotal,
                            cTotalAll = i.cTotalAll,
                            sUnit = i.sUnit,

                            sM1 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M1 : "",
                            sM2 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M2 : "",
                            sM3 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M3 : "",

                            sM4 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M4 : "",
                            sM5 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M5 : "",
                            sM6 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M6 : "",

                            sM7 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M7 : "",
                            sM8 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M8 : "",
                            sM9 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M9 : "",

                            sM10 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M10 : "",
                            sM11 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M11 : "",
                            sM12 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M12 : "",
                        }).ToList();

        lstWater = queryAll.GroupBy(g => new { g.nProductID, g.sProductName, g.cTotalAll, g.cTotal, g.sUnit }).Select(s => new TDataWater
        {
            nProductID = s.Key.nProductID,
            sProductName = s.Key.sProductName,
            cTotalAll = s.Key.cTotalAll,
            cTotal = s.Key.cTotal,
            sUnit = s.Key.sUnit,
            sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
            sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
            sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
            sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
            sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
            sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
            sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
            sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
            sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
            sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
            sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
            sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
        }).ToList();

        foreach (var item in lstWater)
        {
            //Default 0 เนื่องจาก PTT ไม่รับค่าว่าง
            if (string.IsNullOrEmpty(item.sM1))
                item.sM1 = "0";
            if (string.IsNullOrEmpty(item.sM2))
                item.sM2 = "0";
            if (string.IsNullOrEmpty(item.sM3))
                item.sM3 = "0";
            if (string.IsNullOrEmpty(item.sM4))
                item.sM4 = "0";
            if (string.IsNullOrEmpty(item.sM5))
                item.sM5 = "0";
            if (string.IsNullOrEmpty(item.sM6))
                item.sM6 = "0";
            if (string.IsNullOrEmpty(item.sM7))
                item.sM7 = "0";
            if (string.IsNullOrEmpty(item.sM8))
                item.sM8 = "0";
            if (string.IsNullOrEmpty(item.sM9))
                item.sM9 = "0";
            if (string.IsNullOrEmpty(item.sM10))
                item.sM10 = "0";
            if (string.IsNullOrEmpty(item.sM11))
                item.sM11 = "0";
            if (string.IsNullOrEmpty(item.sM12))
                item.sM12 = "0";

            if (item.nProductID == 100)
            {
                item.cTotal = "Y";
            }

            item.sTotal = SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";
            item.sShowM1 = SystemFunction.ConvertFormatDecimal4(item.sM1);
            item.sShowM2 = SystemFunction.ConvertFormatDecimal4(item.sM2);
            item.sShowM3 = SystemFunction.ConvertFormatDecimal4(item.sM3);
            item.sShowM4 = SystemFunction.ConvertFormatDecimal4(item.sM4);
            item.sShowM5 = SystemFunction.ConvertFormatDecimal4(item.sM5);
            item.sShowM6 = SystemFunction.ConvertFormatDecimal4(item.sM6);
            item.sShowM7 = SystemFunction.ConvertFormatDecimal4(item.sM7);
            item.sShowM8 = SystemFunction.ConvertFormatDecimal4(item.sM8);
            item.sShowM9 = SystemFunction.ConvertFormatDecimal4(item.sM9);
            item.sShowM10 = SystemFunction.ConvertFormatDecimal4(item.sM10);
            item.sShowM11 = SystemFunction.ConvertFormatDecimal4(item.sM11);
            item.sShowM12 = SystemFunction.ConvertFormatDecimal4(item.sM12);
            item.sShowTotal = SystemFunction.ConvertFormatDecimal4(item.sTotal);
        }
        #endregion
        result.lstDataWater = lstWater;
        return result;
    }

    private CResultGetDataPageLoad GetDataMaterial(int nYear, int nMainFacID, List<TDataSubFac> lstDataSubFac)
    {
        //Unit ID = 1 is m3, Unit ID = 3 is KG., Unit ID = 2 is Tonnes
        CResultGetDataPageLoad result = new CResultGetDataPageLoad();
        List<TDataMaterial> lstDataMaterial = new List<TDataMaterial>();
        string sYear = nYear + "";
        var lstSubFacID = lstDataSubFac.GroupBy(g => g.FacilityID).Select(s => s.Key).ToList();
        var dataEPIFrom = db.TEPI_Forms.Where(w => w.sYear == sYear && w.IDIndicator == DataType.Indicator.Material && lstSubFacID.Contains(w.FacilityID ?? 0)).ToList();
        int[] arrSetHeader = new int[] { 35, 36, 38, 39, 40 };
        decimal nFactorKGtoTonnes = 0.001m;

        #region Query
        var queryMainMaterial = (from epi in dataEPIFrom
                                 from d in db.TMaterial_Product.Where(w => w.FormID == epi.FormID).AsEnumerable()
                                 from i in db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.Material && w.ProductID == d.ProductID)
                                 group new { i, d } by new { i.ProductID, i.ProductName, i.cTotal, i.cTotalAll, i.sUnit } into grp
                                 select new TDataMaterial
                                 {
                                     nProductID = grp.Key.ProductID,
                                     sProductName = grp.Key.ProductName,
                                     cTotal = grp.Key.cTotal,
                                     cTotalAll = grp.Key.cTotalAll,
                                     sUnit = grp.Key.sUnit,
                                     nOption = grp.Max(x => x.d.nOption) ?? 0,
                                     sSetHeader = arrSetHeader.Contains(grp.Key.ProductID) ? "Y" : "N",
                                 }).ToList();

        var querySubMaterial = (from epi in dataEPIFrom
                                from d in db.TMaterial_ProductData.Where(w => w.FormID == epi.FormID).AsEnumerable()
                                select new
                                {
                                    d.nSubProductID,
                                    d.nUnderbyProductID,
                                    sName = d.sName.Trims(),
                                    d.nOption,
                                    d.nUnitID,
                                    d.Density,
                                    sM1 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M1 : "",
                                    sM2 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M2 : "",
                                    sM3 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M3 : "",

                                    sM4 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M4 : "",
                                    sM5 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M5 : "",
                                    sM6 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M6 : "",

                                    sM7 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M7 : "",
                                    sM8 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M8 : "",
                                    sM9 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M9 : "",

                                    sM10 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M10 : "",
                                    sM11 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M11 : "",
                                    sM12 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M12 : "",
                                }).ToList();
        #endregion

        var dataUnit = db.mTUnit.ToList();
        Func<int, string> GetUnitName = (id) =>
        {
            var q = dataUnit.FirstOrDefault(w => w.UnitID == id);
            return q != null ? q.UnitName : "";
        };

        #region//Renewable Direct Materials Used
        List<TDataMaterial> lstRenewDirect = new List<TDataMaterial>();
        var qRenewDirect = queryMainMaterial.FirstOrDefault(w => w.nProductID == 35);
        if (qRenewDirect != null)
        {
            var qGroupRenewDirect = querySubMaterial.Where(w => w.nUnderbyProductID == 35).GroupBy(g => new
            {
                g.nUnderbyProductID,
                g.nUnitID,
                g.sName
            }).Select(s => new
            {
                s.Key.nUnderbyProductID,
                s.Key.nUnitID,
                s.Key.sName,
                nDesnsity = s.Sum(x => SystemFunction.GetDecimalNull(x.Density)),
                sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
                sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
                sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
                sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
                sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
                sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
                sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
                sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
                sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
                sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
                sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
                sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : ""
            }).OrderBy(o => o.sName).ToList();

            #region Calculate Total Renewable
            decimal? nM1 = null, nM2 = null, nM3 = null, nM4 = null, nM5 = null, nM6 = null, nM7 = null, nM8 = null, nM9 = null, nM10 = null, nM11 = null, nM12 = null;
            foreach (var item in qGroupRenewDirect)
            {
                decimal? nValM1 = SystemFunction.GetDecimalNull(item.sM1);
                decimal? nValM2 = SystemFunction.GetDecimalNull(item.sM2);
                decimal? nValM3 = SystemFunction.GetDecimalNull(item.sM3);
                decimal? nValM4 = SystemFunction.GetDecimalNull(item.sM4);
                decimal? nValM5 = SystemFunction.GetDecimalNull(item.sM5);
                decimal? nValM6 = SystemFunction.GetDecimalNull(item.sM6);
                decimal? nValM7 = SystemFunction.GetDecimalNull(item.sM7);
                decimal? nValM8 = SystemFunction.GetDecimalNull(item.sM8);
                decimal? nValM9 = SystemFunction.GetDecimalNull(item.sM9);
                decimal? nValM10 = SystemFunction.GetDecimalNull(item.sM10);
                decimal? nValM11 = SystemFunction.GetDecimalNull(item.sM11);
                decimal? nValM12 = SystemFunction.GetDecimalNull(item.sM12);
                if (nM1.HasValue || nValM1.HasValue)
                    nM1 = (nM1 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM1) : item.nUnitID == 3 ? nValM1 * nFactorKGtoTonnes : (nValM1 ?? 0));

                if (nM2.HasValue || nValM2.HasValue)
                    nM2 = (nM2 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM2) : item.nUnitID == 3 ? nValM2 * nFactorKGtoTonnes : (nValM2 ?? 0));

                if (nM3.HasValue || nValM3.HasValue)
                    nM3 = (nM3 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM3) : item.nUnitID == 3 ? nValM3 * nFactorKGtoTonnes : (nValM3 ?? 0));

                if (nM4.HasValue || nValM4.HasValue)
                    nM4 = (nM4 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM4) : item.nUnitID == 3 ? nValM4 * nFactorKGtoTonnes : (nValM4 ?? 0));

                if (nM5.HasValue || nValM5.HasValue)
                    nM5 = (nM5 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM5) : item.nUnitID == 3 ? nValM5 * nFactorKGtoTonnes : (nValM5 ?? 0));

                if (nM6.HasValue || nValM6.HasValue)
                    nM6 = (nM6 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM6) : item.nUnitID == 3 ? nValM6 * nFactorKGtoTonnes : (nValM6 ?? 0));

                if (nM7.HasValue || nValM7.HasValue)
                    nM7 = (nM7 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM7) : item.nUnitID == 3 ? nValM7 * nFactorKGtoTonnes : (nValM7 ?? 0));

                if (nM8.HasValue || nValM8.HasValue)
                    nM8 = (nM8 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM8) : item.nUnitID == 3 ? nValM8 * nFactorKGtoTonnes : (nValM8 ?? 0));

                if (nM9.HasValue || nValM9.HasValue)
                    nM9 = (nM9 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM9) : item.nUnitID == 3 ? nValM9 * nFactorKGtoTonnes : (nValM9 ?? 0));

                if (nM10.HasValue || nValM10.HasValue)
                    nM10 = (nM10 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM10) : item.nUnitID == 3 ? nValM10 * nFactorKGtoTonnes : (nValM10 ?? 0));

                if (nM11.HasValue || nValM11.HasValue)
                    nM11 = (nM11 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM11) : item.nUnitID == 3 ? nValM11 * nFactorKGtoTonnes : (nValM11 ?? 0));

                if (nM12.HasValue || nValM12.HasValue)
                    nM12 = (nM12 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM12) : item.nUnitID == 3 ? nValM12 * nFactorKGtoTonnes : (nValM12 ?? 0));
            }
            qRenewDirect.sM1 = nM1 + "";
            qRenewDirect.sM2 = nM2 + "";
            qRenewDirect.sM3 = nM3 + "";
            qRenewDirect.sM4 = nM4 + "";
            qRenewDirect.sM5 = nM5 + "";
            qRenewDirect.sM6 = nM6 + "";
            qRenewDirect.sM7 = nM7 + "";
            qRenewDirect.sM8 = nM8 + "";
            qRenewDirect.sM9 = nM9 + "";
            qRenewDirect.sM10 = nM10 + "";
            qRenewDirect.sM11 = nM11 + "";
            qRenewDirect.sM12 = nM12 + "";
            #endregion
            lstRenewDirect.Add(qRenewDirect);//Add Total Renew

            //Add Sub Renewable
            foreach (var item in qGroupRenewDirect)
            {
                lstRenewDirect.Add(new TDataMaterial
                    {
                        nHeaderID = 35,
                        sProductName = item.sName,
                        UnitID = item.nUnitID ?? 0,
                        sUnit = GetUnitName(item.nUnitID ?? 0),
                        sDensity = item.nDesnsity + "",
                        sM1 = item.sM1,
                        sM2 = item.sM2,
                        sM3 = item.sM3,
                        sM4 = item.sM4,
                        sM5 = item.sM5,
                        sM6 = item.sM6,
                        sM7 = item.sM7,
                        sM8 = item.sM8,
                        sM9 = item.sM9,
                        sM10 = item.sM10,
                        sM11 = item.sM11,
                        sM12 = item.sM12,
                    });
            }
        }
        #endregion

        #region//Non-renewable Direct Materials Used
        List<TDataMaterial> lstNonRenewDirect = new List<TDataMaterial>();
        var qNonRenewDirect = queryMainMaterial.FirstOrDefault(w => w.nProductID == 36);
        if (qNonRenewDirect != null)
        {
            var qGroupNonRenewDirect = querySubMaterial.Where(w => w.nUnderbyProductID == 36).GroupBy(g => new
            {
                g.nUnderbyProductID,
                g.nUnitID,
                g.sName
            }).Select(s => new
            {
                s.Key.nUnderbyProductID,
                s.Key.nUnitID,
                s.Key.sName,
                nDesnsity = s.Sum(x => SystemFunction.GetDecimalNull(x.Density)),
                sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
                sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
                sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
                sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
                sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
                sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
                sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
                sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
                sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
                sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
                sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
                sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : ""
            }).OrderBy(o => o.sName).ToList();

            #region Calculate Total Non-renewable
            decimal? nM1 = null, nM2 = null, nM3 = null, nM4 = null, nM5 = null, nM6 = null, nM7 = null, nM8 = null, nM9 = null, nM10 = null, nM11 = null, nM12 = null;
            foreach (var item in qGroupNonRenewDirect)
            {
                decimal? nValM1 = SystemFunction.GetDecimalNull(item.sM1);
                decimal? nValM2 = SystemFunction.GetDecimalNull(item.sM2);
                decimal? nValM3 = SystemFunction.GetDecimalNull(item.sM3);
                decimal? nValM4 = SystemFunction.GetDecimalNull(item.sM4);
                decimal? nValM5 = SystemFunction.GetDecimalNull(item.sM5);
                decimal? nValM6 = SystemFunction.GetDecimalNull(item.sM6);
                decimal? nValM7 = SystemFunction.GetDecimalNull(item.sM7);
                decimal? nValM8 = SystemFunction.GetDecimalNull(item.sM8);
                decimal? nValM9 = SystemFunction.GetDecimalNull(item.sM9);
                decimal? nValM10 = SystemFunction.GetDecimalNull(item.sM10);
                decimal? nValM11 = SystemFunction.GetDecimalNull(item.sM11);
                decimal? nValM12 = SystemFunction.GetDecimalNull(item.sM12);
                if (nM1.HasValue || nValM1.HasValue)
                    nM1 = (nM1 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM1) : item.nUnitID == 3 ? nValM1 * nFactorKGtoTonnes : (nValM1 ?? 0));

                if (nM2.HasValue || nValM2.HasValue)
                    nM2 = (nM2 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM2) : item.nUnitID == 3 ? nValM2 * nFactorKGtoTonnes : (nValM2 ?? 0));

                if (nM3.HasValue || nValM3.HasValue)
                    nM3 = (nM3 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM3) : item.nUnitID == 3 ? nValM3 * nFactorKGtoTonnes : (nValM3 ?? 0));

                if (nM4.HasValue || nValM4.HasValue)
                    nM4 = (nM4 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM4) : item.nUnitID == 3 ? nValM4 * nFactorKGtoTonnes : (nValM4 ?? 0));

                if (nM5.HasValue || nValM5.HasValue)
                    nM5 = (nM5 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM5) : item.nUnitID == 3 ? nValM5 * nFactorKGtoTonnes : (nValM5 ?? 0));

                if (nM6.HasValue || nValM6.HasValue)
                    nM6 = (nM6 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM6) : item.nUnitID == 3 ? nValM6 * nFactorKGtoTonnes : (nValM6 ?? 0));

                if (nM7.HasValue || nValM7.HasValue)
                    nM7 = (nM7 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM7) : item.nUnitID == 3 ? nValM7 * nFactorKGtoTonnes : (nValM7 ?? 0));

                if (nM8.HasValue || nValM8.HasValue)
                    nM8 = (nM8 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM8) : item.nUnitID == 3 ? nValM8 * nFactorKGtoTonnes : (nValM8 ?? 0));

                if (nM9.HasValue || nValM9.HasValue)
                    nM9 = (nM9 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM9) : item.nUnitID == 3 ? nValM9 * nFactorKGtoTonnes : (nValM9 ?? 0));

                if (nM10.HasValue || nValM10.HasValue)
                    nM10 = (nM10 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM10) : item.nUnitID == 3 ? nValM10 * nFactorKGtoTonnes : (nValM10 ?? 0));

                if (nM11.HasValue || nValM11.HasValue)
                    nM11 = (nM11 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM11) : item.nUnitID == 3 ? nValM11 * nFactorKGtoTonnes : (nValM11 ?? 0));

                if (nM12.HasValue || nValM12.HasValue)
                    nM12 = (nM12 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM12) : item.nUnitID == 3 ? nValM12 * nFactorKGtoTonnes : (nValM12 ?? 0));
            }
            qNonRenewDirect.sM1 = nM1 + "";
            qNonRenewDirect.sM2 = nM2 + "";
            qNonRenewDirect.sM3 = nM3 + "";
            qNonRenewDirect.sM4 = nM4 + "";
            qNonRenewDirect.sM5 = nM5 + "";
            qNonRenewDirect.sM6 = nM6 + "";
            qNonRenewDirect.sM7 = nM7 + "";
            qNonRenewDirect.sM8 = nM8 + "";
            qNonRenewDirect.sM9 = nM9 + "";
            qNonRenewDirect.sM10 = nM10 + "";
            qNonRenewDirect.sM11 = nM11 + "";
            qNonRenewDirect.sM12 = nM12 + "";
            #endregion
            lstNonRenewDirect.Add(qNonRenewDirect);//Add Total Non-renew

            //Add Sub Renewable
            foreach (var item in qGroupNonRenewDirect)
            {
                lstNonRenewDirect.Add(new TDataMaterial
                {
                    nHeaderID = 36,
                    sProductName = item.sName,
                    UnitID = item.nUnitID ?? 0,
                    sUnit = GetUnitName(item.nUnitID ?? 0),
                    sDensity = item.nDesnsity + "",
                    sM1 = item.sM1,
                    sM2 = item.sM2,
                    sM3 = item.sM3,
                    sM4 = item.sM4,
                    sM5 = item.sM5,
                    sM6 = item.sM6,
                    sM7 = item.sM7,
                    sM8 = item.sM8,
                    sM9 = item.sM9,
                    sM10 = item.sM10,
                    sM11 = item.sM11,
                    sM12 = item.sM12,
                });
            }
        }
        #endregion

        #region Total Direct Materials Used
        var qTotalDirect = queryMainMaterial.FirstOrDefault(w => w.nProductID == 34);
        if (qTotalDirect != null && qRenewDirect != null && qNonRenewDirect != null)
        {
            qTotalDirect.sM1 = SystemFunction.SumDataToDecimal(new List<string> { qRenewDirect.sM1, qNonRenewDirect.sM1 }) + "";
            qTotalDirect.sM2 = SystemFunction.SumDataToDecimal(new List<string> { qRenewDirect.sM2, qNonRenewDirect.sM2 }) + "";
            qTotalDirect.sM3 = SystemFunction.SumDataToDecimal(new List<string> { qRenewDirect.sM3, qNonRenewDirect.sM3 }) + "";
            qTotalDirect.sM4 = SystemFunction.SumDataToDecimal(new List<string> { qRenewDirect.sM4, qNonRenewDirect.sM4 }) + "";
            qTotalDirect.sM5 = SystemFunction.SumDataToDecimal(new List<string> { qRenewDirect.sM5, qNonRenewDirect.sM5 }) + "";
            qTotalDirect.sM6 = SystemFunction.SumDataToDecimal(new List<string> { qRenewDirect.sM6, qNonRenewDirect.sM6 }) + "";
            qTotalDirect.sM7 = SystemFunction.SumDataToDecimal(new List<string> { qRenewDirect.sM7, qNonRenewDirect.sM7 }) + "";
            qTotalDirect.sM8 = SystemFunction.SumDataToDecimal(new List<string> { qRenewDirect.sM8, qNonRenewDirect.sM8 }) + "";
            qTotalDirect.sM9 = SystemFunction.SumDataToDecimal(new List<string> { qRenewDirect.sM9, qNonRenewDirect.sM9 }) + "";
            qTotalDirect.sM10 = SystemFunction.SumDataToDecimal(new List<string> { qRenewDirect.sM10, qNonRenewDirect.sM10 }) + "";
            qTotalDirect.sM11 = SystemFunction.SumDataToDecimal(new List<string> { qRenewDirect.sM11, qNonRenewDirect.sM11 }) + "";
            qTotalDirect.sM12 = SystemFunction.SumDataToDecimal(new List<string> { qRenewDirect.sM12, qNonRenewDirect.sM12 }) + "";
        }
        #endregion

        #region//Catalyst Used
        List<TDataMaterial> lstCatalyst = new List<TDataMaterial>();
        var qCatalyst = queryMainMaterial.FirstOrDefault(w => w.nProductID == 38);
        if (qCatalyst != null)
        {
            var qGroupCatalyst = querySubMaterial.Where(w => w.nUnderbyProductID == 38).GroupBy(g => new
            {
                g.nUnderbyProductID,
                g.nUnitID,
                g.sName
            }).Select(s => new
            {
                s.Key.nUnderbyProductID,
                s.Key.nUnitID,
                s.Key.sName,
                nDesnsity = s.Sum(x => SystemFunction.GetDecimalNull(x.Density)),
                sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
                sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
                sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
                sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
                sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
                sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
                sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
                sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
                sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
                sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
                sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
                sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : ""
            }).OrderBy(o => o.sName).ToList();

            #region Calculate Catalyst
            decimal? nM1 = null, nM2 = null, nM3 = null, nM4 = null, nM5 = null, nM6 = null, nM7 = null, nM8 = null, nM9 = null, nM10 = null, nM11 = null, nM12 = null;
            foreach (var item in qGroupCatalyst)
            {
                decimal? nValM1 = SystemFunction.GetDecimalNull(item.sM1);
                decimal? nValM2 = SystemFunction.GetDecimalNull(item.sM2);
                decimal? nValM3 = SystemFunction.GetDecimalNull(item.sM3);
                decimal? nValM4 = SystemFunction.GetDecimalNull(item.sM4);
                decimal? nValM5 = SystemFunction.GetDecimalNull(item.sM5);
                decimal? nValM6 = SystemFunction.GetDecimalNull(item.sM6);
                decimal? nValM7 = SystemFunction.GetDecimalNull(item.sM7);
                decimal? nValM8 = SystemFunction.GetDecimalNull(item.sM8);
                decimal? nValM9 = SystemFunction.GetDecimalNull(item.sM9);
                decimal? nValM10 = SystemFunction.GetDecimalNull(item.sM10);
                decimal? nValM11 = SystemFunction.GetDecimalNull(item.sM11);
                decimal? nValM12 = SystemFunction.GetDecimalNull(item.sM12);
                if (nM1.HasValue || nValM1.HasValue)
                    nM1 = (nM1 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM1) : item.nUnitID == 3 ? nValM1 * nFactorKGtoTonnes : (nValM1 ?? 0));

                if (nM2.HasValue || nValM2.HasValue)
                    nM2 = (nM2 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM2) : item.nUnitID == 3 ? nValM2 * nFactorKGtoTonnes : (nValM2 ?? 0));

                if (nM3.HasValue || nValM3.HasValue)
                    nM3 = (nM3 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM3) : item.nUnitID == 3 ? nValM3 * nFactorKGtoTonnes : (nValM3 ?? 0));

                if (nM4.HasValue || nValM4.HasValue)
                    nM4 = (nM4 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM4) : item.nUnitID == 3 ? nValM4 * nFactorKGtoTonnes : (nValM4 ?? 0));

                if (nM5.HasValue || nValM5.HasValue)
                    nM5 = (nM5 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM5) : item.nUnitID == 3 ? nValM5 * nFactorKGtoTonnes : (nValM5 ?? 0));

                if (nM6.HasValue || nValM6.HasValue)
                    nM6 = (nM6 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM6) : item.nUnitID == 3 ? nValM6 * nFactorKGtoTonnes : (nValM6 ?? 0));

                if (nM7.HasValue || nValM7.HasValue)
                    nM7 = (nM7 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM7) : item.nUnitID == 3 ? nValM7 * nFactorKGtoTonnes : (nValM7 ?? 0));

                if (nM8.HasValue || nValM8.HasValue)
                    nM8 = (nM8 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM8) : item.nUnitID == 3 ? nValM8 * nFactorKGtoTonnes : (nValM8 ?? 0));

                if (nM9.HasValue || nValM9.HasValue)
                    nM9 = (nM9 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM9) : item.nUnitID == 3 ? nValM9 * nFactorKGtoTonnes : (nValM9 ?? 0));

                if (nM10.HasValue || nValM10.HasValue)
                    nM10 = (nM10 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM10) : item.nUnitID == 3 ? nValM10 * nFactorKGtoTonnes : (nValM10 ?? 0));

                if (nM11.HasValue || nValM11.HasValue)
                    nM11 = (nM11 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM11) : item.nUnitID == 3 ? nValM11 * nFactorKGtoTonnes : (nValM11 ?? 0));

                if (nM12.HasValue || nValM12.HasValue)
                    nM12 = (nM12 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM12) : item.nUnitID == 3 ? nValM12 * nFactorKGtoTonnes : (nValM12 ?? 0));
            }
            qCatalyst.sM1 = nM1 + "";
            qCatalyst.sM2 = nM2 + "";
            qCatalyst.sM3 = nM3 + "";
            qCatalyst.sM4 = nM4 + "";
            qCatalyst.sM5 = nM5 + "";
            qCatalyst.sM6 = nM6 + "";
            qCatalyst.sM7 = nM7 + "";
            qCatalyst.sM8 = nM8 + "";
            qCatalyst.sM9 = nM9 + "";
            qCatalyst.sM10 = nM10 + "";
            qCatalyst.sM11 = nM11 + "";
            qCatalyst.sM12 = nM12 + "";
            #endregion
            lstCatalyst.Add(qCatalyst);

            //Add Sub Catalyst
            foreach (var item in qGroupCatalyst)
            {
                lstCatalyst.Add(new TDataMaterial
                {
                    nHeaderID = 38,
                    sProductName = item.sName,
                    UnitID = item.nUnitID ?? 0,
                    sUnit = GetUnitName(item.nUnitID ?? 0),
                    sDensity = item.nDesnsity + "",
                    sM1 = item.sM1,
                    sM2 = item.sM2,
                    sM3 = item.sM3,
                    sM4 = item.sM4,
                    sM5 = item.sM5,
                    sM6 = item.sM6,
                    sM7 = item.sM7,
                    sM8 = item.sM8,
                    sM9 = item.sM9,
                    sM10 = item.sM10,
                    sM11 = item.sM11,
                    sM12 = item.sM12,
                });
            }
        }
        #endregion

        #region//Renewable Associated Materials Used
        List<TDataMaterial> lstRenewAsso = new List<TDataMaterial>();
        var qRenewAsso = queryMainMaterial.FirstOrDefault(w => w.nProductID == 39);
        if (qRenewAsso != null)
        {
            var qGroupRenewAsso = querySubMaterial.Where(w => w.nUnderbyProductID == 39).GroupBy(g => new
            {
                g.nUnderbyProductID,
                g.nUnitID,
                g.sName
            }).Select(s => new
            {
                s.Key.nUnderbyProductID,
                s.Key.nUnitID,
                s.Key.sName,
                nDesnsity = s.Sum(x => SystemFunction.GetDecimalNull(x.Density)),
                sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
                sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
                sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
                sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
                sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
                sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
                sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
                sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
                sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
                sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
                sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
                sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : ""
            }).OrderBy(o => o.sName).ToList();

            #region Calculate Total Renewable
            decimal? nM1 = null, nM2 = null, nM3 = null, nM4 = null, nM5 = null, nM6 = null, nM7 = null, nM8 = null, nM9 = null, nM10 = null, nM11 = null, nM12 = null;
            foreach (var item in qGroupRenewAsso)
            {
                decimal? nValM1 = SystemFunction.GetDecimalNull(item.sM1);
                decimal? nValM2 = SystemFunction.GetDecimalNull(item.sM2);
                decimal? nValM3 = SystemFunction.GetDecimalNull(item.sM3);
                decimal? nValM4 = SystemFunction.GetDecimalNull(item.sM4);
                decimal? nValM5 = SystemFunction.GetDecimalNull(item.sM5);
                decimal? nValM6 = SystemFunction.GetDecimalNull(item.sM6);
                decimal? nValM7 = SystemFunction.GetDecimalNull(item.sM7);
                decimal? nValM8 = SystemFunction.GetDecimalNull(item.sM8);
                decimal? nValM9 = SystemFunction.GetDecimalNull(item.sM9);
                decimal? nValM10 = SystemFunction.GetDecimalNull(item.sM10);
                decimal? nValM11 = SystemFunction.GetDecimalNull(item.sM11);
                decimal? nValM12 = SystemFunction.GetDecimalNull(item.sM12);
                if (nM1.HasValue || nValM1.HasValue)
                    nM1 = (nM1 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM1) : item.nUnitID == 3 ? nValM1 * nFactorKGtoTonnes : (nValM1 ?? 0));

                if (nM2.HasValue || nValM2.HasValue)
                    nM2 = (nM2 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM2) : item.nUnitID == 3 ? nValM2 * nFactorKGtoTonnes : (nValM2 ?? 0));

                if (nM3.HasValue || nValM3.HasValue)
                    nM3 = (nM3 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM3) : item.nUnitID == 3 ? nValM3 * nFactorKGtoTonnes : (nValM3 ?? 0));

                if (nM4.HasValue || nValM4.HasValue)
                    nM4 = (nM4 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM4) : item.nUnitID == 3 ? nValM4 * nFactorKGtoTonnes : (nValM4 ?? 0));

                if (nM5.HasValue || nValM5.HasValue)
                    nM5 = (nM5 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM5) : item.nUnitID == 3 ? nValM5 * nFactorKGtoTonnes : (nValM5 ?? 0));

                if (nM6.HasValue || nValM6.HasValue)
                    nM6 = (nM6 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM6) : item.nUnitID == 3 ? nValM6 * nFactorKGtoTonnes : (nValM6 ?? 0));

                if (nM7.HasValue || nValM7.HasValue)
                    nM7 = (nM7 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM7) : item.nUnitID == 3 ? nValM7 * nFactorKGtoTonnes : (nValM7 ?? 0));

                if (nM8.HasValue || nValM8.HasValue)
                    nM8 = (nM8 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM8) : item.nUnitID == 3 ? nValM8 * nFactorKGtoTonnes : (nValM8 ?? 0));

                if (nM9.HasValue || nValM9.HasValue)
                    nM9 = (nM9 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM9) : item.nUnitID == 3 ? nValM9 * nFactorKGtoTonnes : (nValM9 ?? 0));

                if (nM10.HasValue || nValM10.HasValue)
                    nM10 = (nM10 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM10) : item.nUnitID == 3 ? nValM10 * nFactorKGtoTonnes : (nValM10 ?? 0));

                if (nM11.HasValue || nValM11.HasValue)
                    nM11 = (nM11 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM11) : item.nUnitID == 3 ? nValM11 * nFactorKGtoTonnes : (nValM11 ?? 0));

                if (nM12.HasValue || nValM12.HasValue)
                    nM12 = (nM12 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM12) : item.nUnitID == 3 ? nValM12 * nFactorKGtoTonnes : (nValM12 ?? 0));
            }
            qRenewAsso.sM1 = nM1 + "";
            qRenewAsso.sM2 = nM2 + "";
            qRenewAsso.sM3 = nM3 + "";
            qRenewAsso.sM4 = nM4 + "";
            qRenewAsso.sM5 = nM5 + "";
            qRenewAsso.sM6 = nM6 + "";
            qRenewAsso.sM7 = nM7 + "";
            qRenewAsso.sM8 = nM8 + "";
            qRenewAsso.sM9 = nM9 + "";
            qRenewAsso.sM10 = nM10 + "";
            qRenewAsso.sM11 = nM11 + "";
            qRenewAsso.sM12 = nM12 + "";
            #endregion
            lstRenewAsso.Add(qRenewAsso);//Add Total Non-renew

            //Add Sub Non-renewable
            foreach (var item in qGroupRenewAsso)
            {
                lstRenewAsso.Add(new TDataMaterial
                {
                    nHeaderID = 39,
                    sProductName = item.sName,
                    UnitID = item.nUnitID ?? 0,
                    sUnit = GetUnitName(item.nUnitID ?? 0),
                    sDensity = item.nDesnsity + "",
                    sM1 = item.sM1,
                    sM2 = item.sM2,
                    sM3 = item.sM3,
                    sM4 = item.sM4,
                    sM5 = item.sM5,
                    sM6 = item.sM6,
                    sM7 = item.sM7,
                    sM8 = item.sM8,
                    sM9 = item.sM9,
                    sM10 = item.sM10,
                    sM11 = item.sM11,
                    sM12 = item.sM12,
                });
            }
        }
        #endregion

        #region//Non-renewable Associated Materials Used
        List<TDataMaterial> lstNonRenewAsso = new List<TDataMaterial>();
        var qNonRenewAsso = queryMainMaterial.FirstOrDefault(w => w.nProductID == 40);
        if (qNonRenewAsso != null)
        {
            var qGroupNonRenewAsso = querySubMaterial.Where(w => w.nUnderbyProductID == 40).GroupBy(g => new
            {
                g.nUnderbyProductID,
                g.nUnitID,
                g.sName
            }).Select(s => new
            {
                s.Key.nUnderbyProductID,
                s.Key.nUnitID,
                s.Key.sName,
                nDesnsity = s.Sum(x => SystemFunction.GetDecimalNull(x.Density)),
                sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
                sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
                sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
                sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
                sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
                sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
                sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
                sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
                sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
                sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
                sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
                sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : ""
            }).OrderBy(o => o.sName).ToList();

            #region Calculate Total Non-renewable
            decimal? nM1 = null, nM2 = null, nM3 = null, nM4 = null, nM5 = null, nM6 = null, nM7 = null, nM8 = null, nM9 = null, nM10 = null, nM11 = null, nM12 = null;
            foreach (var item in qGroupNonRenewAsso)
            {
                decimal? nValM1 = SystemFunction.GetDecimalNull(item.sM1);
                decimal? nValM2 = SystemFunction.GetDecimalNull(item.sM2);
                decimal? nValM3 = SystemFunction.GetDecimalNull(item.sM3);
                decimal? nValM4 = SystemFunction.GetDecimalNull(item.sM4);
                decimal? nValM5 = SystemFunction.GetDecimalNull(item.sM5);
                decimal? nValM6 = SystemFunction.GetDecimalNull(item.sM6);
                decimal? nValM7 = SystemFunction.GetDecimalNull(item.sM7);
                decimal? nValM8 = SystemFunction.GetDecimalNull(item.sM8);
                decimal? nValM9 = SystemFunction.GetDecimalNull(item.sM9);
                decimal? nValM10 = SystemFunction.GetDecimalNull(item.sM10);
                decimal? nValM11 = SystemFunction.GetDecimalNull(item.sM11);
                decimal? nValM12 = SystemFunction.GetDecimalNull(item.sM12);
                if (nM1.HasValue || nValM1.HasValue)
                    nM1 = (nM1 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM1) : item.nUnitID == 3 ? nValM1 * nFactorKGtoTonnes : (nValM1 ?? 0));

                if (nM2.HasValue || nValM2.HasValue)
                    nM2 = (nM2 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM2) : item.nUnitID == 3 ? nValM2 * nFactorKGtoTonnes : (nValM2 ?? 0));

                if (nM3.HasValue || nValM3.HasValue)
                    nM3 = (nM3 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM3) : item.nUnitID == 3 ? nValM3 * nFactorKGtoTonnes : (nValM3 ?? 0));

                if (nM4.HasValue || nValM4.HasValue)
                    nM4 = (nM4 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM4) : item.nUnitID == 3 ? nValM4 * nFactorKGtoTonnes : (nValM4 ?? 0));

                if (nM5.HasValue || nValM5.HasValue)
                    nM5 = (nM5 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM5) : item.nUnitID == 3 ? nValM5 * nFactorKGtoTonnes : (nValM5 ?? 0));

                if (nM6.HasValue || nValM6.HasValue)
                    nM6 = (nM6 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM6) : item.nUnitID == 3 ? nValM6 * nFactorKGtoTonnes : (nValM6 ?? 0));

                if (nM7.HasValue || nValM7.HasValue)
                    nM7 = (nM7 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM7) : item.nUnitID == 3 ? nValM7 * nFactorKGtoTonnes : (nValM7 ?? 0));

                if (nM8.HasValue || nValM8.HasValue)
                    nM8 = (nM8 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM8) : item.nUnitID == 3 ? nValM8 * nFactorKGtoTonnes : (nValM8 ?? 0));

                if (nM9.HasValue || nValM9.HasValue)
                    nM9 = (nM9 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM9) : item.nUnitID == 3 ? nValM9 * nFactorKGtoTonnes : (nValM9 ?? 0));

                if (nM10.HasValue || nValM10.HasValue)
                    nM10 = (nM10 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM10) : item.nUnitID == 3 ? nValM10 * nFactorKGtoTonnes : (nValM10 ?? 0));

                if (nM11.HasValue || nValM11.HasValue)
                    nM11 = (nM11 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM11) : item.nUnitID == 3 ? nValM11 * nFactorKGtoTonnes : (nValM11 ?? 0));

                if (nM12.HasValue || nValM12.HasValue)
                    nM12 = (nM12 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM12) : item.nUnitID == 3 ? nValM12 * nFactorKGtoTonnes : (nValM12 ?? 0));
            }
            qNonRenewAsso.sM1 = nM1 + "";
            qNonRenewAsso.sM2 = nM2 + "";
            qNonRenewAsso.sM3 = nM3 + "";
            qNonRenewAsso.sM4 = nM4 + "";
            qNonRenewAsso.sM5 = nM5 + "";
            qNonRenewAsso.sM6 = nM6 + "";
            qNonRenewAsso.sM7 = nM7 + "";
            qNonRenewAsso.sM8 = nM8 + "";
            qNonRenewAsso.sM9 = nM9 + "";
            qNonRenewAsso.sM10 = nM10 + "";
            qNonRenewAsso.sM11 = nM11 + "";
            qNonRenewAsso.sM12 = nM12 + "";
            #endregion
            lstNonRenewAsso.Add(qNonRenewAsso);//Add Total Non-renew

            //Add Sub Non-renewable
            foreach (var item in qGroupNonRenewAsso)
            {
                lstNonRenewAsso.Add(new TDataMaterial
                {
                    nHeaderID = 40,
                    sProductName = item.sName,
                    UnitID = item.nUnitID ?? 0,
                    sUnit = GetUnitName(item.nUnitID ?? 0),
                    sDensity = item.nDesnsity + "",
                    sM1 = item.sM1,
                    sM2 = item.sM2,
                    sM3 = item.sM3,
                    sM4 = item.sM4,
                    sM5 = item.sM5,
                    sM6 = item.sM6,
                    sM7 = item.sM7,
                    sM8 = item.sM8,
                    sM9 = item.sM9,
                    sM10 = item.sM10,
                    sM11 = item.sM11,
                    sM12 = item.sM12,
                });
            }
        }
        #endregion

        #region Total Associated Materials Used
        var qTotalAssociated = queryMainMaterial.FirstOrDefault(w => w.nProductID == 37);
        if (qTotalAssociated != null)
        {
            qTotalAssociated.sM1 = SystemFunction.SumDataToDecimal(new List<string> { qCatalyst.sM1, qRenewAsso.sM1, qNonRenewAsso.sM1 }) + "";
            qTotalAssociated.sM2 = SystemFunction.SumDataToDecimal(new List<string> { qCatalyst.sM2, qRenewAsso.sM2, qNonRenewAsso.sM2 }) + "";
            qTotalAssociated.sM3 = SystemFunction.SumDataToDecimal(new List<string> { qCatalyst.sM3, qRenewAsso.sM3, qNonRenewAsso.sM3 }) + "";
            qTotalAssociated.sM4 = SystemFunction.SumDataToDecimal(new List<string> { qCatalyst.sM4, qRenewAsso.sM4, qNonRenewAsso.sM4 }) + "";
            qTotalAssociated.sM5 = SystemFunction.SumDataToDecimal(new List<string> { qCatalyst.sM5, qRenewAsso.sM5, qNonRenewAsso.sM5 }) + "";
            qTotalAssociated.sM6 = SystemFunction.SumDataToDecimal(new List<string> { qCatalyst.sM6, qRenewAsso.sM6, qNonRenewAsso.sM6 }) + "";
            qTotalAssociated.sM7 = SystemFunction.SumDataToDecimal(new List<string> { qCatalyst.sM7, qRenewAsso.sM7, qNonRenewAsso.sM7 }) + "";
            qTotalAssociated.sM8 = SystemFunction.SumDataToDecimal(new List<string> { qCatalyst.sM8, qRenewAsso.sM8, qNonRenewAsso.sM8 }) + "";
            qTotalAssociated.sM9 = SystemFunction.SumDataToDecimal(new List<string> { qCatalyst.sM9, qRenewAsso.sM9, qNonRenewAsso.sM9 }) + "";
            qTotalAssociated.sM10 = SystemFunction.SumDataToDecimal(new List<string> { qCatalyst.sM10, qRenewAsso.sM10, qNonRenewAsso.sM10 }) + "";
            qTotalAssociated.sM11 = SystemFunction.SumDataToDecimal(new List<string> { qCatalyst.sM11, qRenewAsso.sM11, qNonRenewAsso.sM11 }) + "";
            qTotalAssociated.sM12 = SystemFunction.SumDataToDecimal(new List<string> { qCatalyst.sM12, qRenewAsso.sM12, qNonRenewAsso.sM12 }) + "";
        }
        #endregion

        #region//Recycled Input Materials Used
        List<TDataMaterial> lstRecycled = new List<TDataMaterial>();
        var qTotalRecycled = queryMainMaterial.FirstOrDefault(w => w.nProductID == 41);
        if (qTotalRecycled != null)
        {
            var qGroupRecycled = querySubMaterial.Where(w => w.nUnderbyProductID == 41).GroupBy(g => new
            {
                g.nUnderbyProductID,
                g.nUnitID,
                g.sName
            }).Select(s => new
            {
                s.Key.nUnderbyProductID,
                s.Key.nUnitID,
                s.Key.sName,
                nDesnsity = s.Sum(x => SystemFunction.GetDecimalNull(x.Density)),
                sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
                sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
                sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
                sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
                sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
                sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
                sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
                sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
                sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
                sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
                sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
                sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : ""
            }).OrderBy(o => o.sName).ToList();

            #region Calculate Total Recycled
            decimal? nM1 = null, nM2 = null, nM3 = null, nM4 = null, nM5 = null, nM6 = null, nM7 = null, nM8 = null, nM9 = null, nM10 = null, nM11 = null, nM12 = null;
            foreach (var item in qGroupRecycled)
            {
                decimal? nValM1 = SystemFunction.GetDecimalNull(item.sM1);
                decimal? nValM2 = SystemFunction.GetDecimalNull(item.sM2);
                decimal? nValM3 = SystemFunction.GetDecimalNull(item.sM3);
                decimal? nValM4 = SystemFunction.GetDecimalNull(item.sM4);
                decimal? nValM5 = SystemFunction.GetDecimalNull(item.sM5);
                decimal? nValM6 = SystemFunction.GetDecimalNull(item.sM6);
                decimal? nValM7 = SystemFunction.GetDecimalNull(item.sM7);
                decimal? nValM8 = SystemFunction.GetDecimalNull(item.sM8);
                decimal? nValM9 = SystemFunction.GetDecimalNull(item.sM9);
                decimal? nValM10 = SystemFunction.GetDecimalNull(item.sM10);
                decimal? nValM11 = SystemFunction.GetDecimalNull(item.sM11);
                decimal? nValM12 = SystemFunction.GetDecimalNull(item.sM12);
                if (nM1.HasValue || nValM1.HasValue)
                    nM1 = (nM1 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM1) : item.nUnitID == 3 ? nValM1 * nFactorKGtoTonnes : (nValM1 ?? 0));

                if (nM2.HasValue || nValM2.HasValue)
                    nM2 = (nM2 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM2) : item.nUnitID == 3 ? nValM2 * nFactorKGtoTonnes : (nValM2 ?? 0));

                if (nM3.HasValue || nValM3.HasValue)
                    nM3 = (nM3 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM3) : item.nUnitID == 3 ? nValM3 * nFactorKGtoTonnes : (nValM3 ?? 0));

                if (nM4.HasValue || nValM4.HasValue)
                    nM4 = (nM4 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM4) : item.nUnitID == 3 ? nValM4 * nFactorKGtoTonnes : (nValM4 ?? 0));

                if (nM5.HasValue || nValM5.HasValue)
                    nM5 = (nM5 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM5) : item.nUnitID == 3 ? nValM5 * nFactorKGtoTonnes : (nValM5 ?? 0));

                if (nM6.HasValue || nValM6.HasValue)
                    nM6 = (nM6 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM6) : item.nUnitID == 3 ? nValM6 * nFactorKGtoTonnes : (nValM6 ?? 0));

                if (nM7.HasValue || nValM7.HasValue)
                    nM7 = (nM7 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM7) : item.nUnitID == 3 ? nValM7 * nFactorKGtoTonnes : (nValM7 ?? 0));

                if (nM8.HasValue || nValM8.HasValue)
                    nM8 = (nM8 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM8) : item.nUnitID == 3 ? nValM8 * nFactorKGtoTonnes : (nValM8 ?? 0));

                if (nM9.HasValue || nValM9.HasValue)
                    nM9 = (nM9 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM9) : item.nUnitID == 3 ? nValM9 * nFactorKGtoTonnes : (nValM9 ?? 0));

                if (nM10.HasValue || nValM10.HasValue)
                    nM10 = (nM10 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM10) : item.nUnitID == 3 ? nValM10 * nFactorKGtoTonnes : (nValM10 ?? 0));

                if (nM11.HasValue || nValM11.HasValue)
                    nM11 = (nM11 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM11) : item.nUnitID == 3 ? nValM11 * nFactorKGtoTonnes : (nValM11 ?? 0));

                if (nM12.HasValue || nValM12.HasValue)
                    nM12 = (nM12 ?? 0) + (item.nUnitID == 1 ? (item.nDesnsity ?? 0) * (nValM12) : item.nUnitID == 3 ? nValM12 * nFactorKGtoTonnes : (nValM12 ?? 0));
            }
            qTotalRecycled.sM1 = nM1 + "";
            qTotalRecycled.sM2 = nM2 + "";
            qTotalRecycled.sM3 = nM3 + "";
            qTotalRecycled.sM4 = nM4 + "";
            qTotalRecycled.sM5 = nM5 + "";
            qTotalRecycled.sM6 = nM6 + "";
            qTotalRecycled.sM7 = nM7 + "";
            qTotalRecycled.sM8 = nM8 + "";
            qTotalRecycled.sM9 = nM9 + "";
            qTotalRecycled.sM10 = nM10 + "";
            qTotalRecycled.sM11 = nM11 + "";
            qTotalRecycled.sM12 = nM12 + "";
            #endregion
            lstRecycled.Add(qTotalRecycled);//Add Total Recycled

            //Add Sub Non-renewable
            foreach (var item in qGroupRecycled)
            {
                lstRecycled.Add(new TDataMaterial
                {
                    nHeaderID = 41,
                    sProductName = item.sName,
                    UnitID = item.nUnitID ?? 0,
                    sUnit = GetUnitName(item.nUnitID ?? 0),
                    sDensity = item.nDesnsity + "",
                    sM1 = item.sM1,
                    sM2 = item.sM2,
                    sM3 = item.sM3,
                    sM4 = item.sM4,
                    sM5 = item.sM5,
                    sM6 = item.sM6,
                    sM7 = item.sM7,
                    sM8 = item.sM8,
                    sM9 = item.sM9,
                    sM10 = item.sM10,
                    sM11 = item.sM11,
                    sM12 = item.sM12,
                });
            }
        }
        #endregion

        #region Total Materials Used & Add to list
        var qTotalMaterialUsed = queryMainMaterial.FirstOrDefault(w => w.nProductID == 33);
        if (qTotalMaterialUsed != null && qTotalDirect != null && qTotalAssociated != null)
        {
            qTotalMaterialUsed.sM1 = SystemFunction.SumDataToDecimal(new List<string>() { qTotalDirect.sM1, qTotalAssociated.sM1 }) + "";
            qTotalMaterialUsed.sM2 = SystemFunction.SumDataToDecimal(new List<string>() { qTotalDirect.sM2, qTotalAssociated.sM2 }) + "";
            qTotalMaterialUsed.sM3 = SystemFunction.SumDataToDecimal(new List<string>() { qTotalDirect.sM3, qTotalAssociated.sM3 }) + "";
            qTotalMaterialUsed.sM4 = SystemFunction.SumDataToDecimal(new List<string>() { qTotalDirect.sM4, qTotalAssociated.sM4 }) + "";
            qTotalMaterialUsed.sM5 = SystemFunction.SumDataToDecimal(new List<string>() { qTotalDirect.sM5, qTotalAssociated.sM5 }) + "";
            qTotalMaterialUsed.sM6 = SystemFunction.SumDataToDecimal(new List<string>() { qTotalDirect.sM6, qTotalAssociated.sM6 }) + "";
            qTotalMaterialUsed.sM7 = SystemFunction.SumDataToDecimal(new List<string>() { qTotalDirect.sM7, qTotalAssociated.sM7 }) + "";
            qTotalMaterialUsed.sM8 = SystemFunction.SumDataToDecimal(new List<string>() { qTotalDirect.sM8, qTotalAssociated.sM8 }) + "";
            qTotalMaterialUsed.sM9 = SystemFunction.SumDataToDecimal(new List<string>() { qTotalDirect.sM9, qTotalAssociated.sM9 }) + "";
            qTotalMaterialUsed.sM10 = SystemFunction.SumDataToDecimal(new List<string>() { qTotalDirect.sM10, qTotalAssociated.sM10 }) + "";
            qTotalMaterialUsed.sM11 = SystemFunction.SumDataToDecimal(new List<string>() { qTotalDirect.sM11, qTotalAssociated.sM11 }) + "";
            qTotalMaterialUsed.sM12 = SystemFunction.SumDataToDecimal(new List<string>() { qTotalDirect.sM12, qTotalAssociated.sM12 }) + "";

            lstDataMaterial.Add(qTotalMaterialUsed);

            lstDataMaterial.Add(qTotalDirect);
            foreach (var item in lstRenewDirect)
            {
                lstDataMaterial.Add(item);
            }
            foreach (var item in lstNonRenewDirect)
            {
                lstDataMaterial.Add(item);
            }

            lstDataMaterial.Add(qTotalAssociated);
            foreach (var item in lstCatalyst)
            {
                lstDataMaterial.Add(item);
            }
            foreach (var item in lstRenewAsso)
            {
                lstDataMaterial.Add(item);
            }
            foreach (var item in lstNonRenewAsso)
            {
                lstDataMaterial.Add(item);
            }

            foreach (var item in lstRecycled)
            {
                lstDataMaterial.Add(item);
            }

        }
        #endregion

        foreach (var item in lstDataMaterial)
        {
            item.sTotal = SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";
            item.sShowM1 = SystemFunction.ConvertFormatDecimal4(item.sM1);
            item.sShowM2 = SystemFunction.ConvertFormatDecimal4(item.sM2);
            item.sShowM3 = SystemFunction.ConvertFormatDecimal4(item.sM3);
            item.sShowM4 = SystemFunction.ConvertFormatDecimal4(item.sM4);
            item.sShowM5 = SystemFunction.ConvertFormatDecimal4(item.sM5);
            item.sShowM6 = SystemFunction.ConvertFormatDecimal4(item.sM6);
            item.sShowM7 = SystemFunction.ConvertFormatDecimal4(item.sM7);
            item.sShowM8 = SystemFunction.ConvertFormatDecimal4(item.sM8);
            item.sShowM9 = SystemFunction.ConvertFormatDecimal4(item.sM9);
            item.sShowM10 = SystemFunction.ConvertFormatDecimal4(item.sM10);
            item.sShowM11 = SystemFunction.ConvertFormatDecimal4(item.sM11);
            item.sShowM12 = SystemFunction.ConvertFormatDecimal4(item.sM12);
            item.sShowTotal = SystemFunction.ConvertFormatDecimal4(item.sTotal);
        }

        result.lstDataMaterial = lstDataMaterial;
        return result;
    }

    private CResultGetDataPageLoad GetDataWaste(int nYear, int nMainFacID, List<TDataSubFac> lstDataSubFac)
    {
        CResultGetDataPageLoad result = new CResultGetDataPageLoad();
        List<TDataWaste> lstWaste = new List<TDataWaste>();
        string sYear = nYear + "";
        var lstSubFacID = lstDataSubFac.GroupBy(g => g.FacilityID).Select(s => s.Key).ToList();
        var dataEPIFrom = db.TEPI_Forms.Where(w => w.sYear == sYear && w.IDIndicator == DataType.Indicator.Waste && lstSubFacID.Contains(w.FacilityID ?? 0)).ToList();
        #region Query
        var queryMain = (from epi in dataEPIFrom
                         from d in db.TWaste_Product.Where(w => w.FormID == epi.FormID).AsEnumerable()
                         from i in db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.Waste && w.ProductID == d.ProductID)
                         orderby i.nOrder
                         select new TDataWaste
                         {
                             nProductID = i.ProductID,
                             sProductName = i.ProductName,
                             cTotal = i.cTotal,
                             cTotalAll = i.cTotalAll,
                             sUnit = i.sUnit,
                             nGroupCal = i.nGroupCalc ?? 0,
                             sType = i.sType,
                             nOrder = i.nOrder,

                             sM1 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M1 : "",
                             sM2 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M2 : "",
                             sM3 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M3 : "",

                             sM4 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M4 : "",
                             sM5 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M5 : "",
                             sM6 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M6 : "",

                             sM7 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M7 : "",
                             sM8 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M8 : "",
                             sM9 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M9 : "",

                             sM10 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M10 : "",
                             sM11 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M11 : "",
                             sM12 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M12 : "",

                             sTotal = d.nTotal,
                             sPreviousYear = d.PreviousYear,
                             sReportingYear = d.ReportingYear,
                         }).ToList();

        var querySub = (from epi in dataEPIFrom
                        from d in db.TWaste_Product_data.Where(w => w.FormID == epi.FormID)
                        select new
                        {
                            d.UnderProductID,
                            d.ProductName
                        }).ToList();

        lstWaste = queryMain.GroupBy(g => new { g.nProductID, g.sProductName, g.cTotalAll, g.cTotal, g.nGroupCal, g.sType, g.nOrder }).Select(s => new TDataWaste
        {
            nProductID = s.Key.nProductID,
            sProductName = s.Key.sProductName,
            cTotalAll = s.Key.cTotalAll,
            cTotal = s.Key.cTotal,
            nGroupCal = s.Key.nGroupCal,
            sType = s.Key.sType,
            nOrder = s.Key.nOrder,
            UnitID = (s.Key.cTotal == "Y" || s.Key.nGroupCal == 99 || s.Key.nGroupCal == 12) ? 2 : querySub.Any(x => x.UnderProductID == s.Key.nProductID) ? 2 : 0,//2 = Tonnes, 0 = N/A
            sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
            sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
            sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
            sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
            sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
            sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
            sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
            sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
            sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
            sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
            sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
            sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
            sPreviousYear = s.Sum(x => SystemFunction.GetDecimalNull(x.sPreviousYear)) + "",
            sReportingYear = s.Sum(x => SystemFunction.GetDecimalNull(x.sReportingYear)) + "",
            sTotal = s.Sum(x => SystemFunction.GetDecimalNull(x.sTotal)) + "",
        }).OrderBy(o => o.nOrder).ToList();
        #endregion

        var dataUnit = db.mTUnit.ToList();
        Func<int, string> GetUnitName = (id) =>
        {
            var q = dataUnit.FirstOrDefault(w => w.UnitID == id);
            return q != null ? q.UnitName : "";
        };

        foreach (var item in lstWaste)
        {
            if (item.nGroupCal != 99 && item.nGroupCal != 12)
            {
                item.sTotal = SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";
                item.sShowTotal = SystemFunction.ConvertFormatDecimal4(item.sTotal);
            }
            else
            {
                item.sShowPreviousYear = SystemFunction.ConvertFormatDecimal4(item.sPreviousYear);
                item.sShowReportingYear = SystemFunction.ConvertFormatDecimal4(item.sReportingYear);
                item.sShowTotal = SystemFunction.ConvertFormatDecimal4(item.sTotal);
            }

            item.sUnit = GetUnitName(item.UnitID);
            item.sShowM1 = SystemFunction.ConvertFormatDecimal4(item.sM1);
            item.sShowM2 = SystemFunction.ConvertFormatDecimal4(item.sM2);
            item.sShowM3 = SystemFunction.ConvertFormatDecimal4(item.sM3);
            item.sShowM4 = SystemFunction.ConvertFormatDecimal4(item.sM4);
            item.sShowM5 = SystemFunction.ConvertFormatDecimal4(item.sM5);
            item.sShowM6 = SystemFunction.ConvertFormatDecimal4(item.sM6);
            item.sShowM7 = SystemFunction.ConvertFormatDecimal4(item.sM7);
            item.sShowM8 = SystemFunction.ConvertFormatDecimal4(item.sM8);
            item.sShowM9 = SystemFunction.ConvertFormatDecimal4(item.sM9);
            item.sShowM10 = SystemFunction.ConvertFormatDecimal4(item.sM10);
            item.sShowM11 = SystemFunction.ConvertFormatDecimal4(item.sM11);
            item.sShowM12 = SystemFunction.ConvertFormatDecimal4(item.sM12);
        }
        result.lstDataWaste = lstWaste;
        return result;
    }

    private CResultGetDataPageLoad GetDataEmission(int nYear, int nMainFacID, List<TDataSubFac> lstDataSubFac)
    {
        #region NOTE
        /*
         * ส่งเฉพาะ Stack ที่เป็น Combussion เท่านั้น
         * TEmission_Product เก็บข้อมูลเฉพาะที่เป็น Combussion อยู่แล้ว
         */
        #endregion
        CResultGetDataPageLoad result = new CResultGetDataPageLoad();
        List<TDataEmission_Product> lstDataCombusion = new List<TDataEmission_Product>();
        string sYear = nYear + "";
        var lstSubFacID = lstDataSubFac.GroupBy(g => g.FacilityID).Select(s => s.Key).ToList();
        var dataEPIFrom = db.TEPI_Forms.Where(w => w.sYear == sYear && w.IDIndicator == DataType.Indicator.Emission && lstSubFacID.Contains(w.FacilityID ?? 0)).ToList();

        #region Table Summary
        #region Query
        var queryMain = (from epi in dataEPIFrom
                         from d in db.TEmission_Product.Where(w => w.FormID == epi.FormID).AsEnumerable()
                         from i in db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.Emission && w.ProductID == d.ProductID)
                         orderby i.nOrder
                         select new TDataEmission_Product
                         {
                             nProductID = i.ProductID,
                             sProductName = i.ProductName,
                             cTotal = i.cTotal,
                             cTotalAll = i.cTotalAll,
                             sUnit = i.sUnit,
                             nGroupCal = i.nGroupCalc ?? 0,
                             sType = i.sType,
                             nOrder = i.nOrder,

                             sM1 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M1 : "",
                             sM2 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M2 : "",
                             sM3 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M3 : "",

                             sM4 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M4 : "",
                             sM5 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M5 : "",
                             sM6 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M6 : "",

                             sM7 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M7 : "",
                             sM8 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M8 : "",
                             sM9 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M9 : "",

                             sM10 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M10 : "",
                             sM11 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M11 : "",
                             sM12 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M12 : "",

                             sTotal = d.nTotal,
                         }).ToList();

        lstDataCombusion = queryMain.GroupBy(g => new { g.nProductID, g.sProductName, g.cTotalAll, g.cTotal, g.nGroupCal, g.sType, g.nOrder, g.sUnit }).Select(s => new TDataEmission_Product
        {
            nProductID = s.Key.nProductID,
            sProductName = s.Key.sProductName,
            cTotalAll = s.Key.cTotalAll,
            cTotal = s.Key.cTotal,
            nGroupCal = s.Key.nGroupCal,
            sType = s.Key.sType,
            nOrder = s.Key.nOrder,
            sUnit = s.Key.sUnit,
            sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
            sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
            sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
            sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
            sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
            sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
            sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
            sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
            sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
            sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
            sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
            sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
            sTotal = s.Sum(x => SystemFunction.GetDecimalNull(x.sTotal)) + "",
        }).OrderBy(o => o.nOrder).ToList();
        #endregion

        foreach (var item in lstDataCombusion)
        {
            item.sTotal = SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";
            item.sShowM1 = SystemFunction.ConvertFormatDecimal4(item.sM1);
            item.sShowM2 = SystemFunction.ConvertFormatDecimal4(item.sM2);
            item.sShowM3 = SystemFunction.ConvertFormatDecimal4(item.sM3);
            item.sShowM4 = SystemFunction.ConvertFormatDecimal4(item.sM4);
            item.sShowM5 = SystemFunction.ConvertFormatDecimal4(item.sM5);
            item.sShowM6 = SystemFunction.ConvertFormatDecimal4(item.sM6);
            item.sShowM7 = SystemFunction.ConvertFormatDecimal4(item.sM7);
            item.sShowM8 = SystemFunction.ConvertFormatDecimal4(item.sM8);
            item.sShowM9 = SystemFunction.ConvertFormatDecimal4(item.sM9);
            item.sShowM10 = SystemFunction.ConvertFormatDecimal4(item.sM10);
            item.sShowM11 = SystemFunction.ConvertFormatDecimal4(item.sM11);
            item.sShowM12 = SystemFunction.ConvertFormatDecimal4(item.sM12);
            item.sShowTotal = SystemFunction.ConvertFormatDecimal4(item.sTotal);
        }
        #endregion

        #region Stack >> Combusion Only
        #region Query Stack
        var dataStack = (from epi in dataEPIFrom
                         from stk in db.TEmission_Stack.Where(w => w.FormID == epi.FormID && w.sStackType == "CMS")//Combussion Only
                         from f in db.mTFacility.Where(w => w.ID == epi.FacilityID)
                         select new
                         {
                             epi.FormID,
                             epi.FacilityID,
                             stk.nStackID,
                             stk.sStackName,
                             stk.sRemark,
                             sFacName = f.Name
                         }).OrderBy(o => o.sStackName).ThenBy(o => o.sFacName).ToList();
        var dataStackProduct = (from epi in dataEPIFrom
                                from stk in db.TEmission_Product_Stack.Where(w => w.FormID == epi.FormID)
                                from p in db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.Emission && w.ProductID == stk.ProductID && (w.sType == "2" || w.sType == "2H"))
                                select new
                                {
                                    epi.FormID,
                                    stk.nStackID,
                                    stk.ProductID,
                                    stk.nOptionProduct,
                                    p.sType,
                                    stk.M1,
                                    stk.M2,
                                    stk.M3,
                                    stk.M4,
                                    stk.M5,
                                    stk.M6,
                                    stk.M7,
                                    stk.M8,
                                    stk.M9,
                                    stk.M10,
                                    stk.M11,
                                    stk.M12,
                                }).ToList();
        #endregion

        List<TDataEmission_Stack> lstDataStack = new List<TDataEmission_Stack>();
        foreach (var item in dataStack)
        {
            List<TDataEmission_Product> lstStackProduct = new List<TDataEmission_Product>();
            lstStackProduct = dataStackProduct.Where(w => w.FormID == item.FormID && w.nStackID == item.nStackID).Select(s => new TDataEmission_Product
                {
                    nProductID = s.ProductID,
                    nOptionProduct = s.nOptionProduct,
                    sM1 = lstDataSubFac.Any(x => x.FacilityID == item.FacilityID && x.nQuarter == 1) ? s.M1 : "",
                    sM2 = lstDataSubFac.Any(x => x.FacilityID == item.FacilityID && x.nQuarter == 1) ? s.M2 : "",
                    sM3 = lstDataSubFac.Any(x => x.FacilityID == item.FacilityID && x.nQuarter == 1) ? s.M3 : "",

                    sM4 = lstDataSubFac.Any(x => x.FacilityID == item.FacilityID && x.nQuarter == 2) ? s.M4 : "",
                    sM5 = lstDataSubFac.Any(x => x.FacilityID == item.FacilityID && x.nQuarter == 2) ? s.M5 : "",
                    sM6 = lstDataSubFac.Any(x => x.FacilityID == item.FacilityID && x.nQuarter == 2) ? s.M6 : "",

                    sM7 = lstDataSubFac.Any(x => x.FacilityID == item.FacilityID && x.nQuarter == 3) ? s.M7 : "",
                    sM8 = lstDataSubFac.Any(x => x.FacilityID == item.FacilityID && x.nQuarter == 3) ? s.M8 : "",
                    sM9 = lstDataSubFac.Any(x => x.FacilityID == item.FacilityID && x.nQuarter == 3) ? s.M9 : "",

                    sM10 = lstDataSubFac.Any(x => x.FacilityID == item.FacilityID && x.nQuarter == 4) ? s.M10 : "",
                    sM11 = lstDataSubFac.Any(x => x.FacilityID == item.FacilityID && x.nQuarter == 4) ? s.M11 : "",
                    sM12 = lstDataSubFac.Any(x => x.FacilityID == item.FacilityID && x.nQuarter == 4) ? s.M12 : "",
                }).ToList();
            lstDataStack.Add(new TDataEmission_Stack
                {
                    sStackName = item.sStackName + "(" + item.sFacName + ")",
                    lstProduct = lstStackProduct,
                    sRemark = "",
                });
        }
        #endregion

        #region VOC
        var dataVOCMonthly = (from epi in dataEPIFrom
                              from d in db.TEmission_VOC.Where(w => w.FormID == epi.FormID && w.sOption == "M")
                              select new TDataEmission_Product
                              {
                                  nProductID = d.ProductID,
                                  sOption = d.sOption,
                                  sM1 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M1 : "",
                                  sM2 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M2 : "",
                                  sM3 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M3 : "",

                                  sM4 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M4 : "",
                                  sM5 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M5 : "",
                                  sM6 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M6 : "",

                                  sM7 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M7 : "",
                                  sM8 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M8 : "",
                                  sM9 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M9 : "",

                                  sM10 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M10 : "",
                                  sM11 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M11 : "",
                                  sM12 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M12 : "",
                              }).ToList();

        var dataVOCYearly = (from epi in dataEPIFrom
                             from d in db.TEmission_VOC.Where(w => w.FormID == epi.FormID && w.sOption == "Y")
                             select new TDataEmission_Product
                             {
                                 nProductID = d.ProductID,
                                 sOption = d.sOption,
                                 sM1 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? (SystemFunction.GetDecimalNull(d.nTotal) / 12) + "" : "",
                                 sM2 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? (SystemFunction.GetDecimalNull(d.nTotal) / 12) + "" : "",
                                 sM3 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? (SystemFunction.GetDecimalNull(d.nTotal) / 12) + "" : "",

                                 sM4 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? (SystemFunction.GetDecimalNull(d.nTotal) / 12) + "" : "",
                                 sM5 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? (SystemFunction.GetDecimalNull(d.nTotal) / 12) + "" : "",
                                 sM6 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? (SystemFunction.GetDecimalNull(d.nTotal) / 12) + "" : "",

                                 sM7 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? (SystemFunction.GetDecimalNull(d.nTotal) / 12) + "" : "",
                                 sM8 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? (SystemFunction.GetDecimalNull(d.nTotal) / 12) + "" : "",
                                 sM9 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? (SystemFunction.GetDecimalNull(d.nTotal) / 12) + "" : "",

                                 sM10 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? (SystemFunction.GetDecimalNull(d.nTotal) / 12) + "" : "",
                                 sM11 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? (SystemFunction.GetDecimalNull(d.nTotal) / 12) + "" : "",
                                 sM12 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? (SystemFunction.GetDecimalNull(d.nTotal) / 12) + "" : "",

                                 sTotal = d.nTotal,
                             }).ToList();

        var qCheckOption = (from epi in dataEPIFrom
                            from d in db.TEmission_VOC.Where(w => w.FormID == epi.FormID)
                            group new { d } by new { d.sOption } into grp
                            select new
                            {
                                grp.Key.sOption
                            }).ToList();

        List<TDataEmission_Product> lstDataVOC = new List<TDataEmission_Product>();
        if (qCheckOption.Count == 1)
        {
            #region กรณีที่ทุก Facility เลือก Option เดียวกันทั้งหมด
            string sOption = qCheckOption.FirstOrDefault().sOption;
            if (sOption == "M")
            {
                lstDataVOC = dataVOCMonthly.GroupBy(g => new { g.nProductID, g.sOption }).Select(s => new TDataEmission_Product
                    {
                        nProductID = s.Key.nProductID,
                        sOption = s.Key.sOption,
                        sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
                        sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
                        sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
                        sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
                        sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
                        sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
                        sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
                        sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
                        sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
                        sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
                        sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
                        sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
                    }).ToList();
            }
            else if (sOption == "Y")
            {
                lstDataVOC = dataVOCYearly.GroupBy(g => new { g.nProductID, g.sOption }).Select(s => new TDataEmission_Product
                {
                    nProductID = s.Key.nProductID,
                    sOption = s.Key.sOption,
                    sTotal = s.Any(x => SystemFunction.GetDecimalNull(x.sTotal).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sTotal)) + "" : s.Any(x => x.sTotal == sNAFormat) ? "N/A" : "",
                }).ToList();
            }
            #endregion
        }
        else
        {
            #region กรณีที่มีทั้ง Monthly and Yearly จะทำเป็น Monthly
            lstDataVOC = dataVOCMonthly.GroupBy(g => new { g.nProductID, g.sOption }).Select(s => new TDataEmission_Product
            {
                nProductID = s.Key.nProductID,
                sOption = s.Key.sOption,
                sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
                sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
                sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
                sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
                sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
                sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
                sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
                sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
                sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
                sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
                sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
                sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
            }).ToList();

            var qSumVOCYearly = dataVOCYearly.GroupBy(g => new { g.nProductID, g.sOption }).Select(s => new TDataEmission_Product
            {
                nProductID = s.Key.nProductID,
                sOption = s.Key.sOption,
                sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
                sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
                sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
                sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
                sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
                sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
                sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
                sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
                sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
                sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
                sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
                sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
            }).ToList();

            foreach (var item in lstDataVOC)
            {
                var qData = qSumVOCYearly.FirstOrDefault(w => w.nProductID == item.nProductID);
                if (qData != null)
                {
                    decimal? nValM1 = SystemFunction.SumDataToDecimal(new List<string> { item.sM1, qData.sM1 });
                    decimal? nValM2 = SystemFunction.SumDataToDecimal(new List<string> { item.sM2, qData.sM2 });
                    decimal? nValM3 = SystemFunction.SumDataToDecimal(new List<string> { item.sM3, qData.sM3 });
                    decimal? nValM4 = SystemFunction.SumDataToDecimal(new List<string> { item.sM4, qData.sM4 });
                    decimal? nValM5 = SystemFunction.SumDataToDecimal(new List<string> { item.sM5, qData.sM5 });
                    decimal? nValM6 = SystemFunction.SumDataToDecimal(new List<string> { item.sM6, qData.sM6 });
                    decimal? nValM7 = SystemFunction.SumDataToDecimal(new List<string> { item.sM7, qData.sM7 });
                    decimal? nValM8 = SystemFunction.SumDataToDecimal(new List<string> { item.sM8, qData.sM8 });
                    decimal? nValM9 = SystemFunction.SumDataToDecimal(new List<string> { item.sM9, qData.sM9 });
                    decimal? nValM10 = SystemFunction.SumDataToDecimal(new List<string> { item.sM10, qData.sM10 });
                    decimal? nValM11 = SystemFunction.SumDataToDecimal(new List<string> { item.sM11, qData.sM11 });
                    decimal? nValM12 = SystemFunction.SumDataToDecimal(new List<string> { item.sM12, qData.sM12 });

                    if (nValM1.HasValue)
                        item.sM1 = nValM1 + "";
                    else
                        item.sM1 = item.sM1 == sNAFormat || qData.sM1 == sNAFormat ? sNAFormat : item.sM1;

                    if (nValM2.HasValue)
                        item.sM2 = nValM2 + "";
                    else
                        item.sM2 = item.sM2 == sNAFormat || qData.sM2 == sNAFormat ? sNAFormat : item.sM2;

                    if (nValM3.HasValue)
                        item.sM3 = nValM3 + "";
                    else
                        item.sM3 = item.sM3 == sNAFormat || qData.sM3 == sNAFormat ? sNAFormat : item.sM3;

                    if (nValM4.HasValue)
                        item.sM4 = nValM4 + "";
                    else
                        item.sM4 = item.sM4 == sNAFormat || qData.sM4 == sNAFormat ? sNAFormat : item.sM4;

                    if (nValM5.HasValue)
                        item.sM5 = nValM5 + "";
                    else
                        item.sM5 = item.sM5 == sNAFormat || qData.sM5 == sNAFormat ? sNAFormat : item.sM5;

                    if (nValM6.HasValue)
                        item.sM6 = nValM6 + "";
                    else
                        item.sM6 = item.sM6 == sNAFormat || qData.sM6 == sNAFormat ? sNAFormat : item.sM6;

                    if (nValM7.HasValue)
                        item.sM7 = nValM7 + "";
                    else
                        item.sM7 = item.sM7 == sNAFormat || qData.sM7 == sNAFormat ? sNAFormat : item.sM7;

                    if (nValM8.HasValue)
                        item.sM8 = nValM8 + "";
                    else
                        item.sM8 = item.sM8 == sNAFormat || qData.sM8 == sNAFormat ? sNAFormat : item.sM8;

                    if (nValM9.HasValue)
                        item.sM9 = nValM9 + "";
                    else
                        item.sM9 = item.sM9 == sNAFormat || qData.sM9 == sNAFormat ? sNAFormat : item.sM9;

                    if (nValM10.HasValue)
                        item.sM10 = nValM10 + "";
                    else
                        item.sM10 = item.sM10 == sNAFormat || qData.sM10 == sNAFormat ? sNAFormat : item.sM10;

                    if (nValM11.HasValue)
                        item.sM11 = nValM11 + "";
                    else
                        item.sM11 = item.sM11 == sNAFormat || qData.sM11 == sNAFormat ? sNAFormat : item.sM11;

                    if (nValM12.HasValue)
                        item.sM12 = nValM12 + "";
                    else
                        item.sM12 = item.sM12 == sNAFormat || qData.sM12 == sNAFormat ? sNAFormat : item.sM12;
                }
            }
            #endregion
        }
        lstDataVOC = (from l in lstDataVOC
                      from p in db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.Emission && w.ProductID == l.nProductID && w.sType == "VOC").AsEnumerable()
                      select new TDataEmission_Product
                      {
                          nProductID = p.ProductID,
                          sProductName = p.ProductName,
                          sUnit = p.sUnit,
                          cTotal = p.cTotal,
                          cTotalAll = p.cTotalAll,
                          sOption = l.sOption,
                          sM1 = l.sM1,
                          sM2 = l.sM2,
                          sM3 = l.sM3,
                          sM4 = l.sM4,
                          sM5 = l.sM5,
                          sM6 = l.sM6,
                          sM7 = l.sM7,
                          sM8 = l.sM8,
                          sM9 = l.sM9,
                          sM10 = l.sM10,
                          sM11 = l.sM11,
                          sM12 = l.sM12,
                          sShowM1 = SystemFunction.ConvertFormatDecimal4(l.sM1),
                          sShowM2 = SystemFunction.ConvertFormatDecimal4(l.sM2),
                          sShowM3 = SystemFunction.ConvertFormatDecimal4(l.sM3),
                          sShowM4 = SystemFunction.ConvertFormatDecimal4(l.sM4),
                          sShowM5 = SystemFunction.ConvertFormatDecimal4(l.sM5),
                          sShowM6 = SystemFunction.ConvertFormatDecimal4(l.sM6),
                          sShowM7 = SystemFunction.ConvertFormatDecimal4(l.sM7),
                          sShowM8 = SystemFunction.ConvertFormatDecimal4(l.sM8),
                          sShowM9 = SystemFunction.ConvertFormatDecimal4(l.sM9),
                          sShowM10 = SystemFunction.ConvertFormatDecimal4(l.sM10),
                          sShowM11 = SystemFunction.ConvertFormatDecimal4(l.sM11),
                          sShowM12 = SystemFunction.ConvertFormatDecimal4(l.sM12),

                          sTotal = l.sOption == "M" ? SystemFunction.SumDataToDecimal(new List<string>() { l.sM1, l.sM2, l.sM3, l.sM4, l.sM5, l.sM6, l.sM7, l.sM8, l.sM9, l.sM10, l.sM12, l.sM12 }) + "" : l.sTotal,
                          sShowTotal = SystemFunction.ConvertFormatDecimal4(l.sOption == "M" ? SystemFunction.SumDataToDecimal(new List<string>() { l.sM1, l.sM2, l.sM3, l.sM4, l.sM5, l.sM6, l.sM7, l.sM8, l.sM9, l.sM10, l.sM12, l.sM12 }) + "" : l.sTotal)
                      }).ToList();
        #endregion

        result.lstDataEmissionCombusion = lstDataCombusion;
        result.lstDataEmissionStack = lstDataStack;
        result.lstDataEmissionVOC = lstDataVOC;
        return result;
    }

    private CResultGetDataPageLoad GetDataEffluent(int nYear, int nMainFacID, List<TDataSubFac> lstDataSubFac)
    {
        CResultGetDataPageLoad result = new CResultGetDataPageLoad();
        string sYear = nYear + "";
        var lstSubFacID = lstDataSubFac.GroupBy(g => g.FacilityID).Select(s => s.Key).ToList();
        var dataEPIFrom = db.TEPI_Forms.Where(w => w.sYear == sYear && w.IDIndicator == DataType.Indicator.Effluent && lstSubFacID.Contains(w.FacilityID ?? 0)).ToList();
        List<TDataEffluent_Product> lstDataSummary = new List<TDataEffluent_Product>();
        List<TDataEffluent_Product> lstDataOutput = new List<TDataEffluent_Product>();
        List<TDataEffluent_Product> lstDataPointProduct = new List<TDataEffluent_Product>();
        List<TDataEffluent_Point> lstDataPoint = new List<TDataEffluent_Point>();

        #region Query Table Summary
        var queryMain = (from epi in dataEPIFrom
                         from d in db.TEffluent_Product.Where(w => w.FormID == epi.FormID).AsEnumerable()
                         from i in db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.Effluent && w.ProductID == d.ProductID && (w.sType == "1" || w.sType == "SP"))
                         orderby i.nOrder
                         select new TDataEffluent_Product
                         {
                             nProductID = i.ProductID,
                             sProductName = i.ProductName,
                             cTotal = i.cTotal,
                             cTotalAll = i.cTotalAll,
                             sUnit = i.sUnit,
                             sType = i.sType,
                             nOrder = i.nOrder,

                             sM1 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M1 : "",
                             sM2 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M2 : "",
                             sM3 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M3 : "",

                             sM4 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M4 : "",
                             sM5 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M5 : "",
                             sM6 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M6 : "",

                             sM7 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M7 : "",
                             sM8 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M8 : "",
                             sM9 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M9 : "",

                             sM10 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M10 : "",
                             sM11 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M11 : "",
                             sM12 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M12 : "",

                             sTotal = d.nTotal,
                         }).ToList();

        lstDataSummary = queryMain.GroupBy(g => new { g.nProductID, g.sProductName, g.cTotalAll, g.cTotal, g.sType, g.nOrder, g.sUnit }).Select(s => new TDataEffluent_Product
        {
            nProductID = s.Key.nProductID,
            sProductName = s.Key.sProductName,
            cTotalAll = s.Key.cTotalAll,
            cTotal = s.Key.cTotal,
            sType = s.Key.sType,
            nOrder = s.Key.nOrder,
            sUnit = s.Key.sUnit,
            sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
            sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
            sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
            sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
            sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
            sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
            sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
            sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
            sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
            sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
            sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
            sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
            sTotal = s.Sum(x => SystemFunction.GetDecimalNull(x.sTotal)) + "",
        }).OrderBy(o => o.nOrder).ToList();

        foreach (var item in lstDataSummary)
        {
            item.sTotal = SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";
            item.sShowM1 = SystemFunction.ConvertFormatDecimal4(item.sM1);
            item.sShowM2 = SystemFunction.ConvertFormatDecimal4(item.sM2);
            item.sShowM3 = SystemFunction.ConvertFormatDecimal4(item.sM3);
            item.sShowM4 = SystemFunction.ConvertFormatDecimal4(item.sM4);
            item.sShowM5 = SystemFunction.ConvertFormatDecimal4(item.sM5);
            item.sShowM6 = SystemFunction.ConvertFormatDecimal4(item.sM6);
            item.sShowM7 = SystemFunction.ConvertFormatDecimal4(item.sM7);
            item.sShowM8 = SystemFunction.ConvertFormatDecimal4(item.sM8);
            item.sShowM9 = SystemFunction.ConvertFormatDecimal4(item.sM9);
            item.sShowM10 = SystemFunction.ConvertFormatDecimal4(item.sM10);
            item.sShowM11 = SystemFunction.ConvertFormatDecimal4(item.sM11);
            item.sShowM12 = SystemFunction.ConvertFormatDecimal4(item.sM12);
            item.sShowTotal = SystemFunction.ConvertFormatDecimal4(item.sTotal);
        }
        #endregion

        #region Query Output
        var queryOutput = (from epi in dataEPIFrom
                           from d in db.TProductOutput.Where(w => w.FormID == epi.FormID && w.ProductID != 122).AsEnumerable()
                           from i in db.mTProductIndicatorOutput.Where(w => w.IDIndicator == DataType.Indicator.Effluent && w.ProductID == d.ProductID && (w.sIsSpecific + "") == "")
                           orderby i.nOrder
                           select new TDataEffluent_Product
                           {
                               nProductID = i.ProductID,
                               sProductName = i.ProductName,
                               sUnit = i.sUnit,
                               nOrder = i.nOrder,

                               sM1 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M1 : "",
                               sM2 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M2 : "",
                               sM3 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M3 : "",

                               sM4 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M4 : "",
                               sM5 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M5 : "",
                               sM6 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M6 : "",

                               sM7 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M7 : "",
                               sM8 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M8 : "",
                               sM9 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M9 : "",

                               sM10 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M10 : "",
                               sM11 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M11 : "",
                               sM12 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M12 : "",
                           }).ToList();

        lstDataOutput = queryOutput.GroupBy(g => new { g.nProductID, g.sProductName, g.nOrder, g.sUnit }).Select(s => new TDataEffluent_Product
        {
            nProductID = s.Key.nProductID,
            sProductName = s.Key.sProductName,
            nOrder = s.Key.nOrder,
            sUnit = s.Key.sUnit,
            sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
            sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
            sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
            sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
            sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
            sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
            sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
            sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
            sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
            sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
            sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
            sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
            sTotal = s.Sum(x => SystemFunction.GetDecimalNull(x.sTotal)) + "",
        }).OrderBy(o => o.nOrder).ToList();

        foreach (var item in lstDataOutput)
        {
            item.sTotal = SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";
            item.sShowM1 = SystemFunction.ConvertFormatDecimal4(item.sM1);
            item.sShowM2 = SystemFunction.ConvertFormatDecimal4(item.sM2);
            item.sShowM3 = SystemFunction.ConvertFormatDecimal4(item.sM3);
            item.sShowM4 = SystemFunction.ConvertFormatDecimal4(item.sM4);
            item.sShowM5 = SystemFunction.ConvertFormatDecimal4(item.sM5);
            item.sShowM6 = SystemFunction.ConvertFormatDecimal4(item.sM6);
            item.sShowM7 = SystemFunction.ConvertFormatDecimal4(item.sM7);
            item.sShowM8 = SystemFunction.ConvertFormatDecimal4(item.sM8);
            item.sShowM9 = SystemFunction.ConvertFormatDecimal4(item.sM9);
            item.sShowM10 = SystemFunction.ConvertFormatDecimal4(item.sM10);
            item.sShowM11 = SystemFunction.ConvertFormatDecimal4(item.sM11);
            item.sShowM12 = SystemFunction.ConvertFormatDecimal4(item.sM12);
            item.sShowTotal = SystemFunction.ConvertFormatDecimal4(item.sTotal);
        }
        #endregion

        #region Query Point
        var dataPoint = (from epi in dataEPIFrom
                         from f in db.mTFacility.Where(w => w.ID == epi.FacilityID)
                         from p in db.TEffluent_Point.Where(w => w.FormID == epi.FormID)
                         select new
                         {
                             epi.FormID,
                             nFacID = f.ID,
                             sFacName = f.Name,
                             p.nPointID,
                             p.sPointName,
                             p.sOption1,
                             p.sOption2,
                             p.sPercent,
                             p.nDischargeTo,
                             p.nTreamentMethod,
                             p.sOtherTreamentMethod,
                             p.nArea,
                             p.sPointType,
                         }).ToList();

        var dataProductPoint = (from epi in dataEPIFrom
                                from d in db.TEffluent_Product_Point.Where(w => w.FormID == epi.FormID)
                                from i in db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.Effluent && w.ProductID == d.ProductID)
                                select new
                                {
                                    epi.FormID,
                                    epi.FacilityID,
                                    i.ProductID,
                                    i.ProductName,
                                    d.nPointID,
                                    d.M1,
                                    d.M2,
                                    d.M3,
                                    d.M4,
                                    d.M5,
                                    d.M6,
                                    d.M7,
                                    d.M8,
                                    d.M9,
                                    d.M10,
                                    d.M11,
                                    d.M12,
                                }).ToList();

        foreach (var itemP in dataPoint)
        {
            lstDataPointProduct = new List<TDataEffluent_Product>();
            lstDataPointProduct = dataProductPoint.Where(w => w.FormID == itemP.FormID && w.nPointID == itemP.nPointID).Select(s => new TDataEffluent_Product
                {
                    nProductID = s.ProductID,
                    sM1 = lstDataSubFac.Any(x => x.FacilityID == itemP.nFacID && x.nQuarter == 1) ? s.M1 : "",
                    sM2 = lstDataSubFac.Any(x => x.FacilityID == itemP.nFacID && x.nQuarter == 1) ? s.M2 : "",
                    sM3 = lstDataSubFac.Any(x => x.FacilityID == itemP.nFacID && x.nQuarter == 1) ? s.M3 : "",

                    sM4 = lstDataSubFac.Any(x => x.FacilityID == itemP.nFacID && x.nQuarter == 2) ? s.M4 : "",
                    sM5 = lstDataSubFac.Any(x => x.FacilityID == itemP.nFacID && x.nQuarter == 2) ? s.M5 : "",
                    sM6 = lstDataSubFac.Any(x => x.FacilityID == itemP.nFacID && x.nQuarter == 2) ? s.M6 : "",

                    sM7 = lstDataSubFac.Any(x => x.FacilityID == itemP.nFacID && x.nQuarter == 3) ? s.M7 : "",
                    sM8 = lstDataSubFac.Any(x => x.FacilityID == itemP.nFacID && x.nQuarter == 3) ? s.M8 : "",
                    sM9 = lstDataSubFac.Any(x => x.FacilityID == itemP.nFacID && x.nQuarter == 3) ? s.M9 : "",

                    sM10 = lstDataSubFac.Any(x => x.FacilityID == itemP.nFacID && x.nQuarter == 4) ? s.M10 : "",
                    sM11 = lstDataSubFac.Any(x => x.FacilityID == itemP.nFacID && x.nQuarter == 4) ? s.M11 : "",
                    sM12 = lstDataSubFac.Any(x => x.FacilityID == itemP.nFacID && x.nQuarter == 4) ? s.M12 : "",
                }).ToList();
            lstDataPoint.Add(new TDataEffluent_Point
                {
                    sPointName = itemP.sPointName + "(" + itemP.sFacName + ")",
                    sOption1 = itemP.sOption1,
                    sOption2 = itemP.sOption2,
                    sPercent = itemP.sPercent,
                    nDischargeTo = itemP.nDischargeTo ?? 0,
                    nTreamentMethod = itemP.nTreamentMethod ?? 0,
                    sOtherTreamentMethod = itemP.sOtherTreamentMethod,
                    nArea = itemP.nArea ?? 0,
                    sPointType = itemP.sPointType,
                    lstDataProduct = lstDataPointProduct
                });
        }
        #endregion

        result.lstDataEffluentSumamry = lstDataSummary;
        result.lstDataEffluentOutput = lstDataOutput;
        result.lstDataEffluentPoint = lstDataPoint;
        return result;
    }

    private CResultGetDataPageLoad GetDataSpill(int nYear, int nMainFacID, List<TDataSubFac> lstDataSubFac)
    {
        CResultGetDataPageLoad result = new CResultGetDataPageLoad();
        string sYear = nYear + "";
        var lstSubFacID = lstDataSubFac.GroupBy(g => g.FacilityID).Select(s => s.Key).ToList();
        var dataEPIFrom = db.TEPI_Forms.Where(w => w.sYear == sYear && w.IDIndicator == DataType.Indicator.Spill && lstSubFacID.Contains(w.FacilityID ?? 0)).ToList();
        List<TDataSpill> lstDataSpill = new List<TDataSpill>();
        List<TDataSpill_Product> lstDataSpillProduct = new List<TDataSpill_Product>();

        #region Spill Product >> Table Summary
        var queryMain = (from epi in dataEPIFrom
                         from d in db.TSpill_Product.Where(w => w.FormID == epi.FormID).AsEnumerable()
                         from i in db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.Spill && w.ProductID == d.ProductID)
                         orderby i.nOrder
                         select new TDataSpill_Product
                         {
                             nProductID = i.ProductID,
                             sProductName = i.ProductName,
                             cTotal = i.cTotal,
                             cTotalAll = i.cTotalAll,
                             sUnit = i.sUnit,
                             sType = i.sType,
                             nOrder = i.nOrder,

                             sM1 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M1 : "",
                             sM2 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M2 : "",
                             sM3 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M3 : "",

                             sM4 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M4 : "",
                             sM5 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M5 : "",
                             sM6 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M6 : "",

                             sM7 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M7 : "",
                             sM8 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M8 : "",
                             sM9 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M9 : "",

                             sM10 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M10 : "",
                             sM11 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M11 : "",
                             sM12 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M12 : "",
                         }).ToList();

        lstDataSpillProduct = queryMain.GroupBy(g => new { g.nProductID, g.sProductName, g.cTotalAll, g.cTotal, g.sType, g.nOrder, g.sUnit }).Select(s => new TDataSpill_Product
        {
            nProductID = s.Key.nProductID,
            sProductName = s.Key.sProductName,
            cTotalAll = s.Key.cTotalAll,
            cTotal = s.Key.cTotal,
            sType = s.Key.sType,
            nOrder = s.Key.nOrder,
            sUnit = s.Key.sUnit,
            sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
            sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
            sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
            sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
            sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
            sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
            sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
            sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
            sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
            sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
            sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
            sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
            sTotal = s.Sum(x => SystemFunction.GetDecimalNull(x.sTotal)) + "",
        }).OrderBy(o => o.nOrder).ToList();

        foreach (var item in lstDataSpillProduct)
        {
            item.sTotal = SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";

            item.sShowM1 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM1) + "" : SystemFunction.ConvertFormatDecimal4(item.sM1);
            item.sShowM2 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM2) + "" : SystemFunction.ConvertFormatDecimal4(item.sM2);
            item.sShowM3 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM3) + "" : SystemFunction.ConvertFormatDecimal4(item.sM3);
            item.sShowM4 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM4) + "" : SystemFunction.ConvertFormatDecimal4(item.sM4);
            item.sShowM5 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM5) + "" : SystemFunction.ConvertFormatDecimal4(item.sM5);
            item.sShowM6 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM6) + "" : SystemFunction.ConvertFormatDecimal4(item.sM6);
            item.sShowM7 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM7) + "" : SystemFunction.ConvertFormatDecimal4(item.sM7);
            item.sShowM8 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM8) + "" : SystemFunction.ConvertFormatDecimal4(item.sM8);
            item.sShowM9 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM9) + "" : SystemFunction.ConvertFormatDecimal4(item.sM9);
            item.sShowM10 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM10) + "" : SystemFunction.ConvertFormatDecimal4(item.sM10);
            item.sShowM11 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM11) + "" : SystemFunction.ConvertFormatDecimal4(item.sM11);
            item.sShowM12 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM12) + "" : SystemFunction.ConvertFormatDecimal4(item.sM12);
            item.sShowTotal = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sTotal) + "" : SystemFunction.ConvertFormatDecimal4(item.sTotal);
        }
        #endregion

        #region Spill Case
        var dataSpillDataType = db.TData_Type.Where(w => w.IndicatorID == 9).ToList();
        Func<int, string> GetSpillDataType = (id) =>
        {
            var q = dataSpillDataType.FirstOrDefault(w => w.nID == id);
            return q != null ? q.sName : "";
        };
        var QuerySpill = (from epi in dataEPIFrom
                          from d in db.TSpill.Where(w => w.FormID == epi.FormID).AsEnumerable()
                          select new TDataSpill
                          {
                              nFormID = epi.FormID,
                              nFacilityID = epi.FacilityID ?? 0,
                              nSpillID = d.nSpillID,
                              nPrimaryReasonID = d.PrimaryReasonID ?? 0,
                              sOtherPrimary = d.sOtherPrimary,
                              sSpillType = d.SpillType,
                              nSpillOfID = d.SpillOfID,
                              sOtherSpillOf = d.sOtherSpillOf,
                              sVolume = d.Volume,
                              nUnitVolumeID = d.UnitVolumeID,
                              sDensity = d.Density,
                              nSpillToID = d.SpillToID,
                              sOtherSpillTo = d.sOtherSpillTo,
                              nSpillByID = d.SpillByID,
                              sOtherSpillBy = d.sOtherSpillBy,
                              sIsSensitiveArea = d.sIsSensitiveArea,
                              sSpillDate = d.SpillDate.DateString()
                          }).OrderBy(o => o.nFormID).ThenBy(o => o.nSpillID).ToList();

        int nSpillIDNew = 1;
        foreach (var item in QuerySpill)
        {
            item.nSpillID = nSpillIDNew;//เลขต้องเป็นตัวเดิมเนื่องจากถ้าไม่ตรงกับตัวเดิมระบบที่ PTT ยังไม่สามารถอัพเดทตัวเดิมได้
            DateTime dSpillDate = SystemFunction.ConvertStringToDateTime(item.sSpillDate, "");
            switch (dSpillDate.Month)
            {
                case 1:
                case 2:
                case 3:
                    if (lstDataSubFac.Any(x => x.FacilityID == item.nFacilityID && x.nQuarter == 1))
                    {
                        lstDataSpill.Add(item);
                    }
                    break;
                case 4:
                case 5:
                case 6:
                    if (lstDataSubFac.Any(x => x.FacilityID == item.nFacilityID && x.nQuarter == 2))
                    {
                        lstDataSpill.Add(item);
                    }
                    break;
                case 7:
                case 8:
                case 9:
                    if (lstDataSubFac.Any(x => x.FacilityID == item.nFacilityID && x.nQuarter == 3))
                    {
                        lstDataSpill.Add(item);
                    }
                    break;
                case 10:
                case 11:
                case 12:
                    if (lstDataSubFac.Any(x => x.FacilityID == item.nFacilityID && x.nQuarter == 4))
                    {
                        lstDataSpill.Add(item);
                    }
                    break;
            }
            nSpillIDNew++;
        }

        foreach (var item in lstDataSpill)//Req. 09.04.2019 transfer to ptt case sign. spill only
        {
            item.sPrimaryReasonName = item.nPrimaryReasonID == 20 ? item.sOtherPrimary : GetSpillDataType(item.nPrimaryReasonID);
            item.sSpillTypeName = item.sSpillType == "HC" ? "Hydrocarbon" : item.sSpillType == "NHC" ? "Non-Hydrocarbon" : "";
            item.sSpillOfName = item.nSpillOfID == 30 ? item.sOtherSpillOf : GetSpillDataType(item.nSpillOfID ?? 0);
            item.sSpillToName = item.nSpillToID == 75 ? item.sOtherSpillTo : GetSpillDataType(item.nSpillToID ?? 0);
            item.sSpillByName = item.nSpillByID == 93 ? item.sOtherSpillBy : GetSpillDataType(item.nSpillByID ?? 0);

            var qUnitName = db.mTUnit.FirstOrDefault(w => w.UnitID == item.nUnitVolumeID);
            item.sUnitName = qUnitName != null ? qUnitName.UnitName : "";

            #region Check Spill Case
            decimal? nBarrel = null;
            switch (item.nUnitVolumeID)
            {
                case 63: //L(Liter)
                    nBarrel = EPIFunc.ConvertLiterToBarrel(item.sVolume);
                    break;
                case 64: //bbl(Barrel)
                    nBarrel = EPIFunc.GetDecimalNull(item.sVolume);
                    break;
                case 65: //cu.m.(m3)
                    nBarrel = EPIFunc.ConvertM3ToBarrel(item.sVolume);
                    break;
                case 2: //Tonnes
                    nBarrel = EPIFunc.ConvertLiterToBarrel((EPIFunc.GetDecimalNull(item.sVolume) * EPIFunc.GetDecimalNull(item.sDensity)) + "");
                    break;
            }

            if (item.nPrimaryReasonID == 18)//Third Party Damage
            {
                if (item.nSpillByID == 32)//Transportation
                {
                    if (item.sIsSensitiveArea == "Y")//Sign. Spill
                        item.sSpillIcon = "2";
                    else if (nBarrel >= 1 && nBarrel < 100)//Spill
                        item.sSpillIcon = "1";
                    else if (nBarrel >= 100)
                        item.sSpillIcon = "2";
                }
                else
                {
                    item.sSpillIcon = "3";
                }
            }
            else
            {
                if (item.nSpillByID.HasValue && item.nSpillByID.Value != 32)//Not Transportation (Production/Other)
                {
                    if (item.sIsSensitiveArea == "Y")
                        item.sSpillIcon = "2";
                    else if (nBarrel >= 1 && nBarrel < 100)
                        item.sSpillIcon = "1";
                    else if (nBarrel >= 100)
                        item.sSpillIcon = "2";
                }
                else
                {
                    item.sSpillIcon = "3";
                }
            }
            #endregion
        }
        #endregion

        #region//********* Req. 09.04.2019 transfer to ptt case sign. spill only *******//
        lstDataSpill = lstDataSpill.Where(w => w.sSpillIcon == "2").ToList();
        Func<string, int, bool> CheckSpillInMonth = (sdate, m) =>
        {
            DateTime? d = SystemFunction.ConvertStringToDateTime(sdate, "");
            if (d.HasValue)
            {
                return (d.Value.Month == m);
            }
            else
                return false;
        };

        Func<string, int, string, decimal?> ConvertVolumeToBBL = (sVolume, nUnitVolumeID, sDensity) =>
            {
                decimal? nBarrel = null;
                switch (nUnitVolumeID)
                {
                    case 63: //L(Liter)
                        nBarrel = EPIFunc.ConvertLiterToBarrel(sVolume);
                        break;
                    case 64: //bbl(Barrel)
                        nBarrel = EPIFunc.GetDecimalNull(sVolume);
                        break;
                    case 65: //cu.m.(m3)
                        nBarrel = EPIFunc.ConvertM3ToBarrel(sVolume);
                        break;
                    case 2: //Tonnes
                        nBarrel = EPIFunc.ConvertLiterToBarrel((EPIFunc.GetDecimalNull(sVolume) * EPIFunc.GetDecimalNull(sDensity)) + "");
                        break;
                }
                return nBarrel;
            };
        Func<int, int, string, decimal?> SumDataSpill = (nMonth, toUnitID, sItemValue) =>
        {
            decimal? nSum = null;
            var qSpill = lstDataSpill.Where(w => CheckSpillInMonth(w.sSpillDate, nMonth)).ToList();
            List<decimal?> lstVolumn = new List<decimal?>();
            foreach (var itemS in qSpill)
            {
                lstVolumn.Add(ConvertVolumeToBBL(itemS.sVolume, itemS.nUnitVolumeID ?? 0, itemS.sDensity));//Convert to BBL
            }

            nSum = lstVolumn.Any(x => x.HasValue) ? lstVolumn.Sum() : null;
            if (toUnitID == 63)//L(Liter)
            {
                nSum = EPIFunc.ConvertBarrelToLiter(nSum + "");
            }
            else if (toUnitID == 65)//cu.m.(m3)
            {
                nSum = EPIFunc.ConvertBarrelToM3(nSum + "");
            }

            if (!string.IsNullOrEmpty(sItemValue))//case check none spill
                nSum = 0;
            return nSum;
        };

        foreach (var item in lstDataSpillProduct)
        {
            switch (item.nProductID)
            {
                case 209: //Cases
                    {
                        item.sM1 = item.sM1 == "" ? "" : lstDataSpill.Where(w => CheckSpillInMonth(w.sSpillDate, 1)).Count() + "";
                        item.sM2 = item.sM2 == "" ? "" : lstDataSpill.Where(w => CheckSpillInMonth(w.sSpillDate, 2)).Count() + "";
                        item.sM3 = item.sM3 == "" ? "" : lstDataSpill.Where(w => CheckSpillInMonth(w.sSpillDate, 3)).Count() + "";
                        item.sM4 = item.sM4 == "" ? "" : lstDataSpill.Where(w => CheckSpillInMonth(w.sSpillDate, 4)).Count() + "";
                        item.sM5 = item.sM5 == "" ? "" : lstDataSpill.Where(w => CheckSpillInMonth(w.sSpillDate, 5)).Count() + "";
                        item.sM6 = item.sM6 == "" ? "" : lstDataSpill.Where(w => CheckSpillInMonth(w.sSpillDate, 6)).Count() + "";
                        item.sM7 = item.sM7 == "" ? "" : lstDataSpill.Where(w => CheckSpillInMonth(w.sSpillDate, 7)).Count() + "";
                        item.sM8 = item.sM8 == "" ? "" : lstDataSpill.Where(w => CheckSpillInMonth(w.sSpillDate, 8)).Count() + "";
                        item.sM9 = item.sM9 == "" ? "" : lstDataSpill.Where(w => CheckSpillInMonth(w.sSpillDate, 9)).Count() + "";
                        item.sM10 = item.sM10 == "" ? "" : lstDataSpill.Where(w => CheckSpillInMonth(w.sSpillDate, 10)).Count() + "";
                        item.sM11 = item.sM11 == "" ? "" : lstDataSpill.Where(w => CheckSpillInMonth(w.sSpillDate, 11)).Count() + "";
                        item.sM12 = item.sM12 == "" ? "" : lstDataSpill.Where(w => CheckSpillInMonth(w.sSpillDate, 12)).Count() + "";
                    }
                    break;
                case 210: //Liter
                    item.sM1 = item.sM1 == "" ? "" : SumDataSpill(1, 63, item.sM1) + "";
                    item.sM2 = item.sM2 == "" ? "" : SumDataSpill(2, 63, item.sM2) + "";
                    item.sM3 = item.sM3 == "" ? "" : SumDataSpill(3, 63, item.sM3) + "";
                    item.sM4 = item.sM4 == "" ? "" : SumDataSpill(4, 63, item.sM4) + "";
                    item.sM5 = item.sM5 == "" ? "" : SumDataSpill(5, 63, item.sM5) + "";
                    item.sM6 = item.sM6 == "" ? "" : SumDataSpill(6, 63, item.sM6) + "";
                    item.sM7 = item.sM7 == "" ? "" : SumDataSpill(7, 63, item.sM7) + "";
                    item.sM8 = item.sM8 == "" ? "" : SumDataSpill(8, 63, item.sM8) + "";
                    item.sM9 = item.sM9 == "" ? "" : SumDataSpill(9, 63, item.sM9) + "";
                    item.sM10 = item.sM10 == "" ? "" : SumDataSpill(10, 63, item.sM10) + "";
                    item.sM11 = item.sM11 == "" ? "" : SumDataSpill(11, 63, item.sM11) + "";
                    item.sM12 = item.sM12 == "" ? "" : SumDataSpill(12, 63, item.sM12) + "";
                    break;
                case 256: //m3
                    item.sM1 = item.sM1 == "" ? "" : SumDataSpill(1, 65, item.sM1) + "";
                    item.sM2 = item.sM2 == "" ? "" : SumDataSpill(2, 65, item.sM2) + "";
                    item.sM3 = item.sM3 == "" ? "" : SumDataSpill(3, 65, item.sM3) + "";
                    item.sM4 = item.sM4 == "" ? "" : SumDataSpill(4, 65, item.sM4) + "";
                    item.sM5 = item.sM5 == "" ? "" : SumDataSpill(5, 65, item.sM5) + "";
                    item.sM6 = item.sM6 == "" ? "" : SumDataSpill(6, 65, item.sM6) + "";
                    item.sM7 = item.sM7 == "" ? "" : SumDataSpill(7, 65, item.sM7) + "";
                    item.sM8 = item.sM8 == "" ? "" : SumDataSpill(8, 65, item.sM8) + "";
                    item.sM9 = item.sM9 == "" ? "" : SumDataSpill(9, 65, item.sM9) + "";
                    item.sM10 = item.sM10 == "" ? "" : SumDataSpill(10, 65, item.sM10) + "";
                    item.sM11 = item.sM11 == "" ? "" : SumDataSpill(11, 65, item.sM11) + "";
                    item.sM12 = item.sM12 == "" ? "" : SumDataSpill(12, 65, item.sM12) + "";
                    break;
            }

            item.sTotal = SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";
            item.sShowM1 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM1) + "" : SystemFunction.ConvertFormatDecimal4(item.sM1);
            item.sShowM2 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM2) + "" : SystemFunction.ConvertFormatDecimal4(item.sM2);
            item.sShowM3 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM3) + "" : SystemFunction.ConvertFormatDecimal4(item.sM3);
            item.sShowM4 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM4) + "" : SystemFunction.ConvertFormatDecimal4(item.sM4);
            item.sShowM5 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM5) + "" : SystemFunction.ConvertFormatDecimal4(item.sM5);
            item.sShowM6 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM6) + "" : SystemFunction.ConvertFormatDecimal4(item.sM6);
            item.sShowM7 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM7) + "" : SystemFunction.ConvertFormatDecimal4(item.sM7);
            item.sShowM8 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM8) + "" : SystemFunction.ConvertFormatDecimal4(item.sM8);
            item.sShowM9 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM9) + "" : SystemFunction.ConvertFormatDecimal4(item.sM9);
            item.sShowM10 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM10) + "" : SystemFunction.ConvertFormatDecimal4(item.sM10);
            item.sShowM11 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM11) + "" : SystemFunction.ConvertFormatDecimal4(item.sM11);
            item.sShowM12 = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sM12) + "" : SystemFunction.ConvertFormatDecimal4(item.sM12);
            item.sShowTotal = item.nProductID == 209 ? SystemFunction.GetIntNull(item.sTotal) + "" : SystemFunction.ConvertFormatDecimal4(item.sTotal);
        }
        #endregion

        result.lstDataSpillProduct = lstDataSpillProduct;
        result.lstDataSpill = lstDataSpill.OrderBy(o => SystemFunction.ConvertStringToDateTime(o.sSpillDate, "")).ToList(); ;
        return result;
    }

    private CResultGetDataPageLoad GetDataCompliance(int nYear, int nMainFacID, List<TDataSubFac> lstDataSubFac)
    {
        CResultGetDataPageLoad result = new CResultGetDataPageLoad();
        string sYear = nYear + "";
        var lstSubFacID = lstDataSubFac.GroupBy(g => g.FacilityID).Select(s => s.Key).ToList();
        var dataEPIFrom = db.TEPI_Forms.Where(w => w.sYear == sYear && w.IDIndicator == DataType.Indicator.Compliance && lstSubFacID.Contains(w.FacilityID ?? 0)).ToList();
        List<TDataCompliance_Product> lstDataComplianceProduct = new List<TDataCompliance_Product>();
        List<TDataCompliance> lstDataCompliance = new List<TDataCompliance>();

        #region Compliance Product >> Table Summary
        var queryMain = (from epi in dataEPIFrom
                         from d in db.TCompliance_Product.Where(w => w.FormID == epi.FormID).AsEnumerable()
                         from i in db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.Compliance && w.ProductID == d.ProductID)
                         orderby i.nOrder
                         select new TDataCompliance_Product
                         {
                             nProductID = i.ProductID,
                             sProductName = i.ProductName,
                             cTotal = i.cTotal,
                             cTotalAll = i.cTotalAll,
                             sUnit = i.sUnit,
                             sType = i.sType,
                             nOrder = i.nOrder,

                             sM1 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M1 : "",
                             sM2 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M2 : "",
                             sM3 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 1) ? d.M3 : "",

                             sM4 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M4 : "",
                             sM5 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M5 : "",
                             sM6 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 2) ? d.M6 : "",

                             sM7 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M7 : "",
                             sM8 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M8 : "",
                             sM9 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 3) ? d.M9 : "",

                             sM10 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M10 : "",
                             sM11 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M11 : "",
                             sM12 = lstDataSubFac.Any(x => x.FacilityID == epi.FacilityID && x.nQuarter == 4) ? d.M12 : "",
                         }).ToList();

        var queryComplianceProduct = (from epi in dataEPIFrom
                                      from d in db.TCompliance_Product.Where(w => w.FormID == epi.FormID)
                                      select new
                                      {
                                          d.FormID,
                                          d.IsCheckM1,
                                          d.IsCheckM2,
                                          d.IsCheckM3,
                                          d.IsCheckM4,
                                          d.IsCheckM5,
                                          d.IsCheckM6,
                                          d.IsCheckM7,
                                          d.IsCheckM8,
                                          d.IsCheckM9,
                                          d.IsCheckM10,
                                          d.IsCheckM11,
                                          d.IsCheckM12,
                                      }).ToList();

        lstDataComplianceProduct = queryMain.GroupBy(g => new { g.nProductID, g.sProductName, g.cTotalAll, g.cTotal, g.sType, g.nOrder, g.sUnit }).Select(s => new TDataCompliance_Product
        {
            nProductID = s.Key.nProductID,
            sProductName = s.Key.sProductName,
            cTotalAll = s.Key.cTotalAll,
            cTotal = s.Key.cTotal,
            sType = s.Key.sType,
            nOrder = s.Key.nOrder,
            sUnit = s.Key.sUnit,
            sM1 = s.Any(x => SystemFunction.GetDecimalNull(x.sM1).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM1)) + "" : s.Any(x => x.sM1 == sNAFormat) ? "N/A" : "",
            sM2 = s.Any(x => SystemFunction.GetDecimalNull(x.sM2).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM2)) + "" : s.Any(x => x.sM2 == sNAFormat) ? "N/A" : "",
            sM3 = s.Any(x => SystemFunction.GetDecimalNull(x.sM3).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM3)) + "" : s.Any(x => x.sM3 == sNAFormat) ? "N/A" : "",
            sM4 = s.Any(x => SystemFunction.GetDecimalNull(x.sM4).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM4)) + "" : s.Any(x => x.sM4 == sNAFormat) ? "N/A" : "",
            sM5 = s.Any(x => SystemFunction.GetDecimalNull(x.sM5).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM5)) + "" : s.Any(x => x.sM5 == sNAFormat) ? "N/A" : "",
            sM6 = s.Any(x => SystemFunction.GetDecimalNull(x.sM6).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM6)) + "" : s.Any(x => x.sM6 == sNAFormat) ? "N/A" : "",
            sM7 = s.Any(x => SystemFunction.GetDecimalNull(x.sM7).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM7)) + "" : s.Any(x => x.sM7 == sNAFormat) ? "N/A" : "",
            sM8 = s.Any(x => SystemFunction.GetDecimalNull(x.sM8).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM8)) + "" : s.Any(x => x.sM8 == sNAFormat) ? "N/A" : "",
            sM9 = s.Any(x => SystemFunction.GetDecimalNull(x.sM9).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM9)) + "" : s.Any(x => x.sM9 == sNAFormat) ? "N/A" : "",
            sM10 = s.Any(x => SystemFunction.GetDecimalNull(x.sM10).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM10)) + "" : s.Any(x => x.sM10 == sNAFormat) ? "N/A" : "",
            sM11 = s.Any(x => SystemFunction.GetDecimalNull(x.sM11).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM11)) + "" : s.Any(x => x.sM11 == sNAFormat) ? "N/A" : "",
            sM12 = s.Any(x => SystemFunction.GetDecimalNull(x.sM12).HasValue) ? s.Sum(x => SystemFunction.GetDecimalNull(x.sM12)) + "" : s.Any(x => x.sM12 == sNAFormat) ? "N/A" : "",
            sTotal = s.Sum(x => SystemFunction.GetDecimalNull(x.sTotal)) + "",
        }).OrderBy(o => o.nOrder).ToList();

        foreach (var item in lstDataComplianceProduct)
        {
            if (item.sM1 == "0")
                item.sM1 = queryComplianceProduct.Any(w => w.IsCheckM1 == "Y") ? "0" : "";
            if (item.sM2 == "0")
                item.sM2 = queryComplianceProduct.Any(w => w.IsCheckM2 == "Y") ? "0" : "";
            if (item.sM3 == "0")
                item.sM3 = queryComplianceProduct.Any(w => w.IsCheckM3 == "Y") ? "0" : "";
            if (item.sM4 == "0")
                item.sM4 = queryComplianceProduct.Any(w => w.IsCheckM4 == "Y") ? "0" : "";
            if (item.sM5 == "0")
                item.sM5 = queryComplianceProduct.Any(w => w.IsCheckM5 == "Y") ? "0" : "";
            if (item.sM6 == "0")
                item.sM6 = queryComplianceProduct.Any(w => w.IsCheckM6 == "Y") ? "0" : "";
            if (item.sM7 == "0")
                item.sM7 = queryComplianceProduct.Any(w => w.IsCheckM7 == "Y") ? "0" : "";
            if (item.sM8 == "0")
                item.sM8 = queryComplianceProduct.Any(w => w.IsCheckM8 == "Y") ? "0" : "";
            if (item.sM9 == "0")
                item.sM9 = queryComplianceProduct.Any(w => w.IsCheckM9 == "Y") ? "0" : "";
            if (item.sM10 == "0")
                item.sM10 = queryComplianceProduct.Any(w => w.IsCheckM10 == "Y") ? "0" : "";
            if (item.sM11 == "0")
                item.sM11 = queryComplianceProduct.Any(w => w.IsCheckM11 == "Y") ? "0" : "";
            if (item.sM12 == "0")
                item.sM12 = queryComplianceProduct.Any(w => w.IsCheckM12 == "Y") ? "0" : "";

            item.sTotal = SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";
            item.sShowM1 = SystemFunction.GetIntNull(item.sM1) + "";
            item.sShowM2 = SystemFunction.GetIntNull(item.sM2) + "";
            item.sShowM3 = SystemFunction.GetIntNull(item.sM3) + "";
            item.sShowM4 = SystemFunction.GetIntNull(item.sM4) + "";
            item.sShowM5 = SystemFunction.GetIntNull(item.sM5) + "";
            item.sShowM6 = SystemFunction.GetIntNull(item.sM6) + "";
            item.sShowM7 = SystemFunction.GetIntNull(item.sM7) + "";
            item.sShowM8 = SystemFunction.GetIntNull(item.sM8) + "";
            item.sShowM9 = SystemFunction.GetIntNull(item.sM9) + "";
            item.sShowM10 = SystemFunction.GetIntNull(item.sM10) + "";
            item.sShowM11 = SystemFunction.GetIntNull(item.sM11) + "";
            item.sShowM12 = SystemFunction.GetIntNull(item.sM12) + "";
            item.sShowTotal = SystemFunction.GetIntNull(item.sTotal) + "";
        }
        #endregion

        #region Compliance Case
        var queryCompliance = (from epi in dataEPIFrom
                               from d in db.TCompliance.Where(w => w.FormID == epi.FormID).AsEnumerable()
                               select new TDataCompliance
                               {
                                   nFormID = epi.FormID,
                                   nComplianceID = d.nComplianceID,
                                   nFacilityID = epi.FacilityID ?? 0,
                                   sComplianceDate = d.ComplianceDate.DateString(),
                                   sDocNumber = d.sDocNumber,
                                   sIssueBy = d.sIssueBy,
                                   sSubject = d.sSubject,
                                   sDetail = d.sDetail
                               }).OrderBy(o => o.nFormID).ThenBy(o => o.nComplianceID).ToList();

        int nComplianceIDNew = 1;
        foreach (var item in queryCompliance)
        {
            item.nComplianceID = nComplianceIDNew;//เลขต้องเป็นตัวเดิมเนื่องจากถ้าไม่ตรงกับตัวเดิมระบบที่ PTT ยังไม่สามารถอัพเดทตัวเดิมได้
            DateTime dSpillDate = SystemFunction.ConvertStringToDateTime(item.sComplianceDate, "");
            switch (dSpillDate.Month)
            {
                case 1:
                case 2:
                case 3:
                    if (lstDataSubFac.Any(x => x.FacilityID == item.nFacilityID && x.nQuarter == 1))
                    {
                        lstDataCompliance.Add(item);
                    }
                    break;
                case 4:
                case 5:
                case 6:
                    if (lstDataSubFac.Any(x => x.FacilityID == item.nFacilityID && x.nQuarter == 2))
                    {
                        lstDataCompliance.Add(item);
                    }
                    break;
                case 7:
                case 8:
                case 9:
                    if (lstDataSubFac.Any(x => x.FacilityID == item.nFacilityID && x.nQuarter == 3))
                    {
                        lstDataCompliance.Add(item);
                    }
                    break;
                case 10:
                case 11:
                case 12:
                    if (lstDataSubFac.Any(x => x.FacilityID == item.nFacilityID && x.nQuarter == 4))
                    {
                        lstDataCompliance.Add(item);
                    }
                    break;
            }
            nComplianceIDNew++;
        }
        #endregion

        result.lstDataComplianceProduct = lstDataComplianceProduct;
        result.lstDataCompliance = lstDataCompliance.OrderBy(o => SystemFunction.ConvertStringToDateTime(o.sComplianceDate, "")).ToList();
        return result;
    }

    private CResultGetDataPageLoad GetDataComplaint(int nYear, int nMainFacID, List<TDataSubFac> lstDataSubFac)
    {
        CResultGetDataPageLoad result = new CResultGetDataPageLoad();
        string sYear = nYear + "";
        var lstSubFacID = lstDataSubFac.GroupBy(g => g.FacilityID).Select(s => s.Key).ToList();
        var dataEPIFrom = db.TEPI_Forms.Where(w => w.sYear == sYear && w.IDIndicator == DataType.Indicator.Complaint && lstSubFacID.Contains(w.FacilityID ?? 0)).ToList();
        List<TDataComplaint_Product> lstDataComplaintProduct = new List<TDataComplaint_Product>();
        List<TDataComplaint> lstDataComplaint = new List<TDataComplaint>();

        #region Complaint Case >> ส่งไปที่ PTT > Validated environmental complaint เฉพาะรายการที่เป็น Official valid complaint
        var queryComplaint = (from epi in dataEPIFrom
                              from d in db.TComplaint.Where(w => w.FormID == epi.FormID && w.nComplaintTypeID == 1).AsEnumerable()
                              select new TDataComplaint
                              {
                                  nFormID = epi.FormID,
                                  nComplaintID = d.nComplaintID,
                                  dComplaintDate = d.ComplaintDate,
                                  nFacilityID = epi.FacilityID ?? 0,
                                  sComplaintDate = d.ComplaintDate.DateString(),
                                  sIssueBy = d.sIssueBy,
                                  sSubject = d.sSubject,
                                  sDetail = d.sDetail,
                                  sCorrectiveAction = d.sCorrectiveAction
                              }).OrderBy(o => o.nFormID).ThenBy(o => o.nComplaintID).ToList();

        var queryComplaintProduct = (from epi in dataEPIFrom
                                     from d in db.TComplaint_Product.Where(w => w.FormID == epi.FormID)
                                     select new
                                     {
                                         d.FormID,
                                         d.IsCheckM1,
                                         d.IsCheckM2,
                                         d.IsCheckM3,
                                         d.IsCheckM4,
                                         d.IsCheckM5,
                                         d.IsCheckM6,
                                         d.IsCheckM7,
                                         d.IsCheckM8,
                                         d.IsCheckM9,
                                         d.IsCheckM10,
                                         d.IsCheckM11,
                                         d.IsCheckM12,
                                     }).ToList();

        int nComplaintIDNew = 1;
        foreach (var item in queryComplaint)
        {
            item.nComplaintID = nComplaintIDNew;//เลขต้องเป็นตัวเดิมเนื่องจากถ้าไม่ตรงกับตัวเดิมระบบที่ PTT ยังไม่สามารถอัพเดทตัวเดิมได้
            DateTime dSpillDate = SystemFunction.ConvertStringToDateTime(item.sComplaintDate, "");
            switch (dSpillDate.Month)
            {
                case 1:
                case 2:
                case 3:
                    if (lstDataSubFac.Any(x => x.FacilityID == item.nFacilityID && x.nQuarter == 1))
                    {
                        lstDataComplaint.Add(item);
                    }
                    break;
                case 4:
                case 5:
                case 6:
                    if (lstDataSubFac.Any(x => x.FacilityID == item.nFacilityID && x.nQuarter == 2))
                    {
                        lstDataComplaint.Add(item);
                    }
                    break;
                case 7:
                case 8:
                case 9:
                    if (lstDataSubFac.Any(x => x.FacilityID == item.nFacilityID && x.nQuarter == 3))
                    {
                        lstDataComplaint.Add(item);
                    }
                    break;
                case 10:
                case 11:
                case 12:
                    if (lstDataSubFac.Any(x => x.FacilityID == item.nFacilityID && x.nQuarter == 4))
                    {
                        lstDataComplaint.Add(item);
                    }
                    break;
            }
            nComplaintIDNew++;
        }
        #endregion

        #region Complaint Product >> Table Summary
        lstDataComplaintProduct = db.mTProductIndicator.Where(w => w.IDIndicator == DataType.Indicator.Complaint).AsEnumerable().Select(s => new TDataComplaint_Product
        {
            nProductID = s.ProductID,
            sProductName = s.ProductName,
            sUnit = s.sUnit,
            sM1 = lstDataComplaint.Where(w => w.dComplaintDate.HasValue && w.dComplaintDate.Value.Month == 1).Count() + "",
            sM2 = lstDataComplaint.Where(w => w.dComplaintDate.HasValue && w.dComplaintDate.Value.Month == 2).Count() + "",
            sM3 = lstDataComplaint.Where(w => w.dComplaintDate.HasValue && w.dComplaintDate.Value.Month == 3).Count() + "",
            sM4 = lstDataComplaint.Where(w => w.dComplaintDate.HasValue && w.dComplaintDate.Value.Month == 4).Count() + "",
            sM5 = lstDataComplaint.Where(w => w.dComplaintDate.HasValue && w.dComplaintDate.Value.Month == 5).Count() + "",
            sM6 = lstDataComplaint.Where(w => w.dComplaintDate.HasValue && w.dComplaintDate.Value.Month == 6).Count() + "",
            sM7 = lstDataComplaint.Where(w => w.dComplaintDate.HasValue && w.dComplaintDate.Value.Month == 7).Count() + "",
            sM8 = lstDataComplaint.Where(w => w.dComplaintDate.HasValue && w.dComplaintDate.Value.Month == 8).Count() + "",
            sM9 = lstDataComplaint.Where(w => w.dComplaintDate.HasValue && w.dComplaintDate.Value.Month == 9).Count() + "",
            sM10 = lstDataComplaint.Where(w => w.dComplaintDate.HasValue && w.dComplaintDate.Value.Month == 10).Count() + "",
            sM11 = lstDataComplaint.Where(w => w.dComplaintDate.HasValue && w.dComplaintDate.Value.Month == 11).Count() + "",
            sM12 = lstDataComplaint.Where(w => w.dComplaintDate.HasValue && w.dComplaintDate.Value.Month == 12).Count() + "",
            sTotal = "",
        }).ToList();

        foreach (var item in lstDataComplaintProduct)
        {
            if (item.sM1 == "0")
                item.sM1 = queryComplaintProduct.Any(w => w.IsCheckM1 == "Y") ? "0" : "";
            if (item.sM2 == "0")
                item.sM2 = queryComplaintProduct.Any(w => w.IsCheckM2 == "Y") ? "0" : "";
            if (item.sM3 == "0")
                item.sM3 = queryComplaintProduct.Any(w => w.IsCheckM3 == "Y") ? "0" : "";
            if (item.sM4 == "0")
                item.sM4 = queryComplaintProduct.Any(w => w.IsCheckM4 == "Y") ? "0" : "";
            if (item.sM5 == "0")
                item.sM5 = queryComplaintProduct.Any(w => w.IsCheckM5 == "Y") ? "0" : "";
            if (item.sM6 == "0")
                item.sM6 = queryComplaintProduct.Any(w => w.IsCheckM6 == "Y") ? "0" : "";
            if (item.sM7 == "0")
                item.sM7 = queryComplaintProduct.Any(w => w.IsCheckM7 == "Y") ? "0" : "";
            if (item.sM8 == "0")
                item.sM8 = queryComplaintProduct.Any(w => w.IsCheckM8 == "Y") ? "0" : "";
            if (item.sM9 == "0")
                item.sM9 = queryComplaintProduct.Any(w => w.IsCheckM9 == "Y") ? "0" : "";
            if (item.sM10 == "0")
                item.sM10 = queryComplaintProduct.Any(w => w.IsCheckM10 == "Y") ? "0" : "";
            if (item.sM11 == "0")
                item.sM11 = queryComplaintProduct.Any(w => w.IsCheckM11 == "Y") ? "0" : "";
            if (item.sM12 == "0")
                item.sM12 = queryComplaintProduct.Any(w => w.IsCheckM12 == "Y") ? "0" : "";

            item.sTotal = SystemFunction.SumDataToDecimal(new List<string>() { item.sM1, item.sM2, item.sM3, item.sM4, item.sM5, item.sM6, item.sM7, item.sM8, item.sM9, item.sM10, item.sM12, item.sM12 }) + "";
            item.sShowM1 = SystemFunction.GetIntNull(item.sM1) + "";
            item.sShowM2 = SystemFunction.GetIntNull(item.sM2) + "";
            item.sShowM3 = SystemFunction.GetIntNull(item.sM3) + "";
            item.sShowM4 = SystemFunction.GetIntNull(item.sM4) + "";
            item.sShowM5 = SystemFunction.GetIntNull(item.sM5) + "";
            item.sShowM6 = SystemFunction.GetIntNull(item.sM6) + "";
            item.sShowM7 = SystemFunction.GetIntNull(item.sM7) + "";
            item.sShowM8 = SystemFunction.GetIntNull(item.sM8) + "";
            item.sShowM9 = SystemFunction.GetIntNull(item.sM9) + "";
            item.sShowM10 = SystemFunction.GetIntNull(item.sM10) + "";
            item.sShowM11 = SystemFunction.GetIntNull(item.sM11) + "";
            item.sShowM12 = SystemFunction.GetIntNull(item.sM12) + "";
            item.sShowTotal = SystemFunction.GetIntNull(item.sTotal) + "";
        }
        #endregion

        //Reset Date for java script
        foreach (var item in lstDataComplaint)
        {
            item.dComplaintDate = null;
        }

        result.lstDataComplaintProduct = lstDataComplaintProduct;
        result.lstDataComplaint = lstDataComplaint.OrderBy(o => SystemFunction.ConvertStringToDateTime(o.sComplaintDate, "")).ToList();
        return result;
    }

    private List<TDataSubFac> GetDataSubFacilityAllowTransfer(int nIndID, int nYear)
    {
        #region SQL Sub-facility
        string _sConditonSubFac = " AND TEPI.IDIndicator='" + nIndID + "'";

        string sqlSubFac = @"SELECT T1.FormID,T1.sYear, ISNULL(T1.IDIndicator,0) 'IDIndicator',ISNULL(T1.OperationtypeID,0) 'OperationtypeID',ISNULL(T1.FacilityID,0) 'FacilityID',T1.sFacName,ISNULL(T1.nHeaderID,0) 'nHeaderID', T1.nQuarter FROM
(
	--Q1
	SELECT TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.Name 'sFacName',TFAC.nHeaderID,1 'nQuarter',CASE WHEN (COUNT(TWF.nMonth)) = 3 THEN 1 ELSE 0 END AS nStatus
	FROM TEPI_Forms TEPI
	INNER JOIN TEPI_Workflow TWF ON TEPI.FormID=TWF.FormID
	INNER JOIN mTFacility TFAC ON TEPI.FacilityID=TFAC.ID AND TEPI.OperationtypeID=TFAC.OperationTypeID AND TFAC.cActive='Y' AND TFAC.cDel='N'
	WHERE TEPI.sYear='{0}' AND TWF.nStatusID=5 AND TWF.nMonth IN (1,2,3) " + _sConditonSubFac + @"
	GROUP BY TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.nHeaderID,TFAC.Name
	UNION
	--Q2
	SELECT TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.Name 'sFacName',TFAC.nHeaderID,2 'nQuarter',CASE WHEN (COUNT(TWF.nMonth)) = 3 THEN 1 ELSE 0 END AS nStatus
	FROM TEPI_Forms TEPI
	INNER JOIN TEPI_Workflow TWF ON TEPI.FormID=TWF.FormID
	INNER JOIN mTFacility TFAC ON TEPI.FacilityID=TFAC.ID AND TEPI.OperationtypeID=TFAC.OperationTypeID AND TFAC.cActive='Y' AND TFAC.cDel='N'
	WHERE TEPI.sYear='{0}' AND TWF.nStatusID=5 AND TWF.nMonth IN (4,5,6) " + _sConditonSubFac + @"
	GROUP BY TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.nHeaderID,TFAC.Name
	UNION
	--Q3
	SELECT TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.Name 'sFacName',TFAC.nHeaderID,3 'nQuarter',CASE WHEN (COUNT(TWF.nMonth)) = 3 THEN 1 ELSE 0 END AS nStatus
	FROM TEPI_Forms TEPI
	INNER JOIN TEPI_Workflow TWF ON TEPI.FormID=TWF.FormID
	INNER JOIN mTFacility TFAC ON TEPI.FacilityID=TFAC.ID AND TEPI.OperationtypeID=TFAC.OperationTypeID AND TFAC.cActive='Y' AND TFAC.cDel='N'
	WHERE TEPI.sYear='{0}' AND TWF.nStatusID=5 AND TWF.nMonth IN (7,8,9) " + _sConditonSubFac + @"
	GROUP BY TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.nHeaderID,TFAC.Name
	UNION
	--Q4
	SELECT TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.Name 'sFacName',TFAC.nHeaderID,4 'nQuarter',CASE WHEN (COUNT(TWF.nMonth)) = 3 THEN 1 ELSE 0 END AS nStatus
	FROM TEPI_Forms TEPI
	INNER JOIN TEPI_Workflow TWF ON TEPI.FormID=TWF.FormID
	INNER JOIN mTFacility TFAC ON TEPI.FacilityID=TFAC.ID AND TEPI.OperationtypeID=TFAC.OperationTypeID AND TFAC.cActive='Y' AND TFAC.cDel='N'
	WHERE TEPI.sYear='{0}' AND TWF.nStatusID=5 AND TWF.nMonth IN (10,11,12) " + _sConditonSubFac + @"
	GROUP BY TEPI.FormID,TEPI.sYear,TEPI.IDIndicator,TEPI.OperationtypeID,TEPI.FacilityID,TFAC.nHeaderID,TFAC.Name
) AS T1
WHERE T1.nStatus=1
";
        #endregion
        string _sqlSub = string.Format(sqlSubFac, nYear);
        var lstDataSubFac = db.Database.SqlQuery<TDataSubFac>(_sqlSub).ToList();
        return lstDataSubFac;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static CResultSubmitData SubmitData(CResultGetDataPageLoad objData, CSubmitData data)
    {
        CResultSubmitData result = new CResultSubmitData();
        result.lstResultWSV = new List<TResultWSV>();
        List<TResultWSV> lstResultWSV = new List<TResultWSV>();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            int nYear = STCrypt.Decrypt(data.sYear).toIntNullToZero();
            int nFacID = STCrypt.Decrypt(data.sFacID).toIntNullToZero();
            int nIndID = STCrypt.Decrypt(data.sIndID).toIntNullToZero();
            var dataFac = db.mTFacility.FirstOrDefault(w => w.nLevel == 1 && w.cDel == "N" && w.cActive == "Y" && w.ID == nFacID);
            if (data.arrQuater != null && data.arrQuater.Length > 0)
            {
                List<int> lstQuarter = data.arrQuater.Select(s => s.toIntNullToZero()).Where(s => s != 0 && s <= 4).OrderBy(o => o).ToList();
                if (dataFac != null)
                {
                    var dataFacToPTT = db.mTFacility.FirstOrDefault(w => w.nLevel == 0 && w.cDel == "N" && w.cActive == "Y" && w.ID == dataFac.nHeaderID);
                    if (dataFacToPTT != null)
                    {
                        if (!string.IsNullOrEmpty(dataFacToPTT.sMappingCodePTT))
                        {
                            switch (nIndID)
                            {
                                case DataType.Indicator.IntensityDenonimator:
                                    {
                                        #region Intesity
                                        using (var a = new PTTEPI_Service.EPIServiceSoapClient("EPIServiceSoap"))
                                        {
                                            foreach (var itemQ in lstQuarter)
                                            {
                                                PTTEPI_Service.EPIClass_EPI dataEPI = new PTTEPI_Service.EPIClass_EPI();
                                                dataEPI.Username = UserEPIService;
                                                dataEPI.Password = PasswordEPIService;
                                                dataEPI.CompanyCode = PTTEPIServicePTTGCComCode;
                                                dataEPI.FacilityCode = dataFacToPTT.sMappingCodePTT;
                                                dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.IntensityDenominator;
                                                dataEPI.Year = nYear;
                                                dataEPI.Quarter = itemQ;

                                                if (dataFac.OperationTypeID == 11)
                                                {
                                                    List<PTTEPI_Service.EPIClass_IntensityDenominatorOther> lstInten = new List<PTTEPI_Service.EPIClass_IntensityDenominatorOther>();
                                                    #region Petrochemicals
                                                    var qSubProduct = objData.lstDataInten.Where(w => w.cTotal == "N").OrderBy(o => o.nHeaderID).ToList();
                                                    if (itemQ == 1)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominatorOther { ParentProductID = item.nHeaderID, ProductName = item.sProductName.Trims(), Month = 1, Value = item.sM1 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominatorOther { ParentProductID = item.nHeaderID, ProductName = item.sProductName.Trims(), Month = 2, Value = item.sM2 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominatorOther { ParentProductID = item.nHeaderID, ProductName = item.sProductName.Trims(), Month = 3, Value = item.sM3 });
                                                        }
                                                    }
                                                    else if (itemQ == 2)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominatorOther { ParentProductID = item.nHeaderID, ProductName = item.sProductName.Trims(), Month = 4, Value = item.sM4 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominatorOther { ParentProductID = item.nHeaderID, ProductName = item.sProductName.Trims(), Month = 5, Value = item.sM5 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominatorOther { ParentProductID = item.nHeaderID, ProductName = item.sProductName.Trims(), Month = 6, Value = item.sM6 });
                                                        }
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominatorOther { ParentProductID = item.nHeaderID, ProductName = item.sProductName.Trims(), Month = 7, Value = item.sM7 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominatorOther { ParentProductID = item.nHeaderID, ProductName = item.sProductName.Trims(), Month = 8, Value = item.sM8 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominatorOther { ParentProductID = item.nHeaderID, ProductName = item.sProductName.Trims(), Month = 9, Value = item.sM9 });
                                                        }
                                                    }
                                                    else if (itemQ == 4)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominatorOther { ParentProductID = item.nHeaderID, ProductName = item.sProductName.Trims(), Month = 10, Value = item.sM10 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominatorOther { ParentProductID = item.nHeaderID, ProductName = item.sProductName.Trims(), Month = 11, Value = item.sM11 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominatorOther { ParentProductID = item.nHeaderID, ProductName = item.sProductName.Trims(), Month = 12, Value = item.sM12 });
                                                        }
                                                    }
                                                    #endregion

                                                    #region Call API
                                                    var resultAPI = a.IntensityDenominatorAPI_D4(dataEPI, lstInten);
                                                    if (resultAPI.IsCompleted)
                                                    {
                                                        if (resultAPI.lstSubResult != null)
                                                        {
                                                            foreach (var itemAPI in resultAPI.lstSubResult)
                                                            {
                                                                lstResultWSV.Add(new TResultWSV
                                                                    {
                                                                        nFacilityID = dataFac.ID,
                                                                        sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                        nQuarter = itemQ,
                                                                        IsPass = itemAPI.Status == "S" ? true : false,
                                                                        sMsg = itemAPI.Massage
                                                                    });
                                                            }
                                                        }
                                                        result.Status = SystemFunction.process_Success;
                                                    }
                                                    else
                                                    {
                                                        result.Status = SystemFunction.process_Failed;
                                                        result.Msg = resultAPI.Message + "";

                                                        if (resultAPI.lstSubResult != null)
                                                        {
                                                            foreach (var itemAPI in resultAPI.lstSubResult)
                                                            {
                                                                lstResultWSV.Add(new TResultWSV
                                                                {
                                                                    nFacilityID = dataFac.ID,
                                                                    sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                    nQuarter = itemQ,
                                                                    IsPass = itemAPI.Status == "S" ? true : false,
                                                                    sMsg = itemAPI.Massage
                                                                });
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else if (dataFac.OperationTypeID == 13)
                                                {
                                                    List<PTTEPI_Service.EPIClass_IntensityDenominator> lstInten = new List<PTTEPI_Service.EPIClass_IntensityDenominator>();
                                                    #region Chemical Transportation & Storage
                                                    var qSubProduct = objData.lstDataInten.Where(w => w.cTotal == "N").OrderBy(o => o.nProductID).ToList();
                                                    if (itemQ == 1)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 1, Value = item.sM1 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 2, Value = item.sM2 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 3, Value = item.sM3 });
                                                        }
                                                    }
                                                    else if (itemQ == 2)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 4, Value = item.sM4 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 5, Value = item.sM5 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 6, Value = item.sM6 });
                                                        }
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 7, Value = item.sM7 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 8, Value = item.sM8 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 9, Value = item.sM9 });
                                                        }
                                                    }
                                                    else if (itemQ == 4)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 10, Value = item.sM10 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 11, Value = item.sM11 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 12, Value = item.sM12 });
                                                        }
                                                    }
                                                    #endregion

                                                    #region Call API
                                                    var resultAPI = a.IntensityDenominatorAPI_D3(dataEPI, lstInten);
                                                    if (resultAPI.IsCompleted)
                                                    {
                                                        if (resultAPI.lstSubResult != null)
                                                        {
                                                            foreach (var itemAPI in resultAPI.lstSubResult)
                                                            {
                                                                lstResultWSV.Add(new TResultWSV
                                                                {
                                                                    nFacilityID = dataFac.ID,
                                                                    sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                    nQuarter = itemQ,
                                                                    IsPass = itemAPI.Status == "S" ? true : false,
                                                                    sMsg = itemAPI.Massage
                                                                });
                                                            }
                                                        }
                                                        result.Status = SystemFunction.process_Success;
                                                    }
                                                    else
                                                    {
                                                        result.Status = SystemFunction.process_Failed;
                                                        result.Msg = resultAPI.Message + "";

                                                        if (resultAPI.lstSubResult != null)
                                                        {
                                                            foreach (var itemAPI in resultAPI.lstSubResult)
                                                            {
                                                                lstResultWSV.Add(new TResultWSV
                                                                {
                                                                    nFacilityID = dataFac.ID,
                                                                    sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                    nQuarter = itemQ,
                                                                    IsPass = itemAPI.Status == "S" ? true : false,
                                                                    sMsg = itemAPI.Massage
                                                                });
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else if (dataFac.OperationTypeID == 4)
                                                {
                                                    List<PTTEPI_Service.EPIClass_IntensityDenominator> lstInten = new List<PTTEPI_Service.EPIClass_IntensityDenominator>();
                                                    #region Refinery
                                                    var qSubProduct = objData.lstDataInten.Where(w => w.cTotal == "N").OrderBy(o => o.nProductID).ToList();
                                                    if (itemQ == 1)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 1, Value = item.sM1 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 2, Value = item.sM2 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 3, Value = item.sM3 });
                                                        }
                                                    }
                                                    else if (itemQ == 2)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 4, Value = item.sM4 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 5, Value = item.sM5 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 6, Value = item.sM6 });
                                                        }
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 7, Value = item.sM7 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 8, Value = item.sM8 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 9, Value = item.sM9 });
                                                        }
                                                    }
                                                    else if (itemQ == 4)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 10, Value = item.sM10 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 11, Value = item.sM11 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 12, Value = item.sM12 });
                                                        }
                                                    }
                                                    #endregion

                                                    #region Call API
                                                    var resultAPI = a.IntensityDenominatorAPI_D6(dataEPI, lstInten);
                                                    if (resultAPI.IsCompleted)
                                                    {
                                                        if (resultAPI.lstSubResult != null)
                                                        {
                                                            foreach (var itemAPI in resultAPI.lstSubResult)
                                                            {
                                                                lstResultWSV.Add(new TResultWSV
                                                                {
                                                                    nFacilityID = dataFac.ID,
                                                                    sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                    nQuarter = itemQ,
                                                                    IsPass = itemAPI.Status == "S" ? true : false,
                                                                    sMsg = itemAPI.Massage
                                                                });
                                                            }
                                                        }
                                                        result.Status = SystemFunction.process_Success;
                                                    }
                                                    else
                                                    {
                                                        result.Status = SystemFunction.process_Failed;
                                                        result.Msg = resultAPI.Message + "";

                                                        if (resultAPI.lstSubResult != null)
                                                        {
                                                            foreach (var itemAPI in resultAPI.lstSubResult)
                                                            {
                                                                lstResultWSV.Add(new TResultWSV
                                                                {
                                                                    nFacilityID = dataFac.ID,
                                                                    sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                    nQuarter = itemQ,
                                                                    IsPass = itemAPI.Status == "S" ? true : false,
                                                                    sMsg = itemAPI.Massage
                                                                });
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else if (dataFac.OperationTypeID == 14)
                                                {
                                                    List<PTTEPI_Service.EPIClass_IntensityDenominator> lstInten = new List<PTTEPI_Service.EPIClass_IntensityDenominator>();
                                                    #region PTT Group Building & PTT Research & Technology Institute
                                                    var qSubProduct = objData.lstDataInten.Where(w => w.cTotal == "N").OrderBy(o => o.nProductID).ToList();
                                                    if (itemQ == 1)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 1, Value = item.sM1 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 2, Value = item.sM2 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 3, Value = item.sM3 });
                                                        }
                                                    }
                                                    else if (itemQ == 2)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 4, Value = item.sM4 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 5, Value = item.sM5 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 6, Value = item.sM6 });
                                                        }
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 7, Value = item.sM7 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 8, Value = item.sM8 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 9, Value = item.sM9 });
                                                        }
                                                    }
                                                    else if (itemQ == 4)
                                                    {
                                                        foreach (var item in qSubProduct)
                                                        {
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 10, Value = item.sM10 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 11, Value = item.sM11 });
                                                            lstInten.Add(new PTTEPI_Service.EPIClass_IntensityDenominator { ProductID = item.nProductID, Month = 12, Value = item.sM12 });
                                                        }
                                                    }
                                                    #endregion

                                                    #region Call API
                                                    var resultAPI = a.IntensityDenominatorAPI_D5(dataEPI, lstInten);
                                                    if (resultAPI.IsCompleted)
                                                    {
                                                        if (resultAPI.lstSubResult != null)
                                                        {
                                                            foreach (var itemAPI in resultAPI.lstSubResult)
                                                            {
                                                                lstResultWSV.Add(new TResultWSV
                                                                {
                                                                    nFacilityID = dataFac.ID,
                                                                    sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                    nQuarter = itemQ,
                                                                    IsPass = itemAPI.Status == "S" ? true : false,
                                                                    sMsg = itemAPI.Massage
                                                                });
                                                            }
                                                        }
                                                        result.Status = SystemFunction.process_Success;
                                                    }
                                                    else
                                                    {
                                                        result.Status = SystemFunction.process_Failed;
                                                        result.Msg = resultAPI.Message + "";

                                                        if (resultAPI.lstSubResult != null)
                                                        {
                                                            foreach (var itemAPI in resultAPI.lstSubResult)
                                                            {
                                                                lstResultWSV.Add(new TResultWSV
                                                                {
                                                                    nFacilityID = dataFac.ID,
                                                                    sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                    nQuarter = itemQ,
                                                                    IsPass = itemAPI.Status == "S" ? true : false,
                                                                    sMsg = itemAPI.Massage
                                                                });
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Failed;
                                                    result.Msg = "Not mapping operaion type !";
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    break;
                                case DataType.Indicator.Water:
                                    {
                                        #region Water
                                        using (var a = new PTTEPI_Service.EPIServiceSoapClient("EPIServiceSoap"))
                                        {
                                            foreach (var itemQ in lstQuarter)
                                            {
                                                PTTEPI_Service.EPIClass_EPI dataEPI = new PTTEPI_Service.EPIClass_EPI();
                                                dataEPI.Username = UserEPIService;
                                                dataEPI.Password = PasswordEPIService;
                                                dataEPI.CompanyCode = PTTEPIServicePTTGCComCode;
                                                dataEPI.FacilityCode = dataFacToPTT.sMappingCodePTT;
                                                dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Water;
                                                dataEPI.Year = nYear;
                                                dataEPI.Quarter = itemQ;

                                                List<PTTEPI_Service.EPIClass_Water> lstWater = new List<PTTEPI_Service.EPIClass_Water>();
                                                #region Set Data
                                                var qSubProduct = objData.lstDataWater.Where(w => (w.cTotal == "N" || w.nProductID == 100)).OrderBy(o => o.nProductID).ToList();
                                                if (itemQ == 1)
                                                {
                                                    foreach (var item in qSubProduct)
                                                    {
                                                        lstWater.Add(new PTTEPI_Service.EPIClass_Water { ProductID = item.nProductID, Month = 1, Value = item.sM1 });
                                                        lstWater.Add(new PTTEPI_Service.EPIClass_Water { ProductID = item.nProductID, Month = 2, Value = item.sM2 });
                                                        lstWater.Add(new PTTEPI_Service.EPIClass_Water { ProductID = item.nProductID, Month = 3, Value = item.sM3 });
                                                    }
                                                }
                                                else if (itemQ == 2)
                                                {
                                                    foreach (var item in qSubProduct)
                                                    {
                                                        lstWater.Add(new PTTEPI_Service.EPIClass_Water { ProductID = item.nProductID, Month = 4, Value = item.sM4 });
                                                        lstWater.Add(new PTTEPI_Service.EPIClass_Water { ProductID = item.nProductID, Month = 5, Value = item.sM5 });
                                                        lstWater.Add(new PTTEPI_Service.EPIClass_Water { ProductID = item.nProductID, Month = 6, Value = item.sM6 });
                                                    }
                                                }
                                                else if (itemQ == 3)
                                                {
                                                    foreach (var item in qSubProduct)
                                                    {
                                                        lstWater.Add(new PTTEPI_Service.EPIClass_Water { ProductID = item.nProductID, Month = 7, Value = item.sM7 });
                                                        lstWater.Add(new PTTEPI_Service.EPIClass_Water { ProductID = item.nProductID, Month = 8, Value = item.sM8 });
                                                        lstWater.Add(new PTTEPI_Service.EPIClass_Water { ProductID = item.nProductID, Month = 9, Value = item.sM9 });
                                                    }
                                                }
                                                else if (itemQ == 4)
                                                {
                                                    foreach (var item in qSubProduct)
                                                    {
                                                        lstWater.Add(new PTTEPI_Service.EPIClass_Water { ProductID = item.nProductID, Month = 10, Value = item.sM10 });
                                                        lstWater.Add(new PTTEPI_Service.EPIClass_Water { ProductID = item.nProductID, Month = 11, Value = item.sM11 });
                                                        lstWater.Add(new PTTEPI_Service.EPIClass_Water { ProductID = item.nProductID, Month = 12, Value = item.sM12 });
                                                    }
                                                }
                                                #endregion

                                                #region Call API
                                                var resultAPI = a.WaterAPI(dataEPI, lstWater);
                                                if (resultAPI.IsCompleted)
                                                {
                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                    result.Status = SystemFunction.process_Success;
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Failed;
                                                    result.Msg = resultAPI.Message + "";

                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion
                                    }
                                    break;
                                case DataType.Indicator.Material:
                                    {
                                        #region Material
                                        string sRenewName = "(Renewable)";
                                        string sNonRenewName = "(Non-renewable)";
                                        string sCatalystName = "(Catalyst)";
                                        using (var a = new PTTEPI_Service.EPIServiceSoapClient("EPIServiceSoap"))
                                        {
                                            foreach (var itemQ in lstQuarter)
                                            {
                                                PTTEPI_Service.EPIClass_EPI dataEPI = new PTTEPI_Service.EPIClass_EPI();
                                                dataEPI.Username = UserEPIService;
                                                dataEPI.Password = PasswordEPIService;
                                                dataEPI.CompanyCode = PTTEPIServicePTTGCComCode;
                                                dataEPI.FacilityCode = dataFacToPTT.sMappingCodePTT;
                                                dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Material;
                                                dataEPI.Year = nYear;
                                                dataEPI.Quarter = itemQ;

                                                List<PTTEPI_Service.EPIClass_Material> lstMaterial = new List<PTTEPI_Service.EPIClass_Material>();
                                                List<PTTEPI_Service.EPIClass_MaterialData> lstMaterialData = new List<PTTEPI_Service.EPIClass_MaterialData>();
                                                List<PTTEPI_Service.EPIClass_Material_ReportingBasis> lstMaterialDataReport = new List<PTTEPI_Service.EPIClass_Material_ReportingBasis>();

                                                #region Direct Materials
                                                #region//Renewable Direct Materials Used
                                                var qSubRenewDirec = objData.lstDataMaterial.Where(w => w.nHeaderID == 35).ToList();
                                                foreach (var item in qSubRenewDirec)
                                                {
                                                    if (itemQ == 1)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 1, Value = item.sM1, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 2, Value = item.sM2, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 3, Value = item.sM3, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 2)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 4, Value = item.sM4, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 5, Value = item.sM5, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 6, Value = item.sM6, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 7, Value = item.sM7, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 8, Value = item.sM8, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 9, Value = item.sM9, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 4)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 10, Value = item.sM10, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 11, Value = item.sM11, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 12, Value = item.sM12, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                }
                                                #endregion

                                                #region//Nonrenewable Direct Materials Used
                                                var qSubNonRenewDirec = objData.lstDataMaterial.Where(w => w.nHeaderID == 36).ToList();
                                                foreach (var item in qSubNonRenewDirec)
                                                {
                                                    if (itemQ == 1)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 1, Value = item.sM1, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 2, Value = item.sM2, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 3, Value = item.sM3, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 2)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 4, Value = item.sM4, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 5, Value = item.sM5, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 6, Value = item.sM6, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 7, Value = item.sM7, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 8, Value = item.sM8, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 9, Value = item.sM9, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 4)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 10, Value = item.sM10, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 11, Value = item.sM11, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 12, Value = item.sM12, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                }
                                                #endregion

                                                #region//Reporting Basis Direct Materials Used
                                                var qRenewDirect = objData.lstDataMaterial.FirstOrDefault(w => w.nProductID == 35);
                                                if (qRenewDirect != null)
                                                {
                                                    if (itemQ == 1)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 35, Month = 1, Value = SystemFunction.ParseDecimal(qRenewDirect.sM1) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 35, Month = 2, Value = SystemFunction.ParseDecimal(qRenewDirect.sM2) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 35, Month = 3, Value = SystemFunction.ParseDecimal(qRenewDirect.sM3) + "" });
                                                    }
                                                    else if (itemQ == 2)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 35, Month = 4, Value = SystemFunction.ParseDecimal(qRenewDirect.sM4) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 35, Month = 5, Value = SystemFunction.ParseDecimal(qRenewDirect.sM5) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 35, Month = 6, Value = SystemFunction.ParseDecimal(qRenewDirect.sM6) + "" });
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 35, Month = 7, Value = SystemFunction.ParseDecimal(qRenewDirect.sM7) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 35, Month = 8, Value = SystemFunction.ParseDecimal(qRenewDirect.sM8) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 35, Month = 9, Value = SystemFunction.ParseDecimal(qRenewDirect.sM9) + "" });
                                                    }
                                                    else if (itemQ == 4)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 35, Month = 10, Value = SystemFunction.ParseDecimal(qRenewDirect.sM10) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 35, Month = 11, Value = SystemFunction.ParseDecimal(qRenewDirect.sM11) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 35, Month = 12, Value = SystemFunction.ParseDecimal(qRenewDirect.sM12) + "" });
                                                    }
                                                }
                                                #endregion

                                                lstMaterial.Add(new PTTEPI_Service.EPIClass_Material { MaterialType = "DMU", MaterialOption = "2", ReportingBasisOption = "M", lstDataMaterial = lstMaterialData, lstDataReportingBasis = lstMaterialDataReport });
                                                #endregion

                                                #region Associated Materials
                                                lstMaterialData = new List<PTTEPI_Service.EPIClass_MaterialData>();

                                                #region//Catalyst Used
                                                var qSubCatalyst = objData.lstDataMaterial.Where(w => w.nHeaderID == 38).ToList();
                                                foreach (var item in qSubCatalyst)
                                                {
                                                    if (itemQ == 1)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sCatalystName, UnitID = item.UnitID, Month = 1, Value = item.sM1, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sCatalystName, UnitID = item.UnitID, Month = 2, Value = item.sM2, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sCatalystName, UnitID = item.UnitID, Month = 3, Value = item.sM3, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 2)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sCatalystName, UnitID = item.UnitID, Month = 4, Value = item.sM4, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sCatalystName, UnitID = item.UnitID, Month = 5, Value = item.sM5, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sCatalystName, UnitID = item.UnitID, Month = 6, Value = item.sM6, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sCatalystName, UnitID = item.UnitID, Month = 7, Value = item.sM7, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sCatalystName, UnitID = item.UnitID, Month = 8, Value = item.sM8, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sCatalystName, UnitID = item.UnitID, Month = 9, Value = item.sM9, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 4)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sCatalystName, UnitID = item.UnitID, Month = 10, Value = item.sM10, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sCatalystName, UnitID = item.UnitID, Month = 11, Value = item.sM11, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sCatalystName, UnitID = item.UnitID, Month = 12, Value = item.sM12, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                }
                                                #endregion

                                                #region//Renewable Associated Materials Used
                                                var qSubRenewAsso = objData.lstDataMaterial.Where(w => w.nHeaderID == 39).ToList();
                                                foreach (var item in qSubRenewAsso)
                                                {
                                                    if (itemQ == 1)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 1, Value = item.sM1, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 2, Value = item.sM2, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 3, Value = item.sM3, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 2)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 4, Value = item.sM4, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 5, Value = item.sM5, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 6, Value = item.sM6, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 7, Value = item.sM7, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 8, Value = item.sM8, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 9, Value = item.sM9, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 4)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 10, Value = item.sM10, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 11, Value = item.sM11, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sRenewName, UnitID = item.UnitID, Month = 12, Value = item.sM12, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                }
                                                #endregion

                                                #region//Nonrenewable Associated Materials Used
                                                var qSubNonRenewAsso = objData.lstDataMaterial.Where(w => w.nHeaderID == 40).ToList();
                                                foreach (var item in qSubNonRenewAsso)
                                                {
                                                    if (itemQ == 1)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 1, Value = item.sM1, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 2, Value = item.sM2, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 3, Value = item.sM3, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 2)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 4, Value = item.sM4, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 5, Value = item.sM5, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 6, Value = item.sM6, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 7, Value = item.sM7, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 8, Value = item.sM8, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 9, Value = item.sM9, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                    else if (itemQ == 4)
                                                    {
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 10, Value = item.sM10, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 11, Value = item.sM11, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                        lstMaterialData.Add(new PTTEPI_Service.EPIClass_MaterialData { MaterialName = item.sProductName + sNonRenewName, UnitID = item.UnitID, Month = 12, Value = item.sM12, Density = SystemFunction.GetDecimalNull(item.sDensity) });
                                                    }
                                                }
                                                #endregion

                                                #region//Reporting Basis Associated Materials Used
                                                var qCatalyst = objData.lstDataMaterial.FirstOrDefault(w => w.nProductID == 38);
                                                if (qCatalyst != null)
                                                {
                                                    if (itemQ == 1)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 38, Month = 1, Value = SystemFunction.ParseDecimal(qCatalyst.sM1) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 38, Month = 2, Value = SystemFunction.ParseDecimal(qCatalyst.sM2) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 38, Month = 3, Value = SystemFunction.ParseDecimal(qCatalyst.sM3) + "" });
                                                    }
                                                    else if (itemQ == 2)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 38, Month = 4, Value = SystemFunction.ParseDecimal(qCatalyst.sM4) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 38, Month = 5, Value = SystemFunction.ParseDecimal(qCatalyst.sM5) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 38, Month = 6, Value = SystemFunction.ParseDecimal(qCatalyst.sM6) + "" });
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 38, Month = 7, Value = SystemFunction.ParseDecimal(qCatalyst.sM7) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 38, Month = 8, Value = SystemFunction.ParseDecimal(qCatalyst.sM8) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 38, Month = 9, Value = SystemFunction.ParseDecimal(qCatalyst.sM9) + "" });
                                                    }
                                                    else if (itemQ == 4)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 38, Month = 10, Value = SystemFunction.ParseDecimal(qCatalyst.sM10) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 38, Month = 11, Value = SystemFunction.ParseDecimal(qCatalyst.sM11) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 38, Month = 12, Value = SystemFunction.ParseDecimal(qCatalyst.sM12) + "" });
                                                    }
                                                }

                                                var qRenewAsso = objData.lstDataMaterial.FirstOrDefault(w => w.nProductID == 39);
                                                if (qRenewAsso != null)
                                                {
                                                    if (itemQ == 1)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 39, Month = 1, Value = SystemFunction.ParseDecimal(qRenewAsso.sM1) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 39, Month = 2, Value = SystemFunction.ParseDecimal(qRenewAsso.sM2) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 39, Month = 3, Value = SystemFunction.ParseDecimal(qRenewAsso.sM3) + "" });
                                                    }
                                                    else if (itemQ == 2)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 39, Month = 4, Value = SystemFunction.ParseDecimal(qRenewAsso.sM4) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 39, Month = 5, Value = SystemFunction.ParseDecimal(qRenewAsso.sM5) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 39, Month = 6, Value = SystemFunction.ParseDecimal(qRenewAsso.sM6) + "" });
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 39, Month = 7, Value = SystemFunction.ParseDecimal(qRenewAsso.sM7) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 39, Month = 8, Value = SystemFunction.ParseDecimal(qRenewAsso.sM8) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 39, Month = 9, Value = SystemFunction.ParseDecimal(qRenewAsso.sM9) + "" });
                                                    }
                                                    else if (itemQ == 4)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 39, Month = 10, Value = SystemFunction.ParseDecimal(qRenewAsso.sM10) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 39, Month = 11, Value = SystemFunction.ParseDecimal(qRenewAsso.sM11) + "" });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 39, Month = 12, Value = SystemFunction.ParseDecimal(qRenewAsso.sM12) + "" });
                                                    }
                                                }
                                                #endregion

                                                lstMaterial.Add(new PTTEPI_Service.EPIClass_Material { MaterialType = "AMU", MaterialOption = "2", ReportingBasisOption = "M", lstDataMaterial = lstMaterialData, lstDataReportingBasis = lstMaterialDataReport });
                                                #endregion

                                                #region Recycled Input Materials
                                                var qRecycle = objData.lstDataMaterial.FirstOrDefault(w => w.nProductID == 41);
                                                if (qRecycle != null)
                                                {
                                                    lstMaterialData = new List<PTTEPI_Service.EPIClass_MaterialData>();
                                                    lstMaterialDataReport = new List<PTTEPI_Service.EPIClass_Material_ReportingBasis>();
                                                    if (itemQ == 1)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 41, Month = 1, Value = qRecycle.sM1 });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 41, Month = 2, Value = qRecycle.sM2 });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 41, Month = 3, Value = qRecycle.sM3 });
                                                    }
                                                    else if (itemQ == 2)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 41, Month = 4, Value = qRecycle.sM4 });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 41, Month = 5, Value = qRecycle.sM5 });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 41, Month = 6, Value = qRecycle.sM6 });
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 41, Month = 7, Value = qRecycle.sM7 });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 41, Month = 8, Value = qRecycle.sM8 });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 41, Month = 9, Value = qRecycle.sM9 });
                                                    }
                                                    else if (itemQ == 3)
                                                    {
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 41, Month = 10, Value = qRecycle.sM10 });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 41, Month = 11, Value = qRecycle.sM11 });
                                                        lstMaterialDataReport.Add(new PTTEPI_Service.EPIClass_Material_ReportingBasis { ProductID = 41, Month = 12, Value = qRecycle.sM12 });
                                                    }

                                                    string sReportingBasisOption = objData.lstDataMaterial.Any(w => w.nHeaderID == 41) ? "M" : "0";
                                                    lstMaterial.Add(new PTTEPI_Service.EPIClass_Material { MaterialType = "RMU", ReportingBasisOption = sReportingBasisOption, lstDataReportingBasis = lstMaterialDataReport });
                                                }
                                                #endregion

                                                #region Call API
                                                var resultAPI = a.MaterialAPI(dataEPI, lstMaterial);
                                                if (resultAPI.IsCompleted)
                                                {
                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                    result.Status = SystemFunction.process_Success;
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Failed;
                                                    result.Msg = resultAPI.Message + "";

                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion
                                    }
                                    break;
                                case DataType.Indicator.Waste:
                                    {
                                        #region Waste
                                        int[] arrProductOnSite = new int[] { 14, 31 };
                                        using (var a = new PTTEPI_Service.EPIServiceSoapClient("EPIServiceSoap"))
                                        {
                                            foreach (var itemQ in lstQuarter)
                                            {
                                                PTTEPI_Service.EPIClass_EPI dataEPI = new PTTEPI_Service.EPIClass_EPI();
                                                dataEPI.Username = UserEPIService;
                                                dataEPI.Password = PasswordEPIService;
                                                dataEPI.CompanyCode = PTTEPIServicePTTGCComCode;
                                                dataEPI.FacilityCode = dataFacToPTT.sMappingCodePTT;
                                                dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Waste;
                                                dataEPI.Year = nYear;
                                                dataEPI.Quarter = itemQ;

                                                #region Set Data
                                                List<PTTEPI_Service.EPIClass_Waste> lstData = new List<PTTEPI_Service.EPIClass_Waste>();
                                                foreach (var item in objData.lstDataWaste)
                                                {
                                                    if (arrProductOnSite.Contains(item.nProductID))
                                                    {
                                                        lstData.Add(new PTTEPI_Service.EPIClass_Waste { ProductID = item.nProductID, Value = item.sReportingYear });
                                                    }
                                                    else
                                                    {
                                                        if (itemQ == 1)
                                                        {
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Waste { ProductID = item.nProductID, Month = 1, UnitID = item.UnitID, Value = item.sM1 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Waste { ProductID = item.nProductID, Month = 2, UnitID = item.UnitID, Value = item.sM2 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Waste { ProductID = item.nProductID, Month = 3, UnitID = item.UnitID, Value = item.sM3 });
                                                        }
                                                        else if (itemQ == 2)
                                                        {
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Waste { ProductID = item.nProductID, Month = 4, UnitID = item.UnitID, Value = item.sM4 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Waste { ProductID = item.nProductID, Month = 5, UnitID = item.UnitID, Value = item.sM5 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Waste { ProductID = item.nProductID, Month = 6, UnitID = item.UnitID, Value = item.sM6 });
                                                        }
                                                        else if (itemQ == 3)
                                                        {
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Waste { ProductID = item.nProductID, Month = 7, UnitID = item.UnitID, Value = item.sM7 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Waste { ProductID = item.nProductID, Month = 8, UnitID = item.UnitID, Value = item.sM8 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Waste { ProductID = item.nProductID, Month = 9, UnitID = item.UnitID, Value = item.sM9 });
                                                        }
                                                        else if (itemQ == 4)
                                                        {
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Waste { ProductID = item.nProductID, Month = 10, UnitID = item.UnitID, Value = item.sM10 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Waste { ProductID = item.nProductID, Month = 11, UnitID = item.UnitID, Value = item.sM11 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Waste { ProductID = item.nProductID, Month = 12, UnitID = item.UnitID, Value = item.sM12 });
                                                        }
                                                    }
                                                }
                                                #endregion

                                                #region Call API
                                                var resultAPI = a.WasteAPI(dataEPI, lstData);
                                                if (resultAPI.IsCompleted)
                                                {
                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                    result.Status = SystemFunction.process_Success;
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Failed;
                                                    result.Msg = resultAPI.Message + "";

                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion
                                    }
                                    break;
                                case DataType.Indicator.Emission:
                                    {
                                        #region Emission
                                        using (var a = new PTTEPI_Service.EPIServiceSoapClient("EPIServiceSoap"))
                                        {
                                            foreach (var itemQ in lstQuarter)
                                            {
                                                PTTEPI_Service.EPIClass_EPI dataEPI = new PTTEPI_Service.EPIClass_EPI();
                                                dataEPI.Username = UserEPIService;
                                                dataEPI.Password = PasswordEPIService;
                                                dataEPI.CompanyCode = PTTEPIServicePTTGCComCode;
                                                dataEPI.FacilityCode = dataFacToPTT.sMappingCodePTT;
                                                dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Emission;
                                                dataEPI.Year = nYear;
                                                dataEPI.Quarter = itemQ;

                                                #region Stack
                                                #region NOTE >> Stack Product ID
                                                /*
             * 
             * 
172	Flow rate	2	71 >> ส่ง
173	Operating hours	2	71 >> ส่ง
174	Actual % O<sub>2</sub> content at measurement	2	71 >> ส่ง
             * 
             * 
175	NO<sub>x</sub> Emission	2H	81 >> ไม่ส่ง
176	Monthly total emission load	2	81 >> ถ้าส่ง 176 ห้ามส่ง 177
177	Emission concerntration (at actual O<sub>2</sub> content)	2	81 >> ถ้าส่ง 177 ห้ามส่ง 176
             * 
             * 
178	SO<sub>2 </sub>Emission	2H	91 >> ไม่ส่ง
179	Monthly total emission load	2	91 >> ถ้าส่ง 179 ห้ามส่ง 180
180	Emission concerntration (at actual O<sub>2</sub> content)	2	91 >> ถ้าส่ง 180 ห้ามส่ง 179
             * 
             * 
181	TSP Emission	2H	101 >> ไม่ส่ง
182	Monthly total emission load	2	101 >> ถ้าส่ง 182 ห้ามส่ง 183
183	Emission concerntration (at actual O<sub>2</sub> content)	2	101 >> ถ้าส่ง 183 ห้ามส่ง 182
             */
                                                #endregion

                                                List<PTTEPI_Service.EPIClass_Emission_Stack> lstDataStack = new List<PTTEPI_Service.EPIClass_Emission_Stack>();
                                                if (objData.lstDataEmissionCombusion.Any() && objData.lstDataEmissionStack.Any())
                                                {
                                                    foreach (var itemS in objData.lstDataEmissionStack)
                                                    {
                                                        List<PTTEPI_Service.EPIClass_Emission_Data> lstStackDetail = new List<PTTEPI_Service.EPIClass_Emission_Data>();
                                                        if (itemS.lstProduct != null)
                                                        {
                                                            foreach (var item in itemS.lstProduct)
                                                            {
                                                                #region Flow rate, Operating hours, Actual % O2 content at measurement
                                                                if (item.nProductID == 172 || item.nProductID == 173 || item.nProductID == 174)
                                                                {
                                                                    if (itemQ == 1)
                                                                    {
                                                                        lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 1, Value = item.sM1 });
                                                                        lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 2, Value = item.sM2 });
                                                                        lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 3, Value = item.sM3 });
                                                                    }
                                                                    else if (itemQ == 2)
                                                                    {
                                                                        lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 4, Value = item.sM4 });
                                                                        lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 5, Value = item.sM5 });
                                                                        lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 6, Value = item.sM6 });
                                                                    }
                                                                    else if (itemQ == 3)
                                                                    {
                                                                        lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 7, Value = item.sM7 });
                                                                        lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 8, Value = item.sM8 });
                                                                        lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 9, Value = item.sM9 });
                                                                    }
                                                                    else if (itemQ == 4)
                                                                    {
                                                                        lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 10, Value = item.sM10 });
                                                                        lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 11, Value = item.sM11 });
                                                                        lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 12, Value = item.sM12 });
                                                                    }
                                                                }
                                                                #endregion
                                                            }

                                                            #region NOx
                                                            var qNOx2H = itemS.lstProduct.FirstOrDefault(w => w.nProductID == 175);
                                                            if (qNOx2H != null)
                                                            {
                                                                if (qNOx2H.nOptionProduct == 1)
                                                                {
                                                                    var qData = itemS.lstProduct.FirstOrDefault(w => w.nProductID == 176);
                                                                    if (qData != null)
                                                                    {
                                                                        if (itemQ == 1)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 1, Value = qData.sM1 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 2, Value = qData.sM2 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 3, Value = qData.sM3 });
                                                                        }
                                                                        else if (itemQ == 2)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 4, Value = qData.sM4 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 5, Value = qData.sM5 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 6, Value = qData.sM6 });
                                                                        }
                                                                        else if (itemQ == 3)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 7, Value = qData.sM7 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 8, Value = qData.sM8 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 9, Value = qData.sM9 });
                                                                        }
                                                                        else if (itemQ == 4)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 10, Value = qData.sM10 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 11, Value = qData.sM11 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 12, Value = qData.sM12 });
                                                                        }
                                                                    }
                                                                }
                                                                else if (qNOx2H.nOptionProduct == 2)
                                                                {
                                                                    var qData = itemS.lstProduct.FirstOrDefault(w => w.nProductID == 177);
                                                                    if (qData != null)
                                                                    {
                                                                        if (itemQ == 1)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 1, Value = qData.sM1 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 2, Value = qData.sM2 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 3, Value = qData.sM3 });
                                                                        }
                                                                        else if (itemQ == 2)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 4, Value = qData.sM4 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 5, Value = qData.sM5 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 6, Value = qData.sM6 });
                                                                        }
                                                                        else if (itemQ == 3)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 7, Value = qData.sM7 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 8, Value = qData.sM8 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 9, Value = qData.sM9 });
                                                                        }
                                                                        else if (itemQ == 4)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 10, Value = qData.sM10 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 11, Value = qData.sM11 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 12, Value = qData.sM12 });
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            #endregion

                                                            #region SOx
                                                            var qSOx2H = itemS.lstProduct.FirstOrDefault(w => w.nProductID == 178);
                                                            if (qSOx2H != null)
                                                            {
                                                                if (qSOx2H.nOptionProduct == 1)
                                                                {
                                                                    var qData = itemS.lstProduct.FirstOrDefault(w => w.nProductID == 179);
                                                                    if (qData != null)
                                                                    {
                                                                        if (itemQ == 1)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 1, Value = qData.sM1 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 2, Value = qData.sM2 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 3, Value = qData.sM3 });
                                                                        }
                                                                        else if (itemQ == 2)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 4, Value = qData.sM4 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 5, Value = qData.sM5 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 6, Value = qData.sM6 });
                                                                        }
                                                                        else if (itemQ == 3)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 7, Value = qData.sM7 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 8, Value = qData.sM8 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 9, Value = qData.sM9 });
                                                                        }
                                                                        else if (itemQ == 4)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 10, Value = qData.sM10 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 11, Value = qData.sM11 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 12, Value = qData.sM12 });
                                                                        }
                                                                    }
                                                                }
                                                                else if (qSOx2H.nOptionProduct == 2)
                                                                {
                                                                    var qData = itemS.lstProduct.FirstOrDefault(w => w.nProductID == 180);
                                                                    if (qData != null)
                                                                    {
                                                                        if (itemQ == 1)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 1, Value = qData.sM1 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 2, Value = qData.sM2 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 3, Value = qData.sM3 });
                                                                        }
                                                                        else if (itemQ == 2)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 4, Value = qData.sM4 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 5, Value = qData.sM5 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 6, Value = qData.sM6 });
                                                                        }
                                                                        else if (itemQ == 3)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 7, Value = qData.sM7 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 8, Value = qData.sM8 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 9, Value = qData.sM9 });
                                                                        }
                                                                        else if (itemQ == 4)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 10, Value = qData.sM10 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 11, Value = qData.sM11 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 12, Value = qData.sM12 });
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            #endregion

                                                            #region TSP
                                                            var qTSP2H = itemS.lstProduct.FirstOrDefault(w => w.nProductID == 181);
                                                            if (qTSP2H != null)
                                                            {
                                                                if (qTSP2H.nOptionProduct == 1)
                                                                {
                                                                    var qData = itemS.lstProduct.FirstOrDefault(w => w.nProductID == 182);
                                                                    if (qData != null)
                                                                    {
                                                                        if (itemQ == 1)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 1, Value = qData.sM1 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 2, Value = qData.sM2 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 3, Value = qData.sM3 });
                                                                        }
                                                                        else if (itemQ == 2)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 4, Value = qData.sM4 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 5, Value = qData.sM5 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 6, Value = qData.sM6 });
                                                                        }
                                                                        else if (itemQ == 3)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 7, Value = qData.sM7 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 8, Value = qData.sM8 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 9, Value = qData.sM9 });
                                                                        }
                                                                        else if (itemQ == 4)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 10, Value = qData.sM10 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 11, Value = qData.sM11 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 12, Value = qData.sM12 });
                                                                        }
                                                                    }
                                                                }
                                                                else if (qTSP2H.nOptionProduct == 2)
                                                                {
                                                                    var qData = itemS.lstProduct.FirstOrDefault(w => w.nProductID == 183);
                                                                    if (qData != null)
                                                                    {
                                                                        if (itemQ == 1)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 1, Value = qData.sM1 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 2, Value = qData.sM2 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 3, Value = qData.sM3 });
                                                                        }
                                                                        else if (itemQ == 2)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 4, Value = qData.sM4 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 5, Value = qData.sM5 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 6, Value = qData.sM6 });
                                                                        }
                                                                        else if (itemQ == 3)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 7, Value = qData.sM7 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 8, Value = qData.sM8 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 9, Value = qData.sM9 });
                                                                        }
                                                                        else if (itemQ == 4)
                                                                        {
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 10, Value = qData.sM10 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 11, Value = qData.sM11 });
                                                                            lstStackDetail.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = qData.nProductID, Month = 12, Value = qData.sM12 });
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            #endregion

                                                            lstDataStack.Add(new PTTEPI_Service.EPIClass_Emission_Stack { StackName = itemS.sStackName, lstData = lstStackDetail });
                                                        }
                                                    }

                                                }
                                                #endregion

                                                #region VOC
                                                #region NOTE >> VOC Product ID
                                                /*
194	Fugitive emission from equipment & machines
195	Emission via stack & vent from fuel combustion
196	Emission from tank farm
197	Emission from loading & unloading
198	Emission from flare
199	Emission from wastewater treatment system
             */
                                                #endregion

                                                PTTEPI_Service.EPIClass_Emission_VOC dataVOC = new PTTEPI_Service.EPIClass_Emission_VOC();
                                                List<PTTEPI_Service.EPIClass_Emission_Data> lstDataVOC = new List<PTTEPI_Service.EPIClass_Emission_Data>();
                                                string sReportOption = objData.lstDataEmissionVOC.FirstOrDefault() != null ? objData.lstDataEmissionVOC.FirstOrDefault().sOption : "M";
                                                foreach (var item in objData.lstDataEmissionVOC.Where(w => w.cTotal == "N").ToList())
                                                {
                                                    if (sReportOption == "M")
                                                    {
                                                        if (itemQ == 1)
                                                        {
                                                            lstDataVOC.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 1, Value = item.sM1 });
                                                            lstDataVOC.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 2, Value = item.sM2 });
                                                            lstDataVOC.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 3, Value = item.sM3 });
                                                        }
                                                        else if (itemQ == 2)
                                                        {
                                                            lstDataVOC.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 4, Value = item.sM4 });
                                                            lstDataVOC.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 5, Value = item.sM5 });
                                                            lstDataVOC.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 6, Value = item.sM6 });
                                                        }
                                                        else if (itemQ == 3)
                                                        {
                                                            lstDataVOC.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 7, Value = item.sM7 });
                                                            lstDataVOC.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 8, Value = item.sM8 });
                                                            lstDataVOC.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 9, Value = item.sM9 });
                                                        }
                                                        else if (itemQ == 4)
                                                        {
                                                            lstDataVOC.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 10, Value = item.sM10 });
                                                            lstDataVOC.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 11, Value = item.sM11 });
                                                            lstDataVOC.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 12, Value = item.sM12 });
                                                        }
                                                    }
                                                    else if (sReportOption == "Y")//Set month = 13
                                                    {
                                                        lstDataVOC.Add(new PTTEPI_Service.EPIClass_Emission_Data { ProductID = item.nProductID, Month = 13, Value = item.sTotal });
                                                    }
                                                }
                                                dataVOC.VOCEmissionDataAvailable = sReportOption;
                                                dataVOC.lstData = lstDataVOC;
                                                #endregion

                                                #region Call API
                                                var resultAPI = a.EmissionAPI(dataEPI, lstDataStack, dataVOC);
                                                if (resultAPI.IsCompleted)
                                                {
                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                    result.Status = SystemFunction.process_Success;
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Failed;
                                                    result.Msg = resultAPI.Message + "";

                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion
                                    }
                                    break;
                                case DataType.Indicator.Effluent:
                                    {
                                        #region Effluent
                                        using (var a = new PTTEPI_Service.EPIServiceSoapClient("EPIServiceSoap"))
                                        {
                                            foreach (var itemQ in lstQuarter)
                                            {
                                                PTTEPI_Service.EPIClass_EPI dataEPI = new PTTEPI_Service.EPIClass_EPI();
                                                dataEPI.Username = UserEPIService;
                                                dataEPI.Password = PasswordEPIService;
                                                dataEPI.CompanyCode = PTTEPIServicePTTGCComCode;
                                                dataEPI.FacilityCode = dataFacToPTT.sMappingCodePTT;
                                                dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Effluent;
                                                dataEPI.Year = nYear;
                                                dataEPI.Quarter = itemQ;

                                                #region Set Data
                                                PTTEPI_Service.EPIClass_Effluent dataEffluent = new PTTEPI_Service.EPIClass_Effluent();
                                                List<PTTEPI_Service.EPIClass_Effluent_Point> lstDataPoint = new List<PTTEPI_Service.EPIClass_Effluent_Point>();
                                                foreach (var itemP in objData.lstDataEffluentPoint)
                                                {
                                                    List<PTTEPI_Service.EPIClass_Effluent_Data> lstData = new List<PTTEPI_Service.EPIClass_Effluent_Data>();
                                                    foreach (var item in itemP.lstDataProduct)
                                                    {
                                                        if (itemQ == 1)
                                                        {
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Effluent_Data { ProductID = item.nProductID, Month = 1, Value = item.sM1 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Effluent_Data { ProductID = item.nProductID, Month = 2, Value = item.sM2 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Effluent_Data { ProductID = item.nProductID, Month = 3, Value = item.sM3 });
                                                        }
                                                        else if (itemQ == 2)
                                                        {
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Effluent_Data { ProductID = item.nProductID, Month = 4, Value = item.sM4 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Effluent_Data { ProductID = item.nProductID, Month = 5, Value = item.sM5 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Effluent_Data { ProductID = item.nProductID, Month = 6, Value = item.sM6 });
                                                        }
                                                        else if (itemQ == 3)
                                                        {
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Effluent_Data { ProductID = item.nProductID, Month = 7, Value = item.sM7 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Effluent_Data { ProductID = item.nProductID, Month = 8, Value = item.sM8 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Effluent_Data { ProductID = item.nProductID, Month = 9, Value = item.sM9 });
                                                        }
                                                        else if (itemQ == 4)
                                                        {
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Effluent_Data { ProductID = item.nProductID, Month = 10, Value = item.sM10 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Effluent_Data { ProductID = item.nProductID, Month = 11, Value = item.sM11 });
                                                            lstData.Add(new PTTEPI_Service.EPIClass_Effluent_Data { ProductID = item.nProductID, Month = 12, Value = item.sM12 });
                                                        }
                                                    }

                                                    lstDataPoint.Add(new PTTEPI_Service.EPIClass_Effluent_Point
                                                    {
                                                        PointName = itemP.sPointName,
                                                        PointType = itemP.sPointType,
                                                        DischargeTo = itemP.nDischargeTo,
                                                        TreamentMethod = itemP.nTreamentMethod,
                                                        TreamentMethodOther = itemP.sOtherTreamentMethod + "",
                                                        Area = itemP.nArea,
                                                        CODOption = SystemFunction.GetIntNullToZero(itemP.sOption2),
                                                        lstData = lstData
                                                    });
                                                }

                                                dataEffluent.MainOption = 1;//Fix
                                                if (objData.lstDataEffluentPoint.Any(x => x.sPointType == "C"))
                                                {
                                                    decimal? nPercent = objData.lstDataEffluentPoint.Max(x => SystemFunction.GetDecimalNull(x.sPercent));
                                                    dataEffluent.PercentagOfWaterWithdraw = nPercent;
                                                }

                                                #endregion

                                                #region Call API
                                                var resultAPI = a.EffluentAPI(dataEPI, dataEffluent, lstDataPoint);
                                                if (resultAPI.IsCompleted)
                                                {
                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                    result.Status = SystemFunction.process_Success;
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Failed;
                                                    result.Msg = resultAPI.Message + "";

                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion
                                    }
                                    break;
                                case DataType.Indicator.Spill:
                                    {
                                        #region Spill
                                        foreach (var item in objData.lstDataSpill)
                                        {
                                            item.dSpillDate = SystemFunction.ConvertStringToDateTime(item.sSpillDate, "");
                                        }

                                        using (var a = new PTTEPI_Service.EPIServiceSoapClient("EPIServiceSoap"))
                                        {
                                            foreach (var itemQ in lstQuarter)
                                            {
                                                PTTEPI_Service.EPIClass_EPI dataEPI = new PTTEPI_Service.EPIClass_EPI();
                                                dataEPI.Username = UserEPIService;
                                                dataEPI.Password = PasswordEPIService;
                                                dataEPI.CompanyCode = PTTEPIServicePTTGCComCode;
                                                dataEPI.FacilityCode = dataFacToPTT.sMappingCodePTT;
                                                dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Spill;
                                                dataEPI.Year = nYear;
                                                dataEPI.Quarter = itemQ;

                                                #region Set Data
                                                List<PTTEPI_Service.EPIClass_Spill> lstSpill = new List<PTTEPI_Service.EPIClass_Spill>();
                                                List<PTTEPI_Service.EPIClass_Spill_Data> lstDataSpill = new List<PTTEPI_Service.EPIClass_Spill_Data>();
                                                if (itemQ == 1)
                                                {
                                                    if (!objData.lstDataSpill.Any(x => x.dSpillDate.HasValue && x.dSpillDate.Value.Month == 1))
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 1, NoneSpill = true });
                                                    else
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 1, NoneSpill = false });

                                                    if (!objData.lstDataSpill.Any(x => x.dSpillDate.HasValue && x.dSpillDate.Value.Month == 2))
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 2, NoneSpill = true });
                                                    else
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 2, NoneSpill = false });

                                                    if (!objData.lstDataSpill.Any(x => x.dSpillDate.HasValue && x.dSpillDate.Value.Month == 3))
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 3, NoneSpill = true });
                                                    else
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 3, NoneSpill = false });

                                                }
                                                else if (itemQ == 2)
                                                {
                                                    if (!objData.lstDataSpill.Any(x => x.dSpillDate.HasValue && x.dSpillDate.Value.Month == 4))
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 4, NoneSpill = true });
                                                    else
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 4, NoneSpill = false });

                                                    if (!objData.lstDataSpill.Any(x => x.dSpillDate.HasValue && x.dSpillDate.Value.Month == 5))
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 5, NoneSpill = true });
                                                    else
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 5, NoneSpill = false });

                                                    if (!objData.lstDataSpill.Any(x => x.dSpillDate.HasValue && x.dSpillDate.Value.Month == 6))
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 6, NoneSpill = true });
                                                    else
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 6, NoneSpill = false });
                                                }
                                                else if (itemQ == 3)
                                                {
                                                    if (!objData.lstDataSpill.Any(x => x.dSpillDate.HasValue && x.dSpillDate.Value.Month == 7))
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 7, NoneSpill = true });
                                                    else
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 7, NoneSpill = false });

                                                    if (!objData.lstDataSpill.Any(x => x.dSpillDate.HasValue && x.dSpillDate.Value.Month == 8))
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 8, NoneSpill = true });
                                                    else
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 8, NoneSpill = false });

                                                    if (!objData.lstDataSpill.Any(x => x.dSpillDate.HasValue && x.dSpillDate.Value.Month == 9))
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 9, NoneSpill = true });
                                                    else
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 9, NoneSpill = false });
                                                }
                                                else if (itemQ == 4)
                                                {
                                                    if (!objData.lstDataSpill.Any(x => x.dSpillDate.HasValue && x.dSpillDate.Value.Month == 10))
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 10, NoneSpill = true });
                                                    else
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 10, NoneSpill = false });

                                                    if (!objData.lstDataSpill.Any(x => x.dSpillDate.HasValue && x.dSpillDate.Value.Month == 11))
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 11, NoneSpill = true });
                                                    else
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 11, NoneSpill = false });

                                                    if (!objData.lstDataSpill.Any(x => x.dSpillDate.HasValue && x.dSpillDate.Value.Month == 12))
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 12, NoneSpill = true });
                                                    else
                                                        lstSpill.Add(new PTTEPI_Service.EPIClass_Spill { Month = 12, NoneSpill = false });
                                                }

                                                if (itemQ == 1 || itemQ == 2 || itemQ == 3 || itemQ == 4)
                                                {
                                                    foreach (var item in objData.lstDataSpill.Where(w => w.dSpillDate.HasValue).OrderBy(o => o.nSpillID).ToList())
                                                    {
                                                        lstDataSpill.Add(new PTTEPI_Service.EPIClass_Spill_Data
                                                        {
                                                            SpillID = item.nSpillID,
                                                            PrimaryReason = item.nPrimaryReasonID,
                                                            OtherPrimary = item.sOtherPrimary,
                                                            SpillType = item.sSpillType,
                                                            SpillOf = item.nSpillOfID ?? 0,
                                                            OtherSpillOf = item.sOtherSpillOf,
                                                            Volume = item.nUnitVolumeID == 2 ? (EPIFunc.GetDecimalNull(item.sVolume) * EPIFunc.GetDecimalNull(item.sDensity)) + "" : item.sVolume,//Case unit = Tonnes convert to liter
                                                            UnitVolume = item.nUnitVolumeID == 2 ? 63 : item.nUnitVolumeID ?? 0,//2 = Tonnes, 63 = Liter
                                                            SpillTo = item.nSpillToID ?? 0,
                                                            OtherSpillTo = item.sOtherSpillTo,
                                                            SpillBy = item.nSpillByID == 93 ? 31 : item.nSpillByID ?? 0,//93 = Other, 31 = Production
                                                            SensitiveArea = item.sIsSensitiveArea,
                                                            SpillDate = new DateTime(item.dSpillDate.Value.Year, item.dSpillDate.Value.Month, item.dSpillDate.Value.Day),
                                                            Description = item.sDescription,
                                                            IncidentDescription = item.sIncidentDescription,
                                                            RecoveryAction = item.sRecoveryAction
                                                        });
                                                    }
                                                }
                                                #endregion

                                                #region Call API
                                                var resultAPI = a.SpillAPI(dataEPI, lstSpill, lstDataSpill);
                                                if (resultAPI.IsCompleted)
                                                {
                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                    result.Status = SystemFunction.process_Success;
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Failed;
                                                    result.Msg = resultAPI.Message + "";

                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion
                                    }
                                    break;
                                case DataType.Indicator.Compliance:
                                    {
                                        #region Compliance
                                        foreach (var item in objData.lstDataCompliance)
                                        {
                                            item.dComplianceDate = SystemFunction.ConvertStringToDateTime(item.sComplianceDate, "");
                                        }

                                        using (var a = new PTTEPI_Service.EPIServiceSoapClient("EPIServiceSoap"))
                                        {
                                            foreach (var itemQ in lstQuarter)
                                            {
                                                PTTEPI_Service.EPIClass_EPI dataEPI = new PTTEPI_Service.EPIClass_EPI();
                                                dataEPI.Username = UserEPIService;
                                                dataEPI.Password = PasswordEPIService;
                                                dataEPI.CompanyCode = PTTEPIServicePTTGCComCode;
                                                dataEPI.FacilityCode = dataFacToPTT.sMappingCodePTT;
                                                dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Compliance;
                                                dataEPI.Year = nYear;
                                                dataEPI.Quarter = itemQ;

                                                #region Set Data
                                                List<PTTEPI_Service.EPIClass_Compliance> lstCompliance = new List<PTTEPI_Service.EPIClass_Compliance>();
                                                List<PTTEPI_Service.EPIClass_Compliance_Data> lstDataCompliance = new List<PTTEPI_Service.EPIClass_Compliance_Data>();
                                                if (itemQ == 1)
                                                {
                                                    if (!objData.lstDataCompliance.Any(x => x.dComplianceDate.HasValue && x.dComplianceDate.Value.Month == 1))
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 1, NoneCompliance = true });
                                                    else
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 1, NoneCompliance = false });

                                                    if (!objData.lstDataCompliance.Any(x => x.dComplianceDate.HasValue && x.dComplianceDate.Value.Month == 2))
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 2, NoneCompliance = true });
                                                    else
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 2, NoneCompliance = false });

                                                    if (!objData.lstDataCompliance.Any(x => x.dComplianceDate.HasValue && x.dComplianceDate.Value.Month == 3))
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 3, NoneCompliance = true });
                                                    else
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 3, NoneCompliance = false });

                                                }
                                                else if (itemQ == 2)
                                                {
                                                    if (!objData.lstDataCompliance.Any(x => x.dComplianceDate.HasValue && x.dComplianceDate.Value.Month == 4))
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 4, NoneCompliance = true });
                                                    else
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 4, NoneCompliance = false });

                                                    if (!objData.lstDataCompliance.Any(x => x.dComplianceDate.HasValue && x.dComplianceDate.Value.Month == 5))
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 5, NoneCompliance = true });
                                                    else
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 5, NoneCompliance = false });

                                                    if (!objData.lstDataCompliance.Any(x => x.dComplianceDate.HasValue && x.dComplianceDate.Value.Month == 6))
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 6, NoneCompliance = true });
                                                    else
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 6, NoneCompliance = false });
                                                }
                                                else if (itemQ == 3)
                                                {
                                                    if (!objData.lstDataCompliance.Any(x => x.dComplianceDate.HasValue && x.dComplianceDate.Value.Month == 7))
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 7, NoneCompliance = true });
                                                    else
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 7, NoneCompliance = false });

                                                    if (!objData.lstDataCompliance.Any(x => x.dComplianceDate.HasValue && x.dComplianceDate.Value.Month == 8))
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 8, NoneCompliance = true });
                                                    else
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 8, NoneCompliance = false });

                                                    if (!objData.lstDataCompliance.Any(x => x.dComplianceDate.HasValue && x.dComplianceDate.Value.Month == 9))
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 9, NoneCompliance = true });
                                                    else
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 9, NoneCompliance = false });
                                                }
                                                else if (itemQ == 4)
                                                {
                                                    if (!objData.lstDataCompliance.Any(x => x.dComplianceDate.HasValue && x.dComplianceDate.Value.Month == 10))
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 10, NoneCompliance = true });
                                                    else
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 10, NoneCompliance = false });

                                                    if (!objData.lstDataCompliance.Any(x => x.dComplianceDate.HasValue && x.dComplianceDate.Value.Month == 11))
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 11, NoneCompliance = true });
                                                    else
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 11, NoneCompliance = false });

                                                    if (!objData.lstDataCompliance.Any(x => x.dComplianceDate.HasValue && x.dComplianceDate.Value.Month == 12))
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 12, NoneCompliance = true });
                                                    else
                                                        lstCompliance.Add(new PTTEPI_Service.EPIClass_Compliance { Month = 12, NoneCompliance = false });
                                                }

                                                if (itemQ == 1 || itemQ == 2 || itemQ == 3 || itemQ == 4)
                                                {
                                                    foreach (var item in objData.lstDataCompliance.Where(w => w.dComplianceDate.HasValue).OrderBy(o => o.nComplianceID).ToList())
                                                    {
                                                        lstDataCompliance.Add(new PTTEPI_Service.EPIClass_Compliance_Data
                                                        {
                                                            ComplianceID = item.nComplianceID,
                                                            IssueDate = new DateTime(item.dComplianceDate.Value.Year, item.dComplianceDate.Value.Month, item.dComplianceDate.Value.Day),
                                                            DocNumber = item.sDocNumber,
                                                            IssueBy = item.sIssueBy,
                                                            Subject = item.sSubject,
                                                            CorrectiveAction = item.sDetail
                                                        });
                                                    }
                                                }
                                                #endregion

                                                #region Call API
                                                var resultAPI = a.ComplianceAPI(dataEPI, lstCompliance, lstDataCompliance);
                                                if (resultAPI.IsCompleted)
                                                {
                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                    result.Status = SystemFunction.process_Success;
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Failed;
                                                    result.Msg = resultAPI.Message + "";

                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion
                                    }
                                    break;
                                case DataType.Indicator.Complaint:
                                    {
                                        #region Complaint
                                        foreach (var item in objData.lstDataComplaint)
                                        {
                                            item.dComplaintDate = SystemFunction.ConvertStringToDateTime(item.sComplaintDate, "");
                                        }

                                        using (var a = new PTTEPI_Service.EPIServiceSoapClient("EPIServiceSoap"))
                                        {
                                            foreach (var itemQ in lstQuarter)
                                            {
                                                PTTEPI_Service.EPIClass_EPI dataEPI = new PTTEPI_Service.EPIClass_EPI();
                                                dataEPI.Username = UserEPIService;
                                                dataEPI.Password = PasswordEPIService;
                                                dataEPI.CompanyCode = PTTEPIServicePTTGCComCode;
                                                dataEPI.FacilityCode = dataFacToPTT.sMappingCodePTT;
                                                dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Complaint;
                                                dataEPI.Year = nYear;
                                                dataEPI.Quarter = itemQ;

                                                #region Set Data
                                                List<PTTEPI_Service.EPIClass_Complaint> lstComplaint = new List<PTTEPI_Service.EPIClass_Complaint>();
                                                List<PTTEPI_Service.EPIClass_Complaint_Data> lstDataComplaint = new List<PTTEPI_Service.EPIClass_Complaint_Data>();
                                                if (itemQ == 1)
                                                {
                                                    if (!objData.lstDataComplaint.Any(x => x.dComplaintDate.HasValue && x.dComplaintDate.Value.Month == 1))
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 1, NoneComplaint = true });
                                                    else
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 1, NoneComplaint = false });

                                                    if (!objData.lstDataComplaint.Any(x => x.dComplaintDate.HasValue && x.dComplaintDate.Value.Month == 2))
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 2, NoneComplaint = true });
                                                    else
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 2, NoneComplaint = false });

                                                    if (!objData.lstDataComplaint.Any(x => x.dComplaintDate.HasValue && x.dComplaintDate.Value.Month == 3))
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 3, NoneComplaint = true });
                                                    else
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 3, NoneComplaint = false });

                                                }
                                                else if (itemQ == 2)
                                                {
                                                    if (!objData.lstDataComplaint.Any(x => x.dComplaintDate.HasValue && x.dComplaintDate.Value.Month == 4))
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 4, NoneComplaint = true });
                                                    else
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 4, NoneComplaint = false });

                                                    if (!objData.lstDataComplaint.Any(x => x.dComplaintDate.HasValue && x.dComplaintDate.Value.Month == 5))
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 5, NoneComplaint = true });
                                                    else
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 5, NoneComplaint = false });

                                                    if (!objData.lstDataComplaint.Any(x => x.dComplaintDate.HasValue && x.dComplaintDate.Value.Month == 6))
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 6, NoneComplaint = true });
                                                    else
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 6, NoneComplaint = false });
                                                }
                                                else if (itemQ == 3)
                                                {
                                                    if (!objData.lstDataComplaint.Any(x => x.dComplaintDate.HasValue && x.dComplaintDate.Value.Month == 7))
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 7, NoneComplaint = true });
                                                    else
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 7, NoneComplaint = false });

                                                    if (!objData.lstDataComplaint.Any(x => x.dComplaintDate.HasValue && x.dComplaintDate.Value.Month == 8))
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 8, NoneComplaint = true });
                                                    else
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 8, NoneComplaint = false });

                                                    if (!objData.lstDataComplaint.Any(x => x.dComplaintDate.HasValue && x.dComplaintDate.Value.Month == 9))
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 9, NoneComplaint = true });
                                                    else
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 9, NoneComplaint = false });
                                                }
                                                else if (itemQ == 4)
                                                {
                                                    if (!objData.lstDataComplaint.Any(x => x.dComplaintDate.HasValue && x.dComplaintDate.Value.Month == 10))
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 10, NoneComplaint = true });
                                                    else
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 10, NoneComplaint = false });

                                                    if (!objData.lstDataComplaint.Any(x => x.dComplaintDate.HasValue && x.dComplaintDate.Value.Month == 11))
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 11, NoneComplaint = true });
                                                    else
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 11, NoneComplaint = false });

                                                    if (!objData.lstDataComplaint.Any(x => x.dComplaintDate.HasValue && x.dComplaintDate.Value.Month == 12))
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 12, NoneComplaint = true });
                                                    else
                                                        lstComplaint.Add(new PTTEPI_Service.EPIClass_Complaint { Month = 12, NoneComplaint = false });
                                                }

                                                if (itemQ == 1 || itemQ == 2 || itemQ == 3 || itemQ == 4)
                                                {
                                                    foreach (var item in objData.lstDataComplaint.Where(w => w.dComplaintDate.HasValue).OrderBy(o => o.nComplaintID).ToList())
                                                    {
                                                        lstDataComplaint.Add(new PTTEPI_Service.EPIClass_Complaint_Data
                                                        {
                                                            ComplaintID = item.nComplaintID,
                                                            IssueDate = new DateTime(item.dComplaintDate.Value.Year, item.dComplaintDate.Value.Month, item.dComplaintDate.Value.Day),
                                                            IssueBy = item.sIssueBy,
                                                            Subject = item.sSubject,
                                                            CorrectiveAction = item.sCorrectiveAction
                                                        });
                                                    }
                                                }
                                                #endregion

                                                #region Call API
                                                var resultAPI = a.ComplaintAPI(dataEPI, lstComplaint, lstDataComplaint);
                                                if (resultAPI.IsCompleted)
                                                {
                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                    result.Status = SystemFunction.process_Success;
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Failed;
                                                    result.Msg = resultAPI.Message + "";

                                                    if (resultAPI.lstSubResult != null)
                                                    {
                                                        foreach (var itemAPI in resultAPI.lstSubResult)
                                                        {
                                                            lstResultWSV.Add(new TResultWSV
                                                            {
                                                                nFacilityID = dataFac.ID,
                                                                sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                                                nQuarter = itemQ,
                                                                IsPass = itemAPI.Status == "S" ? true : false,
                                                                sMsg = itemAPI.Massage
                                                            });
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion
                                    }
                                    break;
                                default:
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Invalid Indicator !";
                                    break;
                            }

                            //Update Workflow & Log transfer
                            if (result.Status == SystemFunction.process_Success)
                            {
                                var qResultPass = lstResultWSV.Where(w => w.IsPass).OrderBy(o => o.nQuarter).ToList();
                                foreach (var item in qResultPass)
                                {
                                    #region TEPI_TransferPTT
                                    var qTransfer = db.TEPI_TransferPTT.FirstOrDefault(w => w.nYear == nYear && w.nFacilityID == item.nFacilityID && w.nIndicatorID == nIndID && w.nQuarter == item.nQuarter);
                                    if (qTransfer == null)
                                    {
                                        TEPI_TransferPTT t1 = new TEPI_TransferPTT();
                                        t1.nYear = nYear;
                                        t1.nFacilityID = item.nFacilityID;
                                        t1.nIndicatorID = nIndID;
                                        t1.nQuarter = item.nQuarter;
                                        t1.nStatusID = 28;
                                        t1.nActionBy = UserAcc.GetObjUser().nUserID;
                                        t1.dAction = DateTime.Now;
                                        db.TEPI_TransferPTT.Add(t1);
                                    }
                                    else
                                    {
                                        qTransfer.nStatusID = 28;
                                        qTransfer.nActionBy = UserAcc.GetObjUser().nUserID;
                                        qTransfer.dAction = DateTime.Now;
                                    }
                                    db.SaveChanges();
                                    #endregion

                                    #region TEPI_TransferPTT_SubFacility
                                    var qSubFacTransfer = objData.lstSubFac.Where(w => w.nHeaderID == item.nFacilityID && w.nQuarter == item.nQuarter).ToList();
                                    foreach (var itemSub in qSubFacTransfer)
                                    {
                                        var qSubFacTransPTT = db.TEPI_TransferPTT_SubFacility.FirstOrDefault(w => w.nYear == nYear && w.nHeaderID == item.nFacilityID && w.nFacilityID == itemSub.FacilityID && w.nIndicatorID == nIndID && w.nQuarterID == itemSub.nQuarter);
                                        if (qSubFacTransPTT == null)
                                        {
                                            TEPI_TransferPTT_SubFacility t2 = new TEPI_TransferPTT_SubFacility();
                                            t2.nYear = nYear;
                                            t2.nHeaderID = item.nFacilityID;
                                            t2.nFacilityID = itemSub.FacilityID;
                                            t2.nIndicatorID = nIndID;
                                            t2.nQuarterID = itemSub.nQuarter;
                                            t2.nStatusID = 31;
                                            t2.dAction = DateTime.Now;
                                            db.TEPI_TransferPTT_SubFacility.Add(t2);
                                        }
                                        else
                                        {
                                            qSubFacTransPTT.nQuarterID = itemSub.nQuarter;
                                            qSubFacTransPTT.nStatusID = 31;
                                            qSubFacTransPTT.dAction = DateTime.Now;
                                        }
                                        db.SaveChanges();
                                    }
                                    #endregion

                                    #region TEPI_TransferPTT_Log
                                    TEPI_TransferPTT_Log tl = new TEPI_TransferPTT_Log();
                                    tl.nYear = nYear;
                                    tl.nFacilityID = item.nFacilityID;
                                    tl.nIndicatorID = nIndID;
                                    tl.nQuarter = item.nQuarter;
                                    tl.nStatusID = 28;
                                    tl.nActionBy = UserAcc.GetObjUser().nUserID;
                                    tl.dAction = DateTime.Now;
                                    db.TEPI_TransferPTT_Log.Add(tl);
                                    db.SaveChanges();
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Please Mapping PTT Code !";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Not found PTT facility !";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Not found facility !";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Please select quarter !";
            }
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        result.lstResultWSV = lstResultWSV.OrderBy(o => o.nQuarter).ToList();
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static CResultSubmitData RequestEditData(CSubmitData data)
    {
        CResultSubmitData result = new CResultSubmitData();
        result.lstResultWSV = new List<TResultWSV>();
        List<TResultWSV> lstResultWSV = new List<TResultWSV>();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            int nYear = STCrypt.Decrypt(data.sYear).toIntNullToZero();
            int nFacID = STCrypt.Decrypt(data.sFacID).toIntNullToZero();
            int nIndID = STCrypt.Decrypt(data.sIndID).toIntNullToZero();
            var dataFac = db.mTFacility.FirstOrDefault(w => w.nLevel == 1 && w.cDel == "N" && w.cActive == "Y" && w.ID == nFacID);
            if (data.arrQuater != null && data.arrQuater.Length > 0)
            {
                List<int> lstQuarter = data.arrQuater.Select(s => s.toIntNullToZero()).Where(s => s != 0 && s <= 4).OrderBy(o => o).ToList();
                if (dataFac != null)
                {
                    var dataFacToPTT = db.mTFacility.FirstOrDefault(w => w.nLevel == 0 && w.cDel == "N" && w.cActive == "Y" && w.ID == dataFac.nHeaderID);
                    if (dataFacToPTT != null)
                    {
                        if (!string.IsNullOrEmpty(dataFacToPTT.sMappingCodePTT))
                        {
                            using (var a = new PTTEPI_Service.EPIServiceSoapClient("EPIServiceSoap"))
                            {
                                foreach (var itemQ in lstQuarter)
                                {
                                    PTTEPI_Service.EPIClass_EPI dataEPI = new PTTEPI_Service.EPIClass_EPI();
                                    dataEPI.Username = UserEPIService;
                                    dataEPI.Password = PasswordEPIService;
                                    dataEPI.CompanyCode = PTTEPIServicePTTGCComCode;
                                    dataEPI.FacilityCode = dataFacToPTT.sMappingCodePTT;
                                    switch (nIndID)
                                    {
                                        case DataType.Indicator.IntensityDenonimator: dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.IntensityDenominator; break;
                                        case DataType.Indicator.Material: dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Material; break;
                                        case DataType.Indicator.Waste: dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Waste; break;
                                        case DataType.Indicator.Water: dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Water; break;
                                        case DataType.Indicator.Effluent: dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Effluent; break;
                                        case DataType.Indicator.Emission: dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Emission; break;
                                        case DataType.Indicator.Spill: dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Spill; break;
                                        case DataType.Indicator.Complaint: dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Complaint; break;
                                        case DataType.Indicator.Compliance: dataEPI.IndicatorCode = PTTEPIServiceInidcatorCode.Compliance; break;
                                    }

                                    dataEPI.Year = nYear;
                                    dataEPI.Quarter = itemQ;
                                    dataEPI.Comment = data.sComment;

                                    #region Call API
                                    var resultAPI = a.RequestEditAPI(dataEPI);
                                    if (resultAPI.IsCompleted)
                                    {
                                        lstResultWSV.Add(new TResultWSV
                                        {
                                            nFacilityID = dataFac.ID,
                                            sFacilityCode = dataFacToPTT.sMappingCodePTT,
                                            nQuarter = itemQ,
                                            IsPass = true,
                                            sMsg = "Successfully."
                                        });
                                        result.Status = SystemFunction.process_Success;
                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = resultAPI.Message + "";
                                    }
                                    #endregion
                                }
                            }

                            //Update Workflow & Log transfer
                            int nStatusReqEdit = 32;
                            if (result.Status == SystemFunction.process_Success)
                            {
                                var qResultPass = lstResultWSV.Where(w => w.IsPass).OrderBy(o => o.nQuarter).ToList();
                                foreach (var item in qResultPass)
                                {
                                    #region TEPI_TransferPTT
                                    var qTransfer = db.TEPI_TransferPTT.FirstOrDefault(w => w.nYear == nYear && w.nFacilityID == item.nFacilityID && w.nIndicatorID == nIndID && w.nQuarter == item.nQuarter);
                                    if (qTransfer == null)
                                    {
                                        TEPI_TransferPTT t1 = new TEPI_TransferPTT();
                                        t1.nYear = nYear;
                                        t1.nFacilityID = item.nFacilityID;
                                        t1.nIndicatorID = nIndID;
                                        t1.nQuarter = item.nQuarter;
                                        t1.nStatusID = nStatusReqEdit;
                                        t1.nActionBy = UserAcc.GetObjUser().nUserID;
                                        t1.dAction = DateTime.Now;
                                        db.TEPI_TransferPTT.Add(t1);
                                    }
                                    else
                                    {
                                        qTransfer.nStatusID = nStatusReqEdit;
                                        qTransfer.nActionBy = UserAcc.GetObjUser().nUserID;
                                        qTransfer.dAction = DateTime.Now;
                                    }
                                    db.SaveChanges();
                                    #endregion

                                    #region TEPI_TransferPTT_Log
                                    TEPI_TransferPTT_Log tl = new TEPI_TransferPTT_Log();
                                    tl.nYear = nYear;
                                    tl.nFacilityID = item.nFacilityID;
                                    tl.nIndicatorID = nIndID;
                                    tl.nQuarter = item.nQuarter;
                                    tl.nStatusID = nStatusReqEdit;
                                    tl.sRemark = data.sComment;
                                    tl.nActionBy = UserAcc.GetObjUser().nUserID;
                                    tl.dAction = DateTime.Now;
                                    db.TEPI_TransferPTT_Log.Add(tl);
                                    db.SaveChanges();
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Please Mapping PTT Code !";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Not found PTT facility !";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Not found facility !";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Please select quarter !";
            }
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }

        result.lstResultWSV = lstResultWSV.OrderBy(o => o.nQuarter).ToList();
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod RecalculateToL0(CSubmitData data)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            int nYear = STCrypt.Decrypt(data.sYear).toIntNullToZero();
            int nFacID = STCrypt.Decrypt(data.sFacID).toIntNullToZero();
            int nIndID = STCrypt.Decrypt(data.sIndID).toIntNullToZero();
            int nUserID = UserAcc.GetObjUser().nUserID;
            if (data.arrQuater != null && data.arrQuater.Length > 0)
            {
                List<int> lstQuarter = data.arrQuater.Select(s => s.toIntNullToZero()).Where(s => s != 0 && s <= 4).OrderBy(o => o).ToList();
                var dataFac = db.mTFacility.FirstOrDefault(w => w.nLevel == 1 && w.cDel == "N" && w.cActive == "Y" && w.ID == nFacID);
                if (dataFac != null)
                {
                    var dataTransfer = db.TEPI_TransferPTT.Where(w => w.nYear == nYear && w.nIndicatorID == nIndID && lstQuarter.Contains(w.nQuarter) && w.nFacilityID == nFacID).OrderBy(o => o.nQuarter).ToList();
                    foreach (var itemT in dataTransfer)
                    {
                        if (arrStatusRecalcuate.Contains(itemT.nStatusID ?? 0))
                        {
                            List<int> lstSubFac = new List<int>();
                            var querySubFac = db.TEPI_TransferPTT_SubFacility.Where(w => w.nHeaderID == itemT.nFacilityID && w.nYear == itemT.nYear && w.nIndicatorID == itemT.nIndicatorID && w.nQuarterID == itemT.nQuarter).ToList();
                            foreach (var item in querySubFac)
                            {
                                lstSubFac.Add(item.nFacilityID);
                            }

                            List<int> lstMonth = new List<int>();
                            switch (itemT.nQuarter)
                            {
                                case 1: lstMonth.Add(1); lstMonth.Add(2); lstMonth.Add(3); break;
                                case 2: lstMonth.Add(4); lstMonth.Add(5); lstMonth.Add(6); break;
                                case 3: lstMonth.Add(7); lstMonth.Add(8); lstMonth.Add(9); break;
                                case 4: lstMonth.Add(10); lstMonth.Add(11); lstMonth.Add(12); break;
                            }

                            List<TDataUpdateLog> lstDataLogWF = new List<TDataUpdateLog>();
                            #region TEPI_Workflow
                            string sYear = nYear + "";
                            var dataEPIFrom = db.TEPI_Forms.Where(w => w.IDIndicator == nIndID && w.sYear == sYear && lstSubFac.Contains(w.FacilityID ?? 0)).ToList();
                            foreach (var item in dataEPIFrom)
                            {
                                var queryUpdateWF = db.TEPI_Workflow.Where(w => w.FormID == item.FormID && lstMonth.Contains(w.nMonth)).ToList();
                                foreach (var itemWF in queryUpdateWF)
                                {
                                    lstDataLogWF.Add(new TDataUpdateLog { nReportID = itemWF.nReportID, nMonth = itemWF.nMonth, nFromStatusID = itemWF.nStatusID, nToStatusID = 0 });
                                    itemWF.nStatusID = 0;
                                    itemWF.nHistoryStatusID = 0;
                                    itemWF.dAction = DateTime.Now;
                                    itemWF.nActionBy = nUserID;
                                }
                            }
                            #endregion

                            itemT.nStatusID = 0;//TEPI_TransferPTT >> Update transfer data.
                            db.SaveChanges();

                            //Update Log Workflow
                            foreach (var itemL in lstDataLogWF)
                            {
                                new Workflow().SaveLog(itemL.nReportID, itemL.nToStatusID ?? 0, itemL.nFromStatusID ?? 0, data.sComment);
                            }

                            #region Send Email
                            if (lstSubFac.Any())
                            {
                                var dataUser = (from u in db.mTUser.Where(w => w.cActive == "Y" && w.cDel == "N")
                                                from f in db.mTUser_FacilityPermission.Where(w => w.nUserID == u.ID && w.nRoleID == 2 && w.nGroupIndicatorID == nIndID && w.nPermission == 2)//Role Operational User(L0)
                                                from l in lstSubFac.Where(w => w == f.nFacilityID)
                                                group new { u } by new { u.Email } into grp
                                                select new
                                                {
                                                    grp.Key.Email
                                                }).ToList();
                                var dataGroupIndicator = db.mTIndicator.FirstOrDefault(w => w.ID == nIndID);
                                var dataSubFacility = (from u in db.mTUser.Where(w => w.cActive == "Y" && w.cDel == "N")
                                                       from f in db.mTUser_FacilityPermission.Where(w => w.nUserID == u.ID && w.nRoleID == 2 && w.nGroupIndicatorID == nIndID && w.nPermission == 2)//Role Operational User(L0)
                                                       from l in lstSubFac.Where(w => w == f.nFacilityID)
                                                       from fac in db.mTFacility.Where(w => w.ID == l && w.cDel == "N" && w.cActive == "Y" && w.nLevel == 2)
                                                       group new { fac } by new { fac.ID, fac.Name } into grp
                                                       select new
                                                       {
                                                           grp.Key.ID,
                                                           grp.Key.Name
                                                       }).ToList();
                                List<string> lstMonthName = new List<string>();
                                foreach (var itemMonth in lstDataLogWF.OrderBy(o => o.nMonth).ToList())
                                {
                                    lstMonthName.Add(new Workflow().ConvertMonthToString(itemMonth.nMonth));
                                }

                                string sIndName = dataGroupIndicator != null ? dataGroupIndicator.Indicator : "";
                                string sSubFacName = String.Join(", ", dataSubFacility.OrderBy(o => o.Name).Select(s => s.Name).ToList());
                                string sMonth = String.Join(", ", lstMonthName);

                                string sUrlWebSite = ConfigurationSettings.AppSettings["UrlSite"].ToString();
                                string _sUrl = sUrlWebSite + "AD/loginAD.aspx";
                                string sFrom = System.Configuration.ConfigurationSettings.AppSettings["SystemMail"] + "";
                                string sTo = String.Join(",", dataUser.Select(s => s.Email).ToList());
                                string sSubject = string.Format("Recalcuated by ENVI Corporate (L2) of {0} of {1} {2} ", sIndName, sMonth, nYear); // 0 = Group Indicator,1=Month,2=Year
                                string sMsg = @"Dear All <br />" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Environmental Report has been recalculated for your edit as detailed below.<br />" +
                            "Facility : {0}<br />" +
                            "Group Indicator : {1}<br />" +
                            "Year : {2}<br />" +
                            "Month : {3}<br /><br />" +
                            "Comment : {4}<br /><br />" +
                            "Click <a href='{5}' target='_blank'>link</a> to view for further action.";
                                string sMsgSend = string.Format(sMsg, sSubFacName, sIndName, sYear, sMonth, (data.sComment + "").Replace("\n", "<br/>"), "");
                                Workflow.DataMail_log Log = new Workflow.DataMail_log();
                                Log = SystemFunction.SendMailAll(sFrom, sTo, "", "", sSubject, sMsgSend, "");
                                new Workflow().SaveLogMail(Log);
                            }
                            #endregion
                        }
                    }
                    result.Status = SystemFunction.process_Success;
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Not found facility !";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Please select quarter !";
            }
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    #region class
    [Serializable]
    public class TDataSubFac
    {
        public int FormID { get; set; }
        public string sYear { get; set; }
        public int IDIndicator { get; set; }
        public int OperationtypeID { get; set; }
        public int FacilityID { get; set; }
        public string sFacName { get; set; }
        public int nHeaderID { get; set; }
        public int nQuarter { get; set; }
    }

    [Serializable]
    public class TDataMainFaciltiy
    {
        public int nPTTFacID { get; set; }
        public string sPTTFacName { get; set; }
        public string sMappingCodePTT { get; set; }
        public int nGCFacID { get; set; }
        public string sGCFacName { get; set; }
    }

    [Serializable]
    public class CResultGetDataPageLoad : sysGlobalClass.CResutlWebMethod
    {
        public int nIndID { get; set; }
        public int nOperationtypeID { get; set; }
        public List<TDatIntesity> lstDataInten { get; set; }
        public List<TDataWater> lstDataWater { get; set; }
        public List<TDataMaterial> lstDataMaterial { get; set; }
        public List<TDataWaste> lstDataWaste { get; set; }
        public List<TDataEmission_Product> lstDataEmissionCombusion { get; set; }
        public List<TDataEmission_Stack> lstDataEmissionStack { get; set; }
        public List<TDataEmission_Product> lstDataEmissionVOC { get; set; }
        public List<TDataEffluent_Product> lstDataEffluentSumamry { get; set; }
        public List<TDataEffluent_Product> lstDataEffluentOutput { get; set; }
        public List<TDataEffluent_Point> lstDataEffluentPoint { get; set; }
        public List<TDataSpill_Product> lstDataSpillProduct { get; set; }
        public List<TDataSpill> lstDataSpill { get; set; }
        public List<TDataCompliance_Product> lstDataComplianceProduct { get; set; }
        public List<TDataCompliance> lstDataCompliance { get; set; }
        public List<TDataComplaint_Product> lstDataComplaintProduct { get; set; }
        public List<TDataComplaint> lstDataComplaint { get; set; }
        public List<TDataSubFac> lstSubFac { get; set; }
    }

    [Serializable]
    public class TDatIntesity
    {
        public int nProductID { get; set; }
        public int nHeaderID { get; set; }
        public string sProductName { get; set; }
        public string sUnit { get; set; }
        public int? UnitID { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }

        public string sM1 { get; set; }
        public string sM2 { get; set; }
        public string sM3 { get; set; }
        public string sM4 { get; set; }
        public string sM5 { get; set; }
        public string sM6 { get; set; }
        public string sM7 { get; set; }
        public string sM8 { get; set; }
        public string sM9 { get; set; }
        public string sM10 { get; set; }
        public string sM11 { get; set; }
        public string sM12 { get; set; }
        public string sTotal { get; set; }

        public string sShowM1 { get; set; }
        public string sShowM2 { get; set; }
        public string sShowM3 { get; set; }
        public string sShowM4 { get; set; }
        public string sShowM5 { get; set; }
        public string sShowM6 { get; set; }
        public string sShowM7 { get; set; }
        public string sShowM8 { get; set; }
        public string sShowM9 { get; set; }
        public string sShowM10 { get; set; }
        public string sShowM11 { get; set; }
        public string sShowM12 { get; set; }
        public string sShowTotal { get; set; }
    }

    public class TDataWater
    {
        public int nProductID { get; set; }
        public string sProductName { get; set; }
        public string sUnit { get; set; }
        public int? UnitID { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }

        public string sM1 { get; set; }
        public string sM2 { get; set; }
        public string sM3 { get; set; }
        public string sM4 { get; set; }
        public string sM5 { get; set; }
        public string sM6 { get; set; }
        public string sM7 { get; set; }
        public string sM8 { get; set; }
        public string sM9 { get; set; }
        public string sM10 { get; set; }
        public string sM11 { get; set; }
        public string sM12 { get; set; }
        public string sTotal { get; set; }

        public string sShowM1 { get; set; }
        public string sShowM2 { get; set; }
        public string sShowM3 { get; set; }
        public string sShowM4 { get; set; }
        public string sShowM5 { get; set; }
        public string sShowM6 { get; set; }
        public string sShowM7 { get; set; }
        public string sShowM8 { get; set; }
        public string sShowM9 { get; set; }
        public string sShowM10 { get; set; }
        public string sShowM11 { get; set; }
        public string sShowM12 { get; set; }
        public string sShowTotal { get; set; }
    }

    public class TDataMaterial
    {
        public int nProductID { get; set; }
        public int? nHeaderID { get; set; }
        public string sProductName { get; set; }
        public string sUnit { get; set; }
        public int UnitID { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nOption { get; set; }
        public string sSetHeader { get; set; }
        public string sDensity { get; set; }

        public string sM1 { get; set; }
        public string sM2 { get; set; }
        public string sM3 { get; set; }
        public string sM4 { get; set; }
        public string sM5 { get; set; }
        public string sM6 { get; set; }
        public string sM7 { get; set; }
        public string sM8 { get; set; }
        public string sM9 { get; set; }
        public string sM10 { get; set; }
        public string sM11 { get; set; }
        public string sM12 { get; set; }
        public string sTotal { get; set; }

        public string sShowM1 { get; set; }
        public string sShowM2 { get; set; }
        public string sShowM3 { get; set; }
        public string sShowM4 { get; set; }
        public string sShowM5 { get; set; }
        public string sShowM6 { get; set; }
        public string sShowM7 { get; set; }
        public string sShowM8 { get; set; }
        public string sShowM9 { get; set; }
        public string sShowM10 { get; set; }
        public string sShowM11 { get; set; }
        public string sShowM12 { get; set; }
        public string sShowTotal { get; set; }
    }

    public class TDataWaste
    {
        public int nProductID { get; set; }
        public string sProductName { get; set; }
        public string sUnit { get; set; }
        public int UnitID { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCal { get; set; }
        public string sType { get; set; }
        public int? nOrder { get; set; }

        public string sM1 { get; set; }
        public string sM2 { get; set; }
        public string sM3 { get; set; }
        public string sM4 { get; set; }
        public string sM5 { get; set; }
        public string sM6 { get; set; }
        public string sM7 { get; set; }
        public string sM8 { get; set; }
        public string sM9 { get; set; }
        public string sM10 { get; set; }
        public string sM11 { get; set; }
        public string sM12 { get; set; }
        public string sTotal { get; set; }
        public string sPreviousYear { get; set; }
        public string sReportingYear { get; set; }

        public string sShowM1 { get; set; }
        public string sShowM2 { get; set; }
        public string sShowM3 { get; set; }
        public string sShowM4 { get; set; }
        public string sShowM5 { get; set; }
        public string sShowM6 { get; set; }
        public string sShowM7 { get; set; }
        public string sShowM8 { get; set; }
        public string sShowM9 { get; set; }
        public string sShowM10 { get; set; }
        public string sShowM11 { get; set; }
        public string sShowM12 { get; set; }
        public string sShowTotal { get; set; }
        public string sShowPreviousYear { get; set; }
        public string sShowReportingYear { get; set; }
    }

    [Serializable]
    public class TDataEmission_Stack
    {
        public string sStackName { get; set; }
        public string sRemark { get; set; }
        public List<TDataEmission_Product> lstProduct { get; set; }
    }

    [Serializable]
    public class TDataEmission_Product
    {
        public int nProductID { get; set; }
        public string sProductName { get; set; }
        public int? nOrder { get; set; }
        public string sUnit { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCal { get; set; }
        public string sType { get; set; }
        public int? nOptionProduct { get; set; }
        /// <summary>
        /// VOC : Y = Yearly, M = Monthly
        /// </summary>
        public string sOption { get; set; }

        public string sM1 { get; set; }
        public string sM2 { get; set; }
        public string sM3 { get; set; }
        public string sM4 { get; set; }
        public string sM5 { get; set; }
        public string sM6 { get; set; }
        public string sM7 { get; set; }
        public string sM8 { get; set; }
        public string sM9 { get; set; }
        public string sM10 { get; set; }
        public string sM11 { get; set; }
        public string sM12 { get; set; }
        public string sTotal { get; set; }

        public string sShowM1 { get; set; }
        public string sShowM2 { get; set; }
        public string sShowM3 { get; set; }
        public string sShowM4 { get; set; }
        public string sShowM5 { get; set; }
        public string sShowM6 { get; set; }
        public string sShowM7 { get; set; }
        public string sShowM8 { get; set; }
        public string sShowM9 { get; set; }
        public string sShowM10 { get; set; }
        public string sShowM11 { get; set; }
        public string sShowM12 { get; set; }
        public string sShowTotal { get; set; }
    }

    [Serializable]
    public class TDataEffluent_Product
    {
        public int nProductID { get; set; }
        public string sProductName { get; set; }
        public decimal? nOrder { get; set; }
        public string sUnit { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public string sType { get; set; }

        public string sM1 { get; set; }
        public string sM2 { get; set; }
        public string sM3 { get; set; }
        public string sM4 { get; set; }
        public string sM5 { get; set; }
        public string sM6 { get; set; }
        public string sM7 { get; set; }
        public string sM8 { get; set; }
        public string sM9 { get; set; }
        public string sM10 { get; set; }
        public string sM11 { get; set; }
        public string sM12 { get; set; }
        public string sTotal { get; set; }

        public string sShowM1 { get; set; }
        public string sShowM2 { get; set; }
        public string sShowM3 { get; set; }
        public string sShowM4 { get; set; }
        public string sShowM5 { get; set; }
        public string sShowM6 { get; set; }
        public string sShowM7 { get; set; }
        public string sShowM8 { get; set; }
        public string sShowM9 { get; set; }
        public string sShowM10 { get; set; }
        public string sShowM11 { get; set; }
        public string sShowM12 { get; set; }
        public string sShowTotal { get; set; }
    }

    [Serializable]
    public class TDataEffluent_Point
    {
        public string sPointName { get; set; }
        public string sOption1 { get; set; }
        public string sOption2 { get; set; }
        public string sPercent { get; set; }
        public int nDischargeTo { get; set; }
        public int nTreamentMethod { get; set; }
        public string sOtherTreamentMethod { get; set; }
        public int nArea { get; set; }
        public string sPointType { get; set; }
        public List<TDataEffluent_Product> lstDataProduct { get; set; }
    }

    [Serializable]
    public class TDataSpill_Product
    {
        public int nProductID { get; set; }
        public string sProductName { get; set; }
        public int? nOrder { get; set; }
        public string sUnit { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public string sType { get; set; }

        public string sM1 { get; set; }
        public string sM2 { get; set; }
        public string sM3 { get; set; }
        public string sM4 { get; set; }
        public string sM5 { get; set; }
        public string sM6 { get; set; }
        public string sM7 { get; set; }
        public string sM8 { get; set; }
        public string sM9 { get; set; }
        public string sM10 { get; set; }
        public string sM11 { get; set; }
        public string sM12 { get; set; }
        public string sTotal { get; set; }

        public string sShowM1 { get; set; }
        public string sShowM2 { get; set; }
        public string sShowM3 { get; set; }
        public string sShowM4 { get; set; }
        public string sShowM5 { get; set; }
        public string sShowM6 { get; set; }
        public string sShowM7 { get; set; }
        public string sShowM8 { get; set; }
        public string sShowM9 { get; set; }
        public string sShowM10 { get; set; }
        public string sShowM11 { get; set; }
        public string sShowM12 { get; set; }
        public string sShowTotal { get; set; }
    }

    [Serializable]
    public class TDataSpill
    {
        public int nFormID { get; set; }
        public int nFacilityID { get; set; }
        /// <summary>
        /// nSpillID = nSpillID * FormID
        /// </summary>
        public int nSpillID { get; set; }
        public int nPrimaryReasonID { get; set; }
        public string sPrimaryReasonName { get; set; }
        public string sOtherPrimary { get; set; }
        public string sSpillType { get; set; }
        public string sSpillTypeName { get; set; }
        public int? nSpillOfID { get; set; }
        public string sSpillOfName { get; set; }
        public string sOtherSpillOf { get; set; }
        public string sVolume { get; set; }
        public int? nUnitVolumeID { get; set; }
        public string sUnitName { get; set; }
        public int? nSpillToID { get; set; }
        public string sSpillToName { get; set; }
        public int? nSpillByID { get; set; }
        public string sSpillByName { get; set; }
        public int? nSignification1ID { get; set; }
        public int? nSignification2ID { get; set; }
        public DateTime? dSpillDate { get; set; }
        public string sSpillDate { get; set; }
        public string sDescription { get; set; }
        public string sIncidentDescription { get; set; }
        public string sRecoveryAction { get; set; }
        public string sOtherSpillTo { get; set; }
        public string sIsSensitiveArea { get; set; }
        public string sDensity { get; set; }
        public string sOtherSpillBy { get; set; }

        /// <summary>
        /// 1 = Spill, 2 = Sign. spill, 3 = None
        /// </summary>
        public string sSpillIcon { get; set; }
    }

    [Serializable]
    public class TDataCompliance_Product
    {
        public int nProductID { get; set; }
        public string sProductName { get; set; }
        public int? nOrder { get; set; }
        public string sUnit { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public string sType { get; set; }

        public string sM1 { get; set; }
        public string sM2 { get; set; }
        public string sM3 { get; set; }
        public string sM4 { get; set; }
        public string sM5 { get; set; }
        public string sM6 { get; set; }
        public string sM7 { get; set; }
        public string sM8 { get; set; }
        public string sM9 { get; set; }
        public string sM10 { get; set; }
        public string sM11 { get; set; }
        public string sM12 { get; set; }
        public string sTotal { get; set; }

        public string sShowM1 { get; set; }
        public string sShowM2 { get; set; }
        public string sShowM3 { get; set; }
        public string sShowM4 { get; set; }
        public string sShowM5 { get; set; }
        public string sShowM6 { get; set; }
        public string sShowM7 { get; set; }
        public string sShowM8 { get; set; }
        public string sShowM9 { get; set; }
        public string sShowM10 { get; set; }
        public string sShowM11 { get; set; }
        public string sShowM12 { get; set; }
        public string sShowTotal { get; set; }
    }

    [Serializable]
    public class TDataCompliance
    {
        public int nFormID { get; set; }
        public int nComplianceID { get; set; }
        public int nFacilityID { get; set; }
        public DateTime? dComplianceDate { get; set; }
        public string sComplianceDate { get; set; }
        public string sDocNumber { get; set; }
        public string sIssueBy { get; set; }
        public string sSubject { get; set; }
        public string sDetail { get; set; }
    }

    [Serializable]
    public class TDataComplaint_Product
    {
        public int nProductID { get; set; }
        public string sProductName { get; set; }
        public int? nOrder { get; set; }
        public string sUnit { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public string sType { get; set; }

        public string sM1 { get; set; }
        public string sM2 { get; set; }
        public string sM3 { get; set; }
        public string sM4 { get; set; }
        public string sM5 { get; set; }
        public string sM6 { get; set; }
        public string sM7 { get; set; }
        public string sM8 { get; set; }
        public string sM9 { get; set; }
        public string sM10 { get; set; }
        public string sM11 { get; set; }
        public string sM12 { get; set; }
        public string sTotal { get; set; }

        public string sShowM1 { get; set; }
        public string sShowM2 { get; set; }
        public string sShowM3 { get; set; }
        public string sShowM4 { get; set; }
        public string sShowM5 { get; set; }
        public string sShowM6 { get; set; }
        public string sShowM7 { get; set; }
        public string sShowM8 { get; set; }
        public string sShowM9 { get; set; }
        public string sShowM10 { get; set; }
        public string sShowM11 { get; set; }
        public string sShowM12 { get; set; }
        public string sShowTotal { get; set; }
    }

    [Serializable]
    public class TDataComplaint
    {
        public int nFormID { get; set; }
        public int nComplaintID { get; set; }
        public int nFacilityID { get; set; }
        public DateTime? dComplaintDate { get; set; }
        public string sComplaintDate { get; set; }
        public string sIssueBy { get; set; }
        public string sSubject { get; set; }
        public string sDetail { get; set; }
        public string sCorrectiveAction { get; set; }
    }

    [Serializable]
    public class CSubmitData
    {
        public string sYear { get; set; }
        public string sFacID { get; set; }
        public string sIndID { get; set; }
        public string[] arrQuater { get; set; }
        public string sComment { get; set; }
    }

    [Serializable]
    public class CResultSubmitData : sysGlobalClass.CResutlWebMethod
    {
        public List<TResultWSV> lstResultWSV { get; set; }
    }

    [Serializable]
    public class TResultWSV
    {
        public int nFacilityID { get; set; }
        public string sFacilityCode { get; set; }
        public int nQuarter { get; set; }
        public bool IsPass { get; set; }
        public string sMsg { get; set; }
    }

    [Serializable]
    public class TDataUpdateLog
    {
        public int nReportID { get; set; }
        public int nMonth { get; set; }
        public int? nFromStatusID { get; set; }
        public int? nToStatusID { get; set; }
    }
    #endregion
}