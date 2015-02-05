using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NZBlog.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               "Default3", 
               "{pageIndex}", 
               new { controller = "Blog", action = "Index", pageIndex = UrlParameter.Optional }, // Parameter defaults
               new { pageIndex = @"[\d]" }
           );
            routes.MapRoute(
               "Default2", // Route name
               "blogList/{typeid}/{pageIndex}", // URL with parameters
               new { controller = "Blog", action = "GetTypeList", typeid = 0, pageIndex = UrlParameter.Optional }, // Parameter defaults
               new { typeid = @"[\d]"}
           );
            routes.MapRoute(
               "Default1", 
               "blog/{id}", 
               new { controller = "Blog", action = "BlogDetail", id = UrlParameter.Optional }, // Parameter defaults
               new { id = @"[\d]" }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "blog", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}