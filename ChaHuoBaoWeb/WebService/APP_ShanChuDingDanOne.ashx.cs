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
    /// APP_ShanChuDingDanOne 的摘要说明
    /// </summary>
    public class APP_ShanChuDingDanOne : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            int GpsDingDanMingXiID =Convert.ToInt32( context.Request["GpsDingDanMingXiID"]);
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "删除该条订单记录失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                if (User.Count() > 0)
                {
                    string UserID = User.First().UserID;
                    IEnumerable<GpsDingDanMingXi> GpsDingDanMingXi = db.GpsDingDanMingXi.Where(x => x.GpsDingDanMingXiID == GpsDingDanMingXiID);
                    if (GpsDingDanMingXi.Count() > 0)
                    {
                        db.GpsDingDanMingXi.Remove(GpsDingDanMingXi.First());
                        db.SaveChanges();
                        hash["sign"] = "1";
                        hash["msg"] = "成功删除该条订单记录！";
                    }
                    else
                    {
                        hash["sign"] = "0";
                        hash["msg"] = "删除失败，不存在该条订单记录！";
                    }
                }
                else 
                {
                    hash["sign"] = "0";
                    hash["msg"] = "删除失败，用户不存在！";
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