using DiscountCatalog.Common.Models;
using DiscountCatalog.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiscountCatalog.WebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //using (var context = new UserAddressEntity())
            //{
            //    var address = (from a in context.Address where a.Country == "" select a).FirstOrDefault();
            //}

            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
