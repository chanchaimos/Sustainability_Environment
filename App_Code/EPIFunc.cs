using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EPIFunc
/// </summary>
public class EPIFunc
{
    PTTGC_EPIEntities db = new PTTGC_EPIEntities();
    public EPIFunc()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public class DataType
    {
        public class Company
        {
            public const int PTTGCID = 13;
        }
    }

    //Fix Code
    public const int nUnitID_KG = 3;
    public const int nIndComplaintID = 1;
    public const int nIndComplianceID = 2;
    public const int nIndEffluentID = 3;
    public const int nIndEmissionID = 4;
    public const int nIndIntensityID = 6;
    public const int nIndMaterialID = 8;
    public const int nIndSpillID = 9;
    public const int nIndWasteID = 10;
    public const int nIndWaterID = 11;
    public const string ValueFormatNA = "N/A";

    public void RecalculateWaste(int nOperaID, int nFacID, string sYear)
    {
        var queryEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == 10 && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();// Data Waste
        if (queryEPIForm != null)
        {
            SysFunctionCalculate fc = new SysFunctionCalculate();
            AddProductOutput_Calculate(fc.CalculateWaste(nOperaID, nFacID, sYear), queryEPIForm.FormID);
        }
    }

    public void RecalculateWater(int nOperaID, int nFacID, string sYear)
    {
        var queryEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == 11 && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();// Data Water
        if (queryEPIForm != null)
        {
            SysFunctionCalculate fc = new SysFunctionCalculate();
            AddProductOutput_Calculate(fc.CalculateWater(nOperaID, nFacID, sYear), queryEPIForm.FormID);
        }
    }

    public void RecalculateMaterial(int nOperaID, int nFacID, string sYear)
    {
        var queryEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == 8 && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();// Data Material
        if (queryEPIForm != null)
        {
            SysFunctionCalculate fc = new SysFunctionCalculate();
            AddProductOutput_Calculate(fc.CalculateMaterial(nOperaID, nFacID, sYear), queryEPIForm.FormID);
        }
    }

    public void RecalculateEffluent(int nOperaID, int nFacID, string sYear)
    {
        var queryEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == 3 && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (queryEPIForm != null)
        {
            SysFunctionCalculate fc = new SysFunctionCalculate();
            AddProductOutput_Calculate(fc.CalculateEffluent(nOperaID, nFacID, sYear), queryEPIForm.FormID);
        }
    }

    public void RecalculateEmission(int nOperaID, int nFacID, string sYear)
    {
        var queryEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == 4 && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (queryEPIForm != null)
        {
            SysFunctionCalculate fc = new SysFunctionCalculate();
            AddProductOutput_Calculate(fc.CalculateEmission(nOperaID, nFacID, sYear), queryEPIForm.FormID);
        }
    }

    public void RecalculateSpill(int nOperaID, int nFacID, string sYear)
    {
        var queryEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == 9 && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (queryEPIForm != null)
        {
            SysFunctionCalculate fc = new SysFunctionCalculate();
            AddProductOutput_Calculate(fc.CalculateSpill(nOperaID, nFacID, sYear), queryEPIForm.FormID);
        }
    }

    public void RecalculateComplaint(int nOperaID, int nFacID, string sYear)
    {
        var queryEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == 1 && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (queryEPIForm != null)
        {
            SysFunctionCalculate fc = new SysFunctionCalculate();
            AddProductOutput_Calculate(fc.CalculateComplaint(nOperaID, nFacID, sYear), queryEPIForm.FormID);
        }
    }

    public void RecalculateCompliance(int nOperaID, int nFacID, string sYear)
    {
        var queryEPIForm = db.TEPI_Forms.Where(w => w.IDIndicator == 2 && w.sYear == sYear && w.OperationTypeID == nOperaID && w.FacilityID == nFacID).FirstOrDefault();
        if (queryEPIForm != null)
        {
            SysFunctionCalculate fc = new SysFunctionCalculate();
            AddProductOutput_Calculate(fc.CalculateCompliance(nOperaID, nFacID, sYear), queryEPIForm.FormID);
        }
    }

    private string ConvertToInsertOutput(decimal? nVal)
    {
        string sResult = "";
        if (nVal != null)
        {
            if ((nVal.Value + "").Length > 50)
            {
                string[] ArrVal = (nVal.Value + "").Split('.');
                if (ArrVal.Length > 1)
                {
                    int nCount1 = ArrVal[0].Length;
                    int nCount2 = ArrVal[1].Length;
                    if (nCount1 > 50)
                    {
                        sResult = nVal.Value.ToString("#,##0.###E+0");
                    }
                    else
                    {
                        int nCheck = 50 - nCount1;
                        sResult = nVal.Value.ToString("n" + nCheck);
                    }
                }
                else
                {
                    sResult = nVal.Value.ToString("#,##0.###E+0");
                }
            }
            else
            {
                sResult = nVal.Value + "";
            }
        }

        return sResult;
    }

    public bool IsProductSpecific(List<ClassExecute.TDataOutput> lstdata, int nIndID, int nProductID)//ในกรณีที่ไม่ใช้ Product แบบ Specific จะทำการอัพเดท Target ในกรณีที่มีการบันทึก Input ใหม่
    {
        bool cCheck = false;
        var query = lstdata.Where(w => w.ProductID == nProductID).FirstOrDefault();
        if (query != null)
        {
            cCheck = (query.sMakeField1 + "" == "Y");
        }

        return cCheck;
    }

    public static decimal? GetValueFromListOutput(List<ClassExecute.TDataOutput> lstData, int nColumn, int nProductID)
    {
        decimal? nResult = null;
        var query = lstData.Where(w => w.ProductID == nProductID).FirstOrDefault();
        if (query != null)
        {
            switch (nColumn)
            {
                case 0: nResult = query.nTarget; break;
                case 1: nResult = query.nM1; break;
                case 2: nResult = query.nM2; break;
                case 3: nResult = query.nM3; break;
                case 4: nResult = query.nM4; break;
                case 5: nResult = query.nM5; break;
                case 6: nResult = query.nM6; break;
                case 7: nResult = query.nM7; break;
                case 8: nResult = query.nM8; break;
                case 9: nResult = query.nM9; break;
                case 10: nResult = query.nM10; break;
                case 11: nResult = query.nM11; break;
                case 12: nResult = query.nM12; break;
                case 13: nResult = query.nTotal; break;
                case 14: nResult = query.nQ1; break;
                case 15: nResult = query.nQ2; break;
                case 16: nResult = query.nQ3; break;
                case 17: nResult = query.nQ4; break;
                case 18: nResult = query.nH1; break;
                case 19: nResult = query.nH2; break;
            }
        }
        return nResult;
    }

    public static string ConvertExponentialToString(string sVal)
    {
        string sRsult = "";
        try
        {
            decimal nTemp = 0;
            bool check = Decimal.TryParse(sVal, System.Globalization.NumberStyles.Float, null, out nTemp);
            if (check)
            {
                decimal d = Decimal.Parse(sVal, System.Globalization.NumberStyles.Float);
                sRsult = d + "";
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

    public static bool IsNumberic(string sVal)
    {
        decimal nTemp = 0;
        sVal = ConvertExponentialToString(sVal);
        return decimal.TryParse(sVal, out nTemp);
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
        else
        {
            sResult = sVal;
        }
        return sResult;
    }

    public static string ConvertToStatistic(string sVal, string sE)//Default 3
    {
        string sResult = "";
        if (IsNumberic(sVal))
        {
            string sValCheck = sVal.Replace("-", "");
            sValCheck = ConvertExponentialToString(sValCheck);
            sVal = ConvertExponentialToString(sVal);
            decimal nTemp = decimal.Parse(sVal);
            string sFormat = "";
            int nTemp2 = 0;
            if (int.TryParse(sE, out nTemp2))
            {
                nTemp2 = int.Parse(sE);
                for (int i = 0; i < nTemp2; i++)
                {
                    sFormat += "0";
                }
            }

            if (!string.IsNullOrEmpty(sFormat))
            {
                sResult = nTemp.ToString("0." + sFormat + "E+0");
            }
            else
            {
                sResult = nTemp.ToString("0.000E+0");
            }
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
        if (!string.IsNullOrEmpty(sVal) && sVal != ValueFormatNA)
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

    public static string ConvertFormatDecimal(string sVal, int nDigit)
    {
        string sResult = "";
        if (!string.IsNullOrEmpty(sVal) && sVal != ValueFormatNA)
        {
            sVal = ConvertExponentialToString(sVal);
            if (IsNumberic(sVal))
            {
                decimal nTemp = decimal.TryParse(sVal, out nTemp) ? nTemp : 0;
                sResult = nTemp.ToString("n" + nDigit);
            }
            else
            {
                sResult = sVal;
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

    /// <summary>
    /// แปลงข้อมูล Volume เป็นหน่วย barrel ตามสูตร  Barrel = Lite/158.9873
    /// แปลงหา Liter = Barrel * 158.9873
    /// </summary>
    /// <param name="sVal">Barrel</param>
    /// <returns></returns>
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

    /// <summary>
    /// liter = m3 * 1000
    /// </summary>
    /// <param name="sVal">M3</param>
    /// <returns></returns>
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

    /// <summary>
    /// M3 = Liter/1000
    /// </summary>
    /// <param name="sVal">Liter</param>
    /// <returns></returns>
    public static decimal? ConvertLiterToM3(string sVal)
    {
        decimal? nReturn = null;
        if (!string.IsNullOrEmpty(sVal) && IsNumberic(sVal))
        {
            decimal? nVal = GetDecimalNull(sVal);
            nReturn = nVal / 1000m;
        }
        return nReturn;
    }

    /// <summary>
    /// Barrel = Liter/158.9873
    /// </summary>
    /// <param name="sVal">Liter</param>
    /// <returns></returns>
    public static decimal? ConvertLiterToBarrel(string sVal)
    {
        decimal? nReturn = null;
        if (!string.IsNullOrEmpty(sVal) && IsNumberic(sVal))
        {
            decimal? nVal = GetDecimalNull(sVal);
            nReturn = nVal / 158.9873m;
        }
        return nReturn;
    }

    /// <summary>
    /// แปลงจาก M3 เป็น Liter แล้วแปลงจาก Liter เป็น Barrel
    /// </summary>
    /// <param name="sVal">M3</param>
    /// <returns></returns>
    public static decimal? ConvertM3ToBarrel(string sVal)
    {
        decimal? nReturn = null;
        if (!string.IsNullOrEmpty(sVal) && IsNumberic(sVal))
        {
            nReturn = ConvertLiterToBarrel(ConvertM3ToLiter(sVal) + "");
        }
        return nReturn;
    }

    /// <summary>
    /// แปลงจาก Barrel เป็น Liter แล้วแปลงจาก Liter เป็น M3
    /// </summary>
    /// <param name="sVal"></param>
    /// <returns></returns>
    public static decimal? ConvertBarrelToM3(string sVal)
    {
        decimal? nReturn = null;
        if (!string.IsNullOrEmpty(sVal) && IsNumberic(sVal))
        {
            decimal? nVal = GetDecimalNull(sVal);
            nReturn = ConvertLiterToM3((ConvertBarrelToLiter(sVal) + ""));
        }
        return nReturn;
    }

    public void AddProductOutput_Calculate(List<ClassExecute.TDataOutput> lstTemp, int nEPIFormID)
    {
        int IndicaotrID = 0;
        int nFacID = 0;
        int nOperaID = 0;
        string sYear = "0";

        var qEPIForm = db.TEPI_Forms.Where(w => w.FormID == nEPIFormID).FirstOrDefault();
        if (qEPIForm != null)
        {
            IndicaotrID = qEPIForm.IDIndicator;
            nFacID = qEPIForm.FacilityID ?? 0;
            nOperaID = qEPIForm.OperationTypeID;
            sYear = qEPIForm.sYear;
        }

        //Old Data
        List<ClassExecute.TDataOutput> lstOldDataOutput = new List<ClassExecute.TDataOutput>();
        lstOldDataOutput = FunctionGetData.GetDataOutput(IndicaotrID, nOperaID, nFacID, sYear);

        // Clear data
        string sql = "delete from TProductOutput where FormID = " + CommonFunction.ReplaceInjection(nEPIFormID + "") + "";
        CommonFunction.ExecuteSQL(SystemFunction.strConnect, sql);

        //Data for check product specific
        var query = db.mTProductIndicatorOutput.Where(w => w.IDIndicator == IndicaotrID && w.cDel == "N").Select(s => new ClassExecute.TDataOutput { ProductID = s.ProductID, sMakeField1 = s.sIsSpecific }).ToList();

        //add new data
        foreach (var item in lstTemp)
        {
            TProductOutput t = new TProductOutput();
            t.FormID = nEPIFormID;
            t.ProductID = item.ProductID;
            t.M1 = ConvertToInsertOutput(item.nM1);
            t.M2 = ConvertToInsertOutput(item.nM2);
            t.M3 = ConvertToInsertOutput(item.nM3);
            t.M4 = ConvertToInsertOutput(item.nM4);
            t.M5 = ConvertToInsertOutput(item.nM5);
            t.M6 = ConvertToInsertOutput(item.nM6);
            t.M7 = ConvertToInsertOutput(item.nM7);
            t.M8 = ConvertToInsertOutput(item.nM8);
            t.M9 = ConvertToInsertOutput(item.nM9);
            t.M10 = ConvertToInsertOutput(item.nM10);
            t.M11 = ConvertToInsertOutput(item.nM11);
            t.M12 = ConvertToInsertOutput(item.nM12);

            if (!IsProductSpecific(query, IndicaotrID, item.ProductID))
            {
                t.Target = ConvertToInsertOutput(item.nTarget);
            }
            else
            {
                t.Target = GetValueFromListOutput(lstOldDataOutput, 0, item.ProductID) + "";
            }

            t.nTotal = ConvertToInsertOutput(item.nTotal);
            t.Q1 = ConvertToInsertOutput(item.nQ1);
            t.Q2 = ConvertToInsertOutput(item.nQ2);
            t.Q3 = ConvertToInsertOutput(item.nQ3);
            t.Q4 = ConvertToInsertOutput(item.nQ4);
            t.H1 = ConvertToInsertOutput(item.nH1);
            t.H2 = ConvertToInsertOutput(item.nH2);
            db.TProductOutput.Add(t);
        }
        db.SaveChanges();
    }

    public static List<ClassExecute.TData_Intensity_DisplayInput> GetDataDisplayInputIntensity()
    {
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        List<ClassExecute.TData_Intensity_DisplayInput> lstTemp = new List<ClassExecute.TData_Intensity_DisplayInput>();
        lstTemp = db.TIntensity_DisplayInput.Select(s => new ClassExecute.TData_Intensity_DisplayInput { ID = s.ID, OperaID = s.OperationTypeID.Value, nDisplayType = s.DisplayType.Value, sUrl = s.sUrl }).ToList();
        return lstTemp;
    }

    public static decimal? SumDataToDecimal(decimal?[] ArrValue)
    {
        decimal? nTotal = null;
        bool cNullAll = true;
        decimal nSum = 0;

        for (int i = 0; i < ArrValue.Length; i++)
        {
            if (ArrValue[i] != null)
            {
                cNullAll = false;
                nSum = nSum + ArrValue[i].Value;
            }
        }

        if (!cNullAll)
        {
            nTotal = nSum;
        }

        return nTotal;
    }

    public static decimal? SumDataToDecimal(string[] ArrValue)
    {
        decimal? nTotal = null;
        bool cNullAll = true;
        decimal nSum = 0;

        for (int i = 0; i < ArrValue.Length; i++)
        {
            if (SystemFunction.IsNumberic(ArrValue[i]))
            {
                cNullAll = false;
                nSum = nSum + SystemFunction.GetNumberNullToZero(ArrValue[i]);
            }
        }

        if (!cNullAll)
        {
            nTotal = nSum;
        }

        return nTotal;
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

    public static decimal? SumDataToDecimal(string sM1, string sM2, string sM3, string sM4, string sM5, string sM6, string sM7, string sM8, string sM9, string sM10, string sM11, string sM12)
    {
        string[] ArrValue = { sM1, sM2, sM3, sM4, sM5, sM6, sM7, sM8, sM9, sM10, sM11, sM12 };
        decimal? nTotal = null;
        bool cNullAll = true;
        decimal nSum = 0;

        for (int i = 0; i < ArrValue.Length; i++)
        {
            if (SystemFunction.IsNumberic(ArrValue[i]))
            {
                cNullAll = false;
                nSum = nSum + SystemFunction.GetNumberNullToZero(ArrValue[i]);
            }
        }

        if (!cNullAll)
        {
            nTotal = nSum;
        }

        return nTotal;
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
}