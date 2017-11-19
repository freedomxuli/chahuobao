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
    /// APP_TiJiaoTuiDan 的摘要说明
    /// </summary>
    public class APP_TiJiaoTuiDan : IHttpHandler
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
            hash["msg"] = "提交退单失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                IEnumerable<GpsTuiDan> GpsTuiDan = db.GpsTuiDan.Where(x => x.OrderDenno == OrderDenno && x.GpsTuiDanIsEnd == false);
                if (GpsTuiDan.Count() > 0)
                {
                    string GpsTuiDanDenno = GpsTuiDan.First().GpsTuiDanDenno;
                    IEnumerable<GpsTuiDanMingXi> GpsTuiDanMingXi = db.GpsTuiDanMingXi.Where(x => x.GpsTuiDanDenno == GpsTuiDanDenno);
                    int GpsTuiDanShuLiang = GpsTuiDanMingXi.Count();
                    IEnumerable<JiaGeCeLve> JiaGeCeLve = db.JiaGeCeLve.Where(x => x.JiaGeCeLveLeiXing == "YaJin" && x.JiaGeCeLveCiShu == 1);
                    decimal GpsTuiDanJinE = JiaGeCeLve.First().JiaGeCeLveJinE;
                    GpsTuiDan.First().GpsTuiDanIsEnd = true;
                    GpsTuiDan.First().GpsTuiDanShuLiang = GpsTuiDanShuLiang;
                    GpsTuiDan.First().GpsTuiDanJinE = GpsTuiDanShuLiang * GpsTuiDanJinE;

                    //添加 操作记录
                    CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                    CaoZuoJiLu.UserID = UserID;
                    CaoZuoJiLu.CaoZuoLeiXing = "生成退列表";
                    CaoZuoJiLu.CaoZuoNeiRong = "APP内用户生成退单列表，退单列表单号：" + GpsTuiDanDenno + "；设备数量：" + GpsTuiDanShuLiang + "；订单列表押金：" + GpsTuiDanShuLiang * GpsTuiDanJinE + "。";
                    CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                    CaoZuoJiLu.CaoZuoRemark = "";
                    db.CaoZuoJiLu.Add(CaoZuoJiLu);


                    db.SaveChanges();
                    hash["sign"] = "1";
                    hash["msg"] = "提交退单成功！";
                    hash["GpsTuiDanJinE"] = GpsTuiDanShuLiang * GpsTuiDanJinE;
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