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
    /// APP_ZiYouChaDan 的摘要说明
    /// </summary>
    public class APP_ZiYouChaDan : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];//有值：APP内自由查单  无值：微信公众号运单查询
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            string UserDenno = context.Request["UserDenno"];
            string SuoShuGongSi = context.Request["SuoShuGongSi"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "搜索自由查单失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                if (string.IsNullOrEmpty(UserName))
                {
                    IEnumerable<YunDan> YunDan_list = db.YunDan.Where(x => x.UserDenno == UserDenno && x.SuoShuGongSi.Contains(SuoShuGongSi)).OrderByDescending(x => x.BangDingTime);

                    if (YunDan_list.Count() > 0)
                    {
                        YunDan yundan_one = YunDan_list.First();
                        IEnumerable<User> User = db.User.Where(x => x.UserID == yundan_one.UserID && x.UserLeiXing == "APP");
                        if (User.Count() > 0)
                        {
                            bool weixinchaxun = User.First().UserWxEnable;
                            if (weixinchaxun)
                            {
                                string UserID = User.First().UserID;
                                //添加 操作记录
                                CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                                CaoZuoJiLu.UserID = UserID;
                                CaoZuoJiLu.CaoZuoLeiXing = "自由查单";
                                CaoZuoJiLu.CaoZuoNeiRong = "微信内用户运单查询，搜索单号：" + UserDenno + "；搜索公司：" + SuoShuGongSi + "。";
                                CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                                CaoZuoJiLu.CaoZuoRemark = "";
                                db.CaoZuoJiLu.Add(CaoZuoJiLu);
                                db.SaveChanges();

                                hash["sign"] = "1";
                                hash["msg"] = "搜索自由查单成功";


                                var yundanlist = YunDan_list.ToList();
                                foreach (var obj in yundanlist)
                                {
                                    //obj.QiShiZhan = obj.QiShiZhan.Split(' ')[1].ToString();
                                    //obj.DaoDaZhan = obj.DaoDaZhan.Split(' ')[1].ToString();

                                    IEnumerable<YunDanDistance> YunDanDistance = db.YunDanDistance.Where(x => x.YunDanDenno == obj.YunDanDenno);
                                    if (YunDanDistance.Count() > 0)
                                    {
                                        obj.Gps_distance = YunDanDistance.First().Gps_distance;
                                        obj.Gps_duration = YunDanDistance.First().Gps_duration;
                                    }
                                }
                                hash["yundanlist"] = yundanlist;
                            }
                            else
                            {
                                hash["sign"] = "0";
                                hash["msg"] = "用户未开启微信查询功能";
                            }
                        }
                        else
                        {
                            hash["sign"] = "0";
                            hash["msg"] = "用户不存在";
                        }
                    }
                    else
                    {
                        hash["sign"] = "2";
                        hash["msg"] = "自由为空";
                    }
                }
                else
                {
                    IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                    string UserID = User.First().UserID;
                    if (User.Count() > 0)
                    {
                        IEnumerable<YunDan> YunDan_list = db.YunDan.Where(x => x.UserDenno == UserDenno && x.SuoShuGongSi.Contains(SuoShuGongSi)).OrderByDescending(x => x.BangDingTime);
                        if (YunDan_list.Count() > 0)
                        {
                            //添加 操作记录
                            CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                            CaoZuoJiLu.UserID = UserID;
                            CaoZuoJiLu.CaoZuoLeiXing = "自由查单";
                            CaoZuoJiLu.CaoZuoNeiRong = "APP内用户自由查单查询，搜索单号：" + UserDenno + "；搜索公司：" + SuoShuGongSi + "。";
                            CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                            CaoZuoJiLu.CaoZuoRemark = "";
                            db.CaoZuoJiLu.Add(CaoZuoJiLu);
                            db.SaveChanges();

                            //自由查单成功，像搜索历史表中添加公司历史
                            IEnumerable<SearchHistory> SearchHistory = db.SearchHistory.Where(x => x.UserID == UserID && x.Type == "自由查单_公司" && x.Value == SuoShuGongSi);
                            if (SearchHistory.Count() == 0)
                            {
                                SearchHistory SearchHistory_new = new SearchHistory();
                                SearchHistory_new.UserID = UserID;
                                SearchHistory_new.Type = "自由查单_公司";
                                SearchHistory_new.Value = SuoShuGongSi;
                                db.SearchHistory.Add(SearchHistory_new);
                                db.SaveChanges();
                            }

                            hash["sign"] = "1";
                            hash["msg"] = "搜索自由查单成功";

                            var yundanlist = YunDan_list.ToList();
                            foreach (var obj in yundanlist)
                            {
                                obj.QiShiZhan = obj.QiShiZhan.Split(' ')[1].ToString();
                                obj.DaoDaZhan = obj.DaoDaZhan.Split(' ')[1].ToString();

                                IEnumerable<YunDanDistance> YunDanDistance = db.YunDanDistance.Where(x => x.YunDanDenno == obj.YunDanDenno);
                                if (YunDanDistance.Count() > 0)
                                {
                                    obj.Gps_distance = YunDanDistance.First().Gps_distance;
                                    obj.Gps_duration = YunDanDistance.First().Gps_duration;
                                }
                            }
                            hash["yundanlist"] = yundanlist;
                        }
                        else
                        {
                            hash["sign"] = "2";
                            hash["msg"] = "自由为空";
                        }
                    }
                    else
                    {
                        hash["sign"] = "0";
                        hash["msg"] = "用户不存在";
                    }
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