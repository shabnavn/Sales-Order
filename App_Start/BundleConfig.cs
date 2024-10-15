using System.Web;
using System.Web.Optimization;

namespace Sales_Order
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            
            bundles.Add(new Bundle("~/Content/scripts").Include(
                      "~/Content/plugins/global/plugins.bundle.js",
                       "~/Content/js/scripts.bundle.js",
                       "~/Content/plugins/custom/fullcalendar/fullcalendar.bundle.js",
                       "~/Content/js/custom/widgets.js",
                       "~/Content/js/custom/apps/chat/chat.js",
                       "~/Content/js/custom/modals/create-app.js",
                       "~/Content/js/custom/modals/upgrade-plan.js",
                        "~/Content/waypoints.js",
                        "~/Content/track_all.js"
                      ));

            bundles.Add(new Bundle("~/Content/DispScript").Include(
                     "~/Content/plugins/global/plugins.bundle.js",
                      "~/Content/js/scripts.bundle.js",
                      "~/Content/plugins/custom/fullcalendar/fullcalendar.bundle.js",
                      "~/Content/js/custom/widgets.js",
                      "~/Content/js/custom/apps/chat/chat.js",
                      "~/Content/js/custom/modals/create-app.js",
                      "~/Content/js/custom/modals/upgrade-plan.js",
                       "~/Content/Dispatch/Schedules.js",
                       "~/Content/Dispatch/ConfirmMap.js"
                     ));

            bundles.Add(new StyleBundle("~/Content/styles").Include(
                      "~/Content/plugins/custom/fullcalendar/fullcalendar.bundle.css",
                      "~/Content/css/style.bundle.css"));

         

        }
    }
}
