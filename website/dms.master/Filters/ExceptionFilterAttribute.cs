using Dms.Master.App_Start;
using LazyCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dms.Master.Filters
{
    public class ExceptionFilterAttribute :  FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                Dependencies.cache.Remove(threadId.ToString());

                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];
                HttpException httpException = new HttpException(null, filterContext.Exception);

                filterContext.ExceptionHandled = true;
                Core.Logger.Error(filterContext.Exception.Message);

                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        bizCode = httpException.ErrorCode,
                        controller= controllerName,
                        action= actionName,
                        message= filterContext.Exception.Message,
                        result = new { }
                    },
                    JsonRequestBehavior=JsonRequestBehavior.AllowGet
                };
            }
        }
    }
}