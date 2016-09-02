using System.Web;
using System.Web.Optimization;

namespace SIA_Universitas
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modalform").Include(
                        "~/Scripts/modalForm.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // datePicker styles
            bundles.Add(new StyleBundle("~/plugins/datePickerStyles").Include(
                      "~/Content/plugins/datapicker/datepicker3.css"));

            // datePicker 
            bundles.Add(new ScriptBundle("~/plugins/datePicker").Include(
                      "~/Scripts/plugins/datapicker/bootstrap-datepicker.js",
                      "~/Scripts/plugins/datapicker/locales/bootstrap-datepicker.id.js")); //added

            // dateTimePicker styles
            bundles.Add(new StyleBundle("~/plugins/dateTimePickerStyles").Include(
                      "~/Content/plugins/datapicker/bootstrap-datetimepicker.css"));

            // dateTimePicker 
            bundles.Add(new ScriptBundle("~/plugins/dateTimePicker").Include(
                      "~/Scripts/plugins/datapicker/bootstrap-datetimepicker.js",
                      "~/Scripts/plugins/datapicker/locales/bootstrap-datetimepicker.id.js")); //added

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/sweetalert2.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-admin-theme.css",
                      "~/Content/sweetalert2.css",
                      "~/Content/plugins/Chosen/chosen.css",
                      "~/Content/Site.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
