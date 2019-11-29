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

namespace AbatementHelper.MVC.Controllers
{
    [RoutePrefix("StoreAdmin")]
    public class StoreAdminController : Controller
    {
        [HttpGet]
        public ActionResult GetAllStores()
        {
            string masterStoreID = System.Web.HttpContext.Current.Request.Cookies["UserID"].Value.ToString();

            List<DataBaseStore> stores;

            StoreAdminRepository storeAdmin = new StoreAdminRepository();

            ViewBag.Success = storeAdmin.GetAllStores(masterStoreID).Success;
            ViewBag.Message = storeAdmin.GetAllStores(masterStoreID).Message;

            stores = storeAdmin.GetAllStores(masterStoreID).Value;

            return View(stores);
        }

        [HttpGet]
        public ActionResult GetAllDeletedStores()
        {
            string masterStoreID = System.Web.HttpContext.Current.Request.Cookies["UserID"].Value.ToString();

            List<DataBaseStore> stores;

            StoreAdminRepository storeAdmin = new StoreAdminRepository();

            ViewBag.Success = storeAdmin.GetAllDeletedStores(masterStoreID).Success;
            ViewBag.Message = storeAdmin.GetAllDeletedStores(masterStoreID).Message;

            stores = storeAdmin.GetAllDeletedStores(masterStoreID).Value;

            return View(stores);
        }

        //Registration action
        [HttpGet]
        public ActionResult RegisterStore()
        {
            return View("~/Views/StoreAdmin/RegisterStore.cshtml");
        }
        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterStore([Bind(Exclude = "EmailConfirmed, ActivationCode")] Store store)
        {
            store.MasterStoreID = System.Web.HttpContext.Current.Request.Cookies["UserID"].Value.ToString();
            store.Email = System.Web.HttpContext.Current.Request.Cookies["Email"].Value.ToString();

            StoreAdminRepository register = new StoreAdminRepository();

            var result = await register.RegisterStore(store);

            if (register.RegisterSuccessful)
            {
                ViewBag.Message = "Registration Successful";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Register Unsuccessful";
                return View("~/Views/Shared/Error.cshtml", store); //treba mijenjat
            }
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            DataBaseStore store = new DataBaseStore();

            StoreAdminRepository storeAdmin = new StoreAdminRepository();

            store = storeAdmin.Edit(id);

            return View(store);
        }

        [HttpPost]
        public ActionResult Edit(DataBaseStore store)
        {
            StoreAdminRepository storeAdmin = new StoreAdminRepository();

            if (storeAdmin.Edit(store))
            {
                return RedirectToAction("GetAllStores");
            }
            return View(store);
        }

        public ActionResult DeleteStore(string id)
        {
            StoreAdminRepository storeAdmin = new StoreAdminRepository();

            storeAdmin.DeleteStore(id);

            return RedirectToAction("GetAllStores");
        }

        public ActionResult RestoreStore(string id)
        {
            StoreAdminRepository storeAdmin = new StoreAdminRepository();

            storeAdmin.RestoreStore(id);

            return RedirectToAction("GetAllStores");
        }
    }
}