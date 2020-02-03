using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class epi_output : System.Web.UI.Page
{
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_EPI_FORMS_Output)this.Master).SetBodyEventOnLoad(myFunc);
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

            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static ResultData LoadData(TSearch item)
    {
        ResultData r = new ResultData();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataOutput> lstDataT1 = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstData = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstData2 = new List<ClassExecute.TDataOutput>();

        int nFormID = 0;

        var qForm = db.TEPI_Forms.FirstOrDefault(w => w.IDIndicator == item.nIndicator && w.OperationTypeID == item.nOperationType && w.FacilityID == item.nFacility && w.sYear == item.sYear);
        if (qForm != null)
        {
            nFormID = qForm.FormID;
        }

        //if (lstDataT1.Any())
        //{
        if (item.nIndicator == 10)
        {
            lstData = FunctionGetData.GetWasteDataOutput(nFormID, item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
            //lstData = GetWaste(nFormID, item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
        }
        else if (item.nIndicator == 8)
        {
            lstData = FunctionGetData.GetMaterialDataOutput(nFormID, item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
            // lstData = GetMaterial(nFormID, item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
        }
        else if (item.nIndicator == 6)
        {
            // lstData = GetIntensity(nFormID, item.nIndicator, item.nOperationType, item.nFacility, item.sYear);
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
        //}

        r.lstData = lstData;
        r.sFormID = nFormID + "";
        r.Status = SystemFunction.process_Success;

        return r;
    }

    public static List<ClassExecute.TDataOutput> GetWaste(int nFormID, int nIDIndicator, int OperationType, int nFacility, string sYear) //List<ClassExecute.TDataOutput> lstOut, 
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataOutput> lstOut = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstData = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstHead = new List<ClassExecute.TDataOutput>();

        lstOut = FunctionGetData.GetDataOutput(nIDIndicator, OperationType, nFacility, sYear);
        lstData = lstOut.ToList();
        var lstMark = db.TWaste_Remark.Where(w => w.FormID == nFormID).OrderByDescending(o => o.nVersion).ToList();
        Func<int, string> GetRemarkInput = (productid) =>
        {
            string sremark = "";
            var q = lstMark.FirstOrDefault(w => w.ProductID == productid);
            sremark = q != null ? q.sRemark : "";
            return sremark;
        };

        foreach (var ou in lstOut)
        {
            int UnderProducID = 0;
            int nSpecificID = 0;
            if (ou.ProductID == 4)
            {
                ou.sType = "Group";
                UnderProducID = 2;
                nSpecificID = OperationType == 11 ? 59 : OperationType == 4 ? 71 : OperationType == 14 ? 65 : OperationType == 13 ? 35 : 0;
                ou.sMakeField1 = GetRemarkInput(UnderProducID); //lstMark.Any() ? lstMark.FirstOrDefault(w => w.ProductID == UnderProducID).sRemark : "";
            }
            else if (ou.ProductID == 7)
            {
                ou.sType = "Group";
                UnderProducID = 8;
                nSpecificID = OperationType == 11 ? 60 : OperationType == 4 ? 72 : OperationType == 14 ? 66 : OperationType == 13 ? 36 : 0;
                ou.sMakeField1 = GetRemarkInput(UnderProducID); // lstMark.Any() ? lstMark.FirstOrDefault(w => w.ProductID == UnderProducID).sRemark : "";
            }
            else if (ou.ProductID == 13)
            {
                ou.sType = "Group";
                UnderProducID = 17;
                nSpecificID = OperationType == 11 ? 62 : OperationType == 4 ? 74 : OperationType == 14 ? 68 : OperationType == 13 ? 38 : 0;
                ou.sMakeField1 = GetRemarkInput(UnderProducID); //lstMark.Any() ? lstMark.FirstOrDefault(w => w.ProductID == UnderProducID).sRemark : "";
            }
            else if (ou.ProductID == 16)
            {
                ou.sType = "Group";
                UnderProducID = 24;
                nSpecificID = OperationType == 11 ? 63 : OperationType == 4 ? 75 : OperationType == 14 ? 69 : OperationType == 13 ? 39 : 0;
                ou.sMakeField1 = GetRemarkInput(UnderProducID); //lstMark.Any() ? lstMark.FirstOrDefault(w => w.ProductID == UnderProducID).sRemark : "";
            }
            else if (ou.ProductID == 1)
            { ou.sType = "Group"; }
            else if (ou.ProductID == 10)
            { ou.sType = "Group"; }


            if (UnderProducID != 0)
            {
                #region ADD HEAD
                lstHead = (from a in db.mTProductIndicator.Where(w => w.IDIndicator == nIDIndicator && w.nOption == UnderProducID).AsEnumerable()
                           from b in db.TWaste_Product.Where(w => w.ProductID == a.ProductID && w.FormID == nFormID)
                           from c in db.mTUnit.Where(w => w.UnitID == b.UnitID).AsEnumerable().DefaultIfEmpty()
                           select new ClassExecute.TDataOutput
                           {
                               IDIndicator = nIDIndicator,
                               OperationtypeID = OperationType,
                               FacilityID = nFacility,
                               ProductID = a.ProductID,
                               ProductName = a.ProductName,
                               nUnitID = b.UnitID ?? 0,
                               sUnit = "Tonnes",
                               nOrder = SystemFunction.GetDecimalNull(a.nOrder + "") ?? 0,
                               sType = "Head",
                               nHeadID = ou.ProductID,

                               nTarget = SystemFunction.GetDecimalNull(b.Target),
                               nM1 = SystemFunction.GetDecimalNull(b.M1),
                               nM2 = SystemFunction.GetDecimalNull(b.M2),
                               nM3 = SystemFunction.GetDecimalNull(b.M3),
                               nM4 = SystemFunction.GetDecimalNull(b.M4),
                               nM5 = SystemFunction.GetDecimalNull(b.M5),
                               nM6 = SystemFunction.GetDecimalNull(b.M6),
                               nM7 = SystemFunction.GetDecimalNull(b.M7),
                               nM8 = SystemFunction.GetDecimalNull(b.M8),
                               nM9 = SystemFunction.GetDecimalNull(b.M9),
                               nM10 = SystemFunction.GetDecimalNull(b.M10),
                               nM11 = SystemFunction.GetDecimalNull(b.M11),
                               nM12 = SystemFunction.GetDecimalNull(b.M12),
                               nTotal = EPIFunc.SumDataToDecimal(b.M1, b.M2, b.M3, b.M4, b.M5, b.M6, b.M7, b.M8, b.M9, b.M10, b.M11, b.M12),

                               sTarget = SystemFunction.ConvertFormatDecimal4(b.Target),
                               sM1 = SystemFunction.ConvertFormatDecimal4(b.M1),
                               sM2 = SystemFunction.ConvertFormatDecimal4(b.M2),
                               sM3 = SystemFunction.ConvertFormatDecimal4(b.M3),
                               sM4 = SystemFunction.ConvertFormatDecimal4(b.M4),
                               sM5 = SystemFunction.ConvertFormatDecimal4(b.M5),
                               sM6 = SystemFunction.ConvertFormatDecimal4(b.M6),
                               sM7 = SystemFunction.ConvertFormatDecimal4(b.M7),
                               sM8 = SystemFunction.ConvertFormatDecimal4(b.M8),
                               sM9 = SystemFunction.ConvertFormatDecimal4(b.M9),
                               sM10 = SystemFunction.ConvertFormatDecimal4(b.M10),
                               sM11 = SystemFunction.ConvertFormatDecimal4(b.M11),
                               sM12 = SystemFunction.ConvertFormatDecimal4(b.M12),
                               sTotal = EPIFunc.SumDataToDecimal(b.M1, b.M2, b.M3, b.M4, b.M5, b.M6, b.M7, b.M8, b.M9, b.M10, b.M11, b.M12).ToString(),

                           }).OrderBy(o => o.sType).ThenBy(n => n.nOrder).ToList();

                if (lstHead.Any())
                {
                    int index = lstData.FindIndex(x => x.ProductID == nSpecificID && x.sType == null) + 1;
                    lstData.InsertRange(index, lstHead);
                }
                else
                {
                    lstHead = (from a in db.mTProductIndicator.Where(w => w.IDIndicator == nIDIndicator && w.nOption == UnderProducID).AsEnumerable()
                               from c in db.mTUnit.Where(w => w.UnitID == 2).AsEnumerable().DefaultIfEmpty()
                               select new ClassExecute.TDataOutput
                               {
                                   IDIndicator = nIDIndicator,
                                   OperationtypeID = OperationType,
                                   FacilityID = nFacility,
                                   ProductID = a.ProductID,
                                   ProductName = a.ProductName,
                                   nUnitID = 2,
                                   sUnit = c.UnitName,
                                   nOrder = SystemFunction.GetDecimalNull(a.nOrder + "") ?? 0,
                                   sType = "Head",
                                   nHeadID = ou.ProductID,

                                   nTarget = 0,
                                   nM1 = 0,
                                   nM2 = 0,
                                   nM3 = 0,
                                   nM4 = 0,
                                   nM5 = 0,
                                   nM6 = 0,
                                   nM7 = 0,
                                   nM8 = 0,
                                   nM9 = 0,
                                   nM10 = 0,
                                   nM11 = 0,
                                   nM12 = 0,
                                   nTotal = 0,

                                   sTarget = "",
                                   sM1 = "",
                                   sM2 = "",
                                   sM3 = "",
                                   sM4 = "",
                                   sM5 = "",
                                   sM6 = "",
                                   sM7 = "",
                                   sM8 = "",
                                   sM9 = "",
                                   sM10 = "",
                                   sM11 = "",
                                   sM12 = "",
                                   sTotal = "",

                               }).OrderBy(o => o.sType).ThenBy(n => n.nOrder).ToList();

                    int index = lstData.FindIndex(x => x.ProductID == nSpecificID && x.sType == null) + 1;
                    lstData.InsertRange(index, lstHead);
                }
                #endregion

                #region ADD SUB
                foreach (var h in lstHead)
                {
                    List<ClassExecute.TDataOutput> lstSub = new List<ClassExecute.TDataOutput>();
                    lstSub = (from a in db.TWaste_Product_data.Where(w => w.UnderProductID == h.ProductID && w.FormID == nFormID).AsEnumerable()
                              from b in db.mTUnit.Where(w => w.UnitID == a.UnitID).AsEnumerable().DefaultIfEmpty()
                              select new ClassExecute.TDataOutput
                              {
                                  IDIndicator = nIDIndicator,
                                  OperationtypeID = OperationType,
                                  FacilityID = nFacility,
                                  ProductID = a.nSubProductID,
                                  ProductName = a.ProductName,
                                  nUnitID = a.UnitID ?? 0,
                                  sUnit = b.UnitName,
                                  nOrder = 0,
                                  sType = "Sub",
                                  nHeadID = a.UnderProductID,

                                  nTarget = SystemFunction.GetDecimalNull(a.Target),
                                  nM1 = SystemFunction.GetDecimalNull(a.M1),
                                  nM2 = SystemFunction.GetDecimalNull(a.M2),
                                  nM3 = SystemFunction.GetDecimalNull(a.M3),
                                  nM4 = SystemFunction.GetDecimalNull(a.M4),
                                  nM5 = SystemFunction.GetDecimalNull(a.M5),
                                  nM6 = SystemFunction.GetDecimalNull(a.M6),
                                  nM7 = SystemFunction.GetDecimalNull(a.M7),
                                  nM8 = SystemFunction.GetDecimalNull(a.M8),
                                  nM9 = SystemFunction.GetDecimalNull(a.M9),
                                  nM10 = SystemFunction.GetDecimalNull(a.M10),
                                  nM11 = SystemFunction.GetDecimalNull(a.M11),
                                  nM12 = SystemFunction.GetDecimalNull(a.M12),
                                  nTotal = EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12),

                                  sTarget = SystemFunction.ConvertFormatDecimal4(a.Target),
                                  sM1 = SystemFunction.ConvertFormatDecimal4(a.M1),
                                  sM2 = SystemFunction.ConvertFormatDecimal4(a.M2),
                                  sM3 = SystemFunction.ConvertFormatDecimal4(a.M3),
                                  sM4 = SystemFunction.ConvertFormatDecimal4(a.M4),
                                  sM5 = SystemFunction.ConvertFormatDecimal4(a.M5),
                                  sM6 = SystemFunction.ConvertFormatDecimal4(a.M6),
                                  sM7 = SystemFunction.ConvertFormatDecimal4(a.M7),
                                  sM8 = SystemFunction.ConvertFormatDecimal4(a.M8),
                                  sM9 = SystemFunction.ConvertFormatDecimal4(a.M9),
                                  sM10 = SystemFunction.ConvertFormatDecimal4(a.M10),
                                  sM11 = SystemFunction.ConvertFormatDecimal4(a.M11),
                                  sM12 = SystemFunction.ConvertFormatDecimal4(a.M12),
                                  sTotal = EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12).ToString(),

                              }).OrderBy(o => o.ProductID).ToList();
                    if (lstSub.Any())
                    {
                        int index = lstData.FindIndex(x => x.ProductID == h.ProductID && x.nHeadID == ou.ProductID && x.sType == "Head") + 1;
                        lstData.InsertRange(index, lstSub);
                        var qData = lstData.FirstOrDefault(w => w.ProductID == h.ProductID && w.nHeadID == ou.ProductID && w.sType == "Head");
                        if (qData != null)
                        {
                            qData.isSub = true;
                        }
                    }
                }
                #endregion
            }
        }

        #region ADD
        lstHead = db.mTProductIndicator.Where(w => w.IDIndicator == nIDIndicator && w.ProductID == 240).Select(s => new ClassExecute.TDataOutput
        {
            IDIndicator = nIDIndicator,
            OperationtypeID = OperationType,
            FacilityID = nFacility,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nUnitID = 2,
            sUnit = "Tonnes",
            nOrder = 0,
            sType = "Group",
            nHeadID = 0,

            nTarget = 0,
            nM1 = 0,
            nM2 = 0,
            nM3 = 0,
            nM4 = 0,
            nM5 = 0,
            nM6 = 0,
            nM7 = 0,
            nM8 = 0,
            nM9 = 0,
            nM10 = 0,
            nM11 = 0,
            nM12 = 0,
            nTotal = 0,

            sTarget = "",
            sM1 = "",
            sM2 = "",
            sM3 = "",
            sM4 = "",
            sM5 = "",
            sM6 = "",
            sM7 = "",
            sM8 = "",
            sM9 = "",
            sM10 = "",
            sM11 = "",
            sM12 = "",
            sTotal = "",
        }).ToList();

        foreach (var item in lstHead)
        {
            var q = db.TWaste_Product.FirstOrDefault(w => w.ProductID == item.ProductID && w.FormID == nFormID);
            if (q != null)
            {
                item.sMakeField1 = GetRemarkInput(240);
                item.nTarget = SystemFunction.GetDecimalNull(q.Target);
                item.nM1 = SystemFunction.GetDecimalNull(q.M1);
                item.nM2 = SystemFunction.GetDecimalNull(q.M2);
                item.nM3 = SystemFunction.GetDecimalNull(q.M3);
                item.nM4 = SystemFunction.GetDecimalNull(q.M4);
                item.nM5 = SystemFunction.GetDecimalNull(q.M5);
                item.nM6 = SystemFunction.GetDecimalNull(q.M6);
                item.nM7 = SystemFunction.GetDecimalNull(q.M7);
                item.nM8 = SystemFunction.GetDecimalNull(q.M8);
                item.nM9 = SystemFunction.GetDecimalNull(q.M9);
                item.nM10 = SystemFunction.GetDecimalNull(q.M10);
                item.nM11 = SystemFunction.GetDecimalNull(q.M11);
                item.nM12 = SystemFunction.GetDecimalNull(q.M12);
                item.nTotal = EPIFunc.SumDataToDecimal(q.M1, q.M2, q.M3, q.M4, q.M5, q.M6, q.M7, q.M8, q.M9, q.M10, q.M11, q.M12);

                item.sTarget = SystemFunction.ConvertFormatDecimal4(q.Target);
                item.sM1 = SystemFunction.ConvertFormatDecimal4(q.M1);
                item.sM2 = SystemFunction.ConvertFormatDecimal4(q.M2);
                item.sM3 = SystemFunction.ConvertFormatDecimal4(q.M3);
                item.sM4 = SystemFunction.ConvertFormatDecimal4(q.M4);
                item.sM5 = SystemFunction.ConvertFormatDecimal4(q.M5);
                item.sM6 = SystemFunction.ConvertFormatDecimal4(q.M6);
                item.sM7 = SystemFunction.ConvertFormatDecimal4(q.M7);
                item.sM8 = SystemFunction.ConvertFormatDecimal4(q.M8);
                item.sM9 = SystemFunction.ConvertFormatDecimal4(q.M9);
                item.sM10 = SystemFunction.ConvertFormatDecimal4(q.M10);
                item.sM11 = SystemFunction.ConvertFormatDecimal4(q.M11);
                item.sM12 = SystemFunction.ConvertFormatDecimal4(q.M12);
                item.sTotal = EPIFunc.SumDataToDecimal(q.M1, q.M2, q.M3, q.M4, q.M5, q.M6, q.M7, q.M8, q.M9, q.M10, q.M11, q.M12).ToString();
            }

        }

        if (lstHead.Count > 0)
        {
            lstData.AddRange(lstHead);
            List<ClassExecute.TDataOutput> lstSubOther = new List<ClassExecute.TDataOutput>();
            lstSubOther = (from a in db.TWaste_Product_data.Where(w => w.UnderProductID == 240 && w.FormID == nFormID).AsEnumerable()
                           from b in db.mTUnit.Where(w => w.UnitID == a.UnitID).AsEnumerable().DefaultIfEmpty()
                           select new ClassExecute.TDataOutput
                           {
                               IDIndicator = nIDIndicator,
                               OperationtypeID = OperationType,
                               FacilityID = nFacility,
                               ProductID = a.nSubProductID,
                               ProductName = a.ProductName,
                               nUnitID = a.UnitID ?? 0,
                               sUnit = b.UnitName,
                               nOrder = 0,
                               sType = "Sub",
                               nHeadID = a.UnderProductID,

                               nTarget = SystemFunction.GetDecimalNull(a.Target),
                               nM1 = SystemFunction.GetDecimalNull(a.M1),
                               nM2 = SystemFunction.GetDecimalNull(a.M2),
                               nM3 = SystemFunction.GetDecimalNull(a.M3),
                               nM4 = SystemFunction.GetDecimalNull(a.M4),
                               nM5 = SystemFunction.GetDecimalNull(a.M5),
                               nM6 = SystemFunction.GetDecimalNull(a.M6),
                               nM7 = SystemFunction.GetDecimalNull(a.M7),
                               nM8 = SystemFunction.GetDecimalNull(a.M8),
                               nM9 = SystemFunction.GetDecimalNull(a.M9),
                               nM10 = SystemFunction.GetDecimalNull(a.M10),
                               nM11 = SystemFunction.GetDecimalNull(a.M11),
                               nM12 = SystemFunction.GetDecimalNull(a.M12),
                               nTotal = EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12),

                               sTarget = SystemFunction.ConvertFormatDecimal4(a.Target),
                               sM1 = SystemFunction.ConvertFormatDecimal4(a.M1),
                               sM2 = SystemFunction.ConvertFormatDecimal4(a.M2),
                               sM3 = SystemFunction.ConvertFormatDecimal4(a.M3),
                               sM4 = SystemFunction.ConvertFormatDecimal4(a.M4),
                               sM5 = SystemFunction.ConvertFormatDecimal4(a.M5),
                               sM6 = SystemFunction.ConvertFormatDecimal4(a.M6),
                               sM7 = SystemFunction.ConvertFormatDecimal4(a.M7),
                               sM8 = SystemFunction.ConvertFormatDecimal4(a.M8),
                               sM9 = SystemFunction.ConvertFormatDecimal4(a.M9),
                               sM10 = SystemFunction.ConvertFormatDecimal4(a.M10),
                               sM11 = SystemFunction.ConvertFormatDecimal4(a.M11),
                               sM12 = SystemFunction.ConvertFormatDecimal4(a.M12),
                               sTotal = EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12).ToString(),

                           }).OrderBy(o => o.ProductID).ToList();
            if (lstSubOther.Count > 0)
            {
                lstData.AddRange(lstSubOther);
            }
        }
        #endregion

        return lstData;
    }
    public static List<ClassExecute.TDataOutput> GetMaterial(int nFormID, int nIDIndicator, int OperationType, int nFacility, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataOutput> lstOut = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstData = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstHead = new List<ClassExecute.TDataOutput>();

        lstOut = FunctionGetData.GetDataOutput(nIDIndicator, OperationType, nFacility, sYear);
        lstData = lstOut.ToList();

        var lstMark = db.TMaterial_Remark.Where(w => w.FormID == nFormID).OrderByDescending(o => o.nVersion).ToList();

        Func<int, string> GetRemarkInput = (productid) =>
        {
            string sremark = "";
            var q = lstMark.FirstOrDefault(w => w.ProductID == productid);
            sremark = q != null ? q.sRemark : "";
            return sremark;
        };

        foreach (var ou in lstOut)
        {
            int nINProducID = 0;
            int nSpecificID = 0;
            if (ou.ProductID == 88)//Total Direct Materials Used
            {
                ou.sType = "Group";
                nINProducID = 34;
                nSpecificID = OperationType == 11 ? 113 : OperationType == 4 ? 119 : OperationType == 14 ? 116 : OperationType == 13 ? 101 : 0;
                ou.sMakeField1 = GetRemarkInput(nINProducID);
            }
            else if (ou.ProductID == 91)//Total Associated Materials Used
            {
                ou.sType = "Group";
                nINProducID = 37;
                nSpecificID = OperationType == 11 ? 114 : OperationType == 4 ? 120 : OperationType == 14 ? 117 : OperationType == 13 ? 102 : 0;
                ou.sMakeField1 = GetRemarkInput(nINProducID);
            }
            else if (ou.ProductID == 97)//Recycled Input Material Used
            {/// เป็นกลุ่มที่ไม่มี Head มี sub เลย
                ou.sType = "Group";
                nINProducID = 41;
                nSpecificID = 0;
            }
            else if (ou.ProductID == 94) /// Total Materials Used
            {
                nINProducID = 33;
                ou.sType = "Group";
                ou.sMakeField1 = GetRemarkInput(nINProducID);
            }

            if (nINProducID != 0)
            {
                #region ADD id Product Head
                List<int> lstINProducID = new List<int>();
                if (nINProducID == 34)
                {
                    lstINProducID.Add(36);
                    lstINProducID.Add(35);
                }
                else if (nINProducID == 37)
                {
                    lstINProducID.Add(40);
                    lstINProducID.Add(39);
                    lstINProducID.Add(38);

                }
                #endregion

                if (lstINProducID.Count > 0)
                {

                    foreach (var p in lstINProducID)
                    {
                        #region ADD HEAD
                        lstHead = (from a in db.mTProductIndicator.Where(w => w.IDIndicator == nIDIndicator && w.ProductID == p).AsEnumerable()
                                   from b in db.TMaterial_Product.Where(w => w.ProductID == a.ProductID && w.FormID == nFormID)
                                   from c in db.mTUnit.Where(w => w.UnitID == (b.UnitID ?? 2)).AsEnumerable().DefaultIfEmpty()
                                   select new ClassExecute.TDataOutput
                                   {
                                       IDIndicator = nIDIndicator,
                                       OperationtypeID = OperationType,
                                       FacilityID = nFacility,
                                       ProductID = a.ProductID,
                                       ProductName = a.ProductName,
                                       nUnitID = (b.UnitID ?? 2),
                                       sUnit = c.UnitName,
                                       nOrder = SystemFunction.GetDecimalNull(a.nOrder + "") ?? 0,
                                       sType = "Head",
                                       nHeadID = ou.ProductID,

                                       nTarget = SystemFunction.GetDecimalNull(b.Target),
                                       nM1 = SystemFunction.GetDecimalNull(b.M1),
                                       nM2 = SystemFunction.GetDecimalNull(b.M2),
                                       nM3 = SystemFunction.GetDecimalNull(b.M3),
                                       nM4 = SystemFunction.GetDecimalNull(b.M4),
                                       nM5 = SystemFunction.GetDecimalNull(b.M5),
                                       nM6 = SystemFunction.GetDecimalNull(b.M6),
                                       nM7 = SystemFunction.GetDecimalNull(b.M7),
                                       nM8 = SystemFunction.GetDecimalNull(b.M8),
                                       nM9 = SystemFunction.GetDecimalNull(b.M9),
                                       nM10 = SystemFunction.GetDecimalNull(b.M10),
                                       nM11 = SystemFunction.GetDecimalNull(b.M11),
                                       nM12 = SystemFunction.GetDecimalNull(b.M12),
                                       nTotal = SystemFunction.GetDecimalNull(b.nTotal),

                                       sTarget = SystemFunction.ConvertFormatDecimal4(b.Target),
                                       sM1 = SystemFunction.ConvertFormatDecimal4(b.M1),
                                       sM2 = SystemFunction.ConvertFormatDecimal4(b.M2),
                                       sM3 = SystemFunction.ConvertFormatDecimal4(b.M3),
                                       sM4 = SystemFunction.ConvertFormatDecimal4(b.M4),
                                       sM5 = SystemFunction.ConvertFormatDecimal4(b.M5),
                                       sM6 = SystemFunction.ConvertFormatDecimal4(b.M6),
                                       sM7 = SystemFunction.ConvertFormatDecimal4(b.M7),
                                       sM8 = SystemFunction.ConvertFormatDecimal4(b.M8),
                                       sM9 = SystemFunction.ConvertFormatDecimal4(b.M9),
                                       sM10 = SystemFunction.ConvertFormatDecimal4(b.M10),
                                       sM11 = SystemFunction.ConvertFormatDecimal4(b.M11),
                                       sM12 = SystemFunction.ConvertFormatDecimal4(b.M12),
                                       sTotal = SystemFunction.ConvertFormatDecimal4(b.nTotal),

                                   }).OrderBy(o => o.sType).ThenBy(n => n.nOrder).ToList();

                        if (lstHead.Any())
                        {
                            int index = lstData.FindIndex(x => x.ProductID == nSpecificID && x.sType == null) + 1;
                            lstData.InsertRange(index, lstHead);
                        }
                        else
                        {
                            lstHead = (from a in db.mTProductIndicator.Where(w => w.IDIndicator == nIDIndicator && w.ProductID == p).AsEnumerable()
                                       from c in db.mTUnit.Where(w => w.UnitID == 2).AsEnumerable().DefaultIfEmpty()
                                       select new ClassExecute.TDataOutput
                                       {
                                           IDIndicator = nIDIndicator,
                                           OperationtypeID = OperationType,
                                           FacilityID = nFacility,
                                           ProductID = a.ProductID,
                                           ProductName = a.ProductName,
                                           nUnitID = 2,
                                           sUnit = c.UnitName,
                                           nOrder = SystemFunction.GetDecimalNull(a.nOrder + "") ?? 0,
                                           sType = "Head",
                                           nHeadID = ou.ProductID,

                                           nTarget = 0,
                                           nM1 = 0,
                                           nM2 = 0,
                                           nM3 = 0,
                                           nM4 = 0,
                                           nM5 = 0,
                                           nM6 = 0,
                                           nM7 = 0,
                                           nM8 = 0,
                                           nM9 = 0,
                                           nM10 = 0,
                                           nM11 = 0,
                                           nM12 = 0,
                                           nTotal = 0,

                                           sTarget = "",
                                           sM1 = "",
                                           sM2 = "",
                                           sM3 = "",
                                           sM4 = "",
                                           sM5 = "",
                                           sM6 = "",
                                           sM7 = "",
                                           sM8 = "",
                                           sM9 = "",
                                           sM10 = "",
                                           sM11 = "",
                                           sM12 = "",
                                           sTotal = "",

                                       }).OrderBy(o => o.sType).ThenBy(n => n.nOrder).ToList();

                            int index = lstData.FindIndex(x => x.ProductID == nSpecificID && x.sType == null) + 1;
                            lstData.InsertRange(index, lstHead);
                        }
                        #endregion

                        #region ADD SUB
                        foreach (var h in lstHead)
                        {
                            List<ClassExecute.TDataOutput> lstSub = new List<ClassExecute.TDataOutput>();
                            lstSub = (from a in db.TMaterial_ProductData.Where(w => w.nUnderbyProductID == h.ProductID && w.FormID == nFormID).AsEnumerable()
                                      from b in db.mTUnit.Where(w => w.UnitID == a.nUnitID).AsEnumerable().DefaultIfEmpty()
                                      select new ClassExecute.TDataOutput
                                      {
                                          IDIndicator = nIDIndicator,
                                          OperationtypeID = OperationType,
                                          FacilityID = nFacility,
                                          ProductID = a.nSubProductID,
                                          ProductName = a.sName,
                                          nUnitID = a.nUnitID ?? 0,
                                          sUnit = b.UnitName,
                                          nOrder = 0,
                                          sType = "Sub",
                                          nHeadID = a.nUnderbyProductID,

                                          nTarget = SystemFunction.GetDecimalNull(a.Target),
                                          nM1 = SystemFunction.GetDecimalNull(a.M1),
                                          nM2 = SystemFunction.GetDecimalNull(a.M2),
                                          nM3 = SystemFunction.GetDecimalNull(a.M3),
                                          nM4 = SystemFunction.GetDecimalNull(a.M4),
                                          nM5 = SystemFunction.GetDecimalNull(a.M5),
                                          nM6 = SystemFunction.GetDecimalNull(a.M6),
                                          nM7 = SystemFunction.GetDecimalNull(a.M7),
                                          nM8 = SystemFunction.GetDecimalNull(a.M8),
                                          nM9 = SystemFunction.GetDecimalNull(a.M9),
                                          nM10 = SystemFunction.GetDecimalNull(a.M10),
                                          nM11 = SystemFunction.GetDecimalNull(a.M11),
                                          nM12 = SystemFunction.GetDecimalNull(a.M12),
                                          nTotal = EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12),

                                          sTarget = SystemFunction.ConvertFormatDecimal4(a.Target),
                                          sM1 = SystemFunction.ConvertFormatDecimal4(a.M1),
                                          sM2 = SystemFunction.ConvertFormatDecimal4(a.M2),
                                          sM3 = SystemFunction.ConvertFormatDecimal4(a.M3),
                                          sM4 = SystemFunction.ConvertFormatDecimal4(a.M4),
                                          sM5 = SystemFunction.ConvertFormatDecimal4(a.M5),
                                          sM6 = SystemFunction.ConvertFormatDecimal4(a.M6),
                                          sM7 = SystemFunction.ConvertFormatDecimal4(a.M7),
                                          sM8 = SystemFunction.ConvertFormatDecimal4(a.M8),
                                          sM9 = SystemFunction.ConvertFormatDecimal4(a.M9),
                                          sM10 = SystemFunction.ConvertFormatDecimal4(a.M10),
                                          sM11 = SystemFunction.ConvertFormatDecimal4(a.M11),
                                          sM12 = SystemFunction.ConvertFormatDecimal4(a.M12),
                                          sTotal = EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12).ToString(),

                                      }).OrderBy(o => o.ProductID).ToList();
                            if (lstSub.Any())
                            {
                                int index = lstData.FindIndex(x => x.ProductID == h.ProductID && x.nHeadID == ou.ProductID && x.sType == "Head") + 1;
                                lstData.InsertRange(index, lstSub);
                                var qData = lstData.FirstOrDefault(w => w.ProductID == h.ProductID && w.nHeadID == ou.ProductID && w.sType == "Head");
                                if (qData != null)
                                {
                                    qData.isSub = true;
                                }
                            }
                        }
                        #endregion
                    }
                }
                else if (lstINProducID.Count == 0 && nINProducID == 41)
                {
                    #region ADD SUB

                    List<ClassExecute.TDataOutput> lstSub = new List<ClassExecute.TDataOutput>();
                    lstSub = (from a in db.TMaterial_ProductData.Where(w => w.nUnderbyProductID == nINProducID && w.FormID == nFormID).AsEnumerable()
                              from b in db.mTUnit.Where(w => w.UnitID == a.nUnitID).AsEnumerable().DefaultIfEmpty()
                              select new ClassExecute.TDataOutput
                              {
                                  IDIndicator = nIDIndicator,
                                  OperationtypeID = OperationType,
                                  FacilityID = nFacility,
                                  ProductID = a.nSubProductID,
                                  ProductName = a.sName,
                                  nUnitID = a.nUnitID ?? 0,
                                  sUnit = b.UnitName,
                                  nOrder = 0,
                                  sType = "Sub",
                                  nHeadID = a.nUnderbyProductID,

                                  nTarget = SystemFunction.GetDecimalNull(a.Target),
                                  nM1 = SystemFunction.GetDecimalNull(a.M1),
                                  nM2 = SystemFunction.GetDecimalNull(a.M2),
                                  nM3 = SystemFunction.GetDecimalNull(a.M3),
                                  nM4 = SystemFunction.GetDecimalNull(a.M4),
                                  nM5 = SystemFunction.GetDecimalNull(a.M5),
                                  nM6 = SystemFunction.GetDecimalNull(a.M6),
                                  nM7 = SystemFunction.GetDecimalNull(a.M7),
                                  nM8 = SystemFunction.GetDecimalNull(a.M8),
                                  nM9 = SystemFunction.GetDecimalNull(a.M9),
                                  nM10 = SystemFunction.GetDecimalNull(a.M10),
                                  nM11 = SystemFunction.GetDecimalNull(a.M11),
                                  nM12 = SystemFunction.GetDecimalNull(a.M12),
                                  nTotal = EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12),

                                  sTarget = SystemFunction.ConvertFormatDecimal4(a.Target),
                                  sM1 = SystemFunction.ConvertFormatDecimal4(a.M1),
                                  sM2 = SystemFunction.ConvertFormatDecimal4(a.M2),
                                  sM3 = SystemFunction.ConvertFormatDecimal4(a.M3),
                                  sM4 = SystemFunction.ConvertFormatDecimal4(a.M4),
                                  sM5 = SystemFunction.ConvertFormatDecimal4(a.M5),
                                  sM6 = SystemFunction.ConvertFormatDecimal4(a.M6),
                                  sM7 = SystemFunction.ConvertFormatDecimal4(a.M7),
                                  sM8 = SystemFunction.ConvertFormatDecimal4(a.M8),
                                  sM9 = SystemFunction.ConvertFormatDecimal4(a.M9),
                                  sM10 = SystemFunction.ConvertFormatDecimal4(a.M10),
                                  sM11 = SystemFunction.ConvertFormatDecimal4(a.M11),
                                  sM12 = SystemFunction.ConvertFormatDecimal4(a.M12),
                                  sTotal = EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12).ToString(),

                              }).OrderBy(o => o.ProductID).ToList();
                    if (lstSub.Any())
                    {
                        int index = lstData.FindIndex(x => x.ProductID == ou.ProductID && x.sType == "Group") + 1;
                        lstData.InsertRange(index, lstSub);
                        var qData = lstData.FirstOrDefault(w => w.ProductID == ou.ProductID && w.sType == "Group");
                        if (qData != null)
                        {
                            qData.isSub = true;
                        }
                    }

                    #endregion
                }
            }
        }

        return lstData;
    }

    public static List<ClassExecute.TDataOutput> GetIntensity(int nFormID, int nIDIndicator, int OperationType, int nFacility, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataOutput> lstData = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstHead = new List<ClassExecute.TDataOutput>();

        var lstMark = db.TIntensity_Remark.Where(w => w.FormID == nFormID).OrderByDescending(o => o.nVersion).ToList();
        Func<int, string> GetRemarkInput = (productid) =>
        {
            string sremark = "";
            var q = lstMark.FirstOrDefault(w => w.ProductID == productid);
            sremark = q != null ? q.sRemark : "";
            return sremark;
        };

        lstHead = (from i in db.mTProductIndicator.Where(w => w.IDIndicator.Value == nIDIndicator).AsEnumerable()
                   from u in db.TIntensityUseProduct.Where(w => w.OperationTypeID == OperationType && w.ProductID == i.ProductID).AsEnumerable()
                   from d in db.TIntensityDominator.Where(w => w.ProductID == i.ProductID && w.FormID == nFormID).AsEnumerable().DefaultIfEmpty()
                   from s in db.mTProductIndicatorUnit.Where(w => w.ProductID == u.ProductID && w.IDIndicator == i.IDIndicator).DefaultIfEmpty()
                   from un in db.mTUnit.Where(w => w.UnitID == s.UnitID).DefaultIfEmpty()
                   select new ClassExecute.TDataOutput
                   {
                       ProductID = i.ProductID,
                       ProductName = i.ProductName,

                       IDIndicator = nIDIndicator,
                       OperationtypeID = OperationType,
                       FacilityID = nFacility,
                       sUnit = i.sUnit,
                       nOrder = SystemFunction.GetDecimalNull(u.nOrder + "") ?? 0,
                       sType = i.ProductID == 89 ? "Sub" : i.ProductID == 90 ? "Sub" : i.ProductID == 86 ? "TotalArea" : i.cTotalAll == "Y" ? "Group" : i.cTotal == "N" && i.cTotalAll == "N" ? "SubHead" : "Head",
                       nHeadID = i.ProductID == 89 ? 88 : i.ProductID == 90 ? 88 : 0,
                       isSub = i.ProductID == 88 ? true : false,
                       sTotalArea = nFormID == 0 ? "" : OperationType == 14 ? d.nTotal : "",


                       sTarget = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.Target),
                       sM1 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M1),
                       sM2 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M2),
                       sM3 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M3),
                       sM4 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M4),
                       sM5 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M5),
                       sM6 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M6),
                       sM7 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M7),
                       sM8 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M8),
                       sM9 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M9),
                       sM10 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M10),
                       sM11 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M11),
                       sM12 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M12),
                       sTotal = nFormID == 0 ? "" : GetValMonthMax(d.M12, d.M11, d.M10, d.M9, d.M8, d.M7, d.M6, d.M5, d.M4, d.M3, d.M2, d.M1, i.ProductID),
                       sMakeField1 = nFormID == 0 ? "" : GetRemarkInput(i.ProductID),
                   }).OrderBy(k => k.nOrder).ToList();
        lstData = lstHead.ToList();

        foreach (var h in lstHead)
        {
            var lstSub = db.TIntensity_Other.Where(w => w.FormID == nFormID && w.UnderProductID == h.ProductID).AsEnumerable()
                .Select(s => new ClassExecute.TDataOutput
                {
                    IDIndicator = nIDIndicator,
                    OperationtypeID = OperationType,
                    FacilityID = nFacility,
                    ProductID = s.ProductID,
                    ProductName = s.ProductName,
                    // nUnitID = b.UnitID ?? 0,
                    sUnit = h.sUnit,
                    //nOrder = SystemFunction.GetDecimalNull(i.nOrder + "") ?? 0,
                    sType = "Sub",
                    nHeadID = h.ProductID,

                    nTarget = SystemFunction.GetDecimalNull(s.Target),
                    nM1 = SystemFunction.GetDecimalNull(s.M1),
                    nM2 = SystemFunction.GetDecimalNull(s.M2),
                    nM3 = SystemFunction.GetDecimalNull(s.M3),
                    nM4 = SystemFunction.GetDecimalNull(s.M4),
                    nM5 = SystemFunction.GetDecimalNull(s.M5),
                    nM6 = SystemFunction.GetDecimalNull(s.M6),
                    nM7 = SystemFunction.GetDecimalNull(s.M7),
                    nM8 = SystemFunction.GetDecimalNull(s.M8),
                    nM9 = SystemFunction.GetDecimalNull(s.M9),
                    nM10 = SystemFunction.GetDecimalNull(s.M10),
                    nM11 = SystemFunction.GetDecimalNull(s.M11),
                    nM12 = SystemFunction.GetDecimalNull(s.M12),
                    nTotal = EPIFunc.SumDataToDecimal(s.M1, s.M2, s.M3, s.M4, s.M5, s.M6, s.M7, s.M8, s.M9, s.M10, s.M11, s.M12),

                    sTarget = SystemFunction.ConvertFormatDecimal4(s.Target),
                    sM1 = SystemFunction.ConvertFormatDecimal4(s.M1),
                    sM2 = SystemFunction.ConvertFormatDecimal4(s.M2),
                    sM3 = SystemFunction.ConvertFormatDecimal4(s.M3),
                    sM4 = SystemFunction.ConvertFormatDecimal4(s.M4),
                    sM5 = SystemFunction.ConvertFormatDecimal4(s.M5),
                    sM6 = SystemFunction.ConvertFormatDecimal4(s.M6),
                    sM7 = SystemFunction.ConvertFormatDecimal4(s.M7),
                    sM8 = SystemFunction.ConvertFormatDecimal4(s.M8),
                    sM9 = SystemFunction.ConvertFormatDecimal4(s.M9),
                    sM10 = SystemFunction.ConvertFormatDecimal4(s.M10),
                    sM11 = SystemFunction.ConvertFormatDecimal4(s.M11),
                    sM12 = SystemFunction.ConvertFormatDecimal4(s.M12),
                    sTotal = SystemFunction.ConvertFormatDecimal4(EPIFunc.SumDataToDecimal(s.M1, s.M2, s.M3, s.M4, s.M5, s.M6, s.M7, s.M8, s.M9, s.M10, s.M11, s.M12).ToString()),
                }).ToList();

            if (lstSub.Any())
            {
                int index = lstData.FindIndex(x => x.ProductID == h.ProductID && x.sType == "Head") + 1;
                lstData.InsertRange(index, lstSub);
                var qData = lstData.FirstOrDefault(w => w.ProductID == h.ProductID && w.sType == "Head");
                if (qData != null)
                {
                    qData.isSub = true;
                }
            }
        }
        return lstData;
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
            var lstMark = db.TEmission_Remark.Where(w => w.FormID == itemEPI_FORM.FormID).OrderByDescending(o => o.nVersion).ToList();
            Func<int, string> GetRemarkInput = (productid) =>
            {
                string sremark = "";
                var q = lstMark.FirstOrDefault(w => w.ProductID == productid);
                sremark = q != null ? q.sRemark : "";
                return sremark;
            };
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
                    f.sRemark = GetRemarkInput(f.ProductID);
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
    public static string GetValMonthMax(string sM12, string sM11, string sM10, string sM9, string sM8, string sM7, string sM6, string sM5, string sM4, string sM3, string sM2, string sM1, int ProductID)
    {
        string sTotalPerson = "";

        if (ProductID == 87)
        {
            #region 14
            for (int i = 12; i <= 12; i--)
            {
                if (i == 12)
                {
                    if (!string.IsNullOrEmpty(sM12))
                    {
                        sTotalPerson = sM12; break;
                    }
                }
                else if (i == 11)
                {
                    if (!string.IsNullOrEmpty(sM11))
                    {
                        sTotalPerson = sM11; break;
                    }
                }
                else if (i == 10)
                {
                    if (!string.IsNullOrEmpty(sM10)) { sTotalPerson = sM10; break; }
                }
                else if (i == 9)
                {
                    if (!string.IsNullOrEmpty(sM9)) { sTotalPerson = sM9; break; }
                }
                else if (i == 8)
                {
                    if (!string.IsNullOrEmpty(sM8)) { sTotalPerson = sM8; break; }
                }
                else if (i == 7)
                {
                    if (!string.IsNullOrEmpty(sM7)) { sTotalPerson = sM7; break; }
                }
                else if (i == 6)
                {
                    if (!string.IsNullOrEmpty(sM6)) { sTotalPerson = sM6; break; }
                }
                else if (i == 5)
                {
                    if (!string.IsNullOrEmpty(sM5)) { sTotalPerson = sM5; break; }
                }
                else if (i == 4)
                {
                    if (!string.IsNullOrEmpty(sM4)) { sTotalPerson = sM4; break; }
                }
                else if (i == 3)
                {
                    if (!string.IsNullOrEmpty(sM3)) { sTotalPerson = sM3; break; }
                }
                else if (i == 2)
                {
                    if (!string.IsNullOrEmpty(sM2)) { sTotalPerson = sM2; break; }
                }
                else if (i == 1)
                {
                    if (!string.IsNullOrEmpty(sM1)) { sTotalPerson = sM1; break; }
                }
            }
            #endregion
        }
        else
        {
            sTotalPerson = SystemFunction.ConvertFormatDecimal4(EPIFunc.SumDataToDecimal(sM1, sM2, sM3, sM4, sM5, sM6, sM7, sM8, sM9, sM10, sM11, sM12).ToString());
        }
        return sTotalPerson;
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
            int nOperationType = !string.IsNullOrEmpty(hidOperationType.Value) ? int.Parse(hidOperationType.Value) : 0;
            int nIndicator = !string.IsNullOrEmpty(hidIndicator.Value) ? int.Parse(hidIndicator.Value) : 0;
            int nFacility = !string.IsNullOrEmpty(hidFacility.Value) ? int.Parse(hidFacility.Value) : 0;

            TSearch item = new TSearch();
            item.sYear = hidYear.Value;
            item.nOperationType = nOperationType;
            item.nIndicator = nIndicator;
            item.nFacility = nFacility;

            ResultData dd = LoadData(item);

            string sIndicatorName = "";
            var qI = db.mTIndicator.FirstOrDefault(w => w.ID == nIndicator);
            if (qI != null) sIndicatorName = qI.Indicator;

            string sOName = "";
            var qO = db.mOperationType.FirstOrDefault(w => w.ID == nOperationType);
            if (qO != null) sOName = qO.Name;

            string sFacility = "";
            var qF = db.mTFacility.FirstOrDefault(w => w.ID == nFacility);
            if (qF != null) sFacility = qF.Name;

            string saveAsFileName = string.Format("Output" + "_" + sIndicatorName + "_" + sFacility + "{0:ddMMyyyyHHmmss}.xlsx", DateTime.Now).Replace("/", "_");

            XLWorkbook workbook = GetWorkbookExcel(dd, saveAsFileName
                , sIndicatorName, sOName, sFacility, hidYear.Value, nIndicator, nOperationType);

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
    public static XLWorkbook GetWorkbookExcel(ResultData objData, string sFileName, string sIndicatorName, string sOName, string sFacility, string sYear, int nIndicator, int nOperationType)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        // Creating a new workbook
        XLWorkbook workbook = new XLWorkbook();
        workbook.Style.Font.FontName = "Cordia New";

        //Adding a worksheet
        string saveAsFileName = string.Format("Report_{0:ddMMyyyyHHmmss}", DateTime.Now).Replace("/", "_");

        //row number must be between 1 and 1048576
        int nRownumber = 1;
        //Column number must be between 1 and 16384
        int nColnumber = 1;
        IXLWorksheet worksheet = workbook.Worksheets.Add("Output");
        worksheet.ShowGridLines = false;
        IXLCell icell;
        IXLRow irow;

        #region Set Column Width
        worksheet.Column(1).Width = 50;
        worksheet.Column(2).Width = 40;
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
        if (nIndicator == 6 && nOperationType == 14)
        {
            worksheet.Column(16).Width = 20;
            worksheet.Column(17).Width = 40;
        }
        else
        {
            worksheet.Column(16).Width = 40;
        }
        #endregion

        #region Report Header
        IXLStyle ReportStyle = (IXLStyle)workbook.Style;
        ReportStyle.Font.Bold = true;
        ReportStyle.Font.FontSize = 14;
        ReportStyle.Font.FontColor = XLColor.Black;
        ReportStyle.Fill.BackgroundColor = XLColor.FromHtml("#FFFFFF");
        ReportStyle.Fill.PatternType = XLFillPatternValues.Solid;
        ReportStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        ReportStyle.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        #region function
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

        #region Merge Cell
        Action<string> SetMerge = (sRange) =>
        {
            IXLRange rangeMerge = worksheet.Range(sRange);
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
        //SetMerge("A1:P1");
        //SetMerge("A1:E1");
        //SetMerge("A2:E2");
        //SetMerge("A3:E3");
        #endregion

        #region Add Table Header

        Action<IXLRow, int, string> SetHeadStyle = (row, nColumn, sHeadText) =>
        {
            IXLCell cell = row.Cell(nColumn);
            cell.Value = sHeadText;
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontSize = 14;
            cell.Style.Font.FontColor = XLColor.FromHtml("#31708F");
            cell.Style.Border.TopBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.LeftBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.RightBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.BottomBorderColor = XLColor.FromHtml("#DDDDDD");
            cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#D9EDF7");
            cell.Style.Fill.PatternType = XLFillPatternValues.Solid;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            //cell.Style = THeadStyle;
        };

        #endregion

        #region function BODY
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

            //if (CType == XLCellValues.DateTime) cell.Style.DateFormat.Format = sFormat;
            //else if (CType == XLCellValues.Number) cell.Style.NumberFormat.Format = sFormat;
            if (CType != XLCellValues.Number)
            {
                cell.SetDataType(XLCellValues.Text);
                cell.Value = ObjValue;
            }
            else
            {
                if (ObjValue + "".ToUpper() == "N/A")
                {
                    cell.SetDataType(XLCellValues.Text);
                    cell.Value = ObjValue;
                }
                else
                {
                    if (SystemFunction.IsNumberic(ObjValue + ""))
                    {
                        cell.SetDataType(XLCellValues.Number);
                        var sVal = ObjValue + "";
                        string sValCheck = SystemFunction.ConvertExponentialToString(sVal);
                        sValCheck = sValCheck.Replace("-", "");
                        sVal = SystemFunction.ConvertExponentialToString(sVal);
                        decimal nCheck = decimal.Parse(sValCheck); // แปลงเป็นค่าสัมบูรณ์
                        decimal nTemp = decimal.Parse(sVal);
                        string[] str = sValCheck.Split('.');
                        if (nCheck > 0 && nCheck < 1)
                        {
                            if (str.Length > 1)
                            {
                                if (str[1].Length >= 4)
                                {
                                    cell.Style.NumberFormat.Format = "0.000E+0";
                                }
                                else
                                {
                                    cell.Style.NumberFormat.Format = "#,##0.000";
                                }
                            }
                            else
                            {
                                cell.Style.NumberFormat.Format = "#,##0.000";
                            }
                        }
                        //else if (nCheck >= 1 || nCheck == 0)              
                        else
                        {
                            cell.Style.NumberFormat.Format = "#,##0.000";
                        }
                        cell.Value = ObjValue;
                    }
                    else
                    {
                        cell.SetDataType(XLCellValues.Text);
                        cell.Value = ObjValue;
                    }
                }
            }
            //cell.Style = TBodyStyle;
        };
        #endregion

        #region BIND DATA

        #region HEAD
        nRownumber++;
        irow = worksheet.Row(nRownumber);
        irow.Height = 22.20;

        nColnumber = 1;

        SetHeadStyle(irow, nColnumber++, "Indicator");
        if (nIndicator == 10)
        {
            SetHeadStyle(irow, nColnumber++, "Disposal Code");
        }
        SetHeadStyle(irow, nColnumber++, "Unit");
        SetHeadStyle(irow, nColnumber++, "YTD");
        SetHeadStyle(irow, nColnumber++, "Jan");
        SetHeadStyle(irow, nColnumber++, "Feb");
        SetHeadStyle(irow, nColnumber++, "Mar");
        SetHeadStyle(irow, nColnumber++, "Apr");
        SetHeadStyle(irow, nColnumber++, "May");
        SetHeadStyle(irow, nColnumber++, "Jun");
        SetHeadStyle(irow, nColnumber++, "Jul");
        SetHeadStyle(irow, nColnumber++, "Aug");
        SetHeadStyle(irow, nColnumber++, "Sep");
        SetHeadStyle(irow, nColnumber++, "Oct");
        SetHeadStyle(irow, nColnumber++, "Nov");
        SetHeadStyle(irow, nColnumber++, "Dec");
        if (nIndicator == 6 && nOperationType == 14)
        {
            SetHeadStyle(irow, nColnumber++, "Total Area");
            SetHeadStyle(irow, nColnumber++, "Remark");
        }
        else
        {
            if (nIndicator != 1 && nIndicator != 2 && nIndicator != 9)
            {
                SetHeadStyle(irow, nColnumber++, "Remark");
            }
        }


        #endregion

        #region FUNCTION BIND
        Func<List<ClassExecute.TDataOutput>, string, bool, bool> BindTable = (lstDataBind, sHeadSpill, IsEmission) =>
        {
            bool bReturn = true;
            int index = 1;
            foreach (var item in lstDataBind)
            {
                nColnumber = 1;
                nRownumber++;
                irow = worksheet.Row(nRownumber);
                irow.Height = 17.40;

                string ss = item.sType;
                string sColor = item.sType == "Head" ? "#fabd4f" : item.sType == "Group" ? "#dbea97" : item.sType == "Sub" ? "#ffffff" : "#ffffff"; //|| item.nUnitID == 0
                string sSpace = item.sType == "Group" ? "" : item.sType == "Head" ? "  " : item.sType == "Sub" ? "    " : "    ";
                var sUnit = FunctionGetData.ReplaceHtmlUnit(item.sUnit);
                if (nIndicator == 10)
                {
                    //// KG แปลงเป็น Tonnes
                    if (item.nUnitID == 3)
                    {
                        sUnit = "Tonnes";
                        item.nM1 = item.nM1 / 1000;
                        item.nM2 = item.nM2 / 1000;
                        item.nM3 = item.nM3 / 1000;
                        item.nM4 = item.nM4 / 1000;
                        item.nM5 = item.nM5 / 1000;
                        item.nM6 = item.nM6 / 1000;
                        item.nM7 = item.nM7 / 1000;
                        item.nM8 = item.nM8 / 1000;
                        item.nM9 = item.nM9 / 1000;
                        item.nM10 = item.nM10 / 1000;
                        item.nM11 = item.nM11 / 1000;
                        item.nM12 = item.nM12 / 1000;
                        item.nTotal = item.nTotal / 1000;
                    }
                }


                if (sHeadSpill != "" && index == 1)
                {
                    SetBodyStyle2(irow, nColnumber, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, sHeadSpill, "", sColor);
                    worksheet.Range(nRownumber, 1, nRownumber, IsEmission ? 16 : 15).Merge();
                    worksheet.Range(nRownumber, 1, nRownumber, IsEmission ? 16 : 15).Style.Fill.BackgroundColor = XLColor.FromHtml("#fdb813");
                    nRownumber++;
                    irow = worksheet.Row(nRownumber);
                }
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, sSpace + item.ProductName.Replace("&nbsp;", " ").Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 "), "", sColor);
                if (nIndicator == 10)
                {
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.sDisposalName, "", sColor);
                }
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sUnit, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nTotal, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nM1, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nM2, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nM3, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nM4, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nM5, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nM6, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nM7, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nM8, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nM9, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nM10, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nM11, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nM12, "", sColor);
                if (nIndicator == 6 && nOperationType == 14)
                {
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, item.sTotalArea, "", sColor);
                    SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sMakeField1, "", sColor);
                }
                else
                {
                    if (nIndicator != 1 && nIndicator != 2 && nIndicator != 9)
                    {
                        SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sMakeField1, "", sColor);
                    }
                }

                index++;
            }
            return bReturn;
        };
        Func<List<TData_Emission>, string, bool, bool> BindTableOther = (lstDataBind, sHead, IsEmission) =>
        {
            bool bReturn = true;
            int index = 1;
            foreach (var item in lstDataBind)
            {
                nColnumber = 1;
                nRownumber++;
                irow = worksheet.Row(nRownumber);
                irow.Height = 17.40;

                string ss = item.sType;
                string sColor = item.sType == "Group" ? "#dbea97" : item.sType == "Head" ? "#fabd4f" : "#ffffff";
                string sSpace = item.sType == "Group" ? "" : item.sType == "Head" ? "  " : item.sType == "Sub" ? "    " : "    ";
                var sUnit = FunctionGetData.ReplaceHtmlUnit(item.sUnit);

                if (sHead != "" && index == 1)
                {
                    SetBodyStyle2(irow, nColnumber, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, sHead, "", sColor);
                    worksheet.Range(nRownumber, 1, nRownumber, 16).Merge();
                    worksheet.Range(nRownumber, 1, nRownumber, 16).Style.Fill.BackgroundColor = XLColor.FromHtml("#fdb813");
                    nRownumber++;
                    irow = worksheet.Row(nRownumber);
                }
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, sSpace + (item.sType == "SUM" ? item.ProductName.Replace("&nbsp;", " ").Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 ") : (item.sType == "SUM2" || item.UnitID == 68 ? "" : item.ProductName.Replace("&nbsp;", " ").Replace("<sub>x</sub>", "x").Replace("<sub>2</sub>", "2").Replace("<sub>2 </sub>", "2 "))), "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Center, false, sUnit, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.nTotal, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.M1, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.M2, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.M3, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.M4, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.M5, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.M6, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.M7, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.M8, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.M9, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.M10, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.M11, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Number, XLAlignmentHorizontalValues.Right, false, item.M12, "", sColor);
                SetBodyStyle2(irow, nColnumber++, XLCellValues.Text, XLAlignmentHorizontalValues.Left, false, item.sRemark, "", sColor);

                index++;
            }
            return bReturn;
        };
        #endregion

        if (objData.lstData.Where(w => w.IDIndicator == 9).Count() > 0)
        {
            BindTable(objData.lstData.Where(w => w.sMakeField2 == "0").ToList(), "", false);
            BindTable(objData.lstData.Where(w => w.sMakeField2 == "1" || w.IDIndicator == 0).ToList(), "Spill", false);
            BindTable(objData.lstData.Where(w => w.sMakeField2 == "2" || w.IDIndicator == 0).ToList(), "Significant Spill", false);
        }
        else if (nIndicator == 4)
        {
            if (nOperationType != 13)
            {
                BindTable(objData.lstData, "Combustion", true);
                BindTableOther(objData.lstDataNonCombustion, "Non-Combustion", true);
                BindTableOther(objData.lstDataCEM, "CEM", true);
                BindTableOther(objData.lstDataAdditionalCombustion, "Additional Combustion", true);
                BindTableOther(objData.lstDataAdditionalNonCombustion, "Addition Non-Combustion", true);
            }
            BindTableOther(objData.lstDataVOC, "VOC", true);
        }
        else
        {
            BindTable(objData.lstData, "", false);
        }
        #endregion

        nRownumber++;

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
        public string sRemark { get; set; }
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