using System;
using Common.Extensions;

namespace Common.Caches
{
    /// <summary>
    /// 基缓存管理器
    /// </summary>
    public class CacheManagerBase : ICacheManager
    {
        /// <summary>
        /// 初始化基缓存管理器
        /// </summary>
        /// <param name="provider">缓存提供程序</param>
        /// <param name="cacheKey">缓存键</param>
        public CacheManagerBase(ICacheProvider provider, ICacheKey cacheKey)
        {
            provider.CheckNull("provider");
            cacheKey.CheckNull("cacheKey");
            CacheProvider = provider;
            CacheKey = cacheKey;
        }

        /// <summary>
        /// 缓存提供程序
        /// </summary>
        public ICacheProvider CacheProvider { get; private set; }

        /// <summary>
        /// 缓存键
        /// </summary>
        public ICacheKey CacheKey { get; private set; }

        /// <summary>
        /// 获取缓存对象，当缓存对象不存在，则执行方法并添加到缓存中
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="addHandler">添加缓存方法，当缓存对象不存在时，执行该方法获得缓存对象</param>
        /// <param name="time">缓存过期时间，单位：秒</param>
        public T Get<T>(string key, Func<T> addHandler, int time = 10000)
        {
            var lockKey = GetKey(key);
            var result = CacheProvider.Get<T>(lockKey);
            if (result != null && result.ToStr() != "0" && !result.ToStr().IsEmpty())
                return result;
            lock (lockKey)
            {
                //设置缓存对象
                return SetCache(addHandler, lockKey, time, result);
            }
        }

        /// <summary>
        /// 获取缓存键
        /// </summary>
        private string GetKey(string key)
        {
            return CacheKey.GetKey(key);
        }
        
        /// <summary>
        /// 更新缓存
        /// </summary>
        private T SetCache<T>(Func<T> addHandler, string lockKey, int time, T result)
        {
            if (Equals(result, null))
            {
                result = addHandler();
                CacheProvider.Set(lockKey, result, time);
                return result;
            }
            ThreadHelper.StartTask(() => CacheProvider.Set(lockKey, addHandler(), time));
            return result;
        }

        /// <summary>
        /// 获取缓存对象，当缓存对象不存在，则执行方法并添加到缓存中
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="addHandler">添加缓存方法，当缓存对象不存在时，执行该方法获得缓存对象</param>
        /// <param name="time">缓存过期时间，单位：分</param>
        public T GetByMinutes<T>(string key, Func<T> addHandler, int time = 0)
        {
            return Get(key, addHandler, time * 60);
        }

        /// <summary>
        /// 获取缓存对象，当缓存对象不存在，则执行方法并添加到缓存中
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="addHandler">添加缓存方法，当缓存对象不存在时，执行该方法获得缓存对象</param>
        /// <param name="time">缓存过期时间，单位：小时</param>
        public T GetByHours<T>(string key, Func<T> addHandler, int time = 0)
        {
            return Get(key, addHandler, time * 3600);
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="target">缓存对象</param>
        /// <param name="time">缓存过期时间，单位：秒</param>
        public void Set(string key, object target, int time = 10000)
        {
            //CacheProvider.Set(key, DateTimeHelper.GetDateTime(), time);
            CacheProvider.Set(GetKey(key), target, time);
        }

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key">缓存键</param>
        public void Remove(string key)
        {
            CacheProvider.Remove(GetKey(key));
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public void Clear()
        {
            CacheProvider.Clear();
        }
    }
}
