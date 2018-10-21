using System.Web;
using System.Web.Optimization;

namespace TestApplication
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

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/application").Include(
                        "~/Scripts/knockout-3.3.0.js",
                        "~/Scripts/Application/models.js",
                        "~/Scripts/Application/data-access-layer.js",
                        "~/Scripts/Application/application-viewmodel.js",
                        "~/Scripts/Application/view.js",
                        "~/Scripts/Enum/*.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"));


            var lessBundle = new Bundle("~/Content/BundledLess").IncludeDirectory("~/content/less", "*.less");
            lessBundle.Transforms.Add(new LessTransform());
            bundles.Add(lessBundle);

            BundleTable.EnableOptimizations = true;
        }
    }
}
