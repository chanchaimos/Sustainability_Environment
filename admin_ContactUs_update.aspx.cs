using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_ContactUs_update : System.Web.UI.Page
{
    private const string sFolderInSharePahtTemp = "UploadFiles/ContactUs/Temp/";
    private const string sFolderInPathSave = "UploadFiles/ContactUs/File/{0}/";
    private void SetBodyEventOnLoad(string myFunc)
    {
        ((_MP_Front)this.Master).SetBodyEventOnLoad(myFunc);
    }
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
                string str = Request.QueryString["strid"];
                if (!string.IsNullOrEmpty(str))
                {
                    hidEncyptID.Value = str;
                    hidnID.Value = STCrypt.Decrypt(str);
                    SETDATA(hidnID.Value);
                }
            }
        }
    }

    private void SETDATA(string sID)
    {
        if (!string.IsNullOrEmpty(sID))
        {
            int nID = SystemFunction.GetIntNullToZero(sID);
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            DateTime now = DateTime.Now;
            var Query = db.TContactUs.FirstOrDefault(w => w.cDel == "N" && w.nContactID == nID);
            if (Query != null)
            {
                if (Query.cStatusAns == null)
                {
                    Query.cStatusAns = "1";
                    Query.dUpdate = now;
                    db.SaveChanges();
                }
                if (Query.cStatusAns == "2")
                {
                    txtDesc.Text = Query.sAnswer;
                    txtDesc.Attributes.Add("disabled", "true");
                    DivFileContactUS.Visible = false;
                    // lbUrlFileAdmin.Text = Query.sAnsPath + Query.sAnsFile;
                    if (!string.IsNullOrEmpty(Query.sAnsFile))
                    {
                        lbUrlFileAdmin.Text = "<a class='btn btn-primary'href=" + Query.sAnsPath + Query.sAnsSysFile + " target='_blank'><i class='fa fa-search'></i>&nbsp;" + Query.sAnsFile + "</a>";
                    }
                    else
                    {
                        lbUrlFileAdmin.Text = "-";
                    }


                }
                else
                {
                    DivShowFileAdmin.Visible = false;
                }
                hidsStatus.Value = Query.cStatusAns;
                lbName.Text = Query.sContactName;
                lbEmail.Text = Query.sContactEmail;
                lbSubject.Text = Query.sSubject;
                lbTel.Text = Query.sContactTel;
                lbUserDes.Text = Query.sDetail;
                lbAddDate.Text = Query.dCreate.ToString();
                //lbUrlFileUser.Text = Query.sContactPath + Query.sContactFile;
                if (!string.IsNullOrEmpty(Query.sContactFile))
                {
                    lbUrlFileUser.Text = "<a class='btn btn-primary'href=" + Query.sContactPath + Query.sContactSysFile + " target='_blank'><i class='fa fa-search'></i>&nbsp;" + Query.sContactFile + "</a>";
                }
                else
                {
                    lbUrlFileUser.Text = "-";
                }

                lbStatus.Text = Query.cStatusAns == null ? "Wait" : Query.cStatusAns == "1" ? "Read" : "Success";
            }
        }
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.CResutlWebMethod SaveToDB(string sDesc, string sID, List<dataFileContactUs> objFile)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        DateTime now = DateTime.Now;
        if (UserAcc.UserExpired())
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        else
        {
            try
            {
                UserAcc ua = UserAcc.GetObjUser();
                if (!string.IsNullOrEmpty(sID))
                {
                    string sPathSend = "";
                    int nID = int.Parse(STCrypt.Decrypt(sID));
                    var Query = db.TContactUs.FirstOrDefault(w => w.cDel == "N" && w.nContactID == nID);
                    if (Query != null)
                    {
                        Query.sAnswer = sDesc;
                        Query.cStatusAns = "2";
                        Query.dAnswer = now;
                        Query.dUpdate = now;
                        Query.nAnswerBy = UserAcc.GetObjUser().nUserID;

                        if (objFile.Count > 0 && objFile != null)
                        {
                            string sPathSave = string.Format(sFolderInPathSave, nID);
                            SystemFunction.CreateDirectory(sPathSave);
                            //string sPathSave = string.Format(sFolderInPathSave, nID);
                            //SystemFunction.CreateDirectory(sPathSave);
                            var f = objFile.First();
                            string sSystemFileName = nID + "_" + "-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "." + SystemFunction.GetFileNameFromFileupload(f.sFileName, ""); //SystemFunction2.GetFileType(item.SaveToFileName);
                            SystemFunction.UpFile2Server(f.sPath, sPathSave, f.sSysFileName, sSystemFileName);

                            Query.sAnsFile = f.sFileName;
                            Query.sAnsSysFile = sSystemFileName;
                            Query.sAnsPath = sPathSave;
                            sPathSend = HttpContext.Current.Server.MapPath("./") + sPathSave + sSystemFileName;
                        }

                        var Ispass = db.SaveChanges() > 0;
                        if (Ispass)
                        {
                            string To = Query.sContactEmail; // Contact Mail
                            string AdminContactmail = WebConfigurationManager.AppSettings["ContactMail"] + ""; // Mail Admin
                            //string To1 = "chotika.n@softthai.com";

                            string sSubject = SystemFunction.sAbbrSystem + " : Contact Us." + Query.sSubject;
                            string sGurl = SystemFunction.RequestUrl() + WebConfigurationManager.AppSettings["DefaultPage"] + "";
                            string sHtml = @"
                                    <table align='left'  border='0' cellspacing='2' cellpadding='3'>

                                        <tr>
                                            <td align='left'>Dear: " + Query.sContactName + @"</td>
                                        </tr>
                                         <tr>
                                           <td align='left'>Subject: " + Query.sSubject + @"</td>
                                        </tr>
                                         <tr>
                                           <td align='left'>Description: " + Query.sDetail + @"</td>
                                        </tr>
                                        <tr>
                                           <td align='left'>Answer: " + Query.sAnswer + @"</td>
                                        </tr>

                                    </table>";
                            Workflow.DataMail_log log = new Workflow.DataMail_log();

                            string ccMail = "";
                            var lstUser = db.mTUser.FirstOrDefault(w => w.ID == ua.nUserID);
                            if (lstUser != null)
                            {
                                ccMail = lstUser.Email;
                            }

                            log = SystemFunction.SendMailAll(AdminContactmail, To, ccMail, "", sSubject, sHtml, sPathSend != "" ? sPathSend.Replace("/", "\\") : "");
                            log.nDataID = SystemFunction.GetIntNullToZero(nID + "");
                            log.sPageName = "admin_ContactUs_update.aspx";
                            new Workflow().SaveLogMail(log);
                            if (log.bStatus)
                            {
                                result.Status = SystemFunction.process_Success;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                result.Msg = e.Message;
                result.Status = SystemFunction.process_Failed;
            }

            result.Status = SystemFunction.process_Success;
        }

        return result;
    }

    #region Class
    public class CSave_Data
    {
        public string sName { get; set; }
        public string sEmail { get; set; }
        public string sSubject { get; set; }
        public string sDec { get; set; }
        public string sTel { get; set; }
    }
    [Serializable]
    public class dataFileContactUs
    {
        public int nID { get; set; }
        public string sPath { get; set; }
        public string sSysFileName { get; set; }
        public string sFileName { get; set; }
    }
    #endregion
}