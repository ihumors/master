namespace Dms.Master.Controllers
{
    using App_Start;
    using Filters;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    [MemberAuthorize]
    [ExceptionFilterAttribute]
    public class BaseController : AsyncController
    {
        public string RenderViewToString(string viewName, object model, ControllerContext context)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            var viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewName);
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;
            string result = String.Empty;
            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }
            return result;
        }

        public async Task<string> RenderViewToStringAync(string viewName, object model, ControllerContext context)
        {
            return await Task.Factory.StartNew(() =>
            {
                return RenderViewToString(viewName, model, context);
            });
        }

        #region over ride JsonResult
        public new JsonResult Json(object data)
        {
            return Json(data, null, null, JsonRequestBehavior.DenyGet);
        }
        public new JsonResult Json(object data, JsonRequestBehavior behavior)
        {
            return Json(data, null, null, behavior);
        }
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new CustomJsonResult
            {
                Data = new
                {
                    bizCode = 200,
                    message = "This request has been processed!",
                    result = data
                },
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };

        }
        #endregion
        public JsonResult RemoveCached(string key)
        {
            Dependencies.cache.Remove(key);
            return Json(new
            {
                key = key,
                message = "执行成功"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}