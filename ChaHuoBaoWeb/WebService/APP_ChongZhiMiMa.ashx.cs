using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using ChaHuoBaoWeb.Models;
using ChaHuoBaoWeb.PublickFunction;
using Common;

namespace ChaHuoBaoWeb.WebService
{
    /// <summary>
    /// APP_ChongZhiMiMa 的摘要说明
    /// </summary>
    public class APP_ChongZhiMiMa : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            //用户密码
            string UserPassword = context.Request["UserPassword"];
            string UserLeiXing = context.Request["UserLeiXing"];
            Hashtable hash = new Hashtable();

            hash["sign"] = "0";
            hash["msg"] = "重置密码失败！";
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x=>x.UserName==UserName && x.UserLeiXing==UserLeiXing);
                User.First().UserPassword = UserPassword;

                //添加 操作记录
                CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                CaoZuoJiLu.UserID = User.First().UserID;
                CaoZuoJiLu.CaoZuoLeiXing = "重置密码";
                CaoZuoJiLu.CaoZuoNeiRong = "APP内用户重置密码";
                CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                CaoZuoJiLu.CaoZuoRemark = "";
                db.CaoZuoJiLu.Add(CaoZuoJiLu);



                db.SaveChanges();
                hash["sign"] = "1";
                hash["msg"] = "密码修改成功，请重新登录！";
            }
            catch (Exception ex)
            {
                hash["sign"] = "0";
                hash["msg"] = "内部错误:" + ex.Message;
            }
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