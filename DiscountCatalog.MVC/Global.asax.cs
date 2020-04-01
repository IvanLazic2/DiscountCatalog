using DiscountCatalog.MVC.ModelBinders;
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

        //protected void Application_BeginRequest(Object sender, EventArgs e)
        //{
        //    HttpRuntimeSection runTime = (HttpRuntimeSection)WebConfigurationManager.GetSection("system.web/httpRuntime");
        //    //Approx 100 Kb(for page content) size has been deducted because the maxRequestLength proprty is the page size, not only the file upload size
        //    int maxRequestLength = (runTime.MaxRequestLength - 100) * 1024;

        //    //This code is used to check the request length of the page and if the request length is greater than 
        //    //MaxRequestLength then retrun to the same page with extra query string value action=exception

        //    HttpContext context = ((HttpApplication)sender).Context;
        //    if (context.Request.ContentLength > maxRequestLength)
        //    {
        //        IServiceProvider provider = (IServiceProvider)context;
        //        HttpWorkerRequest workerRequest = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));
        //        // Check if body contains data
        //        if (workerRequest.HasEntityBody())
        //        {
        //            // get the total body length
        //            int requestLength = workerRequest.GetTotalEntityBodyLength();
        //            // Get the initial bytes loaded
        //            int initialBytes = 0;
        //            if (workerRequest.GetPreloadedEntityBody() != null)
        //                initialBytes = workerRequest.GetPreloadedEntityBody().Length;
        //            if (!workerRequest.IsEntireEntityBodyIsPreloaded())
        //            {
        //                byte[] buffer = new byte[512000];
        //                // Set the received bytes to initial bytes before start reading
        //                int receivedBytes = initialBytes;
        //                while (requestLength - receivedBytes >= initialBytes)
        //                {
        //                    // Read another set of bytes
        //                    initialBytes = workerRequest.ReadEntityBody(buffer, buffer.Length);
        //                    // Update the received bytes
        //                    receivedBytes += initialBytes;
        //                }
        //                initialBytes = workerRequest.ReadEntityBody(buffer, requestLength - receivedBytes);
        //            }
        //        }
        //        HttpContext.Current.ClearError();
        //        // Redirect the user to the same page with querystring action=exception. 
        //        //context.Response.Redirect("~/Erros/FicheiroGrande.aspx");
        //        context.Response.Redirect("~/Views/Shared/Errors/FileTooBig.cshtml");
        //    }
        //}

        private void Application_Error(object sender, EventArgs e)
        {
           // HttpContext context = ((HttpApplication)sender).Context;

            var ex = Server.GetLastError();
            var httpException = ex as HttpException ?? ex.InnerException as HttpException;
            if (httpException == null) return;

            if (httpException.WebEventCode == WebEventCodes.RuntimeErrorPostTooLarge)
            {
                //handle the error
                HttpContext.Current.ClearError();
                HttpContext.Current.Response.Redirect("~/Error/FileTooBig");
                //Response.Write("Too big a file, dude"); //for example
            }
        }
    }
}
