using System.Collections.Generic;

namespace Common.Extensions
{
    /// <summary>
    /// 工具扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 转换为用分隔符拼接的字符串
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="quotes">引号，默认不带引号，范例：单引号 "'"</param>
        /// <param name="separator">分隔符，默认使用逗号分隔</param>
        public static string Splice<T>(this IEnumerable<T> list, string quotes = "", string separator = ",")
        {
            return StringHelper.Splice(list, quotes, separator);
        }

        /// <summary>截取字符串,从左边算起 n 个字,并经过 SQL注入过滤 处理(区分中英文长度)，默认去左右两边空格与过滤Sql注入和XSS攻击字符</summary>
        /// <param name="text">字符串</param>
        /// <param name="length">截取的长度(字数)，长度为0或小于0时，则返回过滤后的全部字条串</param>
        /// <param name="isFilterXss">是否去除XSS攻击特殊字符</param>
        /// <param name="isAddEndStr">当字符串长度超出指定长度时，是否在字符串尾部添加“...”字符</param>
        /// <returns></returns>
        public static string Left(this string text, int length, bool isFilterXss = true, bool isAddEndStr = false)
        {
            return StringHelper.Left(text, length, isFilterXss, isAddEndStr);
        }
    }
}
