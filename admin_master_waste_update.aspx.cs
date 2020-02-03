using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_master_waste_update : System.Web.UI.Page
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
                DDL_Disposal();
                string strID = Request.QueryString["str"];
                if (!string.IsNullOrEmpty(strID))
                {
                    hdfWasteID.Value = STCrypt.Decrypt(strID);
                    SetData(hdfWasteID.Value);
                }
            }
        }
    }

    private void SetData(string sID)
    {
        if (!string.IsNullOrEmpty(sID))
        {
            int nID = int.Parse(sID);
            var Query = db.TM_WasteDisposal.FirstOrDefault(w => w.ID == nID);
            if (Query != null)
            {
                txtCode.Text = Query.sCode;
                txtName.Text = Query.sName;
                ddlDisposal.SelectedValue = Query.nTypeID + "";
                rblStatus.SelectedValue = Query.cActive + "";
            }
        }
    }

    private void DDL_Disposal()
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstData = db.TData_Type.Where(w => w.cActive == "Y" && w.sType == "DISCODE").Select(s => new { Value = s.nID, Text = s.sName }).ToList();
        if (lstData.Count() > 0)
        {
            ddlDisposal.DataSource = lstData;
            ddlDisposal.DataValueField = "Value";
            ddlDisposal.DataTextField = "Text";
            ddlDisposal.DataBind();
            ddlDisposal.Items.Insert(0, new ListItem("- Disposal Type -", ""));
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
            int nWasteID = 0;
            Func<string, int?, bool> CheckDuplicateName = (name, id) =>
            {
                bool Isdup = false;
                var q = db.TM_WasteDisposal.Where(w => (id.HasValue ? w.ID != id : true) && w.sName == name && w.cDel == "N");
                Isdup = q.Any();
                return Isdup;
            };

            Func<string, int?, bool> CheckDuplicateCode = (code, id) =>
            {
                bool Isdup = false;
                var q = db.TM_WasteDisposal.Where(w => (id.HasValue ? w.ID != id : true) && w.sCode == code && w.cDel == "N");
                Isdup = q.Any();
                return Isdup;
            };

            if (string.IsNullOrEmpty(data.sWasteID)) // NEW
            {
                nWasteID = db.TM_WasteDisposal.Any() ? db.TM_WasteDisposal.Max(m => m.ID) + 1 : 1;
                TM_WasteDisposal t = new TM_WasteDisposal();

                if (!CheckDuplicateCode(data.sCode, null))
                {
                    if (!CheckDuplicateName(data.sName, null))
                    {
                        t.ID = nWasteID;
                        t.sCode = data.sCode.Trim();
                        t.sName = data.sName.Trim();
                        t.nTypeID = int.Parse(data.sDisposalType);
                        t.cActive = data.sStatus;
                        t.cDel = "N";
                        t.nCreateBy = UserAcc.GetObjUser().nUserID;
                        t.dCreate = DateTime.Now;
                        t.nUpdateBy = UserAcc.GetObjUser().nUserID;
                        t.dUpdate = DateTime.Now;
                        db.TM_WasteDisposal.Add(t);
                    }
                    else
                    {
                        result.Msg = "Duplicate Name !";
                        result.Status = SystemFunction.process_Failed;
                        return result;
                    }
                }
                else
                {
                    result.Msg = "Duplicate Code !";
                    result.Status = SystemFunction.process_Failed;
                    return result;
                }

            }
            else
            {
                nWasteID = int.Parse(data.sWasteID);

                if (!CheckDuplicateCode(data.sCode, nWasteID))
                {
                    if (!CheckDuplicateName(data.sName, nWasteID))
                    {
                        var Query = db.TM_WasteDisposal.FirstOrDefault(w => w.ID == nWasteID);
                        if (Query != null)
                        {
                            Query.sCode = data.sCode.Trim();
                            Query.sName = data.sName.Trim();
                            Query.nTypeID = int.Parse(data.sDisposalType);
                            Query.cActive = data.sStatus;
                            Query.cDel = "N";
                            Query.nUpdateBy = UserAcc.GetObjUser().nUserID;
                            Query.dUpdate = DateTime.Now;
                        }
                    }
                    else
                    {
                        result.Msg = "Duplicate Name !";
                        result.Status = SystemFunction.process_Failed;
                        return result;
                    }
                }
                else
                {
                    result.Msg = "Duplicate Code !";
                    result.Status = SystemFunction.process_Failed;
                    return result;
                }
            }
            db.SaveChanges();
            result.Status = SystemFunction.process_Success;
        }
        return result;
    }
    #endregion

    #region Class  
    [Serializable]
    public class CSaveData
    {
        public string sDisposalType { get; set; }
        public string sCode { get; set; }
        public string sName { get; set; }
        public string sStatus { get; set; }
        public string sWasteID { get; set; }
    }
    #endregion


}