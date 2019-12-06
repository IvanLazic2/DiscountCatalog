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

        [HttpGet]
        [Route("GetAllStores")]
        public async Task<ActionResult> GetAllStores()
        {
            List<WebApiStore> stores;

            stores = await storeAdmin.GetAllStores();

            if (TempData["Message"] != null && TempData["Success"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                ViewBag.Success = (bool)TempData["Success"];
            }

            return View(stores);
        }

        [HttpGet]
        [Route("CreateStore")]
        public ActionResult CreateStore()
        {
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
        public async Task<ActionResult> GetAllDeletedStores()
        {
            List<WebApiStore> stores;

            stores = await storeAdmin.GetAllDeletedStores();

            return View(stores);
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
        public ActionResult Select()
        {
            //mozda u cookie spremit storeID
            return RedirectToAction("Index", "Store");
        }
    }
}