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
    public class APP_ZhiDanMessage : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            Encoding utf8 = Encoding.UTF8;
            string fileText = context.Request["fileText"];
            string daodadi = context.Request["daodadi"];
            string company = context.Request["company"];
            string code = context.Request["code"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "发送失败！";
            try
            {
                new GetYanZhengMa().zhidanmessage(fileText, daodadi, company, code);
                hash["sign"] = "1";
                hash["msg"] = "发送成功！";
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