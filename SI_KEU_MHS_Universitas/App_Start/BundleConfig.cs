using System.Web;
using System.Web.Optimization;

namespace SI_KEU_MHS_Universitas
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //"~/Scripts/jquery-1.10.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/autoNumeric-1.9.18.js",
                        "~/Scripts/customAutonumeric.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // datePicker 
            bundles.Add(new ScriptBundle("~/plugins/datePicker").Include(
                      "~/Scripts/plugins/datapicker/bootstrap-datepicker.js",
                      "~/Scripts/plugins/datapicker/locales/bootstrap-datepicker.id.js")); //added

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/sweetalert2.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-admin-theme.css",
                      "~/Content/sweetalert2.css",
                      "~/Content/plugins/datapicker/datepicker3.css",
                      "~/Content/Site.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
