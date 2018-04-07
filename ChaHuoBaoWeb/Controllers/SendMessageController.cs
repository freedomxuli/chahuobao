using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChaHuoBaoWeb.Models;
using System.IO;
using ChaHuoBaoWeb.Filters;
using System.Collections;
using Common;
using ChaHuoBaoWeb.PublickFunction;

namespace ChaHuoBaoWeb.Controllers
{
    [Authorize]
    [PermissionAuthorize]
    public class SendMessageController : Controller
    {
        //
        // GET: /SendMessage/

        public ActionResult Index()
        {
            return View();
        }

        [PermissionAuthorize]
        //[HttpPost]
        public string SendM()
        {
            string fileText = HttpContext.Request["fileText"];
            string memo = HttpContext.Request["memo"];
            Hashtable hash = new Hashtable();
            hash["sign"] = "0";
            hash["msg"] = "发送失败！";
            try
            {
                new GetYanZhengMa().testmessage(fileText.TrimEnd(','));
                hash["sign"] = "1";
                hash["msg"] = "发送成功";
            }
            catch (Exception ex)
            {
                hash["sign"] = "0";
                hash["msg"] = ex.Message;
            }

            return JsonHelper.ToJson(hash);
        }
    }
}
