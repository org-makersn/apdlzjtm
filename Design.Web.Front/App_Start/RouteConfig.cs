using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Design.Web.Front
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //routes.MapRoute(
            //    name: "Blog",
            //    url: "makers/{url}",
            //    defaults: new { controller = "Profile", action = "index", url = UrlParameter.Optional }

            //    );

            routes.MapRoute(
                name: "cate",
                url: "cate/{cateNm}",
                defaults: new { controller = "article", action = "index", cateNm = UrlParameter.Optional },
                namespaces: new[] { "Design.Web.Front" }
                );

            routes.MapRoute(
                name: "beforeArticle",
                url: "items/detail/id/{no}",
                defaults: new { controller = "article", action = "detail", no = UrlParameter.Optional },
                namespaces: new[] { "Design.Web.Front" }
                );

            routes.MapRoute(
                name: "account",
                url: "account/{action}/{no}",
                defaults: new { controller = "account", action = "Index", no = UrlParameter.Optional },
                namespaces: new[] { "Design.Web.Front" }
            );

            routes.MapRoute(
               name: "design",
               url: "design/{action}/{no}",
               defaults: new { controller = "article", action = "Index", no = UrlParameter.Optional },
               namespaces: new[] { "Design.Web.Front" }
           );

            routes.MapRoute(
                        name: "printing",
                        url: "printing/{action}/{no}",
                        defaults: new { controller = "printing", action = "Index", no = UrlParameter.Optional },
                        namespaces: new[] { "Design.Web.Front" }
                    );

            routes.MapRoute(
                name: "printingprofile",
                url: "printingprofile/{action}/{no}",
                defaults: new { controller = "printingprofile", action = "Index", no = UrlParameter.Optional },
                namespaces: new[] { "Design.Web.Front" }
            );

            routes.MapRoute(
                name: "order",
                url: "order/{action}/{no}",
                defaults: new { controller = "order", action = "Index", no = UrlParameter.Optional },
                namespaces: new[] { "Design.Web.Front" }
            );

            routes.MapRoute(
                name: "article",
                url: "article/{action}/{no}",
                defaults: new { controller = "article", action = "Index", no = UrlParameter.Optional },
                namespaces: new[] { "Design.Web.Front" }
            );

            routes.MapRoute(
                name: "profile",
                url: "profile/{action}/{no}",
                defaults: new { controller = "profile", action = "Index", no = UrlParameter.Optional },
                namespaces: new[] { "Design.Web.Front" }
            );

            routes.MapRoute(
                name: "info",
                url: "info/{action}/{no}",
                defaults: new { controller = "info", action = "Index", no = UrlParameter.Optional },
                namespaces: new[] { "Design.Web.Front" }
            );

            routes.MapRoute(
                name: "base",
                url: "base/{action}/{no}",
                defaults: new { controller = "base", action = "Index", no = UrlParameter.Optional },
                namespaces: new[] { "Design.Web.Front" }
            );
            routes.MapRoute(
                name: "gmap",
                url: "gmap/{action}/{no}",
                defaults: new { controller = "gmap", action = "Index", no = UrlParameter.Optional },
                namespaces: new[] { "Design.Web.Front" }
            );
            routes.MapRoute(
                name: "cleanup",
                url: "cleanup/{action}/{no}",
                defaults: new { controller = "cleanup", action = "Index", no = UrlParameter.Optional },
                namespaces: new[] { "Design.Web.Front" }
            );

            routes.MapRoute(
                 name: "blog",
                 url: "{url}",
                 defaults: new { controller = "main", action = "index", url = UrlParameter.Optional }
             );

            routes.MapRoute(
                 name: "spot",
                 url: "spot/{url}",
                 defaults: new { controller = "PrintingProfile", action = "index", url = UrlParameter.Optional }
             );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{no}",
                defaults: new { controller = "Main", action = "Index", no = UrlParameter.Optional },
                namespaces: new[] { "Design.Web.Front" }
            );





            //routes.MapRoute(
            //  name: "Muilti Lang",
            //  url: "{lang}/{controller}/{action}/{id}",
            //  defaults: new { lang = "ko-kr", controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}