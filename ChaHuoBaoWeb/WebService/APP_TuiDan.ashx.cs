using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using ChaHuoBaoWeb.Models;
using Common;

namespace ChaHuoBaoWeb.WebService
{
    /// <summary>
    /// APP_TuiDan 的摘要说明
    /// </summary>
    public class APP_TuiDan : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            string OrderDenno = context.Request["OrderDenno"];
            string GpsTuiDanZhangHao = context.Request["GpsTuiDanZhangHao"];

            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "退单申请失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                if (User.Count() > 0)
                {
                    IEnumerable<GpsTuiDan> GpsTuiDan = db.GpsTuiDan.Where(x => x.UserID == UserID && x.OrderDenno == OrderDenno);
                    if (GpsTuiDan.First().GpsTuiDanIsShenQing == true)
                    {
                        hash["sign"] = "1";
                        hash["msg"] = "退单已申请，待审核";
                    }
                    else
                    {
                        GpsTuiDan.First().GpsTuiDanZhangHao = GpsTuiDanZhangHao;
                        GpsTuiDan.First().GpsTuiDanIsShenQing = true;
                        GpsTuiDan.First().GpsTuiDanShenQingTime = DateTime.Now;

                        //添加 操作记录
                        CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                        CaoZuoJiLu.UserID = UserID;
                        CaoZuoJiLu.CaoZuoLeiXing = "申请退单";
                        CaoZuoJiLu.CaoZuoNeiRong = "APP内用户申请退单，退单账号：" + GpsTuiDanZhangHao +"。";
                        CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                        CaoZuoJiLu.CaoZuoRemark = "";
                        db.CaoZuoJiLu.Add(CaoZuoJiLu);


                        db.SaveChanges();
                        hash["sign"] = "1";
                        hash["msg"] = "退单申请成功，待审核";
                    }
                }
                else
                {
                    hash["sign"] = "0";
                    hash["msg"] = "用户不存在";
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