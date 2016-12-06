using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Dms.Master.Filters
{
    public class CustomJsonResult : JsonResult
    {
        const string JsonRequest_GetNotAllowed = "This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request.To allow GET requests, set JsonRequestBehavior to AllowGet.";
        public new int MaxJsonLength { get; set; }
        public new int RecursionLimit { get; set; }
        public CustomJsonResult()
        {
            MaxJsonLength = 10240000;
            RecursionLimit = 500;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(JsonRequest_GetNotAllowed);
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer() {
                    MaxJsonLength = MaxJsonLength,
                    RecursionLimit = RecursionLimit
                };

                response.Write(serializer.Serialize(Data));
            }
        }
    }
}