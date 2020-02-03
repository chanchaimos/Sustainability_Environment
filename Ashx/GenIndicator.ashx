<%@ WebHandler Language="C#" Class="GenIndicator" %>

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
public class GenIndicator : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
                bool IsSys = SystemFunction.IsSuperAdmin();
                int nUser = UserAcc.GetObjUser().nUserID;
                int nRoleID = UserAcc.GetObjUser().nRoleID;
                var lstUser = db.mTUser_FacilityPermission.Where(a => a.nUserID == nUser && a.nRoleID == nRoleID).ToList();
                var lstFlow = db.mTWorkFlow.Where(a => ((a.L1 == nUser && nRoleID == 3) || (a.L2 == nUser && nRoleID == 4))).ToList();
                var lstFacility = new List<T_mTFacility>();
                var lsInt = new List<T_mTIndicator>();
                if (lstIn.Count > 0)
                {
                    var lb_1 = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 1).ToList();
                    var lb_2 = db.mTFacility.Where(w => w.cActive == "Y" && w.cDel == "N" && w.nLevel == 2).ToList();

                    var lstFacitltyTemp = (from a in lb_1
                                           from b in lb_2.Where(w => w.nHeaderID == a.ID)
                                           select new T_mTFacility
                                           {
                                               ID = b.ID,
                                               Name = b.Name,
                                               OperationTypeID = b.OperationTypeID,
                                           }).ToList();

                    if (!IsSys)
                    {
                        var lstInUser = (from a in lstIn
                                         from b in lstUser.Where(w => w.nGroupIndicatorID == a.ID)
                                         select new T_mTIndicator
                                         {
                                             ID = a.ID,
                                             Indicator = a.Indicator,
                                             nOrder = a.nOrder,
                                             lstProIn = new List<T_mTProductIndicator>(),
                                         }).ToList();
                        var lstInFlow = (from a in lstIn
                                         from b in lstFlow.Where(w => w.IDIndicator == a.ID)
                                         select new T_mTIndicator
                                         {
                                             ID = a.ID,
                                             Indicator = a.Indicator,
                                             nOrder = a.nOrder,
                                             lstProIn = new List<T_mTProductIndicator>(),
                                         }).ToList();
                        var lstFaUser = (from a in lstFacitltyTemp.AsEnumerable()
                                         from b in lstUser.Where(w => w.nFacilityID == a.ID)
                                         select new T_mTFacility
                                         {
                                             ID = a.ID,
                                             Name = a.Name,
                                             OperationTypeID = a.OperationTypeID,
                                             nGroupIndicatorID = b.nGroupIndicatorID,
                                         }).ToList();
                        var lstFaFlow = (from a in lstFacitltyTemp.AsEnumerable()
                                         from b in lstFlow.Where(w => w.IDFac == a.ID).AsEnumerable()
                                         select new T_mTFacility
                                         {
                                             ID = a.ID,
                                             Name = a.Name,
                                             OperationTypeID = a.OperationTypeID,
                                             nGroupIndicatorID = b.IDIndicator,
                                         }).ToList();
                        var lstInGroup = lstInUser.Concat(lstInFlow).GroupBy(g => new { g.sPath, g.ID, g.Indicator, g.nOrder }).ToList();
                        var lstFacGroup = lstFaUser.Concat(lstFaFlow).OrderBy(x => x.Name).GroupBy(a => new { a.ID, a.Name, a.OperationTypeID, a.nGroupIndicatorID }).ToList();
                        o.lstIn = lstInGroup.Select(g => new T_mTIndicator
                        {

                            ID = g.Key.ID,
                            Indicator = g.Key.Indicator,
                            nOrder = g.Key.nOrder,
                            lstProIn = new List<T_mTProductIndicator>(),
                            sPath = SystemFunction.ReturnPath(g.Key.ID, 0, "", "", "")
                        }).ToList();
                        lstFacility = lstFacGroup.Select(a => new T_mTFacility
                        {
                            ID = a.Key.ID,
                            Name = a.Key.Name,
                            OperationTypeID = a.Key.OperationTypeID,
                            nGroupIndicatorID = a.Key.nGroupIndicatorID
                        }).ToList();
                    }
                    else
                    {
                        o.lstIn = lstIn.Select(s => new T_mTIndicator
                        {
                            ID = s.ID,
                            Indicator = s.Indicator,
                            nOrder = s.nOrder,
                            lstProIn = new List<T_mTProductIndicator>(),
                            sPath = SystemFunction.ReturnPath(s.ID, 0, "", "", ""),
                        }).ToList();


                        lstFacility = lstFacitltyTemp.Select(s => new T_mTFacility
                        {
                            ID = s.ID,
                            Name = s.Name,
                            OperationTypeID = s.OperationTypeID,
                            nGroupIndicatorID = 0,
                        }).OrderBy(order => order.Name).ToList();
                    }

                    var lstOperationType = db.mOperationType.Where(w => w.cManage == "N" && w.cDel == "N" && w.cActive == "Y").Select(s => new T_mOperationType
                    {
                        ID = s.ID,
                        Name = s.Name,
                        sCode = s.sCode
                    }).ToList();

                    var lstOTGroup = (from a in o.lstIn
                                      from b in lstOperationType
                                      from c in lstFacility.Where(w => w.OperationTypeID == b.ID)
                                      select new T_mOperationType
                                      {
                                          ID = b.ID,
                                          Name = b.Name,
                                          sCode = b.sCode,
                                          nIndicator = a.ID,
                                          sPath = SystemFunction.ReturnPath(a.ID, b.ID, "", "", "")
                                      }).ToList();
                    var lstOL = lstOTGroup.GroupBy(g => new { g.ID, g.Name, g.sCode, g.nIndicator, g.sPath }).ToList();
                    var lstOT = lstOL.Select(s => new T_mOperationType
                    {
                        ID = s.Key.ID,
                        Name = s.Key.Name,
                        sCode = s.Key.sCode,
                        nIndicator = s.Key.nIndicator,
                        sPath = s.Key.sPath
                    }).ToList();
                    var lst = new List<T_mTFacility>();


                    o.lstIn.ForEach(f =>
                    {
                        if (lstOperationType.Count > 0)
                        {

                            lstOT.Where(w => w.nIndicator == f.ID).ToList().ForEach(f2 =>
                            {
                                f2.lstFacility = (lstFacility.Where(w => w.OperationTypeID == f2.ID && f.ID == w.nGroupIndicatorID).Count() > 0) || (IsSys == true && lstFacility.Where(w => w.OperationTypeID == f2.ID).Count() > 0) ? IsSys == true ? lstFacility.Where(w => w.OperationTypeID == f2.ID).ToList() : lstFacility.Where(w => w.OperationTypeID == f2.ID && f.ID == w.nGroupIndicatorID).ToList() : new List<T_mTFacility>();
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