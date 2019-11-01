using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbatementHelper.MVC.Controllers
{
    public class StoreAdminController : Controller
    {
        public ActionResult GetAllStores()
        {
            return View();
        }
    }
}