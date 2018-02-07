using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentScheduler;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using Aop.Api.Util;
using ChaHuoBaoWeb.Models;
using System.Diagnostics;

namespace ChaHuoBaoWeb.PublickFunction
{
    public class Autowork : Registry
    {

        public Autowork()
        {

            // Schedule an IJob to run at an interval
            // 立即执行每两秒一次的计划任务。（指定一个时间间隔运行，根据自己需求，可以是秒、分、时、天、月、年等。）

            ChaHuoBaoModels db = new ChaHuoBaoModels();
            IEnumerable<XiTongCanShu> XiTongCanShu_shijianjiange = db.XiTongCanShu.Where(x => x.Name == "DingWeiShiJianJianGe");
            string shijianjiange_str = XiTongCanShu_shijianjiange.First().Value;
            int shijianjiange = Convert.ToInt32(shijianjiange_str);
            Schedule<LocationJob>().ToRunNow().AndEvery(shijianjiange).Minutes();

            //分区插入数据
            IEnumerable<GpsDeviceTable> GpsDeviceTime_8630 = db.GpsDeviceTable.Where(x => x.DeviceCode == "8630");
            int gpstime8630 = GpsDeviceTime_8630.First().DeviceTime;
            Schedule<LocationJob2>().ToRunNow().AndEvery(gpstime8630).Minutes();

            // Schedule an IJob to run once, delayed by a specific time interval
            // 延迟一个指定时间间隔执行一次计划任务。（当然，这个间隔依然可以是秒、分、时、天、月、年等。）
            //Schedule<MyJob>().ToRunOnceIn(5).Seconds();

            // Schedule a simple job to run at a specific time
            // 在一个指定时间执行计划任务（最常用。这里是在每天的下午 1:10 分执行）
            Schedule(() =>
            {
                new OtherLocationJob().Dowork();
            }).ToRunEvery(1).Days().At(0, 00);

            //for (int i = 0; i < 24; i++)
            //{
            //    Schedule(() =>
            //    {

            //        new LocationJob().Dowork();

            //    }).ToRunEvery(1).Days().At(i, 00);
            //}
            // Schedule a more complex action to run immediately and on an monthly interval
            // 立即执行一个在每月的星期一 3:00 的计划任务（可以看出来这个一个比较复杂点的时间，它意思是它也能做到！）
            //Schedule<MyComplexJob>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);

            // Schedule multiple jobs to be run in a single schedule
            // 在同一个计划中执行两个（多个）任务
            //Schedule<MyJob>().AndThen<MyOtherJob>().ToRunNow().AndEvery(5).Minutes();

        }
    }
    //public class LocationJob
    //{

    //    public void Dowork()
    //    {
    //        ChaHuoBaoWeb.Models.ChaHuoBaoModels db = new Models.ChaHuoBaoModels();
    //        string gpsvid = "";
    //        string gpsvkey = "";
    //        //Boolean gpsvupdate = false;
    //        try
    //        {
    //            IEnumerable<Models.YunDan> yundans = db.YunDan.Where(g => g.IsBangding == true).ToList();
    //            ChaHuoBaoWeb.MvcApplication.log4nethelper.Info("计划任务：获取运单位置" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    //            foreach (Models.YunDan yd in yundans)
    //            {
    //                //'获取最新位置，如果有更新则插入location，同时更新yundan中的最新gps信息
    //                if (string.IsNullOrEmpty(yd.GpsDevicevid))
    //                {
    //                    Hashtable gpsinfo = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=vLoginSystem&name=" + yd.GpsDeviceID + "&pwd=123456");
    //                    if (gpsinfo["success"].ToString().ToUpper() != "True".ToUpper())
    //                    {
    //                        ChaHuoBaoWeb.MvcApplication.log4nethelper.Error("获取车辆vkey，vid失败 单号：" + yd.UserDenno + " 设备ID:" + yd.GpsDeviceID + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    //                        continue;

    //                    }
    //                    gpsvid = gpsinfo["vid"].ToString();
    //                    gpsvkey = gpsinfo["vKey"].ToString();
    //                    //gpsvupdate = true;
    //                }
    //                else
    //                {
    //                    gpsvid = yd.GpsDevicevid;
    //                    gpsvkey = yd.GpsDevicevKey;
    //                }

    //                Hashtable gpslocation = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=loadLocation&vid=" + gpsvid + "&vKey=" + gpsvkey + "");
    //                if (gpslocation["success"].ToString().ToUpper() != "True".ToUpper())
    //                {
    //                    //获取位置失败，记录日志。
    //                    ChaHuoBaoWeb.MvcApplication.log4nethelper.Error("获取位置失败,单号：" + yd.UserDenno + " 设备ID" + yd.GpsDeviceID + " userID:" + yd.UserID + "  " + JsonConvert.SerializeObject(gpslocation));
    //                    continue;
    //                }
    //                Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(gpslocation["locs"].ToString());
    //                string newgpstime = ja.First()["gpstime"].ToString();
    //                //newgpstime = newgpstime.Substring(0, newgpstime.Length - 2);
    //                string newlng = ja.First()["lng"].ToString();
    //                //newlng = newlng.Substring(0, newlng.Length - 2);
    //                string newlat = ja.First()["lat"].ToString();
    //                //newlat = newlat.Substring(0, newlat.Length - 2);
    //                string newinfo = ja.First()["info"].ToString();
    //                //newinfo = newinfo.Substring(0, newinfo.Length - 2);


    //                //DateTime gpstm =  DateTime.Parse("1970-01-01 00:00:00");

    //                long time_JAVA_Long = long.Parse(newgpstime);// 1207969641193;//java长整型日期，毫秒为单位          
    //                DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
    //                long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度      
    //                long time_tricks = tricks_1970 + time_JAVA_Long * 10000;//日志日期刻度  
    //                DateTime gpstm = new DateTime(time_tricks).AddHours(8);//转化为DateTime

    //                IEnumerable<GpsLocation> locations = db.GpsLocation.Where(g => g.Gps_time == gpstm & g.GpsDeviceID == yd.GpsDeviceID);

    //                if (locations.Count() == 0)
    //                {

    //                    //'写入location表，更新运单表，要注意判断gps时间，不要重复写入
    //                    GpsLocation pgl = new GpsLocation();
    //                    //写入定位表，更新运单表

    //                    Models.YunDan updateyundan = db.YunDan.Where(g => g.YunDanDenno == yd.YunDanDenno).First();
    //                    updateyundan.Gps_lasttime = gpstm;

    //                    updateyundan.Gps_lastlng = newlng;
    //                    updateyundan.Gps_lastlat = newlat;
    //                    updateyundan.Gps_lastinfo = newinfo;
    //                    if (gpsvid != "")
    //                    {
    //                        updateyundan.GpsDevicevid = gpsvid;
    //                        updateyundan.GpsDevicevKey = gpsvkey;
    //                    }

    //                    //if (gpsvupdate)
    //                    //{
    //                    //    updateyundan.GpsDevicevid = gpsvid;
    //                    //    updateyundan.GpsDevicevKey = gpsvkey;
    //                    //}


    //                    pgl.Gps_info = newinfo;
    //                    pgl.Gps_lat = newlat;
    //                    pgl.Gps_lng = newlng;
    //                    pgl.Gps_time = gpstm;
    //                    pgl.GpsDeviceID = yd.GpsDeviceID;
    //                    pgl.GpsRemark = "自动定位";
    //                    db.GpsLocation.Add(pgl);
    //                    db.SaveChanges();
    //                }

    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug(ex);
    //        }
    //    }

    public class LocationJob : IJob
    {

        void IJob.Execute()
        {
            Models.ChaHuoBaoModels db = new Models.ChaHuoBaoModels();
            string gpsvid = "";
            string gpsvkey = "";
            //Boolean gpsvupdate = false;
            try
            {
                IEnumerable<Models.YunDan> yundans = db.YunDan.Where(g => g.IsBangding == true).ToList();
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Info("计划任务：获取运单位置" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                foreach (Models.YunDan yd in yundans)
                {
                    //'获取最新位置，如果有更新则插入location，同时更新yundan中的最新gps信息
                    if (string.IsNullOrEmpty(yd.GpsDevicevid))
                    {
                        Hashtable gpsinfo = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=vLoginSystem&name=" + yd.GpsDeviceID + "&pwd=123456");
                        if (gpsinfo["success"].ToString().ToUpper() != "True".ToUpper())
                        {
                            ChaHuoBaoWeb.MvcApplication.log4nethelper.Error("获取车辆vkey，vid失败 单号：" + yd.UserDenno + " 设备ID:" + yd.GpsDeviceID + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            continue;
                        }
                        gpsvid = gpsinfo["vid"].ToString();
                        gpsvkey = gpsinfo["vKey"].ToString();
                        //gpsvupdate = true;
                    }
                    else
                    {
                        gpsvid = yd.GpsDevicevid;
                        gpsvkey = yd.GpsDevicevKey;
                    }

                    Hashtable gpslocation = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=loadLocation&vid=" + gpsvid + "&vKey=" + gpsvkey + "");
                    if (gpslocation["success"].ToString().ToUpper() != "True".ToUpper())
                    {
                        //获取位置失败，记录日志。
                        ChaHuoBaoWeb.MvcApplication.log4nethelper.Error("获取位置失败,单号：" + yd.UserDenno + " 设备ID" + yd.GpsDeviceID + " userID:" + yd.UserID + "  " + JsonConvert.SerializeObject(gpslocation));
                        continue;
                    }
                    Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(gpslocation["locs"].ToString());
                    string newgpstime = ja.First()["gpstime"].ToString();
                    //newgpstime = newgpstime.Substring(0, newgpstime.Length - 2);
                    string newlng = ja.First()["lng"].ToString();
                    //newlng = newlng.Substring(0, newlng.Length - 2);
                    string newlat = ja.First()["lat"].ToString();
                    //newlat = newlat.Substring(0, newlat.Length - 2);
                    string newinfo = ja.First()["info"].ToString();
                    //newinfo = newinfo.Substring(0, newinfo.Length - 2);
                    //DateTime gpstm =  DateTime.Parse("1970-01-01 00:00:00");
                    long time_JAVA_Long = long.Parse(newgpstime);// 1207969641193;//java长整型日期，毫秒为单位          
                    DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
                    long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度      
                    long time_tricks = tricks_1970 + time_JAVA_Long * 10000;//日志日期刻度  
                    DateTime gpstm = new DateTime(time_tricks).AddHours(8);//转化为DateTime

                    IEnumerable<GpsLocation> locations = db.GpsLocation.Where(g => g.Gps_time == gpstm & g.GpsDeviceID == yd.GpsDeviceID);

                    if (locations.Count() == 0)
                    {

                        //'写入location表，更新运单表，要注意判断gps时间，不要重复写入
                        GpsLocation pgl = new GpsLocation();
                        //写入定位表，更新运单表
                        Models.YunDan updateyundan = db.YunDan.Where(g => g.YunDanDenno == yd.YunDanDenno).First();
                        updateyundan.Gps_lasttime = gpstm;
                        updateyundan.Gps_lastlng = newlng;
                        updateyundan.Gps_lastlat = newlat;
                        updateyundan.Gps_lastinfo = newinfo;
                        if (gpsvid != "")
                        {
                            updateyundan.GpsDevicevid = gpsvid;
                            updateyundan.GpsDevicevKey = gpsvkey;
                        }
                        pgl.Gps_info = newinfo;
                        pgl.Gps_lat = newlat;
                        pgl.Gps_lng = newlng;
                        pgl.Gps_time = gpstm;
                        pgl.GpsDeviceID = yd.GpsDeviceID;
                        pgl.GpsRemark = "自动定位";
                        db.GpsLocation.Add(pgl);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug(ex);
            }
        }

        public System.Collections.Hashtable Gethttpresult(string url, string data)
        {
            WebRequest request = WebRequest.Create(url);
            Encoding encode = Encoding.GetEncoding("utf-8");
            request.Method = "POST";
            Byte[] byteArray = encode.GetBytes(data);
            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();

            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream, encode);
            String responseFromServer = reader.ReadToEnd();
            string outStr = responseFromServer;
            reader.Close();
            dataStream.Close();
            response.Close();

            Hashtable hashTable = JsonConvert.DeserializeObject<Hashtable>(outStr);
            return hashTable;
        }

    }

    public class LocationJob2 : IJob
    {

        void IJob.Execute()
        {
            Models.ChaHuoBaoModels db = new Models.ChaHuoBaoModels();
            string gpsvid = "";
            string gpsvkey = "";
            //Boolean gpsvupdate = false;
            try
            {
                IEnumerable<Models.YunDan> yundans = db.YunDan.Where(g => g.IsBangding == true && g.GpsDeviceID.StartsWith("8630")).ToList();
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Info("计划任务：获取运单位置" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                foreach (Models.YunDan yd in yundans)
                {
                    //'获取最新位置，如果有更新则插入location，同时更新yundan中的最新gps信息
                    if (string.IsNullOrEmpty(yd.GpsDevicevid))
                    {
                        Hashtable gpsinfo = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=vLoginSystem&name=" + yd.GpsDeviceID + "&pwd=123456");
                        if (gpsinfo["success"].ToString().ToUpper() != "True".ToUpper())
                        {
                            ChaHuoBaoWeb.MvcApplication.log4nethelper.Error("获取车辆vkey，vid失败 单号：" + yd.UserDenno + " 设备ID:" + yd.GpsDeviceID + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            continue;
                        }
                        gpsvid = gpsinfo["vid"].ToString();
                        gpsvkey = gpsinfo["vKey"].ToString();
                        //gpsvupdate = true;
                    }
                    else
                    {
                        gpsvid = yd.GpsDevicevid;
                        gpsvkey = yd.GpsDevicevKey;
                    }

                    Hashtable gpslocation = Gethttpresult("http://101.37.253.238:89/gpsonline/GPSAPI", "version=1&method=loadLocation&vid=" + gpsvid + "&vKey=" + gpsvkey + "");
                    if (gpslocation["success"].ToString().ToUpper() != "True".ToUpper())
                    {
                        //获取位置失败，记录日志。
                        ChaHuoBaoWeb.MvcApplication.log4nethelper.Error("获取位置失败,单号：" + yd.UserDenno + " 设备ID" + yd.GpsDeviceID + " userID:" + yd.UserID + "  " + JsonConvert.SerializeObject(gpslocation));
                        continue;
                    }
                    Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(gpslocation["locs"].ToString());
                    string newgpstime = ja.First()["gpstime"].ToString();
                    //newgpstime = newgpstime.Substring(0, newgpstime.Length - 2);
                    string newlng = ja.First()["lng"].ToString();
                    //newlng = newlng.Substring(0, newlng.Length - 2);
                    string newlat = ja.First()["lat"].ToString();
                    //newlat = newlat.Substring(0, newlat.Length - 2);
                    string newinfo = ja.First()["info"].ToString();
                    //newinfo = newinfo.Substring(0, newinfo.Length - 2);
                    //DateTime gpstm =  DateTime.Parse("1970-01-01 00:00:00");
                    long time_JAVA_Long = long.Parse(newgpstime);// 1207969641193;//java长整型日期，毫秒为单位          
                    DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
                    long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度      
                    long time_tricks = tricks_1970 + time_JAVA_Long * 10000;//日志日期刻度  
                    DateTime gpstm = new DateTime(time_tricks).AddHours(8);//转化为DateTime

                    IEnumerable<GpsLocation2> locations = db.GpsLocation2.Where(g => g.Gps_time == gpstm & g.GpsDeviceID == yd.GpsDeviceID);

                    if (locations.Count() == 0)
                    {

                        //'写入location表，更新运单表，要注意判断gps时间，不要重复写入
                        GpsLocation2 pgl = new GpsLocation2();
                        pgl.Gps_info = newinfo;
                        pgl.Gps_lat = newlat;
                        pgl.Gps_lng = newlng;
                        pgl.Gps_time = gpstm;
                        pgl.GpsDeviceID = yd.GpsDeviceID;
                        pgl.GpsRemark = "自动定位";
                        db.GpsLocation2.Add(pgl);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug(ex);
            }
        }

        public System.Collections.Hashtable Gethttpresult(string url, string data)
        {
            WebRequest request = WebRequest.Create(url);
            Encoding encode = Encoding.GetEncoding("utf-8");
            request.Method = "POST";
            Byte[] byteArray = encode.GetBytes(data);
            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();

            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream, encode);
            String responseFromServer = reader.ReadToEnd();
            string outStr = responseFromServer;
            reader.Close();
            dataStream.Close();
            response.Close();

            Hashtable hashTable = JsonConvert.DeserializeObject<Hashtable>(outStr);
            return hashTable;
        }

    }

    public class OtherLocationJob
    {
        public void Dowork()
        {
            try
            {
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Info("计划任务：删除之前未支付的订单" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<GpsDingDan> GpsDingDanList = db.GpsDingDan.Where(x => x.GpsDingDanTime < DateTime.Now && x.GpsDingDanZhiFuZhuangTai == false);
                if (GpsDingDanList.Count() > 0)
                {
                    foreach (var obj in GpsDingDanList)
                    {
                        string dingdandanhao = obj.GpsDingDanDenno;
                        ChaHuoBaoModels db2 = new ChaHuoBaoModels();
                        IEnumerable<GpsDingDanMingXi> GpsDingDanMingXiList = db2.GpsDingDanMingXi.Where(x => x.GpsDingDanDenno == dingdandanhao);
                        foreach (var obj2 in GpsDingDanMingXiList)
                        {
                            db2.GpsDingDanMingXi.Remove(obj2);
                        }
                        db2.SaveChanges();
                        ChaHuoBaoWeb.MvcApplication.log4nethelper.Info("联级删除：订单编号："+obj.GpsDingDanDenno+"订单设备数量："+obj.GpsDingDanShuLiang);
                        db.GpsDingDan.Remove(obj);
                    }
                    db.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug(ex);
            }
        }
    }

}