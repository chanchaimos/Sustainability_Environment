<%@ WebHandler Language="C#" Class="Deviate" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;

public class Deviate : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public T DeserializeJSON<T>(string jsonData)
    {
        var serializer = new JavaScriptSerializer() { MaxJsonLength = 2147483644 };

        // Converts the specified JSON string to an object of type T
        return (T)serializer.Deserialize<T>(jsonData);
    }

    public void ProcessRequest(HttpContext context)
    {

        string sFuncName = context.Request["funcName"] + "";
        if (sFuncName == "check")
        {
            var lstData = context.Request.Form["lstData"];
            CData data = this.DeserializeJSON<CData>(lstData);
            context.Response.Expires = -1;
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write(new SystemFunction().Ob2Json(CheckDeviate(data)));
            context.Response.End();
        }
        else if (sFuncName == "save")
        {
            var lstData = context.Request.Form["lstProductDeviate"];
            List<CRetrnPrd> data = this.DeserializeJSON<List<CRetrnPrd>>(lstData);
            context.Response.Expires = -1;
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write(new SystemFunction().Ob2Json(SaveDeviate(data)));
            context.Response.End();
        }
    }
    public sysGlobalClass.CResutlWebMethod SaveDeviate(List<CRetrnPrd> data)
    {
        sysGlobalClass.CResutlWebMethod result = new sysGlobalClass.CResutlWebMethod();
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            DateTime dAction = DateTime.Now;
            int nActionBy = UserAcc.GetObjUser().nUserID;
            foreach (var item in data)
            {
                int[] arrMonth = item.lstMonth.Select(s => s.nMonth).ToArray();
                db.TEPI_Forms_Deviate.RemoveRange(db.TEPI_Forms_Deviate.Where(w => w.FormID == item.FormID && w.nProductID == item.ProductID && arrMonth.Contains(w.Month)));
                db.SaveChanges();
                foreach (var itemMonth in item.lstMonth)
                {
                    TEPI_Forms_Deviate deviate = new TEPI_Forms_Deviate();
                    deviate.FormID = item.FormID;
                    deviate.Month = itemMonth.nMonth;
                    deviate.nProductID = item.ProductID.Value;
                    deviate.sRemark = itemMonth.sRemark;
                    deviate.nActionBy = nActionBy;
                    deviate.dAction = dAction;
                    db.TEPI_Forms_Deviate.Add(deviate);
                }
                db.SaveChanges();
            }
            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }
    public CReturnDeviate CheckDeviate(CData data)
    {
        CReturnDeviate result = new CReturnDeviate();
        result.lstProduct = new List<CRetrnPrd>();
        result.IsPass = true;
        if (!UserAcc.UserExpired())
        {
            PTTGC_EPIEntities db = new PTTGC_EPIEntities();
            Func<int?, string> getProductName = (ProducID) =>
            {
                string sProductID = "";
                if (ProducID.HasValue)
                {
                    var item = db.mTProductIndicator.FirstOrDefault(w => w.ProductID == ProducID);
                    if (item != null)
                    {
                        sProductID = item.ProductName;
                    }
                }
                return sProductID;
            };
            Func<int, int, int, string, int> FindFormID = (nIncID, nOprtID, nFacID, sYear) =>
            {
                int FormID = 0;
                var item = db.TEPI_Forms.FirstOrDefault(w => w.IDIndicator == nIncID && w.OperationTypeID == nOprtID && w.FacilityID == nFacID && w.sYear == sYear);
                if (item != null)
                {
                    FormID = item.FormID;
                }
                return FormID;
            };
            Func<int, int, int, int, string> getRemark = (FromID, nMonth, ProductID, nIncID) =>
            {
                string sRemark = "";
                var item = db.TEPI_Forms_Deviate.FirstOrDefault(w => w.FormID == FromID && w.Month == nMonth && (ProductID == 209 || ProductID == 210 ? (w.nProductID == 209 || w.nProductID == 210) : w.nProductID == ProductID));
                if (item != null)
                {
                    sRemark = item.sRemark;
                }
                //if (string.IsNullOrEmpty(sRemark) && nIncID == 10)
                //{
                //    if (ProductID == 2 || ProductID == 17)
                //    {
                //        sRemark += "- โปรดระบุ ปริมาณ waste ที่เกิดจากกิจกรรมซ่อมบำรุงตามแผน (routine)";
                //    }
                //    if (ProductID == 8 || ProductID == 24)
                //    {
                //        sRemark += (sRemark != "" ? "<br/> " : "") + "- โปรดระบุ ปริมาณ waste ที่เกิดจากกิจกรรม Unplan shutdown, spill, contruction  (non-routine)";
                //    }
                //}
                return sRemark;
            };
            int CurrentFromID = FindFormID(data.nIncID, data.nOprtID, data.nFacID, data.sYear);
            bool IsJanPass = true, IsFebPass = true, IsMarPass = true, IsAprPass = true, IsMayPass = true, IsJunPass = true, IsJulPass = true, IsAugPass = true, IsSepPass = true, IsOctPass = true, IsNovPass = true, IsDecPass = true;

            int nLastYear = int.Parse(data.sYear) - 1;
            string sLastYear = nLastYear + "";
            var FORM_EPI = db.TEPI_Forms.FirstOrDefault(w => w.IDIndicator == data.nIncID && w.OperationTypeID == data.nOprtID && w.FacilityID == data.nFacID && w.sYear == sLastYear);

            List<CProduct> lstDataProduct = new List<CProduct>();
            result.lstMonthDeviate = new List<int>();
            if (FORM_EPI != null)
            {
                switch (data.nIncID)
                {
                    case 1://Complaint
                        lstDataProduct = db.TComplaint_Product.Where(w => w.FormID == FORM_EPI.FormID).Select(s => new CProduct
                        {
                            ProductID = s.ProductID,
                            M12 = s.M12,
                        }).ToList();
                        break;
                    case 2://Compliance
                        lstDataProduct = db.TCompliance_Product.Where(w => w.FormID == FORM_EPI.FormID).Select(s => new CProduct
                        {
                            ProductID = s.ProductID,
                            M12 = s.M12,
                        }).ToList();
                        break;
                    case 3://Effluent
                        lstDataProduct = db.TEffluent_Product.Where(w => w.FormID == FORM_EPI.FormID).Select(s => new CProduct
                        {
                            ProductID = s.ProductID,
                            M12 = s.M12,
                        }).ToList();
                        break;
                    case 4://Emission
                        lstDataProduct = db.TEmission_Product.Where(w => w.FormID == FORM_EPI.FormID).Select(s => new CProduct
                        {
                            ProductID = s.ProductID,
                            M12 = s.M12,
                        }).ToList();
                        var lstVOC = db.TEmission_VOC.Where(w => w.FormID == FORM_EPI.FormID && w.ProductID == 193).Select(s => new CProduct
                        {
                            ProductID = s.ProductID,
                            M12 = s.M12,
                        }).ToList();
                        lstDataProduct.AddRange(lstVOC);
                        
                        break;
                    case 6://Intensity
                        lstDataProduct = db.TIntensityDominator.Where(w => w.FormID == FORM_EPI.FormID).Select(s => new CProduct
                        {
                            ProductID = s.ProductID,
                            M12 = s.M12,
                        }).ToList();
                        break;
                    case 8://Material
                        lstDataProduct = db.TMaterial_Product.Where(w => w.FormID == FORM_EPI.FormID).Select(s => new CProduct
                        {
                            ProductID = s.ProductID,
                            M12 = s.M12,
                        }).ToList();
                        break;
                    case 9://Spill
                        lstDataProduct = db.TSpill_Product.Where(w => w.FormID == FORM_EPI.FormID).Select(s => new CProduct
                        {
                            ProductID = s.ProductID,
                            M12 = s.M12,
                        }).ToList();
                        break;
                    case 10://Waste
                        lstDataProduct = db.TWaste_Product.Where(w => w.FormID == FORM_EPI.FormID).Select(s => new CProduct
                        {
                            ProductID = s.ProductID,
                            M12 = s.M12,
                        }).ToList();
                        break;
                    case 11://Water
                        lstDataProduct = db.TWater_Product.Where(w => w.FormID == FORM_EPI.FormID).Select(s => new CProduct
                        {
                            ProductID = s.ProductID,
                            M12 = s.M12,
                        }).ToList();
                        break;
                }
            }
            for (int i = 0; i < data.lstProduct.Count(); i++)
            {
                List<int> lstMonthDeviate = new List<int>();
                decimal? nValueLastYear = null;
                if (FORM_EPI != null)
                {
                    nValueLastYear = SystemFunction.GetDecimalNull(lstDataProduct.FirstOrDefault(w => w.ProductID == data.lstProduct[i].ProductID).M12);
                }
                decimal?[] lstDataM = new decimal?[] { 
                    nValueLastYear
                    , SystemFunction.GetDecimalNull(data.lstProduct[i].M1)
                    , SystemFunction.GetDecimalNull(data.lstProduct[i].M2)
                    , SystemFunction.GetDecimalNull(data.lstProduct[i].M3)
                    , SystemFunction.GetDecimalNull(data.lstProduct[i].M4)
                    , SystemFunction.GetDecimalNull(data.lstProduct[i].M5)
                    , SystemFunction.GetDecimalNull(data.lstProduct[i].M6)
                    , SystemFunction.GetDecimalNull(data.lstProduct[i].M7)
                    , SystemFunction.GetDecimalNull(data.lstProduct[i].M8)
                    , SystemFunction.GetDecimalNull(data.lstProduct[i].M9)
                    , SystemFunction.GetDecimalNull(data.lstProduct[i].M10)
                    , SystemFunction.GetDecimalNull(data.lstProduct[i].M11)
                    , SystemFunction.GetDecimalNull(data.lstProduct[i].M12) };

                for (int j = 0; j < lstDataM.Length; j++)
                {
                    if (j != 0)
                    {
                        if (j == 1)
                        {
                            if (nValueLastYear.HasValue)
                            {
                                decimal? n10Per = (lstDataM[j - 1] * 10) / 100;
                                decimal? nValPre = lstDataM[j - 1];
                                decimal? nValCur = lstDataM[j];
                                decimal? nCal = 0;

                                if (nValPre.HasValue)// ถ้าเดือนก่อนหน้าไม่มีค่า จะไม่เช็ค Deviate
                                {
                                    if (nValCur != 0 || nValPre != 0)
                                    {
                                        if (nValCur > nValPre)
                                        {
                                            nCal = nValCur - nValPre;
                                        }
                                        else
                                        {
                                            nCal = nValPre - nValCur;
                                        }
                                        if (nCal >= n10Per)
                                        {
                                            lstMonthDeviate.Add(j);
                                            switch (j)
                                            {
                                                case 1:
                                                    IsJanPass = false;
                                                    break;
                                                case 2:
                                                    IsFebPass = false;
                                                    break;
                                                case 3:
                                                    IsMarPass = false;
                                                    break;
                                                case 4:
                                                    IsAprPass = false;
                                                    break;
                                                case 5:
                                                    IsMayPass = false;
                                                    break;
                                                case 6:
                                                    IsJunPass = false;
                                                    break;
                                                case 7:
                                                    IsJulPass = false;
                                                    break;
                                                case 8:
                                                    IsAugPass = false;
                                                    break;
                                                case 9:
                                                    IsSepPass = false;
                                                    break;
                                                case 10:
                                                    IsOctPass = false;
                                                    break;
                                                case 11:
                                                    IsNovPass = false;
                                                    break;
                                                case 12:
                                                    IsDecPass = false;
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            decimal? n10Per = (lstDataM[j - 1] * 10) / 100;
                            decimal? nValPre = lstDataM[j - 1];
                            decimal? nValCur = lstDataM[j];
                            decimal? nCal = 0;
                            if (nValPre.HasValue)// ถ้าเดือนก่อนหน้าไม่มีค่า  จะไม่เช็ค Deviate
                            {
                                if (nValCur != 0 || nValPre != 0)
                                {
                                    if (nValCur > nValPre)
                                    {
                                        nCal = nValCur - nValPre;
                                    }
                                    else
                                    {
                                        nCal = nValPre - nValCur;
                                    }
                                    if (nCal >= n10Per)
                                    {
                                        lstMonthDeviate.Add(j);
                                        switch (j)
                                        {
                                            case 1:
                                                IsJanPass = false;
                                                break;
                                            case 2:
                                                IsFebPass = false;
                                                break;
                                            case 3:
                                                IsMarPass = false;
                                                break;
                                            case 4:
                                                IsAprPass = false;
                                                break;
                                            case 5:
                                                IsMayPass = false;
                                                break;
                                            case 6:
                                                IsJunPass = false;
                                                break;
                                            case 7:
                                                IsJulPass = false;
                                                break;
                                            case 8:
                                                IsAugPass = false;
                                                break;
                                            case 9:
                                                IsSepPass = false;
                                                break;
                                            case 10:
                                                IsOctPass = false;
                                                break;
                                            case 11:
                                                IsNovPass = false;
                                                break;
                                            case 12:
                                                IsDecPass = false;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (lstMonthDeviate.Count() > 0)
                {
                    List<CRemark> lstRemark = new List<CRemark>();
                    foreach (var item in lstMonthDeviate)
                    {
                        lstRemark.Add(new CRemark
                        {
                            nMonth = item,
                            sRemark = getRemark(CurrentFromID, item, data.lstProduct[i].ProductID.Value, data.nIncID),
                        });
                    }
                    result.lstProduct.Add(new CRetrnPrd
                    {
                        FormID = CurrentFromID,
                        ProductID = data.lstProduct[i].ProductID,
                        ProductName = data.lstProduct[i].ProductName,
                        //ProductName = getProductName(data.lstProduct[i].ProductID),
                        lstMonth = lstRemark
                    });
                }
            }
            //result.IsPass = IsJanPass && IsFebPass && IsMarPass && IsAprPass && IsMayPass && IsJunPass && IsJulPass && IsAugPass && IsSepPass && IsOctPass && IsNovPass && IsDecPass;
            bool[] lstCheck = new bool[] { IsJanPass, IsFebPass, IsMarPass, IsAprPass, IsMayPass, IsJunPass, IsJulPass, IsAugPass, IsSepPass, IsOctPass, IsNovPass, IsDecPass };
            for (int i = 0; i < lstCheck.Length; i++)
            {
                if (!lstCheck[i])
                {
                    if (data.arrMonth.Contains(i + 1))
                    {
                        result.lstMonthDeviate.Add(i + 1);
                    }
                }
                else
                {
                    if (data.arrMonth.Contains(i + 1))
                    {
                        var item = db.TEPI_Forms_Deviate.Where(w => w.FormID == CurrentFromID && w.Month == (i + 1)).ToList();
                        if (item != null)
                        {
                            db.TEPI_Forms_Deviate.RemoveRange(item);
                        }
                    }
                }
            }
            db.SaveChanges();
            result.IsPass = result.lstMonthDeviate.Count() > 0 ? false : true;
            result.Status = SystemFunction.process_Success;
        }
        else
        {
            result.Status = SystemFunction.process_SessionExpired;
        }
        return result;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    #region Class
    [Serializable]
    public class CReturnDeviate : sysGlobalClass.CResutlWebMethod
    {
        public List<CRetrnPrd> lstProduct { get; set; }
        public bool IsPass { get; set; }
        public List<int> lstMonthDeviate { get; set; }
    }
    [Serializable]
    public class CData
    {
        public List<CProduct> lstProduct { get; set; }
        public List<int> arrMonth { get; set; }
        public int nIncID { get; set; }
        public int nOprtID { get; set; }
        public int nFacID { get; set; }
        public string sYear { get; set; }
    }
    [Serializable]
    public class CProduct
    {
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
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
    }
    [Serializable]
    public class CRetrnPrd
    {
        public int FormID { get; set; }
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
        public List<CRemark> lstMonth { get; set; }
    }
    [Serializable]
    public class CRemark
    {
        public int nMonth { get; set; }
        public string sRemark { get; set; }
    }
    [Serializable]
    public class CAllGroupInc : sysGlobalClass.TData_Intensity
    {

    }
    #endregion
}