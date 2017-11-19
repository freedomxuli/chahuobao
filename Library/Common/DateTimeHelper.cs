using System;
using System.Text;
using Common.Extensions;

namespace Common
{
    /// <summary>
    /// 时间操作
    /// </summary>
    public static class DateTimeHelper
    {
        #region GetDateTime(获取当前日期时间)

        /// <summary>
        /// 获取当前日期时间
        /// </summary>
        public static DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        #endregion

        #region GetDate(获取当前日期)

        /// <summary>
        /// 获取当前日期,不带时间
        /// </summary>
        public static DateTime GetDate()
        {
            return GetDateTime().Date;
        }

        #endregion
        
        #region GetUnixTimestamp(获取Unix时间戳)

        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        public static long GetUnixTimestamp()
        {
            return GetUnixTimestamp(GetDateTime());
        }

        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        /// <param name="time">时间</param>
        public static long GetUnixTimestamp(DateTime time)
        {
            return (time.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        #endregion

        #region GetTimeFromUnixTimestamp(从Unix时间戳获取时间)

        /// <summary>
        /// 从Unix时间戳获取时间
        /// </summary>
        /// <param name="timestamp">Unix时间戳</param>
        public static DateTime GetTimeFromUnixTimestamp(long timestamp)
        {
            var start = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan span = new TimeSpan(long.Parse(timestamp + "0000000"));
            return start.Add(span);
        }

        #endregion

        #region Format(格式化时间间隔)

        /// <summary>
        /// 格式化时间间隔
        /// </summary>
        /// <param name="span">时间间隔</param>
        public static string Format(TimeSpan span)
        {
            StringBuilder result = new StringBuilder();
            if (span.Days > 0)
                result.AppendFormat("{0}天", span.Days);
            if (span.Hours > 0)
                result.AppendFormat("{0}小时", span.Hours);
            if (span.Minutes > 0)
                result.AppendFormat("{0}分", span.Minutes);
            if (span.Seconds > 0)
                result.AppendFormat("{0}秒", span.Seconds);
            return result.ToString();
        }

        #endregion

        #region DateDiff(比较两个时间相差几秒、几分、几小时、几日、几周、几月、几季或几年)
        /// <summary>比较两个时间相差几秒、几分、几小时、几日、几周、几月、几季或几年</summary>
        /// <param name="sign"> y = 年, m =月, d = 日, w = 周, q = 季, h = 时, n = 分钟, s = 秒 </param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        public static long DateDiff(string sign, DateTime startTime, DateTime endTime)
        {
            long result = 0;
            TimeSpan ts = new TimeSpan(endTime.Ticks - startTime.Ticks);
            switch (sign.ToStr().ToLower())
            {
                case "s": result = (long)ts.TotalSeconds; break;
                case "n": result = (long)ts.TotalMinutes; break;
                case "h": result = (long)ts.TotalHours; break;
                case "d": result = ts.Days; break;
                case "w": result = (ts.Days / 7); break;
                case "m": result = (ts.Days / 30); break;
                case "q": result = ((ts.Days / 30) / 3); break;
                case "y": result = (ts.Days / 365); break;
            }
            return (result);
        }
        #endregion

        #region 计算时间间隔
        /// <summary>
        /// 计算时间间隔
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static string LoginDuration(object startTime, object endTime)
        {
            try
            {
                double minu = DateDiff("n", ConvertHelper.ToDate(startTime), ConvertHelper.ToDate(endTime));
                if (minu < 1.0)
                {
                    return "小于1分钟";
                }
                return minu.ToString("0") + "分钟";
            }
            catch (Exception)
            {
                return "计算异常";
            }
        }
        #endregion
    }
}
