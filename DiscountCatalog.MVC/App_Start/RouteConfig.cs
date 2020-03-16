using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DiscountCatalog.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "PasswordLogin",
            //    url: "User/PasswordLogin/{email}",
            //    defaults: new { controller = "User", action = "PasswordLogin" }
            //);

            routes.MapRoute(
                name: "DeleteRoleId",
                url: "Delete/{role}/{id}",
                defaults: new { controller = "Admin", action = "Delete", role = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "RestoreRoleId",
                url: "Restore/{role}/{id}",
                defaults: new { controller = "Admin", action = "Restore", role = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GetAllUsers",
                url: "Admin/GetAllUsers/{role}",
                defaults: new { controller = "Admin", action = "GetAllUsers", role = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Admin/GetAllStores",
                url: "GetAllStores/{role}",
                defaults: new { controller = "Admin", action = "GetAllStores", role = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
