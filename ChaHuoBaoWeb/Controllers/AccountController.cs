using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using ChaHuoBaoWeb.Filters;
using ChaHuoBaoWeb.Models;
using System.Collections;
using Common;
using System.Text;

namespace ChaHuoBaoWeb.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult OnPermissionError()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public string Login(string yonghuming, string mima)
        {
            Hashtable hash = new Hashtable();
            try
            {
                hash["sign"] = "0";
                ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
                //查看用户名密码是否为空
                if (!string.IsNullOrEmpty(yonghuming))
                {
                    if (!string.IsNullOrEmpty(mima))
                    {
                        IEnumerable<User> user = accountdb.User.Include("User_Roles").Where(x => x.UserName == yonghuming && x.UserPassword == mima && x.UserLeiXing == "WEB");
                        if (user.Count() > 0)
                        {
                            user = user.Where(x => x.UserIsLimit == false);
                            if (user.Count() > 0)
                            {
                                user = user.Where(x => x.UserEndTime > DateTime.Now);
                                if (user.Count() > 0)
                                {
                                    string UserID = user.First().UserID;
                                    SaveCookie(UserID, yonghuming, mima);
                                    hash["sign"] = "1";
                                    hash["msg"] = "登录成功";
                                }
                                else
                                {
                                    hash["msg"] = "该账户已过期！";
                                }
                            }
                            else
                            {
                                hash["msg"] = "该账户已被禁止登陆！";
                            }
                        }
                        else
                        {
                            hash["msg"] = "用户名密码不匹配！";
                        }
                    }
                    else
                    {
                        hash["msg"] = "请输入密码！";
                    }
                }
                else
                {
                    hash["msg"] = "请输入用户名！";
                }
            }
            catch (Exception ex)
            {
                hash["msg"] = ex.Message;
            }
            return JsonHelper.ToJson(hash);
        }
        private void SaveCookie(string UserID, string UserName, string UserPassword)
        {
            HttpCookie cookie = new HttpCookie("ChaHuoBao");
            cookie.Value = "ChaHuoBao";
            //cookie.Expires = DateTime.Now+new TimeSpan(0,1,0,0);//不设置这个，表示关闭浏览器
            cookie["yonghuming"] = HttpUtility.UrlEncode(UserName, Encoding.GetEncoding("UTF-8")); ;
            cookie["mima"] = UserPassword;
            cookie["UserID"] = UserID;
            Response.Cookies.Add(cookie);
        }
        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

    }
}
