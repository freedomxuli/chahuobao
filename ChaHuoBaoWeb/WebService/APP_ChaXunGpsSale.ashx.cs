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
    /// APP_ChaXunGps 的摘要说明
    /// </summary>
    public class APP_ChaXunGpsSale : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "查询GPS销售设备失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                IEnumerable<GpsDeviceSale> GpsDevice = db.GpsDeviceSale.Where(x => x.UserID == UserID);
                if (GpsDevice.Count() > 0)
                {
                    List<gpsdevice> gpsdevice_list = new List<gpsdevice>();
                    foreach (var obj in GpsDevice)
                    {
                        string GpsDeviceID = obj.GpsDeviceID;
                        gpsdevice gpsdevice_one = new gpsdevice();
                        gpsdevice_one.GpsDeviceID = GpsDeviceID;
                        gpsdevice_one.UserID = obj.UserID;
                        gpsdevice_one.GpsDeviceRemark = obj.GpsDeviceRemark;
                        ChaHuoBaoModels db_new = new ChaHuoBaoModels();
                        IEnumerable<YunDan> YunDan = db_new.YunDan.Where(x => x.GpsDeviceID == GpsDeviceID && x.IsBangding == true);
                        if (YunDan.Count() > 0)
                        {
                            gpsdevice_one.IsBangding = "已绑定";
                        }
                        else
                        {
                            gpsdevice_one.IsBangding = "未绑定";
                        }
                        gpsdevice_list.Add(gpsdevice_one);
                    }
                    //添加 操作记录
                    CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                    CaoZuoJiLu.UserID = UserID;
                    CaoZuoJiLu.CaoZuoLeiXing = "GPS销售管理";
                    CaoZuoJiLu.CaoZuoNeiRong = "APP内用户查询GPS设备销售列表。";
                    CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                    CaoZuoJiLu.CaoZuoRemark = "";
                    db.CaoZuoJiLu.Add(CaoZuoJiLu);
                    db.SaveChanges();


                    hash["sign"] = "1";
                    hash["msg"] = "查询GPS销售设备成功！";
                    hash["gpsguanlilist"] = gpsdevice_list;
                }
                else
                {
                    hash["sign"] = "2";
                    hash["msg"] = "未查询到GPS销售设备！";
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
        public class gpsdevice
        {
            public string GpsDeviceID { get; set; }
            public string UserID { get; set; }
            public string GpsDeviceRemark { get; set; }
            public string IsBangding { get; set; }
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