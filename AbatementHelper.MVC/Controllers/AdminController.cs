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
    public class AdminController : Controller
    {
        public ActionResult GetAllUsers()
        {
            List<DataBaseUser> users;

            AdminRepository admin = new AdminRepository();

            ViewBag.Success = admin.GetAllUsers().Success;
            ViewBag.Message = admin.GetAllUsers().Message;

            users = admin.GetAllUsers().Value;

            return View(users);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            DataBaseUser user = new DataBaseUser();

            AdminRepository admin = new AdminRepository();

            user = admin.Edit(id);

            return View(user); 
        }

        [HttpPost]
        public ActionResult Edit(DataBaseUser user)
        {
            AdminRepository admin = new AdminRepository();

            if (admin.Edit(user))
            {
                return RedirectToAction("GetAllUsers");
            }

            return View(user);
        }

        [HttpGet]
        public ActionResult GetDelete(string id)
        {
            DataBaseUser user = new DataBaseUser();

            AdminRepository admin = new AdminRepository();

            user = admin.GetDelete(id);

            return View(user);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            AdminRepository admin = new AdminRepository();

            admin.Delete(id);

            return RedirectToAction("GetAllUsers");
        }

        //[HttpGet]
        //public ActionResult Delete(string id)
        //{
        //    DataBaseUser user = new DataBaseUser();

        //    AdminRepository admin = new AdminRepository();

        //    user = admin.Delete(id);

        //    return View(user);
        //}

        //[HttpPost]
        //public ActionResult Delete(DataBaseUser user) 
        //{
        //    AdminRepository admin = new AdminRepository();

        //    admin.Delete(user);

        //    return RedirectToAction("GetAllUsers");
        //}
    }
}