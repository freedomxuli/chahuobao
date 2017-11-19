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
    /// APP_XiaoFeiJiLu 的摘要说明
    /// </summary>
    public class APP_XiaoFeiJiLu : IHttpHandler
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
            hash["msg"] = "查询消费记录失败！";
            #region
            try
            {
                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<User> User = db.User.Where(x => x.UserName == UserName && x.UserLeiXing == "APP");
                string UserID = User.First().UserID;
                IEnumerable<ChongZhi> ChongZhi = db.ChongZhi.Where(x => x.UserID == UserID && x.ZhiFuZhuangTai==true).OrderByDescending(x=>x.ChongZhiTime);
                if (ChongZhi.Count() > 0)
                {
                    //添加 操作记录
                    CaoZuoJiLu CaoZuoJiLu = new CaoZuoJiLu();
                    CaoZuoJiLu.UserID = UserID;
                    CaoZuoJiLu.CaoZuoLeiXing = "消费记录";
                    CaoZuoJiLu.CaoZuoNeiRong = "APP内用户查询消费记录。";
                    CaoZuoJiLu.CaoZuoTime = DateTime.Now;
                    CaoZuoJiLu.CaoZuoRemark = "";
                    db.CaoZuoJiLu.Add(CaoZuoJiLu);
                    db.SaveChanges();
                    hash["sign"] = "1";
                    hash["msg"] = "查询消费记录成功！";
                    hash["xiaofeijilulist"] = ChongZhi;
                }
                else
                {
                    hash["sign"] = "2";
                    hash["msg"] = "未查询到消费记录！";
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