using System.Web;
using System.Web.Optimization;

namespace QsAdmin
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/vendors/bootstrap/dist/js/bootstrap.min.js",
                        "~/Content/vendors/fastclick/lib/fastclick.js",
                        "~/Content/vendors/nprogress/nprogress.js",
                        "~/Content/vendors/iCheck/icheck.min.js",
                        "~/Content/gentelella-master/js/custom.min.js",
                        "~/Content/vendors/datetimepicker/jquery.datetimepicker.full.min.js",
                        "~/Scripts/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Content/gentelella-master/js/custom.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/vendors/bootstrap/dist/css/bootstrap.min.css",
                      //"~/Content/vendors/font-awesome/css/font-awesome.min.css", Azure上で出ない
                      "~/Content/vendors/nprogress/nprogress.css",
                      "~/Content/vendors/iCheck/skins/flat/green.css",
                      "~/Content/vendors/datetimepicker/jquery.datetimepicker.min.css",
                      "~/Content/gentelella-master/css/custom.min.css",
                      "~/Content/Site.css"));
        }
    }
}
