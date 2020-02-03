using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Web.SessionState;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mail;
using System.Net;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Web.Configuration;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Text;
/// <summary>
/// Summary description for SystemFunction
/// </summary>
public class SystemFunction
{
    public SqlConnection _connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["PTTGC_EPIConnStr"].ToString());
    public static string strConnect = WebConfigurationManager.ConnectionStrings["PTTGC_EPIConnStr"].ToString();
    public static string GetDefaultPage = ConfigurationManager.AppSettings["DefaultPage"].ToString();
    public static string GetSystemMail = WebConfigurationManager.AppSettings["SystemMail"].ToString();
    public const string ASC = "asc";
    public const string DESC = "desc";
    public const string FolderUploadFile = "UploadFiles";
    public const string EncryptMode = "1";
    public const string SystemName = "PTTGC Enviromental Performance";
    public const string sAbbrSystem = "PTTGC EPI";


    public SystemFunction()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region All Msg Dialog
    public static string Msg_HeadInfo = "Information";//แจ้งผลการดำเนินการ
    public static string Msg_HeadError = "Error";
    public static string Msg_HeadConfirm = "Confirm";
    public static string Msg_HeadWarning = "Warning";

    public static string Msg_ConfirmDel = "Do you want to delete data ?";
    public static string Msg_ConfirmSave = "Do you want to save data ?";
    public static string Msg_AlertDel = "Please select data to delete.";
    public static string Msg_SaveComplete = "Already saved data.";
    public static string Msg_DelComplete = "Already deleted data.";
    public static string Msg_OverSize = "File upload max size {0}";
    public static string Msg_InvalidFileType = "Incorrect file type.";
    public static string Msg_Failed = "System Error !";
    public static string Msg_Login = "Please Login.";
    public static string Msg_LogonSharepathFailed = "Logon Share Path Failed !";
    public static string Msg_InvalidPOT = "ไม่สามารถบันทึกข้อมูลได้ เนื่องจาก Potential Number ไม่ตรงกับข้อมูล !";

    public static string process_SessionExpired = "SSEXP";
    public static string process_Success = "Success";
    public static string process_Failed = "Failed";
    public static string process_FileOversize = "OverSize";
    public static string process_FileInvalidType = "InvalidType";
    public static string process_LogonSharePathFailed = "LogonSPFailed";
    #endregion

    #region Dialog
    public static string PopupLogin()
    {
        return SystemFunction.DialogAlertLogin(SystemFunction.Msg_HeadInfo, SystemFunction.Msg_Login, SystemFunction.GetDefaultPage);
    }

    public static string DialogInfo(string head, string msg)
    {
        return "DialogInfo('" + head + "','" + msg + "')";
    }

    public static string DialogInfoRedirect(string head, string msg, string redirto)
    {
        return "DialogInfoRedirect('" + head + "','" + msg + "','" + redirto + "')";
    }

    public static string DialogError(string head, string msg)
    {
        return "DialogError('" + head + "','" + msg + "')";
    }

    public static string DialogErrorRedirect(string head, string msg, string redirto)
    {
        return "DialogErrorRedirect('" + head + "','" + msg + "','" + redirto + "')";
    }

    public static string DialogWarning(string head, string msg)
    {
        return "DialogWarning('" + head + "','" + msg + "')";
    }

    public static string DialogWarningRedirect(string head, string msg, string redirto)
    {
        return "DialogWarningRedirect('" + head + "','" + msg + "','" + redirto + "')";
    }

    public static string DialogConfirm(string head, string msg, string funcYes, string funcNo)
    {
        return "DialogConfirm('" + head + "','" + msg + "'," + funcYes + "," + funcNo + ")";
    }

    public static string DialogSuccess(string head, string msg)
    {
        return "DialogSuccess('" + head + "','" + msg + "')";
    }

    public static string DialogSuccessRedirect(string head, string msg, string redirto)
    {
        return "DialogSuccessRedirect('" + head + "','" + msg + "','" + redirto + "')";
    }

    public static string DialogAlertLogin(string head, string msg, string redirto)
    {
        return "DialogAlertLogin('" + head + "','" + msg + "','" + redirto + "')";
    }
    #endregion

    public string Ob2Json(object ob)
    {
        try
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = 2147483644 };
            string res = serializer.Serialize(ob);//new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ob);

            return res;
        }
        catch
        {
            return "";
        }
    }

    public static List<T> DataTableConvertToList<T>(DataTable datatable) where T : new()
    {
        List<T> Temp = new List<T>();
        try
        {
            List<string> columnsNames = new List<string>();
            foreach (DataColumn DataColumn in datatable.Columns)
                columnsNames.Add(DataColumn.ColumnName);
            Temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => getObject<T>(row, columnsNames));
            return Temp;
        }
        catch
        {
            return Temp;
        }

    }
    public static T getObject<T>(DataRow row, List<string> columnsName) where T : new()
    {
        T obj = new T();
        try
        {
            string columnname = "";
            string value = "";
            PropertyInfo[] Properties;
            Properties = typeof(T).GetProperties();
            foreach (PropertyInfo objProperty in Properties)
            {
                columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                if (!string.IsNullOrEmpty(columnname))
                {
                    value = row[columnname].ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                        {
                            value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                            objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                        }
                        else
                        {
                            value = row[columnname].ToString().Replace("%", "");
                            objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                        }
                    }
                }
            }
            return obj;
        }
        catch
        {
            return obj;
        }
    }

    public static bool CreateDirectory(string filepath)
    {
        bool cCheck = true;
        try
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("./") + filepath.Replace("/", "\\")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("./") + filepath.Replace("/", "\\"));
            }
        }
        catch
        {
            cCheck = false;
        }
        return cCheck;
    }

    public sysGlobalClass.CResutlWebMethod DeleteFileInServer(string sPath, string sFileName)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        HttpContext context = HttpContext.Current;
        if (File.Exists(context.Server.MapPath("./") + sPath.Replace("/", "\\") + sFileName))
        {
            File.Delete(context.Server.MapPath("./") + sPath.Replace("/", "\\") + sFileName);
        }
        result.Status = SystemFunction.process_Success;

        return result;
    }

    /// <summary>
    /// false = ส่งหาผู้ใช้งานจริง, true = ส่งหา RecieveDemoMail
    /// </summary>
    /// <returns></returns>
    public static bool IsDemoMail() { return WebConfigurationManager.AppSettings["IsDemoMail"] + "" == "N" ? false : true; }

    public static string GetRecieveDemoEmail() { return WebConfigurationManager.AppSettings["RecieveDemoMail"] + ""; }

    public static Workflow.DataMail_log SendMailAll(string sfrom, string sto, string scc, string sbcc, string subject, string message, string sfilepath)
    {
        Workflow.DataMail_log r = new Workflow.DataMail_log();

        if (ConfigurationSettings.AppSettings["cSendMail"].ToString() == "Y") // เพื่อตรวจสอบว่าจะให้ส่ง mail หรือไม่
        {
            try
            {

                string sToDemo = sto;
                string sCC = scc;

                string sConntentDemo = "<br /> sTo : " + sToDemo + "<br / > sCC : " + sCC;

                System.Net.Mail.MailMessage oMsg = new System.Net.Mail.MailMessage(); //MailMessage oMsg = new MailMessage();

                //Set Font
                string sSetFont = @"<style type='text/css'>
body{
            font-family:Angsana New;
            font-size:14pt;
    }
 </style>";
                if (!IsDemoMail())
                {

                    //From
                    //if (!string.IsNullOrEmpty(sfrom))
                    //{
                    //    oMsg.From = new System.Net.Mail.MailAddress(sfrom);
                    //}
                    //else
                    //{
                    //    oMsg.From = new System.Net.Mail.MailAddress(GetSystemMail);
                    //}
                    sfrom = GetSystemMail;
                    oMsg.From = new System.Net.Mail.MailAddress(sfrom);//Set from system email only

                    //To
                    if (!string.IsNullOrEmpty(sto))
                    {
                        string[] mailToList = sto.Replace(",", ";").Split(';');
                        foreach (string strM in mailToList)
                        {
                            if (strM != "") oMsg.To.Add(strM);
                        }
                    }

                    //CC
                    if (!string.IsNullOrEmpty(scc))
                    {
                        string[] mailToList = scc.Replace(",", ";").Split(';');
                        foreach (string strM in mailToList)
                        {
                            if (strM != "") oMsg.CC.Add(strM);
                        }
                    }

                    //BCC
                    if (!string.IsNullOrEmpty(sbcc))
                    {
                        string[] mailToList = sbcc.Replace(",", ";").Split(';');
                        foreach (string strM in mailToList)
                        {
                            if (strM != "") oMsg.Bcc.Add(strM);
                        }
                    }
                }
                else
                {
                    sfrom = ConfigurationSettings.AppSettings["SystemMail"] + "";

                    sto = ConfigurationSettings.AppSettings["RecieveDemoMail"] + "";

                    scc = ConfigurationSettings.AppSettings["RecieveDemoMail"] + "";

                    sbcc = ConfigurationSettings.AppSettings["RecieveDemoMail"] + "";

                    message += sConntentDemo;

                    //From
                    oMsg.From = new System.Net.Mail.MailAddress(sfrom);

                    //To
                    if (!string.IsNullOrEmpty(sto))
                    {
                        string[] mailToList = sto.Replace(",", ";").Split(';');
                        foreach (string strM in mailToList)
                        {
                            if (strM != "") oMsg.To.Add(strM);
                        }
                    }

                    //CC
                    if (!string.IsNullOrEmpty(scc))
                    {
                        string[] mailToList = scc.Replace(",", ";").Split(';');
                        foreach (string strM in mailToList)
                        {
                            if (strM != "") oMsg.CC.Add(strM);
                        }
                    }

                    //BCC
                    if (!string.IsNullOrEmpty(sbcc))
                    {
                        string[] mailToList = sbcc.Replace(",", ";").Split(';');
                        foreach (string strM in mailToList)
                        {
                            if (strM != "") oMsg.Bcc.Add(strM);
                        }
                    }
                }
                oMsg.Subject = subject;

                // SEND IN HTML FORMAT (comment this line to send plain text).
                oMsg.IsBodyHtml = true; //oMsg.BodyFormat = MailFormat.Html;

                // HTML Body (remove HTML tags for plain text).
                oMsg.Body = @"<HTML>" + sSetFont + "<BODY>" + message + "</BODY></HTML>";



                // ADD AN ATTACHMENT.
                //TODO: Replace with path to attachment.
                if (!sfilepath.Trim().Equals(""))
                {
                    //String sFile = @sfilepath;
                    //MailAttachment oAttch = new MailAttachment(sfilepath, MailEncoding.Base64);
                    //oMsg.Attachments.Add(oAttch);
                    System.Net.Mail.Attachment oAttch = new System.Net.Mail.Attachment(sfilepath);
                    oMsg.Attachments.Add(oAttch);
                }

                // TODO: Replace with the name of your remote SMTP server.
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Port = (ConfigurationSettings.AppSettings["portmail"] + "" != "") ? int.Parse(ConfigurationSettings.AppSettings["portmail"] + "") : 25;
                smtp.Host = ConfigurationSettings.AppSettings["smtpmail"]; //SmtpMail.SmtpServer = ConfigurationSettings.AppSettings["smtpmail"];
                smtp.Send(oMsg);//SmtpMail.Send(oMsg);

                oMsg.Attachments.Dispose();
                oMsg = null;

                r.bStatus = true;
                r.sCC = scc;
                r.sContent = subject + message;
                r.sFrom = sfrom;
                r.sMessage = "PASS";
                r.sTo = sto;

                //oAttch = null;
            }
            catch (Exception ex)
            {
                r.bStatus = false;
                r.sCC = scc;
                r.sContent = subject + message;
                r.sFrom = sfrom;
                r.sMessage = ex.Message.ToString();
                r.sTo = sto;
            }
        }
        else
        {
            r.bStatus = false;
            r.sCC = scc;
            r.sContent = subject + message;
            r.sFrom = sfrom;
            r.sMessage = "cSendMail == N";
            r.sTo = sto;
        }

        return r;
    }

    public static string ConvertExponentialToString(string sVal)
    {
        string sRsult = "";
        try
        {
            decimal nTemp = 0;
            bool check = Decimal.TryParse((sVal + "").Replace(",", ""), System.Globalization.NumberStyles.Float, null, out nTemp);
            if (check)
            {
                decimal d = Decimal.Parse((sVal + "").Replace(",", ""), System.Globalization.NumberStyles.Float);
                sRsult = (d + "").Replace(",", "");
            }
            else
            {
                sRsult = sVal;
            }
        }
        catch
        {
            sRsult = sVal;
        }

        return sRsult != null ? (sRsult.Length < 50 ? sRsult : sRsult.Remove(50)) : ""; //เพื่อไม่ให้ตอน Save Error หากค่าที่เกิดจากผลการคำนวนเกิน Type ใน DB (varchar(50))
    }

    public static decimal? GetDecimalNull(string sVal)
    {
        decimal? nTemp = null;
        decimal nCheck = 0;
        if (!string.IsNullOrEmpty(sVal))
        {
            sVal = ConvertExponentialToString(sVal);
            bool cCheck = decimal.TryParse(sVal, out nCheck);
            if (cCheck)
            {
                nTemp = decimal.Parse(sVal);
            }
        }

        return nTemp;
    }

    public static Double? GetDoubleNull(string sVal)
    {
        Double? nTemp = null;
        Double nCheck = 0;
        if (!string.IsNullOrEmpty(sVal))
        {
            sVal = ConvertExponentialToString(sVal);
            bool cCheck = Double.TryParse(sVal, out nCheck);
            if (cCheck)
            {
                nTemp = Double.Parse(sVal);
            }
        }

        return nTemp;
    }

    public static int? GetIntNull(string sVal)
    {
        int? nTemp = null;
        int nCheck = 0;
        if (!string.IsNullOrEmpty(sVal))
        {
            sVal = ConvertExponentialToString(sVal);
            bool cCheck = int.TryParse(sVal, out nCheck);
            if (cCheck)
            {
                nTemp = int.Parse(sVal);
            }
        }

        return nTemp;
    }

    public static int GetIntNullToZero(string sVal)
    {
        int nTemp = 0;
        int nCheck = 0;
        if (!string.IsNullOrEmpty(sVal))
        {
            sVal = ConvertExponentialToString(sVal);
            bool cCheck = int.TryParse(sVal, out nCheck);
            if (cCheck)
            {
                nTemp = int.Parse(sVal);
            }
        }

        return nTemp;
    }

    public static decimal GetNumberNullToZero(string sVal)
    {
        decimal nTemp = 0;
        sVal = ConvertExponentialToString(sVal);
        nTemp = decimal.TryParse(sVal, out nTemp) ? nTemp : 0;
        return nTemp;
    }

    public static int ParseInt(string sVal)
    {
        int nTemp = 0;
        nTemp = int.TryParse(sVal, out nTemp) ? nTemp : 0;
        return nTemp;
    }

    public static decimal ParseDecimal(string sVal)
    {
        decimal nTemp = 0;
        nTemp = decimal.TryParse(sVal, out nTemp) ? nTemp : 0;
        return nTemp;
    }

    public static bool IsNumberic(string sVal)
    {
        decimal nTemp = 0;
        sVal = ConvertExponentialToString(sVal);
        return decimal.TryParse(sVal, out nTemp);
    }

    public static DateTime ConvertStringToDateTime(string strDate, string strTime)
    {
        DateTime dTemp;
        bool checkDate = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp);
        if (!checkDate)
        {
            if (strTime.Length < 5)
            {
                dTemp = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? "0" + strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : DateTime.Now;
            }
        }
        else
        {
            dTemp = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : DateTime.Now;
        }

        return dTemp;
    }

    public static DateTime? ConvertStringToDateTimeNull(string strDate, string strTime)
    {
        DateTime dTemp;
        bool IsNull = false;
        bool checkDate = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp);
        if (!checkDate)
        {
            if (strTime.Length < 5)
            {
                if (!DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? "0" + strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp))
                    IsNull = true;
            }
        }
        else
        {
            if (!DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp))
                IsNull = true;
        }

        if (IsNull)
            return null;
        else
            return dTemp;
    }

    public static List<T> ConvertObject<T>(object objData)
    {
        List<T> Temp = new List<T>();
        if (objData != null)
        {
            Temp = (List<T>)objData;
        }
        return Temp;
    }

    public static DataTable ConvertObject(object objData)
    {
        DataTable dt = new DataTable();
        if (objData != null)
        {
            dt = (DataTable)objData;
        }
        return dt;
    }

    public static bool ConvertObjectBool(object objData)
    {
        bool cIs = false;
        if (objData != null)
        {
            try
            {
                cIs = (bool)objData;
            }
            catch
            { }
        }
        return cIs;
    }

    public static void ExecuteSQL(string strCon, SqlCommand cmd)
    {
        SqlConnection objConn = new SqlConnection(strCon);
        try
        {
            objConn.Open();
            cmd.Connection = objConn;
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objConn.Close();
        }
    }

    public static void ExecuteSQL(string strCon, string strSQL)
    {
        SqlConnection objConn = new SqlConnection(strCon);

        try
        {
            objConn.Open();
            SqlCommand cmd = new SqlCommand(strSQL, objConn);
            cmd.ExecuteNonQuery();
        }
        finally
        {
            objConn.Close();
        }
    }

    #region For set grid view to ajax
    public static sysGlobalClass.Pagination GetPagination(int nPageSize, int nPageIndex, int nCountAllData)
    {
        sysGlobalClass.Pagination data = new sysGlobalClass.Pagination();
        nPageIndex = nPageIndex == 0 ? 1 : nPageIndex;
        decimal nCalPage = nPageSize > 0 ? (nCountAllData / (decimal)nPageSize) : 0;
        int nPageCount = (int)Math.Ceiling(nCalPage);
        nPageIndex = nPageIndex > nPageCount ? nPageCount : nPageIndex;
        int nSkip = nPageSize * (nPageIndex - 1);
        bool IsNoData = (nSkip < 0);
        nSkip = nSkip < 0 ? 0 : nSkip;

        data.nPageCount = nPageCount;
        data.nSkipData = nSkip;
        data.nTakeData = nPageSize;
        data.nPageIndex = nPageIndex;

        int nCalto = (nSkip + nPageSize) <= nCountAllData ? (nSkip + nPageSize) : nCountAllData;
        data.sPageInfo = IsNoData ? " " : string.Format("Showing {0} to {1} ({2} item(s))", nSkip + 1, nCalto, nCountAllData); //"Showing {0} to {1} ({2} item(s))";
        data.sContentPageIndex = GenPagenationButton(nPageIndex, nPageCount, false);
        data.nStartItemIndex = nSkip + 1;
        return data;
    }

    public static sysGlobalClass.Pagination GetPaginationSmall(int nPageSize, int nPageIndex, int nCountAllData)
    {
        sysGlobalClass.Pagination data = new sysGlobalClass.Pagination();
        nPageIndex = nPageIndex == 0 ? 1 : nPageIndex;
        decimal nCalPage = nPageSize > 0 ? (nCountAllData / (decimal)nPageSize) : 0;
        int nPageCount = (int)Math.Ceiling(nCalPage);
        nPageIndex = nPageIndex > nPageCount ? nPageCount : nPageIndex;
        int nSkip = nPageSize * (nPageIndex - 1);
        bool IsNoData = (nSkip < 0);
        nSkip = nSkip < 0 ? 0 : nSkip;

        data.nPageCount = nPageCount;
        data.nSkipData = nSkip;
        data.nTakeData = nPageSize;
        data.nPageIndex = nPageIndex;

        int nCalto = (nSkip + nPageSize) <= nCountAllData ? (nSkip + nPageSize) : nCountAllData;
        data.sPageInfo = IsNoData ? " " : string.Format("Showing {0} to {1} ({2} item(s))", nSkip + 1, nCalto, nCountAllData); //"Showing {0} to {1} ({2} item(s))";
        data.sContentPageIndex = GenPagenationButton(nPageIndex, nPageCount, true);
        data.nStartItemIndex = nSkip + 1;
        return data;
    }

    public static string GenPagenationButton(int nActiveIndex, int nPageCont)
    {
        StringBuilder sb = new StringBuilder();

        string sDisPrev = nActiveIndex == 1 || nPageCont == 0 ? "disabled" : "";
        string sDisNext = nActiveIndex == nPageCont || nPageCont == 0 ? "disabled" : "";
        sb.Append("<ul class='pagination'>");
        sb.Append("<li class='paginate_button previous ccursor " + sDisPrev + "'><a data-dt-idx='-1'>Previous</a></li>");
        if (nPageCont > 6)//Max Button 6
        {
            int nStart = nActiveIndex > 3 ? nActiveIndex - 3 : nActiveIndex;
            int nEnd = (nActiveIndex + 3) < nPageCont ? (nActiveIndex + 3) : nPageCont;

            //Start
            if (nStart != 1)
            {
                sb.Append("<li class='paginate_button ccursor'><a data-dt-idx='" + 1 + "'>" + 1 + "</a></li>");
                if (nStart > 3)
                {
                    sb.Append("<li class='paginate_button disabled'><a data-dt-idx=''>...</a></li>");
                }
                else
                {
                    nStart = 2;
                }
            }

            //Loop
            for (int i = nStart; i <= nEnd; i++)
            {
                if (i == nActiveIndex)
                    sb.Append("<li class='paginate_button ccursor active'><a data-dt-idx='" + i + "'>" + i + "</a></li>");
                else
                    sb.Append("<li class='paginate_button ccursor'><a data-dt-idx='" + i + "'>" + i + "</a></li>");
            }

            //End
            if (nEnd != nPageCont)
            {
                sb.Append("<li class='paginate_button disabled'><a data-dt-idx=''>...</a></li>");
                sb.Append("<li class='paginate_button ccursor'><a data-dt-idx='" + nPageCont + "'>" + nPageCont + "</a></li>");
            }
        }
        else
        {
            for (int i = 1; i <= nPageCont; i++)
            {
                if (i == nActiveIndex)
                    sb.Append("<li class='paginate_button ccursor active'><a data-dt-idx='" + i + "'>" + i + "</a></li>");
                else
                    sb.Append("<li class='paginate_button ccursor'><a data-dt-idx='" + i + "'>" + i + "</a></li>");
            }
        }
        sb.Append("<li class='paginate_button next ccursor " + sDisNext + "'><a data-dt-idx='+1'>Next</a></li>");
        sb.Append("</ul>");

        return sb.ToString();
    }

    public static string GenPagenationButton(int nActiveIndex, int nPageCont, bool IsSmall)
    {
        StringBuilder sb = new StringBuilder();

        string sDisPrev = nActiveIndex == 1 || nPageCont == 0 ? "disabled" : "";
        string sDisNext = nActiveIndex == nPageCont || nPageCont == 0 ? "disabled" : "";
        string strClass = "";
        if (IsSmall)
            strClass = "small";
        sb.Append("<ul class='pagination " + strClass + "'>");
        sb.Append("<li class='paginate_button previous ccursor " + sDisPrev + "'><a data-dt-idx='-1'>Previous</a></li>");
        if (nPageCont > 6)//Max Button 6
        {
            int nStart = nActiveIndex > 3 ? nActiveIndex - 3 : nActiveIndex;
            int nEnd = (nActiveIndex + 3) < nPageCont ? (nActiveIndex + 3) : nPageCont;

            //Start
            if (nStart != 1)
            {
                sb.Append("<li class='paginate_button ccursor'><a data-dt-idx='" + 1 + "'>" + 1 + "</a></li>");
                if (nStart > 3)
                {
                    sb.Append("<li class='paginate_button disabled'><a data-dt-idx=''>...</a></li>");
                }
                else
                {
                    nStart = 2;
                }
            }

            //Loop
            for (int i = nStart; i <= nEnd; i++)
            {
                if (i == nActiveIndex)
                    sb.Append("<li class='paginate_button ccursor active'><a data-dt-idx='" + i + "'>" + i + "</a></li>");
                else
                    sb.Append("<li class='paginate_button ccursor'><a data-dt-idx='" + i + "'>" + i + "</a></li>");
            }

            //End
            if (nEnd != nPageCont)
            {
                sb.Append("<li class='paginate_button disabled'><a data-dt-idx=''>...</a></li>");
                sb.Append("<li class='paginate_button ccursor'><a data-dt-idx='" + nPageCont + "'>" + nPageCont + "</a></li>");
            }
        }
        else
        {
            for (int i = 1; i <= nPageCont; i++)
            {
                if (i == nActiveIndex)
                    sb.Append("<li class='paginate_button ccursor active'><a data-dt-idx='" + i + "'>" + i + "</a></li>");
                else
                    sb.Append("<li class='paginate_button ccursor'><a data-dt-idx='" + i + "'>" + i + "</a></li>");
            }
        }
        sb.Append("<li class='paginate_button next ccursor " + sDisNext + "'><a data-dt-idx='+1'>Next</a></li>");
        sb.Append("</ul>");

        return sb.ToString();
    }
    #endregion

    public static void ListYearsDESC(DropDownList _ddl, string _label, string _vculture, string _tculture, int _lyear_en)
    {
        string sTextCulture = ConfigurationSettings.AppSettings["sTextCulture"] + "";
        int nAddYear = ParseInt(ConfigurationSettings.AppSettings["AddYearDDL"] + "");
        if (sTextCulture != "")
        {
            _tculture = sTextCulture;
        }

        _ddl.Items.Clear();
        DateTime _time = new DateTime(_lyear_en, 1, 1);
        DateTime _time2 = DateTime.Now.AddYears(nAddYear);
        if (!_label.Equals("")) _ddl.Items.Add(new ListItem(_label, "0"));
        while (_time2 >= _time) //(_time.Year <= _time2.Year)
        {
            _ddl.Items.Add(new ListItem(_time2.ToString("yyyy", new CultureInfo(_tculture)), _time2.ToString("yyyy", new CultureInfo(_vculture))));
            //_time = _time.AddYears(+1);
            _time2 = _time2.AddYears(-1);
        }

        string sYearNow = DateTime.Now.ToString("yyyy", new CultureInfo(_vculture));
        ListItem itemCheck = _ddl.Items.FindByValue(sYearNow);
        if (itemCheck != null)
            _ddl.SelectedValue = sYearNow;
        else
            _ddl.SelectedIndex = 0;
    }

    public static void ListYearsDESC(DropDownList _ddl, string _label, string _lablevalue, string _vculture, string _tculture, int _lyear_en)
    {
        string sTextCulture = ConfigurationSettings.AppSettings["sTextCulture"] + "";
        int nAddYear = ParseInt(ConfigurationSettings.AppSettings["AddYearDDL"] + "");
        if (sTextCulture != "")
        {
            _tculture = sTextCulture;
        }

        _ddl.Items.Clear();
        DateTime _time = new DateTime(_lyear_en, 1, 1);
        DateTime _time2 = DateTime.Now.AddYears(nAddYear);
        if (!_label.Equals("")) _ddl.Items.Add(new ListItem(_label, _lablevalue));
        while (_time2 >= _time) //(_time.Year <= _time2.Year)
        {
            _ddl.Items.Add(new ListItem(_time2.ToString("yyyy", new CultureInfo(_tculture)), _time2.ToString("yyyy", new CultureInfo(_vculture))));
            //_time = _time.AddYears(+1);
            _time2 = _time2.AddYears(-1);
        }

        string sYearNow = DateTime.Now.ToString("yyyy", new CultureInfo(_vculture));
        ListItem itemCheck = _ddl.Items.FindByValue(sYearNow);
        if (itemCheck != null)
            _ddl.SelectedValue = sYearNow;
        else
            _ddl.SelectedIndex = 0;
    }

    public static void ListYearsASC(DropDownList _ddl, string _label, string _vculture, string _tculture, int _lyear_en)
    {
        _ddl.Items.Clear();
        DateTime _time = new DateTime(_lyear_en, 1, 1);
        DateTime _time2 = DateTime.Now.AddYears(1);
        if (!_label.Equals("")) _ddl.Items.Add(new ListItem(_label, "0"));
        while (_time.Year <= _time2.Year)
        {
            _ddl.Items.Add(new ListItem(_time.ToString("yyyy", new CultureInfo(_tculture)), _time.ToString("yyyy", new CultureInfo(_vculture))));
            _time = _time.AddYears(+1);
        }
        _ddl.SelectedValue = DateTime.Now.ToString("yyyy", new CultureInfo(_vculture));
    }

    /// <summary>
    /// Month EN, value >> 01,12
    /// </summary>
    /// <param name="_ddlmonth"></param>
    /// <param name="_label"></param>
    /// <param name="_labelvalue"></param>
    public static void ListMonthEN(DropDownList _ddlmonth, string _label, string _labelvalue)
    {
        _ddlmonth.Items.Clear();
        if (!_label.Equals("")) _ddlmonth.Items.Add(new ListItem(_label, _labelvalue));
        _ddlmonth.Items.Add(new ListItem("January", "01"));
        _ddlmonth.Items.Add(new ListItem("February", "02"));
        _ddlmonth.Items.Add(new ListItem("March", "03"));
        _ddlmonth.Items.Add(new ListItem("April", "04"));
        _ddlmonth.Items.Add(new ListItem("May", "05"));
        _ddlmonth.Items.Add(new ListItem("June", "06"));
        _ddlmonth.Items.Add(new ListItem("July", "07"));
        _ddlmonth.Items.Add(new ListItem("August", "08"));
        _ddlmonth.Items.Add(new ListItem("September", "09"));
        _ddlmonth.Items.Add(new ListItem("October", "10"));
        _ddlmonth.Items.Add(new ListItem("November", "11"));
        _ddlmonth.Items.Add(new ListItem("December", "12"));

        _ddlmonth.SelectedValue = DateTime.Now.ToString("MM");
    }

    /// <summary>
    /// Month EN, value >> 01,12
    /// </summary>
    /// <param name="_ddlmonth"></param>
    /// <param name="_label"></param>
    /// <param name="_labelvalue"></param>
    public static void ListMonthShortEN(DropDownList _ddlmonth, string _label, string _labelvalue)
    {
        _ddlmonth.Items.Clear();
        if (!_label.Equals("")) _ddlmonth.Items.Add(new ListItem(_label, _labelvalue));
        _ddlmonth.Items.Add(new ListItem("Jan", "01"));
        _ddlmonth.Items.Add(new ListItem("Feb", "02"));
        _ddlmonth.Items.Add(new ListItem("Mar", "03"));
        _ddlmonth.Items.Add(new ListItem("Apr", "04"));
        _ddlmonth.Items.Add(new ListItem("May", "05"));
        _ddlmonth.Items.Add(new ListItem("Jun", "06"));
        _ddlmonth.Items.Add(new ListItem("Jul", "07"));
        _ddlmonth.Items.Add(new ListItem("Aug", "08"));
        _ddlmonth.Items.Add(new ListItem("Sep", "09"));
        _ddlmonth.Items.Add(new ListItem("Oct", "10"));
        _ddlmonth.Items.Add(new ListItem("Nov", "11"));
        _ddlmonth.Items.Add(new ListItem("Dec", "12"));

        _ddlmonth.SelectedValue = DateTime.Now.ToString("MM");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ddl"></param>
    /// <param name="nSetPageSize">null default index 0</param>
    public static void BindDropdownPageSize(DropDownList ddl, int? nSetPageSize)
    {
        ddl.DataSource = SystemFunction.GetPageSize();
        ddl.DataValueField = "nPage";
        ddl.DataTextField = "nPage";
        ddl.DataBind();
        if (nSetPageSize != null)
            ddl.SelectedValue = nSetPageSize + "";
        else
            ddl.SelectedIndex = 0;
    }

    public static List<sysGlobalClass.TPageSize> GetPageSize()
    {
        List<sysGlobalClass.TPageSize> lstPageSize = new List<sysGlobalClass.TPageSize>();
        lstPageSize.Add(new sysGlobalClass.TPageSize { nPage = 10 });
        lstPageSize.Add(new sysGlobalClass.TPageSize { nPage = 20 });
        lstPageSize.Add(new sysGlobalClass.TPageSize { nPage = 50 });
        lstPageSize.Add(new sysGlobalClass.TPageSize { nPage = 100 });
        lstPageSize.Add(new sysGlobalClass.TPageSize { nPage = 200 });
        return lstPageSize;
    }

    /// <summary>
    /// ลบไฟล์ที่เคยอัพมาแล้วที่ยังค้างอยู่เกิน 2 วัน
    /// </summary>
    /// <param name="sPathServer">Server.MapPath(sSharePath + "/")</param>
    /// <param name="sPathFile">Path for delete file.</param>
    /// <param name="FileType">format : "pdf,xlsx,xls,doc,docx,pptx,ppt,jpg,gif,png,rar,zip"</param>
    public static void ClearFileInTemp(string sPathServer, string sPathFile, string FileType)
    {
        try
        {
            DateTime DateNow = DateTime.Now;

            DirectoryInfo d = new DirectoryInfo(sPathServer + sPathFile.Replace("/", "\\"));
            string sType = "";
            string[] ArrFileType = FileType.Split(',');
            foreach (string stype in ArrFileType)
            {
                sType = stype.Replace("*", "").Replace(".", "");
                FileInfo[] Files = d.GetFiles("*." + sType); //, SearchOption.AllDirectories

                foreach (FileInfo file in Files)
                {
                    DateTime dTimeFile = file.CreationTime;

                    if (dTimeFile.Date.AddDays(2) < DateNow.Date)
                    {
                        try
                        {
                            if (File.Exists(sPathServer + sPathFile.Replace("/", "\\") + file.Name))
                            {
                                File.Delete(sPathServer + sPathFile.Replace("/", "\\") + file.Name);
                            }
                        }
                        catch { }

                    }
                }
            }

        }
        catch { }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>pdf,xlsx,xls,doc,docx,pptx,ppt,jpg,gif,png,rar,zip</returns>
    public static string GetAllFileTypeForClearFileInTemp()
    {
        return "pdf,xlsx,xls,doc,docx,pptx,ppt,jpg,gif,png,rar,zip";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sPathServer">Server.MapPath(sSharePath + "/")</param>
    /// <param name="sPathFile">Path for get</param>
    /// <param name="FileType">if get all type set FileType = "" else set file type (1 type only)</param>
    /// <returns></returns>
    public static FileInfo[] GetFileOnDierectory(string sPathServer, string sPathFile, string FileType)
    {
        DirectoryInfo d = new DirectoryInfo(sPathServer + sPathFile.Replace("/", "\\"));
        if (d.Exists)//Directory.Exists(sPathServer + sPathFile.Replace("/", "\\")))
        {
            if (!string.IsNullOrEmpty(FileType))
            {
                FileInfo[] Files = d.GetFiles("*." + FileType);
                return Files;
            }
            else
            {
                FileInfo[] Files = d.GetFiles("*.*");
                return Files;
            }
        }
        else
        {
            FileInfo[] Files = null;
            return Files;
        }
    }

    public byte[] ReadFile(string filePath)
    {
        byte[] buffer;
        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        try
        {
            int length = (int)fileStream.Length;  // get file length
            buffer = new byte[length];            // create buffer
            int count;                            // actual number of bytes read
            int sum = 0;                          // total number of bytes read

            // read until Read method returns 0 (end of the stream has been reached)
            while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                sum += count;  // sum is a buffer offset for next reading
        }
        finally
        {
            fileStream.Close();
        }
        return buffer;
    }

    public static string LinkOpenFile(string pathFile, string _Filename, string NameFileShow, string sFuncDeleteFile)
    {
        string strEncrypt = "";
        string sReturn = "";
        if (SystemFunction.EncryptMode.Equals("1"))
        {
            strEncrypt = HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(pathFile + _Filename));
        }
        else
        {
            strEncrypt = pathFile + _Filename;
        }
        sReturn = "<a href='openFile.aspx?str=" + strEncrypt + "' target='_blank' class='btn btn-info'><i class='glyphicon glyphicon-search'></i>View</a> " + (sFuncDeleteFile + "" != "" ? "<button type='button' onclick='" + sFuncDeleteFile + "' class='btn btn-danger'><i class='glyphicon glyphicon-trash'></i>Delete</button>" : ""); //"<a href=openFile.aspx?str=" + strEncrypt + " target=_blank style=color:#666;text-decoration:none>" + NameFileShow + "<i class='glyphicon glyphicon-search'></i></a>";
        return sReturn;
    }

    public static string LinkOpenFile(string pathFile, string _Filename, string NameFileShow, string sFuncDeleteFile, string textView, string textDel)
    {
        string strEncrypt = "";
        string sReturn = "";
        if (SystemFunction.EncryptMode.Equals("1"))
        {
            strEncrypt = HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(pathFile + _Filename));
        }
        else
        {
            strEncrypt = pathFile + _Filename;
        }
        sReturn = "<a href='openFile.aspx?str=" + strEncrypt + "' target='_blank' class='btn btn-info' title='View'><i class='glyphicon glyphicon-search'></i>" + textView + "</a> <button type='button' onclick='" + sFuncDeleteFile + "' class='btn btn-danger' title='Delete'><i class='glyphicon glyphicon-trash'></i>" + textDel + "</button>";
        return sReturn;
    }

    public static string LinkOpenFile(string pathFile, string _Filename, string NameFileShow)
    {
        string strEncrypt = "";
        string sReturn = "";
        if (SystemFunction.EncryptMode.Equals("1"))
        {
            strEncrypt = HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(pathFile + _Filename));
        }
        else
        {
            strEncrypt = pathFile + _Filename;
        }
        sReturn = "<a href='openFile.aspx?str=" + strEncrypt + "' target='_blank'>" + NameFileShow + " <i class='glyphicon glyphicon-search'></i></a>";
        return sReturn;
    }

    public static string LinkOpenFile(string pathFile, string _Filename, string NameFileShow, string sFuncDeleteFile, string sTextShowBtnView)
    {
        string strEncrypt = "";
        string sReturn = "";
        if (SystemFunction.EncryptMode.Equals("1"))
        {
            strEncrypt = HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(pathFile + _Filename));
        }
        else
        {
            strEncrypt = pathFile + _Filename;
        }
        sReturn = "<a href='openFile.aspx?str=" + strEncrypt + "' target='_blank' class='btn btn-info'>" + sTextShowBtnView + " <i class='glyphicon glyphicon-search'></i>View</a> " + (sFuncDeleteFile + "" != "" ? "<button type='button' onclick='" + sFuncDeleteFile + "' class='btn btn-danger'><i class='glyphicon glyphicon-trash'></i>Delete</button>" : ""); //"<a href=openFile.aspx?str=" + strEncrypt + " target=_blank style=color:#666;text-decoration:none>" + NameFileShow + "<i class='glyphicon glyphicon-search'></i></a>";
        return sReturn;
    }

    public static string GetMassegeOnStatus(string sStatus)
    {
        string Msg = "";
        if (sStatus == SystemFunction.process_FileOversize)
        {
            Msg = SystemFunction.Msg_OverSize;
        }
        else if (sStatus == SystemFunction.process_FileInvalidType)
        {
            Msg = SystemFunction.Msg_InvalidFileType;
        }
        else if (sStatus == SystemFunction.process_LogonSharePathFailed)
        {
            Msg = SystemFunction.Msg_LogonSharepathFailed;
        }
        else
        {
            Msg = SystemFunction.Msg_Failed;
        }
        return Msg;
    }

    public static string RequestUrl()
    {
        #region Request Url Query

        string strHost = "";
        if (HttpContext.Current.Request.UrlReferrer.Query.Length > 0)
            strHost = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Replace(HttpContext.Current.Request.UrlReferrer.Query, "");
        else
            strHost = HttpContext.Current.Request.UrlReferrer.AbsoluteUri;
        return strHost = strHost.Replace(Path.GetFileName(HttpContext.Current.Request.UrlReferrer.PathAndQuery), "");

        #endregion
    }

    public static string GetFileNameFromFileupload(string _sFileName, string sBrowserType)
    {
        string sFileName = "";
        string[] arrName = (_sFileName + "").Split('.');
        sFileName = arrName[arrName.Length - 1];
        /*string[] arr = new string[] { "ie", "internetexplorer", "chrome", "chrome46" };
        if (arr.Contains(sBrowserType.ToLower()))
        {
            string[] arrName = (_sFileName + "").Split('\\');
            sFileName = arrName[arrName.Length - 1];
        }
        else
        {
            sFileName = _sFileName;
        }*/
        return sFileName;
    }

    public void addLogError(string Msg, string StackTrace)
    {
        if (ConfigurationSettings.AppSettings["AddErrorLog"].ToString() == "Y")
        {
            ///Paths File
            string strPathName = "./UploadFiles/ErrorLog/";

            #region Create Directory
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(strPathName.Replace("/", "\\"))))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(strPathName.Replace("/", "\\")));
            }
            #endregion
            ///Create a text file containing error details
            string strFileName = DateTime.Now.ToString("ddMMyyyy", new System.Globalization.CultureInfo("th-TH")) + ".txt";
            string errorText = "\r\nError Message: " + Msg + "\r\nStack Trace: " + StackTrace + "\r\n";
            string logMessage = String.Format(@"[{0}] :: {1} ", DateTime.Now.ToString("HH:mm:ss", new System.Globalization.CultureInfo("th-TH")), errorText);
            //System.IO.FileInfo file = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(sSharePath + "/" + strPathName + strFileName));
            //System.IO.StreamWriter _sw = file.AppendText();
            StreamWriter _sw = new StreamWriter(HttpContext.Current.Server.MapPath(strPathName + strFileName), true, Encoding.UTF8);
            _sw.Write(logMessage);
            _sw.Write(_sw.NewLine);
            _sw.WriteLine(("*").PadLeft(logMessage.Length, '*'));
            _sw.Write(_sw.NewLine);
            _sw.Close();
        }
    }

    /// <summary>
    /// Set Path Input
    /// </summary>
    /// <param name="nIndicator">Indicator  ที่ต้องการให้ Dropdown Set Value </param>
    /// <param name="nOperationType">OperationType  ที่ต้องการให้ Dropdown Set Value </param>
    /// <param name="sFacility">Facility ที่ต้องการให้ Dropdown Set Value </param>
    /// <param name="sYear">ปีที่ต้องการให้ Dropdown Set Value ให้ ถ้าส่งเข้ามาจะทำการ LoadData ในแต่ละหน้า</param>
    /// <param name="sStatus">สถานะจะส่งเข้ามาก็ต่อเมื่อต้องการ ให้มีปุ่ม Approve With Edit Content</param>
    public static string ReturnPath(int nIndicator, int nOperationType, string sFacility, string sYear, string sStatus)
    {
        string sPath = "#";
        string sPathSet = "";
        if (!string.IsNullOrEmpty(sFacility))
        {
            sPathSet = "&&fac=" + HttpUtility.UrlEncode(STCrypt.Encrypt(sFacility + ""));
        }
        if (!string.IsNullOrEmpty(sFacility))
        {
            sPathSet += "&&year=" + HttpUtility.UrlEncode(STCrypt.Encrypt(sYear + ""));
        }
        if (!string.IsNullOrEmpty(sFacility))
        {
            sPathSet += "&&status=" + HttpUtility.UrlEncode(STCrypt.Encrypt(sStatus + ""));
        }
        if (nIndicator == 6 && nOperationType == 11)
        {
            sPath = "epi_input_intensity_2.aspx?in=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nIndicator + "")) + "&&ot=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nOperationType + "")) + sPathSet;
        }
        else if (nIndicator == 10)
        {
            sPath = "epi_input_waste.aspx?in=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nIndicator + "")) + "&&ot=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nOperationType + "")) + sPathSet;
        }
        else if (nIndicator == 8)
        {
            sPath = "epi_input_materials.aspx?in=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nIndicator + "")) + "&&ot=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nOperationType + "")) + sPathSet;
        }
        else if (nIndicator == 6 && nOperationType == 4)
        {
            sPath = "epi_input_intensity_1.aspx?in=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nIndicator + "")) + "&&ot=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nOperationType + "")) + sPathSet;
        }
        else if (nIndicator == 6 && nOperationType == 13)
        {
            sPath = "epi_input_intensity_3.aspx?in=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nIndicator + "")) + "&&ot=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nOperationType + "")) + sPathSet;
        }
        else if (nIndicator == 6 && nOperationType == 14)
        {
            sPath = "epi_input_intensity_4.aspx?in=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nIndicator + "")) + "&&ot=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nOperationType + "")) + sPathSet;
        }
        else if (nIndicator == 2)
        {
            sPath = "epi_input_compliance.aspx?in=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nIndicator + "")) + "&&ot=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nOperationType + "")) + sPathSet;
        }
        else if (nIndicator == 1)
        {
            sPath = "epi_input_complaint.aspx?in=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nIndicator + "")) + "&&ot=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nOperationType + "")) + sPathSet;
        }
        else if (nIndicator == 11)
        {
            sPath = "epi_input_water.aspx?in=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nIndicator + "")) + "&&ot=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nOperationType + "")) + sPathSet;
        }
        else if (nIndicator == 3)
        {
            sPath = "epi_input_effluent.aspx?in=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nIndicator + "")) + "&&ot=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nOperationType + "")) + sPathSet;
        }
        else if (nIndicator == 4)
        {
            sPath = "epi_input_emission.aspx?in=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nIndicator + "")) + "&&ot=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nOperationType + "")) + sPathSet;
        }
        else if (nIndicator == 9)
        {
            sPath = "epi_input_spill.aspx?in=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nIndicator + "")) + "&&ot=" + HttpUtility.UrlEncode(STCrypt.Encrypt(nOperationType + "")) + sPathSet;
        }
        return sPath;
    }

    public static bool IsSuperAdmin()
    {
        bool IsAdmin = false;
        if (!UserAcc.UserExpired())
        {
            IsAdmin = UserAcc.GetObjUser().nUserID + "" == System.Configuration.ConfigurationManager.AppSettings["UserIDAdmin"];
        }
        return IsAdmin;
    }
    public static string ConvertFormatDecimal3(string sVal)
    {
        string sResult = "";
        if (IsNumberic(sVal))
        {
            string sValCheck = ConvertExponentialToString(sVal);
            sValCheck = sValCheck.Replace("-", "");
            sVal = ConvertExponentialToString(sVal);
            decimal nCheck = decimal.Parse(sValCheck); // แปลงเป็นค่าสัมบูรณ์
            decimal nTemp = decimal.Parse(sVal);
            if (nCheck > 0 && nCheck < 1)
                //sResult = nTemp.ToString("##0.00E+0");
                sResult = nTemp.ToString("0.000E+0");
            else
                sResult = nTemp.ToString("n3");
        }
        else if (sVal == null)
        {
            sResult = "";
        }
        else
        {
            sResult = sVal;
        }
        return sResult;
    }
    public static string ConverFormatDecimalDynamic(string sVal)
    {
        string sResult = "";
        if (!string.IsNullOrEmpty(sVal) && sVal != ConfigurationSettings.AppSettings["formaNA"].ToString())
        {
            sVal = ConvertExponentialToString(sVal);
            string[] arrDecimal = (sVal + "").Split('.');
            decimal nTemp = decimal.TryParse(sVal, out nTemp) ? nTemp : 0;
            if (arrDecimal.Length > 1)
            {
                int nDecimal = arrDecimal[arrDecimal.Length - 1].Length;
                sResult = nTemp.ToString("n" + nDecimal);
            }
            else
            {
                sResult = nTemp.ToString("n0");
            }
        }
        else
        {
            sResult = sVal;
        }
        return sResult;
    }
    public static bool UpFile2Server(string sFromPath, string sToPath, string sFromFileName, string sToFileName)
    {
        if (File.Exists(HttpContext.Current.Server.MapPath("./") + sFromPath + sFromFileName))
        {
            try
            {
                File.Move(HttpContext.Current.Server.MapPath("./") + sFromPath + sFromFileName, HttpContext.Current.Server.MapPath("./") + sToPath + sToFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Output by Joy
    /// </summary>
    public static string ConvertFormatDecimal4(string sVal)
    {
        string sResult = "";
        if (IsNumberic(sVal))
        {
            string sValCheck = ConvertExponentialToString(sVal);
            sValCheck = sValCheck.Replace("-", "");
            sVal = ConvertExponentialToString(sVal);
            decimal nCheck = decimal.Parse(sValCheck); // แปลงเป็นค่าสัมบูรณ์
            decimal nTemp = decimal.Parse(sVal);
            string[] str = sValCheck.Split('.');
            if (nCheck > 0 && nCheck < 1)
            {
                if (str.Length > 1)
                {
                    if (str[1].Length >= 4)
                    {
                        sResult = nTemp.ToString("0.000E+0");
                    }
                    else
                    {
                        sResult = nTemp.ToString("0.000");
                    }
                }
                else
                {
                    sResult = nTemp.ToString("n3");
                }
            }
            //else if (nCheck >= 1 || nCheck == 0)              
            else
            {
                sResult = nTemp.ToString("n3");
            }

        }
        else if (sVal == null)
        {
            sResult = "";
        }
        else
        {
            sResult = sVal;
        }
        return sResult;
    }
    public static decimal? ConvertBarrelToLiter(string sVal)
    {
        decimal? nReturn = null;
        if (!string.IsNullOrEmpty(sVal) && IsNumberic(sVal))
        {
            decimal? nVal = GetDecimalNull(sVal);
            nReturn = nVal * 158.9873m;
        }
        return nReturn;
    }
    public static decimal? ConvertM3ToLiter(string sVal)
    {
        decimal? nReturn = null;
        if (!string.IsNullOrEmpty(sVal) && IsNumberic(sVal))
        {
            decimal? nVal = GetDecimalNull(sVal);
            nReturn = nVal * 1000m;
        }
        return nReturn;
    }
    public List<int> GetMontStatus(int nStatus, int nIncID, int nOprtID, int nFacID, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<int> result = new List<int>();
        int nStatusID = 0;
        switch (nStatus)
        {
            case 1://Submit
                nStatusID = 1;
                break;
            case 2://Request Edit
                nStatusID = 4;
                break;
            case 24: //Recall
                nStatusID = 1;
                break;
            case 27: // Approve with edit content
                nStatusID = 2;
                break;
        }
        var item = db.TEPI_Forms.FirstOrDefault(w => w.IDIndicator == nIncID && w.OperationTypeID == nOprtID && w.FacilityID == nFacID && w.sYear == sYear);
        if (item != null)
        {
            var lstMonthWkf = db.TEPI_Workflow.Where(w => w.FormID == item.FormID && (nStatus == 1 ? (w.nStatusID == nStatusID || w.nStatusID == 2 || w.nStatusID == 4 || w.nStatusID == 5 || w.nStatusID == 27) : w.nStatusID == nStatusID)).ToList();
            lstMonthWkf.ForEach(f =>
            {
                result.Add(f.nMonth);
            });
        }
        return result;
    }
    public static int GetPermission_EPI_FROMS(int nIndicatior, int nFacility)
    {
        int nPermission = 0;
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            int nUser = UserAcc.GetObjUser().nUserID;
            int nRoleID = UserAcc.GetObjUser().nRoleID;
            var lstUser = db.mTUser_FacilityPermission.Where(a => a.nUserID == nUser && a.nRoleID == nRoleID && a.nGroupIndicatorID == nIndicatior && a.nFacilityID == nFacility).FirstOrDefault();
            var lstFlow = db.mTWorkFlow.Where(a => ((a.L1 == nUser && nRoleID == 3) || (a.L2 == nUser && nRoleID == 4)) && a.IDIndicator == nIndicatior && a.IDFac == nFacility).FirstOrDefault();
            if (lstUser != null)
            {
                nPermission = lstUser.nPermission ?? 0;
            }
            if (lstFlow != null)
            {
                if (nRoleID == 3)
                {
                    nPermission = 1;
                }
                if (nRoleID == 4)
                {
                    nPermission = 2;
                }
            }
            if (IsSuperAdmin())
            {
                nPermission = 2;
            }


        }

        return nPermission;
    }
    //public static int GetPermissionMenu(int nMenuID)
    //{
    //    int nPermission = 0;

    //    if (!UserAcc.UserExpired())
    //    {
    //        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
    //        int nUser = UserAcc.GetObjUser().nUserID;
    //        int nRoleID = UserAcc.GetObjUser().nRoleID;

    //        var lstMenu = db.TMenu.FirstOrDefault(w => w.nMenuID == nMenuID);
    //        if (lstMenu != null)
    //        {
    //            nPermission = 0;
    //            if (nMenuID == 1 || nMenuID == 2 || nMenuID == 3 || nMenuID == 4 || nMenuID == 5)
    //            {
    //                nPermission = 1;
    //            }

    //            if ((lstMenu.nMenuID == 6 || lstMenu.sMenuHeadID == 6) && nRoleID == 1)
    //            {
    //                if (lstMenu.nMenuID == 6 && db.TMenu_Permission.Any(w => w.nRoleID == nRoleID && w.nUserID == nUser && w.nPermission != 0) || lstMenu.sMenuHeadID == 6)
    //                {
    //                    nPermission = 1;
    //                    var lstAdmin = db.TMenu_Permission.FirstOrDefault(w => w.nRoleID == nRoleID && w.nUserID == nUser && w.nMenuID == nMenuID);
    //                    if (lstAdmin != null)
    //                    {
    //                        nPermission = lstAdmin.nPermission ?? 0;
    //                    }
    //                }
    //                else
    //                {
    //                    nPermission = 0;
    //                }

    //            }
    //            else
    //            {
    //                int nIndicatior = 0;
    //                switch (nMenuID)
    //                {
    //                    case 7:
    //                        nIndicatior = 6;
    //                        break;
    //                    case 8:
    //                        nIndicatior = 8;
    //                        break;
    //                    case 9:
    //                        nIndicatior = 11;
    //                        break;
    //                    case 10:
    //                        nIndicatior = 10;
    //                        break;
    //                    case 11:
    //                        nIndicatior = 1;
    //                        break;
    //                    case 12:
    //                        nIndicatior = 4;
    //                        break;
    //                    case 13:
    //                        nIndicatior = 9;
    //                        break;
    //                    case 14:
    //                        nIndicatior = 2;
    //                        break;
    //                    case 15:
    //                        nIndicatior = 1;
    //                        break;
    //                }
    //                var lstUser = db.mTUser_FacilityPermission.Where(a => a.nUserID == nUser && a.nRoleID == nRoleID && a.nGroupIndicatorID == nIndicatior).FirstOrDefault();
    //                var lstFlow = db.mTWorkFlow.Where(a => ((a.L1 == nUser && nRoleID == 3) || (a.L2 == nUser && nRoleID == 4)) && a.IDIndicator == nIndicatior).FirstOrDefault();
    //                if (lstUser != null)
    //                {
    //                    nPermission = lstUser.nPermission ?? 0;
    //                }
    //                if (lstFlow != null)
    //                {
    //                    if (nRoleID == 3)
    //                    {
    //                        nPermission = 1;
    //                    }
    //                    if (nRoleID == 4)
    //                    {
    //                        nPermission = 2;
    //                    }
    //                }
    //            }
    //        }
    //        if (IsSuperAdmin())
    //        {
    //            nPermission = 2;
    //        }

    //    }

    //    return nPermission;
    //}
    public static int GetPermissionMenu(int nMenuID)
    {
        int nPermission = 0;

        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            int nUser = UserAcc.GetObjUser().nUserID;
            int nRoleID = UserAcc.GetObjUser().nRoleID;

            var lstMenu = db.TMenu.FirstOrDefault(w => w.nMenuID == nMenuID);
            if (lstMenu != null)
            {
                nPermission = 0;
                if (nMenuID == 1 || nMenuID == 2 || nMenuID == 3 || nMenuID == 4 || nMenuID == 5)
                {
                    nPermission = 1;
                }

                if ((lstMenu.nMenuID == 6 || lstMenu.sMenuHeadID == 6) && (nRoleID == 1 || nRoleID == 6))
                {
                    nPermission = 0;
                    if (lstMenu.nMenuID == 6)
                    {
                        if (db.TMenu_Permission.Any(w => w.nRoleID == nRoleID && w.nUserID == nUser && w.nPermission != 0) || lstMenu.sMenuHeadID == 6)
                        {
                            nPermission = 2;
                        }
                    }
                    else
                    {
                        var lstAdmin = db.TMenu_Permission.FirstOrDefault(w => w.nRoleID == nRoleID && w.nUserID == nUser && w.nMenuID == nMenuID);
                        if (lstAdmin != null)
                        {
                            nPermission = lstAdmin.nPermission ?? 0;
                        }
                    }

                }
                else
                {
                    int nIndicatior = 0;
                    switch (nMenuID)
                    {
                        case 7:
                            nIndicatior = 6;
                            break;
                        case 8:
                            nIndicatior = 8;
                            break;
                        case 9:
                            nIndicatior = 11;
                            break;
                        case 10:
                            nIndicatior = 10;
                            break;
                        case 11:
                            nIndicatior = 1;
                            break;
                        case 12:
                            nIndicatior = 4;
                            break;
                        case 13:
                            nIndicatior = 9;
                            break;
                        case 14:
                            nIndicatior = 2;
                            break;
                        case 15:
                            nIndicatior = 1;
                            break;
                    }
                    var lstUser = db.mTUser_FacilityPermission.Where(a => a.nUserID == nUser && a.nRoleID == nRoleID && a.nGroupIndicatorID == nIndicatior).FirstOrDefault();
                    var lstFlow = db.mTWorkFlow.Where(a => ((a.L1 == nUser && nRoleID == 3) || (a.L2 == nUser && nRoleID == 4)) && a.IDIndicator == nIndicatior).FirstOrDefault();
                    if (lstUser != null)
                    {
                        nPermission = lstUser.nPermission ?? 0;
                    }
                    if (lstFlow != null)
                    {
                        if (nRoleID == 3)
                        {
                            nPermission = 1;
                        }
                        if (nRoleID == 4)
                        {
                            nPermission = 2;
                        }
                    }
                }
            }
            if (IsSuperAdmin())
            {
                nPermission = 2;
            }

        }

        return nPermission;
    }

    /// <summary>
    /// Blind Menu
    /// </summary>
    ///     ///BANK
    public static string HTML_Menubar(string Url)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        string sHTML = "";

        string sFormat = @"<li><a {5} id='{3}' href='{1}'><i class='{2}'></i>&nbsp;{0}</a>{4}</li>";

        string QUERY = @" SELECT nMenuID 'sMenuID' ,sMenuLink,sMenuName,sMenuHeadID,ISNULL(sMenuLink,'#') 'sUrl',sClassIcon FROM TMenu WHERE cActive='Y' AND sMenuHeadID='0' ORDER BY sMenuOrder ASC ";

        string sFormat2 = @"<li><a {4} id='{3}' href='{1}'><i class='{2}'></i>&nbsp;{0}</a></li>";


        var lstMenu = db.TMenu.Where(w => w.cActive == "Y").OrderBy(o => o.sMenuOrder).ToList();
        string sUrl = Url.ToLower();
        var CheckMenu = lstMenu.FirstOrDefault(w => (w.sMenuLink + "").ToLower() == sUrl);
        var lstHead = lstMenu.Where(w => w.nLevel == 0).ToList();

        string sActive = "", sActive2 = "", sHTML2 = "", HTML_User = "";
        int nActiveID = 0;
        lstHead.ForEach(f =>
        {
            string sFormat_UserMenu = "";
            int Prms = 0;
            sHTML2 = "";
            sActive = "";
            if (CheckMenu != null)
            {
                if (f.flage_Menu == "L")
                {
                    int? nHeadMenu = CheckMenu.sMenuHeadID;
                    if (CheckMenu.nLevel == 2) //Menu_Level2
                    {
                        var HeadLevel_2 = lstMenu.FirstOrDefault(w => w.nMenuID == CheckMenu.sMenuHeadID);
                        if (HeadLevel_2 != null)
                        {
                            nHeadMenu = HeadLevel_2.sMenuHeadID;
                        }
                    }

                    if (f.nMenuID == CheckMenu.nMenuID || f.nMenuID == nHeadMenu)
                    {
                        sActive = " class =\"active\"";
                        nActiveID = f.nMenuID;
                    }
                    if (f.nMenuID == 6)
                    {
                        sHTML2 = "<ul class='menu-sub'>";
                        sFormat = @"<li class='has-children'><a {5} id='{3}' href='{1}'><i class='{2}'></i>&nbsp;{0}</a>{4}</li>";
                        lstMenu.Where(w => w.sMenuHeadID == f.nMenuID).ToList().ForEach(f2 =>
                        {
                            sActive2 = "";
                            if (CheckMenu != null)
                            {
                                if (f2.nMenuID == CheckMenu.nMenuID || f2.nMenuID == CheckMenu.sMenuHeadID)
                                {
                                    sActive2 = " class =\"active\"";
                                    nActiveID = f2.nMenuID;
                                }
                            }

                            Prms = SystemFunction.GetPermissionMenu(f2.nMenuID);
                            if (Prms != 0)
                            {
                                sHTML2 += string.Format(sFormat2, f2.sMenuName.Trim(), f2.sMenuLink, (!string.IsNullOrEmpty(f2.sClassIcon) ? f2.sClassIcon.Trim() : ""), "SubHead_" + f2.nMenuID, sActive2);
                            }
                        });
                        sHTML2 += "</ul>";
                    }
                    Prms = SystemFunction.GetPermissionMenu(f.nMenuID);
                    if (Prms != 0)
                    {
                        sHTML += string.Format(sFormat, f.sMenuName.Trim(), f.sMenuLink, (!string.IsNullOrEmpty(f.sClassIcon) ? f.sClassIcon.Trim() : ""), "Head_" + f.nMenuID, sHTML2, sActive);
                    }
                    else if ((UserAcc.GetObjUser().nRoleID == 4 || UserAcc.GetObjUser().nRoleID == 1) && f.nMenuID == 101)//ENVI Corporate (L2) && System Admin >> Transfer to PTT
                    {
                        sHTML += string.Format(sFormat, f.sMenuName.Trim(), f.sMenuLink, (!string.IsNullOrEmpty(f.sClassIcon) ? f.sClassIcon.Trim() : ""), "Head_" + f.nMenuID, sHTML2, sActive);
                    }
                }
                if (f.flage_Menu == "R")
                {
                    int? nHeadMenu = CheckMenu.sMenuHeadID;
                    if (CheckMenu.nLevel == 2) //Menu_Level2
                    {
                        var HeadLevel_2 = lstMenu.FirstOrDefault(w => w.nMenuID == CheckMenu.sMenuHeadID);
                        if (HeadLevel_2 != null)
                        {
                            nHeadMenu = HeadLevel_2.sMenuHeadID;
                        }
                    }

                    if (f.nMenuID == CheckMenu.nMenuID || f.nMenuID == nHeadMenu)
                    {
                        sActive = " class =\"active\"";
                    }

                    if (f.nMenuID == 81)
                    {
                        sFormat_UserMenu = @"<li><a {4} id='{3}' href='{1}'><i class='{2}'></i>&nbsp;{0}</a></li>";
                        HTML_User += string.Format(sFormat_UserMenu, f.sMenuName.Trim(), f.sMenuLink, (!string.IsNullOrEmpty(f.sClassIcon) ? f.sClassIcon.Trim() : ""), "SubHead_" + f.nMenuID, sActive);
                    }
                    else if (f.nMenuID == 82) // Change_Role
                    {
                        var DataRole = UserAcc.GetRolePermission(UserAcc.GetObjUser().nUserID + "");
                        if (DataRole.Count > 0 && DataRole.Count != 1)
                        {
                            sFormat_UserMenu = @"<li><a {3} style='cursor: pointer;' onclick='PopDetailRole()' id='{2}'><i class='{1}'></i>&nbsp;{0}</a></li>";
                            HTML_User += string.Format(sFormat_UserMenu, f.sMenuName.Trim(), (!string.IsNullOrEmpty(f.sClassIcon) ? f.sClassIcon.Trim() : ""), "SubHead_" + f.nMenuID, sActive);
                        }
                    }
                    else if (f.nMenuID == 83) //Logout
                    {
                        sFormat_UserMenu = @"<li><a {3} id='{2}'><i class='{1}'></i>&nbsp;{0}</a></li>";
                        HTML_User += string.Format(sFormat_UserMenu, f.sMenuName.Trim(), (!string.IsNullOrEmpty(f.sClassIcon) ? f.sClassIcon.Trim() : ""), "SubHead_" + f.nMenuID, " onclick='Logout()' style='cursor:pointer' ");
                    }
                }

            }
        });
        var me = db.TMenu.FirstOrDefault(w => w.sMenuLink.ToLower() == Url.ToLower());
        if(me != null)
        {
            nActiveID = me.nMenuID;
        }
        if (nActiveID != 0)
        {

            if (!UserAcc.UserExpired())
            {
                var obj = UserAcc.GetObjUser();
                var log = new TViewMenu_History();

                log.nUserID = obj.nUserID;
                log.nMenuID = nActiveID;
                log.dAction = DateTime.Now;
                db.TViewMenu_History.Add(log);
                db.SaveChanges();
            }

        }
        return "<ul class='menu' id='menuBar'>" + sHTML + "</ul>" + "<ul class='menu menu-user'>" + HTML_User + "</ul>";

    }
    public static string HTML_Navtab(string Url)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        string sHTML = "";
        string sFormat = @"<li><a {5} id='{3}' href='{1}'><i class='{2}'></i>&nbsp;{0}</a>{4}</li>";

        string QUERY = @" SELECT nMenuID 'sMenuID' ,sMenuLink,sMenuName,sMenuHeadID,ISNULL(sMenuLink,'#') 'sUrl',sClassIcon FROM TMenu WHERE cActive='Y' AND nLevel='0' ORDER BY sMenuOrder ASC ";
        var lst_Menu = db.TMenu.Where(w => w.cActive == "Y" && w.sMenuLink.ToLower() == Url.ToLower()).ToList();
        //string _SQL = @" SELECT nMenuID 'sMenuID' ,sMenuLink,sMenuName,sMenuHeadID,ISNULL(sMenuLink,'#') 'sUrl',sClassIcon FROM TMenu WHERE cActive='Y' ORDER BY sMenuOrder ASC ";

        string sFormat2 = @"<li><a {4} id='{3}' href='{1}'><i class='{2}'></i>&nbsp;{0}</a></li>";
        //string Active = (Url.ToLower() == dt.Rows[i]["sMenuLink"].ToString().ToLower() + "") ? "Class=\"active\"" : "";
        var lstMenu = db.TMenu.Where(w => w.cActive == "Y").OrderBy(o => o.sMenuOrder).ToList();
        string sUrl = Url.ToLower();
        var CheckMenu = lstMenu.FirstOrDefault(w => (w.sMenuLink + "").ToLower() == sUrl);
        var lstHead = lstMenu.Where(w => w.nLevel == 0).ToList();
        string sActive = "", sActive2 = "", sHTML2 = "", HTML_User = "";
        lstHead.ForEach(f =>
        {
            string sFormat_UserMenu = "";
            int Prms = 0;
            sActive = "";
            if (f.flage_Menu == "L")
            {

                if (CheckMenu != null)
                {
                    int? nHeadMenu = CheckMenu.sMenuHeadID;
                    if (CheckMenu.nLevel == 2) //Menu_Level2
                    {
                        var HeadLevel_2 = lstMenu.FirstOrDefault(w => w.nMenuID == CheckMenu.sMenuHeadID);
                        if (HeadLevel_2 != null)
                        {
                            nHeadMenu = HeadLevel_2.sMenuHeadID;
                        }
                    }

                    if (f.nMenuID == CheckMenu.nMenuID || f.nMenuID == nHeadMenu)
                    {
                        sActive = " class =\"active\"";
                    }
                }
                if (f.nMenuID == 6)
                {
                    sFormat = @"<li class='has-children'><a {5} id='{3}' href='{1}'>
                            <i class='{2}'></i>&nbsp;{0}<div class='link-caret'>
                            <i class='fa fa-chevron-right'></i></div></a>{4}</li>";

                    sHTML2 = "<ul class='menu-sub'><li><a style='cursor: pointer;' class='link-back'><i class='fa fa-chevron-left'></i>&nbsp;Back</a></li>";
                    lstMenu.Where(w => w.sMenuHeadID == f.nMenuID).ToList().ForEach(f2 =>
                    {
                        sActive2 = "";
                        if (CheckMenu != null)
                        {
                            if (f2.nMenuID == CheckMenu.nMenuID || f2.nMenuID == CheckMenu.sMenuHeadID)
                            {
                                sActive2 = " class =\"active\"";
                            }
                        }
                        Prms = SystemFunction.GetPermissionMenu(f2.nMenuID);
                        if (Prms != 0)
                        {
                            sHTML2 += string.Format(sFormat2, f2.sMenuName.Trim(), f2.sMenuLink, (!string.IsNullOrEmpty(f2.sClassIcon) ? f2.sClassIcon.Trim() : ""), "SubHead_" + f2.nMenuID, sActive2);
                        }
                    });
                    sHTML2 += "</ul>";
                }
                else
                {
                    sHTML2 = "";
                    sFormat = @"<li><a {5} id='{3}' href='{1}'><i class='{2}'></i>&nbsp;{0}</a>{4}</li>";
                }
                Prms = SystemFunction.GetPermissionMenu(f.nMenuID);
                if (Prms != 0)
                {
                    sHTML += string.Format(sFormat, f.sMenuName.Trim(), f.sMenuLink, (!string.IsNullOrEmpty(f.sClassIcon) ? f.sClassIcon.Trim() : ""), "Head_" + f.nMenuID, sHTML2, sActive);
                }
                else if ((UserAcc.GetObjUser().nRoleID == 4 || UserAcc.GetObjUser().nRoleID == 1) && f.nMenuID == 101)//ENVI Corporate (L2) && System Admin >> Transfer to PTT
                {
                    sHTML += string.Format(sFormat, f.sMenuName.Trim(), f.sMenuLink, (!string.IsNullOrEmpty(f.sClassIcon) ? f.sClassIcon.Trim() : ""), "Head_" + f.nMenuID, sHTML2, sActive);
                }
            }
        });
        return "<ul class='menu' id='navtab'>" + sHTML + "</ul>" + "<ul class='menu menu-user'>" + HTML_User + "</ul>";

    }
    public static string HTML_NavtabUser(string Url)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var lst_Menu = db.TMenu.Where(w => w.cActive == "Y" && w.sMenuLink.ToLower() == Url.ToLower()).ToList();
        //string _SQL = @" SELECT nMenuID 'sMenuID' ,sMenuLink,sMenuName,sMenuHeadID,ISNULL(sMenuLink,'#') 'sUrl',sClassIcon FROM TMenu WHERE cActive='Y' ORDER BY sMenuOrder ASC ";

        //string Active = (Url.ToLower() == dt.Rows[i]["sMenuLink"].ToString().ToLower() + "") ? "Class=\"active\"" : "";
        var lstMenu = db.TMenu.Where(w => w.cActive == "Y").OrderBy(o => o.sMenuOrder).ToList();
        string sUrl = Url.ToLower();
        var CheckMenu = lstMenu.FirstOrDefault(w => (w.sMenuLink + "").ToLower() == sUrl);
        var lstHead = lstMenu.Where(w => w.nLevel == 0).ToList();
        string sActive = "", HTML_User = "";
        lstHead.ForEach(f =>
        {
            string sFormat_UserMenu = "";
            sActive = "";
            if (f.flage_Menu == "R")
            {
                if (CheckMenu != null)
                {
                    int? nHeadMenu = CheckMenu.sMenuHeadID;
                    if (CheckMenu.nLevel == 2) //Menu_Level2
                    {
                        var HeadLevel_2 = lstMenu.FirstOrDefault(w => w.nMenuID == CheckMenu.sMenuHeadID);
                        if (HeadLevel_2 != null)
                        {
                            nHeadMenu = HeadLevel_2.sMenuHeadID;
                        }
                    }

                    if (f.nMenuID == CheckMenu.nMenuID || f.nMenuID == nHeadMenu)
                    {
                        sActive = " class =\"active\"";
                    }
                }


                if (f.nMenuID == 81)
                {
                    sFormat_UserMenu = @"<li><a {4} id='{3}' href='{1}'><i class='{2}'></i>&nbsp;{0}</a></li>";
                    HTML_User += string.Format(sFormat_UserMenu, f.sMenuName.Trim(), f.sMenuLink, (!string.IsNullOrEmpty(f.sClassIcon) ? f.sClassIcon.Trim() : ""), "SubHead_" + f.nMenuID, sActive);
                }
                else if (f.nMenuID == 82) // Change_Role
                {
                    var DataRole = UserAcc.GetRolePermission(UserAcc.GetObjUser().nUserID + "");
                    if (DataRole.Count > 0 && DataRole.Count != 1)
                    {
                        sFormat_UserMenu = @"<li><a {3} style='cursor: pointer;' onclick='PopDetailRole()' id='{2}'><i class='{1}'></i>&nbsp;{0}</a></li>";
                        HTML_User += string.Format(sFormat_UserMenu, f.sMenuName.Trim(), (!string.IsNullOrEmpty(f.sClassIcon) ? f.sClassIcon.Trim() : ""), "SubHead_" + f.nMenuID, sActive);
                    }
                }
                else if (f.nMenuID == 83) //Logout
                {
                    sFormat_UserMenu = @"<li><a {3} id='{2}'><i class='{1}'></i>&nbsp;{0}</a></li>";
                    HTML_User += string.Format(sFormat_UserMenu, f.sMenuName.Trim(), (!string.IsNullOrEmpty(f.sClassIcon) ? f.sClassIcon.Trim() : ""), "SubHead_" + f.nMenuID, " onclick='Logout()' style='cursor:pointer' ");
                }
            }
        });
        return "<ul class='menu menu-user'>" + HTML_User + "</ul>";

    }
    public static string GetURL(string Url)
    {
        string[] arrUrl = Url.ToLower().Replace("~/", "").Split('-');
        return arrUrl.Length == 1 ? arrUrl[0] : arrUrl[0] + (arrUrl[1].Substring(4)); //กรณีเข้าหน้า Edit ให้ตั้งชื่อ Url เป็น xxxxx-edit.aspx 
    }

    public static List<sysGlobalClass.TDeviate> GetDeviate(int nIncID, int nOprtID, int nFacID, string sYear)
    {
        List<sysGlobalClass.TDeviate> lst = new List<sysGlobalClass.TDeviate>();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int nFormID = 0;
        int nYearPrevious = 0;

        var lstForms = db.TEPI_Forms.Where(w => w.IDIndicator == nIncID && w.OperationTypeID == nOprtID && w.FacilityID == nFacID).ToList();
        if (lstForms.Count > 0)
        {
            var qFormsNow = lstForms.FirstOrDefault(w => w.sYear == sYear);
            if (qFormsNow != null)
            {
                nFormID = qFormsNow.FormID;
            }
        }
        string sql = @"select wfd.FormID
                                       ,CASE
		                                WHEN wfd.Month = 1 THEN 'Jan'WHEN wfd.Month = 2 THEN 'Feb'WHEN wfd.Month = 3 THEN 'Mar'WHEN wfd.Month = 4 THEN 'Apr'
		                                WHEN wfd.Month = 5 THEN 'May'WHEN wfd.Month = 6 THEN 'Jun'WHEN wfd.Month = 7 THEN 'Jul'WHEN wfd.Month = 8 THEN 'Aug'
		                                WHEN wfd.Month = 9 THEN 'Sep'WHEN wfd.Month = 10 THEN 'Oct'WHEN wfd.Month = 11 THEN 'Nov'WHEN wfd.Month = 12 THEN 'Dec'
		                                ELSE '-'
	                                   END	 as sMonth
                                      ,wfd.Month as nMonth                                      
                                      --,wfd.sRemark      
                                      --,wfd.dAction	                                 
	                                  --,ISNULL(u.Firstname,'') +' '+ ISNULL(u.Lastname,'-') as sActionBy
                       from TEPI_Forms_Deviate wfd
                       left join mTProductIndicator p on p.ProductID = wfd.nProductID
                       left join mTUser u on u.ID = wfd.nActionBy
                       where wfd.FormID = " + nFormID + @"
                       group by wfd.FormID,wfd.Month--,wfd.sRemark,wfd.dAction,u.Firstname,u.Lastname
                       order by wfd.Month ";
        DataTable dt = CommonFunction.Get_Data(strConnect, sql);
        lst = CommonFunction.ConvertDatableToList<sysGlobalClass.TDeviate>(dt).ToList();

        var lstDataDeviate = db.TEPI_Forms_Deviate.Where(w => w.FormID == nFormID).ToList();
        var lstUesr = db.mTUser.Where(w => w.cDel != "Y").ToList();
        foreach (var i in lst)
        {
            var Data = lstDataDeviate.OrderByDescending(o => o.dAction).FirstOrDefault(w => w.Month == i.nMonth);
            var dataUser = lstUesr.FirstOrDefault(w => w.ID == Data.nActionBy);
            if (Data != null)
            {
                i.sDate = Data.dAction.Value.ToString("dd/MM/yyyy HH:mm:ss");
                i.sRemark = Data.sRemark;
                i.sActionBy = dataUser != null ? dataUser.Firstname + " " + dataUser.Lastname : "-";
            }
            else
            {
                i.sDate = "";
                i.sRemark = "";
                i.sActionBy = "";
            }
        }

        return lst;
    }
    public static List<sysGlobalClass.T_Facility> Get_SubFacility(int OperationID, int nUserID, int nRoleID)
    {
        List<sysGlobalClass.T_Facility> result = new List<sysGlobalClass.T_Facility>();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<sysGlobalClass.T_Facility> lstFacility = new List<sysGlobalClass.T_Facility>();

        var lb_1 = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 1 && w.OperationTypeID == OperationID).Select(s => new { s.CompanyID, s.cDel, s.ID }).Distinct().ToList();
        var lb_2 = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 2 && w.OperationTypeID == OperationID).ToList();
        //var lb_3 = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 1 && w.OperationTypeID == OperationID).Select(s => new { s.CompanyID, s.cDel }).Distinct().ToList();
        var lb_3 = db.mTUser_FacilityPermission.Where(w => w.nUserID == nUserID && w.nRoleID == nRoleID).Select(s => new { s.nFacilityID }).Distinct().ToList();
        if (nRoleID == 6)
        {
            lstFacility = (from a in lb_1.Where(w => w.cDel == "N")
                           from b in lb_2.Where(w => w.nHeaderID == a.ID)
                               //from c in lb_3.
                           from d in lb_3.Where(w => w.nFacilityID == b.ID)
                           select new sysGlobalClass.T_Facility
                           {
                               nFacilityID = b.ID,
                               sFacilityName = b.Name,
                               nOperationTypeID = b.OperationTypeID
                           }).ToList();
        }
        else
        {
            lstFacility = (from a in lb_1
                           from b in lb_2.Where(w => w.nHeaderID == a.ID)
                           select new sysGlobalClass.T_Facility
                           {
                               nFacilityID = b.ID,
                               sFacilityName = b.Name,
                               nOperationTypeID = b.OperationTypeID
                           }).ToList();
        }

        result = lstFacility.Distinct().OrderBy(o => o.sFacilityName).ToList();
        return result;
    }
    public static List<sysGlobalClass.T_Facility> Get_SubFacility_ByMuti(List<int> lst, int nUserID, int nRoleID)
    {
        List<sysGlobalClass.T_Facility> result = new List<sysGlobalClass.T_Facility>();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<sysGlobalClass.T_Facility> lstFacility = new List<sysGlobalClass.T_Facility>();

        if (nRoleID == 6)
        {
            foreach (var item in lst)
            {
                int nOperationID = item;
                var lb_1 = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 1 && w.OperationTypeID == nOperationID).ToList();
                var lb_2 = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 2 && w.OperationTypeID == nOperationID).ToList();
                var lb_3 = db.mTUser_FacilityPermission.Where(w => w.nUserID == nUserID && w.nRoleID == nRoleID).Select(s => new { s.nFacilityID }).Distinct().ToList();
                var lstData = (from a in lb_1
                               from b in lb_2.Where(w => w.nHeaderID == a.ID)
                               from c in lb_3.Where(w => w.nFacilityID == b.ID)
                               select new sysGlobalClass.T_Facility
                               {
                                   nFacilityID = b.ID,
                                   sFacilityName = b.Name,
                                   nOperationTypeID = b.OperationTypeID
                               }).ToList();
                lstFacility.AddRange(lstData);
            }
        }
        else
        {
            foreach (var item in lst)
            {
                int nOperationID = item;
                var lstData = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 2 && w.OperationTypeID == nOperationID).Select(s => new sysGlobalClass.T_Facility
                {
                    nFacilityID = s.ID,
                    sFacilityName = s.Name,
                    nOperationTypeID = s.OperationTypeID,
                }).ToList();
                lstFacility.AddRange(lstData);
            }
        }

        result = lstFacility.Distinct().OrderBy(o => o.sFacilityName).ToList();
        return result;
    }

    public static DataTable Get_Data(string _sql)
    {
        DataTable _dt = new DataTable();
        if (!string.IsNullOrEmpty(_sql))
        {
            using (SqlConnection _conn = new SqlConnection(strConnect))
            {

                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }

                SqlCommand com = new SqlCommand(_sql, _conn);
                com.CommandTimeout = 6000;
                new SqlDataAdapter(com).Fill(_dt);

                return _dt;
            }
        }
        else
        {
            return _dt;
        }
    }
    public static void ListDBToDropDownList(DropDownList _ddl, string _sql, string _label, string _datavalue, string _datatext)
    {
        DataTable _dt = Get_Data(_sql);
        if (_dt.Rows.Count > 0)
        {
            _ddl.Items.Clear();
            _ddl.DataSource = _dt;
            _ddl.DataValueField = _datavalue;
            _ddl.DataTextField = _datatext;
            _ddl.DataBind();
            if (!_label.Equals("")) _ddl.Items.Insert(0, new ListItem(_label, ""));
        }
        else
        {
            _ddl.Items.Clear();
            if (!_label.Equals("")) _ddl.Items.Insert(0, new ListItem(_label, ""));
        }
    }
    public static decimal? sysDivideData(string sVal1, string sVal2)
    {
        decimal? nCal = null;
        if (!string.IsNullOrEmpty(sVal1) && !string.IsNullOrEmpty(sVal2))
        {
            decimal? nVal1 = GetDecimalNull(sVal1);
            decimal? nVal2 = GetDecimalNull(sVal2);
            if (nVal2 != null && nVal2.Value != 0)
            {
                nCal = nVal1 / nVal2;
            }
        }
        return nCal;
    }
    public static decimal? SumDataToDecimal(List<string> lstData)
    {
        decimal? nTotal = null;
        bool cNullAll = true;
        decimal nSum = 0;

        foreach (string sval in lstData)
        {
            if (SystemFunction.IsNumberic(sval + ""))
            {
                cNullAll = false;
                nSum = nSum + SystemFunction.GetNumberNullToZero(sval + "");
            }
        }

        if (!cNullAll)
        {
            nTotal = nSum;
        }

        return nTotal;
    }

    public static bool SaveXML_EPIResult(ENVIService.ENVIClass_Result data)
    {
        try
        {
            string sPath = "UploadFiles/ENVIServiceXML/" + DateTime.Now.ToString("dd-MM-yyyy") + "/";
            SystemFunction.CreateDirectory(sPath);
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(ENVIService.ENVIClass_Result));
            string sFileName = "ENVIResult_Data-" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".xml";
            var path = HttpContext.Current.Server.MapPath("./" + sPath + sFileName);
            System.IO.FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, data);
            file.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool SaveXML_UpdateStatus(ENVIService.ENVIClass_ENVI dataEPI)
    {
        try
        {
            string sPath = "UploadFiles/ENVIServiceXML/" + DateTime.Now.ToString("dd-MM-yyyy") + "/";
            SystemFunction.CreateDirectory(sPath);
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(ENVIService.ENVIClass_ENVI));
            string sFileName = "UpdateStatus_Data-" + DateTime.Now.ToString("ddMMyyHHmmssff") + ".xml";
            var path = HttpContext.Current.Server.MapPath("./" + sPath + sFileName);
            System.IO.FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, dataEPI);
            file.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public static string CovertDateEn2Th(string sDate, string Mode)
    {
        string result = "";
        DateTime dtTemp;
        DateTime dDate;
        switch (Mode)
        {
            case "TH":
                dDate = DateTime.TryParse(ReplaceInjection(sDate), out dtTemp) ? dtTemp : dtTemp;
                result = dDate.ToString("dd/MM/yyyy", new CultureInfo("th-th"));
                break;
            case "EN":
                dDate = DateTime.TryParse(ReplaceInjection(sDate), out dtTemp) ? dtTemp : dtTemp;
                result = dtTemp.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
                break;
            case "EN2":
                dDate = DateTime.TryParse(ReplaceInjection(sDate), out dtTemp) ? dtTemp : dtTemp;
                result = dtTemp.ToString("dd/MM/yyyy", new CultureInfo("en-US"));
                break;
            case "EN3":
                dDate = DateTime.TryParse(ReplaceInjection(sDate), out dtTemp) ? dtTemp : dtTemp;
                result = dtTemp.ToString("dd MMMM yyyy", new CultureInfo("en-US"));
                break;
            case "EN4":
                dDate = DateTime.TryParse(ReplaceInjection(sDate), out dtTemp) ? dtTemp : dtTemp;
                result = dtTemp.ToString("dd/MM/yyyy HH:mm", new CultureInfo("en-US"));
                break;
            case "TH2":
                dDate = DateTime.TryParse(ReplaceInjection(sDate), out dtTemp) ? dtTemp : dtTemp;
                result = dDate.ToString("D", new CultureInfo("th-th"));
                break;
            case "TH3":
                dDate = DateTime.TryParse(ReplaceInjection(sDate), out dtTemp) ? dtTemp : dtTemp;
                result = dDate.ToString("dd/MM/yyyy HH:mm", new CultureInfo("th-th"));
                break;
            case "TH4":
                dDate = DateTime.TryParse(ReplaceInjection(sDate), out dtTemp) ? dtTemp : dtTemp;
                result = dDate.ToString("dd MMM yyyy", new CultureInfo("th-th"));
                break;
        }

        return result;
    }

    public static string ReplaceInjection(string str)
    {//"'",
        string[] _blacklist = new string[] { "\\", "\"", "*/", "--", "<script", "/*", "</script>" };
        string strRep = str;
        if (strRep == null || strRep.Trim().Equals(String.Empty))
            return strRep;
        foreach (string _blk in _blacklist) { strRep = strRep.Replace(_blk, ""); }

        return strRep;
    }
}