using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_facility_update : System.Web.UI.Page
{
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnLoad(myFunc);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!UserAcc.UserExpired())
            {
                PTTGC_EPIEntities db = new PTTGC_EPIEntities();
                string str = Request.QueryString["strid"];
                string strFacID = Request.QueryString["strFacID"];
                int nFacilityID = 0;
                bool IsNew = false;
                if (!string.IsNullOrEmpty(str))
                {
                    hdfReturnStr.Value += "&&strid=" + HttpUtility.UrlEncode(str);
                    int nComID = SystemFunction.GetIntNullToZero(STCrypt.Decrypt(str));
                    hdfComType.Value = nComID == 1 ? "PTT" : "GC";
                    hdfComID.Value = str;
                    var itemCompany = db.mTCompany.FirstOrDefault(w => w.ID == nComID && w.cDel == "N");
                    if (itemCompany != null)
                    {
                        if (!string.IsNullOrEmpty(strFacID))
                        {
                            IsNew = false;
                            nFacilityID = SystemFunction.GetIntNullToZero(STCrypt.Decrypt(strFacID));
                            hdfFacilityID.Value = strFacID;
                            var itemFacility = db.mTFacility.FirstOrDefault(w => w.cDel == "N" && w.ID == nFacilityID);
                            if (itemFacility != null)
                            {
                                ltrHeader.Text = "<a href='admin_company_lst.aspx' style='color:white'>Organization</a> > <a href='admin_facility_lst.aspx?strid=" + HttpUtility.UrlEncode(str) + "' style='color:white'>" + itemCompany.Name + "</a> > " + itemFacility.Name + "  > Edit";//กำหนด Header กรณีเข้ามา EDIT
                            }
                        }
                        else
                        {
                            IsNew = true;
                            ltrHeader.Text = "<a href='admin_company_lst.aspx' style='color:white'>Organization</a> > <a href='admin_facility_lst.aspx?strid=" + HttpUtility.UrlEncode(str) + "' style='color:white'>" + itemCompany.Name + "</a> > Facility  > Create";//กำหนด Header  กรณีเข้ามา ADD
                        }

                        hdfIsNew.Value = IsNew ? "C" : "E";
                    }
                    setDDL_Cbl(IsNew, nComID);
                    setData(nFacilityID);
                }
                if (string.IsNullOrEmpty(str) && string.IsNullOrEmpty(strFacID))
                {
                    SetBodyEventOnLoad(SystemFunction.DialogWarningRedirect(SystemFunction.Msg_HeadWarning, "Invalid Data", "admin_company_lst.aspx"));// กรณีเข้ามาด้วย link ที่ไม่มี Querystring
                }
            }
            else
            {
                SetBodyEventOnLoad(SystemFunction.PopupLogin());
            }
        }
    }

    public void setData(int nFacilityID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var dataFacility = db.mTFacility.FirstOrDefault(w => w.ID == nFacilityID);

        if (dataFacility != null)
        {
            txtFacilityName.Text = dataFacility.Name;
            ddlOperationType.SelectedValue = dataFacility.OperationTypeID + "";
            txtDesc.Text = dataFacility.Description;
            txtRemark.Text = dataFacility.sRemark;
            rblStatus.SelectedValue = dataFacility.cActive;
            txtMapPTTCode.Text = dataFacility.sMappingCodePTT;
            if (rblStatus.SelectedValue == "N")
            {
                txtRemark.Enabled = true;
            }
            if (hdfComType.Value == "GC")
            {
                ddlFacilityPTT.SelectedValue = dataFacility.nHeaderID + "";
                var lstOperationTypeGC = db.mOperationType.Where(w => w.cDel == "N" && w.cManage == "Y").ToList();

                //db.mTFacility_Operationtype.Where(w => w.nFacID == nFacilityID).ToList().ForEach(f =>
                //{
                //    if (lstOperationTypeGC.Any(a => a.ID == f.nOperationtypeID))
                //    {
                //        cblOperationTypeGC.Items.FindByValue(f.nOperationtypeID + "").Selected = true;
                //    }
                //});

            }
        }
    }

    public void setDDL_Cbl(bool IsNew, int nComID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstOperationType = db.mOperationType.Where(w => w.cDel == "N" && w.cManage == "N" && (IsNew ? w.cActive == "Y" : true)).Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        var lstOperationTypeGC = db.mOperationType.Where(w => w.cDel == "N" && w.cManage == "Y" && (IsNew ? w.cActive == "Y" : true)).Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        var lstFacilityPTT = db.mTFacility.Where(w => w.cDel == "N" && w.CompanyID == 1 && w.nLevel == 0 && (IsNew ? w.cActive == "Y" : true)).Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        if (lstOperationType.Count() > 0)
        {
            ddlOperationType.DataSource = lstOperationType;
            ddlOperationType.DataValueField = "Value";
            ddlOperationType.DataTextField = "Text";
            ddlOperationType.DataBind();
            ddlOperationType.Items.Insert(0, new ListItem("- Operation Type -", ""));
        }
        else
        {
            ddlOperationType.Items.Insert(0, new ListItem("- Operation Type -", ""));
        }
        //if (lstOperationTypeGC.Count() > 0)
        //{
        //    cblOperationTypeGC.DataSource = lstOperationTypeGC;
        //    cblOperationTypeGC.DataValueField = "Value";
        //    cblOperationTypeGC.DataTextField = "Text";
        //    cblOperationTypeGC.DataBind();

        //    foreach (ListItem item in cblOperationTypeGC.Items)
        //    {
        //        item.Attributes["class"] = "col-md-3 col-sm-4 col-xs-6";
        //    }
        //}
        if (lstFacilityPTT.Count() > 0)
        {
            ddlFacilityPTT.DataSource = lstFacilityPTT;
            ddlFacilityPTT.DataValueField = "Value";
            ddlFacilityPTT.DataTextField = "Text";
            ddlFacilityPTT.DataBind();
            ddlFacilityPTT.Items.Insert(0, new ListItem("- Facility Name -", ""));
        }
        else
        {
            ddlFacilityPTT.Items.Insert(0, new ListItem("- Facility Name -", ""));
        }
    }

    #region WebMethod
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod getOperationTypePTT(string sFacilityPTT_ID)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        if (!UserAcc.UserExpired())
        {
            int nFacilityPTT_ID = SystemFunction.GetIntNullToZero(sFacilityPTT_ID);
            var item = db.mTFacility.FirstOrDefault(w => w.ID == nFacilityPTT_ID);
            if (item != null)
            {
                result.Content = item.OperationTypeID + "";
                result.Status = SystemFunction.process_Success;
            }
            else
            {
                result.Msg = "Data operation type not found.";
                result.Status = SystemFunction.process_Failed;
            }
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod saveToDB(DataValue dataValue)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        if (!UserAcc.UserExpired())
        {
            dataValue.sComID = STCrypt.Decrypt(dataValue.sComID);
            dataValue.sFacilityID = !string.IsNullOrEmpty(dataValue.sFacilityID) ? STCrypt.Decrypt(dataValue.sFacilityID) : "";
            Func<string, int, int?, int, bool> CheckDuplicateName = (name, comID, facID, nLevel) =>
            {
                bool Isdup = false;
                var q = db.mTFacility.Where(w => w.CompanyID == comID && w.nLevel == nLevel && (facID.HasValue ? w.ID != facID : true) && w.Name == name && w.cDel == "N");
                Isdup = q.Any();
                return Isdup;
            };
            #region  Add / Update
            if (CheckDuplicateName(dataValue.sFacilityName, SystemFunction.GetIntNullToZero(dataValue.sComID), dataValue.IsNew ? (int?)null : SystemFunction.GetIntNullToZero(dataValue.sFacilityID), (dataValue.sComType == "GC" ? 1 : 0)))
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Duplicate Facility name";
                return result;
            }

            int nFacility_PTT = SystemFunction.GetIntNullToZero(dataValue.sFacilityPTT_ID);
            int nFacilityID = SystemFunction.GetIntNullToZero(dataValue.sFacilityID);
            if (dataValue.sComType == "GC" && nFacility_PTT != 0)
            {
                var query = db.mTFacility.Where(w => w.cDel == "N" && w.nLevel == 1 && w.nHeaderID == nFacility_PTT && w.ID != nFacilityID);
                if (query.Any())
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Cannot reference PTT facility, because exist used.";
                    return result;
                }
            }

            int nComID = SystemFunction.GetIntNullToZero(dataValue.sComID);
            var itemFacility = db.mTFacility.FirstOrDefault(w => w.ID == nFacilityID && w.CompanyID == nComID);
            int nMaxID = db.mTFacility.Any() ? db.mTFacility.Max(m => m.ID) + 1 : 1;
            if (dataValue.IsNew)
            {
                itemFacility = new mTFacility();
                itemFacility.ID = nMaxID;
                itemFacility.cDel = "N";
                itemFacility.CreateID = UserAcc.GetObjUser().nUserID;
                itemFacility.dCreate = DateTime.Now;
            }
            itemFacility.sMappingCodePTT = (dataValue.sMapPTTCode + "").Trim();
            itemFacility.Name = dataValue.sFacilityName;
            itemFacility.nLevel = dataValue.sComType == "GC" ? 1 : 0;
            itemFacility.nHeaderID = dataValue.sComType == "GC" ? (nFacility_PTT == 0 ? (int?)null : nFacility_PTT) : (int?)null;
            itemFacility.sRelation = dataValue.sComType == "GC" ? (nFacility_PTT == 0 ? itemFacility.ID + "" : nFacility_PTT + "-" + itemFacility.ID) : itemFacility.ID + "";
            //itemFacility.sRelation = dataValue.sComType == "GC" ? nFacility_PTT + "-" + itemFacility.ID : itemFacility.ID + "";
            itemFacility.OperationTypeID = SystemFunction.GetIntNullToZero(dataValue.sOperationTypePTT_ID);
            itemFacility.CompanyID = SystemFunction.GetIntNullToZero(dataValue.sComID);
            itemFacility.Description = dataValue.sDescription;
            itemFacility.cActive = dataValue.sActive;
            itemFacility.sRemark = dataValue.sActive == "Y" ? null : dataValue.sRemark;
            itemFacility.UpdateID = dataValue.IsNew ? (int?)null : UserAcc.GetObjUser().nUserID;
            itemFacility.dUpdate = DateTime.Now;
            if (dataValue.IsNew) db.mTFacility.Add(itemFacility);

            if (dataValue.sComType == "GC")
            {
                //db.mTFacility_Operationtype.RemoveRange(db.mTFacility_Operationtype.Where(w => w.nFacID == itemFacility.ID));
                //foreach (var item in dataValue.lstOperationTypeGC_ID)
                //{
                //    mTFacility_Operationtype oprt = new mTFacility_Operationtype();
                //    oprt.nFacID = itemFacility.ID;
                //    oprt.nOperationtypeID = SystemFunction.GetIntNullToZero(item);
                //    db.mTFacility_Operationtype.Add(oprt);
                //}
            }
            else
            {
                if (!dataValue.IsNew)
                {
                    db.mTFacility.Where(w => w.nHeaderID == itemFacility.ID && w.cDel == "N").ToList().ForEach(f =>
                    {
                        f.OperationTypeID = itemFacility.OperationTypeID;
                    });
                }
            }
            db.SaveChanges();
            result.Status = SystemFunction.process_Success;
            #endregion
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }
    #endregion

    #region Class
    [Serializable]
    public class DataValue
    {
        public string sComID { get; set; }
        public string sFacilityID { get; set; }
        public string sFacilityName { get; set; }
        public string sFacilityPTT_ID { get; set; }
        public string sOperationTypePTT_ID { get; set; }
        public List<string> lstOperationTypeGC_ID { get; set; }
        public string sDescription { get; set; }
        public string sActive { get; set; }
        public string sRemark { get; set; }
        public string sComType { get; set; }
        public bool IsNew { get; set; }

        public string sMapPTTCode { get; set; }
    }
    #endregion
}

