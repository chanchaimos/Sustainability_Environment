using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Configuration;
using sysExtension;

/// <summary>
/// Summary description for FunctionGetData
/// </summary>
public class FunctionGetData
{
    private static string strConn = SystemFunction.strConnect;

    public static DataTable GetOperationType()
    {
        string sUID = HttpContext.Current.Session["UID"] + "";
        string sRoleID = HttpContext.Current.Session["MPRoleID"] + "";

        string sql = "select * from mOperationType where cActive = 'Y' and cDel = 'N' order by name";
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(strConn, sql);
        return dt;
    }

    public static DataTable GetIndicator()
    {
        string sql = "select * from mTIndicator order by Indicator";
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(strConn, sql);
        return dt;
    }

    public static DataTable GetIndicator(int nNotID)
    {
        string sql = "select * from mTIndicator where id <> '" + nNotID + "' order by Indicator";
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(strConn, sql);
        return dt;
    }

    public static string GetRemarkIntensity(int EPI_FORMID, int nProductID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var query = db.TIntensity_Remark.Where(w => w.FormID == EPI_FORMID && w.ProductID == nProductID).OrderByDescending(o => o.nVersion).FirstOrDefault();
        return query != null ? query.sRemark : "";
    }

    public static List<ClassExecute.TDataRemark2Ouput> GetRemarkIntensity(int EPI_FORMID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataRemark2Ouput> lstTemp = new List<ClassExecute.TDataRemark2Ouput>();
        lstTemp = (from r in db.TIntensity_Remark.Where(w => w.FormID == EPI_FORMID)
                   from p in db.mTProductIndicator.Where(w => w.cDel == "N" && w.ProductID == r.ProductID)
                   where r.nVersion == db.TIntensity_Remark.Where(w => w.FormID == r.FormID && w.ProductID == r.ProductID).Max(x => x.nVersion)
                   select new ClassExecute.TDataRemark2Ouput
                   {
                       nProductID = r.ProductID,
                       sProductName = p.ProductName,
                       sRemark = r.sRemark,
                       nVersion = r.nVersion,
                       nOrder = p.nOrder ?? 0
                   }).OrderBy(o => o.nOrder).ThenBy(o => o.sProductName).ToList();

        return lstTemp;
    }

    public static List<ClassExecute.TUnit> GetUnitF1(string sProductID, int nIndicatorID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TUnit> lstTemp = new List<ClassExecute.TUnit>();
        if (!string.IsNullOrEmpty(sProductID))
        {
            int nProductID = 0;
            nProductID = int.TryParse(sProductID, out nProductID) ? nProductID : 0;

            lstTemp = (from u in db.mTProductIndicatorUnit.AsEnumerable().Where(w => w.ProductID == nProductID && w.IDIndicator == nIndicatorID)
                       from un in db.mTUnit.AsEnumerable().Where(w => w.cDel == "N" && w.cActive == "Y" && w.UnitID == u.UnitID)
                       select new ClassExecute.TUnit
                       {
                           ProductID = u.ProductID,
                           IDIndicator = u.IDIndicator ?? 0,
                           UnitID = u.UnitID,
                           UnitName = un.UnitName,
                           nFactor = u.nDefaultFactor
                       }).OrderBy(o => o.UnitName).ToList();
        }
        else
        {
            lstTemp = db.mTUnit.AsEnumerable().Where(w => w.cDel == "N" && w.cActive == "Y").Select(s => new ClassExecute.TUnit
                       {
                           ProductID = 0,
                           IDIndicator = 0,
                           UnitID = s.UnitID,
                           UnitName = s.UnitName,
                           nFactor = s.nFactor
                       }).OrderBy(o => o.UnitName).ToList();
        }

        return lstTemp;
    }

    public static string GetRemarkMaterial(int nEPIFomrID, int nProductID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var query = db.TMaterial_Remark.Where(w => w.FormID == nEPIFomrID && w.ProductID == nProductID).OrderByDescending(o => o.nVersion).FirstOrDefault();
        return query != null ? query.sRemark : "";
    }

    public static List<ClassExecute.TDataRemark2Ouput> GetRemarkMaterial(int nEPIFomrID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataRemark2Ouput> lstTemp = new List<ClassExecute.TDataRemark2Ouput>();
        lstTemp = (from r in db.TMaterial_Remark.Where(w => w.FormID == nEPIFomrID)
                   from p in db.mTProductIndicator.Where(w => w.cDel == "N" && w.ProductID == r.ProductID)
                   where r.nVersion == db.TMaterial_Remark.Where(w => w.FormID == r.FormID && w.ProductID == r.ProductID).Max(x => x.nVersion)
                   select new ClassExecute.TDataRemark2Ouput
                   {
                       nProductID = r.ProductID,
                       sProductName = p.ProductName,
                       sRemark = r.sRemark,
                       nVersion = r.nVersion,
                       nOrder = p.nOrder ?? 0
                   }).OrderBy(o => o.nOrder).ThenBy(o => o.sProductName).ToList();

        return lstTemp;
    }

    public static List<ClassExecute.TDataRemark2Ouput> GetRemarkWaste(int nEPIFomrID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataRemark2Ouput> lstTemp = new List<ClassExecute.TDataRemark2Ouput>();
        lstTemp = (from r in db.TWaste_Remark.Where(w => w.FormID == nEPIFomrID)
                   from p in db.mTProductIndicator.Where(w => w.cDel == "N" && w.ProductID == r.ProductID)
                   where r.nVersion == db.TWaste_Remark.Where(w => w.FormID == r.FormID && w.ProductID == r.ProductID).Max(x => x.nVersion)
                   select new ClassExecute.TDataRemark2Ouput
                   {
                       nProductID = r.ProductID,
                       sProductName = p.ProductName,
                       sRemark = r.sRemark,
                       nVersion = r.nVersion,
                       nOrder = p.nOrder ?? 0
                   }).OrderBy(o => o.nOrder).ThenBy(o => o.sProductName).ToList();

        return lstTemp;
    }

    public static string GetRemarkWater(int EPI_FORMID, int nProductID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var query = db.TWater_Remark.Where(w => w.FormID == EPI_FORMID && w.ProductID == nProductID).OrderByDescending(o => o.nVersion).FirstOrDefault();
        return query != null ? query.sRemark : "";
    }

    public static List<ClassExecute.TDataRemark2Ouput> GetRemarkWater(int nEPIFomrID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataRemark2Ouput> lstTemp = new List<ClassExecute.TDataRemark2Ouput>();
        lstTemp = (from r in db.TWater_Remark.Where(w => w.FormID == nEPIFomrID)
                   from p in db.mTProductIndicator.Where(w => w.cDel == "N" && w.ProductID == r.ProductID)
                   where r.nVersion == db.TWater_Remark.Where(w => w.FormID == r.FormID && w.ProductID == r.ProductID).Max(x => x.nVersion)
                   select new ClassExecute.TDataRemark2Ouput
                   {
                       nProductID = r.ProductID,
                       sProductName = p.ProductName,
                       sRemark = r.sRemark,
                       nVersion = r.nVersion,
                       nOrder = p.nOrder ?? 0
                   }).OrderBy(o => o.nOrder).ThenBy(o => o.sProductName).ToList();

        return lstTemp;
    }

    public static List<ClassExecute.TDataRemark2Ouput> GetRemarkEffluent(int nEPIFomrID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataRemark2Ouput> lstTemp = new List<ClassExecute.TDataRemark2Ouput>();
        lstTemp = (from r in db.TEffluent_Remark.Where(w => w.FormID == nEPIFomrID)
                   from p in db.mTProductIndicator.Where(w => w.cDel == "N" && w.ProductID == r.ProductID)
                   where r.nVersion == db.TEffluent_Remark.Where(w => w.FormID == r.FormID && w.ProductID == r.ProductID).Max(x => x.nVersion)
                   select new ClassExecute.TDataRemark2Ouput
                   {
                       nProductID = r.ProductID,
                       sProductName = p.ProductName,
                       sRemark = r.sRemark,
                       nVersion = r.nVersion,
                       nOrder = p.nOrder ?? 0
                   }).OrderBy(o => o.nOrder).ThenBy(o => o.sProductName).ToList();

        return lstTemp;
    }

    public static List<ClassExecute.TDataRemark2Ouput> GetRemarkEmission(int nEPIFomrID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataRemark2Ouput> lstTemp = new List<ClassExecute.TDataRemark2Ouput>();
        lstTemp = (from r in db.TEmission_Remark.Where(w => w.FormID == nEPIFomrID)
                   from p in db.mTProductIndicator.Where(w => w.cDel == "N" && w.ProductID == r.ProductID)
                   where r.nVersion == db.TEmission_Remark.Where(w => w.FormID == r.FormID && w.ProductID == r.ProductID).Max(x => x.nVersion)
                   select new ClassExecute.TDataRemark2Ouput
                   {
                       nProductID = r.ProductID,
                       sProductName = p.ProductName,
                       sRemark = r.sRemark,
                       nVersion = r.nVersion,
                       nOrder = p.nOrder ?? 0
                   }).OrderBy(o => o.nOrder).ThenBy(o => o.sProductName).ToList();

        return lstTemp;
    }

    public static List<ClassExecute.TData_Intensity> GetDataIntensityToCalculateOutput(int nOperaID, int nFacID, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 6;
        int nEPIFromID = 0;
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == IndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (qEPIForm != null)
        {
            nEPIFromID = qEPIForm.FormID;
        }

        string sql = @"select tpi.ProductID,tpi.ProductName,tpi.cTotal,tpi.cTotalAll,isnull(tpi.nGroupCalc,0) 'nGroupCalc',tpi.sUnit,tpi.sType,
isnull(tiup.nOrder,0) 'nOrder',
isnull(tid.FormID,0) 'FormID',isnull(tid.UnitID,0) 'UnitID',tid.M1,tid.M2,tid.M3,tid.M4,tid.M5,tid.M6,tid.M7,tid.M8,tid.M9,tid.M10,tid.M11,tid.M12,tid.nTotal,tid.Target
from mTProductIndicator tpi
inner join TIntensityUseProduct tiup on tpi.ProductID = tiup.ProductID and tiup.OperationTypeID = '{0}'
left join TIntensityDominator tid on tiup.ProductID = tid.ProductID and tid.FormID = '{1}'
where tpi.IDIndicator = '{2}'
order by tiup.nOrder";

        List<ClassExecute.TData_Intensity> lstdata = new List<ClassExecute.TData_Intensity>();
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, string.Format(sql, nOperaID, nEPIFromID, IndID));
        if (dt.Rows.Count > 0)
        {
            lstdata = dt.AsEnumerable().Select(s => new ClassExecute.TData_Intensity
            {
                ProductID = s.Field<int>("ProductID"),
                ProductName = s.Field<string>("ProductName"),
                cTotal = s.Field<string>("cTotal"),
                cTotalAll = s.Field<string>("cTotalAll"),
                nGroupCalc = s.Field<int>("nGroupCalc"),
                nOrder = s.Field<decimal>("nOrder"),
                sUnit = s.Field<string>("sUnit"),
                sType = s.Field<string>("sType"),
                FormID = s.Field<int>("FormID"),
                UnitID = s.Field<int>("UnitID"),
                M1 = s.Field<string>("M1"),
                M2 = s.Field<string>("M2"),
                M3 = s.Field<string>("M3"),
                M4 = s.Field<string>("M4"),
                M5 = s.Field<string>("M5"),
                M6 = s.Field<string>("M6"),
                M7 = s.Field<string>("M7"),
                M8 = s.Field<string>("M8"),
                M9 = s.Field<string>("M9"),
                M10 = s.Field<string>("M10"),
                M11 = s.Field<string>("M11"),
                M12 = s.Field<string>("M12"),
                nTotal = s.Field<string>("nTotal"),
                Target = s.Field<string>("Target")
            }).ToList();
        }

        #region Old Code V.1
        /*var qData = (from i in db.mTProductIndicator.AsEnumerable().Where(w => w.IDIndicator.Value == IndID)
                     from u in db.TIntensityUseProduct.AsEnumerable().Where(w => w.OperationTypeID == nOperaID && w.ProductID == i.ProductID)
                     from d in db.TIntensityDominator.AsEnumerable().Where(w => w.ProductID == i.ProductID && w.FormID == nEPIFromID).DefaultIfEmpty()
                     select new ClassExecute.TData_Intensity
                     {
                         ProductID = i.ProductID,
                         ProductName = i.ProductName,
                         cTotal = i.cTotal,
                         cTotalAll = i.cTotalAll,
                         nGroupCalc = i.nGroupCalc ?? 0,
                         nOrder = u.nOrder ?? 0,
                         sUnit = i.sUnit,
                         sType = i.sType,
                         FormID = d != null ? d.FormID : 0,
                         UnitID = d != null ? d.UnitID ?? 0 : 0,
                         M1 = d != null ? d.M1 : "",
                         M2 = d != null ? d.M2 : "",
                         M3 = d != null ? d.M3 : "",
                         M4 = d != null ? d.M4 : "",
                         M5 = d != null ? d.M5 : "",
                         M6 = d != null ? d.M6 : "",
                         M7 = d != null ? d.M7 : "",
                         M8 = d != null ? d.M8 : "",
                         M9 = d != null ? d.M9 : "",
                         M10 = d != null ? d.M10 : "",
                         M11 = d != null ? d.M11 : "",
                         M12 = d != null ? d.M12 : "",
                         nTotal = d != null ? d.nTotal : "",
                         Target = d != null ? d.Target : "",

                     }).OrderBy(o => o.nOrder).ToList();
        */
        #endregion

        #region Old Code V.2
        /*var qData = db.Database.SqlQuery<ClassExecute.TData_Intensity>(string.Format(sql, nOperaID, nEPIFromID, IndID)).Select(s => new ClassExecute.TData_Intensity
        {
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            cTotal = s.cTotal,
            cTotalAll = s.cTotalAll,
            nGroupCalc = s.nGroupCalc,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            sType = s.sType,
            FormID = s.FormID,
            UnitID = s.UnitID,
            M1 = s.M1,
            M2 = s.M2,
            M3 = s.M3,
            M4 = s.M4,
            M5 = s.M5,
            M6 = s.M6,
            M7 = s.M7,
            M8 = s.M8,
            M9 = s.M9,
            M10 = s.M10,
            M11 = s.M11,
            M12 = s.M12,
            nTotal = s.nTotal,
            Target = s.Target
        }).ToList();*/
        #endregion

        return lstdata;
    }

    public static List<ClassExecute.TDataWaste> GetDataWasteToCalculateOutput(int nOperaID, int nFacID, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 10;
        int nEPIFromID = 0;
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == IndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (qEPIForm != null)
        {
            nEPIFromID = qEPIForm.FormID;
        }

        string sql = @"select tpi.ProductID,tpi.ProductName,tpi.cTotal,tpi.cTotalAll,isnull(tpi.nGroupCalc,0) 'nGroupCalc',isnull(tpi.nOrder,0) 'nOrder',tpi.sUnit,tpi.sType
,isnull(twp.FormID,0) 'FormID',isnull(twp.UnitID,0) 'UnitID'
,twp.M1,twp.M2,twp.M3,twp.M4,twp.M5,twp.M6,twp.M7,twp.M8,twp.M9,twp.M10,twp.M11,twp.M12,twp.nTotal,twp.Target,twp.PreviousYear,twp.ReportingYear
from mTProductIndicator tpi
left join TWaste_Product twp on tpi.ProductID = twp.ProductID and twp.FormID = '" + nEPIFromID + @"'
where tpi.IDIndicator = '" + IndID + "'";

        List<ClassExecute.TDataWaste> lstdata = new List<ClassExecute.TDataWaste>();
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, sql);
        if (dt.Rows.Count > 0)
        {
            lstdata = dt.AsEnumerable().Select(s => new ClassExecute.TDataWaste
            {
                ProductID = s.Field<int>("ProductID"),
                ProductName = s.Field<string>("ProductName"),
                cTotal = s.Field<string>("cTotal"),
                cTotalAll = s.Field<string>("cTotalAll"),
                nGroupCalc = s.Field<int>("nGroupCalc"),
                nOrder = s.Field<int>("nOrder"),
                sUnit = s.Field<string>("sUnit"),
                sType = s.Field<string>("sType"),
                FormID = s.Field<int>("FormID"),
                UnitID = s.Field<int>("UnitID"),
                M1 = s.Field<string>("M1"),
                M2 = s.Field<string>("M2"),
                M3 = s.Field<string>("M3"),
                M4 = s.Field<string>("M4"),
                M5 = s.Field<string>("M5"),
                M6 = s.Field<string>("M6"),
                M7 = s.Field<string>("M7"),
                M8 = s.Field<string>("M8"),
                M9 = s.Field<string>("M9"),
                M10 = s.Field<string>("M10"),
                M11 = s.Field<string>("M11"),
                M12 = s.Field<string>("M12"),
                nTotal = s.Field<string>("nTotal"),
                Target = s.Field<string>("Target"),
                PreviousYear = s.Field<string>("PreviousYear"),
                ReportingYear = s.Field<string>("ReportingYear"),

            }).OrderBy(o => o.nOrder).ToList();
        }

        #region old code
        /*var qData = (from i in db.mTProductIndicator.AsEnumerable().Where(w => w.IDIndicator.Value == IndID)
                     from d in db.TWaste_Product.AsEnumerable().Where(w => w.ProductID == i.ProductID && w.FormID == nEPIFromID).DefaultIfEmpty()
                     select new ClassExecute.TDataWaste
                     {
                         ProductID = i.ProductID,
                         ProductName = i.ProductName,
                         cTotal = i.cTotal,
                         cTotalAll = i.cTotalAll,
                         nGroupCalc = i.nGroupCalc ?? 0,
                         nOrder = i.nOrder ?? 0,
                         sUnit = i.sUnit,
                         sType = i.sType,
                         FormID = d != null ? d.FormID : 0,
                         UnitID = d != null ? d.UnitID ?? 0 : 0,
                         M1 = d != null ? d.M1 : "",
                         M2 = d != null ? d.M2 : "",
                         M3 = d != null ? d.M3 : "",
                         M4 = d != null ? d.M4 : "",
                         M5 = d != null ? d.M5 : "",
                         M6 = d != null ? d.M6 : "",
                         M7 = d != null ? d.M7 : "",
                         M8 = d != null ? d.M8 : "",
                         M9 = d != null ? d.M9 : "",
                         M10 = d != null ? d.M10 : "",
                         M11 = d != null ? d.M11 : "",
                         M12 = d != null ? d.M12 : "",
                         nTotal = d != null ? d.nTotal : "",
                         Target = d != null ? d.Target : "",
                         PreviousYear = d != null ? d.PreviousYear : "",
                         ReportingYear = d != null ? d.ReportingYear : ""
                     }).OrderBy(o => o.nOrder).ToList();*/
        #endregion

        return lstdata;
    }

    public static List<ClassExecute.TData_Water> GetDataWaterToCalculateOutput(int nOperaID, int nFacID, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 11;
        int nEPIFromID = 0;
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == IndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (qEPIForm != null)
        {
            nEPIFromID = qEPIForm.FormID;
        }

        string sql = @"select tpi.ProductID,tpi.ProductName,tpi.cTotal,tpi.cTotalAll,isnull(tpi.nGroupCalc,0) 'nGroupCalc',isnull(tpi.nOrder,0) 'nOrder',tpi.sUnit,tpi.sType
,isnull(twp.FormID,0) 'FormID',isnull(twp.UnitID,0) 'UnitID'
,twp.M1,twp.M2,twp.M3,twp.M4,twp.M5,twp.M6,twp.M7,twp.M8,twp.M9,twp.M10,twp.M11,twp.M12,twp.nTotal,twp.Target
from mTProductIndicator tpi
left join TWater_Product twp on tpi.ProductID = twp.ProductID and twp.FormID = '" + nEPIFromID + @"'
where tpi.IDIndicator = '" + IndID + "'";

        List<ClassExecute.TData_Water> lstdata = new List<ClassExecute.TData_Water>();
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, sql);
        if (dt.Rows.Count > 0)
        {
            lstdata = dt.AsEnumerable().Select(s => new ClassExecute.TData_Water
                         {
                             ProductID = s.Field<int>("ProductID"),
                             ProductName = s.Field<string>("ProductName"),
                             cTotal = s.Field<string>("cTotal"),
                             cTotalAll = s.Field<string>("cTotalAll"),
                             nGroupCalc = s.Field<int>("nGroupCalc"),
                             nOrder = s.Field<int>("nOrder"),
                             sUnit = s.Field<string>("sUnit"),
                             sType = s.Field<string>("sType"),
                             FormID = s.Field<int>("FormID"),
                             UnitID = s.Field<int>("UnitID"),
                             M1 = s.Field<string>("M1"),
                             M2 = s.Field<string>("M2"),
                             M3 = s.Field<string>("M3"),
                             M4 = s.Field<string>("M4"),
                             M5 = s.Field<string>("M5"),
                             M6 = s.Field<string>("M6"),
                             M7 = s.Field<string>("M7"),
                             M8 = s.Field<string>("M8"),
                             M9 = s.Field<string>("M9"),
                             M10 = s.Field<string>("M10"),
                             M11 = s.Field<string>("M11"),
                             M12 = s.Field<string>("M12"),
                             nTotal = s.Field<string>("nTotal"),
                             Target = s.Field<string>("Target")
                         }).OrderBy(o => o.nOrder).ToList();
        }

        #region old code
        /*var qData = (from i in db.mTProductIndicator.AsEnumerable().Where(w => w.IDIndicator.Value == IndID)
                     from d in db.TWater_Product.AsEnumerable().Where(w => w.ProductID == i.ProductID && w.FormID == nEPIFromID).DefaultIfEmpty()
                     select new ClassExecute.TData_Water
                     {
                         ProductID = i.ProductID,
                         ProductName = i.ProductName,
                         cTotal = i.cTotal,
                         cTotalAll = i.cTotalAll,
                         nGroupCalc = i.nGroupCalc ?? 0,
                         nOrder = i.nOrder ?? 0,
                         sUnit = i.sUnit,
                         sType = i.sType,
                         FormID = d != null ? d.FormID : 0,
                         UnitID = d != null ? d.UnitID ?? 0 : 0,
                         M1 = d != null ? d.M1 : "",
                         M2 = d != null ? d.M2 : "",
                         M3 = d != null ? d.M3 : "",
                         M4 = d != null ? d.M4 : "",
                         M5 = d != null ? d.M5 : "",
                         M6 = d != null ? d.M6 : "",
                         M7 = d != null ? d.M7 : "",
                         M8 = d != null ? d.M8 : "",
                         M9 = d != null ? d.M9 : "",
                         M10 = d != null ? d.M10 : "",
                         M11 = d != null ? d.M11 : "",
                         M12 = d != null ? d.M12 : "",
                         nTotal = d != null ? d.nTotal : "",
                         Target = d != null ? d.Target : "",
                     }).OrderBy(o => o.nOrder).ToList();*/
        #endregion

        return lstdata;
    }

    public static List<ClassExecute.TDataMaterial> GetDataMaterialToCalculateOutput(int nOperaID, int nFacID, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 8;
        int nEPIFromID = 0;
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == IndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (qEPIForm != null)
        {
            nEPIFromID = qEPIForm.FormID;
        }

        string sql = @"select tpi.ProductID,tpi.ProductName,tpi.cTotal,tpi.cTotalAll,isnull(tpi.nGroupCalc,0) 'nGroupCalc',isnull(tpi.nOrder,0) 'nOrder',tpi.sUnit,tpi.sType
,isnull(twp.FormID,0) 'FormID',isnull(twp.UnitID,0) 'UnitID'
,twp.M1,twp.M2,twp.M3,twp.M4,twp.M5,twp.M6,twp.M7,twp.M8,twp.M9,twp.M10,twp.M11,twp.M12,twp.nTotal,twp.Target
from mTProductIndicator tpi
left join TMaterial_Product twp on tpi.ProductID = twp.ProductID and twp.FormID = '" + nEPIFromID + @"'
where tpi.IDIndicator = '" + IndID + "'";

        List<ClassExecute.TDataMaterial> lstdata = new List<ClassExecute.TDataMaterial>();
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, sql);
        if (dt.Rows.Count > 0)
        {
            lstdata = dt.AsEnumerable().Select(s => new ClassExecute.TDataMaterial
                         {
                             ProductID = s.Field<int>("ProductID"),
                             ProductName = s.Field<string>("ProductName"),
                             cTotal = s.Field<string>("cTotal"),
                             cTotalAll = s.Field<string>("cTotalAll"),
                             nGroupCalc = s.Field<int>("nGroupCalc"),
                             nOrder = s.Field<int>("nOrder"),
                             sUnit = s.Field<string>("sUnit"),
                             sType = s.Field<string>("sType"),
                             FormID = s.Field<int>("FormID"),
                             UnitID = s.Field<int>("UnitID"),
                             M1 = s.Field<string>("M1"),
                             M2 = s.Field<string>("M2"),
                             M3 = s.Field<string>("M3"),
                             M4 = s.Field<string>("M4"),
                             M5 = s.Field<string>("M5"),
                             M6 = s.Field<string>("M6"),
                             M7 = s.Field<string>("M7"),
                             M8 = s.Field<string>("M8"),
                             M9 = s.Field<string>("M9"),
                             M10 = s.Field<string>("M10"),
                             M11 = s.Field<string>("M11"),
                             M12 = s.Field<string>("M12"),
                             nTotal = s.Field<string>("nTotal"),
                             Target = s.Field<string>("Target")
                         }).OrderBy(o => o.nOrder).ToList();
        }

        #region old code
        /*var qData = (from i in db.mTProductIndicator.Where(w => w.IDIndicator == IndID).AsEnumerable()
                     from d in db.TMaterial_Product.Where(w => w.ProductID == i.ProductID && w.FormID == nEPIFromID).AsEnumerable().DefaultIfEmpty()
                     select new ClassExecute.TDataMaterial
                     {
                         ProductID = i.ProductID,
                         ProductName = i.ProductName,
                         cTotal = i.cTotal,
                         cTotalAll = i.cTotalAll,
                         nGroupCalc = i.nGroupCalc ?? 0,
                         nOrder = i.nOrder ?? 0,
                         sUnit = i.sUnit,
                         sType = i.sType,
                         FormID = d != null ? d.FormID : 0,
                         UnitID = d != null ? d.UnitID ?? 0 : 0,
                         M1 = d != null ? d.M1 : "",
                         M2 = d != null ? d.M2 : "",
                         M3 = d != null ? d.M3 : "",
                         M4 = d != null ? d.M4 : "",
                         M5 = d != null ? d.M5 : "",
                         M6 = d != null ? d.M6 : "",
                         M7 = d != null ? d.M7 : "",
                         M8 = d != null ? d.M8 : "",
                         M9 = d != null ? d.M9 : "",
                         M10 = d != null ? d.M10 : "",
                         M11 = d != null ? d.M11 : "",
                         M12 = d != null ? d.M12 : "",
                         nTotal = d != null ? d.nTotal : "",
                         Target = d != null ? d.Target : "",
                     }).OrderBy(o => o.nOrder).ToList();*/
        #endregion

        return lstdata;
    }

    public static List<ClassExecute.TData_Effluent> GetDataEffluentToCalculateOutput(int nOperaID, int nFacID, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 3;
        int nEPIFromID = 0;
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == IndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (qEPIForm != null)
        {
            nEPIFromID = qEPIForm.FormID;
        }

        string sql = @"select tpi.ProductID,tpi.ProductName,tpi.cTotal,tpi.cTotalAll,isnull(tpi.nGroupCalc,0) 'nGroupCalc',isnull(tpi.nOrder,0) 'nOrder',tpi.sUnit,tpi.sType
,isnull(twp.FormID,0) 'FormID',isnull(twp.UnitID,0) 'UnitID'
,twp.M1,twp.M2,twp.M3,twp.M4,twp.M5,twp.M6,twp.M7,twp.M8,twp.M9,twp.M10,twp.M11,twp.M12,twp.nTotal,twp.Target
from mTProductIndicator tpi
left join TEffluent_Product twp on tpi.ProductID = twp.ProductID and twp.FormID = '" + nEPIFromID + @"'
where tpi.IDIndicator = '" + IndID + "'";

        List<ClassExecute.TData_Effluent> lstdata = new List<ClassExecute.TData_Effluent>();
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, sql);
        if (dt.Rows.Count > 0)
        {
            lstdata = dt.AsEnumerable().Select(s => new ClassExecute.TData_Effluent
            {
                ProductID = s.Field<int>("ProductID"),
                ProductName = s.Field<string>("ProductName"),
                cTotal = s.Field<string>("cTotal"),
                cTotalAll = s.Field<string>("cTotalAll"),
                nGroupCalc = s.Field<int>("nGroupCalc"),
                nOrder = s.Field<int>("nOrder"),
                sUnit = s.Field<string>("sUnit"),
                sType = s.Field<string>("sType"),
                FormID = s.Field<int>("FormID"),
                UnitID = s.Field<int>("UnitID"),
                M1 = s.Field<string>("M1"),
                M2 = s.Field<string>("M2"),
                M3 = s.Field<string>("M3"),
                M4 = s.Field<string>("M4"),
                M5 = s.Field<string>("M5"),
                M6 = s.Field<string>("M6"),
                M7 = s.Field<string>("M7"),
                M8 = s.Field<string>("M8"),
                M9 = s.Field<string>("M9"),
                M10 = s.Field<string>("M10"),
                M11 = s.Field<string>("M11"),
                M12 = s.Field<string>("M12"),
                nTotal = s.Field<string>("nTotal"),
                Target = s.Field<string>("Target")
            }).OrderBy(o => o.nOrder).ToList();
        }

        #region old code
        /*var qData = (from i in db.mTProductIndicator.AsEnumerable().Where(w => w.IDIndicator.Value == IndID)
                     from d in db.TEffluent_Product.AsEnumerable().Where(w => w.ProductID == i.ProductID && w.FormID == nEPIFromID).DefaultIfEmpty()
                     select new ClassExecute.TData_Effluent
                     {
                         ProductID = i.ProductID,
                         ProductName = i.ProductName,
                         cTotal = i.cTotal,
                         cTotalAll = i.cTotalAll,
                         nGroupCalc = i.nGroupCalc ?? 0,
                         nOrder = i.nOrder ?? 0,
                         sUnit = i.sUnit,
                         sType = i.sType,
                         FormID = d != null ? d.FormID : 0,
                         UnitID = d != null ? d.UnitID ?? 0 : 0,
                         M1 = d != null ? d.M1 : "",
                         M2 = d != null ? d.M2 : "",
                         M3 = d != null ? d.M3 : "",
                         M4 = d != null ? d.M4 : "",
                         M5 = d != null ? d.M5 : "",
                         M6 = d != null ? d.M6 : "",
                         M7 = d != null ? d.M7 : "",
                         M8 = d != null ? d.M8 : "",
                         M9 = d != null ? d.M9 : "",
                         M10 = d != null ? d.M10 : "",
                         M11 = d != null ? d.M11 : "",
                         M12 = d != null ? d.M12 : "",
                         nTotal = d != null ? d.nTotal : "",
                         Target = d != null ? d.Target : "",
                     }).OrderBy(o => o.nOrder).ToList();*/
        #endregion

        return lstdata;
    }

    public static List<ClassExecute.TData_Emission> GetDataEmissionToCalculateOutput(int nOperaID, int nFacID, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 4;
        int nEPIFromID = 0;
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == IndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (qEPIForm != null)
        {
            nEPIFromID = qEPIForm.FormID;
        }

        DataTable dt = new DataTable();

        #region Data Summary Stack
        List<ClassExecute.TData_Emission> qDataSumStack = new List<ClassExecute.TData_Emission>();
        string sqlStack = @"select tpi.ProductID,tpi.ProductName,tpi.cTotal,tpi.cTotalAll,isnull(tpi.nGroupCalc,0) 'nGroupCalc',isnull(tpi.nOrder,0) 'nOrder',tpi.sUnit,tpi.sType
,isnull(twp.FormID,0) 'FormID',isnull(twp.UnitID,0) 'UnitID'
,twp.M1,twp.M2,twp.M3,twp.M4,twp.M5,twp.M6,twp.M7,twp.M8,twp.M9,twp.M10,twp.M11,twp.M12,twp.nTotal,twp.Target
from mTProductIndicator tpi
left join TEmission_Product twp on tpi.ProductID = twp.ProductID and twp.FormID = '" + nEPIFromID + @"'
where tpi.IDIndicator = '" + IndID + "' and tpi.sType='SUM'";
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, sqlStack);
        if (dt.Rows.Count > 0)
        {
            qDataSumStack = dt.AsEnumerable().Select(s => new ClassExecute.TData_Emission
            {
                ProductID = s.Field<int>("ProductID"),
                ProductName = s.Field<string>("ProductName"),
                cTotal = s.Field<string>("cTotal"),
                cTotalAll = s.Field<string>("cTotalAll"),
                nGroupCalc = s.Field<int>("nGroupCalc"),
                nOrder = s.Field<int>("nOrder"),
                sUnit = s.Field<string>("sUnit"),
                sType = s.Field<string>("sType"),
                FormID = s.Field<int>("FormID"),
                UnitID = s.Field<int>("UnitID"),
                M1 = s.Field<string>("M1"),
                M2 = s.Field<string>("M2"),
                M3 = s.Field<string>("M3"),
                M4 = s.Field<string>("M4"),
                M5 = s.Field<string>("M5"),
                M6 = s.Field<string>("M6"),
                M7 = s.Field<string>("M7"),
                M8 = s.Field<string>("M8"),
                M9 = s.Field<string>("M9"),
                M10 = s.Field<string>("M10"),
                M11 = s.Field<string>("M11"),
                M12 = s.Field<string>("M12"),
                nTotal = s.Field<string>("nTotal"),
                Target = s.Field<string>("Target")
            }).OrderBy(o => o.nOrder).ToList();
        }

        #region Old Summary Stack
        /*var qDataSumStack = (from i in db.mTProductIndicator.Where(w => w.IDIndicator == IndID && w.sType == "SUM").AsEnumerable()
                             from d in db.TEmission_Product.Where(w => w.ProductID == i.ProductID && w.FormID == nEPIFromID).AsEnumerable().DefaultIfEmpty()
                             select new ClassExecute.TData_Emission
                             {
                                 ProductID = i.ProductID,
                                 ProductName = i.ProductName,
                                 cTotal = i.cTotal,
                                 cTotalAll = i.cTotalAll,
                                 nGroupCalc = i.nGroupCalc ?? 0,
                                 nOrder = i.nOrder ?? 0,
                                 sUnit = i.sUnit,
                                 sType = i.sType,
                                 FormID = d != null ? d.FormID : 0,
                                 UnitID = d != null ? d.UnitID ?? 0 : 0,
                                 M1 = d != null ? d.M1 : "",
                                 M2 = d != null ? d.M2 : "",
                                 M3 = d != null ? d.M3 : "",
                                 M4 = d != null ? d.M4 : "",
                                 M5 = d != null ? d.M5 : "",
                                 M6 = d != null ? d.M6 : "",
                                 M7 = d != null ? d.M7 : "",
                                 M8 = d != null ? d.M8 : "",
                                 M9 = d != null ? d.M9 : "",
                                 M10 = d != null ? d.M10 : "",
                                 M11 = d != null ? d.M11 : "",
                                 M12 = d != null ? d.M12 : "",
                                 nTotal = d != null ? d.nTotal : "",
                                 Target = d != null ? d.Target : "",
                             }).OrderBy(o => o.nOrder).ToList();*/
        #endregion
        #endregion

        #region Data VOC >>/// เนื่องจากมีการเก็บค่าเป็น YTD ลงมาด้วย ซึ่งจะทำให้ค่า 12 เดือนเป็น NULL ดังนั้นอิงตามระบบเดิมนำค่า Total มาหาร 12 เพื่อแยกให้แต่ละเดือน
        List<ClassExecute.TData_Emission> qDataVOC = new List<ClassExecute.TData_Emission>();
        dt = new DataTable();
        string sqlVOC = @"SELECT TPI.ProductID,TPI.ProductName,TPI.cTotal,TPI.cTotalAll,isnull(TPI.nGroupCalc,0) 'nGroupCalc',isnull(TPI.nOrder,0) 'nOrder',TPI.sUnit,TPI.sType
,isnull(TEV.FormID,0) 'FormID',isnull(TEV.UnitID,0) 'UnitID'
,TEV.M1,TEV.M2,TEV.M3,TEV.M4,TEV.M5,TEV.M6,TEV.M7,TEV.M8,TEV.M9,TEV.M10,TEV.M11,TEV.M12,TEV.nTotal,TEV.Target
,TEV.sOption
FROM mTProductIndicator TPI
LEFT JOIN TEmission_VOC TEV ON TPI.ProductID=TEV.ProductID AND TEV.FormID='" + nEPIFromID + @"'
WHERE TPI.IDIndicator='" + IndID + "' AND TPI.sType='VOC'";
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, sqlVOC);
        if (dt.Rows.Count > 0)
        {
            qDataVOC = dt.AsEnumerable().Select(s => new ClassExecute.TData_Emission
            {
                ProductID = s.Field<int>("ProductID"),
                ProductName = s.Field<string>("ProductName"),
                cTotal = s.Field<string>("cTotal"),
                cTotalAll = s.Field<string>("cTotalAll"),
                nGroupCalc = s.Field<int>("nGroupCalc"),
                nOrder = s.Field<int>("nOrder"),
                sUnit = s.Field<string>("sUnit"),
                sType = s.Field<string>("sType"),
                FormID = s.Field<int>("FormID"),
                UnitID = s.Field<int>("UnitID"),
                M1 = s.Field<string>("sOption") == "M" ? s.Field<string>("M1") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M2 = s.Field<string>("sOption") == "M" ? s.Field<string>("M2") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M3 = s.Field<string>("sOption") == "M" ? s.Field<string>("M3") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M4 = s.Field<string>("sOption") == "M" ? s.Field<string>("M4") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M5 = s.Field<string>("sOption") == "M" ? s.Field<string>("M5") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M6 = s.Field<string>("sOption") == "M" ? s.Field<string>("M6") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M7 = s.Field<string>("sOption") == "M" ? s.Field<string>("M7") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M8 = s.Field<string>("sOption") == "M" ? s.Field<string>("M8") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M9 = s.Field<string>("sOption") == "M" ? s.Field<string>("M9") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M10 = s.Field<string>("sOption") == "M" ? s.Field<string>("M10") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M11 = s.Field<string>("sOption") == "M" ? s.Field<string>("M11") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M12 = s.Field<string>("sOption") == "M" ? s.Field<string>("M12") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                nTotal = s.Field<string>("nTotal"),
                Target = s.Field<string>("Target")
            }).OrderBy(o => o.nOrder).ToList();
        }

        #region old data voc
        /*var qDataVOC = (from i in db.mTProductIndicator.Where(w => w.IDIndicator == IndID && w.sType == "VOC").AsEnumerable()
                        from d in db.TEmission_VOC.Where(w => w.ProductID == i.ProductID && w.FormID == nEPIFromID).AsEnumerable().DefaultIfEmpty()
                        select new ClassExecute.TData_Emission
                        {
                            ProductID = i.ProductID,
                            ProductName = i.ProductName,
                            cTotal = i.cTotal,
                            cTotalAll = i.cTotalAll,
                            nGroupCalc = i.nGroupCalc ?? 0,
                            nOrder = i.nOrder ?? 0,
                            sUnit = i.sUnit,
                            sType = i.sType,
                            FormID = d != null ? d.FormID : 0,
                            UnitID = d != null ? d.UnitID ?? 0 : 0,
                            M1 = d != null ? (d.sOption == "M" ? d.M1 : SystemFunction.sysDivideData(d.nTotal, "12") + "") : "",
                            M2 = d != null ? (d.sOption == "M" ? d.M2 : SystemFunction.sysDivideData(d.nTotal, "12") + "") : "",
                            M3 = d != null ? (d.sOption == "M" ? d.M3 : SystemFunction.sysDivideData(d.nTotal, "12") + "") : "",
                            M4 = d != null ? (d.sOption == "M" ? d.M4 : SystemFunction.sysDivideData(d.nTotal, "12") + "") : "",
                            M5 = d != null ? (d.sOption == "M" ? d.M5 : SystemFunction.sysDivideData(d.nTotal, "12") + "") : "",
                            M6 = d != null ? (d.sOption == "M" ? d.M6 : SystemFunction.sysDivideData(d.nTotal, "12") + "") : "",
                            M7 = d != null ? (d.sOption == "M" ? d.M7 : SystemFunction.sysDivideData(d.nTotal, "12") + "") : "",
                            M8 = d != null ? (d.sOption == "M" ? d.M8 : SystemFunction.sysDivideData(d.nTotal, "12") + "") : "",
                            M9 = d != null ? (d.sOption == "M" ? d.M9 : SystemFunction.sysDivideData(d.nTotal, "12") + "") : "",
                            M10 = d != null ? (d.sOption == "M" ? d.M10 : SystemFunction.sysDivideData(d.nTotal, "12") + "") : "",
                            M11 = d != null ? (d.sOption == "M" ? d.M11 : SystemFunction.sysDivideData(d.nTotal, "12") + "") : "",
                            M12 = d != null ? (d.sOption == "M" ? d.M12 : SystemFunction.sysDivideData(d.nTotal, "12") + "") : "",
                            nTotal = d != null ? d.nTotal : "",
                            Target = d != null ? d.Target : "",
                        }).OrderBy(o => o.nOrder).ToList();*/
        #endregion
        #endregion

        var qData = qDataSumStack.Concat(qDataVOC).ToList();
        return qData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nOperaID"></param>
    /// <param name="nFacID"></param>
    /// <param name="sYear"></param>
    /// <param name="IsConvertUnitToBarrel">เพื่แปลงค่า Volume ใส่คอลัมน์ nSpillVolume นำไปใช้ตอนคำนวณ </param>
    /// <returns></returns>
    public static List<ClassExecute.TData_Spill> GetDataSpillToCalculateOutput(int nOperaID, int nFacID, string sYear, bool IsConvertUnitToBarrel)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 9;
        int nEPIFromID = 0;
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == IndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (qEPIForm != null)
        {
            nEPIFromID = qEPIForm.FormID;
        }

        /*
         UnitVolumeID == 63 เป็น Liter
         UnitVolumeID == 64 เป็น Barrel
         UnitVolumeID == 65 เป็น M3
         */
        //Data Spill
        var query = db.TSpill.Where(w => w.FormID == nEPIFromID).AsEnumerable().Select(s => new ClassExecute.TData_Spill
        {
            nSpillID = s.nSpillID,
            PrimaryReasonID = s.PrimaryReasonID ?? 0,
            SpillType = s.SpillType,
            SpillOfID = s.SpillOfID ?? 0,
            Volume = s.Volume,
            UnitVolumeID = s.UnitVolumeID ?? 0,
            SpillToID = s.SpillToID ?? 0,
            SpillByID = s.SpillByID ?? 0,
            Signification1ID = s.Signification1ID,
            Signification2ID = s.Signification2ID,
            SpillDate = s.SpillDate,
            sIsSensitiveArea = s.sIsSensitiveArea,
            nSpillVolume = IsConvertUnitToBarrel ? (s.UnitVolumeID == 63 ? EPIFunc.ConvertLiterToBarrel(s.Volume) :
                                                   s.UnitVolumeID == 64 ? EPIFunc.GetDecimalNull(s.Volume) :
                                                   s.UnitVolumeID == 65 ? EPIFunc.ConvertM3ToBarrel(s.Volume) :
                                                   s.UnitVolumeID == 2 ? EPIFunc.ConvertLiterToBarrel((EPIFunc.GetDecimalNull(s.Volume) * EPIFunc.GetDecimalNull(s.Density)) + "") : SystemFunction.GetDecimalNull(s.Volume))
                                                   : SystemFunction.GetDecimalNull(s.Volume)
        }).ToList();

        return query;
    }

    public static List<ClassExecute.TData_Spill_Product> GetDataSpillProductToCalculateOutput(int nOperaID, int nFacID, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 9;
        int nEPIFromID = 0;
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == IndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (qEPIForm != null)
        {
            nEPIFromID = qEPIForm.FormID;
        }
        var query = (from i in db.mTProductIndicator.Where(w => w.IDIndicator == IndID && w.cDel == "N" && w.cTotal == "Y").AsEnumerable()
                     from d in db.TSpill_Product.Where(w => w.ProductID == i.ProductID && w.FormID == nEPIFromID).AsEnumerable().DefaultIfEmpty()
                     select new ClassExecute.TData_Spill_Product
                             {
                                 ProductID = i.ProductID,
                                 ProductName = i.ProductName,
                                 cTotal = i.cTotal,
                                 cTotalAll = i.cTotalAll,
                                 nGroupCalc = i.nGroupCalc ?? 0,
                                 nOrder = i.nOrder ?? 0,
                                 sUnit = i.sUnit,
                                 sType = i.sType,
                                 FormID = d != null ? d.FormID : 0,
                                 UnitID = d != null ? d.UnitID ?? 0 : 0,
                                 M1 = d != null ? d.M1 : "",
                                 M2 = d != null ? d.M2 : "",
                                 M3 = d != null ? d.M3 : "",
                                 M4 = d != null ? d.M4 : "",
                                 M5 = d != null ? d.M5 : "",
                                 M6 = d != null ? d.M6 : "",
                                 M7 = d != null ? d.M7 : "",
                                 M8 = d != null ? d.M8 : "",
                                 M9 = d != null ? d.M9 : "",
                                 M10 = d != null ? d.M10 : "",
                                 M11 = d != null ? d.M11 : "",
                                 M12 = d != null ? d.M12 : "",
                                 nTotal = d != null ? d.nTotal : "",
                                 Target = d != null ? d.Target : "",
                             }).OrderBy(o => o.nOrder).ToList();

        return query;
    }

    public static List<ClassExecute.TData_Complaint_Product> GetDataComplaintToCalculateOutput(int nOperaID, int nFacID, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 1;
        int nEPIFromID = 0;
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == IndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (qEPIForm != null)
        {
            nEPIFromID = qEPIForm.FormID;
        }

        var qData = (from i in db.mTProductIndicator.Where(w => w.IDIndicator == IndID && w.cDel == "N" && w.cTotal == "Y").AsEnumerable()
                     from d in db.TComplaint_Product.Where(w => w.ProductID == i.ProductID && w.FormID == nEPIFromID).AsEnumerable().DefaultIfEmpty()
                     select new ClassExecute.TData_Complaint_Product
                     {
                         ProductID = i.ProductID,
                         ProductName = i.ProductName,
                         cTotal = i.cTotal,
                         cTotalAll = i.cTotalAll,
                         nGroupCalc = i.nGroupCalc ?? 0,
                         nOrder = i.nOrder ?? 0,
                         sUnit = i.sUnit,
                         sType = i.sType,
                         FormID = d != null ? d.FormID : 0,
                         UnitID = d != null ? d.UnitID ?? 0 : 0,
                         M1 = d != null ? d.M1 : "",
                         M2 = d != null ? d.M2 : "",
                         M3 = d != null ? d.M3 : "",
                         M4 = d != null ? d.M4 : "",
                         M5 = d != null ? d.M5 : "",
                         M6 = d != null ? d.M6 : "",
                         M7 = d != null ? d.M7 : "",
                         M8 = d != null ? d.M8 : "",
                         M9 = d != null ? d.M9 : "",
                         M10 = d != null ? d.M10 : "",
                         M11 = d != null ? d.M11 : "",
                         M12 = d != null ? d.M12 : "",
                         nTotal = d != null ? d.nTotal : "",
                         Target = d != null ? d.Target : "",
                     }).OrderBy(o => o.nOrder).ToList();

        return qData;
    }

    public static List<ClassExecute.TData_Compliance_Product> GetDataComplianceToCalculateOutput(int nOperaID, int nFacID, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 2;
        int nEPIFromID = 0;
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == IndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (qEPIForm != null)
        {
            nEPIFromID = qEPIForm.FormID;
        }

        var qData = (from i in db.mTProductIndicator.Where(w => w.IDIndicator == IndID && w.cDel == "N" && w.cTotal == "Y").AsEnumerable()
                     from d in db.TCompliance_Product.Where(w => w.ProductID == i.ProductID && w.FormID == nEPIFromID).AsEnumerable().DefaultIfEmpty()
                     select new ClassExecute.TData_Compliance_Product
                     {
                         ProductID = i.ProductID,
                         ProductName = i.ProductName,
                         cTotal = i.cTotal,
                         cTotalAll = i.cTotalAll,
                         nGroupCalc = i.nGroupCalc ?? 0,
                         nOrder = i.nOrder ?? 0,
                         sUnit = i.sUnit,
                         sType = i.sType,
                         FormID = d != null ? d.FormID : 0,
                         UnitID = d != null ? d.UnitID ?? 0 : 0,
                         M1 = d != null ? d.M1 : "",
                         M2 = d != null ? d.M2 : "",
                         M3 = d != null ? d.M3 : "",
                         M4 = d != null ? d.M4 : "",
                         M5 = d != null ? d.M5 : "",
                         M6 = d != null ? d.M6 : "",
                         M7 = d != null ? d.M7 : "",
                         M8 = d != null ? d.M8 : "",
                         M9 = d != null ? d.M9 : "",
                         M10 = d != null ? d.M10 : "",
                         M11 = d != null ? d.M11 : "",
                         M12 = d != null ? d.M12 : "",
                         nTotal = d != null ? d.nTotal : "",
                         Target = d != null ? d.Target : "",
                     }).OrderBy(o => o.nOrder).ToList();

        return qData;
    }

    /// <summary>
    /// Product Output
    /// สำหรับ Tab Output Indicator Waste จะไม่นำ sType = R ไปเนื่องจากเป็นส่วนที่ใช้ในตอนออกรายงาน(EPI Report) เท่านั้น
    /// </summary>
    /// <param name="nIndID"></param>
    /// <param name="nOperaID"></param>
    /// <param name="nFacID"></param>
    /// <param name="sYear"></param>
    /// <returns></returns>
    public static List<ClassExecute.TDataOutput> GetDataOutput(int nIndID, int nOperaID, int nFacID, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int nEPIFromID = 0;
        var query = db.TEPI_Forms.Where(w => w.IDIndicator == nIndID && w.OperationTypeID == nOperaID && w.FacilityID == nFacID && w.sYear == sYear).FirstOrDefault();
        nEPIFromID = query != null ? query.FormID : 0;

        if (nIndID != 9 && nIndID != 1 && nIndID != 2)
        {
            string sCondition = "";
            if (nIndID == EPIFunc.nIndWasteID)
            {
                sCondition = " and isnull(a.sType,'') <> 'R' "; //เพื่อไม่ให้ดึง Product ที่ใช้งานใน EPI Report มาออกใน tab output
            }

            #region None Spill
            string sql = @"select a.IDIndicator,a.ProductID,a.ProductName,b.nOrder,a.sUnit,
ISNULL(c.UnitID,0) 'UnitID',
d.FormID,d.M1,d.M2,d.M3,d.M4,d.M5,d.M6,d.M7,d.M8,d.M9,d.M10,d.M11,d.M12,d.Target,d.nTotal,d.Q1,d.Q2,d.Q3,d.Q4,d.H1,d.H2,
f.ID 'TooptipID',f.Name 'TooltipName'
from mTProductIndicatorOutput a
inner join TUseProductOutput b on a.ProductID = b.ProductID
left join mTUnit c on a.sUnit = c.UnitName
left join TProductOutput d on d.ProductID = a.ProductID and d.FormID = {0}
left join TProduct_Tooltip e on b.ProductID = e.ProductID and b.IDIndicator = e.IDIndicator and e.cType = '2'
left join TTooltip_Product f on f.ID = e.TooltipID
where a.IDIndicator = {1} and b.OperationtypeID = {2} " + sCondition + @"
order by b.nOrder";

            List<ClassExecute.TDataOutput> lstdata = new List<ClassExecute.TDataOutput>();
            DataTable dt = new DataTable();
            dt = CommonFunction.Get_Data(SystemFunction.strConnect, string.Format(sql, nEPIFromID, nIndID, nOperaID));
            if (dt.Rows.Count > 0)
            {
                lstdata = dt.AsEnumerable().Select(s => new ClassExecute.TDataOutput
                {
                    IDIndicator = s.Field<int>("IDIndicator"),
                    OperationtypeID = nOperaID,
                    FacilityID = nFacID,
                    ProductID = s.Field<int>("ProductID"),
                    ProductName = s.Field<string>("ProductName"),
                    nUnitID = s.Field<int>("UnitID"),
                    sUnit = s.Field<string>("sUnit"),
                    nOrder = s.Field<decimal>("nOrder"),

                    nTarget = SystemFunction.GetDecimalNull(s.Field<string>("Target")),
                    nM1 = SystemFunction.GetDecimalNull(s.Field<string>("M1")),
                    nM2 = SystemFunction.GetDecimalNull(s.Field<string>("M2")),
                    nM3 = SystemFunction.GetDecimalNull(s.Field<string>("M3")),
                    nM4 = SystemFunction.GetDecimalNull(s.Field<string>("M4")),
                    nM5 = SystemFunction.GetDecimalNull(s.Field<string>("M5")),
                    nM6 = SystemFunction.GetDecimalNull(s.Field<string>("M6")),
                    nM7 = SystemFunction.GetDecimalNull(s.Field<string>("M7")),
                    nM8 = SystemFunction.GetDecimalNull(s.Field<string>("M8")),
                    nM9 = SystemFunction.GetDecimalNull(s.Field<string>("M9")),
                    nM10 = SystemFunction.GetDecimalNull(s.Field<string>("M10")),
                    nM11 = SystemFunction.GetDecimalNull(s.Field<string>("M11")),
                    nM12 = SystemFunction.GetDecimalNull(s.Field<string>("M12")),
                    nTotal = SystemFunction.GetDecimalNull(s.Field<string>("nTotal")),
                    nQ1 = SystemFunction.GetDecimalNull(s.Field<string>("Q1")),
                    nQ2 = SystemFunction.GetDecimalNull(s.Field<string>("Q2")),
                    nQ3 = SystemFunction.GetDecimalNull(s.Field<string>("Q3")),
                    nQ4 = SystemFunction.GetDecimalNull(s.Field<string>("Q4")),
                    nH1 = SystemFunction.GetDecimalNull(s.Field<string>("H1")),
                    nH2 = SystemFunction.GetDecimalNull(s.Field<string>("H2")),

                    sTarget = SystemFunction.ConvertFormatDecimal4(s.Field<string>("Target")),
                    sM1 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("M1")),
                    sM2 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("M2")),
                    sM3 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("M3")),
                    sM4 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("M4")),
                    sM5 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("M5")),
                    sM6 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("M6")),
                    sM7 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("M7")),
                    sM8 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("M8")),
                    sM9 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("M9")),
                    sM10 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("M10")),
                    sM11 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("M11")),
                    sM12 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("M12")),
                    sTotal = SystemFunction.ConvertFormatDecimal4(s.Field<string>("nTotal")),
                    sQ1 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("Q1")),
                    sQ2 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("Q2")),
                    sQ3 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("Q3")),
                    sQ4 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("Q4")),
                    sH1 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("H1")),
                    sH2 = SystemFunction.ConvertFormatDecimal4(s.Field<string>("H2")),

                    TooptipID = s.Field<int?>("TooptipID") != null ? s.Field<int?>("TooptipID") + "" : "N",
                    TooltipName = s.Field<string>("TooltipName")
                }).ToList();
            }

            #region old code
            /* var qData = db.Database.SqlQuery<ClassExecute.TDataExcuteOutput>(string.Format(sql, CommonFunction.ReplaceInjection(nEPIFromID + ""), CommonFunction.ReplaceInjection(nIndID + ""), CommonFunction.ReplaceInjection(nOperaID + ""))).Select(s => new ClassExecute.TDataOutput
            {
                IDIndicator = s.IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = s.ProductID,
                ProductName = s.ProductName,
                nUnitID = s.UnitID,
                sUnit = s.sUnit,
                nOrder = s.nOrder,

                nTarget = SystemFunction.GetDecimalNull(s.Target),
                nM1 = SystemFunction.GetDecimalNull(s.M1),
                nM2 = SystemFunction.GetDecimalNull(s.M2),
                nM3 = SystemFunction.GetDecimalNull(s.M3),
                nM4 = SystemFunction.GetDecimalNull(s.M4),
                nM5 = SystemFunction.GetDecimalNull(s.M5),
                nM6 = SystemFunction.GetDecimalNull(s.M6),
                nM7 = SystemFunction.GetDecimalNull(s.M7),
                nM8 = SystemFunction.GetDecimalNull(s.M8),
                nM9 = SystemFunction.GetDecimalNull(s.M9),
                nM10 = SystemFunction.GetDecimalNull(s.M10),
                nM11 = SystemFunction.GetDecimalNull(s.M11),
                nM12 = SystemFunction.GetDecimalNull(s.M12),
                nTotal = SystemFunction.GetDecimalNull(s.nTotal),
                nQ1 = SystemFunction.GetDecimalNull(s.Q1),
                nQ2 = SystemFunction.GetDecimalNull(s.Q2),
                nQ3 = SystemFunction.GetDecimalNull(s.Q3),
                nQ4 = SystemFunction.GetDecimalNull(s.Q4),
                nH1 = SystemFunction.GetDecimalNull(s.H1),
                nH2 = SystemFunction.GetDecimalNull(s.H2),

                sTarget = SystemFunction.ConvertFormatDecimal4(s.Target),
                sM1 = SystemFunction.ConvertFormatDecimal4(s.M1),
                sM2 = SystemFunction.ConvertFormatDecimal4(s.M2),
                sM3 = SystemFunction.ConvertFormatDecimal4(s.M3),
                sM4 = SystemFunction.ConvertFormatDecimal4(s.M4),
                sM5 = SystemFunction.ConvertFormatDecimal4(s.M5),
                sM6 = SystemFunction.ConvertFormatDecimal4(s.M6),
                sM7 = SystemFunction.ConvertFormatDecimal4(s.M7),
                sM8 = SystemFunction.ConvertFormatDecimal4(s.M8),
                sM9 = SystemFunction.ConvertFormatDecimal4(s.M9),
                sM10 = SystemFunction.ConvertFormatDecimal4(s.M10),
                sM11 = SystemFunction.ConvertFormatDecimal4(s.M11),
                sM12 = SystemFunction.ConvertFormatDecimal4(s.M12),
                sTotal = SystemFunction.ConvertFormatDecimal4(s.nTotal),
                sQ1 = SystemFunction.ConvertFormatDecimal4(s.Q1),
                sQ2 = SystemFunction.ConvertFormatDecimal4(s.Q2),
                sQ3 = SystemFunction.ConvertFormatDecimal4(s.Q3),
                sQ4 = SystemFunction.ConvertFormatDecimal4(s.Q4),
                sH1 = SystemFunction.ConvertFormatDecimal4(s.H1),
                sH2 = SystemFunction.ConvertFormatDecimal4(s.H2),

                TooptipID = s.TooptipID != null ? s.TooptipID + "" : "N",
                TooltipName = s.TooltipName

            }).ToList();*/
            #endregion

            return lstdata;
            #endregion
        }
        else if (nIndID == 9) //Spill >> เนื่องจากมีการแสดงผลที่ต่างจาก Indicator อื่นๆ
        {

            List<ClassExecute.TDataOutput> lstOutput1 = new List<ClassExecute.TDataOutput>();
            #region output from data spill proudct input
            string sql = @"select a.IDIndicator,a.ProductID,a.ProductName,a.nOrder,a.sUnit,
ISNULL(c.UnitID,0) 'UnitID',
d.FormID,d.M1,d.M2,d.M3,d.M4,d.M5,d.M6,d.M7,d.M8,d.M9,d.M10,d.M11,d.M12,d.Target,d.nTotal,d.Q1,d.Q2,d.Q3,d.Q4,d.H1,d.H2,
f.ID 'TooptipID',f.Name 'TooltipName'
from mTProductIndicatorOutput a
left join mTUnit c on a.sUnit = c.UnitName
left join TProductOutput d on d.ProductID = a.ProductID and d.FormID = {0}
left join TProduct_Tooltip e on a.ProductID = e.ProductID and a.IDIndicator = e.IDIndicator and e.cType = '2'
left join TTooltip_Product f on f.ID = e.TooltipID
where a.IDIndicator = {1} and a.sType = '0'
order by a.nOrder";

            lstOutput1 = db.Database.SqlQuery<ClassExecute.TDataExcuteOutput>(string.Format(sql, nEPIFromID, nIndID)).Select(s => new ClassExecute.TDataOutput
            {
                sMakeField1 = "",
                sMakeField2 = "0",
                sMakeField3 = "0",
                IDIndicator = s.IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = s.ProductID,
                ProductName = s.ProductName,
                nUnitID = s.UnitID,
                sUnit = s.sUnit,
                nOrder = s.nOrder,

                nTarget = SystemFunction.GetDecimalNull(s.Target),
                nM1 = SystemFunction.GetDecimalNull(s.M1),
                nM2 = SystemFunction.GetDecimalNull(s.M2),
                nM3 = SystemFunction.GetDecimalNull(s.M3),
                nM4 = SystemFunction.GetDecimalNull(s.M4),
                nM5 = SystemFunction.GetDecimalNull(s.M5),
                nM6 = SystemFunction.GetDecimalNull(s.M6),
                nM7 = SystemFunction.GetDecimalNull(s.M7),
                nM8 = SystemFunction.GetDecimalNull(s.M8),
                nM9 = SystemFunction.GetDecimalNull(s.M9),
                nM10 = SystemFunction.GetDecimalNull(s.M10),
                nM11 = SystemFunction.GetDecimalNull(s.M11),
                nM12 = SystemFunction.GetDecimalNull(s.M12),
                nTotal = SystemFunction.GetDecimalNull(s.nTotal),
                nQ1 = SystemFunction.GetDecimalNull(s.Q1),
                nQ2 = SystemFunction.GetDecimalNull(s.Q2),
                nQ3 = SystemFunction.GetDecimalNull(s.Q3),
                nQ4 = SystemFunction.GetDecimalNull(s.Q4),
                nH1 = SystemFunction.GetDecimalNull(s.H1),
                nH2 = SystemFunction.GetDecimalNull(s.H2),

                sTarget = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Target, 0) : SystemFunction.ConvertFormatDecimal4(s.Target),
                sM1 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M1, 0) : SystemFunction.ConvertFormatDecimal4(s.M1),
                sM2 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M2, 0) : SystemFunction.ConvertFormatDecimal4(s.M2),
                sM3 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M3, 0) : SystemFunction.ConvertFormatDecimal4(s.M3),
                sM4 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M4, 0) : SystemFunction.ConvertFormatDecimal4(s.M4),
                sM5 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M5, 0) : SystemFunction.ConvertFormatDecimal4(s.M5),
                sM6 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M6, 0) : SystemFunction.ConvertFormatDecimal4(s.M6),
                sM7 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M7, 0) : SystemFunction.ConvertFormatDecimal4(s.M7),
                sM8 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M8, 0) : SystemFunction.ConvertFormatDecimal4(s.M8),
                sM9 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M9, 0) : SystemFunction.ConvertFormatDecimal4(s.M9),
                sM10 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M10, 0) : SystemFunction.ConvertFormatDecimal4(s.M10),
                sM11 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M11, 0) : SystemFunction.ConvertFormatDecimal4(s.M11),
                sM12 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M12, 0) : SystemFunction.ConvertFormatDecimal4(s.M12),
                sTotal = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.nTotal, 0) : SystemFunction.ConvertFormatDecimal4(s.nTotal),
                sQ1 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Q1, 0) : SystemFunction.ConvertFormatDecimal4(s.Q1),
                sQ2 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Q2, 0) : SystemFunction.ConvertFormatDecimal4(s.Q2),
                sQ3 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Q3, 0) : SystemFunction.ConvertFormatDecimal4(s.Q3),
                sQ4 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Q4, 0) : SystemFunction.ConvertFormatDecimal4(s.Q4),
                sH1 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.H1, 0) : SystemFunction.ConvertFormatDecimal4(s.H1),
                sH2 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.H2, 0) : SystemFunction.ConvertFormatDecimal4(s.H2),

                TooptipID = s.TooptipID != null ? s.TooptipID + "" : "N",
                TooltipName = s.TooltipName

            }).ToList();
            #endregion

            List<ClassExecute.TDataOutput> lstOutput2 = new List<ClassExecute.TDataOutput>();
            lstOutput2 = GetDataOuputSpill(nOperaID, nFacID, sYear, nEPIFromID);

            return lstOutput1.Concat(lstOutput2).ToList();
        }
        else if (nIndID == 1 || nIndID == 2)//Complaint,Compliance
        {
            #region output from data spill proudct input
            string sql = @"select a.IDIndicator,a.ProductID,a.ProductName,a.nOrder,a.sUnit,
ISNULL(c.UnitID,0) 'UnitID',
d.FormID,d.M1,d.M2,d.M3,d.M4,d.M5,d.M6,d.M7,d.M8,d.M9,d.M10,d.M11,d.M12,d.Target,d.nTotal,d.Q1,d.Q2,d.Q3,d.Q4,d.H1,d.H2,
f.ID 'TooptipID',f.Name 'TooltipName'
from mTProductIndicatorOutput a
left join mTUnit c on a.sUnit = c.UnitName
left join TProductOutput d on d.ProductID = a.ProductID and d.FormID = {0}
left join TProduct_Tooltip e on a.ProductID = e.ProductID and a.IDIndicator = e.IDIndicator and e.cType = '2'
left join TTooltip_Product f on f.ID = e.TooltipID
where a.IDIndicator = {1}
order by a.nOrder";

            var qData = db.Database.SqlQuery<ClassExecute.TDataExcuteOutput>(string.Format(sql, nEPIFromID, nIndID)).Select(s => new ClassExecute.TDataOutput
            {
                IDIndicator = s.IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = s.ProductID,
                ProductName = s.ProductName,
                nUnitID = s.UnitID,
                sUnit = s.sUnit,
                nOrder = s.nOrder,

                nTarget = SystemFunction.GetDecimalNull(s.Target),
                nM1 = SystemFunction.GetDecimalNull(s.M1),
                nM2 = SystemFunction.GetDecimalNull(s.M2),
                nM3 = SystemFunction.GetDecimalNull(s.M3),
                nM4 = SystemFunction.GetDecimalNull(s.M4),
                nM5 = SystemFunction.GetDecimalNull(s.M5),
                nM6 = SystemFunction.GetDecimalNull(s.M6),
                nM7 = SystemFunction.GetDecimalNull(s.M7),
                nM8 = SystemFunction.GetDecimalNull(s.M8),
                nM9 = SystemFunction.GetDecimalNull(s.M9),
                nM10 = SystemFunction.GetDecimalNull(s.M10),
                nM11 = SystemFunction.GetDecimalNull(s.M11),
                nM12 = SystemFunction.GetDecimalNull(s.M12),
                nTotal = SystemFunction.GetDecimalNull(s.nTotal),
                nQ1 = SystemFunction.GetDecimalNull(s.Q1),
                nQ2 = SystemFunction.GetDecimalNull(s.Q2),
                nQ3 = SystemFunction.GetDecimalNull(s.Q3),
                nQ4 = SystemFunction.GetDecimalNull(s.Q4),
                nH1 = SystemFunction.GetDecimalNull(s.H1),
                nH2 = SystemFunction.GetDecimalNull(s.H2),

                sTarget = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Target, 0) : SystemFunction.ConvertFormatDecimal4(s.Target),
                sM1 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M1 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.M1),
                sM2 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M2 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.M2),
                sM3 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M3 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.M3),
                sM4 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M4 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.M4),
                sM5 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M5 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.M5),
                sM6 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M6 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.M6),
                sM7 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M7 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.M7),
                sM8 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M8 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.M8),
                sM9 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M9 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.M9),
                sM10 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M10 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.M10),
                sM11 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M11 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.M11),
                sM12 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M12 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.M12),
                sTotal = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.nTotal + "", 0) : SystemFunction.ConvertFormatDecimal4(s.nTotal),
                sQ1 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Q1 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.Q1),
                sQ2 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Q2 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.Q2),
                sQ3 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Q3 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.Q3),
                sQ4 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Q4 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.Q4),
                sH1 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.H1 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.H1),
                sH2 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.H2 + "", 0) : SystemFunction.ConvertFormatDecimal4(s.H2),

                TooptipID = s.TooptipID != null ? s.TooptipID + "" : "N",
                TooltipName = s.TooltipName

            }).ToList();
            #endregion

            return qData;
        }
        else
        {
            List<ClassExecute.TDataOutput> lstOutputTemp = new List<ClassExecute.TDataOutput>();
            return lstOutputTemp;
        }

    }

    public static List<ClassExecute.TDataOutput> GetDataOuputSpill(int nOperaID, int nFacID, string sYear, int nFormID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();

        #region from table ouput
        string sql = @"select * from
(
	select a.sName 'sMakeField1','' 'sMakeField2',CONVERT(varchar,a.nID) 'sMakeField3',0'IDIndicator',0'ProductID',a.sName 'ProductName',0 'nOrder','' 'sUnit',0 'UnitID'
	,0 'FormID','' 'M1','' 'M2','' 'M3','' 'M4','' 'M5','' 'M6','' 'M7','' 'M8','' 'M9','' 'M10','' 'M11','' 'M12','' 'Target','' 'nTotal','' 'Q1','' 'Q2','' 'Q3', '' 'Q4','' 'H1','' 'H2'
	,NULL 'TooptipID',NULL 'TooltipName'
	from TData_Type a
	where a.IndicatorID = 9 and a.sType = 'SPILLKEY'
	union all
	select b.sName 'sMakeField1',case when a.ProductID < 256 then '1' else '2' end as sMakeField2,a.sType 'sMakeField3'
	,a.IDIndicator,a.ProductID,'&nbsp;&nbsp;' + a.ProductName 'ProductName',a.nOrder,a.sUnit,
	c.UnitID,
	d.FormID,d.M1,d.M2,d.M3,d.M4,d.M5,d.M6,d.M7,d.M8,d.M9,d.M10,d.M11,d.M12,d.Target,d.nTotal,d.Q1,d.Q2,d.Q3,d.Q4,d.H1,d.H2,
	f.ID 'TooptipID',f.Name 'TooltipName'
	from mTProductIndicatorOutput a
	inner join TData_Type b on a.sType = b.nID
	left join mTUnit c on a.sUnit = c.UnitName
	left join TProductOutput d on d.ProductID = a.ProductID and d.FormID = {0}
	left join TProduct_Tooltip e on a.ProductID = e.ProductID and a.IDIndicator = e.IDIndicator and e.cType = '2'
	left join TTooltip_Product f on f.ID = e.TooltipID
	where a.IDIndicator = 9
) as t1
order by t1.sMakeField3,t1.nOrder";


        int nRow = 0;
        var qData = db.Database.SqlQuery<ClassExecute.TDataExcuteOutput>(string.Format(sql, CommonFunction.ReplaceInjection(nFormID + ""))).Select(s => new ClassExecute.TDataOutput
        {
            sMakeField1 = s.sMakeField1,
            sMakeField2 = s.sMakeField2,
            sMakeField3 = s.sMakeField3,
            nMakeField1 = nRow++,

            IDIndicator = s.IDIndicator,
            OperationtypeID = nOperaID,
            FacilityID = nFacID,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nUnitID = s.UnitID,
            sUnit = s.sUnit,
            nOrder = s.nOrder,

            nTarget = SystemFunction.GetDecimalNull(s.Target),
            nM1 = SystemFunction.GetDecimalNull(s.M1),
            nM2 = SystemFunction.GetDecimalNull(s.M2),
            nM3 = SystemFunction.GetDecimalNull(s.M3),
            nM4 = SystemFunction.GetDecimalNull(s.M4),
            nM5 = SystemFunction.GetDecimalNull(s.M5),
            nM6 = SystemFunction.GetDecimalNull(s.M6),
            nM7 = SystemFunction.GetDecimalNull(s.M7),
            nM8 = SystemFunction.GetDecimalNull(s.M8),
            nM9 = SystemFunction.GetDecimalNull(s.M9),
            nM10 = SystemFunction.GetDecimalNull(s.M10),
            nM11 = SystemFunction.GetDecimalNull(s.M11),
            nM12 = SystemFunction.GetDecimalNull(s.M12),
            nTotal = SystemFunction.GetDecimalNull(s.nTotal),
            nQ1 = SystemFunction.GetDecimalNull(s.Q1),
            nQ2 = SystemFunction.GetDecimalNull(s.Q2),
            nQ3 = SystemFunction.GetDecimalNull(s.Q3),
            nQ4 = SystemFunction.GetDecimalNull(s.Q4),
            nH1 = SystemFunction.GetDecimalNull(s.H1),
            nH2 = SystemFunction.GetDecimalNull(s.H2),

            sTarget = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Target, 0) : SystemFunction.ConvertFormatDecimal3(s.Target),
            sM1 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M1, 0) : SystemFunction.ConvertFormatDecimal3(s.M1),
            sM2 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M2, 0) : SystemFunction.ConvertFormatDecimal3(s.M2),
            sM3 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M3, 0) : SystemFunction.ConvertFormatDecimal3(s.M3),
            sM4 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M4, 0) : SystemFunction.ConvertFormatDecimal3(s.M4),
            sM5 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M5, 0) : SystemFunction.ConvertFormatDecimal3(s.M5),
            sM6 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M6, 0) : SystemFunction.ConvertFormatDecimal3(s.M6),
            sM7 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M7, 0) : SystemFunction.ConvertFormatDecimal3(s.M7),
            sM8 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M8, 0) : SystemFunction.ConvertFormatDecimal3(s.M8),
            sM9 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M9, 0) : SystemFunction.ConvertFormatDecimal3(s.M9),
            sM10 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M10, 0) : SystemFunction.ConvertFormatDecimal3(s.M10),
            sM11 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M11, 0) : SystemFunction.ConvertFormatDecimal3(s.M11),
            sM12 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.M12, 0) : SystemFunction.ConvertFormatDecimal3(s.M12),
            sTotal = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.nTotal, 0) : SystemFunction.ConvertFormatDecimal3(s.nTotal),
            sQ1 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Q1, 0) : SystemFunction.ConvertFormatDecimal3(s.Q1),
            sQ2 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Q2, 0) : SystemFunction.ConvertFormatDecimal3(s.Q2),
            sQ3 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Q3, 0) : SystemFunction.ConvertFormatDecimal3(s.Q3),
            sQ4 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.Q4, 0) : SystemFunction.ConvertFormatDecimal3(s.Q4),
            sH1 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.H1, 0) : SystemFunction.ConvertFormatDecimal3(s.H1),
            sH2 = s.sUnit == "Cases" ? EPIFunc.ConvertFormatDecimal(s.H2, 0) : SystemFunction.ConvertFormatDecimal3(s.H2),

            TooptipID = s.TooptipID != null ? s.TooptipID + "" : "N",
            TooltipName = s.TooltipName

        }).ToList();
        #endregion

        return qData;
    }

    public static List<ClassExecute.TDataOutput> GetDataOutputIntensity(int nIndID, int nOperaID, int nFacID, string sYear)
    {
        int nMSIndID = 6;
        int nFormID = 0;
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataOutput> lstTemp = new List<ClassExecute.TDataOutput>();
        var queryDisplayOutput = EPIFunc.GetDataDisplayInputIntensity();
        var qDisplayOutput = queryDisplayOutput.Where(w => w.OperaID == nOperaID).FirstOrDefault();
        if (qDisplayOutput != null)
        {
            var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == nMSIndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
            if (qEPIForm != null)
            {
                nFormID = qEPIForm.FormID;
            }

            if (qDisplayOutput.nDisplayType == 1 || qDisplayOutput.nDisplayType == 2 || qDisplayOutput.nDisplayType == 4 || qDisplayOutput.nDisplayType == 6) //D1,D2,D4,D6
            {
                #region Query Data
                lstTemp = (from pi in db.mTProductIndicator.Where(w => w.IDIndicator == nMSIndID && w.cTotal == "Y").AsEnumerable()
                           from up in db.TIntensityUseProduct.Where(w => w.OperationTypeID == nOperaID && w.ProductID == pi.ProductID).AsEnumerable()
                           from d in db.TIntensityDominator.Where(w => w.ProductID == pi.ProductID && w.FormID == nFormID).AsEnumerable().DefaultIfEmpty()
                           select new ClassExecute.TDataOutput
                           {
                               IDIndicator = nMSIndID,
                               OperationtypeID = nOperaID,
                               FacilityID = nFacID,
                               ProductID = pi.ProductID,
                               ProductName = pi.ProductName,
                               nUnitID = 0,
                               sUnit = pi.sUnit,
                               nOrder = up.nOrder ?? 0,

                               nTarget = SystemFunction.GetDecimalNull(d != null ? GetTarget_TIntensity_Output(d.FormID, pi.ProductID, d.Target) : ""),
                               nM1 = SystemFunction.GetDecimalNull(d != null ? d.M1 : ""),
                               nM2 = SystemFunction.GetDecimalNull(d != null ? d.M2 : ""),
                               nM3 = SystemFunction.GetDecimalNull(d != null ? d.M3 : ""),
                               nM4 = SystemFunction.GetDecimalNull(d != null ? d.M4 : ""),
                               nM5 = SystemFunction.GetDecimalNull(d != null ? d.M5 : ""),
                               nM6 = SystemFunction.GetDecimalNull(d != null ? d.M6 : ""),
                               nM7 = SystemFunction.GetDecimalNull(d != null ? d.M7 : ""),
                               nM8 = SystemFunction.GetDecimalNull(d != null ? d.M8 : ""),
                               nM9 = SystemFunction.GetDecimalNull(d != null ? d.M9 : ""),
                               nM10 = SystemFunction.GetDecimalNull(d != null ? d.M10 : ""),
                               nM11 = SystemFunction.GetDecimalNull(d != null ? d.M11 : ""),
                               nM12 = SystemFunction.GetDecimalNull(d != null ? d.M12 : ""),
                               nTotal = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Total"),

                               nQ1 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q1"),
                               nQ2 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q2"),
                               nQ3 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q3"),
                               nQ4 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q4"),
                               nH1 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H1"),
                               nH2 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H2"),

                               sTarget = SystemFunction.ConvertFormatDecimal3(d != null ? GetTarget_TIntensity_Output(d.FormID, pi.ProductID, d.Target) /*d.Target*/ : ""),
                               sM1 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M1 : ""),
                               sM2 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M2 : ""),
                               sM3 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M3 : ""),
                               sM4 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M4 : ""),
                               sM5 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M5 : ""),
                               sM6 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M6 : ""),
                               sM7 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M7 : ""),
                               sM8 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M8 : ""),
                               sM9 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M9 : ""),
                               sM10 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M10 : ""),
                               sM11 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M11 : ""),
                               sM12 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M12 : ""),
                               sTotal = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Total") + ""),

                               sQ1 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q1") + ""),
                               sQ2 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q2") + ""),
                               sQ3 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q3") + ""),
                               sQ4 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q4") + ""),
                               sH1 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H1") + ""),
                               sH2 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H2") + ""),

                               TooptipID = "N", //s.TooptipID != null ? s.TooptipID + "" : "N",
                               TooltipName = ""

                           }).OrderBy(o => o.nOrder).ToList();
                #endregion
            }
            else if (qDisplayOutput.nDisplayType == 3)
            {
                #region Query Data
                lstTemp = (from pi in db.mTProductIndicator.Where(w => w.IDIndicator == nMSIndID).AsEnumerable()
                           from up in db.TIntensityUseProduct.Where(w => w.OperationTypeID == nOperaID && w.ProductID == pi.ProductID).AsEnumerable()
                           from d in db.TIntensityDominator.Where(w => w.ProductID == pi.ProductID && w.FormID == nFormID).AsEnumerable().DefaultIfEmpty()
                           select new ClassExecute.TDataOutput
                           {
                               IDIndicator = nMSIndID,
                               OperationtypeID = nOperaID,
                               FacilityID = nFacID,
                               ProductID = pi.ProductID,
                               ProductName = pi.ProductName,
                               nUnitID = 0,
                               sUnit = pi.sUnit,
                               nOrder = up.nOrder ?? 0,

                               nTarget = SystemFunction.GetDecimalNull(d != null ? GetTarget_TIntensity_Output(d.FormID, pi.ProductID, d.Target) : ""),
                               nM1 = SystemFunction.GetDecimalNull(d != null ? d.M1 : ""),
                               nM2 = SystemFunction.GetDecimalNull(d != null ? d.M2 : ""),
                               nM3 = SystemFunction.GetDecimalNull(d != null ? d.M3 : ""),
                               nM4 = SystemFunction.GetDecimalNull(d != null ? d.M4 : ""),
                               nM5 = SystemFunction.GetDecimalNull(d != null ? d.M5 : ""),
                               nM6 = SystemFunction.GetDecimalNull(d != null ? d.M6 : ""),
                               nM7 = SystemFunction.GetDecimalNull(d != null ? d.M7 : ""),
                               nM8 = SystemFunction.GetDecimalNull(d != null ? d.M8 : ""),
                               nM9 = SystemFunction.GetDecimalNull(d != null ? d.M9 : ""),
                               nM10 = SystemFunction.GetDecimalNull(d != null ? d.M10 : ""),
                               nM11 = SystemFunction.GetDecimalNull(d != null ? d.M11 : ""),
                               nM12 = SystemFunction.GetDecimalNull(d != null ? d.M12 : ""),
                               nTotal = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Total"),

                               nQ1 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q1"),
                               nQ2 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q2"),
                               nQ3 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q3"),
                               nQ4 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q4"),
                               nH1 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H1"),
                               nH2 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H2"),

                               sTarget = SystemFunction.ConvertFormatDecimal3(d != null ? GetTarget_TIntensity_Output(d.FormID, pi.ProductID, d.Target) : ""),
                               sM1 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M1 : ""),
                               sM2 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M2 : ""),
                               sM3 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M3 : ""),
                               sM4 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M4 : ""),
                               sM5 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M5 : ""),
                               sM6 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M6 : ""),
                               sM7 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M7 : ""),
                               sM8 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M8 : ""),
                               sM9 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M9 : ""),
                               sM10 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M10 : ""),
                               sM11 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M11 : ""),
                               sM12 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M12 : ""),
                               sTotal = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Total") + ""),

                               sQ1 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q1") + ""),
                               sQ2 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q2") + ""),
                               sQ3 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q3") + ""),
                               sQ4 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q4") + ""),
                               sH1 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H1") + ""),
                               sH2 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H2") + ""),

                               TooptipID = "N", //s.TooptipID != null ? s.TooptipID + "" : "N",
                               TooltipName = ""

                           }).OrderBy(o => o.nOrder).ToList();
                #endregion
            }
            else if (qDisplayOutput.nDisplayType == 5)
            {
                #region Query Data
                lstTemp = (from pi in db.mTProductIndicator.Where(w => w.IDIndicator == nMSIndID).AsEnumerable()
                           from up in db.TIntensityUseProduct.Where(w => w.OperationTypeID == nOperaID && w.ProductID == pi.ProductID).AsEnumerable()
                           from d in db.TIntensityDominator.Where(w => w.ProductID == pi.ProductID && w.FormID == nFormID).AsEnumerable().DefaultIfEmpty()
                           select new ClassExecute.TDataOutput
                           {
                               IDIndicator = nMSIndID,
                               OperationtypeID = nOperaID,
                               FacilityID = nFacID,
                               ProductID = pi.ProductID,
                               ProductName = pi.ProductName,
                               nUnitID = 0,
                               sUnit = pi.sUnit,
                               nOrder = up.nOrder ?? 0,

                               nTarget = SystemFunction.GetDecimalNull(d != null ? GetTarget_TIntensity_Output(d.FormID, pi.ProductID, d.Target) : ""),
                               nM1 = SystemFunction.GetDecimalNull(d != null ? d.M1 : ""),
                               nM2 = SystemFunction.GetDecimalNull(d != null ? d.M2 : ""),
                               nM3 = SystemFunction.GetDecimalNull(d != null ? d.M3 : ""),
                               nM4 = SystemFunction.GetDecimalNull(d != null ? d.M4 : ""),
                               nM5 = SystemFunction.GetDecimalNull(d != null ? d.M5 : ""),
                               nM6 = SystemFunction.GetDecimalNull(d != null ? d.M6 : ""),
                               nM7 = SystemFunction.GetDecimalNull(d != null ? d.M7 : ""),
                               nM8 = SystemFunction.GetDecimalNull(d != null ? d.M8 : ""),
                               nM9 = SystemFunction.GetDecimalNull(d != null ? d.M9 : ""),
                               nM10 = SystemFunction.GetDecimalNull(d != null ? d.M10 : ""),
                               nM11 = SystemFunction.GetDecimalNull(d != null ? d.M11 : ""),
                               nM12 = SystemFunction.GetDecimalNull(d != null ? d.M12 : ""),
                               nTotal = pi.ProductID == 86 ? SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "FTotal") : SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "TotalAvgPerson"),

                               nQ1 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q1AvgPerson"),
                               nQ2 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q2AvgPerson"),
                               nQ3 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q3AvgPerson"),
                               nQ4 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q4AvgPerson"),
                               nH1 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H1AvgPerson"),
                               nH2 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H2AvgPerson"),

                               sTarget = SystemFunction.ConvertFormatDecimal3(d != null ? GetTarget_TIntensity_Output(d.FormID, pi.ProductID, d.Target) : ""),
                               sM1 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M1 : ""),
                               sM2 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M2 : ""),
                               sM3 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M3 : ""),
                               sM4 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M4 : ""),
                               sM5 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M5 : ""),
                               sM6 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M6 : ""),
                               sM7 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M7 : ""),
                               sM8 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M8 : ""),
                               sM9 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M9 : ""),
                               sM10 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M10 : ""),
                               sM11 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M11 : ""),
                               sM12 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M12 : ""),
                               sTotal = SystemFunction.ConvertFormatDecimal3(pi.ProductID == 86 ? SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "FTotal") + "" : SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "TotalAvgPerson") + ""),

                               sQ1 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q1AvgPerson") + ""),
                               sQ2 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q2AvgPerson") + ""),
                               sQ3 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q3AvgPerson") + ""),
                               sQ4 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q4AvgPerson") + ""),
                               sH1 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H1AvgPerson") + ""),
                               sH2 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H2AvgPerson") + ""),

                               TooptipID = "N", //s.TooptipID != null ? s.TooptipID + "" : "N",
                               TooltipName = ""

                           }).OrderBy(o => o.nOrder).ToList();
                #endregion
            }
            else if (qDisplayOutput.nDisplayType == 7)
            {
                #region Query Data
                lstTemp = (from pi in db.mTProductIndicator.Where(w => w.IDIndicator == nMSIndID && w.cTotal == "Y").AsEnumerable()
                           from up in db.TIntensityUseProduct.Where(w => w.OperationTypeID == nOperaID && w.ProductID == pi.ProductID).AsEnumerable()
                           from d in db.TIntensityDominator.Where(w => w.ProductID == pi.ProductID && w.FormID == nFormID).AsEnumerable().DefaultIfEmpty()
                           select new ClassExecute.TDataOutput
                           {
                               IDIndicator = nMSIndID,
                               OperationtypeID = nOperaID,
                               FacilityID = nFacID,
                               ProductID = pi.ProductID,
                               ProductName = pi.ProductName,
                               nUnitID = 0,
                               sUnit = pi.sUnit,
                               nOrder = up.nOrder ?? 0,

                               nTarget = SystemFunction.GetDecimalNull(d != null ? GetTarget_TIntensity_Output(d.FormID, pi.ProductID, d.Target) : ""),
                               nM1 = SystemFunction.GetDecimalNull(d != null ? d.M1 : ""),
                               nM2 = SystemFunction.GetDecimalNull(d != null ? d.M2 : ""),
                               nM3 = SystemFunction.GetDecimalNull(d != null ? d.M3 : ""),
                               nM4 = SystemFunction.GetDecimalNull(d != null ? d.M4 : ""),
                               nM5 = SystemFunction.GetDecimalNull(d != null ? d.M5 : ""),
                               nM6 = SystemFunction.GetDecimalNull(d != null ? d.M6 : ""),
                               nM7 = SystemFunction.GetDecimalNull(d != null ? d.M7 : ""),
                               nM8 = SystemFunction.GetDecimalNull(d != null ? d.M8 : ""),
                               nM9 = SystemFunction.GetDecimalNull(d != null ? d.M9 : ""),
                               nM10 = SystemFunction.GetDecimalNull(d != null ? d.M10 : ""),
                               nM11 = SystemFunction.GetDecimalNull(d != null ? d.M11 : ""),
                               nM12 = SystemFunction.GetDecimalNull(d != null ? d.M12 : ""),
                               nTotal = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Total"),

                               nQ1 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q1"),
                               nQ2 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q2"),
                               nQ3 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q3"),
                               nQ4 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q4"),
                               nH1 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H1"),
                               nH2 = SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H2"),

                               sTarget = SystemFunction.ConvertFormatDecimal3(d != null ? GetTarget_TIntensity_Output(d.FormID, pi.ProductID, d.Target) /*d.Target*/ : ""),
                               sM1 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M1 : ""),
                               sM2 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M2 : ""),
                               sM3 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M3 : ""),
                               sM4 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M4 : ""),
                               sM5 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M5 : ""),
                               sM6 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M6 : ""),
                               sM7 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M7 : ""),
                               sM8 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M8 : ""),
                               sM9 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M9 : ""),
                               sM10 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M10 : ""),
                               sM11 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M11 : ""),
                               sM12 = SystemFunction.ConvertFormatDecimal3(d != null ? d.M12 : ""),
                               sTotal = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Total") + ""),

                               sQ1 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q1") + ""),
                               sQ2 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q2") + ""),
                               sQ3 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q3") + ""),
                               sQ4 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "Q4") + ""),
                               sH1 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H1") + ""),
                               sH2 = SystemFunction.ConvertFormatDecimal3(SysFunctionCalculate.SumValueToOutput(d != null ? d : null, "H2") + ""),

                               TooptipID = "N", //s.TooptipID != null ? s.TooptipID + "" : "N",
                               TooltipName = ""

                           }).OrderBy(o => o.nOrder).ToList();
                #endregion
            }
        }


        return lstTemp;
    }

    public static decimal? GetValueFromListTDataOutput(ClassExecute.TDataOutput dt, int nMont)
    {
        decimal? nRetrun = null;
        switch (nMont)
        {
            case 1: nRetrun = dt.nM1; break;
            case 2: nRetrun = dt.nM2; break;
            case 3: nRetrun = dt.nM3; break;
            case 4: nRetrun = dt.nM4; break;
            case 5: nRetrun = dt.nM5; break;
            case 6: nRetrun = dt.nM6; break;
            case 7: nRetrun = dt.nM7; break;
            case 8: nRetrun = dt.nM8; break;
            case 9: nRetrun = dt.nM9; break;
            case 10: nRetrun = dt.nM10; break;
            case 11: nRetrun = dt.nM11; break;
            case 12: nRetrun = dt.nM12; break;
        }
        return nRetrun;
    }

    public static List<ClassExecute.TDataRole> GetRoleUser(string sUID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int nUID = SystemFunction.ParseInt(sUID);
        List<ClassExecute.TDataRole> lsTemp = new List<ClassExecute.TDataRole>();
        lsTemp = (from uir in db.mTUserInRole.Where(w => w.nUID == nUID)
                  from ur in db.mTUserRole.Where(w => w.cDel == "N" && w.cActive == "Y" && w.ID == uir.nRoleID)
                  select new ClassExecute.TDataRole
                  {
                      nRoleID = ur.ID,
                      sRoleName = ur.Name
                  }).OrderBy(o => o.sRoleName).ToList();
        return lsTemp;
    }

    public static DataTable GetDataOperationType()
    {
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, "select  * from mOperationType where cActive = 'Y' and cDel = 'N' order by Name");
        return dt;
    }

    public static DataTable ListDataCommentToPopUp(string sOperaID, string sFacID, string sIndID, string sYear, string sQuater)
    {
        string condition = " and a2.nQuater = " + CommonFunction.ReplaceInjection(sQuater);

        string sql = @"select a.FormID,a2.nQuater,c.sStatusName,b.sComment,b.dAction,d.Firstname + ' ' + d.Lastname 'FullName'
,case when b.nNextStepID = -1 then 'Operational User' else d2.Firstname + ' ' + d2.Lastname end as FullNameNext
,c.sColor 
from TEPI_Forms a
inner join TEPI_Workflow a2 on a.FormID = a2.FormID
inner join TEPI_LogWorkflow b on a.FormID = b.FormID and b.nWorkFlowID = a2.nWorkFlowID
inner join TStatus_Workflow c on c.nStatustID = b.nStatusID
inner join mTUser d on b.nActionBy = d.ID
left join mTUser d2 on b.nNextStepID = d2.ID
where a.FacilityID = " + CommonFunction.ReplaceInjection(sFacID) + " and a.sYear = '" + CommonFunction.ReplaceInjection(sYear + "") + @"' and IDIndicator = " + CommonFunction.ReplaceInjection(sIndID) + " and OperationTypeID = " + sOperaID + (!string.IsNullOrEmpty(sQuater) ? condition : "") + @" 
and c.cActive = 'Y' order by b.dAction desc";

        DataTable dt = new DataTable();
        if (SystemFunction.IsNumberic(sFacID) && SystemFunction.IsNumberic(sIndID))
        {
            dt = CommonFunction.Get_Data(SystemFunction.strConnect, sql);
        }

        return dt;
    }

    public static List<ClassExecute.TDataOutput> GetDataMaterialToPieChart(int nIndID, int nOperaID, int nFacID, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int nEPIFromID = 0;
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == nIndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (qEPIForm != null)
        {
            nEPIFromID = qEPIForm.FormID;
        }

        var qData = (from i in db.mTProductIndicator.AsEnumerable().Where(w => w.IDIndicator.Value == nIndID && (w.ProductID == 34 || w.ProductID == 37))
                     from s in db.TMaterial_Product.Where(w => w.ProductID == i.ProductID && w.FormID == nEPIFromID).AsEnumerable().DefaultIfEmpty()
                     select new ClassExecute.TDataOutput
                     {
                         IDIndicator = i.IDIndicator ?? 0,
                         OperationtypeID = nOperaID,
                         FacilityID = nFacID,
                         ProductID = i.ProductID,
                         ProductName = i.ProductName,
                         nUnitID = 0,
                         sUnit = i.sUnit,
                         nOrder = i.nOrder ?? 0,

                         nTarget = s != null ? SystemFunction.GetDecimalNull(s.Target) : null,
                         nM1 = s != null ? SystemFunction.GetDecimalNull(s.M1) : null,
                         nM2 = s != null ? SystemFunction.GetDecimalNull(s.M2) : null,
                         nM3 = s != null ? SystemFunction.GetDecimalNull(s.M3) : null,
                         nM4 = s != null ? SystemFunction.GetDecimalNull(s.M4) : null,
                         nM5 = s != null ? SystemFunction.GetDecimalNull(s.M5) : null,
                         nM6 = s != null ? SystemFunction.GetDecimalNull(s.M6) : null,
                         nM7 = s != null ? SystemFunction.GetDecimalNull(s.M7) : null,
                         nM8 = s != null ? SystemFunction.GetDecimalNull(s.M8) : null,
                         nM9 = s != null ? SystemFunction.GetDecimalNull(s.M9) : null,
                         nM10 = s != null ? SystemFunction.GetDecimalNull(s.M10) : null,
                         nM11 = s != null ? SystemFunction.GetDecimalNull(s.M11) : null,
                         nM12 = s != null ? SystemFunction.GetDecimalNull(s.M12) : null,
                         nTotal = s != null ? SystemFunction.GetDecimalNull(s.nTotal) : null,
                         nQ1 = s != null ? SystemFunction.GetDecimalNull(s.Q1) : null,
                         nQ2 = s != null ? SystemFunction.GetDecimalNull(s.Q2) : null,
                         nQ3 = s != null ? SystemFunction.GetDecimalNull(s.Q3) : null,
                         nQ4 = s != null ? SystemFunction.GetDecimalNull(s.Q4) : null,
                         nH1 = s != null ? SystemFunction.GetDecimalNull(s.H1) : null,
                         nH2 = s != null ? SystemFunction.GetDecimalNull(s.H2) : null,

                         sTarget = s != null ? SystemFunction.ConvertFormatDecimal3(s.Target) : "",
                         sM1 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M1) : "",
                         sM2 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M2) : "",
                         sM3 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M3) : "",
                         sM4 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M4) : "",
                         sM5 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M5) : "",
                         sM6 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M6) : "",
                         sM7 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M7) : "",
                         sM8 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M8) : "",
                         sM9 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M9) : "",
                         sM10 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M10) : "",
                         sM11 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M11) : "",
                         sM12 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M12) : "",
                         sTotal = s != null ? SystemFunction.ConvertFormatDecimal3(s.nTotal) : "",
                         sQ1 = s != null ? SystemFunction.ConvertFormatDecimal3(s.Q1) : "",
                         sQ2 = s != null ? SystemFunction.ConvertFormatDecimal3(s.Q2) : "",
                         sQ3 = s != null ? SystemFunction.ConvertFormatDecimal3(s.Q3) : "",
                         sQ4 = s != null ? SystemFunction.ConvertFormatDecimal3(s.Q4) : "",
                         sH1 = s != null ? SystemFunction.ConvertFormatDecimal3(s.H1) : "",
                         sH2 = s != null ? SystemFunction.ConvertFormatDecimal3(s.H2) : "",
                     }).OrderBy(o => o.nOrder).ToList();

        return qData;
    }

    public static List<ClassExecute.TDataOutput> GetDataWaterToPieChart(int nIndID, int nOperaID, int nFacID, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int nEPIFromID = 0;
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == nIndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (qEPIForm != null)
        {
            nEPIFromID = qEPIForm.FormID;
        }

        var qData = (from i in db.mTProductIndicator.AsEnumerable().Where(w => w.IDIndicator.Value == nIndID && w.cTotal == "N" && w.sType == "2")
                     from s in db.TWater_Product.Where(w => w.ProductID == i.ProductID && w.FormID == nEPIFromID).AsEnumerable().DefaultIfEmpty()
                     select new ClassExecute.TDataOutput
                     {
                         IDIndicator = i.IDIndicator ?? 0,
                         OperationtypeID = nOperaID,
                         FacilityID = nFacID,
                         ProductID = i.ProductID,
                         ProductName = i.ProductName,
                         nUnitID = 0,
                         sUnit = i.sUnit,
                         nOrder = i.nOrder ?? 0,

                         nTarget = s != null ? SystemFunction.GetDecimalNull(s.Target) : null,
                         nM1 = s != null ? SystemFunction.GetDecimalNull(s.M1) : null,
                         nM2 = s != null ? SystemFunction.GetDecimalNull(s.M2) : null,
                         nM3 = s != null ? SystemFunction.GetDecimalNull(s.M3) : null,
                         nM4 = s != null ? SystemFunction.GetDecimalNull(s.M4) : null,
                         nM5 = s != null ? SystemFunction.GetDecimalNull(s.M5) : null,
                         nM6 = s != null ? SystemFunction.GetDecimalNull(s.M6) : null,
                         nM7 = s != null ? SystemFunction.GetDecimalNull(s.M7) : null,
                         nM8 = s != null ? SystemFunction.GetDecimalNull(s.M8) : null,
                         nM9 = s != null ? SystemFunction.GetDecimalNull(s.M9) : null,
                         nM10 = s != null ? SystemFunction.GetDecimalNull(s.M10) : null,
                         nM11 = s != null ? SystemFunction.GetDecimalNull(s.M11) : null,
                         nM12 = s != null ? SystemFunction.GetDecimalNull(s.M12) : null,
                         nTotal = s != null ? EPIFunc.SumDataToDecimal(s.M1, s.M2, s.M3, s.M4, s.M5, s.M6, s.M7, s.M8, s.M9, s.M10, s.M11, s.M12) : null,
                         nQ1 = s != null ? SystemFunction.GetDecimalNull(s.Q1) : null,
                         nQ2 = s != null ? SystemFunction.GetDecimalNull(s.Q2) : null,
                         nQ3 = s != null ? SystemFunction.GetDecimalNull(s.Q3) : null,
                         nQ4 = s != null ? SystemFunction.GetDecimalNull(s.Q4) : null,
                         nH1 = s != null ? SystemFunction.GetDecimalNull(s.H1) : null,
                         nH2 = s != null ? SystemFunction.GetDecimalNull(s.H2) : null,

                         sTarget = s != null ? SystemFunction.ConvertFormatDecimal3(s.Target) : "",
                         sM1 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M1) : "",
                         sM2 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M2) : "",
                         sM3 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M3) : "",
                         sM4 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M4) : "",
                         sM5 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M5) : "",
                         sM6 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M6) : "",
                         sM7 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M7) : "",
                         sM8 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M8) : "",
                         sM9 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M9) : "",
                         sM10 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M10) : "",
                         sM11 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M11) : "",
                         sM12 = s != null ? SystemFunction.ConvertFormatDecimal3(s.M12) : "",
                         sTotal = s != null ? SystemFunction.ConvertFormatDecimal3(EPIFunc.SumDataToDecimal(s.M1, s.M2, s.M3, s.M4, s.M5, s.M6, s.M7, s.M8, s.M9, s.M10, s.M11, s.M12) + "") : "",
                         sQ1 = s != null ? SystemFunction.ConvertFormatDecimal3(s.Q1) : "",
                         sQ2 = s != null ? SystemFunction.ConvertFormatDecimal3(s.Q2) : "",
                         sQ3 = s != null ? SystemFunction.ConvertFormatDecimal3(s.Q3) : "",
                         sQ4 = s != null ? SystemFunction.ConvertFormatDecimal3(s.Q4) : "",
                         sH1 = s != null ? SystemFunction.ConvertFormatDecimal3(s.H1) : "",
                         sH2 = s != null ? SystemFunction.ConvertFormatDecimal3(s.H2) : "",

                     }).OrderBy(o => o.nOrder).ToList();

        return qData;
    }

    public static List<ClassExecute.TDataOutput> GetDataWasteToPieChart(int nIndID, int nOperaID, int nFacID, string sYear) //เปลี่ยน Unit ที่เป็น KG. เป็น TONNES
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int nEPIFromID = 0;
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == nIndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (qEPIForm != null)
        {
            nEPIFromID = qEPIForm.FormID;
        }

        string sql = @"select tpu.IDIndicator,tpi.ProductID,tpu.UnitID,tun.UnitName,tun.nFactor
from mTProductIndicator tpi 
inner join mTProductIndicatorUnit tpu on tpi.ProductID = tpu.ProductID
inner join mTUnit tun on tun.UnitID = tpu.UnitID
where tun.cDel = 'N' and tun.cActive = 'Y' and tpu.cType = 1 and tun.cManage = 'N' and tpu.IDIndicator = " + CommonFunction.ReplaceInjection(nIndID + "") + @"
order by tpi.ProductID asc, tun.UnitName desc";

        List<ClassExecute.TUnit> lstMSUnit = new List<ClassExecute.TUnit>();
        lstMSUnit = db.Database.SqlQuery<ClassExecute.TUnit>(sql).Select(s => new ClassExecute.TUnit { ProductID = s.ProductID, UnitID = s.UnitID, UnitName = s.UnitName, IDIndicator = s.IDIndicator, nFactor = s.nFactor }).OrderBy(o => o.ProductID).ToList();


        var qData = (from i in db.mTProductIndicator.AsEnumerable().Where(w => w.IDIndicator.Value == nIndID && w.cTotal == "N" && w.nGroupCalc < 3 && (w.sType == "HZD" || w.sType == "NHZD"))
                     from s in db.TWaste_Product.Where(w => w.ProductID == i.ProductID && w.FormID == nEPIFromID).AsEnumerable().DefaultIfEmpty()
                     select new ClassExecute.TDataOutput
                     {
                         IDIndicator = i.IDIndicator ?? 0,
                         OperationtypeID = nOperaID,
                         FacilityID = nFacID,
                         ProductID = i.ProductID,
                         ProductName = i.ProductName,
                         nUnitID = 0,
                         sUnit = s != null ? (s.UnitID != null ? (s.UnitID.Value != 0 ? "Tonnes" : "N/A") : "") : "", //GetUnitName(lstMSUnit, (s != null ? s.UnitID ?? 0 : 0)),
                         nOrder = i.nOrder ?? 0,
                         nGroupCal = i.nGroupCalc ?? 0,
                         sType = i.sType,

                         nTarget = s != null ? SystemFunction.GetDecimalNull(CalculateValueAndUnit(s.Target, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nM1 = s != null ? SystemFunction.GetDecimalNull(CalculateValueAndUnit(s.M1, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nM2 = s != null ? SystemFunction.GetDecimalNull(CalculateValueAndUnit(s.M2, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nM3 = s != null ? SystemFunction.GetDecimalNull(CalculateValueAndUnit(s.M3, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nM4 = s != null ? SystemFunction.GetDecimalNull(CalculateValueAndUnit(s.M4, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nM5 = s != null ? SystemFunction.GetDecimalNull(CalculateValueAndUnit(s.M5, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nM6 = s != null ? SystemFunction.GetDecimalNull(CalculateValueAndUnit(s.M6, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nM7 = s != null ? SystemFunction.GetDecimalNull(CalculateValueAndUnit(s.M7, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nM8 = s != null ? SystemFunction.GetDecimalNull(CalculateValueAndUnit(s.M8, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nM9 = s != null ? SystemFunction.GetDecimalNull(CalculateValueAndUnit(s.M9, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nM10 = s != null ? SystemFunction.GetDecimalNull(CalculateValueAndUnit(s.M10, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nM11 = s != null ? SystemFunction.GetDecimalNull(CalculateValueAndUnit(s.M11, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nM12 = s != null ? SystemFunction.GetDecimalNull(CalculateValueAndUnit(s.M12, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nTotal = s != null ? EPIFunc.SumDataToDecimal(
                         CalculateValueAndUnit(s.M1, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M2, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M3, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M4, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M5, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M6, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M7, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M8, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M9, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M10, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M11, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M12, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : null,
                         nQ1 = s != null ? SystemFunction.GetDecimalNull(s.Q1) : null,
                         nQ2 = s != null ? SystemFunction.GetDecimalNull(s.Q2) : null,
                         nQ3 = s != null ? SystemFunction.GetDecimalNull(s.Q3) : null,
                         nQ4 = s != null ? SystemFunction.GetDecimalNull(s.Q4) : null,
                         nH1 = s != null ? SystemFunction.GetDecimalNull(s.H1) : null,
                         nH2 = s != null ? SystemFunction.GetDecimalNull(s.H2) : null,

                         sTarget = s != null ? SystemFunction.ConvertFormatDecimal3(CalculateValueAndUnit(s.Target, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : "",
                         sM1 = s != null ? SystemFunction.ConvertFormatDecimal3(CalculateValueAndUnit(s.M1, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : "",
                         sM2 = s != null ? SystemFunction.ConvertFormatDecimal3(CalculateValueAndUnit(s.M2, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : "",
                         sM3 = s != null ? SystemFunction.ConvertFormatDecimal3(CalculateValueAndUnit(s.M3, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : "",
                         sM4 = s != null ? SystemFunction.ConvertFormatDecimal3(CalculateValueAndUnit(s.M4, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : "",
                         sM5 = s != null ? SystemFunction.ConvertFormatDecimal3(CalculateValueAndUnit(s.M5, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : "",
                         sM6 = s != null ? SystemFunction.ConvertFormatDecimal3(CalculateValueAndUnit(s.M6, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : "",
                         sM7 = s != null ? SystemFunction.ConvertFormatDecimal3(CalculateValueAndUnit(s.M7, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : "",
                         sM8 = s != null ? SystemFunction.ConvertFormatDecimal3(CalculateValueAndUnit(s.M8, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : "",
                         sM9 = s != null ? SystemFunction.ConvertFormatDecimal3(CalculateValueAndUnit(s.M9, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : "",
                         sM10 = s != null ? SystemFunction.ConvertFormatDecimal3(CalculateValueAndUnit(s.M10, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : "",
                         sM11 = s != null ? SystemFunction.ConvertFormatDecimal3(CalculateValueAndUnit(s.M11, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : "",
                         sM12 = s != null ? SystemFunction.ConvertFormatDecimal3(CalculateValueAndUnit(s.M12, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) : "",
                         sTotal = s != null ? SystemFunction.ConvertFormatDecimal3(
                         EPIFunc.SumDataToDecimal(
                         CalculateValueAndUnit(s.M1, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M2, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M3, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M4, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M5, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M6, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M7, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M8, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M9, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M10, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M11, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit)),
                         CalculateValueAndUnit(s.M12, GetFactorUnit(i.ProductID, (s != null ? s.UnitID ?? 0 : 0), lstMSUnit))) + "") : "",
                         sQ1 = s != null ? SystemFunction.ConvertFormatDecimal3(s.Q1) : "",
                         sQ2 = s != null ? SystemFunction.ConvertFormatDecimal3(s.Q2) : "",
                         sQ3 = s != null ? SystemFunction.ConvertFormatDecimal3(s.Q3) : "",
                         sQ4 = s != null ? SystemFunction.ConvertFormatDecimal3(s.Q4) : "",
                         sH1 = s != null ? SystemFunction.ConvertFormatDecimal3(s.H1) : "",
                         sH2 = s != null ? SystemFunction.ConvertFormatDecimal3(s.H2) : "",
                     }).OrderBy(o => o.nOrder).ToList();

        return qData;
    }

    public static List<ClassExecute.TDataOutput> ConvertDataToPieChart(List<ClassExecute.TDataOutput> lstTData)
    {
        List<ClassExecute.TDataOutput> lstTemp = new List<ClassExecute.TDataOutput>();
        lstTemp = lstTData.Select(s => new ClassExecute.TDataOutput
                     {
                         IDIndicator = s.IDIndicator,
                         OperationtypeID = s.OperationtypeID,
                         FacilityID = s.FacilityID,
                         ProductID = s.ProductID,
                         ProductName = s.ProductName + (!string.IsNullOrEmpty(s.sUnit + "") ? GetUnit(s.sUnit) : ""),
                         nUnitID = 0,
                         sUnit = s.sUnit,
                         nOrder = s.nOrder,
                         nGroupCal = s.nGroupCal,
                         sType = s.sType,

                         nTarget = s.nTarget,
                         nM1 = s.nM1,
                         nM2 = s.nM2,
                         nM3 = s.nM3,
                         nM4 = s.nM4,
                         nM5 = s.nM5,
                         nM6 = s.nM6,
                         nM7 = s.nM7,
                         nM8 = s.nM8,
                         nM9 = s.nM9,
                         nM10 = s.nM10,
                         nM11 = s.nM11,
                         nM12 = s.nM12,
                         nTotal = s.nTotal,
                         nQ1 = s.nQ1,
                         nQ2 = s.nQ2,
                         nQ3 = s.nQ3,
                         nQ4 = s.nQ4,
                         nH1 = s.nH1,
                         nH2 = s.nH2,

                         sTarget = s.sTarget,
                         sM1 = s.sM1,
                         sM2 = s.sM2,
                         sM3 = s.sM3,
                         sM4 = s.sM4,
                         sM5 = s.sM5,
                         sM6 = s.sM6,
                         sM7 = s.sM7,
                         sM8 = s.sM8,
                         sM9 = s.sM9,
                         sM10 = s.sM10,
                         sM11 = s.sM11,
                         sM12 = s.sM12,
                         sTotal = s.sTotal,
                         sQ1 = s.sQ1,
                         sQ2 = s.sQ2,
                         sQ3 = s.sQ3,
                         sQ4 = s.sQ4,
                         sH1 = s.sH1,
                         sH2 = s.sH2
                     }).OrderBy(o => o.nOrder).ToList();

        return lstTemp;

    }

    public static List<ClassExecute.TDataOutput> ConvertDataToPieChartJqplot(List<ClassExecute.TDataOutput> lstTData)
    {
        List<ClassExecute.TDataOutput> lstTemp = new List<ClassExecute.TDataOutput>();
        lstTemp = lstTData.Select(s => new ClassExecute.TDataOutput
        {
            IDIndicator = s.IDIndicator,
            OperationtypeID = s.OperationtypeID,
            FacilityID = s.FacilityID,
            ProductID = s.ProductID,
            ProductName = s.ProductName + (!string.IsNullOrEmpty(s.sUnit + "") ? GetUnit(s.sUnit) : "") + (!string.IsNullOrEmpty(s.sTotal + "") ? " : " + EPIFunc.ConvertToStatistic(s.sTotal, "3") : ""),
            nUnitID = 0,
            sUnit = s.sUnit,
            nOrder = s.nOrder,
            nGroupCal = s.nGroupCal,
            sType = s.sType,

            nTarget = s.nTarget,
            nM1 = s.nM1,
            nM2 = s.nM2,
            nM3 = s.nM3,
            nM4 = s.nM4,
            nM5 = s.nM5,
            nM6 = s.nM6,
            nM7 = s.nM7,
            nM8 = s.nM8,
            nM9 = s.nM9,
            nM10 = s.nM10,
            nM11 = s.nM11,
            nM12 = s.nM12,
            nTotal = s.nTotal,
            nQ1 = s.nQ1,
            nQ2 = s.nQ2,
            nQ3 = s.nQ3,
            nQ4 = s.nQ4,
            nH1 = s.nH1,
            nH2 = s.nH2,

            sTarget = s.sTarget,
            sM1 = s.sM1,
            sM2 = s.sM2,
            sM3 = s.sM3,
            sM4 = s.sM4,
            sM5 = s.sM5,
            sM6 = s.sM6,
            sM7 = s.sM7,
            sM8 = s.sM8,
            sM9 = s.sM9,
            sM10 = s.sM10,
            sM11 = s.sM11,
            sM12 = s.sM12,
            sTotal = s.sTotal,
            sQ1 = s.sQ1,
            sQ2 = s.sQ2,
            sQ3 = s.sQ3,
            sQ4 = s.sQ4,
            sH1 = s.sH1,
            sH2 = s.sH2
        }).OrderBy(o => o.nOrder).ToList();

        return lstTemp;

    }

    public static string GetUnitName(List<ClassExecute.TUnit> lstUnitTemp, int nUnitID)
    {
        string sRetrun = "";
        var query = lstUnitTemp.Where(w => w.UnitID == nUnitID).FirstOrDefault();
        sRetrun = query != null ? query.UnitName : "";
        return sRetrun;
    }

    private static string GetUnit(string strUnit)
    {
        return "(" + ReplaceHtmlUnit(strUnit) + ")";
    }

    public static decimal? GetFactorUnit(int nProductID, int nUnitID, List<ClassExecute.TUnit> lstUnit)
    {
        var query = lstUnit.Where(w => w.ProductID == nProductID && w.UnitID == nUnitID).FirstOrDefault();
        if (query != null)
        {
            return query.nFactor;
        }
        else
        {
            return null;
        }
    }

    public static string ReplaceHtmlUnit(string str)
    {
        string[] _blacklist = new string[] { "<sup>", "</sup>", "<sub>", "</sub>", "<p>", "</p>" };
        string strRep = str;
        if (strRep == null || strRep.Trim().Equals(String.Empty))
            return strRep;
        foreach (string _blk in _blacklist) { strRep = strRep.Replace(_blk, ""); }

        return strRep;
    }

    public static int GetQuater()
    {
        int nQ = 0;
        int nMonth = DateTime.Now.Month;
        if (nMonth <= 3)
        {
            nQ = 1;
        }
        else if (nMonth <= 6)
        {
            nQ = 2;
        }
        else if (nMonth <= 9)
        {
            nQ = 3;
        }
        else if (nMonth <= 12)
        {
            nQ = 4;
        }

        return nQ;
    }

    public static List<ClassExecute.TDataOutput> ConvertRecalDataOutput(List<ClassExecute.TDataOutput> lstDataRecal, List<ClassExecute.TDataOutput> lstOldData)
    {
        List<ClassExecute.TDataOutput> lstReturn = new List<ClassExecute.TDataOutput>();
        lstReturn = (from lr in lstDataRecal
                     from lo in lstOldData.Where(w => w.ProductID == lr.ProductID)
                     select new ClassExecute.TDataOutput
                     {
                         IDIndicator = lo.IDIndicator,
                         OperationtypeID = lo.OperationtypeID,
                         FacilityID = lo.FacilityID,
                         ProductID = lo.ProductID,
                         ProductName = lo.ProductName,
                         nUnitID = lo.nUnitID,
                         sUnit = lo.sUnit,
                         nOrder = lo.nOrder,

                         nTarget = lr.nTarget,
                         nM1 = lr.nM1,
                         nM2 = lr.nM2,
                         nM3 = lr.nM3,
                         nM4 = lr.nM4,
                         nM5 = lr.nM5,
                         nM6 = lr.nM6,
                         nM7 = lr.nM7,
                         nM8 = lr.nM8,
                         nM9 = lr.nM9,
                         nM10 = lr.nM10,
                         nM11 = lr.nM11,
                         nM12 = lr.nM12,
                         nTotal = lr.nTotal,
                         nQ1 = lr.nQ1,
                         nQ2 = lr.nQ2,
                         nQ3 = lr.nQ3,
                         nQ4 = lr.nQ4,
                         nH1 = lr.nH1,
                         nH2 = lr.nH2,

                         sTarget = SystemFunction.ConvertFormatDecimal3(lr.nTarget + ""),
                         sM1 = SystemFunction.ConvertFormatDecimal3(lr.nM1 + ""),
                         sM2 = SystemFunction.ConvertFormatDecimal3(lr.nM2 + ""),
                         sM3 = SystemFunction.ConvertFormatDecimal3(lr.nM3 + ""),
                         sM4 = SystemFunction.ConvertFormatDecimal3(lr.nM4 + ""),
                         sM5 = SystemFunction.ConvertFormatDecimal3(lr.nM5 + ""),
                         sM6 = SystemFunction.ConvertFormatDecimal3(lr.nM6 + ""),
                         sM7 = SystemFunction.ConvertFormatDecimal3(lr.nM7 + ""),
                         sM8 = SystemFunction.ConvertFormatDecimal3(lr.nM8 + ""),
                         sM9 = SystemFunction.ConvertFormatDecimal3(lr.nM9 + ""),
                         sM10 = SystemFunction.ConvertFormatDecimal3(lr.nM10 + ""),
                         sM11 = SystemFunction.ConvertFormatDecimal3(lr.nM11 + ""),
                         sM12 = SystemFunction.ConvertFormatDecimal3(lr.nM12 + ""),
                         sTotal = SystemFunction.ConvertFormatDecimal3(lr.nTotal + ""),
                         sQ1 = SystemFunction.ConvertFormatDecimal3(lr.nQ1 + ""),
                         sQ2 = SystemFunction.ConvertFormatDecimal3(lr.nQ2 + ""),
                         sQ3 = SystemFunction.ConvertFormatDecimal3(lr.nQ3 + ""),
                         sQ4 = SystemFunction.ConvertFormatDecimal3(lr.nQ4 + ""),
                         sH1 = SystemFunction.ConvertFormatDecimal3(lr.nH1 + ""),
                         sH2 = SystemFunction.ConvertFormatDecimal3(lr.nH2 + ""),

                         TooptipID = lo.TooptipID,
                         TooltipName = lo.TooltipName

                     }).OrderBy(o => o.nOrder).ToList();
        return lstReturn;
    }

    private static string GetTarget_TIntensity_Output(int nFormID, int nProductID, string sOldVal)
    {
        string sReturn = "";
        string sql = @"select * from TProductOutput_Intensity where FormID = " + nFormID + " and ProductID = " + nProductID;
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(strConn, sql);
        if (dt.Rows.Count > 0)
        {
            sReturn = dt.Rows[0]["target"] + "";
        }
        else
        {
            sReturn = sOldVal;
        }

        return sReturn;
    }

    public static string CalculateValueAndUnit(string sVal, decimal? nfactor)
    {
        string sReturn = "";

        decimal? nVal = null;
        if (!string.IsNullOrEmpty(sVal))
        {
            nVal = SystemFunction.GetDecimalNull(sVal) * nfactor;
            sReturn = nVal + "";
        }

        return sReturn;
    }

    public static string GetFacilityName(string sFacID)
    {
        string sName = "";
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int nFacID = SystemFunction.ParseInt(sFacID);
        var query = db.mTFacility.Where(w => w.ID == nFacID).FirstOrDefault();
        if (query != null)
        {
            sName = query.Name;
        }

        return sName;
    }

    public static string GetGroupIndName(string sGroupID)
    {
        string sName = "";
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int nGroupID = SystemFunction.ParseInt(sGroupID);
        var query = db.mTIndicator.Where(w => w.ID == nGroupID).FirstOrDefault();
        if (query != null)
        {
            sName = query.Indicator;
        }

        return sName;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="IndicatorID"></param>
    /// <param name="nType">1 = Input, 2 = Output</param>
    /// <returns></returns>
    public static List<ClassExecute.TData_Tooltip> GetIndicatorTooltip(int IndicatorID, int nType)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        string sql = @"select a.ID 'nTootipID',a.Name 'sTooltipName',b.ProductID
from TTooltip_Product a
inner join TProduct_Tooltip b on a.ID = b.TooltipID
where b.IDIndicator = '{0}'";
        var query = db.Database.SqlQuery<ClassExecute.TData_Tooltip>(string.Format(sql, IndicatorID)).Select(s => new ClassExecute.TData_Tooltip
            {
                nTootipID = s.nTootipID,
                sTooltipName = s.sTooltipName,
                ProductID = s.ProductID
            }).ToList();

        return query;

    }

    /// <summary>
    /// Tooltip
    /// </summary>
    /// <param name="IndicatorID"></param>
    /// <param name="sProductID"></param>
    /// <param name="nType">1 = Input, 2 = Output</param>
    /// <returns></returns>
    public static string GetIndicatorTooltip(int IndicatorID, int ProductID, int nType)
    {
        string sql = @"select a.Name
from TTooltip_Product a
inner join TProduct_Tooltip b on a.ID = b.TooltipID
where b.IDIndicator = '{0}' and b.ProductID = '{1}' and b.cType = '{2}'";
        string sResult = "";
        sResult = CommonFunction.Get_Value(SystemFunction.strConnect, string.Format(sql, IndicatorID, ProductID, nType));
        return sResult;
    }

    /// <summary>
    /// Get Data From TTooltip_Product Only
    /// </summary>
    /// <param name="nTooltipID"></param>
    /// <returns></returns>
    public static string GetIndicatorTooltip(int nTooltipID)
    {
        string sql = @"select a.Name
from TTooltip_Product a
where a.ID = '{0}'";
        string sResult = "";
        sResult = CommonFunction.Get_Value(SystemFunction.strConnect, string.Format(sql, nTooltipID));
        return sResult;
    }

    public static string GetIndicatorTooltip(List<ClassExecute.TData_Tooltip> lstDataTooltip, int ProductID)
    {
        string sResult = "";
        var query = lstDataTooltip.Where(w => w.ProductID == ProductID).FirstOrDefault();
        if (query != null)
        {
            sResult = query.sTooltipName;
        }
        return sResult;
    }

    /// <summary>
    /// ข้อมูล Product Output ทั้งหมด
    /// รวมถึง Indicator Waste ที่ sType = 'R' ด้วย
    /// ไม่ใช้ nOperaID เนื่องจาก Report ที่ไม่ได้เลือก Operation Type จะมี Product แสดงไม่ครบ แต่จะตัด Product จาก Sub Product แทนใน sysEPIObjReport().GetProductPlotGraph
    /// </summary>
    /// <param name="nIndID"></param>
    /// <param name="nOperaID"></param>
    /// <returns></returns>
    public List<ClassExecute.TData_ProductCalculate> GetProductOutputToReport(int nIndID, int nOperaID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();

        string sqlProductOutput = "";
        if (nIndID == 9 || nIndID == 1 || nIndID == 2)//Spill,Complaint,Compliance
        {
            sqlProductOutput = @"select a.IDIndicator,a.ProductID,a.ProductName,a.nOrder,a.sUnit,a.sType,
                                    ISNULL(b.UnitID,0) 'UnitID',ISNULL(b.UnitName,a.sUnit) 'UnitName'
                                    from mTProductIndicatorOutput a
                                    left join mTUnit b on a.sUnit = b.UnitName
                                    where a.IDIndicator = " + nIndID + " order by a.nOrder ";
        }
        else
        {
            sqlProductOutput = @"select a.IDIndicator,a.ProductID,a.ProductName,a.nOrder,a.sUnit,a.sType,
                                    ISNULL(b.UnitID,0) 'UnitID',ISNULL(b.UnitName,a.sUnit) 'UnitName'
                                    from mTProductIndicatorOutput a
                                    inner join TUseProductOutput d on a.ProductID = d.ProductID
                                    left join mTUnit b on a.sUnit = b.UnitName
                                    where a.IDIndicator = " + nIndID + " --and d.OperationtypeID = " + nOperaID + @"
                                    group by a.IDIndicator,a.ProductID,a.ProductName,a.nOrder,a.sUnit,a.sType,ISNULL(b.UnitID,0),ISNULL(b.UnitName,a.sUnit)
                                    order by a.nOrder ";
        }

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(sqlProductOutput).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();
        return qProductOutput;
    }

    /// <summary>
    /// ดึงข้อมูล Product Input จาก List Sub Indicator
    /// </summary>
    /// <param name="nIndID"></param>
    /// <param name="nOperaID"></param>
    /// <param name="lstSubIndicator"></param>
    /// <returns></returns>
    public List<ClassExecute.TData_ProductCalculate> GetProductInputToReport(int nIndID, int nOperaID, List<ClassExecute.TData_TargetIndicator> lstSubIndicator)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TData_ProductCalculate> lstdata = new List<ClassExecute.TData_ProductCalculate>();
        if (nIndID == EPIFunc.nIndWasteID)
        {
            #region Waste
            lstdata = (from p in db.mTProductIndicator.AsEnumerable().Where(w => w.IDIndicator == nIndID && w.cDel == "N")
                       from l in lstSubIndicator.Where(w => w.nProductID == p.ProductID)
                       select new ClassExecute.TData_ProductCalculate
                         {
                             IDIndicator = nIndID,
                             ProductID = p.ProductID,
                             ProductName = p.ProductName,
                             nOrder = p.nOrder ?? 0,
                             sUnit = "Tonnes",
                             UnitID = 0,
                             UnitName = "Tonnes"
                         }).OrderBy(o => o.nOrder).ToList();
            #endregion
        }
        else if (nIndID == EPIFunc.nIndWaterID)
        {
            #region Water
            lstdata = (from p in db.mTProductIndicator.AsEnumerable().Where(w => w.IDIndicator == nIndID && w.cDel == "N" && w.cTotal == "N" && w.sType == "2")
                       from l in lstSubIndicator.Where(w => w.nProductID == p.ProductID)
                       select new ClassExecute.TData_ProductCalculate
                       {
                           IDIndicator = nIndID,
                           ProductID = p.ProductID,
                           ProductName = p.ProductName,
                           nOrder = p.nOrder ?? 0,
                           sUnit = "m<sup>3</sup>",
                           UnitID = 0,
                           UnitName = "m<sup>3</sup>"
                       }).OrderBy(o => o.nOrder).ToList();
            #endregion
        }
        else if (nIndID == EPIFunc.nIndMaterialID)
        {
            #region Material
            lstdata = (from p in db.mTProductIndicator.AsEnumerable().Where(w => w.IDIndicator == nIndID && w.cDel == "N" && w.cTotal == "Y" && (w.ProductID == 34 || w.ProductID == 37))
                       from l in lstSubIndicator.Where(w => w.nProductID == p.ProductID)
                       select new ClassExecute.TData_ProductCalculate
                       {
                           IDIndicator = nIndID,
                           ProductID = p.ProductID,
                           ProductName = p.ProductName,
                           nOrder = p.nOrder ?? 0,
                           sUnit = "Tonnes",
                           UnitID = 0,
                           UnitName = "Tonnes"
                       }).OrderBy(o => o.nOrder).ToList();
            #endregion
        }
        return lstdata;
    }

    public int GetFormID(int nIndID, string sYear, int nOperaID, int nFacID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        var qEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == nIndID && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        return qEPIForm != null ? qEPIForm.FormID : 0;
    }

    public List<ClassExecute.TData_Intensity> GetDataIntensityToCalculateOutput(List<int> lstEPIFormID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 6;
        string strEPIFromID = lstEPIFormID.SplitToInSQL();

        string sql = @"select isnull(tepi.FacilityID,0) 'FacilityID', isnull(tepi.OperationTypeID,0) 'OperationTypeID',tepi.sYear
,tpi.ProductID,tpi.ProductName,tpi.cTotal,tpi.cTotalAll,isnull(tpi.nGroupCalc,0) 'nGroupCalc',tpi.sUnit,tpi.sType,isnull(tiup.nOrder,0) 'nOrder'
,isnull(tid.FormID,0) 'FormID',isnull(tid.UnitID,0) 'UnitID'
,tid.M1,tid.M2,tid.M3,tid.M4,tid.M5,tid.M6,tid.M7,tid.M8,tid.M9,tid.M10,tid.M11,tid.M12,tid.nTotal,tid.Target
from TEPI_Forms tepi
inner join TIntensityDominator tid on tepi.FormID=tid.FormID
inner join TIntensityUseProduct tiup on tiup.OperationTypeID=tepi.OperationTypeID and tid.ProductID=tiup.ProductID
inner join mTProductIndicator tpi on tiup.ProductID=tpi.ProductID and tpi.IDIndicator=6
where tepi.IDIndicator=" + IndID + " and tepi.FormID in (" + strEPIFromID + @")
order by tiup.nOrder";

        List<ClassExecute.TData_Intensity> lstdata = new List<ClassExecute.TData_Intensity>();
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, sql);
        if (dt.Rows.Count > 0)
        {
            lstdata = dt.AsEnumerable().Select(s => new ClassExecute.TData_Intensity
            {
                FacilityID = s.Field<int>("FacilityID"),
                OperationTypeID = s.Field<int>("OperationTypeID"),
                sYear = s.Field<string>("sYear"),
                ProductID = s.Field<int>("ProductID"),
                ProductName = s.Field<string>("ProductName"),
                cTotal = s.Field<string>("cTotal"),
                cTotalAll = s.Field<string>("cTotalAll"),
                nGroupCalc = s.Field<int>("nGroupCalc"),
                nOrder = s.Field<decimal>("nOrder"),
                sUnit = s.Field<string>("sUnit"),
                sType = s.Field<string>("sType"),
                FormID = s.Field<int>("FormID"),
                UnitID = s.Field<int>("UnitID"),
                M1 = s.Field<string>("M1"),
                M2 = s.Field<string>("M2"),
                M3 = s.Field<string>("M3"),
                M4 = s.Field<string>("M4"),
                M5 = s.Field<string>("M5"),
                M6 = s.Field<string>("M6"),
                M7 = s.Field<string>("M7"),
                M8 = s.Field<string>("M8"),
                M9 = s.Field<string>("M9"),
                M10 = s.Field<string>("M10"),
                M11 = s.Field<string>("M11"),
                M12 = s.Field<string>("M12"),
                nTotal = s.Field<string>("nTotal"),
                Target = s.Field<string>("Target")
            }).ToList();
        }
        return lstdata;
    }

    public List<ClassExecute.TData_Water> GetDataWaterToCalculateOutput(List<int> lstEPIFormID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 11;
        string strFromID = lstEPIFormID.SplitToInSQL();

        string sql = @"select tpi.ProductID,tpi.ProductName,tpi.cTotal,tpi.cTotalAll,isnull(tpi.nGroupCalc,0) 'nGroupCalc',isnull(tpi.nOrder,0) 'nOrder',tpi.sUnit,tpi.sType
,isnull(twp.FormID,0) 'FormID',isnull(twp.UnitID,0) 'UnitID'
,twp.M1,twp.M2,twp.M3,twp.M4,twp.M5,twp.M6,twp.M7,twp.M8,twp.M9,twp.M10,twp.M11,twp.M12,twp.nTotal,twp.Target
,isnull(tepi.FacilityID,0) 'FacilityID', isnull(tepi.OperationTypeID,0) 'OperationTypeID',tepi.sYear
from mTProductIndicator tpi
inner join TWater_Product twp on tpi.ProductID = twp.ProductID and twp.FormID in (" + strFromID + @")
inner join TEPI_Forms tepi on twp.FormID=tepi.FormID
where tpi.IDIndicator = '" + IndID + "'";

        List<ClassExecute.TData_Water> lstdata = new List<ClassExecute.TData_Water>();
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, sql);
        if (dt.Rows.Count > 0)
        {
            lstdata = dt.AsEnumerable().Select(s => new ClassExecute.TData_Water
            {
                FacilityID = s.Field<int>("FacilityID"),
                OperationTypeID = s.Field<int>("OperationTypeID"),
                sYear = s.Field<string>("sYear"),
                ProductID = s.Field<int>("ProductID"),
                ProductName = s.Field<string>("ProductName"),
                cTotal = s.Field<string>("cTotal"),
                cTotalAll = s.Field<string>("cTotalAll"),
                nGroupCalc = s.Field<int>("nGroupCalc"),
                nOrder = s.Field<int>("nOrder"),
                sUnit = s.Field<string>("sUnit"),
                sType = s.Field<string>("sType"),
                FormID = s.Field<int>("FormID"),
                UnitID = s.Field<int>("UnitID"),
                M1 = s.Field<string>("M1"),
                M2 = s.Field<string>("M2"),
                M3 = s.Field<string>("M3"),
                M4 = s.Field<string>("M4"),
                M5 = s.Field<string>("M5"),
                M6 = s.Field<string>("M6"),
                M7 = s.Field<string>("M7"),
                M8 = s.Field<string>("M8"),
                M9 = s.Field<string>("M9"),
                M10 = s.Field<string>("M10"),
                M11 = s.Field<string>("M11"),
                M12 = s.Field<string>("M12"),
                nTotal = s.Field<string>("nTotal"),
                Target = s.Field<string>("Target")
            }).OrderBy(o => o.nOrder).ToList();
        }
        return lstdata;
    }

    public List<ClassExecute.TDataWaste> GetDataWasteToCalculateOutput(List<int> lstEPIFormID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 10;
        string strFormID = lstEPIFormID.SplitToInSQL();

        string sql = @"select tpi.ProductID,tpi.ProductName,tpi.cTotal,tpi.cTotalAll,isnull(tpi.nGroupCalc,0) 'nGroupCalc',isnull(tpi.nOrder,0) 'nOrder',tpi.sUnit,tpi.sType
,isnull(twp.FormID,0) 'FormID',isnull(twp.UnitID,0) 'UnitID'
,twp.M1,twp.M2,twp.M3,twp.M4,twp.M5,twp.M6,twp.M7,twp.M8,twp.M9,twp.M10,twp.M11,twp.M12,twp.nTotal,twp.Target,twp.PreviousYear,twp.ReportingYear
,isnull(tepi.FacilityID,0) 'FacilityID', isnull(tepi.OperationTypeID,0) 'OperationTypeID',tepi.sYear
from mTProductIndicator tpi
inner join TWaste_Product twp on tpi.ProductID = twp.ProductID and twp.FormID in (" + strFormID + @")
inner join TEPI_Forms tepi on twp.FormID=tepi.FormID
where tpi.IDIndicator = '" + IndID + "'";

        List<ClassExecute.TDataWaste> lstdata = new List<ClassExecute.TDataWaste>();
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, sql);
        if (dt.Rows.Count > 0)
        {
            lstdata = dt.AsEnumerable().Select(s => new ClassExecute.TDataWaste
            {
                FacilityID = s.Field<int>("FacilityID"),
                OperationTypeID = s.Field<int>("OperationTypeID"),
                sYear = s.Field<string>("sYear"),
                ProductID = s.Field<int>("ProductID"),
                ProductName = s.Field<string>("ProductName"),
                cTotal = s.Field<string>("cTotal"),
                cTotalAll = s.Field<string>("cTotalAll"),
                nGroupCalc = s.Field<int>("nGroupCalc"),
                nOrder = s.Field<int>("nOrder"),
                sUnit = s.Field<string>("sUnit"),
                sType = s.Field<string>("sType"),
                FormID = s.Field<int>("FormID"),
                UnitID = s.Field<int>("UnitID"),
                M1 = s.Field<string>("M1"),
                M2 = s.Field<string>("M2"),
                M3 = s.Field<string>("M3"),
                M4 = s.Field<string>("M4"),
                M5 = s.Field<string>("M5"),
                M6 = s.Field<string>("M6"),
                M7 = s.Field<string>("M7"),
                M8 = s.Field<string>("M8"),
                M9 = s.Field<string>("M9"),
                M10 = s.Field<string>("M10"),
                M11 = s.Field<string>("M11"),
                M12 = s.Field<string>("M12"),
                nTotal = s.Field<string>("nTotal"),
                Target = s.Field<string>("Target"),
                PreviousYear = s.Field<string>("PreviousYear"),
                ReportingYear = s.Field<string>("ReportingYear"),

            }).OrderBy(o => o.nOrder).ToList();
        }
        return lstdata;
    }

    public List<ClassExecute.TDataMaterial> GetDataMaterialToCalculateOutput(List<int> lstEPIFormID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 8;
        string strFormID = lstEPIFormID.SplitToInSQL();

        string sql = @"select tpi.ProductID,tpi.ProductName,tpi.cTotal,tpi.cTotalAll,isnull(tpi.nGroupCalc,0) 'nGroupCalc',isnull(tpi.nOrder,0) 'nOrder',tpi.sUnit,tpi.sType
,isnull(twp.FormID,0) 'FormID',isnull(twp.UnitID,0) 'UnitID'
,twp.M1,twp.M2,twp.M3,twp.M4,twp.M5,twp.M6,twp.M7,twp.M8,twp.M9,twp.M10,twp.M11,twp.M12,twp.nTotal,twp.Target
,isnull(tepi.FacilityID,0) 'FacilityID', isnull(tepi.OperationTypeID,0) 'OperationTypeID',tepi.sYear
from mTProductIndicator tpi
inner join TMaterial_Product twp on tpi.ProductID = twp.ProductID and twp.FormID in (" + strFormID + @")
inner join TEPI_Forms tepi on twp.FormID=tepi.FormID
where tpi.IDIndicator = '" + IndID + "'";

        List<ClassExecute.TDataMaterial> lstdata = new List<ClassExecute.TDataMaterial>();
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, sql);
        if (dt.Rows.Count > 0)
        {
            lstdata = dt.AsEnumerable().Select(s => new ClassExecute.TDataMaterial
            {
                FacilityID = s.Field<int>("FacilityID"),
                OperationTypeID = s.Field<int>("OperationTypeID"),
                sYear = s.Field<string>("sYear"),
                ProductID = s.Field<int>("ProductID"),
                ProductName = s.Field<string>("ProductName"),
                cTotal = s.Field<string>("cTotal"),
                cTotalAll = s.Field<string>("cTotalAll"),
                nGroupCalc = s.Field<int>("nGroupCalc"),
                nOrder = s.Field<int>("nOrder"),
                sUnit = s.Field<string>("sUnit"),
                sType = s.Field<string>("sType"),
                FormID = s.Field<int>("FormID"),
                UnitID = s.Field<int>("UnitID"),
                M1 = s.Field<string>("M1"),
                M2 = s.Field<string>("M2"),
                M3 = s.Field<string>("M3"),
                M4 = s.Field<string>("M4"),
                M5 = s.Field<string>("M5"),
                M6 = s.Field<string>("M6"),
                M7 = s.Field<string>("M7"),
                M8 = s.Field<string>("M8"),
                M9 = s.Field<string>("M9"),
                M10 = s.Field<string>("M10"),
                M11 = s.Field<string>("M11"),
                M12 = s.Field<string>("M12"),
                nTotal = s.Field<string>("nTotal"),
                Target = s.Field<string>("Target")
            }).OrderBy(o => o.nOrder).ToList();
        }
        return lstdata;
    }

    public List<ClassExecute.TData_Emission> GetDataEmissionToCalculateOutput(List<int> lstEPIFormID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 4;
        string strFormID = lstEPIFormID.SplitToInSQL();

        DataTable dt = new DataTable();

        #region Data Summary Stack
        List<ClassExecute.TData_Emission> qDataSumStack = new List<ClassExecute.TData_Emission>();
        string sqlStack = @"select tpi.ProductID,tpi.ProductName,tpi.cTotal,tpi.cTotalAll,isnull(tpi.nGroupCalc,0) 'nGroupCalc',isnull(tpi.nOrder,0) 'nOrder',tpi.sUnit,tpi.sType
,isnull(twp.FormID,0) 'FormID',isnull(twp.UnitID,0) 'UnitID'
,twp.M1,twp.M2,twp.M3,twp.M4,twp.M5,twp.M6,twp.M7,twp.M8,twp.M9,twp.M10,twp.M11,twp.M12,twp.nTotal,twp.Target
,isnull(tepi.FacilityID,0) 'FacilityID', isnull(tepi.OperationTypeID,0) 'OperationTypeID',tepi.sYear
from mTProductIndicator tpi
inner join TEmission_Product twp on tpi.ProductID = twp.ProductID and twp.FormID in (" + strFormID + @")
inner join TEPI_Forms tepi on twp.FormID=tepi.FormID
where tpi.IDIndicator = '" + IndID + "' and tpi.sType='SUM'";

        dt = CommonFunction.Get_Data(SystemFunction.strConnect, sqlStack);
        if (dt.Rows.Count > 0)
        {
            qDataSumStack = dt.AsEnumerable().Select(s => new ClassExecute.TData_Emission
            {
                FacilityID = s.Field<int>("FacilityID"),
                OperationTypeID = s.Field<int>("OperationTypeID"),
                sYear = s.Field<string>("sYear"),
                ProductID = s.Field<int>("ProductID"),
                ProductName = s.Field<string>("ProductName"),
                cTotal = s.Field<string>("cTotal"),
                cTotalAll = s.Field<string>("cTotalAll"),
                nGroupCalc = s.Field<int>("nGroupCalc"),
                nOrder = s.Field<int>("nOrder"),
                sUnit = s.Field<string>("sUnit"),
                sType = s.Field<string>("sType"),
                FormID = s.Field<int>("FormID"),
                UnitID = s.Field<int>("UnitID"),
                M1 = s.Field<string>("M1"),
                M2 = s.Field<string>("M2"),
                M3 = s.Field<string>("M3"),
                M4 = s.Field<string>("M4"),
                M5 = s.Field<string>("M5"),
                M6 = s.Field<string>("M6"),
                M7 = s.Field<string>("M7"),
                M8 = s.Field<string>("M8"),
                M9 = s.Field<string>("M9"),
                M10 = s.Field<string>("M10"),
                M11 = s.Field<string>("M11"),
                M12 = s.Field<string>("M12"),
                nTotal = s.Field<string>("nTotal"),
                Target = s.Field<string>("Target")
            }).OrderBy(o => o.nOrder).ToList();
        }
        #endregion

        #region Data VOC >>/// เนื่องจากมีการเก็บค่าเป็น YTD ลงมาด้วย ซึ่งจะทำให้ค่า 12 เดือนเป็น NULL ดังนั้นอิงตามระบบเดิมนำค่า Total มาหาร 12 เพื่อแยกให้แต่ละเดือน
        List<ClassExecute.TData_Emission> qDataVOC = new List<ClassExecute.TData_Emission>();
        dt = new DataTable();
        string sqlVOC = @"SELECT TPI.ProductID,TPI.ProductName,TPI.cTotal,TPI.cTotalAll,isnull(TPI.nGroupCalc,0) 'nGroupCalc',isnull(TPI.nOrder,0) 'nOrder',TPI.sUnit,TPI.sType
,isnull(TEV.FormID,0) 'FormID',isnull(TEV.UnitID,0) 'UnitID'
,TEV.M1,TEV.M2,TEV.M3,TEV.M4,TEV.M5,TEV.M6,TEV.M7,TEV.M8,TEV.M9,TEV.M10,TEV.M11,TEV.M12,TEV.nTotal,TEV.Target
,TEV.sOption
,isnull(TEPI.FacilityID,0) 'FacilityID', isnull(TEPI.OperationTypeID,0) 'OperationTypeID',TEPI.sYear
FROM mTProductIndicator TPI
INNER JOIN TEmission_VOC TEV ON TPI.ProductID=TEV.ProductID AND TEV.FormID in (" + strFormID + @")
INNER join TEPI_Forms TEPI on TEV.FormID=TEPI.FormID
WHERE TPI.IDIndicator='" + IndID + "' AND TPI.sType='VOC'";
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, sqlVOC);
        if (dt.Rows.Count > 0)
        {
            qDataVOC = dt.AsEnumerable().Select(s => new ClassExecute.TData_Emission
            {
                FacilityID = s.Field<int>("FacilityID"),
                OperationTypeID = s.Field<int>("OperationTypeID"),
                sYear = s.Field<string>("sYear"),
                ProductID = s.Field<int>("ProductID"),
                ProductName = s.Field<string>("ProductName"),
                cTotal = s.Field<string>("cTotal"),
                cTotalAll = s.Field<string>("cTotalAll"),
                nGroupCalc = s.Field<int>("nGroupCalc"),
                nOrder = s.Field<int>("nOrder"),
                sUnit = s.Field<string>("sUnit"),
                sType = s.Field<string>("sType"),
                FormID = s.Field<int>("FormID"),
                UnitID = s.Field<int>("UnitID"),
                M1 = s.Field<string>("sOption") == "M" ? s.Field<string>("M1") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M2 = s.Field<string>("sOption") == "M" ? s.Field<string>("M2") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M3 = s.Field<string>("sOption") == "M" ? s.Field<string>("M3") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M4 = s.Field<string>("sOption") == "M" ? s.Field<string>("M4") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M5 = s.Field<string>("sOption") == "M" ? s.Field<string>("M5") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M6 = s.Field<string>("sOption") == "M" ? s.Field<string>("M6") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M7 = s.Field<string>("sOption") == "M" ? s.Field<string>("M7") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M8 = s.Field<string>("sOption") == "M" ? s.Field<string>("M8") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M9 = s.Field<string>("sOption") == "M" ? s.Field<string>("M9") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M10 = s.Field<string>("sOption") == "M" ? s.Field<string>("M10") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M11 = s.Field<string>("sOption") == "M" ? s.Field<string>("M11") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                M12 = s.Field<string>("sOption") == "M" ? s.Field<string>("M12") : EPIFunc.sysDivideData(s.Field<string>("nTotal"), "12") + "",
                nTotal = s.Field<string>("nTotal"),
                Target = s.Field<string>("Target")
            }).OrderBy(o => o.nOrder).ToList();
        }
        #endregion

        var qData = qDataSumStack.Concat(qDataVOC).ToList();
        return qData;
    }

    public List<ClassExecute.TData_Effluent> GetDataEffluentToCalculateOutput(List<int> lstEPIFormID)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        int IndID = 3;
        string strEPIFormID = lstEPIFormID.SplitToInSQL();

        string sql = @"select tpi.ProductID,tpi.ProductName,tpi.cTotal,tpi.cTotalAll,isnull(tpi.nGroupCalc,0) 'nGroupCalc',isnull(tpi.nOrder,0) 'nOrder',tpi.sUnit,tpi.sType
,isnull(twp.FormID,0) 'FormID',isnull(twp.UnitID,0) 'UnitID'
,twp.M1,twp.M2,twp.M3,twp.M4,twp.M5,twp.M6,twp.M7,twp.M8,twp.M9,twp.M10,twp.M11,twp.M12,twp.nTotal,twp.Target
,isnull(tepi.FacilityID,0) 'FacilityID', isnull(tepi.OperationTypeID,0) 'OperationTypeID',tepi.sYear
from mTProductIndicator tpi
inner join TEffluent_Product twp on tpi.ProductID = twp.ProductID and twp.FormID in (" + strEPIFormID + @")
inner join TEPI_Forms tepi on twp.FormID=tepi.FormID
where tpi.IDIndicator = '" + IndID + "'";

        List<ClassExecute.TData_Effluent> lstdata = new List<ClassExecute.TData_Effluent>();
        DataTable dt = new DataTable();
        dt = CommonFunction.Get_Data(SystemFunction.strConnect, sql);
        if (dt.Rows.Count > 0)
        {
            lstdata = dt.AsEnumerable().Select(s => new ClassExecute.TData_Effluent
            {
                FacilityID = s.Field<int>("FacilityID"),
                OperationTypeID = s.Field<int>("OperationTypeID"),
                sYear = s.Field<string>("sYear"),
                ProductID = s.Field<int>("ProductID"),
                ProductName = s.Field<string>("ProductName"),
                cTotal = s.Field<string>("cTotal"),
                cTotalAll = s.Field<string>("cTotalAll"),
                nGroupCalc = s.Field<int>("nGroupCalc"),
                nOrder = s.Field<int>("nOrder"),
                sUnit = s.Field<string>("sUnit"),
                sType = s.Field<string>("sType"),
                FormID = s.Field<int>("FormID"),
                UnitID = s.Field<int>("UnitID"),
                M1 = s.Field<string>("M1"),
                M2 = s.Field<string>("M2"),
                M3 = s.Field<string>("M3"),
                M4 = s.Field<string>("M4"),
                M5 = s.Field<string>("M5"),
                M6 = s.Field<string>("M6"),
                M7 = s.Field<string>("M7"),
                M8 = s.Field<string>("M8"),
                M9 = s.Field<string>("M9"),
                M10 = s.Field<string>("M10"),
                M11 = s.Field<string>("M11"),
                M12 = s.Field<string>("M12"),
                nTotal = s.Field<string>("nTotal"),
                Target = s.Field<string>("Target")
            }).OrderBy(o => o.nOrder).ToList();
        }
        return lstdata;
    }

    /// <summary>
    /// ดึงข้อมูล Product Output and SUB
    /// </summary>   
    public static List<ClassExecute.TDataOutput> GetWasteDataOutput(int nFormID, int nIDIndicator, int OperationType, int nFacility, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataOutput> lstOut = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstData = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstHead = new List<ClassExecute.TDataOutput>();

        lstOut = FunctionGetData.GetDataOutput(nIDIndicator, OperationType, nFacility, sYear);
        lstData = lstOut.ToList();
        var lstMark = db.TWaste_Remark.Where(w => w.FormID == nFormID).OrderByDescending(o => o.nVersion).ToList();
        Func<int, string> GetRemarkInput = (productid) =>
        {
            string sremark = "";
            var q = lstMark.FirstOrDefault(w => w.ProductID == productid);
            sremark = q != null ? q.sRemark : "";
            return sremark;
        };

        foreach (var ou in lstOut)
        {
            int UnderProducID = 0;
            int nSpecificID = 0;
            if (ou.ProductID == 4)
            {
                ou.sType = "Group";
                UnderProducID = 2;
                nSpecificID = OperationType == 11 ? 59 : OperationType == 4 ? 71 : OperationType == 14 ? 65 : OperationType == 13 ? 35 : 0;
                ou.sMakeField1 = GetRemarkInput(UnderProducID);  //lstMark.Any() ? lstMark.FirstOrDefault(w => w.ProductID == UnderProducID).sRemark : "";
            }
            else if (ou.ProductID == 7)
            {
                ou.sType = "Group";
                UnderProducID = 8;
                nSpecificID = OperationType == 11 ? 60 : OperationType == 4 ? 72 : OperationType == 14 ? 66 : OperationType == 13 ? 36 : 0;
                ou.sMakeField1 = GetRemarkInput(UnderProducID); // lstMark.Any() ? lstMark.FirstOrDefault(w => w.ProductID == UnderProducID).sRemark : "";
            }
            else if (ou.ProductID == 13)
            {
                ou.sType = "Group";
                UnderProducID = 17;
                nSpecificID = OperationType == 11 ? 62 : OperationType == 4 ? 74 : OperationType == 14 ? 68 : OperationType == 13 ? 38 : 0;
                ou.sMakeField1 = GetRemarkInput(UnderProducID);  //lstMark.Any() ? lstMark.FirstOrDefault(w => w.ProductID == UnderProducID).sRemark : "";
            }
            else if (ou.ProductID == 16)
            {
                ou.sType = "Group";
                UnderProducID = 24;
                nSpecificID = OperationType == 11 ? 63 : OperationType == 4 ? 75 : OperationType == 14 ? 69 : OperationType == 13 ? 39 : 0;
                ou.sMakeField1 = GetRemarkInput(UnderProducID); // lstMark.Any() ? lstMark.FirstOrDefault(w => w.ProductID == UnderProducID).sRemark : "";
            }
            else if (ou.ProductID == 1)
            { ou.sType = "Group"; }
            else if (ou.ProductID == 10)
            { ou.sType = "Group"; }

            if (UnderProducID != 0)
            {
                #region ADD HEAD
                lstHead = (from a in db.mTProductIndicator.Where(w => w.IDIndicator == nIDIndicator && w.nOption == UnderProducID).AsEnumerable()
                           from b in db.TWaste_Product.Where(w => w.ProductID == a.ProductID && w.FormID == nFormID)
                           from c in db.mTUnit.Where(w => w.UnitID == b.UnitID).AsEnumerable().DefaultIfEmpty()
                           select new ClassExecute.TDataOutput
                           {
                               IDIndicator = nIDIndicator,
                               OperationtypeID = OperationType,
                               FacilityID = nFacility,
                               ProductID = a.ProductID,
                               ProductName = a.ProductName,
                               nUnitID = b.UnitID ?? 0,
                               sUnit = "Tonnes",
                               nOrder = SystemFunction.GetDecimalNull(a.nOrder + "") ?? 0,
                               sType = "Head",
                               nHeadID = ou.ProductID,

                               nTarget = SystemFunction.GetDecimalNull(b.Target),
                               nM1 = SystemFunction.GetDecimalNull(b.M1),
                               nM2 = SystemFunction.GetDecimalNull(b.M2),
                               nM3 = SystemFunction.GetDecimalNull(b.M3),
                               nM4 = SystemFunction.GetDecimalNull(b.M4),
                               nM5 = SystemFunction.GetDecimalNull(b.M5),
                               nM6 = SystemFunction.GetDecimalNull(b.M6),
                               nM7 = SystemFunction.GetDecimalNull(b.M7),
                               nM8 = SystemFunction.GetDecimalNull(b.M8),
                               nM9 = SystemFunction.GetDecimalNull(b.M9),
                               nM10 = SystemFunction.GetDecimalNull(b.M10),
                               nM11 = SystemFunction.GetDecimalNull(b.M11),
                               nM12 = SystemFunction.GetDecimalNull(b.M12),
                               nTotal = EPIFunc.SumDataToDecimal(b.M1, b.M2, b.M3, b.M4, b.M5, b.M6, b.M7, b.M8, b.M9, b.M10, b.M11, b.M12),

                               sTarget = SystemFunction.ConvertFormatDecimal4(b.Target),
                               sM1 = SystemFunction.ConvertFormatDecimal4(b.M1),
                               sM2 = SystemFunction.ConvertFormatDecimal4(b.M2),
                               sM3 = SystemFunction.ConvertFormatDecimal4(b.M3),
                               sM4 = SystemFunction.ConvertFormatDecimal4(b.M4),
                               sM5 = SystemFunction.ConvertFormatDecimal4(b.M5),
                               sM6 = SystemFunction.ConvertFormatDecimal4(b.M6),
                               sM7 = SystemFunction.ConvertFormatDecimal4(b.M7),
                               sM8 = SystemFunction.ConvertFormatDecimal4(b.M8),
                               sM9 = SystemFunction.ConvertFormatDecimal4(b.M9),
                               sM10 = SystemFunction.ConvertFormatDecimal4(b.M10),
                               sM11 = SystemFunction.ConvertFormatDecimal4(b.M11),
                               sM12 = SystemFunction.ConvertFormatDecimal4(b.M12),
                               sTotal = SystemFunction.ConvertFormatDecimal4(EPIFunc.SumDataToDecimal(b.M1, b.M2, b.M3, b.M4, b.M5, b.M6, b.M7, b.M8, b.M9, b.M10, b.M11, b.M12).ToString()),

                           }).OrderBy(o => o.sType).ThenBy(n => n.nOrder).ToList();

                if (lstHead.Any())
                {
                    int index = lstData.FindIndex(x => x.ProductID == nSpecificID && x.sType == null) + 1;
                    lstData.InsertRange(index, lstHead);
                }
                else
                {
                    lstHead = (from a in db.mTProductIndicator.Where(w => w.IDIndicator == nIDIndicator && w.nOption == UnderProducID).AsEnumerable()
                               from c in db.mTUnit.Where(w => w.UnitID == 2).AsEnumerable().DefaultIfEmpty()
                               select new ClassExecute.TDataOutput
                               {
                                   IDIndicator = nIDIndicator,
                                   OperationtypeID = OperationType,
                                   FacilityID = nFacility,
                                   ProductID = a.ProductID,
                                   ProductName = a.ProductName,
                                   nUnitID = 2,
                                   sUnit = c.UnitName,
                                   nOrder = SystemFunction.GetDecimalNull(a.nOrder + "") ?? 0,
                                   sType = "Head",
                                   nHeadID = ou.ProductID,

                                   nTarget = 0,
                                   nM1 = 0,
                                   nM2 = 0,
                                   nM3 = 0,
                                   nM4 = 0,
                                   nM5 = 0,
                                   nM6 = 0,
                                   nM7 = 0,
                                   nM8 = 0,
                                   nM9 = 0,
                                   nM10 = 0,
                                   nM11 = 0,
                                   nM12 = 0,
                                   nTotal = 0,

                                   sTarget = "",
                                   sM1 = "",
                                   sM2 = "",
                                   sM3 = "",
                                   sM4 = "",
                                   sM5 = "",
                                   sM6 = "",
                                   sM7 = "",
                                   sM8 = "",
                                   sM9 = "",
                                   sM10 = "",
                                   sM11 = "",
                                   sM12 = "",
                                   sTotal = "",

                               }).OrderBy(o => o.sType).ThenBy(n => n.nOrder).ToList();

                    int index = lstData.FindIndex(x => x.ProductID == nSpecificID && x.sType == null) + 1;
                    lstData.InsertRange(index, lstHead);
                }
                #endregion

                #region ADD SUB
                foreach (var h in lstHead)
                {
                    List<ClassExecute.TDataOutput> lstSub = new List<ClassExecute.TDataOutput>();
                    lstSub = (from a in db.TWaste_Product_data.Where(w => w.UnderProductID == h.ProductID && w.FormID == nFormID).AsEnumerable()
                              from b in db.mTUnit.Where(w => w.UnitID == a.UnitID).AsEnumerable()
                              from t in db.TM_WasteDisposal.Where(w => w.ID == a.nDisposalID).AsEnumerable().DefaultIfEmpty()
                              select new ClassExecute.TDataOutput
                              {
                                  IDIndicator = nIDIndicator,
                                  OperationtypeID = OperationType,
                                  FacilityID = nFacility,
                                  ProductID = a.nSubProductID,
                                  ProductName = a.ProductName,
                                  nUnitID = a.UnitID ?? 0,
                                  sUnit = b.UnitName,
                                  nOrder = 0,
                                  sType = "Sub",
                                  nHeadID = a.UnderProductID,

                                  nTarget = SystemFunction.GetDecimalNull(a.Target),
                                  nM1 = SystemFunction.GetDecimalNull(a.M1),
                                  nM2 = SystemFunction.GetDecimalNull(a.M2),
                                  nM3 = SystemFunction.GetDecimalNull(a.M3),
                                  nM4 = SystemFunction.GetDecimalNull(a.M4),
                                  nM5 = SystemFunction.GetDecimalNull(a.M5),
                                  nM6 = SystemFunction.GetDecimalNull(a.M6),
                                  nM7 = SystemFunction.GetDecimalNull(a.M7),
                                  nM8 = SystemFunction.GetDecimalNull(a.M8),
                                  nM9 = SystemFunction.GetDecimalNull(a.M9),
                                  nM10 = SystemFunction.GetDecimalNull(a.M10),
                                  nM11 = SystemFunction.GetDecimalNull(a.M11),
                                  nM12 = SystemFunction.GetDecimalNull(a.M12),
                                  nTotal = EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12),

                                  sTarget = SystemFunction.ConvertFormatDecimal4(a.Target),
                                  sDisposalName = t == null ? "" : t.sCode + " " + t.sName,
                                  sM1 = SystemFunction.ConvertFormatDecimal4(a.M1),
                                  sM2 = SystemFunction.ConvertFormatDecimal4(a.M2),
                                  sM3 = SystemFunction.ConvertFormatDecimal4(a.M3),
                                  sM4 = SystemFunction.ConvertFormatDecimal4(a.M4),
                                  sM5 = SystemFunction.ConvertFormatDecimal4(a.M5),
                                  sM6 = SystemFunction.ConvertFormatDecimal4(a.M6),
                                  sM7 = SystemFunction.ConvertFormatDecimal4(a.M7),
                                  sM8 = SystemFunction.ConvertFormatDecimal4(a.M8),
                                  sM9 = SystemFunction.ConvertFormatDecimal4(a.M9),
                                  sM10 = SystemFunction.ConvertFormatDecimal4(a.M10),
                                  sM11 = SystemFunction.ConvertFormatDecimal4(a.M11),
                                  sM12 = SystemFunction.ConvertFormatDecimal4(a.M12),
                                  sTotal = SystemFunction.ConvertFormatDecimal4(EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12).ToString()),

                              }).OrderBy(o => o.ProductID).ToList();
                    if (lstSub.Any())
                    {
                        int index = lstData.FindIndex(x => x.ProductID == h.ProductID && x.nHeadID == ou.ProductID && x.sType == "Head") + 1;
                        lstData.InsertRange(index, lstSub);
                        var qData = lstData.FirstOrDefault(w => w.ProductID == h.ProductID && w.nHeadID == ou.ProductID && w.sType == "Head");
                        if (qData != null)
                        {
                            qData.isSub = true;
                        }
                    }
                }
                #endregion
            }

        }

        #region ADD OTHER
        lstHead = db.mTProductIndicator.Where(w => w.IDIndicator == nIDIndicator && w.ProductID == 240).Select(s => new ClassExecute.TDataOutput
        {
            IDIndicator = nIDIndicator,
            OperationtypeID = OperationType,
            FacilityID = nFacility,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nUnitID = 2,
            sUnit = "Tonnes",
            nOrder = 0,
            sType = "Group",
            nHeadID = 0,

            nTarget = 0,
            nM1 = 0,
            nM2 = 0,
            nM3 = 0,
            nM4 = 0,
            nM5 = 0,
            nM6 = 0,
            nM7 = 0,
            nM8 = 0,
            nM9 = 0,
            nM10 = 0,
            nM11 = 0,
            nM12 = 0,
            nTotal = 0,

            sTarget = "",
            sM1 = "",
            sM2 = "",
            sM3 = "",
            sM4 = "",
            sM5 = "",
            sM6 = "",
            sM7 = "",
            sM8 = "",
            sM9 = "",
            sM10 = "",
            sM11 = "",
            sM12 = "",
            sTotal = "",
        }).ToList();

        foreach (var item in lstHead)
        {
            var q = db.TWaste_Product.FirstOrDefault(w => w.ProductID == item.ProductID && w.FormID == nFormID);
            if (q != null)
            {
                item.sMakeField1 = GetRemarkInput(240);
                item.nTarget = SystemFunction.GetDecimalNull(q.Target);
                item.nM1 = SystemFunction.GetDecimalNull(q.M1);
                item.nM2 = SystemFunction.GetDecimalNull(q.M2);
                item.nM3 = SystemFunction.GetDecimalNull(q.M3);
                item.nM4 = SystemFunction.GetDecimalNull(q.M4);
                item.nM5 = SystemFunction.GetDecimalNull(q.M5);
                item.nM6 = SystemFunction.GetDecimalNull(q.M6);
                item.nM7 = SystemFunction.GetDecimalNull(q.M7);
                item.nM8 = SystemFunction.GetDecimalNull(q.M8);
                item.nM9 = SystemFunction.GetDecimalNull(q.M9);
                item.nM10 = SystemFunction.GetDecimalNull(q.M10);
                item.nM11 = SystemFunction.GetDecimalNull(q.M11);
                item.nM12 = SystemFunction.GetDecimalNull(q.M12);
                item.nTotal = EPIFunc.SumDataToDecimal(q.M1, q.M2, q.M3, q.M4, q.M5, q.M6, q.M7, q.M8, q.M9, q.M10, q.M11, q.M12);

                item.sTarget = SystemFunction.ConvertFormatDecimal4(q.Target);
                item.sM1 = SystemFunction.ConvertFormatDecimal4(q.M1);
                item.sM2 = SystemFunction.ConvertFormatDecimal4(q.M2);
                item.sM3 = SystemFunction.ConvertFormatDecimal4(q.M3);
                item.sM4 = SystemFunction.ConvertFormatDecimal4(q.M4);
                item.sM5 = SystemFunction.ConvertFormatDecimal4(q.M5);
                item.sM6 = SystemFunction.ConvertFormatDecimal4(q.M6);
                item.sM7 = SystemFunction.ConvertFormatDecimal4(q.M7);
                item.sM8 = SystemFunction.ConvertFormatDecimal4(q.M8);
                item.sM9 = SystemFunction.ConvertFormatDecimal4(q.M9);
                item.sM10 = SystemFunction.ConvertFormatDecimal4(q.M10);
                item.sM11 = SystemFunction.ConvertFormatDecimal4(q.M11);
                item.sM12 = SystemFunction.ConvertFormatDecimal4(q.M12);
                item.sTotal = EPIFunc.SumDataToDecimal(q.M1, q.M2, q.M3, q.M4, q.M5, q.M6, q.M7, q.M8, q.M9, q.M10, q.M11, q.M12).ToString();
            }

        }

        if (lstHead.Count > 0)
        {
            lstData.AddRange(lstHead);
            List<ClassExecute.TDataOutput> lstSubOther = new List<ClassExecute.TDataOutput>();
            lstSubOther = (from a in db.TWaste_Product_data.Where(w => w.UnderProductID == 240 && w.FormID == nFormID).AsEnumerable()
                           from b in db.mTUnit.Where(w => w.UnitID == a.UnitID).AsEnumerable().DefaultIfEmpty()
                           select new ClassExecute.TDataOutput
                           {
                               IDIndicator = nIDIndicator,
                               OperationtypeID = OperationType,
                               FacilityID = nFacility,
                               ProductID = a.nSubProductID,
                               ProductName = a.ProductName,
                               nUnitID = a.UnitID ?? 0,
                               sUnit = b.UnitName,
                               nOrder = 0,
                               sType = "Sub",
                               nHeadID = a.UnderProductID,

                               nTarget = SystemFunction.GetDecimalNull(a.Target),
                               nM1 = SystemFunction.GetDecimalNull(a.M1),
                               nM2 = SystemFunction.GetDecimalNull(a.M2),
                               nM3 = SystemFunction.GetDecimalNull(a.M3),
                               nM4 = SystemFunction.GetDecimalNull(a.M4),
                               nM5 = SystemFunction.GetDecimalNull(a.M5),
                               nM6 = SystemFunction.GetDecimalNull(a.M6),
                               nM7 = SystemFunction.GetDecimalNull(a.M7),
                               nM8 = SystemFunction.GetDecimalNull(a.M8),
                               nM9 = SystemFunction.GetDecimalNull(a.M9),
                               nM10 = SystemFunction.GetDecimalNull(a.M10),
                               nM11 = SystemFunction.GetDecimalNull(a.M11),
                               nM12 = SystemFunction.GetDecimalNull(a.M12),
                               nTotal = EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12),

                               sTarget = SystemFunction.ConvertFormatDecimal4(a.Target),
                               sM1 = SystemFunction.ConvertFormatDecimal4(a.M1),
                               sM2 = SystemFunction.ConvertFormatDecimal4(a.M2),
                               sM3 = SystemFunction.ConvertFormatDecimal4(a.M3),
                               sM4 = SystemFunction.ConvertFormatDecimal4(a.M4),
                               sM5 = SystemFunction.ConvertFormatDecimal4(a.M5),
                               sM6 = SystemFunction.ConvertFormatDecimal4(a.M6),
                               sM7 = SystemFunction.ConvertFormatDecimal4(a.M7),
                               sM8 = SystemFunction.ConvertFormatDecimal4(a.M8),
                               sM9 = SystemFunction.ConvertFormatDecimal4(a.M9),
                               sM10 = SystemFunction.ConvertFormatDecimal4(a.M10),
                               sM11 = SystemFunction.ConvertFormatDecimal4(a.M11),
                               sM12 = SystemFunction.ConvertFormatDecimal4(a.M12),
                               sTotal = EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12).ToString(),

                           }).OrderBy(o => o.ProductID).ToList();
            if (lstSubOther.Count > 0)
            {
                lstData.AddRange(lstSubOther);
            }
        }
        #endregion

        return lstData;
    }

    public static List<ClassExecute.TDataOutput> GetMaterialDataOutput(int nFormID, int nIDIndicator, int OperationType, int nFacility, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataOutput> lstOut = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstData = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstHead = new List<ClassExecute.TDataOutput>();

        lstOut = FunctionGetData.GetDataOutput(nIDIndicator, OperationType, nFacility, sYear);
        lstData = lstOut.ToList();

        var lstMark = db.TMaterial_Remark.Where(w => w.FormID == nFormID).OrderByDescending(o => o.nVersion).ToList();

        Func<int, string> GetRemarkInput = (productid) =>
        {
            string sremark = "";
            var q = lstMark.FirstOrDefault(w => w.ProductID == productid);
            sremark = q != null ? q.sRemark : "";
            return sremark;
        };

        foreach (var ou in lstOut)
        {
            int nINProducID = 0;
            int nSpecificID = 0;
            if (ou.ProductID == 88)//Total Direct Materials Used
            {
                ou.sType = "Group";
                nINProducID = 34;
                nSpecificID = OperationType == 11 ? 113 : OperationType == 4 ? 119 : OperationType == 14 ? 116 : OperationType == 13 ? 101 : 0;
                ou.sMakeField1 = GetRemarkInput(nINProducID);
            }
            else if (ou.ProductID == 91)//Total Associated Materials Used
            {
                ou.sType = "Group";
                nINProducID = 37;
                nSpecificID = OperationType == 11 ? 114 : OperationType == 4 ? 120 : OperationType == 14 ? 117 : OperationType == 13 ? 102 : 0;
                ou.sMakeField1 = GetRemarkInput(nINProducID);
            }
            else if (ou.ProductID == 97)//Recycled Input Material Used
            {/// เป็นกลุ่มที่ไม่มี Head มี sub เลย
                ou.sType = "Group";
                nINProducID = 41;
                nSpecificID = 0;
            }
            else if (ou.ProductID == 94) /// Total Materials Used
            {
                nINProducID = 33;
                ou.sType = "Group";
                ou.sMakeField1 = GetRemarkInput(nINProducID);
            }

            if (nINProducID != 0)
            {
                #region ADD id Product Head
                List<int> lstINProducID = new List<int>();
                if (nINProducID == 34)
                {
                    lstINProducID.Add(36);
                    lstINProducID.Add(35);
                }
                else if (nINProducID == 37)
                {
                    lstINProducID.Add(40);
                    lstINProducID.Add(39);
                    lstINProducID.Add(38);

                }
                #endregion

                if (lstINProducID.Count > 0)
                {
                    foreach (var p in lstINProducID)
                    {
                        #region ADD HEAD
                        lstHead = (from a in db.mTProductIndicator.Where(w => w.IDIndicator == nIDIndicator && w.ProductID == p).AsEnumerable()
                                   from b in db.TMaterial_Product.Where(w => w.ProductID == a.ProductID && w.FormID == nFormID)
                                   from c in db.mTUnit.Where(w => w.UnitID == (b.UnitID ?? 2)).AsEnumerable().DefaultIfEmpty()
                                   select new ClassExecute.TDataOutput
                                   {
                                       IDIndicator = nIDIndicator,
                                       OperationtypeID = OperationType,
                                       FacilityID = nFacility,
                                       ProductID = a.ProductID,
                                       ProductName = a.ProductName,
                                       nUnitID = (b.UnitID ?? 2),
                                       sUnit = c.UnitName,
                                       nOrder = SystemFunction.GetDecimalNull(a.nOrder + "") ?? 0,
                                       sType = "Head",
                                       nHeadID = ou.ProductID,

                                       nTarget = SystemFunction.GetDecimalNull(b.Target),
                                       nM1 = SystemFunction.GetDecimalNull(b.M1),
                                       nM2 = SystemFunction.GetDecimalNull(b.M2),
                                       nM3 = SystemFunction.GetDecimalNull(b.M3),
                                       nM4 = SystemFunction.GetDecimalNull(b.M4),
                                       nM5 = SystemFunction.GetDecimalNull(b.M5),
                                       nM6 = SystemFunction.GetDecimalNull(b.M6),
                                       nM7 = SystemFunction.GetDecimalNull(b.M7),
                                       nM8 = SystemFunction.GetDecimalNull(b.M8),
                                       nM9 = SystemFunction.GetDecimalNull(b.M9),
                                       nM10 = SystemFunction.GetDecimalNull(b.M10),
                                       nM11 = SystemFunction.GetDecimalNull(b.M11),
                                       nM12 = SystemFunction.GetDecimalNull(b.M12),
                                       nTotal = SystemFunction.GetDecimalNull(b.nTotal),

                                       sTarget = SystemFunction.ConvertFormatDecimal4(b.Target),
                                       sM1 = SystemFunction.ConvertFormatDecimal4(b.M1),
                                       sM2 = SystemFunction.ConvertFormatDecimal4(b.M2),
                                       sM3 = SystemFunction.ConvertFormatDecimal4(b.M3),
                                       sM4 = SystemFunction.ConvertFormatDecimal4(b.M4),
                                       sM5 = SystemFunction.ConvertFormatDecimal4(b.M5),
                                       sM6 = SystemFunction.ConvertFormatDecimal4(b.M6),
                                       sM7 = SystemFunction.ConvertFormatDecimal4(b.M7),
                                       sM8 = SystemFunction.ConvertFormatDecimal4(b.M8),
                                       sM9 = SystemFunction.ConvertFormatDecimal4(b.M9),
                                       sM10 = SystemFunction.ConvertFormatDecimal4(b.M10),
                                       sM11 = SystemFunction.ConvertFormatDecimal4(b.M11),
                                       sM12 = SystemFunction.ConvertFormatDecimal4(b.M12),
                                       sTotal = SystemFunction.ConvertFormatDecimal4(b.nTotal),

                                   }).OrderBy(o => o.sType).ThenBy(n => n.nOrder).ToList();

                        if (lstHead.Any())
                        {
                            int index = lstData.FindIndex(x => x.ProductID == nSpecificID && x.sType == null) + 1;
                            lstData.InsertRange(index, lstHead);
                        }
                        else
                        {
                            lstHead = (from a in db.mTProductIndicator.Where(w => w.IDIndicator == nIDIndicator && w.ProductID == p).AsEnumerable()
                                       from c in db.mTUnit.Where(w => w.UnitID == 2).AsEnumerable().DefaultIfEmpty()
                                       select new ClassExecute.TDataOutput
                                       {
                                           IDIndicator = nIDIndicator,
                                           OperationtypeID = OperationType,
                                           FacilityID = nFacility,
                                           ProductID = a.ProductID,
                                           ProductName = a.ProductName,
                                           nUnitID = 2,
                                           sUnit = c.UnitName,
                                           nOrder = SystemFunction.GetDecimalNull(a.nOrder + "") ?? 0,
                                           sType = "Head",
                                           nHeadID = ou.ProductID,

                                           nTarget = 0,
                                           nM1 = 0,
                                           nM2 = 0,
                                           nM3 = 0,
                                           nM4 = 0,
                                           nM5 = 0,
                                           nM6 = 0,
                                           nM7 = 0,
                                           nM8 = 0,
                                           nM9 = 0,
                                           nM10 = 0,
                                           nM11 = 0,
                                           nM12 = 0,
                                           nTotal = 0,

                                           sTarget = "",
                                           sM1 = "",
                                           sM2 = "",
                                           sM3 = "",
                                           sM4 = "",
                                           sM5 = "",
                                           sM6 = "",
                                           sM7 = "",
                                           sM8 = "",
                                           sM9 = "",
                                           sM10 = "",
                                           sM11 = "",
                                           sM12 = "",
                                           sTotal = "",

                                       }).OrderBy(o => o.sType).ThenBy(n => n.nOrder).ToList();

                            int index = lstData.FindIndex(x => x.ProductID == nSpecificID && x.sType == null) + 1;
                            lstData.InsertRange(index, lstHead);
                        }
                        #endregion

                        #region ADD SUB
                        foreach (var h in lstHead)
                        {
                            List<ClassExecute.TDataOutput> lstSub = new List<ClassExecute.TDataOutput>();
                            lstSub = (from a in db.TMaterial_ProductData.Where(w => w.nUnderbyProductID == h.ProductID && w.FormID == nFormID).AsEnumerable()
                                      from b in db.mTUnit.Where(w => w.UnitID == a.nUnitID).AsEnumerable().DefaultIfEmpty()
                                      select new ClassExecute.TDataOutput
                                      {
                                          IDIndicator = nIDIndicator,
                                          OperationtypeID = OperationType,
                                          FacilityID = nFacility,
                                          ProductID = a.nSubProductID,
                                          ProductName = a.sName,
                                          nUnitID = a.nUnitID ?? 0,
                                          sUnit = b.UnitName,
                                          nOrder = 0,
                                          sType = "Sub",
                                          nHeadID = a.nUnderbyProductID,

                                          nTarget = SystemFunction.GetDecimalNull(a.Target),
                                          nM1 = SystemFunction.GetDecimalNull(a.M1),
                                          nM2 = SystemFunction.GetDecimalNull(a.M2),
                                          nM3 = SystemFunction.GetDecimalNull(a.M3),
                                          nM4 = SystemFunction.GetDecimalNull(a.M4),
                                          nM5 = SystemFunction.GetDecimalNull(a.M5),
                                          nM6 = SystemFunction.GetDecimalNull(a.M6),
                                          nM7 = SystemFunction.GetDecimalNull(a.M7),
                                          nM8 = SystemFunction.GetDecimalNull(a.M8),
                                          nM9 = SystemFunction.GetDecimalNull(a.M9),
                                          nM10 = SystemFunction.GetDecimalNull(a.M10),
                                          nM11 = SystemFunction.GetDecimalNull(a.M11),
                                          nM12 = SystemFunction.GetDecimalNull(a.M12),
                                          nTotal = EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12),

                                          sTarget = SystemFunction.ConvertFormatDecimal4(a.Target),
                                          sM1 = SystemFunction.ConvertFormatDecimal4(a.M1),
                                          sM2 = SystemFunction.ConvertFormatDecimal4(a.M2),
                                          sM3 = SystemFunction.ConvertFormatDecimal4(a.M3),
                                          sM4 = SystemFunction.ConvertFormatDecimal4(a.M4),
                                          sM5 = SystemFunction.ConvertFormatDecimal4(a.M5),
                                          sM6 = SystemFunction.ConvertFormatDecimal4(a.M6),
                                          sM7 = SystemFunction.ConvertFormatDecimal4(a.M7),
                                          sM8 = SystemFunction.ConvertFormatDecimal4(a.M8),
                                          sM9 = SystemFunction.ConvertFormatDecimal4(a.M9),
                                          sM10 = SystemFunction.ConvertFormatDecimal4(a.M10),
                                          sM11 = SystemFunction.ConvertFormatDecimal4(a.M11),
                                          sM12 = SystemFunction.ConvertFormatDecimal4(a.M12),
                                          sTotal = SystemFunction.ConvertFormatDecimal4(EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12).ToString()),

                                      }).OrderBy(o => o.ProductID).ToList();
                            if (lstSub.Any())
                            {
                                int index = lstData.FindIndex(x => x.ProductID == h.ProductID && x.nHeadID == ou.ProductID && x.sType == "Head") + 1;
                                lstData.InsertRange(index, lstSub);
                                var qData = lstData.FirstOrDefault(w => w.ProductID == h.ProductID && w.nHeadID == ou.ProductID && w.sType == "Head");
                                if (qData != null)
                                {
                                    qData.isSub = true;
                                }
                            }
                        }
                        #endregion
                    }
                }
                else if (lstINProducID.Count == 0 && nINProducID == 41)
                {
                    #region ADD SUB

                    List<ClassExecute.TDataOutput> lstSub = new List<ClassExecute.TDataOutput>();
                    lstSub = (from a in db.TMaterial_ProductData.Where(w => w.nUnderbyProductID == nINProducID && w.FormID == nFormID).AsEnumerable()
                              from b in db.mTUnit.Where(w => w.UnitID == a.nUnitID).AsEnumerable().DefaultIfEmpty()
                              select new ClassExecute.TDataOutput
                              {
                                  IDIndicator = nIDIndicator,
                                  OperationtypeID = OperationType,
                                  FacilityID = nFacility,
                                  ProductID = a.nSubProductID,
                                  ProductName = a.sName,
                                  nUnitID = a.nUnitID ?? 0,
                                  sUnit = b.UnitName,
                                  nOrder = 0,
                                  sType = "Sub",
                                  nHeadID = a.nUnderbyProductID,

                                  nTarget = SystemFunction.GetDecimalNull(a.Target),
                                  nM1 = SystemFunction.GetDecimalNull(a.M1),
                                  nM2 = SystemFunction.GetDecimalNull(a.M2),
                                  nM3 = SystemFunction.GetDecimalNull(a.M3),
                                  nM4 = SystemFunction.GetDecimalNull(a.M4),
                                  nM5 = SystemFunction.GetDecimalNull(a.M5),
                                  nM6 = SystemFunction.GetDecimalNull(a.M6),
                                  nM7 = SystemFunction.GetDecimalNull(a.M7),
                                  nM8 = SystemFunction.GetDecimalNull(a.M8),
                                  nM9 = SystemFunction.GetDecimalNull(a.M9),
                                  nM10 = SystemFunction.GetDecimalNull(a.M10),
                                  nM11 = SystemFunction.GetDecimalNull(a.M11),
                                  nM12 = SystemFunction.GetDecimalNull(a.M12),
                                  nTotal = EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12),

                                  sTarget = SystemFunction.ConvertFormatDecimal4(a.Target),
                                  sM1 = SystemFunction.ConvertFormatDecimal4(a.M1),
                                  sM2 = SystemFunction.ConvertFormatDecimal4(a.M2),
                                  sM3 = SystemFunction.ConvertFormatDecimal4(a.M3),
                                  sM4 = SystemFunction.ConvertFormatDecimal4(a.M4),
                                  sM5 = SystemFunction.ConvertFormatDecimal4(a.M5),
                                  sM6 = SystemFunction.ConvertFormatDecimal4(a.M6),
                                  sM7 = SystemFunction.ConvertFormatDecimal4(a.M7),
                                  sM8 = SystemFunction.ConvertFormatDecimal4(a.M8),
                                  sM9 = SystemFunction.ConvertFormatDecimal4(a.M9),
                                  sM10 = SystemFunction.ConvertFormatDecimal4(a.M10),
                                  sM11 = SystemFunction.ConvertFormatDecimal4(a.M11),
                                  sM12 = SystemFunction.ConvertFormatDecimal4(a.M12),
                                  sTotal = SystemFunction.ConvertFormatDecimal4(EPIFunc.SumDataToDecimal(a.M1, a.M2, a.M3, a.M4, a.M5, a.M6, a.M7, a.M8, a.M9, a.M10, a.M11, a.M12).ToString()),

                              }).OrderBy(o => o.ProductID).ToList();
                    if (lstSub.Any())
                    {
                        int index = lstData.FindIndex(x => x.ProductID == ou.ProductID && x.sType == "Group") + 1;
                        lstData.InsertRange(index, lstSub);
                        var qData = lstData.FirstOrDefault(w => w.ProductID == ou.ProductID && w.sType == "Group");
                        if (qData != null)
                        {
                            qData.isSub = true;
                        }
                    }

                    #endregion
                }
            }
        }

        return lstData;
    }

    public static List<ClassExecute.TDataOutput> GetIntensityDataOutput(int nFormID, int nIDIndicator, int OperationType, int nFacility, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataOutput> lstData = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstHead = new List<ClassExecute.TDataOutput>();

        #region OLD

        //var lstMark = db.TIntensity_Remark.Where(w => w.FormID == nFormID).OrderByDescending(o => o.nVersion).ToList();
        //lstHead = (from i in db.mTProductIndicator.Where(w => w.IDIndicator.Value == 6).AsEnumerable()
        //           from u in db.TIntensityUseProduct.Where(w => w.OperationTypeID == OperationType && w.ProductID == i.ProductID).AsEnumerable()
        //           from d in db.TIntensityDominator.Where(w => w.ProductID == i.ProductID && w.FormID == nFormID).AsEnumerable().DefaultIfEmpty()
        //           select new ClassExecute.TDataOutput
        //           {
        //               IDIndicator = nIDIndicator,
        //               OperationtypeID = OperationType,
        //               FacilityID = nFacility,
        //               ProductID = i.ProductID,
        //               ProductName = i.ProductName,
        //               // nUnitID = b.UnitID ?? 0,
        //               sUnit = i.sUnit,
        //               nOrder = SystemFunction.GetDecimalNull(u.nOrder + "") ?? 0,
        //               sType = i.ProductID == 89 ? "Sub" : i.ProductID == 90 ? "Sub" : i.ProductID == 86 ? "TotalArea" : i.cTotalAll == "Y" ? "Group" : i.cTotal == "N" && i.cTotalAll == "N" ? "SubHead" : "Head",
        //               nHeadID = i.ProductID == 89 ? 88 : i.ProductID == 90 ? 88 : 0,
        //               isSub = i.ProductID == 88 ? true : false,
        //               sTotalArea = OperationType == 14 ? d.nTotal : "",
        //               sMakeField1 = "", // แก้ใหม่ lstMark.Any() ? !string.IsNullOrEmpty(lstMark.FirstOrDefault(w => w.ProductID == i.ProductID).sRemark) ? lstMark.FirstOrDefault(w => w.ProductID == i.ProductID).sRemark : "" : "",

        //               nTarget = SystemFunction.GetDecimalNull(d.Target),
        //               nM1 = SystemFunction.GetDecimalNull(d.M1),
        //               nM2 = SystemFunction.GetDecimalNull(d.M2),
        //               nM3 = SystemFunction.GetDecimalNull(d.M3),
        //               nM4 = SystemFunction.GetDecimalNull(d.M4),
        //               nM5 = SystemFunction.GetDecimalNull(d.M5),
        //               nM6 = SystemFunction.GetDecimalNull(d.M6),
        //               nM7 = SystemFunction.GetDecimalNull(d.M7),
        //               nM8 = SystemFunction.GetDecimalNull(d.M8),
        //               nM9 = SystemFunction.GetDecimalNull(d.M9),
        //               nM10 = SystemFunction.GetDecimalNull(d.M10),
        //               nM11 = SystemFunction.GetDecimalNull(d.M11),
        //               nM12 = SystemFunction.GetDecimalNull(d.M12),
        //               nTotal = EPIFunc.SumDataToDecimal(d.M1, d.M2, d.M3, d.M4, d.M5, d.M6, d.M7, d.M8, d.M9, d.M10, d.M11, d.M12),

        //               sTarget = SystemFunction.ConvertFormatDecimal4(d.Target),
        //               sM1 = SystemFunction.ConvertFormatDecimal4(d.M1),
        //               sM2 = SystemFunction.ConvertFormatDecimal4(d.M2),
        //               sM3 = SystemFunction.ConvertFormatDecimal4(d.M3),
        //               sM4 = SystemFunction.ConvertFormatDecimal4(d.M4),
        //               sM5 = SystemFunction.ConvertFormatDecimal4(d.M5),
        //               sM6 = SystemFunction.ConvertFormatDecimal4(d.M6),
        //               sM7 = SystemFunction.ConvertFormatDecimal4(d.M7),
        //               sM8 = SystemFunction.ConvertFormatDecimal4(d.M8),
        //               sM9 = SystemFunction.ConvertFormatDecimal4(d.M9),
        //               sM10 = SystemFunction.ConvertFormatDecimal4(d.M10),
        //               sM11 = SystemFunction.ConvertFormatDecimal4(d.M11),
        //               sM12 = SystemFunction.ConvertFormatDecimal4(d.M12),
        //               sTotal = EPIFunc.SumDataToDecimal(d.M1, d.M2, d.M3, d.M4, d.M5, d.M6, d.M7, d.M8, d.M9, d.M10, d.M11, d.M12).ToString(),
        //           }).OrderBy(k => k.nOrder).ToList();
        #endregion

        var lstMark = db.TIntensity_Remark.Where(w => w.FormID == nFormID).OrderByDescending(o => o.nVersion).ToList();
        Func<int, string> GetRemarkInput = (productid) =>
        {
            string sremark = "";
            var q = lstMark.FirstOrDefault(w => w.ProductID == productid);
            sremark = q != null ? q.sRemark : "";
            return sremark;
        };

        lstHead = (from i in db.mTProductIndicator.Where(w => w.IDIndicator.Value == nIDIndicator).AsEnumerable()
                   from u in db.TIntensityUseProduct.Where(w => w.OperationTypeID == OperationType && w.ProductID == i.ProductID).AsEnumerable()
                   from d in db.TIntensityDominator.Where(w => w.ProductID == i.ProductID && w.FormID == nFormID).AsEnumerable().DefaultIfEmpty()
                   from s in db.mTProductIndicatorUnit.Where(w => w.ProductID == u.ProductID && w.IDIndicator == i.IDIndicator).DefaultIfEmpty()
                   from un in db.mTUnit.Where(w => w.UnitID == s.UnitID).DefaultIfEmpty()
                   select new ClassExecute.TDataOutput
                   {
                       ProductID = i.ProductID,
                       ProductName = i.ProductName,

                       IDIndicator = nIDIndicator,
                       OperationtypeID = OperationType,
                       FacilityID = nFacility,
                       sUnit = i.sUnit,
                       nOrder = SystemFunction.GetDecimalNull(u.nOrder + "") ?? 0,
                       sType = i.ProductID == 89 ? "Sub" : i.ProductID == 90 ? "Sub" : i.ProductID == 86 ? "TotalArea" : i.cTotalAll == "Y" ? "Group" : i.cTotal == "N" && i.cTotalAll == "N" ? "SubHead" : "Head",
                       nHeadID = i.ProductID == 89 ? 88 : i.ProductID == 90 ? 88 : 0,
                       isSub = i.ProductID == 88 ? true : false,
                       sTotalArea = nFormID == 0 ? "" : OperationType == 14 ? d.nTotal : "",


                       sTarget = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.Target),
                       sM1 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M1),
                       sM2 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M2),
                       sM3 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M3),
                       sM4 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M4),
                       sM5 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M5),
                       sM6 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M6),
                       sM7 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M7),
                       sM8 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M8),
                       sM9 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M9),
                       sM10 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M10),
                       sM11 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M11),
                       sM12 = nFormID == 0 ? "" : SystemFunction.ConvertFormatDecimal4(d.M12),
                       sTotal = nFormID == 0 ? "" : GetValMonthMax(d.M12, d.M11, d.M10, d.M9, d.M8, d.M7, d.M6, d.M5, d.M4, d.M3, d.M2, d.M1, i.ProductID),
                       sMakeField1 = nFormID == 0 ? "" : GetRemarkInput(i.ProductID),
                   }).OrderBy(k => k.nOrder).ToList();
        lstData = lstHead.ToList();

        foreach (var h in lstHead)
        {
            var lstSub = db.TIntensity_Other.Where(w => w.FormID == nFormID && w.UnderProductID == h.ProductID).AsEnumerable()
                .Select(s => new ClassExecute.TDataOutput
                {
                    IDIndicator = nIDIndicator,
                    OperationtypeID = OperationType,
                    FacilityID = nFacility,
                    ProductID = s.ProductID,
                    ProductName = s.ProductName,
                    // nUnitID = b.UnitID ?? 0,
                    sUnit = h.sUnit,
                    //nOrder = SystemFunction.GetDecimalNull(i.nOrder + "") ?? 0,
                    sType = "Sub",
                    nHeadID = h.ProductID,

                    nTarget = SystemFunction.GetDecimalNull(s.Target),
                    nM1 = SystemFunction.GetDecimalNull(s.M1),
                    nM2 = SystemFunction.GetDecimalNull(s.M2),
                    nM3 = SystemFunction.GetDecimalNull(s.M3),
                    nM4 = SystemFunction.GetDecimalNull(s.M4),
                    nM5 = SystemFunction.GetDecimalNull(s.M5),
                    nM6 = SystemFunction.GetDecimalNull(s.M6),
                    nM7 = SystemFunction.GetDecimalNull(s.M7),
                    nM8 = SystemFunction.GetDecimalNull(s.M8),
                    nM9 = SystemFunction.GetDecimalNull(s.M9),
                    nM10 = SystemFunction.GetDecimalNull(s.M10),
                    nM11 = SystemFunction.GetDecimalNull(s.M11),
                    nM12 = SystemFunction.GetDecimalNull(s.M12),
                    nTotal = EPIFunc.SumDataToDecimal(s.M1, s.M2, s.M3, s.M4, s.M5, s.M6, s.M7, s.M8, s.M9, s.M10, s.M11, s.M12),

                    sTarget = SystemFunction.ConvertFormatDecimal4(s.Target),
                    sM1 = SystemFunction.ConvertFormatDecimal4(s.M1),
                    sM2 = SystemFunction.ConvertFormatDecimal4(s.M2),
                    sM3 = SystemFunction.ConvertFormatDecimal4(s.M3),
                    sM4 = SystemFunction.ConvertFormatDecimal4(s.M4),
                    sM5 = SystemFunction.ConvertFormatDecimal4(s.M5),
                    sM6 = SystemFunction.ConvertFormatDecimal4(s.M6),
                    sM7 = SystemFunction.ConvertFormatDecimal4(s.M7),
                    sM8 = SystemFunction.ConvertFormatDecimal4(s.M8),
                    sM9 = SystemFunction.ConvertFormatDecimal4(s.M9),
                    sM10 = SystemFunction.ConvertFormatDecimal4(s.M10),
                    sM11 = SystemFunction.ConvertFormatDecimal4(s.M11),
                    sM12 = SystemFunction.ConvertFormatDecimal4(s.M12),
                    sTotal = SystemFunction.ConvertFormatDecimal4(EPIFunc.SumDataToDecimal(s.M1, s.M2, s.M3, s.M4, s.M5, s.M6, s.M7, s.M8, s.M9, s.M10, s.M11, s.M12).ToString()),
                }).ToList();

            if (lstSub.Any())
            {
                int index = lstData.FindIndex(x => x.ProductID == h.ProductID && x.sType == "Head") + 1;
                lstData.InsertRange(index, lstSub);
                var qData = lstData.FirstOrDefault(w => w.ProductID == h.ProductID && w.sType == "Head");
                if (qData != null)
                {
                    qData.isSub = true;
                }
            }
        }
        return lstData;
    }
    public static List<ClassExecute.TDataOutput> GetWaterDataOutput(int nFormID, int nIDIndicator, int OperationType, int nFacility, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataOutput> lstOut = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstData = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstHead = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TData_Water> lstDataWater = new List<ClassExecute.TData_Water>();
        lstOut = FunctionGetData.GetDataOutput(nIDIndicator, OperationType, nFacility, sYear);
        lstData = lstOut.ToList();
        int[] lstUnit = new int[] { 1, 2, 3 };
        var lstDataUnit = db.mTUnit.Where(w => lstUnit.Contains(w.UnitID)).OrderByDescending(o => o.nFactor).ThenByDescending(o => o.UnitName).Select(s => new { nUnitID = s.UnitID, sUnitName = s.UnitName }).ToList();

        db.mTProductIndicator.Where(w => w.IDIndicator == 11).ToList().ForEach(f =>
        {
            ClassExecute.TData_Water item = new ClassExecute.TData_Water();
            switch (f.ProductID)
            {
                case 91:
                    item.nHeaderID = null;
                    item.nLevel = 1;
                    item.sType = "Group";
                    break;
                case 92:
                    item.nHeaderID = 91;
                    item.nLevel = 2;
                    item.sType = "Group";
                    break;
                case 93:
                    item.nHeaderID = 92;
                    item.nLevel = 3;
                    item.sType = "Head";
                    break;
                case 94:
                    item.nHeaderID = 92;
                    item.nLevel = 3;
                    item.sType = "Head";
                    break;
                case 95:
                    item.nHeaderID = 92;
                    item.nLevel = 3;
                    item.sType = "Head";
                    break;
                case 96:
                    item.nHeaderID = 92;
                    item.nLevel = 3;
                    item.sType = "Head";
                    break;
                case 97:
                    item.nHeaderID = 91;
                    item.nLevel = 2;
                    item.sType = "Group";
                    break;
                case 98:
                    item.nHeaderID = 97;
                    item.nLevel = 3;
                    item.sType = "Head";
                    break;
                case 99:
                    item.nHeaderID = 97;
                    item.nLevel = 3;
                    item.sType = "Head";
                    break;
                case 100:
                    item.nHeaderID = 91;
                    item.nLevel = 2;
                    item.sType = "Group";
                    break;
                case 101:
                    item.nHeaderID = null;
                    item.nLevel = 2;
                    item.sType = "Group";
                    break;
                case 102:
                    item.nHeaderID = 101;
                    item.nLevel = 3;
                    item.sType = "Head";
                    break;
                case 103:
                    item.nHeaderID = 101;
                    item.nLevel = 3;
                    item.sType = "Head";
                    break;
            }
            item.ProductID = f.ProductID;
            item.ProductName = f.ProductName;
            item.sUnit = f.sUnit;
            item.cTotal = f.cTotal;
            item.cTotalAll = f.cTotalAll;
            lstDataWater.Add(item);
        });

        var lstTWater_Product = db.TWater_Product.Where(w => w.FormID == nFormID).ToList();
        var lstWater_ProductData = db.TWater_ProductData.Where(w => w.FormID == nFormID).ToList();
        List<ClassExecute.TData_Water> lstOutDataWater = new List<ClassExecute.TData_Water>();
        List<int?> lstPrdIDWaterReuse = new List<int?>() { 91, 101, 102, 103 };
        lstOutDataWater.AddRange(lstDataWater);
        lstDataWater.ForEach(f =>
        {
            if (f.nLevel <= 3)
            {
                var item = lstTWater_Product.FirstOrDefault(w => w.ProductID == f.ProductID);
                if (item != null)
                {
                    f.M1 = item.M1;
                    f.M2 = item.M2;
                    f.M3 = item.M3;
                    f.M4 = item.M4;
                    f.M5 = item.M5;
                    f.M6 = item.M6;
                    f.M7 = item.M7;
                    f.M8 = item.M8;
                    f.M9 = item.M9;
                    f.M10 = item.M10;
                    f.M11 = item.M11;
                    f.M12 = item.M12;
                    f.Target = item.Target ?? "";
                    f.nTotal = item.nTotal;
                }
                else
                {
                    f.M1 = "";
                    f.M2 = "";
                    f.M3 = "";
                    f.M4 = "";
                    f.M5 = "";
                    f.M6 = "";
                    f.M7 = "";
                    f.M8 = "";
                    f.M9 = "";
                    f.M10 = "";
                    f.M11 = "";
                    f.M12 = "";
                    f.Target = "";
                    f.nTotal = "";
                }
                List<ClassExecute.TData_Water> lstPrdData = new List<ClassExecute.TData_Water>();
                lstWater_ProductData.Where(w => w.nUnderbyProductID == f.ProductID).ToList().ForEach(data =>
                        {
                            var dataUnit = lstDataUnit.FirstOrDefault(w => w.nUnitID == data.nUnitID);
                            ClassExecute.TData_Water prdData = new ClassExecute.TData_Water();
                            prdData.sType = "Sub";
                            prdData.nLevel = 4;
                            prdData.ProductID = data.nSubProductID;
                            prdData.ProductName = data.sName;
                            prdData.nHeaderID = data.nUnderbyProductID;
                            prdData.sUnit = dataUnit != null ? dataUnit.sUnitName : "";
                            prdData.cTotal = "N";
                            prdData.cTotalAll = "N";
                            prdData.Target = data.Target ?? "";
                            prdData.M1 = data.M1;
                            prdData.M2 = data.M2;
                            prdData.M3 = data.M3;
                            prdData.M4 = data.M4;
                            prdData.M5 = data.M5;
                            prdData.M6 = data.M6;
                            prdData.M7 = data.M7;
                            prdData.M8 = data.M8;
                            prdData.M9 = data.M9;
                            prdData.M10 = data.M10;
                            prdData.M11 = data.M11;
                            prdData.M12 = data.M12;
                            prdData.nTotal = item.nTotal;
                            lstPrdData.Add(prdData);
                        });
                if (lstPrdData.Count() > 0)
                {
                    int index = lstOutDataWater.FindIndex(x => x.ProductID == f.ProductID && x.nHeaderID == f.nHeaderID) + 1;
                    f.isSub = true;
                    lstOutDataWater.InsertRange(index, lstPrdData);
                }
            }
        });
        lstOut = new List<ClassExecute.TDataOutput>();
        lstOut = lstOutDataWater.Where(w => (!lstPrdIDWaterReuse.Contains(w.ProductID) && w.nLevel < 4) || (!lstPrdIDWaterReuse.Contains(w.nHeaderID) && w.sType == "Sub")).Select(s => new ClassExecute.TDataOutput
        {
            nHeadID = s.nHeaderID.Value,
            ProductID = s.ProductID.Value,
            ProductName = s.ProductName,
            sUnit = s.sUnit,
            sType = s.sType,
            sTotal = s.nTotal,
            sM1 = s.M1,
            sM2 = s.M2,
            sM3 = s.M3,
            sM4 = s.M4,
            sM5 = s.M5,
            sM6 = s.M6,
            sM7 = s.M7,
            sM8 = s.M8,
            sM9 = s.M9,
            sM10 = s.M10,
            sM11 = s.M11,
            sM12 = s.M12,
            isSub = s.isSub.HasValue ? s.isSub.Value : false,
        }).ToList();
        int nIndex = lstData.FindIndex(x => x.ProductID == 79);
        lstData.InsertRange(nIndex, lstOut);

        lstOut = new List<ClassExecute.TDataOutput>();
        lstPrdIDWaterReuse.RemoveAt(0);
        lstOut = lstOutDataWater.Where(w => (lstPrdIDWaterReuse.Contains(w.nHeaderID) || lstPrdIDWaterReuse.Where(w2 => w2 != 101).ToList().Contains(w.ProductID))).Select(s => new ClassExecute.TDataOutput
        {
            nHeadID = s.nHeaderID.Value,
            ProductID = s.ProductID.Value,
            ProductName = s.ProductName,
            sUnit = s.sUnit,
            sType = s.sType,
            sTotal = s.nTotal,
            sM1 = s.M1,
            sM2 = s.M2,
            sM3 = s.M3,
            sM4 = s.M4,
            sM5 = s.M5,
            sM6 = s.M6,
            sM7 = s.M7,
            sM8 = s.M8,
            sM9 = s.M9,
            sM10 = s.M10,
            sM11 = s.M11,
            sM12 = s.M12,
            isSub = s.isSub.HasValue ? s.isSub.Value : false,
        }).ToList();
        nIndex = lstData.FindIndex(x => x.ProductID == 79) + 1;
        lstData.InsertRange(nIndex, lstOut);
        lstData.ForEach(f =>
        {
            if (f.ProductID == 76 || f.ProductID == 79)
            {
                f.sType = "Group";
                f.sMakeField1 = f.ProductID == 76 ? GetRemarkWater(nFormID, 91) : GetRemarkWater(nFormID, 101);
            }
        });
        return lstData;
    }
    public static List<ClassExecute.TDataOutput> GetComplianceDataOutput(int nFormID, int nIDIndicator, int OperationType, int nFacility, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataOutput> lstOut = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstData = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TDataOutput> lstHead = new List<ClassExecute.TDataOutput>();
        lstOut = FunctionGetData.GetDataOutput(nIDIndicator, OperationType, nFacility, sYear);

        lstData = lstOut.ToList();
        return lstData;
    }
    public static List<ClassExecute.TDataOutput> GetComplaintDataOutput(int nFormID, int nIDIndicator, int OperationType, int nFacility, string sYear)
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TDataOutput> lstData = new List<ClassExecute.TDataOutput>();
        lstData = FunctionGetData.GetDataOutput(nIDIndicator, OperationType, nFacility, sYear);
        if (lstData.Count() == 0)
        {
            lstData = new List<ClassExecute.TDataOutput>();
        }
        return lstData;
    }

    public static string GetValMonthMax(string sM12, string sM11, string sM10, string sM9, string sM8, string sM7, string sM6, string sM5, string sM4, string sM3, string sM2, string sM1, int ProductID)
    {
        string sTotalPerson = "";

        if (ProductID == 87)
        {
            #region 14
            for (int i = 12; i <= 12; i--)
            {
                if (i == 12)
                {
                    if (!string.IsNullOrEmpty(sM12))
                    {
                        sTotalPerson = sM12; break;
                    }
                }
                else if (i == 11)
                {
                    if (!string.IsNullOrEmpty(sM11))
                    {
                        sTotalPerson = sM11; break;
                    }
                }
                else if (i == 10)
                {
                    if (!string.IsNullOrEmpty(sM10)) { sTotalPerson = sM10; break; }
                }
                else if (i == 9)
                {
                    if (!string.IsNullOrEmpty(sM9)) { sTotalPerson = sM9; break; }
                }
                else if (i == 8)
                {
                    if (!string.IsNullOrEmpty(sM8)) { sTotalPerson = sM8; break; }
                }
                else if (i == 7)
                {
                    if (!string.IsNullOrEmpty(sM7)) { sTotalPerson = sM7; break; }
                }
                else if (i == 6)
                {
                    if (!string.IsNullOrEmpty(sM6)) { sTotalPerson = sM6; break; }
                }
                else if (i == 5)
                {
                    if (!string.IsNullOrEmpty(sM5)) { sTotalPerson = sM5; break; }
                }
                else if (i == 4)
                {
                    if (!string.IsNullOrEmpty(sM4)) { sTotalPerson = sM4; break; }
                }
                else if (i == 3)
                {
                    if (!string.IsNullOrEmpty(sM3)) { sTotalPerson = sM3; break; }
                }
                else if (i == 2)
                {
                    if (!string.IsNullOrEmpty(sM2)) { sTotalPerson = sM2; break; }
                }
                else if (i == 1)
                {
                    if (!string.IsNullOrEmpty(sM1)) { sTotalPerson = sM1; break; }
                }
            }
            #endregion
        }
        else
        {
            sTotalPerson = SystemFunction.ConvertFormatDecimal4(EPIFunc.SumDataToDecimal(sM1, sM2, sM3, sM4, sM5, sM6, sM7, sM8, sM9, sM10, sM11, sM12).ToString());
        }
        return sTotalPerson;
    }
}