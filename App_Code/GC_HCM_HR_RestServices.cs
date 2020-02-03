using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for GC_HCM_HR_RestServices
/// </summary>
public class GC_HCM_HR_RestServices
{
    public GC_HCM_HR_RestServices()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public class Metadata
    {
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class EmployeeDataResult
    {
        public Metadata __metadata { get; set; }
        public string PictureUrl { get; set; }
        public object UserID { get; set; }
        public object PersonalID { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string NameTH { get; set; }
        public string ENTitle { get; set; }
        public string ENFirstName { get; set; }
        public string ENLastName { get; set; }
        public string THTitle { get; set; }
        public string THFirstName { get; set; }
        public string THLastName { get; set; }
        public string Gender { get; set; }
        public string GenderTxt { get; set; }
        public string DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string MaritalStatusTxt { get; set; }
        public string MarryDate { get; set; }
        public string CurrentAddr { get; set; }
        public string HouseAddr { get; set; }
        public object EmailAddress { get; set; }
        public object Extension { get; set; }
        public object MobilePhone { get; set; }
        public string Pager { get; set; }
        public string Fax { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyShortTxt { get; set; }
        public string CompanyName { get; set; }
        public object WorkingLocation { get; set; }
        public string Area { get; set; }
        public string SubArea { get; set; }
        public string EmpGroup { get; set; }
        public string EmpGroupTxt { get; set; }
        public string EmpSubGroup { get; set; }
        public string EmpSubGroupTxt { get; set; }
        public string HiringDate { get; set; }
        public string TerminationDate { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmploymentStatusTxt { get; set; }
        public string AdminGroup { get; set; }
        public string PayrollArea { get; set; }
        public string MainPositionCostCenter { get; set; }
        public object AssignmentCostCenter { get; set; }
        public object Indicator { get; set; }
        public object OrgID { get; set; }
        public object OrgTextEN { get; set; }
        public object OrgTextTH { get; set; }
        public object OrgShortTextEN { get; set; }
        public object OrgShortTextTH { get; set; }
        public object OrgLevel { get; set; }
        public object PositionID { get; set; }
        public object PositionTextEN { get; set; }
        public object PositionTextTH { get; set; }
        public object PositionShortTextEN { get; set; }
        public object PositionShortTextTH { get; set; }
        public object PositionLevel { get; set; }
        public object ManagerialFlag { get; set; }
        public object MainPositionFlg { get; set; }
        public object ParentOrgID { get; set; }
        public string UnitOrgID { get; set; }
        public string UnitShortTextEN { get; set; }
        public string UnitShortTextTH { get; set; }
        public string UnitTextEN { get; set; }
        public string UnitTextTH { get; set; }
        public string UnitManagerPositionID { get; set; }
        public string UnitManagerEmpID { get; set; }
        public string SupOrgID { get; set; }
        public string SupShortTextEN { get; set; }
        public string SupShortTextTH { get; set; }
        public string SupTextEN { get; set; }
        public string SupTextTH { get; set; }
        public string SupManagerPositionID { get; set; }
        public string SupManagerEmpID { get; set; }
        public string ShiftOrgID { get; set; }
        public string ShiftShortTextEN { get; set; }
        public string ShiftShortTextTH { get; set; }
        public string ShiftTextEN { get; set; }
        public string ShiftTextTH { get; set; }
        public string ShiftManagerPositionID { get; set; }
        public string ShiftManagerEmpID { get; set; }
        public string DivOrgID { get; set; }
        public string DivShortTextEN { get; set; }
        public string DivShortTextTH { get; set; }
        public string DivTextEN { get; set; }
        public string DivTextTH { get; set; }
        public string DivManagerPositionID { get; set; }
        public string DivManagerEmpID { get; set; }
        public string DepOrgID { get; set; }
        public string DepShortTextEN { get; set; }
        public string DepShortTextTH { get; set; }
        public string DepTextEN { get; set; }
        public string DepTextTH { get; set; }
        public string DepManagerPositionID { get; set; }
        public string DepManagerEmpID { get; set; }
        public string FNOrgID { get; set; }
        public string FNShortTextEN { get; set; }
        public string FNShortTextTH { get; set; }
        public string FNTextEN { get; set; }
        public string FNTextTH { get; set; }
        public string FNManagerPositionID { get; set; }
        public string FNManagerEmpID { get; set; }
        public string FNGRPOrgID { get; set; }
        public string FNGRPShortTextEN { get; set; }
        public string FNGRPShortTextTH { get; set; }
        public string FNGRPTextEN { get; set; }
        public string FNGRPTextTH { get; set; }
        public string FNGRPManagerPositionID { get; set; }
        public string FNGRPManagerEmpID { get; set; }
        public string CEOOrgID { get; set; }
        public string CEOShortTextEN { get; set; }
        public string CEOShortTextTH { get; set; }
        public string CEOTextEN { get; set; }
        public string CEOTextTH { get; set; }
        public string CEOManagerPositionID { get; set; }
        public string CEOManagerEmpID { get; set; }
        public string ManagerID { get; set; }
        public string ManagerPositionID { get; set; }
        public string ManagerOrgID { get; set; }
        public string CurrentPG { get; set; }
        public string JobIFamilyD { get; set; }
        public string SkillGroupID { get; set; }
        public string IndicatorCatelog { get; set; }
    }

    public class D
    {
        public List<EmployeeDataResult> results { get; set; }
    }

    public class RootObject
    {
        public D d { get; set; }
    }

    // Company 
    public class CompanyRootObject
    {
        public CompanyD d { get; set; }
    }
    public class CompanyD
    {
        public List<CompanyDataResult> results { get; set; }
    }
    public class CompanyDataResult
    {
        public Metadata __metadata { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
    }

    //Organize
    public class OrganizeRootObject
    {
        public OrganizeD d { get; set; }
    }
    public class OrganizeD
    {
        public List<OrganizeDataResult> results { get; set; }
    }

    public class OrganizeDataResult
    {
        public Metadata __metadata { get; set; }
        public string O_ObjID { get; set; }
        public object O_Level { get; set; }
        public object O_ShortTextEN { get; set; }
        public object O_ShortTextTH { get; set; }
        public object O_TextEN { get; set; }
        public object O_TextTH { get; set; }
        public string S_ObjID { get; set; }
        public string S_Level { get; set; }
        public string S_ShortTextEN { get; set; }
        public string S_ShortTextTH { get; set; }
        public string S_TextEN { get; set; }
        public string S_TextTH { get; set; }
        public object EmployeeID { get; set; }
        public string ManagerFlg { get; set; }
        public object MainPositionFlg { get; set; }
        public string ParentO_ObjID { get; set; }
    }

    // ReportTo 
    public class ReportToRootObject
    {
        public CompanyD d { get; set; }
    }
    public class ReportToD
    {
        public List<ReportToDataResult> results { get; set; }
    }
    public class ReportToDataResult
    {
        public Metadata __metadata { get; set; }
        public string PositionID { get; set; }
        public string ReportToPosition { get; set; }
        //public string __invalid_name__row.count { get; set; }
    }

    // OrgDetails
    public class OrgDetailsRootObject
    {
        public OrgDetailsD d { get; set; }
    }
    public class OrgDetailsD
    {
        public List<OrgDetailsDataResult> results { get; set; }
    }
    public class OrgDetailsDataResult
    {
        public Metadata __metadata { get; set; }
        public string OTYPE { get; set; }
        public string OBJID { get; set; }
        public object OBJLV { get; set; }
        public object SHORTTEXTEN { get; set; }
        public object SHORTTEXTTH { get; set; }
        public object TEXTEN { get; set; }
        public object TEXTTH { get; set; }
    }

    // OSPRelation
    public class OSPRelationRootObject
    {
        public OSPRelationD d { get; set; }
    }
    public class OSPRelationD
    {
        public List<OSPRelationDataResult> results { get; set; }
    }
    public class OSPRelationDataResult
    {
        public Metadata __metadata { get; set; }
        public string PARENTOBJTYP { get; set; }
        public string PARENTOBJID { get; set; }
        public string CHILDOBJTYP { get; set; }
        public string CHILDOBJID { get; set; }
        public string MainPositionFlg { get; set; }
    }

    //JobArchitecture 

    public class JobArchitectureDataResult
    {
        public Metadata __metadata { get; set; }
        public string PositionID { get; set; }
        public string JobID { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
        public string JobCodeTH { get; set; }
        public string JobNameTH { get; set; }
        public string JobFamilyID { get; set; }
        public string JobFamilyCode { get; set; }
        public string JobFamilyName { get; set; }
        public string JobFamilyCodeTH { get; set; }
        public string JobFamilyNameTH { get; set; }
        public object FunctionalAreaID { get; set; }
        public object FunctionalAreaCode { get; set; }
        public object FunctionalAreaName { get; set; }
        public object FunctionalAreaCodeTH { get; set; }
        public object FunctionalAreaNameTH { get; set; }
    }

    public class JobArchitectureD
    {
        public List<JobArchitectureDataResult> results { get; set; }
    }

    public class JobArchitectureRootObject
    {
        public JobArchitectureD d { get; set; }
    }


    public class PersonelGradeDataResult
    {
        public Metadata __metadata { get; set; }
        public string EmployeeID { get; set; }
        public string PersonalID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string NameTH { get; set; }
        public string ENTitle { get; set; }
        public string ENFirstName { get; set; }
        public string ENLastName { get; set; }
        public string THTitle { get; set; }
        public string THFirstName { get; set; }
        public string THLastName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string MarryDate { get; set; }
        public string CurrentAddr { get; set; }
        public string HouseAddr { get; set; }
        public string CompanyCode { get; set; }
        public string Area { get; set; }
        public string SubArea { get; set; }
        public string EmpGroup { get; set; }
        public string EmpSubGroup { get; set; }
        public string HiringDate { get; set; }
        public object TerminationDate { get; set; }
        public string EmploymentStatus { get; set; }
        public string AdminGroup { get; set; }
        public string PayrollArea { get; set; }
        public string Indicator { get; set; }
        public string PersonalGrade { get; set; }
        //public Positions Positions { get; set; }
        //public Company Company { get; set; }
    }
    public class PersonelGradeD
    {
        public List<PersonelGradeDataResult> results { get; set; }
    }

    public class PersonelGradeRootObject
    {
        public PersonelGradeD d { get; set; }
    }


    public class IndicatorMappingDataResult
    {

        public string sIndicatorCat { get; set; }
        public string sMappingCode { get; set; }
        public string sJobFamilyCode { get; set; }
        public string sSkillGroupCode { get; set; }
        public string sNotSkillGroupCode { get; set; }
    }

    public class IndicatorMappingD
    {
        public List<IndicatorMappingDataResult> results { get; set; }
    }

    public class IndicatorMappingRootObject
    {
        public IndicatorMappingD d { get; set; }
    }
}