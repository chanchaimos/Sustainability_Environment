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


public partial class bypass : System.Web.UI.Page
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
            var query = db.mTUser.FirstOrDefault(w => w.Username == sUserName && w.cDel == "N" && w.cActive == "Y");
            if (query != null)
            {
                if (sPassword == ConfigurationManager.AppSettings["DefaultPass"].ToString())
                {
                    var qRole = db.mTUserInRole.FirstOrDefault(w => w.nUID == query.ID);
                    if (qRole != null)
                    {
                        var qRoleName = db.mTUserRole.FirstOrDefault(w => w.ID == qRole.nRoleID);
                        ua.nUserID = query.ID;
                        ua.sFullName = query.Firstname + " " + query.Lastname;
                        ua.nRoleID = qRole.nRoleID;
                        ua.sActionRoleName = qRoleName != null ? qRoleName.Name : "";
                        UserAcc.SetObjUser(ua);
                        result.Status = SystemFunction.process_Success;
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Not found role !";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Incorrect pasword !";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Not found user !";
            }
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SelectedRole(string sUserID, string sRoleID)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (new bypass().SetUserLogin(sUserID, sRoleID))
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
            us.nRoleID = lst_RoleAdmin.Any() ? lst_RoleAdmin.First().nRoleID : lst_RoleOther.Any() ? lst_RoleOther.First().nRoleID : 0;
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