<%@ WebHandler Language="C#" Class="Fileuploader" %>

using System;
using System.Web;

public class Fileuploader : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        string Function = context.Request["funcname"] + "";
        string savetopath = context.Request["savetopath"] + "";
        string savetoname = context.Request["savetoname"] + "";
        string delonpath = context.Request["delpath"] + "";
        string delfilename = context.Request["delfilename"] + "";

        if (Function == "UPLOAD")
        {
            sysGlobalClass.FuncFileUpload.ItemData data = new sysGlobalClass.FuncFileUpload.ItemData();
            if (context.Request.Files.Count > 0)
            {
                HttpPostedFile file = null;
                string filepath = SystemFunction.FolderUploadFile + "/" + savetopath;
                string sFileName = "";
                string sSysFileName = savetoname;
                for (int i = 0; i < context.Request.Files.Count; i++)
                {
                    file = context.Request.Files[i];
                    if (file.ContentLength > 0)
                    {
                        sFileName = file.FileName;
                        string[] arrfilename = (sFileName + "").Split('.');

                        if (string.IsNullOrEmpty(sSysFileName))
                        {
                            for (int j = 0; j < (arrfilename.Length - 1); j++)
                            {
                                sSysFileName += arrfilename[j];
                            }
                            sSysFileName = sSysFileName + "_" + DateTime.Now.ToString("ddMMyyyyHHmmssff") + "." + arrfilename[arrfilename.Length - 1];
                        }
                        else
                        {
                            sSysFileName = sSysFileName + "." + arrfilename[arrfilename.Length - 1];
                        }

                        if (SystemFunction.CreateDirectory(filepath))
                        {
                            file.SaveAs(context.Server.MapPath("./" + filepath + sSysFileName));

                            data.ID = 0;
                            data.IsCompleted = true;
                            data.SaveToFileName = sSysFileName;
                            data.FileName = sFileName;
                            data.SaveToPath = filepath;
                            data.url = filepath + sSysFileName;
                            data.sDirectoryEncrypt = HttpContext.Current.Server.UrlEncode(STCrypt.Encrypt(filepath + sSysFileName));
                            data.IsNewFile = true;
                            data.IsNewChoose = true;
                            data.sDelete = "N";
                            string[] sDe = sFileName.Split('.');
                            string sDescription = "";
                            if (sDe.Length > 0)
                            {
                                for (int d = 0; d < sDe.Length; d++)
                                {
                                    if (d != (sDe.Length - 1))
                                    {
                                        sDescription += sDe[d].ToString();
                                    }

                                }
                            }
                            data.sDescription = sDescription;
                        }
                        else
                        {
                            data.IsCompleted = false;
                            data.sMsg = "Error: Cannot create directory !";
                        }
                    }
                }
            }
            context.Response.Expires = -1;
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write(new SystemFunction().Ob2Json(data));
            context.Response.End();
        }
        else if (Function == "DEL")
        {
            if (System.IO.File.Exists(context.Server.MapPath("./") + delonpath.Replace("/", "\\") + delfilename))
            {
                System.IO.File.Delete(context.Server.MapPath("./") + delonpath.Replace("/", "\\") + delfilename);
            }

            sysGlobalClass.FuncFileUpload.ItemData d = new sysGlobalClass.FuncFileUpload.ItemData();
            d.IsCompleted = true;

            context.Response.Expires = -1;
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write(new SystemFunction().Ob2Json(d));
            context.Response.End();
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// Structure for jquery
    /// </summary>
    public class DataFile
    {
        public string name { get; set; }
        /// <summary>
        /// unit Kb
        /// </summary>
        public decimal? size { get; set; }
        /// <summary>
        /// file type
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// for open file case not custom
        /// </summary>
        public string file { get; set; }
        /// <summary>
        /// for custom
        /// </summary>
        public sysGlobalClass.FuncFileUpload.ItemData data { get; set; }
    }


}