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
    /// APP_ZhuangHuChongZhi 的摘要说明
    /// </summary>
    public class APP_ZhuangHuChongZhi : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //用户名
            Encoding utf8 = Encoding.UTF8;
            string UserName = context.Request["UserName"];
            UserName = HttpUtility.UrlDecode(UserName.ToUpper(), utf8);
            int JiaGeCeLveID =Convert.ToInt32(context.Request["JiaGeCeLveID"]);
            int ChongZhiCiShu =Convert.ToInt32( context.Request["ChongZhiCiShu"]);
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "生成充值记录失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;

                IEnumerable<JiaGeCeLve> JiaGeCeLve = db.JiaGeCeLve.Where(x => x.JiaGeCeLveID==JiaGeCeLveID);
                if (JiaGeCeLve.Count() > 0)
                {
                    decimal ChongZhiCiShuJinE_db = JiaGeCeLve.First().JiaGeCeLveJinE;
                    int ChongZhiCiShu_db= JiaGeCeLve.First().JiaGeCeLveCiShu;
                    string ChongZhiRemark_db = "套餐充值，充值：" + ChongZhiCiShu_db + "单。共计：" + ChongZhiCiShuJinE_db + "元";
                    if (ChongZhiCiShu_db == 1)
                    {
                        ChongZhiCiShu_db = ChongZhiCiShu;
                        ChongZhiCiShuJinE_db = ChongZhiCiShu * ChongZhiCiShuJinE_db;
                        ChongZhiRemark_db = "单次充值，充值：" + ChongZhiCiShu_db + "单。共计：" + ChongZhiCiShuJinE_db + "元";
                    }
                    ChongZhi ChongZhi = new ChongZhi();
                    GetTableID getdenno=new GetTableID();
                    string OrderDenno="01"+getdenno.getdenno();
                    ChongZhi.UserID = UserID;
                    ChongZhi.OrderDenno = OrderDenno;
                    ChongZhi.ChongZhiJinE = ChongZhiCiShuJinE_db;
                    ChongZhi.ChongZhiCiShu = ChongZhiCiShu_db;
                    ChongZhi.ChongZhiTime = DateTime.Now;
                    ChongZhi.ZhiFuZhuangTai = false;
                    ChongZhi.ChongZhiRemark = ChongZhiRemark_db;
                    db.ChongZhi.Add(ChongZhi);
                    db.SaveChanges();
                    hash["sign"] = "1";
                    hash["msg"] = "生成充值记录成功！";
                    hash["OrderDenno"] = OrderDenno;
                }
                else
                {
                    hash["sign"] = "0";
                    hash["msg"] = "未查询到价格策略，生成充值记录失败！";
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