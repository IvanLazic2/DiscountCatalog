using AbatementHelper.CommonModels.Models;
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
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("GetAllUsers/{role}")]
        public ActionResult GetAllUsers(string role)
        {
            List<DataBaseUser> users;

            AdminRepository admin = new AdminRepository();

            ViewBag.Success = admin.GetAllUsers(role).Success;
            ViewBag.Message = admin.GetAllUsers(role).Message;

            users = admin.GetAllUsers(role).Value;

            return View(users);
        }

        [HttpGet]
        [Route("GetAllStores/{role}")]
        public ActionResult GetAllStores(string role)
        {
            List<DataBaseStore> stores;

            AdminRepository admin = new AdminRepository();

            ViewBag.Success = admin.GetAllStores().Success;
            ViewBag.Message = admin.GetAllStores().Message;

            stores = admin.GetAllStores().Value;

            return View(stores);
        }

        [HttpGet]
        [Route("EditUser/{id}")]
        public ActionResult EditUser(string id)
        {
            DataBaseUser user = new DataBaseUser();

            AdminRepository admin = new AdminRepository();

            user = admin.EditUser(id);

            return View(user);
        }

        [HttpPost]
        [Route("EditUser/{id}")]
        public ActionResult EditUser(DataBaseUser user)
        {
            AdminRepository admin = new AdminRepository();

            if (admin.EditUser(user))
            {
                if (user.Role == "User")
                {
                    return RedirectToAction("GetAllUsers/User");
                }
                else if (user.Role == "StoreAdmin")
                {
                    return RedirectToAction("GetAllUsers/StoreAdmin");
                }
                else if (user.Role == "Admin")
                {
                    return RedirectToAction("GetAllUsers/Admin");
                }
                else if (user.Role == "Store")
                {
                    return RedirectToAction("GetAllUsers/Store");
                }
            }

            return View(user);
        }

        [HttpGet]
        [Route("EditUser/{id}")]
        public ActionResult EditStore(string id)
        {
            DataBaseStore store = new DataBaseStore();

            AdminRepository admin = new AdminRepository();

            store = admin.EditStore(id);

            return View(store);
        }

        [HttpPost]
        [Route("EditUser/{id}")]
        public ActionResult EditStore(DataBaseStore store)
        {
            AdminRepository admin = new AdminRepository();

            if (admin.EditStore(store))
            {
                return RedirectToAction("GetAllStores");
            }

            return View(store);
        }

        [Route("Delete/{role}/{id}")]
        public ActionResult Delete(string role, string id)
        {
            AdminRepository admin = new AdminRepository();

            admin.Delete(role, id);

            return View("~/Views/Admin/Index.cshtml");
        }

        [Route("Restore/{role}/{id}")]
        public ActionResult Restore(string role, string id)
        {
            AdminRepository admin = new AdminRepository();

            admin.Restore(role, id);

            return View("~/Views/Admin/Index.cshtml");
        }

    }
}