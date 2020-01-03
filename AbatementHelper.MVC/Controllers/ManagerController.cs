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

namespace AbatementHelper.MVC.Controllers
{
    public class ManagerController : Controller
    {
        private ManagerRepository manager = new ManagerRepository();

        public ActionResult Index()
        {
            return View();
        }

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

            List<WebApiStore> stores = await manager.GetAllStoresAsync();

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
        [Route("Select/{id}")]
        public async Task<ActionResult> Select(string id)
        {
            if (id != null)
            {
                SelectedStore store = await manager.SelectAsync(id);

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

        [HttpGet]
        [Route("EditStore/{id}")]
        public async Task<ActionResult> EditStore(string id)
        {
            WebApiStore store = await manager.EditStoreAsync(id);

            return View(store);
        }

        [HttpPost]
        [Route("EditStore")]
        public async Task<ActionResult> EditStore(WebApiStore store)
        {
            Response editStoreResponse = await manager.EditStoreAsync(store);

            TempData["Message"] = editStoreResponse.Message;
            TempData["Success"] = editStoreResponse.Success;

            return RedirectToAction("GetAllStores");
        }

        [HttpGet]
        [Route("DetailsStore/{id}")]
        public async Task<ActionResult> DetailsStore(string id)
        {
            WebApiStore store = await manager.DetailsStoreAsync(id);

            return View(store);
        }

        [HttpGet]
        [Route("AbandonStore/{id}")]
        public async Task<ActionResult> AbandonStore(string id)
        {
            WebApiStore store = await manager.DetailsStoreAsync(id);

            return View(store);
        }

        [HttpPost]
        [Route("AbandonStore/{id}")]
        public async Task<ActionResult> AbandonStore(WebApiStore store)
        {
            Response storeUnassignResponse = await manager.AbandonStoreAsync(store.Id);

            TempData["Message"] = storeUnassignResponse.Message;
            TempData["Success"] = storeUnassignResponse.Success;

            return RedirectToAction("GetAllStores");
        }
    }
}