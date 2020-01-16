using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.MVC.Repositories;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbatementHelper.MVC.Extensions;
using System.Threading.Tasks;
using AbatementHelper.MVC.Models;

namespace AbatementHelper.MVC.Controllers
{
    public class ManagerController : Controller
    {
        private ManagerRepository managerRepository = new ManagerRepository();

        public ActionResult Index()
        {
            return View();
        }

        [Route("GetAllStores")]
        public async Task<ActionResult> GetAllStores(string sortOrder, string currentFilter, string searchString, int? page)
        {
            List<WebApiStore> stores = new List<WebApiStore>();

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

            WebApiListOfStoresResult result = await managerRepository.GetAllStoresAsync();

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

        [HttpGet]
        [Route("Select/{id}")]
        public async Task<ActionResult> Select(string id)
        {
            WebApiSelectedStoreResult result = await managerRepository.SelectAsync(id);

            SelectedStore store = result.Store;

            if (result.Success)
            {
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

            foreach (var error in result.ModelState)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }

            return RedirectToAction("GetAllStores");

        }

        [HttpGet]
        [Route("EditStore/{id}")]
        public async Task<ActionResult> EditStore(string id)
        {
            WebApiStoreResult result = await managerRepository.EditStoreAsync(id);

            if (result.Success)
            {
                WebApiStore store = result.Store;

                return View(store);
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

        [HttpPost]
        [Route("EditStore")]
        public async Task<ActionResult> EditStore(WebApiStore store)
        {
            WebApiResult result = await managerRepository.EditStoreAsync(store);

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
        [Route("DetailsStore/{id}")]
        public async Task<ActionResult> DetailsStore(string id)
        {
            WebApiStoreResult result = await managerRepository.DetailsStoreAsync(id);

            if (result.Success)
            {
                return View(result.Store);
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

        [HttpGet]
        [Route("AbandonStore/{id}")]
        public async Task<ActionResult> AbandonStore(string id)
        {
            WebApiStoreResult result = await managerRepository.DetailsStoreAsync(id);

            if (result.Success)
            {
                return View(result.Store);
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

        [HttpPost]
        [Route("UnassignStore/{id}")]
        public async Task<ActionResult> AbandonStorePost(string id)
        {
            WebApiResult result = await managerRepository.AbandonStoreAsync(new WebApiStoreAssign { StoreId = id });

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return RedirectToAction("");
        }
    }
}