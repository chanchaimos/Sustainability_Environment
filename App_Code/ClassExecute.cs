using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for ClassExecute
/// </summary>
public class ClassExecute
{
    public class TDataWaste
    {
        //mTProductIndicator
        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public int nOrder { get; set; }
        public string cHilight { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }

        //TWaste_Product
        public int FormID { get; set; }
        public int ProductID { get; set; }
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
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string nTotal { get; set; }
        public string sRemark { get; set; }
        public decimal? nDefaultFactor { get; set; }
        public string PreviousYear { get; set; }
        public string ReportingYear { get; set; }
        public int? sAddBy { get; set; }
        public DateTime? dAddDate { get; set; }
        public int? sUpdateBy { get; set; }
        public DateTime? dUpdateDate { get; set; }
        public int? UnitID { get; set; }
        public string Target { get; set; }

        public int? nStatusIDQ1 { get; set; }
        public int? nStatusIDQ2 { get; set; }
        public int? nStatusIDQ3 { get; set; }
        public int? nStatusIDQ4 { get; set; }

        public string IsEnableInputQ1 { get; set; }
        public string IsEnableInputQ2 { get; set; }
        public string IsEnableInputQ3 { get; set; }
        public string IsEnableInputQ4 { get; set; }

        //for report
        public string sYear { get; set; }
        public int FacilityID { get; set; }
        public int DepartmentID { get; set; }
        public int BUID { get; set; }
        public int CompanyID { get; set; }

    }
    public class TDataMaterial
    {
        //mTProductIndicator
        public int ProductID { get; set; }
        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public int nOrder { get; set; }
        public string cHilight { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }

        //TMaterail_Product
        public int FormID { get; set; }
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
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string nTotal { get; set; }
        public string sRemark { get; set; }
        public decimal? nDefaultFactor { get; set; }
        public int? sAddBy { get; set; }
        public DateTime? dAddDate { get; set; }
        public int? sUpdateBy { get; set; }
        public DateTime? dUpdateDate { get; set; }
        public int? UnitID { get; set; }
        public string Target { get; set; }
        public string sTotalYTD { get; set; }
        public decimal? nTotalYTD { get; set; }
        public int? nOption { get; set; }
        public string sReporting { get; set; }

        public int? nStatusIDQ1 { get; set; }
        public int? nStatusIDQ2 { get; set; }
        public int? nStatusIDQ3 { get; set; }
        public int? nStatusIDQ4 { get; set; }

        public string IsEnableInputQ1 { get; set; }
        public string IsEnableInputQ2 { get; set; }
        public string IsEnableInputQ3 { get; set; }
        public string IsEnableInputQ4 { get; set; }

        public string sTooltip { get; set; }

        //for report
        public string sYear { get; set; }
        public int FacilityID { get; set; }
        public int DepartmentID { get; set; }
        public int BUID { get; set; }
        public int CompanyID { get; set; }

    }
    public class TUnit
    {
        public int? IDIndicator { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal? nFactor { get; set; }
    }
    public class TFacility
    {
        public int nFacID { get; set; }
        public int nOperaID { get; set; }
        public string sName { get; set; }
        public int nCompanyID { get; set; }
        public int nBUID { get; set; }
        public int nDeptID { get; set; }
    }
    public class TPrmsFacility
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public int IndicatorID { get; set; }
        public int nPermission { get; set; }
        public int nFacID { get; set; }
        public string sFacName { get; set; }
        public int OperationTypeID { get; set; }
        public int CompanyID { get; set; }
        public int? BusinessUnitID { get; set; }
        public int? DepartmentID { get; set; }
    }
    public class TData_EPI_Forms
    {
        public int FormID { get; set; }
        public string sYear { get; set; }
        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public int FacilityID { get; set; }
        public int ResponsiblePerson { get; set; }
        public DateTime? dSendate { get; set; }
        public string cStatus { get; set; }
        public DateTime? dAddDate { get; set; }
        public int sUpdateBy { get; set; }
        public DateTime? dUpdateDate { get; set; }
    }
    public class TData_Intensity
    {
        //mTProductIndicator
        public int ProductID { get; set; }
        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }

        public int FormID { get; set; }
        //public int IntensityDominatorID { get; set; }
        public int UnitID { get; set; }
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
        public string Target { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string nTotal { get; set; }
        public decimal? SpecificFactor { get; set; }

        public int? nStatusIDQ1 { get; set; }
        public int? nStatusIDQ2 { get; set; }
        public int? nStatusIDQ3 { get; set; }
        public int? nStatusIDQ4 { get; set; }

        public string IsEnableInputQ1 { get; set; }
        public string IsEnableInputQ2 { get; set; }
        public string IsEnableInputQ3 { get; set; }
        public string IsEnableInputQ4 { get; set; }

        public string sTooltip { get; set; }

        //for report
        public string sYear { get; set; }
        public int FacilityID { get; set; }
        public int DepartmentID { get; set; }
        public int BUID { get; set; }
        public int CompanyID { get; set; }
    }
    public class TData_TIntensity_Other
    {
        //mTProductIndicator

        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }

        public int FormID { get; set; }
        //public int IntensityDominatorID { get; set; }
        public int UnderProductID { get; set; }
        public int ProductID { get; set; }
        public int UnitID { get; set; }
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
        public string Target { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string nTotal { get; set; }
        public decimal? Factor { get; set; }

        //เก็บหน่วยที่กรอกจาก Header
        public string sUnitSF { get; set; }

        public int? nStatusIDQ1 { get; set; }
        public int? nStatusIDQ2 { get; set; }
        public int? nStatusIDQ3 { get; set; }
        public int? nStatusIDQ4 { get; set; }

        public string IsEnableInputQ1 { get; set; }
        public string IsEnableInputQ2 { get; set; }
        public string IsEnableInputQ3 { get; set; }
        public string IsEnableInputQ4 { get; set; }

        public string sTooltip { get; set; }

    }
    public class TData_Intensity_DisplayInput
    {
        public int ID { get; set; }
        public int OperaID { get; set; }
        public int nDisplayType { get; set; }
        public string sUrl { get; set; }
    }
    public class TData_Water
    {
        //mTProductIndicator
        public int? nHeaderID { get; set; }
        public int nLevel { get; set; }
        public int? ProductID { get; set; }
        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }
        public string cHilight { get; set; }
        public bool? isSub { get; set; }

        public int FormID { get; set; }
        public int UnitID { get; set; }
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
        public string Target { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string nTotal { get; set; }
        public decimal? Factor { get; set; }

        public int? nStatusIDQ1 { get; set; }
        public int? nStatusIDQ2 { get; set; }
        public int? nStatusIDQ3 { get; set; }
        public int? nStatusIDQ4 { get; set; }

        public string IsEnableInputQ1 { get; set; }
        public string IsEnableInputQ2 { get; set; }
        public string IsEnableInputQ3 { get; set; }
        public string IsEnableInputQ4 { get; set; }

        public string sTooltip { get; set; }

        //for report
        public string sYear { get; set; }
        public int FacilityID { get; set; }
        public int DepartmentID { get; set; }
        public int BUID { get; set; }
        public int CompanyID { get; set; }
    }
    public class TData_Remark
    {
        public int nFromID { get; set; }
        public int ProductID { get; set; }
        public int nVersion { get; set; }
        public string sRemark { get; set; }
        public int? sAddBy { get; set; }
        public DateTime? dAddDate { get; set; }
    }
    public class TDataOutput
    {
        public int IDIndicator { get; set; }
        public int OperationtypeID { get; set; }
        public int FacilityID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int nUnitID { get; set; }
        public string sUnit { get; set; }
        public decimal nOrder { get; set; }
        public int nGroupCal { get; set; }
        public string sType { get; set; }
        public string cTotalAll { get; set; }
        public string cTotalGroup { get; set; }
        public string sTotalArea { get; set; }

        ///CHeck
        public bool isSub { get; set; }

        //Tootip
        public string TooptipID { get; set; }
        public string TooltipName { get; set; }

        //value show
        public string sM1 { get; set; }
        public string sM2 { get; set; }
        public string sM3 { get; set; }
        public string sM4 { get; set; }
        public string sM5 { get; set; }
        public string sM6 { get; set; }
        public string sM7 { get; set; }
        public string sM8 { get; set; }
        public string sM9 { get; set; }
        public string sM10 { get; set; }
        public string sM11 { get; set; }
        public string sM12 { get; set; }
        public string sTarget { get; set; }
        public string sTotal { get; set; }
        public string sQ1 { get; set; }
        public string sQ2 { get; set; }
        public string sQ3 { get; set; }
        public string sQ4 { get; set; }
        public string sH1 { get; set; }
        public string sH2 { get; set; }

        //value calculate
        public decimal? nM1 { get; set; }
        public decimal? nM2 { get; set; }
        public decimal? nM3 { get; set; }
        public decimal? nM4 { get; set; }
        public decimal? nM5 { get; set; }
        public decimal? nM6 { get; set; }
        public decimal? nM7 { get; set; }
        public decimal? nM8 { get; set; }
        public decimal? nM9 { get; set; }
        public decimal? nM10 { get; set; }
        public decimal? nM11 { get; set; }
        public decimal? nM12 { get; set; }
        public decimal? nTarget { get; set; }
        public decimal? nTotal { get; set; }
        public decimal? nQ1 { get; set; }
        public decimal? nQ2 { get; set; }
        public decimal? nQ3 { get; set; }
        public decimal? nQ4 { get; set; }
        public decimal? nH1 { get; set; }
        public decimal? nH2 { get; set; }

        //Other Value
        public string sMakeField1 { get; set; }
        public string sMakeField2 { get; set; }
        public string sMakeField3 { get; set; }
        public string sMakeField4 { get; set; }
        public int? nMakeField1 { get; set; }
        public int? nMakeField2 { get; set; }
        public int? nMakeField3 { get; set; }
        public int? nMakeField4 { get; set; }

        //For Report
        public string sYear { get; set; }
        public int DepartmentID { get; set; }
        public int BUID { get; set; }
        public int CompanyID { get; set; }
        public string sDisposalName { get; set; }
        /// <summary>
        /// for epi report
        /// </summary>
        public decimal? nTotalYTD { get; set; }
        /// <summary>
        /// for epi report
        /// </summary>
        public string sTotalYTD { get; set; }
        /// <summary>
        /// for check Head
        /// </summary>
        public int nHeadID { get; set; }
    }
    public class TDataExcuteOutput
    {

        public int IDIndicator { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }
        public int UnitID { get; set; }
        public int? FormID { get; set; }
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
        public string nTotal { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public int? TooptipID { get; set; }
        public string TooltipName { get; set; }

        //Other Value
        public string sMakeField1 { get; set; }
        public string sMakeField2 { get; set; }
        public string sMakeField3 { get; set; }
        public string sMakeField4 { get; set; }
    }
    public class TData_ProductCalculate
    {
        public int IDIndicator { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string sType { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }

    }
    public class TDataBindGraph
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string sUnit { get; set; }
        public string sMonth { get; set; }
        public decimal? nValue { get; set; }
        public int nOrder { get; set; }
        public int nMonth { get; set; }
        public int nGroup { get; set; }
        public decimal? nValueTarget { get; set; }

    }
    public class TDataRole
    {
        public int nRoleID { get; set; }
        public string sRoleName { get; set; }
    }
    public class TDataRemark2Ouput
    {
        public int nProductID { get; set; }
        public string sProductName { get; set; }
        public string sRemark { get; set; }
        public int nVersion { get; set; }
        public int? nOrder { get; set; }
    }
    public class TDataFileUploadWorkflow
    {
        public int nFormID { get; set; }
        public int nFacID { get; set; }
        public int nIndID { get; set; }
        public int nFileTempID { get; set; }
        public int nFileID { get; set; }
        public string sPath { get; set; }
        public string sSysFileName { get; set; }
        public string sFileName { get; set; }
        public string sShow { get; set; }
        public string sIsNewFile { get; set; }
    }
    public class TUpdateEPIWorkflow
    {
        public int nWorkflowID { get; set; }
        public int nFromID { get; set; }
        public int nQuater { get; set; }
        public int nStatustID { get; set; }
        public int nLevel { get; set; }
        public int UpdateID { get; set; }

        public int nNextStepUID { get; set; }
        public string sComment { get; set; }
        //public int nLevelUserCCEmail { get; set; }

        public bool IsServiceComplete { get; set; }
        public string sServiceMsg { get; set; }
    }
    public class TDataStatus
    {
        public int nStatusID { get; set; }
        public string sStatusName { get; set; }
        public string sShortName { get; set; }
    }
    public class TDataSendMailEPIWF
    {
        public int nFormID { get; set; }
        public int nOperaID { get; set; }
        public string sYear { get; set; }
        public int nIndID { get; set; }
        public int nFacID { get; set; }
        public int nToUID { get; set; }
        public int nLevel { get; set; }
        public int nQuater { get; set; }
        public int nStatusID { get; set; }

        public string sAppLevleName { get; set; }
        public string sFacName { get; set; }
        public string sOperaName { get; set; }
        public string sIndName { get; set; }
        public string sUserFullName { get; set; }
        public string sMassage { get; set; }
        public string sStatusName { get; set; }

        public string sComment { get; set; }
        //public int nLevelUserCCEmail { get; set; }
    }
    public class TData_Effluent
    {
        public int ProductID { get; set; }
        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }
        public string cHilight { get; set; }
        public string cSetCode { get; set; }
        public string sSetHtml { get; set; }

        public int FormID { get; set; }
        public int UnitID { get; set; }
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
        public string Target { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string nTotal { get; set; }
        public decimal? Factor { get; set; }

        public int? nStatusIDQ1 { get; set; }
        public int? nStatusIDQ2 { get; set; }
        public int? nStatusIDQ3 { get; set; }
        public int? nStatusIDQ4 { get; set; }

        public string IsEnableInputQ1 { get; set; }
        public string IsEnableInputQ2 { get; set; }
        public string IsEnableInputQ3 { get; set; }
        public string IsEnableInputQ4 { get; set; }

        //for report
        public string sYear { get; set; }
        public int FacilityID { get; set; }
        public int DepartmentID { get; set; }
        public int BUID { get; set; }
        public int CompanyID { get; set; }
    }
    public class TData_Effluent_Point : TData_Effluent
    {
        public int nPointID { get; set; }
        public string sPointName { get; set; }
    }
    public class TDataProduct
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string sType { get; set; }
        public string sUnit { get; set; }
        public int? nUnitID { get; set; }
        public string cTotalAll { get; set; }
        public string cTotal { get; set; }
        public int nGroupCalc { get; set; }
        public int nOrder { get; set; }

        public int nReferenceID { get; set; }
    }
    public class TData_Emission
    {
        public int ProductID { get; set; }
        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }

        public int FormID { get; set; }
        public int UnitID { get; set; }
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
        public string Target { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string nTotal { get; set; }
        public decimal? Factor { get; set; }

        public int? nStatusIDQ1 { get; set; }
        public int? nStatusIDQ2 { get; set; }
        public int? nStatusIDQ3 { get; set; }
        public int? nStatusIDQ4 { get; set; }

        public string IsEnableInputQ1 { get; set; }
        public string IsEnableInputQ2 { get; set; }
        public string IsEnableInputQ3 { get; set; }
        public string IsEnableInputQ4 { get; set; }

        //For Emission VOC
        public string sOption { get; set; }

        //for report
        public string sYear { get; set; }
        public int FacilityID { get; set; }
        public int DepartmentID { get; set; }
        public int BUID { get; set; }
        public int CompanyID { get; set; }
    }
    public class TData_Emission_Stack
    {
        public int nStackID { get; set; }
        public int ProductID { get; set; }
        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }
        public string cSetCode { get; set; }

        public int FormID { get; set; }
        public int UnitID { get; set; }
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
        public string Target { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string nTotal { get; set; }
        public decimal? Factor { get; set; }

        public int? nStatusIDQ1 { get; set; }
        public int? nStatusIDQ2 { get; set; }
        public int? nStatusIDQ3 { get; set; }
        public int? nStatusIDQ4 { get; set; }

        public string IsEnableInputQ1 { get; set; }
        public string IsEnableInputQ2 { get; set; }
        public string IsEnableInputQ3 { get; set; }
        public string IsEnableInputQ4 { get; set; }

        public int? nOptionProduct { get; set; }

        //For Emission VOC
        public string sOption { get; set; }

        //for report
        public string sStackName { get; set; }
        public string sYear { get; set; }
        public int FacilityID { get; set; }
        public int DepartmentID { get; set; }
        public int BUID { get; set; }
        public int CompanyID { get; set; }
    }
    public class TMaster_DataType
    {
        public int nID { get; set; }
        public string sName { get; set; }
        public string sType { get; set; }
        public string cActive { get; set; }
        public int? nOrder { get; set; }
        public int? nReferenceID { get; set; }
    }
    public class TData_Spill_Product
    {
        public int ProductID { get; set; }
        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }

        public int FormID { get; set; }
        public int UnitID { get; set; }
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
        public string Target { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string nTotal { get; set; }
        public decimal? Factor { get; set; }

        public int? nStatusIDQ1 { get; set; }
        public int? nStatusIDQ2 { get; set; }
        public int? nStatusIDQ3 { get; set; }
        public int? nStatusIDQ4 { get; set; }

        public string IsEnableInputQ1 { get; set; }
        public string IsEnableInputQ2 { get; set; }
        public string IsEnableInputQ3 { get; set; }
        public string IsEnableInputQ4 { get; set; }
    }
    public class TData_Spill
    {
        public int nSpillID { get; set; }//*
        public int PrimaryReasonID { get; set; } //*Primary reason for loss of containment แก้ชื่อเป็น Cause of Spill
        public string sPrimaryReason { get; set; }
        public string sOtherPrimary { get; set; }
        public string SpillType { get; set; }//*
        public string sSpillTypeName { get; set; }//HC = Hydrocarbon,NHC = Non-hydrocarbon,
        public int SpillOfID { get; set; }//*
        public string sSpillOfName { get; set; }
        public string sOtherSpillOf { get; set; }
        public string Volume { get; set; }//*
        public int UnitVolumeID { get; set; }//*
        public string sUnitName { get; set; }
        public int SpillToID { get; set; }//*
        public string sOtherSpillTo { get; set; }
        public string sSpillToName { get; set; }
        public int SpillByID { get; set; }//*
        public string sSpillByName { get; set; }
        public int? Signification1ID { get; set; }//*ถ้าไม่เท่ากับ 0 หรือ Null จะนำค่าไปคำนวณ (ถ้าระบุว่า Spill to environment จะนำมาคำนวนทุกกรณี)
        public int? Signification2ID { get; set; }//*
        public DateTime? SpillDate { get; set; }//*
        public string sDescription { get; set; }
        public string IncidentDescription { get; set; }//>>แก้ไขชื่อคอลัมน์หน้าเว็บเป็น Intermediate Action >> 11/3/2558 by PTTEP
        public string RecoveryAction { get; set; }
        public string sIsSensitiveArea { get; set; }//Y = นำไปคำนวณเป็น Significant Spill
        public string IsPrmsEdit { get; set; }
        public string IsAddFile { get; set; }

        //เพื่อให้ไปคำนวณ
        public decimal? nSpillVolume { get; set; }
    }
    public class TData_Complaint_Product
    {
        public int ProductID { get; set; }
        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }

        public int FormID { get; set; }
        public int UnitID { get; set; }
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
        public string Target { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string nTotal { get; set; }
        public decimal? Factor { get; set; }

        public int? nStatusIDQ1 { get; set; }
        public int? nStatusIDQ2 { get; set; }
        public int? nStatusIDQ3 { get; set; }
        public int? nStatusIDQ4 { get; set; }

        public string IsEnableInputQ1 { get; set; }
        public string IsEnableInputQ2 { get; set; }
        public string IsEnableInputQ3 { get; set; }
        public string IsEnableInputQ4 { get; set; }
    }
    public class TData_Complaint
    {
        public int nComplaintID { get; set; }
        public string sComplaint { get; set; }
        public DateTime? ComplaintDate { get; set; }
        public string sIssueBy { get; set; }
        public string sSubject { get; set; }
        public string sDetail { get; set; }
        public string IsPrmsEdit { get; set; }
    }
    public class TData_Compliance_Product
    {
        public int ProductID { get; set; }
        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }

        public int FormID { get; set; }
        public int UnitID { get; set; }
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
        public string Target { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string nTotal { get; set; }
        public decimal? Factor { get; set; }

        public int? nStatusIDQ1 { get; set; }
        public int? nStatusIDQ2 { get; set; }
        public int? nStatusIDQ3 { get; set; }
        public int? nStatusIDQ4 { get; set; }

        public string IsEnableInputQ1 { get; set; }
        public string IsEnableInputQ2 { get; set; }
        public string IsEnableInputQ3 { get; set; }
        public string IsEnableInputQ4 { get; set; }
    }
    public class TData_Compliance
    {
        public int nComplianceID { get; set; }
        public string sComplaint { get; set; }
        public DateTime? ComplianceDate { get; set; }
        public string sDocNumber { get; set; }
        public string sIssueBy { get; set; }
        public string sSubject { get; set; }
        public string sDetail { get; set; }
        public string IsPrmsEdit { get; set; }
    }
    public class TData_Tooltip
    {
        public int nTootipID { get; set; }
        public string sTooltipName { get; set; }
        public int ProductID { get; set; }
    }
    public class TData_Utility
    {
        public int? nField1 { get; set; }
        public int? nField2 { get; set; }
        public int? nField3 { get; set; }
        public int? nField4 { get; set; }
        public int? nField5 { get; set; }

        public decimal? nField6 { get; set; }
        public decimal? nField7 { get; set; }
        public decimal? nField8 { get; set; }
        public decimal? nField9 { get; set; }
        public decimal? nField10 { get; set; }

        public string sField1 { get; set; }
        public string sField2 { get; set; }
        public string sField3 { get; set; }
        public string sField4 { get; set; }
        public string sField5 { get; set; }
        public string sField6 { get; set; }
        public string sField7 { get; set; }
        public string sField8 { get; set; }
        public string sField9 { get; set; }
        public string sField10 { get; set; }
    }
    public class TData_NotificationMail
    {
        public int nID { get; set; }
        public int? nOption { get; set; }

        //Option 1
        public DateTime? dSendMail1Q1 { get; set; }
        public DateTime? dSendMail1Q2 { get; set; }
        public DateTime? dSendMail1Q3 { get; set; }
        public DateTime? dSendMail1Q4 { get; set; }

        public DateTime? dSendMail2Q1 { get; set; }
        public DateTime? dSendMail2Q2 { get; set; }
        public DateTime? dSendMail2Q3 { get; set; }
        public DateTime? dSendMail2Q4 { get; set; }

        public DateTime? dSendMail3Q1 { get; set; }
        public DateTime? dSendMail3Q2 { get; set; }
        public DateTime? dSendMail3Q3 { get; set; }
        public DateTime? dSendMail3Q4 { get; set; }


        //ใช่ร่วมกับ Option 1,2
        public int? nSendMail2 { get; set; }
        public string sTypeSendMail2 { get; set; }
        public int? nSendMail3 { get; set; }
        public string sTypeSendMail3 { get; set; }
        public string sMail2ToL1 { get; set; }
        public string sMail2ToL2 { get; set; }
        public string sMail2ToL3 { get; set; }
        public string sMail2ToL4 { get; set; }
        public string sMail3ToL1 { get; set; }
        public string sMail3ToL2 { get; set; }
        public string sMail3ToL3 { get; set; }
        public string sMail3ToL4 { get; set; }
        public DateTime? dDueDateQ1 { get; set; }
        public DateTime? dDueDateQ2 { get; set; }
        public DateTime? dDueDateQ3 { get; set; }
        public DateTime? dDueDateQ4 { get; set; }
        public DateTime? dDueDateH1 { get; set; }
        public DateTime? dDueDateH2 { get; set; }

        //Option 2
        public DateTime? dSendMail1H1 { get; set; }
        public DateTime? dSendMail1H2 { get; set; }
        public DateTime? dSendMail2H1 { get; set; }
        public DateTime? dSendMail2H2 { get; set; }
        public DateTime? dSendMail3H1 { get; set; }
        public DateTime? dSendMail3H2 { get; set; }

    }

    //Report
    public class TData_TargetIndicator
    {
        public int nProductID { get; set; }
        public string sProductName { get; set; }
        public string sUnit { get; set; }
        public string sTargetIndicator { get; set; }
        public int? RefProdcutID { get; set; }
        /// <summary>
        /// I = Input,O = Output
        /// </summary>
        public string sProductType { get; set; }
    }
    public class TData_ReportFac
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string Remark { get; set; }
        public int nOldProductID { get; set; }
        public List<ClassExecute.TDataOutput> lstDetail { get; set; }
    }
    public class TData_ReportDept
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string Remark { get; set; }
        public int nOldProductID { get; set; }
        public DataTable dtData { get; set; }
    }

    public class TData_ReportCompany
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string Remark { get; set; }
        public int nOldProductID { get; set; }
        public DataTable dtData { get; set; }
    }

    public class TData_EPICompleted
    {
        public int FormID { get; set; }
        public string sYear { get; set; }
        public int FacilityID { get; set; }
        public int OperationTypeID { get; set; }
        public int CompanyID { get; set; }
        public int nQuarter { get; set; }
    }

    public class TData_Report_Org
    {
        public int? CompanyID { get; set; }
        public int? BusinessUnitID { get; set; }
        public int? DepartmentID { get; set; }
        public int? FacilityID { get; set; }
        public int? OperationtypeID { get; set; }

        public string CompanyName { get; set; }
        public string BusinessUnitName { get; set; }
        public string DepartmentName { get; set; }
        public string FacilityName { get; set; }
        public string OperationtypeName { get; set; }

        public int? IndicatorID { get; set; }
        public string IndicatorName { get; set; }
    }

    public class TSelect_Opera
    {
        public string sOperaID { get; set; }
        public string sOperaName { get; set; }
    }

    public class TSelect_Facility
    {
        public string sFacD { get; set; }
        public string sFacName { get; set; }
    }

    public class TColumn_Name
    {
        public int nID { get; set; }
        public int nColIndex { get; set; }
        public string sName { get; set; }
    }

    public class TData_ReportLaws
    {
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }
        public int nYear { get; set; }
        public string sUnit { get; set; }
        public decimal? nM1 { get; set; }
        public decimal? nM2 { get; set; }
        public decimal? nM3 { get; set; }
        public decimal? nM4 { get; set; }
        public decimal? nM5 { get; set; }
        public decimal? nM6 { get; set; }
        public decimal? nM7 { get; set; }
        public decimal? nM8 { get; set; }
        public decimal? nM9 { get; set; }
        public decimal? nM10 { get; set; }
        public decimal? nM11 { get; set; }
        public decimal? nM12 { get; set; }
        public decimal? nTotal { get; set; }
        public decimal? nAVG { get; set; }
        public decimal? nTarget { get; set; }

        public string sM1 { get; set; }
        public string sM2 { get; set; }
        public string sM3 { get; set; }
        public string sM4 { get; set; }
        public string sM5 { get; set; }
        public string sM6 { get; set; }
        public string sM7 { get; set; }
        public string sM8 { get; set; }
        public string sM9 { get; set; }
        public string sM10 { get; set; }
        public string sM11 { get; set; }
        public string sM12 { get; set; }
        public string sTotal { get; set; }
        public string sAVG { get; set; }
        public string sTarget { get; set; }

        public decimal? nLegalStd { get; set; }

        public int nStackID { get; set; }
        public int nPointID { get; set; }
        public string sStackName { get; set; }
        public string sPointName { get; set; }
    }
    public class TData_ReportLawsUnit
    {
        public string sUnit { get; set; }
        public decimal? nVal { get; set; }
        public string sVal { get; set; }
    }
    public class TData_ReportLaws_SelectRow
    {
        public int FacilityID { get; set; }
        /// <summary>
        /// StackID or PointID
        /// </summary>
        public int DataID { get; set; }
        public string sUnit { get; set; }
    }

    //Export
    public class TExport_Input
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal? nTarget { get; set; }
        public decimal? nM1 { get; set; }
        public decimal? nM2 { get; set; }
        public decimal? nM3 { get; set; }
        public decimal? nM4 { get; set; }
        public decimal? nM5 { get; set; }
        public decimal? nM6 { get; set; }
        public decimal? nM7 { get; set; }
        public decimal? nM8 { get; set; }
        public decimal? nM9 { get; set; }
        public decimal? nM10 { get; set; }
        public decimal? nM11 { get; set; }
        public decimal? nM12 { get; set; }
        public decimal? nTotal { get; set; }
        public string Remark { get; set; }

        public decimal? PreviousYear { get; set; }
        public decimal? ReportingYear { get; set; }
        public string ProductGroup { get; set; }

        public decimal? nFactor { get; set; }

        //for D7
        public string sCylinderType { get; set; }
        public decimal? nTotalTonnes { get; set; }
    }
    public class TExport_Output
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal? nTarget { get; set; }
        public decimal? nM1 { get; set; }
        public decimal? nM2 { get; set; }
        public decimal? nM3 { get; set; }
        public decimal? nM4 { get; set; }
        public decimal? nM5 { get; set; }
        public decimal? nM6 { get; set; }
        public decimal? nM7 { get; set; }
        public decimal? nM8 { get; set; }
        public decimal? nM9 { get; set; }
        public decimal? nM10 { get; set; }
        public decimal? nM11 { get; set; }
        public decimal? nM12 { get; set; }

        public decimal? nQ1 { get; set; }
        public decimal? nQ2 { get; set; }
        public decimal? nQ3 { get; set; }
        public decimal? nQ4 { get; set; }
        public decimal? nTotal { get; set; }
    }
    public class TData_ReportPie
    {
        //mTProductIndicator
        public int ProductID { get; set; }
        public int IDIndicator { get; set; }
        public int OperationTypeID { get; set; }
        public string ProductName { get; set; }
        public string cTotal { get; set; }
        public string cTotalAll { get; set; }
        public int nGroupCalc { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }
        public string sType { get; set; }
        public string cHilight { get; set; }

        public int FormID { get; set; }
        public int UnitID { get; set; }
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
        public string Target { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string nTotal { get; set; }
        public decimal? Factor { get; set; }

        public int? nStatusIDQ1 { get; set; }
        public int? nStatusIDQ2 { get; set; }
        public int? nStatusIDQ3 { get; set; }
        public int? nStatusIDQ4 { get; set; }

        public string IsEnableInputQ1 { get; set; }
        public string IsEnableInputQ2 { get; set; }
        public string IsEnableInputQ3 { get; set; }
        public string IsEnableInputQ4 { get; set; }

        public string sTooltip { get; set; }

        //for report
        public string sYear { get; set; }
        public int FacilityID { get; set; }
        public int DepartmentID { get; set; }
        public int BUID { get; set; }
        public int CompanyID { get; set; }
    }
    public class TData_Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string sType { get; set; }
        public decimal nOrder { get; set; }
        public string sUnit { get; set; }

        public int nProductHeaderID { get; set; }
        public int nOperationTypeID { get; set; }
    }
    public class TData_WFF
    {
        public int nFormID { get; set; }
        public int nQuarter { get; set; }
        public string FacilityName { get; set; }
        public string CompanyName { get; set; }
        public string OperationType { get; set; }
        public string Indicator { get; set; }
        public string sOpenFile { get; set; }
    }
}