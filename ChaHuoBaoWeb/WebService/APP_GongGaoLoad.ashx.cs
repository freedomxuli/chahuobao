using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using ChaHuoBaoWeb.Models;
using Common;
using ChaHuoBaoWeb.PublickFunction;
using System.Configuration;

namespace ChaHuoBaoWeb.WebService
{
    /// <summary>
    /// APP_GongGaoLoad 的摘要说明
    /// </summary>
    public class APP_GongGaoLoad : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "获取公告失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<XiTongCanShu> XiTongCanShu_gonggao = db.XiTongCanShu.Where(x => x.Name == "GongGaoModel");
                hash["sign"] = "1";
                hash["gonggaoneirong"] = XiTongCanShu_gonggao.First().Value;
                hash["msg"] = "获取公告成功！";
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