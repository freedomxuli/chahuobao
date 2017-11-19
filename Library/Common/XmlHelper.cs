/*******************************************
 *  Desc:       Xml 公共函数
 *  Author:     July 
 *  Date:       2014-09-28
********************************************/
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using File = Common.Files.File;

namespace Common
{
    /// <summary>
    /// Xml 帮助类
    /// </summary>
    public class XmlHelper
    {
        #region Xml序列化
        /// <summary>序列化</summary>
        /// <param name="obj">对象</param>  
        /// <returns></returns>
        public static string ToXml(object obj)
        {
            XmlSerializer oXml = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            try
            {
                oXml.Serialize(ms, obj);
            }
            catch (Exception e)
            {
                return "";
            }

            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string str = sr.ReadToEnd();
            ms.Dispose();
            return str;
        }
        #endregion

        #region Xml反序列化
        /// <summary>反序列化</summary>
        /// <param name="xmlFilePath">xml文件路径</param>
        /// <returns>类型数据</returns>
        public static object XmlFileToObject<T>(string xmlFilePath)
        {
            var xml = File.Read(File.GetPhysicalPath(xmlFilePath));
            if (xml.Length > 0)
            {
                object ret;

                try
                {
                    XmlSerializer oXml = new XmlSerializer(typeof(T));
                    using (TextReader sr = new StringReader(xml))
                    {
                        ret = oXml.Deserialize(sr);
                    }
                    return ret;
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }

            }
            return null;
        }

        /// <summary>反序列化</summary>
        /// <param name="xml">xml字符串</param>
        /// <returns>类型数据</returns>
        public static T ToObject<T>(string xml)
        {
            XmlSerializer oXml = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(xml))
            {
                return (T)oXml.Deserialize(sr);
            }
        }

        /// <summary>反序列化</summary>
        /// <param name="xml">xml字符串</param>
        /// <returns>类型数据</returns>
        public static object ToObject2<T>(string xml)
        {
            try
            {
                XmlSerializer oXml = new XmlSerializer(typeof(T));
                using (StringReader sr = new StringReader(xml))
                {
                    return oXml.Deserialize(sr);
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
        #endregion

        #region 读取
        /// <summary>读取一个Node 并返回 InnerText</summary>
        /// <param name="oXmlDoc"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static XmlNode ReadNode(XmlElement oXmlDoc, string key)
        {
            return oXmlDoc.SelectSingleNode(key);
        }

        /// <summary>读取一个Node 并返回 InnerText</summary>
        /// <param name="oXmlDoc"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadNodeInnerText(XmlElement oXmlDoc, string key)
        {
            var oNode = oXmlDoc.SelectSingleNode(key);
            if (oNode != null)
            {
                return oNode.InnerText;

            }
            return "";
        }

        /// <summary>读取一个Node 并返回 InnerText</summary>
        /// <param name="oXmlDoc"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int ReadNodeInnerTextInt(XmlElement oXmlDoc, string key)
        {
            var oNode = oXmlDoc.SelectSingleNode(key);
            if (oNode != null)
            {
                return ConvertHelper.ToInt(oNode.InnerText);

            }
            return 0;
        }

        /// <summary>读取一个Node 并返回 InnerText</summary>
        /// <param name="oXmlDoc"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadNodeInnerText(XmlNode oXmlDoc, string key)
        {
            var oNode = oXmlDoc.SelectSingleNode(key);
            if (oNode != null)
            {
                return oNode.InnerText;

            }
            return "";
        }

        /// <summary>读取一个Node 并返回 InnerText</summary>
        /// <param name="oXmlDoc"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int ReadNodeInnerTextInt(XmlNode oXmlDoc, string key)
        {
            var oNode = oXmlDoc.SelectSingleNode(key);
            if (oNode != null)
            {
                return ConvertHelper.ToInt(oNode.InnerText);

            }
            return 0;
        }
        #endregion
    }
}
