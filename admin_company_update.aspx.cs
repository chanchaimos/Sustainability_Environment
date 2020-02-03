using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Web.Services;
using sysExtension;

public partial class admin_company_update : System.Web.UI.Page
{
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnLoad(myFunc);
    }

    PTTGC_EPIEntities db = new PTTGC_EPIEntities();

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
                string strID = Request.QueryString["strid"];
                if (!string.IsNullOrEmpty(strID))
                {
                    hdfEncryptCompanyID.Value = STCrypt.Decrypt(strID);
                    SetData(hdfEncryptCompanyID.Value.toIntNullToZero());
                    hdfEncryptCompanyID.Value = STCrypt.Encrypt(hdfEncryptCompanyID.Value);
                }
                else
                {
                    //CR. 06.02.2019 Sync from SAP 
                    txtCode.Enabled = false;
                    txtCompanyName.Enabled = false;
                }
            }
        }
    }

    private void SetData(int nCompID)
    {
        var query = db.mTCompany.FirstOrDefault(w => w.ID == nCompID);
        if (query != null)
        {
            txtCompanyName.Text = query.Name;
            txtCode.Text = query.sCode;
            txtDesc.Text = query.Description;
            rblStatus.SelectedValue = query.cActive;
            if (query.cActive == "N")
            {
                txtRemark.Text = query.sRemark;
                txtRemark.Enabled = true;
            }
        }
        //CR. 06.02.2019 Sync from SAP 
        txtCode.Enabled = false;
        txtCompanyName.Enabled = false;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveData(CSaveData data)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();

            Func<string, int?, bool> CheckDuplicateName = (name, id) =>
            {
                bool Isdup = false;
                var q = db.mTCompany.Where(w => (id.HasValue ? w.ID != id : true) && w.Name == name && w.cDel == "N");
                Isdup = q.Any();
                return Isdup;
            };
            Func<string, int?, bool> CheckDuplicateCode = (code, id) =>
            {
                bool Isdup = false;
                var q = db.mTCompany.Where(w => (id.HasValue ? w.ID != id : true) && w.sCode == code && w.cDel == "N");
                Isdup = q.Any();
                return Isdup;
            };
            string sCompID = data.sCompID.STCDecrypt();
            if (!string.IsNullOrEmpty(sCompID))
            {
                int nCompID = sCompID.toIntNullToZero();
                if (!CheckDuplicateName(data.sCompName, nCompID))
                {
                    if (!CheckDuplicateCode(data.sCode, nCompID))
                    {
                        var query = db.mTCompany.FirstOrDefault(w => w.ID == nCompID);
                        if (query != null)
                        {
                            query.Name = data.sCompName.Trims();
                            query.sCode = data.sCode.Trims();
                            query.Description = data.sDesc;
                            query.cActive = data.sStatus;
                            query.dUpdate = DateTime.Now;
                            query.UpdateID = UserAcc.GetObjUser().nUserID;
                            if (data.sStatus == "N")
                            {
                                query.sRemark = data.sRemark.Trims();
                            }
                            else
                            {
                                query.sRemark = "";
                            }
                            db.SaveChanges();
                            result.Status = SystemFunction.process_Success;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Not found data !";
                        }
                    }
                    else
                    {
                        result.Msg = "Duplicate SAP code !";
                        result.Status = SystemFunction.process_Failed;
                    }
                }
                else
                {
                    result.Msg = "Duplicate company name !";
                    result.Status = SystemFunction.process_Failed;
                }
            }
            else
            {
                if (!CheckDuplicateName(data.sCompName, null))
                {
                    if (!CheckDuplicateCode(data.sCode, null))
                    {
                        int nCompID = db.mTCompany.Any() ? db.mTCompany.Max(x => x.ID) + 1 : 1;
                        mTCompany t = new mTCompany();
                        t.ID = nCompID;
                        t.Name = data.sCompName.Trims();
                        t.sCode = data.sCode.Trims();
                        t.Description = data.sDesc;
                        t.cActive = data.sStatus;
                        if (data.sStatus == "N")
                        {
                            t.sRemark = data.sRemark.Trims();
                        }
                        t.CreateID = UserAcc.GetObjUser().nUserID;
                        t.dCreate = DateTime.Now;
                        t.UpdateID = UserAcc.GetObjUser().nUserID;
                        t.dUpdate = DateTime.Now;
                        t.cDel = "N";
                        db.mTCompany.Add(t);
                        db.SaveChanges();
                        result.Status = SystemFunction.process_Success;
                    }
                    else
                    {
                        result.Msg = "Duplicate SAP code !";
                        result.Status = SystemFunction.process_Failed;
                    }
                }
                else
                {
                    result.Msg = "Duplicate company name !";
                    result.Status = SystemFunction.process_Failed;
                }
            }
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    [Serializable]
    public class CSaveData
    {
        public string sCompID { get; set; }
        public string sCompName { get; set; }
        public string sCode { get; set; }
        public string sDesc { get; set; }
        public string sStatus { get; set; }
        public string sRemark { get; set; }
    }
}