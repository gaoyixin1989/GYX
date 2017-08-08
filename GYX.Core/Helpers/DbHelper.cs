using System;
using System.Text.RegularExpressions;

namespace GYX.Core.Helpers
{
    public static class DbHelper
    {
        private static Regex m_DangerSQLRegex = new Regex("['|;]", RegexOptions.Compiled);

        #region convert DB object to other type
        public static bool ToBoolean(object obj)
        {
            return ToBoolean(obj, false);
        }

        public static bool ToBoolean(object obj, bool defaultValue)
        {
            return (null == obj || obj == DBNull.Value) ? defaultValue : Convert.ToBoolean(obj);
        }

        public static string ToString(object obj)
        {
            return ToString(obj, "");
        }

        public static string ToString(object obj, string defaultValue)
        {
            return (null == obj || obj == DBNull.Value) ? defaultValue : obj.ToString();
        }

        public static int ToInt32(object obj)
        {
            return ToInt32(obj, 0);
        }

        public static int ToInt32(object obj, int defaultValue)
        {
            return (null == obj || obj == DBNull.Value) ? defaultValue : Convert.ToInt32(obj);
        }

        public static float ToSingle(object obj)
        {
            return ToSingle(obj, 0);
        }

        public static float ToSingle(object obj, int defaultValue)
        {
            return (null == obj || obj == DBNull.Value) ? defaultValue : Convert.ToSingle(obj);
        }
        /// <summary>
        /// 转换指定对象为 double 类型.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(object value)
        {
            return ToSingle(value, 0);
        }

        /// <summary>
        /// 转换指定对象为 double 类型.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(object value, int defaultValue)
        {
            return (null == value || value == DBNull.Value || value.ToString().Length == 0) ? defaultValue : Convert.ToDouble(value);
        }

        /// <summary>
        /// 转换为时间类型.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;
            DateTime output;
            if (DateTime.TryParse(value.ToString(), out output))
                return output;
            return null;
        }

        /// <summary>
        /// 转化为 Guid 类型.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid? ToGuid(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;
            try { return new Guid(value.ToString()); }
            catch { }
            return null;
        }
        #endregion

        /// <summary>
        /// 过滤SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string FilterSQL(string sql)
        {
            if (null == sql || sql.Length == 0)
            {
                return String.Empty;
            }
            return m_DangerSQLRegex.Replace(sql, String.Empty);
        }
    }
}
