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
    public class APP_TiJiaoDingDanSale : IHttpHandler
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
            hash["msg"] = "提交销售订单失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                IEnumerable<GpsDingDanSale> GpsDingDanSale = db.GpsDingDanSale.Where(x => x.OrderDenno == OrderDenno && x.GpsDingDanIsEnd == false);
                if (GpsDingDanSale.Count() > 0)
                {
                    string GpsDingDanDenno = GpsDingDanSale.First().GpsDingDanDenno;
                    IEnumerable<GpsDingDanSaleMingXi> GpsDingDanSaleMingXi = db.GpsDingDanSaleMingXi.Where(x => x.GpsDingDanDenno == GpsDingDanDenno);
                    int GpsDingDanShuLiang = GpsDingDanSaleMingXi.Count();
                    IEnumerable<JiaGeCeLve> JiaGeCeLve = db.JiaGeCeLve.Where(x => x.JiaGeCeLveLeiXing == "Goumai" && x.JiaGeCeLveCiShu == 1);
                    decimal GpsDingDanJinE = JiaGeCeLve.First().JiaGeCeLveJinE;
                    GpsDingDanSale.First().GpsDingDanIsEnd = true;
                    GpsDingDanSale.First().GpsDingDanShuLiang = GpsDingDanShuLiang;
                    GpsDingDanSale.First().GpsDingDanJinE = GpsDingDanShuLiang * GpsDingDanJinE;


                    //添加 操作记录
                    CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                    CaoZuoJiLu.UserID = UserID;
                    CaoZuoJiLu.CaoZuoLeiXing = "生成销售订单列表";
                    CaoZuoJiLu.CaoZuoNeiRong = "APP内用户生成销售订单列表，销售订单列表单号：" + GpsDingDanDenno + "；设备数量：" + GpsDingDanShuLiang + "；销售订单列表金额：" + GpsDingDanShuLiang * GpsDingDanJinE + "。";
                    CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                    CaoZuoJiLu.CaoZuoRemark = "";
                    db.CaoZuoJiLu.Add(CaoZuoJiLu);

                    db.SaveChanges();
                    hash["sign"] = "1";
                    hash["msg"] = "提交销售订单成功！";
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