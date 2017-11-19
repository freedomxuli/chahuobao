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
    /// APP_ShanChuTuiDan 的摘要说明
    /// </summary>
    public class APP_ShanChuTuiDan : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            string OrderDenno = context.Request["OrderDenno"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "删除该条退单记录失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                if (User.Count() > 0)
                {
                    string UserID = User.First().UserID;
                    IEnumerable<GpsTuiDan> GpsTuiDan = db.GpsTuiDan.Where(x => x.UserID == UserID && x.OrderDenno == OrderDenno);
                    if (GpsTuiDan.Count() > 0)
                    {
                        db.GpsTuiDan.Remove(GpsTuiDan.First());

                        //添加 操作记录
                        CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                        CaoZuoJiLu.UserID = UserID;
                        CaoZuoJiLu.CaoZuoLeiXing = "删除退单列表";
                        CaoZuoJiLu.CaoZuoNeiRong = "APP内用户删除退单列表，预设支付单号：" + OrderDenno + "。";
                        CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                        CaoZuoJiLu.CaoZuoRemark = "";
                        db.CaoZuoJiLu.Add(CaoZuoJiLu);


                        db.SaveChanges();
                        hash["sign"] = "1";
                        hash["msg"] = "成功删除该条退单！";
                    }
                    else
                    {
                        hash["sign"] = "0";
                        hash["msg"] = "删除失败，不存在该条退单！";
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