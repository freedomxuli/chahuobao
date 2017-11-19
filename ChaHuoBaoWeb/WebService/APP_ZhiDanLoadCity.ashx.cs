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
    /// APP_ZhiDanLoadCity 的摘要说明
    /// </summary>
    public class APP_ZhiDanLoadCity : IHttpHandler
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
            hash["msg"] = "搜索用户所在城市失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                if (User.Count() > 0)
                {
                    hash["sign"] = "1";
                    hash["msg"] = "搜索用户所在城市成功";
                    hash["User"] = User.First();
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