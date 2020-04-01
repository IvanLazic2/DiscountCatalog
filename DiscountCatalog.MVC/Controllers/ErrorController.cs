using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiscountCatalog.MVC.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult FileTooBig()
        {
            return View();
        }
    }
}