using System;
using Common.Extensions;

namespace Common
{
    /// <summary>
    /// 转换类
    /// </summary>
    public class ConvertHelper
    {
        #region 数值转换

        #region ToByte
        /// <summary>
        /// 转换为byte
        /// </summary>
        /// <param name="data">数据</param>
        public static byte ToByte(object data)
        {
            if (data == null)
                return 0;
            byte result;
            var success = byte.TryParse(data.ToStr(), out result);
            if (success)
                return result;
            try
            {
                return Convert.ToByte(ToDouble(data, 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 转换为可空byte
        /// </summary>
        /// <param name="data">数据</param>
        public static byte? ToByteOrNull(object data)
        {
            if( data == null )
                return null;
            byte result;
            bool isValid = byte.TryParse( data.ToStr(), out result );
            if( isValid )
                return result;
            return null;
        }
        #endregion

        #region ToInt
        /// <summary>
        /// 转换为整型
        /// </summary>
        /// <param name="data">数据</param>
        public static int ToInt(object data)
        {
            if (data == null)
                return 0;
            int result;
            var success = int.TryParse(data.ToStr(), out result);
            if (success)
                return result;
            try
            {
                return Convert.ToInt32(ToDouble(data, 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 转换为整型，小于0返回0，否则返回原int值
        /// </summary>
        /// <param name="data">数据</param>
        public static int ToInt0(object data)
        {
            int value = ToInt(data);
            return value < 0 ? 0 : value;
        }

        /// <summary>
        /// 转换为整型，小于0返回1，否则返回原int值
        /// </summary>
        /// <param name="data">数据</param>
        public static int ToInt1(object data)
        {
            int value = ToInt(data);
            return value < 1 ? 1 : value;
        }
        
        /// <summary>
        /// 转换为可空整型
        /// </summary>
        /// <param name="data">数据</param>
        public static int? ToIntOrNull( object data ) {
            if( data == null )
                return null;
            int result;
            bool isValid = int.TryParse( data.ToStr(), out result );
            if( isValid )
                return result;
            return null;
        }
        #endregion

        #region ToLong
        /// <summary>
        /// 转换为long
        /// </summary>
        /// <param name="data">数据</param>
        public static long ToLong(object data)
        {
            if (data == null)
                return 0;
            long result;
            var success = long.TryParse(data.ToStr(), out result);
            if (success)
                return result;
            try
            {
                return Convert.ToInt64(ToDouble(data, 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 转换为long，小于0返回0，否则返回原int值
        /// </summary>
        /// <param name="data">数据</param>
        public static long ToLong0(object data)
        {
            long value = ToLong(data);
            return value < 0 ? 0 : value;
        }

        /// <summary>
        /// 转换为long，小于0返回0，否则返回原int值
        /// </summary>
        /// <param name="data">数据</param>
        public static long ToLong1(object data)
        {
            long value = ToLong(data);
            return value < 1 ? 1 : value;
        }

        /// <summary>
        /// 转换为可空long
        /// </summary>
        /// <param name="data">数据</param>
        public static long? ToLongOrNull(object data)
        {
            if (data == null)
                return null;
            long result;
            bool isValid = long.TryParse(data.ToStr(), out result);
            if (isValid)
                return result;
            return null;
        }
        #endregion

        #region ToDouble
        /// <summary>
        /// 转换为双精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static double ToDouble(object data)
        {
            if (data == null)
                return 0;
            double result;
            return double.TryParse(data.ToStr(), out result) ? result : 0;
        }

        /// <summary>
        /// 转换为双精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static double ToDouble(object data, int digits)
        {
            return Math.Round(ToDouble(data), digits);
        }
        
        /// <summary>
        /// 转换为双精度浮点数，小于0返回0，否则返回原double值
        /// </summary>
        /// <param name="data">数据</param>
        public static double ToDouble0(object data)
        {
            double value = ToDouble(data);
            return value < 0 ? 0 : value;
        }

        /// <summary>
        /// 转换为双精度浮点数，小于0返回0，否则返回原double值
        /// </summary>
        /// <param name="data">数据</param>
        public static double ToDouble1(object data)
        {
            double value = ToDouble(data);
            return value < 1 ? 1 : value;
        }

        /// <summary>
        /// 转换为可空双精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static double? ToDoubleOrNull( object data ) {
            if( data == null )
                return null;
            double result;
            bool isValid = double.TryParse( data.ToStr(), out result );
            if( isValid )
                return result;
            return null;
        }
        
        #endregion

        #region ToDecimal
        /// <summary>
        /// 转换为高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static decimal ToDecimal(object data)
        {
            if (data == null)
                return 0;
            decimal result;
            return decimal.TryParse(data.ToStr(), out result) ? result : 0;
        }

        /// <summary>
        /// 转换为高精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static decimal ToDecimal(object data, int digits)
        {
            return Math.Round(ToDecimal(data), digits);
        }
        
        /// <summary>
        /// 转换为高精度浮点数，小于0返回0，否则返回原decimal值
        /// </summary>
        /// <param name="data">数据</param>
        public static Decimal ToDecimal0(object data)
        {
            Decimal value = ToDecimal(data);
            return value < 0 ? 0 : value;
        }

        /// <summary>
        /// 转换为高精度浮点数，小于1返回1，否则返回原decimal值
        /// </summary>
        /// <param name="data">数据</param>
        public static Decimal ToDecimal1(object data)
        {
            Decimal value = ToDecimal(data);
            return value < 1 ? 1 : value;
        }

        /// <summary>
        /// 转换为可空高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static decimal? ToDecimalOrNull( object data ) {
            if( data == null )
                return null;
            decimal result;
            bool isValid = decimal.TryParse( data.ToStr(), out result );
            if( isValid )
                return result;
            return null;
        }

        /// <summary>
        /// 转换为可空高精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static decimal? ToDecimalOrNull( object data, int digits ) {
            var result = ToDecimalOrNull( data );
            if( result == null )
                return null;
            return Math.Round( result.Value, digits );
        }
        #endregion

        #endregion

        #region 日期转换

        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="data">数据</param>
        public static DateTime ToDate(object data)
        {
            if (data == null)
                return DateTime.MinValue;
            DateTime result;
            return DateTime.TryParse(data.ToStr(), out result) ? result : DateTime.MinValue;
        }

        /// <summary>
        /// 转换为可空日期
        /// </summary>
        /// <param name="data">数据</param>
        public static DateTime? ToDateOrNull(object data)
        {
            if (data == null)
                return null;
            DateTime result;
            bool isValid = DateTime.TryParse(data.ToStr(), out result);
            if (isValid)
                return result;
            return null;
        }

        #endregion

        #region 布尔转换

        /// <summary>
        /// 转换为布尔值
        /// </summary>
        /// <param name="data">数据</param>
        public static bool ToBool(object data)
        {
            if (data == null)
                return false;
            bool? value = GetBool(data);
            if (value != null)
                return value.Value;
            bool result;
            return bool.TryParse(data.ToStr(), out result) && result;
        }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        private static bool? GetBool(object data)
        {
            switch (data.ToStr().ToLower())
            {
                case "0":
                    return false;
                case "1":
                    return true;
                case "是":
                    return true;
                case "否":
                    return false;
                case "yes":
                    return true;
                case "no":
                    return false;
                case "y":
                    return true;
                case "n":
                    return false;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 转换为可空布尔值
        /// </summary>
        /// <param name="data">数据</param>
        public static bool? ToBoolOrNull(object data)
        {
            if (data == null)
                return null;
            bool? value = GetBool(data);
            if (value != null)
                return value.Value;
            bool result;
            bool isValid = bool.TryParse(data.ToStr(), out result);
            if (isValid)
                return result;
            return null;
        }

        #endregion

        #region 字符串转换

        /// <summary>
        /// 转换为字符串，并去除两边空格
        /// </summary>
        /// <param name="data">数据</param>
        public static string ToString(object data)
        {
            return data == null ? "" : data.ToString().Trim();
        }

        #endregion

        #region 通用转换
        /// <summary>
        /// 转换为目标元素
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="data">数据</param>
        public static T To<T>(object data)
        {
            if (data == null)
                return default(T);
            if (data is string && string.IsNullOrWhiteSpace(data.ToString()))
                return default(T);
            Type type = ReflectionHelper.GetType<T>();
            try
            {
                if (type.Name.ToLower() == "guid")
                    return (T)(object)new Guid(data.ToString());
                if (data is IConvertible)
                    return (T)Convert.ChangeType(data, type);
                return (T)data;
            }
            catch
            {
                return default(T);
            }
        }
        #endregion

    }
}
