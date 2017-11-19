using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using ChaHuoBaoWeb.Models;
using Common;
using ChaHuoBaoWeb.PublickFunction;

namespace ChaHuoBaoWeb.WebService
{
    /// <summary>
    /// APP_ShengChengDingDan 的摘要说明
    /// </summary>
    public class APP_ShengChengDingDan : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            //用户密码
            string GpsDeviceID = context.Request["GpsDeviceID"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "生成订单失败！";
            #region
            try
            {
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Info("订单扫描请求");
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                //不应限制当前用户，而应该检查全部设备
                //IEnumerable<GpsDevice> GpsDevice = db.GpsDevice.Where(x => x.UserID == UserID && x.GpsDeviceID == GpsDeviceID);
                IEnumerable<GpsDevice> GpsDevice = db.GpsDevice.Where(x => x.GpsDeviceID == GpsDeviceID);
                if (GpsDevice.Count() > 0)
                {
                    hash["sign"] = "0";
                    hash["msg"] = "该设备已支付押金，无需再生成订单！";
                    ChaHuoBaoWeb.MvcApplication.log4nethelper.Info(hash["msg"]);
                }
                else
                {
                    //去除同一个用户的限制，检查未支付的订单，因为已经支付的订单设备都在设备列表里面，客户退还设备后，又可以扫描了。
                    //IEnumerable<GpsDingDanMingXi> GpsDingDanMingXi = db.GpsDingDanMingXi.Include("GpsDingDanModel").Where(x => x.GpsDeviceID == GpsDeviceID && x.GpsDingDanModel.UserID == UserID && x.GpsDingDanModel.GpsDingDanIsEnd == false);
                    IEnumerable<GpsDingDanMingXi> GpsDingDanMingXi = db.GpsDingDanMingXi.Include("GpsDingDanModel").Where(x => x.GpsDeviceID == GpsDeviceID && x.GpsDingDanModel.GpsDingDanZhiFuZhuangTai == false);

                    if (GpsDingDanMingXi.Count() > 0)
                    {
                        //未支付订单
                        //已提交，退出 警告
                        if (GpsDingDanMingXi.Where(g => g.GpsDingDanModel.GpsDingDanIsEnd == true).Count() > 0)
                        {
                            hash["sign"] = "0";
                            hash["msg"] = "该设备已生成订单,待支付！";
                            ChaHuoBaoWeb.MvcApplication.log4nethelper.Info(hash["msg"]);
                        }
                        else
                        {
                            hash["sign"] = "0";
                            hash["msg"] = "该设备已生成订单,待提交！";
                            ChaHuoBaoWeb.MvcApplication.log4nethelper.Info(hash["msg"]);
                        }
                        //未提交，附加原来单号

                    }
                    else
                    {
                        //需要限制同一个用户订单
                        //IEnumerable<GpsDingDan> GpsDingDan = db.GpsDingDan.Where(x => x.GpsDingDanIsEnd == false );
                        IEnumerable<GpsDingDan> GpsDingDan = db.GpsDingDan.Where(x => x.GpsDingDanIsEnd == false && x.UserID == UserID);
                        string GpsDingDanDenno = "";
                        if (GpsDingDan.Count() > 0)
                        {
                            GpsDingDanDenno = GpsDingDan.First().GpsDingDanDenno;
                            hash["OrderDenno"] = GpsDingDan.First().OrderDenno;
                        }
                        else
                        {
                            GetTableID gettableid = new GetTableID();
                            GpsDingDanDenno = gettableid.gettableid();
                            GpsDingDan GpsDingDan_new = new GpsDingDan();
                            GpsDingDan_new.GpsDingDanDenno = GpsDingDanDenno;
                            GpsDingDan_new.UserID = UserID;
                            GpsDingDan_new.GpsDingDanIsEnd = false;
                            GpsDingDan_new.GpsDingDanShuLiang = 0;
                            GpsDingDan_new.GpsDingDanJinE = 0;
                            GpsDingDan_new.GpsDingDanTime = DateTime.Now;
                            GpsDingDan_new.OrderDenno = "02" + gettableid.getdenno();
                            hash["OrderDenno"] = GpsDingDan_new.OrderDenno;
                            db.GpsDingDan.Add(GpsDingDan_new);
                            db.SaveChanges();
                        }
                        GpsDingDanMingXi GpsDingDanMingXi_new = new GpsDingDanMingXi();
                        GpsDingDanMingXi_new.GpsDingDanDenno = GpsDingDanDenno;
                        GpsDingDanMingXi_new.GpsDeviceID = GpsDeviceID;
                        GpsDingDanMingXi_new.GpsDingDanMingXiTime = DateTime.Now;
                        GpsDingDanMingXi_new.GpsDingDanMingXiRemark = "";
                        db.GpsDingDanMingXi.Add(GpsDingDanMingXi_new);


                        //添加 操作记录
                        CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                        CaoZuoJiLu.UserID = UserID;
                        CaoZuoJiLu.CaoZuoLeiXing = "生成订单";
                        CaoZuoJiLu.CaoZuoNeiRong = "APP内用户生成订单，所属订单列表单号：" + GpsDingDanDenno + "。";
                        CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                        CaoZuoJiLu.CaoZuoRemark = "";
                        db.CaoZuoJiLu.Add(CaoZuoJiLu);



                        db.SaveChanges();
                        hash["sign"] = "1";
                        hash["msg"] = "生成订单成功！";
                        ChaHuoBaoWeb.MvcApplication.log4nethelper.Info(hash["msg"]);
                    }
                }
            }
            catch (Exception ex)
            {
                hash["sign"] = "0";
                hash["msg"] = "内部错误:" + ex.Message;
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug(ex);


            }
            #endregion
            ChaHuoBaoWeb.MvcApplication.log4nethelper.Info(JsonHelper.ToJson(hash));
            context.Response.Write(JsonHelper.ToJson(hash));
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}