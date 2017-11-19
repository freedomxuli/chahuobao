using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Web.Script.Serialization;
using System.Web;
using System.Collections.Specialized;
using System.Xml;
using System.Configuration;

namespace ChaHuoBaoWeb.Wxpay
{
    public class Payment
    {
        private string WeiXinPayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        private string packageStr = "Sign=WXPay";
        /// <summary>
        /// 微信支付商户号（从微信发给你的邮件中获得的）
        /// </summary>
        //public string MchId = "1487599122"; //易科供应链
        public string MchId = ConfigurationManager.AppSettings.GetValues("MchId").First();
        /// <summary>
        /// 应用的APPID（微信发给你的邮件中也有这项内容，一般以wx开头，微信开放平台-管理中心-应用详情也可以看到这项内容）
        /// </summary>
        //public string AppId = "wxb88c2a58b573d655";
        public string AppId = ConfigurationManager.AppSettings.GetValues("AppId").First();
        /// <summary>
        /// 这里是API密钥，不是Appsecret，这里最容易出错了！请务必注意！
        /// 设置方法：微信商户平台(pay.weixin.qq.com)-->账户设置-->API安全-->密钥设置
        /// </summary>
        //public string ApiKey = "weimingliang1980guyulv550weiming";
        public string ApiKey = ConfigurationManager.AppSettings.GetValues("ApiKey").First();

        /// <summary>
        /// 支付成功后，微信会请求这个路径，
        /// </summary>
        public string NotifyUrl = "http://chb.yk56.net/WxPay/Notify";  
        /// <summary>
        /// 支付类构造函数，三个关键参数缺一不可，均不能为空
        /// </summary>
        /// <param name="MchId">微信支付商户号（从微信发给你的邮件中获得的）</param>
        /// <param name="AppId">应用的APPID（微信发给你的邮件中也有这项内容，一般以wx开头，微信开放平台-管理中心-应用详情也可以看到这项内容）</param>
        /// <param name="ApiKey">
        /// 这里是API密钥，不是Appsecret，这里最容易出错了！请务必注意！
        /// 设置方法：微信商户平台(pay.weixin.qq.com)-->账户设置-->API安全-->密钥设置
        /// </param>
        //public Payment(string MchId, string AppId, string ApiKey, string NotifyUrl)
        //{
        //    this.MchId = MchId;
        //    this.AppId = AppId;
        //    this.ApiKey = ApiKey;
        //    this.NotifyUrl = NotifyUrl;
        //}
        /// <summary>
        /// 开发发起支付
        /// </summary>
        /// <param name="TotalFee">总金额，单位：分，不能为空</param>
        /// <param name="TradeNo">订单号，你自己定就好了，不要重复，不能为空</param>
        /// <param name="Des">订单描述，不能为空</param>
        /// <param name="ClientIp">客户端的IP地址，不能为空</param>
        /// <param name="FeeType">货币类型，默认是CNY，人民币</param>
        /// <returns></returns>
        public string Pay(Int64 TotalFee, string TradeNo, string Des, string ClientIp, string FeeType = "CNY")
        {
            string result;

            //为发送请求给微信服务器准备数据
            var nstr = MakeNonceStr();
            Hashtable packageParameter = new Hashtable();
            packageParameter.Add("appid", this.AppId);
            packageParameter.Add("body", Des);
            packageParameter.Add("mch_id", this.MchId);
            packageParameter.Add("notify_url", this.NotifyUrl);


            //ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug(this.NotifyUrl);

            packageParameter.Add("nonce_str", nstr);
            packageParameter.Add("out_trade_no", TradeNo);
            packageParameter.Add("total_fee", TotalFee.ToString());
            packageParameter.Add("spbill_create_ip", ClientIp);
            packageParameter.Add("trade_type", "APP");
            packageParameter.Add("fee_type", FeeType);
            var sign = CreateMd5Sign(packageParameter);
            packageParameter.Add("sign", sign);
            var xe = PostDataToWeiXin(packageParameter);
            //为响应客户端的请求准备数据 
            var timeStamp = MakeTimestamp();
            //ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug(xe);
            if (xe.Element("result_code").Value.ToString() == "FAIL")
            {
                throw new Exception(xe.Element("err_code_des").Value.ToString());//err_code_des
            }
            var prepayId = xe.Element("prepay_id").Value;

            nstr = xe.Element("nonce_str").Value;
            Hashtable paySignReqHandler = new Hashtable();
            paySignReqHandler.Add("appid", this.AppId);
            paySignReqHandler.Add("partnerid", this.MchId);
            paySignReqHandler.Add("prepayid", prepayId);
            paySignReqHandler.Add("noncestr", nstr);
            paySignReqHandler.Add("package", packageStr);
            paySignReqHandler.Add("timestamp", timeStamp.ToString());
            var paySign = CreateMd5Sign(paySignReqHandler);

            var obj = new
            {
                appid = this.AppId,
                partnerid = this.MchId,
                prepayid = prepayId,
                package = packageStr,
                noncestr = nstr,
                timestamp = timeStamp,
                sign = paySign
            };
            var serializer = new JavaScriptSerializer();
            return result = serializer.Serialize(obj);


        }

        private Int64 MakeTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        private XElement PostDataToWeiXin(Hashtable parameters)
        {
            var xmlStr = getXmlStr(parameters);
            var data = Encoding.UTF8.GetBytes(xmlStr);
            Stream responseStream;
            HttpWebRequest request = WebRequest.Create(WeiXinPayUrl) as HttpWebRequest;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            try
            {
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            string str = string.Empty;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                str = reader.ReadToEnd();
            }
            responseStream.Close();
            var xe = XElement.Parse(str);
            return xe;
        }

        private string getXmlStr(Hashtable parameters)
        {
            var sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (string k in parameters.Keys)
            {
                var v = (string)parameters[k];
                if (Regex.IsMatch(v, @"^[0-9.]$"))
                {
                    sb.Append("<" + k + ">" + v + "</" + k + ">");
                }
                else
                {
                    sb.Append("<" + k + "><![CDATA[" + v + "]]></" + k + ">");
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }
        private string CreateMd5Sign(Hashtable parameters)
        {
            var sb = new StringBuilder();
            var akeys = new ArrayList(parameters.Keys);
            akeys.Sort();//排序，这是微信要求的
            foreach (string k in akeys)
            {
                var v = (string)parameters[k];
                sb.Append(k + "=" + v + "&");
            }
            sb.Append("key=" + ApiKey);
            string sign = GetMD5(sb.ToString());
            return sign;
        }

        private string MakeNonceStr()
        {
            var timestap = DateTime.Now.ToString("yyyyMMddhhmmssffff");
            return GetMD5(timestap);
        }
        public string GetMD5(string src)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(src);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            var retStr = BitConverter.ToString(md5data);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
    }

    public class WxPayResponseHandler
    {
        // 密钥 
        private string key;

        // appkey
        private string appkey;

        //xmlMap
        public Hashtable xmlMap;

        // 应答的参数
        protected Hashtable parameters;

        //debug信息
        private string debugInfo;
        //原始内容
        protected string content;

        private string charset = "gb2312";

        //参与签名的参数列表
        private static string SignField = "appid,appkey,timestamp,openid,noncestr,issubscribe";

        protected HttpContext httpContext;

        //初始化函数
        public virtual void init()
        {
        }

        //获取页面提交的get和post参数
        public WxPayResponseHandler(HttpContext httpContext)
        {
            parameters = new Hashtable();
            xmlMap = new Hashtable();

            this.httpContext = httpContext;
            NameValueCollection collection;
            //post data
            if (this.httpContext.Request.HttpMethod == "POST")
            {
                collection = this.httpContext.Request.Form;
                //TOTest
                foreach (string k in collection)
                {
                    string v = (string)collection[k];
                    this.setParameter(k, v);
                }
            }
            //query string
            collection = this.httpContext.Request.QueryString;
            foreach (string k in collection)
            {
                string v = (string)collection[k];
                this.setParameter(k, v);
            }
            if (this.httpContext.Request.InputStream.Length > 0)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(this.httpContext.Request.InputStream);
                XmlNode root = xmlDoc.SelectSingleNode("xml");
                XmlNodeList xnl = root.ChildNodes;

                foreach (XmlNode xnf in xnl)
                {
                    xmlMap.Add(xnf.Name, xnf.InnerText);
                    //LogInfo.SystemLog("key:" + xnf.Name + "  value:" + xnf.InnerText);
                }
            }
        }


        /** 获取密钥 */
        public string getKey()
        { return key; }

        /** 设置密钥 */
        public void setKey(string key, string appkey)
        {
            this.key = key;
            this.appkey = appkey;
        }

        /** 获取参数值 */
        public string getParameter(string parameter)
        {
            string s = (string)parameters[parameter];
            return (null == s) ? "" : s;
        }

        /** 获取xml参数值 */
        public string getXmlMap(string xmlNode)
        {
            string s = (string)xmlMap[xmlNode];
            return (null == s) ? "" : s;
        }

        /** 设置xml参数值 */
        public void SetXmlMap(string name, string innerText)
        {
            xmlMap.Add(name, innerText);
        }

        /** 设置参数值 */
        public void setParameter(string parameter, string parameterValue)
        {
            if (parameter != null && parameter != "")
            {
                if (parameters.Contains(parameter))
                {
                    parameters.Remove(parameter);
                }

                parameters.Add(parameter, parameterValue);
            }
        }

        /** 是否财付通签名,规则是:按参数名称a-z排序,遇到空值的参数不参加签名。 
         * @return boolean */
        public virtual Boolean isTenpaySign()
        {
            StringBuilder sb = new StringBuilder();

            ArrayList akeys = new ArrayList(parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + this.getKey());
            string sign = new Payment().GetMD5(sb.ToString()); //MD5Util.GetMD5(sb.ToString(), getCharset()).ToLower();
            //this.setDebugInfo(sb.ToString() + " => sign:" + sign);
            //debug信息
            return getParameter("sign").ToLower().Equals(sign);
        }

        public static string GetSHA1(string strSource)
        {
            string strResult = "";

            //Create
            System.Security.Cryptography.SHA1 sha = System.Security.Cryptography.SHA1.Create();
            byte[] bytResult = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strSource));
            for (int i = 0; i < bytResult.Length; i++)
            {
                strResult = strResult + bytResult[i].ToString("X2");
            }
            return strResult;
        }

        //判断微信签名
        public virtual Boolean isWXsign()
        {
            StringBuilder sb = new StringBuilder();
            Hashtable signMap = new Hashtable();

            foreach (string k in xmlMap.Keys)
            {
                if (k != "SignMethod" && k != "AppSignature")
                {
                    signMap.Add(k.ToLower(), xmlMap[k]);
                }
            }
            signMap.Add("appkey", this.appkey);


            ArrayList akeys = new ArrayList(signMap.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)signMap[k];
                if (sb.Length == 0)
                {
                    sb.Append(k + "=" + v);
                }
                else
                {
                    sb.Append("&" + k + "=" + v);
                }
            }

            string sign = GetSHA1(sb.ToString()).ToString().ToLower();

            this.setDebugInfo(sb.ToString() + " => SHA1 sign:" + sign);

            return sign.Equals(xmlMap["AppSignature"]);

        }


        //判断是否是微信APP 签名
        public virtual Boolean isWXAppsign()
        {
            StringBuilder sb = new StringBuilder();
            Hashtable signMap = new Hashtable();

            foreach (string k in xmlMap.Keys)
            {
                if (k != "sign" && k != "key" && k != "")
                {
                    signMap.Add(k.ToLower(), xmlMap[k]);
                }
            }

            ArrayList akeys = new ArrayList(signMap.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)signMap[k];
                if (sb.Length == 0)
                {
                    sb.Append(k + "=" + v);
                }
                else
                {
                    sb.Append("&" + k + "=" + v);
                }
            }

            sb.Append("&key=" + this.getKey());
            string sign = new Payment().GetMD5(sb.ToString()); //MD5Util.GetMD5(sb.ToString(), getCharset()).ToUpper();
            return sign.Equals(xmlMap["sign"]);
        }

        //判断微信维权签名
        public virtual Boolean isWXsignfeedback()
        {
            StringBuilder sb = new StringBuilder();
            Hashtable signMap = new Hashtable();

            foreach (string k in xmlMap.Keys)
            {
                if (SignField.IndexOf(k.ToLower()) != -1)
                {
                    signMap.Add(k.ToLower(), xmlMap[k]);
                }
            }
            signMap.Add("appkey", this.appkey);


            ArrayList akeys = new ArrayList(signMap.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)signMap[k];
                if (sb.Length == 0)
                {
                    sb.Append(k + "=" + v);
                }
                else
                {
                    sb.Append("&" + k + "=" + v);
                }
            }

            string sign = GetSHA1(sb.ToString()).ToString().ToLower();

            this.setDebugInfo(sb.ToString() + " => SHA1 sign:" + sign);

            return sign.Equals(xmlMap["AppSignature"]);

        }

        /** 获取debug信息 */
        public string getDebugInfo()
        { return debugInfo; }

        /** 设置debug信息 */
        protected void setDebugInfo(String debugInfo)
        { this.debugInfo = debugInfo; }

        protected virtual string getCharset()
        {
            return this.httpContext.Request.ContentEncoding.BodyName;

        }


    }
}
