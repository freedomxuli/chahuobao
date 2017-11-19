using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// EnumItemTable
    /// </summary>
    public class EnumItemTable
    {
        /// <summary>
        /// 表名
        /// </summary>
        public static string TableName
        {
            get { return "EnumItem"; }
        }

        /// <summary>
        ///  Key值
        /// </summary>
        public static string Text
        {
            get { return "Text"; }
        }

        /// <summary>
        /// 显示的内容
        /// </summary>
        public static string Value
        {
            get { return "Value"; }
        }
        
        /// <summary>
        /// 排序Id
        /// </summary>
        public static string SortId
        {
            get { return "SortId"; }
        }
    }
}
