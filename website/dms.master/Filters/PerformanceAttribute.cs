using Dms.Master.App_Start;
using LazyCache;
using System;
using System.Web.Mvc;

namespace Dms.Master.Filters
{
    public struct ReqThreadInfo
    {
        public DateTime startTime { get; set; }
        public string pathUrl { get; set; }
        public string reqMethod { get; set; }
    }
    public class PerformanceAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction) return;

            var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            Dependencies.cache.Add(threadId.ToString(), new ReqThreadInfo() {
                pathUrl= filterContext.HttpContext.Request.Url == null ? string.Empty : filterContext.HttpContext.Request.Url.AbsoluteUri,
                reqMethod= filterContext.HttpContext.Request.HttpMethod,
                startTime= DateTime.Now
            }, DateTimeOffset.Now.AddMinutes(10));
    
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            var threadInfo = Dependencies.cache.Get<ReqThreadInfo>(threadId.ToString());

            var costSeconds = (DateTime.Now - threadInfo.startTime).TotalMilliseconds;
            Core.Logger.Write("Track", string.Format("http_method: '{0}', total_millisecond: {1}, request_url:'{2}'",
                threadInfo.reqMethod,
                costSeconds.ToString("0"),
                threadInfo.pathUrl));

            Dependencies.cache.Remove(threadId.ToString());
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var acceptEncoding = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(acceptEncoding))
            {
                acceptEncoding = acceptEncoding.ToLower();
                var response = filterContext.HttpContext.Response;
                if (acceptEncoding.Contains("gzip"))
                {
                    response.AppendHeader("Content-encoding", "gzip");
                    response.Filter = new System.IO.Compression.GZipStream(response.Filter, System.IO.Compression.CompressionMode.Compress);
                }
                else if (acceptEncoding.Contains("deflate"))
                {
                    response.AppendHeader("Content-encoding", "deflate");
                    response.Filter = new System.IO.Compression.DeflateStream(response.Filter, System.IO.Compression.CompressionMode.Compress);
                }
            }
        }
    }
}