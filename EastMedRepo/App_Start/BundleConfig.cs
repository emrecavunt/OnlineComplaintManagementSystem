using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace EastMedRepo
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/assets/js/jquery-{version}.js",
                        "~/assets/js/jquery.jqGrid.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/assets/js/jquery.validate*",
                        "~/Scripts/jquery.validate.unobtrusive*"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                      "~/assets/js/jquery-ui-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/assets/js/bootstrap/dist/js/bootstrap.min.js",
                     "~/Scripts/respond.js",
                     "~/assets/js/bootstrap-wysiwyg.min.js"
                    ));

            bundles.Add(new Bundle("~/bundles/sweetAlertCss")
      .Include("~/Content/bootstrap.min.css")
      .Include("~/Content/sweetalert/sweet-alert.css")
               );
            bundles.Add(new ScriptBundle("~/bundles/sweetAlertJS")
               .Include("~/Content/sweetalert/sweet-alert.js")
               .Include("~/Scripts/jquery-{version}.js")
               );
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                        "~/assets/js/jquery.dataTables.min.js",
                        "~/assets/js/jquery.dataTables.bootstrap.min.js",
                          "~/assets/js/dataTables.buttons.min.js",
              "~/assets/js/buttons.flash.min.js",
              "~/assets/js/buttons.html5.min.js",
              "~/assets/js/buttons.print.min.js",
              "~/assets/js/buttons.colVis.min.js",
              "~/assets/js/dataTables.select.min.js"
                        ));                               
            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/assets/css/ui.jqgrid.min.css",
                      "~/assets/css/bootstrap.min.css",
                      "~/assets/css/bootstrap-datepicker3.min.css",
                      "~/assets/css/bootstrap-duallistbox.min.css",
                      "~/assets/font-awesome/4.5.0/css/font-awesome.min.css",
                      "~/assets/css/jquery-ui.min.css",
                      "~/assets/css/fonts.googleapis.com.css",
                      "~/assets/css/ace-rtl.min.css",
                      "~/assets/css/ace-skins.min.css",
                      "~/assets/datatables/media/css/jquery.dataTables.min.css",
                      "~/assets/datatables/media/css/jquery.dataTables_themeroller.css"

                      ));
            bundles.Add(new StyleBundle("~/bundles/Default").Include(
                "~/Content/DefaultStyle.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/eastmed").Include(
                "~/assets/js/ace-extra.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendors").Include(
                "~/assets/js/jquery.easypiechart.min.js",
                "~/assets/js/jquery.sparkline.index.min.js"
                ////"~/assets/js/jquery.flot.min.js",
                //"~/assets/js/jquery.flot.pie.min.js",
                /*"~/assets/js/jquery.flot.resize.min.js"*/));

            bundles.Add(new ScriptBundle("~/bundles/elements").Include(
                 "~/assets/js/ace-elements.min.js",
                 "~/assets/js/ace.min.js",
                 "~/Scripts/bootbox.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/typeahead").Include("~/Scripts/typeahead.bundle*"));
            
        }
    }
}
