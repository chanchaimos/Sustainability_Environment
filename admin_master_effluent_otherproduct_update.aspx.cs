using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_master_effluent_otherproduct_update : System.Web.UI.Page
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
                string strID = Request.QueryString["str"];
                if (!string.IsNullOrEmpty(strID))
                {
                    hdfProductID.Value = STCrypt.Decrypt(strID);
                    SetData(hdfProductID.Value);
                }
            }
        }
    }

    private void SetData(string sID)
    {
        if (!string.IsNullOrEmpty(sID))
        {
            int nID = int.Parse(sID);
            var Query = db.TM_Effluent_OtherProduct.FirstOrDefault(w => w.nProductID == nID);
            if (Query != null)
            {
                txtName.Text = Query.sName;
                rblStatus.SelectedValue = Query.cActive + "";
            }
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
            int nProductID = 0;
            Func<string, int?, bool> CheckDuplicateName = (name, id) =>
            {
                bool Isdup = false;
                var q = db.TM_Effluent_OtherProduct.Where(w => (id.HasValue ? w.nProductID != id : true) && w.sName == name && w.cDel == "N");
                Isdup = q.Any();
                return Isdup;
            };

            if (string.IsNullOrEmpty(data.sProductID)) // NEW
            {
                nProductID = db.TM_Effluent_OtherProduct.Any() ? db.TM_Effluent_OtherProduct.Max(m => m.nProductID) + 1 : 1;
                TM_Effluent_OtherProduct t = new TM_Effluent_OtherProduct();

                if (!CheckDuplicateName(data.sName, null))
                {
                    t.nProductID = nProductID;
                    t.sName = data.sName.Trim();
                    t.cActive = data.sStatus;
                    t.cDel = "N";
                    t.nAddBy = UserAcc.GetObjUser().nUserID;
                    t.dAdd = DateTime.Now;
                    t.nUpdateBy = UserAcc.GetObjUser().nUserID;
                    t.dUpdate = DateTime.Now;
                    db.TM_Effluent_OtherProduct.Add(t);
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
                nProductID = int.Parse(data.sProductID);
                if (!CheckDuplicateName(data.sName, nProductID))
                {
                    var Query = db.TM_Effluent_OtherProduct.FirstOrDefault(w => w.nProductID == nProductID);
                    if (Query != null)
                    {
                        Query.sName = data.sName.Trim();
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
        public string sName { get; set; }
        public string sStatus { get; set; }
        public string sProductID { get; set; }
    }
    #endregion
}