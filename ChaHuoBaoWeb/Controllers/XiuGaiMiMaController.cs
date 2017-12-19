using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChaHuoBaoWeb.Models;
using ChaHuoBaoWeb.Filters;

namespace ChaHuoBaoWeb.Controllers
{
    [Authorize]
    [PermissionAuthorize]
    public class XiuGaiMiMaController : Controller
    {
        //修改密码首页
        // GET: /XiuGaiMiMa/
        ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string UserName, string yuanmima, string xinmima, string querenxinmima)
        {
            string msg = "";
            ViewData["yuanmima"] = yuanmima;
            if (string.IsNullOrEmpty(xinmima))
            {
                msg = "新密码不可设置为空，修改失败！";
            }
            else
            {

                IEnumerable<User> user = accountdb.User.Where(x => x.UserName == UserName && x.UserPassword == yuanmima);
                if (user.Count() > 0)
                {
                    if (querenxinmima == xinmima)
                    {
                        user.First().UserPassword = xinmima;
                        accountdb.SaveChanges();
                        msg = "密码修改成功！";
                        ViewData["xinmima"] = xinmima;
                        ViewData["querenxinmima"] = querenxinmima;
                    }
                    else
                    {
                        msg = "两次密码不一致，修改失败！";
                    }

                }
                else
                {
                    msg = "原密码不正确，修改失败！";
                }
            }
            ViewData["msg"] = msg;
            return View();
        }
    }
}
