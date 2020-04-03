using DiscountCatalog.MVC.Cookies.Contractor;
using DiscountCatalog.MVC.Cookies.Implementation;
using DiscountCatalog.MVC.Factory;
using DiscountCatalog.MVC.ModelBinders;
using DiscountCatalog.MVC.Models.Redirect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Management;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DiscountCatalog.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly List<string> _allowedControllers = new List<string>
        {
            "Home",
            "Error"
        };
        private readonly List<string> _allowedActions = new List<string>
        {
            "Login",
            "Logout",
            "AccountTypeSelection",
            "Register"
        };
        private RedirectFactory _redirectFactory;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            System.Web.Mvc.ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());

            //System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime), new CustomDateModelBinder());
        }

        protected void Application_BeginRequest()
        {
            var currentCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            currentCulture.NumberFormat.NumberDecimalSeparator = ".";
            currentCulture.NumberFormat.NumberGroupSeparator = " ";
            currentCulture.NumberFormat.CurrencyDecimalSeparator = ".";

            Thread.CurrentThread.CurrentCulture = currentCulture;
            Thread.CurrentThread.CurrentUICulture = currentCulture;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
            HttpContext currentContext = HttpContext.Current;

            RouteData routeData = RouteTable.Routes.GetRouteData(context);

            if (routeData != null)
            {
                string controllerName = routeData.GetRequiredString("controller");
                string actionName = routeData.GetRequiredString("action");

                _redirectFactory = new RedirectFactory(currentContext, context, _allowedControllers, _allowedActions);
                _redirectFactory.GenerateRedirect(new RedirectModel(controllerName, actionName));
            }
        }

        private void Application_Error(object sender, EventArgs e)
        {
           // HttpContext context = ((HttpApplication)sender).Context;

            var ex = Server.GetLastError();
            var httpException = ex as HttpException ?? ex.InnerException as HttpException;
            if (httpException == null) return;

            if (httpException.WebEventCode == WebEventCodes.RuntimeErrorPostTooLarge)
            {
                HttpContext.Current.ClearError();
                HttpContext.Current.Response.Redirect("~/Error/FileTooBig");
                //Response.Write("Too big a file, dude");
            }
        }
    }
}
