using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;

using System.Web.Mail;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Configuration;

/// <summary>
/// ฟังก์ชันทั่วๆไป สามารถใช้กับระบบอื่นๆได้
/// </summary>
public class CommonFunction
{
    public static DataTable LinqToDataTable<T>(IEnumerable<T> Data)
    {
        DataTable dtReturn = new DataTable();
        if (Data.ToList().Count == 0) return null;
        // Could add a check to verify that there is an element 0

        T TopRec = Data.ElementAt(0);

        // Use reflection to get property names, to create table

        // column names

        PropertyInfo[] oProps =
        ((Type)TopRec.GetType()).GetProperties();

        foreach (PropertyInfo pi in oProps)
        {

            Type colType = pi.PropertyType; if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {

                colType = colType.GetGenericArguments()[0];

            }

            dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
        }

        foreach (T rec in Data)
        {

            DataRow dr = dtReturn.NewRow(); foreach (PropertyInfo pi in oProps)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
            }
            dtReturn.Rows.Add(dr);

        }
        return dtReturn;
    }

    public static T CreateItem<T>(DataRow row)
    {
        string columnName;
        T obj = default(T);
        if (row != null)
        {
            //Create the instance of type T
            obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in row.Table.Columns)
            {
                columnName = column.ColumnName;
                //Get property with same columnName
                PropertyInfo prop = obj.GetType().GetProperty(columnName);
                try
                {
                    //Get value for the column
                    object value = (row[columnName].GetType() == typeof(DBNull)) ? null : row[columnName];
                    //Set property value
                    prop.SetValue(obj, value, null);
                }
                catch
                {
                    throw;
                    //Catch whatever here
                }
            }
        }
        return obj;
    }

    public static string Get_Value(string Conn, string _sql)
    {
        using (SqlConnection _conn = new SqlConnection(Conn))
        {
            DataTable _dt = new DataTable();
            _conn.Open();
            new SqlDataAdapter(_sql, _conn).Fill(_dt);
            if (_dt.Rows.Count >= 1)
            {
                string _return = _dt.Rows[0][0].ToString();
                _dt.Dispose();
                return _return;
            }
            return "";
        }
    }

    public static string Get_Value(SqlConnection _conn, string _sql)
    {
        DataTable _dt = new DataTable();
        new SqlDataAdapter(_sql, _conn).Fill(_dt);
        if (_dt.Rows.Count >= 1)
        {
            string _return = _dt.Rows[0][0].ToString();
            _dt.Dispose();
            return _return;
        }
        return "";
    }

    public static DataTable Get_Data(string Conn, string _sql)
    {
        using (SqlConnection _conn = new SqlConnection(Conn))
        {
            DataTable _dt = new DataTable();
            _conn.Open();
            new SqlDataAdapter(_sql, _conn).Fill(_dt);
            _conn.Close();
            return _dt;
        }
    }

    public static DataTable Get_Data(SqlConnection Conn, string _sql)
    {
        try
        {
            DataTable _dt = new DataTable();
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
            new SqlDataAdapter(_sql, Conn).Fill(_dt);
            return _dt;
        }
        finally
        {
            Conn.Close();
        }

    }

    public static int Count_Value(string Conn, string _sql)
    {
        using (SqlConnection _conn = new SqlConnection(Conn))
        {
            DataTable _dt = new DataTable();
            _conn.Open();
            new SqlDataAdapter(_sql, _conn).Fill(_dt);
            if (_dt.Rows.Count > 0)
            {
                int _return = _dt.Rows.Count;
                _dt.Dispose();
                return _return;
            }
            return 0;
        }

    }

    public static int Count_Value(SqlConnection Conn, string _sql)
    {

        DataTable _dt = new DataTable();
        if (Conn.State == ConnectionState.Closed) Conn.Open();
        new SqlDataAdapter(_sql, Conn).Fill(_dt);
        if (_dt.Rows.Count > 0)
        {
            int _return = _dt.Rows.Count;
            _dt.Dispose();
            return _return;
        }
        return 0;

    }

    public static string Gen_ID(string Conn, string _sql)
    {
        using (SqlConnection _conn = new SqlConnection(Conn))
        {
            string sReturn = "";
            DataTable _dt = new DataTable();
            _conn.Open();
            new SqlDataAdapter(_sql, _conn).Fill(_dt);

            if (_dt.Rows.Count >= 1)
            {
                sReturn = "" + (Convert.ToDecimal(_dt.Rows[0][0]) + 1);
            }
            else
            {
                sReturn = "1";
            }
            _dt.Dispose();
            return sReturn;
        }
    }

    public static string Gen_ID(SqlConnection Conn, string _sql)
    {
        if (Conn.State == ConnectionState.Closed) Conn.Open();
        string sReturn = "";
        DataTable _dt = new DataTable();

        new SqlDataAdapter(_sql, Conn).Fill(_dt);

        if (_dt.Rows.Count >= 1)
        {
            sReturn = "" + (Convert.ToDecimal(_dt.Rows[0][0]) + 1);
        }
        else
        {
            sReturn = "1";
        }
        _dt.Dispose();
        return sReturn;
    }

    public static sbyte FindIndexColumnOfDataFieldInGrid(DataGrid _dgd, string _fieldname)
    {
        sbyte _i = 0; bool _c = false;
        for (; _i < _dgd.Columns.Count; _i++)
        {
            if (_dgd.Columns[_i].GetType().ToString().Equals("System.Web.UI.WebControls.TemplateColumn")) continue;
            //
            if (((BoundColumn)_dgd.Columns[_i]).DataField.Equals(_fieldname)) { _c = true; break; }
        }
        if (!_c) _i = -1;
        return _i;
    }
    // Anti SQL Injection
    public static string ReplaceInjection(string str)
    {
        string[] _blacklist = new string[] { "'", "\\", "\"", "*/", ";", "--", "<script", "/*", "</script>" };
        string strRep = str;
        if (strRep == null || strRep.Trim().Equals(String.Empty))
            return strRep;
        foreach (string _blk in _blacklist) { strRep = strRep.Replace(_blk, ""); }

        return strRep;
    }

    public static bool SendMail(string sfrom, string sto, string subject, string message, string sfilepath)
    {
        try
        {
            MailMessage oMsg = new MailMessage();

            // TODO: Replace with sender e-mail address. 
            oMsg.From = sfrom;

            // TODO: Replace with recipient e-mail address.
            oMsg.To = sto;
            oMsg.Subject = subject;

            // SEND IN HTML FORMAT (comment this line to send plain text).
            oMsg.BodyFormat = MailFormat.Html;

            // HTML Body (remove HTML tags for plain text).
            oMsg.Body = "<HTML><BODY>" + message + "</BODY></HTML>";

            // ADD AN ATTACHMENT.
            // TODO: Replace with path to attachment.
            // String sFile = @"D:\FTP\username\Htdocs\Hello.txt";
            // String sFile = @sfilepath;
            // MailAttachment oAttch = new MailAttachment(sfilepath, MailEncoding.Base64);
            // oMsg.Attachments.Add(oAttch);

            // TODO: Replace with the name of your remote SMTP server.
            SmtpMail.SmtpServer = ConfigurationManager.AppSettings["smtpmail"];
            SmtpMail.Send(oMsg);

            oMsg = null;
            return true;
            //oAttch = null;
        }
        catch
        {
            return false;
        }
    }

    public static DataTable ArrayDataRowToDataTable(DataTable _dt, DataRow[] _ardr)
    {
        DataTable dt = _dt.Copy();
        dt.Rows.Clear();
        foreach (DataRow _dr in _ardr) dt.ImportRow(_dr);
        return dt;
    }

    public static string chkPermission(string session, string idpage)
    {
        return "";
    }

    public static string Gen_IDString(string _wwcode, int _length)
    {
        //formate yy-MMxxx
        if (string.IsNullOrEmpty(_wwcode))
        {
            return string.Empty;
        }

        string sReturn = "";
        int length = _length;

        int id = Convert.ToInt32(_wwcode.Substring(4, _length));

        string yy = DateTime.Now.ToString("yy");

        string mm = DateTime.Now.Month.ToString().PadLeft(2, '0');

        sReturn = yy + mm + (id + 1).ToString().PadLeft(length, '0');

        return sReturn;
    }

    public static DataRow GetDR(DataTable DT)
    {
        DataRow DR = null;
        if (DT.Rows.Count > 0)
        {
            DR = DT.Rows[0];
        }
        return DR;
    }

    public static sbyte FindIndexTempColumnOfDataFieldInGrid(DataGrid _dgd, string _fieldname)
    {
        sbyte _i = 0; bool _c = false;
        for (; _i < _dgd.Columns.Count; _i++)
        {
            // ให้ข้ามฟิลที่มีชนิดเป็น TemplateColumn
            if (_dgd.Columns[_i].GetType().ToString().Equals("System.Web.UI.WebControls.BoundColumn")) continue;
            //
            if (((TemplateColumn)_dgd.Columns[_i]).HeaderTemplate.Equals(_fieldname)) { _c = true; break; }
        }
        if (!_c) _i = -1;
        return _i;
    }

    public static sbyte FindIndexColumnOfDataFieldInGridView(GridView _dgd, string _HeaderText)
    {
        sbyte _i = 0; bool _c = false;
        for (; _i < _dgd.Columns.Count; _i++)
        {
            if (_dgd.Columns[_i].GetType().ToString().Equals("System.Web.UI.WebControls.TemplateColumn")) continue;
            //
            if (((DataControlField)_dgd.Columns[_i]).HeaderText.Equals(_HeaderText)) { _c = true; break; }
        }
        if (!_c) _i = -1;
        return _i;
    }

    public static void ExecuteSQL(string strCon, string sql)
    {
        SqlConnection objConn = new SqlConnection(strCon);
        SqlCommand cmd;
        try
        {
            objConn.Open();
            cmd = new SqlCommand(sql, objConn);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objConn.Close();
        }
    }
    public static List<T> ConvertObject<T>(object objData)
    {
        List<T> Temp = new List<T>();
        if (objData != null)
        {
            Temp = (List<T>)objData;
        }
        return Temp;
    }
    public static IList<T> ConvertDatableToList<T>(DataTable table)
    {
        if (table == null)
            return null;
        List<DataRow> rows = new List<DataRow>();
        foreach (DataRow row in table.Rows)
            rows.Add(row);
        return ConvertTo<T>(rows);
    }
    public static IList<T> ConvertTo<T>(IList<DataRow> rows)
    {
        IList<T> list = null;
        if (rows != null)
        {
            list = new List<T>();
            foreach (DataRow row in rows)
            {
                //try
                //{
                T item = CreateItem<T>(row);
                list.Add(item);
                //}
                //catch (Exception e)
                //{

                //}
            }
        }
        return list;
    }
}