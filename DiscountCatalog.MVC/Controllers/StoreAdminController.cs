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

namespace DiscountCatalog.MVC.Controllers
{
    [RoutePrefix("StoreAdmin")]
    public class StoreAdminController : Controller
    {
        private StoreAdminRepository storeAdminRepository = new StoreAdminRepository();

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

                return View(manager);
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

        //TU SAM STAO!!!!!!!!!!

        #endregion

        //        //[HttpGet]
        //        [Route("GetAllStores")]
        //        public async Task<ActionResult> GetAllStores(string sortOrder, string currentFilter, string searchString, int? page)
        //        {
        //            List<WebApiStore> stores = new List<WebApiStore>();

        //            ViewBag.CurrentSort = sortOrder;
        //            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

        //            if (searchString != null)
        //            {
        //                page = 1;
        //            }
        //            else
        //            {
        //                searchString = currentFilter;
        //            }

        //            ViewBag.CurrentFilter = searchString;

        //            WebApiListOfStoresResult result = await storeAdminRepository.GetAllStoresAsync();

        //            if (result.Success)
        //            {
        //                stores = result.Stores;
        //            }
        //            else
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(searchString))
        //            {
        //                stores = stores.Where(u => u.StoreName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
        //            }

        //            switch (sortOrder)
        //            {
        //                case "name_desc":
        //                    stores = stores.OrderByDescending(u => u.StoreName).ToList();
        //                    break;
        //                default:
        //                    stores = stores.OrderBy(u => u.StoreName).ToList();
        //                    break;
        //            }

        //            int pageSize = 12;
        //            int pageNumber = (page ?? 1);

        //            return View(stores.ToPagedList(pageNumber, pageSize));
        //        }

        //        [HttpGet]
        //        [Route("CreateStore")]
        //        public ActionResult CreateStore()
        //        {
        //            return View();
        //        }

        //        [HttpPost]
        //        [Route("CreateStore")]
        //        public async Task<ActionResult> CreateStore(WebApiStore store)
        //        {
        //            WebApiResult result = await storeAdminRepository.CreateStoreAsync(store);

        //            if (!result.Success)
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }

        //                return View(store);
        //            }

        //            return RedirectToAction("GetAllStores");
        //        }

        //        [HttpGet]
        //        [Route("EditStore/{id}")]
        //        public async Task<ActionResult> EditStore(string id)
        //        {
        //            WebApiStoreResult result = await storeAdminRepository.EditStoreAsync(id);

        //            if (result.Success)
        //            {
        //                WebApiStore store = result.Store;

        //                return View(store);
        //            }
        //            else
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }

        //                return RedirectToAction("GetAllStores");
        //            }
        //        }

        //        [HttpPost]
        //        [Route("EditStore")]
        //        public async Task<ActionResult> EditStore(WebApiStore store)
        //        {
        //            WebApiResult result = await storeAdminRepository.EditStoreAsync(store);

        //            if (result.Success)
        //            {
        //                return RedirectToAction("GetAllStores");
        //            }
        //            else
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }

        //                return View(store);
        //            }
        //        }

        //        [Route("PostStoreImage")]
        //        public async Task PostStoreImage(PostImage image)
        //        {
        //            WebApiResult result = new WebApiResult();

        //            if (image.File != null)
        //            {
        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    image.File.InputStream.CopyTo(ms);
        //                    byte[] array = ms.GetBuffer();

        //                    float mb = (array.Length / 1024f) / 1024f;

        //                    if (mb < 1)
        //                    {
        //                        WebApiPostImage webApiImage = new WebApiPostImage
        //                        {
        //                            Id = image.Id,
        //                            Image = array
        //                        };

        //                        result = await storeAdminRepository.PostStoreImageAsync(webApiImage);
        //                    }
        //                    else
        //                    {
        //                        byte[] arrayScaled = ImageProcessor.To1MB(array);
        //                        float mbScaled = (arrayScaled.Length / 1024f) / 1024f;

        //                        if (arrayScaled != null)
        //                        {
        //                            WebApiPostImage webApiImage = new WebApiPostImage
        //                            {
        //                                Id = image.Id,
        //                                Image = arrayScaled
        //                            };

        //                            result = await storeAdminRepository.PostStoreImageAsync(webApiImage);
        //                        }
        //                    }
        //                }
        //            }

        //            if (!result.Success)
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }
        //            }
        //        }

        //        [Route("GetStoreImage/{id}")]
        //        public async Task<ActionResult> GetStoreImage(string id)
        //        {
        //            byte[] byteArray = await storeAdminRepository.GetStoreImageAsync(id);

        //            return File(byteArray, "image/png");
        //        }

        //        [HttpGet]
        //        [Route("DetailsStore/{id}")]
        //        public async Task<ActionResult> DetailsStore(string id)
        //        {
        //            WebApiStoreResult result = await storeAdminRepository.DetailsStoreAsync(id);

        //            if (result.Success)
        //            {
        //                return View(result.Store);
        //            }
        //            else
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }

        //                return RedirectToAction("GetAllStores");
        //            }
        //        }

        //        [HttpGet]
        //        [Route("DeleteStore/{id}")]
        //        public async Task<ActionResult> DeleteStore(string id)
        //        {
        //            WebApiStoreResult result = await storeAdminRepository.DetailsStoreAsync(id);

        //            if (result.Success)
        //            {
        //                return View(result.Store);
        //            }
        //            else
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }

        //                return RedirectToAction("GetAllStores");
        //            }
        //        }

        //        [HttpPost]
        //        [Route("DeleteStore")]
        //        public async Task<ActionResult> DeleteStore(WebApiStore store)
        //        {
        //            WebApiResult result = await storeAdminRepository.DeleteStoreAsync(store.Id);

        //            foreach (var error in result.ModelState)
        //            {
        //                ModelState.AddModelError(error.Key, error.Value);
        //            }

        //            return RedirectToAction("GetAllStores");
        //        }

        //        [HttpGet]
        //        [Route("GetAllDeletedStores")]
        //        public async Task<ActionResult> GetAllDeletedStores(string sortOrder, string currentFilter, string searchString, int? page)
        //        {
        //            List<WebApiStore> stores = new List<WebApiStore>();

        //            ViewBag.CurrentSort = sortOrder;
        //            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

        //            if (searchString != null)
        //            {
        //                page = 1;
        //            }
        //            else
        //            {
        //                searchString = currentFilter;
        //            }

        //            ViewBag.CurrentFilter = searchString;

        //            WebApiListOfStoresResult result = await storeAdminRepository.GetAllDeletedStoresAsync();

        //            if (result.Success)
        //            {
        //                stores = result.Stores;
        //            }
        //            else
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(searchString))
        //            {
        //                stores = stores.Where(u => u.StoreName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
        //            }

        //            switch (sortOrder)
        //            {
        //                case "name_desc":
        //                    stores = stores.OrderByDescending(u => u.StoreName).ToList();
        //                    break;
        //                default:
        //                    stores = stores.OrderBy(u => u.StoreName).ToList();
        //                    break;
        //            }

        //            int pageSize = 12;
        //            int pageNumber = (page ?? 1);

        //            return View(stores.ToPagedList(pageNumber, pageSize));
        //        }

        //        [HttpGet]
        //        [Route("RestoreStore/{id}")]
        //        public async Task<ActionResult> RestoreStore(string id)
        //        {
        //            WebApiResult result = await storeAdminRepository.RestoreStoreAsync(id);

        //            foreach (var error in result.ModelState)
        //            {
        //                ModelState.AddModelError(error.Key, error.Value);
        //            }

        //            return RedirectToAction("GetAllDeletedStores");
        //        }

        //        [HttpGet]
        //        [Route("Select/{id}")]
        //        public async Task<ActionResult> Select(string id)
        //        {
        //            WebApiSelectedStoreResult result = await storeAdminRepository.SelectAsync(id);

        //            SelectedStore store = result.Store;

        //            if (result.Success)
        //            {
        //                if (store != null)
        //                {
        //                    Response.Cookies.Add(new HttpCookie("StoreID")
        //                    {
        //                        Value = store.Id,
        //                        HttpOnly = true
        //                    });
        //                    Response.Cookies.Add(new HttpCookie("StoreName")
        //                    {
        //                        Value = store.StoreName,
        //                        HttpOnly = true
        //                    });

        //                    return RedirectToAction("Index", "Store");
        //                }
        //            }

        //            foreach (var error in result.ModelState)
        //            {
        //                ModelState.AddModelError(error.Key, error.Value);
        //            }

        //            return RedirectToAction("GetAllStores");

        //        }

        //        //[HttpGet]
        //        [Route("GetAllManagers")]
        //        public async Task<ActionResult> GetAllManagers(string sortOrder, string currentFilter, string searchString, int? page)
        //        {
        //            List<WebApiManager> managers = new List<WebApiManager>();

        //            ViewBag.CurrentSort = sortOrder;
        //            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

        //            if (searchString != null)
        //            {
        //                page = 1;
        //            }
        //            else
        //            {
        //                searchString = currentFilter;
        //            }

        //            ViewBag.CurrentFilter = searchString;

        //            WebApiListOfManagersResult result = await storeAdminRepository.GetAllManagersAsync();

        //            if (result.Success)
        //            {
        //                managers = result.Managers;
        //            }
        //            else
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(searchString))
        //            {
        //                managers = managers.Where(u => u.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
        //            }

        //            switch (sortOrder)
        //            {
        //                case "name_desc":
        //                    managers = managers.OrderByDescending(u => u.UserName).ToList();
        //                    break;
        //                default:
        //                    managers = managers.OrderBy(u => u.UserName).ToList();
        //                    break;
        //            }

        //            int pageSize = 12;
        //            int pageNumber = (page ?? 1);

        //            return View(managers.ToPagedList(pageNumber, pageSize));
        //        }

        //        [HttpGet]
        //        [Route("CreateManager")]
        //        public ActionResult CreateManager()
        //        {
        //            return View();
        //        }

        //        [HttpPost]
        //        [Route("CreateManager")]
        //        public async Task<ActionResult> CreateManager(CreateManagerModel manager)
        //        {
        //            WebApiResult result = await storeAdminRepository.CreateManagerAsync(manager);

        //            if (!result.Success)
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }

        //                return View(manager);
        //            }

        //            return RedirectToAction("GetAllManagers");
        //        }

        //        [HttpGet]
        //        [Route("DetailsManager/{id}")]
        //        public async Task<ActionResult> DetailsManager(string id)
        //        {
        //            WebApiManagerResult result = await storeAdminRepository.DetailsManagerAsync(id);

        //            if (result.Success)
        //            {
        //                WebApiManager manager = result.Manager;

        //                return View(manager);
        //            }
        //            else
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }

        //                return RedirectToAction("GetAllManagers");
        //            }
        //        }

        //        [HttpGet]
        //        [Route("EditManager/{id}")]
        //        public async Task<ActionResult> EditManager(string id)
        //        {
        //            WebApiManagerResult result = await storeAdminRepository.EditManagerAsync(id);

        //            if (result.Success)
        //            {
        //                WebApiManager manager = result.Manager;

        //                return View(manager);
        //            }
        //            else
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }

        //                return RedirectToAction("GetAllManagers");
        //            }
        //        }

        //        [HttpPost]
        //        [Route("EditManager")]
        //        public async Task<ActionResult> EditManager(WebApiManager manager)
        //        {
        //            WebApiResult result = await storeAdminRepository.EditManagerAsync(manager);

        //            if (!result.Success)
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }

        //                return View(manager);
        //            }

        //            return RedirectToAction("GetAllManagers");
        //        }

        //        [Route("PostManagerImage")]
        //        public async Task PostManagerImage(PostImage image)
        //        {
        //            WebApiResult result = new WebApiResult();

        //            if (image.File != null)
        //            {
        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    image.File.InputStream.CopyTo(ms);
        //                    byte[] array = ms.GetBuffer();

        //                    float mb = (array.Length / 1024f) / 1024f;

        //                    if (mb < 1)
        //                    {
        //                        WebApiPostImage webApiImage = new WebApiPostImage
        //                        {
        //                            Id = image.Id,
        //                            Image = array
        //                        };

        //                        result = await storeAdminRepository.PostManagerImageAsync(webApiImage);
        //                    }
        //                    else
        //                    {
        //                        byte[] arrayScaled = ImageProcessor.To1MB(array);
        //                        float mbScaled = (arrayScaled.Length / 1024f) / 1024f;

        //                        if (arrayScaled != null)
        //                        {
        //                            WebApiPostImage webApiImage = new WebApiPostImage
        //                            {
        //                                Id = image.Id,
        //                                Image = arrayScaled
        //                            };

        //                            result = await storeAdminRepository.PostManagerImageAsync(webApiImage);
        //                        }
        //                    }
        //                }
        //            }

        //            if (!result.Success)
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }
        //            }
        //        }

        //        [Route("GetManagerImage/{id}")]
        //        public async Task<ActionResult> GetManagerImage(string id)
        //        {
        //            byte[] byteArray = await storeAdminRepository.GetManagerImageAsync(id);

        //            return File(byteArray, "image/png");
        //        }

        //        [HttpGet]
        //        [Route("DeleteManager/{id}")]
        //        public async Task<ActionResult> DeleteManager(string id)
        //        {
        //            WebApiManagerResult result = await storeAdminRepository.DetailsManagerAsync(id);

        //            if (result.Success)
        //            {
        //                WebApiManager manager = result.Manager;

        //                return View(manager);
        //            }
        //            else
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }

        //                return RedirectToAction("GetAllManagers");
        //            }
        //        }

        //        [HttpPost]
        //        [Route("DeleteManager")]
        //        public async Task<ActionResult> DeleteManager(WebApiManager manager)
        //        {
        //            WebApiResult result = await storeAdminRepository.DeleteManagerAsync(manager.Id);

        //            if (!result.Success)
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }
        //            }

        //            return RedirectToAction("GetAllManagers");
        //        }

        //        //[HttpGet]
        //        [Route("GetAllDeletedManager")]
        //        public async Task<ActionResult> GetAllDeletedManagers(string sortOrder, string currentFilter, string searchString, int? page)
        //        {
        //            List<WebApiManager> managers = new List<WebApiManager>();

        //            ViewBag.CurrentSort = sortOrder;
        //            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

        //            if (searchString != null)
        //            {
        //                page = 1;
        //            }
        //            else
        //            {
        //                searchString = currentFilter;
        //            }

        //            ViewBag.CurrentFilter = searchString;

        //            WebApiListOfManagersResult result = await storeAdminRepository.GetAllDeletedManagers();

        //            if (result.Success)
        //            {
        //                managers = result.Managers;
        //            }
        //            else
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(searchString))
        //            {
        //                managers = managers.Where(u => u.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
        //            }

        //            switch (sortOrder)
        //            {
        //                case "name_desc":
        //                    managers = managers.OrderByDescending(u => u.UserName).ToList();
        //                    break;
        //                default:
        //                    managers = managers.OrderBy(u => u.UserName).ToList();
        //                    break;
        //            }

        //            int pageSize = 12;
        //            int pageNumber = (page ?? 1);

        //            return View(managers.ToPagedList(pageNumber, pageSize));
        //        }

        //        [HttpGet]
        //        [Route("RestoreManager/{id}")]
        //        public async Task<ActionResult> RestoreManager(string id)
        //        {
        //            WebApiResult result = await storeAdminRepository.RestoreManagerAsync(id);

        //            if (!result.Success)
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }
        //            }

        //            return RedirectToAction("GetAllDeletedManagers");
        //        }

        //        [HttpGet]
        //        [Route("GetAllManagerStores/{id}")]
        //        public async Task<ActionResult> GetAllManagerStores(string id, string sortOrder, string currentFilter, string searchString, int? page)
        //        {
        //            List<WebApiManagerStore> stores = new List<WebApiManagerStore>();

        //            ViewBag.CurrentSort = sortOrder;
        //            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

        //            if (searchString != null)
        //            {
        //                page = 1;
        //            }
        //            else
        //            {
        //                searchString = currentFilter;
        //            }

        //            ViewBag.CurrentFilter = searchString;

        //            WebApiManagerResult managerResult = await storeAdminRepository.DetailsManagerAsync(id);

        //            if (managerResult.Success)
        //            {
        //                WebApiManager manager = managerResult.Manager;

        //                if (manager != null)
        //                {
        //                    Response.Cookies.Add(new HttpCookie("ManagerID")
        //                    {
        //                        Value = manager.Id,
        //                        HttpOnly = true
        //                    });
        //                    Response.Cookies.Add(new HttpCookie("ManagerUserName")
        //                    {
        //                        Value = manager.UserName,
        //                        HttpOnly = true
        //                    });

        //                    ViewBag.ManagerUserName = manager.UserName;
        //                }
        //            }
        //            else
        //            {
        //                foreach (var error in managerResult.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }

        //                return RedirectToAction("GetAllManagers");
        //            }

        //            WebApiListOfManagerStoresResult storeResult = await storeAdminRepository.GetAllManagerStoresAsync(id);

        //            if (storeResult.Success)
        //            {
        //                stores = storeResult.Stores;
        //            }
        //            else
        //            {
        //                foreach (var error in storeResult.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(searchString))
        //            {
        //                stores = stores.Where(u => u.Store.StoreName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
        //            }

        //            switch (sortOrder)
        //            {
        //                case "name_desc":
        //                    stores = stores.OrderByDescending(u => u.Store.StoreName).ToList();
        //                    break;
        //                default:
        //                    stores = stores.OrderBy(u => u.Store.StoreName).ToList();
        //                    break;
        //            }

        //            int pageSize = 12;
        //            int pageNumber = (page ?? 1);

        //            return View(stores.ToPagedList(pageNumber, pageSize));
        //        }

        //        [HttpGet]
        //        [Route("AssignStore/{managerId}/{storeId}")]
        //        public async Task<ActionResult> AssignStore(string managerId, string storeId)
        //        {
        //            WebApiResult result = await storeAdminRepository.AssignStoreAsync(new WebApiStoreAssign { ManagerId = managerId, StoreId = storeId });

        //            if (!result.Success)
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }
        //            }

        //            return RedirectToAction("GetAllManagerStores", new { id = managerId });
        //        }

        //        [HttpGet]
        //        [Route("UnassignStore/{managerId}/{storeId}")]
        //        public async Task<ActionResult> UnassignStore(string managerId, string storeId)
        //        {
        //            WebApiResult result = await storeAdminRepository.UnassignStoreAsync(new WebApiStoreAssign { ManagerId = managerId, StoreId = storeId });

        //            if (!result.Success)
        //            {
        //                foreach (var error in result.ModelState)
        //                {
        //                    ModelState.AddModelError(error.Key, error.Value);
        //                }
        //            }

        //            return RedirectToAction("GetAllManagerStores", new { id = managerId });
        //        }
    }
}