using System;
using System.Collections.Generic;
using System.IO;
using Common.Extensions;
using Common.Logs;

namespace Common.Files
{
    /// <summary>
    /// 文件及流操作 - 文件信息
    /// </summary>
    public partial class File
    {
        #region 从路径中抽取文件名(包括扩展名)
        /// <summary>
        /// 从路径中抽取文件名(包括扩展名)
        /// </summary>
        /// <param name="path"></param>
        public static string GetFileNameForUrl(string path)
        {
            if (path.IsEmpty()) return "";

            if (path.LastIndexOf("/") > 0)
            {
                path = path.Substring(path.LastIndexOf("/") + 1);
            }
            else if (path.LastIndexOf("\\") > 0)
            {
                path = path.Substring(path.LastIndexOf("\\") + 1);
            }
            return path;
        }
        #endregion

        #region 从文件名中获得扩展名
        /// <summary>
        /// 从文件名中抽取扩展名
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static string GetExtension(string filePath)
        {
            if (filePath.IsEmpty()) return "";

            if (filePath.LastIndexOf(".") > 0)
            {
                filePath = filePath.Substring(filePath.LastIndexOf(".") + 1);
            }
            else
            {
                filePath = "";
            }
            return filePath;
        }
        #endregion

        #region 取得随机文件名(原文件名),用yyMMddhhmmss + (xxx),共15位数字
        /// <summary>
        ///  取得随机文件名(原文件名),用yyMMddhhmmss + (xxx),共15位数字
        /// </summary>
        /// <param name="extension">扩展名</param>
        public static string GetRandomFileName(string extension)
        {
            return RandomHelper.GetKey() + "." + extension;
        }
        #endregion

        #region 读取文件大小（单位Kb）
        /// <summary>
        /// 读取文件大小（单位Kb）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static long GetFileSize(string filePath)
        {
            if (filePath.IsEmpty()) return 0;

            if (filePath.IndexOf(":") < 0) { filePath = GetPhysicalPath(filePath); }

            if (FileExists(filePath))
            {
                try
                {
                    return ConvertHelper.ToLong0(new System.IO.FileInfo(filePath).Length / 1024);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return 0;
        }
        #endregion

        #region 在指定文件名中添加后缀字符,组成新文件名,用于缩略图,(包括路径)
        /// <summary>
        /// 在指定文件名中添加后缀字符,组成新文件名,用于缩略图,(包括路径)
        /// <para />
        /// 例如:getFilePathPostfix("090801.gif","s") return "090801s.gif" <para />
        /// </summary>
        /// <param name="filePath">文件名</param>
        /// <param name="postfix">后缀字符</param>
        public static string GetFilePathPostfix(string filePath, string postfix)
        {
            if (filePath.IsEmpty()) return "";

            string str = filePath;

            if (str.LastIndexOf(".") > 0)
            {
                int index = str.LastIndexOf(".");

                str = str.Substring(0, index) + postfix + str.Substring(index);
            }
            else
            {
                str += postfix;
            }
            return str;
        }
        #endregion
    }
}
