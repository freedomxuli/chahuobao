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
    /// APP_JieChuBangDing 的摘要说明
    /// </summary>
    public class APP_JieChuBangDing : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserID = context.Request["UserID"];
            //用户密码
            string YunDanDenno = context.Request["YunDanDenno"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "解除绑定失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<YunDan> YunDan = db.YunDan.Where(x => x.UserID == UserID && x.YunDanDenno == YunDanDenno);
                if (YunDan.Count() > 0)
                {
                    if (YunDan.First().IsBangding == true)
                    {
                        //YunDan.First().GpsDeviceID = "";
                        YunDan.First().GpsDevicevid = "";
                        YunDan.First().GpsDevicevKey = "";
                        YunDan.First().JieBangTime = DateTime.Now;
                        YunDan.First().IsBangding = false;

                        //添加 操作记录
                        CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                        CaoZuoJiLu.UserID = UserID;
                        CaoZuoJiLu.CaoZuoLeiXing = "解除绑定";
                        CaoZuoJiLu.CaoZuoNeiRong = "APP内用户运单解除设备绑定，运单系统单号：" + YunDanDenno + "。";
                        CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                        CaoZuoJiLu.CaoZuoRemark = "";
                        db.CaoZuoJiLu.Add(CaoZuoJiLu);


                        db.SaveChanges();
                        hash["sign"] = "1";
                        hash["msg"] = "解除绑定成功！";
                    }
                    else
                    {
                        hash["sign"] = "0";
                        hash["msg"] = "该运单已被解绑！";
                    }
                }
                else
                {
                    hash["sign"] = "0";
                    hash["msg"] = "解除绑定失败,为查询到该运单！";
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