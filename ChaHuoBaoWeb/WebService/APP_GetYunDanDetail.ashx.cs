using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using Common;
using ChaHuoBaoWeb.Models;
using ChaHuoBaoWeb.PublickFunction;

namespace ChaHuoBaoWeb.WebService
{
    /// <summary>
    /// APP_ZhuCe 的摘要说明
    /// </summary>
    public class APP_GetYunDanDetail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            Hashtable hash = new Hashtable();
            try
            {
                string YunDanDenno = context.Request["YunDanDenno"];
                string UserName = context.Request["UserName"];
                Map_yundanviewModel viewmodel = new Map_yundanviewModel();
                viewmodel.locationlst = new List<GpsLocation>();
                hash["sign"] = "0";
                hash["msg"] = "获取运单信息失败！";
                ChaHuoBaoModels db = new ChaHuoBaoModels();

                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;

                ChaHuoBaoWeb.Models.YunDan yundandt = db.YunDan.Where(g => g.YunDanDenno == YunDanDenno & g.UserID == UserID).First();
                hash["sign"] = "1";
                hash["msg"] = "获取运单信息成功！";
                Hashtable addresshash = new ChaHuoBaoWeb.PublickFunction.Map().getmapinfobyaddress(yundandt.QiShiZhan, "");
                if (addresshash["sign"] == "1")
                {
                    viewmodel.qishizhan_lng = addresshash["location"].ToString().Split(',')[0];
                    viewmodel.qishizhan_lat = addresshash["location"].ToString().Split(',')[1];
                }
                Hashtable daozhanaddresshash = new ChaHuoBaoWeb.PublickFunction.Map().getmapinfobyaddress(yundandt.DaoDaZhan, "");
                if (daozhanaddresshash["sign"] == "1")
                {
                    viewmodel.daodazhan_lng = daozhanaddresshash["location"].ToString().Split(',')[0];
                    viewmodel.daodazhan_lat = daozhanaddresshash["location"].ToString().Split(',')[1];
                }
                IEnumerable<GpsLocation> gpslocations = db.GpsLocation.Where(g => g.GpsDeviceID == yundandt.GpsDeviceID & g.Gps_time > yundandt.BangDingTime);
                if (yundandt.IsBangding == false)
                {
                    gpslocations = gpslocations.Where(g => g.Gps_time < yundandt.JieBangTime);
                }
                viewmodel.locationlst = gpslocations.ToList();
                hash["location_result"] = viewmodel;
            }
            catch (Exception ex)
            {
                hash["sign"] = "0";
                hash["msg"] = "内部错误:" + ex.Message;
                ChaHuoBaoWeb.MvcApplication.log4nethelper.Debug(ex);

            }
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

}