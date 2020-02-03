using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _MP_Front : System.Web.UI.MasterPage
{
    public void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_AllSource)this.Master).SetBodyEventOnLoad(myFunc);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (UserAcc.UserExpired() && SystemFunction.GetURL(Page.AppRelativeVirtualPath.ToString()) != "helper_indicator.aspx")
        {
            SetBodyEventOnLoad(SystemFunction.PopupLogin());
        }
        else
        {
            //var DataRole = UserAcc.GetRolePermission(UserAcc.GetObjUser().nUserID + "");
            //dvUSer.Visible = true;
            //if (DataRole.Count == 1)
            //{
            //    liChange.Visible = false;
            //    liChange2.Visible = false;
            //}
            string Url = SystemFunction.GetURL(Page.AppRelativeVirtualPath);
            if(SystemFunction.GetURL(Page.AppRelativeVirtualPath.ToString()) != "helper_indicator.aspx")
            {
                lrtMenu.Text = SystemFunction.HTML_Menubar(Url);
                lrtNav_menu.Text = SystemFunction.HTML_Navtab(Url);
                lrtNav_UserMenu.Text = SystemFunction.HTML_NavtabUser(Url);
            }
            else
            {
                dvUSer.Visible = false;
            }
            ltrFullName1.Text = ltrFullName2.Text = UserAcc.GetObjUser().sFullName;
            ltrActionRole1.Text = ltrActionRole2.Text = UserAcc.GetObjUser().sActionRoleName;
        }
    }


    public static string HTML_Menubar()
    {
        string sHTML = "";
        string SS = @" <ul class='menu'>
                       <li><a class='active'><i class='fa fa-star'></i>&nbsp;My Task</a></li>
                       <li><a href = 'Intensity_from.aspx'><i class='fa fa-clipboard-list'></i>&nbsp;Input</a></li> 
                       <li><a><i class='fa fa-paste'></i>&nbsp;Output</a></li>
                        <li>
                            <a><i class='fa fa-eye'></i>&nbsp;Monitoring</a>
                        </li>
                        <li>
                            <a href ='f_ContactUs.aspx'>< i class='fa fa-envelope'></i>&nbsp;Contact Us</a>
                        </li>
                        <li class='has-children'>
                            <a><i class='fa fa-cogs'></i>&nbsp;Administrator</a>
                            <ul class='menu-sub'>
                                <li><a href ='admin_user_info_lst.aspx' >< i class='fa fa-cog'></i>&nbsp;User Info</a></li>
                                <li><a href ='admin_work_flow.aspx' >< i class='fa fa-cog'></i>&nbsp;Workflow</a></li>
                                <li><a href ='admin_company_lst.aspx' >< i class='fa fa-cog'></i>&nbsp;Organization</a></li>
                                <li><a href ='admin_operation_type_lst.aspx' >< i class='fa fa-cog'></i>&nbsp;Operation type</a></li>
                                <li><a href ='admin_ContactUs_lst.aspx' >< i class='fa fa-envelope'></i>&nbsp;Contact Us</a></li>
                            </ul>
                        </li>
                    </ul>";

        //string sFotmat = @"<li><a id='{3}' href='{1}'>
        //                            <div class='menu-icon'><i class='{2}'></i></div>
        //                            <div class='menu-name text-uppercase'>{0}</div>
        //                        </a>{4}</li>";

        string sFotmat = @"<li><a id='{3}' href='{1}'><i class=''{2}''></i>&nbsp;My Task</a>{4}</li>";

        string QUERY = @" SELECT nMenuID 'sMenuID' ,sMenuName,ISNULL(sMenuLink,'#') 'sUrl',sClassIcon FROM TMenu WHERE cActive='Y' AND sMenuHeadID='0' ORDER BY sMenuOrder ASC ";

        string sFormat2 = @"<li><a id='{3}' href='{1}'>< i class='{2}'></i>&nbsp;{0}</a></li>";

        string sHTML2 = "";
        DataTable dt = CommonFunction.Get_Data(SystemFunction.strConnect, QUERY);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            sHTML2 = "";
            if (dt.Rows[i]["sMenuID"].ToString() == "6")
            {
                string sQuery2 = @" SELECT nMenuID 'sMenuID' ,sMenuName,ISNULL(sMenuLink,'#') 'sUrl',sClassIcon FROM TMenu WHERE cActive='Y' AND cType='1' AND sMenuHeadID = '6' ORDER BY sMenuOrder ASC ";
                DataTable dt2 = CommonFunction.Get_Data(SystemFunction.strConnect, sQuery2);
                sHTML2 = "<ul class='menu-sub'>";
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    sHTML2 += string.Format(sFormat2, dt2.Rows[j]["sMenuName"] + "", dt2.Rows[j]["sUrl"] + "", dt2.Rows[j]["sClassIcon"] + "", "SubHead_" + dt2.Rows[j]["sMenuID"]);
                }
                sHTML2 += "</ul>";
            }
            sHTML += string.Format(sFotmat, dt.Rows[i]["sMenuName"] + "", dt.Rows[i]["sUrl"] + "", dt.Rows[i]["sClassIcon"] + "", "Head_" + dt.Rows[i]["sMenuID"], sHTML2);
        }

        return "<ul id='menuBar'>" + sHTML + "</ul>";

        //return @"<ul id='menuBar'>
        //                            <li><a href='index.aspx' class='hvr-underline-from-left hvr-fill-gray'>
        //                                <div class='menu-icon'><i class='glyphicon glyphicon-globe'></i></div>
        //                                <div class='menu-name text-uppercase'>Home</div>
        //                            </a></li>
        //                            <li><a href='registration_import.aspx' class='hvr-underline-from-left hvr-fill-gray'>
        //                                <div class='menu-icon'><i class='fa fa-file-text-o'></i></div>
        //                                <div class='menu-name text-uppercase'>Register New Topic</div>
        //                            </a></li>
        //                            <li><a href='#' class='hvr-underline-from-left hvr-fill-gray'>
        //                                <div class='menu-icon'><i class='glyphicon glyphicon-export'></i></div>
        //                                <div class='menu-name text-uppercase'>Update Result</div>
        //                            </a></li>
        //                            <li><a href='#' class='hvr-underline-from-left hvr-fill-gray'>
        //                                <div class='menu-icon'><i class='glyphicon glyphicon-saved'></i></div>
        //                                <div class='menu-name text-uppercase'>Approval</div>
        //                            </a></li>
        //                            <li><a href='#' class='hvr-underline-from-left hvr-fill-gray'>
        //                                <div class='menu-icon'><i class='glyphicon glyphicon-search'></i></div>
        //                                <div class='menu-name text-uppercase'>Topic Search</div>
        //                            </a></li>
        //                            <li><a href='#' class='hvr-underline-from-left hvr-fill-gray'>
        //                                <div class='menu-icon'><i class='glyphicon glyphicon-stats'></i></div>
        //                                <div class='menu-name text-uppercase'>KPI</div>
        //                            </a></li>
        //                            <li><a href='admin_directimpact.aspx' class='hvr-underline-from-left hvr-fill-gray'>
        //                                <div class='menu-icon'><i class='glyphicon glyphicon-user'></i></div>
        //                                <div class='menu-name text-uppercase'>Administrator</div>
        //                            </a></li>
        //                        </ul>";

    }
    public int MenuID_Selected
    {
        get
        {
            int n = 0;
            int.TryParse(hidSelectedMenu.Value, out n);
            return n;
        }
        set { hidSelectedMenu.Value = value.ToString(); }
    }
    #region Class
    [Serializable]
    public class CResult : sysGlobalClass.CResutlWebMethod
    {
        public int nUserID { get; set; }
        public string sFullName { get; set; }
        public string sRoleName { get; set; }
        public List<UserAcc.TDataRole> TDataRole { get; set; }
    }
    #endregion
}
