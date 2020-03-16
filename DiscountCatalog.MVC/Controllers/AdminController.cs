using DiscountCatalog.Common.Models;
using DiscountCatalog.Common.WebApiModels;
using DiscountCatalog.MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiscountCatalog.MVC.Extensions;
using PagedList;
using System.Threading.Tasks;
using DiscountCatalog.Common.CreateModels;
using AutoMapper;
using AbatementHelper.MVC.Mapping;
using DiscountCatalog.MVC.ViewModels;
using DiscountCatalog.MVC.Models.Paging;

namespace DiscountCatalog.MVC.Controllers
{
    [RoutePrefix("Admin")]
    public class AdminController : Controller
    {
        private AdminRepository adminRepository = new AdminRepository();
        private readonly IMapper mapper;

        public AdminController()
        {
            mapper = AutoMapping.Initialise();
        }

        public ActionResult Index()
        {
            return View();
        }

        #region User

        
        //USER

        [Route("CreateUser")]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<ActionResult> CreateUser(UserViewModel user)
        {
            var result = await adminRepository.CreateUser(user);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(user);
            }
            else
            {
                return RedirectToAction("GetAllUsers");
            }
        }

        [Route("GetAllUsers")]
        public async Task<ActionResult> GetAllUsers(string sortOrder, string currentFilter, string searchString, int? page)
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

            PagingEntity<User> users = await adminRepository.GetAllUsers(sortOrder, searchString, pageIndex, pageSize);

            StaticPagedList<User> list = new StaticPagedList<User>(users.Items, users.MetaData.PageNumber, users.MetaData.PageSize, users.MetaData.TotalItemCount);

            return View(list);
        }

        [HttpGet]
        [Route("UserDetails")]
        public async Task<ActionResult> UserDetails(string id)
        {
            User user = await adminRepository.GetUser(id);

            return View(user);
        }

        [HttpGet]
        [Route("EditUser/{id}")]
        public async Task<ActionResult> EditUser(string id)
        {
            User user = await adminRepository.GetUser(id);
            
            return View(user);
        }

        [HttpPost]
        [Route("EditUser")]
        public async Task<ActionResult> EditUser(User user)
        {
            Result result = await adminRepository.EditUser(user);

            if (result.Success)
            {
                return RedirectToAction("GetAllUsers");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(user);
            }
        }

        [HttpGet]
        [Route("DeleteUser/{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            User user = await adminRepository.GetUser(id);
            
            return View(user);
        }

        [HttpPost]
        [Route("DeleteUser")]
        public async Task<ActionResult> DeleteUser(User user)
        {
            Result result = await adminRepository.DeleteUser(user.Id);

            if (result.Success)
            {
                return RedirectToAction("GetAllUsers");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(user);
            }
        }

        [HttpGet]
        [Route("RestoreUser/{id}")]
        public async Task<ActionResult> RestoreUser(string id)
        {
            Result result = await adminRepository.RestoreUser(id);

            if (result.Success)
            {
                return RedirectToAction("GetAllUsers");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return RedirectToAction("GetAllUsers");
            }
        }

        #endregion

        #region StoreAdmin

        //STOREADMIN

        [Route("CreateStoreAdmin")]
        public ActionResult CreateStoreAdmin()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateStoreAdmin")]
        public async Task<ActionResult> CreateStoreAdmin(UserViewModel storeAdmin)
        {
            var result = await adminRepository.CreateStoreAdmin(storeAdmin);

            if (!result.Success)
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(storeAdmin);
            }
            else
            {
                return RedirectToAction("GetAllStoreAdmins");
            }
        }

        [Route("GetAllStoreAdmins")]
        public async Task<ActionResult> GetAllStoreAdmins(string sortOrder, string currentFilter, string searchString, int? page)
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

            PagingEntity<StoreAdmin> storeAdmins = await adminRepository.GetAllStoreAdmins(sortOrder, searchString, pageIndex, pageSize);

            StaticPagedList<StoreAdmin> list = new StaticPagedList<StoreAdmin>(storeAdmins.Items, storeAdmins.MetaData.PageNumber, storeAdmins.MetaData.PageSize, storeAdmins.MetaData.TotalItemCount);

            return View(list);
        }

        [HttpGet]
        [Route("StoreAdminDetails")]
        public async Task<ActionResult> StoreAdminDetails(string id)
        {
            StoreAdmin storeAdmin = await adminRepository.GetStoreAdmin(id);

            return View(storeAdmin);
        }

        [HttpGet]
        [Route("EditStoreAdmin/{id}")]
        public async Task<ActionResult> EditStoreAdmin(string id)
        {
            StoreAdmin storeAdmin = await adminRepository.GetStoreAdmin(id);

            return View(storeAdmin);
        }

        [HttpPost]
        [Route("EditStoreAdmin")]
        public async Task<ActionResult> EditStoreAdmin(StoreAdmin storeAdmin)
        {
            Result result = await adminRepository.EditStoreAdmin(storeAdmin);

            if (result.Success)
            {
                return RedirectToAction("GetAllStoreAdmins");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(storeAdmin);
            }
        }

        [HttpGet]
        [Route("DeleteStoreAdmin/{id}")]
        public async Task<ActionResult> DeleteStoreAdmin(string id)
        {
            StoreAdmin storeAdmin = await adminRepository.GetStoreAdmin(id);

            return View(storeAdmin);
        }

        [HttpPost]
        [Route("DeleteStoreAdmin")]
        public async Task<ActionResult> DeleteStoreAdmin(StoreAdmin storeAdmin)
        {
            Result result = await adminRepository.DeleteStoreAdmin(storeAdmin.Id);

            if (result.Success)
            {
                return RedirectToAction("GetAllStoreAdmins");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return View(storeAdmin);
            }
        }

        [HttpGet]
        [Route("RestoreStoreAdmin/{id}")]
        public async Task<ActionResult> RestoreStoreAdmin(string id)
        {
            Result result = await adminRepository.RestoreStoreAdmin(id);

            if (result.Success)
            {
                return RedirectToAction("GetAllStoreAdmins");
            }
            else
            {
                foreach (var error in result.ModelState)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                return RedirectToAction("GetAllStoreAdmins");
            }
        }

        #endregion

        #region Manager

        [Route("CreateManager")]
        public ActionResult CreateManager()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateManager")]
        public async Task<ActionResult> CreateManager(ManagerViewModel manager) //ovo ne radi - treba select store admin
        {
            var result = await adminRepository.CreateManager(manager);

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

            PagingEntity<Manager> managers = await adminRepository.GetAllManagers(sortOrder, searchString, pageIndex, pageSize);

            StaticPagedList<Manager> list = new StaticPagedList<Manager>(managers.Items, managers.MetaData.PageNumber, managers.MetaData.PageSize, managers.MetaData.TotalItemCount);

            return View(list);
        }

        [HttpGet]
        [Route("ManagerDetails")]
        public async Task<ActionResult> ManagerDetails(string id)
        {
            Manager manager = await adminRepository.GetManager(id);

            return View(manager);
        }

        //[HttpGet]
        //[Route("EditUser/{id}")]
        //public async Task<ActionResult> EditUser(string id)
        //{
        //    User user = await adminRepository.GetUser(id);

        //    return View(user);
        //}

        //[HttpPost]
        //[Route("EditUser")]
        //public async Task<ActionResult> EditUser(User user)
        //{
        //    Result result = await adminRepository.EditUser(user);

        //    if (result.Success)
        //    {
        //        return RedirectToAction("GetAllUsers");
        //    }
        //    else
        //    {
        //        foreach (var error in result.ModelState)
        //        {
        //            ModelState.AddModelError(error.Key, error.Value);
        //        }

        //        return View(user);
        //    }
        //}

        //[HttpGet]
        //[Route("DeleteUser/{id}")]
        //public async Task<ActionResult> DeleteUser(string id)
        //{
        //    User user = await adminRepository.GetUser(id);

        //    return View(user);
        //}

        //[HttpPost]
        //[Route("DeleteUser")]
        //public async Task<ActionResult> DeleteUser(User user)
        //{
        //    Result result = await adminRepository.DeleteUser(user.Id);

        //    if (result.Success)
        //    {
        //        return RedirectToAction("GetAllUsers");
        //    }
        //    else
        //    {
        //        foreach (var error in result.ModelState)
        //        {
        //            ModelState.AddModelError(error.Key, error.Value);
        //        }

        //        return View(user);
        //    }
        //}

        //[HttpGet]
        //[Route("RestoreUser/{id}")]
        //public async Task<ActionResult> RestoreUser(string id)
        //{
        //    Result result = await adminRepository.RestoreUser(id);

        //    if (result.Success)
        //    {
        //        return RedirectToAction("GetAllUsers");
        //    }
        //    else
        //    {
        //        foreach (var error in result.ModelState)
        //        {
        //            ModelState.AddModelError(error.Key, error.Value);
        //        }

        //        return RedirectToAction("GetAllUsers");
        //    }
        //}

        #endregion























        [Route("CreateStore")]
        public ActionResult CreateStore()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateStore")]
        public async Task<ActionResult> CreateStore(StoreViewModel store)
        {
            Result result = await adminRepository.CreateStore(store);

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

        //[Route("CreateManager")]
        //public ActionResult CreateManager()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[Route("CreateManager")]
        //public async Task<ActionResult> CreateManager(CreateManagerModel manager)
        //{
        //    WebApiResult result = await adminRepository.CreateManagerAsync(manager);

        //    if (!result.Success)
        //    {
        //        foreach (var error in result.ModelState)
        //        {
        //            ModelState.AddModelError(error.Key, error.Value);
        //        }

        //        return View(manager);
        //    }

        //    return RedirectToAction("GetAllManagers");
        //}



        //[Route("GetAllStores")]
        //public async Task<ActionResult> GetAllStores(string sortOrder, string currentFilter, string searchString, int? page)
        //{
        //    var stores = new List<WebApiStore>();

        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewBag.CurrentFilter = searchString;

        //    WebApiListOfStoresResult result = await adminRepository.GetAllStoresAsync();

        //    if (result.Success)
        //    {
        //        stores = result.Stores;
        //    }
        //    else
        //    {
        //        foreach (var error in result.ModelState)
        //        {
        //            ModelState.AddModelError(error.Key, error.Value);
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        stores = stores.Where(u => u.StoreName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
        //    }

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            stores = stores.OrderByDescending(u => u.StoreName).ToList();
        //            break;
        //        default:
        //            stores = stores.OrderBy(u => u.StoreName).ToList();
        //            break;
        //    }

        //    int pageSize = 12;
        //    int pageNumber = (page ?? 1);

        //    return View(stores.ToPagedList(pageNumber, pageSize));
        //}

        ////get all managers



        //[HttpGet]
        //[Route("EditStore/{id}")]
        //public async Task<ActionResult> EditStore(string id)
        //{
        //    WebApiStore store = new WebApiStore();

        //    WebApiStoreResult result = await adminRepository.EditStoreAsync(id);

        //    if (result.Success)
        //    {
        //        store = result.Store;
        //    }
        //    else
        //    {
        //        foreach (var error in result.ModelState)
        //        {
        //            ModelState.AddModelError(error.Key, error.Value);
        //        }
        //    }

        //    return View(store);
        //}



        //[HttpPost]
        //[Route("EditStore")]
        //public async Task<ActionResult> EditStore(WebApiStore store)
        //{
        //    WebApiResult result = await adminRepository.EditStoreAsync(store);

        //    if (result.Success)
        //    {
        //        return RedirectToAction("GetAllStores");
        //    }
        //    else
        //    {
        //        foreach (var error in result.ModelState)
        //        {
        //            ModelState.AddModelError(error.Key, error.Value);
        //        }

        //        return View(store);
        //    }
        //}

        //[HttpGet]
        //[Route("UserDetails/{id}")]
        //public async Task<ActionResult> UserDetails(string id)
        //{
        //    WebApiUser user = new WebApiUser();

        //    WebApiUserResult result = await adminRepository.UserDetailsAsync(id);

        //    if (result.Success)
        //    {
        //        user = result.User;
        //    }
        //    else
        //    {
        //        foreach (var error in result.ModelState)
        //        {
        //            ModelState.AddModelError(error.Key, error.Value);
        //        }
        //    }

        //    return View(user);
        //}

        //[HttpGet]
        //[Route("StoreDetails/{id}")]
        //public async Task<ActionResult> StoreDetails(string id)
        //{
        //    WebApiStore store = new WebApiStore();

        //    WebApiStoreResult result = await adminRepository.StoreDetailsAsync(id);

        //    if (result.Success)
        //    {
        //        store = result.Store;
        //    }
        //    else
        //    {
        //        foreach (var error in result.ModelState)
        //        {
        //            ModelState.AddModelError(error.Key, error.Value);
        //        }
        //    }

        //    return View(store);
        //}



        //[HttpGet]
        //[Route("DeleteStore/{id}")]
        //public async Task<ActionResult> DeleteStore(string id)
        //{
        //    WebApiStore store = new WebApiStore();

        //    WebApiStoreResult result = await adminRepository.StoreDetailsAsync(id);

        //    if (result.Success)
        //    {
        //        store = result.Store;
        //    }
        //    else
        //    {
        //        foreach (var error in result.ModelState)
        //        {
        //            ModelState.AddModelError(error.Key, error.Value);
        //        }
        //    }

        //    return View(store);
        //}



        //[HttpPost]
        //[Route("DeleteStore")]
        //public async Task<ActionResult> DeleteStore(WebApiStore store)
        //{
        //    WebApiResult result = await adminRepository.DeleteStoreAsync(store);

        //    if (result.Success)
        //    {
        //        return RedirectToAction("GetAllStores");
        //    }
        //    else
        //    {
        //        foreach (var error in result.ModelState)
        //        {
        //            ModelState.AddModelError(error.Key, error.Value);
        //        }

        //        return View(store);
        //    }
        //}



        //[HttpGet]
        //[Route("RestoreStore/{id}")]
        //public async Task<ActionResult> RestoreStore(string id)
        //{
        //    WebApiResult result = await adminRepository.RestoreStoreAsync(id);

        //    if (result.Success)
        //    {
        //        return RedirectToAction("GetAllStores");
        //    }
        //    else
        //    {
        //        foreach (var error in result.ModelState)
        //        {
        //            ModelState.AddModelError(error.Key, error.Value);
        //        }

        //        return RedirectToAction("GetAllStores");
        //    }
        //}




















        //[Route("GetAllUsers")]
        //public async Task<ActionResult> GetAllUsers(string sortOrder, string currentFilter, string searchString, int? page)
        //{
        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewBag.CurrentFilter = searchString;

        //    List<User> users = await adminRepository.GetAllUsers();

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        users = users.Where(u => u.UserName.Contains(searchString, StringComparer.OrdinalIgnoreCase)).ToList();
        //    }

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            users = users.OrderByDescending(u => u.UserName).ToList();
        //            break;
        //        default:
        //            users = users.OrderBy(u => u.UserName).ToList();
        //            break;
        //    }

        //    int pageSize = 12;
        //    int pageNumber = (page ?? 1);

        //    return View(users.ToPagedList(pageNumber, pageSize));
        //}
    }
}