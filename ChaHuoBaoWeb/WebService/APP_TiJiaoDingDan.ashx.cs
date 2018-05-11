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
    /// APP_TiJiaoDingDan 的摘要说明
    /// </summary>
    public class APP_TiJiaoDingDan : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            //订单单号
            string OrderDenno = context.Request["OrderDenno"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "提交订单失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                IEnumerable<GpsDingDan> GpsDingDan = db.GpsDingDan.Where(x => x.OrderDenno == OrderDenno && x.GpsDingDanIsEnd == false);
                if (GpsDingDan.Count() > 0)
                {
                    string GpsDingDanDenno = GpsDingDan.First().GpsDingDanDenno;
                    IEnumerable<GpsDingDanMingXi> GpsDingDanMingXi = db.GpsDingDanMingXi.Where(x => x.GpsDingDanDenno == GpsDingDanDenno && x.GpsDeviceID.StartsWith("1919"));
                    int GpsDingDanShuLiang = GpsDingDanMingXi.Count();
                    IEnumerable<JiaGeCeLve> JiaGeCeLve = db.JiaGeCeLve.Where(x => x.JiaGeCeLveLeiXing == "YaJin" && x.JiaGeCeLveCiShu==1);
                    decimal GpsDingDanJinE = JiaGeCeLve.First().JiaGeCeLveJinE;

                    //IEnumerable<GpsDingDanMingXi> GpsDingDanMingXi2 = db.GpsDingDanMingXi.Where(x => x.GpsDingDanDenno == GpsDingDanDenno && x.GpsDeviceID.StartsWith("8630"));
                    //int GpsDingDanShuLiang2 = GpsDingDanMingXi2.Count();
                    //IEnumerable<JiaGeCeLve> JiaGeCeLve2 = db.JiaGeCeLve.Where(x => x.JiaGeCeLveLeiXing == "YaJin3" && x.JiaGeCeLveCiShu == 1);
                    //decimal GpsDingDanJinE2 = JiaGeCeLve2.First().JiaGeCeLveJinE;

                    GpsDingDan.First().GpsDingDanIsEnd = true;
                    GpsDingDan.First().GpsDingDanShuLiang = GpsDingDanShuLiang;
                    GpsDingDan.First().GpsDingDanJinE = GpsDingDanShuLiang * GpsDingDanJinE;


                    //添加 操作记录
                    CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                    CaoZuoJiLu.UserID = UserID;
                    CaoZuoJiLu.CaoZuoLeiXing = "生成订单列表";
                    CaoZuoJiLu.CaoZuoNeiRong = "APP内用户生成订单列表，订单列表单号：" + GpsDingDanDenno + "；设备数量：" + GpsDingDanShuLiang + "；订单列表押金：" + GpsDingDanShuLiang * GpsDingDanJinE + "。";
                    CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                    CaoZuoJiLu.CaoZuoRemark = "";
                    db.CaoZuoJiLu.Add(CaoZuoJiLu);

                    db.SaveChanges();
                    hash["sign"] = "1";
                    hash["msg"] = "提交订单成功！";
                    hash["GpsDingDanJinE"] = GpsDingDanShuLiang * GpsDingDanJinE;
                }
            }
            catch (Exception ex)
            {
                hash["sign"] = "0";
                hash["msg"] = "内部错误:" + ex.Message;
            }
            #endregion
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