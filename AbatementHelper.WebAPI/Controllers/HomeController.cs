using AbatementHelper.CommonModels.Models;
using AbatementHelper.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbatementHelper.WebAPI.Controllers
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
