using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sysExtension;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Configuration;

public partial class login : System.Web.UI.Page
{
    PTTGC_EPIEntities db = new PTTGC_EPIEntities();
    public HtmlGenericControl SetBody
    {
        get { return this.bodyMain; }
    }
    private void SetBodyEventOnLoad(string myFunc)
    {
        bodyMain.Attributes.Add("onLoad", myFunc);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string str = Request.QueryString["strad"];
        string sm = Request.QueryString["smod"];
        string sAD = "";
        if (!string.IsNullOrEmpty(sm))
        {
            if (!string.IsNullOrEmpty(str))
            {
                hdfUserAD.Value = STCrypt.Decrypt(str);
            }
        }


    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static CResultLogin Login(string sUserName, string sPassword, string sMode)
    {
        CResultLogin result = new CResultLogin();
        UserAcc ua = new UserAcc();
        if (!string.IsNullOrEmpty(sUserName))
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            sUserName = sUserName.Trims();
            sPassword = sPassword.Trims();
            int nUserIDSuperAdmin = SystemFunction.ParseInt(ConfigurationManager.AppSettings["UserIDAdmin"].ToString());
            var query = db.mTUser.FirstOrDefault(w => w.Username == sUserName && w.cDel == "N" && w.cActive == "Y");
            if (query != null && query.ID == nUserIDSuperAdmin)
            {
                if (sPassword == ConfigurationManager.AppSettings["SupperAdminPWD"].ToString())
                {
                    ua.nUserID = query.ID;
                    ua.sFullName = query.Firstname + " " + query.Lastname;
                    ua.nRoleID = 1;
                    ua.sActionRoleName = "System Admin";
                    UserAcc.SetObjUser(ua);
                    result.Status = SystemFunction.process_Success;
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Incorrect password !";
                }
            }
            else
            {
                var resultLogin = UserAcc.Login(sUserName, sPassword, sMode);
                result.Msg = resultLogin.Msg;
                result.nUserID = resultLogin.nUserID;
                result.Status = resultLogin.Status;
                result.TDataRole = resultLogin.TDataRole;
            }
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SelectedRole(string sUserID, string sRoleID)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (new login().SetUserLogin(sUserID, sRoleID))
        {
            result.Status = SystemFunction.process_Success;
            result.Content = "epi_mytask.aspx";
        }
        else
        {
            result.Status = SystemFunction.process_Failed;
            result.Msg = "data not found";

        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod ForgetPassword(string sEmail, string sUsername)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        sEmail = sEmail.ToLower();
        sUsername = sUsername.ToLower();

        var lstUser = db.mTUser.Where(w => w.cActive == "Y" && w.cDel == "N" && w.Username.ToLower() == sUsername && w.Email.ToLower() == sEmail).ToList();
        if (lstUser.Any())
        {
            var Data = lstUser.First();

            string sTitle = "";
            string sText = "";
            string subject = "";
            string message = "";
            string sURL = "";
            string sFoot = "";// "Should you have any questions about RD&T work process.";

            // sURL = Applicationpath + "login_forget.aspx?str=" + SystemFunction.Encrypt_UrlEncrypt(nDocID + "");

            subject = "Password Confirmation | " + SystemFunction.SystemName + "";

            sText += "<p>Your password is " + STCrypt.Decrypt(Data.PasswordEncrypt) + "</p>";

            string From = SystemFunction.GetSystemMail;
            string To = Data.Email;
            message = string.Format(GET_TemplateEmail(),
            "Dear " + Data.Firstname + ' ' + Data.Lastname,
            sText,
            sURL,
            sFoot,
            "",
            "");
            Workflow.DataMail_log log = new Workflow.DataMail_log();
            log = SystemFunction.SendMailAll(From, To, "", "", subject, message, "");
            log.nDataID = SystemFunction.GetIntNullToZero(Data.ID + "");
            log.sPageName = "login.aspx";
            new Workflow().SaveLogMail(log);
            if (log.bStatus)
            {
                result.Status = SystemFunction.process_Success;
            }
        }
        else
        {
            result.Status = SystemFunction.process_Failed;
            result.Msg = "data not found user";
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


    private bool SetUserLogin(string sUserID, string sRoleID)
    {
        int nUserID = SystemFunction.ParseInt(sUserID);
        int nRoleID = SystemFunction.ParseInt(sRoleID);

        var dataUser = db.mTUser.FirstOrDefault(w => w.ID == nUserID);
        var dataRole = db.mTUserRole.FirstOrDefault(w => w.ID == nRoleID);

        var lst_RoleAdmin = db.TMenu_Permission.Where(w => w.nUserID == nUserID && w.nRoleID == nRoleID).ToList(); // Role_Admin
        var lst_RoleOther = db.mTUser_FacilityPermission.Where(w => w.nUserID == nUserID && w.nRoleID == nRoleID).ToList(); // Role_Other
        var lstDataRole = db.mTUserRole.ToList();// Query Role_Name

        if (dataUser != null && dataRole != null)
        {
            string sNameRole = lstDataRole.Any() ? lstDataRole.First(w => w.ID == nRoleID).Name : "";
            UserAcc us = new UserAcc();
            us.nUserID = dataUser.ID;
            us.sFullName = dataUser.Firstname + " " + dataUser.Lastname;
            us.nRoleID = nRoleID != 0 ? nRoleID : lst_RoleAdmin.Any() ? lst_RoleAdmin.First().nRoleID : lst_RoleOther.Any() ? lst_RoleOther.First().nRoleID : 0;
            us.sActionRoleName = sNameRole;
            UserAcc.SetObjUser(us);
            return true;
        }
        else
        {
            return false;
        }
    }

    [Serializable]
    public class CResultLogin : sysGlobalClass.CResutlWebMethod
    {
        public int nUserID { get; set; }
        public string sFullName { get; set; }
        public string sRoleName { get; set; }
        public List<UserAcc.TDataRole> TDataRole { get; set; }
    }
    [Serializable]
    public class TDataRole
    {
        public int nRoleID { get; set; }
        public string sRoleName { get; set; }
    }
}