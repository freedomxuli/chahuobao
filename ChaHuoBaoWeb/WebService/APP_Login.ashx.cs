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
    /// APP_Login 的摘要说明
    /// </summary>
    public class APP_Login : IHttpHandler
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
            hash["msg"] = "用户登录失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserPassword == UserPassword && x.UserLeiXing == UserLeiXing);
                if (User.Count() > 0)
                {
                    if (User.First().UserIsLimit == false)
                    {
                        //添加 操作记录
                        CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                        CaoZuoJiLu.UserID = User.First().UserID;
                        CaoZuoJiLu.CaoZuoLeiXing = "登录";
                        CaoZuoJiLu.CaoZuoNeiRong = "APP内用户登录";
                        CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                        CaoZuoJiLu.CaoZuoRemark = "";
                        db.CaoZuoJiLu.Add(CaoZuoJiLu);
                        db.SaveChanges();

                        hash["sign"] = "1";
                        hash["msg"] = "登陆成功！";
                    }
                    else
                    {
                        hash["sign"] = "0";
                        hash["msg"] = "用户未授权登陆！";
                    }
                }
                else
                {
                    hash["sign"] = "0";
                    hash["msg"] = "账号密码错误，登陆失败！";
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