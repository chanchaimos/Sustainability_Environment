using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sysExtension;
using System.Web.Script.Services;
using System.Web.Services;
using System.Configuration;

public partial class _MP_EPI_FORMS_Output : System.Web.UI.MasterPage
{
    public void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnLoad(myFunc);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string sIN = Request.QueryString["in"] + "";
        string sOT = Request.QueryString["ot"] + "";

        if (!string.IsNullOrEmpty(sIN))
        {
            hdfsIndicator.Value = STCrypt.Decrypt(sIN);
        }
        if (!string.IsNullOrEmpty(sOT))
        {
            hdfsOperationType.Value = STCrypt.Decrypt(sOT);
        }
        SystemFunction.ListYearsDESC(ddlYear, "-select-", "en-US", "th-TH", short.Parse(ConfigurationSettings.AppSettings["startYear"].ToString()));
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static SetPageLoadMaster LoadData()
    {
        SetPageLoadMaster o = new SetPageLoadMaster();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lstIn = db.mTIndicator.ToList();
        o.lstIn = new List<T_mTIndicator>();
 
        if (lstIn.Count > 0)
        {
            o.lstIn = lstIn.Select(s => new T_mTIndicator
            {
                ID = s.ID,
                Indicator = s.Indicator,
                lstProIn = new List<T_mTProductIndicator>()
            }).ToList();
            var lstProIn = db.mTProductIndicator.Where(w => w.cManage == "N" && w.cDel == "N").Select(s => new T_mTProductIndicator
            {
                ProductID = s.ProductID,
                ProductName = s.ProductName,
                nOrder = s.nOrder,
                IDIndicator = s.IDIndicator
            }).OrderBy(r => r.ProductID).ToList();

            o.lstIn.ForEach(f =>
            {
                if (lstProIn.Count > 0)
                {
                    f.lstProIn = lstProIn.Where(w => w.IDIndicator == f.ID).ToList();
                }

            });
        }


        return o;
    }
    public class SetPageLoadMaster
    {
        public List<T_mTIndicator> lstIn { get; set; }
    }
    public class T_mTIndicator
    {
        public int ID { get; set; }
        public string Indicator { get; set; }
        public List<T_mTProductIndicator> lstProIn { get; set; }
    }
    public class T_mTProductIndicator
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int? nOrder { get; set; }
        public int? IDIndicator { get; set; }
    }
    public string Indicator
    {
        set { hdfsddlIndicator.Value = value; }
        get { return hdfsddlIndicator.Value; }
    }
    public string OperationType
    {
        set { hdfsddlOperationType.Value = value; }
        get { return hdfsddlOperationType.Value; }
    }
    public string Facility
    {
        set { hdfsddlFacility.Value = value; }
        get { return hdfsddlFacility.Value; }
    }
    public string Year
    {
        set { hdfsddlYear.Value = value; }
        get { return hdfsddlYear.Value; }
    }
}
