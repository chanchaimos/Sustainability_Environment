using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using sysExtension;

public partial class admin_asset_update : System.Web.UI.Page
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
                string strAssetID = Request.QueryString["strAssetID"];
                bool IsNew = false;
                int? nAssetID = null;
                if (!string.IsNullOrEmpty(str))
                {
                    hdfReturnStr.Value += "&&strid=" + HttpUtility.UrlEncode(str);
                    int nFacID = SystemFunction.GetIntNullToZero(STCrypt.Decrypt(str));
                    hdfFacilityID.Value = str;
                    var itemFac = db.mTFacility.FirstOrDefault(w => w.ID == nFacID && w.cDel == "N");
                    if (itemFac != null)
                    {
                        var itemCompany = db.mTCompany.FirstOrDefault(w => w.ID == itemFac.CompanyID);
                        if (!string.IsNullOrEmpty(strAssetID))
                        {
                            nAssetID = SystemFunction.GetIntNullToZero(STCrypt.Decrypt(strAssetID));
                            IsNew = false;
                            hdfAssetID.Value = strAssetID;
                            var itemAsset = db.mTFacility.FirstOrDefault(w => w.cDel == "N" && w.ID == nAssetID);
                            if (itemAsset != null)
                            {
                                ltrHeader.Text = "<a href='admin_company_lst.aspx' style='color:white'>Organization</a> > <a href='admin_facility_lst.aspx?strid=" + HttpUtility.UrlEncode(STCrypt.Encrypt(itemCompany.ID + "")) + "' style='color:white'>" + itemCompany.Name + "</a> > <a href='admin_asset_lst.aspx?strid=" + HttpUtility.UrlEncode(str) + "' style='color:white'>" + itemFac.Name + "</a> > " + itemAsset.Name + "  > Edit";//กำหนด Header กรณีเข้ามา EDIT
                            }
                        }
                        else
                        {
                            IsNew = true;
                            ltrHeader.Text = "<a href='admin_company_lst.aspx' style='color:white'>Organization</a> > <a href='admin_facility_lst.aspx?strid=" + HttpUtility.UrlEncode(STCrypt.Encrypt(itemCompany.ID + "")) + "' style='color:white'>" + itemCompany.Name + "</a> > <a href='admin_asset_lst.aspx?strid=" + HttpUtility.UrlEncode(str) + "' style='color:white'>" + itemFac.Name + "</a> > Sub - Facility  > Create";//กำหนด Header  กรณีเข้ามา ADD
                        }

                        hdfIsNew.Value = IsNew ? "C" : "E";
                    }
                    setDDL_Cbl(IsNew);
                    setData(nFacID, nAssetID);
                }
                if (string.IsNullOrEmpty(str) && string.IsNullOrEmpty(strAssetID))
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

    public void setData(int nFacID, int? nAssetID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var dataAsset = db.mTFacility.FirstOrDefault(w => w.ID == nAssetID);
        var dataFacility = db.mTFacility.FirstOrDefault(w => w.ID == nFacID);
        if (dataFacility != null)
        {
            txtFacilityName.Text = dataFacility.Name;
            //db.mTFacility_Operationtype.Where(w => w.nFacID == dataFacility.ID).ToList().ForEach(f =>
            //{
            //    ListItem itemFac = cblOperationTypeGC.Items.FindByValue(f.nOperationtypeID + "");
            //    if (itemFac != null)
            //    {
            //        cblOperationTypeGC.Items.FindByValue(f.nOperationtypeID + "").Selected = true;
            //    }
            //});
        }

        if (dataAsset != null)
        {
            rblOption.SelectedValue = dataAsset.sRefFacType == "" ? "N" : dataAsset.sRefFacType;
            if (rblOption.SelectedValue == "P")//SAP Master (Plant)
            {
                txtSAPFacilitySearch.Text = dataAsset.sRefFacCode + " - " + dataAsset.Name;
                txtSAPFacilityCode.Text = dataAsset.sRefFacCode;
            }
            else//Manual (None)
            {
                ddlCategory.SelectedValue = dataAsset.sRefFacSubType + "";
                txtSubFacManual.Text = dataAsset.Name;
                if (!string.IsNullOrEmpty(dataAsset.sRefFacCode))
                {
                    txtRefSAPCodeSearch.Text = dataAsset.sRefFacCode + " - " + dataAsset.Name;
                    txtRefSAPCodeValue.Text = dataAsset.sRefFacCode;
                }
                txtInternalCode.Text = dataAsset.sInternalCode + "";

                if (!string.IsNullOrEmpty(dataAsset.sRefFacSubType))
                {
                    ddlCategory.Enabled = false;
                }
            }

            txtDesc.Text = dataAsset.Description;
            txtRemark.Text = dataAsset.sRemark;
            //txtCodeSap.Text = dataAsset.sRefFacCode;
            //txtNameSap.Text = dataAsset.sSAPName;
            rblStatus.SelectedValue = dataAsset.cActive;
            if (rblStatus.SelectedValue == "N")
            {
                txtRemark.Enabled = true;
            }
        }
    }

    public void setDDL_Cbl(bool IsNew)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstOperationType = db.mOperationType.Where(w => w.cDel == "N" && w.cManage == "N" && (IsNew ? w.cActive == "Y" : true)).Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        var lstOperationTypeGC = db.mOperationType.Where(w => w.cDel == "N" && w.cManage == "Y" && (IsNew ? w.cActive == "Y" : true)).Select(s => new { Value = s.ID, Text = s.Name }).ToList();
        var lstFacilityPTT = db.mTFacility.Where(w => w.cDel == "N" && w.CompanyID == 1 && w.nLevel == 0 && (IsNew ? w.cActive == "Y" : true)).Select(s => new { Value = s.ID, Text = s.Name }).ToList();

        //if (lstOperationTypeGC.Count() > 0)
        //{
        //    cblOperationTypeGC.DataSource = lstOperationTypeGC;
        //    cblOperationTypeGC.DataValueField = "Value";
        //    cblOperationTypeGC.DataTextField = "Text";
        //    cblOperationTypeGC.DataBind();
        //}

        //foreach (ListItem item in cblOperationTypeGC.Items)
        //{
        //    item.Attributes["class"] = "col-md-3 col-sm-4 col-xs-6";
        //}
    }

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
            dataValue.sFacilityID = STCrypt.Decrypt(dataValue.sFacilityID);
            dataValue.sAssetID = !string.IsNullOrEmpty(dataValue.sAssetID) ? STCrypt.Decrypt(dataValue.sAssetID) : "";

            Func<string, int?, bool> CheckDuplicateName = (name, AssetID) =>
           {
               bool Isdup = false;
               var q = db.mTFacility.Where(w => (AssetID.HasValue ? w.ID != AssetID : true) && w.nLevel == 2 && w.Name.Trim().ToLower() == name.Trim().ToLower() && w.cDel == "N");
               Isdup = q.Any();
               return Isdup;
           };

            Func<string, int?, bool> CheckDuplicateNameSAP = (name, AssetID) =>
            {
                bool Isdup = false;
                var q = db.mTFacility.Where(w => (AssetID.HasValue ? w.ID != AssetID : true) && w.nLevel == 2 && w.sSAPName.Trim().ToLower() == name.Trim().ToLower() && w.cDel == "N");
                Isdup = q.Any();
                return Isdup;
            };

            Func<string, int?, bool> CheckDuplicateCodeSAP = (code, AssetID) =>
            {
                bool Isdup = false;
                var q = db.mTFacility.Where(w => (AssetID.HasValue ? w.ID != AssetID : true) && w.nLevel == 2 && w.sRefFacCode.Trim().ToLower() == code.Trim().ToLower() && w.cDel == "N");
                Isdup = q.Any();
                return Isdup;
            };

            #region  Add / Update
            if (dataValue.sRefFacType == "N" && CheckDuplicateName(dataValue.sAssetName, dataValue.IsNew ? (int?)null : SystemFunction.GetIntNullToZero(dataValue.sAssetID)))
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Already Sub-facility Name !";
                return result;
            }

            if (dataValue.sRefFacType == "P" && !string.IsNullOrEmpty(dataValue.sRefFacCode))
            {
                if (CheckDuplicateCodeSAP(dataValue.sRefFacCode, dataValue.IsNew ? (int?)null : SystemFunction.GetIntNullToZero(dataValue.sAssetID)))
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Already SAP Code !";
                    return result;
                }
            }

            bool IsPass = true;
            int sAssetID = SystemFunction.GetIntNullToZero(dataValue.sAssetID);
            int nFacilityID = SystemFunction.GetIntNullToZero(dataValue.sFacilityID);
            var itemAsset = db.mTFacility.FirstOrDefault(w => w.ID == sAssetID);
            var itemFacility = db.mTFacility.FirstOrDefault(w => w.ID == nFacilityID);
            int nMaxID = db.mTFacility.Any() ? db.mTFacility.Max(m => m.ID) + 1 : 1;
            if (dataValue.IsNew)
            {
                itemAsset = new mTFacility();
                itemAsset.ID = nMaxID;
                itemAsset.cDel = "N";
                itemAsset.CreateID = UserAcc.GetObjUser().nUserID;
                itemAsset.dCreate = DateTime.Now;
            }

            if (dataValue.sRefFacType == "P")
            {
                #region SAP Master
                var dataComp = db.mTCompany.FirstOrDefault(w => w.ID == itemFacility.CompanyID);
                if (dataComp != null)
                {
                    var qSAPFac = db.v_TM_SAP_ALLFAC.FirstOrDefault(w => w.sCode == dataValue.sRefFacCode && w.sCompCode == dataComp.sCode);
                    if (qSAPFac != null)
                    {
                        itemAsset.sRefFacCode = dataValue.sRefFacCode;
                        itemAsset.Name = qSAPFac.sName.Trims();
                        itemAsset.sRefFacSubType = qSAPFac.sType;
                        itemAsset.sInternalCode = "";
                    }
                    else
                    {
                        IsPass = false;
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Not found SAP master data !";
                    }
                }
                else
                {
                    IsPass = false;
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Not found company data !";
                }
                #endregion
            }
            else
            {
                #region Manual Create
                var dataComp = db.mTCompany.FirstOrDefault(w => w.ID == itemFacility.CompanyID);
                if (dataComp != null)
                {
                    if (!string.IsNullOrEmpty(dataValue.sRefFacCode))//มีการอัพเดท SAP Code หลังจากที่ manual create
                    {
                        var qSAPFac = db.v_TM_SAP_ALLFAC.FirstOrDefault(w => w.sCode == dataValue.sRefFacCode && w.sCompCode == dataComp.sCode);
                        if (qSAPFac != null)
                        {
                            itemAsset.sRefFacCode = dataValue.sRefFacCode;
                            itemAsset.Name = qSAPFac.sName.Trims();
                        }
                        else
                        {
                            IsPass = false;
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Not found SAP master data !";
                        }
                    }
                    else
                    {
                        itemAsset.Name = dataValue.sAssetName;
                        itemAsset.sRefFacCode = "";
                    }

                    itemAsset.sRefFacSubType = dataValue.sRefFacSubType;
                    if (dataValue.IsNew || string.IsNullOrEmpty(itemAsset.sInternalCode))
                    {
                        string sType = dataValue.sRefFacSubType == "O" ? "Z" : dataValue.sRefFacSubType;
                        char cType = '-';
                        switch (sType)
                        {
                            case "O": cType = 'Z'; break;
                            case "S": cType = 'S'; break;
                            case "P": cType = 'P'; break;
                        }

                        Func<string, int> GetNumber = (code) =>
                        {
                            string[] arr = code.Split(cType);
                            return (arr[arr.Length - 1]).toIntNullToZero();
                        };

                        var qFacInternalCode = db.mTFacility.Where(w => w.cDel == "N" && w.CompanyID == itemFacility.CompanyID && w.sInternalCode.Contains(sType)).AsEnumerable().Select(s => new
                        {
                            nNumber = GetNumber(s.sInternalCode)
                        }).ToList();
                        int nNumber = qFacInternalCode.Any() ? qFacInternalCode.Max(x => x.nNumber) + 1 : 1;
                        itemAsset.sInternalCode = dataComp.sCode + sType + nNumber; //EX. 10Z1
                    }
                }
                else
                {
                    IsPass = false;
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Not found company data !";
                }
                #endregion
            }

            if (IsPass)
            {
                itemAsset.sRefFacType = dataValue.sRefFacType;
                itemAsset.nLevel = 2;
                itemAsset.nHeaderID = itemFacility.ID;
                itemAsset.sRelation = itemFacility.sRelation + "-" + itemAsset.ID;
                itemAsset.OperationTypeID = itemFacility.OperationTypeID;
                itemAsset.CompanyID = itemFacility.CompanyID;
                itemAsset.Description = dataValue.sDescription;
                itemAsset.cActive = dataValue.sActive;
                itemAsset.sRemark = dataValue.sActive == "Y" ? null : dataValue.sRemark;
                itemAsset.UpdateID = dataValue.IsNew ? (int?)null : UserAcc.GetObjUser().nUserID;
                itemAsset.dUpdate = DateTime.Now;
                if (dataValue.IsNew) db.mTFacility.Add(itemAsset);

                db.SaveChanges();
                result.Status = SystemFunction.process_Success;
            }
            #endregion
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static List<TDataSAPFacility> SearchSAPFacility(string sSearch, string sFacilityID, string sAssesstID)
    {
        List<TDataSAPFacility> lstData = new List<TDataSAPFacility>();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int nFacID = STCrypt.Decrypt(sFacilityID).toIntNullToZero();
        var dataFac = db.mTFacility.FirstOrDefault(w => w.ID == nFacID);
        if (dataFac != null)
        {
            var dataCompany = db.mTCompany.FirstOrDefault(w => w.ID == dataFac.CompanyID);
            if (dataCompany != null)
            {
                int? nAssessetID = null;
                if (!string.IsNullOrEmpty(sAssesstID))
                {
                    nAssessetID = STCrypt.Decrypt(sAssesstID).toIntNull();
                }

                var qFacSAPCode = db.mTFacility.Where(w => w.cDel == "N" && w.nLevel == 2 && !string.IsNullOrEmpty(w.sRefFacCode) && (nAssessetID.HasValue ? w.ID != nAssessetID : true)).Select(s => s.sRefFacCode).ToList();
                sSearch = sSearch.Trims().ToLower();
                var dataVPlant = db.v_TM_SAP_ALLFAC.Where(w => w.sType == "P" && !qFacSAPCode.Contains(w.sCode) && w.sCompCode == dataCompany.sCode && ((w.sName + "").ToLower().Contains(sSearch) || (w.sShortName + "").ToLower().Contains(sSearch) || (w.sCode + "").ToLower().Contains(sSearch))).Select(s => new TDataSAPFacility { sCode = s.sCode, sName = s.sCode + " - " + s.sName }).OrderBy(o => o.sName).Take(30).ToList();
                var dataVComp = db.v_TM_SAP_ALLFAC.Where(w => w.sType == "O" && !qFacSAPCode.Contains(w.sCode) && ((w.sName + "").ToLower().Contains(sSearch) || (w.sShortName + "").ToLower().Contains(sSearch) || (w.sCode + "").ToLower().Contains(sSearch))).Select(s => new TDataSAPFacility { sCode = s.sCode, sName = s.sCode + " - " + s.sName }).OrderBy(o => o.sName).Take(30).ToList();
                lstData = dataVPlant.Concat(dataVComp).OrderBy(o => o.sName).Take(30).ToList();
            }
        }
        return lstData;
    }

    #region Class
    [Serializable]
    public class DataValue
    {
        public string sAssetID { get; set; }
        public string sFacilityID { get; set; }
        /// <summary>
        /// Option >> P=PLANT(SAP Master), N=NONE(Manual)
        /// </summary>
        public string sRefFacType { get; set; }
        /// <summary>
        /// Category >> S=Storage, P = Plan/sub, O=Office
        /// </summary>
        public string sRefFacSubType { get; set; }
        /// <summary>
        /// SAP MASTER CODE
        /// </summary>
        public string sRefFacCode { get; set; }
        /// <summary>
        /// Manual Name
        /// </summary>
        public string sAssetName { get; set; }

        //public string sAssetName { get; set; }
        public string sDescription { get; set; }
        public string sActive { get; set; }
        public string sRemark { get; set; }
        public bool IsNew { get; set; }
        //public string sCodeSAP { get; set; }
        //public string sNameSAP { get; set; }
    }
    #endregion

    [Serializable]
    public class TDataSAPFacility
    {
        public string sCode { get; set; }
        public string sName { get; set; }
    }
}