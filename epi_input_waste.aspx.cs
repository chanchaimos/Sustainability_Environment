using ClosedXML.Excel;
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

public partial class epi_input_waste : System.Web.UI.Page
{
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_EPI_FORMS)this.Master).SetBodyEventOnLoad(myFunc);
    }

    private const string sFolderInSharePahtTemp = "UploadFiles/Waste/Temp/";
    private const string sFolderInPathSave = "UploadFiles/Waste/File/{0}/";
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
                if (SystemFunction.IsSuperAdmin()) hdfIsAdmin.Value = "Y";
                else
                {
                    //hdfIsAdmin.Value = "N";
                    int nRoleID = UserAcc.GetObjUser().nRoleID;
                    hdfRole.Value = nRoleID + "";
                }

                ((_MP_EPI_FORMS)this.Master).hdfPRMS = SystemFunction.GetPermissionMenu(10) + "";
                ((_MP_EPI_FORMS)this.Master).hdfCheckRole = UserAcc.GetObjUser().nRoleID + "";
            }
        }
    }

    #region GENDDL

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static List<TDataDDL> GetTM_Data()
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<TDataDDL> lstData = new List<TDataDDL>();

        lstData = db.mTUnit.Where(w => w.cDel == "N" && w.cActive == "Y" && w.UnitID < 5 && w.UnitID != 1).Select(s => new TDataDDL
        {
            Value = s.UnitID.ToString(),
            sText = s.UnitName,
        }).OrderBy(o => o.Value).ToList();
        return lstData;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static List<TDataDDL> GetDisposal_Data(string sMode, int ProductID, int nSubID, string sFromID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<TDataDDL> lstData = new List<TDataDDL>();
        List<TDataDDL> lstNotActive = new List<TDataDDL>();
        int nTypeID = 0;
        int nFromID = !string.IsNullOrEmpty(sFromID) ? int.Parse(sFromID) : 0;

        #region GET TYPEID
        //switch (ProductID)
        //{
        //    /// HZD
        //    case 3: //Disposal Method : Reuse
        //        nTypeID = 81;
        //        break;
        //    case 4://Disposal Method : Recycling
        //        nTypeID = 82;
        //        break;
        //    case 5://Disposal Method : Recovery
        //        nTypeID = 83;
        //        break;
        //    case 6://Disposal Method : Secured Landfill
        //        nTypeID = 84;
        //        break;
        //    case 215://Disposal Method : Compositing
        //        nTypeID = 85;
        //        break;
        //    case 216://Disposal Method : Incineration (mass burn)
        //        nTypeID = 86;
        //        break;
        //    case 217://Disposal Method : Deep well injection
        //        nTypeID = 87;
        //        break;
        //    case 7://Disposal Method : Other (e.g. Land reclamation)
        //        nTypeID = 88;
        //        break;

        //    case 9: //Disposal Method : Reuse
        //        nTypeID = 81;
        //        break;
        //    case 10://Disposal Method : Recycling
        //        nTypeID = 82;
        //        break;
        //    case 11://Disposal Method : Recovery
        //        nTypeID = 83;
        //        break;
        //    case 12://Disposal Method : Secured Landfill
        //        nTypeID = 84;
        //        break;
        //    case 218://Disposal Method : Compositing
        //        nTypeID = 85;
        //        break;
        //    case 219://Disposal Method : Incineration (mass burn)
        //        nTypeID = 86;
        //        break;
        //    case 220://Disposal Method : Deep well injection
        //        nTypeID = 87;
        //        break;
        //    case 13://Disposal Method : Other (e.g. Land reclamation)
        //        nTypeID = 88;
        //        break;

        //    //// NHZD
        //    case 18: //Disposal Method : Reuse
        //        nTypeID = 81;
        //        break;
        //    case 19://Disposal Method : Recycling
        //        nTypeID = 82;
        //        break;
        //    case 20://Disposal Method : Recovery
        //        nTypeID = 83;
        //        break;
        //    case 21://Disposal Method : Secured Landfill
        //        nTypeID = 84;
        //        break;
        //    case 221://Disposal Method : Compositing
        //        nTypeID = 85;
        //        break;
        //    case 222://Disposal Method : Incineration (mass burn)
        //        nTypeID = 86;
        //        break;
        //    case 223://Disposal Method : Deep well injection
        //        nTypeID = 87;
        //        break;
        //    case 22://Disposal Method : Other (e.g. Land reclamation)
        //        nTypeID = 88;
        //        break;
        //    case 23://Domestic/ municipal waste disposed - Routine
        //        nTypeID = 89;
        //        break;

        //    case 25: //Disposal Method : Reuse
        //        nTypeID = 81;
        //        break;
        //    case 26://Disposal Method : Recycling
        //        nTypeID = 82;
        //        break;
        //    case 27://Disposal Method : Recovery
        //        nTypeID = 83;
        //        break;
        //    case 28://Disposal Method : Secured Landfill
        //        nTypeID = 84;
        //        break;
        //    case 224://Disposal Method : Compositing
        //        nTypeID = 85;
        //        break;
        //    case 225://Disposal Method : Incineration (mass burn)
        //        nTypeID = 86;
        //        break;
        //    case 226://Disposal Method : Deep well injection
        //        nTypeID = 87;
        //        break;
        //    case 29://Disposal Method : Other (e.g. Land reclamation)
        //        nTypeID = 88;
        //        break;
        //    case 30://Domestic/ municipal waste disposed - Non-routine
        //        nTypeID = 89;
        //        break;
        //}

        #endregion

        if (sMode == "N")
        {
            nTypeID = GetTypeID(ProductID);
            lstData = db.TM_WasteDisposal.Where(w => w.cDel == "N" && w.cActive == "Y" && w.nTypeID == nTypeID).Select(s => new TDataDDL
            {
                Value = s.ID.ToString(),
                sText = s.sCode + " " + s.sName,
            }).OrderBy(o => o.Value).ToList();
        }
        else
        {
            bool isActive = true;
            var qSub = db.TWaste_Product_data.FirstOrDefault(w => w.FormID == nFromID && w.nSubProductID == nSubID && w.UnderProductID == ProductID);
            if (qSub != null)
            {
                int nDisposalID = qSub.nDisposalID ?? 0;
                if (nDisposalID == 0)
                {
                    nTypeID = GetTypeID(qSub.UnderProductID);
                }
                else
                {
                    var q = db.TM_WasteDisposal.FirstOrDefault(w => w.ID == nDisposalID);
                    if (q != null)
                    {
                        nTypeID = q.nTypeID ?? 0;
                        if (q.cActive == "N")
                        {
                            isActive = false;
                        }

                        if (!isActive)
                        {
                            lstNotActive = db.TM_WasteDisposal.Where(w => w.ID == nDisposalID).Select(s => new TDataDDL
                            {
                                Value = s.ID.ToString(),
                                sText = s.sCode + " " + s.sName,
                            }).OrderBy(o => o.Value).ToList();
                        }
                    }
                    else
                    {
                        nTypeID = GetTypeID(qSub.UnderProductID);
                    }
                }

                lstData = db.TM_WasteDisposal.Where(w => w.cDel == "N" && w.cActive == "Y" && w.nTypeID == nTypeID).Select(s => new TDataDDL
                {
                    Value = s.ID.ToString(),
                    sText = s.sCode + " " + s.sName,
                }).OrderBy(o => o.Value).ToList();

                if (lstNotActive.Count > 0)
                {
                    lstData.AddRange(lstNotActive);
                }
            }
            else
            {
                nTypeID = GetTypeID(ProductID);

                lstData = db.TM_WasteDisposal.Where(w => w.cDel == "N" && w.cActive == "Y" && w.nTypeID == nTypeID).Select(s => new TDataDDL
                {
                    Value = s.ID.ToString(),
                    sText = s.sCode + " " + s.sName,
                }).OrderBy(o => o.Value).ToList();
            }
        }

        return lstData.OrderBy(o => o.sText).ToList();
    }

    public static int GetTypeID(int ProductID)
    {
        int nTypeID = 0;
        switch (ProductID)
        {
            /// HZD
            case 3: //Disposal Method : Reuse
                nTypeID = 81;
                break;
            case 4://Disposal Method : Recycling
                nTypeID = 82;
                break;
            case 5://Disposal Method : Recovery
                nTypeID = 83;
                break;
            case 6://Disposal Method : Secured Landfill
                nTypeID = 84;
                break;
            case 215://Disposal Method : Compositing
                nTypeID = 85;
                break;
            case 216://Disposal Method : Incineration (mass burn)
                nTypeID = 86;
                break;
            case 217://Disposal Method : Deep well injection
                nTypeID = 87;
                break;
            case 7://Disposal Method : Other (e.g. Land reclamation)
                nTypeID = 88;
                break;

            case 9: //Disposal Method : Reuse
                nTypeID = 81;
                break;
            case 10://Disposal Method : Recycling
                nTypeID = 82;
                break;
            case 11://Disposal Method : Recovery
                nTypeID = 83;
                break;
            case 12://Disposal Method : Secured Landfill
                nTypeID = 84;
                break;
            case 218://Disposal Method : Compositing
                nTypeID = 85;
                break;
            case 219://Disposal Method : Incineration (mass burn)
                nTypeID = 86;
                break;
            case 220://Disposal Method : Deep well injection
                nTypeID = 87;
                break;
            case 13://Disposal Method : Other (e.g. Land reclamation)
                nTypeID = 88;
                break;

            //// NHZD
            case 18: //Disposal Method : Reuse
                nTypeID = 81;
                break;
            case 19://Disposal Method : Recycling
                nTypeID = 82;
                break;
            case 20://Disposal Method : Recovery
                nTypeID = 83;
                break;
            case 21://Disposal Method : Secured Landfill
                nTypeID = 84;
                break;
            case 221://Disposal Method : Compositing
                nTypeID = 85;
                break;
            case 222://Disposal Method : Incineration (mass burn)
                nTypeID = 86;
                break;
            case 223://Disposal Method : Deep well injection
                nTypeID = 87;
                break;
            case 22://Disposal Method : Other (e.g. Land reclamation)
                nTypeID = 88;
                break;
            case 23://Domestic/ municipal waste disposed - Routine
                nTypeID = 89;
                break;

            case 25: //Disposal Method : Reuse
                nTypeID = 81;
                break;
            case 26://Disposal Method : Recycling
                nTypeID = 82;
                break;
            case 27://Disposal Method : Recovery
                nTypeID = 83;
                break;
            case 28://Disposal Method : Secured Landfill
                nTypeID = 84;
                break;
            case 224://Disposal Method : Compositing
                nTypeID = 85;
                break;
            case 225://Disposal Method : Incineration (mass burn)
                nTypeID = 86;
                break;
            case 226://Disposal Method : Deep well injection
                nTypeID = 87;
                break;
            case 29://Disposal Method : Other (e.g. Land reclamation)
                nTypeID = 88;
                break;
            case 30://Domestic/ municipal waste disposed - Non-routine
                nTypeID = 89;
                break;

            case 240://Disposal
                nTypeID = 89;
                break;
        }

        return nTypeID;
    }

    #endregion

    #region LIST DATA

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static ResultData ListData(TSearch item)
    {
        ResultData r = new ResultData();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();

        List<TDataProductIndicator> lstHZD = new List<TDataProductIndicator>();
        List<TDataProductIndicator> lstNHZD = new List<TDataProductIndicator>();
        List<TDataProductIndicator> lstMul = new List<TDataProductIndicator>();
        List<TDataProduct> lstProduct = new List<TDataProduct>();
        List<TDataSub> lstSub = new List<TDataSub>();
        List<TDataMarsk> lstMarsk = new List<TDataMarsk>();
        List<string> lstRe = new List<string>();
        List<string> lstMonthCheck = new List<string>();
        bool isSubOld = false;

        int nFormID = 0;
        int nYearPrevious = 0;
        int nFormPreviousID = 0;

        var lstForms = db.TEPI_Forms.Where(w => w.IDIndicator == item.nIndicator && w.OperationTypeID == item.nOperationType && w.FacilityID == item.nFacility).ToList();
        if (lstForms.Count > 0)
        {
            var qFormsNow = lstForms.FirstOrDefault(w => w.sYear == item.sYear);
            if (qFormsNow != null)
            {
                nFormID = qFormsNow.FormID;
                nYearPrevious = int.Parse(item.sYear) - 1;
                var q = lstForms.FirstOrDefault(w => w.sYear == nYearPrevious.ToString());
                if (q != null) nFormPreviousID = q.FormID;
            }
            else
            {
                nYearPrevious = int.Parse(item.sYear) - 1;
                isSubOld = true;
                var q = lstForms.FirstOrDefault(w => w.sYear == nYearPrevious.ToString());
                if (q != null) nFormPreviousID = q.FormID;
            }
        }

        lstHZD = GetIndicator(item.nIndicator, "HZD", nFormID, nFormPreviousID);
        lstNHZD = GetIndicator(item.nIndicator, "NHZD", nFormID, nFormPreviousID);
        lstMul = GetIndicator(item.nIndicator, "MUL", nFormID, nFormPreviousID);
        lstMarsk = GetRemarsk(nFormID);

        #region List SUB
        /// เอาชื่อซับข้อมูลเก่ามา
        if (isSubOld)
        {
            var qFormsOld = lstForms.FirstOrDefault(w => w.sYear == nYearPrevious.ToString());
            if (qFormsOld != null)
            {
                #region
                lstSub = (from s in db.TWaste_Product_data.Where(w => w.FormID == qFormsOld.FormID).AsEnumerable()
                          from m in db.mTProductIndicator.Where(w => w.ProductID == s.UnderProductID).AsEnumerable()
                          from t in db.TM_WasteDisposal.Where(w => w.ID == s.nDisposalID).AsEnumerable().DefaultIfEmpty()
                          select new TDataSub
                          {
                              nSubID = s.nSubProductID,
                              FormID = 0,
                              nHeadID = s.UnderProductID,
                              nGroupCalc = m.nGroupCalc,
                              sName = s.ProductName,
                              sType = m.sType,
                              sUnit = s.UnitID + "",
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
                              sStatus = "Y",
                              sDisposal = s.nDisposalID + "",
                              nOptionHead = 0,
                              sDisposalName = t == null ? "" : t.sCode + " " + t.sName,
                          }).OrderBy(o => o.nGroupCalc).ToList();
                #endregion
            }
        }
        else
        {
            lstSub = GetSub(nFormID);
        }

        #endregion

        #region LIST WF
        List<sysGlobalClass.T_TEPI_Workflow> lstMonth = new List<sysGlobalClass.T_TEPI_Workflow>();

        var lstWF = db.TEPI_Workflow.Where(w => w.FormID == nFormID).ToList();
        if (lstWF.Count > 0)
        {
            foreach (var w in lstWF)
            {
                sysGlobalClass.T_TEPI_Workflow q = new sysGlobalClass.T_TEPI_Workflow();
                q.nMonth = w.nMonth;
                q.nStatusID = w.nStatusID;
                lstMonth.Add(q);
            }

            var lstAppove = lstWF.Where(w => w.nStatusID == 4).ToList();
            var lstSubmit = lstWF.Where(w => w.nStatusID == 1).ToList();
            var lstRecall = lstWF.Where(w => w.nStatusID == 24).ToList();

            if (lstSubmit.Count > 0)
            {
                foreach (var wf in lstSubmit)
                {
                    lstMonthCheck.Add(wf.nMonth.ToString());
                }

                r.nStatusWF = 1;
            }
            else
            {
                var lstStatusNotSD = lstMonth.Where(w => w.nStatusID != 0).ToList();
                if (lstStatusNotSD.Count > 0)
                {
                    r.nStatusWF = 1;
                }
                else
                {
                    r.nStatusWF = 0;
                }
            }

            //if (lstAppove.Count > 0)
            //{
            //    r.nStatusWF = 1;
            //}
        }
        else
        {
            r.nStatusWF = 0;
        }
        #endregion

        r.lstMul = lstMul;
        r.hidPrms = SystemFunction.GetPermission_EPI_FROMS(10, item.nFacility).ToString();
        r.lstRecall = lstRe;
        r.lstMonth = lstMonth;
        r.lstMonthCheck = lstMonthCheck;
        r.lstNHZD = lstNHZD;
        r.lstMarsk = lstMarsk;
        r.lstSub = lstSub.OrderBy(o => o.sType).ThenBy(n => n.nHeadID).ToList();
        r.sFormID = nFormID + "";
        r.lstHZD = lstHZD.ToList();
        r.Status = SystemFunction.process_Success;

        return r;
    }

    public static List<TDataProductIndicator> GetIndicator(int nIndicator, string sType, int nFormID, int nFormPreviousID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<TDataProductIndicator> lstData = new List<TDataProductIndicator>();
        var lstTooltip = (from a in db.TProduct_Tooltip.Where(w => w.IDIndicator == 10 && w.cType == 1)
                          from b in db.TTooltip_Product.Where(w => w.ID == a.TooltipID)
                          select new
                          {
                              b.Name,
                              a.ProductID
                          }).ToList();

        Func<int, string> GetTooltip = (prouductID) =>
        {
            var q = lstTooltip.FirstOrDefault(w => w.ProductID == prouductID);
            return q != null ? q.Name : "";
        };

        lstData = db.mTProductIndicator.Where(w => w.cDel == "N" && w.IDIndicator == nIndicator && w.sType == sType).AsEnumerable().Select(s => new TDataProductIndicator
        {
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            cTotal = s.cTotal,
            cTotalAll = s.cTotalAll,
            nGroupCalc = s.nGroupCalc,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            sSetHtml = !string.IsNullOrEmpty(s.sSetHtml) ? s.sSetHtml : "",
            sTooltip = GetTooltip(s.ProductID),
            nOption = s.nOption,
        }).OrderBy(o => o.nOrder).ToList();

        foreach (var h in lstData)
        {
            bool isOnSite = false;
            if (h.nGroupCalc == 99 && h.cTotal == "N" && h.cTotalAll == "N")
            {
                isOnSite = true;
            }

            List<TDataProduct> lstProductHZD = new List<TDataProduct>();

            if (isOnSite)
            {
                #region
                string sPrevious = "";
                var qPrevious = db.TWaste_Product.FirstOrDefault(w => w.FormID == nFormPreviousID && w.ProductID == h.ProductID);
                if (qPrevious != null)
                {
                    sPrevious = qPrevious.ReportingYear;
                }

                var q = db.TWaste_Product.FirstOrDefault(w => w.FormID == nFormID && w.ProductID == h.ProductID);
                if (q != null)
                {
                    lstProductHZD = db.TWaste_Product.Where(w => w.FormID == nFormID && w.ProductID == h.ProductID).Select(s => new TDataProduct
                    {
                        ProductID = s.ProductID,
                        FormID = s.FormID,
                        M1 = !string.IsNullOrEmpty(s.M1) ? s.M1 : "",
                        M2 = !string.IsNullOrEmpty(s.M2) ? s.M2 : "",
                        M3 = !string.IsNullOrEmpty(s.M3) ? s.M3 : "",
                        M4 = !string.IsNullOrEmpty(s.M4) ? s.M4 : "",
                        M5 = !string.IsNullOrEmpty(s.M5) ? s.M5 : "",
                        M6 = !string.IsNullOrEmpty(s.M6) ? s.M6 : "",
                        M7 = !string.IsNullOrEmpty(s.M7) ? s.M7 : "",
                        M8 = !string.IsNullOrEmpty(s.M8) ? s.M8 : "",
                        M9 = !string.IsNullOrEmpty(s.M9) ? s.M9 : "",
                        M10 = !string.IsNullOrEmpty(s.M10) ? s.M10 : "",
                        M11 = !string.IsNullOrEmpty(s.M11) ? s.M11 : "",
                        M12 = !string.IsNullOrEmpty(s.M12) ? s.M12 : "",
                        nTotal = s.nTotal,
                        PreviousYear = !string.IsNullOrEmpty(sPrevious) ? sPrevious : !string.IsNullOrEmpty(s.PreviousYear) ? s.PreviousYear : "",
                        ReportingYear = s.ReportingYear,
                        Target = s.Target,
                    }).ToList();
                }
                else
                {
                    TDataProduct t = new TDataProduct();
                    t.ProductID = h.ProductID;
                    t.FormID = nFormID;
                    t.M1 = "";
                    t.M2 = "";
                    t.M3 = "";
                    t.M4 = "";
                    t.M5 = "";
                    t.M6 = "";
                    t.M7 = "";
                    t.M8 = "";
                    t.M9 = "";
                    t.M10 = "";
                    t.M11 = "";
                    t.M12 = "";
                    t.nTotal = "";
                    t.ReportingYear = "";
                    t.PreviousYear = !string.IsNullOrEmpty(sPrevious) ? sPrevious : "";
                    t.Target = "";
                    lstProductHZD.Add(t);
                }
                #endregion
            }
            else
            {
                lstProductHZD = db.TWaste_Product.Where(w => w.FormID == nFormID && w.ProductID == h.ProductID).Select(s => new TDataProduct
                {
                    ProductID = s.ProductID,
                    FormID = s.FormID,
                    M1 = !string.IsNullOrEmpty(s.M1) ? s.M1 : "",
                    M2 = !string.IsNullOrEmpty(s.M2) ? s.M2 : "",
                    M3 = !string.IsNullOrEmpty(s.M3) ? s.M3 : "",
                    M4 = !string.IsNullOrEmpty(s.M4) ? s.M4 : "",
                    M5 = !string.IsNullOrEmpty(s.M5) ? s.M5 : "",
                    M6 = !string.IsNullOrEmpty(s.M6) ? s.M6 : "",
                    M7 = !string.IsNullOrEmpty(s.M7) ? s.M7 : "",
                    M8 = !string.IsNullOrEmpty(s.M8) ? s.M8 : "",
                    M9 = !string.IsNullOrEmpty(s.M9) ? s.M9 : "",
                    M10 = !string.IsNullOrEmpty(s.M10) ? s.M10 : "",
                    M11 = !string.IsNullOrEmpty(s.M11) ? s.M11 : "",
                    M12 = !string.IsNullOrEmpty(s.M12) ? s.M12 : "",
                    nTotal = s.nTotal,
                    PreviousYear = "",
                    ReportingYear = s.ReportingYear,
                    Target = s.Target,
                }).ToList();
            }
            h.lstProduct = lstProductHZD;
        }

        return lstData;
    }
    public static List<TDataSub> GetSub(int nFormID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<TDataSub> lstData = new List<TDataSub>();

        lstData = (from s in db.TWaste_Product_data.Where(w => w.FormID == nFormID).AsEnumerable()
                   from m in db.mTProductIndicator.Where(w => w.ProductID == s.UnderProductID).AsEnumerable()
                   from t in db.TM_WasteDisposal.Where(w => w.ID == s.nDisposalID).AsEnumerable().DefaultIfEmpty()
                   select new TDataSub
                   {
                       sStatus = "Y",
                       nSubID = s.nSubProductID,
                       FormID = s.FormID,
                       nHeadID = s.UnderProductID,
                       nGroupCalc = m.nGroupCalc,
                       sName = s.ProductName,
                       sType = m.sType,
                       sUnit = s.UnitID.ToString(),
                       M1 = !string.IsNullOrEmpty(s.M1) ? s.M1 : "",
                       M2 = !string.IsNullOrEmpty(s.M2) ? s.M2 : "",
                       M3 = !string.IsNullOrEmpty(s.M3) ? s.M3 : "",
                       M4 = !string.IsNullOrEmpty(s.M4) ? s.M4 : "",
                       M5 = !string.IsNullOrEmpty(s.M5) ? s.M5 : "",
                       M6 = !string.IsNullOrEmpty(s.M6) ? s.M6 : "",
                       M7 = !string.IsNullOrEmpty(s.M7) ? s.M7 : "",
                       M8 = !string.IsNullOrEmpty(s.M8) ? s.M8 : "",
                       M9 = !string.IsNullOrEmpty(s.M9) ? s.M9 : "",
                       M10 = !string.IsNullOrEmpty(s.M10) ? s.M10 : "",
                       M11 = !string.IsNullOrEmpty(s.M11) ? s.M11 : "",
                       M12 = !string.IsNullOrEmpty(s.M12) ? s.M12 : "",
                       Target = s.Target,
                       sDisposal = s.nDisposalID == null ? "" : s.nDisposalID.ToString(),
                       nOptionHead = m.nOption,
                       sDisposalName = t == null ? "" : (!string.IsNullOrEmpty(t.sCode) ? t.sCode : "") + " " + (!string.IsNullOrEmpty(t.sName) ? t.sName : ""),
                   }).OrderBy(o => o.nGroupCalc).ToList();

        return lstData;
    }
    public static List<TDataMarsk> GetRemarsk(int nFormID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<TDataMarsk> lstData = new List<TDataMarsk>();

        int nVerMax = db.TWaste_Remark.Where(w => w.FormID == nFormID).Any() ? db.TWaste_Remark.Where(w => w.FormID == nFormID).Max(m => m.nVersion) : 0;
        lstData = db.TWaste_Remark.Where(w => w.FormID == nFormID && w.nVersion == nVerMax).Select(s => new TDataMarsk
        {
            nProductID = s.ProductID,
            sText = s.sRemark
        }).OrderBy(o => o.nProductID).ToList();

        return lstData;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static CResultGetDataFileOther LoadDataFileOther(string sFormID) //, string sHDFMode
    {
        CResultGetDataFileOther result = new CResultGetDataFileOther();
        if (!UserAcc.UserExpired())
        {
            //if (sHDFMode != TDataType.sModeDuplicatePlan)
            //{
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            int nFromID = SystemFunction.ParseInt(sFormID); //SystemFunction2.ParseInt(sPlanID.STCDecrypt());
            result.lstData = db.TEPI_Forms_File.Where(w => w.FormID == nFromID).Select(s => new sysGlobalClass.FuncFileUpload.ItemData
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
            //}
            //else
            //{
            //    result.lstData = new List<sysGlobalClass.FuncFileUpload.ItemData>();
            //}
            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    #endregion

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod saveToDB(string sFormID, TSearch itemSeach, ItemData item, string Status)
    {
        sysGlobalClass.CResutlWebMethod r = new sysGlobalClass.CResutlWebMethod();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        DateTime now = DateTime.Now;
        List<TDataSub> lstSaveSub = new List<TDataSub>();
        int nUser = UserAcc.GetObjUser().nUserID;

        if (UserAcc.UserExpired())
        {
            r.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            #region SAVE FROM
            TEPI_Forms dF = new TEPI_Forms();
            bool isNew = false;
            int nFromID = !string.IsNullOrEmpty(sFormID) ? int.Parse(sFormID) : 0;
            int nYear = !string.IsNullOrEmpty(itemSeach.sYear) ? int.Parse(itemSeach.sYear) : 0;

            //// nFromID == 0 คือ ใหม่ 
            if (nFromID == 0)
            {
                nFromID = db.TEPI_Forms.Any() ? db.TEPI_Forms.Max(m => m.FormID) + 1 : 1;
                isNew = true;
            }
            else
            {
                dF = db.TEPI_Forms.FirstOrDefault(w => w.FormID == nFromID);
            }

            dF.IDIndicator = itemSeach.nIndicator;
            dF.FacilityID = itemSeach.nFacility;
            dF.sYear = itemSeach.sYear;
            dF.OperationTypeID = itemSeach.nOperationType;

            if (!SystemFunction.IsSuperAdmin())
            {
                dF.ResponsiblePerson = UserAcc.GetObjUser().nUserID;
                dF.dUpdateDate = now;
                dF.sUpdateBy = nUser;
            }

            if (isNew)
            {
                dF.FormID = nFromID;
                dF.sAddBy = nUser;
                dF.dAddDate = now;
                db.TEPI_Forms.Add(dF);
            }

            db.SaveChanges();

            #endregion

            #region SAVE WF
            int nWkFlowID = db.TEPI_Workflow.Any() ? db.TEPI_Workflow.Max(m => m.nReportID) + 1 : 1;
            for (int i = 1; i <= 12; i++)
            {
                int nStatus = 0;

                if (Status != "0")
                {
                    if (Status != "9999")
                    {
                        var itemData = item.lstMonth.FirstOrDefault(a => a == i);
                        if (itemData != null)
                        {
                            nStatus = int.Parse(Status);
                        }
                    }
                }

                int nReportID = db.TEPI_Workflow.Any() ? db.TEPI_Workflow.Max(ma => ma.nReportID) + 1 : 1;
                var wkflow = db.TEPI_Workflow.FirstOrDefault(w => w.FormID == nFromID && w.nMonth == i);
                if (wkflow == null)
                {
                    wkflow = new TEPI_Workflow();
                    wkflow.nReportID = nWkFlowID;
                    wkflow.FormID = nFromID;
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

            if (Status != "24" && Status != "2")
            {
                #region SAVE HZD
                foreach (var h in item.lstHZD)
                {
                    if (h.lstProduct.Count > 0)
                    {
                        foreach (var p in h.lstProduct)
                        {
                            #region มีข้อมูล
                            TWaste_Product dH = new TWaste_Product();
                            bool isNewProduct = false;
                            var q = db.TWaste_Product.FirstOrDefault(w => w.FormID == nFromID && w.ProductID == h.ProductID);
                            if (q != null)
                            {
                                dH = q;
                            }
                            else
                            {
                                isNewProduct = true;
                            }

                            dH.M1 = p.M1;
                            dH.M2 = p.M2;
                            dH.M3 = p.M3;
                            dH.M4 = p.M4;
                            dH.M5 = p.M5;
                            dH.M6 = p.M6;
                            dH.M7 = p.M7;
                            dH.M8 = p.M8;
                            dH.M9 = p.M9;
                            dH.M10 = p.M10;
                            dH.M11 = p.M11;
                            dH.M12 = p.M12;
                            dH.nTotal = p.nTotal;
                            dH.PreviousYear = p.PreviousYear;
                            dH.ReportingYear = p.ReportingYear;
                            dH.Target = p.Target;

                            if (!SystemFunction.IsSuperAdmin())
                            {
                                dH.dUpdateDate = now;
                                dH.sUpdateBy = UserAcc.GetObjUser().nUserID;
                            }

                            if (isNewProduct)
                            {
                                dH.ProductID = h.ProductID;
                                dH.FormID = nFromID;
                                dH.UnitID = 2;
                                db.TWaste_Product.Add(dH);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        /// ไม่มีข้อมูล
                        TWaste_Product dH = new TWaste_Product();
                        dH.M1 = "";
                        dH.M2 = "";
                        dH.M3 = "";
                        dH.M4 = "";
                        dH.M5 = "";
                        dH.M6 = "";
                        dH.M7 = "";
                        dH.M8 = "";
                        dH.M9 = "";
                        dH.M10 = "";
                        dH.M11 = "";
                        dH.M12 = "";
                        dH.nTotal = "";
                        dH.PreviousYear = "";
                        dH.ReportingYear = "";
                        dH.Target = "";

                        if (!SystemFunction.IsSuperAdmin())
                        {
                            dH.dUpdateDate = now;
                            dH.sUpdateBy = UserAcc.GetObjUser().nUserID;
                        }

                        dH.ProductID = h.ProductID;
                        dH.FormID = nFromID;
                        dH.UnitID = 0;
                        db.TWaste_Product.Add(dH);
                    }
                }
                db.SaveChanges();
                #endregion

                #region SAVE NHZD
                foreach (var h in item.lstNHZD)
                {
                    if (h.lstProduct.Count > 0)
                    {
                        #region  Data
                        foreach (var p in h.lstProduct)
                        {
                            TWaste_Product dH = new TWaste_Product();
                            bool isNewProduct = false;
                            var q = db.TWaste_Product.FirstOrDefault(w => w.FormID == nFromID && w.ProductID == h.ProductID);
                            if (q != null)
                            {
                                dH = q;
                            }
                            else
                            {
                                isNewProduct = true;
                            }

                            dH.M1 = p.M1;
                            dH.M2 = p.M2;
                            dH.M3 = p.M3;
                            dH.M4 = p.M4;
                            dH.M5 = p.M5;
                            dH.M6 = p.M6;
                            dH.M7 = p.M7;
                            dH.M8 = p.M8;
                            dH.M9 = p.M9;
                            dH.M10 = p.M10;
                            dH.M11 = p.M11;
                            dH.M12 = p.M12;
                            dH.nTotal = p.nTotal;
                            dH.PreviousYear = p.PreviousYear;
                            dH.ReportingYear = p.ReportingYear;
                            dH.Target = p.Target;

                            if (!SystemFunction.IsSuperAdmin())
                            {
                                dH.dUpdateDate = now;
                                dH.sUpdateBy = UserAcc.GetObjUser().nUserID;
                            }

                            if (isNewProduct)
                            {
                                dH.ProductID = h.ProductID;
                                dH.FormID = nFromID;
                                dH.UnitID = 2;
                                db.TWaste_Product.Add(dH);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        /// ไม่มีข้อมูล
                        TWaste_Product dH = new TWaste_Product();
                        dH.M1 = "";
                        dH.M2 = "";
                        dH.M3 = "";
                        dH.M4 = "";
                        dH.M5 = "";
                        dH.M6 = "";
                        dH.M7 = "";
                        dH.M8 = "";
                        dH.M9 = "";
                        dH.M10 = "";
                        dH.M11 = "";
                        dH.M12 = "";
                        dH.nTotal = "";
                        dH.PreviousYear = "";
                        dH.ReportingYear = "";
                        dH.Target = ""; ;

                        if (!SystemFunction.IsSuperAdmin())
                        {
                            dH.dUpdateDate = now;
                            dH.sUpdateBy = UserAcc.GetObjUser().nUserID;
                        }

                        dH.ProductID = h.ProductID;
                        dH.FormID = nFromID;
                        dH.UnitID = 0;
                        db.TWaste_Product.Add(dH);
                    }
                }
                db.SaveChanges();
                #endregion

                #region SAVE MUL
                foreach (var mu in item.lstMul)
                {
                    if (mu.lstProduct.Count > 0)
                    {
                        #region  Data
                        foreach (var p in mu.lstProduct)
                        {
                            TWaste_Product dH = new TWaste_Product();
                            bool isNewProduct = false;
                            var q = db.TWaste_Product.FirstOrDefault(w => w.FormID == nFromID && w.ProductID == mu.ProductID);
                            if (q != null)
                            {
                                dH = q;
                            }
                            else
                            {
                                isNewProduct = true;
                            }

                            dH.M1 = p.M1;
                            dH.M2 = p.M2;
                            dH.M3 = p.M3;
                            dH.M4 = p.M4;
                            dH.M5 = p.M5;
                            dH.M6 = p.M6;
                            dH.M7 = p.M7;
                            dH.M8 = p.M8;
                            dH.M9 = p.M9;
                            dH.M10 = p.M10;
                            dH.M11 = p.M11;
                            dH.M12 = p.M12;
                            dH.nTotal = p.nTotal;
                            dH.PreviousYear = p.PreviousYear;
                            dH.ReportingYear = p.ReportingYear;
                            dH.Target = p.Target;

                            if (!SystemFunction.IsSuperAdmin())
                            {
                                dH.dUpdateDate = now;
                                dH.sUpdateBy = UserAcc.GetObjUser().nUserID;
                            }

                            if (isNewProduct)
                            {
                                dH.ProductID = mu.ProductID;
                                dH.FormID = nFromID;
                                dH.UnitID = 2;
                                db.TWaste_Product.Add(dH);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        /// ไม่มีข้อมูล
                        TWaste_Product dH = new TWaste_Product();
                        dH.M1 = "";
                        dH.M2 = "";
                        dH.M3 = "";
                        dH.M4 = "";
                        dH.M5 = "";
                        dH.M6 = "";
                        dH.M7 = "";
                        dH.M8 = "";
                        dH.M9 = "";
                        dH.M10 = "";
                        dH.M11 = "";
                        dH.M12 = "";
                        dH.nTotal = "";
                        dH.PreviousYear = "";
                        dH.ReportingYear = "";
                        dH.Target = ""; ;

                        if (!SystemFunction.IsSuperAdmin())
                        {
                            dH.dUpdateDate = now;
                            dH.sUpdateBy = UserAcc.GetObjUser().nUserID;
                        }

                        dH.ProductID = mu.ProductID;
                        dH.FormID = nFromID;
                        dH.UnitID = 0;
                        db.TWaste_Product.Add(dH);
                    }
                }
                db.SaveChanges();
                #endregion

                #region SAVE SUB

                if (item.lstSub.Any())
                {
                    /// DELETE ALL
                    db.TWaste_Product_data.Where(w => w.FormID == nFromID).ToList().ForEach(x => db.TWaste_Product_data.Remove(x));
                    db.SaveChanges();

                    var qGroupHeader = item.lstSub.GroupBy(g => g.nHeadID).Select(s => s.Key).ToList();
                    foreach (var itemH in qGroupHeader)
                    {
                        int nSubID = itemH + 1;
                        var lstSubHead = item.lstSub.Where(w => w.nHeadID == itemH && w.sStatus == "Y").ToList();
                        foreach (var s in lstSubHead)
                        {
                            //if (s.sStatus == "Y")
                            //{
                            TWaste_Product_data dS = new TWaste_Product_data();
                            dS.ProductName = s.sName;
                            dS.UnitID = !string.IsNullOrEmpty(s.sUnit) ? int.Parse(s.sUnit) : 0;
                            dS.Target = s.Target;
                            dS.M1 = s.M1;
                            dS.M2 = s.M2;
                            dS.M3 = s.M3;
                            dS.M4 = s.M4;
                            dS.M5 = s.M5;
                            dS.M6 = s.M6;
                            dS.M7 = s.M7;
                            dS.M8 = s.M8;
                            dS.M9 = s.M9;
                            dS.M10 = s.M10;
                            dS.M11 = s.M11;
                            dS.M12 = s.M12;

                            if (!string.IsNullOrEmpty(s.sDisposal))
                            {
                                dS.nDisposalID = int.Parse(s.sDisposal);
                            }

                            dS.nSubProductID = nSubID;
                            dS.FormID = nFromID;
                            dS.UnderProductID = itemH;
                            db.TWaste_Product_data.Add(dS);
                            nSubID++;
                            //}
                        }
                    }
                    db.SaveChanges();
                }

                #endregion

                #region SAVE MASK

                int nVer = db.TWaste_Remark.Where(w => w.FormID == nFromID).Any() ? db.TWaste_Remark.Where(w => w.FormID == nFromID).Max(m => m.nVersion) + 1 : 1;
                foreach (var m in item.lstMask)
                {
                    TWaste_Remark dR = new TWaste_Remark();

                    dR.FormID = nFromID;
                    dR.ProductID = m.nProductID;
                    dR.nVersion = nVer;
                    dR.sRemark = m.sText;
                    dR.dAddDate = now;
                    dR.sAddBy = nUser;

                    db.TWaste_Remark.Add(dR);
                    db.SaveChanges();

                }

                #endregion

                #region SAVE FILE
                if (item.lstDataFile.Count > 0)
                {
                    string sPathSave = string.Format(sFolderInPathSave, nFromID);
                    SystemFunction.CreateDirectory(sPathSave);

                    //ลบไฟล์เดิมที่เคยมีและกดลบจากหน้าเว็บ
                    var qDelFile = item.lstDataFile.Where(w => w.IsNewFile == false && w.sDelete == "Y").ToList();
                    if (qDelFile.Any())
                    {
                        foreach (var qf in qDelFile)
                        {
                            var query = db.TEPI_Forms_File.FirstOrDefault(w => w.FormID == nFromID && w.sSysFileName == qf.SaveToFileName);
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
                    item.lstDataFile.Where(w => w.IsNewFile == false && w.sDelete == "N").ToList().ForEach(f2U =>
                    {
                        var data2Update = db.TEPI_Forms_File.FirstOrDefault(w => w.FormID == nFromID && w.nFileID == f2U.ID);
                        if (data2Update != null)
                        {
                            data2Update.sDescription = f2U.sDescription;
                        }
                    });
                    //Save New File Only
                    var lstSave = item.lstDataFile.Where(w => w.IsNewFile == true && w.sDelete == "N").ToList();
                    if (lstSave.Any())
                    {
                        int nFileID = db.TEPI_Forms_File.Where(w => w.FormID == nFromID).Any() ? db.TEPI_Forms_File.Where(w => w.FormID == nFromID).Max(m => m.nFileID) + 1 : 1;

                        foreach (var s in lstSave)
                        {
                            string sSystemFileName = nFromID + "_" + nFileID + "-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "." + SystemFunction.GetFileNameFromFileupload(s.SaveToFileName, ""); //SystemFunction2.GetFileType(item.SaveToFileName);
                            SystemFunction.UpFile2Server(s.SaveToPath, sPathSave, s.SaveToFileName, sSystemFileName);

                            // SystemFunction2.UpFile2Server(item.SaveToPath, sPathSave, item.SaveToFileName, sSystemFileName);
                            TEPI_Forms_File t = new TEPI_Forms_File();
                            t.FormID = nFromID;
                            t.nFileID = nFileID;
                            t.sSysFileName = sSystemFileName;
                            t.sFileName = s.FileName;
                            t.sPath = sPathSave;
                            t.sDescription = s.sDescription;
                            t.dAdd = now;
                            t.nAddBy = nUser;
                            db.TEPI_Forms_File.Add(t);
                            nFileID++;
                        }
                    }
                    db.SaveChanges();

                    //Save OLD File Only
                    var lstSaveOld = item.lstDataFile.Where(w => w.IsNewFile == false && w.sDelete == "N").ToList();
                    if (lstSaveOld.Any())
                    {
                        foreach (var f in lstSaveOld)
                        {
                            TEPI_Forms_File dFile = new TEPI_Forms_File();

                            int nFileID = 0;
                            nFileID = f.ID;
                            dFile = db.TEPI_Forms_File.FirstOrDefault(w => w.FormID == nFromID && w.nFileID == nFileID);

                            dFile.sDescription = f.sDescription;
                            db.SaveChanges();
                        }
                    }
                }

                #endregion
            }

            //Calcuate Output
            new EPIFunc().RecalculateWaste(itemSeach.nOperationType, itemSeach.nFacility, itemSeach.sYear);

            if (Status != "27")
            {
                new Workflow().UpdateHistoryStatus(nFromID);
            }

            if (Status != "0")//Not Save Draft
            {
                if (Status != "9999")//Not L2 and Not Supper Admin
                {
                    string sMode = "";
                    switch (Status)
                    {
                        case "1":
                            sMode = "SM";
                            break;
                        case "2":
                            sMode = "RQ";
                            break;
                        case "24":
                            sMode = "RC";
                            break;
                        case "27":
                            sMode = "APC";
                            break;
                    }

                    r = new Workflow().WorkFlowAction(nFromID, item.lstMonth, sMode, UserAcc.GetObjUser().nUserID, UserAcc.GetObjUser().nRoleID, item.sComment);
                }
                else
                {
                    if (UserAcc.GetObjUser().nRoleID == 4)//ENVI Corporate (L2) >> Req.09.04.2019 Send email to L0 on L2 Modified data.
                    {
                        new Workflow().SendEmailToL0onL2EditData(itemSeach.sYear, itemSeach.nIndicator, itemSeach.nFacility, itemSeach.nOperationType);
                    }
                    r.Status = SystemFunction.process_Success;
                }
            }
            else
            {
                r.Status = SystemFunction.process_Success;
            }

        }

        return r;
    }

    #region EXPORT
    protected void btnEx_Click(object sender, EventArgs e)
    {
        ExportData_new();
    }
    public void ExportData_new()
    {

        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        if (!UserAcc.UserExpired())  //Check Session
        {
            int nOperationType = !string.IsNullOrEmpty(((_MP_EPI_FORMS)this.Master).OperationType) ? int.Parse(((_MP_EPI_FORMS)this.Master).OperationType) : 0;
            int nIndicator = !string.IsNullOrEmpty(((_MP_EPI_FORMS)this.Master).Indicator) ? int.Parse(((_MP_EPI_FORMS)this.Master).Indicator) : 0;
            int nFacility = !string.IsNullOrEmpty(((_MP_EPI_FORMS)this.Master).Facility) ? int.Parse(((_MP_EPI_FORMS)this.Master).Facility) : 0;

            TSearch item = new TSearch();
            item.sYear = ((_MP_EPI_FORMS)this.Master).Year;
            item.nOperationType = nOperationType;
            item.nIndicator = nIndicator;
            item.nFacility = nFacility;

            ResultData dd = ListData(item);

            string sIndicatorName = "";
            var qI = db.mTIndicator.FirstOrDefault(w => w.ID == nIndicator);
            if (qI != null) sIndicatorName = qI.Indicator;

            string sOName = "";
            var qO = db.mOperationType.FirstOrDefault(w => w.ID == nOperationType);
            if (qO != null) sOName = qO.Name;

            string sFacility = "";
            var qF = db.mTFacility.FirstOrDefault(w => w.ID == nFacility);
            if (qF != null) sFacility = qF.Name;

            var lstDeviate = SystemFunction.GetDeviate(nIndicator, nOperationType, nFacility, ((_MP_EPI_FORMS)this.Master).Year);

            string saveAsFileName = "Input_" + sIndicatorName + "_" + sFacility + "_" + DateTime.Now.ToString("ddMMyyHHmmss", new CultureInfo("en-US")) + ".xlsx";

            XLWorkbook workbook = GetWorkbookExcel(dd.lstHZD, dd.lstNHZD, dd.lstMarsk, dd.lstSub, saveAsFileName
                , sIndicatorName, sOName, sFacility, ((_MP_EPI_FORMS)this.Master).Year, lstDeviate, dd.lstMul);

            #region Save the Excel spreadsheet to a MemoryStream and return it to the client
            using (MemoryStream exportData = new MemoryStream())
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("Downloaded", "True"));
                workbook.SaveAs(exportData);
                exportData.Position = 0;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.BinaryWrite(exportData.ToArray());
                HttpContext.Current.Response.End();
            }
            #endregion
        }
    }
    public static XLWorkbook GetWorkbookExcel(List<TDataProductIndicator> lstHZD, List<TDataProductIndicator> lstNHZD
        , List<TDataMarsk> lstMarsk, List<TDataSub> lstSub, string sFileName, string sIndicatorName, string sOName, string sFacility, string sYear
        , List<sysGlobalClass.TDeviate> lstDeviate, List<TDataProductIndicator> lstMul)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        // Creating a new workbook
        XLWorkbook workbook = new XLWorkbook();
        workbook.Style.Font.FontName = "Cordia New";
        //Adding a worksheet
        string saveAsFileName = string.Format("Report_{0:ddMMyyyyHHmmss}", DateTime.Now).Replace("/", "_");

        #region Report Header
        IXLStyle ReportStyle = (IXLStyle)workbook.Style;
        ReportStyle.Font.Bold = true;
        ReportStyle.Font.FontSize = 14;
        ReportStyle.Font.FontColor = XLColor.Black;
        ReportStyle.Fill.BackgroundColor = XLColor.FromHtml("#FFFFFF");
        ReportStyle.Fill.PatternType = XLFillPatternValues.Solid;
        ReportStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        ReportStyle.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        #endregion

        #region FUNTION กลาง

        #region STYLE HEAD
        Action<IXLRow, int, string> SetHeadStyle = (row, nColumn, sHeadText) =>
        {
            IXLCell cell = row.Cell(nColumn);
            cell.Value = sHeadText;
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.FromHtml("#fff");
            cell.Style.Border.TopBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.LeftBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.RightBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.BottomBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#9cb726");
            cell.Style.Fill.PatternType = XLFillPatternValues.Solid;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            //cell.Style = THeadStyle;
        };
        Action<IXLRow, int, XLCellValues, XLAlignmentHorizontalValues, bool, object, string> SetBodyStyleHeader = (row, nColumn, CType, iHAlign, isBold, ObjValue, sFormat) =>
        {
            IXLCell cell = row.Cell(nColumn);
            cell.DataType = CType;
            cell.Value = ObjValue;
            //IXLStyle TBodyStyle = (IXLStyle)workbook.Style;
            cell.Style.Font.Bold = isBold;
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.Black;
            cell.Style.Border.TopBorderColor = XLColor.FromHtml("#FFFFFF");
            cell.Style.Border.LeftBorderColor = XLColor.FromHtml("#FFFFFF");
            cell.Style.Border.RightBorderColor = XLColor.FromHtml("#FFFFFF");
            cell.Style.Border.BottomBorderColor = XLColor.FromHtml("#FFFFFF");
            cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#ffffff");
            cell.Style.Fill.PatternType = XLFillPatternValues.Solid;
            cell.Style.Alignment.Horizontal = iHAlign;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.DateFormat.Format = "";
            cell.Style.NumberFormat.Format = "";

            if (CType == XLCellValues.DateTime) cell.Style.DateFormat.Format = sFormat;
            else if (CType == XLCellValues.Number) cell.Style.NumberFormat.Format = sFormat;
            //cell.Style = TBodyStyle;
        };
        #endregion

        #region Merge Cell
        Action<string, IXLWorksheet> SetMerge = (sRange, ws) =>
        {
            IXLRange rangeMerge = ws.Range(sRange);
            rangeMerge.Merge();
            rangeMerge.Style.Border.TopBorderColor = XLColor.FromHtml("#FFFFFF");
            rangeMerge.Style.Border.LeftBorderColor = XLColor.FromHtml("#FFFFFF");
            rangeMerge.Style.Border.RightBorderColor = XLColor.FromHtml("#FFFFFF");
            rangeMerge.Style.Border.BottomBorderColor = XLColor.FromHtml("#FFFFFF");
            rangeMerge.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            rangeMerge.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            rangeMerge.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            rangeMerge.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        };
        #endregion

        #region STYLE BODY
        Action<IXLRow, int, XLCellValues, XLAlignmentHorizontalValues, bool, object, string> SetBodyStyle = (row, nColumn, CType, iHAlign, isBold, ObjValue, sFormat) =>
        {
            IXLCell cell = row.Cell(nColumn);
            cell.DataType = CType;
            cell.Value = ObjValue;
            //IXLStyle TBodyStyle = (IXLStyle)workbook.Style;
            cell.Style.Font.Bold = isBold;
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.Black;
            cell.Style.Border.TopBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.LeftBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.RightBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.BottomBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFFFFF");
            cell.Style.Fill.PatternType = XLFillPatternValues.Solid;
            cell.Style.Alignment.Horizontal = iHAlign;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.DateFormat.Format = "";
            cell.Style.NumberFormat.Format = "";
            cell.Style.Alignment.WrapText = true;

            if (CType == XLCellValues.DateTime) cell.Style.DateFormat.Format = sFormat;
            else if (CType == XLCellValues.Number) cell.Style.NumberFormat.Format = sFormat;
            //cell.Style = TBodyStyle;
        };

        Action<IXLRow, int, XLCellValues, XLAlignmentHorizontalValues, bool, object, string, string> SetBodyStyle2 = (row, nColumn, CType, iHAlign, isBold, ObjValue, sFormat, sBackgroundColor) =>
        {
            IXLCell cell = row.Cell(nColumn);
            cell.DataType = CType;
            cell.Value = ObjValue;
            //IXLStyle TBodyStyle = (IXLStyle)workbook.Style;
            cell.Style.Font.Bold = isBold;
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Border.TopBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.LeftBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.RightBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.BottomBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            if (sBackgroundColor == "#fabd4f") cell.Style.Font.FontColor = XLColor.Black;
            else if (sBackgroundColor == "#ffffff") cell.Style.Font.FontColor = XLColor.Black;
            else cell.Style.Font.FontColor = XLColor.Black;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml(sBackgroundColor);
            cell.Style.Fill.PatternType = XLFillPatternValues.Solid;
            cell.Style.Alignment.Horizontal = iHAlign;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.DateFormat.Format = "";
            cell.Style.NumberFormat.Format = "";
            cell.Style.Alignment.WrapText = true;

            if (CType == XLCellValues.DateTime) cell.Style.DateFormat.Format = sFormat;
            else if (CType == XLCellValues.Number) cell.Style.NumberFormat.Format = sFormat;
            //cell.Style = TBodyStyle;
        };
        #endregion

        #endregion

        #region HZD/NHZD

        //row number must be between 1 and 1048576
        int nRownumber = 1;
        //Column number must be between 1 and 16384
        int nColnumber = 1;
        IXLWorksheet worksheet = workbook.Worksheets.Add("Waste");
        worksheet.ShowGridLines = false;
        IXLCell icell;
        IXLRow irow;

        #region Set Column Width
        worksheet.Column(1).Width = 50;
        worksheet.Column(2).Width = 20;
        worksheet.Column(3).Width = 20;
        worksheet.Column(4).Width = 20;
        worksheet.Column(5).Width = 20;
        worksheet.Column(6).Width = 20;
        worksheet.Column(7).Width = 20;
        worksheet.Column(8).Width = 20;
        worksheet.Column(9).Width = 20;
        worksheet.Column(10).Width = 20;
        worksheet.Column(11).Width = 20;
        worksheet.Column(12).Width = 20;
        worksheet.Column(13).Width = 20;
        worksheet.Column(14).Width = 20;
        worksheet.Column(15).Width = 20;
        worksheet.Column(16).Width = 20;
        worksheet.Column(17).Width = 50;

        worksheet.Row(nRownumber).Height = 30;
        irow = worksheet.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Indicator : " + sIndicatorName, "0");
        nRownumber++;

        worksheet.Row(nRownumber).Height = 30;
        irow = worksheet.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Operation Type : " + sOName, "0");
        nRownumber++;

        worksheet.Row(nRownumber).Height = 30;
        irow = worksheet.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Sub - facility : " + sFacility, "0");
        nRownumber++;

        worksheet.Row(nRownumber).Height = 30;
        irow = worksheet.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Year : " + sYear, "0");
        nRownumber++;
        #endregion

        var lstUnit = db.mTUnit.Where(w => w.cActive == "Y" && w.cDel == "N" && w.UnitID < 5 && w.UnitID != 1).ToList();
        #region HZD

        #region HEAD
        nRownumber++;
        worksheet.Row(nRownumber).Height = 30;
        irow = worksheet.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Center, true, "Hazardous Waste", "0");

        nRownumber++;
        irow = worksheet.Row(nRownumber);
        irow.Height = 22.20;

        nColnumber = 1;

        SetHeadStyle(irow, nColnumber++, "Indicator");
        SetHeadStyle(irow, nColnumber++, "Disposal Code");
        SetHeadStyle(irow, nColnumber++, "Unit");
        SetHeadStyle(irow, nColnumber++, "Target");
        SetHeadStyle(irow, nColnumber++, "Q1 : Jan");
        SetHeadStyle(irow, nColnumber++, "Q1 : Feb");
        SetHeadStyle(irow, nColnumber++, "Q1 : Mar");
        SetHeadStyle(irow, nColnumber++, "Q2 : Apr");
        SetHeadStyle(irow, nColnumber++, "Q2 : May");
        SetHeadStyle(irow, nColnumber++, "Q2 : Jun");
        SetHeadStyle(irow, nColnumber++, "Q3 : Jul");
        SetHeadStyle(irow, nColnumber++, "Q3 : Aug");
        SetHeadStyle(irow, nColnumber++, "Q3 : Sep");
        SetHeadStyle(irow, nColnumber++, "Q4 : Oct");
        SetHeadStyle(irow, nColnumber++, "Q4 : Nov");
        SetHeadStyle(irow, nColnumber++, "Q4 : Dec");
        SetHeadStyle(irow, nColnumber++, "Remark");

        #endregion

        int index = 1;
        foreach (var item in lstHZD)
        {
            nColnumber = 1;
            nRownumber++;
            irow = worksheet.Row(nRownumber);
            irow.Height = 17.40;

            List<TDataSub> lstS = new List<TDataSub>();
            List<TDataMarsk> lstM = new List<TDataMarsk>();

            lstM = lstMarsk.Where(w => w.nProductID == item.ProductID).ToList();

            string sColor = item.cTotal == "Y" && item.cTotalAll == "N" ? "#fabd4f" : item.nGroupCalc == 99 ? "#fabd4f" : item.nGroupCalc == 12 ? "#fabd4f" : item.cTotal == "Y" && item.cTotalAll == "Y" ? "#dbea97" : "#ffedc4";
            SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.ProductName, "", sColor);
            SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor); ///Disposal Code
            SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.sUnit, "", sColor);
            if (item.lstProduct.Any())
            {
                ////// TOTAL
                if (item.nGroupCalc == 12)
                {
                    foreach (var p in item.lstProduct)
                    {
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, p.nTotal, "", sColor);
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", sColor);
                    }
                }
                else if (item.nGroupCalc == 99)
                {
                    foreach (var p in item.lstProduct)
                    {
                        int nYearOld = !string.IsNullOrEmpty(sYear) ? int.Parse(sYear) - 1 : 0;
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "Previous year(" + nYearOld + ")  " + p.PreviousYear, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "Reporting year(" + sYear + ")  " + p.ReportingYear, "", sColor);
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", sColor);
                    }
                }
                else
                {
                    foreach (var p in item.lstProduct)
                    {
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.Target, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M1, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M2, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M3, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M4, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M5, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M6, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M7, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M8, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M9, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M10, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M11, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M12, "", sColor);
                    }
                }

                /////Remark
                if (lstM.Count > 0)
                {
                    foreach (var m in lstM)
                    {
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, m.sText, sColor);
                    }
                }
                else
                {
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, "", sColor);
                }

            }
            else
            {
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                /////Remark
                if (lstM.Count > 0)
                {
                    foreach (var m in lstM)
                    {
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, m.sText, sColor);
                    }
                }
                else
                {
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, "", sColor);
                }
            }

            //// BIND SUB 

            lstS = lstSub.Where(w => w.nHeadID == item.ProductID).ToList();
            if (lstS.Count > 0)
            {
                sColor = "#ffffff";
                foreach (var s in lstS)
                {
                    nColnumber = 1;
                    nRownumber++;
                    irow = worksheet.Row(nRownumber);
                    irow.Height = 17.40;

                    int nUnit = !string.IsNullOrEmpty(s.sUnit) ? int.Parse(s.sUnit) : 0;
                    string sUnit = lstUnit.FirstOrDefault(w => w.UnitID == nUnit).UnitName;

                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, "  " + s.sName, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, "  " + s.sDisposalName, "", sColor);///Disposal Code
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sUnit, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.Target, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M1, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M2, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M3, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M4, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M5, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M6, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M7, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M8, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M9, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M10, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M11, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M12, "", sColor);
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", sColor);
                }

            }

            index++;
        }
        #endregion

        nRownumber++;

        #region NHZD
        nRownumber++;
        worksheet.Row(nRownumber).Height = 30;
        irow = worksheet.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Center, true, "None Hazardous Waste", "0");

        nRownumber++;
        irow = worksheet.Row(nRownumber);
        irow.Height = 22.20;

        nColnumber = 1;

        SetHeadStyle(irow, nColnumber++, "Indicator");
        SetHeadStyle(irow, nColnumber++, "Disposal Code");
        SetHeadStyle(irow, nColnumber++, "Unit");
        SetHeadStyle(irow, nColnumber++, "Target");
        SetHeadStyle(irow, nColnumber++, "Q1 : Jan");
        SetHeadStyle(irow, nColnumber++, "Q1 : Feb");
        SetHeadStyle(irow, nColnumber++, "Q1 : Mar");
        SetHeadStyle(irow, nColnumber++, "Q2 : Apr");
        SetHeadStyle(irow, nColnumber++, "Q2 : May");
        SetHeadStyle(irow, nColnumber++, "Q2 : Jun");
        SetHeadStyle(irow, nColnumber++, "Q3 : Jul");
        SetHeadStyle(irow, nColnumber++, "Q3 : Aug");
        SetHeadStyle(irow, nColnumber++, "Q3 : Sep");
        SetHeadStyle(irow, nColnumber++, "Q4 : Oct");
        SetHeadStyle(irow, nColnumber++, "Q4 : Nov");
        SetHeadStyle(irow, nColnumber++, "Q4 : Dec");
        SetHeadStyle(irow, nColnumber++, "Remark");

        int indexN = 1;
        foreach (var item in lstNHZD)
        {
            nColnumber = 1;
            nRownumber++;
            irow = worksheet.Row(nRownumber);
            irow.Height = 17.40;



            List<TDataSub> lstS = new List<TDataSub>();
            List<TDataMarsk> lstM = new List<TDataMarsk>();

            lstM = lstMarsk.Where(w => w.nProductID == item.ProductID).ToList();

            string sColor = item.cTotal == "Y" && item.cTotalAll == "N" ? "#fabd4f" : item.nGroupCalc == 99 ? "#fabd4f" : item.nGroupCalc == 12 ? "#fabd4f" : item.cTotal == "Y" && item.cTotalAll == "Y" ? "#dbea97" : "#ffedc4";
            SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.ProductName, "", sColor);
            SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, "", "", sColor);
            SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.sUnit, "", sColor);
            if (item.lstProduct.Any())
            {
                ////// TOTAL
                if (item.nGroupCalc == 12)
                {
                    foreach (var p in item.lstProduct)
                    {
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, p.nTotal, "", sColor);
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", sColor);
                    }
                }
                else if (item.nGroupCalc == 99)
                {
                    foreach (var p in item.lstProduct)
                    {
                        int nYearOld = !string.IsNullOrEmpty(sYear) ? int.Parse(sYear) - 1 : 0;
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "Previous year(" + nYearOld + ")   " + p.PreviousYear, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "Reporting year(" + sYear + ")   " + p.ReportingYear, "", sColor);
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", sColor);
                    }
                }
                else
                {
                    foreach (var p in item.lstProduct)
                    {
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.Target, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M1, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M2, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M3, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M4, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M5, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M6, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M7, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M8, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M9, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M10, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M11, "", sColor);
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M12, "", sColor);
                    }
                }

                if (lstM.Count > 0)
                {
                    foreach (var m in lstM)
                    {
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, m.sText, sColor);
                    }
                }
                else
                {
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", sColor);
                }
            }
            else
            {
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);

                if (lstM.Count > 0)
                {
                    foreach (var m in lstM)
                    {
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, m.sText, sColor);
                    }
                }
                else
                {
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", sColor);
                }
            }

            lstS = lstSub.Where(w => w.nHeadID == item.ProductID).ToList();
            if (lstS.Count > 0)
            {
                sColor = "#ffffff";
                foreach (var s in lstS)
                {
                    nColnumber = 1;
                    nRownumber++;
                    irow = worksheet.Row(nRownumber);
                    irow.Height = 17.40;

                    int nUnit = !string.IsNullOrEmpty(s.sUnit) ? int.Parse(s.sUnit) : 0;
                    string sUnit = lstUnit.FirstOrDefault(w => w.UnitID == nUnit).UnitName;

                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, "  " + s.sName, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, s.sDisposalName, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sUnit, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.Target, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M1, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M2, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M3, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M4, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M5, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M6, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M7, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M8, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M9, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M10, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M11, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M12, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                }

            }

            indexN++;
        }
        #endregion
        nRownumber++;

        #region Other municipal waste
        nRownumber++;
        worksheet.Row(nRownumber).Height = 30;
        irow = worksheet.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Center, true, "Other municipal waste", "0");

        nRownumber++;
        irow = worksheet.Row(nRownumber);
        irow.Height = 22.20;

        nColnumber = 1;

        SetHeadStyle(irow, nColnumber++, "Indicator");
        SetHeadStyle(irow, nColnumber++, "Disposal Code");
        SetHeadStyle(irow, nColnumber++, "Unit");
        SetHeadStyle(irow, nColnumber++, "Target");
        SetHeadStyle(irow, nColnumber++, "Q1 : Jan");
        SetHeadStyle(irow, nColnumber++, "Q1 : Feb");
        SetHeadStyle(irow, nColnumber++, "Q1 : Mar");
        SetHeadStyle(irow, nColnumber++, "Q2 : Apr");
        SetHeadStyle(irow, nColnumber++, "Q2 : May");
        SetHeadStyle(irow, nColnumber++, "Q2 : Jun");
        SetHeadStyle(irow, nColnumber++, "Q3 : Jul");
        SetHeadStyle(irow, nColnumber++, "Q3 : Aug");
        SetHeadStyle(irow, nColnumber++, "Q3 : Sep");
        SetHeadStyle(irow, nColnumber++, "Q4 : Oct");
        SetHeadStyle(irow, nColnumber++, "Q4 : Nov");
        SetHeadStyle(irow, nColnumber++, "Q4 : Dec");
        SetHeadStyle(irow, nColnumber++, "Remark");

        int indexO = 1;
        foreach (var item in lstMul)
        {
            nColnumber = 1;
            nRownumber++;
            irow = worksheet.Row(nRownumber);
            irow.Height = 17.40;

            List<TDataSub> lstS = new List<TDataSub>();
            List<TDataMarsk> lstM = new List<TDataMarsk>();

            lstM = lstMarsk.Where(w => w.nProductID == item.ProductID).ToList();

            string sColor = item.cTotal == "Y" && item.cTotalAll == "N" ? "#fabd4f" : item.nGroupCalc == 99 ? "#fabd4f" : item.nGroupCalc == 12 ? "#fabd4f" : item.cTotal == "Y" && item.cTotalAll == "Y" ? "#dbea97" : "#ffedc4";
            SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.ProductName, "", sColor);
            SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, "", "", sColor);
            SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.sUnit, "", sColor);
            if (item.lstProduct.Any())
            {
                ////// TOTAL
                foreach (var p in item.lstProduct)
                {
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.Target, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M1, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M2, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M3, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M4, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M5, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M6, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M7, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M8, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M9, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M10, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M11, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, p.M12, "", sColor);
                }

                if (lstM.Count > 0)
                {
                    foreach (var m in lstM)
                    {
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, m.sText, sColor);
                    }
                }
                else
                {
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", sColor);
                }
            }
            else
            {
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);

                if (lstM.Count > 0)
                {
                    foreach (var m in lstM)
                    {
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, m.sText, sColor);
                    }
                }
                else
                {
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", sColor);
                }
            }

            lstS = lstSub.Where(w => w.nHeadID == item.ProductID).ToList();
            if (lstS.Count > 0)
            {
                sColor = "#ffffff";
                foreach (var s in lstS)
                {
                    nColnumber = 1;
                    nRownumber++;
                    irow = worksheet.Row(nRownumber);
                    irow.Height = 17.40;

                    int nUnit = !string.IsNullOrEmpty(s.sUnit) ? int.Parse(s.sUnit) : 0;
                    string sUnit = lstUnit.FirstOrDefault(w => w.UnitID == nUnit).UnitName;

                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, "  " + s.sName, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, s.sDisposalName, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sUnit, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.Target, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M1, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M2, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M3, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M4, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M5, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M6, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M7, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M8, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M9, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M10, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M11, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, s.M12, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "", sColor);
                }

            }

            indexO++;
        }
        #endregion
        nRownumber++;

        #endregion

        #region BIND DATA DEVIATE
        IXLWorksheet ws2 = workbook.Worksheets.Add("Deviate");
        nRownumber = 1;
        nColnumber = 1;

        ws2.Column(1).Width = 20;
        ws2.Column(2).Width = 20;
        ws2.Column(3).Width = 50;
        ws2.Column(4).Width = 30;
        ws2.Column(5).Width = 20;

        ws2.Row(nRownumber).Height = 30;
        irow = ws2.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Indicator : " + sIndicatorName, "0");
        nRownumber++;

        ws2.Row(nRownumber).Height = 30;
        irow = ws2.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Operation Type : " + sOName, "0");
        nRownumber++;

        ws2.Row(nRownumber).Height = 30;
        irow = ws2.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Sub - facility : " + sFacility, "0");
        nRownumber++;

        ws2.Row(nRownumber).Height = 30;
        irow = ws2.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Year : " + sYear, "0");
        nRownumber++;

        irow = ws2.Row(nRownumber);
        irow.Height = 22.20;

        nColnumber = 1;

        SetHeadStyle(irow, nColnumber++, "No.");
        SetHeadStyle(irow, nColnumber++, "Month");
        SetHeadStyle(irow, nColnumber++, "Remark");
        SetHeadStyle(irow, nColnumber++, "Action By");
        SetHeadStyle(irow, nColnumber++, "Date");

        if (lstDeviate.Any())
        {
            for (int i = 0; i < lstDeviate.Count(); i++)
            {
                nColnumber = 1;
                nRownumber++;
                irow = ws2.Row(nRownumber);
                irow.Height = 17.40;

                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, (i + 1), "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, lstDeviate[i].sMonth, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, lstDeviate[i].sRemark, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, lstDeviate[i].sActionBy, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "'" + lstDeviate[i].sDate + "", "");
            }
        }
        #endregion

        return workbook;
    }

    #endregion

    #region CLASS
    public class ResultDetail : sysGlobalClass.CResutlWebMethod
    {
        public List<TDataDetail> lstData { get; set; }
    }
    public class ResultData : sysGlobalClass.CResutlWebMethod
    {
        public int nStatusWF { get; set; }
        public string sFormID { get; set; }
        public string hidPrms { get; set; }
        public List<TDataProductIndicator> lstHZD { get; set; }
        public List<TDataProductIndicator> lstNHZD { get; set; }
        public List<TDataSub> lstSub { get; set; }
        public List<TDataMarsk> lstMarsk { get; set; }
        public List<sysGlobalClass.T_TEPI_Workflow> lstMonth { get; set; }
        public List<string> lstMonthCheck { get; set; }
        public List<string> lstRecall { get; set; }
        public List<TDataProductIndicator> lstMul { get; set; }
    }
    public class ItemData
    {
        public List<TDataProductIndicator> lstHZD { get; set; }
        public List<TDataProductIndicator> lstNHZD { get; set; }
        public List<TDataSub> lstSub { get; set; }
        public List<TDataMarsk> lstMask { get; set; }
        public List<sysGlobalClass.FuncFileUpload.ItemData> lstDataFile { get; set; }
        public List<int> lstMonth { get; set; }
        public List<string> lstRecall { get; set; }
        public string sComment { get; set; }
        public List<TDataProductIndicator> lstMul { get; set; }
    }
    public class TSearch
    {
        public int nIndicator { get; set; }
        public int nOperationType { get; set; }
        public int nFacility { get; set; }
        public string sYear { get; set; }
        public bool isCheckQ1 { get; set; }
        public bool isCheckQ2 { get; set; }
        public bool isCheckQ3 { get; set; }
        public bool isCheckQ4 { get; set; }
    }
    public class TDataProductIndicator
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int? nGroupCalc { get; set; }
        public int? nOrder { get; set; }
        public string sUnit { get; set; }
        public string sSetHtml { get; set; }
        public string sTextMarsk { get; set; }
        public List<TDataProduct> lstProduct { get; set; }
        public string sTooltip { get; set; }
        public int? nOption { get; set; }
    }
    public class TDataProduct
    {
        public int? FormID { get; set; }
        public int? ProductID { get; set; }
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
        public string nTotal { get; set; }
        public string PreviousYear { get; set; }
        public string ReportingYear { get; set; }
        public string Target { get; set; }
    }
    public class TDataSub
    {
        public string sStatus { get; set; }
        public int? FormID { get; set; }
        public int nHeadID { get; set; }
        public int nSubID { get; set; }
        public int? nGroupCalc { get; set; }
        public string sName { get; set; }
        public string sType { get; set; }
        public string sUnit { get; set; }
        public string Target { get; set; }
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
        public bool isSubmit { get; set; }
        public string sDisposal { get; set; }
        public int? nOptionHead { get; set; }
        public string sDisposalName { get; set; }
    }
    public class TDataDDL
    {
        public string Value { get; set; }
        public string sText { get; set; }
    }
    public class TDataMarsk
    {
        public int nProductID { get; set; }
        public string sText { get; set; }
    }

    [Serializable]
    public class CResultGetDataFileOther : sysGlobalClass.CResutlWebMethod
    {
        public List<sysGlobalClass.FuncFileUpload.ItemData> lstData { get; set; }
    }

    public class TDataDetail
    {
        public int FormID { get; set; }
        public string sMonth { get; set; }
        public int nProductID { get; set; }
        public string sRemark { get; set; }
        public DateTime dAction { get; set; }
        public string ProductName { get; set; }
        public string sActionBy { get; set; }
        public string sDate { get; set; }
    }

    #endregion
}