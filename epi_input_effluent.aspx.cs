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

public partial class epi_input_effluent : System.Web.UI.Page
{
    private static int nMSProductTWD = 115; //Total Water Discharge
    private static int nMSPorductFlowrate = 124;//Flow rate
    private static int nMSProductOperatinghours = 125;//Operating hours
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_EPI_FORMS)this.Master).SetBodyEventOnLoad(myFunc);
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
                if (SystemFunction.IsSuperAdmin()) hdfIsAdmin.Value = "Y";
                else
                {
                    //hdfIsAdmin.Value = "N";
                    int nRoleID = UserAcc.GetObjUser().nRoleID;
                    hdfRole.Value = nRoleID + "";
                }

                ((_MP_EPI_FORMS)this.Master).hdfPRMS = SystemFunction.GetPermissionMenu(10) + "";
                ((_MP_EPI_FORMS)this.Master).hdfCheckRole = UserAcc.GetObjUser().nRoleID + "";
                BindData();
            }
        }
    }

    public void BindData()
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lst = db.TData_Type.Where(w => w.IndicatorID == 3 && w.cActive == "Y").ToList();
        ddlTreatment.DataSource = lst.Where(w => w.sType == "TMMTHOD").Select(s => new { val = s.nID, name = s.sName });
        ddlTreatment.DataValueField = "val";
        ddlTreatment.DataTextField = "name";
        ddlTreatment.DataBind();
        ddlTreatment.Items.Insert(0, new ListItem("- select -", ""));
        ddlTreatment.Items.Add(new ListItem("Other", "999"));

        ddlArea.DataSource = lst.Where(w => w.sType == "AREA").Select(s => new { val = s.nID, name = s.sName });
        ddlArea.DataValueField = "val";
        ddlArea.DataTextField = "name";
        ddlArea.DataBind();
        ddlArea.Items.Insert(0, new ListItem("- select -", ""));

        ddlDischarge.DataSource = lst.Where(w => w.sType == "DISCH2").Select(s => new { val = s.nID, name = s.sName });
        ddlDischarge.DataValueField = "val";
        ddlDischarge.DataTextField = "name";
        ddlDischarge.DataBind();
        ddlDischarge.Items.Insert(0, new ListItem("- select -", ""));
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static List<TDataDDL> GetTM_Data(string sType)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<TDataDDL> lstData = new List<TDataDDL>();

        if (sType == "I")
        {
            //// && w.cActive == "Y"
            lstData = db.TM_Effluent_OtherProduct.Where(w => w.cDel == "N").Select(s => new TDataDDL
            {
                Value = s.nProductID.ToString(),
                sText = s.sName,
                cActive = s.cActive,
            }).OrderBy(o => o.Value).ToList();
        }
        else if (sType == "U")
        {
            ////&& w.cActive == "Y"
            lstData = db.TM_Effluent_Unit.Where(w => w.cDel == "N").Select(s => new TDataDDL
            {
                Value = s.nUnitID.ToString(),
                sText = s.sName,
                cActive = s.cActive,
            }).OrderBy(o => o.Value).ToList();
        }

        return lstData;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static CReturnData LoadData(CParam param)
    {
        CReturnData result = new CReturnData();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();

            int nIndID = SystemFunction.GetIntNullToZero(param.sIndID);
            int nOprtID = SystemFunction.GetIntNullToZero(param.sOprtID);
            int nFacID = SystemFunction.GetIntNullToZero(param.sFacID);
            result.hdfPRMS = SystemFunction.GetPermission_EPI_FROMS(nIndID, nFacID) + "";
            result.lstStatus = new List<sysGlobalClass.T_TEPI_Workflow>();
            result.lstIndicatorOther = new List<TDataDDL>();
            result.lstUnitOther = new List<TDataDDL>();
            result.sRemarkTotal = "";
            result.lstPointInput = new List<CPointType>();
            result.lstPoint = new List<TData_Point>();
            result.lstOther = new List<OtherPoint>();
            result.sPercentWater = "";
            result.sNumPoint = "";
            result.EPI_FORMID = "0";
            string sYear = param.sYear;
            bool IsNew = true;

            List<CProduct> lstProduct = new List<CProduct>();

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

            #region TProductIndicator
            string sqlPro = @"select up.ProductID,up.OperationTypeID,up.nOrder,up.cSetCode
                                    ,i.ProductName
                                    ,i.sType
                                    ,i.cTotal
                                    ,i.cTotalAll
                                    ,i.nGroupCalc
                                    ,i.sUnit
                            from TEffluentUseProduct up
                            left join mTProductIndicator i on i.ProductID = up.ProductID
                            where i.IDIndicator = " + nIndID + @" and i.cDel = 'N' and up.OperationTypeID = " + nOprtID + @" 
                            order by i.nGroupCalc";
            List<CProduct> lstPro = db.Database.SqlQuery<CProduct>(sqlPro).ToList();
            if (lstPro.Any()) result.lstProduct = lstPro;

            ///// มีข้อมูลแล้ว
            if (itemEPI_FORM != null)
            {
                var lstCheckPoint = db.TEffluent_Point.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
                var lst = db.TEffluent_Product.Where(w => w.FormID == itemEPI_FORM.FormID).ToList();
                foreach (var f in result.lstProduct)
                {
                    var q = lst.FirstOrDefault(w => w.ProductID == f.ProductID);
                    if (q != null)
                    {
                        #region DATA
                        f.M1 = q.M1;
                        f.M2 = q.M2;
                        f.M3 = q.M3;
                        f.M4 = q.M4;
                        f.M5 = q.M5;
                        f.M6 = q.M6;
                        f.M7 = q.M7;
                        f.M8 = q.M8;
                        f.M9 = q.M9;
                        f.M10 = q.M10;
                        f.M11 = q.M11;
                        f.M12 = q.M12;
                        f.Target = q.Target;
                        f.IsCheckM1 = q.IsCheckM1;
                        f.IsCheckM2 = q.IsCheckM2;
                        f.IsCheckM3 = q.IsCheckM3;
                        f.IsCheckM4 = q.IsCheckM4;
                        f.IsCheckM5 = q.IsCheckM5;
                        f.IsCheckM6 = q.IsCheckM6;
                        f.IsCheckM7 = q.IsCheckM7;
                        f.IsCheckM8 = q.IsCheckM8;
                        f.IsCheckM9 = q.IsCheckM9;
                        f.IsCheckM10 = q.IsCheckM10;
                        f.IsCheckM11 = q.IsCheckM11;
                        f.IsCheckM12 = q.IsCheckM12;
                        switch (f.ProductID)
                        {
                            case 117: //Disposal Method : Reuse
                                f.sStatus = lstCheckPoint.Where(w => w.nDischargeTo == 1).Any() ? "Y" : "N";
                                break;
                            case 118: //Disposal Method : Reuse
                                f.sStatus = lstCheckPoint.Where(w => w.nDischargeTo == 2).Any() ? "Y" : "N";
                                break;
                            case 119: //Disposal Method : Reuse
                                f.sStatus = lstCheckPoint.Where(w => w.nDischargeTo == 3).Any() ? "Y" : "N";
                                break;
                            case 120: //Disposal Method : Reuse
                                f.sStatus = lstCheckPoint.Where(w => w.nDischargeTo == 4).Any() ? "Y" : "N";
                                break;
                            case 121: //Disposal Method : Reuse
                                f.sStatus = lstCheckPoint.Where(w => w.nDischargeTo == 5).Any() ? "Y" : "N";
                                break;
                            case 122: //Disposal Method : Reuse
                                f.sStatus = lstCheckPoint.Where(w => w.nDischargeTo == 6).Any() ? "Y" : "N";
                                break;
                            case 214: //Disposal Method : Reuse
                                f.sStatus = lstCheckPoint.Where(w => w.nDischargeTo == 76).Any() ? "Y" : "N";
                                break;
                        }
                        #endregion
                    }
                    else
                    {
                        #region NODATA Product
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
                        f.IsCheckM1 = "N";
                        f.IsCheckM2 = "N";
                        f.IsCheckM3 = "N";
                        f.IsCheckM4 = "N";
                        f.IsCheckM5 = "N";
                        f.IsCheckM6 = "N";
                        f.IsCheckM7 = "N";
                        f.IsCheckM8 = "N";
                        f.IsCheckM9 = "N";
                        f.IsCheckM10 = "N";
                        f.IsCheckM11 = "N";
                        f.IsCheckM12 = "N";
                        f.sStatus = "N";
                        #endregion
                    }
                }
            }
            else
            {
                #region NODATA Product
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
                    f.IsCheckM1 = "N";
                    f.IsCheckM2 = "N";
                    f.IsCheckM3 = "N";
                    f.IsCheckM4 = "N";
                    f.IsCheckM5 = "N";
                    f.IsCheckM6 = "N";
                    f.IsCheckM7 = "N";
                    f.IsCheckM8 = "N";
                    f.IsCheckM9 = "N";
                    f.IsCheckM10 = "N";
                    f.IsCheckM11 = "N";
                    f.IsCheckM12 = "N";
                    f.sStatus = "N";
                });
                #endregion
            }

            CProduct objHeaderChekbox = new CProduct();
            var dHead = result.lstProduct.FirstOrDefault(w => w.ProductID == 115);
            if (dHead != null)
            {
                objHeaderChekbox.M1 = dHead.IsCheckM1;
                objHeaderChekbox.M2 = dHead.IsCheckM2;
                objHeaderChekbox.M3 = dHead.IsCheckM3;
                objHeaderChekbox.M4 = dHead.IsCheckM4;
                objHeaderChekbox.M5 = dHead.IsCheckM5;
                objHeaderChekbox.M6 = dHead.IsCheckM6;
                objHeaderChekbox.M7 = dHead.IsCheckM7;
                objHeaderChekbox.M8 = dHead.IsCheckM8;
                objHeaderChekbox.M9 = dHead.IsCheckM9;
                objHeaderChekbox.M10 = dHead.IsCheckM10;
                objHeaderChekbox.M11 = dHead.IsCheckM11;
                objHeaderChekbox.M12 = dHead.IsCheckM12;
            }
            #endregion

            int nFormID = int.Parse(result.EPI_FORMID);
            var qRemark = db.TEffluent_Remark.Where(w => w.FormID == nFormID).OrderByDescending(o => o.nVersion).FirstOrDefault();
            if (qRemark != null) result.sRemarkTotal = qRemark.sRemark;

            #region POINT
            string sqlPointIN = @"select p.nPointID
                                  ,p.sPointName
                                  ,p.sOption2 as cCOD                                   
                                  ,CONVERT(varchar,p.nDischargeTo) as sDisChargeID
                                  ,CONVERT(varchar,p.nTreamentMethod) as sTreatmentID
                                  ,p.sOtherTreamentMethod
                                  ,CONVERT(varchar,p.nArea) as sAreaID
                                  ,p.sPointType as sTypePoint
                                  ,p.sRemark
                                  ,dt.sName as sTreatmentName
	                              ,da.sName as sAreaName
	                              ,dd.sName as sDisChargeName
                                  ,'Y' as cStatus
                                  ,sOtherTreamentMethod as sTreatmentOther
                                  ,'N' as cNew
                              from TEffluent_Point p
                              left join TData_Type dt on dt.nID = p.nTreamentMethod and dt.IndicatorID = 3
                              left join TData_Type da on da.nID = p.nArea and da.IndicatorID = 3
                              left join TData_Type dd on dd.nID = p.nDischargeTo and dd.IndicatorID = 3
                              where p.FormID = " + nFormID + "";
            List<CPointType> lstPointInput = db.Database.SqlQuery<CPointType>(sqlPointIN).ToList();
            if (lstPointInput.Any())
            {
                result.lstPointInput = lstPointInput;
                result.lstPointInput.ForEach(f =>
                {
                    if (f.sTreatmentID == "999") f.sTreatmentName = "Other";
                });
            }

            string sqlPoint = @"SELECT nPointID
                                      ,ProductID
                                      ,M1
                                      ,M2
                                      ,M3
                                      ,M4
                                      ,M5
                                      ,M6
                                      ,M7
                                      ,M8
                                      ,M9
                                      ,M10
                                      ,M11
                                      ,M12
                                      ,Target
                                  FROM TEffluent_Product_Point
                                  where FormID = " + nFormID + "";
            List<TData_Point> lstPoint = db.Database.SqlQuery<TData_Point>(sqlPoint).ToList();
            if (lstPoint.Any())
            {
                result.lstPoint = lstPoint;
                result.lstPoint.ForEach(f =>
                    {
                        var q = lstPointInput.FirstOrDefault(w => w.nPointID == f.nPointID);
                        if (q != null) f.sTypePoint = q.sTypePoint;
                        else f.sTypePoint = "";
                        f.IsCheckM1 = objHeaderChekbox.M1;
                        f.IsCheckM2 = objHeaderChekbox.M2;
                        f.IsCheckM3 = objHeaderChekbox.M3;
                        f.IsCheckM4 = objHeaderChekbox.M4;
                        f.IsCheckM5 = objHeaderChekbox.M5;
                        f.IsCheckM6 = objHeaderChekbox.M6;
                        f.IsCheckM7 = objHeaderChekbox.M7;
                        f.IsCheckM8 = objHeaderChekbox.M8;
                        f.IsCheckM9 = objHeaderChekbox.M9;
                        f.IsCheckM10 = objHeaderChekbox.M10;
                        f.IsCheckM11 = objHeaderChekbox.M11;
                        f.IsCheckM12 = objHeaderChekbox.M12;
                    });
            }
            #endregion

            #region OTHER
            string sqlOther = @"SELECT FormID,nPointID
                              ,nOtherID
                              ,CONVERT(varchar,nProductID) as sProductID 
                              ,CONVERT(varchar,nUnitID) as sUnitID 
                              ,M1
                              ,M2
                              ,M3
                              ,M4
                              ,M5
                              ,M6
                              ,M7
                              ,M8
                              ,M9
                              ,M10
                              ,M11
                              ,M12
                              ,Target  
                              ,'N' as cNew                           
                          FROM TEffluent_OtherProduct_Point
                          where FormID = " + nFormID + "";
            List<OtherPoint> lstOther = db.Database.SqlQuery<OtherPoint>(sqlOther).ToList();
            if (lstOther.Any())
            {
                result.lstOther = lstOther;
                result.lstOther.ForEach(f =>
                {
                    var q = lstPointInput.FirstOrDefault(w => w.nPointID == f.nPointID);
                    if (q != null) f.sTypePoint = q.sTypePoint;
                    else f.sTypePoint = "";
                    f.sStatus = "Y";
                });
            }
            #endregion

            var qOption = db.TEffluent_Option.FirstOrDefault(w => w.FormID == nFormID);
            if (qOption != null)
            {
                result.sNumPoint = qOption.nNumberOfPoint + "";
                result.sPercentWater = qOption.sPercentOfPoint;
            }

            result.lstIndicatorOther = GetTM_Data("I");
            result.lstUnitOther = GetTM_Data("U");

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
    public static CReturnWater SetDataWater(CParam param, string sPercent)
    {
        CReturnWater r = new CReturnWater();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            int nOprtID = SystemFunction.GetIntNullToZero(param.sOprtID);
            int nFacID = SystemFunction.GetIntNullToZero(param.sFacID);
            string sYear = param.sYear;

            int? nPercent = null;
            string Percent = "";
            if (sPercent == "N/A") Percent = "";
            else if (!string.IsNullOrEmpty(sPercent))
            {
                Percent = sPercent;
            }


            var itemEPI_FORM = db.TEPI_Forms.FirstOrDefault(w => w.sYear == sYear && w.IDIndicator == 11 && w.OperationTypeID == nOprtID && w.FacilityID == nFacID);
            if (itemEPI_FORM != null)
            {
                var q = db.TWater_Product.FirstOrDefault(w => w.FormID == itemEPI_FORM.FormID && w.ProductID == 91);
                if (q != null)
                {
                    r.M1 = CalWater(Percent, q.M1);
                    r.M2 = CalWater(Percent, q.M2);
                    r.M3 = CalWater(Percent, q.M3);
                    r.M4 = CalWater(Percent, q.M4);
                    r.M5 = CalWater(Percent, q.M5);
                    r.M6 = CalWater(Percent, q.M6);
                    r.M7 = CalWater(Percent, q.M7);
                    r.M8 = CalWater(Percent, q.M8);
                    r.M9 = CalWater(Percent, q.M9);
                    r.M10 = CalWater(Percent, q.M10);
                    r.M11 = CalWater(Percent, q.M11);
                    r.M12 = CalWater(Percent, q.M12);
                }
                else
                {
                    r.M1 = "";
                    r.M2 = "";
                    r.M3 = "";
                    r.M4 = "";
                    r.M5 = "";
                    r.M6 = "";
                    r.M7 = "";
                    r.M8 = "";
                    r.M9 = "";
                    r.M10 = "";
                    r.M11 = "";
                    r.M12 = "";
                }
            }
            else
            {
                r.M1 = "";
                r.M2 = "";
                r.M3 = "";
                r.M4 = "";
                r.M5 = "";
                r.M6 = "";
                r.M7 = "";
                r.M8 = "";
                r.M9 = "";
                r.M10 = "";
                r.M11 = "";
                r.M12 = "";
            }

            r.Status = SystemFunction.process_Success;
        }
        else
        {
            r.Status = SystemFunction.process_SessionExpired;
        }
        return r;
    }
    public static string CalWater(string sPerCent, string sMonth)
    {
        string sVal = "";
        if (sPerCent != "")
        {
            if (!string.IsNullOrEmpty(sMonth))
            {
                decimal? sM = EPIFunc.GetDecimalNull(sMonth);// SystemFunction.ConvertFormatDecimal4(sMonth);
                decimal? sP = EPIFunc.GetDecimalNull(sPerCent + "");
                decimal? nVal = (sP * sM) / 100;
                sVal = nVal + "";
            }
        }

        return sVal;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod saveToDB(CSaveToDB arrData)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();

        if (!UserAcc.UserExpired())
        {
            DateTime now = DateTime.Now;
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
                #region Product - (TYPE IN 1 SP 2)
                var lstP = arrData.lstProduct.Where(w => w.sType != "2H");
                foreach (var d in arrData.lstProduct)
                {
                    TEffluent_Product eff = new TEffluent_Product();
                    bool isEffNew = false;
                    eff = db.TEffluent_Product.FirstOrDefault(w => w.FormID == FORM_ID && w.ProductID == d.ProductID);
                    if (eff == null)
                    {
                        isEffNew = true;
                        eff = new TEffluent_Product();
                        eff.FormID = FORM_ID;
                        eff.ProductID = d.ProductID.Value;
                        eff.UnitID = 0;
                    }
                    eff.M1 = d.M1;
                    eff.M2 = d.M2;
                    eff.M3 = d.M3;
                    eff.M4 = d.M4;
                    eff.M5 = d.M5;
                    eff.M6 = d.M6;
                    eff.M7 = d.M7;
                    eff.M8 = d.M8;
                    eff.M9 = d.M9;
                    eff.M10 = d.M10;
                    eff.M11 = d.M11;
                    eff.M12 = d.M12;
                    eff.Target = d.Target;
                    eff.IsCheckM1 = d.IsCheckM1;
                    eff.IsCheckM2 = d.IsCheckM2;
                    eff.IsCheckM3 = d.IsCheckM3;
                    eff.IsCheckM4 = d.IsCheckM4;
                    eff.IsCheckM5 = d.IsCheckM5;
                    eff.IsCheckM6 = d.IsCheckM6;
                    eff.IsCheckM7 = d.IsCheckM7;
                    eff.IsCheckM8 = d.IsCheckM8;
                    eff.IsCheckM9 = d.IsCheckM9;
                    eff.IsCheckM10 = d.IsCheckM10;
                    eff.IsCheckM11 = d.IsCheckM11;
                    eff.IsCheckM12 = d.IsCheckM12;
                    if (isEffNew) db.TEffluent_Product.Add(eff);
                }
                db.SaveChanges();
                #endregion

                #region POINT
                string sqlDel = @"delete from TEffluent_Point where FormID = " + FORM_ID + @"
                                delete from TEffluent_Product_Point where FormID = " + FORM_ID + @"
                                delete from TEffluent_OtherProduct_Point where FormID = " + FORM_ID + @"";
                CommonFunction.ExecuteSQL(SystemFunction.strConnect, sqlDel);

                var lstPointHead = arrData.lstPointInput.Where(w => w.cStatus != "N").ToList();
                int nPointID = 1;
                foreach (var p in lstPointHead)
                {
                    #region POINT MAIN
                    int nDischargeTo = int.Parse(p.sDisChargeID);
                    int nTreamentMethod = int.Parse(p.sTreatmentID);
                    int nArea = int.Parse(p.sAreaID);

                    TEffluent_Point effP = new TEffluent_Point();
                    bool isEffPNew = false;
                    effP = db.TEffluent_Point.FirstOrDefault(w => w.FormID == FORM_ID && w.nPointID == nPointID); //p.nPointID
                    if (effP == null)
                    {
                        isEffPNew = true;
                        effP = new TEffluent_Point();
                        effP.FormID = FORM_ID;
                        effP.nPointID = nPointID;
                        effP.nAddBy = UserAcc.GetObjUser().nUserID;
                        effP.dAdd = now;
                    }
                    //string sOption2 = p.cCOD == "U" ? "1" : "2";
                    effP.sPointName = p.sPointName;
                    effP.sOption1 = "C";
                    effP.sOption2 = p.cCOD; // sOption2; //
                    effP.sPercent = "";
                    effP.nDischargeTo = nDischargeTo;
                    effP.nTreamentMethod = nTreamentMethod;
                    effP.sOtherTreamentMethod = nTreamentMethod == 999 ? p.sTreatmentOther : "";
                    effP.nArea = nArea;
                    effP.sPointType = p.sTypePoint;
                    effP.sRemark = p.sRemark;
                    effP.sPercent = arrData.sPercentWater;
                    if (isEffPNew) db.TEffluent_Point.Add(effP);
                    db.SaveChanges();
                    #endregion

                    #region POINT DATA
                    var lstPointSub = arrData.lstPoitn.Where(w => w.nPointID == p.nPointID && w.sTypePoint == p.sTypePoint).ToList();
                    foreach (var pp in lstPointSub)
                    {
                        TEffluent_Product_Point effPP = new TEffluent_Product_Point();
                        bool isEffPPNew = false;
                        effPP = db.TEffluent_Product_Point.FirstOrDefault(w => w.FormID == FORM_ID && w.nPointID == nPointID);
                        if (effPP == null)
                        {
                            isEffPPNew = true;
                            effPP = new TEffluent_Product_Point();
                            effPP.FormID = FORM_ID;
                            effPP.nPointID = nPointID;
                            effPP.ProductID = pp.ProductID;
                            effPP.UnitID = 0;
                        }
                        bool isPass = true;
                        if (p.cCOD == "1") //p.cCOD == "U"
                        {
                            if (pp.ProductID == 128) isPass = false;
                        }
                        else
                        {
                            if (pp.ProductID == 127) isPass = false;
                        }

                        if (isPass)
                        {
                            effPP.M1 = pp.M1;
                            effPP.M2 = pp.M2;
                            effPP.M3 = pp.M3;
                            effPP.M4 = pp.M4;
                            effPP.M5 = pp.M5;
                            effPP.M6 = pp.M6;
                            effPP.M7 = pp.M7;
                            effPP.M8 = pp.M8;
                            effPP.M9 = pp.M9;
                            effPP.M10 = pp.M10;
                            effPP.M11 = pp.M11;
                            effPP.M12 = pp.M12;
                            effPP.Target = pp.Target;
                        }
                        else
                        {
                            effPP.M1 = "";
                            effPP.M2 = "";
                            effPP.M3 = "";
                            effPP.M4 = "";
                            effPP.M5 = "";
                            effPP.M6 = "";
                            effPP.M7 = "";
                            effPP.M8 = "";
                            effPP.M9 = "";
                            effPP.M10 = "";
                            effPP.M11 = "";
                            effPP.M12 = "";
                            effPP.Target = "";
                        }

                        if (isEffPPNew) db.TEffluent_Product_Point.Add(effPP);
                    }
                    db.SaveChanges();
                    #endregion

                    #region OTHER
                    var lstOther = arrData.lstProductOther.Where(w => w.nPointID == p.nPointID && w.sTypePoint == p.sTypePoint && w.sStatus != "N").ToList();
                    int nOtherID = 1;
                    foreach (var ot in lstOther)
                    {
                        // int nOtherID = int.Parse(ot.sOtherID);
                        int? nProducID = null;
                        if (!string.IsNullOrEmpty(ot.sProductID)) nProducID = int.Parse(ot.sProductID);
                        int? nUnitID = null;
                        if (!string.IsNullOrEmpty(ot.sUnitID)) nUnitID = int.Parse(ot.sUnitID);

                        TEffluent_OtherProduct_Point effOther = new TEffluent_OtherProduct_Point();
                        bool isOtherNew = false;
                        effOther = db.TEffluent_OtherProduct_Point.FirstOrDefault(w => w.FormID == FORM_ID && w.nPointID == nPointID && w.nOtherID == ot.nOtherID);
                        if (effOther == null)
                        {
                            isOtherNew = true;
                            effOther = new TEffluent_OtherProduct_Point();
                            effOther.FormID = FORM_ID;
                            effOther.nPointID = nPointID;
                            effOther.nOtherID = nOtherID; //ot.nOtherID;
                        }
                        effOther.nProductID = nProducID;
                        effOther.nUnitID = nUnitID;
                        effOther.M1 = ot.M1;
                        effOther.M2 = ot.M2;
                        effOther.M3 = ot.M3;
                        effOther.M4 = ot.M4;
                        effOther.M5 = ot.M5;
                        effOther.M6 = ot.M6;
                        effOther.M7 = ot.M7;
                        effOther.M8 = ot.M8;
                        effOther.M9 = ot.M9;
                        effOther.M10 = ot.M10;
                        effOther.M11 = ot.M11;
                        effOther.M12 = ot.M12;
                        effOther.Target = ot.Target;
                        if (isOtherNew) db.TEffluent_OtherProduct_Point.Add(effOther);
                        nOtherID++;
                    }
                    #endregion
                    nPointID++;
                    db.SaveChanges();
                }

                #endregion

                #region OPTION
                int? nNumPoint = null;
                if (!string.IsNullOrEmpty(arrData.sNumPoint)) nNumPoint = int.Parse(arrData.sNumPoint);

                TEffluent_Option op = new TEffluent_Option();
                bool idOpNew = false;
                int nn = FORM_ID;
                op = db.TEffluent_Option.FirstOrDefault(w => w.FormID == FORM_ID && w.ProductID == 115);
                if (op == null)
                {
                    idOpNew = true;
                    op = new TEffluent_Option();
                    op.FormID = nn;
                    op.ProductID = 115;
                }
                op.sOption = "3";
                op.sValuesPercent = arrData.sPercentWater;
                op.sPercentOfPoint = arrData.sPercentWater;
                op.nNumberOfPoint = nNumPoint;
                if (idOpNew) db.TEffluent_Option.Add(op);
                db.SaveChanges();
                #endregion

                #region Remark
                if (!string.IsNullOrEmpty(arrData.sRemarkTotal))
                {
                    var itemRemark = db.TEffluent_Remark.Where(w => w.FormID == FORM_ID && w.ProductID == 115);
                    int nVersion = itemRemark.Any() ? itemRemark.Max(m => m.nVersion) + 1 : 1;
                    TEffluent_Remark Rmrk = new TEffluent_Remark();
                    Rmrk.FormID = FORM_ID;
                    Rmrk.ProductID = 115;
                    Rmrk.nVersion = nVersion;
                    Rmrk.sRemark = arrData.sRemarkTotal;
                    if (itemRemark.Count() > 0)
                    {
                        if (!SystemFunction.IsSuperAdmin())
                        {
                            Rmrk.sAddBy = UserAcc.GetObjUser().nUserID;
                            Rmrk.dAddDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        Rmrk.sAddBy = UserAcc.GetObjUser().nUserID;
                        Rmrk.dAddDate = DateTime.Now;
                    }
                    db.TEffluent_Remark.Add(Rmrk);
                }

                db.SaveChanges();
                #endregion

                #region EFF - Product Type = 2H

                //Checkbox Allowed data to zero(0)
                CProduct objHeaderChekbox = new CProduct();
                var dHead = arrData.lstProduct.FirstOrDefault(w => w.sType == "1" && w.sStatus == "Y");
                if (dHead != null)
                {
                    objHeaderChekbox.M1 = dHead.IsCheckM1;
                    objHeaderChekbox.M2 = dHead.IsCheckM2;
                    objHeaderChekbox.M3 = dHead.IsCheckM3;
                    objHeaderChekbox.M4 = dHead.IsCheckM4;
                    objHeaderChekbox.M5 = dHead.IsCheckM5;
                    objHeaderChekbox.M6 = dHead.IsCheckM6;
                    objHeaderChekbox.M7 = dHead.IsCheckM7;
                    objHeaderChekbox.M8 = dHead.IsCheckM8;
                    objHeaderChekbox.M9 = dHead.IsCheckM9;
                    objHeaderChekbox.M10 = dHead.IsCheckM10;
                    objHeaderChekbox.M11 = dHead.IsCheckM11;
                    objHeaderChekbox.M12 = dHead.IsCheckM12;
                }

                //Product ที่จะเก็บลง DB
                var qProduct2H = (from i in db.mTProductIndicator.Where(w => w.IDIndicator.Value == arrData.nIndicatorID && w.cDel == "N" && w.sType == "2H")
                                  from iu in db.TEffluentUseProduct.Where(w => w.OperationTypeID == arrData.nOperationID && w.ProductID == i.ProductID)
                                  select new
                                  {
                                      i.ProductID,
                                      i.sType,
                                      i.nGroupCalc,
                                      i.nOrder
                                  }).OrderBy(o => o.nOrder).ToList();

                foreach (var item in qProduct2H)
                {
                    string sProductID = "", sH1 = "", sH2 = "", sTarget = "", sM1 = "", sM2 = "", sM3 = "", sM4 = "", sM5 = "", sM6 = "", sM7 = "", sM8 = "", sM9 = "", sM10 = "", sM11 = "", sM12 = "";
                    //Get Sub Product >> แยกตาม nGroupCalc >> เฉพาะ ProductID 127 = Average COD concentration(Ton) ไม่ต้อง convert แต่ product อื่นๆที่มีหน่วยเป็น mg/l ให้เป็น Ton ก่อนถึงจะรวมกันได้
                    var qSubProduct = (from i in db.mTProductIndicator.Where(w => w.IDIndicator.Value == arrData.nIndicatorID && w.cDel == "N" && w.sType == "2" && w.nGroupCalc == item.nGroupCalc)// && w.ProductID != 128)
                                       from iu in db.TEffluentUseProduct.Where(w => w.OperationTypeID == arrData.nOperationID && w.ProductID == i.ProductID)
                                       select new
                                       {
                                           i.ProductID
                                       }).ToList();

                    //Get Data Point To For Sum
                    if (qSubProduct.Count() > 0)
                    {  //// 124 = Flow rate 125 = Operating hours
                        var qData1 = db.TEffluent_Product_Point.Where(w => w.FormID == FORM_ID && (w.ProductID == 124 || w.ProductID == 125)).ToList();

                        //**** แปลงหน่วยแยกแต่ละ Point ก่อนแล้วค่อยนำมารวมกัน ****
                        var qDataProductPoint = db.TEffluent_Product_Point.AsEnumerable().Where(w => w.FormID == FORM_ID && qSubProduct.Where(x => x.ProductID == w.ProductID).Count() > 0).Select(s => new
                        {
                            Target = s.ProductID == 127 ? s.Target : SysFunctionCalculate.EffluentConvertMGL2Ton(new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSPorductFlowrate, 0), new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSProductOperatinghours, 0), s.Target + "") + "",
                            M1 = s.ProductID == 127 ? s.M1 : SysFunctionCalculate.EffluentConvertMGL2Ton(new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSPorductFlowrate, 1), new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSProductOperatinghours, 1), s.M1 + "") + "",
                            M2 = s.ProductID == 127 ? s.M2 : SysFunctionCalculate.EffluentConvertMGL2Ton(new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSPorductFlowrate, 2), new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSProductOperatinghours, 2), s.M2 + "") + "",
                            M3 = s.ProductID == 127 ? s.M3 : SysFunctionCalculate.EffluentConvertMGL2Ton(new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSPorductFlowrate, 3), new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSProductOperatinghours, 3), s.M3 + "") + "",
                            M4 = s.ProductID == 127 ? s.M4 : SysFunctionCalculate.EffluentConvertMGL2Ton(new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSPorductFlowrate, 4), new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSProductOperatinghours, 4), s.M4 + "") + "",
                            M5 = s.ProductID == 127 ? s.M5 : SysFunctionCalculate.EffluentConvertMGL2Ton(new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSPorductFlowrate, 5), new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSProductOperatinghours, 5), s.M5 + "") + "",
                            M6 = s.ProductID == 127 ? s.M6 : SysFunctionCalculate.EffluentConvertMGL2Ton(new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSPorductFlowrate, 6), new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSProductOperatinghours, 6), s.M6 + "") + "",
                            M7 = s.ProductID == 127 ? s.M7 : SysFunctionCalculate.EffluentConvertMGL2Ton(new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSPorductFlowrate, 7), new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSProductOperatinghours, 7), s.M7 + "") + "",
                            M8 = s.ProductID == 127 ? s.M8 : SysFunctionCalculate.EffluentConvertMGL2Ton(new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSPorductFlowrate, 8), new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSProductOperatinghours, 8), s.M8 + "") + "",
                            M9 = s.ProductID == 127 ? s.M9 : SysFunctionCalculate.EffluentConvertMGL2Ton(new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSPorductFlowrate, 9), new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSProductOperatinghours, 9), s.M9 + "") + "",
                            M10 = s.ProductID == 127 ? s.M10 : SysFunctionCalculate.EffluentConvertMGL2Ton(new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSPorductFlowrate, 10), new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSProductOperatinghours, 10), s.M10 + "") + "",
                            M11 = s.ProductID == 127 ? s.M11 : SysFunctionCalculate.EffluentConvertMGL2Ton(new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSPorductFlowrate, 11), new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSProductOperatinghours, 11), s.M11 + "") + "",
                            M12 = s.ProductID == 127 ? s.M12 : SysFunctionCalculate.EffluentConvertMGL2Ton(new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSPorductFlowrate, 12), new epi_input_effluent().GetDataForCalculate2H(qData1, s.nPointID, nMSProductOperatinghours, 12), s.M12 + "") + ""
                        }).ToList();

                        sTarget = EPIFunc.SumDataToDecimal(qDataProductPoint.Select(s => s.Target).ToList()) + "";
                        sM1 = EPIFunc.SumDataToDecimal(qDataProductPoint.Select(s => s.M1).ToList()) + "";
                        sM2 = EPIFunc.SumDataToDecimal(qDataProductPoint.Select(s => s.M2).ToList()) + "";
                        sM3 = EPIFunc.SumDataToDecimal(qDataProductPoint.Select(s => s.M3).ToList()) + "";
                        sM4 = EPIFunc.SumDataToDecimal(qDataProductPoint.Select(s => s.M4).ToList()) + "";
                        sM5 = EPIFunc.SumDataToDecimal(qDataProductPoint.Select(s => s.M5).ToList()) + "";
                        sM6 = EPIFunc.SumDataToDecimal(qDataProductPoint.Select(s => s.M6).ToList()) + "";
                        sM7 = EPIFunc.SumDataToDecimal(qDataProductPoint.Select(s => s.M7).ToList()) + "";
                        sM8 = EPIFunc.SumDataToDecimal(qDataProductPoint.Select(s => s.M8).ToList()) + "";
                        sM9 = EPIFunc.SumDataToDecimal(qDataProductPoint.Select(s => s.M9).ToList()) + "";
                        sM10 = EPIFunc.SumDataToDecimal(qDataProductPoint.Select(s => s.M10).ToList()) + "";
                        sM11 = EPIFunc.SumDataToDecimal(qDataProductPoint.Select(s => s.M11).ToList()) + "";
                        sM12 = EPIFunc.SumDataToDecimal(qDataProductPoint.Select(s => s.M12).ToList()) + "";
                    }

                    //******** Save To Unit Ton For Calculate Output ***************
                    new epi_input_effluent().Update_TEffluentProduct(arrData.sYear, FORM_ID, item.ProductID, sM1, sM2, sM3, sM4, sM5, sM6, sM7, sM8, sM9, sM10, sM11, sM12, 0, sTarget, "", objHeaderChekbox);
                }
                db.SaveChanges();
                #endregion

                new EPIFunc().RecalculateEffluent(arrData.nOperationID, arrData.nFacilityID, arrData.sYear);
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
                result = new Workflow().WorkFlowAction(FORM_ID, arrData.lstMonthSubmit, sMode, UserAcc.GetObjUser().nUserID, UserAcc.GetObjUser().nRoleID, arrData.sRemarkTotal);
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

    private string GetDataForCalculate2H(List<TEffluent_Product_Point> lstdata, int nPointID, int nProductID, int nMonth)
    {
        string sresult = "";
        var query = lstdata.Where(w => w.nPointID == nPointID && w.ProductID == nProductID).FirstOrDefault();
        if (query != null)
        {
            switch (nMonth)
            {
                case 0: sresult = query.Target; break;
                case 1: sresult = query.M1; break;
                case 2: sresult = query.M2; break;
                case 3: sresult = query.M3; break;
                case 4: sresult = query.M4; break;
                case 5: sresult = query.M5; break;
                case 6: sresult = query.M6; break;
                case 7: sresult = query.M7; break;
                case 8: sresult = query.M8; break;
                case 9: sresult = query.M9; break;
                case 10: sresult = query.M10; break;
                case 11: sresult = query.M11; break;
                case 12: sresult = query.M12; break;
            }
        }
        return sresult;
    }

    private void Update_TEffluentProduct(string _sYear, int _EPIFromID, int _sProductID, string _M1, string _M2, string _M3, string _M4, string _M5, string _M6
, string _M7, string _M8, string _M9, string _M10, string _M11, string _M12, int _UnitID, string _Target, string _sFactor, CProduct objHeaderCheckbox)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var query = db.TEffluent_Product.Where(w => w.FormID == _EPIFromID && w.ProductID == _sProductID).FirstOrDefault();
        if (query != null)
        {
            query.UnitID = _UnitID;
            query.Target = SystemFunction.ConvertExponentialToString(_Target.Replace(",", ""));
            query.M1 = SystemFunction.ConvertExponentialToString(_M1.Replace(",", ""));
            query.M2 = SystemFunction.ConvertExponentialToString(_M2.Replace(",", ""));
            query.M3 = SystemFunction.ConvertExponentialToString(_M3.Replace(",", ""));
            query.M4 = SystemFunction.ConvertExponentialToString(_M4.Replace(",", ""));
            query.M5 = SystemFunction.ConvertExponentialToString(_M5.Replace(",", ""));
            query.M6 = SystemFunction.ConvertExponentialToString(_M6.Replace(",", ""));
            query.M7 = SystemFunction.ConvertExponentialToString(_M7.Replace(",", ""));
            query.M8 = SystemFunction.ConvertExponentialToString(_M8.Replace(",", ""));
            query.M9 = SystemFunction.ConvertExponentialToString(_M9.Replace(",", ""));
            query.M10 = SystemFunction.ConvertExponentialToString(_M10.Replace(",", ""));
            query.M11 = SystemFunction.ConvertExponentialToString(_M11.Replace(",", ""));
            query.M12 = SystemFunction.ConvertExponentialToString(_M12.Replace(",", ""));

            //เพื่อนำมาเช็ค Header In Grid
            query.IsCheckM1 = objHeaderCheckbox.M1;
            query.IsCheckM2 = objHeaderCheckbox.M2;
            query.IsCheckM3 = objHeaderCheckbox.M3;
            query.IsCheckM4 = objHeaderCheckbox.M4;
            query.IsCheckM5 = objHeaderCheckbox.M5;
            query.IsCheckM6 = objHeaderCheckbox.M6;
            query.IsCheckM7 = objHeaderCheckbox.M7;
            query.IsCheckM8 = objHeaderCheckbox.M8;
            query.IsCheckM9 = objHeaderCheckbox.M9;
            query.IsCheckM10 = objHeaderCheckbox.M10;
            query.IsCheckM11 = objHeaderCheckbox.M11;
            query.IsCheckM12 = objHeaderCheckbox.M12;

        }
        else
        {
            TEffluent_Product t = new TEffluent_Product();
            t.FormID = _EPIFromID;
            t.ProductID = _sProductID;
            t.UnitID = _UnitID;
            t.Target = SystemFunction.ConvertExponentialToString(_Target.Replace(",", ""));
            t.M1 = SystemFunction.ConvertExponentialToString(_M1.Replace(",", ""));
            t.M2 = SystemFunction.ConvertExponentialToString(_M2.Replace(",", ""));
            t.M3 = SystemFunction.ConvertExponentialToString(_M3.Replace(",", ""));
            t.M4 = SystemFunction.ConvertExponentialToString(_M4.Replace(",", ""));
            t.M5 = SystemFunction.ConvertExponentialToString(_M5.Replace(",", ""));
            t.M6 = SystemFunction.ConvertExponentialToString(_M6.Replace(",", ""));
            t.M7 = SystemFunction.ConvertExponentialToString(_M7.Replace(",", ""));
            t.M8 = SystemFunction.ConvertExponentialToString(_M8.Replace(",", ""));
            t.M9 = SystemFunction.ConvertExponentialToString(_M9.Replace(",", ""));
            t.M10 = SystemFunction.ConvertExponentialToString(_M10.Replace(",", ""));
            t.M11 = SystemFunction.ConvertExponentialToString(_M11.Replace(",", ""));
            t.M12 = SystemFunction.ConvertExponentialToString(_M12.Replace(",", ""));

            //เพื่อนำมาเช็ค Header In Grid
            t.IsCheckM1 = objHeaderCheckbox.M1;
            t.IsCheckM2 = objHeaderCheckbox.M2;
            t.IsCheckM3 = objHeaderCheckbox.M3;
            t.IsCheckM4 = objHeaderCheckbox.M4;
            t.IsCheckM5 = objHeaderCheckbox.M5;
            t.IsCheckM6 = objHeaderCheckbox.M6;
            t.IsCheckM7 = objHeaderCheckbox.M7;
            t.IsCheckM8 = objHeaderCheckbox.M8;
            t.IsCheckM9 = objHeaderCheckbox.M9;
            t.IsCheckM10 = objHeaderCheckbox.M10;
            t.IsCheckM11 = objHeaderCheckbox.M11;
            t.IsCheckM12 = objHeaderCheckbox.M12;

            db.TEffluent_Product.Add(t);
        }
        db.SaveChanges();
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

            CParam item = new CParam();
            item.sYear = ((_MP_EPI_FORMS)this.Master).Year;
            item.sOprtID = ((_MP_EPI_FORMS)this.Master).OperationType;
            item.sIndID = ((_MP_EPI_FORMS)this.Master).Indicator;
            item.sFacID = ((_MP_EPI_FORMS)this.Master).Facility;

            CReturnData dd = LoadData(item);

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

            XLWorkbook workbook = GetWorkbookExcel(dd.lstProduct, dd.lstPointInput, dd.lstPoint, dd.sRemarkTotal, dd.sNumPoint, dd.sPercentWater, dd.lstOther, saveAsFileName
                , sIndicatorName, sOName, sFacility, ((_MP_EPI_FORMS)this.Master).Year, lstDeviate);

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
    public static XLWorkbook GetWorkbookExcel(List<CProduct> lstProduct, List<CPointType> lstPointHead
        , List<TData_Point> lstPoint, string sRemarkTotal, string sNumPoint, string sPercent, List<OtherPoint> lstOther
        , string sFileName, string sIndicatorName, string sOName, string sFacility, string sYear, List<sysGlobalClass.TDeviate> lstDeviate)
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

        #region PRODUCT HEAD SHEET 1

        //row number must be between 1 and 1048576
        int nRownumber = 1;
        //Column number must be between 1 and 16384
        int nColnumber = 1;
        IXLWorksheet worksheet = workbook.Worksheets.Add("Effluent");
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
        worksheet.Column(16).Width = 50;

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
        #region Product

        #region HEAD
        nRownumber++;
        worksheet.Row(nRownumber).Height = 30;
        irow = worksheet.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Center, true, "Effluent", "0");

        nRownumber++;
        irow = worksheet.Row(nRownumber);
        irow.Height = 22.20;

        nColnumber = 1;

        SetHeadStyle(irow, nColnumber++, "Product");
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
        var lst = lstProduct.Where(w => w.cTotal == "Y").ToList();
        foreach (var item in lst)
        {
            string sColor = item.cTotal == "Y" && item.cTotalAll == "N" ? "#fabd4f" : item.nGroupCalc == 99 ? "#fabd4f" : item.nGroupCalc == 12 ? "#fabd4f" : item.cTotal == "Y" && item.cTotalAll == "Y" ? "#dbea97" : "#ffffff";

            ////// TOTAL
            if (item.sType == "1")
            {
                nColnumber = 1;
                nRownumber++;
                irow = worksheet.Row(nRownumber);
                irow.Height = 17.40;

                string sUnit = item.sUnit.Replace("<sup>", "").Replace("</sup>", "");
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.ProductName, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sUnit, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.Target, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M1, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M2, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M3, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M4, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M5, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M6, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M7, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M8, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M9, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M10, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M11, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M12, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, sRemarkTotal, "", sColor);
            }
            else if (item.sStatus == "Y")
            {
                nColnumber = 1;
                nRownumber++;
                irow = worksheet.Row(nRownumber);
                irow.Height = 17.40;
                string sUnit = item.sUnit.Replace("<sup>", "").Replace("</sup>", "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.ProductName, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sUnit, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.Target, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M1, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M2, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M3, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M4, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M5, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M6, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M7, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M8, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M9, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M10, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M11, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.M12, "");
                SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
            }
        }
        #endregion

        #endregion

        #region POINT
        IXLWorksheet ws3 = workbook.Worksheets.Add("Point");
        nRownumber = 1;
        nColnumber = 1;

        ws3.Column(1).Width = 20;
        ws3.Column(2).Width = 20;
        ws3.Column(3).Width = 50;
        ws3.Column(4).Width = 30;
        ws3.Column(5).Width = 30;
        ws3.Column(6).Width = 30;
        ws3.Column(7).Width = 30;
        ws3.Column(8).Width = 50;
        ws3.Column(9).Width = 30;
        ws3.Column(10).Width = 20;
        ws3.Column(11).Width = 20;
        ws3.Column(12).Width = 20;
        ws3.Column(13).Width = 20;
        ws3.Column(14).Width = 20;
        ws3.Column(15).Width = 20;
        ws3.Column(16).Width = 20;
        ws3.Column(17).Width = 20;
        ws3.Column(18).Width = 20;
        ws3.Column(19).Width = 20;
        ws3.Column(20).Width = 20;
        ws3.Column(21).Width = 20;
        ws3.Column(22).Width = 30;

        ws3.Row(nRownumber).Height = 30;
        irow = ws3.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Indicator : " + sIndicatorName, "0");
        nRownumber++;

        ws3.Row(nRownumber).Height = 30;
        irow = ws3.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Operation Type : " + sOName, "0");
        nRownumber++;

        ws3.Row(nRownumber).Height = 30;
        irow = ws3.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Sub - facility : " + sFacility, "0");
        nRownumber++;

        ws3.Row(nRownumber).Height = 30;
        irow = ws3.Row(nRownumber);
        SetBodyStyleHeader(irow, 1, XLCellValues.Text, XLAlignmentHorizontalValues.Left, true, "Year : " + sYear, "0");
        nRownumber++;

        irow = ws3.Row(nRownumber);
        irow.Height = 22.20;

        nColnumber = 1;

        SetHeadStyle(irow, nColnumber++, "Number of point");
        SetHeadStyle(irow, nColnumber++, "% of water withdraw");
        SetHeadStyle(irow, nColnumber++, "Point Name");
        SetHeadStyle(irow, nColnumber++, "Option");
        SetHeadStyle(irow, nColnumber++, "Treament Method");
        SetHeadStyle(irow, nColnumber++, "Discharge to");
        SetHeadStyle(irow, nColnumber++, "Area");
        SetHeadStyle(irow, nColnumber++, "Product");
        SetHeadStyle(irow, nColnumber++, "Unit");
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

        if (lstPointHead.Any())
        {

            for (int i = 0; i < lstPointHead.Count(); i++)
            {
                var lstP = lstProduct.Where(w => w.sType == "2").ToList();
                string sTreatmentName = lstPointHead[i].sTreatmentID == "999" ? lstPointHead[i].sTreatmentOther : lstPointHead[i].sTreatmentName;
                string sType = lstPointHead[i].sTypePoint == "C" ? "Calculated" : "Manually";
                var lstPointSub = lstPoint.Where(w => w.nPointID == lstPointHead[i].nPointID && w.sTypePoint == lstPointHead[i].sTypePoint).ToList();
                if (lstPointHead[i].cCOD == "1") //lstPointHead[i].cCOD == "U"
                {
                    lstP = lstP.Where(w => w.ProductID != 128).ToList();
                }
                else
                {
                    lstP = lstP.Where(w => w.ProductID != 127).ToList();
                }
                foreach (var p in lstP)
                {
                    #region POINT
                    nColnumber = 1;
                    nRownumber++;
                    irow = ws3.Row(nRownumber);
                    irow.Height = 17.40;
                    //if (nPoint != lstPointHead[i].nPointID) nStart = nRownumber;

                    string sUnit = p.sUnit.Replace("<sup>", "").Replace("</sup>", "");
                    if (lstPointHead[i].sTypePoint == "C")
                    {
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sNumPoint, "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sPercent, "");
                    }
                    else
                    {
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                    }

                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, lstPointHead[i].sPointName, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sType, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sTreatmentName, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, lstPointHead[i].sDisChargeName, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, lstPointHead[i].sAreaName, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, p.ProductName, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sUnit, "");

                    var q = lstPointSub.FirstOrDefault(w => w.ProductID == p.ProductID);
                    if (q != null)
                    {
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, q.M1, "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, q.M2, "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, q.M3, "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, q.M4, "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, q.M5, "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, q.M6, "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, q.M7, "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, q.M8, "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, q.M9, "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, q.M10, "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, q.M11, "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, q.M12, "");

                    }
                    else
                    {
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, "", "");
                        SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, "", "");

                    }

                    if (p.ProductID == 124) SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, lstPointHead[i].sRemark, "");
                    else SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, "", "");

                    #endregion
                    //nPoint = lstPointHead[i].nPointID;
                }

                #region OTHER
                var lstProductOther = db.TM_Effluent_OtherProduct.Where(w => w.cDel == "N" && w.cActive == "Y").ToList();
                var lstUnitOther = db.TM_Effluent_Unit.Where(w => w.cDel == "N" && w.cActive == "Y").ToList();
                var lstOtherPoint = lstOther.Where(w => w.nPointID == lstPointHead[i].nPointID).ToList();
                foreach (var o in lstOtherPoint)
                {
                    nColnumber = 1;
                    nRownumber++;
                    irow = ws3.Row(nRownumber);
                    irow.Height = 17.40;
                    //if (nPoint == lstPointHead[i].nPointID) nMax = nRownumber;

                    int nP = !string.IsNullOrEmpty(o.sProductID) ? int.Parse(o.sProductID) : 0;
                    int nU = !string.IsNullOrEmpty(o.sUnitID) ? int.Parse(o.sUnitID) : 0;
                    string sName = ""; string sUnitNameOther = "";
                    var q = lstProductOther.FirstOrDefault(w => w.nProductID == nP);
                    if (q != null)
                    {
                        sName = q.sName;
                    }
                    var qu = lstUnitOther.FirstOrDefault(w => w.nUnitID == nU);
                    if (qu != null)
                    {
                        sUnitNameOther = qu.sName;
                    }

                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, "", "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, lstPointHead[i].sPointName, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sType, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sTreatmentName, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, lstPointHead[i].sDisChargeName, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, lstPointHead[i].sAreaName, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, sName, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sUnitNameOther, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, o.M1, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, o.M2, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, o.M3, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, o.M4, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, o.M5, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, o.M6, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, o.M7, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, o.M8, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, o.M9, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, o.M10, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, o.M11, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Right, false, o.M12, "");
                    SetBodyStyle(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, "", "");

                }
                #endregion
                //if (lstOtherPoint.Count == 0) nMax = nStart + 6;
                //if (!string.IsNullOrEmpty(lstPointHead[i].sRemark)) SetMerge("V" + nStart + ":V" + nMax + "", ws3);
            }
        }
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
    public class TDataDDL
    {
        public string Value { get; set; }
        public string sText { get; set; }
        public string cActive { get; set; }
    }
    public class CParam
    {
        public string sIndID { get; set; }
        public string sOprtID { get; set; }
        public string sFacID { get; set; }
        public string sYear { get; set; }
    }
    public class CSaveToDB
    {
        public int nStatus { get; set; }
        public string sRemarkTotal { get; set; }
        public int nIndicatorID { get; set; }
        public int nOperationID { get; set; }
        public int nFacilityID { get; set; }
        public string sYear { get; set; }
        public string sNumPoint { get; set; }
        public string sPercentWater { get; set; }
        public List<int> lstMonthSubmit { get; set; }
        public List<CProduct> lstProduct { get; set; }
        public List<CPointType> lstPointInput { get; set; }
        public List<TData_Point> lstPoitn { get; set; }
        public List<OtherPoint> lstProductOther { get; set; }
        public List<OtherPoint> lstOther { get; set; }
    }
    public class CReturnData : sysGlobalClass.CResutlWebMethod
    {
        public string EPI_FORMID { get; set; }
        public string sRemarkTotal { get; set; }
        public int nStatus { get; set; }
        public List<sysGlobalClass.T_TEPI_Workflow> lstStatus { get; set; }
        public List<CProduct> lstProduct { get; set; }
        public List<CTooltip> lstTooltip { get; set; }
        public string hdfUrlEcrypt { get; set; }
        public string hdfPRMS { get; set; }
        public string sNumPoint { get; set; }
        public string sPercentWater { get; set; }
        public List<TDataDDL> lstIndicatorOther { get; set; }
        public List<TDataDDL> lstUnitOther { get; set; }
        public List<CPointType> lstPointInput { get; set; }
        public List<TData_Point> lstPoint { get; set; }
        public List<OtherPoint> lstOther { get; set; }
    }
    public class CReturnWater : sysGlobalClass.CResutlWebMethod
    {
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
    public class CProduct
    {
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
        public string cSetCode { get; set; }
        public string sType { get; set; }
        public string sUnit { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public string Target { get; set; }
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
        public string sStatus { get; set; }
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
    public class CTooltip
    {
        public int ProductID { get; set; }
        public string sTooltip { get; set; }
    }
    public class CPointType
    {
        public int nPointID { get; set; }
        public string sTypePoint { get; set; }
        public string sPointName { get; set; }
        public string sDisChargeName { get; set; }
        public string sDisChargeID { get; set; }
        public string sTreatmentName { get; set; }
        public string sTreatmentID { get; set; }
        public string sAreaName { get; set; }
        public string sAreaID { get; set; }
        public string cStatus { get; set; }
        public string cCOD { get; set; }
        public string sRemark { get; set; }
        public string sTreatmentOther { get; set; }
        public string cNew { get; set; }
    }
    public class TData_Point
    {
        public int nPointID { get; set; }
        public string sTypePoint { get; set; }
        public int ProductID { get; set; }
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
    public class OtherPoint
    {
        public string sStatus { get; set; }
        public int FormID { get; set; }
        public string sTypePoint { get; set; }
        public int nPointID { get; set; }
        public int nOtherID { get; set; }
        public string sProductID { get; set; }
        public string sUnitID { get; set; }
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
        public string cNew { get; set; }
    }

    #endregion

}