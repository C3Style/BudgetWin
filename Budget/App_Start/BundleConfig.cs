using System.Web;
using System.Web.Optimization;

namespace Budget
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/scripts").Include(
                        "~/Scripts/jquery.js",
                        "~/Scripts/jquery-ui.min.js",
                        "~/Scripts/jquery.dataTables.js",
                        // "~/Scripts/jquery-sortable.js",
                        "~/Scripts/angular.min.js",
                        "~/Scripts/angular-mocks.js",
                        "~/Scripts/angular-ui-bootstrap.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/raphael.min.js",
                        "~/Scripts/morris.min.js",
                        "~/Scripts/jquery.sparkline.min.js",
                        "~/Scripts/jquery-jvectormap-1.2.2.min.js",
                        "~/Scripts/jquery-jvectormap-world-mill-en.js",
                        "~/Scripts/jquery.knob.min.js",
                        "~/Scripts/moment-with-locales.min.js",
                        "~/Scripts/daterangepicker.js",
                        "~/Scripts/bootstrap-datepicker.min.js",
                        "~/Scripts/bootstrap3-wysihtml5.all.min.js",
                        "~/Scripts/jquery.slimscroll.min.js",
                        "~/Scripts/fastclick.js",
                        "~/Scripts/adminlte.min.js",
                        "~/Scripts/dashboard.js",
                        "~/Scripts/dataTables/dataTables.bootstrap4.js",
                        "~/Scripts/toastr.min.js",
                        "~/Scripts/bootstrap-slider.js",
                        "~/Scripts/jsgrid/jsgrid.js",
                        "~/Scripts/jsgrid/i18n/jsgrid-fr.js",
                        "~/Scripts/simple-iconpicker.js",
                        "~/Scripts/xeditable/xeditable.js",
                        "~/Scripts/site.js",
                        "~/Scripts/app/transaction-controller.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/AdminLTE.min.css",
                      "~/Content/_all-skins.min.css",
                      "~/Content/morris.css",
                      "~/Content/jquery-jvectormap.css",
                      "~/Content/bootstrap-datepicker.min.css",
                      "~/Content/bootstrap3-wysihtml5.min.css",
                      "~/Content/jquery-ui/jquery-ui.css",
                      "~/Content/jquery-ui/jquery-ui.structure.css",
                      "~/Content/jquery-ui/jquery-ui.theme.css",
                      "~/Content/dataTables/dataTables.bootstrap4.css",
                      "~/Content/toastr.min.css",
                      "~/Content/bootstrap-slider.css",
                      "~/Content/jsgrid/jsgrid.min.css",
                      "~/Content/jsgrid/jsgrid-theme.css",
                      "~/Content/simple-iconpicker.css",
                      "~/Content/xeditable/xeditable.css",
                      "~/Content/site.css"
                      ));

        }
    }
}
