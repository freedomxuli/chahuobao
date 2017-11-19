using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using ChaHuoBaoWeb.Models;
using Common;
using ChaHuoBaoWeb.PublickFunction;
using System.Configuration;

namespace ChaHuoBaoWeb.WebService
{
    /// <summary>
    /// APP_JianChaBanBen 的摘要说明
    /// </summary>
    public class APP_JianChaBanBen : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string xitong = context.Request["xitong"];
            //当前版本
            string dangqianbanbenhao = context.Request["dangqianbanbenhao"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "已是最新版本！";
            #region
            try
            {
                //string ZuiXinBanBenVersion = ConfigurationManager.AppSettings.GetValues("ZuiXinBanBenVersion").First();
                //string ZuiXinBanBenUrl = ConfigurationManager.AppSettings.GetValues("ZuiXinBanBenUrl").First();
                //if (string.Compare(ZuiXinBanBenVersion, dangqianbanbenhao) > 0)
                //{
                //    hash["sign"] = "1";
                //    hash["zuixinbanben"] = ZuiXinBanBenVersion;
                //    hash["genxinurl"] = ZuiXinBanBenUrl;
                //    hash["msg"] = "可更新最新版本！";
                //}
                //else
                //{
                //    hash["sign"] = "0";
                //    hash["msg"] = "已是最新版本！";
                //}

                ChaHuoBaoModels db = new ChaHuoBaoModels();
                IEnumerable<XiTongCanShu> XiTongCanShu_banbenhao = db.XiTongCanShu.Where(x => x.Name == "BanBenHao");
                IEnumerable<XiTongCanShu> XiTongCanShu_banbenurl = db.XiTongCanShu.Where(x => x.Name == "BanBenUrl");
                if (string.Compare(XiTongCanShu_banbenhao.First().Value, dangqianbanbenhao) > 0)
                {
                    hash["sign"] = "1";
                    hash["zuixinbanben"] = XiTongCanShu_banbenhao.First().Value;
                    hash["genxinurl"] = XiTongCanShu_banbenurl.First().Value;
                    hash["msg"] = "可更新最新版本！";
                }
                else
                {
                    hash["sign"] = "0";
                    hash["msg"] = "已是最新版本！";
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