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
    /// APP_ChaXunDingDanMingXi 的摘要说明
    /// </summary>
    public class APP_ChaXunDingDanSaleMingXi : IHttpHandler
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
            hash["msg"] = "查询未提交销售订单明细失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                IEnumerable<GpsDingDanSale> GpsDingDan = db.GpsDingDanSale.Where(x => x.GpsDingDanIsEnd == false && x.UserID == UserID);
                if (GpsDingDan.Count() > 0)
                {
                    string GpsDingDanDenno = GpsDingDan.First().GpsDingDanDenno;
                    hash["OrderDenno"] = GpsDingDan.First().OrderDenno;
                    IEnumerable<GpsDingDanSaleMingXi> GpsDingDanMingXi_list = db.GpsDingDanSaleMingXi.Where(x => x.GpsDingDanDenno == GpsDingDanDenno);
                    if (GpsDingDanMingXi_list.Count() > 0)
                    {
                        hash["sign"] = "1";
                        hash["msg"] = "查询未提交销售订单明细成功！";
                        hash["GpsDingDanMingXi"] = GpsDingDanMingXi_list;
                    }
                    else
                    {
                        hash["sign"] = "2";
                        hash["msg"] = "该未提交销售订单内未扫描任何设备！";
                    }
                }
                else
                {
                    hash["sign"] = "2";
                    hash["msg"] = "没有未提交销售订单！";
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