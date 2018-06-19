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
    /// APP_GetYanZhengMa 的摘要说明
    /// </summary>
    public class APP_GetYanZhengMa : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            //操作类型
            string type = context.Request["type"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "获取验证码失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName);
                if (type == "zhuce")
                {
                    if (User.Count() > 0)
                    {
                        hash["sign"] = "0";
                        hash["msg"] = "用户已存在，无需重新注册！";
                    }
                    else
                    {
                        GetYanZhengMa getyanzhenma = new GetYanZhengMa();
                        string yanzhengma = getyanzhenma.yanzhengma("查货宝", "SMS_137666565", UserName);
                        if (string.IsNullOrEmpty(yanzhengma) == false)
                        {
                            if (yanzhengma.Length == 6)
                            {
                                hash["sign"] = "1";
                                hash["msg"] = "获取验证码成功！";
                                hash["yanzhengma"] = yanzhengma;
                            }
                        }
                    }
                }

                if (type == "chongzhimima")
                {
                    if (User.Count() > 0)
                    {
                        GetYanZhengMa getyanzhenma = new GetYanZhengMa();
                        string yanzhengma = getyanzhenma.yanzhengma("查货宝", "SMS_90005025", UserName);
                        if (string.IsNullOrEmpty(yanzhengma) == false)
                        {
                            if (yanzhengma.Length == 6)
                            {
                                hash["sign"] = "1";
                                hash["msg"] = "获取验证码成功！";
                                hash["yanzhengma"] = yanzhengma;
                            }
                        }
                    }
                    else
                    {
                        hash["sign"] = "0";
                        hash["msg"] = "未查询到用户，密码重置失败！";
                    }
                }


                if (type == "tuidanshenqing")
                {
                    if (User.Count() > 0)
                    {
                        GetYanZhengMa getyanzhenma = new GetYanZhengMa();
                        string yanzhengma = getyanzhenma.yanzhengma("查货宝", "SMS_90005025", UserName);
                        if (string.IsNullOrEmpty(yanzhengma) == false)
                        {
                            if (yanzhengma.Length == 6)
                            {
                                hash["sign"] = "1";
                                hash["msg"] = "获取验证码成功！";
                                hash["yanzhengma"] = yanzhengma;
                            }
                        }
                    }
                    else
                    {
                        hash["sign"] = "0";
                        hash["msg"] = "未查询到用户，密码重置失败！";
                    }
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