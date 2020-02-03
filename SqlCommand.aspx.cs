using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SqlCommand : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lbCon.Text = SystemFunction.strConnect;
    }
    protected void btnExecute_Click(object sender, EventArgs e)
    {
        try
        {
            dgd.DataSource = CommonFunction.Get_Data(SystemFunction.strConnect, txtCommand.Text);
            dgd.DataBind();
        }
        catch (Exception ex)
        {

        }
    }
}