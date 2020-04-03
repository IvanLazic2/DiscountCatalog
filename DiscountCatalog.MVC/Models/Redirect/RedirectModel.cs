using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Models.Redirect
{
    public class RedirectModel
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public RedirectModel(string controllerName, string actionName)
        {
            ControllerName = controllerName;
            ActionName = actionName;
        }
    }
}