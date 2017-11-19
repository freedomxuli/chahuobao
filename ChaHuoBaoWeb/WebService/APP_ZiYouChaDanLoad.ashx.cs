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
    /// APP_ZiYouChaDanLoad 的摘要说明
    /// </summary>
    public class APP_ZiYouChaDanLoad : IHttpHandler
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
            hash["msg"] = "搜索历史公司数据失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                List<gongsi> gongsis = new List<gongsi>();
                if (User.Count() > 0)
                {
                    var result = db.SearchHistory.Where(x => x.UserID == UserID && x.Type=="自由查单_公司").GroupBy(x => new { x.Value }).Select(g => new
                    {
                        SuoShuGongSi = g.Key.Value
                    });
                    if (result.Count() > 0)
                    {
                        foreach (var obj in result)
                        {
                            gongsi gongsi = new gongsi();
                            gongsi.text = obj.SuoShuGongSi;
                            gongsis.Add(gongsi);
                        }
                        hash["sign"] = "1";
                        hash["msg"] = "搜索历史公司数据成功";
                        hash["gongsis"] = gongsis;
                    }
                    else
                    {
                        hash["sign"] = "2";
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
        public class gongsi
        {
            public string text { get; set; }
        }
    }
}