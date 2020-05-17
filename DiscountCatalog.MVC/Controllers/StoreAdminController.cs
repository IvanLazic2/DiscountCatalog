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
using DiscountCatalog.MVC.Cookies.Contractor;
using DiscountCatalog.MVC.Cookies.Implementation;
using DiscountCatalog.MVC.Validators;
using DiscountCatalog.MVC.Models.ManyToManyModels;
using DiscountCatalog.MVC.ViewModels.ManyToManyViewModels;
using DiscountCatalog.MVC.Models.ManyToManyModels.Manager;
using DiscountCatalog.MVC.Models.ManyToManyModels.Store;

namespace DiscountCatalog.MVC.Controllers
{
    [RoutePrefix("StoreAdmin")]
    public class StoreAdminController : Controller
    {
        #region Fields

        private readonly IMapper mapper;
        private readonly ICookieHandler cookieHandler;
        //private readonly AccountCookieHandler accountCookieHandler;
        private readonly StoreCookieHandler storeCookieHandler;
        private readonly StoreAdminRepository storeAdminRepository;

        #endregion

        #region Constructors

        public StoreAdminController()
        {
            mapper = AutoMapping.Initialise();
            cookieHandler = new CookieHandler();
            //accountCookieHandler = new AccountCookieHandler();
            storeCookieHandler = new StoreCookieHandler();
            storeAdminRepository = new StoreAdminRepository();
            
        }

        #endregion

        #region Methods



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
            manager.Identity.UserImage = ImageProcessor.ToValidByteArray(HttpContext.Request.Files[0]);

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
                return RedirectToAction("GetAllManagers").Success(result.SuccessMessage);
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
        public async Task<ActionResult> ManagerDetails(string id, string sortOrder, string currentFilter, string searchString, int? page)
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

            ManagerREST manager = await storeAdminRepository.GetManager(id);

            if (GlobalValidator.IsManagerValid(manager))
            {
                manager.Stores = ManagerProcessor.SearchStores(manager, searchString);
                manager.Stores = ManagerProcessor.OrderStores(manager, sortOrder);

                manager.Stores = manager.Stores.ToPagedList(pageIndex, pageSize);

                return View(manager);
            }

            return RedirectToAction("GetAllManagers").Error("Something went wrong, please try again.");
        }

        [HttpGet]
        [Route("EditManager")]
        public async Task<ActionResult> EditManager(string id)
        {
            ManagerREST manager = await storeAdminRepository.GetManager(id);

            if (GlobalValidator.IsManagerValid(manager))
            {
                return View(manager);
            }
            else
            {
                return RedirectToAction("ManagerDetails").Error("Something went wrong, please try again.");
            }
        }

        [HttpPost]
        [Route("EditManager")]
        public async Task<ActionResult> EditManager(ManagerRESTPut manager)
        {
            manager.Identity.UserImage = ImageProcessor.ToValidByteArray(HttpContext.Request.Files[0]);

            Result result = await storeAdminRepository.EditManager(manager);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(mapper.Map<ManagerREST>(manager));
            }

            return RedirectToAction("ManagerDetails", new { id = manager.Id }).Success(result.SuccessMessage);
        }

        [HttpGet]
        [Route("DeleteManager/{id}")]
        public async Task<ActionResult> DeleteManager(string id)
        {
            ManagerREST manager = await storeAdminRepository.GetManager(id);

            if (GlobalValidator.IsManagerValid(manager))
            {
                return View(manager);
            }

            return RedirectToAction("ManagerDetails").Error("Something went wrong, please try again.");
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

            return RedirectToAction("GetAllManagers").Warning(result.SuccessMessage + ", you can restore it <a class='btn btn-warning btn-sm' href='/StoreAdmin/GetAllDeletedManagers')'> HERE</a>");
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

            return RedirectToAction("GetAllDeletedManagers").Success(result.SuccessMessage);
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

            if (!string.IsNullOrEmpty(id))
            {
                ManagerStores managerStores = await storeAdminRepository.GetManagerStores(id, sortOrder, searchString, pageIndex, pageSize);

                if (managerStores != null)
                {
                    if (GlobalValidator.IsManagerValid(managerStores.Manager))
                    {
                        StaticPagedList<ManagerStore> list = new StaticPagedList<ManagerStore>(managerStores.Stores.Items, managerStores.Stores.MetaData.PageNumber, managerStores.Stores.MetaData.PageSize, managerStores.Stores.MetaData.TotalItemCount);

                        ManagerStoresViewModel viewModel = new ManagerStoresViewModel
                        {
                            Manager = managerStores.Manager,
                            Stores = list
                        };

                        return View(viewModel);
                    }

                }
            }

            return RedirectToAction("ManagerDetails", new { id }).Error("Something went wrong, please try again.");
        }

        [HttpGet]
        [Route("AssignStore/{managerId}/{storeId}")]
        public async Task<ActionResult> AssignStore(string managerId, string storeId)
        {
            Result result = await storeAdminRepository.AssignStore(managerId, storeId);

            if (!result.Success)
            {
                return RedirectToAction("GetManagerStores", new { id = managerId }).Error("Something went wrong, please try again.");
            }

            return RedirectToAction("GetManagerStores", new { id = managerId }).Success(result.SuccessMessage);
        }

        [HttpGet]
        [Route("UnassignStore/{managerId}/{storeId}")]
        public async Task<ActionResult> UnassignStore(string managerId, string storeId)
        {
            Result result = await storeAdminRepository.UnassignStore(managerId, storeId);

            if (!result.Success)
            {
                return RedirectToAction("GetManagerStores", new { id = managerId }).Error("Something went wrong, please try again.");
            }

            return RedirectToAction("GetManagerStores", new { id = managerId }).Success(result.SuccessMessage);
        }

        [Route("PostManagerImage/{id}")]
        public async Task<ActionResult> PostManagerImage(string id, HttpPostedFileBase file)
        {
            byte[] image = ImageProcessor.ToValidByteArray(file);

            Result result = await storeAdminRepository.PostManagerImage(id, image);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return RedirectToAction("ManagerDetails", new { id }).Success(result.SuccessMessage);
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
            store.StoreImage = ImageProcessor.ToValidByteArray(HttpContext.Request.Files[0]);

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
                return RedirectToAction("GetAllStores").Success(result.SuccessMessage);
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
        public async Task<ActionResult> StoreDetails(string id, string sortOrder, string currentFilter, string searchString, int? page)
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

            StoreREST store = await storeAdminRepository.GetStore(id);

            if (GlobalValidator.IsStoreValid(store))
            {
                store.Managers = StoreProcessor.SearchManagers(store, searchString);
                store.Managers = StoreProcessor.OrderManagers(store, sortOrder);

                store.Managers = store.Managers.ToPagedList(pageIndex, pageSize);

                return View(store);
            }

            return RedirectToAction("GetAllStores").Error("Something went wrong, please try again.");
        }

        [HttpGet]
        [Route("EditStore")]
        public async Task<ActionResult> EditStore(string id)
        {
            StoreREST store = await storeAdminRepository.GetStore(id);

            if (GlobalValidator.IsStoreValid(store))
            {
                return View(store);
            }

            return RedirectToAction("StoreDetails").Error("Something went wrong, please try again.");
        }

        [HttpPost]
        [Route("EditStore")]
        public async Task<ActionResult> EditStore(StoreRESTPut store)
        {
            store.StoreImage = ImageProcessor.ToValidByteArray(HttpContext.Request.Files[0]);

            Result result = await storeAdminRepository.EditStore(store);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(mapper.Map<StoreREST>(store));
            }

            return RedirectToAction("StoreDetails", new { id = store.Id }).Success(result.SuccessMessage);
        }

        [HttpGet]
        [Route("DeleteStore/{id}")]
        public async Task<ActionResult> DeleteStore(string id)
        {
            StoreREST store = await storeAdminRepository.GetStore(id);

            if (GlobalValidator.IsStoreValid(store))
            {
                return View(store);
            }

            return RedirectToAction("StoreDetails").Error("Something went wrong, please try again.");
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

            return RedirectToAction("GetAllStores").Warning(result.SuccessMessage);
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

            return RedirectToAction("GetAllDeletedStores").Success(result.SuccessMessage);
        }

        [Route("PostStoreImage/{id}")]
        public async Task<ActionResult> PostStoreImage(string id, HttpPostedFileBase file)
        {
            byte[] image = ImageProcessor.ToValidByteArray(file);

            Result result = await storeAdminRepository.PostStoreImage(id, image);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            return RedirectToAction("StoreDetails", new { id }).Success(result.SuccessMessage);
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
                cookieHandler.Set("StoreID", store.Id, true, System.Web.HttpContext.Current);
                this.HttpContext.Session.Add("StoreID", store.Id);
                cookieHandler.Set("StoreName", store.StoreName, true, System.Web.HttpContext.Current);

                return RedirectToAction("Index", "Store").Success($"{store.StoreName} selected.");
            }

            return RedirectToAction("GetAllStores").Error("Something went wrong, please try again.");

        }

        [HttpGet]
        [Route("GetStoreManagers/{id}")]
        public async Task<ActionResult> GetStoreManagers(string id, string sortOrder, string currentFilter, string searchString, int? page)
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

            if (!string.IsNullOrEmpty(id))
            {
                StoreManagers storeManagers = await storeAdminRepository.GetStoreManagers(id, sortOrder, searchString, pageIndex, pageSize);

                if (storeManagers != null)
                {
                    if (GlobalValidator.IsStoreValid(storeManagers.Store))
                    {
                        StaticPagedList<StoreManager> list = new StaticPagedList<StoreManager>(storeManagers.Managers.Items, storeManagers.Managers.MetaData.PageNumber, storeManagers.Managers.MetaData.PageSize, storeManagers.Managers.MetaData.TotalItemCount);

                        StoreManagersViewModel viewModel = new StoreManagersViewModel
                        {
                            Store = storeManagers.Store,
                            Managers = list
                        };

                        return View(viewModel);
                    }
                }
            }

            return RedirectToAction("StoreDetails", new { id }).Error("Something went wrong, please try again.");
        }

        [HttpGet]
        [Route("AssignManager/{storeId}/{managerId}")]
        public async Task<ActionResult> AssignManager(string storeId, string managerId)
        {
            Result result = await storeAdminRepository.AssignManager(storeId, managerId);

            if (!result.Success)
            {
                return RedirectToAction("GetStoreManagers", new { id = storeId }).Error("Something went wrong, please try again.");
            }

            return RedirectToAction("GetStoreManagers", new { id = storeId }).Success(result.SuccessMessage);
        }

        [HttpGet]
        [Route("UnassignManager/{storeId}/{managerId}")]
        public async Task<ActionResult> UnassignManager(string storeId, string managerId)
        {
            Result result = await storeAdminRepository.UnassignManager(storeId, managerId);

            if (!result.Success)
            {
                return RedirectToAction("GetStoreManagers", new { id = storeId }).Error("Something went wrong, please try again.");
            }

            return RedirectToAction("GetStoreManagers", new { id = storeId }).Success(result.SuccessMessage);
        }

        #endregion


        #endregion


    }
}