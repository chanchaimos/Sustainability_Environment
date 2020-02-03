<%@ WebHandler Language="C#" Class="SystemFunctions" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;

public class SystemFunctions : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        string sFuncName = context.Request["funcName"] + "";
        string sPara1 = context.Request["param1"] + "";
        string sPara2 = context.Request["param2"] + "";
        string sPara3 = context.Request["param3"] + "";
        if (sFuncName == "encrypt")//for web server in code behind
        {
            string sval = HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(sPara1 + ""));
            context.Response.Expires = -1;
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write(sval);
            context.Response.End();
        }
        else if (sFuncName == "encrypt_decodejava")//endcode use on javascript decode
        {
            string sval = STCrypt.Encrypt(sPara1 + "");
            context.Response.Expires = -1;
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write(sval);
            context.Response.End();
        }
        else if (sFuncName == "decrypt")//decode from encode by function encrypt_decodejava
        {
            string sval = STCrypt.Decrypt(sPara1);
            context.Response.Expires = -1;
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write(sval);
            context.Response.End();
        }
        else if (sFuncName == "changerole")
        {
            context.Response.Expires = -1;
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write(new SystemFunction().Ob2Json(GetRoleDetail()));
            context.Response.End();
        }
        else if (sFuncName == "selectedrole")
        {
            context.Response.Expires = -1;
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write(new SystemFunction().Ob2Json(RoleSeleted(sPara1)));
            context.Response.End();
        }
        else if (sFuncName == "urlsite")
        {
            context.Response.Expires = -1;
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write(SystemFunction.GetDefaultPage);
            context.Response.End();
        }
        else if (sFuncName == "addlogerror")
        {
            new SystemFunction().addLogError(sPara1, sPara2);
            context.Response.Expires = -1;
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write("");
            context.Response.End();
        }
        else
        {
            context.Response.Expires = -1;
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write("");
            context.Response.End();
        }
    }

    public TRestulRole GetRoleDetail()
    {
        TRestulRole result = new TRestulRole();
        if (!UserAcc.UserExpired())
        {
            List<TDataRole> lstData = new List<TDataRole>();
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            UserAcc ua = UserAcc.GetObjUser();

            int nUserID = ua.nUserID;
            // var lst_RoleAdmin = db.TMenu_Permission.Where(w => w.nUserID == nUserID).ToList();
            var lstRoleAdmin = db.TMenu_Permission.Where(w => w.nUserID == nUserID).Select(s => s.nRoleID).Distinct().ToList();
            var lst_RoleOther = db.mTUser_FacilityPermission.Where(w => w.nUserID == nUserID && w.nRoleID != 1).Select(s => s.nRoleID).Distinct().ToList();
            //var lst_RoleOther = db.mTUser_FacilityPermission.Where(w => w.nUserID == nUserID).ToList();
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


            //if (lstRoleAdmin.Any())
            //{
            //    foreach (var item in lstRoleAdmin)
            //    {
            //        int nRole = int.Parse(item + "");
            //        lstData.Add(new TDataRole
            //        {
            //            nRoleID = nRole,
            //            sRoleName = lstDataRole.Any(a => a.ID == item) ? lstDataRole.First(a => a.ID == item).Name : "",
            //        });
            //    }
            //}

            //if (lst_RoleOther.Any())
            //{
            //    foreach (var item in lst_RoleOther)
            //    {
            //        int nRole = int.Parse(item + "");
            //        lstData.Add(new TDataRole
            //        {
            //            nRoleID = nRole,
            //            sRoleName = lstDataRole.Any(a => a.ID == item) ? lstDataRole.First(a => a.ID == item).Name : "",
            //        });
            //    }
            //}
            //dt = SystemFunction
            //dt = SystemFunction2.GetPrmsRole(sUserID);
            //lstData = dt.AsEnumerable().Select(s => new TDataRole
            //    {
            //        nRoleID = s.Field<int>("nGroupID"),
            //        sRoleName = s.Field<string>("sGroupname")
            //    }).ToList();
            lstData = lstData.Distinct().ToList();
            result.lstData = lstData;
            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    public sysGlobalClass.CResutlWebMethod RoleSeleted(string sRoleID)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            int nRoleID = SystemFunction.GetIntNullToZero(sRoleID);
            var query = db.mTUserRole.FirstOrDefault(w => w.cActive == "Y" && w.ID == nRoleID);
            if (query != null)
            {
                UserAcc.GetObjUser().nRoleID = nRoleID;
                UserAcc.GetObjUser().sActionRoleName = query.Name;
                result.Content = "epi_mytask.aspx";
                result.Status = SystemFunction.process_Success;
            }
            else
            {
                result.Msg = "No data the role";
                result.Status = SystemFunction.process_Failed;
            }
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    #region class
    [Serializable]
    public class TDataRole
    {
        public int nRoleID { get; set; }
        public string sRoleName { get; set; }
    }

    [Serializable]
    public class TRestulRole : sysGlobalClass.CResutlWebMethod
    {
        public List<TDataRole> lstData { get; set; }
    }
    #endregion

}