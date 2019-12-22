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

namespace AbatementHelper.MVC.Controllers
{
    public class ManagerController : Controller
    {
        private ManagerRepository manager = new ManagerRepository();

        public ActionResult Index()
        {
            return View();
        }

        //GetAllStores
        [Route("GetAllStores")]
        public ActionResult GetAllStores(string sortOrder, string currentFilter, string searchString, int? page)
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

            List<WebApiStore> stores = manager.GetAllStores();

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

        //Select

        [HttpGet]
        [Route("Select/{id}")]
        public ActionResult Select(string id)
        {
            if (id != null)
            {
                var store = manager.Select(id);

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
        public ActionResult EditStore(string id)
        {
            WebApiStore store = manager.EditStore(id);

            return View(store);
        }

        [HttpPost]
        [Route("EditStore")]
        public ActionResult EditStore(WebApiStore store)
        {
            Response editStoreResponse = manager.EditStore(store);

            TempData["Message"] = editStoreResponse.ResponseMessage;
            TempData["Success"] = editStoreResponse.Success;

            return RedirectToAction("GetAllStores");
        }

        [HttpGet]
        [Route("DetailsStore/{id}")]
        public ActionResult DetailsStore(string id)
        {
            WebApiStore store = manager.DetailsStore(id);

            return View(store);
        }

        //AbandonStore

        [HttpGet]
        [Route("AbandonStore/{id}")]
        public ActionResult AbandonStore(string id)
        {
            WebApiStore store = manager.DetailsStore(id);

            return View(store);
        }

        [HttpPost]
        [Route("AbandonStore/{id}")]
        public ActionResult AbandonStore(WebApiStore store)
        {
            Response storeUnassignResponse = manager.AbandonStore(store.Id);

            TempData["Message"] = storeUnassignResponse.ResponseMessage;
            TempData["Success"] = storeUnassignResponse.Success;

            return RedirectToAction("GetAllStores");
        }
    }
}