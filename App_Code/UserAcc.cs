using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for UserAcc
/// </summary>
public class UserAcc
{
    public int nUserID { get; set; }
    public string sFullName { get; set; }
    public int nRoleID { get; set; }
    public string sActionRoleName { get; set; }

    public UserAcc()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static UserAcc GetObjUser()
    {
        UserAcc user = new UserAcc();
        if (HttpContext.Current.Session["MSUserLogin"] != null)
        {
            user = HttpContext.Current.Session["MSUserLogin"] as UserAcc;
        }
        return user;
    }

    public static void SetObjUser(UserAcc user)
    {
        HttpContext.Current.Session["MSUserLogin"] = user;
    }

    public static bool UserExpired()
    {
        if (HttpContext.Current.Session["MSUserLogin"] == null)
            return true;
        else
            return false;
    }

    public static CResultLogin Login(string sUserName, string sPassword, string sMode)
    {
        CResultLogin result = new CResultLogin();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<TDataRole> lstData = new List<TDataRole>();
        if (!string.IsNullOrEmpty(sUserName) && (string.IsNullOrEmpty(sMode) ? !string.IsNullOrEmpty(sPassword) : true))
        {
            string sPassEncypt = STCrypt.Encrypt(sPassword);
            var User = db.mTUser.Where(w => w.Username == sUserName && (string.IsNullOrEmpty(sMode) ? w.PasswordEncrypt == sPassEncypt : true) && w.cDel == "N" && w.cActive == "Y").ToList();
            if (User.Any())
            {
                int nUserID = User.Any() ? User.First().ID : 0;
                int nRoleID = 0;
                var lst_Role = db.mTUserInRole.Where(w => w.nUID == nUserID).Select(s => s.nRoleID).Distinct().ToList();
                var lstDataRole = db.mTUserRole.ToList();
                if (lst_Role.Count > 1)
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "muti";
                    result.nUserID = nUserID;
                    lstData = GetRolePermission(nUserID + "");
                    result.TDataRole = lstData;
                    return result;
                }
                else
                {
                    if (lst_Role.Any())
                    {
                        nRoleID = lst_Role.First();
                    }

                    string sNameRole = lstDataRole.Any() ? lstDataRole.First(w => w.ID == nRoleID).Name : "";

                    UserAcc ua = (new UserAcc
                    {
                        nUserID = nUserID,
                        nRoleID = nRoleID,
                        sFullName = User.First().Firstname + ' ' + User.First().Lastname,
                        sActionRoleName = sNameRole,
                    });
                    UserAcc.SetObjUser(ua);
                    result.Status = SystemFunction.process_Success;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(sMode))
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "User account not Register.";
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "User account and password incorrect.";
                }
            }
        }
        return result;
    }

    public static List<TDataRole> GetRolePermission(string sUserID)
    {
        List<TDataRole> lstData = new List<TDataRole>();
        DataTable dt = new DataTable();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int nUserID = 0;
        if (!string.IsNullOrEmpty(sUserID))
        {
            nUserID = int.Parse(sUserID);
        }
        var TBUser_InRow = db.mTUserInRole.Where(w => w.nUID == nUserID).ToList();
        var lstDataRole = db.mTUserRole.ToList();
        if (TBUser_InRow.Any())
        {
            TBUser_InRow.ForEach(f =>
            {
                lstData.Add(new TDataRole
                {
                    nRoleID = f.nRoleID,
                    sRoleName = lstDataRole.Any(a => a.ID == f.nRoleID) ? lstDataRole.First(a => a.ID == f.nRoleID).Name : "",
                });
            });
        }
        lstData = lstData.Distinct().ToList();
        return lstData;
    }

    public static sysGlobalClass.CResutlWebMethod RoleSelected(string sRoleID)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        //if (!UserAcc.UserExpired())
        //{
        //    PTTPIMSAssessmentEntities db = new PTTPIMSAssessmentEntities();
        //    int nRoleID = SystemFunction2.ParseInt(sRoleID);
        //    var query = db.TGroupUser.FirstOrDefault(w => w.nGroupID == nRoleID && w.cActive == "1");
        //    if (query != null)
        //    {
        //        UserAcc.GetObjUser().nRoleID = query.nGroupID;
        //        UserAcc.GetObjUser().sACTIONROLE = query.sGroupname;

        //        HttpContext.Current.Session["ROLE"] = query.nGroupID;
        //        HttpContext.Current.Session["ACTIONROLE"] = query.sGroupname;

        //        result.Status = SystemFunction2.process_Success;
        //        result.Content = SystemFunction2.GetPageDefault();
        //    }
        //    else
        //    {
        //        result.Status = SystemFunction2.process_Failed;
        //        result.Msg = "ไม่พบ Role !";
        //    }
        //}
        //else
        //{
        //    result.Status = SystemFunction2.process_SessionExpired;
        //}
        return result;
    }

    public class CResultLogin : sysGlobalClass.CResutlWebMethod
    {
        public int nUserID { get; set; }
        public string sFullName { get; set; }
        public string sRoleName { get; set; }
        public List<TDataRole> TDataRole { get; set; }
    }

    public class TDataRole
    {
        public int nRoleID { get; set; }
        public string sRoleName { get; set; }
    }
}