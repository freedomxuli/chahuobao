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
    /// APP_ChaXunTuiDanMingXi 的摘要说明
    /// </summary>
    public class APP_ChaXunTuiDanMingXi : IHttpHandler
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
            hash["msg"] = "查询未提交退单明细失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                IEnumerable<GpsTuiDan> GpsTuiDan = db.GpsTuiDan.Where(x => x.GpsTuiDanIsEnd == false && x.UserID == UserID);
                if (GpsTuiDan.Count() > 0)
                {
                    string GpsTuiDanDenno = GpsTuiDan.First().GpsTuiDanDenno;
                    hash["OrderDenno"] = GpsTuiDan.First().OrderDenno;
                    IEnumerable<GpsTuiDanMingXi> GpsTuiDanMingXi_list = db.GpsTuiDanMingXi.Where(x => x.GpsTuiDanDenno == GpsTuiDanDenno);
                    if (GpsTuiDanMingXi_list.Count() > 0)
                    {
                        hash["sign"] = "1";
                        hash["msg"] = "查询未提交订单明细成功！";
                        hash["GpsTuiDanMingXi"] = GpsTuiDanMingXi_list;
                    }
                    else
                    {
                        hash["sign"] = "2";
                        hash["msg"] = "该未提交订单内未扫描任何设备！";
                    }
                }
                else
                {
                    hash["sign"] = "2";
                    hash["msg"] = "没有未提交订单！";
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