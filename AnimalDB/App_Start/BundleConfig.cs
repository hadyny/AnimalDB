using System.Web;
using System.Web.Optimization;

namespace AnimalDB
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/select2.js",
                        "~/Scripts/select2-sort.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/moment.js",
                        "~/Scripts/bootstrap-datetimepicker.js",
                        "~/Scripts/DateFunctions.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                        "~/Scripts/knockout-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/umd/popper.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/fontawesome-all.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap/bootstrap.css",
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/select2.css",
                      "~/Content/uploadfile.css",
                      "~/Content/fa-svg-with-js.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/printcss").Include("~/Content/siteprint.css"));
            bundles.Add(new StyleBundle("~/Content/printcss2").Include("~/Content/siteprint2.css"));
        }
    }
}
