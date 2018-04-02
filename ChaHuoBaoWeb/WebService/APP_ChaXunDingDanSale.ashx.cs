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
    /// APP_ChaXunDingDan 的摘要说明
    /// </summary>
    public class APP_ChaXunDingDanSale : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "查询订单失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                IEnumerable<GpsDingDanSale> GpsDingDanSale = db.GpsDingDanSale.Where(x => x.GpsDingDanIsEnd == true && x.UserID == UserID).OrderByDescending(x => x.GpsDingDanTime);
                if (GpsDingDanSale.Count() > 0)
                {
                    //添加 操作记录
                    CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                    CaoZuoJiLu.UserID = UserID;
                    CaoZuoJiLu.CaoZuoLeiXing = "销售订单列表";
                    CaoZuoJiLu.CaoZuoNeiRong = "APP内用户查询销售订单列表。";
                    CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                    CaoZuoJiLu.CaoZuoRemark = "";
                    db.CaoZuoJiLu.Add(CaoZuoJiLu);
                    db.SaveChanges();


                    hash["dingdanlist"] = GpsDingDanSale;
                    hash["sign"] = "1";
                    hash["msg"] = "查询销售订单成功！";
                }
                else
                {
                    hash["sign"] = "2";
                    hash["msg"] = "未查询到销售订单！";
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