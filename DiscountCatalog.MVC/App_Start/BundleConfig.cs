using System.Web;
using System.Web.Optimization;

namespace DiscountCatalog.MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/moment-with-locales.min.js",
                        "~/Scripts/moment.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/bootstrap-datetimepicker.min.js",
                      "~/Scripts/bootstrap-datepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                      "~/Scripts/bootstrap-datetimepicker.min.js",
                      "~/Scripts/datepicker.js"));

            //bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
            //    "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/bootstrap-datepicker3.css"));

            //INDIVIDUAL PAGE STYLES

            bundles.Add(new StyleBundle("~/Content/IndividualPageStyles/Common").Include(
                    "~/Content/IndividualPageStyles/Common.css"));

            bundles.Add(new StyleBundle("~/Content/IndividualPageStyles/Details").Include(
                    "~/Content/IndividualPageStyles/Details/Details_main.css"));

            bundles.Add(new StyleBundle("~/Content/IndividualPageStyles/Create").Include(
                    "~/Content/IndividualPageStyles/Create/Create_main.css"));

            bundles.Add(new StyleBundle("~/Content/IndividualPageStyles/Edit").Include(
                    "~/Content/IndividualPageStyles/Edit/Edit.css"));

            bundles.Add(new StyleBundle("~/Content/IndividualPageStyles/GetAll").Include(
                    "~/Content/IndividualPageStyles/GetAll/GetAll_main.css"));

            bundles.Add(new StyleBundle("~/Content/IndividualPageStyles/Delete").Include(
                    "~/Content/IndividualPageStyles/Delete/Delete_main.css"));

            bundles.Add(new StyleBundle("~/Content/IndividualPageStyles/Register").Include(
                    "~/Content/IndividualPageStyles/Register/AccountTypeSelecion_main.css"));


            bundles.Add(new ScriptBundle("~/Scripts/IndividualPageScripts/Details").Include(
                      "~/Scripts/IndividualPageScripts/Details/Details_main.js"));

            bundles.Add(new ScriptBundle("~/Scripts/IndividualPageScripts/Create").Include(
                      "~/Scripts/IndividualPageScripts/Create/Create_main.js"));

            bundles.Add(new ScriptBundle("~/Scripts/IndividualPageScripts/Edit").Include(
                      "~/Scripts/IndividualPageScripts/Edit/Edit_main.js"));

            bundles.Add(new ScriptBundle("~/Scripts/IndividualPageScripts/GetAll").Include(
                      "~/Scripts/IndividualPageScripts/GetAll/GetAll_main.js"));

            bundles.Add(new ScriptBundle("~/Scripts/IndividualPageScripts/Delete").Include(
                      "~/Scripts/IndividualPageScripts/Delete/Delete_main.js"));


            //FLASH MESSAGES

            bundles.Add(new ScriptBundle("~/Scripts/FlashMessage").Include(
                "~/Scripts/jquery.cookie.js",
                "~/Scripts/jquery.flashmessage.js"));

        }
    }
}
