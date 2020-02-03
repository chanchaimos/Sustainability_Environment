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

public partial class epi_input_materials : System.Web.UI.Page
{
    private const string sFolderInSharePahtTemp = "UploadFiles/Materials/Temp/";
    private const string sFolderInPathSave = "UploadFiles/Materials/File/{0}/";
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

                    ((_MP_EPI_FORMS)this.Master).hdfPRMS = SystemFunction.GetPermissionMenu(8) + "";
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
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            result.lstProduct = new List<CProduct>();
            result.lstUnit = new List<CUnit>();
            result.lstStatus = new List<sysGlobalClass.T_TEPI_Workflow>();
            result.lstFile = new List<sysGlobalClass.FuncFileUpload.ItemData>();
            result.lstTooltip = new List<CTooltip>();
            int nIndID = SystemFunction.GetIntNullToZero(param.sIndID);
            int nOprtID = SystemFunction.GetIntNullToZero(param.sOprtID);
            int nFacID = SystemFunction.GetIntNullToZero(param.sFacID);
            result.hdfPRMS = SystemFunction.GetPermission_EPI_FROMS(nIndID, nFacID) + "";
            string sYear = param.sYear;
            bool IsNew = true;

            #region Tooltip
            var DataTooltip = (from i in db.TProduct_Tooltip.Where(w => w.IDIndicator == 8 && w.cType == 1)
                               from t in db.TTooltip_Product.Where(w => w.ID == i.TooltipID)
                               select new CTooltip
                               {
                                   ProductID = i.ProductID,
                                   sTooltip = t.Name
                               }).ToList();
            Func<int, string> GetTooltip = (ProducID) =>
            {
                string sTooltip = "";
                var itemTooltip = DataTooltip.FirstOrDefault(w => w.ProductID == ProducID);
                sTooltip = itemTooltip != null ? itemTooltip.sTooltip : "";
                return sTooltip;
            };
            #endregion
            //result.lstTooltip
            #region FORM_EPI
            var itemEPI_FORM = db.TEPI_Forms.FirstOrDefault(w => w.sYear == sYear && w.IDIndicator == nIndID && w.OperationTypeID == nOprtID && w.FacilityID == nFacID);
            if (itemEPI_FORM != null)
            {
                IsNew = false;
                result.EPI_FORMID = itemEPI_FORM.FormID + "";
                result.lstStatus = db.TEPI_Workflow.Where(w => w.FormID == itemEPI_FORM.FormID).Select(s => new sysGlobalClass.T_TEPI_Workflow { nMonth = s.nMonth, nStatusID = s.nStatusID }).ToList();

                result.nStatus = result.lstStatus.Any(w => w.nStatusID > 0) ? 1 : 0;
            }
            else
            {
                for (int i = 1; i <= 12; i++)
                {
                    result.lstStatus.Add(new sysGlobalClass.T_TEPI_Workflow { nMonth = i, nStatusID = 0 });
                }
                result.nStatus = 0;
                sYear = (int.Parse(sYear) - 1) + "";
                itemEPI_FORM = db.TEPI_Forms.FirstOrDefault(w => w.sYear == sYear && w.IDIndicator == nIndID && w.OperationTypeID == nOprtID && w.FacilityID == nFacID);
                if (itemEPI_FORM != null)
                {
                    result.EPI_FORMID = itemEPI_FORM.FormID + "";
                }
            }
            int[] lstUnit = new int[] { 1, 2, 3 };
            result.lstUnit = db.mTUnit.Where(w => lstUnit.Contains(w.UnitID)).OrderByDescending(o => o.nFactor).ThenByDescending(o => o.UnitName).Select(s => new CUnit { nUnitID = s.UnitID, sUnitName = s.UnitName }).ToList();
            db.mTProductIndicator.Where(w => w.IDIndicator == 8).ToList().ForEach(f =>
            {
                CProduct item = new CProduct();
                switch (f.ProductID)
                {
                    case 33:
                        item.nHeaderID = null;
                        item.nLevel = 1;
                        break;
                    case 34:
                        item.nHeaderID = 33;
                        item.nLevel = 2;
                        break;
                    case 35:
                        item.nHeaderID = 34;
                        item.nLevel = 3;
                        break;
                    case 36:
                        item.nHeaderID = 34;
                        item.nLevel = 3;
                        break;
                    case 37:
                        item.nHeaderID = 33;
                        item.nLevel = 2;
                        break;
                    case 38:
                        item.nHeaderID = 37;
                        item.nLevel = 3;
                        break;
                    case 39:
                        item.nHeaderID = 37;
                        item.nLevel = 3;
                        break;
                    case 40:
                        item.nHeaderID = 37;
                        item.nLevel = 3;
                        break;
                    case 41:
                        item.nHeaderID = 33;
                        item.nLevel = 2;
                        break;
                }
                item.ProductID = f.ProductID;
                item.ProductName = f.ProductName;
                item.sUnit = f.sUnit;
                item.sOption = item.nLevel == 1 ? "" : (item.nLevel == 2 ? "0" : "");
                item.cTotal = f.cTotal;
                item.cTotalAll = f.cTotalAll;
                item.sTooltip = GetTooltip(f.ProductID);
                result.lstProduct.Add(item);
            });
            if (itemEPI_FORM != null)
            {
                var lstTMaterial_Product = db.TMaterial_Product.Where(w => w.FormID + "" == result.EPI_FORMID).ToList();
                var lstMeterial_ProductData = db.TMaterial_ProductData.Where(w => w.FormID + "" == result.EPI_FORMID).ToList();
                result.lstProduct.ForEach(f =>
                {
                    if (f.nLevel <= 3)
                    {
                        var item = lstTMaterial_Product.FirstOrDefault(w => w.ProductID == f.ProductID);
                        if (item != null)
                        {
                            if (!IsNew)
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
                                f.Target = item.Target ?? "";
                                f.sOption = f.nLevel == 2 ? item.nOption + "" : "";
                            }
                            else
                            {
                                f.M1 = "";
                                f.M2 = "";
                                f.M3 = "";
                                f.M4 = "";
                                f.M5 = "";
                                f.M6 = "";
                                f.M7 = "";
                                f.M8 = "";
                                f.M9 = "";
                                f.M10 = "";
                                f.M11 = "";
                                f.M12 = "";
                                f.Target = "";
                                f.sOption = f.nLevel == 2 ? item.nOption + "" : "";
                            }
                        }
                        else
                        {
                            f.M1 = "";
                            f.M2 = "";
                            f.M3 = "";
                            f.M4 = "";
                            f.M5 = "";
                            f.M6 = "";
                            f.M7 = "";
                            f.M8 = "";
                            f.M9 = "";
                            f.M10 = "";
                            f.M11 = "";
                            f.M12 = "";
                            f.Target = "";
                            f.sOption = f.nLevel == 2 ? "0" : "";
                        }
                        lstMeterial_ProductData.Where(w => w.nUnderbyProductID == f.ProductID).ToList().ForEach(data =>
                        {
                            CProduct prdData = new CProduct();
                            prdData.nLevel = 4;
                            prdData.ProductID = f.nHeaderID;
                            prdData.ProductName = data.sName;
                            prdData.nHeaderID = data.nUnderbyProductID;
                            prdData.sUnit = data.nUnitID + "";
                            prdData.sOption = "";
                            prdData.cTotal = "N";
                            prdData.cTotalAll = "N";
                            if (!IsNew)
                            {
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
                            }
                            else
                            {
                                prdData.sDensity = "";
                                prdData.Target = "";
                                prdData.M1 = "";
                                prdData.M2 = "";
                                prdData.M3 = "";
                                prdData.M4 = "";
                                prdData.M5 = "";
                                prdData.M6 = "";
                                prdData.M7 = "";
                                prdData.M8 = "";
                                prdData.M9 = "";
                                prdData.M10 = "";
                                prdData.M11 = "";
                                prdData.M12 = "";
                            }
                            result.lstProduct.Add(prdData);

                        });
                    }
                });
                if (!IsNew)
                {
                    int FORM_ID = int.Parse(result.EPI_FORMID);
                    result.lstFile = db.TEPI_Forms_File.Where(w => w.FormID == FORM_ID).Select(s => new sysGlobalClass.FuncFileUpload.ItemData
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
                    var lstRemark = db.TMaterial_Remark.Where(w => w.FormID == FORM_ID).ToList();
                    result.sRemarkTMU = lstRemark.Any(w => w.ProductID == 33) ? lstRemark.Where(w => w.ProductID == 33).OrderByDescending(o => o.nVersion).FirstOrDefault().sRemark : "";
                    result.sRemarkDMU = lstRemark.Any(w => w.ProductID == 34) ? lstRemark.Where(w => w.ProductID == 34).OrderByDescending(o => o.nVersion).FirstOrDefault().sRemark : "";
                    result.sRemarkAMU = lstRemark.Any(w => w.ProductID == 37) ? lstRemark.Where(w => w.ProductID == 37).OrderByDescending(o => o.nVersion).FirstOrDefault().sRemark : "";
                }
            }
            else
            {
                result.lstProduct.ForEach(f =>
                {
                    f.M1 = "";
                    f.M2 = "";
                    f.M3 = "";
                    f.M4 = "";
                    f.M5 = "";
                    f.M6 = "";
                    f.M7 = "";
                    f.M8 = "";
                    f.M9 = "";
                    f.M10 = "";
                    f.M11 = "";
                    f.M12 = "";
                    f.Target = "";
                    f.sOption = f.nLevel == 2 ? "0" : "";
                });
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
    public static sysGlobalClass.CResutlWebMethod saveToDB(CSaveToDB arrData)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
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

            if (arrData.nStatus != 24)
            {
                #region Product
                if (!IsNew)
                {
                    db.TMaterial_ProductData.RemoveRange(db.TMaterial_ProductData.Where(w => w.FormID == FORM_ID));
                    db.SaveChanges();
                }
                var lstProductLV1 = arrData.lstProduct.Where(w => w.nLevel == 1).ToList();
                foreach (var itemLv1 in lstProductLV1)
                {
                    #region LV1
                    bool IsLV1New = false;
                    var prdLv1 = db.TMaterial_Product.FirstOrDefault(w => w.FormID == FORM_ID && w.ProductID == itemLv1.ProductID);
                    if (prdLv1 == null)
                    {
                        IsLV1New = true;
                        prdLv1 = new TMaterial_Product();
                        prdLv1.FormID = FORM_ID;
                        prdLv1.ProductID = itemLv1.ProductID.Value;
                        prdLv1.sAddBy = UserAcc.GetObjUser().nUserID;
                        prdLv1.dAddDate = DateTime.Now;
                    }
                    prdLv1.M1 = itemLv1.M1;
                    prdLv1.M2 = itemLv1.M2;
                    prdLv1.M3 = itemLv1.M3;
                    prdLv1.M4 = itemLv1.M4;
                    prdLv1.M5 = itemLv1.M5;
                    prdLv1.M6 = itemLv1.M6;
                    prdLv1.M7 = itemLv1.M7;
                    prdLv1.M8 = itemLv1.M8;
                    prdLv1.M9 = itemLv1.M9;
                    prdLv1.M10 = itemLv1.M10;
                    prdLv1.M11 = itemLv1.M11;
                    prdLv1.M12 = itemLv1.M12;
                    prdLv1.nTotal = SumAll(new string[] { itemLv1.M1, itemLv1.M2, itemLv1.M3, itemLv1.M4, itemLv1.M5, itemLv1.M6, itemLv1.M7, itemLv1.M8, itemLv1.M9, itemLv1.M10, itemLv1.M11, itemLv1.M12, });
                    if (!IsLV1New)
                    {
                        if (!SystemFunction.IsSuperAdmin())
                        {
                            prdLv1.sUpdateBy = UserAcc.GetObjUser().nUserID;
                            prdLv1.dUpdateDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        prdLv1.sUpdateBy = UserAcc.GetObjUser().nUserID;
                        prdLv1.dUpdateDate = DateTime.Now;
                    }
                    prdLv1.Target = itemLv1.Target;
                    prdLv1.nOption = 2;
                    if (IsLV1New) db.TMaterial_Product.Add(prdLv1);

                    #endregion
                    var lstProductLV2 = arrData.lstProduct.Where(w => w.nHeaderID == itemLv1.ProductID).ToList();
                    foreach (var itemLv2 in lstProductLV2)
                    {
                        #region LV2
                        bool IsLV2New = false;
                        var prdLv2 = db.TMaterial_Product.FirstOrDefault(w => w.FormID == FORM_ID && w.ProductID == itemLv2.ProductID);
                        if (prdLv2 == null)
                        {
                            IsLV2New = true;
                            prdLv2 = new TMaterial_Product();
                            prdLv2.FormID = FORM_ID;
                            prdLv2.ProductID = itemLv2.ProductID.Value;
                            prdLv2.sAddBy = UserAcc.GetObjUser().nUserID;
                            prdLv2.dAddDate = DateTime.Now;
                        }
                        prdLv2.M1 = itemLv2.M1;
                        prdLv2.M2 = itemLv2.M2;
                        prdLv2.M3 = itemLv2.M3;
                        prdLv2.M4 = itemLv2.M4;
                        prdLv2.M5 = itemLv2.M5;
                        prdLv2.M6 = itemLv2.M6;
                        prdLv2.M7 = itemLv2.M7;
                        prdLv2.M8 = itemLv2.M8;
                        prdLv2.M9 = itemLv2.M9;
                        prdLv2.M10 = itemLv2.M10;
                        prdLv2.M11 = itemLv2.M11;
                        prdLv2.M12 = itemLv2.M12;
                        prdLv2.nTotal = SumAll(new string[] { itemLv2.M1, itemLv2.M2, itemLv2.M3, itemLv2.M4, itemLv2.M5, itemLv2.M6, itemLv2.M7, itemLv2.M8, itemLv2.M9, itemLv2.M10, itemLv2.M11, itemLv2.M12, });
                        if (!IsLV2New)
                        {
                            if (!SystemFunction.IsSuperAdmin())
                            {
                                prdLv2.sUpdateBy = UserAcc.GetObjUser().nUserID;
                                prdLv2.dUpdateDate = DateTime.Now;
                            }
                        }
                        else
                        {
                            prdLv2.sUpdateBy = UserAcc.GetObjUser().nUserID;
                            prdLv2.dUpdateDate = DateTime.Now;
                        }
                        prdLv2.Target = itemLv2.Target;
                        prdLv2.nOption = itemLv2.sOption != "0" ? 2 : 0;
                        if (IsLV2New) db.TMaterial_Product.Add(prdLv2);

                        #endregion
                        var lstProductLV3 = arrData.lstProduct.Where(w => w.nHeaderID == itemLv2.ProductID).ToList();
                        if (itemLv2.ProductID == 41)
                        {
                            if (itemLv2.sOption != "0")
                            {
                                int nSubID = itemLv2.ProductID.Value + 1;
                                foreach (var itemLv3 in lstProductLV3)
                                {
                                    #region LV3 Only ProductID = 41
                                    TMaterial_ProductData prdLv4 = new TMaterial_ProductData();
                                    prdLv4.nSubProductID = nSubID;
                                    prdLv4.nUnderbyProductID = itemLv3.nHeaderID.Value;
                                    prdLv4.FormID = FORM_ID;
                                    prdLv4.sName = itemLv3.ProductName;
                                    prdLv4.nOption = 2;
                                    prdLv4.M1 = itemLv3.M1;
                                    prdLv4.M2 = itemLv3.M2;
                                    prdLv4.M3 = itemLv3.M3;
                                    prdLv4.M4 = itemLv3.M4;
                                    prdLv4.M5 = itemLv3.M5;
                                    prdLv4.M6 = itemLv3.M6;
                                    prdLv4.M7 = itemLv3.M7;
                                    prdLv4.M8 = itemLv3.M8;
                                    prdLv4.M9 = itemLv3.M9;
                                    prdLv4.M10 = itemLv3.M10;
                                    prdLv4.M11 = itemLv3.M11;
                                    prdLv4.M12 = itemLv3.M12;
                                    prdLv4.nTotal = SumAll(new string[] { itemLv3.M1, itemLv3.M2, itemLv3.M3, itemLv3.M4, itemLv3.M5, itemLv3.M6, itemLv3.M7, itemLv3.M8, itemLv3.M9, itemLv3.M10, itemLv3.M11, itemLv3.M12, });
                                    prdLv4.Target = itemLv3.Target;
                                    prdLv4.Density = itemLv3.sUnit == "1" ? itemLv3.sDensity : "";
                                    prdLv4.nUnitID = int.Parse(itemLv3.sUnit);
                                    db.TMaterial_ProductData.Add(prdLv4);
                                    nSubID++;
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            foreach (var itemLv3 in lstProductLV3)
                            {
                                #region LV3
                                bool IsLV3New = false;
                                var prdLv3 = db.TMaterial_Product.FirstOrDefault(w => w.FormID == FORM_ID && w.ProductID == itemLv3.ProductID);
                                if (prdLv3 == null)
                                {
                                    IsLV3New = true;
                                    prdLv3 = new TMaterial_Product();
                                    prdLv3.FormID = FORM_ID;
                                    prdLv3.ProductID = itemLv3.ProductID.Value;
                                    prdLv3.sAddBy = UserAcc.GetObjUser().nUserID;
                                    prdLv3.dAddDate = DateTime.Now;
                                }
                                prdLv3.M1 = itemLv2.sOption != "0" ? itemLv3.M1 : "";
                                prdLv3.M2 = itemLv2.sOption != "0" ? itemLv3.M2 : "";
                                prdLv3.M3 = itemLv2.sOption != "0" ? itemLv3.M3 : "";
                                prdLv3.M4 = itemLv2.sOption != "0" ? itemLv3.M4 : "";
                                prdLv3.M5 = itemLv2.sOption != "0" ? itemLv3.M5 : "";
                                prdLv3.M6 = itemLv2.sOption != "0" ? itemLv3.M6 : "";
                                prdLv3.M7 = itemLv2.sOption != "0" ? itemLv3.M7 : "";
                                prdLv3.M8 = itemLv2.sOption != "0" ? itemLv3.M8 : "";
                                prdLv3.M9 = itemLv2.sOption != "0" ? itemLv3.M9 : "";
                                prdLv3.M10 = itemLv2.sOption != "0" ? itemLv3.M10 : "";
                                prdLv3.M11 = itemLv2.sOption != "0" ? itemLv3.M11 : "";
                                prdLv3.M12 = itemLv2.sOption != "0" ? itemLv3.M12 : "";
                                prdLv3.nTotal = SumAll(new string[] { itemLv3.M1, itemLv3.M2, itemLv3.M3, itemLv3.M4, itemLv3.M5, itemLv3.M6, itemLv3.M7, itemLv3.M8, itemLv3.M9, itemLv3.M10, itemLv3.M11, itemLv3.M12, });
                                if (!IsLV3New)
                                {
                                    if (!SystemFunction.IsSuperAdmin())
                                    {
                                        prdLv3.sUpdateBy = UserAcc.GetObjUser().nUserID;
                                        prdLv3.dUpdateDate = DateTime.Now;
                                    }
                                }
                                else
                                {
                                    prdLv3.sUpdateBy = UserAcc.GetObjUser().nUserID;
                                    prdLv3.dUpdateDate = DateTime.Now;
                                }
                                prdLv3.Target = itemLv3.Target;
                                prdLv3.nOption = itemLv2.sOption != "0" ? 2 : 0;
                                if (IsLV3New) db.TMaterial_Product.Add(prdLv3);

                                #endregion
                                var lstProductLV4 = arrData.lstProduct.Where(w => w.nHeaderID == itemLv3.ProductID).ToList();
                                if (lstProductLV4.Any())
                                {
                                    if (itemLv2.sOption != "0")
                                    {
                                        int nSubID = itemLv3.ProductID.Value + 1;
                                        foreach (var itemLv4 in lstProductLV4)
                                        {
                                            #region LV4
                                            TMaterial_ProductData prdLv4 = new TMaterial_ProductData();
                                            prdLv4.nSubProductID = nSubID;
                                            prdLv4.nUnderbyProductID = itemLv4.nHeaderID.Value;
                                            prdLv4.FormID = FORM_ID;
                                            prdLv4.sName = itemLv4.ProductName;
                                            prdLv4.nOption = 2;
                                            prdLv4.M1 = itemLv4.M1;
                                            prdLv4.M2 = itemLv4.M2;
                                            prdLv4.M3 = itemLv4.M3;
                                            prdLv4.M4 = itemLv4.M4;
                                            prdLv4.M5 = itemLv4.M5;
                                            prdLv4.M6 = itemLv4.M6;
                                            prdLv4.M7 = itemLv4.M7;
                                            prdLv4.M8 = itemLv4.M8;
                                            prdLv4.M9 = itemLv4.M9;
                                            prdLv4.M10 = itemLv4.M10;
                                            prdLv4.M11 = itemLv4.M11;
                                            prdLv4.M12 = itemLv4.M12;
                                            prdLv4.nTotal = SumAll(new string[] { itemLv4.M1, itemLv4.M2, itemLv4.M3, itemLv4.M4, itemLv4.M5, itemLv4.M6, itemLv4.M7, itemLv4.M8, itemLv4.M9, itemLv4.M10, itemLv4.M11, itemLv4.M12, });
                                            prdLv4.Target = itemLv4.Target;
                                            prdLv4.Density = itemLv4.sUnit == "1" ? itemLv4.sDensity : "";
                                            prdLv4.nUnitID = int.Parse(itemLv4.sUnit);
                                            db.TMaterial_ProductData.Add(prdLv4);
                                            nSubID++;
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                db.SaveChanges();
                #endregion

                #region Remark
                if (!string.IsNullOrEmpty(arrData.sRemarkTMU))
                {
                    var itemRemark = db.TMaterial_Remark.Where(w => w.FormID == FORM_ID && w.ProductID == 33);
                    int nVersion = itemRemark.Any() ? itemRemark.Max(m => m.nVersion) + 1 : 1;
                    TMaterial_Remark TMURmrk = new TMaterial_Remark();
                    TMURmrk.FormID = FORM_ID;
                    TMURmrk.ProductID = 33;
                    TMURmrk.nVersion = nVersion;
                    TMURmrk.sRemark = arrData.sRemarkTMU;
                    if (itemRemark.Count() > 0)
                    {
                        if (!SystemFunction.IsSuperAdmin())
                        {
                            TMURmrk.sAddBy = UserAcc.GetObjUser().nUserID;
                            TMURmrk.dAddDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        TMURmrk.sAddBy = UserAcc.GetObjUser().nUserID;
                        TMURmrk.dAddDate = DateTime.Now;
                    }
                    db.TMaterial_Remark.Add(TMURmrk);
                }
                if (!string.IsNullOrEmpty(arrData.sRemarkDMU))
                {
                    var itemRemark = db.TMaterial_Remark.Where(w => w.FormID == FORM_ID && w.ProductID == 34);
                    int nVersion = itemRemark.Any() ? itemRemark.Max(m => m.nVersion) + 1 : 1;
                    TMaterial_Remark DMURmrk = new TMaterial_Remark();
                    DMURmrk.FormID = FORM_ID;
                    DMURmrk.ProductID = 34;
                    DMURmrk.nVersion = nVersion;
                    DMURmrk.sRemark = arrData.sRemarkDMU;
                    if (itemRemark.Count() > 0)
                    {
                        if (!SystemFunction.IsSuperAdmin())
                        {
                            DMURmrk.sAddBy = UserAcc.GetObjUser().nUserID;
                            DMURmrk.dAddDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        DMURmrk.sAddBy = UserAcc.GetObjUser().nUserID;
                        DMURmrk.dAddDate = DateTime.Now;
                    }
                    db.TMaterial_Remark.Add(DMURmrk);
                }
                if (!string.IsNullOrEmpty(arrData.sRemarkAMU))
                {
                    var itemRemark = db.TMaterial_Remark.Where(w => w.FormID == FORM_ID && w.ProductID == 37);
                    int nVersion = itemRemark.Any() ? itemRemark.Max(m => m.nVersion) + 1 : 1;
                    TMaterial_Remark AMURmrk = new TMaterial_Remark();
                    AMURmrk.FormID = FORM_ID;
                    AMURmrk.ProductID = 37;
                    AMURmrk.nVersion = nVersion;
                    AMURmrk.sRemark = arrData.sRemarkAMU;
                    if (itemRemark.Count() > 0)
                    {
                        if (!SystemFunction.IsSuperAdmin())
                        {
                            AMURmrk.sAddBy = UserAcc.GetObjUser().nUserID;
                            AMURmrk.dAddDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        AMURmrk.sAddBy = UserAcc.GetObjUser().nUserID;
                        AMURmrk.dAddDate = DateTime.Now;
                    }
                    db.TMaterial_Remark.Add(AMURmrk);
                }
                db.SaveChanges();
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

                            dFile.sDescription = f.sDescription;
                            db.SaveChanges();
                        }
                    }
                }
                #endregion

                new EPIFunc().RecalculateMaterial(arrData.nOperationID, arrData.nFacilityID, arrData.sYear);
            }
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
    public static string SumAll(string[] M)
    {
        string result = "";
        double Total = 0;
        bool IsPass = false;
        foreach (var item in M)
        {
            if (item.Trim() != "" && item.ToLower().Trim() != "n/a")
            {
                IsPass = true;
                Total += double.Parse(item);
            }
        }
        if (IsPass)
        {
            result = SystemFunction.ConvertExponentialToString(Total + "");
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
        List<CProductDataExport> lstProduct = new List<CProductDataExport>();
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
            CProductDataExport itemH = new CProductDataExport();
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
                        CProductDataExport prdData = new CProductDataExport();
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
            string[] arrDataInMonth = new string[12] { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
            SetTbl(ws1, "'" + item.ProductName, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, item.sDensity, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
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
            SetTbl(ws1, item.Total, nRow, nCol, 14, true, XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Center, true, null, null);
            nCol++;
            SetTbl(ws1, "'" + item.sRemark, nRow, nCol, 14, false, XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Center, true, null, null);
            ws1.Range(nRow, 1, nRow, nCol).Style.Fill.BackgroundColor = XLColor.FromHtml(arrColor[item.nLevel - 1]);
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
    public class CReturnData : sysGlobalClass.CResutlWebMethod
    {
        public string EPI_FORMID { get; set; }
        public string sRemarkTMU { get; set; }
        public string sRemarkDMU { get; set; }
        public string sRemarkAMU { get; set; }
        public int nStatus { get; set; }
        public List<sysGlobalClass.T_TEPI_Workflow> lstStatus { get; set; }
        public List<CUnit> lstUnit { get; set; }
        public List<CProduct> lstProduct { get; set; }
        public List<sysGlobalClass.FuncFileUpload.ItemData> lstFile { get; set; }
        public List<CTooltip> lstTooltip { get; set; }
        public string hdfPRMS { get; set; }
    }
    public class CProduct
    {
        public int? nHeaderID { get; set; }
        public int nLevel { get; set; }
        public string sOption { get; set; }
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
        public string sUnit { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public string Target { get; set; }
        public string sDensity { get; set; }
        public string sTooltip { get; set; }
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
    }
    public class CUnit
    {
        public int nUnitID { get; set; }
        public string sUnitName { get; set; }
    }
    public class CSaveToDB
    {
        public int nStatus { get; set; }
        public string sRemarkTMU { get; set; }
        public string sRemarkDMU { get; set; }
        public string sRemarkAMU { get; set; }
        //public string EPI_FORM { get; set; }
        public int nIndicatorID { get; set; }
        public int nOperationID { get; set; }
        public int nFacilityID { get; set; }
        public string sYear { get; set; }
        public string sRemarkRequestEdit { get; set; }
        public List<int> lstMonthSubmit { get; set; }
        //public List<string> lstMonthRecall { get; set; }
        //public List<CStatus> lstStatus { get; set; }
        public List<CProduct> lstProduct { get; set; }
        public List<sysGlobalClass.FuncFileUpload.ItemData> lstFile { get; set; }
    }
    public class CStatus
    {
        public int nMonth { get; set; }
        public int? nStatus { get; set; }
    }
    public class CProductDataExport
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
    public class CTooltip
    {
        public int ProductID { get; set; }
        public string sTooltip { get; set; }
    }
    #endregion
}