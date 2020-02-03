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
using AbatementHelper.CommonModels.CreateModels;

namespace AbatementHelper.MVC.Controllers
{
    [RoutePrefix("Admin")]
    public class AdminController : Controller
    {
        private AdminRepository adminRepository = new AdminRepository();

        public ActionResult Index()
        {
            return View();
        }

        [Route("CreateUser")]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<ActionResult> CreateUser(CreateUserModel user)
        {
            var result = await adminRepository.CreateUserAsync(user);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(user);
            }
            else
            {
                return RedirectToAction("GetAllUsers");
            }
        }

        [Route("CreateStore")]
        public ActionResult CreateStore()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateStore")]
        public async Task<ActionResult> CreateStore(WebApiStore store)
        {
            WebApiResult result = await adminRepository.CreateStoreAsync(store);

            if (result.Success)
            {
                return RedirectToAction("GetAllStores");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(store);
            }
        }

        [Route("CreateManager")]
        public ActionResult CreateManager()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateManager")]
        public async Task<ActionResult> CreateManager(CreateManagerModel manager)
        {
            WebApiResult result = await adminRepository.CreateManagerAsync(manager);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(manager);
            }

            return RedirectToAction("GetAllManagers");
        }

        [Route("GetAllUsers")]
        public async Task<ActionResult> GetAllUsers(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var users = new List<WebApiUser>();

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

            WebApiListOfUsersResult result = await adminRepository.GetAllUsersAsync();

            if (result.Success)
            {
                users = result.Users;
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
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

        [Route("GetAllStores")]
        public async Task<ActionResult> GetAllStores(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var stores = new List<WebApiStore>();

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

            WebApiListOfStoresResult result = await adminRepository.GetAllStoresAsync();

            if (result.Success)
            {
                stores = result.Stores;
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                stores = stores.Where(u => u.StoreName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    stores = stores.OrderByDescending(u => u.StoreName).ToList();
                    break;
                default:
                    stores = stores.OrderBy(u => u.StoreName).ToList();
                    break;
            }

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(stores.ToPagedList(pageNumber, pageSize));
        }

        //get all managers

        [HttpGet]
        [Route("EditUser/{id}")]
        public async Task<ActionResult> EditUser(string id)
        {
            WebApiUser user = new WebApiUser();

            WebApiUserResult result = await adminRepository.EditUserAsync(id);

            if (result.Success)
            {
                user = result.User;
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return View(user);
        }

        [HttpGet]
        [Route("EditStore/{id}")]
        public async Task<ActionResult> EditStore(string id)
        {
            WebApiStore store = new WebApiStore();

            WebApiStoreResult result = await adminRepository.EditStoreAsync(id);

            if (result.Success)
            {
                store = result.Store;
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return View(store);
        }

        [HttpPost]
        [Route("EditUser")]
        public async Task<ActionResult> EditUser(WebApiUser user)
        {
            WebApiResult result = await adminRepository.EditUserAsync(user);

            if (result.Success)
            {
                return RedirectToAction("GetAllUsers");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(user);
            }
        }

        [HttpPost]
        [Route("EditStore")]
        public async Task<ActionResult> EditStore(WebApiStore store)
        {
            WebApiResult result = await adminRepository.EditStoreAsync(store);

            if (result.Success)
            {
                return RedirectToAction("GetAllStores");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(store);
            }
        }

        [HttpGet]
        [Route("UserDetails/{id}")]
        public async Task<ActionResult> UserDetails(string id)
        {
            WebApiUser user = new WebApiUser();

            WebApiUserResult result = await adminRepository.UserDetailsAsync(id);

            if (result.Success)
            {
                user = result.User;
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return View(user);
        }

        [HttpGet]
        [Route("StoreDetails/{id}")]
        public async Task<ActionResult> StoreDetails(string id)
        {
            WebApiStore store = new WebApiStore();

            WebApiStoreResult result = await adminRepository.StoreDetailsAsync(id);

            if (result.Success)
            {
                store = result.Store;
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return View(store);
        }

        [HttpGet]
        [Route("DeleteUser/{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            WebApiUser user = new WebApiUser();

            WebApiUserResult result = await adminRepository.UserDetailsAsync(id);

            if (result.Success)
            {
                user = result.User;
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return View(user);
        }

        [HttpGet]
        [Route("DeleteStore/{id}")]
        public async Task<ActionResult> DeleteStore(string id)
        {
            WebApiStore store = new WebApiStore();

            WebApiStoreResult result = await adminRepository.StoreDetailsAsync(id);

            if (result.Success)
            {
                store = result.Store;
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return View(store);
        }

        [HttpPost]
        [Route("DeleteUser")]
        public async Task<ActionResult> DeleteUser(WebApiUser user)
        {
            WebApiResult result = await adminRepository.DeleteUserAsync(user);

            if (result.Success)
            {
                return RedirectToAction("GetAllUsers");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(user);
            }
        }

        [HttpPost]
        [Route("DeleteStore")]
        public async Task<ActionResult> DeleteStore(WebApiStore store)
        {
            WebApiResult result = await adminRepository.DeleteStoreAsync(store);

            if (result.Success)
            {
                return RedirectToAction("GetAllStores");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(store);
            }
        }

        [HttpGet]
        [Route("RestoreUser/{id}")]
        public async Task<ActionResult> RestoreUser(string id)
        {
            WebApiResult result = await adminRepository.RestoreUserAsync(id);

            if (result.Success)
            {
                return RedirectToAction("GetAllUsers");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return RedirectToAction("GetAllUsers");
            }
        }

        [HttpGet]
        [Route("RestoreStore/{id}")]
        public async Task<ActionResult> RestoreStore(string id)
        {
            WebApiResult result = await adminRepository.RestoreStoreAsync(id);

            if (result.Success)
            {
                return RedirectToAction("GetAllStores");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return RedirectToAction("GetAllStores");
            }
        }
    }
}