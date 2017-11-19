using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using ChaHuoBaoWeb.Models;
using ChaHuoBaoWeb.PublickFunction;
using System.Text;
using System.Collections;

namespace ChaHuoBaoWeb.Controllers
{
    public class MapController : Controller
    {
        //
        // GET: /Map/

        public ActionResult Index(string UserID, string YunDanDenno)
        {
            ViewData["UserID"] = UserID;
            ViewData["YunDanDenno"] = YunDanDenno;
            return View();
        }
        [HttpPost]
        public JsonResult APP_GetYunDanDetail(string UserID, string YunDanDenno)
        {
            Encoding utf8 = Encoding.UTF8;
            Hashtable hash = new Hashtable();
            try
            {
                Map_yundanviewModel viewmodel = new Map_yundanviewModel();
                viewmodel.locationlst = new List<GpsLocation>();
                hash["sign"] = "0";
                hash["msg"] = "获取运单信息失败！";
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                ChaHuoBaoWeb.Models.YunDan yundandt;
                if (string.IsNullOrEmpty(UserID) == false)
                {
                     yundandt = db.YunDan.Where(g => g.YunDanDenno == YunDanDenno & g.UserID == UserID).First();
                }
                else
                {
                    yundandt = db.YunDan.Where(g => g.YunDanDenno == YunDanDenno ).First();
                }
                hash["sign"] = "1";
                hash["msg"] = "获取运单信息成功！";
                viewmodel.qishizhan_lng = yundandt.QiShiZhan_lng;
                viewmodel.qishizhan_lat = yundandt.QiShiZhan_lat;
                viewmodel.daodazhan_lng = yundandt.DaoDaZhan_lng;
                viewmodel.daodazhan_lat = yundandt.DaoDaZhan_lat;
                //Hashtable addresshash = new ChaHuoBaoWeb.PublickFunction.Map().getmapinfobyaddress(yundandt.QiShiZhan, "");
                //if (addresshash["sign"] == "1")
                //{
                //    viewmodel.qishizhan_lng = addresshash["location"].ToString().Split(',')[0];
                //    viewmodel.qishizhan_lat = addresshash["location"].ToString().Split(',')[1];
                //}
                //Hashtable daozhanaddresshash = new ChaHuoBaoWeb.PublickFunction.Map().getmapinfobyaddress(yundandt.DaoDaZhan, "");
                //if (daozhanaddresshash["sign"] == "1")
                //{
                //    viewmodel.daodazhan_lng = daozhanaddresshash["location"].ToString().Split(',')[0];
                //    viewmodel.daodazhan_lat = daozhanaddresshash["location"].ToString().Split(',')[1];
                //}
                DateTime BangDingTime_new = yundandt.BangDingTime.AddHours(-1);
                IEnumerable<GpsLocation> gpslocations = db.GpsLocation.Where(g => g.GpsDeviceID == yundandt.GpsDeviceID & g.Gps_time > BangDingTime_new);
                if (yundandt.IsBangding == false)
                {
                    gpslocations = gpslocations.Where(g => g.Gps_time < yundandt.JieBangTime);
                }
                viewmodel.locationlst = gpslocations.ToList();
                hash["location_result"] = viewmodel;

                if (hash["sign"] == "1")
                {
                    //添加 操作记录
                    CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                    CaoZuoJiLu.UserID = UserID;
                    CaoZuoJiLu.CaoZuoLeiXing = "运单轨迹";
                    CaoZuoJiLu.CaoZuoNeiRong = "APP内用户运单轨迹查询，运单系统单号："+YunDanDenno+"。";
                    CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                    CaoZuoJiLu.CaoZuoRemark = "";
                    db.CaoZuoJiLu.Add(CaoZuoJiLu);
                    db.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                hash["sign"] = "0";
                hash["msg"] = "内部错误:" + ex.Message;
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug(ex);

            }
            return Json(hash);
        }
        public class Map_yundanviewModel
        {
            public string qishizhan_lng { get; set; }
            public string qishizhan_lat
            { get; set; }

            public string daodazhan_lng { get; set; }
            public string daodazhan_lat
            { get; set; }

            public List<ChaHuoBaoWeb.Models.GpsLocation> locationlst { get; set; }
        }

        public ActionResult OnRoadCarList()
        {
            return View();
        }

        [HttpPost]
        public string Getonroadcarlists()
        {
            //string tmcom = "88888";
            System.Collections.Hashtable ht = new System.Collections.Hashtable();
            ht["sign"] = "0";
            ht["msg"] = "加载失败";
            ChaHuoBaoWeb.Models.ChaHuoBaoModels chbdb=new ChaHuoBaoModels();
 var result = from x in chbdb.YunDan where x.IsBangding == true select x;

            List<onroadcarviewmodel> viewlist = new List<onroadcarviewmodel>();
            onroadcarviewmodel viewmodel;
            System.Collections.Hashtable lbsht = new System.Collections.Hashtable();

            try
            {
                foreach (ChaHuoBaoWeb.Models.YunDan yundanmodel in result)
                {
                    //viewmodel = new onroadcarviewmodel { carpaizhao = zcd.chepaihao, cartel = zcd.jiashiyuanshouji, fromandto = zcd.fasongduanModel.gongsimingcheng + ">>>>" + zcd.jieshouduanModel.gongsimingcheng, gpsjingweidu = "", gpslocation = "", gpstime = "" };
                    ///获取位置
                    viewmodel = new onroadcarviewmodel();

                    viewmodel.jingweidu =yundanmodel.Gps_lastlng +","+yundanmodel.Gps_lastlat ;
                    //viewmodel.gpslocation = lbsht["address"].ToString();
                    //viewmodel.gpstime = lbsht["gpstime"].ToString();
                    viewmodel.labelinfo =""; //zcd.fasongduanModel.gongsimingcheng + ">>>>" + zcd.jieshouduanModel.gongsimingcheng + " " + zcd.chepaihao;
                    viewmodel.markinfo = "<p style='margin:0;font-size:15px;font-weight:bold'>详细信息</p>" +
         "<HR style='border:1 solid #2828FF' width='100%'>"
         + "<p style='margin:0;font-size:13px'>行驶路线：" + yundanmodel.QiShiZhan  + ">>>" + yundanmodel.DaoDaZhan + "</p>"
        + "<p style='margin:0;font-size:13px'>建单公司：" + yundanmodel.SuoShuGongSi + "</p>"
        + "<p style='margin:0;font-size:13px'>单号：" + yundanmodel.UserDenno + "</p>"
        + "<p style='margin:0;font-size:13px'>所在位置：" + yundanmodel.Gps_lastinfo + "</p>"
        + "<p style='margin:0;font-size:14px;color:Red'>定位时间：" + yundanmodel.Gps_lasttime + "</p>"
        + "<a style='margin:0;font-size:14px' href='/Map?YunDanDenno="+yundanmodel.YunDanDenno+"' target='_blank'>查看轨迹 </a>" 
        + "</div>";
                    viewlist.Add(viewmodel);
                }
                ht["cars"] = viewlist;
                ht["sign"] = "1";
                //ht["data"] = Newtonsoft.Json.JsonConvert.SerializeObject(zhuangchedans.ToList());
            }
            catch (Exception ex)
            {
                ht["msg"] = ex.Message;
            }

            string jsons = Newtonsoft.Json.JsonConvert.SerializeObject(ht);
            return jsons;
        }

    }

    public class onroadcarviewmodel
    {
        public string jingweidu { get; set; }
        public string labelinfo { get; set; }
        public string markinfo { get; set; }
    }
}
