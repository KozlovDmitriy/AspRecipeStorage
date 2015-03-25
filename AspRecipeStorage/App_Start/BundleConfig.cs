using System.Web;
using System.Web.Optimization;

namespace AspRecipeStorage
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusive").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/Gridmvc.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Material/css").Include(
                        "~/Material/css/roboto.min.css",
                        "~/Material/css/material-fullpalette.min.css",
                        "~/Material/css/ripples.min.css",
                        "~/Material/css/bootstrap-elements.css"));

            bundles.Add(new StyleBundle("~/Material/js").Include(
                      "~/Material/js/material.min.js",
                      "~/Material/js/ripples.min.js"));
        }
    }
}
