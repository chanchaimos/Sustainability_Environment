using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SysFunctionCalculate
/// </summary>
public class SysFunctionCalculate
{
    #region //Property
    private string sGetValueMode;
    private const string sysYTD = "YTD";
    private const string sysH1 = "H1";
    private const string sysH2 = "H2";
    private const string sysQ1 = "Q1";
    private const string sysQ2 = "Q2";
    private const string sysQ3 = "Q3";
    private const string sysQ4 = "Q4";

    // Recalculate From Output
    public const string sysRC = "RC";
    private bool sysCQ1 = false;
    private bool sysCQ2 = false;
    private bool sysCQ3 = false;
    private bool sysCQ4 = false;

    //Factor Convert Unit
    private decimal FactorM32MBOE = 1;
    private decimal FactorM32TOE = 1;
    private decimal FactorM22MBOE = 1;
    private decimal FactorM22TOE = 1;
    private decimal FactorMGL2TON = 1;
    private decimal FactorMGL2MBOE = 1;
    private decimal FactorMGL2TOE = 1;
    private decimal FactorTON2MBOE = 1;
    private decimal FactorTON2TOE = 1;
    private decimal FactorM32TonnesThroughput = 1;
    private decimal FactorTON2TonnesThroughput = 1;
    private decimal FactorM32mmscf = 1;
    private decimal FactorM32mmbtu = 1;
    private decimal FactorTON2mmscf = 1;
    private decimal FactorTON2mmbtu = 1;
    private decimal FactorM32LitresofLubricantsold = 1;
    private decimal FactorTON2LitresofLubricantsold = 1;
    private decimal FactorM32TonnesProduct = 1;
    private decimal FactorTON2TonnesProduct = 1;
    private decimal FactorM32M2 = 1;
    private decimal FactorTON2M2 = 1;
    private decimal FactorM32RefiningTonnesThroughput = 1;
    private decimal FactorTON2RefiningTonnesThroughput = 1;
    private decimal FactorM32MWh = 1;
    private decimal FactorTON2MWh = 1;

    public string ModeValue
    {
        get { return this.sGetValueMode; }
        set { this.sGetValueMode = value; }
    }
    public bool CheckRecalQ1
    {
        get { return this.sysCQ1; }
        set { this.sysCQ1 = value; }
    }
    public bool CheckRecalQ2
    {
        get { return this.sysCQ2; }
        set { this.sysCQ2 = value; }
    }
    public bool CheckRecalQ3
    {
        get { return this.sysCQ3; }
        set { this.sysCQ3 = value; }
    }
    public bool CheckRecalQ4
    {
        get { return this.sysCQ4; }
        set { this.sysCQ4 = value; }
    }

    /// <summary>
    /// UnitID of kilogram
    /// </summary>
    public const int UnitIDKG = 3;
    /// <summary>
    /// Factor convert unit kilogram to tonnes
    /// </summary>
    public const decimal FactorKG = 0.001m;

    /// <summary>
    /// Non-hazardous Waste Disposed - Routine Non-hazardous Waste
    /// </summary>
    public const int nProductIDInputNHZDRoutine = 17;
    /// <summary>
    /// Domestic/ municipal waste disposed - Routine
    /// </summary>
    public const int nProductIDInputDomRoutine = 23;
    /// <summary>
    /// Non-hazardous Waste Disposed - Non-routine Non-hazardous Waste
    /// </summary>
    public const int nProductIDInputNHZDNonRoutine = 24;
    /// <summary>
    /// Domestic/ municipal waste disposed - Non-routine
    /// </summary>
    public const int nProductIDInputDomNonRoutine = 30;

    #endregion

    PTTGC_EPIEntities db = new PTTGC_EPIEntities();

    public SysFunctionCalculate()
    {
        ModeValue = "";
    }

    public static string InputSumToTotalF1(List<ClassExecute.TData_TIntensity_Other> lstTemp, int nMonth)
    {
        bool cNullAll = true;
        decimal nSum = 0;
        var query = lstTemp.Where(w => w.cTotal == "N").ToList();
        foreach (var item in query)
        {
            switch (nMonth)
            {
                case 0:
                    if (SystemFunction.IsNumberic(item.Target) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (SystemFunction.GetNumberNullToZero(item.Target) * item.Factor.Value);
                        cNullAll = false;
                    }
                    break;
                case 1:
                    if (SystemFunction.IsNumberic(item.M1) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (SystemFunction.GetNumberNullToZero(item.M1) * item.Factor.Value);
                        cNullAll = false;
                    }
                    break;
                case 2:
                    if (SystemFunction.IsNumberic(item.M2) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (SystemFunction.GetNumberNullToZero(item.M2) * item.Factor.Value);
                        cNullAll = false;
                    }
                    break;
                case 3:
                    if (SystemFunction.IsNumberic(item.M3) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (SystemFunction.GetNumberNullToZero(item.M3) * item.Factor.Value);
                        cNullAll = false;
                    }
                    break;
                case 4:
                    if (SystemFunction.IsNumberic(item.M4) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (SystemFunction.GetNumberNullToZero(item.M4) * item.Factor.Value);
                        cNullAll = false;
                    }
                    break;
                case 5:
                    if (SystemFunction.IsNumberic(item.M5) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (SystemFunction.GetNumberNullToZero(item.M5) * item.Factor.Value);
                        cNullAll = false;
                    }
                    break;
                case 6:
                    if (SystemFunction.IsNumberic(item.M6) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (SystemFunction.GetNumberNullToZero(item.M6) * item.Factor.Value);
                        cNullAll = false;
                    }
                    break;
                case 7:
                    if (SystemFunction.IsNumberic(item.M7) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (SystemFunction.GetNumberNullToZero(item.M7) * item.Factor.Value);
                        cNullAll = false;
                    }
                    break;
                case 8:
                    if (SystemFunction.IsNumberic(item.M8) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (SystemFunction.GetNumberNullToZero(item.M8) * item.Factor.Value);
                        cNullAll = false;
                    }
                    break;
                case 9:
                    if (SystemFunction.IsNumberic(item.M9) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (SystemFunction.GetNumberNullToZero(item.M9) * item.Factor.Value);
                        cNullAll = false;
                    }
                    break;
                case 10:
                    if (SystemFunction.IsNumberic(item.M10) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (SystemFunction.GetNumberNullToZero(item.M10) * item.Factor.Value);
                        cNullAll = false;
                    }
                    break;
                case 11:
                    if (SystemFunction.IsNumberic(item.M11) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (SystemFunction.GetNumberNullToZero(item.M11) * item.Factor.Value);
                        cNullAll = false;
                    }
                    break;
                case 12:
                    if (SystemFunction.IsNumberic(item.M12) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (SystemFunction.GetNumberNullToZero(item.M12) * item.Factor.Value);
                        cNullAll = false;
                    }
                    break;
            }

        }


        return cNullAll == false ? nSum + "" : "";
    }

    public static string InputSumToTotalF2(List<ClassExecute.TData_TIntensity_Other> lstTemp, int nMonth, decimal FactorDiv)
    {
        bool cNullAll = true;
        decimal nSum = 0;
        var query = lstTemp.Where(w => w.cTotal == "N").ToList();
        foreach (var item in query)
        {
            switch (nMonth)
            {
                case 0:
                    if (SystemFunction.IsNumberic(item.Target) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + ((SystemFunction.GetNumberNullToZero(item.Target) * item.Factor.Value) / FactorDiv);
                        cNullAll = false;
                    }
                    break;
                case 1:
                    if (SystemFunction.IsNumberic(item.M1) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + ((SystemFunction.GetNumberNullToZero(item.M1) * item.Factor.Value) / FactorDiv);
                        cNullAll = false;
                    }
                    break;
                case 2:
                    if (SystemFunction.IsNumberic(item.M2) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + ((SystemFunction.GetNumberNullToZero(item.M2) * item.Factor.Value) / FactorDiv);
                        cNullAll = false;
                    }
                    break;
                case 3:
                    if (SystemFunction.IsNumberic(item.M3) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + ((SystemFunction.GetNumberNullToZero(item.M3) * item.Factor.Value) / FactorDiv);
                        cNullAll = false;
                    }
                    break;
                case 4:
                    if (SystemFunction.IsNumberic(item.M4) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + ((SystemFunction.GetNumberNullToZero(item.M4) * item.Factor.Value) / FactorDiv);
                        cNullAll = false;
                    }
                    break;
                case 5:
                    if (SystemFunction.IsNumberic(item.M5) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + ((SystemFunction.GetNumberNullToZero(item.M5) * item.Factor.Value) / FactorDiv);
                        cNullAll = false;
                    }
                    break;
                case 6:
                    if (SystemFunction.IsNumberic(item.M6) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + ((SystemFunction.GetNumberNullToZero(item.M6) * item.Factor.Value) / FactorDiv);
                        cNullAll = false;
                    }
                    break;
                case 7:
                    if (SystemFunction.IsNumberic(item.M7) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + ((SystemFunction.GetNumberNullToZero(item.M7) * item.Factor.Value) / FactorDiv);
                        cNullAll = false;
                    }
                    break;
                case 8:
                    if (SystemFunction.IsNumberic(item.M8) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + ((SystemFunction.GetNumberNullToZero(item.M8) * item.Factor.Value) / FactorDiv);
                        cNullAll = false;
                    }
                    break;
                case 9:
                    if (SystemFunction.IsNumberic(item.M9) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + ((SystemFunction.GetNumberNullToZero(item.M9) * item.Factor.Value) / FactorDiv);
                        cNullAll = false;
                    }
                    break;
                case 10:
                    if (SystemFunction.IsNumberic(item.M10) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + ((SystemFunction.GetNumberNullToZero(item.M10) * item.Factor.Value) / FactorDiv);
                        cNullAll = false;
                    }
                    break;
                case 11:
                    if (SystemFunction.IsNumberic(item.M11) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + ((SystemFunction.GetNumberNullToZero(item.M11) * item.Factor.Value) / FactorDiv);
                        cNullAll = false;
                    }
                    break;
                case 12:
                    if (SystemFunction.IsNumberic(item.M12) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + ((SystemFunction.GetNumberNullToZero(item.M12) * item.Factor.Value) / FactorDiv);
                        cNullAll = false;
                    }
                    break;
            }

        }


        return cNullAll == false ? nSum + "" : "";
    }

    /// <summary>
    /// ใช้ในกรณีที่หน่วยเป็น liter,Kilogram(Kg) ต้องหารด้วย 1000 ก่อนคูณ Default Factor
    /// </summary>
    /// <param name="lstTemp"></param>
    /// <param name="nMonth"></param>
    /// <param name="FactorDiv"></param>
    /// <returns></returns>
    public static string InputSumToTotalF3(List<ClassExecute.TData_TIntensity_Other> lstTemp, int nMonth, decimal FactorDiv)
    {
        decimal nDivLiter = 1000;//Liter,Kg
        int nUnitLtiterID = 21;
        int nUnitKGID = EPIFunc.nUnitID_KG;
        bool cNullAll = true;
        decimal nSum = 0;
        var query = lstTemp.Where(w => w.cTotal == "N").ToList();
        foreach (var item in query)
        {
            switch (nMonth)
            {
                case 0:
                    if (SystemFunction.IsNumberic(item.Target) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (item.UnitID == nUnitLtiterID || item.UnitID == nUnitKGID ? (((SystemFunction.GetNumberNullToZero(item.Target) / nDivLiter) * item.Factor.Value) / FactorDiv) : ((SystemFunction.GetNumberNullToZero(item.Target) * item.Factor.Value) / FactorDiv));
                        cNullAll = false;
                    }
                    break;
                case 1:
                    if (SystemFunction.IsNumberic(item.M1) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (item.UnitID == nUnitLtiterID || item.UnitID == nUnitKGID ? (((SystemFunction.GetNumberNullToZero(item.M1) / nDivLiter) * item.Factor.Value) / FactorDiv) : ((SystemFunction.GetNumberNullToZero(item.M1) * item.Factor.Value) / FactorDiv));
                        cNullAll = false;
                    }
                    break;
                case 2:
                    if (SystemFunction.IsNumberic(item.M2) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (item.UnitID == nUnitLtiterID || item.UnitID == nUnitKGID ? (((SystemFunction.GetNumberNullToZero(item.M2) / nDivLiter) * item.Factor.Value) / FactorDiv) : ((SystemFunction.GetNumberNullToZero(item.M2) * item.Factor.Value) / FactorDiv));
                        cNullAll = false;
                    }
                    break;
                case 3:
                    if (SystemFunction.IsNumberic(item.M3) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (item.UnitID == nUnitLtiterID || item.UnitID == nUnitKGID ? (((SystemFunction.GetNumberNullToZero(item.M3) / nDivLiter) * item.Factor.Value) / FactorDiv) : ((SystemFunction.GetNumberNullToZero(item.M3) * item.Factor.Value) / FactorDiv));
                        cNullAll = false;
                    }
                    break;
                case 4:
                    if (SystemFunction.IsNumberic(item.M4) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (item.UnitID == nUnitLtiterID || item.UnitID == nUnitKGID ? (((SystemFunction.GetNumberNullToZero(item.M4) / nDivLiter) * item.Factor.Value) / FactorDiv) : ((SystemFunction.GetNumberNullToZero(item.M4) * item.Factor.Value) / FactorDiv));
                        cNullAll = false;
                    }
                    break;
                case 5:
                    if (SystemFunction.IsNumberic(item.M5) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (item.UnitID == nUnitLtiterID || item.UnitID == nUnitKGID ? (((SystemFunction.GetNumberNullToZero(item.M5) / nDivLiter) * item.Factor.Value) / FactorDiv) : ((SystemFunction.GetNumberNullToZero(item.M5) * item.Factor.Value) / FactorDiv));
                        cNullAll = false;
                    }
                    break;
                case 6:
                    if (SystemFunction.IsNumberic(item.M6) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (item.UnitID == nUnitLtiterID || item.UnitID == nUnitKGID ? (((SystemFunction.GetNumberNullToZero(item.M6) / nDivLiter) * item.Factor.Value) / FactorDiv) : ((SystemFunction.GetNumberNullToZero(item.M6) * item.Factor.Value) / FactorDiv));
                        cNullAll = false;
                    }
                    break;
                case 7:
                    if (SystemFunction.IsNumberic(item.M7) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (item.UnitID == nUnitLtiterID || item.UnitID == nUnitKGID ? (((SystemFunction.GetNumberNullToZero(item.M7) / nDivLiter) * item.Factor.Value) / FactorDiv) : ((SystemFunction.GetNumberNullToZero(item.M7) * item.Factor.Value) / FactorDiv));
                        cNullAll = false;
                    }
                    break;
                case 8:
                    if (SystemFunction.IsNumberic(item.M8) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (item.UnitID == nUnitLtiterID || item.UnitID == nUnitKGID ? (((SystemFunction.GetNumberNullToZero(item.M8) / nDivLiter) * item.Factor.Value) / FactorDiv) : ((SystemFunction.GetNumberNullToZero(item.M8) * item.Factor.Value) / FactorDiv));
                        cNullAll = false;
                    }
                    break;
                case 9:
                    if (SystemFunction.IsNumberic(item.M9) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (item.UnitID == nUnitLtiterID || item.UnitID == nUnitKGID ? (((SystemFunction.GetNumberNullToZero(item.M9) / nDivLiter) * item.Factor.Value) / FactorDiv) : ((SystemFunction.GetNumberNullToZero(item.M9) * item.Factor.Value) / FactorDiv));
                        cNullAll = false;
                    }
                    break;
                case 10:
                    if (SystemFunction.IsNumberic(item.M10) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (item.UnitID == nUnitLtiterID || item.UnitID == nUnitKGID ? (((SystemFunction.GetNumberNullToZero(item.M10) / nDivLiter) * item.Factor.Value) / FactorDiv) : ((SystemFunction.GetNumberNullToZero(item.M10) * item.Factor.Value) / FactorDiv));
                        cNullAll = false;
                    }
                    break;
                case 11:
                    if (SystemFunction.IsNumberic(item.M11) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (item.UnitID == nUnitLtiterID || item.UnitID == nUnitKGID ? (((SystemFunction.GetNumberNullToZero(item.M11) / nDivLiter) * item.Factor.Value) / FactorDiv) : ((SystemFunction.GetNumberNullToZero(item.M11) * item.Factor.Value) / FactorDiv));
                        cNullAll = false;
                    }
                    break;
                case 12:
                    if (SystemFunction.IsNumberic(item.M12) && SystemFunction.IsNumberic(item.Factor + ""))
                    {
                        nSum = nSum + (item.UnitID == nUnitLtiterID || item.UnitID == nUnitKGID ? (((SystemFunction.GetNumberNullToZero(item.M12) / nDivLiter) * item.Factor.Value) / FactorDiv) : ((SystemFunction.GetNumberNullToZero(item.M12) * item.Factor.Value) / FactorDiv));
                        cNullAll = false;
                    }
                    break;
            }

        }


        return cNullAll == false ? nSum + "" : "";
    }

    public static string SumDecimalToStringF1(string sVal1, string sVal2)
    {
        decimal nSum = 0;
        bool cNullAll = true;
        sVal1 = SystemFunction.ConvertExponentialToString(sVal1);
        sVal2 = SystemFunction.ConvertExponentialToString(sVal2);
        if (SystemFunction.IsNumberic(sVal1))
        {
            nSum = nSum + decimal.Parse(sVal1);
            cNullAll = false;
        }

        if (SystemFunction.IsNumberic(sVal2))
        {
            nSum = nSum + decimal.Parse(sVal2);
            cNullAll = false;
        }

        return cNullAll == false ? nSum + "" : "";
    }

    public List<ClassExecute.TDataOutput> CalculateWaste(int nOperaID, int nFacID, string sYear) //Edit 10.12.2557
    {
        int IDIndicator = 10;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();

        var qIntensity = FunctionGetData.GetDataIntensityToCalculateOutput(nOperaID, nFacID, sYear);
        var qWaste = FunctionGetData.GetDataWasteToCalculateOutput(nOperaID, nFacID, sYear);

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;
        int nModeFunction = 0;
        nModeFunction = GetFunctionCalWaste(nOperaID);
        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            if (nModeFunction == 1 || nModeFunction == 7)//nOperaID == 16 || nOperaID == 1 || nOperaID == 2 || nOperaID == 7 || nOperaID == 18 || nOperaID == 17 || nOperaID == 15 || nOperaID == 5 || nOperaID == 6 || nOperaID == 9 || nOperaID == 10) //D1
            {
                #region call funtion calculate
                Target = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 0, "");
                M1 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 1, "");
                M2 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 2, "");
                M3 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 3, "");
                M4 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 4, "");
                M5 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 5, "");
                M6 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 6, "");
                M7 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 7, "");
                M8 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 8, "");
                M9 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 9, "");
                M10 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 10, "");
                M11 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 11, "");
                M12 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 12, "");
                /*decimal?[] arrDataYTD = { M1, M2, M3, M4, M5, M6, M7, M8, M9, M10, M11, M12 };
                Total = EPIFunc.SumDataToDecimal(arrDataYTD);

                decimal?[] arrDataQ1 = { M1, M2, M3 };
                decimal?[] arrDataQ2 = { M4, M5, M6 };
                decimal?[] arrDataQ3 = { M7, M8, M9 };
                decimal?[] arrDataQ4 = { M10, M11, M12 };
                Q1 = EPIFunc.SumDataToDecimal(arrDataQ1);
                Q2 = EPIFunc.SumDataToDecimal(arrDataQ2);
                Q3 = EPIFunc.SumDataToDecimal(arrDataQ3);
                Q4 = EPIFunc.SumDataToDecimal(arrDataQ4);

                decimal?[] arrDataH1 = { Q1, Q2 };
                decimal?[] arrDataH2 = { Q3, Q4 };
                H1 = EPIFunc.SumDataToDecimal(arrDataH1);
                H2 = EPIFunc.SumDataToDecimal(arrDataH2);*/

                Total = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysYTD);
                Q1 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysQ1);
                Q2 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysQ2);
                Q3 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysQ3);
                Q4 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysQ4);
                H1 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysH1);
                H2 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysH2);

                #endregion
            }
            else if (nModeFunction == 2)//nOperaID == 12) //D2
            {
                #region call funtion calculate
                Target = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 0, "");
                M1 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 1, "");
                M2 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 2, "");
                M3 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 3, "");
                M4 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 4, "");
                M5 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 5, "");
                M6 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 6, "");
                M7 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 7, "");
                M8 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 8, "");
                M9 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 9, "");
                M10 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 10, "");
                M11 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 11, "");
                M12 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 12, "");
                /*decimal?[] arrDataYTD = { M1, M2, M3, M4, M5, M6, M7, M8, M9, M10, M11, M12 };
                Total = EPIFunc.SumDataToDecimal(arrDataYTD);

                decimal?[] arrDataQ1 = { M1, M2, M3 };
                decimal?[] arrDataQ2 = { M4, M5, M6 };
                decimal?[] arrDataQ3 = { M7, M8, M9 };
                decimal?[] arrDataQ4 = { M10, M11, M12 };
                Q1 = EPIFunc.SumDataToDecimal(arrDataQ1);
                Q2 = EPIFunc.SumDataToDecimal(arrDataQ2);
                Q3 = EPIFunc.SumDataToDecimal(arrDataQ3);
                Q4 = EPIFunc.SumDataToDecimal(arrDataQ4);

                decimal?[] arrDataH1 = { Q1, Q2 };
                decimal?[] arrDataH2 = { Q3, Q4 };
                H1 = EPIFunc.SumDataToDecimal(arrDataH1);
                H2 = EPIFunc.SumDataToDecimal(arrDataH2);*/

                Total = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysYTD);
                Q1 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysQ1);
                Q2 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysQ2);
                Q3 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysQ3);
                Q4 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysQ4);
                H1 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysH1);
                H2 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysH2);
                #endregion
            }
            else if (nModeFunction == 3)//nOperaID == 13 || nOperaID == 3 || nOperaID == 8) //D3
            {
                #region call funtion calculate
                Target = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 0, "");
                M1 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 1, "");
                M2 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 2, "");
                M3 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 3, "");
                M4 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 4, "");
                M5 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 5, "");
                M6 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 6, "");
                M7 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 7, "");
                M8 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 8, "");
                M9 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 9, "");
                M10 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 10, "");
                M11 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 11, "");
                M12 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 12, "");
                /*decimal?[] arrDataYTD = { M1, M2, M3, M4, M5, M6, M7, M8, M9, M10, M11, M12 };
                Total = EPIFunc.SumDataToDecimal(arrDataYTD);

                decimal?[] arrDataQ1 = { M1, M2, M3 };
                decimal?[] arrDataQ2 = { M4, M5, M6 };
                decimal?[] arrDataQ3 = { M7, M8, M9 };
                decimal?[] arrDataQ4 = { M10, M11, M12 };
                Q1 = EPIFunc.SumDataToDecimal(arrDataQ1);
                Q2 = EPIFunc.SumDataToDecimal(arrDataQ2);
                Q3 = EPIFunc.SumDataToDecimal(arrDataQ3);
                Q4 = EPIFunc.SumDataToDecimal(arrDataQ4);

                decimal?[] arrDataH1 = { Q1, Q2 };
                decimal?[] arrDataH2 = { Q3, Q4 };
                H1 = EPIFunc.SumDataToDecimal(arrDataH1);
                H2 = EPIFunc.SumDataToDecimal(arrDataH2);
                */

                Total = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysYTD);
                Q1 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysQ1);
                Q2 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysQ2);
                Q3 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysQ3);
                Q4 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysQ4);
                H1 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysH1);
                H2 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysH2);
                #endregion
            }
            else if (nModeFunction == 4)//nOperaID == 11) //D4
            {
                #region call funtion calculate
                Target = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 0, "");
                M1 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 1, "");
                M2 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 2, "");
                M3 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 3, "");
                M4 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 4, "");
                M5 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 5, "");
                M6 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 6, "");
                M7 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 7, "");
                M8 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 8, "");
                M9 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 9, "");
                M10 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 10, "");
                M11 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 11, "");
                M12 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 12, "");
                /*decimal?[] arrDataYTD = { M1, M2, M3, M4, M5, M6, M7, M8, M9, M10, M11, M12 };
                Total = EPIFunc.SumDataToDecimal(arrDataYTD);

                decimal?[] arrDataQ1 = { M1, M2, M3 };
                decimal?[] arrDataQ2 = { M4, M5, M6 };
                decimal?[] arrDataQ3 = { M7, M8, M9 };
                decimal?[] arrDataQ4 = { M10, M11, M12 };
                Q1 = EPIFunc.SumDataToDecimal(arrDataQ1);
                Q2 = EPIFunc.SumDataToDecimal(arrDataQ2);
                Q3 = EPIFunc.SumDataToDecimal(arrDataQ3);
                Q4 = EPIFunc.SumDataToDecimal(arrDataQ4);

                decimal?[] arrDataH1 = { Q1, Q2 };
                decimal?[] arrDataH2 = { Q3, Q4 };
                H1 = EPIFunc.SumDataToDecimal(arrDataH1);
                H2 = EPIFunc.SumDataToDecimal(arrDataH2);
                */

                Total = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysYTD);
                Q1 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysQ1);
                Q2 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysQ2);
                Q3 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysQ3);
                Q4 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysQ4);
                H1 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysH1);
                H2 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysH2);
                #endregion
            }
            else if (nModeFunction == 5)//nOperaID == 14) //D5
            {
                #region call funtion calculate
                Target = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 0, "");
                M1 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 1, "");
                M2 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 2, "");
                M3 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 3, "");
                M4 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 4, "");
                M5 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 5, "");
                M6 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 6, "");
                M7 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 7, "");
                M8 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 8, "");
                M9 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 9, "");
                M10 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 10, "");
                M11 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 11, "");
                M12 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 12, "");
                /*decimal?[] arrDataYTD = { M1, M2, M3, M4, M5, M6, M7, M8, M9, M10, M11, M12 };
                Total = EPIFunc.SumDataToDecimal(arrDataYTD);

                decimal?[] arrDataQ1 = { M1, M2, M3 };
                decimal?[] arrDataQ2 = { M4, M5, M6 };
                decimal?[] arrDataQ3 = { M7, M8, M9 };
                decimal?[] arrDataQ4 = { M10, M11, M12 };
                Q1 = EPIFunc.SumDataToDecimal(arrDataQ1);
                Q2 = EPIFunc.SumDataToDecimal(arrDataQ2);
                Q3 = EPIFunc.SumDataToDecimal(arrDataQ3);
                Q4 = EPIFunc.SumDataToDecimal(arrDataQ4);

                decimal?[] arrDataH1 = { Q1, Q2 };
                decimal?[] arrDataH2 = { Q3, Q4 };
                H1 = EPIFunc.SumDataToDecimal(arrDataH1);
                H2 = EPIFunc.SumDataToDecimal(arrDataH2);
                */

                Total = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysYTD);
                Q1 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysQ1);
                Q2 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysQ2);
                Q3 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysQ3);
                Q4 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysQ4);
                H1 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysH1);
                H2 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysH2);
                #endregion
            }
            else if (nModeFunction == 6)//nOperaID == 4) //D6
            {
                #region call funtion calculate
                Target = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 0, "");
                M1 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 1, "");
                M2 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 2, "");
                M3 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 3, "");
                M4 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 4, "");
                M5 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 5, "");
                M6 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 6, "");
                M7 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 7, "");
                M8 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 8, "");
                M9 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 9, "");
                M10 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 10, "");
                M11 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 11, "");
                M12 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 12, "");
                /*decimal?[] arrDataYTD = { M1, M2, M3, M4, M5, M6, M7, M8, M9, M10, M11, M12 };
                Total = EPIFunc.SumDataToDecimal(arrDataYTD);

                decimal?[] arrDataQ1 = { M1, M2, M3 };
                decimal?[] arrDataQ2 = { M4, M5, M6 };
                decimal?[] arrDataQ3 = { M7, M8, M9 };
                decimal?[] arrDataQ4 = { M10, M11, M12 };
                Q1 = EPIFunc.SumDataToDecimal(arrDataQ1);
                Q2 = EPIFunc.SumDataToDecimal(arrDataQ2);
                Q3 = EPIFunc.SumDataToDecimal(arrDataQ3);
                Q4 = EPIFunc.SumDataToDecimal(arrDataQ4);

                decimal?[] arrDataH1 = { Q1, Q2 };
                decimal?[] arrDataH2 = { Q3, Q4 };
                H1 = EPIFunc.SumDataToDecimal(arrDataH1);
                H2 = EPIFunc.SumDataToDecimal(arrDataH2);
                */

                Total = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysYTD);
                Q1 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysQ1);
                Q2 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysQ2);
                Q3 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysQ3);
                Q4 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysQ4);
                H1 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysH1);
                H2 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysH2);
                #endregion
            }

            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
                {
                    IDIndicator = IDIndicator,
                    OperationtypeID = nOperaID,
                    FacilityID = nFacID,
                    ProductID = item.ProductID,
                    ProductName = item.ProductName,
                    nUnitID = item.UnitID,
                    sUnit = item.sUnit,
                    nOrder = item.nOrder,

                    /*
                    sM1 = SystemFunction.ConvertFormatDecimal3(M1 + ""),
                    sM2 = SystemFunction.ConvertFormatDecimal3(M2 + ""),
                    sM3 = SystemFunction.ConvertFormatDecimal3(M3 + ""),
                    sM4 = SystemFunction.ConvertFormatDecimal3(M4 + ""),
                    sM5 = SystemFunction.ConvertFormatDecimal3(M5 + ""),
                    sM6 = SystemFunction.ConvertFormatDecimal3(M6 + ""),
                    sM7 = SystemFunction.ConvertFormatDecimal3(M7 + ""),
                    sM8 = SystemFunction.ConvertFormatDecimal3(M8 + ""),
                    sM9 = SystemFunction.ConvertFormatDecimal3(M9 + ""),
                    sM10 = SystemFunction.ConvertFormatDecimal3(M10 + ""),
                    sM11 = SystemFunction.ConvertFormatDecimal3(M11 + ""),
                    sM12 = SystemFunction.ConvertFormatDecimal3(M12 + ""),
                    sTarget = SystemFunction.ConvertFormatDecimal3(Target + ""),
                    sTotal = SystemFunction.ConvertFormatDecimal3(Total + ""),
                    */
                    nM1 = M1,
                    nM2 = M2,
                    nM3 = M3,
                    nM4 = M4,
                    nM5 = M5,
                    nM6 = M6,
                    nM7 = M7,
                    nM8 = M8,
                    nM9 = M9,
                    nM10 = M10,
                    nM11 = M11,
                    nM12 = M12,
                    nTarget = Target,
                    nTotal = Total,

                    nQ1 = Q1,
                    nQ2 = Q2,
                    nQ3 = Q3,
                    nQ4 = Q4,
                    nH1 = H1,
                    nH2 = H2

                    /*
                    sQ1 = SystemFunction.ConvertFormatDecimal3(Q1 + ""),
                    sQ2 = SystemFunction.ConvertFormatDecimal3(Q2 + ""),
                    sQ3 = SystemFunction.ConvertFormatDecimal3(Q3 + ""),
                    sQ4 = SystemFunction.ConvertFormatDecimal3(Q4 + ""),
                    sH1 = SystemFunction.ConvertFormatDecimal3(H1 + ""),
                    sH2 = SystemFunction.ConvertFormatDecimal3(H2 + "")
                    */
                });
            #endregion
        }

        return lstTempReturn;

    }

    public List<ClassExecute.TDataOutput> CalculateWater(int nOperaID, int nFacID, string sYear) //Edit 10.12.2557
    {
        int IDIndicator = 11;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();

        var qIntensity = FunctionGetData.GetDataIntensityToCalculateOutput(nOperaID, nFacID, sYear);
        var qWater = FunctionGetData.GetDataWaterToCalculateOutput(nOperaID, nFacID, sYear);

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            #region call funtion calculate
            Target = CalculateWater(item.ProductID, qWater, qIntensity, 0, "");
            M1 = CalculateWater(item.ProductID, qWater, qIntensity, 1, "");
            M2 = CalculateWater(item.ProductID, qWater, qIntensity, 2, "");
            M3 = CalculateWater(item.ProductID, qWater, qIntensity, 3, "");
            M4 = CalculateWater(item.ProductID, qWater, qIntensity, 4, "");
            M5 = CalculateWater(item.ProductID, qWater, qIntensity, 5, "");
            M6 = CalculateWater(item.ProductID, qWater, qIntensity, 6, "");
            M7 = CalculateWater(item.ProductID, qWater, qIntensity, 7, "");
            M8 = CalculateWater(item.ProductID, qWater, qIntensity, 8, "");
            M9 = CalculateWater(item.ProductID, qWater, qIntensity, 9, "");
            M10 = CalculateWater(item.ProductID, qWater, qIntensity, 10, "");
            M11 = CalculateWater(item.ProductID, qWater, qIntensity, 11, "");
            M12 = CalculateWater(item.ProductID, qWater, qIntensity, 12, "");
            //decimal?[] arrDataYTD = { M1, M2, M3, M4, M5, M6, M7, M8, M9, M10, M11, M12 };
            Total = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysYTD); //EPIFunc.SumDataToDecimal(arrDataYTD);

            /* decimal?[] arrDataQ1 = { M1, M2, M3 };
             decimal?[] arrDataQ2 = { M4, M5, M6 };
             decimal?[] arrDataQ3 = { M7, M8, M9 };
             decimal?[] arrDataQ4 = { M10, M11, M12 };
             */
            Q1 = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysQ1);
            Q2 = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysQ2);
            Q3 = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysQ3);
            Q4 = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysQ4);

            /*
            decimal?[] arrDataH1 = { Q1, Q2 };
            decimal?[] arrDataH2 = { Q3, Q4 };
            */
            H1 = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysH1);
            H2 = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysH2);
            #endregion


            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }

        return lstTempReturn;

    }

    public List<ClassExecute.TDataOutput> CalculateMaterial(int nOperaID, int nFacID, string sYear) //Edit 10.12.2557
    {
        int IDIndicator = 8;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();

        var qIntensity = FunctionGetData.GetDataIntensityToCalculateOutput(nOperaID, nFacID, sYear);
        var qMaterial = FunctionGetData.GetDataMaterialToCalculateOutput(nOperaID, nFacID, sYear);

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            #region call funtion calculate
            Target = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 0, "");
            M1 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 1, "");
            M2 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 2, "");
            M3 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 3, "");
            M4 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 4, "");
            M5 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 5, "");
            M6 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 6, "");
            M7 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 7, "");
            M8 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 8, "");
            M9 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 9, "");
            M10 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 10, "");
            M11 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 11, "");
            M12 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 12, "");
            //decimal?[] arrDataYTD = { M1, M2, M3, M4, M5, M6, M7, M8, M9, M10, M11, M12 };
            Total = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysYTD);//EPIFunc.SumDataToDecimal(arrDataYTD);

            /*decimal?[] arrDataQ1 = { M1, M2, M3 };
            decimal?[] arrDataQ2 = { M4, M5, M6 };
            decimal?[] arrDataQ3 = { M7, M8, M9 };
            decimal?[] arrDataQ4 = { M10, M11, M12 };
            */
            Q1 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysQ1);
            Q2 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysQ2);
            Q3 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysQ3);
            Q4 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysQ4);

            //decimal?[] arrDataH1 = { Q1, Q2 };
            //decimal?[] arrDataH2 = { Q3, Q4 };
            H1 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysH1);
            H2 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysH2);
            #endregion


            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }

        return lstTempReturn;

    }

    public List<ClassExecute.TDataOutput> CalculateEffluent(int nOperaID, int nFacID, string sYear)
    {
        int IDIndicator = 3;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();

        var qIntensity = FunctionGetData.GetDataIntensityToCalculateOutput(nOperaID, nFacID, sYear);
        var qWater = FunctionGetData.GetDataWaterToCalculateOutput(nOperaID, nFacID, sYear);
        var qEffluent = FunctionGetData.GetDataEffluentToCalculateOutput(nOperaID, nFacID, sYear);

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            #region call funtion calculate
            Target = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 0, "");
            M1 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 1, "");
            M2 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 2, "");
            M3 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 3, "");
            M4 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 4, "");
            M5 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 5, "");
            M6 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 6, "");
            M7 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 7, "");
            M8 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 8, "");
            M9 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 9, "");
            M10 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 10, "");
            M11 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 11, "");
            M12 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 12, "");
            //decimal?[] arrDataYTD = { M1, M2, M3, M4, M5, M6, M7, M8, M9, M10, M11, M12 };
            Total = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysYTD);//EPIFunc.SumDataToDecimal(arrDataYTD);

            /*decimal?[] arrDataQ1 = { M1, M2, M3 };
            decimal?[] arrDataQ2 = { M4, M5, M6 };
            decimal?[] arrDataQ3 = { M7, M8, M9 };
            decimal?[] arrDataQ4 = { M10, M11, M12 };
            */
            Q1 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysQ1);
            Q2 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysQ2);
            Q3 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysQ3);
            Q4 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysQ4);

            //decimal?[] arrDataH1 = { Q1, Q2 };
            //decimal?[] arrDataH2 = { Q3, Q4 };
            H1 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysH1);
            H2 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysH2);
            #endregion


            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }

        return lstTempReturn;

    }

    public List<ClassExecute.TDataOutput> CalculateEmission(int nOperaID, int nFacID, string sYear)
    {
        int IDIndicator = 4;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();

        var qIntensity = FunctionGetData.GetDataIntensityToCalculateOutput(nOperaID, nFacID, sYear);
        var qEmission = FunctionGetData.GetDataEmissionToCalculateOutput(nOperaID, nFacID, sYear);

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            #region call funtion calculate
            Target = CalculateEmission(item.ProductID, qEmission, qIntensity, 0, "");
            M1 = CalculateEmission(item.ProductID, qEmission, qIntensity, 1, "");
            M2 = CalculateEmission(item.ProductID, qEmission, qIntensity, 2, "");
            M3 = CalculateEmission(item.ProductID, qEmission, qIntensity, 3, "");
            M4 = CalculateEmission(item.ProductID, qEmission, qIntensity, 4, "");
            M5 = CalculateEmission(item.ProductID, qEmission, qIntensity, 5, "");
            M6 = CalculateEmission(item.ProductID, qEmission, qIntensity, 6, "");
            M7 = CalculateEmission(item.ProductID, qEmission, qIntensity, 7, "");
            M8 = CalculateEmission(item.ProductID, qEmission, qIntensity, 8, "");
            M9 = CalculateEmission(item.ProductID, qEmission, qIntensity, 9, "");
            M10 = CalculateEmission(item.ProductID, qEmission, qIntensity, 10, "");
            M11 = CalculateEmission(item.ProductID, qEmission, qIntensity, 11, "");
            M12 = CalculateEmission(item.ProductID, qEmission, qIntensity, 12, "");

            Total = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysYTD);
            Q1 = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysQ1);
            Q2 = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysQ2);
            Q3 = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysQ3);
            Q4 = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysQ4);

            H1 = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysH1);
            H2 = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysH2);
            #endregion


            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }

        return lstTempReturn;

    }

    public List<ClassExecute.TDataOutput> CalculateSpill(int nOperaID, int nFacID, string sYear)
    {
        int IDIndicator = 9;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            sType = s.sType,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();

        var qSpill = FunctionGetData.GetDataSpillToCalculateOutput(nOperaID, nFacID, sYear, true);
        var qSpillProduct = FunctionGetData.GetDataSpillProductToCalculateOutput(nOperaID, nFacID, sYear);

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;

            #region call funtion calculate
            Target = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 0, "");
            M1 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 1, "");
            M2 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 2, "");
            M3 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 3, "");
            M4 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 4, "");
            M5 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 5, "");
            M6 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 6, "");
            M7 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 7, "");
            M8 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 8, "");
            M9 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 9, "");
            M10 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 10, "");
            M11 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 11, "");
            M12 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 12, "");

            Total = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysYTD);
            Q1 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysQ1);
            Q2 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysQ2);
            Q3 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysQ3);
            Q4 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysQ4);

            H1 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysH1);
            H2 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysH2);
            #endregion

            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }
        return lstTempReturn;
    }

    public List<ClassExecute.TDataOutput> CalculateComplaint(int nOperaID, int nFacID, string sYear)
    {
        int IDIndicator = 1;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();

        var qComplaint = FunctionGetData.GetDataComplaintToCalculateOutput(nOperaID, nFacID, sYear);

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            #region call funtion calculate
            Target = CalculateComplaint(item.ProductID, qComplaint, 0, "");
            M1 = CalculateComplaint(item.ProductID, qComplaint, 1, "");
            M2 = CalculateComplaint(item.ProductID, qComplaint, 2, "");
            M3 = CalculateComplaint(item.ProductID, qComplaint, 3, "");
            M4 = CalculateComplaint(item.ProductID, qComplaint, 4, "");
            M5 = CalculateComplaint(item.ProductID, qComplaint, 5, "");
            M6 = CalculateComplaint(item.ProductID, qComplaint, 6, "");
            M7 = CalculateComplaint(item.ProductID, qComplaint, 7, "");
            M8 = CalculateComplaint(item.ProductID, qComplaint, 8, "");
            M9 = CalculateComplaint(item.ProductID, qComplaint, 9, "");
            M10 = CalculateComplaint(item.ProductID, qComplaint, 10, "");
            M11 = CalculateComplaint(item.ProductID, qComplaint, 11, "");
            M12 = CalculateComplaint(item.ProductID, qComplaint, 12, "");
            Total = CalculateComplaint(item.ProductID, qComplaint, 13, sysYTD); //EPIFunc.SumDataToDecimal(arrDataYTD);

            Q1 = CalculateComplaint(item.ProductID, qComplaint, 13, sysQ1);
            Q2 = CalculateComplaint(item.ProductID, qComplaint, 13, sysQ2);
            Q3 = CalculateComplaint(item.ProductID, qComplaint, 13, sysQ3);
            Q4 = CalculateComplaint(item.ProductID, qComplaint, 13, sysQ4);

            H1 = CalculateComplaint(item.ProductID, qComplaint, 13, sysH1);
            H2 = CalculateComplaint(item.ProductID, qComplaint, 13, sysH2);
            #endregion


            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }

        return lstTempReturn;
    }

    public List<ClassExecute.TDataOutput> CalculateCompliance(int nOperaID, int nFacID, string sYear)
    {
        int IDIndicator = 2;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();

        var qCompliance = FunctionGetData.GetDataComplianceToCalculateOutput(nOperaID, nFacID, sYear);

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            #region call funtion calculate
            Target = CalculateCompliance(item.ProductID, qCompliance, 0, "");
            M1 = CalculateCompliance(item.ProductID, qCompliance, 1, "");
            M2 = CalculateCompliance(item.ProductID, qCompliance, 2, "");
            M3 = CalculateCompliance(item.ProductID, qCompliance, 3, "");
            M4 = CalculateCompliance(item.ProductID, qCompliance, 4, "");
            M5 = CalculateCompliance(item.ProductID, qCompliance, 5, "");
            M6 = CalculateCompliance(item.ProductID, qCompliance, 6, "");
            M7 = CalculateCompliance(item.ProductID, qCompliance, 7, "");
            M8 = CalculateCompliance(item.ProductID, qCompliance, 8, "");
            M9 = CalculateCompliance(item.ProductID, qCompliance, 9, "");
            M10 = CalculateCompliance(item.ProductID, qCompliance, 10, "");
            M11 = CalculateCompliance(item.ProductID, qCompliance, 11, "");
            M12 = CalculateCompliance(item.ProductID, qCompliance, 12, "");
            Total = CalculateCompliance(item.ProductID, qCompliance, 13, sysYTD); //EPIFunc.SumDataToDecimal(arrDataYTD);

            Q1 = CalculateCompliance(item.ProductID, qCompliance, 13, sysQ1);
            Q2 = CalculateCompliance(item.ProductID, qCompliance, 13, sysQ2);
            Q3 = CalculateCompliance(item.ProductID, qCompliance, 13, sysQ3);
            Q4 = CalculateCompliance(item.ProductID, qCompliance, 13, sysQ4);

            H1 = CalculateCompliance(item.ProductID, qCompliance, 13, sysH1);
            H2 = CalculateCompliance(item.ProductID, qCompliance, 13, sysH2);
            #endregion


            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }

        return lstTempReturn;
    }

    /// <summary>
    /// D1 & D7
    /// </summary>
    /// <param name="nProductID"></param>
    /// <param name="lstDataWaste"></param>
    /// <param name="lstDataIntensity"></param>
    /// <param name="nMonth"></param>
    /// <param name="sMode"></param>
    /// <returns></returns>
    public decimal? CalculateWaste_D1(int nProductID, List<ClassExecute.TDataWaste> lstDataWaste, List<ClassExecute.TData_Intensity> lstDataIntensity, int nMonth, string sMode)
    {
        decimal? nReturn = null;
        string sVal1 = "", sVal2 = "", sVal3 = "";
        decimal? nCalc = null;
        decimal? nVal1 = null, nVal2 = null;
        ModeValue = sMode + "";

        switch (nProductID)
        {
            #region calculate
            #region D1
            case 1: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 1, nMonth)); break;
            case 2:
                sVal1 = GetValueFromList(lstDataWaste, 1, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 3:
                sVal1 = GetValueFromList(lstDataWaste, 1, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 4: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 2, nMonth)); break;
            case 5:
                sVal1 = GetValueFromList(lstDataWaste, 2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 6:
                sVal1 = GetValueFromList(lstDataWaste, 2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 7: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 8, nMonth)); break;
            case 8:
                sVal1 = GetValueFromList(lstDataWaste, 8, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 9:
                sVal1 = GetValueFromList(lstDataWaste, 8, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 10: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 16, nMonth)); break;
            case 11:
                sVal1 = GetValueFromList(lstDataWaste, 16, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 12:
                sVal1 = GetValueFromList(lstDataWaste, 16, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 13: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 17, nMonth)); break;
            case 14:
                sVal1 = GetValueFromList(lstDataWaste, 17, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 15:
                sVal1 = GetValueFromList(lstDataWaste, 17, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 16: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 24, nMonth)); break;
            case 17:
                sVal1 = GetValueFromList(lstDataWaste, 24, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 18:
                sVal1 = GetValueFromList(lstDataWaste, 24, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 19: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 104, nMonth)); break;
            case 20:
                sVal1 = GetValueFromList(lstDataWaste, 104, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 21:
                sVal1 = GetValueFromList(lstDataWaste, 104, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 22: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 105, nMonth)); break;
            case 23:
                sVal1 = GetValueFromList(lstDataWaste, 105, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 24:
                sVal1 = GetValueFromList(lstDataWaste, 105, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 25: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 110, nMonth)); break;
            case 26:
                sVal1 = GetValueFromList(lstDataWaste, 110, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 27:
                sVal1 = GetValueFromList(lstDataWaste, 110, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region D7
            case 314: //Specific Total Hazardous Waste Disposed
                sVal1 = GetValueFromList(lstDataWaste, 1, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 315://Specific Hazardous Waste Disposed - Routine
                sVal1 = GetValueFromList(lstDataWaste, 2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 316://Specific Hazardous Waste Disposed - Non-routine
                sVal1 = GetValueFromList(lstDataWaste, 8, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 317://Specific Total Non-hazardous Waste Disposed
                sVal1 = GetValueFromList(lstDataWaste, 16, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 318://Specific Non-hazardous Waste Disposed - Routine
                sVal1 = GetValueFromList(lstDataWaste, 17, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 319://Specific Non-hazardous Waste Disposed - Non-routine
                sVal1 = GetValueFromList(lstDataWaste, 24, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            default://Product Industrial
                nReturn = CalculateWaste_Industrial(nProductID, lstDataWaste, lstDataIntensity, nMonth, sMode);
                break;
            #endregion
        }
        ModeValue = "";
        return nReturn;
    }

    public decimal? CalculateWaste_D2(int nProductID, List<ClassExecute.TDataWaste> lstDataWaste, List<ClassExecute.TData_Intensity> lstDataIntensity, int nMonth, string sMode)
    {
        decimal? nReturn = null;
        string sVal1 = "", sVal2 = "", sVal3 = "";
        decimal? nCalc = null;
        decimal? nVal1 = null, nVal2 = null;
        ModeValue = sMode + "";

        switch (nProductID)
        {
            #region calculate
            case 1: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 1, nMonth)); break;
            case 28:
                sVal1 = GetValueFromList(lstDataWaste, 1, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 4: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 2, nMonth)); break;
            case 29:
                sVal1 = GetValueFromList(lstDataWaste, 2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 7: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 8, nMonth)); break;
            case 30:
                sVal1 = GetValueFromList(lstDataWaste, 8, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 10: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 16, nMonth)); break;
            case 31:
                sVal1 = GetValueFromList(lstDataWaste, 16, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 13: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 17, nMonth)); break;
            case 32:
                sVal1 = GetValueFromList(lstDataWaste, 17, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 16: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 24, nMonth)); break;
            case 33:
                sVal1 = GetValueFromList(lstDataWaste, 24, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            default://Product Industrial
                nReturn = CalculateWaste_Industrial(nProductID, lstDataWaste, lstDataIntensity, nMonth, sMode);
                break;
            #endregion
        }

        ModeValue = "";
        return nReturn;
    }

    public decimal? CalculateWaste_D3(int nProductID, List<ClassExecute.TDataWaste> lstDataWaste, List<ClassExecute.TData_Intensity> lstDataIntensity, int nMonth, string sMode)
    {
        decimal? nReturn = null;
        string sVal1 = "", sVal2 = "", sVal3 = "";
        decimal? nCalc = null;
        decimal? nVal1 = null, nVal2 = null;
        ModeValue = sMode + "";

        switch (nProductID)
        {
            case 1: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 1, nMonth)); break;
            case 4: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 2, nMonth)); break;
            case 7: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 8, nMonth)); break;
            case 10: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 16, nMonth)); break;
            case 13: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 17, nMonth)); break;
            case 16: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 24, nMonth)); break;

            #region  Chemical Transportation & Storage
            case 34:
                sVal1 = GetValueFromList(lstDataWaste, 1, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 35:
                sVal1 = GetValueFromList(lstDataWaste, 2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 36:
                sVal1 = GetValueFromList(lstDataWaste, 8, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 37:
                sVal1 = GetValueFromList(lstDataWaste, 16, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 38:
                sVal1 = GetValueFromList(lstDataWaste, 17, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 39:
                sVal1 = GetValueFromList(lstDataWaste, 24, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region Gas Transmission
            case 40:
                sVal1 = GetValueFromList(lstDataWaste, 1, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 41:
                sVal1 = GetValueFromList(lstDataWaste, 1, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;

            case 42:
                sVal1 = GetValueFromList(lstDataWaste, 2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 43:
                sVal1 = GetValueFromList(lstDataWaste, 2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;

            case 44:
                sVal1 = GetValueFromList(lstDataWaste, 8, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 45:
                sVal1 = GetValueFromList(lstDataWaste, 8, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;

            case 46:
                sVal1 = GetValueFromList(lstDataWaste, 16, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 47:
                sVal1 = GetValueFromList(lstDataWaste, 16, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;

            case 48:
                sVal1 = GetValueFromList(lstDataWaste, 17, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 49:
                sVal1 = GetValueFromList(lstDataWaste, 17, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;

            case 50:
                sVal1 = GetValueFromList(lstDataWaste, 24, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 51:
                sVal1 = GetValueFromList(lstDataWaste, 24, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region Lubrication Oil
            case 52:
                sVal1 = GetValueFromList(lstDataWaste, 1, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 53:
                sVal1 = GetValueFromList(lstDataWaste, 2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 54:
                sVal1 = GetValueFromList(lstDataWaste, 8, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 55:
                sVal1 = GetValueFromList(lstDataWaste, 16, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 56:
                sVal1 = GetValueFromList(lstDataWaste, 17, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 57:
                sVal1 = GetValueFromList(lstDataWaste, 24, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            default://Product Industrial
                nReturn = CalculateWaste_Industrial(nProductID, lstDataWaste, lstDataIntensity, nMonth, sMode);
                break;
        }

        ModeValue = "";
        return nReturn;
    }

    public decimal? CalculateWaste_D4(int nProductID, List<ClassExecute.TDataWaste> lstDataWaste, List<ClassExecute.TData_Intensity> lstDataIntensity, int nMonth, string sMode)
    {
        decimal? nReturn = null;
        string sVal1 = "", sVal2 = "", sVal3 = "";
        decimal? nCalc = null;
        decimal? nVal1 = null, nVal2 = null;
        ModeValue = sMode + "";

        switch (nProductID)
        {
            #region calculate
            case 1: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 1, nMonth)); break;
            case 58:
                sVal1 = GetValueFromList(lstDataWaste, 1, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 4: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 2, nMonth)); break;
            case 59:
                sVal1 = GetValueFromList(lstDataWaste, 2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 7: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 8, nMonth)); break;
            case 60:
                sVal1 = GetValueFromList(lstDataWaste, 8, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 10: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 16, nMonth)); break;
            case 61:
                sVal1 = GetValueFromList(lstDataWaste, 16, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 13: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 17, nMonth)); break;
            case 62:
                sVal1 = GetValueFromList(lstDataWaste, 17, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 16: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 24, nMonth)); break;
            case 63:
                sVal1 = GetValueFromList(lstDataWaste, 24, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            default://Product Industrial
                nReturn = CalculateWaste_Industrial(nProductID, lstDataWaste, lstDataIntensity, nMonth, sMode);
                break;
            #endregion
        }

        ModeValue = "";
        return nReturn;
    }

    public decimal? CalculateWaste_D5(int nProductID, List<ClassExecute.TDataWaste> lstDataWaste, List<ClassExecute.TData_Intensity> lstDataIntensity, int nMonth, string sMode)
    {
        decimal? nReturn = null;
        string sVal1 = "", sVal2 = "", sVal3 = "";
        decimal? nCalc = null;
        decimal? nVal1 = null, nVal2 = null;
        ModeValue = sMode + "";

        switch (nProductID)
        {
            #region calculate
            case 1: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 1, nMonth)); break;
            case 64:
                sVal1 = GetValueFromList(lstDataWaste, 1, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 87, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 4: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 2, nMonth)); break;
            case 65:
                sVal1 = GetValueFromList(lstDataWaste, 2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 87, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 7: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 8, nMonth)); break;
            case 66:
                sVal1 = GetValueFromList(lstDataWaste, 8, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 87, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 10: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 16, nMonth)); break;
            case 67:
                sVal1 = GetValueFromList(lstDataWaste, 16, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 87, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 13: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 17, nMonth)); break;
            case 68:
                sVal1 = GetValueFromList(lstDataWaste, 17, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 87, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 16: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 24, nMonth)); break;
            case 69:
                sVal1 = GetValueFromList(lstDataWaste, 24, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 87, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            default://Product Industrial
                nReturn = CalculateWaste_Industrial(nProductID, lstDataWaste, lstDataIntensity, nMonth, sMode);
                break;
            #endregion
        }

        ModeValue = "";
        return nReturn;
    }

    public decimal? CalculateWaste_D6(int nProductID, List<ClassExecute.TDataWaste> lstDataWaste, List<ClassExecute.TData_Intensity> lstDataIntensity, int nMonth, string sMode)
    {
        decimal? nReturn = null;
        string sVal1 = "", sVal2 = "", sVal3 = "";
        decimal? nCalc = null;
        decimal? nVal1 = null, nVal2 = null;

        ModeValue = sMode + "";
        switch (nProductID)
        {
            #region calculate
            case 1: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 1, nMonth)); break;
            case 70:
                sVal1 = GetValueFromList(lstDataWaste, 1, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 4: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 2, nMonth)); break;
            case 71:
                sVal1 = GetValueFromList(lstDataWaste, 2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 7: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 8, nMonth)); break;
            case 72:
                sVal1 = GetValueFromList(lstDataWaste, 8, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 10: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 16, nMonth)); break;
            case 73:
                sVal1 = GetValueFromList(lstDataWaste, 16, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 13: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 17, nMonth)); break;
            case 74:
                sVal1 = GetValueFromList(lstDataWaste, 17, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 16: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWaste, 24, nMonth)); break;
            case 75:
                sVal1 = GetValueFromList(lstDataWaste, 24, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            default://Product Industrial
                nReturn = CalculateWaste_Industrial(nProductID, lstDataWaste, lstDataIntensity, nMonth, sMode);
                break;
            #endregion
        }

        ModeValue = "";
        return nReturn;
    }

    /// <summary>
    /// New product calculate for epi report เป็นการคำนวณเฉพาะ  Industrial  โดยการตัด Domestic/ municipal waste disposed
    /// </summary>
    /// <param name="nProductID"></param>
    /// <param name="lstDataWaste"></param>
    /// <param name="lstDataIntensity"></param>
    /// <param name="nMonth"></param>
    /// <param name="sMode"></param>
    /// <returns></returns>
    public decimal? CalculateWaste_Industrial(int nProductID, List<ClassExecute.TDataWaste> lstDataWaste, List<ClassExecute.TData_Intensity> lstDataIntensity, int nMonth, string sMode)
    {
        decimal? nReturn = null;
        string sVal1 = "", sVal2 = "", sVal3 = "";
        decimal? nCalc = null;
        decimal? nVal1 = null, nVal2 = null, nVal3 = null;
        ModeValue = sMode + "";

        int nUNITID = 0;
        /*NOTE ต้องเปลงหน่วยก่อนจะนำไปลบ เนื่องจากหน้า Input สามารถเลือกหน่วยได้
         UnitID 2 = Tonnes	Factor = 1.000
         UnitID 3 = Kg Factor =	0.001
         */

        switch (nProductID)
        {
            #region//D1
            case 285://Specific Non-hazardous Waste Disposed - Routine Industrial Non-hazardous Waste >> Tonnes/MBOE
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 286://Specific Non-hazardous Waste Disposed - Routine Industrial Non-hazardous Waste >> Tonnes/TOE
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 295://Specific Non-hazardous Waste Disposed - Non-routine Industrial Non-hazardous Waste >> Tonnes/MBOE
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDNonRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomNonRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomNonRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 296://Specific Non-hazardous Waste Disposed - Non-routine Industrial Non-hazardous Waste >> Tonnes/TOE
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDNonRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomNonRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomNonRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region//D2
            case 287://Specific Non-hazardous Waste Disposed - Routine Industrial Non-hazardous Waste >> Tonnes/MWh
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 297://Specific Non-hazardous Waste Disposed - Non-routine Industrial Non-hazardous Waste >> Tonnes/MWh
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDNonRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomNonRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomNonRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region//D3
            //routine
            case 288://Specific Non-hazardous Waste Disposed - Routine Industrial Non-hazardous Waste >> Tonnes/Tonnes Throughput
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 289://Specific Non-hazardous Waste Disposed - Routine Industrial Non-hazardous Waste >> Tonnes/mmscf
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 290://Specific Non-hazardous Waste Disposed - Routine Industrial Non-hazardous Waste >> Tonnes/mmbtu
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 291://Specific Non-hazardous Waste Disposed - Routine Industrial Non-hazardous Waste >> Tonnes/Litres of Lubricant sold(Filling + Blending + Distribution)
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;

            //Non-routine
            case 298://Specific Non-hazardous Waste Disposed - Non-routine Industrial Non-hazardous Waste >> Tonnes/Tonnes Throughput
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDNonRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomNonRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomNonRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 299://Specific Non-hazardous Waste Disposed - Non-routine Industrial Non-hazardous Waste >> Tonnes/mmscf
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDNonRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomNonRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomNonRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 300://Specific Non-hazardous Waste Disposed - Non-routine Industrial Non-hazardous Waste >> Tonnes/mmbtu
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDNonRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomNonRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomNonRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 301://Specific Non-hazardous Waste Disposed - Non-routine Industrial Non-hazardous Waste >> Tonnes/Litres of Lubricant sold(Filling + Blending + Distribution)
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDNonRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomNonRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomNonRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region//D4
            case 292://Specific Non-hazardous Waste Disposed - Routine Industrial Non-hazardous Waste >> Tonnes/Tonnes Product
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 302://Specific Non-hazardous Waste Disposed - Non-routine Industrial Non-hazardous Waste >> Tonnes/Tonnes Product
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDNonRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomNonRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomNonRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region//D5
            case 293://Specific Non-hazardous Waste Disposed - Routine Industrial Non-hazardous Waste >> Tonnes/Persons
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 87, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 303://Specific Non-hazardous Waste Disposed - Non-routine Industrial Non-hazardous Waste >> Tonnes/Persons
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDNonRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 87, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomNonRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomNonRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region//D6
            case 294://Specific Non-hazardous Waste Disposed - Routine Industrial Non-hazardous Waste >> Tonnes/Refining Tonnes Throughput
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 304://Specific Non-hazardous Waste Disposed - Non-routine Industrial Non-hazardous Waste >> Tonnes/Refining Tonnes Throughput
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDNonRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomNonRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomNonRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region//D7
            case 320://Specific Non-hazardous Waste Disposed - Routine Industrial Non-hazardous Waste
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 321://Specific Non-hazardous Waste Specific Disposed - Non-routine Industrial Non-hazardous Waste
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDNonRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                sVal3 = GetValueFromList(lstDataWaste, nProductIDInputDomNonRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomNonRoutine);
                nVal3 = nUNITID == UnitIDKG ? (nVal3 * FactorKG) : nVal3;

                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 - nVal3) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region All
            case 305://Non-hazardous Waste Disposed - Routine Industrial Non-hazardous Waste >> Tonnes
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataWaste, nProductIDInputDomRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomRoutine);
                nVal2 = nUNITID == UnitIDKG ? (nVal2 * FactorKG) : nVal2;

                nCalc = nVal1 - (nVal2 ?? 0);
                nReturn = nCalc;
                break;
            case 306://Non-hazardous Waste Disposed - Non-routine Industrial Non-hazardous Waste >> Tonnes
                sVal1 = GetValueFromList(lstDataWaste, nProductIDInputNHZDNonRoutine, nMonth);
                sVal2 = GetValueFromList(lstDataWaste, nProductIDInputDomNonRoutine, nMonth);

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);

                nUNITID = GetUnitIDOfProduct(lstDataWaste, nProductIDInputDomNonRoutine);
                nVal2 = nUNITID == UnitIDKG ? (nVal2 * FactorKG) : nVal2;

                nCalc = nVal1 - (nVal2 ?? 0);
                nReturn = nCalc;
                break;
            #endregion
        }
        ModeValue = "";
        return nReturn;
    }

    public decimal? CalculateWater(int nProductID, List<ClassExecute.TData_Water> lstDataWater, List<ClassExecute.TData_Intensity> lstDataIntensity, int nMonth, string sMode)
    {
        decimal? nReturn = null;
        string sVal1 = "", sVal2 = "", sVal3 = "";
        decimal? nCalc = null;
        decimal? nVal1 = null, nVal2 = null;
        decimal? nDiv = null;
        ModeValue = sMode + "";

        switch (nProductID)
        {
            #region calculate
            case 76: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataWater, 91, nMonth)); break;
            case 77:
                sVal1 = GetValueFromList(lstDataWater, 91, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 78:
                sVal1 = GetValueFromList(lstDataWater, 91, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 79:
                sVal1 = GetValueFromList(lstDataWater, 91, nMonth); // 115
                sVal2 = GetValueFromList(lstDataWater, 101, nMonth); // 32
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal1 != null || nVal2 != null) // Row 32 + Row 115 In Excel
                {
                    nDiv = SystemFunction.GetNumberNullToZero(sVal1) + SystemFunction.GetNumberNullToZero(sVal2);
                }

                if (nDiv > 0)
                {
                    nReturn = (nVal2 * 100) / nDiv;
                }
                else if (nDiv != null)
                {
                    nReturn = 0;
                }
                break;
            case 80:
                sVal1 = GetValueFromList(lstDataWater, 91, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 81:
                sVal1 = GetValueFromList(lstDataWater, 91, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 82:
                sVal1 = GetValueFromList(lstDataWater, 91, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 83:
                sVal1 = GetValueFromList(lstDataWater, 91, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 84:
                sVal1 = GetValueFromList(lstDataWater, 91, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 85:
                sVal1 = GetValueFromList(lstDataWater, 91, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 86:
                sVal1 = GetValueFromList(lstDataWater, 91, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 87, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 87:
                sVal1 = GetValueFromList(lstDataWater, 91, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 313:
                sVal1 = GetValueFromList(lstDataWater, 91, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion
        }

        ModeValue = "";
        return nReturn;
    }

    public decimal? CalculateMaterial(int nProductID, List<ClassExecute.TDataMaterial> lstDataMaterial, List<ClassExecute.TData_Intensity> lstDataIntensity, int nMonth, string sMode)
    {
        decimal? nReturn = null;
        string sVal1 = "", sVal2 = "", sVal3 = "";
        decimal? nCalc = null;
        decimal? nVal1 = null, nVal2 = null;
        decimal? nDiv = null;
        ModeValue = sMode + "";

        switch (nProductID)
        {
            #region //D1
            case 88: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataMaterial, 34, nMonth)); break;
            case 89:
                sVal1 = GetValueFromList(lstDataMaterial, 34, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 90:
                sVal1 = GetValueFromList(lstDataMaterial, 34, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 91: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataMaterial, 37, nMonth)); break;
            case 92:
                sVal1 = GetValueFromList(lstDataMaterial, 37, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 93:
                sVal1 = GetValueFromList(lstDataMaterial, 37, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 94: nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataMaterial, 33, nMonth)); break;
            case 95:
                sVal1 = GetValueFromList(lstDataMaterial, 33, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 96:
                sVal1 = GetValueFromList(lstDataMaterial, 33, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 97: //IF(OR(G170=0,G18=0),"-",(G170/G18)*100) // ** (41/33)*100

                sVal1 = GetValueFromList(lstDataMaterial, 41, nMonth); sVal2 = GetValueFromList(lstDataMaterial, 33, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 / nVal2) * 100;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }

                break;
            #endregion

            #region//D2
            case 98:
                sVal1 = GetValueFromList(lstDataMaterial, 34, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 99:
                sVal1 = GetValueFromList(lstDataMaterial, 37, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 100:
                sVal1 = GetValueFromList(lstDataMaterial, 33, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region //D3
            //D3 Chemical Transportation & Storage
            case 101:
                sVal1 = GetValueFromList(lstDataMaterial, 34, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 102:
                sVal1 = GetValueFromList(lstDataMaterial, 37, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 103:
                sVal1 = GetValueFromList(lstDataMaterial, 33, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            //*** D3 Gas Transmission
            case 104:
                sVal1 = GetValueFromList(lstDataMaterial, 34, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 105:
                sVal1 = GetValueFromList(lstDataMaterial, 34, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 106:
                sVal1 = GetValueFromList(lstDataMaterial, 37, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 107:
                sVal1 = GetValueFromList(lstDataMaterial, 37, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 108:
                sVal1 = GetValueFromList(lstDataMaterial, 33, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 109:
                sVal1 = GetValueFromList(lstDataMaterial, 33, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            //D3 Lubrication Oil
            case 110:
                sVal1 = GetValueFromList(lstDataMaterial, 34, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 111:
                sVal1 = GetValueFromList(lstDataMaterial, 37, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 112:
                sVal1 = GetValueFromList(lstDataMaterial, 33, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region//D4
            case 113:
                sVal1 = GetValueFromList(lstDataMaterial, 34, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 114:
                sVal1 = GetValueFromList(lstDataMaterial, 37, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 115:
                sVal1 = GetValueFromList(lstDataMaterial, 33, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region//D5
            case 116:
                sVal1 = GetValueFromList(lstDataMaterial, 34, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 86, 13);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 117:
                sVal1 = GetValueFromList(lstDataMaterial, 37, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 86, 13);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 118:
                sVal1 = GetValueFromList(lstDataMaterial, 33, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 86, 13);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region//D6
            case 119:
                sVal1 = GetValueFromList(lstDataMaterial, 34, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 120:
                sVal1 = GetValueFromList(lstDataMaterial, 37, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 121:
                sVal1 = GetValueFromList(lstDataMaterial, 33, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region//D7
            case 310:
                sVal1 = GetValueFromList(lstDataMaterial, 34, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 311:
                sVal1 = GetValueFromList(lstDataMaterial, 37, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 312:
                sVal1 = GetValueFromList(lstDataMaterial, 33, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion
        }

        ModeValue = "";
        return nReturn;
    }

    public decimal? CalculateEffulent(int nProductID, List<ClassExecute.TData_Effluent> lstDataEffluntProductAndProductPoint, List<ClassExecute.TData_Intensity> lstDataIntensity, List<ClassExecute.TData_Water> lstDataWater, int nMonth, string sMode) //lstDataEffluntProductAndProductPoint เป็นตัวรวมหน้าจอ Input Main Option และ Sub Option
    {
        decimal? nReturn = null;
        string sVal1 = "", sVal2 = "", sVal3 = "", sVal4 = "";
        decimal? nCalc = null;
        decimal? nVal1 = null, nVal2 = null, nVal3 = null, nVal4 = null;
        decimal? nDiv = null;
        ModeValue = sMode + "";

        switch (nProductID)
        {
            #region C1
            case 122://Total Water Discharge (M3)
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEffluntProductAndProductPoint, 115, nMonth));
                break;
            case 123://Specific Total Water Discharge (M3/MOBE)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 115, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32MBOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 124://Specific Total Water Discharge (M3/TOE)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 115, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32TOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;

            case 125://COD Discharge (TON)
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEffluntProductAndProductPoint, 126, nMonth));
                break;
            case 126://Specific COD Discharge (TON/MOBE)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 126, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorTON2MBOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 127://Specific COD Discharge (TON/TOE)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 126, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorTON2TOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;

            case 128://BOD Discharge (TON)
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEffluntProductAndProductPoint, 129, nMonth));
                nReturn = nReturn * FactorMGL2TON;
                break;
            case 129://Specific BOD Discharge (Ton/MBOE)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 129, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2MBOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 130://Specific BOD Discharge (Ton/TOE)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 129, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2TOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;

            case 131://SS Discharge (Ton) >> ยกเลิก TSS Discharge (Ton) >>>> แก้ไขตาม CR. โดยยกเลิการใช้งาน TSS(inputid 131) เปลี่ยนเป็น SS(inputid 135) แทน
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEffluntProductAndProductPoint, 135, nMonth));
                nReturn = nReturn * FactorMGL2TON;
                break;
            case 132://Specific SS Discharge (Ton/MBOE)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 135, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2MBOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 133://Specific SS Discharge (Ton/TOE)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 135, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2TOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;

            case 134://Oil & Grease Discharge  (Ton)
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEffluntProductAndProductPoint, 133, nMonth));
                nReturn = nReturn * FactorMGL2TON;
                break;
            case 135://Specific Oil & Grease Discharge  (Ton/MBOE)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 133, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2MBOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 136://Specific Oil & Grease Discharge  (Ton/TOE)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 133, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2TOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C2
            case 137://Specific Total Water Discharge (M3/Tonnes Throughput)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 115, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32TonnesThroughput) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 138://Specific COD Discharge (Ton/Tonnes Throughput)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 126, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorTON2TonnesThroughput)) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 139://Specific BOD Discharge (Ton/Tonnes Throughput)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 129, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2TonnesThroughput) / nVal2; //Remark การคำนวณ : แปลงหน่วย mg/l > Ton > แปลงต่อไปเป็น > Tonnes Throughput เพื่อให้เป็นหน่วยเดียวกับตัวหาร
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 140://Specific SS Discharge (Ton/Tonnes Throughput)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 135, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2TonnesThroughput) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 141://Specific Oil & Grease Discharge (Ton/Tonnes Throughput)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 133, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2TonnesThroughput) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C3
            case 142://Specific Total Water Discharge (m3/mmscf)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 115, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32mmscf) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 143://Specific Total Water Discharge (m3/mmbtu)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 115, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32mmbtu) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 144://Specific COD Discharge (Ton/mmscf)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 126, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorTON2mmscf) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 145://Specific COD Discharge (Ton/mmbtu)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 126, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorTON2mmbtu) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 146://Specific BOD Discharge(Ton/mmscf)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 129, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2mmscf) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 147://Specific BOD Discharge(Ton/mmbtu)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 129, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2mmbtu) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 148://Specific SS Discharge(Ton/mmscf)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 135, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2mmscf) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 149://Specific TSS Discharge(Ton/mmbtu)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 135, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2mmbtu) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 150://Specific Oil & Grease Discharge (Ton/mmscf)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 133, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2mmscf) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 151://Specific Oil & Grease Discharge (Ton/mmbtu)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 133, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2mmbtu) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C4
            case 152://Specific Total Water Discharge (m<sup>3</sup>/Litres of Lubricant sold (Filling + Blending + Distribution))
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 115, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32LitresofLubricantsold) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 153://Specific COD Discharge (Ton/Litres of Lubricant sold (Filling + Blending + Distribution))
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 126, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorTON2LitresofLubricantsold) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 154://Specific BOD Discharge (Ton/Litres of Lubricant sold (Filling + Blending + Distribution))
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 129, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2LitresofLubricantsold) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 155://Specific SS Discharge (Ton/Litres of Lubricant sold (Filling + Blending + Distribution))
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 135, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2LitresofLubricantsold) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 156://Specific Oil & Grease Discharge (Ton/Litres of Lubricant sold (Filling + Blending + Distribution))
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 133, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2LitresofLubricantsold) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C5
            case 157://Specific Total Water Discharge (m<sup>3</sup>/Tonnes Product)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 115, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32TonnesProduct) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 158://Specific COD Discharge (Ton/Tonnes Product)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 126, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorTON2TonnesProduct) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 159://Specific BOD Discharge (Ton/Tonnes Product)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 129, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2TonnesProduct) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 160://Specific SS Discharge (Ton/Tonnes Product)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 135, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2TonnesProduct) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 161://Specific Oil & Grease Discharge (Ton/Tonnes Product)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 133, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2TonnesProduct) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C6 // ใช้ nMonth = 13 เนื่องจาก ตัวหารคือ Total Area ซึ่งจะเก็บค่าลงในคอลัมน์ Total อย่างเดียว
            case 162://Specific Total Water Discharge (m<sup>3</sup>/m<sup>2</sup>)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 115, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 86, 13);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32M2) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 163://Specific COD Discharge (Ton/m<sup>2</sup>)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 126, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 86, 13);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorTON2M2) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 164://Specific BOD Discharge (Ton/m<sup>2</sup>)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 129, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 86, 13);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2M2) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 165://Specific SS Discharge (Ton/m<sup>2</sup>)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 135, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 86, 13);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2M2) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 166://Specific Oil & Grease Discharge (Ton/m<sup>2</sup>)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 133, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 86, 13);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2M2) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C7
            case 167://Specific Total Water Discharge (m<sup>3</sup>/Refining Tonnes Throughput)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 115, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32RefiningTonnesThroughput) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 168://Specific COD Discharge (Ton/Refining Tonnes Throughput)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 126, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorTON2RefiningTonnesThroughput) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 169://Specific BOD Discharge (Ton/Refining Tonnes Throughput)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 129, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2RefiningTonnesThroughput) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 170://Specific SS Discharge (Ton/Refining Tonnes Throughput)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 135, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2RefiningTonnesThroughput) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 171://Specific Oil & Grease Discharge (Ton/Refining Tonnes Throughput)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 133, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2RefiningTonnesThroughput) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C8
            case 172://Specific Total Water Discharge (m<sup>3</sup>/MWh)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 115, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32MWh) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 173://Specific COD Discharge (Ton/MWh)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 126, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorTON2MWh) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 174://Specific BOD Discharge (Ton/MWh)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 129, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2MWh) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 175://Specific SS Discharge (Ton/MWh)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 135, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2MWh) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 176://Specific Oil & Grease Discharge (Ton/MWh)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 133, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2MWh) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C9 >> Exploration and Production >> ยังขาดConsumptive ที่ยังไม่สมบูรณ์
            case 177://Total Produced Water Discharge (m<sup>3</sup>)
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEffluntProductAndProductPoint, 155, nMonth)); // >> Produced water discharged
                break;
            case 178://Consumptive  water (m<sup>3</sup>) use all operation type>> {Fresh water withdrawal} – {total water discharge} – {discharge to sea} – {Total water discharge to same are} >> Note : discharge to sea = saltwater

                sVal1 = GetValueFromList(lstDataWater, 92, nMonth); //Fresh water withdrawal
                sVal2 = GetValueFromList(lstDataEffluntProductAndProductPoint, 115, nMonth); //{total water discharge} 
                sVal3 = GetValueFromList(lstDataWater, 119, nMonth); //discharge to saltwater
                sVal4 = GetValueFromList(lstDataWater, 118, nMonth); //Discharge to Municipal treatment plant >> **** รอคำตอบจาก user

                nVal1 = SystemFunction.GetDecimalNull(sVal1);
                nVal2 = SystemFunction.GetDecimalNull(sVal2);
                nVal3 = SystemFunction.GetDecimalNull(sVal3);
                nVal4 = SystemFunction.GetDecimalNull(sVal4);
                if (nVal1 != null)
                {
                    nCalc = (nVal1 ?? 0) - (nVal2 ?? 0) - (nVal3 ?? 0) - (nVal4 ?? 0);
                }
                nReturn = nCalc;
                break;
            case 179://Hydrocarbon Discharge(Ton)
                nCalc = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEffluntProductAndProductPoint, 158, nMonth));
                nReturn = nCalc * FactorMGL2TON;
                break;
            case 180://Specific Hydrocarbon Discharge (Ton/MBOE)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 158, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2MBOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 181://Specific Hydrocarbon Discharge (Ton/TOE)
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 158, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = ((nVal1 * FactorMGL2TON) * FactorTON2TOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C10 >> Cylinder
            case 323://Specific Total Water Discharge
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 115, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32MBOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 324://Specific COD Discharge
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 126, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32MBOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 325://Specific BOD Discharge
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 129, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32MBOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 326://Specific SS Discharge
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 135, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32MBOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 327://Specific Oil & Grease Discharge 
                sVal1 = GetValueFromList(lstDataEffluntProductAndProductPoint, 133, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = (nVal1 * FactorM32MBOE) / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion
        }

        ModeValue = "";
        return nReturn;
    }

    public decimal? CalculateEmission(int nProductID, List<ClassExecute.TData_Emission> lstDataEmission, List<ClassExecute.TData_Intensity> lstDataIntensity, int nMonth, string sMode)
    {
        decimal? nReturn = null;
        string sVal1 = "", sVal2 = "", sVal3 = "", sVal4 = "";
        decimal? nCalc = null;
        decimal? nVal1 = null, nVal2 = null, nVal3 = null, nVal4 = null;
        decimal? nDiv = null;
        int nPIDTotalNOx = 160, nPIDTotalSO2 = 162, nPIDTotalTSP = 164, nPIDTotalVOC = 193;
        ModeValue = sMode + "";

        switch (nProductID)
        {
            #region C1
            case 182://Total NO<sub>x</sub> Emission Ton
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEmission, nPIDTotalNOx, nMonth));
                break;
            case 183://Specific Total NO<sub>x</sub> Emission Ton/MBOE
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalNOx, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 184://Specific Total NO<sub>x</sub> Emission Ton/TOE
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalNOx, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 185://Total SO<sub>2</sub> Emission Ton
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEmission, nPIDTotalSO2, nMonth));
                break;
            case 186://Specific Total SO<sub>2</sub> Emission Ton/MBOE
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalSO2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 187://Specific Total SO<sub>2</sub> Emission Ton/TOE
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalSO2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 188://Total TSP Emission Ton
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEmission, nPIDTotalTSP, nMonth));
                break;
            case 189://Specific Total TSP Emission Ton/MBOE
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalTSP, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 190://Specific Total TSP Emission Ton/TOE
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalTSP, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 192://Total CO Emission Ton
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEmission, 166, nMonth));
                break;
            case 193://Specific Total CO Emission Ton/MBOE
                sVal1 = GetValueFromList(lstDataEmission, 166, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 194://Specific Total CO Emission Ton/TOE
                sVal1 = GetValueFromList(lstDataEmission, 166, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 195://Total Hg Emission Ton
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEmission, 168, nMonth));
                break;
            case 196://Specific Total Hg Emission Ton/MBOE
                sVal1 = GetValueFromList(lstDataEmission, 168, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 197://Specific Total Hg Emission Ton/TOE
                sVal1 = GetValueFromList(lstDataEmission, 168, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 198://Total VOC Emission Ton
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEmission, nPIDTotalVOC, nMonth));
                break;
            case 199://Specific Total VOC Emission Ton/MBOE
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalVOC, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 200://Specific Total VOC Emission Ton/TOE
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalVOC, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C2
            case 202://Specific Total VOC Emission Ton/Tonnes Throughput
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalVOC, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 79, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;


            case 203://Specific Total NO<sub>x</sub> Emission Ton/mmscf
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalNOx, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 204://Specific Total NO<sub>x</sub> Emission Ton/mmbtu
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalNOx, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 205://Specific Total SO<sub>2</sub> Emission Ton/mmscf
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalSO2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 206://Specific Total SO<sub>2</sub> Emission Ton/mmbtu
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalSO2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 207://Specific Total TSP Emission Ton/mmscf
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalTSP, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 208://Specific Total TSP Emission Ton/mmbtu
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalTSP, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 209://Specific Total VOC Emission Ton/mmscf
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalVOC, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 80, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 210://Specific Total VOC Emission Ton/mmbtu
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalVOC, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 81, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;

            case 211://Specific Total VOC Emission Ton/Litres of Lubricant sold (Filling + Blending + Distribution)
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalVOC, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C3
            case 212://Specific Total NO<sub>x</sub> Emission Ton/Tonnes Product
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalNOx, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 213://Specific Total SO<sub>2</sub> Emission Ton/Tonnes Product
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalSO2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 214://Specific Total TSP Emission Ton/Tonnes Product
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalTSP, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 215://Specific Total VOC Emission Ton/Tonnes Product
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalVOC, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 83, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C4 ******** ใช้ nMonth = 13 เนื่องจาก ตัวหารคือ Total Area ซึ่งจะเก็บค่าลงในคอลัมน์ Total อย่างเดียว
            case 216://Specific Total NO<sub>x</sub> Emission Ton/m<sup>2</sup>
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalNOx, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 86, 13);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 217://Specific Total SO<sub>2</sub> Emission Ton/m<sup>2</sup>
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalSO2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 86, 13);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 218://Specific Total TSP Emission Ton/m<sup>2</sup>
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalTSP, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 86, 13);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 219://Specific Total VOC Emission Ton/m<sup>2</sup>
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalVOC, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 86, 13);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C5
            case 220://Specific Total NO<sub>x</sub> Emission Ton/Refining Tonnes Throughput
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalNOx, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 221://Specific Total SO<sub>2</sub> Emission Ton/Refining Tonnes Throughput
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalSO2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 222://Specific Total TSP Emission Ton/Refining Tonnes Throughput
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalTSP, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 223://Specific Total VOC Emission Ton/Refining Tonnes Throughput
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalVOC, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 88, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C6
            case 224://Specific Total NO<sub>x</sub> Emission Ton/MWh
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalNOx, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 225://Specific Total SO<sub>2</sub> Emission Ton/MWh
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalSO2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 226://Specific Total TSP Emission Ton/MWh
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalTSP, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 227://Specific Total VOC Emission Ton/MWh
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalVOC, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 78, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C7
            case 228://Total H<sub>2</sub>S Emission Ton
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataEmission, 170, nMonth));
                break;
            case 229://Specific Total H<sub>2</sub>S Emission Ton/MBOE
                sVal1 = GetValueFromList(lstDataEmission, 170, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 42, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 230://Specific Total H<sub>2</sub>S Emission Ton/TOE
                sVal1 = GetValueFromList(lstDataEmission, 170, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 43, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C8 //Lubrication Oil >> Tonnes/Litres of Lubricant sold (Filling + Blending + Distribution)
            case 307: //Specific Total NO<sub>x</sub> Emission
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalNOx, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 308: //Specific Total SO<sub>2</sub> Emission
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalSO2, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            case 309: //Specific Total TSP Emission
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalTSP, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 82, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion

            #region C9 >> Cylinder
            case 322:
                sVal1 = GetValueFromList(lstDataEmission, nPIDTotalVOC, nMonth); sVal2 = GetValueFromList(lstDataIntensity, 230, nMonth);
                nVal1 = SystemFunction.GetDecimalNull(sVal1); nVal2 = SystemFunction.GetDecimalNull(sVal2);
                if (nVal2 != null && nVal2.Value > 0)
                {
                    nCalc = nVal1 / nVal2;
                    nReturn = nCalc;
                }
                else { nReturn = GetValueFromVariableDiv(nVal1, nVal2); }
                break;
            #endregion
        }

        ModeValue = "";
        return nReturn;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nProductID"></param>
    /// <param name="lstDataSpill">จะต้องแปลงค่า Volume ให้เป็น Barrel มาให้</param>
    /// <param name="sTypeProductOutput">sType ของตัว Output เพื่อนำไปหา PrimaryReasonID โดยค่านั้นจะตรงกับ ID ใน TData_Type </param>
    /// <param name="nSpillOption">1 = Spill,2 = Significant Spill</param>
    /// <param name="nMonth"></param>
    /// <param name="sMode"></param>
    /// <returns>Voume unit M3 for show on tab output ยกเว้น Product(281) ที่ดึงตรงมาจาก Input</returns>
    public decimal? CalculateSpill(int nProductID, List<ClassExecute.TData_Spill> lstDataSpill, List<ClassExecute.TData_Spill_Product> lstDataSpillProudct, string sTypeProductOutput, int nMonth, string sMode)
    {
        /*
         * หน่วยที่ใช้เทียบต้องเป็น barrels
         * แยกเป็น Spill และ Significant Spill
         * Volume ตั้งแต่ 1-99 barrels เป็น Spill
         * Volume >= 100 barrels ขึ้นไป เป็น significant spill 
         * */


        decimal? nReturn = null;
        int nPrimaryReasonID = SystemFunction.ParseInt(sTypeProductOutput);
        string sHC = "HC", sNHC = "NHC";

        ModeValue = sMode + "";
        int nSpillOption = nProductID < 256 ? 1 : 2;

        switch (nProductID)
        {
            #region Spill >> nSpillOption = 1
            //Equipment failure
            case 231://Number of Hydrocarbon Spills
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 232://Volume of Hydrocarbon Spills
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 234://Number of Non-Hydrocarbon Spills
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            case 235://Volume of Non-Hydrocarbon Spills
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;

            //Corrosion
            case 236:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 237:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 238:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            case 239:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;

            //Operator or technical error
            case 240:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 241:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 242:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            case 243:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;

            //Third Party Damage
            case 244:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 245:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 246:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            case 247:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;

            //Unknown
            case 248:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 249:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 250:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            case 251:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;

            //Other (specify)
            case 252:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 253:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 254:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            case 255:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            #endregion

            #region Significant Spill >> nSpillOption = 2
            //Equipment failure
            case 256://Number of Hydrocarbon Spills
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 257://Volume of Hydrocarbon Spills
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 258://Number of Non-Hydrocarbon Spills
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            case 259://Volume of Non-Hydrocarbon Spills
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;

            //Corrosion
            case 260:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 261:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 262:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            case 263:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;

            //Operator or technical error
            case 264:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 265:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 266:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            case 267:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;

            //Third Party Damage
            case 268:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 269:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 270:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            case 271:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;

            //Unknown
            case 272:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 273:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 274:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            case 275:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;

            //Other (specify)
            case 276:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 277:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sHC, nSpillOption, nMonth);
                break;
            case 278:
                nReturn = CountSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            case 279:
                nReturn = SumSpill(lstDataSpill, nPrimaryReasonID, sNHC, nSpillOption, nMonth);
                break;
            #endregion

            //From Input
            case 280://Number of Spill
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataSpillProudct, 209, nMonth));
                break;
            case 281://Volume of Spill
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataSpillProudct, 210, nMonth));
                break;
            case 328://Volume of Spill M3
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataSpillProudct, 256, nMonth));
                break;
        }

        ModeValue = "";
        return nReturn;
    }

    public decimal? CalculateComplaint(int nProductID, List<ClassExecute.TData_Complaint_Product> lstDataComplaint, int nMonth, string sMode)
    {
        decimal? nReturn = null;
        string sVal1 = "", sVal2 = "", sVal3 = "";
        decimal? nCalc = null;
        decimal? nVal1 = null, nVal2 = null;
        decimal? nDiv = null;
        ModeValue = sMode + "";

        switch (nProductID)
        {
            case 282:
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataComplaint, 211, nMonth));
                break;
            case 283:
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataComplaint, 212, nMonth));
                break;
        }

        ModeValue = "";
        return nReturn;
    }

    public decimal? CalculateCompliance(int nProductID, List<ClassExecute.TData_Compliance_Product> lstDataComplaint, int nMonth, string sMode)
    {
        decimal? nReturn = null;
        string sVal1 = "", sVal2 = "", sVal3 = "";
        decimal? nCalc = null;
        decimal? nVal1 = null, nVal2 = null;
        decimal? nDiv = null;
        ModeValue = sMode + "";

        switch (nProductID)
        {
            case 284:
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataComplaint, 213, nMonth));
                break;
            case 285:
                nReturn = SystemFunction.GetDecimalNull(GetValueFromList(lstDataComplaint, 214, nMonth));
                break;
        }

        ModeValue = "";
        return nReturn;
    }

    public decimal? GetValueFromVariableDiv(decimal? val1, decimal? val2) // val1 คือ ตัวตั้ง, val2 คือ ตัวหาร
    {
        decimal? nVal = null;
        if (val2 != null && val2.Value == 0 && val1 != null)
        {
            nVal = 0;
        }
        return nVal;
    }

    public static decimal? ConvertValueCauseUnitPerson(decimal? nVal) //**** Note หน่วยเป็น Person(คน) ให้ปัดทศนิยมขึ้นทั้งหมด
    {
        decimal? nRetrun = null;
        if (nVal != null)
        {
            if (nVal.Value > 0)
            {
                string[] ArrVal = (nVal.Value + "").Split('.');
                if (ArrVal.Length > 1)
                {
                    int nTemp1 = SystemFunction.ParseInt(ArrVal[0]); // ก่อนจุด
                    int nTemp2 = SystemFunction.ParseInt(ArrVal[1]); // หลังจุด
                    if (nTemp2 > 0)
                    {
                        nRetrun = nTemp1 + 1;
                    }
                    else
                    {
                        nRetrun = nTemp1;
                    }
                }
                else
                {
                    nRetrun = nVal;
                }
            }
            else
            {
                nRetrun = 0;
            }
        }

        return nRetrun;
    }

    private string GetValueFromList(List<ClassExecute.TData_Intensity> lstDataIntensity, int nProductID, int nMonth) //Edit 10.12.2557
    {
        string sResult = "";

        var query = lstDataIntensity.Where(w => w.ProductID == nProductID).FirstOrDefault();
        if (query != null)
        {
            if (ModeValue + "" == "")
            {
                #region Normal calculate
                switch (nMonth)
                {
                    case 0: sResult = query.Target; break;
                    case 1: sResult = query.M1; break;
                    case 2: sResult = query.M2; break;
                    case 3: sResult = query.M3; break;
                    case 4: sResult = query.M4; break;
                    case 5: sResult = query.M5; break;
                    case 6: sResult = query.M6; break;
                    case 7: sResult = query.M7; break;
                    case 8: sResult = query.M8; break;
                    case 9: sResult = query.M9; break;
                    case 10: sResult = query.M10; break;
                    case 11: sResult = query.M11; break;
                    case 12: sResult = query.M12; break;
                    case 13: sResult = query.nTotal; break;
                }
                #endregion
            }
            else
            {
                #region Get value by case // 09.12.2557
                string[] arrVal = new string[12];
                decimal? nValReturn = null;
                switch (ModeValue)
                {
                    case sysYTD:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH2:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, "", "", "", "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ2:
                        arrVal = new string[] { "", "", "", query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ3:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ4:
                        arrVal = new string[] { "", "", "", "", "", "", "", "", "", query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysRC:
                        #region Call from tab output on EPI Form
                        List<string> lstDataCal = new List<string>();
                        if (CheckRecalQ1)
                        {
                            lstDataCal.Add(query.M1); lstDataCal.Add(query.M2); lstDataCal.Add(query.M3);
                        }
                        if (CheckRecalQ2)
                        {
                            lstDataCal.Add(query.M4); lstDataCal.Add(query.M5); lstDataCal.Add(query.M6);
                        }
                        if (CheckRecalQ3)
                        {
                            lstDataCal.Add(query.M7); lstDataCal.Add(query.M8); lstDataCal.Add(query.M9);
                        }
                        if (CheckRecalQ4)
                        {
                            lstDataCal.Add(query.M10); lstDataCal.Add(query.M11); lstDataCal.Add(query.M12);
                        }
                        #endregion
                        nValReturn = EPIFunc.SumDataToDecimal(lstDataCal);
                        break;
                }
                sResult = nValReturn + "";
                #endregion
            }
        }

        return sResult;
    }

    private string GetValueFromList(List<ClassExecute.TDataWaste> lstData, int nProductID, int nMonth) //Edit 10.12.2557
    {
        string sResult = "";

        var query = lstData.Where(w => w.ProductID == nProductID).FirstOrDefault();
        if (query != null)
        {
            if (ModeValue + "" == "")
            {
                #region Normal calculate
                switch (nMonth)
                {
                    case 0: sResult = query.Target; break;
                    case 1: sResult = query.M1; break;
                    case 2: sResult = query.M2; break;
                    case 3: sResult = query.M3; break;
                    case 4: sResult = query.M4; break;
                    case 5: sResult = query.M5; break;
                    case 6: sResult = query.M6; break;
                    case 7: sResult = query.M7; break;
                    case 8: sResult = query.M8; break;
                    case 9: sResult = query.M9; break;
                    case 10: sResult = query.M10; break;
                    case 11: sResult = query.M11; break;
                    case 12: sResult = query.M12; break;
                }
                #endregion
            }
            else
            {
                #region Get value by case // 09.12.2557
                string[] arrVal = new string[12];
                decimal? nValReturn = null;
                switch (ModeValue)
                {
                    case sysYTD:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH2:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, "", "", "", "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ2:
                        arrVal = new string[] { "", "", "", query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ3:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ4:
                        arrVal = new string[] { "", "", "", "", "", "", "", "", "", query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysRC:
                        #region Call from tab output on EPI Form
                        List<string> lstDataCal = new List<string>();
                        if (CheckRecalQ1)
                        {
                            lstDataCal.Add(query.M1); lstDataCal.Add(query.M2); lstDataCal.Add(query.M3);
                        }
                        if (CheckRecalQ2)
                        {
                            lstDataCal.Add(query.M4); lstDataCal.Add(query.M5); lstDataCal.Add(query.M6);
                        }
                        if (CheckRecalQ3)
                        {
                            lstDataCal.Add(query.M7); lstDataCal.Add(query.M8); lstDataCal.Add(query.M9);
                        }
                        if (CheckRecalQ4)
                        {
                            lstDataCal.Add(query.M10); lstDataCal.Add(query.M11); lstDataCal.Add(query.M12);
                        }
                        #endregion
                        nValReturn = EPIFunc.SumDataToDecimal(lstDataCal);
                        break;
                }
                sResult = nValReturn + "";
                #endregion
            }
        }

        return sResult;
    }

    private string GetValueFromList(List<ClassExecute.TData_Water> lstData, int nProductID, int nMonth) //Edit 10.12.2557
    {
        string sResult = "";

        var query = lstData.Where(w => w.ProductID == nProductID).FirstOrDefault();
        if (query != null)
        {
            if (ModeValue + "" == "")
            {
                #region Normal calculate
                switch (nMonth)
                {
                    case 0: sResult = query.Target; break;
                    case 1: sResult = query.M1; break;
                    case 2: sResult = query.M2; break;
                    case 3: sResult = query.M3; break;
                    case 4: sResult = query.M4; break;
                    case 5: sResult = query.M5; break;
                    case 6: sResult = query.M6; break;
                    case 7: sResult = query.M7; break;
                    case 8: sResult = query.M8; break;
                    case 9: sResult = query.M9; break;
                    case 10: sResult = query.M10; break;
                    case 11: sResult = query.M11; break;
                    case 12: sResult = query.M12; break;
                }
                #endregion
            }
            else
            {
                #region Get value by case // 09.12.2557
                string[] arrVal = new string[12];
                decimal? nValReturn = null;
                switch (ModeValue)
                {
                    case sysYTD:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH2:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, "", "", "", "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ2:
                        arrVal = new string[] { "", "", "", query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ3:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ4:
                        arrVal = new string[] { "", "", "", "", "", "", "", "", "", query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysRC:
                        #region Call from tab output on EPI Form
                        List<string> lstDataCal = new List<string>();
                        if (CheckRecalQ1)
                        {
                            lstDataCal.Add(query.M1); lstDataCal.Add(query.M2); lstDataCal.Add(query.M3);
                        }
                        if (CheckRecalQ2)
                        {
                            lstDataCal.Add(query.M4); lstDataCal.Add(query.M5); lstDataCal.Add(query.M6);
                        }
                        if (CheckRecalQ3)
                        {
                            lstDataCal.Add(query.M7); lstDataCal.Add(query.M8); lstDataCal.Add(query.M9);
                        }
                        if (CheckRecalQ4)
                        {
                            lstDataCal.Add(query.M10); lstDataCal.Add(query.M11); lstDataCal.Add(query.M12);
                        }
                        #endregion
                        nValReturn = EPIFunc.SumDataToDecimal(lstDataCal);
                        break;
                }
                sResult = nValReturn + "";
                #endregion
            }
        }

        return sResult;
    }

    private string GetValueFromList(List<ClassExecute.TDataMaterial> lstData, int nProductID, int nMonth) //Edit 10.12.2557
    {
        string sResult = "";

        var query = lstData.Where(w => w.ProductID == nProductID).FirstOrDefault();
        if (query != null)
        {
            if (ModeValue + "" == "")
            {
                #region Normal calculate
                switch (nMonth)
                {
                    case 0: sResult = query.Target; break;
                    case 1: sResult = query.M1; break;
                    case 2: sResult = query.M2; break;
                    case 3: sResult = query.M3; break;
                    case 4: sResult = query.M4; break;
                    case 5: sResult = query.M5; break;
                    case 6: sResult = query.M6; break;
                    case 7: sResult = query.M7; break;
                    case 8: sResult = query.M8; break;
                    case 9: sResult = query.M9; break;
                    case 10: sResult = query.M10; break;
                    case 11: sResult = query.M11; break;
                    case 12: sResult = query.M12; break;
                }
                #endregion
            }
            else
            {
                #region Get value by case // 09.12.2557
                string[] arrVal = new string[12];
                decimal? nValReturn = null;
                switch (ModeValue)
                {
                    case sysYTD:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH2:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, "", "", "", "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ2:
                        arrVal = new string[] { "", "", "", query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ3:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ4:
                        arrVal = new string[] { "", "", "", "", "", "", "", "", "", query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysRC:
                        #region Call from tab output on EPI Form
                        List<string> lstDataCal = new List<string>();
                        if (CheckRecalQ1)
                        {
                            lstDataCal.Add(query.M1); lstDataCal.Add(query.M2); lstDataCal.Add(query.M3);
                        }
                        if (CheckRecalQ2)
                        {
                            lstDataCal.Add(query.M4); lstDataCal.Add(query.M5); lstDataCal.Add(query.M6);
                        }
                        if (CheckRecalQ3)
                        {
                            lstDataCal.Add(query.M7); lstDataCal.Add(query.M8); lstDataCal.Add(query.M9);
                        }
                        if (CheckRecalQ4)
                        {
                            lstDataCal.Add(query.M10); lstDataCal.Add(query.M11); lstDataCal.Add(query.M12);
                        }
                        #endregion
                        nValReturn = EPIFunc.SumDataToDecimal(lstDataCal);
                        break;
                }
                sResult = nValReturn + "";
                #endregion
            }
        }
        return sResult;
    }

    private string GetValueFromList(List<ClassExecute.TData_Effluent> lstData, int nProductID, int nMonth) //Edit 10.12.2557
    {
        string sResult = "";

        var query = lstData.Where(w => w.ProductID == nProductID).FirstOrDefault();
        if (query != null)
        {
            if (ModeValue + "" == "")
            {
                #region Normal calculate
                switch (nMonth)
                {
                    case 0: sResult = query.Target; break;
                    case 1: sResult = query.M1; break;
                    case 2: sResult = query.M2; break;
                    case 3: sResult = query.M3; break;
                    case 4: sResult = query.M4; break;
                    case 5: sResult = query.M5; break;
                    case 6: sResult = query.M6; break;
                    case 7: sResult = query.M7; break;
                    case 8: sResult = query.M8; break;
                    case 9: sResult = query.M9; break;
                    case 10: sResult = query.M10; break;
                    case 11: sResult = query.M11; break;
                    case 12: sResult = query.M12; break;
                }
                #endregion
            }
            else
            {
                #region Get value by case // 09.12.2557
                string[] arrVal = new string[12];
                decimal? nValReturn = null;
                switch (ModeValue)
                {
                    case sysYTD:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH2:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, "", "", "", "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ2:
                        arrVal = new string[] { "", "", "", query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ3:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ4:
                        arrVal = new string[] { "", "", "", "", "", "", "", "", "", query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysRC:
                        #region Call from tab output on EPI Form
                        List<string> lstDataCal = new List<string>();
                        if (CheckRecalQ1)
                        {
                            lstDataCal.Add(query.M1); lstDataCal.Add(query.M2); lstDataCal.Add(query.M3);
                        }
                        if (CheckRecalQ2)
                        {
                            lstDataCal.Add(query.M4); lstDataCal.Add(query.M5); lstDataCal.Add(query.M6);
                        }
                        if (CheckRecalQ3)
                        {
                            lstDataCal.Add(query.M7); lstDataCal.Add(query.M8); lstDataCal.Add(query.M9);
                        }
                        if (CheckRecalQ4)
                        {
                            lstDataCal.Add(query.M10); lstDataCal.Add(query.M11); lstDataCal.Add(query.M12);
                        }
                        #endregion
                        nValReturn = EPIFunc.SumDataToDecimal(lstDataCal);
                        break;
                }
                sResult = nValReturn + "";
                #endregion
            }
        }
        return sResult;
    }

    private string GetValueFromList(List<ClassExecute.TData_Emission> lstData, int nProductID, int nMonth) //Edit 10.12.2557
    {
        string sResult = "";

        var query = lstData.Where(w => w.ProductID == nProductID).FirstOrDefault();
        if (query != null)
        {
            if (ModeValue + "" == "")
            {
                #region Normal calculate
                switch (nMonth)
                {
                    case 0: sResult = query.Target; break;
                    case 1: sResult = query.M1; break;
                    case 2: sResult = query.M2; break;
                    case 3: sResult = query.M3; break;
                    case 4: sResult = query.M4; break;
                    case 5: sResult = query.M5; break;
                    case 6: sResult = query.M6; break;
                    case 7: sResult = query.M7; break;
                    case 8: sResult = query.M8; break;
                    case 9: sResult = query.M9; break;
                    case 10: sResult = query.M10; break;
                    case 11: sResult = query.M11; break;
                    case 12: sResult = query.M12; break;
                }
                #endregion
            }
            else
            {
                #region Get value by case // 09.12.2557
                string[] arrVal = new string[12];
                decimal? nValReturn = null;
                switch (ModeValue)
                {
                    case sysYTD:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH2:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, "", "", "", "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ2:
                        arrVal = new string[] { "", "", "", query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ3:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ4:
                        arrVal = new string[] { "", "", "", "", "", "", "", "", "", query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysRC:
                        #region Call from tab output on EPI Form
                        List<string> lstDataCal = new List<string>();
                        if (CheckRecalQ1)
                        {
                            lstDataCal.Add(query.M1); lstDataCal.Add(query.M2); lstDataCal.Add(query.M3);
                        }
                        if (CheckRecalQ2)
                        {
                            lstDataCal.Add(query.M4); lstDataCal.Add(query.M5); lstDataCal.Add(query.M6);
                        }
                        if (CheckRecalQ3)
                        {
                            lstDataCal.Add(query.M7); lstDataCal.Add(query.M8); lstDataCal.Add(query.M9);
                        }
                        if (CheckRecalQ4)
                        {
                            lstDataCal.Add(query.M10); lstDataCal.Add(query.M11); lstDataCal.Add(query.M12);
                        }
                        #endregion
                        nValReturn = EPIFunc.SumDataToDecimal(lstDataCal);
                        break;
                }
                sResult = nValReturn + "";
                #endregion
            }
        }
        return sResult;
    }

    private string GetValueFromList(List<ClassExecute.TData_Spill_Product> lstData, int nProductID, int nMonth) //Edit 10.12.2557
    {
        string sResult = "";

        var query = lstData.Where(w => w.ProductID == nProductID).FirstOrDefault();
        if (query != null)
        {
            if (ModeValue + "" == "")
            {
                #region Normal calculate
                switch (nMonth)
                {
                    case 0: sResult = query.Target; break;
                    case 1: sResult = query.M1; break;
                    case 2: sResult = query.M2; break;
                    case 3: sResult = query.M3; break;
                    case 4: sResult = query.M4; break;
                    case 5: sResult = query.M5; break;
                    case 6: sResult = query.M6; break;
                    case 7: sResult = query.M7; break;
                    case 8: sResult = query.M8; break;
                    case 9: sResult = query.M9; break;
                    case 10: sResult = query.M10; break;
                    case 11: sResult = query.M11; break;
                    case 12: sResult = query.M12; break;
                }
                #endregion
            }
            else
            {
                #region Get value by case // 09.12.2557
                string[] arrVal = new string[12];
                decimal? nValReturn = null;
                switch (ModeValue)
                {
                    case sysYTD:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH2:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, "", "", "", "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ2:
                        arrVal = new string[] { "", "", "", query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ3:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ4:
                        arrVal = new string[] { "", "", "", "", "", "", "", "", "", query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysRC:
                        #region Call from tab output on EPI Form
                        List<string> lstDataCal = new List<string>();
                        if (CheckRecalQ1)
                        {
                            lstDataCal.Add(query.M1); lstDataCal.Add(query.M2); lstDataCal.Add(query.M3);
                        }
                        if (CheckRecalQ2)
                        {
                            lstDataCal.Add(query.M4); lstDataCal.Add(query.M5); lstDataCal.Add(query.M6);
                        }
                        if (CheckRecalQ3)
                        {
                            lstDataCal.Add(query.M7); lstDataCal.Add(query.M8); lstDataCal.Add(query.M9);
                        }
                        if (CheckRecalQ4)
                        {
                            lstDataCal.Add(query.M10); lstDataCal.Add(query.M11); lstDataCal.Add(query.M12);
                        }
                        #endregion
                        nValReturn = EPIFunc.SumDataToDecimal(lstDataCal);
                        break;
                }
                sResult = nValReturn + "";
                #endregion
            }
        }
        return sResult;
    }

    private string GetValueFromList(List<ClassExecute.TData_Complaint_Product> lstData, int nProductID, int nMonth) //Edit 10.12.2557
    {
        string sResult = "";

        var query = lstData.Where(w => w.ProductID == nProductID).FirstOrDefault();
        if (query != null)
        {
            if (ModeValue + "" == "")
            {
                #region Normal calculate
                switch (nMonth)
                {
                    case 0: sResult = query.Target; break;
                    case 1: sResult = query.M1; break;
                    case 2: sResult = query.M2; break;
                    case 3: sResult = query.M3; break;
                    case 4: sResult = query.M4; break;
                    case 5: sResult = query.M5; break;
                    case 6: sResult = query.M6; break;
                    case 7: sResult = query.M7; break;
                    case 8: sResult = query.M8; break;
                    case 9: sResult = query.M9; break;
                    case 10: sResult = query.M10; break;
                    case 11: sResult = query.M11; break;
                    case 12: sResult = query.M12; break;
                }
                #endregion
            }
            else
            {
                #region Get value by case // 09.12.2557
                string[] arrVal = new string[12];
                decimal? nValReturn = null;
                switch (ModeValue)
                {
                    case sysYTD:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH2:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, "", "", "", "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ2:
                        arrVal = new string[] { "", "", "", query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ3:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ4:
                        arrVal = new string[] { "", "", "", "", "", "", "", "", "", query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysRC:
                        #region Call from tab output on EPI Form
                        List<string> lstDataCal = new List<string>();
                        if (CheckRecalQ1)
                        {
                            lstDataCal.Add(query.M1); lstDataCal.Add(query.M2); lstDataCal.Add(query.M3);
                        }
                        if (CheckRecalQ2)
                        {
                            lstDataCal.Add(query.M4); lstDataCal.Add(query.M5); lstDataCal.Add(query.M6);
                        }
                        if (CheckRecalQ3)
                        {
                            lstDataCal.Add(query.M7); lstDataCal.Add(query.M8); lstDataCal.Add(query.M9);
                        }
                        if (CheckRecalQ4)
                        {
                            lstDataCal.Add(query.M10); lstDataCal.Add(query.M11); lstDataCal.Add(query.M12);
                        }
                        #endregion
                        nValReturn = EPIFunc.SumDataToDecimal(lstDataCal);
                        break;
                }
                sResult = nValReturn + "";
                #endregion
            }
        }
        return sResult;
    }

    private string GetValueFromList(List<ClassExecute.TData_Compliance_Product> lstData, int nProductID, int nMonth) //Edit 10.12.2557
    {
        string sResult = "";

        var query = lstData.Where(w => w.ProductID == nProductID).FirstOrDefault();
        if (query != null)
        {
            if (ModeValue + "" == "")
            {
                #region Normal calculate
                switch (nMonth)
                {
                    case 0: sResult = query.Target; break;
                    case 1: sResult = query.M1; break;
                    case 2: sResult = query.M2; break;
                    case 3: sResult = query.M3; break;
                    case 4: sResult = query.M4; break;
                    case 5: sResult = query.M5; break;
                    case 6: sResult = query.M6; break;
                    case 7: sResult = query.M7; break;
                    case 8: sResult = query.M8; break;
                    case 9: sResult = query.M9; break;
                    case 10: sResult = query.M10; break;
                    case 11: sResult = query.M11; break;
                    case 12: sResult = query.M12; break;
                }
                #endregion
            }
            else
            {
                #region Get value by case // 09.12.2557
                string[] arrVal = new string[12];
                decimal? nValReturn = null;
                switch (ModeValue)
                {
                    case sysYTD:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysH2:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ1:
                        arrVal = new string[] { query.M1, query.M2, query.M3, "", "", "", "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ2:
                        arrVal = new string[] { "", "", "", query.M4, query.M5, query.M6, "", "", "", "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ3:
                        arrVal = new string[] { "", "", "", "", "", "", query.M7, query.M8, query.M9, "", "", "" };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysQ4:
                        arrVal = new string[] { "", "", "", "", "", "", "", "", "", query.M10, query.M11, query.M12 };
                        nValReturn = EPIFunc.SumDataToDecimal(arrVal);
                        break;
                    case sysRC:
                        #region Call from tab output on EPI Form
                        List<string> lstDataCal = new List<string>();
                        if (CheckRecalQ1)
                        {
                            lstDataCal.Add(query.M1); lstDataCal.Add(query.M2); lstDataCal.Add(query.M3);
                        }
                        if (CheckRecalQ2)
                        {
                            lstDataCal.Add(query.M4); lstDataCal.Add(query.M5); lstDataCal.Add(query.M6);
                        }
                        if (CheckRecalQ3)
                        {
                            lstDataCal.Add(query.M7); lstDataCal.Add(query.M8); lstDataCal.Add(query.M9);
                        }
                        if (CheckRecalQ4)
                        {
                            lstDataCal.Add(query.M10); lstDataCal.Add(query.M11); lstDataCal.Add(query.M12);
                        }
                        #endregion
                        nValReturn = EPIFunc.SumDataToDecimal(lstDataCal);
                        break;
                }
                sResult = nValReturn + "";
                #endregion
            }
        }
        return sResult;
    }

    /// <summary>
    /// Product output >> Support tab output EPI Froms
    /// สำหรับ Tab Output Indicator Waste จะไม่นำ sType = R ไปเนื่องจากเป็นส่วนที่ใช้ในตอนออกรายงาน(EPI Report) เท่านั้น
    /// </summary>
    /// <param name="nIndID"></param>
    /// <param name="nOperaID"></param>
    /// <returns></returns>
    public static string GetSQLQueryProductOutput(int nIndID, int nOperaID)//หากแก้ไข SQL จะต้องไปแก้ function GetProductOutputToReport ใน sysEPIObjReport ด้วย
    {
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
            string sCondition = "";
            if (nIndID == EPIFunc.nIndWasteID)
            {
                sCondition = " and isnull(a.sType,'') <> 'R' "; //เพื่อไม่ให้ดึง Product ที่ใช้งานใน EPI Report มาออกใน tab output
            }

            sqlProductOutput = @"select a.IDIndicator,a.ProductID,a.ProductName,a.nOrder,a.sUnit,a.sType,
                                    ISNULL(b.UnitID,0) 'UnitID',ISNULL(b.UnitName,a.sUnit) 'UnitName'
                                    from mTProductIndicatorOutput a
                                    inner join TUseProductOutput d on a.ProductID = d.ProductID
                                    left join mTUnit b on a.sUnit = b.UnitName
                                    where a.IDIndicator = " + nIndID + " and d.OperationtypeID = " + nOperaID + sCondition + @"
                                    order by a.nOrder ";
        }
        return sqlProductOutput;
    }

    public static decimal? SumValueToOutput(TIntensityDominator d, string spara)
    {
        string[] arrData = new string[30];
        decimal? nReturn = null;
        if (d != null)
        {
            switch (spara)
            {
                case "Total":
                    arrData = new string[] { d.M1, d.M2, d.M3, d.M4, d.M5, d.M6, d.M7, d.M8, d.M9, d.M10, d.M11, d.M12 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "Q1":
                    arrData = new string[] { d.M1, d.M2, d.M3 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "Q2":
                    arrData = new string[] { d.M4, d.M5, d.M6 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "Q3":
                    arrData = new string[] { d.M7, d.M8, d.M9 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "Q4":
                    arrData = new string[] { d.M10, d.M11, d.M12 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "H1":
                    arrData = new string[] { d.M1, d.M2, d.M3, d.M4, d.M5, d.M6 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "H2":
                    arrData = new string[] { d.M7, d.M8, d.M9, d.M10, d.M11, d.M12 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "FTotal":
                    nReturn = EPIFunc.GetDecimalNull(d.nTotal);
                    break;
                case "Q1Avg":
                    arrData = new string[] { d.M1, d.M2, d.M3 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "Q2Avg":
                    arrData = new string[] { d.M4, d.M5, d.M6 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "Q3Avg":
                    arrData = new string[] { d.M7, d.M8, d.M9 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "Q4Avg":
                    arrData = new string[] { d.M10, d.M11, d.M12 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "H1Avg":
                    arrData = new string[] { d.M1, d.M2, d.M3, d.M4, d.M5, d.M6 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "H2Avg":
                    arrData = new string[] { d.M7, d.M8, d.M9, d.M10, d.M11, d.M12 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "TotalAvg":
                    arrData = new string[] { d.M1, d.M2, d.M3, d.M4, d.M5, d.M6, d.M7, d.M8, d.M9, d.M10, d.M11, d.M12 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "Q1AvgPerson":
                    arrData = new string[] { d.M1, d.M2, d.M3 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
                case "Q2AvgPerson":
                    arrData = new string[] { d.M4, d.M5, d.M6 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
                case "Q3AvgPerson":
                    arrData = new string[] { d.M7, d.M8, d.M9 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
                case "Q4AvgPerson":
                    arrData = new string[] { d.M10, d.M11, d.M12 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
                case "H1AvgPerson":
                    arrData = new string[] { d.M1, d.M2, d.M3, d.M4, d.M5, d.M6 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
                case "H2AvgPerson":
                    arrData = new string[] { d.M7, d.M8, d.M9, d.M10, d.M11, d.M12 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
                case "TotalAvgPerson":
                    arrData = new string[] { d.M1, d.M2, d.M3, d.M4, d.M5, d.M6, d.M7, d.M8, d.M9, d.M10, d.M11, d.M12 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
            }

        }

        return nReturn;
    }

    public static decimal? SumValueToOutput(TSpill_Product d, string spara)
    {
        string[] arrData = new string[30];
        decimal? nReturn = null;
        if (d != null)
        {
            switch (spara)
            {
                case "Total":
                    arrData = new string[] { d.M1, d.M2, d.M3, d.M4, d.M5, d.M6, d.M7, d.M8, d.M9, d.M10, d.M11, d.M12 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "Q1":
                    arrData = new string[] { d.M1, d.M2, d.M3 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "Q2":
                    arrData = new string[] { d.M4, d.M5, d.M6 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "Q3":
                    arrData = new string[] { d.M7, d.M8, d.M9 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "Q4":
                    arrData = new string[] { d.M10, d.M11, d.M12 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "H1":
                    arrData = new string[] { d.M1, d.M2, d.M3, d.M4, d.M5, d.M6 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "H2":
                    arrData = new string[] { d.M7, d.M8, d.M9, d.M10, d.M11, d.M12 };
                    nReturn = EPIFunc.SumDataToDecimal(arrData);
                    break;
                case "FTotal":
                    nReturn = SystemFunction.GetDecimalNull(d.nTotal);
                    break;
                case "Q1Avg":
                    arrData = new string[] { d.M1, d.M2, d.M3 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "Q2Avg":
                    arrData = new string[] { d.M4, d.M5, d.M6 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "Q3Avg":
                    arrData = new string[] { d.M7, d.M8, d.M9 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "Q4Avg":
                    arrData = new string[] { d.M10, d.M11, d.M12 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "H1Avg":
                    arrData = new string[] { d.M1, d.M2, d.M3, d.M4, d.M5, d.M6 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "H2Avg":
                    arrData = new string[] { d.M7, d.M8, d.M9, d.M10, d.M11, d.M12 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "TotalAvg":
                    arrData = new string[] { d.M1, d.M2, d.M3, d.M4, d.M5, d.M6, d.M7, d.M8, d.M9, d.M10, d.M11, d.M12 };
                    nReturn = CaculateSumToDivAVG(arrData);
                    break;
                case "Q1AvgPerson":
                    arrData = new string[] { d.M1, d.M2, d.M3 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
                case "Q2AvgPerson":
                    arrData = new string[] { d.M4, d.M5, d.M6 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
                case "Q3AvgPerson":
                    arrData = new string[] { d.M7, d.M8, d.M9 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
                case "Q4AvgPerson":
                    arrData = new string[] { d.M10, d.M11, d.M12 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
                case "H1AvgPerson":
                    arrData = new string[] { d.M1, d.M2, d.M3, d.M4, d.M5, d.M6 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
                case "H2AvgPerson":
                    arrData = new string[] { d.M7, d.M8, d.M9, d.M10, d.M11, d.M12 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
                case "TotalAvgPerson":
                    arrData = new string[] { d.M1, d.M2, d.M3, d.M4, d.M5, d.M6, d.M7, d.M8, d.M9, d.M10, d.M11, d.M12 };
                    nReturn = ConvertValueCauseUnitPerson(CaculateSumToDivAVG(arrData));
                    break;
            }

        }

        return nReturn;
    }

    public static decimal? CaculateSumToDivAVG(string[] ArrData)
    {
        decimal? nReturn = null;
        int nDiv = 0;
        decimal? nSum = EPIFunc.SumDataToDecimal(ArrData);
        foreach (string value in ArrData)
        {
            if (SystemFunction.IsNumberic(value))
            {
                nDiv++;
            }
        }
        if (nDiv > 0)
        {
            nReturn = nSum / nDiv;
        }

        return nReturn;

    }

    public static int GetFunctionCalWaste(int nOperaID)
    {
        int nReturn = 0;
        switch (nOperaID)
        {
            case 16:
            case 1:
            case 2:
            case 7:
            case 18:
            case 17:
            case 15:
            case 5:
            case 6:
            case 9:
            case 10:
                nReturn = 1; //D1
                break;
            case 12: nReturn = 2; //D2
                break;
            case 13:
            case 3:
            case 8:
                nReturn = 3; //D3
                break;
            case 11:
                nReturn = 4; //D4
                break;
            case 14:
                nReturn = 5; //D5
                break;
            case 4:
                nReturn = 6; //D6
                break;
        }

        //อาจจะเกิดการเพิ่ม Operation type มาใหม่ให้อิงตามตัว Intensity
        if (nReturn == 0)
        {
            string sDisplayType = CommonFunction.Get_Value(SystemFunction.strConnect, "select DisplayType from TIntensity_DisplayInput where OperationTypeID = '" + nOperaID + "'");
            nReturn = SystemFunction.ParseInt(sDisplayType);
        }

        return nReturn;
    }

    public static decimal? EffluentConvertMGL2Ton(string sFlowrateVal, string sOperatinghoursVal, string sProductVal)//(Flow rate(m3/hour) * Operating hours (hour/month) * product(mg/l))/10^6(10^6 = 1,000,000)
    {
        decimal? nReturn = null;
        decimal? Flowrate = SystemFunction.GetDecimalNull(sFlowrateVal);
        decimal? Operatinghours = SystemFunction.GetDecimalNull(sOperatinghoursVal);
        decimal? ProductVal = SystemFunction.GetDecimalNull(sProductVal);
        nReturn = (Flowrate * Operatinghours * ProductVal) / 1000000;
        return nReturn;

    }

    /// <summary>
    /// ใช้สำหรับหาค่า  Emission concentration (at 7% O2 content)
    /// </summary>
    /// <param name="actuaPercentlO2">General Info : Actual %O2 content at measurement</param>
    /// <param name="actualO2Content">Option 2 : Emission concentration (at actual O2 content)</param>
    /// <returns>Option 2 : Emission concentration (at 7% O2 content)</returns>
    public static decimal? CalculateEmissionConcentration(decimal? actuaPercentlO2, decimal? actualO2Content)
    {
        decimal? result = null;

        if (actuaPercentlO2 != null && actualO2Content != null)
        {
            if (actuaPercentlO2.Value <= 20.9m)// Old Code from sshe 2.0 > actuaPercentlO2.Value != 20.9m)
            {
                result = actualO2Content.Value * ((20.9m - 7m) / (20.9m - actuaPercentlO2.Value));
            }
        }
        return result;
    }

    /// </summary>
    /// //ใช้สำหรับคำนวณหา Calculate Emission concentration to kg. >> convert data option 2 ให้หน่วยเป็น กิโลกรม เพื่อนำไปเข้าสูตรการหา Total หน่วยที่เป็น Ton ได้โดยไม่ต้องแปลงซ้ำอีกแล้ว
    /// <param name="actualO2">ค่า Emission concentration (at actual O2 content) </param>
    /// <param name="flowrate"> General Info - Flow Rate</param>
    /// <param name="operatingHour">General Info - Operating Hour</param>
    public static decimal? CalculateEmissionConcentration(int nProductID, string sactualO2, string sflowrate, string soperatingHour)
    {
        decimal? result = null;
        decimal? actualO2 = null;
        decimal? flowrate = null;
        decimal? operatingHour = null;
        actualO2 = SystemFunction.GetDecimalNull(sactualO2);
        flowrate = SystemFunction.GetDecimalNull(sflowrate);
        operatingHour = SystemFunction.GetDecimalNull(soperatingHour);

        if (actualO2 != null && flowrate != null && operatingHour != null)
        {
            switch (nProductID)
            {
                case 175://NOx
                    result = actualO2 * 46.01m / 24.45m * flowrate * operatingHour * 0.000001m;
                    break;
                case 178://SO2
                    result = actualO2 * 64.07m / 24.45m * flowrate * operatingHour * 0.000001m;
                    break;
                case 181://TSP
                    result = actualO2 * flowrate * operatingHour * 0.000001m;
                    break;
                case 184://CO
                    result = actualO2 * 28.01m / 24.45m * flowrate * operatingHour * 0.000001m;
                    break;
                case 187://Hg
                    result = actualO2 * flowrate * operatingHour * 0.000001m;
                    break;
                case 190://H2S
                    result = actualO2 * 34.082m / 24.45m * flowrate * operatingHour * 0.000001m;
                    break;
                default:
                    result = actualO2 * flowrate * operatingHour * 0.000001m;
                    break;
            }
        }
        return result;
    }

    /// <summary>
    /// ใช้สำหรับคำนวณหาค่า Total Emission (g/sec)
    /// </summary>
    /// <param name="TotalEmission">Total Emission</param>
    /// <param name="TotalOperating">Total Operating Hours</param>
    public static decimal? CalculateGrumPerSecond(string sTotalEmission, string sTotalOperating)
    {
        decimal? result = null;
        decimal? TotalEmission = SystemFunction.GetDecimalNull(sTotalEmission);
        decimal? TotalOperating = SystemFunction.GetDecimalNull(sTotalOperating);

        if (TotalEmission != null && TotalOperating != null && TotalOperating.Value > 0)
        {
            result = TotalEmission * 1000 * 1000 / (TotalOperating * 60m * 60m);
        }
        else if (TotalEmission != null && TotalOperating != null && TotalOperating.Value == 0)
        {
            result = 0;
        }
        return result;
    }

    /// <summary>
    /// Number of Spills
    /// แยกตาม PrimaryReasonID (Equipment failure,Corrosion,Operator or technical error,Third Party Damage,Unknown,Other (specify))
    /// และ sSpillType (HC = Hydrocarbon,NHC = Non-Hydrocarbon)
    /// โดย ระบุว่า  Spill to environment  จะนำมานับเสมอ
    /// </summary>
    /// <param name="lstDataSpill">volume unit is barrel</param>
    /// <param name="nSpillCaseID"> Primary Reasion</param>
    /// <param name="sSpillType">HC,NHC</param>
    /// <param name="nSpillOption">1 = Spill,2 = Significant Spill</param>
    /// <returns>จำนวณ</returns>
    public int? CountSpill(List<ClassExecute.TData_Spill> lstDataSpill, int nSpillCaseID, string sSpillType, int nSpillOption, int nMonth)
    {
        int nCount = 0;
        List<ClassExecute.TData_Spill> query = new List<ClassExecute.TData_Spill>();
        List<int> nSpillByID = nSpillCaseID == 18 ? new List<int>() { 32 } : new List<int>() { 31, 93 };//ในกรณีที่เป็น Third Party Damage คิดเฉพาะที่ spill by Transport ที่เหลือคิกจาก Prodction สำหรับ SpillBy ที่เป็น Other จะนับเหมือนกับ Production
        if (ModeValue + "" == "")
        {
            #region Normall Mode
            if (nSpillOption == 1)//Count volume 1-99 barrels และ จะต้องไม่เลือกเป็น Sensitive Area
            {
                query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && w.SpillDate.Value.Month == nMonth && nSpillByID.Contains(w.SpillByID))
                                                    && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y" /*|| w.Signification1ID == 33*/)).ToList();
                nCount = query.Count;
            }
            else if (nSpillOption == 2)//มากกว่าหรือเท่ากับ 100 หรือ sIsSensitiveArea = Y เป็น significant spill 
            {
                query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && w.SpillDate.Value.Month == nMonth && nSpillByID.Contains(w.SpillByID))
                                                    && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                nCount = query.Count;
            }
            #endregion
        }
        else
        {
            #region Get value by case

            switch (ModeValue)
            {
                case sysYTD://All Month
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nCount = query.Count;
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nCount = query.Count;
                    }
                    break;
                case sysH1://Month 1 - 6
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 1 && w.SpillDate.Value.Month <= 6))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nCount = query.Count;
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 1 && w.SpillDate.Value.Month <= 6))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nCount = query.Count;
                    }
                    break;
                case sysH2://Month 7 - 12
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 7 && w.SpillDate.Value.Month <= 12))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nCount = query.Count;
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 7 && w.SpillDate.Value.Month <= 12))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nCount = query.Count;
                    }
                    break;
                case sysQ1://Month 1 - 3
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 1 && w.SpillDate.Value.Month <= 3))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nCount = query.Count;
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 1 && w.SpillDate.Value.Month <= 3))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nCount = query.Count;
                    }
                    break;
                case sysQ2://Month 4 - 6
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 4 && w.SpillDate.Value.Month <= 6))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nCount = query.Count;
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 4 && w.SpillDate.Value.Month <= 6))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nCount = query.Count;
                    }
                    break;
                case sysQ3://Month 7 - 9
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 7 && w.SpillDate.Value.Month <= 9))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nCount = query.Count;
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 7 && w.SpillDate.Value.Month <= 9))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nCount = query.Count;
                    }
                    break;
                case sysQ4://Month 10 - 12
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 10 && w.SpillDate.Value.Month <= 12))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nCount = query.Count;
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 10 && w.SpillDate.Value.Month <= 12))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nCount = query.Count;
                    }
                    break;

                case sysRC:
                    #region Call from tab output on EPI Form
                    List<string> lstDataCal = new List<string>();
                    if (CheckRecalQ1)
                    {
                        if (nSpillOption == 1)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 1 && w.SpillDate.Value.Month <= 3))
                                                            && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        }
                        else if (nSpillOption == 2)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 1 && w.SpillDate.Value.Month <= 3))
                                                                               && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        }
                        nCount = nCount + query.Count;
                    }

                    if (CheckRecalQ2)
                    {
                        if (nSpillOption == 1)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 4 && w.SpillDate.Value.Month <= 6))
                                                            && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        }
                        else if (nSpillOption == 2)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 4 && w.SpillDate.Value.Month <= 6))
                                                                               && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        }
                        nCount = nCount + query.Count;
                    }

                    if (CheckRecalQ3)
                    {
                        if (nSpillOption == 1)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 7 && w.SpillDate.Value.Month <= 9))
                                                            && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        }
                        else if (nSpillOption == 2)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 7 && w.SpillDate.Value.Month <= 9))
                                                                               && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        }
                        nCount = nCount + query.Count;
                    }

                    if (CheckRecalQ4)
                    {
                        if (nSpillOption == 1)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 10 && w.SpillDate.Value.Month <= 12))
                                                            && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        }
                        else if (nSpillOption == 2)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 10 && w.SpillDate.Value.Month <= 12))
                                                                               && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        }
                        nCount = nCount + query.Count;
                    }
                    #endregion
                    break;
            }
            #endregion
        }


        if (nCount == 0)
        {
            if (lstDataSpill.Where(w => w.SpillDate != null).Count() == 0)//สำหรับกรณีที่ไม่ได้บันทึกข้อมูลมาเลย
            {
                return null;
            }
            else
            {
                return nCount;
            }
        }
        else
        {
            return nCount;
        }
    }

    /// <summary>
    /// Sum Spills
    /// แยกตาม PrimaryReasonID (Equipment failure,Corrosion,Operator or technical error,Third Party Damage,Unknown,Other (specify))
    /// และ sSpillType (HC = Hydrocarbon,NHC = Non-Hydrocarbon)
    /// โดย ระบุว่า  Spill to environment  จะนำมานับเสมอ
    /// </summary>
    /// <param name="lstDataSpill">volume unit is barrel</param>
    /// <param name="nSpillCaseID"> Primary Reasion</param>
    /// <param name="sSpillType">HC,NHC</param>
    /// <param name="sSpillOption">1 = Spill,2 = Significant Spill</param>
    /// <param name="nMonth"></param>
    /// <returns>ผลรวม หน่วยเป็น M3 </returns>
    public decimal? SumSpill(List<ClassExecute.TData_Spill> lstDataSpill, int nSpillCaseID, string sSpillType, int nSpillOption, int nMonth)
    {
        decimal? nRetrun = null;//ค่าตอนแรกจะเป็น Barrel
        decimal? nSumTemp = null;
        List<ClassExecute.TData_Spill> query = new List<ClassExecute.TData_Spill>();
        List<int> nSpillByID = nSpillCaseID == 18 ? new List<int>() { 32 } : new List<int>() { 31, 93 };//ในกรณีที่เป็น Third Party Damage คิดเฉพาะที่ spill by Transport ที่เหลือคิกจาก Prodction สำหรับ SpillBy ที่เป็น Other จะนับเหมือนกับ Production
        if (ModeValue + "" == "")
        {
            if (nSpillOption == 1)//Sum volume 1-99 barrels และ จะต้องไม่เลือกเป็น Sensitive Area
            {
                query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && w.SpillDate.Value.Month == nMonth && nSpillByID.Contains(w.SpillByID))
                                                    && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y" /*|| w.Signification1ID == 33*/)).ToList();
                nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
            }
            else if (nSpillOption == 2)//มากกว่าหรือเท่ากับ 100 หรือ sIsSensitiveArea = Y เป็น significant spill 
            {
                query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && w.SpillDate.Value.Month == nMonth && nSpillByID.Contains(w.SpillByID))
                                                    && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
            }
        }
        else
        {
            #region Get value by case

            switch (ModeValue)
            {
                case sysYTD://All Month
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    break;
                case sysH1://Month 1 - 6
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 1 && w.SpillDate.Value.Month <= 6))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 1 && w.SpillDate.Value.Month <= 6))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    break;
                case sysH2://Month 7 - 12
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 7 && w.SpillDate.Value.Month <= 12))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 7 && w.SpillDate.Value.Month <= 12))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    break;
                case sysQ1://Month 1 - 3
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 1 && w.SpillDate.Value.Month <= 3))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 1 && w.SpillDate.Value.Month <= 3))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    break;
                case sysQ2://Month 4 - 6
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 4 && w.SpillDate.Value.Month <= 6))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 4 && w.SpillDate.Value.Month <= 6))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    break;
                case sysQ3://Month 7 - 9
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 7 && w.SpillDate.Value.Month <= 9))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 7 && w.SpillDate.Value.Month <= 9))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    break;
                case sysQ4://Month 10 - 12
                    if (nSpillOption == 1)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 10 && w.SpillDate.Value.Month <= 12))
                                                        && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    else if (nSpillOption == 2)
                    {
                        query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 10 && w.SpillDate.Value.Month <= 12))
                                                                           && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        nRetrun = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                    }
                    break;

                case sysRC:
                    #region Call from tab output on EPI Form
                    List<string> lstDataCal = new List<string>();
                    if (CheckRecalQ1)
                    {
                        if (nSpillOption == 1)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 1 && w.SpillDate.Value.Month <= 3))
                                                            && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        }
                        else if (nSpillOption == 2)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 1 && w.SpillDate.Value.Month <= 3))
                                                                               && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        }

                        nSumTemp = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                        if (nSumTemp != null || nRetrun != null)
                        {
                            nRetrun = (nRetrun ?? 0) + (nSumTemp ?? 0);
                        }
                    }

                    if (CheckRecalQ2)
                    {
                        if (nSpillOption == 1)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 4 && w.SpillDate.Value.Month <= 6))
                                                            && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        }
                        else if (nSpillOption == 2)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 4 && w.SpillDate.Value.Month <= 6))
                                                                               && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        }

                        nSumTemp = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                        if (nSumTemp != null || nRetrun != null)
                        {
                            nRetrun = (nRetrun ?? 0) + (nSumTemp ?? 0);
                        }
                    }

                    if (CheckRecalQ3)
                    {
                        if (nSpillOption == 1)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 7 && w.SpillDate.Value.Month <= 9))
                                                            && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        }
                        else if (nSpillOption == 2)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 7 && w.SpillDate.Value.Month <= 9))
                                                                               && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        }

                        nSumTemp = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                        if (nSumTemp != null || nRetrun != null)
                        {
                            nRetrun = (nRetrun ?? 0) + (nSumTemp ?? 0);
                        }
                    }

                    if (CheckRecalQ4)
                    {
                        if (nSpillOption == 1)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 10 && w.SpillDate.Value.Month <= 12))
                                                            && ((w.nSpillVolume >= 1 && w.nSpillVolume < 100) && w.sIsSensitiveArea + "" != "Y")).ToList();
                        }
                        else if (nSpillOption == 2)
                        {
                            query = lstDataSpill.Where(w => (w.PrimaryReasonID == nSpillCaseID && w.SpillType == sSpillType && w.SpillDate != null && nSpillByID.Contains(w.SpillByID) && (w.SpillDate.Value.Month >= 10 && w.SpillDate.Value.Month <= 12))
                                                                               && (w.nSpillVolume >= 100 || w.sIsSensitiveArea == "Y")).ToList();
                        }

                        nSumTemp = EPIFunc.SumDataToDecimal(query.Select(s => s.nSpillVolume + "").ToList());
                        if (nSumTemp != null || nRetrun != null)
                        {
                            nRetrun = (nRetrun ?? 0) + (nSumTemp ?? 0);
                        }
                    }
                    #endregion
                    break;
            }
            #endregion
        }
        return EPIFunc.ConvertBarrelToM3(nRetrun + "");
    }

    private int GetUnitIDOfProduct(List<ClassExecute.TDataWaste> lstDataWaste, int nProductID)
    {
        var qUnit = lstDataWaste.Where(w => w.ProductID == nProductID).FirstOrDefault();
        int nUNITID = qUnit != null ? qUnit.UnitID ?? 0 : 0;
        return nUNITID;
    }

    //For EPI Report
    public List<ClassExecute.TDataOutput> CalculateWater(int nOperaID, int nFacID, string sYear, List<ClassExecute.TData_Water> qWater, List<ClassExecute.TData_Intensity> qIntensity)
    {
        int IDIndicator = 11;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            #region call funtion calculate
            Target = CalculateWater(item.ProductID, qWater, qIntensity, 0, "");
            M1 = CalculateWater(item.ProductID, qWater, qIntensity, 1, "");
            M2 = CalculateWater(item.ProductID, qWater, qIntensity, 2, "");
            M3 = CalculateWater(item.ProductID, qWater, qIntensity, 3, "");
            M4 = CalculateWater(item.ProductID, qWater, qIntensity, 4, "");
            M5 = CalculateWater(item.ProductID, qWater, qIntensity, 5, "");
            M6 = CalculateWater(item.ProductID, qWater, qIntensity, 6, "");
            M7 = CalculateWater(item.ProductID, qWater, qIntensity, 7, "");
            M8 = CalculateWater(item.ProductID, qWater, qIntensity, 8, "");
            M9 = CalculateWater(item.ProductID, qWater, qIntensity, 9, "");
            M10 = CalculateWater(item.ProductID, qWater, qIntensity, 10, "");
            M11 = CalculateWater(item.ProductID, qWater, qIntensity, 11, "");
            M12 = CalculateWater(item.ProductID, qWater, qIntensity, 12, "");
            //decimal?[] arrDataYTD = { M1, M2, M3, M4, M5, M6, M7, M8, M9, M10, M11, M12 };
            Total = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysYTD); //EPIFunc.SumDataToDecimal(arrDataYTD);

            /* decimal?[] arrDataQ1 = { M1, M2, M3 };
             decimal?[] arrDataQ2 = { M4, M5, M6 };
             decimal?[] arrDataQ3 = { M7, M8, M9 };
             decimal?[] arrDataQ4 = { M10, M11, M12 };
             */
            Q1 = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysQ1);
            Q2 = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysQ2);
            Q3 = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysQ3);
            Q4 = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysQ4);

            /*
            decimal?[] arrDataH1 = { Q1, Q2 };
            decimal?[] arrDataH2 = { Q3, Q4 };
            */
            H1 = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysH1);
            H2 = CalculateWater(item.ProductID, qWater, qIntensity, 13, sysH2);
            #endregion


            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                sYear = sYear,
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }
        return lstTempReturn;
    }

    public List<ClassExecute.TDataOutput> CalculateComplaint(int nOperaID, int nFacID, string sYear, List<ClassExecute.TData_Complaint_Product> qComplaint)
    {
        int IDIndicator = 1;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            #region call funtion calculate
            Target = CalculateComplaint(item.ProductID, qComplaint, 0, "");
            M1 = CalculateComplaint(item.ProductID, qComplaint, 1, "");
            M2 = CalculateComplaint(item.ProductID, qComplaint, 2, "");
            M3 = CalculateComplaint(item.ProductID, qComplaint, 3, "");
            M4 = CalculateComplaint(item.ProductID, qComplaint, 4, "");
            M5 = CalculateComplaint(item.ProductID, qComplaint, 5, "");
            M6 = CalculateComplaint(item.ProductID, qComplaint, 6, "");
            M7 = CalculateComplaint(item.ProductID, qComplaint, 7, "");
            M8 = CalculateComplaint(item.ProductID, qComplaint, 8, "");
            M9 = CalculateComplaint(item.ProductID, qComplaint, 9, "");
            M10 = CalculateComplaint(item.ProductID, qComplaint, 10, "");
            M11 = CalculateComplaint(item.ProductID, qComplaint, 11, "");
            M12 = CalculateComplaint(item.ProductID, qComplaint, 12, "");
            Total = CalculateComplaint(item.ProductID, qComplaint, 13, sysYTD); //EPIFunc.SumDataToDecimal(arrDataYTD);

            Q1 = CalculateComplaint(item.ProductID, qComplaint, 13, sysQ1);
            Q2 = CalculateComplaint(item.ProductID, qComplaint, 13, sysQ2);
            Q3 = CalculateComplaint(item.ProductID, qComplaint, 13, sysQ3);
            Q4 = CalculateComplaint(item.ProductID, qComplaint, 13, sysQ4);

            H1 = CalculateComplaint(item.ProductID, qComplaint, 13, sysH1);
            H2 = CalculateComplaint(item.ProductID, qComplaint, 13, sysH2);
            #endregion


            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                sYear = sYear,
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }

        return lstTempReturn;
    }

    public List<ClassExecute.TDataOutput> CalculateCompliance(int nOperaID, int nFacID, string sYear, List<ClassExecute.TData_Compliance_Product> qCompliance)
    {
        int IDIndicator = 2;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            #region call funtion calculate
            Target = CalculateCompliance(item.ProductID, qCompliance, 0, "");
            M1 = CalculateCompliance(item.ProductID, qCompliance, 1, "");
            M2 = CalculateCompliance(item.ProductID, qCompliance, 2, "");
            M3 = CalculateCompliance(item.ProductID, qCompliance, 3, "");
            M4 = CalculateCompliance(item.ProductID, qCompliance, 4, "");
            M5 = CalculateCompliance(item.ProductID, qCompliance, 5, "");
            M6 = CalculateCompliance(item.ProductID, qCompliance, 6, "");
            M7 = CalculateCompliance(item.ProductID, qCompliance, 7, "");
            M8 = CalculateCompliance(item.ProductID, qCompliance, 8, "");
            M9 = CalculateCompliance(item.ProductID, qCompliance, 9, "");
            M10 = CalculateCompliance(item.ProductID, qCompliance, 10, "");
            M11 = CalculateCompliance(item.ProductID, qCompliance, 11, "");
            M12 = CalculateCompliance(item.ProductID, qCompliance, 12, "");
            Total = CalculateCompliance(item.ProductID, qCompliance, 13, sysYTD); //EPIFunc.SumDataToDecimal(arrDataYTD);

            Q1 = CalculateCompliance(item.ProductID, qCompliance, 13, sysQ1);
            Q2 = CalculateCompliance(item.ProductID, qCompliance, 13, sysQ2);
            Q3 = CalculateCompliance(item.ProductID, qCompliance, 13, sysQ3);
            Q4 = CalculateCompliance(item.ProductID, qCompliance, 13, sysQ4);

            H1 = CalculateCompliance(item.ProductID, qCompliance, 13, sysH1);
            H2 = CalculateCompliance(item.ProductID, qCompliance, 13, sysH2);
            #endregion


            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                sYear = sYear,
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }

        return lstTempReturn;
    }

    public List<ClassExecute.TDataOutput> CalculateEffluent(int nOperaID, int nFacID, string sYear, List<ClassExecute.TData_Effluent> qEffluent, List<ClassExecute.TData_Intensity> qIntensity, List<ClassExecute.TData_Water> qWater)
    {
        int IDIndicator = 3;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            #region call funtion calculate
            Target = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 0, "");
            M1 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 1, "");
            M2 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 2, "");
            M3 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 3, "");
            M4 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 4, "");
            M5 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 5, "");
            M6 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 6, "");
            M7 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 7, "");
            M8 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 8, "");
            M9 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 9, "");
            M10 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 10, "");
            M11 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 11, "");
            M12 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 12, "");
            //decimal?[] arrDataYTD = { M1, M2, M3, M4, M5, M6, M7, M8, M9, M10, M11, M12 };
            Total = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysYTD);//EPIFunc.SumDataToDecimal(arrDataYTD);

            /*decimal?[] arrDataQ1 = { M1, M2, M3 };
            decimal?[] arrDataQ2 = { M4, M5, M6 };
            decimal?[] arrDataQ3 = { M7, M8, M9 };
            decimal?[] arrDataQ4 = { M10, M11, M12 };
            */
            Q1 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysQ1);
            Q2 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysQ2);
            Q3 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysQ3);
            Q4 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysQ4);

            //decimal?[] arrDataH1 = { Q1, Q2 };
            //decimal?[] arrDataH2 = { Q3, Q4 };
            H1 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysH1);
            H2 = CalculateEffulent(item.ProductID, qEffluent, qIntensity, qWater, 13, sysH2);
            #endregion


            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                sYear = sYear,
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }

        return lstTempReturn;

    }

    public List<ClassExecute.TDataOutput> CalculateEmission(int nOperaID, int nFacID, string sYear, List<ClassExecute.TData_Emission> qEmission, List<ClassExecute.TData_Intensity> qIntensity)
    {
        int IDIndicator = 4;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();
        List<ClassExecute.TData_ProductCalculate> qProductOutput = new List<ClassExecute.TData_ProductCalculate>();
        if (nOperaID == 2)//Modify 27.10.2015 >> เนื่องจาก product ออกมาไม่ครบกรณีที่ Fix ค่ามาเป็น 2 >> โดยดึกทุก product ไม่แยก operation type เนื่องจากตอนแสดงมีการตัด product อยู่แล้ว
        {
            string sql = @"select a.IDIndicator,a.ProductID,a.ProductName,a.nOrder,a.sUnit,a.sType,
                                    ISNULL(b.UnitID,0) 'UnitID',ISNULL(b.UnitName,a.sUnit) 'UnitName'
                                    from mTProductIndicatorOutput a
                                    inner join TUseProductOutput d on a.ProductID = d.ProductID
                                    left join mTUnit b on a.sUnit = b.UnitName
                                    where a.IDIndicator = 4
									group by  a.IDIndicator,a.ProductID,a.ProductName,a.nOrder,a.sUnit,a.sType,ISNULL(b.UnitID,0) ,ISNULL(b.UnitName,a.sUnit)
									order by a.ProductID";

            qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(sql).Select(s => new ClassExecute.TData_ProductCalculate
            {
                IDIndicator = s.IDIndicator,
                ProductID = s.ProductID,
                ProductName = s.ProductName,
                nOrder = s.nOrder,
                sUnit = s.sUnit,
                UnitID = s.UnitID,
                UnitName = s.UnitName
            }).ToList();
        }
        else
        {
            qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
            {
                IDIndicator = s.IDIndicator,
                ProductID = s.ProductID,
                ProductName = s.ProductName,
                nOrder = s.nOrder,
                sUnit = s.sUnit,
                UnitID = s.UnitID,
                UnitName = s.UnitName
            }).ToList();
        }

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            #region call funtion calculate
            Target = CalculateEmission(item.ProductID, qEmission, qIntensity, 0, "");
            M1 = CalculateEmission(item.ProductID, qEmission, qIntensity, 1, "");
            M2 = CalculateEmission(item.ProductID, qEmission, qIntensity, 2, "");
            M3 = CalculateEmission(item.ProductID, qEmission, qIntensity, 3, "");
            M4 = CalculateEmission(item.ProductID, qEmission, qIntensity, 4, "");
            M5 = CalculateEmission(item.ProductID, qEmission, qIntensity, 5, "");
            M6 = CalculateEmission(item.ProductID, qEmission, qIntensity, 6, "");
            M7 = CalculateEmission(item.ProductID, qEmission, qIntensity, 7, "");
            M8 = CalculateEmission(item.ProductID, qEmission, qIntensity, 8, "");
            M9 = CalculateEmission(item.ProductID, qEmission, qIntensity, 9, "");
            M10 = CalculateEmission(item.ProductID, qEmission, qIntensity, 10, "");
            M11 = CalculateEmission(item.ProductID, qEmission, qIntensity, 11, "");
            M12 = CalculateEmission(item.ProductID, qEmission, qIntensity, 12, "");

            Total = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysYTD);
            Q1 = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysQ1);
            Q2 = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysQ2);
            Q3 = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysQ3);
            Q4 = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysQ4);

            H1 = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysH1);
            H2 = CalculateEmission(item.ProductID, qEmission, qIntensity, 13, sysH2);
            #endregion


            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                sYear = sYear,
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }

        return lstTempReturn;

    }

    public List<ClassExecute.TDataOutput> CalculateMaterial(int nOperaID, int nFacID, string sYear, List<ClassExecute.TDataMaterial> qMaterial, List<ClassExecute.TData_Intensity> qIntensity)
    {
        int IDIndicator = 8;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).ToList();

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            #region call funtion calculate
            Target = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 0, "");
            M1 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 1, "");
            M2 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 2, "");
            M3 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 3, "");
            M4 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 4, "");
            M5 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 5, "");
            M6 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 6, "");
            M7 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 7, "");
            M8 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 8, "");
            M9 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 9, "");
            M10 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 10, "");
            M11 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 11, "");
            M12 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 12, "");
            //decimal?[] arrDataYTD = { M1, M2, M3, M4, M5, M6, M7, M8, M9, M10, M11, M12 };
            Total = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysYTD);//EPIFunc.SumDataToDecimal(arrDataYTD);

            /*decimal?[] arrDataQ1 = { M1, M2, M3 };
            decimal?[] arrDataQ2 = { M4, M5, M6 };
            decimal?[] arrDataQ3 = { M7, M8, M9 };
            decimal?[] arrDataQ4 = { M10, M11, M12 };
            */
            Q1 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysQ1);
            Q2 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysQ2);
            Q3 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysQ3);
            Q4 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysQ4);

            //decimal?[] arrDataH1 = { Q1, Q2 };
            //decimal?[] arrDataH2 = { Q3, Q4 };
            H1 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysH1);
            H2 = CalculateMaterial(item.ProductID, qMaterial, qIntensity, 13, sysH2);
            #endregion


            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                sYear = sYear,
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }

        return lstTempReturn;

    }

    public List<ClassExecute.TDataOutput> CalculateSpill(int nOperaID, int nFacID, string sYear, List<ClassExecute.TData_Spill_Product> qSpillProduct)
    {
        int IDIndicator = 9;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = db.Database.SqlQuery<ClassExecute.TData_ProductCalculate>(GetSQLQueryProductOutput(IDIndicator, nOperaID)).Select(s => new ClassExecute.TData_ProductCalculate
        {
            IDIndicator = s.IDIndicator,
            ProductID = s.ProductID,
            ProductName = s.ProductName,
            sType = s.sType,
            nOrder = s.nOrder,
            sUnit = s.sUnit,
            UnitID = s.UnitID,
            UnitName = s.UnitName
        }).Where(w => w.ProductID == 280 || w.ProductID == 281).ToList(); //take >> Spill to environment 

        var qSpill = new List<ClassExecute.TData_Spill>();

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;

        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;

            #region call funtion calculate
            Target = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 0, "");
            M1 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 1, "");
            M2 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 2, "");
            M3 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 3, "");
            M4 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 4, "");
            M5 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 5, "");
            M6 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 6, "");
            M7 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 7, "");
            M8 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 8, "");
            M9 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 9, "");
            M10 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 10, "");
            M11 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 11, "");
            M12 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 12, "");

            Total = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysYTD);
            Q1 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysQ1);
            Q2 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysQ2);
            Q3 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysQ3);
            Q4 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysQ4);

            H1 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysH1);
            H2 = CalculateSpill(item.ProductID, qSpill, qSpillProduct, item.sType, 13, sysH2);
            #endregion

            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                sYear = sYear,
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }
        return lstTempReturn;
    }

    /// <summary>
    /// นำ Product ที่ sType = R มาคำนวณและแสดงด้วย
    /// </summary>
    /// <param name="nOperaID"></param>
    /// <param name="nFacID"></param>
    /// <param name="sYear"></param>
    /// <param name="qWaste"></param>
    /// <param name="qIntensity"></param>
    /// <returns></returns>
    public List<ClassExecute.TDataOutput> CalculateWaste(int nOperaID, int nFacID, string sYear, List<ClassExecute.TDataWaste> qWaste, List<ClassExecute.TData_Intensity> qIntensity)
    {
        int IDIndicator = 10;
        List<ClassExecute.TDataOutput> lstTempReturn = new List<ClassExecute.TDataOutput>();

        var qProductOutput = new FunctionGetData().GetProductOutputToReport(IDIndicator, nOperaID);

        decimal? M1 = null, M2 = null, M3 = null, M4 = null, M5 = null, M6 = null, M7 = null, M8 = null, M9 = null, M10 = null, M11 = null, M12 = null, Target = null, Total = null;
        decimal? Q1 = null, Q2 = null, Q3 = null, Q4 = null, H1 = null, H2 = null;
        int nModeFunction = 0;
        nModeFunction = GetFunctionCalWaste(nOperaID);
        foreach (var item in qProductOutput)
        {
            M1 = null; M2 = null; M3 = null; M4 = null; M5 = null; M6 = null; M7 = null; M8 = null; M9 = null; M10 = null; M11 = null; M12 = null; Target = null; Total = null; Q1 = null; Q2 = null; Q3 = null; Q4 = null; H1 = null; H2 = null;
            if (nModeFunction == 1 || nModeFunction == 7)//nOperaID == 16 || nOperaID == 1 || nOperaID == 2 || nOperaID == 7 || nOperaID == 18 || nOperaID == 17 || nOperaID == 15 || nOperaID == 5 || nOperaID == 6 || nOperaID == 9 || nOperaID == 10) //D1
            {
                #region call funtion calculate
                Target = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 0, "");
                M1 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 1, "");
                M2 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 2, "");
                M3 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 3, "");
                M4 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 4, "");
                M5 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 5, "");
                M6 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 6, "");
                M7 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 7, "");
                M8 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 8, "");
                M9 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 9, "");
                M10 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 10, "");
                M11 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 11, "");
                M12 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 12, "");

                Total = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysYTD);
                Q1 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysQ1);
                Q2 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysQ2);
                Q3 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysQ3);
                Q4 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysQ4);
                H1 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysH1);
                H2 = CalculateWaste_D1(item.ProductID, qWaste, qIntensity, 13, sysH2);

                #endregion
            }
            else if (nModeFunction == 2)//nOperaID == 12) //D2
            {
                #region call funtion calculate
                Target = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 0, "");
                M1 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 1, "");
                M2 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 2, "");
                M3 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 3, "");
                M4 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 4, "");
                M5 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 5, "");
                M6 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 6, "");
                M7 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 7, "");
                M8 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 8, "");
                M9 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 9, "");
                M10 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 10, "");
                M11 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 11, "");
                M12 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 12, "");

                Total = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysYTD);
                Q1 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysQ1);
                Q2 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysQ2);
                Q3 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysQ3);
                Q4 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysQ4);
                H1 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysH1);
                H2 = CalculateWaste_D2(item.ProductID, qWaste, qIntensity, 13, sysH2);
                #endregion
            }
            else if (nModeFunction == 3)//nOperaID == 13 || nOperaID == 3 || nOperaID == 8) //D3
            {
                #region call funtion calculate
                Target = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 0, "");
                M1 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 1, "");
                M2 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 2, "");
                M3 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 3, "");
                M4 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 4, "");
                M5 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 5, "");
                M6 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 6, "");
                M7 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 7, "");
                M8 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 8, "");
                M9 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 9, "");
                M10 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 10, "");
                M11 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 11, "");
                M12 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 12, "");

                Total = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysYTD);
                Q1 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysQ1);
                Q2 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysQ2);
                Q3 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysQ3);
                Q4 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysQ4);
                H1 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysH1);
                H2 = CalculateWaste_D3(item.ProductID, qWaste, qIntensity, 13, sysH2);
                #endregion
            }
            else if (nModeFunction == 4)//nOperaID == 11) //D4
            {
                #region call funtion calculate
                Target = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 0, "");
                M1 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 1, "");
                M2 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 2, "");
                M3 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 3, "");
                M4 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 4, "");
                M5 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 5, "");
                M6 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 6, "");
                M7 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 7, "");
                M8 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 8, "");
                M9 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 9, "");
                M10 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 10, "");
                M11 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 11, "");
                M12 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 12, "");

                Total = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysYTD);
                Q1 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysQ1);
                Q2 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysQ2);
                Q3 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysQ3);
                Q4 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysQ4);
                H1 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysH1);
                H2 = CalculateWaste_D4(item.ProductID, qWaste, qIntensity, 13, sysH2);
                #endregion
            }
            else if (nModeFunction == 5)//nOperaID == 14) //D5
            {
                #region call funtion calculate
                Target = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 0, "");
                M1 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 1, "");
                M2 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 2, "");
                M3 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 3, "");
                M4 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 4, "");
                M5 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 5, "");
                M6 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 6, "");
                M7 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 7, "");
                M8 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 8, "");
                M9 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 9, "");
                M10 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 10, "");
                M11 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 11, "");
                M12 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 12, "");

                Total = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysYTD);
                Q1 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysQ1);
                Q2 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysQ2);
                Q3 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysQ3);
                Q4 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysQ4);
                H1 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysH1);
                H2 = CalculateWaste_D5(item.ProductID, qWaste, qIntensity, 13, sysH2);
                #endregion
            }
            else if (nModeFunction == 6)//nOperaID == 4) //D6
            {
                #region call funtion calculate
                Target = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 0, "");
                M1 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 1, "");
                M2 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 2, "");
                M3 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 3, "");
                M4 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 4, "");
                M5 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 5, "");
                M6 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 6, "");
                M7 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 7, "");
                M8 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 8, "");
                M9 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 9, "");
                M10 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 10, "");
                M11 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 11, "");
                M12 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 12, "");

                Total = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysYTD);
                Q1 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysQ1);
                Q2 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysQ2);
                Q3 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysQ3);
                Q4 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysQ4);
                H1 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysH1);
                H2 = CalculateWaste_D6(item.ProductID, qWaste, qIntensity, 13, sysH2);
                #endregion
            }

            #region add data to list for return
            lstTempReturn.Add(new ClassExecute.TDataOutput
            {
                sYear = sYear,
                IDIndicator = IDIndicator,
                OperationtypeID = nOperaID,
                FacilityID = nFacID,
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                nUnitID = item.UnitID,
                sUnit = item.sUnit,
                nOrder = item.nOrder,
                nM1 = M1,
                nM2 = M2,
                nM3 = M3,
                nM4 = M4,
                nM5 = M5,
                nM6 = M6,
                nM7 = M7,
                nM8 = M8,
                nM9 = M9,
                nM10 = M10,
                nM11 = M11,
                nM12 = M12,
                nTarget = Target,
                nTotal = Total,

                nQ1 = Q1,
                nQ2 = Q2,
                nQ3 = Q3,
                nQ4 = Q4,
                nH1 = H1,
                nH2 = H2
            });
            #endregion
        }

        return lstTempReturn;
    }
}