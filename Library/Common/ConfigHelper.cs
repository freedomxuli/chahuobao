using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using Common.Caches;
using Common.Extensions;

namespace Common
{
    /// <summary>
    /// web.config操作类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 读取AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key">Key</param>
        public static string GetString(string key)
        {
            string cacheKey = "AppSettings-" + key;
            try
            {
                //创建默认缓存工厂，从缓存中读取配置参数，如果缓存不存在，则从配置中读出参数并存储到缓存中，缓存默认10分钟过期
                //return CacheFactory.CreateDefaultCache().Get<string>(cacheKey, () => ConfigurationManager.AppSettings[key], 600).ToStr();
                return WebConfigurationManager.AppSettings[key];
            }
            catch
            { }
            return "";
        }

        /// <summary>
        /// 读取AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="separator">分隔符，默认为逗号</param>
        public static string[] GetStringArr(string key, string separator = ",")
        {
            try
            {
                return StringHelper.Split(GetString(key), separator);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 读取AppSettings中的配置Bool信息
        /// </summary>
        /// <param name="key">Key</param>
        public static bool GetBool(string key)
        {
            string value = GetString(key);
            return value == "1" || value.ToLower() == "true" || value == "是";
        }

        /// <summary>
        /// 读取AppSettings中的配置Decimal信息
        /// </summary>
        /// <param name="key">Key</param>
        public static decimal GetDecimal(string key)
        {
            return GetString(key).ToDecimal0();
        }

        /// <summary>
        /// 读取AppSettings中的配置int信息
        /// </summary>
        /// <param name="key">Key</param>
        public static int GetInt(string key)
        {
            return GetString(key).ToInt0();
        }

        /// <summary>
        /// 读取AppSettings中的配置Double信息
        /// </summary>
        /// <param name="key">Key</param>
        public static double GetDouble(string key)
        {
            return GetString(key).ToDouble0();
        }

        #region GetLogContextKey(获取日志上下文键名)

        /// <summary>
        /// 获取日志上下文键名
        /// </summary>
        public static string GetLogContextKey()
        {
            return "Util.Logs.ContextLogger";
        }

        #endregion
    }
}
