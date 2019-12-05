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
        [Route("Edit/{id}")]
        public ActionResult Edit(string id)
        {
            WebApiUser user = new WebApiUser();

            user = admin.EditUser(id);

            return View(user);
        }

        [HttpPost]
        [Route("Edit")]
        public ActionResult Edit(WebApiUser user)
        {
            admin.EditUser(user);

            return RedirectToAction("GetAllUsers");
        }

        [HttpGet]
        [Route("Details/{id}")]
        public ActionResult Details(string id)
        {
            WebApiUser user = admin.DetailsUser(id);

            return View(user);
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public ActionResult Delete(string id)
        {
            WebApiUser user = admin.DetailsUser(id);

            return View(user);
        }

        [HttpPost]
        [Route("Delete")]
        public ActionResult Delete(WebApiUser user)
        {
            admin.DeleteUser(user);

            return RedirectToAction("GetAllUsers");
        }

        [HttpGet]
        [Route("Restore/{id}")]
        public ActionResult Restore(string id)
        {
            admin.RestoreUser(id);

            return RedirectToAction("GetAllUsers");
        }
    }
}