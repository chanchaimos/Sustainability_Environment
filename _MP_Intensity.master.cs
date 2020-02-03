using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _MP_Intensity : System.Web.UI.MasterPage
{
    public const int nMenu = 7;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!UserAcc.UserExpired())
        {
            ((_MP_EPI_FORMS)this.Master).hdfPRMS = SystemFunction.GetPermissionMenu(nMenu) + "";
            ((_MP_EPI_FORMS)this.Master).hdfCheckRole = UserAcc.GetObjUser().nRoleID+"";

            hdfsRoleID.Value =  UserAcc.GetObjUser().nRoleID + "";

        }
      
    }
}
