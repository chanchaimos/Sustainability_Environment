﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class _MP_AllSource : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region Prevent unusable PostBack on IE11
        {
            string sUA = Request.UserAgent;
            int nVersion = Request.Browser.MajorVersion;
            if (sUA.Contains("like Gecko") && sUA.Contains("Trident") && nVersion == 0) Page.ClientTarget = "uplevel";
        }
        #endregion

        #region Prevent death-loop WebMethod when securiy scanning
        {
            var lstLink = Request.RawUrl.Split('/').ToList();
            var arrUrl = Request.Url;
            var sLink = lstLink.FirstOrDefault(w => w.Contains(".aspx"));

            if (sLink != null)
            {
                int nSubPath = lstLink.IndexOf(sLink);
                if (nSubPath < lstLink.Count - 1)
                {
                    var lstLinkUse = new List<string>();
                    for (int i = 0; i < nSubPath + 1; i++) lstLinkUse.Add(lstLink[i]);

                    if (lstLink.Count > nSubPath)
                    {
                        string sUrl = arrUrl.OriginalString.Replace(arrUrl.PathAndQuery, "") + "" + string.Join("/", lstLinkUse);
                        Response.Redirect(sUrl);
                    }
                }
            }
        }
        #endregion

        if (!IsPostBack)
        {
            Page.Title = System.Configuration.ConfigurationManager.AppSettings["PageTitle"];
        }
    }

    public HtmlGenericControl Body { get { return mainBody; } }

    public void SetBodyEventOnLoad(string sJsFunction)
    {
        if (!sJsFunction.EndsWith(";")) sJsFunction += ";";
        Body.Attributes.Add("onload", sJsFunction + "$('#" + Body.ClientID + "').removeAttr('onload');");
    }
}
