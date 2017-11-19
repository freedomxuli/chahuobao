using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using Aop.Api.Util;
using System.Web.Http;
using Common;
using ChaHuoBaoWeb.Alipay;
using System.Text;
using ChaHuoBaoWeb.Wxpay;
using ChaHuoBaoWeb.Controllers;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace ChaHuoBaoWeb
{
    public class WxPayController : Controller
    {
        //支付回调页面
        public string Notify()
        {
            try
            {
                string resultFromWx = getPostStr();
                var res = XDocument.Parse(resultFromWx);
                //通信成功
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug(res.Element("xml").Element("return_code").Value);
                if (res.Element("xml").Element("return_code").Value == "SUCCESS")
                {
                    if (res.Element("xml").Element("result_code").Value == "SUCCESS")
                    {
                        //交易成功
                        ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new Models.ChaHuoBaoModels();
                        string orderdenno = res.Element("xml").Element("out_trade_no").Value.ToString();
                        string total_fee = res.Element("xml").Element("total_fee").Value.ToString();
                        if (orderdenno.StartsWith("01"))
                        {
                            Models.ChongZhi chongzhimode = db.ChongZhi.Where(g => g.OrderDenno == orderdenno).First();
                            string userid = chongzhimode.UserID;
                            //ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug((Convert.ToDecimal(chongzhimode.ChongZhiJinE) * 100).ToString());
                            if (((Int64)(chongzhimode.ChongZhiJinE* 100)).ToString() == total_fee)
                            {
                                //添加 操作记录
                                Models.CaoZuoJiLu CaoZuoJiLu = new Models.CaoZuoJiLu();
                                CaoZuoJiLu.UserID = userid;
                                CaoZuoJiLu.CaoZuoLeiXing = "充值";
                                CaoZuoJiLu.CaoZuoNeiRong = "APP内用户充值，充值方式：微信；充值单号：" + orderdenno + "；充值金额：" + chongzhimode.ChongZhiJinE + "。";
                                CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                                CaoZuoJiLu.CaoZuoRemark = "";
                                db.CaoZuoJiLu.Add(CaoZuoJiLu);

                                chongzhimode.ZhiFuZhuangTai = true;
                                chongzhimode.ZhiFuTime = DateTime.Now;
                                Models.User user = db.User.Where(g => g.UserID == userid).First();
                                user.UserRemainder += chongzhimode.ChongZhiCiShu;
                                db.SaveChanges();
                            }
                        }
                        else if (orderdenno.StartsWith("02"))
                        {
                            Models.GpsDingDan dingdanmode = db.GpsDingDan.Where(g => g.OrderDenno == orderdenno).First();
                            if (((Int64)(dingdanmode.GpsDingDanJinE* 100)).ToString() == total_fee)
                            {
                                string UserID = dingdanmode.UserID;
                                //添加 操作记录
                                Models.CaoZuoJiLu CaoZuoJiLu = new Models.CaoZuoJiLu();
                                CaoZuoJiLu.UserID = UserID;
                                CaoZuoJiLu.CaoZuoLeiXing = "押金";
                                CaoZuoJiLu.CaoZuoNeiRong = "APP内用户押金，押金方式：微信；押金单号：" + orderdenno + "；押金金额：" + dingdanmode.GpsDingDanJinE + "。";
                                CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                                CaoZuoJiLu.CaoZuoRemark = "";
                                db.CaoZuoJiLu.Add(CaoZuoJiLu);

                                dingdanmode.GpsDingDanZhiFuZhuangTai = true;
                                dingdanmode.GpsDingDanZhiFuShiJian = DateTime.Now;
                                db.SaveChanges();

                                
                                string GpsDingDanDenno = dingdanmode.GpsDingDanDenno;
                                IEnumerable<Models.GpsDingDanMingXi> GpsDingDanMingXi = db.GpsDingDanMingXi.Where(x => x.GpsDingDanDenno == GpsDingDanDenno);
                                if (GpsDingDanMingXi.Count() > 0)
                                {
                                    foreach (var obj in GpsDingDanMingXi)
                                    {
                                        Models.GpsDevice GpsDevice = new Models.GpsDevice();
                                        GpsDevice.UserID = UserID;
                                        GpsDevice.GpsDeviceID = obj.GpsDeviceID;
                                        db.GpsDevice.Add(GpsDevice);
                                    }
                                    db.SaveChanges();
                                }
                            }
                        }
                        ///验证参数，更新运单信息，此处需要注意支付宝返回的消息可能会有重复
                        return "success";
                    }
                    else
                    {
                        ChaHuoBaoWeb.MvcApplication.log4nethelper.Error("微信支付失败！");
                        return "failure";
                    }
                }
                else
                {
                    ChaHuoBaoWeb.MvcApplication.log4nethelper.Error("微信验证签名失败！");
                    return "failure";
                }
            }
            catch (Exception ex)
            {
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug(ex);
                return "failer";
            }
        }
        //获得Post过来的数据
        public string getPostStr()
        {
            Int32 intLen = Convert.ToInt32(Request.InputStream.Length);
            byte[] b = new byte[intLen];
            Request.InputStream.Read(b, 0, intLen);
            return System.Text.Encoding.UTF8.GetString(b);
        }
    }
}
