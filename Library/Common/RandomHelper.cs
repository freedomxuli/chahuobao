using System;
using System.Text;
using Common.Extensions;

namespace Common
{
    /// <summary>
    /// 随机数操作类
    /// </summary>
    public class RandomHelper
    {
        #region 定义常量
        /// <summary>
        /// 小写英文字母
        /// </summary>
        private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// 大写英文字母
        /// </summary>
        private const string Majuscule = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 数字
        /// </summary>
        private const string Numbers = "0123456789";
        #endregion

        #region 生成一个指定范围的随机整数
        /// <summary>
        /// 获取指定范围的随机整数，该范围包括最小值，但不包括最大值
        /// </summary>
        /// <param name="minNum">最小值</param>
        /// <param name="maxNum">最大值</param>
        public static int GetInt(int minNum, int maxNum)
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(minNum, maxNum);
        }
        #endregion

        #region 生成一个指定范围的随机Decimal
        /// <summary>
        /// 生成一个指定范围的随机Decimal
        /// </summary>
        /// <param name="min">随机数的最小值</param> 
        /// <param name="max">随机数的最大值(结果小于该值)</param> 
        /// <returns></returns>
        public static decimal GetDecimal(decimal min, decimal max)
        {
            decimal value = max - min;
            return (decimal)GetDouble() * value + min;
        }
        #endregion

        #region 生成一个0.0到1.0的随机小数
        /// <summary>
        /// 生成一个0.0到1.0的随机小数
        /// </summary>
        public static double GetDouble()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            return random.NextDouble();
        }
        #endregion

        #region 从字符串里随机得到，规定个数的字符串.
        /// <summary>
        /// 生成随机字符串（数字）
        /// </summary>
        /// <param name="maxLength">最大长度</param>
        public static string GetNumber(int maxLength)
        {
            return Generate(maxLength, Numbers);
        }

        /// <summary>
        /// 生成随机字符串（大小写英文字母+数字）
        /// </summary>
        /// <param name="maxLength">最大长度</param>
        public static string GetString(int maxLength)
        {
            return Generate(maxLength, LowercaseLetters + Majuscule + Numbers);
        }

        /// <summary>
        /// 生成随机英文字母字符串（大小写英文字母）
        /// </summary>
        /// <param name="maxLength">最大长度</param>
        public static string GetLetters(int maxLength)
        {
            return Generate(maxLength, LowercaseLetters + Majuscule);
        }

        /// <summary>
        /// 随机生成指定个数的字符串
        /// </summary>
        private static string Generate(int maxLength, string text)
        {
            var result = new StringBuilder();
            for (int i = 0; i < maxLength; i++)
            {
                result.Append(GetRandomChar(text));
                ThreadHelper.Sleep(6);
            }
            return result.ToString();
        }

        /// <summary>
        /// 获取随机字符
        /// </summary>
        private static string GetRandomChar(string text)
        {
            var index = GetInt(0, text.Length);
            return text[index].ToString();
        }
        #endregion

        #region 获取当前时间+4位随机数（yyMMddhhmmssfff + (xxxx),共19位数字）
        /// <summary> 
        /// 获取当前时间+4位随机数（yyMMddhhmmssfff + (xxxx),共19位数字）
        /// </summary>
        /// <param name="isRemoveMillisecond">是否移除毫秒，移除毫秒后，得到16位数字</param>
        public static string GetKey(bool isRemoveMillisecond = false)
        {
            return DateTimeHelper.GetDateTime().ToDateTimeAllString(isRemoveMillisecond) + GetNumber(4);
        }
        #endregion
        
    }
}
