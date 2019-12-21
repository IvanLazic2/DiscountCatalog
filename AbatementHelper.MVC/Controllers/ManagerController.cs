using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult GetAllStores()
        {
            List<WebApiStore> stores = manager.GetAllStores();

            if (TempData["Message"] != null && TempData["Success"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                ViewBag.Success = (bool)TempData["Success"];
            }

            return View(stores);
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