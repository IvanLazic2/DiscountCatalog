using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.MVC.Repositeories;
using AbatementHelper.MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbatementHelper.MVC.Controllers
{
    [RoutePrefix("Admin")]
    public class AdminController : Controller
    {
        private AdminRepository admin = new AdminRepository();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public ActionResult GetAllUsers()
        {
            List<WebApiUser> users;

            var result = admin.GetAllUsers();

            ViewBag.Success = result.Success;
            ViewBag.Message = result.Message;

            users = result.Value;

            return View(users);
        }

        [HttpGet]
        [Route("EditUser/{id}")]
        public ActionResult EditUser(string id)
        {
            WebApiUser user = new WebApiUser();

            user = admin.EditUser(id);

            return View(user);
        }

        [HttpPost]
        [Route("EditUser")]
        public ActionResult EditUser(WebApiUser user)
        {
            admin.EditUser(user);

            return RedirectToAction("GetAllUsers");
        }

        [HttpGet]
        [Route("DetailsUser/{id}")]
        public ActionResult DetailsUser(string id)
        {
            WebApiUser user = admin.DetailsUser(id);

            return View(user);
        }

        [HttpGet]
        [Route("DeleteUser/{id}")]
        public ActionResult DeleteUser(string id)
        {
            WebApiUser user = admin.DetailsUser(id);

            return View(user);
        }

        [HttpPost]
        [Route("DeleteUser")]
        public ActionResult DeleteUser(WebApiUser user)
        {
            admin.DeleteUser(user);

            return RedirectToAction("GetAllUsers");
        }

        [HttpGet]
        [Route("RestoreUser/{id}")]
        public ActionResult RestoreUser(string id)
        {
            admin.RestoreUser(id);

            return RedirectToAction("GetAllUsers");
        }
    }
}