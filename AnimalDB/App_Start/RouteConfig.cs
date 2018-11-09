using System.Web.Mvc;
using System.Web.Routing;

namespace AnimalDB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "{id}/details",
                defaults: new { controller = "Animals", action = "Details" }
            );

            routes.MapRoute(
                name: null,
                url: "{id}/feeds",
                defaults: new { controller = "Feed", action = "Details" }
            );

            routes.MapRoute(
                name: null,
                url: "{id}/notes",
                defaults: new { controller = "Note", action = "Index" }
            );

            routes.MapRoute(
                name: null,
                url: "{id}/medication",
                defaults: new { controller = "Medication", action = "Index" }
            );

            routes.MapRoute(
                name: null,
                url: "{id}/surgicalnotes",
                defaults: new { controller = "SurgicalNote", action = "Index" }
            );

            routes.MapRoute(
                name: null,
                url: "{id}/cagelocation",
                defaults: new { controller = "CageLocationHistory", action = "Index" }
            );

            routes.MapRoute(
                name: null,
                url: "{id}/aupnumber",
                defaults: new { controller = "EthicsNumberHistory", action = "Index" }
            );

            routes.MapRoute(
                name: null,
                url: "{id}/relationships",
                defaults: new { controller = "Relationships", action = "Details" }
            );

            routes.MapRoute(
                name: null,
                url: "{id}/reports",
                defaults: new { controller = "ClinicalIncidentReports", action = "Index" }
            );

            routes.MapRoute(
                name: "Rename Ethics to AUP",
                url: "AUPNumber/{action}/{id}",
                defaults: new { controller = "EthicsNumber", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "AnimalDB.Web.Controllers" }
            );
        }
    }
}
