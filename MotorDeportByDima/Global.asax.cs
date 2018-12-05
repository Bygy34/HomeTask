using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MotorDeportByDima
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        protected void Application_Start()
        {
            logger.Info("Application Start");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override void Init()
        {
            logger.Info("Application Init");
        }

        public override void Dispose()
        {
            logger.Info("Application Dispose");
        }

        protected void Application_Error()
        {
            if (Request.Url.ToString().StartsWith("http://localhost:"))
                return;
            string msg;
            Exception ex = Server.GetLastError().GetBaseException();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Exception Found");
            sb.AppendLine("Timestamp: " + System.DateTime.Now.ToString());
            sb.AppendLine("Error in: " + Request.Url.ToString());
            sb.AppendLine("Browser Version: " + Request.UserAgent.ToString());
            sb.AppendLine("User IP: " + Request.UserHostAddress.ToString());
            sb.AppendLine("Error Message: " + ex.Message);
            sb.AppendLine("Stack Trace: " + ex.StackTrace);
            msg = sb.ToString();
            Server.ClearError();
            Response.Redirect("~/Error.html");
            logger.Info(msg);
        }


        protected void Application_End()
        {
            logger.Info("Application End");
        }
    }
}
