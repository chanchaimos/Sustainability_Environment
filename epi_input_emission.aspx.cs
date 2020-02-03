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

public partial class epi_input_emission : System.Web.UI.Page
{
    private const string sFolderInSharePahtTemp = "UploadFiles/Emission/Temp/";
    private const string sFolderInPathSave = "UploadFiles/Emission/File/{0}/";
    private const int nIndicator = 4;
    private const int nMSFlowrateID = 172;
    private const int nMSOperatinghour = 173;
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
                //setCBL();
                string sQueryStrIndID = Request.QueryString["in"];
                string sQuertStrOprtID = Request.QueryString["ot"];
                if (!string.IsNullOrEmpty(sQueryStrIndID))
                {
                    hdfIndID.Value = STCrypt.Encrypt(sQueryStrIndID);
                    hdfOprtID.Value = STCrypt.Encrypt(sQuertStrOprtID);
                    ((_MP_EPI_FORMS)this.Master).hdfPRMS = SystemFunction.GetPermissionMenu(11) + "";
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
            result.objDataEmission = new ObjData();
            result.lstAddStack = new List<TData_Emission>();
            result.lstAddStackCEM = new List<TData_Emission>();
            result.objDataEmission.lstCombustion = new List<TData_Emission>();
            result.objDataEmission.lstNonCombustion = new List<TData_Emission>();
            result.objDataEmission.lstCEM = new List<TData_Emission>();
            result.objDataEmission.lstAdditional = new List<TData_Emission>();
            result.objDataEmission.lstAdditionalNonCombustion = new List<TData_Emission>();
            result.objDataEmission.lstStack = new List<TData_Stack>();
            result.lstAddAdditional = new List<TData_Emission>();
            result.lstOtherPrd = new List<TM_Emission_OtherProduct>();
            result.lstVOC = new List<TData_Emission>();
            result.lstFile = new List<sysGlobalClass.FuncFileUpload.ItemData>();
            result.lstStatus = new List<sysGlobalClass.T_TEPI_Workflow>();
            int nIndID = SystemFunction.GetIntNullToZero(param.sIndID);
            int nOprtID = SystemFunction.GetIntNullToZero(param.sOprtID);
            int nFacID = SystemFunction.GetIntNullToZero(param.sFacID);
            result.hdfPRMS = SystemFunction.GetPermission_EPI_FROMS(nIndID, nFacID) + "";
            string sYear = param.sYear;
            List<int> lstIDFlowlate = new List<int>() { 172, 173, 174 };
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
            #region mTProductIndicator
            var lstTProduct = db.mTProductIndicator.Where(w => w.IDIndicator == nIndicator).ToList();
            var lstUseProduct = db.TEmissionUseProduct.Where(w => w.OperationTypeID == nOprtID).Select(s => new
            {
                ProductID = s.ProductID,
                nOrder = s.nOrder,
                cSetCode = s.cSetCode,
            }).ToList();
            //var lstmTProductIndicator = db.mTProductIndicator.Where(w => w.IDIndicator == nIndicator && lstUseProduct.Contains(w.ProductID)).ToList();
            var lstmTProductIndicator = (from p in lstTProduct
                                         from u in lstUseProduct.Where(w => w.ProductID == p.ProductID)
                                         select new
                                         {
                                             ProductID = p.ProductID,
                                             ProductName = p.ProductName,
                                             sType = p.sType,
                                             sUnit = p.sUnit,
                                             nGroupCalc = p.nGroupCalc,
                                             nOption = p.nOption,
                                             cTotal = p.cTotal,
                                             nOrder = u.nOrder,
                                             cSetCode = u.cSetCode,
                                         }).ToList();
            lstmTProductIndicator.Where(w => w.sType == "SUM" || w.sType == "SUM2").ToList().ForEach(f =>
            {
                if (f.sType == "SUM" || f.sType == "SUM2")
                {
                    result.objDataEmission.lstCombustion.Add(new TData_Emission
                    {
                        ProductID = f.ProductID,
                        ProductName = f.ProductName,
                        UnitID = f.sType == "SUM" ? (int?)2 : null,
                        sUnit = f.sUnit,
                        sType = f.sType,
                        cTotal = f.cTotal,
                        cSetCode = f.cSetCode,
                        nGroupCalc = f.nGroupCalc,
                        M1 = "",
                        M2 = "",
                        M3 = "",
                        M4 = "",
                        M5 = "",
                        M6 = "",
                        M7 = "",
                        M8 = "",
                        M9 = "",
                        M10 = "",
                        M11 = "",
                        M12 = "",
                        Target = "",

                        IsCheckM1 = "N",
                        IsCheckM2 = "N",
                        IsCheckM3 = "N",
                        IsCheckM4 = "N",
                        IsCheckM5 = "N",
                        IsCheckM6 = "N",
                        IsCheckM7 = "N",
                        IsCheckM8 = "N",
                        IsCheckM9 = "N",
                        IsCheckM10 = "N",
                        IsCheckM11 = "N",
                        IsCheckM12 = "N",
                    });
                    result.objDataEmission.lstNonCombustion.Add(new TData_Emission
                    {
                        ProductID = f.ProductID,
                        ProductName = f.ProductName,
                        UnitID = f.sType == "SUM" ? (int?)2 : null,
                        sUnit = f.sUnit,
                        sType = f.sType,
                        cTotal = f.cTotal,
                        cSetCode = f.cSetCode,
                        nGroupCalc = f.nGroupCalc,
                        M1 = "",
                        M2 = "",
                        M3 = "",
                        M4 = "",
                        M5 = "",
                        M6 = "",
                        M7 = "",
                        M8 = "",
                        M9 = "",
                        M10 = "",
                        M11 = "",
                        M12 = "",
                        Target = "",

                        IsCheckM1 = "N",
                        IsCheckM2 = "N",
                        IsCheckM3 = "N",
                        IsCheckM4 = "N",
                        IsCheckM5 = "N",
                        IsCheckM6 = "N",
                        IsCheckM7 = "N",
                        IsCheckM8 = "N",
                        IsCheckM9 = "N",
                        IsCheckM10 = "N",
                        IsCheckM11 = "N",
                        IsCheckM12 = "N",
                    });
                    result.objDataEmission.lstCEM.Add(new TData_Emission
                    {
                        ProductID = f.ProductID,
                        ProductName = f.ProductName,
                        UnitID = f.sType == "SUM" ? (int?)2 : null,
                        sUnit = f.sUnit,
                        sType = f.sType,
                        cTotal = f.cTotal,
                        cSetCode = f.cSetCode,
                        nGroupCalc = f.nGroupCalc,
                        M1 = "",
                        M2 = "",
                        M3 = "",
                        M4 = "",
                        M5 = "",
                        M6 = "",
                        M7 = "",
                        M8 = "",
                        M9 = "",
                        M10 = "",
                        M11 = "",
                        M12 = "",
                        Target = "",

                        IsCheckM1 = "N",
                        IsCheckM2 = "N",
                        IsCheckM3 = "N",
                        IsCheckM4 = "N",
                        IsCheckM5 = "N",
                        IsCheckM6 = "N",
                        IsCheckM7 = "N",
                        IsCheckM8 = "N",
                        IsCheckM9 = "N",
                        IsCheckM10 = "N",
                        IsCheckM11 = "N",
                        IsCheckM12 = "N",
                    });
                }
            });
            lstmTProductIndicator.Where(w => w.sType == "2" || w.sType == "2H").OrderBy(o => o.nOrder).ThenBy(o2 => o2.nGroupCalc).ToList().ForEach(f =>
            {
                result.lstAddStack.Add(new TData_Emission
                {
                    ProductID = f.ProductID,
                    ProductName = f.ProductName,
                    sUnit = f.sUnit,
                    sType = f.sType,
                    nOption = f.nOption ?? null,
                    cTotal = f.cTotal,
                    nGroupCalc = f.nGroupCalc,
                    cSetCode = f.cSetCode,
                    M1 = "",
                    M2 = "",
                    M3 = "",
                    M4 = "",
                    M5 = "",
                    M6 = "",
                    M7 = "",
                    M8 = "",
                    M9 = "",
                    M10 = "",
                    M11 = "",
                    M12 = "",
                    Target = "",

                    IsCheckM1 = "N",
                    IsCheckM2 = "N",
                    IsCheckM3 = "N",
                    IsCheckM4 = "N",
                    IsCheckM5 = "N",
                    IsCheckM6 = "N",
                    IsCheckM7 = "N",
                    IsCheckM8 = "N",
                    IsCheckM9 = "N",
                    IsCheckM10 = "N",
                    IsCheckM11 = "N",
                    IsCheckM12 = "N",
                });

            });
            lstmTProductIndicator.Where(w => w.sType == "CEM" || w.sType == "2H").OrderBy(o => o.nOrder).ThenBy(o2 => o2.nGroupCalc).ToList().ForEach(f =>
            {
                result.lstAddStackCEM.Add(new TData_Emission
                {
                    ProductID = f.ProductID,
                    ProductName = f.ProductName,
                    sUnit = f.sUnit,
                    sType = f.sType,
                    nOption = f.nOption ?? null,
                    cTotal = f.cTotal,
                    nGroupCalc = f.nGroupCalc,
                    cSetCode = f.cSetCode,
                    M1 = "",
                    M2 = "",
                    M3 = "",
                    M4 = "",
                    M5 = "",
                    M6 = "",
                    M7 = "",
                    M8 = "",
                    M9 = "",
                    M10 = "",
                    M11 = "",
                    M12 = "",
                    Target = "",

                    IsCheckM1 = "N",
                    IsCheckM2 = "N",
                    IsCheckM3 = "N",
                    IsCheckM4 = "N",
                    IsCheckM5 = "N",
                    IsCheckM6 = "N",
                    IsCheckM7 = "N",
                    IsCheckM8 = "N",
                    IsCheckM9 = "N",
                    IsCheckM10 = "N",
                    IsCheckM11 = "N",
                    IsCheckM12 = "N",
                });

            });
            db.mTProductIndicator.Where(w => w.IDIndicator == nIndicator && w.sType == "OTH").OrderBy(o => o.nOrder).ThenBy(o2 => o2.nGroupCalc).ToList().ForEach(f =>
            {
                result.lstAddAdditional.Add(new TData_Emission
                {
                    ProductID = f.ProductID,
                    ProductName = f.ProductName,
                    sUnit = f.sUnit,
                    sType = f.sType,
                    nOption = f.nOption ?? null,
                    cTotal = f.cTotal,
                    nGroupCalc = f.nGroupCalc,
                    M1 = "",
                    M2 = "",
                    M3 = "",
                    M4 = "",
                    M5 = "",
                    M6 = "",
                    M7 = "",
                    M8 = "",
                    M9 = "",
                    M10 = "",
                    M11 = "",
                    M12 = "",
                    Target = "",

                    IsCheckM1 = "N",
                    IsCheckM2 = "N",
                    IsCheckM3 = "N",
                    IsCheckM4 = "N",
                    IsCheckM5 = "N",
                    IsCheckM6 = "N",
                    IsCheckM7 = "N",
                    IsCheckM8 = "N",
                    IsCheckM9 = "N",
                    IsCheckM10 = "N",
                    IsCheckM11 = "N",
                    IsCheckM12 = "N",
                });
            });
            lstmTProductIndicator.Where(w => w.sType == "VOC").OrderBy(o => o.nOrder).ToList().ForEach(f =>
            {
                result.lstVOC.Add(new TData_Emission
                {
                    ProductID = f.ProductID,
                    ProductName = f.ProductName,
                    sUnit = f.sUnit,
                    sType = f.sType,
                    nOption = f.nOption ?? null,
                    cTotal = f.cTotal,
                    nGroupCalc = f.nGroupCalc,
                    sOption = "M",
                    M1 = "",
                    M2 = "",
                    M3 = "",
                    M4 = "",
                    M5 = "",
                    M6 = "",
                    M7 = "",
                    M8 = "",
                    M9 = "",
                    M10 = "",
                    M11 = "",
                    M12 = "",
                    Target = "",
                    nTotal = "",

                    IsCheckM1 = "N",
                    IsCheckM2 = "N",
                    IsCheckM3 = "N",
                    IsCheckM4 = "N",
                    IsCheckM5 = "N",
                    IsCheckM6 = "N",
                    IsCheckM7 = "N",
                    IsCheckM8 = "N",
                    IsCheckM9 = "N",
                    IsCheckM10 = "N",
                    IsCheckM11 = "N",
                    IsCheckM12 = "N",
                });
            });

            db.TM_Emission_OtherProduct.Where(w => w.cActive == "Y" && w.cDel == "N").ToList().ForEach(f =>
            {
                result.lstOtherPrd.Add(new TM_Emission_OtherProduct
                {
                    nProductID = f.nProductID,
                    sName = f.sName
                });
            });
            #endregion

            #region EPI_FORM
            var itemEPI_FORM = db.TEPI_Forms.FirstOrDefault(w => w.sYear == sYear && w.IDIndicator == nIndID && w.OperationTypeID == nOprtID && w.FacilityID == nFacID);
            if (itemEPI_FORM != null)
            {
                #region Combustion
                var dataCombustion = db.TEmission_Product.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
                result.objDataEmission.lstCombustion.ForEach(f =>
                {
                    var item = dataCombustion.FirstOrDefault(w => w.ProductID == f.ProductID);
                    if (item != null)
                    {
                        f.M1 = item.M1;
                        f.M2 = item.M2;
                        f.M3 = item.M3;
                        f.M4 = item.M4;
                        f.M5 = item.M5;
                        f.M6 = item.M6;
                        f.M7 = item.M7;
                        f.M8 = item.M8;
                        f.M9 = item.M9;
                        f.M10 = item.M10;
                        f.M11 = item.M11;
                        f.M12 = item.M12;
                        f.Target = item.Target;

                        f.IsCheckM1 = item.IsCheckM1;
                        f.IsCheckM2 = item.IsCheckM2;
                        f.IsCheckM3 = item.IsCheckM3;
                        f.IsCheckM4 = item.IsCheckM4;
                        f.IsCheckM5 = item.IsCheckM5;
                        f.IsCheckM6 = item.IsCheckM6;
                        f.IsCheckM7 = item.IsCheckM7;
                        f.IsCheckM8 = item.IsCheckM8;
                        f.IsCheckM9 = item.IsCheckM9;
                        f.IsCheckM10 = item.IsCheckM10;
                        f.IsCheckM11 = item.IsCheckM11;
                        f.IsCheckM12 = item.IsCheckM12;
                    }
                });
                #endregion

                #region Non-Combustion
                var dataNonCombustion = db.TEmission_Product_ByType.Where(w => w.sType == "NCS" && w.FormID == itemEPI_FORM.FormID).ToList();
                result.objDataEmission.lstNonCombustion.ForEach(f =>
                {
                    var item = dataNonCombustion.FirstOrDefault(w => w.ProductID == f.ProductID);
                    if (item != null)
                    {
                        f.M1 = item.M1;
                        f.M2 = item.M2;
                        f.M3 = item.M3;
                        f.M4 = item.M4;
                        f.M5 = item.M5;
                        f.M6 = item.M6;
                        f.M7 = item.M7;
                        f.M8 = item.M8;
                        f.M9 = item.M9;
                        f.M10 = item.M10;
                        f.M11 = item.M11;
                        f.M12 = item.M12;
                        f.Target = item.Target;
                    }
                });
                #endregion

                #region CEM
                var dataCEM = db.TEmission_Product_ByType.Where(w => w.sType == "CEM" && w.FormID == itemEPI_FORM.FormID).ToList();
                result.objDataEmission.lstCEM.ForEach(f =>
                {
                    var item = dataCEM.FirstOrDefault(w => w.ProductID == f.ProductID);
                    if (item != null)
                    {
                        f.M1 = item.M1;
                        f.M2 = item.M2;
                        f.M3 = item.M3;
                        f.M4 = item.M4;
                        f.M5 = item.M5;
                        f.M6 = item.M6;
                        f.M7 = item.M7;
                        f.M8 = item.M8;
                        f.M9 = item.M9;
                        f.M10 = item.M10;
                        f.M11 = item.M11;
                        f.M12 = item.M12;
                        f.Target = item.Target;
                    }
                });
                #endregion

                #region Additional Combustion
                var dataAdddtionalCombustion = db.TEmission_Product_ByType.Where(w => w.sType == "ACS" && w.FormID == itemEPI_FORM.FormID).ToList();
                foreach (var item in dataAdddtionalCombustion)
                {
                    var dataName = result.lstOtherPrd.FirstOrDefault(f => f.nProductID == item.ProductID);
                    result.objDataEmission.lstAdditional.Add(new TData_Emission
                    {
                        ProductID = item.ProductID,
                        ProductName = dataName != null ? dataName.sName : "",
                        sUnit = GetValueNameUnit(item.UnitID + ""),
                        UnitID = item.UnitID,
                        nOtherProductID = item.ProductID,
                        M1 = item.M1,
                        M2 = item.M2,
                        M3 = item.M3,
                        M4 = item.M4,
                        M5 = item.M5,
                        M6 = item.M6,
                        M7 = item.M7,
                        M8 = item.M8,
                        M9 = item.M9,
                        M10 = item.M10,
                        M11 = item.M11,
                        M12 = item.M12,
                        Target = item.Target,
                        sType = "OTH"
                    });
                }
                #endregion

                #region Additional Non-Combustion
                var dataAdddtionalNonCombustion = db.TEmission_Product_ByType.Where(w => w.sType == "ANC" && w.FormID == itemEPI_FORM.FormID).ToList();
                foreach (var item in dataAdddtionalNonCombustion)
                {
                    var dataName = result.lstOtherPrd.FirstOrDefault(f => f.nProductID == item.ProductID);
                    result.objDataEmission.lstAdditionalNonCombustion.Add(new TData_Emission
                    {
                        ProductID = item.ProductID,
                        ProductName = dataName != null ? dataName.sName : "",
                        sUnit = GetValueNameUnit(item.UnitID + ""),
                        nOtherProductID = item.ProductID,
                        UnitID = item.UnitID,
                        M1 = item.M1,
                        M2 = item.M2,
                        M3 = item.M3,
                        M4 = item.M4,
                        M5 = item.M5,
                        M6 = item.M6,
                        M7 = item.M7,
                        M8 = item.M8,
                        M9 = item.M9,
                        M10 = item.M10,
                        M11 = item.M11,
                        M12 = item.M12,
                        Target = item.Target,
                        sType = "OTH",
                    });
                }
                result.objDataEmission.lstAdditionalNonCombustion = result.objDataEmission.lstAdditionalNonCombustion.OrderBy(o => o.ProductName).ToList();
                #endregion

                #region Stack
                var dataStack = db.TEmission_Stack.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
                var dataLstStack = db.TEmission_Product_Stack.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
                var dataLstStackOther = db.TEmission_OtherProduct_Stack.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
                dataStack.ForEach(f =>
                {
                    result.objDataEmission.lstStack.Add(new TData_Stack
                    {
                        nStackID = f.nStackID,
                        sStackName = f.sStackName,
                        sStackType = f.sStackType,
                        sRemark = f.sRemark,
                        IsSaved = true,
                        IsSubmited = true,
                        lstDataStack = lstmTProductIndicator.Where(w => w.sType == "2" || w.sType == "2H").OrderBy(o => o.nOrder).ThenBy(o2 => o2.nGroupCalc).Select(s => new TData_Emission
                        {
                            ProductID = s.ProductID,
                            ProductName = s.ProductName,
                            sUnit = s.sUnit,
                            sType = s.sType,
                            nOption = s.nOption ?? null,
                            cTotal = s.cTotal,
                            nGroupCalc = s.nGroupCalc,
                            cSetCode = s.cSetCode,
                            M1 = "",
                            M2 = "",
                            M3 = "",
                            M4 = "",
                            M5 = "",
                            M6 = "",
                            M7 = "",
                            M8 = "",
                            M9 = "",
                            M10 = "",
                            M11 = "",
                            M12 = "",
                            Target = "",

                            IsCheckM1 = "N",
                            IsCheckM2 = "N",
                            IsCheckM3 = "N",
                            IsCheckM4 = "N",
                            IsCheckM5 = "N",
                            IsCheckM6 = "N",
                            IsCheckM7 = "N",
                            IsCheckM8 = "N",
                            IsCheckM9 = "N",
                            IsCheckM10 = "N",
                            IsCheckM11 = "N",
                            IsCheckM12 = "N",
                        }).ToList(),
                        lstDataStackCEM = lstmTProductIndicator.Where(w => w.sType == "CEM" || w.sType == "2H").OrderBy(o => o.nOrder).ThenBy(o2 => o2.nGroupCalc).Select(s => new TData_Emission
                        {
                            ProductID = s.ProductID,
                            ProductName = s.ProductName,
                            sUnit = s.sUnit,
                            sType = s.sType,
                            nOption = s.nOption ?? null,
                            cTotal = s.cTotal,
                            nGroupCalc = s.nGroupCalc,
                            cSetCode = s.cSetCode,
                            M1 = "",
                            M2 = "",
                            M3 = "",
                            M4 = "",
                            M5 = "",
                            M6 = "",
                            M7 = "",
                            M8 = "",
                            M9 = "",
                            M10 = "",
                            M11 = "",
                            M12 = "",
                            Target = "",

                            IsCheckM1 = "N",
                            IsCheckM2 = "N",
                            IsCheckM3 = "N",
                            IsCheckM4 = "N",
                            IsCheckM5 = "N",
                            IsCheckM6 = "N",
                            IsCheckM7 = "N",
                            IsCheckM8 = "N",
                            IsCheckM9 = "N",
                            IsCheckM10 = "N",
                            IsCheckM11 = "N",
                            IsCheckM12 = "N",
                        }).ToList(),

                    });
                });
                result.objDataEmission.lstStack.ForEach(f =>
                {
                    foreach (var item in f.lstDataStack)
                    {
                        item.nStackID = f.nStackID;
                        var data = dataLstStack.FirstOrDefault(w => w.nStackID == f.nStackID && w.ProductID == item.ProductID && w.nOptionProduct != 3);
                        if (data != null)
                        {
                            item.M1 = data.M1;
                            item.M2 = data.M2;
                            item.M3 = data.M3;
                            item.M4 = data.M4;
                            item.M5 = data.M5;
                            item.M6 = data.M6;
                            item.M7 = data.M7;
                            item.M8 = data.M8;
                            item.M9 = data.M9;
                            item.M10 = data.M10;
                            item.M11 = data.M11;
                            item.M12 = data.M12;
                            item.Target = data.Target;
                            item.nOptionProduct = data.nOptionProduct;
                            item.IsSaved = true;
                            item.IsSubmited = true;
                        }
                    }
                    foreach (var item in f.lstDataStackCEM)
                    {
                        item.nStackID = f.nStackID;
                        var data = dataLstStack.FirstOrDefault(w => w.nStackID == f.nStackID && w.ProductID == item.ProductID && w.nOptionProduct == 3);
                        if (data != null)
                        {
                            item.M1 = data.M1;
                            item.M2 = data.M2;
                            item.M3 = data.M3;
                            item.M4 = data.M4;
                            item.M5 = data.M5;
                            item.M6 = data.M6;
                            item.M7 = data.M7;
                            item.M8 = data.M8;
                            item.M9 = data.M9;
                            item.M10 = data.M10;
                            item.M11 = data.M11;
                            item.M12 = data.M12;
                            item.Target = data.Target;
                            item.nOptionProduct = data.nOptionProduct;
                            item.IsSaved = true;
                            item.IsSubmited = true;
                        }
                    }
                    var dataLstOther = result.lstAddAdditional;
                    var dataOther = (from v in dataLstStackOther.Where(w => w.nStackID == f.nStackID).ToList()
                                     from d in dataLstOther.Where(w => w.ProductID == v.ProductID).ToList()
                                     select new TData_Emission
                                     {
                                         ProductID = d.ProductID,
                                         ProductName = d.ProductName,
                                         sUnit = d.sUnit,
                                         sType = d.sType,
                                         nOption = d.nOption ?? null,
                                         cTotal = d.cTotal,
                                         nGroupCalc = d.nGroupCalc,
                                         nStackID = f.nStackID,
                                         nID = v.nID,
                                         M1 = v.M1,
                                         M2 = v.M2,
                                         M3 = v.M3,
                                         M4 = v.M4,
                                         M5 = v.M5,
                                         M6 = v.M6,
                                         M7 = v.M7,
                                         M8 = v.M8,
                                         M9 = v.M9,
                                         M10 = v.M10,
                                         M11 = v.M11,
                                         M12 = v.M12,
                                         Target = v.Target,
                                         nOptionProduct = v.nOptionProduct,
                                         nOtherProductID = v.nOtherProductID,
                                         cIsVOCs = v.cIsVOCs,
                                         IsSaved = true,
                                         IsSubmited = true,
                                     }).ToList();
                    f.lstDataStack.AddRange(dataOther);

                });
                #endregion

                #region VOC
                var dataVoc = db.TEmission_VOC.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
                result.lstVOC.ForEach(f =>
                {
                    var item = dataVoc.FirstOrDefault(w => w.ProductID == f.ProductID);
                    if (item != null)
                    {
                        f.M1 = item.sOption == "Y" ? "" : item.M1;
                        f.M2 = item.sOption == "Y" ? "" : item.M2;
                        f.M3 = item.sOption == "Y" ? "" : item.M3;
                        f.M4 = item.sOption == "Y" ? "" : item.M4;
                        f.M5 = item.sOption == "Y" ? "" : item.M5;
                        f.M6 = item.sOption == "Y" ? "" : item.M6;
                        f.M7 = item.sOption == "Y" ? "" : item.M7;
                        f.M8 = item.sOption == "Y" ? "" : item.M8;
                        f.M9 = item.sOption == "Y" ? "" : item.M9;
                        f.M10 = item.sOption == "Y" ? "" : item.M10;
                        f.M11 = item.sOption == "Y" ? "" : item.M11;
                        f.M12 = item.sOption == "Y" ? "" : item.M12;
                        f.Target = item.Target;
                        f.sOption = item.sOption;
                        f.nTotal = item.nTotal;

                        f.IsCheckM1 = item.sOption == "Y" ? "N" : item.IsCheckM1;
                        f.IsCheckM2 = item.sOption == "Y" ? "N" : item.IsCheckM2;
                        f.IsCheckM3 = item.sOption == "Y" ? "N" : item.IsCheckM3;
                        f.IsCheckM4 = item.sOption == "Y" ? "N" : item.IsCheckM4;
                        f.IsCheckM5 = item.sOption == "Y" ? "N" : item.IsCheckM5;
                        f.IsCheckM6 = item.sOption == "Y" ? "N" : item.IsCheckM6;
                        f.IsCheckM7 = item.sOption == "Y" ? "N" : item.IsCheckM7;
                        f.IsCheckM8 = item.sOption == "Y" ? "N" : item.IsCheckM8;
                        f.IsCheckM9 = item.sOption == "Y" ? "N" : item.IsCheckM9;
                        f.IsCheckM10 = item.sOption == "Y" ? "N" : item.IsCheckM10;
                        f.IsCheckM11 = item.sOption == "Y" ? "N" : item.IsCheckM11;
                        f.IsCheckM12 = item.sOption == "Y" ? "N" : item.IsCheckM12;
                    }
                });

                var dataRemark = db.TEmission_Remark.Where(w => w.FormID == itemEPI_FORM.FormID).OrderByDescending(o => o.nVersion).FirstOrDefault();
                if (dataRemark != null)
                {
                    result.sRemarkVOC = dataRemark.sRemark;
                }
                #endregion

                #region FILE
                result.lstFile = db.TEPI_Forms_File.Where(w => w.FormID == itemEPI_FORM.FormID).Select(s => new sysGlobalClass.FuncFileUpload.ItemData
                {
                    ID = s.nFileID,
                    FileName = s.sFileName,
                    SaveToFileName = s.sSysFileName,
                    url = s.sPath + s.sSysFileName,
                    SaveToPath = s.sPath,
                    IsNewFile = false,
                    sDelete = "N",
                    sDescription = s.sDescription,
                }).ToList();
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
    public static CReturnData CalculateToTable(CParam param, List<TData_Stack> lstStack, List<TData_Emission> lstDataCombustion
                                                                                       , List<TData_Emission> lstDataNonCombustion
                                                                                       , List<TData_Emission> lstDataCEM
                                                                                       , List<TData_Emission> lstDataAdditional
                                                                                       , List<TData_Emission> lstDataAdditionalNonCombustion)
    {
        CReturnData result = new CReturnData();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();

        #region mTProductIndicator
        result.objDataEmission = new ObjData();
        result.objDataEmission.lstCombustion = new List<TData_Emission>();
        result.objDataEmission.lstNonCombustion = new List<TData_Emission>();
        result.objDataEmission.lstCEM = new List<TData_Emission>();
        result.objDataEmission.lstAdditional = new List<TData_Emission>();
        result.objDataEmission.lstAdditionalNonCombustion = new List<TData_Emission>();

        result.objDataEmission.lstCombustion = lstDataCombustion;
        result.objDataEmission.lstNonCombustion = lstDataNonCombustion;
        result.objDataEmission.lstCEM = lstDataCEM;
        //result.objDataEmission.lstAdditional = lstDataAdditional;
        //result.objDataEmission.lstAdditionalNonCombustion = lstDataAdditionalNonCombustion;
        int nOprtID = SystemFunction.GetIntNullToZero(param.sOprtID);
        #endregion

        string sFactorKG2Ton = "1000";
        //List<int> lstCEMProductID = new List<int>() { 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252 };
        List<int> lstCEMProductID = new List<int>() { 241, 242, 243, 244, 245, 246 };
        List<int> lstCutProductID = new List<int>() { 247, 248, 249, 250, 251, 252 };
        #region Combustion
        var lstCombustion = lstStack.Where(w => w.sStackType == "CMS").ToList();
        List<TData_Emission> lstAllCMS = new List<TData_Emission>();
        foreach (var item in lstCombustion)
        {
            lstAllCMS.AddRange(item.lstDataStack.Where(w => w.cTotal != "Y").ToList());
        }
        var Product2H = lstAllCMS.Where(w => w.sType == "2H").ToList();
        List<TData_Emission> lstCombustion2TON = new List<TData_Emission>();
        foreach (var item in Product2H)
        {
            if (item.nOptionProduct == 1)
            {
                #region Option 1
                lstCombustion2TON.Add(new TData_Emission
                {
                    nStackID = item.nStackID,
                    ProductID = item.ProductID,
                    sType = item.sType,
                    nGroupCalc = item.nGroupCalc,
                    cSetCode = item.cSetCode,
                    M1 = SystemFunction.sysDivideData(GetValueFromList(lstAllCMS, item.nStackID, 1, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M2 = SystemFunction.sysDivideData(GetValueFromList(lstAllCMS, item.nStackID, 2, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M3 = SystemFunction.sysDivideData(GetValueFromList(lstAllCMS, item.nStackID, 3, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M4 = SystemFunction.sysDivideData(GetValueFromList(lstAllCMS, item.nStackID, 4, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M5 = SystemFunction.sysDivideData(GetValueFromList(lstAllCMS, item.nStackID, 5, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M6 = SystemFunction.sysDivideData(GetValueFromList(lstAllCMS, item.nStackID, 6, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M7 = SystemFunction.sysDivideData(GetValueFromList(lstAllCMS, item.nStackID, 7, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M8 = SystemFunction.sysDivideData(GetValueFromList(lstAllCMS, item.nStackID, 8, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M9 = SystemFunction.sysDivideData(GetValueFromList(lstAllCMS, item.nStackID, 9, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M10 = SystemFunction.sysDivideData(GetValueFromList(lstAllCMS, item.nStackID, 10, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M11 = SystemFunction.sysDivideData(GetValueFromList(lstAllCMS, item.nStackID, 11, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M12 = SystemFunction.sysDivideData(GetValueFromList(lstAllCMS, item.nStackID, 12, item.cSetCode, 1), sFactorKG2Ton) + ""
                });
                #endregion
            }
            else if (item.nOptionProduct == 2)
            {
                #region Option 2
                lstCombustion2TON.Add(new TData_Emission
                {
                    nStackID = item.nStackID,
                    ProductID = item.ProductID,
                    sType = item.sType,
                    nGroupCalc = item.nGroupCalc,
                    cSetCode = item.cSetCode,
                    M1 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllCMS, item.nStackID, 1, item.cSetCode, 2), GetValueFromList(lstAllCMS, item.nStackID, 1, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 1, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M2 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllCMS, item.nStackID, 2, item.cSetCode, 2), GetValueFromList(lstAllCMS, item.nStackID, 2, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 2, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M3 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllCMS, item.nStackID, 3, item.cSetCode, 2), GetValueFromList(lstAllCMS, item.nStackID, 3, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 3, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M4 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllCMS, item.nStackID, 4, item.cSetCode, 2), GetValueFromList(lstAllCMS, item.nStackID, 4, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 4, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M5 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllCMS, item.nStackID, 5, item.cSetCode, 2), GetValueFromList(lstAllCMS, item.nStackID, 5, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 5, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M6 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllCMS, item.nStackID, 6, item.cSetCode, 2), GetValueFromList(lstAllCMS, item.nStackID, 6, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 6, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M7 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllCMS, item.nStackID, 7, item.cSetCode, 2), GetValueFromList(lstAllCMS, item.nStackID, 7, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 7, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M8 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllCMS, item.nStackID, 8, item.cSetCode, 2), GetValueFromList(lstAllCMS, item.nStackID, 8, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 8, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M9 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllCMS, item.nStackID, 9, item.cSetCode, 2), GetValueFromList(lstAllCMS, item.nStackID, 9, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 9, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M10 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllCMS, item.nStackID, 10, item.cSetCode, 2), GetValueFromList(lstAllCMS, item.nStackID, 10, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 10, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M11 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllCMS, item.nStackID, 11, item.cSetCode, 2), GetValueFromList(lstAllCMS, item.nStackID, 11, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 11, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M12 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllCMS, item.nStackID, 12, item.cSetCode, 2), GetValueFromList(lstAllCMS, item.nStackID, 12, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 12, nMSOperatinghour)) + "", sFactorKG2Ton) + ""
                });
                #endregion
            }
        }

        #region //1. Calculate Total Combustion (Ton)
        foreach (var item in result.objDataEmission.lstCombustion.Where(w => w.sType == "SUM").ToList())
        {
            int nProductIDCal = 0;
            switch (item.ProductID)
            {
                case 160: //Total NOx Emission 
                    nProductIDCal = 175;
                    break;
                case 162://Total SO2> Emission
                    nProductIDCal = 178;
                    break;
                case 164://Total TSP Emission
                    nProductIDCal = 181;
                    break;
                case 166://Total CO Emission
                    nProductIDCal = 184;
                    break;
                case 168://Total Hg Emission
                    nProductIDCal = 187;
                    break;
                case 170://Total H2S Emission
                    nProductIDCal = 190;
                    break;
            }

            var queryToSum = lstCombustion2TON.Where(w => w.ProductID == nProductIDCal).ToList();
            item.M1 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M1).ToList()) + "";
            item.M2 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M2).ToList()) + "";
            item.M3 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M3).ToList()) + "";
            item.M4 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M4).ToList()) + "";
            item.M5 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M5).ToList()) + "";
            item.M6 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M6).ToList()) + "";
            item.M7 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M7).ToList()) + "";
            item.M8 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M8).ToList()) + "";
            item.M9 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M9).ToList()) + "";
            item.M10 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M10).ToList()) + "";
            item.M11 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M11).ToList()) + "";
            item.M12 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M12).ToList()) + "";
        }
        #endregion

        #region //2. Calculate Total Combustion (g/sec)
        var qProductSumTon = result.objDataEmission.lstCombustion.Where(w => w.sType == "SUM").ToList();
        var qOperatinghour = lstAllCMS.Where(w => w.ProductID == nMSOperatinghour).ToList();
        foreach (var item in result.objDataEmission.lstCombustion.Where(w => w.sType == "SUM2").ToList())
        {
            item.M1 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 1, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M1).ToList()) + "") + "";
            item.M2 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 2, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M2).ToList()) + "") + "";
            item.M3 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 3, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M3).ToList()) + "") + "";
            item.M4 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 4, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M4).ToList()) + "") + "";
            item.M5 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 5, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M5).ToList()) + "") + "";
            item.M6 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 6, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M6).ToList()) + "") + "";
            item.M7 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 7, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M7).ToList()) + "") + "";
            item.M8 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 8, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M8).ToList()) + "") + "";
            item.M9 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 9, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M9).ToList()) + "") + "";
            item.M10 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 10, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M10).ToList()) + "") + "";
            item.M11 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 11, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M11).ToList()) + "") + "";
            item.M12 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 12, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M12).ToList()) + "") + "";
        }
        #endregion
        #endregion

        #region NON-Combustion
        var lstNonCombustion = lstStack.Where(w => w.sStackType == "NCS").ToList();
        List<TData_Emission> lstAllNCS = new List<TData_Emission>();
        foreach (var item in lstNonCombustion)
        {
            lstAllNCS.AddRange(item.lstDataStack.Where(w => w.cTotal != "Y").ToList());
        }
        var ProductNON2H = lstAllNCS.Where(w => w.sType == "2H").ToList();
        List<TData_Emission> lstNonCombustion2TON = new List<TData_Emission>();
        foreach (var item in ProductNON2H)
        {
            if (item.nOptionProduct == 1)
            {
                #region Option 1
                lstNonCombustion2TON.Add(new TData_Emission
                {
                    nStackID = item.nStackID,
                    ProductID = item.ProductID,
                    sType = item.sType,
                    nGroupCalc = item.nGroupCalc,
                    cSetCode = item.cSetCode,
                    M1 = SystemFunction.sysDivideData(GetValueFromList(lstAllNCS, item.nStackID, 1, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M2 = SystemFunction.sysDivideData(GetValueFromList(lstAllNCS, item.nStackID, 2, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M3 = SystemFunction.sysDivideData(GetValueFromList(lstAllNCS, item.nStackID, 3, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M4 = SystemFunction.sysDivideData(GetValueFromList(lstAllNCS, item.nStackID, 5, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M6 = SystemFunction.sysDivideData(GetValueFromList(lstAllCMS, item.nStackID, 6, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M7 = SystemFunction.sysDivideData(GetValueFromList(lstAllNCS, item.nStackID, 7, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M8 = SystemFunction.sysDivideData(GetValueFromList(lstAllNCS, item.nStackID, 8, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M9 = SystemFunction.sysDivideData(GetValueFromList(lstAllNCS, item.nStackID, 9, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M10 = SystemFunction.sysDivideData(GetValueFromList(lstAllNCS, item.nStackID, 10, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M11 = SystemFunction.sysDivideData(GetValueFromList(lstAllNCS, item.nStackID, 11, item.cSetCode, 1), sFactorKG2Ton) + "",
                    M12 = SystemFunction.sysDivideData(GetValueFromList(lstAllNCS, item.nStackID, 12, item.cSetCode, 1), sFactorKG2Ton) + ""
                });
                #endregion
            }
            else if (item.nOptionProduct == 2)
            {
                #region Option 2
                lstNonCombustion2TON.Add(new TData_Emission
                {
                    nStackID = item.nStackID,
                    ProductID = item.ProductID,
                    sType = item.sType,
                    nGroupCalc = item.nGroupCalc,
                    cSetCode = item.cSetCode,
                    M1 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllNCS, item.nStackID, 1, item.cSetCode, 2), GetValueFromList(lstAllNCS, item.nStackID, 1, nMSFlowrateID), GetValueFromList(lstAllNCS, item.nStackID, 1, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M2 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllNCS, item.nStackID, 2, item.cSetCode, 2), GetValueFromList(lstAllNCS, item.nStackID, 2, nMSFlowrateID), GetValueFromList(lstAllNCS, item.nStackID, 2, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M3 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllNCS, item.nStackID, 3, item.cSetCode, 2), GetValueFromList(lstAllNCS, item.nStackID, 3, nMSFlowrateID), GetValueFromList(lstAllNCS, item.nStackID, 3, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M4 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllNCS, item.nStackID, 4, item.cSetCode, 2), GetValueFromList(lstAllNCS, item.nStackID, 4, nMSFlowrateID), GetValueFromList(lstAllNCS, item.nStackID, 4, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M5 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllNCS, item.nStackID, 5, item.cSetCode, 2), GetValueFromList(lstAllNCS, item.nStackID, 5, nMSFlowrateID), GetValueFromList(lstAllNCS, item.nStackID, 5, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M6 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllNCS, item.nStackID, 6, item.cSetCode, 2), GetValueFromList(lstAllNCS, item.nStackID, 6, nMSFlowrateID), GetValueFromList(lstAllNCS, item.nStackID, 6, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M7 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllNCS, item.nStackID, 7, item.cSetCode, 2), GetValueFromList(lstAllNCS, item.nStackID, 7, nMSFlowrateID), GetValueFromList(lstAllNCS, item.nStackID, 7, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M8 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllNCS, item.nStackID, 8, item.cSetCode, 2), GetValueFromList(lstAllNCS, item.nStackID, 8, nMSFlowrateID), GetValueFromList(lstAllNCS, item.nStackID, 8, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M9 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllNCS, item.nStackID, 9, item.cSetCode, 2), GetValueFromList(lstAllNCS, item.nStackID, 9, nMSFlowrateID), GetValueFromList(lstAllNCS, item.nStackID, 9, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M10 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllNCS, item.nStackID, 10, item.cSetCode, 2), GetValueFromList(lstAllNCS, item.nStackID, 10, nMSFlowrateID), GetValueFromList(lstAllNCS, item.nStackID, 10, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M11 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllNCS, item.nStackID, 11, item.cSetCode, 2), GetValueFromList(lstAllNCS, item.nStackID, 11, nMSFlowrateID), GetValueFromList(lstAllNCS, item.nStackID, 11, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M12 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromList(lstAllNCS, item.nStackID, 12, item.cSetCode, 2), GetValueFromList(lstAllNCS, item.nStackID, 12, nMSFlowrateID), GetValueFromList(lstAllNCS, item.nStackID, 12, nMSOperatinghour)) + "", sFactorKG2Ton) + ""
                });
                #endregion
            }
        }

        #region //1. Calculate Total Non-Combustion (Ton)
        foreach (var item in result.objDataEmission.lstNonCombustion.Where(w => w.sType == "SUM").ToList())
        {
            int nProductIDCal = 0;
            switch (item.ProductID)
            {
                case 160: //Total NOx Emission 
                    nProductIDCal = 175;
                    break;
                case 162://Total SO2> Emission
                    nProductIDCal = 178;
                    break;
                case 164://Total TSP Emission
                    nProductIDCal = 181;
                    break;
                case 166://Total CO Emission
                    nProductIDCal = 184;
                    break;
                case 168://Total Hg Emission
                    nProductIDCal = 187;
                    break;
                case 170://Total H2S Emission
                    nProductIDCal = 190;
                    break;
            }

            var queryToSum = lstNonCombustion2TON.Where(w => w.ProductID == nProductIDCal).ToList();
            item.M1 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M1).ToList()) + "";
            item.M2 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M2).ToList()) + "";
            item.M3 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M3).ToList()) + "";
            item.M4 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M4).ToList()) + "";
            item.M5 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M5).ToList()) + "";
            item.M6 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M6).ToList()) + "";
            item.M7 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M7).ToList()) + "";
            item.M8 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M8).ToList()) + "";
            item.M9 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M9).ToList()) + "";
            item.M10 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M10).ToList()) + "";
            item.M11 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M11).ToList()) + "";
            item.M12 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M12).ToList()) + "";
        }
        #endregion

        #region //2. Calculate Total Non-Combustion (g/sec)
        qProductSumTon = result.objDataEmission.lstNonCombustion.Where(w => w.sType == "SUM").ToList();
        qOperatinghour = lstAllNCS.Where(w => w.ProductID == nMSOperatinghour).ToList();
        foreach (var item in result.objDataEmission.lstNonCombustion.Where(w => w.sType == "SUM2").ToList())
        {
            item.M1 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 1, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M1).ToList()) + "") + "";
            item.M2 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 2, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M2).ToList()) + "") + "";
            item.M3 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 3, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M3).ToList()) + "") + "";
            item.M4 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 4, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M4).ToList()) + "") + "";
            item.M5 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 5, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M5).ToList()) + "") + "";
            item.M6 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 6, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M6).ToList()) + "") + "";
            item.M7 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 7, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M7).ToList()) + "") + "";
            item.M8 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 8, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M8).ToList()) + "") + "";
            item.M9 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 9, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M9).ToList()) + "") + "";
            item.M10 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 10, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M10).ToList()) + "") + "";
            item.M11 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 11, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M11).ToList()) + "") + "";
            item.M12 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 12, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M12).ToList()) + "") + "";
        }
        #endregion
        #endregion

        #region CEM
        List<TData_Emission> lstCEM = new List<TData_Emission>();
        foreach (var item in lstStack)
        {
            lstCEM.AddRange(item.lstDataStackCEM.Where(w => !lstCutProductID.Contains(w.ProductID)).ToList());
        }
        var ProductCEM2H = lstCEM.Where(w => w.sType == "2H").ToList();
        List<TData_Emission> lstCEM2TON = new List<TData_Emission>();
        foreach (var item in ProductCEM2H)
        {
            lstCEM2TON.Add(new TData_Emission
            {
                nStackID = item.nStackID,
                ProductID = item.ProductID,
                sType = item.sType,
                nGroupCalc = item.nGroupCalc,
                cSetCode = item.cSetCode,
                M1 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForCEM(lstCEM, item.nStackID, 1, item.cSetCode, 3, lstCEMProductID), GetValueFromList(lstAllCMS, item.nStackID, 1, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 1, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                M2 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForCEM(lstCEM, item.nStackID, 2, item.cSetCode, 3, lstCEMProductID), GetValueFromList(lstAllCMS, item.nStackID, 2, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 2, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                M3 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForCEM(lstCEM, item.nStackID, 3, item.cSetCode, 3, lstCEMProductID), GetValueFromList(lstAllCMS, item.nStackID, 3, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 3, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                M4 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForCEM(lstCEM, item.nStackID, 4, item.cSetCode, 3, lstCEMProductID), GetValueFromList(lstAllCMS, item.nStackID, 4, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 4, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                M5 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForCEM(lstCEM, item.nStackID, 5, item.cSetCode, 3, lstCEMProductID), GetValueFromList(lstAllCMS, item.nStackID, 5, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 5, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                M6 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForCEM(lstCEM, item.nStackID, 6, item.cSetCode, 3, lstCEMProductID), GetValueFromList(lstAllCMS, item.nStackID, 6, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 6, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                M7 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForCEM(lstCEM, item.nStackID, 7, item.cSetCode, 3, lstCEMProductID), GetValueFromList(lstAllCMS, item.nStackID, 7, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 7, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                M8 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForCEM(lstCEM, item.nStackID, 8, item.cSetCode, 3, lstCEMProductID), GetValueFromList(lstAllCMS, item.nStackID, 8, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 8, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                M9 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForCEM(lstCEM, item.nStackID, 9, item.cSetCode, 3, lstCEMProductID), GetValueFromList(lstAllCMS, item.nStackID, 9, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 9, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                M10 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForCEM(lstCEM, item.nStackID, 10, item.cSetCode, 3, lstCEMProductID), GetValueFromList(lstAllCMS, item.nStackID, 10, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 10, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                M11 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForCEM(lstCEM, item.nStackID, 11, item.cSetCode, 3, lstCEMProductID), GetValueFromList(lstAllCMS, item.nStackID, 11, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 11, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                M12 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForCEM(lstCEM, item.nStackID, 12, item.cSetCode, 3, lstCEMProductID), GetValueFromList(lstAllCMS, item.nStackID, 12, nMSFlowrateID), GetValueFromList(lstAllCMS, item.nStackID, 12, nMSOperatinghour)) + "", sFactorKG2Ton) + ""
            });
        }

        #region //1. Calculate Total CEM (Ton)
        foreach (var item in result.objDataEmission.lstCEM.Where(w => w.sType == "SUM").ToList())
        {
            int nProductIDCal = 0;
            switch (item.ProductID)
            {
                case 160: //Total NOx Emission 
                    nProductIDCal = 175;
                    break;
                case 162://Total SO2> Emission
                    nProductIDCal = 178;
                    break;
                case 164://Total TSP Emission
                    nProductIDCal = 181;
                    break;
                case 166://Total CO Emission
                    nProductIDCal = 184;
                    break;
                case 168://Total Hg Emission
                    nProductIDCal = 187;
                    break;
                case 170://Total H2S Emission
                    nProductIDCal = 190;
                    break;
            }

            var queryToSum = lstCEM2TON.Where(w => w.ProductID == nProductIDCal).ToList();
            item.M1 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M1).ToList()) + "";
            item.M2 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M2).ToList()) + "";
            item.M3 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M3).ToList()) + "";
            item.M4 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M4).ToList()) + "";
            item.M5 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M5).ToList()) + "";
            item.M6 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M6).ToList()) + "";
            item.M7 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M7).ToList()) + "";
            item.M8 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M8).ToList()) + "";
            item.M9 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M9).ToList()) + "";
            item.M10 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M10).ToList()) + "";
            item.M11 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M11).ToList()) + "";
            item.M12 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M12).ToList()) + "";
        }
        #endregion

        #region //2. Calculate Total CEM (g/sec)
        qProductSumTon = result.objDataEmission.lstCEM.Where(w => w.sType == "SUM").ToList();
        qOperatinghour = lstAllCMS.Where(w => w.ProductID == nMSOperatinghour).ToList();
        foreach (var item in result.objDataEmission.lstCEM.Where(w => w.sType == "SUM2").ToList())
        {
            item.M1 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 1, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M1).ToList()) + "") + "";
            item.M2 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 2, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M2).ToList()) + "") + "";
            item.M3 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 3, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M3).ToList()) + "") + "";
            item.M4 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 4, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M4).ToList()) + "") + "";
            item.M5 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 5, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M5).ToList()) + "") + "";
            item.M6 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 6, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M6).ToList()) + "") + "";
            item.M7 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 7, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M7).ToList()) + "") + "";
            item.M8 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 8, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M8).ToList()) + "") + "";
            item.M9 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 9, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M9).ToList()) + "") + "";
            item.M10 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 10, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M10).ToList()) + "") + "";
            item.M11 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 11, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M11).ToList()) + "") + "";
            item.M12 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromList(qProductSumTon, 12, item.nGroupCalc), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M12).ToList()) + "") + "";
        }
        #endregion
        #endregion

        #region ADDITIONAL Combustion
        List<TData_Emission> lstOtherPRD = new List<TData_Emission>();
        db.TM_Emission_OtherProduct.Where(w => w.cActive == "Y" && w.cDel == "N").ToList().ForEach(f =>
        {
            //for (int i = 0; i < 2; i++)
            //{
            lstOtherPRD.Add(new TData_Emission
            {
                ProductID = f.nProductID,
                ProductName = f.sName,
                //sUnit = i == 0 ? "Tonnes" : "g/sec",
                //sType = i == 0 ? "OTH_SUM" : "OTH_SUM2",
                //nOption = null,
            });
            //}
        });
        List<TData_Emission> lstAllAdditionalCMS = new List<TData_Emission>();
        foreach (var item in lstCombustion)
        {
            lstAllAdditionalCMS.AddRange(item.lstDataStack.Where(w => (w.sType == "OTH" && w.cTotal != "Y") || w.nGroupCalc == 7).ToList());
        }
        List<TData_Emission> lstAdditionalCombustion2TON = new List<TData_Emission>();
        foreach (var item in lstAllAdditionalCMS)
        {
            var dataName = lstOtherPRD.FirstOrDefault(w => w.ProductID == item.nOtherProductID);
            if (item.nOptionProduct == 1 && item.nOption == item.nOptionProduct)
            {
                #region Option 1
                lstAdditionalCombustion2TON.Add(new TData_Emission
                {
                    nStackID = item.nStackID,
                    ProductID = item.ProductID,
                    ProductName = dataName != null ? dataName.ProductName : "",
                    sType = item.sType,
                    nGroupCalc = item.nGroupCalc,
                    cSetCode = item.cSetCode,
                    nOption = item.nOption,
                    nOtherProductID = item.nOtherProductID,
                    nOptionProduct = item.nOptionProduct,
                    M1 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 1, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M2 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 2, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M3 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 3, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M4 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 4, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M5 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 5, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M6 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 6, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M7 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 7, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M8 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 8, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M9 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 9, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M10 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 10, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M11 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 11, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M12 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 12, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + ""
                });
                #endregion
            }
            else if (item.nOptionProduct == 2 && item.nOption == item.nOptionProduct)
            {
                #region Option 2
                lstAdditionalCombustion2TON.Add(new TData_Emission
                {
                    nStackID = item.nStackID,
                    ProductID = item.ProductID,
                    ProductName = dataName != null ? dataName.ProductName : "",
                    sType = item.sType,
                    nGroupCalc = item.nGroupCalc,
                    cSetCode = item.cSetCode,
                    nOption = item.nOption,
                    nOtherProductID = item.nOtherProductID,
                    nOptionProduct = item.nOptionProduct,
                    M1 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 1, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 1, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 1, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M2 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 2, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 2, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 2, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M3 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 3, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 3, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 3, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M4 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 4, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 4, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 4, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M5 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 5, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 5, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 5, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M6 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 6, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 6, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 6, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M7 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 7, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 7, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 7, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M8 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 8, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 8, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 8, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M9 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 9, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 9, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 9, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M10 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 10, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 10, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 10, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M11 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 11, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 11, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 11, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M12 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 12, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 12, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 12, nMSOperatinghour)) + "", sFactorKG2Ton) + ""
                });
                #endregion
            }
        }
        #region //1. Calculate Total Addtional Combustion (Ton)
        foreach (var item in lstAdditionalCombustion2TON.ToList())
        {
            if (result.objDataEmission.lstAdditional.Where(w => w.nOtherProductID == item.nOtherProductID && w.sUnit == "Tonnes").Count() == 0)
            {
                var dataName = lstOtherPRD.FirstOrDefault(w => w.ProductID == item.nOtherProductID);
                var queryToSum = lstAdditionalCombustion2TON.Where(w => w.nOtherProductID == item.nOtherProductID).ToList();
                TData_Emission itemDataAdd = new TData_Emission();
                itemDataAdd.nStackID = item.nStackID;
                itemDataAdd.ProductID = item.ProductID;
                itemDataAdd.ProductName = dataName != null ? dataName.ProductName : "";
                itemDataAdd.sType = item.sType;
                itemDataAdd.nGroupCalc = item.nGroupCalc;
                itemDataAdd.cSetCode = item.cSetCode;
                itemDataAdd.nOption = item.nOption;
                itemDataAdd.nOtherProductID = item.nOtherProductID;
                itemDataAdd.nOptionProduct = item.nOptionProduct;
                itemDataAdd.UnitID = 2;
                var dataTarget = lstDataAdditional.FirstOrDefault(w => w.nOtherProductID == item.nOtherProductID && w.UnitID == 2);
                itemDataAdd.Target = dataTarget != null ? dataTarget.Target : "";
                itemDataAdd.M1 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M1).ToList()) + "";
                itemDataAdd.M2 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M2).ToList()) + "";
                itemDataAdd.M3 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M3).ToList()) + "";
                itemDataAdd.M4 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M4).ToList()) + "";
                itemDataAdd.M5 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M5).ToList()) + "";
                itemDataAdd.M6 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M6).ToList()) + "";
                itemDataAdd.M7 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M7).ToList()) + "";
                itemDataAdd.M8 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M8).ToList()) + "";
                itemDataAdd.M9 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M9).ToList()) + "";
                itemDataAdd.M10 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M10).ToList()) + "";
                itemDataAdd.M11 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M11).ToList()) + "";
                itemDataAdd.M12 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M12).ToList()) + "";
                itemDataAdd.sUnit = "Tonnes";

                result.objDataEmission.lstAdditional.Add(itemDataAdd);
            }
        }
        #endregion

        #region //2. Calculate Total Addtional Non-Combustion (g/sec)
        qProductSumTon = result.objDataEmission.lstAdditional.ToList();
        qOperatinghour = lstAllAdditionalCMS.Where(w => w.ProductID == nMSOperatinghour).ToList();
        foreach (var item in lstAdditionalCombustion2TON.ToList())
        {
            if (result.objDataEmission.lstAdditional.Where(w => w.nOtherProductID == item.nOtherProductID && w.sUnit == "g/sec").Count() == 0)
            {
                var dataName = lstOtherPRD.FirstOrDefault(w => w.ProductID == item.nOtherProductID);
                TData_Emission itemDataAdd = new TData_Emission();
                itemDataAdd.nStackID = item.nStackID;
                itemDataAdd.ProductID = item.ProductID;
                itemDataAdd.ProductName = dataName != null ? dataName.ProductName : "";
                itemDataAdd.sType = item.sType;
                itemDataAdd.nGroupCalc = item.nGroupCalc;
                itemDataAdd.cSetCode = item.cSetCode;
                itemDataAdd.nOption = item.nOption;
                itemDataAdd.nOtherProductID = item.nOtherProductID;
                itemDataAdd.nOptionProduct = item.nOptionProduct;
                itemDataAdd.UnitID = 68;
                var dataTarget = lstDataAdditional.FirstOrDefault(w => w.nOtherProductID == item.nOtherProductID && w.UnitID == 68);
                itemDataAdd.Target = dataTarget != null ? dataTarget.Target : "";
                itemDataAdd.M1 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 1, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M1).ToList()) + "") + "";
                itemDataAdd.M2 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 2, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M2).ToList()) + "") + "";
                itemDataAdd.M3 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 3, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M3).ToList()) + "") + "";
                itemDataAdd.M4 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 4, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M4).ToList()) + "") + "";
                itemDataAdd.M5 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 5, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M5).ToList()) + "") + "";
                itemDataAdd.M6 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 6, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M6).ToList()) + "") + "";
                itemDataAdd.M7 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 7, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M7).ToList()) + "") + "";
                itemDataAdd.M8 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 8, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M8).ToList()) + "") + "";
                itemDataAdd.M9 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 9, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M9).ToList()) + "") + "";
                itemDataAdd.M10 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 10, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M10).ToList()) + "") + "";
                itemDataAdd.M11 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 11, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M11).ToList()) + "") + "";
                itemDataAdd.M12 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 12, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M12).ToList()) + "") + "";
                itemDataAdd.sUnit = "g/sec";
                result.objDataEmission.lstAdditional.Add(itemDataAdd);
            }
        }
        #endregion

        result.objDataEmission.lstAdditional = result.objDataEmission.lstAdditional.OrderBy(o => o.ProductName).ToList();
        #endregion

        #region ADDITIONAL NON-Combustion
        lstAllAdditionalCMS = new List<TData_Emission>();
        foreach (var item in lstNonCombustion)
        {
            lstAllAdditionalCMS.AddRange(item.lstDataStack.Where(w => (w.sType == "OTH" && w.cTotal != "Y") || w.nGroupCalc == 7).ToList());
        }
        lstAdditionalCombustion2TON = new List<TData_Emission>();
        foreach (var item in lstAllAdditionalCMS)
        {
            var dataName = lstOtherPRD.FirstOrDefault(w => w.ProductID == item.nOtherProductID);
            if (item.nOptionProduct == 1 && item.nOption == item.nOptionProduct)
            {
                #region Option 1
                lstAdditionalCombustion2TON.Add(new TData_Emission
                {
                    nStackID = item.nStackID,
                    ProductID = item.ProductID,
                    ProductName = dataName != null ? dataName.ProductName : "",
                    sType = item.sType,
                    nGroupCalc = item.nGroupCalc,
                    cSetCode = item.cSetCode,
                    nOption = item.nOption,
                    nOtherProductID = item.nOtherProductID,
                    nOptionProduct = item.nOptionProduct,
                    M1 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 1, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M2 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 2, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M3 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 3, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M4 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 4, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M5 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 5, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M6 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 6, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M7 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 7, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M8 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 8, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M9 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 9, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M10 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 10, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M11 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 11, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + "",
                    M12 = SystemFunction.sysDivideData(GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 12, item.nOtherProductID, 1, item.nID), sFactorKG2Ton) + ""
                });
                #endregion
            }
            else if (item.nOptionProduct == 2 && item.nOption == item.nOptionProduct)
            {
                #region Option 2
                lstAdditionalCombustion2TON.Add(new TData_Emission
                {
                    nStackID = item.nStackID,
                    ProductID = item.ProductID,
                    ProductName = dataName != null ? dataName.ProductName : "",
                    sType = item.sType,
                    nGroupCalc = item.nGroupCalc,
                    cSetCode = item.cSetCode,
                    nOption = item.nOption,
                    nOtherProductID = item.nOtherProductID,
                    nOptionProduct = item.nOptionProduct,
                    M1 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 1, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 1, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 1, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M2 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 2, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 2, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 2, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M3 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 3, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 3, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 3, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M4 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 4, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 4, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 4, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M5 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 5, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 5, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 5, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M6 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 6, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 6, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 6, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M7 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 7, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 7, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 7, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M8 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 8, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 8, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 8, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M9 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 9, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 9, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 9, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M10 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 10, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 10, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 10, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M11 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 11, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 11, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 11, nMSOperatinghour)) + "", sFactorKG2Ton) + "",
                    M12 = SystemFunction.sysDivideData(SysFunctionCalculate.CalculateEmissionConcentration(item.ProductID, GetValueFromListForAdditional(lstAllAdditionalCMS, item.nStackID, 12, item.nOtherProductID, 2, item.nID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 12, nMSFlowrateID), GetValueFromList(lstAllAdditionalCMS, item.nStackID, 12, nMSOperatinghour)) + "", sFactorKG2Ton) + ""
                });
                #endregion
            }
        }
        #region //1. Calculate Total Addtional Non-Combustion (Ton)
        foreach (var item in lstAdditionalCombustion2TON.ToList())
        {
            if (result.objDataEmission.lstAdditionalNonCombustion.Where(w => w.nOtherProductID == item.nOtherProductID && w.sUnit == "Tonnes").Count() == 0)
            {
                var dataName = lstOtherPRD.FirstOrDefault(w => w.ProductID == item.nOtherProductID);
                var queryToSum = lstAdditionalCombustion2TON.Where(w => w.nOtherProductID == item.nOtherProductID).ToList();
                TData_Emission itemDataAdd = new TData_Emission();
                itemDataAdd.nStackID = item.nStackID;
                itemDataAdd.ProductID = item.ProductID;
                itemDataAdd.ProductName = dataName != null ? dataName.ProductName : "";
                itemDataAdd.sType = item.sType;
                itemDataAdd.nGroupCalc = item.nGroupCalc;
                itemDataAdd.cSetCode = item.cSetCode;
                itemDataAdd.nOption = item.nOption;
                itemDataAdd.nOtherProductID = item.nOtherProductID;
                itemDataAdd.nOptionProduct = item.nOptionProduct;
                itemDataAdd.UnitID = 2;
                var dataTarget = lstDataAdditionalNonCombustion.FirstOrDefault(w => w.nOtherProductID == item.nOtherProductID && w.UnitID == 2);
                itemDataAdd.Target = dataTarget != null ? dataTarget.Target : "";
                itemDataAdd.M1 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M1).ToList()) + "";
                itemDataAdd.M2 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M2).ToList()) + "";
                itemDataAdd.M3 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M3).ToList()) + "";
                itemDataAdd.M4 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M4).ToList()) + "";
                itemDataAdd.M5 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M5).ToList()) + "";
                itemDataAdd.M6 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M6).ToList()) + "";
                itemDataAdd.M7 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M7).ToList()) + "";
                itemDataAdd.M8 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M8).ToList()) + "";
                itemDataAdd.M9 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M9).ToList()) + "";
                itemDataAdd.M10 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M10).ToList()) + "";
                itemDataAdd.M11 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M11).ToList()) + "";
                itemDataAdd.M12 = SystemFunction.SumDataToDecimal(queryToSum.Select(s => s.M12).ToList()) + "";
                itemDataAdd.sUnit = "Tonnes";
                result.objDataEmission.lstAdditionalNonCombustion.Add(itemDataAdd);
            }
        }
        #endregion

        #region //2. Calculate Total Addtional Non-Combustion (g/sec)
        qProductSumTon = result.objDataEmission.lstAdditionalNonCombustion.ToList();
        qOperatinghour = lstAllAdditionalCMS.Where(w => w.ProductID == nMSOperatinghour).ToList();
        foreach (var item in lstAdditionalCombustion2TON.ToList())
        {
            if (result.objDataEmission.lstAdditionalNonCombustion.Where(w => w.nOtherProductID == item.nOtherProductID && w.sUnit == "g/sec").Count() == 0)
            {
                var dataName = lstOtherPRD.FirstOrDefault(w => w.ProductID == item.nOtherProductID);
                TData_Emission itemDataAdd = new TData_Emission();
                itemDataAdd.nStackID = item.nStackID;
                itemDataAdd.ProductID = item.ProductID;
                itemDataAdd.ProductName = dataName != null ? dataName.ProductName : "";
                itemDataAdd.sType = item.sType;
                itemDataAdd.nGroupCalc = item.nGroupCalc;
                itemDataAdd.cSetCode = item.cSetCode;
                itemDataAdd.nOption = item.nOption;
                itemDataAdd.nOtherProductID = item.nOtherProductID;
                itemDataAdd.nOptionProduct = item.nOptionProduct;
                itemDataAdd.UnitID = 68;
                var dataTarget = lstDataAdditionalNonCombustion.FirstOrDefault(w => w.nOtherProductID == item.nOtherProductID && w.UnitID == 68);
                itemDataAdd.Target = dataTarget != null ? dataTarget.Target : "";
                itemDataAdd.M1 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 1, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M1).ToList()) + "") + "";
                itemDataAdd.M2 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 2, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M2).ToList()) + "") + "";
                itemDataAdd.M3 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 3, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M3).ToList()) + "") + "";
                itemDataAdd.M4 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 4, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M4).ToList()) + "") + "";
                itemDataAdd.M5 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 5, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M5).ToList()) + "") + "";
                itemDataAdd.M6 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 6, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M6).ToList()) + "") + "";
                itemDataAdd.M7 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 7, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M7).ToList()) + "") + "";
                itemDataAdd.M8 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 8, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M8).ToList()) + "") + "";
                itemDataAdd.M9 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 9, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M9).ToList()) + "") + "";
                itemDataAdd.M10 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 10, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M10).ToList()) + "") + "";
                itemDataAdd.M11 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 11, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M11).ToList()) + "") + "";
                itemDataAdd.M12 = SysFunctionCalculate.CalculateGrumPerSecond(GetValueFromListForGrumPerSec(qProductSumTon, 12, item.nOtherProductID), SystemFunction.SumDataToDecimal(qOperatinghour.Select(s => s.M12).ToList()) + "") + "";
                itemDataAdd.sUnit = "g/sec";
                result.objDataEmission.lstAdditionalNonCombustion.Add(itemDataAdd);
            }
        }
        #endregion

        result.objDataEmission.lstAdditionalNonCombustion = result.objDataEmission.lstAdditionalNonCombustion.OrderBy(o => o.ProductName).ToList();
        #endregion

        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveToDB(cDataForSave arrData)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (!UserAcc.UserExpired())
        {
            Func<List<string>, string> Sum = (lstData) =>
            {
                string sReusltData = "";
                decimal TotalDecimal = 0;
                foreach (var item in lstData)
                {
                    if (item != null)
                    {
                        if (item.Trim() != "" && item.ToLower().Trim() != "n/a")
                        {
                            TotalDecimal += SystemFunction.ParseDecimal(item.Trim());
                            sReusltData = TotalDecimal + "";
                        }
                    }
                }
                return sReusltData;
            };
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
                    db.TEmission_OtherProduct_Stack.RemoveRange(db.TEmission_OtherProduct_Stack.Where(w => w.FormID == FORM_ID));
                    db.TEmission_Product.RemoveRange(db.TEmission_Product.Where(w => w.FormID == FORM_ID));
                    db.TEmission_Product_ByType.RemoveRange(db.TEmission_Product_ByType.Where(w => w.FormID == FORM_ID));
                    db.TEmission_Product_Stack.RemoveRange(db.TEmission_Product_Stack.Where(w => w.FormID == FORM_ID));
                    db.TEmission_Stack.RemoveRange(db.TEmission_Stack.Where(w => w.FormID == FORM_ID));
                    db.TEmission_VOC.RemoveRange(db.TEmission_VOC.Where(w => w.FormID == FORM_ID));
                    //db.TEPI_Forms_File.RemoveRange(db.TEPI_Forms_File.Where(w => w.FormID == FORM_ID));
                    db.SaveChanges();
                }

                if (arrData.nOperationID != 13) // OperationID  = 13 Chemical Transportation & Storage จะเก็บเฉพาะ VOC
                {
                    #region TEmission_Product
                    arrData.lstDataCombustion.ForEach(f =>
                    {
                        TEmission_Product item = new TEmission_Product();
                        item.FormID = FORM_ID;
                        item.ProductID = f.ProductID;
                        item.UnitID = 0;
                        item.M1 = f.M1;
                        item.M2 = f.M2;
                        item.M3 = f.M3;
                        item.M4 = f.M4;
                        item.M5 = f.M5;
                        item.M6 = f.M6;
                        item.M7 = f.M7;
                        item.M8 = f.M8;
                        item.M9 = f.M9;
                        item.M10 = f.M10;
                        item.M11 = f.M11;
                        item.M12 = f.M12;

                        item.IsCheckM1 = f.IsCheckM1;
                        item.IsCheckM2 = f.IsCheckM2;
                        item.IsCheckM3 = f.IsCheckM3;
                        item.IsCheckM4 = f.IsCheckM4;
                        item.IsCheckM5 = f.IsCheckM5;
                        item.IsCheckM6 = f.IsCheckM6;
                        item.IsCheckM7 = f.IsCheckM7;
                        item.IsCheckM8 = f.IsCheckM8;
                        item.IsCheckM9 = f.IsCheckM9;
                        item.IsCheckM10 = f.IsCheckM10;
                        item.IsCheckM11 = f.IsCheckM11;
                        item.IsCheckM12 = f.IsCheckM12;

                        item.Target = f.Target;
                        List<string> lstForSum = new List<string> { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                        item.nTotal = Sum(lstForSum);
                        db.TEmission_Product.Add(item);
                    });
                    #endregion

                    #region TEmission_Product_ByType

                    #region Combustion
                    arrData.lstDataCombustion.ForEach(f =>
                    {
                        TEmission_Product_ByType item = new TEmission_Product_ByType();
                        item.FormID = FORM_ID;
                        item.ProductID = f.ProductID;
                        item.sType = "CBS";
                        item.UnitID = 0;
                        item.M1 = f.M1;
                        item.M2 = f.M2;
                        item.M3 = f.M3;
                        item.M4 = f.M4;
                        item.M5 = f.M5;
                        item.M6 = f.M6;
                        item.M7 = f.M7;
                        item.M8 = f.M8;
                        item.M9 = f.M9;
                        item.M10 = f.M10;
                        item.M11 = f.M11;
                        item.M12 = f.M12;

                        item.IsCheckM1 = f.IsCheckM1;
                        item.IsCheckM2 = f.IsCheckM2;
                        item.IsCheckM3 = f.IsCheckM3;
                        item.IsCheckM4 = f.IsCheckM4;
                        item.IsCheckM5 = f.IsCheckM5;
                        item.IsCheckM6 = f.IsCheckM6;
                        item.IsCheckM7 = f.IsCheckM7;
                        item.IsCheckM8 = f.IsCheckM8;
                        item.IsCheckM9 = f.IsCheckM9;
                        item.IsCheckM10 = f.IsCheckM10;
                        item.IsCheckM11 = f.IsCheckM11;
                        item.IsCheckM12 = f.IsCheckM12;

                        item.Target = f.Target;
                        List<string> lstForSum = new List<string> { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                        item.nTotal = Sum(lstForSum);
                        db.TEmission_Product_ByType.Add(item);
                    });
                    #endregion

                    #region Non-Combustion
                    arrData.lstDataNonCombustion.ForEach(f =>
                    {
                        TEmission_Product_ByType item = new TEmission_Product_ByType();
                        item.FormID = FORM_ID;
                        item.ProductID = f.ProductID;
                        item.sType = "NCS";
                        item.UnitID = 0;
                        item.M1 = f.M1;
                        item.M2 = f.M2;
                        item.M3 = f.M3;
                        item.M4 = f.M4;
                        item.M5 = f.M5;
                        item.M6 = f.M6;
                        item.M7 = f.M7;
                        item.M8 = f.M8;
                        item.M9 = f.M9;
                        item.M10 = f.M10;
                        item.M11 = f.M11;
                        item.M12 = f.M12;

                        item.IsCheckM1 = f.IsCheckM1;
                        item.IsCheckM2 = f.IsCheckM2;
                        item.IsCheckM3 = f.IsCheckM3;
                        item.IsCheckM4 = f.IsCheckM4;
                        item.IsCheckM5 = f.IsCheckM5;
                        item.IsCheckM6 = f.IsCheckM6;
                        item.IsCheckM7 = f.IsCheckM7;
                        item.IsCheckM8 = f.IsCheckM8;
                        item.IsCheckM9 = f.IsCheckM9;
                        item.IsCheckM10 = f.IsCheckM10;
                        item.IsCheckM11 = f.IsCheckM11;
                        item.IsCheckM12 = f.IsCheckM12;

                        item.Target = f.Target;
                        List<string> lstForSum = new List<string> { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                        item.nTotal = Sum(lstForSum);
                        db.TEmission_Product_ByType.Add(item);
                    });
                    #endregion

                    #region CEM
                    arrData.lstDataCEM.ForEach(f =>
                    {
                        TEmission_Product_ByType item = new TEmission_Product_ByType();
                        item.FormID = FORM_ID;
                        item.ProductID = f.ProductID;
                        item.sType = "CEM";
                        item.UnitID = 0;
                        item.M1 = f.M1;
                        item.M2 = f.M2;
                        item.M3 = f.M3;
                        item.M4 = f.M4;
                        item.M5 = f.M5;
                        item.M6 = f.M6;
                        item.M7 = f.M7;
                        item.M8 = f.M8;
                        item.M9 = f.M9;
                        item.M10 = f.M10;
                        item.M11 = f.M11;
                        item.M12 = f.M12;

                        item.IsCheckM1 = f.IsCheckM1;
                        item.IsCheckM2 = f.IsCheckM2;
                        item.IsCheckM3 = f.IsCheckM3;
                        item.IsCheckM4 = f.IsCheckM4;
                        item.IsCheckM5 = f.IsCheckM5;
                        item.IsCheckM6 = f.IsCheckM6;
                        item.IsCheckM7 = f.IsCheckM7;
                        item.IsCheckM8 = f.IsCheckM8;
                        item.IsCheckM9 = f.IsCheckM9;
                        item.IsCheckM10 = f.IsCheckM10;
                        item.IsCheckM11 = f.IsCheckM11;
                        item.IsCheckM12 = f.IsCheckM12;

                        item.Target = f.Target;
                        List<string> lstForSum = new List<string> { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                        item.nTotal = Sum(lstForSum);
                        db.TEmission_Product_ByType.Add(item);
                    });
                    #endregion

                    #region Additional
                    arrData.lstDataAdditional.ForEach(f =>
                    {
                        TEmission_Product_ByType item = new TEmission_Product_ByType();
                        item.FormID = FORM_ID;
                        item.ProductID = f.nOtherProductID.HasValue ? f.nOtherProductID.Value : 0;
                        item.sType = "ACS";
                        item.UnitID = f.sUnit == "Tonnes" ? 2 : 68;
                        item.M1 = f.M1;
                        item.M2 = f.M2;
                        item.M3 = f.M3;
                        item.M4 = f.M4;
                        item.M5 = f.M5;
                        item.M6 = f.M6;
                        item.M7 = f.M7;
                        item.M8 = f.M8;
                        item.M9 = f.M9;
                        item.M10 = f.M10;
                        item.M11 = f.M11;
                        item.M12 = f.M12;

                        item.IsCheckM1 = f.IsCheckM1;
                        item.IsCheckM2 = f.IsCheckM2;
                        item.IsCheckM3 = f.IsCheckM3;
                        item.IsCheckM4 = f.IsCheckM4;
                        item.IsCheckM5 = f.IsCheckM5;
                        item.IsCheckM6 = f.IsCheckM6;
                        item.IsCheckM7 = f.IsCheckM7;
                        item.IsCheckM8 = f.IsCheckM8;
                        item.IsCheckM9 = f.IsCheckM9;
                        item.IsCheckM10 = f.IsCheckM10;
                        item.IsCheckM11 = f.IsCheckM11;
                        item.IsCheckM12 = f.IsCheckM12;

                        item.Target = f.Target;
                        List<string> lstForSum = new List<string> { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                        item.nTotal = Sum(lstForSum);
                        db.TEmission_Product_ByType.Add(item);
                    });
                    #endregion

                    #region Non-Additional
                    arrData.lstDataAdditionalNonCombustion.ForEach(f =>
                    {
                        TEmission_Product_ByType item = new TEmission_Product_ByType();
                        item.FormID = FORM_ID;
                        item.ProductID = f.nOtherProductID.HasValue ? f.nOtherProductID.Value : 0;
                        item.sType = "ANC";
                        item.UnitID = f.sUnit == "Tonnes" ? 2 : 68;
                        item.M1 = f.M1;
                        item.M2 = f.M2;
                        item.M3 = f.M3;
                        item.M4 = f.M4;
                        item.M5 = f.M5;
                        item.M6 = f.M6;
                        item.M7 = f.M7;
                        item.M8 = f.M8;
                        item.M9 = f.M9;
                        item.M10 = f.M10;
                        item.M11 = f.M11;
                        item.M12 = f.M12;

                        item.IsCheckM1 = f.IsCheckM1;
                        item.IsCheckM2 = f.IsCheckM2;
                        item.IsCheckM3 = f.IsCheckM3;
                        item.IsCheckM4 = f.IsCheckM4;
                        item.IsCheckM5 = f.IsCheckM5;
                        item.IsCheckM6 = f.IsCheckM6;
                        item.IsCheckM7 = f.IsCheckM7;
                        item.IsCheckM8 = f.IsCheckM8;
                        item.IsCheckM9 = f.IsCheckM9;
                        item.IsCheckM10 = f.IsCheckM10;
                        item.IsCheckM11 = f.IsCheckM11;
                        item.IsCheckM12 = f.IsCheckM12;

                        item.Target = f.Target;
                        List<string> lstForSum = new List<string> { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                        item.nTotal = Sum(lstForSum);
                        db.TEmission_Product_ByType.Add(item);
                    });
                    #endregion

                    #endregion

                    #region TEmission_Stack

                    int nStackID = db.TEmission_Stack.Any(a => a.FormID == FORM_ID) ? db.TEmission_Stack.Where(w => w.FormID == FORM_ID).Max(m => m.nStackID) + 1 : 1;
                    arrData.lstStack.ForEach(f =>
                    {
                        TEmission_Stack item = new TEmission_Stack();
                        item.FormID = FORM_ID;
                        item.nStackID = nStackID;
                        item.sStackName = f.sStackName;
                        item.sRemark = f.sRemark;
                        item.nAddBy = UserAcc.GetObjUser().nUserID;
                        item.dAdd = DateTime.Now;
                        item.sStackType = f.sStackType;
                        db.TEmission_Stack.Add(item);
                        #region TEmission_Product_Stack
                        f.lstDataStack.ForEach(f2 =>
                        {
                            if (f2.sType == "OTH")// Save Data Additional
                            {
                                TEmission_OtherProduct_Stack itemOther = new TEmission_OtherProduct_Stack();
                                itemOther.FormID = FORM_ID;
                                itemOther.nStackID = nStackID;
                                itemOther.nOtherProductID = f2.nOtherProductID.HasValue ? f2.nOtherProductID.Value : 0;
                                itemOther.ProductID = f2.ProductID;
                                itemOther.nID = f2.nID.HasValue ? f2.nID.Value : 0;
                                itemOther.UnitID = 0;
                                itemOther.nOptionProduct = f2.nOptionProduct;
                                itemOther.cIsVOCs = f2.cIsVOCs;

                                itemOther.M1 = f2.M1;
                                itemOther.M2 = f2.M2;
                                itemOther.M3 = f2.M3;
                                itemOther.M4 = f2.M4;
                                itemOther.M5 = f2.M5;
                                itemOther.M6 = f2.M6;
                                itemOther.M7 = f2.M7;
                                itemOther.M8 = f2.M8;
                                itemOther.M9 = f2.M9;
                                itemOther.M10 = f2.M10;
                                itemOther.M11 = f2.M11;
                                itemOther.M12 = f2.M12;

                                itemOther.Target = f2.Target;
                                List<string> lstForSum = new List<string> { itemOther.M1, itemOther.M2, itemOther.M3, itemOther.M4, itemOther.M5, itemOther.M6, itemOther.M7, itemOther.M8, itemOther.M9, itemOther.M10, itemOther.M11, itemOther.M12 };
                                itemOther.nTotal = Sum(lstForSum);
                                db.TEmission_OtherProduct_Stack.Add(itemOther);
                            }
                            else // Save Data General
                            {
                                TEmission_Product_Stack itemStack = new TEmission_Product_Stack();
                                itemStack.FormID = FORM_ID;
                                itemStack.nStackID = nStackID;
                                itemStack.ProductID = f2.ProductID;
                                itemStack.UnitID = 0;
                                itemStack.nOptionProduct = f2.nOptionProduct;
                                itemStack.M1 = f2.M1;
                                itemStack.M2 = f2.M2;
                                itemStack.M3 = f2.M3;
                                itemStack.M4 = f2.M4;
                                itemStack.M5 = f2.M5;
                                itemStack.M6 = f2.M6;
                                itemStack.M7 = f2.M7;
                                itemStack.M8 = f2.M8;
                                itemStack.M9 = f2.M9;
                                itemStack.M10 = f2.M10;
                                itemStack.M11 = f2.M11;
                                itemStack.M12 = f2.M12;

                                itemStack.Target = f2.Target;
                                List<string> lstForSum = new List<string> { itemStack.M1, itemStack.M2, itemStack.M3, itemStack.M4, itemStack.M5, itemStack.M6, itemStack.M7, itemStack.M8, itemStack.M9, itemStack.M10, itemStack.M11, itemStack.M12 };
                                itemStack.nTotal = Sum(lstForSum);
                                db.TEmission_Product_Stack.Add(itemStack);
                            }
                        });
                        f.lstDataStackCEM.Where(w => w.sType != "2H").ForEach(f2 => // Save Data CEM
                        {
                            TEmission_Product_Stack itemStack = new TEmission_Product_Stack();
                            itemStack.FormID = FORM_ID;
                            itemStack.nStackID = nStackID;
                            itemStack.ProductID = f2.ProductID;
                            itemStack.UnitID = 0;
                            itemStack.nOptionProduct = 3; // กรณีเป็น CEM จะเก็ยค่าออฟชั่นเป็น 3 
                            if (f.sStackType == "CMS")
                            {
                                itemStack.M1 = f2.M1;
                                itemStack.M2 = f2.M2;
                                itemStack.M3 = f2.M3;
                                itemStack.M4 = f2.M4;
                                itemStack.M5 = f2.M5;
                                itemStack.M6 = f2.M6;
                                itemStack.M7 = f2.M7;
                                itemStack.M8 = f2.M8;
                                itemStack.M9 = f2.M9;
                                itemStack.M10 = f2.M10;
                                itemStack.M11 = f2.M11;
                                itemStack.M12 = f2.M12;

                                itemStack.Target = f2.Target;
                                List<string> lstForSum = new List<string> { itemStack.M1, itemStack.M2, itemStack.M3, itemStack.M4, itemStack.M5, itemStack.M6, itemStack.M7, itemStack.M8, itemStack.M9, itemStack.M10, itemStack.M11, itemStack.M12 };
                                itemStack.nTotal = Sum(lstForSum);
                            }
                            else
                            {
                                itemStack.M1 = "";
                                itemStack.M2 = "";
                                itemStack.M3 = "";
                                itemStack.M4 = "";
                                itemStack.M5 = "";
                                itemStack.M6 = "";
                                itemStack.M7 = "";
                                itemStack.M8 = "";
                                itemStack.M9 = "";
                                itemStack.M10 = "";
                                itemStack.M11 = "";
                                itemStack.M12 = "";

                                itemStack.Target = "";
                                itemStack.nTotal = "";
                            }
                            db.TEmission_Product_Stack.Add(itemStack);
                        });
                        #endregion
                        nStackID++;
                    });
                    #endregion

                }

                #region TEmission_VOC
                arrData.objVOC.lstVOC.ForEach(f =>
                {
                    TEmission_VOC item = new TEmission_VOC();
                    item.FormID = FORM_ID;
                    item.ProductID = f.ProductID;
                    item.UnitID = 0;
                    item.sOption = f.sOption;
                    item.M1 = f.sOption == "Y" ? "" : f.M1;
                    item.M2 = f.sOption == "Y" ? "" : f.M2;
                    item.M3 = f.sOption == "Y" ? "" : f.M3;
                    item.M4 = f.sOption == "Y" ? "" : f.M4;
                    item.M5 = f.sOption == "Y" ? "" : f.M5;
                    item.M6 = f.sOption == "Y" ? "" : f.M6;
                    item.M7 = f.sOption == "Y" ? "" : f.M7;
                    item.M8 = f.sOption == "Y" ? "" : f.M8;
                    item.M9 = f.sOption == "Y" ? "" : f.M9;
                    item.M10 = f.sOption == "Y" ? "" : f.M10;
                    item.M11 = f.sOption == "Y" ? "" : f.M11;
                    item.M12 = f.sOption == "Y" ? "" : f.M12;

                    item.IsCheckM1 = f.sOption == "Y" ? "N" : f.IsCheckM1;
                    item.IsCheckM2 = f.sOption == "Y" ? "N" : f.IsCheckM2;
                    item.IsCheckM3 = f.sOption == "Y" ? "N" : f.IsCheckM3;
                    item.IsCheckM4 = f.sOption == "Y" ? "N" : f.IsCheckM4;
                    item.IsCheckM5 = f.sOption == "Y" ? "N" : f.IsCheckM5;
                    item.IsCheckM6 = f.sOption == "Y" ? "N" : f.IsCheckM6;
                    item.IsCheckM7 = f.sOption == "Y" ? "N" : f.IsCheckM7;
                    item.IsCheckM8 = f.sOption == "Y" ? "N" : f.IsCheckM8;
                    item.IsCheckM9 = f.sOption == "Y" ? "N" : f.IsCheckM9;
                    item.IsCheckM10 = f.sOption == "Y" ? "N" : f.IsCheckM10;
                    item.IsCheckM11 = f.sOption == "Y" ? "N" : f.IsCheckM11;
                    item.IsCheckM12 = f.sOption == "Y" ? "N" : f.IsCheckM12;
                    item.Target = f.Target;
                    List<string> lstForSum = new List<string> { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                    if (f.sOption == "Y")
                    {
                        item.nTotal = f.nTotal;
                    }
                    else
                    {
                        item.nTotal = Sum(lstForSum);
                    }
                    db.TEmission_VOC.Add(item);

                });
                #endregion

                #region Remark
                if (!string.IsNullOrEmpty(arrData.objVOC.sRemarkVOC))
                {
                    var itemRemark = db.TEmission_Remark.Where(w => w.FormID == FORM_ID);
                    int nVersion = itemRemark.Any() ? itemRemark.Max(m => m.nVersion) + 1 : 1;
                    TEmission_Remark rmk = new TEmission_Remark();
                    rmk.FormID = FORM_ID;
                    rmk.ProductID = 193;
                    rmk.nVersion = nVersion;
                    rmk.sRemark = arrData.objVOC.sRemarkVOC;
                    if (itemRemark.Count() > 0)
                    {
                        if (!SystemFunction.IsSuperAdmin())
                        {
                            rmk.sAddBy = UserAcc.GetObjUser().nUserID;
                            rmk.dAddDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        rmk.sAddBy = UserAcc.GetObjUser().nUserID;
                        rmk.dAddDate = DateTime.Now;
                    }
                    db.TEmission_Remark.Add(rmk);
                }
                #endregion

                #region FILE
                if (arrData.lstFile.Any())
                {
                    string sPathSave = string.Format(sFolderInPathSave, FORM_ID);
                    SystemFunction.CreateDirectory(sPathSave);

                    //ลบไฟล์เดิมที่เคยมีและกดลบจากหน้าเว็บ
                    var qDelFile = arrData.lstFile.Where(w => w.IsNewFile == false && w.sDelete == "Y").ToList();
                    if (qDelFile.Any())
                    {
                        foreach (var qf in qDelFile)
                        {
                            var query = db.TEPI_Forms_File.FirstOrDefault(w => w.FormID == FORM_ID && w.sSysFileName == qf.SaveToFileName);
                            if (query != null)
                            {
                                new SystemFunction().DeleteFileInServer(query.sPath, query.sSysFileName);
                                // new SystemFunction2().DeleteFileInServer(query.sSysPath, query.sSysFileName);
                                db.TEPI_Forms_File.Remove(query);
                                ///db.TAuditPlan_AttachFile.Remove(query);
                            }
                        }
                        db.SaveChanges();
                    }
                    //Update Description
                    arrData.lstFile.Where(w => w.IsNewFile == false && w.sDelete == "N").ToList().ForEach(f2U =>
                    {
                        var data2Update = db.TEPI_Forms_File.FirstOrDefault(w => w.FormID == FORM_ID && w.nFileID == f2U.ID);
                        if (data2Update != null)
                        {
                            data2Update.sDescription = f2U.sDescription;
                        }
                    });
                    //Save New File Only
                    var lstSave = arrData.lstFile.Where(w => w.IsNewFile == true && w.sDelete == "N").ToList();
                    if (lstSave.Any())
                    {
                        int nFileID = db.TEPI_Forms_File.Where(w => w.FormID == FORM_ID).Any() ? db.TEPI_Forms_File.Where(w => w.FormID == FORM_ID).Max(m => m.nFileID) + 1 : 1;

                        foreach (var s in lstSave)
                        {
                            string sSystemFileName = FORM_ID + "_" + nFileID + "-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "." + SystemFunction.GetFileNameFromFileupload(s.SaveToFileName, ""); //+ "." + SystemFunction.GetFileNameFromFileupload(s.SaveToFileName, "") SystemFunction2.GetFileType(item.SaveToFileName);
                            SystemFunction.UpFile2Server(s.SaveToPath, sPathSave, s.SaveToFileName, sSystemFileName);

                            // SystemFunction2.UpFile2Server(item.SaveToPath, sPathSave, item.SaveToFileName, sSystemFileName);
                            TEPI_Forms_File t = new TEPI_Forms_File();
                            t.FormID = FORM_ID;
                            t.nFileID = nFileID;
                            t.sSysFileName = sSystemFileName;
                            t.sFileName = s.FileName;
                            t.sPath = sPathSave;
                            t.sDescription = s.sDescription;
                            t.dAdd = DateTime.Now;
                            t.nAddBy = UserAcc.GetObjUser().nUserID;
                            db.TEPI_Forms_File.Add(t);
                            nFileID++;
                        }
                    }

                    db.SaveChanges();
                    //Save OLD File Only
                    var lstSaveOld = arrData.lstFile.Where(w => w.IsNewFile == false && w.sDelete == "N").ToList();
                    if (lstSaveOld.Any())
                    {
                        foreach (var f in lstSaveOld)
                        {
                            TEPI_Forms_File dFile = new TEPI_Forms_File();

                            int nFileID = 0;
                            nFileID = f.ID;
                            dFile = db.TEPI_Forms_File.FirstOrDefault(w => w.FormID == FORM_ID && w.nFileID == nFileID);
                            if (dFile != null)
                            {
                                dFile.sDescription = f.sDescription;
                            }
                            db.SaveChanges();
                        }
                    }
                }
                #endregion

                db.SaveChanges();

                new EPIFunc().RecalculateEmission(arrData.nOperationID, arrData.nFacilityID, arrData.sYear);
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
        List<int> lstOtherID = new List<int>() { 20, 27, 30, 75 };
        CReturnData result = new CReturnData();
        result.objDataEmission = new ObjData();
        result.lstAddStack = new List<TData_Emission>();
        result.lstAddStackCEM = new List<TData_Emission>();
        result.objDataEmission.lstCombustion = new List<TData_Emission>();
        result.objDataEmission.lstNonCombustion = new List<TData_Emission>();
        result.objDataEmission.lstCEM = new List<TData_Emission>();
        result.objDataEmission.lstAdditional = new List<TData_Emission>();
        result.objDataEmission.lstAdditionalNonCombustion = new List<TData_Emission>();
        result.objDataEmission.lstStack = new List<TData_Stack>();
        result.lstAddAdditional = new List<TData_Emission>();
        result.lstOtherPrd = new List<TM_Emission_OtherProduct>();
        result.lstVOC = new List<TData_Emission>();
        bool IsNew = true;
        var itemEPI_FORM = db.TEPI_Forms.FirstOrDefault(w => w.sYear == sYear && w.IDIndicator == nIndID && w.OperationTypeID == nOprtID && w.FacilityID == nFacID);
        int EPI_FORMID = 0;
        #region mTProductIndicator
        var lstTProduct = db.mTProductIndicator.Where(w => w.IDIndicator == nIndicator).ToList();
        var lstUseProduct = db.TEmissionUseProduct.Where(w => w.OperationTypeID == nOprtID).Select(s => new
        {
            ProductID = s.ProductID,
            nOrder = s.nOrder,
            cSetCode = s.cSetCode,
        }).ToList();
        //var lstmTProductIndicator = db.mTProductIndicator.Where(w => w.IDIndicator == nIndicator && lstUseProduct.Contains(w.ProductID)).ToList();
        var lstmTProductIndicator = (from p in lstTProduct
                                     from u in lstUseProduct.Where(w => w.ProductID == p.ProductID)
                                     select new
                                     {
                                         ProductID = p.ProductID,
                                         ProductName = p.ProductName,
                                         sType = p.sType,
                                         sUnit = p.sUnit,
                                         nGroupCalc = p.nGroupCalc,
                                         nOption = p.nOption,
                                         cTotal = p.cTotal,
                                         nOrder = u.nOrder,
                                         cSetCode = u.cSetCode,
                                     }).ToList();
        lstmTProductIndicator.Where(w => w.sType == "2" || w.sType == "2H").OrderBy(o => o.nOrder).ThenBy(o2 => o2.nGroupCalc).ToList().ForEach(f =>
        {
            result.lstAddStack.Add(new TData_Emission
            {
                ProductID = f.ProductID,
                ProductName = f.ProductName.Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 "),
                sUnit = f.sUnit,
                sType = f.sType,
                nOption = f.nOption ?? null,
                cTotal = f.cTotal,
                nGroupCalc = f.nGroupCalc,
                cSetCode = f.cSetCode,
            });
        });
        lstmTProductIndicator.Where(w => w.sType == "CEM" || w.sType == "2H").OrderBy(o => o.nOrder).ThenBy(o2 => o2.nGroupCalc).ToList().ForEach(f =>
        {
            result.lstAddStackCEM.Add(new TData_Emission
            {
                ProductID = f.ProductID,
                ProductName = f.ProductName.Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 "),
                sUnit = f.sUnit,
                sType = f.sType,
                nOption = f.nOption ?? null,
                cTotal = f.cTotal,
                nGroupCalc = f.nGroupCalc,
                cSetCode = f.cSetCode,
                M1 = "",
                M2 = "",
                M3 = "",
                M4 = "",
                M5 = "",
                M6 = "",
                M7 = "",
                M8 = "",
                M9 = "",
                M10 = "",
                M11 = "",
                M12 = "",
                Target = "",

                IsCheckM1 = "N",
                IsCheckM2 = "N",
                IsCheckM3 = "N",
                IsCheckM4 = "N",
                IsCheckM5 = "N",
                IsCheckM6 = "N",
                IsCheckM7 = "N",
                IsCheckM8 = "N",
                IsCheckM9 = "N",
                IsCheckM10 = "N",
                IsCheckM11 = "N",
                IsCheckM12 = "N",
            });

        });
        lstmTProductIndicator.Where(w => w.sType == "SUM" || w.sType == "SUM2").ToList().ForEach(f =>
        {
            if (f.sType == "SUM" || f.sType == "SUM2")
            {
                result.objDataEmission.lstCombustion.Add(new TData_Emission
                {
                    ProductID = f.ProductID,
                    ProductName = f.ProductName.Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 "),
                    UnitID = f.sType == "SUM" ? (int?)2 : null,
                    sUnit = f.sUnit,
                    sType = f.sType,
                    cTotal = f.cTotal,
                    cSetCode = f.cSetCode,
                    nGroupCalc = f.nGroupCalc,
                });
                result.objDataEmission.lstNonCombustion.Add(new TData_Emission
                {
                    ProductID = f.ProductID,
                    ProductName = f.ProductName.Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 "),
                    UnitID = f.sType == "SUM" ? (int?)2 : null,
                    sUnit = f.sUnit,
                    sType = f.sType,
                    cTotal = f.cTotal,
                    cSetCode = f.cSetCode,
                    nGroupCalc = f.nGroupCalc,
                });
                result.objDataEmission.lstCEM.Add(new TData_Emission
                {
                    ProductID = f.ProductID,
                    ProductName = f.ProductName.Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 "),
                    UnitID = f.sType == "SUM" ? (int?)2 : null,
                    sUnit = f.sUnit,
                    sType = f.sType,
                    cTotal = f.cTotal,
                    cSetCode = f.cSetCode,
                    nGroupCalc = f.nGroupCalc,
                });
            }
        });
        lstmTProductIndicator.Where(w => w.sType == "VOC").OrderBy(o => o.nOrder).ToList().ForEach(f =>
        {
            result.lstVOC.Add(new TData_Emission
            {
                ProductID = f.ProductID,
                ProductName = f.ProductName.Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 "),
                sUnit = f.sUnit,
                sType = f.sType,
                nOption = f.nOption ?? null,
                cTotal = f.cTotal,
                nGroupCalc = f.nGroupCalc,
            });
        });
        db.mTProductIndicator.Where(w => w.IDIndicator == nIndicator && w.sType == "OTH").OrderBy(o => o.nOrder).ThenBy(o2 => o2.nGroupCalc).ToList().ForEach(f =>
        {
            result.lstAddAdditional.Add(new TData_Emission
            {
                ProductID = f.ProductID,
                ProductName = f.ProductName.Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 "),
                sUnit = f.sUnit,
                sType = f.sType,
                nOption = f.nOption ?? null,
                cTotal = f.cTotal,
                nGroupCalc = f.nGroupCalc,
            });
        });

        db.TM_Emission_OtherProduct.Where(w => w.cActive == "Y" && w.cDel == "N").ToList().ForEach(f =>
        {
            result.lstOtherPrd.Add(new TM_Emission_OtherProduct
            {
                nProductID = f.nProductID,
                sName = f.sName.Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 ")
            });
        });
        #endregion
        if (itemEPI_FORM != null)
        {
            IsNew = false;
            EPI_FORMID = itemEPI_FORM.FormID;

            var lstMark = db.TEmission_Stack.Where(w => w.FormID == EPI_FORMID).ToList();
            Func<int, string> GetRemarkInput = (nStackID) =>
            {
                string sremark = "";
                var q = lstMark.FirstOrDefault(w => w.nStackID == nStackID);
                sremark = q != null ? q.sRemark : "";
                return sremark;
            };
            var lstMarkVOC = db.TEmission_Remark.Where(w => w.FormID == itemEPI_FORM.FormID).OrderByDescending(o => o.nVersion).ToList();
            Func<int, string> GetRemarkInputVOC = (productid) =>
            {
                string sremark = "";
                var q = lstMarkVOC.FirstOrDefault(w => w.ProductID == productid);
                sremark = q != null ? q.sRemark : "";
                return sremark;
            };
            #region Combustion
            List<TEmission_Product> dataCombustion = new List<TEmission_Product>();
            dataCombustion = db.TEmission_Product.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
            result.objDataEmission.lstCombustion.ForEach(f =>
            {
                var item = dataCombustion.FirstOrDefault(w => w.ProductID == f.ProductID);
                if (item != null)
                {
                    f.M1 = item.M1;
                    f.M2 = item.M2;
                    f.M3 = item.M3;
                    f.M4 = item.M4;
                    f.M5 = item.M5;
                    f.M6 = item.M6;
                    f.M7 = item.M7;
                    f.M8 = item.M8;
                    f.M9 = item.M9;
                    f.M10 = item.M10;
                    f.M11 = item.M11;
                    f.M12 = item.M12;
                    f.Target = item.Target;

                    f.IsCheckM1 = item.IsCheckM1;
                    f.IsCheckM2 = item.IsCheckM2;
                    f.IsCheckM3 = item.IsCheckM3;
                    f.IsCheckM4 = item.IsCheckM4;
                    f.IsCheckM5 = item.IsCheckM5;
                    f.IsCheckM6 = item.IsCheckM6;
                    f.IsCheckM7 = item.IsCheckM7;
                    f.IsCheckM8 = item.IsCheckM8;
                    f.IsCheckM9 = item.IsCheckM9;
                    f.IsCheckM10 = item.IsCheckM10;
                    f.IsCheckM11 = item.IsCheckM11;
                    f.IsCheckM12 = item.IsCheckM12;
                    f.nTotal = item.nTotal;
                }
            });
            #endregion

            #region Non-Combustion
            var dataNonCombustion = db.TEmission_Product_ByType.Where(w => w.sType == "NCS" && w.FormID == itemEPI_FORM.FormID).ToList();
            result.objDataEmission.lstNonCombustion.ForEach(f =>
            {
                var item = dataNonCombustion.FirstOrDefault(w => w.ProductID == f.ProductID);
                if (item != null)
                {
                    f.M1 = item.M1;
                    f.M2 = item.M2;
                    f.M3 = item.M3;
                    f.M4 = item.M4;
                    f.M5 = item.M5;
                    f.M6 = item.M6;
                    f.M7 = item.M7;
                    f.M8 = item.M8;
                    f.M9 = item.M9;
                    f.M10 = item.M10;
                    f.M11 = item.M11;
                    f.M12 = item.M12;
                    f.Target = item.Target;
                    f.nTotal = item.nTotal;
                }
            });
            #endregion

            #region CEM
            var dataCEM = db.TEmission_Product_ByType.Where(w => w.sType == "CEM" && w.FormID == itemEPI_FORM.FormID).ToList();
            result.objDataEmission.lstCEM.ForEach(f =>
            {
                var item = dataCEM.FirstOrDefault(w => w.ProductID == f.ProductID);
                if (item != null)
                {
                    f.M1 = item.M1;
                    f.M2 = item.M2;
                    f.M3 = item.M3;
                    f.M4 = item.M4;
                    f.M5 = item.M5;
                    f.M6 = item.M6;
                    f.M7 = item.M7;
                    f.M8 = item.M8;
                    f.M9 = item.M9;
                    f.M10 = item.M10;
                    f.M11 = item.M11;
                    f.M12 = item.M12;
                    f.Target = item.Target;
                    f.nTotal = item.nTotal;
                }
            });
            #endregion

            #region Additional Combustion
            var dataAdddtionalCombustion = db.TEmission_Product_ByType.Where(w => w.sType == "ACS" && w.FormID == itemEPI_FORM.FormID).ToList();
            foreach (var item in dataAdddtionalCombustion)
            {
                var dataName = result.lstOtherPrd.FirstOrDefault(f => f.nProductID == item.ProductID);
                result.objDataEmission.lstAdditional.Add(new TData_Emission
                {
                    ProductID = item.ProductID,
                    ProductName = dataName != null ? dataName.sName : "",
                    sUnit = GetValueNameUnit(item.UnitID + ""),
                    nOtherProductID = item.ProductID,
                    UnitID = item.UnitID,
                    M1 = item.M1,
                    M2 = item.M2,
                    M3 = item.M3,
                    M4 = item.M4,
                    M5 = item.M5,
                    M6 = item.M6,
                    M7 = item.M7,
                    M8 = item.M8,
                    M9 = item.M9,
                    M10 = item.M10,
                    M11 = item.M11,
                    M12 = item.M12,
                    Target = item.Target,
                    sType = "OTH",
                    nTotal = item.nTotal,
                });
            }
            #endregion

            #region Additional Non-Combustion
            var dataAdddtionalNonCombustion = db.TEmission_Product_ByType.Where(w => w.sType == "ANC" && w.FormID == itemEPI_FORM.FormID).ToList();
            foreach (var item in dataAdddtionalNonCombustion)
            {
                var dataName = result.lstOtherPrd.FirstOrDefault(f => f.nProductID == item.ProductID);
                result.objDataEmission.lstAdditionalNonCombustion.Add(new TData_Emission
                {
                    ProductID = item.ProductID,
                    ProductName = dataName != null ? dataName.sName : "",
                    sUnit = GetValueNameUnit(item.UnitID + ""),
                    nOtherProductID = item.ProductID,
                    UnitID = item.UnitID,
                    M1 = item.M1,
                    M2 = item.M2,
                    M3 = item.M3,
                    M4 = item.M4,
                    M5 = item.M5,
                    M6 = item.M6,
                    M7 = item.M7,
                    M8 = item.M8,
                    M9 = item.M9,
                    M10 = item.M10,
                    M11 = item.M11,
                    M12 = item.M12,
                    Target = item.Target,
                    sType = "OTH",
                    nTotal = item.nTotal,
                });
            }
            result.objDataEmission.lstAdditionalNonCombustion = result.objDataEmission.lstAdditionalNonCombustion.OrderBy(o => o.ProductName).ToList();
            #endregion

            #region VOC
            var dataVoc = db.TEmission_VOC.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
            result.lstVOC.ForEach(f =>
            {
                var item = dataVoc.FirstOrDefault(w => w.ProductID == f.ProductID);
                if (item != null)
                {
                    f.M1 = item.M1;
                    f.M2 = item.M2;
                    f.M3 = item.M3;
                    f.M4 = item.M4;
                    f.M5 = item.M5;
                    f.M6 = item.M6;
                    f.M7 = item.M7;
                    f.M8 = item.M8;
                    f.M9 = item.M9;
                    f.M10 = item.M10;
                    f.M11 = item.M11;
                    f.M12 = item.M12;
                    f.Target = item.Target;
                    f.nTotal = item.nTotal;

                    f.IsCheckM1 = item.IsCheckM1;
                    f.IsCheckM2 = item.IsCheckM2;
                    f.IsCheckM3 = item.IsCheckM3;
                    f.IsCheckM4 = item.IsCheckM4;
                    f.IsCheckM5 = item.IsCheckM5;
                    f.IsCheckM6 = item.IsCheckM6;
                    f.IsCheckM7 = item.IsCheckM7;
                    f.IsCheckM8 = item.IsCheckM8;
                    f.IsCheckM9 = item.IsCheckM9;
                    f.IsCheckM10 = item.IsCheckM10;
                    f.IsCheckM11 = item.IsCheckM11;
                    f.IsCheckM12 = item.IsCheckM12;
                    f.sRemark = GetRemarkInput(f.ProductID);
                }
            });

            var dataRemark = db.TEmission_Remark.Where(w => w.FormID == itemEPI_FORM.FormID).OrderByDescending(o => o.nVersion).FirstOrDefault();
            if (dataRemark != null)
            {
                if (result.lstVOC.Count() > 0)
                {
                    result.lstVOC[0].sRemark = dataRemark.sRemark;
                }
                result.sRemarkVOC = dataRemark.sRemark;
            }
            #endregion

            #region Stack
            var dataStack = db.TEmission_Stack.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
            var dataLstStack = db.TEmission_Product_Stack.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
            var dataLstStackOther = db.TEmission_OtherProduct_Stack.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
            dataStack.ForEach(f =>
            {

                result.objDataEmission.lstStack.Add(new TData_Stack
                {
                    nStackID = f.nStackID,
                    sStackName = f.sStackName,
                    sStackType = f.sStackType,
                    sRemark = f.sRemark,
                    IsSaved = true,
                    lstDataStack = lstmTProductIndicator.Where(w => w.sType == "2" || w.sType == "2H").OrderBy(o => o.nOrder).ThenBy(o2 => o2.nGroupCalc).Select(s => new TData_Emission
                        {
                            ProductID = s.ProductID,
                            ProductName = s.ProductName.Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 "),
                            sUnit = s.sUnit,
                            sType = s.sType,
                            nOption = s.nOption ?? null,
                            cTotal = s.cTotal,
                            nGroupCalc = s.nGroupCalc,
                            cSetCode = s.cSetCode,
                            M1 = "",
                            M2 = "",
                            M3 = "",
                            M4 = "",
                            M5 = "",
                            M6 = "",
                            M7 = "",
                            M8 = "",
                            M9 = "",
                            M10 = "",
                            M11 = "",
                            M12 = "",
                            Target = "",

                            IsCheckM1 = "N",
                            IsCheckM2 = "N",
                            IsCheckM3 = "N",
                            IsCheckM4 = "N",
                            IsCheckM5 = "N",
                            IsCheckM6 = "N",
                            IsCheckM7 = "N",
                            IsCheckM8 = "N",
                            IsCheckM9 = "N",
                            IsCheckM10 = "N",
                            IsCheckM11 = "N",
                            IsCheckM12 = "N",
                        }).ToList(),
                    lstDataStackCEM = lstmTProductIndicator.Where(w => w.sType == "CEM" || w.sType == "2H").OrderBy(o => o.nOrder).ThenBy(o2 => o2.nGroupCalc).Select(s => new TData_Emission
                    {
                        ProductID = s.ProductID,
                        ProductName = s.ProductName.Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 "),
                        sUnit = s.sUnit,
                        sType = s.sType,
                        nOption = s.nOption ?? null,
                        cTotal = s.cTotal,
                        nGroupCalc = s.nGroupCalc,
                        cSetCode = s.cSetCode,
                        M1 = "",
                        M2 = "",
                        M3 = "",
                        M4 = "",
                        M5 = "",
                        M6 = "",
                        M7 = "",
                        M8 = "",
                        M9 = "",
                        M10 = "",
                        M11 = "",
                        M12 = "",
                        Target = "",

                        IsCheckM1 = "N",
                        IsCheckM2 = "N",
                        IsCheckM3 = "N",
                        IsCheckM4 = "N",
                        IsCheckM5 = "N",
                        IsCheckM6 = "N",
                        IsCheckM7 = "N",
                        IsCheckM8 = "N",
                        IsCheckM9 = "N",
                        IsCheckM10 = "N",
                        IsCheckM11 = "N",
                        IsCheckM12 = "N",
                    }).ToList(),

                });
            });
            result.objDataEmission.lstStack.ForEach(f =>
            {
                foreach (var item in f.lstDataStack)
                {
                    item.sStackName = f.sStackName;
                    item.nStackID = f.nStackID;
                    var data = dataLstStack.FirstOrDefault(w => w.nStackID == f.nStackID && w.ProductID == item.ProductID);
                    if (data != null)
                    {
                        item.M1 = data.M1;
                        item.M2 = data.M2;
                        item.M3 = data.M3;
                        item.M4 = data.M4;
                        item.M5 = data.M5;
                        item.M6 = data.M6;
                        item.M7 = data.M7;
                        item.M8 = data.M8;
                        item.M9 = data.M9;
                        item.M10 = data.M10;
                        item.M11 = data.M11;
                        item.M12 = data.M12;
                        item.Target = data.Target;
                        item.nOptionProduct = data.nOptionProduct;
                        item.IsSaved = true;
                    }
                }
                foreach (var item in f.lstDataStackCEM)
                {
                    item.sStackName = f.sStackName;
                    item.nStackID = f.nStackID;
                    var data = dataLstStack.FirstOrDefault(w => w.nStackID == f.nStackID && w.ProductID == item.ProductID && w.nOptionProduct == 3);
                    if (data != null)
                    {
                        item.M1 = data.M1;
                        item.M2 = data.M2;
                        item.M3 = data.M3;
                        item.M4 = data.M4;
                        item.M5 = data.M5;
                        item.M6 = data.M6;
                        item.M7 = data.M7;
                        item.M8 = data.M8;
                        item.M9 = data.M9;
                        item.M10 = data.M10;
                        item.M11 = data.M11;
                        item.M12 = data.M12;
                        item.Target = data.Target;
                        item.nOptionProduct = data.nOptionProduct;
                        item.IsSaved = true;
                        item.IsSubmited = true;
                    }
                }
                var dataLstOther = result.lstAddAdditional;
                var dataOther = (from v in dataLstStackOther.Where(w => w.nStackID == f.nStackID).ToList()
                                 from d in dataLstOther.Where(w => w.ProductID == v.ProductID).ToList()
                                 select new TData_Emission
                                 {
                                     sStackName = f.sStackName,
                                     ProductID = d.ProductID,
                                     ProductName = d.ProductName.Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 "),
                                     sUnit = d.sUnit,
                                     sType = d.sType,
                                     nOption = d.nOption ?? null,
                                     cTotal = d.cTotal,
                                     nGroupCalc = d.nGroupCalc,
                                     nStackID = f.nStackID,
                                     nID = v.nID,
                                     M1 = v.M1,
                                     M2 = v.M2,
                                     M3 = v.M3,
                                     M4 = v.M4,
                                     M5 = v.M5,
                                     M6 = v.M6,
                                     M7 = v.M7,
                                     M8 = v.M8,
                                     M9 = v.M9,
                                     M10 = v.M10,
                                     M11 = v.M11,
                                     M12 = v.M12,
                                     Target = v.Target,
                                     nOptionProduct = v.nOptionProduct,
                                     nOtherProductID = v.nOtherProductID,
                                     cIsVOCs = v.cIsVOCs,
                                     IsSaved = true,
                                 }).ToList();
                if (dataOther.Count() > 0)
                {
                    var lstID = dataOther.Select(s => new
                    {
                        s.nID,
                        s.nOtherProductID
                    }).Distinct().ToList();
                    foreach (var item in lstID)
                    {
                        var dataOtherProduct = result.lstOtherPrd.FirstOrDefault(w => w.nProductID == item.nOtherProductID);
                        if (dataOtherProduct != null)
                        {
                            var nIndex = dataOther.IndexOf(dataOther.FirstOrDefault(w => w.nID == item.nID));
                            dataOther.Insert(nIndex, new TData_Emission { sStackName = f.sStackName, ProductName = dataOtherProduct.sName, sType = "2H" });
                        }
                    }
                    f.lstDataStack.AddRange(dataOther);
                }
                if (f.lstDataStack.Count() > 0)
                {
                    f.lstDataStack[0].sRemark = GetRemarkInput(f.nStackID);
                }
            });
            #endregion


        }
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
        string[] arrColor = new string[2] { "#dbea97", "#e7bb5b" };

        List<List<TData_Emission>> LoopBindData = new List<List<TData_Emission>>();
        List<string> lstHead = new List<string> { "Combustion", "Non-Combustion", "CEM", "Additional Combustion", "Additional Non-Combustion", "VOC" };
        if (nOprtID != 13)
        {
            LoopBindData.Add(result.objDataEmission.lstCombustion);
            LoopBindData.Add(result.objDataEmission.lstNonCombustion);
            LoopBindData.Add(result.objDataEmission.lstCEM);
            LoopBindData.Add(result.objDataEmission.lstAdditional);
            LoopBindData.Add(result.objDataEmission.lstAdditionalNonCombustion);
        }
        else
        {
            lstHead.RemoveRange(0, 5);
        }
        LoopBindData.Add(result.lstVOC);
        SetTbl(ws1, "Indicator : " + sIncName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        nRow++;
        SetTbl(ws1, "Operation : " + sOprtName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        nRow++;
        SetTbl(ws1, "Facility : " + sFacName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        nRow++;
        SetTbl(ws1, "Year : " + sYear, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
        nRow++;

        for (int index = 0; index < LoopBindData.Count(); index++)
        {
            nCol = 1;
            nRow++;
            ws1.Range(nRow, nCol, nRow, nCol + 16).Merge();
            SetTbl(ws1, lstHead[index], nRow, nCol, 14, true, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            ws1.Range(nRow, 1, nRow, nCol).Style.Fill.BackgroundColor = XLColor.FromHtml("#fdb813");
            ws1.Range(nRow, 1, nRow, nCol + 16).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws1.Range(nRow, 1, nRow, nCol + 16).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            nRow++;
            SetTbl(ws1, "Indicator", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 45);
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
            SetTbl(ws1, "Remark", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, 20);
            ws1.Range(nRow, 1, nRow, nCol).Style.Fill.BackgroundColor = XLColor.FromHtml("#9cb726");
            ws1.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws1.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            if (LoopBindData[index].Count() > 0)
            {
                foreach (var item in LoopBindData[index])
                {
                    nRow++;
                    nCol = 1;
                    string[] arrDataInMonth = new string[12] { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                    SetTbl(ws1, "'" + (item.sType == "SUM" ? item.ProductName : (item.sType == "SUM2" || item.UnitID == 68 ? "" : item.ProductName)), nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
                    nCol++;
                    SetTbl(ws1, "'" + item.sUnit, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null);
                    nCol++;
                    SetTbl(ws1, item.Target, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
                    nCol++;
                    for (int i = 0; i < 12; i++)
                    {
                        SetTbl(ws1, arrDataInMonth[i], nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
                        nCol++;
                    }
                    SetTbl(ws1, item.nTotal, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
                    nCol++;
                    SetTbl(ws1, "'" + item.sRemark, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
                    ws1.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    ws1.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
            }
            nRow = nRow + 2;
        }
        #endregion

        if (nOprtID != 13)
        {
            #region BIND DATA STACK
            IXLWorksheet wsStack = wb.Worksheets.Add("Stack");
            wsStack.PageSetup.Margins.Top = 0.2;
            wsStack.PageSetup.Margins.Bottom = 0.2;
            wsStack.PageSetup.Margins.Left = 0.1;
            wsStack.PageSetup.Margins.Right = 0;
            wsStack.PageSetup.Margins.Footer = 0;
            wsStack.PageSetup.Margins.Header = 0;
            wsStack.Style.Font.FontName = "Cordia New";

            nRow = 1;
            nCol = 1;

            SetTbl(wsStack, "Indicator : " + sIncName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nRow++;
            SetTbl(wsStack, "Operation : " + sOprtName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nRow++;
            SetTbl(wsStack, "Facility : " + sFacName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nRow++;
            SetTbl(wsStack, "Year : " + sYear, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nRow++;

            SetTbl(wsStack, "Stack Name ", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 40);
            nCol++;
            SetTbl(wsStack, "Indicator", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 45);
            nCol++;
            SetTbl(wsStack, "Unit", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 10.5);
            nCol++;
            SetTbl(wsStack, "Target", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17);
            nCol++;
            for (int i = 0; i < 12; i++)
            {
                SetTbl(wsStack, arrShortMonth[i], nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 17);
                nCol++;
            }
            SetTbl(wsStack, "Total", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 15);
            nCol++;
            SetTbl(wsStack, "Remark", nRow, nCol, 14, true, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, 20);
            wsStack.Range(nRow, 1, nRow, nCol).Style.Fill.BackgroundColor = XLColor.FromHtml("#9cb726");
            wsStack.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            wsStack.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            foreach (var stack in result.objDataEmission.lstStack)
            {
                foreach (var item in stack.lstDataStack)
                {
                    nRow++;
                    nCol = 1;
                    string[] arrDataInMonth = new string[12] { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                    SetTbl(wsStack, "'" + item.sStackName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
                    nCol++;
                    SetTbl(wsStack, "'" + item.ProductName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
                    nCol++;
                    SetTbl(wsStack, "'" + item.sUnit, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null);
                    nCol++;
                    SetTbl(wsStack, item.Target, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
                    nCol++;
                    for (int i = 0; i < 12; i++)
                    {
                        SetTbl(wsStack, arrDataInMonth[i], nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
                        nCol++;
                    }
                    SetTbl(wsStack, item.nTotal, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
                    nCol++;
                    SetTbl(wsStack, "'" + item.sRemark, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
                    if (item.nGroupCalc == 7 || item.sType == "2H")
                    {
                        wsStack.Range(nRow, 1, nRow, nCol).Style.Fill.BackgroundColor = XLColor.FromHtml(item.nGroupCalc == 7 ? (arrColor[0]) : (item.sType == "2H" ? arrColor[1] : ""));
                    }
                    wsStack.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    wsStack.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                if (stack.sStackType == "CMS")
                {
                    foreach (var item in stack.lstDataStackCEM)
                    {
                        nRow++;
                        nCol = 1;
                        string[] arrDataInMonth = new string[12] { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                        SetTbl(wsStack, "'" + item.sStackName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
                        nCol++;
                        SetTbl(wsStack, "'" + item.ProductName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
                        nCol++;
                        SetTbl(wsStack, "'" + item.sUnit, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Center, true, null, null);
                        nCol++;
                        SetTbl(wsStack, item.Target, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
                        nCol++;
                        for (int i = 0; i < 12; i++)
                        {
                            SetTbl(wsStack, arrDataInMonth[i], nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
                            nCol++;
                        }
                        SetTbl(wsStack, item.nTotal, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
                        nCol++;
                        SetTbl(wsStack, "'" + item.sRemark, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
                        if (item.nGroupCalc == 7 || item.sType == "2H")
                        {
                            wsStack.Range(nRow, 1, nRow, nCol).Style.Fill.BackgroundColor = XLColor.FromHtml(item.nGroupCalc == 7 ? (arrColor[0]) : (item.sType == "2H" ? arrColor[1] : ""));
                        }
                        wsStack.Range(nRow, 1, nRow, nCol).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        wsStack.Range(nRow, 1, nRow, nCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }
                }
            }

            #endregion
        }
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

    #region Function For Calululate
    private static string GetValueFromListForAdditional(List<TData_Emission> lstData, int? nStackID, int nMonth, int? nOtherProductID, int nOptionProduct, int? nID)
    {
        string sResult = "";
        nStackID = nStackID ?? 0;
        var query = lstData.Where(w => w.nStackID == nStackID && w.nOtherProductID == nOtherProductID && w.nOption == nOptionProduct && w.nID == nID).FirstOrDefault();
        if (query != null)
        {
            switch (nMonth)
            {
                case 1: sResult = query.M1; break;
                case 2: sResult = query.M2; break;
                case 3: sResult = query.M3; break;
                case 4: sResult = query.M4; break;
                case 5: sResult = query.M5; break;
                case 6: sResult = query.M6; break;
                case 7: sResult = query.M7; break;
                case 8: sResult = query.M8; break;
                case 9: sResult = query.M9; break;
                case 10: sResult = query.M10; break;
                case 11: sResult = query.M11; break;
                case 12: sResult = query.M12; break;

            }
        }
        return sResult;
    }
    private static string GetValueFromListForGrumPerSec(List<TData_Emission> lstData, int nMonth, int? nOtherProductID)
    {
        string sResult = "";
        var query = lstData.Where(w => w.nOtherProductID == nOtherProductID).FirstOrDefault();//Reference by group calculate
        if (query != null)
        {
            switch (nMonth)
            {
                case 1: sResult = query.M1; break;
                case 2: sResult = query.M2; break;
                case 3: sResult = query.M3; break;
                case 4: sResult = query.M4; break;
                case 5: sResult = query.M5; break;
                case 6: sResult = query.M6; break;
                case 7: sResult = query.M7; break;
                case 8: sResult = query.M8; break;
                case 9: sResult = query.M9; break;
                case 10: sResult = query.M10; break;
                case 11: sResult = query.M11; break;
                case 12: sResult = query.M12; break;

            }
        }
        return sResult;
    }
    private static string GetValueFromListForCEM(List<TData_Emission> lstData, int? nStackID, int nMonth, string sSetCodeH, int nOption, List<int> lstCEMID)
    {
        string sGetSetCode = sSetCodeH + "-" + nOption;
        string sResult = "";
        nStackID = nStackID ?? 0;
        var query = lstData.Where(w => w.nStackID == nStackID && w.cSetCode == sGetSetCode && lstCEMID.Contains(w.ProductID)).ToList();
        if (query != null)
        {
            switch (nMonth)
            {
                case 1: sResult = SystemFunction.SumDataToDecimal(query.Select(s => s.M1).ToList()) + ""; break;
                case 2: sResult = SystemFunction.SumDataToDecimal(query.Select(s => s.M2).ToList()) + ""; break;
                case 3: sResult = SystemFunction.SumDataToDecimal(query.Select(s => s.M3).ToList()) + ""; break;
                case 4: sResult = SystemFunction.SumDataToDecimal(query.Select(s => s.M4).ToList()) + ""; break;
                case 5: sResult = SystemFunction.SumDataToDecimal(query.Select(s => s.M5).ToList()) + ""; break;
                case 6: sResult = SystemFunction.SumDataToDecimal(query.Select(s => s.M6).ToList()) + ""; break;
                case 7: sResult = SystemFunction.SumDataToDecimal(query.Select(s => s.M7).ToList()) + ""; break;
                case 8: sResult = SystemFunction.SumDataToDecimal(query.Select(s => s.M8).ToList()) + ""; break;
                case 9: sResult = SystemFunction.SumDataToDecimal(query.Select(s => s.M9).ToList()) + ""; break;
                case 10: sResult = SystemFunction.SumDataToDecimal(query.Select(s => s.M10).ToList()) + ""; break;
                case 11: sResult = SystemFunction.SumDataToDecimal(query.Select(s => s.M11).ToList()) + ""; break;
                case 12: sResult = SystemFunction.SumDataToDecimal(query.Select(s => s.M12).ToList()) + ""; break;

            }
        }
        return sResult;
    }
    private static string GetValueFromList(List<TData_Emission> lstData, int? nStackID, int nMonth, string sSetCodeH, int nOption)
    {
        string sGetSetCode = sSetCodeH + "-" + nOption;
        string sResult = "";
        nStackID = nStackID ?? 0;
        var query = lstData.Where(w => w.nStackID == nStackID && w.cSetCode == sGetSetCode).FirstOrDefault();
        if (query != null)
        {
            switch (nMonth)
            {
                case 1: sResult = query.M1; break;
                case 2: sResult = query.M2; break;
                case 3: sResult = query.M3; break;
                case 4: sResult = query.M4; break;
                case 5: sResult = query.M5; break;
                case 6: sResult = query.M6; break;
                case 7: sResult = query.M7; break;
                case 8: sResult = query.M8; break;
                case 9: sResult = query.M9; break;
                case 10: sResult = query.M10; break;
                case 11: sResult = query.M11; break;
                case 12: sResult = query.M12; break;

            }
        }
        return sResult;
    }
    private static string GetValueFromList(List<TData_Emission> lstData, int? nStackID, int nMonth, int nProductID)
    {
        string sResult = "";
        nStackID = nStackID ?? 0;
        var query = lstData.Where(w => w.nStackID == nStackID && w.ProductID == nProductID).FirstOrDefault();
        if (query != null)
        {
            switch (nMonth)
            {
                case 1: sResult = query.M1; break;
                case 2: sResult = query.M2; break;
                case 3: sResult = query.M3; break;
                case 4: sResult = query.M4; break;
                case 5: sResult = query.M5; break;
                case 6: sResult = query.M6; break;
                case 7: sResult = query.M7; break;
                case 8: sResult = query.M8; break;
                case 9: sResult = query.M9; break;
                case 10: sResult = query.M10; break;
                case 11: sResult = query.M11; break;
                case 12: sResult = query.M12; break;

            }
        }
        return sResult;
    }

    private static string GetValueFromList(List<TData_Emission> lstData, int nMonth, int? nGroupCal)
    {
        string sResult = "";
        nGroupCal = nGroupCal ?? 0;
        var query = lstData.Where(w => w.nGroupCalc == nGroupCal).FirstOrDefault();//Reference by group calculate
        if (query != null)
        {
            switch (nMonth)
            {
                case 1: sResult = query.M1; break;
                case 2: sResult = query.M2; break;
                case 3: sResult = query.M3; break;
                case 4: sResult = query.M4; break;
                case 5: sResult = query.M5; break;
                case 6: sResult = query.M6; break;
                case 7: sResult = query.M7; break;
                case 8: sResult = query.M8; break;
                case 9: sResult = query.M9; break;
                case 10: sResult = query.M10; break;
                case 11: sResult = query.M11; break;
                case 12: sResult = query.M12; break;

            }
        }
        return sResult;
    }

    #endregion

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
        public ObjData objDataEmission { get; set; }
        public List<TData_Emission> lstVOC { get; set; }
        public List<TData_Emission> lstAddStack { get; set; }
        public List<TData_Emission> lstAddStackCEM { get; set; }
        public List<TData_Emission> lstAddAdditional { get; set; }
        public List<TM_Emission_OtherProduct> lstOtherPrd { get; set; }
        public List<sysGlobalClass.FuncFileUpload.ItemData> lstFile { get; set; }
        public List<sysGlobalClass.T_TEPI_Workflow> lstStatus { get; set; }
        public string sRemarkVOC { get; set; }
        public string hdfPRMS { get; set; }
    }
    public class ObjData
    {
        public List<TData_Emission> lstCombustion { get; set; }
        public List<TData_Emission> lstNonCombustion { get; set; }
        public List<TData_Emission> lstCEM { get; set; }
        public List<TData_Emission> lstAdditional { get; set; }
        public List<TData_Emission> lstAdditionalNonCombustion { get; set; }
        public List<TData_Stack> lstStack { get; set; }
    }
    public class TData_Emission : TEmission_Product
    {
        public string ProductName { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }
        public int? nOption { get; set; }
        public string cTotal { get; set; }
        public int? nGroupCalc { get; set; }
        public int? nOtherProductID { get; set; }
        public int? nID { get; set; }
        public string cIsVOCs { get; set; }
        public bool? IsSaved { get; set; }
        public bool? IsSubmited { get; set; }
        public int? nOptionProduct { get; set; }
        public bool? IsDel { get; set; }
        public int? nStackID { get; set; }
        public string sStackName { get; set; }
        public string cSetCode { get; set; }
        public string sRemark { get; set; }
        public string sOption { get; set; }
    }
    public class TData_Stack : TEmission_Stack
    {
        public bool? IsSaved { get; set; }
        public bool? IsSubmited { get; set; }
        public List<TData_Emission> lstDataStack { get; set; }
        public List<TData_Emission> lstDataStackCEM { get; set; }
    }
    public class cDataForSave
    {
        public int nIndicatorID { get; set; }
        public int nOperationID { get; set; }
        public int nFacilityID { get; set; }
        public string sYear { get; set; }
        public List<TData_Stack> lstStack { get; set; }
        public List<TData_Emission> lstDataCombustion { get; set; }
        public List<TData_Emission> lstDataNonCombustion { get; set; }
        public List<TData_Emission> lstDataCEM { get; set; }
        public List<TData_Emission> lstDataAdditional { get; set; }
        public List<TData_Emission> lstDataAdditionalNonCombustion { get; set; }
        public ObjVOC objVOC { get; set; }
        public List<sysGlobalClass.FuncFileUpload.ItemData> lstFile { get; set; }
        public List<int> lstMonthSubmit { get; set; }
        public int nStatus { get; set; }
        public string sRemarkRequestEdit { get; set; }
    }
    public class ObjVOC
    {
        public List<TData_Emission> lstVOC { get; set; }
        public string sRemarkVOC { get; set; }
    }
    #endregion
}