using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Helper_Indicator : System.Web.UI.Page
{
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnLoad(myFunc);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string sIndicatorID = Request.QueryString["ind"];
        string sProductID = Request.QueryString["prd"];
        if (!string.IsNullOrEmpty(sIndicatorID) && !string.IsNullOrEmpty(sProductID))
        {
            SetFocus(sIndicatorID, sProductID);
        }
    }
    private void SetFocus(string sIndicatorID, string sProductID)
    {
        int nIndID = SystemFunction.ParseInt(sIndicatorID);
        int nProductID = SystemFunction.ParseInt(sProductID);
        switch (nIndID)
        {
            case 1:
                SetBodyEventOnLoad("formfocus('divEC')");
                break;
            case 2:
                SetBodyEventOnLoad("formfocus('divERC')");
                break;
            case 3:
                SetBodyEventOnLoad("formfocus('C24')");
                break;
            case 4:
                SetBodyEventOnLoad("formfocus('E" + nProductID + "')");
                break;
            case 5:
                SetBodyEventOnLoad("formfocus('C24')");
                break;
            case 6://Intensity Denominator
                SetBodyEventOnLoad("formfocus('dvIntensity')");
                break;
            case 7:
                SetBodyEventOnLoad("formfocus('C24')");
                break;
            case 8://Material
                switch (nProductID)
                {
                    case 33: SetBodyEventOnLoad("formfocus('dvMaterial')"); break;
                    case 34: SetBodyEventOnLoad("formfocus('dvMaterial')"); break;
                    case 37: SetBodyEventOnLoad("formfocus('dvMaterial')"); break;
                    case 41: SetBodyEventOnLoad("formfocus('TBM2')"); break;
                }
                break;
            case 9:// SPILL
                switch (nProductID)
                {
                    case 90: SetBodyEventOnLoad("formfocus('ST1')"); break;
                    case 91: SetBodyEventOnLoad("formfocus('ST2')"); break;
                    default: SetBodyEventOnLoad("formfocus('divSpill')"); break;
                }
                break;
            case 10: //Waste
                switch (nProductID)
                {
                    case 1: SetBodyEventOnLoad("formfocus('dvWaste')"); break;
                    case 16: SetBodyEventOnLoad("formfocus('TBW2')"); break;
                    case 104: SetBodyEventOnLoad("formfocus('TBW3')"); break;
                }
                break;
            case 11://Water
                switch (nProductID)
                {
                    case 91:
                        SetBodyEventOnLoad("formfocus('divWaterWithdrawal')");
                        break;
                    case 101:
                        SetBodyEventOnLoad("formfocus('divWaterReuse')");
                        break;
                }
                break;
        }
    }
}