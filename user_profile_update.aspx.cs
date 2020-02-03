using sysExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class user_profile_update : System.Web.UI.Page
{
    PTTGC_EPIEntities db = new PTTGC_EPIEntities();
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
                UserAcc ua = UserAcc.GetObjUser();
                hidUserID.Value = ua.nUserID + "";
                hidEncryptUserID.Value = STCrypt.Encrypt(hidUserID.Value);
                SetData(hidUserID.Value.toIntNullToZero());
                //txtOldPass.Attributes.Add("type", "password");
                //string strID = Request.QueryString["strid"];
                //if (!string.IsNullOrEmpty(strID))
                //{
                //    hidUserID.Value = STCrypt.Decrypt(strID);
                //    SetData(hidUserID.Value.toIntNullToZero());
                //    hidEncryptUserID.Value = STCrypt.Encrypt(hidUserID.Value);
                //}
            }
        }
    }

    private void SetData(int nUserID)
    {
        var query = db.mTUser.FirstOrDefault(w => w.ID == nUserID);
        if (query != null)
        {
            if (query.cUserType == "0")
            {
                txtNameGc.Text = query.Firstname;
                txtNameGc.Attributes.Add("disabled", "true");
                txtOrgGc.Text = query.Company;
                txtOrgGc.Attributes.Add("disabled", "true");
                txtEmailGc.Text = query.Email;
                txtEmailGc.Attributes.Add("disabled", "true");
                DivContract.Style.Add("display", "none");
                hdfEmployeeID.Value = query.Username;
                hdfEmployeeName.Value = query.Firstname;
            }
            else
            {
                DivGc.Style.Add("display", "none");
            }
            txtName.Text = query.Firstname;
            txtSurname.Text = query.Lastname;
            txtOrg.Text = query.Company;
            txtEmail.Text = query.Email;
            txtUsername.Text = query.Username;
            //txtOldPass.Text = STCrypt.Decrypt(query.PasswordEncrypt);
            //txtPassword.Text = query.PasswordEncrypt == null ? "" : STCrypt.Decrypt(query.PasswordEncrypt);
            //txtPassword.ReadOnly = true;
            //rblUsertype.Attributes.Add("disabled", "true");
            //rblStatus.SelectedValue = query.cActive;
            rblUsertype.SelectedValue = query.cUserType;

            //rblUsertype.Enabled = false;
            //txtCompanyName.Text = query.Name;
            //txtDesc.Text = query.Description;
            //rblStatus.SelectedValue = query.cActive;
            //if (query.cActive == "N")
            //{
            //    txtRemark.Text = query.sRemark;
            //    txtRemark.Enabled = true;
            //}
        }
    }

    #region SaveData
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(CSaveData data)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        if (UserAcc.UserExpired())
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            Func<string, int?, bool> CheckDuplicateName = (name, id) =>
            {
                bool Isdup = false;
                var q = db.mTUser.Where(w => (id.HasValue ? w.ID != id : true) && w.Username == name);
                Isdup = q.Any();
                return Isdup;
            };

            if (!string.IsNullOrEmpty(data.sUserID))
            {
                int nUserID = data.sUserID.toIntNullToZero();

                var query = db.mTUser.FirstOrDefault(w => w.ID == nUserID);
                if (query != null)
                {
                    if (query.cUserType == "0") //Employee_GC
                    {
                        if (!CheckDuplicateName(data.sUserCode, nUserID))
                        {
                            query.Firstname = data.sName.Trim();
                            query.Email = data.sEmail.Trim();
                            query.Company = data.sOrg.Trim();
                            //query.sUserCode = data.sUserCode.Trim();
                            //query.Username = data.sUserCode.Trim();
                        }
                        else
                        {
                            result.Msg = "Duplicate Employee Code !";
                            result.Status = SystemFunction.process_Failed;
                            return result;
                        }

                    }
                    else //Employee_Other
                    {
                        if (!CheckDuplicateName(data.sUsername, nUserID))
                        {
                            query.Username = data.sUsername.Trim();
                            query.Email = data.sEmail.Trim();
                            query.Company = data.sOrg.Trim();
                            query.Firstname = data.sName.Trim();
                            query.Lastname = data.sSurName.Trim();
                            if (!string.IsNullOrEmpty(data.sPassword))
                            {
                                query.Password = STCrypt.encryptMD5(data.sPassword.Trim());
                                query.PasswordEncrypt = STCrypt.Encrypt(data.sPassword.Trim());
                            }
                        }
                        else
                        {
                            result.Msg = "Duplicate Username !";
                            result.Status = SystemFunction.process_Failed;
                            return result;
                        }

                    }
                    query.nUpdateID = UserAcc.GetObjUser().nUserID;
                    query.dUpdate = DateTime.Now;

                    db.SaveChanges();
                    result.Status = SystemFunction.process_Success;
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Not found data !";
                return result;
            }
        }

        return result;
    }
    #endregion

    #region Class 
    [Serializable]
    public class CSaveData
    {
        public string sUserID { get; set; }
        public string sName { get; set; }
        public string sNameGc { get; set; }
        public string sUserCode { get; set; }
        public string sOrgGc { get; set; }
        public string sEmailGc { get; set; }
        public string sSurName { get; set; }
        public string sOrg { get; set; }
        public string sEmail { get; set; }
        public string sUsername { get; set; }
        public string sPassword { get; set; }
        public string sStatus { get; set; }
        public string sUserType { get; set; }
        //public List<T_RoleUser> lstData_Role { get; set; }
    }
    #endregion
}