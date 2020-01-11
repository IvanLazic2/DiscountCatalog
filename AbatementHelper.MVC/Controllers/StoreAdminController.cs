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
using System.IO;
using AbatementHelper.MVC.Processors;

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

            List<WebApiStore> stores = await storeAdmin.GetAllStoresAsync();

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
        public async Task<ActionResult> CreateStore(WebApiStore store)
        {
            ModelStateResponse response = await storeAdmin.CreateStoreAsync(store);

            if (!response.IsValid)
            {
                foreach (var error in response.ModelSate)
                {
                    ModelState.AddModelError(error.Key, error.Value.First());
                }

                return View(store);
            }

            return RedirectToAction("GetAllStores");
        }

        [HttpGet]
        [Route("EditStore/{id}")]
        public async Task<ActionResult> EditStore(string id)
        {
            WebApiStore store = await storeAdmin.EditStoreAsync(id);

            return View(store);
        }

        [HttpPost]
        [Route("EditStore")]
        public async Task<ActionResult> EditStore(WebApiStore store)
        {
            ModelStateResponse response = await storeAdmin.EditStoreAsync(store);

            if (!response.IsValid)
            {
                foreach (var error in response.ModelSate)
                {
                    ModelState.AddModelError(error.Key, error.Value.First());
                }

                return View(store);
            }

            return RedirectToAction("GetAllStores");
        }

        [Route("PostStoreImage")]
        public async Task<ActionResult> PostStoreImage(PostImage image)
        {
            if (image.File != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.File.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    float mb = (array.Length / 1024f) / 1024f;

                    if (mb < 1)
                    {
                        WebApiPostImage webApiImage = new WebApiPostImage
                        {
                            Id = image.Id,
                            Image = array
                        };
                        await storeAdmin.PostStoreImageAsync(webApiImage);
                    }
                    else
                    {
                        byte[] arrayScaled = ImageProcessor.To1MB(array);
                        float mbScaled = (arrayScaled.Length / 1024f) / 1024f;

                        if (arrayScaled != null)
                        {
                            WebApiPostImage webApiImage = new WebApiPostImage
                            {
                                Id = image.Id,
                                Image = arrayScaled
                            };
                            await storeAdmin.PostStoreImageAsync(webApiImage);
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [Route("GetStoreImage/{id}")]
        public async Task<ActionResult> GetStoreImage(string id)
        {
            byte[] byteArray = await storeAdmin.GetStoreImageAsync(id);

            return File(byteArray, "image/png");
        }

        [HttpGet]
        [Route("DetailsStore/{id}")]
        public async Task<ActionResult> DetailsStore(string id)
        {
            WebApiStore store = await storeAdmin.DetailsStoreAsync(id);

            return View(store);
        }

        [HttpGet]
        [Route("DeleteStore/{id}")]
        public async Task<ActionResult> DeleteStore(string id)
        {
            WebApiStore store = await storeAdmin.DetailsStoreAsync(id);

            return View(store);
        }

        [HttpPost]
        [Route("DeleteStore")]
        public async Task<ActionResult> DeleteStore(WebApiStore store)
        {
            await storeAdmin.DeleteStoreAsync(store.Id);

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

            stores = await storeAdmin.GetAllDeletedStoresAsync();

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
        public async Task<ActionResult> RestoreStore(string id)
        {
            await storeAdmin.RestoreStoreAsync(id);

            return RedirectToAction("GetAllDeletedStores");
        }

        [HttpGet]
        [Route("Select/{id}")]
        public async Task<ActionResult> Select(string id)
        {
            if (id != null)
            {
                var store = await storeAdmin.SelectAsync(id);

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

            List<WebApiManager> managers = await storeAdmin.GetAllManagersAsync();

            //if (TempData["Message"] != null && TempData["Success"] != null)
            //{
            //    ViewBag.Message = TempData["Message"].ToString();
            //    ViewBag.Success = (bool)TempData["Success"];
            //}

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
            //if (TempData["Message"] != null && TempData["Success"] != null)
            //{
            //    ViewBag.Message = TempData["Message"].ToString();
            //    ViewBag.Success = (bool)TempData["Success"];
            //}

            return View();
        }

        [HttpPost]
        [Route("CreateManager")]
        public async Task<ActionResult> CreateManager(CreateManagerModel manager)
        {
            ModelStateResponse response = await storeAdmin.CreateManagerAsync(manager);

            if (!response.IsValid)
            {
                foreach (var error in response.ModelSate)
                {
                    ModelState.AddModelError(error.Key, error.Value.First());
                }

                return View(manager);
            }

            return RedirectToAction("GetAllManagers");
        }

        [HttpGet]
        [Route("DetailsManager/{id}")]
        public async Task<ActionResult> DetailsManager(string id)
        {
            WebApiManager manager = await storeAdmin.DetailsManagerAsync(id);

            return View(manager);
        }

        [HttpGet]
        [Route("EditManager/{id}")]
        public async Task<ActionResult> EditManager(string id)
        {
            WebApiManager manager = await storeAdmin.EditManagerAsync(id);

            if (TempData["Message"] != null && TempData["Success"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                ViewBag.Success = (bool)TempData["Success"];
            }

            return View(manager);
        }

        [HttpPost]
        [Route("EditManager")]
        public async Task<ActionResult> EditManager(WebApiManager manager)
        {
            ModelStateResponse response = await storeAdmin.EditManagerAsync(manager);

            if (!response.IsValid)
            {
                foreach (var error in response.ModelSate)
                {
                    ModelState.AddModelError(error.Key, error.Value.First());
                }

                return View(manager);
            }

            return RedirectToAction("GetAllManagers");
        }
        
        [Route("PostManagerImage")]
        public async Task<ActionResult> PostManagerImage(PostImage image)
        {
            if (image.File != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.File.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    float mb = (array.Length / 1024f) / 1024f;

                    if (mb < 1)
                    {
                        WebApiPostImage webApiImage = new WebApiPostImage
                        {
                            Id = image.Id,
                            Image = array
                        };
                        await storeAdmin.PostManagerImageAsync(webApiImage);
                    }
                    else
                    {
                        byte[] arrayScaled = ImageProcessor.To1MB(array);
                        float mbScaled = (arrayScaled.Length / 1024f) / 1024f;

                        if (arrayScaled != null)
                        {
                            WebApiPostImage webApiImage = new WebApiPostImage
                            {
                                Id = image.Id,
                                Image = arrayScaled
                            };
                            await storeAdmin.PostManagerImageAsync(webApiImage);
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [Route("GetManagerImage/{id}")]
        public async Task<ActionResult> GetManagerImage(string id)
        {
            byte[] byteArray = await storeAdmin.GetManagerImageAsync(id);

            return File(byteArray, "image/png");
        }

        [HttpGet]
        [Route("DeleteManager/{id}")]
        public async Task<ActionResult> DeleteManager(string id)
        {
            WebApiManager manager = await storeAdmin.DetailsManagerAsync(id);

            return View(manager);
        }

        [HttpPost]
        [Route("DeleteManager")]
        public async Task<ActionResult> DeleteManager(WebApiManager manager)
        {
            if (await storeAdmin.DeleteManagerAsync(manager.Id))
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
        public async Task<ActionResult> RestoreManager(string id)
        {
            await storeAdmin.RestoreManagerAsync(id);

            return RedirectToAction("GetAllDeletedManagers");
        }

        [HttpGet]
        [Route("GetAllManagerStores/{id}")]
        public async Task<ActionResult> GetAllManagerStores(string id, string sortOrder, string currentFilter, string searchString, int? page)
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

            if (TempData["Message"] != null && TempData["Success"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                ViewBag.Success = (bool)TempData["Success"];
            }

            WebApiManager manager = await storeAdmin.DetailsManagerAsync(id);

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

                ViewBag.ManagerUserName = manager.UserName;
            }
            else
            {
                return RedirectToAction("GetAllManagers");
            }

            List<WebApiManagerStore> stores = await storeAdmin.GetAllManagerStoresAsync(id);

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
        public async Task<ActionResult> AssignStore(string managerId, string storeId)
        {
            Response assignStoreResponse = await storeAdmin.AssignStoreAsync(new WebApiStoreAssign { ManagerId = managerId, StoreId = storeId });

            TempData["Message"] = assignStoreResponse.Message;
            TempData["Success"] = assignStoreResponse.Success;

            return RedirectToAction("GetAllManagerStores", new { id = managerId });
        }

        [HttpGet]
        [Route("UnassignStore/{managerId}/{storeId}")]
        public async Task<ActionResult> UnassignStore(string managerId, string storeId)
        {
            Response storeUnassignResponse = await storeAdmin.UnassignStoreAsync(new WebApiStoreAssign { ManagerId = managerId, StoreId = storeId });

            TempData["Message"] = storeUnassignResponse.Message;
            TempData["Success"] = storeUnassignResponse.Success;

            return RedirectToAction("GetAllManagerStores", new { id = managerId });
        }
    }
}