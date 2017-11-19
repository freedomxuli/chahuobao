using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace ChaHuoBaoWeb.Alipay
{
    /// <summary>
    ///  支付宝参数配置
    /// </summary>
    public static  class ConfigApi
    {
        ///以下数据对应 

        public static string AliPay_App_app_id = ConfigurationManager.AppSettings.GetValues("AliPay_App_app_id").First();
        public static string AliPay_App_seller_id = ConfigurationManager.AppSettings.GetValues("AliPay_App_seller_id").First(); //支付app对应的“使用者ID” 此处为易哲软件 2088721840601782   2088202393464386  2088102155201900
        //public static string AliPay_App_seller_id = ConfigurationManager.AppSettings.GetValues("AliPay_App_seller_id").ToString();
        public static string AliPay_App_method = "alipay.trade.app.pay";
        public static string AliPay_App_charset = "utf-8";
        public static string AliPay_App_notify_url = "http://chb.yk56.net/AliPay/Notify";
        //应用公钥
        public static string AliPar_App_Publickey = ConfigurationManager.AppSettings.GetValues("AliPar_App_Publickey").First();
        //应用私钥
        public static string AliPar_App_Privatekey = ConfigurationManager.AppSettings.GetValues("AliPar_App_Privatekey").First();
        //支付宝公钥
        public static string AliPay_ZhiFuBao_Publickey = ConfigurationManager.AppSettings.GetValues("AliPay_ZhiFuBao_Publickey").First();
   



    
    
    }
    public class biz_content
    {
        /// <summary>
        /// 订单描述 [传入]
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 订单标题 [传入]
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// 付款总金额 [传入]
        /// </summary>
        public string total_amount { get; set; }
        /// <summary>
        /// 订单编号GUID
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 订单有效时限
        /// </summary>
        public string timeout_express { get; set; }
    }
}