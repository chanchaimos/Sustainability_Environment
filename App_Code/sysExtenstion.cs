using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Globalization;

/// <summary>
/// Summary description for sysExtenstion
/// </summary>

namespace sysExtension
{
    public static class sysExtenstion
    {
        public static bool IsNumber(this string instance)
        {
            foreach (char ch in instance)
            {
                if (!char.IsNumber(ch)) return false;
            }
            return true;
        }

        public static bool In(this object instance, IEnumerable collections)
        {
            foreach (var item in collections)
            {
                if (item.Equals(instance)) return true;
            }
            return false;
        }

        /// <summary>
        /// Equals Value
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="sval">format : "1,2,3" or "x,y,z"</param>
        /// <returns></returns>
        public static bool In(this object instance, string sval)
        {
            string[] arr = (sval + "").Split(',');
            foreach (var item in arr)
            {
                if (item.Equals(instance)) return true;
            }
            return false;
        }

        /// <summary>
        /// Spill data to format 'value','value'
        /// </summary>
        /// <param name="collections"></param>
        /// <returns>case null value return ''</returns>
        public static string SplitToInSQL(this IEnumerable instance)
        {
            string result = "";
            foreach (var item in instance)
            {
                result += ",'" + item + "'";
            }
            result = result.Length > 1 ? result.Remove(0, 1) : "''";
            return result;
        }

        public static string DecimalString(this decimal? instance, int nDigit)
        {
            return instance != null ? instance.Value.ToString("n" + nDigit) : "";
        }

        /// <summary>
        /// Date format dd/MM/yyyy
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string DateString(this DateTime? instance)
        {
            return instance != null ? instance.Value.ToString("dd/MM/yyyy", new CultureInfo("en-US")) : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="sCulture">Default en-US</param>
        /// <returns></returns>
        public static string DateStringCulture(this DateTime? instance, string sCulture)
        {
            sCulture = string.IsNullOrEmpty(sCulture) ? "en-US" : "";
            return instance != null ? instance.Value.ToString("dd/MM/yyyy", new CultureInfo(sCulture)) : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string DateString(this DateTime? instance, string format)
        {
            return instance != null ? instance.Value.ToString(format) : "";
        }

        /// <summary>
        /// Date Time format dd/MM/yyyy HH:mm
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string DateTimeString(this DateTime? instance)
        {
            return instance != null ? instance.Value.ToString("dd/MM/yyyy HH:mm") : "";
        }

        /// <summary>
        /// Date Time format dd/MM/yyyy HH:mm tt
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string DateTime12HrString(this DateTime? instance)
        {
            return instance != null ? instance.Value.ToString("dd/MM/yyyy HH:mm tt") : "";
        }

        public static string DateTime12HrString(this DateTime instance)
        {
            return instance != null ? instance.ToString("dd/MM/yyyy HH:mm tt") : "";
        }

        public static string IntString(this int? instance)
        {
            return instance != null ? instance.Value.ToString("n0") : "";
        }

        public static string SplitEmailToName(this object instance)
        {
            return instance != null ? (instance + "").Split('@')[0] + "" : "";
        }

        public static decimal? ToDecimal(this decimal? instance, int nDigit)
        {
            if (instance.HasValue)
            {
                return Math.Round(instance.Value, nDigit);
            }
            else
            {
                return null;
            }
        }

        public static string SubString(this string instance, int nStartIndex, int nLength)
        {
            if (!string.IsNullOrEmpty(instance))
            {
                if (instance.Length <= (nStartIndex + nLength))
                {
                    return instance.Substring(nStartIndex, instance.Length);
                }
                else
                {
                    return instance.Substring(nStartIndex, nLength);
                }
            }
            else
                return "";
        }

        public static string MaxLengthText(this string instance, int nLength)
        {
            if (!string.IsNullOrEmpty(instance))
            {
                if (instance.Length <= nLength)
                {
                    return instance;
                }
                else
                {
                    return instance.Substring(0, nLength);
                }
            }
            else
                return "";
        }

        public static string Trims(this string instance)
        {
            return (instance + "").Trim();
        }

        // Anti SQL Injection
        public static string ReplaceInjection(this string str)
        {
            string[] _blacklist = new string[] { "'", "\\", "\"", "*/", ";", "--", "<script", "/*", "</script>" };
            string strRep = str;
            if (strRep == null || strRep.Trim().Equals(String.Empty))
                return strRep;
            foreach (string _blk in _blacklist) { strRep = strRep.Replace(_blk, ""); }

            return strRep;
        }

        public static string STCDecrypt(this string instance)
        {
            if (!string.IsNullOrEmpty(instance))
                return STCrypt.Decrypt(instance);
            else
                return "";
        }

        public static int toIntNullToZero(this string instance)
        {
            return SystemFunction.ParseInt(instance);
        }

        public static int? toIntNull(this string instance)
        {
            return SystemFunction.GetIntNull(instance);
        }

        #region Sorting
        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name == name);
            if (matchedProperty == null)
                throw new ArgumentException("name");

            return matchedProperty;
        }

        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var expr = Expression.Lambda(propAccess, paramExpr);
            return expr;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> query, string name)
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
        }

        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> query, string name)
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string name)
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
        }
        #endregion
    }
}