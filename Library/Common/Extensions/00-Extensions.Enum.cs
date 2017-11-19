using System;
using System.ComponentModel;
using System.Linq;

namespace Common.Extensions
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static partial class Extensions
    {
        #region 获取描述,使用System.ComponentModel.Description特性设置描述
        /// <summary>
        /// 获取描述,使用System.ComponentModel.Description特性设置描述
        /// </summary>
        /// <param name="instance">枚举实例</param>
        public static string Description(this System.Enum instance)
        {
            return GetDescription(instance.GetType(), instance);
        }
        #endregion

        #region 获取成员值
        /// <summary>
        /// 获取成员值
        /// </summary>
        /// <param name="instance">枚举实例</param>
        public static int Value(this System.Enum instance)
        {
            return GetValue(instance.GetType(), instance);
        }
        #endregion

        #region 获取成员值
        /// <summary>
        /// 获取成员值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="instance">枚举实例</param>
        public static T Value<T>(this System.Enum instance)
        {
            return ConvertHelper.To<T>(Value(instance));
        }
        #endregion

        #region GetName(获取成员名)

        /// <summary>
        /// 获取成员名
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="member">成员名、值、实例均可</param>
        private static string GetName(Type type, object member)
        {
            if (type == null)
                return string.Empty;
            if (member == null)
                return string.Empty;
            if (member is string)
                return member.ToString();
            if (type.IsEnum == false)
                return string.Empty;
            return System.Enum.GetName(type, member);
        }

        #endregion

        #region GetValue(获取成员值)

        /// <summary>
        /// 获取成员值
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="member">成员名、值、实例均可</param>
        private static int GetValue(Type type, object member)
        {
            string value = member.ToStr();
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("member");
            return (int)System.Enum.Parse(type, member.ToString(), true);
        }

        #endregion

        #region GetDescription(获取描述)

        /// <summary>
        /// 获取描述,使用System.ComponentModel.Description特性设置描述
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="member">成员名、值、实例均可</param>
        private static string GetDescription(Type type, object member)
        {
            if (type == null)
                return string.Empty;

            var memberName = GetName(type, member);
            if (memberName.IsEmpty())
                return string.Empty;

            var field = type.GetField(memberName);
            if (field == null)
                return string.Empty;

            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
            if (attribute == null)
                return field.Name;
            return attribute.Description;
        }

        #endregion
    }
}
