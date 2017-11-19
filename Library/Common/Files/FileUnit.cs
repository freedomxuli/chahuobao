using System.ComponentModel;
using Common.Extensions;

namespace Common.Files
{
    /// <summary>
    /// 文件容量单位
    /// </summary>
    public enum FileUnit
    {
        /// <summary>
        /// 字节
        /// </summary>
        [Description("B")]
        Byte = 1,
        /// <summary>
        /// K字节
        /// </summary>
        [Description("KB")]
        K = 2,
        /// <summary>
        /// M字节
        /// </summary>
        [Description("MB")]
        M = 3,
        /// <summary>
        /// G字节
        /// </summary>
        [Description("GB")]
        G = 4
    }
}
