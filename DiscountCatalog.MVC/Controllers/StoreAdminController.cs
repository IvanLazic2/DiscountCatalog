using DiscountCatalog.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.Common.CreateModels;
using PagedList;
using DiscountCatalog.MVC.Extensions;
using System.IO;
using DiscountCatalog.MVC.Processors;
using DiscountCatalog.MVC.Repositories;
using DiscountCatalog.MVC.ViewModels;
using DiscountCatalog.MVC.Models.Paging;
using DiscountCatalog.MVC.ViewModels.Manager;
using DiscountCatalog.MVC.REST.Manager;
using DiscountCatalog.Common.Models;
using DiscountCatalog.MVC.REST.Store;
using AutoMapper;
using AbatementHelper.MVC.Mapping;

namespace DiscountCatalog.MVC.Controllers
{
    [RoutePrefix("StoreAdmin")]
    public class StoreAdminController : Controller
    {
        private IMapper mapper;
        private StoreAdminRepository storeAdminRepository = new StoreAdminRepository();

        public StoreAdminController()
        {
            mapper = AutoMapping.Initialise();
        }

        [HttpGet]
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }

        #region Manager

        [HttpGet]
        [Route("CreateManager")]
        public ActionResult CreateManager()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateManager")]
        public async Task<ActionResult> CreateManager(ManagerRESTPost manager)
        {
            Result result = await storeAdminRepository.CreateManager(manager);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(manager);
            }
            else
            {
                return RedirectToAction("GetAllManagers");
            }
        }

        [HttpGet]
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

            int pageIndex = (page ?? 1);
            int pageSize = 4;

            PagingEntity<ManagerREST> managers = await storeAdminRepository.GetAllManagers(sortOrder, searchString, pageIndex, pageSize);

            StaticPagedList<ManagerREST> list = new StaticPagedList<ManagerREST>(managers.Items, managers.MetaData.PageNumber, managers.MetaData.PageSize, managers.MetaData.TotalItemCount);

            return View(list);
        }

        [HttpGet]
        [Route("GetAllDeletedManagers")]
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

            int pageIndex = (page ?? 1);
            int pageSize = 4;

            PagingEntity<ManagerREST> managers = await storeAdminRepository.GetAllDeletedManagers(sortOrder, searchString, pageIndex, pageSize);

            StaticPagedList<ManagerREST> list = new StaticPagedList<ManagerREST>(managers.Items, managers.MetaData.PageNumber, managers.MetaData.PageSize, managers.MetaData.TotalItemCount);

            return View(list);
        }

        [HttpGet]
        [Route("ManagerDetails/{id}")]
        public async Task<ActionResult> ManagerDetails(string id)
        {
            ManagerREST manager = await storeAdminRepository.GetManager(id);

            return View(manager);
        }

        [HttpGet]
        [Route("EditManager")]
        public async Task<ActionResult> EditManager(string id)
        {
            ManagerREST manager = await storeAdminRepository.GetManager(id);

            return View(manager);
        }

        [HttpPost]
        [Route("EditManager")]
        public async Task<ActionResult> EditManager(ManagerRESTPut manager)
        {
            Result result = await storeAdminRepository.EditManager(manager);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(mapper.Map<ManagerREST>(manager));
            }

            return RedirectToAction("ManagerDetails", new { id = manager.Id });
        }

        [HttpGet]
        [Route("DeleteManager/{id}")]
        public async Task<ActionResult> DeleteManager(string id)
        {
            ManagerREST manager = await storeAdminRepository.GetManager(id);

            return View(manager);
        }

        [HttpPost]
        [Route("DeleteManager")]
        public async Task<ActionResult> DeleteManager(ManagerRESTPut manager)
        {
            Result result = await storeAdminRepository.DeleteManager(manager.Id);

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

        [HttpGet]
        [Route("RestoreManager/{id}")]
        public async Task<ActionResult> RestoreManager(string id)
        {
            Result result = await storeAdminRepository.RestoreManager(id);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return RedirectToAction("GetAllDeletedManagers");
        }

        [HttpGet]
        [Route("GetManagerStores/{id}")]
        public async Task<ActionResult> GetManagerStores(string id, string sortOrder, string currentFilter, string searchString, int? page)
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

            int pageIndex = (page ?? 1);
            int pageSize = 4;

            PagingEntity<ManagerStore> managerStores = await storeAdminRepository.GetManagerStores(id, sortOrder, searchString, pageIndex, pageSize);

            StaticPagedList<ManagerStore> list = new StaticPagedList<ManagerStore>(managerStores.Items, managerStores.MetaData.PageNumber, managerStores.MetaData.PageSize, managerStores.MetaData.TotalItemCount);

            return View(list);
        }

        [HttpGet]
        [Route("Assign/{managerId}/{storeId}")]
        public async Task<ActionResult> Assign(string managerId, string storeId)
        {
            Result result = await storeAdminRepository.Assign(managerId, storeId);

            foreach (var error in result.ModelState)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }

            return RedirectToAction("GetManagerStores", new { id = managerId });
        }

        [HttpGet]
        [Route("Unassign/{managerId}/{storeId}")]
        public async Task<ActionResult> Unassign(string managerId, string storeId)
        {
            Result result = await storeAdminRepository.Unassign(managerId, storeId);

            foreach (var error in result.ModelState)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }

            return RedirectToAction("GetManagerStores", new { id = managerId });
        }

        [Route("PostManagerImage/{id}")]
        public async Task<ActionResult> PostManagerImage(string id, PostImage image)
        {
            byte[] array = ImageProcessor.GetBuffer(image.File);

            byte[] imageArray = array;

            float mb = (array.Length / 1024f) / 1024f;

            if (mb > 1)
            {
                byte[] arrayScaled = ImageProcessor.To1MB(array);

                imageArray = arrayScaled;
            }

            Result result = await storeAdminRepository.PostManagerImage(id, imageArray);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return RedirectToAction("ManagerDetails", new { id });
        }

        [Route("GetManagerImage/{id}")]
        public async Task<ActionResult> GetManagerImage(string id)
        {
            byte[] byteArray = await storeAdminRepository.GetManagerImage(id);

            return File(byteArray, "image/png");
        }


        #endregion

        #region Store

        [HttpGet]
        public ActionResult CreateStore()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateStore")]
        public async Task<ActionResult> CreateStore(StoreRESTPost store)
        {
            Result result = await storeAdminRepository.CreateStore(store);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(store);
            }
            else
            {
                return RedirectToAction("GetAllStores");
            }
        }

        [HttpGet]
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

            int pageIndex = (page ?? 1);
            int pageSize = 4;

            PagingEntity<StoreREST> stores = await storeAdminRepository.GetAllStores(sortOrder, searchString, pageIndex, pageSize);

            StaticPagedList<StoreREST> list = new StaticPagedList<StoreREST>(stores.Items, stores.MetaData.PageNumber, stores.MetaData.PageSize, stores.MetaData.TotalItemCount);

            return View(list);
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

            int pageIndex = (page ?? 1);
            int pageSize = 4;

            PagingEntity<StoreREST> stores = await storeAdminRepository.GetAllDeletedStores(sortOrder, searchString, pageIndex, pageSize);

            StaticPagedList<StoreREST> list = new StaticPagedList<StoreREST>(stores.Items, stores.MetaData.PageNumber, stores.MetaData.PageSize, stores.MetaData.TotalItemCount);

            return View(list);

        }

        [HttpGet]
        [Route("StoreDetails/{id}")]
        public async Task<ActionResult> StoreDetails(string id)
        {
            StoreREST store = await storeAdminRepository.GetStore(id);

            return View(store);
        }

        [HttpGet]
        [Route("EditStore")]
        public async Task<ActionResult> EditStore(string id)
        {
            StoreREST store = await storeAdminRepository.GetStore(id);

            return View(store);
        }

        [HttpPost]
        [Route("EditStore")]
        public async Task<ActionResult> EditStore(StoreRESTPut store)
        {
            Result result = await storeAdminRepository.EditStore(store);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(mapper.Map<StoreREST>(store));
            }

            return RedirectToAction("StoreDetails", new { id = store.Id });
        }

        [HttpGet]
        [Route("DeleteStore/{id}")]
        public async Task<ActionResult> DeleteStore(string id)
        {
            StoreREST store = await storeAdminRepository.GetStore(id);

            return View(store);
        }

        [HttpPost]
        [Route("DeleteStore")]
        public async Task<ActionResult> DeleteStore(StoreRESTPut store)
        {
            Result result = await storeAdminRepository.DeleteStore(store.Id);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(store);
            }

            return RedirectToAction("GetAllStores");
        }

        [HttpGet]
        [Route("RestoreStore/{id}")]
        public async Task<ActionResult> RestoreStore(string id)
        {
            Result result = await storeAdminRepository.RestoreStore(id);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return RedirectToAction("GetAllDeletedStores");
        }

        [Route("PostStoreImage/{id}")]
        public async Task<ActionResult> PostStoreImage(string id, PostImage image)
        {
            byte[] array = ImageProcessor.GetBuffer(image.File);

            byte[] imageArray = array;

            float mb = (array.Length / 1024f) / 1024f;

            if (mb > 1)
            {
                byte[] arrayScaled = ImageProcessor.To1MB(array);

                imageArray = arrayScaled;
            }

            Result result = await storeAdminRepository.PostStoreImage(id, imageArray);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return RedirectToAction("StoreDetails", new { id });
        }

        [Route("GetStoreImage/{id}")]
        public async Task<ActionResult> GetStoreImage(string id)
        {
            byte[] byteArray = await storeAdminRepository.GetStoreImage(id);

            return File(byteArray, "image/png");
        }

        [HttpGet]
        [Route("Select/{id}")]
        public async Task<ActionResult> Select(string id)
        {
            StoreREST store = await storeAdminRepository.GetStore(id);

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

            return RedirectToAction("GetAllStores");

        }

        #endregion

    }
}