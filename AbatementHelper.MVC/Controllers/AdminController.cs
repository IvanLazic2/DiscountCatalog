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

            user = admin.EditGet(id);

            return View(user); 
        }

        [HttpPost]
        public ActionResult Edit(DataBaseUser user)
        {
            AdminRepository admin = new AdminRepository();

            if (admin.EditPost(user))
            {
                return RedirectToAction("GetAllUsers");
            }

            return View(user);
        }

        
        public ActionResult Delete(string id)
        {
            AdminRepository admin = new AdminRepository();

            admin.Delete(id);

            return RedirectToAction("GetAllUsers");
        }
    }
}