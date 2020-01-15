using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.MVC.Repositeories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbatementHelper.MVC.Extensions;
using PagedList;
using System.Threading.Tasks;

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
        public async Task<ActionResult> GetAllUsers(string sortOrder, string currentFilter, string searchString, int? page)
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

            WebApiListOfUsersResult result = await admin.GetAllUsersAsync();

            List<WebApiUser> users = result.Users;

            if (TempData["Message"] != null && TempData["Success"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                ViewBag.Success = (bool)TempData["Success"];
            }

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
        public async Task<ActionResult> Edit(string id)
        {
            WebApiUser user = new WebApiUser();

            user = await admin.EditUserAsync(id);

            return View(user);
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<ActionResult> Edit(WebApiUser user)
        {
            Response editUserResponse = await admin.EditUserAsync(user);

            TempData["Message"] = editUserResponse.Message;
            TempData["Success"] = editUserResponse.Success;

            return RedirectToAction("GetAllUsers");
        }

        [HttpGet]
        [Route("Details/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            WebApiUser user = await admin.DetailsUserAsync(id);

            return View(user);
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            WebApiUser user = await admin.DetailsUserAsync(id);

            return View(user);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult> Delete(WebApiUser user)
        {
            await admin.DeleteUserAsync(user);

            return RedirectToAction("GetAllUsers");
        }

        [HttpGet]
        [Route("Restore/{id}")]
        public async Task<ActionResult> Restore(string id)
        {
            await admin.RestoreUserAsync(id);

            return RedirectToAction("GetAllUsers");
        }
    }
}