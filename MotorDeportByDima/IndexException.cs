using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotorDeportByDima
{
    public class IndexException : FilterAttribute, IExceptionFilter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled /*&& exceptionContext.Exception is IndexOutOfRangeException*/)
            {
                logger.Error("controller: " + exceptionContext.Controller + "path" + exceptionContext.RouteData + " сама ошибка" + exceptionContext);
                exceptionContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Error.cshtml"
                };
                exceptionContext.ExceptionHandled = true;
            }
        }
    }
}