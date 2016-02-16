using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Makers.Store
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Category",
                url: "category/{category}",
                defaults: new { controller = "Category", action = "Index", category = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Model",
            //    url: "model/upload-and-buy/{no}",
            //    defaults: new { controller = "Model", action = "Index", no = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{no}",
                defaults: new { controller = "Home", action = "Index", no = UrlParameter.Optional }
            );
        }
    }
}