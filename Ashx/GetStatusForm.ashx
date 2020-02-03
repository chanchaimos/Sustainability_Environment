<%@ WebHandler Language="C#" Class="GetStatusForm" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sysExtension;
using System.Web.Script.Services;
using System.Web.Services;
using System.Configuration;

public class GetStatusForm : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Expires = -1;
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        SetPageLoadMaster o = new SetPageLoadMaster();
        PTTGC_EPIEntities db = new PTTGC_EPIEntities();
        try
        {
            if (!UserAcc.UserExpired())
            {
                var lstIn = db.mTIndicator.ToList();
                o.lstIn = new List<T_mTIndicator>();
                bool IsSys = UserAcc.GetObjUser().nUserID + "" == System.Configuration.ConfigurationManager.AppSettings["UserIDAdmin"];
                int nUser = UserAcc.GetObjUser().nUserID;
                int nRoleID = UserAcc.GetObjUser().nRoleID;
                var lstUser = db.mTUser_FacilityPermission.Where(a => a.nUserID == nUser && a.nRoleID == nRoleID).ToList();
                var lstFlow = db.mTWorkFlow.Where(a => a.L1 == nUser || a.L2 == nUser).ToList();
                if (lstIn.Count > 0)
                {
                    var lstOperationType = db.mOperationType.Where(w => w.cManage == "N" && w.cDel == "N" && w.cActive == "Y").Select(s => new T_mOperationType
                    {
                        ID = s.ID,
                        Name = s.Name,
                        sCode = s.sCode
                    }).ToList();


                    var lstFacility = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 2).Select(s => new T_mTFacility
                    {
                        ID = s.ID,
                        Name = s.Name,
                        OperationTypeID = s.OperationTypeID,
                    }).ToList();
                    var grouplst = new List<T_mTIndicator>();
                    if (IsSys)
                    {
                        grouplst = (from a in lstIn
                                    from b in lstOperationType
                                    from c in lstFacility.Where(w => w.OperationTypeID == b.ID)
                                    select new T_mTIndicator
                                    {
                                        ID = a.ID,
                                        ID_OperationType = b.ID,
                                        ID_Facilitye = c.ID
                                    }).ToList();
                    }
                    else
                    {

                        var qUserInfo = (from l in lstUser
                                         from lf in lstFacility.Where(w => w.ID == l.nFacilityID)
                                         select new
                                         {
                                             nFacilityID = l.nFacilityID,
                                             OperationTypeID = lf.OperationTypeID ?? 0,
                                             nGroupIndicatorID = l.nGroupIndicatorID,
                                             nPermission = l.nPermission ?? 0
                                         }).ToList();

                        var qUserWF = (from l in lstFlow
                                       from lf in lstFacility.Where(w => w.ID == l.IDFac)
                                       select new
                                       {
                                           nFacilityID = l.IDFac,
                                           OperationTypeID = lf.OperationTypeID ?? 0,
                                           nGroupIndicatorID = lf.nGroupIndicatorID,
                                           nPermission = l.L1 == nUser ? 1 : 2
                                       }).ToList();

                        grouplst = qUserInfo.Concat(qUserWF).GroupBy(g => new
                        {
                            g.nFacilityID,
                            g.OperationTypeID,
                            g.nGroupIndicatorID
                        }).Select(s => new T_mTIndicator
                        {
                            ID = s.Key.nFacilityID,
                            ID_OperationType = s.Key.OperationTypeID,
                            ID_Facilitye = s.Key.nGroupIndicatorID,
                            nPermission = s.Max(x => x.nPermission)
                        }).ToList();

                        grouplst = (from a in lstIn
                                    from b in lstOperationType
                                    from c in lstFacility.Where(w => w.OperationTypeID == b.ID)
                                    select new T_mTIndicator
                                    {
                                        ID = a.ID,
                                        ID_OperationType = b.ID,
                                        ID_Facilitye = c.ID
                                    }).ToList();
                    }

                    var glstIn = grouplst.GroupBy(g => g.ID).ToList();
                    var glstOT = grouplst.GroupBy(g => g.ID_OperationType).ToList();
                    var glstFT = grouplst.GroupBy(g => g.ID_Facilitye).ToList();
                    o.lstIn = (from s in lstIn
                               from b in glstIn.Where(w => w.Key == s.ID)
                               select new T_mTIndicator
                               {
                                   ID = s.ID,
                                   Indicator = s.Indicator,
                                   nOrder = s.nOrder,
                                   lstProIn = new List<T_mTProductIndicator>(),
                                   sPath = SystemFunction.ReturnPath(s.ID, 0, "", "", ""),
                               }).OrderBy(r => r.nOrder).ToList();
                    var lstOT = (from a in o.lstIn
                                 from b in lstOperationType
                                 from c in glstOT.Where(w => w.Key == b.ID)
                                 select new T_mOperationType
                                 {
                                     ID = b.ID,
                                     Name = b.Name,
                                     sCode = b.sCode,
                                     nIndicator = a.ID,
                                     sPath = SystemFunction.ReturnPath(a.ID, b.ID, "", "", "")
                                 }).ToList();

                    var lst = new List<T_mTFacility>();
                    if (IsSys)
                    {
                        lst = (from a in lstOT
                               from c in lstFacility.Where(w => w.OperationTypeID == a.ID)
                               select new T_mTFacility
                               {
                                   ID = c.ID,
                                   Name = c.Name,
                                   nGroupIndicatorID = a.nIndicator,
                                   OperationTypeID = c.OperationTypeID
                               }).ToList();
                    }
                    else
                    {
                        lst = (from a in lstOT
                               from c in lstFacility.Where(w => w.OperationTypeID == a.ID)
                               from d in lstUser.Where(w => w.nFacilityID == c.ID && w.nGroupIndicatorID == a.nIndicator)
                               select new T_mTFacility
                               {
                                   ID = c.ID,
                                   Name = c.Name,
                                   nGroupIndicatorID = a.nIndicator,
                                   OperationTypeID = c.OperationTypeID
                               }).ToList();
                    }

                    o.lstIn.ForEach(f =>
                    {
                        if (lstOperationType.Count > 0)
                        {

                            lstOT.Where(w => w.nIndicator == f.ID).ToList().ForEach(f2 =>
                            {
                                f2.lstFacility = lst.Where(w => w.OperationTypeID == f2.ID && w.nGroupIndicatorID == f.ID).Count() > 0 ? lst.Where(w => w.OperationTypeID == f2.ID && w.nGroupIndicatorID == f.ID).ToList() : new List<T_mTFacility>();
                            });
                            f.lstOperationType = lstOT.Where(w => w.nIndicator == f.ID).ToList();
                        }

                    });

                }
                o.sStatus = SystemFunction.process_Success;
                o.sMsg = "";
            }
            else
            {
                o.sStatus = SystemFunction.process_SessionExpired;
                o.sMsg = "Session Expired !!";
            }
        }
        catch (Exception ex)
        {
            o.sStatus = SystemFunction.process_Failed;
            o.sMsg = ex.ToString();
        }


        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = 2147483644 };
        string res = serializer.Serialize(o);//new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ob);

        context.Response.Write(res);
        context.Response.End();
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    public class SetPageLoadMaster
    {
        public List<T_mTIndicator> lstIn { get; set; }
        public string sMsg { get; set; }
        public string sStatus { get; set; }
    }
    public class T_mTIndicator
    {
        public int ID { get; set; }
        public int ID_OperationType { get; set; }
        public int ID_Facilitye { get; set; }
        public string Indicator { get; set; }
        public List<T_mTProductIndicator> lstProIn { get; set; }
        public List<T_mOperationType> lstOperationType { get; set; }
        public int? nOrder { get; set; }
        public string sPath { get; set; }
        public int nPermission { get; set; }
    }
    public class T_mTProductIndicator
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int? nOrder { get; set; }
        public int? IDIndicator { get; set; }
    }
    public class T_mOperationType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string sCode { get; set; }
        public string sPath { get; set; }
        public List<T_mTFacility> lstFacility { get; set; }
        public int nIndicator { get; set; }
    }
    public class T_mTFacility
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? OperationTypeID { get; set; }
        public bool IsActive { get; set; }
        public int nGroupIndicatorID { get; set; }

    }


}