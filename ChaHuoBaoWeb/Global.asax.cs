using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Text;
using ChaHuoBaoWeb.Models;
using System.Web.Security;
using FluentScheduler;
using ChaHuoBaoWeb.PublickFunction;
using log4net;

namespace ChaHuoBaoWeb
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static ILog log4nethelper;
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception objErr = Server.GetLastError().GetBaseException();
            log4nethelper.Error(objErr);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {

            // ExceptionLogAttribute继承自HandleError，主要作用是将异常信息写入日志系统中
            filters.Add(new ChaHuoBaoWeb.FilterConfig.ExceptionLogAttribute());
            //默认的异常记录类
            filters.Add(new HandleErrorAttribute());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));
            log4nethelper = LogManager.GetLogger("log4netlogger");
            //定时任务
            JobManager.Initialize(new Autowork());
        }
        public MvcApplication()
        {
            AuthorizeRequest += new EventHandler(MvcApplication_AuthorizeRequest);
        }
        void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["ChaHuoBao"];
            if (cookie != null)
            {
                ChaHuoBaoWeb.Models.CookieModels.myidentity myidentity = new Models.CookieModels.myidentity();
                myidentity.Name = HttpUtility.UrlDecode(cookie.Values["yonghuming"], Encoding.GetEncoding("UTF-8"));
                myidentity.yonghuming = HttpUtility.UrlDecode(cookie.Values["yonghuming"], Encoding.GetEncoding("UTF-8"));
                myidentity.mima = cookie.Values["mima"];
                myidentity.UserID = cookie.Values["UserID"];
                ChaHuoBaoModels accountdb = new ChaHuoBaoModels();
                IEnumerable<User> yonghu = accountdb.User.Include("User_Roles").Include("User_Roles.Role").Where(x => x.UserName == myidentity.yonghuming & x.UserPassword == myidentity.mima & x.UserLeiXing == "WEB");
                if (yonghu.Count() <= 0)
                {
                    FormsAuthentication.SignOut();
                }
                else
                {
                    myidentity.IsAuthenticated = true;
                    ChaHuoBaoWeb.Models.CookieModels.UserData UserData = new ChaHuoBaoWeb.Models.CookieModels.UserData();
                    UserData.UserId = myidentity.UserID;
                    UserData.UserName = myidentity.yonghuming;
                    UserData.Roles = new List<string>();
                    foreach (var obj in yonghu.First().User_Roles)
                    {
                        string roleid = obj.Role.RoleID;
                        string rolename = obj.Role.RoleName;
                        UserData.Roles.Add(rolename);
                    }
                    var cookiemodel = new ChaHuoBaoWeb.Models.CookieModels.CustomPrincipal(myidentity, UserData);
                    HttpContext.Current.User = cookiemodel;
                }
            }
        }
    }
}