using AbatementHelper.CommonModels.Models;
using AbatementHelper.MVC.Models;
using AbatementHelper.MVC.Repositeories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.CommonModels.CreateModels;
using PagedList;
using AbatementHelper.MVC.Extensions;

namespace AbatementHelper.MVC.Controllers
{
    [RoutePrefix("StoreAdmin")]
    public class StoreAdminController : Controller
    {
        private StoreAdminRepository storeAdmin = new StoreAdminRepository();

        [HttpGet]
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        [Route("GetAllStores")]
        public async Task<ActionResult> GetAllStores(string sortOrder, string currentFilter, string searchString, int? page)
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

            List<WebApiStore> stores;

            stores = await storeAdmin.GetAllStores();

            if (TempData["Message"] != null && TempData["Success"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                ViewBag.Success = (bool)TempData["Success"];
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

        [HttpGet]
        [Route("CreateStore")]
        public ActionResult CreateStore()
        {
            if (TempData["Message"] != null && TempData["Success"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                ViewBag.Success = (bool)TempData["Success"];
            }

            return View();
        }

        [HttpPost]
        [Route("CreateStore")]
        public ActionResult CreateStore(WebApiStore store)
        {
            Response createStoreResponse = storeAdmin.CreateStore(store);

            TempData["Message"] = createStoreResponse.ResponseMessage;
            TempData["Success"] = createStoreResponse.Success;

            return RedirectToAction("GetAllStores");
        }

        [HttpGet]
        [Route("EditStore/{id}")]
        public ActionResult EditStore(string id)
        {
            WebApiStore store = storeAdmin.EditStore(id);

            return View(store);
        }

        [HttpPost]
        [Route("EditStore")]
        public ActionResult EditStore(WebApiStore store)
        {
            Response editStoreResponse = storeAdmin.EditStore(store);

            TempData["Message"] = editStoreResponse.ResponseMessage;
            TempData["Success"] = editStoreResponse.Success;

            return RedirectToAction("GetAllStores");
        }

        [HttpGet]
        [Route("DetailsStore/{id}")]
        public ActionResult DetailsStore(string id)
        {
            WebApiStore store = storeAdmin.DetailsStore(id);

            return View(store);
        }

        [HttpGet]
        [Route("DeleteStore/{id}")]
        public ActionResult DeleteStore(string id)
        {
            WebApiStore store = storeAdmin.DetailsStore(id);

            return View(store);
        }

        [HttpPost]
        [Route("DeleteStore")]
        public ActionResult DeleteStore(WebApiStore store)
        {
            storeAdmin.DeleteStore(store.Id);

            return RedirectToAction("GetAllStores");
        }

        [HttpGet]
        [Route("GetAllDeletedStores")]
        public async Task<ActionResult> GetAllDeletedStores(string sortOrder, string currentFilter, string searchString, int? page)
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

            List<WebApiStore> stores;

            stores = await storeAdmin.GetAllDeletedStores();

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

        [HttpGet]
        [Route("RestoreStore/{id}")]
        public ActionResult RestoreStore(string id)
        {
            storeAdmin.RestoreStore(id);

            return RedirectToAction("GetAllDeletedStores");
        }

        [HttpGet]
        [Route("Select/{id}")]
        public ActionResult Select(string id)
        {
            if (id != null)
            {
                var store = storeAdmin.Select(id);

                if (store != null)
                {
                    Response.Cookies.Add(new HttpCookie("StoreID")
                    {
                        Value = store.Id,
                        HttpOnly = true
                    });
                    Response.Cookies.Add(new HttpCookie("StoreName")
                    {
                        Value = store.StoreName,
                        HttpOnly = true
                    });

                    return RedirectToAction("Index", "Store");
                }
            }

            return RedirectToAction("GetAllStores");
        }

        //[HttpGet]
        [Route("GetAllManagers")]
        public async Task<ActionResult> GetAllManagers(string sortOrder, string currentFilter, string searchString, int? page)
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

            List<WebApiManager> managers;

            managers = await storeAdmin.GetAllManagers();

            if (TempData["Message"] != null && TempData["Success"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                ViewBag.Success = (bool)TempData["Success"];
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                managers = managers.Where(u => u.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    managers = managers.OrderByDescending(u => u.UserName).ToList();
                    break;
                default:
                    managers = managers.OrderBy(u => u.UserName).ToList();
                    break;
            }

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(managers.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Route("CreateManager")]
        public ActionResult CreateManager()
        {
            if (TempData["Message"] != null && TempData["Success"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                ViewBag.Success = (bool)TempData["Success"];
            }

            return View();
        }

        [HttpPost]
        [Route("CreateManager")]
        public ActionResult CreateManager(CreateManagerModel manager)
        {
            var response = storeAdmin.CreateManager(manager);

            TempData["Message"] = response.ResponseMessage;
            TempData["Success"] = response.Success;

            if (response.Success)
            {
                return RedirectToAction("GetAllManagers");
            }
            else
            {
                return RedirectToAction("CreateManager");
            }
        }

        [HttpGet]
        [Route("DetailsManager/{id}")]
        public ActionResult DetailsManager(string id)
        {
            WebApiManager manager = storeAdmin.DetailsManager(id);


            return View(manager);
        }

        [HttpGet]
        [Route("EditManager/{id}")]
        public ActionResult EditManager(string id)
        {
            WebApiManager manager = storeAdmin.EditManager(id);

            if (TempData["Message"] != null && TempData["Success"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                ViewBag.Success = (bool)TempData["Success"];
            }

            return View(manager);
        }

        [HttpPost]
        [Route("EditManager")]
        public ActionResult EditManager(WebApiManager manager)
        {
            Response editManagerResponse = storeAdmin.EditManager(manager);

            TempData["Message"] = editManagerResponse.ResponseMessage;
            TempData["Success"] = editManagerResponse.Success;

            if (editManagerResponse.Success)
            {
                return RedirectToAction("GetAllManagers");
            }
            else
            {
                return RedirectToAction("EditManager");
            }

        }

        [HttpGet]
        [Route("DeleteManager/{id}")]
        public ActionResult DeleteManager(string id)
        {
            WebApiManager manager = storeAdmin.DetailsManager(id);

            return View(manager);
        }

        [HttpPost]
        [Route("DeleteManager")]
        public ActionResult DeleteManager(WebApiManager manager)
        {
            if (storeAdmin.DeleteManager(manager.Id))
            {
                return RedirectToAction("GetAllManagers");
            }
            else
            {
                return RedirectToAction("Delete");
            }
        }

        //[HttpGet]
        [Route("GetAllDeletedManager")]
        public async Task<ActionResult> GetAllDeletedManagers(string sortOrder, string currentFilter, string searchString, int? page)
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

            List<WebApiManager> managers = await storeAdmin.GetAllDeletedManagers();


            if (!string.IsNullOrEmpty(searchString))
            {
                managers = managers.Where(u => u.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    managers = managers.OrderByDescending(u => u.UserName).ToList();
                    break;
                default:
                    managers = managers.OrderBy(u => u.UserName).ToList();
                    break;
            }

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(managers.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Route("RestoreManager/{id}")]
        public ActionResult RestoreManager(string id)
        {
            storeAdmin.RestoreManager(id);

            return RedirectToAction("GetAllDeletedManagers");
        }

        [HttpGet]
        [Route("GetAllManagerStores/{id}")]
        public ActionResult GetAllManagerStores(string id, string sortOrder, string currentFilter, string searchString, int? page)
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

            //jos dodat error messages
            WebApiManager manager = storeAdmin.DetailsManager(id);

            if (manager != null)
            {
                Response.Cookies.Add(new HttpCookie("ManagerID")
                {
                    Value = manager.Id,
                    HttpOnly = true
                });
                Response.Cookies.Add(new HttpCookie("ManagerUserName")
                {
                    Value = manager.UserName,
                    HttpOnly = true
                });
            }

            List<WebApiManagerStore> stores = storeAdmin.GetAllManagerStores(id);

            if (!string.IsNullOrEmpty(searchString))
            {
                stores = stores.Where(u => u.Store.StoreName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    stores = stores.OrderByDescending(u => u.Store.StoreName).ToList();
                    break;
                default:
                    stores = stores.OrderBy(u => u.Store.StoreName).ToList();
                    break;
            }

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(stores.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Route("AssignStore/{managerId}/{storeId}")]
        public ActionResult AssignStore(string managerId, string storeId)
        {
            Response assignStoreResponse = storeAdmin.AssignStore(new WebApiStoreAssign { ManagerId = managerId, StoreId = storeId });

            TempData["Message"] = assignStoreResponse.ResponseMessage;
            TempData["Success"] = assignStoreResponse.Success;

            return RedirectToAction("GetAllManagerStores", new { id = managerId });
        }

        [HttpGet]
        [Route("UnassignStore/{managerId}/{storeId}")]
        public ActionResult UnassignStore(string managerId, string storeId)
        {
            Response storeUnassignResponse = storeAdmin.UnassignStore(new WebApiStoreAssign { ManagerId = managerId, StoreId = storeId });

            TempData["Message"] = storeUnassignResponse.ResponseMessage;
            TempData["Success"] = storeUnassignResponse.Success;

            return RedirectToAction("GetAllManagerStores", new { id = managerId });
        }
    }
}