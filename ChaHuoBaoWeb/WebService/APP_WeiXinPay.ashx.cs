using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using ChaHuoBaoWeb.Wxpay;
using Common;
using System.Collections;

namespace ChaHuoBaoWeb.WebService
{
    /// <summary>
    /// APP_WeiXinPay 的摘要说明
    /// </summary>
    public class APP_WeiXinPay : IHttpHandler
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
        /// <param name="dingdan_no">押金订单：01开头，充值订单：02开头</param>
        /// <returns></returns>
        public string Sign(string OrderDenno)
        {
            //编码（101-登录无效，102-账号无效，200-成功，201-失败，202~299-其他原因1-99,300-无效提交方式，400-无效参数）
            //MessagesDataCodeModel json = new MessagesDataCodeModel(false, "无效参数", 401);
            //Aop.Api.IAopClient aop=Aop.Api.DefaultAopClient;
            ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new Models.ChaHuoBaoModels();
            System.Collections.Hashtable hs = new System.Collections.Hashtable();
            string dingdanmiaoshu = "";
            Int64 total_fee = 0;

            try
            {
                //if (OrderDenno != null)
                //{
                //    var payment = new Payment();
                //    //var orderId = "TS" + DateTime.Now.ToString("yyyyMMddhhmmssffff");
                //    var jsonStr = payment.Pay(100, OrderDenno, "卖肉钱", "127.0.0.1");
                //}

                if (OrderDenno == null)
                {
                    ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug("参数错误！");
                    throw new Exception("参数错误！");
                }


                //biz_content data = new biz_content();
                if (OrderDenno.StartsWith("01"))
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
                    //data.body = chongzhimodel.ChongZhiDescribe;
                    //data.out_trade_no = chongzhimodel.OrderDenno;
                    //data.subject = chongzhimodel.ChongZhiDescribe;
                    //data.total_amount = chongzhimodel.ChongZhiJinE.ToString("0.00");

                    //订单编号
                    //out_trade_no = OrderDenno; //Guid.NewGuid().ToString().Replace("-", "");
                    dingdanmiaoshu = "充值支付";
                    total_fee = (Int64)(chongzhimodel.ChongZhiJinE * 100);//费用 1分钱（测试）
                }
                else if (OrderDenno.StartsWith("02"))
                {
                    ChaHuoBaoWeb.Models.GpsDingDan dingdanmodel = db.GpsDingDan.Where(g => g.OrderDenno == OrderDenno).First();
                    if (dingdanmodel.GpsDingDanZhiFuZhuangTai)
                    {
                        ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug("此订单已完成充值！");
                        throw new Exception("此订单已完成充值！");
                    }
                    //充值描述
                    //data.body = "设备押金支付-测试";
                    //data.out_trade_no = dingdanmodel.OrderDenno;
                    //data.subject = "设备押金支付 - 测试";
                    //data.total_amount = dingdanmodel.GpsDingDanJinE.ToString("0.00");

                    //订单编号
                    //out_trade_no = OrderDenno; //Guid.NewGuid().ToString().Replace("-", "");
                    dingdanmiaoshu = "设备押金支付";
                    total_fee = (Int64)(dingdanmodel.GpsDingDanJinE * 100);//费用 1分钱（测试）

                }

                var payment = new Payment();
                //var orderId = "TS" + DateTime.Now.ToString("yyyyMMddhhmmssffff");
                //var jsonStr = payment.Pay(total_fee, OrderDenno, dingdanmiaoshu, Request.ServerVariables["REMOTE_ADDR"].ToString());
                var jsonStr = payment.Pay(total_fee, OrderDenno, dingdanmiaoshu, "47.96.248.12");
                hs["sign"] = "1";
                hs["msg"] = jsonStr;
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Info("成功："+jsonStr);
            }
            catch (Exception ex)
            {
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Info("错误：" + ex);
                hs["sign"] = "0";
                hs["msg"] = ex.Message;

            }

            return JsonHelper.ToJson(hs);
        }
    }
}