using System;
using System.IO;
using System.Web;
using ServiceStack.Common;

namespace Common.Files
{
    /// <summary>
    /// 文件及流操作 - 文件路径操作
    /// </summary>
    public partial class File
    {
        #region 连接基路径和子路径
        /// <summary>
        /// 连接基路径和子路径,比如把 c: 与 test.doc 连接成 c:\test.doc
        /// </summary>
        /// <param name="basePath">基路径,范例：c:</param>
        /// <param name="subPath">子路径,可以是文件名, 范例：test.doc</param>
        public static string JoinPath(string basePath, string subPath)
        {
            basePath = basePath.TrimEnd('/').TrimEnd('\\');
            subPath = subPath.TrimStart('/').TrimStart('\\');
            string path = basePath + "\\" + subPath;
            return path.Replace("/", "\\").ToLower();
        }
        #endregion

        #region GetPhysicalPath(获取物理路径)

        /// <summary>
        /// 获取物理路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        public static string GetPhysicalPath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return string.Empty;

            if (HttpContext.Current == null)
            {
                if (relativePath.StartsWith("~"))
                    relativePath = relativePath.Remove(0, 2);
                return Path.GetFullPath(relativePath);
            }
            if (relativePath.StartsWith("~"))
                return HttpContext.Current.Server.MapPath(relativePath);
            if (relativePath.StartsWith("/") || relativePath.StartsWith("\\"))
                return HttpContext.Current.Server.MapPath("~" + relativePath);
            return HttpContext.Current.Server.MapPath("~/" + relativePath);
        }

        #endregion

        #region 目录是否存在
        /// <summary>目录是否存在</summary>
        /// <param name="dirpath">目录名</param>
        /// <returns>是否存在</returns>
        public static bool ExistsDirectory(string dirpath)
        {
            if (string.IsNullOrEmpty(dirpath)) return false;

            return Directory.Exists(dirpath);
        }
        #endregion

        #region 创建目录，如果父目录不存在，将一级级生成
        /// <summary>创建目录，如果父目录不存在，将一级级生成</summary>
        /// <param name="dirpath">/newsfile/2009/07/</param>
        /// <returns>返回创建目录是否成功</returns>
        public static bool CreateDirectory(string dirpath)
        {
            if (dirpath.IsEmpty())
            {
                return false;
            }

            if (!dirpath.EndsWith("/"))
            {
                dirpath += "/";
            }

            //判断当前路径是否是相对路径
            if (dirpath.IndexOf(":") == -1)
            {
                dirpath = GetPhysicalPath(dirpath);
            }
            else
            {
                //非web程序引用
                dirpath = dirpath.Replace("/", "\\");
                if (dirpath.StartsWith("\\"))
                {
                    dirpath = dirpath.TrimStart('\\');
                }
            }
            //判断目录是否存在
            if (ExistsDirectory(dirpath))
            {
                return true;
            }
            else
            {
                string[] pathArr = dirpath.Split('\\');
                string path = pathArr[0] + "\\";
                //逐层判断所有文件夹是否存在，不存在的则直接创建
                for (int i = 1; i < pathArr.Length; i++)
                {
                    path += pathArr[i] + "\\";
                    try
                    {
                        if (!ExistsDirectory(path))
                        {
                            if (!ExistsDirectory(path)) Directory.CreateDirectory(path);
                            try
                            {
                                if (!ExistsDirectory(path)) Directory.CreateDirectory(path);
                            }
                            catch (Exception e)
                            {
                                return false;
                            }
                        }
                    }
                    catch
                    {}
                }
                return ExistsDirectory(dirpath);
            }
        }
        #endregion

        #region 修正路径右边缺少"/"
        /// <summary>修正路径右边缺少"/"</summary>
        /// <param name="path">路径</param>
        public static string FixDirPath(string path)
        {
            if (path.IsEmpty()) return "";
            //将\\替换为/
            path = path.Replace("\\", "/");
            //判断最后一个字条是否为/，不是的话添加上
            if (!path.EndsWith("/")) { path += "/"; }
            return path;
        }
        #endregion

    }
}
