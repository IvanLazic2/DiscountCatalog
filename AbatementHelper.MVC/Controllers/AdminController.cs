using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.MVC.Repositeories;
using AbatementHelper.MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbatementHelper.MVC.Extensions;
using PagedList;

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

        //[HttpGet]
        [Route("GetAllUsers")]
        public ActionResult GetAllUsers(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            List<WebApiUser> users;

            var result = admin.GetAllUsers();

            if (TempData["Message"] != null && TempData["Success"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                ViewBag.Success = (bool)TempData["Success"];
            }

            users = result.Value;

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.UserName).ToList();
                    break;
                default:
                    users = users.OrderBy(u => u.UserName).ToList();
                    break;
            }

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(users.ToPagedList(pageNumber, pageSize));
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
            Response editUserResponse = admin.EditUser(user);

            TempData["Message"] = editUserResponse.ResponseMessage;
            TempData["Success"] = editUserResponse.Success;

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