using System.Web;
using System.Web.Optimization;

namespace Dms.Master
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/uiframework/js/jquery.min.js",
                      "~/Content/uiframework/js/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/utils").Include(
                    "~/Scripts/dms.master.base.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/uiframework/css/bootstrap.min.css",
                      "~/Content/uiframework/css/font-awesome.min.css",
                      "~/Content/uiframework/css/plugins/dataTables/dataTables.bootstrap.css",
                      "~/Content/uiframework/css/style.min.css",
                      "~/Content/default/site.css"));
        }
    }
}
