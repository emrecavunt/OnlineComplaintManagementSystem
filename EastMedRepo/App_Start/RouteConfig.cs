using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EastMedRepo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.config");
            //routes.IgnoreRoute("assets/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
            //routes.MapRoute("GetLocationsByDeptID",
            //               "User/Add/",
            //               new { controller = "User", action = "GetLocationsByDeptID" },
            //               new[] { "EastMedRepo.Controllers" });
        }
    }
}
