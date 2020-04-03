using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Models.Redirect
{
    public class Route
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string FullRoute { get; set; }

        public Route(string actionName, string controllerName, string fullRoute)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            FullRoute = fullRoute;
        }
    }
}