using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sysExtension;
using System.Configuration;

public partial class loginAD : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strID = "";
        string[] userindname = HttpContext.Current.User.Identity.Name.Split('\\');
        if (userindname.Length >= 2)
        {
            strID = userindname[1] + "";
        }
        else//for test
        {
            strID = "";
        }

        string sUrl = "";
        if (userindname.Length >= 2)
        {
            sUrl = "~/login.aspx?strad=" + Server.UrlEncode(STCrypt.Encrypt(strID)) + "&&smod=%Agf4D%";
        }
        else//for test
        {
            sUrl = "~/login.aspx";
        }
        Response.Redirect(sUrl);
    }
}