﻿using System.Web.Mvc;
using System.Web.Routing;

namespace Code_College
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{DashboardController}/{action}/{id}",
                defaults: new { controller = "Dash", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}