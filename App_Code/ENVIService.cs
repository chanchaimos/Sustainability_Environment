using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for ENVIService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class ENVIService : System.Web.Services.WebService
{
    PTTGC_EPIEntities db = new PTTGC_EPIEntities();
    public ENVIService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    #region Class
    public class EPIClass_DataType
    {
        public class CompnayCode
        {
            public static string PTT = "10";
        }

        public class InidcatorCode
        {
            public const string Complaint = "001";
            public const string Compliance = "002";
            public const string Effluent = "003";
            public const string Emission = "004";
            public const string IntensityDenominator = "006";
            public const string Material = "008";
            public const string Spill = "009";
            public const string Waste = "010";
            public const string Water = "011";
        }

    }

    public class ENVIClass_Authentication
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ENVIClass_ENVI : ENVIClass_Authentication
    {
        /// <summary>
        /// Required
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public string FacilityCode { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Required
        /// </summary>
        public string IndicatorCode { get; set; }
        /// <summary>
        /// Required: 1-4 Only
        /// </summary>
        public int Quarter { get; set; }
        public string Comment { get; set; }
    }

    public class ENVIClass_Result
    {
        public bool IsCompleted { get; set; }
        public string Message { get; set; }
        public List<EPIClass_SubResult> lstSubResult { get; set; }
    }

    public class EPIClass_SubResult
    {
        public string FacilityCode { get; set; }
        /// <summary>
        /// S=Success, E=Error
        /// </summary>
        public string Status { get; set; }
        public string Massage { get; set; }
    }
    #endregion

    [WebMethod]
    public ENVIClass_Result ApproveAPI(ENVIClass_ENVI dataENVI)
    {
        ENVIClass_Result result = new ENVIClass_Result();
        if (new ENVIService().Login(dataENVI.Username, dataENVI.Password, "ALL"))
        {
            if (!string.IsNullOrEmpty(dataENVI.CompanyCode) && !string.IsNullOrEmpty(dataENVI.FacilityCode) && !string.IsNullOrEmpty(dataENVI.IndicatorCode) && dataENVI.Year > 0 && dataENVI.Quarter > 0)
            {
                API_ENVI.ENVIWorkFlow.ENVIWorkFlow API = new API_ENVI.ENVIWorkFlow.ENVIWorkFlow();
                result = API.Workflow_PTTApproveL3(dataENVI);
                SystemFunction.SaveXML_UpdateStatus(dataENVI);
                SystemFunction.SaveXML_EPIResult(result);
            }
            else
            {
                result.IsCompleted = false;
                result.Message = "Invalid Required Parameter: CompanyCode, FacilityCode, IndicatorCode, Year, Quarter";
            }
        }
        else
        {
            result.IsCompleted = false;
            result.Message = "Login Failed";
        }
        return result;
    }

    [WebMethod]
    public ENVIClass_Result RejectAPI(ENVIClass_ENVI dataENVI)
    {
        ENVIClass_Result result = new ENVIClass_Result();
        if (new ENVIService().Login(dataENVI.Username, dataENVI.Password, "ALL"))
        {
            if (!string.IsNullOrEmpty(dataENVI.CompanyCode) && !string.IsNullOrEmpty(dataENVI.FacilityCode) && !string.IsNullOrEmpty(dataENVI.IndicatorCode) && dataENVI.Year > 0 && dataENVI.Quarter > 0)
            {
                API_ENVI.ENVIWorkFlow.ENVIWorkFlow API = new API_ENVI.ENVIWorkFlow.ENVIWorkFlow();
                result = API.Workflow_PTTReject(dataENVI);
                SystemFunction.SaveXML_UpdateStatus(dataENVI);
                SystemFunction.SaveXML_EPIResult(result);
            }
            else
            {
                result.IsCompleted = false;
                result.Message = "Invalid Required Parameter: CompanyCode, FacilityCode, IndicatorCode, Year, Quarter";
            }
        }
        else
        {
            result.IsCompleted = false;
            result.Message = "Login Failed";
        }
        return result;
    }

    [WebMethod]
    public ENVIClass_Result AcceptEditRequestAPI(ENVIClass_ENVI dataENVI)
    {
        ENVIClass_Result result = new ENVIClass_Result();
        if (new ENVIService().Login(dataENVI.Username, dataENVI.Password, "ALL"))
        {
            if (!string.IsNullOrEmpty(dataENVI.CompanyCode) && !string.IsNullOrEmpty(dataENVI.FacilityCode) && !string.IsNullOrEmpty(dataENVI.IndicatorCode) && dataENVI.Year > 0 && dataENVI.Quarter > 0)
            {
                API_ENVI.ENVIWorkFlow.ENVIWorkFlow API = new API_ENVI.ENVIWorkFlow.ENVIWorkFlow();
                result = API.Workflow_PTTAcceptEditRequest(dataENVI);
                SystemFunction.SaveXML_UpdateStatus(dataENVI);
                SystemFunction.SaveXML_EPIResult(result);
            }
            else
            {
                result.IsCompleted = false;
                result.Message = "Invalid Required Parameter: CompanyCode, FacilityCode, IndicatorCode, Year, Quarter";
            }
        }
        else
        {
            result.IsCompleted = false;
            result.Message = "Login Failed";
        }
        return result;
    }

    [WebMethod]
    public ENVIClass_Result AcceptResetWFAPI(ENVIClass_ENVI dataENVI)
    {
        ENVIClass_Result result = new ENVIClass_Result();
        if (new ENVIService().Login(dataENVI.Username, dataENVI.Password, "ALL"))
        {
            if (!string.IsNullOrEmpty(dataENVI.CompanyCode) && !string.IsNullOrEmpty(dataENVI.FacilityCode) && !string.IsNullOrEmpty(dataENVI.IndicatorCode) && dataENVI.Year > 0 && dataENVI.Quarter > 0)
            {
                API_ENVI.ENVIWorkFlow.ENVIWorkFlow API = new API_ENVI.ENVIWorkFlow.ENVIWorkFlow();
                result = API.Workflow_PTTClearWF(dataENVI);
                SystemFunction.SaveXML_UpdateStatus(dataENVI);
                SystemFunction.SaveXML_EPIResult(result);
            }
            else
            {
                result.IsCompleted = false;
                result.Message = "Invalid Required Parameter: CompanyCode, FacilityCode, IndicatorCode, Year, Quarter";
            }
        }
        else
        {
            result.IsCompleted = false;
            result.Message = "Login Failed";
        }
        return result;
    }

    [WebMethod]
    public ENVIClass_Result CloseWFAPI(ENVIClass_ENVI dataENVI)
    {
        ENVIClass_Result result = new ENVIClass_Result();
        if (new ENVIService().Login(dataENVI.Username, dataENVI.Password, "ALL"))
        {
            if (!string.IsNullOrEmpty(dataENVI.CompanyCode) && !string.IsNullOrEmpty(dataENVI.FacilityCode) && !string.IsNullOrEmpty(dataENVI.IndicatorCode) && dataENVI.Year > 0 && dataENVI.Quarter > 0)
            {
                API_ENVI.ENVIWorkFlow.ENVIWorkFlow API = new API_ENVI.ENVIWorkFlow.ENVIWorkFlow();
                result = API.Workflow_PTTApproveL4(dataENVI);
                SystemFunction.SaveXML_UpdateStatus(dataENVI);
                SystemFunction.SaveXML_EPIResult(result);
            }
            else
            {
                result.IsCompleted = false;
                result.Message = "Invalid Required Parameter: CompanyCode, FacilityCode, IndicatorCode, Year, Quarter";
            }
        }
        else
        {
            result.IsCompleted = false;
            result.Message = "Login Failed";
        }
        return result;
    }

    private bool Login(string Username, string Password, string sType)
    {
        var query = db.TAPI_User.FirstOrDefault(w => w.sType == sType && w.sUsername == Username && w.sPassword == Password);
        return (query != null);
    }
}
