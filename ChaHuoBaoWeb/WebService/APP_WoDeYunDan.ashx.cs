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
    /// APP_WoDeYunDan 的摘要说明
    /// </summary>
    public class APP_WoDeYunDan : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            string UserDenno = context.Request["UserDenno"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "搜索我的运单失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                if (User.Count() > 0)
                {
                    IEnumerable<YunDan> YunDan_list = db.YunDan.Where(x => x.UserID == UserID && x.UserDenno.Contains(UserDenno)).OrderByDescending(x=>x.BangDingTime);
                    if (YunDan_list.Count() > 0)
                    {
                        //添加 操作记录
                        CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                        CaoZuoJiLu.UserID = UserID;
                        CaoZuoJiLu.CaoZuoLeiXing = "我的运单";
                        CaoZuoJiLu.CaoZuoNeiRong = "APP内用户我的运单查询，搜索单号：" + UserDenno + "。";
                        CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                        CaoZuoJiLu.CaoZuoRemark = "";
                        db.CaoZuoJiLu.Add(CaoZuoJiLu);
                        db.SaveChanges();


                        hash["sign"] = "1";
                        hash["msg"] = "搜索我的运单成功";

                        var yundanlist = YunDan_list.ToList();
                        foreach (var obj in yundanlist)
                        {
                            obj.QiShiZhan = obj.QiShiZhan.Split(' ')[1].ToString();
                            obj.DaoDaZhan = obj.DaoDaZhan.Split(' ')[1].ToString();
                        }
                        hash["yundanlist"] = yundanlist;
                    }
                    else 
                    {
                        hash["sign"] = "2";
                        hash["msg"] = "我的运单为空";
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
    }
}