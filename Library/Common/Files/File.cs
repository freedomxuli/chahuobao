using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.Extensions;

namespace Common.Files
{
    /// <summary>
    /// 文件及流操作
    /// </summary>
    public partial class File
    {

        #region Read(读取文件到字符串)

        /// <summary>
        /// 读取文件到字符串
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static string Read(string filePath)
        {
            return Read(filePath, Encoding.UTF8);
        }

        /// <summary>
        /// 读取文件到字符串
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="encoding">字符编码</param>
        public static string Read(string filePath, Encoding encoding)
        {
            if (!FileExists(filePath))
                return string.Empty;
            using (var reader = new StreamReader(filePath, encoding))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion

        #region ReadToBytes(将文件读取到字节流中)

        /// <summary>
        /// 将文件读取到字节流中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static byte[] ReadToBytes(string filePath)
        {
            if (FileExists(filePath))
                return null;
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
            int fileSize = (int)fileInfo.Length;
            using (BinaryReader reader = new BinaryReader(fileInfo.Open(FileMode.Open)))
            {
                return reader.ReadBytes(fileSize);
            }
        }

        #endregion

        #region Write(将字节流写入文件)

        /// <summary>
        /// 将内容写入日志里
        /// </summary>
        /// <param name="dirPath">存储目录路径（可以是绝对路径，也可以是相对路径）</param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        public static void Write(string dirPath, string fileName, string content)
        {
            if (dirPath.IsEmpty())
                return;

            try
            {
                var now = DateTimeHelper.GetDateTime();
                dirPath = dirPath + now.ToYearMonthString() + "/";
                CreateDirectory(dirPath);

                dirPath += fileName + "_" + now.ToYearMonthDayString() + ".log";
            }
            catch (Exception e)
            {
                throw;
            }

            Write(dirPath, content);
        }

        /// <summary>
        /// 将字符串写入文件,文件不存在则创建
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="content">数据</param>
        public static void Write(string filePath, string content)
        {
            if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(content))
                return;
            System.IO.File.AppendAllText(filePath, content);
        }

        /// <summary>
        /// 将字节流写入文件,文件不存在则创建
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="bytes">数据</param>
        public static void Write(string filePath, byte[] bytes)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;
            if (bytes == null)
                return;
            System.IO.File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// 将日志内容写到log文件中，地址通过Web.config的LogPath属性进行配置
        /// </summary>
        /// <param name="context">日志内容</param>
        /// <param name="fileName">文件名</param>
        public static void WriteLog(string context, string fileName = "")
        {
            Write(ConfigHelper.GetString("LogPath"), fileName, context + "\r\n");
        }
        #endregion

        #region Delete(删除文件)

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePaths">文件集合的绝对路径</param>
        public static void Delete(IEnumerable<string> filePaths)
        {
            foreach (var filePath in filePaths)
                Delete(filePath);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static bool Delete(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            if (filePath.IsEmpty()) return false;

            if (filePath.IndexOf(":") < 0) { filePath = GetPhysicalPath(filePath); }

            if (FileExists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                    return (!FileExists(filePath));
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        #region 删除图片文件,连同相关的大图,中图,小图一并删除
        /// <summary>
        /// 删除图片文件,连同相关的大图,中图,小图一并删除
        /// </summary>
        /// <param name="filePath">文件名(包括完整路径)</param>
        public static bool DeleteFile(string filePath)
        {
            if (filePath.IsEmpty()) return false;

            string bigImg = File.GetFilePathPostfix(filePath, "b");
            string midImg = File.GetFilePathPostfix(filePath, "m");
            string minImg = File.GetFilePathPostfix(filePath, "s");
            string orgImg = File.GetFilePathPostfix(filePath, "o");
            string hotImg = File.GetFilePathPostfix(filePath, "h");

            Delete(filePath);
            Delete(bigImg);
            Delete(midImg);
            Delete(minImg);
            Delete(orgImg);
            Delete(hotImg);

            return true;
        }
        #endregion

        #endregion

        #region CopyFile（复制文件）
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="oldFilePath">原始文件名(包括完整路径)</param>
        /// <param name="newFilePath">目标文件名(包括完整路径)</param>
        public static bool CopyFile(string oldFilePath, string newFilePath)
        {
            if (string.IsNullOrEmpty(oldFilePath)) return false;
            if (oldFilePath.IndexOf(":") < 0) { oldFilePath = GetPhysicalPath(oldFilePath); }
            if (newFilePath.IndexOf(":") < 0) { newFilePath = GetPhysicalPath(newFilePath); }

            if (FileExists(oldFilePath))
            {
                try
                {
                    System.IO.File.Copy(oldFilePath, newFilePath, true);
                    return (FileExists(newFilePath));
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region FileExists（检查文件是否存在）
        /// <summary>
        /// 检查文件是否存在（可以是相对路径也可以是绝对路径）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filePath)
        {
            if (filePath.IsEmpty()) return false;

            if (filePath.IndexOf(":") < 0) { filePath = GetPhysicalPath(filePath); }

            return System.IO.File.Exists(filePath);
        }
        #endregion

        #region GetAllFiles(获取目录中全部文件列表)

        /// <summary>
        /// 获取目录中全部文件列表，包括子目录
        /// </summary>
        /// <param name="directoryPath">目录绝对路径</param>
        public static List<string> GetAllFiles(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories).ToList();
        }

        #endregion
    }
}
