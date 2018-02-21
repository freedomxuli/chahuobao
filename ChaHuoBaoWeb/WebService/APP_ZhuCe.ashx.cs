using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using Common;
using ChaHuoBaoWeb.Models;
using ChaHuoBaoWeb.PublickFunction;

namespace ChaHuoBaoWeb.WebService
{
    /// <summary>
    /// APP_ZhuCe 的摘要说明
    /// </summary>
    public class APP_ZhuCe : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserCity = context.Request["UserCity"];
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            //用户密码
            string UserPassword = context.Request["UserPassword"];
            string UserLeiXing = context.Request["UserLeiXing"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "用户注册失败！";
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == UserLeiXing);
                if (User.Count() <= 0)
                {
                    GetTableID gettableid = new GetTableID();
                    User newUser = new User();
                    newUser.UserID =Guid.NewGuid().ToString(); //gettableid.gettableid();
                    newUser.UserName = UserName;
                    newUser.UserLeiXing = UserLeiXing;
                    newUser.UserNameDescribe = "APP注册用户";
                    newUser.UserPassword = UserPassword;
                    newUser.UserCreateTime = DateTime.Now;
                    newUser.UserEndTime = DateTime.Now.AddYears(100);
                    newUser.UserIsLimit = false;
                    newUser.UserCity = UserCity;
                    newUser.UserRemainder = 0;
                    newUser.UserRemark = "APP注册用户";
                    newUser.UserWxEnable = true;
                    db.User.Add(newUser);

                    //添加 操作记录
                    CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                    CaoZuoJiLu.UserID = newUser.UserID;
                    CaoZuoJiLu.CaoZuoLeiXing = "注册";
                    CaoZuoJiLu.CaoZuoNeiRong = "APP内用户注册";
                    CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                    CaoZuoJiLu.CaoZuoRemark = "";
                    db.CaoZuoJiLu.Add(CaoZuoJiLu);

                    db.SaveChanges();
                    hash["sign"] = "1";
                    hash["msg"] = "用户注册成功！";
                }
                else
                {
                    hash["sign"] = "0";
                    hash["msg"] = "用户已存在，注册失败！";
                }
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