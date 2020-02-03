using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class epi_input_intensity_4 : System.Web.UI.Page
{
    private const string sFolderInSharePahtTemp = "UploadFiles/intensity_1/Temp/";
    private const string sFolderInPathSave = "UploadFiles/intensity_4/File/{0}/";
    private const int nIndicator = 6;
    private const int nOperationType = 14;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.TRetunrLoadData LoadData(string sFacility, string sYear)
    {
        sysGlobalClass.TRetunrLoadData o = new sysGlobalClass.TRetunrLoadData();
        Intensity_Function.IntensityClass ic = new Intensity_Function.IntensityClass();
        ic.rt = o;
        ic.nIndicator = nIndicator;
        ic.nOperationType = nOperationType;
        ic.nFacility = SystemFunction.GetIntNullToZero(sFacility);
        ic.sYear = sYear;
        ic.sFolderInPathSave = sFolderInPathSave;
        ic.lstMonth = new List<int>();
        o = Intensity_Function.GetLoadData(ic);
        return o;
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.TRetunrLoadData CreateLinkExport(string sFacility, string sYear)
    {
        sysGlobalClass.TRetunrLoadData o = new sysGlobalClass.TRetunrLoadData();
        Intensity_Function.IntensityClass ic = new Intensity_Function.IntensityClass();
        ic.rt = o;
        ic.nIndicator = nIndicator;
        ic.nOperationType = nOperationType;
        ic.nFacility = SystemFunction.GetIntNullToZero(sFacility);
        ic.sYear = sYear;
        ic.sFolderInPathSave = sFolderInPathSave;
        ic.lstMonth = new List<int>();
        o = Intensity_Function.CreateLinkExport(ic);
        return o;
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public static sysGlobalClass.TRetunrLoadData SaveData(sysGlobalClass.TRetunrLoadData lst, string sFacility, string sYear)
    {
        sysGlobalClass.TRetunrLoadData o = new sysGlobalClass.TRetunrLoadData();
        Intensity_Function.IntensityClass ic = new Intensity_Function.IntensityClass();
        ic.rt = lst;
        ic.nIndicator = nIndicator;
        ic.nOperationType = nOperationType;
        ic.nFacility = SystemFunction.GetIntNullToZero(sFacility);
        ic.sYear = sYear;
        ic.sFolderInPathSave = sFolderInPathSave;
        ic.lstMonth = lst.lstMonth;
        o = Intensity_Function.SaveData(ic);
        return o;
    }
}