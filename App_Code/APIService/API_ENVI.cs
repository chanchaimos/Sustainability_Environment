using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace API_ENVI
{
    /// <summary>
    /// Summary description for API_ENVI
    /// </summary>
    public class API_ENVI
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        public string sUrlWebSite = ConfigurationSettings.AppSettings["UrlSite"].ToString();
        public API_ENVI()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public class DataType
        {
            public const int nIndIntensityID = 6;
            public const int nIndMaterialID = 8;
            public const int nIndWaterID = 11;
            public const int nIndWasteID = 10;
            public const int nIndEmissionID = 4;
            public const int nIndEffluentID = 3;
            public const int nIndSpillID = 9;
            public const int nIndComplaintID = 1;
            public const int nIndComplianceID = 2;
        }

        public void SendEmail_RejectByPTT(List<int> lstSubFacility, int nIndID, int nYear, int nQuarter, string sComment)
        {
            string sMsg = @"Dear All, <br />" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Environmental Report has been rejected by ptt as detailed below.<br />" +
                        "Facility : {0}<br />" +
                        "Group Indicator : {1}<br />" +
                        "Year : {2}<br />" +
                        "Quarter : {3}<br /><br />" +
                        "Comment : {4}<br /><br />" +
                        "Click <a href='{5}' target='_blank'>link</a> to view for further action.";
            var dataFac = db.mTFacility.Where(w => w.cDel == "N" && w.cActive == "Y" && w.nLevel == 2 && lstSubFacility.Contains(w.ID)).ToList();
            string strFacName = String.Join(", ", dataFac.Select(s => s.Name).OrderBy(o => o).ToList());
            var dataInd = db.mTIndicator.FirstOrDefault(w => w.ID == nIndID);

            string _sUrl = sUrlWebSite + "AD/loginAD.aspx";
            string sFrom = System.Configuration.ConfigurationSettings.AppSettings["SystemMail"] + "";
            var dataToUser = (from d in db.mTWorkFlow.Where(w => w.IDIndicator == nIndID && lstSubFacility.Contains(w.IDFac))
                              from u in db.mTUser.Where(w => w.cDel == "N" && w.cActive == "Y" && w.ID == d.L2)
                              select new
                              {
                                  u.Email
                              }).GroupBy(g => g.Email).Select(s => s.Key).ToList();
            string strTo = String.Join(",", dataToUser);
            string sMsgSend = string.Format(sMsg, strFacName, (dataInd != null ? dataInd.Indicator : ""), nYear, nQuarter, sComment, _sUrl);
            string sSubject = string.Format("Rejected by ptt of {0} of quarter {1} {2} ", (dataInd != null ? dataInd.Indicator : ""), nQuarter, nYear); // 0 = Group Indicator,1=Quater,2=Year
            Workflow.DataMail_log Log = new Workflow.DataMail_log();
            Log = SystemFunction.SendMailAll(sFrom, strTo, "", "", sSubject, sMsgSend, "");
            new Workflow().SaveLogMail(Log);
        }

        public void SendEmail_ApproveByPTT(List<int> lstSubFacility, int nIndID, int nYear, int nQuarter, string sComment)
        {
            string sMsg = @"Dear All, <br />" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Environmental Report has been approved by ptt as detailed below.<br />" +
                        "Facility : {0}<br />" +
                        "Group Indicator : {1}<br />" +
                        "Year : {2}<br />" +
                        "Quarter : {3}<br /><br />" +
                        "Comment : {4}<br /><br />" +
                        "Click <a href='{5}' target='_blank'>link</a> to view for further action.";
            var dataFac = db.mTFacility.Where(w => w.cDel == "N" && w.cActive == "Y" && w.nLevel == 2 && lstSubFacility.Contains(w.ID)).ToList();
            string strFacName = String.Join(", ", dataFac.Select(s => s.Name).OrderBy(o => o).ToList());
            var dataInd = db.mTIndicator.FirstOrDefault(w => w.ID == nIndID);

            string _sUrl = sUrlWebSite + "AD/loginAD.aspx";
            string sFrom = System.Configuration.ConfigurationSettings.AppSettings["SystemMail"] + "";
            var dataToUser = (from d in db.mTWorkFlow.Where(w => w.IDIndicator == nIndID && lstSubFacility.Contains(w.IDFac))
                              from u in db.mTUser.Where(w => w.cDel == "N" && w.cActive == "Y" && w.ID == d.L2)
                              select new
                              {
                                  u.Email
                              }).GroupBy(g => g.Email).Select(s => s.Key).ToList();
            string strTo = String.Join(",", dataToUser);
            string sMsgSend = string.Format(sMsg, strFacName, (dataInd != null ? dataInd.Indicator : ""), nYear, nQuarter, sComment, _sUrl);
            string sSubject = string.Format("Approved by ptt of {0} of quarter {1} {2} ", (dataInd != null ? dataInd.Indicator : ""), nQuarter, nYear); // 0 = Group Indicator,1=Quater,2=Year
            Workflow.DataMail_log Log = new Workflow.DataMail_log();
            Log = SystemFunction.SendMailAll(sFrom, strTo, "", "", sSubject, sMsgSend, "");
            new Workflow().SaveLogMail(Log);
        }

        public void SendEmail_AcceptEditRequestByPTT(List<int> lstSubFacility, int nIndID, int nYear, int nQuarter, string sComment)
        {
            string sMsg = @"Dear All, <br />" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Environmental Report has been accepted request edit by ptt as detailed below.<br />" +
                        "Facility : {0}<br />" +
                        "Group Indicator : {1}<br />" +
                        "Year : {2}<br />" +
                        "Quarter : {3}<br /><br />" +
                        "Comment : {4}<br /><br />" +
                        "Click <a href='{5}' target='_blank'>link</a> to view for further action.";
            var dataFac = db.mTFacility.Where(w => w.cDel == "N" && w.cActive == "Y" && w.nLevel == 2 && lstSubFacility.Contains(w.ID)).ToList();
            string strFacName = String.Join(", ", dataFac.Select(s => s.Name).OrderBy(o => o).ToList());
            var dataInd = db.mTIndicator.FirstOrDefault(w => w.ID == nIndID);

            string _sUrl = sUrlWebSite + "AD/loginAD.aspx";
            string sFrom = System.Configuration.ConfigurationSettings.AppSettings["SystemMail"] + "";
            var dataToUser = (from d in db.mTWorkFlow.Where(w => w.IDIndicator == nIndID && lstSubFacility.Contains(w.IDFac))
                              from u in db.mTUser.Where(w => w.cDel == "N" && w.cActive == "Y" && w.ID == d.L2)
                              select new
                              {
                                  u.Email
                              }).GroupBy(g => g.Email).Select(s => s.Key).ToList();
            string strTo = String.Join(",", dataToUser);
            string sMsgSend = string.Format(sMsg, strFacName, (dataInd != null ? dataInd.Indicator : ""), nYear, nQuarter, sComment, _sUrl);
            string sSubject = string.Format("Accepted request edit by ptt of {0} of quarter {1} {2} ", (dataInd != null ? dataInd.Indicator : ""), nQuarter, nYear); // 0 = Group Indicator,1=Quater,2=Year
            Workflow.DataMail_log Log = new Workflow.DataMail_log();
            Log = SystemFunction.SendMailAll(sFrom, strTo, "", "", sSubject, sMsgSend, "");
            new Workflow().SaveLogMail(Log);
        }
    }

    namespace ENVIWorkFlow
    {
        public class ENVIWorkFlow
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();

            public ENVIService.ENVIClass_Result Workflow_PTTApproveL3(ENVIService.ENVIClass_ENVI dataENVI)
            {
                ENVIService.ENVIClass_Result result = new ENVIService.ENVIClass_Result();
                var dataFacPTT = db.mTFacility.FirstOrDefault(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 0 && w.CompanyID == 1 && w.sMappingCodePTT == dataENVI.FacilityCode);
                if (dataFacPTT != null)
                {
                    var dataFacilityGC = db.mTFacility.FirstOrDefault(w => w.nLevel == 1 && w.cDel == "N" && w.cActive == "Y" && w.nHeaderID == dataFacPTT.ID);
                    if (dataFacilityGC != null)
                    {
                        if (dataENVI.Quarter == 1 || dataENVI.Quarter == 2 || dataENVI.Quarter == 3 || dataENVI.Quarter == 4)
                        {
                            int nIndID = 0;
                            switch (dataENVI.IndicatorCode)
                            {
                                case ENVIService.EPIClass_DataType.InidcatorCode.IntensityDenominator: nIndID = API_ENVI.DataType.nIndIntensityID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Material: nIndID = API_ENVI.DataType.nIndMaterialID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Water: nIndID = API_ENVI.DataType.nIndWaterID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Waste: nIndID = API_ENVI.DataType.nIndWasteID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Emission: nIndID = API_ENVI.DataType.nIndEmissionID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Effluent: nIndID = API_ENVI.DataType.nIndEffluentID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Spill: nIndID = API_ENVI.DataType.nIndSpillID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Complaint: nIndID = API_ENVI.DataType.nIndComplaintID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Compliance: nIndID = API_ENVI.DataType.nIndComplianceID; break;
                            }

                            if (nIndID != 0)
                            {
                                int nStatusApprove = 30;
                                int[] arrAllowedStatusReject = new int[] { 28, 32 };
                                var dataTransfer = db.TEPI_TransferPTT.FirstOrDefault(w => w.nYear == dataENVI.Year && w.nIndicatorID == nIndID && w.nQuarter == dataENVI.Quarter && w.nFacilityID == dataFacilityGC.ID && arrAllowedStatusReject.Contains(w.nStatusID ?? 0));
                                if (dataTransfer != null)
                                {
                                    //TEPI_TransferPTT
                                    dataTransfer.nStatusID = nStatusApprove;
                                    dataTransfer.dAction = DateTime.Now;

                                    //TEPI_TransferPTT_SubFacility
                                    List<int> lstSubFac = new List<int>();
                                    var querySubFac = db.TEPI_TransferPTT_SubFacility.Where(w => w.nHeaderID == dataTransfer.nFacilityID && w.nYear == dataTransfer.nYear && w.nIndicatorID == dataTransfer.nIndicatorID && w.nQuarterID == dataTransfer.nQuarter).ToList();
                                    foreach (var item in querySubFac)
                                    {
                                        lstSubFac.Add(item.nFacilityID);
                                        item.nStatusID = nStatusApprove;
                                        item.dAction = DateTime.Now;
                                    }

                                    #region TEPI_TransferPTT_Log
                                    TEPI_TransferPTT_Log tl = new TEPI_TransferPTT_Log();
                                    tl.nYear = dataENVI.Year;
                                    tl.nFacilityID = dataFacilityGC.ID;
                                    tl.nIndicatorID = nIndID;
                                    tl.nQuarter = dataENVI.Quarter;
                                    tl.nStatusID = nStatusApprove;
                                    tl.nActionBy = -1;
                                    tl.dAction = DateTime.Now;
                                    tl.sRemark = dataENVI.Comment;
                                    db.TEPI_TransferPTT_Log.Add(tl);
                                    db.SaveChanges();
                                    #endregion

                                    #region TEPI_Workflow
                                    string sYear = dataENVI.Year + "";
                                    var dataEPIFrom = db.TEPI_Forms.Where(w => w.IDIndicator == nIndID && w.sYear == sYear && lstSubFac.Contains(w.FacilityID ?? 0)).ToList();
                                    foreach (var item in dataEPIFrom)
                                    {
                                        List<int> lstMonth = new List<int>();
                                        switch (dataENVI.Quarter)
                                        {
                                            case 1: lstMonth.Add(1); lstMonth.Add(2); lstMonth.Add(3); break;
                                            case 2: lstMonth.Add(4); lstMonth.Add(5); lstMonth.Add(6); break;
                                            case 3: lstMonth.Add(7); lstMonth.Add(8); lstMonth.Add(9); break;
                                            case 4: lstMonth.Add(10); lstMonth.Add(11); lstMonth.Add(12); break;
                                        }

                                        var queryUpdateWF = db.TEPI_Workflow.Where(w => w.FormID == item.FormID && lstMonth.Contains(w.nMonth)).ToList();
                                        foreach (var itemWF in queryUpdateWF)
                                        {
                                            itemWF.nHistoryStatusID = 26;//Completed(L3);
                                        }
                                    }
                                    #endregion

                                    db.SaveChanges();

                                    new API_ENVI().SendEmail_ApproveByPTT(lstSubFac, nIndID, dataENVI.Year, dataENVI.Quarter, dataENVI.Comment);
                                    result.IsCompleted = true;
                                }
                                else
                                {
                                    dataTransfer = db.TEPI_TransferPTT.FirstOrDefault(w => w.nYear == dataENVI.Year && w.nIndicatorID == nIndID && w.nQuarter == dataENVI.Quarter && w.nFacilityID == dataFacilityGC.ID);
                                    if (dataTransfer != null && dataTransfer.nStatusID == nStatusApprove)//เพื่อให้รองรับกรณีที่ PTT(L4) reject to L3 and L3 Approve to L4
                                    {
                                        result.IsCompleted = true;
                                    }
                                    else
                                    {
                                        result.IsCompleted = false;
                                        result.Message = "Update Status Failed.";
                                    }
                                }
                            }
                            else
                            {
                                result.IsCompleted = false;
                                result.Message = "Invalid Indicator Code.";
                            }
                        }
                        else
                        {
                            result.IsCompleted = false;
                            result.Message = "Invalid Quater.";
                        }
                    }
                    else
                    {
                        result.IsCompleted = false;
                        result.Message = "Not Found GC Facility";
                    }
                }
                else
                {
                    result.IsCompleted = false;
                    result.Message = "Not Found Facility Code.";
                }
                return result;
            }

            public ENVIService.ENVIClass_Result Workflow_PTTReject(ENVIService.ENVIClass_ENVI dataENVI)
            {
                ENVIService.ENVIClass_Result result = new ENVIService.ENVIClass_Result();
                var dataFacPTT = db.mTFacility.FirstOrDefault(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 0 && w.CompanyID == 1 && w.sMappingCodePTT == dataENVI.FacilityCode);
                if (dataFacPTT != null)
                {
                    var dataFacilityGC = db.mTFacility.FirstOrDefault(w => w.nLevel == 1 && w.cDel == "N" && w.cActive == "Y" && w.nHeaderID == dataFacPTT.ID);
                    if (dataFacilityGC != null)
                    {
                        if (dataENVI.Quarter == 1 || dataENVI.Quarter == 2 || dataENVI.Quarter == 3 || dataENVI.Quarter == 4)
                        {
                            int nIndID = 0;
                            switch (dataENVI.IndicatorCode)
                            {
                                case ENVIService.EPIClass_DataType.InidcatorCode.IntensityDenominator: nIndID = API_ENVI.DataType.nIndIntensityID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Material: nIndID = API_ENVI.DataType.nIndMaterialID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Water: nIndID = API_ENVI.DataType.nIndWaterID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Waste: nIndID = API_ENVI.DataType.nIndWasteID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Emission: nIndID = API_ENVI.DataType.nIndEmissionID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Effluent: nIndID = API_ENVI.DataType.nIndEffluentID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Spill: nIndID = API_ENVI.DataType.nIndSpillID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Complaint: nIndID = API_ENVI.DataType.nIndComplaintID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Compliance: nIndID = API_ENVI.DataType.nIndComplianceID; break;
                            }

                            if (nIndID != 0)
                            {
                                int nStatusReject = 29;
                                int[] arrAllowedStatusReject = new int[] { 28, 30, 32 };
                                var dataTransfer = db.TEPI_TransferPTT.FirstOrDefault(w => w.nYear == dataENVI.Year && w.nIndicatorID == nIndID && w.nQuarter == dataENVI.Quarter && w.nFacilityID == dataFacilityGC.ID && arrAllowedStatusReject.Contains(w.nStatusID ?? 0));
                                if (dataTransfer != null)
                                {
                                    //TEPI_TransferPTT
                                    dataTransfer.nStatusID = nStatusReject;
                                    dataTransfer.dAction = DateTime.Now;

                                    //TEPI_TransferPTT_SubFacility
                                    List<int> lstSubFac = new List<int>();
                                    var querySubFac = db.TEPI_TransferPTT_SubFacility.Where(w => w.nHeaderID == dataTransfer.nFacilityID && w.nYear == dataTransfer.nYear && w.nIndicatorID == dataTransfer.nIndicatorID && w.nQuarterID == dataTransfer.nQuarter).ToList();
                                    foreach (var item in querySubFac)
                                    {
                                        lstSubFac.Add(item.nFacilityID);
                                        item.nStatusID = nStatusReject;
                                        item.dAction = DateTime.Now;
                                    }

                                    #region TEPI_TransferPTT_Log
                                    TEPI_TransferPTT_Log tl = new TEPI_TransferPTT_Log();
                                    tl.nYear = dataENVI.Year;
                                    tl.nFacilityID = dataFacilityGC.ID;
                                    tl.nIndicatorID = nIndID;
                                    tl.nQuarter = dataENVI.Quarter;
                                    tl.nStatusID = nStatusReject;
                                    tl.nActionBy = -1;
                                    tl.dAction = DateTime.Now;
                                    tl.sRemark = dataENVI.Comment;
                                    db.TEPI_TransferPTT_Log.Add(tl);
                                    db.SaveChanges();
                                    #endregion

                                    #region TEPI_Workflow
                                    string sYear = dataENVI.Year + "";
                                    var dataEPIFrom = db.TEPI_Forms.Where(w => w.IDIndicator == nIndID && w.sYear == sYear && lstSubFac.Contains(w.FacilityID ?? 0)).ToList();
                                    foreach (var item in dataEPIFrom)
                                    {
                                        List<int> lstMonth = new List<int>();
                                        switch (dataENVI.Quarter)
                                        {
                                            case 1: lstMonth.Add(1); lstMonth.Add(2); lstMonth.Add(3); break;
                                            case 2: lstMonth.Add(4); lstMonth.Add(5); lstMonth.Add(6); break;
                                            case 3: lstMonth.Add(7); lstMonth.Add(8); lstMonth.Add(9); break;
                                            case 4: lstMonth.Add(10); lstMonth.Add(11); lstMonth.Add(12); break;
                                        }

                                        var queryUpdateWF = db.TEPI_Workflow.Where(w => w.FormID == item.FormID && lstMonth.Contains(w.nMonth)).ToList();
                                        foreach (var itemWF in queryUpdateWF)
                                        {
                                            if (itemWF.nHistoryStatusID == 26)//Completed (L3) >> Approved by PTT
                                            {
                                                itemWF.nHistoryStatusID = 5;//Approved by ENVI Corporate
                                            }
                                        }
                                    }
                                    #endregion

                                    db.SaveChanges();

                                    new API_ENVI().SendEmail_RejectByPTT(lstSubFac, nIndID, dataENVI.Year, dataENVI.Quarter, dataENVI.Comment);
                                    result.IsCompleted = true;
                                }
                                else
                                {
                                    result.IsCompleted = false;
                                    result.Message = "Cannot Reject.";
                                }
                            }
                            else
                            {
                                result.IsCompleted = false;
                                result.Message = "Invalid Indicator Code.";
                            }
                        }
                        else
                        {
                            result.IsCompleted = false;
                            result.Message = "Invalid Quater.";
                        }
                    }
                    else
                    {
                        result.IsCompleted = false;
                        result.Message = "Not Found GC Facility";
                    }
                }
                else
                {
                    result.IsCompleted = false;
                    result.Message = "Not Found Facility Code.";
                }
                return result;
            }

            public ENVIService.ENVIClass_Result Workflow_PTTAcceptEditRequest(ENVIService.ENVIClass_ENVI dataENVI)
            {
                ENVIService.ENVIClass_Result result = new ENVIService.ENVIClass_Result();
                var dataFacPTT = db.mTFacility.FirstOrDefault(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 0 && w.CompanyID == 1 && w.sMappingCodePTT == dataENVI.FacilityCode);
                if (dataFacPTT != null)
                {
                    var dataFacilityGC = db.mTFacility.FirstOrDefault(w => w.nLevel == 1 && w.cDel == "N" && w.cActive == "Y" && w.nHeaderID == dataFacPTT.ID);
                    if (dataFacilityGC != null)
                    {
                        if (dataENVI.Quarter == 1 || dataENVI.Quarter == 2 || dataENVI.Quarter == 3 || dataENVI.Quarter == 4)
                        {
                            int nIndID = 0;
                            switch (dataENVI.IndicatorCode)
                            {
                                case ENVIService.EPIClass_DataType.InidcatorCode.IntensityDenominator: nIndID = API_ENVI.DataType.nIndIntensityID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Material: nIndID = API_ENVI.DataType.nIndMaterialID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Water: nIndID = API_ENVI.DataType.nIndWaterID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Waste: nIndID = API_ENVI.DataType.nIndWasteID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Emission: nIndID = API_ENVI.DataType.nIndEmissionID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Effluent: nIndID = API_ENVI.DataType.nIndEffluentID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Spill: nIndID = API_ENVI.DataType.nIndSpillID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Complaint: nIndID = API_ENVI.DataType.nIndComplaintID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Compliance: nIndID = API_ENVI.DataType.nIndComplianceID; break;
                            }

                            if (nIndID != 0)
                            {
                                int nStatusAccept = 33;
                                int[] arrAllowedStatusReject = new int[] { 32 };
                                var dataTransfer = db.TEPI_TransferPTT.FirstOrDefault(w => w.nYear == dataENVI.Year && w.nIndicatorID == nIndID && w.nQuarter == dataENVI.Quarter && w.nFacilityID == dataFacilityGC.ID && arrAllowedStatusReject.Contains(w.nStatusID ?? 0));
                                if (dataTransfer != null)
                                {
                                    //TEPI_TransferPTT
                                    dataTransfer.nStatusID = nStatusAccept;
                                    dataTransfer.dAction = DateTime.Now;

                                    //TEPI_TransferPTT_SubFacility
                                    List<int> lstSubFac = new List<int>();
                                    var querySubFac = db.TEPI_TransferPTT_SubFacility.Where(w => w.nHeaderID == dataTransfer.nFacilityID && w.nYear == dataTransfer.nYear && w.nIndicatorID == dataTransfer.nIndicatorID && w.nQuarterID == dataTransfer.nQuarter).ToList();
                                    foreach (var item in querySubFac)
                                    {
                                        lstSubFac.Add(item.nFacilityID);
                                        item.nStatusID = nStatusAccept;
                                        item.dAction = DateTime.Now;
                                    }

                                    #region TEPI_TransferPTT_Log
                                    TEPI_TransferPTT_Log tl = new TEPI_TransferPTT_Log();
                                    tl.nYear = dataENVI.Year;
                                    tl.nFacilityID = dataFacilityGC.ID;
                                    tl.nIndicatorID = nIndID;
                                    tl.nQuarter = dataENVI.Quarter;
                                    tl.nStatusID = nStatusAccept;
                                    tl.nActionBy = -1;
                                    tl.dAction = DateTime.Now;
                                    tl.sRemark = dataENVI.Comment;
                                    db.TEPI_TransferPTT_Log.Add(tl);
                                    db.SaveChanges();
                                    #endregion

                                    #region TEPI_Workflow
                                    string sYear = dataENVI.Year + "";
                                    var dataEPIFrom = db.TEPI_Forms.Where(w => w.IDIndicator == nIndID && w.sYear == sYear && lstSubFac.Contains(w.FacilityID ?? 0)).ToList();
                                    foreach (var item in dataEPIFrom)
                                    {
                                        List<int> lstMonth = new List<int>();
                                        switch (dataENVI.Quarter)
                                        {
                                            case 1: lstMonth.Add(1); lstMonth.Add(2); lstMonth.Add(3); break;
                                            case 2: lstMonth.Add(4); lstMonth.Add(5); lstMonth.Add(6); break;
                                            case 3: lstMonth.Add(7); lstMonth.Add(8); lstMonth.Add(9); break;
                                            case 4: lstMonth.Add(10); lstMonth.Add(11); lstMonth.Add(12); break;
                                        }

                                        var queryUpdateWF = db.TEPI_Workflow.Where(w => w.FormID == item.FormID && lstMonth.Contains(w.nMonth)).ToList();
                                        foreach (var itemWF in queryUpdateWF)
                                        {
                                            if (itemWF.nHistoryStatusID == 26)//Completed (L3) >> Approved by PTT
                                            {
                                                itemWF.nHistoryStatusID = 5;//Approved by ENVI Corporate
                                            }
                                        }
                                    }
                                    #endregion

                                    db.SaveChanges();

                                    new API_ENVI().SendEmail_AcceptEditRequestByPTT(lstSubFac, nIndID, dataENVI.Year, dataENVI.Quarter, dataENVI.Comment);
                                    result.IsCompleted = true;
                                }
                                else
                                {
                                    result.IsCompleted = false;
                                    result.Message = "Update Status Failed.";
                                }
                            }
                            else
                            {
                                result.IsCompleted = false;
                                result.Message = "Invalid Indicator Code.";
                            }
                        }
                        else
                        {
                            result.IsCompleted = false;
                            result.Message = "Invalid Quater.";
                        }
                    }
                    else
                    {
                        result.IsCompleted = false;
                        result.Message = "Not Found GC Facility";
                    }
                }
                else
                {
                    result.IsCompleted = false;
                    result.Message = "Not Found Facility Code.";
                }
                return result;
            }

            public ENVIService.ENVIClass_Result Workflow_PTTClearWF(ENVIService.ENVIClass_ENVI dataENVI)
            {
                ENVIService.ENVIClass_Result result = new ENVIService.ENVIClass_Result();
                var dataFacPTT = db.mTFacility.FirstOrDefault(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 0 && w.CompanyID == 1 && w.sMappingCodePTT == dataENVI.FacilityCode);
                if (dataFacPTT != null)
                {
                    var dataFacilityGC = db.mTFacility.FirstOrDefault(w => w.nLevel == 1 && w.cDel == "N" && w.cActive == "Y" && w.nHeaderID == dataFacPTT.ID);
                    if (dataFacilityGC != null)
                    {
                        if (dataENVI.Quarter == 1 || dataENVI.Quarter == 2 || dataENVI.Quarter == 3 || dataENVI.Quarter == 4)
                        {
                            int nIndID = 0;
                            switch (dataENVI.IndicatorCode)
                            {
                                case ENVIService.EPIClass_DataType.InidcatorCode.IntensityDenominator: nIndID = API_ENVI.DataType.nIndIntensityID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Material: nIndID = API_ENVI.DataType.nIndMaterialID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Water: nIndID = API_ENVI.DataType.nIndWaterID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Waste: nIndID = API_ENVI.DataType.nIndWasteID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Emission: nIndID = API_ENVI.DataType.nIndEmissionID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Effluent: nIndID = API_ENVI.DataType.nIndEffluentID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Spill: nIndID = API_ENVI.DataType.nIndSpillID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Complaint: nIndID = API_ENVI.DataType.nIndComplaintID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Compliance: nIndID = API_ENVI.DataType.nIndComplianceID; break;
                            }

                            if (nIndID != 0)
                            {
                                int nStatusWaitingSubmit = 0;
                                var dataTransfer = db.TEPI_TransferPTT.FirstOrDefault(w => w.nYear == dataENVI.Year && w.nIndicatorID == nIndID && w.nQuarter == dataENVI.Quarter && w.nFacilityID == dataFacilityGC.ID);
                                if (dataTransfer != null)
                                {
                                    //TEPI_TransferPTT
                                    dataTransfer.nStatusID = nStatusWaitingSubmit;
                                    dataTransfer.dAction = DateTime.Now;

                                    //TEPI_TransferPTT_SubFacility
                                    List<int> lstSubFac = new List<int>();
                                    var querySubFac = db.TEPI_TransferPTT_SubFacility.Where(w => w.nHeaderID == dataTransfer.nFacilityID && w.nYear == dataTransfer.nYear && w.nIndicatorID == dataTransfer.nIndicatorID && w.nQuarterID == dataTransfer.nQuarter).ToList();
                                    foreach (var item in querySubFac)
                                    {
                                        lstSubFac.Add(item.nFacilityID);
                                        item.nStatusID = nStatusWaitingSubmit;
                                        item.dAction = DateTime.Now;
                                    }

                                    #region TEPI_TransferPTT_Log
                                    TEPI_TransferPTT_Log tl = new TEPI_TransferPTT_Log();
                                    tl.nYear = dataENVI.Year;
                                    tl.nFacilityID = dataFacilityGC.ID;
                                    tl.nIndicatorID = nIndID;
                                    tl.nQuarter = dataENVI.Quarter;
                                    tl.nStatusID = nStatusWaitingSubmit;
                                    tl.nActionBy = -1;
                                    tl.dAction = DateTime.Now;
                                    tl.sRemark = dataENVI.Comment;
                                    db.TEPI_TransferPTT_Log.Add(tl);
                                    db.SaveChanges();
                                    #endregion

                                    #region TEPI_Workflow
                                    string sYear = dataENVI.Year + "";
                                    var dataEPIFrom = db.TEPI_Forms.Where(w => w.IDIndicator == nIndID && w.sYear == sYear && lstSubFac.Contains(w.FacilityID ?? 0)).ToList();
                                    foreach (var item in dataEPIFrom)
                                    {
                                        List<int> lstMonth = new List<int>();
                                        switch (dataENVI.Quarter)
                                        {
                                            case 1: lstMonth.Add(1); lstMonth.Add(2); lstMonth.Add(3); break;
                                            case 2: lstMonth.Add(4); lstMonth.Add(5); lstMonth.Add(6); break;
                                            case 3: lstMonth.Add(7); lstMonth.Add(8); lstMonth.Add(9); break;
                                            case 4: lstMonth.Add(10); lstMonth.Add(11); lstMonth.Add(12); break;
                                        }

                                        var queryUpdateWF = db.TEPI_Workflow.Where(w => w.FormID == item.FormID && lstMonth.Contains(w.nMonth)).ToList();
                                        foreach (var itemWF in queryUpdateWF)
                                        {
                                            if (itemWF.nHistoryStatusID == 26)//Completed (L3) >> Approved by PTT
                                            {
                                                itemWF.nHistoryStatusID = 5;//Approved by ENVI Corporate
                                            }
                                        }
                                    }
                                    #endregion

                                    db.SaveChanges();
                                    result.IsCompleted = true;
                                }
                                else
                                {
                                    result.IsCompleted = false;
                                    result.Message = "Update Status Failed.";
                                }
                            }
                            else
                            {
                                result.IsCompleted = false;
                                result.Message = "Invalid Indicator Code.";
                            }
                        }
                        else
                        {
                            result.IsCompleted = false;
                            result.Message = "Invalid Quater.";
                        }
                    }
                    else
                    {
                        result.IsCompleted = false;
                        result.Message = "Not Found GC Facility";
                    }
                }
                else
                {
                    result.IsCompleted = false;
                    result.Message = "Not Found Facility Code.";
                }
                return result;
            }

            public ENVIService.ENVIClass_Result Workflow_PTTApproveL4(ENVIService.ENVIClass_ENVI dataENVI)
            {
                ENVIService.ENVIClass_Result result = new ENVIService.ENVIClass_Result();
                var dataFacPTT = db.mTFacility.FirstOrDefault(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 0 && w.CompanyID == 1 && w.sMappingCodePTT == dataENVI.FacilityCode);
                if (dataFacPTT != null)
                {
                    var dataFacilityGC = db.mTFacility.FirstOrDefault(w => w.nLevel == 1 && w.cDel == "N" && w.cActive == "Y" && w.nHeaderID == dataFacPTT.ID);
                    if (dataFacilityGC != null)
                    {
                        if (dataENVI.Quarter == 1 || dataENVI.Quarter == 2 || dataENVI.Quarter == 3 || dataENVI.Quarter == 4)
                        {
                            int nIndID = 0;
                            switch (dataENVI.IndicatorCode)
                            {
                                case ENVIService.EPIClass_DataType.InidcatorCode.IntensityDenominator: nIndID = API_ENVI.DataType.nIndIntensityID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Material: nIndID = API_ENVI.DataType.nIndMaterialID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Water: nIndID = API_ENVI.DataType.nIndWaterID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Waste: nIndID = API_ENVI.DataType.nIndWasteID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Emission: nIndID = API_ENVI.DataType.nIndEmissionID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Effluent: nIndID = API_ENVI.DataType.nIndEffluentID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Spill: nIndID = API_ENVI.DataType.nIndSpillID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Complaint: nIndID = API_ENVI.DataType.nIndComplaintID; break;
                                case ENVIService.EPIClass_DataType.InidcatorCode.Compliance: nIndID = API_ENVI.DataType.nIndComplianceID; break;
                            }

                            if (nIndID != 0)
                            {
                                int nStatusApprove = 34;
                                int[] arrAllowedStatus = new int[] { 30 };//Approve by PTT(L3)
                                var dataTransfer = db.TEPI_TransferPTT.FirstOrDefault(w => w.nYear == dataENVI.Year && w.nIndicatorID == nIndID && w.nQuarter == dataENVI.Quarter && w.nFacilityID == dataFacilityGC.ID && arrAllowedStatus.Contains(w.nStatusID ?? 0));
                                if (dataTransfer != null)
                                {
                                    //TEPI_TransferPTT
                                    dataTransfer.nStatusID = nStatusApprove;
                                    dataTransfer.dAction = DateTime.Now;

                                    //TEPI_TransferPTT_SubFacility
                                    List<int> lstSubFac = new List<int>();
                                    var querySubFac = db.TEPI_TransferPTT_SubFacility.Where(w => w.nHeaderID == dataTransfer.nFacilityID && w.nYear == dataTransfer.nYear && w.nIndicatorID == dataTransfer.nIndicatorID && w.nQuarterID == dataTransfer.nQuarter).ToList();
                                    foreach (var item in querySubFac)
                                    {
                                        lstSubFac.Add(item.nFacilityID);
                                        item.nStatusID = nStatusApprove;
                                        item.dAction = DateTime.Now;
                                    }

                                    #region TEPI_TransferPTT_Log
                                    TEPI_TransferPTT_Log tl = new TEPI_TransferPTT_Log();
                                    tl.nYear = dataENVI.Year;
                                    tl.nFacilityID = dataFacilityGC.ID;
                                    tl.nIndicatorID = nIndID;
                                    tl.nQuarter = dataENVI.Quarter;
                                    tl.nStatusID = nStatusApprove;
                                    tl.nActionBy = -1;
                                    tl.dAction = DateTime.Now;
                                    tl.sRemark = dataENVI.Comment;
                                    db.TEPI_TransferPTT_Log.Add(tl);
                                    db.SaveChanges();
                                    #endregion

                                    db.SaveChanges();
                                    result.IsCompleted = true;
                                }
                                else
                                {
                                    result.IsCompleted = false;
                                    result.Message = "Update Status Failed.";
                                }
                            }
                            else
                            {
                                result.IsCompleted = false;
                                result.Message = "Invalid Indicator Code.";
                            }
                        }
                        else
                        {
                            result.IsCompleted = false;
                            result.Message = "Invalid Quater.";
                        }
                    }
                    else
                    {
                        result.IsCompleted = false;
                        result.Message = "Not Found GC Facility";
                    }
                }
                else
                {
                    result.IsCompleted = false;
                    result.Message = "Not Found Facility Code.";
                }
                return result;
            }
        }
    }
}