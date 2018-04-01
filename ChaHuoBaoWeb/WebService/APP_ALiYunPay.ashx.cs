using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using ChaHuoBaoWeb.Models;
using System.Collections;
using Common;
using ChaHuoBaoWeb.Alipay;
using Aop.Api.Util;
using System.Collections.Specialized;

namespace ChaHuoBaoWeb.WebService
{
    /// <summary>
    /// APP_ALiYunPay 的摘要说明
    /// </summary>
    public class APP_ALiYunPay : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string OrderDenno = context.Request["OrderDenno"];
            OrderDenno = HttpUtility.UrlDecode(OrderDenno.ToUpper(), utf8);
            string result = Sign(OrderDenno);
            context.Response.Write(result);
            context.Response.End();
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 根据订单号获取签名 客户端提交支付宝支付，需要携带参数，根据订单编号获取相应订单信息组装URL后返回
        /// </summary>
        /// <param name="dingdan_no">充值订单：01开头，押金订单：02开头</param>
        /// <returns></returns>
        public string Sign(string OrderDenno)
        {
            //编码（101-登录无效，102-账号无效，200-成功，201-失败，202~299-其他原因1-99,300-无效提交方式，400-无效参数）
            //MessagesDataCodeModel json = new MessagesDataCodeModel(false, "无效参数", 401);
            //Aop.Api.IAopClient aop=Aop.Api.DefaultAopClient;

            ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new Models.ChaHuoBaoModels();
            System.Collections.Hashtable hs = new System.Collections.Hashtable();
            string out_trade_no = "";
            double total_fee = 0;
            try
            {
                if (OrderDenno == null)
                {
                    ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug("参数错误！");
                    throw new Exception("参数错误！");
                }


                biz_content data = new biz_content();
                if (OrderDenno.StartsWith("01"))//充值
                {
                    ChaHuoBaoWeb.Models.ChongZhi chongzhimodel;
                    chongzhimodel = db.ChongZhi.Where(g => g.OrderDenno == OrderDenno).First();
                    //生成签名之前，编写自己的验证逻辑...
                    if (chongzhimodel.ZhiFuZhuangTai)
                    {
                        ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug("此订单已完成充值！");
                        throw new Exception("此订单已完成充值！");
                    }

                    //充值描述
                    data.body = "设备充值支付-测试";
                    data.out_trade_no = chongzhimodel.OrderDenno;
                    data.subject = "设备充值支付-测试";
                    data.total_amount = chongzhimodel.ChongZhiJinE.ToString("0.00");

                    //订单编号
                    out_trade_no = OrderDenno; //Guid.NewGuid().ToString().Replace("-", "");
                    total_fee = (Double)(chongzhimodel.ChongZhiJinE);//费用 1分钱（测试）
                }
                else if (OrderDenno.StartsWith("02"))//押金
                {
                    ChaHuoBaoWeb.Models.GpsDingDan dingdanmodel = db.GpsDingDan.Where(g => g.OrderDenno == OrderDenno).First();
                    if (dingdanmodel.GpsDingDanZhiFuZhuangTai)
                    {
                        ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug("此订单已完成充值！");
                        throw new Exception("此订单已完成充值！");
                    }
                    //充值描述
                    data.body = "设备押金支付-测试";
                    data.out_trade_no = dingdanmodel.OrderDenno;
                    data.subject = "设备押金支付 - 测试";
                    data.total_amount = dingdanmodel.GpsDingDanJinE.ToString("0.00");

                    //订单编号
                    out_trade_no = OrderDenno; //Guid.NewGuid().ToString().Replace("-", "");
                    total_fee = (Double)(dingdanmodel.GpsDingDanJinE);//费用 1分钱（测试）

                }
                else if (OrderDenno.StartsWith("03"))
                {
                    ChaHuoBaoWeb.Models.GpsDingDanSale dingdanmodel = db.GpsDingDanSale.Where(g => g.OrderDenno == OrderDenno).First();
                    if (dingdanmodel.GpsDingDanZhiFuZhuangTai)
                    {
                        ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug("此销售订单已完成付款！");
                        throw new Exception("此销售订单已完成付款！");
                    }
                    //充值描述
                    data.body = "设备销售支付";
                    data.out_trade_no = dingdanmodel.OrderDenno;
                    data.subject = "设备销售支付";
                    data.total_amount = dingdanmodel.GpsDingDanJinE.ToString("0.00");

                    //订单编号
                    out_trade_no = OrderDenno; //Guid.NewGuid().ToString().Replace("-", "");
                    total_fee = (Double)(dingdanmodel.GpsDingDanJinE);//费用 1分钱（测试）
                }





                //AopUtils.SignAopRequest(GetBizContent(dingdan_code), Alipay.ConfigApi.AliPar_App_Privatekey, Alipay.ConfigApi.AliPay_App_charset, "RSA2");

                #region 生成签名
                string publicKeyPem = Alipay.ConfigApi.AliPar_App_Publickey;//公钥
                string privateKeyPem = Alipay.ConfigApi.AliPar_App_Privatekey;//私钥
                string app_id = ConfigApi.AliPay_App_app_id;//app支付，支付宝中该应用的ID
                string seller_id = ConfigApi.AliPay_App_seller_id;//商户账户
                string method = ConfigApi.AliPay_App_method;//alipay.trade.app.pay
                string charset = ConfigApi.AliPay_App_charset;//utf-8
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string version = @"1.0";
                string sign_type = @"RSA2";
                string timeout_express = "30m";
                string notify_url = ConfigApi.AliPay_App_notify_url;
                string body = data.body;
                string subject = data.subject;

                //拼接签名使用的字符串【编码】
                string app_id_encode = HttpUtility.UrlEncode(app_id, Encoding.GetEncoding(charset));//
                string charset_encode = HttpUtility.UrlEncode(charset, Encoding.GetEncoding(charset));//
                string method_encode = HttpUtility.UrlEncode(method, Encoding.GetEncoding(charset));//
                string sign_type_encode = HttpUtility.UrlEncode(sign_type, Encoding.GetEncoding(charset));//
                string timestamp_encode = HttpUtility.UrlEncode(timestamp, Encoding.GetEncoding(charset));//
                string version_encode = HttpUtility.UrlEncode(version, Encoding.GetEncoding(charset));//
                string notify_url_encode = HttpUtility.UrlEncode(ConfigApi.AliPay_App_notify_url, Encoding.GetEncoding(charset));//
                string body_encode = HttpUtility.UrlEncode(data.body, Encoding.GetEncoding(charset));//
                string subject_encode = HttpUtility.UrlEncode(data.subject, Encoding.GetEncoding(charset));//
                //订单内容
                string biz_content = "{\"body\":\"" + body + "\",\"subject\":\"" + subject + "\",\"out_trade_no\":\"" + out_trade_no + "\",\"timeout_express\":\"" + timeout_express + "\",\"total_amount\":\"" + total_fee + "\",\"seller_id\":\"" + seller_id + "\",\"product_code\":\"QUICK_MSECURITY_PAY\"}";
                //将订单内容编码，必须和支付宝指定的编码一致 utf-8
                string biz_content_encode = HttpUtility.UrlEncode(biz_content, Encoding.GetEncoding(charset));
                //构建签名参数集合
                IDictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("app_id", app_id);
                dic.Add("biz_content", biz_content);
                dic.Add("charset", charset);
                dic.Add("method", method);
                dic.Add("notify_url", notify_url);
                dic.Add("sign_type", sign_type);
                dic.Add("timestamp", timestamp);
                dic.Add("version", version);
                //得到签名字符串
                string result1 = Aop.Api.Util.AlipaySignature.RSASign(dic, privateKeyPem, charset, false, sign_type);
                //把得到的签名字符串使用指定的格式编码（utf-8），返回给客户端再用utf-8解码就行了
                string result = HttpUtility.UrlEncode(result1, Encoding.GetEncoding(charset));
                string jsonStr = Aop.Api.Util.AlipaySignature.GetSignContent(dic);//得到签名原字符串，客户端要用（支付宝提供的方法）
                //下面是我手动拼接的，其实阿里提供的有...
                string alipaysign = @"app_id=" + app_id_encode + "&biz_content=" + biz_content_encode + "&charset=" + charset_encode + "&method=" + method_encode + "&notify_url=" + notify_url_encode + "&sign_type=" + sign_type_encode + "&timestamp=" + timestamp_encode + "&version=" + version_encode + "&sign=" + result;
                #endregion

                #region 生成订单，返回 out_trade_no
                //生成订单的逻辑...
                #endregion
                //HashTable dt = new HashTable();

                hs["sign"] = "1";
                hs["msg"] = alipaysign;

                //json.Success = true;
                //json.Msg = "操作成功";
                //json.Code = 200;
                //json.Data = new { TradeNo = out_trade_no, Sign = result, SignContent = jsonStr };

            }
            catch (Exception ex)
            {

                hs["sign"] = "0";
                hs["msg"] = ex.Message;
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug(ex);

            }

            return JsonHelper.ToJson(hs);
        }

        /// <summary>
        /// 获取支付内容详情
        /// </summary>
        /// <param name="dingdan_code">订单编码：01 押金 02：充值</param>
        /// <returns></returns>
        //public string GetBizContent(string dingdan_code)
        //{
        //    //根据订单编码获取订单信息
        //    Dictionary<string, string> biz_content_info = new Dictionary<string, string>();
        //    biz_content_info.Add("timeout_express", "30m");//该笔订单允许的最晚付款时间，逾期将关闭交易。
        //    biz_content_info.Add("seller_id", "");//收款支付宝用户ID。 如果该值为空，则默认为商户签约账号对应的支付宝用户ID
        //    biz_content_info.Add("product_code", "QUICK_MSECURITY_PAY");//销售产品码，商家和支付宝签约的产品码，为固定值QUICK_MSECURITY_PAY
        //    biz_content_info.Add("total_amount", "0.01");//订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]
        //    biz_content_info.Add("subject", "Iphone7 128G");//商品的标题/交易标题/订单标题/订单关键字等。
        //    biz_content_info.Add("body", "最新款的手机啦");//对一笔交易的具体描述信息。如果是多种商品，请将商品描述字符串累加传给body。
        //    biz_content_info.Add("out_trade_no", DateTime.Now.ToString("yyyyMMddHHmmssffffff"));//商户网站唯一订单号
        //    string strBizContent = JsonHelper.ToJson(biz_content_info);

        //    return strBizContent;
        //}
    }
}