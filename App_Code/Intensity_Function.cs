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

/// <summary>
/// Summary description for Intensity_Function
/// </summary>
public class Intensity_Function
{
    public Intensity_Function()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static sysGlobalClass.TRetunrLoadData GetLoadData(IntensityClass it)
    {
        sysGlobalClass.TRetunrLoadData o = new sysGlobalClass.TRetunrLoadData();
        try
        {
            if (!UserAcc.UserExpired())
            {
                PTTGC_EPIEntities db = new PTTGC_EPIEntities();
                bool IsNew = false;
                int nFormID = 0;
                var qEPIFomr = db.TEPI_Forms.Where(w => w.IDIndicator == it.nIndicator && w.sYear == it.sYear && w.OperationTypeID == it.nOperationType && w.FacilityID == it.nFacility).FirstOrDefault();
                if (qEPIFomr == null)
                {
                    IsNew = true;
                    string sYearDel = (SystemFunction.GetIntNullToZero(it.sYear) - 1) + "";
                    qEPIFomr = db.TEPI_Forms.Where(w => w.IDIndicator == it.nIndicator && w.sYear == sYearDel && w.OperationTypeID == it.nOperationType && w.FacilityID == it.nFacility).FirstOrDefault();
                }
                if (qEPIFomr != null)
                {
                    nFormID = qEPIFomr.FormID;
                }
                var lstOther = db.TIntensity_Other.Where(w => w.FormID == nFormID).AsEnumerable().Select(s => new sysGlobalClass.T_TIntensity_Other
                {
                    UnderProductID = s.UnderProductID, // Header
                    nProductID = s.ProductID,
                    sIndicator = s.ProductName,
                    cTotal = "N",
                    cTotalAll = "N",
                    FormID = s.FormID,
                    M1 = s.M1 != null && !IsNew ? s.M1 : "",
                    M2 = s.M2 != null && !IsNew ? s.M2 : "",
                    M3 = s.M3 != null && !IsNew ? s.M3 : "",
                    M4 = s.M4 != null && !IsNew ? s.M4 : "",
                    M5 = s.M5 != null && !IsNew ? s.M5 : "",
                    M6 = s.M6 != null && !IsNew ? s.M6 : "",
                    M7 = s.M7 != null && !IsNew ? s.M7 : "",
                    M8 = s.M8 != null && !IsNew ? s.M8 : "",
                    M9 = s.M9 != null && !IsNew ? s.M9 : "",
                    M10 = s.M10 != null && !IsNew ? s.M10 : "",
                    M11 = s.M11 != null && !IsNew ? s.M11 : "",
                    M12 = s.M12 != null && !IsNew ? s.M12 : "",
                    IsActive = true,
                    sTarget = !IsNew ? s.Target : "",
                    nFactor = s.Factor,
                }).ToList();
                var efi_wf = db.TEPI_Workflow.Where(w => w.FormID == nFormID).Select(s => new sysGlobalClass.T_TEPI_Workflow
                {

                    nReportID = s.nReportID,
                    FormID = s.FormID,
                    nMonth = s.nMonth,
                    nStatusID = IsNew ? 0 : s.nStatusID.HasValue ? s.nStatusID : 0,
                    nActionBy = s.nActionBy,
                    dAction = s.dAction
                }).ToList();
                if (IsNew)
                {
                    efi_wf = new List<sysGlobalClass.T_TEPI_Workflow>();
                }
                o.lstwf = efi_wf;
                o.lstIn = (from i in db.mTProductIndicator.Where(w => w.IDIndicator.Value == it.nIndicator).AsEnumerable()
                           from u in db.TIntensityUseProduct.Where(w => w.OperationTypeID == it.nOperationType && w.ProductID == i.ProductID).AsEnumerable()
                           from d in db.TIntensityDominator.Where(w => w.ProductID == i.ProductID && w.FormID == nFormID).AsEnumerable().DefaultIfEmpty()
                           from s in db.mTProductIndicatorUnit.Where(w => w.ProductID == u.ProductID && w.IDIndicator == i.IDIndicator).DefaultIfEmpty()
                           from un in db.mTUnit.Where(w => w.UnitID == s.UnitID).DefaultIfEmpty()
                           select new sysGlobalClass.TData_Intensity
                           {
                               ProductID = i.ProductID,
                               ProductName = i.ProductName,
                               cTotal = i.cTotal,
                               cTotalAll = i.cTotalAll,
                               nGroupCalc = i.nGroupCalc ?? 0,
                               nOrder = u.nOrder ?? 0,
                               sUnit = s != null && un != null ? un.UnitName : i.sUnit,
                               UnitID = s != null && un != null ? s.UnitID : 0,
                               sType = i.sType,
                               M1 = d != null && !IsNew ? d.M1 != null ? d.M1 : "" : "",
                               M2 = d != null && !IsNew ? d.M2 != null ? d.M2 : "" : "",
                               M3 = d != null && !IsNew ? d.M3 != null ? d.M3 : "" : "",
                               M4 = d != null && !IsNew ? d.M4 != null ? d.M4 : "" : "",
                               M5 = d != null && !IsNew ? d.M5 != null ? d.M5 : "" : "",
                               M6 = d != null && !IsNew ? d.M6 != null ? d.M6 : "" : "",
                               M7 = d != null && !IsNew ? d.M7 != null ? d.M7 : "" : "",
                               M8 = d != null && !IsNew ? d.M8 != null ? d.M8 : "" : "",
                               M9 = d != null && !IsNew ? d.M9 != null ? d.M9 : "" : "",
                               M10 = d != null && !IsNew ? d.M10 != null ? d.M10 : "" : "",
                               M11 = d != null && !IsNew ? d.M11 != null ? d.M11 : "" : "",
                               M12 = d != null && !IsNew ? d.M12 != null ? d.M12 : "" : "",
                               nTotal = d != null && !IsNew ? d.nTotal != null ? d.nTotal : "" : "",
                               Target = d != null && !IsNew ? d.Target : "",
                               lstarrDetail = lstOther.Where(w => w.UnderProductID == i.ProductID).ToList(),
                               sRemark = !IsNew ? db.TIntensity_Remark.Any(w => w.FormID == nFormID && w.ProductID == i.ProductID) ? db.TIntensity_Remark.AsEnumerable().LastOrDefault(w => w.FormID == nFormID && w.ProductID == i.ProductID).sRemark : "" : "",
                           }).OrderBy(k => k.nOrder).ToList();
                o.lstDataFile = db.TEPI_Forms_File.Where(w => w.FormID == nFormID).Select(s => new sysGlobalClass.FuncFileUpload.ItemData
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
                if (IsNew)
                {
                    o.lstDataFile = new List<sysGlobalClass.FuncFileUpload.ItemData>();
                }
                o.hdfPRMS = SystemFunction.GetPermission_EPI_FROMS(it.nIndicator, it.nFacility);
                o.sStatus = SystemFunction.process_Success;
                o.sMsg = "";
            }
            else
            {
                o.sStatus = SystemFunction.process_SessionExpired;
                o.sMsg = "Session Expired !!";
            }
        }
        catch (Exception ex)
        {
            o.sStatus = SystemFunction.process_Failed;
            o.sMsg = ex.ToString();
        }
        return o;
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.TRetunrLoadData SaveData(IntensityClass it)
    {
        sysGlobalClass.TRetunrLoadData result = new sysGlobalClass.TRetunrLoadData();
        string sStatus = it.rt.nStatusWF + "";
        string sStatusTemp = sStatus;
        int nFormID = 0;
        try
        {
            if (!UserAcc.UserExpired())
            {

                if (it.rt.lstIn != null)
                {

                    PTTGC_EPIEntities db = new PTTGC_EPIEntities();

                    var ef = db.TEPI_Forms.FirstOrDefault(w => w.IDIndicator == it.nIndicator && w.sYear == it.sYear && w.OperationTypeID == it.nOperationType && w.FacilityID == it.nFacility);

                    if (ef == null)
                    {
                        nFormID = (db.TEPI_Forms.Any() ? db.TEPI_Forms.Max(m => m.FormID) : 0) + 1;
                        ef = new TEPI_Forms();
                        ef.FormID = nFormID;
                        ef.sAddBy = UserAcc.GetObjUser().nUserID;
                        ef.dAddDate = DateTime.Now;
                        ef.sYear = it.sYear;
                        ef.IDIndicator = it.nIndicator;
                        ef.OperationTypeID = it.nOperationType;
                        ef.FacilityID = it.nFacility;
                        db.TEPI_Forms.Add(ef);
                    }
                    else
                    {
                        nFormID = ef.FormID;
                    }

                    if (!SystemFunction.IsSuperAdmin())
                    {
                        ef.ResponsiblePerson = UserAcc.GetObjUser().nUserID;
                        ef.sUpdateBy = UserAcc.GetObjUser().nUserID;
                        ef.dUpdateDate = DateTime.Now;
                    }

                    var ot = db.TIntensity_Other.Where(w => w.FormID == nFormID).ToList();
                    int nReportID = (db.TEPI_Workflow.Any() ? db.TEPI_Workflow.Max(m => m.nReportID) : 0) + 1;


                    if (sStatus == "24")
                    {
                        sStatus = "0";
                    }
                    List<string> lstMonthAdd = new List<string>();
                    for (int i = 1; i <= 12; i++)
                    {
                        lstMonthAdd.Add(i + "");
                    }
                    foreach (string i in lstMonthAdd)
                    {
                        int nM = SystemFunction.GetIntNullToZero(i);
                        var ef_wf = db.TEPI_Workflow.FirstOrDefault(w => w.FormID == nFormID && w.nMonth == nM);
                        int? nStatus = 0;
                        int nStatusID = SystemFunction.GetIntNullToZero(sStatus);
                        if (ef_wf == null)
                        {
                            ef_wf = new TEPI_Workflow();
                            ef_wf.nReportID = nReportID;
                            ef_wf.FormID = nFormID;
                            ef_wf.nMonth = nM;
                            db.TEPI_Workflow.Add(ef_wf);

                            if (!SystemFunction.IsSuperAdmin())
                            {
                                ef_wf.nStatusID = nStatus;
                                ef_wf.nActionBy = UserAcc.GetObjUser().nUserID;
                                ef_wf.dAction = DateTime.Now;
                            }
                            nReportID++;
                        }
                    }
                    if (sStatusTemp != "24" && sStatusTemp != "2")
                    {
                        db.TIntensity_Other.RemoveRange(ot);
                        db.SaveChanges();
                        it.rt.lstIn.ForEach(f =>
                        {
                            var td = db.TIntensityDominator.FirstOrDefault(w => w.FormID == nFormID && w.ProductID == f.ProductID);
                            int nVersion = (db.TIntensity_Remark.Any(w => w.FormID == nFormID && w.ProductID == f.ProductID) ? db.TIntensity_Remark.Where(w => w.FormID == nFormID && w.ProductID == f.ProductID).Max(m => m.nVersion) : 0) + 1;
                            if (td == null)
                            {
                                td = new TIntensityDominator();
                                td.ProductID = f.ProductID;
                                td.FormID = nFormID;
                                db.TIntensityDominator.Add(td);
                            }
                            td.UnitID = 0;
                            td.M1 = f.M1;
                            td.M2 = f.M2;
                            td.M3 = f.M3;
                            td.M4 = f.M4;
                            td.M5 = f.M5;
                            td.M6 = f.M6;
                            td.M7 = f.M7;
                            td.M8 = f.M8;
                            td.M9 = f.M9;
                            td.M10 = f.M10;
                            td.M11 = f.M11;
                            td.M12 = f.M12;
                            td.UnitID = f.UnitID;
                            if (!string.IsNullOrEmpty(f.nTotal))
                            {
                                td.nTotal = f.nTotal;
                            }
                            td.Target = f.Target;

                            var rm = new TIntensity_Remark();
                            rm.FormID = nFormID;
                            rm.ProductID = f.ProductID;
                            rm.nVersion = nVersion;
                            rm.sRemark = f.sRemark;
                            if (!SystemFunction.IsSuperAdmin())
                            {
                                rm.dAddDate = DateTime.Now;
                                rm.sAddBy = UserAcc.GetObjUser().nUserID;
                            }
                            int nUnit = f.UnitID;
                            db.TIntensity_Remark.Add(rm);

                            f.lstarrDetail.Where(w => w.IsActive == true).ToList().ForEach(f2 =>
                            {
                                var to = db.TIntensity_Other.Where(w => w.FormID == nFormID && w.ProductID == f2.nProductID && w.UnderProductID == f2.UnderProductID).FirstOrDefault();
                                if (to == null)
                                {
                                    to = new TIntensity_Other();
                                    to.ProductID = f2.nProductID;
                                    to.FormID = nFormID;
                                    to.UnderProductID = f2.UnderProductID;
                                    db.TIntensity_Other.Add(to);
                                }
                                to.UnitID = nUnit;
                                to.UnitID = 0;
                                to.M1 = f2.M1;
                                to.M2 = f2.M2;
                                to.M3 = f2.M3;
                                to.M4 = f2.M4;
                                to.M5 = f2.M5;
                                to.M6 = f2.M6;
                                to.M7 = f2.M7;
                                to.M8 = f2.M8;
                                to.M9 = f2.M9;
                                to.M10 = f2.M10;
                                to.M11 = f2.M11;
                                to.M12 = f2.M12;
                                to.Target = f2.sTarget;
                                to.ProductName = f2.sIndicator;
                            });

                        });

                        #region SAVE FILE
                        if (it.rt.lstDataFile.Count > 0)
                        {
                            string sPathSave = string.Format(it.sFolderInPathSave, nFormID);
                            SystemFunction.CreateDirectory(sPathSave);

                            //ลบไฟล์เดิมที่เคยมีและกดลบจากหน้าเว็บ
                            var qDelFile = it.rt.lstDataFile.Where(w => w.IsNewFile == false && w.sDelete == "Y").ToList();
                            if (qDelFile.Any())
                            {
                                foreach (var qf in qDelFile)
                                {
                                    var query = db.TEPI_Forms_File.FirstOrDefault(w => w.FormID == nFormID && w.sSysFileName == qf.SaveToFileName);
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

                            //Save New File Only
                            var lstSave = it.rt.lstDataFile.Where(w => w.sDelete == "N").ToList();

                            if (lstSave.Any())
                            {
                                int nFileID = db.TEPI_Forms_File.Where(w => w.FormID == nFormID).Any() ? db.TEPI_Forms_File.Where(w => w.FormID == nFormID).Max(m => m.nFileID) + 1 : 1;
                                lstSave.ForEach(f =>
                                {
                                    string sSystemFileName = "";
                                    if (f.IsNewFile == true)
                                    {
                                        sSystemFileName = nFormID + "_" + nFileID + "-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "." + SystemFunction.GetFileNameFromFileupload(f.SaveToFileName, "");
                                        SystemFunction.UpFile2Server(f.SaveToPath, sPathSave, f.SaveToFileName, sSystemFileName);
                                        f.ID = nFileID;
                                    }
                                    var t = db.TEPI_Forms_File.FirstOrDefault(w => w.nFileID == f.ID && w.FormID == nFormID);
                                    if (t == null)
                                    {
                                        t = new TEPI_Forms_File();
                                        t.FormID = nFormID;
                                        t.nFileID = nFileID;
                                        t.sSysFileName = sSystemFileName;
                                        t.sFileName = f.FileName;
                                        t.sPath = sPathSave;
                                        db.TEPI_Forms_File.Add(t);
                                        nFileID++;
                                    }
                                    t.sDescription = f.sDescription;
                                    if (!SystemFunction.IsSuperAdmin())
                                    {
                                        t.dAdd = DateTime.Now;
                                        t.nAddBy = UserAcc.GetObjUser().nUserID;
                                    }

                                });
                            }
                        }

                        #endregion
                    }

                    db.SaveChanges();

                    new EPIFunc().RecalculateMaterial(it.nOperationType, it.nFacility, it.sYear);
                    new EPIFunc().RecalculateWaste(it.nOperationType, it.nFacility, it.sYear);
                    new EPIFunc().RecalculateWater(it.nOperationType, it.nFacility, it.sYear);
                }

                if (sStatusTemp != "27")
                {
                    new Workflow().UpdateHistoryStatus(nFormID);
                }

                int nStatusTemp = SystemFunction.GetIntNullToZero(sStatusTemp);
                if (nStatusTemp != 0)
                {
                    string sMode = "";
                    switch (nStatusTemp)
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
                    if (!string.IsNullOrEmpty(sMode))
                    {
                        var wf = new Workflow().WorkFlowAction(nFormID, it.lstMonth, sMode, UserAcc.GetObjUser().nUserID, UserAcc.GetObjUser().nRoleID, it.rt.sComment);
                        result.sStatus = wf.Status;
                        result.sMsg = wf.Msg;
                    }
                    else
                    {
                        if (UserAcc.GetObjUser().nRoleID == 4)//ENVI Corporate (L2) >> Req.09.04.2019 Send email to L0 on L2 Modified data.
                        {
                            new Workflow().SendEmailToL0onL2EditData(it.sYear, it.nIndicator, it.nFacility, it.nOperationType);
                        }

                        result.sStatus = SystemFunction.process_Success;
                        result.sMsg = "";
                    }

                }
                else
                {
                    result.sStatus = SystemFunction.process_Success;
                    result.sMsg = "";
                }

            }
            else
            {
                result.sStatus = SystemFunction.process_SessionExpired;
                result.sMsg = "Session Expired !!";
            }
        }
        catch (Exception ex)
        {
            result.sStatus = SystemFunction.process_Failed;
            result.sMsg = ex.ToString();
        }
        string sPath = "#";
        switch (it.nOperationType)
        {
            case 4:
                sPath = "epi_input_intensity_1.aspx";
                ssTRetunrLoadData_1 = GetLoadData(it);
                break;
            case 11:
                sPath = "epi_input_intensity_2.aspx";
                ssTRetunrLoadData_2 = GetLoadData(it);
                break;
            case 13:
                sPath = "epi_input_intensity_3.aspx";
                ssTRetunrLoadData_3 = GetLoadData(it);
                break;
            case 14:
                sPath = "epi_input_intensity_4.aspx";
                ssTRetunrLoadData_4 = GetLoadData(it);
                break;
        }
        if (sPath != "#")
        {
            sPath = sPath + "?in=" + HttpUtility.UrlEncode(STCrypt.Encrypt(it.nIndicator + "")) + "&&ot=" + HttpUtility.UrlEncode(STCrypt.Encrypt(it.nOperationType + ""));
        }
        result.sPath = sPath;

        return result;
    }

    public static sysGlobalClass.TRetunrLoadData CreateLinkExport(IntensityClass it)
    {
        sysGlobalClass.TRetunrLoadData o = new sysGlobalClass.TRetunrLoadData();
        try
        {
            if (!UserAcc.UserExpired())
            {
                string sPath = "#";
                switch (it.nOperationType)
                {
                    case 4:
                        sPath = "export_epi_input_intensity_1.aspx";
                        ssTRetunrLoadData_1 = GetLoadData(it);
                        break;
                    case 11:
                        sPath = "export_epi_input_intensity_2.aspx";
                        ssTRetunrLoadData_2 = GetLoadData(it);
                        break;
                    case 13:
                        sPath = "export_epi_input_intensity_3.aspx";
                        ssTRetunrLoadData_3 = GetLoadData(it);
                        break;
                    case 14:
                        sPath = "export_epi_input_intensity_4.aspx";
                        ssTRetunrLoadData_4 = GetLoadData(it);
                        break;
                }
                if (sPath != "#")
                {
                    sPath = sPath + "?str1=" + HttpUtility.UrlEncode(STCrypt.Encrypt(it.nFacility + "")) + "&&str2=" + HttpUtility.UrlEncode(STCrypt.Encrypt(it.sYear + ""));
                }
                o.sPath = sPath;
                //var lst = GetLoadData(it);

                o.sStatus = SystemFunction.process_Success;
                o.sMsg = "";
            }
            else
            {
                o.sStatus = SystemFunction.process_SessionExpired;
                o.sMsg = "Session Expired !!";
            }
        }
        catch (Exception ex)
        {
            o.sStatus = SystemFunction.process_Failed;
            o.sMsg = ex.ToString();
        }
        return o;
    }
    [Serializable]
    public class IntensityClass
    {
        public sysGlobalClass.TRetunrLoadData rt { get; set; }
        public int nIndicator { get; set; }
        public string sYear { get; set; }
        public int nOperationType { get; set; }
        public int nFacility { get; set; }
        public List<int> lstMonth { get; set; }
        public string sFolderInPathSave { get; set; }
        public string sComment { get; set; }
    }
    public static sysGlobalClass.TRetunrLoadData ssTRetunrLoadData_1
    {
        get
        {
            return HttpContext.Current.Session["ssTRetunrLoadData_1"] is sysGlobalClass.TRetunrLoadData ?
                (sysGlobalClass.TRetunrLoadData)HttpContext.Current.Session["ssTRetunrLoadData_1"] : new sysGlobalClass.TRetunrLoadData();
        }
        set { HttpContext.Current.Session["ssTRetunrLoadData_1"] = value; }
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
    public static sysGlobalClass.TRetunrLoadData ssTRetunrLoadData_3
    {
        get
        {
            return HttpContext.Current.Session["ssTRetunrLoadData_3"] is sysGlobalClass.TRetunrLoadData ?
                (sysGlobalClass.TRetunrLoadData)HttpContext.Current.Session["ssTRetunrLoadData_3"] : new sysGlobalClass.TRetunrLoadData();
        }
        set { HttpContext.Current.Session["ssTRetunrLoadData_3"] = value; }
    }
    public static sysGlobalClass.TRetunrLoadData ssTRetunrLoadData_4
    {
        get
        {
            return HttpContext.Current.Session["ssTRetunrLoadData_4"] is sysGlobalClass.TRetunrLoadData ?
                (sysGlobalClass.TRetunrLoadData)HttpContext.Current.Session["ssTRetunrLoadData_4"] : new sysGlobalClass.TRetunrLoadData();
        }
        set { HttpContext.Current.Session["ssTRetunrLoadData_4"] = value; }
    }
}