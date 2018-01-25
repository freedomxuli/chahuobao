using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using System.Collections;

namespace ChaHuoBaoWeb.WebService
{
    /// <summary>
    /// AliData 的摘要说明
    /// </summary>
    public class AliData : IHttpHandler
    {
        ChaHuoBaoWeb.Models.ChaHuoBaoModels chbdb;
        public void ProcessRequest(HttpContext context)
        {
            chbdb = new ChaHuoBaoWeb.Models.ChaHuoBaoModels();
            string sign = context.Request["sign"];
            string type = context.Request["type"];
            string type_sub = context.Request["type_sub"];
            if (type == "GetGpsTotalAccount")
            {
                context.Response.Write(GetGpsTotalAccount());

            }

            if (type == "GetOnlineGpsTotalAccount")
            {
                context.Response.Write(GetOnlineGpsTotalAccount());

            }
            if (type == "GetChongzhiJine")
            {
                context.Response.Write(GetChongzhiJine());

            }

            if (type == "GetTodayYunDanAccount")
            {
                context.Response.Write(GetTodayYunDanAccount());

            }

            if (type == "GetUserZhuceliang")
            {
                context.Response.Write(GetUserZhuceliang());

            }



            if (type == "GetSixWeekUserZhuceliang")
            {
                context.Response.Write(GetSixWeekUserZhuceliang());

            }

            if (type == "GetChadanliang")
            {
                context.Response.Write(GetChadanliang());

            }
            if (type == "GetSixWeekChadanliang")
            {
                context.Response.Write(GetSixWeekChadanliang());

            }
            if (type == "GetJiandanliang")
            {
                context.Response.Write(GetJiandanliang());

            }

            if (type == "GetSixWeekJiandanliang")
            {
                context.Response.Write(GetSixWeekJiandanliang());

            }

            if (type == "GetYunyingtabledata")
            {
                context.Response.Write(GetYunyingtabledata());

            }


            if (type == "GetMapPoint")
            {
                context.Response.Write(GetMapPoint());

            }

            if (type == "GetMapLine")
            {
                context.Response.Write(GetMapLine());

            }

            if (type == "GetDeviceAccountByProvince")
            {
                context.Response.Write(GetDeviceAccountByProvince());
            }

            if (type == "GetChaDanAccount")
            {
                context.Response.Write(GetChaDanAccount(type_sub));
            }
            if (type == "GetChaDanList")
            {
                context.Response.Write(GetChaDanList());
            }

            if (type == "GetYunDanLst")
            {
                context.Response.Write(GetYunDanLst());
            }

            if (type == "GetYunDanSumLst")
            {
                context.Response.Write(GetYunDanSumLst());
            }
            if (type == "GetDKCDL")
            {
                context.Response.Write(GetDKCDL());
            }
            if (type == "GetCDLBYTIME")
            {
                context.Response.Write(GetCDLBYTIME());
            }

        }

        private string GetYunDanLst()
        {
            var ynd = chbdb.YunDan.Take(30).OrderByDescending(g => g.BangDingTime).Select(x => new { SuoShuGongSi = x.SuoShuGongSi, UserDenno = x.UserDenno, QiShiZhan = x.QiShiZhan, DaoDaZhan = x.DaoDaZhan, BangDingTime =x.BangDingTime});
            return JsonHelper.ToJson(ynd);
        }
        private string GetYunDanSumLst()
        {
            var ydsumlst = chbdb.YunDan.GroupBy(g => g.SuoShuGongSi).Select(s => new { com = s.Key, count = s.Count() }).OrderByDescending(o=>o.count);
            return JsonHelper.ToJson(ydsumlst);
        }
        private string GetChaDanList()
        {
            IQueryable<ChaHuoBaoWeb.Models.CaoZuoJiLu> shs = chbdb.CaoZuoJiLu.Where(g => g.CaoZuoLeiXing == "自由查单" || g.CaoZuoLeiXing == "我的运单").Take(20).OrderByDescending(g => g.CaoZuoJiLuID);
            return JsonHelper.ToJson(shs);
        }

        private string GetChaDanAccount(string type_sub)
        {
            DateTime dt1 = DateTime.Now.Date;
            DateTime dt2 = DateTime.Now.AddDays(1).Date.AddSeconds(-1);
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;
           IQueryable<ChaHuoBaoWeb.Models.CaoZuoJiLu> czjls= chbdb.CaoZuoJiLu.Where(g => g.CaoZuoLeiXing == "自由查单" || g.CaoZuoLeiXing == "我的运单");
            Hashtable hs = new Hashtable();
            if (type_sub == "all" || string.IsNullOrEmpty(type_sub))
            {
                hs["value"] = czjls.Count();
            }
            if (type_sub == "all_app")
            {
                hs["value"] = czjls.Where(g=>g.CaoZuoNeiRong.Contains("App")).Count();
            }
            if (type_sub == "all_wx")
            {
                hs["value"] = czjls.Where(g => g.CaoZuoNeiRong.Contains("微信")).Count();
            }
            if (type_sub == "all_pc")
            {
                hs["value"] = czjls.Where(g => g.CaoZuoNeiRong.Contains("web")).Count();
            }
            if (type_sub == "today")
            {
                hs["value"] = czjls.Where(g => g.CaoZuoTime > dt1 && g.CaoZuoTime < dt2).Count();
            }
            if (type_sub == "today_app")
            {
                hs["value"] = czjls.Where(g => g.CaoZuoNeiRong.Contains("App") & g.CaoZuoTime > dt1 && g.CaoZuoTime < dt2).Count();
            }
            if (type_sub == "today_wx")
            {
                hs["value"] = czjls.Where(g => g.CaoZuoNeiRong.Contains("微信") & g.CaoZuoTime > dt1 && g.CaoZuoTime < dt2).Count();
            }
            if (type_sub == "today_pc")
            {
                hs["value"] = czjls.Where(g => g.CaoZuoNeiRong.Contains("web") & g.CaoZuoTime > dt1 && g.CaoZuoTime < dt2).Count();
            }
           
            List<Hashtable> result = new List<Hashtable>();
            result.Add(hs);




            return JsonHelper.ToJson(result);
        }

        private string GetGpsTotalAccount()
        {
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;

            Hashtable hs = new Hashtable();
            hs["name"] = "";
            hs["value"] = chbdb.GpsDevice.Count();
            List<Hashtable> result = new List<Hashtable>();
            result.Add(hs);
            return JsonHelper.ToJson(result);
        }
        /// <summary>
        /// 在线GPS总数
        /// </summary>
        /// <returns></returns>
        private string GetOnlineGpsTotalAccount()
        {
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;

            Hashtable hs = new Hashtable();
            hs["name"] = "";
            hs["value"] = chbdb.YunDan.Where(g => g.IsBangding == true && string.IsNullOrEmpty(g.Gps_lastinfo) == false).Count();

            List<Hashtable> result = new List<Hashtable>();
            result.Add(hs);
            return JsonHelper.ToJson(result);
        }

        private string GetChongzhiJine()
        {
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;

            Hashtable hs = new Hashtable();
            hs["name"] = "";
            hs["value"] = chbdb.ChongZhi.Where(g => g.ZhiFuZhuangTai == true).Sum(g => g.ChongZhiJinE);
            List<Hashtable> result = new List<Hashtable>();
            result.Add(hs);
            return JsonHelper.ToJson(result);
        }

        private string GetTodayYunDanAccount()
        {
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;

            Hashtable hs = new Hashtable();
            hs["name"] = "";
            DateTime dt1 = DateTime.Now.Date;
            DateTime dt2 = DateTime.Now.Date.AddDays(1);
            hs["value"] = chbdb.YunDan.Where(g => g.BangDingTime >= dt1 && g.BangDingTime < dt2).Count();
            List<Hashtable> result = new List<Hashtable>();
            result.Add(hs);
            return JsonHelper.ToJson(result);
        }

        private string GetUserZhuceliang()
        {
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;

            Hashtable hs = new Hashtable();
            hs["name"] = "";
            hs["value"] = chbdb.User.Count();
            List<Hashtable> result = new List<Hashtable>();
            result.Add(hs);
            return JsonHelper.ToJson(result);
        }

        private string GetSixWeekUserZhuceliang()
        {
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;
            //获取本周数据

            List<xandy> results = new List<xandy>();

            xandy xy;
            DateTime dt1;
            DateTime dt2;

            dt1 = GetWeekdt1(5);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.User.Where(g => g.UserCreateTime >= dt1 && g.UserCreateTime < dt2).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(4);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.User.Where(g => g.UserCreateTime >= dt1 && g.UserCreateTime < dt2).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(3);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.User.Where(g => g.UserCreateTime >= dt1 && g.UserCreateTime < dt2).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(2);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.User.Where(g => g.UserCreateTime >= dt1 && g.UserCreateTime < dt2).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(1);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.User.Where(g => g.UserCreateTime >= dt1 && g.UserCreateTime < dt2).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(0);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.User.Where(g => g.UserCreateTime >= dt1 && g.UserCreateTime < dt2).Count();
            results.Add(xy);

            return JsonHelper.ToJson(results);

        }

        private string GetChadanliang()
        {
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;

            Hashtable hs = new Hashtable();
            hs["name"] = "";
            hs["value"] = chbdb.CaoZuoJiLu.Where(g => g.CaoZuoLeiXing == "自由查单" || g.CaoZuoLeiXing == "我的运单").Count();
            List<Hashtable> result = new List<Hashtable>();
            result.Add(hs);
            return JsonHelper.ToJson(result);
        }
        private string GetSixWeekChadanliang()
        {
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;
            //获取本周数据

            List<xandy> results = new List<xandy>();

            xandy xy;
            DateTime dt1;
            DateTime dt2;

            dt1 = GetWeekdt1(5);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.CaoZuoJiLu.Where(g => g.CaoZuoTime >= dt1 && g.CaoZuoTime < dt2 && (g.CaoZuoLeiXing == "自由查单" || g.CaoZuoLeiXing == "我的运单")).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(4);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.CaoZuoJiLu.Where(g => g.CaoZuoTime >= dt1 && g.CaoZuoTime < dt2 && (g.CaoZuoLeiXing == "自由查单" || g.CaoZuoLeiXing == "我的运单")).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(3);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.CaoZuoJiLu.Where(g => g.CaoZuoTime >= dt1 && g.CaoZuoTime < dt2 && (g.CaoZuoLeiXing == "自由查单" || g.CaoZuoLeiXing == "我的运单")).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(2);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.CaoZuoJiLu.Where(g => g.CaoZuoTime >= dt1 && g.CaoZuoTime < dt2 && (g.CaoZuoLeiXing == "自由查单" || g.CaoZuoLeiXing == "我的运单")).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(1);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.CaoZuoJiLu.Where(g => g.CaoZuoTime >= dt1 && g.CaoZuoTime < dt2 && (g.CaoZuoLeiXing == "自由查单" || g.CaoZuoLeiXing == "我的运单")).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(0);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.CaoZuoJiLu.Where(g => g.CaoZuoTime >= dt1 && g.CaoZuoTime < dt2 && (g.CaoZuoLeiXing == "自由查单" || g.CaoZuoLeiXing == "我的运单")).Count();
            results.Add(xy);

            return JsonHelper.ToJson(results);

        }

        private string GetJiandanliang()
        {
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;

            Hashtable hs = new Hashtable();
            hs["name"] = "";
            hs["value"] = chbdb.YunDan.Count();
            List<Hashtable> result = new List<Hashtable>();
            result.Add(hs);
            return JsonHelper.ToJson(result);
        }

        private string GetSixWeekJiandanliang()
        {
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;
            //获取本周数据

            List<xandy> results = new List<xandy>();

            xandy xy;
            DateTime dt1;
            DateTime dt2;

            dt1 = GetWeekdt1(5);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.YunDan.Where(g => g.BangDingTime >= dt1 && g.BangDingTime < dt2).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(4);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.YunDan.Where(g => g.BangDingTime >= dt1 && g.BangDingTime < dt2).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(3);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.YunDan.Where(g => g.BangDingTime >= dt1 && g.BangDingTime < dt2).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(2);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.YunDan.Where(g => g.BangDingTime >= dt1 && g.BangDingTime < dt2).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(1);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.YunDan.Where(g => g.BangDingTime >= dt1 && g.BangDingTime < dt2).Count();
            results.Add(xy);

            dt1 = GetWeekdt1(0);
            dt2 = dt1.AddDays(7).AddSeconds(-1);
            xy = new xandy();
            xy.x = dt1.Date.ToString("MM-dd");
            xy.y = chbdb.YunDan.Where(g => g.BangDingTime >= dt1 && g.BangDingTime < dt2).Count();
            results.Add(xy);

            return JsonHelper.ToJson(results);

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private DateTime GetWeekdt1(int week)
        {
            DateTime dt1 = DateTime.Now.Date;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                dt1 = DateTime.Now.Date;
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            {
                dt1 = DateTime.Now.Date.AddDays(-1);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
            {
                dt1 = DateTime.Now.Date.AddDays(-2);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
            {
                dt1 = DateTime.Now.Date.AddDays(-3);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                dt1 = DateTime.Now.Date.AddDays(-4);
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                dt1 = DateTime.Now.Date.AddDays(-5);
            }

            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                dt1 = DateTime.Now.Date.AddDays(-6);
            }

            return dt1.AddDays(-7 * week);

        }

        private string GetYunyingtabledata()
        {
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;

            Hashtable hs = new Hashtable();
            var yajinzongeval = chbdb.GpsDingDan.Where(g => g.GpsDingDanZhiFuZhuangTai == true).ToList().Sum(g => g.GpsDingDanJinE);
            decimal yajinzonge = 0;
            if (yajinzongeval != null)
            {
                yajinzonge = (decimal)(yajinzongeval);
            }
            var tuihuanyajinval = chbdb.GpsTuiDan.Where(g => g.GpsTuiDanShenHeZhuangTai == true).ToList().Sum(g => g.GpsTuiDanJinE);
            decimal tuihuanyajin = 0;
            if (tuihuanyajinval != null)
            {
                tuihuanyajin = (decimal)(tuihuanyajinval);
            }



            Int32 chongzhidanliang = chbdb.ChongZhi.Where(g => g.ZhiFuZhuangTai == true).Sum(g => g.ChongZhiCiShu);
            Int32 xiaohaodanliang = chbdb.YunDan.Count();
            Int32 shengyudanliang = chongzhidanliang - xiaohaodanliang;

            List<Hashtable> result = new List<Hashtable>();
            hs = new Hashtable();
            hs.Add("押金总额", yajinzonge);
            result.Add(hs);

            hs = new Hashtable();
            hs.Add("剩余押金", yajinzonge - tuihuanyajin);
            result.Add(hs);

            hs = new Hashtable();
            hs.Add("充值单量", chongzhidanliang);
            result.Add(hs);

            hs = new Hashtable();
            hs.Add("消耗单量", xiaohaodanliang);
            result.Add(hs);

            hs = new Hashtable();
            hs.Add("剩余单量", shengyudanliang);
            result.Add(hs);








            return JsonHelper.ToJson(result);
        }

        private string GetMapPoint()
        {
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;

            var result = from x in chbdb.YunDan where x.IsBangding == true select new { lat = x.Gps_lastlat, lng = x.Gps_lastlng, value = 1, type = 1 };

            return JsonHelper.ToJson(result);
        }
        private string GetMapLine()
        {
            //            "from": "116.85059,31.69078",
            //"to": "118.69629,36.20882",
            //"fromInfo": "起点",
            //"toInfo": "终点"
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;
            List<Hashtable> hss = new List<Hashtable>();
            Hashtable hs = new Hashtable();
            IEnumerable<ChaHuoBaoWeb.Models.YunDan> yundans = from x in chbdb.YunDan where x.IsBangding == true select x;
            foreach (ChaHuoBaoWeb.Models.YunDan yd in yundans)
            {
                hs = new Hashtable();
                string fromgps = yd.QiShiZhan_lng + "," + yd.QiShiZhan_lat;
                string togps = yd.DaoDaZhan_lng + "," + yd.DaoDaZhan_lat;
                if (string.IsNullOrEmpty(yd.QiShiZhan_lng))
                {
                    hs["from"] = new ChaHuoBaoWeb.PublickFunction.Map().getmapinfobyaddress(yd.QiShiZhan, "")["location"].ToString();
                }
                else
                {
                    hs["from"] = fromgps;
                }
                if (string.IsNullOrEmpty(yd.DaoDaZhan_lng))
                {

                    hs["to"] = new ChaHuoBaoWeb.PublickFunction.Map().getmapinfobyaddress(yd.DaoDaZhan, "")["location"].ToString();
                }
                else
                {
                    hs["to"] = togps;
                }
                hs["fromInfo"] = yd.QiShiZhan;
                hs["toInfo"] = yd.DaoDaZhan;
                hss.Add(hs);

            }
            return JsonHelper.ToJson(hss);
        }

        private string GetDeviceAccountByProvince()
        {
            //IEnumerable<ChaHuoBaoWeb.Models.GpsDevice> devices = from x in chbdb.GpsDevice select x;
            List<deviceandaccount> lst = new List<deviceandaccount>();
           
            lst.Add(new deviceandaccount{area ="江苏省",  account=0});
            lst.Add(new deviceandaccount{area ="河北省",  account=0});
            lst.Add(new deviceandaccount{area ="辽宁省",  account=0});
            lst.Add(new deviceandaccount{area ="吉林省",  account=0});
            lst.Add(new deviceandaccount{area ="黑龙江省",  account=0});
            lst.Add(new deviceandaccount{area ="浙江省",  account=0});
            lst.Add(new deviceandaccount{area ="安徽省",  account=0});
            lst.Add(new deviceandaccount{area ="福建省",  account=0});
            lst.Add(new deviceandaccount{area ="江西省",  account=0});
            lst.Add(new deviceandaccount{area ="河南省",  account=0});
            lst.Add(new deviceandaccount{area ="湖北省",  account=0});
            lst.Add(new deviceandaccount{area ="湖南省",  account=0});
            lst.Add(new deviceandaccount{area ="广东省",  account=0});
            lst.Add(new deviceandaccount{area ="海南省",  account=0});
            lst.Add(new deviceandaccount{area ="四川省",  account=0});
            lst.Add(new deviceandaccount{area ="贵州省",  account=0});
            lst.Add(new deviceandaccount{area ="云南省",  account=0});
            lst.Add(new deviceandaccount{area ="陕西省",  account=0});
            lst.Add(new deviceandaccount{area ="甘肃省",  account=0});
            lst.Add(new deviceandaccount{area ="青海省",  account=0});
            lst.Add(new deviceandaccount{area ="台湾省",  account=0});
            lst.Add(new deviceandaccount{area ="北京市",  account=0});
            lst.Add(new deviceandaccount{area ="天津市",  account=0});
            lst.Add(new deviceandaccount{area ="重庆市",  account=0});
            lst.Add(new deviceandaccount{area ="上海市",  account=0});
            lst.Add(new deviceandaccount{area ="广西壮族自治区",  account=0});
            lst.Add(new deviceandaccount{area ="内蒙古自治区",  account=0});
            lst.Add(new deviceandaccount{area ="宁夏回族自治区",  account=0});
            lst.Add(new deviceandaccount{area ="西藏自治区",  account=0});
            lst.Add(new deviceandaccount{area ="新疆维吾尔自治区",  account=0});
            lst.Add(new deviceandaccount{area ="香港特别行政区",  account=0});
            lst.Add(new deviceandaccount{area ="澳门特别行政区",  account=0});
            var result = from x in chbdb.YunDan where x.IsBangding == true select x;
            foreach (ChaHuoBaoWeb.Models.YunDan ydmodel in result)
            {
                foreach (deviceandaccount da in lst)
                {
                    if (ydmodel.Gps_lastinfo.Contains(da.area))
                    {
                        da.account += 1;
                    }
                }
            }
            lst = lst.OrderByDescending(g => g.account).ToList();
            return JsonHelper.ToJson(lst);
        }

        private string GetDKCDL()
        {
            IQueryable<ChaHuoBaoWeb.Models.CaoZuoJiLu> czjls = chbdb.CaoZuoJiLu.Where(g => g.CaoZuoLeiXing == "自由查单" || g.CaoZuoLeiXing == "我的运单");
            List<dkcdl> lst = new List<dkcdl>();
            lst.Add(new dkcdl { x = "PC", y = czjls.Where(g => g.CaoZuoNeiRong.Contains("web")).Count(), s = "1" });
            lst.Add(new dkcdl { x = "APP", y = czjls.Where(g => g.CaoZuoNeiRong.Contains("App")).Count(), s = "1" });
            lst.Add(new dkcdl { x = "微信", y = czjls.Where(g => g.CaoZuoNeiRong.Contains("微信")).Count(), s = "1" });
            return JsonHelper.ToJson(lst);
        }

        private string GetCDLBYTIME()
        {
            IQueryable<ChaHuoBaoWeb.Models.CaoZuoJiLu> czjls = chbdb.CaoZuoJiLu.Where(g => g.CaoZuoLeiXing == "自由查单" || g.CaoZuoLeiXing == "我的运单");
            List<dkcdl> lst = new List<dkcdl>();

            int week = (int)DateTime.Today.DayOfWeek;
            if (week == 0) week = 7; //周日
            DateTime beginDate = DateTime.Today.AddDays(-(week - 1));
            DateTime endDate = beginDate.AddDays(6);

            DateTime beginDateM = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime endDateM = beginDate.AddMonths(1).AddDays(-1);

            lst.Add(new dkcdl { x = "周", y = czjls.Where(g => g.CaoZuoTime >= beginDate && g.CaoZuoTime < endDate).Count(), s = "1" });
            lst.Add(new dkcdl { x = "月", y = czjls.Where(g => g.CaoZuoTime >= beginDateM && g.CaoZuoTime < endDateM).Count(), s = "1" });
            lst.Add(new dkcdl { x = "总额", y = czjls.Where(g => g.CaoZuoTime < DateTime.Now).Count(), s = "1" });
            return JsonHelper.ToJson(lst);
        }
    }


    public class deviceandaccount
    {
        public string area { get; set; }
        public Int32 account { get; set; }
    }

    public class alimapdata
    {
        public string lat { get; set; }
        public string lng { get; set; }
        public string value { get; set; }
        public string type { get; set; }
    }
    /// <summary>
    /// Name Value 键值对
    /// </summary>
    public class nameandvalue
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    /// <summary>
    /// x y 键值对
    /// </summary>
    public class xandy
    {
        public string x { get; set; }
        public Int32 y { get; set; }
    }
    /// <summary>
    /// 查单量
    /// </summary>
    public class dkcdl
    {
        public string x { get; set; }
        public Int32 y { get; set; }
        public string s { get; set; }
    }
    /// <summary>
    ///  在线GPS总量
    /// </summary>
    //public class aliGPS_LiveAccount
    //{
    //    public string name { get; set; }
    //    public string value { get; set; }
    //}

    /// <summary>
    /// 注册量
    /// </summary>
    //    public class aliGPS_ZhuceAccount
    //{
    //    public string name { get; set; }
    //    public string value { get; set; }
    //}



    //public class aligenzongdata
    //{
    //    public string wangdian { get; set; }
    //    public DateTime  dt { get; set; }
    //    public string genzong { get; set; }
    //    public string pingtaidanhao { get; set; }
    //}

    //public class ykdata
    //{
    //    public decimal value { get; set; }
    //}


    //    "lat": 31.8998,
    //"lng": 102.2212,
    //"value": 1,
    //"type": 1
}