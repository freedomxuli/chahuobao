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
    /// APP_ZhiDan2 的摘要说明
    /// </summary>
    public class APP_ZhiDan2 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            string QiShiZhan = context.Request["QiShiZhan"];
            string DaoDaZhan = context.Request["DaoDaZhan"];
            string SuoShuGongSi = context.Request["SuoShuGongSi"];
            string UserDenno = context.Request["UserDenno"];
            string GpsDeviceID = context.Request["GpsDeviceID"];
            string YunDanRemark = context.Request["YunDanRemark"];
            string Expect_Hour = context.Request["Expect_Hour"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "制单失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                int UserRemainder = User.First().UserRemainder;
                IEnumerable<YunDan> YunDan_GPS = db.YunDan.Where(x => x.GpsDeviceID == GpsDeviceID && x.IsBangding == true);
                if (User.Count() > 0)
                {
                    IEnumerable<GpsDeviceSale> GpsDeviceSale = db.GpsDeviceSale.Where(x => x.GpsDeviceID == GpsDeviceID);
                    int sale_num = GpsDeviceSale.Count();
                    bool IsCanSale = GpsDeviceID.Substring(0, 4) == "2020" ? true : false;
                    if (YunDan_GPS.Count() > 0)
                    {
                        string YunDan_GPS_userid = YunDan_GPS.First().UserID;
                        if (YunDan_GPS_userid == UserID)
                        {
                            //YunDan_GPS.First().GpsDeviceID = "";
                            YunDan_GPS.First().GpsDevicevid = "";
                            YunDan_GPS.First().GpsDevicevKey = "";
                            YunDan_GPS.First().JieBangTime = DateTime.Now;
                            YunDan_GPS.First().IsBangding = false;
                            db.SaveChanges();
                            //设备已定位自己的单子，可自动解绑然后绑定另外运单
                            hash = create_yundan(sale_num, IsCanSale, UserRemainder, UserID, QiShiZhan, DaoDaZhan, SuoShuGongSi, UserDenno, GpsDeviceID, YunDanRemark, Expect_Hour);
                            if (hash["sign"].ToString() == "1")
                            {
                                if (sale_num == 0)
                                    User.First().UserRemainder = UserRemainder - 1;

                                //添加 操作记录
                                CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                                CaoZuoJiLu.UserID = UserID;
                                CaoZuoJiLu.CaoZuoLeiXing = "制单";
                                CaoZuoJiLu.CaoZuoNeiRong = "APP内用户制单，单号：" + UserDenno + "；起始站：" + QiShiZhan + "；到达站：" + DaoDaZhan + "；所属公司：" + SuoShuGongSi + "；设备id:" + GpsDeviceID + "。";
                                CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                                CaoZuoJiLu.CaoZuoRemark = "";
                                db.CaoZuoJiLu.Add(CaoZuoJiLu);


                                db.SaveChanges();
                                if (sale_num==0)
                                    hash["msg"] = "制单成功（自动解绑，更新运单）,剩余制单数：" + (UserRemainder - 1) + "!";
                                else
                                    hash["msg"] = "制单成功（自动解绑，更新运单）";
                            }
                        }
                        else
                        {
                            hash["sign"] = "0";
                            hash["msg"] = "制单失败（设备已被绑定到其他用户）";
                        }
                    }
                    else
                    {
                        //设备未绑定，运单直接绑定该设备
                        hash = create_yundan(sale_num, IsCanSale, UserRemainder, UserID, QiShiZhan, DaoDaZhan, SuoShuGongSi, UserDenno, GpsDeviceID, YunDanRemark, Expect_Hour);
                        if (hash["sign"].ToString() == "1")
                        {
                            if (sale_num == 0)
                                User.First().UserRemainder = UserRemainder - 1;


                            //添加 操作记录
                            CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                            CaoZuoJiLu.UserID = UserID;
                            CaoZuoJiLu.CaoZuoLeiXing = "制单";
                            CaoZuoJiLu.CaoZuoNeiRong = "APP内用户制单，单号：" + UserDenno + "；起始站：" + QiShiZhan + "；到达站：" + DaoDaZhan + "；所属公司：" + SuoShuGongSi + "；设备id:" + GpsDeviceID + "。";
                            CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                            CaoZuoJiLu.CaoZuoRemark = "";
                            db.CaoZuoJiLu.Add(CaoZuoJiLu);


                            db.SaveChanges();
                            if (sale_num == 0)
                                hash["msg"] = "制单成功,剩余制单数：" + (UserRemainder - 1) + "!";
                            else
                                hash["msg"] = "制单成功";
                        }
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

        private Hashtable create_yundan(int sale_num,bool IsCanSale,int UserRemainder, string UserID, string QiShiZhan, string DaoDaZhan, string SuoShuGongSi, string UserDenno, string GpsDeviceID, string YunDanRemark, string Expect_Hour)
        {
            Hashtable hash = new Hashtable();
            try
            {
                ChaHuoBaoModels db2 = new ChaHuoBaoModels();

                IEnumerable<YunDan> yundan = db2.YunDan.Where(x => x.UserID == UserID && x.UserDenno == UserDenno);
                if (yundan.Count() > 0)
                {
                    hash["sign"] = "0";
                    hash["msg"] = "制单失败，单号已存在！";
                }
                else
                {
                    if (sale_num == 0)
                    {
                        if (IsCanSale)
                        {
                            hash["sign"] = "0";
                            hash["msg"] = "制单失败，请先购买该设备！";
                        }
                        else
                        {
                            if (UserRemainder > 0)
                            {
                                GetVidVkey gvdk = new GetVidVkey();
                                hash = gvdk.gvk(GpsDeviceID);
                                string gpsvid = "";
                                string gpsvkey = "";
                                if (hash["sign"].ToString() == "1")
                                {
                                    gpsvid = hash["vid"].ToString();
                                    gpsvkey = hash["vKey"].ToString();
                                }

                                ChaHuoBaoWeb.MvcApplication.log4nethelper.Info("APP制单操作=>GPSID：" + GpsDeviceID + ";制单时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ";hash_sign" + hash["sign"].ToString() + ";gpsvid:" + gpsvid + ";gpsvkey:" + gpsvkey);

                                LocationJob locajob = new LocationJob();
                                Hashtable gpslocation = locajob.GethttpresultBybsj("http://47.98.58.55:8998/gpsonline/GPSAPI?method=loadLocation&DeviceID=" + GpsDeviceID + "");
                                if (gpslocation["success"].ToString().ToUpper() != "True".ToUpper())
                                {
                                    gpslocation = locajob.Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=loadLocation&vid=" + gpsvid + "&vKey=" + gpsvkey + "");
                                }

                                string newlng = "";
                                string newlat = "";
                                string newinfo = "";
                                DateTime gpstm = DateTime.Now;
                                if (gpslocation["success"].ToString().ToUpper() == "True".ToUpper())
                                {
                                    Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(gpslocation["locs"].ToString());
                                    string newgpstime = ja.First()["gpstime"].ToString();
                                    //newgpstime = newgpstime.Substring(0, newgpstime.Length - 2);
                                    newlng = ja.First()["lng"].ToString();
                                    //newlng = newlng.Substring(0, newlng.Length - 2);
                                    newlat = ja.First()["lat"].ToString();
                                    //newlat = newlat.Substring(0, newlat.Length - 2);
                                    newinfo = ja.First()["info"].ToString();
                                    //newinfo = newinfo.Substring(0, newinfo.Length - 2);
                                    //DateTime gpstm =  DateTime.Parse("1970-01-01 00:00:00");
                                    long time_JAVA_Long = long.Parse(newgpstime);// 1207969641193;//java长整型日期，毫秒为单位          
                                    DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
                                    long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度      
                                    long time_tricks = tricks_1970 + time_JAVA_Long * 10000;//日志日期刻度  
                                    gpstm = new DateTime(time_tricks).AddHours(8);//转化为DateTime
                                    IEnumerable<GpsLocation> locations = db2.GpsLocation.Where(g => g.Gps_time == gpstm & g.GpsDeviceID == GpsDeviceID);
                                    if (locations.Count() == 0)
                                    {
                                        //'写入location表，更新运单表，要注意判断gps时间，不要重复写入
                                        GpsLocation pgl = new GpsLocation();
                                        pgl.Gps_info = newinfo;
                                        pgl.Gps_lat = newlat;
                                        pgl.Gps_lng = newlng;
                                        pgl.Gps_time = gpstm;
                                        pgl.GpsDeviceID = GpsDeviceID;
                                        pgl.GpsRemark = "自动定位";
                                        db2.GpsLocation.Add(pgl);
                                        db2.SaveChanges();
                                    }
                                }
                                //获取起始站、到达站位置
                                string QiShiZhan_lat = "";
                                string QiShiZhan_lng = "";
                                string DaoDaZhan_lat = "";
                                string DaoDaZhan_lng = "";

                                Hashtable addresshash = new ChaHuoBaoWeb.PublickFunction.Map().getmapinfobyaddress(QiShiZhan, "");
                                if (addresshash["sign"] == "1")
                                {
                                    QiShiZhan_lng = addresshash["location"].ToString().Split(',')[0];
                                    QiShiZhan_lat = addresshash["location"].ToString().Split(',')[1];
                                }
                                Hashtable daozhanaddresshash = new ChaHuoBaoWeb.PublickFunction.Map().getmapinfobyaddress(DaoDaZhan, "");
                                if (daozhanaddresshash["sign"] == "1")
                                {
                                    DaoDaZhan_lng = daozhanaddresshash["location"].ToString().Split(',')[0];
                                    DaoDaZhan_lat = daozhanaddresshash["location"].ToString().Split(',')[1];
                                }

                                //创建新的运单
                                YunDan yundan_new = new YunDan();
                                GetTableID gettableid = new GetTableID();
                                string YunDanDenno = gettableid.getdenno();
                                yundan_new.YunDanDenno = YunDanDenno;
                                yundan_new.UserID = UserID;
                                yundan_new.UserDenno = UserDenno;
                                yundan_new.QiShiZhan = QiShiZhan;
                                yundan_new.DaoDaZhan = DaoDaZhan;
                                yundan_new.SuoShuGongSi = SuoShuGongSi;
                                yundan_new.GpsDeviceID = GpsDeviceID;
                                yundan_new.BangDingTime = DateTime.Now;
                                yundan_new.JieBangTime = null;
                                yundan_new.GpsDevicevid = gpsvid;
                                yundan_new.GpsDevicevKey = gpsvkey;
                                yundan_new.Gps_lastlat = newlat;
                                yundan_new.Gps_lastlng = newlng;
                                if (newinfo == "")
                                {
                                    yundan_new.Gps_lasttime = null;
                                }
                                else
                                {
                                    yundan_new.Gps_lasttime = gpstm;
                                }
                                yundan_new.Gps_lastinfo = newinfo;
                                yundan_new.IsBangding = true;
                                yundan_new.YunDanRemark = YunDanRemark;
                                if (string.IsNullOrEmpty(Expect_Hour))
                                    yundan_new.Expect_Hour = null;
                                else
                                    yundan_new.Expect_Hour = Decimal.Parse(Expect_Hour);
                                yundan_new.QiShiZhan_lat = QiShiZhan_lat;
                                yundan_new.QiShiZhan_lng = QiShiZhan_lng;
                                yundan_new.DaoDaZhan_lat = DaoDaZhan_lat;
                                yundan_new.DaoDaZhan_lng = DaoDaZhan_lng;
                                db2.YunDan.Add(yundan_new);
                                db2.SaveChanges();
                                hash["sign"] = "1";
                                hash["msg"] = "制单成功！";
                            }
                            else
                            {
                                hash["sign"] = "2";
                                hash["msg"] = "制单失败，账户可用次数不足！";
                            }
                        }
                    }
                    else
                    {
                        GetVidVkey gvdk = new GetVidVkey();
                        hash = gvdk.gvk(GpsDeviceID);
                        string gpsvid = "";
                        string gpsvkey = "";
                        if (hash["sign"].ToString() == "1")
                        {
                            gpsvid = hash["vid"].ToString();
                            gpsvkey = hash["vKey"].ToString();
                        }

                        ChaHuoBaoWeb.MvcApplication.log4nethelper.Info("APP制单操作=>GPSID：" + GpsDeviceID + ";制单时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ";hash_sign" + hash["sign"].ToString() + ";gpsvid:" + gpsvid + ";gpsvkey:" + gpsvkey);

                        LocationJob locajob = new LocationJob();
                        Hashtable gpslocation = locajob.GethttpresultBybsj("http://47.98.58.55:8998/gpsonline/GPSAPI?method=loadLocation&DeviceID=" + GpsDeviceID + "");
                        if (gpslocation["success"].ToString().ToUpper() != "True".ToUpper())
                        {
                            gpslocation = locajob.Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=loadLocation&vid=" + gpsvid + "&vKey=" + gpsvkey + "");
                        }
                        string newlng = "";
                        string newlat = "";
                        string newinfo = "";
                        DateTime gpstm = DateTime.Now;
                        if (gpslocation["success"].ToString().ToUpper() == "True".ToUpper())
                        {
                            Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(gpslocation["locs"].ToString());
                            string newgpstime = ja.First()["gpstime"].ToString();
                            //newgpstime = newgpstime.Substring(0, newgpstime.Length - 2);
                            newlng = ja.First()["lng"].ToString();
                            //newlng = newlng.Substring(0, newlng.Length - 2);
                            newlat = ja.First()["lat"].ToString();
                            //newlat = newlat.Substring(0, newlat.Length - 2);
                            newinfo = ja.First()["info"].ToString();
                            //newinfo = newinfo.Substring(0, newinfo.Length - 2);
                            //DateTime gpstm =  DateTime.Parse("1970-01-01 00:00:00");
                            long time_JAVA_Long = long.Parse(newgpstime);// 1207969641193;//java长整型日期，毫秒为单位          
                            DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
                            long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度      
                            long time_tricks = tricks_1970 + time_JAVA_Long * 10000;//日志日期刻度  
                            gpstm = new DateTime(time_tricks).AddHours(8);//转化为DateTime
                            IEnumerable<GpsLocation> locations = db2.GpsLocation.Where(g => g.Gps_time == gpstm & g.GpsDeviceID == GpsDeviceID);
                            if (locations.Count() == 0)
                            {
                                //'写入location表，更新运单表，要注意判断gps时间，不要重复写入
                                GpsLocation pgl = new GpsLocation();
                                pgl.Gps_info = newinfo;
                                pgl.Gps_lat = newlat;
                                pgl.Gps_lng = newlng;
                                pgl.Gps_time = gpstm;
                                pgl.GpsDeviceID = GpsDeviceID;
                                pgl.GpsRemark = "自动定位";
                                db2.GpsLocation.Add(pgl);
                                db2.SaveChanges();
                            }
                        }
                        //获取起始站、到达站位置
                        string QiShiZhan_lat = "";
                        string QiShiZhan_lng = "";
                        string DaoDaZhan_lat = "";
                        string DaoDaZhan_lng = "";

                        Hashtable addresshash = new ChaHuoBaoWeb.PublickFunction.Map().getmapinfobyaddress(QiShiZhan, "");
                        if (addresshash["sign"] == "1")
                        {
                            QiShiZhan_lng = addresshash["location"].ToString().Split(',')[0];
                            QiShiZhan_lat = addresshash["location"].ToString().Split(',')[1];
                        }
                        Hashtable daozhanaddresshash = new ChaHuoBaoWeb.PublickFunction.Map().getmapinfobyaddress(DaoDaZhan, "");
                        if (daozhanaddresshash["sign"] == "1")
                        {
                            DaoDaZhan_lng = daozhanaddresshash["location"].ToString().Split(',')[0];
                            DaoDaZhan_lat = daozhanaddresshash["location"].ToString().Split(',')[1];
                        }

                        //创建新的运单
                        YunDan yundan_new = new YunDan();
                        GetTableID gettableid = new GetTableID();
                        string YunDanDenno = gettableid.getdenno();
                        yundan_new.YunDanDenno = YunDanDenno;
                        yundan_new.UserID = UserID;
                        yundan_new.UserDenno = UserDenno;
                        yundan_new.QiShiZhan = QiShiZhan;
                        yundan_new.DaoDaZhan = DaoDaZhan;
                        yundan_new.SuoShuGongSi = SuoShuGongSi;
                        yundan_new.GpsDeviceID = GpsDeviceID;
                        yundan_new.BangDingTime = DateTime.Now;
                        yundan_new.JieBangTime = null;
                        yundan_new.GpsDevicevid = gpsvid;
                        yundan_new.GpsDevicevKey = gpsvkey;
                        yundan_new.Gps_lastlat = newlat;
                        yundan_new.Gps_lastlng = newlng;
                        if (newinfo == "")
                        {
                            yundan_new.Gps_lasttime = null;
                        }
                        else
                        {
                            yundan_new.Gps_lasttime = gpstm;
                        }
                        yundan_new.Gps_lastinfo = newinfo;
                        yundan_new.IsBangding = true;
                        yundan_new.YunDanRemark = YunDanRemark;
                        if (string.IsNullOrEmpty(Expect_Hour))
                            yundan_new.Expect_Hour = null;
                        else
                            yundan_new.Expect_Hour = Decimal.Parse(Expect_Hour);
                        yundan_new.QiShiZhan_lat = QiShiZhan_lat;
                        yundan_new.QiShiZhan_lng = QiShiZhan_lng;
                        yundan_new.DaoDaZhan_lat = DaoDaZhan_lat;
                        yundan_new.DaoDaZhan_lng = DaoDaZhan_lng;
                        db2.YunDan.Add(yundan_new);
                        db2.SaveChanges();
                        hash["sign"] = "1";
                        hash["msg"] = "制单成功！";
                    }
                }
            }
            catch (Exception ex)
            {
                hash["sign"] = "0";
                hash["msg"] = "内部错误:" + ex.Message;
            }
            return hash;
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