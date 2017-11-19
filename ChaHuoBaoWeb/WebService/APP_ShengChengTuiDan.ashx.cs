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
    /// APP_ShengChengTuiDan 的摘要说明
    /// </summary>
    public class APP_ShengChengTuiDan : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            //用户密码
            string GpsDeviceID = context.Request["GpsDeviceID"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "生成退单失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                IEnumerable<GpsDevice> GpsDevice = db.GpsDevice.Where(x => x.UserID == UserID && x.GpsDeviceID == GpsDeviceID);
                if (GpsDevice.Count() == 0)
                {
                    hash["sign"] = "0";
                    hash["msg"] = "该设备不属于你，无法生成退单！";
                }
                else
                {
                    IEnumerable<GpsTuiDanMingXi> GpsTuiDanMingXi = db.GpsTuiDanMingXi.Include("GpsTuiDanModel").Where(x => x.GpsDeviceID == GpsDeviceID && x.GpsTuiDanModel.UserID == UserID && x.GpsTuiDanModel.GpsTuiDanIsEnd == false);
                    if (GpsTuiDanMingXi.Count() > 0)
                    {
                        hash["sign"] = "0";
                        hash["msg"] = "该设备已生成退单,待申请！";
                    }
                    else
                    {
                        IEnumerable<GpsTuiDan> GpsTuiDan = db.GpsTuiDan.Where(x => x.GpsTuiDanIsEnd == false && x.UserID==UserID);
                        string GpsTuiDanDenno = "";
                        if (GpsTuiDan.Count() > 0)
                        {
                            GpsTuiDanDenno = GpsTuiDan.First().GpsTuiDanDenno;
                            hash["OrderDenno"] = GpsTuiDan.First().OrderDenno;
                        }
                        else
                        {
                            GetTableID gettableid = new GetTableID();
                            GpsTuiDanDenno = gettableid.gettableid();
                            GpsTuiDan GpsTuiDan_new = new GpsTuiDan();
                            GpsTuiDan_new.GpsTuiDanDenno = GpsTuiDanDenno;
                            GpsTuiDan_new.UserID = UserID;
                            GpsTuiDan_new.GpsTuiDanIsEnd = false;
                            GpsTuiDan_new.GpsTuiDanShuLiang = 0;
                            GpsTuiDan_new.GpsTuiDanJinE = 0;
                            GpsTuiDan_new.GpsTuiDanTime = DateTime.Now;
                            GpsTuiDan_new.OrderDenno = "02" + gettableid.getdenno();
                            hash["OrderDenno"] = GpsTuiDan_new.OrderDenno;
                            db.GpsTuiDan.Add(GpsTuiDan_new);
                            db.SaveChanges();
                        }
                        GpsTuiDanMingXi GpsTuiDanMingXi_new = new GpsTuiDanMingXi();
                        GpsTuiDanMingXi_new.GpsTuiDanDenno = GpsTuiDanDenno;
                        GpsTuiDanMingXi_new.GpsDeviceID = GpsDeviceID;
                        GpsTuiDanMingXi_new.GpsTuiDanMingXiTime = DateTime.Now;
                        GpsTuiDanMingXi_new.GpsTuiDanMingXiRemark = "";
                        db.GpsTuiDanMingXi.Add(GpsTuiDanMingXi_new);


                        //添加 操作记录
                        CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                        CaoZuoJiLu.UserID = UserID;
                        CaoZuoJiLu.CaoZuoLeiXing = "生成退单";
                        CaoZuoJiLu.CaoZuoNeiRong = "APP内用户生成退单，所属退单列表单号：" + GpsTuiDanDenno + "。";
                        CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                        CaoZuoJiLu.CaoZuoRemark = "";
                        db.CaoZuoJiLu.Add(CaoZuoJiLu);

                        db.SaveChanges();
                        hash["sign"] = "1";
                        hash["msg"] = "生成退单成功！";
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