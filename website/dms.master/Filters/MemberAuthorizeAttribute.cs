using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Dms.Master.Filters
{
    public class MemberAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (!AuthorizeCore(filterContext.HttpContext) &&
                !filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) &&
                !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
               )
            {
                HandleUnauthorizedRequest(filterContext);
                return;
            }
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Dms.Core.Cookies cookies = new Dms.Core.Cookies();
            if (string.IsNullOrEmpty(cookies.Get("uid")))
                return false;

            SetClaimsPrincipal(cookies);
            cookies.Dispose();

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context != null)
            {
                string path = context.HttpContext.Request.Path;
                string strUrl = "/Member/Security/Login?returnUrl={0}";
                context.HttpContext.Response.Redirect(string.Format(strUrl, HttpUtility.UrlEncode(path)), true);
                return;
            }
            throw new ArgumentNullException("filterContext");
        }

        protected void SetClaimsPrincipal(Dms.Core.Cookies cookies)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, cookies.Get("uid")));
            claims.Add(new Claim(ClaimTypes.Role, "超级管理员"));
            claims.Add(new Claim(ClaimTypes.UserData, "{uid:'234241369',email:'hn-man@live.cn',right:1023,comments:'1|2|4|8|16|32|64|128|256|512'}"));
            claims.Add(new Claim(ClaimTypes.PrimaryGroupSid, "{用户数据}"));
            var identity = new ClaimsIdentity(claims, "");

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            HttpContext.Current.User = principal;
        }
    }
}