﻿namespace Common.Logs.Contexts {
    /// <summary>
    /// 上下文
    /// </summary>
    internal interface IContext {
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键名</param>
        /// <param name="value">对象</param>
        void Add<T>( string key, T value );
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键名</param>
        T Get<T>( string key );
        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="key">键名</param>
        void Remove( string key );
    }
}
