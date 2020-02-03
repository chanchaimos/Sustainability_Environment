using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class f_ContactUs : System.Web.UI.Page
{
    private const string sFolderInSharePahtTemp = "UploadFiles/ContactUs/Temp/";
    private const string sFolderInPathSave = "UploadFiles/ContactUs/File/{0}/";
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnLoad(myFunc);
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
                PTTGC_EPIEntities db = new PTTGC_EPIEntities();
                UserAcc ua = UserAcc.GetObjUser();
                var get_user = db.mTUser.FirstOrDefault(w => w.ID == ua.nUserID);
                if (get_user != null)
                {
                    txtName.Text = get_user.Firstname + "  " + get_user.Lastname;
                    txtEmail.Text = get_user.Email;
                    txtName.Enabled = false;
                    txtEmail.Enabled = false;
                }
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveToDB(CSave_Data item, List<dataFileContactUs> objFile)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        DateTime now = DateTime.Now;
        if (UserAcc.UserExpired())
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            UserAcc ua = UserAcc.GetObjUser();
            try
            {
                string sPathSend = "";
                int nID = db.TContactUs.Any() ? db.TContactUs.Max(m => m.nContactID) + 1 : 1;
                TContactUs t = new TContactUs();
                t.nContactID = nID;
                t.sContactName = item.sName;
                t.sContactEmail = item.sEmail;
                t.sContactTel = item.sTel;
                t.sSubject = item.sSubject;
                t.sDetail = item.sDec;
                t.dCreate = now;
                t.dUpdate = now;
                t.cDel = "N";
                if (objFile.Count > 0 && objFile != null)
                {
                    string sPathSave = string.Format(sFolderInPathSave, nID);
                    SystemFunction.CreateDirectory(sPathSave);
                    //string sPathSave = string.Format(sFolderInPathSave, nID);
                    //SystemFunction.CreateDirectory(sPathSave);
                    var f = objFile.First();
                    string sSystemFileName = nID + "_" + "-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "." + SystemFunction.GetFileNameFromFileupload(f.sFileName, ""); //SystemFunction2.GetFileType(item.SaveToFileName);
                    SystemFunction.UpFile2Server(f.sPath, sPathSave, f.sSysFileName, sSystemFileName);

                    t.sContactFile = f.sFileName;
                    t.sContactSysFile = sSystemFileName;
                    t.sContactPath = sPathSave;
                    sPathSend = HttpContext.Current.Server.MapPath("./") + sPathSave + sSystemFileName;
                }
                //t.cStatusAns = "NULL";
                db.TContactUs.Add(t);
                var Ispass = db.SaveChanges() > 0;

                if (Ispass)
                {
                    string From = "";
                    var lstUser = db.mTUser.FirstOrDefault(w => w.ID == ua.nUserID);
                    if (lstUser != null)
                    {
                        From = lstUser.Email;
                    }

                    // string From = item.sEmail;
                    #region Get_Email_Admin 
                    string AdminContactmail = "";
                    var lst_Email_Admin = db.TMenu_Permission.Where(w => w.nRoleID == 1 && w.nMenuID == 65 && w.nPermission == 2).ToList();
                    if (lst_Email_Admin.Any())
                    {
                        lst_Email_Admin.ForEach(f =>
                        {
                            int nUserID = f.nUserID;
                            var lst_Tuser = db.mTUser.FirstOrDefault(w => w.cActive == "Y" && w.ID == nUserID);
                            if (lst_Tuser != null)
                            {
                                AdminContactmail += "," + lst_Tuser.Email + " ";
                            }
                        });
                        AdminContactmail = AdminContactmail.Remove(0, 1);
                    }
                    else
                    {
                        AdminContactmail = WebConfigurationManager.AppSettings["ContactMail"] + "";
                    }
                    #endregion




                    string To = "chotika.n@softthai.com";
                    string sSubject = SystemFunction.sAbbrSystem + " : Contact Us." + item.sSubject;
                    string sGurl = SystemFunction.RequestUrl() + WebConfigurationManager.AppSettings["DefaultPage"] + "";
                    string sHtml = @"
                    <table align='left'  border='0' cellspacing='2' cellpadding='3'>

                        <tr>
                            <td align='left'>Dear administrator: " + SystemFunction.SystemName + @" </td>
                        </tr>
                         <tr>
                           <td align='left'>Subject: " + item.sSubject + @"</td>
                        </tr>
                         <tr>
                           <td align='left'>Description: " + item.sDec + @"</td>
                        </tr>
                         <tr>
                           <td align='left'>From: " + item.sName + " E-mail: " + item.sEmail + " Contact phone number: " + item.sTel + @"</td>
                        </tr>
                        <tr>
                            <td align='left'><b>Check out more details.</b>: <a href='" + sGurl + @"' target='_blank'>Please click here</a></td>
                        </tr>
                       
                    </table>";
                    //sPathSend != "" ? sPathSend.Replace("/", "\\") : ""
                    Workflow.DataMail_log log = new Workflow.DataMail_log();
                    log = SystemFunction.SendMailAll(From, AdminContactmail, "", "", sSubject, sHtml, sPathSend != "" ? sPathSend.Replace("/", "\\") : "");
                    log.nDataID = SystemFunction.GetIntNullToZero(nID + "");
                    log.sPageName = "f_ContactUs.aspx";
                    new Workflow().SaveLogMail(log);
                    if (log.bStatus)
                    {
                        result.Status = SystemFunction.process_Success;
                    }
                }
            }
            catch (Exception e)
            {
                result.Msg = e.Message;
                result.Status = SystemFunction.process_Failed;
            }

            result.Status = SystemFunction.process_Success;
        }

        return result;
    }

    #region Send Mail
    public static string GET_TemplateEmail()
    {
        return @"<div id=':km' class='ii gt adP adO'>
                <div id=':l9' class='a3s aXjCH m15f05c377e26ea4b'>
                    <u></u>
                    <div style='background: #f9f9f9'>
                        <div style='background-color: #f9f9f9'>

                            <div style='margin: 0px auto; /* max-width: 630px; */background: transparent;'>
                                <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: transparent;' align='center' border='0'>
                                    <tbody>
                                        <tr>
                                            <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; /* padding: 40px 0px */'>
                                                <div style='font-size: 1px; line-height: 12px'>&nbsp;{4}</div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div style='max-width: 640px; margin: 0 auto; border-radius: 4px; overflow: hidden'>
                                <div style='margin: 0px auto; max-width: 640px; background: #ffffff'>
                                    <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: #ffffff' align='center' border='0'>
                                        <tbody>
                                            <tr>
                                                <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; padding: 40px 70px'>
                                                    <div aria-labelledby='mj-column-per-100' class='m_5841562294398106085mj-column-per-100 m_5841562294398106085outlook-group-fix' style='vertical-align: top; display: inline-block; direction: ltr; font-size: 13px; text-align: left; width: 100%'>
                                                        <table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'>
                                                            <tbody>
                                                                <tr>
                                                                    <td style='word-break: break-word; font-size: 0px; padding: 0px' align='left'>
                                                                        <div style='color: #737f8d; font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-size: 16px; line-height: 24px; text-align: left'>

                                                                            <h2 style='font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-weight: 500; font-size: 20px; color: #4f545c; letter-spacing: 0.27px'>{0}</h2>
                                                                            {1}
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style='word-break: break-word; font-size: 0px; padding: 0px' align='left'>
                                                                        <div style='color: #737f8d; font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-size: 16px; line-height: 24px; text-align: left'>
                                                                            {5}
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style='word-break: break-word; font-size: 0px; padding-top: 15px;'>
                                                                        <p style='font-size: 1px; margin: 0px auto; border-top: 1px solid #dcddde; width: 100%'></p>
                                                                    </td>
                                                                </tr>
                                                                <tr style='display:none;'>
                                                                    <td style='word-break: break-word; font-size: 0px; padding: 0px' align='left'>
                                                                        <div style='color: #747f8d; font-family: Whitney,Helvetica Neue,Helvetica,Arial,Lucida Grande,sans-serif; font-size: 13px; line-height: 16px; text-align: left'>
                                                                            <p>
                                                                                {3}
                                                                            </p>
                                                                            <p>
                                                                                Best regards,<br>
                                                                                Technology Management System Team
                                                                            </p>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div style='margin: 0px auto; max-width: 640px; background: transparent'>
                                <table role='presentation' cellpadding='0' cellspacing='0' style='font-size: 0px; width: 100%; background: transparent' align='center' border='0'>
                                    <tbody>
                                        <tr>
                                            <td style='text-align: center; vertical-align: top; direction: ltr; font-size: 0px; padding: 0px'>
                                                <div aria-labelledby='mj-column-per-100' class='m_5841562294398106085mj-column-per-100 m_5841562294398106085outlook-group-fix' style='vertical-align: top; display: inline-block; direction: ltr; font-size: 13px; text-align: left; width: 100%'>
                                                    <table role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'>
                                                        <tbody>
                                                            <tr>
                                                                <td style='word-break: break-word; font-size: 0px'>
                                                                    <div style='font-size: 1px; line-height: 12px'>&nbsp;</div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>";
    }
    #endregion

    #region Class
    [Serializable]
    public class CSave_Data
    {
        public string sName { get; set; }
        public string sEmail { get; set; }
        public string sSubject { get; set; }
        public string sDec { get; set; }
        public string sTel { get; set; }
    }
    [Serializable]
    public class dataFileContactUs
    {
        public int nID { get; set; }
        public string sPath { get; set; }
        public string sSysFileName { get; set; }
        public string sFileName { get; set; }
    }
    #endregion
}