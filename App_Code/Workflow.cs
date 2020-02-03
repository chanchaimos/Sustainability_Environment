using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Workflow
/// </summary>
public class Workflow
{
    PTTGC_EPIEntities db = new PTTGC_EPIEntities();
    public string sUrl = ConfigurationSettings.AppSettings["UrlSite"].ToString();
    #region Mail WorkFlow
    [Serializable]
    public class DataResultMail
    {
        public bool IsPass { get; set; }
        public string sContent { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sMode">
    ///     SM = Submit
    ///     RC = Recall
    ///     RQ = Request Edit
    ///     APC = Approve With Edit Content
    ///     AP = Approve
    ///     RJ = Reject 
    ///     ACE = Accept Edit Request
    /// </param>
    /// <returns> string Content </returns>
    public DataResultMail MailTemPlete_Subject(string sMode)
    {
        DataResultMail r = new DataResultMail();
        r.IsPass = false;
        r.sContent = "";
        if (sMode == "SM")
        {
            r.IsPass = true;
            r.sContent = "For Submit of {0} of {1} {2} "; // 0 = Group Indicator,1=Month,2=year
        }
        else if (sMode == "RC")
        {
            r.IsPass = true;
            r.sContent = "Recall of {0} of {1} {2}"; // 0 = Group Indicator,1=Month,2=year
        }
        else if (sMode == "RQ")
        {
            r.IsPass = true;
            r.sContent = "Request Edit of {0} of {1} {2} "; // 0 = Group Indicator,1=Month,2=year
        }
        else if (sMode == "APC")
        {
            r.IsPass = true;
            r.sContent = "For Approval with edit content of {0} of {1} {2} "; // 0 = Group Indicator,1=Month,2=year
        }
        else if (sMode == "AP")
        {
            r.IsPass = true;
            r.sContent = "For Approval of {0} of {1} {2} "; // 0 = Group Indicator,1=Month,2=year
        }
        else if (sMode == "RJ")
        {
            r.IsPass = true;
            r.sContent = "Reject of {0} of {1} {2}"; // 0 = Group Indicator,1=Month,2=year
        }
        else if (sMode == "ACE")
        {
            r.IsPass = true;
            r.sContent = "For Accept Request Edit of {0} of {1} {2} "; // 0 = Group Indicator,1=Month,2=year
        }
        return r;
    }

    public DataResultMail MailTemPlete_Content(string sMode)
    {
        DataResultMail r = new DataResultMail();
        r.IsPass = false;
        r.sContent = "";
        if (sMode == "SM")
        {
            r.IsPass = true;
            r.sContent = "Dear {0} <br />" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Environmental Report has been submitted for your approval as detailed below.<br />" +
                        "Facility : {1}<br />" +
                        "Group Indicator : {2}<br />" +
                        "Year : {3}<br />" +
                        "Month : {4}<br /><br />" +
                        "Click <a href='{5}' target='_blank'>link</a> to view for further action.";
        }
        else if (sMode == "RC")
        {
            r.IsPass = true;
            r.sContent = "Dear {0} <br />" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Environmental Report has been recalled to edit as detailed below.<br />" +
                        "Facility : {1}<br />" +
                        "Group Indicator : {2}<br />" +
                        "Year : {3}<br />" +
                        "Month : {4}<br /><br />" +
                        "Comment : {6}<br /><br />" +
                        "Click <a href='{5}' target='_blank'>link</a> to view for further action.";
        }
        else if (sMode == "RQ")
        {
            r.IsPass = true;
            r.sContent = "Dear {0} <br />" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Environmental Report has been requested to edit as detailed below.<br />" +
                        "Facility : {1}<br />" +
                        "Group Indicator : {2}<br />" +
                        "Year : {3}<br />" +
                        "Month : {4}<br /><br />" +
                        "Comment : {6}<br /><br />" +
                        "Click <a href='{5}' target='_blank'>link</a> to view for further action.";
        }
        else if (sMode == "APC")
        {
            r.IsPass = true;
            r.sContent = "Dear {0} <br />" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Environmental Report has been submitted for your approval as detailed below.<br />" +
                        "Facility : {1}<br />" +
                        "Group Indicator : {2}<br />" +
                        "Year : {3}<br />" +
                        "Month : {4}<br /><br />" +
                        "Click <a href='{5}' target='_blank'>link</a> to view for further action.";
        }
        else if (sMode == "AP")
        {
            r.IsPass = true;
            r.sContent = "Dear {0} <br />" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Environmental Report has been submitted for your approval as detailed below.<br />" +
                        "Facility : {1}<br />" +
                        "Group Indicator : {2}<br />" +
                        "Year : {3}<br />" +
                        "Month : {4}<br /><br />" +
                        "Click <a href='{5}' target='_blank'>link</a> to view for further action.";
        }
        else if (sMode == "RJ")
        {
            r.IsPass = true;
            r.sContent = "Dear {0} <br />" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Environmental Report has been rejected for your edit as detailed below.<br />" +
                        "Facility : {1}<br />" +
                        "Group Indicator : {2}<br />" +
                        "Year : {3}<br />" +
                        "Month : {4}<br /><br />" +
                        "Comment : {6}<br /><br />" +
                        "Click <a href='{5}' target='_blank'>link</a> to view for further action.";
        }
        else if (sMode == "ACE")
        {
            r.IsPass = true;
            r.sContent = "Dear {0} <br />" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Environmental Report has been Accepted to requested edit as detailed below.<br />" +
                        "Facility : {1}<br />" +
                        "Group Indicator : {2}<br />" +
                        "Year : {3}<br />" +
                        "Month : {4}<br /><br />" +
                        "Click <a href='{5}' target='_blank'>link</a> to view for further action.";
        }
        return r;
    }
    #endregion

    /// <summary>
    ///     
    /// </summary>
    /// <param name="FormID"> Form ID </param>
    /// <param name="nMount"> List Month For Change Status </param>
    /// <param name="sMode">
    ///     SM = Submit
    ///     RC = Recall
    ///     RQ = Request Edit
    ///     APC = Approve With Edit Content
    ///     AP = Approve
    ///     RJ = Reject 
    ///     ACE = Accept Edit Request
    /// </param>
    /// <param name="nUserID"></param>
    /// <param name="nRole"></param>
    /// <param name="sComment"> If No Data = ""</param>
    /// <returns></returns>
    public sysGlobalClass.CResutlWebMethod WorkFlowAction(int FormID, List<int> lstMonth, string sMode, int nUserID, int nRole, string sComment)
    {
        sysGlobalClass.CResutlWebMethod r = new sysGlobalClass.CResutlWebMethod();
        string comment = !string.IsNullOrEmpty(sComment) ? sComment : "";
        if (!string.IsNullOrEmpty(sMode))
        {
            var dataEPIForm = db.TEPI_Forms.FirstOrDefault(w => w.FormID == FormID);
            if (dataEPIForm != null)
            {
                r = CheckActiveUser(dataEPIForm.FacilityID ?? 0, dataEPIForm.IDIndicator);
                if (r.Status == SystemFunction.process_Success)
                {
                    if (lstMonth.Any(w => w >= 1 && w <= 12))
                    {
                        switch (sMode)
                        {
                            case "SM":
                                r = Submit(FormID, lstMonth, nUserID, comment);
                                break;

                            case "RC":
                                r = Recall(FormID, lstMonth, nUserID, comment);
                                break;

                            case "RQ":
                                r = RequestEdit(FormID, lstMonth, nUserID, comment);
                                break;

                            case "APC":
                                r = L2_ApproveWith(FormID, lstMonth, nUserID, comment);
                                break;

                            case "AP":
                                if (nRole == 3)
                                {
                                    r = L1_Approve(FormID, lstMonth, nUserID, comment);
                                }//L1
                                else
                                {
                                    r = L2_Approve(FormID, lstMonth, nUserID, comment);
                                }//L2
                                break;

                            case "RJ":
                                if (nRole == 3)
                                {
                                    r = L1_Reject(FormID, lstMonth, nUserID, comment);
                                }//L1
                                else
                                {
                                    r = L2_Reject(FormID, lstMonth, nUserID, comment);
                                }//L2
                                break;

                            case "ACE":
                                r = AcceptRequestEdit(FormID, lstMonth, nUserID, comment);
                                break;

                            default:
                                r.Msg = "No Mode For Action";
                                r.Status = SystemFunction.process_Failed;
                                break;
                        }
                    }
                    else
                    {
                        r.Msg = "Not found month !";
                        r.Status = SystemFunction.process_Failed;
                    }
                }
            }
            else
            {
                r.Msg = "Not found report !";
                r.Status = SystemFunction.process_Failed;
            }
        }
        else
        {
            r.Msg = "No Mode For Action !";
            r.Status = SystemFunction.process_Failed;
        }

        return r;
    }
    [Serializable]
    public class Data_Check
    {
        public string sMonth { get; set; }
        public int nMonth { get; set; }
        public bool IsPass { get; set; }
        public int nReportID { get; set; }
    }

    private sysGlobalClass.CResutlWebMethod CheckActiveUser(int nFacilityID, int nIndicatorID)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        var query = db.mTWorkFlow.FirstOrDefault(w => w.cDel == "N" && w.IDFac == nFacilityID && w.IDIndicator == nIndicatorID);
        if (query != null)
        {
            var qL1 = db.mTUser.FirstOrDefault(w => w.ID == query.L1 && w.cActive == "Y" && w.cDel == "N");
            if (qL1 != null)
            {
                var qRoleL1 = db.mTUserInRole.FirstOrDefault(w => w.nUID == qL1.ID && w.nRoleID == 3);
                if (qRoleL1 != null)
                {
                    var qL2 = db.mTUser.FirstOrDefault(w => w.ID == query.L2 && w.cActive == "Y" && w.cDel == "N");
                    if (qL2 != null)
                    {
                        var qRoleL2 = db.mTUserInRole.FirstOrDefault(w => w.nUID == qL2.ID && w.nRoleID == 4);
                        if (qRoleL2 != null)
                        {
                            result.Status = SystemFunction.process_Success;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Not found ENVI corporate !";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Not found ENVI corporate !";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Not found manager !";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Not found manager !";
            }
        }
        else
        {
            result.Status = SystemFunction.process_Failed;
            result.Msg = "Not found workflow !";
        }
        return result;
    }

    public sysGlobalClass.CResutlWebMethod L1_Approve(int FormID, List<int> lstMonth, int nUserID, string sComment)
    {
        sysGlobalClass.CResutlWebMethod r = new sysGlobalClass.CResutlWebMethod();
        int nStatusTo = 4;
        int nStatusFrom = 0;
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        List<Data_Check> LstDataCheck = new List<Data_Check>();
        var glstData = env.TEPI_Workflow.Where(w => w.FormID == FormID && lstMonth.Contains(w.nMonth)).ToList();
        if (glstData.Any())
        {
            LstDataCheck = glstData.Select(s => new Data_Check
            {
                sMonth = ConvertMonthToString(s.nMonth),
                nMonth = s.nMonth,
                IsPass = s.nStatusID == 1,
                nReportID = s.nReportID
            }).ToList();
        }

        if (LstDataCheck.Any(a => !a.IsPass))
        {
            var gFail = LstDataCheck.Where(w => !w.IsPass).ToList();
            string sMessage = "{0} {1}<br />";
            string sResult = "Cannot Approve <br />";
            int i = 1;
            gFail.ForEach(f =>
            {
                sResult += string.Format(sMessage, i + ".", f.sMonth);
                i++;
            });
            r.Msg = sResult;
            r.Status = SystemFunction.process_Failed;
        }
        else
        {
            LstDataCheck.ForEach(f =>
            {
                var gData = env.TEPI_Workflow.FirstOrDefault(w => w.nReportID == f.nReportID);
                if (gData != null)
                {
                    nStatusFrom = gData.nStatusID.Value;
                    gData.nStatusID = nStatusTo;
                    gData.dAction = DateTime.Now;
                    gData.nActionBy = nUserID;
                    gData.nHistoryStatusID = nStatusTo;
                    env.SaveChanges();

                    SaveLog(gData.nReportID, nStatusTo, nStatusFrom, sComment);
                }
            });
            r.Msg = "Save Success";
            r.Status = SystemFunction.process_Success;

            #region Mail
            if (r.Status == SystemFunction.process_Success)
            {
                DataResultMail cSubject = MailTemPlete_Subject("AP");
                DataResultMail cContent = new DataResultMail();
                if (cSubject.IsPass)
                {
                    cContent = MailTemPlete_Content("AP");
                    if (cContent.IsPass)
                    {
                        Data_Email content = new Data_Email();
                        content = GetAllContentMail(FormID, lstMonth);
                        //  sUrl += "epi_mytask.aspx";
                        sUrl += "AD/loginAD.aspx";
                        string sSubject = string.Format(cSubject.sContent, content.sGroupIndicatorName, content.sMonth, content.sYear);
                        string sMessage = string.Format(cContent.sContent, content.sNameL2, content.sFacilitiesName, content.sGroupIndicatorName, content.sYear, content.sMonth, sUrl);

                        DataMail_log Log = new DataMail_log();
                        Log = SystemFunction.SendMailAll(content.sMailL1, content.sMailL2, content.sMailL0, "", sSubject, sMessage, "");
                        Log.nDataID = FormID;
                        SaveLogMail(Log);
                    }
                }
            }
            #endregion
        }

        return r;
    }

    public sysGlobalClass.CResutlWebMethod L1_Reject(int FormID, List<int> lstMonth, int nUserID, string sComment)
    {
        sysGlobalClass.CResutlWebMethod r = new sysGlobalClass.CResutlWebMethod();
        int nStatusTo = 8;
        int nStatusFrom = 0;
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        List<Data_Check> LstDataCheck = new List<Data_Check>();
        var glstData = env.TEPI_Workflow.Where(w => w.FormID == FormID && lstMonth.Contains(w.nMonth)).ToList();
        if (glstData.Any())
        {
            LstDataCheck = glstData.Select(s => new Data_Check
            {
                sMonth = ConvertMonthToString(s.nMonth),
                nMonth = s.nMonth,
                IsPass = s.nStatusID == 1,
                nReportID = s.nReportID
            }).ToList();
        }

        if (LstDataCheck.Any(a => !a.IsPass))
        {
            var gFail = LstDataCheck.Where(w => !w.IsPass).ToList();
            string sMessage = "{0} {1}<br />";
            string sResult = "Cannot Reject <br />";
            int i = 1;
            gFail.ForEach(f =>
            {
                sResult += string.Format(sMessage, i + ".", f.sMonth);
                i++;
            });
            r.Msg = sResult;
            r.Status = SystemFunction.process_Failed;
        }
        else
        {
            LstDataCheck.ForEach(f =>
            {
                var gData = env.TEPI_Workflow.FirstOrDefault(w => w.nReportID == f.nReportID);
                if (gData != null)
                {
                    nStatusFrom = gData.nStatusID.Value;
                    gData.nStatusID = 0;
                    gData.dAction = DateTime.Now;
                    gData.nActionBy = nUserID;
                    gData.nHistoryStatusID = nStatusTo;
                    env.SaveChanges();

                    SaveLog(gData.nReportID, nStatusTo, nStatusFrom, sComment);
                }
            });
            r.Msg = "Save Success";
            r.Status = SystemFunction.process_Success;

            #region Mail
            if (r.Status == SystemFunction.process_Success)
            {
                DataResultMail cSubject = MailTemPlete_Subject("RJ");
                DataResultMail cContent = new DataResultMail();
                if (cSubject.IsPass)
                {
                    cContent = MailTemPlete_Content("RJ");
                    if (cContent.IsPass)
                    {
                        Data_Email content = new Data_Email();
                        content = GetAllContentMail(FormID, lstMonth);
                        // sUrl += "epi_mytask.aspx";
                        sUrl += "AD/loginAD.aspx";
                        string sSubject = string.Format(cSubject.sContent, content.sGroupIndicatorName, content.sMonth, content.sYear);
                        string sMessage = string.Format(cContent.sContent, "All", content.sFacilitiesName, content.sGroupIndicatorName, content.sYear, content.sMonth, sUrl, (sComment + "").Replace("\n", "<br/>"));

                        DataMail_log Log = new DataMail_log();
                        Log = SystemFunction.SendMailAll(content.sMailL1, content.sMailL0, "", "", sSubject, sMessage, "");
                        Log.nDataID = FormID;
                        SaveLogMail(Log);
                    }
                }
            }
            #endregion
        }

        return r;
    }

    public sysGlobalClass.CResutlWebMethod L2_Approve(int FormID, List<int> lstMonth, int nUserID, string sComment)
    {
        sysGlobalClass.CResutlWebMethod r = new sysGlobalClass.CResutlWebMethod();
        int nStatusTo = 5;
        int nStatusFrom = 0;
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        List<Data_Check> LstDataCheck = new List<Data_Check>();
        var glstData = env.TEPI_Workflow.Where(w => w.FormID == FormID && lstMonth.Contains(w.nMonth)).ToList();
        if (glstData.Any())
        {
            LstDataCheck = glstData.Select(s => new Data_Check
            {
                sMonth = ConvertMonthToString(s.nMonth),
                nMonth = s.nMonth,
                IsPass = s.nStatusID == 4 || s.nStatusID == 2,
                nReportID = s.nReportID
            }).ToList();
        }

        if (LstDataCheck.Any(a => !a.IsPass))
        {
            var gFail = LstDataCheck.Where(w => !w.IsPass).ToList();
            string sMessage = "{0} {1}<br />";
            string sResult = "Cannot Approve <br />";
            int i = 1;
            gFail.ForEach(f =>
            {
                sResult += string.Format(sMessage, i + ".", f.sMonth);
                i++;
            });
            r.Msg = sResult;
            r.Status = SystemFunction.process_Failed;
        }
        else
        {
            LstDataCheck.ForEach(f =>
            {
                var gData = env.TEPI_Workflow.FirstOrDefault(w => w.nReportID == f.nReportID);
                if (gData != null)
                {
                    nStatusFrom = gData.nStatusID.Value;
                    gData.nStatusID = nStatusTo;
                    gData.dAction = DateTime.Now;
                    gData.nActionBy = nUserID;
                    gData.nHistoryStatusID = nStatusTo;
                    env.SaveChanges();

                    SaveLog(gData.nReportID, nStatusTo, nStatusFrom, sComment);
                }
            });
            r.Msg = "Save Success";
            r.Status = SystemFunction.process_Success;

            #region Mail
            if (r.Status == SystemFunction.process_Success)
            {
                DataResultMail cSubject = MailTemPlete_Subject("AP");
                DataResultMail cContent = new DataResultMail();
                if (cSubject.IsPass)
                {
                    cContent = MailTemPlete_Content("AP");
                    if (cContent.IsPass)
                    {
                        Data_Email content = new Data_Email();
                        content = GetAllContentMail(FormID, lstMonth);
                        // sUrl += "epi_mytask.aspx";
                        sUrl += "AD/loginAD.aspx";
                        string sSubject = string.Format(cSubject.sContent, content.sGroupIndicatorName, content.sMonth, content.sYear);
                        string sMessage = string.Format(cContent.sContent, "All", content.sFacilitiesName, content.sGroupIndicatorName, content.sYear, content.sMonth, sUrl);

                        DataMail_log Log = new DataMail_log();
                        Log = SystemFunction.SendMailAll(content.sMailL2, content.sMailL0, content.sMailL1, "", sSubject, sMessage, "");
                        Log.nDataID = FormID;
                        SaveLogMail(Log);

                    }
                }
            }
            #endregion
        }

        return r;
    }

    public sysGlobalClass.CResutlWebMethod L2_Reject(int FormID, List<int> lstMonth, int nUserID, string sComment)
    {
        sysGlobalClass.CResutlWebMethod r = new sysGlobalClass.CResutlWebMethod();
        int nStatusTo = 9;
        int nStatusFrom = 0;
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        List<Data_Check> LstDataCheck = new List<Data_Check>();
        var glstData = env.TEPI_Workflow.Where(w => w.FormID == FormID && lstMonth.Contains(w.nMonth)).ToList();
        if (glstData.Any())
        {
            LstDataCheck = glstData.Select(s => new Data_Check
            {
                sMonth = ConvertMonthToString(s.nMonth),
                nMonth = s.nMonth,
                IsPass = s.nStatusID == 4 || s.nStatusID == 2,
                nReportID = s.nReportID
            }).ToList();
        }

        if (LstDataCheck.Any(a => !a.IsPass))
        {
            var gFail = LstDataCheck.Where(w => !w.IsPass).ToList();
            string sMessage = "{0} {1}<br />";
            string sResult = "Cannot Reject <br />";
            int i = 1;
            gFail.ForEach(f =>
            {
                sResult += string.Format(sMessage, i + ".", f.sMonth);
                i++;
            });
            r.Msg = sResult;
            r.Status = SystemFunction.process_Failed;
        }
        else
        {
            LstDataCheck.ForEach(f =>
            {
                var gData = env.TEPI_Workflow.FirstOrDefault(w => w.nReportID == f.nReportID);
                if (gData != null)
                {
                    nStatusFrom = gData.nStatusID.Value;
                    gData.nStatusID = 0;
                    gData.dAction = DateTime.Now;
                    gData.nActionBy = nUserID;
                    gData.nHistoryStatusID = nStatusTo;
                    env.SaveChanges();

                    SaveLog(gData.nReportID, nStatusTo, nStatusFrom, sComment);
                }
            });
            r.Msg = "Save Success";
            r.Status = SystemFunction.process_Success;

            #region Mail
            if (r.Status == SystemFunction.process_Success)
            {
                DataResultMail cSubject = MailTemPlete_Subject("RJ");
                DataResultMail cContent = new DataResultMail();
                if (cSubject.IsPass)
                {
                    cContent = MailTemPlete_Content("RJ");
                    if (cContent.IsPass)
                    {
                        Data_Email content = new Data_Email();
                        content = GetAllContentMail(FormID, lstMonth);
                        // sUrl += "epi_mytask.aspx";
                        sUrl += "AD/loginAD.aspx";
                        string sSubject = string.Format(cSubject.sContent, content.sGroupIndicatorName, content.sMonth, content.sYear);
                        string sMessage = string.Format(cContent.sContent, "All", content.sFacilitiesName, content.sGroupIndicatorName, content.sYear, content.sMonth, sUrl, (sComment + "").Replace("\n", "<br/>"));

                        DataMail_log Log = new DataMail_log();
                        Log = SystemFunction.SendMailAll(content.sMailL2, content.sMailL0, content.sMailL1, "", sSubject, sMessage, "");
                        Log.nDataID = FormID;
                        SaveLogMail(Log);
                    }
                }
            }
            #endregion
        }

        return r;
    }

    public sysGlobalClass.CResutlWebMethod L2_ApproveWith(int FormID, List<int> lstMonth, int nUserID, string sComment)
    {
        sysGlobalClass.CResutlWebMethod r = new sysGlobalClass.CResutlWebMethod();
        int nStatusTo = 5;
        int nStatusFrom = 0;
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        List<Data_Check> LstDataCheck = new List<Data_Check>();
        var glstData = env.TEPI_Workflow.Where(w => w.FormID == FormID && lstMonth.Contains(w.nMonth)).ToList();
        if (glstData.Any())
        {
            LstDataCheck = glstData.Select(s => new Data_Check
            {
                sMonth = ConvertMonthToString(s.nMonth),
                nMonth = s.nMonth,
                IsPass = s.nStatusID == 2,
                nReportID = s.nReportID
            }).ToList();
        }

        if (LstDataCheck.Any(a => !a.IsPass))
        {
            var gFail = LstDataCheck.Where(w => !w.IsPass).ToList();
            string sMessage = "{0} {1}<br />";
            string sResult = "Cannot Approve <br />";
            int i = 1;
            gFail.ForEach(f =>
            {
                sResult += string.Format(sMessage, i + ".", f.sMonth);
                i++;
            });
            r.Msg = sResult;
            r.Status = SystemFunction.process_Failed;
        }
        else
        {
            LstDataCheck.ForEach(f =>
            {
                var gData = env.TEPI_Workflow.FirstOrDefault(w => w.nReportID == f.nReportID);
                if (gData != null)
                {
                    nStatusFrom = gData.nStatusID.Value;
                    gData.nStatusID = nStatusTo;
                    gData.dAction = DateTime.Now;
                    gData.nActionBy = nUserID;
                    gData.nHistoryStatusID = nStatusTo;
                    env.SaveChanges();

                    SaveLog(gData.nReportID, 27, nStatusFrom, sComment);
                }
            });
            r.Msg = "Save Success";
            r.Status = SystemFunction.process_Success;

            #region Mail
            if (r.Status == SystemFunction.process_Success)
            {
                DataResultMail cSubject = MailTemPlete_Subject("APC");
                DataResultMail cContent = new DataResultMail();
                if (cSubject.IsPass)
                {
                    cContent = MailTemPlete_Content("APC");
                    if (cContent.IsPass)
                    {
                        Data_Email content = new Data_Email();
                        content = GetAllContentMail(FormID, lstMonth);
                        // sUrl += "epi_mytask.aspx";
                        sUrl += "AD/loginAD.aspx";
                        string sSubject = string.Format(cSubject.sContent, content.sGroupIndicatorName, content.sMonth, content.sYear);
                        string sMessage = string.Format(cContent.sContent, "All", content.sFacilitiesName, content.sGroupIndicatorName, content.sYear, content.sMonth, sUrl);


                        DataMail_log Log = new DataMail_log();
                        Log = SystemFunction.SendMailAll(content.sMailL2, content.sMailL0, content.sMailL1, "", sSubject, sMessage, "");
                        Log.nDataID = FormID;
                        SaveLogMail(Log);
                    }
                }
            }
            #endregion
        }

        return r;
    }

    public sysGlobalClass.CResutlWebMethod Submit(int FormID, List<int> lstMonth, int nUserID, string sComment)
    {
        sysGlobalClass.CResutlWebMethod r = new sysGlobalClass.CResutlWebMethod();
        int nStatusTo = 1;
        int nStatusFrom = 0;
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        List<Data_Check> LstDataCheck = new List<Data_Check>();


        var glstData = env.TEPI_Workflow.Where(w => w.FormID == FormID && lstMonth.Contains(w.nMonth)).ToList();
        if (glstData.Any())
        {
            LstDataCheck = glstData.Select(s => new Data_Check
            {
                sMonth = ConvertMonthToString(s.nMonth),
                nMonth = s.nMonth,
                IsPass = s.nStatusID == 0,
                nReportID = s.nReportID
            }).ToList();
        }


        if (LstDataCheck.Any(a => !a.IsPass))
        {
            var gFail = LstDataCheck.Where(w => !w.IsPass).ToList();
            string sMessage = "{0} {1}<br />";
            string sResult = "Cannot Approve <br />";
            int i = 1;
            gFail.ForEach(f =>
            {
                sResult += string.Format(sMessage, i + ".", f.sMonth);
                i++;
            });
            r.Msg = sResult;
            r.Status = SystemFunction.process_Failed;
        }
        else
        {
            #region Action
            LstDataCheck.ForEach(f =>
            {
                var gData = env.TEPI_Workflow.FirstOrDefault(w => w.nReportID == f.nReportID);
                if (gData != null)
                {
                    nStatusFrom = gData.nStatusID.Value;
                    gData.nStatusID = nStatusTo;
                    gData.dAction = DateTime.Now;
                    gData.nActionBy = nUserID;
                    gData.nHistoryStatusID = nStatusTo;
                    env.SaveChanges();

                    SaveLog(gData.nReportID, nStatusTo, nStatusFrom, sComment);
                }
            });
            r.Msg = "Save Success";
            r.Status = SystemFunction.process_Success;
            #endregion

            #region Mail
            if (r.Status == SystemFunction.process_Success)
            {
                DataResultMail cSubject = MailTemPlete_Subject("SM");
                DataResultMail cContent = new DataResultMail();
                if (cSubject.IsPass)
                {
                    cContent = MailTemPlete_Content("SM");
                    if (cContent.IsPass)
                    {
                        Data_Email content = new Data_Email();
                        content = GetAllContentMail(FormID, lstMonth);
                        // sUrl += "epi_mytask.aspx";
                        sUrl += "AD/loginAD.aspx";
                        string sSubject = string.Format(cSubject.sContent, content.sGroupIndicatorName, content.sMonth, content.sYear);
                        string sMessage = string.Format(cContent.sContent, content.sNameL1, content.sFacilitiesName, content.sGroupIndicatorName, content.sYear, content.sMonth, sUrl);

                        DataMail_log Log = new DataMail_log();
                        Log = SystemFunction.SendMailAll(content.sMailL0, content.sMailL1, "", "", sSubject, sMessage, "");
                        Log.nDataID = FormID;
                        SaveLogMail(Log);

                    }
                }
            }
            #endregion
        }

        return r;
    }

    public sysGlobalClass.CResutlWebMethod Recall(int FormID, List<int> lstMonth, int nUserID, string sComment)
    {
        sysGlobalClass.CResutlWebMethod r = new sysGlobalClass.CResutlWebMethod();
        int nStatusTo = 24;
        int nStatusFrom = 0;
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        List<Data_Check> LstDataCheck = new List<Data_Check>();
        var glstData = env.TEPI_Workflow.Where(w => w.FormID == FormID && lstMonth.Contains(w.nMonth)).ToList();
        if (glstData.Any())
        {
            LstDataCheck = glstData.Select(s => new Data_Check
            {
                sMonth = ConvertMonthToString(s.nMonth),
                nMonth = s.nMonth,
                IsPass = s.nStatusID == 1,
                nReportID = s.nReportID
            }).ToList();
        }

        if (LstDataCheck.Any(a => !a.IsPass))
        {
            var gFail = LstDataCheck.Where(w => !w.IsPass).ToList();
            string sMessage = "{0} {1}<br />";
            string sResult = "Cannot Approve <br />";
            int i = 1;
            gFail.ForEach(f =>
            {
                sResult += string.Format(sMessage, i + ".", f.sMonth);
                i++;
            });
            r.Msg = sResult;
            r.Status = SystemFunction.process_Failed;
        }
        else
        {
            LstDataCheck.ForEach(f =>
            {
                var gData = env.TEPI_Workflow.FirstOrDefault(w => w.nReportID == f.nReportID);
                if (gData != null)
                {
                    nStatusFrom = gData.nStatusID.Value;
                    gData.nStatusID = 0;
                    gData.dAction = DateTime.Now;
                    gData.nActionBy = nUserID;
                    gData.nHistoryStatusID = nStatusTo;
                    env.SaveChanges();

                    SaveLog(gData.nReportID, nStatusTo, nStatusFrom, sComment);
                }
            });
            r.Msg = "Save Success";
            r.Status = SystemFunction.process_Success;

            #region Mail
            if (r.Status == SystemFunction.process_Success)
            {
                DataResultMail cSubject = MailTemPlete_Subject("RC");
                DataResultMail cContent = new DataResultMail();
                if (cSubject.IsPass)
                {
                    cContent = MailTemPlete_Content("RC");
                    if (cContent.IsPass)
                    {
                        Data_Email content = new Data_Email();
                        content = GetAllContentMail(FormID, lstMonth);
                        // sUrl += "epi_mytask.aspx";
                        sUrl += "AD/loginAD.aspx";
                        string sSubject = string.Format(cSubject.sContent, content.sGroupIndicatorName, content.sMonth, content.sYear);
                        // string sMessage = string.Format(cContent.sContent, content.sNameL1, content.sFacilitiesName, content.sGroupIndicatorName, content.sYear, content.sMonth, sUrl);
                        string sMessage = string.Format(cContent.sContent, content.sNameL1, content.sFacilitiesName, content.sGroupIndicatorName, content.sYear, content.sMonth, sUrl, (sComment + "").Replace("\n", "<br/>"));


                        DataMail_log Log = new DataMail_log();
                        Log = SystemFunction.SendMailAll(content.sMailL0, content.sMailL1, "", "", sSubject, sMessage, "");
                        Log.nDataID = FormID;
                        SaveLogMail(Log);
                    }
                }
            }
            #endregion
        }

        return r;
    }

    public sysGlobalClass.CResutlWebMethod RequestEdit(int FormID, List<int> lstMonth, int nUserID, string sComment)
    {
        sysGlobalClass.CResutlWebMethod r = new sysGlobalClass.CResutlWebMethod();
        int nStatusTo = 2;
        int nStatusFrom = 0;
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        List<Data_Check> LstDataCheck = new List<Data_Check>();
        var glstData = env.TEPI_Workflow.Where(w => w.FormID == FormID && lstMonth.Contains(w.nMonth)).ToList();
        if (glstData.Any())
        {
            LstDataCheck = glstData.Select(s => new Data_Check
            {
                sMonth = ConvertMonthToString(s.nMonth),
                nMonth = s.nMonth,
                IsPass = s.nStatusID == 4,
                nReportID = s.nReportID
            }).ToList();
        }

        if (LstDataCheck.Any(a => !a.IsPass))
        {
            var gFail = LstDataCheck.Where(w => !w.IsPass).ToList();
            string sMessage = "{0} {1}<br />";
            string sResult = "Cannot Approve <br />";
            int i = 1;
            gFail.ForEach(f =>
            {
                sResult += string.Format(sMessage, i + ".", f.sMonth);
                i++;
            });
            r.Msg = sResult;
            r.Status = SystemFunction.process_Failed;
        }
        else
        {
            LstDataCheck.ForEach(f =>
            {
                var gData = env.TEPI_Workflow.FirstOrDefault(w => w.nReportID == f.nReportID);
                if (gData != null)
                {
                    nStatusFrom = gData.nStatusID.Value;
                    gData.nStatusID = nStatusTo;
                    gData.dAction = DateTime.Now;
                    gData.nActionBy = nUserID;
                    gData.nHistoryStatusID = nStatusTo;
                    env.SaveChanges();

                    SaveLog(gData.nReportID, nStatusTo, nStatusFrom, sComment);
                }
            });
            r.Msg = "Save Success";
            r.Status = SystemFunction.process_Success;
            #region Mail
            if (r.Status == SystemFunction.process_Success)
            {
                DataResultMail cSubject = MailTemPlete_Subject("RQ");
                DataResultMail cContent = new DataResultMail();
                if (cSubject.IsPass)
                {
                    cContent = MailTemPlete_Content("RQ");
                    if (cContent.IsPass)
                    {
                        Data_Email content = new Data_Email();
                        content = GetAllContentMail(FormID, lstMonth);
                        // sUrl += "epi_mytask.aspx";
                        sUrl += "AD/loginAD.aspx";
                        string sSubject = string.Format(cSubject.sContent, content.sGroupIndicatorName, content.sMonth, content.sYear);
                        string sMessage = string.Format(cContent.sContent, content.sNameL2, content.sFacilitiesName, content.sGroupIndicatorName, content.sYear, content.sMonth, sUrl, (sComment + "").Replace("\n", "<br/>"));

                        DataMail_log Log = new DataMail_log();
                        Log = SystemFunction.SendMailAll(content.sMailL0, content.sMailL2, "", "", sSubject, sMessage, "");
                        Log.nDataID = FormID;
                        SaveLogMail(Log);
                    }
                }
            }
            #endregion
        }

        return r;
    }

    public sysGlobalClass.CResutlWebMethod AcceptRequestEdit(int FormID, List<int> lstMonth, int nUserID, string sComment)
    {
        sysGlobalClass.CResutlWebMethod r = new sysGlobalClass.CResutlWebMethod();
        int nStatusTo = 18;
        int nStatusFrom = 0;
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        List<Data_Check> LstDataCheck = new List<Data_Check>();
        var glstData = env.TEPI_Workflow.Where(w => w.FormID == FormID && lstMonth.Contains(w.nMonth)).ToList();
        if (glstData.Any())
        {
            LstDataCheck = glstData.Select(s => new Data_Check
            {
                sMonth = ConvertMonthToString(s.nMonth),
                nMonth = s.nMonth,
                IsPass = s.nStatusID == 2,
                nReportID = s.nReportID
            }).ToList();
        }

        if (LstDataCheck.Any(a => !a.IsPass))
        {
            var gFail = LstDataCheck.Where(w => !w.IsPass).ToList();
            string sMessage = "{0} {1}<br />";
            string sResult = "Cannot Approve <br />";
            int i = 1;
            gFail.ForEach(f =>
            {
                sResult += string.Format(sMessage, i + ".", f.sMonth);
                i++;
            });
            r.Msg = sResult;
            r.Status = SystemFunction.process_Failed;
        }
        else
        {
            LstDataCheck.ForEach(f =>
            {
                var gData = env.TEPI_Workflow.FirstOrDefault(w => w.nReportID == f.nReportID);
                if (gData != null)
                {
                    nStatusFrom = gData.nStatusID.Value;
                    gData.nStatusID = 0;
                    gData.dAction = DateTime.Now;
                    gData.nActionBy = nUserID;
                    gData.nHistoryStatusID = nStatusTo;
                    env.SaveChanges();

                    SaveLog(gData.nReportID, nStatusTo, nStatusFrom, sComment);
                }
            });
            r.Msg = "Save Success";
            r.Status = SystemFunction.process_Success;

            #region Mail
            if (r.Status == SystemFunction.process_Success)
            {
                DataResultMail cSubject = MailTemPlete_Subject("ACE");
                DataResultMail cContent = new DataResultMail();
                if (cSubject.IsPass)
                {
                    cContent = MailTemPlete_Content("ACE");
                    if (cContent.IsPass)
                    {
                        Data_Email content = new Data_Email();
                        content = GetAllContentMail(FormID, lstMonth);
                        // sUrl += "epi_mytask.aspx";
                        sUrl += "AD/loginAD.aspx";
                        string sSubject = string.Format(cSubject.sContent, content.sGroupIndicatorName, content.sMonth, content.sYear);
                        string sMessage = string.Format(cContent.sContent, "All", content.sFacilitiesName, content.sGroupIndicatorName, content.sYear, content.sMonth, sUrl);

                        DataMail_log Log = new DataMail_log();
                        Log = SystemFunction.SendMailAll(content.sMailL2, content.sMailL0, content.sMailL1, "", sSubject, sMessage, "");
                        Log.nDataID = FormID;
                        SaveLogMail(Log);
                    }
                }
            }
            #endregion
        }

        return r;
    }

    public void SaveLog(int nReportID, int nStatusTO, int nStatusFrom, string sRemark)
    {
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        int nUserID = UserAcc.GetObjUser().nUserID;
        TEPI_Workflow_History s = new TEPI_Workflow_History();
        s.dUpdate = DateTime.Now;
        s.nActionBy = nUserID;
        s.nFromStatusID = nStatusFrom;
        s.nToStatusID = nStatusTO;
        s.sRemark = sRemark;
        s.nReportID = nReportID;
        env.TEPI_Workflow_History.Add(s);
        env.SaveChanges();

    }

    public string ConvertMonthToString(int nMonth)
    {
        string sResult = "";
        switch (nMonth)
        {
            case 1:
                sResult = "Jan.";
                break;
            case 2:
                sResult = "Feb.";
                break;
            case 3:
                sResult = "Mar.";
                break;
            case 4:
                sResult = "Apr.";
                break;
            case 5:
                sResult = "May";
                break;
            case 6:
                sResult = "Jun.";
                break;
            case 7:
                sResult = "Jul.";
                break;
            case 8:
                sResult = "Aug.";
                break;
            case 9:
                sResult = "Sep.";
                break;
            case 10:
                sResult = "Oct.";
                break;
            case 11:
                sResult = "Nov.";
                break;
            case 12:
                sResult = "Dec.";
                break;
            default:
                sResult = "Dec";
                break;

        }
        return sResult;
    }

    public void SendEmailToL0onL2EditData(string sYear, int nIndID, int nFacilityID, int nOperationtypeID)
    {
        var dataEPIFrom = db.TEPI_Forms.FirstOrDefault(w => w.sYear == sYear && w.IDIndicator == nIndID && w.FacilityID == nFacilityID && w.OperationTypeID == nOperationtypeID);
        if (dataEPIFrom != null)
        {
            var dataL0 = (from u in db.mTUser.Where(w => w.cDel == "N" && w.cActive == "Y")
                          from ur in db.mTUserInRole.Where(w => w.nUID == u.ID && w.nRoleID == 2)//Operational (L0) Only
                          from up in db.mTUser_FacilityPermission.Where(w => w.nUserID == ur.nUID && w.nRoleID == ur.nRoleID && w.nGroupIndicatorID == nIndID && w.nFacilityID == nFacilityID && w.nPermission != 0)
                          group new { u } by new { u.ID, u.Firstname, u.Lastname, u.Email } into grp
                          select new
                          {
                              grp.Key.ID,
                              grp.Key.Firstname,
                              grp.Key.Lastname,
                              grp.Key.Email
                          }).ToList();
            var dataFac = db.mTFacility.FirstOrDefault(w => w.cDel == "N" && w.ID == nFacilityID);
            var dataInd = db.mTIndicator.FirstOrDefault(w => w.ID == nIndID);
            if (dataFac != null && dataInd != null)
            {
                string strEmailTo = String.Join(",", dataL0.GroupBy(g => g.Email).Select(s => s.Key).ToList());
                sUrl += "AD/loginAD.aspx";

                string sContent = "Dear {0} <br />" +
                        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Environmental Report has been modified data by ENVI Corporate(L2) for your review as detailed below.<br />" +
                        "Facility : {1}<br />" +
                        "Group Indicator : {2}<br />" +
                        "Year : {3}<br />" +
                        "Click <a href='{4}' target='_blank'>link</a> to view for further action.";
                string sSubject = string.Format("ENVI Corporate(L2) modified data of {0} of {1}", dataInd.Indicator, sYear); // 0 = Group Indicator,1=Year
                string sMessage = string.Format(sContent, "All", dataFac.Name, dataInd.Indicator, sYear, sUrl);

                DataMail_log Log = new DataMail_log();
                Log = SystemFunction.SendMailAll("", strEmailTo, "", "", sSubject, sMessage, "");
                Log.nDataID = dataEPIFrom.FormID;
                SaveLogMail(Log);
            }
        }
    }

    [Serializable]
    public class Data_Email
    {
        public string sFacilitiesName { get; set; }
        public string sGroupIndicatorName { get; set; }
        public string sYear { get; set; }
        public string sMonth { get; set; }
        public string sMailL0 { get; set; }
        public string sMailL1 { get; set; }
        public string sMailL2 { get; set; }
        public string sNameL0 { get; set; }
        public string sNameL1 { get; set; }
        public string sNameL2 { get; set; }
    }

    public Data_Email GetAllContentMail(int FormID, List<int> lstMonth)
    {
        Data_Email r = new Data_Email();
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        var gDataForm = env.TEPI_Forms.FirstOrDefault(f => f.FormID == FormID);
        var gFacilities = env.mTFacility.Where(w => w.cDel == "N" && w.nLevel == 2).ToList();
        GetDataUser DataL0 = new GetDataUser();
        GetDataUser DataL1 = new GetDataUser();
        GetDataUser DataL2 = new GetDataUser();
        string sMonth = "";
        if (lstMonth.Any())
        {
            int lenght = lstMonth.Count();
            int nRound = 1;
            lstMonth.ForEach(f =>
            {
                if (lenght == nRound)
                {
                    sMonth += ConvertMonthToString(f);
                }
                else
                {
                    sMonth += ConvertMonthToString(f) + ",";
                }
                nRound++;
            });
        }
        if (gDataForm != null)
        {
            r.sFacilitiesName = gFacilities.Any(a => a.ID == gDataForm.FacilityID) ? gFacilities.First(f => f.ID == gDataForm.FacilityID).Name : "";
            r.sGroupIndicatorName = env.mTIndicator.First(f => f.ID == gDataForm.IDIndicator).Indicator;
            r.sYear = gDataForm.sYear;
            r.sMonth = sMonth;
            DataL0 = GetData(gDataForm.sAddBy.Value, "L0", gDataForm.FacilityID.Value, gDataForm.IDIndicator);
            DataL1 = GetData(0, "L1", gDataForm.FacilityID.Value, gDataForm.IDIndicator);
            DataL2 = GetData(0, "L2", gDataForm.FacilityID.Value, gDataForm.IDIndicator);

            r.sNameL0 = DataL0.sName;
            r.sMailL0 = DataL0.sMail;
            r.sNameL1 = DataL1.sName;
            r.sMailL1 = DataL1.sMail;
            r.sNameL2 = DataL2.sName;
            r.sMailL2 = DataL2.sMail;
        }
        return r;
    }

    [Serializable]
    public class GetDataUser
    {
        public string sMail { get; set; }
        public string sName { get; set; }
    }

    public GetDataUser GetData(int nUserID, string sMode, int nFacilities, int nGroupIndicator)
    {
        GetDataUser r = new GetDataUser();
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        r.sMail = "";
        r.sName = "";

        var gDataUser = env.mTWorkFlow.FirstOrDefault(f => f.IDFac == nFacilities && f.IDIndicator == nGroupIndicator);
        if (sMode == "L0")
        {
            var gL0 = (from uf in env.mTUser_FacilityPermission.Where(w => w.nFacilityID == nFacilities && w.nGroupIndicatorID == nGroupIndicator && w.nRoleID == 2 && w.nPermission == 2)
                       from u in env.mTUser.Where(w => w.cActive == "Y" && w.cDel == "N" && w.ID == uf.nUserID)
                       group new { u } by new { u.ID, u.Firstname, u.Lastname, u.Email } into grp
                       select new
                       {
                           grp.Key.ID,
                           grp.Key.Firstname,
                           grp.Key.Lastname,
                           grp.Key.Email,
                       }).ToList();
            gL0.ForEach(f =>
            {
                r.sName += f.Firstname + " " + f.Lastname;
                r.sMail += f.Email + ",";
            });

        }
        else if (sMode == "L1")
        {
            if (gDataUser != null)
            {
                r.sName = env.mTUser.First(f => f.ID == gDataUser.L1).Firstname + " " + env.mTUser.First(f => f.ID == gDataUser.L1).Lastname;
                r.sMail = env.mTUser.First(f => f.ID == gDataUser.L1).Email;
            }
        }
        else if (sMode == "L2")
        {
            if (gDataUser != null)
            {
                r.sName = env.mTUser.First(f => f.ID == gDataUser.L2).Firstname + " " + env.mTUser.First(f => f.ID == gDataUser.L2).Lastname;
                r.sMail = env.mTUser.First(f => f.ID == gDataUser.L2).Email;
            }
        }
        return r;
    }

    [Serializable]
    public class DataMail_log
    {

        public string sFrom { get; set; }
        public string sTo { get; set; }
        public string sCC { get; set; }
        public string sContent { get; set; }
        public int nDataID { get; set; }
        public bool bStatus { get; set; }
        public string sMessage { get; set; }
        public string sPageName { get; set; }

    }

    public void SaveLogMail(DataMail_log objLog)
    {
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();
        T_Logmail s = new T_Logmail();
        s.bStatus = objLog.bStatus;
        s.dSend = DateTime.Now;
        s.nDataID = objLog.nDataID;
        s.sCC = objLog.sCC;
        s.sContent = objLog.sContent;
        s.sFrom = objLog.sFrom;
        s.sMessage = objLog.sMessage;
        s.sPagename = objLog.sPageName;
        s.sTo = objLog.sTo;
        env.T_Logmail.Add(s);
        env.SaveChanges();
    }

    public void UpdateHistoryStatus(int FormID)
    {
        PTTGC_EPIEntities env = new PTTGC_EPIEntities();

        var gData = env.TEPI_Workflow.Where(w => w.FormID == FormID);
        foreach (var item in gData)
        {
            item.nHistoryStatusID = item.nStatusID;
        }

        env.SaveChanges();
    }
}