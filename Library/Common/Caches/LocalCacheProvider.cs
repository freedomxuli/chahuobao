using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace Common.Caches
{
    /// <summary>
    /// 本地缓存提供程序
    /// </summary>
    public class LocalCacheProvider : CacheProviderBase
    {
        /// <summary>
        /// 设置缓存对象
        /// </summary>        
        /// <param name="key">缓存键</param>
        /// <param name="target">缓存对象</param>
        /// <param name="time">缓存过期时间，单位:秒</param>
        protected override void SetCache(string key, object target, int time)
        {
            HttpRuntime.Cache.Insert(key, target, null, DateTimeHelper.GetDateTime().AddSeconds(time), Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        protected override T GetCache<T>(string key)
        {
            return ConvertHelper.To<T>(HttpRuntime.Cache.Get(key));
        }

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key">缓存键</param>
        protected override void RemoveCache(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public override void Clear()
        {
            var enumerator = HttpRuntime.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Remove(enumerator.Key.ToString());
            }
        }

    }
}
