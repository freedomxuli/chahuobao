using System.Web;
using System.Web.Mvc;
using log4net;
using System.Reflection;

namespace ChaHuoBaoWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionLogAttribute());
        }
        /// <summary>
        /// 异常持久化类
        /// </summary>
        public class ExceptionLogAttribute : HandleErrorAttribute
        {
            /// <summary>
            /// 触发异常时调用的方法
            /// </summary>
            /// <param name="filterContext"></param>
            public override void OnException(ExceptionContext filterContext)
            {

                string message = string.Format("消息类型：{0} 消息内容：{1} 引发异常的方法：{2} 引发异常的对象：{3} 异常目录：{4} 异常文件：{5}"
                    , filterContext.Exception.GetType().Name
                    , filterContext.Exception.Message
                    , filterContext.Exception.TargetSite
                    , filterContext.Exception.Source
                    , filterContext.RouteData.GetRequiredString("controller")
                    , filterContext.RouteData.GetRequiredString("action"));
                //VLog.VLogFactory.CreateVLog().ErrorLog(message); //TODO:将 ex 错误对象记录到系统日志模块
                ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                logger.Error(message);
                base.OnException(filterContext);
            }
        }
    }
}