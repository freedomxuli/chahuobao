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
                        string duration = "";
                        string distance_str = "";
                        foreach (var obj in yundanlist)
                        {
                            obj.QiShiZhan = obj.QiShiZhan.Split(' ')[1].ToString();
                            obj.DaoDaZhan = obj.DaoDaZhan.Split(' ')[1].ToString();
                            if (obj.IsBangding == true)
                            {
                                double distance = GetDistance(Convert.ToDouble(obj.DaoDaZhan_lng.ToString()), Convert.ToDouble(obj.DaoDaZhan_lat.ToString()), Convert.ToDouble(obj.Gps_lastlng.ToString()), Convert.ToDouble(obj.Gps_lastlat.ToString()));
                                distance_str = (distance / 1000).ToString("F2") + "公里";
                                duration = (Convert.ToDecimal((distance / 80000))).ToString("F2") + "小时";
                            }
                        }
                        hash["yundanlist"] = yundanlist;
                        hash["distance"] = distance_str;
                        hash["duration"] = duration;
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

        //地球半径，单位米
        private const double EARTH_RADIUS = 6378137;

        /// <summary>
        /// 计算两点位置的距离，返回两点的距离，单位：米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="lng1">第一点经度</param>
        /// <param name="lat1">第一点纬度</param>        
        /// <param name="lng2">第二点经度</param>
        /// <param name="lat2">第二点纬度</param>
        /// <returns></returns>
        public static double GetDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;
        }

        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }
    }
}