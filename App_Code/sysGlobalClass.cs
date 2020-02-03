using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for sysGlobalClass
/// </summary>
public class sysGlobalClass
{
    public sysGlobalClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public class TPageSize
    {
        public int nPage { get; set; }
    }

    public class CResutlWebMethod
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public string Content { get; set; }
    }

    /// <summary>
    /// Pagination : CResutlWebMethod
    /// </summary>
    public class Pagination : CResutlWebMethod
    {
        public int nPageCount { get; set; }
        public int nSkipData { get; set; }
        public int nTakeData { get; set; }
        public int nPageIndex { get; set; }
        public string sPageInfo { get; set; }
        public int nStartItemIndex { get; set; }
        public string sContentPageIndex { get; set; }
    }

    public class CommonLoadData
    {
        public string sIndexCol { get; set; }
        public string sOrderBy { get; set; }
        public string sPageSize { get; set; }
        public string sPageIndex { get; set; }
        public string sMode { get; set; }
    }

    public class TDataFile
    {
        public int nFileID { get; set; }
        public string sFileName { get; set; }
        public string sSystemName { get; set; }
        public string sPath { get; set; }
        public string sAjaxFileID { get; set; }
        public bool IsTempFile { get; set; }

        public string ALinkFile { get; set; }
        public int? nVersion { get; set; }
    }

    public class TBindDropdown
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class TDataStandard
    {
        public string label { get; set; }
        public List<TSourceMultiSelect> children { get; set; }
    }

    public class TSourceMultiSelect
    {
        public string label { get; set; }
        public string value { get; set; }
        public bool selected { get; set; }
    }

    public class FuncFileUpload
    {
        public class ItemData
        {
            public int ID { get; set; }
            public string SaveToFileName { get; set; }
            public string FileName { get; set; }
            public string SaveToPath { get; set; }
            /// <summary>
            /// for open file
            /// </summary>
            public string url { get; set; }
            public string sDirectoryEncrypt { get; set; }
            public bool IsNewFile { get; set; }
            public bool IsCompleted { get; set; }
            public bool IsNewChoose { get; set; }
            public string sMsg { get; set; }
            public string sDelete { get; set; }

            public string sDescription { get; set; }
        }
    }

    public class CGetStatusMonth : CResutlWebMethod
    {
        public List<int> lstMonth { get; set; }
    }
    #region epi_input_intensity_2

    [Serializable]
    public class TRetunrLoadData
    {
        public List<TData_Intensity> lstIn { get; set; }
        public string sMsg { get; set; }
        public string sStatus { get; set; }
        public int nStatusWF { get; set; }
        public List<sysGlobalClass.FuncFileUpload.ItemData> lstDataFile { get; set; }
        public string sPath { get; set; }
        public List<T_TEPI_Workflow> lstwf { get; set; }
        public List<int> lstMonth { get; set; }
        public int hdfPRMS { get; set; }
        public string sComment { get; set; }


    }
    [Serializable]
    public class TData_Intensity
    {

        public int UnitID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }
        public decimal nOrder { get; set; }
        public string Target { get; set; }
        public string M1 { get; set; }
        public string M2 { get; set; }
        public string M3 { get; set; }
        public string M4 { get; set; }
        public string M5 { get; set; }
        public string M6 { get; set; }
        public string M7 { get; set; }
        public string M8 { get; set; }
        public string M9 { get; set; }
        public string M10 { get; set; }
        public string M11 { get; set; }
        public string M12 { get; set; }
        public List<T_TIntensity_Other> lstarrDetail { get; set; }
        public string sRemark { get; set; }
        public string nTotal { get; set; }

    }
    [Serializable]
    public class T_TEPI_Workflow
    {
        public int nReportID { get; set; }
        public int FormID { get; set; }
        public int nMonth { get; set; }
        public int? nStatusID { get; set; }
        public int? nActionBy { get; set; }
        public DateTime? dAction { get; set; }
    }
    [Serializable]
    public class T_TIntensity_Other
    {
        public int nID { get; set; }
        public int FormID { get; set; }
        public int UnderProductID { get; set; }
        public int nProductID { get; set; }
        public string sIndicator { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public string sTarget { get; set; }
        public decimal? nFactor { get; set; }
        public string M1 { get; set; }
        public string M2 { get; set; }
        public string M3 { get; set; }
        public string M4 { get; set; }
        public string M5 { get; set; }
        public string M6 { get; set; }
        public string M7 { get; set; }
        public string M8 { get; set; }
        public string M9 { get; set; }
        public string M10 { get; set; }
        public string M11 { get; set; }
        public string M12 { get; set; }
        public bool IsActive { get; set; }
        public string sUnit { get; set; }
        public string sRemark { get; set; }
    }
    #endregion epi_input_intensity_2
    [Serializable]
    public class TDeviate
    {
        public int FormID { get; set; }
        public string sMonth { get; set; }
        public int nMonth { get; set; }
        public int nProductID { get; set; }
        public string sRemark { get; set; }
        public DateTime dAction { get; set; }
        public string ProductName { get; set; }
        public string sActionBy { get; set; }
        public string sDate { get; set; }
    }
    public class T_Facility
    {
        public int nFacilityID { get; set; }
        public string sFacilityName { get; set; }
        public int? nHeaderID { get; set; }
        public int? nLevel { get; set; }
        public string sRelation { get; set; }
        public int nOperationTypeID { get; set; }
        public string sOperationName { get; set; }
    }
}