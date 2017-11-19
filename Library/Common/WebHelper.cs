using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Common.Extensions;
using File = Common.Files.File;

namespace Common
{
    /// <summary>
    /// Web操作
    /// </summary>
    public static class WebHelper
    {
        #region 编码与解码

        #region HtmlEncode(对html字符串进行编码)

        /// <summary>
        /// 对html字符串进行编码
        /// </summary>
        /// <param name="html">html字符串</param>
        public static string HtmlEncode(string html)
        {
            return HttpUtility.HtmlEncode(html);
        }

        #endregion

        #region HtmlDecode(对html字符串进行解码)

        /// <summary>
        /// 对html字符串进行解码
        /// </summary>
        /// <param name="html">html字符串</param>
        public static string HtmlDecode(string html)
        {
            return HttpUtility.HtmlDecode(html);
        }

        #endregion

        #region UrlEncode(对Url进行编码)

        /// <summary>
        /// 对Url进行编码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="isUpper">编码字符是否转成大写,范例,"http://"转成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, bool isUpper = false)
        {
            return UrlEncode(url, Encoding.UTF8, isUpper);
        }

        /// <summary>
        /// 对Url进行编码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="isUpper">编码字符是否转成大写,范例,"http://"转成"http%3A%2F%2F"</param>
        public static string UrlEncode(string url, Encoding encoding, bool isUpper = false)
        {
            var result = HttpUtility.UrlEncode(url, encoding);
            if (!isUpper)
                return result;
            return GetUpperEncode(result);
        }

        /// <summary>
        /// 获取大写编码字符串
        /// </summary>
        private static string GetUpperEncode(string encode)
        {
            var result = new StringBuilder();
            int index = 0;
            for (int i = 0; i < encode.Length; i++)
            {
                string character = encode[i].ToString();
                if (character == "%")
                    index = i;
                if (i - index == 1 || i - index == 2)
                    character = character.ToUpper();
                result.Append(character);
            }
            return result.ToString();
        }

        #endregion

        #region UrlDecode(对Url进行解码)

        /// <summary>
        /// 对Url进行解码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码
        /// </summary>
        /// <param name="url">url</param>
        public static string UrlDecode(string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        /// <summary>
        /// 对Url进行解码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="encoding">字符编码,对于javascript的encodeURIComponent函数编码参数,应使用utf-8字符编码来解码</param>
        public static string UrlDecode(string url, Encoding encoding)
        {
            return HttpUtility.UrlDecode(url, encoding);
        }

        #endregion

        #endregion

        #region Download

        #region DownloadFile(下载文件)

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        /// <param name="fileName">文件名,包含扩展名</param>
        public static void DownloadFile(string filePath, string fileName)
        {
            DownloadFile(filePath, fileName, Encoding.UTF8);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        /// <param name="fileName">文件名,包含扩展名</param>
        /// <param name="encoding">字符编码</param>
        public static void DownloadFile(string filePath, string fileName, Encoding encoding)
        {
            var bytes = Files.File.ReadToBytes(filePath);
            Download(bytes, fileName, encoding);
        }

        #endregion

        #region DownloadUrl(从Http地址下载)

        /// <summary>
        /// 从Http地址下载
        /// </summary>
        /// <param name="url">Http地址，范例：http://www.test.com/a.rar </param>
        /// <param name="fileName">文件名，包括扩展名</param>
        public static void DownloadUrl(string url, string fileName)
        {
            DownloadUrl(url, fileName, Encoding.UTF8);
        }

        /// <summary>
        /// 从Http地址下载
        /// </summary>
        /// <param name="url">Http地址，范例：http://www.test.com/a.rar </param>
        /// <param name="fileName">文件名，包括扩展名</param>
        /// <param name="encoding">字符编码</param>
        public static void DownloadUrl(string url, string fileName, Encoding encoding)
        {
            var client = new WebClient();
            var bytes = client.DownloadData(url);
            Download(bytes, fileName, encoding);
        }

        #endregion

        #region Download(下载)

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="fileName">文件名,包含扩展名</param>
        public static void Download(string text, string fileName)
        {
            Download(text, fileName, Encoding.UTF8);
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="fileName">文件名,包含扩展名</param>
        /// <param name="encoding">字符编码</param>
        public static void Download(string text, string fileName, Encoding encoding)
        {
            var bytes = Files.File.StringToBytes(text, encoding);
            Download(bytes, fileName, encoding);
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="fileName">文件名,包含扩展名</param>
        public static void Download(Stream stream, string fileName)
        {
            Download(stream, fileName, Encoding.UTF8);
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="fileName">文件名,包含扩展名</param>
        /// <param name="encoding">字符编码</param>
        public static void Download(Stream stream, string fileName, Encoding encoding)
        {
            Download(File.StreamToBytes(stream), fileName, encoding);
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <param name="fileName">文件名,包含扩展名</param>
        public static void Download(byte[] bytes, string fileName)
        {
            Download(bytes, fileName, Encoding.UTF8);
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <param name="fileName">文件名,包含扩展名</param>
        /// <param name="encoding">字符编码</param>
        public static void Download(byte[] bytes, string fileName, Encoding encoding)
        {
            if (bytes == null || bytes.Length == 0)
                return;
            HttpContext.Current.Response.ContentType = "application/cotet-stream";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + UrlEncode(fileName.Replace(" ", "")));
            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.ContentEncoding = encoding;
            HttpContext.Current.Response.End();
        }

        #endregion

        #region GetFileControls(获取客户端文件控件集合)

        /// <summary>
        /// 获取有效客户端文件控件集合,文件控件必须上传了内容，为空将被忽略,
        /// 注意:Form标记必须加入属性 enctype="multipart/form-data",服务器端才能获取客户端file控件.
        /// </summary>
        public static List<HttpPostedFile> GetFileControls()
        {
            var result = new List<HttpPostedFile>();
            var files = HttpContext.Current.Request.Files;
            if (files.Count == 0)
                return result;
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                if (file.ContentLength == 0)
                    continue;
                result.Add(files[i]);
            }
            return result;
        }

        #endregion

        #region GetFileControl(获取第一个有效客户端文件控件)

        /// <summary>
        /// 获取第一个有效客户端文件控件,文件控件必须上传了内容，为空将被忽略,
        /// 注意:Form标记必须加入属性 enctype="multipart/form-data",服务器端才能获取客户端file控件.
        /// </summary>
        public static HttpPostedFile GetFileControl()
        {
            var files = GetFileControls();
            if (files == null || files.Count == 0)
                return null;
            return files[0];
        }

        #endregion

        #endregion

        #region Session

        #region SetSession(创建Session)

        /// <summary>
        /// 创建Session
        /// </summary>
        /// <typeparam name="T">Session键值的类型</typeparam>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        /// <param name="session">Session，主要用于异步线程调用时，可以获取Session值</param>
        public static void SetSession<T>(string key, T value, HttpSessionState session = null)
        {
            if (session == null && HttpContext.Current != null)
                session = HttpContext.Current.Session;

            if (key.IsEmpty() || session == null)
                return;
            session[key] = value;
        }

        /// <summary>
        /// 创建Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        /// <param name="session">Session，主要用于异步线程调用时，可以获取Session值</param>
        public static void SetSession(string key, string value, HttpSessionState session = null)
        {
            SetSession<string>(key, value, session);
        }

        #endregion

        #region GetSession(读取Session)

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <typeparam name="T">Session键值的类型</typeparam>
        /// <param name="key">Session的键名</param>        
        public static T GetSession<T>(string key)
        {
            if (key.IsEmpty())
                return default(T);
            return ConvertHelper.To<T>(HttpContext.Current.Session[key]);
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="session">Session，主要用于异步线程调用时，可以获取Session值</param>
        public static string GetSession(string key, HttpSessionState session = null)
        {
            if (session == null && HttpContext.Current != null)
                session = HttpContext.Current.Session;

            if (key.IsEmpty() || session == null)
                return "";

            var value = session[key];
            return value == null ? "" : value.ToStr();
        }
        #endregion

        #region RemoveSession(删除指定Session)

        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="session">Session，主要用于异步线程调用时，可以获取Session值</param>
        public static void RemoveSession(string key, HttpSessionState session = null)
        {
            if (session == null && HttpContext.Current != null)
                session = HttpContext.Current.Session;

            if (key.IsEmpty() || session == null)
                return;
            session.Contents.Remove(key);
        }

        #endregion

        #endregion

        #region Server

        #region Host(获取主机名)

        /// <summary>
        /// 获取主机名,即域名，
        /// 范例：用户输入网址http://www.a.com/b.htm?a=1&amp;b=2，
        /// 返回值为: www.a.com
        /// </summary>
        public static string Host
        {
            get
            {
                if (HttpContext.Current == null)
                    return "";

                string url = "http://" + HttpContext.Current.Request.Url.Host;
                if (HttpContext.Current.Request.Url.Port != 80)
                {
                    url += ":" + HttpContext.Current.Request.Url.Port;
                }
                return url;
            }
        }
        #endregion

        #region ResolveUrl(解析相对Url)

        /// <summary>
        /// 解析相对Url
        /// </summary>
        /// <param name="relativeUrl">相对Url</param>
        public static string ResolveUrl(string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
                return string.Empty;
            relativeUrl = relativeUrl.Replace("\\", "/");
            if (relativeUrl.StartsWith("/"))
                return relativeUrl;
            if (relativeUrl.Contains("://"))
                return relativeUrl;
            return VirtualPathUtility.ToAbsolute(relativeUrl);
        }

        #endregion

        #region 获得当前地址的绝对路径
        /// <summary>获得当前地址的绝对路径</summary>
        /// <param name="path">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string path)
        {
            return File.GetPhysicalPath(path);
        }
        #endregion

        #region 返回指定的服务器变量信息
        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            if (HttpContext.Current == null)
            {
                return "";
            }
            return HttpContext.Current.Request.ServerVariables[strName].ToStr();
        }
        #endregion

        #region 检查是否是当前站点提交
        /// <summary>
        /// 检查是否是当前站点提交
        /// </summary>
        public static bool CheckPost()
        {
            string str1 = "", str2 = "";
            str1 = GetServerString("HTTP_REFERER");
            str2 = GetServerString("SERVER_NAME");

            if (str1.Length > 8)
                str1 = str1.Substring(7);

            if (str1.Length > str2.Length)
                str1 = str1.Substring(0, str2.Length);

            return (str1 == str2);
        }

        #endregion

        #endregion

        #region Request

        #region 获得Post提交的参数值
        /// <summary>获得Post提交的参数值</summary>
        /// <param name="name">表单参数</param>
        /// <param name="isFilterXss">是否过滤XSS</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string name, bool isFilterXss = true)
        {
            return StringHelper.FilterSql(HttpContext.Current.Request.Form[name].ToStr(), isFilterXss);
        }
        #endregion

        #region 获得Get提交的参数值
        /// <summary> 获得Get提交的参数值 </summary>
        /// <param name="name">Url参数</param>
        /// <param name="isFilterXss">是否过滤XSS</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string name, bool isFilterXss = true)
        {
            return StringHelper.FilterSql(HttpContext.Current.Request.QueryString[name].ToStr(), isFilterXss);
        }
        #endregion

        #region 获得Url或表单参数的值(不区分Post或Get提交,优先处理Get方式)
        /// <summary>
        /// 获得Url或表单参数的值(不区分Post或Get提交,优先处理Get方式)
        /// </summary>
        /// <param name="name">参数</param>
        /// <param name="isFilterXss">是否过滤XSS</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string name, bool isFilterXss = true)
        {
            var text = GetQueryString(name, isFilterXss);

            if (text.Length == 0)
            {
                text = GetFormString(name, isFilterXss);
            }
            return text;
        }
        #endregion

        #region 获得Post提交的全部值
        /// <summary>
        /// 获得Post提交的全部值
        /// </summary>
        /// <returns>获得Post提交的全部值</returns>
        public static string GetFormAll()
        {
            return GetFormAll(HttpContext.Current.Request);
        }
        #endregion

        #region 获得Post提交的全部值
        /// <summary>
        /// 获得Post提交的全部值
        /// </summary>
        /// <returns>获得Post提交的全部值</returns>
        public static string GetFormAll(System.Web.HttpRequest request)
        {
            StringBuilder sb = new StringBuilder();
            int ti = request.Form.Count;
            if (ti > 0)
            {
                for (int i = 0; i < ti; i++)
                {
                    sb.Append(request.Form.Keys[i]);
                    sb.Append("=");
                    sb.AppendLine(request.Form[i]);
                    sb.Append(";");
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 检查输入口,是否为同一个服务器提交
        /// <summary>
        /// 检查输入口,是否为同一个服务器提交
        /// </summary>
        public static bool CheckPostUrl()
        {
            var referer = GetServerString("HTTP_REFERER");
            var serverName = GetServerString("SERVER_NAME");

            if (referer.Length > 8)
                referer = referer.Substring(7);

            if (referer.Length > serverName.Length)
                referer = referer.Substring(0, serverName.Length);

            return (referer == serverName);
        }
        #endregion

        #endregion

    }
}
