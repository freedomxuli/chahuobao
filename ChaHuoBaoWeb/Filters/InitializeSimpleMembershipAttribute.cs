using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using ChaHuoBaoWeb.Models;
using System.Web;
using ChaHuoBaoWeb.PublickFunction;

namespace ChaHuoBaoWeb.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                //Database.SetInitializer<UsersContext>(null);
                Database.SetInitializer<ChaHuoBaoModels>(null);
                try
                {
                    //using (var context = new UsersContext())
                    using (var context = new ChaHuoBaoModels())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }
                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "User", "UserID", "UserName", autoCreateTables: false);
                    //WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }

    [InitializeSimpleMembership]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User as ChaHuoBaoWeb.Models.CookieModels.CustomPrincipal;
            if (user == null)
            {
                return false;
            }
            if (httpContext.User.Identity.IsAuthenticated == false)
            {
                return false;
            }
            if (HttpContext.Current.User.Identity.Name == "")
            {
                return false;
            }
            if (user.IsInRole("超级管理员"))
            {
                return true;
            }
            string permission = HttpContext.Current.Request.Path;
            ///访问主页不做判断
            if (permission == "/")
            {
                return true;
            }
            permission = "/" + HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString() + "/" + HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            if (SearchQuanXian.CheckUserHasPermision(HttpContext.Current.User.Identity.Name, permission) == false)
            {
                //throw new Exception("您没有操作权限");
                System.Web.Mvc.AuthorizationContext a = new AuthorizationContext();
                HttpContext.Current.Response.Redirect("/Account/OnPermissionError");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
