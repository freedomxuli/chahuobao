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
    /// APP_ZhuangHuGuanLi 的摘要说明
    /// </summary>
    public class APP_ZhuangHuGuanLi : IHttpHandler
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
            hash["msg"] = "查询价格策略失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                int UserRemainder = User.First().UserRemainder;
                IEnumerable<JiaGeCeLve> JiaGeCeLve = db.JiaGeCeLve.Where(x => x.JiaGeCeLveLeiXing=="ChongZhi").OrderBy(x=>x.JiaGeCeLveCiShu);
                if (JiaGeCeLve.Count() > 0)
                {
                    hash["sign"] = "1";
                    hash["msg"] = "查询价格策略成功！";
                    hash["JiaGeCeLve"] = JiaGeCeLve;
                    hash["UserRemainder"] = UserRemainder;
                }
                else
                {
                    hash["sign"] = "0";
                    hash["msg"] = "查询价格策略失败！";
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