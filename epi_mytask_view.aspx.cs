using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class epi_mytask_view : System.Web.UI.Page
{

    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnLoad(myFunc);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetBodyEventOnLoad("");
        if (UserAcc.UserExpired())
        {
            SetBodyEventOnLoad(SystemFunction.PopupLogin());
        }
        else
        {
            if (!IsPostBack)
            {
                PTTGC_EPIEntities env = new PTTGC_EPIEntities();
                string sFormID = Request.QueryString["strid"]; // Form ID
                string sLevel = Request.QueryString["strlevel"]; // Level (L1 = 3,L2 = 4 ,L0 = 2) After Decrypt
                int nFormID = 0; int.TryParse(STCrypt.Decrypt(sFormID), out nFormID);
                var gDataForm = env.TEPI_Forms.FirstOrDefault(w => w.FormID == nFormID);
                if (!string.IsNullOrEmpty(sFormID))
                {
                    if (gDataForm != null)
                    {
                        hidFacility.Value = gDataForm.FacilityID.ToString();
                        hidIndicator.Value = gDataForm.IDIndicator.ToString();
                        hidOperationType.Value = gDataForm.OperationTypeID.ToString();
                        hidYear.Value = gDataForm.sYear;

                        var qOpera = env.mOperationType.FirstOrDefault(w => w.ID == gDataForm.OperationTypeID);
                        var qFac = env.mTFacility.FirstOrDefault(w => w.ID == gDataForm.FacilityID);
                        var qGroupInd = env.mTIndicator.FirstOrDefault(w => w.ID == gDataForm.IDIndicator);
                        lblGroupIndicator.Text = qGroupInd != null ? qGroupInd.Indicator : "";
                        lblOperationtype.Text = qOpera.Name;
                        lblFacility.Text = qFac != null ? qFac.Name : "";
                        lblYear.Text = gDataForm.sYear;
                    }

                    hidFormID.Value = STCrypt.Decrypt(sFormID);
                    if (!string.IsNullOrEmpty(sLevel))
                    {
                        hidLevel.Value = STCrypt.Decrypt(sLevel);
                    }
                }
                else
                {
                    Response.Redirect("epi_mytask.aspx");
                }
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveAction(List<int> lstMonth, int FormID, string sComment, string sMode)
    {
        sysGlobalClass.CResutlWebMethod r = new sysGlobalClass.CResutlWebMethod();

        if (!UserAcc.UserExpired())
        {
            int nRoleID = UserAcc.GetObjUser().nRoleID;
            int nUserID = UserAcc.GetObjUser().nUserID;
            if (nRoleID == 3)
            {
                r = new Workflow().WorkFlowAction(FormID, lstMonth, sMode, nUserID, nRoleID, sComment);
            }// L1
            else if (nRoleID == 2)
            {

            }//
            else if (nRoleID == 4)
            {
                r = new Workflow().WorkFlowAction(FormID, lstMonth, sMode, nUserID, nRoleID, sComment);
            }//L2

        }
        else
        {
            r.Status = SystemFunction.process_SessionExpired;
            r.Msg = "";
        }

        //r = new Workflow().WorkFlowAction();
        return r;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod ApproveWithEditContent(int FormID)
    {
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        sysGlobalClass.CResutlWebMethod r = new sysGlobalClass.CResutlWebMethod();
        int nOperationType = 0;
        if (!UserAcc.UserExpired())
        {
            var gData = env.TEPI_Forms.FirstOrDefault(f => f.FormID == FormID);
            if (gData != null)
            {
                r.Msg = SystemFunction.ReturnPath(gData.IDIndicator, gData.OperationTypeID, gData.FacilityID.ToString(), gData.sYear, "27");
                r.Status = SystemFunction.process_Success;
            }
            else
            {
                r.Msg = "";
                r.Status = SystemFunction.process_Failed;
            }
        }
        else
        {
            r.Msg = "";
            r.Status = SystemFunction.process_SessionExpired;
        }
        return r;
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static ResultData LoadData(TSearch item)
    {
        ResultData r = new ResultData();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataOutput> lstDataT1 = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstData = new List<ClassExecute.TDataOutput>();

        int nFormID = 0;

        var qForm = db.TEPI_Forms.FirstOrDefault(w => w.IDIndicator == item.nIndicator && w.OperationTypeID == item.nOperationType && w.FacilityID == item.nFacility && w.sYear == item.sYear);
        if (qForm != null)
        {
            nFormID = qForm.FormID;
        }

        if (item.nIndicator == 10)
        {
            lstData = FunctionGetData.GetWasteDataOutput(nFormID, item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
        }
        else if (item.nIndicator == 8)
        {
            lstData = FunctionGetData.GetMaterialDataOutput(nFormID, item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
        }
        else if (item.nIndicator == 6)
        {
            lstData = FunctionGetData.GetIntensityDataOutput(nFormID, item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
        }
        else if (item.nIndicator == 11)
        {
            lstData = FunctionGetData.GetWaterDataOutput(nFormID, item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
        }
        else if (item.nIndicator == 1)
        {
            lstData = FunctionGetData.GetComplaintDataOutput(nFormID, item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
        }
        else if (item.nIndicator == 2)
        {
            lstData = FunctionGetData.GetComplianceDataOutput(nFormID, item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
        }
        else if (item.nIndicator == 3)
        {
            lstData = FunctionGetData.GetDataOutput(item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
        }
        else if (item.nIndicator == 4)
        {
            lstData = FunctionGetData.GetDataOutput(item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
            lstDataOutputEmission(item.nIndicator, item.nOperationType, item.nFacility, item.sYear, r);
        }
        else if (item.nIndicator == 9)
        {
            lstData = FunctionGetData.GetDataOutput(item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
        }

        r.lstData = lstData;
        r.sFormID = nFormID + "";
        r.Status = SystemFunction.process_Success;

        return r;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static Data_Month GetMonth(int FormID, int nLevel)
    {
        Data_Month r = new Data_Month();
        r.lstShowButton = new List<string>();
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        var gMonth = env.TEPI_Workflow.Where(w => w.FormID == FormID).ToList();
        if (!UserAcc.UserExpired())
        {
            int[] lstStatusL1 = { 1 };
            int[] lstStatusL2 = { 4, 2 };

            if (gMonth.Any())
            {
                if (nLevel == 3)
                {
                    r.lstMonth = gMonth.Where(w => lstStatusL1.Contains(w.nStatusID ?? 0)).Select(s => s.nMonth).ToList();
                    if (r.lstMonth.Any())
                    {
                        r.lstShowButton.Add("btnAppr");
                        r.lstShowButton.Add("btnReject");
                    }
                }// L1
                else if (nLevel == 4)
                {
                    r.lstMonth = gMonth.Where(w => lstStatusL2.Contains(w.nStatusID ?? 0)).Select(s => s.nMonth).ToList();
                    if (r.lstMonth.Any())
                    {
                        r.lstShowButton.Add("btnAppr");
                        r.lstShowButton.Add("btnReject");
                    }

                    if (gMonth.Any(w => w.nStatusID == 2))
                    {
                        r.lstShowButton.Add("btnAppr_with");
                        r.lstShowButton.Add("btnAppr_Re");
                    }

                }//L2
                r.Msg = "";
                r.Status = SystemFunction.process_Success;
            }
            else
            {
                r.Msg = "No Data";
                r.Status = SystemFunction.process_Failed;
            }
        }
        else
        {
            r.Msg = "";
            r.Status = SystemFunction.process_SessionExpired;
        }
        return r;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static CReturnDataDeviate GetDataDeviate(CForGetDataDeviate lstSearch)
    {
        CReturnDataDeviate result = new CReturnDataDeviate();
        result.lstMonth = new List<CDataDeviate>();
        if (!UserAcc.UserExpired())
        {
            result.lstMonth = SystemFunction.GetDeviate(lstSearch.nIncID, lstSearch.nOprtID, lstSearch.nFacID, lstSearch.sYear).Where(w => lstSearch.lstMonth.Contains(w.nMonth) && w.sRemark.Trim() != "").Select(s => new CDataDeviate
            {
                nMonth = s.nMonth,
                sRemark = s.sRemark.Replace("\n", "<br/>")
            }).ToList();
            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    public static void lstDataOutputEmission(int nIDIndicator, int OperationType, int nFacility, string sYear, ResultData resultData)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();

        CReturnData result = new CReturnData();
        result.objDataEmission = new ObjData();
        result.lstAddStack = new List<TData_Emission>();
        result.objDataEmission.lstNonCombustion = new List<TData_Emission>();
        result.objDataEmission.lstCEM = new List<TData_Emission>();
        result.objDataEmission.lstAdditional = new List<TData_Emission>();
        result.objDataEmission.lstAdditionalNonCombustion = new List<TData_Emission>();
        result.objDataEmission.lstStack = new List<TData_Stack>();
        result.lstAddAdditional = new List<TData_Emission>();
        result.lstOtherPrd = new List<TM_Emission_OtherProduct>();
        result.lstVOC = new List<TData_Emission>();
        Func<string, string> GetValueNameMasterData = (sIDMaster) =>
        {
            string sMasterName = "";
            int nIDMaster = int.Parse(sIDMaster);
            var sDataMasterType = db.TData_Type.FirstOrDefault(w => w.cActive == "Y" && w.IndicatorID == nIDIndicator && w.nID == nIDMaster);
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
        var lstTProduct = db.mTProductIndicator.Where(w => w.IDIndicator == nIDIndicator).ToList();
        var lstUseProduct = db.TEmissionUseProduct.Where(w => w.OperationTypeID == OperationType).Select(s => new
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
                    nTotal = "",

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
                    nTotal = "",

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
                nTotal = "",
            });

        });
        db.mTProductIndicator.Where(w => w.IDIndicator == nIDIndicator && w.sType == "OTH").OrderBy(o => o.nOrder).ThenBy(o2 => o2.nGroupCalc).ToList().ForEach(f =>
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
                nTotal = "",
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
        var itemEPI_FORM = db.TEPI_Forms.FirstOrDefault(w => w.sYear == sYear && w.IDIndicator == nIDIndicator && w.OperationTypeID == OperationType && w.FacilityID == nFacility);
        if (itemEPI_FORM != null)
        {
            #region Non-Combustion
            var dataNonCombustion = db.TEmission_Product_ByType.Where(w => w.sType == "NCS" && w.FormID == itemEPI_FORM.FormID).ToList();
            result.objDataEmission.lstNonCombustion.ForEach(f =>
            {
                var item = dataNonCombustion.FirstOrDefault(w => w.ProductID == f.ProductID);
                if (item != null)
                {
                    f.M1 = SystemFunction.ConvertFormatDecimal4(item.M1);
                    f.M2 = SystemFunction.ConvertFormatDecimal4(item.M2);
                    f.M3 = SystemFunction.ConvertFormatDecimal4(item.M3);
                    f.M4 = SystemFunction.ConvertFormatDecimal4(item.M4);
                    f.M5 = SystemFunction.ConvertFormatDecimal4(item.M5);
                    f.M6 = SystemFunction.ConvertFormatDecimal4(item.M6);
                    f.M7 = SystemFunction.ConvertFormatDecimal4(item.M7);
                    f.M8 = SystemFunction.ConvertFormatDecimal4(item.M8);
                    f.M9 = SystemFunction.ConvertFormatDecimal4(item.M9);
                    f.M10 = SystemFunction.ConvertFormatDecimal4(item.M10);
                    f.M11 = SystemFunction.ConvertFormatDecimal4(item.M11);
                    f.M12 = SystemFunction.ConvertFormatDecimal4(item.M12);
                    f.Target = SystemFunction.ConvertFormatDecimal4(item.Target);
                    f.nTotal = SystemFunction.ConvertFormatDecimal4(item.nTotal);
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
                    f.M1 = SystemFunction.ConvertFormatDecimal4(item.M1);
                    f.M2 = SystemFunction.ConvertFormatDecimal4(item.M2);
                    f.M3 = SystemFunction.ConvertFormatDecimal4(item.M3);
                    f.M4 = SystemFunction.ConvertFormatDecimal4(item.M4);
                    f.M5 = SystemFunction.ConvertFormatDecimal4(item.M5);
                    f.M6 = SystemFunction.ConvertFormatDecimal4(item.M6);
                    f.M7 = SystemFunction.ConvertFormatDecimal4(item.M7);
                    f.M8 = SystemFunction.ConvertFormatDecimal4(item.M8);
                    f.M9 = SystemFunction.ConvertFormatDecimal4(item.M9);
                    f.M10 = SystemFunction.ConvertFormatDecimal4(item.M10);
                    f.M11 = SystemFunction.ConvertFormatDecimal4(item.M11);
                    f.M12 = SystemFunction.ConvertFormatDecimal4(item.M12);
                    f.Target = SystemFunction.ConvertFormatDecimal4(item.Target);
                    f.nTotal = SystemFunction.ConvertFormatDecimal4(item.nTotal);
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
                    M1 = SystemFunction.ConvertFormatDecimal4(item.M1),
                    M2 = SystemFunction.ConvertFormatDecimal4(item.M2),
                    M3 = SystemFunction.ConvertFormatDecimal4(item.M3),
                    M4 = SystemFunction.ConvertFormatDecimal4(item.M4),
                    M5 = SystemFunction.ConvertFormatDecimal4(item.M5),
                    M6 = SystemFunction.ConvertFormatDecimal4(item.M6),
                    M7 = SystemFunction.ConvertFormatDecimal4(item.M7),
                    M8 = SystemFunction.ConvertFormatDecimal4(item.M8),
                    M9 = SystemFunction.ConvertFormatDecimal4(item.M9),
                    M10 = SystemFunction.ConvertFormatDecimal4(item.M10),
                    M11 = SystemFunction.ConvertFormatDecimal4(item.M11),
                    M12 = SystemFunction.ConvertFormatDecimal4(item.M12),
                    Target = SystemFunction.ConvertFormatDecimal4(item.Target),
                    nTotal = SystemFunction.ConvertFormatDecimal4(item.nTotal),
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
                    M1 = SystemFunction.ConvertFormatDecimal4(item.M1),
                    M2 = SystemFunction.ConvertFormatDecimal4(item.M2),
                    M3 = SystemFunction.ConvertFormatDecimal4(item.M3),
                    M4 = SystemFunction.ConvertFormatDecimal4(item.M4),
                    M5 = SystemFunction.ConvertFormatDecimal4(item.M5),
                    M6 = SystemFunction.ConvertFormatDecimal4(item.M6),
                    M7 = SystemFunction.ConvertFormatDecimal4(item.M7),
                    M8 = SystemFunction.ConvertFormatDecimal4(item.M8),
                    M9 = SystemFunction.ConvertFormatDecimal4(item.M9),
                    M10 = SystemFunction.ConvertFormatDecimal4(item.M10),
                    M11 = SystemFunction.ConvertFormatDecimal4(item.M11),
                    M12 = SystemFunction.ConvertFormatDecimal4(item.M12),
                    Target = SystemFunction.ConvertFormatDecimal4(item.Target),
                    nTotal = SystemFunction.ConvertFormatDecimal4(item.nTotal),
                    sType = "OTH",
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
                    f.M1 = SystemFunction.ConvertFormatDecimal4(item.M1);
                    f.M2 = SystemFunction.ConvertFormatDecimal4(item.M2);
                    f.M3 = SystemFunction.ConvertFormatDecimal4(item.M3);
                    f.M4 = SystemFunction.ConvertFormatDecimal4(item.M4);
                    f.M5 = SystemFunction.ConvertFormatDecimal4(item.M5);
                    f.M6 = SystemFunction.ConvertFormatDecimal4(item.M6);
                    f.M7 = SystemFunction.ConvertFormatDecimal4(item.M7);
                    f.M8 = SystemFunction.ConvertFormatDecimal4(item.M8);
                    f.M9 = SystemFunction.ConvertFormatDecimal4(item.M9);
                    f.M10 = SystemFunction.ConvertFormatDecimal4(item.M10);
                    f.M11 = SystemFunction.ConvertFormatDecimal4(item.M11);
                    f.M12 = SystemFunction.ConvertFormatDecimal4(item.M12);
                    f.Target = SystemFunction.ConvertFormatDecimal4(item.Target);
                    f.nTotal = SystemFunction.ConvertFormatDecimal4(item.nTotal);
                }
            });
            #endregion
        #endregion
        }

        resultData.lstDataNonCombustion = result.objDataEmission.lstNonCombustion;
        resultData.lstDataCEM = result.objDataEmission.lstCEM;
        resultData.lstDataAdditionalCombustion = result.objDataEmission.lstAdditional;
        resultData.lstDataAdditionalNonCombustion = result.objDataEmission.lstAdditionalNonCombustion;
        resultData.lstDataVOC = result.lstVOC;
    }
    #region Class
    public class CForGetDataDeviate
    {
        public int nIncID { get; set; }
        public int nOprtID { get; set; }
        public int nFacID { get; set; }
        public string sYear { get; set; }
        public int[] lstMonth { get; set; }
    }
    public class CReturnDataDeviate : sysGlobalClass.CResutlWebMethod
    {
        public List<CDataDeviate> lstMonth { get; set; }
    }
    public class CDataDeviate
    {
        public int nMonth { get; set; }
        public string sRemark { get; set; }
    }
    public class TSearch
    {
        public int nIndicator { get; set; }
        public int nOperationType { get; set; }
        public int nFacility { get; set; }
        public string sYear { get; set; }
    }

    [Serializable]
    public class Data_Month : sysGlobalClass.CResutlWebMethod
    {
        public List<int> lstMonth { get; set; }
        public List<string> lstShowButton { get; set; }
    }


    public class ResultData : sysGlobalClass.CResutlWebMethod
    {
        public string sFormID { get; set; }
        public List<ClassExecute.TDataOutput> lstData { get; set; }
        public List<TData_Emission> lstDataNonCombustion { get; set; }
        public List<TData_Emission> lstDataCEM { get; set; }
        public List<TData_Emission> lstDataAdditionalCombustion { get; set; }
        public List<TData_Emission> lstDataAdditionalNonCombustion { get; set; }
        public List<TData_Emission> lstDataVOC { get; set; }
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
        public int? nOptionProduct { get; set; }
        public bool? IsDel { get; set; }
        public int? nStackID { get; set; }
        public string sStackName { get; set; }
        public string cSetCode { get; set; }
    }

    public class CReturnData : sysGlobalClass.CResutlWebMethod
    {
        public ObjData objDataEmission { get; set; }
        public List<TData_Emission> lstVOC { get; set; }
        public List<TData_Emission> lstAddStack { get; set; }
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
    public class TData_Stack : TEmission_Stack
    {
        public bool? IsSaved { get; set; }
        public bool? IsSubmited { get; set; }
        public List<TData_Emission> lstDataStack { get; set; }
    }
    #endregion
}