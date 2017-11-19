using System;

namespace Common.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class Extensions
    {
        #region 转换为byte
        /// <summary>
        /// 转换为byte
        /// </summary>
        /// <param name="obj">数据</param>
        public static byte ToByte(this string obj)
        {
            return ConvertHelper.ToByte(obj);
        }

        /// <summary>
        /// 转换为可空byte
        /// </summary>
        /// <param name="obj">数据</param>
        public static byte? ToByteOrNull(this string obj)
        {
            return ConvertHelper.ToByteOrNull(obj);
        }

        /// <summary>
        /// 转换为byte
        /// </summary>
        /// <param name="obj">数据</param>
        public static byte ToByte(this int obj)
        {
            return ConvertHelper.ToByte(obj);
        }

        #endregion

        #region 转换为int
        /// <summary>
        /// 转换为int
        /// </summary>
        /// <param name="obj">数据</param>
        public static int ToInt(this string obj)
        {
            return ConvertHelper.ToInt(obj);
        }

        /// <summary>
        /// 转换为int，小于0返回0，否则返回原int值
        /// </summary>
        /// <param name="obj">数据</param>
        public static int ToInt0(this string obj)
        {
            return ConvertHelper.ToInt0(obj);
        }

        /// <summary>
        /// 转换为int，小于1返回1，否则返回原int值
        /// </summary>
        /// <param name="obj">数据</param>
        public static int ToInt1(this string obj)
        {
            return ConvertHelper.ToInt1(obj);
        }

        /// <summary>
        /// 转换为可空int
        /// </summary>
        /// <param name="obj">数据</param>
        public static int? ToIntOrNull(this string obj)
        {
            return ConvertHelper.ToIntOrNull(obj);
        }
        #endregion

        #region 转换为long
        /// <summary>
        /// 转换为int
        /// </summary>
        /// <param name="obj">数据</param>
        public static long ToLong(this string obj)
        {
            return ConvertHelper.ToLong(obj);
        }

        /// <summary>
        /// 转换为int，小于0返回0，否则返回原int值
        /// </summary>
        /// <param name="obj">数据</param>
        public static long ToLong0(this string obj)
        {
            return ConvertHelper.ToLong0(obj);
        }

        /// <summary>
        /// 转换为int，小于1返回1，否则返回原int值
        /// </summary>
        /// <param name="obj">数据</param>
        public static long ToLong1(this string obj)
        {
            return ConvertHelper.ToLong1(obj);
        }

        /// <summary>
        /// 转换为可空int
        /// </summary>
        /// <param name="obj">数据</param>
        public static long? ToLongOrNull(this string obj)
        {
            return ConvertHelper.ToLongOrNull(obj);
        }
        #endregion

        #region 转换为double
        /// <summary>
        /// 转换为double
        /// </summary>
        /// <param name="obj">数据</param>
        public static double ToDouble(this string obj)
        {
            return ConvertHelper.ToDouble(obj);
        }

        /// <summary>
        /// 转换为double，小于0返回0，否则返回原double值
        /// </summary>
        /// <param name="obj">数据</param>
        public static double ToDouble0(this string obj)
        {
            return ConvertHelper.ToDouble0(obj);
        }

        /// <summary>
        /// 转换为double，小于1返回1，否则返回原double值
        /// </summary>
        /// <param name="obj">数据</param>
        public static double ToDouble1(this string obj)
        {
            return ConvertHelper.ToDouble1(obj);
        }

        /// <summary>
        /// 转换为可空double
        /// </summary>
        /// <param name="obj">数据</param>
        public static double? ToDoubleOrNull(this string obj)
        {
            return ConvertHelper.ToDoubleOrNull(obj);
        }
        #endregion

        #region 转换为decimal
        /// <summary>
        /// 转换为decimal
        /// </summary>
        /// <param name="obj">数据</param>
        public static decimal ToDecimal(this string obj)
        {
            return ConvertHelper.ToDecimal(obj);
        }

        /// <summary>
        /// 转换为decimal，小于0返回0，否则返回原decimal值
        /// </summary>
        /// <param name="obj">数据</param>
        public static decimal ToDecimal0(this string obj)
        {
            return ConvertHelper.ToDecimal0(obj);
        }

        /// <summary>
        /// 转换为decimal，小于1返回1，否则返回原decimal值
        /// </summary>
        /// <param name="obj">数据</param>
        public static decimal ToDecimal1(this string obj)
        {
            return ConvertHelper.ToDecimal1(obj);
        }

        /// <summary>
        /// 转换为可空decimal
        /// </summary>
        /// <param name="obj">数据</param>
        public static decimal? ToDecimalOrNull(this string obj)
        {
            return ConvertHelper.ToDecimalOrNull(obj);
        }
        #endregion

        #region 转换为日期
        /// <summary>
        /// 转换为日期（yyyy-MM-dd）
        /// </summary>
        /// <param name="obj">数据</param>
        public static DateTime ToDate(this string obj)
        {
            return ConvertHelper.ToDate(obj).Date;
        }

        /// <summary>
        /// 转换为日期（yyyy-MM-dd HH:mm:ss）
        /// </summary>
        /// <param name="obj">数据</param>
        public static DateTime ToDateTime(this string obj)
        {
            return ConvertHelper.ToDate(obj);
        }

        /// <summary>
        /// 转换为可空日期
        /// </summary>
        /// <param name="obj">数据</param>
        public static DateTime? ToDateOrNull(this string obj)
        {
            return ConvertHelper.ToDateOrNull(obj);
        }
        #endregion

        #region 转换字符串
        /// <summary>
        /// 转换字符串，并去除两边空格
        /// </summary>
        /// <param name="obj">对象</param>
        public static string ToStr(this object obj)
        {
            return ConvertHelper.ToString(obj);
        }
        #endregion
    }
}
