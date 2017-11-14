using System.Web;
using System.Web.Optimization;

namespace Budget
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.1.1.js",
                        "~/Scripts/jquery.dataTables.js",
                        "~/Scripts/jquery-ui/jquery-ui.js",
                        "~/Scripts/angular.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap/bootstrap.js",
                      "~/Scripts/respond.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Scripts/dataTables/dataTables.bootstrap4.js",
                      // "~/Scripts/chart.js/Chart.min.js",
                      "~/Scripts/sb/sb-admin-datatables.min.js",
                      "~/Scripts/sb/sb-admin.min.js",
                      "~/Scripts/toastr.min.js",
                      "~/Scripts/jsgrid/jsgrid.js",
                      "~/Scripts/jsgrid/i18n/jsgrid-fr.js",
                      "~/Scripts/simple-iconpicker.js",
                      "~/Scripts/jquery-sortable.js",
                      "~/Scripts/app/transaction-controller.js"
                      // "~/Scripts/sb/sb-admin-charts.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap/bootstrap.css",
                      "~/Content/jquery-ui/jquery-ui.css",
                      "~/Content/jquery-ui/jquery-ui.structure.css",
                      "~/Content/jquery-ui/jquery-ui.theme.css",
                      "~/Content/dataTables/dataTables.bootstrap4.css",
                      "~/Content/font-awesome/font-awesome.min.css",
                      "~/Content/toastr.min.css",
                      "~/Content/jsgrid/jsgrid.min.css",
                      "~/Content/jsgrid/jsgrid-theme.css",
                      "~/Content/sb/sb-admin.css",
                      "~/Content/simple-iconpicker.css",
                      "~/Content/site.css"
                      ));

        }
    }
}
