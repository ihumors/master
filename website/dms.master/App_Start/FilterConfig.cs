using Dms.Master.Filters;
using System.Web;
using System.Web.Mvc;

namespace Dms.Master
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new PerformanceAttribute());
            filters.Add(new ExceptionFilterAttribute());
        }
    }
}
